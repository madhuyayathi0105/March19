using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Configuration;

public partial class MarkMod_StudentsMarkPrevousHistory : System.Web.UI.Page
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
    ArrayList arrColHdrNames1 = new ArrayList();
    DataTable data = new DataTable();
    DataRow drow;
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
                divStudentDetail.Visible = false;
                divMainContents.Visible = false;
                divPrint1.Visible = false;
                Bindcollege();
                SetStudentWiseSettings();
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

    private void SetStudentWiseSettings()
    {
        try
        {
            ddlSearchBy.Items.Clear();
            ddlSearchBy.Enabled = false;
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                ddlSearchBy.SelectedIndex = 0;
                if (CheckSchoolOrCollege(collegeCode))
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
                lblSearchStudent.Text = ddlSearchBy.SelectedItem.Text;

            }
            else
            {
                if (lblCollege.Text.Trim().ToUpper() == "SCHOOL")
                {
                    lblSearchStudent.Text = "Admission No";
                    ddlSearchBy.Items.Insert(0, new ListItem("Admission No", "1"));
                }
                else
                {
                    lblSearchStudent.Text = "Roll No";
                    ddlSearchBy.Items.Insert(0, new ListItem("Roll No", "3"));
                }
            }
            if (ddlSearchBy.Items.Count <= 1)
            {
                ddlSearchBy.Enabled = false;
            }
            else
            {
                ddlSearchBy.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindPreviousCollege(string appNo, byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedCollege = null)
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
            ddlPrevCollege.Items.Clear();
            ddlPrevCollege.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("appNo", appNo);
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetStudentPreviousColleges", dicQueryParameter);
            if (dtCommon.Rows.Count > 0)
            {
                ddlPrevCollege.DataSource = dtCommon;
                ddlPrevCollege.DataTextField = "collname";
                ddlPrevCollege.DataValueField = "college_code";
                ddlPrevCollege.DataBind();
                ddlPrevCollege.SelectedIndex = 0;
                ddlPrevCollege.Enabled = true;
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

    public void BindRightsBaseBatch(string appNo, byte redoType = 2, string defaultSelectedBatch = null)
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
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
                qryCollegeCode = " and college_code in(" + collegeCode + ")";
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollegeCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights where batch_year<>'' " + qryCollegeCode + qryUserOrGroupCode + " order by batch_year desc";
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
                    qryBatchYear1 = " and srh.BatchYear in('" + batchList + "')";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(appNo))
            {
                qry = "select distinct srh.BatchYear from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.collegeCode=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=srh.collegeCode and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and srh.degreeCode=dg.Degree_Code  and srh.BatchYear<>'0' and srh.BatchYear<>-1 and srh.RedoType='" + redoType + "' and srh.collegeCode in(" + collegeCode + ") and srh.App_No='" + appNo + "' " + qryBatchYear1 + " order by srh.BatchYear desc";
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

    private void BindPreviousDegrees(string appNo, string collegeCode, string batchYear = null, byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedDegree = null)
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
                collegeCode = ((ddlPrevCollege.Items.Count > 0) ? Convert.ToString(ddlPrevCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
            if (string.IsNullOrEmpty(batchYear))
                batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
            ddlDegree.Items.Clear();
            ddlDegree.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("appNo", appNo);
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetStudentPreviousDegrees", dicQueryParameter);
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

    private void BindPreviousDepartment(string appNo, string collegeCode, string courseID, string batchYear = null, byte redoType = 2, byte withOrWithoutRights = 1, string defaultSelectedDegree = null)
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
                collegeCode = ((ddlPrevCollege.Items.Count > 0) ? Convert.ToString(ddlPrevCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
            if (string.IsNullOrEmpty(batchYear))
                batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
            if (string.IsNullOrEmpty(courseID))
                courseID = ((ddlDegree.Items.Count > 0) ? Convert.ToString(ddlDegree.SelectedValue).Trim() : "");

            dicQueryParameter.Add("appNo", appNo);
            dicQueryParameter.Add("userCode", userCode);
            dicQueryParameter.Add("groupCode", groupUserCode);
            dicQueryParameter.Add("singleUser", singleUser);
            dicQueryParameter.Add("withRights", Convert.ToString(withOrWithoutRights).Trim());
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("courseID", courseID);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dtCommon = storeAcc.selectDataTable("uspGetStudentPreviousDepartment", dicQueryParameter);
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

    private void BindPreviousSemesters(string appNo, string collegeCode = null, string batchYear = null, string degreeCode = null, byte redoType = 2, string defaultSelectedDegree = null)
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
                collegeCode = ((ddlPrevCollege.Items.Count > 0) ? Convert.ToString(ddlPrevCollege.SelectedValue).Trim() : ((Session["collegecode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["collegecode"]).Trim())) ? Convert.ToString(Session["collegecode"]).Trim() : "13"));
            if (string.IsNullOrEmpty(batchYear))
                batchYear = ((ddlBatch.Items.Count > 0) ? Convert.ToString(ddlBatch.SelectedValue).Trim() : "");
            if (string.IsNullOrEmpty(degreeCode))
                degreeCode = ((ddlBranch.Items.Count > 0) ? Convert.ToString(ddlBranch.SelectedValue).Trim() : "");
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            dicQueryParameter.Clear();
            dicQueryParameter.Add("appNo", appNo);
            dicQueryParameter.Add("collegeCode", collegeCode);
            dicQueryParameter.Add("batchYear", batchYear);
            dicQueryParameter.Add("redoType", Convert.ToString(redoType).Trim());
            dicQueryParameter.Add("degreeCode", degreeCode);
            dtCommon = storeAcc.selectDataTable("uspGetStudentPreviousSemester", dicQueryParameter);
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

    public void BindRightsBasedSectionDetail(string appNo, string defaultSelectedSections = null)
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
            qryCollegeCode = " and college_code in(" + collegeCode + ")";
            qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
        }
        if (ddlBatch.Items.Count > 0)
        {
            batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and batch_year in(" + batchYear + ")";
                qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
            }
        }
        if (ddlBranch.Items.Count > 0)
        {
            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
            }
        }
        if (ddlSem.Items.Count > 0)
        {
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            qrySemester = " and srh.semester in(" + semester + ")";
        }
        if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryBatchYear))
        {
            qrySection = dirAcc.selectScalarString("select distinct sections from tbl_attendance_rights where batch_year<>'' " + qryUserOrGroupCode + qryCollegeCode + qryBatchYear).Trim();
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
            qrySection = " and isnull(ltrim(rtrim(srh.sections)),'') in(" + sections + ") ";
        }
        else
        {
            qrySection = string.Empty;
        }
        if (!string.IsNullOrEmpty(qryCollegeCode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryDegreeCode1) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryBatchYear1))// && !string.IsNullOrEmpty(qrySection)
        {
            string sqlnew = "select distinct case when isnull(ltrim(rtrim(srh.sections)),'')='' then 'Empty' else isnull(ltrim(rtrim(srh.sections)),'') end sections, isnull(ltrim(rtrim(srh.sections)),'') SecValues from StudentRegisterHistory srh where isnull(ltrim(rtrim(srh.sections)),'')<>'-1' and isnull(ltrim(rtrim(srh.sections)),'')<>'' and RedoType='2' and srh.App_no='" + appNo + "'" + qryCollegeCode1 + qryDegreeCode1 + qryBatchYear1 + qrySection + qrySemester + " order by SecValues";
            ds.Clear();
            ds = dirAcc.selectDataSet(sqlnew);
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

                    qrySection = " and e.sections in('" + section + "')";
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
                ddlTest.Visible = false;

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
                //foreach (ListItem li in cblTest.Items)
                //{
                //    if (li.Selected)
                //    {
                //        if (!string.IsNullOrEmpty(testNo))
                //        {
                //            testNo += ",'" + li.Value + "'";
                //        }
                //        else
                //        {
                //            testNo = "'" + li.Value + "'";
                //        }
                //    }
                //}
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
                dtCommon = dirAcc.selectDataTable("select distinct s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ")" + qrySection + " order by s.subject_code");
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

    public void Init_Spread(int type = 0)
    {
        try
        {
            if (type == 0)
            {
                arrColHdrNames1.Add("S.No");
                arrColHdrNames1.Add("Roll No");
                arrColHdrNames1.Add("Register No");
                arrColHdrNames1.Add("Student Name");
                arrColHdrNames1.Add("Student Type");
                arrColHdrNames1.Add("Gender");

                data.Columns.Add("S.No");
                data.Columns.Add("Roll No");
                data.Columns.Add("Register No");
                data.Columns.Add("Student Name");
                data.Columns.Add("Student Type");
                data.Columns.Add("Gender");


                byte value = 0;

            }
            else
            {


                arrColHdrNames1.Add("SNo");
                arrColHdrNames1.Add("Subject Code");
                arrColHdrNames1.Add("Subject Name");

                data.Columns.Add("SNo");
                data.Columns.Add("Subject Code");
                data.Columns.Add("Subject Name");


            }
            DataRow drHdr1 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames1[grCol];
            data.Rows.Add(drHdr1);
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
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divPrint1.Visible = false;
            txtSearchStudent.Text = string.Empty;
            //BindBatch();
            //BindDegree();
            //BindBranch();
            //BindSem();
            //BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divPrint1.Visible = false;
            txtSearchStudent.Text = string.Empty;
            if (ddlSearchBy.Items.Count > 0)
            {
                lblSearchStudent.Text = ddlSearchBy.SelectedItem.Text;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #region Previous Details

    #region Index Changed Events

    protected void ddlPrevCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
            BindRightsBaseBatch(studentApplicationNo);
            BindPreviousDegrees(studentApplicationNo, "");
            BindPreviousDepartment(studentApplicationNo, "", "");
            BindPreviousSemesters(studentApplicationNo, "", "");
            BindRightsBasedSectionDetail(studentApplicationNo);
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
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
            BindPreviousDegrees(studentApplicationNo, "");
            BindPreviousDepartment(studentApplicationNo, "", "");
            BindPreviousSemesters(studentApplicationNo, "", "");
            BindRightsBasedSectionDetail(studentApplicationNo);
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
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
            BindPreviousDepartment(studentApplicationNo, "", "");
            BindPreviousSemesters(studentApplicationNo, "", "");
            BindRightsBasedSectionDetail(studentApplicationNo);
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
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
            BindPreviousSemesters(studentApplicationNo, "", "");
            BindRightsBasedSectionDetail(studentApplicationNo);
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
            divPrint1.Visible = false;
            divMainContents.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
            BindRightsBasedSectionDetail(studentApplicationNo);
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
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
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
            divPrint1.Visible = false;
            string studentApplicationNo = lblAppNo.Text.Trim();
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
            divPrint1.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = lblAppNo.Text.Trim();
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
            divPrint1.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = lblAppNo.Text.Trim();
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
            divPrint1.Visible = false;
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
            divPrint1.Visible = false;
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
            divPrint1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Index Changed Events

    #endregion

    #endregion

    #region Button Events

    #region Search Student Click

    protected void btnSearchStudent_Click(object sender, EventArgs e)
    {
        try
        {
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            DataTable dtStudentDetails = new DataTable();

            string studentRegNo = string.Empty;
            string studentRollNo = string.Empty;
            string searchedStudent = string.Empty;
            string studentAdmissionNo = string.Empty;
            string studentApplicationNo = string.Empty;
            string studentAppNo = string.Empty;

            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;

            orderBy = string.Empty;
            orderBySetting = string.Empty;

            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            searchedStudent = txtSearchStudent.Text.Trim();
            if (!string.IsNullOrEmpty(searchedStudent))
            {
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
                        case "1":
                            studentRegNo = searchedStudent;
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Roll_Admit='" + searchedStudent + "'");
                            break;
                        case "2":
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Reg_no='" + searchedStudent + "'");
                            break;
                        case "3":
                            studentAdmissionNo = searchedStudent;
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Roll_No='" + searchedStudent + "'");
                            break;
                    }

                }
                else
                {
                    if (CheckSchoolOrCollege(collegeCode))
                    {
                        studentAdmissionNo = searchedStudent;
                        studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Roll_Admit='" + searchedStudent + "'");
                    }
                    else
                    {
                        if (lblSearchStudent.Text.Trim().ToLower() == "register no" || lblSearchStudent.Text.Trim().ToLower() == "reg no")
                        {
                            studentRegNo = searchedStudent;
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Reg_no='" + searchedStudent + "'");
                        }
                        else if (lblSearchStudent.Text.Trim().ToLower() == "admission no")
                        {
                            studentAdmissionNo = searchedStudent;
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Roll_Admit='" + searchedStudent + "'");
                        }
                        else if (lblSearchStudent.Text.Trim().ToLower().Contains("student roll_no") || lblSearchStudent.Text.Trim().ToLower().Contains("roll no"))
                        {
                            studentRollNo = searchedStudent;
                            studentAppNo = dirAcc.selectScalarString("select App_no from Registration where Roll_No='" + searchedStudent + "'");
                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Enter " + lblSearchStudent.Text.Trim();
                divPopAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(studentAppNo) && !string.IsNullOrEmpty(collegeCode))
            {
                qry = "select clg.collname,ISNULL(InstType,'0') as InstType,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else '' end end end as DegreeDetails,dt.dept_acronym + CASE WHEN (LTRIM(RTRIM(ISNULL(r.Sections, ''))) <> '') THEN ' - ' + LTRIM(RTRIM(ISNULL(r.Sections, '')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) ELSE ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) END AS ClassDetails,r.Batch_year,r.degree_code,r.current_semester,LTRIM(RTRIM(ISNULL(r.Sections, ''))) as Sections,r.college_code,r.app_no,ISNULL(convert(varchar(200),r.serialno),'') as serialNo,r.reg_no,r.roll_no,r.Roll_Admit,r.Stud_Type,r.Stud_Name,Convert(varchar(20),r.Adm_Date,103) as AdmissionDate from Registration r,Course c,Degree dg,Department dt,collinfo clg where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and dt.college_code=clg.college_code and clg.college_code=r.college_code and r.college_code=dg.college_code and r.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=r.degree_code and r.app_no='" + studentAppNo + "' and r.college_code='" + collegeCode + "'";
                dtStudentDetails = dirAcc.selectDataTable(qry);
            }
            if (dtStudentDetails.Rows.Count > 0)
            {
                studentRegNo = string.Empty;
                studentRollNo = string.Empty;
                studentAdmissionNo = string.Empty;
                studentApplicationNo = string.Empty;
                int count = 0;
                foreach (DataRow drStudents in dtStudentDetails.Rows)
                {
                    studentRegNo = Convert.ToString(drStudents["reg_no"]).Trim();
                    studentRollNo = Convert.ToString(drStudents["roll_no"]).Trim();
                    studentAdmissionNo = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                    studentApplicationNo = Convert.ToString(drStudents["app_no"]).Trim();
                    string studentSerialNo = Convert.ToString(drStudents["serialNo"]).Trim();
                    string currentSemester = Convert.ToString(drStudents["current_semester"]).Trim();
                    string collegeName = Convert.ToString(drStudents["current_semester"]).Trim();
                    degreeCode = Convert.ToString(drStudents["degree_code"]).Trim();
                    collegeCode = Convert.ToString(drStudents["college_code"]).Trim();
                    batchYear = Convert.ToString(drStudents["Batch_year"]).Trim();
                    section = Convert.ToString(drStudents["Sections"]).Trim();
                    string AdmissionDate = Convert.ToString(drStudents["AdmissionDate"]).Trim();
                    string studentName = Convert.ToString(drStudents["Stud_Name"]).Trim();
                    string studentType = Convert.ToString(drStudents["Stud_Type"]).Trim();
                    string DegreeDetails = Convert.ToString(drStudents["DegreeDetails"]).Trim();
                    string ClassDetails = Convert.ToString(drStudents["ClassDetails"]).Trim();
                    string insType = Convert.ToString(drStudents["InstType"]).Trim();

                    lblStudentName.Text = studentName;
                    lblStudentRollNo.Text = studentRollNo;
                    lblRegNo.Text = studentRegNo;
                    lblAdmissionNo.Text = studentAdmissionNo;
                    lblAppNo.Text = studentApplicationNo;
                    if (!string.IsNullOrEmpty(insType) && insType.Trim() == "1")
                    {
                        lblClassName.Text = ClassDetails;
                    }
                    else
                    {
                        lblClassName.Text = DegreeDetails;
                    }
                    count++;
                    if (count == 1)
                        break;
                }
                divStudentDetail.Visible = true;
                if (!string.IsNullOrEmpty(studentApplicationNo))
                {
                    BindPreviousCollege(studentApplicationNo);
                    BindRightsBaseBatch(studentApplicationNo, defaultSelectedBatch: batchYear);
                    BindPreviousDegrees(studentApplicationNo, "");
                    BindPreviousDepartment(studentApplicationNo, "", "");
                    BindPreviousSemesters(studentApplicationNo, "", "");
                    BindRightsBasedSectionDetail(studentApplicationNo);
                    BindPreviousTestName();
                    BindPreviousSubject();
                }
            }
            else
            {
                divStudentDetail.Visible = false;
                lblAlertMsg.Text = "No Student Details Were Found";
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

    #region Get Students Marks

    protected void btnGetMarks_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divPrint1.Visible = false;
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
            qryCourseId = string.Empty;
            qrytestNo = string.Empty;
            qrytestName = string.Empty;
            qrySubjectNo = string.Empty;
            qrySubjectName = string.Empty;
            qrySubjectCode = string.Empty;

            DataTable dtStudentMarks = new DataTable();
            DataTable dtGradeDetails = new DataTable();

            if (ddlPrevCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblPrevCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddlPrevCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and srh.collegeCode in(" + collegeCode + ")";
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
                    qryBatchYear = " and srh.BatchYear in(" + batchYear + ")";
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
                    qryDegreeCode = " and srh.degreeCode in(" + degreeCode + ")";
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
                    qrySemester = " and srh.semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0 && ddlSec.Enabled)
            {
                string secValue = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
                {
                    section = secValue;
                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + secValue + "')";
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
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(subjectCode))
                        {
                            subjectCode += ",'" + li.Value + "'";
                        }
                        else
                        {
                            subjectCode = "'" + li.Value + "'";
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
            else if (ddlSubject.Items.Count > 0 && ddlSubject.Visible == true)
            {
                subjectCode = Convert.ToString(ddlSubject.SelectedValue).Trim();
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



            string studentAppNo = string.Empty;
            studentAppNo = lblAppNo.Text.Trim();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(studentAppNo) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
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
                dtStudentMarks = dirAcc.selectDataTable("SELECT srh.App_no,srh.Roll_no,srh.collegeCode,srh.RegNo,srh.BatchYear,srh.degreeCode,srh.semester,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,s.subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,e.min_mark as ConductedMinMark,e.max_mark as ConductedMaxMark,ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'') as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'') as RetestMark FROM CriteriaForInternal c,Exam_type e,Result re,StudentRegisterHistory srh,syllabus_master sm,subject s where s.subject_no=e.subject_no and s.syll_code=sm.syll_code and s.syll_code=c.syll_code and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and srh.BatchYear=sm.Batch_Year and srh.degreeCode=sm.degree_code and srh.semester=sm.semester and srh.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(srh.sections,''))) and srh.App_no='" + studentAppNo + "' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qrytestNo + qrySubjectCode + " and srh.RedoType='2' order by srh.App_no,s.subject_code");

                //qry = "select * from Grade_Master where batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' union select * from Grade_Master where batch_year='' and College_Code='' and Degree_Code='' and Semester='0'";
                qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,gm.Criteria,gm.classify from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and gm.Semester='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,gm.Criteria,gm.classify from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and gm.Semester='0'"; // order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Frange,gm.Trange
                dtGradeDetails = dirAcc.selectDataTable(qry);
            }
            if (dtStudentMarks.Rows.Count > 0)
            {
                DataTable dtDistinctSubject = new DataTable();
                DataTable dtDistinctTest = new DataTable();
                dtStudentMarks.DefaultView.Sort = "TestNo";
                dtDistinctTest = dtStudentMarks.DefaultView.ToTable(true, "TestNo", "TestName");
                dtStudentMarks.DefaultView.Sort = "subject_code";
                dtDistinctSubject = dtStudentMarks.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name", "subjectpriority");
                Init_Spread(1);
                int testCount = 0;

                int rowcnt = 0;
                foreach (DataRow drTest in dtDistinctTest.Rows)
                {
                    int markCol = 0;
                    int gradeCol = 0;
                    string testNumber = Convert.ToString(drTest["TestNo"]).Trim();
                    string testNames = Convert.ToString(drTest["TestName"]).Trim();


                    // markCol = FpStudentMarkList.Sheets[0].ColumnCount - 2;
                    // gradeCol = FpStudentMarkList.Sheets[0].ColumnCount - 1;

                    data.Columns.Add(testNames);


                    System.Text.StringBuilder grade = new System.Text.StringBuilder("Grade");

                    AddTableColumn(data, grade);

                    data.Rows[0][data.Columns.Count - 2] = testNames;
                    data.Rows[0][data.Columns.Count - 1] = "Grade";
                    // FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, markCol].Tag = testNumber;
                    //  FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, markCol, 2, 1);



                    //FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, gradeCol].Tag = testNumber;
                    // FpStudentMarkList.Sheets[0].ColumnHeader.Cells[0, gradeCol].Note = testNames;
                    // FpStudentMarkList.Sheets[0].ColumnHeaderSpanModel.Add(0, gradeCol, 2, 1);
                    int subjectRow = 0;
                    int row = 0;
                    rowcnt++;
                    foreach (DataRow drSubject in dtDistinctSubject.Rows)
                    {
                        string subjectCodeVal = Convert.ToString(drSubject["subject_code"]).Trim();
                        string subjectNameVal = Convert.ToString(drSubject["subject_name"]).Trim();
                        string subjectNoVal = Convert.ToString(drSubject["subject_no"]).Trim();
                        string testMark = string.Empty;
                        string testMaxMark = string.Empty;
                        string testMinMark = string.Empty;
                        double testSubMarks = 0;
                        double testMaxMarks = 0;
                        double testMinMarks = 0;
                        row++;
                        DataView dvTestMark = new DataView();
                        dtStudentMarks.DefaultView.RowFilter = "subject_no='" + subjectNoVal + "' and TestNo='" + testNumber + "'";
                        dvTestMark = dtStudentMarks.DefaultView;
                        if (testCount == 0)
                        {
                            if (rowcnt == 1)
                            {
                                drow = data.NewRow();
                                data.Rows.Add(drow);
                                data.Rows[data.Rows.Count - 1][0] = Convert.ToString((subjectRow + 1)).Trim();
                                data.Rows[data.Rows.Count - 1][1] = Convert.ToString(subjectCodeVal).Trim();
                                data.Rows[data.Rows.Count - 1][2] = Convert.ToString(subjectNameVal).Trim();
                            }
                            else
                            {

                                data.Rows[row][0] = Convert.ToString((subjectRow + 1)).Trim();
                                data.Rows[row][1] = Convert.ToString(subjectCodeVal).Trim();
                                data.Rows[row][2] = Convert.ToString(subjectNameVal).Trim();
                            }
                        }
                        string displayMark = string.Empty;
                        string displayGrade = string.Empty;
                        bool result = false;
                        if (dvTestMark.Count > 0)
                        {
                            testMark = Convert.ToString(dvTestMark[0]["TestMark"]).Trim();
                            testMaxMark = Convert.ToString(dvTestMark[0]["ConductedMaxMark"]).Trim();
                            testMinMark = Convert.ToString(dvTestMark[0]["ConductedMinMark"]).Trim();
                            string batch = Convert.ToString(dvTestMark[0]["BatchYear"]).Trim();
                            string college = Convert.ToString(dvTestMark[0]["collegeCode"]).Trim();
                            string degree = Convert.ToString(dvTestMark[0]["degreeCode"]).Trim();
                            string sems = Convert.ToString(dvTestMark[0]["semester"]).Trim();

                            double.TryParse(testMark, out testSubMarks);
                            double.TryParse(testMaxMark, out testMaxMarks);
                            double.TryParse(testMinMark, out testMinMarks);

                            double outof100 = 0;
                            if (testSubMarks > 0 && testMaxMarks > 0)
                                outof100 = Math.Round((testSubMarks / testMaxMarks) * 100, 0, MidpointRounding.AwayFromZero);
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
                            }
                            else if (string.IsNullOrEmpty(testMark))
                            {
                                displayMark = "--";
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
                                result = true;
                            }
                            else
                            {
                                displayGrade = "--";
                                result = true;
                            }
                        }
                        else
                        {
                            displayMark = "--";
                            displayGrade = "--";
                            result = true;
                        }
                        if (rowcnt == 1)
                        {
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = Convert.ToString(displayMark).Trim();
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = Convert.ToString(displayGrade).Trim();
                        }
                        else
                        {
                            data.Rows[row][data.Columns.Count - 2] = Convert.ToString(displayMark).Trim();
                            data.Rows[row][data.Columns.Count - 1] = Convert.ToString(displayGrade).Trim();

                        }

                        subjectRow++;
                    }

                    testCount++;
                }

                if (data.Columns.Count > 0 && data.Rows.Count > 1)
                {
                    divMainContents.Visible = true;
                    divPrint1.Visible = true;
                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;

                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }


    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = 2; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        catch
        {


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
            Printcontrol.Visible = false;
            string reportname = txtExcelName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (Showgrid.Visible == true)
                {
                    da.printexcelreportgrid(Showgrid, reportname);
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
            string ss = Session["usercode"].ToString();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = spPageHeading.InnerHtml.ToString();
            string pagename = "StudentsMarkPrevousHistory.aspx";
            string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " " + lblSem.Text.Trim() + " : " + Convert.ToString(ddlSem.SelectedItem).Trim();
            if (Showgrid.Visible == true)
            {
                Printcontrol.loadspreaddetails(Showgrid, pagename, rptheadname, 0, ss);
            }
            Printcontrol.Visible = true;
            lblExcelErr.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }


    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Student's Previous CAM Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


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
            lbl.Add(lblPrevCollege);
            lbl.Add(lblDegree);
            lbl.Add(lblBranch);
            lbl.Add(lblSem);
            fields.Add(0);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
                spPageHeading.InnerHtml = "Student's Previous Test Report";
                Page.Title = "Student's Previous Test Report";
            }
            else
            {
                lblBatch.Text = "Batch";
                spPageHeading.InnerHtml = "Student's Previous CAM Report";
                Page.Title = "Student's Previous CAM Report";
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

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = "ORDER BY " + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
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
            }
        }
        catch
        {
        }
        return mark;
    }

    private void GetSUbjectGrade()
    {
        try
        {

        }
        catch
        {
        }
    }

    #endregion

}