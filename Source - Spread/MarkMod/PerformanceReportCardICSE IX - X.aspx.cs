using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Configuration;


public partial class MarkMod_PerformanceReportCardICSE_IX___X : System.Web.UI.Page
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
    string BestOfSubjects = string.Empty;

    StringBuilder sbSelectedAppNo = new StringBuilder();
    List<string> lstSelectedAppNo = new List<string>();

    int selectedCount = 0;


    Institution institute;

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

            userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                //txtFromDate.Attributes.Add("readonly", "readonly");
                //txtToDate.Attributes.Add("readonly", "readonly");

                //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                setLabelText();
                divMainContents.Visible = false;
                BindPreviousCollege();
                BindRightsBaseBatch();
                BindPreviousDegrees("");
                BindPreviousDepartment("", "");
                BindSemesters();
                BindRightsBasedSectionDetail();
                BindPreviousTestName();
                ////BindPreviousSubject();
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

    private void BindPreviousSemesters(string collegeCode = null, string batchYear = null, string degreeCode = null, byte redoType = 2, string defaultSelectedDegree = null)
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
            if (string.IsNullOrEmpty(degreeCode))
                degreeCode = ((ddlBranch.Items.Count > 0) ? Convert.ToString(ddlBranch.SelectedValue).Trim() : "");
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dicQueryParameter.Add("degreeCode", degreeCode);
            dtCommon = storeAcc.selectDataTable("uspGetAllSemester", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlSem.DataSource = dtCommon;
                ddlSem.DataTextField = "semester";
                ddlSem.DataValueField = "semester";
                ddlSem.DataBind();
                ddlSem.SelectedIndex = 0;
                ddlSem.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindSemesters(string collegeCode = null, string batchYear = null, string degreeCode = null, byte redoType = 2, string defaultSelectedSemester = null)
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
            if (string.IsNullOrEmpty(degreeCode))
                degreeCode = ((ddlBranch.Items.Count > 0) ? Convert.ToString(ddlBranch.SelectedValue).Trim() : "");
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("degreeCode", degreeCode);
            dtCommon = storeAcc.selectDataTable("uspGetSemesterCTE", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlSem.DataSource = dtCommon;
                ddlSem.DataTextField = "semester";
                ddlSem.DataValueField = "semester";
                ddlSem.DataBind();
                ddlSem.SelectedIndex = 0;
                ddlSem.Enabled = true;
            }
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
            qry = "select distinct case when isnull(ltrim(rtrim(srh.sections)),'')<>'' then isnull(ltrim(rtrim(srh.sections)),'') when isnull(ltrim(rtrim(r.Sections)),'')<>'' then isnull(ltrim(rtrim(r.sections)),'') else case when(isnull(ltrim(rtrim(srh.sections)),'')='' or isnull(ltrim(rtrim(r.sections)),'')='') then 'Empty' end end as sections, case when isnull(ltrim(rtrim(srh.sections)),'')<>'' then isnull(ltrim(rtrim(srh.sections)),'') when isnull(ltrim(rtrim(r.sections)),'')<>'' then isnull(ltrim(rtrim(r.sections)),'') else '' end SecValues from Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and isnull(ltrim(rtrim(srh.sections)),'')<>'-1' and isnull(ltrim(rtrim(srh.sections)),'')<>'0' and srh.RedoType='2' " + qryCollegeCode1 + qryDegreeCode1 + qryBatchYear1 + qrySection1 + qrySemester1 + " where isnull(ltrim(rtrim(r.sections)),'')<>'-1' and isnull(ltrim(rtrim(r.sections)),'')<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + qrySection + " order by SecValues";
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

    //private void BindPreviousSubject()
    //{
    //    try
    //    {
    //        batchYear = string.Empty;
    //        collegeCode = string.Empty;
    //        degreeCode = string.Empty;
    //        semester = string.Empty;
    //        testName = string.Empty;
    //        testNo = string.Empty;
    //        string sections = string.Empty;

    //        qrySection = string.Empty;
    //        qryCollegeCode = string.Empty;
    //        qryBatchYear = string.Empty;
    //        qryDegreeCode = string.Empty;
    //        qrySemester = string.Empty;
    //        dtCommon.Clear();

    //        cblSubject.Items.Clear();
    //        txtSubject.Text = "--Select--";
    //        txtSubject.Enabled = false;
    //        chkSubject.Checked = false;

    //        ddlSubject.Items.Clear();
    //        ddlSubject.Enabled = false;
    //        if (ddlCollege.Items.Count > 0)
    //        {
    //            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
    //        }
    //        if (!string.IsNullOrEmpty(collegeCode))
    //        {
    //            qryCollegeCode = " and college_code in('" + collegeCode + "')";
    //        }
    //        if (ddlBatch.Items.Count > 0)
    //        {
    //            batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
    //            if (!string.IsNullOrEmpty(batchYear))
    //            {
    //                qryBatchYear = " and batch_year in('" + batchYear + "')";
    //            }
    //        }
    //        if (ddlBranch.Items.Count > 0)
    //        {
    //            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
    //            if (!string.IsNullOrEmpty(degreeCode))
    //            {
    //                qryDegreeCode = " and degree_code in('" + degreeCode + "')";
    //            }
    //        }
    //        if (ddlSem.Items.Count > 0)
    //        {
    //            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
    //            qrySemester = " and sm.semester in('" + semester + "')";
    //        }
    //        if (ddlSec.Items.Count > 0 && ddlSec.Enabled == true)
    //        {
    //            section = Convert.ToString(ddlSec.SelectedValue).Trim();
    //            qrySection = string.Empty;
    //            if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
    //                section = string.Empty;
    //            else
    //            {
    //                qrySection = " and e.sections in('" + section + "')";
    //            }
    //        }
    //        if (ddlTest.Items.Count > 0 && ddlTest.Visible)
    //        {
    //            testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
    //            testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
    //        }
    //        else if (cblTest.Items.Count > 0 && txtTest.Visible)
    //        {
    //            testNo = getCblSelectedValue(cblTest);
    //            testName = getCblSelectedText(cblTest);
    //        }
    //        if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
    //        {
    //            //dicQueryParameter.Clear();
    //            //dicQueryParameter.Add("batchYear", batchYear);
    //            //dicQueryParameter.Add("degreeCode", degreeCode);
    //            //dicQueryParameter.Add("semester", semester);
    //            //dicQueryParameter.Add("section", section);
    //            //dicQueryParameter.Add("testNo", testNo);
    //            //dtCommon = storeAcc.selectDataTable("uspGetPreviousTestSubjectDetails", dicQueryParameter);
    //            //dtCommon = dirAcc.selectDataTable("select distinct s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ")" + qrySection + " order by s.subject_code");
    //            qry = "select distinct s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " union select subject_code=STUFF((select '$mr$'+s.subject_code  from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " for XML PATH('')),1,4,''),ss.subject_type as subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " group by ss.subject_type order by subject_code";
    //            dtCommon = dirAcc.selectDataTable(qry);
    //        }
    //        if (dtCommon.Rows.Count > 0)
    //        {
    //            cblSubject.DataSource = dtCommon;
    //            cblSubject.DataTextField = "subject_name";
    //            cblSubject.DataValueField = "subject_code";
    //            cblSubject.DataBind();
    //            txtSubject.Enabled = true;
    //            checkBoxListselectOrDeselect(cblSubject, true);
    //            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");

    //            ddlSubject.DataSource = dtCommon;
    //            ddlSubject.DataTextField = "subject_name";
    //            ddlSubject.DataValueField = "subject_code";
    //            ddlSubject.DataBind();
    //            ddlSubject.Enabled = true;
    //            ddlSubject.SelectedIndex = 0;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
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
            FpSpread1.Sheets[0].AutoPostBack = false;
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
                //startColumm = (isRollNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegNoVisible;
                //startColumm = (isRegNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmissionNoVisible;
                //startColumm = (isAdmissionNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                //startColumm = (isStudentTypeVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 220;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 9;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 80;
                FpSpread1.Sheets[0].Columns[2].Width = 80;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 200;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 100;
                FpSpread1.Sheets[0].Columns[8].Width = 100;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = false;
                FpSpread1.Sheets[0].Columns[6].Locked = false;
                FpSpread1.Sheets[0].Columns[7].Locked = false;
                FpSpread1.Sheets[0].Columns[8].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;

                FpSpread1.Sheets[0].Columns[5].CellType = new Farpoint.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[6].CellType = new Farpoint.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[7].CellType = new Farpoint.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[8].CellType = new Farpoint.CheckBoxCellType();

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblSem.Text;
                FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mark";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Grade";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Consider Grand Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Complusary";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #region Initialize Spread

    public void Init_Spread(Farpoint.FpSpread FpViewSpread)
    {
        try
        {
            #region FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.Sheets[0].ColumnCount = 0;
            FpViewSpread.Sheets[0].RowCount = 0;
            FpViewSpread.Sheets[0].SheetCorner.ColumnCount = 0;
            FpViewSpread.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpViewSpread.Height = 350;
            FpViewSpread.Width = 580;

            FpViewSpread.Visible = false;
            FpViewSpread.CommandBar.Visible = false;
            FpViewSpread.RowHeader.Visible = false;
            FpViewSpread.Sheets[0].AutoPostBack = false;
            FpViewSpread.Sheets[0].RowCount = 1;
            FpViewSpread.Sheets[0].ColumnCount = 6;
            FpViewSpread.Sheets[0].FrozenRowCount = 1;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);

            FpViewSpread.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpViewSpread.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;

            FpViewSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpViewSpread.Sheets[0].DefaultStyle = sheetstyle;
            FpViewSpread.Sheets[0].ColumnHeader.RowCount = 2;
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Sex";

            FpViewSpread.Sheets[0].Columns[0].Width = 40;
            FpViewSpread.Sheets[0].Columns[1].Width = 50;
            FpViewSpread.Sheets[0].Columns[2].Width = 120;
            FpViewSpread.Sheets[0].Columns[3].Width = 120;
            FpViewSpread.Sheets[0].Columns[4].Width = 250;
            FpViewSpread.Sheets[0].Columns[5].Width = 100;

            FpViewSpread.Sheets[0].Columns[0].Locked = true;
            FpViewSpread.Sheets[0].Columns[2].Locked = isRollNoVisible;
            FpViewSpread.Sheets[0].Columns[3].Locked = isRegNoVisible;
            FpViewSpread.Sheets[0].Columns[4].Locked = true;
            FpViewSpread.Sheets[0].Columns[5].Locked = true;

            FpViewSpread.Sheets[0].Columns[0].Resizable = false;
            FpViewSpread.Sheets[0].Columns[1].Resizable = false;
            FpViewSpread.Sheets[0].Columns[2].Resizable = false;
            FpViewSpread.Sheets[0].Columns[3].Resizable = false;
            FpViewSpread.Sheets[0].Columns[4].Resizable = false;
            FpViewSpread.Sheets[0].Columns[5].Resizable = false;

            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            Farpoint.CheckBoxCellType chkSelectAll = new Farpoint.CheckBoxCellType();


            chkSelectAll.AutoPostBack = true;
            FpViewSpread.Sheets[0].Cells[0, 1].CellType = chkSelectAll;
            FpViewSpread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpViewSpread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpViewSpread.Sheets[0].SpanModel.Add(0, 2, 1, 4);

            FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;
            FpViewSpread.SaveChanges();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Initialize Spread

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
            BindSemesters("", "");
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            //BindPreviousSubject();
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
            BindSemesters();
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            //BindPreviousSubject();
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
            BindSemesters();
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            //BindPreviousSubject();
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
            BindSemesters();
            BindRightsBasedSectionDetail();
            BindPreviousTestName();
            //BindPreviousSubject();
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
            //BindPreviousSubject();
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
            //BindPreviousSubject();
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
            //BindPreviousSubject();
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
            //BindPreviousSubject();
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
            ////BindPreviousSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    //protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsg.Text = string.Empty;
    //        divPopAlert.Visible = false;
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divMainContents.Visible = false;
    //        CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsg.Text = string.Empty;
    //        divPopAlert.Visible = false;
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divMainContents.Visible = false;
    //        CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void ddlSubject_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsg.Text = string.Empty;
    //        divPopAlert.Visible = false;
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divMainContents.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void chkConvertedTo_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblAlertMsg.Text = string.Empty;
    //        divPopAlert.Visible = false;
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divMainContents.Visible = false;
    //        txtConvertedMaxMark.Text = string.Empty;
    //        txtConvertedMaxMark.Enabled = chkConvertedTo.Checked;

    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

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
            //fromDate = txtFromDate.Text.Trim();
            //toDate = txtToDate.Text.Trim();

            //if (CheckSettings())
            //{
            //    divMainContents.Visible = false;
            //    lblAlertMsg.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (fromDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            //    if (!isValidDate)
            //    {
            //        txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //        lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblAlertMsg.Visible = true;
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "Please Choose From Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (toDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
            //    if (!isValidDate)
            //    {
            //        txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //        lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblAlertMsg.Visible = true;
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else
            //{
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "Please Choose To Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtFromDate > dtToday)
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to Today Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtToDate > dtToday)
            //{
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "To Date Must Be Lesser Than or Equal to Today Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtFromDate > dtToDate)
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to To Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

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
            //fromDate = txtFromDate.Text.Trim();
            //toDate = txtToDate.Text.Trim();

            ////if (CheckSettings())
            ////{
            ////    divMainContents.Visible = false;
            ////    lblAlertMsg.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
            ////    lblAlertMsg.Visible = true;
            ////    divPopAlert.Visible = true;
            ////    return;
            ////}

            //if (fromDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            //    if (!isValidDate)
            //    {
            //        txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //        lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblAlertMsg.Visible = true;
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "Please Choose From Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (toDate.Trim() != "")
            //{
            //    isValidDate = false;
            //    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
            //    if (!isValidDate)
            //    {
            //        txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //        lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
            //        lblAlertMsg.Visible = true;
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else
            //{
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "Please Choose To Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtFromDate > dtToday)
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "From Date Must Be Lesser Than or Equal to Today Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtToDate > dtToday)
            //{
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "To Date Must Be Lesser Than or Equal to Today Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}

            //if (dtFromDate > dtToDate)
            //{
            //    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
            //    lblAlertMsg.Text = "To Date Must Be Greater Than or Equal to From Date";
            //    lblAlertMsg.Visible = true;
            //    divPopAlert.Visible = true;
            //    return;
            //}
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    protected void rblSubjectOrSubjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            //CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            //string studentApplicationNo = string.Empty;
            ////BindPreviousSubject();
            btnMarkTypeSettings_Click(sender, e);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void FpStudentList_Command(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            if (Convert.ToInt32(FpStudentList.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < FpStudentList.Sheets[0].RowCount; i++)
                {
                    FpStudentList.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(FpStudentList.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < FpStudentList.Sheets[0].RowCount; i++)
                {
                    FpStudentList.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
            FpStudentList.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Index Changed Events

    #region Button Events

    #region Get Students Marks

    protected void btnGo_Click(object sender, EventArgs e)
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

            DataTable dtStudentDetails = new DataTable();


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
                    //qrySemester = " and r.current_semester in(" + semester + ")";
                    qrySemester1 = " and srh.semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled)
            {
                string secValue = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
                {
                    section = secValue;
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(srh.Sections,''))) in('" + secValue + "')";
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
            else if (cblTest.Items.Count > 0 && txtTest.Visible && txtTest.Enabled)
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

            if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryBatchYear1) && !string.IsNullOrEmpty(qrySemester1) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryDegreeCode1) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryCollegeCode1)) //&& !string.IsNullOrEmpty(qrySemester)
            {
                qry = "select distinct r.serialno,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,case a.sex when 0 then 'Male' when 1 then 'Female' else 'Transgender' end Sex from Registration r,applyn a where a.app_no=r.App_No and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " union select distinct r.serialno,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,case a.sex when 0 then 'Male' when 1 then 'Female' else 'Transgender' end Sex from Registration r,StudentRegisterHistory srh,applyn a where a.app_no=r.App_No and r.App_No=srh.App_no and a.app_no=srh.App_No and srh.RedoType='2' and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + qrySection1 + " " + orderByStudents(collegeCode, "r"); ;
                dtStudentDetails = dirAcc.selectDataTable(qry);

                /* and r.college_code='" + collegeCode + "' and r.Batch_Year='" + batchYear + "' and r.degree_code='" + degreeCode + "' and r.Current_Semester='" + semester + "' and LTRIM(RTRIM(ISNULL(r.Sections,'')))='" + section + "' and srh.collegeCode='" + collegeCode + "' and srh.BatchYear='" + batchYear + "' and srh.degreeCode='" + degreeCode + "' and srh.semester='" + semester + "' and LTRIM(RTRIM(ISNULL(srh.sections,'')))='" + section + "'*/

            }
            if (dtStudentDetails.Rows.Count > 0)
            {
                Init_Spread(FpStudentList);
                int serialNo = 0;
                foreach (DataRow drStudent in dtStudentDetails.Rows)
                {
                    string studentName = Convert.ToString(drStudent["Stud_Name"]).Trim();
                    string studentAppNo = Convert.ToString(drStudent["App_No"]).Trim();
                    string studentRollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                    string studentRegNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                    string studentRollAdmit = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                    string studentType = Convert.ToString(drStudent["Stud_Type"]).Trim();
                    string studentGender = Convert.ToString(drStudent["Sex"]).Trim();

                    FpStudentList.Sheets[0].RowCount++;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    Farpoint.CheckBoxCellType chkSelect = new Farpoint.CheckBoxCellType();

                    int markCol = 0;
                    int subjectRow = FpStudentList.Sheets[0].RowCount - 1;
                    serialNo++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(serialNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(studentAppNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString("").Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                    markCol++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = chkSelect;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = false;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                    markCol++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentRollNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(studentAppNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString("").Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                    markCol++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentRegNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(studentAppNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString("").Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                    markCol++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentName).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(studentAppNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString("").Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                    markCol++;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentGender).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(studentAppNo).Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString("").Trim();
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                }
                divMainContents.Visible = true;
                FpStudentList.Sheets[0].PageSize = FpStudentList.Sheets[0].RowCount;
                FpStudentList.Height = 500;
                FpStudentList.Width = 700;
                FpStudentList.SaveChanges();
                FpStudentList.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Found";
                divPopAlert.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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

    #region Print PDF

    protected void btnReportCard_Click(object sender, EventArgs e)
    {
        try
        {
            sbSelectedAppNo.Clear();
            lstSelectedAppNo.Clear();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int checkedcount = 0;
            string rollnos = string.Empty;
            FpStudentList.SaveChanges();
            if (FpStudentList.Sheets[0].RowCount > 1)
            {
                for (int i = 0; i < FpStudentList.Sheets[0].RowCount; i++)
                {
                    if (Convert.ToInt32(FpStudentList.Sheets[0].Cells[i, 1].Value) == 1)
                    {
                        string appNo = Convert.ToString(FpStudentList.Sheets[0].Cells[i, 0].Tag).Trim();
                        checkedcount++;
                        if (rollnos == "")
                        {
                            rollnos = "'" + Convert.ToString(FpStudentList.Sheets[0].Cells[i, 2].Text) + "'";
                        }
                        else
                        {
                            rollnos = rollnos + ",'" + Convert.ToString(FpStudentList.Sheets[0].Cells[i, 2].Text) + "'";
                        }
                        if (!string.IsNullOrEmpty(appNo))
                        {
                            if (!lstSelectedAppNo.Contains(appNo))
                            {
                                lstSelectedAppNo.Add(appNo);
                            }
                            sbSelectedAppNo.Append(appNo + ",");
                        }
                    }
                }
                if (checkedcount == 0)
                {
                    lblAlertMsg.Text = "Please Select Atleast Any one Student";
                    divPopAlert.Visible = true;
                    return;
                }
                if (lstSelectedAppNo.Count > 0)
                {
                    //ReportCard_Class_I_And_II(rollnos.Trim().Trim(','));
                    AcademicPerformanceReport(lstSelectedAppNo, rollnos.Trim().Trim(','));
                }
            }
            else
            {
                lblAlertMsg.Text = "No Student Were Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #region Settings

    protected void btnMarkTypeSettings_Click(object sender, EventArgs e)
    {
        try
        {
            divSubjectSetting.Visible = false;
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

            DataTable dtStudentDetails = new DataTable();

            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }

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
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(srh.Sections,''))) in('" + secValue + "')";
                }
            }
            string selectedBestOfSubject = string.Empty;

            selectedBestOfSubject = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='IX-X BestOfSubjects' and college_code= '" + ddlCollege.SelectedValue + "'");
            //if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            //{
            //    testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
            //    testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            //    if (!string.IsNullOrEmpty(testNo))
            //    {
            //        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            //    }
            //    else
            //    {
            //        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else if (cblTest.Items.Count > 0 && txtTest.Visible && txtTest.Enabled)
            //{
            //    testNo = getCblSelectedValue(cblTest);
            //    testName = getCblSelectedText(cblTest);
            //    if (!string.IsNullOrEmpty(testNo))
            //    {
            //        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            //    }
            //    else
            //    {
            //        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
            //        divPopAlert.Visible = true;
            //        return;
            //    }                
            //}
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}
            DataTable dtSubjects = new DataTable();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                //modified on 5/12/2017 by prabha 
                //requirement :  subject should be loaded based on the Selected sem
                //solution  :   lesserthan sem condition in the qry have been changed as equalto sem
                qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.acronym,ISNULL(s.subjectpriority,'0') as subjectpriority,ISNULL( ss.isSingleSubject,'0') as isSingleSubject,ISNULL(s.subjectMarkType,'1') as subjectMarkOrGrade,s.subject_no,ISNULL(isCompulsaoryForGrandTotal,'0') isCompulsaoryForGrandTotal,ISNULL(isConsiderForGrandTotal,'0') isConsiderForGrandTotal from subject s,syllabus_master sm,sub_sem ss where sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "' and sm.semester='" + semester + "' order by sm.Batch_Year,sm.degree_code,sm.semester,subjectpriority,ss.subType_no,s.subject_code;";

                if (isSubjectType)
                    qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,ss.subject_type,ss.subType_no,'' subject_name,'' subject_code,'' acronym,'0' as subjectpriority,ISNULL(ss.isSingleSubject,'0') as isSingleSubject,'1' as subjectMarkOrGrade,'' subject_no,'0' isCompulsaoryForGrandTotal ,'0' isConsiderForGrandTotal from subject s,syllabus_master sm,sub_sem ss where sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "' and sm.semester='" + semester + "' order by sm.Batch_Year,sm.degree_code,sm.semester,subjectpriority,ss.subType_no,s.subject_code";
                dtSubjects = dirAcc.selectDataTable(qry);
            }
            if (dtSubjects.Rows.Count > 0)
            {
                Init_Spread(FpSubjectList, 1);
                Farpoint.CheckBoxCellType chkall = new Farpoint.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Farpoint.CheckBoxCellType chkSelect = new Farpoint.CheckBoxCellType();
                FpSubjectList.Sheets[0].RowCount = 1;
                for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
                {
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, col].CellType = chkall;
                }
                int serialNo = 0;
                foreach (DataRow drSubject in dtSubjects.Rows)
                {
                    serialNo++;
                    FpSubjectList.Sheets[0].RowCount++;
                    string subjectCodeNew = Convert.ToString(drSubject["subject_code"]).Trim();
                    string subjectNameNew = Convert.ToString(drSubject["subject_name"]).Trim();
                    string subjectTypeNew = Convert.ToString(drSubject["subject_type"]).Trim();
                    string subjectTypeNoNew = Convert.ToString(drSubject["subType_no"]).Trim();
                    string subjectNoNew = Convert.ToString(drSubject["subject_no"]).Trim();
                    string subjectSemester = Convert.ToString(drSubject["semester"]).Trim();
                    string subjectMarkOrGrade = Convert.ToString(drSubject["subjectMarkOrGrade"]).Trim();
                    string isSingleSubject = Convert.ToString(drSubject["isSingleSubject"]).Trim();
                    string isCompulsaoryForGrandTotal = Convert.ToString(drSubject["isCompulsaoryForGrandTotal"]).Trim();
                    string isConsiderForGrandTotal = Convert.ToString(drSubject["isConsiderForGrandTotal"]).Trim();

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjectSemester);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectTypeNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(subjectTypeNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(subjectCodeNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(subjectNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(subjectNameNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(subjectNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].CellType = chkSelect;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].CellType = chkSelect;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].CellType = chkSelect;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 0;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].Value = 0;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Value = 0;

                    FpSubjectList.Sheets[0].Columns[0].Visible = true;
                    FpSubjectList.Sheets[0].Columns[1].Visible = true;
                    FpSubjectList.Sheets[0].Columns[2].Visible = true;
                    FpSubjectList.Sheets[0].Columns[3].Visible = true;
                    FpSubjectList.Sheets[0].Columns[4].Visible = true;
                    FpSubjectList.Sheets[0].Columns[5].Visible = true;
                    FpSubjectList.Sheets[0].Columns[6].Visible = true;
                    FpSubjectList.Sheets[0].Columns[7].Visible = true;

                    if (isSubjectType)
                    {
                        FpSubjectList.Sheets[0].Columns[0].Visible = true;
                        FpSubjectList.Sheets[0].Columns[1].Visible = true;
                        FpSubjectList.Sheets[0].Columns[2].Visible = true;
                        FpSubjectList.Sheets[0].Columns[3].Visible = false;
                        FpSubjectList.Sheets[0].Columns[4].Visible = false;
                        FpSubjectList.Sheets[0].Columns[5].Visible = true;
                        FpSubjectList.Sheets[0].Columns[6].Visible = false;
                        FpSubjectList.Sheets[0].Columns[7].Visible = false;
                        FpSubjectList.Sheets[0].Columns[8].Visible = false;
                        if (isSingleSubject.Trim() == "1" || isSingleSubject.Trim().ToLower() == "true")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 0;
                        }
                    }
                    else
                    {
                        if (subjectMarkOrGrade.Trim() == "1")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                        else if (subjectMarkOrGrade.Trim() == "2")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                        if (isCompulsaoryForGrandTotal.Trim().ToLower() == "1" || isCompulsaoryForGrandTotal.Trim().ToLower() == "true")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 8].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 8].Value = 0;
                        }
                        if (isConsiderForGrandTotal.Trim().ToLower() == "1" || isConsiderForGrandTotal.Trim().ToLower() == "true")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Value = 0;
                        }

                    }

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                }
                Farpoint.TextCellType txt = new Farpoint.TextCellType();
                if (!isSubjectType)
                {
                    FpSubjectList.Sheets[0].RowCount++;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].CellType = txt;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].CellType = txt;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 8].CellType = txt;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].Text = "Best Of ";
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].ColumnSpan = 7;
                    if (!string.IsNullOrEmpty(selectedBestOfSubject))
                        FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Text = selectedBestOfSubject;
                    else
                        FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Text = "4";
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].ColumnSpan = 2;

                }
                divSubjectSetting.Visible = true;
                FpSubjectList.Visible = true;
                FpSubjectList.Sheets[0].PageSize = FpSubjectList.Sheets[0].RowCount;
                FpSubjectList.Height = 350;
                FpSubjectList.Width = 800;
                FpSubjectList.SaveChanges();
            }
            else
            {
                lblAlertMsg.Text = "No Subject(s) Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bool isSaved = false;
    //        bool isSubjectType = false;
    //        if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
    //        {
    //            isSubjectType = true;
    //        }
    //        if (FpSubjectList.Sheets[0].RowCount > 1)
    //        {
    //            for (int row = 1; row < FpSubjectList.Sheets[0].RowCount; row++)
    //            {
    //                string subjectNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 4].Tag).Trim();
    //                string subjectTypeNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 2].Tag).Trim();
    //                string markType = string.Empty;
    //                for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
    //                {
    //                    string typeVal = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, col].Value).Trim();
    //                    if (isSubjectType)
    //                    {
    //                        markType = typeVal;
    //                        break;
    //                    }
    //                    else
    //                    {
    //                        if (typeVal == "1")
    //                        {
    //                            if (col == 5)
    //                                markType = "1";
    //                            else if (col == 6)
    //                                markType = "2";
    //                            break;
    //                        }
    //                    }
    //                }
    //                if (isSubjectType)
    //                {
    //                    if (!string.IsNullOrEmpty(subjectTypeNo))
    //                    {
    //                        qry = "update sub_sem set isSingleSubject='" + markType + "' where subType_no='" + subjectTypeNo + "'";
    //                        int upd = dirAcc.updateData(qry);
    //                        if (upd != 0)
    //                            isSaved = true;
    //                    }
    //                }
    //                else
    //                {
    //                    if (!string.IsNullOrEmpty(subjectNo))
    //                    {
    //                        qry = "update subject set subjectMarkType='" + markType + "' where subject_no='" + subjectNo + "'";
    //                        int upd = dirAcc.updateData(qry);
    //                        if (upd != 0)
    //                            isSaved = true;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            lblAlertMsg.Text = "No Record(s) Were Found";
    //            divPopAlert.Visible = true;
    //            return;
    //        }
    //        lblAlertMsg.Text = (isSaved) ? "Saved Successfully" : "Not Saved";
    //        divPopAlert.Visible = true;
    //        return;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSaved = false;
            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }
            FpSubjectList.SaveChanges();
            if (FpSubjectList.Sheets[0].RowCount > 1)
            {
                for (int row = 1; row < FpSubjectList.Sheets[0].RowCount - 1; row++)
                {
                    string subjectNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 4].Tag).Trim();
                    string subjectTypeNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 2].Tag).Trim();
                    string markType = string.Empty;
                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount - 2; col++)
                    {
                        string typeVal = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, col].Value).Trim();
                        if (isSubjectType)
                        {
                            markType = typeVal;
                            break;
                        }
                        else
                        {
                            if (typeVal == "1")
                            {
                                if (col == 5)
                                    markType = "1";
                                else if (col == 6)
                                    markType = "2";
                                break;
                            }
                        }
                    }
                    string compulsaryfrgrandtotal = string.Empty;
                    string considerfrgrandtotal = string.Empty;

                    if (FpSubjectList.Sheets[0].Cells[row, FpSubjectList.Sheets[0].ColumnCount - 1].Value == null || Convert.ToString(FpSubjectList.Sheets[0].Cells[row, FpSubjectList.Sheets[0].ColumnCount - 1].Value) == "0")
                        compulsaryfrgrandtotal = "0";
                    else
                        compulsaryfrgrandtotal = "1";

                    if (FpSubjectList.Sheets[0].Cells[row, FpSubjectList.Sheets[0].ColumnCount - 2].Value == null || Convert.ToString(FpSubjectList.Sheets[0].Cells[row, FpSubjectList.Sheets[0].ColumnCount - 2].Value) == "0")
                        considerfrgrandtotal = "0";
                    else
                        considerfrgrandtotal = "1";


                    if (isSubjectType)
                    {
                        if (!string.IsNullOrEmpty(subjectTypeNo))
                        {
                            qry = "update sub_sem set isSingleSubject='" + markType + "' where subType_no='" + subjectTypeNo + "'";
                            int upd = dirAcc.updateData(qry);
                            if (upd != 0)
                                isSaved = true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(subjectNo))
                        {
                            qry = "update subject set subjectMarkType='" + markType + "' , isConsiderForGrandTotal='" + considerfrgrandtotal + "' , isCompulsaoryForGrandTotal='" + compulsaryfrgrandtotal + "'  where subject_no='" + subjectNo + "'";
                            int upd = dirAcc.updateData(qry);
                            if (upd != 0)
                                isSaved = true;
                        }
                    }
                }
                string BestOfSubjects = Convert.ToString(FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 7].Value);
                string bestOfSubjectsqry = "if exists (select LinkValue from New_InsSettings where LinkName='IX-X BestOfSubjects' and college_code='" + ddlCollege.SelectedValue + "' )update New_InsSettings set  LinkValue='" + BestOfSubjects + "'  where LinkName='IX-X BestOfSubjects' and college_code='" + ddlCollege.SelectedValue + "'  else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) values('IX-X BestOfSubjects','" + BestOfSubjects + "','0','" + ddlCollege.SelectedValue + "')";

                int res = dirAcc.updateData(bestOfSubjectsqry);
                if (res != 0)
                    isSaved = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
            lblAlertMsg.Text = (isSaved) ? "Saved Successfully" : "Not Saved";
            divPopAlert.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        try
        {
            divSubjectSetting.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSubjectList_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int r = FpSubjectList.Sheets[0].ActiveRow;
            int j = FpSubjectList.Sheets[0].ActiveColumn;
            int k = Convert.ToInt32(j);

            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);
            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }
            if (r >= 0 && FpSubjectList.Sheets[0].ColumnHeader.Cells[0, j].Text.Trim().ToLower() != "select" && j != FpSubjectList.Sheets[0].ColumnCount - 2 && !isSubjectType && j != FpSubjectList.Sheets[0].ColumnCount - 1)
            {
                if (Convert.ToInt32(r) == 0)
                {
                    if (r.ToString().Trim() != "" && j.ToString().Trim() != "")
                    {
                        if (FpSubjectList.Sheets[0].RowCount > 0)
                        {
                            int checkval = Convert.ToInt32(FpSubjectList.Sheets[0].Cells[a, b].Value);
                            if (checkval == 0)
                            {
                                string headervalue = Convert.ToString(FpSubjectList.Sheets[0].ColumnHeader.Cells[0, b].Tag);
                                for (int i = 1; i < FpSubjectList.Sheets[0].RowCount; i++)
                                {
                                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount - 2; col++)
                                    {
                                        if (col != b)
                                        {
                                            FpSubjectList.Sheets[0].Cells[i, col].Value = 0;
                                            FpSubjectList.Sheets[0].Cells[0, col].Value = 0;
                                        }
                                        else
                                        {
                                            FpSubjectList.Sheets[0].Cells[i, col].Value = 1;
                                            FpSubjectList.Sheets[0].Cells[0, col].Value = 1;
                                        }
                                    }

                                }
                            }
                            else if (checkval == 1)
                            {
                                for (int i = 1; i < FpSubjectList.Sheets[0].RowCount - ((isSubjectType) ? 0 : 1); i++)
                                {
                                    FpSubjectList.Sheets[0].Cells[i, b].Value = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string headervalue = Convert.ToString(FpSubjectList.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(j)].Tag);

                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount - 2; col++)
                    {
                        if (col != j)
                        {
                            FpSubjectList.Sheets[0].Cells[a, col].Value = 0;
                        }
                    }
                }
            }
            else if (FpSubjectList.Sheets[0].ColumnHeader.Cells[0, j].Text.Trim().ToLower() != "select")
            {
                if (r == 0)
                {
                    if (FpSubjectList.Sheets[0].RowCount > 0)
                    {
                        string checkval = Convert.ToString(FpSubjectList.Sheets[0].Cells[a, j].Value);
                        int value = 0;
                        for (int i = 1; i < FpSubjectList.Sheets[0].RowCount - ((isSubjectType) ? 0 : 1); i++)
                        {
                            if (string.IsNullOrEmpty(checkval) || checkval == "0")
                            {
                                value = 1;
                            }
                            else if (checkval == "1")
                            {
                                value = 0;
                            }
                            FpSubjectList.Sheets[0].Cells[i, j].Value = value;
                            FpSubjectList.Sheets[0].Cells[0, j].Value = value;
                        }
                    }
                }
                else
                {
                    //if (FpSubjectList.Sheets[0].RowCount > 0)
                    //{
                    //    int checkval = Convert.ToInt32(FpSubjectList.Sheets[0].Cells[a, b].Value);
                    //}
                }
            }
        }
        catch
        {

        }
    }

    #endregion

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
                spPageHeading.InnerHtml = "Performance Report Card ICSE IX - X";
                Page.Title = "Performance Report Card ICSE IX - X";
            }
            else
            {
                lblBatch.Text = "Batch";
                spPageHeading.InnerHtml = "Performance Report Card ICSE IX - X";
                Page.Title = "Performance Report Card ICSE IX - X";
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

            orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
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

    public void CalculateRankByPercentage(Dictionary<string, double> dicTotalMarks, Dictionary<string, double> dicTotalPercentage, ref DataTable dtRankList)
    {
        try
        {
            dicTotalPercentage = dicTotalPercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dtRankList = new DataTable();
            dtRankList.Clear();
            dtRankList.Columns.Add("AppNo");
            dtRankList.Columns.Add("Total");
            dtRankList.Columns.Add("Rank");
            DataRow drRankList;
            int rank = 1;
            double previousPercentage = 0;
            foreach (KeyValuePair<string, double> keyPercentage in dicTotalPercentage)
            {
                string keyAppNo = keyPercentage.Key.Trim();
                double currentPercentage = keyPercentage.Value;
                double totalMark = 0;
                if (dicTotalMarks.ContainsKey(keyAppNo))
                {
                    totalMark = dicTotalMarks[keyAppNo];
                }
                if (previousPercentage != 0 && previousPercentage != currentPercentage)
                {
                    rank++;
                }
                previousPercentage = currentPercentage;
                drRankList = dtRankList.NewRow();
                drRankList["AppNo"] = keyAppNo;
                drRankList["Total"] = totalMark;
                drRankList["Rank"] = rank;
                dtRankList.Rows.Add(drRankList);
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

    /*private void AcademicPerformanceReport1(List<string> lstAppNo, string rollNo)
    {
        try
        {
            #region Font Creation

            Font fontColName = new Font("Times New Roman", 18, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Times New Roman", 15, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Times New Roman", 18, FontStyle.Bold);
            Font fontstudClass = new Font("Times New Roman", 15, FontStyle.Bold);
            Font fontReportContent = new Font("Times New Roman", 11, FontStyle.Regular);
            Font fontReportContentBold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontReportStudProfileHeader = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontReportFooter = new Font("Times New Roman", 10, FontStyle.Regular);

            #endregion Font Creation

            string appNoList = string.Empty;
            StringBuilder sbErr = new StringBuilder();

            DataTable dtStudentDetails = new DataTable();
            DataTable dtStudentPhotos = new DataTable();
            DataTable dtCollegeDetails = new DataTable();
            DataTable dtStudentAllMarksDetails = new DataTable();
            DataTable dtStudentMarksDetails = new DataTable();
            DataTable dtStudentActivities = new DataTable();
            DataTable dtSem = new DataTable();
            DataTable dtGradeDetails = new DataTable();
            DataTable dtGeneralGrade = new DataTable();

            DataSet dsStudMarks = new DataSet();
            DataSet dsPart = new DataSet();
            DataSet dsAttRemark = new DataSet();

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

            Gios.Pdf.PdfDocument pdfDocumentReport = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage pdfReportPage;
            bool status = false;
            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                orderBy = orderByStudents(collegeCode, "r");
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
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(e.Sections,''))) in('" + secValue + "')";
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
            else if (cblTest.Items.Count > 0 && txtTest.Visible && txtTest.Enabled)
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
            if (lstAppNo.Count > 0)
            {
                appNoList = string.Join("','", lstAppNo.ToArray());

                if (!string.IsNullOrEmpty(appNoList))
                {
                    qry = "select distinct r.serialno,r.App_No,r.college_code,r.Roll_No,r.Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code  and r.app_no in ('" + appNoList + "') " + orderBy;
                    dtStudentDetails = dirAcc.selectDataTable(qry);

                    qry = "select CONVERT(VARCHAR(30),start_date,103) as start_date ,CONVERT(VARCHAR(30),end_date,103) as end_date from seminfo where semester='" + (semester) + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "'";
                    dtSem = dirAcc.selectDataTable(qry);

                    qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,c.criteria,c.Criteria_no,ss.subject_type,ss.subType_no,ISNULL(s.subjectMarkType,'1') as subjectMarkType,s.subject_name,s.acronym,s.subject_code,LTRIM(RTRIM(ISNULL(e.sections,''))) sections,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,ISNULL(s.subjectpriority,'0') as subjectpriority,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,ISNULL( ss.isSingleSubject,'0') as isSingleSubject from Exam_type e,subject s,CriteriaForInternal c,syllabus_master sm,Result re,sub_sem ss where sm.syll_code=s.syll_code and c.syll_code=sm.syll_code and c.syll_code=s.syll_code and sm.Batch_Year=e.batch_year and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and re.exam_code=e.exam_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "'  and c.criteria in(" + testName + ") and sm.semester<='" + semester + "' " + qrySection1 + "  order by sm.Batch_Year,sm.degree_code,sm.semester,c.criteria,c.Criteria_no,subjectpriority,ss.subType_no,s.subject_code; select rg.App_No,r.roll_no,rg.Reg_No,rg.Stud_Name,rg.Roll_Admit,sm.Batch_Year,rg.degree_code,rg.Current_Semester,sm.semester,rg.sections,c.Criteria_no,c.criteria,e.exam_code,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,s.subject_code,s.subject_no,ss.subject_type,ss.subType_no,s.syll_code,ISNULL(s.subjectMarkType,'1') as subjectMarkType,s.subject_name,s.acronym,ISNULL(s.subjectpriority,'0') as subjectpriority,r.marks_obtained,isnull(r.remarks,'') as remarks,convert(varchar(10),e.exam_date,103)as exam_date,LTRIM(RTRIM(ISNULL(e.sections,''))) sections,ISNULL( ss.isSingleSubject,'0') as isSingleSubject from Registration rg,CriteriaForInternal c,Exam_type e,Result r,syllabus_master sm,subject s,sub_sem ss where rg.Roll_No =r.roll_no and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and e.exam_code=r.exam_code and e.batch_year=rg.Batch_Year and e.sections=rg.Sections and sm.Batch_Year=rg.Batch_Year and rg.degree_code=sm.degree_code and sm.syll_code=s.syll_code and sm.syll_code=c.syll_code and e.batch_year=sm.Batch_Year and cc=0 and delflag=0 and exam_flag<>'Debar' and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and rg.Batch_Year='" + batchYear + "' and rg.degree_code='" + degreeCode + "' and rg.college_code='" + collegeCode + "' and c.criteria in(" + testName + ") and sm.semester<='" + semester + "' " + qrySection1 + " and rg.App_no in('" + appNoList + "') order by rg.Roll_No,c.Criteria_no,subjectpriority,ss.subType_no,s.subject_code;";
                    dsStudMarks = dirAcc.selectDataSet(qry);

                    qry = "SELECT * from CoCurrActivitie_Det where  istype='Att'and Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and term in(" + semester + ") and Roll_No in(" + rollNo + ");SELECT * from CoCurrActivitie_Det where istype='remks' and Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and term in(" + semester + ") and Roll_No in(" + rollNo + "); ";
                    dsAttRemark.Clear();
                    dsAttRemark.Reset();
                    dsAttRemark = dirAcc.selectDataSet(qry);

                    qry = "select distinct PartName,Part_No from CoCurr_Activitie where Batch_Year='" + batchYear + "' and Degree_Code='" + degreeCode + "' and ISNULL(Part_No,'0')<>'0' and Part_No<>'1'; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term from activity_entry ae,CoCurr_Activitie ca,TextValTable tv where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "' and ae.term in(" + semester + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "'  and ae.term in(" + semester + ") and det.Roll_No in(" + rollNo + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark,ag.Grade,ag.description,ag.frompoint,ag.topoint from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det,activity_gd ag where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and ag.batch_year=det.Batch_Year and ag.batch_year=ae.Batch_Year and ag.batch_year=ca.Batch_Year and ae.Degree_Code=ag.Degree_Code and ag.Degree_Code=ca.Degree_Code and det.Degree_Code=ag.Degree_Code and ag.term=det.term and ag.term=ae.term and tv.TextCode=ag.ActivityTextVal and ae.ActivityTextVal=ag.ActivityTextVal and det.ActivityTextVal=ag.ActivityTextVal and det.Mark between ag.frompoint and ag.topoint and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "' and ae.term in(" + semester + ") and det.Roll_No in(" + rollNo + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ";
                    dsPart.Clear();
                    dsPart.Reset();
                    dsPart = dirAcc.selectDataSet(qry);

                    qry = "select * from StdPhoto where app_no in('" + appNoList + "')";
                    dtStudentPhotos = dirAcc.selectDataTable(qry);

                    qry = "SELECT college_code,collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,website from collinfo where college_code='" + collegeCode + "';";
                    dtCollegeDetails = dirAcc.selectDataTable(qry);

                    qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc";
                    dtGradeDetails = dirAcc.selectDataTable(qry);
                    if (dtGradeDetails.Rows.Count > 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                }
                if (dtStudentDetails.Rows.Count > 0)
                {
                    foreach (DataRow drStudent in dtStudentDetails.Rows)
                    {
                        int posX = 15;
                        int posY = 25;
                        bool isReportSave = false;
                        string appNo = Convert.ToString(drStudent["App_No"]).Trim();
                        string studentRollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                        string studentName = Convert.ToString(drStudent["stud_name"]).Trim();
                        string studentRollAdmit = Convert.ToString(drStudent["roll_admit"]).Trim();
                        string admitDate = Convert.ToString(drStudent["adm_date"]).Trim();
                        string studentBatch = Convert.ToString(drStudent["Batch_Year"]).Trim();
                        string studentDegreeCode = Convert.ToString(drStudent["degree_code"]).Trim();
                        string studentSemester = Convert.ToString(drStudent["Current_Semester"]).Trim();
                        string studentSections = Convert.ToString(drStudent["Sections"]).Trim();
                        string studentRegNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                        string studentDepartment = Convert.ToString(drStudent["Dept_Name"]).Trim();
                        string studentFatherName = Convert.ToString(drStudent["parent_name"]).Trim();
                        string studentMotherName = Convert.ToString(drStudent["mother"]).Trim();
                        string studentHeight = Convert.ToString(drStudent["StudHeight"]).Trim();
                        string studentWeight = Convert.ToString(drStudent["StudWeight"]).Trim();
                        string studentBloodGroup = Convert.ToString(drStudent["Blood_Grp"]).Trim();
                        string studentHouse = Convert.ToString(drStudent["studhouse"]).Trim();
                        string studentDOB = Convert.ToString(drStudent["dob"]).Trim();
                        string studentCollegeCode = Convert.ToString(drStudent["college_code"]).Trim();
                        string studClassandSec = ((!string.IsNullOrEmpty(studentDepartment)) ? studentDepartment + ((!string.IsNullOrEmpty(studentSections)) ? " & " + studentSections : "") : "");

                        string academicYear = string.Empty;
                        int currentYear = DateTime.Today.Year;
                        int batchStartYear = 0;
                        int.TryParse(batchYear, out batchStartYear);
                        academicYear = batchStartYear.ToString() + " - " + Convert.ToString((batchStartYear + 1));

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

                        if (dtSem.Rows.Count > 0)
                        {
                            frdate = Convert.ToString(dtSem.Rows[0]["start_date"]).Trim();
                            todate = Convert.ToString(dtSem.Rows[0]["end_date"]).Trim();
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

                            persentmonthcal(studentCollegeCode, studentDegreeCode, semester, rollNo, admitDate);

                            double absenthours = per_workingdays1 - per_per_hrs;
                            double per_tage_date = 0;
                            if (per_workingdays > 0)
                            {
                                per_tage_date = ((pre_present_date / per_workingdays) * 100);
                            }
                            if (per_tage_date > 100)
                            {
                                per_tage_date = 100;
                            }

                            double per_tage_hrs = 0;
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

                        string clgname = string.Empty;
                        string clgaff = string.Empty;
                        string clgaddress1 = string.Empty;
                        string clgaddress2 = string.Empty;
                        string clgaddress3 = string.Empty;
                        string clgdistrict = string.Empty;
                        string clgpincode = string.Empty;
                        string clgemail = string.Empty;
                        string clgfulladdress = string.Empty;
                        int totalParts = 0;

                        DataTable dtCurrentSemesterSubject = new DataTable();
                        DataTable dtCurrentStudentMarks = new DataTable();
                        DataTable dtPreviousSemesterSubject = new DataTable();
                        DataTable dtPreviousStudentMarks = new DataTable();
                        DataTable dtDistinctSubjects = new DataTable();
                        DataTable dtDistinctTestName = new DataTable();
                        DataTable dtDistinctCommonTestName = new DataTable();
                        DataTable dtCollegeInfo = new DataTable();
                        DataTable dtStudActivityMarks = new DataTable();
                        DataTable dtStudPart = new DataTable();
                        DataTable dtDistinctSingleSubject = new DataTable();
                        DataTable dtCurrentSingleSubject = new DataTable();
                        DataTable dtCurrentSingleSubjectType = new DataTable();
                        DataTable dtPreviousSingleSubject = new DataTable();

                        PdfImage CollegeLeftLogo = null;
                        PdfImage collegeRightLogo = null;
                        PdfImage studPhoto = null;
                        PdfLine pdfLine;
                        PdfTextArea pdfTA;
                        PdfTablePage tblPage;
                        PdfTable tblStudentDetails;
                        PdfTable tblStudentMarks;
                        PdfTable tblStudentActivity;
                        PdfTable tblStudentPerformanceAnalysis;
                        PdfTable tblSign;

                        bool hasActivity = false;

                        if (dtCollegeDetails.Rows.Count > 0)
                        {
                            dtCollegeDetails.DefaultView.RowFilter = "college_code='" + studentCollegeCode + "'";
                            dtCollegeInfo = dtCollegeDetails.DefaultView.ToTable();
                        }
                        if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0)
                        {
                            dsStudMarks.Tables[0].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "'";
                            dsStudMarks.Tables[0].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtDistinctSubjects = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");

                            dsStudMarks.Tables[0].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and isSingleSubject=1";
                            dsStudMarks.Tables[0].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtDistinctSingleSubject = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");
                        }
                        if (dsStudMarks.Tables.Count > 1 && dsStudMarks.Tables[1].Rows.Count > 0)
                        {
                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtCurrentSemesterSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and isSingleSubject=1";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtCurrentSingleSubjectType = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");
                            dtCurrentSingleSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subjectMarkType", "subject_code", "subject_name", "isSingleSubject");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and subject_type<>'others' and subject_type<>'other'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtPreviousSemesterSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "Criteria_no";
                            dtDistinctTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria", "Criteria_no", "Convert_Minmark", "Convert_Maxmark");
                            dtDistinctCommonTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and App_No='" + appNo + "'";
                            dtCurrentStudentMarks = dsStudMarks.Tables[1].DefaultView.ToTable();

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester<'" + semester + "' and sections='" + section + "' and App_No='" + appNo + "'";
                            dtPreviousStudentMarks = dsStudMarks.Tables[1].DefaultView.ToTable();

                        }
                        if (dsPart.Tables.Count > 0)
                        {
                            if (dsPart.Tables[0].Rows.Count > 0)
                            {
                                totalParts = dsPart.Tables[0].Rows.Count;
                            }
                            if (dsPart.Tables.Count >= 2 && dsPart.Tables[1].Rows.Count > 0)
                            {
                                dsPart.Tables[1].DefaultView.RowFilter = string.Empty;
                                dtStudPart = dsPart.Tables[1].DefaultView.ToTable(true);
                            }
                            if (dsPart.Tables.Count >= 3 && dsPart.Tables[2].Rows.Count > 0)
                            {
                                dsPart.Tables[2].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "'";
                                dtStudPart = dsPart.Tables[2].DefaultView.ToTable(true, "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle");
                                //ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark
                            }
                            if (dsPart.Tables.Count >= 4 && dsPart.Tables[3].Rows.Count > 0)
                            {
                                dsPart.Tables[3].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "'";
                                dtStudActivityMarks = dsPart.Tables[3].DefaultView.ToTable(true, "Roll_No", "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle", "term", "Mark", "Grade", "description");
                                totalParts = dsPart.Tables[3].DefaultView.ToTable(true, "Part_No").Rows.Count;
                                if (dtStudActivityMarks.Rows.Count > 0)
                                {
                                    hasActivity = true;
                                }
                            }
                        }
                        else
                        {
                            hasActivity = false;
                        }
                        if (dtCurrentStudentMarks.Rows.Count > 0)
                        {
                            isReportSave = true;
                            status = true;
                            pdfReportPage = pdfDocumentReport.NewPage();
                            MemoryStream memoryStream = new MemoryStream();
                            if (dtStudentPhotos.Rows.Count > 0)
                            {
                                byte[] file = (byte[])dtStudentPhotos.Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg")))
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg")))
                            {
                                studPhoto = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg"));
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg")))
                                {
                                    studPhoto = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                }
                            }
                            if (dtCollegeInfo.Rows.Count > 0)
                            {
                                clgname = Convert.ToString(dtCollegeInfo.Rows[0]["collname"]);
                                clgaff = "(" + Convert.ToString(dtCollegeInfo.Rows[0]["affliatedby"]) + ")";
                                clgaddress1 = Convert.ToString(dtCollegeInfo.Rows[0]["address1"]).Trim();
                                clgaddress2 = Convert.ToString(dtCollegeInfo.Rows[0]["address2"]).Trim();
                                clgaddress3 = Convert.ToString(dtCollegeInfo.Rows[0]["address3"]).Trim();
                                clgdistrict = Convert.ToString(dtCollegeInfo.Rows[0]["district"]).Trim();
                                clgpincode = Convert.ToString(dtCollegeInfo.Rows[0]["pincode"]).Trim();
                                clgemail = "Email : " + Convert.ToString(dtCollegeInfo.Rows[0]["email"]).Trim();
                                clgfulladdress = string.Empty;

                                #region College Address

                                if (clgaddress1.Trim().Trim(',') != "")
                                {
                                    clgfulladdress = clgaddress1.Trim().Trim(',');
                                }
                                if (clgaddress2.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += ", " + clgaddress2.Trim().Trim(',');
                                    }
                                    else
                                    {
                                        clgfulladdress = clgaddress2.Trim().Trim(',');
                                    }
                                }
                                if (clgaddress3.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += ", " + clgaddress3.Trim().Trim(',');
                                    }
                                    else
                                    {
                                        clgfulladdress = clgaddress3.Trim().Trim(',');
                                    }
                                }
                                if (clgdistrict.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += ", " + clgdistrict.Trim().Trim(',');
                                    }
                                    else
                                    {
                                        clgfulladdress = clgdistrict.Trim().Trim(',');
                                    }
                                }
                                if (clgpincode.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += "-" + clgpincode.Trim().Trim(',').Trim('.') + ".";
                                    }
                                    else
                                    {
                                        clgfulladdress = clgpincode.Trim().Trim(',').Trim('.') + ".";
                                    }
                                }

                                #endregion College Address

                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    byte[] file = (byte[])dtCollegeInfo.Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    CollegeLeftLogo = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"));
                                    pdfReportPage.Add(CollegeLeftLogo, posX, posY, 540);
                                }

                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    byte[] file = (byte[])dtCollegeInfo.Rows[0]["logo2"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    collegeRightLogo = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg"));
                                    pdfReportPage.Add(collegeRightLogo, pdfDocumentReport.PageWidth - 60, posY, 540);

                                }
                            }

                            #region College Details

                            //pdfTA=new PdfTextArea(
                            posY += 10;
                            pdfTA = new PdfTextArea(fontColName, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgname);
                            pdfReportPage.Add(pdfTA);

                            posY += 18;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgaff);
                            pdfReportPage.Add(pdfTA);

                            posY += 18;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgfulladdress);
                            pdfReportPage.Add(pdfTA);

                            posY += 30;
                            pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "PERFORMANCE RECORD");
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Academic Year

                            posY += 20;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "Academic Year :" + academicYear);
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Student Details

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 2, 9, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 85, 8, 350, 158, 8, 180, 115, 8, 150 });

                            tblStudentDetails.Cell(0, 0).SetContent("Name");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 2).SetContent(studentName);
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetContent("Admission No");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(studentRollAdmit);
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 6).SetContent("Std & Sec");
                            tblStudentDetails.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 7).SetContent(":");
                            tblStudentDetails.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 8).SetContent(studClassandSec);
                            tblStudentDetails.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 0).SetContent("DOB");
                            tblStudentDetails.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 1).SetContent(":");
                            tblStudentDetails.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 2).SetContent(studentDOB);
                            tblStudentDetails.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 3).SetContent("Blood Group");
                            tblStudentDetails.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 4).SetContent(":");
                            tblStudentDetails.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 5).SetContent(studentBloodGroup);
                            tblStudentDetails.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 6).SetContent("House");
                            tblStudentDetails.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 7).SetContent(":");
                            tblStudentDetails.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 8).SetContent(studentHouse);
                            tblStudentDetails.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            posY += 30;
                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            double height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height));

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 1, 6, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 210, 5, 270, 133, 5, 230 });

                            tblStudentDetails.Cell(0, 0).SetContent("Father's / Guardian Name");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 2).SetContent(studentFatherName);
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetContent("Mother's Name");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(studentMotherName);
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height));

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 1, 12, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 38, 6, 40, 38, 6, 35, 30, 6, 30, 30, 6, 30 });

                            tblStudentDetails.Cell(0, 0).SetContent("Attendance");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            string attendanceDays = string.Empty;
                            if (per_workingdays > 0)
                            {
                                attendanceDays = ((pre_present_date > 0) ? Convert.ToString(pre_present_date).Trim() : " - ") + " / " + Convert.ToString(per_workingdays).Trim();
                                tblStudentDetails.Cell(0, 2).SetContent(attendanceDays + " Days.");
                            }
                            else
                                tblStudentDetails.Cell(0, 2).SetContent("     Days.");
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetContent("Percentage");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(((per_workingdays > 0) ? dum_tage_date : ""));
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 6).SetContent("Height");
                            tblStudentDetails.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 7).SetContent(":");
                            tblStudentDetails.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 8).SetContent((!string.IsNullOrEmpty(studentHeight) && studentHeight != "0") ? studentHeight + " cm" : "");
                            tblStudentDetails.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 9).SetContent("Weight");
                            tblStudentDetails.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 10).SetContent(":");
                            tblStudentDetails.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 11).SetContent((!string.IsNullOrEmpty(studentWeight) && studentWeight != "0") ? studentWeight + " kg" : "");
                            tblStudentDetails.Cell(0, 11).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height)) + 5;

                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 40, posY, pdfDocumentReport.PageWidth - 80, 20), ContentAlignment.MiddleCenter, "-----------------------------------------------------------------------------------------------------------");
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Performance Analysis

                            Dictionary<string, double> dicSubjectWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicTestWiseTotal = new Dictionary<string, double>();

                            Dictionary<string, Dictionary<string, double>> dicTermWiseTotal = new Dictionary<string, Dictionary<string, double>>();

                            Dictionary<string, double> dicSubjectTypeWiseAverageOld = new Dictionary<string, double>();
                            Dictionary<string, double> dicSubjectTypeTestWiseAverageOld = new Dictionary<string, double>();
                            Dictionary<string, double> dicSingleSubjectTestWiseTotalOld = new Dictionary<string, double>();

                            Dictionary<string, double> dicSingleSubjectWiseTotalOld = new Dictionary<string, double>();
                            Dictionary<string, double> dicCommonSubjectWiseTotalOld = new Dictionary<string, double>();
                            if (dtPreviousStudentMarks.Rows.Count > 0)
                            {
                                DataTable dtTerm = new DataTable();
                                dtTerm = dtPreviousStudentMarks.DefaultView.ToTable(true, "semester");

                                string conductedTestNames = string.Empty;
                                List<string> lstTestName = dtDistinctCommonTestName.AsEnumerable().Select(r => r.Field<string>("criteria")).ToList();
                                conductedTestNames = string.Join("','", lstTestName.ToArray());

                                int termRow = 0;
                                foreach (DataRow drTerm in dtTerm.Rows)
                                {
                                    string term = Convert.ToString(drTerm["semester"]).Trim();
                                    double maxTestMark = 0;
                                    dicSubjectWiseTotal.Clear();
                                    dicTestWiseTotal.Clear();
                                    Dictionary<string, double> dicSubjectWiseOverAllTotal = new Dictionary<string, double>();
                                    termRow++;
                                    int subjectCol = 1;
                                    string displayTerm = "Term - " + ToRoman(term);
                                    Dictionary<string, double> dicSingleSubject = new Dictionary<string, double>();
                                    string previousSubjectTypeOld = string.Empty;
                                    string previousSubjectTypeNoOld = string.Empty;
                                    foreach (DataRow drSubject in dtPreviousSemesterSubject.Rows)
                                    {
                                        string subjectTCode = Convert.ToString(drSubject["subject_code"]).Trim();
                                        string subjectTName = Convert.ToString(drSubject["subject_name"]).Trim();
                                        string markOrGrade = Convert.ToString(drSubject["subjectMarkType"]).Trim();
                                        string subjectTAcronymn = Convert.ToString(drSubject["acronym"]).Trim();
                                        string subjectTypeName = Convert.ToString(drSubject["subject_type"]).Trim();
                                        string subjectTypeNo = Convert.ToString(drSubject["subType_no"]).Trim();
                                        string subjectTSingle = Convert.ToString(drSubject["isSingleSubject"]).Trim();
                                        double maxSubjecTestMark = 0;
                                        int testColumn = 1;
                                        if (!string.IsNullOrEmpty(previousSubjectTypeNoOld) && previousSubjectTypeNoOld != subjectTypeNo)
                                        {
                                            double subjectTypeWiseTotal = 0;
                                            foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                            {
                                                string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                                string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                                string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                                string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                                double minimumTestMark = 0;
                                                double.TryParse(convertMin, out minimumTestMark);
                                                double maximumTestMark = 0;
                                                double.TryParse(convertMax, out maximumTestMark);
                                                maxTestMark += maximumTestMark;
                                                double subjectAvg = 0;
                                                if (dicSubjectTypeTestWiseAverageOld.ContainsKey(previousSubjectTypeNoOld.Trim() + "@" + studTestNo.Trim()))
                                                    subjectAvg = dicSubjectTypeTestWiseAverageOld[previousSubjectTypeNoOld.Trim() + "@" + studTestNo.Trim()];

                                                dtCurrentSemesterSubject.DefaultView.RowFilter = "subType_no='" + previousSubjectTypeNoOld + "'";
                                                DataTable dtDistSingleSubject = dtCurrentSemesterSubject.DefaultView.ToTable(true, "subject_code", "subject_name", "acronym");
                                                subjectAvg = subjectAvg / dtDistSingleSubject.Rows.Count;
                                                subjectTypeWiseTotal += subjectAvg;
                                                if (dicSingleSubjectTestWiseTotalOld.ContainsKey(studTestNo.Trim()))
                                                    dicSingleSubjectTestWiseTotalOld[studTestNo.Trim()] = subjectAvg;
                                                else
                                                    dicSingleSubjectTestWiseTotalOld.Add(studTestNo.Trim(), subjectAvg);
                                                if (testColumn == dtDistinctTestName.Rows.Count)
                                                {
                                                    double sum = Math.Round((subjectTypeWiseTotal / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                                    if (dicSingleSubjectWiseTotalOld.ContainsKey(previousSubjectTypeNoOld.Trim()))
                                                        dicSingleSubjectWiseTotalOld[previousSubjectTypeNoOld.Trim()] = subjectAvg;
                                                    else
                                                        dicSingleSubjectWiseTotalOld.Add(previousSubjectTypeNoOld.Trim(), subjectAvg);
                                                }
                                                testColumn++;
                                            }
                                            //subjectRow++;
                                        }
                                        if (subjectTSingle.Trim() == "1" || subjectTSingle.Trim().ToLower() == "true")
                                        {
                                            previousSubjectTypeNoOld = subjectTypeName;
                                            previousSubjectTypeNoOld = subjectTypeNo;
                                        }
                                        else
                                        {
                                            previousSubjectTypeNoOld = string.Empty;
                                            previousSubjectTypeNoOld = string.Empty;
                                        }
                                        subjectCol++;
                                        testColumn = 1;
                                        foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                        {
                                            string subjectMaxMark = string.Empty;
                                            string subjectminMark = string.Empty;
                                            string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                            string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                            string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                            string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                            double minimumTestMark = 0;
                                            double.TryParse(convertMin, out minimumTestMark);
                                            double maximumTestMark = 0;
                                            double.TryParse(convertMax, out maximumTestMark);
                                            maxTestMark += maximumTestMark;
                                            DataView dvStudMarks = new DataView();
                                            if (true)
                                            {
                                                dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and semester='" + term + "' and criteria='" + studTestName + "'";
                                                dvStudMarks = dtPreviousStudentMarks.DefaultView;
                                                if (dvStudMarks.Count > 0)
                                                {
                                                    subjectMaxMark = Convert.ToString(dvStudMarks[0]["Conducted_max"]).Trim();
                                                    subjectminMark = Convert.ToString(dvStudMarks[0]["Conduct_Minmark"]).Trim();
                                                    convertMin = Convert.ToString(dvStudMarks[0]["Convert_Minmark"]).Trim();
                                                    convertMax = Convert.ToString(dvStudMarks[0]["Convert_Maxmark"]).Trim();
                                                    string subjectMark = Convert.ToString(dvStudMarks[0]["marks_obtained"]).Trim();

                                                    double testSubMarks = 0;
                                                    double.TryParse(subjectMark, out testSubMarks);

                                                    double testMinMark = 0;
                                                    double.TryParse(convertMin, out testMinMark);

                                                    double testMaxMark = 0;
                                                    double.TryParse(convertMax, out testMaxMark);
                                                    maxSubjecTestMark += testMaxMark;

                                                    double testSubMinMarks = 0;
                                                    double.TryParse(subjectminMark, out testSubMinMarks);

                                                    double testSubMaxMarks = 0;
                                                    double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                                    double convertTestMark = testSubMarks;
                                                    double outOff100 = testSubMarks;
                                                    if (testSubMarks >= 0 && !string.IsNullOrEmpty(subjectMark))
                                                    {
                                                        if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                            convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;
                                                        convertTestMark = Math.Round(convertTestMark, 1, MidpointRounding.AwayFromZero);
                                                        if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                            outOff100 = (testSubMarks / testSubMaxMarks) * 100;
                                                        if (subjectTSingle.Trim() == "0" || subjectTSingle.Trim().ToLower() == "false")
                                                        {
                                                            if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                                dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                            else
                                                                dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);
                                                            if (dicCommonSubjectWiseTotalOld.ContainsKey(subjectTCode.Trim().ToLower()))
                                                                dicCommonSubjectWiseTotalOld[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                            else
                                                                dicCommonSubjectWiseTotalOld.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                        }
                                                        if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                            dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                        else
                                                            dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);

                                                        if (dicSubjectTypeTestWiseAverageOld.ContainsKey(subjectTypeNo.Trim() + "@" + studTestNo.Trim()))
                                                            dicSubjectTypeTestWiseAverageOld[subjectTypeNo.Trim() + "@" + studTestNo.Trim()] += convertTestMark;
                                                        else
                                                            dicSubjectTypeTestWiseAverageOld.Add(subjectTypeNo.Trim() + "@" + studTestNo.Trim(), convertTestMark);

                                                        if (dicSubjectTypeWiseAverageOld.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                            dicSubjectTypeWiseAverageOld[subjectTypeNo.Trim().ToLower()] += convertTestMark;
                                                        else
                                                            dicSubjectTypeWiseAverageOld.Add(subjectTypeNo.Trim().ToLower(), convertTestMark);
                                                    }
                                                }
                                                if (testColumn == dtDistinctTestName.Rows.Count)
                                                {
                                                    double subjectWiseTotal = 0;
                                                    if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                                    if (!dicSubjectWiseOverAllTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicSubjectWiseOverAllTotal.Add(subjectTCode.Trim().ToLower(), subjectWiseTotal);
                                                    else
                                                        dicSubjectWiseOverAllTotal[subjectTCode.Trim().ToLower()] = subjectWiseTotal;
                                                }
                                            }
                                            testColumn++;
                                        }
                                    }
                                    dicTermWiseTotal.Add(term.ToLower().Trim(), dicSubjectWiseOverAllTotal);
                                }
                            }

                            #endregion

                            #region Student Marks Content

                            posY += 20;
                            pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "TERM - " + ToRoman(semester));
                            pdfReportPage.Add(pdfTA);

                            posY += 30;

                            double markReportHeight = 0;
                            int totalRows = dtCurrentSemesterSubject.Rows.Count + 2 + dtCurrentSingleSubjectType.Rows.Count;
                            int totalColumns = dtDistinctTestName.Rows.Count + 2 + ((semester.Trim() == "1") ? 0 : 1);
                            tblStudentMarks = pdfDocumentReport.NewTable(fontReportContent, totalRows, totalColumns, 5);
                            tblStudentMarks.VisibleHeaders = false;
                            tblStudentMarks.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            tblStudentMarks.Cell(0, 0).SetContent("Subject");
                            tblStudentMarks.Cell(0, 0).SetFont(fontReportContentBold);
                            tblStudentMarks.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblStudentMarks.Columns[0].SetWidth(160);

                            int subjectRow = 0;
                            dicSubjectWiseTotal.Clear();
                            dicTestWiseTotal.Clear();
                            double overAllSubjectWeightage = 0;
                            string previousSubjectType = string.Empty;
                            string previousSubjectTypeNo = string.Empty;
                            Dictionary<string, double> dicSubjectTypeWiseAverage = new Dictionary<string, double>();
                            Dictionary<string, double> dicSubjectTypeTestWiseAverage = new Dictionary<string, double>();
                            Dictionary<string, double> dicSingleSubjectTestWiseTotal = new Dictionary<string, double>();

                            Dictionary<string, double> dicSingleSubjectWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicCommonSubjectWiseTotal = new Dictionary<string, double>();
                            foreach (DataRow drSubjects in dtCurrentSemesterSubject.Rows)
                            {
                                string subjectTCode = Convert.ToString(drSubjects["subject_code"]).Trim();
                                string subjectTName = Convert.ToString(drSubjects["subject_name"]).Trim();
                                string markOrGrade = Convert.ToString(drSubjects["subjectMarkType"]).Trim();
                                string subjectTypeName = Convert.ToString(drSubjects["subject_type"]).Trim();
                                string subjectTypeNo = Convert.ToString(drSubjects["subType_no"]).Trim();
                                string subjectTSingle = Convert.ToString(drSubjects["isSingleSubject"]).Trim();
                                int testColumn = 1;
                                subjectRow++;
                                double maxTestMark = 0;
                                double maxSubjecTestMark = 0;

                                if ((!string.IsNullOrEmpty(previousSubjectTypeNo) && previousSubjectTypeNo != subjectTypeNo) || ((subjectTSingle.Trim() == "1" || subjectTSingle.Trim().ToLower() == "true") && subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count))
                                {
                                    tblStudentMarks.Cell(subjectRow, 0).SetContent(previousSubjectType);
                                    tblStudentMarks.Cell(subjectRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    double subjectTypeWiseTotal = 0;
                                    foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                    {
                                        string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                        string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                        string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                        string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                        double minimumTestMark = 0;
                                        double.TryParse(convertMin, out minimumTestMark);
                                        double maximumTestMark = 0;
                                        double.TryParse(convertMax, out maximumTestMark);
                                        maxTestMark += maximumTestMark;
                                        double subjectAvg = 0;
                                        if (dicSubjectTypeTestWiseAverage.ContainsKey(previousSubjectTypeNo.Trim() + "@" + studTestNo.Trim()))
                                            subjectAvg = dicSubjectTypeTestWiseAverage[previousSubjectTypeNo.Trim() + "@" + studTestNo.Trim()];

                                        dtCurrentSemesterSubject.DefaultView.RowFilter = "subType_no='" + previousSubjectTypeNo + "'";
                                        DataTable dtDistSingleSubject = dtCurrentSemesterSubject.DefaultView.ToTable(true, "subject_code", "subject_name", "acronym");
                                        subjectAvg = subjectAvg / dtDistSingleSubject.Rows.Count;
                                        subjectTypeWiseTotal += subjectAvg;
                                        string subjectAcr = string.Empty;
                                        List<string> lstSubAcr = dtDistSingleSubject.AsEnumerable().Select(r => r.Field<string>("acronym")).ToList();
                                        subjectAcr = string.Join(", ", lstSubAcr.ToArray());
                                        if (!string.IsNullOrEmpty(subjectAcr))
                                            subjectAcr = "(Avg:" + subjectAcr + ")";
                                        tblStudentMarks.Cell(subjectRow, 0).SetContent(previousSubjectType + "\n" + subjectAcr);
                                        tblStudentMarks.Cell(subjectRow, 0).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContent((subjectAvg > 0) ? subjectAvg.ToString() : "--");
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetFont(fontReportContentBold);

                                        if (dicSingleSubjectTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                            dicSingleSubjectTestWiseTotal[studTestNo.Trim()] = subjectAvg;
                                        else
                                            dicSingleSubjectTestWiseTotal.Add(studTestNo.Trim(), subjectAvg);
                                        if (testColumn == dtDistinctTestName.Rows.Count)
                                        {
                                            double sum = Math.Round((subjectTypeWiseTotal / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                            if (dicSingleSubjectWiseTotal.ContainsKey(previousSubjectTypeNo.Trim()))
                                                dicSingleSubjectWiseTotal[previousSubjectTypeNo.Trim()] = subjectAvg;
                                            else
                                                dicSingleSubjectWiseTotal.Add(previousSubjectTypeNo.Trim(), subjectAvg);
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((sum > 0) ? sum.ToString() : "--");
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetFont(fontReportContentBold);

                                            switch (semester.Trim().ToLower())
                                            {
                                                case "2":
                                                case "3":
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetContent((semester.Trim().ToLower() == "2") ? "Subject Average of T1 & T2" : "Weighted Average");
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblStudentMarks.Columns[testColumn + 2].SetWidth(85);

                                                    double cummulativeSubjectTotal = 0;
                                                    double currentTermSubjectTotal = 0;
                                                    if (dicSingleSubjectWiseTotal.ContainsKey(previousSubjectTypeNo.Trim().ToLower()))
                                                        currentTermSubjectTotal = Math.Round(dicSingleSubjectWiseTotal[previousSubjectTypeNo.Trim().ToLower()], 0, MidpointRounding.AwayFromZero);
                                                    foreach (KeyValuePair<string, Dictionary<string, double>> keyValue in dicTermWiseTotal)
                                                    {
                                                        string prevTerm = keyValue.Key;
                                                        Dictionary<string, double> dicPrevSubjectTotal = keyValue.Value;
                                                        double subjectTotal = 0;
                                                        if (dicPrevSubjectTotal.Count > 0)
                                                        {
                                                            double averageTotal = 0;
                                                            if (dicPrevSubjectTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                            {
                                                                subjectTotal = dicPrevSubjectTotal[subjectTCode.Trim().ToLower()];
                                                            }
                                                            if (semester.Trim().ToLower() == "2")
                                                            {
                                                                cummulativeSubjectTotal += subjectTotal;
                                                            }
                                                            else
                                                            {
                                                                averageTotal = Math.Round((subjectTotal / 100) * 25, 1, MidpointRounding.AwayFromZero);
                                                                cummulativeSubjectTotal += averageTotal;
                                                            }
                                                        }
                                                    }
                                                    double subjectWiseTotal = 0;
                                                    if (semester.Trim().ToLower() == "2")
                                                    {
                                                        cummulativeSubjectTotal += currentTermSubjectTotal;
                                                        subjectWiseTotal = Math.Round((cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1)), 1, MidpointRounding.AwayFromZero);
                                                    }
                                                    else
                                                    {
                                                        double averageTotal = 0;
                                                        averageTotal = Math.Round((currentTermSubjectTotal / 100) * 50, 1, MidpointRounding.AwayFromZero);
                                                        cummulativeSubjectTotal += averageTotal;
                                                        cummulativeSubjectTotal = Math.Round((cummulativeSubjectTotal), 1, MidpointRounding.AwayFromZero);
                                                        subjectWiseTotal = cummulativeSubjectTotal;
                                                    }
                                                    string displayTotal = subjectWiseTotal.ToString();

                                                    overAllSubjectWeightage += subjectWiseTotal;
                                                    tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContent((subjectWiseTotal > 0) ? displayTotal : "--");
                                                    tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                                    {
                                                        tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContent((overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                        tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    }
                                                    break;
                                            }
                                        }
                                        testColumn++;
                                    }
                                    subjectRow++;
                                }
                                if (subjectTSingle.Trim() == "1" || subjectTSingle.Trim().ToLower() == "true")
                                {
                                    previousSubjectType = subjectTypeName;
                                    previousSubjectTypeNo = subjectTypeNo;
                                }
                                else
                                {
                                    previousSubjectType = string.Empty;
                                    previousSubjectTypeNo = string.Empty;
                                }
                                tblStudentMarks.Cell(subjectRow, 0).SetContent(subjectTName);
                                tblStudentMarks.Cell(subjectRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                maxTestMark = 0;
                                maxSubjecTestMark = 0;
                                testColumn = 1;
                                foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                {
                                    string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                    string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                    string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                    string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                    double minimumTestMark = 0;
                                    double.TryParse(convertMin, out minimumTestMark);
                                    double maximumTestMark = 0;
                                    double.TryParse(convertMax, out maximumTestMark);
                                    maxTestMark += maximumTestMark;
                                    string subjectMaxMark = string.Empty;
                                    string subjectminMark = string.Empty;
                                    if (subjectRow == 1)
                                    {
                                        tblStudentMarks.Columns[testColumn].SetWidth(75);
                                    }
                                    DataView dvStudMarks = new DataView();
                                    dtCurrentStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and Criteria_no='" + studTestNo + "'";
                                    dvStudMarks = dtCurrentStudentMarks.DefaultView;
                                    bool setTestName = false;
                                    if (dvStudMarks.Count > 0)
                                    {
                                        subjectMaxMark = Convert.ToString(dvStudMarks[0]["Conducted_max"]).Trim();
                                        subjectminMark = Convert.ToString(dvStudMarks[0]["Conduct_Minmark"]).Trim();
                                        convertMin = Convert.ToString(dvStudMarks[0]["Convert_Minmark"]).Trim();
                                        convertMax = Convert.ToString(dvStudMarks[0]["Convert_Maxmark"]).Trim();
                                        markOrGrade = Convert.ToString(dvStudMarks[0]["subjectMarkType"]).Trim();
                                        string subjectMark = Convert.ToString(dvStudMarks[0]["marks_obtained"]).Trim();
                                        string sems = Convert.ToString(dvStudMarks[0]["semester"]).Trim();

                                        double testSubMarks = 0;
                                        double.TryParse(subjectMark, out testSubMarks);

                                        double testMinMark = 0;
                                        double.TryParse(convertMin, out testMinMark);

                                        double testMaxMark = 0;
                                        double.TryParse(convertMax, out testMaxMark);
                                        maxSubjecTestMark += testMaxMark;

                                        double testSubMinMarks = 0;
                                        double.TryParse(subjectminMark, out testSubMinMarks);

                                        double testSubMaxMarks = 0;
                                        double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                        double convertTestMark = testSubMarks;
                                        double outOff100 = testSubMarks;
                                        string displayMark = string.Empty;
                                        string displayMark1 = string.Empty;
                                        string displayGrade = string.Empty;
                                        bool isFail = false;
                                        if (testSubMarks < 0)
                                        {
                                            displayMark1 = displayMark = getMarkText(subjectMark);
                                            isFail = true;
                                        }
                                        else if (string.IsNullOrEmpty(subjectMark))
                                        {
                                            displayMark1 = displayMark = "--";
                                            isFail = true;
                                        }
                                        else
                                        {
                                            isFail = false;
                                            displayMark = testSubMarks.ToString();
                                            if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;
                                            convertTestMark = Math.Round(convertTestMark, 1, MidpointRounding.AwayFromZero);
                                            displayMark1 = convertTestMark.ToString();
                                            if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                outOff100 = (testSubMarks / testSubMaxMarks) * 100;
                                            if (subjectTSingle.Trim() == "0" || subjectTSingle.Trim().ToLower() == "false")
                                            {
                                                if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                    dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                else
                                                    dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);

                                                if (dicCommonSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                    dicCommonSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                else
                                                    dicCommonSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                            }

                                            if (dicSubjectTypeTestWiseAverage.ContainsKey(subjectTypeNo.Trim() + "@" + studTestNo.Trim()))
                                                dicSubjectTypeTestWiseAverage[subjectTypeNo.Trim() + "@" + studTestNo.Trim()] += convertTestMark;
                                            else
                                                dicSubjectTypeTestWiseAverage.Add(subjectTypeNo.Trim() + "@" + studTestNo.Trim(), convertTestMark);

                                            if (dicSubjectTypeWiseAverage.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                dicSubjectTypeWiseAverage[subjectTypeNo.Trim().ToLower()] += convertTestMark;
                                            else
                                                dicSubjectTypeWiseAverage.Add(subjectTypeNo.Trim().ToLower(), convertTestMark);

                                            if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                            else
                                                dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                        }
                                        DataView dvGrade = new DataView();
                                        if (dtGradeDetails.Rows.Count > 0)
                                        {
                                            dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + sems + "' and Criteria='" + studTestName.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                            dvGrade = dtGradeDetails.DefaultView;
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='" + studTestName.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + sems + "' and Frange<='" + outOff100 + "' and Trange>='" + outOff100 + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + outOff100 + "' and Trange>='" + outOff100 + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                        }
                                        if (dvGrade.Count > 0)
                                        {
                                            displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                        }
                                        else
                                        {
                                            displayGrade = "--";
                                        }
                                        if (markOrGrade == "2")
                                        {
                                            displayMark1 = (isFail) ? displayMark1 : displayGrade;
                                        }
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContent(displayMark1);
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        if (!setTestName)
                                        {
                                            tblStudentMarks.Cell(0, testColumn).SetContent(studTestName + "\n(" + testMaxMark + ")");
                                            tblStudentMarks.Cell(0, testColumn).SetFont(fontReportContentBold);
                                            tblStudentMarks.Cell(0, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        }
                                    }
                                    else
                                    {
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContent("--");
                                        tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    }
                                    if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                    {
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetContent("Total");
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetFont(fontReportContentBold);
                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetContent("Average");
                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                        {
                                            double singleSubjectSum = (dicSingleSubjectTestWiseTotal.ContainsKey(studTestNo.Trim())) ? dicSingleSubjectTestWiseTotal[studTestNo.Trim()] : 0;
                                            tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum));
                                            double average = Math.Round((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum) / (dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count - dtCurrentSingleSubject.Rows.Count), 2, MidpointRounding.AwayFromZero);
                                            //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent(average);
                                        }
                                        else
                                        {
                                            tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent("--");
                                            //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent("--");
                                        }
                                        tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        if (testColumn == dtDistinctTestName.Rows.Count)
                                        {
                                            double sum = dicCommonSubjectWiseTotal.Sum(x => Math.Round((x.Value / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero)) + dicSingleSubjectWiseTotal.Sum(x => Math.Round(x.Value, 0, MidpointRounding.AwayFromZero));
                                            tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContent((sum > 0) ? sum.ToString() : "--");
                                            tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        }
                                    }
                                    if (testColumn == dtDistinctTestName.Rows.Count)
                                    {
                                        tblStudentMarks.Cell(0, testColumn + 1).SetContent("Total\n(100)");
                                        tblStudentMarks.Cell(0, testColumn + 1).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(0, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblStudentMarks.Columns[testColumn + 1].SetWidth(50);
                                        if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                        {
                                            double subjectWiseTotal = 0;
                                            subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                            string displayTotal = subjectWiseTotal.ToString();
                                            DataView dvGrade = new DataView();
                                            if (dtGradeDetails.Rows.Count > 0)
                                            {
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                            }
                                            if (markOrGrade == "2")
                                            {
                                                if (dvGrade.Count > 0)
                                                {
                                                    displayTotal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                                }
                                                else
                                                {
                                                    displayTotal = subjectWiseTotal.ToString();
                                                }
                                            }
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent(displayTotal);
                                        }
                                        else
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent("--");
                                        tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        switch (semester.Trim().ToLower())
                                        {
                                            case "2":
                                            case "3":
                                                tblStudentMarks.Cell(0, testColumn + 2).SetContent((semester.Trim().ToLower() == "2") ? "Subject Average of T1 & T2" : "Weighted Average");
                                                tblStudentMarks.Cell(0, testColumn + 2).SetFont(fontReportContentBold);
                                                tblStudentMarks.Cell(0, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblStudentMarks.Columns[testColumn + 2].SetWidth(85);

                                                double cummulativeSubjectTotal = 0;
                                                double currentTermSubjectTotal = 0;
                                                if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                    currentTermSubjectTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                                foreach (KeyValuePair<string, Dictionary<string, double>> keyValue in dicTermWiseTotal)
                                                {
                                                    string prevTerm = keyValue.Key;
                                                    Dictionary<string, double> dicPrevSubjectTotal = keyValue.Value;
                                                    double subjectTotal = 0;
                                                    if (dicPrevSubjectTotal.Count > 0)
                                                    {
                                                        double averageTotal = 0;
                                                        if (dicPrevSubjectTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        {
                                                            subjectTotal = dicPrevSubjectTotal[subjectTCode.Trim().ToLower()];
                                                        }
                                                        if (semester.Trim().ToLower() == "2")
                                                        {
                                                            cummulativeSubjectTotal += subjectTotal;
                                                        }
                                                        else
                                                        {
                                                            averageTotal = Math.Round((subjectTotal / 100) * 25, 1, MidpointRounding.AwayFromZero);
                                                            cummulativeSubjectTotal += averageTotal;
                                                        }
                                                    }
                                                }
                                                double subjectWiseTotal = 0;
                                                if (semester.Trim().ToLower() == "2")
                                                {
                                                    cummulativeSubjectTotal += currentTermSubjectTotal;
                                                    subjectWiseTotal = Math.Round((cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1)), 1, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    double averageTotal = 0;
                                                    averageTotal = Math.Round((currentTermSubjectTotal / 100) * 50, 1, MidpointRounding.AwayFromZero);
                                                    cummulativeSubjectTotal += averageTotal;
                                                    cummulativeSubjectTotal = Math.Round((cummulativeSubjectTotal), 1, MidpointRounding.AwayFromZero);
                                                    subjectWiseTotal = cummulativeSubjectTotal;
                                                }
                                                string displayTotal = subjectWiseTotal.ToString();
                                                DataView dvGrade = new DataView();
                                                if (dtGradeDetails.Rows.Count > 0)
                                                {
                                                    if (dvGrade.Count == 0)
                                                    {
                                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                        dvGrade = dtGradeDetails.DefaultView;
                                                    }
                                                    if (dvGrade.Count == 0)
                                                    {
                                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                        dvGrade = dtGradeDetails.DefaultView;
                                                    }
                                                }
                                                if (markOrGrade == "2")
                                                {
                                                    if (dvGrade.Count > 0)
                                                    {
                                                        displayTotal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                                    }
                                                    else
                                                    {
                                                        displayTotal = subjectWiseTotal.ToString();
                                                    }
                                                }
                                                overAllSubjectWeightage += subjectWiseTotal;
                                                tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContent((subjectWiseTotal > 0) ? displayTotal : "--");
                                                tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                if (subjectRow == dtCurrentSemesterSubject.Rows.Count)
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContent((overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                break;
                                        }
                                    }
                                    testColumn++;
                                }
                            }

                            tblPage = tblStudentMarks.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, (pdfDocumentReport.PageWidth / 2), 400));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;

                            #endregion

                            #region Activity Details

                            double activityHeight = 0;
                            if (hasActivity)
                            {
                                if (dtStudActivityMarks.Rows.Count > 0)
                                {
                                    DataTable dtSubPart = new DataTable();
                                    int partTotalRows = dtStudActivityMarks.Rows.Count + dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle").Rows.Count + 1;
                                    tblStudentActivity = pdfDocumentReport.NewTable(fontReportContent, partTotalRows, 2, 5);
                                    tblStudentActivity.VisibleHeaders = false;
                                    tblStudentActivity.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tblStudentActivity.SetColumnsWidth(new int[] { 250, 100 });

                                    tblStudentActivity.Cell(0, 0).SetContent("Skills");
                                    tblStudentActivity.Cell(0, 0).SetFont(fontReportContentBold);
                                    tblStudentActivity.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    tblStudentActivity.Cell(0, 1).SetContent("Grade");
                                    tblStudentActivity.Cell(0, 1).SetFont(fontReportContentBold);
                                    tblStudentActivity.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    int activityStartRow = 0;
                                    for (int part = 0; part < totalParts; part++)
                                    {
                                        dtSubPart = new DataTable();
                                        dtStudActivityMarks.DefaultView.RowFilter = "Part_No='" + (part + 2) + "'";
                                        dtSubPart = dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle");
                                        foreach (DataRow drActivity in dtSubPart.Rows)
                                        {
                                            activityStartRow++;
                                            string subTitleNo = Convert.ToString(drActivity["Title_Name"]).Trim();
                                            string subTitleName = string.Empty;
                                            GetSubTitleName(subTitleNo, ref subTitleName);
                                            DataTable dtActivity = new DataTable();
                                            dtStudActivityMarks.DefaultView.RowFilter = "Title_Name='" + subTitleNo + "' ";
                                            dtActivity = dtStudActivityMarks.DefaultView.ToTable();
                                            tblStudentActivity.Cell(activityStartRow, 0).SetContent(subTitleName);
                                            tblStudentActivity.Cell(activityStartRow, 0).SetFont(fontReportContentBold);
                                            tblStudentActivity.Cell(activityStartRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in tblStudentActivity.CellRange(activityStartRow, 0, activityStartRow, 0).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }

                                            foreach (DataRow drActivityMarks in dtActivity.Rows)
                                            {
                                                activityStartRow++;

                                                string activityName = Convert.ToString(drActivityMarks["TextVal"]).Trim();
                                                string grade = Convert.ToString(drActivityMarks["Grade"]).Trim();

                                                tblStudentActivity.Cell(activityStartRow, 0).SetContent(activityName);
                                                tblStudentActivity.Cell(activityStartRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tblStudentActivity.Cell(activityStartRow, 1).SetContent(grade);
                                                tblStudentActivity.Cell(activityStartRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                    }

                                    tblPage = tblStudentActivity.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, (pdfDocumentReport.PageWidth / 2) + 30, posY, (pdfDocumentReport.PageWidth / 2) - 50, 400));
                                    pdfReportPage.Add(tblPage);
                                    activityHeight = tblPage.Area.Height;
                                }
                            }
                            if (activityHeight <= height)
                                posY += int.Parse(Convert.ToString(height));
                            else
                                posY += int.Parse(Convert.ToString(activityHeight));

                            #endregion

                            #region Remark Details

                            posY += 15;
                            pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - (2 * 25), 20), ContentAlignment.MiddleLeft, "Remarks\t:\t----------------------------------------------------------------------------------------------------------------------------------------------------");
                            pdfReportPage.Add(pdfTA);

                            posY += 26;
                            pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - (2 * 25), 25), ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            pdfReportPage.Add(pdfTA);
                            posY += 10;

                            #endregion

                            #region Performance Analysis

                            if (dtPreviousStudentMarks.Rows.Count > 0)
                            {
                                DataTable dtTerm = new DataTable();
                                dtPreviousStudentMarks.DefaultView.RowFilter = "";
                                dtTerm = dtPreviousStudentMarks.DefaultView.ToTable(true, "semester");

                                string conductedTestNames = string.Empty;
                                List<string> lstTestName = dtDistinctCommonTestName.AsEnumerable().Select(r => r.Field<string>("criteria")).ToList();
                                conductedTestNames = string.Join("','", lstTestName.ToArray());
                                int totalPerfColumns = dtPreviousSemesterSubject.Rows.Count + 2;
                                int totalPerfRows = dtTerm.Rows.Count + 1;
                                if (dtTerm.Rows.Count > 0)
                                {
                                    posY += 10;
                                    pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "PERFORMANCE ANALYSIS");
                                    pdfReportPage.Add(pdfTA);
                                    posY += 20;
                                    tblStudentPerformanceAnalysis = pdfDocumentReport.NewTable(fontReportFooter, totalPerfRows, totalPerfColumns, 5);
                                    tblStudentPerformanceAnalysis.VisibleHeaders = false;
                                    tblStudentPerformanceAnalysis.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetContent("Test");
                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetFont(fontReportFooter);
                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetContent("Max");
                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetFont(fontReportFooter);
                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    int termRow = 0;
                                    foreach (DataRow drTerm in dtTerm.Rows)
                                    {
                                        string term = Convert.ToString(drTerm["semester"]).Trim();
                                        double maxTestMark = 0;
                                        dicSubjectWiseTotal.Clear();
                                        dicTestWiseTotal.Clear();
                                        termRow++;
                                        int subjectCol = 1;
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetContent("Term - " + ToRoman(term));
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetFont(fontReportFooter);
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        foreach (DataRow drSubject in dtPreviousSemesterSubject.Rows)
                                        {
                                            string subjectTCode = Convert.ToString(drSubject["subject_code"]).Trim();
                                            string subjectTName = Convert.ToString(drSubject["subject_name"]).Trim();
                                            string subjectTAcronymn = Convert.ToString(drSubject["acronym"]).Trim();
                                            string subjectTypeName = Convert.ToString(drSubject["subject_type"]).Trim();
                                            string subjectTypeNo = Convert.ToString(drSubject["subType_no"]).Trim();
                                            string subjectTSingle = Convert.ToString(drSubject["isSingleSubject"]).Trim();
                                            double maxSubjecTestMark = 0;
                                            int testColumn = 1;
                                            subjectCol++;
                                            tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetContent(subjectTAcronymn);
                                            tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetFont(fontReportFooter);
                                            tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                            {
                                                string subjectMaxMark = string.Empty;
                                                string subjectminMark = string.Empty;
                                                string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                                string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                                string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                                string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                                double minimumTestMark = 0;
                                                double.TryParse(convertMin, out minimumTestMark);
                                                double maximumTestMark = 0;
                                                double.TryParse(convertMax, out maximumTestMark);
                                                maxTestMark += maximumTestMark;
                                                DataView dvStudMarks = new DataView();
                                                dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and semester='" + term + "' and criteria='" + studTestName + "'";
                                                dvStudMarks = dtPreviousStudentMarks.DefaultView;
                                                tblStudentPerformanceAnalysis.Cell(termRow, 1).SetContent("");
                                                tblStudentPerformanceAnalysis.Cell(termRow, 1).SetFont(fontReportFooter);
                                                tblStudentPerformanceAnalysis.Cell(termRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                if (dvStudMarks.Count > 0)
                                                {
                                                    subjectMaxMark = Convert.ToString(dvStudMarks[0]["Conducted_max"]).Trim();
                                                    subjectminMark = Convert.ToString(dvStudMarks[0]["Conduct_Minmark"]).Trim();
                                                    convertMin = Convert.ToString(dvStudMarks[0]["Convert_Minmark"]).Trim();
                                                    convertMax = Convert.ToString(dvStudMarks[0]["Convert_Maxmark"]).Trim();
                                                    string subjectMark = Convert.ToString(dvStudMarks[0]["marks_obtained"]).Trim();

                                                    double testSubMarks = 0;
                                                    double.TryParse(subjectMark, out testSubMarks);

                                                    double testMinMark = 0;
                                                    double.TryParse(convertMin, out testMinMark);

                                                    double testMaxMark = 0;
                                                    double.TryParse(convertMax, out testMaxMark);
                                                    maxSubjecTestMark += testMaxMark;

                                                    double testSubMinMarks = 0;
                                                    double.TryParse(subjectminMark, out testSubMinMarks);

                                                    double testSubMaxMarks = 0;
                                                    double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                                    double convertTestMark = testSubMarks;
                                                    double outOff100 = testSubMarks;
                                                    if (testSubMarks >= 0 && !string.IsNullOrEmpty(subjectMark))
                                                    {
                                                        if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                            convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;
                                                        convertTestMark = Math.Round(convertTestMark, 1, MidpointRounding.AwayFromZero);
                                                        if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                            outOff100 = (testSubMarks / testSubMaxMarks) * 100;
                                                        if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                            dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                        else
                                                            dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);
                                                        if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                            dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                        else
                                                            dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                    }
                                                }
                                                if (testColumn == dtDistinctTestName.Rows.Count)
                                                {
                                                    if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                    {
                                                        double subjectWiseTotal = 0;
                                                        subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] / maxTestMark) * 100, 0, MidpointRounding.AwayFromZero);
                                                        tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContent(subjectWiseTotal);
                                                    }
                                                    else
                                                        tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContent("--");
                                                    tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                testColumn++;
                                            }
                                        }
                                    }

                                    tblPage = tblStudentPerformanceAnalysis.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, (pdfDocumentReport.PageWidth) - 50, 400));
                                    pdfReportPage.Add(tblPage);
                                    height = tblPage.Area.Height;
                                    posY += int.Parse(Convert.ToString(height));
                                }
                            }

                            #endregion

                            #region Signature

                            tblSign = pdfDocumentReport.NewTable(fontReportFooter, 1, 3, 5);
                            tblSign.VisibleHeaders = false;
                            tblSign.SetBorders(Color.Black, 1, BorderType.None);
                            tblSign.SetColumnsWidth(new int[] { 200, 200, 200 });
                            tblSign.Cell(0, 0).SetContent("Signature of Teacher");
                            tblSign.Cell(0, 0).SetFont(fontReportFooter);
                            tblSign.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblSign.Cell(0, 1).SetContent("Signature of Principal");
                            tblSign.Cell(0, 1).SetFont(fontReportFooter);
                            tblSign.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblSign.Cell(0, 2).SetContent("Signature of Principal");
                            tblSign.Cell(0, 2).SetFont(fontReportFooter);
                            tblSign.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblPage = tblSign.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, pdfDocumentReport.PageHeight - 80, (pdfDocumentReport.PageWidth) - 50, 50));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;

                            pdfLine = new PdfLine(pdfDocumentReport, new PointF(35, (float)pdfDocumentReport.PageHeight - 50), new PointF((float)pdfDocumentReport.PageWidth - 35, (float)pdfDocumentReport.PageHeight - 50), Color.Black, 1);
                            pdfReportPage.Add(pdfLine);

                            pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 0, pdfDocumentReport.PageHeight - 50, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "The report card signed by the Parents or Guardian should be returned to the Class Teacher, within three days of issue of report card.");
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            if (isReportSave)
                                pdfReportPage.SaveToDocument();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select Atleast Any one Student";
                divPopAlert.Visible = true;
                return;
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "PerformanceReportCardICSE_IX_And_X" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    pdfDocumentReport.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblAlertMsg.Text = "There is No Report Card Generated";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }*/

    private void AcademicPerformanceReport(List<string> lstAppNo, string rollNo)
    {
        try
        {
            #region Font Creation

            Font fontColName = new Font("Times New Roman", 18, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Times New Roman", 13, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Times New Roman", 18, FontStyle.Bold);
            Font fontstudClass = new Font("Times New Roman", 15, FontStyle.Bold);
            Font fontReportContent = new Font("Times New Roman", 11, FontStyle.Regular);
            Font fontReportContentBold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontReportStudProfileHeader = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontReportFooter = new Font("Times New Roman", 10, FontStyle.Regular);

            #endregion Font Creation

            string appNoList = string.Empty;
            StringBuilder sbErr = new StringBuilder();

            DataTable dtStudentDetails = new DataTable();
            DataTable dtStudentPhotos = new DataTable();
            DataTable dtCollegeDetails = new DataTable();
            DataTable dtStudentAllMarksDetails = new DataTable();
            DataTable dtStudentMarksDetails = new DataTable();
            DataTable dtStudentActivities = new DataTable();
            DataTable dtSem = new DataTable();
            DataTable dtGradeDetails = new DataTable();
            DataTable dtGeneralGrade = new DataTable();

            DataSet dsStudMarks = new DataSet();
            DataSet dsPart = new DataSet();
            DataSet dsAttRemark = new DataSet();

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

            Gios.Pdf.PdfDocument pdfDocumentReport = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage pdfReportPage;
            bool status = false;
            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                orderBy = orderByStudents(collegeCode, "r");
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
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(e.Sections,''))) in('" + secValue + "')";
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
            else if (cblTest.Items.Count > 0 && txtTest.Visible && txtTest.Enabled)
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
            if (lstAppNo.Count > 0)
            {
                appNoList = string.Join("','", lstAppNo.ToArray());

                if (!string.IsNullOrEmpty(appNoList))
                {
                    qry = "select distinct r.serialno,r.App_No,r.college_code,r.Roll_No,r.Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code  and r.app_no in ('" + appNoList + "') " + orderBy;
                    dtStudentDetails = dirAcc.selectDataTable(qry);

                    qry = "select CONVERT(VARCHAR(30),start_date,103) as start_date ,CONVERT(VARCHAR(30),end_date,103) as end_date from seminfo where semester='" + (semester) + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "'";
                    dtSem = dirAcc.selectDataTable(qry);

                    #region PreviousSemester added by prabha

                    string previousTestName = BindPreviousTestNameNEW();

                    #endregion
                    //previousTestName instead of testname 
                    //modified by prabha on jan 30 2018
                    qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,c.criteria,c.Criteria_no,ss.subject_type,ss.subType_no,ISNULL(s.subjectMarkType,'1') as subjectMarkType,s.subject_name,s.acronym,s.subject_code,LTRIM(RTRIM(ISNULL(e.sections,''))) sections,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,ISNULL(s.subjectpriority,'0') as subjectpriority,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,ISNULL( ss.isSingleSubject,'0') as isSingleSubject,s.isCompulsaoryForGrandTotal , s.isConsiderForGrandTotal from Exam_type e,subject s,CriteriaForInternal c,syllabus_master sm,Result re,sub_sem ss where sm.syll_code=s.syll_code and c.syll_code=sm.syll_code and c.syll_code=s.syll_code and sm.Batch_Year=e.batch_year and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and re.exam_code=e.exam_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "'  and c.criteria in(" + previousTestName + ") and sm.semester<='" + semester + "' " + qrySection1 + "  order by sm.Batch_Year,sm.degree_code,sm.semester,c.criteria,c.Criteria_no,subjectpriority,ss.subType_no,s.subject_code;  "

                   + "select rg.App_No,r.roll_no,rg.Reg_No,rg.Stud_Name,rg.Roll_Admit,sm.Batch_Year,rg.degree_code,rg.Current_Semester,sm.semester,rg.sections,c.Criteria_no,c.criteria,e.exam_code,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,s.subject_code,s.subject_no,ss.subject_type,ss.subType_no,s.syll_code,ISNULL(s.subjectMarkType,'1') as subjectMarkType,s.subject_name,s.acronym,ISNULL(s.subjectpriority,'0') as subjectpriority,r.marks_obtained,isnull(r.remarks,'') as remarks,convert(varchar(10),e.exam_date,103)as exam_date,LTRIM(RTRIM(ISNULL(e.sections,''))) sections,ISNULL( ss.isSingleSubject,'0') as isSingleSubject,s.isCompulsaoryForGrandTotal , s.isConsiderForGrandTotal from Registration rg,CriteriaForInternal c,Exam_type e,Result r,syllabus_master sm,subject s,sub_sem ss where rg.Roll_No =r.roll_no and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and e.exam_code=r.exam_code and e.batch_year=rg.Batch_Year and e.sections=rg.Sections and sm.Batch_Year=rg.Batch_Year and rg.degree_code=sm.degree_code and sm.syll_code=s.syll_code and sm.syll_code=c.syll_code and e.batch_year=sm.Batch_Year and cc=0 and delflag=0 and exam_flag<>'Debar' and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and rg.Batch_Year='" + batchYear + "' and rg.degree_code='" + degreeCode + "' and rg.college_code='" + collegeCode + "' and c.criteria in(" + previousTestName + ") and sm.semester<='" + semester + "' " + qrySection1 + " and rg.App_no in('" + appNoList + "') order by rg.Roll_No,c.Criteria_no,subjectpriority,ss.subType_no,s.subject_code;";//,s.isCompulsaoryForGrandTotal , s.isConsiderForGrandTotal
                    dsStudMarks = dirAcc.selectDataSet(qry);

                    qry = "SELECT * from CoCurrActivitie_Det where  istype='Att'and Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and term in(" + semester + ") and Roll_No in(" + rollNo + ");SELECT * from CoCurrActivitie_Det where istype='remks' and Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and term in(" + semester + ") and Roll_No in(" + rollNo + "); ";
                    dsAttRemark.Clear();
                    dsAttRemark.Reset();
                    dsAttRemark = dirAcc.selectDataSet(qry);

                    qry = "select distinct PartName,Part_No from CoCurr_Activitie where Batch_Year='" + batchYear + "' and Degree_Code='" + degreeCode + "' and ISNULL(Part_No,'0')<>'0' and Part_No<>'1'; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term from activity_entry ae,CoCurr_Activitie ca,TextValTable tv where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "' and ae.term in(" + semester + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "'  and ae.term in(" + semester + ") and det.Roll_No in(" + rollNo + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark,ag.Grade,ag.description,ag.frompoint,ag.topoint from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det,activity_gd ag where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and ag.batch_year=det.Batch_Year and ag.batch_year=ae.Batch_Year and ag.batch_year=ca.Batch_Year and ae.Degree_Code=ag.Degree_Code and ag.Degree_Code=ca.Degree_Code and det.Degree_Code=ag.Degree_Code and ag.term=det.term and ag.term=ae.term and tv.TextCode=ag.ActivityTextVal and ae.ActivityTextVal=ag.ActivityTextVal and det.ActivityTextVal=ag.ActivityTextVal and det.Mark between ag.frompoint and ag.topoint and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegeCode + "' and ae.Batch_Year='" + batchYear + "' and ae.Degree_Code='" + degreeCode + "' and ae.term in(" + semester + ") and det.Roll_No in(" + rollNo + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ";
                    dsPart.Clear();
                    dsPart.Reset();
                    dsPart = dirAcc.selectDataSet(qry);

                    qry = "select * from StdPhoto where app_no in('" + appNoList + "')";
                    dtStudentPhotos = dirAcc.selectDataTable(qry);

                    qry = "SELECT college_code,collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,website from collinfo where college_code='" + collegeCode + "';";
                    dtCollegeDetails = dirAcc.selectDataTable(qry);

                    qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc";
                    dtGradeDetails = dirAcc.selectDataTable(qry);
                    if (dtGradeDetails.Rows.Count > 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                }

                BestOfSubjects = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='IX-X BestOfSubjects' and college_code= '" + ddlCollege.SelectedValue + "' ");
                //BestOfSubjects = Convert.ToString(BestOfSubjects.Split('@').Last());
                int bestOfSubjectCount = 0;
                int.TryParse(BestOfSubjects, out bestOfSubjectCount);

                if (dtStudentDetails.Rows.Count > 0)
                {
                    foreach (DataRow drStudent in dtStudentDetails.Rows)
                    {
                        int posX = 15;
                        int posY = 25;
                        bool isReportSave = false;
                        string appNo = Convert.ToString(drStudent["App_No"]).Trim();
                        string studentRollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                        string studentName = Convert.ToString(drStudent["stud_name"]).Trim();
                        string studentRollAdmit = Convert.ToString(drStudent["roll_admit"]).Trim();
                        string admitDate = Convert.ToString(drStudent["adm_date"]).Trim();
                        string studentBatch = Convert.ToString(drStudent["Batch_Year"]).Trim();
                        string studentDegreeCode = Convert.ToString(drStudent["degree_code"]).Trim();
                        string studentSemester = Convert.ToString(drStudent["Current_Semester"]).Trim();
                        string studentSections = Convert.ToString(drStudent["Sections"]).Trim();
                        string studentRegNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                        string studentDepartment = Convert.ToString(drStudent["Dept_Name"]).Trim();
                        string studentFatherName = Convert.ToString(drStudent["parent_name"]).Trim();
                        string studentMotherName = Convert.ToString(drStudent["mother"]).Trim();
                        string studentHeight = Convert.ToString(drStudent["StudHeight"]).Trim();
                        string studentWeight = Convert.ToString(drStudent["StudWeight"]).Trim();
                        string studentBloodGroup = Convert.ToString(drStudent["Blood_Grp"]).Trim();
                        string studentHouse = Convert.ToString(drStudent["studhouse"]).Trim();
                        string studentDOB = Convert.ToString(drStudent["dob"]).Trim();
                        string studentCollegeCode = Convert.ToString(drStudent["college_code"]).Trim();
                        string studentStandard = dirAcc.selectScalarString("select dept_acronym from Department where Dept_Code in(select Dept_Code from Degree where Degree_Code='" + studentDegreeCode + "')");
                        string studClassandSec = ((!string.IsNullOrEmpty(studentDepartment)) ? studentDepartment.Replace("STD", "").Replace("std", "") + ((!string.IsNullOrEmpty(studentSections)) ? "  " + studentSections : "") : "");

                        string academicYear = string.Empty;
                        int currentYear = DateTime.Today.Year;
                        int batchStartYear = 0;
                        int.TryParse(batchYear, out batchStartYear);
                        academicYear = batchStartYear.ToString() + " - " + Convert.ToString((batchStartYear + 1));

                        #region Student Attendance And Remarks

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
                        string studentRemarks = string.Empty;

                        DataView dvAttRmk = new DataView();
                        if (!chkManualAttendance.Checked)
                        {
                            if (dtSem.Rows.Count > 0)
                            {
                                frdate = Convert.ToString(dtSem.Rows[0]["start_date"]).Trim();
                                todate = Convert.ToString(dtSem.Rows[0]["end_date"]).Trim();
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

                                persentmonthcal(studentCollegeCode, studentDegreeCode, semester, rollNo, admitDate);

                                double absenthours = per_workingdays1 - per_per_hrs;
                                double per_tage_date = 0;
                                if (per_workingdays > 0)
                                {
                                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                                }
                                if (per_tage_date > 100)
                                {
                                    per_tage_date = 100;
                                }

                                double per_tage_hrs = 0;
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
                        }
                        else
                        {
                            if (dsAttRemark.Tables.Count > 0 && dsAttRemark.Tables[0].Rows.Count > 0)
                            {
                                dsAttRemark.Tables[0].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "' and term='" + semester + "' and Mark<>0 and totatt_remarks<>'-'";
                                dvAttRmk = dsAttRemark.Tables[0].DefaultView;
                                string studentAttendanceTotWorking = string.Empty;
                                string studentAttendanceTotPresent = string.Empty;
                                if (dvAttRmk.Count > 0)
                                {
                                    studentAttendanceTotWorking = Convert.ToString(dvAttRmk[0]["totatt_remarks"]).Trim();
                                    studentAttendanceTotPresent = Convert.ToString(dvAttRmk[0]["Mark"]).Trim();
                                    double.TryParse(studentAttendanceTotWorking, out per_workingdays);
                                    double.TryParse(studentAttendanceTotPresent, out pre_present_date);

                                }
                                else
                                {
                                    studentAttendanceTotWorking = string.Empty;
                                    studentAttendanceTotPresent = string.Empty;
                                    per_workingdays = 0;
                                    pre_present_date = 0;
                                }
                            }
                            double absenthours = per_workingdays1 - per_per_hrs;
                            double per_tage_date = 0;
                            if (per_workingdays > 0)
                            {
                                per_tage_date = ((pre_present_date / per_workingdays) * 100);
                            }
                            if (per_tage_date > 100)
                            {
                                per_tage_date = 100;
                            }
                            dum_tage_date = string.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                            if (dum_tage_date == "NaN")
                            {
                                dum_tage_date = "0";
                            }
                            else if (dum_tage_date == "Infinity")
                            {
                                dum_tage_date = "0";
                            }
                        }
                        if (dsAttRemark.Tables.Count >= 2 && dsAttRemark.Tables[1].Rows.Count > 0)
                        {
                            dsAttRemark.Tables[1].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "' and term='" + semester + "'";
                            dvAttRmk = dsAttRemark.Tables[1].DefaultView;
                            if (dvAttRmk.Count > 0)
                            {
                                studentRemarks = Convert.ToString(dvAttRmk[0]["totatt_remarks"]).Trim();
                            }
                        }

                        #endregion  Student Attendance And Remarks

                        string clgname = string.Empty;
                        string clgaff = string.Empty;
                        string clgaddress1 = string.Empty;
                        string clgaddress2 = string.Empty;
                        string clgaddress3 = string.Empty;
                        string clgdistrict = string.Empty;
                        string clgpincode = string.Empty;
                        string clgemail = string.Empty;
                        string clgfulladdress = string.Empty;
                        int totalParts = 0;
                        int mainsubmark = 0;

                        DataTable dtCurrentSemesterSubject = new DataTable();
                        DataTable dtCurrentStudentMarks = new DataTable();
                        DataTable dtPreviousSemesterSubject = new DataTable();
                        DataTable dtPreviousStudentMarks = new DataTable();
                        DataTable dtDistinctSubjects = new DataTable();
                        DataTable dtDistinctTestName = new DataTable();
                        DataTable dtDistinctCommonTestName = new DataTable();
                        DataTable dtCollegeInfo = new DataTable();
                        DataTable dtStudActivityMarks = new DataTable();
                        DataTable dtStudPart = new DataTable();
                        DataTable dtOtherSubjects = new DataTable();

                        DataTable dtConsideredSubjects = new DataTable();
                        DataTable dtCompulsarySubjects = new DataTable();

                        PdfImage CollegeLeftLogo = null;
                        PdfImage collegeRightLogo = null;
                        PdfImage studPhoto = null;
                        PdfLine pdfLine;
                        PdfTextArea pdfTA;
                        PdfTablePage tblPage;
                        PdfTable tblStudentDetails;
                        PdfTable tblStudentMarks;
                        PdfTable tblStudentActivity;
                        PdfTable tblStudentPerformanceAnalysis;
                        PdfTable tblSign;

                        DataTable dtDistinctSingleSubject = new DataTable();
                        DataTable dtCurrentSingleSubjectType = new DataTable();
                        DataTable dtCurrentSingleSubject = new DataTable();

                        bool hasActivity = false;

                        if (dtCollegeDetails.Rows.Count > 0)
                        {
                            dtCollegeDetails.DefaultView.RowFilter = "college_code='" + studentCollegeCode + "'";
                            dtCollegeInfo = dtCollegeDetails.DefaultView.ToTable();
                        }
                        if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0)
                        {
                            dsStudMarks.Tables[0].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "'";
                            dsStudMarks.Tables[0].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtDistinctSubjects = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");

                            dsStudMarks.Tables[0].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and isSingleSubject=1";
                            dsStudMarks.Tables[0].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtDistinctSingleSubject = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");

                        }
                        if (dsStudMarks.Tables.Count > 1 && dsStudMarks.Tables[1].Rows.Count > 0)
                        {
                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and App_No='" + appNo + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtCurrentSemesterSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "subject_type", "isSingleSubject", "isCompulsaoryForGrandTotal", "isConsiderForGrandTotal");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and isSingleSubject=1 and App_No='" + appNo + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtCurrentSingleSubjectType = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subjectMarkType", "subType_no", "subject_type", "isSingleSubject");
                            dtCurrentSingleSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subjectMarkType", "subject_code", "subject_name", "isSingleSubject");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and subject_type<>'others' and subject_type<>'other' and App_No='" + appNo + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "subjectpriority,subType_no,subject_code";
                            dtPreviousSemesterSubject = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectpriority", "acronym", "subjectMarkType", "subType_no", "isSingleSubject");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester ='" + semester + "' and sections='" + section + "'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "Criteria_no";
                            dtDistinctTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria", "Criteria_no", "Convert_Minmark", "Convert_Maxmark");
                            dtDistinctCommonTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and criteria='others'";
                            dsStudMarks.Tables[1].DefaultView.Sort = "Criteria_no";
                            dtOtherSubjects = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria", "Criteria_no", "Convert_Minmark", "Convert_Maxmark");

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and sections='" + section + "' and App_No='" + appNo + "'";
                            dtCurrentStudentMarks = dsStudMarks.Tables[1].DefaultView.ToTable();

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester<'" + semester + "' and sections='" + section + "' and App_No='" + appNo + "'";
                            dtPreviousStudentMarks = dsStudMarks.Tables[1].DefaultView.ToTable();

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and App_No='" + appNo + "' and isConsiderForGrandTotal =1 and isCompulsaoryForGrandTotal  =0 ";
                            dtConsideredSubjects = dsStudMarks.Tables[1].DefaultView.ToTable();

                            dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and App_No='" + appNo + "' and isCompulsaoryForGrandTotal  =1 ";
                            dtCompulsarySubjects = dsStudMarks.Tables[1].DefaultView.ToTable();

                        }
                        if (dsPart.Tables.Count > 0)
                        {
                            if (dsPart.Tables[0].Rows.Count > 0)
                            {
                                totalParts = dsPart.Tables[0].Rows.Count;
                            }
                            if (dsPart.Tables.Count >= 2 && dsPart.Tables[1].Rows.Count > 0)
                            {
                                dsPart.Tables[1].DefaultView.RowFilter = string.Empty;
                                dtStudPart = dsPart.Tables[1].DefaultView.ToTable(true);
                            }
                            if (dsPart.Tables.Count >= 3 && dsPart.Tables[2].Rows.Count > 0)
                            {
                                dsPart.Tables[2].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "'";
                                dtStudPart = dsPart.Tables[2].DefaultView.ToTable(true, "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle");
                                //ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark
                            }
                            if (dsPart.Tables.Count >= 4 && dsPart.Tables[3].Rows.Count > 0)
                            {
                                dsPart.Tables[3].DefaultView.RowFilter = "Roll_No='" + studentRollNo + "'";
                                dtStudActivityMarks = dsPart.Tables[3].DefaultView.ToTable(true, "Roll_No", "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle", "term", "Mark", "Grade", "description");
                                totalParts = dsPart.Tables[3].DefaultView.ToTable(true, "Part_No").Rows.Count;
                                if (dtStudActivityMarks.Rows.Count > 0)
                                {
                                    hasActivity = true;
                                }
                            }
                        }
                        else
                        {
                            hasActivity = false;
                        }
                        if (dtCurrentStudentMarks.Rows.Count > 0)
                        {
                            isReportSave = true;
                            status = true;
                            pdfReportPage = pdfDocumentReport.NewPage();
                            MemoryStream memoryStream = new MemoryStream();
                            if (dtStudentPhotos.Rows.Count > 0)
                            {
                                byte[] file = (byte[])dtStudentPhotos.Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg")))
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg")))
                            {
                                studPhoto = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + appNo + ".jpeg"));
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg")))
                                {
                                    studPhoto = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                }
                            }
                            if (dtCollegeInfo.Rows.Count > 0)
                            {
                                clgname = Convert.ToString(dtCollegeInfo.Rows[0]["collname"]);
                                clgaff = "(" + Convert.ToString(dtCollegeInfo.Rows[0]["affliatedby"]).Split(',')[0] + ")";
                                clgaddress1 = Convert.ToString(dtCollegeInfo.Rows[0]["address1"]).Trim();
                                clgaddress2 = Convert.ToString(dtCollegeInfo.Rows[0]["address2"]).Trim();
                                clgaddress3 = Convert.ToString(dtCollegeInfo.Rows[0]["address3"]).Trim();
                                clgdistrict = Convert.ToString(dtCollegeInfo.Rows[0]["district"]).Trim();
                                clgpincode = Convert.ToString(dtCollegeInfo.Rows[0]["pincode"]).Trim();
                                clgemail = "Email : " + Convert.ToString(dtCollegeInfo.Rows[0]["email"]).Trim();
                                clgfulladdress = string.Empty;

                                #region College Address

                                if (clgaddress1.Trim().Trim(',') != "")
                                {
                                    //clgfulladdress = clgaddress1.Trim().Trim(',');
                                }
                                if (clgaddress2.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += ", " + clgaddress2.Trim().Trim(',');
                                    }
                                    else
                                    {
                                        clgfulladdress = clgaddress2.Trim().Trim(',');
                                    }
                                }
                                if (clgaddress3.Trim().Trim(',') != "")
                                {
                                    //if (clgfulladdress != "")
                                    //{
                                    //    clgfulladdress += ", " + clgaddress3.Trim().Trim(',');
                                    //}
                                    //else
                                    //{
                                    //    clgfulladdress = clgaddress3.Trim().Trim(',');
                                    //}
                                }
                                if (clgdistrict.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += ", " + clgdistrict.Trim().Trim(',');
                                    }
                                    else
                                    {
                                        clgfulladdress = clgdistrict.Trim().Trim(',');
                                    }
                                }
                                if (clgpincode.Trim().Trim(',') != "")
                                {
                                    if (clgfulladdress != "")
                                    {
                                        clgfulladdress += "-" + clgpincode.Trim().Trim(',').Trim('.') + ".";
                                    }
                                    else
                                    {
                                        clgfulladdress = clgpincode.Trim().Trim(',').Trim('.') + ".";
                                    }
                                }

                                #endregion College Address

                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    byte[] file = (byte[])dtCollegeInfo.Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    CollegeLeftLogo = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"));
                                    pdfReportPage.Add(CollegeLeftLogo, posX, posY, 540);
                                }

                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    byte[] file = (byte[])dtCollegeInfo.Rows[0]["logo2"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                {
                                    //collegeRightLogo = pdfDocumentReport.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + studentCollegeCode.ToString() + ".jpeg"));
                                    //pdfReportPage.Add(collegeRightLogo, pdfDocumentReport.PageWidth - 60, posY, 540);

                                }
                            }

                            #region College Details

                            //pdfTA=new PdfTextArea(
                            //posY += 10;
                            pdfTA = new PdfTextArea(fontColName, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgname);
                            pdfReportPage.Add(pdfTA);

                            posY += 20;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgaff);
                            pdfReportPage.Add(pdfTA);

                            posY += 20;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, clgfulladdress);
                            pdfReportPage.Add(pdfTA);

                            posY += 23;
                            pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "PERFORMANCE RECORD");
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Academic Year

                            posY += 20;
                            pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "Academic Year :" + academicYear);
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Student Details

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 2, 9, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 85, 8, 350, 158, 8, 180, 115, 8, 150 });

                            tblStudentDetails.Cell(0, 0).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 0).SetContent("Name");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 2).SetContent(studentName);
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 3).SetContent("Admission No");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(studentRollAdmit);
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 6).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 6).SetContent("Std & Sec");
                            tblStudentDetails.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 7).SetContent(":");
                            tblStudentDetails.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 8).SetContent(studClassandSec);
                            tblStudentDetails.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 0).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(1, 0).SetContent("DOB");
                            tblStudentDetails.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 1).SetContent(":");
                            tblStudentDetails.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 2).SetContent(studentDOB);
                            tblStudentDetails.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 3).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(1, 3).SetContent("Blood Group");
                            tblStudentDetails.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 4).SetContent(":");
                            tblStudentDetails.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 5).SetContent(studentBloodGroup);
                            tblStudentDetails.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 6).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(1, 6).SetContent("House");
                            tblStudentDetails.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(1, 7).SetContent(":");
                            tblStudentDetails.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(1, 8).SetContent(studentHouse);
                            tblStudentDetails.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            posY += 22;
                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            double height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height));

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 1, 6, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 210, 5, 270, 133, 5, 230 });

                            tblStudentDetails.Cell(0, 0).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 0).SetContent("Father's / Guardian Name");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 2).SetContent(studentFatherName);
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 3).SetContent("Mother's Name");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(studentMotherName);
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height));

                            tblStudentDetails = pdfDocumentReport.NewTable(fontReportContent, 2, 12, 5);
                            tblStudentDetails.VisibleHeaders = false;
                            tblStudentDetails.SetBorders(Color.Black, 1, BorderType.None);
                            tblStudentDetails.SetColumnsWidth(new int[] { 38, 6, 40, 38, 6, 35, 30, 6, 30, 30, 6, 30 });

                            tblStudentDetails.Cell(0, 0).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 0).SetContent("Attendance");
                            tblStudentDetails.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 1).SetContent(":");
                            tblStudentDetails.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            string attendanceDays = string.Empty;
                            if (per_workingdays > 0)
                            {
                                attendanceDays = ((pre_present_date > 0) ? Convert.ToString(pre_present_date).Trim() : " - ") + " / " + Convert.ToString(per_workingdays).Trim();
                                tblStudentDetails.Cell(0, 2).SetContent(attendanceDays + " Days.");
                            }
                            else
                            {
                                tblStudentDetails.Cell(0, 2).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                                tblStudentDetails.Cell(0, 2).SetContent("     Days.");
                            }
                            tblStudentDetails.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 3).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 3).SetContent("Percentage");
                            tblStudentDetails.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 4).SetContent(":");
                            tblStudentDetails.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 5).SetContent(((per_workingdays > 0) ? dum_tage_date.Contains('%') ? dum_tage_date : dum_tage_date + "%" : "   %"));
                            tblStudentDetails.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 6).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 6).SetContent("Height");
                            tblStudentDetails.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 7).SetContent(":");
                            tblStudentDetails.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 8).SetContent((!string.IsNullOrEmpty(studentHeight) && studentHeight != "0") ? studentHeight + " cm" : "");
                            tblStudentDetails.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 9).SetFont(new Font("Times New Roman", 11, FontStyle.Bold));
                            tblStudentDetails.Cell(0, 9).SetContent("Weight");
                            tblStudentDetails.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tblStudentDetails.Cell(0, 10).SetContent(":");
                            tblStudentDetails.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblStudentDetails.Cell(0, 11).SetContent((!string.IsNullOrEmpty(studentWeight) && studentWeight != "0") ? studentWeight + " kg" : "");
                            tblStudentDetails.Cell(0, 11).SetContentAlignment(ContentAlignment.MiddleLeft);

                            foreach (PdfCell pc in tblStudentDetails.CellRange(1, 0, 1, 0).Cells)
                            {
                                pc.ColSpan = 12;
                            }

                            tblPage = tblStudentDetails.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - 50, 400));
                            pdfReportPage.Add(tblPage);
                            pdfLine = tblPage.CellArea(1, 0).LowerBound(Color.Black, 1);
                            pdfReportPage.Add(pdfLine);
                            height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height)) + 5;

                            //pdfTA = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(pdfDocumentReport, 40, posY, pdfDocumentReport.PageWidth - 80, 20), ContentAlignment.MiddleCenter, "-----------------------------------------------------------------------------------------------------------");
                            //pdfReportPage.Add(pdfTA);

                            #endregion

                            #region Performance Analysis

                            int ReduceColumnCount = 0;

                            Dictionary<string, bool> dicSubjectWiseAbsent = new Dictionary<string, bool>();
                            Dictionary<string, double> dicSubjectWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicTestWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, int> dicSubjectTypeWiseSubjectCount = new Dictionary<string, int>();
                            Dictionary<string, Dictionary<string, double>> dicTermWiseTotal = new Dictionary<string, Dictionary<string, double>>();
                            Dictionary<string, Dictionary<string, bool>> dicTermWiseSubjectAbsent = new Dictionary<string, Dictionary<string, bool>>();

                            Dictionary<string, string> dicSingleSubjecttotalColumn = new Dictionary<string, string>();
                            Dictionary<string, double> dicSingleSubjecttotalMarks = new Dictionary<string, double>();
                            Dictionary<string, double> dicSingleSubjectOveralltotalMarks = new Dictionary<string, double>();

                            if (dtPreviousStudentMarks.Rows.Count > 0)
                            {
                                DataTable dtTerm = new DataTable();
                                dtTerm = dtPreviousStudentMarks.DefaultView.ToTable(true, "semester");

                                string conductedTestNames = string.Empty;
                                List<string> lstTestName = dtDistinctCommonTestName.AsEnumerable().Select(r => r.Field<string>("criteria")).ToList();
                                conductedTestNames = string.Join("','", lstTestName.ToArray());

                                int termRow = 0;
                                foreach (DataRow drTerm in dtTerm.Rows)
                                {
                                    string term = Convert.ToString(drTerm["semester"]).Trim();
                                    double maxTestMark = 0;
                                    dicSubjectWiseTotal.Clear();
                                    dicTestWiseTotal.Clear();
                                    Dictionary<string, double> dicSubjectWiseOverAllTotal = new Dictionary<string, double>();
                                    termRow++;
                                    int subjectCol = 1;
                                    string displayTerm = "Term - " + ToRoman(term);
                                    Dictionary<string, bool> dicSubjectWiseAbsent1 = new Dictionary<string, bool>();
                                    foreach (DataRow drSubject in dtPreviousSemesterSubject.Rows)
                                    {
                                        string subjectTCode = Convert.ToString(drSubject["subject_code"]).Trim();
                                        string subjectTName = Convert.ToString(drSubject["subject_name"]).Trim();
                                        string subjectTAcronymn = Convert.ToString(drSubject["acronym"]).Trim();

                                        string subjectType = Convert.ToString(drSubject["subType_no"]).Trim();
                                        string isSingleSubject = Convert.ToString(drSubject["isSingleSubject"]).Trim();

                                        if (isSingleSubject.ToLower() == "true")
                                        {
                                            if (!dicSingleSubjecttotalColumn.ContainsKey(subjectTAcronymn))
                                            {
                                                ReduceColumnCount++;
                                                dicSingleSubjecttotalColumn.Add(subjectTAcronymn, subjectTCode);
                                            }
                                        }

                                        double maxSubjecTestMark = 0;
                                        int testColumn = 1;
                                        subjectCol++;
                                        maxTestMark = 0;
                                        foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                        {
                                            string subjectMaxMark = string.Empty;
                                            string subjectminMark = string.Empty;
                                            string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                            string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                            string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                            string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                            double minimumTestMark = 0;
                                            double.TryParse(convertMin, out minimumTestMark);
                                            double maximumTestMark = 0;
                                            double.TryParse(convertMax, out maximumTestMark);
                                            maxTestMark += maximumTestMark;
                                            DataView dvStudMarks = new DataView();
                                            dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and semester='" + term + "' and criteria='" + studTestName + "'";
                                            dvStudMarks = dtPreviousStudentMarks.DefaultView;
                                            if (dvStudMarks.Count > 0)
                                            {
                                                subjectMaxMark = Convert.ToString(dvStudMarks[0]["Conducted_max"]).Trim();
                                                subjectminMark = Convert.ToString(dvStudMarks[0]["Conduct_Minmark"]).Trim();
                                                convertMin = Convert.ToString(dvStudMarks[0]["Convert_Minmark"]).Trim();
                                                convertMax = Convert.ToString(dvStudMarks[0]["Convert_Maxmark"]).Trim();
                                                string subjectMark = Convert.ToString(dvStudMarks[0]["marks_obtained"]).Trim();

                                                double testSubMarks = 0;
                                                double.TryParse(subjectMark, out testSubMarks);

                                                double testMinMark = 0;
                                                double.TryParse(convertMin, out testMinMark);

                                                double testMaxMark = 0;
                                                double.TryParse(convertMax, out testMaxMark);
                                                maxSubjecTestMark += testMaxMark;

                                                double testSubMinMarks = 0;
                                                double.TryParse(subjectminMark, out testSubMinMarks);

                                                double testSubMaxMarks = 0;
                                                double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                                double convertTestMark = testSubMarks;
                                                double outOff100 = testSubMarks;
                                                if (testSubMarks >= 0 && !string.IsNullOrEmpty(subjectMark))
                                                {
                                                    if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                        convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;

                                                    convertTestMark = Math.Round(convertTestMark, 2, MidpointRounding.AwayFromZero);

                                                    if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                        outOff100 = (testSubMarks / testSubMaxMarks) * 100;

                                                    if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                        dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                    else
                                                        dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);

                                                    if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);

                                                    if (isSingleSubject.ToLower() == "true")
                                                    {
                                                        if (dicSingleSubjecttotalMarks.ContainsKey(subjectTCode.Trim().ToLower()))
                                                            dicSingleSubjecttotalMarks[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                        else
                                                            dicSingleSubjecttotalMarks.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                    }
                                                }
                                            }
                                            if (testColumn == dtDistinctTestName.Rows.Count)
                                            {
                                                double subjectWiseTotal = 0;
                                                if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                    subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]), 2, MidpointRounding.AwayFromZero);
                                                //subjectWiseTotal = (dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]);
                                                if (!dicSubjectWiseOverAllTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                    dicSubjectWiseOverAllTotal.Add(subjectTCode.Trim().ToLower(), subjectWiseTotal);
                                                else
                                                    dicSubjectWiseOverAllTotal[subjectTCode.Trim().ToLower()] = subjectWiseTotal;

                                                //dicSingleSubjectOveralltotalMarks
                                                if (isSingleSubject.ToLower() == "true")
                                                {
                                                    if (!dicSingleSubjectOveralltotalMarks.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicSingleSubjectOveralltotalMarks.Add(subjectTCode.Trim().ToLower(), subjectWiseTotal);
                                                    else
                                                        dicSingleSubjectOveralltotalMarks[subjectTCode.Trim().ToLower()] = subjectWiseTotal;
                                                }
                                            }
                                            testColumn++;
                                        }
                                    }
                                    dicTermWiseTotal.Add(term.ToLower().Trim(), dicSubjectWiseOverAllTotal);
                                    dicTermWiseSubjectAbsent.Add(term.ToLower().Trim(), dicSubjectWiseAbsent1);
                                }
                            }

                            #endregion

                            #region Student Marks Content

                            pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "TERM - " + ToRoman(semester));
                            pdfReportPage.Add(pdfTA);
                            posY += 23;

                            double markReportHeight = 0;
                            int totalRows = dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count + 2;
                            int totalColumns = 0;
                            if (studentStandard.Trim().ToLower() != "x")
                                totalColumns = (dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count) + 2 + ((semester.Trim() == "1") ? 0 : 1);
                            else
                                totalColumns = (dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count) + 2 + ((semester.Trim() == "1") ? 1 : 1);

                            if (studentStandard.Trim().ToLower() != "x")  //modified by prabha
                                tblStudentMarks = pdfDocumentReport.NewTable(fontReportContent, totalRows, totalColumns, 3);
                            else
                                tblStudentMarks = pdfDocumentReport.NewTable(fontReportContent, totalRows, totalColumns - 1, 3);

                            tblStudentMarks.VisibleHeaders = false;
                            tblStudentMarks.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            tblStudentMarks.Cell(0, 0).SetContent("Subject");
                            tblStudentMarks.Cell(0, 0).SetFont(fontReportContentBold);
                            tblStudentMarks.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblStudentMarks.Columns[0].SetWidth(160);

                            int subjectRow = 0;
                            dicSubjectWiseTotal.Clear();
                            dicTestWiseTotal.Clear();
                            double overAllSubjectWeightage = 0;
                            dicSubjectTypeWiseSubjectCount = new Dictionary<string, int>();
                            Dictionary<string, double> dicSubjectTypeWiseAverage = new Dictionary<string, double>();
                            Dictionary<string, double> dicSubjectTypeTestWiseAverage = new Dictionary<string, double>();
                            Dictionary<string, double> dicSingleSubjectTestWiseTotal = new Dictionary<string, double>();

                            Dictionary<string, double> dicSingleSubjectWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicSingleSubjectWiseOverAllTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicCommonSubjectWiseTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicCommonSubjectWiseOverAllTotal = new Dictionary<string, double>();

                            Dictionary<string, double> dicConsiderSubjectTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicCompulsarysubjectsTotal = new Dictionary<string, double>();

                            Dictionary<string, double> dicConsiderSubjectTypeTotal = new Dictionary<string, double>();
                            Dictionary<string, double> dicCompulsarysubjectsTypeTotal = new Dictionary<string, double>();

                            bool isAbsent = false;
                            bool isWeighted = false;
                            dtCurrentStudentMarks.DefaultView.RowFilter = "marks_obtained<0";
                            DataView dvStudentAbsent = dtCurrentStudentMarks.DefaultView;
                            if (dvStudentAbsent.Count > 0)
                                isAbsent = true;
                            bool isPrevAbsent = false;
                            dtPreviousStudentMarks.DefaultView.RowFilter = "marks_obtained<0";
                            DataView dvPrevStudentAbsent = dtPreviousStudentMarks.DefaultView;
                            if (dvPrevStudentAbsent.Count > 0)
                                isPrevAbsent = true;
                            foreach (DataRow drSubjects in dtCurrentSemesterSubject.Rows)
                            {
                                string subjectTCode = Convert.ToString(drSubjects["subject_code"]).Trim();
                                string subjectTName = Convert.ToString(drSubjects["subject_name"]).Trim();
                                string markOrGrade = Convert.ToString(drSubjects["subjectMarkType"]).Trim();
                                string subjectTypeName = Convert.ToString(drSubjects["subject_type"]).Trim();
                                string subjectTypeNo = Convert.ToString(drSubjects["subType_no"]).Trim();
                                string subjectTSingle = Convert.ToString(drSubjects["isSingleSubject"]).Trim();

                                string compulsorySubjectForGrandTotal = Convert.ToString(drSubjects["isCompulsaoryForGrandTotal"]).Trim();

                                string considerSubjectForGrandTotal = Convert.ToString(drSubjects["isConsiderForGrandTotal"]).Trim();

                                //string SubjectTypeNameN = getSubjectTypeName(dsStudMarks.Tables[1], subjectTCode);
                                bool others = false;

                                int subjectTypeWiseSubjectCount = 0;
                                if (!dicSubjectTypeWiseSubjectCount.ContainsKey(subjectTypeNo.Trim()))
                                    dicSubjectTypeWiseSubjectCount.Add(subjectTypeNo.Trim(), 1);
                                else
                                    dicSubjectTypeWiseSubjectCount[subjectTypeNo.Trim()] += 1;
                                subjectTypeWiseSubjectCount = (!dicSubjectTypeWiseSubjectCount.ContainsKey(subjectTypeNo.Trim())) ? 0 : dicSubjectTypeWiseSubjectCount[subjectTypeNo.Trim()];
                                dtCurrentSemesterSubject.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                                int subjectCount = 0;
                                DataTable dtSubjectTypeWiseSubject = dtCurrentSemesterSubject.DefaultView.ToTable(true, "subject_code", "subject_name", "acronym");
                                subjectCount = dtSubjectTypeWiseSubject.Rows.Count;

                                int testColumn = 1;
                                subjectRow++;
                                tblStudentMarks.Cell(subjectRow, 0).SetContent(subjectTName);
                                tblStudentMarks.Cell(subjectRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                double maxTestMark = 0;
                                double maxSubjecTestMark = 0;
                                bool isSubjectAbsent = false;
                                dtCurrentStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and marks_obtained<0";
                                DataView dvSubjectAbsent = dtCurrentStudentMarks.DefaultView;
                                if (dvSubjectAbsent.Count > 0)
                                    isSubjectAbsent = true;

                                bool isPrevSubjectAbsent = false;
                                dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and marks_obtained<0";
                                DataView dvPrevSubjectAbsent = dtPreviousStudentMarks.DefaultView;
                                if (dvPrevSubjectAbsent.Count > 0)
                                    isPrevSubjectAbsent = true;

                                bool isSubjectTypeAbsent = false;
                                dtCurrentStudentMarks.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "' and marks_obtained<0";
                                DataView dvSubjectTypeAbsent = dtCurrentStudentMarks.DefaultView;
                                if (dvSubjectTypeAbsent.Count > 0)
                                    isSubjectTypeAbsent = true;

                                bool isPrevSubjectTypeAbsent = false;
                                dtPreviousStudentMarks.DefaultView.RowFilter = "subject_type='" + subjectTypeName + "' and marks_obtained<0";
                                DataView dvPrevSubjectTypeAbsent = dtPreviousStudentMarks.DefaultView;
                                if (dvPrevSubjectTypeAbsent.Count > 0)
                                    isPrevSubjectTypeAbsent = true;

                                foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                {
                                    string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                    string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                    string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                    string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                    double minimumTestMark = 0;
                                    double.TryParse(convertMin, out minimumTestMark);
                                    double maximumTestMark = 0;
                                    double.TryParse(convertMax, out maximumTestMark);
                                    maxTestMark += maximumTestMark;
                                    string subjectMaxMark = string.Empty;
                                    string subjectminMark = string.Empty;
                                    int countavg = 0;
                                    bool includeTest = false;
                                    if (studTestName.Trim().ToLower() != "others")
                                        includeTest = true;
                                    else
                                        includeTest = false;
                                    //modified
                                    if (subjectTypeName.Trim().ToLower() == "others")
                                    {
                                        others = true;
                                    }

                                    if (subjectRow == 1)
                                    {
                                        tblStudentMarks.Columns[testColumn].SetWidth(75);
                                    }

                                    dtCurrentStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and Criteria_no='" + studTestNo + "' and isConsiderForGrandTotal =1 and isCompulsaoryForGrandTotal  =0";
                                    dtConsideredSubjects = dtCurrentStudentMarks.DefaultView.ToTable();

                                    dtCurrentStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and Criteria_no='" + studTestNo + "' and isCompulsaoryForGrandTotal =1";
                                    dtCompulsarySubjects = dtCurrentStudentMarks.DefaultView.ToTable();

                                    DataView dvStudMarks = new DataView();
                                    dtCurrentStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and Criteria_no='" + studTestNo + "'";
                                    dvStudMarks = dtCurrentStudentMarks.DefaultView;

                                    bool setTestName = false;
                                    if (dvStudMarks.Count > 0)
                                    {
                                        subjectMaxMark = Convert.ToString(dvStudMarks[0]["Conducted_max"]).Trim();
                                        subjectminMark = Convert.ToString(dvStudMarks[0]["Conduct_Minmark"]).Trim();
                                        convertMin = Convert.ToString(dvStudMarks[0]["Convert_Minmark"]).Trim();
                                        convertMax = Convert.ToString(dvStudMarks[0]["Convert_Maxmark"]).Trim();
                                        markOrGrade = Convert.ToString(dvStudMarks[0]["subjectMarkType"]).Trim();
                                        string subjectMark = Convert.ToString(dvStudMarks[0]["marks_obtained"]).Trim();
                                        string sems = Convert.ToString(dvStudMarks[0]["semester"]).Trim();

                                        double testSubMarks = 0;
                                        double.TryParse(subjectMark, out testSubMarks);

                                        double testMinMark = 0;
                                        double.TryParse(convertMin, out testMinMark);

                                        double testMaxMark = 0;
                                        double.TryParse(convertMax, out testMaxMark);
                                        maxSubjecTestMark += testMaxMark;

                                        double testSubMinMarks = 0;
                                        double.TryParse(subjectminMark, out testSubMinMarks);

                                        double testSubMaxMarks = 0;
                                        double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                        double convertTestMark = testSubMarks;
                                        double outOff100 = testSubMarks;
                                        string displayMark = string.Empty;
                                        string displayMark1 = string.Empty;
                                        string displayGrade = string.Empty;
                                        bool isFail = false;
                                        if (testSubMarks < 0)
                                        {
                                            displayMark1 = displayMark = getMarkText(subjectMark);
                                            isFail = true;
                                            isSubjectAbsent = true;
                                            isAbsent = true;
                                        }
                                        else if (string.IsNullOrEmpty(subjectMark))
                                        {
                                            displayMark1 = displayMark = "--";
                                            isFail = true;
                                        }
                                        else
                                        {
                                            isFail = false;
                                            displayMark = testSubMarks.ToString();
                                            if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;
                                            convertTestMark = Math.Round(convertTestMark, 2, MidpointRounding.AwayFromZero);
                                            displayMark1 = convertTestMark.ToString();

                                            if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                outOff100 = (testSubMarks / testSubMaxMarks) * 100;
                                            if (subjectTSingle.Trim() == "0" || subjectTSingle.Trim().ToLower() == "false")
                                            {
                                                if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                    dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                else
                                                    dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);
                                                if (includeTest && markOrGrade != "2")//SubjectTypeName.Trim().ToLower() != "others" && 
                                                {
                                                    if (dicCommonSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicCommonSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicCommonSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                }
                                                if (includeTest && subjectTypeName.Trim().ToLower() != "others" && markOrGrade != "2")// 
                                                {
                                                    if (dicCommonSubjectWiseOverAllTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicCommonSubjectWiseOverAllTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicCommonSubjectWiseOverAllTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                }
                                                if (compulsorySubjectForGrandTotal.ToLower().Trim() == "true")
                                                {
                                                    if (dicCompulsarysubjectsTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicCompulsarysubjectsTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicCompulsarysubjectsTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);

                                                }
                                                else if (considerSubjectForGrandTotal.ToLower().Trim() == "true")
                                                {
                                                    if (dicConsiderSubjectTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        dicConsiderSubjectTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicConsiderSubjectTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                }
                                            }
                                            else
                                            {
                                                if (compulsorySubjectForGrandTotal.ToLower().Trim() == "true")
                                                {
                                                    if (dicCompulsarysubjectsTypeTotal.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                        dicCompulsarysubjectsTypeTotal[subjectTypeNo.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicCompulsarysubjectsTypeTotal.Add(subjectTypeNo.Trim().ToLower(), convertTestMark);

                                                }
                                                else if (considerSubjectForGrandTotal.ToLower().Trim() == "true")
                                                {
                                                    if (dicConsiderSubjectTypeTotal.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                        dicConsiderSubjectTypeTotal[subjectTypeNo.Trim().ToLower()] += convertTestMark;
                                                    else
                                                        dicConsiderSubjectTypeTotal.Add(subjectTypeNo.Trim().ToLower(), convertTestMark);
                                                }
                                            }
                                            countavg++;

                                            if (dicSubjectTypeTestWiseAverage.ContainsKey(subjectTypeNo.Trim() + "@" + studTestNo.Trim()))
                                                dicSubjectTypeTestWiseAverage[subjectTypeNo.Trim() + "@" + studTestNo.Trim()] += convertTestMark;
                                            else
                                                dicSubjectTypeTestWiseAverage.Add(subjectTypeNo.Trim() + "@" + studTestNo.Trim(), convertTestMark);

                                            if (dicSubjectTypeWiseAverage.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                dicSubjectTypeWiseAverage[subjectTypeNo.Trim().ToLower()] += convertTestMark;
                                            else
                                                dicSubjectTypeWiseAverage.Add(subjectTypeNo.Trim().ToLower(), convertTestMark);
                                            if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                            else
                                                dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                        }
                                        DataView dvGrade = new DataView();
                                        if (dtGradeDetails.Rows.Count > 0)
                                        {
                                            dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + sems + "' and Criteria='" + studTestName.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                            dvGrade = dtGradeDetails.DefaultView;
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='" + studTestName.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + sems + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                        }
                                        if (dvGrade.Count > 0)
                                        {
                                            displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                        }
                                        else
                                        {
                                            displayGrade = "--";
                                        }
                                        if (markOrGrade == "2")
                                        {
                                            displayMark1 = (isFail) ? displayMark1 : displayGrade;
                                        }

                                        if (includeTest)
                                        {

                                            tblStudentMarks.Cell(subjectRow, testColumn).SetContent(displayMark1);
                                            tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            if (!setTestName)
                                            {
                                                tblStudentMarks.Cell(0, testColumn).SetContent(studTestName + "\n(" + testMaxMark + ")");
                                                tblStudentMarks.Cell(0, testColumn).SetFont(fontReportContentBold);
                                                tblStudentMarks.Cell(0, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                        else
                                        {
                                            if (studentStandard.Trim().ToLower() != "x")
                                            {
                                                tblStudentMarks.Cell(subjectRow, totalColumns - 1).SetContent(displayMark1);
                                                tblStudentMarks.Cell(subjectRow, totalColumns - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                            else
                                            {
                                                tblStudentMarks.Cell(subjectRow, 3).SetContent(displayMark1);
                                                tblStudentMarks.Cell(subjectRow, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (others)//&& markOrGrade == "2"
                                        {
                                            if (studentStandard.Trim().ToLower() == "x")
                                            {
                                                if (semester.Trim() == "1")
                                                    tblStudentMarks.Cell(subjectRow, 0).ColSpan = 3;
                                                else if (semester.Trim() == "3")
                                                    tblStudentMarks.Cell(subjectRow, 0).ColSpan = 2;
                                                else
                                                    tblStudentMarks.Cell(subjectRow, 0).ColSpan = 3;
                                            }
                                            else
                                            {
                                                if (semester.Trim() == "1")
                                                    tblStudentMarks.Cell(subjectRow, 0).ColSpan = 2;
                                                else
                                                    tblStudentMarks.Cell(subjectRow, 0).ColSpan = 3;
                                            }
                                        }
                                        else
                                        {
                                            if (includeTest)
                                            {
                                                if (studentStandard.Trim().ToLower() != "x")  //modified  by prabha
                                                {
                                                    tblStudentMarks.Cell(subjectRow, testColumn).SetContent("--");
                                                    tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else
                                                {
                                                    tblStudentMarks.Cell(subjectRow, testColumn - 1).SetContent("--");
                                                    tblStudentMarks.Cell(subjectRow, testColumn - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                            }
                                        }
                                    }
                                    if (testColumn == dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count)
                                    {
                                        tblStudentMarks.Cell(0, testColumn + 1).SetContent("Total\n(100)");
                                        tblStudentMarks.Cell(0, testColumn + 1).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(0, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblStudentMarks.Columns[testColumn + 1].SetWidth(50);
                                        if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                        {
                                            double subjectWiseTotal = 0;
                                            subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]), 2, MidpointRounding.AwayFromZero);
                                            //subjectWiseTotal = (dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]);
                                            string displayTotal = subjectWiseTotal.ToString();
                                            DataView dvGrade = new DataView();
                                            if (dtGradeDetails.Rows.Count > 0)
                                            {
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                            }
                                            if (markOrGrade == "2")
                                            {
                                                if (dvGrade.Count > 0)
                                                {
                                                    displayTotal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                                }
                                                else
                                                {
                                                    displayTotal = subjectWiseTotal.ToString();
                                                }
                                            }
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectAbsent) ? "--" : displayTotal);
                                        }
                                        else
                                            if (studentStandard.Trim().ToLower() != "x")
                                            {
                                                if (!others)//&& markOrGrade == "2"
                                                    tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectAbsent) ? "--" : "--");
                                                else
                                                    if (semester.Trim() == "1")
                                                        tblStudentMarks.Cell(subjectRow, 0).ColSpan = 3;
                                                    else if (semester.Trim() == "3")
                                                        tblStudentMarks.Cell(subjectRow, 0).ColSpan = 2;
                                                    else
                                                        tblStudentMarks.Cell(subjectRow, 0).ColSpan = 4;
                                            }
                                            else
                                                tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectAbsent) ? "--" : "--");

                                        tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);  // to be modified

                                        if (studentStandard.Trim().ToLower() != "x")
                                        {
                                            switch (semester.Trim().ToLower())
                                            {
                                                case "2":
                                                case "3":
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetContent((semester.Trim().ToLower() == "2") ? "Subject Average of T1 & T2" : "Weighted Average");
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(0, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblStudentMarks.Columns[testColumn + 2].SetWidth(85);

                                                    double cummulativeSubjectTotal = 0;
                                                    double currentTermSubjectTotal = 0;
                                                    if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        currentTermSubjectTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]), 2, MidpointRounding.AwayFromZero);
                                                    //currentTermSubjectTotal = (dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]);
                                                    foreach (KeyValuePair<string, Dictionary<string, double>> keyValue in dicTermWiseTotal)
                                                    {
                                                        string prevTerm = keyValue.Key;
                                                        Dictionary<string, double> dicPrevSubjectTotal = keyValue.Value;
                                                        double subjectTotal = 0;
                                                        double subjectTypeTotal = 0;
                                                        if (dicPrevSubjectTotal.Count > 0)
                                                        {
                                                            double averageTotal = 0;
                                                            //if (subjectTSingle.Trim() == "0" || subjectTSingle.Trim().ToLower() == "false")
                                                            //{
                                                            if (dicPrevSubjectTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                            {
                                                                subjectTotal = dicPrevSubjectTotal[subjectTCode.Trim().ToLower()];
                                                            }
                                                            if (semester.Trim().ToLower() == "2")
                                                            {
                                                                cummulativeSubjectTotal += subjectTotal;
                                                            }
                                                            else
                                                            {
                                                                averageTotal = Math.Round((subjectTotal / 100) * 25, 2, MidpointRounding.AwayFromZero);
                                                                //averageTotal = (subjectTotal / 100) * 25;
                                                                cummulativeSubjectTotal += averageTotal;
                                                            }
                                                            //}
                                                            //else
                                                            //{
                                                            //foreach (DataRow drSubjectTCode in dtSubjectTypeWiseSubject.Rows)
                                                            //{
                                                            //    string subjectCodeType = Convert.ToString(drSubjectTCode["Subject_code"]).Trim().ToLower();
                                                            //    if (dicPrevSubjectTotal.ContainsKey(subjectCodeType.Trim().ToLower()))
                                                            //    {
                                                            //        subjectTotal = dicPrevSubjectTotal[subjectCodeType.Trim().ToLower()];
                                                            //    }
                                                            //    if (semester.Trim().ToLower() == "2")
                                                            //    {
                                                            //        subjectTypeTotal += subjectTotal;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        averageTotal = Math.Round((subjectTotal / 100) * 25, 1, MidpointRounding.AwayFromZero);
                                                            //        subjectTypeTotal += averageTotal;
                                                            //    }
                                                            //}
                                                            //cummulativeSubjectTotal += (subjectTypeTotal / dtSubjectTypeWiseSubject.Rows.Count);
                                                            //}
                                                        }
                                                    }
                                                    double subjectWiseTotal = 0;
                                                    if (semester.Trim().ToLower() == "2")
                                                    {
                                                        cummulativeSubjectTotal += currentTermSubjectTotal;
                                                        subjectWiseTotal = Math.Round((cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1)), 2, MidpointRounding.AwayFromZero);
                                                        //subjectWiseTotal = (cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1));
                                                    }
                                                    else
                                                    {
                                                        double averageTotal = 0;
                                                        averageTotal = Math.Round((currentTermSubjectTotal / 100) * 50, 2, MidpointRounding.AwayFromZero);
                                                        //averageTotal = (currentTermSubjectTotal / 100) * 50;
                                                        cummulativeSubjectTotal += averageTotal;
                                                        cummulativeSubjectTotal = Math.Round((cummulativeSubjectTotal), 2, MidpointRounding.AwayFromZero);
                                                        subjectWiseTotal = cummulativeSubjectTotal;
                                                    }
                                                    string displayTotal = subjectWiseTotal.ToString();
                                                    DataView dvGrade = new DataView();
                                                    if (dtGradeDetails.Rows.Count > 0)
                                                    {
                                                        if (dvGrade.Count == 0)
                                                        {
                                                            dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                            dvGrade = dtGradeDetails.DefaultView;
                                                        }
                                                        if (dvGrade.Count == 0)
                                                        {
                                                            dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Frange<='" + subjectWiseTotal + "' and Trange>='" + subjectWiseTotal + "'";
                                                            dvGrade = dtGradeDetails.DefaultView;
                                                        }
                                                    }
                                                    if (markOrGrade == "2")
                                                    {
                                                        if (dvGrade.Count > 0)
                                                        {
                                                            displayTotal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                                        }
                                                        else
                                                        {
                                                            displayTotal = subjectWiseTotal.ToString();
                                                        }
                                                    }
                                                    if (subjectTSingle.Trim() == "0" || subjectTSingle.Trim().ToLower() == "false")
                                                        overAllSubjectWeightage += subjectWiseTotal;
                                                    tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContent((isSubjectAbsent || isPrevSubjectAbsent) ? "--" : (subjectWiseTotal > 0) ? displayTotal : "--");
                                                    tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    if (dicSubjectWiseAbsent.ContainsKey(subjectTCode.Trim().ToLower()) && dicSubjectWiseAbsent[subjectTCode.Trim().ToLower()])
                                                        isWeighted = true;
                                                    if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                                    {
                                                        //tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContent((isAbsent || isPrevAbsent) ? "--" : (overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                        //tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetFont(fontReportContentBold);
                                                        //tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    }
                                                    break;
                                            }
                                        }

                                    }

                                    if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                    {
                                        int tempcolumnvlue = testColumn;
                                        if (studentStandard.Trim().ToLower() == "x")
                                        {
                                            tempcolumnvlue -= 1;
                                        }

                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetContent("Grand Total");
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                        if (studentStandard.Trim().ToLower() == "x")
                                        {
                                            //if (semester.Trim() == "1")
                                            if (semester.Trim() == "1")
                                                tblStudentMarks.Cell(subjectRow + 1, 0).ColSpan = 3;
                                            else if (semester.Trim() == "2")
                                                tblStudentMarks.Cell(subjectRow + 1, 0).ColSpan = 3;
                                            else if (semester.Trim() == "3")
                                                tblStudentMarks.Cell(subjectRow + 1, 0).ColSpan = 2;
                                            tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                            //else
                                            //    tblStudentMarks.Cell(subjectRow + 1, 1).ColSpan = 2;
                                        }
                                        else
                                        {
                                            if (semester.Trim() == "1")
                                            {
                                                tblStudentMarks.Cell(subjectRow + 1, 0).ColSpan = 3;
                                                tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                            else
                                            {
                                                tblStudentMarks.Cell(subjectRow + 1, 0).ColSpan = 4;
                                                tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }

                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetContent("Average");
                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetFont(fontReportContentBold);
                                        tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        //tblStudentMarks.Cell(subjectRow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        double singleSubjectSum = (dicSingleSubjectTestWiseTotal.ContainsKey(studTestNo.Trim())) ? dicSingleSubjectTestWiseTotal[studTestNo.Trim()] : 0;
                                        tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue).SetFont(fontReportContentBold);
                                        if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                        {
                                            //tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum));
                                            double average = Math.Round((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum) / (dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count - dtCurrentSingleSubject.Rows.Count), 2, MidpointRounding.AwayFromZero);
                                            //double average = (dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum) / (dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count - dtCurrentSingleSubject.Rows.Count);
                                            //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent(average);
                                        }
                                        else
                                        {
                                            //tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent((singleSubjectSum > 0) ? singleSubjectSum.ToString() : "--");
                                            //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent("--");
                                        }
                                        tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        if (testColumn == dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count)
                                        {
                                            //double sum = dicCommonSubjectWiseTotal.Sum(x => Math.Round((x.Value), 0, MidpointRounding.AwayFromZero)) + dicSingleSubjectWiseTotal.Sum(x => Math.Round(x.Value, 0, MidpointRounding.AwayFromZero));
                                            double subjectTypeWiseTotal = 0;

                                            foreach (KeyValuePair<string, double> dicKey in dicCompulsarysubjectsTypeTotal)
                                            {
                                                dtCurrentSemesterSubject.DefaultView.RowFilter = "subType_no='" + dicKey.Key.ToLower().Trim() + "'";
                                                int subjectCount1 = 0;
                                                DataTable dtSubjectTypeWiseSubject1 = dtCurrentSemesterSubject.DefaultView.ToTable(true, "subject_code", "subject_name", "acronym");
                                                subjectCount1 = dtSubjectTypeWiseSubject1.Rows.Count;
                                                subjectTypeWiseTotal += Math.Round((dicKey.Value / subjectCount1), 2, MidpointRounding.AwayFromZero);
                                                //subjectTypeWiseTotal +=(dicKey.Value / subjectCount1);
                                            }

                                            foreach (KeyValuePair<string, double> dicKey in dicConsiderSubjectTypeTotal)
                                            {
                                                dtCurrentSemesterSubject.DefaultView.RowFilter = "subType_no='" + dicKey.Key.ToLower().Trim() + "'";
                                                int subjectCount1 = 0;
                                                DataTable dtSubjectTypeWiseSubject1 = dtCurrentSemesterSubject.DefaultView.ToTable(true, "subject_code", "subject_name", "acronym");
                                                subjectCount1 = dtSubjectTypeWiseSubject1.Rows.Count;
                                                double subjectTypeWiseTotal2 = Math.Round((dicKey.Value / subjectCount1), 2, MidpointRounding.AwayFromZero);
                                                //double subjectTypeWiseTotal2 = (dicKey.Value / subjectCount1);
                                                if (!dicConsiderSubjectTotal.ContainsKey(dicKey.Key.ToLower().Trim()))
                                                    dicConsiderSubjectTotal.Add(dicKey.Key.ToLower().Trim(), subjectTypeWiseTotal2);
                                                else
                                                    dicConsiderSubjectTotal[dicKey.Key.ToLower().Trim()] += subjectTypeWiseTotal2;
                                            }
                                            Dictionary<string, double> dicOtherSubjectTotal = dicConsiderSubjectTotal.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                            int count = 0;
                                            if (bestOfSubjectCount <= 0)
                                                bestOfSubjectCount = 4;
                                            foreach (KeyValuePair<string, double> keyVal in dicOtherSubjectTotal)
                                            {
                                                if (count < bestOfSubjectCount)
                                                    subjectTypeWiseTotal += keyVal.Value;
                                                count++;
                                            }
                                            double sum = dicCompulsarysubjectsTotal.Sum(x => Math.Round((x.Value), 2, MidpointRounding.AwayFromZero)) + subjectTypeWiseTotal;
                                            //double sum = dicCompulsarysubjectsTotal.Sum(x => (x.Value) + subjectTypeWiseTotal);
                                            if (studentStandard.Trim().ToLower() != "x")
                                            {
                                                //double sum = Math.Round((total_mrks / 100) * 25, 1, MidpointRounding.AwayFromZero);
                                                if (semester.Trim() == "1")
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 1).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 1).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                                else
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }

                                            }
                                            else
                                            {
                                                if (semester.Trim() == "1")
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvlue + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                            }
                                        }
                                    }
                                    if (studTestName.Trim().ToLower() != "others")
                                        testColumn++;
                                }

                                if ((subjectTSingle.Trim() == "1" || subjectTSingle.Trim().ToLower() == "true") && subjectCount == subjectTypeWiseSubjectCount)
                                {
                                    subjectRow++;
                                    tblStudentMarks.Cell(subjectRow, 0).SetContent(subjectTypeName);
                                    tblStudentMarks.Cell(subjectRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    double subjectTypeWiseTotal = 0;
                                    testColumn = 1;
                                    maxTestMark = 0;
                                    foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                    {
                                        string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                        string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                        string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                        string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                        double minimumTestMark = 0;
                                        double.TryParse(convertMin, out minimumTestMark);
                                        double maximumTestMark = 0;
                                        double.TryParse(convertMax, out maximumTestMark);
                                        maxTestMark += maximumTestMark;
                                        double subjectAvg = 0;
                                        bool includeTest = false;
                                        if (studTestName.Trim().ToLower() != "others")
                                            includeTest = true;
                                        else
                                            includeTest = false;
                                        if (dicSubjectTypeTestWiseAverage.ContainsKey(subjectTypeNo.Trim() + "@" + studTestNo.Trim()))
                                            subjectAvg = dicSubjectTypeTestWiseAverage[subjectTypeNo.Trim() + "@" + studTestNo.Trim()];
                                        subjectAvg = subjectAvg / dtSubjectTypeWiseSubject.Rows.Count;
                                        subjectAvg = Math.Round(subjectAvg, 2, MidpointRounding.AwayFromZero);
                                        subjectTypeWiseTotal += subjectAvg;
                                        string subjectAcr = string.Empty;
                                        List<string> lstSubAcr = dtSubjectTypeWiseSubject.AsEnumerable().Select(r => r.Field<string>("acronym")).ToList();
                                        subjectAcr = string.Join(", ", lstSubAcr.ToArray());
                                        if (!string.IsNullOrEmpty(subjectAcr))
                                            subjectAcr = "(Avg:" + subjectAcr + ")";
                                        if (includeTest)
                                        {
                                            tblStudentMarks.Cell(subjectRow, 0).SetContent(subjectTypeName + "\n" + subjectAcr);
                                            tblStudentMarks.Cell(subjectRow, 0).SetFont(fontReportContentBold);

                                            tblStudentMarks.Cell(subjectRow, testColumn).SetContent((subjectAvg > 0) ? subjectAvg.ToString() : "--");
                                            tblStudentMarks.Cell(subjectRow, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblStudentMarks.Cell(subjectRow, testColumn).SetFont(fontReportContentBold);
                                        }
                                        if (includeTest && markOrGrade != "2")//SubjectTypeName.Trim().ToLower() != "others" && 
                                        {
                                            if (dicSingleSubjectWiseTotal.ContainsKey(subjectTypeNo.Trim()))
                                                dicSingleSubjectWiseTotal[subjectTypeNo.Trim()] += subjectAvg;
                                            else
                                                dicSingleSubjectWiseTotal.Add(subjectTypeNo.Trim(), subjectAvg);
                                        }
                                        if (includeTest && subjectTypeName.Trim().ToLower() != "others" && markOrGrade != "2")
                                        {
                                            if (dicSingleSubjectWiseOverAllTotal.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                dicSingleSubjectWiseOverAllTotal[subjectTypeNo.Trim().ToLower()] += subjectAvg;
                                            else
                                                dicSingleSubjectWiseOverAllTotal.Add(subjectTypeNo.Trim().ToLower(), subjectAvg);
                                        }

                                        if (dicSingleSubjectTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                            dicSingleSubjectTestWiseTotal[studTestNo.Trim()] += subjectAvg;
                                        else
                                            dicSingleSubjectTestWiseTotal.Add(studTestNo.Trim(), subjectAvg);
                                        if (testColumn == dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count)
                                        {
                                            double sum = Math.Round((subjectTypeWiseTotal), 2, MidpointRounding.AwayFromZero);
                                            //double sum =subjectTypeWiseTotal;

                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectTypeAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetFont(fontReportContentBold);
                                            if (studentStandard.Trim().ToLower() != "x")  //modified by prabha
                                            {
                                                switch (semester.Trim().ToLower())
                                                {
                                                    case "2":
                                                    case "3":
                                                        tblStudentMarks.Cell(0, testColumn + 2).SetContent((semester.Trim().ToLower() == "2") ? "Subject Average of T1 & T2" : "Weighted Average");
                                                        tblStudentMarks.Cell(0, testColumn + 2).SetFont(fontReportContentBold);
                                                        tblStudentMarks.Cell(0, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblStudentMarks.Columns[testColumn + 2].SetWidth(85);

                                                        double cummulativeSubjectTotal = 0;
                                                        double currentTermSubjectTotal = 0;
                                                        if (dicSingleSubjectWiseTotal.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                            currentTermSubjectTotal = Math.Round(dicSingleSubjectWiseTotal[subjectTypeNo.Trim().ToLower()], 2, MidpointRounding.AwayFromZero);
                                                        //currentTermSubjectTotal = dicSingleSubjectWiseTotal[subjectTypeNo.Trim().ToLower()];
                                                        foreach (KeyValuePair<string, Dictionary<string, double>> keyValue in dicTermWiseTotal)
                                                        {
                                                            string prevTerm = keyValue.Key;
                                                            Dictionary<string, double> dicPrevSubjectTotal = keyValue.Value;
                                                            double subjectTotal = 0;
                                                            double subjectTypeTotal = 0;
                                                            if (dicPrevSubjectTotal.Count > 0)
                                                            {
                                                                double averageTotal = 0;
                                                                foreach (DataRow drSubjectTCode in dtSubjectTypeWiseSubject.Rows)
                                                                {
                                                                    string subjectCodeType = Convert.ToString(drSubjectTCode["Subject_code"]).Trim().ToLower();
                                                                    if (dicPrevSubjectTotal.ContainsKey(subjectCodeType.Trim().ToLower()))
                                                                    {
                                                                        subjectTotal = dicPrevSubjectTotal[subjectCodeType.Trim().ToLower()];
                                                                    }
                                                                    if (semester.Trim().ToLower() == "2")
                                                                    {
                                                                        subjectTypeTotal += subjectTotal;
                                                                    }
                                                                    else
                                                                    {
                                                                        averageTotal = Math.Round((subjectTotal / 100) * 25, 2, MidpointRounding.AwayFromZero);
                                                                        //averageTotal = (subjectTotal / 100) * 25;
                                                                        subjectTypeTotal += averageTotal;
                                                                    }
                                                                }
                                                                cummulativeSubjectTotal += (subjectTypeTotal / dtSubjectTypeWiseSubject.Rows.Count);
                                                            }
                                                        }
                                                        double subjectWiseTotal = 0;
                                                        if (semester.Trim().ToLower() == "2")
                                                        {
                                                            cummulativeSubjectTotal += currentTermSubjectTotal;
                                                            subjectWiseTotal = Math.Round((cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1)), 2, MidpointRounding.AwayFromZero);
                                                            //subjectWiseTotal = (cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1));
                                                        }
                                                        else
                                                        {
                                                            double averageTotal = 0;
                                                            averageTotal = Math.Round((currentTermSubjectTotal / 100) * 50, 2, MidpointRounding.AwayFromZero);
                                                            //averageTotal = (currentTermSubjectTotal / 100) * 50;
                                                            cummulativeSubjectTotal += averageTotal;
                                                            cummulativeSubjectTotal = Math.Round((cummulativeSubjectTotal), 2, MidpointRounding.AwayFromZero);
                                                            //subjectWiseTotal = cummulativeSubjectTotal;
                                                        }
                                                        string displayTotal = subjectWiseTotal.ToString();

                                                        overAllSubjectWeightage += subjectWiseTotal;
                                                        if (studentStandard.Trim().ToLower() != "x")//modified
                                                        {
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 2).SetFont(fontReportContentBold);
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContent((isSubjectTypeAbsent || isPrevSubjectTypeAbsent) ? "--" : (subjectWiseTotal > 0) ? displayTotal : "--");
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        }
                                                        else
                                                        {
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetFont(fontReportContentBold);
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectTypeAbsent || isPrevSubjectTypeAbsent) ? "--" : (subjectWiseTotal > 0) ? displayTotal : "--");
                                                            tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        }

                                                        if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                                        {
                                                            if (studentStandard.Trim().ToLower() != "x")
                                                            {
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContent((isAbsent || isPrevAbsent) ? "--" : (overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetFont(fontReportContentBold);
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            }
                                                            else
                                                            {
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContent((isAbsent || isPrevAbsent) ? "--" : (overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetFont(fontReportContentBold);
                                                                tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                            else if (studentStandard.Trim().ToLower() == "x")  //modified by prabha
                                            {
                                                switch (semester.Trim().ToLower())
                                                {
                                                    case "2":
                                                    case "3":
                                                        //tblStudentMarks.Cell(0, testColumn + 2).SetContent((semester.Trim().ToLower() == "2") ? "Subject Average of T1 & T2" : "Weighted Average");
                                                        tblStudentMarks.Cell(0, testColumn + 1).SetFont(fontReportContentBold);
                                                        tblStudentMarks.Cell(0, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblStudentMarks.Columns[testColumn + 1].SetWidth(85);

                                                        double cummulativeSubjectTotal = 0;
                                                        double currentTermSubjectTotal = 0;
                                                        if (dicSingleSubjectWiseTotal.ContainsKey(subjectTypeNo.Trim().ToLower()))
                                                            currentTermSubjectTotal = Math.Round(dicSingleSubjectWiseTotal[subjectTypeNo.Trim().ToLower()], 2, MidpointRounding.AwayFromZero);
                                                        //currentTermSubjectTotal = dicSingleSubjectWiseTotal[subjectTypeNo.Trim().ToLower()];
                                                        foreach (KeyValuePair<string, Dictionary<string, double>> keyValue in dicTermWiseTotal)
                                                        {
                                                            string prevTerm = keyValue.Key;
                                                            Dictionary<string, double> dicPrevSubjectTotal = keyValue.Value;
                                                            double subjectTotal = 0;
                                                            double subjectTypeTotal = 0;
                                                            if (dicPrevSubjectTotal.Count > 0)
                                                            {
                                                                double averageTotal = 0;
                                                                foreach (DataRow drSubjectTCode in dtSubjectTypeWiseSubject.Rows)
                                                                {
                                                                    string subjectCodeType = Convert.ToString(drSubjectTCode["Subject_code"]).Trim().ToLower();
                                                                    if (dicPrevSubjectTotal.ContainsKey(subjectCodeType.Trim().ToLower()))
                                                                    {
                                                                        subjectTotal = dicPrevSubjectTotal[subjectCodeType.Trim().ToLower()];
                                                                    }
                                                                    if (semester.Trim().ToLower() == "2")
                                                                    {
                                                                        subjectTypeTotal += subjectTotal;
                                                                    }
                                                                    else
                                                                    {
                                                                        averageTotal = Math.Round((subjectTotal / 100) * 25, 2, MidpointRounding.AwayFromZero);
                                                                        //averageTotal =(subjectTotal / 100) * 25;
                                                                        subjectTypeTotal += averageTotal;
                                                                    }
                                                                }
                                                                cummulativeSubjectTotal += (subjectTypeTotal / dtSubjectTypeWiseSubject.Rows.Count);
                                                            }
                                                        }
                                                        double subjectWiseTotal = 0;
                                                        if (semester.Trim().ToLower() == "2")
                                                        {
                                                            cummulativeSubjectTotal += currentTermSubjectTotal;
                                                            subjectWiseTotal = Math.Round((cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1)), 2, MidpointRounding.AwayFromZero);
                                                            //subjectWiseTotal =(cummulativeSubjectTotal / (dicTermWiseTotal.Count + 1));
                                                        }
                                                        else
                                                        {
                                                            double averageTotal = 0;
                                                            averageTotal = Math.Round((currentTermSubjectTotal / 100) * 50, 2, MidpointRounding.AwayFromZero);
                                                            //averageTotal = (currentTermSubjectTotal / 100) * 50;
                                                            cummulativeSubjectTotal += averageTotal;
                                                            cummulativeSubjectTotal = Math.Round((cummulativeSubjectTotal), 2, MidpointRounding.AwayFromZero);
                                                            subjectWiseTotal = cummulativeSubjectTotal;
                                                        }
                                                        string displayTotal = subjectWiseTotal.ToString();

                                                        overAllSubjectWeightage += subjectWiseTotal;

                                                        //tblStudentMarks.Cell(subjectRow, testColumn + 1).SetFont(fontReportContentBold);
                                                        //tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContent((isSubjectTypeAbsent || isPrevSubjectTypeAbsent) ? "" : (subjectWiseTotal > 0) ? displayTotal : "--");
                                                        tblStudentMarks.Cell(subjectRow, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                        if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)
                                                        {
                                                            tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContent((isAbsent || isPrevAbsent) ? "--" : (overAllSubjectWeightage > 0) ? overAllSubjectWeightage.ToString() : "--");
                                                            tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetFont(fontReportContentBold);
                                                            tblStudentMarks.Cell(subjectRow + 1, testColumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        if (subjectRow == dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count)  //mmm
                                        {
                                            int tempcolumnvalue = testColumn;
                                            if (studentStandard.Trim().ToLower() == "x") //modified
                                            {
                                                tempcolumnvalue -= 1;
                                            }
                                            tblStudentMarks.Cell(subjectRow + 1, 0).SetContent("Grand Total");
                                            tblStudentMarks.Cell(subjectRow + 1, 0).SetFont(fontReportContentBold);
                                            //tblStudentMarks.Cell(subjectRow + 2, 0).SetContent("Average");
                                            //tblStudentMarks.Cell(subjectRow + 2, 0).SetFont(fontReportContentBold);
                                            tblStudentMarks.Cell(subjectRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //tblStudentMarks.Cell(subjectRow + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            double singleSubjectSum = (dicSingleSubjectTestWiseTotal.ContainsKey(studTestNo.Trim())) ? dicSingleSubjectTestWiseTotal[studTestNo.Trim()] : 0;
                                            tblStudentMarks.Cell(subjectRow + 1, testColumn).SetFont(fontReportContentBold);
                                            if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                            {
                                                //tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum));
                                                double average = Math.Round((dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum) / (dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count - dtCurrentSingleSubject.Rows.Count), 2, MidpointRounding.AwayFromZero);
                                                //double average = (dicTestWiseTotal[studTestNo.Trim()] + singleSubjectSum) / (dtCurrentSemesterSubject.Rows.Count + dtCurrentSingleSubjectType.Rows.Count - dtCurrentSingleSubject.Rows.Count);
                                                //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent(average);
                                            }
                                            else
                                            {
                                                //tblStudentMarks.Cell(subjectRow + 1, testColumn).SetContent((singleSubjectSum > 0) ? singleSubjectSum.ToString() : "--");
                                                //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContent("--");
                                            }
                                            tblStudentMarks.Cell(subjectRow + 1, tempcolumnvalue - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //tblStudentMarks.Cell(subjectRow + 2, testColumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            if (testColumn == dtDistinctTestName.Rows.Count - dtOtherSubjects.Rows.Count)
                                            {
                                                double sum = dicCommonSubjectWiseTotal.Sum(x => Math.Round((x.Value), 2, MidpointRounding.AwayFromZero)) + dicSingleSubjectWiseTotal.Sum(x => Math.Round(x.Value, 2, MidpointRounding.AwayFromZero));
                                                if (studentStandard.Trim().ToLower() != "x") //modified
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvalue - 1).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvalue - 1).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, tempcolumnvalue - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else
                                                {
                                                    tblStudentMarks.Cell(subjectRow + 1, 1).SetContent((isAbsent) ? "--" : (sum > 0) ? sum.ToString() : "--");
                                                    tblStudentMarks.Cell(subjectRow + 1, 1).SetFont(fontReportContentBold);
                                                    tblStudentMarks.Cell(subjectRow + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                            }
                                        }
                                        if (studTestName.Trim().ToLower() != "others")
                                            testColumn++;
                                    }
                                    //subjectRow++;  
                                }
                            }

                            tblPage = tblStudentMarks.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, (pdfDocumentReport.PageWidth / 2), 400));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;

                            #endregion

                            #region Activity Details

                            double activityHeight = 0;
                            if (hasActivity)
                            {
                                if (dtStudActivityMarks.Rows.Count > 0)
                                {
                                    DataTable dtSubPart = new DataTable();
                                    int partTotalRows = dtStudActivityMarks.Rows.Count + dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle").Rows.Count + 1;
                                    tblStudentActivity = pdfDocumentReport.NewTable(fontReportContent, partTotalRows, 2, 3);
                                    tblStudentActivity.VisibleHeaders = false;
                                    tblStudentActivity.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tblStudentActivity.SetColumnsWidth(new int[] { 250, 65 });

                                    tblStudentActivity.Cell(0, 0).SetContent("Skills");
                                    tblStudentActivity.Cell(0, 0).SetFont(fontReportContentBold);
                                    tblStudentActivity.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    tblStudentActivity.Cell(0, 1).SetContent("Grade");
                                    tblStudentActivity.Cell(0, 1).SetFont(fontReportContentBold);
                                    tblStudentActivity.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    int activityStartRow = 0;
                                    for (int part = 0; part < totalParts; part++)
                                    {
                                        dtSubPart = new DataTable();
                                        dtStudActivityMarks.DefaultView.RowFilter = "Part_No='" + (part + 2) + "'";
                                        dtSubPart = dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle");
                                        foreach (DataRow drActivity in dtSubPart.Rows)
                                        {
                                            activityStartRow++;
                                            string subTitleNo = Convert.ToString(drActivity["Title_Name"]).Trim();
                                            string subTitleName = string.Empty;
                                            GetSubTitleName(subTitleNo, ref subTitleName);
                                            DataTable dtActivity = new DataTable();
                                            dtStudActivityMarks.DefaultView.RowFilter = "Title_Name='" + subTitleNo + "' ";
                                            dtActivity = dtStudActivityMarks.DefaultView.ToTable();
                                            tblStudentActivity.Cell(activityStartRow, 0).SetContent(subTitleName);
                                            tblStudentActivity.Cell(activityStartRow, 0).SetFont(fontReportContentBold);
                                            tblStudentActivity.Cell(activityStartRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in tblStudentActivity.CellRange(activityStartRow, 0, activityStartRow, 0).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }

                                            foreach (DataRow drActivityMarks in dtActivity.Rows)
                                            {
                                                activityStartRow++;

                                                string activityName = Convert.ToString(drActivityMarks["TextVal"]).Trim();
                                                string grade = Convert.ToString(drActivityMarks["Grade"]).Trim();

                                                tblStudentActivity.Cell(activityStartRow, 0).SetContent(activityName);
                                                tblStudentActivity.Cell(activityStartRow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tblStudentActivity.Cell(activityStartRow, 1).SetContent(grade);
                                                tblStudentActivity.Cell(activityStartRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                    }

                                    tblPage = tblStudentActivity.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, (pdfDocumentReport.PageWidth / 2) + 30, posY, (pdfDocumentReport.PageWidth / 2) - 50, 400));
                                    pdfReportPage.Add(tblPage);
                                    activityHeight = tblPage.Area.Height;
                                }
                            }
                            if (activityHeight <= height)
                                posY += int.Parse(Convert.ToString(height));
                            else
                                posY += int.Parse(Convert.ToString(activityHeight));

                            #endregion

                            #region Remark Details

                            posY += 5;
                            //pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - (2 * 25), 20), ContentAlignment.MiddleLeft, "Remarks\t:\t----------------------------------------------------------------------------------------------------------------------------------------------------");
                            //pdfReportPage.Add(pdfTA);

                            //posY += 26;
                            //pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 25, posY, pdfDocumentReport.PageWidth - (2 * 25), 25), ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            //pdfReportPage.Add(pdfTA);
                            //posY += 10;

                            int newxposition = posX + 10;
                            pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, newxposition, posY + 1, 50, 20), ContentAlignment.TopLeft, "Remarks :");
                            pdfReportPage.Add(pdfTA);

                            PdfTable pdftblRemarks = pdfDocumentReport.NewTable(fontReportContent, 1, 1, 1);
                            pdftblRemarks.VisibleHeaders = false;
                            pdftblRemarks.SetBorders(Color.Black, 1, BorderType.None);
                            pdftblRemarks.SetColumnsWidth(new int[] { 180 });


                            int len = studentRemarks.Length;
                            string displayRemarks = studentRemarks;
                            string displayRemarks1 = string.Empty;
                            //if (len > 68)
                            //{
                            //    displayRemarks = studentRemarks.Substring(0, 68);
                            //    displayRemarks1 = studentRemarks.Substring(68, ((len - 68) > 75) ? 75 : (len - 68));
                            //}
                            //pdftblRemarks.Cell(0, 0).SetContent("Remarks\t:");
                            //pdftblRemarks.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftblRemarks.Cell(0, 0).SetContent(displayRemarks);
                            pdftblRemarks.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //pdftblRemarks.Cell(1, 0).SetContent(displayRemarks1);
                            //pdftblRemarks.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //foreach (PdfCell pc in pdftblRemarks.CellRange(0, 1, 0, 1).Cells)
                            //{
                            //    pc.RowSpan = 2;
                            //}
                            newxposition = posX + 60;
                            tblPage = pdftblRemarks.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, newxposition, posY, pdfDocumentReport.PageWidth - (2 * posX + newxposition), 180));
                            pdfReportPage.Add(tblPage);
                            //pdfLine = tblPage.CellArea(0, 1).LowerBound(Color.Black, 1);
                            //pdfReportPage.Add(pdfLine);
                            //pdfLine = tblPage.CellArea(1, 0).LowerBound(Color.Black, 1);
                            //pdfReportPage.Add(pdfLine);
                            //pdfLine = tblPage.Area.LowerBound(Color.Black, 1);
                            //pdfReportPage.Add(pdfLine);
                            height = tblPage.Area.Height;
                            posY += int.Parse(Convert.ToString(height));

                            #endregion

                            #region Performance Analysis

                            Dictionary<string, string> addedcolumn = new Dictionary<string, string>();

                            int differneceCount = 0;
                            if (dtPreviousStudentMarks.Rows.Count > 0)
                            {
                                DataTable dtTerm = new DataTable();
                                dtPreviousStudentMarks.DefaultView.RowFilter = "";
                                dtTerm = dtPreviousStudentMarks.DefaultView.ToTable(true, "semester");

                                string conductedTestNames = string.Empty;
                                List<string> lstTestName = dtDistinctCommonTestName.AsEnumerable().Select(r => r.Field<string>("criteria")).ToList();
                                conductedTestNames = string.Join("','", lstTestName.ToArray());
                                foreach (DataRow row in dtPreviousSemesterSubject.Rows)
                                {
                                    if (Convert.ToString(row["acronym"]).ToLower().Trim() == "eng")
                                    {
                                        differneceCount++;
                                    }
                                }
                                int totalPerfColumns = dtPreviousSemesterSubject.Rows.Count + 2;
                                int totalPerfRows = dtTerm.Rows.Count + 1;
                                if (dtTerm.Rows.Count > 0)
                                {
                                    posY += 10;
                                    pdfTA = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(pdfDocumentReport, 0, posY, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "PERFORMANCE ANALYSIS");
                                    pdfReportPage.Add(pdfTA);
                                    posY += 23;
                                    if (differneceCount == 0)
                                        tblStudentPerformanceAnalysis = pdfDocumentReport.NewTable(fontReportFooter, totalPerfRows, totalPerfColumns, 5);
                                    else
                                        tblStudentPerformanceAnalysis = pdfDocumentReport.NewTable(fontReportFooter, totalPerfRows, totalPerfColumns - 1, 5);

                                    tblStudentPerformanceAnalysis.VisibleHeaders = false;
                                    tblStudentPerformanceAnalysis.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetContent("Test");
                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetFont(fontReportFooter);
                                    tblStudentPerformanceAnalysis.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetContent("Max");
                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetFont(fontReportFooter);
                                    tblStudentPerformanceAnalysis.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    int termRow = 0;
                                    int avgCount = 0;
                                    foreach (DataRow drTerm in dtTerm.Rows)
                                    {
                                        Double SubjectTypeMark = 0;
                                        string term = Convert.ToString(drTerm["semester"]).Trim();
                                        double maxTestMark = 0;
                                        dicSubjectWiseTotal.Clear();
                                        dicTestWiseTotal.Clear();
                                        termRow++;
                                        int subjectCol = 1;
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetContent("Term - " + ToRoman(term));
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetFont(fontReportFooter);
                                        tblStudentPerformanceAnalysis.Cell(termRow, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        //bool isPrevTAbsent = false;
                                        foreach (DataRow drSubject in dtPreviousSemesterSubject.Rows)
                                        {

                                            string subjectTCode = Convert.ToString(drSubject["subject_code"]).Trim();
                                            string subjectTName = Convert.ToString(drSubject["subject_name"]).Trim();
                                            string subjectTAcronymn = Convert.ToString(drSubject["acronym"]).Trim();

                                            string subjectTypeno = Convert.ToString(drSubject["subType_no"]).Trim();
                                            string isSingleSubject = Convert.ToString(drSubject["isSingleSubject"]).Trim();
                                            DataView dvSingleSubject = new DataView();
                                            if (!addedcolumn.ContainsKey(subjectTAcronymn))
                                            {
                                                //if (subjectTAcronymn.ToLower().Trim() == "eng")
                                                //{
                                                //    dtPreviousStudentMarks.DefaultView.RowFilter = "acronym='" + subjectTAcronymn + "' and semester='" + term + "' and  marks_obtained<0";
                                                //    dvSingleSubject = dtPreviousStudentMarks.DefaultView;
                                                //}
                                                addedcolumn.Add(subjectTAcronymn, subjectTName);
                                                double maxSubjecTestMark = 0;
                                                int testColumn = 1;

                                                maxTestMark = 0;

                                                bool isPrevTAbsent = false;
                                                dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and semester='" + term + "' and  marks_obtained<0";
                                                DataView dvPrevSubject = dtPreviousStudentMarks.DefaultView;
                                                if (dvPrevSubject.Count > 0)
                                                    isPrevTAbsent = true;

                                                //if (isSingleSubject.ToLower() == "false")
                                                //{
                                                subjectCol++;
                                                tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetContent(subjectTAcronymn);
                                                tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetFont(fontReportFooter);
                                                tblStudentPerformanceAnalysis.Cell(0, subjectCol).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                dsStudMarks.Tables[1].DefaultView.RowFilter = " Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester ='" + term + "' and sections='" + section + "'";
                                                dsStudMarks.Tables[1].DefaultView.Sort = "Criteria_no";
                                                dtDistinctTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria", "Criteria_no", "Convert_Minmark", "Convert_Maxmark");
                                                dtDistinctCommonTestName = dsStudMarks.Tables[1].DefaultView.ToTable(true, "criteria");

                                                foreach (DataRow dtTest in dtDistinctTestName.Rows)
                                                {
                                                    string subjectMaxMark = string.Empty;
                                                    string subjectminMark = string.Empty;
                                                    string studTestNo = Convert.ToString(dtTest["Criteria_no"]).Trim();
                                                    string studTestName = Convert.ToString(dtTest["criteria"]).Trim();
                                                    string convertMin = Convert.ToString(dtTest["Convert_Minmark"]).Trim();
                                                    string convertMax = Convert.ToString(dtTest["Convert_Maxmark"]).Trim();
                                                    double minimumTestMark = 0;
                                                    double.TryParse(convertMin, out minimumTestMark);
                                                    double maximumTestMark = 0;
                                                    double.TryParse(convertMax, out maximumTestMark);
                                                    maxTestMark += maximumTestMark;
                                                    DataView dvStudMarks = new DataView();
                                                    if (subjectTAcronymn.ToLower().Trim() == "eng")
                                                    {
                                                        dtPreviousStudentMarks.DefaultView.RowFilter = "acronym='" + subjectTAcronymn + "' and semester='" + term + "' and criteria='" + studTestName + "'";
                                                        dvStudMarks = dtPreviousStudentMarks.DefaultView;
                                                    }
                                                    else
                                                    {
                                                        dtPreviousStudentMarks.DefaultView.RowFilter = "subject_code='" + subjectTCode + "' and semester='" + term + "' and criteria='" + studTestName + "'";
                                                        dvStudMarks = dtPreviousStudentMarks.DefaultView;
                                                    }
                                                    List<int> listMaxMarks = new List<int>();
                                                    //foreach (DataRow row in dtPreviousStudentMarks.Rows)
                                                    //{
                                                    //    listMaxMarks.Add(Convert.ToInt32(Convert.ToString(row["marks_obtained"])));
                                                    //}
                                                    tblStudentPerformanceAnalysis.Cell(termRow, 1).SetContent("100");
                                                    tblStudentPerformanceAnalysis.Cell(termRow, 1).SetFont(fontReportFooter);
                                                    tblStudentPerformanceAnalysis.Cell(termRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    if (dvStudMarks.Count > 0)
                                                    {
                                                        foreach (DataRow rows in dvStudMarks.ToTable().Rows)
                                                        {
                                                            subjectMaxMark = Convert.ToString(rows["Conducted_max"]).Trim();
                                                            subjectminMark = Convert.ToString(rows["Conduct_Minmark"]).Trim();
                                                            convertMin = Convert.ToString(rows["Convert_Minmark"]).Trim();
                                                            convertMax = Convert.ToString(rows["Convert_Maxmark"]).Trim();
                                                            string subjectMark = Convert.ToString(rows["marks_obtained"]).Trim();

                                                            double testSubMarks = 0;
                                                            double.TryParse(subjectMark, out testSubMarks);

                                                            double testMinMark = 0;
                                                            double.TryParse(convertMin, out testMinMark);

                                                            double testMaxMark = 0;
                                                            double.TryParse(convertMax, out testMaxMark);
                                                            maxSubjecTestMark += testMaxMark;

                                                            double testSubMinMarks = 0;
                                                            double.TryParse(subjectminMark, out testSubMinMarks);

                                                            double testSubMaxMarks = 0;
                                                            double.TryParse(subjectMaxMark, out testSubMaxMarks);

                                                            double convertTestMark = testSubMarks;
                                                            double outOff100 = testSubMarks;
                                                            if (testSubMarks >= 0 && !string.IsNullOrEmpty(subjectMark))
                                                            {
                                                                if (testSubMaxMarks > 0 && testMaxMark > 0)
                                                                    convertTestMark = (testSubMarks / testSubMaxMarks) * testMaxMark;
                                                                convertTestMark = Math.Round(convertTestMark, 2, MidpointRounding.AwayFromZero);
                                                                if (testSubMaxMarks > 0 && testSubMarks > 0)
                                                                    outOff100 = (testSubMarks / testSubMaxMarks) * 100;
                                                                if (dicTestWiseTotal.ContainsKey(studTestNo.Trim()))
                                                                    dicTestWiseTotal[studTestNo.Trim()] += convertTestMark;
                                                                else
                                                                    dicTestWiseTotal.Add(studTestNo.Trim(), convertTestMark);
                                                                if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                                    dicSubjectWiseTotal[subjectTCode.Trim().ToLower()] += convertTestMark;
                                                                else
                                                                    dicSubjectWiseTotal.Add(subjectTCode.Trim().ToLower(), convertTestMark);
                                                            }
                                                        }
                                                    }
                                                    if (testColumn == dtDistinctTestName.Rows.Count)
                                                    {
                                                        if (dicSubjectWiseTotal.ContainsKey(subjectTCode.Trim().ToLower()))
                                                        {
                                                            if (subjectTAcronymn.ToLower().Trim() == "eng")
                                                            {
                                                                double subjectWiseTotal = 0;
                                                                subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]), 2, MidpointRounding.AwayFromZero);
                                                                //subjectWiseTotal = (dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]);
                                                                subjectWiseTotal = subjectWiseTotal / 2;
                                                                subjectWiseTotal = Math.Round(subjectWiseTotal, 2, MidpointRounding.AwayFromZero);
                                                                tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContent((isPrevTAbsent) ? "--" : subjectWiseTotal.ToString());
                                                            }
                                                            else
                                                            {
                                                                double subjectWiseTotal = 0;
                                                                subjectWiseTotal = Math.Round((dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]), 2, MidpointRounding.AwayFromZero);
                                                                //subjectWiseTotal = (dicSubjectWiseTotal[subjectTCode.Trim().ToLower()]);
                                                                tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContent((isPrevTAbsent) ? "--" : subjectWiseTotal.ToString());
                                                            }
                                                        }
                                                        else
                                                            tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContent((isPrevTAbsent) ? "--" : "--");
                                                        tblStudentPerformanceAnalysis.Cell(termRow, subjectCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    }
                                                    testColumn++;
                                                }

                                            }
                                        }
                                        addedcolumn.Clear();
                                    }

                                    tblPage = tblStudentPerformanceAnalysis.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, posY, (pdfDocumentReport.PageWidth) - 50, 400));
                                    pdfReportPage.Add(tblPage);
                                    height = tblPage.Area.Height;
                                    posY += int.Parse(Convert.ToString(height));
                                }
                            }

                            #endregion

                            #region Signature

                            tblSign = pdfDocumentReport.NewTable(fontReportFooter, 1, 3, 5);
                            tblSign.VisibleHeaders = false;
                            tblSign.SetBorders(Color.Black, 1, BorderType.None);
                            tblSign.SetColumnsWidth(new int[] { 200, 200, 200 });
                            tblSign.Cell(0, 0).SetContent("Signature of Teacher");
                            tblSign.Cell(0, 0).SetFont(fontReportFooter);
                            tblSign.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblSign.Cell(0, 1).SetContent("Signature of Principal");
                            tblSign.Cell(0, 1).SetFont(fontReportFooter);
                            tblSign.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblSign.Cell(0, 2).SetContent("Signature of Parent");
                            tblSign.Cell(0, 2).SetFont(fontReportFooter);
                            tblSign.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblPage = tblSign.CreateTablePage(new Gios.Pdf.PdfArea(pdfDocumentReport, 25, pdfDocumentReport.PageHeight - 80, (pdfDocumentReport.PageWidth) - 50, 50));
                            pdfReportPage.Add(tblPage);
                            height = tblPage.Area.Height;

                            pdfLine = new PdfLine(pdfDocumentReport, new PointF(35, (float)pdfDocumentReport.PageHeight - 50), new PointF((float)pdfDocumentReport.PageWidth - 35, (float)pdfDocumentReport.PageHeight - 50), Color.Black, 1);
                            pdfReportPage.Add(pdfLine);

                            pdfTA = new PdfTextArea(fontReportFooter, Color.Black, new PdfArea(pdfDocumentReport, 0, pdfDocumentReport.PageHeight - 50, pdfDocumentReport.PageWidth, 20), ContentAlignment.MiddleCenter, "The report card signed by the Parents or Guardian should be returned to the Class Teacher, within three days of issue of report card.");
                            pdfReportPage.Add(pdfTA);

                            #endregion

                            if (isReportSave)
                                pdfReportPage.SaveToDocument();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select Atleast Any one Student";
                divPopAlert.Visible = true;
                return;
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "PerformanceReportCardICSE_IX_To_X" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    pdfDocumentReport.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblAlertMsg.Text = "There is No Report Card Generated";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "Performance Report Card IX - X");
        }
    }

    private void GetSubTitleName(string titleName, ref string subtitlename)
    {
        try
        {
            subtitlename = string.Empty;
            if (!string.IsNullOrEmpty(titleName.Trim()))
            {
                qry = "select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Title_Name='" + titleName.Trim() + "'";
                subtitlename = dirAcc.selectScalarString(qry);
            }
            else
            {
                subtitlename = string.Empty;
            }
        }
        catch (Exception ex)
        {
            subtitlename = string.Empty;
        }
    }

    public string ToRoman(string part)
    {
        string roman = string.Empty;
        try
        {
            switch (part)
            {
                case "1":
                    roman = "I";
                    break;

                case "2":
                    roman = "II";
                    break;
                case "3":
                    roman = "III";
                    break;
                case "4":
                    roman = "IV";
                    break;
                case "5":
                    roman = "V";
                    break;
                case "6":
                    roman = "VI";
                    break;
                case "7":
                    roman = "VII";
                    break;
                case "8":
                    roman = "VIII";
                    break;
                case "9":
                    roman = "IX";
                    break;
                case "10":
                    roman = "X";
                    break;
                case "11":
                    roman = "XI";
                    break;
                case "12":
                    roman = "XII";
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        return roman;
    }


    private string BindPreviousTestNameNEW()
    {
        string testname = string.Empty;
        try
        {
            int SemsterVal = 0;
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
                Int32.TryParse(semester, out SemsterVal);
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
            for (int i = 1; i <= SemsterVal; i++)
            {
                if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
                {
                    dtCommon.Clear();
                    dicQueryParameter.Clear();
                    dicQueryParameter.Add("batchYear", batchYear);
                    dicQueryParameter.Add("degreeCode", degreeCode);
                    dicQueryParameter.Add("semester", Convert.ToString(i));
                    dicQueryParameter.Add("section", section);
                    dtCommon = storeAcc.selectDataTable("uspGetPreviousTestDetails", dicQueryParameter);
                }
                if (dtCommon.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCommon.Rows)
                    {
                        if (string.IsNullOrEmpty(testname))
                            testname = "'" + Convert.ToString(row["criteria"]) + "'";
                        else
                            testname += ",'" + Convert.ToString(row["criteria"]) + "'";
                    }
                }
            }

        }
        catch
        {
        }
        return testname;
    }


    #endregion

}