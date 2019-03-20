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
using System.IO;
using Gios.Pdf;
using System.Globalization;
using System.Web;
using System.Configuration;

#endregion Namespace Declaration

#region Class Definition

public partial class CoeMod_COEQPaperPacking : System.Web.UI.Page
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
                qry = "select distinct s.subject_code,s.subject_name,ed.exam_date,ed.exam_session from subject s,exmtt et,exmtt_det ed where et.exam_code=ed.exam_code and ed.subject_no=s.subject_no " + qryCollege + qryDegreeCode + qryExamYear + qryExamMonth + qryExamDates + qryExamSessions;
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
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;

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

    protected void FpQPaperPacking_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpQPaperPacking.SaveChanges();
            int r = FpQPaperPacking.Sheets[0].ActiveRow;
            int j = FpQPaperPacking.Sheets[0].ActiveColumn;
            if (r == 0 && j == 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpQPaperPacking.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpQPaperPacking.Sheets[0].RowCount; row++)
                {
                    if (FpQPaperPacking.Sheets[0].Cells[row, 0].Text != string.Empty)
                    {
                        if (val == 1)
                            FpQPaperPacking.Sheets[0].Cells[row, j].Value = 1;
                        else
                            FpQPaperPacking.Sheets[0].Cells[row, j].Value = 0;
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
            if (divHall.Visible && isBasedOnSeatingArrangement)
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
                //qry = "select distinct Count(distinct ex.roll_no) as strength,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,CONVERT(VARCHAR(50),etd.exam_date,103) exam_date,etd.exam_session,etd.exam_date as Date from exmtt et,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex,Registration r where ex.roll_no=r.Roll_No and r.degree_code=et.degree_code and r.Batch_Year=et.batchFrom and ex.appl_no=ea.appl_no and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + qrySubjectCodes + qryCollege + qryExamSessions + qryExamDates + qryExamMonth + qryExamYear + qryDegreeCode + "  group by s.subject_code,s.subject_name,etd.exam_date,etd.exam_session order by exam_date asc,etd.exam_session desc,s.subject_code";
                qry = "select distinct Count(distinct ex.roll_no) as strength,s.subject_code,s.subject_name,s.subject_name+' ( '+s.subject_code+')' as SubjectDetails,CONVERT(VARCHAR(50),etd.exam_date,103) exam_date,etd.exam_session,etd.exam_date as Date from exmtt et,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex,Registration r where ex.roll_no=r.Roll_No and r.degree_code=et.degree_code and r.Batch_Year=et.batchFrom and ex.appl_no=ea.appl_no and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + qrySubjectCodes + qryCollege + qryExamSessions + qryExamDates + qryExamMonth + qryExamYear + qryDegreeCode + "  group by s.subject_code,s.subject_name,etd.exam_date,etd.exam_session order by etd.exam_date asc,etd.exam_session desc,s.subject_code";
                dsStudentStrength = da.select_method_wo_parameter(qry, "Text");

                qry = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.college_code  as coll_code,r.degree_code,r.Batch_Year,r.Current_Semester,s.subject_code,s.subject_name,isnull(s.part_type,'0') as part_type,s.subject_name+' ( '+s.subject_code+' )' as SubjectDetails,etd.exam_date,etd.exam_session,etd.exam_date as Date from exmtt et,exmtt_det etd,subject s,exam_appl_details ea,exam_application ex ,Registration r where ex.roll_no=r.Roll_No and r.degree_code=et.degree_code and r.Batch_Year=et.batchFrom and ex.appl_no=ea.appl_no and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and etd.subject_no=ea.subject_no and ea.subject_no=s.subject_no " + qrySubjectCodes + qryCollege + qryExamSessions + qryExamDates + qryExamMonth + qryExamYear + qryDegreeCode + " order by r.college_code,r.Batch_Year,r.Degree_code,r.Reg_No,etd.exam_date asc,etd.exam_session desc,s.subject_code";
                dsAllStudents = da.select_method_wo_parameter(qry, "Text");

                qry = " select distinct clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,r.degree_code,r.Reg_no,c.edu_level,c.Course_Name,dt.Dept_Name,dt.dept_acronym from Exam_Details et,Registration r,Course c,Department dt ,Degree dg,collinfo clg where et.batch_year=r.Batch_Year and et.degree_code=r.degree_code and r.degree_code=dg.Degree_Code and et.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dg.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and c.college_code=clg.college_code and et.Exam_year<>'0' and et.Exam_Month<>'0' " + qryCollege + qryDegreeCode + qryExamMonth + qryExamYear;
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

                    Init_Spread(FpQPaperPacking, 1);
                    ht.Clear();
                    FpQPaperPacking.Width = 950;
                    FpQPaperPacking.Visible = true;
                    FpQPaperPacking.Sheets[0].RowCount = 0;
                    FpQPaperPacking.Sheets[0].RowCount++;
                    FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].CellType = chkSelectAll;
                    FpQPaperPacking.Sheets[0].SpanModel.Add(FpQPaperPacking.Sheets[0].RowCount - 1, 2, 1, 3);
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
                        if (!ht.Contains(examDate + "-" + examSession))
                        {
                            FpQPaperPacking.Sheets[0].RowCount++;
                            FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Text = examDate + " - " + examSession;
                            FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#458547");
                            FpQPaperPacking.Sheets[0].Rows[FpQPaperPacking.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#458547");
                            FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Locked = true;
                            FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            ht.Add(examDate + "-" + examSession, examDate + "-" + examSession);
                        }
                        string regNo = string.Empty;
                        string majorDepartment = string.Empty;
                        string partType = string.Empty;
                        DataView dvAllStudent = new DataView();
                        if (dsAllStudents.Tables.Count > 0 && dsAllStudents.Tables[0].Rows.Count > 0)
                        {
                            dsAllStudents.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectCode + "' and Date='" + ddate + "' and exam_session='" + examSession + "'";
                            dvAllStudent = dsAllStudents.Tables[0].DefaultView;
                        }
                        if (dvAllStudent.Count > 0)
                        {
                            DataView dvMajorType = new DataView();
                            DataTable dtMajorType = dvAllStudent.ToTable(true, "Part_Type");
                            dtMajorType.DefaultView.RowFilter = "Part_Type='3'";
                            dvMajorType = dtMajorType.DefaultView;

                            List<int> listPartType = dtMajorType.AsEnumerable().Select(r => r.Field<int>("Part_Type")).ToList();
                            partType = string.Join(",", listPartType.Distinct().ToArray());
                            if (dvMajorType.Count > 0)
                            {
                                majorPart = "3";
                                isMajor = true;
                            }
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
                            //DataTable dtMajorDepartment = new DataTable();
                            //if (dsAllCourseDetails.Tables.Count > 0 && dsAllCourseDetails.Tables[0].Rows.Count > 0)
                            //{
                            //    dsAllCourseDetails.Tables[0].DefaultView.RowFilter = "college_code in('" + collCode + "') and Reg_no in(" + regNo + ")";
                            //    dtMajorDepartment = dsAllCourseDetails.Tables[0].DefaultView.ToTable(true, "Dept_Name", "dept_acronym");
                            //}

                            //List<string> lstDept = dtMajorDepartment.AsEnumerable()
                            //                           .Select(r => r.Field<string>("Dept_Name"))
                            //                           .ToList();
                            //majorDepartment = string.Join(",", lstDept.Distinct().ToArray());
                        }
                        sno++;
                        FpQPaperPacking.Sheets[0].RowCount++;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examDate).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(examSession).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].CellType = chkOneByOne;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].Tag = isMajor;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].Note = majorDepartment;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].Locked = false;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectDetails).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(subjectCode).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(subjectName).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(studentsCont).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(regNo).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(partType).Trim();
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpQPaperPacking.Sheets[0].Cells[FpQPaperPacking.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    }
                    FpQPaperPacking.SaveChanges();
                    FpQPaperPacking.Sheets[0].PageSize = FpQPaperPacking.Sheets[0].RowCount;
                    FpQPaperPacking.Height = 500;
                    FpQPaperPacking.SaveChanges();
                    FpQPaperPacking.Visible = true;
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

    #region QPaper Generator

    public void printQuestionPaperPacking()
    {
        try
        {
            FpQPaperPacking.SaveChanges();
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

            Font Fontbold1 = new Font("Book Antique", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            Font font4smallnew = new Font("Palatino Linotype", 7, FontStyle.Bold);

            bool selected = false;
            string subjectCodeAll = string.Empty;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            ArrayList arrDate = new ArrayList();
            ArrayList arrSession = new ArrayList();
            ArrayList arrCollege = new ArrayList();
            ArrayList arrSubjectCode = new ArrayList();
            ArrayList arrRegNo = new ArrayList();
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            //if (!string.IsNullOrEmpty(collegeCode.Trim()))
            //{
            //    qryCollege = " and r.college_code in (" + collegeCode + ")";
            //}
            //else
            //{
            //    FSNominee.Visible = false;
            //    btngen.Visible = false;
            //    lblnorec.Visible = true;
            //    lblnorec.Text = "Please Select Any College";
            //    return;
            //}
            string qryDate = string.Empty;
            string examdate = string.Empty;
            string[] dsplit;
            string qrySession = string.Empty;
            string strsubjectcode = string.Empty;
            string RegAll = string.Empty;
            string dateNew = string.Empty;
            string sessionNew = string.Empty;
            //ArrayList arrSubjectCodeList = new ArrayList();
            //ArrayList arrRegNoList = new ArrayList();
            DataTable dtSelectedSubjects = new DataTable();
            dtSelectedSubjects.Columns.Add("sno");
            dtSelectedSubjects.Columns.Add("subjectCode");
            dtSelectedSubjects.Columns.Add("subjectName");
            dtSelectedSubjects.Columns.Add("examDate");
            dtSelectedSubjects.Columns.Add("examSession");
            dtSelectedSubjects.Columns.Add("studentCount");
            dtSelectedSubjects.Columns.Add("majorPart");
            dtSelectedSubjects.Columns.Add("partType");
            dtSelectedSubjects.Columns.Add("regNos");
            dtSelectedSubjects.Columns.Add("selected");
            DataRow drSelectedSubjects;
            if (FpQPaperPacking.Sheets[0].RowCount > 0)
            {
                //collegeCode = string.Empty;
                for (int row = 0; row < FpQPaperPacking.Sheets[0].RowCount; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 1].Value).Trim(), out sel);
                    if (sel == 1)
                    {
                        selected = true;
                        string allRegNo = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 3].Tag).Trim();
                        string subName = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 2].Note).Trim();
                        string subCode = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 2].Tag).Trim();
                        string examDateNew = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 0].Tag).Trim();
                        string examSessionNew = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 0].Note).Trim();
                        string totalStudents = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 3].Text).Trim();
                        string majorPart = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 1].Tag).Trim();
                        string partType = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 3].Note).Trim();
                        drSelectedSubjects = dtSelectedSubjects.NewRow();
                        drSelectedSubjects["sno"] = Convert.ToString(FpQPaperPacking.Sheets[0].Cells[row, 0].Text).Trim();
                        drSelectedSubjects["subjectCode"] = subCode;
                        drSelectedSubjects["subjectName"] = subName;
                        drSelectedSubjects["examDate"] = examDateNew;
                        drSelectedSubjects["examSession"] = examSessionNew;
                        drSelectedSubjects["studentCount"] = totalStudents;
                        drSelectedSubjects["majorPart"] = majorPart;
                        drSelectedSubjects["partType"] = partType;
                        drSelectedSubjects["regNos"] = allRegNo;
                        drSelectedSubjects["selected"] = "1";
                        dtSelectedSubjects.Rows.Add(drSelectedSubjects);
                        string[] RegNo = allRegNo.Split(',');
                        if (!arrSubjectCode.Contains(subCode))
                        {
                            if (string.IsNullOrEmpty(subjectCodeAll))
                            {
                                subjectCodeAll = "'" + subCode + "'";
                            }
                            else
                            {
                                subjectCodeAll += ",'" + subCode + "'";
                            }
                            arrSubjectCode.Add(subCode);
                        }
                        for (int reg = 0; reg < RegNo.Length; reg++)
                        {
                            if (!arrRegNo.Contains(RegNo[reg]))
                            {
                                if (string.IsNullOrEmpty(RegAll))
                                {
                                    RegAll = "" + RegNo[reg] + "";
                                }
                                else
                                {
                                    RegAll += "," + RegNo[reg] + "";
                                }
                                arrRegNo.Add(RegNo[reg]);
                            }
                        }
                        if (!arrDate.Contains(examDateNew))
                        {
                            if (string.IsNullOrEmpty(dateNew))
                            {
                                dateNew = "'" + examDateNew + "'";
                            }
                            else
                            {
                                dateNew += ",'" + examDateNew + "'";
                            }
                            arrDate.Add(examDateNew);
                        }
                        if (!arrSession.Contains(examSessionNew))
                        {
                            if (string.IsNullOrEmpty(sessionNew))
                            {
                                sessionNew = "'" + examSessionNew + "'";
                            }
                            else
                            {
                                sessionNew += ",'" + examSessionNew + "'";
                            }
                            arrSession.Add(examSessionNew);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                qryCollege = " and r.college_code in (" + collegeCode + ") ";
            }
            string qrRegNo = string.Empty;
            if (!string.IsNullOrEmpty(RegAll.Trim()))
            {
                qrRegNo = " and r.Reg_No in (" + RegAll + ") ";
            }
            if (!string.IsNullOrEmpty(dateNew.Trim()))
            {
                qryDate = " and CONVERT(varchar(50),etd.exam_date,103) in(" + dateNew + ") ";
            }
            if (!string.IsNullOrEmpty(sessionNew.Trim()))
            {
                qrySession = "  and etd.exam_session in (" + sessionNew + ") ";
            }
            if (!string.IsNullOrEmpty(subjectCodeAll.Trim()))
            {
                strsubjectcode = " and s.subject_code in (" + subjectCodeAll + ") ";
            }
            DataSet dsColInfo = da.select_method_wo_parameter("select college_code,UPPER(collname)+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby from collinfo", "Text");
            Line6 = "COVER SHEET";
            Line7 = "SEMESTER EXAMINATION - " + Convert.ToString(ddlExamMonth.SelectedItem.Text.Trim()).ToUpper() + " " + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
            Line8 = "COURSE - ";
            string subjectCode = string.Empty;
            string major = string.Empty;
            string subjectName = string.Empty;
            string examDate = string.Empty;
            string examSession = string.Empty;
            string qry = "select distinct c.Edu_Level,c.Course_Name,isnull(s.subjectpriority,'0') as subjectpriority,s.subject_code,s.subject_name,convert(nvarchar(15),etd.exam_date,103) as edate,etd.exam_date,etd.exam_session,CONVERT(varchar, etd.start_time, 108) AS start_time,CONVERT(varchar, etd.end_time, 108) AS end_time,etd.start_time as ST,etd.end_time as ET from Exam_Details et,exmtt_det etd,subject s,Course c,Degree dg,Department dt,exam_application ea,exam_appl_details ed,Registration r where r.degree_code=et.degree_code and dg.Degree_Code=r.degree_code and et.batch_year=r.Batch_Year and r.Roll_No=ea.roll_no and ea.appl_no=ed.appl_no  and ed.subject_no=s.subject_no and etd.subject_no=ed.subject_no and etd.subject_no=s.subject_no and dg.Degree_Code=et.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code  " + strsubjectcode + qryCollege + qryDate + qrRegNo + qrySession + " and et.Exam_year='" + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim() + "' and et.Exam_month='" + Convert.ToString(ddlExamMonth.SelectedValue).Trim() + "' order by etd.exam_date,etd.exam_session desc,c.Edu_Level desc,subjectpriority";
            DataSet dsAll = da.select_method_wo_parameter(qry, "Text");
            int posY = 0;
            bool status = false;
            ArrayList arrSubjectsList = new ArrayList();
            if (selected)
            {
                bool notSave = false;
                mypdfpage = mydocument.NewPage();
                bool freshPage = true;
                int sno = 0;
                for (int row = 0; row < dtSelectedSubjects.Rows.Count; row++)
                {
                    int sel = 0;
                    int.TryParse(Convert.ToString(dtSelectedSubjects.Rows[row]["selected"]).Trim(), out sel);
                    string rowno = Convert.ToString(dtSelectedSubjects.Rows[row]["sno"]).Trim();
                    if (sel == 1)
                    {
                        status = true;
                        bool pageHas = false;
                        //posY = 10;
                        string allRegNo = Convert.ToString(dtSelectedSubjects.Rows[row]["regNos"]).Trim();
                        string subName = Convert.ToString(dtSelectedSubjects.Rows[row]["subjectName"]).Trim();
                        string subCode = Convert.ToString(dtSelectedSubjects.Rows[row]["subjectCode"]).Trim();
                        string examDateNew = Convert.ToString(dtSelectedSubjects.Rows[row]["examDate"]).Trim();
                        string examSessionNew = Convert.ToString(dtSelectedSubjects.Rows[row]["examSession"]).Trim();
                        string totalStudents = Convert.ToString(dtSelectedSubjects.Rows[row]["studentCount"]).Trim();
                        string majorPart = Convert.ToString(dtSelectedSubjects.Rows[row]["majorPart"]).Trim();
                        //string partType = Convert.ToString(dtSelectedSubjects.Rows[row]["regNos"]).Trim();
                        bool isMajor = false;
                        bool.TryParse(majorPart, out isMajor);
                        int totalStudentsCount = 0;
                        int.TryParse(totalStudents.Trim(), out totalStudentsCount);
                        string[] RegNo = allRegNo.Split(',');
                        string partType = Convert.ToString(dtSelectedSubjects.Rows[row]["partType"]).Trim();
                        DataView dvAll = new DataView();
                        string edulevel = string.Empty;
                        string course = string.Empty;
                        string examStartTime = string.Empty;
                        string examEndTime = string.Empty;
                        if (!arrSubjectsList.Contains(subCode.Trim().ToLower() + "@" + subName.Trim().ToLower()))
                        {
                            DataTable dtDistinctCourse = new DataTable();
                            if (dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                            {
                                dsAll.Tables[0].DefaultView.RowFilter = "subject_code='" + subCode + "' and subject_name='" + subName + "'";
                                dvAll = dsAll.Tables[0].DefaultView;
                                dtDistinctCourse = dvAll.ToTable(true, "Course_Name", "subject_code");
                            }
                            int loop = 0;
                            loop = totalStudentsCount / 160;
                            if (totalStudentsCount % 160 != 0)
                            {
                                loop += 1;
                            }
                            PdfTable pdfTbl;
                            PdfTablePage pdfTblPAge;
                            PdfLine pdfLine;
                            for (int iteration = 0; iteration < loop; iteration++)
                            {
                                sno++;
                                if (dvAll.Count > 0)
                                {
                                    edulevel = Convert.ToString(dvAll[0]["Edu_Level"]).Trim();
                                    course = Convert.ToString(dvAll[0]["Course_Name"]).Trim();
                                    DateTime st = new DateTime();
                                    DateTime.TryParseExact(Convert.ToString(dvAll[0]["start_time"]).Trim(), "HH:mm:ss", null, DateTimeStyles.None, out st);
                                    examStartTime = st.ToString("hh:mm tt");
                                    DateTime et = new DateTime();
                                    DateTime.TryParseExact(Convert.ToString(dvAll[0]["end_time"]).Trim(), "HH:mm:ss", null, DateTimeStyles.None, out et);
                                    examEndTime = et.ToString("hh:mm tt");

                                    string deg = " DEGREE \n" + Convert.ToString(ddlExamMonth.SelectedItem.Text).Trim().ToUpper() + "." + Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                                    string dateVAlue = examDateNew + "\n" + examStartTime + "\nTo\n" + examEndTime;
                                    int tblRows = 1;
                                    if (dvAll.Count == 1)
                                    {
                                        //if (freshPage)
                                        //{
                                        tblRows = 2;
                                        //}
                                        pdfTbl = mydocument.NewTable(Fontbold1, tblRows, 4, 10);
                                        pdfTbl.SetBorders(Color.Black, 1, BorderType.Rows);

                                        pdfTbl.VisibleHeaders = false;
                                        pdfTbl.Columns[0].SetWidth(100);
                                        pdfTbl.Columns[1].SetWidth(100);
                                        pdfTbl.Columns[2].SetWidth(250);
                                        pdfTbl.Columns[3].SetWidth(150);
                                        //if (freshPage)
                                        //{
                                        pdfTbl.Cell(0, 0).SetContent("Name of Examination and Year");
                                        pdfTbl.Cell(0, 1).SetContent("Subject Code");
                                        pdfTbl.Cell(0, 2).SetContent("Title of the Paper");
                                        pdfTbl.Cell(0, 3).SetContent("Date & Hour");
                                        pdfTbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 0).SetCellPadding(1);
                                        pdfTbl.Cell(0, 1).SetCellPadding(1);
                                        pdfTbl.Cell(0, 2).SetCellPadding(1);
                                        pdfTbl.Cell(0, 3).SetCellPadding(1);

                                        //}
                                        pdfTbl.Cell(tblRows - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        if (dtDistinctCourse.Rows.Count == 1)
                                        {
                                            if (partType.Contains("1") || partType.Contains("2"))
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                            }
                                            else if (partType.Contains("3"))
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(course + deg);
                                            }
                                            else
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                            }
                                        }
                                        else
                                        {
                                            pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                        }
                                        pdfTbl.Cell(tblRows - 1, 0).SetFont(head);

                                        pdfTbl.Cell(tblRows - 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 1).SetContent(subCode);

                                        pdfTbl.Cell(tblRows - 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 2).SetContent(subName);

                                        pdfTbl.Cell(tblRows - 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 3).SetContent(dateVAlue);

                                        //posY += 10;
                                        //pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 100));
                                        //mypdfpage.Add(pdfTblPAge);
                                        //posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                        //mypdfpage.SaveToDocument();
                                        if (posY > mydocument.PageHeight - 140)
                                        {
                                            notSave = true;
                                            if (freshPage)
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            else
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }

                                            //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                            //mypdfpage.Add(pdfLine);

                                            pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                            mypdfpage.Add(pdfLine);
                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            freshPage = true;
                                            posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                            notSave = false;
                                            posY = 10;
                                        }
                                        else
                                        {
                                            posY += 10;
                                            if (freshPage)
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            else
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                            //mypdfpage.Add(pdfLine);
                                            pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                            mypdfpage.Add(pdfLine);
                                            posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                            notSave = false;
                                            freshPage = false;
                                        }
                                    }
                                    else if (dvAll.Count > 1)
                                    {
                                        //if (freshPage)
                                        //{
                                        tblRows = 2;
                                        //}
                                        pdfTbl = mydocument.NewTable(Fontbold1, tblRows, 4, 10);
                                        pdfTbl.SetBorders(Color.Black, 1, BorderType.Rows);

                                        pdfTbl.VisibleHeaders = false;
                                        pdfTbl.Columns[0].SetWidth(100);
                                        pdfTbl.Columns[1].SetWidth(100);
                                        pdfTbl.Columns[2].SetWidth(250);
                                        pdfTbl.Columns[3].SetWidth(150);

                                        //if (freshPage)
                                        //{
                                        pdfTbl.Cell(0, 0).SetContent("Name of Examination and Year");
                                        pdfTbl.Cell(0, 1).SetContent("Subject Code");
                                        pdfTbl.Cell(0, 2).SetContent("Title of the Paper");
                                        pdfTbl.Cell(0, 3).SetContent("Date & Hour");
                                        pdfTbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(0, 0).SetCellPadding(2);
                                        pdfTbl.Cell(0, 1).SetCellPadding(2);
                                        pdfTbl.Cell(0, 2).SetCellPadding(2);
                                        pdfTbl.Cell(0, 3).SetCellPadding(2);
                                        //}
                                        pdfTbl.Cell(tblRows - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //pdfTbl.Cell(tblRows-1, 0).SetContent(edulevel + deg);
                                        if (dtDistinctCourse.Rows.Count == 1)
                                        {
                                            if (partType.Contains("1") || partType.Contains("2"))
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                            }
                                            else if (partType.Contains("3"))
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(course + deg);
                                            }
                                            else
                                            {
                                                pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                            }
                                        }
                                        else
                                        {
                                            pdfTbl.Cell(tblRows - 1, 0).SetContent(edulevel + deg);
                                        }
                                        pdfTbl.Cell(tblRows - 1, 0).SetFont(head);

                                        pdfTbl.Cell(tblRows - 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 1).SetContent(subCode);

                                        pdfTbl.Cell(tblRows - 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 2).SetContent(subName);

                                        pdfTbl.Cell(tblRows - 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        pdfTbl.Cell(tblRows - 1, 3).SetContent(dateVAlue);

                                        if (posY > mydocument.PageHeight - 140)
                                        {
                                            notSave = true;
                                            if (freshPage)
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            else
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                            mypdfpage.Add(pdfLine);

                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            freshPage = true;
                                            posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                            notSave = false;
                                            posY = 10;

                                            //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                            //mypdfpage.Add(pdfLine);
                                        }
                                        else
                                        {
                                            posY += 10;
                                            if (freshPage)
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            else
                                            {
                                                pdfTblPAge = pdfTbl.CreateTablePage(new PdfArea(mydocument, 20, posY, mydocument.PageWidth - 50, 220));
                                                mypdfpage.Add(pdfTblPAge);
                                            }
                                            freshPage = false;
                                            //pdfLine = pdfTblPAge.Area.LowerBound(Color.Black, 1);
                                            //mypdfpage.Add(pdfLine);
                                            pdfLine = pdfTblPAge.Area.UpperBound(Color.Black, 1);
                                            mypdfpage.Add(pdfLine);

                                            posY += Convert.ToInt16(pdfTblPAge.Area.Height) + 15;
                                            notSave = false;
                                        }
                                    }
                                    arrSubjectsList.Add(subCode.Trim().ToLower() + "@" + subName.Trim().ToLower());
                                }
                            }
                        }
                    }
                }
                if (!notSave)
                    mypdfpage.SaveToDocument();
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
                    string szFile = "QPaperPacking_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
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

    protected void btnPrintQPaperBacking_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkBasedOnSeating.Checked)
            {

            }
            else
            {
                //printCoverSheet();
                printQuestionPaperPacking();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion QPaper Generator

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
                if (FpQPaperPacking.Visible == true)
                {
                    da.printexcelreport(FpQPaperPacking, reportname);
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
            rptheadname = "Question Paper Packing Report";
            string pagename = "COEQPaperPacking.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpQPaperPacking.Visible == true)
            {
                printMaster1.loadspreaddetails(FpQPaperPacking, pagename, rptheadname);
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