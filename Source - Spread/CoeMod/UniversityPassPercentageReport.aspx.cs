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
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class UniversityPassPercentageReport : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    string collegeCode = string.Empty;
    string eduLevel = string.Empty;
    string qryEduLevel = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string examMonth = string.Empty;

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string examYear = string.Empty;
    string qryExamYear = string.Empty;
    string streamNames = string.Empty;
    string qryStream = string.Empty;
    string qryCourseId = string.Empty;
    string qryExamMonth = string.Empty;

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
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
          
            if (!IsPostBack)
            {
                Bindcollege();
                BindStream();
                BindEduLevel();
                BindRightsBaseBatch();
                BindDegree();
                bindbranch();
                BindSemester();
                BindExamYear();
                BindExamMonth();
                BindSubjectType();

            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region college

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Text = "--Select--";
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

                cblCollege.DataSource = dtCommon;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                checkBoxListselectOrDeselect(cblCollege, true);
                CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");

            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion

    public void BindStream()
    {
        try
        {
            collegeCode = string.Empty;
            ds.Clear();
            cblStream.Items.Clear();
            chkStream.Checked = false;
            txtStream.Text = "--Select--";
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //}
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();

            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and c.college_code in(" + collegeCode + ") ";
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string mode = "select distinct ltrim(rtrim(isnull(c.type,''))) as type from course c where c.college_code in (" + collegeCode + ") and c.type is not null and c.type<>''";
                ds = da.select_method_wo_parameter(mode, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblStream.DataSource = ds;
                cblStream.DataTextField = "type";
                cblStream.DataValueField = "type";
                cblStream.DataBind();
                checkBoxListselectOrDeselect(cblStream, true);
                CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");

                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "type";
                ddlStream.DataValueField = "type";
                ddlStream.DataBind();
                ddlStream.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindEduLevel()
    {
        try
        {
            ds.Clear();
            ddlEduLevel.Items.Clear();
            cblEduLevel.Items.Clear();
            chkEduLevel.Checked = false;
            txtEduLevel.Text = "--Select--";
            collegeCode = string.Empty;
            streamNames = string.Empty;
            qryStream = string.Empty;
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //}
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();

            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and c.college_code in(" + collegeCode + ") ";
            }

            if (ddlStream.Items.Count > 0 && ddlStream.Visible)
            {
                streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

            }
            else if (cblStream.Items.Count > 0 && txtStream.Visible)
            {
                streamNames = getCblSelectedText(cblStream);
            }

            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                string qry = "select distinct c.Edu_Level from course c where c.college_code in(" + collegeCode + ") " + qryStream + " order by c.Edu_Level desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblEduLevel.DataSource = ds;
                    cblEduLevel.DataTextField = "Edu_Level";
                    cblEduLevel.DataValueField = "Edu_Level";
                    cblEduLevel.DataBind();
                    checkBoxListselectOrDeselect(cblEduLevel, true);
                    CallCheckboxListChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");

                    ddlEduLevel.DataSource = ds;
                    ddlEduLevel.DataTextField = "Edu_Level";
                    ddlEduLevel.DataValueField = "Edu_Level";
                    ddlEduLevel.DataBind();
                    ddlEduLevel.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region batch

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;

            ddlBatch.Items.Clear();
            cblBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "--Select--";
            qryBatchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            qryStream = string.Empty;

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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();

            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
            }
            if (ddlStream.Items.Count > 0 && ddlStream.Visible)
            {
                //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
                streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

            }
            else if (cblStream.Items.Count > 0 && txtStream.Visible)
            {
                streamNames = getCblSelectedText(cblStream);
            }

            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
            {
                eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
            {
                eduLevel = getCblSelectedValue(cblEduLevel);
            }
            if (!string.IsNullOrEmpty(eduLevel))
            {
                qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
            }

            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct r.batch_year from tbl_attendance_rights r where ISNULL(batch_year,'0')<>'0' and batch_year<>'' and Batch_Year<>-1 " + qryCollegeCode + qryUserOrGroupCode + " order by r.batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            List<int> lstBatchYear = new List<int>();
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                lstBatchYear = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatchYear.Count > 0)
                    qryBatchYear = " and r.Batch_Year in(" + string.Join(",", lstBatchYear.ToArray()) + ")";
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dt.Dept_Code=dg.Dept_Code and r.college_code=dg.college_code and ISNULL(r.Batch_Year,'0')<>'0' and r.Batch_Year<>-1 " + qryCollegeCode + qryStream + qryBatchYear + qryEduLevel + " order by r.Batch_Year desc";//and r.college_code in(" + collegeCodes + ") 

                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_Year";
                ddlBatch.DataValueField = "Batch_Year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;

                cblBatch.DataSource = ds;
                cblBatch.DataTextField = "Batch_Year";
                cblBatch.DataValueField = "Batch_Year";
                cblBatch.DataBind();
                checkBoxListselectOrDeselect(cblBatch, true);
                CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");

            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            string Master1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                Master1 = group.Split(';')[0];
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]).Trim();
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Master1.Trim()) && !string.IsNullOrEmpty(collegecode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code='" + collegecode + "'";
                //user_id='" + Master1 + "' and 
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    #endregion

    #region degree

    public void BindDegree()
    {
        DataSet ds = new DataSet();
        string college_code = Convert.ToString(ddlCollege.SelectedValue).Trim();
        string query = string.Empty;
        ddlDegree.Items.Clear();
        cblDegree.Items.Clear();
        chkDegree.Checked = false;
        txtDegree.Text = "--Select--";

        batchYear = string.Empty;
        collegeCode = string.Empty;
        streamNames = string.Empty;
        courseId = string.Empty;
        eduLevel = string.Empty;

        qryCollegeCode = string.Empty;
        qryBatchYear = string.Empty;
        qryStream = string.Empty;
        qryEduLevel = string.Empty;
        qryCourseId = string.Empty;

        string usercode = Convert.ToString(Session["usercode"]).Trim();
        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
        {
            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();

        }
        else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
        {
            collegeCode = getCblSelectedValue(cblCollege);
        }
        if (!string.IsNullOrEmpty(collegeCode))
        {
            qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
        }
        if (ddlStream.Items.Count > 0 && ddlStream.Visible)
        {
            //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
            streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

        }
        else if (cblStream.Items.Count > 0 && txtStream.Visible)
        {
            streamNames = getCblSelectedText(cblStream);
        }

        if (!string.IsNullOrEmpty(streamNames))
        {
            qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
        }
        if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
        {
            //eduLevel = Convert.ToString(ddlEduLevel.SelectedValue).Trim();
            eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
        }
        else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
        {
            eduLevel = getCblSelectedValue(cblEduLevel);
        }
        if (!string.IsNullOrEmpty(eduLevel))
        {
            qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
        }
        if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryEduLevel))
        {
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
            {
                query = "select distinct dg.course_id,c.course_name from degree dg,course c,deptprivilages dp where c.course_id=dg.course_id and c.college_code = dg.college_code and dp.Degree_code=dg.Degree_code and group_code='" + group_user + "' " + qryCollegeCode + qryStream + qryEduLevel;
            }
            else
            {
                query = "select distinct dg.course_id,c.course_name from degree dg,course c,deptprivilages dp where c.course_id=dg.course_id and c.college_code = dg.college_code and dp.Degree_code=dg.Degree_code and user_code='" + usercode + "' " + qryCollegeCode + qryStream + qryEduLevel;
            }

            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();

            cblDegree.DataSource = ds;
            cblDegree.DataTextField = "Course_Name";
            cblDegree.DataValueField = "Course_Id";
            cblDegree.DataBind();
            checkBoxListselectOrDeselect(cblDegree, true);
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
        }
    }

    #endregion

    #region Branch

    public void bindbranch()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddlBranch.Items.Clear();
            cblBranch.Items.Clear();
            chkBranch.Checked = false;
            txtBranch.Text = "--Select--";
            ht.Clear();

            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            courseId = string.Empty;
            eduLevel = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryCourseId = string.Empty;

            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string singleuser = Convert.ToString(Session["single_user"]).Trim();
            string group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }

            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }
            if (ddlStream.Items.Count > 0 && ddlStream.Visible)
            {
                //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
                streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

            }
            else if (cblStream.Items.Count > 0 && txtStream.Visible)
            {
                streamNames = getCblSelectedText(cblStream);
            }

            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
            {
                //eduLevel = Convert.ToString(ddlEduLevel.SelectedValue).Trim();
                eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
            {
                eduLevel = getCblSelectedValue(cblEduLevel);
            }
            if (!string.IsNullOrEmpty(eduLevel))
            {
                qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
            }

            if (ddlDegree.Items.Count > 0 && ddlDegree.Visible)
            {
                courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
            }
            else if (cblDegree.Items.Count > 0 && txtDegree.Visible)
            {
                courseId = getCblSelectedValue(cblDegree);
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                qryCourseId = " and c.course_id in(" + courseId + ")";
            }

            string course_id = string.Empty;
            if (!string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                string query = string.Empty;
                if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
                {
                    query = "select distinct dg.degree_code,dt.dept_name from degree dg,department dt,course c,deptprivilages dp where c.course_id=dg.course_id and dt.dept_code=dg.dept_code and c.college_code = dg.college_code and dt.college_code = dg.college_code and dp.Degree_code=dg.Degree_code and dp.group_code='" + group_user + "' " + qryCollegeCode + qryCourseId + qryStream + qryEduLevel;
                }
                else
                {
                    query = "select distinct dg.degree_code,dt.dept_name from degree dg,department dt,course c,deptprivilages dp where c.course_id=dg.course_id and dt.dept_code=dg.dept_code and c.college_code = dg.college_code and dt.college_code = dg.college_code and dp.Degree_code=dg.Degree_code and dp.user_code='" + usercode + "' " + qryCollegeCode + qryCourseId + qryStream + qryEduLevel;
                }
                ds = da.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();

                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    #region Semester

    public void bindsem()
    {
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

        }
    }

    private void BindSemester()
    {
        try
        {
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            ddlSem.Items.Clear();
            cblSem.Items.Clear();
            chkSem.Checked = false;
            txtSem.Enabled = false;
            txtSem.Text = "--Select--";
            ds.Clear();
            ds.Reset();
            bool isFirstNonSem = false;
            int duration = 0;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
            {
                batchYear = getCblSelectedValue(cblBatch);
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and dg.batch_year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
            {
                degreeCode = getCblSelectedValue(cblBranch);
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and dg.degree_code in(" + degreeCode + ")";
            }
            if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                qry = "  select distinct MAX(dg.ndurations) as Duration,dg.first_year_nonsemester AS first_year_nonsemester from ndegree dg where ndurations<>'0' " + qryBatchYear + qryCollegeCode + qryDegreeCode + "  group by dg.first_year_nonsemester";
                ds = da.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
                {
                    qry = " select distinct MAX(dg.duration) Duration,dg.first_year_nonsemester AS first_year_nonsemester  from degree dg where duration<>'0' " + qryCollegeCode + qryDegreeCode + " group by dg.first_year_nonsemester";
                    ds = da.select_method_wo_parameter(qry, "text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["first_year_nonsemester"]).Trim(), out isFirstNonSem);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["Duration"]).Trim(), out duration);
                for (int i = 1; i <= duration; i++)
                {
                    if (isFirstNonSem == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                        cblSem.Items.Add(i.ToString());
                    }
                    else if (isFirstNonSem == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                        cblSem.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();

            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            courseId = string.Empty;
            eduLevel = string.Empty;
            degreeCode = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;

            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }
            if (ddlStream.Items.Count > 0 && ddlStream.Visible)
            {
                //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
                streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

            }
            else if (cblStream.Items.Count > 0 && txtStream.Visible)
            {
                streamNames = getCblSelectedText(cblStream);
            }

            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
            {
                //eduLevel = Convert.ToString(ddlEduLevel.SelectedValue).Trim();
                eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
            {
                eduLevel = getCblSelectedValue(cblEduLevel);
            }
            if (!string.IsNullOrEmpty(eduLevel))
            {
                qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
            }

            if (ddlDegree.Items.Count > 0 && ddlDegree.Visible)
            {
                courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
            }
            else if (cblDegree.Items.Count > 0 && txtDegree.Visible)
            {
                courseId = getCblSelectedValue(cblDegree);
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                qryCourseId = " and c.course_id in(" + courseId + ")";
            }

            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
            {
                batchYear = getCblSelectedValue(cblBatch);
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
            {
                degreeCode = getCblSelectedValue(cblBranch);
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
            }
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + " order by ed.Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamYear.DataSource = ds;
                    ddlExamYear.DataTextField = "Exam_year";
                    ddlExamYear.DataValueField = "Exam_year";
                    ddlExamYear.DataBind();
                    ddlExamYear.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            ddlExamMonth.Items.Clear();
            ds.Clear();
            examYear = string.Empty;
            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            courseId = string.Empty;
            eduLevel = string.Empty;
            degreeCode = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            qryExamYear = string.Empty;

            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }
            if (ddlStream.Items.Count > 0 && ddlStream.Visible)
            {
                //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
                streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";

            }
            else if (cblStream.Items.Count > 0 && txtStream.Visible)
            {
                streamNames = getCblSelectedText(cblStream);
            }

            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
            {
                //eduLevel = Convert.ToString(ddlEduLevel.SelectedValue).Trim();
                eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
            {
                eduLevel = getCblSelectedValue(cblEduLevel);
            }
            if (!string.IsNullOrEmpty(eduLevel))
            {
                qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
            }

            if (ddlDegree.Items.Count > 0 && ddlDegree.Visible)
            {
                courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
            }
            else if (cblDegree.Items.Count > 0 && txtDegree.Visible)
            {
                courseId = getCblSelectedValue(cblDegree);
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                qryCourseId = " and c.course_id in(" + courseId + ")";
            }

            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
            {
                batchYear = getCblSelectedValue(cblBatch);
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
            {
                degreeCode = getCblSelectedValue(cblBranch);
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
            }

            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and Exam_year in (" + examYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + qryExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamMonth.DataSource = ds;
                    ddlExamMonth.DataTextField = "Month_Name";
                    ddlExamMonth.DataValueField = "Exam_Month";
                    ddlExamMonth.DataBind();
                    ddlExamMonth.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void BindSubjectType()
    {
        try
        {
            ddlSubjectType.Items.Clear();
            ddlSubjectType.Enabled = false;
            ds.Clear();
            examYear = string.Empty;
            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            courseId = string.Empty;
            eduLevel = string.Empty;
            degreeCode = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            qryExamYear = string.Empty;

            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
            {
                batchYear = getCblSelectedValue(cblBatch);
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and sm.Batch_Year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
            {
                degreeCode = getCblSelectedValue(cblBranch);
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
            }

            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and ed.Exam_year in (" + examYear + ")";
                }
            }

            if (ddlExamMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in (" + examMonth + ")";
                }
            }

            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                qry = " select distinct ss.subject_type from sub_sem ss,Syllabus_master sm,Exam_Details ed where sm.syll_code=ss.syll_code and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code " + qryExamMonth + qryExamYear + qryBatchYear + qryDegreeCode + " order by ss.subject_type";
                ds = da.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSubjectType.DataSource = ds;
                ddlSubjectType.DataTextField = "subject_type";
                ddlSubjectType.DataValueField = "subject_type";
                ddlSubjectType.DataBind();
                ddlSubjectType.SelectedIndex = 0;
                ddlSubjectType.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //lblAlertMsg.Text = string.Empty;
            //divPopAlert.Visible = false;
            //lblErrSearch.Text = string.Empty;
            //lblErrSearch.Visible = false;
            //string studentApplicationNo = string.Empty;
            //ShowReport.Visible = false;
            //btnPrint.Visible = false;
            //BindRightsBaseBatch();
            //BindDegree();
            //bindbranch();
            //bindsem();
            //BindSectionDetail();
            //bindTest();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
            ShowReport.Visible = false;

        }
        catch (Exception ex)
        {
            //    lblErrSearch.Text = Convert.ToString(ex);
            //    lblErrSearch.Visible = true;
            //    da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void chkStream_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            //CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            //BindDegree();
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void chkEduLevel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            //ShowReport.Visible = false;
            //btnPrint.Visible = false;
            //string studentApplicationNo = string.Empty;

            //BindDegree();
            //bindbranch();
            //bindsem();
            //BindSectionDetail();
            //bindTest();
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
            ShowReport.Visible = false;

        }
        catch (Exception ex)
        {
            //    lblErrSearch.Text = Convert.ToString(ex);
            //    lblErrSearch.Visible = true;
            //    da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            ShowReport.Visible = false;


            //bindbranch();
            //bindsem();
            //BindSectionDetail();
            //bindTest();
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();

        }
        catch (Exception ex)
        {
            //    lblErrSearch.Text = Convert.ToString(ex);
            //    lblErrSearch.Visible = true;
            //    da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            ShowReport.Visible = false;
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            //    lblErrSearch.Text = Convert.ToString(ex);
            //    lblErrSearch.Visible = true;
            //    da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSemester();
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlSem_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            //lblAlertMsg.Text = string.Empty;
            //divPopAlert.Visible = false;
            //lblErrSearch.Text = string.Empty;
            //lblErrSearch.Visible = false;
            //ShowReport.Visible = false;
            //btnPrint.Visible = false;
            //string studentApplicationNo = string.Empty;
            //BindSectionDetail();
            //bindTest();
            //BindSubjectType();
            ShowReport.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            ShowReport.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowReport.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            BindExamMonth();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            BindSubjectType();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSubjectType_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Index Changed Events

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

    #endregion

    #region Report

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowReport.Visible = false;
        lblSubjectType.Visible = false;
        ddlSubjectType.Visible = false;
        if (ddlReport.SelectedIndex == 0)
        {
            lblSubjectType.Visible = true;
            ddlSubjectType.Visible = true;
        }
    }

    #endregion

    #region Go

    protected void btnGo_Click(object sender, EventArgs e)
    {
        examYear = string.Empty;
        degreeCode = string.Empty;
        batchYear = string.Empty;
        collegeCode = string.Empty;
        streamNames = string.Empty;
        courseId = string.Empty;
        eduLevel = string.Empty;


        qryCollegeCode = string.Empty;
        qryBatchYear = string.Empty;
        qryStream = string.Empty;
        qryEduLevel = string.Empty;
        qryCourseId = string.Empty;
        qryDegreeCode = string.Empty;
        qryExamYear = string.Empty;
        if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
        {
            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
        }
        else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
        {
            collegeCode = getCblSelectedValue(cblCollege);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(collegeCode))
        {
            qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblCollege.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        if (ddlStream.Items.Count > 0 && ddlStream.Visible)
        {
            //streamNames = Convert.ToString(ddlStream.SelectedValue).Trim();
            streamNames = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";
            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            else
            {
                //lblAlertMsg.Text = "Please Select Atleast One " + lblStream.Text.Trim();
                //divPopAlert.Visible = true;
                //return;
            }
        }
        else if (cblStream.Items.Count > 0 && txtStream.Visible)
        {
            streamNames = getCblSelectedText(cblStream);
            if (!string.IsNullOrEmpty(streamNames))
            {
                qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
            }
            else
            {
                //lblAlertMsg.Text = "Please Select Atleast One " + lblStream.Text.Trim();
                //divPopAlert.Visible = true;
                //return;
            }
        }
        if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible)
        {
            //eduLevel = Convert.ToString(ddlEduLevel.SelectedValue).Trim();
            eduLevel = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
        }
        else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible)
        {
            eduLevel = getCblSelectedValue(cblEduLevel);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblEduLevel.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(eduLevel))
        {
            qryEduLevel = " and c.edu_level in(" + eduLevel + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblEduLevel.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        if (ddlDegree.Items.Count > 0 && ddlDegree.Visible)
        {
            courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
        }
        else if (cblDegree.Items.Count > 0 && txtDegree.Visible)
        {
            courseId = getCblSelectedValue(cblDegree);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(courseId))
        {
            qryCourseId = " and c.course_id in(" + courseId + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblDegree.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
        {
            batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
        }
        else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
        {
            batchYear = getCblSelectedValue(cblBatch);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(batchYear))
        {
            qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblBatch.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
        {
            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
        }
        else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
        {
            degreeCode = getCblSelectedValue(cblBranch);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(degreeCode))
        {
            qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblBranch.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        if (ddlSem.Items.Count > 0 && ddlSem.Visible)
        {
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
        }
        else if (cblSem.Items.Count > 0 && txtSem.Visible)
        {
            semester = getCblSelectedValue(cblSem);
        }
        else
        {
            lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (!string.IsNullOrEmpty(semester))
        {
            qrySemester = " and sm.semester in(" + semester + ")";
        }
        else
        {
            lblAlertMsg.Text = "Please Select Atleast One " + lblSem.Text.Trim();
            divPopAlert.Visible = true;
            return;
        }

        examYear = string.Empty;
        qryExamYear = string.Empty;
        if (ddlExamYear.Items.Count > 0)
        {
            examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(examYear))
            {
                qryExamYear = " and Exam_year in (" + examYear + ")";
            }
        }
        else
        {
            lblAlertMsg.Text = "No " + lblExamYear.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        if (ddlExamMonth.Items.Count > 0)
        {
            examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(examMonth))
            {
                qryExamMonth = " and exam_month in (" + examMonth + ")";
            }
        }
        else
        {
            lblAlertMsg.Text = "No " + lblExamMonth.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }

        ds = getdetailsSubjectWisereport();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadspread(ds);
        }
        else
        {
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            lblAlertMsg.Text = "No Record Found!";
            divPopAlert.Visible = true;
            return;
        }
    }

    #endregion

    #region Close Popup

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

    #endregion Close Popup

    private DataSet getdetailsSubjectWisereport()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value

            //string collegecode = string.Empty;
            //string batch = string.Empty;
            //string degree = string.Empty;
            //string branch = string.Empty;
            //string sem = string.Empty;
            //string examyear = string.Empty;
            //string exammonth = string.Empty;

            //if (ddlCollege.Items.Count > 0)
            //    collegecode = Convert.ToString(ddlCollege.SelectedValue);
            //if (ddlBatch.Items.Count > 0)
            //    batch = Convert.ToString(ddlBatch.SelectedValue);
            //if (ddlDegree.Items.Count > 0)
            //    degree = Convert.ToString(ddlDegree.SelectedValue); //course Id B.sc
            //if (ddlBranch.Items.Count > 0)
            //    branch = Convert.ToString(ddlBranch.SelectedValue);  //department physics
            //if (ddlSem.Items.Count > 0)
            //    sem = Convert.ToString(ddlSem.SelectedItem.Text);
            //if (ddlExamYear.Items.Count > 0)
            //    examyear = Convert.ToString(ddlExamYear.SelectedValue);
            //if (ddlExamMonth.Items.Count > 0)
            //    exammonth = Convert.ToString(ddlExamMonth.SelectedValue);

            examYear = string.Empty;
            batchYear = string.Empty;
            collegeCode = string.Empty;
            streamNames = string.Empty;
            courseId = string.Empty;
            eduLevel = string.Empty;
            degreeCode = string.Empty;

            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            qryExamYear = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }

            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
            }

            if (ddlDegree.Items.Count > 0 && ddlDegree.Visible)
            {
                courseId = Convert.ToString(ddlDegree.SelectedValue).Trim();
            }
            else if (cblDegree.Items.Count > 0 && txtDegree.Visible)
            {
                courseId = getCblSelectedValue(cblDegree);
            }
            if (!string.IsNullOrEmpty(courseId))
            {
                qryCourseId = " and c.course_id in(" + courseId + ")";
            }

            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible)
            {
                batchYear = getCblSelectedValue(cblBatch);
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and sm.Batch_Year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Visible)
            {
                degreeCode = getCblSelectedValue(cblBranch);
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
            }

            if (ddlSem.Items.Count > 0 && ddlSem.Visible)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            else if (cblSem.Items.Count > 0 && txtSem.Visible)
            {
                semester = getCblSelectedValue(cblSem);
            }
            if (!string.IsNullOrEmpty(semester))
            {
                qrySemester = " and sm.semester in(" + semester + ")";
            }

            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and ed.Exam_year in (" + examYear + ")";
                }
            }

            if (ddlExamMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(examMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in (" + examMonth + ")";
                }
            }

            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qrySemester))
            {
                if (ddlReport.SelectedValue == "1") //subject type
                {
                    selQ = " select distinct ss.subject_type from Exam_Details ed, exam_appl_details ead,exam_application ea,subject s,sub_sem ss,syllabus_master sm where sm.Batch_Year=ed.batch_year and ed.degree_code =sm.degree_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and s.subject_no=ead.subject_no and ea.exam_code=ed.exam_code and ea.appl_no =ead.appl_no and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' " + qryBatchYear + qryDegreeCode + qryExamMonth + qryExamYear + qrySemester + " group by ss.subject_type,ss.subType_no order by ss.subject_type;";//Subject Type,Subject Type No and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' and sm.Batch_Year='" + batch + "'  and sm.degree_code='" + branch + "'

                    selQ += " select ss.subject_type,Count(distinct m.roll_no) as appearedcount from mark_entry m,subject s,syllabus_master sm,sub_sem ss,Exam_Details ed where s.subject_no=m.subject_no and m.exam_code=ed.exam_code and s.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code and m.total is not null and m.result is not null and ISNULL(m.external_mark,'0')>=0 and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' " + qryBatchYear + qryDegreeCode + qryExamMonth + qryExamYear + qrySemester + " group by ss.subject_type order by ss.subject_type;";//Appeared 

                    selQ += " select ss.subject_type,Count(distinct m.roll_no) as Passedcount from mark_entry m,subject s,syllabus_master sm,sub_sem ss,Exam_Details ed where s.subject_no=m.subject_no and m.exam_code=ed.exam_code and s.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code and m.total is not null and m.result is not null and ISNULL(m.external_mark,'0')>=0 and m.result='pass' and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  " + qryBatchYear + qryDegreeCode + qrySemester + qryExamMonth + qryExamYear + " group by ss.subject_type order by ss.subject_type;";//Passed
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(selQ, "Text");

                }
                else  //subject Wise
                {
                    string subjectTypeName = string.Empty;
                    string qrySubjectTypeName = string.Empty;
                    if (ddlSubjectType.Items.Count > 0)
                    {
                        subjectTypeName = Convert.ToString(ddlSubjectType.SelectedValue).Trim();
                        qrySubjectTypeName = " and ss.subject_type='" + subjectTypeName + "'";
                    }
                    selQ = " select distinct s.subject_name,s.subject_code from Exam_Details ed, exam_appl_details ead,exam_application ea,subject s,sub_sem ss,syllabus_master sm where sm.Batch_Year=ed.batch_year and ed.degree_code =sm.degree_code and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and s.subject_no=ead.subject_no and ea.exam_code=ed.exam_code and ea.appl_no =ead.appl_no and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' " + qryBatchYear + qryDegreeCode + qryExamMonth + qryExamYear + qrySemester + qrySubjectTypeName + " group by s.subject_name,s.subject_code order by s.subject_name,s.subject_code;"; //Subject Code,SubjectCode No

                    selQ += " select s.subject_name,s.subject_code,Count(distinct m.roll_no)as appearedcount from mark_entry m,subject s,syllabus_master sm,sub_sem ss,Exam_Details ed where s.subject_no=m.subject_no and m.exam_code=ed.exam_code and s.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code and m.total is not null and m.result is not null and ISNULL(m.external_mark,'0')>=0 and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  " + qryBatchYear + qryDegreeCode + qrySemester + qryExamMonth + qryExamYear + qrySubjectTypeName + " group by s.subject_name,s.subject_code order by s.subject_name,s.subject_code;";//Appeared 

                    selQ += " select s.subject_name,s.subject_code,Count(distinct m.roll_no)as Passedcount from mark_entry m,subject s,syllabus_master sm,sub_sem ss,Exam_Details ed where s.subject_no=m.subject_no and m.exam_code=ed.exam_code and s.syll_code=sm.syll_code and s.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and ed.batch_year=sm.Batch_Year and ed.degree_code=sm.degree_code and m.total is not null and m.result is not null and ISNULL(m.external_mark,'0')>=0 and m.result='pass' and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  " + qryBatchYear + qrySemester + qryDegreeCode + qryExamMonth + qryExamYear + qrySubjectTypeName + " group by s.subject_name,s.subject_code order by s.subject_name,s.subject_code;";//Passed

                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(selQ, "Text");
                }
            }

            #endregion
        }
        catch (Exception ex)
        { }
        return dsload;
    }

    private void loadspread(DataSet ds)
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Paper Part");
            dt.Columns.Add("Paper Code");
            dt.Columns.Add("Appeared");
            dt.Columns.Add("Passed");
            ////dt.Columns.Add("Course");
            dt.Columns.Add("Percent");

            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int row = 0; row < dt.Columns.Count; row++)
            {
                spreadDet.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);

                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[row].Visible = true;
                if (col == "Paper Code" && ddlReport.SelectedValue == "1")
                {
                    spreadDet.Sheets[0].Columns[row].Visible = false;

                }
            }

            DataRow drow;
            int rowcount = 0;
            DataTable dtnew = new DataTable();
            double pc = 0, ac = 0;
            double outof100 = 0;
            string Passedcount = string.Empty;
            string appearedcount = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Passedcount = string.Empty;
                    appearedcount = string.Empty;
                    outof100 = 0;
                    if (ddlReport.SelectedValue == "1")
                    {
                        string subno = Convert.ToString(ds.Tables[0].Rows[row]["subject_type"]).Trim();//Appeared
                        ds.Tables[1].DefaultView.RowFilter = "subject_type ='" + subno + "'";
                        dtnew = ds.Tables[1].DefaultView.ToTable();
                        if (dtnew.Rows.Count > 0)
                            appearedcount = Convert.ToString(dtnew.Rows[0]["appearedcount"]).Trim();

                        string subtypeno = Convert.ToString(ds.Tables[0].Rows[row]["subject_type"]).Trim();//passed
                        ds.Tables[2].DefaultView.RowFilter = "subject_type ='" + subtypeno + "'";
                        dtnew = ds.Tables[2].DefaultView.ToTable();
                        if (dtnew.Rows.Count > 0)
                            Passedcount = Convert.ToString(dtnew.Rows[0]["Passedcount"]).Trim();
                    }
                    else
                    {
                        string subno = Convert.ToString(ds.Tables[0].Rows[row]["subject_code"]).Trim();//Appeared
                        ds.Tables[1].DefaultView.RowFilter = "subject_code ='" + subno + "'";
                        dtnew = ds.Tables[1].DefaultView.ToTable();
                        if (dtnew.Rows.Count > 0)
                            appearedcount = Convert.ToString(dtnew.Rows[0]["appearedcount"]).Trim();

                        string subtypeno = Convert.ToString(ds.Tables[0].Rows[row]["subject_code"]).Trim();//passed
                        ds.Tables[2].DefaultView.RowFilter = "subject_code ='" + subtypeno + "'";
                        dtnew = ds.Tables[2].DefaultView.ToTable();
                        if (dtnew.Rows.Count > 0)
                            Passedcount = Convert.ToString(dtnew.Rows[0]["Passedcount"]).Trim();
                    }

                    double.TryParse(Passedcount, out pc);
                    double.TryParse(appearedcount, out ac);
                    if (ac == 0)
                        continue;
                    spreadDet.Sheets[0].RowCount++;
                    if (pc >= 0 && ac > 0)
                        outof100 = Math.Round((pc / ac) * 100, 2, MidpointRounding.AwayFromZero);
                    string passPercetage = string.Format("{0:0.00}", outof100);
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        if (col == 0)
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(++rowcount);
                        else if (col == 3)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = ac.ToString();
                        }
                        else if (col == 4)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = pc.ToString();
                        }
                        else if (col == 5)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].CellType = new FarPoint.Web.Spread.TextCellType();
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = passPercetage;

                        }
                        else
                        {
                            if (ddlReport.SelectedValue == "2")
                            {
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][col - 1]);
                            }
                            else
                            {
                                if (col == 1)
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][col - 1]);
                            }
                        }
                    }
                }

                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;

                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 5].Width = 50;
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                spreadDet.SaveChanges();
                ShowReport.Visible = true;
                print.Visible = true;
            }
            if (spreadDet.Sheets[0].RowCount == 0)
            {
                lblAlertMsg.Text = "No Record Found!";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch { }
    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Student Alumni  Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "Alumni1.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }

    #endregion

}