using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Collections;
using wc = System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class CoeMod_IndividualStudentResult : System.Web.UI.Page
{
    #region variable declaration
    string usercode = string.Empty;
    static int selectedMode = 0;
    static string collegeCode = string.Empty;
    string stud_name = string.Empty;
    string appno = string.Empty;
    string curr_Sem = string.Empty;
    string rollNo = string.Empty;
    string degCode = string.Empty;
    string batchYear = string.Empty;
    string sec = string.Empty;

    string sem = "";
    int perdayhrs;
    double hours_present = 0;
    double hours_absent = 0;
    double hours_od = 0;
    double hours_total = 0;
    double hours_leave = 0;
    double hours_conduct = 0;
    double hours_pres = 0;
    string semprint = string.Empty;
    int fromDay, fromMonth, fromYear, toDay, toMonth, toYear, fromcal, tocal;
    int perDayHrs = 0, firstHalfHrs = 0, secondHalfHrs = 0, minpresI = 0, minpresII = 0;
    int demfcal, demtcal, cal_from_date = 0, cal_to_date = 0;
    string monthcal;
    DateTime per_from_date = new DateTime();
    DateTime per_to_date = new DateTime();
    DateTime dumm_from_date = new DateTime();
    int ObtValue = 0, per_abshrs = 0, njhr = 0, per_perhrs = 0, tot_ondu = 0, per_ondu = 0, tot_per_hrs = 0;
    int dum_unmark = 0;
    int per_holidate_mng = 0, per_holidate_evng = 0, per_workingdays1 = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, notconsider_value = 0, conduct_hour_new = 0, absent_hours = 0;
    int njdate_mng = 0, njdate_evng = 0, mmyycount = 0, moncount = 0;
    double Present = 0, leave_point = 0, Leave = 0;
    double Absent = 0, absent_point = 0, Onduty = 0;
    int next = 0, count = 0;
    double per_njdate = 0, pre_present_date = 0, per_absent_date = 0, pre_ondu_date = 0;
    double per_holidate = 0, njdate = 0, workingdays = 0;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    TimeSpan ts;
    string diff_date = string.Empty;
    double dif_date1 = 0;
    string value_holi_status = "", split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string[] split_holiday_status;
    string[] split1 = new string[3];
    double dif_date = 0;
    int leave_pointer = 0, absent_pointer = 0;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string date = "", value = "", tempvalue = string.Empty;
    int per_leave = 0, per_hhday = 0, unmark = 0;
    int per_tot_ondu = 0, per_per_hrs = 0;
    double pre_leave_date = 0, per_workingdays = 0;
    double per_tage_date = 0, per_con_hrs = 0, per_tage_hrs = 0, per_dum_unmark = 0;
    double dum_tage_date = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    double per_leave_fals = 0;

    string Att_mark = string.Empty;
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }



            if (!IsPostBack)
            {
                bindcollege();
                load_ddlrollno();
                bindsem();

            }
        }
        catch (Exception ex)
        {
        }
    }

    #region College
    public void bindcollege()
    {
        ddlcollege.Items.Clear();

        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
            collegeCode = Convert.ToString(ddlcollege.SelectedValue);
        }

    }

    protected void ddlcollege_indexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
            collegeCode = Convert.ToString(ddlcollege.SelectedValue);

    }
    #endregion

    #region semester
    //public void bindsem()
    //{
    //    try
    //    {

    //        DataSet dsSem = new DataSet();
    //        chklSem.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + collegeCode + " and degree_code=" + Convert.ToString(degCode) + " and batch_year=" + Convert.ToString(batchYear) + " ";
    //        dsSem = d2.select_method_wo_parameter(sqluery, "text");
    //        if (dsSem.Tables[0].Rows.Count > 0)
    //        {
    //            first_year = Convert.ToBoolean(dsSem.Tables[0].Rows[0]["first_year_nonsemester"]);
    //            duration = Convert.ToInt16(dsSem.Tables[0].Rows[0]["ndurations"]);
    //            for (i = 1; i <= duration; i++)
    //            {
    //                if (first_year == false)
    //                {
    //                    chklSem.Items.Add(i.ToString());
    //                }
    //                else if (first_year == true && i != 2)
    //                {
    //                    chklSem.Items.Add(i.ToString());
    //                }
    //            }
    //        }
    //        else
    //        {
    //            sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + Convert.ToString(degCode) + "  and college_code=" + Convert.ToString(collegeCode) + "";
    //            chklSem.Items.Clear();
    //            dsSem = d2.select_method_wo_parameter(sqluery, "text");
    //            if (dsSem.Tables[0].Rows.Count > 0)
    //            {
    //                first_year = Convert.ToBoolean(dsSem.Tables[0].Rows[0]["first_year_nonsemester"]);
    //                duration = Convert.ToInt16(dsSem.Tables[0].Rows[0]["duration"]);
    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {
    //                        chklSem.Items.Add(i.ToString());
    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {
    //                        chklSem.Items.Add(i.ToString());
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void chkSem_checkedchanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chkSem, chklSem, txt_sem, lblsemester.Text, "--Select--");
    }

    protected void chklSem_selectedchanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkSem, chklSem, txt_sem, lblsemester.Text, "--Select--");
    }
    #endregion

    public void bindsem()
    {
        try
        {
            DataSet dsSem = new DataSet();
          // string sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + Convert.ToString(degCode) + "  and college_code=" + Convert.ToString(collegeCode) + "";
            string sqluery = "select distinct current_semester from Registration order by Current_Semester";
            //            chklSem.Items.Clear();
            dsSem = d2.select_method_wo_parameter(sqluery, "text");
            if (dsSem.Tables[0].Rows.Count > 0)
            {

                chklSem.DataSource = dsSem;
                chklSem.DataTextField = "current_semester";
                chklSem.DataValueField = "current_semester";
                chklSem.DataBind();
                checkBoxListselectOrDeselect(chklSem, true);
                CallCheckboxListChange(chkSem, chklSem, txt_sem, lblsemester.Text, "--Select--");
            }
        }
        catch
        {

        }
    }
    //added by Mullai
    public void bindsemester()
    {
        chkSem.Checked = false;
        chklSem.Items.Clear();
        int sem = Convert.ToInt32(semprint);
        for (int i = 1; sem >= i; i++)
        {
            chklSem.Items.Add(i.ToString());
            txt_sem.Text="--Select--";
        }
    }
    //
    public void load_ddlrollno()
    {
        try
        {
            ListItem lstItem1 = new ListItem("Roll No", "0");
            ListItem lstItem2 = new ListItem("Reg No", "1");
            ListItem lstItem3 = new ListItem("Admission No", "2");
            ListItem lstItem4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            ddlrollno.Items.Clear();
            string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddlrollno.Items.Add(lstItem1);
            }


            insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddlrollno.Items.Add(lstItem2);
            }

            insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddlrollno.Items.Add(lstItem3);
            }

            insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                ddlrollno.Items.Add(lstItem4);
            }

            if (ddlrollno.Items.Count == 0)
            {
                ddlrollno.Items.Add(lstItem1);
            }
            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    selectedMode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    selectedMode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    selectedMode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    selectedMode = 3;
                    break;
            }

        }
        catch (Exception ex) { }
    }

    protected void ddlrollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_rollno.Text = "";
        switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
        {
            case 0:
                txt_rollno.Attributes.Add("Placeholder", "Roll No");
                selectedMode = 0;
                break;
            case 1:
                txt_rollno.Attributes.Add("Placeholder", "Reg No");
                selectedMode = 1;
                break;
            case 2:
                txt_rollno.Attributes.Add("Placeholder", "Admission No");
                selectedMode = 2;
                break;
            case 3:
                txt_rollno.Attributes.Add("Placeholder", "App No");
                selectedMode = 3;
                break;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        
        {
            string query = "";
            WebService ws = new WebService();

            if (selectedMode == 0)
            {
                query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' and r.college_code='" + collegeCode + "'  order by  Roll_No asc";
            }
            else if (selectedMode == 1)
            {
                query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + collegeCode + "'  order by  Reg_No asc";
            }
            else if (selectedMode == 2)
            {
                query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + collegeCode + "'  order by  Roll_admit asc";
            }
            else if (selectedMode == 3)
            {
                query = "select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + collegeCode + "'  order by  app_formno asc";

            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetStudentDetails();
            //bindsem();
            bindsemester();
        }
        catch { }
    }

    public void GetStudentDetails()
    {
        try
        {
            string str = string.Empty;
            string txtValue = Convert.ToString(txt_rollno.Text);
            string rolltype = Convert.ToString(ddlrollno.SelectedItem).Trim();
            switch (rolltype)
            {
                case "Roll No":
                    str = " and r.Roll_No='" + txtValue + "'";
                    break;
                case "Reg No":
                    str = " and r.reg_no='" + txtValue + "'";
                    break;
                case "Admission No":
                    str = " and r.Roll_Admit='" + txtValue + "'";
                    break;
                case "App No":
                    str = " and a.app_no='" + txtValue + "'";
                    break;
            }


            string qry = "select r.Roll_No,r.reg_no,r.Roll_Admit,a.app_no,r.degree_code,r.Batch_Year,r.Current_Semester,r.stud_name,r.Sections from applyn a,Registration r where a.app_no=r.App_No " + str; //and a.app_formno=r.Roll_No and r.degree_code=a.degree_code and a.batch_year=r.Batch_Year
            DataSet dsStudDetails = d2.select_method_wo_parameter(qry, "Text");

            if (dsStudDetails.Tables.Count > 0 && dsStudDetails.Tables[0].Rows.Count > 0)
            {
                stud_name = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["stud_name"]).Trim();
                rollNo = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["roll_no"]).Trim();
                appno = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["app_no"]).Trim();
                curr_Sem = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["current_semester"]).Trim();
                degCode = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["degree_code"]).Trim();
                batchYear = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["batch_year"]).Trim();
                sec = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["Sections"]).Trim();
                semprint = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["current_semester"]).Trim();
            }

        }
        catch (Exception ex) { }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GetStudentDetails();
            collegeCode = Convert.ToString(ddlcollege.SelectedValue);
            #region spreadsheet design
            spreadDet.Visible = true;
            spreadDet.Sheets[0].PageSize = 200;
            spreadDet.Rows.Default.Height = 20;
            spreadDet.ColumnHeader.Visible = false;
            spreadDet.CommandBar.Visible = false;
            spreadDet.RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 15;
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            spreadDet.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
            spreadDet.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Medium;
            spreadDet.ActiveSheetView.Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            spreadDet.ActiveSheetView.Rows.Default.VerticalAlign = VerticalAlign.Middle;
            spreadDet.ActiveSheetView.Columns.Default.Font.Name = "Book Antiqua";
            spreadDet.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Medium;
            spreadDet.ActiveSheetView.Columns.Default.HorizontalAlign = HorizontalAlign.Center;
            spreadDet.ActiveSheetView.Columns.Default.VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].DefaultStyle.VerticalAlign = VerticalAlign.Middle;
            spreadDet.Columns.Default.Width = 70;
            spreadDet.Columns[1].Width = 270;
            spreadDet.Columns[1].HorizontalAlign = HorizontalAlign.Left;

            #endregion

            string str_sec = "";
            string syllYear = "-1";
            string obtained_mark = "-1";
            int spanrow = 0, spancolumn = 0;
            int span_row = 0;
            int column = 0;
            string historyDegCode = "";
            Boolean day_flag_str = false;
            Boolean hr_flag_str = false;

            string Master = string.Empty;
            Master = "select * from Master_Settings where usercode in(select user_code from usermaster where user_id='admin') and (settings='Day Wise' or settings='Hour Wise')";
            readcon.Close();
            readcon.Open();
            SqlDataReader mtrdr;
            SqlCommand mtcmd = new SqlCommand(Master, readcon);
            mtrdr = mtcmd.ExecuteReader();
            if (mtrdr.HasRows)
            {
                while (mtrdr.Read())
                {
                    if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                    {
                        day_flag_str = true;
                    }
                    if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                    {
                        hr_flag_str = true;
                    }
                }
            }

            ArrayList criteria = new ArrayList();
            string sql = string.Empty;

            if (sec != "")
            {
                str_sec = " and exam_type.sections='" + sec + "'";

            }

            syllYear = d2.GetFunction("select isnull(syllabus_year,-1) Syllabus_Year from syllabus_master where degree_code=" + degCode + " and semester =" + curr_Sem + " and batch_year='" + batchYear + "'");
            if (syllYear == "-1")
            {

            }
            // bool status = feeStatusSettings();
            for (int i = 0; i < chklSem.Items.Count; i++)
            {
                if (chklSem.Items[i].Selected == true)
                {
                    sem = Convert.ToString(Convert.ToInt32(i + 1));


                    double creditPoint = 0, gradePoint = 0, gpa = 0, cgpa = 0;
                    double mark = 0, total = 0, percentage = 0;
                    int sub = 0;

                    #region  display the previous mark report even after department transfer

                    string deptflagqry = "select * from StudentRegisterHistory where app_no in (select App_no from registration where roll_no='" + rollNo + "')";
                    DataSet deptflagDataSet = d2.select_method_wo_parameter(deptflagqry, "Text");
                    bool dept_TransferFlag = false;

                    if (deptflagDataSet.Tables[0].Rows.Count > 0)
                    {
                        dept_TransferFlag = true;
                    }
                    string subDetailsQry = "";
                    
                    if (dept_TransferFlag)
                    {
                        for (int ii = 0; ii < deptflagDataSet.Tables[0].Rows.Count; ii++)
                        {
                            if (Convert.ToString(deptflagDataSet.Tables[0].Rows[ii]["degreecode"]) != degCode.ToString())
                            {
                                string previousexistqry = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code  and subject.subject_no =subjectchooser.subject_no and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.degree_code='" + Convert.ToString(deptflagDataSet.Tables[0].Rows[ii]["degreecode"]).Trim() + "' and roll_no = '" + rollNo + "' order by subject.subject_no";
                                DataSet previousexistDataSet = d2.select_method_wo_parameter(previousexistqry, "Text");

                                if (previousexistDataSet.Tables.Count > 0 && previousexistDataSet.Tables[0].Rows.Count > 0)
                                {
                                    subDetailsQry = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code  and subject.subject_no =subjectchooser.subject_no and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.degree_code='" + Convert.ToString(deptflagDataSet.Tables[0].Rows[ii]["degreecode"]).Trim() + "' and roll_no = '" + rollNo + "' order by subject.subject_no";
                                    historyDegCode = Convert.ToString(deptflagDataSet.Tables[0].Rows[ii]["degreecode"]).Trim();
                                    break;
                                }
                                else
                                {
                                    subDetailsQry = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code  and subject.subject_no =subjectchooser.subject_no and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.degree_code='" + degCode.ToString() + "' and roll_no = '" + rollNo + "' order by subject.subject_no";
                                }
                                
                            }
                            else
                            {
                                subDetailsQry = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code  and subject.subject_no =subjectchooser.subject_no and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.degree_code='" + degCode.ToString() + "' and roll_no = '" + rollNo + "' order by subject.subject_no";
                            }
                        }

                    }
                    else
                    {
                        subDetailsQry = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code  and subject.subject_no =subjectchooser.subject_no and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.degree_code='" + degCode.ToString() + "' and roll_no = '" + rollNo + "' order by subject.subject_no";
                    }

                    #endregion

                    DataSet subDetailsDataSet = d2.select_method_wo_parameter(subDetailsQry, "Text");
                    if (subDetailsDataSet.Tables.Count > 0 && subDetailsDataSet.Tables[0].Rows.Count > 0)
                    {
                        creditPoint = 0;
                        gradePoint = 0;
                        gpa = 0;
                        cgpa = 0;

                        spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 1;
                        span_row = spreadDet.Sheets[0].RowCount - 1;
                        spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 2;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].BackColor = Color.LightCyan;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightCyan;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Bold = true;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, 0, 2, 1);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, 0].Text = "S.No";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, 1, 2, 1);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, 1].Text = "Subject";
                        spanrow = spreadDet.Sheets[0].RowCount - 3;
                        int col = 2;
                        string criteriaInfoQry = "select c.criteria,c.criteria_no  from CriteriaForInternal as c where c.syll_code in(select syll_code from subject where subject_no in(select distinct subject.subject_no from subject,sub_sem,syllabus_master,exam_type where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code='" + degCode.ToString() + "' and syllabus_master.semester='" + sem.ToString() + "' and syllabus_master.batch_year='" + batchYear + "'" + str_sec + " )) and c.criteria_no is not null order by c.criteria";

                  
                        DataSet criteriaInfoDataSet = d2.select_method_wo_parameter(criteriaInfoQry, "Text");
                        string gradesett = "select * from gradesettings where college_code in(Select college_code from registration where roll_no='" + rollNo + "')";


                        DataSet gradesettings = d2.select_method_wo_parameter(gradesett, "Text");
                        //if (criteriaInfoDataSet.Tables.Count > 0 && criteriaInfoDataSet.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int j = 0; j < criteriaInfoDataSet.Tables[0].Rows.Count; j++)
                        //    {
                        //        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, col, 2, 1);
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col].Text = Convert.ToString(criteriaInfoDataSet.Tables[0].Rows[j]["criteria"]);
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col].Tag = Convert.ToString(criteriaInfoDataSet.Tables[0].Rows[j]["criteria_no"]);
                        //        criteria.Add(Convert.ToString(criteriaInfoDataSet.Tables[0].Rows[j]["criteria_no"]));
                        //        col = col + 1;
                        //    }

                        //}
                        if (col > column)
                            column = col;
                        spancolumn = col;
                        int row = spreadDet.Sheets[0].RowCount - 1;

                        string excode = string.Empty;
                        int temp = 0;
                        byte gradeFlagNew = 3;

                        string exmDetailsQry = "Select top 1 isnull(Exam_Code,-1)exam_code,exam_month,exam_year from Exam_Details where Degree_Code = '" + degCode.ToString() + "' and Current_Semester = '" + sem + "' and Batch_Year = '" + batchYear.ToString() + "' order by exam_year desc";

                        DataSet exmDetailsDataSet = d2.select_method_wo_parameter(exmDetailsQry, "Text");

                        if (exmDetailsDataSet.Tables.Count > 0 && exmDetailsDataSet.Tables[0].Rows.Count > 0)
                        {
                            excode = Convert.ToString(exmDetailsDataSet.Tables[0].Rows[0]["exam_code"]);

                            string gradeFlagQry = "Select grade_flag from grademaster where degree_code='" + degCode.ToString() + "' and exam_month= '" + Convert.ToString(exmDetailsDataSet.Tables[0].Rows[0]["exam_month"]) + "' and exam_year='" + Convert.ToString(exmDetailsDataSet.Tables[0].Rows[0]["exam_year"]) + "' and batch_year='" + batchYear.ToString() + "'";

                            DataSet gradeFlagDataSet = d2.select_method_wo_parameter(gradeFlagQry, "Text");
                            if (gradeFlagDataSet.Tables.Count > 0 && gradeFlagDataSet.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToString(gradeFlagDataSet.Tables[0].Rows[0]["grade_flag"]) == "3")
                                {
                                    gradeFlagNew = 3;

                                    sql = "select syllabus_master.semester,type,subject.subject_code,subject.subject_name,(select  str(exam_year) from exam_details where exam_code=me.exam_code) as mon_year,case when markorgrade=0 then max_int_marks else null end,case when markorgrade=0 then min_int_marks else null end,";
                                    sql = sql + "case when markorgrade=0 then internal_mark else null end,case when markorgrade=0 then max_ext_marks else null end,case when markorgrade=0 then min_ext_marks else null end,case when markorgrade=0 then external_mark else null end,case when markorgrade=0 then maxtotal else null end,";
                                    sql = sql + "case when markorgrade=0 then mintotal else null end,case when markorgrade=0 then total else null end, case when markorgrade=0 then result else remarks end ,isnull(markorgrade,0), roll_no,subject.subtype_no,subject.subject_no,me.exam_code,subject.credit_points,me.attempts as attempt,result  from mark_entry as me,subject,sub_sem,syllabus_master where sub_sem.subtype_no=subject.subtype_no ";
                                    sql = sql + " and total <= maxtotal and internal_mark <= max_int_marks and external_mark <= max_ext_marks   ";//and result='pass'
                                    sql = sql + " and roll_no = '" + rollNo + "' and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no";
                                    temp = 3;
                                }
                                else if (Convert.ToString(gradeFlagDataSet.Tables[0].Rows[0]["grade_flag"]) == "2")
                                {
                                    gradeFlagNew = 2;
                                    sql = "select syllabus_master.semester,type,subject.subject_code,subject.subject_name,(select  str(exam_year)+'-'+convert(varchar(3),DateAdd(month,Exam_Month,-1)) from exam_details where exam_code=me.exam_code) as mon_year,case when markorgrade=0 then max_int_marks else null end,case when markorgrade=0 then min_int_marks else null end,";
                                    sql = sql + "case when markorgrade=0 then max_ext_marks else null end,case when markorgrade=0 then min_ext_marks else null end,case when markorgrade=0 then maxtotal else null end,";
                                    sql = sql + "case when markorgrade=0 then mintotal else null end,case when markorgrade=0 then total else null end, case when markorgrade=0 then result else remarks end ,isnull(markorgrade,0), roll_no,subject.subtype_no,subject.subject_no,me.exam_code,subject.credit_points,me.grade,me.attempts as attempt,isnull(me.grade,'')grade,result  from mark_entry as me,subject,sub_sem,syllabus_master where sub_sem.subtype_no=subject.subtype_no ";
                                    //sql = sql + "and result='pass' ";
                                    sql = sql + " and roll_no = '" + rollNo + "' and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no";
                                    temp = 2;
                                }
                                else if (Convert.ToString(gradeFlagDataSet.Tables[0].Rows[0]["grade_flag"]) == "1")
                                {
                                    gradeFlagNew = 1;
                                    sql = "select syllabus_master.semester,type,subject.subject_code,subject.subject_name,(select  str(exam_year) from exam_details where exam_code=me.exam_code) as mon_year,case when markorgrade=0 then max_int_marks else null end,case when markorgrade=0 then min_int_marks else null end,";
                                    sql = sql + "case when markorgrade=0 then internal_mark else null end,case when markorgrade=0 then max_ext_marks else null end,case when markorgrade=0 then min_ext_marks else null end,case when markorgrade=0 then external_mark else null end,case when markorgrade=0 then maxtotal else null end,";
                                    sql = sql + "case when markorgrade=0 then mintotal else null end,case when markorgrade=0 then total else null end, case when markorgrade=0 then result else remarks end ,isnull(markorgrade,0), roll_no,subject.subtype_no,subject.subject_no,me.exam_code,subject.credit_points,me.grade,me.attempts as attempt,result  from mark_entry as me,subject,sub_sem,syllabus_master where sub_sem.subtype_no=subject.subtype_no ";

                                    sql = sql + " and roll_no = '" + rollNo + "' and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no ";
                                    temp = 1;
                                }
                            }
                            else
                            {
                                sql = "select syllabus_master.semester,type,subject.subject_code,subject.subject_name,(select  str(exam_year) from exam_details where exam_code=me.exam_code) as mon_year,case when markorgrade=0 then max_int_marks else null end,case when markorgrade=0 then min_int_marks else null end,";
                                sql = sql + "case when markorgrade=0 then internal_mark else null end,case when markorgrade=0 then max_ext_marks else null end,case when markorgrade=0 then min_ext_marks else null end,case when markorgrade=0 then external_mark else null end,case when markorgrade=0 then maxtotal else null end,";
                                sql = sql + "case when markorgrade=0 then mintotal else null end,case when markorgrade=0 then total else null end, case when markorgrade=0 then result else remarks end ,isnull(markorgrade,0), roll_no,subject.subtype_no,subject.subject_no,me.exam_code,subject.credit_points,me.attempts as attempt,result  from mark_entry as me,subject,sub_sem,syllabus_master where sub_sem.subtype_no=subject.subtype_no ";
                                sql = sql + " and total <= maxtotal and internal_mark <= max_int_marks and external_mark <= max_ext_marks   ";//and result='pass'
                                sql = sql + " and roll_no = '" + rollNo + "' and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no ";
                            }

                            if (gradeFlagNew == 2)
                            {
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, col, 1, 4);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col].Text = "University Exams";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = "Attempts";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = "INT";
                                spreadDet.Sheets[0].Columns[col + 1].Visible = false;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = "Grade";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = "Result";
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, col + 4, 2, 1);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col + 4].Text = "Year of Passing";
                                spancolumn = col + 5;
                            }
                            else
                            {
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, col, 1, 3);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col].Text = "University Exams";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = "Attempts";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = "INT";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = "* Marks";
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 2, col + 3, 2, 1);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 2, col + 3].Text = "Year of Passing";
                                spancolumn = col + 4;
                            }
                        }

                        if (excode != "" || col > 2)
                        {

                            for (int m = 0; m < subDetailsDataSet.Tables[0].Rows.Count; m++)
                            {
                                spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 1;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = (sub + 1).ToString();
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subDetailsDataSet.Tables[0].Rows[m]["subject_name"]);
                                for (int loop_col = 2; loop_col < col; loop_col++)
                                {

                                    string examcodeqry = "select exam_code from exam_type where  criteria_no='" + criteria[loop_col - 2] + "' and subject_no = '" + Convert.ToString(subDetailsDataSet.Tables[0].Rows[m]["subject_no"]) + "'" + str_sec + " and batch_year='" + batchYear.ToString() + "'";
                                    string exam_code = GetFunction(examcodeqry);
                                    if (exam_code != "")
                                    {
                                        string resultQry = "select isnull(marks_obtained,'') marks_obtained from result,registration where registration.roll_no=result.roll_no and delflag=0 and cc=0 and exam_flag<>'DEBAR' and result.roll_no='" + rollNo.ToString() + "' and result.exam_code='" + exam_code + "'";

                                        DataSet resultDataSet = d2.select_method_wo_parameter(resultQry, "Text");


                                        if (resultDataSet.Tables.Count > 0 && resultDataSet.Tables[0].Rows.Count > 0)
                                            obtained_mark = Convert.ToString(resultDataSet.Tables[0].Rows[0]["marks_obtained"]).Trim();

                                        double obm = 0;
                                        if (double.TryParse(obtained_mark, out obm))
                                        {
                                            if (obm < 0)
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, loop_col].Text = Attmark(obtained_mark);
                                            else
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, loop_col].Text = obtained_mark;
                                        }
                                        else if (obtained_mark == "")
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, loop_col].Text = " - ";
                                        obtained_mark = "";
                                    }
                                }
                                if (excode != "")
                                {
                                    cmd.CommandText = sql + " and subject.subject_no='" + Convert.ToString(subDetailsDataSet.Tables[0].Rows[m]["subject_no"]) + "' ";
                                    cmd.Connection = con4;
                                    con4.Open();
                                    SqlDataReader rmark = cmd.ExecuteReader();
                                    string attempts = string.Empty;
                                    string newSA = sql + " and subject.subject_no='" + Convert.ToString(subDetailsDataSet.Tables[0].Rows[m]["subject_no"]) + "' ";
                                    DataSet dsSA = d2.select_method_wo_parameter(newSA, "text");
                                    string strSA = string.Empty;
                                    if (dsSA.Tables.Count > 0 && dsSA.Tables[0].Rows.Count > 0)
                                    {
                                        dsSA.Tables[0].DefaultView.RowFilter = " subject_no='" + dsSA.Tables[0].Rows[0]["subject_no"] + "'  and result='pass'";
                                        DataView dv1 = dsSA.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                            strSA = Convert.ToString(dv1.Table.Rows[0]["result"]);
                                        else
                                            strSA = Convert.ToString(dsSA.Tables[0].Rows[0]["result"]);
                                    }
                                    if (strSA.ToLower() == "pass")
                                    {
                                        if (rmark.Read())
                                        {
                                            string attem = Convert.ToString(rmark["attempt"]);
                                            if (!string.IsNullOrEmpty(attem))
                                            {
                                                if (attem.Trim() == "0")
                                                {
                                                    attempts = "1";
                                                }
                                                else
                                                {
                                                    attempts = rmark["attempt"].ToString();
                                                }
                                            }
                                            if (temp == 3)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = attempts;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = (gradeFlagNew == 2) ? "" : rmark.GetValue(7).ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = rmark.GetValue(10).ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = rmark["mon_year"].ToString();
                                                if (Double.TryParse(rmark.GetValue(10).ToString(), out mark))
                                                    total = total + mark;
                                            }
                                            else if (temp == 2)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = attempts;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = rmark.GetValue(6).ToString();
                                                gradesettings.Tables[0].DefaultView.RowFilter = "ActualGrade='" + rmark["grade"].ToString() + "'";
                                                DataView dvholiday = gradesettings.Tables[0].DefaultView;
                                                if (dvholiday.Count > 0)
                                                {
                                                    if (Convert.ToString(dvholiday[0]["grade"]) != "")
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = Convert.ToString(dvholiday[0]["grade"]);
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = Convert.ToString(dvholiday[0]["Result"]);
                                                    }
                                                    else
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = rmark["grade"].ToString();
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = rmark.GetValue(12).ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = rmark["grade"].ToString();
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = rmark.GetValue(12).ToString();
                                                }
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 4].Text = rmark["mon_year"].ToString();
                                            }
                                            else if (temp == 1)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = attempts;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = (gradeFlagNew == 2) ? "" : rmark.GetValue(7).ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = rmark["grade"].ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = rmark["mon_year"].ToString();
                                            }
                                            else
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = attempts;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 1].Text = rmark.GetValue(7).ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = rmark.GetValue(10).ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = rmark["mon_year"].ToString();
                                                if (Double.TryParse(rmark.GetValue(10).ToString(), out mark))
                                                    total = total + mark;
                                            }
                                        }

                                        else
                                        {
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = "RA";
                                        }
                                    }
                                    else if (dsSA.Tables.Count > 0 && dsSA.Tables[0].Rows.Count > 0)
                                    {
                                        
                                        dsSA.Tables[0].DefaultView.RowFilter = "subject_no='" + dsSA.Tables[0].Rows[0]["subject_no"] + "'";
                                        DataView dv = dsSA.Tables[0].DefaultView;
                                        dv.Sort = "exam_code desc";
                                         gradesettings.Tables[0].DefaultView.RowFilter = "ActualGrade='" + Convert.ToString(dv.Table.Rows[0]["grade"]) + "'";
                                                DataView dvholiday = gradesettings.Tables[0].DefaultView;
                                                if (dvholiday.Count > 0)
                                                {
                                                    if (Convert.ToString(dvholiday[0]["grade"]) != "")
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = Convert.ToString(dvholiday[0]["grade"]);
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 3].Text = Convert.ToString(dvholiday[0]["Result"]);
                                                    }
                                                }
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col + 2].Text = "RA";
                                    }
                                    rmark.Close();
                                    con4.Close();
                                }
                                sub = sub + 1;

                            }
                            if (excode != "")
                            {
                                if (temp == 1 || temp == 2)
                                {
                                    string markDetailsQry = "Select cp,(cp*(select distinct top 1 g.credit_points from grade_master as g,Mark_Entry as m where m.subject_no=subject.subject_no and m.grade=g.mark_grade and m.roll_no='" + rollNo.ToString() + "' and g.degree_code='" + degCode.ToString() + "' and m.exam_code='" + excode + "' and g.batch_year='" + batchYear.ToString() + "' and g.college_code='" + collegeCode.ToString() + "')) as gp from  Mark_Entry as m,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code  and m.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = '" + excode + "' and roll_no='" + rollNo.ToString() + "' and attempts='1' and result='pass'";

                                   // string markDetailsQry = "Select cp,(cp*(select distinct top 1 g.credit_points from grade_master as g,Mark_Entry as m where m.subject_no=subject.subject_no and m.grade=g.mark_grade and m.roll_no='" + rollNo.ToString() + "' and g.degree_code='" + degCode.ToString() + "' and  g.batch_year='" + batchYear.ToString() + "' and g.college_code='" + collegeCode.ToString() + "')) as gp from  Mark_Entry as m,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code  and m.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Subject.Subject_No in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and roll_no='" + rollNo.ToString() + "' and exam_code in(SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degCode.ToString() + "' AND Batch_Year ='" + batchYear.ToString() + "' AND current_semester='" + sem + "')) and roll_no='" + rollNo.ToString() + "'  and result='pass'";
                                    
                                    DataSet markDetailsDataSet = d2.select_method_wo_parameter(markDetailsQry, "Text");
                                    if (markDetailsDataSet.Tables.Count > 0 && markDetailsDataSet.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < markDetailsDataSet.Tables[0].Rows.Count; k++)
                                        {
                                            double c = 0, g = 0;
                                            if (double.TryParse(Convert.ToString(markDetailsDataSet.Tables[0].Rows[k]["cp"]), out c))
                                                creditPoint = creditPoint + c;
                                            if (double.TryParse(Convert.ToString(markDetailsDataSet.Tables[0].Rows[k]["gp"]), out g))
                                                gradePoint = gradePoint + g;
                                        }
                                        if (creditPoint > 0 && gradePoint > 0)
                                            gpa = gradePoint / creditPoint;
                                    }

                                }
                                else
                                {
                                    percentage = (total > 0 && sub > 0) ? total / sub : 0;
                                }
                            }
                            if (temp == 1 || temp == 2)
                            {
                                string arrearcount = d2.GetFunction("select COUNT (distinct subject_no)as noofarrear from mark_entry where roll_no='" + rollNo.ToString() + "' and subject_no not in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and m.result='pass' and roll_no='" + rollNo.ToString() + "' ) and   subject_no in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no  and exam_code in(SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degCode.ToString() + "' AND Batch_Year ='" + batchYear.ToString() + "' AND current_semester<='" + sem + "') and roll_no='" + rollNo.ToString() + "')");
                                string cgpstr;
                                if (arrearcount == "0" || arrearcount == "")
                                {
                                    if (dept_TransferFlag)
                                        cgpstr = Convert.ToString(d2.Calculete_CGPA(rollNo, sem, degCode, batchYear, "", collegeCode, true));
                                    else
                                        cgpstr = Convert.ToString(d2.Calculete_CGPA(rollNo, sem, degCode, batchYear, "", collegeCode, false));
                                    Double.TryParse(cgpstr, out cgpa);
                                }
                            }
                            spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 1;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Left;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 2, 1, 2);

                            if (temp == 0 || temp == 3)
                            {
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Percentage :";
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Math.Round(percentage, 2, MidpointRounding.AwayFromZero).ToString();
                            }
                            else
                            {
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "GPA        : " + Math.Round(gpa, 2, MidpointRounding.AwayFromZero).ToString();
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = "CGPA       : " + Math.Round(cgpa, 2, MidpointRounding.AwayFromZero).ToString();
                            }

                            //apercentage();
                            //spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 1;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Size = FontUnit.Medium;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].HorizontalAlign = HorizontalAlign.Left;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Left;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Name = "Book Antiqua";
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Bold = true;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Bold = true;
                            //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                            //dum_tage_date = (per_workingdays > 0) ? ((pre_present_date / per_workingdays) * 100) : 0;
                            //if (dum_tage_date > 100)
                            //{
                            //    dum_tage_date = 100;
                            //}
                            //else
                            //{
                            //    dum_tage_date = Math.Round(dum_tage_date, 2, MidpointRounding.AwayFromZero);
                            //}
                            //if (day_flag_str == true)
                            //{
                            //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Daywise Attendance Percentage :" + dum_tage_date;
                            //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = true;
                            //}
                            //else
                            //{
                            //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = " ";
                            //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = false;
                            //}
                            //spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount + 1;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Size = FontUnit.Medium;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].HorizontalAlign = HorizontalAlign.Left;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Left;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Name = "Book Antiqua";
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 2].Font.Bold = true;
                            //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Bold = true;
                            //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                            //per_con_hrs = (per_workingdays1) + tot_conduct_hr_spl_fals;
                            //per_tage_hrs = (per_con_hrs > 0) ? (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100) : 0;
                            //if (per_tage_hrs > 100)
                            //{
                            //    per_tage_hrs = 100;
                            //}
                            //else
                            //{
                            //    per_tage_hrs = Math.Round(per_tage_hrs, 2, MidpointRounding.AwayFromZero);
                            //}
                            //if (hr_flag_str == true)
                            //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Hourwise Attendance Percentage :" + per_tage_hrs;
                            //else
                            //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = " ";

                            //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);

                            //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 2, 1, 2);
                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = "Remarks";
                            spancolumn = 6;
                          spreadDet.Sheets[0].SpanModel.Add(span_row, 0, 1, spancolumn);
                            spreadDet.Sheets[0].Cells[span_row, 0].Text = "Semester " + (sem).ToString();

                            spreadDet.Sheets[0].Cells[span_row, 0].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Cells[span_row, 0].Font.Size = FontUnit.XLarge;
                            spreadDet.Sheets[0].Cells[span_row, 0].Font.Bold = true;
                            spreadDet.Sheets[0].Cells[span_row, 0].ForeColor = Color.Blue;
                            spreadDet.Sheets[0].Cells[span_row, 0].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].Cells[span_row, 0].BackColor = Color.LightGray;
                            spreadDet.Sheets[0].SpanModel.Add(span_row, spancolumn, sub + 5, spreadDet.Sheets[0].ColumnCount - spancolumn);//
                            spreadDet.Sheets[0].Cells[span_row, spancolumn-1].BackColor = Color.White;

                        }
                        else
                        {
                            spreadDet.Sheets[0].RowCount = spreadDet.Sheets[0].RowCount - 3;
                        }

                    }
                }
            }

            if (spreadDet.Sheets[0].RowCount > 0)
            {
                for (int j = column + 5; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    spreadDet.Sheets[0].Columns[j].Visible = false;
                }
                int widt = 0; int heights = 0;
                widt = (75 * (column + 5)) + 200;
                if (widt > 900)
                {
                    heights = 16;
                    widt = 900;
                    spreadDet.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                }
                else
                {
                    spreadDet.Width = widt;
                    spreadDet.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                }
                heights = heights + (spreadDet.Rows.Default.Height * spreadDet.Sheets[0].RowCount) +140 ;//65
                if (heights >= 500)
                {
                    spreadDet.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    spreadDet.Height = 500;
                    spreadDet.Width = widt + 16;
                }
                else if (heights < 500)
                {
                    spreadDet.Height = heights;
                    spreadDet.Width = widt;
                }
                spreadDet.TitleInfo.Text = "Marks";
                spreadDet.TitleInfo.HorizontalAlign = HorizontalAlign.Center;
                spreadDet.SaveChanges();
                spreadDet.Visible = true;
                divSpreadDet.Visible = true;
                errmsg.Visible = false;
                txtexcelname.Text = "";
            }
            else
            {
                spreadDet.Sheets[0].ColumnCount = 0;
                spreadDet.Sheets[0].ColumnCount = 4;
                spreadDet.Sheets[0].RowCount = 1;
                spreadDet.ColumnHeader.Visible = false;
                spreadDet.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                spreadDet.Width = 600;
                spreadDet.Height = 45;
               // spreadDet.Sheets[0].Cells[0, 0].Text = "No information Available";
               
                spreadDet.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[0, 0].ForeColor = Color.Blue;
                spreadDet.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                spreadDet.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                spreadDet.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                errmsg.Text = "No information Available";
                errmsg.Visible = true;
                divSpreadDet.Visible = false;

            }
        }
        catch (Exception ex) { }
    }

    protected void spreadDet_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //questionflag = true;
        spreadDet.Visible = true;
        //lblqa.Visible = true;
        //txttype.Visible = true;
        //lblmarks.Visible = true;
        //ddlmarks.Visible = true;
        //ptype.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
                divSpreadDet.Visible = true;
                spreadDet.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(Object sender, EventArgs e)
    {
        try
        {
            GetStudentDetails();
            string deginfo = d2.GetFunction("select (c.Course_Name+'-'+de.Dept_Name) as deg from Degree d,course c,Department de where c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code='" + degCode + "'");//Rajkumar on 28-5-2018
            string degreedetails = "Marks Report " + '@' + "Name : " + stud_name + '@' + "Degree : " + deginfo + '@' + "Sem : " + curr_Sem + '@' + "Batch : " + batchYear + '@' + "Date :" + DateTime.Now.ToString();
            string pagename = "IndividualStudentResult.aspx";
            Printcontrol.loadspreaddetails(spreadDet, pagename, degreedetails);
            divSpreadDet.Visible = true;
            spreadDet.Visible = true;
            Printcontrol.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            errmsg.Visible = false;
            //Fpsmarks.Visible = true;
            //Printcontrol.Visible = true;
            //Fpsmarks.Visible = true;
            //Printcontrol.loadspreaddetails(Fpsmarks, "IndReport.aspx", "Marks Report");
        }
        catch
        {
        }
    }

    // Key management for scrambling support
    public byte[] ScrambleKey
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                // Use existing key if non provided
                key = ScrambleKey;
            }
            Session["ScrambleKey"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleKey"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateKey();
                key = rc2.Key;
                Session["ScrambleKey"] = key;
            }
            return key;
        }
    }

    // Initialization vector management for scrambling support
    public byte[] ScrambleIV
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                key = ScrambleIV;
            }
            Session["ScrambleIV"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleIV"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateIV();
                key = rc2.IV;
                Session["ScrambleIV"] = key;
            }
            return key;
        }
    }

    public string Decrypt(string scrambledMessage)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        // URL decode , replace and convert from Base64
        string b64mod = HttpUtility.UrlDecode(scrambledMessage);
        // Replace '@' back to '+' (avoid URLDecode problem)
        string b64 = b64mod.Replace('@', '+');
        // Base64 decode
        byte[] encrypted = Convert.FromBase64String(b64);
        //Get a decryptor that uses the same key and IV as the encryptor.
        ICryptoTransform decryptor = rc2CSP.CreateDecryptor(ScrambleKey, ScrambleIV);
        //Now decrypt the previously encrypted message using the decryptor
        // obtained in the above step.
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[encrypted.Length - 4];
        //Read the data out of the crypto stream.
        byte[] length = new byte[4];
        csDecrypt.Read(length, 0, 4);
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
        int len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);
        //Convert the byte array back into a string.
        return textConverter.GetString(fromEncrypt).Substring(0, len);
    }

    private bool feeStatusSettings()
    {
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings = 'ExcludeUnpaidStudents'");
            if (value == "1")
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public void apercentage()
    {
        String qry = "Select p.No_of_hrs_per_day as 'PER DAY',p.no_of_hrs_I_half_day as 'I_HALF_DAY' ,p.no_of_hrs_II_half_day as 'II_HALF_DAY',p.min_pres_I_half_day as 'MIN PREE I DAY',p.min_pres_II_half_day as 'MIN PREE II DAY' from PeriodAttndSchedule p where degree_code=" + degCode + " and semester=" + sem;

        ds.Clear();
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(qry, "Text");
        int count = ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows.Count != 0)
        {
            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["PER DAY"]), out perdayhrs);
            {
                hours_pres = 0;
                hours_leave = 0;
                hours_od = 0;
                hours_absent = 0;
                hours_present = 0;
                hours_total = 0;
                hours_conduct = 0;
                persentmonthcal();
            }
        }
    }


    public void persentmonthcal()
    {
        string semInfoQry = "Select day(start_date)start_day,month(start_date)start_month,year(start_date)start_year,day(end_date)end_day,month(end_date)end_month,year(end_date)end_year from seminfo where degree_code='" + degCode + "' and semester='" + sem + "' and batch_year=" + batchYear;
        DataSet semInfoDataSet = d2.select_method_wo_parameter(semInfoQry, "Text");
        if (semInfoDataSet.Tables.Count > 0 && semInfoDataSet.Tables[0].Rows.Count > 0)
        {
            fromDay = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["start_day"]));
            fromMonth = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["start_month"]));
            fromYear = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["start_year"]));
            toDay = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["end_day"]));
            toMonth = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["end_month"]));
            toYear = int.Parse(Convert.ToString(semInfoDataSet.Tables[0].Rows[0]["end_year"]));

            find_values(rollNo, fromMonth + "/" + fromDay + "/" + fromYear, toMonth + "/" + toDay + "/" + toYear, degCode, sem);
        }
    }

    public void find_values(string roll_no, string cur_start_date_date, string cur_end_date_date, string deg, string sem)
    {
        string cur_start_date = "", cur_end_date = string.Empty;
        int tot_abs_hrs = 0;
        hat.Clear();
        ds.Clear();
        hat.Add("degree_code", deg);
        hat.Add("sem_ester", sem);
        ds = d2.select_method("period_attnd_schedule", hat, "sp");
        if (ds.Tables[0].Rows.Count != 0)
        {
            perDayHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
            firstHalfHrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
            secondHalfHrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
            minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
            minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
        }

        string[] cur_start_date_splt = cur_start_date_date.Split(' ');
        cur_start_date = cur_start_date_splt[0].ToString();
        string[] cur_end_date_splt = cur_end_date_date.Split(' ');
        cur_end_date = cur_end_date_splt[0].ToString();
        string dt = cur_start_date;
        string[] dsplit = dt.Split(new Char[] { '/' });
        cur_start_date = dsplit[0].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[2].ToString();
        demfcal = int.Parse(dsplit[2].ToString());
        demfcal = demfcal * 12;
        cal_from_date = demfcal + int.Parse(dsplit[0].ToString());
        monthcal = cal_from_date.ToString();
        dt = cur_end_date;
        dsplit = dt.Split(new Char[] { '/' });
        cur_end_date = dsplit[0].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[2].ToString();
        demtcal = int.Parse(dsplit[2].ToString());
        demtcal = demtcal * 12;
        cal_to_date = demtcal + int.Parse(dsplit[0].ToString());
        per_from_date = Convert.ToDateTime(cur_start_date_date);
        per_to_date = Convert.ToDateTime(cur_end_date_date);
        dumm_from_date = per_from_date;
        persentmonthcal_attnd(roll_no, cur_start_date, cur_end_date, deg, sem);
        double dum_tage_hrs = 0;
        dum_tage_date = ((pre_present_date / per_workingdays) * 100);
        if (dum_tage_date > 100)
        {
            dum_tage_date = 100;
        }
        else
        {
            dum_tage_date = Math.Round(dum_tage_date, 2, MidpointRounding.AwayFromZero);
        }
        per_con_hrs = (per_workingdays1 - dum_unmark) + tot_conduct_hr_spl_fals;
        dum_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);

        if (dum_tage_hrs > 100)
        {
            dum_tage_hrs = 100;
        }
        else
        {
            dum_tage_hrs = Math.Round(dum_tage_hrs, 2, MidpointRounding.AwayFromZero);
        }
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 1].Text = per_workingdays.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 2].Text = pre_present_date.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 3].Text = per_absent_date.ToString(); //per_dum_unmark.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].Text = dum_tage_date.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].Text = per_con_hrs.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 6].Text = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 7].Text = absent_hours.ToString(); //(per_absent_date + per_abshrs_spl).ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 8].Text = dum_tage_hrs.ToString();
        //Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 8].Note = dum_tage_hrs.ToString();

    }

    public void persentmonthcal_attnd(string roll_no, string cur_start_date_date, string cur_end_date_date, string deg, string sem)
    {
        // try
        {

            int conducthrs = 0;
            tot_ondu = 0; dum_unmark = 0; per_holidate_mng = 0;
            njdate = 0; Present = 0; tot_per_hrs = 0; Absent = 0; Leave = 0; workingdays = 0; absent_hours = 0;
            per_holidate = 0; per_njdate = 0; mng_conducted_half_days = 0;
            evng_conducted_half_days = 0; per_holidate_evng = 0; notconsider_value = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            Boolean splhr_flag = false;
            notconsider_value = 0;
            int demfcal, demtcal;
            string monthcal;
            conduct_hour_new = 0;
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;

            hat.Clear();
            hat.Add("std_rollno", roll_no);
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            {
                hat.Clear();
                hat.Add("degree_code", deg);
                hat.Add("sem", sem);
                hat.Add("from_date", cur_start_date_date.ToString());
                hat.Add("to_date", cur_end_date_date.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + cur_start_date_date.ToString() + "' and '" + cur_end_date_date.ToString() + "' and degree_code=" + deg + " and semester=" + sem + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();
                holiday_table11.Clear();
                holiday_table21.Clear();
                holiday_table31.Clear();
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                    {
                        if (ds3.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[0].Rows[0]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[0].Rows[0]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }
                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                        string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);

                    }
                }
                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table21.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                            holiday_table21.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        if (ds3.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[1].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[1].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }
                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }
                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                            holiday_table31.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        if (ds3.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[2].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[2].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }
                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        if (!holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                con.Close();
                cmd.CommandText = "select rights from  special_hr_rights ";
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;

                        }
                    }
                }
            }

            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;
            if (ds2.Tables[0].Rows.Count != 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;

                while (dumm_from_date <= (per_to_date))
                {
                    int temp_unmark = 0;
                    if (splhr_flag == true)
                    {
                        getspecial_hr(roll_no, cur_start_date_date, cur_end_date_date, deg, sem);
                    }
                    for (int i = 1; i <= mmyycount; i++)
                    {
                        if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                        {
                            string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');
                            if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[0])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                            {
                                holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[0])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                            }
                            if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[0])).ToString().TrimStart('0') + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                            {
                                value_holi_status = GetCorrespondingKey(dummy_split[1].ToString().TrimStart('0') + "/" + dummy_split[0].ToString().TrimStart('0') + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                split_holiday_status = value_holi_status.Split('*');
                                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                {
                                    split_holiday_status_1 = "1";
                                    split_holiday_status_2 = "1";
                                }
                                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                {
                                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                    {
                                        split_holiday_status_1 = "0";
                                        split_holiday_status_2 = "1";
                                    }
                                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "0";
                                    }
                                }
                                else if (split_holiday_status[0].ToString() == "0")
                                {
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;
                                        }
                                    }
                                    break;
                                }
                                if (ds3.Tables[1].Rows.Count != 0)
                                {
                                    ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                    diff_date = Convert.ToString(ts.Days);
                                    dif_date = double.Parse(diff_date.ToString());
                                }
                                else
                                {
                                    dif_date = 0;
                                }
                                if (dif_date == 1)
                                {
                                    leave_pointer = holi_leav;
                                    absent_pointer = holi_absent;
                                }
                                else if (dif_date == -1)
                                {
                                    leave_pointer = holi_leav;
                                    absent_pointer = holi_absent;
                                    if (ccount > rowcount)
                                    {
                                        rowcount += 1;
                                    }
                                }
                                else
                                {
                                    leave_pointer = leav_pt;
                                    absent_pointer = absent_pt;
                                }
                                if (ds3.Tables[2].Rows.Count != 0)
                                {
                                    ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                    diff_date = Convert.ToString(ts.Days);
                                    dif_date = double.Parse(diff_date.ToString());
                                    if (dif_date == 1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                    }
                                }
                                if (dif_date1 == -1)
                                {
                                    leave_pointer = holi_leav;
                                    absent_pointer = holi_absent;
                                }
                                dif_date1 = 0;
                                if (split_holiday_status_1 == "1")
                                {
                                    for (i = 1; i <= firstHalfHrs; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds2.Tables[0].Rows[next][date].ToString();
                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (value != "12")
                                                conducthrs = conducthrs + 1;
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count; j++)
                                                {
                                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                                absent_hours = absent_hours + 1;
                                            }
                                            else if (ObtValue == 2)
                                            {
                                                notconsider_value += 1;
                                                njhr += 1;
                                            }
                                            else if (ObtValue == 0)
                                            {
                                                per_perhrs += 1;
                                                tot_per_hrs += 1;
                                            }
                                            if (value == "3")
                                            {
                                                per_ondu += 1;
                                                tot_ondu += 1;
                                            }
                                            else if (value == "10")
                                            {
                                                per_leave += 1;
                                            }
                                        }
                                        else if (value == "7")
                                        {
                                            per_hhday += 1;
                                        }
                                        else
                                        {
                                            unmark += 1;
                                            temp_unmark++;
                                        }
                                    }

                                    if (per_perhrs + njhr >= minpresI)
                                    {
                                        Present += 0.5;
                                    }
                                    else if (per_leave >= 1)
                                    {
                                        leave_point += leave_pointer / 2;
                                        Leave += 0.5;
                                    }
                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }
                                    if (njhr >= minpresI)
                                    {
                                        njdate += 0.5;
                                        njdate_mng += 1;
                                    }
                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }
                                    if (temp_unmark == firstHalfHrs)
                                    {
                                        per_holidate_mng += 1;
                                        per_holidate += 0.5;
                                        unmark = 0;
                                    }
                                    else
                                    {
                                        dum_unmark = temp_unmark;
                                    }
                                    if (firstHalfHrs - temp_unmark >= minpresI)
                                    {
                                        workingdays += 0.5;
                                    }
                                    mng_conducted_half_days += 1;
                                }
                                per_perhrs = 0;
                                per_ondu = 0;
                                per_leave = 0;
                                per_abshrs = 0;
                                temp_unmark = 0;
                                njhr = 0;
                                int k = firstHalfHrs + 1;
                                if (split_holiday_status_2 == "1")
                                {
                                    for (i = k; i <= perDayHrs; i++)
                                    {
                                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                        value = ds2.Tables[0].Rows[next][date].ToString();
                                        if (value != null && value != "0" && value != "7" && value != "")
                                        {
                                            if (value != "12")
                                                conducthrs = conducthrs + 1;
                                            if (tempvalue != value)
                                            {
                                                tempvalue = value;
                                                for (int j = 0; j < count; j++)
                                                {
                                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                    {
                                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                        j = count;
                                                    }
                                                }
                                            }
                                            if (ObtValue == 1)
                                            {
                                                per_abshrs += 1;
                                                absent_hours = absent_hours + 1;
                                            }
                                            else if (ObtValue == 2)
                                            {
                                                notconsider_value += 1;
                                                njhr += 1;
                                            }
                                            else if (ObtValue == 0)
                                            {
                                                per_perhrs += 1;
                                                tot_per_hrs += 1;
                                            }
                                            if (value == "3")
                                            {
                                                per_ondu += 1;
                                                tot_ondu += 1;
                                            }
                                            else if (value == "10")
                                                per_leave += 1;
                                        }
                                        else if (value == "7")
                                        {
                                            per_hhday += 1;
                                        }
                                        else
                                        {
                                            unmark += 1;
                                            temp_unmark++;
                                        }
                                    }

                                    if (per_perhrs + njhr >= minpresII)
                                    {
                                        Present += 0.5;
                                    }
                                    else if (per_leave >= 1)
                                    {
                                        leave_point += leave_pointer / 2;
                                        Leave += 0.5;
                                    }
                                    else if (per_abshrs >= 1)
                                    {
                                        Absent += 0.5;
                                        absent_point += absent_pointer / 2;
                                    }
                                    if (njhr >= minpresII)
                                    {
                                        njdate_evng += 1;
                                        njdate += 0.5;
                                    }
                                    if (per_ondu >= 1)
                                    {
                                        Onduty += 0.5;
                                    }
                                    if (temp_unmark == perDayHrs - firstHalfHrs)
                                    {
                                        per_holidate_evng += 1;
                                        per_holidate += 0.5;
                                        unmark = 0;
                                    }
                                    else
                                    {
                                        dum_unmark += unmark;
                                    }
                                    if ((perDayHrs - firstHalfHrs) - temp_unmark >= minpresII)
                                    {
                                        workingdays += 0.5;
                                    }
                                    evng_conducted_half_days += 1;
                                }
                                per_perhrs = 0;
                                per_ondu = 0;
                                per_leave = 0;
                                per_abshrs = 0;
                                unmark = 0;
                                njhr = 0;
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                    if (moncount > next)
                                    {
                                        next++;
                                    }
                                }
                                per_perhrs = 0;
                            }
                        }
                        else
                        {
                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                cal_from_date++;
                                if (moncount > next)
                                {

                                }
                            }

                        }
                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }
            per_tot_ondu = tot_ondu;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_njdate;
            per_workingdays1 = conducthrs;
            per_dum_unmark = dum_unmark;
            Present = 0;
            tot_per_hrs = 0;
            Absent = 0;
            Onduty = 0;
            Leave = 0;
            workingdays = 0;
            per_holidate = 0;
            dum_unmark = 0;
            absent_point = 0;
            leave_point = 0;
            njdate = 0;
            tot_ondu = 0;
        }
        //   catch
        {
        }
    }

    public void getspecial_hr(string roll_no, string cur_start_date_date, string cur_end_date_date, string deg, string sem)
    {
        //  try
        {
            con_splhr_query_master.Close();
            con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();

            string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no in (select hrentry_no from  specialhr_master where batch_year=" + batchYear + " and semester=" + sem + " and degree_code=" + deg + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + batchYear + " and current_semester=" + sem + " and degree_code=" + deg + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + roll_no + "'  order by r.roll_no asc";
            SqlDataReader dr_splhr_query_master;
            cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
            dr_splhr_query_master = cmd.ExecuteReader();
            while (dr_splhr_query_master.Read())
            {
                if (dr_splhr_query_master.HasRows)
                {
                    value = dr_splhr_query_master[0].ToString();
                    if (value != null && value != "0" && value != "7" && value != "" && value != "12")
                    {
                        if (tempvalue != value)
                        {
                            tempvalue = value;
                            for (int j = 0; j < count; j++)
                            {
                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                {
                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                    j = count;
                                }
                            }
                        }
                        if (ObtValue == 1)
                        {
                            per_abshrs_spl += 1;
                        }
                        else if (ObtValue == 2)
                        {
                            notconsider_value += 1;
                            njhr += 1;
                        }
                        else if (ObtValue == 0)
                        {
                            tot_per_hrs_spl += 1;
                        }
                        if (value == "3")
                        {
                            tot_ondu_spl += 1;
                        }
                        else if (value == "10")
                        {
                            per_leave += 1;
                        }
                        tot_conduct_hr_spl++;
                    }
                    else if (value == "7")
                    {
                        per_hhday_spl += 1;
                        tot_conduct_hr_spl--;
                    }
                    else
                    {
                        unmark_spl += 1;
                        tot_conduct_hr_spl--;
                    }
                }
            }
            per_abshrs_spl_fals = per_abshrs_spl;
            tot_per_hrs_spl_fals = tot_per_hrs_spl;
            per_leave_fals = per_leave;
            tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
            tot_ondu_spl_fals = tot_ondu_spl;
        }
        //  catch
        {
        }
    }


    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr = string.Empty;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataReader drnew;
        SqlCommand cd = new SqlCommand(sqlstr);
        cd.Connection = getsql;
        drnew = cd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    public string Attmark(string Attstr_mark)
    {
        if (Attstr_mark == "-1")
        {
            Att_mark = "AAA";
        }
        else if (Attstr_mark == "-2")
        {
            Att_mark = "EL";
        }
        else if (Attstr_mark == "-3")
        {
            Att_mark = "EOD";
        }
        if (Attstr_mark == "-4")
        {
            Att_mark = "ML";
        }
        else if (Attstr_mark == "-5")
        {
            Att_mark = "SOD";
        }
        else if (Attstr_mark == "-6")
        {
            Att_mark = "NSS";
        }
        if (Attstr_mark == "-7")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "-8")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "-9")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "-10")
        {
            Att_mark = "NCC";
        }
        else if (Attstr_mark == "-11")
        {
            Att_mark = "HS";
        }
        else if (Attstr_mark == "-12")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "-13")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "-14")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "-15")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "-16")
        {
            Att_mark = "OD";
        }
        return Att_mark;
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

}