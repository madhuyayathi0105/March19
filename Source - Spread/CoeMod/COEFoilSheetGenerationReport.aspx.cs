#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using Gios.Pdf;
using System.Web;
using System.IO;
using System.Configuration;

#endregion Namespace Declaration

#region Class Definition

public partial class CoeMod_COEFoilSheetGenerationReport : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    bool isSchool = false;
    bool isBasedOnSeatingArrangement = false;
    int selected = 0;

    string collegeCodes = string.Empty;
    string collegeNames = string.Empty;
    string streamNames = string.Empty;
    string courseTypes = string.Empty;
    string eduLevels = string.Empty;
    string courseIds = string.Empty;
    string courseNames = string.Empty;
    string batchYears = string.Empty;
    string degreeCodes = string.Empty;
    string departmentNames = string.Empty;
    string semesters = string.Empty;
    string subjectTypes = string.Empty;
    string subjectNames = string.Empty;
    string subjectNos = string.Empty;
    string subjectCodes = string.Empty;
    string ExamMonth = string.Empty;
    string ExamYear = string.Empty;
    string examDates = string.Empty;
    string examSessions = string.Empty;
    string examDate = string.Empty;
    string examSession = string.Empty;
    string hallNos = string.Empty;
    string hallNo = string.Empty;

    string qry = string.Empty;
    string qryCollege = string.Empty;
    string qryStream = string.Empty;
    string qryEduLevel = string.Empty;
    string qryCourseId = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryDepartment = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySubjectNos = string.Empty;
    string qrySubjectNames = string.Empty;
    string qrySubjectCodes = string.Empty;
    string qryExamMonth = string.Empty;
    string qryExamYear = string.Empty;
    string qryExamDates = string.Empty;
    string qryExamSessions = string.Empty;
    string qryHallNo = string.Empty;
    string qrySubjectFilter = string.Empty;

    #endregion Field Declaration

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
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;
                isBasedOnSeatingArrangement = false;
                chkBasedOnSeating.Checked = false;
                divHall.Visible = false;
                chkRevaluation.Checked = false;
                txtValuation.Text = string.Empty;
                txtValuation.Enabled = true;
                txtExamDate.Enabled = true;
                ddlExamDate.Enabled = true;
                txtExamSession.Enabled = true;
                ddlExamSession.Enabled = true;
                Bindcollege();
                BindStream();
                BindEduLevel();
                //BindDegree();
                BindBranch();
                BindExamYear();
                BindExamMonth();
                BindExamDateSession();
                BindSubject();
                BindHall();
                if (ddlFormat.Items.Count > 0)
                {
                    ddlFormat.SelectedIndex = 0;
                }

            }
        }
        catch (ThreadAbortException tex)
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Page Load

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
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
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Text = "--Select--";
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                cblCollege.DataSource = dsprint;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                checkBoxListselectOrDeselect(cblCollege, true);
                CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindStream()
    {
        try
        {
            collegeCodes = string.Empty;
            ds.Clear();
            cblStream.Items.Clear();
            chkStream.Checked = false;
            txtStream.Text = "--Select--";
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCodes.Trim()))
            {
                string mode = "select distinct ltrim(rtrim(isnull(c.type,''))) as type from course c where c.college_code in (" + collegeCodes + ") and c.type is not null and c.type<>''";
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
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            qryStream = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                string qry = "select distinct c.Edu_Level from course c where c.college_code in(" + collegeCodes + ") " + qryStream + " order by c.Edu_Level desc";
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblBranch.Items.Clear();
            chkBranch.Checked = false;
            txtBranch.Text = "--Select--";
            ds.Clear();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            eduLevels = string.Empty;
            qryCollege = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible == true)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible == true && cblEduLevel.Visible == true)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}

            if (!string.IsNullOrEmpty(collegeCodes))
            {
                //ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCourseId + qryCollege + columnfield + qryStream + qryEduLevel + qryBatch + "order by dg.Degree_Code", "text");//and r.CC='1' and ISNULL(r.isRedo,'0')='0' 
                ds = da.select_method_wo_parameter("select distinct dg.degree_code,c.course_name + ' - '+ dt.dept_name as degree,dg.Acronym,dg.course_id  from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code " + qryCollege + columnfield + qryStream + qryEduLevel + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "degree";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    collegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (ddlEduLevel.Items.Count > 0)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + collegeCode + qryDegreeCode + qryBatch + " order by ed.Exam_year desc";
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    collegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (ddlEduLevel.Items.Count > 0)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            string ExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    ExamYear = " and Exam_year in (" + ExamYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + collegeCode + qryDegreeCode + ExamYear + " order by Exam_Month";
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamDateSession()
    {
        try
        {
            cblExamDate.Items.Clear();
            ddlExamDate.Items.Clear();
            cblExamSession.Items.Clear();
            ddlExamSession.Items.Clear();
            ds.Clear();

            chkExamDate.Checked = false;
            txtExamDate.Text = "--Select--";
            chkExamSession.Checked = false;
            txtExamSession.Text = "--Select--";

            ExamMonth = string.Empty;
            ExamYear = string.Empty;
            collegeCode = string.Empty;
            degreeCodes = string.Empty;
            qryCollege = string.Empty;
            qryDegreeCode = string.Empty;
            qryExamDates = string.Empty;
            qryExamMonth = string.Empty;
            DataTable dtExamDate = new DataTable();
            DataTable dtExamSession = new DataTable();
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and et.coll_code in(" + collegeCodes + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and e.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlExamYear.Items.Count > 0)
            {
                ExamYear = string.Empty;
                foreach (ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    qryExamYear = " and e.Exam_Year in(" + ExamYear + ")";
                }
            }
            if (ddlExamMonth.Items.Count > 0)
            {
                ExamMonth = string.Empty;
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamMonth))
                        {
                            ExamMonth = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamMonth += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamMonth))
                {
                    qryExamMonth = " and e.exam_Month in(" + ExamMonth + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(ExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(ExamYear))
            {
                qry = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,103) as ExamDateDDMMYYYY,LTRIM(RTRIM(ISNULL(et.exam_session,''))) as exam_session,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code " + qryCollege + qryExamYear + qryExamMonth + qryDegreeCode + " order by et.exam_date,exam_session desc";//and  e.exam_Month='11' and e.Exam_Year='2016' and et.coll_code in(15,14,13) and e.degree_code in(52)
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtExamDate = ds.Tables[0].DefaultView.ToTable(true, "ExamDate", "ExamDateDDMMYYYY", "exam_date");
                    dtExamSession = ds.Tables[0].DefaultView.ToTable(true, "exam_session");
                }
            }
            if (dtExamDate.Rows.Count > 0)
            {
                cblExamDate.DataSource = dtExamDate;
                cblExamDate.DataTextField = "ExamDate";
                cblExamDate.DataValueField = "ExamDateDDMMYYYY";
                cblExamDate.DataBind();
                checkBoxListselectOrDeselect(cblExamDate, true);
                CallCheckboxListChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
                txtExamDate.Enabled = true;

                ddlExamDate.DataSource = dtExamDate;
                ddlExamDate.DataTextField = "ExamDate";
                ddlExamDate.DataValueField = "ExamDateDDMMYYYY";
                ddlExamDate.DataBind();
                ddlExamDate.SelectedIndex = 0;
                ddlExamDate.Enabled = true;
            }
            else
            {
                ddlExamDate.Items.Clear();
                cblExamDate.Items.Clear();
                ddlExamDate.Enabled = false;
                chkExamDate.Checked = false;
                txtExamDate.Text = "--Select--";
                txtExamDate.Enabled = false;
            }
            if (dtExamSession.Rows.Count > 0)
            {
                cblExamSession.DataSource = dtExamSession;
                cblExamSession.DataTextField = "exam_session";
                cblExamSession.DataValueField = "exam_session";
                cblExamSession.DataBind();
                checkBoxListselectOrDeselect(cblExamSession, true);
                CallCheckboxListChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
                txtExamSession.Enabled = true;

                ddlExamSession.DataSource = dtExamSession;
                ddlExamSession.DataTextField = "exam_session";
                ddlExamSession.DataValueField = "exam_session";
                ddlExamSession.DataBind();
                ddlExamSession.Enabled = true;
                ddlExamSession.SelectedIndex = 0;
            }
            else
            {
                ddlExamSession.Items.Clear();
                cblExamSession.Items.Clear();
                ddlExamSession.Enabled = false;
                chkExamSession.Checked = false;
                txtExamSession.Text = "--Select--";
                txtExamSession.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindSubject()
    {
        try
        {
            ds.Clear();
            ddlSubejct.Items.Clear();
            cblSubject.Items.Clear();
            chkSubject.Checked = false;
            txtSubject.Text = "--Select--";
            txtSubject.Enabled = false;
            ddlSubejct.Enabled = false;

            ExamMonth = string.Empty;
            ExamYear = string.Empty;
            collegeCode = string.Empty;
            degreeCodes = string.Empty;
            examDates = string.Empty;
            examSessions = string.Empty;

            qryCollege = string.Empty;
            qryDegreeCode = string.Empty;
            qryExamDates = string.Empty;
            qryExamMonth = string.Empty;
            qryExamDates = string.Empty;
            qryExamSessions = string.Empty;
            isBasedOnSeatingArrangement = false;
            string qryCollege1 = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and et.coll_code in(" + collegeCodes + ")";
                    //qryCollege1 = " and ed.coll_code in(" + collegeCodes + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlExamYear.Items.Count > 0)
            {
                ExamYear = string.Empty;
                foreach (ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    qryExamYear = " and ed.Exam_year in(" + ExamYear + ")";
                }
            }
            if (ddlExamMonth.Items.Count > 0)
            {
                ExamMonth = string.Empty;
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamMonth))
                        {
                            ExamMonth = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamMonth += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamMonth))
                {
                    qryExamMonth = " and ed.Exam_month in(" + ExamMonth + ")";
                }
            }
            if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
            {
                examDates = getCblSelectedValue(cblExamDate);
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDates = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                }
            }
            else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
            {
                examDates = string.Empty;
                foreach (ListItem li in ddlExamDate.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examDates))
                        {
                            examDates = "'" + li.Value + "'";
                        }
                        else
                        {
                            examDates += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDates = " and convert(varchar(20),et.exam_date,103) in(" + examDates + ")";
                }
            }
            if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
            {
                examSessions = getCblSelectedValue(cblExamSession);
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSessions = " and et.Exam_Session in(" + examSessions + ")";
                }
            }
            else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
            {
                examSessions = string.Empty;
                foreach (ListItem li in ddlExamSession.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examSessions))
                        {
                            examSessions = "'" + li.Value + "'";
                        }
                        else
                        {
                            examSessions += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSessions = " and et.Exam_Session in(" + examSessions + ")";
                }
            }
            if (chkBasedOnSeating.Checked)
            {
                isBasedOnSeatingArrangement = true;
            }
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(ExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(examDates) && !string.IsNullOrEmpty(qryExamDates) && !string.IsNullOrEmpty(examSessions) && !string.IsNullOrEmpty(qryExamSessions))
            {
                qry = "select distinct s.subject_code, s.subject_name,et.exam_date,et.exam_session from subject s,exmtt ed,exmtt_det et where et.exam_code=ed.exam_code and et.subject_no=s.subject_no " + qryCollege + qryDegreeCode + qryExamYear + qryExamMonth + qryExamDates + qryExamSessions;
                if (isBasedOnSeatingArrangement)
                {
                    qry = "select distinct s.subject_code, s.subject_name,et.exam_date,et.exam_session from subject s,exmtt ed,exmtt_det et,exam_seating es where et.exam_code=ed.exam_code and et.subject_no=s.subject_no and es.subject_no=et.subject_no and es.degree_code=ed.degree_code and es.edate=et.exam_date and et.exam_session=es.ses_sion " + qryCollege + qryDegreeCode + qryExamYear + qryExamMonth + qryExamDates + qryExamSessions;
                }
                if (chkRevaluation.Checked)
                {
                    qry = "select distinct s.subject_code,s.subject_name from Exam_Details ed,exam_appl_details ead,exam_application ea, subject s where ea.exam_code=ed.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no " + qryDegreeCode + qryExamYear + qryExamMonth + " and ea.Exam_type='2'";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblSubject.DataSource = ds;
                    cblSubject.DataTextField = "subject_name";
                    cblSubject.DataValueField = "subject_code";
                    cblSubject.DataBind();
                    checkBoxListselectOrDeselect(cblSubject, true);
                    CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
                    txtSubject.Enabled = true;

                    ddlSubejct.DataSource = ds;
                    ddlSubejct.DataTextField = "subject_name";
                    ddlSubejct.DataValueField = "subject_code";
                    ddlSubejct.DataBind();
                    ddlSubejct.Enabled = true;
                    ddlSubejct.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindHall()
    {
        try
        {
            if (divHall.Visible && !chkRevaluation.Checked)
            {
                ds.Clear();
                cblHallNo.Items.Clear();
                chkHallNo.Checked = false;
                txtHallNo.Enabled = false;
                txtHallNo.Text = "--Select--";

                ExamMonth = string.Empty;
                ExamYear = string.Empty;
                collegeCode = string.Empty;
                degreeCodes = string.Empty;
                examDates = string.Empty;
                examSessions = string.Empty;
                subjectCodes = string.Empty;

                qryCollege = string.Empty;
                qryDegreeCode = string.Empty;
                qryExamDates = string.Empty;
                qryExamMonth = string.Empty;
                qryExamDates = string.Empty;
                qryExamSessions = string.Empty;
                qrySubjectCodes = string.Empty;
                qrySubjectFilter = string.Empty;
                isBasedOnSeatingArrangement = false;
                if (chkBasedOnSeating.Checked)
                {
                    isBasedOnSeatingArrangement = true;
                }
                if (cblCollege.Items.Count > 0)
                {
                    collegeCodes = getCblSelectedValue(cblCollege);
                    if (!string.IsNullOrEmpty(collegeCodes))
                    {
                        qryCollege = " and r.college_code in(" + collegeCodes + ")";
                    }
                }
                if (cblBranch.Items.Count > 0)
                {
                    degreeCodes = getCblSelectedValue(cblBranch);
                    if (!string.IsNullOrEmpty(degreeCodes))
                    {
                        qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                    }
                }
                if (ddlExamYear.Items.Count > 0)
                {
                    ExamYear = string.Empty;
                    foreach (ListItem li in ddlExamYear.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(ExamYear))
                            {
                                ExamYear = "'" + li.Value + "'";
                            }
                            else
                            {
                                ExamYear += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ExamYear))
                    {
                        qryExamYear = " and ed.Exam_year in(" + ExamYear + ")";
                    }
                }
                if (ddlExamMonth.Items.Count > 0)
                {
                    ExamMonth = string.Empty;
                    foreach (ListItem li in ddlExamMonth.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(ExamMonth))
                            {
                                ExamMonth = "'" + li.Value + "'";
                            }
                            else
                            {
                                ExamMonth += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ExamMonth))
                    {
                        qryExamMonth = " and ed.Exam_month in(" + ExamMonth + ")";
                    }
                }
                if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
                {
                    examDates = getCblSelectedValue(cblExamDate);
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDates = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    }
                }
                else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
                {
                    examDates = string.Empty;
                    foreach (ListItem li in ddlExamDate.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examDates))
                            {
                                examDates = "'" + li.Value + "'";
                            }
                            else
                            {
                                examDates += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDates = " and convert(varchar(20),es.edate,103) in(" + examDates + ")";
                    }
                }
                if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
                {
                    examSessions = getCblSelectedValue(cblExamSession);
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSessions = " and es.ses_sion in(" + examSessions + ")";
                    }
                }
                else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
                {
                    examSessions = string.Empty;
                    foreach (ListItem li in ddlExamSession.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examSessions))
                            {
                                examSessions = "'" + li.Value + "'";
                            }
                            else
                            {
                                examSessions += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSessions = " and es.ses_sion in(" + examSessions + ")";
                    }
                }
                if (cblSubject.Items.Count > 0 && txtSubject.Visible == true)
                {
                    subjectCodes = getCblSelectedValue(cblSubject);
                    if (!string.IsNullOrEmpty(subjectCodes))
                    {
                        qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    }
                }
                else if (ddlSubejct.Items.Count > 0 && ddlSubejct.Visible == true)
                {
                    subjectCodes = string.Empty;
                    foreach (ListItem li in ddlSubejct.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(subjectCodes))
                            {
                                subjectCodes = "'" + li.Value + "'";
                            }
                            else
                            {
                                subjectCodes += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(subjectCodes))
                    {
                        qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                        qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                    }
                }

                if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(ExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(examDates) && !string.IsNullOrEmpty(qryExamDates) && !string.IsNullOrEmpty(examSessions) && !string.IsNullOrEmpty(qryExamSessions))
                {
                    qry = " SELECT distinct es.roomno ,cm.priority FROM exam_seating es,Registration r,class_master cm,subject s,Exam_Details ed where cm.rno=es.roomno and es.regno=r.Reg_No and ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and es.degree_code=ed.degree_code and s.subject_no=es.subject_no " + qryCollege + qryExamMonth + qryExamYear + qryExamDates + qryExamSessions + qryDegreeCode + " order by cm.priority";//((isBasedOnSeatingArrangement) ? qrySubjectCodes : "") +
                    ds.Clear();
                    ds = da.select_method_wo_parameter(qry, "Text");
                    DataTable dtRoomDetails = new DataTable();
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "";
                        if (isBasedOnSeatingArrangement)
                        {
                            ds.Tables[0].DefaultView.RowFilter = qrySubjectFilter;
                        }
                        dtRoomDetails = ds.Tables[0].DefaultView.ToTable();
                    }
                    if (dtRoomDetails.Rows.Count > 0)
                    {
                        cblHallNo.DataSource = dtRoomDetails;
                        cblHallNo.DataTextField = "roomno";
                        cblHallNo.DataValueField = "roomno";
                        cblHallNo.DataBind();
                        checkBoxListselectOrDeselect(cblHallNo, true);
                        CallCheckboxListChange(chkHallNo, cblHallNo, txtHallNo, lblHallNo.Text, "--Select--");
                        txtHallNo.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

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
            darkstyle.ForeColor = Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = Color.Black;

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
            FpSpread1.ActiveSheetView.SelectionBackColor = Color.Transparent;

            if (type == 0)
            {
                FpSpread1.Sheets[0].FrozenRowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 8;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                FpSpread1.Sheets[0].Columns[2].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dept";
                FpSpread1.Sheets[0].Columns[3].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "From";
                FpSpread1.Sheets[0].Columns[5].Width = 150;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "To";
                FpSpread1.Sheets[0].Columns[6].Width = 150;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "degreecode";
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = false;
                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].FrozenRowCount = 1;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name ( Subject Code )";
                FpSpread1.Sheets[0].Columns[2].Width = 600;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Count";
                FpSpread1.Sheets[0].Columns[3].Width = 150;

            }

            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].Columns[0].Visible = true;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

            FpSpread1.Sheets[0].Columns[1].Width = 80;
            FpSpread1.Sheets[0].Columns[1].Locked = false;
            FpSpread1.Sheets[0].Columns[1].Resizable = false;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Resizable = false;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Resizable = false;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

        }
        catch (Exception ex)
        {
        }
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            txtValuation.Text = string.Empty;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            //CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            //BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            txtValuation.Text = string.Empty;
            divMainContents.Visible = false;
            //CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            //CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            BindExamMonth();
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            BindExamDateSession();
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkExamDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblExamDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkExamDate, cblExamDate, txtExamDate, lblExamDate.Text, "--Select--");
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkExamSession_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblExamSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkExamSession, cblExamSession, txtExamSession, lblExamSession.Text, "--Select--");
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
            if (!chkRevaluation.Checked)
                BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
            if (!chkRevaluation.Checked)
                BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSubejct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            if (!chkRevaluation.Checked)
                BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkHallNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxChange(chkHallNo, cblHallNo, txtHallNo, lblHallNo.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblHallNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            CallCheckboxListChange(chkHallNo, cblHallNo, txtHallNo, lblHallNo.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBasedOnSeating_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtValuation.Text = string.Empty;
            BindSubject();
            BindHall();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkRevaluation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            txtExamDate.Enabled = true;
            ddlExamDate.Enabled = true;
            txtExamSession.Enabled = true;
            ddlExamSession.Enabled = true;
            txtValuation.Text = string.Empty;
            if (chkRevaluation.Checked)
            {
                txtExamDate.Enabled = false;
                ddlExamDate.Enabled = false;
                txtExamSession.Enabled = false;
                ddlExamSession.Enabled = false;
            }
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            chkRevaluation.Checked = false;
            txtValuation.Text = string.Empty;
            txtValuation.Enabled = false;
            if (ddlFormat.Items.Count > 0)
            {
                int index = ddlFormat.SelectedIndex;
                switch (index)
                {
                    case 0:
                        txtValuation.Enabled = true;
                        break;
                    case 1:
                        break;
                    case 2:
                        chkRevaluation.Checked = true;
                        break;
                }
            }
            BindExamDateSession();
            if (chkRevaluation.Checked)
            {
                txtExamDate.Enabled = false;
                ddlExamDate.Enabled = false;
                txtExamSession.Enabled = false;
                ddlExamSession.Enabled = false;
            }            
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpPhasing_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpPhasing.SaveChanges();
            int r = FpPhasing.Sheets[0].ActiveRow;
            int j = FpPhasing.Sheets[0].ActiveColumn;
            if (r == 0 && j == 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    if (FpPhasing.Sheets[0].Cells[row, 0].Text != string.Empty)
                    {
                        if (val == 1)
                            FpPhasing.Sheets[0].Cells[row, j].Value = 1;
                        else
                            FpPhasing.Sheets[0].Cells[row, j].Value = 0;
                    }
                }
            }
        }
        catch
        {
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

    #region Button Events

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
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Close

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;

            collegeCodes = string.Empty;
            streamNames = string.Empty;
            eduLevels = string.Empty;
            batchYears = string.Empty;
            courseIds = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            subjectCodes = string.Empty;
            subjectNames = string.Empty;
            subjectNos = string.Empty;
            ExamMonth = string.Empty;
            ExamYear = string.Empty;
            hallNos = string.Empty;
            hallNo = string.Empty;
            examDate = string.Empty;
            examDates = string.Empty;
            examSession = string.Empty;
            examSessions = string.Empty;

            qryCollege = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryBatch = string.Empty;
            qryCourseId = string.Empty;
            qryDegree = string.Empty;
            qryDegreeCode = string.Empty;
            qryDepartment = string.Empty;
            qrySemester = string.Empty;
            qrySubjectNos = string.Empty;
            qrySubjectNames = string.Empty;
            qrySubjectCodes = string.Empty;
            qryExamYear = string.Empty;
            qryExamMonth = string.Empty;
            qryHallNo = string.Empty;
            qryExamDates = string.Empty;
            qryExamSessions = string.Empty;
            isBasedOnSeatingArrangement = false;

            if (chkBasedOnSeating.Checked)
            {
                isBasedOnSeatingArrangement = true;
                //btnPrintFoilSheet.Text = "Phasing Sheet";
            }

            string qryRedoBatch = string.Empty;
            string qryRedoDegreeCode = string.Empty;

            if (cblCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and r.college_code in(" + collegeCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblCollege.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblStream.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible == true)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblEduLevel.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible == true && cblEduLevel.Visible == true)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblEduLevel.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblEduLevel.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                    qryRedoDegreeCode = " and sr.DegreeCode in(" + degreeCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            if (ddlExamYear.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamYear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            ExamYear = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    qryExamYear = " and ed.Exam_Year in(" + ExamYear + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamYear.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }

            if (ddlExamMonth.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamMonth.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(ExamMonth))
                        {
                            ExamMonth += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            ExamMonth = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in(" + ExamMonth + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamMonth.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }

            if (!chkRevaluation.Checked)
            {
                if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
                {
                    examDates = getCblSelectedValue(cblExamDate);
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDates = " and convert(varchar(20),etd.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (ddlExamDate.Items.Count > 0 && ddlExamDate.Visible == true)
                {
                    examDates = string.Empty;
                    foreach (ListItem li in ddlExamDate.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examDates))
                            {
                                examDates = "'" + li.Value + "'";
                            }
                            else
                            {
                                examDates += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examDates))
                    {
                        qryExamDates = " and convert(varchar(20),etd.exam_date,103) in(" + examDates + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblExamDate.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblExamDate.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
                {
                    examSessions = getCblSelectedValue(cblExamSession);
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSessions = " and etd.Exam_Session in(" + examSessions + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (ddlExamSession.Items.Count > 0 && ddlExamSession.Visible == true)
                {
                    examSessions = string.Empty;
                    foreach (ListItem li in ddlExamSession.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(examSessions))
                            {
                                examSessions = "'" + li.Value + "'";
                            }
                            else
                            {
                                examSessions += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(examSessions))
                    {
                        qryExamSessions = " and etd.Exam_Session in(" + examSessions + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblExamSession.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblExamSession.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblSubject.Items.Count > 0 && txtSubject.Visible == true)
            {
                subjectCodes = getCblSelectedValue(cblSubject);
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlSubejct.Items.Count > 0 && ddlSubejct.Visible == true)
            {
                subjectCodes = string.Empty;
                foreach (ListItem li in ddlSubejct.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(subjectCodes))
                        {
                            subjectCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            subjectCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjects.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (divHall.Visible && isBasedOnSeatingArrangement && !chkRevaluation.Checked)
            {
                if (cblHallNo.Items.Count > 0 && txtHallNo.Visible == true)
                {
                    hallNos = getCblSelectedValue(cblHallNo);
                    if (!string.IsNullOrEmpty(hallNos))
                    {
                        qryHallNo = " and room_no in(" + hallNos + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblHallNo.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblHallNo.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
            }

            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth))
            {
                DataSet dsAllStudents = new DataSet();
                DataSet dsStudentStrength = new DataSet();
                DataSet dsAllCourseDetails = new DataSet();

                qry = "select distinct Count(distinct ex.roll_no) as strength,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+')' as SubjectDetails,CONVERT(VARCHAR(50),etd.exam_date,103) exam_date,etd.exam_session,etd.exam_date as Date from exmtt ed,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex,Registration r where ex.roll_no=r.Roll_No and r.degree_code=ed.degree_code and r.Batch_Year=ed.batchFrom and ex.appl_no=ea.appl_no and ed.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + qrySubjectCodes + qryCollege + qryExamSessions + qryExamDates + qryExamMonth + qryExamYear + qryDegreeCode + "  group by s.subject_code,s.subject_name,etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc,s.subject_code";
                if (chkRevaluation.Checked)
                    qry = "select distinct Count(distinct ea.roll_no) as strength,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+')' as SubjectDetails,'' exam_date,'' exam_session,'' as Date,ISNULL(ea.Exam_type,'0') Exam_type from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s,Registration r where r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ea.appl_no=ead.appl_no and ea.exam_code=ed.exam_code and s.subject_no=ead.subject_no and ea.Exam_type='2' " + qrySubjectCodes + qryExamMonth + qryExamYear + qryDegreeCode + qryCollege + " group by s.subject_code,s.subject_name,Exam_type order by s.subject_code ";
                dsStudentStrength = da.select_method_wo_parameter(qry, "Text");

                qry = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.college_code  as coll_code,r.degree_code,r.Batch_Year,r.Current_Semester,s.subject_code,s.subject_name,s.part_type,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,etd.exam_date,etd.exam_session,etd.exam_date as Date from exmtt ed,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex ,Registration r where ex.roll_no=r.Roll_No and r.degree_code=ed.degree_code and r.Batch_Year=ed.batchFrom and ex.appl_no=ea.appl_no and ed.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + qrySubjectCodes + qryCollege + qryExamSessions + qryExamDates + qryExamMonth + qryExamYear + qryDegreeCode + "  order by  r.college_code,r.Batch_Year,r.Degree_code,r.Reg_No,etd.exam_date asc,etd.exam_session desc,s.subject_code";
                if (chkRevaluation.Checked)
                    qry = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.college_code  as coll_code,r.degree_code,r.Batch_Year,r.Current_Semester,s.subject_code,s.subject_name,s.part_type,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,'' exam_date,'' exam_session,'' as Date from Exam_Details ed,subject s,exam_appl_details ead,exam_application ea ,Registration r where ea.roll_no=r.Roll_No and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and ead.appl_no=ea.appl_no and ed.exam_code=ea.exam_code and ead.subject_no=s.subject_no and ea.Exam_type='2' " + qryCollege + qrySubjectCodes + qryExamMonth + qryExamYear + qryDegreeCode + " order by  r.college_code,r.Batch_Year,r.Degree_code,r.Reg_No,s.subject_code";
                dsAllStudents = da.select_method_wo_parameter(qry, "Text");

                qry = "select distinct clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,r.degree_code,r.Reg_no,c.edu_level,c.Course_Name,dt.Dept_Name,dt.dept_acronym from Exam_Details ed,Registration r,Course c,Department dt ,Degree dg,collinfo clg where ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and r.degree_code=dg.Degree_Code and ed.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dg.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and c.college_code=clg.college_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' " + qryCollege + qryDegreeCode + qryExamMonth + qryExamYear;
                qry += " order by c.edu_level desc,r.Reg_no,dt.dept_acronym";
                dsAllCourseDetails = da.select_method_wo_parameter(qry, "text");

                FarPoint.Web.Spread.CheckBoxCellType chkOneByOne = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkSelectAll = new FarPoint.Web.Spread.CheckBoxCellType();
                chkSelectAll.AutoPostBack = true;
                string strength = string.Empty;
                string roomno = string.Empty;
                string sesson = string.Empty;
                string exdate = string.Empty;
                string dept = string.Empty;
                string bun = string.Empty;
                string degrrcode = string.Empty;
                string batchyr = string.Empty;
                string sbjno = string.Empty;
                int sno = 0;
                if (dsStudentStrength.Tables.Count > 0 && dsStudentStrength.Tables[0].Rows.Count > 0)
                {
                    Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
                    chkCellAll.AutoPostBack = true;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    Farpoint.CheckBoxCellType chkCell = new Farpoint.CheckBoxCellType();
                    chkCell.AutoPostBack = false;

                    string collegeCode = string.Empty;
                    string collegeName = string.Empty;
                    string batchYear = string.Empty;
                    string courseId = string.Empty;
                    string deptCode = string.Empty;
                    string degreeCode = string.Empty;
                    string courseType = string.Empty;
                    string eduLevel = string.Empty;
                    string courseName = string.Empty;
                    string departmentName = string.Empty;
                    string departmentAcr = string.Empty;
                    string degreeName = string.Empty;
                    string examCode = string.Empty;
                    string examYear = string.Empty;
                    string examMonth = string.Empty;
                    string monthName = string.Empty;
                    string examMonthYear = string.Empty;
                    string currentSemester = string.Empty;
                    string redoStatus = string.Empty;
                    string maxDuration = string.Empty;
                    int serialNo = 0;

                    Init_Spread(FpPhasing, 1);
                    ht.Clear();
                    FpPhasing.Width = 950;
                    FpPhasing.Visible = true;
                    FpPhasing.Sheets[0].RowCount = 0;
                    FpPhasing.Sheets[0].RowCount++;
                    FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = chkSelectAll;
                    FpPhasing.Sheets[0].SpanModel.Add(FpPhasing.Sheets[0].RowCount - 1, 2, 1, 3);
                    sno = 0;
                    foreach (DataRow drStudentStrength in dsStudentStrength.Tables[0].Rows)
                    {
                        string subjectCode = Convert.ToString(drStudentStrength["subject_code"]).Trim();
                        string subjectName = Convert.ToString(drStudentStrength["subject_name"]).Trim();
                        string subjectDetails = Convert.ToString(drStudentStrength["SubjectDetails"]).Trim();
                        string studentsCont = Convert.ToString(drStudentStrength["strength"]).Trim();
                        //string degreeCodeValue = Convert.ToString(drStudentStrength["degree_code"]).Trim();
                        examDate = Convert.ToString(drStudentStrength["exam_date"]).Trim();
                        examSession = Convert.ToString(drStudentStrength["exam_session"]).Trim();
                        string collCode = string.Empty;// Convert.ToString(drStudentStrength["coll_code"]).Trim();
                        string ddate = Convert.ToString(drStudentStrength["Date"]).Trim();
                        string majorPart = string.Empty;
                        bool isMajor = false;
                        if (!chkRevaluation.Checked)
                        {
                            if (!ht.Contains(examDate + "-" + examSession))
                            {
                                FpPhasing.Sheets[0].RowCount++;
                                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = examDate + " - " + examSession;
                                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#458547");
                                FpPhasing.Sheets[0].Rows[FpPhasing.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#458547");
                                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                ht.Add(examDate + "-" + examSession, examDate + "-" + examSession);
                            }
                        }
                        string regNo = string.Empty;
                        string majorDepartment = string.Empty;
                        DataView dvAllStudent = new DataView();
                        if (dsAllStudents.Tables.Count > 0 && dsAllStudents.Tables[0].Rows.Count > 0)
                        {
                            dsAllStudents.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "' and Date='" + ddate + "' and exam_session='" + examSession + "' ";//and degree_code='" + degreeCodeValue + "'
                            dvAllStudent = dsAllStudents.Tables[0].DefaultView;
                        }
                        if (dvAllStudent.Count > 0)
                        {
                            DataView dvMajorType = new DataView();
                            DataTable dtMajorType = dvAllStudent.ToTable(true, "Part_Type");
                            dtMajorType.DefaultView.RowFilter = "Part_Type='3'";
                            dvMajorType = dtMajorType.DefaultView;
                            if (dvMajorType.Count > 0)
                            {
                                majorPart = "3";
                                isMajor = true;
                            }
                            dvAllStudent.Sort = "Reg_No";
                            List<decimal> list = dvAllStudent.ToTable().AsEnumerable().Select(r => r.Field<decimal>("coll_code")).ToList();
                            collCode = string.Join("','", list.Distinct().ToArray());

                            List<string> lstRegNo = dvAllStudent.ToTable().AsEnumerable().Select(r => r.Field<string>("Reg_No")).ToList();
                            regNo = string.Join("','", lstRegNo.Distinct().ToArray());
                            regNo = "'" + regNo + "'";
                            DataTable dtMajorDepartment = new DataTable();
                            DataTable dtRegNoList = new DataTable();
                            if (dsAllCourseDetails.Tables.Count > 0 && dsAllCourseDetails.Tables[0].Rows.Count > 0)
                            {
                                dsAllCourseDetails.Tables[0].DefaultView.RowFilter = "college_code in('" + collCode + "') and Reg_no in(" + regNo + ") ";//and degree_code='" + degreeCodeValue + "'
                                dtMajorDepartment = dsAllCourseDetails.Tables[0].DefaultView.ToTable(true, "Dept_Name", "dept_acronym");

                                dsAllCourseDetails.Tables[0].DefaultView.RowFilter = "college_code in('" + collCode + "') and Reg_no in(" + regNo + ") ";//and degree_code='" + degreeCodeValue + "'
                                DataView dvRegNo = new DataView();
                                dvRegNo = dsAllCourseDetails.Tables[0].DefaultView;
                                dvRegNo.Sort = "edu_level desc,Reg_no";
                                dtRegNoList = dvRegNo.ToTable(true, "Reg_no", "edu_level", "Dept_Name", "dept_acronym");

                                List<string> lstRegNo1 = dvRegNo.ToTable().AsEnumerable().Select(r => r.Field<string>("Reg_No")).ToList();
                                regNo = string.Join("','", lstRegNo1.Distinct().ToArray());
                                regNo = "'" + regNo + "'";
                            }
                            List<string> lstDept = dtMajorDepartment.AsEnumerable().Select(r => r.Field<string>("Dept_Name")).ToList();
                            majorDepartment = string.Join(",", lstDept.Distinct().ToArray());
                        }
                        sno++;
                        FpPhasing.Sheets[0].RowCount++;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examDate).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(examSession).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].CellType = chkOneByOne;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].Tag = isMajor;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].Note = majorDepartment;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].Locked = false;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectDetails).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(subjectCode).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(subjectName).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(studentsCont).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(regNo).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(collCode).Trim();
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpPhasing.Sheets[0].Cells[FpPhasing.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                    }
                    FpPhasing.SaveChanges();
                    FpPhasing.Sheets[0].PageSize = FpPhasing.Sheets[0].RowCount;
                    FpPhasing.Height = 500;
                    FpPhasing.SaveChanges();
                    FpPhasing.Visible = true;
                    divMainContents.Visible = true;
                }
                else
                {
                    divMainContents.Visible = false;
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
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

    #region Foil Sheet

    private void printFoilSheetOld()
    {
        try
        {
            FpPhasing.SaveChanges();
            string Line1 = string.Empty;
            string Line2 = string.Empty;
            string Line3 = string.Empty;
            string Line4 = string.Empty;
            string Line5 = string.Empty;
            string Line6 = string.Empty;
            string Line7 = string.Empty;
            string Line8 = string.Empty;
            PdfDocument mydocument = new PdfDocument(PdfDocumentFormat.A4);
            PdfPage mypdfpage;

            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);

            Font Fontbold1 = new Font("Algerian", 13, FontStyle.Bold);
            Font Fontbold12 = new Font("Algerian", 12, FontStyle.Regular);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            Font font4smallnew = new Font("Palatino Linotype", 7, FontStyle.Bold);

            bool selected = false;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " college_code in (" + collegeCode + ")";
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any " + lblCollege.Text.Trim();
                divPopAlert.Visible = true;
                return;
            }
            if (FpPhasing.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                    }
                }
            }
            DataSet dsColInfo = da.select_method_wo_parameter("select com_name,college_code,case when ISNULL(com_name,'')<>'' then UPPER(ISNULL(com_name,'')) else UPPER(collname) end+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby,logo1 from collinfo", "Text");
            if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
            {

            }
            Line6 = "STATEMENT OF MARKS";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            int posY = 0;
            bool status = false;
            DataSet dsSubjectAdditionalDetail = da.select_method_wo_parameter("SELECT distinct sm.semester,Replicate('M', sm.semester/1000)+ REPLACE(REPLACE(REPLACE(Replicate('C', sm.semester%1000/100),Replicate('C', 9), 'CM'),Replicate('C', 5), 'D'),Replicate('C', 4), 'CD')+ REPLACE(REPLACE(REPLACE(Replicate('X', sm.semester%100 / 10),Replicate('X', 9),'XC'),Replicate('X', 5), 'L'),Replicate('X', 4), 'XL')+ REPLACE(REPLACE(REPLACE(Replicate('I', sm.semester%10),Replicate('I', 9),'IX'),Replicate('I', 5), 'V'),Replicate('I', 4),'IV') as SemRoman,sm.degree_code,s.subject_code,s.subject_name,s.min_int_marks,s.max_int_marks,s.min_ext_marks,s.max_ext_marks,s.mintotal,s.maxtotal FROM SUBJECT S,syllabus_master sm,sub_sem ss,Registration r where r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no order by sm.semester asc,sm.degree_code,s.subject_code", "text");
            if (selected)
            {
                for (int row = 1; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    string rowno = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Text).Trim();
                    if (sel == 1)
                    {
                        int PageNo = 1;
                        int ToatlPage = 1;
                        status = true;
                        bool pageHas = false;
                        posY = 5;
                        string allRegNo = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Note).Trim();
                        //string degreeCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Tag).Trim();
                        string majorDept = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Note).Trim();

                        string majorPart = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Tag).Trim();
                        bool isMajor = false;
                        bool.TryParse(majorPart, out isMajor);
                        if (isMajor)
                        {
                            if (majorDept.Split(',').Length != 1)
                            {
                                majorDept = string.Empty;
                            }
                        }
                        else
                        {
                            majorDept = string.Empty;
                        }
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Note).Trim();
                        if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                        {
                            DataView dvColege = new DataView();
                            dsColInfo.Tables[0].DefaultView.RowFilter = "college_code in('" + collcode + "')";
                            dvColege = dsColInfo.Tables[0].DefaultView;
                            collcode = Convert.ToString(dsColInfo.Tables[0].Rows[0]["college_code"]).Trim();
                            if (dvColege.Count > 0)
                            {
                                collcode = Convert.ToString(dvColege[0]["college_code"]).Trim();
                                PdfTable table2;
                                PdfImage LogoImage;
                                PdfTablePage tblPage;

                                PdfTextArea pdfLine1;
                                PdfTextArea pdfLine2;
                                PdfTextArea pdfLine3;
                                PdfTextArea pdfLine4;
                                PdfTextArea pdfLine5;
                                PdfTextArea pdfLine6;
                                PdfTextArea pdfLine7;

                                PdfTablePage pdftblPage;
                                PdfTablePage pdftblPageTop;
                                PdfTable pdfTableTop;

                                PdfTable pdfTableHeading;
                                PdfTablePage pdftblPageHeading;

                                PdfTable pdfTableMain;
                                PdfTablePage pdftblPageMain;

                                PdfRectangle pdfRectTopTable;

                                string subjectMinINT = string.Empty;
                                string subjectMaxINT = string.Empty;

                                string subjectMinEXT = string.Empty;
                                string subjectMaxEXT = string.Empty;

                                string subjectMinTOT = string.Empty;
                                string subjectMaxTOT = string.Empty;
                                string subjectSemester = string.Empty;

                                DataTable dtSubjectDetails = new DataTable();
                                if (dsSubjectAdditionalDetail.Tables.Count > 0 && dsSubjectAdditionalDetail.Tables[0].Rows.Count > 0)
                                {
                                    dsSubjectAdditionalDetail.Tables[0].DefaultView.RowFilter = "subject_code='" + subCode + "'";//and degree_code='" + degreeCode + "'
                                    dtSubjectDetails = dsSubjectAdditionalDetail.Tables[0].DefaultView.ToTable();
                                    if (dtSubjectDetails.Rows.Count > 0)
                                    {
                                        subjectMinINT = Convert.ToString(dtSubjectDetails.Rows[0]["min_int_marks"]).Trim();
                                        subjectMaxINT = Convert.ToString(dtSubjectDetails.Rows[0]["max_int_marks"]).Trim();

                                        subjectMinEXT = Convert.ToString(dtSubjectDetails.Rows[0]["min_ext_marks"]).Trim();
                                        subjectMaxEXT = Convert.ToString(dtSubjectDetails.Rows[0]["max_ext_marks"]).Trim();

                                        subjectMinTOT = Convert.ToString(dtSubjectDetails.Rows[0]["mintotal"]).Trim();
                                        subjectMaxTOT = Convert.ToString(dtSubjectDetails.Rows[0]["maxtotal"]).Trim();
                                        subjectSemester = Convert.ToString(dtSubjectDetails.Rows[0]["SemRoman"]).Trim();
                                    }
                                }

                                Line1 = Convert.ToString(dvColege[0]["Line1"]).Trim();
                                try
                                {
                                    string[] affli = Convert.ToString(dvColege[0]["affliatedby"]).Trim().Split('\\');
                                    Line2 = affli[0].Split(',')[0];
                                    Line4 = "(" + affli[2].Split(',')[0] + ")";
                                    Line3 = affli[1].Split(',')[0];
                                }
                                catch { }
                                Line5 = Convert.ToString(dvColege[0]["distr"]).Trim();
                                Line6 = "STATEMENT OF MARKS";
                                Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                subjectName = "SUBJECT TITLE\t\t:\t\t " + subName;
                                subjectCode = "CODE\t\t:\t\t" + subCode;
                                if (RegNo.Length > 0)
                                {
                                    if (RegNo.Length % 25 == 0)
                                    {
                                        ToatlPage = RegNo.Length / 25;
                                    }
                                    else
                                    {
                                        ToatlPage = (RegNo.Length / 25) + 1;
                                    }
                                    pageHas = true;
                                    mypdfpage = mydocument.NewPage();
                                    int rightY = posY;
                                    pdfTableTop = mydocument.NewTable(Fontbold1, 2, 4, 5);
                                    pdfTableTop.SetBorders(Color.Black, 1, BorderType.None);
                                    pdfTableTop.VisibleHeaders = false;
                                    pdfTableTop.Columns[0].SetWidth(80);
                                    pdfTableTop.Columns[1].SetWidth(250);
                                    pdfTableTop.Columns[2].SetWidth(100);
                                    pdfTableTop.Columns[3].SetWidth(60);

                                    pdfTableTop.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableTop.Cell(0, 0).SetContent("Valuation\t:\t");
                                    pdfTableTop.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableTop.Cell(0, 1).SetContent("");
                                    pdfTableTop.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableTop.Cell(0, 2).SetContent("Packet No.");
                                    pdfTableTop.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableTop.Cell(0, 3).SetContent("");

                                    pdftblPageTop = pdfTableTop.CreateTablePage(new PdfArea(mydocument, 10, posY, mydocument.PageWidth - 25, 120));
                                    mypdfpage.Add(pdftblPageTop);
                                    posY += Convert.ToInt32(pdftblPageTop.Area.Height) - 8;
                                    pdfRectTopTable = pdftblPageTop.CellArea(0, 3).ToRectangle(Color.Black, 1.2, Color.White);
                                    mypdfpage.Add(pdfRectTopTable);

                                    MemoryStream memoryStream = new MemoryStream();
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(dvColege[0]["logo1"])))
                                        {
                                            byte[] file = (byte[])dvColege[0]["logo1"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                                {
                                                }
                                                else
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 5, posY, 600);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                        mypdfpage.Add(LogoImage, 5, posY, 600);
                                    }
                                    PdfTextArea pdfSince = new PdfTextArea(font3small, Color.Black, new PdfArea(mydocument, 12, posY + 50, 100, 15), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                    mypdfpage.Add(pdfSince);

                                    pdfLine1 = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydocument, 3, posY, (mydocument.PageWidth - 6), 30), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                    mypdfpage.Add(pdfLine1);

                                    posY += 20;
                                    pdfLine2 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                    mypdfpage.Add(pdfLine2);

                                    posY += 15;
                                    pdfLine3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                    mypdfpage.Add(pdfLine3);

                                    posY += 15;
                                    pdfLine4 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                    mypdfpage.Add(pdfLine4);

                                    posY += 15;
                                    pdfLine5 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                    mypdfpage.Add(pdfLine5);

                                    posY += 15;
                                    pdfLine6 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                    mypdfpage.Add(pdfLine6);

                                    posY += 15;
                                    pdfLine7 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                    mypdfpage.Add(pdfLine7);

                                    posY += 20;
                                    pdfTableHeading = mydocument.NewTable(Fontbold, 4, 9, 8);
                                    pdfTableHeading.SetBorders(Color.Black, 1, BorderType.RowsAndBounds);
                                    pdfTableHeading.VisibleHeaders = false;
                                    pdfTableHeading.Columns[0].SetWidth(200);
                                    pdfTableHeading.Columns[1].SetWidth(8);
                                    pdfTableHeading.Columns[2].SetWidth(100);
                                    pdfTableHeading.Columns[3].SetWidth(200);
                                    pdfTableHeading.Columns[4].SetWidth(8);
                                    pdfTableHeading.Columns[5].SetWidth(150);
                                    pdfTableHeading.Columns[6].SetWidth(100);
                                    pdfTableHeading.Columns[7].SetWidth(8);
                                    pdfTableHeading.Columns[8].SetWidth(180);

                                    pdfTableHeading.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 0).SetContent("Max. Marks");
                                    pdfTableHeading.Cell(0, 0).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 1).SetContent(":");
                                    pdfTableHeading.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 2).SetContent(subjectMaxEXT);

                                    pdfTableHeading.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 3).SetContent("Passing Min.");
                                    pdfTableHeading.Cell(0, 3).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 4).SetContent(":");
                                    pdfTableHeading.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 5).SetContent(subjectMinEXT);

                                    pdfTableHeading.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 6).SetContent("Absent");
                                    pdfTableHeading.Cell(0, 6).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 7).SetContent(":");
                                    pdfTableHeading.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 8).SetContent("AAA");

                                    pdfTableHeading.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 0).SetContent("MAJOR");
                                    pdfTableHeading.Cell(1, 0).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(1, 1).SetContent(":");
                                    pdfTableHeading.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 2).SetContent(majorDept);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(1, 2, 1, 2).Cells)
                                    {
                                        pc.ColSpan = 4;
                                    }

                                    pdfTableHeading.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 6).SetContent("SEM");
                                    pdfTableHeading.Cell(1, 6).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(1, 7).SetContent(":");
                                    pdfTableHeading.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 8).SetContent(subjectSemester);

                                    pdfTableHeading.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(2, 0).SetContent("CODE");
                                    pdfTableHeading.Cell(2, 0).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(2, 1).SetContent(":");
                                    pdfTableHeading.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(2, 2).SetContent(subCode);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(2, 2, 2, 2).Cells)
                                    {
                                        pc.ColSpan = 7;
                                    }

                                    pdfTableHeading.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(3, 0).SetContent("SUBJECT");
                                    pdfTableHeading.Cell(3, 0).SetFont(Fontbold12);
                                    pdfTableHeading.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(3, 1).SetContent(":");
                                    pdfTableHeading.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(3, 2).SetContent(subName);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(3, 2, 3, 2).Cells)
                                    {
                                        pc.ColSpan = 7;
                                    }

                                    pdftblPageHeading = pdfTableHeading.CreateTablePage(new PdfArea(mydocument, 10, posY, mydocument.PageWidth - 25, 120));
                                    mypdfpage.Add(pdftblPageHeading);
                                    PdfLine pdfHLine1 = pdftblPageHeading.CellArea(0, 2).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine1);
                                    PdfLine pdfHLine2 = pdftblPageHeading.CellArea(0, 5).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine2);
                                    PdfLine pdfHLine3 = pdftblPageHeading.CellArea(1, 2).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine3);
                                    posY += Convert.ToInt32(pdftblPageHeading.Area.Height) + 25;

                                    int rOw = 0;
                                    bool newPage = false;
                                    int tempRow = 0;

                                    PdfLine pdfLIneBott;
                                    pdfTableMain = mydocument.NewTable(font2bold, 31, 6, 2);
                                    pdfTableMain.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    pdfTableMain.VisibleHeaders = false;
                                    pdfTableMain.Columns[0].SetWidth(50);
                                    pdfTableMain.Columns[1].SetWidth(50);
                                    pdfTableMain.Columns[2].SetWidth(50);
                                    pdfTableMain.Columns[3].SetWidth(50);
                                    pdfTableMain.Columns[4].SetWidth(50);
                                    pdfTableMain.Columns[5].SetWidth(50);

                                    pdfTableMain.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableMain.Cell(0, 0).SetContent("Register Number");
                                    pdfTableMain.Cell(0, 0).SetCellPadding(2);
                                    foreach (PdfCell pc in pdfTableMain.CellRange(0, 0, 0, 0).Cells)
                                    {
                                        pc.ColSpan = 4;
                                    }

                                    pdfTableMain.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableMain.Cell(0, 4).SetContent("MARKS");
                                    pdfTableMain.Cell(0, 4).SetCellPadding(2);
                                    foreach (PdfCell pc in pdfTableMain.CellRange(0, 4, 0, 4).Cells)
                                    {
                                        pc.ColSpan = 2;
                                    }

                                    for (int roow = rOw; roow < RegNo.Length; roow++)
                                    {
                                        if (rOw % 25 == 0 && rOw != 0 && (RegNo.Length > rOw))
                                        {
                                            PageNo++;
                                            for (int rowQ = tempRow; rowQ < 31; rowQ++)
                                            {
                                                if (rowQ + 1 < 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(5);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 27)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                                }
                                                else if (rowQ + 1 == 30)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 29)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(20);
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(20);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                }
                                                else if (rowQ + 1 < 31)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(6);
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                            }

                                            pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 50, posY, mydocument.PageWidth - 100, 700));
                                            mypdfpage.Add(pdftblPageMain);
                                            posY += Convert.ToInt16(pdftblPageMain.Area.Height) + 15;
                                            //pdfLIneBott= pdftblPageMain.CellArea(28, 0).RightBound(Color.White, 1);
                                            //mypdfpage.Add(pdfLIneBott);

                                            //pdfLIneBott = pdftblPageMain.CellArea(28, 0).LowerBound(Color.White, 1);
                                            //mypdfpage.Add(pdfLIneBott);

                                            //pdfLIneBott = pdftblPageMain.CellArea(28, 3).LowerBound(Color.White, 1);
                                            //mypdfpage.Add(pdfLIneBott);


                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            tempRow = 0;
                                            posY = 5;
                                            tempRow = 0;
                                            rightY = posY;
                                            pdfTableTop = mydocument.NewTable(Fontbold1, 2, 4, 5);
                                            pdfTableTop.SetBorders(Color.Black, 1, BorderType.None);
                                            pdfTableTop.VisibleHeaders = false;
                                            pdfTableTop.Columns[0].SetWidth(80);
                                            pdfTableTop.Columns[1].SetWidth(250);
                                            pdfTableTop.Columns[2].SetWidth(100);
                                            pdfTableTop.Columns[3].SetWidth(60);

                                            pdfTableTop.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableTop.Cell(0, 0).SetContent("Valuation\t:\t");
                                            pdfTableTop.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableTop.Cell(0, 1).SetContent("");
                                            pdfTableTop.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableTop.Cell(0, 2).SetContent("Packet No.");
                                            pdfTableTop.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableTop.Cell(0, 3).SetContent("");

                                            pdftblPageTop = pdfTableTop.CreateTablePage(new PdfArea(mydocument, 10, posY, mydocument.PageWidth - 25, 120));
                                            mypdfpage.Add(pdftblPageTop);
                                            posY += Convert.ToInt32(pdftblPageTop.Area.Height) - 8;
                                            pdfRectTopTable = pdftblPageTop.CellArea(0, 3).ToRectangle(Color.Black, 1.2, Color.White);
                                            mypdfpage.Add(pdfRectTopTable);

                                            memoryStream = new MemoryStream();
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                            {
                                                if (!string.IsNullOrEmpty(Convert.ToString(dvColege[0]["logo1"])))
                                                {
                                                    byte[] file = (byte[])dvColege[0]["logo1"];
                                                    memoryStream.Write(file, 0, file.Length);
                                                    if (file.Length > 0)
                                                    {
                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                        }
                                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                        mypdfpage.Add(LogoImage, 5, posY, 600);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 5, posY, 600);
                                            }
                                            pdfSince = new PdfTextArea(font3small, Color.Black, new PdfArea(mydocument, 12, posY + 50, 100, 15), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                            mypdfpage.Add(pdfSince);

                                            pdfLine1 = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydocument, 3, posY, (mydocument.PageWidth - 6), 30), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                            mypdfpage.Add(pdfLine1);

                                            posY += 20;
                                            pdfLine2 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                            mypdfpage.Add(pdfLine2);

                                            posY += 15;
                                            pdfLine3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                            mypdfpage.Add(pdfLine3);

                                            posY += 15;
                                            pdfLine4 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                            mypdfpage.Add(pdfLine4);

                                            posY += 15;
                                            pdfLine5 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                            mypdfpage.Add(pdfLine5);

                                            posY += 15;
                                            pdfLine6 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                            mypdfpage.Add(pdfLine6);

                                            posY += 15;
                                            pdfLine7 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 3, posY, mydocument.PageWidth - 6, 30), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                            mypdfpage.Add(pdfLine7);

                                            posY += 20;
                                            pdfTableHeading = mydocument.NewTable(Fontbold, 4, 9, 8);
                                            pdfTableHeading.SetBorders(Color.Black, 1, BorderType.RowsAndBounds);
                                            pdfTableHeading.VisibleHeaders = false;
                                            pdfTableHeading.Columns[0].SetWidth(200);
                                            pdfTableHeading.Columns[1].SetWidth(8);
                                            pdfTableHeading.Columns[2].SetWidth(100);
                                            pdfTableHeading.Columns[3].SetWidth(200);
                                            pdfTableHeading.Columns[4].SetWidth(8);
                                            pdfTableHeading.Columns[5].SetWidth(150);
                                            pdfTableHeading.Columns[6].SetWidth(100);
                                            pdfTableHeading.Columns[7].SetWidth(8);
                                            pdfTableHeading.Columns[8].SetWidth(180);

                                            pdfTableHeading.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 0).SetContent("Max. Marks");
                                            pdfTableHeading.Cell(0, 0).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 1).SetContent(":");
                                            pdfTableHeading.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 2).SetContent(subjectMaxEXT);

                                            pdfTableHeading.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 3).SetContent("Passing Min.");
                                            pdfTableHeading.Cell(0, 3).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 4).SetContent(":");
                                            pdfTableHeading.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 5).SetContent(subjectMinEXT);

                                            pdfTableHeading.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 6).SetContent("Absent");
                                            pdfTableHeading.Cell(0, 6).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 7).SetContent(":");
                                            pdfTableHeading.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 8).SetContent("AAA");

                                            pdfTableHeading.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 0).SetContent("MAJOR");
                                            pdfTableHeading.Cell(1, 0).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(1, 1).SetContent(":");
                                            pdfTableHeading.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 2).SetContent(majorDept);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(1, 2, 1, 2).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }

                                            pdfTableHeading.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 6).SetContent("SEM");
                                            pdfTableHeading.Cell(1, 6).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(1, 7).SetContent(":");
                                            pdfTableHeading.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 8).SetContent(subjectSemester);

                                            pdfTableHeading.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(2, 0).SetContent("CODE");
                                            pdfTableHeading.Cell(2, 0).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(2, 1).SetContent(":");
                                            pdfTableHeading.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(2, 2).SetContent(subCode);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(2, 2, 2, 2).Cells)
                                            {
                                                pc.ColSpan = 7;
                                            }

                                            pdfTableHeading.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(3, 0).SetContent("SUBJECT");
                                            pdfTableHeading.Cell(3, 0).SetFont(Fontbold12);
                                            pdfTableHeading.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(3, 1).SetContent(":");
                                            pdfTableHeading.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(3, 2).SetContent(subName);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(3, 2, 3, 2).Cells)
                                            {
                                                pc.ColSpan = 7;
                                            }

                                            pdftblPageHeading = pdfTableHeading.CreateTablePage(new PdfArea(mydocument, 10, posY, mydocument.PageWidth - 25, 120));
                                            mypdfpage.Add(pdftblPageHeading);
                                            pdfHLine1 = pdftblPageHeading.CellArea(0, 2).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine1);
                                            pdfHLine2 = pdftblPageHeading.CellArea(0, 5).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine2);
                                            pdfHLine3 = pdftblPageHeading.CellArea(1, 2).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine3);
                                            posY += Convert.ToInt32(pdftblPageHeading.Area.Height) + 25;

                                            newPage = false;
                                            tempRow = 0;

                                            pdfTableMain = mydocument.NewTable(font2bold, 31, 6, 2);
                                            pdfTableMain.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            pdfTableMain.VisibleHeaders = false;
                                            pdfTableMain.Columns[0].SetWidth(50);
                                            pdfTableMain.Columns[1].SetWidth(50);
                                            pdfTableMain.Columns[2].SetWidth(50);
                                            pdfTableMain.Columns[3].SetWidth(50);
                                            pdfTableMain.Columns[4].SetWidth(50);
                                            pdfTableMain.Columns[5].SetWidth(50);

                                            pdfTableMain.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Cell(0, 0).SetContent("Register Number");
                                            pdfTableMain.Cell(0, 0).SetCellPadding(2);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(0, 0, 0, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }

                                            pdfTableMain.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Cell(0, 4).SetContent("REG.No.");
                                            pdfTableMain.Cell(0, 4).SetCellPadding(2);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(0, 4, 0, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }

                                            for (int rowQ = 0; rowQ < 31; rowQ++)
                                            {
                                                if (rowQ + 1 < 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(5);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 27)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                                }
                                                else if (rowQ + 1 == 30)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 29)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(20);
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(20);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                }
                                                else if (rowQ + 1 < 31)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                            }
                                        }
                                        if (RegNo.Length > rOw)
                                        {
                                            pdfTableMain.Cell(tempRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Rows[tempRow + 1].SetCellPadding(5);
                                            pdfTableMain.Cell(tempRow + 1, 0).SetContent(RegNo[rOw].ToString().Trim(new char[] { '\'' }).Replace("'", "").Trim());
                                            pdfTableMain.Cell(tempRow + 1, 0).SetFont(Fontnormal);
                                            pdfTableMain.Cell(tempRow + 1, 4).SetContent("\t");
                                            pdfTableMain.Cell(tempRow + 1, 4).SetCellPadding(5);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 0, tempRow + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 4, tempRow + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                            rOw++;
                                        }
                                        else
                                        {
                                            if (tempRow + 1 < 26)
                                            {
                                                pdfTableMain.Cell(tempRow + 1, 0).SetContent("\t");
                                                pdfTableMain.Cell(tempRow + 1, 0).SetCellPadding(5);
                                                pdfTableMain.Cell(tempRow + 1, 4).SetContent("\t");
                                                pdfTableMain.Cell(tempRow + 1, 4).SetCellPadding(5);
                                                foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 0, tempRow + 1, 0).Cells)
                                                {
                                                    pc.ColSpan = 4;
                                                }
                                                foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 4, tempRow + 1, 4).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                            }
                                        }
                                        tempRow++;
                                    }

                                    for (int rowQ = tempRow; rowQ < 31; rowQ++)
                                    {
                                        if (rowQ + 1 < 26)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(5);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                        }
                                        else if (rowQ + 1 == 26)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                        }
                                        else if (rowQ + 1 == 27)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                            pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                        }
                                        else if (rowQ + 1 == 30)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                        }
                                        else if (rowQ + 1 == 29)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(20);
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            //{
                                            //    pc.ColSpan = 2;
                                            //}
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                            pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(20);
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            //{
                                            //    pc.ColSpan = 2;
                                            //}
                                        }
                                        else if (rowQ + 1 < 31)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                            //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(4);
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(4);
                                            //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(8);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(8);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                        }
                                    }

                                    //pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 50, posY, mydocument.PageWidth - 100, mydocument.PageHeight - posY));
                                    //mypdfpage.Add(pdftblPageMain);
                                    pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 50, posY, mydocument.PageWidth - 100, 700));
                                    mypdfpage.Add(pdftblPageMain);
                                    posY += Convert.ToInt16(pdftblPageMain.Area.Height) + 15;
                                    //pdfLIneBott = pdftblPageMain.CellArea(28, 0).RightBound(Color.White, 1);
                                    //mypdfpage.Add(pdfLIneBott);

                                    //pdfLIneBott = pdftblPageMain.CellArea(28, 0).LowerBound(Color.White, 1);
                                    //mypdfpage.Add(pdfLIneBott);

                                    //pdfLIneBott = pdftblPageMain.CellArea(28, 3).LowerBound(Color.White, 1);
                                    //mypdfpage.Add(pdfLIneBott);

                                    mypdfpage.SaveToDocument();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any One Record And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
            if (status)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "FoilSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void printFoilSheet()
    {
        try
        {
            FpPhasing.SaveChanges();
            string Line1 = string.Empty;
            string Line2 = string.Empty;
            string Line3 = string.Empty;
            string Line4 = string.Empty;
            string Line5 = string.Empty;
            string Line6 = string.Empty;
            string Line7 = string.Empty;
            string Line8 = string.Empty;
            PdfDocument mydocument = new PdfDocument(PdfDocumentFormat.A4);
            PdfPage mypdfpage;

            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);

            Font Fontbold1 = new Font("Algerian", 13, FontStyle.Bold);
            Font Fontbold12 = new Font("Algerian", 12, FontStyle.Regular);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            Font font4smallnew = new Font("Palatino Linotype", 7, FontStyle.Bold);

            string valuation = Convert.ToString(txtValuation.Text).Trim();
            int valuationNo = 1;
            int.TryParse(valuation.Trim(), out valuationNo);
            if (valuationNo <= 0)
            {
                valuationNo = 1;
            }
            bool selected = false;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            ExamMonth = string.Empty;
            ExamYear = string.Empty;
            qryExamMonth = string.Empty;
            qryExamYear = string.Empty;
            qrySubjectCodes = string.Empty;
            qrySubjectNos = string.Empty;
            subjectCodes = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " and r.college_code in (" + collegeCode + ")";
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any " + lblCollege.Text.Trim();
                divPopAlert.Visible = true;
                return;
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            if (ddlExamYear.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamYear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            ExamYear = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    qryExamYear = " and ed.Exam_Year in(" + ExamYear + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamYear.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }

            if (ddlExamMonth.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamMonth.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(ExamMonth))
                        {
                            ExamMonth += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            ExamMonth = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in(" + ExamMonth + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamMonth.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblSubject.Items.Count > 0 && txtSubject.Visible == true)
            {
                subjectCodes = getCblSelectedValue(cblSubject);
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlSubejct.Items.Count > 0 && ddlSubejct.Visible == true)
            {
                subjectCodes = string.Empty;
                foreach (ListItem li in ddlSubejct.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(subjectCodes))
                        {
                            subjectCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            subjectCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjects.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            if (FpPhasing.Sheets[0].RowCount > 0)
            {
                subjectCodes = string.Empty;
                for (int row = 0; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        if (!string.IsNullOrEmpty(subCode))
                        {
                            if (string.IsNullOrEmpty(subjectCodes))
                            {
                                subjectCodes = "'" + subCode + "'";
                            }
                            else
                            {
                                subjectCodes += ",'" + subCode + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    qrySubjectCodes = " and s.subject_code in(" + subjectCodes + ")";
                    qrySubjectFilter = "subject_code in(" + subjectCodes + ")";
                }
            }

            DataSet dsColInfo = da.select_method_wo_parameter("select com_name,college_code,case when ISNULL(com_name,'')<>'' then UPPER(ISNULL(com_name,'')) else UPPER(collname) end+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby,logo1 from collinfo", "Text");
            if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
            {

            }
            Line6 = "STATEMENT OF MARKS";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            int posY = 0;
            bool status = false;

            DataSet dsSubjectAdditionalDetail = new DataSet();// da.select_method_wo_parameter("SELECT distinct sm.semester,Replicate('M', sm.semester/1000)+ REPLACE(REPLACE(REPLACE(Replicate('C', sm.semester%1000/100),Replicate('C', 9), 'CM'),Replicate('C', 5), 'D'),Replicate('C', 4), 'CD')+ REPLACE(REPLACE(REPLACE(Replicate('X', sm.semester%100 / 10),Replicate('X', 9),'XC'),Replicate('X', 5), 'L'),Replicate('X', 4), 'XL')+ REPLACE(REPLACE(REPLACE(Replicate('I', sm.semester%10),Replicate('I', 9),'IX'),Replicate('I', 5), 'V'),Replicate('I', 4),'IV') as SemRoman,sm.degree_code,s.subject_code,s.subject_name,s.min_int_marks,s.max_int_marks,s.min_ext_marks,s.max_ext_marks,s.mintotal,s.maxtotal FROM SUBJECT S,syllabus_master sm,sub_sem ss,Registration r where r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no " + qryCollege + " order by sm.semester asc,sm.degree_code,s.subject_code", "text");

            bool withMark = false;
            bool isReValuation = false;
            bool isValuation = false;
            if (chkRevaluation.Checked)
                isReValuation = true;
            if (ddlReport.Items.Count > 0)
            {
                if (ddlReport.SelectedIndex == 0)
                    withMark = true;
            }
            if (ddlFormat.Items.Count > 0)
            {
                if (ddlFormat.SelectedIndex == 0)
                    isValuation = true;
            }
            DataSet dsMarkDetails = new DataSet();

            if (selected)
            {
                dsSubjectAdditionalDetail = da.select_method_wo_parameter("SELECT distinct sm.semester,Replicate('M', sm.semester/1000)+ REPLACE(REPLACE(REPLACE(Replicate('C', sm.semester%1000/100),Replicate('C', 9), 'CM'),Replicate('C', 5), 'D'),Replicate('C', 4), 'CD')+ REPLACE(REPLACE(REPLACE(Replicate('X', sm.semester%100 / 10),Replicate('X', 9),'XC'),Replicate('X', 5), 'L'),Replicate('X', 4), 'XL')+ REPLACE(REPLACE(REPLACE(Replicate('I', sm.semester%10),Replicate('I', 9),'IX'),Replicate('I', 5), 'V'),Replicate('I', 4),'IV') as SemRoman,sm.degree_code,s.subject_code,s.subject_name,s.min_int_marks,s.max_int_marks,s.min_ext_marks,s.max_ext_marks,s.mintotal,s.maxtotal FROM SUBJECT S,syllabus_master sm,sub_sem ss,Registration r where r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no " + qryCollege + qrySubjectCodes + qryDegreeCode + " order by sm.semester asc,sm.degree_code,s.subject_code", "text");
                if (isReValuation || withMark)
                {
                    dsMarkDetails = da.select_method_wo_parameter("select ed.exam_code,ed.batch_year,ed.degree_code,ea.roll_no,r.Reg_No,ead.subject_no,s.subject_code,s.subject_name,m.internal_mark,m.external_mark,m.total,m.grade,m.result,m.evaluation1,m.evaluation2,m.evaluation3,m.Act_Reval_Mark,m.actual_external_mark,m.actual_internal_mark,m.actual_total from Exam_Details ed,exam_appl_details ead,exam_application ea,mark_entry m,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=m.roll_no and m.exam_code=ed.exam_code and m.subject_no=ead.subject_no and s.subject_no=ead.subject_no and r.Roll_No=m.roll_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year " + ((isReValuation) ? " and ea.Exam_type='2' " : "") + qryCollege + qrySubjectCodes + qryDegreeCode + " and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' order by ea.roll_no", "text");
                }
                for (int row = 1; row < FpPhasing.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    string rowno = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Text).Trim();
                    if (sel == 1)
                    {
                        int PageNo = 1;
                        int ToatlPage = 1;
                        status = true;
                        bool pageHas = false;
                        posY = 5;
                        string allRegNo = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 0].Note).Trim();
                        //string degreeCode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Tag).Trim();
                        string majorDept = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Note).Trim();

                        string majorPart = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 1].Tag).Trim();
                        bool isMajor = false;
                        bool.TryParse(majorPart, out isMajor);
                        if (isMajor)
                        {
                            if (majorDept.Split(',').Length != 1)
                            {
                                majorDept = string.Empty;
                            }
                        }
                        else
                        {
                            majorDept = string.Empty;
                        }
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpPhasing.Sheets[0].Cells[row, 3].Note).Trim();
                        if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                        {
                            DataView dvColege = new DataView();
                            dsColInfo.Tables[0].DefaultView.RowFilter = "college_code in('" + collcode + "')";
                            dvColege = dsColInfo.Tables[0].DefaultView;
                            collcode = Convert.ToString(dsColInfo.Tables[0].Rows[0]["college_code"]).Trim();
                            if (dvColege.Count > 0)
                            {
                                collcode = Convert.ToString(dvColege[0]["college_code"]).Trim();
                                PdfTable table2;
                                PdfImage LogoImage;
                                PdfTablePage tblPage;

                                PdfTextArea pdfLine1;
                                PdfTextArea pdfLine2;
                                PdfTextArea pdfLine3;
                                PdfTextArea pdfLine4;
                                PdfTextArea pdfLine5;
                                PdfTextArea pdfLine6;
                                PdfTextArea pdfLine7;

                                PdfTablePage pdftblPage;
                                PdfTablePage pdftblPageTop;
                                PdfTable pdfTableTop;

                                PdfTable pdfTableHeading;
                                PdfTablePage pdftblPageHeading;

                                PdfTable pdfTableMain;
                                PdfTablePage pdftblPageMain;

                                PdfRectangle pdfRectTopTable;

                                string subjectMinINT = string.Empty;
                                string subjectMaxINT = string.Empty;

                                string subjectMinEXT = string.Empty;
                                string subjectMaxEXT = string.Empty;

                                string subjectMinTOT = string.Empty;
                                string subjectMaxTOT = string.Empty;
                                string subjectSemester = string.Empty;

                                DataTable dtSubjectDetails = new DataTable();
                                if (dsSubjectAdditionalDetail.Tables.Count > 0 && dsSubjectAdditionalDetail.Tables[0].Rows.Count > 0)
                                {
                                    dsSubjectAdditionalDetail.Tables[0].DefaultView.RowFilter = "subject_code='" + subCode + "'";//and degree_code='" + degreeCode + "'
                                    dtSubjectDetails = dsSubjectAdditionalDetail.Tables[0].DefaultView.ToTable();
                                    if (dtSubjectDetails.Rows.Count > 0)
                                    {
                                        subjectMinINT = Convert.ToString(dtSubjectDetails.Rows[0]["min_int_marks"]).Trim();
                                        subjectMaxINT = Convert.ToString(dtSubjectDetails.Rows[0]["max_int_marks"]).Trim();

                                        subjectMinEXT = Convert.ToString(dtSubjectDetails.Rows[0]["min_ext_marks"]).Trim();
                                        subjectMaxEXT = Convert.ToString(dtSubjectDetails.Rows[0]["max_ext_marks"]).Trim();

                                        subjectMinTOT = Convert.ToString(dtSubjectDetails.Rows[0]["mintotal"]).Trim();
                                        subjectMaxTOT = Convert.ToString(dtSubjectDetails.Rows[0]["maxtotal"]).Trim();
                                        subjectSemester = Convert.ToString(dtSubjectDetails.Rows[0]["SemRoman"]).Trim();
                                    }
                                }

                                Line1 = Convert.ToString(dvColege[0]["Line1"]).Trim();
                                try
                                {
                                    string[] affli = Convert.ToString(dvColege[0]["affliatedby"]).Trim().Split('\\');
                                    Line2 = affli[0].Split(',')[0];
                                    Line4 = "(" + affli[2].Split(',')[0] + ")";
                                    Line3 = affli[1].Split(',')[0];
                                }
                                catch { }
                                Line5 = Convert.ToString(dvColege[0]["distr"]).Trim();
                                Line6 = "STATEMENT OF MARKS";
                                Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                subjectName = "SUBJECT TITLE\t\t:\t\t " + subName;
                                subjectCode = "CODE\t\t:\t\t" + subCode;
                                if (RegNo.Length > 0)
                                {
                                    if (RegNo.Length % 25 == 0)
                                    {
                                        ToatlPage = RegNo.Length / 25;
                                    }
                                    else
                                    {
                                        ToatlPage = (RegNo.Length / 25) + 1;
                                    }
                                    pageHas = true;
                                    mypdfpage = mydocument.NewPage();
                                    int rightY = posY;
                                    pdfTableTop = mydocument.NewTable(Fontbold1, 2, 4, 1);
                                    pdfTableTop.SetBorders(Color.Black, 1, BorderType.None);
                                    pdfTableTop.VisibleHeaders = false;
                                    pdfTableTop.Columns[0].SetWidth(130);
                                    pdfTableTop.Columns[1].SetWidth(200);
                                    pdfTableTop.Columns[2].SetWidth(150);
                                    pdfTableTop.Columns[3].SetWidth(60);

                                    pdfTableTop.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableTop.Cell(0, 0).SetContent("Valuation\t:\t");
                                    pdfTableTop.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableTop.Cell(0, 1).SetContent((isReValuation || !isValuation) ? "" : ToRoman(valuationNo));
                                    pdfTableTop.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                    pdfTableTop.Cell(0, 2).SetContent("Packet No.");
                                    pdfTableTop.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableTop.Cell(0, 3).SetContent("");
                                    pdfTableTop.Cell(0, 3).SetCellPadding(4);

                                    pdftblPageTop = pdfTableTop.CreateTablePage(new PdfArea(mydocument, 5, posY, (mydocument.PageWidth / 2) - 20, 100));
                                    mypdfpage.Add(pdftblPageTop);
                                    posY += Convert.ToInt32(pdftblPageTop.Area.Height);
                                    pdfRectTopTable = pdftblPageTop.CellArea(0, 3).ToRectangle(Color.Black, 1.2, Color.White);
                                    mypdfpage.Add(pdfRectTopTable);

                                    MemoryStream memoryStream = new MemoryStream();
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(dvColege[0]["logo1"])))
                                        {
                                            byte[] file = (byte[])dvColege[0]["logo1"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                                {
                                                }
                                                else
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 3, posY + 5, 680);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                        mypdfpage.Add(LogoImage, 3, posY + 5, 680);
                                    }
                                    PdfTextArea pdfSince = new PdfTextArea(Fontsmall, Color.Black, new PdfArea(mydocument, 5, posY + 43, 100, 15), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                    mypdfpage.Add(pdfSince);

                                    pdfLine1 = new PdfTextArea(font2bold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line1);
                                    mypdfpage.Add(pdfLine1);

                                    posY += 15;
                                    pdfLine2 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line2);
                                    mypdfpage.Add(pdfLine2);

                                    posY += 15;
                                    pdfLine3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line3);
                                    mypdfpage.Add(pdfLine3);

                                    posY += 15;
                                    pdfLine4 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line4);
                                    mypdfpage.Add(pdfLine4);

                                    posY += 15;
                                    pdfLine5 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line5);
                                    mypdfpage.Add(pdfLine5);

                                    posY += 15;
                                    pdfLine6 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line6);
                                    mypdfpage.Add(pdfLine6);

                                    posY += 15;
                                    pdfLine7 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line7);
                                    mypdfpage.Add(pdfLine7);

                                    posY += 16;
                                    pdfTableHeading = mydocument.NewTable(Fontbold, 4, 9, 3);
                                    pdfTableHeading.SetBorders(Color.Black, 1, BorderType.RowsAndBounds);
                                    pdfTableHeading.VisibleHeaders = false;
                                    pdfTableHeading.Columns[0].SetWidth(250);
                                    pdfTableHeading.Columns[1].SetWidth(8);
                                    pdfTableHeading.Columns[2].SetWidth(80);
                                    pdfTableHeading.Columns[3].SetWidth(250);
                                    pdfTableHeading.Columns[4].SetWidth(8);
                                    pdfTableHeading.Columns[5].SetWidth(80);
                                    pdfTableHeading.Columns[6].SetWidth(150);
                                    pdfTableHeading.Columns[7].SetWidth(8);
                                    pdfTableHeading.Columns[8].SetWidth(100);

                                    pdfTableHeading.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 0).SetContent("Max. Marks");
                                    pdfTableHeading.Cell(0, 0).SetFont(font2small);
                                    pdfTableHeading.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 1).SetContent(":");
                                    pdfTableHeading.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 2).SetContent(subjectMaxEXT);

                                    pdfTableHeading.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 3).SetContent("Passing Min.");
                                    pdfTableHeading.Cell(0, 3).SetFont(font2small);
                                    pdfTableHeading.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 4).SetContent(":");
                                    pdfTableHeading.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 5).SetContent(subjectMinEXT);

                                    pdfTableHeading.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 6).SetContent("Absent");
                                    pdfTableHeading.Cell(0, 6).SetFont(font2small);
                                    pdfTableHeading.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(0, 7).SetContent(":");
                                    pdfTableHeading.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(0, 8).SetContent("AAA");

                                    pdfTableHeading.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 0).SetContent("MAJOR");
                                    pdfTableHeading.Cell(1, 0).SetFont(font2small);
                                    pdfTableHeading.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(1, 1).SetContent(":");
                                    pdfTableHeading.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 2).SetContent(majorDept);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(1, 2, 1, 2).Cells)
                                    {
                                        pc.ColSpan = 4;
                                    }

                                    pdfTableHeading.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 6).SetContent("SEM");
                                    pdfTableHeading.Cell(1, 6).SetFont(font2small);
                                    pdfTableHeading.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(1, 7).SetContent(":");
                                    pdfTableHeading.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(1, 8).SetContent(subjectSemester);

                                    pdfTableHeading.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(2, 0).SetContent("CODE");
                                    pdfTableHeading.Cell(2, 0).SetFont(font2small);
                                    pdfTableHeading.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(2, 1).SetContent(":");
                                    pdfTableHeading.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(2, 2).SetContent(subCode);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(2, 2, 2, 2).Cells)
                                    {
                                        pc.ColSpan = 7;
                                    }

                                    pdfTableHeading.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(3, 0).SetContent("SUBJECT");
                                    pdfTableHeading.Cell(3, 0).SetCellPadding(1);
                                    pdfTableHeading.Cell(3, 0).SetFont(font2small);
                                    pdfTableHeading.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableHeading.Cell(3, 1).SetContent(":");
                                    pdfTableHeading.Cell(3, 1).SetCellPadding(1);
                                    pdfTableHeading.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    pdfTableHeading.Cell(3, 2).SetContent(subName);
                                    pdfTableHeading.Cell(3, 2).SetCellPadding(1);
                                    foreach (PdfCell pc in pdfTableHeading.CellRange(3, 2, 3, 2).Cells)
                                    {
                                        pc.ColSpan = 7;
                                    }

                                    pdftblPageHeading = pdfTableHeading.CreateTablePage(new PdfArea(mydocument, 4, posY, (mydocument.PageWidth / 2) - 8, 120));
                                    mypdfpage.Add(pdftblPageHeading);
                                    PdfLine pdfHLine1 = pdftblPageHeading.CellArea(0, 2).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine1);
                                    PdfLine pdfHLine2 = pdftblPageHeading.CellArea(0, 5).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine2);
                                    PdfLine pdfHLine3 = pdftblPageHeading.CellArea(1, 2).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfHLine3);
                                    posY += Convert.ToInt32(pdftblPageHeading.Area.Height) + 3;

                                    int rOw = 0;
                                    bool newPage = false;
                                    int tempRow = 0;

                                    PdfLine pdfLIneBott;
                                    pdfTableMain = mydocument.NewTable(font2bold, 31, 6, 4);
                                    pdfTableMain.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    pdfTableMain.VisibleHeaders = false;
                                    pdfTableMain.Columns[0].SetWidth(50);
                                    pdfTableMain.Columns[1].SetWidth(50);
                                    pdfTableMain.Columns[2].SetWidth(50);
                                    pdfTableMain.Columns[3].SetWidth(50);
                                    pdfTableMain.Columns[4].SetWidth(50);
                                    pdfTableMain.Columns[5].SetWidth(50);

                                    pdfTableMain.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableMain.Cell(0, 0).SetContent("Register Number");
                                    pdfTableMain.Cell(0, 0).SetCellPadding(2);
                                    foreach (PdfCell pc in pdfTableMain.CellRange(0, 0, 0, 0).Cells)
                                    {
                                        pc.ColSpan = 4;
                                    }

                                    pdfTableMain.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    pdfTableMain.Cell(0, 4).SetContent("MARKS");
                                    pdfTableMain.Cell(0, 4).SetCellPadding(2);
                                    foreach (PdfCell pc in pdfTableMain.CellRange(0, 4, 0, 4).Cells)
                                    {
                                        pc.ColSpan = 2;
                                    }

                                    for (int roow = rOw; roow < RegNo.Length; roow++)
                                    {
                                        if (rOw % 25 == 0 && rOw != 0 && (RegNo.Length > rOw))
                                        {
                                            PageNo++;
                                            for (int rowQ = tempRow; rowQ < 31; rowQ++)
                                            {
                                                if (rowQ + 1 < 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(4);
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(4);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 27)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                                }
                                                else if (rowQ + 1 == 30)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 28)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(18);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(18);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 29)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(1);
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(1);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                }
                                                else if (rowQ + 1 < 31)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(6);
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                            }

                                            pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 10, posY, (mydocument.PageWidth / 2) - 20, 700));
                                            mypdfpage.Add(pdftblPageMain);
                                            posY += Convert.ToInt16(pdftblPageMain.Area.Height) + 15;

                                            pdfLIneBott = pdftblPageMain.CellArea(30, 0).RightBound(Color.White, 1);
                                            mypdfpage.Add(pdfLIneBott);

                                            pdfLIneBott = pdftblPageMain.CellArea(28, 0).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfLIneBott);

                                            pdfLIneBott = pdftblPageMain.CellArea(28, 0).LowerBound(Color.White, 1);
                                            mypdfpage.Add(pdfLIneBott);

                                            pdfLIneBott = pdftblPageMain.CellArea(28, 3).LowerBound(Color.White, 1);
                                            mypdfpage.Add(pdfLIneBott);

                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            tempRow = 0;
                                            posY = 5;
                                            tempRow = 0;
                                            rightY = posY;
                                            pdfTableTop = mydocument.NewTable(Fontbold1, 2, 4, 3);
                                            pdfTableTop.SetBorders(Color.Black, 1, BorderType.None);
                                            pdfTableTop.VisibleHeaders = false;
                                            pdfTableTop.Columns[0].SetWidth(130);
                                            pdfTableTop.Columns[1].SetWidth(200);
                                            pdfTableTop.Columns[2].SetWidth(150);
                                            pdfTableTop.Columns[3].SetWidth(60);

                                            pdfTableTop.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableTop.Cell(0, 0).SetContent("Valuation\t:\t");
                                            pdfTableTop.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableTop.Cell(0, 1).SetContent((isReValuation || !isValuation) ? "" : ToRoman(valuationNo));
                                            pdfTableTop.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                            pdfTableTop.Cell(0, 2).SetContent("Packet No.");
                                            pdfTableTop.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableTop.Cell(0, 3).SetContent("");
                                            pdfTableTop.Cell(0, 3).SetCellPadding(4);

                                            pdftblPageTop = pdfTableTop.CreateTablePage(new PdfArea(mydocument, 5, posY, (mydocument.PageWidth / 2) - 20, 100));
                                            mypdfpage.Add(pdftblPageTop);
                                            posY += Convert.ToInt32(pdftblPageTop.Area.Height);
                                            pdfRectTopTable = pdftblPageTop.CellArea(0, 3).ToRectangle(Color.Black, 1.2, Color.White);
                                            mypdfpage.Add(pdfRectTopTable);

                                            memoryStream = new MemoryStream();
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                            {
                                                if (!string.IsNullOrEmpty(Convert.ToString(dvColege[0]["logo1"])))
                                                {
                                                    byte[] file = (byte[])dvColege[0]["logo1"];
                                                    memoryStream.Write(file, 0, file.Length);
                                                    if (file.Length > 0)
                                                    {
                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                        }
                                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                        mypdfpage.Add(LogoImage, 3, posY + 5, 680);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 3, posY + 5, 680);
                                            }
                                            pdfSince = new PdfTextArea(Fontsmall, Color.Black, new PdfArea(mydocument, 5, posY + 43, 100, 15), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                            mypdfpage.Add(pdfSince);

                                            pdfLine1 = new PdfTextArea(font2bold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line1);
                                            mypdfpage.Add(pdfLine1);

                                            posY += 15;
                                            pdfLine2 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line2);
                                            mypdfpage.Add(pdfLine2);

                                            posY += 15;
                                            pdfLine3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line3);
                                            mypdfpage.Add(pdfLine3);

                                            posY += 15;
                                            pdfLine4 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line4);
                                            mypdfpage.Add(pdfLine4);

                                            posY += 15;
                                            pdfLine5 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line5);
                                            mypdfpage.Add(pdfLine5);

                                            posY += 15;
                                            pdfLine6 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 12, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line6);
                                            mypdfpage.Add(pdfLine6);

                                            posY += 15;
                                            pdfLine7 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 10, posY, (mydocument.PageWidth / 2), 20), System.Drawing.ContentAlignment.TopCenter, Line7);
                                            mypdfpage.Add(pdfLine7);

                                            //posY += 18;
                                            posY += 16;
                                            pdfTableHeading = mydocument.NewTable(Fontbold, 4, 9, 3);
                                            pdfTableHeading.SetBorders(Color.Black, 1, BorderType.RowsAndBounds);
                                            pdfTableHeading.VisibleHeaders = false;
                                            pdfTableHeading.Columns[0].SetWidth(250);
                                            pdfTableHeading.Columns[1].SetWidth(8);
                                            pdfTableHeading.Columns[2].SetWidth(80);
                                            pdfTableHeading.Columns[3].SetWidth(250);
                                            pdfTableHeading.Columns[4].SetWidth(8);
                                            pdfTableHeading.Columns[5].SetWidth(80);
                                            pdfTableHeading.Columns[6].SetWidth(150);
                                            pdfTableHeading.Columns[7].SetWidth(8);
                                            pdfTableHeading.Columns[8].SetWidth(100);

                                            pdfTableHeading.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 0).SetContent("Max. Marks");
                                            pdfTableHeading.Cell(0, 0).SetFont(font2small);
                                            pdfTableHeading.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 1).SetContent(":");
                                            pdfTableHeading.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 2).SetContent(subjectMaxEXT);

                                            pdfTableHeading.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 3).SetContent("Passing Min.");
                                            pdfTableHeading.Cell(0, 3).SetFont(font2small);
                                            pdfTableHeading.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 4).SetContent(":");
                                            pdfTableHeading.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 5).SetContent(subjectMinEXT);

                                            pdfTableHeading.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 6).SetContent("Absent");
                                            pdfTableHeading.Cell(0, 6).SetFont(font2small);
                                            pdfTableHeading.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(0, 7).SetContent(":");
                                            pdfTableHeading.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(0, 8).SetContent("AAA");

                                            pdfTableHeading.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 0).SetContent("MAJOR");
                                            pdfTableHeading.Cell(1, 0).SetFont(font2small);
                                            pdfTableHeading.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(1, 1).SetContent(":");
                                            pdfTableHeading.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 2).SetContent(majorDept);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(1, 2, 1, 2).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }

                                            pdfTableHeading.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 6).SetContent("SEM");
                                            pdfTableHeading.Cell(1, 6).SetFont(font2small);
                                            pdfTableHeading.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(1, 7).SetContent(":");
                                            pdfTableHeading.Cell(1, 8).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(1, 8).SetContent(subjectSemester);

                                            pdfTableHeading.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(2, 0).SetContent("CODE");
                                            pdfTableHeading.Cell(2, 0).SetFont(font2small);
                                            pdfTableHeading.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(2, 1).SetContent(":");
                                            pdfTableHeading.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(2, 2).SetContent(subCode);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(2, 2, 2, 2).Cells)
                                            {
                                                pc.ColSpan = 7;
                                            }

                                            pdfTableHeading.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(3, 0).SetContent("SUBJECT");
                                            pdfTableHeading.Cell(3, 0).SetCellPadding(1);
                                            pdfTableHeading.Cell(3, 0).SetFont(font2small);
                                            pdfTableHeading.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableHeading.Cell(3, 1).SetContent(":");
                                            pdfTableHeading.Cell(3, 1).SetCellPadding(1);
                                            pdfTableHeading.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableHeading.Cell(3, 2).SetContent(subName);
                                            pdfTableHeading.Cell(3, 2).SetCellPadding(1);
                                            foreach (PdfCell pc in pdfTableHeading.CellRange(3, 2, 3, 2).Cells)
                                            {
                                                pc.ColSpan = 7;
                                            }

                                            pdftblPageHeading = pdfTableHeading.CreateTablePage(new PdfArea(mydocument, 4, posY, (mydocument.PageWidth / 2) - 8, 120));
                                            mypdfpage.Add(pdftblPageHeading);
                                            pdfHLine1 = pdftblPageHeading.CellArea(0, 2).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine1);
                                            pdfHLine2 = pdftblPageHeading.CellArea(0, 5).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine2);
                                            pdfHLine3 = pdftblPageHeading.CellArea(1, 2).RightBound(Color.Black, 1);
                                            mypdfpage.Add(pdfHLine3);
                                            posY += Convert.ToInt32(pdftblPageHeading.Area.Height) + 3;

                                            newPage = false;
                                            tempRow = 0;

                                            pdfTableMain = mydocument.NewTable(font2bold, 31, 6, 4);
                                            pdfTableMain.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            pdfTableMain.VisibleHeaders = false;
                                            pdfTableMain.Columns[0].SetWidth(50);
                                            pdfTableMain.Columns[1].SetWidth(50);
                                            pdfTableMain.Columns[2].SetWidth(50);
                                            pdfTableMain.Columns[3].SetWidth(50);
                                            pdfTableMain.Columns[4].SetWidth(50);
                                            pdfTableMain.Columns[5].SetWidth(50);

                                            pdfTableMain.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Cell(0, 0).SetContent("Register Number");
                                            pdfTableMain.Cell(0, 0).SetCellPadding(2);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(0, 0, 0, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }

                                            pdfTableMain.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Cell(0, 4).SetContent("MARKS");
                                            pdfTableMain.Cell(0, 4).SetCellPadding(2);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(0, 4, 0, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }

                                            for (int rowQ = 0; rowQ < 31; rowQ++)
                                            {
                                                if (rowQ + 1 < 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(4);
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(4);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 26)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 4;
                                                    }
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                }
                                                else if (rowQ + 1 == 27)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                                    pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                                    pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                                }
                                                else if (rowQ + 1 == 30)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 28)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(18);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(18);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                                else if (rowQ + 1 == 29)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(1);
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(1);
                                                    pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    //{
                                                    //    pc.ColSpan = 2;
                                                    //}
                                                }
                                                else if (rowQ + 1 < 31)
                                                {
                                                    pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(6);
                                                    //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                    //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(10);
                                                    foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                                    {
                                                        pc.ColSpan = 3;
                                                    }
                                                }
                                            }
                                        }
                                        if (RegNo.Length > rOw)
                                        {
                                            string externalMark = string.Empty;
                                            if (withMark)
                                            {
                                                DataView dvMarks = new DataView();
                                                if (dsMarkDetails.Tables.Count > 0 && dsMarkDetails.Tables[0].Rows.Count > 0)
                                                {
                                                    dsMarkDetails.Tables[0].DefaultView.RowFilter = "subject_code='" + subCode + "' and reg_no='" + RegNo[rOw].ToString().Trim(new char[] { '\'' }).Replace("'", "").Trim() + "'";
                                                    dvMarks = dsMarkDetails.Tables[0].DefaultView;
                                                }
                                                if (dvMarks.Count > 0)
                                                {
                                                    if (isReValuation || !isValuation)
                                                    {
                                                        externalMark = Convert.ToString(dvMarks[0]["external_mark"]).Trim();
                                                    }
                                                    else if (isValuation)
                                                    {
                                                        switch (valuationNo)
                                                        {
                                                            case 1:
                                                                externalMark = Convert.ToString(dvMarks[0]["evaluation1"]).Trim();
                                                                break;
                                                            case 2:
                                                                externalMark = Convert.ToString(dvMarks[0]["evaluation2"]).Trim();
                                                                break;
                                                            case 3:
                                                                externalMark = Convert.ToString(dvMarks[0]["evaluation3"]).Trim();
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        externalMark = Convert.ToString(dvMarks[0]["external_mark"]).Trim();
                                                    }
                                                }
                                            }
                                            double extMark = 0;
                                            double.TryParse(externalMark.Trim(), out extMark);
                                            if (extMark < 0)
                                            {
                                                if (extMark == -1)
                                                {
                                                    externalMark = "AAA";
                                                }
                                                else if (extMark == -2)
                                                {
                                                    externalMark = "NE";
                                                }
                                                else if (extMark == -3)
                                                {
                                                    externalMark = "NR";
                                                }
                                                else if (extMark == -4)
                                                {
                                                    externalMark = "LT";
                                                }
                                            }
                                            pdfTableMain.Cell(tempRow + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            pdfTableMain.Rows[tempRow + 1].SetCellPadding(4);
                                            pdfTableMain.Cell(tempRow + 1, 0).SetContent(RegNo[rOw].ToString().Trim(new char[] { '\'' }).Replace("'", "").Trim());
                                            pdfTableMain.Cell(tempRow + 1, 0).SetFont(Fontnormal);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 0, tempRow + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            pdfTableMain.Cell(tempRow + 1, 4).SetContent(externalMark);
                                            pdfTableMain.Cell(tempRow + 1, 4).SetFont(Fontnormal);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 4, tempRow + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                            rOw++;
                                        }
                                        else
                                        {
                                            if (tempRow + 1 < 26)
                                            {
                                                pdfTableMain.Cell(tempRow + 1, 0).SetContent("\t");
                                                pdfTableMain.Rows[tempRow + 1].SetCellPadding(4);
                                                foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 0, tempRow + 1, 0).Cells)
                                                {
                                                    pc.ColSpan = 4;
                                                }
                                                foreach (PdfCell pc in pdfTableMain.CellRange(tempRow + 1, 4, tempRow + 1, 4).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                            }
                                        }
                                        tempRow++;
                                    }

                                    for (int rowQ = tempRow; rowQ < 31; rowQ++)
                                    {
                                        if (rowQ + 1 < 26)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(4);
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetCellPadding(4);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                        }
                                        else if (rowQ + 1 == 26)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Total");
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 4;
                                            }
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 4, rowQ + 1, 4).Cells)
                                            {
                                                pc.ColSpan = 2;
                                            }
                                        }
                                        else if (rowQ + 1 == 27)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Regd.");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 1).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 2).SetContent("Absent");
                                            pdfTableMain.Cell(rowQ + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContent("Valued");
                                            pdfTableMain.Cell(rowQ + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            pdfTableMain.Cell(rowQ + 1, 5).SetContent("\t");
                                        }
                                        else if (rowQ + 1 == 30)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Verified by\t:\t\t");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("Date\t:\t\t");
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                        }
                                        else if (rowQ + 1 == 28)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(18);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(18);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                        }
                                        else if (rowQ + 1 == 29)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("Name & Sign. of Examiner (s) with date");
                                            pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(1);
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContentAlignment(ContentAlignment.BottomCenter);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            //{
                                            //    pc.ColSpan = 2;
                                            //}
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContent("Signature of Chairman with date");
                                            pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(1);
                                            pdfTableMain.Cell(rowQ + 1, 3).SetContentAlignment(ContentAlignment.BottomCenter);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            //{
                                            //    pc.ColSpan = 2;
                                            //}
                                        }
                                        else if (rowQ + 1 < 31)
                                        {
                                            pdfTableMain.Cell(rowQ + 1, 0).SetContent("\t");
                                            //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(5);
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetContent("\t");
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(6);
                                            //pdfTableMain.Cell(rowQ + 1, 0).SetCellPadding(8);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 0, rowQ + 1, 0).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                            //pdfTableMain.Cell(rowQ + 1, 3).SetCellPadding(8);
                                            foreach (PdfCell pc in pdfTableMain.CellRange(rowQ + 1, 3, rowQ + 1, 3).Cells)
                                            {
                                                pc.ColSpan = 3;
                                            }
                                        }
                                    }

                                    pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 10, posY, (mydocument.PageWidth / 2) - 20, 700));
                                    mypdfpage.Add(pdftblPageMain);
                                    posY += Convert.ToInt16(pdftblPageMain.Area.Height) + 15;
                                    //pdftblPageMain = pdfTableMain.CreateTablePage(new PdfArea(mydocument, 10, posY, (mydocument.PageWidth / 2) - 20, mydocument.PageHeight - posY));
                                    //mypdfpage.Add(pdftblPageMain);
                                    //posY += Convert.ToInt16(pdftblPageMain.Area.Height) + 15;
                                    pdfLIneBott = pdftblPageMain.CellArea(30, 0).RightBound(Color.White, 1);
                                    mypdfpage.Add(pdfLIneBott);

                                    pdfLIneBott = pdftblPageMain.CellArea(28, 0).RightBound(Color.Black, 1);
                                    mypdfpage.Add(pdfLIneBott);

                                    pdfLIneBott = pdftblPageMain.CellArea(28, 0).LowerBound(Color.White, 1);
                                    mypdfpage.Add(pdfLIneBott);

                                    pdfLIneBott = pdftblPageMain.CellArea(28, 3).LowerBound(Color.White, 1);
                                    mypdfpage.Add(pdfLIneBott);

                                    mypdfpage.SaveToDocument();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any One Record And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
            if (status)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "FoilSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPrintFoilSheet_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkBasedOnSeating.Checked)
            {
            }
            else
            {
                printFoilSheet();
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

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            printMaster1.Visible = false;
            string reportname = txtExcelFileName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpPhasing.Visible == true)
                {
                    da.printexcelreport(FpPhasing, reportname);
                }
                lblExcelError.Visible = false;
            }
            else
            {
                lblExcelError.Text = "Please Enter Your Report Name";
                lblExcelError.Visible = true;
                txtExcelFileName.Focus();
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
            rptheadname = ((!chkBasedOnSeating.Checked) ? "Cover Sheet" : "Phasing Sheet") + " Report";
            string pagename = "COECoverSheetGeneration.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpPhasing.Visible == true)
            {
                printMaster1.loadspreaddetails(FpPhasing, pagename, rptheadname);
            }
            printMaster1.Visible = true;
            lblExcelError.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
    }

    #endregion Button Events

}

#endregion Class Definition