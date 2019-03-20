using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using InsproDataAccess;
using System.Configuration;

public partial class AttendanceMOD_StudentOnDutyEntryDetailsNew : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();

    InsproDirectAccess d2 = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable holiDateCheck = new Hashtable();
    DataRow drgridod = null;

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    static string sel_date = string.Empty;
    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string hours = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;
    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryCourseId = string.Empty;
    string qryHours = string.Empty;

    byte dayType = 0;
    byte totalHours = 0;
    int selected = 0;

    bool isSchool = false;
    bool cellclick = false;
    bool flag_true = false;
    bool isValidDate = false;
    bool isValidFromDate = false;
    bool isValidToDate = false;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    DateTime dtDummyDate = new DateTime();
    DateTime chk;  //mullai
    Hashtable hats = new Hashtable();

    bool isRollVisible = false;
    bool isRegVisible = false;
    bool isAdmitNoVisible = false;
    bool isStudentTypeVisible = false;

    #region
    DataTable dtoddetails = new DataTable();
    DataRow droddetails = null;
    DataTable dtdetail = new DataTable();
    DataRow drdetail = null;
    ArrayList arrayoddetails = new ArrayList();
    DateTime dtFromDates = new DateTime();
    DateTime dtToDates = new DateTime();
    //Hashtable snotag = new Hashtable();
    //Hashtable snonote = new Hashtable();
    //Hashtable rolltag = new Hashtable();
    //Hashtable rollnote = new Hashtable();
    #endregion

    Hashtable htHoursPerDay = new Hashtable();

    Institution institute;

    #endregion

    #region Attendance variable
    Hashtable hatroll = new Hashtable();
    Dictionary<string, string> SemInfoDet = new Dictionary<string, string>();
    Dictionary<string, int> HolidayInfoDet = new Dictionary<string, int>();
    double conductedDays = 0;
    double presentDays = 0;
    double absentDays = 0;
    double conductedHours = 0;
    double presentHours = 0;
    double absentHours = 0;
    double absentDaysPercentage = 0;
    double absentHoursPercentage = 0;
    string absentDaysPercentage1 = string.Empty;
    string absentHoursPercentage1 = string.Empty;
    string dum_tage_date = "", dum_tage_hrs;
    double leavfinaeamount = 0;
    double medicalLeaveDays = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;

    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;

    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;

    string[] string_session_values;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string isonumber = string.Empty;
    int inirow_count = 0;
    int demfcal, demtcal;
    string monthcal;
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    bool yesflag = false;
    int dum_diff_date, unmark;
    double per_leavehrs;
    double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    string leftlogo = "", rightlogo = "", leftlength = "", rightlength = "", multi_iso = string.Empty;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    int mmyycount;
    string dd = string.Empty;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    bool deptflag = false;
    static bool splhr_flag = false;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    TimeSpan ts;
    string diff_date;
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }

            if (!Request.FilePath.Contains("AttendanceHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                    return;
                }
            }

            if (!IsPostBack)
            {
                lblReasonErr.Text = string.Empty;
                txtStudent.Text = string.Empty;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDateOD.Attributes.Add("readonly", "readonly");
                txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDateOD.Attributes.Add("readonly", "readonly");
                txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

                chkStudentWise.Checked = false;
                divODEntryDetails.Visible = false;
                divAddStudents.Visible = false;
                divSearchAllStudents.Visible = true;
                divHalfHr.Visible = false;

                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;
                divUpdateOD.Visible = false;
                divDeleteOD.Visible = false;
                setLabelText();
                Bindcollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindSectionDetail();
                BindAttendanceRights();

                BindCollegeOD();
                BindBatchOD();
                BindDegreeOD();
                BindBranchOD();
                BindSemOD();
                BindSectionDetailOD();
                BindReason();
                BindHour();
                BindMinute();
                SetStudentWiseSettings();

                ddlInTimeHr.SelectedValue = DateTime.Now.ToString("hh");
                ddlInTimeMM.SelectedValue = DateTime.Now.ToString("mm");
                ddlInTimeSess.SelectedValue = DateTime.Now.ToString("tt");

                ddlOutTimeHr.SelectedValue = DateTime.Now.ToString("hh");
                ddlOutTimeMM.SelectedValue = DateTime.Now.ToString("mm");
                ddlOutTimeSess.SelectedValue = DateTime.Now.ToString("tt");

                //Init_Spread(FpShowODDetails, 0);
                //Init_Spread(FpStudentDetails, 1);
                this.txtStudent.Attributes.Add("onkeypress", "btnAddStudent_Click(this,'" + this.btnAddStudent.ClientID + "')");
                ddlPurpose.Attributes.Add("onfocus", "frelig()");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Admissionno"] = "0";
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "' ";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "' ";
                }
                ds.Clear();
                ht.Clear();
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                    ds = da.select_method(Master1, ht, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Admissionno"] = "1";
                        }
                    }
                }
                Session["attdaywisecla"] = "0";
                string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
                if (daywisecal.Trim() == "1")
                {
                    Session["attdaywisecla"] = "1";
                }
            }
        }
        catch (ThreadAbortException tt)
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dtoddetails"] != null)
            {
                Session.Remove("dtoddetails");
            }
            if (Session["arrColHdrNames2"] != null)
            {
                Session.Remove("arrColHdrNames2");
            }
        }
        callGridBind();
    }

    public void callGridBind()
    {
        if (Session["dtoddetails"] != null)
        {
            DataTable dtGrid = (DataTable)Session["dtoddetails"];

            gview.DataSource = dtGrid;
            gview.DataBind();
            gview.Visible = true;
        }
        else
        {
            gview.DataSource = null;
            gview.DataBind();
        }
    }

    #endregion Page Load

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBatch()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            collegeCode = string.Empty;
            userCode = string.Empty;
            ddlBatch.Items.Clear();
            ds.Clear();
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                //}
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year<>'' and batch_year<>'0' and batch_year<>'-1'  " + qryUserOrGroupCode + " order by batch_year desc";
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCollegeCode + qryUserOrGroupCode + "  order by c.Priority", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataBind();
                    ddlDegree.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlBranch.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCourseId + qryUserOrGroupCode + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSem()
    {
        try
        {
            ds.Clear();
            ddlSem.Items.Clear();
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCode = string.Empty;
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollegeCode + qryBatchYear + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSem.SelectedIndex = 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree where duration<>'0' " + qryDegreeCode + qryCollegeCode + " group by first_year_nonsemester";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSem.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            ds = new DataSet();
            ddlSec.Items.Clear();
            cblSec.Items.Clear();
            chkSec.Checked = false;
            txtSec.Enabled = false;
            txtSec.Text = "-- Select --";
            if (ddlCollege.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                //}
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                //}
            }
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            string qrysections = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qrysections = da.GetFunctionv("select distinct sections from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year in(" + batchYear + ")  " + qryUserOrGroupCode).Trim();
            }
            if (!string.IsNullOrEmpty(qrysections.Trim()))
            {
                string[] sectionsAll = qrysections.Trim().Split(new char[] { ',' });
                string sections = string.Empty;
                bool hasEmpty = false;
                if (sectionsAll.Length > 0)
                {
                    for (int sec = 0; sec < sectionsAll.Length; sec++)
                    {
                        if (!string.IsNullOrEmpty(sectionsAll[sec].Trim()))
                        {
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                        else if (!hasEmpty)
                        {
                            hasEmpty = true;
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sections.Trim()))
                {
                    string sqlnew = "select distinct sections from registration where batch_year in(" + Convert.ToString(batchYear).Trim() + ") and degree_code in(" + Convert.ToString(degreeCode).Trim() + ") and sections<>'-1' and sections<>' ' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") and sections in(" + sections + ") and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
                cblSec.DataSource = ds;
                cblSec.DataTextField = "sections";
                cblSec.DataValueField = "sections";
                cblSec.DataBind();
                for (int h = 0; h < cblSec.Items.Count; h++)
                {
                    cblSec.Items[h].Selected = true;
                }
                txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
                chkSec.Checked = true;
                txtSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
                txtSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void Init_Spread(int type = 0)//Farpoint.FpSpread FpSpread1, 
    {
        try
        {

            dtoddetails.Rows.Clear();
            dtoddetails.Columns.Clear();

            dtdetail.Rows.Clear();
            dtdetail.Columns.Clear();

            DataSet dsSettings = new DataSet();
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method_wo_parameter(Master1, "Text");
            }
            isRollVisible = ColumnHeaderVisiblity(0, dsSettings);
            isRegVisible = ColumnHeaderVisiblity(1, dsSettings);
            isAdmitNoVisible = ColumnHeaderVisiblity(2, dsSettings);
            isStudentTypeVisible = ColumnHeaderVisiblity(3, dsSettings);
            if (type == 0)
            {
                dtoddetails.Columns.Add("SNo");
                dtoddetails.Columns.Add("SNotag");
                dtoddetails.Columns.Add("SNonote");
                dtoddetails.Columns.Add("Roll No");
                dtoddetails.Columns.Add("Rolltag");
                dtoddetails.Columns.Add("Rollnote");
                dtoddetails.Columns.Add("Reg No");
                dtoddetails.Columns.Add("Admission No");
                dtoddetails.Columns.Add("Student Name");
                dtoddetails.Columns.Add("Degree");
                dtoddetails.Columns.Add("Branch");
                dtoddetails.Columns.Add("Sec");
                dtoddetails.Columns.Add("Purpose");
                dtoddetails.Columns.Add("From Date");
                dtoddetails.Columns.Add("To Date");
                dtoddetails.Columns.Add("Out Time");
                dtoddetails.Columns.Add("outtmetag");
                dtoddetails.Columns.Add("In Time");
                dtoddetails.Columns.Add("Type");
                dtoddetails.Columns.Add("Total NoOf Hrs");
                dtoddetails.Columns.Add("Total NoOf Days");

                droddetails = dtoddetails.NewRow();
                droddetails["SNo"] = "S.No";
                droddetails["Roll No"] = "Roll No";
                droddetails["Reg No"] = "Reg No";
                droddetails["Admission No"] = "Admission No";
                droddetails["Student Name"] = "Student Name";
                droddetails["Degree"] = "Degree";
                droddetails["Branch"] = "Branch";
                droddetails["Sec"] = "Sec";
                droddetails["Purpose"] = "Purpose";
                droddetails["From Date"] = "From Date";
                droddetails["To Date"] = "To Date";
                droddetails["Out Time"] = "Out Time";
                droddetails["In Time"] = "In Time";
                droddetails["Type"] = "Type";
                droddetails["Total NoOf Hrs"] = "Total No.Of Hrs";
                droddetails["Total NoOf Days"] = "Total No.Of Days";
                dtoddetails.Rows.Add(droddetails);

            }
            else
            {

                dtdetail.Columns.Add("sno");
                dtdetail.Columns.Add("snotag");
                dtdetail.Columns.Add("snonote");
                dtdetail.Columns.Add("snovalue");
                dtdetail.Columns.Add("Roll No");
                dtdetail.Columns.Add("Rolltag");
                dtdetail.Columns.Add("Rollnote");
                dtdetail.Columns.Add("Rollvalue");
                dtdetail.Columns.Add("Reg No");
                dtdetail.Columns.Add("Regtag");
                dtdetail.Columns.Add("Regnote");
                dtdetail.Columns.Add("Regvalue");
                dtdetail.Columns.Add("Admission No");
                dtdetail.Columns.Add("Admissiontag");
                dtdetail.Columns.Add("Admissionnote");
                dtdetail.Columns.Add("Admissionvalue");
                dtdetail.Columns.Add("Student Name");
                dtdetail.Columns.Add("Studenttag");
                dtdetail.Columns.Add("Studentnote");
                dtdetail.Columns.Add("Studentvalue");
                dtdetail.Columns.Add("Semester");
                dtdetail.Columns.Add("Semestertag");
                dtdetail.Columns.Add("Semesternote");
                dtdetail.Columns.Add("Semestervalue");
                dtdetail.Columns.Add("OD Count");
                dtdetail.Columns.Add("ODtag");
                dtdetail.Columns.Add("ODnote");
                //dtdetail.Columns.Add("ReferDay");
                dtdetail.Columns.Add("ODvalue");
                dtdetail.Columns.Add("Select", System.Type.GetType("System.Boolean"));

                drdetail = dtdetail.NewRow();
                dtdetail.Rows.Add(drdetail);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBranch();
            BindSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSem_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            int count = 0;
            if (chkSec.Checked == true)
            {
                count++;
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = true;
                }
                txtSec.Text = "Section(" + (cblSec.Items.Count) + ")";
                txtSec.Enabled = true;
            }
            else
            {
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = false;
                }
                txtSec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            int commcount = 0;
            chkSec.Checked = false;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                if (cblSec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSec.Items.Count)
                {
                    chkSec.Checked = true;
                }
                txtSec.Text = "Section(" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSec_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Index Changed Events

    #region Show ON Duty Spread Events

    //protected void FpShowODDetails_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        cellclick = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void FpShowODDetails_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        string ar = e.CommandArgument.ToString();
    //        string[] spitval = ar.Split(',');
    //        string[] spitrow = spitval[0].Split('=');
    //        string actrow = spitrow[1].ToString();
    //        string[] spiticol = spitval[1].Split('=');
    //        string[] spitvn = spiticol[1].Split('}');
    //        string actcol = spitvn[0].ToString();
    //        btnOnDutyDelete.Visible = false;
    //        btnOnDutyUpdate.Visible = false;
    //        divUpdateOD.Visible = false;
    //        divDeleteOD.Visible = false;
    //        if (flag_true == false && actrow == "0")
    //        {
    //            string seltext = FpShowODDetails.Sheets[0].Cells[0, 16].Value.ToString();
    //            int setval = 0;
    //            if (seltext.Trim() == "1")
    //            {
    //                btnOnDutyDelete.Visible = true;
    //                divDeleteOD.Visible = true;
    //                if (FpShowODDetails.Sheets[0].RowCount == 2)
    //                {
    //                    btnOnDutyUpdate.Visible = true;
    //                    divUpdateOD.Visible = true;
    //                }
    //                else
    //                {
    //                    btnOnDutyUpdate.Visible = false;
    //                    divUpdateOD.Visible = false;
    //                }
    //            }
    //            else
    //            {
    //                btnOnDutyDelete.Visible = false;
    //                btnOnDutyUpdate.Visible = false;
    //                divUpdateOD.Visible = false;
    //                divDeleteOD.Visible = false;
    //            }
    //            for (int j = 1; j < Convert.ToInt16(FpShowODDetails.Sheets[0].RowCount); j++)
    //            {
    //                if (seltext != "System.Object" && seltext.Trim() != "-1")
    //                {
    //                    FpShowODDetails.Sheets[0].Cells[j, 16].Value = seltext.ToString();
    //                }
    //            }
    //            flag_true = true;
    //        }
    //        else if (flag_true == false && actrow != "0")
    //        {
    //            int setval = 0;
    //            for (int j = 1; j < Convert.ToInt16(FpShowODDetails.Sheets[0].RowCount); j++)
    //            {
    //                int isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[j, 16].Value);
    //                if (isval == 1)
    //                {
    //                    setval++;
    //                }
    //            }
    //            if (setval == 1)
    //            {
    //                btnOnDutyDelete.Visible = true;
    //                btnOnDutyUpdate.Visible = true;
    //                divUpdateOD.Visible = true;
    //                divDeleteOD.Visible = true;
    //            }
    //            else if (setval > 1)
    //            {
    //                btnOnDutyDelete.Visible = true;
    //                btnOnDutyUpdate.Visible = false;
    //                divUpdateOD.Visible = false;
    //                divDeleteOD.Visible = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void FpShowODDetails_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsg.Text = string.Empty;
    //        divPopAlert.Visible = false;
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        if (cellclick == true)
    //        {
    //            string activerow = string.Empty;
    //            string activecol = string.Empty;
    //            activerow = FpShowODDetails.ActiveSheetView.ActiveRow.ToString();
    //            activecol = FpShowODDetails.ActiveSheetView.ActiveColumn.ToString();
    //            divODEntryDetails.Visible = false;
    //            BindReason();
    //            //string activerow = Convert.ToString(res).Trim();
    //            string retAppNo = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag).Trim();
    //            string retroll = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text).Trim();
    //            string purpose = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text).Trim();
    //            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    //            FpStudentDetails.Sheets[0].Columns[6].CellType = chkcell;
    //            string attedancetype = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text;

    //            //string retAppNo1 = (gviewODDetails.Rows[0].FindControl("lblsnotag") as Label).Text.Trim();
    //            //string retroll1 = (gviewODDetails.Rows[0].FindControl("lblroll") as Label).Text.Trim();
    //            //string purpose1 = (gviewODDetails.Rows[0].FindControl("lblpurpose") as Label).Text.Trim();

    //            //string attedancetype

    //            string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code AND r.app_no='" + retAppNo + "'";
    //            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
    //            int sno = 0;
    //            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
    //            if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
    //            {
    //                divODEntryDetails.Visible = true;
    //                sno++;
    //                string studname = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["stud_name"]).Trim();
    //                string app_No = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["app_No"]).Trim();
    //                string rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["roll_no"]).Trim();
    //                string regno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Reg_no"]).Trim();
    //                string sem = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["current_semester"]).Trim();
    //                string degreecode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]).Trim();
    //                string batchval = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["batch_year"]).Trim();
    //                string section = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["sections"]).Trim();
    //                string collegeCode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["college_code"]).Trim();
    //                string courseId = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Course_Id"]).Trim();
    //                string admissionno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Roll_Admit"]).Trim();
    //                attendenace(degreecode, sem);
    //                ddlCollegeOD.Enabled = false;
    //                ddlBatchOD.Enabled = false;
    //                ddlBranchOD.Enabled = false;
    //                ddlDegreeOD.Enabled = false;
    //                ddlSemOD.Enabled = false;
    //                ddlSecOD.Enabled = false;
    //                if (ddlCollegeOD.Items.Count > 0)
    //                {
    //                    ddlCollegeOD.SelectedValue = collegeCode;
    //                    ddlCollegeOD_SelectedIndexChanged(new object(), new EventArgs());
    //                }
    //                if (ddlBatchOD.Items.Count > 0)
    //                {
    //                    ddlBatchOD.SelectedValue = batchval;
    //                    ddlBatchOD_SelectedIndexChanged(new object(), new EventArgs());
    //                }
    //                if (ddlDegreeOD.Items.Count > 0)
    //                {
    //                    ddlDegreeOD.SelectedValue = courseId;
    //                    ddlDegreeOD_SelectedIndexChanged(new object(), new EventArgs());
    //                }
    //                if (ddlBranchOD.Items.Count > 0)
    //                {
    //                    ddlBranchOD.SelectedValue = degreecode;
    //                    ddlBranchOD_SelectedIndexChanged(new object(), new EventArgs());
    //                }
    //                if (ddlSemOD.Items.Count > 0)
    //                {
    //                    ddlSemOD.SelectedValue = sem;
    //                    ddlSemOD_SelectedIndexChanged(new object(), new EventArgs());
    //                }
    //                if (ddlSecOD.Items.Count > 0)
    //                {
    //                    ddlSecOD.Enabled = false;
    //                    ddlSecOD.SelectedIndex = 0;
    //                    if (section.Trim().ToLower() != "")
    //                    {
    //                        ddlSecOD.SelectedValue = section;
    //                    }
    //                }
    //                divPopODAlert.Visible = false;
    //                Init_Spread(FpStudentDetails, 1);
    //                FpStudentDetails.Sheets[0].RowCount = 0;
    //                FpStudentDetails.Sheets[0].RowCount = FpStudentDetails.Sheets[0].RowCount + 1;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Tag = batchval;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Note = collegeCode;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = textcel_type;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Tag = degreecode;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = rollno;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Value = rollno;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = regno;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Tag = section;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Note = courseId;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = textcel_type;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = admissionno;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = studname;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = sem;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = app_No;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].CellType = new Farpoint.CheckBoxCellType();
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Locked = true;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Value = 1;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
    //                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = sem;

    //                drdetail = dtdetail.NewRow();
    //                drdetail["sno"] = Convert.ToString(sno);
    //                drdetail["snotag"] = batchval;
    //                drdetail["snonote"] = collegeCode;
    //                drdetail["Roll No"] = rollno;
    //                drdetail["Rolltag"] = degreecode;
    //                drdetail["Rollvalue"] = rollno;
    //                drdetail["Reg No"] = regno;
    //                drdetail["Regtag"] = section;
    //                drdetail["Regnote"] = courseId;
    //                drdetail["Admission No"] = admissionno;
    //                drdetail["Student Name"] = studname;
    //                drdetail["Semester"] = sem;
    //                drdetail["Semestertag"] = app_No;
    //                drdetail["Select"] = true;
    //                dtdetail.Rows.Add(drdetail);

    //                //dtdetail.Columns.Add("OD Count");
    //                //dtdetail.Columns.Add("ODtag");
    //                //dtdetail.Columns.Add("ODnote");
    //                //dtdetail.Columns.Add("ODvalue");

    //                txtNoOfHours.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Tag).Trim();
    //                txtFromDateOD.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text).Trim();
    //                txtToDate.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text).Trim();

    //                txtNoOfHours.Text = (gviewODDetails.Rows[0].FindControl("lblouttmetag") as Label).Text;
    //                txtFromDateOD.Text = (gviewODDetails.Rows[0].FindControl("lblfrmdate") as Label).Text;
    //                txtToDate.Text = (gviewODDetails.Rows[0].FindControl("lbltodate") as Label).Text;


    //                btnPopSaveOD.Text = "Update";
    //                string getouttime = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text;
    //                string getintime = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text;

    //                string getouttime1 = (gviewODDetails.Rows[0].FindControl("lblouttme") as Label).Text;
    //                string getintime1 = (gviewODDetails.Rows[0].FindControl("lblintme") as Label).Text;

    //                string[] splitouttime = getouttime.Split(new char[] { ' ' });
    //                string[] splitintime = getintime.Split(new char[] { ' ' });
    //                string splitedouttime = splitouttime[0].ToString();
    //                string splitedoutmeridian = splitouttime[1].ToString();
    //                string splitedintime = splitintime[0].ToString();
    //                string splitedinmeridian = splitintime[1].ToString();
    //                string[] hourList = txtNoOfHours.Text.Split(',');
    //                if (hourList.Length > 0)
    //                {
    //                    cblHours.Items.Clear();
    //                    int item = 0;
    //                    foreach (string hrslst in hourList)
    //                    {
    //                        cblHours.Items.Add(new ListItem(hrslst, hrslst));
    //                        cblHours.Items[item].Selected = true;
    //                        item++;
    //                    }
    //                }
    //                string[] outtime = splitedouttime.Split(new char[] { ':' });
    //                string hour = outtime[0];
    //                string min = outtime[1];
    //                if (outtime[0].Length == 1)
    //                {
    //                    hour = "0" + outtime[0];
    //                }
    //                if (min.Length == 1)
    //                {
    //                    min = "0" + outtime[1];
    //                }
    //                string[] intime = splitedintime.Split(new char[] { ':' });
    //                string outhr = intime[0].ToString();
    //                string outmm = intime[1].ToString();
    //                if (outhr.Length == 1)
    //                {
    //                    outhr = "0" + outhr;
    //                }
    //                if (outmm.Length == 1)
    //                {
    //                    outmm = "0" + outmm;
    //                }
    //                ddlOutTimeHr.Enabled = false;
    //                ddlOutTimeMM.Enabled = false;
    //                ddlOutTimeSess.Enabled = false;
    //                ddlOutTimeHr.Text = hour;
    //                ddlOutTimeMM.Text = min;
    //                ddlOutTimeSess.Text = splitedoutmeridian;
    //                ddlInTimeHr.Enabled = false;
    //                ddlInTimeMM.Enabled = false;
    //                ddlInTimeSess.Enabled = false;
    //                ddlInTimeHr.Text = outhr;
    //                ddlInTimeMM.Text = outmm;
    //                ddlInTimeSess.Text = splitedinmeridian;
    //                purpose = da.GetFunction("select textcode from TextValTable where TextCriteria='Attrs' and textval='" + purpose + "' and college_code='" + collegeCode + "'");
    //                if (purpose.Trim() != "" && purpose.Trim() != "0")
    //                {
    //                    if (ddlPurpose.Items.Count > 0)
    //                    {
    //                        ddlPurpose.SelectedValue = purpose;
    //                    }
    //                }
    //                BindAttendanceRights();
    //                ddlAttendanceOption.Enabled = false;
    //                if (ddlAttendanceOption.Items.Count > 0)
    //                {
    //                    ListItem list = new ListItem(attedancetype.Trim().ToUpper(), attedancetype.Trim().ToUpper());

    //                    if (ddlAttendanceOption.Items.Contains(list))
    //                    {
    //                        ddlAttendanceOption.Text = attedancetype;
    //                    }
    //                }
    //                btnPopSaveOD.Enabled = true;
    //                FpStudentDetails.Sheets[0].PageSize = FpStudentDetails.Sheets[0].RowCount;
    //                //FpStudentDetails.Width = 880;
    //                FpStudentDetails.Height = 300;
    //                FpStudentDetails.SaveChanges();
    //            }
    //            else
    //            {
    //                lblAlertMsg.Text = "No Record Found";
    //                divPopAlert.Visible = true;
    //                return;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void FpCalculateCGPA_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        FpCalculateCGPA.SaveChanges();
    //        int r = FpCalculateCGPA.Sheets[0].ActiveRow;
    //        int j = FpCalculateCGPA.Sheets[0].ActiveColumn;
    //        if (r == 0 && j == 0)
    //        {
    //            int val = 0;
    //            int.TryParse(Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[r, j].Value).Trim(), out val);
    //            for (int row = 1; row < FpCalculateCGPA.Sheets[0].RowCount; row++)
    //            {
    //                if (val == 1)
    //                    FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 1;
    //                else
    //                    FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 0;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    #endregion Show ON Duty Spread Events

    #region Button Events

    #region Button Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            arrayoddetails.Clear();

            DateTime dtm = new DateTime();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divUpdateOD.Visible = false;
            divDeleteOD.Visible = false;

            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;
            orderBy = string.Empty;
            orderBySetting = string.Empty;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;
            isValidDate = false;
            isValidFromDate = false;
            isValidToDate = false;

            fromDate = Convert.ToString(txtFromDate.Text).Trim();
            toDate = Convert.ToString(txtToDate.Text).Trim();
            DataSet dsODStudentDetails = new DataSet();
            DataSet dsDegreeDetails = new DataSet();
            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = string.Empty;
                qryCollegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = string.Empty;
                qryBatchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                courseId = string.Empty;
                qryCourseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = string.Empty;
                qryDegreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semester = string.Empty;
                qrySemester = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + li.Value + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.current_semester in(" + semester + ")";
                }
            }
            if (cblSec.Items.Count > 0) // Modify by jairam 07-08-2017 
            {
                section = string.Empty;
                qrySection = string.Empty;
                // foreach (ListItem li in ddlSec.Items)
                for (int li = 0; li < cblSec.Items.Count; li++)
                {
                    if (cblSec.Items[li].Selected == true)//if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(section))
                        {
                            section = "'" + cblSec.Items[li].Text + "'";
                        }
                        else
                        {
                            section += ",'" + cblSec.Items[li].Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and sections in(" + section + ")";
                }
            }
            else
            {
                section = string.Empty;
                qrySection = string.Empty;
            }
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                //qryDate = " and convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                // qryDate = "and (convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105) between  '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "')";//Rajkumar on 

                //=============Commond on 5-3-2019
                // qryDate = " and (convert(datetime,od.fromdate,105) >= '" + dtFromDate.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105)>='" + dtFromDate.ToString("MM/dd/yyyy") + "') and  (convert(datetime,od.fromdate,105) <='" + dtToDate.ToString("MM/dd/yyyy") + "' or convert(datetime,od.Todate,105)<= '" + dtToDate.ToString("MM/dd/yyyy") + "')";
                qryDate = "  and od.day between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                //=============================

                //qryDate = "and (convert(datetime,od.fromdate,105) >= '" + dtFromDate.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105)>='" + dtFromDate.ToString("MM/dd/yyyy") + "') and  convert(datetime,od.fromdate,105) <='" + dtToDate.ToString("MM/dd/yyyy") + "' or (convert(datetime,od.Todate,105)<= '" + dtToDate.ToString("MM/dd/yyyy") + "')";//Rajkumar on 

            }
            orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
            orderBySetting = orderBySetting.Trim();
            orderBy = "ORDER BY rollNoLen,r.roll_no"; //"ORDER BY fromdate,rollNoLen,r.roll_no";
            switch (orderBySetting)
            {
                case "0":
                    orderBy = "ORDER BY rollNoLen,r.roll_no"; //"ORDER BY fromdate,rollNoLen,r.roll_no";
                    break;
                case "1":
                    orderBy = "ORDER BY regNoLen,r.Reg_No";//"ORDER BY fromdate,regNoLen,r.Reg_No";
                    break;
                case "2":
                    orderBy = "ORDER BY r.Stud_Name";//"ORDER BY fromdate,r.Stud_Name";
                    break;
                case "0,1,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No,r.stud_name";//"ORDER BY fromdate,rollNoLen,r.roll_no,regNoLen,r.Reg_No,r.stud_name";
                    break;
                case "0,1":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No";//"ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No";
                    break;
                case "1,2":
                    orderBy = "ORDER BY regNoLen,r.Reg_No,r.Stud_Name";//"ORDER BY fromdate,regNoLen,r.Reg_No,r.Stud_Name";
                    break;
                case "0,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,r.Stud_Name";//"ORDER BY fromdate,rollNoLen,r.roll_no,r.Stud_Name";
                    break;
                default:
                    orderBy = "ORDER BY rollNoLen,r.roll_no";//"ORDER BY fromdate,rollNoLen,r.roll_no";
                    break;
            }
            Farpoint.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && isValidFromDate && isValidToDate)
            {
                //qry = "select distinct r.college_code,r.roll_no,r.reg_no,r.Roll_Admit,stud_name,purpose,fromdate,todate,outtime,intime,attnd_type,len(r.Reg_No),len(r.roll_no),no_of_hourse,hourse ,r.degree_code,r.sections,r.app_no from registration r,onduty_stud od where od.roll_no=r.roll_no and convert(datetime,fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' and r.roll_no in(select roll_no from registration where degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedValue + "' and current_semester='" + ddlSem.SelectedItem.Value + "' " + qrySection + ")  " + orderBy;
                qry = "select distinct r.college_code,r.Current_Semester,r.Batch_Year,r.roll_no,r.reg_no,r.Roll_Admit,r.stud_name,od.purpose,convert(varchar, od.fromdate, 103) as fromdate,convert(varchar, od.todate, 103)  as todate,convert(varchar, od.outtime, 108) as outtime,convert(varchar, od.intime, 108) as intime,od.attnd_type,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,od.no_of_hourse,od.hourse,r.degree_code,r.sections,r.app_no,convert(varchar, od.day,103) as odday,od.day  from registration r,onduty_stud od where od.roll_no=r.roll_no " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryDate + " " + orderBy + " ,odday";
                dsODStudentDetails.Clear();
                dsODStudentDetails = da.select_method_wo_parameter(qry, "text");
                qry = "select d.Degree_Code,(c.Course_Name ) as degreename,(dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id ; select No_of_hrs_per_day,degree_code,semester from periodattndschedule where degree_code in(" + degreeCode + ") and semester in(" + semester + ")";
                dsDegreeDetails = da.select_method_wo_parameter(qry, "text");

                string fromDates = txtFromDate.Text;
                string toDates = txtToDate.Text;
                DateTime dt11 = new DateTime();
                string[] dsplits = fromDates.Split(new Char[] { '/' });
                dt11 = Convert.ToDateTime(dsplits[1].ToString() + "/" + dsplits[0].ToString() + "/" + dsplits[2].ToString());

                DateTime dt12 = new DateTime();
                dsplits = toDates.Split(new Char[] { '/' });
                dt12 = Convert.ToDateTime(dsplits[1].ToString() + "/" + dsplits[0].ToString() + "/" + dsplits[2].ToString());

                ht.Clear();
                ht.Add("degree_code", int.Parse(ddlBranch.SelectedValue));
                ht.Add("sem", int.Parse(ddlSem.SelectedValue));
                ht.Add("from_date", dt11);
                ht.Add("to_date", dt12);
                ht.Add("coll_code", int.Parse(ddlCollege.SelectedValue));
                int iscount = 0;
                DataSet ds2 = new DataSet();




                string qrys = "select isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt11 + "' and '" + dt12 + "' and degree_code=" + ddlBranch.SelectedValue + " and semester=" + ddlSem.SelectedValue + "";
                ds2.Reset();
                ds2.Dispose();
                ds2 = da.select_method(qrys, ht, "Text");
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(ds2.Tables[0].Rows[0]["cnt"].ToString());
                }
                ht.Add("iscount", iscount);
                DataSet ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");

                Hashtable holiday_table = new Hashtable();
                holiday_table.Clear();
                if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                {
                    for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                    {
                        if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                        {
                            holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                        }
                    }
                }
                if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                    {
                        if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                        {
                            holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                        }
                    }
                }
                if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                    {
                        if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                        {
                            holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                        }
                    }
                }


                dsODStudentDetails.Tables[0].DefaultView.RowFilter = "isnull(odday,'1/1/1900 12:00:00 AM')<>'1/1/1900 12:00:00 AM'";
                DataView dtviewOld = dsODStudentDetails.Tables[0].DefaultView;
                if (dtviewOld.Table.Rows.Count > 0)
                {
                    if (dsODStudentDetails.Tables.Count > 0 && dsODStudentDetails.Tables[0].Rows.Count > 0)
                    {
                        Init_Spread(0);
                        //  DataTable dtstudent = dsODStudentDetails.Tables[0].DefaultView.ToTable(true, "roll_no", "fromdate", "todate", "odday", "hourse");
                        DataTable dtstudent = dsODStudentDetails.Tables[0].DefaultView.ToTable();
                        int serialNo = 0;
                        Hashtable rolladd = new Hashtable();
                        //   foreach (DataRow drODStudent in dtstudent.Rows)
                        for (int j = 0; j < dtstudent.Rows.Count; j++)
                        {

                            string appNo = string.Empty;
                            string rollNo = string.Empty;
                            string regNo = string.Empty;
                            string admitNo = string.Empty;
                            string studentName = string.Empty;
                            string fromDateNew = string.Empty;
                            string toDateNew = string.Empty;
                            string outTime = string.Empty;
                            string inTime = string.Empty;
                            string purpose = string.Empty;
                            string degreeName = string.Empty;
                            string departmentName = string.Empty;
                            string collegeCodeNew = string.Empty;
                            string batchYearNew = string.Empty;
                            string degreeCodeNew = string.Empty;
                            string currentSemester = string.Empty;
                            string sectionNew = string.Empty;
                            string attendanceTypeVal = string.Empty;
                            string attendanceType = string.Empty;
                            string noOfHrs = string.Empty;
                            double noOfHrsCount = 0;
                            double noOfDays = 0;
                            double totalHoursNew = 0;
                            double remainderDay = 0;
                            string hours = string.Empty;
                            DateTime dtFromDateNew = new DateTime();
                            DateTime dtToDateNew = new DateTime();
                            DateTime dtOutTimeNew = new DateTime();
                            DateTime dtInTimeNew = new DateTime();
                            string nextroll = string.Empty;
                            Boolean roll_per = false;

                            rollNo = Convert.ToString(dtstudent.Rows[j]["roll_no"]).Trim();
                            string day = Convert.ToString(dtstudent.Rows[j]["day"]).Trim();

                            DateTime dtms = new DateTime();
                            dtms = Convert.ToDateTime(day);

                            if (dtstudent.Rows.Count > j + 1)
                            {
                                nextroll = Convert.ToString(dtstudent.Rows[j + 1]["roll_no"]).Trim();
                                if (rollNo == nextroll)
                                    roll_per = true;
                                else
                                {
                                    //rolladd.Clear();
                                    roll_per = false;
                                }

                            }
                            if (!rolladd.ContainsKey(rollNo))
                            {
                                string[] hr = Convert.ToString(dtstudent.Rows[j]["hourse"]).Split(',');
                                if (!holiday_table.ContainsKey(dtms))
                                {
                                    int len = hr.Length;
                                    rolladd.Add(rollNo, len);
                                }
                            }
                            else
                            {
                                string val = Convert.ToString(rolladd[rollNo]);
                                int tolen = 0;
                                int.TryParse(val, out tolen);
                                if (!holiday_table.ContainsKey(dtms))
                                {
                                    string[] hr = Convert.ToString(dtstudent.Rows[j]["hourse"]).Split(',');
                                    int len = hr.Length;
                                    tolen += len;
                                    rolladd[rollNo] = tolen;
                                }
                            }

                            if (roll_per == false)
                            {
                                //DataView dvinfo = dsODStudentDetails.Tables[0].DefaultView;
                                //dvinfo.Sort = "no_of_hourse desc";
                                appNo = Convert.ToString(dtstudent.Rows[j]["app_no"]);
                                rollNo = Convert.ToString(dtstudent.Rows[j]["roll_no"]);
                                regNo = Convert.ToString(dtstudent.Rows[j]["reg_no"]);
                                admitNo = Convert.ToString(dtstudent.Rows[j]["Roll_Admit"]);
                                studentName = Convert.ToString(dtstudent.Rows[j]["stud_name"]);
                                outTime = Convert.ToString(dtstudent.Rows[j]["outtime"]);
                                inTime = Convert.ToString(dtstudent.Rows[j]["intime"]);
                                purpose = Convert.ToString(dtstudent.Rows[j]["purpose"]);
                                collegeCodeNew = Convert.ToString(dtstudent.Rows[j]["college_code"]);
                                batchYearNew = Convert.ToString(dtstudent.Rows[j]["Batch_Year"]);
                                degreeCodeNew = Convert.ToString(dtstudent.Rows[j]["degree_code"]);
                                currentSemester = Convert.ToString(dtstudent.Rows[j]["Current_Semester"]);
                                sectionNew = Convert.ToString(dtstudent.Rows[j]["sections"]);
                                attendanceTypeVal = Convert.ToString(dtstudent.Rows[j]["attnd_type"]);
                                attendanceType = Convert.ToString(dtstudent.Rows[j]["attnd_type"]);
                                noOfHrs = Convert.ToString(rolladd[rollNo]);
                                hours = Convert.ToString(dtstudent.Rows[j]["hourse"]);

                                DataView dvDegreeName = new DataView();
                                DataView dvPeriodDetails = new DataView();
                                if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsDegreeDetails.Tables[0].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "'";
                                    dvDegreeName = dsDegreeDetails.Tables[0].DefaultView;
                                    if (dvDegreeName.Count > 0)
                                    {
                                        degreeName = Convert.ToString(dvDegreeName[0]["degreename"]);
                                        departmentName = Convert.ToString(dvDegreeName[0]["dept_acronym"]);
                                    }
                                }
                                if (dsDegreeDetails.Tables.Count > 1 && dsDegreeDetails.Tables[1].Rows.Count > 0)
                                {
                                    dsDegreeDetails.Tables[1].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "' and semester='" + currentSemester + "'";
                                    dvDegreeName = dsDegreeDetails.Tables[1].DefaultView;
                                    if (dvDegreeName.Count > 0)
                                    {
                                        string totalHours = Convert.ToString(dvDegreeName[0]["No_of_hrs_per_day"]);
                                        double.TryParse(totalHours, out totalHoursNew);
                                    }
                                }
                                noOfHrsCount = 0;
                                noOfDays = 0;
                                string noofODday = string.Empty;
                                double.TryParse(Convert.ToString(noOfHrs), out noOfHrsCount);
                                noOfDays = noOfHrsCount / totalHoursNew;
                                string noofday = Convert.ToString(noOfDays);
                                remainderDay = 0;
                                remainderDay = noOfHrsCount % totalHoursNew;
                                string[] dayandhours = noofday.Split('.');
                                if (dayandhours.Length > 1)
                                {
                                    if (dayandhours[0] == "0")
                                        noofODday = Convert.ToString(remainderDay) + " Hour" + (remainderDay <= 1 ? "" : "s");
                                    else
                                        noofODday = Convert.ToString(dayandhours[0]).Trim() + " Day" + (dayandhours[0].Trim() == "1" ? "" : "s") + " " + Convert.ToString(remainderDay) + " Hour" + (remainderDay <= 1 ? "" : "s");
                                }
                                else
                                {
                                    noofODday = Convert.ToString(dayandhours[0]) + " Day" + (dayandhours[0].Trim() == "1" ? "" : "s");
                                }
                                droddetails = dtoddetails.NewRow();
                                serialNo++;
                                droddetails["SNo"] = Convert.ToString(serialNo).Trim();
                                droddetails["SNotag"] = Convert.ToString(appNo).Trim();
                                droddetails["SNonote"] = Convert.ToString(degreeCodeNew).Trim();

                                droddetails["Roll No"] = Convert.ToString(rollNo).Trim();
                                droddetails["Rolltag"] = Convert.ToString(collegeCodeNew).Trim();
                                droddetails["Rollnote"] = Convert.ToString(currentSemester).Trim();

                                droddetails["Reg No"] = Convert.ToString(regNo).Trim();
                                droddetails["Admission No"] = Convert.ToString(admitNo).Trim();
                                droddetails["Student Name"] = Convert.ToString(studentName).Trim();
                                droddetails["Degree"] = Convert.ToString(degreeName).Trim();
                                droddetails["Branch"] = Convert.ToString(departmentName).Trim();
                                droddetails["Sec"] = Convert.ToString(sectionNew).Trim();
                                droddetails["Purpose"] = Convert.ToString(purpose).Trim();
                                droddetails["From Date"] = txtFromDate.Text;
                                droddetails["To Date"] = txtToDate.Text;
                                droddetails["Out Time"] = dtOutTimeNew.ToString("hh:mm:ss tt");
                                droddetails["outtmetag"] = Convert.ToString(hours).Trim();
                                droddetails["In Time"] = dtInTimeNew.ToString("hh:mm:ss tt");
                                dtoddetails.Rows.Add(droddetails);

                                if (!string.IsNullOrEmpty(attendanceTypeVal))
                                {
                                    attendanceType = GetAttendanceStatusName(attendanceTypeVal);
                                }
                                else
                                {
                                    attendanceType = "--";
                                }
                                droddetails["Type"] = Convert.ToString(attendanceType).Trim();
                                droddetails["Total NoOf Hrs"] = Convert.ToString(noOfHrs).Trim();
                                droddetails["Total NoOf Days"] = Convert.ToString(noofODday).Trim();
                            }
                        }
                    }
                }

                if (dsODStudentDetails.Tables.Count > 0 && dsODStudentDetails.Tables[0].Rows.Count > 0)
                {
                    //Init_Spread(FpShowODDetails, 0);
                    //Init_Spread(0);

                    //DataTable dtstudent = dsODStudentDetails.Tables[0].DefaultView.ToTable(true, "roll_no", "fromdate", "todate");

                    //int serialNo = 0;

                    #region old
                    //foreach (DataRow drODStudent in dtstudent.Rows)
                    //{
                    //    serialNo++;
                    //    string appNo = string.Empty;
                    //    string rollNo = string.Empty;
                    //    string regNo = string.Empty;
                    //    string admitNo = string.Empty;
                    //    string studentName = string.Empty;
                    //    string fromDateNew = string.Empty;
                    //    string toDateNew = string.Empty;
                    //    string outTime = string.Empty;
                    //    string inTime = string.Empty;
                    //    string purpose = string.Empty;
                    //    string degreeName = string.Empty;
                    //    string departmentName = string.Empty;
                    //    string collegeCodeNew = string.Empty;
                    //    string batchYearNew = string.Empty;
                    //    string degreeCodeNew = string.Empty;
                    //    string currentSemester = string.Empty;
                    //    string sectionNew = string.Empty;
                    //    string attendanceTypeVal = string.Empty;
                    //    string attendanceType = string.Empty;
                    //    string noOfHrs = string.Empty;
                    //    double noOfHrsCount = 0;
                    //    double noOfDays = 0;
                    //    double totalHoursNew = 0;
                    //    double remainderDay = 0;
                    //    string hours = string.Empty;
                    //    DateTime dtFromDateNew = new DateTime();
                    //    DateTime dtToDateNew = new DateTime();
                    //    DateTime dtOutTimeNew = new DateTime();
                    //    DateTime dtInTimeNew = new DateTime();
                    //    rollNo = Convert.ToString(drODStudent["roll_no"]).Trim();
                    //    fromDateNew = Convert.ToString(drODStudent["fromdate"]).Trim();
                    //    toDateNew = Convert.ToString(drODStudent["todate"]).Trim();

                    //    dsODStudentDetails.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "' and fromdate='" + fromDateNew + "' and todate='" + toDateNew + "'";

                    //    DataView dvinfo = dsODStudentDetails.Tables[0].DefaultView;
                    //    dvinfo.Sort = "no_of_hourse desc";
                    //    appNo = Convert.ToString(dvinfo[0]["app_no"]);
                    //    rollNo = Convert.ToString(dvinfo[0]["roll_no"]);
                    //    regNo = Convert.ToString(dvinfo[0]["reg_no"]);
                    //    admitNo = Convert.ToString(dvinfo[0]["Roll_Admit"]);
                    //    studentName = Convert.ToString(dvinfo[0]["stud_name"]);
                    //    outTime = Convert.ToString(dvinfo[0]["outtime"]);
                    //    inTime = Convert.ToString(dvinfo[0]["intime"]);
                    //    purpose = Convert.ToString(dvinfo[0]["purpose"]);
                    //    collegeCodeNew = Convert.ToString(dvinfo[0]["college_code"]);
                    //    batchYearNew = Convert.ToString(dvinfo[0]["Batch_Year"]);
                    //    degreeCodeNew = Convert.ToString(dvinfo[0]["degree_code"]);
                    //    currentSemester = Convert.ToString(dvinfo[0]["Current_Semester"]);
                    //    sectionNew = Convert.ToString(dvinfo[0]["sections"]);
                    //    attendanceTypeVal = Convert.ToString(dvinfo[0]["attnd_type"]);
                    //    attendanceType = Convert.ToString(dvinfo[0]["attnd_type"]);
                    //    noOfHrs = Convert.ToString(dvinfo[0]["no_of_hourse"]);
                    //    hours = Convert.ToString(dvinfo[0]["hourse"]);

                    //    //appNo = Convert.ToString(drODStudent["app_no"]).Trim();
                    //    //rollNo = Convert.ToString(drODStudent["roll_no"]).Trim();
                    //    //regNo = Convert.ToString(drODStudent["reg_no"]).Trim();
                    //    //admitNo = Convert.ToString(drODStudent["Roll_Admit"]).Trim();
                    //    //studentName = Convert.ToString(drODStudent["stud_name"]).Trim();
                    //    //outTime = Convert.ToString(drODStudent["outtime"]).Trim();
                    //    //inTime = Convert.ToString(drODStudent["intime"]).Trim();
                    //    //purpose = Convert.ToString(drODStudent["purpose"]).Trim();
                    //    //collegeCodeNew = Convert.ToString(drODStudent["college_code"]).Trim();
                    //    //batchYearNew = Convert.ToString(drODStudent["Batch_Year"]).Trim();
                    //    //degreeCodeNew = Convert.ToString(drODStudent["degree_code"]).Trim();
                    //    //currentSemester = Convert.ToString(drODStudent["Current_Semester"]).Trim();
                    //    //sectionNew = Convert.ToString(drODStudent["sections"]).Trim();
                    //    //attendanceTypeVal = Convert.ToString(drODStudent["attnd_type"]).Trim();
                    //    //attendanceType = Convert.ToString(drODStudent["attnd_type"]).Trim();
                    //    //noOfHrs = Convert.ToString(drODStudent["no_of_hourse"]).Trim();
                    //    //hours = Convert.ToString(drODStudent["hourse"]).Trim();
                    //    double.TryParse(noOfHrs, out noOfHrsCount);

                    //    DateTime.TryParseExact(fromDateNew, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDateNew);
                    //    DateTime.TryParseExact(toDateNew, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDateNew);
                    //    DateTime.TryParseExact(outTime, "HH:mm:ss", null, DateTimeStyles.None, out dtOutTimeNew);
                    //    DateTime.TryParseExact(inTime, "HH:mm:ss", null, DateTimeStyles.None, out dtInTimeNew);
                    //    DataView dvDegreeName = new DataView();
                    //    DataView dvPeriodDetails = new DataView();
                    //    if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                    //    {
                    //        dsDegreeDetails.Tables[0].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "'";
                    //        dvDegreeName = dsDegreeDetails.Tables[0].DefaultView;
                    //        if (dvDegreeName.Count > 0)
                    //        {
                    //            degreeName = Convert.ToString(dvDegreeName[0]["degreename"]);
                    //            departmentName = Convert.ToString(dvDegreeName[0]["dept_acronym"]);
                    //        }
                    //    }
                    //    if (dsDegreeDetails.Tables.Count > 1 && dsDegreeDetails.Tables[1].Rows.Count > 0)
                    //    {
                    //        dsDegreeDetails.Tables[1].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "' and semester='" + currentSemester + "'";
                    //        dvDegreeName = dsDegreeDetails.Tables[1].DefaultView;
                    //        if (dvDegreeName.Count > 0)
                    //        {
                    //            string totalHours = Convert.ToString(dvDegreeName[0]["No_of_hrs_per_day"]);
                    //            double.TryParse(totalHours, out totalHoursNew);
                    //        }
                    //    }
                    //    noOfHrsCount = 0;
                    //    noOfDays = 0;
                    //    string noofODday = string.Empty;
                    //    double.TryParse(Convert.ToString(noOfHrs), out noOfHrsCount);
                    //    noOfDays = noOfHrsCount / totalHoursNew;
                    //    string noofday = Convert.ToString(noOfDays);
                    //    remainderDay = 0;
                    //    remainderDay = noOfHrsCount % totalHoursNew;
                    //    string[] dayandhours = noofday.Split('.');
                    //    if (dayandhours.Length > 1)
                    //    {
                    //        if (dayandhours[0] == "0")
                    //            noofODday = Convert.ToString(remainderDay) + " Hour" + (remainderDay <= 1 ? "" : "s");
                    //        else
                    //            noofODday = Convert.ToString(dayandhours[0]).Trim() + " Day" + (dayandhours[0].Trim() == "1" ? "" : "s") + " " + Convert.ToString(remainderDay) + " Hour" + (remainderDay <= 1 ? "" : "s");
                    //    }
                    //    else
                    //    {
                    //        noofODday = Convert.ToString(dayandhours[0]) + " Day" + (dayandhours[0].Trim() == "1" ? "" : "s");
                    //    }

                    //    droddetails = dtoddetails.NewRow();

                    //    droddetails["SNo"] = Convert.ToString(serialNo).Trim();
                    //    droddetails["SNotag"] = Convert.ToString(appNo).Trim();
                    //    droddetails["SNonote"] = Convert.ToString(degreeCodeNew).Trim();

                    //    droddetails["Roll No"] = Convert.ToString(rollNo).Trim();
                    //    droddetails["Rolltag"] = Convert.ToString(collegeCodeNew).Trim();
                    //    droddetails["Rollnote"] = Convert.ToString(currentSemester).Trim();

                    //    droddetails["Reg No"] = Convert.ToString(regNo).Trim();
                    //    droddetails["Admission No"] = Convert.ToString(admitNo).Trim();
                    //    droddetails["Student Name"] = Convert.ToString(studentName).Trim();
                    //    droddetails["Degree"] = Convert.ToString(degreeName).Trim();
                    //    droddetails["Branch"] = Convert.ToString(departmentName).Trim();
                    //    droddetails["Sec"] = Convert.ToString(sectionNew).Trim();
                    //    droddetails["Purpose"] = Convert.ToString(purpose).Trim();
                    //    droddetails["From Date"] = dtFromDateNew.ToString("dd/MM/yyyy");
                    //    droddetails["To Date"] = dtToDateNew.ToString("dd/MM/yyyy");
                    //    droddetails["Out Time"] = dtOutTimeNew.ToString("hh:mm:ss tt");
                    //    droddetails["outtmetag"] = Convert.ToString(hours).Trim();
                    //    droddetails["In Time"] = dtInTimeNew.ToString("hh:mm:ss tt");
                    //    dtoddetails.Rows.Add(droddetails);

                    //    if (!string.IsNullOrEmpty(attendanceTypeVal))
                    //    {
                    //        attendanceType = GetAttendanceStatusName(attendanceTypeVal);
                    //    }
                    //    else
                    //    {
                    //        attendanceType = "--";
                    //    }
                    //    droddetails["Type"] = Convert.ToString(attendanceType).Trim();
                    //    droddetails["Total NoOf Hrs"] = Convert.ToString(noOfHrs).Trim();
                    //    droddetails["Total NoOf Days"] = Convert.ToString(noofODday).Trim();

                    //}
                    #endregion
                    divMainContents.Visible = true;
                    Session["dtoddetails"] = dtoddetails;

                    gview.DataSource = dtoddetails;
                    gview.DataBind();
                    RowHead(gview, 1);
                    gview.Visible = true;

                    for (int i = 0; i < gview.Rows.Count; i++)
                    {
                        gview.Rows[i].Cells[0].Visible = false;
                        gview.Rows[i].Cells[3].Visible = false;
                        gview.Rows[i].Cells[4].Visible = false;
                        gview.Rows[i].Cells[6].Visible = false;
                        gview.Rows[i].Cells[7].Visible = false;
                        gview.Rows[i].Cells[18].Visible = false;
                        if (!isRollVisible)
                        {
                            gview.Rows[i].Cells[5].Visible = false;
                        }
                        if (!isRegVisible)
                        {
                            gview.Rows[i].Cells[8].Visible = false;
                        }
                        if (!isAdmitNoVisible)
                        {
                            gview.Rows[i].Cells[9].Visible = false;
                        }
                        for (int cell = 0; cell < gview.Rows[i].Cells.Count; cell++)
                        {
                            if (gview.HeaderRow.Cells[cell].Text != "Student Name")
                            {
                                gview.Rows[i].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }

                }
                else
                {
                    divMainContents.Visible = false;
                    lblAlertMsg.Text = "No Record(s) Found";
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            //  da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Button Go Click

    protected void chkall_Indexchanged(object sender, EventArgs e)
    {
        //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        //if (uid != null && uid.Contains("ctl"))
        //{
        //string[] values = uid.Split('$');

        CheckBox chkall = (CheckBox)gview.Rows[0].FindControl("checkall");
        if (chkall.Checked)
        {
            if (gview.Rows.Count == 2)
            {
                divUpdateOD.Visible = true;
                btnOnDutyUpdate.Visible = true;
            }
            divDeleteOD.Visible = true;
            btnOnDutyDelete.Visible = true;
            for (int row = 1; row < gview.Rows.Count; row++)
            {
                CheckBox chk = (CheckBox)gview.Rows[row].FindControl("chck");
                chk.Checked = true;
            }
        }
        else
        {
            btnOnDutyDelete.Visible = false;
            btnOnDutyUpdate.Visible = false;
            divUpdateOD.Visible = false;
            divDeleteOD.Visible = false;
            for (int row = 1; row < gview.Rows.Count; row++)
            {
                CheckBox chk = (CheckBox)gview.Rows[row].FindControl("chck");
                chk.Checked = false;
            }
        }
        //}
    }

    protected void chk_Indexchanged(object sender, EventArgs e)
    {
        try
        {
            btnOnDutyDelete.Visible = false;
            btnOnDutyUpdate.Visible = false;
            divUpdateOD.Visible = false;
            divDeleteOD.Visible = false;

            if (flag_true == false)// && actrow != "0"
            {
                int setval = 0;
                for (int j = 1; j < Convert.ToInt16(gview.Rows.Count); j++)
                {
                    CheckBox chk = (CheckBox)gview.Rows[j].FindControl("chck");
                    if (chk.Checked)
                    {
                        setval++;
                    }
                }
                if (setval == 1)
                {
                    btnOnDutyDelete.Visible = true;
                    btnOnDutyUpdate.Visible = true;
                    divUpdateOD.Visible = true;
                    divDeleteOD.Visible = true;
                }
                else if (setval > 1)
                {
                    btnOnDutyDelete.Visible = true;
                    btnOnDutyUpdate.Visible = false;
                    divUpdateOD.Visible = false;
                    divDeleteOD.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void gview_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                //e.Row.Cells[e.Row.Cells.Count - 1].Text = "Select";
            }
            if (e.Row.RowIndex == 1)
            {
                //e.Row.Cells[e.Row.Cells.Count - 1].Text = "";
                //CheckBox chkall = new CheckBox();
                //chkall.ID = "checkall";
                //chkall.AutoPostBack = true;
                //chkall.Enabled = true;
                //chkall.CheckedChanged += new EventHandler(chkall_Indexchanged);
                //e.Row.Cells[1].Controls.Add(chkall);
                //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[0].ColumnSpan = e.Row.Cells.Count - 2;//16 arrayoddetails.Count - 1;
                //for (int cell = 2; cell < e.Row.Cells.Count; cell++)
                //{
                //    e.Row.Cells[cell].Visible = false;
                //}
                //if (chkall.Checked)
                //{
                //    chkall.Attributes.Add("onclick", "HeaderCheckBoxClick(this);");
                //}
            }
        }
    }

    //protected void ODDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (e.Row.RowIndex == 0)
    //        {
    //            for (int cell = 0; cell < e.Row.Cells.Count; cell++)
    //            {
    //                if (arrayoddetails.Count > 0)
    //                    e.Row.Cells[cell].Text = Convert.ToString(arrayoddetails[cell]);
    //            }
    //        }
    //        if (e.Row.RowIndex == 1)
    //        {
    //            e.Row.Cells[e.Row.Cells.Count - 1].Text = "";
    //            CheckBox chkall = new CheckBox();
    //            chkall.ID = "checkall";
    //            chkall.AutoPostBack = true;
    //            chkall.Enabled = true;
    //            chkall.CheckedChanged += new EventHandler(chkall_Indexchanged);
    //            e.Row.Cells[1].Controls.Add(chkall);
    //            e.Row.Cells[0].ColumnSpan = 16;// arrayoddetails.Count - 1;
    //            for (int cell = 2; cell < e.Row.Cells.Count; cell++)
    //            {
    //                e.Row.Cells[cell].Visible = false;
    //            }
    //            //if (chkall.Checked)
    //            //{
    //            //    chkall.Attributes.Add("onclick", "HeaderCheckBoxClick(this);");
    //            //}
    //        }
    //        if (e.Row.RowIndex != 0 && e.Row.RowIndex != 1)
    //        {
    //            CheckBox chk = (CheckBox)e.Row.FindControl("ctl00");
    //            chk.AutoPostBack = true;
    //            chk.Enabled = true;
    //            chk.CheckedChanged += new EventHandler(chk_Indexchanged);
    //        }
    //    }
    //}

    //protected void ODDetails_OnDataBound(object sender, EventArgs e)
    //{
    //    for (int row = 1; row < gviewODDetails.Rows.Count; row++)
    //    {
    //        for (int cell = 0; cell < gviewODDetails.Rows[row].Cells.Count; cell++)
    //        {
    //            if (gviewODDetails.HeaderRow.Cells[cell].Text.Trim() != "Student Name")
    //                gviewODDetails.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
    //        }
    //    }
    //}

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].Font.Name = "Book Antique";
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            CheckBox chkall = (gview.Rows[0].FindControl("checkall") as CheckBox);
            chkall.Visible = true;

            CheckBox chk = (gview.Rows[0].FindControl("chck") as CheckBox);
            chk.Visible = false;
        }
    }

    #region Button Add Click

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            gviewOD.Visible = false;
            divMainContents.Visible = false;
            divODEntryDetails.Visible = false;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            divHalfHr.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            txtStudent.Text = string.Empty;
            btnPopSaveOD.Text = "Save";
            txtFromDateOD.Attributes.Add("readonly", "readonly");
            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDateOD.Attributes.Add("readonly", "readonly");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindCollegeOD();
            BindBatchOD();
            BindDegreeOD();
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            BindReason();
            BindHour();
            BindMinute();
            if (ddlCollegeOD.Items.Count > 0)
            {
                ddlCollegeOD.Enabled = true;
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                ddlBatchOD.Enabled = true;
            }
            if (ddlDegreeOD.Items.Count > 0)
            {
                ddlDegreeOD.Enabled = true;
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                ddlBranchOD.Enabled = true;
            }
            if (ddlSemOD.Items.Count > 0)
            {
                ddlSemOD.Enabled = true;
            }
            if (ddlSecOD.Items.Count > 0)
            {
                ddlSecOD.Enabled = true;
            }
            if (ddlPurpose.Items.Count > 0)
            {
                ddlPurpose.Enabled = true;
            }
            if (ddlAttendanceOption.Items.Count > 0)
            {
                ddlAttendanceOption.Enabled = true;
            }
            if (ddlOutTimeHr.Items.Count > 0)
            {
                ddlOutTimeHr.Enabled = true;
            }
            if (ddlOutTimeMM.Items.Count > 0)
            {
                ddlOutTimeMM.Enabled = true;
            }
            if (ddlOutTimeSess.Items.Count > 0)
            {
                ddlOutTimeSess.Enabled = true;
            }
            if (ddlInTimeHr.Items.Count > 0)
            {
                ddlInTimeHr.Enabled = true;
            }
            if (ddlInTimeMM.Items.Count > 0)
            {
                ddlInTimeMM.Enabled = true;
            }
            if (ddlInTimeSess.Items.Count > 0)
            {
                ddlInTimeSess.Enabled = true;
            }
            Init_Spread(1);
            //ShowStudentsList(0);
            SetDefaultODEntry();
            divODEntryDetails.Visible = true;
            //-------------------added by Deepali on 6.4.18
            lblBatchOD.Visible = true;
            ddlBatchOD.Visible = true;
            lblDegreeOD.Visible = true;
            ddlDegreeOD.Visible = true;
            lblBranchOD.Visible = true;
            ddlBranchOD.Visible = true;
            lblSemOD.Visible = true;
            ddlSemOD.Visible = true;
            lblSecOD.Visible = true;
            ddlSecOD.Visible = true;
            ddlCollegeOD.Width = 80;

            #region com
            //DataTable dtOdHour = new DataTable();
            //DataRow drOdHour;

            //dtOdHour.Columns.Add("OdDate");
            //drOdHour = dtOdHour.NewRow();
            //dtOdHour.Rows.Add(drOdHour);

            //string[] dbsplfm = fromDate.Split('/');
            //string[] dbspltoo = toDate.Split('/');
            //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
            //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
            //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
            //DateTime dbconvto = Convert.ToDateTime(convto);

            //while (dbconvfrm <= dbconvto)
            //{
            //    drOdHour = dtOdHour.NewRow();
            //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
            //    string[] dtespl = Convert.ToString(spl[0]).Split('/'); if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
            //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
            //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
            //    drOdHour["OdDate"] = dte;
            //    dtOdHour.Rows.Add(drOdHour);
            //    dbconvfrm = dbconvfrm.AddDays(1);
            //}
            //string tothrs = string.Empty;
            //string fsthalfhrs = string.Empty;
            //string scndhalfhrs = string.Empty;
            //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            //if (htHours.Count == 0)
            //{
            //    attendenace("", "", 1);
            //}
            //foreach (DictionaryEntry parameter1 in htHours)
            //{
            //    string daytext = Convert.ToString(parameter1.Key);
            //    string noofhours = Convert.ToString(parameter1.Value);
            //    if (daytext == "full")
            //    {
            //        tothrs = noofhours;
            //    }
            //    if (daytext == "fn")
            //    {
            //        fsthalfhrs = noofhours;
            //    }
            //    if (daytext == "an")
            //    {
            //        scndhalfhrs = noofhours;
            //    }
            //}
            //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
            //if (tothrs != "" && tothrs != null)
            //{
            //    string temp = string.Empty;
            //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
            //    {
            //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
            //    }
            //}

            //gviewOD.DataSource = dtOdHour;
            //gviewOD.DataBind();
            //gviewOD.Visible = true;

            //if (gviewOD.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dthour.Rows.Count; i++)
            //    {
            //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
            //        {
            //            gviewOD.Columns[i + 1].Visible = true;
            //        }
            //        else
            //        {
            //            gviewOD.Columns[i + 1].Visible = false;
            //        }
            //    }
            //}
            #endregion
            //------------------------------------
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Button Add Click

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (gview.Visible == true)
                {
                    da.printexcelreportgrid(gview, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() :((ddlCollege.Items.Count>0)?Convert.ToString(ddlCollege.SelectedValue).Trim():"13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion Generate Excel

    #region Print PDF

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Student On Duty Details";
            string pagename = "StudentOndutydetails.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (gview.Visible == true)
            {
                // string ss = null;
                string ss = Session["usercode"].ToString();
                Printcontrol.loadspreaddetails(gview, pagename, rptheadname, 0, ss);
                Printcontrol.Visible = true;
            }
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Print PDF

    #region Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"))), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnODPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txtStudent.Text = string.Empty;
            txtStudent.Focus();
            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
            gviewOD.Visible = false;
            if (btnPopSaveOD.Text.Trim().ToLower() == "update")
            {
                gviewOD.Visible = true;
            }
            if (chkStudentWise.Checked)
            {
                DataTable dttab = (DataTable)ViewState["odtable"];
                gviewstudetails.DataSource = dttab;
                gviewstudetails.DataBind();
                gviewstudetails.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"))), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion  Popup Close

    #region Popup Confimation

    protected void btnConfirmYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divPopODAlert.Visible = false;
            lblODAlertMsg.Text = string.Empty;
            divConfirm.Visible = false;
            bool isSaveSucc = false;
            if (lblSaveorDelete.Text.Trim() == "1")
            {
                save();
            }
            else if (lblSaveorDelete.Text.Trim() == "2") //delete
            {
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnConfirmNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirm.Visible = false;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnConfirmShowYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divPopODAlert.Visible = false;
            lblODAlertMsg.Text = string.Empty;
            divConfirmShow.Visible = false;
            bool isSaveSucc = false;
            if (lblSaveorDeleteShow.Text.Trim() == "2")
            {
                deleteODDetails();
            }
            else if (lblSaveorDeleteShow.Text.Trim() == "1")
            {
                updateODDetails();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnConfirmShowNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmShow.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion  Popup Confimation

    #region Update OnDuty Details

    private void updateODDetails()
    {
        try
        {
            double OdCount1 = 0;
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divPopODAlert.Visible = false;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            string fromDate1 = string.Empty;
            string toDate1 = string.Empty;

            for (int res = 1; res < gview.Rows.Count; res++)
            {
                CheckBox chk = (CheckBox)gview.Rows[res].FindControl("chck");
                if (chk.Checked)
                {
                    //loadreason();
                    BindReason();
                    int activerow = res;

                    Label AppNo = (Label)gview.Rows[activerow].FindControl("lblsnotag");
                    string retAppNo = AppNo.Text;
                    string retroll = gview.Rows[activerow].Cells[5].Text;
                    string purpose = gview.Rows[activerow].Cells[14].Text;
                    string attedancetype1 = gview.Rows[activerow].Cells[20].Text;
                    string attedancetype = gview.Rows[activerow].Cells[20].Text;
                    string fdate = gview.Rows[activerow].Cells[15].Text; string[] splf = fdate.Split('/'); string ffinal = splf[2] + "-" + splf[1] + "-" + splf[0];
                    string tdate = gview.Rows[activerow].Cells[16].Text; string[] splt = tdate.Split('/'); string tfinal = splt[2] + "-" + splt[1] + "-" + splt[0];
                    string btfdate = splf[2] + "/" + splf[1] + "/" + splf[0]; string bttdate = splt[2] + "/" + splt[1] + "/" + splt[0];

                    string qryDate = " and (convert(datetime,od.fromdate,105) >= '" + btfdate + "' or  convert(datetime,od.Todate,105)>='" + btfdate + "') and  (convert(datetime,od.fromdate,105) <='" + bttdate + "' or convert(datetime,od.Todate,105)<= '" + bttdate + "')";

                    //string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id,convert(varchar(10),adm_date,103) as adm_date from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code AND r.app_no='" + retAppNo + "'";
                    //string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id,convert(varchar(10),adm_date,103) as adm_date,od.hourse,od.Fromdate,od.Todate,convert(nvarchar(15),convert(date,od.day),101) as date from registration r,course c,Degree dg,Department dt,Onduty_Stud od where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code and od.Roll_no=r.Roll_No AND r.app_no='" + retAppNo + "' and Fromdate >='" + ffinal + "' and Todate <='" + tfinal + "' ORDER BY date";

                    string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id,convert(varchar(10),adm_date,103) as adm_date,od.hourse,od.Fromdate,od.Todate,convert(nvarchar(15),convert(date,od.day),101) as date from registration r,course c,Degree dg,Department dt,Onduty_Stud od where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code and od.Roll_no=r.Roll_No AND r.app_no='" + retAppNo + "' " + qryDate + " ORDER BY date";

                    DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
                    int sno = 0;
                    if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
                    {
                        divODEntryDetails.Visible = true;
                        sno++;
                        string studname = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["stud_name"]).Trim();
                        string app_No = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["app_No"]).Trim();
                        string rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["roll_no"]).Trim();
                        string regno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Reg_no"]).Trim();
                        string sem = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["current_semester"]).Trim();
                        string degreecode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]).Trim();
                        string batchval = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["batch_year"]).Trim();
                        string section = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["sections"]).Trim();
                        string collegeCode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["college_code"]).Trim();
                        string courseId = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Course_Id"]).Trim();
                        string admissionno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Roll_Admit"]).Trim();
                        string AdmitDate = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["adm_date"]);
                        attendenace(degreecode, sem);
                        ddlCollegeOD.Enabled = false;
                        ddlBatchOD.Enabled = false;
                        ddlBranchOD.Enabled = false;
                        ddlDegreeOD.Enabled = false;
                        ddlSemOD.Enabled = false;
                        ddlSecOD.Enabled = false;
                        if (ddlCollegeOD.Items.Count > 0)
                        {
                            ddlCollegeOD.SelectedValue = collegeCode;
                            ddlCollegeOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlBatchOD.Items.Count > 0)
                        {
                            ddlBatchOD.SelectedValue = batchval;
                            ddlBatchOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlDegreeOD.Items.Count > 0)
                        {
                            ddlDegreeOD.SelectedValue = courseId;
                            ddlDegreeOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlBranchOD.Items.Count > 0)
                        {
                            ddlBranchOD.SelectedValue = degreecode;
                            ddlBranchOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlSemOD.Items.Count > 0)
                        {
                            ddlSemOD.SelectedValue = sem;
                            ddlSemOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlSecOD.Items.Count > 0)
                        {
                            ddlSecOD.Enabled = false;
                            ddlSecOD.SelectedIndex = 0;
                            if (section.Trim().ToLower() != "")
                            {
                                ddlSecOD.SelectedValue = section;
                            }
                        }
                        divPopODAlert.Visible = false;
                        Init_Spread(1);
                        double OdCount = 0;
                        string GetODCount = da.GetFunction("select linkValue from inssettings where linkName='NoOfOdPerStudents' and College_code ='" + collegeCode + "'");
                        if (GetODCount.Trim() != "" && GetODCount.Trim() != "0")
                        {
                            string[] SplitCount = GetODCount.Split(';');
                            if (SplitCount.Length > 1)
                            {
                                ViewState["ODCheck"] = Convert.ToString(SplitCount[0]);
                                ViewState["ODCont"] = Convert.ToString(SplitCount[1]);
                            }
                        }
                        else
                        {
                            ViewState["ODCheck"] = "0";
                            ViewState["ODCont"] = "0";
                        }
                        if (Convert.ToString(ViewState["ODCheck"]).Trim() == "1")
                        {
                            AttendancePercentage(collegeCode, batchval, degreecode, sem, rollno, AdmitDate, ref OdCount);
                        }

                        drdetail = dtdetail.NewRow();
                        drdetail["sno"] = Convert.ToString(sno);
                        drdetail["snotag"] = batchval;
                        drdetail["snonote"] = collegeCode;
                        drdetail["Roll No"] = rollno;
                        drdetail["Rolltag"] = degreecode;
                        drdetail["Rollvalue"] = rollno;
                        drdetail["Reg No"] = regno;
                        drdetail["Regtag"] = section;
                        drdetail["Regnote"] = courseId;
                        drdetail["Admission No"] = admissionno;
                        drdetail["Student Name"] = studname;
                        drdetail["Semester"] = sem;
                        drdetail["Semestertag"] = app_No;
                        drdetail["OD Count"] = Convert.ToString(OdCount);
                        OdCount1 = Convert.ToDouble(OdCount);
                        dtdetail.Rows.Add(drdetail);

                        double TotalCount = 0;
                        double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount);
                        if (TotalCount != 0 && TotalCount <= OdCount)
                        {
                            //FpStudentDetails.Rows[FpStudentDetails.Sheets[0].RowCount - 1].BackColor = Color.Tan;  //modified by prabha 
                            //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Value = 0;
                        }
                        else
                        {
                            //FpStudentDetails.Rows[FpStudentDetails.Sheets[0].RowCount - 1].BackColor = Color.Tan;  //modified by prabha 
                            //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Value = 1;
                        }

                        txtNoOfHours.Text = (gview.Rows[activerow].FindControl("lblouttmetag") as Label).Text;
                        txtFromDateOD.Text = gview.Rows[activerow].Cells[15].Text;
                        txtToDateOD.Text = gview.Rows[activerow].Cells[16].Text;

                        #region Gview_OD
                        DataTable dtOdHour = new DataTable();
                        DataRow drOdHour;

                        fromDate1 = gview.Rows[activerow].Cells[15].Text;
                        toDate1 = gview.Rows[activerow].Cells[16].Text;

                        dtOdHour.Columns.Add("OdDate");
                        drOdHour = dtOdHour.NewRow();
                        dtOdHour.Rows.Add(drOdHour);

                        string[] dbsplfm = fromDate1.Split('/');
                        string[] dbspltoo = toDate1.Split('/');
                        string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
                        DateTime dbconvfrm = Convert.ToDateTime(convfrm);
                        string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
                        DateTime dbconvto = Convert.ToDateTime(convto);

                        while (dbconvfrm <= dbconvto)
                        {
                            drOdHour = dtOdHour.NewRow();
                            string[] spl = Convert.ToString(dbconvfrm).Split(' ');
                            string[] dtespl = Convert.ToString(spl[0]).Split('/');
                            if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
                            if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
                            string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
                            drOdHour["OdDate"] = dte;
                            dtOdHour.Rows.Add(drOdHour);
                            dbconvfrm = dbconvfrm.AddDays(1);
                        }
                        dbconvfrm = Convert.ToDateTime(convfrm);
                        dbconvto = Convert.ToDateTime(convto);
                        string tothrs = string.Empty;
                        string fsthalfhrs = string.Empty;
                        string scndhalfhrs = string.Empty;
                        Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
                        if (htHours.Count == 0)
                        {
                            attendenace("", "", 1);
                        }
                        foreach (DictionaryEntry parameter1 in htHours)
                        {
                            string daytext = Convert.ToString(parameter1.Key);
                            string noofhours = Convert.ToString(parameter1.Value);
                            if (daytext == "full")
                            {
                                tothrs = noofhours;
                            }
                            if (daytext == "fn")
                            {
                                fsthalfhrs = noofhours;
                            }
                            if (daytext == "an")
                            {
                                scndhalfhrs = noofhours;
                            }
                        }
                        DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
                        if (tothrs != "" && tothrs != null)
                        {
                            string temp = string.Empty;
                            for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                            {
                                dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
                            }
                        }

                        gviewOD.DataSource = dtOdHour;
                        gviewOD.DataBind();
                        gviewOD.Visible = true;

                        if (gviewOD.Rows.Count > 0)
                        {
                            for (int i = 0; i < dthour.Rows.Count; i++)
                            {
                                if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
                                {
                                    gviewOD.Columns[i + 1].Visible = true;
                                }
                                else
                                {
                                    gviewOD.Columns[i + 1].Visible = false;
                                }
                            }
                        }

                        DataView dview = new DataView();
                        //while (dbconvfrm <= dbconvto)
                        //{
                        for (int view = 1; view < gviewOD.Rows.Count; view++)
                        {
                            string gdate = (gviewOD.Rows[view].FindControl("lblODdate") as Label).Text;
                            //string[] spldate = Convert.ToString(dbconvfrm).Split(' ');
                            //string[] dbdate = Convert.ToString(spldate[0]).Split('/');
                            //if (gdate == dbdate[1] + "/" + dbdate[0] + "/" + dbdate[2])
                            //{
                            for (int hrs = 0; hrs < dsstudinfo.Tables[0].Rows.Count; hrs++)
                            {
                                string hours = Convert.ToString(dsstudinfo.Tables[0].Rows[hrs]["hourse"]).Trim();
                                string ddate = Convert.ToString(dsstudinfo.Tables[0].Rows[hrs]["Date"]).Trim();
                                string[] dde = hours.Split(',');
                                for (int head = 0; head < dde.Length; head++)
                                {
                                    if (!string.IsNullOrEmpty(dde[head]))
                                    {
                                        int s = Convert.ToInt16(dde[head]);
                                        if (!string.IsNullOrEmpty(ddate))
                                        {
                                            string[] spddate = ddate.Split('/');
                                            string datte = spddate[1] + "/" + spddate[0] + "/" + spddate[2];
                                            if (datte == gdate)
                                            {
                                                if (gviewOD.Columns[s].Visible == true)
                                                {
                                                    CheckBox chck = (gviewOD.Rows[view].FindControl("chkhour" + s.ToString()) as CheckBox);
                                                    chck.Checked = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string dbdate = Convert.ToString(dsstudinfo.Tables[0].Rows[hrs]["hourse"]).Trim();
                                            string[] spldbdate = dbdate.Split(',');
                                            for (int ji = 0; ji < spldbdate.Length; ji++)
                                            {
                                                int s1 = Convert.ToInt16(spldbdate[ji]);
                                                if (gviewOD.Columns[s1].Visible == true)
                                                {
                                                    CheckBox chck = (gviewOD.Rows[view].FindControl("chkhour" + s1.ToString()) as CheckBox);
                                                    chck.Checked = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }
                        //dbconvfrm = dbconvfrm.AddDays(1);
                        //}
                        #endregion

                        btnPopSaveOD.Text = "Update";//day

                        string getouttime = gview.Rows[activerow].Cells[17].Text;
                        string getintime = gview.Rows[activerow].Cells[19].Text;

                        string[] splitouttime = getouttime.Split(new char[] { ' ' });
                        string[] splitintime = getintime.Split(new char[] { ' ' });
                        string splitedouttime = splitouttime[0].ToString();
                        string splitedoutmeridian = splitouttime[1].ToString();
                        string splitedintime = splitintime[0].ToString();
                        string splitedinmeridian = splitintime[1].ToString();
                        string[] hourList = txtNoOfHours.Text.Split(',');
                        if (hourList.Length > 0)
                        {
                            cblHours.Items.Clear();
                            int item = 0;
                            foreach (string hrslst in hourList)
                            {
                                cblHours.Items.Add(new ListItem(hrslst, hrslst));
                                cblHours.Items[item].Selected = true;
                                item++;
                            }
                        }
                        string[] outtime = splitedouttime.Split(new char[] { ':' });
                        string hour = outtime[0];
                        string min = outtime[1];
                        if (outtime[0].Length == 1)
                        {
                            hour = "0" + outtime[0];
                        }
                        if (min.Length == 1)
                        {
                            min = "0" + outtime[1];
                        }
                        string[] intime = splitedintime.Split(new char[] { ':' });
                        string outhr = intime[0].ToString();
                        string outmm = intime[1].ToString();
                        if (outhr.Length == 1)
                        {
                            outhr = "0" + outhr;
                        }
                        if (outmm.Length == 1)
                        {
                            outmm = "0" + outmm;
                        }
                        ddlOutTimeHr.Enabled = false;
                        ddlOutTimeMM.Enabled = false;
                        ddlOutTimeSess.Enabled = false;
                        ddlOutTimeHr.Text = hour;
                        ddlOutTimeMM.Text = min;
                        ddlOutTimeSess.Text = splitedoutmeridian;
                        ddlInTimeHr.Enabled = false;
                        ddlInTimeMM.Enabled = false;
                        ddlInTimeSess.Enabled = false;
                        ddlInTimeHr.Text = outhr;
                        ddlInTimeMM.Text = outmm;
                        ddlInTimeSess.Text = splitedinmeridian;
                        purpose = da.GetFunction("select textcode from TextValTable where TextCriteria='Attrs' and textval='" + purpose + "' and college_code='" + collegeCode + "'");
                        if (purpose.Trim() != "" && purpose.Trim() != "0")
                        {
                            if (ddlPurpose.Items.Count > 0)
                            {
                                ddlPurpose.SelectedValue = purpose;
                            }
                        }
                        BindAttendanceRights();
                        ddlAttendanceOption.Enabled = false;
                        if (ddlAttendanceOption.Items.Count > 0)
                        {
                            ListItem list = new ListItem(attedancetype.Trim().ToUpper(), attedancetype.Trim().ToUpper());
                            if (ddlAttendanceOption.Items.Contains(list))
                            {
                                ddlAttendanceOption.Text = attedancetype;
                            }
                        }
                        btnPopSaveOD.Enabled = true;
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record Found";
                        divPopAlert.Visible = true;
                        return;
                    }

                    gviewstudetails.DataSource = dtdetail;
                    gviewstudetails.DataBind();
                    gviewstudetails.Visible = true;

                    gviewstudetails.Columns[1].Visible = isRollVisible;
                    gviewstudetails.Columns[2].Visible = isRegVisible;
                    gviewstudetails.Columns[3].Visible = isAdmitNoVisible;

                    double TotalCount1 = 0;
                    double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount1);
                    if (TotalCount1 != 0 && TotalCount1 <= OdCount1)
                    {
                        CheckBox chck = (gviewstudetails.Rows[1].FindControl("check") as CheckBox);
                        chck.Checked = false;
                        gviewstudetails.Rows[1].BackColor = Color.Tan;
                    }
                    else
                    {
                        CheckBox chck = (gviewstudetails.Rows[1].FindControl("check") as CheckBox);
                        chck.Checked = true;
                    }

                    if (Convert.ToString(ViewState["ODCheck"]).Trim() != "0")
                    {
                        gviewstudetails.Columns[6].Visible = true;
                    }
                    else
                    {
                        gviewstudetails.Columns[6].Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnOnDutyUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divPopODAlert.Visible = false;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;

            bool isSelected = false;
            int count = 0;
            for (int res = 1; res < gview.Rows.Count; res++)
            {
                CheckBox chk = (CheckBox)gview.Rows[res].FindControl("chck");

                if (chk.Checked)
                {
                    count++;
                    isSelected = true;
                }
            }
            if (count > 1)
            {
                lblAlertMsg.Text = "Please Select Only One Record To Update.";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (isSelected)
            {
                lblSaveorDeleteShow.Text = "1";
                lblConfirmMsgShow.Text = "Do You Want To Update OD Details?";
                divConfirmShow.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any One Record To Update";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Update OnDuty Details

    #region Delete OnDuty Details

    protected void btnOnDutyDelete_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmShow.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            bool isSelected = false;

            for (int res = 1; res < gview.Rows.Count; res++)
            {
                CheckBox chk = (CheckBox)gview.Rows[res].FindControl("chck");
                if (chk.Checked)
                {
                    isSelected = true;
                }
            }
            if (isSelected)
            {
                lblSaveorDeleteShow.Text = "2";
                lblConfirmMsgShow.Text = "Do You Want To Delete OD Details?";
                divConfirmShow.Visible = true;
                return;
            }
            else
            {
                //lblAlertMsg.Text = "Deleted Successfully";
                lblAlertMsg.Text = "Please Select Atleast One Record To Delete";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void deleteODDetails()
    {
        try
        {
            int savevalue = 0;
            bool isDeleteSucc = false;
            string dt = string.Empty;

            for (int res = 1; res < gview.Rows.Count; res++)
            {
                CheckBox chk = (CheckBox)gview.Rows[res].FindControl("chck");
                if (chk.Checked)
                {
                    string ar = Convert.ToString(res).Trim();

                    Label app = (Label)gview.Rows[res].FindControl("lblsnotag");
                    string appNo = app.Text;
                    Label degcode = (Label)gview.Rows[res].FindControl("lblsnonote");
                    string degreecode = degcode.Text;
                    //Label rollno = (Label)gview.Rows[res].FindControl("lblroll");
                    string stdRollno = gview.Rows[res].Cells[5].Text;
                    Label hour = (Label)gview.Rows[res].FindControl("lblouttmetag");
                    string hours = hour.Text;//day
                    //Label date = (Label)gview.Rows[res].FindControl("lblfrmdate");
                    string getdate = gview.Rows[res].Cells[15].Text;
                    Label colcode = (Label)gview.Rows[res].FindControl("lblrolltag");
                    string collegeCode = colcode.Text;
                    Label semnew = (Label)gview.Rows[res].FindControl("lblrollnote");
                    string semesterNew = semnew.Text;
                    string enddate = gview.Rows[res].Cells[16].Text;

                    string[] spdate = getdate.Split('/'); string[] spedate = enddate.Split('/');
                    string frdate = spdate[1] + '/' + spdate[0] + '/' + spdate[2]; string toodate = spedate[1] + '/' + spedate[0] + '/' + spedate[2];

                    string strNew = "select * from Onduty_Stud where Roll_no='" + stdRollno + "' and semester='" + semesterNew + "'   and day between '" + frdate + "' and '" + toodate + "' order by day";
                    DataSet dsNew = da.select_method_wo_parameter(strNew, "Text");
                    DataView dod = new DataView();
                    if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    {
                        //string deleteentry = "delete from onduty_stud where roll_no='" + stdRollno + "' and semester='" + semesterNew + "' and fromdate='" + frdate + "' and todate='" + toodate + "' and day is not null";
                        string deleteentry = "delete from onduty_stud where roll_no='" + stdRollno + "' and semester='" + semesterNew + "' and day between '" + frdate + "' and '" + toodate + "' and day is not null";
                        savevalue = da.update_method_wo_parameter(deleteentry, "Text");
                        if (savevalue != 0)
                        {
                            isDeleteSucc = true;
                        }
                        fromDate = getdate;
                        toDate = gview.Rows[res].Cells[16].Text;
                        dt = fromDate;
                        string[] dsplit = dt.Split(new Char[] { '/' });
                        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        int demfcal = int.Parse(dsplit[2].ToString());
                        demfcal = demfcal * 12;
                        int cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                        string monthcal = cal_from_date.ToString();
                        dt = toDate;
                        dsplit = dt.Split(new Char[] { '/' });
                        toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        int demtcal = int.Parse(dsplit[2].ToString());
                        demtcal = demfcal * 12;
                        int cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                        DateTime per_from_date = Convert.ToDateTime(frdate);
                        DateTime per_to_date = Convert.ToDateTime(toDate);
                        DateTime dumm_from_date = per_from_date;
                        string reason = string.Empty;
                        ht.Clear();
                        ht.Add("degree_code", int.Parse(degreecode));
                        ht.Add("sem", int.Parse(semesterNew));
                        ht.Add("from_date", frdate.ToString());
                        ht.Add("to_date", toDate.ToString());
                        ht.Add("coll_code", int.Parse(collegeCode));
                        int iscount = 0;
                        string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + toDate.ToString() + "' and degree_code='" + degreecode + "' and semester='" + semesterNew + "' ";
                        DataSet ds_holi = new DataSet();
                        ds_holi.Reset();
                        ds_holi.Dispose();
                        ds_holi = da.select_method(strquery, ht, "Text");
                        if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count > 0)
                        {
                            iscount = Convert.ToInt16(ds_holi.Tables[0].Rows[0]["cnt"].ToString());
                        }
                        ht.Add("iscount", iscount);
                        ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");
                        Hashtable holiday_table = new Hashtable();
                        holiday_table.Clear();
                        if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                            {
                                if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                                {
                                    holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                                }
                            }
                        }
                        if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                            {
                                if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                                {
                                    holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                                }
                            }
                        }
                        if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                            {
                                if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                                {
                                    holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                                }
                            }
                        }
                        Dictionary<string, StringBuilder[]> dicQueryValue = new Dictionary<string, StringBuilder[]>();
                        if (dumm_from_date <= per_to_date)
                        {
                            while (dumm_from_date <= per_to_date)
                            {
                                dsNew.Tables[0].DefaultView.RowFilter = "day='" + dumm_from_date + "'";
                                dod = dsNew.Tables[0].DefaultView;
                                if (dod.Count > 0)
                                {
                                    hours = Convert.ToString(dod[0]["hourse"]);
                                    StringBuilder sbQueryUpdate = new StringBuilder();
                                    StringBuilder sbQUeryInsertValue = new StringBuilder();
                                    StringBuilder sbQueryColumnName = new StringBuilder();
                                    int monthyear = 0;
                                    if (!holiday_table.ContainsKey(dumm_from_date))
                                    {
                                        string dummfromdate = Convert.ToString(dumm_from_date);
                                        string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                        string fromdate2 = fromdate1[0].ToString();
                                        string[] fromdate = fromdate2.Split(new char[] { '/' });
                                        string fromdatedate = fromdate[1].ToString();
                                        string fromdatemonth = fromdate[0].ToString();
                                        string fromdateyear = fromdate[2].ToString();
                                        monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                        string valueupdate = string.Empty;
                                        string insertvalue = string.Empty;
                                        string odvalue = string.Empty;
                                        int totnoofhours = 0;
                                        string[] hourslimit = hours.Split(new char[] { ',' });
                                        totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                        int taken_hourse = 0;
                                        taken_hourse = taken_hourse + totnoofhours;
                                        for (int i = 0; i < Convert.ToInt32(totnoofhours); i++)
                                        {
                                            string particularhrs = hourslimit[i].ToString();
                                            string value = ("d" + fromdatedate + "d" + particularhrs);
                                            string reval = value;
                                            string attValue = "null";
                                            value = value + "=''";
                                            if (valueupdate == "")
                                            {
                                                valueupdate = reval + "=" + attValue;
                                            }
                                            else
                                            {
                                                valueupdate = valueupdate + "," + reval + "=" + attValue;
                                            }
                                            if (insertvalue == "")
                                            {
                                                insertvalue = reval;
                                            }
                                            else
                                            {
                                                insertvalue = insertvalue + "," + reval;
                                            }
                                            if (odvalue == "")
                                            {
                                                odvalue = attValue;
                                            }
                                            else
                                            {
                                                odvalue = odvalue + "," + attValue;
                                            }
                                            //string updateattend = "update Attendance set " + value + " where  Roll_no='" + stdRollno + "' and month_year=" + monthyear + "";
                                            //int save = da.update_method_wo_parameter(updateattend, "Text");
                                            //ht.Clear();
                                            //ht.Add("AtWr_App_no", appNo);
                                            //ht.Add("AttWr_CollegeCode", collegeCode);
                                            //ht.Add("columnname", reval);
                                            //ht.Add("roll_no", stdRollno);
                                            //ht.Add("month_year", monthyear);
                                            //ht.Add("values", reason);
                                            //strquery = "sp_ins_upd_student_attendance_reason";
                                            //int insert = da.insert_method(strquery, ht, "sp");
                                            //if (insert != 0)
                                            //{
                                            //    isDeleteSucc = true;
                                            //}
                                        }
                                        if (!string.IsNullOrEmpty(insertvalue))
                                        {
                                            sbQueryColumnName.Append(insertvalue + ",");
                                        }
                                        if (!string.IsNullOrEmpty(odvalue))
                                        {
                                            sbQUeryInsertValue.Append(odvalue + ",");
                                        }
                                        if (!string.IsNullOrEmpty(valueupdate))
                                        {
                                            sbQueryUpdate.Append(valueupdate + ",");
                                        }
                                    }
                                    StringBuilder[] sbAll = new StringBuilder[3];
                                    if (!string.IsNullOrEmpty(sbQueryColumnName.ToString().Trim()) && !string.IsNullOrEmpty(sbQUeryInsertValue.ToString().Trim()) && !string.IsNullOrEmpty(sbQueryUpdate.ToString().Trim()))
                                    {
                                        if (dicQueryValue.ContainsKey(monthyear.ToString().Trim()))
                                        {
                                            sbAll = dicQueryValue[monthyear.ToString().Trim()];
                                            sbAll[0].Append(sbQueryColumnName);
                                            sbAll[1].Append(sbQUeryInsertValue);
                                            sbAll[2].Append(sbQueryUpdate);
                                            dicQueryValue[monthyear.ToString().Trim()] = sbAll;
                                        }
                                        else if (monthyear != 0)
                                        {
                                            sbAll[0] = new StringBuilder();
                                            sbAll[1] = new StringBuilder();
                                            sbAll[2] = new StringBuilder();
                                            sbAll[0].Append(Convert.ToString(sbQueryColumnName));
                                            sbAll[1].Append(Convert.ToString(sbQUeryInsertValue));
                                            sbAll[2].Append(Convert.ToString(sbQueryUpdate));
                                            dicQueryValue.Add(monthyear.ToString().Trim(), sbAll);
                                        }
                                    }
                                }
                                dumm_from_date = dumm_from_date.AddDays(1);
                            }
                            if (dicQueryValue.Count > 0)
                            {
                                StringBuilder[] spAll = new StringBuilder[3];
                                foreach (KeyValuePair<string, StringBuilder[]> dicQueery in dicQueryValue)
                                {
                                    spAll = new StringBuilder[3];
                                    string monthValue = dicQueery.Key;
                                    spAll = dicQueery.Value;
                                    string insertColumnName = spAll[0].ToString().Trim(',');
                                    string insertColumnValue = spAll[1].ToString().Trim(',');
                                    string updateColumnNameValue = spAll[2].ToString().Trim(',');
                                    string[] splitColumn = insertColumnName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string sp in splitColumn)
                                    {
                                        ht.Clear();
                                        ht.Add("AtWr_App_no", appNo);
                                        ht.Add("AttWr_CollegeCode", collegeCode);
                                        ht.Add("columnname", sp);
                                        ht.Add("roll_no", stdRollno);
                                        ht.Add("month_year", monthValue);
                                        ht.Add("values", reason);
                                        strquery = "sp_ins_upd_student_attendance_reason";
                                        int insert = da.insert_method(strquery, ht, "sp");
                                        if (savevalue != 0)
                                        {
                                            isDeleteSucc = true;
                                        }
                                    }
                                    ht.Clear();
                                    ht.Add("Att_App_no", appNo);
                                    ht.Add("Att_CollegeCode", collegeCode);
                                    ht.Add("rollno", stdRollno);
                                    ht.Add("monthyear", monthValue);
                                    ht.Add("columnname", insertColumnName);
                                    ht.Add("colvalues", insertColumnValue);
                                    ht.Add("coulmnvalue", updateColumnNameValue);
                                    savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                    if (savevalue != 0)
                                    {
                                        isDeleteSucc = true;
                                    }
                                }
                            }
                        }
                    }
                    string strOld = "select * from Onduty_Stud where Roll_no='" + stdRollno + "' and semester='" + semesterNew + "' and Fromdate='" + frdate + "'  and todate='" + toodate + "'";
                    DataSet dsOld = da.select_method_wo_parameter(strOld, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string deleteentry = "delete from onduty_stud where roll_no='" + stdRollno + "' and semester='" + semesterNew + "' and fromdate='" + frdate + "' and todate='" + toodate + "'";
                        //savevalue = da.update_method_wo_parameter(deleteentry, "Text");
                    }
                }
            }
            btnOnDutyDelete.Visible = false;
            btnOnDutyUpdate.Visible = false;
            btnGo_Click(new Object(), new EventArgs());
            if (isDeleteSucc)
            {
                lblAlertMsg.Text = "Deleted Successfully";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Deleted";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        catch
        {

        }
    }

    #endregion Delete OnDuty Details

    #endregion Button Events

    #region OnDuty Details

    #region Bind Header

    public void BindCollegeOD()
    {
        try
        {
            ddlCollegeOD.Items.Clear();
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlCollegeOD.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollegeOD.DataSource = dsprint;
                ddlCollegeOD.DataTextField = "collname";
                ddlCollegeOD.DataValueField = "college_code";
                ddlCollegeOD.DataBind();
                ddlCollegeOD.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBatchOD()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            collegeCode = string.Empty;
            userCode = string.Empty;
            ddlBatchOD.Items.Clear();
            ds.Clear();
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollegeOD.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                //}
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year<>'' and batch_year<>'0' and batch_year<>'-1'  " + qryUserOrGroupCode + " order by batch_year desc";
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatchOD.DataSource = ds;
                ddlBatchOD.DataTextField = "Batch_year";
                ddlBatchOD.DataValueField = "Batch_year";
                ddlBatchOD.DataBind();
                ddlBatchOD.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegreeOD()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegreeOD.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (ddlCollegeOD.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlBatchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qryCollegeCode + qryUserOrGroupCode + "  order by c.Priority", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDegreeOD.DataSource = ds;
                    ddlDegreeOD.DataTextField = "course_name";
                    ddlDegreeOD.DataValueField = "course_id";
                    ddlDegreeOD.DataBind();
                    ddlDegreeOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranchOD()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlBranchOD.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            if (ddlCollegeOD.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlBatchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegreeOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegreeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qryCourseId + qryCollegeCode + qryUserOrGroupCode + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranchOD.DataSource = ds;
                    ddlBranchOD.DataTextField = "dept_name";
                    ddlBranchOD.DataValueField = "degree_code";
                    ddlBranchOD.DataBind();
                    ddlBranchOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSemOD()
    {
        try
        {
            ds.Clear();
            ddlSemOD.Items.Clear();
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCode = string.Empty;
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            if (ddlCollegeOD.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollegeCode + qryBatchYear + " group by first_year_nonsemester";
                sqlnew += " select distinct Current_Semester  from registration where degree_code in (" + degreeCode + ") and batch_year in (" + batchYear + ") and college_code in (" + collegeCode + ") and cc=0 and delflag=0 and exam_flag<>'DEBAR' order by Current_semester desc ";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                if (ds.Tables[1].Rows.Count > 0) // Added by jairam 07-08-2017
                {
                    string CurrentSemvalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    ddlSemOD.SelectedIndex = ddlSemOD.Items.IndexOf(ddlSemOD.Items.FindByValue(CurrentSemvalue));
                }
                else
                {
                    ddlSemOD.SelectedIndex = 0;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree where duration<>'0' " + qryDegreeCode + qryCollegeCode + " group by first_year_nonsemester";
                    sqlnew += " select distinct Current_Semester  from registration where degree_code in (" + degreeCode + ") and batch_year in (" + batchYear + ") and college_code in (" + collegeCode + ") and cc=0 and delflag=0 and exam_flag<>'DEBAR' order by Current_semester desc ";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0) // Added by jairam 07-08-2017
                    {
                        string CurrentSemvalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                        ddlSemOD.SelectedIndex = ddlSemOD.Items.IndexOf(ddlSemOD.Items.FindByValue(CurrentSemvalue));
                    }
                    else
                    {
                        ddlSemOD.SelectedIndex = 0;
                    }
                    //ddlSemOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSectionDetailOD()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            ds = new DataSet();
            ddlSecOD.Items.Clear();
            ddlSecOD.Enabled = false;
            if (ddlCollegeOD.Items.Count > 0)
            {                //collegeCode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                //}
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                //batchYear = Convert.ToString(ddlBatchOD.SelectedValue).Trim();
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                //degreeCode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                //}
            }
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            string qrysections = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qrysections = da.GetFunctionv("select distinct sections from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year in(" + batchYear + ")  " + qryUserOrGroupCode).Trim();
            }
            if (!string.IsNullOrEmpty(qrysections.Trim()))
            {
                string[] sectionsAll = qrysections.Trim().Split(new char[] { ',' });
                string sections = string.Empty;
                bool hasEmpty = false;
                if (sectionsAll.Length > 0)
                {
                    for (int sec = 0; sec < sectionsAll.Length; sec++)
                    {
                        if (!string.IsNullOrEmpty(sectionsAll[sec].Trim()))
                        {
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                        else if (!hasEmpty)
                        {
                            hasEmpty = true;
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sections.Trim()))
                {
                    string sqlnew = "select distinct sections from registration where batch_year in(" + Convert.ToString(batchYear).Trim() + ") and degree_code in (" + Convert.ToString(degreeCode).Trim() + ") and sections<>'-1' and sections<>' ' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") and sections in(" + sections + ") and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSecOD.DataSource = ds;
                ddlSecOD.DataTextField = "sections";
                ddlSecOD.DataValueField = "sections";
                ddlSecOD.DataBind();
                //ddlSec.Items.Insert(0, "All");
                ddlSecOD.Enabled = true;
            }
            else
            {
                ddlSecOD.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindHour()
    {
        try
        {
            ddlInTimeHr.Items.Clear();
            ddlOutTimeHr.Items.Clear();
            for (int hr = 0; hr <= 12; hr++)
            {
                ddlInTimeHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).Trim().PadLeft(2, '0'), Convert.ToString(hr).Trim().PadLeft(2, '0')));
                ddlOutTimeHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).Trim().PadLeft(2, '0'), Convert.ToString(hr).Trim().PadLeft(2, '0')));
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindMinute()
    {
        try
        {
            ddlInTimeMM.Items.Clear();
            ddlOutTimeMM.Items.Clear();
            int s = 0;
            for (int mm = 0; mm <= 60; mm++)
            {
                ddlInTimeMM.Items.Insert(s, new ListItem(Convert.ToString(mm).Trim().PadLeft(2, '0'), Convert.ToString(mm).Trim().PadLeft(2, '0')));
                ddlOutTimeMM.Items.Insert(s, new ListItem(Convert.ToString(mm).Trim().PadLeft(2, '0'), Convert.ToString(mm).Trim().PadLeft(2, '0')));
                s++;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindReason()
    {
        try
        {
            ddlPurpose.Items.Clear();
            collegeCode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code='" + collegeCode + "' order by Textval";
                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlPurpose.DataSource = ds;
                ddlPurpose.DataTextField = "Textval";
                ddlPurpose.DataValueField = "TextCode";
                ddlPurpose.DataBind();
                btnPopSaveOD.Enabled = true;
            }
            else
            {
                btnPopSaveOD.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindAttendanceRights()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            ddlAttendanceOption.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qry = "select distinct rights from OD_Master_Setting where " + qryUserOrGroupCode + "";
                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<string, int> dicAttRights = new Dictionary<string, int>();
                int itemCount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAttendanceOption.Enabled = true;
                    string odRights = string.Empty;
                    string temp1 = string.Empty;
                    Hashtable htOD = new Hashtable();
                    odRights = Convert.ToString(ds.Tables[0].Rows[i]["rights"]).Trim();
                    if (odRights != string.Empty)
                    {
                        string[] splitODRights = odRights.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int odTemp = 0; odTemp < splitODRights.Length; odTemp++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(splitODRights[odTemp]).Trim()))
                            {
                                if (!dicAttRights.ContainsKey(Convert.ToString(splitODRights[odTemp]).Trim().ToLower()))
                                {
                                    ddlAttendanceOption.Items.Add(Convert.ToString(splitODRights[odTemp]).Trim());
                                    dicAttRights.Add(Convert.ToString(splitODRights[odTemp]).Trim().ToLower(), 1);
                                    itemCount++;
                                }
                            }
                        }
                    }
                }
                if (ddlAttendanceOption.Items.Count == 0)
                {
                    ddlAttendanceOption.Enabled = false;
                }
                else
                {
                    ddlAttendanceOption.Enabled = true;
                }
            }
            else
            {
                ddlAttendanceOption.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void ddlCollegeOD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;

            if (!chkStudentWise.Checked)//added by Deepali on 6.4.18
            {
                BindBatchOD();
                BindDegreeOD();
                BindBranchOD();
                BindSemOD();
                BindSectionDetailOD();
                BindReason();
                ShowStudentsList(0);
            }
            else
            {
                ShowStudentsList(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBatchOD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            BindDegreeOD();
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlDegreeOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranchOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSemOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSecOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ////divMainContents.Visible = false;
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void rbFullDay_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            divHalfHr.Visible = false;
            rbPM.Checked = false;
            rbAM.Checked = false;
            cblHours.Items.Clear();
            // holiDateCheck.Clear();
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            if (htHours.Contains("full"))
            {
                string get_ful_hr = GetCorrespondingKey("full", htHours).ToString();
                for (int x = 1; x <= Convert.ToInt16(get_ful_hr); x++)
                {
                    cblHours.Items.Add("" + x + "");
                    cblHours.Items[x - 1].Selected = true;
                    if (txtNoOfHours.Text == "")
                    {
                        txtNoOfHours.Text = x.ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + x.ToString();
                    }
                }
                lblNoOfHours.Visible = false;
            }
            else
            {
                lblPopODErr.Visible = true;
                lblPopODErr.Text = "Please Add " + lblSemOD.Text + " Information";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void rbHourWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            cblHours.Items.Clear();
            divHalfHr.Visible = false;
            rbAM.Visible = false;
            rbPM.Visible = false;
            txtNoOfHours.Visible = true;
            lblNoOfHours.Visible = true;
            string tothrs = string.Empty;
            string fsthalfhrs = string.Empty;
            string scndhalfhrs = string.Empty;
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHours)
            {
                string daytext = Convert.ToString(parameter1.Key);
                string noofhours = Convert.ToString(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (tothrs != "" && tothrs != null)
            {
                if (rbHourWise.Checked == true)
                {
                    string temp = string.Empty;
                    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                    {
                        cblHours.Items.Add("" + fulhrs + "");
                        //Chkselecthours.Items[fulhrs-1].Selected = true;
                        //if (temp == "")
                        //{
                        //    txtselecthours.Text = (fulhrs).ToString();
                        //    temp = (fulhrs).ToString();
                        //}
                        //else
                        //{
                        //    txtselecthours.Text = txtselecthours.Text + "," + (fulhrs).ToString();
                        //}
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void rbHalfDay_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            cblHours.Items.Clear();
            // holiDateCheck.Clear();
            divHalfHr.Visible = true;
            rbAM.Visible = true;
            rbPM.Visible = true;
            lblNoOfHours.Visible = true;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            txtNoOfHours.Visible = true;
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHours)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (rbHalfDay.Checked == true)
            {
                if (rbAM.Checked == true)
                {
                    string temp = string.Empty;
                    for (int fsthrs = 1; fsthrs <= fsthalfhrs; fsthrs++)
                    {
                        cblHours.Items.Add("" + fsthrs + "");
                        cblHours.Items[fsthrs - 1].Selected = true;
                        if (temp == "")
                        {
                            txtNoOfHours.Text = (fsthrs).ToString();
                            temp = (fsthrs).ToString();
                        }
                        else
                        {
                            txtNoOfHours.Text = txtNoOfHours.Text + "," + (fsthrs).ToString();
                        }
                    }
                }
                else if (rbPM.Checked == true)
                {
                    string temp = string.Empty;
                    for (int scdhrs = fsthalfhrs + 1; scdhrs <= Convert.ToInt32(fsthalfhrs + scndhalfhrs); scdhrs++)
                    {
                        cblHours.Items.Add("" + scdhrs + "");
                        cblHours.Items[scdhrs - (fsthalfhrs + 1)].Selected = true;

                        if (temp == "")
                        {
                            txtNoOfHours.Text = (scdhrs).ToString();
                            temp = (scdhrs).ToString();
                        }
                        else
                        {
                            txtNoOfHours.Text = txtNoOfHours.Text + "," + (scdhrs).ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void rbAM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            cblHours.Items.Clear();
            txtNoOfHours.Text = string.Empty;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHours)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (rbAM.Checked == true)
            {
                string temp = string.Empty;
                for (int fsthrs = 1; fsthrs <= Convert.ToInt32(fsthalfhrs); fsthrs++)
                {
                    cblHours.Items.Add("" + fsthrs + "");
                    cblHours.Items[fsthrs - 1].Selected = true;
                    if (temp == "")
                    {
                        txtNoOfHours.Text = (fsthrs).ToString();
                        temp = (fsthrs).ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + (fsthrs).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void rbPM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            cblHours.Items.Clear();
            txtNoOfHours.Text = string.Empty;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHours)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }// panelselecthours.Visible = true;
            if (rbPM.Checked == true)
            {
                string temp = string.Empty;
                for (int scdhrs = fsthalfhrs + 1; scdhrs <= Convert.ToInt32(scndhalfhrs + fsthalfhrs); scdhrs++)
                {
                    cblHours.Items.Add("" + scdhrs + "");
                    cblHours.Items[scdhrs - (fsthalfhrs + 1)].Selected = true;
                    if (temp == "")
                    {
                        txtNoOfHours.Text = (scdhrs).ToString();
                        temp = scdhrs.ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + (scdhrs).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void txtNoOfHours_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblHours_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            string value = string.Empty;
            for (int i = 0; i < cblHours.Items.Count; i++)
            {
                if (cblHours.Items[i].Selected == true)
                {
                    value = cblHours.Items[i].Text;
                    if (txtNoOfHours.Text == "")
                    {
                        txtNoOfHours.Text = value;
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtStudent.Text = string.Empty;
            if (ddlSearchBy.Items.Count > 0)
            {
                lblStudentOptions.Text = ddlSearchBy.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void txtFromDateOD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            #region com
            //fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            //toDate = Convert.ToString(txtToDateOD.Text).Trim();
            //if (fromDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            //    isValidFromDate = isValidDate;
            //    if (!isValidDate)
            //    {
            //        lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblODAlertMsg.Visible = true;
            //        divPopODAlert.Visible = true;
            //        gviewOD.Visible = false;
            //        return;
            //    }
            //}
            //else
            //{
            //    lblODAlertMsg.Text = "Please Choose From Date";
            //    lblODAlertMsg.Visible = true;
            //    divPopODAlert.Visible = true;
            //    gviewOD.Visible = false;
            //    return;
            //}
            //if (toDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
            //    isValidToDate = isValidDate;
            //    if (!isValidDate)
            //    {
            //        lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblODAlertMsg.Visible = true;
            //        divPopODAlert.Visible = true;
            //        gviewOD.Visible = false;
            //        return;
            //    }
            //}
            //else
            //{
            //    lblODAlertMsg.Text = "Please Choose To Date";
            //    lblODAlertMsg.Visible = true;
            //    divPopODAlert.Visible = true;
            //    gviewOD.Visible = false;
            //    return;
            //}
            //string qryDate = string.Empty;
            //if (dtFromDate > dtToDate)
            //{
            //    lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
            //    lblODAlertMsg.Visible = true;
            //    divPopODAlert.Visible = true;
            //    gviewOD.Visible = false;
            //    return;
            //}

            //DataTable dtOdHour = new DataTable();
            //DataRow drOdHour;

            //dtOdHour.Columns.Add("OdDate");
            //drOdHour = dtOdHour.NewRow();
            //dtOdHour.Rows.Add(drOdHour);

            //string[] dbsplfm = fromDate.Split('/');
            //string[] dbspltoo = toDate.Split('/');
            //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
            //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
            //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
            //DateTime dbconvto = Convert.ToDateTime(convto);

            //while (dbconvfrm <= dbconvto)
            //{
            //    drOdHour = dtOdHour.NewRow();
            //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
            //    string[] dtespl = Convert.ToString(spl[0]).Split('/');
            //    if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
            //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
            //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
            //    drOdHour["OdDate"] = dte;
            //    dtOdHour.Rows.Add(drOdHour);
            //    dbconvfrm = dbconvfrm.AddDays(1);
            //}
            //string tothrs = string.Empty;
            //string fsthalfhrs = string.Empty;
            //string scndhalfhrs = string.Empty;
            //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            //if (htHours.Count == 0)
            //{
            //    attendenace("", "", 1);
            //}
            //foreach (DictionaryEntry parameter1 in htHours)
            //{
            //    string daytext = Convert.ToString(parameter1.Key);
            //    string noofhours = Convert.ToString(parameter1.Value);
            //    if (daytext == "full")
            //    {
            //        tothrs = noofhours;
            //    }
            //    if (daytext == "fn")
            //    {
            //        fsthalfhrs = noofhours;
            //    }
            //    if (daytext == "an")
            //    {
            //        scndhalfhrs = noofhours;
            //    }
            //}
            //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
            //if (tothrs != "" && tothrs != null)
            //{
            //    string temp = string.Empty;
            //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
            //    {
            //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
            //    }
            //}

            //gviewOD.DataSource = dtOdHour;
            //gviewOD.DataBind();
            //gviewOD.Visible = true;

            //if (gviewOD.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dthour.Rows.Count; i++)
            //    {
            //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
            //        {
            //            gviewOD.Columns[i + 1].Visible = true;
            //        }
            //        else
            //        {
            //            gviewOD.Columns[i + 1].Visible = false;
            //        }
            //    }
            //}
            #endregion
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void txtToDateOD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            try
            {
                #region
                //fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
                //toDate = Convert.ToString(txtToDateOD.Text).Trim();
                //if (fromDate.Trim() != "")
                //{
                //    isValidDate = false;
                //    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                //    isValidFromDate = isValidDate;
                //    if (!isValidDate)
                //    {
                //        lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                //        lblODAlertMsg.Visible = true;
                //        divPopODAlert.Visible = true;
                //        gviewOD.Visible = false;
                //        return;
                //    }
                //}
                //else
                //{
                //    lblODAlertMsg.Text = "Please Choose From Date";
                //    lblODAlertMsg.Visible = true;
                //    divPopODAlert.Visible = true;
                //    gviewOD.Visible = false;
                //    return;
                //}
                //if (toDate.Trim() != "")
                //{
                //    isValidDate = false;
                //    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                //    isValidToDate = isValidDate;
                //    if (!isValidDate)
                //    {
                //        lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                //        lblODAlertMsg.Visible = true;
                //        divPopODAlert.Visible = true;
                //        gviewOD.Visible = false;
                //        return;
                //    }
                //}
                //else
                //{
                //    lblODAlertMsg.Text = "Please Choose To Date";
                //    lblODAlertMsg.Visible = true;
                //    divPopODAlert.Visible = true;
                //    gviewOD.Visible = false;
                //    return;
                //}
                //string qryDate = string.Empty;
                //if (dtFromDate > dtToDate)
                //{
                //    lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                //    lblODAlertMsg.Visible = true;
                //    divPopODAlert.Visible = true;
                //    gviewOD.Visible = false;
                //    return;
                //}

                //DataTable dtOdHour = new DataTable();
                //DataRow drOdHour;

                //dtOdHour.Columns.Add("OdDate");
                //drOdHour = dtOdHour.NewRow();
                //dtOdHour.Rows.Add(drOdHour);

                //string[] dbsplfm = fromDate.Split('/');
                //string[] dbspltoo = toDate.Split('/');
                //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
                //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
                //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
                //DateTime dbconvto = Convert.ToDateTime(convto);

                //while (dbconvfrm <= dbconvto)
                //{
                //    drOdHour = dtOdHour.NewRow();
                //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
                //    string[] dtespl = Convert.ToString(spl[0]).Split('/');
                //    if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
                //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
                //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
                //    drOdHour["OdDate"] = dte;
                //    dtOdHour.Rows.Add(drOdHour);
                //    dbconvfrm = dbconvfrm.AddDays(1);
                //}
                //string tothrs = string.Empty;
                //string fsthalfhrs = string.Empty;
                //string scndhalfhrs = string.Empty;
                //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
                //if (htHours.Count == 0)
                //{
                //    attendenace("", "", 1);
                //}
                //foreach (DictionaryEntry parameter1 in htHours)
                //{
                //    string daytext = Convert.ToString(parameter1.Key);
                //    string noofhours = Convert.ToString(parameter1.Value);
                //    if (daytext == "full")
                //    {
                //        tothrs = noofhours;
                //    }
                //    if (daytext == "fn")
                //    {
                //        fsthalfhrs = noofhours;
                //    }
                //    if (daytext == "an")
                //    {
                //        scndhalfhrs = noofhours;
                //    }
                //}
                //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
                //if (tothrs != "" && tothrs != null)
                //{
                //    string temp = string.Empty;
                //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                //    {
                //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
                //    }
                //}

                //gviewOD.DataSource = dtOdHour;
                //gviewOD.DataBind();
                //gviewOD.Visible = true;

                //if (gviewOD.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dthour.Rows.Count; i++)
                //    {
                //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
                //        {
                //            gviewOD.Columns[i + 1].Visible = true;
                //        }
                //        else
                //        {
                //            gviewOD.Columns[i + 1].Visible = false;
                //        }
                //    }
                //}
                #endregion
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkStudentWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gviewOD.Visible = false;
            gviewstudetails.Visible = false;
            txtStudent.Text = string.Empty;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;

            txtFromDateOD.Attributes.Add("readonly", "readonly");
            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtToDateOD.Attributes.Add("readonly", "readonly");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

            BindHour();
            BindMinute();

            ddlInTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlInTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlInTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            ddlOutTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlOutTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlOutTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            if (chkStudentWise.Checked)
            {
                // divSearchAllStudents.Visible = false; // commented by Deepali on 6.4.18
                //-------------------added by Deepali on 6.4.18
                //lblPopODErr.Visible = false;
                ViewState["odtable"] = null;
                divSearchAllStudents.Visible = true;
                lblBatchOD.Visible = false;
                ddlBatchOD.Visible = false;
                lblDegreeOD.Visible = false;
                ddlDegreeOD.Visible = false;
                lblBranchOD.Visible = false;
                ddlBranchOD.Visible = false;
                lblSemOD.Visible = false;
                ddlSemOD.Visible = false;
                lblSecOD.Visible = false;
                ddlSecOD.Visible = false;
                ddlCollegeOD.Width = 250;

                //------------------------------------
                divAddStudents.Visible = true;
                btnRemoveOdStudents.Visible = true;

                txtStudent.Focus();
                SetStudentWiseSettings();
                Init_Spread(1);
                ShowStudentsList(1);

                #region com
                //DataTable dtOdHour = new DataTable();
                //DataRow drOdHour;

                //dtOdHour.Columns.Add("OdDate");
                //drOdHour = dtOdHour.NewRow();
                //dtOdHour.Rows.Add(drOdHour);

                //string[] dbsplfm = fromDate.Split('/');
                //string[] dbspltoo = toDate.Split('/');
                //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
                //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
                //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
                //DateTime dbconvto = Convert.ToDateTime(convto);

                //while (dbconvfrm <= dbconvto)
                //{
                //    drOdHour = dtOdHour.NewRow();
                //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
                //    string[] dtespl = Convert.ToString(spl[0]).Split('/'); if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
                //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
                //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
                //    drOdHour["OdDate"] = dte;
                //    dtOdHour.Rows.Add(drOdHour);
                //    dbconvfrm = dbconvfrm.AddDays(1);
                //}
                //string tothrs = string.Empty;
                //string fsthalfhrs = string.Empty;
                //string scndhalfhrs = string.Empty;
                //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
                //if (htHours.Count == 0)
                //{
                //    attendenace("", "", 1);
                //}
                //foreach (DictionaryEntry parameter1 in htHours)
                //{
                //    string daytext = Convert.ToString(parameter1.Key);
                //    string noofhours = Convert.ToString(parameter1.Value);
                //    if (daytext == "full")
                //    {
                //        tothrs = noofhours;
                //    }
                //    if (daytext == "fn")
                //    {
                //        fsthalfhrs = noofhours;
                //    }
                //    if (daytext == "an")
                //    {
                //        scndhalfhrs = noofhours;
                //    }
                //}
                //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
                //if (tothrs != "" && tothrs != null)
                //{
                //    string temp = string.Empty;
                //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                //    {
                //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
                //    }
                //}

                //gviewOD.DataSource = dtOdHour;
                //gviewOD.DataBind();
                //gviewOD.Visible = true;

                //if (gviewOD.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dthour.Rows.Count; i++)
                //    {
                //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
                //        {
                //            gviewOD.Columns[i + 1].Visible = true;
                //        }
                //        else
                //        {
                //            gviewOD.Columns[i + 1].Visible = false;
                //        }
                //    }
                //}
                #endregion
            }
            else
            {
                divSearchAllStudents.Visible = true;
                //-------------------added by Deepali on 6.4.18

                lblBatchOD.Visible = true;
                ddlBatchOD.Visible = true;
                lblDegreeOD.Visible = true;
                ddlDegreeOD.Visible = true;
                lblBranchOD.Visible = true;
                ddlBranchOD.Visible = true;
                lblSemOD.Visible = true;
                ddlSemOD.Visible = true;
                lblSecOD.Visible = true;
                ddlSecOD.Visible = true;
                ddlCollegeOD.Width = 80;
                //------------------------------------
                divAddStudents.Visible = false;
                btnRemoveOdStudents.Visible = false;
                BindCollegeOD();
                BindBatchOD();
                BindDegreeOD();
                BindBranchOD();
                BindSemOD();
                BindSectionDetailOD();
                Init_Spread(1);
                ShowStudentsList(0);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Index Changed Events

    //protected void FpStudentDetails_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        string actrow = e.SheetView.ActiveRow.ToString();
    //        if (flag_true == false && actrow == "0")
    //        {
    //            for (int j = 1; j < Convert.ToInt16(FpStudentDetails.Sheets[0].RowCount); j++)
    //            {
    //                string actcol = e.SheetView.ActiveColumn.ToString();
    //                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
    //                if (seltext != "System.Object")
    //                    FpStudentDetails.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
    //            }
    //            flag_true = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    private void SetStudentWiseSettings()
    {
        try
        {
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            divSearchBy.Visible = false;
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                divSearchBy.Visible = true;
                ddlSearchBy.SelectedIndex = 0;
                if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL")
                {
                    foreach (System.Web.UI.WebControls.ListItem li in ddlSearchBy.Items)
                    {
                        if (li.Text.Trim().ToLower().Contains("admission no"))
                        {
                            ddlSearchBy.SelectedValue = li.Value;
                        }
                    }
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem li in ddlSearchBy.Items)
                    {
                        if (li.Text.Trim().ToLower().Contains("roll no"))
                        {
                            ddlSearchBy.SelectedValue = li.Value;
                        }
                    }
                }
                lblStudentOptions.Text = ddlSearchBy.SelectedItem.Text;
                if (dsSearchBy.Tables[0].Rows.Count == 1)
                {
                    ddlSearchBy.Enabled = false;
                }
            }
            else
            {
                divSearchBy.Visible = false;
                if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL")
                {
                    lblStudentOptions.Text = "Admission No";
                }
                else
                {
                    lblStudentOptions.Text = "Roll No";
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void SetDefaultODEntry()
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            chkStudentWise.Checked = false;

            divHalfHr.Visible = false;
            rbFullDay.Checked = false;
            rbHalfDay.Checked = false;
            rbHourWise.Checked = false;
            txtNoOfHours.Text = string.Empty;
            txtStudent.Text = string.Empty;
            chkIncludeSplHrs.Checked = false;

            ddlInTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlInTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlInTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            ddlOutTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlOutTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlOutTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (cblHours.Items.Count > 0)
            {
                foreach (ListItem liHr in cblHours.Items)
                {
                    liHr.Selected = false;
                }
            }
            //if (ddlPurpose.Items.Count > 0)
            //{
            //    ddlPurpose.SelectedIndex = 0;
            //}
            if (ddlAttendanceOption.Items.Count > 0)
            {
                ddlAttendanceOption.SelectedIndex = 0;
            }
            if (ddlPurpose.Items.Count > 0)
            {
                ddlPurpose.SelectedIndex = 0;
            }
            ddlSecOD_SelectedIndexChanged(new object(), new EventArgs());

            if (gviewstudetails.Rows.Count > 2)
            {
                for (int row = 2; row < gviewstudetails.Rows.Count; row++)
                {
                    CheckBox chk = gviewstudetails.Rows[row].FindControl("check") as CheckBox;
                    chk.Checked = false;
                }
            }
            #region com
            //DataTable dtOdHour = new DataTable();
            //DataRow drOdHour;

            //dtOdHour.Columns.Add("OdDate");
            //drOdHour = dtOdHour.NewRow();
            //dtOdHour.Rows.Add(drOdHour);

            //string[] dbsplfm = fromDate.Split('/');
            //string[] dbspltoo = toDate.Split('/');
            //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
            //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
            //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
            //DateTime dbconvto = Convert.ToDateTime(convto);

            //while (dbconvfrm <= dbconvto)
            //{
            //    drOdHour = dtOdHour.NewRow();
            //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
            //    string[] dtespl = Convert.ToString(spl[0]).Split('/'); if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
            //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
            //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
            //    drOdHour["OdDate"] = dte;
            //    dtOdHour.Rows.Add(drOdHour);
            //    dbconvfrm = dbconvfrm.AddDays(1);
            //}
            //string tothrs = string.Empty;
            //string fsthalfhrs = string.Empty;
            //string scndhalfhrs = string.Empty;
            //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            //if (htHours.Count == 0)
            //{
            //    attendenace("", "", 1);
            //}
            //foreach (DictionaryEntry parameter1 in htHours)
            //{
            //    string daytext = Convert.ToString(parameter1.Key);
            //    string noofhours = Convert.ToString(parameter1.Value);
            //    if (daytext == "full")
            //    {
            //        tothrs = noofhours;
            //    }
            //    if (daytext == "fn")
            //    {
            //        fsthalfhrs = noofhours;
            //    }
            //    if (daytext == "an")
            //    {
            //        scndhalfhrs = noofhours;
            //    }
            //}
            //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
            //if (tothrs != "" && tothrs != null)
            //{
            //    string temp = string.Empty;
            //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
            //    {
            //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
            //    }
            //}

            //gviewOD.DataSource = dtOdHour;
            //gviewOD.DataBind();
            //gviewOD.Visible = true;

            //if (gviewOD.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dthour.Rows.Count; i++)
            //    {
            //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
            //        {
            //            gviewOD.Columns[i + 1].Visible = true;
            //        }
            //        else
            //        {
            //            gviewOD.Columns[i + 1].Visible = false;
            //        }
            //    }
            //}
            #endregion
        }
        catch (Exception ex)
        {

        }
    }

    public void attendenace(string degreecode, string semester, int type = 0)
    {
        try
        {
            //rbFullDay.Checked = true;
            //rbHalfDay.Checked = false;
            //rbHourWise.Checked = false;
            //divHalfHr.Visible = false;
            //rbAM.Visible = false;
            //rbPM.Visible = false;
            cblHours.Items.Clear();
            htHoursPerDay.Clear();
            Dictionary<string, int> dicHrsPerDay = new Dictionary<string, int>();
            DataSet dsPeriodDetails = new DataSet();
            ArrayList arrDegree = new ArrayList();
            ArrayList arrSem = new ArrayList();
            if (type != 0)
            {
                //degreecode = string.Empty;
                //semester = string.Empty;
                if (chkStudentWise.Checked)
                {
                    if (gviewstudetails.Rows.Count > 2)
                    {
                        for (int row = 1; row < gview.Rows.Count; row++)//for (int row = 0; row < FpStudentDetails.Sheets[0].RowCount; row++)
                        {
                            //string degree = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                            //string sem = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();

                            string degree = (gview.Rows[row].FindControl("lblrolltag") as Label).Text;
                            string sem = gview.Rows[row].Cells[11].Text;
                            if (!string.IsNullOrEmpty(sem))
                            {
                                if (!arrSem.Contains(sem.Trim()))
                                {
                                    arrSem.Add(sem.Trim());
                                    if (string.IsNullOrEmpty(semester))
                                    {
                                        semester = "'" + sem + "'";
                                    }
                                    else
                                    {
                                        semester += ",'" + sem + "'";
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(degree))
                            {
                                if (!arrDegree.Contains(degree.Trim()))
                                {
                                    arrDegree.Add(degree.Trim());
                                    if (string.IsNullOrEmpty(degreecode))
                                    {
                                        degreecode = "'" + degree + "'";
                                    }
                                    else
                                    {
                                        degreecode += ",'" + degree + "'";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                    semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
                }
            }
            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(semester))
            {
                string gerhours = "select no_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day from periodattndschedule where degree_code in(" + degreecode + ") and semester in(" + semester + ")";
                dsPeriodDetails = da.select_method_wo_parameter(gerhours, "Text");
            }
            string tothrs = string.Empty;
            string fsthalfhrs = string.Empty;
            string scndhalfhrs = string.Empty;
            if (dsPeriodDetails.Tables.Count > 0 && dsPeriodDetails.Tables[0].Rows.Count > 0)
            {
                tothrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_per_day"].ToString();
                if (tothrs != "")
                {
                    htHoursPerDay.Add("full", tothrs);
                }
                fsthalfhrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                if (fsthalfhrs != "")
                {
                    htHoursPerDay.Add("fn", fsthalfhrs);
                }
                scndhalfhrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                if (scndhalfhrs != "")
                {
                    htHoursPerDay.Add("an", scndhalfhrs);
                }
                Session["htHoursPerDay"] = htHoursPerDay;
            }
            #region com
            //DataTable dtOdHour = new DataTable();
            //DataRow drOdHour;

            //dtOdHour.Columns.Add("OdDate");
            //drOdHour = dtOdHour.NewRow();
            //dtOdHour.Rows.Add(drOdHour);

            //string[] dbsplfm = fromDate.Split('/');
            //string[] dbspltoo = toDate.Split('/');
            //string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
            //DateTime dbconvfrm = Convert.ToDateTime(convfrm);
            //string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
            //DateTime dbconvto = Convert.ToDateTime(convto);

            //while (dbconvfrm <= dbconvto)
            //{
            //    drOdHour = dtOdHour.NewRow();
            //    string[] spl = Convert.ToString(dbconvfrm).Split(' ');
            //    string[] dtespl = Convert.ToString(spl[0]).Split('/'); if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
            //    if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
            //    string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
            //    drOdHour["OdDate"] = dte;
            //    dtOdHour.Rows.Add(drOdHour);
            //    dbconvfrm = dbconvfrm.AddDays(1);
            //}

            //Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            //if (htHours.Count == 0)
            //{
            //    attendenace("", "", 1);
            //}
            //foreach (DictionaryEntry parameter1 in htHours)
            //{
            //    string daytext = Convert.ToString(parameter1.Key);
            //    string noofhours = Convert.ToString(parameter1.Value);
            //    if (daytext == "full")
            //    {
            //        tothrs = noofhours;
            //    }
            //    if (daytext == "fn")
            //    {
            //        fsthalfhrs = noofhours;
            //    }
            //    if (daytext == "an")
            //    {
            //        scndhalfhrs = noofhours;
            //    }
            //}
            //DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
            //if (tothrs != "" && tothrs != null)
            //{
            //    string temp = string.Empty;
            //    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
            //    {
            //        dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
            //    }
            //}

            //gviewOD.DataSource = dtOdHour;
            //gviewOD.DataBind();
            //gviewOD.Visible = true;

            //if (gviewOD.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dthour.Rows.Count; i++)
            //    {
            //        if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
            //        {
            //            gviewOD.Columns[i + 1].Visible = true;
            //        }
            //        else
            //        {

            //        }
            //    }
            //    for (int j = dthour.Rows.Count; j < gviewOD.Columns.Count - 1; j++)
            //    {
            //        gviewOD.Columns[j + 1].Visible = false;
            //    }
            //}


            ////string temp = string.Empty;
            //if (tothrs != "" && tothrs != null)
            //{
            //    if (rbFullDay.Checked == true)
            //    {
            //        for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
            //        {
            //            cblHours.Items.Add("" + fulhrs + "");
            //        }
            //    }
            //}
            #endregion
        }
        catch
        {
        }
    }

    protected void btnAddStudent_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["odtable"] = null;
            ShowStudentsList(1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnRemoveOdStudents_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            divPopODAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            //chkAll.AutoPostBack = true;
            //Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            //Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("Batch");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("DegreeCode");
            dt.Columns.Add("RegNo");
            dt.Columns.Add("Section");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("Semester");
            dt.Columns.Add("AdmissionNo");
            dt.Columns.Add("AppNo");
            dt.Columns.Add("CollegeCode");
            dt.Columns.Add("courseId");
            dt.Columns.Add("odcnt");
            if (gviewstudetails.Rows.Count > 1)
            {
                int count = 0;
                bool isremove = false;
                for (int row = 1; row < gviewstudetails.Rows.Count; row++)
                {
                    CheckBox chk = gviewstudetails.Rows[row].FindControl("check") as CheckBox;
                    if (chk.Checked)
                    {
                        count++;
                        isremove = true;
                    }
                    else
                    {
                        //string batchYearNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Tag).Trim();
                        //string collegeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Note).Trim();
                        //string rollno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Text).Trim();
                        //string degreeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                        //string regno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Text).Trim();
                        //string sectionNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Tag).Trim();
                        //string courseID = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Note).Trim();
                        //string admissionno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 3].Text).Trim();
                        //string studname = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 4].Text).Trim();
                        //string semesterNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();
                        //string appNo = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Tag).Trim();

                        string batchYearNew = (gviewstudetails.Rows[row].FindControl("lblsnotag") as Label).Text;
                        string collegeCodeNew = (gviewstudetails.Rows[row].FindControl("lblsnonote") as Label).Text;
                        string rollno = (gviewstudetails.Rows[row].FindControl("lblroll") as Label).Text;
                        string degreeCodeNew = (gviewstudetails.Rows[row].FindControl("lblrolltag") as Label).Text;
                        string regno = (gviewstudetails.Rows[row].FindControl("lblreg") as Label).Text;
                        string sectionNew = (gviewstudetails.Rows[row].FindControl("lblregtag") as Label).Text;
                        string courseID = (gviewstudetails.Rows[row].FindControl("lblregnote") as Label).Text;
                        string admissionno = (gviewstudetails.Rows[row].FindControl("lbladmno") as Label).Text;
                        string studname = (gviewstudetails.Rows[row].FindControl("lblstunme") as Label).Text;
                        string semesterNew = (gviewstudetails.Rows[row].FindControl("lblsem") as Label).Text;
                        string appNo = (gviewstudetails.Rows[row].FindControl("lblsemtag") as Label).Text;
                        string odcout = (gviewstudetails.Rows[row].FindControl("lblodcon") as Label).Text;


                        dr = dt.NewRow();
                        dr["Batch"] = batchYearNew;
                        dr["RollNo"] = rollno;
                        dr["RegNo"] = regno;
                        dr["DegreeCode"] = degreeCodeNew;
                        dr["Section"] = sectionNew;
                        dr["StudentName"] = studname;
                        dr["Semester"] = semesterNew;
                        dr["AdmissionNo"] = admissionno;
                        dr["AppNo"] = appNo;
                        dr["CollegeCode"] = collegeCodeNew;
                        dr["courseId"] = courseID;
                        dr["odcnt"] = odcout;
                        dt.Rows.Add(dr);
                    }
                }
                if (isremove)
                {
                    Init_Spread(1);
                    int serialNo = 0;
                    foreach (DataRow drStudents in dt.Rows)
                    {
                        serialNo++;
                        string batchYearNew = Convert.ToString(drStudents["Batch"]).Trim();
                        string rollno = Convert.ToString(drStudents["RollNo"]).Trim();
                        string degreeCodeNew = Convert.ToString(drStudents["DegreeCode"]).Trim();
                        string regno = Convert.ToString(drStudents["RegNo"]).Trim();
                        string sectionNew = Convert.ToString(drStudents["Section"]).Trim();
                        string studname = Convert.ToString(drStudents["StudentName"]).Trim();
                        string semesterNew = Convert.ToString(drStudents["Semester"]).Trim();
                        string admissionno = Convert.ToString(drStudents["AdmissionNo"]);
                        string appNo = Convert.ToString(drStudents["appNo"]).Trim();
                        string collegeCodeNew = Convert.ToString(drStudents["CollegeCode"]).Trim();
                        string courseID = Convert.ToString(drStudents["courseId"]).Trim();
                        string odcont = Convert.ToString(drStudents["odcnt"]).Trim();

                        drdetail = dtdetail.NewRow();
                        drdetail["sno"] = Convert.ToString(serialNo).Trim();
                        drdetail["snotag"] = Convert.ToString(batchYearNew).Trim();
                        drdetail["snonote"] = Convert.ToString(collegeCodeNew).Trim();
                        drdetail["Roll No"] = Convert.ToString(rollno).Trim();
                        drdetail["Rolltag"] = Convert.ToString(degreeCodeNew).Trim();
                        drdetail["Rollvalue"] = Convert.ToString(rollno).Trim();
                        drdetail["Reg No"] = Convert.ToString(regno).Trim();
                        drdetail["Regtag"] = Convert.ToString(sectionNew).Trim();
                        drdetail["Regnote"] = Convert.ToString(courseID).Trim();
                        drdetail["Admission No"] = Convert.ToString(admissionno).Trim();
                        drdetail["Student Name"] = Convert.ToString(studname).Trim();
                        drdetail["Semester"] = Convert.ToString(semesterNew).Trim();
                        drdetail["Semestertag"] = Convert.ToString(appNo).Trim();
                        drdetail["OD Count"] = Convert.ToString(odcont).Trim();

                        dtdetail.Rows.Add(drdetail);
                    }
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();

                    ViewState["odtable"] = dtdetail;
                    gviewstudetails.DataSource = dtdetail;
                    gviewstudetails.DataBind();
                    gviewstudetails.Visible = false;
                    if (gviewstudetails.Rows.Count > 1)
                    {
                        gviewstudetails.Visible = true;
                    }

                    gviewstudetails.Columns[1].Visible = isRollVisible;
                    gviewstudetails.Columns[2].Visible = isRegVisible;
                    gviewstudetails.Columns[3].Visible = isAdmitNoVisible;

                    for (int row = 1; row < gviewstudetails.Rows.Count; row++)
                    {
                        //string cnt = (gviewstudetails.Rows[row].FindControl("lblodcon") as Label).Text;
                        double per_tot_ondu1 = Convert.ToDouble((gviewstudetails.Rows[row].FindControl("lblodcon") as Label).Text);
                        double TotalCount1 = 0;
                        double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount1);
                        if (TotalCount1 != 0 && TotalCount1 <= per_tot_ondu1)
                        {
                            gviewstudetails.Rows[row].BackColor = Color.Tan;

                        }
                        else
                        {
                            //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                        }
                    }

                    if (Convert.ToString(ViewState["ODCheck"]).Trim() != "0")
                    {
                        gviewstudetails.Columns[6].Visible = true;
                    }
                    else
                    {
                        gviewstudetails.Columns[6].Visible = false;
                    }
                }
                if (count == 0)
                {
                    divPopODAlert.Visible = true;
                    //-------------------------------comment and added by Deepali on 6.4.18
                    //lblPopODErr.Text = "Please Select Atleast One Student";
                    //lblPopODErr.Visible = true;
                    lblODAlertMsg.Text = "Please Select Atleast One Student";
                    lblODAlertMsg.Visible = true;
                    //------------------------------
                    return;
                }
            }
            else
            {
                Init_Spread(1);
                divPopODAlert.Visible = true;
                //---------comment and added by Deepali on 6.4.18
                //lblPopODErr.Text = "No Student Were Found";
                //lblPopODErr.Visible = true;
                divPopODAlert.Visible = true;
                lblODAlertMsg.Text = "No Student Were Found";
                lblODAlertMsg.Visible = true;
                //------------------------------
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnPopDeleteOD_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirm.Visible = true;
            lblSaveorDelete.Text = "2";
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnPopSaveOD_Click(object sender, EventArgs e)
    {
        try
        {

            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose From Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            int reval = 0;
            bool isSelected = false;
            if (btnPopSaveOD.Text.Trim().ToLower() == "save")
            {
                reval = 1;
            }
            else
            {
                reval = 0;
            }
            if (ddlAttendanceOption.Items.Count == 0)
            {
                lblODAlertMsg.Text = "There is No Attendance Right(s) To This User";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }

            if (gviewstudetails.Rows.Count == reval)
            {
                lblODAlertMsg.Text = "No Students Were Found";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            //if (cblHours.Items.Count == 0)
            //{
            //    lblODAlertMsg.Text = "There is No Hour(s) Were Found";
            //    lblODAlertMsg.Visible = true;
            //    divPopODAlert.Visible = true;
            //    return;
            //}
            //else
            //{
            //    bool hourSelect = false;
            //    foreach (ListItem liHr in cblHours.Items)
            //    {
            //        if (liHr.Selected)
            //        {
            //            hourSelect = true;
            //        }
            //    }
            //    if (!hourSelect)
            //    {
            //        lblODAlertMsg.Text = "Please Select Any One Hour And Then Proceed";
            //        lblODAlertMsg.Visible = true;
            //        divPopODAlert.Visible = true;
            //        return;
            //    }
            //}

            for (int res = 1; res < gviewstudetails.Rows.Count; res++)
            {

                CheckBox chk = (gviewstudetails.Rows[res].FindControl("check") as CheckBox);
                if (chk.Checked)
                {
                    isSelected = true;
                }
            }
            if (gviewOD.Visible)
            {

            }
            if (isSelected)
            {
                int selectcol = 0;
                for (int od = 0; od < gviewOD.Rows.Count; od++)
                {
                    for (int col = 1; col < gviewOD.Rows[od].Cells.Count; col++)
                    {
                        if (gviewOD.Columns[col].Visible == true)
                        {
                            CheckBox chkbx = (gviewOD.Rows[od].FindControl("chkhour" + col) as CheckBox);
                            if (chkbx.Checked)
                            {
                                selectcol++;
                            }
                        }
                    }
                }
                if (selectcol == 0)
                {
                    lblODAlertMsg.Text = "Please Mark OD For The Student";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }


                bool daychek = AttendanceDayLock(dtFromDate, ((chkStudentWise.Checked) ? 1 : 0));
                if (Session["UserName"].ToString().Trim() == "admin")
                {
                    daychek = true;
                }
                if (daychek == true)
                {
                    if (btnPopSaveOD.Text.Trim().ToLower() == "save" || btnPopSaveOD.Text.Trim().ToLower() == "update")
                    {
                        lblSaveorDelete.Text = "1";
                    }
                    else
                    {
                        lblSaveorDelete.Text = "2";
                    }

                    #region added by prabha on feb 15 2018

                    string odexceededstudents = string.Empty;
                    string Attvalue = string.Empty;
                    fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
                    toDate = Convert.ToString(txtToDateOD.Text).Trim();
                    if (fromDate.Trim() != "")
                    {
                        isValidDate = false;
                        isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                        isValidFromDate = isValidDate;
                        if (!isValidDate)
                        {
                            lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                            lblODAlertMsg.Visible = true;
                            divPopODAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblODAlertMsg.Text = "Please Choose From Date";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                    if (toDate.Trim() != "")
                    {
                        isValidDate = false;
                        isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                        isValidToDate = isValidDate;
                        if (!isValidDate)
                        {
                            lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                            lblODAlertMsg.Visible = true;
                            divPopODAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblODAlertMsg.Text = "Please Choose To Date";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                    qryDate = string.Empty;
                    if (dtFromDate > dtToDate)
                    {
                        lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                    string dt = fromDate;
                    string strholiday = string.Empty;
                    bool isSchoolAttendance = false;
                    string[] dsplit = dt.Split(new Char[] { '/' });
                    fromDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    int demfcal = int.Parse(dsplit[2].ToString());
                    demfcal = demfcal * 12;
                    int cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                    string monthcal = cal_from_date.ToString();
                    dt = toDate;
                    dsplit = dt.Split(new Char[] { '/' });
                    toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    int demtcal = int.Parse(dsplit[2].ToString());
                    demtcal = demfcal * 12;
                    int cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                    DateTime per_from_date = Convert.ToDateTime(fromDate);
                    DateTime per_to_date = Convert.ToDateTime(toDate);
                    DateTime dumm_from_date = per_from_date;
                    int flag = 0;
                    reval = 0;
                    string leavhlf = string.Empty;
                    bool setDefault = false;
                    if (btnPopSaveOD.Text.Trim().ToLower() == "save")
                    {
                        reval = 1;
                        setDefault = true;
                    }
                    else
                    {
                        reval = 0;
                        setDefault = false;
                    }
                    int chkstudcount = 0;
                    string ErrorMsg = string.Empty;
                    //string qrySections = string.Empty;
                    //string qrySemesters = string.Empty;
                    //string qryDegreeCodes = string.Empty;
                    //string qryBatchYears = string.Empty;
                    //string qryCollegeCodes = string.Empty;
                    //if (!string.IsNullOrEmpty(ddlSecOD.SelectedItem.Text))
                    //{
                    //    qrySections = " and sections in('" + ddlSecOD.SelectedItem.Text + "')";
                    //}
                    //if (!string.IsNullOrEmpty(ddlSemOD.SelectedItem.Text))
                    //{
                    //    qrySemesters = " and r.current_semester in(" + ddlSemOD.SelectedItem.Text + ")";
                    //}
                    //if (!string.IsNullOrEmpty(ddlBranchOD.SelectedItem.Value))
                    //{
                    //    qryDegreeCodes = " and r.degree_code in(" + ddlBranchOD.SelectedItem.Value + ")";
                    //}
                    //if (!string.IsNullOrEmpty(ddlBatchOD.SelectedItem.Text))
                    //{
                    //    qryBatchYears = " and r.Batch_year in(" + ddlBatchOD.SelectedItem.Text + ")";
                    //}
                    //if (!string.IsNullOrEmpty(ddlCollegeOD.SelectedItem.Value))
                    //{
                    //    qryCollegeCodes = " and r.college_code in(" + ddlCollegeOD.SelectedItem.Value + ")";
                    //}
                    //string fromDates = string.Empty;
                    //string toDates = string.Empty;

                    //fromDates = Convert.ToString(txtFromDateOD.Text).Trim();
                    //toDates = Convert.ToString(txtToDateOD.Text).Trim();
                    //if (fromDates.Trim() != "")
                    //{
                    //    isValidDate = false;
                    //    isValidDate = DateTime.TryParseExact(fromDates.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDates);
                    //    isValidFromDate = isValidDate;
                    //    if (!isValidDate)
                    //    {
                    //        lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    //        lblAlertMsg.Visible = true;
                    //        divPopAlert.Visible = true;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    lblAlertMsg.Text = "Please Choose From Date";
                    //    lblAlertMsg.Visible = true;
                    //    divPopAlert.Visible = true;
                    //    return;
                    //}
                    //if (toDates.Trim() != "")
                    //{
                    //    isValidDate = false;
                    //    isValidDate = DateTime.TryParseExact(toDates.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDates);
                    //    isValidToDate = isValidDate;
                    //    if (!isValidDate)
                    //    {
                    //        lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    //        lblAlertMsg.Visible = true;
                    //        divPopAlert.Visible = true;
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    lblAlertMsg.Text = "Please Choose To Date";
                    //    lblAlertMsg.Visible = true;
                    //    divPopAlert.Visible = true;
                    //    return;
                    //}
                    //string qryDates = string.Empty;
                    //if (dtFromDates > dtToDates)
                    //{
                    //    lblAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                    //    lblAlertMsg.Visible = true;
                    //    divPopAlert.Visible = true;
                    //    return;
                    //}
                    //else
                    //{
                    //    //qryDate = " and convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                    //    // qryDate = "and (convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105) between  '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "')";//Rajkumar on 
                    //    qryDates = " and (convert(datetime,od.fromdate,105) >= '" + dtFromDates.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105)>='" + dtFromDates.ToString("MM/dd/yyyy") + "') and  (convert(datetime,od.fromdate,105) <='" + dtToDates.ToString("MM/dd/yyyy") + "' or convert(datetime,od.Todate,105)<= '" + dtToDates.ToString("MM/dd/yyyy") + "')";
                    //    //qryDate = "and (convert(datetime,od.fromdate,105) >= '" + dtFromDate.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105)>='" + dtFromDate.ToString("MM/dd/yyyy") + "') and  convert(datetime,od.fromdate,105) <='" + dtToDate.ToString("MM/dd/yyyy") + "' or (convert(datetime,od.Todate,105)<= '" + dtToDate.ToString("MM/dd/yyyy") + "')";//Rajkumar on 

                    //}

                    //qry = "select distinct r.college_code,r.Current_Semester,r.Batch_Year,r.roll_no,r.reg_no,r.Roll_Admit,r.stud_name,od.purpose,convert(varchar, od.fromdate, 103) as fromdate,convert(varchar, od.todate, 103)  as todate,convert(varchar, od.outtime, 108) as outtime,convert(varchar, od.intime, 108) as intime,od.attnd_type,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,od.no_of_hourse,od.hourse,r.degree_code,r.sections,r.app_no from registration r,onduty_stud od where od.roll_no=r.roll_no  " + qryCollegeCodes + qryBatchYears + qryDegreeCodes + qrySemesters + qrySections + qryDates;
                    //dsODStudentDetailss.Clear();
                    //dsODStudentDetailss = da.select_method_wo_parameter(qry, "text");

                    for (int res = reval; res < gviewstudetails.Rows.Count; res++)
                    {
                        CheckBox chk = gviewstudetails.Rows[res].FindControl("check") as CheckBox;

                        if (chk.Checked)
                        {
                            //chkstudcount++;
                            //string rollno = (gviewstudetails.Rows[res].FindControl("lblroll") as Label).Text;
                            //dsODStudentDetailss.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                            //DataTable dtisRedo = dsODStudentDetailss.Tables[0].DefaultView.ToTable();
                            //if (dtisRedo.Rows.Count > 0)
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Student already have OD in same day')", true);
                            //    return;
                            //}
                            string batchval = (gviewstudetails.Rows[res].FindControl("lblsnotag") as Label).Text;
                            string degree = (gviewstudetails.Rows[res].FindControl("lblrolltag") as Label).Text;
                            string semester = (gviewstudetails.Rows[res].FindControl("lblsem") as Label).Text;
                            string section = (gviewstudetails.Rows[res].FindControl("lblregtag") as Label).Text;
                            string collegeCode = (gviewstudetails.Rows[res].FindControl("lblsnonote") as Label).Text;
                            string ODCount = (gviewstudetails.Rows[res].FindControl("lblodcon") as Label).Text;

                            //Color backclr = FpStudentDetails.Sheets[0].Rows[res].BackColor;
                            int TakenOd = 0;
                            if (ODCount.Trim() != "0" && ODCount.Trim() != "")
                            {
                                int.TryParse(ODCount, out TakenOd);
                            }
                            int MaxODCount = 0;
                            int.TryParse(Convert.ToString(ViewState["ODCont"]), out MaxODCount);
                            ht.Clear();
                            ht.Add("degree_code", int.Parse(degree.ToString()));
                            ht.Add("sem", int.Parse(semester));
                            ht.Add("from_date", fromDate.ToString());
                            ht.Add("to_date", toDate.ToString());
                            ht.Add("coll_code", int.Parse(collegeCode));
                            int iscount = 0;
                            DataSet ds2 = new DataSet();
                            qry = "select isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and degree_code=" + degree + " and semester=" + semester.ToString() + "";
                            ds2.Reset();
                            ds2.Dispose();
                            ds2 = da.select_method(qry, ht, "Text");
                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                iscount = Convert.ToInt16(ds2.Tables[0].Rows[0]["cnt"].ToString());
                            }
                            ht.Add("iscount", iscount);
                            DataSet ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");
                            isSchoolAttendance = false;
                            isSchoolAttendance = CheckSchoolOrCollege(collegeCode);
                            Hashtable holiday_table = new Hashtable();
                            holiday_table.Clear();
                            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            int fhrs = 0;
                            string hrs = da.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degree.ToString() + " and semester='" + semester.ToString() + "'");
                            DataSet dsval = new DataSet();
                            DataView dvval = new DataView();
                            int noMaxHrsDay = 0;
                            int noFstHrsDay = 0;
                            int noSndHrsDay = 0;
                            int noMinFstHrsDay = 0;
                            int noMinSndHrsDay = 0;
                            string selQ = " select CONVERT(varchar(50), start_date,105) as start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,p.no_of_hrs_I_half_day,p.no_of_hrs_II_half_day,s.batch_year,s.degree_code,s.semester,p.min_pres_I_half_day,p.min_pres_II_half_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=" + degree.ToString() + " and p.semester='" + semester.ToString() + "'";
                            dsval.Clear();
                            dsval = da.select_method_wo_parameter(selQ, "Text");
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {
                                //sch_order = dv1[0]["schorder"].ToString();
                                //no_days = dv1[0]["nodays"].ToString();
                                //startdate = dv1[0]["start_date"].ToString();
                                //starting_dayorder = dv1[0]["starting_dayorder"].ToString();
                                //no_of_hrs = dv1[0]["No_of_hrs_per_day"].ToString();
                                //frst_half_day = dv1[0]["no_of_hrs_I_half_day"].ToString();
                                //secd_half_day = dv1[0]["no_of_hrs_II_half_day"].ToString();
                                //min_frst_half_day = dv1[0]["min_pres_I_half_day"].ToString();
                                //min_secd_half_day = dv1[0]["min_pres_II_half_day"].ToString();
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["No_of_hrs_per_day"]), out noMaxHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_I_half_day"]), out noFstHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_II_half_day"]), out noSndHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_I_half_day"]), out noMinFstHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_II_half_day"]), out noMinSndHrsDay);
                            }
                            //no_of_hrs = dv1[0]["No_of_hrs_per_day"].ToString();
                            //frst_half_day = dv1[0]["no_of_hrs_I_half_day"].ToString();
                            //secd_half_day = dv1[0]["no_of_hrs_II_half_day"].ToString();
                            //min_frst_half_day = dv1[0]["min_pres_I_half_day"].ToString();
                            //min_secd_half_day = dv1[0]["min_pres_II_half_day"].ToString();
                            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
                            {
                                fhrs = Convert.ToInt32(hrs);
                            }
                            bool leaveflag = false;
                            string strsec = string.Empty;
                            if (ddlSecOD.Items.Count > 0)
                            {
                                if (ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "all" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "-1")
                                {
                                    strsec = " and sm.sections='" + ddlSecOD.SelectedValue.ToString() + "'";
                                }
                                else
                                {
                                    strsec = string.Empty;
                                }
                            }
                            string strspecdeiatlqury = "select sm.date,sd.hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and sm.batch_year='" + batchval.ToString() + "' and sm.date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and sm.degree_code=" + degree.ToString() + " and sm.semester='" + semester.ToString() + "' " + strsec + "";
                            DataSet dsspecial = da.select_method_wo_parameter(strspecdeiatlqury, "Text");
                            flag = 1;

                            string appNo = (gviewstudetails.Rows[res].FindControl("lblsemtag") as Label).Text;
                            string stdRollno = (gviewstudetails.Rows[res].FindControl("lblrollvalue") as Label).Text;
                            string stdregno = (gviewstudetails.Rows[res].FindControl("lblreg") as Label).Text;
                            string stdname = (gviewstudetails.Rows[res].FindControl("lblstunme") as Label).Text;
                            string stdsem = (gviewstudetails.Rows[res].FindControl("lblsem") as Label).Text;

                            string AdmitDate = string.Empty;
                            int taken_hourse = 0;
                            int monthyear = 0;
                            int tothrscount = 0;
                            if (txtFromDateOD.Text != "")//&& txttodate.Text != "")
                            {
                                if (gviewOD.Visible == true)//if (txtNoOfHours.Text != "" || rbFullDay.Checked == true)
                                {
                                    fromDate = txtFromDateOD.Text;
                                    toDate = txtToDateOD.Text;
                                    dt = fromDate;
                                    dsplit = dt.Split(new Char[] { '/' });
                                    fromDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    demfcal = int.Parse(dsplit[2].ToString());
                                    demfcal = demfcal * 12;
                                    cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                    monthcal = cal_from_date.ToString();
                                    dt = toDate;
                                    dsplit = dt.Split(new Char[] { '/' });
                                    toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    demtcal = int.Parse(dsplit[2].ToString());
                                    demtcal = demfcal * 12;
                                    cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                                    per_from_date = Convert.ToDateTime(fromDate);
                                    per_to_date = Convert.ToDateTime(toDate);
                                    dumm_from_date = per_from_date;
                                    int totnoofhours = 0;
                                    string[] hourslimit = txtNoOfHours.Text.Split(new char[] { ',' });
                                    totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                    int intNoOfOdCount = 0;
                                    string selectddl_value = string.Empty;
                                    selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                    Attvalue = GetAttendanceStatusCode(selectddl_value);
                                    ArrayList DateHAsh = new ArrayList();

                                    if (Attvalue.Trim() == "3")
                                    {
                                        if (MaxODCount != 0)
                                        {
                                            if (dumm_from_date <= per_to_date)
                                            {
                                                while (dumm_from_date <= per_to_date)
                                                {
                                                    if (!holiday_table.ContainsKey(dumm_from_date) && dumm_from_date.ToString("dddd") != "Sunday")
                                                    {
                                                        tothrscount = tothrscount + totnoofhours;
                                                        intNoOfOdCount += totnoofhours;
                                                        DateHAsh.Add(dumm_from_date);
                                                    }
                                                    dumm_from_date = dumm_from_date.AddDays(1);
                                                }
                                                double TakenCont = 0;
                                                AttendancePercentage(collegeCode, batchval, degree, semester, stdRollno, AdmitDate, ref TakenCont, DateHAsh);
                                                intNoOfOdCount += Convert.ToInt32(TakenCont);
                                                int noododcountstud = tothrscount + Convert.ToInt32(ODCount);
                                                if (MaxODCount < noododcountstud)
                                                {
                                                    //ErrorMsg += " <br>" + stdRollno + " Exceeding Max no of OD Count";
                                                    odexceededstudents += "- " + stdRollno;
                                                    //divConfirm.Visible = true;
                                                    //lblConfirmMsg.Text = "Max No. of OD Limit Exceeded for "+odexceededstudents+" !!!\nDo You Wish To Continue ?";
                                                }
                                                else
                                                {
                                                    //divConfirm.Visible = true;
                                                    //lblConfirmMsg.Text = "Do you want mark attendance from " + dtFromDate.ToString("dd/MM/yyyy") + " to " + dtToDate.ToString("dd/MM/yyyy");
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                    #endregion
                        }
                    }  //end of student for loop
                    if (!string.IsNullOrEmpty(odexceededstudents))
                    {
                        string alterval = da.GetFunction("select linkValue from inssettings where linkName='OD Limit Exceeds' and College_code ='" + ddlCollege.SelectedValue + "'");

                        if (alterval == "1")
                        {
                            if (chkstudcount == 1) //added by Mullai
                            {
                                divPopODAlert.Visible = true;

                                lblODAlertMsg.Text = "Max No. of OD Limit Exceeded for " + odexceededstudents + "";
                                lblODAlertMsg.Visible = true;
                            }
                            else
                            {
                                divConfirm.Visible = true;
                                lblConfirmMsg.Text = "Max No. of OD Limit Exceeded for " + odexceededstudents + " !!!\nDo You Wish To Continue ?";
                            }
                        }
                        else
                        {
                            divConfirm.Visible = true;
                            lblConfirmMsg.Text = "Max No. of OD Limit Exceeded for " + odexceededstudents + " !!!\nDo You Wish To Continue ?";
                        }
                    }
                    else
                    {
                        divConfirm.Visible = true;
                        lblConfirmMsg.Text = "Do you want mark attendance from " + dtFromDate.ToString("dd/MM/yyyy") + " to " + dtToDate.ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                    lblODAlertMsg.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Select Atleast One Student and Then Proceed";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnPopExitOD_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["odtable"] = null;
            divODEntryDetails.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void save()
    {
        try
        {
            Hashtable hasreason = new Hashtable();
            string hours = string.Empty;
            string[] spl1 = new string[0];
            bool odckeck = false;
            int cont = 0;
            string finalhour = string.Empty;
            DataSet dsODStudentDetailss = new DataSet();
            divConfirm.Visible = false;
            bool save_flag = false;
            bool isSaveAttendance = false;
            int savevalue = 0;
            string Attvalue = string.Empty;

            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();

            //added by mullai 17/3/18
            DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out chk);
            bool isval1 = DayLockForUser(chk);

            if (!isval1)
            {
                lblODAlertMsg.Text = "You cannot edit this day attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;

            }
            // }
            //============================


            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose From Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string dt = fromDate;
            string strholiday = string.Empty;
            string reason = string.Empty;
            string purpose = string.Empty;
            if (ddlPurpose.Text == "")
            {
                reason = string.Empty;
                purpose = string.Empty;
            }
            else
            {
                purpose = Convert.ToString(ddlPurpose.SelectedItem).Trim();
                reason = Convert.ToString(ddlPurpose.SelectedItem).Trim();
            }
            bool isSchoolAttendance = false;
            string[] dsplit = dt.Split(new Char[] { '/' });
            fromDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            int demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            int cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            string monthcal = cal_from_date.ToString();
            dt = toDate;
            dsplit = dt.Split(new Char[] { '/' });
            toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            int demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demfcal * 12;
            int cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
            DateTime per_from_date = Convert.ToDateTime(fromDate);
            DateTime per_to_date = Convert.ToDateTime(toDate);
            DateTime dumm_from_date = per_from_date;
            lblPopODErr.Visible = false;
            int flag = 0;
            int reval = 0;
            string leavhlf = string.Empty;
            bool setDefault = false;
            if (btnPopSaveOD.Text.Trim().ToLower() == "save")
            {
                reval = 2;
                setDefault = true;
            }
            else
            {
                reval = 0;
                setDefault = false;
            }

            DataTable dtgridod = new DataTable();
            string ErrorMsg = string.Empty;
            string qrySections = string.Empty;
            string qrySemesters = string.Empty;
            string qryDegreeCodes = string.Empty;
            string qryBatchYears = string.Empty;
            string qryCollegeCodes = string.Empty;
            string qrys = string.Empty;
            if (!string.IsNullOrEmpty(ddlSecOD.SelectedValue))
            {
                qrySections = " and sections in('" + ddlSecOD.SelectedItem.Text + "')";
            }
            if (!string.IsNullOrEmpty(ddlSemOD.SelectedItem.Text))
            {
                qrySemesters = " and r.current_semester in(" + ddlSemOD.SelectedItem.Text + ")";
            }
            if (!string.IsNullOrEmpty(ddlBranchOD.SelectedItem.Value))
            {
                qryDegreeCodes = " and r.degree_code in(" + ddlBranchOD.SelectedItem.Value + ")";
            }
            if (!string.IsNullOrEmpty(ddlBatchOD.SelectedItem.Text))
            {
                qryBatchYears = " and r.Batch_year in(" + ddlBatchOD.SelectedItem.Text + ")";
            }
            if (!string.IsNullOrEmpty(ddlCollegeOD.SelectedItem.Value))
            {
                qryCollegeCodes = " and r.college_code in(" + ddlCollegeOD.SelectedItem.Value + ")";
            }
            string fromDates = string.Empty;
            string toDates = string.Empty;

            fromDates = Convert.ToString(txtFromDateOD.Text).Trim();
            toDates = Convert.ToString(txtToDateOD.Text).Trim();
            if (fromDates.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDates.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDates);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (toDates.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDates.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDates);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            string qryDates = string.Empty;
            if (dtFromDates > dtToDates)
            {
                lblAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                qryDates = " and (convert(datetime,od.fromdate,105) >= '" + dtFromDates.ToString("MM/dd/yyyy") + "' or  convert(datetime,od.Todate,105)>='" + dtFromDates.ToString("MM/dd/yyyy") + "') and  (convert(datetime,od.fromdate,105) <='" + dtToDates.ToString("MM/dd/yyyy") + "' or convert(datetime,od.Todate,105)<= '" + dtToDates.ToString("MM/dd/yyyy") + "')";
            }
            qrys = "select distinct r.college_code,r.Current_Semester,r.Batch_Year,r.roll_no,r.reg_no,r.Roll_Admit,r.stud_name,od.purpose,convert(varchar, od.fromdate, 103) as fromdate,convert(varchar, od.todate, 103)  as todate,convert(varchar, od.outtime, 108) as outtime,convert(varchar, od.intime, 108) as intime,od.attnd_type,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,od.no_of_hourse,od.hourse,r.degree_code,r.sections,r.app_no from registration r,onduty_stud od where od.roll_no=r.roll_no  " + qryCollegeCodes + qryBatchYears + qryDegreeCodes + qrySemesters + qrySections + qryDates;
            dsODStudentDetailss.Clear();
            dsODStudentDetailss = da.select_method_wo_parameter(qrys, "text");
            string error = "";
            string errors = "";
            for (int res = 1; res < gviewstudetails.Rows.Count; res++)
            {
                dtgridod.Reset();
                CheckBox chck = (gviewstudetails.Rows[res].FindControl("check") as CheckBox);
                if (chck.Checked)
                {
                    string frm1 = string.Empty;
                    string tod1 = string.Empty;
                    string rollno = (gviewstudetails.Rows[res].FindControl("lblroll") as Label).Text;
                    dsODStudentDetailss.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataTable dtisRedo = dsODStudentDetailss.Tables[0].DefaultView.ToTable();
                    Boolean cnfrm = false;
                    string hourcun = string.Empty;
                lable2:
                    if (!cnfrm)
                    {
                        int hrcount = 0;

                        hrcount = 0;
                        if (hrcount == 0)
                        {
                            string batchval = (gviewstudetails.Rows[res].FindControl("lblsnotag") as Label).Text;
                            string degree = (gviewstudetails.Rows[res].FindControl("lblrolltag") as Label).Text;
                            string semester = (gviewstudetails.Rows[res].FindControl("lblsem") as Label).Text;
                            string section = (gviewstudetails.Rows[res].FindControl("lblregtag") as Label).Text;
                            string collegeCode = (gviewstudetails.Rows[res].FindControl("lblsnonote") as Label).Text;
                            string ODCount = (gviewstudetails.Rows[res].FindControl("lblodcon") as Label).Text;

                            int TakenOd = 0;
                            if (ODCount.Trim() != "0" && ODCount.Trim() != "")
                            {
                                int.TryParse(ODCount, out TakenOd);
                            }
                            int MaxODCount = 0;
                            int.TryParse(Convert.ToString(ViewState["ODCont"]), out MaxODCount);
                            ht.Clear();
                            ht.Add("degree_code", int.Parse(degree.ToString()));
                            ht.Add("sem", int.Parse(semester));
                            ht.Add("from_date", fromDate.ToString());
                            ht.Add("to_date", toDate.ToString());
                            ht.Add("coll_code", int.Parse(collegeCode));
                            int iscount = 0;
                            DataSet ds2 = new DataSet();
                            qry = "select isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and degree_code=" + degree + " and semester=" + semester.ToString() + "";
                            ds2.Reset();
                            ds2.Dispose();
                            ds2 = da.select_method(qry, ht, "Text");
                            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                iscount = Convert.ToInt16(ds2.Tables[0].Rows[0]["cnt"].ToString());
                            }
                            ht.Add("iscount", iscount);
                            DataSet ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");
                            isSchoolAttendance = false;
                            isSchoolAttendance = CheckSchoolOrCollege(collegeCode);
                            Hashtable holiday_table = new Hashtable();
                            holiday_table.Clear();
                            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                                {
                                    if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                                    {
                                        holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                                    }
                                }
                            }
                            int fhrs = 0;
                            string hrs = da.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degree.ToString() + " and semester='" + semester.ToString() + "'");
                            DataSet dsval = new DataSet();
                            DataView dvval = new DataView();
                            int noMaxHrsDay = 0;
                            int noFstHrsDay = 0;
                            int noSndHrsDay = 0;
                            int noMinFstHrsDay = 0;
                            int noMinSndHrsDay = 0;
                            string selQ = " select CONVERT(varchar(50), start_date,105) as start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,p.no_of_hrs_I_half_day,p.no_of_hrs_II_half_day,s.batch_year,s.degree_code,s.semester,p.min_pres_I_half_day,p.min_pres_II_half_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=" + degree.ToString() + " and p.semester='" + semester.ToString() + "'";
                            dsval.Clear();
                            dsval = da.select_method_wo_parameter(selQ, "Text");
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {

                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["No_of_hrs_per_day"]), out noMaxHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_I_half_day"]), out noFstHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_II_half_day"]), out noSndHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_I_half_day"]), out noMinFstHrsDay);
                                int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_II_half_day"]), out noMinSndHrsDay);
                            }

                            dtgridod.Columns.Add("Date");
                            for (int col = 1; col <= noMaxHrsDay; col++)
                            {
                                dtgridod.Columns.Add(Convert.ToString(col));
                            }
                            for (int dtgview = 1; dtgview < gviewOD.Rows.Count; dtgview++)
                            {
                                drgridod = dtgridod.NewRow();
                                for (int dtcell = 0; dtcell < gviewOD.Columns.Count; dtcell++)
                                {
                                    if (gviewOD.Columns[dtcell].Visible == true)
                                    {
                                        if (dtcell == 0)
                                        {
                                            drgridod[dtcell] = (gviewOD.Rows[dtgview].FindControl("lblODdate") as Label).Text;
                                        }
                                        else
                                        {
                                            CheckBox chkbx = (gviewOD.Rows[dtgview].FindControl("chkhour" + dtcell) as CheckBox);
                                            if (chkbx.Checked)
                                            {
                                                drgridod[dtcell] = "True";
                                            }
                                            else
                                            {
                                                drgridod[dtcell] = "False";
                                            }
                                        }
                                    }
                                }
                                dtgridod.Rows.Add(drgridod);
                            }

                            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
                            {
                                fhrs = Convert.ToInt32(hrs);
                            }
                            bool leaveflag = false;
                            string strsec = string.Empty;
                            if (ddlSecOD.Items.Count > 0)
                            {
                                if (ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "all" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "-1")
                                {
                                    strsec = " and sm.sections='" + ddlSecOD.SelectedValue.ToString() + "'";
                                }
                                else
                                {
                                    strsec = string.Empty;
                                }
                            }
                            string strspecdeiatlqury = "select sm.date,sd.hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and sm.batch_year='" + batchval.ToString() + "' and sm.date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and sm.degree_code=" + degree.ToString() + " and sm.semester='" + semester.ToString() + "' " + strsec + "";
                            DataSet dsspecial = da.select_method_wo_parameter(strspecdeiatlqury, "Text");
                            flag = 1;
                            string appNo = (gviewstudetails.Rows[res].FindControl("lblsemtag") as Label).Text;
                            string stdRollno = (gviewstudetails.Rows[res].FindControl("lblrollvalue") as Label).Text;
                            string stdregno = (gviewstudetails.Rows[res].FindControl("lblreg") as Label).Text;
                            string stdname = (gviewstudetails.Rows[res].FindControl("lblstunme") as Label).Text;
                            string stdsem = (gviewstudetails.Rows[res].FindControl("lblsem") as Label).Text;
                            string AdmitDate = string.Empty;
                            int taken_hourse = 0;
                            int monthyear = 0;
                            string txtnoOfhours = string.Empty;
                            if (txtFromDateOD.Text != "")//&& txttodate.Text != "")
                            {
                                if (gviewOD.Visible == true)//if (txtNoOfHours.Text != "" || rbFullDay.Checked == true)
                                {
                                    if (!string.IsNullOrEmpty(finalhour))
                                    {
                                        txtnoOfhours = finalhour;
                                    }
                                    else
                                    {
                                        txtnoOfhours = txtNoOfHours.Text;
                                    }
                                    fromDate = txtFromDateOD.Text;
                                    toDate = txtToDateOD.Text;
                                    dt = fromDate;
                                    dsplit = dt.Split(new Char[] { '/' });
                                    fromDate = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();//day
                                    demfcal = int.Parse(dsplit[2].ToString());
                                    demfcal = demfcal * 12;
                                    cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                    monthcal = cal_from_date.ToString();
                                    dt = toDate;
                                    dsplit = dt.Split(new Char[] { '/' });
                                    toDate = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();//day
                                    demtcal = int.Parse(dsplit[2].ToString());
                                    demtcal = demfcal * 12;
                                    cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                                    per_from_date = Convert.ToDateTime(fromDate);
                                    per_to_date = Convert.ToDateTime(toDate);
                                    dumm_from_date = per_from_date;
                                    int totnoofhours = 0;
                                    int totnoofhours1 = 0;
                                    string[] hourslimit = txtnoOfhours.Split(new char[] { ',' });//txtNoOfHours.Text.Split(new char[] { ',' });
                                    string[] hourslimit1 = txtnoOfhours.Split(new char[] { ',' });
                                    totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                    int intNoOfOdCount = 0;
                                    int totalnoofodcount = Convert.ToInt32(ODCount);
                                    string selectddl_value = string.Empty;
                                    selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                    Attvalue = GetAttendanceStatusCode(selectddl_value);
                                    ArrayList DateHAsh = new ArrayList();
                                    if (Attvalue.Trim() == "3")
                                    {
                                        if (MaxODCount != 0)
                                        {
                                            if (dumm_from_date <= per_to_date)
                                            {
                                                while (dumm_from_date <= per_to_date)
                                                {
                                                    if (!holiday_table.ContainsKey(dumm_from_date) && dumm_from_date.ToString("dddd") != "Sunday")
                                                    {
                                                        intNoOfOdCount += totnoofhours;
                                                        totalnoofodcount += totnoofhours; //added by Mullai
                                                        DateHAsh.Add(dumm_from_date);
                                                    }
                                                    dumm_from_date = dumm_from_date.AddDays(1);
                                                }
                                                double TakenCont = 0;
                                                AttendancePercentage(collegeCode, batchval, degree, semester, stdRollno, AdmitDate, ref TakenCont, DateHAsh);
                                                intNoOfOdCount += Convert.ToInt32(TakenCont);
                                                if (MaxODCount < intNoOfOdCount)
                                                {

                                                }
                                            }
                                        }
                                    }
                                    odckeck = false;
                                    dumm_from_date = per_from_date;
                                    Dictionary<string, StringBuilder[]> dicQueryValue = new Dictionary<string, StringBuilder[]>();
                                    Dictionary<string, StringBuilder[]> dicQueryValue1 = new Dictionary<string, StringBuilder[]>();
                                    //added by Mullai
                                    string alterval = da.GetFunction("select linkValue from inssettings where linkName='OD Limit Exceeds' and College_code ='" + ddlCollege.SelectedValue + "'");

                                    if (alterval == "1" || alterval.ToLower() == "true")
                                    {
                                        if (totalnoofodcount <= MaxODCount)
                                        {
                                            odckeck = true;
                                        }
                                        else
                                            odckeck = false;
                                    }
                                    else
                                    {
                                        odckeck = true;
                                    }
                                    string odCount = string.Empty;
                                    string odCount1 = string.Empty;
                                    if (odckeck)
                                    {
                                        if (dumm_from_date <= per_to_date)
                                        {

                                            string selec = "delete From Onduty_Stud where Fromdate='" + fromDate + "' and todate='" + toDate + "' and Roll_no='" + stdRollno + "' and day is null";
                                            int del = da.update_method_wo_parameter(selec, "Text");

                                            string attenOdqry = "select Roll_no,Semester,Purpose,Fromdate,Outtime,Intime,Todate,college_code,attnd_type,no_of_hourse,hourse,ISNULL(Day,0)[Day] from Onduty_Stud od where od.Roll_no='" + stdRollno + "' and (convert(datetime,od.fromdate,105) >= '" + fromDate + "' or  convert(datetime,od.Todate,105)>='" + fromDate + "') and  (convert(datetime,od.fromdate,105) <='" + toDate + "' or convert(datetime,od.Todate,105)<= '" + toDate + "')";
                                            DataSet ds = da.select_method_wo_parameter(attenOdqry, "Text");
                                            DataView odview = new DataView();
                                            DataView dtview = new DataView();
                                            for (int OdDte = 1; OdDte < gviewOD.Rows.Count; OdDte++)
                                            {

                                                string OdDate = (gviewOD.Rows[OdDte].FindControl("lblODdate") as Label).Text;
                                                string[] sploddate = OdDate.Split('/');
                                                string OdDatecrct = sploddate[1] + "/" + sploddate[0] + "/" + sploddate[2];
                                                DateTime from_date = Convert.ToDateTime(OdDatecrct);
                                                string[] splt = Convert.ToString(from_date).Split(' ');
                                                string[] spl = Convert.ToString(splt[0]).Split('/');

                                                //if (OdDate == spl[1] + "/" + spl[0] + "/" + spl[2])01/01/1990
                                                //{
                                                odCount = string.Empty;
                                                odCount1 = string.Empty;

                                                ds.Tables[0].DefaultView.RowFilter = "(day='" + OdDatecrct + "' or day='01/01/1900') and Roll_no='" + stdRollno + "'";
                                                odview = ds.Tables[0].DefaultView;
                                                if (odview.Count > 0)
                                                {
                                                    hours = Convert.ToString(odview[0]["hourse"]);
                                                    spl1 = hours.Split(',');
                                                }
                                                Hashtable hatval = new Hashtable();
                                                dtview = new DataView(dtgridod);
                                                dtview.RowFilter = "Date='" + OdDate + "'";
                                                string strval = string.Empty;
                                                for (int chkbox = 1; chkbox < dtgridod.Columns.Count; chkbox++)
                                                {
                                                    if (hours.Contains(dtgridod.Columns[chkbox].ToString()))
                                                    {
                                                        if (dtview[0][chkbox] == "True")
                                                        {
                                                            if (odCount == string.Empty)
                                                            {

                                                                odCount = dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            else
                                                            {
                                                                odCount = odCount + "," + dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            hatval.Add(dtgridod.Columns[chkbox].ToString(), "true");
                                                        }
                                                        else
                                                        {
                                                            if (odCount == string.Empty)
                                                            {
                                                                odCount = dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            else
                                                            {
                                                                odCount = odCount + "," + dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            hatval.Add(dtgridod.Columns[chkbox].ToString(), "false");
                                                        }
                                                        if (dtview[0][chkbox] == "True")
                                                        {
                                                            if (odCount1 == string.Empty)
                                                            {
                                                                odCount1 = dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            else
                                                            {
                                                                odCount1 = odCount1 + "," + dtgridod.Columns[chkbox].ToString();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dtview[0][chkbox] == "True")
                                                        {
                                                            if (odCount == string.Empty)
                                                            {
                                                                odCount = dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            else
                                                            {
                                                                odCount = odCount + "," + dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            hatval.Add(dtgridod.Columns[chkbox].ToString(), "true");
                                                            if (odCount1 == string.Empty)
                                                            {
                                                                odCount1 = dtgridod.Columns[chkbox].ToString();
                                                            }
                                                            else
                                                            {
                                                                odCount1 = odCount1 + "," + dtgridod.Columns[chkbox].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                                hourslimit = odCount.Split(new char[] { ',' });
                                                hourslimit1 = odCount1.Split(new char[] { ',' });
                                                totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                                totnoofhours1 = Convert.ToInt32(hourslimit1.GetUpperBound(0).ToString()) + 1;

                                                StringBuilder sbQueryUpdate = new StringBuilder();
                                                StringBuilder sbQUeryInsertValue = new StringBuilder();
                                                StringBuilder sbQueryColumnName = new StringBuilder();
                                                StringBuilder sbQueryUpdateVal = new StringBuilder();
                                                string[] odspl = odCount.Split(',');
                                                if (!holiday_table.ContainsKey(from_date))
                                                {
                                                    string dummfromdate = Convert.ToString(dumm_from_date);
                                                    string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                                    string fromdate2 = fromdate1[0].ToString();
                                                    string[] fromdate = OdDate.Split(new char[] { '/' }); //fromdate2.Split(new char[] { '/' });
                                                    string fromdatedate = fromdate[0].ToString(); //fromdate[1].ToString();
                                                    if (fromdatedate[0].ToString() == "0")
                                                        fromdatedate = fromdatedate[1].ToString();

                                                    string fromdatemonth = fromdate[1].ToString();//fromdate[0].ToString();
                                                    string fromdateyear = fromdate[2].ToString();
                                                    monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                                    string valueupdate = string.Empty;
                                                    string insertvalue = string.Empty;
                                                    string updateVal = string.Empty;
                                                    string odvalue = string.Empty;
                                                    taken_hourse = taken_hourse + totnoofhours1;
                                                    for (int i = 0; i < Convert.ToInt32(totnoofhours); i++)
                                                    {
                                                        string particularhrs = odspl[i]; //hourslimit[i].ToString();
                                                        string value = ("d" + fromdatedate + "d" + particularhrs);
                                                        selectddl_value = string.Empty;
                                                        if (hatval.ContainsKey(particularhrs) && hatval[particularhrs] == "true")
                                                        {
                                                            if (ddlAttendanceOption.Items.Count > 0)
                                                            {
                                                                selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                                Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                                if (valueupdate == "")
                                                                {
                                                                    valueupdate = value + "=" + Attvalue;
                                                                }
                                                                else
                                                                {
                                                                    valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                                }
                                                                if (insertvalue == "")
                                                                {
                                                                    insertvalue = value;
                                                                }
                                                                else
                                                                {
                                                                    insertvalue = insertvalue + "," + value;
                                                                }
                                                                if (odvalue == "")
                                                                {
                                                                    odvalue = Attvalue;
                                                                }
                                                                else
                                                                {
                                                                    odvalue = odvalue + "," + Attvalue;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ddlAttendanceOption.Items.Count > 0)
                                                            {
                                                                selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                                Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                                if (valueupdate == "")
                                                                {
                                                                    valueupdate = value + "=Null";
                                                                }
                                                                else
                                                                {
                                                                    valueupdate = valueupdate + "," + value + "=Null";
                                                                }
                                                                if (updateVal == "")
                                                                {
                                                                    updateVal = value;
                                                                }
                                                                else
                                                                {
                                                                    updateVal = updateVal + "," + value;
                                                                }
                                                                if (insertvalue == "")
                                                                {
                                                                    insertvalue = value;
                                                                }
                                                                else
                                                                {
                                                                    insertvalue = insertvalue + "," + value;
                                                                }
                                                                if (odvalue == "")
                                                                {
                                                                    odvalue = "Null";
                                                                }
                                                                else
                                                                {
                                                                    odvalue = odvalue + ",Null";
                                                                }
                                                            }
                                                        }
                                                        if (ddlAttendanceOption.Items.Count > 0)
                                                        {
                                                        }
                                                        else
                                                        {
                                                            lblPopODErr.Visible = true;
                                                            lblPopODErr.Text = "There No Right(s) To This User";
                                                            return;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(insertvalue))
                                                    {
                                                        sbQueryColumnName.Append(insertvalue + ",");
                                                    }
                                                    if (!string.IsNullOrEmpty(odvalue))
                                                    {
                                                        sbQUeryInsertValue.Append(odvalue + ",");
                                                    }
                                                    if (!string.IsNullOrEmpty(valueupdate))
                                                    {
                                                        sbQueryUpdate.Append(valueupdate + ",");
                                                    }
                                                    if (!string.IsNullOrEmpty(updateVal))
                                                    {
                                                        sbQueryUpdateVal.Append(updateVal + ",");
                                                    }
                                                    save_flag = true;
                                                }
                                                else
                                                {
                                                    int starthout = 0;
                                                    string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + from_date.ToString("MM/dd/yyyy") + "'";
                                                    DataSet dsholidayval = da.select_method_wo_parameter(strholyquery, "Text");
                                                    if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                                                    {
                                                        string sethours = string.Empty;
                                                        string[] sphrsp = odCount.Split(','); //txtnoOfhours.Split(',');// txtNoOfHours.Text.Split(',');
                                                        for (int sph = 0; sph <= sphrsp.GetUpperBound(0); sph++)
                                                        {
                                                            int sehrou = Convert.ToInt32(sphrsp[sph]);
                                                            if (sehrou <= fhrs)
                                                            {
                                                                if (dsholidayval.Tables[0].Rows[0]["morning"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                {
                                                                }
                                                                else
                                                                {
                                                                    taken_hourse = taken_hourse + 1;
                                                                    if (sethours == "")
                                                                    {
                                                                        sethours = sehrou.ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        sethours = sethours + ',' + sehrou.ToString();
                                                                    }
                                                                }
                                                            }
                                                            if (sehrou > fhrs)
                                                            {
                                                                if (dsholidayval.Tables[0].Rows[0]["evening"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                {
                                                                }
                                                                else
                                                                {
                                                                    taken_hourse = taken_hourse + 1;
                                                                    if (sethours == "")
                                                                    {
                                                                        sethours = sehrou.ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        sethours = sethours + ',' + sehrou.ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (sethours != "")
                                                        {
                                                            totnoofhours = 0;
                                                            hourslimit = sethours.Split(new char[] { ',' });
                                                            totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                                            string dummfromdate = Convert.ToString(dumm_from_date);
                                                            string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                                            string fromdate2 = fromdate1[0].ToString();
                                                            string[] fromdate = OdDate.Split(new char[] { '/' }); //fromdate2.Split(new char[] { '/' });

                                                            string fromdatedate = fromdate[0].ToString();//fromdate[1].ToString();
                                                            if (fromdatedate[0].ToString() == "0")
                                                                fromdatedate = fromdatedate[1].ToString();
                                                            string fromdatemonth = fromdate[1].ToString(); //fromdate[0].ToString();
                                                            string fromdateyear = fromdate[2].ToString();
                                                            monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                                            string valueupdate = string.Empty;
                                                            string insertvalue = string.Empty;
                                                            string updateVal = string.Empty;
                                                            string odvalue = string.Empty;
                                                            for (int i = starthout; i < Convert.ToInt32(totnoofhours); i++)
                                                            {
                                                                string particularhrs = hourslimit[i].ToString();
                                                                string value = ("d" + fromdatedate + "d" + particularhrs);
                                                                selectddl_value = string.Empty;
                                                                if (hatval.ContainsKey(particularhrs) && hatval[particularhrs] == "true")
                                                                {
                                                                    if (ddlAttendanceOption.Items.Count > 0)
                                                                    {
                                                                        selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                                        Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                                        if (valueupdate == "")
                                                                        {
                                                                            valueupdate = value + "=" + Attvalue;
                                                                        }
                                                                        else
                                                                        {
                                                                            valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                                        }
                                                                        if (insertvalue == "")
                                                                        {
                                                                            insertvalue = value;
                                                                        }
                                                                        else
                                                                        {
                                                                            insertvalue = insertvalue + "," + value;
                                                                        }
                                                                        if (odvalue == "")
                                                                        {
                                                                            odvalue = Attvalue;
                                                                        }
                                                                        else
                                                                        {
                                                                            odvalue = odvalue + "," + Attvalue;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ddlAttendanceOption.Items.Count > 0)
                                                                    {
                                                                        selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                                        Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                                        if (valueupdate == "")
                                                                        {
                                                                            valueupdate = value + "=Null";
                                                                        }
                                                                        else
                                                                        {
                                                                            valueupdate = valueupdate + "," + value + "=Null";
                                                                        }
                                                                        if (updateVal == "")
                                                                        {
                                                                            updateVal = value;
                                                                        }
                                                                        else
                                                                        {
                                                                            updateVal = updateVal + "," + value;
                                                                        }
                                                                    }
                                                                }
                                                                if (ddlAttendanceOption.Items.Count > 0)
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    lblPopODErr.Visible = true;
                                                                    lblPopODErr.Text = "There No Right(s) To This User";
                                                                    return;
                                                                }

                                                            }
                                                            if (!string.IsNullOrEmpty(insertvalue))
                                                            {
                                                                sbQueryColumnName.Append(insertvalue + ",");
                                                            }
                                                            if (!string.IsNullOrEmpty(odvalue))
                                                            {
                                                                sbQUeryInsertValue.Append(odvalue + ",");
                                                            }
                                                            if (!string.IsNullOrEmpty(valueupdate))
                                                            {
                                                                sbQueryUpdate.Append(valueupdate + ",");
                                                            }
                                                            if (!string.IsNullOrEmpty(updateVal))
                                                            {
                                                                sbQueryUpdateVal.Append(updateVal + ",");
                                                            }

                                                            save_flag = true;
                                                        }
                                                        if (leaveflag == false && sethours == "")
                                                        {
                                                            if (!holiDateCheck.ContainsKey(dumm_from_date))
                                                            {
                                                                holiDateCheck.Add(dumm_from_date, "");
                                                                if (strholiday == "")
                                                                {
                                                                    strholiday = "Holiday(s) are : " + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                                }
                                                                else
                                                                {
                                                                    strholiday = strholiday + "," + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (leaveflag == false)
                                                        {
                                                            if (!holiDateCheck.ContainsKey(from_date))
                                                            {
                                                                holiDateCheck.Add(from_date, "");
                                                                if (strholiday == "")
                                                                {
                                                                    strholiday = "Holiday(s) are : " + from_date.ToString("dd/MM/yyyy");
                                                                }
                                                                else
                                                                {
                                                                    strholiday = strholiday + "," + from_date.ToString("dd/MM/yyyy");
                                                                }
                                                            }


                                                        }
                                                    }
                                                }
                                                StringBuilder[] sbAll = new StringBuilder[4];
                                                if (!string.IsNullOrEmpty(sbQueryColumnName.ToString().Trim()) && !string.IsNullOrEmpty(sbQUeryInsertValue.ToString().Trim()) && !string.IsNullOrEmpty(sbQueryUpdate.ToString().Trim()))
                                                {
                                                    if (dicQueryValue.ContainsKey(monthyear.ToString().Trim()))
                                                    {
                                                        sbAll = dicQueryValue[monthyear.ToString().Trim()];
                                                        sbAll[0].Append(sbQueryColumnName);
                                                        sbAll[1].Append(sbQUeryInsertValue);
                                                        sbAll[2].Append(sbQueryUpdate);
                                                        sbAll[3].Append(sbQueryUpdateVal);
                                                        dicQueryValue[monthyear.ToString().Trim()] = sbAll;
                                                    }
                                                    else if (monthyear != 0)
                                                    {
                                                        sbAll[0] = new StringBuilder();//day
                                                        sbAll[1] = new StringBuilder();
                                                        sbAll[2] = new StringBuilder();
                                                        sbAll[3] = new StringBuilder();
                                                        sbAll[0].Append(Convert.ToString(sbQueryColumnName));
                                                        sbAll[1].Append(Convert.ToString(sbQUeryInsertValue));
                                                        sbAll[2].Append(Convert.ToString(sbQueryUpdate));
                                                        sbAll[3].Append(Convert.ToString(sbQueryUpdateVal));
                                                        dicQueryValue.Add(monthyear.ToString().Trim(), sbAll);
                                                    }
                                                }
                                                StringBuilder[] sbAllval = new StringBuilder[1];
                                                if (!string.IsNullOrEmpty(sbQueryUpdateVal.ToString().Trim()))
                                                {
                                                    if (dicQueryValue1.ContainsKey(monthyear.ToString().Trim()))
                                                    {
                                                        sbAllval = dicQueryValue1[monthyear.ToString().Trim()];
                                                        sbAllval[0].Append(sbQueryUpdateVal);
                                                        dicQueryValue1[monthyear.ToString().Trim()] = sbAllval;
                                                    }
                                                    else if (monthyear != 0)
                                                    {
                                                        sbAllval[0] = new StringBuilder();
                                                        sbAllval[0].Append(Convert.ToString(sbQueryUpdateVal));
                                                        dicQueryValue1.Add(monthyear.ToString().Trim(), sbAllval);
                                                    }
                                                }
                                                //====================Special Hour Attendance =======================
                                                dsspecial.Tables[0].DefaultView.RowFilter = "date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                                                DataView dvspecy = dsspecial.Tables[0].DefaultView;
                                                for (int sph = 0; sph < dvspecy.Count; sph++)
                                                {
                                                    string hrdepno = dvspecy[sph]["hrdet_no"].ToString();
                                                    string strspattval = " if exists (select * from specialhr_attendance where hrdet_no='" + hrdepno + "' and month_year='" + monthyear + "' and roll_no='" + stdRollno + "' and SpHr_App_no='" + appNo + "')";
                                                    strspattval = strspattval + " update specialhr_attendance set attendance='" + Attvalue + "',SpHr_CollegeCode='" + collegeCode + "'  where hrdet_no='" + hrdepno + "' and month_year='" + monthyear + "' and roll_no='" + stdRollno + "'  and SpHr_App_no='" + appNo + "'";
                                                    strspattval = strspattval + " else";
                                                    strspattval = strspattval + " insert into specialhr_attendance(SpHr_App_no,SpHr_CollegeCode,roll_no,hrdet_no,attendance,month_year) values('" + appNo + "','" + collegeCode + "','" + stdRollno + "','" + hrdepno + "','" + Attvalue + "','" + monthyear + "')";
                                                    savevalue = da.update_method_wo_parameter(strspattval, "Text");
                                                }

                                                string txtintime = ddlInTimeHr.SelectedValue.ToString() + ":" + ddlInTimeMM.SelectedValue.ToString() + ":00" + " " + ddlInTimeSess.SelectedValue.ToString();
                                                string txtouttime = ddlOutTimeHr.SelectedValue.ToString() + ":" + ddlOutTimeMM.SelectedValue.ToString() + ":00" + " " + ddlOutTimeSess.SelectedValue.ToString();


                                                ht.Clear();
                                                ht.Add("rollno", stdRollno);
                                                ht.Add("semester", stdsem);
                                                ht.Add("purpose", purpose.ToString());
                                                ht.Add("fromdate", Convert.ToDateTime(fromDate));
                                                ht.Add("todate", Convert.ToDateTime(toDate));
                                                ht.Add("outtime", Convert.ToDateTime(txtouttime));
                                                ht.Add("intime", Convert.ToDateTime(txtintime));
                                                ht.Add("college_code", collegeCode);
                                                ht.Add("attnd_type", Attvalue);
                                                ht.Add("no_of_hourse", taken_hourse);
                                                ht.Add("hourse", odCount1);
                                                ht.Add("day", OdDatecrct);
                                                string colgcode = collegeCode;
                                                if (odCount1 != "")
                                                {
                                                    string intqry = "if exists(select * from onduty_stud where roll_no='" + stdRollno + "' and semester='" + stdsem + "' and day='" + OdDatecrct + "' ) update onduty_stud set purpose='" + purpose.ToString() + "',college_code='" + collegeCode + "', attnd_type='" + Attvalue + "',no_of_hourse='" + taken_hourse + "', hourse='" + odCount1 + "' where Roll_no='" + stdRollno + "' and semester='" + stdsem + "' and day='" + OdDatecrct + "' Else insert into onduty_stud(roll_no,semester,purpose,fromdate,todate,outtime,intime,college_code, attnd_type,no_of_hourse, hourse,day) values('" + stdRollno + "','" + stdsem + "','" + purpose.ToString() + "','" + Convert.ToDateTime(fromDate) + "','" + Convert.ToDateTime(toDate) + "','" + Convert.ToDateTime(txtouttime) + "','" + Convert.ToDateTime(txtintime) + "','" + colgcode + "', '" + Attvalue + "','" + taken_hourse + "', '" + odCount1 + "','" + OdDatecrct + "') ";//fromdate='" + fromDate + "',todate='" + toDate + "'


                                                    //string intqry = "if exists(select * from onduty_stud where roll_no='" + stdRollno + "' and fromdate='" + fromDate + "' and day='" + OdDatecrct + "' ) update onduty_stud set roll_no='" + stdRollno + "',semester='" + stdsem + "',purpose='" + purpose.ToString() + "',fromdate='" + fromDate + "',todate='" + toDate + "', outtime='" + Convert.ToDateTime(txtouttime) + "',intime='" + Convert.ToDateTime(txtintime) + "',college_code='" + collegeCode + "', attnd_type='" + Attvalue + "',no_of_hourse='" + taken_hourse + "', hourse='" + odCount1 + "' where Roll_no='" + stdRollno + "' and fromdate='" + fromDate + "' and day='" + OdDatecrct + "' Else insert into onduty_stud(roll_no,semester,purpose,fromdate,todate,outtime,intime,college_code, attnd_type,no_of_hourse, hourse,day) values('" + stdRollno + "','" + stdsem + "','" + purpose.ToString() + "','" + Convert.ToDateTime(fromDate) + "','" + Convert.ToDateTime(toDate) + "','" + Convert.ToDateTime(txtouttime) + "','" + Convert.ToDateTime(txtintime) + "','" + colgcode + "', '" + Attvalue + "','" + taken_hourse + "', '" + odCount1 + "','" + OdDatecrct + "') ";
                                                    cont = da.update_method_wo_parameter(intqry, "Text");
                                                }
                                            }
                                            if (cont != 0)
                                            {
                                                isSaveAttendance = true;
                                            }
                                            if (dicQueryValue.Count > 0 && !string.IsNullOrEmpty(appNo) && !string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(stdRollno))
                                            {
                                                StringBuilder[] spAll = new StringBuilder[4];
                                                foreach (KeyValuePair<string, StringBuilder[]> dicQueery in dicQueryValue)
                                                {
                                                    spAll = new StringBuilder[4];
                                                    string monthValue = dicQueery.Key;
                                                    spAll = dicQueery.Value;
                                                    string insertColumnName = spAll[0].ToString().Trim(',');
                                                    string insertColumnValue = spAll[1].ToString().Trim(',');
                                                    string updateColumnNameValue = spAll[2].ToString().Trim(',');
                                                    string Updatereason = spAll[3].ToString().Trim();
                                                    if (Attvalue.Trim() == "3")
                                                    {
                                                        string[] splitColumn = insertColumnName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (string sp in splitColumn)
                                                        {
                                                            ht.Clear();
                                                            ht.Add("AtWr_App_no", appNo);
                                                            ht.Add("AttWr_CollegeCode", collegeCode);
                                                            ht.Add("columnname", sp);
                                                            ht.Add("roll_no", stdRollno);
                                                            ht.Add("month_year", monthValue);
                                                            ht.Add("values", reason);
                                                            string strquery = "sp_ins_upd_student_attendance_reason";
                                                            int insert = da.insert_method(strquery, ht, "sp");
                                                            if (insert != 0)
                                                            {
                                                                isSaveAttendance = true;
                                                            }
                                                        }
                                                        string[] splitColumnUncheck = Updatereason.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                        foreach (string sp1 in splitColumnUncheck)
                                                        {
                                                            ht.Clear();
                                                            ht.Add("AtWr_App_no", appNo);
                                                            ht.Add("AttWr_CollegeCode", collegeCode);
                                                            ht.Add("columnname", sp1);
                                                            ht.Add("roll_no", stdRollno);
                                                            ht.Add("month_year", monthValue);
                                                            ht.Add("values", "Null");
                                                            string strquery = "sp_ins_upd_student_attendance_reason";
                                                            int insert = da.insert_method(strquery, ht, "sp");
                                                        }
                                                    }
                                                    ht.Clear();
                                                    ht.Add("Att_App_no", appNo);
                                                    ht.Add("Att_CollegeCode", collegeCode);
                                                    ht.Add("rollno", stdRollno);
                                                    ht.Add("monthyear", monthValue);
                                                    ht.Add("columnname", insertColumnName);
                                                    ht.Add("colvalues", insertColumnValue);
                                                    ht.Add("coulmnvalue", updateColumnNameValue);
                                                    savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");//Attendance Update,Insert 
                                                    if (savevalue != 0)
                                                    {
                                                        isSaveAttendance = true;
                                                    }
                                                }
                                                dumm_from_date = per_from_date;
                                                while (dumm_from_date <= per_to_date)
                                                {
                                                    StringBuilder sbQueryUpdate1 = new StringBuilder();
                                                    StringBuilder sbQUeryInsertValue1 = new StringBuilder();
                                                    StringBuilder sbQueryColumnName1 = new StringBuilder();
                                                    if (!holiday_table.ContainsKey(dumm_from_date))
                                                    {
                                                        int monthValue = Convert.ToInt32(dumm_from_date.Month.ToString().TrimStart('0')) + Convert.ToInt32(dumm_from_date.Year) * 12;
                                                        StringBuilder strPerDay = new StringBuilder();
                                                        string strPerDays = string.Empty;
                                                        bool hrcheck = false;
                                                        for (int hrcnt = 1; hrcnt <= noMaxHrsDay; hrcnt++)
                                                        {
                                                            strPerDay.Append("d" + dumm_from_date.Day.ToString().TrimStart('0') + "d" + hrcnt + ",");
                                                        }
                                                        if (strPerDay.Length > 0)
                                                        {
                                                            strPerDay.Remove(strPerDay.Length - 1, 1);
                                                            strPerDays = Convert.ToString(strPerDay);
                                                        }
                                                        if (save_flag == true && isSaveAttendance)
                                                        {
                                                            int attval = 0;
                                                            if (Attvalue == "")
                                                                attval = 0;
                                                            else
                                                                attval = Convert.ToInt32(Attvalue);
                                                            if (isSchoolAttendance)
                                                            {
                                                                attendanceMark(Convert.ToString(appNo), Convert.ToInt32(monthValue), strPerDays, noMaxHrsDay, noFstHrsDay, noSndHrsDay, noMinFstHrsDay, noMinSndHrsDay, Convert.ToString(dtFromDate.ToString("yyyy/MM/dd")), Convert.ToString(collegeCode), attval);
                                                            }
                                                        }
                                                    }
                                                    dumm_from_date = dumm_from_date.AddDays(1);
                                                }
                                            }
                                            if (setDefault)
                                            {
                                            }
                                            totnoofhours = 0;
                                        }
                                        else
                                        {
                                            lblPopODErr.Visible = true;
                                            lblPopODErr.Text = "From date should be less than Todate";
                                        }
                                    }
                                }
                                else
                                {
                                    lblPopODErr.Visible = true;
                                    lblPopODErr.Text = "Select Hours";
                                }
                            }
                            leaveflag = true;
                        }
                    }
                }
            }
            if (error != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('" + errors + "')", true);
            }
            if (flag == 0)
            {
                lblPopODErr.Visible = true;
                lblPopODErr.Text = "Select Students and Proceed";
            }
            string successMessage = string.Empty;
            if (save_flag == true)
            {
                if (strholiday != "")
                {
                    lblPopODErr.Visible = true;
                    lblPopODErr.Text = strholiday + " " + leavhlf;
                }
                if (setDefault)
                {
                    successMessage = "Saved ";
                    if (chkStudentWise.Checked == false)
                    {
                        SetDefaultODEntry();
                    }
                }
                else
                {
                    successMessage = "Updated ";
                }
                //if (!odckeck)
                //{
                if (chkStudentWise.Checked == true)
                {
                    if (gviewstudetails.Rows.Count > 0)
                    {
                        for (int i = 0; i < gviewstudetails.Rows.Count; i++)
                        {
                            CheckBox chck = (gviewstudetails.Rows[i].FindControl("check") as CheckBox);
                            chck.Checked = false;
                        }
                    }
                    if (gviewOD.Visible == true)
                    {
                        for (int i = 0; i < gviewOD.Rows.Count; i++)
                        {
                            for (int col = 1; col < gviewOD.Rows[i].Cells.Count; col++)
                            {
                                CheckBox chck = (gviewOD.Rows[i].FindControl("chkhour" + col) as CheckBox);
                                chck.Checked = false;
                            }
                        }
                    }
                }
                //}
                if (isSaveAttendance)
                {
                    lblODAlertMsg.Text = successMessage + " Successfully" + " " + ErrorMsg;
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    //ViewState["odtable"] = null;
                    //gviewOD.DataSource = null;
                    //gviewOD.DataBind();
                    if (strholiday != "") //added by Deepali on 2/4/2018
                    {
                        lblPopODErr.Visible = true;
                        lblPopODErr.Text = strholiday;
                    }
                    return;
                }
                else
                {
                    lblODAlertMsg.Text = "Not " + successMessage + " " + ErrorMsg;
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                if (setDefault)
                {
                    successMessage = "Saved ";
                    if (odckeck)
                    {
                        if (chkStudentWise.Checked == false)
                        {
                            SetDefaultODEntry();
                        }
                    }
                }
                else
                {
                    successMessage = "Updated ";
                }
                if (!odckeck)
                {
                    for (int i = 0; i < gviewstudetails.Rows.Count; i++)
                    {
                        CheckBox chck = (gviewstudetails.Rows[i].FindControl("check") as CheckBox);
                        chck.Checked = false;
                    }
                }
                if (strholiday != "")
                {
                    lblPopODErr.Visible = true;
                    lblPopODErr.Text = strholiday;
                }
                lblODAlertMsg.Text = "Not " + successMessage + " " + ErrorMsg;
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public bool AttendanceDayLock(DateTime seldate, int type = 0)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = true;
        string cudate = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime curdate = Convert.ToDateTime(cudate);
        //string batch = ddlbatch.SelectedValue.ToString();
        string degree = ddlBranchOD.SelectedValue.ToString();
        string sem = ddlSemOD.SelectedValue.ToString();
        ArrayList arrDegree = new ArrayList();
        ArrayList arrSem = new ArrayList();
        string degreecode = string.Empty;
        string semester = string.Empty;
        if (type != 0)
        {
            if (chkStudentWise.Checked)
            {
                //if (FpStudentDetails.Sheets[0].RowCount > 1)
                if (gviewstudetails.Rows.Count > 2)
                {
                    for (int row = 2; row < gviewstudetails.Rows.Count; row++)
                    {
                        //string degree1 = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                        //string sem1 = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();

                        string degree1 = (gviewstudetails.Rows[row].FindControl("lblrolltag") as Label).Text;
                        string sem1 = (gviewstudetails.Rows[row].FindControl("lblsem") as Label).Text;

                        if (!string.IsNullOrEmpty(sem1))
                        {
                            if (!arrSem.Contains(sem1.Trim()))
                            {
                                arrSem.Add(sem1.Trim());
                                if (string.IsNullOrEmpty(semester))
                                {
                                    semester = "'" + sem1 + "'";
                                }
                                else
                                {
                                    semester += ",'" + sem1 + "'";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(degree1))
                        {
                            if (!arrDegree.Contains(degree1.Trim()))
                            {
                                arrDegree.Add(degree1.Trim());
                                if (string.IsNullOrEmpty(degreecode))
                                {
                                    degreecode = "'" + degree1 + "'";
                                }
                                else
                                {
                                    degreecode += ",'" + degree1 + "'";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
                collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            }
        }
        else
        {
            degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
            semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
            collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
        }
        if (seldate.ToString("MM/dd/yyyy") == curdate.ToString("MM/dd/yyyy"))
        {
            return daycheck;
        }
        else
        {
            string lockdayvalue = "select LockODDays,LOD_Flag from collinfo where college_code='" + collegecode + "'";
            DataSet ds = new DataSet();
            ds = da.select_method_wo_parameter(lockdayvalue, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LOD_Flag"].ToString().Trim().ToLower() == "true" || ds.Tables[0].Rows[0]["LOD_Flag"].ToString().Trim() == "1")
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != null && int.Parse(ds.Tables[0].Rows[0][0].ToString()) >= 0)
                    {
                        int total = int.Parse(ds.Tables[0].Rows[0]["LockODDays"].ToString());
                        String strholidasquery = "select distinct holiday_date from holidaystudents where degree_code in(" + degreecode + ")  and semester in(" + semester + ") and holiday_date between '" + seldate.ToString("MM/dd/yyyy") + "' and '" + curdate.ToString("MM/dd/yyyy") + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = da.select_method_wo_parameter(strholidasquery, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            total = total + ds1.Tables[0].Rows.Count;
                        }
                        DateTime dt1 = seldate;
                        DateTime dt2 = curdate;
                        TimeSpan ts = dt2 - dt1;
                        int dif_days = ts.Days;
                        if (dif_days > total)
                        {
                            daycheck = false;
                        }
                    }
                }
            }
        }
        return daycheck;
    }

    private void ShowStudentsList(byte type = 0, string studentRollNo = null)
    {
        try
        {
            bool RightsFlag = true;
            lblPopODErr.Text = string.Empty;
            divPopODAlert.Visible = false;
            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;
            orderBy = string.Empty;
            orderBySetting = string.Empty;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;
            isValidDate = false;
            isValidFromDate = false;
            isValidToDate = false;
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            if (type != 0)
            {

            }
            orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
            orderBySetting = orderBySetting.Trim();
            orderBy = "ORDER BY rollNoLen,r.roll_no";
            switch (orderBySetting)
            {
                case "0":
                    orderBy = "ORDER BY rollNoLen,r.roll_no";
                    break;
                case "1":
                    orderBy = "ORDER BY regNoLen,r.Reg_No";
                    break;
                case "2":
                    orderBy = "ORDER BY r.Stud_Name";
                    break;
                case "0,1,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No,r.stud_name";
                    break;
                case "0,1":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No";
                    break;
                case "1,2":
                    orderBy = "ORDER BY regNoLen,r.Reg_No,r.Stud_Name";
                    break;
                case "0,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,r.Stud_Name";
                    break;
                default:
                    orderBy = "ORDER BY rollNoLen,r.roll_no";
                    break;
            }


            DataSet dsStudentDetails = new DataSet();
            DataSet dsDegreeDetails = new DataSet();
            if (type == 0)
            {
                if (ddlCollegeOD.Items.Count == 0)
                {
                    lblPopODErr.Text = "No " + lblCollegeOD.Text.Trim() + " Were Found";
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    return;
                }
                else
                {
                    collegeCode = string.Empty;
                    qryCollegeCode = string.Empty;
                    foreach (ListItem li in ddlCollegeOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(collegeCode))
                            {
                                collegeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                collegeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(collegeCode))
                    {
                        qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                    }
                }
                if (ddlBatchOD.Items.Count == 0)
                {
                    //-------------------------------comment and added by Deepali on 6.4.18
                    //lblPopODErr.Text = "No " + lblBatchOD.Text.Trim() + " Were Found";
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "No " + lblBatchOD.Text.Trim() + " Were Found";
                    lblODAlertMsg.Visible = true;
                    //------------------------------
                    return;
                }
                else
                {
                    batchYear = string.Empty;
                    qryBatchYear = string.Empty;
                    foreach (ListItem li in ddlBatchOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(batchYear))
                            {
                                batchYear = "'" + li.Text + "'";
                            }
                            else
                            {
                                batchYear += ",'" + li.Text + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(batchYear))
                    {
                        qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                    }
                }
                if (ddlDegreeOD.Items.Count == 0)
                {
                    lblPopODErr.Text = "No " + lblDegreeOD.Text.Trim() + " Were Found";
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    return;
                }
                else
                {
                    courseId = string.Empty;
                    qryCourseId = string.Empty;
                    foreach (ListItem li in ddlDegreeOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(courseId))
                            {
                                courseId = "'" + li.Value + "'";
                            }
                            else
                            {
                                courseId += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(courseId))
                    {
                        qryCourseId = " and c.Course_Id in(" + courseId + ")";
                    }
                }
                if (ddlBranchOD.Items.Count == 0)
                {
                    lblPopODErr.Text = "No " + lblBranchOD.Text.Trim() + " Were Found";
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    return;
                }
                else
                {
                    degreeCode = string.Empty;
                    qryDegreeCode = string.Empty;
                    foreach (ListItem li in ddlBranchOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(degreeCode))
                            {
                                degreeCode = "'" + li.Value + "'";
                            }
                            else
                            {
                                degreeCode += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(degreeCode))
                    {
                        qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    }
                }
                if (ddlSemOD.Items.Count == 0)
                {
                    lblPopODErr.Text = "No " + lblSemOD.Text.Trim() + " Were Found";
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    return;
                }
                else
                {
                    semester = string.Empty;
                    qrySemester = string.Empty;
                    foreach (ListItem li in ddlSemOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(semester))
                            {
                                semester = "'" + li.Value + "'";
                            }
                            else
                            {
                                semester += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qrySemester = " and r.current_semester in(" + semester + ")";
                    }
                }
                if (ddlSecOD.Items.Count > 0)
                {
                    section = string.Empty;
                    qrySection = string.Empty;
                    foreach (ListItem li in ddlSecOD.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(section))
                            {
                                section = "'" + li.Value + "'";
                            }
                            else
                            {
                                section += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(section))
                    {
                        qrySection = " and sections in(" + section + ")";
                    }
                }
                else
                {
                    section = string.Empty;
                    qrySection = string.Empty;
                }
                if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
                {
                    //qry = "select r.roll_no,r.college_code,r.reg_no,r.stud_name,r.current_semester,r.sections,r.Roll_Admit,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";
                    qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id,convert(varchar(10),r.adm_date,103) as adm_date from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";
                    dsStudentDetails = da.select_method_wo_parameter(qry, "text");
                }
                string GetODCount = da.GetFunction("select linkValue from inssettings where linkName='NoOfOdPerStudents' and College_code ='" + ddlCollegeOD.SelectedValue + "'");
                if (GetODCount.Trim() != "" && GetODCount.Trim() != "0")
                {
                    string[] SplitCount = GetODCount.Split(';');
                    if (SplitCount.Length > 1)
                    {
                        ViewState["ODCheck"] = Convert.ToString(SplitCount[0]);
                        ViewState["ODCont"] = Convert.ToString(SplitCount[1]);
                    }
                }
                else
                {
                    ViewState["ODCheck"] = "0";
                    ViewState["ODCont"] = "0";
                }
            }
            else if (type == 2)
            {
                if (!string.IsNullOrEmpty(studentRollNo))
                {
                    qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id,convert(varchar(10),r.adm_date,103) as adm_date from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and r.Roll_No='" + studentRollNo + "' and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)" + orderBy;
                    dsStudentDetails = da.select_method_wo_parameter(qry, "text");
                }
            }
            else if (type == 1)
            {

                string commonSelection = string.Empty;
                string rollNo = string.Empty;
                string appNo = string.Empty;
                string regNo = string.Empty;
                string admitNo = string.Empty;
                fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
                toDate = Convert.ToString(txtToDateOD.Text).Trim();
                commonSelection = Convert.ToString(txtStudent.Text).Trim();
                if (string.IsNullOrEmpty(commonSelection))
                {
                    lblPopODErr.Text = "Please Enter The " + lblStudentOptions.Text.Trim();
                    lblPopODErr.Visible = true;
                    divPopODAlert.Visible = false;
                    btnODPopAlertClose.Focus();
                    return;
                }

                collegeCode = string.Empty;
                qryCollegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }

                if (ddlSearchBy.Items.Count > 0)
                {
                    string selectedItems = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim().ToLower();
                    string selectedValue = Convert.ToString(ddlSearchBy.SelectedValue).Trim();
                    //switch (selectedItems)
                    //{
                    //    case "roll no":
                    //        rollNo = commonSelection.Trim();
                    //        break;
                    //    case "register no":
                    //        rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + rollNo + "'");
                    //        break;
                    //    case "admission no":
                    //        rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                    //        break;
                    //}
                    switch (selectedValue)
                    {
                        case "3":
                            rollNo = commonSelection.Trim();
                            break;
                        case "2":
                            rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + commonSelection + "'");
                            break;
                        case "1":
                            rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                            break;
                    }
                }
                else
                {
                    if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL") //jai
                    {
                        rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                    }
                    else
                    {
                        if (lblStudentOptions.Text.Trim().ToLower() == "register no")
                        {
                            rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + commonSelection + "'");
                        }
                        else if (lblStudentOptions.Text.Trim().ToLower() == "admission no")
                        {
                            rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                        }
                        else if (lblStudentOptions.Text.Trim().ToLower().Contains("student roll_no") || lblStudentOptions.Text.Trim().ToLower().Contains("roll no"))
                        {
                            rollNo = commonSelection;
                        }
                    }
                }

                for (int r = 1; r < gviewstudetails.Rows.Count; r++)
                {

                    if (gviewstudetails.Rows[r].Cells[1].Text.ToString().Trim().ToLower() == rollNo.Trim().ToLower())
                    {
                        txtStudent.Text = string.Empty;
                        divPopODAlert.Visible = true;
                        lblODAlertMsg.Text = "Already Exist the " + lblStudentOptions.Text.Trim() + ": " + commonSelection;
                        btnODPopAlertClose.Focus();
                        return;
                    }
                }
                if (fromDate.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                    isValidFromDate = isValidDate;
                    if (!isValidDate)
                    {
                        txtStudent.Text = string.Empty;
                        btnODPopAlertClose.Focus();
                        lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    txtStudent.Text = string.Empty;
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "Please Choose From Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                if (toDate.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                    isValidToDate = isValidDate;
                    if (!isValidDate)
                    {
                        txtStudent.Text = string.Empty;
                        btnODPopAlertClose.Focus();
                        lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    txtStudent.Text = string.Empty;
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "Please Choose To Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                string qryDate = string.Empty;
                if (dtFromDate > dtToDate)
                {
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                else
                {
                    qryDate = " and convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                }
                if (string.IsNullOrEmpty(rollNo) || rollNo == "0")
                {
                    lblODAlertMsg.Text = lblStudentOptions.Text.Trim() + " " + txtStudent.Text.Trim() + " is Not Available.";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    return;
                }
                string existroll = da.GetFunction("select Convert(nvarchar(15),r.fromdate,103)+' To '+ Convert(nvarchar(15),r.Todate,103) from Onduty_Stud r where Roll_no='" + rollNo + "' and (convert(datetime,r.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' or convert(datetime,r.Todate,105)  between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "')" + qryCollegeCode);
                if (existroll.Trim() != "" && existroll != "0")
                {
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "Already Student " + lblStudentOptions.Text + " " + commonSelection + " Exist in Od Entry at " + existroll;
                    divPopODAlert.Visible = true;
                    return;
                }
                //qry = "select r.roll_no,r.college_code,r.reg_no,r.stud_name,r.current_semester,r.sections,r.Roll_Admit,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";

                #region modified on 11/12/2017 User Rights based Reg or Roll No added by prabha

                string columnfield = string.Empty;
                string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]);
                }
                if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
                {
                    columnfield = " group_code='" + group_user + "'";
                }
                else if (Session["usercode"] != null)
                {
                    columnfield = " user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string user_code = Convert.ToString(Session["usercode"]).Trim();

                string degreerights = "select degree_code from DeptPrivilages where " + columnfield + " ";
                ArrayList aldegreesights = new ArrayList();
                DataTable dtuserrights = d2.selectDataTable(degreerights);
                if (dtuserrights.Rows.Count > 0)
                {
                    foreach (DataRow row in dtuserrights.Rows)
                    {
                        string Code = Convert.ToString(row["degree_code"]);
                        if (!aldegreesights.Contains(Code))
                            aldegreesights.Add(Code);
                    }
                }
                bool User_Rights_for_student = false;
                string EnteredVal = txtStudent.Text.Trim();
                string RegorRoll = (ddlSearchBy.SelectedItem.Text.ToLower() == "roll no") ? "roll" : "reg";
                string student_Degree_Code = string.Empty;
                string selectdegree = string.Empty;
                if (RegorRoll == "roll")
                    selectdegree = "select degree_code from Registration  where Roll_No='" + EnteredVal + "' and CC='0' and DelFlag='0' and Exam_Flag<>'debar'";
                else
                    selectdegree = "select degree_code from Registration where Reg_No='" + EnteredVal + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'debar'";
                student_Degree_Code = d2.selectScalarString(selectdegree);
                foreach (string degree in aldegreesights)
                {
                    if (degree == student_Degree_Code)
                        User_Rights_for_student = true;
                }

                #endregion

                qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id,convert(varchar(10),r.adm_date,103) as adm_date from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and r.Roll_No='" + rollNo + "' and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)" + qryCollegeCode + orderBy;

                if (User_Rights_for_student)  //modified on 11/12/2017
                    dsStudentDetails = da.select_method_wo_parameter(qry, "text");
                else
                {
                    RightsFlag = false;
                }

                string GetODCount = da.GetFunction("select linkValue from inssettings where linkName='NoOfOdPerStudents' and College_code ='" + ddlCollegeOD.SelectedValue + "'");
                if (GetODCount.Trim() != "" && GetODCount.Trim() != "0")
                {
                    string[] SplitCount = GetODCount.Split(';');
                    if (SplitCount.Length > 1)
                    {
                        ViewState["ODCheck"] = Convert.ToString(SplitCount[0]);
                        ViewState["ODCont"] = Convert.ToString(SplitCount[1]);
                    }
                }
                else
                {
                    ViewState["ODCheck"] = "0";
                    ViewState["ODCont"] = "0";
                }
            }
            if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("Batch");
                dt.Columns.Add("RollNo");
                dt.Columns.Add("DegreeCode");
                dt.Columns.Add("RegNo");
                dt.Columns.Add("Section");
                dt.Columns.Add("StudentName");
                dt.Columns.Add("Semester");
                dt.Columns.Add("AdmissionNo");
                dt.Columns.Add("AppNo");
                dt.Columns.Add("CollegeCode");
                dt.Columns.Add("courseId");
                dt.Columns.Add("adm_date");
                int serialNo = 0;
                if (type == 0)
                {
                    Init_Spread(1);

                    serialNo = 0;

                    attendenace(Convert.ToString(ddlBranchOD.SelectedValue).Trim(), Convert.ToString(ddlSemOD.SelectedValue).Trim());
                    if (rbFullDay.Checked == true)
                    {
                        rbFullDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHalfDay.Checked == true)
                    {
                        rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHourWise.Checked == true)
                    {
                        rbHourWise_CheckedChanged(new Object(), new EventArgs());
                    }
                }
                else if (type == 1)
                {
                    gviewstudetails.DataSource = null;
                    gviewstudetails.DataBind();
                    DataTable odtable = (DataTable)ViewState["odtable"];
                    if (odtable != null)
                    {
                        int con = odtable.Rows.Count;

                        if (odtable.Rows.Count > 1)
                        {
                            for (int row = 1; row < odtable.Rows.Count; row++)
                            {

                                string batchYearNew = odtable.Rows[row][1].ToString();
                                string collegeCodeNew = odtable.Rows[row][2].ToString();
                                string rollno = odtable.Rows[row][4].ToString();
                                string degreeCodeNew = odtable.Rows[row][5].ToString();
                                string regno = odtable.Rows[row][8].ToString();
                                string sectionNew = odtable.Rows[row][9].ToString();
                                string courseID = odtable.Rows[row][10].ToString();
                                string admissionno = odtable.Rows[row][12].ToString();
                                string studname = odtable.Rows[row][16].ToString();
                                string semesterNew = odtable.Rows[row][20].ToString();
                                string appNo = odtable.Rows[row][21].ToString();
                                string admit_date = odtable.Rows[row][6].ToString();

                                dr = dt.NewRow();
                                dr["Batch"] = batchYearNew;
                                dr["RollNo"] = rollno;
                                dr["RegNo"] = regno;
                                dr["DegreeCode"] = degreeCodeNew;
                                dr["Section"] = sectionNew;
                                dr["StudentName"] = studname;
                                dr["Semester"] = semesterNew;
                                dr["AdmissionNo"] = admissionno;
                                dr["AppNo"] = appNo;
                                dr["CollegeCode"] = collegeCodeNew;
                                dr["courseId"] = courseID;
                                dr["adm_date"] = admit_date;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        Init_Spread(1);

                        serialNo = 0;
                    }
                    Init_Spread(1);
                    serialNo = 0;

                    //for (int row = 0; row < dt.Rows.Count; row++)
                    foreach (DataRow drStudents in dt.Rows)
                    {

                        string batchYearNew = Convert.ToString(drStudents["Batch"]).Trim();
                        string rollno = Convert.ToString(drStudents["RollNo"]).Trim();
                        string degreeCodeNew = Convert.ToString(drStudents["DegreeCode"]).Trim();
                        string regno = Convert.ToString(drStudents["RegNo"]).Trim();
                        string sectionNew = Convert.ToString(drStudents["Section"]).Trim();
                        string studname = Convert.ToString(drStudents["StudentName"]).Trim();
                        string semesterNew = Convert.ToString(drStudents["Semester"]).Trim();
                        string admissionno = Convert.ToString(drStudents["AdmissionNo"]);
                        string appNo = Convert.ToString(drStudents["appNo"]).Trim();
                        string collegeCodeNew = Convert.ToString(drStudents["CollegeCode"]).Trim();
                        string courseID = Convert.ToString(drStudents["courseId"]).Trim();
                        string AdmitDate = Convert.ToString(drStudents["adm_date"]).Trim();
                        attendenace(Convert.ToString(degreeCodeNew).Trim(), Convert.ToString(semesterNew).Trim());
                        if (!hatroll.ContainsKey(rollno))
                        {
                            hatroll.Add(rollno, regno);
                            serialNo++;
                            if (rbFullDay.Checked == true)
                            {
                                rbFullDay_CheckedChanged(new Object(), new EventArgs());
                            }
                            else if (rbHalfDay.Checked == true)
                            {
                                rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                            }
                            else if (rbHourWise.Checked == true)
                            {
                                rbHourWise_CheckedChanged(new Object(), new EventArgs());
                            }
                            double OdCount = 0;
                            if (Convert.ToString(ViewState["ODCheck"]).Trim() == "1")
                            {
                                AttendancePercentage(collegeCodeNew, batchYearNew, degreeCodeNew, semesterNew, rollno, AdmitDate, ref OdCount);
                            }

                            drdetail = dtdetail.NewRow();
                            drdetail["sno"] = Convert.ToString(serialNo).Trim();
                            drdetail["snotag"] = Convert.ToString(batchYearNew).Trim();
                            drdetail["snonote"] = Convert.ToString(collegeCodeNew).Trim();
                            drdetail["Roll No"] = Convert.ToString(rollno).Trim();
                            drdetail["Rolltag"] = Convert.ToString(degreeCodeNew).Trim();
                            drdetail["Rollnote"] = Convert.ToString(AdmitDate).Trim();
                            drdetail["Rollvalue"] = Convert.ToString(rollno).Trim();
                            drdetail["Reg No"] = Convert.ToString(regno).Trim();
                            drdetail["Regtag"] = Convert.ToString(sectionNew).Trim();
                            drdetail["Regnote"] = Convert.ToString(courseID).Trim();
                            drdetail["Admission No"] = Convert.ToString(admissionno).Trim();
                            drdetail["Student Name"] = Convert.ToString(studname).Trim();
                            drdetail["Semester"] = Convert.ToString(semesterNew).Trim();
                            drdetail["Semestertag"] = Convert.ToString(appNo).Trim();
                            drdetail["OD Count"] = Convert.ToString(per_tot_ondu).Trim();
                            drdetail["ODtag"] = Convert.ToString(appNo).Trim();
                            dtdetail.Rows.Add(drdetail);
                            Hashtable hass = new Hashtable();
                            double TotalCount = 0;
                            double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount);
                            if (TotalCount != 0 && TotalCount <= per_tot_ondu)
                            {
                                hass.Add(rollno, per_tot_ondu);
                                //    FpStudentDetails.Rows[FpStudentDetails.Sheets[0].RowCount - 1].BackColor = Color.Tan;
                                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                            }
                        }
                        //else
                        //{
                        //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                        //}
                    }



                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();

                    ViewState["odtable"] = dtdetail;
                    gviewstudetails.DataSource = dtdetail;
                    gviewstudetails.DataBind();

                    gviewstudetails.Visible = true;

                    gviewstudetails.Columns[1].Visible = isRollVisible;
                    gviewstudetails.Columns[2].Visible = isRegVisible;
                    gviewstudetails.Columns[3].Visible = isAdmitNoVisible;

                    for (int row = 1; row < gviewstudetails.Rows.Count; row++)
                    {
                        double per_tot_ondu1 = Convert.ToDouble((gviewstudetails.Rows[row].FindControl("lblodcon") as Label).Text);
                        double TotalCount1 = 0;
                        double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount1);
                        if (TotalCount1 != 0 && TotalCount1 <= per_tot_ondu1)
                        {
                            gviewstudetails.Rows[row].BackColor = Color.Tan;

                        }
                        else
                        {
                            //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                        }
                    }

                    if (Convert.ToString(ViewState["ODCheck"]).Trim() != "0")
                    {
                        gviewstudetails.Columns[6].Visible = true;
                    }
                    else
                    {
                        gviewstudetails.Columns[6].Visible = false;
                    }
                }
                foreach (DataRow drStudents in dsStudentDetails.Tables[0].Rows)
                {
                    droddetails = dtoddetails.NewRow();

                    string rollNo = string.Empty;
                    string regNo = string.Empty;
                    string admitNo = string.Empty;
                    string appNo = string.Empty;
                    //string referDay = string.Empty;
                    string studentName = string.Empty;
                    string collegeCodeNew = string.Empty;
                    string batchYearNew = string.Empty;
                    string degreeCodeNew = string.Empty;
                    string currentSemester = string.Empty;
                    string sectionNew = string.Empty;
                    string courseId = string.Empty;
                   

                    rollNo = Convert.ToString(drStudents["roll_no"]).Trim();
                    regNo = Convert.ToString(drStudents["reg_no"]).Trim();
                    admitNo = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                    appNo = Convert.ToString(drStudents["app_no"]).Trim();
                    //referDay = Convert.ToString(drStudents[""]).Trim();
                    studentName = Convert.ToString(drStudents["stud_name"]).Trim();
                    collegeCodeNew = Convert.ToString(drStudents["college_code"]).Trim();
                    batchYearNew = Convert.ToString(drStudents["batch_year"]).Trim();
                    degreeCodeNew = Convert.ToString(drStudents["degree_code"]).Trim();
                    currentSemester = Convert.ToString(drStudents["current_semester"]).Trim();
                    sectionNew = Convert.ToString(drStudents["sections"]).Trim();
                    courseId = Convert.ToString(drStudents["Course_Id"]).Trim();
                    string AdmitDate = Convert.ToString(drStudents["adm_date"]).Trim();
                    attendenace(Convert.ToString(degreeCodeNew).Trim(), Convert.ToString(currentSemester).Trim());
                    if (!hatroll.ContainsKey(rollNo))
                    {
                        hatroll.Add(rollNo, regNo);
                        serialNo++;
                        if (rbFullDay.Checked == true)
                        {
                            rbFullDay_CheckedChanged(new Object(), new EventArgs());
                        }
                        else if (rbHalfDay.Checked == true)
                        {
                            rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                        }
                        else if (rbHourWise.Checked == true)
                        {
                            rbHourWise_CheckedChanged(new Object(), new EventArgs());
                        }
                        double Odcount = 0;
                        if (Convert.ToString(ViewState["ODCheck"]).Trim() == "1")
                        {
                            AttendancePercentage(collegeCodeNew, batchYearNew, degreeCodeNew, currentSemester, rollNo, AdmitDate, ref Odcount);
                        }



                        drdetail = dtdetail.NewRow();
                        drdetail["sno"] = Convert.ToString(serialNo).Trim();
                        drdetail["snotag"] = Convert.ToString(batchYearNew).Trim();
                        drdetail["snonote"] = Convert.ToString(collegeCodeNew).Trim();
                        drdetail["Roll No"] = Convert.ToString(rollNo).Trim();
                        drdetail["Rolltag"] = Convert.ToString(degreeCodeNew).Trim();
                        drdetail["Rollnote"] = Convert.ToString(AdmitDate).Trim();
                        drdetail["Rollvalue"] = Convert.ToString(rollNo).Trim();
                        drdetail["Reg No"] = Convert.ToString(regNo).Trim();
                        drdetail["Regtag"] = Convert.ToString(sectionNew).Trim();
                        drdetail["Regnote"] = Convert.ToString(courseId).Trim();
                        drdetail["Admission No"] = Convert.ToString(admitNo).Trim();
                        drdetail["Student Name"] = Convert.ToString(studentName).Trim();
                        drdetail["Semester"] = Convert.ToString(currentSemester).Trim();
                        drdetail["Semestertag"] = Convert.ToString(appNo).Trim();
                        drdetail["OD Count"] = Convert.ToString(per_tot_ondu).Trim();
                        drdetail["ODtag"] = Convert.ToString(appNo).Trim();
                        //drdetail[""] = Convert.ToString().Trim();
                        dtdetail.Rows.Add(drdetail);
                        //dtdetail.DefaultView.ToTable(true,'sno',"snotag");
                        double TotalCount = 0;
                        double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount);
                        if (TotalCount != 0 && TotalCount <= per_tot_ondu)
                        {
                            if (!hats.Contains(rollNo))
                                hats.Add(rollNo, per_tot_ondu);
                            //    FpStudentDetails.Rows[FpStudentDetails.Sheets[0].RowCount - 1].BackColor = Color.Tan;
                            //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                            //    //dtdetail.Rows[dtdetail.Rows.Count - 1][7] = false;
                        }
                    }
                    //else
                    //{
                    //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                    //    dtdetail.Rows[dtdetail.Rows.Count - 1][7] = false;
                    //}
                }

                gviewstudetails.DataSource = dtdetail;
                gviewstudetails.DataBind();
                gviewstudetails.Visible = true;
                //ViewState["odtable"] = dtdetail;

                for (int row = 1; row < gviewstudetails.Rows.Count; row++)
                {
                    double per_tot_ondu1 = Convert.ToDouble((gviewstudetails.Rows[row].FindControl("lblodcon") as Label).Text);
                    double TotalCount1 = 0;
                    double.TryParse(Convert.ToString(ViewState["ODCont"]), out TotalCount1);
                    if (TotalCount1 != 0 && TotalCount1 <= per_tot_ondu1)
                    {
                        gviewstudetails.Rows[row].BackColor = Color.Tan;

                    }
                    else
                    {
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 7].Locked = false;
                    }
                }

                for (int i = 0; i < gviewstudetails.Rows.Count; i++)
                {
                    string roll = (gviewstudetails.Rows[i].FindControl("lblroll") as Label).Text;
                    if (hats.Contains(roll))
                    {
                        gviewstudetails.Rows[i].BackColor = Color.Tan;
                    }
                }

                gviewstudetails.Columns[1].Visible = isRollVisible;
                gviewstudetails.Columns[2].Visible = isRegVisible;
                gviewstudetails.Columns[3].Visible = isAdmitNoVisible;

                if (Convert.ToString(ViewState["ODCheck"]).Trim() != "0")
                {
                    gviewstudetails.Columns[6].Visible = true;
                }
                else
                {
                    gviewstudetails.Columns[6].Visible = false;
                }
            }
            else
            {
                if (type != 1)
                {

                }
                if (type == 0)
                {
                    lblODAlertMsg.Text = "No Record(s) Were Found.";
                }
                else if (!RightsFlag)
                {
                    lblODAlertMsg.Text = lblStudentOptions.Text.Trim() + " " + txtStudent.Text.Trim() + " is Not Available.";
                    divPopODAlert.Visible = true;
                }
                else
                {
                    lblODAlertMsg.Text = lblStudentOptions.Text.Trim() + " " + txtStudent.Text.Trim() + " is Not Available.";
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                }
                gviewstudetails.Visible = false;
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "StudentOnDutyDetails");
        }
    }

    #region allstudentattendancereport new table

    protected void attendanceMark(string appNo, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode, int Attvalue)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appNo + "' ";
            dsload.Clear();
            dsload = da.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            EnullCnt++;
                    }
                }
                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, Attvalue, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, Attvalue, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select * from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = da.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, int Attvalue, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = 1;
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = Attvalue;
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    #endregion

    #region Reason

    protected void btnReasonDel_Click(object sender, EventArgs e)
    {
        try
        {
            lblReasonErr.Text = string.Empty;
            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
            if (ddlPurpose.Items.Count > 0)
            {
                string collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
                string reason = ddlPurpose.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    BindReason();
                }
            }
            divShowInFraction.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnReasonSet_Click(object sender, EventArgs e)
    {
        try
        {
            lblReasonErr.Text = string.Empty;
            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
            divShowInFraction.Visible = true;
            txtReason.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnAddReason_Click(object sender, EventArgs e)
    {
        try
        {
            lblODAlertMsg.Text = string.Empty;
            lblReasonErr.Text = string.Empty;
            divPopODAlert.Visible = false;
            divShowInFraction.Visible = true;
            string collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            string reason = txtReason.Text.ToString();
            if (reason.Trim() != "" && collegecode != "")
            {
                string strquery = "if not exists (select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code='" + collegecode + "' and TextVal='" + reason + "' ) insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason + "','Attrs','" + collegecode + "')";
                int a = da.update_method_wo_parameter(strquery, "Text");
                lblReasonErr.Text = "Reason Added Successfully";
                lblReasonErr.ForeColor = Color.Green;
                txtReason.Text = string.Empty;
                BindReason();
            }
            else
            {
                lblReasonErr.Text = "Please Enter The Reason And Then Proceed";
                lblReasonErr.ForeColor = Color.Red;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void btnExitReason_Click(object sender, EventArgs e)
    {
        try
        {
            lblReasonErr.Text = string.Empty;
            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
            divShowInFraction.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion

    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(lblCollege);
            lbl.Add(lblCollegeOD);
            lbl.Add(lbl_clgT);
            lbl.Add(lblDegree);
            lbl.Add(lblDegreeOD);
            lbl.Add(lblBranch);
            lbl.Add(lblBranchOD);
            lbl.Add(lblSem);
            lbl.Add(lblSemOD);
            fields.Add(0);
            fields.Add(0);
            fields.Add(0);
            fields.Add(2);
            fields.Add(2);
            fields.Add(3);
            fields.Add(3);
            fields.Add(4);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
                lblBatchOD.Text = "Year";
            }
            else
            {
                lblBatch.Text = "Batch";
                lblBatchOD.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                    dsSettings = da.select_method_wo_parameter(Master1, "Text");
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private DataSet GetSettings()
    {
        DataSet dsSettings = new DataSet();
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string groupCode = Convert.ToString(Session["group_code"]).Trim();
                string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                if (groupUser.Length > 0)
                {
                    groupCode = groupUser[0].Trim();
                }
                if (!string.IsNullOrEmpty(groupCode.Trim()))
                {
                    grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                }
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select distinct settings,value,ROW_NUMBER() over (ORDER BY settings DESC) as SetValue1,Case when settings='Admission No' then '1' when settings='Register No' then '2' when settings='Roll No' then '3' end as SetValue from Master_Settings where settings in('Roll No','Register No','Admission No') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method(Master1, ht, "Text");
            }
            else
            {
                dsSettings.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Columns.Add("settings");
                dt.Columns.Add("SetValue");
                dt.Rows.Add("Admission No", "1");
                dt.Rows.Add("Register No", "2");
                dt.Rows.Add("Roll No", "3");
                dsSettings.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return dsSettings;
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = da.GetFunction(qry).Trim();
                if (string.IsNullOrEmpty(insType) || insType.Trim() == "0")
                {
                    isSchoolOrCollege = false;
                }
                else if (!string.IsNullOrEmpty(insType) && insType.Trim() == "1")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }
            }
            return isSchoolOrCollege;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    public string GetAttendanceStatusName(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim();
        switch (attStatusCode)
        {
            case "1":
                attendanceStatus = "P";
                break;
            case "2":
                attendanceStatus = "A";
                break;
            case "3":
                attendanceStatus = "OD";
                break;
            case "4":
                attendanceStatus = "ML";
                break;
            case "5":
                attendanceStatus = "SOD";
                break;
            case "6":
                attendanceStatus = "NSS";
                break;
            case "7":
                attendanceStatus = "H";
                break;
            case "8":
                attendanceStatus = "NJ";
                break;
            case "9":
                attendanceStatus = "S";
                break;
            case "10":
                attendanceStatus = "L";
                break;
            case "11":
                attendanceStatus = "NCC";
                break;
            case "12":
                attendanceStatus = "HS";
                break;
            case "13":
                attendanceStatus = "PP";
                break;
            case "14":
                attendanceStatus = "SYOD";
                break;
            case "15":
                attendanceStatus = "COD";
                break;
            case "16":
                attendanceStatus = "OOD";
                break;
            case "17":
                attendanceStatus = "LA";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus.ToUpper().Trim();
    }

    public string GetAttendanceStatusCode(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim().ToUpper();
        switch (attStatusCode)
        {
            case "P":
                attendanceStatus = "1";
                break;
            case "A":
                attendanceStatus = "2";
                break;
            case "OD":
                attendanceStatus = "3";
                break;
            case "ML":
                attendanceStatus = "4";
                break;
            case "SOD":
                attendanceStatus = "5";
                break;
            case "NSS":
                attendanceStatus = "6";
                break;
            case "H":
                attendanceStatus = "7";
                break;
            case "NJ":
                attendanceStatus = "8";
                break;
            case "S":
                attendanceStatus = "9";
                break;
            case "L":
                attendanceStatus = "10";
                break;
            case "NCC":
                attendanceStatus = "11";
                break;
            case "HS":
                attendanceStatus = "12";
                break;
            case "PP":
                attendanceStatus = "13";
                break;
            case "SYOD":
                attendanceStatus = "14";
                break;
            case "COD":
                attendanceStatus = "15";
                break;
            case "OOD":
                attendanceStatus = "16";
                break;
            case "LA":
                attendanceStatus = "17";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus;
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        try
        {
            IDictionaryEnumerator e = hashTable.GetEnumerator();
            while (e.MoveNext())
            {
                if (Convert.ToString(e.Key).Trim() == Convert.ToString(key).Trim())
                {
                    return e.Value;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    protected void AttendancePercentage(string collegeCodeP, string BatchYear, string degreeP, string semP, string rollnoP, string admDateP, ref double OdCount, ArrayList DateHash = null)
    {
        string SemStartDate = string.Empty;
        string SemEndDate = string.Empty;
        if (!SemInfoDet.ContainsKey(degreeP + "$" + semP + "$" + BatchYear))
        {
            string SemInfoQry = "select semester,CONVERT(varchar(10), start_date,103)start_date,CONVERT(varchar(10), end_date,103)end_date,no_of_working_days from seminfo where degree_code=" + degreeP + " and semester =" + semP + " and batch_year= " + BatchYear + "  order by semester ";
            DataSet semdetailsDs = d2.selectDataSet(SemInfoQry);
            if (semdetailsDs.Tables[0].Rows.Count > 0)
            {
                SemStartDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["start_date"]);
                SemEndDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["end_date"]);
            }
            SemInfoDet.Add(degreeP + "$" + semP + "$" + BatchYear, SemStartDate + "*" + SemEndDate);
        }
        else
        {
            string[] semDate = Convert.ToString(SemInfoDet[degreeP + "$" + semP + "$" + BatchYear]).Split('*');
            if (semDate.Length == 2)
            {
                SemStartDate = Convert.ToString(semDate[0]);
                SemEndDate = Convert.ToString(semDate[1]);
            }
        }
        string dt = SemStartDate;
        string[] dsplit = dt.Split(new Char[] { '/' });
        SemStartDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demfcal = int.Parse(dsplit[2].ToString());
        demfcal = demfcal * 12;
        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
        cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
        monthcal = cal_from_date.ToString();
        dt = SemEndDate;
        dsplit = dt.Split(new Char[] { '/' });
        SemEndDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demtcal = int.Parse(dsplit[2].ToString());
        demtcal = demtcal * 12;
        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());
        per_from_gendate = Convert.ToDateTime(SemStartDate);
        per_to_gendate = Convert.ToDateTime(SemEndDate);

        ArrayList arrDegree = new ArrayList();
        if (!arrDegree.Contains(degreeP))
        {
            hat.Clear();
            hat.Add("degree_code", degreeP);
            hat.Add("sem_ester", int.Parse(semP));
            ds = da.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            int count = ds1.Tables[0].Rows.Count;
            arrDegree.Add(degreeP);
        }
        persentmonthcal(collegeCodeP, degreeP, semP, rollnoP, admDateP, SemStartDate, SemEndDate, DateHash);
        OdCount = per_tot_ondu;
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate, string SemStartDate, string SemEndDate, ArrayList DateHash = null)
    {
        medicalLeaveCountPerSession = 0;
        bool isadm = false;
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
        DataSet ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        int count = ds1.Tables[0].Rows.Count;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
            hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
            hat.Add("from_date", Convert.ToString(SemStartDate));
            hat.Add("to_date", Convert.ToString(SemEndDate));
            hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + SemStartDate.ToString() + "' and '" + SemEndDate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
            DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                    if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
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
                bool CheckFalge = false;
                if (DateHash == null || (DateHash != null && !DateHash.Contains(dumm_from_date)))
                {
                    CheckFalge = true;
                }
                if (dumm_from_date >= Admission_date && CheckFalge)
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
                                    value_holi_status = holiday_table11[(Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()].ToString();//dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()
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
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
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
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
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
        per_tot_ondu = tot_ondu;
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
        tot_ondu = 0;
    }
    //added by mullai 17/3/2018

    public bool DayLockForUser(DateTime seldate)
    {

        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate, prevdate, pdate;
        long total, k, s;
        int diff_days = 0;
        int lockday1 = 0;
        string[] ddate = new string[1000];
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }

        else
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string lockdayvalue = "select isnull(value,'0') as value  from Master_Settings where settings='OD Lock Days' " + grouporusercode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            // ds = da.select_method_wo_parameter(lockdayvalue, "text");
            //lockdayvalue = da.GetFunction("select value from Master_Settings where settings='OD Lock Days' " + grouporusercode + "");
            string lokdaysval = "0";
            if (ds.Tables[0].Rows.Count > 0)
            {
                lokdaysval = ds.Tables[0].Rows[0]["value"].ToString();
            }
            else
            {
                lokdaysval = "0";
            }
            lockday1 = Convert.ToInt32(lokdaysval);
            string degree = ddlBranchOD.SelectedValue.ToString();
            DataSet ds2 = new DataSet();
            string fdat = txtFromDateOD.Text.ToString();
            string[] frdat = fdat.Split('/');
            fdat = frdat[2] + "/" + frdat[1] + "/" + frdat[0];
            string tdat = txtToDateOD.Text.ToString();
            string[] todat = tdat.Split('/');
            tdat = todat[2] + "/" + todat[1] + "/" + todat[0];

            qry = da.GetFunction("select isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + fdat.ToString() + "' and '" + tdat.ToString() + "' and degree_code=" + degree + " and semester=" + Convert.ToString(ddlSemOD.SelectedItem.Text) + "");
            prevdate = Convert.ToDateTime(c_date);
            pdate = Convert.ToDateTime(seldate);
            TimeSpan ts = prevdate - pdate;
            diff_days = ts.Days;

            if (diff_days < lockday1)
            {

                daycheck = true;
                return daycheck;
            }
            //added by Mullai
            if (diff_days > lockday1)
            {
                if (Convert.ToInt32(qry) > 0)
                {
                    int odcout = diff_days - Convert.ToInt32(qry);
                    if (odcout < lockday1)
                    {
                        daycheck = true;
                        return daycheck;
                    }
                }
            }
            //***
            //command by Rajkumar on 8-10-2018
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][0].ToString() != "")
            //        {
            //            total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
            //            total = total + 1;

            //            String strholidasquery = "select holiday_date from holidaystudents where degree_code='" + Convert.ToString(degree).Trim() + "'  and semester='" + Convert.ToString(ddlSemOD.SelectedItem.Text).Trim() + "'";//Session["deg_code"]--Session["semester"]
            //            string colode=Convert.ToString(ddlCollegeOD.SelectedItem.Value);
            //            //String strholidasquery = "select holiday_date from holidaystudents where college_code='" + colode + "'";
            //            DataSet ds1 = new DataSet();
            //            ds1 = da.select_method(strholidasquery, hat, "Text");
            //            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count <= 0)
            //            {
            //                for (int i1 = 1; i1 < total; i1++)
            //                {
            //                    string temp_date = todate_day.AddDays(-i1).ToString();
            //                    string temp2 = todate_day.AddDays(i1).ToString();
            //                    if (temp_date == seldate.ToString())
            //                    {
            //                        daycheck = true;
            //                        return daycheck;
            //                    }
            //                    if (temp2 == seldate.ToString())
            //                    {
            //                        daycheck = true;
            //                        return daycheck;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                k = 0;
            //                for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
            //                {
            //                    ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
            //                    k++;
            //                }
            //                i = 0;
            //                while (i <= total - 1)
            //                {
            //                    string temp_date = curdate.AddDays(-i).ToString();
            //                    for (s = 0; s < k - 1; s++)
            //                    {
            //                        if (temp_date == ddate[s].ToString())
            //                        {
            //                            total = total + 1;
            //                            goto lab;
            //                        }
            //                    }
            //                lab:
            //                    i = i + 1;
            //                    if (temp_date == seldate.ToString())
            //                    {
            //                        daycheck = true;
            //                        return daycheck;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            daycheck = false;
            //        }
            //    }
            //}

        }
        return daycheck;

    }

    protected void gview_OnSelectedIndexChanged(Object sender, EventArgs e)
    {

        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            divODEntryDetails.Visible = false;
            BindReason();

            string retAppNo = (gview.Rows[rowIndex].FindControl("lblsnotag") as Label).Text.Trim();
            string retroll = gview.Rows[rowIndex].Cells[5].Text;
            string purpose = gview.Rows[rowIndex].Cells[14].Text;
            string attedancetype = gview.Rows[rowIndex].Cells[20].Text;

            string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code AND r.app_no='" + retAppNo + "'";
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            int sno = 0;
            if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
            {
                divODEntryDetails.Visible = true;
                sno++;
                string studname = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["stud_name"]).Trim();
                string app_No = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["app_No"]).Trim();
                string rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["roll_no"]).Trim();
                string regno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Reg_no"]).Trim();
                string sem = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["current_semester"]).Trim();
                string degreecode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]).Trim();
                string batchval = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["batch_year"]).Trim();
                string section = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["sections"]).Trim();
                string collegeCode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["college_code"]).Trim();
                string courseId = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Course_Id"]).Trim();
                string admissionno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Roll_Admit"]).Trim();
                attendenace(degreecode, sem);
                ddlCollegeOD.Enabled = false;
                ddlBatchOD.Enabled = false;
                ddlBranchOD.Enabled = false;
                ddlDegreeOD.Enabled = false;
                ddlSemOD.Enabled = false;
                ddlSecOD.Enabled = false;
                if (ddlCollegeOD.Items.Count > 0)
                {
                    ddlCollegeOD.SelectedValue = collegeCode;
                    ddlCollegeOD_SelectedIndexChanged(new object(), new EventArgs());
                }
                if (ddlBatchOD.Items.Count > 0)
                {
                    ddlBatchOD.SelectedValue = batchval;
                    ddlBatchOD_SelectedIndexChanged(new object(), new EventArgs());
                }
                if (ddlDegreeOD.Items.Count > 0)
                {
                    ddlDegreeOD.SelectedValue = courseId;
                    ddlDegreeOD_SelectedIndexChanged(new object(), new EventArgs());
                }
                if (ddlBranchOD.Items.Count > 0)
                {
                    ddlBranchOD.SelectedValue = degreecode;
                    ddlBranchOD_SelectedIndexChanged(new object(), new EventArgs());
                }
                if (ddlSemOD.Items.Count > 0)
                {
                    ddlSemOD.SelectedValue = sem;
                    ddlSemOD_SelectedIndexChanged(new object(), new EventArgs());
                }
                if (ddlSecOD.Items.Count > 0)
                {
                    ddlSecOD.Enabled = false;
                    ddlSecOD.SelectedIndex = 0;
                    if (section.Trim().ToLower() != "")
                    {
                        ddlSecOD.SelectedValue = section;
                    }
                }
                divPopODAlert.Visible = false;
                Init_Spread(1);

                drdetail = dtdetail.NewRow();
                drdetail["sno"] = Convert.ToString(sno);
                drdetail["snotag"] = batchval;
                drdetail["snonote"] = collegeCode;
                drdetail["Roll No"] = rollno;
                drdetail["Rolltag"] = degreecode;
                drdetail["Rollvalue"] = rollno;
                drdetail["Reg No"] = regno;
                drdetail["Regtag"] = section;
                drdetail["Regnote"] = courseId;
                drdetail["Admission No"] = admissionno;
                drdetail["Student Name"] = studname;
                drdetail["Semester"] = sem;
                drdetail["Semestertag"] = app_No;
                drdetail["Select"] = true;
                dtdetail.Rows.Add(drdetail);

                txtNoOfHours.Text = (gview.Rows[rowIndex].FindControl("lblouttmetag") as Label).Text;
                txtFromDateOD.Text = gview.Rows[rowIndex].Cells[15].Text;
                txtToDateOD.Text = gview.Rows[rowIndex].Cells[16].Text;


                btnPopSaveOD.Text = "Update";

                string getouttime = gview.Rows[rowIndex].Cells[17].Text;
                string getintime = gview.Rows[rowIndex].Cells[19].Text;

                string[] splitouttime = getouttime.Split(new char[] { ' ' });
                string[] splitintime = getintime.Split(new char[] { ' ' });
                string splitedouttime = splitouttime[0].ToString();
                string splitedoutmeridian = splitouttime[1].ToString();
                string splitedintime = splitintime[0].ToString();
                string splitedinmeridian = splitintime[1].ToString();
                string[] hourList = txtNoOfHours.Text.Split(',');
                if (hourList.Length > 0)
                {
                    cblHours.Items.Clear();
                    int item = 0;
                    foreach (string hrslst in hourList)
                    {
                        cblHours.Items.Add(new ListItem(hrslst, hrslst));
                        cblHours.Items[item].Selected = true;
                        item++;
                    }
                }
                string[] outtime = splitedouttime.Split(new char[] { ':' });
                string hour = outtime[0];
                string min = outtime[1];
                if (outtime[0].Length == 1)
                {
                    hour = "0" + outtime[0];
                }
                if (min.Length == 1)
                {
                    min = "0" + outtime[1];
                }
                string[] intime = splitedintime.Split(new char[] { ':' });
                string outhr = intime[0].ToString();
                string outmm = intime[1].ToString();
                if (outhr.Length == 1)
                {
                    outhr = "0" + outhr;
                }
                if (outmm.Length == 1)
                {
                    outmm = "0" + outmm;
                }
                ddlOutTimeHr.Enabled = false;
                ddlOutTimeMM.Enabled = false;
                ddlOutTimeSess.Enabled = false;
                ddlOutTimeHr.Text = hour;
                ddlOutTimeMM.Text = min;
                ddlOutTimeSess.Text = splitedoutmeridian;
                ddlInTimeHr.Enabled = false;
                ddlInTimeMM.Enabled = false;
                ddlInTimeSess.Enabled = false;
                ddlInTimeHr.Text = outhr;
                ddlInTimeMM.Text = outmm;
                ddlInTimeSess.Text = splitedinmeridian;
                purpose = da.GetFunction("select textcode from TextValTable where TextCriteria='Attrs' and textval='" + purpose + "' and college_code='" + collegeCode + "'");
                if (purpose.Trim() != "" && purpose.Trim() != "0")
                {
                    if (ddlPurpose.Items.Count > 0)
                    {
                        ddlPurpose.SelectedValue = purpose;
                    }
                }
                BindAttendanceRights();
                ddlAttendanceOption.Enabled = false;
                if (ddlAttendanceOption.Items.Count > 0)
                {
                    ListItem list = new ListItem(attedancetype.Trim().ToUpper(), attedancetype.Trim().ToUpper());

                    if (ddlAttendanceOption.Items.Contains(list))
                    {
                        ddlAttendanceOption.Text = attedancetype;
                    }
                }
                btnPopSaveOD.Enabled = true;

                gviewstudetails.DataSource = dtdetail;
                gviewstudetails.DataBind();
                gviewstudetails.Visible = true;
                //CheckBox chk = (gviewstudetails.Rows[1].FindControl("check") as CheckBox);
                //chk.Checked = true;

                //gviewstudetails.DataSource = dtdetail;
                //gviewstudetails.DataBind();
                //gviewstudetails.Visible = true;

                CheckBox chck = (gviewstudetails.Rows[1].FindControl("check") as CheckBox);
                chck.Checked = true;

                gviewstudetails.Columns[1].Visible = isRollVisible;
                gviewstudetails.Columns[2].Visible = isRegVisible;
                gviewstudetails.Columns[3].Visible = isAdmitNoVisible;

                if (Convert.ToString(ViewState["ODCheck"]).Trim() != "0")
                {
                    gviewstudetails.Columns[6].Visible = true;
                }
                else
                {
                    gviewstudetails.Columns[6].Visible = false;
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record Found";
                divPopAlert.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void gview_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex != 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 2; i < e.Row.Cells.Count; i++)
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
        }
    }

    protected void gviewstudetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    ((CheckBox)e.Row.FindControl("check")).Attributes.Add("onclick",
                       "javascript:SelLedgers('" +
                       ((CheckBox)e.Row.FindControl("check")).ClientID + "')");
                }
            }
        }
        catch
        {

        }
    }

    protected void gviewOD_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                for (int cell = 1; cell < e.Row.Cells.Count; cell++)
                {
                    //CheckBox chk = (e.Row.Cells[cell].FindControl("chkhour" + cell + "") as CheckBox);
                    //chk.ID = "selectall" + cell + "";

                    ((CheckBox)e.Row.FindControl("chkhour" + cell + "")).Attributes.Add("onclick",
                       "javascript:Selectall('" +
                       ((CheckBox)e.Row.FindControl("chkhour" + cell + "")).ClientID + "')");
                }

            }
        }
    }

    #region ODHour
    protected void btnOdhour_Click(object sender, EventArgs e)
    {
        try
        {
            bool chkflag = false;
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    gviewOD.Visible = false;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose From Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                gviewOD.Visible = false;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    gviewOD.Visible = false;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                gviewOD.Visible = false;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                gviewOD.Visible = false;
                return;
            }

            for (int grd = 1; grd < gviewstudetails.Rows.Count; grd++)
            {
                CheckBox ckbx = (gviewstudetails.Rows[grd].FindControl("check") as CheckBox);
                if (ckbx.Checked)
                {
                    chkflag = true;
                }
            }
            if (!chkflag)
            {
                lblODAlertMsg.Text = "Please Choose Student to Mark OD";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                gviewOD.Visible = false;
                return;
            }

            DataTable dtOdHour = new DataTable();
            DataRow drOdHour;

            dtOdHour.Columns.Add("OdDate");
            drOdHour = dtOdHour.NewRow();
            dtOdHour.Rows.Add(drOdHour);

            string[] dbsplfm = fromDate.Split('/');
            string[] dbspltoo = toDate.Split('/');
            string convfrm = dbsplfm[1] + "/" + dbsplfm[0] + "/" + dbsplfm[2];
            DateTime dbconvfrm = Convert.ToDateTime(convfrm);
            string convto = dbspltoo[1] + "/" + dbspltoo[0] + "/" + dbspltoo[2];
            DateTime dbconvto = Convert.ToDateTime(convto);

            while (dbconvfrm <= dbconvto)
            {
                drOdHour = dtOdHour.NewRow();
                string[] spl = Convert.ToString(dbconvfrm).Split(' ');
                string[] dtespl = Convert.ToString(spl[0]).Split('/');
                if (dtespl[0].Length == 1) { dtespl[0] = "0" + dtespl[0]; }
                if (dtespl[1].Length == 1) { dtespl[1] = "0" + dtespl[1]; }
                string dte = dtespl[1] + "/" + dtespl[0] + "/" + dtespl[2];
                drOdHour["OdDate"] = dte;
                dtOdHour.Rows.Add(drOdHour);
                dbconvfrm = dbconvfrm.AddDays(1);
            }
            string tothrs = string.Empty;
            string fsthalfhrs = string.Empty;
            string scndhalfhrs = string.Empty;
            Hashtable htHours = (Hashtable)Session["htHoursPerDay"];
            if (htHours.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHours)
            {
                string daytext = Convert.ToString(parameter1.Key);
                string noofhours = Convert.ToString(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            DataTable dthour = new DataTable(); dthour.Columns.Add("Hour"); DataRow dr;
            if (tothrs != "" && tothrs != null)
            {
                string temp = string.Empty;
                for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                {
                    dr = dthour.NewRow(); dr["Hour"] = fulhrs; dthour.Rows.Add(dr);
                }
            }

            gviewOD.DataSource = dtOdHour;
            gviewOD.DataBind();
            gviewOD.Visible = true;

            if (gviewOD.Rows.Count > 0)
            {
                for (int i = 0; i < dthour.Rows.Count; i++)
                {
                    if (gviewOD.Columns[i + 1].HeaderText == Convert.ToString(dthour.Rows[i]["Hour"]))
                    {
                        gviewOD.Columns[i + 1].Visible = true;
                    }
                    else
                    {
                        gviewOD.Columns[i + 1].Visible = false;
                    }
                }
            }

            #region MarkOD
            string[] spltFrmdate = fromDate.Split('/');
            string chkFrmdate = spltFrmdate[1] + "/" + spltFrmdate[0] + "/" + spltFrmdate[2];
            string[] spltTodate = toDate.Split('/');
            string chkTodate = spltTodate[1] + "/" + spltTodate[0] + "/" + spltTodate[2];
            string rollin = string.Empty;
            string qryRoll = string.Empty;
            string qrySem = string.Empty;
            string semOd = string.Empty;
            DataSet dset = new DataSet();
            for (int i = 1; i < gviewstudetails.Rows.Count; i++)
            {
                CheckBox chkbox = (gviewstudetails.Rows[i].FindControl("check") as CheckBox);
                string rollNo = (gviewstudetails.Rows[i].FindControl("lblroll") as Label).Text;
                string strOd = (gviewstudetails.Rows[i].FindControl("lblsem") as Label).Text;
                if (chkbox.Checked)
                {
                    if (string.IsNullOrEmpty(rollin))
                    {
                        rollin = rollNo;
                    }
                    else
                    {
                        rollin = rollin + "','" + rollNo;
                    }
                    if (string.IsNullOrEmpty(semOd))
                    {
                        semOd = strOd;
                    }
                    else
                    {
                        if (!semOd.Contains(strOd))
                        {
                            semOd = semOd + "','" + strOd;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(rollin) && !string.IsNullOrEmpty(semOd))
            {
                qryRoll = " and Roll_no in('" + rollin + "')";
                qrySem = "and Semester in('" + semOd + "')";
            }
            if (!string.IsNullOrEmpty(qryRoll))
            {
                string strqry = "select hourse,day from Onduty_Stud where day is not null and day between '" + chkFrmdate + "' and '" + chkTodate + "' " + qryRoll + " " + qrySem + " order by day";
                dset = da.select_method_wo_parameter(strqry, "Text");
            }
            if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
            {
                for (int ichk = 0; ichk < dset.Tables[0].Rows.Count; ichk++)
                {
                    string hours = Convert.ToString(dset.Tables[0].Rows[ichk]["hourse"]);
                    string days = Convert.ToString(dset.Tables[0].Rows[ichk]["day"]);
                    string[] splday = days.Split(' ');
                    string[] spday = splday[0].Split('/');
                    string day = spday[0] + "/" + spday[1] + "/" + spday[2];
                    DateTime dt = Convert.ToDateTime(day);
                    for (int gint = 1; gint < gviewOD.Rows.Count; gint++)
                    {
                        string gdate = (gviewOD.Rows[gint].FindControl("lblODdate") as Label).Text;
                        string[] spday1 = gdate.Split('/');
                        string days1 = spday1[1] + "/" + spday1[0] + "/" + spday1[2];
                        DateTime dt1 = Convert.ToDateTime(days1);
                        if (dt1 == dt)
                        {
                            for (int cint = 1; cint < gviewOD.Columns.Count; cint++)
                            {
                                if (gviewOD.Columns[cint].Visible == true)
                                {
                                    string colName = gviewOD.Columns[cint].HeaderText;
                                    if (hours.Contains(colName))
                                    {
                                        CheckBox chkB = (gviewOD.Rows[gint].FindControl("chkhour" + colName) as CheckBox);
                                        chkB.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int ii = 1; ii < gviewOD.Rows.Count; ii++)
                {
                    for (int col = 1; col < gviewOD.Columns.Count; col++)
                    {
                        CheckBox cbx = (gviewOD.Rows[ii].FindControl("chkhour" + col) as CheckBox);
                        cbx.Checked = true;
                    }
                }
            }
            #endregion
        }
        catch
        { }
    }
    #endregion

    public void demo(object sender, EventArgs e)
    {

    }
}