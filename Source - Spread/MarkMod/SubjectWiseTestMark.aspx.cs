using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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
    int selectedCount = 0;
    Institution institute;

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
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : string.Empty);
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
    //        da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            //qrySemester = " and r.current_semester in(" + semester + ")";
            //qrySemester1 = " and srh.semester in(" + semester + ")";
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
        if (!string.IsNullOrEmpty(qryCollegeCode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryDegreeCode1) && !string.IsNullOrEmpty(qryBatchYear1))   //&& !string.IsNullOrEmpty(qrySemester)
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
                qry = "select distinct s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " union select subject_code=STUFF((select '$mr$'+s.subject_code  from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " for XML PATH('')),1,4,''),ss.subject_type as subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " group by ss.subject_type order by subject_code";
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
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
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
                FpSpread1.Sheets[0].Columns[0].Width = 38;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].Columns[1].Width = 150;
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
                FpSpread1.Sheets[0].Columns[4].Width = 130;
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
                FpSpread1.Sheets[0].Columns[6].Width = 250;
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

    #endregion

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

    #endregion

    #region Button Click Event

    protected void btnGetMarks_Click(object sender, EventArgs e)
    {
        try
        {
            string abnotappno = string.Empty;
            bool abflagmulsub = false;
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
            DataSet dsSubSubjectMarkList = new DataSet();
            DataTable dtStudentMarks = new DataTable();
            DataTable dtSubSubjectMarkList = new DataTable();
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
                    //removed by prabha on feb 16 2018
                    //qrySemester = " and r.current_semester in(" + semester + ")";
                    //qrySemester1 = " and srh.semester in(" + semester + ")";
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
                    qrySubjectCode = " and s.subject_code in(" + subjectCode + ")";
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
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo) && !string.IsNullOrEmpty(qrySubjectCode) && !string.IsNullOrEmpty(subjectCode))
            {
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
                string studentmarkqry = "SELECT Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Roll_No),''))) end Roll_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.college_code),''))) end college_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Reg_No),''))) end Reg_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Batch_Year),''))) end Batch_Year,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.degree_code),''))) end degree_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Current_Semester),''))) end semester,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Sections),''))) end ClassSection,LTRIM(RTRIM(ISNULL(Convert(varchar(500),e.sections),''))) as ExamSection,r.Stud_Name,r.Stud_Type,r.Roll_Admit,ISNULL(r.serialno,'0') as serialno,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,a.app_formno as ApplicationNo,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'' and LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'01/01/1900' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.admissionDate,103),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Adm_Date,103),''))) end AdmissionDate,case when a.sex='0' then 'Male' when a.sex='1' then 'Female' else 'Transgender' end as Gender,ss.subject_type,ss.subType_no,s.subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,CAST(ISNULL(e.min_mark,'0') as float) as ConductedMinMark,CAST(ISNULL(e.max_mark,'0') as float) as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as RetestMark,case when (ISNULL(re.marks_obtained,'0')>='0' and ISNULL(re.marks_obtained,'0')>=ISNULL(e.min_mark,'0')) then 'Pass' when ISNULL(re.marks_obtained,'0')='-1' then 'AAA' else 'Fail' end as Result,CAST(case when ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'')<>'' and ISNULL(re.marks_obtained,'0')>=0 and ISNULL(CONVERT(VARCHAR(100),e.max_mark),'')<>'' and ISNULL(e.max_mark,'0')>0 then ROUND(ISNULL(re.marks_obtained,'0')/ ISNULL(e.max_mark,'0') * " + ((convertedMark > 0) ? convertedMark.ToString() : "100") + ", " + ((chkRoundOffMarks.Checked) ? "0" : "1") + ")  else ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') end as float) OutOffMarks,ISNULL(ss.isSingleSubject,'0') as Single FROM CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s,applyn a,Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where ss.syll_code=s.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and r.App_No=a.app_no and s.subject_no=e.subject_no and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qrytestNo + qrySubjectCode + " and isnull(srh.isLatest,'1')='1'";
                dtStudentMarks = dirAcc.selectDataTable(studentmarkqry);//and srh.App_no='" + studentAppNo + "'
                qry = "select distinct ISNULL(e.max_mark,'0') as max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year=e.batch_year and sm.Batch_Year in(" + batchYear + ") and sm.degree_code in(" + degreeCode + ") and sm.semester in(" + semester + ")" + qrySection + qrytestNo + qrySubjectCode + "";
                string maximumTestMark = dirAcc.selectScalarString(qry);
                double.TryParse(maximumTestMark, out maximumTestMarks);
                maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;
                qry = "select sm.Batch_Year,sm.degree_code,sm.semester,LTRIM(RTRIM(ISNULL(ss.Sections,''))) as Sections,s.subject_no,s.subject_code,s.acronym,ss.staff_code,sfm.staff_name from staff_selector ss,Syllabus_master sm,subject s,staffmaster sfm where s.syll_code=sm.syll_code and s.subject_no=ss.subject_no and ss.batch_year=sm.Batch_Year and sfm.staff_code=ss.staff_code and sm.Batch_year='" + batchYear + "' and sm.semester='" + semester + "' and sm.degree_code='" + degreeCode + "' " + qrySection1 + qrySubjectCode;
                dtStaffDetails = dirAcc.selectDataTable(qry);
                qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' "; //order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc
                dtGradeDetails = dirAcc.selectDataTable(qry);

                string subsubjectMarklistqry = "select ss.subType_no,s.subjectId,s.subSubjectName,sm.appno,e.criteria_no,sm.testMark,sm.ReTestMark,e.exam_code,su.subject_no from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e,syllabus_master sy,subject su,sub_sem ss where s.subjectId=sm.subjectId and s.examCode=e.exam_code and su.syll_code=sy.syll_code and ss.syll_code=su.syll_code and sy.syll_code=ss.syll_code and e.subject_no=su.subject_no and su.subType_no=ss.subType_no and  e.criteria_no='" + testNo + "' " + qrySection + "  order by s.subjectId";
                dsSubSubjectMarkList = dirAcc.selectDataSet(subsubjectMarklistqry);


                string qry2 = "select ss.subType_no,s.subjectId,s.subSubjectName,su.subject_no from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e,syllabus_master sy,subject su,sub_sem ss where s.subjectId=sm.subjectId and s.examCode=e.exam_code and su.syll_code=sy.syll_code and ss.syll_code=su.syll_code and sy.syll_code=ss.syll_code and e.subject_no=su.subject_no and su.subType_no=ss.subType_no and  e.criteria_no='" + testNo + "' " + qrySection + "  order by s.subjectId";
                dtSubSubjectMarkList = dirAcc.selectDataTable(qry2);

                //dsSubSubjectMarkList = da.select_method_wo_parameter("select s.subjectId, s.subSubjectName,sm.appno,sm.testMark,sm.ReTestMark,e.exam_code,subject_no from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and  criteria_no='" + testNo + "' " + qrySection + "  order by s.subjectId", "text");

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
            //Raj modeified 18-9-2017
            //---------------------
            if (dtStudentMarks.Rows.Count > 0)
            {
                Dictionary<string, double> dicSubjectWiseLeastMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseHieghestMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseTotalMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAverageMark = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAppearedCount = new Dictionary<string, double>();
                Dictionary<string, double> dicSubjectWiseAbsentCount = new Dictionary<string, double>();
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
                dtDistinctStudents = dtStudentMarks.DefaultView.ToTable(true, "App_no", "Roll_No", "Reg_No", "Stud_Type", "Roll_Admit", "serialno");
                DataTable dtDistinctSubject = new DataTable();
                DataTable dtDistinctSubjectTypeSingle = new DataTable();
                DataTable dtDistinctSubjectSingle = new DataTable();
                dtStudentMarks.DefaultView.RowFilter = "Single=0";
                dtStudentMarks.DefaultView.Sort = "subject_code";
                dtDistinctSubject = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name", "subjectpriority", "ConductedMaxMark", "Single");
                dtStudentMarks.DefaultView.RowFilter = "Single=1";
                dtStudentMarks.DefaultView.Sort = "subject_code";
                dtDistinctSubjectTypeSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_type", "subType_no", "Single");
                dtStudentMarks.DefaultView.RowFilter = "Single=1";
                dtStudentMarks.DefaultView.Sort = "subject_code";
                dtDistinctSubjectSingle = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_type", "subType_no", "subject_code", "subject_name", "subjectpriority", "Single");
                int spanStartColumn = 0;
                Init_Spread(FpStudentMarkList, ref spanStartColumn, 0);
                Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                int testCount = 0;
                int serialNo = 0;
                object count = 0;

                double subjectHeighestMarks = 0;
                double subjectLeastMarks = 0;
                double subjectAverage = 0;
                double absenteesCount = 0;
                double appearedCount = 0;
                double subjectTotal = 0;
                //modified by prabha jan 11 2018
                //dictionary added 
                Dictionary<string, string> dicabsentees = new Dictionary<string, string>();
                Dictionary<string, string> dicabsenteesnot = new Dictionary<string, string>();
                //count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 ");
                //double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                //count = dtStudentMarks.Compute("MAX(OutOffMarks)", "OutOffMarks>=0 ");
                //double.TryParse(Convert.ToString(count).Trim(), out subjectHeighestMarks);
                //count = dtStudentMarks.Compute("SUM(OutOffMarks)", "OutOffMarks>=0");
                //double.TryParse(Convert.ToString(count).Trim(), out subjectTotal);
                //count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks>=0");
                //double.TryParse(Convert.ToString(count).Trim(), out appearedCount);
                //count = dtStudentMarks.Compute("COUNT(app_no)", "OutOffMarks='-1'");
                //double.TryParse(Convert.ToString(count).Trim(), out absenteesCount);
                //if (subjectTotal > 0 && appearedCount > 0)
                //    subjectAverage = subjectTotal / appearedCount;
                //subjectAverage = Math.Round(subjectAverage, (chkRoundOffMarks.Checked) ? 0 : 2, MidpointRounding.AwayFromZero);
                double apnos2 = 0;
                if (dtDistinctSubject.Rows.Count > 0)
                {
                    foreach (DataRow drSubjects in dtDistinctSubject.Rows)
                    {
                        bool fg = false;
                        object minct = 0;
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
                        string condmaxmark = Convert.ToString(drSubjects["ConductedMaxMark"]).Trim();
                        count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>=0 and subject_no='" + subjectNos + "'");
                        double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                        minct = dtStudentMarks.Compute("MIN(app_no)", "OutOffMarks>=0 and OutOffMarks='" + Convert.ToString(subjectLeastMarks) + "'");
                        double.TryParse(Convert.ToString(minct).Trim(), out apnos2);
                        if (dsSubSubjectMarkList.Tables.Count > 0 && dsSubSubjectMarkList.Tables[0].Rows.Count > 0)
                        {

                            DataView dvSubSubjectMarks = new DataView();
                            dsSubSubjectMarkList.Tables[0].DefaultView.RowFilter = "appno='" + Convert.ToString(apnos2) + "' and subject_no in(" + subjectNos + ") ";
                            dvSubSubjectMarks = dsSubSubjectMarkList.Tables[0].DefaultView;
                            if (dvSubSubjectMarks.ToTable().Rows.Count > 0)
                            {
                                foreach (DataRowView SubjectPraticalDR in dvSubSubjectMarks)
                                {

                                    string TestMark = (Convert.ToString(SubjectPraticalDR["testMark"]).Trim() == "-1") ? getMarkText("-1") : Convert.ToString(SubjectPraticalDR["testMark"]).Trim();
                                    string SubjectID = Convert.ToString(SubjectPraticalDR["subjectId"]).Trim();
                                    if (TestMark == "Ab")
                                    {
                                        fg = true;
                                        goto lbl3;
                                    }

                                }
                            }
                        }

                    lbl3:
                        if (fg == true)
                        {
                            count = 0;
                            count = dtStudentMarks.Compute("MIN(OutOffMarks)", "OutOffMarks>='" + Convert.ToString(subjectLeastMarks) + "' and subject_no='" + subjectNos + "' and app_no<>'" + Convert.ToString(apnos2) + "'");
                            double.TryParse(Convert.ToString(count).Trim(), out subjectLeastMarks);
                        }


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


                        //=============================================================================================================================
                        //Raj modified 18-9-2017   
                        //DataSet dsNew = new DataSet();
                        DataTable DistinctSubject = new DataTable();
                        if (dtSubSubjectMarkList.Rows.Count > 0)
                        {
                            dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no in(" + subjectNos + ")";
                            DistinctSubject = dtSubSubjectMarkList.DefaultView.ToTable(true, "subSubjectName", "subjectId");
                        }


                        //DistinctSubject = dsNew.Tables[0].DefaultView.ToTable(true, "subSubjectName", "subjectId");
                        foreach (DataRow header in DistinctSubject.Rows)
                        {
                            FpStudentMarkList.Sheets[0].ColumnCount++;
                            FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = subjectNames;
                            FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = Convert.ToString(header["subSubjectName"] + "\n");
                            FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(header["subjectId"]);
                            //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount -4 , 0, 4);

                        }
                        //-----------------------------
                        FpStudentMarkList.Sheets[0].ColumnCount += 3;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = subjectNames;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Tag = subjectNos;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Note = "0";
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Marks\n" + condmaxmark;
                        FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - (DistinctSubject.Rows.Count + 3), 1, (DistinctSubject.Rows.Count + 3));
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                        convertMark = txtConvertedMaxMark.Text;
                        double convertedMax = 0;
                        double.TryParse(convertMark.Trim(), out convertedMax);

                        string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = ((chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? true : false);
                        if (string.IsNullOrEmpty(displayOutof100))
                            displayOutof100 = Convert.ToString(condmaxmark);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "Mark\n" + displayOutof100;
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = (chkIncludeGrade.Checked);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Grade";
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);
                        //==========================================================================================================================
                        ////Raj modified 18-9-2017   
                        //   DataSet dsNew = da.select_method_wo_parameter("select s.subjectId, s.subSubjectName,sm.appno,sm.testMark,sm.ReTestMark,e.exam_code from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and  criteria_no='" + testNo + "' and subject_no='" + subjectNos + "' and sections='" + section + "'", "text");
                        //   DataTable DistinctSubject = dsNew.Tables[0].DefaultView.ToTable(true, "subSubjectName", "subjectId");
                        //   foreach (DataRow header in DistinctSubject.Rows)
                        //   {
                        //       FpStudentMarkList.Sheets[0].ColumnCount++;
                        //       FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = subjectNames;
                        //       FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = Convert.ToString(header["subSubjectName"]);
                        //       FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(header["subjectId"]);
                        //       //Hashtable hs = new Hashtable();
                        //       //int col =Convert.ToInt32( FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1]);
                        //   }
                    }
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
                        dtStudentMarks.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "'";
                        dtStudentMarks.DefaultView.Sort = "app_no,subject_code";
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

                        DataTable DistinctSubject = new DataTable();
                        if (dtSubSubjectMarkList.Rows.Count > 0)
                        {
                            dtSubSubjectMarkList.DefaultView.RowFilter = "subType_no in(" + subjectTypeNo + ")";
                            DistinctSubject = dtSubSubjectMarkList.DefaultView.ToTable(true, "subSubjectName", "subjectId");
                        }
                        //DistinctSubject = dsNew.Tables[0].DefaultView.ToTable(true, "subSubjectName", "subjectId");
                        //foreach (DataRow header in DistinctSubject.Rows)
                        //{
                        //    FpStudentMarkList.Sheets[0].ColumnCount++;
                        //    FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = subjectType;
                        //    FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = Convert.ToString(header["subSubjectName"] + "\n");
                        //    FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(header["subjectId"]);
                        //    //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount -4 , 0, 4);

                        //}
                        FpStudentMarkList.Sheets[0].ColumnCount += 3;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 3].Visible = true;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = subjectType;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Tag = subjectNoList;
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Note = "1";
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text = "Marks\n";
                        FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 3, 1, 3);
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Resizable = false;
                        FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - (DistinctSubject.Rows.Count + 3), 1, (DistinctSubject.Rows.Count + 3));
                        convertMark = txtConvertedMaxMark.Text;
                        double convertedMax = 0;
                        double.TryParse(convertMark.Trim(), out convertedMax);
                        string displayOutof100 = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 2].Visible = ((chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? true : false);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 2].Text = "Mark\n" + displayOutof100;
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 2, 2, 1);
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Width = 80;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Locked = true;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Resizable = false;
                        FpStudentMarkList.Sheets[0].Columns[FpStudentMarkList.Sheets[0].ColumnCount - 1].Visible = (chkIncludeGrade.Checked);
                        FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, FpStudentMarkList.Sheets[0].ColumnCount - 1].Text = "Grade";
                        //FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, FpStudentMarkList.Sheets[0].ColumnCount - 1, 2, 1);
                    }
                }
                Dictionary<string, int> dicGradeWiseCount = new Dictionary<string, int>();
                Dictionary<string, ArrayList> dicNewGradeWiseCount = new Dictionary<string, ArrayList>();

                int endColumn = FpStudentMarkList.Sheets[0].ColumnCount - 1;
                if (dtDistinctStudents.Rows.Count > 0)
                {
                    string staffName = string.Empty;
                    foreach (DataRow drStudent in dtDistinctStudents.Rows)
                    {
                        int colval = 0;
                        string subjectCodeVal = string.Empty;
                        string subjectNameVal = string.Empty;
                        string subjectNoVal = string.Empty;
                        string testMark = string.Empty;
                        string testMaxMark = string.Empty;
                        string testMinMark = string.Empty;
                        double testSubMarks = 0;
                        double testMaxMarks = 0;
                        double testMinMarks = 0;
                        string studentAppNos = Convert.ToString(drStudent["App_no"]).Trim();
                        subjectCodeVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Note).Trim();
                        subjectNoVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Tag).Trim();
                        subjectNameVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, FpStudentMarkList.Sheets[0].ColumnCount - 3].Text).Trim();
                        //subjectCodeVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, colval + 7].Note).Trim();
                        //subjectNoVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, colval + 7].Tag).Trim();
                        //subjectNameVal = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, colval + 7].Text).Trim();
                        DataView dvTestMark = new DataView();
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


                        //dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                        //dvTestMark = dtStudentMarks.DefaultView;

                        string displayMark = string.Empty;
                        string displayGrade = string.Empty;
                        bool absentflag = false;
                        bool result = false;
                        int subjectRow = 0;
                        if (dvTestMark.Count > 0)
                        {
                            FpStudentMarkList.Sheets[0].RowCount++;
                            subjectRow = FpStudentMarkList.Sheets[0].RowCount - 1;
                            testMark = (Convert.ToString(dvTestMark[0]["TestMark"]).Trim() == "-1") ? "Ab" : Convert.ToString(dvTestMark[0]["TestMark"]).Trim();


                            testMaxMark = Convert.ToString(dvTestMark[0]["ConductedMaxMark"]).Trim();
                            testMinMark = Convert.ToString(dvTestMark[0]["ConductedMinMark"]).Trim();
                            subjectNameVal = Convert.ToString(dvTestMark[0]["subject_name"]).Trim();
                            subjectCodeVal = Convert.ToString(dvTestMark[0]["subject_code"]).Trim();
                            subjectNoVal = Convert.ToString(dvTestMark[0]["subject_no"]).Trim();
                            string appNo = Convert.ToString(dvTestMark[0]["app_no"]).Trim();
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
                            DataView dvStaff = new DataView();
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
                            bool isSuccess = false;
                            string convertMarkNew = Convert.ToString(dvTestMark[0]["OutOffMarks"]).Trim();
                            if (testMark != "Ab")
                            {
                                isSuccess = double.TryParse(testMark, out testSubMarks);
                                testMark = testSubMarks.ToString();
                                testMark = (isSuccess && chkRoundOffMarks.Checked) ? testSubMarks.ToString() : testMark;
                            }
                            else
                            {

                                testMark = "Ab";
                                testSubMarks = -1.0;
                            }


                            double.TryParse(testMaxMark, out testMaxMarks);
                            double.TryParse(testMinMark, out testMinMarks);
                            double outof100 = 0;
                            double convertedMinMark = 0;
                            double convertedMaxMark = 0;
                            string convertedObtainedMark = testMark;
                            string convertedMinimumMark = string.Empty;
                            string convertedMaximumMark = string.Empty;
                            convertedMinimumMark = testMinMark;
                            convertedMaximumMark = testMaxMark;
                            if (testMark != "Ab")
                            {
                                ConvertedMark(convertMark, ref convertedMaximumMark, ref convertedObtainedMark, ref convertedMinimumMark);
                            }
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
                                if (testMark != "Ab")
                                {
                                    displayMark = getMarkText(testMark);
                                    convertedObtainedMark = displayMark;
                                }
                                else
                                {
                                    displayMark = getMarkText("-1");
                                    convertedObtainedMark = displayMark;
                                }
                            }
                            else if (string.IsNullOrEmpty(testMark))
                            {
                                displayMark = "--";
                                convertedObtainedMark = "--";
                                result = true;
                            }
                            else
                            {
                                if (testSubMarks >= testMinMarks)
                                    result = true;
                                displayMark = testSubMarks.ToString();
                            }
                            if (dvGrade.Count > 0)
                            {
                                displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                if (!string.IsNullOrEmpty(displayGrade))
                                {
                                    if (!dicGradeWiseCount.ContainsKey(displayGrade.Trim().ToLower()))
                                    {
                                        dicGradeWiseCount.Add(displayGrade.Trim().ToLower(), 1);
                                    }
                                    else
                                    {
                                        dicGradeWiseCount[displayGrade.Trim().ToLower()] += 1;
                                    }
                                }
                            }
                            else
                            {
                                displayGrade = "--";
                            }
                            int markCol = 0;
                            serialNo++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(serialNo).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(appNo).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(rollNo).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(regNo).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(admissionNo).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentType).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(gender).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentName).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            //if()
                            if (dsSubSubjectMarkList.Tables.Count > 0 && dsSubSubjectMarkList.Tables[0].Rows.Count > 0 && subjectNoList.Length == 1)
                            {

                                DataView dvSubSubjectMarks = new DataView();
                                dsSubSubjectMarkList.Tables[0].DefaultView.RowFilter = "appno='" + appNo + "' and subject_no in(" + subjectNoVal + ") ";
                                dvSubSubjectMarks = dsSubSubjectMarkList.Tables[0].DefaultView;
                                if (dvSubSubjectMarks.ToTable().Rows.Count > 0)
                                {
                                    foreach (DataRowView SubjectPraticalDR in dvSubSubjectMarks)
                                    {
                                        string examcode = Convert.ToString(SubjectPraticalDR["exam_code"]).Trim();
                                        string TestMark = (Convert.ToString(SubjectPraticalDR["testMark"]).Trim() == "-1") ? getMarkText("-1") : Convert.ToString(SubjectPraticalDR["testMark"]).Trim();
                                        string SubjectID = Convert.ToString(SubjectPraticalDR["subjectId"]).Trim();
                                        markCol++;
                                        string SubjectHeaderCode = Convert.ToString(FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, markCol].Tag);
                                        if (SubjectID == SubjectHeaderCode)
                                        {
                                            endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(TestMark).Trim();
                                            if (TestMark == "Ab")
                                            {
                                                if (!dicabsentees.ContainsKey(studentAppNos))
                                                {
                                                    dicabsentees.Add(studentAppNos, "");
                                                }
                                                absentflag = true;
                                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;

                                            }

                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(SubjectID).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(examcode).Trim();
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                                        }
                                    }
                                }
                                else  //modified on 5/12/2017
                                {
                                    if (testMark == "Ab")
                                    {
                                        if (!dicabsentees.ContainsKey(studentAppNos))
                                        {
                                            dicabsentees.Add(studentAppNos, "");
                                        }
                                        absentflag = true;
                                        //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;
                                    }
                                    //markCol++;
                                    //endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(testMark).Trim();
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;


                                    //markCol++;
                                    //endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(testMark).Trim();
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                                }
                                //if (abflagmulsub == false)
                                //{
                                //    if (string.IsNullOrEmpty(abnotappno))
                                //        abnotappno = "'" + rollNo + "'";
                                //    else
                                //        abnotappno = abnotappno + ",'" + rollNo + "'";
                                //}
                            }
                            else
                            {
                                if (testMark == "Ab")
                                {
                                    if (!dicabsentees.ContainsKey(studentAppNos))
                                    {
                                        dicabsentees.Add(studentAppNos, "");
                                    }
                                    absentflag = true;
                                    //FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;
                                }
                            }

                            if (colval > 0)
                            {

                            }

                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            string display = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && testMaxMarks > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(testMaxMarks).Trim() + ")" : Convert.ToString(testMaxMarks).Trim()) : "";
                            if (string.IsNullOrEmpty(display))
                                display = testMaxMark;
                            FpStudentMarkList.Sheets[0].ColumnHeader.Cells[1, markCol].Text = "Marks\n" + display;
                            if (chkRoundOffMarks.Checked)
                            {
                                if (displayMark.ToUpper() == "AB")
                                {
                                }
                                else
                                {
                                    double testMk = Math.Round(Convert.ToDouble(displayMark), 0, MidpointRounding.AwayFromZero);
                                    displayMark = Convert.ToString(testMk);
                                }
                            }

                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(displayMark).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = (result) ? Color.Black : Color.Red;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;
                            markCol++;

                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(convertedObtainedMark).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = (result) ? Color.Black : Color.Red;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                            if (absentflag)
                            {
                                displayGrade = "--";
                            }

                            markCol++;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(displayGrade).Trim();
                            if (displayGrade == "--")
                            {
                                FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].ForeColor = Color.Red;
                            }
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                            endColumn = FpStudentMarkList.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                            subjectRow++;
                        }


                    }


                    int columnVal = 0;
                    //spanStartColumn = 0;
                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    int startingRows = FpStudentMarkList.Sheets[0].RowCount - 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;

                    // FpStudentMarkList.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "TOTAL NO.OF ANSWER SCRIPTS EVALUATED: " + Convert.ToString(appearedCount).Trim();
                    //FpStudentMarkList.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(appearedCount).Trim();
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;


                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;

                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "SUBJECT AVERAGE: " + Convert.ToString(subjectAverage + "/" + convertedMark).Trim(); ;
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    //FpStudentMarkList.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(subjectAverage + "/" + convertedMark).Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "HIGHEST SCORE: " + Convert.ToString(subjectHeighestMarks + "/" + convertedMark).Trim();
                    //FpStudentMarkList.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(subjectHeighestMarks + "/" + convertedMark).Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "LEAST SCORE: " + Convert.ToString(subjectLeastMarks + "/" + convertedMark).Trim();

                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(subjectLeastMarks + "/" + convertedMark).Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "NO OF ABSENTEES: " + Convert.ToString(absenteesCount).Trim();dicabsentees
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "NO OF ABSENTEES: " + Convert.ToString(dicabsentees.Count).Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(absenteesCount).Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    int notIncludeRowStart = FpStudentMarkList.Sheets[0].RowCount - 1;

                    if (CheckBox1.Checked == false)
                    {
                        FpStudentMarkList.Sheets[0].RowCount += 1;
                        if (dtGeneralGrade.Rows.Count > 0)
                        {
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].Text = "MARKS";
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, 0, 1, 6);
                            int col = 6;
                            if (chkIncludeGrade.Checked)
                            {
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = "GRADE";
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, col, 1, 1);
                                col++;
                            }
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = "NUMBER OF STUDENTS";
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                            FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, col, 1, endColumn + 1 - 6);
                            foreach (DataRow drGrade in dtGeneralGrade.Rows)
                            {
                                FpStudentMarkList.Sheets[0].RowCount++;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(drGrade["Ranges"]).Trim();
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, 0, 1, 6);
                                col = 6;
                                string grade = Convert.ToString(drGrade["Mark_Grade"]).Trim();
                                if (chkIncludeGrade.Checked)
                                {
                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = Convert.ToString(grade).Trim();
                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, col, 1, 1);
                                    col++;
                                }
                                //if part  added by prabha  --- existing code = else part    modified on jan 11 2018
                                if (grade.ToLower().Trim() == "e")
                                {
                                    int failedstudent = 0;
                                    if (dicGradeWiseCount.ContainsKey(grade.Trim().ToLower()))
                                    {
                                        Int32.TryParse(Convert.ToString(dicGradeWiseCount[grade.Trim().ToLower()]), out failedstudent);
                                        if (failedstudent == dicabsentees.Count)
                                        {
                                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = "--";
                                        }
                                        else if (failedstudent > dicabsentees.Count)
                                        {
                                            failedstudent = failedstudent - dicabsentees.Count;
                                            FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = Convert.ToString(failedstudent).Trim();
                                        }
                                    }
                                }
                                else
                                {
                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].Text = (!dicGradeWiseCount.ContainsKey(grade.Trim().ToLower())) ? "--" : Convert.ToString(dicGradeWiseCount[grade.Trim().ToLower()]).Trim();
                                }
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, col, 1, endColumn + 1 - 6);
                            }
                            if (dtStudentMarks.Rows.Count > 0)
                            {
                                string MinMark = Convert.ToString(dtStudentMarks.Rows[0]["TestMinMark"]);
                                dtStudentMarks.DefaultView.RowFilter = "TestMark<'" + MinMark + "'";
                                DataTable testFail = dtStudentMarks.DefaultView.ToTable();
                                FpStudentMarkList.Sheets[0].RowCount++;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].Text = "No.of.Failure";
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, 0, 1, 6);
                                int col1 = 6;
                                if (testFail.Rows.Count > 0)
                                {

                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(testFail.Rows.Count);

                                }
                                else
                                {
                                    FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col1].Text = "--";
                                }
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentMarkList.Sheets[0].Cells[FpStudentMarkList.Sheets[0].RowCount - 1, col1].VerticalAlign = VerticalAlign.Middle;
                                FpStudentMarkList.Sheets[0].AddSpanCell(FpStudentMarkList.Sheets[0].RowCount - 1, col1, 1, endColumn + 1 - 6);
                            }

                        }
                    }
                    int notIncludeRowEND = FpStudentMarkList.Sheets[0].RowCount;
                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;

                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "NAME OF THE SUBJECT TEACHER : " + Convert.ToString(staffName).Trim();

                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString(staffName).Trim();
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;

                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = "NAME OF THE EVALUATOR: ";
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    FpStudentMarkList.Sheets[0].Rows[columnVal].Visible = false;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString("").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = Convert.ToString("SIGNATURE OF THE EVALUATOR").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, 6);
                    FpStudentMarkList.Sheets[0].Rows[columnVal].Visible = false;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString("").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = Convert.ToString("NAME OF THE EVALUATOR:").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Rows[columnVal].Visible = false;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].Text = Convert.ToString("").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;



                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = Convert.ToString("NAME OF THE EVALUATOR:").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 5].Text = Convert.ToString("").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    FpStudentMarkList.Sheets[0].RowCount += 1;
                    columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = Convert.ToString("SIGNATURE OF THE EVALUATOR:").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 5].Text = Convert.ToString("").Trim();
                    FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 0].VerticalAlign = VerticalAlign.Middle;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentMarkList.Sheets[0].Cells[columnVal, 6].VerticalAlign = VerticalAlign.Middle;

                    //FpStudentMarkList.Sheets[0].RowCount += 1;
                    //columnVal = FpStudentMarkList.Sheets[0].RowCount - 1;
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 0].Text = Convert.ToString("CHECKED AND VERIFIED BY SUBJECT TEACHER / CLASS TEACHER").Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 0, 1, FpStudentMarkList.Sheets[0].ColumnCount - 1);
                    //FpStudentMarkList.Sheets[0].Cells[columnVal, 5].Text = Convert.ToString("").Trim();
                    //FpStudentMarkList.Sheets[0].AddSpanCell(columnVal, 6, 1, endColumn + 1 - 6);
                    //int rowCount = 0;
                    //for (int row = startingRows; row < FpStudentMarkList.Sheets[0].RowCount; row++)
                    //{
                    //    bool setBorder = true;
                    //    if (row > notIncludeRowStart - 1 && row < notIncludeRowEND)
                    //    {
                    //        setBorder = false;
                    //        if ((row == notIncludeRowStart))
                    //        {
                    //            FpStudentMarkList.Sheets[0].Rows[row].Border.BorderColorBottom = Color.Black;
                    //            rowCount++;
                    //        }
                    //    }
                    //    else
                    //        rowCount++;
                    //    for (int col = 0; col < FpStudentMarkList.Sheets[0].ColumnCount; col++)
                    //    {
                    //        if (setBorder)
                    //        {
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Border.BorderColorTop = Color.Black;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Border.BorderColorBottom = Color.White;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Border.BorderColorLeft = (col != 0) ? Color.White : Color.Black;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Border.BorderColorRight = (rowCount % 2 != 0) ? ((col != endColumn) ? Color.White : Color.Black) : ((col == 6) ? Color.Black : Color.White);
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].CellType = txtCell;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Font.Name = "Book Antiqua";
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Font.Bold = (col == 0) ? true : false;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].Locked = true;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].HorizontalAlign = (col == 6) ? HorizontalAlign.Left : HorizontalAlign.Left;
                    //            FpStudentMarkList.Sheets[0].Cells[row, col].VerticalAlign = VerticalAlign.Middle;
                    //        }
                    //    }
                    //}
                    divMainContents.Visible = true;
                    FpStudentMarkList.Sheets[0].PageSize = FpStudentMarkList.Sheets[0].RowCount;
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

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

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Subject Wise Test Mark Report";
            string pagename = "SubjectWiseTestMark.aspx";
            string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            //  rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "$ SUBJECT : " + ((ddlSubject.Items.Count > 0) ? Convert.ToString(ddlSubject.SelectedItem.Text).Trim() : "") + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + ((ddlSec.Items.Count == 0) ? "" : (ddlSec.Items.Count > 0 && !string.IsNullOrEmpty(ddlSec.SelectedItem.Text.Trim()) && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? " - " + ddlSec.SelectedItem.Text.Trim() : "") + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " " + lblSem.Text.Trim() + " : " + Convert.ToString(ddlSem.SelectedItem).Trim();

            rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + ((ddlSec.Items.Count == 0) ? "" : (ddlSec.Items.Count > 0 && !string.IsNullOrEmpty(ddlSec.SelectedItem.Text.Trim()) && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? " - " + ddlSec.SelectedItem.Text.Trim() : "");
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

    #endregion

    #region Reusable Methods

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

    #endregion

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
                spPageHeading.InnerHtml = "Subject Wise Test Mark Report";
                Page.Title = "Subject Wise Test Mark Report";
            }
            else
            {
                lblBatch.Text = "Batch";
                spPageHeading.InnerHtml = "Subject Wise Test Mark Report";
                Page.Title = "Subject Wise Test Mark Report";
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

    #endregion

}