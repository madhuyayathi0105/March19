using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class MarkMod_SubjectWiseTestMark : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string testName = string.Empty;
    string testNo = string.Empty;
    string subjectName = string.Empty;
    string subjectNo = string.Empty;
    string subjectCode = string.Empty;
    string sections = string.Empty;

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryCourseId = string.Empty;
    string qrytestNo = string.Empty;
    string qrytestName = string.Empty;
    string qrySubjectNo = string.Empty;
    string qrySubjectName = string.Empty;
    string qrySubjectCode = string.Empty;

    Dictionary<int, string> diccolspan = new Dictionary<int, string>();

    int selectedCount = 0;
    int subcount = 0;
    int subcount1 = 0;
    Institution institute;

    static int coluheadcount1 = 0;

    bool drtprint = false;

    #region Attendance

    string fromDate = string.Empty;
    string toDate = string.Empty;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    bool isValidDate = false;

    TimeSpan tsFromToDiff = new TimeSpan();

    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds1 = new DataSet();

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
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    string dd = string.Empty;
    string diff_date;
    string value, date;
    string tempvalue = "-1";
    string value_holi_status = string.Empty;
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;

    string[] split_holiday_status = new string[1000];

    double dif_date = 0;
    double dif_date1 = 0;
    double per_perhrs, per_abshrs, per_leavehrs;
    double per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    double leavfinaeamount = 0;
    double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    double medicalLeaveDays = 0;

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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                setLabelText();
                chkConvertedTo.Checked = false;
                chkIncludeGrade.Checked = false;
                chkRoundOffMarks.Checked = false;
                //divStudentDetail.Visible = false;
                divMainContents.Visible = false;
                chkConvertedTo.Checked = false;
                txtConvertedMaxMark.Text = string.Empty;
                txtConvertedMaxMark.Enabled = false;
                BindPreviousCollege();
                BindRightsBaseBatch();
                BindPreviousDegrees("");
                BindPreviousDepartment("", "");
                //magesh 26.2.18
                //BindPreviousSemesters("", "");
                bindsem();//magesh 26.2.18
                BindRightsBasedSectionDetail();
                BindPreviousTestName();
                BindPreviousSubject();
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
                dsSettings = dirAcc.selectDataSet(Master1);
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

    private void BindPreviousCollege(byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedCollege = null)
    {
        try
        {
            string singleUser = ((Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "true");
            string groupUserCode = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "");
            string userCode = ((Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "0");
            if (groupUserCode.Contains(';'))
            {
                string[] groupUser = groupUserCode.Split(';');
                groupUserCode = groupUser[0].ToString();
            }
            dtCommon.Clear();
            ddlCollege.Items.Clear();
            ddlCollege.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetAllCollege", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
                if (!string.IsNullOrEmpty(defaultSelectedCollege))
                {
                    //ddlCollege.SelectedValue = defaultSelectedCollege;
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

    public void BindRightsBaseBatch(byte redoType = 2, string defaultSelectedBatch = null)
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryCollegeCode = string.Empty;
            qryCollegeCode1 = string.Empty;
            ddlBatch.Items.Clear();
            ddlBatch.Enabled = false;
            ds.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
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
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                qryCollegeCode1 = " and college_code in(" + collegeCode + ")";
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollegeCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryCollegeCode + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = dirAcc.selectDataSet(qry);
            }
            List<string> lstBatch = new List<string>();
            string qryBatchYear1 = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => Convert.ToString(r.Field<int>("batch_year"))).ToList();
                string batchList = string.Join("','", lstBatch.ToArray());
                if (!string.IsNullOrEmpty(batchList))
                {
                    qryBatchYear1 = " and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),''))) in('" + batchList + "')";
                    qryBatchYear = " and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),''))) in('" + batchList + "')";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select distinct srh.BatchYear from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.collegeCode=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=srh.collegeCode and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and srh.degreeCode=dg.Degree_Code  and srh.BatchYear<>'0' and srh.BatchYear<>-1 and srh.RedoType='" + redoType + "' and srh.collegeCode in(" + collegeCode + ") " + qryBatchYear1 + " order by srh.BatchYear desc";
                qry = "select distinct case when LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),'')))<>'' then LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),''))) when LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),'')))<>'' then  LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),''))) end  BatchYear from Course c,Degree dg,Department dt,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),'')))<>'0' and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),'')))<>-1 and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),srh.BatchYear),'')))<>'' and srh.RedoType='" + redoType + "' and srh.collegeCode in(" + collegeCode + ") " + qryBatchYear1 + " where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),'')))<>'-1' and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),'')))<>'0' and LTRIM(RTRIM(ISNULL(CONVERT(varchar(20),r.Batch_year),'')))<>'' and r.college_code in(" + collegeCode + ") " + qryBatchYear + "  order by BatchYear desc";
                ds.Clear();
                ds = dirAcc.selectDataSet(qry);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "BatchYear";
                ddlBatch.DataValueField = "BatchYear";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
                ddlBatch.Enabled = true;
                //if (!string.IsNullOrEmpty(defaultSelectedBatch))
                //    SelectDataBound(ddlBatch, defaultSelectedBatch, defaultSelectedBatch);
            }
        }
        catch
        {
        }
    }

    private void BindPreviousDegrees(string collegeCode, string batchYear = null, byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedDegree = null)
    {
        try
        {
            string singleUser = ((Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "true");
            string groupUserCode = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "");
            string userCode = ((Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "0");
            if (groupUserCode.Contains(';'))
            {
                string[] groupUser = groupUserCode.Split(';');
                groupUserCode = groupUser[0].ToString();
            }
            dtCommon.Clear();
            if (string.IsNullOrEmpty(collegeCode))
                collegeCode = ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
            if (string.IsNullOrEmpty(batchYear))
                batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
            ddlDegree.Items.Clear();
            ddlDegree.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetAllDegrees", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlDegree.DataSource = dtCommon;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
                ddlDegree.SelectedIndex = 0;
                ddlDegree.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindPreviousDepartment(string collegeCode, string courseID, string batchYear = null, byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedDegree = null)
    {
        try
        {
            string singleUser = ((Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "true");
            string groupUserCode = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "");
            string userCode = ((Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "0");
            if (groupUserCode.Contains(';'))
            {
                string[] groupUser = groupUserCode.Split(';');
                groupUserCode = groupUser[0].ToString();
            }
            dtCommon.Clear();
            ddlBranch.Items.Clear();
            ddlBranch.Enabled = false;
            dicQueryParameter.Clear();
            if (string.IsNullOrEmpty(collegeCode))
                collegeCode = ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
            if (string.IsNullOrEmpty(batchYear))
                batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
            if (string.IsNullOrEmpty(courseID))
                courseID = ((ddlDegree.Items.Count > 0) ? Convert.ToString(ddlDegree.SelectedValue).Trim() : "");

            //dicQueryParameter.Add("appNo", appNo);
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("courseID", courseID);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetAllDepartment", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlBranch.DataSource = dtCommon;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.SelectedIndex = 0;
                ddlBranch.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    //private void BindPreviousSemesters(string collegeCode = null, string batchYear = null, string degreeCode = null, byte redoType = 2, string defaultSelectedDegree = null)
    //{
    //    try
    //    {

    //        string singleUser = ((Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "true");
    //        string groupUserCode = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "");
    //        string userCode = ((Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "0");
    //        if (groupUserCode.Contains(';'))
    //        {
    //            string[] groupUser = groupUserCode.Split(';');
    //            groupUserCode = groupUser[0].ToString();
    //        }
    //        dtCommon.Clear();
    //        if (string.IsNullOrEmpty(collegeCode))
    //            collegeCode = ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
    //        if (string.IsNullOrEmpty(batchYear))
    //            batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
    //        if (string.IsNullOrEmpty(degreeCode))
    //            degreeCode = ((ddlBranch.Items.Count > 0) ? Convert.ToString(ddlBranch.SelectedValue).Trim() : "");
    //        ddlSem.Items.Clear();
    //        ddlSem.Enabled = false;
    //        dicQueryParameter.Clear();
    //        dicQueryParameter.Add("collegeCode", collegeCode);
    //        dicQueryParameter.Add("batchYear", batchYear);
    //        dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
    //        dicQueryParameter.Add("degreeCode", degreeCode);
    //        dtCommon = storeAcc.selectDataTable("uspGetAllSemester", dicQueryParameter);

    //        if (dtCommon.Rows.Count > 0)
    //        {
    //            ddlSem.DataSource = dtCommon;
    //            ddlSem.DataTextField = "semester";
    //            ddlSem.DataValueField = "semester";
    //            ddlSem.DataBind();
    //            ddlSem.SelectedIndex = 0;
    //            ddlSem.Enabled = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    public void bindsem()
    {
        try
        {
            //--------------------semester load
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string query = string.Empty;
            DataSet ds = new DataSet();
            if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
            {
                query = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(query, "Text");
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
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                if (ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
                {
                    query = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                    ddlSem.Items.Clear();
                    ds = new DataSet();
                    ds.Clear();
                    ds = da.select_method_wo_parameter(query, "Text");
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
                            ddlSem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(i.ToString());
                        }
                    }
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                ddlSem.SelectedIndex = 0;

                //magesh 26.2.18
                //BindSectionDetail();
            }
            //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBasedSectionDetail(string defaultSelectedSections = null)
    {
        batchYear = string.Empty;
        collegeCode = string.Empty;
        degreeCode = string.Empty;
        semester = string.Empty;
        string sections = string.Empty;

        qrySection = string.Empty;
        qryCollegeCode = string.Empty;
        qryBatchYear = string.Empty;
        qryDegreeCode = string.Empty;
        qrySemester = string.Empty;
        string qryCollegeCode1 = string.Empty;
        string qryBatchYear1 = string.Empty;
        string qryDegreeCode1 = string.Empty;
        string qrySemester1 = string.Empty;
        string qrySection1 = string.Empty;
        ds.Clear();
        ddlSec.Items.Clear();

        userCode = string.Empty;
        groupUserCode = string.Empty;
        qryUserOrGroupCode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            string group = Convert.ToString(Session["group_code"]).Trim();
            if (group.Contains(';'))
            {
                string[] group_semi = group.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
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
        if (ddlCollege.Items.Count > 0)
        {
            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
        }
        if (!string.IsNullOrEmpty(collegeCode))
        {
            qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
            qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
        }
        if (ddlBatch.Items.Count > 0)
        {
            batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and r.batch_year in(" + batchYear + ")";
                qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
            }
        }
        if (ddlBranch.Items.Count > 0)
        {
            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
            }
        }
        if (ddlSem.Items.Count > 0)
        {
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            qrySemester = " and r.current_semester in(" + semester + ")";
            qrySemester1 = " and srh.semester in(" + semester + ")";
        }
        if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryBatchYear))
        {
            qrySection = dirAcc.selectScalarString("select distinct sections from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + qryCollegeCode + qryBatchYear).Trim();
        }
        if (!string.IsNullOrEmpty(qrySection.Trim()) && qrySection.Trim() != "0" && qrySection.Trim() != "-1")
        {
            string[] sectionsAll = qrySection.Trim().Split(new char[] { ',' }, StringSplitOptions.None);
            sections = string.Empty;
            qrySection = string.Empty;
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
                    else
                    {
                        if (!hasEmpty)
                        {
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                            hasEmpty = true;
                        }
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(sections.Trim()))
        {
            qrySection = " and isnull(ltrim(rtrim(r.sections)),'') in(" + sections + ") ";
            qrySection1 = " and isnull(ltrim(rtrim(srh.sections)),'') in(" + sections + ") ";
        }
        else
        {
            qrySection = string.Empty;
        }
        if (!string.IsNullOrEmpty(qryCollegeCode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryDegreeCode1) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryBatchYear1))
        {
            qry = "select distinct case when isnull(ltrim(rtrim(srh.sections)),'')<>'' then isnull(ltrim(rtrim(srh.sections)),'') when isnull(ltrim(rtrim(r.Sections)),'')<>'' then isnull(ltrim(rtrim(r.sections)),'') else case when(isnull(ltrim(rtrim(srh.sections)),'')='' or isnull(ltrim(rtrim(r.sections)),'')='') then 'Empty' end end as sections, case when isnull(ltrim(rtrim(srh.sections)),'')<>'' then isnull(ltrim(rtrim(srh.sections)),'') when isnull(ltrim(rtrim(r.sections)),'')<>'' then isnull(ltrim(rtrim(r.sections)),'') else '' end SecValues from Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and isnull(ltrim(rtrim(srh.sections)),'')<>'-1' and isnull(ltrim(rtrim(srh.sections)),'')<>'0' and srh.RedoType='2' " + qryCollegeCode1 + qryDegreeCode1 + qryBatchYear1 + qrySection1 + qrySemester1 + " where isnull(ltrim(rtrim(r.sections)),'')<>'-1' and isnull(ltrim(rtrim(r.sections)),'')<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + qrySection + qrySemester + " order by SecValues";
            ds.Clear();
            ds = dirAcc.selectDataSet(qry);
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "SecValues";
            ddlSec.DataBind();
            ddlSec.Enabled = true;

        }
        else
        {
            ddlSec.Enabled = false;
        }
    }

    private void BindPreviousTestName()
    {
        try
        {
            batchYear = string.Empty;
            collegeCode = string.Empty;
            degreeCode = string.Empty;
            semester = string.Empty;
            sections = string.Empty;
            section = string.Empty;

            qrySection = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            dtCommon.Clear();
            ddlTest.Items.Clear();
            ddlTest.Enabled = false;
            cblTest.Items.Clear();
            chkTest.Checked = false;
            txtTest.Text = "--Select--";
            txtTest.Enabled = false;

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and college_code in(" + collegeCode + ")";
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
                qrySemester = " and sm.semester in(" + semester + ")";
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled == true)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                qrySection = string.Empty;
                if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
                    section = string.Empty;
                else
                {

                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + section + "')";
                }
            }
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {

                dicQueryParameter.Clear();
                dicQueryParameter.Add("batchYear", batchYear);
                dicQueryParameter.Add("degreeCode", degreeCode);
                dicQueryParameter.Add("semester", semester);
                dicQueryParameter.Add("section", section);
                dtCommon = storeAcc.selectDataTable("uspGetPreviousTestDetails", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlTest.DataSource = dtCommon;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "Criteria_no";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
                ddlTest.Enabled = true;

                cblTest.DataSource = dtCommon;
                cblTest.DataTextField = "criteria";
                cblTest.DataValueField = "Criteria_no";
                cblTest.DataBind();
                txtTest.Enabled = true;
                checkBoxListselectOrDeselect(cblTest, true);
                CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");

            }
        }
        catch
        {
        }
    }

    private void BindPreviousSubject()
    {
        try
        {
            batchYear = string.Empty;
            collegeCode = string.Empty;
            degreeCode = string.Empty;
            semester = string.Empty;
            testName = string.Empty;
            testNo = string.Empty;
            string sections = string.Empty;

            qrySection = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            dtCommon.Clear();

            cblSubject.Items.Clear();
            txtSubject.Text = "--Select--";
            txtSubject.Enabled = false;
            chkSubject.Checked = false;

            ddlSubject.Items.Clear();
            ddlSubject.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and college_code in('" + collegeCode + "')";
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and batch_year in('" + batchYear + "')";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in('" + degreeCode + "')";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
                qrySemester = " and sm.semester in('" + semester + "')";
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled == true)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                qrySection = string.Empty;
                if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
                    section = string.Empty;
                else
                {
                    qrySection = " and e.sections in('" + section + "')";
                }
            }
            if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            {
                testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
                testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            else if (cblTest.Items.Count > 0 && txtTest.Visible)
            {
                testNo = getCblSelectedValue(cblTest);
                testName = getCblSelectedText(cblTest);
            }
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
            {
                //dicQueryParameter.Clear();
                //dicQueryParameter.Add("batchYear", batchYear);
                //dicQueryParameter.Add("degreeCode", degreeCode);
                //dicQueryParameter.Add("semester", semester);
                //dicQueryParameter.Add("section", section);
                //dicQueryParameter.Add("testNo", testNo);
                //dtCommon = storeAcc.selectDataTable("uspGetPreviousTestSubjectDetails", dicQueryParameter);
                //dtCommon = dirAcc.selectDataTable("select distinct s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ")" + qrySection + " order by s.subject_code");
                qry = "select distinct s.subject_code,s.subject_name,ISNULL(s.subjectpriority,'0') as subjectpriority from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " union select subject_code=STUFF((select '$mr$'+s.subject_code from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " for XML PATH('')),1,4,''),ss.subject_type as subject_name,min(ISNULL(s.subjectpriority,'0')) as subjectpriority from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " group by ss.subject_type order by subjectpriority,subject_code";
                dtCommon = dirAcc.selectDataTable(qry);
            }
            if (dtCommon.Rows.Count > 0)
            {
                cblSubject.DataSource = dtCommon;
                cblSubject.DataTextField = "subject_name";
                cblSubject.DataValueField = "subject_code";
                cblSubject.DataBind();
                txtSubject.Enabled = true;
                checkBoxListselectOrDeselect(cblSubject, true);
                CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");

                ddlSubject.DataSource = dtCommon;
                ddlSubject.DataTextField = "subject_name";
                ddlSubject.DataValueField = "subject_code";
                ddlSubject.DataBind();
                ddlSubject.Enabled = true;
                ddlSubject.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, ref int startColumm, int endColumn, int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            Dictionary<string, byte> dicColumnVisiblity = new Dictionary<string, byte>();
            //columnVisibility(ref dicColumnVisiblity);
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            bool isVisibleColumn = false;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 7;

                byte value = 0;
                FpSpread1.Sheets[0].Columns[0].Width = 35;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isRollNoVisible;
                startColumm = (isRollNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegNoVisible;
                startColumm = (isRegNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmissionNoVisible;
                startColumm = (isAdmissionNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                startColumm = (isStudentTypeVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 400;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                //FpSpread1.Sheets[0].Columns[7].Width = 80;
                //FpSpread1.Sheets[0].Columns[7].Locked = true;
                //FpSpread1.Sheets[0].Columns[7].Resizable = false;
                //FpSpread1.Sheets[0].Columns[7].Visible = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Marks\n";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                //FpSpread1.Sheets[0].Columns[8].Width = 80;
                //FpSpread1.Sheets[0].Columns[8].Locked = true;
                //FpSpread1.Sheets[0].Columns[8].Resizable = false;
                //string convertMark = txtConvertedMaxMark.Text;
                //double convertedMax = 0;
                //double.TryParse(convertMark.Trim(), out convertedMax);
                //string display = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                //FpSpread1.Sheets[0].Columns[8].Visible = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? true : false;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Mark\n" + display;
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                //FpSpread1.Sheets[0].Columns[9].Width = 80;
                //FpSpread1.Sheets[0].Columns[9].Locked = true;
                //FpSpread1.Sheets[0].Columns[9].Resizable = false;
                //FpSpread1.Sheets[0].Columns[9].Visible = (chkIncludeGrade.Checked);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Grade";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 3;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 250;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            string studentApplicationNo = string.Empty;
            BindRightsBaseBatch();
            BindPreviousDegrees("");
            BindPreviousDepartment("", "");
            //magesh 26.2.18
            //BindPreviousSemesters("", "");
            bindsem();//magesh 26.2.18
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            BindPreviousSubject();
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
            string studentApplicationNo = string.Empty;
            BindPreviousDegrees("");
            BindPreviousDepartment("", "");
            //magesh 26.2.18
            //BindPreviousSemesters("", "");
            bindsem();//magesh 26.2.18
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            BindPreviousSubject();
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
            string studentApplicationNo = string.Empty;
            BindPreviousDepartment("", "");
            //magesh 26.2.18
            //BindPreviousSemesters("", "");
            bindsem();//magesh 26.2.18
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            BindPreviousSubject();
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
            string studentApplicationNo = string.Empty;
            //magesh 26.2.18
            //BindPreviousSemesters("", "");
            bindsem();//magesh 26.2.18
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            BindPreviousSubject();
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
            string studentApplicationNo = string.Empty;
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            BindPreviousSubject();
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
            string studentApplicationNo = string.Empty;
            BindPreviousTestName();
            BindPreviousSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlTest_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            string studentApplicationNo = string.Empty;
            BindPreviousSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkTest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = string.Empty;
            BindPreviousSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = string.Empty;
            BindPreviousSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSubject_SelectedIndexChanged(Object sender, EventArgs e)
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

    protected void chkConvertedTo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtConvertedMaxMark.Text = string.Empty;
            txtConvertedMaxMark.Enabled = chkConvertedTo.Checked;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #region TextBox Changed

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContents.Visible = false;
            DateTime dtToday = new DateTime();
            dtToday = DateTime.Now;
            fromDate = txtFromDate.Text.Trim();
            toDate = txtToDate.Text.Trim();

            //if (CheckSettings())
            //{
            //    divMainContents.Visible = false;
            //    lblAlertMsg.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                if (!isValidDate)
                {
                    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "Please Choose From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                if (!isValidDate)
                {
                    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "Please Choose To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtFromDate > dtToday)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtToDate > dtToday)
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtFromDate > dtToDate)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContents.Visible = false;

            DateTime dtToday = new DateTime();
            dtToday = DateTime.Now;
            fromDate = txtFromDate.Text.Trim();
            toDate = txtToDate.Text.Trim();

            //if (CheckSettings())
            //{
            //    divMainContents.Visible = false;
            //    lblAlertMsg.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                if (!isValidDate)
                {
                    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "Please Choose From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                if (!isValidDate)
                {
                    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "Please Choose To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtFromDate > dtToday)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtToDate > dtToday)
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (dtFromDate > dtToDate)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblAlertMsg.Text = "To Date Must Be Greater Than or Equal to From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    #endregion Index Changed Events

    #region Button Events

    #region Get Students Marks

    protected void btnprintdiect(object sender, EventArgs e)
    {
        FpStudentMarkList.Visible = true;
        //FpStudentMarkList.SaveChanges();
        string totval = string.Empty;
        Dictionary<int, string> dicsubnam = new Dictionary<int, string>();
        Dictionary<int, string> dicsubno = new Dictionary<int, string>();
        Dictionary<int, string> dicrak = new Dictionary<int, string>();
        drtprint = true;
        contentDiv.InnerHtml = "";
        StringBuilder html = new StringBuilder();
        int totVar = 0;
        lblAlertMsg.Text = string.Empty;
        divPopAlert.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        divMainContents.Visible = false;

        collegeCode = string.Empty;
        batchYear = string.Empty;
        degreeCode = string.Empty;
        semester = string.Empty;
        section = string.Empty;
        testName = string.Empty;
        testNo = string.Empty;
        subjectCode = string.Empty;
        subjectName = string.Empty;
        subjectNo = string.Empty;

        orderBy = string.Empty;
        orderBySetting = string.Empty;

        qry = string.Empty;
        qryCollegeCode = string.Empty;
        qryBatchYear = string.Empty;
        qryDegreeCode = string.Empty;
        qrySemester = string.Empty;
        qrySection = string.Empty;

        string qryCollegeCode1 = string.Empty;
        string qryBatchYear1 = string.Empty;
        string qryDegreeCode1 = string.Empty;
        string qrySemester1 = string.Empty;
        string qrySection1 = string.Empty;

        qryCourseId = string.Empty;
        qrytestNo = string.Empty;
        qrytestName = string.Empty;
        qrySubjectNo = string.Empty;
        qrySubjectName = string.Empty;
        qrySubjectCode = string.Empty;

        DataTable dtStudentMarks = new DataTable();
        DataTable dtGradeDetails = new DataTable();
        string colgnam = Convert.ToString(ddlCollege.SelectedItem.Text);
        string clgdet = "select collname,affliatedby from collinfo where college_code='" + ddlCollege.SelectedValue.ToString() + "'";
        DataSet cgdet = da.select_method_wo_parameter(clgdet, "text");
        string affilat = string.Empty;
        if (cgdet.Tables.Count > 0 && cgdet.Tables[0].Rows.Count > 0)
        {
            string aff = Convert.ToString(cgdet.Tables[0].Rows[0]["affliatedby"]);
            string[] split = aff.Split(',');
            affilat = split[0];
        }
        if (ddlTest.Items.Count > 0 && ddlTest.Visible)
        {
            testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
            testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            if (!string.IsNullOrEmpty(testNo))
            {
                qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            }
            else
            {
                lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
        }
        string classadvi = string.Empty;
        string clsnam = " select sm.staff_name from Semester_Schedule s ,staffmaster sm where s.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and s.semester='" + ddlSem.SelectedItem.Text.ToString() + "' and s.batch_year='" + ddlBatch.SelectedItem.Text.ToString() + "'  and sm.staff_code=s.class_advisor and s.Sections='" + ddlSec.SelectedItem.Text.ToString() + "'";
        DataSet dsadnam = da.select_method_wo_parameter(clsnam, "text");
        if (dsadnam.Tables.Count > 0 && dsadnam.Tables[0].Rows.Count > 0)
        {
            classadvi = dsadnam.Tables[0].Rows[0]["staff_name"].ToString();
        }
        string studstrg = string.Empty;
        string strg = "select count(roll_no) as strength from Registration where Batch_Year='" + ddlBatch.SelectedItem.Text.ToString() + "' and Sections='" + ddlSec.SelectedItem.Text.ToString() + "' and college_code='" + ddlCollege.SelectedValue.ToString() + "' and  degree_code='" + ddlBranch.SelectedValue.ToString() + "' and Current_Semester='" + ddlSem.SelectedItem.Text.ToString() + "' and delflag=0 and exam_flag<>'Debar' and cc=0";
        DataSet dsstrg = da.select_method_wo_parameter(strg, "text");
        if (dsstrg.Tables.Count > 0 && dsstrg.Tables[0].Rows.Count > 0)
        {
            studstrg = dsstrg.Tables[0].Rows[0]["strength"].ToString();
        }
        else
        {
            studstrg = "-";
        }
        string yr = DateTime.Now.ToString("yyyy");
        int yer = Convert.ToInt32(yr) + 1;
        string sections = string.Empty;
        if (ddlSec.Enabled == true)
        {
            sections = ddlSec.SelectedItem.Text.ToString();
            if (sections == "Empty")
                sections = string.Empty;
            else
                sections = " - " + sections + "";
        }
        int ctval3 = coluheadcount1;
        int ctval4 = 0;
        int cval5 = 0;

        if (ctval3 > 6)
        {
            ctval4 = 1220;
            cval5 = 2990;
        }
        else
        {
            ctval4 = 1020;
            cval5 = 2650;
        }
        string Acrdemicyear1 = da.GetFunction("select value from master_settings where settings='Academic year'");
        string[] spt1 = Acrdemicyear1.Split(',');
        string acdyr = "(" + spt1[0] + " - " + spt1[1] + ")";
        html.Append("<div style=' page-break-after: always;' ><center><table style='width: 1701px;'><tr><td style='text-align:center;width: 1701px;font-size:40px;' Font-Family='Arial Black'><b>" + colgnam.ToUpper() + "</b></td></tr><tr><td style='text-align:center;width: 1701px;font-size:x-large;' Font-Family='Arial Black'><b>" + affilat.ToUpper() + "</b></td></tr><tr><td style='text-align:center;font-size:40px;width: 1701px;' Font-Family='Arial Black'><b>" + testName.ToUpper() + "</b></td></tr><tr><td style='text-align:center;width: 1701px;font-size:40px;'  Font-Family='Arial Black' ><b>CONSOLIDATED STATEMENT OF MARKS</b></td></tr><tr><td style='text-align:center;width: 1701px; font-size:x-large;'  Font-Family='Arial Black'><b>" + acdyr + "</b></td></tr></table></center><table style='margin-bottom:-30px;margin-left:20px;width:" + cval5 + "px; margin-top:25px'><tr></tr><tr></tr><tr><td style='text-align:left;width:" + ctval4 + "px;font-size:30px'  Font-Names='Times New Roman'>NAME OF THE CLASS TEACHER:" + classadvi + "</td><td style='text-align:center;width:570px;font-size:30px'  Font-Names='Times New Roman'>CLASS:" + ddlBranch.SelectedItem.Text.ToString() + sections + "</td><td style='width:10px'></td><td style='text-align:right;font-size:30px'  Font-Names='Times New Roman'>STRENGTH:" + studstrg + "</td></tr></table>");


        if (ddlCollege.Items.Count == 0)
        {
            lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        else
        {
            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
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
            batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and r.Batch_Year in(" + batchYear + ")";
                qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
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
            courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
        }
        if (ddlBranch.Items.Count == 0)
        {
            lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        else
        {
            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
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
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(semester))
            {
                qrySemester = " and r.current_semester in(" + semester + ")";
                qrySemester1 = " and srh.semester in(" + semester + ")";
            }
        }
        if (ddlSec.Items.Count > 0 && ddlSec.Enabled)
        {
            string secValue = Convert.ToString(ddlSec.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
            {
                section = secValue;
                qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + secValue + "')";
                qrySection1 = " and LTRIM(RTRIM(ISNULL(ss.Sections,''))) in('" + secValue + "')";
            }
        }

        else if (cblTest.Items.Count > 0 && txtTest.Visible)
        {
            testNo = getCblSelectedValue(cblTest);
            testName = getCblSelectedText(cblTest);
            if (!string.IsNullOrEmpty(testNo))
            {
                qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            }
            else
            {
                lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
        }
        else
        {
            lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (cblSubject.Items.Count > 0 && txtSubject.Visible)
        {
            foreach (ListItem li in cblSubject.Items)
            {
                string subjectValue = li.Value;
                if (li.Selected)//ISNULL(ss.isSingleSubject,'0')
                {
                    string[] s = subjectValue.Split(new string[] { "$mr$" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var val in s)
                    {
                        if (!string.IsNullOrEmpty(subjectCode))
                        {
                            subjectCode += ",'" + val + "'";
                        }
                        else
                        {
                            subjectCode = "'" + val + "'";
                        }
                    }

                }
            }
            qrySubjectCode = string.Empty;
            if (!string.IsNullOrEmpty(subjectCode))
            {
                qrySubjectCode = " and s.subject_code in(" + subjectCode + ")";
            }
            else
            {
                lblAlertMsg.Text = "Please Select " + lblSubject.Text.Trim() + " And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
        }
        else if (ddlSubject.Items.Count > 0 && ddlSubject.Visible)
        {
            subjectCode = string.Empty;
            string subjectValue = Convert.ToString(ddlSubject.SelectedValue).Trim();
            string[] s = subjectValue.Split(new string[] { "$mr$" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in s)
            {
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    subjectCode += ",'" + val + "'";
                }
                else
                {
                    subjectCode = "'" + val + "'";
                }
            }
            if (!string.IsNullOrEmpty(subjectCode))
            {
                qrySubjectCode = " and s.subject_code in('" + subjectCode + "')";
            }
        }
        else
        {
            lblAlertMsg.Text = "No " + lblSubject.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        string convertMark = string.Empty;
        double convertedMark = 0;
        if (chkConvertedTo.Checked)
        {
            convertMark = Convert.ToString(txtConvertedMaxMark.Text).Trim();
            double.TryParse(convertMark, out convertedMark);
            if (string.IsNullOrEmpty(convertMark))
            {
                lblAlertMsg.Text = "Please Enter Converted Mark";
                divPopAlert.Visible = true;
                return;
            }
            else if (!double.TryParse(convertMark, out convertedMark))
            {
                lblAlertMsg.Text = "Please Enter Valid Converted Mark";
                divPopAlert.Visible = true;
                return;
            }
            else if (convertedMark <= 0)
            {
                lblAlertMsg.Text = "Converted Mark Must Be Greater Than Zero!!!";
                divPopAlert.Visible = true;
                return;
            }
        }
        string studentAppNo = string.Empty;
        studentAppNo = string.Empty;
        double maximumTestMarks = 0;
        DataTable dtGeneralGrade = new DataTable();
        DataTable dtStaffDetails = new DataTable();
        DataTable dtSubjectCount = new DataTable();
        DataTable dtSubSubjectMarkList = new DataTable();
        DataTable dtSubSubjectMarkDetails = new DataTable();
        DataTable dtBestSubjects = new DataTable();
        int comsub = 0;
        int def = 0;

        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo) && !string.IsNullOrEmpty(qrySubjectCode) && !string.IsNullOrEmpty(subjectCode))
        {


            string SelectQ = "select * from CamBestCalc where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "'";
            dtBestSubjects = dirAcc.selectDataTable(SelectQ);
            if (dtBestSubjects.Rows.Count > 0)
            {
                string com = Convert.ToString(dtBestSubjects.Rows[0]["defaultSubjects"]);
                string deft = Convert.ToString(dtBestSubjects.Rows[0]["Bestofsubjects"]);
                if (com != "")
                {
                    string[] cCount = com.Split(',');
                    int.TryParse(cCount.Length.ToString(), out def);
                }

                string[] dCount = deft.Split(',');
                string totCount = Convert.ToString(dtBestSubjects.Rows[0]["noofBest"]);
                int.TryParse(totCount, out comsub);

            }
            dicQueryParameter.Clear();
            dicQueryParameter.Add("appNo", studentAppNo);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("degreeCode", degreeCode);
            dicQueryParameter.Add("semester", semester);
            dicQueryParameter.Add("section", section);
            dicQueryParameter.Add("testNo", testNo);
            dicQueryParameter.Add("redoType", "2");
            //dtStudentMarks = storeAcc.selectDataTable("uspGetStudentPreviousMarks", dicQueryParameter);
            convertedMark = ((convertedMark > 0) ? convertedMark : 100);
            qry = "SELECT Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Roll_No),''))) end Roll_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.college_code),''))) end college_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Reg_No),''))) end Reg_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Batch_Year),''))) end Batch_Year,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.degree_code),''))) end degree_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Current_Semester),''))) end semester,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Sections),''))) end ClassSection,LTRIM(RTRIM(ISNULL(Convert(varchar(500),e.sections),''))) as ExamSection,a.app_formno as ApplicationNo,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'' and LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'01/01/1900' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.admissionDate,103),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Adm_Date,103),''))) end AdmissionDate,r.Stud_Name,r.Stud_Type,r.Roll_Admit,ISNULL(r.serialno,'0') as serialno,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,case when a.sex='0' then 'Male' when a.sex='1' then 'Female' else 'Transgender' end as Gender,ss.subject_type,ss.subType_no,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,CAST(ISNULL(e.min_mark,'0') as float) as ConductedMinMark,CAST(ISNULL(e.max_mark,'0') as float) as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as RetestMark,case when (ISNULL(re.marks_obtained,'0')>='0' and ISNULL(re.marks_obtained,'0')>=ISNULL(e.min_mark,'0')) then 'Pass' when ISNULL(re.marks_obtained,'0')='-1' then 'AAA' else 'Fail' end as Result,CAST(case when ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'')<>'' and ISNULL(re.marks_obtained,'0')>=0 and ISNULL(CONVERT(VARCHAR(100),e.max_mark),'')<>'' and ISNULL(e.max_mark,'0')>0 then ROUND(ISNULL(re.marks_obtained,'0')/ ISNULL(e.max_mark,'0') * " + ((convertedMark > 0) ? convertedMark.ToString() : "100") + ", " + ((chkRoundOffMarks.Checked) ? "0" : "1") + ")  else ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') end as float) OutOffMarks,ISNULL(ss.isSingleSubject,'0') as Single FROM CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s,applyn a,Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where ss.syll_code=s.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and r.App_No=a.app_no and s.subject_no=e.subject_no and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySection + qrytestNo + qrySubjectCode + " and isnull(srh.isLatest,'1')='1'";//srh.isLatest='1'
            dtStudentMarks = dirAcc.selectDataTable(qry);
            qry = "select distinct ISNULL(e.max_mark,'0') as max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year=e.batch_year and sm.Batch_Year in(" + batchYear + ") and sm.degree_code in(" + degreeCode + ") and sm.semester in(" + semester + ")" + qrySection + qrytestNo + qrySubjectCode + "";
            string maximumTestMark = dirAcc.selectScalarString(qry);
            double.TryParse(maximumTestMark, out maximumTestMarks);
            maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;

            string qry2 = "select distinct s.subjectId,s.subSubjectName,subject_no,s.minMark,s.maxMark from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and criteria_no='" + testNo + "' " + qrySection;
            dtSubSubjectMarkList = dirAcc.selectDataTable(qry2);// and subject_no='" + subjectNos + "'

            qry2 = "select s.subjectId,s.subSubjectName,e.subject_no,s.minMark,s.maxMark,criteria_no,sm.*  from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and criteria_no='" + testNo + "' " + qrySection;
            dtSubSubjectMarkDetails = dirAcc.selectDataTable(qry2);

            qry = "select sm.Batch_Year,sm.degree_code,sm.semester,LTRIM(RTRIM(ISNULL(ss.Sections,''))) as Sections,s.subject_no,s.subject_code,s.acronym,ss.staff_code,sfm.staff_name from staff_selector ss,Syllabus_master sm,subject s,staffmaster sfm where s.syll_code=sm.syll_code and s.subject_no=ss.subject_no and ss.batch_year=sm.Batch_Year and sfm.staff_code=ss.staff_code and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySection1 + qrySubjectCode;
            dtStaffDetails = dirAcc.selectDataTable(qry);

            qry = "select case when ISNULL(c.App_no,'')<>'' then c.App_no when ISNULL(s.App_no,'')<>'' then s.App_no end as App_no,ISNULL(c.totalSubject,'0')+ISNULL(s.totalSubject,'0') as totalSubject from (select Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Count(distinct sc.subject_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by r.App_No,srh.App_no) as c  full join (select Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Count(distinct ss.subType_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by r.App_No,srh.App_no ) as s on c.App_no=s.App_no order by s.App_No";
            dtSubjectCount = dirAcc.selectDataTable(qry);

            qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' ";
            //order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc
            dtGradeDetails = dirAcc.selectDataTable(qry);
            if (dtGradeDetails.Rows.Count > 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
        }

        if (dtStudentMarks.Rows.Count > 0)
        {
            DataTable dtStudMarks = new DataTable();
            dtStudMarks.Columns.Add("app_no", typeof(long));
            dtStudMarks.Columns.Add("subject_type", typeof(string));
            dtStudMarks.Columns.Add("subject_name", typeof(string));
            dtStudMarks.Columns.Add("subject_code", typeof(string));
            dtStudMarks.Columns.Add("subject_no", typeof(string));
            dtStudMarks.Columns.Add("ApplicationNo", typeof(string));
            dtStudMarks.Columns.Add("AdmissionDate", typeof(string));
            dtStudMarks.Columns.Add("Roll_No", typeof(string));
            dtStudMarks.Columns.Add("Reg_No", typeof(string));
            dtStudMarks.Columns.Add("Roll_Admit", typeof(string));
            dtStudMarks.Columns.Add("serialno", typeof(string));
            dtStudMarks.Columns.Add("Stud_Name", typeof(string));
            dtStudMarks.Columns.Add("Stud_Type", typeof(string));
            dtStudMarks.Columns.Add("ClassSection", typeof(string));
            dtStudMarks.Columns.Add("ExamSection", typeof(string));
            dtStudMarks.Columns.Add("Gender", typeof(string));
            dtStudMarks.Columns.Add("Batch_Year", typeof(string));
            dtStudMarks.Columns.Add("college_code", typeof(string));
            dtStudMarks.Columns.Add("degree_code", typeof(string));
            dtStudMarks.Columns.Add("semester", typeof(string));
            dtStudMarks.Columns.Add("TestName", typeof(string));
            dtStudMarks.Columns.Add("TestNo", typeof(string));
            dtStudMarks.Columns.Add("TestMark", typeof(decimal));
            dtStudMarks.Columns.Add("ConductedMaxMark", typeof(decimal));
            dtStudMarks.Columns.Add("ConductedMinMark", typeof(decimal));
            dtStudMarks.Columns.Add("OutOffMarks", typeof(decimal));




            DataTable dtDistinctStudents = new DataTable();
            dtStudentMarks.DefaultView.Sort = orderByStudents(collegeCode, includeOrderBy: 1);
            dtDistinctStudents = dtStudentMarks.DefaultView.ToTable(true, "App_no", "Roll_No", "Reg_No", "ApplicationNo", "Stud_Type", "Roll_Admit", "serialno");
            DataTable dtDistinctSubject = new DataTable();
            DataTable dtDistinctSubjectTypeSingle = new DataTable();
            DataTable dtDistinctSubjectSingle = new DataTable();
            dtStudentMarks.DefaultView.RowFilter = "Single=0";
            dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
            dtDistinctSubject = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name", "subjectpriority", "TestMaxMark", "Single");


            dtStudentMarks.DefaultView.RowFilter = "Single=1";
            dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
            dtDistinctSubjectTypeSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_type", "subType_no", "Single");

            dtStudentMarks.DefaultView.RowFilter = "Single=1";
            dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
            dtDistinctSubjectSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_type", "subType_no", "subject_code", "subject_name", "subjectpriority", "Single");
            int spanStartColumn = 0;
            // Init_Spread(FpStudentMarkList, ref spanStartColumn, 0);

            int serialNo = 0;
            object count = 0;
            double subjectHeighestMarks = 0;
            double subjectLeastMarks = 0;
            double subjectAverage = 0;
            double absenteesCount = 0;
            double appearedCount = 0;
            double subjectTotal = 0;

            Dictionary<string, double> dicSubjectWiseLeastMark = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectWiseHieghestMark = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectWiseTotalMark = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectWiseAverageMark = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectWiseAppearedCount = new Dictionary<string, double>();
            Dictionary<string, double> dicSubjectWiseAbsentCount = new Dictionary<string, double>();
            Dictionary<string, double> dicInvariationofSubjectMarkEntry = new Dictionary<string, double>();
            Dictionary<string, double> dicFailedAbsentStudents = new Dictionary<string, double>();

            int totalColumnValue = 0;
            int rankColumnValue = 0;
            int attendanceColumnValue = 0;
            int remarkColumnValue = 0;
            DataTable dtColumnHeader = new DataTable();
            dtColumnHeader.Columns.Add("subject_name", typeof(string));
            dtColumnHeader.Columns.Add("subject_code", typeof(string));
            dtColumnHeader.Columns.Add("subject_no", typeof(string));
            dtColumnHeader.Columns.Add("subjectpriority", typeof(int));
            dtColumnHeader.Columns.Add("Single", typeof(int));
            dtColumnHeader.Columns.Add("DisplayOutOffMarks", typeof(double));
            dtColumnHeader.Columns.Add("MaxMark", typeof(double));
            DataRow drSubjectColumn;
            foreach (DataRow drSubjects in dtDistinctSubject.Rows)
            {
                count = 0;
                subjectHeighestMarks = 0;
                subjectLeastMarks = 0;
                subjectAverage = 0;
                absenteesCount = 0;
                appearedCount = 0;
                subjectTotal = 0;

                string subjectCodes = Convert.ToString(drSubjects["subject_code"]).Trim();
                string subjectNos = Convert.ToString(drSubjects["subject_no"]).Trim();
                string subjectNames = Convert.ToString(drSubjects["subject_name"]).Trim();
                string subjectPriority = Convert.ToString(drSubjects["subjectpriority"]).Trim();
                string maxmarkct = Convert.ToString(drSubjects["TestMaxMark"]).Trim();

                count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                if (!dicSubjectWiseLeastMark.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseLeastMark.Add(subjectNos.Trim(), subjectLeastMarks);

                count = dtStudentMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                if (!dicSubjectWiseHieghestMark.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseHieghestMark.Add(subjectNos.Trim(), subjectHeighestMarks);

                count = dtStudentMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                if (!dicSubjectWiseTotalMark.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseTotalMark.Add(subjectNos.Trim(), subjectTotal);

                count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                if (!dicSubjectWiseAppearedCount.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseAppearedCount.Add(subjectNos.Trim(), appearedCount);

                count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks='-1' and subject_no='" + subjectNos + "'");
                double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                if (!dicSubjectWiseAbsentCount.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseAbsentCount.Add(subjectNos.Trim(), absenteesCount);

                if (subjectTotal > 0 && appearedCount > 0)
                    subjectAverage = subjectTotal / appearedCount;
                subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                if (!dicSubjectWiseAverageMark.ContainsKey(subjectNos.Trim()))
                    dicSubjectWiseAverageMark.Add(subjectNos.Trim(), subjectAverage);

                convertMark = txtConvertedMaxMark.Text;
                double convertedMax = 0;
                double.TryParse(convertMark.Trim(), out convertedMax);
                string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                drSubjectColumn = dtColumnHeader.NewRow();
                drSubjectColumn["subject_name"] = subjectNames;
                drSubjectColumn["subject_code"] = subjectCodes;
                drSubjectColumn["subject_no"] = subjectNos;
                drSubjectColumn["subjectpriority"] = subjectPriority;
                drSubjectColumn["Single"] = "0";
                drSubjectColumn["DisplayOutOffMarks"] = convertedMax;
                drSubjectColumn["MaxMark"] = maxmarkct;
                dtColumnHeader.Rows.Add(drSubjectColumn);

            }
            if (dtDistinctSubjectTypeSingle.Rows.Count > 0)
            {
                foreach (DataRow drSubjectType in dtDistinctSubjectTypeSingle.Rows)
                {
                    count = 0;
                    subjectHeighestMarks = 0;
                    subjectLeastMarks = 0;
                    subjectAverage = 0;
                    absenteesCount = 0;
                    appearedCount = 0;
                    subjectTotal = 0;

                    string subjectType = Convert.ToString(drSubjectType["subject_type"]).Trim();
                    string subjectTypeNo = Convert.ToString(drSubjectType["subType_no"]).Trim();
                    string subjectNoList = string.Empty;
                    string subjectCodeList = string.Empty;
                    double minimumMark = 0;
                    double maximumMark = 0;
                    DataTable dtSingleMarks = new DataTable();

                    DataTable dtSingleSubject = new DataTable();
                    dtDistinctSubjectSingle.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                    dtSingleSubject = dtDistinctSubjectSingle.DefaultView.ToTable();
                    List<string> list = dtSingleSubject.AsEnumerable().Select(r => Convert.ToString(r.Field<decimal>("subject_no"))).ToList();
                    subjectNoList = string.Join(",", list.ToArray());
                    list = dtSingleSubject.AsEnumerable().Select(r => Convert.ToString(r.Field<string>("subject_code"))).ToList();
                    subjectCodeList = string.Join(",", list.ToArray());
                    int subjectPriority = 0;
                    count = dtSingleSubject.Compute("MIN(subjectpriority)", "subjectpriority>=0");
                    int.TryParse(Convert.ToString(count).Trim(), out subjectPriority);

                    dtStudentMarks.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                    dtSingleMarks = dtStudentMarks.DefaultView.ToTable();
                    DataTable dtDistinctStud = dtStudentMarks.DefaultView.ToTable(true, "app_no");
                    DataRow drMarks;
                    Dictionary<string, double> dicTestMarks = new Dictionary<string, double>();
                    Dictionary<string, double> dicConductedMaxMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicConductedMinMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicOutOffMarks = new Dictionary<string, double>();

                    foreach (DataRow drMark in dtDistinctStud.Rows)
                    {
                        DataTable dtMarks = new DataTable();


                        dtSingleMarks.DefaultView.RowFilter = "app_no='" + Convert.ToString(drMark["app_no"]).Trim() + "'";
                        dtMarks = dtSingleMarks.DefaultView.ToTable();
                        double mark = 0;
                        double outoff100 = 0;
                        double TestMark = 0;
                        double ConductedMaxMark = 0;
                        double ConductedMinMark = 0;
                        if (dtMarks.Rows.Count > 0)
                        {
                            drMarks = dtStudMarks.NewRow();
                            drMarks["app_no"] = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                            drMarks["subject_type"] = Convert.ToString(dtMarks.Rows[0]["subject_type"]).Trim();
                            drMarks["subject_name"] = Convert.ToString(dtMarks.Rows[0]["subject_type"]).Trim();
                            drMarks["subject_code"] = subjectCodeList;
                            drMarks["subject_no"] = subjectNoList;

                            drMarks["ApplicationNo"] = Convert.ToString(dtMarks.Rows[0]["ApplicationNo"]).Trim();
                            drMarks["AdmissionDate"] = Convert.ToString(dtMarks.Rows[0]["AdmissionDate"]).Trim();
                            drMarks["Roll_No"] = Convert.ToString(dtMarks.Rows[0]["Roll_No"]).Trim();
                            drMarks["Reg_No"] = Convert.ToString(dtMarks.Rows[0]["Reg_No"]).Trim();
                            drMarks["Roll_Admit"] = Convert.ToString(dtMarks.Rows[0]["Roll_Admit"]).Trim();
                            drMarks["serialno"] = Convert.ToString(dtMarks.Rows[0]["serialno"]).Trim();
                            drMarks["Stud_Name"] = Convert.ToString(dtMarks.Rows[0]["Stud_Name"]).Trim();
                            drMarks["Stud_Type"] = Convert.ToString(dtMarks.Rows[0]["Stud_Type"]).Trim();
                            drMarks["ClassSection"] = Convert.ToString(dtMarks.Rows[0]["ClassSection"]).Trim();
                            drMarks["ExamSection"] = Convert.ToString(dtMarks.Rows[0]["ExamSection"]).Trim();
                            drMarks["Gender"] = Convert.ToString(dtMarks.Rows[0]["Gender"]).Trim();
                            drMarks["Batch_Year"] = Convert.ToString(dtMarks.Rows[0]["Batch_Year"]).Trim();
                            drMarks["college_code"] = Convert.ToString(dtMarks.Rows[0]["college_code"]).Trim();
                            drMarks["degree_code"] = Convert.ToString(dtMarks.Rows[0]["degree_code"]).Trim();

                            drMarks["semester"] = Convert.ToString(dtMarks.Rows[0]["semester"]).Trim();
                            drMarks["TestName"] = Convert.ToString(dtMarks.Rows[0]["TestName"]).Trim();
                            drMarks["TestNo"] = Convert.ToString(dtMarks.Rows[0]["TestNo"]).Trim();


                            #region Added on 9/12/2017 by prabhakaran

                            DataTable dtSubMarkFilter = new DataTable();

                            string app_No_Stud = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                            int StudentMarkEnteredSubject = 0;
                            dtStudentMarks.DefaultView.RowFilter = "App_no='" + app_No_Stud + "'";
                            StudentMarkEnteredSubject = dtStudentMarks.DefaultView.Count;

                            int StudentRegisteredSubject = 0;
                            dtSubjectCount.DefaultView.RowFilter = "App_no='" + app_No_Stud + "'";
                            DataView dvStuRegistered = dtSubjectCount.DefaultView;
                            if (dvStuRegistered.Count > 0)
                                StudentRegisteredSubject = Convert.ToInt32(dvStuRegistered[0]["totalSubject"]);

                            int differenceCount = StudentRegisteredSubject - StudentMarkEnteredSubject;
                            if (StudentRegisteredSubject > StudentMarkEnteredSubject)
                                if (!dicInvariationofSubjectMarkEntry.ContainsKey(app_No_Stud))
                                    dicInvariationofSubjectMarkEntry.Add(app_No_Stud, differenceCount);

                            #endregion

                            object sum = dtMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                            double.TryParse(Convert.ToString(sum).Trim(), out outoff100);
                            if (outoff100 == 0)
                            {
                                sum = dtMarks.Compute("MIN(OutOffMarks)", "OutOffMarks<0");
                                double.TryParse(Convert.ToString(sum).Trim(), out outoff100);
                            }
                            double maxTotal = ((convertedMark > 0) ? convertedMark * dtMarks.Rows.Count : 100 * dtMarks.Rows.Count);
                            if (maxTotal > 0 && outoff100 > 0)
                                outoff100 = Math.Round((outoff100 / maxTotal) * convertedMark, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);

                            sum = dtMarks.Compute("SUM(ConductedMaxMark)", "ConductedMaxMark>=0");
                            double.TryParse(Convert.ToString(sum).Trim(), out ConductedMaxMark);
                            if (ConductedMaxMark == 0)
                            {
                                sum = dtMarks.Compute("MIN(ConductedMaxMark)", "ConductedMaxMark<0");
                                double.TryParse(Convert.ToString(sum).Trim(), out ConductedMaxMark);
                            }

                            sum = dtMarks.Compute("SUM(TestMark)", "TestMark>=0");
                            double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                            bool isCheck = true;
                            if (TestMark == 0)
                            {
                                sum = dtMarks.Compute("MIN(TestMark)", "TestMark<0");
                                double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                                isCheck = false;
                                outoff100 = TestMark;
                            }
                            if (isCheck)
                            {
                                outoff100 = 0;
                                if (TestMark > 0 && ConductedMaxMark > 0)
                                    outoff100 = Math.Round((TestMark / ConductedMaxMark) * 100, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                            }

                            sum = dtMarks.Compute("SUM(ConductedMinMark)", "ConductedMinMark>=0");
                            double.TryParse(Convert.ToString(sum).Trim(), out ConductedMinMark);
                            if (ConductedMinMark == 0)
                            {
                                sum = dtMarks.Compute("MIN(ConductedMinMark)", "ConductedMinMark<0");
                                double.TryParse(Convert.ToString(sum).Trim(), out ConductedMinMark);
                            }
                            drMarks["TestMark"] = Convert.ToString(TestMark).Trim();
                            drMarks["ConductedMaxMark"] = Convert.ToString(ConductedMaxMark).Trim();
                            drMarks["ConductedMinMark"] = Convert.ToString(ConductedMinMark).Trim();
                            drMarks["OutOffMarks"] = Convert.ToString(outoff100).Trim();
                            dtStudMarks.Rows.Add(drMarks);
                        }
                    }

                    count = dtStudMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                    if (!dicSubjectWiseLeastMark.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseLeastMark.Add(subjectNoList.Trim(), subjectLeastMarks);

                    count = dtStudMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                    if (!dicSubjectWiseHieghestMark.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseHieghestMark.Add(subjectNoList.Trim(), subjectHeighestMarks);

                    count = dtStudMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                    if (!dicSubjectWiseTotalMark.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseTotalMark.Add(subjectNoList.Trim(), subjectTotal);

                    count = dtStudMarks.Compute("COUNT(app_no)", "OutOffMarks>=0");
                    double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                    if (!dicSubjectWiseAppearedCount.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseAppearedCount.Add(subjectNoList.Trim(), appearedCount);

                    count = dtStudMarks.Compute("COUNT(app_no)", "OutOffMarks='-1'");
                    double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                    if (!dicSubjectWiseAbsentCount.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseAbsentCount.Add(subjectNoList.Trim(), absenteesCount);

                    if (subjectTotal > 0 && appearedCount > 0)
                        subjectAverage = subjectTotal / appearedCount;
                    subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                    if (!dicSubjectWiseAverageMark.ContainsKey(subjectNoList.Trim()))
                        dicSubjectWiseAverageMark.Add(subjectNoList.Trim(), subjectAverage);

                    convertMark = txtConvertedMaxMark.Text;
                    double convertedMax = 0;
                    double.TryParse(convertMark.Trim(), out convertedMax);
                    string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                    drSubjectColumn = dtColumnHeader.NewRow();
                    drSubjectColumn["subject_name"] = subjectType;
                    drSubjectColumn["subject_code"] = subjectCodeList;
                    drSubjectColumn["subject_no"] = subjectNoList;
                    drSubjectColumn["subjectpriority"] = subjectPriority;
                    drSubjectColumn["Single"] = "1";
                    drSubjectColumn["DisplayOutOffMarks"] = convertedMax;
                    dtColumnHeader.Rows.Add(drSubjectColumn);
                }
            }
            subcount = 0;
            subcount1 = 0;
            int col1head = 0;
            int widsz = 0;
            int colheadct = coluheadcount1;
            if (colheadct > 18)
            {
                col1head = 1250;

                //if (chkConvertedTo.Checked == true)
                //    col1head = 1900;
                //else
                //    col1head = 1600;
            }
            else
            {
                col1head = 1250;
            }
            if (dtColumnHeader.Rows.Count > 0)
            {
                html.Append("<center><table border='2px' cellspacing='0' cellpadding='5'  style='margin-top: 40px ; border: 2px solid black; height:" + col1head + "px ; width:2800px;margin-left:22px'><tr><td rowspan ='2px' style='text-align:center;font-size:25px;font-family:Arial;'><b>S.NO</b></td><td rowspan ='2px' style='text-align:center; font-size:25px;font-family:Arial;' ><b>Exam No</b></td><td rowspan='2px' style='text-align:center; font-size:25px;font-family:Arial;'><b>NAME OF THE STUDENT</b></td>");

                // html.Append("<table><tr><td>S.NO</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>ROLL NO</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>STUDENT NAME</td>");
                DataTable dtHeading = new DataTable();
                //dtColumnHeader.DefaultView.RowFilter = "";
                dtColumnHeader.DefaultView.Sort = "subjectpriority";
                dtHeading = dtColumnHeader.DefaultView.ToTable();
                int cols = 0;
                foreach (DataRow drNewColumns in dtHeading.Rows)
                {

                    cols++;
                    string subjectCodes = Convert.ToString(drNewColumns["subject_code"]).Trim();
                    string subjectNos = Convert.ToString(drNewColumns["subject_no"]).Trim();
                    string subjectNames = Convert.ToString(drNewColumns["subject_name"]).Trim();
                    string subjectPriority = Convert.ToString(drNewColumns["subjectpriority"]).Trim();
                    string Single = Convert.ToString(drNewColumns["Single"]).Trim();
                    //asdasdasd
                    string displayOutof100 = Convert.ToString(drNewColumns["DisplayOutOffMarks"]).Trim();
                    string maxct = Convert.ToString(drNewColumns["MaxMark"]).Trim();
                    if (string.IsNullOrEmpty(maxct))
                        maxct = "0";
                    subcount = subcount + Convert.ToInt32(maxct);
                    subcount1++;
                    DataTable dt = new DataTable();
                    if (dtSubSubjectMarkList.Rows.Count > 0)
                    {
                        dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNos + "'";
                        dt = dtSubSubjectMarkList.DefaultView.ToTable();
                    }

                    int colsp = 0;
                    if (chkIncludeGrade.Checked)
                    {
                        colsp = 1;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        colsp += dt.Rows.Count;
                    }
                    colsp += 1;
                    //int sub_no=Convert.ToInt32(subjectNos);
                    dicsubnam.Add(cols, subjectNames);
                    dicsubno.Add(cols, subjectNos);
                    html.Append("<td colspan='" + colsp + "px' style='text-align:center; font-size:25px;font-family:Arial;'><b>" + subjectNames + "</b></td>");

                    //  subcount++;

                    if (dt.Rows.Count > 0)
                    {
                        // html.Append("<")
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            cols++;
                            string subjName = Convert.ToString(dt.Rows[row]["subSubjectName"]).ToUpper();
                            string subjId = Convert.ToString(dt.Rows[row]["subjectId"]);

                            diccolspan.Add(cols, subjName);

                        }
                    }

                    #region modified on 18/9/17

                    bool ISTRUE = true;
                    string[] exmcode = null;
                    string[] appno = null;
                    int i = 0;

                    #endregion


                    convertMark = txtConvertedMaxMark.Text;
                    double convertedMax = 0;
                    double.TryParse(displayOutof100.Trim(), out convertedMax);
                    displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";

                    if (chkConvertedTo.Checked)
                    {
                        cols++;
                        diccolspan.Add(cols, "MARK\n" + displayOutof100);
                    }
                    else
                    {
                        cols++;
                        diccolspan.Add(cols, "MARKS");
                    }
                    if (chkIncludeGrade.Checked)
                    {
                        cols++;
                        diccolspan.Add(cols, "GRADE");
                    }
                }

            }
            int dissubcount = subcount;
            int dissubct = subcount1;

            Dictionary<string, int> dicStudentSubjectCount = new Dictionary<string, int>();
            int maxSubjectCount = 0;
            if (dtSubjectCount.Rows.Count > 0)
            {
                foreach (DataRow drSubject in dtSubjectCount.Rows)
                {
                    string rollNos = Convert.ToString(drSubject["App_no"]).Trim();
                    string subjectCount = Convert.ToString(drSubject["totalSubject"]).Trim();
                    int allotedSubjectCount = 0;
                    if (int.TryParse(subjectCount.Trim(), out allotedSubjectCount))
                        if (!dicStudentSubjectCount.ContainsKey(rollNos.Trim()))
                            dicStudentSubjectCount.Add(rollNos.Trim(), allotedSubjectCount);
                    if (maxSubjectCount < allotedSubjectCount)
                        maxSubjectCount = allotedSubjectCount;
                    //else
                    //    dicStudentPassedTotalOutof100[rollNos.Trim()] += allotedSubjectCount;
                }
            }
            if (chkConvertedTo.Checked)
            {
                dissubcount = dissubct;
            }
            else
            {
                dissubcount = subcount;
            }
            totval = "TOTAL\n " + ((dissubcount != 0) ? "(" + Convert.ToString((dissubcount * ((chkConvertedTo.Checked) ? convertedMark : 1))) + ")" : "");
            //html.Append("<td rowspan='2px' style='text-align:center; font-size:large' Font-Bold='true' Font-Names='Times New Roman'><b>" + totval + "</b></td>");

            if (dtBestSubjects.Rows.Count > 0 && comsub != 0)
            {
                // totVar = (comsub + def) * 100;
                totVar = (comsub) * 100;
                totval = "Total\n " + totVar.ToString();

                //  FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 4].Text = "Total " + totVar.ToString();
            }
            html.Append("<td rowspan='2px' style='text-align:center; font-size:25px;font-family:Arial ;'><b>" + totval + "</b></td>");
            html.Append("<td rowspan='2px' style='text-align:center; font-size:25px;font-family:Arial ;'><b>RANK</b></td>");
            if (cbattndperc.Checked == true)
            {
                html.Append("<td rowspan='2px' style='text-align:center; font-size:25px;font-family:Arial ;'><b>ATT%</b></td>");
            }
            html.Append("</tr>");

            if (diccolspan.Count > 0)
            {
                html.Append("<tr>");
                foreach (KeyValuePair<int, string> dc in diccolspan)
                {
                    string colval = dc.Value;
                    html.Append("<td style='text-align:center; font-size:25px;font-family:Arial ;' ><b>" + colval + "</b></td>");

                }
                html.Append("</tr>");
            }

            Dictionary<byte, double> dicOverall = new Dictionary<byte, double>();
            count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 ");
            double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
            dicOverall.Add(0, subjectLeastMarks);

            count = dtStudentMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0 ");
            double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
            dicOverall.Add(1, subjectHeighestMarks);

            count = dtStudentMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
            double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
            dicOverall.Add(2, subjectTotal);

            count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks>=0");
            double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
            dicOverall.Add(3, appearedCount);

            count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks='-1'");
            double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
            dicOverall.Add(4, absenteesCount);

            if (subjectTotal > 0 && appearedCount > 0)
                subjectAverage = subjectTotal / appearedCount;
            subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
            dicOverall.Add(5, subjectAverage);
            Dictionary<string, int> dicGradeWiseCount = new Dictionary<string, int>();

            Dictionary<string, int> dicGradeWiseCountForDefault = new Dictionary<string, int>();

            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            int testCount = 0;
            //  int endColumn = FpStudentMarkList.Sheets[0].ColumnCount - 1;
            int startingRows = 0;
            Dictionary<string, double> dicStudentTotal = new Dictionary<string, double>();
            Dictionary<string, double> dicStudentPassedTotal = new Dictionary<string, double>();
            Dictionary<string, double> dicStudentTotalOutof100 = new Dictionary<string, double>();
            Dictionary<string, double> dicStudentPassedTotalOutof100 = new Dictionary<string, double>();

            Dictionary<string, int> dicStudentPassedSubjectCount = new Dictionary<string, int>();

            Dictionary<string, double> dicStudentPassedAverage = new Dictionary<string, double>();
            Dictionary<string, double> dicStudentPassedAverageOutof100 = new Dictionary<string, double>();

            DataTable dtGradeMarkRanges = new DataTable();
            dtGradeMarkRanges.Columns.Add("Mark_Grade");
            dtGradeMarkRanges.Columns.Add("Frange", typeof(decimal));
            dtGradeMarkRanges.Columns.Add("Trange", typeof(decimal));
            dtGradeMarkRanges.Columns.Add("Ranges");

            dtGradeMarkRanges.Rows.Add("", "95", "100", "95 - 100");
            dtGradeMarkRanges.Rows.Add("", "90", "94.99", "90 - 94");
            dtGradeMarkRanges.Rows.Add("", "75", "89.99", "75 - 89");
            dtGradeMarkRanges.Rows.Add("", "60", "74.99", "60 - 74");
            dtGradeMarkRanges.Rows.Add("", "40", "59.99", "40 - 59");
            dtGradeMarkRanges.Rows.Add("", "0", "39.99", "Below 40");
            dtGradeMarkRanges.Rows.Add("", "0", "34.99", "No.of.Failure (Below 35)");
            // FpStudentMarkList.SaveChanges();
            bool brkpg = false;
            bool hednam = false;
            int sno4 = 0;
            int pg_sz = 15;
            bool pgsize = false;
            int cout = 0;
            if (cbattndperc.Checked == true)
            {
                cout = FpStudentMarkList.Sheets[0].Columns.Count - 1;
            }
            else
            {
                cout = FpStudentMarkList.Sheets[0].Columns.Count - 2;
            }
            for (int j2 = 0; j2 < FpStudentMarkList.Sheets[0].Rows.Count; j2++)
            {
                sno4++;
                html.Append("<tr>");

                for (int k2 = 0; k2 < cout; k2++)
                {
                    //  string colna1 = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[j2, k2].Text);
                    string colnam1 = Convert.ToString(FpStudentMarkList.Sheets[0].Cells[j2, k2].Tag);
                    FpSpreadViewSubjects.Sheets[0].Columns[2].Visible = false;
                    if (k2 == 3 || k2 == 2 || k2 == 4)
                    {
                        hednam = true;
                    }
                    else
                    {
                        hednam = false;
                    }

                    if (colnam1.ToLower() == "break page")
                    {
                        brkpg = true;
                    }
                    if (brkpg == false)
                    {
                        if (sno4 >= pg_sz)
                        {
                            pgsize = true;
                            pg_sz = 25;
                            html.Append("</table></center></div><div style=' page-break-after: always;'>");
                            if (dtColumnHeader.Rows.Count > 0)
                            {
                                html.Append("<center><table border='2px' cellpadding='7px' cellspacing='0px' style='border-collapse:collapse;border:2px solid black ;height: " + col1head + "px;width:2800px;  margin-top: 50px;margin-left:22px '><tr><td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>S.NO</b></td><td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>Exam No</b></td><td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>NAME OF THE STUDENT</b></td>");
                                DataTable dtHeading = new DataTable();
                                dtColumnHeader.DefaultView.Sort = "subjectpriority";
                                dtHeading = dtColumnHeader.DefaultView.ToTable();
                                int cols = 0;
                                foreach (DataRow drNewColumns in dtHeading.Rows)
                                {

                                    cols++;
                                    string subjectCodes = Convert.ToString(drNewColumns["subject_code"]).Trim();
                                    string subjectNos = Convert.ToString(drNewColumns["subject_no"]).Trim();
                                    string subjectNames = Convert.ToString(drNewColumns["subject_name"]).Trim();
                                    string subjectPriority = Convert.ToString(drNewColumns["subjectpriority"]).Trim();
                                    string Single = Convert.ToString(drNewColumns["Single"]).Trim();
                                    //asdasdasd
                                    string displayOutof100 = Convert.ToString(drNewColumns["DisplayOutOffMarks"]).Trim();

                                    DataTable dt = new DataTable();
                                    if (dtSubSubjectMarkList.Rows.Count > 0)
                                    {
                                        dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNos + "'";
                                        dt = dtSubSubjectMarkList.DefaultView.ToTable();
                                    }

                                    int colsp = 0;
                                    if (chkIncludeGrade.Checked)
                                    {
                                        colsp = 1;
                                    }

                                    if (dt.Rows.Count > 0)
                                    {
                                        colsp += dt.Rows.Count;
                                    }
                                    colsp += 1;

                                    html.Append("<td colspan='" + colsp + "px' style='text-align:center;font-size:25px;font-family:Arial'><b>" + subjectNames + "</b></td>");

                                    subcount++;

                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int row = 0; row < dt.Rows.Count; row++)
                                        {
                                            cols++;
                                            string subjName = Convert.ToString(dt.Rows[row]["subSubjectName"]);
                                            string subjId = Convert.ToString(dt.Rows[row]["subjectId"]);


                                        }
                                    }

                                    #region modified on 18/9/17

                                    bool ISTRUE = true;
                                    string[] exmcode = null;
                                    string[] appno = null;
                                    int i = 0;

                                    #endregion


                                    convertMark = txtConvertedMaxMark.Text;
                                    double convertedMax = 0;
                                    double.TryParse(displayOutof100.Trim(), out convertedMax);
                                    displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";

                                    if (chkConvertedTo.Checked)
                                    {
                                        cols++;
                                        // diccolspan.Add(cols, "MARK\n" + displayOutof100);
                                    }
                                    else
                                    {
                                        cols++;

                                    }
                                    if (chkIncludeGrade.Checked)
                                    {
                                        cols++;

                                    }
                                }

                            }
                            int dissubcount1 = subcount;

                            Dictionary<string, int> dicStudentSubjectCount1 = new Dictionary<string, int>();
                            int maxSubjectCoun1 = 0;
                            if (dtSubjectCount.Rows.Count > 0)
                            {
                                foreach (DataRow drSubject in dtSubjectCount.Rows)
                                {
                                    string rollNos = Convert.ToString(drSubject["App_no"]).Trim();
                                    string subjectCount = Convert.ToString(drSubject["totalSubject"]).Trim();
                                    int allotedSubjectCount = 0;
                                    if (int.TryParse(subjectCount.Trim(), out allotedSubjectCount))
                                        if (!dicStudentSubjectCount1.ContainsKey(rollNos.Trim()))
                                            dicStudentSubjectCount1.Add(rollNos.Trim(), allotedSubjectCount);
                                    if (maxSubjectCount < allotedSubjectCount)
                                        maxSubjectCount = allotedSubjectCount;
                                }
                            }

                            // string totval1 = "TOTAL\n " + ((dissubcount1 != 0) ? "(" + Convert.ToString((dissubcount1 * ((chkConvertedTo.Checked) ? convertedMark : 100))) + ")" : "");
                            html.Append("<td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>" + totval + "</b></td>");

                            //if (dtBestSubjects.Rows.Count > 0 && comsub != 0)
                            //{

                            //    string totv = "Total\n " + totVar.ToString();
                            //    html.Append("<td rowspan='2px' style='text-align:center;font-size:large;' Font-Bold='true' Font-Names='Times New Roman' ><b>" + totv + "</b></td>");
                            //}
                            html.Append("<td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>RANK</b></td>");
                            if (cbattndperc.Checked == true)
                            {
                                html.Append("<td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>ATT%</b></td>");
                            }
                            html.Append("</tr>");


                            if (diccolspan.Count > 0)
                            {
                                html.Append("<tr>");
                                foreach (KeyValuePair<int, string> dc in diccolspan)
                                {
                                    string colval = dc.Value;
                                    html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + colval + "</b></td>");

                                }
                                html.Append("</tr><tr>");
                            }
                            sno4 = 0;
                        }
                    }
                    if (sno4 < 21)
                    {
                        if (brkpg == false && hednam == false)
                        {
                            if (k2 == 6)
                            {
                                if (FpStudentMarkList.Sheets[0].ColumnHeader.Columns[k2].Visible == true)
                                {
                                    string colnam = Convert.ToString(FpStudentMarkList.Sheets[0].Cells[j2, k2].Text);


                                    html.Append("<td style='font-size:25px;font-family:Arial'><b>" + colnam + "</b></td>");

                                }
                            }
                            else
                            {
                                if (k2 == 1)
                                {
                                    string colnam = Convert.ToString(FpStudentMarkList.Sheets[0].Cells[j2, k2].Text);

                                    html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + colnam + "</b></td>");

                                }
                                else
                                {
                                    if (FpStudentMarkList.Sheets[0].ColumnHeader.Columns[k2].Visible == true)
                                    {
                                        string colnam = Convert.ToString(FpStudentMarkList.Sheets[0].Cells[j2, k2].Text);

                                        html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + colnam + "</b></td>");

                                    }
                                }
                            }
                        }
                    }


                }
                html.Append("</tr>");
            }
            html.Append("</table></center></div><div style=' page-break-after: always;'><div style='width:1000px;margin-left:25px;'><table border='2px' cellpadding='7px' cellspacing='7px' style='border-collapse:collapse;border:2px balck solid;height:800px;width:990px;margin-top:95px;margin-left:25'px'>");


            int sno2 = 0;
            if (dtDistinctStudents.Rows.Count > 0)
            {
                foreach (DataRow drStudent in dtDistinctStudents.Rows)
                {
                    string subjectCodeVal = string.Empty;
                    string subjectNameVal = string.Empty;
                    string subjectNoVal = string.Empty;
                    string testMark = string.Empty;
                    string testMaxMark = string.Empty;
                    string testMinMark = string.Empty;
                    double testSubMarks = 0;
                    double testMaxMarks = 0;
                    double testMinMarks = 0;
                    int subjectCount = 0;

                    int columnVal = 0;

                    string studentAppNos = Convert.ToString(drStudent["App_no"]).Trim();
                    frdate = txtFromDate.Text;
                    todate = txtToDate.Text;
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

                    per_abshrs_spl = 0;
                    tot_per_hrs_spl = 0;
                    tot_ondu_spl = 0;
                    tot_ml_spl = 0;
                    tot_conduct_hr_spl = 0;
                    per_workingdays1 = 0;
                    leavfinaeamount = 0;
                    medicalLeaveDays = 0;
                    medicalLeaveHours = 0;
                    string dum_tage_date = string.Empty;
                    string dum_tage_hrs = string.Empty;

                    #region Added on 9/12/2017 by prabhakaran

                    DataTable dtSubMarkFilter = new DataTable();

                    //string app_No_Stud = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                    int StudentMarkEnteredSubject = 0;
                    dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                    StudentMarkEnteredSubject = dtStudentMarks.DefaultView.Count;

                    int StudentRegisteredSubject = 0;
                    dtSubjectCount.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                    DataView dvStuRegistered = dtSubjectCount.DefaultView;
                    if (dvStuRegistered.Count > 0)
                        StudentRegisteredSubject = Convert.ToInt32(dvStuRegistered[0]["totalSubject"]);

                    int differenceCount = StudentRegisteredSubject - StudentMarkEnteredSubject;
                    if (StudentRegisteredSubject > StudentMarkEnteredSubject)
                        if (!dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos))
                            dicInvariationofSubjectMarkEntry.Add(studentAppNos, differenceCount);

                    #endregion

                    DataTable dtStudent = new DataTable();
                    dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                    dtStudent = dtStudentMarks.DefaultView.ToTable(true, "app_no", "ApplicationNo", "AdmissionDate", "Roll_No", "Reg_No", "Batch_Year", "college_code", "degree_code", "semester");

                    if (dtStudent.Rows.Count > 0)
                    {
                        sno2++;

                        string appNo = Convert.ToString(dtStudent.Rows[0]["app_no"]).Trim();
                        string applicationNo = Convert.ToString(dtStudent.Rows[0]["ApplicationNo"]).Trim();
                        string admissionDate = Convert.ToString(dtStudent.Rows[0]["AdmissionDate"]).Trim();
                        string rollNo = Convert.ToString(dtStudent.Rows[0]["Roll_No"]).Trim();
                        string stunam = "select stud_name from Registration where Roll_No='" + rollNo + "' ";
                        DataSet stnam = da.select_method_wo_parameter(stunam, "text");
                        string regNo = Convert.ToString(dtStudent.Rows[0]["Reg_No"]).Trim();
                        string batch = Convert.ToString(dtStudent.Rows[0]["Batch_Year"]).Trim();
                        string college = Convert.ToString(dtStudent.Rows[0]["college_code"]).Trim();
                        string degree = Convert.ToString(dtStudent.Rows[0]["degree_code"]).Trim();
                        string sems = Convert.ToString(dtStudent.Rows[0]["semester"]).Trim();

                        persentmonthcal(college, degree, sems, rollNo, admissionDate);

                        double absenthours = per_workingdays1 - per_per_hrs;
                        double per_tage_date = 0;// ((pre_present_date / per_workingdays) * 100);

                        if (per_workingdays > 0)
                        {
                            per_tage_date = ((pre_present_date / per_workingdays) * 100);
                        }
                        if (per_tage_date > 100)
                        {
                            per_tage_date = 100;
                        }

                        double per_tage_hrs = 0;// (((per_per_hrs) / (per_workingdays1)) * 100);

                        if (per_workingdays1 > 0)
                        {
                            per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
                        }

                        if (per_tage_hrs > 100)
                        {
                            per_tage_hrs = 100;
                        }

                        dum_tage_date = string.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));

                        per_tage_hrs = Math.Round(per_tage_hrs, 2);
                        dum_tage_hrs = per_tage_hrs.ToString();
                        dum_tage_hrs = string.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
                        if (dum_tage_hrs == "NaN")
                        {
                            dum_tage_hrs = "0";
                        }
                        else if (dum_tage_hrs == "Infinity")
                        {
                            dum_tage_hrs = "0";
                        }

                        if (dum_tage_date == "NaN")
                        {
                            dum_tage_date = "0";
                        }
                        else if (dum_tage_date == "Infinity")
                        {
                            dum_tage_date = "0";
                        }

                    }
                    DataTable bestSubjects = new DataTable();
                    DataTable dtCommonSub = new DataTable();
                    if (dtStudentMarks.Rows.Count > 0 && dtBestSubjects.Rows.Count > 0)
                    {
                        string defaultSub = Convert.ToString(dtBestSubjects.Rows[0]["BestofSubjects"]);
                        string commonSub = Convert.ToString(dtBestSubjects.Rows[0]["DefaultSubjects"]);
                        string NoBest = Convert.ToString(dtBestSubjects.Rows[0]["NoofBest"]);
                        int Nosub = 0;
                        int.TryParse(NoBest, out Nosub);
                        if (!string.IsNullOrEmpty(defaultSub))
                        {
                            dtStudentMarks.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + defaultSub + ")";
                            bestSubjects = dtStudentMarks.DefaultView.ToTable();
                            bestSubjects.DefaultView.Sort = "TestMark desc";
                            bestSubjects = bestSubjects.DefaultView.ToTable(true);
                        }
                        if (!string.IsNullOrEmpty(commonSub))
                        {
                            dtStudentMarks.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + commonSub + ")";
                            dtCommonSub = dtStudentMarks.DefaultView.ToTable();
                        }
                        if (bestSubjects.Rows.Count >= Nosub)
                            bestSubjects = SelectTopDataRow(bestSubjects, Nosub);

                    }

                    for (int col = 7; col < FpStudentMarkList.Sheets[0].ColumnCount - 4; col += 3)
                    {
                        string staffName = string.Empty;
                        string noteVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, col].Note).Trim();

                        string subid = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, col].Tag).Trim();

                        testMark = string.Empty;
                        testMaxMark = string.Empty;
                        testMinMark = string.Empty;
                        testSubMarks = 0;
                        testMaxMarks = 0;
                        testMinMarks = 0;
                        subjectCodeVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Note).Trim();
                        subjectNoVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                        subjectNameVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Text).Trim();
                        DataView dvTestMark = new DataView();
                        DataTable dtNew = new DataTable();
                        DataTable dt2 = new DataTable();
                        if (noteVal.Trim() == "-1")
                        {
                            if (dtSubSubjectMarkDetails.Rows.Count > 0)
                            {

                                dtSubSubjectMarkDetails.DefaultView.RowFilter = "appNo='" + studentAppNos + "' and  subject_no='" + subjectNoVal + "'";
                                dt2 = dtSubSubjectMarkDetails.DefaultView.ToTable();
                            }
                            if (dtSubSubjectMarkList.Rows.Count > 0)
                            {
                                dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                dtNew = dtSubSubjectMarkList.DefaultView.ToTable();
                            }
                            if (subjectCodeVal.Trim().ToLower() == "0")
                            {
                                dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                dvTestMark = dtStudentMarks.DefaultView;
                            }
                            else
                                if (dtStudMarks.Rows.Count > 0)
                                {
                                    dtStudMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                    dvTestMark = dtStudMarks.DefaultView;
                                }
                        }
                        else
                        {
                            if (subjectCodeVal.Trim().ToLower() == "0")
                            {
                                dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                dvTestMark = dtStudentMarks.DefaultView;
                            }
                            else
                                if (dtStudMarks.Rows.Count > 0)
                                {
                                    dtStudMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                    dvTestMark = dtStudMarks.DefaultView;
                                }
                        }


                        string displayMark = string.Empty;
                        string displayGrade = string.Empty;

                        bool result = false;
                        Boolean FailedOrAbsent = false;
                        int subjectRow = 0;
                        int subjectVisibleCount = 0;
                        int spanCount = 0;
                        if (FpStudentMarkList.Sheets[0].Columns[0].Visible)
                            spanCount = 1;
                        if (FpStudentMarkList.Sheets[0].Columns[1].Visible)
                            spanCount = 2;
                        if (FpStudentMarkList.Sheets[0].Columns[2].Visible)
                            spanCount = 3;
                        if (FpStudentMarkList.Sheets[0].Columns[3].Visible)
                            spanCount = 4;
                        if (FpStudentMarkList.Sheets[0].Columns[4].Visible)
                            spanCount = 5;
                        if (FpStudentMarkList.Sheets[0].Columns[5].Visible)
                            spanCount = 6;
                        if (FpStudentMarkList.Sheets[0].Columns[6].Visible)
                            spanCount = 7;

                        if (dvTestMark.Count > 0)
                        {
                            if (subjectCount == 0)
                            {
                                // FpStudentMarkList.Sheets[0].RowCount++;
                                serialNo++;
                                subjectRow = FpStudentMarkList.Sheets[0].RowCount - 1;
                                startingRows = subjectRow;
                            }
                            else
                            {
                                subjectRow = startingRows;
                            }

                            testMark = Convert.ToString(dvTestMark[0]["TestMark"]).Trim();
                            testMaxMark = Convert.ToString(dvTestMark[0]["ConductedMaxMark"]).Trim();
                            testMinMark = Convert.ToString(dvTestMark[0]["ConductedMinMark"]).Trim();
                            string totmaxmark = string.Empty;

                            double.TryParse(testMaxMark, out maximumTestMarks);
                            maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;

                            if (dtNew.Rows.Count > 0)
                            {
                                object sumOfMaxMark = dtNew.Compute("SUM(maxMark)", "maxMark>=0");
                                double.TryParse(Convert.ToString(sumOfMaxMark).Trim(), out maximumTestMarks);
                                testMaxMark = maximumTestMarks.ToString();

                                sumOfMaxMark = dtNew.Compute("SUM(minMark)", "minMark>=0");
                                double.TryParse(Convert.ToString(sumOfMaxMark).Trim(), out testMinMarks);
                                testMinMark = testMinMarks.ToString();
                                //maximumTestMarks=
                            }
                            subjectNameVal = Convert.ToString(dvTestMark[0]["subject_name"]).Trim();
                            subjectCodeVal = Convert.ToString(dvTestMark[0]["subject_code"]).Trim();
                            subjectNoVal = Convert.ToString(dvTestMark[0]["subject_no"]).Trim();

                            string appNo = Convert.ToString(dvTestMark[0]["app_no"]).Trim();
                            string applicationNo = Convert.ToString(dvTestMark[0]["ApplicationNo"]).Trim();
                            string admissionDate = Convert.ToString(dvTestMark[0]["AdmissionDate"]).Trim();
                            string rollNo = Convert.ToString(dvTestMark[0]["Roll_No"]).Trim();
                            string regNo = Convert.ToString(dvTestMark[0]["Reg_No"]).Trim();
                            string admissionNo = Convert.ToString(dvTestMark[0]["Roll_Admit"]).Trim();
                            string serialNos = Convert.ToString(dvTestMark[0]["serialno"]).Trim();
                            string studentName = Convert.ToString(dvTestMark[0]["Stud_Name"]).Trim();
                            string studentType = Convert.ToString(dvTestMark[0]["Stud_Type"]).Trim();
                            string classSection = Convert.ToString(dvTestMark[0]["ClassSection"]).Trim();
                            string examSection = Convert.ToString(dvTestMark[0]["ExamSection"]).Trim();
                            string gender = Convert.ToString(dvTestMark[0]["Gender"]).Trim();

                            string batch = Convert.ToString(dvTestMark[0]["Batch_Year"]).Trim();
                            string college = Convert.ToString(dvTestMark[0]["college_code"]).Trim();
                            string degree = Convert.ToString(dvTestMark[0]["degree_code"]).Trim();
                            string sems = Convert.ToString(dvTestMark[0]["semester"]).Trim();
                            string testNames = Convert.ToString(dvTestMark[0]["TestName"]).Trim();
                            string testNos = Convert.ToString(dvTestMark[0]["TestNo"]).Trim();

                            bool isSuccess = false;
                            string convertMarkNew = Convert.ToString(dvTestMark[0]["OutOffMarks"]).Trim();
                            isSuccess = double.TryParse(testMark, out testSubMarks);
                            //testSubMarks = Math.Round(testSubMarks, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                            testMark = testSubMarks.ToString();
                            testMark = (isSuccess && chkRoundOffMarks.Checked) ? testSubMarks.ToString() : testMark;
                            double.TryParse(testMaxMark, out testMaxMarks);
                            double.TryParse(testMinMark, out testMinMarks);

                            double outof100 = 0;

                            double convertedMinMark = 0;
                            double convertedMaxMark = 0;
                            string convertedObtainedMark = testMark;
                            string convertedMinimumMark = testMinMark;
                            string convertedMaximumMark = testMaxMark;
                            ConvertedMark((convertMark.Trim() != "" && convertMark != "0") ? convertMark : "100", ref convertedMaximumMark, ref convertedObtainedMark, ref convertedMinimumMark);
                            double.TryParse(convertedMinimumMark, out convertedMinMark);
                            double.TryParse(convertedMaximumMark, out convertedMaxMark);
                            double outOff = 0;
                            isSuccess = double.TryParse(convertedObtainedMark, out outOff);
                            //outOff = Math.Round(outOff, 1, MidpointRounding.AwayFromZero);
                            outOff = Math.Round(outOff, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                            convertedObtainedMark = outOff.ToString();
                            convertedObtainedMark = (isSuccess && chkRoundOffMarks.Checked) ? outOff.ToString() : convertedObtainedMark;

                            if (testSubMarks != 0 && testMaxMarks > 0)
                                outof100 = Math.Round((testSubMarks / testMaxMarks) * 100, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                            DataView dvGrade = new DataView();
                            if (dtGradeDetails.Rows.Count > 0)
                            {
                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                dvGrade = dtGradeDetails.DefaultView;
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                            }

                            if (testSubMarks < 0)
                            {
                                displayMark = getMarkText(testMark);
                                convertedObtainedMark = displayMark;
                                FailedOrAbsent = true;
                            }
                            else if (string.IsNullOrEmpty(testMark) || testMark.Trim() == "0")
                            {
                                displayMark = "--";
                                convertedObtainedMark = "--";
                                result = true;
                                FailedOrAbsent = true;
                            }
                            else if (testSubMarks < testMinMarks)
                            {
                                FailedOrAbsent = true;
                            }
                            else
                            {
                                if (testSubMarks >= testMinMarks)
                                    result = true;
                                displayMark = testSubMarks.ToString();
                            }
                            DataView dvGradeMarkRangesDf = new DataView();
                            if (dtGradeMarkRanges.Rows.Count > 0)
                            {
                                dtGradeMarkRanges.DefaultView.RowFilter = "Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                dvGradeMarkRangesDf = dtGradeMarkRanges.DefaultView;
                            }
                            if (dvGradeMarkRangesDf.Count > 0)
                            {
                                for (int gg = 0; gg < dvGradeMarkRangesDf.Count; gg++)
                                {
                                    string ranges = Convert.ToString(dvGradeMarkRangesDf[gg]["Ranges"]).Trim().ToLower();
                                    if (!dicGradeWiseCountForDefault.ContainsKey(subjectNoVal.Trim() + "@" + ranges.Trim().ToLower()))
                                    {
                                        dicGradeWiseCountForDefault.Add(subjectNoVal.Trim() + "@" + ranges.Trim().ToLower(), 1);
                                    }
                                    else
                                    {
                                        dicGradeWiseCountForDefault[subjectNoVal.Trim() + "@" + ranges.Trim().ToLower()] += 1;
                                    }
                                }
                            }
                            if (dvGrade.Count > 0)
                            {
                                displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                //result = true;
                                if (!string.IsNullOrEmpty(displayGrade))
                                {
                                    if (!dicGradeWiseCount.ContainsKey(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()))
                                    {
                                        dicGradeWiseCount.Add(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower(), 1);
                                    }
                                    else
                                    {
                                        dicGradeWiseCount[subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()] += 1;
                                    }
                                }
                            }
                            else
                            {
                                displayGrade = "--";
                            }




                            if (FailedOrAbsent)
                                if (!dicFailedAbsentStudents.ContainsKey(studentAppNos.Trim()))
                                    dicFailedAbsentStudents.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                else
                                    dicFailedAbsentStudents[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);
                            if (bestSubjects.Rows.Count > 0 && dtCommonSub.Rows.Count > 0)
                            {
                                bestSubjects.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + subjectNoVal + ")";
                                DataTable dttemp = bestSubjects.DefaultView.ToTable();
                                dtCommonSub.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + subjectNoVal + ")";
                                DataTable dttemp2 = dtCommonSub.DefaultView.ToTable();
                                if (dttemp.Rows.Count > 0 || dttemp2.Rows.Count > 0)
                                {
                                    if (!dicStudentTotal.ContainsKey(studentAppNos.Trim()))
                                        dicStudentTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                    else
                                        dicStudentTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                    if (!dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                        dicStudentTotalOutof100.Add(studentAppNos.Trim(), (outOff < 0) ? 0 : outOff);
                                    else
                                        dicStudentTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);

                                    if (!dicStudentPassedTotal.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                    else
                                        dicStudentPassedTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                    if (!dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedTotalOutof100.Add(studentAppNos.Trim(), ((outOff < 0) ? 0 : outOff));
                                    else
                                        dicStudentPassedTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);
                                }

                            }
                            else
                            {
                                if (!dicStudentTotal.ContainsKey(studentAppNos.Trim()))
                                    dicStudentTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                else
                                    dicStudentTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                if (!dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                    dicStudentTotalOutof100.Add(studentAppNos.Trim(), (outOff < 0) ? 0 : outOff);
                                else
                                    dicStudentTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);

                                if (!dicStudentPassedTotal.ContainsKey(studentAppNos.Trim()))
                                    dicStudentPassedTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                else
                                    dicStudentPassedTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                if (!dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                    dicStudentPassedTotalOutof100.Add(studentAppNos.Trim(), ((outOff < 0) ? 0 : outOff));
                                else
                                    dicStudentPassedTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);
                            }

                            if (result && !string.IsNullOrEmpty(testMark))  //&& dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos)
                            {
                                if (!dicStudentPassedSubjectCount.ContainsKey(studentAppNos.Trim()))
                                    dicStudentPassedSubjectCount.Add(studentAppNos.Trim(), 1);
                                else
                                    dicStudentPassedSubjectCount[studentAppNos.Trim()] += 1;
                            }
                            // html.Append("<td>" + displayMark + "</td>");
                            int markCol = 0;


                            if (dtNew.Rows.Count > 0)
                            {
                                //maximumTestMarks
                                int subSubCol = 0;
                                foreach (DataRow drSubSubject in dtNew.Rows)
                                {
                                    string subSubId = Convert.ToString(drSubSubject["subjectId"]).Trim();
                                    DataView dvSubSubject = new DataView();
                                    if (dt2.Rows.Count > 0)
                                    {
                                        dt2.DefaultView.RowFilter = "subjectId='" + subSubId + "'";
                                        dvSubSubject = dt2.DefaultView;
                                    }
                                    if (dvSubSubject.Count > 0)
                                    {
                                        //,s.minMark,s.maxMark
                                        string subsubjectmark = Convert.ToString(dvSubSubject[0]["testMark"]).Trim();
                                        double subSubjectMarks = 0;
                                        double.TryParse(subsubjectmark, out subSubjectMarks);

                                        string subSubjectMaxMark = Convert.ToString(dvSubSubject[0]["maxMark"]).Trim();
                                        double subSubjectMaxMarks = 0;
                                        double.TryParse(subSubjectMaxMark, out subSubjectMaxMarks);

                                        string subSubjectMinMark = Convert.ToString(dvSubSubject[0]["minMark"]).Trim();
                                        double subSubjectMinMarks = 0;
                                        double.TryParse(subSubjectMinMark, out subSubjectMinMarks);

                                        string displaySubMark = string.Empty;
                                        bool resultSub = false;
                                        if (subSubjectMarks < 0)
                                        {
                                            displaySubMark = getMarkText(subsubjectmark);
                                        }
                                        else if (string.IsNullOrEmpty(subsubjectmark) || subsubjectmark.Trim() == "0")
                                        {
                                            displaySubMark = "--";
                                            resultSub = true;
                                        }
                                        else
                                        {
                                            if (subSubjectMarks >= subSubjectMinMarks)
                                                resultSub = true;
                                            displaySubMark = subSubjectMarks.ToString();
                                        }



                                    }

                                    subSubCol++;
                                }

                            }

                            string display = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && maximumTestMarks > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(maximumTestMarks).Trim() + ")" : Convert.ToString(maximumTestMarks).Trim()) : (maximumTestMarks > 0) ? Convert.ToString(maximumTestMarks).Trim() : "";

                            string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMark > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMark).Trim() + ")" : Convert.ToString(convertedMark).Trim()) : (convertedMark > 0) ? Convert.ToString(convertedMark).Trim() : "";



                        }
                        else
                        {
                            if (subjectCount == 0)
                            {

                                serialNo++;
                                startingRows = subjectRow;
                            }
                            else
                            {
                                subjectRow = startingRows;
                            }
                            displayMark = "--";
                            displayGrade = "--";
                            result = true;


                        }

                        if (col == FpStudentMarkList.Sheets[0].ColumnCount - 7)
                        {

                            string tot = (!chkConvertedTo.Checked) ? (dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()) ? Convert.ToString(dicStudentTotalOutof100[studentAppNos.Trim()]).Trim() : "--") : (dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()) ? Convert.ToString(dicStudentTotalOutof100[studentAppNos.Trim()]).Trim() : "--");

                            int studentAllotedSubject = (dicStudentSubjectCount.ContainsKey(studentAppNos.Trim())) ? dicStudentSubjectCount[studentAppNos.Trim()] : 0;
                            int studentPassedSubject = (dicStudentPassedSubjectCount.ContainsKey(studentAppNos.Trim())) ? dicStudentPassedSubjectCount[studentAppNos.Trim()] : 0;
                            if (studentPassedSubject > 0 && studentPassedSubject > 0)
                            {

                                if (studentAllotedSubject != 0 && studentPassedSubject > 0 && (!dicFailedAbsentStudents.ContainsKey(studentAppNos)))
                                {
                                    double studentTotal = (dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim())) ? dicStudentPassedTotalOutof100[studentAppNos.Trim()] : 0;
                                    double studentAverage = 0;
                                    if (studentTotal > 0 && studentPassedSubject > 0)
                                        studentAverage = (studentTotal / studentPassedSubject);

                                    if (!dicStudentPassedAverageOutof100.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedAverageOutof100.Add(studentAppNos.Trim(), studentAverage);
                                    else
                                        dicStudentPassedAverageOutof100[studentAppNos.Trim()] += studentAverage;
                                }
                            }

                        }

                        subjectCount++;
                        if (serialNo == dtDistinctStudents.Rows.Count && col < FpStudentMarkList.Sheets[0].ColumnCount - 4)
                        {
                            if (col == 7)
                            {

                                columnVal = FpStudentMarkList.Sheets[0].RowCount - (12 + dtGeneralGrade.Rows.Count + dtGradeMarkRanges.Rows.Count);
                            }
                            else
                            {

                                columnVal = FpStudentMarkList.Sheets[0].RowCount - (12 + dtGeneralGrade.Rows.Count + dtGradeMarkRanges.Rows.Count);
                            }
                            //dicStudentPassedAverageOutof100.OrderBy();
                            DataTable dtRankList = new DataTable();
                            int i3 = 0;

                            int notIncludeRowStart = columnVal - 1;

                            int notIncludeRowEND = columnVal;
                            columnVal++;
                            DataView dvStaff = new DataView();
                            if (!string.IsNullOrEmpty(subjectNoVal))
                            {
                                string[] subjectNoList = subjectNoVal.Split(',');
                                staffName = string.Empty;
                                foreach (string s in subjectNoList)
                                {
                                    if (dtStaffDetails.Rows.Count > 0)
                                    {
                                        dtStaffDetails.DefaultView.RowFilter = "subject_no='" + s + "'";
                                        dvStaff = dtStaffDetails.DefaultView;
                                    }

                                    if (dvStaff.Count > 0)
                                    {
                                        string subjectAcr = Convert.ToString(dvStaff[0]["acronym"]).Trim();
                                        if (!string.IsNullOrEmpty(staffName))
                                            staffName += "," + Convert.ToString(dvStaff[0]["staff_name"]).Trim() + ((!string.IsNullOrEmpty(subjectAcr)) ? " (" + subjectAcr + ")" : "");
                                        else
                                            staffName = Convert.ToString(dvStaff[0]["staff_name"]).Trim() + ((!string.IsNullOrEmpty(subjectAcr)) ? " (" + subjectAcr + ")" : "");
                                    }
                                }
                            }

                            columnVal += 1;

                        }

                        col += dtNew.Rows.Count;
                    }

                }

                if (dtGeneralGrade.Rows.Count > 0)
                {
                    html.Append("<tr><td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>MARKS</b></td>");
                    if (chkIncludeGrade.Checked)
                    {
                        html.Append("<td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>GRADE</b></td>");
                    }
                    int ct = dicsubnam.Count;
                    html.Append("<td colspan='" + ct + "px' style ='text-align:center;font-size:25px;font-family:Arial'><b>NUMBER OF STUDENTS</b></td></tr><tr>");

                    if (dicsubnam.Count > 0)
                    {
                        foreach (KeyValuePair<int, string> dic in dicsubnam)
                        {
                            string subnam = dic.Value;
                            html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + subnam + "</b></td>");
                        }
                    }
                    html.Append("</tr>");

                    foreach (DataRow drGrade in dtGeneralGrade.Rows)
                    {
                        html.Append("<tr>");
                        string rng = Convert.ToString(drGrade["Ranges"]).Trim();
                        html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + rng + "</b></td>");

                        string grade = Convert.ToString(drGrade["Mark_Grade"]).Trim();
                        if (chkIncludeGrade.Checked)
                        {
                            html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + grade + "</b></td>");

                        }
                        DataTable dt1 = new DataTable();
                        if (dicsubno.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> dic in dicsubno)
                            {

                                string subjno = dic.Value;
                                if (dtSubSubjectMarkList.Rows.Count > 0)
                                {
                                    dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjno + "'";
                                    dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                }
                                string mk = (!dicGradeWiseCount.ContainsKey(subjno.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCount[subjno.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + mk + "</b></td>");

                            }
                        }
                        html.Append("</tr>");



                    }
                }
                // html.Append("<table></table>");
                html.Append("</table></div><div><table border='2px' cellpadding='3px' cellspacing='3px' style='border-collapse:collapse;border:2px solid black;margin-left:1550px;margin-top:-790px;height:800px;width:955px'>");

                if (chkrange.Checked == false)
                {
                    if (dtGradeMarkRanges.Rows.Count > 0)
                    {
                        int ct = dicsubnam.Count;
                        html.Append("<tr><td rowspan='2px' style='text-align:center;font-size:25px;font-family:Arial'><b>MARKS</b></td><td colspan='" + ct + "px' style ='text-align:center;font-size:25px;font-family:Arial'><b>NUMBER OF STUDENTS</b></td></tr><tr>");
                        if (dicsubnam.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> dic in dicsubnam)
                            {
                                string subnam = dic.Value;
                                html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + subnam + "</b></td>");
                            }
                        }
                        html.Append("</tr>");

                        foreach (DataRow drGrade in dtGradeMarkRanges.Rows)
                        {
                            html.Append("<tr>");
                            string rng = Convert.ToString(drGrade["Ranges"]).Trim();
                            html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + rng + "</b></td>");

                            string grade = Convert.ToString(drGrade["Ranges"]).Trim();

                            DataTable dt1 = new DataTable();
                            if (dicsubno.Count > 0)
                            {
                                foreach (KeyValuePair<int, string> dic in dicsubno)
                                {

                                    string subjno = dic.Value;
                                    if (dtSubSubjectMarkList.Rows.Count > 0)
                                    {
                                        dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjno + "'";
                                        dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                    }
                                    string mk = (!dicGradeWiseCountForDefault.ContainsKey(subjno.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCountForDefault[subjno.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                    html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + mk + "</b></td>");

                                }
                            }
                            html.Append("</tr>");
                        }

                        //if (dtStudentMarks.Rows.Count > 0)
                        //{
                        //    html.Append("<tr><td style='text-align:center;font-size:25px;font-family:Arial'><b>No.of.Failure (Below 35)</b></td>");
                        //    if (dicsubno.Count > 0)
                        //    {
                        //        foreach (KeyValuePair<int, string> dic in dicsubno)
                        //        {

                        //            string subjno = dic.Value;
                        //            dtStudentMarks.DefaultView.RowFilter = "subject_no='" + subjno + "'";
                        //            DataTable dicSubjectCount = dtStudentMarks.DefaultView.ToTable();
                        //            if (dicSubjectCount.Rows.Count > 0)
                        //            {
                        //                string MinMark = Convert.ToString(dicSubjectCount.Rows[0]["TestMinMark"]);
                        //                dicSubjectCount.DefaultView.RowFilter = "TestMark<'" + MinMark + "'";
                        //                DataTable testFail = dicSubjectCount.DefaultView.ToTable();
                        //                DataTable dt1 = new DataTable();
                        //                if (dtSubSubjectMarkList.Rows.Count > 0)
                        //                {
                        //                    dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjno + "'";
                        //                    dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                        //                }
                        //                string mk = string.Empty;
                        //                if (testFail.Rows.Count > 0)
                        //                {
                        //                    mk = Convert.ToString(testFail.Rows.Count);

                        //                }

                        //                else
                        //                {
                        //                    mk = "--";
                        //                }
                        //                //if (string.IsNullOrEmpty(mk))
                        //                //{
                        //                //    mk = "--";
                        //                //}
                        //                html.Append("<td style='text-align:center;font-size:25px;font-family:Arial'><b>" + mk + "</b></td>");
                        //            }
                        //        }
                        //    }
                        //    //  html.Append("<td  style='text-align:center;font-size:21px;font-family:Arial'><b>--</b></td>");
                        //    html.Append("</tr>");
                        //}
                    }

                }
                html.Append("</table></div>");

                html.Append("<div><table cellpadding='15px'  style='margin-top:80px;margin-left:30px'><tr><td style='width:1150px;font-size:25px;font-family:Arial Black'>SIGNATURE OF THE CLASS TEACHER:</td></tr></table></div>");



                string qry4 = da.GetFunction("select template from Master_Settings where settings='Student consolidate Signature Settings' and usercode='" + Convert.ToString(ddlCollege.SelectedValue) + "'");
                if (!string.IsNullOrEmpty(qry4) && qry4.Trim() != "0")
                {
                    string sgn1 = string.Empty;


                    string sign1 = qry4;

                    string val = "principal";
                    string val1 = "viceprincipal";
                    string val2 = "vice";
                    string val3 = "director";
                    string val4 = "vp";
                    string sig = "Select * from collinfo where college_code='" + ddlCollege.SelectedValue.ToString() + "'";
                    DataSet dss = da.select_method_wo_parameter(sig, "text");
                    if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
                    {
                        if (sign1.ToLower().Contains(val))
                        {
                            sgn1 = Convert.ToString(dss.Tables[0].Rows[0]["principal"]);
                        }
                        if (sign1.ToLower().Contains(val1) || sign1.ToLower().Contains(val2) || sign1.ToLower().Contains(val4))
                        {
                            sgn1 = Convert.ToString(dss.Tables[0].Rows[0]["viceprincipal"]);
                        }
                        if (sign1.ToLower().Contains(val3))
                        {
                            sgn1 = Convert.ToString(dss.Tables[0].Rows[0]["coe"]);
                        }

                    }
                    html.Append("<table style='margin-left:50px;margin-top:50px'><tr><td style='font-size:25px;font-family:Arial Black;margin-left:-10px'>" + sgn1.ToUpper() + "</td></tr>");
                    html.Append("<tr><td style='font-size:25px;font-family:Arial Black;margin-left:-10px'>" + sign1.ToUpper() + "</td></tr></table></div>");
                }

                //FpStudentMarkList.SaveChanges();
                FpStudentMarkList.Visible = true;
            }
        }
        else
        {
            lblAlertMsg.Text = "No Record(s) Were Found";
            divPopAlert.Visible = true;
            return;
        }

        html.Append("</div>");
        contentDiv.InnerHtml = html.ToString();
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btnDirectPrint", "PrintDiv();", true);
        divMainContents.Visible = true;
        FpStudentMarkList.Visible = true;



    }

    protected void btnsettings_OnClick(object sender, EventArgs e)
    {
        string qry = da.GetFunction("select template from Master_Settings where settings='Student consolidate Signature Settings' and usercode='" + Convert.ToString(ddlCollege.SelectedValue) + "'");
        if (!string.IsNullOrEmpty(qry) && qry.Trim() != "0")
        {

            txtfooter1.Text = Convert.ToString(qry);


        }
        else
        {
            txtfooter1.Text = string.Empty;

        }
        divsettings.Visible = true;
        divsignsettings.Visible = true;

    }

    protected void btnsavefooter_OnClick(object sender, EventArgs e)
    {
        try
        {
            string leftsign = txtfooter1.Text;

            string template = leftsign;
            string updateqry = "if exists(select template from Master_Settings where settings='Student consolidate Signature Settings' and usercode='" + Convert.ToString(ddlCollege.SelectedValue) + "') update Master_Settings set template='" + Convert.ToString(template) + "' where usercode='" + Convert.ToString(ddlCollege.SelectedValue) + "' and settings='Student consolidate Signature Settings' else insert into Master_Settings (usercode,settings,template) values('" + Convert.ToString(ddlCollege.SelectedValue) + "','Student consolidate Signature Settings','" + Convert.ToString(template) + "')";
            int updqry = da.update_method_wo_parameter(updateqry, "text");
            if (updqry > 0)
            {
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
            }
            else
            {
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Not Saved";
            }
        }
        catch
        {
        }
    }
    protected void btnClosefooter_OnClick(object sender, EventArgs e)
    {
        divsettings.Visible = false;
        divsignsettings.Visible = false;
    }
    protected void btnGetMarks_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;

            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            testName = string.Empty;
            testNo = string.Empty;
            subjectCode = string.Empty;
            subjectName = string.Empty;
            subjectNo = string.Empty;

            orderBy = string.Empty;
            orderBySetting = string.Empty;

            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;

            string qryCollegeCode1 = string.Empty;
            string qryBatchYear1 = string.Empty;
            string qryDegreeCode1 = string.Empty;
            string qrySemester1 = string.Empty;
            string qrySection1 = string.Empty;

            qryCourseId = string.Empty;
            qrytestNo = string.Empty;
            qrytestName = string.Empty;
            qrySubjectNo = string.Empty;
            qrySubjectName = string.Empty;
            qrySubjectCode = string.Empty;

            DataTable dtStudentMarks = new DataTable();
            DataTable dtGradeDetails = new DataTable();


            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            contentDiv.InnerHtml = "";
            StringBuilder html = new StringBuilder();
            html.Append("<table><tr><td>gfreg</td></tr></table>");
            contentDiv.InnerHtml = html.ToString();
            contentDiv.Visible = true;
            if (drtprint == true)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "btnDirectPrint", "PrintPanel2();", true);

            }
            if (ddlBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_Year in(" + batchYear + ")";
                    qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
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
                courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
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
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.current_semester in(" + semester + ")";
                    qrySemester1 = " and srh.semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled)
            {
                string secValue = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
                {
                    section = secValue;
                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(ss.Sections,''))) in('" + secValue + "')";
                }
            }
            if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            {
                testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
                testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(testNo))
                {
                    qrytestNo = " and c.Criteria_no in(" + testNo + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (cblTest.Items.Count > 0 && txtTest.Visible)
            {
                testNo = getCblSelectedValue(cblTest);
                testName = getCblSelectedText(cblTest);
                if (!string.IsNullOrEmpty(testNo))
                {
                    qrytestNo = " and c.Criteria_no in(" + testNo + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (cblSubject.Items.Count > 0 && txtSubject.Visible)
            {
                foreach (ListItem li in cblSubject.Items)
                {
                    string subjectValue = li.Value;
                    if (li.Selected)//ISNULL(ss.isSingleSubject,'0')
                    {
                        string[] s = subjectValue.Split(new string[] { "$mr$" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var val in s)
                        {
                            if (!string.IsNullOrEmpty(subjectCode))
                            {
                                subjectCode += ",'" + val + "'";
                            }
                            else
                            {
                                subjectCode = "'" + val + "'";
                            }
                        }

                    }
                }
                qrySubjectCode = string.Empty;
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    qrySubjectCode = " and s.subject_code in(" + subjectCode + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubject.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlSubject.Items.Count > 0 && ddlSubject.Visible)
            {
                subjectCode = string.Empty;
                string subjectValue = Convert.ToString(ddlSubject.SelectedValue).Trim();
                string[] s = subjectValue.Split(new string[] { "$mr$" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var val in s)
                {
                    if (!string.IsNullOrEmpty(subjectCode))
                    {
                        subjectCode += ",'" + val + "'";
                    }
                    else
                    {
                        subjectCode = "'" + val + "'";
                    }
                }
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    qrySubjectCode = " and s.subject_code in('" + subjectCode + "')";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubject.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            string convertMark = string.Empty;
            double convertedMark = 0;
            if (chkConvertedTo.Checked)
            {
                convertMark = Convert.ToString(txtConvertedMaxMark.Text).Trim();
                double.TryParse(convertMark, out convertedMark);
                if (string.IsNullOrEmpty(convertMark))
                {
                    lblAlertMsg.Text = "Please Enter Converted Mark";
                    divPopAlert.Visible = true;
                    return;
                }
                else if (!double.TryParse(convertMark, out convertedMark))
                {
                    lblAlertMsg.Text = "Please Enter Valid Converted Mark";
                    divPopAlert.Visible = true;
                    return;
                }
                else if (convertedMark <= 0)
                {
                    lblAlertMsg.Text = "Converted Mark Must Be Greater Than Zero!!!";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            string studentAppNo = string.Empty;
            studentAppNo = string.Empty;
            double maximumTestMarks = 0;
            DataTable dtGeneralGrade = new DataTable();
            DataTable dtStaffDetails = new DataTable();
            DataTable dtSubjectCount = new DataTable();
            DataTable dtSubSubjectMarkList = new DataTable();
            DataTable dtSubSubjectMarkDetails = new DataTable();
            DataTable dtBestSubjects = new DataTable();
            int comsub = 0;
            int def = 0;

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo) && !string.IsNullOrEmpty(qrySubjectCode) && !string.IsNullOrEmpty(subjectCode))
            {


                string SelectQ = "select * from CamBestCalc where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "'";
                dtBestSubjects = dirAcc.selectDataTable(SelectQ);
                if (dtBestSubjects.Rows.Count > 0)
                {
                    string com = Convert.ToString(dtBestSubjects.Rows[0]["defaultSubjects"]);
                    string deft = Convert.ToString(dtBestSubjects.Rows[0]["Bestofsubjects"]);
                    if (com != "")
                    {
                        string[] cCount = com.Split(',');
                        int.TryParse(cCount.Length.ToString(), out def);
                    }

                    string[] dCount = deft.Split(',');
                    string totCount = Convert.ToString(dtBestSubjects.Rows[0]["noofBest"]);
                    int.TryParse(totCount, out comsub);

                }
                dicQueryParameter.Clear();
                dicQueryParameter.Add("appNo", studentAppNo);
                dicQueryParameter.Add("batchYear", batchYear);
                dicQueryParameter.Add("degreeCode", degreeCode);
                dicQueryParameter.Add("semester", semester);
                dicQueryParameter.Add("section", section);
                dicQueryParameter.Add("testNo", testNo);
                dicQueryParameter.Add("redoType", "2");
                //dtStudentMarks = storeAcc.selectDataTable("uspGetStudentPreviousMarks", dicQueryParameter);
                convertedMark = ((convertedMark > 0) ? convertedMark : 100);
                qry = "SELECT Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Roll_No),''))) end Roll_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.college_code),''))) end college_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Reg_No),''))) end Reg_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Batch_Year),''))) end Batch_Year,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.degree_code),''))) end degree_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Current_Semester),''))) end semester,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Sections),''))) end ClassSection,LTRIM(RTRIM(ISNULL(Convert(varchar(500),e.sections),''))) as ExamSection,a.app_formno as ApplicationNo,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'' and LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'01/01/1900' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.admissionDate,103),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Adm_Date,103),''))) end AdmissionDate,r.Stud_Name,r.Stud_Type,r.Roll_Admit,ISNULL(r.serialno,'0') as serialno,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,case when a.sex='0' then 'Male' when a.sex='1' then 'Female' else 'Transgender' end as Gender,ss.subject_type,ss.subType_no,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,CAST(ISNULL(e.min_mark,'0') as float) as ConductedMinMark,CAST(ISNULL(e.max_mark,'0') as float) as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as RetestMark,case when (ISNULL(re.marks_obtained,'0')>='0' and ISNULL(re.marks_obtained,'0')>=ISNULL(e.min_mark,'0')) then 'Pass' when ISNULL(re.marks_obtained,'0')='-1' then 'AAA' else 'Fail' end as Result,CAST(case when ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'')<>'' and ISNULL(re.marks_obtained,'0')>=0 and ISNULL(CONVERT(VARCHAR(100),e.max_mark),'')<>'' and ISNULL(e.max_mark,'0')>0 then ROUND(ISNULL(re.marks_obtained,'0')/ ISNULL(e.max_mark,'0') * " + ((convertedMark > 0) ? convertedMark.ToString() : "100") + ", " + ((chkRoundOffMarks.Checked) ? "0" : "1") + ")  else ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') end as float) OutOffMarks,ISNULL(ss.isSingleSubject,'0') as Single FROM CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s,applyn a,Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where ss.syll_code=s.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and r.App_No=a.app_no and s.subject_no=e.subject_no and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySection + qrytestNo + qrySubjectCode + " and isnull(srh.isLatest,'1')='1'";//srh.isLatest='1'  //modified by Mullai
                dtStudentMarks = dirAcc.selectDataTable(qry);//and srh.App_no='" + studentAppNo + "',CAST(ISNULL(e.min_mark,'0') as float) as ConductedMinMark,CAST(ISNULL(e.max_mark,'0') as float) as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as Float) as RetestMark,case when (ISNULL(re.marks_obtained,'0')>='0' and ISNULL(re.marks_obtained,'0')>=ISNULL(e.min_mark,'0')) then 'Pass' when ISNULL(re.marks_obtained,'0')='-1' then 'AAA' else 'Fail' end as Result,CAST(case when ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'')<>'' and ISNULL(re.marks_obtained,'0')>=0 and ISNULL(CONVERT(VARCHAR(100),e.max_mark),'')<>'' and ISNULL(e.max_mark,'0')>=0 then ROUND(ISNULL(re.marks_obtained,'0')/ ISNULL(e.max_mark,'0') * 100, 0)  else ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') end as float) OutOffMarks

                qry = "select distinct ISNULL(e.max_mark,'0') as max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year=e.batch_year and sm.Batch_Year in(" + batchYear + ") and sm.degree_code in(" + degreeCode + ") and sm.semester in(" + semester + ")" + qrySection + qrytestNo + qrySubjectCode + "";
                string maximumTestMark = dirAcc.selectScalarString(qry);
                double.TryParse(maximumTestMark, out maximumTestMarks);
                maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;

                string qry2 = "select distinct s.subjectId,s.subSubjectName,subject_no,s.minMark,s.maxMark from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and criteria_no='" + testNo + "' " + qrySection;
                dtSubSubjectMarkList = dirAcc.selectDataTable(qry2);// and subject_no='" + subjectNos + "'

                qry2 = "select s.subjectId,s.subSubjectName,e.subject_no,s.minMark,s.maxMark,criteria_no,sm.*  from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and criteria_no='" + testNo + "' " + qrySection;
                dtSubSubjectMarkDetails = dirAcc.selectDataTable(qry2);

                qry = "select sm.Batch_Year,sm.degree_code,sm.semester,LTRIM(RTRIM(ISNULL(ss.Sections,''))) as Sections,s.subject_no,s.subject_code,s.acronym,ss.staff_code,sfm.staff_name from staff_selector ss,Syllabus_master sm,subject s,staffmaster sfm where s.syll_code=sm.syll_code and s.subject_no=ss.subject_no and ss.batch_year=sm.Batch_Year and sfm.staff_code=ss.staff_code and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySection1 + qrySubjectCode;
                dtStaffDetails = dirAcc.selectDataTable(qry);

                //qry = "select sc.roll_no,Count(distinct sc.subject_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc where s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by sc.roll_no order by sc.roll_no";
                //qry = "select Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Count(distinct sc.subject_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by r.App_No,srh.App_no order by App_No";

                qry = "select case when ISNULL(c.App_no,'')<>'' then c.App_no when ISNULL(s.App_no,'')<>'' then s.App_no end as App_no,ISNULL(c.totalSubject,'0')+ISNULL(s.totalSubject,'0') as totalSubject from (select Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Count(distinct sc.subject_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by r.App_No,srh.App_no) as c  full join (select Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Count(distinct ss.subType_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r left join StudentRegisterHistory srh on srh.App_no=r.App_No and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySubjectCode + " /*and ss.promote_count=1*/ group by r.App_No,srh.App_no ) as s on c.App_no=s.App_no order by s.App_No";
                dtSubjectCount = dirAcc.selectDataTable(qry);

                qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' ";
                //order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc
                dtGradeDetails = dirAcc.selectDataTable(qry);
                if (dtGradeDetails.Rows.Count > 0)
                {
                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                    dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                    dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                }
                if (dtGeneralGrade.Rows.Count == 0)
                {
                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                    dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                    dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                }
                if (dtGeneralGrade.Rows.Count == 0)
                {
                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                    dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                    dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                }
                if (dtGeneralGrade.Rows.Count == 0)
                {
                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                    dtGradeDetails.DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Trange desc,Frange desc";
                    dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                }
            }
            if (dtStudentMarks.Rows.Count > 0)
            {
                DataTable dtStudMarks = new DataTable();
                dtStudMarks.Columns.Add("app_no", typeof(long));
                dtStudMarks.Columns.Add("subject_type", typeof(string));
                dtStudMarks.Columns.Add("subject_name", typeof(string));
                dtStudMarks.Columns.Add("subject_code", typeof(string));
                dtStudMarks.Columns.Add("subject_no", typeof(string));
                dtStudMarks.Columns.Add("ApplicationNo", typeof(string));
                dtStudMarks.Columns.Add("AdmissionDate", typeof(string));
                dtStudMarks.Columns.Add("Roll_No", typeof(string));
                dtStudMarks.Columns.Add("Reg_No", typeof(string));
                dtStudMarks.Columns.Add("Roll_Admit", typeof(string));
                dtStudMarks.Columns.Add("serialno", typeof(string));
                dtStudMarks.Columns.Add("Stud_Name", typeof(string));
                dtStudMarks.Columns.Add("Stud_Type", typeof(string));
                dtStudMarks.Columns.Add("ClassSection", typeof(string));
                dtStudMarks.Columns.Add("ExamSection", typeof(string));
                dtStudMarks.Columns.Add("Gender", typeof(string));
                dtStudMarks.Columns.Add("Batch_Year", typeof(string));
                dtStudMarks.Columns.Add("college_code", typeof(string));
                dtStudMarks.Columns.Add("degree_code", typeof(string));
                dtStudMarks.Columns.Add("semester", typeof(string));
                dtStudMarks.Columns.Add("TestName", typeof(string));
                dtStudMarks.Columns.Add("TestNo", typeof(string));
                dtStudMarks.Columns.Add("TestMark", typeof(decimal));
                dtStudMarks.Columns.Add("ConductedMaxMark", typeof(decimal));
                dtStudMarks.Columns.Add("ConductedMinMark", typeof(decimal));
                dtStudMarks.Columns.Add("OutOffMarks", typeof(decimal));




                DataTable dtDistinctStudents = new DataTable();
                dtStudentMarks.DefaultView.Sort = orderByStudents(collegeCode, includeOrderBy: 1);
                dtDistinctStudents = dtStudentMarks.DefaultView.ToTable(true, "App_no", "Roll_No", "Reg_No", "ApplicationNo", "Stud_Type", "Roll_Admit", "serialno");
                DataTable dtDistinctSubject = new DataTable();
                DataTable dtDistinctSubjectTypeSingle = new DataTable();
                DataTable dtDistinctSubjectSingle = new DataTable();
                dtStudentMarks.DefaultView.RowFilter = "Single=0";
                dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
                dtDistinctSubject = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name", "subjectpriority", "TestMaxMark", "Single");
                //added by Mullai

                dtStudentMarks.DefaultView.RowFilter = "Single=1";
                dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
                dtDistinctSubjectTypeSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_type", "subType_no", "TestMaxMark", "Single");

                dtStudentMarks.DefaultView.RowFilter = "Single=1";
                dtStudentMarks.DefaultView.Sort = "subjectpriority,subject_code";
                dtDistinctSubjectSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_type", "subType_no", "subject_code", "subject_name", "subjectpriority", "Single");
                int spanStartColumn = 0;
                Init_Spread(FpStudentMarkList, ref spanStartColumn, 0);

                int serialNo = 0;
                object count = 0;
                double subjectHeighestMarks = 0;
                double subjectLeastMarks = 0;
                double subjectAverage = 0;
                double absenteesCount = 0;
                double appearedCount = 0;
                double subjectTotal = 0;

                Dictionary<string, double> dicSubjectWiseLeastMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseHieghestMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseTotalMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAverageMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAppearedCount = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAbsentCount = new Dictionary<string, double>();
                Dictionary<string, double> dicInvariationofSubjectMarkEntry = new Dictionary<string, double>();
                Dictionary<string, double> dicFailedAbsentStudents = new Dictionary<string, double>();

                int totalColumnValue = 0;
                int rankColumnValue = 0;
                int attendanceColumnValue = 0;
                int remarkColumnValue = 0;
                DataTable dtColumnHeader = new DataTable();
                dtColumnHeader.Columns.Add("subject_name", typeof(string));
                dtColumnHeader.Columns.Add("subject_code", typeof(string));
                dtColumnHeader.Columns.Add("subject_no", typeof(string));
                dtColumnHeader.Columns.Add("subjectpriority", typeof(int));
                dtColumnHeader.Columns.Add("Single", typeof(int));
                dtColumnHeader.Columns.Add("DisplayOutOffMarks", typeof(double));
                dtColumnHeader.Columns.Add("MaxMark", typeof(double));//mm
                DataRow drSubjectColumn;
                foreach (DataRow drSubjects in dtDistinctSubject.Rows)
                {
                    count = 0;
                    subjectHeighestMarks = 0;
                    subjectLeastMarks = 0;
                    subjectAverage = 0;
                    absenteesCount = 0;
                    appearedCount = 0;
                    subjectTotal = 0;

                    string subjectCodes = Convert.ToString(drSubjects["subject_code"]).Trim();
                    string subjectNos = Convert.ToString(drSubjects["subject_no"]).Trim();
                    string subjectNames = Convert.ToString(drSubjects["subject_name"]).Trim();
                    string subjectPriority = Convert.ToString(drSubjects["subjectpriority"]).Trim();
                    string maxmarkct = Convert.ToString(drSubjects["TestMaxMark"]).Trim();

                    count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                    if (!dicSubjectWiseLeastMark.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseLeastMark.Add(subjectNos.Trim(), subjectLeastMarks);

                    count = dtStudentMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                    if (!dicSubjectWiseHieghestMark.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseHieghestMark.Add(subjectNos.Trim(), subjectHeighestMarks);

                    count = dtStudentMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                    double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                    if (!dicSubjectWiseTotalMark.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseTotalMark.Add(subjectNos.Trim(), subjectTotal);

                    count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                    double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                    if (!dicSubjectWiseAppearedCount.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseAppearedCount.Add(subjectNos.Trim(), appearedCount);

                    count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks='-1' and subject_no='" + subjectNos + "'");
                    double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                    if (!dicSubjectWiseAbsentCount.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseAbsentCount.Add(subjectNos.Trim(), absenteesCount);

                    if (subjectTotal > 0 && appearedCount > 0)
                        subjectAverage = subjectTotal / appearedCount;
                    subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                    if (!dicSubjectWiseAverageMark.ContainsKey(subjectNos.Trim()))
                        dicSubjectWiseAverageMark.Add(subjectNos.Trim(), subjectAverage);

                    //FpStudentMarkList.Sheets[0].ColumnCount += 3;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 80;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;

                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = subjectNames;
                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Tag = subjectNos;
                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Note = "0";
                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Marks\n";
                    //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 3, 1, 3);

                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 80;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = (chkIncludeGrade.Checked);
                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "Grade";
                    ////FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);


                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 80;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                    //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = ((chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? false : false);
                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Mark\n" + displayOutof100;
                    //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);

                    convertMark = txtConvertedMaxMark.Text;
                    double convertedMax = 0;
                    double.TryParse(convertMark.Trim(), out convertedMax);
                    string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                    drSubjectColumn = dtColumnHeader.NewRow();
                    drSubjectColumn["subject_name"] = subjectNames;
                    drSubjectColumn["subject_code"] = subjectCodes;
                    drSubjectColumn["subject_no"] = subjectNos;
                    drSubjectColumn["subjectpriority"] = subjectPriority;
                    drSubjectColumn["Single"] = "0";
                    drSubjectColumn["DisplayOutOffMarks"] = convertedMax;
                    drSubjectColumn["MaxMark"] = maxmarkct;
                    dtColumnHeader.Rows.Add(drSubjectColumn);

                }
                if (dtDistinctSubjectTypeSingle.Rows.Count > 0)
                {
                    foreach (DataRow drSubjectType in dtDistinctSubjectTypeSingle.Rows)
                    {
                        count = 0;
                        subjectHeighestMarks = 0;
                        subjectLeastMarks = 0;
                        subjectAverage = 0;
                        absenteesCount = 0;
                        appearedCount = 0;
                        subjectTotal = 0;

                        string subjectType = Convert.ToString(drSubjectType["subject_type"]).Trim();
                        string subjectTypeNo = Convert.ToString(drSubjectType["subType_no"]).Trim();
                        string maxmarkct = Convert.ToString(drSubjectType["TestMaxMark"]).Trim();
                        string subjectNoList = string.Empty;
                        string subjectCodeList = string.Empty;
                        double minimumMark = 0;
                        double maximumMark = 0;
                        DataTable dtSingleMarks = new DataTable();

                        DataTable dtSingleSubject = new DataTable();
                        dtDistinctSubjectSingle.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                        dtSingleSubject = dtDistinctSubjectSingle.DefaultView.ToTable();
                        List<string> list = dtSingleSubject.AsEnumerable().Select(r => Convert.ToString(r.Field<decimal>("subject_no"))).ToList();
                        subjectNoList = string.Join(",", list.ToArray());
                        list = dtSingleSubject.AsEnumerable().Select(r => Convert.ToString(r.Field<string>("subject_code"))).ToList();
                        subjectCodeList = string.Join(",", list.ToArray());
                        int subjectPriority = 0;
                        count = dtSingleSubject.Compute("MIN(subjectpriority)", "subjectpriority>=0");
                        int.TryParse(Convert.ToString(count).Trim(), out subjectPriority);

                        dtStudentMarks.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                        dtSingleMarks = dtStudentMarks.DefaultView.ToTable();
                        DataTable dtDistinctStud = dtStudentMarks.DefaultView.ToTable(true, "app_no");
                        DataRow drMarks;
                        Dictionary<string, double> dicTestMarks = new Dictionary<string, double>();
                        Dictionary<string, double> dicConductedMaxMark = new Dictionary<string, double>();
                        Dictionary<string, double> dicConductedMinMark = new Dictionary<string, double>();
                        Dictionary<string, double> dicOutOffMarks = new Dictionary<string, double>();

                        foreach (DataRow drMark in dtDistinctStud.Rows)
                        {
                            DataTable dtMarks = new DataTable();


                            dtSingleMarks.DefaultView.RowFilter = "app_no='" + Convert.ToString(drMark["app_no"]).Trim() + "'";
                            dtMarks = dtSingleMarks.DefaultView.ToTable();
                            double mark = 0;
                            double outoff100 = 0;
                            double TestMark = 0;
                            double ConductedMaxMark = 0;
                            double ConductedMinMark = 0;
                            if (dtMarks.Rows.Count > 0)
                            {
                                drMarks = dtStudMarks.NewRow();
                                drMarks["app_no"] = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                                drMarks["subject_type"] = Convert.ToString(dtMarks.Rows[0]["subject_type"]).Trim();
                                drMarks["subject_name"] = Convert.ToString(dtMarks.Rows[0]["subject_type"]).Trim();
                                drMarks["subject_code"] = subjectCodeList;
                                drMarks["subject_no"] = subjectNoList;

                                drMarks["ApplicationNo"] = Convert.ToString(dtMarks.Rows[0]["ApplicationNo"]).Trim();
                                drMarks["AdmissionDate"] = Convert.ToString(dtMarks.Rows[0]["AdmissionDate"]).Trim();
                                drMarks["Roll_No"] = Convert.ToString(dtMarks.Rows[0]["Roll_No"]).Trim();
                                drMarks["Reg_No"] = Convert.ToString(dtMarks.Rows[0]["Reg_No"]).Trim();
                                drMarks["Roll_Admit"] = Convert.ToString(dtMarks.Rows[0]["Roll_Admit"]).Trim();
                                drMarks["serialno"] = Convert.ToString(dtMarks.Rows[0]["serialno"]).Trim();
                                drMarks["Stud_Name"] = Convert.ToString(dtMarks.Rows[0]["Stud_Name"]).Trim();
                                drMarks["Stud_Type"] = Convert.ToString(dtMarks.Rows[0]["Stud_Type"]).Trim();
                                drMarks["ClassSection"] = Convert.ToString(dtMarks.Rows[0]["ClassSection"]).Trim();
                                drMarks["ExamSection"] = Convert.ToString(dtMarks.Rows[0]["ExamSection"]).Trim();
                                drMarks["Gender"] = Convert.ToString(dtMarks.Rows[0]["Gender"]).Trim();
                                drMarks["Batch_Year"] = Convert.ToString(dtMarks.Rows[0]["Batch_Year"]).Trim();
                                drMarks["college_code"] = Convert.ToString(dtMarks.Rows[0]["college_code"]).Trim();
                                drMarks["degree_code"] = Convert.ToString(dtMarks.Rows[0]["degree_code"]).Trim();

                                drMarks["semester"] = Convert.ToString(dtMarks.Rows[0]["semester"]).Trim();
                                drMarks["TestName"] = Convert.ToString(dtMarks.Rows[0]["TestName"]).Trim();
                                drMarks["TestNo"] = Convert.ToString(dtMarks.Rows[0]["TestNo"]).Trim();

                                //modified for the issue as 
                                //when mark has not been entered for an particular  subject Rank is not getting calculated 
                                #region Added on 9/12/2017 by prabhakaran

                                DataTable dtSubMarkFilter = new DataTable();

                                string app_No_Stud = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                                int StudentMarkEnteredSubject = 0;
                                dtStudentMarks.DefaultView.RowFilter = "App_no='" + app_No_Stud + "'";
                                StudentMarkEnteredSubject = dtStudentMarks.DefaultView.Count;

                                int StudentRegisteredSubject = 0;
                                dtSubjectCount.DefaultView.RowFilter = "App_no='" + app_No_Stud + "'";
                                DataView dvStuRegistered = dtSubjectCount.DefaultView;
                                if (dvStuRegistered.Count > 0)
                                    StudentRegisteredSubject = Convert.ToInt32(dvStuRegistered[0]["totalSubject"]);

                                int differenceCount = StudentRegisteredSubject - StudentMarkEnteredSubject;
                                if (StudentRegisteredSubject > StudentMarkEnteredSubject)
                                    if (!dicInvariationofSubjectMarkEntry.ContainsKey(app_No_Stud))
                                        dicInvariationofSubjectMarkEntry.Add(app_No_Stud, differenceCount);

                                #endregion

                                object sum = dtMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                                double.TryParse(Convert.ToString(sum).Trim(), out outoff100);
                                if (outoff100 == 0)
                                {
                                    sum = dtMarks.Compute("MIN(OutOffMarks)", "OutOffMarks<0");
                                    double.TryParse(Convert.ToString(sum).Trim(), out outoff100);
                                }
                                double maxTotal = ((convertedMark > 0) ? convertedMark * dtMarks.Rows.Count : 100 * dtMarks.Rows.Count);
                                if (maxTotal > 0 && outoff100 > 0)
                                    outoff100 = Math.Round((outoff100 / maxTotal) * convertedMark, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);

                                sum = dtMarks.Compute("SUM(ConductedMaxMark)", "ConductedMaxMark>=0");
                                double.TryParse(Convert.ToString(sum).Trim(), out ConductedMaxMark);
                                if (ConductedMaxMark == 0)
                                {
                                    sum = dtMarks.Compute("MIN(ConductedMaxMark)", "ConductedMaxMark<0");
                                    double.TryParse(Convert.ToString(sum).Trim(), out ConductedMaxMark);
                                }

                                sum = dtMarks.Compute("SUM(TestMark)", "TestMark>=0");
                                double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                                bool isCheck = true;
                                if (TestMark == 0)
                                {
                                    sum = dtMarks.Compute("MIN(TestMark)", "TestMark<0");
                                    double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                                    isCheck = false;
                                    outoff100 = TestMark;
                                }
                                if (isCheck)
                                {
                                    outoff100 = 0;
                                    if (TestMark > 0 && ConductedMaxMark > 0)
                                        outoff100 = Math.Round((TestMark / ConductedMaxMark) * 100, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                                }
                                //sum = dtMarks.Compute("SUM(TestMark)", "TestMark>=0");
                                //double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                                //if (TestMark == 0)
                                //{
                                //    sum = dtMarks.Compute("MIN(TestMark)", "TestMark<0");
                                //    double.TryParse(Convert.ToString(sum).Trim(), out TestMark);
                                //}


                                //outoff100 = 0;
                                //if (TestMark > 0 && ConductedMaxMark > 0)
                                //    outoff100 = Math.Round((TestMark / ConductedMaxMark) * 100, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);

                                sum = dtMarks.Compute("SUM(ConductedMinMark)", "ConductedMinMark>=0");
                                double.TryParse(Convert.ToString(sum).Trim(), out ConductedMinMark);
                                if (ConductedMinMark == 0)
                                {
                                    sum = dtMarks.Compute("MIN(ConductedMinMark)", "ConductedMinMark<0");
                                    double.TryParse(Convert.ToString(sum).Trim(), out ConductedMinMark);
                                }
                                drMarks["TestMark"] = Convert.ToString(TestMark).Trim();
                                drMarks["ConductedMaxMark"] = Convert.ToString(ConductedMaxMark).Trim();
                                drMarks["ConductedMinMark"] = Convert.ToString(ConductedMinMark).Trim();
                                drMarks["OutOffMarks"] = Convert.ToString(outoff100).Trim();
                                dtStudMarks.Rows.Add(drMarks);
                            }
                        }

                        count = dtStudMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0");
                        double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                        if (!dicSubjectWiseLeastMark.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseLeastMark.Add(subjectNoList.Trim(), subjectLeastMarks);

                        count = dtStudMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0");
                        double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                        if (!dicSubjectWiseHieghestMark.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseHieghestMark.Add(subjectNoList.Trim(), subjectHeighestMarks);

                        count = dtStudMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                        double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                        if (!dicSubjectWiseTotalMark.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseTotalMark.Add(subjectNoList.Trim(), subjectTotal);

                        count = dtStudMarks.Compute("COUNT(app_no)", "OutOffMarks>=0");
                        double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                        if (!dicSubjectWiseAppearedCount.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseAppearedCount.Add(subjectNoList.Trim(), appearedCount);

                        count = dtStudMarks.Compute("COUNT(app_no)", "OutOffMarks='-1'");
                        double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                        if (!dicSubjectWiseAbsentCount.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseAbsentCount.Add(subjectNoList.Trim(), absenteesCount);

                        if (subjectTotal > 0 && appearedCount > 0)
                            subjectAverage = subjectTotal / appearedCount;
                        subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                        if (!dicSubjectWiseAverageMark.ContainsKey(subjectNoList.Trim()))
                            dicSubjectWiseAverageMark.Add(subjectNoList.Trim(), subjectAverage);

                        //FpStudentMarkList.Sheets[0].ColumnCount += 3;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 80;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;

                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = subjectType;
                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Tag = subjectNoList;
                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Note = "1";
                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Marks\n";
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 3, 1, 3);

                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 80;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = (chkIncludeGrade.Checked);
                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "Grade";
                        ////FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);

                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 80;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                        //FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = ((chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? false : false);
                        //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Mark\n" + displayOutof100;
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);
                        convertMark = txtConvertedMaxMark.Text;
                        double convertedMax = 0;
                        double.TryParse(convertMark.Trim(), out convertedMax);
                        string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                        drSubjectColumn = dtColumnHeader.NewRow();
                        drSubjectColumn["subject_name"] = subjectType;
                        drSubjectColumn["subject_code"] = subjectCodeList;
                        drSubjectColumn["subject_no"] = subjectNoList;
                        drSubjectColumn["subjectpriority"] = subjectPriority;
                        drSubjectColumn["Single"] = "1";
                        drSubjectColumn["DisplayOutOffMarks"] = convertedMax;
                        drSubjectColumn["MaxMark"] = maxmarkct;
                        dtColumnHeader.Rows.Add(drSubjectColumn);
                    }
                }
                subcount = 0;
                bool colnumct = true;
                coluheadcount1 = 0;
                if (dtColumnHeader.Rows.Count > 0)
                {
                    DataTable dtHeading = new DataTable();
                    //dtColumnHeader.DefaultView.RowFilter = "";
                    dtColumnHeader.DefaultView.Sort = "subjectpriority";
                    dtHeading = dtColumnHeader.DefaultView.ToTable();
                    foreach (DataRow drNewColumns in dtHeading.Rows)
                    {
                        string subjectCodes = Convert.ToString(drNewColumns["subject_code"]).Trim();
                        string subjectNos = Convert.ToString(drNewColumns["subject_no"]).Trim();
                        string subjectNames = Convert.ToString(drNewColumns["subject_name"]).Trim();
                        string subjectPriority = Convert.ToString(drNewColumns["subjectpriority"]).Trim();
                        string Single = Convert.ToString(drNewColumns["Single"]).Trim();
                        //asdasdasd
                        string displayOutof100 = Convert.ToString(drNewColumns["DisplayOutOffMarks"]).Trim();

                        DataTable dt = new DataTable();
                        if (dtSubSubjectMarkList.Rows.Count > 0)
                        {
                            dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNos + "'";
                            dt = dtSubSubjectMarkList.DefaultView.ToTable();
                        }

                        int columnCount = FpStudentMarkList.Sheets[0].ColumnCount += dt.Rows.Count + 3;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 250;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;


                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - (dt.Rows.Count + 3)].Text = subjectNames;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - (dt.Rows.Count + 3)].Tag = subjectNos;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - (dt.Rows.Count + 3)].Note = Single;
                        string maxct = Convert.ToString(drNewColumns["MaxMark"]).Trim();
                        if (string.IsNullOrEmpty(maxct))
                            maxct = "0";
                        subcount = subcount + Convert.ToInt32(maxct);
                        subcount1++;
                        //FpStudentMarkList.Sheets[0].AddSpanCell(0, FpStudentMarkList.Sheets[0].ColumnCount - (dt.Rows.Count + 3), 1, (dt.Rows.Count + 3));
                        //asdasdasd
                        if (dt.Rows.Count > 0)
                        {
                            for (int row = 0; row < dt.Rows.Count; row++)
                            {
                                coluheadcount1++;
                                string subjName = Convert.ToString(dt.Rows[row]["subSubjectName"]);
                                string subjId = Convert.ToString(dt.Rows[row]["subjectId"]);
                                //if (row != 0)
                                //FpStudentMarkList.Sheets[0].ColumnCount++;
                                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, columnCount - (dt.Rows.Count + 3 - row)].Text = subjectNames;
                                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, columnCount - (dt.Rows.Count + 3 - row)].Text = subjName;
                                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, columnCount - (dt.Rows.Count + 3 - row)].Tag = subjId;
                                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, columnCount - (dt.Rows.Count + 3 - row)].Note = "-1";
                            }
                        }
                        else
                        {
                            colnumct = false;
                        }

                        #region modified on 18/9/17

                        bool ISTRUE = true;
                        string[] exmcode = null;
                        string[] appno = null;
                        int i = 0;

                        #endregion

                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Marks\n";
                        FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - (dt.Rows.Count + 3), 1, (dt.Rows.Count + 3));


                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 100;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = (chkIncludeGrade.Checked);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "Grade";
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);

                        convertMark = txtConvertedMaxMark.Text;
                        double convertedMax = 0;
                        double.TryParse(displayOutof100.Trim(), out convertedMax);
                        displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 100;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = ((chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? false : false);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Mark\n" + displayOutof100;
                        FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);
                    }
                }
                int dissubcount = subcount;
                int dissubct = subcount1;//mm

                Dictionary<string, int> dicStudentSubjectCount = new Dictionary<string, int>();
                int maxSubjectCount = 0;
                if (dtSubjectCount.Rows.Count > 0)
                {
                    foreach (DataRow drSubject in dtSubjectCount.Rows)
                    {
                        string rollNos = Convert.ToString(drSubject["App_no"]).Trim();
                        string subjectCount = Convert.ToString(drSubject["totalSubject"]).Trim();
                        int allotedSubjectCount = 0;
                        if (int.TryParse(subjectCount.Trim(), out allotedSubjectCount))
                            if (!dicStudentSubjectCount.ContainsKey(rollNos.Trim()))
                                dicStudentSubjectCount.Add(rollNos.Trim(), allotedSubjectCount);
                        if (maxSubjectCount < allotedSubjectCount)
                            maxSubjectCount = allotedSubjectCount;
                        //else
                        //    dicStudentPassedTotalOutof100[rollNos.Trim()] += allotedSubjectCount;
                    }
                }

                FpStudentMarkList.Sheets[0].ColumnCount += 4;
                totalColumnValue = FpStudentMarkList.Sheets[0].ColumnCount - 4;
                if (chkConvertedTo.Checked)
                {
                    dissubcount = dissubct;
                }
                else
                {
                    dissubcount = subcount;
                }

                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 4].Text = "Total " + ((dissubcount != 0) ? "(" + Convert.ToString((dissubcount * ((chkConvertedTo.Checked) ? convertedMark : 1))) + ")" : "");//mm

                if (dtBestSubjects.Rows.Count > 0 && comsub != 0)
                {
                    //  int totVar = (comsub + def) * 100;
                    int totVar = (comsub) * 100;
                    FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 4].Text = "Total " + totVar.ToString();
                }


                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 4].Width = 100;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 4].Locked = true;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 4].Resizable = false;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 4].Visible = true;
                FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 4, 2, 1);

                rankColumnValue = FpStudentMarkList.Sheets[0].ColumnCount - 3;
                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Rank";
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 120;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;
                FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 3, 2, 1);

                attendanceColumnValue = FpStudentMarkList.Sheets[0].ColumnCount - 2;
                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "ATT%";
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 120;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = true;
                FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);

                remarkColumnValue = FpStudentMarkList.Sheets[0].ColumnCount - 1;
                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Remark";
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 150;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = true;
                FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);

                Dictionary<byte, double> dicOverall = new Dictionary<byte, double>();
                count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 ");
                double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                dicOverall.Add(0, subjectLeastMarks);

                count = dtStudentMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0 ");
                double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                dicOverall.Add(1, subjectHeighestMarks);

                count = dtStudentMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                dicOverall.Add(2, subjectTotal);

                count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks>=0");
                double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                dicOverall.Add(3, appearedCount);

                count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks='-1'");
                double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                dicOverall.Add(4, absenteesCount);

                if (subjectTotal > 0 && appearedCount > 0)
                    subjectAverage = subjectTotal / appearedCount;
                subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                dicOverall.Add(5, subjectAverage);
                Dictionary<string, int> dicGradeWiseCount = new Dictionary<string, int>();

                Dictionary<string, int> dicGradeWiseCountForDefault = new Dictionary<string, int>();

                Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                int testCount = 0;
                int endColumn = FpStudentMarkList.Sheets[0].ColumnCount - 1;
                int startingRows = 0;
                Dictionary<string, double> dicStudentTotal = new Dictionary<string, double>();
                Dictionary<string, double> dicStudentPassedTotal = new Dictionary<string, double>();
                Dictionary<string, double> dicStudentTotalOutof100 = new Dictionary<string, double>();
                Dictionary<string, double> dicStudentPassedTotalOutof100 = new Dictionary<string, double>();

                Dictionary<string, int> dicStudentPassedSubjectCount = new Dictionary<string, int>();

                Dictionary<string, double> dicStudentPassedAverage = new Dictionary<string, double>();
                Dictionary<string, double> dicStudentPassedAverageOutof100 = new Dictionary<string, double>();

                DataTable dtGradeMarkRanges = new DataTable();
                dtGradeMarkRanges.Columns.Add("Mark_Grade");
                dtGradeMarkRanges.Columns.Add("Frange", typeof(decimal));
                dtGradeMarkRanges.Columns.Add("Trange", typeof(decimal));
                dtGradeMarkRanges.Columns.Add("Ranges");

                dtGradeMarkRanges.Rows.Add("", "95", "100", "95 - 100");
                dtGradeMarkRanges.Rows.Add("", "90", "94.99", "90 - 94");
                dtGradeMarkRanges.Rows.Add("", "75", "89.99", "75 - 89");
                dtGradeMarkRanges.Rows.Add("", "60", "74.99", "60 - 74");
                dtGradeMarkRanges.Rows.Add("", "40", "59.99", "40 - 59");
                dtGradeMarkRanges.Rows.Add("", "0", "39.99", "Below 40");
                dtGradeMarkRanges.Rows.Add("", "0", "34.99", "No.of.Failure(Below 35)");

                if (dtDistinctStudents.Rows.Count > 0)
                {
                    foreach (DataRow drStudent in dtDistinctStudents.Rows)
                    {
                        string subjectCodeVal = string.Empty;
                        string subjectNameVal = string.Empty;
                        string subjectNoVal = string.Empty;
                        string testMark = string.Empty;
                        string testMaxMark = string.Empty;
                        string testMinMark = string.Empty;
                        double testSubMarks = 0;
                        double testMaxMarks = 0;
                        double testMinMarks = 0;
                        int subjectCount = 0;

                        int columnVal = 0;

                        string studentAppNos = Convert.ToString(drStudent["App_no"]).Trim();
                        frdate = txtFromDate.Text;
                        todate = txtToDate.Text;
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

                        per_abshrs_spl = 0;
                        tot_per_hrs_spl = 0;
                        tot_ondu_spl = 0;
                        tot_ml_spl = 0;
                        tot_conduct_hr_spl = 0;
                        per_workingdays1 = 0;
                        leavfinaeamount = 0;
                        medicalLeaveDays = 0;
                        medicalLeaveHours = 0;
                        string dum_tage_date = string.Empty;
                        string dum_tage_hrs = string.Empty;

                        #region Added on 9/12/2017 by prabhakaran

                        DataTable dtSubMarkFilter = new DataTable();

                        //string app_No_Stud = Convert.ToString(dtMarks.Rows[0]["app_no"]).Trim();
                        int StudentMarkEnteredSubject = 0;
                        dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                        StudentMarkEnteredSubject = dtStudentMarks.DefaultView.Count;

                        int StudentRegisteredSubject = 0;
                        dtSubjectCount.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                        DataView dvStuRegistered = dtSubjectCount.DefaultView;
                        if (dvStuRegistered.Count > 0)
                            StudentRegisteredSubject = Convert.ToInt32(dvStuRegistered[0]["totalSubject"]);

                        int differenceCount = StudentRegisteredSubject - StudentMarkEnteredSubject;
                        if (StudentRegisteredSubject > StudentMarkEnteredSubject)
                            if (!dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos))
                                dicInvariationofSubjectMarkEntry.Add(studentAppNos, differenceCount);

                        #endregion

                        DataTable dtStudent = new DataTable();
                        dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                        dtStudent = dtStudentMarks.DefaultView.ToTable(true, "app_no", "ApplicationNo", "AdmissionDate", "Roll_No", "Reg_No", "Batch_Year", "college_code", "degree_code", "semester");
                        if (dtStudent.Rows.Count > 0)
                        {
                            string appNo = Convert.ToString(dtStudent.Rows[0]["app_no"]).Trim();
                            string applicationNo = Convert.ToString(dtStudent.Rows[0]["ApplicationNo"]).Trim();
                            string admissionDate = Convert.ToString(dtStudent.Rows[0]["AdmissionDate"]).Trim();
                            string rollNo = Convert.ToString(dtStudent.Rows[0]["Roll_No"]).Trim();
                            string regNo = Convert.ToString(dtStudent.Rows[0]["Reg_No"]).Trim();
                            string batch = Convert.ToString(dtStudent.Rows[0]["Batch_Year"]).Trim();
                            string college = Convert.ToString(dtStudent.Rows[0]["college_code"]).Trim();
                            string degree = Convert.ToString(dtStudent.Rows[0]["degree_code"]).Trim();
                            string sems = Convert.ToString(dtStudent.Rows[0]["semester"]).Trim();

                            persentmonthcal(college, degree, sems, rollNo, admissionDate);

                            double absenthours = per_workingdays1 - per_per_hrs;
                            double per_tage_date = 0;// ((pre_present_date / per_workingdays) * 100);

                            if (per_workingdays > 0)
                            {
                                per_tage_date = ((pre_present_date / per_workingdays) * 100);
                            }
                            if (per_tage_date > 100)
                            {
                                per_tage_date = 100;
                            }

                            double per_tage_hrs = 0;// (((per_per_hrs) / (per_workingdays1)) * 100);

                            if (per_workingdays1 > 0)
                            {
                                per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
                            }

                            if (per_tage_hrs > 100)
                            {
                                per_tage_hrs = 100;
                            }

                            dum_tage_date = string.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));

                            per_tage_hrs = Math.Round(per_tage_hrs, 2);
                            dum_tage_hrs = per_tage_hrs.ToString();
                            dum_tage_hrs = string.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
                            if (dum_tage_hrs == "NaN")
                            {
                                dum_tage_hrs = "0";
                            }
                            else if (dum_tage_hrs == "Infinity")
                            {
                                dum_tage_hrs = "0";
                            }

                            if (dum_tage_date == "NaN")
                            {
                                dum_tage_date = "0";
                            }
                            else if (dum_tage_date == "Infinity")
                            {
                                dum_tage_date = "0";
                            }

                        }
                        DataTable bestSubjects = new DataTable();
                        DataTable dtCommonSub = new DataTable();
                        if (dtStudentMarks.Rows.Count > 0 && dtBestSubjects.Rows.Count > 0)
                        {
                            string defaultSub = Convert.ToString(dtBestSubjects.Rows[0]["BestofSubjects"]);
                            string commonSub = Convert.ToString(dtBestSubjects.Rows[0]["DefaultSubjects"]);
                            string NoBest = Convert.ToString(dtBestSubjects.Rows[0]["NoofBest"]);
                            int Nosub = 0;
                            int.TryParse(NoBest, out Nosub);
                            if (!string.IsNullOrEmpty(defaultSub))
                            {
                                dtStudentMarks.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + defaultSub + ")";
                                bestSubjects = dtStudentMarks.DefaultView.ToTable();
                                bestSubjects.DefaultView.Sort = "TestMark desc";
                                bestSubjects = bestSubjects.DefaultView.ToTable(true);
                            }
                            if (!string.IsNullOrEmpty(commonSub))
                            {
                                dtStudentMarks.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + commonSub + ")";
                                dtCommonSub = dtStudentMarks.DefaultView.ToTable();
                            }
                            if (bestSubjects.Rows.Count >= Nosub)
                                bestSubjects = SelectTopDataRow(bestSubjects, Nosub);

                        }
                        int colnumctval = 0;
                        if (colnumct == true)
                        {
                            colnumctval = 9;
                        }
                        else
                        {
                            colnumctval = 7;
                        }
                        for (int col = 7; col < FpStudentMarkList.Sheets[0].ColumnCount - 4; col += 3)
                        {
                            string staffName = string.Empty;
                            string noteVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, col].Note).Trim();

                            string subid = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, col].Tag).Trim();

                            testMark = string.Empty;
                            testMaxMark = string.Empty;
                            testMinMark = string.Empty;
                            testSubMarks = 0;
                            testMaxMarks = 0;
                            testMinMarks = 0;
                            subjectCodeVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Note).Trim();
                            subjectNoVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                            subjectNameVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, col].Text).Trim();
                            DataView dvTestMark = new DataView();
                            DataTable dtNew = new DataTable();
                            DataTable dt2 = new DataTable();
                            if (noteVal.Trim() == "-1")
                            {
                                if (dtSubSubjectMarkDetails.Rows.Count > 0)
                                {

                                    dtSubSubjectMarkDetails.DefaultView.RowFilter = "appNo='" + studentAppNos + "' and  subject_no='" + subjectNoVal + "'";
                                    dt2 = dtSubSubjectMarkDetails.DefaultView.ToTable();
                                }
                                if (dtSubSubjectMarkList.Rows.Count > 0)
                                {
                                    dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                    dtNew = dtSubSubjectMarkList.DefaultView.ToTable();
                                }
                                if (subjectCodeVal.Trim().ToLower() == "0")
                                {
                                    dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                    dvTestMark = dtStudentMarks.DefaultView;
                                }
                                else
                                    if (dtStudMarks.Rows.Count > 0)
                                    {
                                        dtStudMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                        dvTestMark = dtStudMarks.DefaultView;
                                    }
                            }
                            else
                            {
                                if (subjectCodeVal.Trim().ToLower() == "0")
                                {
                                    dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                    dvTestMark = dtStudentMarks.DefaultView;
                                }
                                else
                                    if (dtStudMarks.Rows.Count > 0)
                                    {
                                        dtStudMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'";
                                        dvTestMark = dtStudMarks.DefaultView;
                                    }
                            }


                            string displayMark = string.Empty;
                            string displayGrade = string.Empty;

                            bool result = false;
                            Boolean FailedOrAbsent = false;
                            int subjectRow = 0;
                            int subjectVisibleCount = 0;
                            int spanCount = 0;
                            if (FpStudentMarkList.Sheets[0].Columns[0].Visible)
                                spanCount = 1;
                            if (FpStudentMarkList.Sheets[0].Columns[1].Visible)
                                spanCount = 2;
                            if (FpStudentMarkList.Sheets[0].Columns[2].Visible)
                                spanCount = 3;
                            if (FpStudentMarkList.Sheets[0].Columns[3].Visible)
                                spanCount = 4;
                            if (FpStudentMarkList.Sheets[0].Columns[4].Visible)
                                spanCount = 5;
                            if (FpStudentMarkList.Sheets[0].Columns[5].Visible)
                                spanCount = 6;
                            if (FpStudentMarkList.Sheets[0].Columns[6].Visible)
                                spanCount = 7;

                            if (dvTestMark.Count > 0)
                            {
                                if (subjectCount == 0)
                                {
                                    FpStudentMarkList.Sheets[0].RowCount++;
                                    serialNo++;
                                    subjectRow = FpStudentMarkList.Sheets[0].RowCount - 1;
                                    startingRows = subjectRow;
                                }
                                else
                                {
                                    subjectRow = startingRows;
                                }

                                testMark = Convert.ToString(dvTestMark[0]["TestMark"]).Trim();
                                testMaxMark = Convert.ToString(dvTestMark[0]["ConductedMaxMark"]).Trim();
                                testMinMark = Convert.ToString(dvTestMark[0]["ConductedMinMark"]).Trim();
                                string totmaxmark = string.Empty;

                                double.TryParse(testMaxMark, out maximumTestMarks);
                                maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;

                                if (dtNew.Rows.Count > 0)
                                {
                                    object sumOfMaxMark = dtNew.Compute("SUM(maxMark)", "maxMark>=0");
                                    double.TryParse(Convert.ToString(sumOfMaxMark).Trim(), out maximumTestMarks);
                                    testMaxMark = maximumTestMarks.ToString();

                                    sumOfMaxMark = dtNew.Compute("SUM(minMark)", "minMark>=0");
                                    double.TryParse(Convert.ToString(sumOfMaxMark).Trim(), out testMinMarks);
                                    testMinMark = testMinMarks.ToString();
                                    //maximumTestMarks=
                                }
                                subjectNameVal = Convert.ToString(dvTestMark[0]["subject_name"]).Trim();
                                subjectCodeVal = Convert.ToString(dvTestMark[0]["subject_code"]).Trim();
                                subjectNoVal = Convert.ToString(dvTestMark[0]["subject_no"]).Trim();

                                string appNo = Convert.ToString(dvTestMark[0]["app_no"]).Trim();
                                string applicationNo = Convert.ToString(dvTestMark[0]["ApplicationNo"]).Trim();
                                string admissionDate = Convert.ToString(dvTestMark[0]["AdmissionDate"]).Trim();
                                string rollNo = Convert.ToString(dvTestMark[0]["Roll_No"]).Trim();
                                string regNo = Convert.ToString(dvTestMark[0]["Reg_No"]).Trim();
                                string admissionNo = Convert.ToString(dvTestMark[0]["Roll_Admit"]).Trim();
                                string serialNos = Convert.ToString(dvTestMark[0]["serialno"]).Trim();
                                string studentName = Convert.ToString(dvTestMark[0]["Stud_Name"]).Trim();
                                string studentType = Convert.ToString(dvTestMark[0]["Stud_Type"]).Trim();
                                string classSection = Convert.ToString(dvTestMark[0]["ClassSection"]).Trim();
                                string examSection = Convert.ToString(dvTestMark[0]["ExamSection"]).Trim();
                                string gender = Convert.ToString(dvTestMark[0]["Gender"]).Trim();

                                string batch = Convert.ToString(dvTestMark[0]["Batch_Year"]).Trim();
                                string college = Convert.ToString(dvTestMark[0]["college_code"]).Trim();
                                string degree = Convert.ToString(dvTestMark[0]["degree_code"]).Trim();
                                string sems = Convert.ToString(dvTestMark[0]["semester"]).Trim();
                                string testNames = Convert.ToString(dvTestMark[0]["TestName"]).Trim();
                                string testNos = Convert.ToString(dvTestMark[0]["TestNo"]).Trim();

                                bool isSuccess = false;
                                string convertMarkNew = Convert.ToString(dvTestMark[0]["OutOffMarks"]).Trim();
                                isSuccess = double.TryParse(testMark, out testSubMarks);
                                //testSubMarks = Math.Round(testSubMarks, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                                testMark = testSubMarks.ToString();
                                testMark = (isSuccess && chkRoundOffMarks.Checked) ? testSubMarks.ToString() : testMark;
                                double.TryParse(testMaxMark, out testMaxMarks);
                                double.TryParse(testMinMark, out testMinMarks);

                                double outof100 = 0;

                                double convertedMinMark = 0;
                                double convertedMaxMark = 0;
                                string convertedObtainedMark = testMark;
                                string convertedMinimumMark = testMinMark;
                                string convertedMaximumMark = testMaxMark;
                                ConvertedMark((convertMark.Trim() != "" && convertMark != "0") ? convertMark : "100", ref convertedMaximumMark, ref convertedObtainedMark, ref convertedMinimumMark);
                                double.TryParse(convertedMinimumMark, out convertedMinMark);
                                double.TryParse(convertedMaximumMark, out convertedMaxMark);
                                double outOff = 0;
                                isSuccess = double.TryParse(convertedObtainedMark, out outOff);
                                //outOff = Math.Round(outOff, 1, MidpointRounding.AwayFromZero);
                                outOff = Math.Round(outOff, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                                convertedObtainedMark = outOff.ToString();
                                convertedObtainedMark = (isSuccess && chkRoundOffMarks.Checked) ? outOff.ToString() : convertedObtainedMark;

                                if (testSubMarks != 0 && testMaxMarks > 0)
                                    outof100 = Math.Round((testSubMarks / testMaxMarks) * 100, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                                DataView dvGrade = new DataView();
                                if (dtGradeDetails.Rows.Count > 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                }

                                if (testSubMarks < 0)
                                {
                                    displayMark = getMarkText(testMark);
                                    convertedObtainedMark = displayMark;
                                    FailedOrAbsent = true;
                                }
                                else if (string.IsNullOrEmpty(testMark) || testMark.Trim() == "0")
                                {
                                    displayMark = "--";
                                    convertedObtainedMark = "--";
                                    result = true;
                                    FailedOrAbsent = true;
                                }
                                else if (testSubMarks < testMinMarks)
                                {
                                    FailedOrAbsent = true;
                                    if (chkRoundOffMarks.Checked == true)
                                    {
                                        testSubMarks = Math.Round(testSubMarks, 0, MidpointRounding.AwayFromZero);
                                    }
                                    displayMark = testSubMarks.ToString();
                                }
                                else
                                {
                                    if (testSubMarks >= testMinMarks)
                                        result = true;
                                    if (chkRoundOffMarks.Checked == true)
                                    {
                                        testSubMarks = Math.Round(testSubMarks, 0, MidpointRounding.AwayFromZero);
                                    }
                                    displayMark = testSubMarks.ToString();
                                }
                                DataView dvGradeMarkRangesDf = new DataView();
                                if (dtGradeMarkRanges.Rows.Count > 0)
                                {
                                    dtGradeMarkRanges.DefaultView.RowFilter = "Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                    dvGradeMarkRangesDf = dtGradeMarkRanges.DefaultView;
                                }
                                if (dvGradeMarkRangesDf.Count > 0)
                                {
                                    for (int gg = 0; gg < dvGradeMarkRangesDf.Count; gg++)
                                    {
                                        string ranges = Convert.ToString(dvGradeMarkRangesDf[gg]["Ranges"]).Trim().ToLower();
                                        if (!dicGradeWiseCountForDefault.ContainsKey(subjectNoVal.Trim() + "@" + ranges.Trim().ToLower()))
                                        {
                                            dicGradeWiseCountForDefault.Add(subjectNoVal.Trim() + "@" + ranges.Trim().ToLower(), 1);
                                        }
                                        else
                                        {
                                            dicGradeWiseCountForDefault[subjectNoVal.Trim() + "@" + ranges.Trim().ToLower()] += 1;
                                        }
                                    }
                                }
                                if (dvGrade.Count > 0)
                                {
                                    displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                    //result = true;
                                    if (!string.IsNullOrEmpty(displayGrade))
                                    {
                                        if (!dicGradeWiseCount.ContainsKey(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()))
                                        {
                                            dicGradeWiseCount.Add(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower(), 1);
                                        }
                                        else
                                        {
                                            dicGradeWiseCount[subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()] += 1;
                                        }
                                    }
                                }
                                else
                                {
                                    displayGrade = "--";
                                }




                                if (FailedOrAbsent)
                                    if (!dicFailedAbsentStudents.ContainsKey(studentAppNos.Trim()))
                                        dicFailedAbsentStudents.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                    else
                                        dicFailedAbsentStudents[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);
                                if (bestSubjects.Rows.Count > 0 && dtCommonSub.Rows.Count > 0)
                                {
                                    bestSubjects.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + subjectNoVal + ")";
                                    DataTable dttemp = bestSubjects.DefaultView.ToTable();
                                    dtCommonSub.DefaultView.RowFilter = "app_no='" + studentAppNos + "' and subject_no in(" + subjectNoVal + ")";
                                    DataTable dttemp2 = dtCommonSub.DefaultView.ToTable();
                                    if (dttemp.Rows.Count > 0 || dttemp2.Rows.Count > 0)
                                    {
                                        if (!dicStudentTotal.ContainsKey(studentAppNos.Trim()))
                                            dicStudentTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                        else
                                            dicStudentTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                        if (!dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                            dicStudentTotalOutof100.Add(studentAppNos.Trim(), (outOff < 0) ? 0 : outOff);
                                        else
                                            dicStudentTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);

                                        if (!dicStudentPassedTotal.ContainsKey(studentAppNos.Trim()))
                                            dicStudentPassedTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                        else
                                            dicStudentPassedTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                        if (!dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                            dicStudentPassedTotalOutof100.Add(studentAppNos.Trim(), ((outOff < 0) ? 0 : outOff));
                                        else
                                            dicStudentPassedTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);
                                    }

                                }
                                else
                                {
                                    if (!dicStudentTotal.ContainsKey(studentAppNos.Trim()))
                                        dicStudentTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                    else
                                        dicStudentTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                    if (!dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                        dicStudentTotalOutof100.Add(studentAppNos.Trim(), (outOff < 0) ? 0 : outOff);
                                    else
                                        dicStudentTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);

                                    if (!dicStudentPassedTotal.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                    else
                                        dicStudentPassedTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                    if (!dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedTotalOutof100.Add(studentAppNos.Trim(), ((outOff < 0) ? 0 : outOff));
                                    else
                                        dicStudentPassedTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);
                                }

                                if (result && !string.IsNullOrEmpty(testMark))  //&& dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos)
                                {
                                    if (!dicStudentPassedSubjectCount.ContainsKey(studentAppNos.Trim()))
                                        dicStudentPassedSubjectCount.Add(studentAppNos.Trim(), 1);
                                    else
                                        dicStudentPassedSubjectCount[studentAppNos.Trim()] += 1;
                                }

                                int markCol = 0;

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(serialNo).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                markCol++;

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(rollNo).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Top;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(regNo).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Top;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(admissionNo).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Top;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentType).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Top;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(gender).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Top;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentName).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                if (FpStudentMarkList.Sheets[0].Columns[markCol].Visible)
                                    spanCount = markCol + 1;
                                endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                markCol++;
                                if (dtNew.Rows.Count > 0)
                                {
                                    //maximumTestMarks
                                    int subSubCol = 0;
                                    foreach (DataRow drSubSubject in dtNew.Rows)
                                    {
                                        string subSubId = Convert.ToString(drSubSubject["subjectId"]).Trim();
                                        DataView dvSubSubject = new DataView();
                                        if (dt2.Rows.Count > 0)
                                        {
                                            dt2.DefaultView.RowFilter = "subjectId='" + subSubId + "'";
                                            dvSubSubject = dt2.DefaultView;
                                        }
                                        if (dvSubSubject.Count > 0)
                                        {
                                            //,s.minMark,s.maxMark
                                            string subsubjectmark = Convert.ToString(dvSubSubject[0]["testMark"]).Trim();
                                            double subSubjectMarks = 0;
                                            double.TryParse(subsubjectmark, out subSubjectMarks);

                                            string subSubjectMaxMark = Convert.ToString(dvSubSubject[0]["maxMark"]).Trim();
                                            double subSubjectMaxMarks = 0;
                                            double.TryParse(subSubjectMaxMark, out subSubjectMaxMarks);

                                            string subSubjectMinMark = Convert.ToString(dvSubSubject[0]["minMark"]).Trim();
                                            double subSubjectMinMarks = 0;
                                            double.TryParse(subSubjectMinMark, out subSubjectMinMarks);

                                            string displaySubMark = string.Empty;
                                            bool resultSub = false;
                                            if (subSubjectMarks < 0)
                                            {
                                                displaySubMark = getMarkText(subsubjectmark);
                                            }
                                            else if (string.IsNullOrEmpty(subsubjectmark) || subsubjectmark.Trim() == "0")
                                            {
                                                displaySubMark = "--";
                                                resultSub = true;
                                            }
                                            else
                                            {
                                                if (subSubjectMarks >= subSubjectMinMarks)
                                                    resultSub = true;
                                                displaySubMark = subSubjectMarks.ToString();
                                            }

                                            if (string.IsNullOrEmpty(subsubjectmark) || subsubjectmark == "0")
                                            {
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].CellType = txtCell;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Text = "--";
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Font.Name = "Book Antiqua";
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Locked = true;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].HorizontalAlign = HorizontalAlign.Center;

                                            }
                                            else
                                            {
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].CellType = txtCell;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Text = Convert.ToString(displaySubMark).Trim();
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].ForeColor = (resultSub) ? Color.Black : Color.Red;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Font.Name = "Book Antiqua";
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Locked = true;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].HorizontalAlign = HorizontalAlign.Center;
                                            }

                                        }
                                        else
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].CellType = txtCell;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Text = "--";
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Font.Name = "Book Antiqua";
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, col + subSubCol].Locked = true;
                                        }
                                        subSubCol++;
                                    }

                                }
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].CellType = txtCell;
                                string display = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && maximumTestMarks > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(maximumTestMarks).Trim() + ")" : Convert.ToString(maximumTestMarks).Trim()) : (maximumTestMarks > 0) ? Convert.ToString(maximumTestMarks).Trim() : "";

                                string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMark > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMark).Trim() + ")" : Convert.ToString(convertedMark).Trim()) : (convertedMark > 0) ? Convert.ToString(convertedMark).Trim() : "";

                                FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, col + (dtNew.Rows.Count)].Text = "Marks\n" + ((!chkConvertedTo.Checked) ? display : displayOutof100);
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Text = (!chkConvertedTo.Checked) ? Convert.ToString(displayMark).Trim() : Convert.ToString(convertedObtainedMark).Trim();

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].ForeColor = (result) ? Color.Black : Color.Red;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Font.Name = "Book Antiqua";
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col].Visible ? col : endColumn;
                                if (FpStudentMarkList.Sheets[0].Columns[col].Visible)
                                    subjectVisibleCount = 1;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;

                                markCol++;
                                //col += 2; //modified
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Text = Convert.ToString(displayGrade).Trim(); //displaying the marks instead of grade
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + 1].Note = Convert.ToString(subjectNoVal).Trim();
                                if (FpStudentMarkList.Sheets[0].Columns[col + (dtNew.Rows.Count) + 1].Visible)
                                    subjectVisibleCount = 2;
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col + 1].Visible ? col + 2 : endColumn;
                                //FpStudentMarkList.Sheets[0].Cells[subjectRow, col + 1].ForeColor = (result) ? Color.Black : Color.Red;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].VerticalAlign = VerticalAlign.Middle;

                                markCol++;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Text = Convert.ToString(convertedObtainedMark).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].ForeColor = (result) ? Color.Black : Color.Red;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Font.Name = "Book Antiqua";
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col + 2].Visible ? col + 1 : endColumn;
                                if (FpStudentMarkList.Sheets[0].Columns[col + (dtNew.Rows.Count) + 2].Visible)
                                    subjectVisibleCount = 3;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].VerticalAlign = VerticalAlign.Middle;


                                //subjectRow++;
                            }
                            else
                            {
                                if (subjectCount == 0)
                                {
                                    FpStudentMarkList.Sheets[0].RowCount++;
                                    serialNo++;
                                    subjectRow = FpStudentMarkList.Sheets[0].RowCount - 1;
                                    startingRows = subjectRow;
                                }
                                else
                                {
                                    subjectRow = startingRows;
                                }
                                displayMark = "--";
                                displayGrade = "--";
                                result = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayMark).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].ForeColor = (result) ? Color.Black : Color.Red;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Font.Name = "Book Antiqua";
                                if (FpStudentMarkList.Sheets[0].Columns[col].Visible)
                                    subjectVisibleCount = 1;
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col].Visible ? col : endColumn;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].CellType = txtCell;

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Text = Convert.ToString(displayMark).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Note = Convert.ToString(subjectNoVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].ForeColor = (result) ? Color.Black : Color.Red;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Font.Name = "Book Antiqua";
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col + 1].Visible ? col + 1 : endColumn;
                                if (FpStudentMarkList.Sheets[0].Columns[col + (dtNew.Rows.Count) + 1].Visible)
                                    subjectVisibleCount = 2;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 1].VerticalAlign = VerticalAlign.Middle;

                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Text = Convert.ToString(displayGrade).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Tag = Convert.ToString(subjectCodeVal).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Note = Convert.ToString(subjectNoVal).Trim();
                                //endColumn = FpStudentMarkList.Sheets[0].Columns[col + 2].Visible ? col + 2 : endColumn;
                                if (FpStudentMarkList.Sheets[0].Columns[col + (dtNew.Rows.Count) + 2].Visible)
                                    subjectVisibleCount = 3;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, col + (dtNew.Rows.Count) + 2].VerticalAlign = VerticalAlign.Middle;
                            }

                            if (col == FpStudentMarkList.Sheets[0].ColumnCount - colnumctval)
                            {
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, totalColumnValue].Text = (!chkConvertedTo.Checked) ? (dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()) ? Convert.ToString(dicStudentTotalOutof100[studentAppNos.Trim()]).Trim() : "--") : (dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()) ? Convert.ToString(dicStudentTotalOutof100[studentAppNos.Trim()]).Trim() : "--");
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, totalColumnValue].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, totalColumnValue].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, totalColumnValue].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, totalColumnValue].VerticalAlign = VerticalAlign.Middle;
                                int studentAllotedSubject = (dicStudentSubjectCount.ContainsKey(studentAppNos.Trim())) ? dicStudentSubjectCount[studentAppNos.Trim()] : 0;
                                int studentPassedSubject = (dicStudentPassedSubjectCount.ContainsKey(studentAppNos.Trim())) ? dicStudentPassedSubjectCount[studentAppNos.Trim()] : 0;
                                if (studentPassedSubject > 0 && studentPassedSubject > 0)
                                {
                                    //modified  dicFailedAbsentStudents
                                    //if ((studentAllotedSubject == studentPassedSubject && studentAllotedSubject != 0) ||(dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos)))
                                    if (studentAllotedSubject != 0 && studentPassedSubject > 0 && (!dicFailedAbsentStudents.ContainsKey(studentAppNos))) // && (dicInvariationofSubjectMarkEntry.ContainsKey(studentAppNos))
                                    {
                                        double studentTotal = (dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim())) ? dicStudentPassedTotalOutof100[studentAppNos.Trim()] : 0;
                                        double studentAverage = 0;
                                        if (studentTotal > 0 && studentPassedSubject > 0)
                                            studentAverage = (studentTotal / studentPassedSubject);
                                        //studentAverage = Math.Round(studentAverage, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);

                                        if (!dicStudentPassedAverageOutof100.ContainsKey(studentAppNos.Trim()))
                                            dicStudentPassedAverageOutof100.Add(studentAppNos.Trim(), studentAverage);
                                        else
                                            dicStudentPassedAverageOutof100[studentAppNos.Trim()] += studentAverage;
                                    }
                                }
                            }

                            subjectCount++;
                            if (serialNo == dtDistinctStudents.Rows.Count && col < FpStudentMarkList.Sheets[0].ColumnCount - 4)
                            {
                                if (col == 7)
                                {
                                    FpStudentMarkList.Sheets[0].RowCount += 13 + dtGeneralGrade.Rows.Count + dtGradeMarkRanges.Rows.Count;
                                    //startingRows = FpStudentMarkList.Sheets[0].RowCount;
                                    columnVal = FpStudentMarkList.Sheets[0].RowCount - (12 + dtGeneralGrade.Rows.Count + dtGradeMarkRanges.Rows.Count);
                                    //startingRows = dtDistinctStudents.Rows.Count + 1;
                                }
                                else
                                {
                                    //columnVal = FpStudentMarkList.Sheets[0].RowCount - (20 + dtGeneralGrade.Rows.Count);
                                    //columnVal = dtDistinctStudents.Rows.Count + 1;
                                    columnVal = FpStudentMarkList.Sheets[0].RowCount - (12 + dtGeneralGrade.Rows.Count + dtGradeMarkRanges.Rows.Count);
                                }
                                //dicStudentPassedAverageOutof100.OrderBy();
                                DataTable dtRankList = new DataTable();
                                if (col == FpStudentMarkList.Sheets[0].ColumnCount - colnumctval)
                                {
                                    bool rankOnePlus = true;
                                    CalculateRankByPercentage(dicStudentPassedTotalOutof100, dicStudentPassedAverageOutof100, ref dtRankList, rankOnePlus);
                                    for (int rowStudent = 0; rowStudent < dtDistinctStudents.Rows.Count; rowStudent++)
                                    {
                                        string appNoStudent = Convert.ToString(dtDistinctStudents.Rows[rowStudent]["app_no"]).Trim();
                                        DataView dvRankList = new DataView();
                                        if (dtRankList.Rows.Count > 0)
                                        {
                                            dtRankList.DefaultView.RowFilter = "appNo='" + appNoStudent + "'";
                                            dvRankList = dtRankList.DefaultView;
                                        }
                                        if (dvRankList.Count > 0)
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Text = (rankOnePlus) ? Convert.ToString(dvRankList[0]["RankOnePlus"]).Trim() : Convert.ToString(dvRankList[0]["rank"]).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Note = (rankOnePlus) ? Convert.ToString(dvRankList[0]["RankOnePlus"]).Trim() : Convert.ToString(dvRankList[0]["rank"]).Trim();
                                        }
                                        else
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Text = Convert.ToString("--").Trim();
                                            FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Note = Convert.ToString("--").Trim();
                                        }
                                        FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Font.Name = "Book Antiqua";
                                        FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].Locked = true;
                                        FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[rowStudent, rankColumnValue].VerticalAlign = VerticalAlign.Middle;
                                    }
                                }
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Tag = Convert.ToString("BREAK PAGE").Trim();
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("TOTAL NO.OF ANSWER SCRIPTS EVALUATED").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                string displayValue = ((dicSubjectWiseAppearedCount.ContainsKey(subjectNoVal.Trim())) ? Convert.ToString(dicSubjectWiseAppearedCount[subjectNoVal.Trim()]).Trim() : "--");

                                DataTable dt1 = new DataTable();
                                if (dtSubSubjectMarkList.Rows.Count > 0)
                                {
                                    dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                    dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                }
                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                else
                                {

                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col + (dtNew.Rows.Count), 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayValue).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;
                                }




                                columnVal += 1;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("SUBJECT AVERAGE").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                displayValue = ((dicSubjectWiseAverageMark.ContainsKey(subjectNoVal.Trim())) ? Convert.ToString(dicSubjectWiseAverageMark[subjectNoVal.Trim()]).Trim() : "--");
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();

                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                else
                                {

                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }


                                // FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);


                                columnVal += 1;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("HIGHEST SCORE").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                displayValue = ((dicSubjectWiseHieghestMark.ContainsKey(subjectNoVal.Trim())) ? Convert.ToString(dicSubjectWiseHieghestMark[subjectNoVal.Trim()]).Trim() : "--");



                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                else
                                {

                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                // FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);


                                columnVal += 1;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("LEAST SCORE").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                displayValue = ((dicSubjectWiseLeastMark.ContainsKey(subjectNoVal.Trim())) ? Convert.ToString(dicSubjectWiseLeastMark[subjectNoVal.Trim()]).Trim() : "--");
                                // FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                else
                                {

                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue + "/" + convertedMark).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);


                                columnVal += 1;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("NO OF ABSENTEES").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                displayValue = ((dicSubjectWiseAbsentCount.ContainsKey(subjectNoVal.Trim())) ? Convert.ToString(dicSubjectWiseAbsentCount[subjectNoVal.Trim()]).Trim() : "--");
                                // FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayValue).Trim();
                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(displayValue).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;

                                }
                                else
                                {

                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(displayValue).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;

                                }
                                // FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);

                                columnVal += 1;
                                int notIncludeRowStart = columnVal - 1;

                                if (dtGeneralGrade.Rows.Count > 0)
                                {
                                    FpStudentMarkList.Sheets[0].Columns[0].Width = 200;
                                    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 0].Text = "MARKS";
                                    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Columns[0].Width = 150;
                                    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 0].VerticalAlign = VerticalAlign.Top;
                                    FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 0, 1, (chkIncludeGrade.Checked) ? spanCount - 1 : spanCount);

                                    int col1 = col + (dtNew.Rows.Count);
                                    int countCol = 0;
                                    if (chkIncludeGrade.Checked && col == 7)
                                    {
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].Text = "GRADE";
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 6, 1, 1);
                                        //col1++;
                                        countCol++;
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].Text = "NUMBER OF STUDENTS";
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 7, 1, FpStudentMarkList.Sheets[0].ColumnCount - (4 + spanCount + subjectVisibleCount - 1));
                                    }
                                    else if (col == 7)
                                    {
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].Text = "NUMBER OF STUDENTS";
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 7, 1, FpStudentMarkList.Sheets[0].ColumnCount - (4 + spanCount + subjectVisibleCount - 1));
                                    }
                                    //FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart + 1, col1].Text = subjectNameVal;
                                    //FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart + 1, col1].HorizontalAlign = HorizontalAlign.Center;
                                    //FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart + 1, col1].VerticalAlign = VerticalAlign.Middle;
                                    //FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart + 1, col1, 1, subjectVisibleCount);
                                    //columnVal++;
                                    foreach (DataRow drGrade in dtGeneralGrade.Rows)
                                    {
                                        columnVal++;
                                        FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString(drGrade["Ranges"]).Trim();
                                        FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, (chkIncludeGrade.Checked) ? spanCount - 1 : spanCount);
                                        col1 = col + (dtNew.Rows.Count);
                                        countCol = 0;
                                        string grade = Convert.ToString(drGrade["Mark_Grade"]).Trim();
                                        if (chkIncludeGrade.Checked && col == 7)
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].Text = Convert.ToString(grade).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 6, 1, 1);
                                            //col1++;
                                            countCol++;
                                        }

                                        if (dtSubSubjectMarkList.Rows.Count > 0)
                                        {
                                            dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                            dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                        }

                                        if (dt1.Rows.Count == 2)
                                        {
                                            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col1 - 2, 1, subjectVisibleCount + 2);
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].Text = (!dicGradeWiseCount.ContainsKey(subjectNoVal.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCount[subjectNoVal.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].VerticalAlign = VerticalAlign.Middle;

                                        }
                                        else
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].Text = (!dicGradeWiseCount.ContainsKey(subjectNoVal.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCount[subjectNoVal.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].VerticalAlign = VerticalAlign.Middle;
                                            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col1, 1, subjectVisibleCount);

                                        }



                                    }
                                }
                                columnVal += 1;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1,0].

                                FpStudentMarkList.Sheets[0].Rows[columnVal - 1].Border.BorderColor = Color.White;
                                FpStudentMarkList.Sheets[0].Rows[columnVal - 1].Border.BorderColorTop = Color.Black;
                                FpStudentMarkList.Sheets[0].Rows[columnVal - 1].Border.BorderColorBottom = Color.Black;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Border.BorderColor = Color.Black;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Border.BorderColorRight = Color.White;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;


                                if (chkrange.Checked == false)
                                {
                                    if (dtGradeMarkRanges.Rows.Count > 0)
                                    {
                                        FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "MARKS";
                                        FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, spanCount);
                                        int col1 = col + (dtNew.Rows.Count);
                                        int countCol = 0;
                                        //if (chkIncludeGrade.Checked && col == 7)
                                        //{
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].Text = "GRADE";
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].HorizontalAlign = HorizontalAlign.Center;
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 6].VerticalAlign = VerticalAlign.Middle;
                                        //    FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 6, 2, 1);
                                        //    //col1++;
                                        //    countCol++;
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].Text = "NUMBER OF STUDENTS";
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //    FpStudentMarkList.Sheets[0].Cells[notIncludeRowStart, 7].VerticalAlign = VerticalAlign.Middle;
                                        //    FpStudentMarkList.Sheets[0].AddSpanCell(notIncludeRowStart, 7, 1, FpStudentMarkList.Sheets[0].ColumnCount - (4 + spanCount + subjectVisibleCount - 1));
                                        //}
                                        //else 
                                        if (col == 7)
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[columnVal, 7].Text = "NUMBER OF STUDENTS";
                                            FpStudentMarkList.Sheets[0].Cells[columnVal, 7].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal, 7].VerticalAlign = VerticalAlign.Middle;
                                            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 7, 1, FpStudentMarkList.Sheets[0].ColumnCount - (4 + spanCount + subjectVisibleCount - 1));
                                        }
                                        //
                                        FpStudentMarkList.Sheets[0].Cells[columnVal + 1, col1].Text = subjectNameVal;
                                        FpStudentMarkList.Sheets[0].Cells[columnVal + 1, col1].HorizontalAlign = HorizontalAlign.Center;
                                        FpStudentMarkList.Sheets[0].Cells[columnVal + 1, col1].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentMarkList.Sheets[0].AddSpanCell(columnVal + 1, col1, 1, subjectVisibleCount);
                                        columnVal += 2;
                                        foreach (DataRow drGrade in dtGradeMarkRanges.Rows)
                                        {
                                            columnVal++;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString(drGrade["Ranges"]).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                            col1 = col + (dtNew.Rows.Count);
                                            countCol = 0;
                                            string grade = Convert.ToString(drGrade["Ranges"]).Trim();
                                            //if (chkIncludeGrade.Checked && col == 7)
                                            //{
                                            //    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].Text = Convert.ToString(grade).Trim();
                                            //    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            //    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                            //    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 6, 1, 1);
                                            //    //col1++;
                                            //    countCol++;
                                            //}
                                            if (dtSubSubjectMarkList.Rows.Count > 0)
                                            {
                                                dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                                dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                            }
                                            if (dt1.Rows.Count == 2)
                                            {
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].Text = (!dicGradeWiseCountForDefault.ContainsKey(subjectNoVal.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCountForDefault[subjectNoVal.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1 - 2].VerticalAlign = VerticalAlign.Middle;
                                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col1 - 2, 1, subjectVisibleCount + 2);
                                            }
                                            else
                                            {
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].Text = (!dicGradeWiseCountForDefault.ContainsKey(subjectNoVal.Trim() + "@" + grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCountForDefault[subjectNoVal.Trim() + "@" + grade.Trim().ToLower()]).Trim();
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].HorizontalAlign = HorizontalAlign.Center;
                                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col1].VerticalAlign = VerticalAlign.Middle;
                                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col1, 1, subjectVisibleCount);
                                            }
                                        }

                                        //if (dtStudentMarks.Rows.Count > 0)
                                        //{
                                        //    columnVal++;
                                        //    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = "No.of.Failure (Below 35)";
                                        //    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        //    FpStudentMarkList.Sheets[0].Columns[0].Width = 100;
                                        //    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                        //    col1 = col1 + 1;
                                        //    countCol = 0;
                                        //    dtStudentMarks.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                        //    DataTable dicSubjectCount = dtStudentMarks.DefaultView.ToTable();
                                        //    if (dicSubjectCount.Rows.Count > 0)
                                        //    {
                                        //        string MinMark = Convert.ToString(dicSubjectCount.Rows[0]["TestMinMark"]);
                                        //        dicSubjectCount.DefaultView.RowFilter = "TestMark<'35' and TestMark<>'-1'";
                                        //        DataTable testFail = dicSubjectCount.DefaultView.ToTable();

                                        //        if (dtSubSubjectMarkList.Rows.Count > 0)
                                        //        {
                                        //            dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                        //            dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                        //        }
                                        //        if (dt1.Rows.Count == 2)
                                        //        {

                                        //            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                        //            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                        //            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                        //            if (testFail.Rows.Count > 0)
                                        //            {

                                        //                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(testFail.Rows.Count);

                                        //            }
                                        //            else
                                        //            {
                                        //                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = "--";
                                        //            }
                                        //        }
                                        //        else
                                        //        {

                                        //            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                        //            FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                        //            FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                        //            if (testFail.Rows.Count > 0)
                                        //            {

                                        //                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(testFail.Rows.Count);

                                        //            }
                                        //            else
                                        //            {
                                        //                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = "--";
                                        //            }

                                        //        }


                                        //    }
                                        //}
                                    }

                                }

                                int notIncludeRowEND = columnVal;
                                columnVal++;
                                DataView dvStaff = new DataView();
                                if (!string.IsNullOrEmpty(subjectNoVal))
                                {
                                    string[] subjectNoList = subjectNoVal.Split(',');
                                    staffName = string.Empty;
                                    foreach (string s in subjectNoList)
                                    {
                                        if (dtStaffDetails.Rows.Count > 0)
                                        {
                                            dtStaffDetails.DefaultView.RowFilter = "subject_no='" + s + "'";
                                            dvStaff = dtStaffDetails.DefaultView;
                                        }

                                        if (dvStaff.Count > 0)
                                        {
                                            string subjectAcr = Convert.ToString(dvStaff[0]["acronym"]).Trim();
                                            if (!string.IsNullOrEmpty(staffName))
                                                staffName += "," + Convert.ToString(dvStaff[0]["staff_name"]).Trim() + ((!string.IsNullOrEmpty(subjectAcr)) ? " (" + subjectAcr + ")" : "");
                                            else
                                                staffName = Convert.ToString(dvStaff[0]["staff_name"]).Trim() + ((!string.IsNullOrEmpty(subjectAcr)) ? " (" + subjectAcr + ")" : "");
                                        }
                                    }
                                }
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("NAME OF THE SUBJECT TEACHER").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                if (dtSubSubjectMarkList.Rows.Count > 0)
                                {
                                    dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "'";
                                    dt1 = dtSubSubjectMarkList.DefaultView.ToTable();
                                }
                                if (dt1.Rows.Count == 2)
                                {
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].Text = Convert.ToString(staffName).Trim();
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount + 2);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col].VerticalAlign = VerticalAlign.Middle;
                                }
                                else
                                {
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString(staffName).Trim();
                                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col + (dtNew.Rows.Count), 1, subjectVisibleCount);
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;
                                }

                                columnVal += 1;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("NAME OF THE EVALUATOR").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString("").Trim();
                                FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                FpStudentMarkList.Sheets[0].Rows[columnVal - 1].Visible = false;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;

                                //columnVal += 1;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].Text = Convert.ToString("SIGNATURE OF THE EVALUATOR").Trim();
                                //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, 0, 1, spanCount);
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].Text = Convert.ToString("").Trim();
                                //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal - 1, col, 1, subjectVisibleCount);
                                //FpStudentMarkList.Sheets[0].Rows[columnVal - 1].Visible = false;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].HorizontalAlign = HorizontalAlign.Center;
                                //FpStudentMarkList.Sheets[0].Cells[columnVal - 1, col + (dtNew.Rows.Count)].VerticalAlign = VerticalAlign.Middle;

                            }
                            if (col == 7)
                            {
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].CellType = txtCell;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].Text = Convert.ToString(dum_tage_date).Trim();
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].Font.Name = "Book Antiqua";
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].Locked = true;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, attendanceColumnValue].VerticalAlign = VerticalAlign.Middle;
                            }
                            col += dtNew.Rows.Count;
                        }
                    }
                    FpStudentMarkList.SaveChanges();

                    for (int i = 0; i < FpStudentMarkList.Sheets[0].RowCount; i++)
                    {
                        if (string.IsNullOrEmpty(FpStudentMarkList.Sheets[0].Cells[i, 0].Text))
                        {
                            FpStudentMarkList.Sheets[0].Rows[i].Remove();
                        }
                    }
                    divMainContents.Visible = true;
                    FpStudentMarkList.Sheets[0].PageSize = FpStudentMarkList.Sheets[0].RowCount;
                    //FpStudentMarkList.Width = 1000;
                    FpStudentMarkList.Height = 1500;
                    FpStudentMarkList.SaveChanges();
                    FpStudentMarkList.Visible = true;
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblAlertMsg.Text = ex.ToString();
            divPopAlert.Visible = true;
            return;
            //da.sendErrorMail(ex, collegeCode, "Consolidated Statement Of Marks");
        }
    }

    #endregion

    #region Alert Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region Confirmation Yes/No Click

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            printCommonPdf.Visible = false;
            string reportname = txtExcelName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpStudentMarkList.Visible == true)
                {
                    da.printexcelreport(FpStudentMarkList, reportname);
                }
                lblExcelErr.Visible = false;
            }
            else
            {
                lblExcelErr.Text = "Please Enter Your Report Name";
                lblExcelErr.Visible = true;
                txtExcelName.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

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
            rptheadname = "CONSOLIDATED STATEMENT OF MARKS";
            string pagename = "ConsolidatedStatementofMarks.aspx";
            string batchyr = ddlBatch.SelectedItem.Text.ToString();
            int btyr = Convert.ToInt32(batchyr) + 1;
            string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "$" + "(" + (Convert.ToString(ddlBatch.SelectedItem).Trim() + "-" + Convert.ToString(btyr)) + ")" + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + ((ddlSec.Items.Count == 0) ? "" : (ddlSec.Items.Count > 0 && !string.IsNullOrEmpty(ddlSec.SelectedItem.Text.Trim()) && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? " - " + ddlSec.SelectedItem.Text.Trim() : "");
            //rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + ((ddlSec.Items.Count == 0) ? "" : (ddlSec.Items.Count > 0 && !string.IsNullOrEmpty(ddlSec.SelectedItem.Text.Trim()) && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? " - " + ddlSec.SelectedItem.Text.Trim() : "") + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " " + lblSem.Text.Trim() + " : " + Convert.ToString(ddlSem.SelectedItem).Trim();
            if (FpStudentMarkList.Visible == true)
            {
                printCommonPdf.loadspreaddetails(FpStudentMarkList, pagename, rptheadname);
            }
            printCommonPdf.Visible = true;
            lblExcelErr.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #endregion

    #region Reusable Method

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
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
            lbl.Add(lblDegree);
            lbl.Add(lblBranch);
            lbl.Add(lblSem);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
                spPageHeading.InnerHtml = "CONSOLIDATED STATEMENT OF MARKS REPORT";
                Page.Title = "CONSOLIDATED STATEMENT OF MARKS REPORT";
            }
            else
            {
                lblBatch.Text = "Batch";
                spPageHeading.InnerHtml = "CONSOLIDATED STATEMENT OF MARKS REPORT";
                Page.Title = "CONSOLIDATED STATEMENT OF MARKS REPORT";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
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
                string insType = dirAcc.selectScalarString(qry).Trim();
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null, byte includeOrderBy = 0)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return orderBy;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
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
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
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
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "Ab";
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
            }
        }
        catch
        {
        }
        return mark;
    }

    private void GetSubjectGrade()
    {
        try
        {

        }
        catch
        {
        }
    }

    /// <summary>
    /// author Malang Raja T
    /// </summary>
    /// <param name="txtConvertTo">A string type txtConvertTo is used for to be converted</param>
    /// <param name="maxMark">ref type maxMark parameter was used to gives the minimum mark for converted obtained marks</param>
    /// <param name="obtainedMark">ref type obtainedMark parameter was used to gives the calculated or converted obtained marks</param>
    /// <param name="minMark">ref type minMark parameter was used to gives the minimum mark for converted obtained marks</param>
    public void ConvertedMark(string txtConvertTo, ref string maxMark, ref string obtainedMark, ref string minMark)
    {
        double Mark, max;
        bool r = double.TryParse(obtainedMark, out Mark);
        bool maxflag = double.TryParse(txtConvertTo, out max);
        double multiply;
        double minmultyply;
        double min = 0;
        double max_minCal = 0;
        bool maxbool = double.TryParse(maxMark, out max_minCal);
        bool minbool = double.TryParse(minMark, out min);
        double convertMax = max_minCal;
        if (maxflag)
        {
            if (r && max_minCal > 0)
            {
                //multiply = max / max_minCal;
                if (maxbool == true && minbool == true && min > 0 && max_minCal > 0)
                {
                    //minmultyply = max_minCal / min;
                    //min = max / minmultyply;
                    double convertMin = (min / max_minCal) * max;
                    min = convertMin;
                }
                if (Mark >= 0)
                    obtainedMark = Convert.ToString(max * (Mark / max_minCal));
                convertMax = max;
            }
            minMark = min.ToString();
            maxMark = txtConvertTo;
        }
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate)
    {
        try
        {
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

            Hashtable hat = new Hashtable();
            string admdate = admitDate;// ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            //Admission_date = Convert.ToDateTime(admdate);
            DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);

            hat.Clear();
            hat.Add("degree_code", degree);
            hat.Add("sem_ester", int.Parse(sem));
            ds = da.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }

            hat.Clear();
            hat.Add("colege_code", collegeCode);
            ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0) ? ds1.Tables[0].Rows.Count : 0;

            string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            bool attendanceDayWiseCalculation = false;
            if (daywisecal.Trim() == "1")
            {
                attendanceDayWiseCalculation = true;
            }



            dd = rollno.Trim();
            hat.Clear();
            ds2.Clear();
            hat.Add("std_rollno", rollno.Trim());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");

            mmyycount = (ds2.Tables.Count > 0) ? ds2.Tables[0].Rows.Count : 0;
            moncount = mmyycount - 1;
            deptflag = false;
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
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
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
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
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

                if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (holiday_table21.ContainsKey(dummy_split))  //added by Mullai
                        {
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }

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

                if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
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
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0) ? ds3.Tables[1].Rows.Count : 0;
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

                                        if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count != 0)
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
                                        if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
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

                                            if (medicalLeaveCountPerSession + njhr >= minpresII)
                                            {
                                                medicalLeaveDays = medicalLeaveDays + 0.5;
                                            }

                                            if (attendanceDayWiseCalculation)
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
        catch
        {
        }
    }
    //public void CalculateRankByPercentage(Dictionary<string, double> dicTotalMarks, Dictionary<string, double> dicTotalPercentage, ref DataTable dtRankList, bool rankOnePlus = false)
    //{
    //    try
    //    {
    //        dicTotalPercentage = dicTotalPercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    //        dicTotalMarks = dicTotalMarks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    //        dtRankList = new DataTable();
    //        dtRankList.Clear();
    //        dtRankList.Columns.Add("AppNo");
    //        dtRankList.Columns.Add("Total");
    //        dtRankList.Columns.Add("Percentage");
    //        dtRankList.Columns.Add("Rank");
    //        dtRankList.Columns.Add("RankOnePlus");
    //        DataRow drRankList;
    //        int rank = 1;
    //        int rankOnePlusBy = 1;
    //        int actualRank = 0;
    //        double previousPercentage = 0;
    //        foreach (KeyValuePair<string, double> keyPercentage in dicTotalPercentage)
    //        {
    //            string keyAppNo = keyPercentage.Key.Trim();
    //            double currentPercentage = keyPercentage.Value;
    //            double totalMark = 0;
    //            if (dicTotalMarks.ContainsKey(keyAppNo))
    //            {
    //                totalMark = dicTotalMarks[keyAppNo];
    //            }
    //            bool equalToPrevious = true;
    //            if (previousPercentage != 0 && previousPercentage != currentPercentage)
    //            {
    //                if (rankOnePlus && actualRank != 0)
    //                {
    //                    rankOnePlusBy = actualRank;
    //                }
    //                rank++;
    //                rankOnePlusBy++;
    //                equalToPrevious = false;
    //            }
    //            actualRank++;
    //            previousPercentage = currentPercentage;
    //            drRankList = dtRankList.NewRow();
    //            drRankList["AppNo"] = keyAppNo;
    //            drRankList["Total"] = totalMark;
    //            drRankList["Percentage"] = currentPercentage;
    //            drRankList["Rank"] = rank;
    //            drRankList["RankOnePlus"] = rankOnePlusBy;
    //            dtRankList.Rows.Add(drRankList);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void CalculateRankByPercentage(Dictionary<string, double> dicTotalMarks, Dictionary<string, double> dicTotalPercentage, ref DataTable dtRankList, bool rankOnePlus = false, byte forPercentageOrTotal = 0)
    {
        try
        {
            dicTotalPercentage = dicTotalPercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dicTotalMarks = dicTotalMarks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dtRankList = new DataTable();
            dtRankList.Clear();
            dtRankList.Columns.Add("AppNo");
            dtRankList.Columns.Add("Total");
            dtRankList.Columns.Add("Percentage");
            dtRankList.Columns.Add("Rank");
            dtRankList.Columns.Add("RankOnePlus");
            DataRow drRankList;
            int rank = 1;
            int rankOnePlusBy = 1;
            int actualRank = 0;
            double previousPercentage = 0;
            double previousTotal = 0;

            if (forPercentageOrTotal == 0)
            {
                foreach (KeyValuePair<string, double> keyPercentage in dicTotalPercentage)
                {
                    string keyAppNo = keyPercentage.Key.Trim();
                    double currentPercentage = keyPercentage.Value;
                    double totalMark = 0;
                    if (dicTotalMarks.ContainsKey(keyAppNo))
                    {
                        totalMark = dicTotalMarks[keyAppNo];
                    }
                    bool equalToPrevious = true;
                    if (previousPercentage != 0 && previousPercentage != currentPercentage)
                    {
                        if (rankOnePlus && actualRank != 0)
                        {
                            rankOnePlusBy = actualRank;
                        }
                        rank++;
                        rankOnePlusBy++;
                        equalToPrevious = false;
                    }
                    actualRank++;
                    previousPercentage = currentPercentage;
                    drRankList = dtRankList.NewRow();
                    drRankList["AppNo"] = keyAppNo;
                    drRankList["Total"] = totalMark;
                    drRankList["Percentage"] = currentPercentage;
                    drRankList["Rank"] = rank;
                    drRankList["RankOnePlus"] = rankOnePlusBy;
                    dtRankList.Rows.Add(drRankList);
                }
            }
            else
            {
                foreach (KeyValuePair<string, double> keyTotal in dicTotalMarks)
                {
                    string keyAppNo = keyTotal.Key.Trim();
                    double currentPercentage = 0;
                    double totalMark = keyTotal.Value;
                    if (dicTotalPercentage.ContainsKey(keyAppNo))
                    {
                        currentPercentage = dicTotalPercentage[keyAppNo];
                        bool equalToPrevious = true;
                        if (previousTotal != 0 && previousTotal != totalMark)
                        {
                            if (rankOnePlus && actualRank != 0)
                            {
                                rankOnePlusBy = actualRank;
                            }
                            rank++;
                            rankOnePlusBy++;
                            equalToPrevious = false;
                        }
                        actualRank++;
                        previousTotal = totalMark;
                        drRankList = dtRankList.NewRow();
                        drRankList["AppNo"] = keyAppNo;
                        drRankList["Total"] = totalMark;
                        drRankList["Percentage"] = currentPercentage;
                        drRankList["Rank"] = rank;
                        drRankList["RankOnePlus"] = rankOnePlusBy;
                        dtRankList.Rows.Add(drRankList);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public DataTable convertToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();
        PropertyInfo[] oProps = null;
        if (varlist == null) return dtReturn;
        foreach (T rec in varlist)
        {
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
        }
        return dtReturn;
    }

    #endregion

    protected void lnksettting_Click(object sender, EventArgs e)
    {
        try
        {
            batchYear = string.Empty;
            collegeCode = string.Empty;
            degreeCode = string.Empty;
            semester = string.Empty;
            testName = string.Empty;
            testNo = string.Empty;
            string sections = string.Empty;

            qrySection = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            dtCommon.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and college_code in('" + collegeCode + "')";
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and batch_year in('" + batchYear + "')";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in('" + degreeCode + "')";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
                qrySemester = " and sm.semester in('" + semester + "')";
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled == true)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                qrySection = string.Empty;
                if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
                    section = string.Empty;
                else
                {
                    qrySection = " and e.sections in('" + section + "')";
                }
            }
            if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            {
                testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
                testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            else if (cblTest.Items.Count > 0 && txtTest.Visible)
            {
                testNo = getCblSelectedValue(cblTest);
                testName = getCblSelectedText(cblTest);
            }

            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
            {
                string incSubject = da.GetFunction("select BestofSubjects from CamBestCalc where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "'");
                string Noofsubject = da.GetFunction("select NoofBest from CamBestCalc where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "'");
                txtNoofSubject.Text = Noofsubject;
                //qry = "select distinct s.subject_code,s.subject_name,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_no from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " union select subject_code=STUFF((select '$mr$'+s.subject_code from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " for XML PATH('')),1,4,''),ss.subject_type as subject_name,min(ISNULL(s.subjectpriority,'0')) as subjectpriority from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " group by ss.subject_type order by subjectpriority,subject_code";
                qry = "select distinct s.subject_code,s.subject_name,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_no from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + "";
                dtCommon = dirAcc.selectDataTable(qry);

                FpSpreadViewSubjects.Sheets[0].RowCount = 0;
                if (dtCommon.Rows.Count > 0)
                {
                    divViewSubjects.Visible = true;
                    FpSpreadViewSubjects.Visible = false;
                    FpSpreadViewSubjects.CommandBar.Visible = false;
                    FpSpreadViewSubjects.Sheets[0].RowCount = 0;
                    FpSpreadViewSubjects.Sheets[0].ColumnCount = 4;
                    FpSpreadViewSubjects.Sheets[0].AutoPostBack = false;
                    FpSpreadViewSubjects.Sheets[0].Columns[0].Locked = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[1].Locked = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[1].Visible = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[2].Locked = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[2].Visible = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[3].Visible = true;
                    FpSpreadViewSubjects.Sheets[0].Columns[3].Locked = false;
                    FpSpreadViewSubjects.Sheets[0].RowHeader.Visible = false;
                    FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                    FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].SheetCorner.RowCount = 1;
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 1].Text = "SubjectCode";
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
                    FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
                    FpSpreadViewSubjects.Sheets[0].Columns[0].Width = 60;
                    FpSpreadViewSubjects.Sheets[0].Columns[1].Width = 100;
                    FpSpreadViewSubjects.Sheets[0].Columns[2].Width = 300;
                    FpSpreadViewSubjects.Sheets[0].Columns[3].Width = 50;
                    int sno = 0;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkcell.AutoPostBack = false;
                    foreach (DataRow dt in dtCommon.Rows)
                    {
                        sno++;
                        string subCode = Convert.ToString(dt["subject_code"]);
                        string subName = Convert.ToString(dt["subject_name"]);
                        string subNo = Convert.ToString(dt["subject_no"]);

                        FpSpreadViewSubjects.Sheets[0].RowCount++;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Text = subCode;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Text = subName;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Note = subNo;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].CellType = chkcell;
                        if (incSubject.Contains(subNo))
                            FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Value = 1;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    }
                    FpSpreadViewSubjects.Sheets[0].PageSize = FpSpreadViewSubjects.Sheets[0].RowCount;
                    FpSpreadViewSubjects.Width = 590;
                    FpSpreadViewSubjects.Height = 300;
                    FpSpreadViewSubjects.Visible = true;
                    divViewSubjects.Visible = true;
                    lblViewSubjectError.Visible = false;
                    FpSpreadViewSubjects.SaveChanges();
                }
            }
        }
        catch
        {

        }
    }

    protected void btnViewSubjects_exit_Clcik(object sender, EventArgs e)
    {
        try
        {
            lblViewSubjectError.Text = string.Empty;
            lblViewSubjectError.Visible = false;
            divViewSubjects.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Clcik(object sender, EventArgs e)
    {
        try
        {
            batchYear = string.Empty;
            collegeCode = string.Empty;
            degreeCode = string.Empty;
            semester = string.Empty;
            testName = string.Empty;
            testNo = string.Empty;
            string sections = string.Empty;
            string NoofBest = string.Empty;
            qrySection = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            dtCommon.Clear();

            //cblSubject.Items.Clear();
            //txtSubject.Text = "--Select--";
            //txtSubject.Enabled = false;
            //chkSubject.Checked = false;

            //ddlSubject.Items.Clear();
            //ddlSubject.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and college_code in('" + collegeCode + "')";
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and batch_year in('" + batchYear + "')";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in('" + degreeCode + "')";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
                qrySemester = " and sm.semester in('" + semester + "')";
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled == true)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                qrySection = string.Empty;
                if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
                    section = string.Empty;
                else
                {
                    qrySection = " and e.sections in('" + section + "')";
                }
            }
            if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            {
                testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
                testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            else if (cblTest.Items.Count > 0 && txtTest.Visible)
            {
                testNo = getCblSelectedValue(cblTest);
                testName = getCblSelectedText(cblTest);
            }
            NoofBest = txtNoofSubject.Text;
            FpSpreadViewSubjects.SaveChanges();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
            {
                string InSubejctNo = string.Empty;
                string OutsubjectNo = string.Empty;
                for (int i = 0; i < FpSpreadViewSubjects.Sheets[0].RowCount; i++)
                {
                    string subNo = Convert.ToString(FpSpreadViewSubjects.Sheets[0].Cells[i, 2].Note);
                    if (Convert.ToString(FpSpreadViewSubjects.Sheets[0].Cells[i, 3].Value) == "1")
                    {
                        if (string.IsNullOrEmpty(InSubejctNo))
                            InSubejctNo = subNo;
                        else
                            InSubejctNo = InSubejctNo + "," + subNo;

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(OutsubjectNo))
                            OutsubjectNo = subNo;
                        else
                            OutsubjectNo = OutsubjectNo + "," + subNo;

                    }
                }

                string inserQ = "if exists(select * from CamBestCalc where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "') update CamBestCalc SET DefaultSubjects='" + OutsubjectNo + "',BestofSubjects='" + InSubejctNo + "',NoofBest='" + NoofBest + "' where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and Semester='" + semester + "' and testNo='" + testNo + "' else insert into CamBestCalc (batchYear,degreeCode,Semester,DefaultSubjects,BestofSubjects,NoofBest,testNo) values ('" + batchYear + "','" + degreeCode + "','" + semester + "','" + OutsubjectNo + "','" + InSubejctNo + "','" + NoofBest + "','" + testNo + "')";
                int cout = da.update_method_wo_parameter(inserQ, "text");
                if (cout != 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Saved Sucessfully";
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Not Saved";
                }

            }
        }
        catch
        {

        }
    }

    public DataTable SelectTopDataRow(DataTable dt, int count)
    {
        DataTable dtn = dt.Clone();
        for (int i = 0; i < count; i++)
        {
            dtn.ImportRow(dt.Rows[i]);
        }

        return dtn;
    }
}