using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Globalization;
using System.Configuration;

public partial class CoeMod_StudentWiseReport : System.Web.UI.Page
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
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    static byte roll = 0;

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
    string examMonth = string.Empty;
    string qryExamMonth = string.Empty;

    InsproDirectAccess dirAcc = new InsproDirectAccess();

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
            else
            {
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }
            if (!IsPostBack)
            {
                ShowReport.Visible = false;
                Bindcollege();
                BindRightsBaseBatch();
                BindDegree();
                bindbranch();
                BindRightsBasedSectionDetail();
                BindExamYear();
                BindExamMonth();
            }
        }
        catch (Exception ex)
        {
        }
    }

    #region college

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
        }
    }

    #endregion

    #region batch

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            qryBatchYear = string.Empty;
            collegeCode = string.Empty;
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

                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }

            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where ISNULL(batch_year,'0')<>'0' and batch_year<>'' and Batch_Year<>-1 " + qryCollegeCode + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            List<int> lstBatchYear = new List<int>();
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                lstBatchYear = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatchYear.Count > 0)
                    qryBatchYear = " and r.Batch_Year in(" + string.Join(",", lstBatchYear.ToArray()) + ")";
                //ddlBatch.DataSource = dsBatch;
                //ddlBatch.DataTextField = "Batch_year";
                //ddlBatch.DataValueField = "Batch_year";
                //ddlBatch.DataBind();
                //ddlBatch.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                qry = "select distinct r.Batch_Year from Registration r where ISNULL(r.Batch_Year,'0')<>'0' and r.Batch_Year<>-1 " + qryCollegeCode + qryBatchYear + " order by r.Batch_Year desc";//and r.college_code in(" + collegeCodes + ") 

                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_Year";
                ddlBatch.DataValueField = "Batch_Year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code='" + collegecode + "' order by batch_year desc";
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
        string college_code = Convert.ToString(ddlCollege.SelectedValue).Trim();
        string query = string.Empty;
        ddlDegree.Items.Clear();
        string usercode = Convert.ToString(Session["usercode"]).Trim();
        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
        }
        DataSet ds = new DataSet();
        ds.Clear();
        ds = da.select_method_wo_parameter(query, "Text");
        // DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            // ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
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
            ht.Clear();
            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string singleuser = Convert.ToString(Session["single_user"]).Trim();
            string group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            string course_id = string.Empty;// ddlDegree.SelectedValue.ToString();
            if (ddlDegree.Items.Count > 0)
            {
                course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
                string query = string.Empty;
                if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
                }
                else
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
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
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    #region ExamYear

    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
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

    #endregion

    #region ExamMonth

    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
            }
            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examYear))
                        {
                            examYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            examYear += ",'" + li.Value + "'";
                        }
                    }
                }
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

    #endregion

    private void BindSection()
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    public void BindRightsBasedSectionDetail()
    {
        batchYear = string.Empty;
        collegeCode = string.Empty;
        degreeCode = string.Empty;
        semester = string.Empty;
        string sections = string.Empty;

        string qrySection = string.Empty;
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
        if (!string.IsNullOrEmpty(sections.Trim()) && sections.Trim().ToLower() != "all")
        {
            qrySection = " and ltrim(rtrim(isnull(r.sections,''))) in(" + sections + ") ";
            qrySection1 = " and ltrim(rtrim(isnull(srh.sections,''))) in(" + sections + ") ";
        }
        else
        {
            qrySection = string.Empty;
            qrySection1 = string.Empty;
        }
        qrySection = string.Empty;
        qrySection1 = string.Empty;
        if (!string.IsNullOrEmpty(qryCollegeCode1) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryDegreeCode1) && !string.IsNullOrEmpty(qryBatchYear1))
        {
            qry = "select distinct case when ltrim(rtrim(isnull(r.sections,'')))<>'' then ltrim(rtrim(isnull(r.sections,''))) else case when ltrim(rtrim(isnull(r.sections,'')))='' then 'Empty' end end as sections, case when ltrim(rtrim(isnull(r.sections,'')))<>'' then ltrim(rtrim(isnull(r.sections,''))) else '' end SecValues  from Registration r  where ltrim(rtrim(isnull(r.sections,'')))<>'-1' and ltrim(rtrim(isnull(r.sections,'')))<>'0' and r.DelFlag='0' and r.Exam_Flag<>'debar'  " + qryCollegeCode + qryDegreeCode + qryBatchYear + qrySection + " order by SecValues";
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
            ddlSec.Items.Add(new ListItem("ALL", "ALL"));
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindRightsBaseBatch();
            BindDegree();
            bindbranch();
            BindRightsBasedSectionDetail();
            BindExamYear();
            BindExamMonth();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindDegree();
            bindbranch();
            BindRightsBasedSectionDetail();
            BindExamYear();
            BindExamMonth();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlDegree_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            BindRightsBasedSectionDetail();
            BindExamYear();
            BindExamMonth();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlBranch_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindRightsBasedSectionDetail();
            BindExamYear();
            BindExamMonth();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlSem_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindRightsBasedSectionDetail();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindExamMonth();
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ShowReport.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlSec_SelectedIndexChanged(Object sender, EventArgs e)
    {
    }

    #endregion Index Changed Events

    #region Go

    protected void btnGo_Click(object sender, EventArgs e)
    {
        ShowReport.Visible = false;

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
            }
        }
        if (ddlExamYear.Items.Count == 0)
        {
            lblAlertMsg.Text = "No " + lblexamyear.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        else
        {
            examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(examYear))
            {
                qryExamYear = " and ed.exam_year in(" + examYear + ")";
            }
        }

        if (ddlExamMonth.Items.Count == 0)
        {
            lblAlertMsg.Text = "No " + lblexammonth.Text.Trim() + " Were Found";
            divPopAlert.Visible = true;
            return;
        }
        else
        {
            examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
            if (!string.IsNullOrEmpty(examMonth))
            {
                qryExamMonth = " and ed.exam_month in(" + examMonth + ")";
            }
        }
        DataTable dtCGPACalculation = new DataTable();


        ds = getdetailsStudentWisereport();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(examMonth))
            {
                dicSQLParameter.Clear();
                dicSQLParameter.Add("@examYear", examYear);
                dicSQLParameter.Add("@examMonth", examMonth);
                dicSQLParameter.Add("@batchYear", batchYear);
                dicSQLParameter.Add("@degreeCode", degreeCode);
                //dicSQLParameter.Add("@section", degreeCode);
                dtCGPACalculation = storeAcc.selectDataTable("uspCGPACalculation", dicSQLParameter);
            }
            loadspread(ds, dtCGPACalculation);
        }
        else
        {
            lblAlertMsg.Text = "No Record Found";
            divPopAlert.Visible = true;
            return;
        }
    }

    #endregion

    #region PopupAlert

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch
        {


        }
    }

    #endregion

    private DataSet getdetailsStudentWisereport()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value

            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;
            string branch = string.Empty;
            string examyear = string.Empty;
            string exammonth = string.Empty;
            string section = string.Empty;
            string qrySection = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlBatch.Items.Count > 0)
                batch = Convert.ToString(ddlBatch.SelectedValue);
            if (ddlDegree.Items.Count > 0)
                degree = Convert.ToString(ddlDegree.SelectedValue);
            if (ddlBranch.Items.Count > 0)
                branch = Convert.ToString(ddlBranch.SelectedValue);
            if (ddlExamYear.Items.Count > 0)
                examyear = Convert.ToString(ddlExamYear.SelectedValue);
            if (ddlExamMonth.Items.Count > 0)
                exammonth = Convert.ToString(ddlExamMonth.SelectedValue);
            if (ddlSec.Items.Count > 0)
                section = Convert.ToString(ddlSec.SelectedItem.Text);
            if (!string.IsNullOrEmpty(section) && section.ToLower() != "all")
                qrySection = " and ltrim(rtrim(isnull(r.sections,'Empty')))='" + section + "'";
            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(branch))
            {
                selQ = "select distinct r.serialno,r.App_No,r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Type,r.Stud_Name,case a.sex when 0 then 'Male' when 1 then 'Female' else 'Transgender' end Sex,Count(Distinct s.subject_code) as AllotedSubject from subject s,sub_sem ss,syllabus_master sm,Registration r,applyn a,subjectChooser sc where r.App_No=a.app_no and sc.roll_no=r.Roll_No and sc.subject_no=s.subject_no and r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year  and s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and sm.Batch_Year in('" + batch + "') and sm.degree_code in('" + branch + "') " + qrySection + " and r.DelFlag='0' and r.Exam_Flag<>'debar' group by r.serialno,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,Sex,r.Reg_No " + orderByStudents(collegecode, "r");
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");
            }

            #endregion
        }
        catch (Exception ex)
        { }
        return dsload;
    }

    private void loadspread(DataSet ds, DataTable dtCGPACalculation)
    {
        try
        {
            ShowReport.Visible = false;
            //RollAndRegSettings();
            DataTable dtStudentWisePerform = new DataTable();
            dtStudentWisePerform.Columns.Add("SNo", typeof(string));
            dtStudentWisePerform.Columns.Add("App No", typeof(string));
            dtStudentWisePerform.Columns.Add("Roll No", typeof(string));
            dtStudentWisePerform.Columns.Add("Admit No", typeof(string));
            dtStudentWisePerform.Columns.Add("Reg No", typeof(string));
            dtStudentWisePerform.Columns.Add("Student Type", typeof(string));
            dtStudentWisePerform.Columns.Add("Student Name", typeof(string));
            dtStudentWisePerform.Columns.Add("Sex", typeof(string));
            dtStudentWisePerform.Columns.Add("No Of Papers", typeof(int));
            dtStudentWisePerform.Columns.Add("No Of Appeared", typeof(int));
            dtStudentWisePerform.Columns.Add("No Of Passed", typeof(int));
            dtStudentWisePerform.Columns.Add("No Of Attempt", typeof(int));
            dtStudentWisePerform.Columns.Add("With Attempts", typeof(int));
            dtStudentWisePerform.Columns.Add("Without Attempts", typeof(int));
            dtStudentWisePerform.Columns.Add("Pass Pecentage", typeof(decimal));
            DataRow drStudentWisePerform;
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
            int rowcount = 0;
            bool boolroll = false;
            int rollno = 0;
            int regno = 0;
            int admNo = 0;
            //RollAndRegSettings();
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dtStudentWisePerform.Columns.Count; row++)
                {
                    spreadDet.Sheets[0].ColumnCount++;
                    string col = Convert.ToString(dtStudentWisePerform.Columns[row].ColumnName);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Text = col;

                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[row].Visible = true;
                    spreadDet.Sheets[0].Columns[row].Width = ((row == 6) ? 250 : (row == 0 || row == 1) ? 50 : (row < 9) ? 130 : 80);
                    if (col == "App No")
                    {
                        spreadDet.Sheets[0].Columns[row].Visible = false;
                    }
                    switch (col)
                    {
                        case "Admit No":
                            spreadDet.Sheets[0].Columns[row].Width = 150;
                            spreadDet.Sheets[0].Columns[row].Visible = isAdmissionNoVisible;
                            admNo = Convert.ToInt32(row);
                            boolroll = true;
                            break;
                        case "Roll No":
                            spreadDet.Sheets[0].Columns[row].Width = 110;
                            spreadDet.Sheets[0].Columns[row].Visible = isRollNoVisible;
                            rollno = Convert.ToInt32(row);
                            boolroll = true;
                            break;
                        case "Reg No":
                            spreadDet.Sheets[0].Columns[row].Width = 110;
                            spreadDet.Sheets[0].Columns[row].Visible = isRegNoVisible;
                            regno = Convert.ToInt32(row);
                            boolroll = true;
                            break;
                        case "Student Type":
                            spreadDet.Sheets[0].Columns[row].Width = 100;
                            spreadDet.Sheets[0].Columns[row].Visible = isStudentTypeVisible;
                            break;

                    }
                }
                //if (boolroll)//roll ,reg and admission no hide
                //    spreadColumnVisible(rollno, regno, admNo);
            }

            DataRow drow;
            int studentwisereport = 0;
            rowcount = 0;
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string rollNo = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]).Trim();
                    string regNo = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]).Trim();
                    string appNo = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]).Trim();

                    string examyear = Convert.ToString(ddlExamYear.SelectedValue);
                    string examMonth = Convert.ToString(ddlExamMonth.SelectedValue);

                    //qry = "select case when ISNULL(Tab1.roll_no,'')<>'' then ISNULL(Tab1.roll_no,'')  when ISNULL(Tab2.roll_no,'')<>'' then ISNULL(Tab2.roll_no,'') when ISNULL(Tab3.roll_no,'')<>'' then ISNULL(Tab3.roll_no,'') when ISNULL(Tab4.roll_no,'')<>'' then ISNULL(Tab4.roll_no,'') when ISNULL(Tab5.roll_no,'')<>'' then ISNULL(Tab5.roll_no,'') when ISNULL(Tab6.roll_no,'')<>'' then ISNULL(Tab6.roll_no,'') end roll_no,SUM(ISNULL(Tab1.TotalSubjectCount,'0')) as TotalSubjectCount,SUM(ISNULL(Tab2.PassedCount,'0')) as PassedCount,SUM(ISNULL(Tab3.FailedCount,'0')) as FailedCount,SUM(ISNULL(Tab4.CurrentArrearCount,'0')) as CurrentArrearCount,SUM(ISNULL(Tab6.WithOutArrear,'0')) as WithOutArrear,SUM(ISNULL(Tab5.WithArrear,'0')) as WithArrear from ((select sc.roll_no,count(distinct s.subject_code) as TotalSubjectCount from subjectchooser sc,subject s,registration r,syllabus_master sm,sub_sem ss where ss.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=ss.subType_no and sm.syll_code=s.syll_code and sm.degree_code=r.degree_code  and sc.roll_no=r.roll_no and s.subject_no=sc.subject_no  and sc.roll_no in('" + rollNo + "')  group by sc.roll_no) as Tab1 full join  (Select sc.roll_no,count(distinct s.subject_code) as PassedCount from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and sm.syll_code=ss.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='pass' and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no  and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  and sc.roll_no in('" + rollNo + "')  and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) group by sc.roll_no) as Tab2 on Tab1.roll_no=Tab2.roll_no full join (Select sc.roll_no,count(distinct s.subject_code) as FailedCount from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail' and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no  and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  and sc.roll_no in('" + rollNo + "')  and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) group by sc.roll_no) as Tab3 on Tab2.roll_no=Tab3.roll_no full join (select sc.roll_no,count(distinct s.subject_code) as CurrentArrearCount from mark_entry m,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and  s.syll_code=sm.syll_code and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.result<>'Pass' and s.subject_code not in(select distinct s.subject_code from mark_entry m1,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed1 where ed1.exam_code=m.exam_code and ed1.batch_year=sm.Batch_Year and sm.degree_code=ed1.degree_code and s.syll_code=sm.syll_code and s.subject_no=m1.subject_no and s.subject_no=sc.subject_no and m1.roll_no=sc.roll_no and m1.subject_no=sc.subject_no and m1.roll_no=m.roll_no and m1.result='Pass' and ISNULL(ed1.Exam_Month,'')<>'' and ISNULL(ed1.Exam_year,'')<>'' and ISNULL(ed1.Exam_Month,'')<>'-1' and ISNULL(ed1.Exam_year,'')<>'-1' and CAST(CONVERT(varchar(20),ed1.Exam_Month)+'/01/'+CONVERT(varchar(20),ed1.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) and sc.roll_no in('" + rollNo + "')  ) and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' and sc.roll_no in('" + rollNo + "') and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) group by sc.roll_no) as Tab4 on Tab3.roll_no=Tab4.roll_no full join (select sc.roll_no,count(distinct s.subject_code) as WithArrear from mark_entry m,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and  s.syll_code=sm.syll_code and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.result='Pass' and s.subject_code in(select distinct s.subject_code from mark_entry m1,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed1 where ed1.exam_code=m.exam_code and ed1.batch_year=sm.Batch_Year and sm.degree_code=ed1.degree_code and  s.syll_code=sm.syll_code and s.subject_no=m1.subject_no and s.subject_no=sc.subject_no and m1.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.roll_no=m1.roll_no  and sc.roll_no in('" + rollNo + "')  and m1.result='fail' and ISNULL(ed1.Exam_Month,'')<>'' and ISNULL(ed1.Exam_year,'')<>'' and ISNULL(ed1.Exam_Month,'')<>'-1' and ISNULL(ed1.Exam_year,'')<>'-1' and CAST(CONVERT(varchar(20),ed1.Exam_Month)+'/01/'+CONVERT(varchar(20),ed1.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime))  and sc.roll_no in('" + rollNo + "')  and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) group by sc.roll_no) as Tab5 on Tab5.roll_no=Tab4.roll_no full join (select sc.roll_no,count(distinct s.subject_code) as WithOutArrear from mark_entry m,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and s.syll_code=sm.syll_code and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and m.result='Pass' and s.subject_code not in(select distinct s.subject_code from mark_entry m1,subjectChooser sc,syllabus_master sm,subject s,Exam_Details ed1 where ed1.exam_code=m.exam_code and ed1.batch_year=sm.Batch_Year and sm.degree_code=ed1.degree_code and s.syll_code=sm.syll_code and s.subject_no=m1.subject_no and s.subject_no=sc.subject_no and m1.roll_no=sc.roll_no and m1.subject_no=sc.subject_no  and m.roll_no=m1.roll_no  and m1.result='fail'  and ISNULL(ed1.Exam_Month,'')<>'' and ISNULL(ed1.Exam_year,'')<>'' and ISNULL(ed1.Exam_Month,'')<>'-1' and ISNULL(ed1.Exam_year,'')<>'-1'  and sc.roll_no in('" + rollNo + "')  and CAST(CONVERT(varchar(20),ed1.Exam_Month)+'/01/'+CONVERT(varchar(20),ed1.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime))  and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1'  and sc.roll_no in('" + rollNo + "')  and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime) group by sc.roll_no) as Tab6 on Tab5.roll_no=Tab6.roll_no) group by case when ISNULL(Tab1.roll_no,'')<>'' then ISNULL(Tab1.roll_no,'')  when ISNULL(Tab2.roll_no,'')<>'' then ISNULL(Tab2.roll_no,'') when ISNULL(Tab3.roll_no,'')<>'' then ISNULL(Tab3.roll_no,'') when ISNULL(Tab4.roll_no,'')<>'' then ISNULL(Tab4.roll_no,'') when ISNULL(Tab5.roll_no,'')<>'' then ISNULL(Tab5.roll_no,'') when ISNULL(Tab6.roll_no,'')<>'' then ISNULL(Tab6.roll_no,'') end";

                    DataTable dtStudOverAllResult = new DataTable();

                    dicSQLParameter.Clear();
                    dicSQLParameter.Add("@rollNo", rollNo.Trim().Replace("'", ""));
                    dicSQLParameter.Add("@examyear", examyear.Trim().Replace("'", ""));
                    dicSQLParameter.Add("@examMonth", examMonth.Trim().Replace("'", ""));
                    dtStudOverAllResult = storeAcc.selectDataTable("uspCOEStudentWisePassPercentage", dicSQLParameter);

                    //dicSQLParameter.Clear();
                    //dicSQLParameter.Add("@rollNo", rollNo.Trim().Replace("'", ""));
                    //dicSQLParameter.Add("@examyear", examyear.Trim().Replace("'", ""));
                    //dicSQLParameter.Add("@examMonth", examMonth.Trim().Replace("'", ""));
                    //dtStudOverAllResult = storeAcc.selectDataTable("uspCOEStudentWisePassPercentage", dicSQLParameter);

                    qry = "select top 1 m.roll_no,Count(s.subject_code) as MaxAttempt from Subject s,syllabus_master sm,sub_sem ss,Exam_Details ed,mark_entry m where m.exam_code=ed.exam_code and sm.syll_code=s.syll_code and sm.Batch_Year=ed.batch_year and ed.degree_code=sm.degree_code and sm.syll_code=ss.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.subject_no=m.subject_no  and ISNULL(ed.Exam_Month,'')<>'' and ISNULL(ed.Exam_year,'')<>'' and ISNULL(ed.Exam_Month,'')<>'-1' and ISNULL(ed.Exam_year,'')<>'-1' and m.external_mark is not null and m.total is not null and m.result is not null and ISNULL(m.external_mark,'0')>=0 and m.result not in('aaa','absent','ab') and m.roll_no='" + rollNo + "' and CAST(CONVERT(varchar(20),ed.Exam_Month)+'/01/'+CONVERT(varchar(20),ed.Exam_year) as Datetime)<=CAST(CONVERT(varchar(20),'" + examMonth + "')+'/01/'+CONVERT(varchar(20),'" + examyear + "') as Datetime)  group by m.roll_no,s.subject_code order by Count(s.subject_code) desc";
                    DataTable dtmaxattempt = new DataTable();
                    dtmaxattempt = dirAcc.selectDataTable(qry);
                    drStudentWisePerform = dtStudentWisePerform.NewRow();

                    string Appeared = "0";
                    string Passed = "0";
                    string maxAttempt = "1";
                    string WithOutAttempt = "0";
                    string WithAttempt = "0";
                    string passPercentage = "0.00";
                    string currentarrearcount = "0";
                    int arrearcount = 0;
                    double pc = 0, ac = 0;
                    double outof100 = 0;
                    outof100 = 0;
                    if (dtmaxattempt.Rows.Count > 0)
                    {
                        maxAttempt = Convert.ToString(dtmaxattempt.Rows[0]["MaxAttempt"]).Trim();
                    }
                    DataView dvCgpaCalculation = new DataView();
                    if (dtCGPACalculation.Rows.Count > 0)
                    {
                        dtCGPACalculation.DefaultView.RowFilter = "App_No='" + appNo + "'";
                        dvCgpaCalculation = dtCGPACalculation.DefaultView;
                    }
                    if (dvCgpaCalculation.Count > 0)
                    {
                        string cgpa = Convert.ToString(dvCgpaCalculation[0]["GPAPercentage"]).Trim();
                        double.TryParse(cgpa, out outof100);
                        passPercentage = string.Format("{0:0.00}", outof100);
                    }
                    if (dtStudOverAllResult.Rows.Count > 0)
                    {
                        Appeared = Convert.ToString(dtStudOverAllResult.Rows[0]["AppearedSubject"]).Trim();
                        Passed = Convert.ToString(dtStudOverAllResult.Rows[0]["PassedCount"]).Trim();
                        // maxAttempt = Convert.ToString(dtStudOverAllResult.Rows[0]["TotalSubjectCount"]).Trim();
                        WithOutAttempt = Convert.ToString(dtStudOverAllResult.Rows[0]["WithOutArrear"]).Trim();
                        WithAttempt = Convert.ToString(dtStudOverAllResult.Rows[0]["WithArrear"]).Trim();
                        currentarrearcount = Convert.ToString(dtStudOverAllResult.Rows[0]["CurrentArrearCount"]).Trim();
                        int.TryParse(currentarrearcount, out arrearcount);
                        double.TryParse(Passed, out pc);
                        double.TryParse(Appeared, out ac);

                        //if (pc >= 0 && ac > 0)
                        //    outof100 = Math.Round((pc / ac) * 100, 2, MidpointRounding.AwayFromZero);
                        //passPercentage = string.Format("{0:0.00}", outof100);

                    }
                    if (arrearcount == 0)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            drStudentWisePerform[col] = Convert.ToString(ds.Tables[0].Rows[row][col]).Trim();
                        }
                        drStudentWisePerform["No Of Appeared"] = Appeared;
                        drStudentWisePerform["No Of Passed"] = Passed;
                        drStudentWisePerform["No Of Attempt"] = maxAttempt;
                        drStudentWisePerform["Without Attempts"] = WithOutAttempt;
                        drStudentWisePerform["With Attempts"] = WithAttempt;
                        drStudentWisePerform["Pass Pecentage"] = passPercentage;
                        dtStudentWisePerform.Rows.Add(drStudentWisePerform);
                    }
                }

                dtStudentWisePerform.DefaultView.Sort = "No Of Attempt asc,With Attempts asc,Pass Pecentage desc";
                DataTable dtNewstudentwise = dtStudentWisePerform.DefaultView.ToTable();
                int sno = 0;
                if (dtNewstudentwise.Rows.Count > 0)
                {
                    foreach (DataRow drPerfRow in dtNewstudentwise.Rows)
                    {
                        spreadDet.Sheets[0].RowCount++;
                        sno++;
                        string rollNo = Convert.ToString(drPerfRow["Roll No"]).Trim();
                        string appNo = Convert.ToString(drPerfRow["App No"]).Trim();
                        string admitNo = Convert.ToString(drPerfRow["Admit No"]).Trim();
                        string regNo = Convert.ToString(drPerfRow["Reg No"]).Trim();
                        string studentType = Convert.ToString(drPerfRow["Student Type"]).Trim();
                        string studentName = Convert.ToString(drPerfRow["Student Name"]).Trim();
                        string Sex = Convert.ToString(drPerfRow["Sex"]).Trim();
                        string noOfPapers = Convert.ToString(drPerfRow["No Of Papers"]).Trim();
                        string noOfAppears = Convert.ToString(drPerfRow["No Of Appeared"]).Trim();
                        string noOfPassed = Convert.ToString(drPerfRow["No Of Passed"]).Trim();
                        string noOfAttempt = Convert.ToString(drPerfRow["No Of Attempt"]).Trim();
                        string withAttempt = Convert.ToString(drPerfRow["With Attempts"]).Trim();
                        string withoutAttempt = Convert.ToString(drPerfRow["Without Attempts"]).Trim();
                        string passPercentage = Convert.ToString(drPerfRow["Pass Pecentage"]).Trim();
                        //for (int col = 0; col < dtNewstudentwise.Columns.Count; col++)
                        //{

                        //}
                        //for (int col = 9; col < 15; col++)
                        //{
                        //    if (col == 9)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Appeared;
                        //    else if (col == 10)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Passed;
                        //    else if (col == 11)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = maxAttempt;
                        //    else if (col == 12)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = WithAttempt;
                        //    else if (col == 13)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = WithOutAttempt;
                        //    else if (col == 14)
                        //        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = passPercentage;
                        //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                        //}

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].CellType = txtCell;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].CellType = txtCell;

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = appNo;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = rollNo;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = admitNo;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = regNo;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = studentType;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = studentName;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Sex;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = noOfPapers;

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = noOfAppears;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Text = noOfPassed;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].Text = noOfAttempt;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].Text = withAttempt;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].Text = withoutAttempt;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].Text = passPercentage;

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 11].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 12].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 13].Locked = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 14].Locked = true;


                    }

                    spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                    spreadDet.SaveChanges();
                    ShowReport.Visible = true;
                    print.Visible = true;
                }
                else
                {
                    lblAlertMsg.Text = "No Record Found";
                    divPopAlert.Visible = true;
                }

            }
            else
            {
                lblAlertMsg.Text = "No Record Found";
                divPopAlert.Visible = true;
            }
        }
        catch { }
    }

    #region roll,reg,admission setting

    //private void RollAndRegSettings()
    //{
    //    try
    //    {
    //        DataSet dsl = new DataSet();
    //        string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
    //        dsl = d2.select_method_wo_parameter(Master1, "text");
    //        Session["Rollflag"] = "0";
    //        Session["Regflag"] = "0";
    //        Session["Admission"] = "0";

    //        if (dsl.Tables[0].Rows.Count > 0)
    //        {
    //            for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
    //            {
    //                if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
    //                {
    //                    Session["Rollflag"] = "1";
    //                }
    //                if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
    //                {
    //                    Session["Regflag"] = "1";
    //                }
    //                if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
    //                {
    //                    Session["Admission"] = "1";
    //                }
    //            }
    //            settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
    //        }
    //    }
    //    catch { }
    //}

    //private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    //{
    //    // Tuple<byte, byte>
    //    string rollval = rollvalue;
    //    string regval = regvalue;
    //    string addVal = addmis;
    //    try
    //    {
    //        if (rollval != "" && regval != "")
    //        {
    //            if (rollval == "0" && regval == "0" && addVal == "0")
    //                roll = 0;
    //            else if (rollval == "1" && regval == "1" && addVal == "1")
    //                roll = 1;
    //            else if (rollval == "1" && regval == "0" && addVal == "0")
    //                roll = 2;
    //            else if (rollval == "0" && regval == "1" && addVal == "0")
    //                roll = 3;
    //            else if (rollval == "0" && regval == "0" && addVal == "1")
    //                roll = 4;
    //            else if (rollval == "1" && regval == "1" && addVal == "0")
    //                roll = 5;
    //            else if (rollval == "0" && regval == "1" && addVal == "1")
    //                roll = 6;
    //            else if (rollval == "1" && regval == "0" && addVal == "1")
    //                roll = 7;
    //        }
    //    }
    //    catch { }
    //    // return new Tuple<byte, byte>(roll,reg);

    //}

    //protected void spreadColumnVisible(int rollNo, int regNo, int admNo)
    //{
    //    try
    //    {
    //        #region
    //        if (roll == 0)
    //        {
    //            spreadDet.Columns[rollNo].Visible = true;
    //            spreadDet.Columns[regNo].Visible = true;
    //            spreadDet.Columns[admNo].Visible = true;
    //        }
    //        else if (roll == 1)
    //        {

    //            spreadDet.Columns[rollNo].Visible = true;
    //            spreadDet.Columns[regNo].Visible = true;
    //            spreadDet.Columns[admNo].Visible = true;
    //        }
    //        else if (roll == 2)
    //        {

    //            spreadDet.Columns[rollNo].Visible = true;
    //            spreadDet.Columns[regNo].Visible = false;
    //            spreadDet.Columns[admNo].Visible = false;

    //        }
    //        else if (roll == 3)
    //        {

    //            spreadDet.Columns[rollNo].Visible = false;
    //            spreadDet.Columns[regNo].Visible = true;
    //            spreadDet.Columns[admNo].Visible = false;
    //        }
    //        else if (roll == 4)
    //        {

    //            spreadDet.Columns[rollNo].Visible = false;
    //            spreadDet.Columns[regNo].Visible = false;
    //            spreadDet.Columns[admNo].Visible = true;
    //        }
    //        else if (roll == 5)
    //        {

    //            spreadDet.Columns[rollNo].Visible = true;
    //            spreadDet.Columns[regNo].Visible = true;
    //            spreadDet.Columns[admNo].Visible = false;
    //        }
    //        else if (roll == 6)
    //        {

    //            spreadDet.Columns[rollNo].Visible = false;
    //            spreadDet.Columns[regNo].Visible = true;
    //            spreadDet.Columns[admNo].Visible = true;
    //        }
    //        else if (roll == 7)
    //        {

    //            spreadDet.Columns[rollNo].Visible = true;
    //            spreadDet.Columns[regNo].Visible = false;
    //            spreadDet.Columns[admNo].Visible = true;
    //        }
    //        #endregion
    //    }
    //    catch { }
    //}

    #endregion

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.Trim().Replace(" ", "");
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
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            string degreedetails;
            string pagename;
            degreedetails = "Student Wise Pass Pecentage Report";
            pagename = "StudentWiseReport.aspx";

            string Course_Name = Convert.ToString(ddlDegree.SelectedItem);
            string exam_y1 = Convert.ToString(ddlExamYear.SelectedItem.Text);
            string exam_m1 = Convert.ToString(ddlExamMonth.SelectedItem.Value);

            string monthyear1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m1));
            monthyear1 = monthyear1.ToUpper() + " - " + exam_y1;

            degreedetails += "@ " + Course_Name + "  DEGREE EXAMINATION " + monthyear1 + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem);
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void getPrintSettings()
    {
        try
        {

            #region Excel print settings
            string usertype = string.Empty;
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
            return false;
        }
    }

}
