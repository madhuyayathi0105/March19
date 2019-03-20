using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Globalization;
using System.Configuration;
public partial class StudentMod_Attendance_certificate : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    DataSet ds_Allstudent = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    ReuasableMethods rs = new ReuasableMethods();
    string q1 = "";
    Boolean cellclick = false;
    Hashtable hat = new Hashtable();
    int i = 0; string strorder, strregorder = "";
   
    #region Attendance
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;
    TimeSpan ts;
    Boolean deptflag = false;
    string frdate, todate;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string dd = "";
    string diff_date;
    string value, date;
    string tempvalue = "-1";
    string value_holi_status = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string[] split_holiday_status = new string[1000];
    double dif_date = 0;
    double dif_date1 = 0;
    double per_perhrs, per_abshrs, per_leavehrs;
    double per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    //magesh 29/1/18
    double per_ondu;//magesh 29/1/18
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    Double leavfinaeamount = 0;
    Double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    Double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    Double medicalLeaveDays = 0;
    int mmyycount = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int notconsider_value = 0;
    int moncount;
    int unmark;
    int NoHrs = 0;
    int fnhrs = 0;
    int minpresI = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    int rows_count;
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;
    #endregion Attendance
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            lblvalidation1.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = "and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = "and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                q1 = "select * from Master_Settings where settings in('Roll No','Register No','Student_Type','Admission No')  " + grouporusercode + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Admissionflag"] = "0";
                //magesh 29/1/18
                Session["attdaywisecla"] = "0";
                string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
                if (daywisecal.Trim() == "1")
                {
                    Session["attdaywisecla"] = "1";
                }//magesh 29/1/18
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["settings"].ToString() == "Roll No" && dr["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dr["settings"].ToString() == "Register No" && dr["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dr["settings"].ToString() == "Admission No" && dr["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                        if (dr["settings"].ToString() == "Student_Type" && dr["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
                //setLabelText();
                Bindcollege();
                Bindbatch();
                BindDegree();
                Bindbranch();
                Bindsem();
                BindSection();
            }
        }
        catch (Exception ex) { }
    }
   
    protected void Bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            q1 = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"].ToString() + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch { }
    }
    
    public void Bindbatch()
    {
        try
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct batch_year from tbl_attendance_rights order by batch_year desc", "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch
        {
        }
    }
    
    public void BindDegree()
    {
        try
        {
            if (ddl_college.Items.Count > 0)
            {
                ddldegree.Items.Clear();
                usercode = Session["usercode"].ToString();
                collegecode = ddl_college.SelectedItem.Value.ToString();//Session["collegecode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                hat.Clear();
                hat.Add("single_user", singleuser);
                hat.Add("group_code", group_user);
                hat.Add("college_code", collegecode);
                hat.Add("user_code", usercode);
                ds = d2.select_method("bind_degree", hat, "sp");
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataBind();
                }
            }
        }
        catch { }
    }
    
    public void Bindbranch()
    {
        ddlbranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = ddl_college.SelectedItem.Value.ToString(); //Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
    }
    
    public void Bindsem()
    {
        ddlsemester.Items.Clear(); cbl_printsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        if (ddl_college.Items.Count > 0)
        {
            q1 = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + ddl_college.SelectedItem.Value.ToString() + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0]["ndurations"].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                        cbl_printsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                        cbl_printsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                q1 = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + ddl_college.SelectedItem.Value.ToString() + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0]["duration"].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsemester.Items.Add(i.ToString());
                            cbl_printsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            cbl_printsem.Items.Add(i.ToString());
                            ddlsemester.Items.Add(i.ToString());
                        }
                    }
                }
            }
        }
    }
    
    public void BindSection()
    {
        ddlsection.Items.Clear();
        q1 = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code='" + ddl_college.SelectedItem.Value.ToString() + "' and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlsection.DataSource = ds;
            ddlsection.DataTextField = "sections";
            ddlsection.DataBind();
            ddlsection.Items.Insert(0, "All");
        }
        if (ds.Tables[0].Rows.Count > 0 == true)
        {
            if (ds.Tables[0].Rows[0]["sections"].ToString() == string.Empty)
            {
                ddlsection.Enabled = false;
            }
            else
            {
                ddlsection.Enabled = true;
            }
        }
        else
        {
            ddlsection.Enabled = false;
        }
    }
    
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bindbranch();
        Bindsem();
        BindSection();
    }
    
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSection();
    }
    
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bindbranch();
        Bindsem();
        BindSection();
    }
    
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if (ddlbranch.Items.Count > 0)
            {
                Bindsem();
                BindSection();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSection();
    }
    
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (ddl_college.Items.Count > 0 && ddlbatch.Items.Count > 0 && ddldegree.Items.Count > 0 && ddlbranch.Items.Count > 0)
            {
                string filterwithsection = "";
                string header = "S.No-50/Select-50/Roll No-150/Reg No-150/Admission No-150/Student Name-250/Student Type-150/Degree-300/Section-100";
                //string header = "S.No/Select/Roll No/Reg No/Admission No/Student Name/Student Type/Degree";
                Fpreadheaderbindmethod(header, FpSpread1, "false");
                if (FpSpread1.RowHeader.ColumnCount > 0)
                {
                    filteration();
                    string sec;
                    if (ddlsection.Enabled == true)
                    {
                        if (ddlsection.SelectedItem.ToString() == string.Empty || ddlsection.Text == "All")
                        {
                            sec = "";
                        }
                        else
                        {
                            sec = ddlsection.SelectedItem.ToString();
                        }
                    }
                    else
                    {
                        sec = "";
                    }
                    if (sec != "")
                        filterwithsection = " and exam_flag<>'debar' and delflag=0 and r.batch_year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "'   and r.sections='" + sec.ToString() + "'" + strorder + "";
                    else
                        filterwithsection = "and exam_flag<>'debar' and delflag=0 and r.batch_year='" + ddlbatch.SelectedItem.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + strorder + "";
                    q1 = "select distinct roll_no as ROLL_NO,Reg_No as REG_NO,Stud_Name as STUD_NAME,Roll_Admit as ADMIT_NO, stud_type as Student_Type, len(roll_no ), convert(varchar(15),adm_date,103) as adm_date,convert(varchar(10),r.Batch_Year)+'-'+ c.Course_Name +'-'+dt.Dept_Name as degree,r.degree_code,r.App_No,r.Sections from registration r,Degree d,course c,Department dt where d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.college_code=d.college_code " + filterwithsection + "";
                    ds_Allstudent = d2.select_method_wo_parameter(q1, "text");
                    if (ds_Allstudent.Tables != null)
                    {
                        if (ds_Allstudent.Tables[0].Rows.Count > 0)
                        {
                            //magesh 27/1/18
                            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();//magesh 27/1/18
                            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkcell.AutoPostBack = false;
                            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkall.AutoPostBack = true;
                            ++FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            foreach (DataRow dr in ds_Allstudent.Tables[0].Rows)
                            {
                                ++FpSpread1.Sheets[0].RowCount;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = (FpSpread1.Sheets[0].RowCount - 1).ToString();
                                //magesh 27/1/18
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;//magesh 27/1/18
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dr["Roll_No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dr["app_no"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = dr["adm_date"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dr["Reg_No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = dr["degree_code"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dr["ADMIT_NO"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = dr["Stud_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dr["Student_Type"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = dr["degree"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = dr["Sections"].ToString();


                                #region Alignment
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                                #endregion
                            }
                            if (Convert.ToString(Session["Rollflag"]) == "0")
                                FpSpread1.Sheets[0].Columns[2].Visible = false;
                            else
                                FpSpread1.Sheets[0].Columns[2].Visible = true;
                            if (Convert.ToString(Session["Regflag"]) == "0")
                                FpSpread1.Sheets[0].Columns[3].Visible = false;
                            else
                                FpSpread1.Sheets[0].Columns[3].Visible = true;
                            if (Convert.ToString(Session["Admissionflag"]) == "0")
                                FpSpread1.Sheets[0].Columns[4].Visible = false;
                            else
                                FpSpread1.Sheets[0].Columns[4].Visible = true;
                            if (Convert.ToString(Session["Studflag"]) == "0")
                                FpSpread1.Sheets[0].Columns[6].Visible = false;
                            else
                                FpSpread1.Sheets[0].Columns[6].Visible = true;
                        }
                        FpSpread1.SaveChanges();
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Sheets[0].FrozenRowCount = 1;
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                    }
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Font.Size = FontUnit.Medium;
                lblalerterr.Text = "Please select all fields";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    
    public void filteration()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
        if (orderby_Setting == "")
        {
            strorder = "";
            strregorder = "";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strregorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strregorder = "ORDER BY registration.Stud_Name";
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
        }
    }
    
    protected void Fpspread_command(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string selval = Convert.ToString(FpSpread1.Sheets[0].Cells[0, 1].Value);
            if (selval == "1")
            {
                for (i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else
            {
                for (i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
        }
        catch { }
    }
    
    protected void btn_Generate_cer_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            printcertificate();
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    
    protected void printcertificate()
    {
        FpSpread1.SaveChanges(); bool selectvalueempty = false;
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        Gios.Pdf.PdfPage mypage;// = mydoc.NewPage();
        string printsem = cblselecteditemcount(cbl_printsem);
        if (printsem != "0")
        {
            for (int sel = 1; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
            {
                int value = Convert.ToInt32(FpSpread1.Sheets[0].Cells[sel, 1].Value);
                if (value == 1)
                {
                    mypage = mydoc.NewPage();
                    selectvalueempty = true;
                    string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 2].Text);
                    string appno = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 2].Tag);
                    string admitDate = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 2].Note);
                    string degreecode = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 3].Tag);
                    string stud_name = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 5].Text);
                    string[] classname = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 7].Text).Split('-');
                    string classdet = classname[1] + " - " + classname[2];
                    string yearofstudy = Convert.ToInt32(classname[0]) + " - " + (Convert.ToInt32(classname[0]) + 3);
                    string batchyear = Convert.ToString(classname[0]);
                    #region college
                    Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
                    Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
                    Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypage.Add(LogoImage, 30, 30, 400);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypage.Add(LogoImage, 500, 30, 400);
                    }
                    string collquery = "";
                    collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + Convert.ToString(ddl_college.SelectedItem.Value) + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(collquery, "Text");
                    string collegename = "";
                    string collegeaddress = "";
                    string collegedistrict = "";
                    string phonenumber = "";
                    string fax = "";
                    string email = "";
                    string website = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                        collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                        phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                        email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                    }
                    PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 10, 20, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 130, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 130, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 130, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 130, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 130, 75, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);
                    mypage.Add(ptc);
                    #endregion
                    int coltop = 75;
                    int left1 = 80;
                    PdfArea pa1 = new PdfArea(mydoc, 14, 14, 565, 810);// 14, 12, 560, 825);
                    PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                    mypage.Add(pr3);
                    coltop = coltop + 20;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE CERTIFICATE");
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "____________________________");
                    mypage.Add(ptc);
                    coltop = coltop + 60;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "NAME                        :");
                    mypage.Add(ptc);
                    left1 = 200;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, left1, coltop, 250, 50), System.Drawing.ContentAlignment.MiddleLeft, stud_name);
                    mypage.Add(ptc);
                    coltop = coltop + 30; left1 = 80;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "CLASS                       :");
                    mypage.Add(ptc);
                    left1 = 200;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, classdet);
                    mypage.Add(ptc);
                    coltop = coltop + 30; left1 = 80;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "YEAR OF STUDY   :");
                    mypage.Add(ptc);
                    left1 = 200;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, yearofstudy);
                    mypage.Add(ptc);

                    Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, (Convert.ToInt32(printsem) + 1), 3, 4);
                    table2 = mydoc.NewTable(Fontsmall, (Convert.ToInt32(printsem) + 1), 3, 4);
                    table2.VisibleHeaders = false;
                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table2.Columns[0].SetWidth(100);
                    table2.Columns[1].SetWidth(100);
                    table2.Columns[2].SetWidth(100);
                    table2.CellRange(0, 0, 0, 2).SetFont(Fontbold1);
                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 0).SetContent("SEMESTER");
                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 1).SetContent("TOTAL NUMBER OF WORKING DAYS");
                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 2).SetContent("NUMBER OF DAYS ABSENT");
                    coltop = coltop + 80;
                    if (cbl_printsem.Items.Count > 0)
                    {
                        int row = 0;
                        for (int add = 0; add < cbl_printsem.Items.Count; add++)
                        {
                            if (cbl_printsem.Items[add].Selected == true)
                            {
                                per_workingdays = 0; pre_present_date = 0;
                                persentmonthcal(ddl_college.SelectedItem.Value, degreecode, cbl_printsem.Items[add].Value, rollno, admitDate, batchyear);
                                row++;
                                table2.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(row, 0).SetContent(rs.romanLetter(cbl_printsem.Items[add].Value));
                                table2.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(row, 1).SetContent(per_workingdays);
                                double absentdays = (per_workingdays - Convert.ToDouble(pre_present_date));
                                table2.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(row, 2).SetContent(absentdays);
                            }
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new PdfArea(mydoc, 80, coltop, 400, 550));
                        mypage.Add(myprov_pdfpage1);
                        coltop += Convert.ToInt32(myprov_pdfpage1.Area.Height);
                    }
                    coltop = coltop + 50; left1 = 80;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE :" + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    mypage.Add(ptc);
                    left1 = 430;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "SIGNATURE");
                    mypage.Add(ptc);
                    mypage.SaveToDocument();
                }
            }
            if (selectvalueempty)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.End();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Font.Size = FontUnit.Medium;
                lblalerterr.Text = "Please Any one student";
            }
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Font.Size = FontUnit.Medium;
            lblalerterr.Text = "Please Select Print Semester";
        }
    }
    
    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate, string batchyear)
    {
        hat.Clear();
        hat.Add("degree_code", degree);
        hat.Add("sem_ester", int.Parse(sem));
        ds = d2.select_method("period_attnd_schedule", hat, "sp");
        if (ds.Tables[0].Rows.Count != 0)
        {
            NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
            fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
            minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
            minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
            minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
        }
        hat.Clear();
        hat.Add("colege_code", Session["collegecode"].ToString());
        ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
        count = ds1.Tables[0].Rows.Count;
        q1 = "select CONVERT(varchar(10), start_date,103) start_date,CONVERT(varchar(10), end_date,103) end_date,no_of_working_Days from seminfo where degree_code='" + degree + "' and batch_year='" + batchyear + "' and semester='" + sem + "'";
        DataSet semstartenddate = new DataSet();
        semstartenddate = d2.select_method_wo_parameter(q1, "TEXT");
        if (semstartenddate.Tables != null)
        {
            if (semstartenddate.Tables[0].Rows.Count > 0)
            {
                //frdate = Convert.ToString(semstartenddate.Tables[0].Rows[0]["start_date"]); //txtFromDate.Text;
                //todate = Convert.ToString(semstartenddate.Tables[0].Rows[0]["end_date"]);
                frdate = txt_fromdate.Text; //txtFromDate.Text;
                todate = txt_todate.Text;
                string dt = frdate;
                string[] dsplit = dt.Split(new Char[] { '/' });
                frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                int demfcal = int.Parse(dsplit[2].ToString());
                demfcal = demfcal * 12;
                cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
                string monthcal = cal_from_date.ToString();
                dt = todate;
                dsplit = dt.Split(new Char[] { '/' });
                todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                int demtcal = int.Parse(dsplit[2].ToString());
                demtcal = demtcal * 12;
                cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());
                per_from_gendate = Convert.ToDateTime(frdate);
                per_to_gendate = Convert.ToDateTime(todate);

            }
            else
            {
                return;
            }
        }
        medicalLeaveCountPerSession = 0;
        Boolean isadm = false;
        per_abshrs_spl = 0;
        tot_per_hrs_spl = 0;
        tot_conduct_hr_spl = 0;
        tot_ondu_spl = 0;
        tot_ml_spl = 0;
        int my_un_mark = 0;
        int njdate_mng = 0, njdate_evng = 0;
        int per_holidate_mng = 0, per_holidate_evng = 0;
        mng_conducted_half_days = 0;
        evng_conducted_half_days = 0;
        notconsider_value = 0;
        cal_from_date = cal_from_date_tmp;
        cal_to_date = cal_to_date_tmp;
        per_from_date = per_from_gendate;
        per_to_date = per_to_gendate;
        dumm_from_date = per_from_date;
        string admdate = admitDate;
        DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);
        dd = rollno.Trim();
        hat.Clear();
        hat.Add("std_rollno", rollno.Trim());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
            hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
            hat.Add("from_date", Convert.ToString(frdate));
            hat.Add("to_date", Convert.ToString(todate));
            hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
            DataSet dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            DataSet dsondutyva = new DataSet();
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
                    if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                    }
                }
            }
            if (ds3.Tables[1].Rows.Count != 0)
            {
                for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                {
                    string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                    string[] dummy_split = split_date_time1[0].Split('/');
                    holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
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
                    if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                    {
                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                    }
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
                    if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }
            }
        }
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
                medicalLeaveCountPerSession = 0;
                nohrsprsentperday = 0;
                noofdaypresen = 0;
                isadm = false;
                if (dumm_from_date >= Admission_date)
                {
                    isadm = true;
                    int temp_unmark = 0;
                    for (int i = 1; i <= mmyycount; i++)
                    {
                        ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + rollno + "'";
                        DataView dvattvalue = ds2.Tables[0].DefaultView;
                        if (dvattvalue.Count > 0)
                        {
                            if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
                            {
                                string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');
                                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                                }
                                if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    value_holi_status = holiday_table11[dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()].ToString();
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
                                    per_leavehrs = 0;
                                    if (split_holiday_status_1 == "1")
                                    {
                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = dvattvalue[0][date].ToString();
                                            if (value != null && value != "0" && value != "7" && value != "")
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
                                                    per_abshrs += 1;
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
                                                if (value == "4")
                                                {
                                                    medicalLeaveCountPerSession++;
                                                    medicalLeaveHours++;
                                                }
                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
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
                                                my_un_mark++;
                                            }
                                        }
                                        nohrsprsentperday = per_perhrs + njhr;
                                        if (per_perhrs + njhr >= minpresI)
                                        {
                                            Present += 0.5;
                                            noofdaypresen = 0.5;
                                        }
                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                            studentabsentfine = studentabsentfine + moringabsentfine;
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + moringabsentfine;
                                            }
                                        }
                                        if (njhr >= minpresI)
                                        {
                                            njdate += 0.5;
                                            njdate_mng += 1;
                                        }
                                        //magesh 29/1/18
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }//magesh 29/1/18
                                        if (temp_unmark == fnhrs)
                                        {
                                            per_holidate_mng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark = temp_unmark;
                                        }
                                        if (fnhrs - temp_unmark >= minpresI)
                                        {
                                            workingdays += 0.5;
                                        }
                                        mng_conducted_half_days += 1;
                                        if (medicalLeaveCountPerSession + njhr >= minpresI)
                                        {
                                            medicalLeaveDays = medicalLeaveDays + 0.5;
                                        }
                                    }
                                    medicalLeaveCountPerSession = 0;
                                    per_perhrs = 0;
                                    per_abshrs = 0;
                                    temp_unmark = 0;
                                    per_leavehrs = 0;
                                    njhr = 0;
                                    int k = fnhrs + 1;
                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = dvattvalue[0][date].ToString();
                                            if (value != null && value != "0" && value != "7" && value != "")
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
                                                    per_abshrs += 1;
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
                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
                                                }
                                                if (value == "4")
                                                {
                                                    medicalLeaveCountPerSession++;
                                                    medicalLeaveHours++;
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
                                                my_un_mark++;
                                            }
                                        }
                                        nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                        if (per_perhrs + njhr >= minpresII)
                                        {
                                            Present += 0.5;
                                            noofdaypresen = noofdaypresen + 0.5;
                                        }
                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                            studentabsentfine = studentabsentfine + eveingabsentfine;
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + eveingabsentfine;
                                            }
                                        }
                                        if (njhr >= minpresII)
                                        {
                                            njdate_evng += 1;
                                            njdate += 0.5;
                                        }
                                        //magesh 29/1/18
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }//magesh 29/1/18
                                        if (medicalLeaveCountPerSession + njhr >= minpresII)
                                        {
                                            medicalLeaveDays = medicalLeaveDays + 0.5;
                                        }
                                        if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                        {
                                            if (nohrsprsentperday < minpresday)
                                            {
                                                Present = Present - noofdaypresen;
                                                Absent = Absent + noofdaypresen;
                                            }
                                        }
                                        nohrsprsentperday = 0;
                                        noofdaypresen = 0;
                                        if (temp_unmark == NoHrs - fnhrs)
                                        {
                                            per_holidate_evng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark += unmark;
                                        }
                                        if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                        {
                                            workingdays += 0.5;
                                        }
                                        evng_conducted_half_days += 1;
                                    }
                                    per_perhrs = 0;
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
                                        next++;
                                    }
                                }
                            }
                            i = mmyycount + 1;
                        }
                        else
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
                        }
                    }
                }
                if (isadm == false)
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
                }
                nohrsprsentperday = 0;
                noofdaypresen = 0;
            }
            int diff_Date = per_from_date.Day - dumm_from_date.Day;
        }
        per_njdate = njdate;
        pre_present_date = Present - njdate;
        per_per_hrs = tot_per_hrs;
        per_absent_date = Absent;
        pre_ondu_date = Onduty;
        pre_leave_date = Leave;
        per_workingdays = workingdays - per_njdate;
        per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
        per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
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
    }
    
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Attendance Certificate Report";
            string pagename = "Attendance_certificate.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Font.Size = FontUnit.Smaller;
            lblalerterr.Text = ex.ToString();
        }
    }
    
    public string cblselecteditemcount(CheckBoxList cblSelected)
    {
        int count = 0;
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                }
            }
        }
        catch { count = 0; }
        return count.ToString();
    }

}