#region Namespace Declaration

using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;

#endregion Namespace Declaration

#region Class Definition

public partial class CoeMod_COEDateWiseStudentStrengthReport : System.Web.UI.Page
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
                divDateWiseReport.Visible = false;
                if (ddlReportFormat.Items.Count > 0)
                {
                    if (ddlReportFormat.SelectedIndex == 0)
                    {
                        divDateWiseReport.Visible = true;
                    }
                }
                chkNeedDateWiseTotal.Checked = true;
                chkNeedOverallTotal.Checked = true;
                chkNeedSubjectTotal.Checked = false;
                chkWithoutRegularArrear.Checked = true;
                if (!chkDepartmentWise.Checked)
                {
                    chkNeedSubjectTotal.Checked = true;
                }
                //BindSubject();
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
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and ed.coll_code in(" + collegeCodes + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and et.degree_code in(" + degreeCodes + ")";
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
                    qryExamYear = " and et.Exam_year in(" + ExamYear + ")";
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
                    qryExamMonth = " and et.Exam_month in(" + ExamMonth + ")";
                }
            }
            if (cblExamDate.Items.Count > 0 && txtExamDate.Visible == true)
            {
                examDates = getCblSelectedValue(cblExamDate);
                if (!string.IsNullOrEmpty(examDates))
                {
                    qryExamDates = " and convert(varchar(20),ed.exam_date,103) in(" + examDates + ")";
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
                    qryExamDates = " and convert(varchar(20),ed.exam_date,103) in(" + examDates + ")";
                }
            }
            if (cblExamSession.Items.Count > 0 && txtExamSession.Visible == true)
            {
                examSessions = getCblSelectedValue(cblExamSession);
                if (!string.IsNullOrEmpty(examSessions))
                {
                    qryExamSessions = " and ed.Exam_Session in(" + examSessions + ")";
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
                    qryExamSessions = " and ed.Exam_Session in(" + examSessions + ")";
                }
            }
            if (chkBasedOnSeating.Checked)
            {
                isBasedOnSeatingArrangement = true;
            }
            if (!string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(ExamMonth) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(examDates) && !string.IsNullOrEmpty(qryExamDates) && !string.IsNullOrEmpty(examSessions) && !string.IsNullOrEmpty(qryExamSessions))
            {
                qry = "select distinct s.subject_code, s.subject_name,ed.exam_date,ed.exam_session from subject s,exmtt et,exmtt_det ed where et.exam_code=ed.exam_code and ed.subject_no=s.subject_no " + qryCollege + qryDegreeCode + qryExamYear + qryExamMonth + qryExamDates + qryExamSessions;
                if (isBasedOnSeatingArrangement)
                {
                    qry = "select distinct s.subject_code, s.subject_name,ed.exam_date,ed.exam_session from subject s,exmtt et,exmtt_det ed,exam_seating es where et.exam_code=ed.exam_code and ed.subject_no=s.subject_no and es.subject_no=ed.subject_no and es.degree_code=et.degree_code and es.edate=ed.exam_date and ed.exam_session=es.ses_sion " + qryCollege + qryDegreeCode + qryExamYear + qryExamMonth + qryExamDates + qryExamSessions;
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
            if (divHall.Visible)
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

            if (type == 0)
            {
                FpSpread1.Sheets[0].FrozenRowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Course Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[1].Width = 250;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = chkDepartmentWise.Checked;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[2].Width = 120;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[3].Width = 250;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[4].Width = 80;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Regular/Arrear";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[5].Width = 95;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = !chkWithoutRegularArrear.Checked;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = " No. Of ";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Students";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[6].Width = 80;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Male(M)";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[7].Width = 80;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 7, 1, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Female(F)";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 8, 1, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 5;
                FpSpread1.Sheets[0].FrozenRowCount = 1;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date & Session";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = " No. Of ";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Students";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[2].Width = 80;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 3);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Male(M)";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[3].Width = 80;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Female(F)";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[4].Width = 80;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 1, 1);

            }
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
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
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
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
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


    protected void chkDepartmentWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divDateWiseReport.Visible = false;
            if (ddlReportFormat.Items.Count > 0)
            {
                if (ddlReportFormat.SelectedIndex == 0)
                {
                    divDateWiseReport.Visible = true;
                }
            }
            chkNeedDateWiseTotal.Checked = true;
            chkNeedOverallTotal.Checked = true;
            chkNeedSubjectTotal.Checked = false;
            chkWithoutRegularArrear.Checked = true;
            if (!chkDepartmentWise.Checked)
            {
                chkNeedSubjectTotal.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlReportFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            divDateWiseReport.Visible = false;
            if (ddlReportFormat.Items.Count > 0)
            {
                if (ddlReportFormat.SelectedIndex == 0)
                {
                    divDateWiseReport.Visible = true;
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

    protected void FpStudentStrength_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpStudentStrength.SaveChanges();
            int r = FpStudentStrength.Sheets[0].ActiveRow;
            int j = FpStudentStrength.Sheets[0].ActiveColumn;
            if (r == 0 && j == 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpStudentStrength.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpStudentStrength.Sheets[0].RowCount; row++)
                {
                    if (FpStudentStrength.Sheets[0].Cells[row, 0].Text != string.Empty)
                    {
                        if (val == 1)
                            FpStudentStrength.Sheets[0].Cells[row, j].Value = 1;
                        else
                            FpStudentStrength.Sheets[0].Cells[row, j].Value = 0;
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
            btnPrintCoverSheet.Text = "Cover Sheet";

            DataSet dsAllStudentsStrength = new DataSet();
            DataSet dsMaleStrength = new DataSet();
            DataSet dsFemaleStrength = new DataSet();

            if (chkBasedOnSeating.Checked)
            {
                isBasedOnSeatingArrangement = true;
                btnPrintCoverSheet.Text = "Phasing Sheet";
            }

            string qryRedoBatch = string.Empty;
            string qryRedoDegreeCode = string.Empty;
            bool isRedoStud = true;

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
                    qryExamYear = " and et.Exam_Year in(" + ExamYear + ")";
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
                    qryExamMonth = " and et.Exam_Month in(" + ExamMonth + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamMonth.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
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

            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(qryExamDates) && !string.IsNullOrEmpty(qryExamSessions) && !string.IsNullOrEmpty(qrySubjectCodes))
            {
                int serialNo = 0;
                if (ddlReportFormat.SelectedIndex == 1)
                {
                    qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year  " + qryCollege + qryDegreeCode + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    dsAllStudentsStrength = da.select_method_wo_parameter(qry, "text");
                    qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and IsNull(a.sex,'0')='0' " + qryCollege + qryDegreeCode + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    dsMaleStrength = da.select_method_wo_parameter(qry, "text");
                    qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and IsNull(a.sex,'0')='1' " + qryCollege + qryDegreeCode + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    dsFemaleStrength = da.select_method_wo_parameter(qry, "text");

                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year  " + qryCollege + qryDegreeCode + qryExamDates+qrySubjectCodes + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    //dsAllStudentsStrength = da.select_method_wo_parameter(qry, "text");
                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and IsNull(a.sex,'0')='0' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    //dsMaleStrength = da.select_method_wo_parameter(qry, "text");
                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,Count(distinct ea.roll_no) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and ed.degree_code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and IsNull(a.sex,'0')='1' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc";
                    //dsFemaleStrength = da.select_method_wo_parameter(qry, "text");

                    if (dsAllStudentsStrength.Tables.Count > 0 && dsAllStudentsStrength.Tables[0].Rows.Count > 0)
                    {
                        Init_Spread(FpStudentStrength, 1);
                        serialNo = 0;
                        foreach (DataRow drDateWise in dsAllStudentsStrength.Tables[0].Rows)
                        {
                            serialNo++;
                            string examDate1 = Convert.ToString(drDateWise["edate"]).Trim();
                            string examDateNew = Convert.ToString(drDateWise["exam_date"]).Trim();
                            string examSession1 = Convert.ToString(drDateWise["exam_session"]).Trim();
                            string totalStudent = Convert.ToString(drDateWise["Strength"]).Trim();

                            int studentCount = 0;
                            int maleCount = 0;
                            int femaleCount = 0;

                            string maleCounts = string.Empty;
                            string femaleCounts = string.Empty;
                            int.TryParse(totalStudent, out studentCount);

                            DataTable dtMale = new DataTable();
                            DataTable dtFemale = new DataTable();
                            if (dsMaleStrength.Tables.Count > 0 && dsMaleStrength.Tables[0].Rows.Count > 0)
                            {
                                dsMaleStrength.Tables[0].DefaultView.RowFilter = "exam_date='" + examDateNew + "' and exam_session='" + examSession1 + "'";
                                dtMale = dsMaleStrength.Tables[0].DefaultView.ToTable();
                            }
                            if (dsFemaleStrength.Tables.Count > 0 && dsFemaleStrength.Tables[0].Rows.Count > 0)
                            {
                                dsFemaleStrength.Tables[0].DefaultView.RowFilter = "exam_date='" + examDateNew + "' and exam_session='" + examSession1 + "'";
                                dtFemale = dsFemaleStrength.Tables[0].DefaultView.ToTable();
                            }
                            if (dtMale.Rows.Count > 0)
                            {
                                maleCounts = Convert.ToString(dtMale.Rows[0]["Strength"]).Trim();
                            }
                            if (dtFemale.Rows.Count > 0)
                            {
                                femaleCounts = Convert.ToString(dtFemale.Rows[0]["Strength"]).Trim();
                            }
                            int.TryParse(maleCounts.Trim(), out maleCount);
                            int.TryParse(femaleCounts.Trim(), out femaleCount);

                            FpStudentStrength.Sheets[0].RowCount++;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examDate).Trim();
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(examSession).Trim();
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].Text = examDate1 + "\t\t" + examSession1;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].Text = ((studentCount != 0) ? Convert.ToString(studentCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].Text = ((maleCount != 0) ? Convert.ToString(maleCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].Text = ((femaleCount != 0) ? Convert.ToString(femaleCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        }
                        FpStudentStrength.SaveChanges();
                        FpStudentStrength.Sheets[0].PageSize = FpStudentStrength.Sheets[0].RowCount;
                        FpStudentStrength.Height = 500;
                        FpStudentStrength.SaveChanges();
                        FpStudentStrength.Visible = true;
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
                else
                {
                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when ead.attempts=0 then 'Regular' else  'Arrear' end as status,sm.semester,dg.college_code,Count(distinct ea.Roll_No) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sm,Degree dg,Course c,Department dt,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=dg.Degree_Code and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=ed.degree_code and ed.degree_code=sm.degree_code and ed.degree_code=et.degree_code and r.college_code=dg.college_code and r.college_code=c.college_code and c.college_code=dt.college_code and dt.college_code=dg.college_code and sm.degree_code=et.degree_code and sm.degree_code=dg.Degree_Code and dg.Degree_Code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.syll_code=sm.syll_code and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year  " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,ead.attempts,sm.semester,dg.college_code order by etd.exam_date asc,etd.exam_session desc,s.subject_code,r.degree_code,status desc,sm.semester";
                    qry = "select count(distinct ea.roll_no) as Strength,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else '1' end as StatusCode,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then 'Regular' else  'Arrear' end as status,etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code from  exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and ed.degree_code=et.degree_code and et.batchFrom=ed.batch_year and et.degree_code=r.degree_code and et.batchFrom=r.Batch_Year and etd.subject_no=ead.subject_no " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code,ead.attempts ";
                    DataSet dsAllStudentsStrengthCount = da.select_method_wo_parameter(qry, "text");
                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when ead.attempts=0 then 'Regular' else  'Arrear' end as status ,sm.semester,dg.college_code,Count(distinct ea.Roll_No) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sm,Degree dg,Course c,Department dt,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=dg.Degree_Code and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=ed.degree_code and ed.degree_code=sm.degree_code and ed.degree_code=et.degree_code and r.college_code=dg.college_code and r.college_code=c.college_code and c.college_code=dt.college_code and dt.college_code=dg.college_code and sm.degree_code=et.degree_code and sm.degree_code=dg.Degree_Code and dg.Degree_Code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.syll_code=sm.syll_code and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and ISNULL(a.sex,'0')='0' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,ead.attempts,sm.semester,dg.college_code order by etd.exam_date asc,etd.exam_session desc,s.subject_code,r.degree_code,status desc,sm.semester";
                    qry = "select count(distinct ea.roll_no) as Strength,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then 'Regular' else  'Arrear' end as status,etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code from  exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and ed.degree_code=et.degree_code and et.batchFrom=ed.batch_year and et.degree_code=r.degree_code and et.batchFrom=r.Batch_Year and etd.subject_no=ead.subject_no and IsNull(a.sex,'0')='0' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code,ead.attempts ";
                    dsMaleStrength = da.select_method_wo_parameter(qry, "text");
                    //qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when ead.attempts=0 then 'Regular' else  'Arrear' end as status ,sm.semester,dg.college_code,Count(distinct ea.Roll_No) as Strength from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sm,Degree dg,Course c,Department dt,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=dg.Degree_Code and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=ed.degree_code and ed.degree_code=sm.degree_code and ed.degree_code=et.degree_code and r.college_code=dg.college_code and r.college_code=c.college_code and c.college_code=dt.college_code and dt.college_code=dg.college_code and sm.degree_code=et.degree_code and sm.degree_code=dg.Degree_Code and dg.Degree_Code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.syll_code=sm.syll_code and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year and IsNull(a.sex,'0')='1' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,r.degree_code,c.Course_Name,dt.Dept_Name,s.subject_code,s.subject_name,ead.attempts,sm.semester,dg.college_code order by etd.exam_date asc,etd.exam_session desc,s.subject_code,r.degree_code,status desc,sm.semester";
                    qry = "select count(distinct ea.roll_no) as Strength,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then 'Regular' else  'Arrear' end as status,etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code from  exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and ed.degree_code=et.degree_code and et.batchFrom=ed.batch_year and et.degree_code=r.degree_code and et.batchFrom=r.Batch_Year and etd.subject_no=ead.subject_no and IsNull(a.sex,'0')='1' " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " group by etd.exam_date,etd.exam_session,s.subject_code,s.subject_name,r.degree_code,ead.attempts ";
                    dsFemaleStrength = da.select_method_wo_parameter(qry, "text");
                    qry = "select distinct convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,r.degree_code,c.edu_level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,s.subject_code,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_name,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then '0' else  '1' end as StatusCode,case when LTRIM(RTRIM(ISNULL(ead.attempts,'0')))=0 then 'Regular' else  'Arrear' end as status,sm.semester,dg.college_code from exmtt et,exmtt_det etd,Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sm,Degree dg,Course c,Department dt,Registration r,applyn a where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=dg.Degree_Code and r.degree_code=ed.degree_code and r.degree_code=et.degree_code and r.Batch_Year=ed.batch_year and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=ed.degree_code and ed.degree_code=sm.degree_code and ed.degree_code=et.degree_code and r.college_code=dg.college_code and r.college_code=c.college_code and c.college_code=dt.college_code and dt.college_code=dg.college_code and sm.degree_code=et.degree_code and sm.degree_code=dg.Degree_Code and dg.Degree_Code=et.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and et.exam_code=etd.exam_code and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and s.syll_code=sm.syll_code and s.subject_no=ead.subject_no and ead.subject_no=etd.subject_no and s.subject_no=etd.subject_no and et.batchFrom=ed.batch_year  " + qryCollege + qryDegreeCode + qrySubjectCodes + qryExamDates + qryExamSessions + qryExamMonth + qryExamYear + " order by etd.exam_date asc,etd.exam_session desc,c.Edu_Level desc,s.subject_code,dt.dept_acronym,status desc,sm.semester";//order by etd.exam_date asc,etd.exam_session desc,c.Edu_Level,dt.dept_acronym,subjectpriority,s.subject_code,status desc,sm.semester order by etd.exam_date asc,etd.exam_session desc,c.edu_level desc,s.subjectpriority,s.subject_code,status desc,sm.semester
                    dsAllStudentsStrength = da.select_method_wo_parameter(qry, "text");

                    if (dsAllStudentsStrength.Tables.Count > 0 && dsAllStudentsStrength.Tables[0].Rows.Count > 0)
                    {
                        Init_Spread(FpStudentStrength, 0);
                        serialNo = 0;
                        DataTable dtDistinctDateSession = new DataTable();
                        DataTable dtDistinctCourses = new DataTable();
                        DataTable dtDistinctSubjectCode = new DataTable();
                        DataTable dtDateWiseSubjectList = new DataTable();

                        dtDistinctDateSession = dsAllStudentsStrength.Tables[0].DefaultView.ToTable(true, "edate", "exam_date", "exam_session");
                        int OverallStudentCount = 0;
                        int OverallMaleCount = 0;
                        int OverallFemaleCount = 0;

                        Dictionary<string, int> dicDateWiseAllStudentCount = new Dictionary<string, int>();
                        Dictionary<string, int> dicDateWiseMaleStudentCount = new Dictionary<string, int>();
                        Dictionary<string, int> dicDateWiseFemaleStudentCount = new Dictionary<string, int>();


                        Dictionary<string, int> dicOverAllStudentCount = new Dictionary<string, int>();
                        dicOverAllStudentCount.Clear();
                        //Dictionary<string, int> dicOverAllMaleStudentCount = new Dictionary<string, int>();
                        //Dictionary<string, int> dicOverAllFemaleStudentCount = new Dictionary<string, int>();
                        dicOverAllStudentCount.Add("1", 0);
                        dicOverAllStudentCount.Add("2", 0);
                        dicOverAllStudentCount.Add("3", 0);
                        foreach (DataRow drDateWise in dtDistinctDateSession.Rows)
                        {
                            string examDate1 = Convert.ToString(drDateWise["edate"]).Trim();
                            string examDateNew = Convert.ToString(drDateWise["exam_date"]).Trim();
                            string examSession1 = Convert.ToString(drDateWise["exam_session"]).Trim();
                            string tempSubjectCode = string.Empty;
                            string degreeCodeNew = string.Empty;
                            string courseName = string.Empty;
                            string departmentName = string.Empty;
                            string subjectCode = string.Empty;
                            string subjectName = string.Empty;
                            string status = string.Empty;
                            string statusCode = string.Empty;
                            string semesterValue = string.Empty;
                            string collegeCode = string.Empty;
                            string strength = string.Empty;

                            int DateWiseStudentCount = 0;
                            int DateWiseMaleCount = 0;
                            int DateWiseFemaleCount = 0;

                            int SubjectWiseStudentCount = 0;
                            int SubjectWiseMaleCount = 0;
                            int SubjectWiseFemaleCount = 0;

                            ArrayList arrDegreeDetails = new ArrayList();

                            FpStudentStrength.Sheets[0].RowCount++;
                            string dateWiseKey = examDate1 + "@" + examSession1;

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Text = examDate1 + "\t\t" + examSession1;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#458547");
                            FpStudentStrength.Sheets[0].Rows[FpStudentStrength.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#458547");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentStrength.Sheets[0].AddSpanCell(FpStudentStrength.Sheets[0].RowCount - 1, 0, 1, FpStudentStrength.Sheets[0].ColumnCount);

                            dsAllStudentsStrength.Tables[0].DefaultView.RowFilter = "edate='" + examDate1 + "' and exam_session='" + examSession1 + "'";
                            //dsAllStudentsStrength.Tables[0].DefaultView.Sort = "";
                            //dsAllStudentsStrength.Tables[0].DefaultView.Sort = "exam_date,exam_session desc,subjectpriority,subject_code";
                            dtDateWiseSubjectList = dsAllStudentsStrength.Tables[0].DefaultView.ToTable();
                            dsAllStudentsStrength.Tables[0].DefaultView.RowFilter = "edate='" + examDate1 + "' and exam_session='" + examSession1 + "'";
                            dtDistinctCourses = dsAllStudentsStrength.Tables[0].DefaultView.ToTable(true, "Course_Name", "Dept_Name", "subject_code");
                            dsAllStudentsStrength.Tables[0].DefaultView.RowFilter = "edate='" + examDate1 + "' and exam_session='" + examSession1 + "'";
                            dtDistinctSubjectCode = dsAllStudentsStrength.Tables[0].DefaultView.ToTable(true, "subject_code");

                            int subjectCount = 0;
                            Dictionary<string, int> dicTotalStudentCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicMaleStudentCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicFemaleStudentCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicRowCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicRowCountSubTot = new Dictionary<string, int>();

                            Dictionary<string, int> dicSubjectWiseAllStudentCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicSubjectWiseMaleStudentCount = new Dictionary<string, int>();
                            Dictionary<string, int> dicSubjectWiseFemaleStudentCount = new Dictionary<string, int>();
                            int countTotalStudent = 0;
                            int countMaleStudent = 0;
                            int countFemaleStudent = 0;

                            ArrayList arrStudents = new ArrayList();
                            foreach (DataRow drDateWiseList in dtDateWiseSubjectList.Rows)
                            {
                                subjectCount++;
                                string totalStudent = string.Empty;
                                examDate1 = Convert.ToString(drDateWiseList["edate"]).Trim();
                                examDateNew = Convert.ToString(drDateWiseList["exam_date"]).Trim();
                                examSession1 = Convert.ToString(drDateWiseList["exam_session"]).Trim();
                                degreeCodeNew = Convert.ToString(drDateWiseList["degree_code"]).Trim();
                                courseName = Convert.ToString(drDateWiseList["Course_Name"]).Trim();
                                departmentName = Convert.ToString(drDateWiseList["Dept_Name"]).Trim();
                                subjectCode = Convert.ToString(drDateWiseList["subject_code"]).Trim();
                                subjectName = Convert.ToString(drDateWiseList["subject_name"]).Trim();
                                status = Convert.ToString(drDateWiseList["status"]).Trim();
                                statusCode = Convert.ToString(drDateWiseList["StatusCode"]).Trim();
                                semesterValue = Convert.ToString(drDateWiseList["semester"]).Trim();
                                collegeCode = Convert.ToString(drDateWiseList["college_code"]).Trim();
                                //strength = Convert.ToString(drDateWiseList["Strength"]).Trim();
                                //string totalStudent = Convert.ToString(drDateWiseList["Strength"]).Trim();

                                //string qryStatusCode = " and LTRIM(RTRIM(ISnull(ead.attempts,'0')))=0";
                                //if (statusCode.Trim().ToLower() == "1")
                                //{
                                //    qryStatusCode = " and LTRIM(RTRIM(ISnull(ead.attempts,'0')))>0";
                                //}
                                //string qryStatusCode = " and  LTRIM(RTRIM(ISnull(ead.attempts,'0')))=0";
                                //if (status.Trim().ToLower() == "arrear")
                                //{
                                //    qryStatusCode = " and LTRIM(RTRIM(ISnull(ead.attempts,'0')))<>0";
                                //}
                                //totalStudent = da.GetFunction("select count(distinct ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and s.subject_code='" + subjectCode + "' and ed.degree_code='" + degreeCodeNew + "' and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryStatusCode + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' and e.degree_code='" + degreeCodeNew + "' and s1.subject_code='" + subjectCode + "')");
                                //if (string.IsNullOrEmpty(tempSubjectCode))
                                //{
                                //    tempSubjectCode = subjectCode;
                                //}
                                string keyValue = string.Empty;
                                string keyValue1 = string.Empty;
                                if (chkDepartmentWise.Checked)
                                {
                                    keyValue = Convert.ToString(courseName).Trim() + '-' + Convert.ToString(departmentName).Trim().ToLower() + "@";
                                }

                                keyValue += Convert.ToString(Convert.ToString(subjectCode).Trim().ToLower()).Trim().ToLower();
                                if (!chkWithoutRegularArrear.Checked)
                                {
                                    keyValue += "-" + status.Trim().ToLower();
                                }
                                
                                if (string.IsNullOrEmpty(tempSubjectCode))
                                {
                                    tempSubjectCode = subjectCode;
                                }
                                if (tempSubjectCode != subjectCode)
                                {
                                    if (chkNeedSubjectTotal.Checked)
                                    {
                                        keyValue1 = string.Empty;
                                        keyValue1 += Convert.ToString(Convert.ToString(tempSubjectCode).Trim().ToLower()).Trim().ToLower();
                                        SubjectWiseStudentCount = 0;
                                        if (dicSubjectWiseAllStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseAllStudentCount[keyValue1];
                                            SubjectWiseStudentCount = dicSubjectWiseAllStudentCount[keyValue1];
                                        }
                                        SubjectWiseMaleCount = 0;
                                        if (dicSubjectWiseMaleStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseMaleStudentCount[keyValue1];
                                            SubjectWiseMaleCount = dicSubjectWiseMaleStudentCount[keyValue1];
                                        }
                                        SubjectWiseFemaleCount = 0;
                                        if (dicSubjectWiseFemaleStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseFemaleStudentCount[keyValue1];
                                            SubjectWiseFemaleCount = dicSubjectWiseFemaleStudentCount[keyValue1];
                                        }
                                        int subTotRows = 0;
                                        if (!dicRowCountSubTot.ContainsKey(keyValue1.Trim().ToLower()))
                                        {
                                            FpStudentStrength.Sheets[0].RowCount++;
                                            dicRowCountSubTot.Add(keyValue1.Trim().ToLower(), FpStudentStrength.Sheets[0].RowCount - 1);
                                            subTotRows = FpStudentStrength.Sheets[0].RowCount - 1;
                                        }
                                        else
                                        {
                                            subTotRows = dicRowCountSubTot[keyValue1.Trim().ToLower()];
                                        }
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].Text = "Sub Total :";
                                        //FpStudentStrength.Sheets[0].Rows[subTotRows ].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 0].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 0].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].HorizontalAlign = HorizontalAlign.Right;
                                        FpStudentStrength.Sheets[0].AddSpanCell(subTotRows, 0, 1, 6);

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].Text = ((SubjectWiseStudentCount != 0) ? Convert.ToString(SubjectWiseStudentCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 6].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 6].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].HorizontalAlign = HorizontalAlign.Center;

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].Text = ((SubjectWiseMaleCount != 0) ? Convert.ToString(SubjectWiseMaleCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 7].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 7].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].HorizontalAlign = HorizontalAlign.Center;

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].Text = ((SubjectWiseFemaleCount != 0) ? Convert.ToString(SubjectWiseFemaleCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 8].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 8].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                    tempSubjectCode = subjectCode;
                                    SubjectWiseStudentCount = 0;
                                    SubjectWiseMaleCount = 0;
                                    SubjectWiseFemaleCount = 0;

                                    countTotalStudent = 0;
                                    countMaleStudent = 0;
                                    countFemaleStudent = 0;
                                }

                                keyValue1 = string.Empty;
                                keyValue1 += Convert.ToString(Convert.ToString(tempSubjectCode).Trim().ToLower()).Trim().ToLower();
                                int studentCount = 0;
                                int maleCount = 0;
                                int femaleCount = 0;

                                string maleCounts = string.Empty;
                                string femaleCounts = string.Empty;
                                DataTable dtAll = new DataTable();
                                DataTable dtMale = new DataTable();
                                DataTable dtFemale = new DataTable();
                                string keyDegWise = examDate1 + "@" + examSession1 + "@" + degreeCodeNew + "@" + subjectCode + "@" + status;
                                if (dsAllStudentsStrengthCount.Tables.Count > 0 && dsAllStudentsStrengthCount.Tables[0].Rows.Count > 0)
                                {
                                    dsAllStudentsStrengthCount.Tables[0].DefaultView.RowFilter = "exam_date='" + examDateNew + "' and exam_session='" + examSession1 + "' and degree_code='" + degreeCodeNew + "' and subject_code='" + subjectCode + "' and status='" + status + "'";
                                    dtAll = dsAllStudentsStrengthCount.Tables[0].DefaultView.ToTable();
                                }
                                if (dsMaleStrength.Tables.Count > 0 && dsMaleStrength.Tables[0].Rows.Count > 0)
                                {
                                    dsMaleStrength.Tables[0].DefaultView.RowFilter = "exam_date='" + examDateNew + "' and exam_session='" + examSession1 + "' and degree_code='" + degreeCodeNew + "' and subject_code='" + subjectCode + "' and status='" + status + "'";
                                    dtMale = dsMaleStrength.Tables[0].DefaultView.ToTable();
                                }
                                if (dsFemaleStrength.Tables.Count > 0 && dsFemaleStrength.Tables[0].Rows.Count > 0)
                                {
                                    dsFemaleStrength.Tables[0].DefaultView.RowFilter = "exam_date='" + examDateNew + "' and exam_session='" + examSession1 + "' and degree_code='" + degreeCodeNew + "' and subject_code='" + subjectCode + "' and status='" + status + "'";
                                    dtFemale = dsFemaleStrength.Tables[0].DefaultView.ToTable();
                                }
                                if (!arrStudents.Contains(keyDegWise.Trim().ToLower()))
                                {
                                    if (dtAll.Rows.Count > 0)
                                    {
                                        object males = dtAll.Compute("Sum(Strength)", "Strength<>'0'");
                                        //totalStudent = Convert.ToString(dtMale.Rows[0]["Strength"]).Trim();
                                        totalStudent = Convert.ToString(males).Trim();
                                    }
                                    if (dtMale.Rows.Count > 0)
                                    {
                                        object males = dtMale.Compute("Sum(Strength)", "Strength<>'0'");
                                        //maleCounts = Convert.ToString(dtMale.Rows[0]["Strength"]).Trim();
                                        maleCounts = Convert.ToString(males).Trim();
                                    }
                                    if (dtFemale.Rows.Count > 0)
                                    {
                                        object males = dtFemale.Compute("Sum(Strength)", "Strength<>'0'");
                                        //femaleCounts = Convert.ToString(dtFemale.Rows[0]["Strength"]).Trim();
                                        femaleCounts = Convert.ToString(males).Trim();
                                    }
                                    arrStudents.Add(keyDegWise.Trim().ToLower());
                                }
                                else
                                {

                                }
                                int.TryParse(totalStudent, out studentCount);
                                //countTotalStudent += studentCount;
                                SubjectWiseStudentCount += studentCount;
                                DateWiseStudentCount += studentCount;
                                OverallStudentCount += studentCount;
                                if (dicTotalStudentCount.ContainsKey(keyValue))
                                {
                                    int count = dicTotalStudentCount[keyValue];
                                    dicTotalStudentCount[keyValue] += studentCount;
                                    countTotalStudent = dicTotalStudentCount[keyValue];
                                }
                                else
                                {
                                    dicTotalStudentCount.Add(keyValue, studentCount);
                                    countTotalStudent = dicTotalStudentCount[keyValue];
                                }
                                if (dicSubjectWiseAllStudentCount.ContainsKey(keyValue1))
                                {
                                    int count = dicSubjectWiseAllStudentCount[keyValue1];
                                    dicSubjectWiseAllStudentCount[keyValue1] += studentCount;
                                    //SubjectWiseStudentCount = dicSubjectWiseAllStudentCount[keyValue];
                                }
                                else
                                {
                                    dicSubjectWiseAllStudentCount.Add(keyValue1, studentCount);
                                    //SubjectWiseStudentCount = dicSubjectWiseAllStudentCount[keyValue];
                                }
                                if (!dicDateWiseAllStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    dicDateWiseAllStudentCount.Add(dateWiseKey.Trim().ToLower(), studentCount);
                                }
                                else
                                {
                                    dicDateWiseAllStudentCount[dateWiseKey.Trim().ToLower()] += studentCount;
                                }

                                if (!dicOverAllStudentCount.ContainsKey("1"))
                                {
                                    dicOverAllStudentCount.Add("1", studentCount);
                                }
                                else
                                {
                                    dicOverAllStudentCount["1"] += studentCount;
                                }
                                //maleCounts = da.GetFunction("select count(distinct ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and IsNull(a.sex,'0')='0' and s.subject_code='" + subjectCode + "' and ed.degree_code='" + degreeCodeNew + "' and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryStatusCode + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' and e.degree_code='" + degreeCodeNew + "' and s1.subject_code='" + subjectCode + "')");

                                //femaleCounts = da.GetFunction("select count(distinct ea.roll_no) from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,applyn a where r.App_No=a.app_no and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and ed.batch_year=r.Batch_Year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and IsNull(a.sex,'0')='1' and s.subject_code='" + subjectCode + "' and ed.degree_code='" + degreeCodeNew + "' and ed.Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' " + qryStatusCode + " and s.subject_no in(select et.subject_no from exmtt e,exmtt_det et,subject s1 where e.exam_code=et.exam_code and s1.subject_no=et.subject_no and e.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlExamYear.SelectedValue).Trim() + "' and e.degree_code='" + degreeCodeNew + "' and s1.subject_code='" + subjectCode + "')");

                                int.TryParse(maleCounts.Trim(), out maleCount);
                                int.TryParse(femaleCounts.Trim(), out femaleCount);

                                //countMaleStudent += maleCount;
                                SubjectWiseMaleCount += maleCount;
                                DateWiseMaleCount += maleCount;
                                OverallMaleCount += maleCount;

                                if (dicMaleStudentCount.ContainsKey(keyValue))
                                {
                                    int count = dicMaleStudentCount[keyValue];
                                    dicMaleStudentCount[keyValue] += maleCount;
                                    countMaleStudent = dicMaleStudentCount[keyValue];
                                }
                                else
                                {
                                    dicMaleStudentCount.Add(keyValue, maleCount);
                                    countMaleStudent = dicMaleStudentCount[keyValue];
                                }
                                if (dicSubjectWiseMaleStudentCount.ContainsKey(keyValue1))
                                {
                                    int count = dicSubjectWiseMaleStudentCount[keyValue1];
                                    dicSubjectWiseMaleStudentCount[keyValue1] += maleCount;
                                    //SubjectWiseMaleCount = dicSubjectWiseMaleStudentCount[keyValue];
                                }
                                else
                                {
                                    dicSubjectWiseMaleStudentCount.Add(keyValue1, maleCount);
                                    //SubjectWiseMaleCount = dicSubjectWiseMaleStudentCount[keyValue];
                                }
                                if (!dicDateWiseMaleStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    dicDateWiseMaleStudentCount.Add(dateWiseKey.Trim().ToLower(), maleCount);
                                }
                                else
                                {
                                    dicDateWiseMaleStudentCount[dateWiseKey.Trim().ToLower()] += maleCount;
                                }
                                if (!dicOverAllStudentCount.ContainsKey("2"))
                                {
                                    dicOverAllStudentCount.Add("2", maleCount);
                                }
                                else
                                {
                                    dicOverAllStudentCount["2"] += maleCount;
                                }
                                //countFemaleStudent += femaleCount;
                                SubjectWiseFemaleCount += femaleCount;
                                DateWiseFemaleCount += femaleCount;
                                OverallFemaleCount += femaleCount;

                                if (dicFemaleStudentCount.ContainsKey(keyValue))
                                {
                                    int count = dicFemaleStudentCount[keyValue];
                                    dicFemaleStudentCount[keyValue] += femaleCount;
                                    countFemaleStudent = dicFemaleStudentCount[keyValue];
                                }
                                else
                                {
                                    dicFemaleStudentCount.Add(keyValue, femaleCount);
                                    countFemaleStudent = dicFemaleStudentCount[keyValue];
                                }
                                if (dicSubjectWiseFemaleStudentCount.ContainsKey(keyValue1))
                                {
                                    int count = dicSubjectWiseFemaleStudentCount[keyValue1];
                                    dicSubjectWiseFemaleStudentCount[keyValue1] += femaleCount;
                                    //SubjectWiseFemaleCount = dicSubjectWiseFemaleStudentCount[keyValue];
                                }
                                else
                                {
                                    dicSubjectWiseFemaleStudentCount.Add(keyValue1, femaleCount);
                                    //SubjectWiseFemaleCount = dicSubjectWiseFemaleStudentCount[keyValue];
                                }
                                if (!dicDateWiseFemaleStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    dicDateWiseFemaleStudentCount.Add(dateWiseKey.Trim().ToLower(), femaleCount);
                                }
                                else
                                {
                                    dicDateWiseFemaleStudentCount[dateWiseKey.Trim().ToLower()] += femaleCount;
                                }
                                if (!dicOverAllStudentCount.ContainsKey("3"))
                                {
                                    dicOverAllStudentCount.Add("3", femaleCount);
                                }
                                else
                                {
                                    dicOverAllStudentCount["3"] += femaleCount;
                                }
                                if (!arrDegreeDetails.Contains(keyValue))
                                {
                                    serialNo++;
                                    FpStudentStrength.Sheets[0].RowCount++;
                                    if (!dicRowCount.ContainsKey(keyValue))
                                    {
                                        dicRowCount.Add(keyValue, FpStudentStrength.Sheets[0].RowCount - 1);
                                    }
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examDate).Trim();
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(examSession).Trim();
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].Text = courseName + "\t-\t" + departmentName;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].Note = degreeCodeNew;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].Text = subjectCode;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].Text = subjectName;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].Text = semesterValue;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 5].Text = status;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 5].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Text = ((studentCount != 0) ? Convert.ToString(studentCount).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Text = ((maleCount != 0) ? Convert.ToString(maleCount).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Text = ((femaleCount != 0) ? Convert.ToString(femaleCount).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    arrDegreeDetails.Add(keyValue);
                                }
                                else
                                {
                                    int rowValue = FpStudentStrength.Sheets[0].RowCount - 1;
                                    if (dicRowCount.ContainsKey(keyValue))
                                    {
                                        rowValue = dicRowCount[keyValue];
                                    }
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 6].Text = ((countTotalStudent != 0) ? Convert.ToString(countTotalStudent).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 6].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 6].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 6].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[rowValue, 7].Text = ((countMaleStudent != 0) ? Convert.ToString(countMaleStudent).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 7].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 7].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 7].HorizontalAlign = HorizontalAlign.Center;

                                    FpStudentStrength.Sheets[0].Cells[rowValue, 8].Text = ((countFemaleStudent != 0) ? Convert.ToString(countFemaleStudent).Trim() : "--");
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 8].Locked = true;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 8].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentStrength.Sheets[0].Cells[rowValue, 8].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (subjectCount == dtDateWiseSubjectList.Rows.Count)
                                {
                                    if (chkNeedSubjectTotal.Checked)
                                    {
                                        //FpStudentStrength.Sheets[0].RowCount++;
                                        SubjectWiseStudentCount = 0;
                                        if (dicSubjectWiseAllStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseAllStudentCount[keyValue1];
                                            SubjectWiseStudentCount = dicSubjectWiseAllStudentCount[keyValue1];
                                        }
                                        SubjectWiseMaleCount = 0;
                                        if (dicSubjectWiseMaleStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseMaleStudentCount[keyValue1];
                                            SubjectWiseMaleCount = dicSubjectWiseMaleStudentCount[keyValue1];
                                        }
                                        SubjectWiseFemaleCount = 0;
                                        if (dicSubjectWiseFemaleStudentCount.ContainsKey(keyValue1))
                                        {
                                            int count = dicSubjectWiseFemaleStudentCount[keyValue1];
                                            SubjectWiseFemaleCount = dicSubjectWiseFemaleStudentCount[keyValue1];
                                        }
                                        int subTotRows = 0;//FpStudentStrength.Sheets[0].RowCount - 1;
                                        if (!dicRowCountSubTot.ContainsKey(keyValue1.Trim().ToLower()))
                                        {
                                            FpStudentStrength.Sheets[0].RowCount++;
                                            dicRowCountSubTot.Add(keyValue1.Trim().ToLower(), FpStudentStrength.Sheets[0].RowCount - 1);
                                            subTotRows = FpStudentStrength.Sheets[0].RowCount - 1;
                                        }
                                        else
                                        {
                                            subTotRows = dicRowCountSubTot[keyValue1.Trim().ToLower()];
                                        }
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].Text = "Sub Total :";
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 0].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Rows[subTotRows ].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 0].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 0].HorizontalAlign = HorizontalAlign.Right;
                                        FpStudentStrength.Sheets[0].AddSpanCell(subTotRows, 0, 1, 6);

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].Text = ((SubjectWiseStudentCount != 0) ? Convert.ToString(SubjectWiseStudentCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 6].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 6].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 6].HorizontalAlign = HorizontalAlign.Center;

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].Text = ((SubjectWiseMaleCount != 0) ? Convert.ToString(SubjectWiseMaleCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 7].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 7].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 7].HorizontalAlign = HorizontalAlign.Center;

                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].Text = ((SubjectWiseFemaleCount != 0) ? Convert.ToString(SubjectWiseFemaleCount).Trim() : "--");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 8].BackColor = ColorTranslator.FromHtml("#008080");
                                        //FpStudentStrength.Sheets[0].Cells[subTotRows , 8].ForeColor = ColorTranslator.FromHtml("#0000FF");
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].Locked = true;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].VerticalAlign = VerticalAlign.Middle;
                                        FpStudentStrength.Sheets[0].Cells[subTotRows, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                            if (chkNeedDateWiseTotal.Checked)
                            {
                                FpStudentStrength.Sheets[0].RowCount++;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total :";
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentStrength.Sheets[0].AddSpanCell(FpStudentStrength.Sheets[0].RowCount - 1, 0, 1, 6);
                                DateWiseStudentCount = 0;
                                if (dicDateWiseAllStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    int count = dicDateWiseAllStudentCount[dateWiseKey.Trim().ToLower()];
                                    DateWiseStudentCount = dicDateWiseAllStudentCount[dateWiseKey.Trim().ToLower()];
                                }
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Text = ((DateWiseStudentCount != 0) ? Convert.ToString(DateWiseStudentCount).Trim() : "--");
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Locked = true;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                                DateWiseMaleCount = 0;
                                if (dicDateWiseMaleStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    int count = dicDateWiseMaleStudentCount[dateWiseKey.Trim().ToLower()];
                                    DateWiseMaleCount = dicDateWiseMaleStudentCount[dateWiseKey.Trim().ToLower()];
                                }
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Text = ((DateWiseMaleCount != 0) ? Convert.ToString(DateWiseMaleCount).Trim() : "--");
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Locked = true;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                DateWiseFemaleCount = 0;
                                if (dicDateWiseFemaleStudentCount.ContainsKey(dateWiseKey.Trim().ToLower()))
                                {
                                    int count = dicDateWiseFemaleStudentCount[dateWiseKey.Trim().ToLower()];
                                    DateWiseFemaleCount = dicDateWiseFemaleStudentCount[dateWiseKey.Trim().ToLower()];
                                }
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Text = ((DateWiseFemaleCount != 0) ? Convert.ToString(DateWiseFemaleCount).Trim() : "--");
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Locked = true;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        if (chkNeedOverallTotal.Checked)
                        {
                            FpStudentStrength.Sheets[0].RowCount++;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Text = "Over All Total :";
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpStudentStrength.Sheets[0].AddSpanCell(FpStudentStrength.Sheets[0].RowCount - 1, 0, 1, 6);

                            OverallStudentCount = 0;
                            if (dicOverAllStudentCount.ContainsKey("1"))
                            {
                                int count = dicOverAllStudentCount["1"];
                                OverallStudentCount = dicOverAllStudentCount["1"];
                            }

                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Text = ((OverallStudentCount != 0) ? Convert.ToString(OverallStudentCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                            OverallMaleCount = 0;
                            if (dicOverAllStudentCount.ContainsKey("2"))
                            {
                                int count = dicOverAllStudentCount["2"];
                                OverallMaleCount = dicOverAllStudentCount["2"];
                            }
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Text = ((OverallMaleCount != 0) ? Convert.ToString(OverallMaleCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                            OverallFemaleCount = 0;
                            if (dicOverAllStudentCount.ContainsKey("3"))
                            {
                                int count = dicOverAllStudentCount["3"];
                                OverallFemaleCount = dicOverAllStudentCount["3"];
                            }
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Text = ((OverallFemaleCount != 0) ? Convert.ToString(OverallFemaleCount).Trim() : "--");
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].Locked = true;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                            FpStudentStrength.Sheets[0].Cells[FpStudentStrength.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpStudentStrength.SaveChanges();
                        FpStudentStrength.Sheets[0].PageSize = FpStudentStrength.Sheets[0].RowCount;
                        FpStudentStrength.Height = 500;
                        FpStudentStrength.SaveChanges();
                        FpStudentStrength.Visible = true;
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Cover Sheet

    public void printCoverSheet()
    {
        try
        {
            FpStudentStrength.SaveChanges();
            string Line1 = string.Empty;
            string Line2 = string.Empty;
            string Line3 = string.Empty;
            string Line4 = string.Empty;
            string Line5 = string.Empty;
            string Line6 = string.Empty;
            string Line7 = string.Empty;
            string Line8 = string.Empty;
            PdfDocument mydocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
            PdfPage mypdfpage;

            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);

            Font Fontbold1 = new Font("Algerian", 13, FontStyle.Bold);
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
            if (FpStudentStrength.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpStudentStrength.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                    }
                }
            }
            DataSet dsColInfo = da.select_method_wo_parameter("select college_code,UPPER(collname)+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby from collinfo", "Text");
            if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
            {
                //Line1 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["Line1"]).Trim();
                //try
                //{
                //    string[] affli = Convert.ToString(dsColInfo.Tables[0].Rows[0]["affliatedby"]).Trim().Split('\\');
                //    Line2 = affli[0].Split(',')[0];
                //    Line4 = "(" + affli[2].Split(',')[0] + ")";
                //    Line3 = affli[1].Split(',')[0];
                //}
                //catch { }
                //Line5 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["distr"]).Trim();
            }
            Line6 = "COVER SHEET";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            //string studName = "STUDENT NAME : " + lblsname.Text.Trim().ToUpper();
            //string rollNumber = "ROLL NO : " + rollno.ToUpper();
            //string regNumber = "REG.NO : " + regNo.ToUpper();
            int posY = 0;
            bool status = false;
            if (selected)
            {
                for (int row = 1; row < FpStudentStrength.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    string rowno = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 0].Text).Trim();
                    if (sel == 1)
                    {
                        int PageNo = 1;
                        int ToatlPage = 1;
                        status = true;
                        bool pageHas = false;
                        posY = 10;
                        string allRegNo = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 0].Note).Trim();
                        string majorDept = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 1].Note).Trim();
                        string[] RegNo = allRegNo.Split(',');
                        string collcode = Convert.ToString(FpStudentStrength.Sheets[0].Cells[row, 3].Note).Trim();
                        if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                        {
                            DataView dvColege = new DataView();
                            dsColInfo.Tables[0].DefaultView.RowFilter = "college_code in('" + collcode + "')";
                            dvColege = dsColInfo.Tables[0].DefaultView;
                            collcode = Convert.ToString(dsColInfo.Tables[0].Rows[0]["college_code"]).Trim();
                            if (dvColege.Count > 0)
                            {
                                collcode = Convert.ToString(dvColege[0]["college_code"]).Trim();
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
                                if (RegNo.Length > 0)
                                {
                                    if (RegNo.Length % 50 == 0)
                                    {
                                        ToatlPage = RegNo.Length / 50;
                                    }
                                    else
                                    {
                                        ToatlPage = (RegNo.Length / 50) + 1;
                                    }
                                    pageHas = true;
                                    mypdfpage = mydocument.NewPage();
                                    PdfTable table2;
                                    Line6 = "COVER SHEET";
                                    Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                    subjectName = "SUBJECT TITLE\t\t:\t\t " + subName;
                                    subjectCode = "CODE\t\t:\t\t" + subCode;
                                    PdfImage LogoImage;
                                    PdfTablePage tblPage;
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                    {
                                        LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                        mypdfpage.Add(LogoImage, posY, 10, 500);
                                    }
                                    PdfTextArea pdfSince = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydocument, 15, 60, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                    mypdfpage.Add(pdfSince);

                                    PdfTextArea pdfLine1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                    mypdfpage.Add(pdfLine1);
                                    int rightY = posY;
                                    int neee = Convert.ToInt16((mydocument.PageWidth / 2) + 90);

                                    PdfTable paftblPageNo = mydocument.NewTable(Fontbold, 2, 1, 5);
                                    paftblPageNo.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    paftblPageNo.VisibleHeaders = false;
                                    paftblPageNo.Columns[0].SetWidth(50);

                                    paftblPageNo.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    paftblPageNo.Cell(0, 0).SetContent(PageNo);
                                    paftblPageNo.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    paftblPageNo.Cell(1, 0).SetContent(ToatlPage);

                                    tblPage = paftblPageNo.CreateTablePage(new PdfArea(mydocument, (mydocument.PageWidth - 100), rightY, 50, 80));
                                    mypdfpage.Add(tblPage);

                                    PdfTextArea pdfSubCode = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, subjectCode);
                                    mypdfpage.Add(pdfSubCode);

                                    posY += 20;
                                    PdfTextArea pdfLine2 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                    mypdfpage.Add(pdfLine2);

                                    rightY += 30;
                                    PdfTextArea pdfMajor = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "MAJOR : " + majorDept);
                                    mypdfpage.Add(pdfMajor);

                                    posY += 15;
                                    PdfTextArea pdfLine3 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                    mypdfpage.Add(pdfLine3);

                                    rightY += 30;
                                    PdfTextArea pdfDateSession = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE & DURATION\t\t:\t\t" + examDateNew + "-" + examSessionNew);
                                    mypdfpage.Add(pdfDateSession);

                                    posY += 15;
                                    PdfTextArea pdfLine4 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                    mypdfpage.Add(pdfLine4);

                                    rightY += 30;
                                    PdfTextArea pdfNoofBooks = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOT. NO. OF ANS. BOOKS IN PACK.");
                                    mypdfpage.Add(pdfNoofBooks);

                                    posY += 15;
                                    PdfTextArea pdfLine5 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                    mypdfpage.Add(pdfLine5);

                                    posY += 15;
                                    PdfTextArea pdfLine6 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                    mypdfpage.Add(pdfLine6);

                                    posY += 15;
                                    PdfTextArea pdfLine7 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                    mypdfpage.Add(pdfLine7);

                                    posY += 30;
                                    PdfLine pdfVertcalLine = new PdfLine(mydocument, new Point(neee, 10), new Point(neee, posY - 5), Color.Black, 1);
                                    mypdfpage.Add(pdfVertcalLine);
                                    neee = Convert.ToInt16(mydocument.PageWidth - 15);
                                    PdfLine pdfHeaderLine = new PdfLine(mydocument, new Point(15, posY), new Point(neee, posY), Color.Black, 1);
                                    mypdfpage.Add(pdfHeaderLine);

                                    posY += 8;
                                    PdfTextArea pdfSubjectName = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 15, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, subjectName);
                                    mypdfpage.Add(pdfSubjectName);

                                    PdfTextArea pdfFooterText;
                                    PdfTable table1 = mydocument.NewTable(Fontbold, 11, 10, 11);
                                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    table1.VisibleHeaders = false;
                                    table1.Columns[0].SetWidth(70);
                                    table1.Columns[1].SetWidth(40);
                                    table1.Columns[2].SetWidth(70);
                                    table1.Columns[3].SetWidth(40);
                                    table1.Columns[4].SetWidth(70);
                                    table1.Columns[5].SetWidth(40);
                                    table1.Columns[6].SetWidth(70);
                                    table1.Columns[7].SetWidth(40);
                                    table1.Columns[8].SetWidth(70);
                                    table1.Columns[9].SetWidth(40);

                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 0).SetContent("REG.No.");
                                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 1).SetContent("P/A");
                                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 2).SetContent("REG.No.");
                                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 3).SetContent("P/A");
                                    table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 4).SetContent("REG.No.");
                                    table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 5).SetContent("P/A");
                                    table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 6).SetContent("REG.No.");
                                    table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 7).SetContent("P/A");
                                    table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 8).SetContent("REG.No.");
                                    table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(0, 9).SetContent("P/A");
                                    table1.Rows[0].SetCellPadding(10);

                                    int rOw = 0;
                                    bool newPage = false;
                                    int tempRow = 0;
                                    for (int roow = rOw; roow < RegNo.Length; roow++)
                                    {
                                        if (rOw % 50 == 0 && rOw != 0 && (RegNo.Length > rOw))
                                        {
                                            posY += 20;
                                            PageNo++;
                                            tblPage = table1.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 100, 500));
                                            mypdfpage.Add(tblPage);

                                            posY += Convert.ToInt16(tblPage.Area.Height) + 15;
                                            pdfFooterText = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 50, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, "This Packet is intended to hold 50 Answer Books only\t\t|\t\tPresence or Absence of Candidates to be marked in the small box provided P/A");//This Packet is intended to hold 50 Answer Books only\t\t|\t\tPresence or Absence of Candidates to be marked in the small box provided P/A This Packet is indented to hold 50 Answer Books Only.\t\t|\t\tPresence or Absence of Candidates to be marked in small box provided P/A
                                            mypdfpage.Add(pdfFooterText);

                                            table2 = mydocument.NewTable(Fontbold, 3, 2, 5);
                                            table2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                            table2.VisibleHeaders = false;
                                            table2.Columns[0].SetWidth(150);
                                            table2.Columns[1].SetWidth(280);

                                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(0, 0).SetContent("Date\t:");
                                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(0, 1).SetContent("Name of Examiner(s)\t:");

                                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table2.Cell(2, 0).SetContent("Signature of the chief Superintendent");
                                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(2, 1).SetContent("Signature with Date\t:");
                                            posY += 20;
                                            tblPage = table2.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 80, 100));
                                            mypdfpage.Add(tblPage);

                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            tempRow = 0;
                                            posY = 10;
                                            tempRow = 0;
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg")))
                                            {
                                                LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, posY, 10, 500);
                                            }
                                            pdfSince = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydocument, 15, 60, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                                            mypdfpage.Add(pdfSince);

                                            pdfLine1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                                            mypdfpage.Add(pdfLine1);

                                            rightY = posY;
                                            neee = Convert.ToInt16((mydocument.PageWidth / 2) + 90);

                                            paftblPageNo = mydocument.NewTable(Fontbold, 2, 1, 5);
                                            paftblPageNo.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                            paftblPageNo.VisibleHeaders = false;
                                            paftblPageNo.Columns[0].SetWidth(50);

                                            paftblPageNo.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            paftblPageNo.Cell(0, 0).SetContent(PageNo.ToString());
                                            paftblPageNo.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            paftblPageNo.Cell(1, 0).SetContent(ToatlPage);

                                            tblPage = paftblPageNo.CreateTablePage(new PdfArea(mydocument, (mydocument.PageWidth - 100), rightY, 50, 80));
                                            mypdfpage.Add(tblPage);

                                            pdfSubCode = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, subjectCode);
                                            mypdfpage.Add(pdfSubCode);

                                            posY += 20;
                                            pdfLine2 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                                            mypdfpage.Add(pdfLine2);

                                            rightY += 30;
                                            pdfMajor = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "MAJOR : " + majorDept);
                                            mypdfpage.Add(pdfMajor);

                                            posY += 15;
                                            pdfLine3 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                                            mypdfpage.Add(pdfLine3);
                                            rightY += 30;
                                            pdfDateSession = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "DATE & DURATION\t\t:\t\t" + examDateNew + "-" + examSessionNew);
                                            mypdfpage.Add(pdfDateSession);

                                            posY += 15;
                                            pdfLine4 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                                            mypdfpage.Add(pdfLine4);
                                            rightY += 30;
                                            pdfNoofBooks = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, (mydocument.PageWidth / 2) + 100, rightY, (mydocument.PageWidth / 2) - 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOT. NO. OF ANS. BOOKS IN PACK.");
                                            mypdfpage.Add(pdfNoofBooks);

                                            posY += 15;
                                            pdfLine5 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                                            mypdfpage.Add(pdfLine5);

                                            posY += 15;
                                            pdfLine6 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                                            mypdfpage.Add(pdfLine6);

                                            posY += 15;
                                            pdfLine7 = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, posY, (mydocument.PageWidth / 2) + 120, 50), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                                            mypdfpage.Add(pdfLine7);

                                            posY += 30;
                                            pdfVertcalLine = new PdfLine(mydocument, new Point(neee, 10), new Point(neee, posY - 5), Color.Black, 1);
                                            mypdfpage.Add(pdfVertcalLine);
                                            neee = Convert.ToInt16(mydocument.PageWidth - 15);
                                            pdfHeaderLine = new PdfLine(mydocument, new Point(15, posY), new Point(neee, posY), Color.Black, 1);
                                            mypdfpage.Add(pdfHeaderLine);

                                            posY += 8;
                                            pdfSubjectName = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 15, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, subjectName);
                                            mypdfpage.Add(pdfSubjectName);

                                            table1 = mydocument.NewTable(Fontbold, 11, 10, 11);
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                            table1.VisibleHeaders = false;
                                            table1.Columns[0].SetWidth(70);
                                            table1.Columns[1].SetWidth(40);
                                            table1.Columns[2].SetWidth(70);
                                            table1.Columns[3].SetWidth(40);
                                            table1.Columns[4].SetWidth(70);
                                            table1.Columns[5].SetWidth(40);
                                            table1.Columns[6].SetWidth(70);
                                            table1.Columns[7].SetWidth(40);
                                            table1.Columns[8].SetWidth(70);
                                            table1.Columns[9].SetWidth(40);

                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("REG.No.");
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetContent("P/A");
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetContent("REG.No.");
                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetContent("P/A");
                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 4).SetContent("REG.No.");
                                            table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 5).SetContent("P/A");
                                            table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 6).SetContent("REG.No.");
                                            table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 7).SetContent("P/A");
                                            table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 8).SetContent("REG.No.");
                                            table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 9).SetContent("P/A");
                                            table1.Rows[0].SetCellPadding(10);

                                        }
                                        for (int cOl = 0; cOl < 10; cOl += 2)
                                        {
                                            if (RegNo.Length > rOw)
                                            {
                                                table1.Cell(tempRow + 1, cOl).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Rows[tempRow + 1].SetCellPadding(10);
                                                table1.Cell(tempRow + 1, cOl).SetContent(RegNo[rOw].ToString().Trim(new char[] { '\'' }).Replace("'", "").Trim());
                                                table1.Cell(tempRow + 1, cOl).SetFont(Fontnormal);
                                                rOw++;
                                            }
                                            else
                                            {
                                                if (tempRow + 1 < 11)
                                                    table1.Cell(tempRow + 1, cOl).SetContent("\n");
                                            }
                                        }
                                        tempRow++;
                                    }

                                    posY += 20;
                                    tblPage = table1.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 100, 500));
                                    mypdfpage.Add(tblPage);
                                    posY += Convert.ToInt16(tblPage.Area.Height) + 15;

                                    pdfFooterText = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydocument, 50, posY, (mydocument.PageWidth / 2) + 120, 20), System.Drawing.ContentAlignment.MiddleLeft, "This Packet is intended to hold 50 Answer Books only\t\t|\t\tPresence or Absence of Candidates to be marked in the small box provided P/A");
                                    mypdfpage.Add(pdfFooterText);

                                    table2 = mydocument.NewTable(Fontbold, 3, 2, 5);
                                    table2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                    table2.VisibleHeaders = false;
                                    table2.Columns[0].SetWidth(150);
                                    table2.Columns[1].SetWidth(280);

                                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(0, 0).SetContent("Date\t:");
                                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(0, 1).SetContent("Name of Examiner(s)\t:");

                                    table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    table2.Cell(2, 0).SetContent("Signature of the chief Superintendent");
                                    table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(2, 1).SetContent("Signature with Date\t:");
                                    posY += 20;
                                    tblPage = table2.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 80, 100));
                                    mypdfpage.Add(tblPage);
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
                    string szFile = "CoverSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
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

    protected void btnPrintCoverSheet_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkBasedOnSeating.Checked)
            {
            }
            else
            {
                printCoverSheet();
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
                if (FpStudentStrength.Visible == true)
                {
                    da.printexcelreport(FpStudentStrength, reportname);
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
            string pagename = "COECoverSheetGeneration.aspx";
            if (ddlReportFormat.SelectedIndex == 1)
            {
                rptheadname = "U.G. / P.G. DEGREE EXAMINATIONS - " + Convert.ToString(ddlExamMonth.SelectedItem.Text) + "\t\t" + Convert.ToString(ddlExamYear.SelectedItem.Text);
                rptheadname += "$ DATE & SESSION WISE REGISTERED CANDIDATES SUMMARY";
            }
            else
            {
                rptheadname = "U.G. / P.G. DEGREE EXAMINATIONS - " + Convert.ToString(ddlExamMonth.SelectedItem.Text) + "\t\t" + Convert.ToString(ddlExamYear.SelectedItem.Text);
                rptheadname += "$ DATE & SESSION WISE REGISTERED CANDIDATES STRENGTH LIST";
            }

            if (FpStudentStrength.Visible == true)
            {
                printMaster1.loadspreaddetails(FpStudentStrength, pagename, rptheadname);
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

    #endregion Button Events

}

#endregion Class Definition