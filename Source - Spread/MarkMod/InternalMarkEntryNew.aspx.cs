using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Collections;
using InsproDataAccess;

public partial class MarkMod_InternalMarkEntryNew : System.Web.UI.Page
{
    #region variables Declaration
    //SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproStoreAccess storAcc = new InsproStoreAccess();
    InsproDirectAccess dir = new InsproDirectAccess();
    static DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    static string usercode = string.Empty;
    static string collegeCode = string.Empty;
    static int selectedMode = 0;
    bool isBasedOnBatchRights = false;
    static bool isSchool = false;
    bool Cellclick;
    string datelocksetting = string.Empty;
    public bool day_check=true;
    static string selectedSubTest = string.Empty;
    static string selectedSubDetails = "";
    static string newMaxMinMark = "";
    public bool d_check;
    Hashtable hat = new Hashtable();

    static MarkMod_InternalMarkEntryNew nn = new MarkMod_InternalMarkEntryNew();
    DataTable dt = new DataTable();
    DataTable dtest = new DataTable();
    DataTable dtmrk = new DataTable();
    DataRow dr;
    DataRow dr1;
    DataRow dr2;
    static Dictionary<int, string> dicsubdetails = new Dictionary<int, string>();
    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    GridViewRow HeaderGridRow2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    GridViewRow HeaderGridRow3 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    GridViewRow HeaderGridRow4 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }
            if (!IsPostBack)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim();
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                    usercode = Session["group_code"].ToString();
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                    usercode = Convert.ToString(Session["usercode"]).Trim();
                }


                isBasedOnBatchRights = false;
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string batchYearSettings = da.GetFunction("select value from Master_Settings where settings='CAM Entry Based On Batch And Section Rights' and " + grouporusercode + "");
                    if (batchYearSettings.Trim() == "1")
                        isBasedOnBatchRights = true;
                }

                Session["StaffSelector"] = "0";
                string check_Stu_Staff_selector = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");

                if (check_Stu_Staff_selector.Trim() == "1")
                {
                    Session["StaffSelector"] = "1";
                }
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Admisionflag"] = "0";
                Session["Appflag"] = "0";

                string masterQry = string.Empty;
                masterQry = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dsMasterSetting = da.select_method_wo_parameter(masterQry, "Text");
                if (dsMasterSetting.Tables.Count > 0 && dsMasterSetting.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsMasterSetting.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Roll No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Register No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Student_Type" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Admission No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Admisionflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Application No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Appflag"] = "1";
                        }
                    }
                }

                if (Convert.ToString(Session["staff_code"]).Trim() != "")
                {
                    GridView1.Visible = true;
                }
                else
                {
                    GridView1.Visible = false;

                }

                loadSubjectDetails();
                testDetailsLblDiv.Visible = false;
                GridView2.Visible = false;
                Div1.Visible = false;
                divPopSpread.Visible = false;
                btnok.Visible = false;
                chkretest.Visible = false;
                btnReport.Visible = false;
                clearFields();
                //lblNote.Visible = false;
                //GridView3.Visible = false;
                lblNote2.Visible = false;
            }
        }
        catch { }
    }

    #region Subject Details

    private void loadSubjectDetails()
    {
        try
        {
            dt.Columns.Add("batch_year");
            dt.Columns.Add("degree_code");
            dt.Columns.Add("degree");
            dt.Columns.Add("sem");
            dt.Columns.Add("Section");
            dt.Columns.Add("subject_no");
            dt.Columns.Add("subject_name");
            dt.Columns.Add("subject_code");

            string staff_code = string.Empty;
            staff_code = Convert.ToString(Session["staff_code"]).Trim();

            if (staff_code != "")
            {
                string userId = grouporusercode;
                string subDetailsQry = string.Empty;

                string qryBatchBasedSetting = string.Empty;
                if (isBasedOnBatchRights)
                {
                    qryBatchBasedSetting = " and r.Batch_Year in(select Batch_Year from tbl_attendance_rights where user_id='" + Convert.ToString(userId).Trim() + "')";
                }

                subDetailsQry = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and st.staff_code='" + Session["staff_code"].ToString() + "' " + qryBatchBasedSetting + " order by st.batch_year,sy.degree_code,semester,st.sections ";//and lab <>1 

                if (Session["StaffSelector"].ToString() == "1")
                {
                    if (staff_code != null)
                    {
                        if (staff_code.ToString().Trim() != "" && staff_code.ToString().Trim() != "0")
                        {
                            subDetailsQry = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,sy.semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb,subjectChooser sc where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'  and st.staff_code = '" + Convert.ToString(staff_code) + "' and sc.staffcode like '%" + Convert.ToString(staff_code) + "%' and  sc.roll_no=sc.roll_no and sc.subject_no=st.subject_no and sy.semester=sc.semester and sb.subType_no=sc.subtype_no and s.subject_no=sc.subject_no  order by st.batch_year,sy.degree_code,sy.semester,st.sections ";//and lab <>1
                        }
                    }
                }

                DataSet dsSubDetails = da.select_method_wo_parameter(subDetailsQry, "Text");

                if (dsSubDetails.Tables.Count > 0 && dsSubDetails.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSubDetails.Tables[0].Rows.Count; i++)
                    {
                        string current_sem = string.Empty;
                        current_sem = GetFunction("select distinct current_semester from registration where degree_code='" + dsSubDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and batch_year='" + dsSubDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'");
                        if (Convert.ToString(current_sem) == Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]))
                        {

                            if ((Convert.ToString(Session["collegecode"]) != "") && Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]) != "")
                            {
                                string sqlstr = string.Empty;
                                sqlstr = "select course_name + '-'+dept_acronym from degree d,course c,department dp where d.course_id=c.course_id and d.dept_code=dp.dept_code and degree_code= '" + Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]) + "'";
                                string degree = string.Empty;
                                degree = GetFunction(sqlstr.ToString());

                                dr = dt.NewRow();
                                dr["batch_year"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["batch_year"]);
                                dr["degree_code"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]);
                                dr["degree"] = degree.ToString();
                                if (Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]) == "-1")
                                {
                                    dr["sem"] = " ";
                                }
                                else
                                {
                                    dr["sem"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]);
                                }
                                if (Convert.ToString(dsSubDetails.Tables[0].Rows[i]["sections"]) == "-1")
                                {
                                    dr["Section"] = " ";
                                }
                                else
                                {
                                    dr["Section"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["sections"]);
                                }
                                dr["subject_no"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_no"]);
                                dr["subject_name"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_name"]);
                                dr["subject_code"] = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_code"]);
                                dt.Rows.Add(dr);

                            }

                        }
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
        catch { }
    }

    #endregion

    #region Test Details

    public bool daycheck(int CriteriaNo)
    {
        bool daycheck = false;
        string curdate, Dateval;
        int total, k;
        string[] ddate = new string[100];
        curdate = DateTime.Today.ToString();
        string qry = "select Clock,LastDate from CriteriaforInternal where Criteria_no='" + CriteriaNo + "' and Clock = '1' ";
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(qry, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][1].ToString() != null)
                {
                    if (ds.Tables[0].Rows[i][0].ToString().Trim().ToLower() == "true")
                    {
                        Dateval = ds.Tables[0].Rows[i][1].ToString();
                        string[] sel_date12 = Dateval.Split(new Char[] { ' ' });
                        string[] sel_date13 = curdate.Split(new Char[] { ' ' });
                        TimeSpan t = Convert.ToDateTime(sel_date13[0]).Subtract(Convert.ToDateTime(sel_date12[0]));
                        long days = t.Days;
                        if (days >= 0)
                        {
                            daycheck = false;
                        }
                        else
                        {
                            daycheck = true;
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
                else
                {
                    daycheck = true;
                }
            }
        }
        else
        {
            daycheck = true;
        }
        return daycheck;
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Row"] = null;
            Session["Examtype"] = null;
            string ForMate = da.GetFunction("select value from Master_Settings where settings='SaveCoFormate'");
            Session["SaveCoFormate"] = ForMate;
            lblTot.Visible = false;
            lblGrandTotal.Visible = false;
            GridView3.Visible = false;
            int check = 0;
            foreach (GridViewRow row in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)row.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    check++;
                    if (check == 1)
                    {
                        Label tstnam = (Label)row.FindControl("lbltest");
                        string testName = tstnam.Text;
                        lblTestTitle.Text = testName;
                        Label tstno = (Label)row.FindControl("lblcriteriano");
                        string testNo = tstno.Text;
                        Label bind = (Label)row.FindControl("lblbind");
                        string bindsubdet = bind.Text;
                        int j = 0;
                        dicsubdetails.Clear();
                        dicsubdetails.Add(0, bindsubdet);
                        string[] subdetails = bindsubdet.Split('-');
                        string subName = Convert.ToString(subdetails[0]);
                        string SubCode = Convert.ToString(subdetails[2]);
                        lblSubName.Text = SubCode + " - " + subName;
                        string subNo = Convert.ToString(subdetails[1]);
                        selectedSubTest = subNo + "#" + testNo;
                        Label maxmrk1 = (Label)row.FindControl("lblmaxmarks");
                        double new_max_mark1 = Convert.ToSingle(maxmrk1.Text.ToString());
                        lblMaxMark.Text = new_max_mark1.ToString();
                        string date = string.Empty;
                        string month = string.Empty;
                        string year = string.Empty;
                        string examDate = "";
                        string date1 = string.Empty;
                        string month1 = string.Empty;
                        string year1 = string.Empty;
                        string entryDate = "";
                        string hours = string.Empty;
                        string minutes = string.Empty;
                        string duration = "";
                        float new_max_mark = 0;
                        float new_min_mark = 0;

                        Label dat = (Label)row.FindControl("lblexamdate");
                        date = dat.Text.Trim().PadLeft(2, '0');
                        Label mon = (Label)row.FindControl("lblexammonth");
                        month = mon.Text.Trim().PadLeft(2, '0');
                        Label yr = (Label)row.FindControl("lblexamyear");
                        year = yr.Text.Trim();
                        if ((date != "") && (month != "") && (year != "") && (date != null) && (month != null) && (year != null))
                        {
                            examDate = month + "/" + date + "/" + year;
                        }
                        else
                        {
                            examDate = string.Empty;
                        }
                        Label endat = (Label)row.FindControl("lblentrydate");
                        date1 = endat.Text.Trim().PadLeft(2, '0');
                        Label enmon = (Label)row.FindControl("lblentrymonth");
                        month1 = enmon.Text.Trim().PadLeft(2, '0');
                        Label enyr = (Label)row.FindControl("lblentryyear");
                        year1 = enyr.Text.Trim();
                        if ((date1 != "") && (month1 != "") && (year1 != "") && (date1 != null) && (month1 != null) && (year1 != null))
                        {
                            entryDate = month1 + "/" + date1 + "/" + year1;
                        }
                        else
                        {
                            entryDate = DateTime.Now.ToString("MM/dd/yyyy");
                        }


                        if ((hours == "") && (minutes == ""))
                        {
                            duration = "00:00:00";
                        }
                        Label hrs = (Label)row.FindControl("lblhrs");
                        hours = hrs.Text.Trim().Trim().PadLeft(2, '0');
                        Label mins = (Label)row.FindControl("lblmins");
                        minutes = mins.Text.Trim().Trim().PadLeft(2, '0');
                        if ((minutes != null) && (hours != null) && (hours != "") && (minutes != ""))
                        {
                            duration = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                        }
                        else
                        {
                            duration = "00:00:00";
                        }

                        Label maxmrk = (Label)row.FindControl("lblmaxmarks");
                        new_max_mark = Convert.ToSingle(maxmrk.Text.ToString());
                        Label minmrk = (Label)row.FindControl("lblminmarks");
                        new_min_mark = Convert.ToSingle(minmrk.Text.ToString());

                        newMaxMinMark = Convert.ToString(new_max_mark) + "," + Convert.ToString(new_min_mark);
                        Session["MinMax"] = Convert.ToString(new_max_mark) + "," + Convert.ToString(new_min_mark);
                        string staffCode = Convert.ToString(Session["Staff_Code"].ToString());
                        string batch = Convert.ToString(subdetails[3]);
                        string degreeCode = Convert.ToString(subdetails[5]);
                        string semester = Convert.ToString(subdetails[6]);
                        string section = Convert.ToString(subdetails[4]);
                        string subno = Convert.ToString(subdetails[1]);
                        string strsec = "";
                        if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and sections='" + section + "'";
                        }

                        string staffSelectorValue = Convert.ToString(Session["StaffSelector"]);
                        selectedSubDetails = batch + "-" + degreeCode + "-" + semester + "-" + strsec + "-" + subno + "-" + staffCode + "-" + staffSelectorValue;

                        //--------------------------------------------------display the testname and retrieve the data from the fpreport
                        //int temp = Convert.ToInt32(testNo);
                        //d_check = daycheck(temp);
                        //if (datelocksetting == "1")
                        //{
                          
                        //}


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select single Test!')", true);
                        Div1.Visible = false;
                        divPopSpread.Visible = false;

                        //lblNote.Visible = false;
                        GridView3.Visible = false;
                        lblNote2.Visible = false;
                        break;
                    }



                }
            }
            if (check == 1)
            {
                txtRollOrRegChanhge();
                Div1.Visible = false;
                lblDegName.Text = Convert.ToString(Session["DegreeDet"]);
                divPopSpread.Visible = true;
                //lblNote.Visible = true;
                //txtRollOrReg.Text = "";
                //lblStuName.Text = "";
                lblErrorMsg.Visible = false;
                clearFields();
                //GridView3.Visible = false;
                lblNote2.Visible = false;
                GridReport.Visible = false;

            }
            else if (check == 0)
            {
                Div1.Visible = false;
                divPopSpread.Visible = false;
                //lblNote.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Test!')", true);
                txtRollOrReg.Text = "";
                lblStuName.Text = "";
                clearFields();
                GridView3.Visible = false;
                lblNote2.Visible = false;
            }
            rblStatus.SelectedIndex = 0;
            rblStatus_OnSelectedIndexChanged(sender, e);
            //loadReportSpread();========================


        }
        catch { }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        try
        {
            lblTot.Visible = false;
            lblGrandTotal.Visible = false;
            GridView3.Visible = false;
            int check = 0;
            foreach (GridViewRow row in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)row.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    check++;
                    if (check == 1)
                    {
                        Label tstnam = (Label)row.FindControl("lbltest");
                        string testName = tstnam.Text;
                        lblTestTitle.Text = testName;
                        Label tstno = (Label)row.FindControl("lblcriteriano");
                        string testNo = tstno.Text;
                        Label bind = (Label)row.FindControl("lblbind");
                        string bindsubdet = bind.Text;
                        int j = 0;
                        dicsubdetails.Clear();
                        dicsubdetails.Add(0, bindsubdet);
                        string[] subdetails = bindsubdet.Split('-');
                        string subName = Convert.ToString(subdetails[0]);
                        string SubCode = Convert.ToString(subdetails[2]);
                        lblSubName.Text = SubCode + " - " + subName;
                        string subNo = Convert.ToString(subdetails[1]);
                        selectedSubTest = subNo + "#" + testNo;
                        Label maxmrk = (Label)row.FindControl("lblmaxmarks");
                        double new_max_mark1 = Convert.ToSingle(maxmrk.Text.ToString());
                        lblMaxMark.Text = new_max_mark1.ToString();
                        string date = string.Empty;
                        string month = string.Empty;
                        string year = string.Empty;
                        string examDate = "";
                        string date1 = string.Empty;
                        string month1 = string.Empty;
                        string year1 = string.Empty;
                        string entryDate = "";
                        string hours = string.Empty;
                        string minutes = string.Empty;
                        string duration = "";
                        float new_max_mark = 0;
                        float new_min_mark = 0;

                        Label dat = (Label)row.FindControl("lblexamdate");
                        date = dat.Text.Trim().PadLeft(2, '0');
                        Label mon = (Label)row.FindControl("lblexammonth");
                        month = mon.Text.Trim().PadLeft(2, '0');
                        Label yr = (Label)row.FindControl("lblexamyear");
                        year = yr.Text.Trim();
                        if ((date != "") && (month != "") && (year != "") && (date != null) && (month != null) && (year != null))
                        {
                            examDate = month + "/" + date + "/" + year;
                        }
                        else
                        {
                            examDate = string.Empty;
                        }
                        Label endat = (Label)row.FindControl("lblentrydate");
                        date1 = endat.Text.Trim().PadLeft(2, '0');
                        Label enmon = (Label)row.FindControl("lblentrymonth");
                        month1 = enmon.Text.Trim().PadLeft(2, '0');
                        Label enyr = (Label)row.FindControl("lblentryyear");
                        year1 = enyr.Text.Trim();
                        if ((date1 != "") && (month1 != "") && (year1 != "") && (date1 != null) && (month1 != null) && (year1 != null))
                        {
                            entryDate = month1 + "/" + date1 + "/" + year1;
                        }
                        else
                        {
                            entryDate = DateTime.Now.ToString("MM/dd/yyyy");
                        }


                        if ((hours == "") && (minutes == ""))
                        {
                            duration = "00:00:00";
                        }
                        Label hrs = (Label)row.FindControl("lblhrs");
                        hours = hrs.Text.Trim().Trim().PadLeft(2, '0');
                        Label mins = (Label)row.FindControl("lblmins");
                        minutes = mins.Text.Trim().Trim().PadLeft(2, '0');
                        if ((minutes != null) && (hours != null) && (hours != "") && (minutes != ""))
                        {
                            duration = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                        }
                        else
                        {
                            duration = "00:00:00";
                        }

                        Label maxmrk1 = (Label)row.FindControl("lblmaxmarks");
                        new_max_mark = Convert.ToSingle(maxmrk1.Text.ToString());
                        Label minmrk = (Label)row.FindControl("lblminmarks");
                        new_min_mark = Convert.ToSingle(minmrk.Text.ToString());

                        newMaxMinMark = Convert.ToString(new_max_mark) + "," + Convert.ToString(new_min_mark);

                        string staffCode = Convert.ToString(Session["Staff_Code"].ToString());
                        string batch = Convert.ToString(subdetails[3]);
                        string degreeCode = Convert.ToString(subdetails[5]);
                        string semester = Convert.ToString(subdetails[6]);
                        string section = Convert.ToString(subdetails[4]);
                        string subno = Convert.ToString(subdetails[1]);
                        string strsec = "";
                        if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and sections='" + section + "'";
                        }

                        string staffSelectorValue = Convert.ToString(Session["StaffSelector"]);
                        selectedSubDetails = batch + "-" + degreeCode + "-" + semester + "-" + strsec + "-" + subno + "-" + staffCode + "-" + staffSelectorValue;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select single Test!')", true);
                        Div1.Visible = false;
                        divPopSpread.Visible = false;

                        //lblNote.Visible = false;
                        GridView3.Visible = false;
                        lblNote2.Visible = false;
                        break;
                    }
                }
            }
            if (check == 1)
            {
                loadReportSpread();
                //Div1.Visible = true;
                //divPopSpread.Visible = false;
                ////lblNote.Visible = true;
                //txtRollOrReg.Text = "";
                //lblStuName.Text = "";
                //lblErrorMsg.Visible = false;
                //clearFields();
                //GridView3.Visible = false;
                lblNote2.Visible = true;
            }
            else if (check == 0)
            {
                Div1.Visible = false;
                divPopSpread.Visible = false;
                //lblNote.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Test!')", true);
                txtRollOrReg.Text = "";
                lblStuName.Text = "";
                clearFields();
                GridView3.Visible = false;
                lblNote2.Visible = false;
            }
            //rblStatus.SelectedIndex = 0;
            //rblStatus_OnSelectedIndexChanged(sender, e);
            //========================


        }
        catch { }
    }

    #endregion

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            lblErrorMsg.Visible = false;
            string staff_code = Convert.ToString(Session["staff_code"]).Trim();
            string exam_code = string.Empty;
            string bindnote = string.Empty;
            string hrs2 = string.Empty;
            string mins2 = string.Empty;
            if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
            {
                dtest.Columns.Add("test");
                dtest.Columns.Add("criteria_no");
                dtest.Columns.Add("examcode");
                dtest.Columns.Add("bind");
                dtest.Columns.Add("bindNote");
                dtest.Columns.Add("exmdt");
                dtest.Columns.Add("examdate");
                dtest.Columns.Add("exammonth");
                dtest.Columns.Add("examyear");
                dtest.Columns.Add("entdt");
                dtest.Columns.Add("entrydate");
                dtest.Columns.Add("entrymonth");
                dtest.Columns.Add("entryyear");
                dtest.Columns.Add("durationhours");
                dtest.Columns.Add("durationhrs");
                dtest.Columns.Add("durationmins");
                dtest.Columns.Add("max_mark");
                dtest.Columns.Add("min_mark");
                string exam_date1 = string.Empty;
                string entry_date1 = string.Empty;
                string duration_hrs = string.Empty;
                string datelock = GetFunction("select value from master_settings where settings='Cam Date Lock' and " + grouporusercode + "");
                if (datelock.Trim() != "")
                {
                    datelocksetting = datelock;
                }
                else
                {
                    datelocksetting = "0";
                }
                int ar;
                ar = rowIndex;
                if (ar != -1)
                {
                    string sqlStr = string.Empty;
                    string batch = string.Empty;
                    string degreeCode = string.Empty;
                    string semester = string.Empty;
                    string section = string.Empty;
                    string subno = string.Empty;
                    string strsec = string.Empty;


                    string batchYearQry = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year";
                    DataSet dsBatchYear = da.select_method_wo_parameter(batchYearQry, "Text");

                    int batchYearsCount = 0;
                    int.TryParse(Convert.ToString(dsBatchYear.Tables[0].Rows.Count), out batchYearsCount);
                    string[] arrayBatchyear = new string[batchYearsCount + 1];

                    if (dsBatchYear.Tables.Count > 0 && dsBatchYear.Tables[0].Rows.Count > 0)
                    {
                        int r = 0;
                        for (r = 0; r < dsBatchYear.Tables[0].Rows.Count; r++)
                        {
                            arrayBatchyear[r] = Convert.ToString(dsBatchYear.Tables[0].Rows[r]["batch_year"]);
                        }
                        int curr_year = Convert.ToInt16(DateTime.Today.Year);

                        if (arrayBatchyear.Contains(Convert.ToString(curr_year)) != true)
                        {
                            arrayBatchyear[r] = Convert.ToString(curr_year);
                        }

                    }


                    Label bat = (Label)GridView1.Rows[ar].FindControl("lblbatch");
                    batch = bat.Text.Trim();
                    Label degcode = (Label)GridView1.Rows[ar].FindControl("lbldegreecode");
                    degreeCode = degcode.Text;
                    Label sem = (Label)GridView1.Rows[ar].FindControl("lblsem");
                    semester = sem.Text.Trim();
                    Label sec = (Label)GridView1.Rows[ar].FindControl("lblsection");
                    section = sec.Text.Trim();
                    Label subjectno = (Label)GridView1.Rows[ar].FindControl("lblsubno");
                    subno = subjectno.Text;
                    Label subjectname = (Label)GridView1.Rows[ar].FindControl("lblsubject");
                    string subnam = subjectname.Text;
                    Label subjectcode = (Label)GridView1.Rows[ar].FindControl("lblsubcode");
                    string subcod = subjectcode.Text;
                    Label degName = (Label)GridView1.Rows[ar].FindControl("lbldegree");
                    string degreeName = degName.Text;
                    string secv = string.Empty;
                    if (!string.IsNullOrEmpty(section))
                        secv = " -" + section;
                    else
                        secv = string.Empty;

                    Session["DegreeDet"] = batch + "-" + degreeName + "-" + semester + secv;

                    lblTestName.InnerText = "Test Details - " + subcod + " - " + subnam + " ";
                    if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + section + "'";
                    }

                    string bind = string.Empty;
                    if (subnam.Contains('-'))
                        subnam = subnam.Replace('-', ' ');

                    bind = subnam + "-" + subno + "-" + subcod + "-" + batch + "-" + section + "-" + degreeCode + "-" + semester;

                    //------------------------------------------- Query for display the Testname,max,min marks,date and duration in the spread2-spreadTestDetails
                    if (staff_code != "")
                    {

                        //sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " ' and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='') order by criteria";

                        sqlStr = "select distinct criteria,exam_type.criteria_no,exam_type.max_mark,exam_type.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + "   " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " ' and batch_year = " + batch.ToString() + " and staff_code= '" + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal,exam_type  where exam_type.criteria_no=CriteriaForInternal.criteria_no and exam_type.subject_no='" + subno.ToString() + "' and syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='') order by criteria";

                    }
                    else
                    {
                        sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='') order by criteria";
                    }
                    DataSet dsExamDetails = da.select_method_wo_parameter(sqlStr, "text");

                    string examAllDetailsQry = "select * from exam_type where subject_no='" + subno.ToString() + "' " + strsec.ToString() + "";
                    DataSet dsExamAllDetails = da.select_method_wo_parameter(examAllDetailsQry, "text");
                    if (dsExamDetails.Tables.Count > 0 && dsExamDetails.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsExamDetails.Tables[0].Rows.Count; i++)
                        {

                            string display = string.Empty;
                            string criteria_no = string.Empty;
                            string criteria = string.Empty;
                            float max_mark = 0;
                            float min_mark = 0;
                            string exmday = string.Empty;
                            string exmmon = string.Empty;
                            string exmyr = string.Empty;
                            string entryday = string.Empty;
                            string entrymon = string.Empty;
                            string entryyr = string.Empty;
                            int hour = 0;
                            int min = 0;
                            int seconds = 0;

                            criteria = Convert.ToString(dsExamDetails.Tables[0].Rows[i]["criteria"]);
                            criteria_no = Convert.ToString(dsExamDetails.Tables[0].Rows[i]["criteria_no"]);

                            dsExamAllDetails.Tables[0].DefaultView.RowFilter = " criteria_no='" + criteria_no + "'";
                            DataView dvexm = dsExamAllDetails.Tables[0].DefaultView;

                            if (dvexm.Count > 0)
                            {
                                max_mark = Convert.ToSingle(dvexm[0]["max_mark"].ToString());
                                min_mark = Convert.ToSingle(dvexm[0]["min_mark"].ToString());
                            }
                            else
                            {
                                max_mark = Convert.ToSingle(dsExamDetails.Tables[0].Rows[i]["max_mark"]);
                                min_mark = Convert.ToSingle(dsExamDetails.Tables[0].Rows[i]["min_mark"]);
                            }

                            int temp = Convert.ToInt32(criteria_no);
                            day_check = daycheck(temp);

                            if (dvexm.Count > 0)
                            {
                                string resExamDate = string.Empty;
                                string resEntryDate = string.Empty;
                                string resMaxMrk = string.Empty;
                                string resMinMrk = string.Empty;
                                string resDuration = string.Empty;
                                string resNewMaxmMrk = string.Empty;
                                string resNewMinMrk = string.Empty;
                                string examDate = string.Empty;
                                string srtprd = string.Empty;
                                string endprd = string.Empty;

                                examDate = dvexm[0]["exam_date"].ToString();
                                exam_date1 = examDate;
                                if (examDate != "")
                                {
                                    string[] examDateSplit = examDate.Split(new char[] { ' ' });
                                    string[] formatetime = examDateSplit[0].Split(new char[] { '/' });
                                    string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
                                    if (formatetime[1].Length == 1)
                                    {
                                        formatetime[1] = "0" + formatetime[1];
                                    }
                                    if (formatetime[0].Length == 1)
                                    {
                                        formatetime[0] = "0" + formatetime[0];
                                    }
                                    exmday = formatetime[1].ToString().Trim().PadLeft(2, '0');
                                    exmmon = formatetime[0].ToString().Trim().PadLeft(2, '0');
                                    exmyr = formatetime[2].ToString();
                                }
                                else
                                {
                                    string examconcat = string.Empty;
                                    exmday = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                    exmmon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                    exmyr = DateTime.Now.Year.ToString();
                                }
                                string entryDate = string.Empty;
                                entryDate = dvexm[0]["entry_date"].ToString();
                                entry_date1 = entryDate;
                                if (entryDate != "")
                                {
                                    string[] entryDateSplit = entryDate.Split(new char[] { ' ' });
                                    string[] formatentrytime = entryDateSplit[0].Split(new char[] { '/' });
                                    string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
                                    if (formatentrytime[1].Length == 1)
                                    {
                                        formatentrytime[1] = "0" + formatentrytime[1];
                                    }
                                    if (formatentrytime[0].Length == 1)
                                    {
                                        formatentrytime[0] = "0" + formatentrytime[0];
                                    }
                                    entryday = formatentrytime[1].ToString().Trim().PadLeft(2, '0');
                                    entrymon = formatentrytime[0].ToString().Trim().PadLeft(2, '0');
                                    entryyr = formatentrytime[2].ToString();

                                }
                                else
                                {
                                    string entryconcat = string.Empty;
                                    entryday = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                    entrymon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                    entryyr = DateTime.Now.Year.ToString();

                                }
                                string mxmrk = dvexm[0]["max_mark"].ToString();
                                max_mark = Convert.ToInt32(mxmrk);
                                string mimrk = dvexm[0]["min_mark"].ToString();
                                min_mark = Convert.ToInt32(mimrk);
                                string duration = string.Empty;
                                string examDurationNew = Convert.ToString(dvexm[0]["durationNew"]).Trim();
                                string examDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                TimeSpan tsDuration = new TimeSpan(0, 0, 0);
                                duration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                duration_hrs = duration;
                                if (duration.ToString().Trim() != "")
                                {
                                    string[] splitdur = duration.Split(new char[] { ':' });
                                    if (splitdur.GetUpperBound(0) == 1)
                                    {
                                        if (splitdur[1].ToString() != "")
                                        {
                                            mins2 = splitdur[1].Trim().ToString();
                                        }
                                    }
                                }

                                string[] durationSplit = examDurationNew.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (durationSplit.Length > 0)
                                {
                                    if (durationSplit.Length >= 3)
                                    {
                                        int.TryParse(durationSplit[0].Trim(), out hour);
                                        int.TryParse(durationSplit[1].Trim(), out min);
                                        int.TryParse(durationSplit[2].Trim(), out seconds);
                                    }
                                    else if (durationSplit.Length == 2)
                                    {
                                        int tempnew1 = 0;
                                        int tempnew2 = 0;
                                        int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                        int.TryParse(durationSplit[1].Trim(), out tempnew2);

                                        if (tempnew1 <= 12 || tempnew1 <= 23)
                                        {
                                            hour = tempnew1;
                                        }
                                        else if (tempnew1 < 60)
                                        {
                                            min = tempnew1;
                                        }
                                        if (tempnew2 <= 59)
                                        {
                                            min = tempnew2;
                                        }
                                    }
                                    else if (durationSplit.Length == 1)
                                    {
                                        int tempnew1 = 0;
                                        int.TryParse(durationSplit[0].Trim(), out tempnew1);

                                        if (tempnew1 <= 12 || tempnew1 <= 23)
                                        {
                                            hour = tempnew1;
                                        }
                                        else if (tempnew1 < 60)
                                        {
                                            min = tempnew1;
                                        }
                                    }
                                }
                                if (hour == 0 && min == 0 && seconds == 0)
                                {
                                    durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (durationSplit.Length > 0)
                                    {
                                        if (durationSplit.Length >= 3)
                                        {
                                            int.TryParse(durationSplit[0].Trim(), out hour);
                                            int.TryParse(durationSplit[1].Trim(), out min);
                                            int.TryParse(durationSplit[2].Trim(), out seconds);
                                        }
                                        else if (durationSplit.Length == 2)
                                        {
                                            int tempnew1 = 0;
                                            int tempnew2 = 0;
                                            int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                            int.TryParse(durationSplit[1].Trim(), out tempnew2);

                                            if (tempnew1 <= 12 || tempnew1 <= 23)
                                            {
                                                hour = tempnew1;
                                            }
                                            else if (tempnew1 < 60)
                                            {
                                                min = tempnew1;
                                            }
                                            if (tempnew2 <= 59)
                                            {
                                                min = tempnew2;
                                            }
                                        }
                                        else if (durationSplit.Length == 1)
                                        {
                                            int tempnew1 = 0;
                                            int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                            if (tempnew1 <= 12 || tempnew1 <= 23)
                                            {
                                                hour = tempnew1;
                                            }
                                            else if (tempnew1 < 60)
                                            {
                                                min = tempnew1;
                                            }
                                        }
                                    }
                                }
                                tsDuration = new TimeSpan(hour, min, seconds);
                                resExamDate = dvexm[0]["exam_date"].ToString();
                                resEntryDate = dvexm[0]["entry_date"].ToString();
                                resDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                resMaxMrk = dvexm[0]["max_mark"].ToString();
                                resMinMrk = dvexm[0]["min_mark"].ToString();
                                resNewMaxmMrk = dvexm[0]["new_maxmark"].ToString();
                                resNewMinMrk = dvexm[0]["new_minmark"].ToString();

                                string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                                hrs2 = hour.ToString().Trim().PadLeft(2, '0');
                                mins2 = min.ToString().Trim().PadLeft(2, '0');

                                exam_code = dvexm[0]["exam_code"].ToString();
                                bindnote = bind + ";" + resExamDate + "-" + resEntryDate + "-" + resDuration + "-" + resNewMaxmMrk + "-" + resMaxMrk + "-" + resNewMinMrk + "-" + resMinMrk;

                                try
                                {
                                    if (Session["Staff_Code"].ToString().Trim() != "")
                                    {
                                        string examlock = dvexm[0]["islock"].ToString();
                                        if (examlock.Trim().ToLower() == "true" || examlock.Trim() == "1")
                                        {
                                            string elockdate = dvexm[0]["elockdate"].ToString();
                                            if (elockdate.Trim() != "")
                                            {
                                                DateTime dte = Convert.ToDateTime(elockdate);
                                                DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                                                if (dte < dtnow)
                                                {
                                                    day_check = false;
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                string examconcat = string.Empty;
                                exmday = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                exmmon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                exmyr = DateTime.Now.Year.ToString();
                                entryday = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                entrymon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                entryyr = DateTime.Now.Year.ToString();

                            }

                            dr1 = dtest.NewRow();
                            dr1["test"] = criteria.ToString();
                            dr1["criteria_no"] = criteria_no.ToString();
                            dr1["examcode"] = exam_code;
                            dr1["bind"] = bind.ToString();
                            dr1["bindNote"] = bindnote;
                            dr1["exmdt"] = exam_date1;
                            dr1["examdate"] = exmday;
                            dr1["exammonth"] = exmmon;
                            dr1["examyear"] = exmyr;
                            dr1["entdt"] = entry_date1;
                            dr1["entrydate"] = entryday;
                            dr1["entrymonth"] = entrymon;
                            dr1["entryyear"] = entryyr;
                            dr1["durationhours"] = duration_hrs;
                            dr1["durationhrs"] = hrs2;
                            dr1["durationmins"] = mins2;
                            dr1["max_mark"] = max_mark;
                            dr1["min_mark"] = min_mark;
                            dtest.Rows.Add(dr1);
                        }
                        GridView2.DataSource = dtest;
                        GridView2.DataBind();
                    }
                    else
                    {
                        GridView2.Visible = false;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "No Test Conducted For The Subject";
                    }
                }
                
                if (GridView2.Rows.Count > 0)
                {
                }
                else
                {
                    Div1.Visible = false;
                    divPopSpread.Visible = false;

                    //lblNote.Visible = false;
                    GridView2.Visible = false;
                    lblNote2.Visible = false;
                }
                if (!day_check)
                {
                    GridView2.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Entry Date Locked!')", true);
                }
                else
                    GridView2.Enabled = true;
                

                Cellclick = false;
                testDetailsLblDiv.Visible = true;
                btnok.Visible = true;
                chkretest.Visible = true;
                btnReport.Visible = false;
                // lblNote.Visible = false;
                GridView2.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drexmdt = (DropDownList)e.Row.FindControl("ddlexamdate");
                DropDownList drexmmon = (DropDownList)e.Row.FindControl("ddlexammonth");
                DropDownList drexyr = (DropDownList)e.Row.FindControl("ddlexamyear");
                DropDownList drentrydt = (DropDownList)e.Row.FindControl("ddlentrydate");
                DropDownList drentrymon = (DropDownList)e.Row.FindControl("ddlentrymonth");
                DropDownList drentryyr = (DropDownList)e.Row.FindControl("ddlentryyear");
                DropDownList drhrs = (DropDownList)e.Row.FindControl("ddlhrs");
                DropDownList drmins = (DropDownList)e.Row.FindControl("ddlmins");

                drexmdt.Items.Insert(0, "");
                drexmmon.Items.Insert(0, "");
                drentrydt.Items.Insert(0, "");
                drentrymon.Items.Insert(0, "");
                drhrs.Items.Insert(0, "");
                drmins.Items.Insert(0, "");
                for (int i = 1; i < 13; i++)
                {
                    string item = i.ToString();
                    if (item.Length < 2)
                    {
                        item = "0" + item;
                    }
                    drexmmon.Items.Add(item);
                    drentrymon.Items.Add(item);
                }
                for (int i1 = 0; i1 < 25; i1++)
                {
                    string item = i1.ToString();
                    if (item.Length < 2)
                    {
                        item = "0" + item;
                    }
                    drhrs.Items.Add(item);
                }
                for (int i2 = 0; i2 <= 12; i2++)
                {
                    int cal = i2 * 5;
                    string item = cal.ToString();
                    if (item.Length < 2)
                    {
                        item = "0" + item;
                    }
                    drmins.Items.Add(item);
                }
                for (int i3 = 0; i3 <= 31; i3++)
                {
                    string item = i3.ToString();
                    if (item.Length < 2)
                    {
                        item = "0" + item;
                    }
                    drexmdt.Items.Add(item);
                    drentrydt.Items.Add(item);
                }
                string batch = "select distinct batch_year as batch_year  from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar'";
                DataSet dbat = da.select_method_wo_parameter(batch, "text");
                if (dbat.Tables.Count > 0 && dbat.Tables[0].Rows.Count > 0)
                {
                    drexyr.DataSource = dbat;
                    drexyr.DataValueField = "batch_year";
                    drexyr.DataTextField = "batch_year";
                    drexyr.Items.Insert(0, " ");
                    drexyr.DataBind();

                    drentryyr.DataSource = dbat;
                    drentryyr.DataValueField = "batch_year";
                    drentryyr.DataTextField = "batch_year";
                    drentryyr.Items.Insert(0, " ");
                    drentryyr.DataBind();
                }

                Label exdt = e.Row.FindControl("lblexamdate") as Label;
                string exdtt = exdt.Text;
                if (exdtt == "")
                    drexmdt.Items[0].Selected = true;
                else
                    drexmdt.Items.FindByText(exdtt).Selected = true;

                Label exmon = e.Row.FindControl("lblexammonth") as Label;
                string exammon = exmon.Text;
                if (exammon == "")
                    drexmmon.Items[0].Selected = true;
                else
                    drexmmon.Items.FindByText(exammon).Selected = true;

                Label exyr = e.Row.FindControl("lblexamyear") as Label;
                string examyr = exyr.Text;
                if (examyr == "")
                    drexyr.Items[0].Selected = true;
                else
                    drexyr.Items.FindByText(examyr).Selected = true;

                Label entrdt = e.Row.FindControl("lblentrydate") as Label;
                string entdat = entrdt.Text;
                if (entdat == "")
                    drentrydt.Items[0].Selected = true;
                else
                    drentrydt.Items.FindByText(entdat).Selected = true;

                Label entrmon = e.Row.FindControl("lblentrymonth") as Label;
                string entrymon = entrmon.Text;
                if (entrymon == "")
                    drentrymon.Items[0].Selected = true;
                else
                    drentrymon.Items.FindByText(entrymon).Selected = true;

                Label entryr = e.Row.FindControl("lblentryyear") as Label;
                string entryyr = entryr.Text;
                if (entryyr == "")
                    drentryyr.Items[0].Selected = true;
                else
                    drentryyr.Items.FindByText(entryyr).Selected = true;

                Label durhr = e.Row.FindControl("lblhrs") as Label;
                string durhrs = durhr.Text;
                if (durhrs.Length == 1)
                    durhrs = "0" + durhrs;
                if (durhrs == "")
                    drhrs.Items[0].Selected = true;
                else
                    drhrs.Items.FindByText(durhrs).Selected = true;

                Label durmin = e.Row.FindControl("lblmins") as Label;
                string durmins = durmin.Text;
                if (durmins.Length == 1)
                    durmins = "0" + durmins;
                if (durmins == "")
                    drmins.Items[0].Selected = true;
                else
                    drmins.Items.FindByText(durmins).Selected = true;


                if (datelocksetting.Trim() == "1")
                {
                    e.Row.Cells[6].CssClass = "Locked";
                    e.Row.Cells[7].CssClass = "Locked";
                    e.Row.Cells[8].CssClass = "Locked";
                }

            }
        }
        catch
        {
        }
    }
    protected void rblStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridView3.Enabled = true;
            if (rblStatus.SelectedIndex.Equals(1) || rblStatus.SelectedIndex.Equals(2))
            {
                GridView3.Enabled = false;
                clearFields();
            }
            else
            {
                GridView3.Enabled = true;
            }
        }
        catch { }
    }
    protected void gridview4_onrowdatabound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["Rollflag"].ToString() == "0")
            {
                e.Row.Cells[1].Visible = false;
                GridView3.Columns[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                e.Row.Cells[2].Visible = false;
                GridView3.Columns[2].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                e.Row.Cells[3].Visible = false;
                GridView3.Columns[3].Visible = false;
            }
            if (Session["Admisionflag"].ToString() == "0")
            {
                e.Row.Cells[5].Visible = false;
                GridView3.Columns[5].Visible = false;
            }
            if (Session["Appflag"].ToString() == "0")
            {
                e.Row.Cells[4].Visible = false;
                GridView3.Columns[4].Visible = false;
            }
        }
    }
  
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        funconn.Close();
        funconn.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, funconn);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = funconn;
        drnew = funcmd.ExecuteReader();
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
    public bool checkSchoolOrCollege()
    {
        try
        {
            DataSet schoolds = new DataSet();
            string schoolvalue = GetFunction("select value from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "");

            if (schoolvalue.Trim() == "0")
            {
                isSchool = true;
            }
            else
            {
                isSchool = false;
            }

            return isSchool;
        }
        catch
        {
            return false;
        }
    }
    public void clearFields()
    {

    }
    protected float checkMark(float mark)
    {
        try
        {
            if (mark == -1)
                return 0;
            else
                return mark;
        }
        catch
        {
            return 0;
        }
    }
    protected string checkStatus(string mark)
    {
        try
        {
            if (mark == "-1")
                return "AAA";
            else if (mark == "-20")  //added by Mullai
                return " ";
            else
                return mark;
        }
        catch
        {
            return null;
        }
    }
    //added by Mullai
    protected string checkStatus1(string mark)
    {
        try
        {
            if (mark == "-20")
                return " ";
            else
                return mark;
        }
        catch
        {
            return null;
        }
    }
    public void subjecttestDetails()
    {
        try
        {
            int val = 0;
            int check = 0;
            foreach (GridViewRow row in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)row.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    check++;
                    if (check == 1)
                    {
                        Label tstnam = (Label)row.FindControl("lbltest");
                        string testName = tstnam.Text;
                        lblTestTitle.Text = testName;
                        Label tstno = (Label)row.FindControl("lblcriteriano");
                        string testNo = tstno.Text;
                        Label bind = (Label)row.FindControl("lblbind");
                        string bindsubdet = bind.Text;
                        string[] subdetails = bindsubdet.Split('-');
                        string subName = Convert.ToString(subdetails[0]);
                        string subCode = Convert.ToString(subdetails[2]);
                        lblSubName.Text = subCode + " - " + subName;
                        string subNo = Convert.ToString(subdetails[1]);
                        selectedSubTest = subNo + "#" + testNo;

                        Label maxmrk = (Label)row.FindControl("lblmaxmarks");
                        double new_max_mark1 = Convert.ToDouble(maxmrk.Text.ToString());
                        lblMaxMark.Text = new_max_mark1.ToString();

                    }
                }
            }
        }
        catch
        {

        }
    }
    public void selectSubject()
    {
        try
        {
            int check = 0;
            foreach (GridViewRow row in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)row.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    check++;
                    if (check == 1)
                    {
                        Label tstnam = (Label)row.FindControl("lbltest");
                        string testName = tstnam.Text;
                        lblTestTitle.Text = testName;
                        Label tstno = (Label)row.FindControl("lblcriteriano");
                        string testNo = tstno.Text;
                        Label bind = (Label)row.FindControl("lblbind");
                        string bindsubdet = bind.Text;
                        int j = 0;
                        dicsubdetails.Clear();
                        dicsubdetails.Add(0, bindsubdet);
                    }
                }
            }

            string staffCode = string.Empty;
            string batch = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string section = string.Empty;
            string subno = string.Empty;
            string strsec = string.Empty;
            string subdet = string.Empty;
            if (dicsubdetails.Count > 0)
            {
                foreach (KeyValuePair<int, string> dicdet in dicsubdetails)
                {
                    subdet = dicdet.Value;
                }
            }
            if (subdet != "")
            {
                string[] subjectdet = subdet.Split('-');
                staffCode = Convert.ToString(Session["Staff_Code"].ToString());
                batch = Convert.ToString(subjectdet[3]);
                degreeCode = Convert.ToString(subjectdet[5]);
                semester = Convert.ToString(subjectdet[6]);
                section = Convert.ToString(subjectdet[4]);
                subno = Convert.ToString(subjectdet[1]);
                strsec = "";
                if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + section + "'";
                }
            }

            string staffSelectorValue = Convert.ToString(Session["StaffSelector"]);
            selectedSubDetails = batch + "-" + degreeCode + "-" + semester + "-" + strsec + "-" + subno + "-" + staffCode + "-" + staffSelectorValue;
        }
        catch
        {
        }
    }
    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (isSchool == true)
            {
                e.Row.Cells[1].Text = "Year";
                e.Row.Cells[3].Text = "Term";
            }
            else
            {
                e.Row.Cells[1].Text = "Batch Year";
                e.Row.Cells[3].Text = "Semester";
            }
        }
    }
    protected void ddlexamdate_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlexammonth_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlexamyear_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentrydate_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentrymonth_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentryyear_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlhrs_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlmins_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void btnokclk_Click(object sender, EventArgs e)
    {
        Session["Row"] = null;
        Session["Examtype"] = null;
        divpopalter.Visible = false;
        lblaltermsgs.Visible = false;
    }
    protected void btnclosespread_OnClick(object sender, EventArgs e)
    {
        divPopSpread.Visible = false;
        Session["Row"] = null;
        Session["Examtype"] = null;
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        try
        {

            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string datelock = GetFunction("select value from master_settings where settings='Cam Date Lock' and " + grouporusercode + "");
            if (datelock.Trim() != "")
            {
                datelocksetting = datelock;
            }
            else
            {
                datelocksetting = "0";
            }
            if (datelocksetting != "1")
            {
                double MaxMar = 0;
                double grandtoto = 0;
                double.TryParse(lblMaxMark.Text, out MaxMar);
                double.TryParse(lblGrandTotal.Text, out grandtoto);
                if (grandtoto <= MaxMar)
                {
                    if (txtRollOrReg.Text != "")
                    {
                        string appNo = string.Empty;
                        string subNo = string.Empty;
                        string criteriaNo = string.Empty;
                        string qry = string.Empty;
                        float grandTotal = 0;
                        string staffCode = Convert.ToString(Session["staff_code"]).Trim();
                        string rollOrReg = txtRollOrReg.Text;
                        subjecttestDetails();
                        string[] arr = selectedSubTest.Split('#');
                        subNo = Convert.ToString(arr[0]);
                        criteriaNo = Convert.ToString(arr[1]);
                        string colname = string.Empty;
                        string MarkVal = string.Empty;
                        string ForMate = da.GetFunction("select value from Master_Settings where settings='SaveCoFormate'");
                        //string ForMate = Convert.ToString(Session["SaveCoFormate"]);
                        string examCodeNew = lblExamCode.Text;
                        //string rollOrReg = txtRollOrReg.Text;
                        appNo = da.GetFunction("select app_no from registration  where roll_no ='" + rollOrReg + "' and cc=0 and delflag=0 and exam_flag<>'debar'");
                        //appNo = Convert.ToString((GridView3.Rows[0].FindControl("lblappno") as Label).Text);
                        float grandTotal11 = 0;
                        int qryStatus = 0;
                        string exmco = lblExamCode.Text;
                        string islab = da.GetFunction("select ss.Lab from subject s,sub_sem ss where s.subType_no=ss.subType_no and subject_no='" + subNo + "'");

                        if (appNo != "0" && appNo.ToLower() != "operation")
                        {
                            string del = string.Empty;
                            //if (!chkretest.Checked)
                            //     del = "delete NewInternalMarkEntry where app_no='" + appNo + "' and examCode='" + examCode + "'";
                            //else

                            //int delete = da.update_method_wo_parameter(del, "text");
                            int qVal = 0;
                            if (Session["Examtype"] == null || string.IsNullOrEmpty(Convert.ToString(Session["Examtype"])))
                            {
                                string bindnote = string.Empty;
                                string exam_date = string.Empty;
                                string entry_date = string.Empty;
                                string duration = string.Empty;
                                string max_mark = string.Empty;
                                string min_mark = string.Empty;
                                foreach (GridViewRow gr in GridView2.Rows)
                                {
                                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                                    if (chk.Checked == true)
                                    {
                                        Label bindnot = (Label)gr.FindControl("lblbind");
                                        bindnote = bindnot.Text;
                                        Label exmdt = (Label)gr.FindControl("lblexmdt");
                                        exam_date = exmdt.Text;
                                        Label entdt = (Label)gr.FindControl("lblentdt");
                                        entry_date = entdt.Text;
                                        Label durahrs = (Label)gr.FindControl("lbldurhrs");
                                        duration = durahrs.Text;
                                        Label mxmrk = (Label)gr.FindControl("lblmaxmarks");
                                        max_mark = mxmrk.Text;
                                        Label mimrk = (Label)gr.FindControl("lblminmarks");
                                        min_mark = mimrk.Text;
                                    }
                                }
                                string[] arrBindNote = bindnote.Split('-');
                                string batchYear = arrBindNote[3];
                                string sec = arrBindNote[4];

                                string[] arrNewMark = newMaxMinMark.Split(',');
                                if (arrNewMark.Length == 1)
                                {
                                    if (Session["MinMax"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["MinMax"])))
                                        arrNewMark = Convert.ToString(Session["MinMax"]).Split(',');
                                }
                                float newMaxMark = 0;
                                float newMinMark = 0;
                                if (arrNewMark.Length == 1)
                                {
                                    newMaxMark = 0;
                                    newMinMark = 0;
                                }
                                else
                                {
                                    float.TryParse(arrNewMark[0], out newMaxMark);
                                    float.TryParse(arrNewMark[1], out newMinMark);
                                }
                                string startPeriod = "";
                                string endPeriod = "";

                                hat.Clear();
                                hat.Add("criteria_no", criteriaNo);
                                hat.Add("staff_code", staffCode);
                                hat.Add("subject_no", subNo);
                                hat.Add("duration", duration);
                                hat.Add("entry_date", entry_date);
                                hat.Add("exam_date", exam_date);
                                hat.Add("batch_year", batchYear);
                                hat.Add("max_mark", max_mark);
                                hat.Add("min_mark", min_mark);
                                hat.Add("sections", sec);
                                hat.Add("new_maxmark", newMaxMark);
                                hat.Add("new_minmark", newMinMark);
                                hat.Add("start_period", startPeriod);
                                hat.Add("end_period", endPeriod);
                                int insert = da.insert_method("sp_ins_upd_cam_exam_type_dead", hat, "sp");
                                if (insert != 0)
                                    Session["Examtype"] = "1";
                            }

                            if (ForMate == "1")
                            {
                                DataTable dtSubSubject = dir.selectDataTable("select * from subsubjectTestDetails where examCode='" + exmco + "'");
                                if (dtSubSubject.Rows.Count > 0 && gridLab.Visible == true)
                                {
                                    foreach (GridViewRow grid in gridLab.Rows)
                                    {
                                        string mark = string.Empty;
                                        string app = Convert.ToString((grid.FindControl("lblAppNo") as Label).Text);
                                        string MasterId = Convert.ToString((grid.FindControl("lblSubId") as Label).Text);
                                        string exmCode = Convert.ToString((grid.FindControl("lblExamCode") as Label).Text);
                                        if (rblStatus.SelectedIndex.Equals(0))
                                        {
                                            mark = Convert.ToString((grid.FindControl("txttest") as TextBox).Text);
                                        }
                                        else if (rblStatus.SelectedIndex.Equals(1))
                                        {
                                            mark = "-1";
                                        }
                                        else
                                            mark = "-16";

                                        if (string.IsNullOrEmpty(mark))
                                            mark = "-20";
                                        if (!chkretest.Checked)//
                                            qryStatus = da.update_method_wo_parameter("if not exists(Select * from subSubjectWiseMarkEntry where subjectid='" + MasterId + "' and appNo='" + app + "') Insert into subSubjectWiseMarkEntry(appNo,subjectid,testmark) values('" + app + "','" + MasterId + "','" + mark + "') else update subSubjectWiseMarkEntry  SET testmark='" + mark + "' where subjectid='" + MasterId + "' and appNo='" + app + "'", "text");
                                        else
                                            qryStatus = da.update_method_wo_parameter("if not exists(Select * from subSubjectWiseMarkEntry where subjectid='" + MasterId + "' and appNo='" + app + "') Insert into subSubjectWiseMarkEntry(appNo,subjectid,retestmark) values('" + app + "','" + MasterId + "','" + mark + "') else update subSubjectWiseMarkEntry  SET retestmark='" + mark + "' where subjectid='" + MasterId + "' and appNo='" + app + "'", "text");
                                        float marva = 0;
                                        float.TryParse(mark, out marva);
                                        grandTotal = grandTotal + marva;
                                    }
                                }
                                else
                                {
                                    foreach (GridViewRow grid in GridView3.Rows)
                                    {
                                        string examCode = Convert.ToString((GridView3.Rows[0].FindControl("lblExamCode") as Label).Text);
                                        qVal++;
                                        //drMark = dtInsert.NewRow();
                                        string mark = string.Empty;
                                        string subjectNo = Convert.ToString((grid.FindControl("lblsubid") as Label).Text);
                                        string Qno = Convert.ToString((grid.FindControl("lblregno") as Label).Text);
                                        string Cri = Convert.ToString((grid.FindControl("lblCri") as Label).Text);
                                        string app = Convert.ToString((grid.FindControl("lblappno") as Label).Text);
                                        string MasterId = Convert.ToString((grid.FindControl("lblMaterId") as Label).Text);
                                        string exmCode = Convert.ToString((grid.FindControl("lblExamCode") as Label).Text);

                                        if (rblStatus.SelectedIndex.Equals(0))
                                        {
                                            mark = Convert.ToString((grid.FindControl("txttest") as TextBox).Text);
                                        }
                                        else if (rblStatus.SelectedIndex.Equals(1))
                                        {
                                            mark = "-1";
                                        }
                                        else
                                            mark = "-16";

                                        if (string.IsNullOrEmpty(mark))
                                            mark = "-20";

                                        if (chkretest.Checked)
                                            qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + app + "','" + exmCode + "','" + MasterId + "','" + mark + "') else update NewInternalMarkEntry SET RetestMark='" + mark + "' where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "'", "text");
                                        else
                                            qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + app + "','" + exmCode + "','" + MasterId + "','" + mark + "') else update NewInternalMarkEntry SET Marks='" + mark + "' where ExamCode='" + exmCode + "' and app_no='" + app + "' and MasterID='" + MasterId + "'", "text");

                                        float marva = 0;
                                        float.TryParse(mark, out marva);
                                        if (marva != -1 && marva != -16 && marva != -20)
                                            grandTotal = grandTotal + marva;
                                    }
                                }
                            }
                            else if (ForMate == "2")
                            {
                                if (islab == "1" || islab.ToLower().Trim() == "true")
                                {
                                    float q1Mark = 0;
                                    float q2Mark = 0;
                                    float q3Mark = 0;
                                    float.TryParse(txtRec.Text.Trim(), out q1Mark);
                                    float.TryParse(txtObser.Text.Trim(), out q2Mark);
                                    float.TryParse(txtinternal.Text.Trim(), out q3Mark);
                                    if (string.IsNullOrEmpty(txtinternal.Text.Trim()))
                                    {
                                        q3Mark = -20;
                                    }
                                    if (string.IsNullOrEmpty(txtObser.Text.Trim()))
                                    {
                                        q2Mark = -20;
                                    }
                                    if (string.IsNullOrEmpty(txtRec.Text.Trim()))
                                    {
                                        q1Mark = -20;
                                    }
                                    if (q1Mark != -1 && q1Mark != -16 && q1Mark != -20)
                                        grandTotal11 = grandTotal11 + q1Mark;
                                    if (q2Mark != -1 && q2Mark != -16 && q2Mark != -20)
                                        grandTotal11 = grandTotal11 + q2Mark;
                                    if (q3Mark != -1 && q3Mark != -16 && q3Mark != -20)
                                        grandTotal11 = grandTotal11 + q3Mark;

                                    if (!chkretest.Checked)
                                    {
                                        string MasterId1 = lblRec.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId1 + "','" + q1Mark + "') else update NewInternalMarkEntry SET Marks='" + q1Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "'", "text");
                                        string MasterId2 = lblOb.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId2 + "','" + q2Mark + "') else update NewInternalMarkEntry SET Marks='" + q2Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "'", "text");
                                        string MasterId3 = lblint.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId3 + "','" + q3Mark + "') else update NewInternalMarkEntry SET Marks='" + q3Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "'", "text");
                                    }
                                    else
                                    {
                                        string MasterId1 = lblRec.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId1 + "','" + q1Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q1Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "'", "text");
                                        string MasterId2 = lblOb.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId2 + "','" + q2Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q2Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "'", "text");
                                        string MasterId3 = lblint.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId3 + "','" + q3Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q3Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "'", "text");
                                    }

                                }
                                else
                                {
                                    float q1Mark = 0;
                                    float q2Mark = 0;
                                    float q3Mark = 0;
                                    float q4Mark = 0;
                                    float descMarks = 0;
                                    float quizMarks = 0;
                                    float assignmentMarks = 0;
                                    float.TryParse(txtQ1.Text.Trim(), out q1Mark);
                                    float.TryParse(txtQ2.Text.Trim(), out q2Mark);
                                    float.TryParse(txtQ3.Text.Trim(), out q3Mark);
                                    float.TryParse(txtQ4.Text.Trim(), out q4Mark);

                                    if (txtQ1.Text.Trim() == "")
                                    {
                                        q1Mark = -20;
                                    }
                                    if (txtQ2.Text.Trim() == "")
                                    {
                                        q2Mark = -20;
                                    }
                                    if (txtQ3.Text.Trim() == "")
                                    {
                                        q3Mark = -20;
                                    }
                                    if (txtQ4.Text.Trim() == "")
                                    {
                                        q4Mark = -20;
                                    }

                                    if (Convert.ToString(hdnDescTotal.Value).ToLower() != "nan" && Convert.ToString(hdnDescTotal.Value).ToLower() != "-1" && Convert.ToString(hdnDescTotal.Value).ToLower() != "-20" && Convert.ToString(hdnDescTotal.Value).ToLower() != "-16")
                                        float.TryParse(hdnDescTotal.Value, out descMarks);
                                    else
                                    {
                                        if (q1Mark != -1 && q1Mark != -16 && q1Mark != -20)
                                            descMarks = grandTotal11 + q1Mark;
                                        if (q2Mark != -1 && q2Mark != -16 && q2Mark != -20)
                                            descMarks = grandTotal11 + q2Mark;
                                        if (q3Mark != -1 && q3Mark != -16 && q3Mark != -20)
                                            descMarks = grandTotal11 + q3Mark;
                                        if (q4Mark != -1 && q4Mark != -16 && q4Mark != -20)
                                            descMarks = grandTotal11 + q4Mark;
                                        //descMarks = q1Mark + q2Mark + q3Mark + q4Mark;
                                    }
                                    float.TryParse(txtQuizMark.Text.Trim(), out quizMarks);
                                    float.TryParse(txtAssignmntMark.Text.Trim(), out assignmentMarks);
                                    if (txtQuizMark.Text.Trim() == "")
                                        quizMarks = -20;
                                    if (txtAssignmntMark.Text.Trim() == "")
                                        assignmentMarks = -20;

                                    grandTotal11 = descMarks;
                                    if (quizMarks != -1 && quizMarks != -16 && quizMarks != -20)
                                        grandTotal11 = grandTotal11 + quizMarks;
                                    if (assignmentMarks != -1 && assignmentMarks != -16 && assignmentMarks != -20)
                                        grandTotal11 = grandTotal11 + assignmentMarks;

                                    string errMsg = "";
                                    if (q1Mark > 5 || q1Mark < -1)
                                        errMsg = "Q1 Mark should be between -1 to 5" + "\n";
                                    if (q2Mark > 5 || q2Mark < -1)
                                        errMsg = errMsg + " Q2 Mark should be between -1 to 5" + "\n";
                                    if (q3Mark > 5 || q3Mark < -1)
                                        errMsg = errMsg + " Q3 Mark should be between -1 to 5" + "\n";
                                    if (q4Mark > 5 || q4Mark < -1)
                                        errMsg = errMsg + " Q4 Mark should be between -1 to 5" + "\n";
                                    if (quizMarks > 10 || quizMarks < -1)
                                        errMsg = errMsg + " Quiz Mark should be between -1 to 10" + "\n";
                                    if (assignmentMarks > 5 || assignmentMarks < -1)
                                        errMsg = errMsg + " Assignment Mark should be between -1 to 5" + "\n";

                                    lblErrorMsg.Text = errMsg;
                                    //added by Mullai

                                    //***
                                    if (!chkretest.Checked)
                                    {
                                        string MasterId1 = lblQ1Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId1 + "','" + q1Mark + "') else update NewInternalMarkEntry SET Marks='" + q1Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "'", "text");

                                        string MasterId2 = lblQ2Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId2 + "','" + q2Mark + "') else update NewInternalMarkEntry SET Marks='" + q2Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "'", "text");

                                        string MasterId3 = lblQ3Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId3 + "','" + q3Mark + "') else update NewInternalMarkEntry SET Marks='" + q3Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "'", "text");

                                        string MasterId4 = lblQ4Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId4 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId4 + "','" + q4Mark + "') else update NewInternalMarkEntry SET Marks='" + q4Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId4 + "'", "text");

                                        string MasterId5 = lblQuiz.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId5 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId5 + "','" + quizMarks + "') else update NewInternalMarkEntry SET Marks='" + quizMarks + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId5 + "'", "text");

                                        string MasterId6 = lblAss.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId6 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,Marks) values('" + appNo + "','" + examCodeNew + "','" + MasterId6 + "','" + assignmentMarks + "') else update NewInternalMarkEntry SET Marks='" + assignmentMarks + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId6 + "'", "text");

                                        qryStatus = da.update_method_wo_parameter("update NewInternalMarkEntry SET DescTotal='" + descMarks + "' where app_no='" + appNo + "' and ExamCode='" + examCodeNew + "'", "text");
                                    }
                                    else
                                    {
                                        string MasterId1 = lblQ1Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId1 + "','" + q1Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q1Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId1 + "'", "text");

                                        string MasterId2 = lblQ2Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId2 + "','" + q2Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q2Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId2 + "'", "text");

                                        string MasterId3 = lblQ3Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId3 + "','" + q3Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q3Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId3 + "'", "text");

                                        string MasterId4 = lblQ4Code.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId4 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId4 + "','" + q4Mark + "') else update NewInternalMarkEntry SET RetestMark='" + q4Mark + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId4 + "'", "text");

                                        string MasterId5 = lblQuiz.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId5 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId5 + "','" + quizMarks + "') else update NewInternalMarkEntry SET RetestMark='" + quizMarks + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId5 + "'", "text");

                                        string MasterId6 = lblAss.Text;
                                        qryStatus = da.update_method_wo_parameter("if not exists(select * from NewInternalMarkEntry where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId6 + "') insert into NewInternalMarkEntry (app_no,examcode,MasterID,RetestMark) values('" + appNo + "','" + examCodeNew + "','" + MasterId6 + "','" + assignmentMarks + "') else update NewInternalMarkEntry SET RetestMark='" + assignmentMarks + "' where ExamCode='" + examCodeNew + "' and app_no='" + appNo + "' and MasterID='" + MasterId6 + "'", "text");

                                        qryStatus = da.update_method_wo_parameter("update NewInternalMarkEntry SET RetestDescTotal='" + descMarks + "' where app_no='" + appNo + "' and ExamCode='" + examCodeNew + "'", "text");
                                    }
                                }
                            }
                            else
                            {
                                string rollNos = string.Empty;
                                foreach (GridViewRow gridV in GridStudent.Rows)
                                {
                                    float stuMarks = 0;
                                    float subtot = 0;
                                    float.TryParse(lblMaxMark.Text, out subtot);
                                    string rollNoStu = Convert.ToString((gridV.FindControl("lblRollNO") as Label).Text);
                                    string MarkValue = Convert.ToString((gridV.FindControl("lblRollNO") as TextBox).Text);
                                    float.TryParse(MarkValue, out stuMarks);
                                    if (stuMarks > subtot)
                                    {
                                        hat.Clear();//lblGrandTotal
                                        hat.Add("roll_no", rollNoStu);
                                        hat.Add("exam_code", examCodeNew);
                                        int insertVal = 0;
                                        if (!chkretest.Checked)
                                        {
                                            hat.Add("marks_obtained", Math.Round(stuMarks));
                                            insertVal = da.insert_method("sp_ins_upd_cam_mark_dead", hat, "sp");
                                        }
                                        else
                                        {
                                            hat.Add("marks_obtained", Math.Round(stuMarks));
                                            insertVal = da.insert_method("sp_ins_upd_cam_REmark_dead", hat, "sp");
                                        }
                                        if (insertVal != 0)
                                            qryStatus = 1;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(rollNos))
                                            rollNos = rollNos + "," + rollNoStu;
                                        else
                                            rollNos = rollNoStu;
                                    }
                                }
                                if (!string.IsNullOrEmpty(rollNos))
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                else
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Some Students are Not Inserted!!')", true);
                            }
                            if (qryStatus != 0 && (ForMate == "1" || ForMate == "2"))
                            {
                                if (rblStatus.SelectedIndex.Equals(1))
                                {
                                    grandTotal = -1;
                                    grandTotal11 = -1;
                                }
                                else if (rblStatus.SelectedIndex.Equals(2))
                                {
                                    grandTotal = -16;
                                    grandTotal11 = -16;
                                }

                                hat.Clear();//lblGrandTotal
                                hat.Add("roll_no", txtRollOrReg.Text);
                                hat.Add("exam_code", examCodeNew);
                                if (ForMate == "1")
                                {
                                    hat.Add("marks_obtained", Math.Round(grandTotal, 1, MidpointRounding.AwayFromZero));
                                    lblGrandTotal.Text = grandTotal.ToString();
                                }
                                else
                                {

                                    hat.Add("marks_obtained", Math.Round(grandTotal11, 0, MidpointRounding.AwayFromZero));
                                    lblGrandTotal.Text = grandTotal11.ToString();
                                }
                                int insert = 0;
                                if (!chkretest.Checked)
                                    insert = da.insert_method("sp_ins_upd_cam_mark_dead", hat, "sp");
                                else
                                    insert = da.insert_method("sp_ins_upd_cam_REmark_dead", hat, "sp");

                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);


                                txtRollOrRegChanhge();
                                //if (ForMate == "1")
                                //{
                                //    loadReportSpread();//Need to Uncommand
                                //}
                                //else
                                //{
                                //    loadReportSpreadNew();
                                //}
                                //lnkAttMark11(sender,e);
                            }
                            lblErrorMsg.Visible = false;
                            clearFields();
                            lblNote2.Visible = false;
                            Div1.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Student!!')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Student!!')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Total Mark should be bellow from Max.Mark')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Entry time locked!!')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Error Occured - Not Saved')", true);
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (GridView3.Rows.Count > 0)
            {
                if (txtRollOrReg.Text != "")
                {
                    string appNo = string.Empty;
                    string subNo = string.Empty;
                    string criteriaNo = string.Empty;
                    string qry = string.Empty;
                    float grandTotal = 0;
                    string staffCode = Convert.ToString(Session["staff_code"]).Trim();
                    string rollOrReg = txtRollOrReg.Text;
                    appNo = da.GetFunction("select app_no from registration  where roll_no ='" + rollOrReg + "' and cc=0 and delflag=0 and exam_flag<>'debar'");

                    subjecttestDetails();
                    string[] arr = selectedSubTest.Split('#');
                    subNo = Convert.ToString(arr[0]);
                    criteriaNo = Convert.ToString(arr[1]);
                    string colname = string.Empty;
                    string MarkVal = string.Empty;
                    string examCodeSet = lblExamCode.Text;
                    string examCode = Convert.ToString((GridView3.Rows[0].FindControl("lblExamCode") as Label).Text);
                    int delete = 0;
                    if (appNo != "0" && appNo.ToLower() != "operation")
                    {
                        string del = "delete NewInternalMarkEntry where app_no='" + appNo + "' and examCode='" + examCodeSet + "'";
                        delete = da.update_method_wo_parameter(del, "text");
                        string delresu = "delete Result where roll_no='" + rollOrReg + "' and exam_code='" + examCodeSet + "'";
                        delete = da.update_method_wo_parameter(delresu, "text");
                        txtRollOrRegChanhge();
                        txtQ1.Text = "";
                        txtQ2.Text = "";
                        txtQ3.Text = "";
                        txtQ4.Text = "";
                        txtQuizMark.Text = "";
                        txtAssignmntMark.Text = "";
                        lblDescTotal.Text = "";
                        txtObser.Text = "";
                        txtRec.Text = "";
                        txtinternal.Text = "";
                    }
                    if (delete != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted!!')", true);
                    }


                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Student!!')", true);
                }
            }
        }
        catch
        {
        }
    }
    protected void btndeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            subjecttestDetails();
            string[] arr = selectedSubTest.Split('#');
            string subNo = Convert.ToString(arr[0]);
            string criteriaNo = Convert.ToString(arr[1]);
            string colname = string.Empty;
            string MarkVal = string.Empty;
            string examCodeSet = lblExamCode.Text;
            int delete = 0;
            foreach (GridViewRow grid in GridStudent.Rows)
            {
                //drMark = dtInsert.NewRow();
                string mark = string.Empty;
                //string subjectNo = Convert.ToString((grid.FindControl("lblsubid") as Label).Text);
                string rollOrReg = Convert.ToString((grid.FindControl("lblRollNO") as LinkButton).Text);
                string app = Convert.ToString((grid.FindControl("lblappno") as Label).Text);
                if (app != "0" && app.ToLower() != "operation")
                {
                    string del = "delete NewInternalMarkEntry where app_no='" + app + "' and examCode='" + examCodeSet + "'";
                    delete = da.update_method_wo_parameter(del, "text");
                    string delresu = "delete Result where roll_no='" + rollOrReg + "' and exam_code='" + examCodeSet + "'";
                    delete = da.update_method_wo_parameter(delresu, "text");
                }
            }
            if (delete != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted!!')", true);
            }
            txtRollOrRegChanhge();
            txtQ1.Text = "";
            txtQ2.Text = "";
            txtQ3.Text = "";
            txtQ4.Text = "";
            txtQuizMark.Text = "";
            txtAssignmntMark.Text = "";
            lblDescTotal.Text = "";
            txtObser.Text = "";
            txtRec.Text = "";
            txtinternal.Text = "";


        }
        catch
        {
        }
    }
    private string getMarkValue(string mark)
    {
        mark = mark.ToUpper();
        string getmark = mark;
        try
        {
            switch (mark)
            {
                case "AAA":
                    getmark = "-1";
                    break;
                case "EOD":
                    getmark = "-3";
                    break;
                case "OOD":
                    getmark = "-15";
                    break;
                case "EL":
                    getmark = "-2";
                    break;
                case "COD":
                    getmark = "-14";
                    break;
                case "ML":
                    getmark = "-4";
                    break;
                case "SOD":
                    getmark = "-5";
                    break;
                case "NSS":
                    getmark = "-6";
                    break;
                //case "H":
                //          getmark ="-7";
                //           break;
                case "NJ":
                    getmark = "-7";
                    break;
                case "S":
                    getmark = "-8";
                    break;
                case "L":
                    getmark = "-9";
                    break;
                case "NCC":
                    getmark = "-10";
                    break;
                case "HS":
                    getmark = "-11";
                    break;
                case "PP":
                    getmark = "-12";
                    break;
                case "SYOD":
                    getmark = "-13";
                    break;
                case "OD":
                    getmark = "-16";
                    break;
                case "LA":
                    getmark = "-17";
                    break;
                //****Modified By Subburaj 20.08.2014******//
                case "RAA":
                    getmark = "-18";
                    break;
                //****************End*****************//
            }
        }
        catch
        {
        }
        return getmark;
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridView3.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView3.Rows[rowIndex];
                GridViewRow previousRow = GridView3.Rows[rowIndex + 1];

                string l1 = (row.FindControl("lblPartNo") as Label).Text;
                string l2 = (previousRow.FindControl("lblPartNo") as Label).Text;
                if (l1 == l2)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }

                string l3 = (row.FindControl("lblregno") as Label).Text;
                string l4 = (previousRow.FindControl("lblregno") as Label).Text;
                if (l3 == l4)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void loadReportSpread()
    {
        try
        {
            selectSubject();
            string[] arr = selectedSubDetails.Split('-');
            string batch = Convert.ToString(arr[0].Trim());
            string degreeCode = Convert.ToString(arr[1].Trim());
            string semester = Convert.ToString(arr[2].Trim());
            string strsec = Convert.ToString(arr[3].Trim());
            string subno = Convert.ToString(arr[4].Trim());
            string staffCode = Convert.ToString(arr[5].Trim());
            subjecttestDetails();
            string[] arrTest = selectedSubTest.Split('#');
            string criteriaNo = Convert.ToString(arrTest[1]);
            lblSuNo.Text = subno;
            lblCriteriaNO.Text = criteriaNo;
            string strstaffselector = string.Empty;
            Session["StaffSelector"] = "0";
            strstaffselector = string.Empty;
            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            if (!string.IsNullOrEmpty(staffbatchyear) && staffbatchyear != "0")
            {
                string[] splitminimumabsentsms = staffbatchyear.Split('-');
                if (splitminimumabsentsms.Length == 2)
                {
                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                    if (splitminimumabsentsms[0].ToString() == "1")
                    {
                        if (Convert.ToInt32(batch) >= batchyearsetting)
                        {
                            Session["StaffSelector"] = "1";
                        }
                    }
                }
            }
            if (Session["StaffSelector"].ToString() == "1")
            {
                if (Session["Staff_Code"] != null)
                {
                    if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                    {
                        strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
                    }
                }
            }

            string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            string ForMate = da.GetFunction("select value from Master_Settings where settings='SaveCoFormate'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }
            string examCodeValue = da.GetFunction("select exam_code from Exam_type where subject_no='" + subno + "' and criteria_no='" + criteriaNo + "'" + strsec + "");
            //string qryStuDetails = "Select distinct registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,ap.app_formno as ApplicationNumber,registration.roll_admit as AdmissionNo,im.subject_no,Q1Mark,Q2Mark,Q3Mark,Q4Mark,DescTotal,QuizMark,AssignmentMark from registration ,SubjectChooser  ,applyn ap,InternalMarkEntry im where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degreeCode + "' and Semester = '" + semester + "' and registration.Batch_Year = '" + batch + "' and SubjectChooser.Subject_No = im.subject_no " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  and im.subject_no in ('" + subno + "') and criteria_no='" + criteriaNo + "' and ap.app_no=im.app_no " + strstaffselector + " " + strorderby + "";

            // string qryStuDetails = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,q.PartNo,q.NO_Ques, q.QNo,m.Marks,re.marks_obtained,q.CriteriaNo,q.subjectNo from InternalMarkEntry m,CAQuesSettingsParent q,registration r,SubjectChooser s,Result re,Exam_type e   where r.App_No=m.app_no and LTRIM(rtrim(ISNULL(r.sections,'')))=LTRIM(rtrim(ISNULL(e.sections,''))) and s.subject_no=q.subjectNo and e.exam_code=re.exam_code and e.exam_code=m.ExamCode and e.subject_no=q.subjectNo and e.criteria_no=q.CriteriaNo and r.Roll_No=s.roll_no and q.MasterID=m.MasterID and re.roll_no=r.Roll_No and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and q.subjectNo in('" + subno + "') and q.CriteriaNo='" + criteriaNo + "' " + strorderby + "";//and e.sections=''  " + strstaffselector + "
            string qryStuDetails = "select r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,q.PartNo,q.NO_Ques, q.QNo,m.Marks,re.marks_obtained,q.CriteriaNo,q.subjectNo from NewInternalMarkEntry m,CAQuesSettingsParent q,registration r,Result re   where r.App_No=m.app_no   and q.MasterID=m.MasterID and re.roll_no=r.Roll_No and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and q.subjectNo in('" + subno + "') and q.CriteriaNo='" + criteriaNo + "' and r.Batch_Year='" + batch + "' and r.degree_code='" + degreeCode + "' and r.Current_Semester='" + semester + "'" + strsec + " and re.exam_code='" + examCodeValue + "'  ORDER BY r.Reg_No";//and re.exam_code='38789'
            DataSet dsStuDetails = da.select_method_wo_parameter(qryStuDetails, "Text");
            DataTable dtData = new DataTable();
            dtData.Columns.Add("Roll No");
            dtData.Columns.Add("Reg No");
            dtData.Columns.Add("Stud Name");
            DataRow drNew = null;

            string islab = da.GetFunction("select ss.Lab from subject s,sub_sem ss where s.subType_no=ss.subType_no and subject_no='" + subno + "'");
            if (dsStuDetails.Tables.Count > 0 && dsStuDetails.Tables[0].Rows.Count > 0)
            {
                DataTable dicPartQNo = dsStuDetails.Tables[0].DefaultView.ToTable(true, "NO_Ques", "PartNo");
                DataTable dicStu = dsStuDetails.Tables[0].DefaultView.ToTable(true, "App_No");
                DataTable dicQNos = dsStuDetails.Tables[0].DefaultView.ToTable(true, "QNo");
                DataView dvQ = dicQNos.DefaultView;
                dvQ.Sort = "QNo asc";
                for (int q = 1; q <= dvQ.Count; q++)
                {
                    if (ForMate == "1")
                    {
                        dtData.Columns.Add(q.ToString());
                    }
                    if (ForMate == "2")
                    {
                        string colName = "Q" + q + "Mark";
                        if (q == 5)
                            colName = "Quiz Mark";
                        if (q == 6)
                            colName = "Assignment Mark";

                        dtData.Columns.Add(colName);
                    }
                }
                dtData.Columns.Add("Total");
                if (dicStu.Rows.Count > 0)
                {
                    foreach (DataRow dt in dicStu.Rows)
                    {
                        string appNO = Convert.ToString(dt["App_No"]);
                        dsStuDetails.Tables[0].DefaultView.RowFilter = "App_No='" + appNO + "'";
                        DataTable dtStuMarks = dsStuDetails.Tables[0].DefaultView.ToTable();
                        drNew = dtData.NewRow();
                        drNew["Roll No"] = Convert.ToString(dtStuMarks.Rows[0]["Roll_No"]);
                        drNew["Reg No"] = Convert.ToString(dtStuMarks.Rows[0]["Reg_No"]);
                        drNew["Stud Name"] = Convert.ToString(dtStuMarks.Rows[0]["Stud_Name"]);

                        drNew["Total"] = getMarkText(Convert.ToString(dtStuMarks.Rows[0]["marks_obtained"]));
                        for (int q = 1; q <= dvQ.Count; q++)
                        {
                            dtStuMarks.DefaultView.RowFilter = "App_No='" + appNO + "' and QNo='" + q.ToString() + "'";
                            DataView dvmark = dtStuMarks.DefaultView;
                            if (ForMate == "1")
                            {
                                if (dvmark.Count > 0)
                                    drNew[q.ToString()] = getMarkText(Convert.ToString(dvmark[0]["Marks"]));
                                else
                                    drNew[q.ToString()] = "-";
                            }
                            else if (ForMate == "2")
                            {
                                string colName = "Q" + q + "Mark";
                                if (q == 5)
                                    colName = "Quiz Mark";
                                if (q == 6)
                                    colName = "Assignment Mark";

                                if (dvmark.Count > 0)
                                    drNew[colName] = getMarkText(Convert.ToString(dvmark[0]["Marks"]));
                                else
                                    drNew[colName] = "-";
                            }
                        }
                        dtData.Rows.Add(drNew);
                    }
                }
            }
            else
            {
                lblNote2.Visible = false;
            }
            if (dtData.Rows.Count > 0)
            {

                //if (islab == "0" || islab.ToLower() == "true")
                //{
                //    GridView5.DataSource = dtData;
                //    GridView5.DataBind();
                //    GridView5.Visible = true;
                //    GridReport.Visible = false;
                //}
                //else
                //{
                GridReport.DataSource = dtData;
                GridReport.DataBind();
                GridReport.Visible = true;
                //}
                Div1.Visible = true;
                //divPopSpread.Visible = false;
                //lblNote.Visible = true;
                //txtRollOrReg.Text = "";
                //lblStuName.Text = "";
                lblErrorMsg.Visible = false;
                clearFields();
                //GridView3.Visible = false;
                lblNote2.Visible = true;

            }
            //GridView3.DataSource = dtmrk;
            //GridView3.DataBind();
            //GridView3.Visible = true;
        }
        catch { }
    }
    public void txtRollOrRegChanhge()
    {
        try
        {
         
            string qryStudeStaffSelector = string.Empty;
            selectSubject();
            string query = "";
            WebService ws = new WebService();
            string[] arr = selectedSubDetails.Split('-');

            string batch = Convert.ToString(arr[0].Trim());
            string degreeCode = Convert.ToString(arr[1].Trim());
            string semester = Convert.ToString(arr[2].Trim());
            string strsec = Convert.ToString(arr[3].Trim());
            string subno = Convert.ToString(arr[4].Trim());
            string staffCode = Convert.ToString(arr[5].Trim());
            string staffSelectorVal = Convert.ToString(arr[6].Trim());
            string strstaffselecotr = "";
            subjecttestDetails();
            string[] arrTest = selectedSubTest.Split('#');
            string criteriaNo = Convert.ToString(arrTest[1]);
            lblSuNo.Text = subno;
            lblCriteriaNO.Text = criteriaNo;
            string ForMate = da.GetFunction("select value from Master_Settings where settings='SaveCoFormate'");
            string examCode = da.GetFunction("select exam_code from Exam_type where criteria_no='" + lblCriteriaNO.Text + "' and subject_no='" + lblSuNo.Text + "'" + strsec);

            //added by Mullai
            bool staffSelector = false;
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batch.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            if (staffSelector)
            {
                strstaffselecotr = " and subjectchooser.staffcode like '%" + Convert.ToString(staffCode) + "%'";
            }
          

            //if (staffSelectorVal == "1")
            //{
            //    if (staffCode != null)
            //    {
            //        if (staffCode != "" && staffCode != "0")
            //        {
            //            strstaffselecotr = " and SubjectChooser.staffcode like '%" + staffCode + "%' ";
            //        }
            //    }
            //}
            string strorder = filterfunction();
            query = "Select  distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.sections,registration.App_No,Registration.Sections from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + Convert.ToString(degreeCode) + "' and Semester = '" + Convert.ToString(semester) + "' and registration.Batch_Year = '" + Convert.ToString(batch) + "' and Subject_No = '" + Convert.ToString(subno) + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + Convert.ToString(semester) + "'   " + strstaffselecotr + "  " + strorder + " ";

            //and registration.roll_no='" + txtRollOrReg.Text.Trim() + "'
            string MArks = "select e.marks_obtained,e.Retest_Marks_obtained,e.roll_no from Registration r,Result e where r.Roll_No=e.roll_no and r.Batch_Year='" + Convert.ToString(batch) + "' and r.degree_code='" + Convert.ToString(degreeCode) + "' and Current_Semester='" + Convert.ToString(semester) + "' " + strsec + " and e.exam_code='" + Convert.ToString(examCode) + "'";
            DataTable dtM = dir.selectDataTable(MArks);
            DataSet dtstudent = da.select_method_wo_parameter(query, "text");
            DataTable dttext = new DataTable();
            dttext.Columns.Add("roll_no");
            dttext.Columns.Add("reg_no");
            dttext.Columns.Add("stud_name");
            dttext.Columns.Add("Sections");
            dttext.Columns.Add("App_No");
            dttext.Columns.Add("Marks");
            DataRow drN = null;
            string StudName = string.Empty;
            if (dtstudent.Tables.Count > 0 && dtstudent.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow gr in dtstudent.Tables[0].Rows)
                {
                    drN = dttext.NewRow();
                    string rollNo = Convert.ToString(gr["roll_no"]);
                    string RegNO = Convert.ToString(gr["reg_no"]);
                    StudName = Convert.ToString(gr["stud_name"]);
                    string Sec = Convert.ToString(gr["Sections"]);
                    string app = Convert.ToString(gr["App_No"]);
                    string ObMark = string.Empty;
                    lblExamCode.Text = Convert.ToString(examCode);
                    if (dtM.Rows.Count > 0)
                    {
                        dtM.DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                        DataView dt = dtM.DefaultView;
                        if (dt.Count > 0)
                        {
                            if (!chkretest.Checked)
                                ObMark = getMarkText(Convert.ToString(dt[0]["marks_obtained"]));
                            else
                                ObMark = getMarkText(Convert.ToString(dt[0]["Retest_Marks_obtained"]));
                        }
                        else
                            ObMark = "NE";
                    }
                    else
                        ObMark = "NE";

                    drN["roll_no"] = rollNo;
                    drN["reg_no"] = RegNO;
                    drN["stud_name"] = StudName;
                    drN["Sections"] = Sec;
                    drN["App_No"] = app;
                    drN["Marks"] = ObMark;
                    dttext.Rows.Add(drN);
                }
                if (dttext.Rows.Count > 0)
                {
                    clearFields();
                    rblStatus.SelectedIndex = 0;
                    GridStudent.Visible = true;
                    GridStudent.DataSource = dttext;
                    GridStudent.DataBind();
                    GridStudent.Columns[1].Visible = false;
                    GridStudent.Columns[2].Visible = false;
                    //GridStudent.Columns[3].Visible = false;
                    if (Session["Rollflag"].ToString() == "1")
                    {
                        GridStudent.Columns[1].Visible = true;
                        GridStudent.Columns[1].HeaderStyle.Width = 100;
                        //FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                        // FpSpread2.Sheets[0].Columns[1].Width = 100;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        GridStudent.Columns[2].Visible = true;
                        GridStudent.Columns[2].HeaderStyle.Width = 100;
                        //FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                        //FpSpread2.Sheets[0].Columns[2].Width = 100;
                    }
                    if (Session["Studflag"].ToString() == "1")
                    {
                        GridStudent.Columns[3].Visible = true;
                        GridStudent.Columns[3].HeaderStyle.Width = 200;
                        //FpSpread2.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                        //FpSpread2.Sheets[0].Columns[5].Width = 100;
                    }


                    if (ForMate != "1" && ForMate != "2")
                    {
                        GridStudent.Enabled = true;
                        foreach (GridViewRow gri in GridStudent.Rows)
                        {
                            TextBox txt = (gri.FindControl("txtTotMark") as TextBox);
                            txt.Enabled = true;
                        }
                    }

                    string sections = Convert.ToString((GridStudent.Rows[0].FindControl("lblSec") as Label).Text);
                    string rollNo11 = Convert.ToString((GridStudent.Rows[0].FindControl("lblRollNO") as LinkButton).Text);
                    string appNo = (GridStudent.Rows[0].FindControl("lblappno") as Label).Text;
                    string sec = (GridStudent.Rows[0].FindControl("lblSec") as Label).Text;
                    string studname = (GridStudent.Rows[0].FindControl("lblName") as LinkButton).Text;
                    string rollNo = (GridStudent.Rows[0].FindControl("lblRollNO") as LinkButton).Text;
                    if (Session["Row"] == null)
                    {
                        Session["Row"] = "0";
                        txtRollOrReg.Text = rollNo.Trim();
                        lblStuName.Text = studname.Trim();
                        loadMarkEntry(sections, rollNo11, appNo);

                    }
                    lblErrorMsg.Visible = false;
                }
            }
            else
            {
                txtRollOrReg.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Number')", true);
            }

        }
        catch { }
    }
    protected void lnkAttMark11(object sender, EventArgs e)
    {
        LinkButton lnkSelected = (LinkButton)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        Session["Row"] = rowIndx;
        string appNo = (GridStudent.Rows[rowIndx].FindControl("lblappno") as Label).Text;
        string sec = (GridStudent.Rows[rowIndx].FindControl("lblSec") as Label).Text;
        string studname = (GridStudent.Rows[rowIndx].FindControl("lblName") as LinkButton).Text;
        string rollNo = (GridStudent.Rows[rowIndx].FindControl("lblRollNO") as LinkButton).Text;
        //GridStudent.Rows[rowIndx].BackColor = ColorTranslator.FromHtml("#f47f41"); 
        rblStatus.SelectedIndex = 0;
        rblStatus_OnSelectedIndexChanged(sender, e);
        txtRollOrReg.Text = rollNo.Trim();
        lblStuName.Text = studname.Trim();
        loadMarkEntry(sec, rollNo, appNo);
    }
    protected void BtnPerv_Click(object sender, EventArgs e)
    {
        if (GridStudent.Rows.Count > 0)
        {
            int rowPre = 0;
            if (Session["Row"] != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Row"])))
                {
                    rowPre = Convert.ToInt16(Convert.ToString(Session["Row"])) - 1;
                }
                if (rowPre >= 0)
                {
                    string appNo = (GridStudent.Rows[rowPre].FindControl("lblappno") as Label).Text;
                    string sec = (GridStudent.Rows[rowPre].FindControl("lblSec") as Label).Text;
                    string studname = (GridStudent.Rows[rowPre].FindControl("lblName") as LinkButton).Text;
                    string rollNo = (GridStudent.Rows[rowPre].FindControl("lblRollNO") as LinkButton).Text;
                    //GridStudent.Rows[rowPre].BackColor = ColorTranslator.FromHtml("#f47f41"); 
                    Session["Row"] = rowPre;
                    rblStatus.SelectedIndex = 0;
                    rblStatus_OnSelectedIndexChanged(sender, e);
                    txtRollOrReg.Text = rollNo.Trim();
                    lblStuName.Text = studname.Trim();
                    loadMarkEntry(sec, rollNo, appNo);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Pls Select Student!!')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (GridStudent.Rows.Count > 0)
        {
            int rowPre = 0;
            if (Session["Row"] != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Row"])))
                {
                    rowPre = Convert.ToInt16(Convert.ToString(Session["Row"])) + 1;
                }
                if (rowPre >= 0 && rowPre < GridStudent.Rows.Count)
                {
                    string appNo = (GridStudent.Rows[rowPre].FindControl("lblappno") as Label).Text;
                    string sec = (GridStudent.Rows[rowPre].FindControl("lblSec") as Label).Text;
                    string studname = (GridStudent.Rows[rowPre].FindControl("lblName") as LinkButton).Text;
                    string rollNo = (GridStudent.Rows[rowPre].FindControl("lblRollNO") as LinkButton).Text;
                    // GridStudent.Rows[rowPre].BackColor = ColorTranslator.FromHtml("#f47f41"); 
                    Session["Row"] = rowPre;
                    rblStatus.SelectedIndex = 0;
                    rblStatus_OnSelectedIndexChanged(sender, e);
                    txtRollOrReg.Text = rollNo.Trim();
                    lblStuName.Text = studname.Trim();
                    loadMarkEntry(sec, rollNo, appNo);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Pls Select Student!!')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Invalid Row')", true);
        }
    }
    protected void loadReportSpreadNew()
    {
        try
        {
            dtmrk.Columns.Add("rollno");
            dtmrk.Columns.Add("regno");
            dtmrk.Columns.Add("student_type");
            dtmrk.Columns.Add("applicationno");
            dtmrk.Columns.Add("admn_no");
            dtmrk.Columns.Add("studname");
            dtmrk.Columns.Add("q1_mrk");
            dtmrk.Columns.Add("q2_mrk");
            dtmrk.Columns.Add("q3_mrk");
            dtmrk.Columns.Add("q4_mrk");
            dtmrk.Columns.Add("descript_mrk");
            dtmrk.Columns.Add("quiz_mrk");
            dtmrk.Columns.Add("assign_mrk");

            string[] arr = selectedSubDetails.Split('-');
            string batch = Convert.ToString(arr[0].Trim());
            string degreeCode = Convert.ToString(arr[1].Trim());
            string semester = Convert.ToString(arr[2].Trim());
            string strsec = Convert.ToString(arr[3].Trim());
            string subno = Convert.ToString(arr[4].Trim());
            string staffCode = Convert.ToString(arr[5].Trim());
            subjecttestDetails();
            string[] arrTest = selectedSubTest.Split('#');
            string criteriaNo = Convert.ToString(arrTest[1]);
            string islab = da.GetFunction("select ss.Lab from subject s,sub_sem ss where s.subType_no=ss.subType_no and subject_no='" + subno + "'");
            string strstaffselector = string.Empty;
            Session["StaffSelector"] = "0";
            strstaffselector = string.Empty;
            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            string[] splitminimumabsentsms = staffbatchyear.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batch) >= batchyearsetting)
                    {
                        Session["StaffSelector"] = "1";
                    }
                }
            }
            if (Session["StaffSelector"].ToString() == "1")
            {
                if (Session["Staff_Code"] != null)
                {
                    if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                    {
                        strstaffselector = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
                    }
                }
            }

            string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY registration.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY registration.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY Registration.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,Registration.Stud_Name";
                }
            }
            string qryStuDetails = "Select distinct registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,ap.app_formno as ApplicationNumber,registration.roll_admit as AdmissionNo,im.subject_no,Q1Mark,Q2Mark,Q3Mark,Q4Mark,DescTotal,QuizMark,AssignmentMark from registration ,SubjectChooser  ,applyn ap,NewInternalMarkEntry im where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degreeCode + "' and Semester = '" + semester + "' and registration.Batch_Year = '" + batch + "' and SubjectChooser.Subject_No = im.subject_no " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  and im.subject_no in ('" + subno + "') and criteria_no='" + criteriaNo + "' and ap.app_no=im.app_no " + strstaffselector + " " + strorderby + "";
            DataSet dsStuDetails = da.select_method_wo_parameter(qryStuDetails, "Text");
            if (dsStuDetails.Tables.Count > 0 && dsStuDetails.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsStuDetails.Tables[0].Rows.Count; row++)
                {
                    string rolno = dsStuDetails.Tables[0].Rows[row]["RollNumber"].ToString();
                    string regno = dsStuDetails.Tables[0].Rows[row]["RegistrationNumber"].ToString();
                    string studtype = dsStuDetails.Tables[0].Rows[row]["StudentType"].ToString();
                    string applicationno = dsStuDetails.Tables[0].Rows[row]["ApplicationNumber"].ToString();
                    string admnno = dsStuDetails.Tables[0].Rows[row]["AdmissionNo"].ToString();
                    string studname = dsStuDetails.Tables[0].Rows[row]["Student_Name"].ToString();
                    string q1_mrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["Q1Mark"].ToString());
                    string q2_mrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["Q2Mark"].ToString());
                    string q3_mrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["Q3Mark"].ToString());
                    string q4_mrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["Q4Mark"].ToString());
                    string dectot = getMarkText(dsStuDetails.Tables[0].Rows[row]["DescTotal"].ToString());
                    string quizmrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["QuizMark"].ToString());
                    string assignmrk = getMarkText(dsStuDetails.Tables[0].Rows[row]["AssignmentMark"].ToString());

                    dr2 = dtmrk.NewRow();
                    dr2["rollno"] = rolno;
                    dr2["regno"] = regno;
                    dr2["student_type"] = studtype;
                    dr2["applicationno"] = applicationno;
                    dr2["admn_no"] = admnno;
                    dr2["studname"] = studname;
                    dr2["q1_mrk"] = q1_mrk;
                    dr2["q2_mrk"] = q2_mrk;
                    dr2["q3_mrk"] = q3_mrk;
                    dr2["q4_mrk"] = q4_mrk;
                    dr2["descript_mrk"] = dectot;
                    dr2["quiz_mrk"] = quizmrk;
                    dr2["assign_mrk"] = assignmrk;
                    dtmrk.Rows.Add(dr2);
                }
            }
            else
            {
                lblNote2.Visible = false;
            }
            GridView4.DataSource = dtmrk;
            GridView4.DataBind();
            GridView4.Visible = true;
        }
        catch { }
    }
    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
                case "-20":
                    mark = " ";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    public void loadMarkEntry(string sections, string rollNo, string appNo)
    {
        try
        {
            txtQ1.Text = "";
            txtQ2.Text = "";
            txtQ3.Text = "";
            txtQ4.Text = "";
            txtQuizMark.Text = "";
            txtAssignmntMark.Text = "";
            lblDescTotal.Text = "";
            txtObser.Text = "";
            txtRec.Text = "";
            txtinternal.Text = "";
            Button3.Visible = false;
            Button4.Visible = false;
            Save.Visible = false;
            Delete.Visible = false;
            GridView3.Visible = false;
            Format2.Visible = false;
            btnSave.Visible = false;
            Lab.Visible = false;
            Button1.Visible = false;
            Button2.Visible = false;
            btnDelete.Visible = false;
            gridLab.Visible = false;
            //divPopSpread.Visible = false;
            DataTable dtMarks = new DataTable();
            dtMarks.Columns.Add("appNo");
            dtMarks.Columns.Add("PartNo");
            dtMarks.Columns.Add("PartName");
            dtMarks.Columns.Add("CourseOutComeNo");
            dtMarks.Columns.Add("QNo");
            dtMarks.Columns.Add("SubNo");
            dtMarks.Columns.Add("maxmrk");
            dtMarks.Columns.Add("criteria");
            dtMarks.Columns.Add("StuMark");//
            dtMarks.Columns.Add("MasterID");
            dtMarks.Columns.Add("examCode");
            dtMarks.Columns.Add("sub1");
            dtMarks.Columns.Add("sub2");
            DataRow drResult = null;


            DataTable dtMarkslab = new DataTable();
            dtMarkslab.Columns.Add("subSubjectName");
            dtMarkslab.Columns.Add("subjectId");
            dtMarkslab.Columns.Add("examCode");
            dtMarkslab.Columns.Add("cono");
            dtMarkslab.Columns.Add("maxMark");
            dtMarkslab.Columns.Add("StuMark");
            dtMarkslab.Columns.Add("appNo");
            dtMarkslab.Columns.Add("RollNo");
            DataRow drLabResult = null;

            string str = string.Empty;
            string rollOrReg = txtRollOrReg.Text;

            //string ForMate = Convert.ToString(Session["SaveCoFormate"]);
            string ForMate = da.GetFunction("select value from Master_Settings where settings='SaveCoFormate'");

            if (!string.IsNullOrEmpty(sections))
                sections = "  and sections='" + sections + "'";
            //appNo = da.GetFunction("select app_no from registration  where roll_no ='" + rollOrReg + "' and cc=0 and delflag=0 and exam_flag<>'debar'");
            DataTable dtCoSett = dir.selectDataTable("select * from Master_Settings where settings='COSettings'");
            if (!string.IsNullOrEmpty(lblSuNo.Text) && !string.IsNullOrEmpty(lblCriteriaNO.Text) && !string.IsNullOrEmpty(appNo) && appNo != "0")
            {
                Dictionary<string, string> ParametersDic = new Dictionary<string, string>();
                ParametersDic.Add("@subno", Convert.ToString(lblSuNo.Text));
                ParametersDic.Add("@CriteriaNO", lblCriteriaNO.Text);
                DataTable dtSettings = storAcc.selectDataTable("getCAQuesSettings", ParametersDic);
                //string SelectQ = "select * from CAQuesSettingsParent where subjectNo='" + lblSuNo.Text + "' and CriteriaNo='" + lblCriteriaNO.Text + "' order by QNo,sub1,sub2";
                //DataTable dtSettings = dir.selectDataTable(SelectQ);
                //DataTable dicPartnQ = dtSettings.DefaultView.ToTable(true, "PartNo", "NO_Ques");
                string examCode = da.GetFunction("select exam_code from Exam_type where criteria_no='" + lblCriteriaNO.Text + "' and subject_no='" + lblSuNo.Text + "'" + sections);
                string gT = string.Empty;
                if (!chkretest.Checked)
                    gT = da.GetFunction("select marks_obtained from Result where roll_no='" + rollNo + "' and exam_code='" + examCode + "'");
                else
                    gT = da.GetFunction("select Retest_Marks_obtained from Result where roll_no='" + rollNo + "' and exam_code='" + examCode + "'");
                //string getMarks = "select * from NewInternalMarkEntry where app_no='" + appNo + "' and ExamCode='" + examCode + "'";
                //DataTable dtStuMarks = dir.selectDataTable(getMarks);
                bool isla = false;
                if (ForMate == "1")
                {
                    DataTable dtSubSubject = dir.selectDataTable("select * from subsubjectTestDetails where examCode='" + examCode + "'");
                    DataTable dtStulabMark = dir.selectDataTable("select * from subSubjectWiseMarkEntry where appNo='" + appNo + "'");
                    if (dtSubSubject.Rows.Count > 0)
                    {
                        isla = true;
                        foreach (DataRow drl in dtSubSubject.Rows)
                        {
                            drLabResult = dtMarkslab.NewRow();
                            drLabResult["subSubjectName"] = Convert.ToString(drl["subSubjectName"]);
                            drLabResult["subjectId"] = Convert.ToString(drl["subjectId"]);
                            drLabResult["examCode"] = Convert.ToString(drl["examCode"]);
                            //drLabResult["cono"] = Convert.ToString(drl["cono"]);
                            if (dtCoSett.Rows.Count > 0)
                            {
                                dtCoSett.DefaultView.RowFilter = "masterno='" + Convert.ToString(drl["cono"]) + "'";
                                DataView dvCo = dtCoSett.DefaultView;
                                if (dvCo.Count > 0)
                                    drLabResult["cono"] = Convert.ToString(dvCo[0]["template"]);
                                else
                                    drLabResult["cono"] = "";
                            }
                            else
                                drLabResult["cono"] = "";

                            drLabResult["maxMark"] = Convert.ToString(drl["maxMark"]);
                            if (dtStulabMark.Rows.Count > 0)
                            {
                                dtStulabMark.DefaultView.RowFilter = "subjectId='" + Convert.ToString(drl["subjectId"]) + "'";
                                DataView dvlabMark = dtStulabMark.DefaultView;
                                if (!chkretest.Checked)
                                {
                                    if (dvlabMark.Count > 0)
                                        drLabResult["StuMark"] = Convert.ToString(dvlabMark[0]["testMark"]);
                                    else
                                        drLabResult["StuMark"] = "";
                                }
                                else
                                {
                                    if (dvlabMark.Count > 0)
                                        drLabResult["StuMark"] = Convert.ToString(dvlabMark[0]["ReTestMark"]);
                                    else
                                        drLabResult["StuMark"] = "";
                                }
                            }
                            else
                            {
                                drLabResult["StuMark"] = "";
                            }
                            //drLabResult["StuMark"] = "";
                            drLabResult["appNo"] = appNo;
                            drLabResult["RollNo"] = rollNo;
                            dtMarkslab.Rows.Add(drLabResult);
                        }
                        if (dtMarkslab.Rows.Count > 0)
                        {
                            gridLab.DataSource = dtMarkslab;
                            gridLab.DataBind();
                            gridLab.Visible = true;
                            Button3.Visible = true;
                            Button4.Visible = true;
                            GridView3.Visible = false;
                            Save.Visible = false;
                            Delete.Visible = false;
                            btnSave.Visible = false;
                            btnDelete.Visible = false;
                            lblTot.Visible = false;
                            Format2.Visible = false;
                            Lab.Visible = false;
                            Button1.Visible = false;
                            Button2.Visible = false;
                            lblTot.Visible = true;
                            lblGrandTotal.Visible = true;
                            lblGrandTotal.Text = getMarkText(gT.ToString());
                        }
                    }
                    else
                    {
                        isla = false;
                    }
                }
                if (!isla)
                {

                    string islab = da.GetFunction("select ss.Lab from subject s,sub_sem ss where s.subType_no=ss.subType_no and subject_no='" + lblSuNo.Text + "'");

                    ParametersDic.Clear();
                    ParametersDic.Add("@Appno", appNo);
                    ParametersDic.Add("@ExamCode", examCode);
                    DataTable dtStuMarks = storAcc.selectDataTable("getCAMarks", ParametersDic);

                    int QNO = 0;
                    float grandtot = 0;
                    if (dtSettings.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSettings.Rows)
                        {
                            int Part = 0;
                            int QUes = 0;
                            string NoPart = Convert.ToString(dr["PartNo"]);
                            string NoQues = Convert.ToString(dr["NO_Ques"]);
                            string masterID = string.Empty;
                            int.TryParse(NoPart, out Part);
                            int.TryParse(NoQues, out QUes);
                            if (QUes > 0 && Part > 0)
                            {
                                drResult = dtMarks.NewRow();
                                QNO++;
                                string Qname = "Q" + QNO + "Mark";
                                string UnitNo = string.Empty;
                                string mark = " ";
                                string martxt = string.Empty;
                                string DescTotal = string.Empty;
                                string MaxMark = string.Empty;
                                string sub1 = Convert.ToString(dr["sub1"]);
                                string sub2 = Convert.ToString(dr["sub2"]);
                                string QVal = Convert.ToString(dr["Qno"]);

                                masterID = Convert.ToString(dr["MasterID"]);
                                if (QNO == 1)
                                {
                                    lblQ1Code.Text = masterID;
                                    lblRec.Text = masterID;
                                }
                                if (QNO == 2)
                                {
                                    lblQ2Code.Text = masterID;
                                    lblOb.Text = masterID;
                                }
                                if (QNO == 3)
                                {
                                    lblQ3Code.Text = masterID;
                                    lblint.Text = masterID;
                                }
                                if (QNO == 4)
                                    lblQ4Code.Text = masterID;
                                if (QNO == 5)
                                    lblQuiz.Text = masterID;
                                if (QNO == 6)
                                    lblAss.Text = masterID;
                                if (dtStuMarks.Rows.Count > 0)
                                {
                                    float markVal = 0;
                                    dtStuMarks.DefaultView.RowFilter = "MasterID='" + masterID + "'";
                                    DataView dvMark = dtStuMarks.DefaultView;
                                    if (dvMark.Count > 0)
                                    {
                                        //DescTotal = getMarkText(Convert.ToString(dvMark[0]["DescTotal"]));
                                        if (!chkretest.Checked)
                                        {
                                            mark = getMarkText(Convert.ToString(dvMark[0]["Marks"]));
                                            martxt = Convert.ToString(dvMark[0]["Marks"]);
                                        }
                                        else
                                        {
                                            mark = getMarkText(Convert.ToString(dvMark[0]["RetestMark"]));
                                            martxt = Convert.ToString(dvMark[0]["RetestMark"]);
                                        }


                                        if (ForMate == "2")
                                        {
                                            if (martxt == "-1" || martxt == "-16" || martxt == "-20")
                                                martxt = " ";

                                            if (QNO == 1)
                                            {

                                                txtQ1.Text = martxt;
                                                txtRec.Text = martxt;
                                            }
                                            if (QNO == 2)
                                            {
                                                txtQ2.Text = martxt;
                                                txtObser.Text = martxt;
                                            }
                                            if (QNO == 3)
                                            {
                                                txtQ3.Text = martxt;
                                                txtinternal.Text = martxt;
                                            }
                                            if (QNO == 4)
                                                txtQ4.Text = martxt;
                                            if (QNO == 5)
                                                txtQuizMark.Text = martxt;
                                            if (QNO == 6)
                                                txtAssignmntMark.Text = martxt;

                                            if (!chkretest.Checked)
                                                DescTotal = getMarkText(Convert.ToString(dvMark[0]["DescTotal"]));
                                            else
                                                DescTotal = getMarkText(Convert.ToString(dvMark[0]["RetestDescTotal"]));
                                            lblDescTotal.Text = DescTotal;

                                        }
                                    }
                                    else
                                    {

                                    }
                                    float.TryParse(mark, out markVal);
                                    grandtot = grandtot + markVal;
                                }
                                MaxMark = Convert.ToString(dr["Mark"]);
                                UnitNo = Convert.ToString(dr["CourseOutComeNo"]);

                                if (!string.IsNullOrEmpty(UnitNo) && dtCoSett.Rows.Count > 0)
                                {
                                    dtCoSett.DefaultView.RowFilter = " masterno='" + UnitNo + "'";
                                    DataView dvCo = dtCoSett.DefaultView;
                                    if (dvCo.Count > 0)
                                        UnitNo = Convert.ToString(dvCo[0]["template"]);
                                }
                                else
                                    UnitNo = "0";

                                drResult["appNo"] = appNo;
                                drResult["PartNo"] = NoPart;//PartName
                                drResult["PartName"] = getPartText(NoPart);
                                drResult["CourseOutComeNo"] = UnitNo;
                                drResult["QNo"] = QVal;
                                drResult["SubNo"] = lblSuNo.Text;
                                drResult["maxmrk"] = MaxMark;
                                drResult["criteria"] = lblCriteriaNO.Text;
                                drResult["StuMark"] = mark;
                                drResult["MasterID"] = masterID;
                                drResult["examCode"] = examCode;
                                drResult["sub1"] = sub1;
                                drResult["sub2"] = sub2;
                                dtMarks.Rows.Add(drResult);
                            }
                            //}
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Question Setting Not found')", true);
                    }
                    if (dtMarks.Rows.Count > 0)
                    {
                        double grandtotal = Math.Round(grandtot);
                        GridView3.DataSource = dtMarks;
                        GridView3.DataBind();//Setting Need

                        if (ForMate == "1")
                        {
                            GridView3.Visible = true;
                            Save.Visible = true;
                            Delete.Visible = true;
                            btnSave.Visible = false;
                            btnDelete.Visible = false;
                            lblTot.Visible = true;
                            Format2.Visible = false;
                            Lab.Visible = false;
                            Button1.Visible = false;
                            Button2.Visible = false;
                            lblTot.Visible = true;
                            lblGrandTotal.Visible = true;
                            lblGrandTotal.Text = getMarkText(gT.ToString());
                        }
                        else if (ForMate == "2")
                        {
                            Save.Visible = false;
                            Delete.Visible = false;
                            btnSave.Visible = true;
                            btnDelete.Visible = true;
                            Format2.Visible = true;
                            lblTot.Visible = false;
                            lblGrandTotal.Visible = false;
                            lblDescTotal.Visible = true;
                            Lab.Visible = false;
                            Button1.Visible = false;
                            Button2.Visible = false;
                            if (islab == "1" || islab.ToLower() == "true")
                            {
                                Save.Visible = false;
                                Delete.Visible = false;
                                btnSave.Visible = false;
                                btnDelete.Visible = false;
                                Format2.Visible = false;
                                lblTot.Visible = false;
                                lblGrandTotal.Visible = false;
                                lblDescTotal.Visible = false;
                                Lab.Visible = true;
                                Button1.Visible = true;
                                Button2.Visible = true;
                                //lblGrandTotal.Text = getMarkText(gT.ToString());
                            }
                            //lblDescTotal.Text = grandtotal.ToString();
                        }

                        else
                        {
                            GridView3.Visible = false;
                            Save.Visible = true;
                            Delete.Visible = true;
                            btnSave.Visible = false;
                            btnDelete.Visible = false;
                            lblTot.Visible = false;
                            Format2.Visible = false;
                            lblTot.Visible = false;
                            Lab.Visible = false;
                            Button1.Visible = false;
                            Button2.Visible = false;
                            lblGrandTotal.Visible = false;
                            lblGrandTotal.Text = getMarkText(gT.ToString());
                        }
                        divPopSpread.Visible = true;

                    }
                }
            }
        }
        catch
        {

        }
    }
    private string getPartText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "Part A";
                    break;
                case "2":
                    mark = "Part B";
                    break;
                case "3":
                    mark = "Part C";
                    break;
                case "4":
                    mark = "Part D";
                    break;
                case "5":
                    mark = "Part E";
                    break;
                case "6":
                    mark = "Part F";
                    break;
                case "7":
                    mark = "Part G";
                    break;
                case "8":
                    mark = "Part H";
                    break;
                case "9":
                    mark = "Part I";
                    break;
                case "10":
                    mark = "Part J";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    private string getSubText1(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "A";
                    break;
                case "2":
                    mark = "B";
                    break;
                case "3":
                    mark = "C";
                    break;
                case "4":
                    mark = "D";
                    break;
                case "5":
                    mark = "E";
                    break;
                case "6":
                    mark = "F";
                    break;
                case "7":
                    mark = "G";
                    break;
                case "8":
                    mark = "H";
                    break;
                case "9":
                    mark = "I";
                    break;
                case "10":
                    mark = "J";
                    break;

            }
        }
        catch
        {
        }
        return mark;
    }
    private string getSubText2(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "i";
                    break;
                case "2":
                    mark = "ii";
                    break;
                case "3":
                    mark = "iii";
                    break;
                case "4":
                    mark = "iv";
                    break;
                case "5":
                    mark = "v";
                    break;
                case "6":
                    mark = "vi";
                    break;
                case "7":
                    mark = "vii";
                    break;
                case "8":
                    mark = "viii";
                    break;
                case "9":
                    mark = "ix";
                    break;
                case "10":
                    mark = "x";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "   order by registration.serialno";
        }
        else
        {
            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "0")
            {
                strorder = "  ORDER BY Registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "   ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "  ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "   ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "  ORDER BY Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }
        return strorder;
    }



}