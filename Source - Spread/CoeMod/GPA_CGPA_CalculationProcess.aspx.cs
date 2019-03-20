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
using System.Configuration;

public partial class CoeMod_GPA_CGPA_CalculationProcess : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
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
    string partNos = string.Empty;
    string partNames = string.Empty;
    string subjectNames = string.Empty;
    string subjectNos = string.Empty;
    string subjectCodes = string.Empty;
    string ExamMonth = string.Empty;
    string ExamYear = string.Empty;
    string topValues = string.Empty;
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
    string qryPartTypeNos = string.Empty;
    string qrySubjectNos = string.Empty;
    string qrySubjectNames = string.Empty;
    string qrySubjectCodes = string.Empty;
    string qryExamMonth = string.Empty;
    string qryExamYear = string.Empty;
    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    bool isSchool = false;
    int selected = 0;
    int top = 1;

    #endregion

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
                Bindcollege();
                BindStream();
                BindEduLevel();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindExamYear();
                BindExamMonth();
                //BindSubject();
            }
        }
        catch (ThreadAbortException tt)
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

    public void BindBatch()
    {
        try
        {
            cblBatch.Items.Clear();
            ddlBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "--Select--";
            ds.Clear();
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            eduLevels = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
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
            //--and LTRIM(RTRIM(ISNULL(c.type,''))) in('aided') and r.college_code in(14) and c.Edu_Level in('pg')
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.college_code in(" + collegeCodes + ") " + qryStream + qryEduLevel + " order by r.Batch_Year desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();
                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
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

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "--Select--";
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
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
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCollege + columnfield + qryEduLevel + qryStream + qryBatch + "  order by c.Priority", "text");//and r.CC='1' and ISNULL(r.isRedo,'0')='0'
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();
                    checkBoxListselectOrDeselect(cblDegree, true);
                    CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
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
            if (cblDegree.Items.Count > 0)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCourseId))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCourseId + qryCollege + columnfield + qryStream + qryEduLevel + qryBatch + "order by dg.Degree_Code", "text");//and r.CC='1' and ISNULL(r.isRedo,'0')='0' 
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
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

    public void BindSem()
    {
        try
        {
            ds.Clear();
            cblSem.Items.Clear();
            ddlSem.Items.Clear();
            chkSem.Checked = false;
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollege = string.Empty;
            collegeCodes = string.Empty;
            qryBatch = string.Empty;
            batchYears = string.Empty;
            courseIds = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and college_code in(" + collegeCodes + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and Batch_year in(" + batchYears + ")";
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and Batch_year in(" + batchYears + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollege + qryBatch + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        cblSem.Items.Add(i.ToString());
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        cblSem.Items.Add(i.ToString());
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSem.SelectedIndex = 0;
                checkBoxListselectOrDeselect(cblSem, true);
                CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree where duration<>'0' " + qryDegreeCode + qryCollege + " group by first_year_nonsemester";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            cblSem.Items.Add(i.ToString());
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            cblSem.Items.Add(i.ToString());
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSem.SelectedIndex = 0;
                    checkBoxListselectOrDeselect(cblSem, true);
                    CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
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
    /// Added By Malang Raja
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
            if (cblDegree.Items.Count > 0)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
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
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblSem.Items.Count > 0 && txtSem.Visible == true)
            {
                semesters = getCblSelectedValue(cblSem);
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";
                }
            }
            else if (ddlSem.Items.Count > 0 && ddlSem.Visible == true)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Text + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch))
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
            if (cblDegree.Items.Count > 0)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
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
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblSem.Items.Count > 0 && txtSem.Visible == true)
            {
                semesters = getCblSelectedValue(cblSem);
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";
                }
            }
            else if (ddlSem.Items.Count > 0 && ddlSem.Visible == true)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Text + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";
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
            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + collegeCode + qryBatch + qryDegreeCode + ExamYear + " order by Exam_Month";
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
            else
            {
            }
            //ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
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
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].Columns[1].Width = 38;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].Columns[2].Width = 330;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "College Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].Columns[3].Width = 70;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].Columns[4].Width = 250;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(5, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].Columns[6].Width = 110;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Exam Month & Year";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(6, Farpoint.Model.MergePolicy.Always);
            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 5;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 38;
                FpSpread1.Sheets[0].Columns[2].Width = 60;
                FpSpread1.Sheets[0].Columns[3].Width = 195;
                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Exam Month & Year";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
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
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContents.Visible = false;
            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            BindExamYear();
            BindExamMonth();
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
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindSem();
            BindExamYear();
            BindExamMonth();
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
            BindSem();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindExamYear();
            BindExamMonth();
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpCalculateCGPA_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpCalculateCGPA.SaveChanges();
            int r = FpCalculateCGPA.Sheets[0].ActiveRow;
            int j = FpCalculateCGPA.Sheets[0].ActiveColumn;
            if (r == 0 && j == 0)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpCalculateCGPA.Sheets[0].RowCount; row++)
                {
                    if (val == 1)
                        FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 1;
                    else
                        FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 0;
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
            string qryRedoBatch = string.Empty;
            string qryRedoDegreeCode = string.Empty;
            bool isRedoStud = true;
            partNos = string.Empty;
            partNames = string.Empty;
            topValues = string.Empty;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
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
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblStream.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}
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
            if (cblBatch.Items.Count == 0 && ddlBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_Year in(" + batchYears + ")";
                    qryRedoBatch = " and sr.BatchYear in(" + batchYears + ")";

                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (cblDegree.Items.Count > 0)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblDegree.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
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
            if (cblSem.Items.Count == 0 && ddlSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else if (cblSem.Items.Count > 0 && txtSem.Visible == true)
            {
                semesters = getCblSelectedValue(cblSem);
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";

                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlSem.Items.Count > 0 && ddlSem.Visible == true)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Text + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and ed.current_semester in(" + semesters + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
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

            #region single student
            string qryregNo = string.Empty;
            if (!string.IsNullOrEmpty(txt_searchbyreg.Text))
            {
                string stuinfo = "select reg_no,roll_no,app_no,degree_code,Batch_Year,college_code,Current_Semester from Registration where Reg_No='" + txt_searchbyreg.Text + "'";
                DataSet dsstu = da.select_method_wo_parameter(stuinfo, "text");
                if (dsstu.Tables.Count > 0 && dsstu.Tables[0].Rows.Count > 0)
                {
                    qrySemester = " and ed.current_semester in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["Current_Semester"]) + ")";
                    qryDegreeCode = " and r.degree_code in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["degree_code"]) + ")";
                    qryRedoDegreeCode = " and sr.DegreeCode in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["degree_code"]) + ")";
                    qryBatch = " and r.Batch_Year in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["Batch_Year"]) + ")";
                    qryRedoBatch = " and sr.BatchYear in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["Batch_Year"]) + ")";
                    qryCollege = " and r.college_code in(" + Convert.ToString(dsstu.Tables[0].Rows[0]["college_code"]) + ")";
                    qryregNo = " and r.reg_no in('" + Convert.ToString(dsstu.Tables[0].Rows[0]["reg_no"]) + "')";
                }
            }
            string semWise = "select distinct sx.app_no,r.reg_no,r.roll_no,r.degree_code,r.Batch_Year,r.college_code,r.Current_Semester from CalculateSemWiseGPA_CGPA sx,Registration r where r.App_No=sx.app_no " + qryCollege + qryBatch + qryDegreeCode;
            string partwise = "select distinct ex.app_no,r.reg_no,r.roll_no,r.degree_code,r.Batch_Year,r.college_code,r.Current_Semester from CalculateExamWiseGPA_CGPA ex,Registration r where r.App_No=ex.app_no" + qryCollege + qryBatch + qryDegreeCode;
            string qryStud1 = "select r.Roll_No as Roll_No,r.Reg_No as Reg_No,r.Roll_Admit as Roll_Admit,r.App_No as App_No,r.college_code as college_code,r.Batch_Year as Batch_Year,r.degree_code as degree_code,r.Current_Semester as Current_Semester,ed.Exam_Month as Exam_Month,ed.Exam_year as Exam_year,ed.exam_code as exam_code from Registration r,applyn a,Exam_Details ed where a.app_no=r.App_No and ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and a.batch_year=ed.batch_year and a.degree_code=ed.degree_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' " + qryCollege + qryBatch + qryDegreeCode + qryExamMonth + qryExamYear;
            DataSet dsExamWise = da.select_method_wo_parameter(partwise,"text");
            DataSet dsSemWise = da.select_method_wo_parameter(semWise, "text");
            DataSet dsOverall = da.select_method_wo_parameter(qryStud1, "text");
            #endregion



            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth))
            {

                string qryStud = "select r.Roll_No as Roll_No,r.Reg_No as Reg_No,r.Roll_Admit as Roll_Admit,r.App_No as App_No,r.college_code as college_code,r.Batch_Year as Batch_Year,r.degree_code as degree_code,r.Current_Semester as Current_Semester,ed.Exam_Month as Exam_Month,ed.Exam_year as Exam_year,ed.exam_code as exam_code from Registration r,applyn a,Exam_Details ed where a.app_no=r.App_No and ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and a.batch_year=ed.batch_year and a.degree_code=ed.degree_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' " + qryCollege + qryBatch + qryDegreeCode + qryExamMonth + qryExamYear + qryregNo;
                qry = " select distinct clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,r.Batch_Year as Batch_Year,c.Course_Id,dt.Dept_Code,r.degree_code as degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,ed.exam_code,ed.Exam_year,ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1)))+'-'+Convert(varchar(100),ed.Exam_year) as ExamMonthYear,ed.current_semester,dg.Duration,'0' as Redo_Status from Exam_Details ed,Registration r,Course c,Department dt ,Degree dg,collinfo clg where ed.batch_year=r.Batch_Year and ed.degree_code=r.degree_code and r.degree_code=dg.Degree_Code and ed.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dg.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and c.college_code=clg.college_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' " + qryCollege + qryBatch + qryDegreeCode + qryExamMonth + qryExamYear + qryregNo;

                if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryRedoBatch) && !string.IsNullOrEmpty(qryRedoDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth))
                {
                    string studRedoQ = " select distinct clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,sr.BatchYear as Batch_Year,c.Course_Id,dt.Dept_Code,sr.DegreeCode as Degree_Code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,ed.exam_code,ed.Exam_year,ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1)))+'-'+Convert(varchar(100),ed.Exam_year) as ExamMonthYear,ed.current_semester,dg.Duration,'1' as Redo_Status from Exam_Details ed,Registration r,Course c,Department dt ,Degree dg,collinfo clg,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and ed.batch_year=sr.BatchYear and ed.degree_code=sr.DegreeCode and sr.DegreeCode=dg.Degree_Code and ed.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dg.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and c.college_code=clg.college_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' and sr.RedoType='1' " + qryCollege + qryRedoBatch + qryRedoDegreeCode + qryExamMonth + qryExamYear + qryregNo;

                    string qryStudRedo = " select r.Roll_No as Roll_No,r.Reg_No as Reg_No,r.Roll_Admit as Roll_Admit,r.App_No as App_No,r.college_code as college_code,sr.BatchYear as Batch_Year,sr.DegreeCode as degree_code,r.Current_Semester as Current_Semester,ed.Exam_Month as Exam_Month,ed.Exam_year  as Exam_year,ed.exam_code as exam_code from Registration r,StudentRedoDetails sr,applyn a,Exam_Details ed where a.app_no=r.App_No and sr.Stud_AppNo=r.App_No and a.app_no=sr.Stud_AppNo and ed.batch_year=sr.BatchYear and sr.DegreeCode=ed.degree_code and a.batch_year=ed.batch_year and a.degree_code=ed.degree_code and ed.Exam_year<>'0' and ed.Exam_Month<>'0' and sr.RedoType='1' " + qryCollege + qryRedoBatch + qryRedoDegreeCode + qryExamMonth + qryExamYear + qryregNo;
                    if (isRedoStud)
                    {
                        qry += " union " + studRedoQ;
                        qryStud += "  union " + qryStudRedo;
                    }
                }
                qry += " order by clg.college_code,type,c.Edu_Level,Batch_Year desc,degree_code,ed.Exam_year,ed.Exam_Month,ed.current_semester";
                qryStud += " order by college_code,Batch_Year desc,degree_code,Exam_year,Exam_Month,current_semester,App_No";
                DataSet dsAllCourseDetails = new DataSet();
                DataSet dsStudents = new DataSet();
                dsAllCourseDetails = da.select_method_wo_parameter(qry, "text");
                dsStudents = da.select_method_wo_parameter(qryStud, "text");
                if (dsAllCourseDetails.Tables.Count > 0 && dsAllCourseDetails.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpCalculateCGPA, 0);
                    FpCalculateCGPA.Sheets[0].RowCount = 0;
                    Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
                    chkCellAll.AutoPostBack = true;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    Farpoint.CheckBoxCellType chkCell = new Farpoint.CheckBoxCellType();
                    chkCell.AutoPostBack = false;
                    FpCalculateCGPA.Sheets[0].RowCount++;
                    FpCalculateCGPA.Sheets[0].Columns[0].CellType = chkCell;
                    FpCalculateCGPA.Sheets[0].Cells[0, 0].CellType = chkCellAll;
                    FpCalculateCGPA.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpCalculateCGPA.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                    FpCalculateCGPA.Sheets[0].AddSpanCell(0, 1, 1, FpCalculateCGPA.Sheets[0].ColumnCount - 1);
                    FpCalculateCGPA.Sheets[0].FrozenRowCount = 1;
                    FpCalculateCGPA.Sheets[0].FrozenRowCount = 1;
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
                    bool isval=false;
                    foreach (DataRow drCourseDetails in dsAllCourseDetails.Tables[0].Rows)
                    {
                        isval=false;
                        serialNo++;
                        collegeCode = Convert.ToString(drCourseDetails["college_code"]).Trim();
                        collegeName = Convert.ToString(drCourseDetails["collname"]).Trim();
                        batchYear = Convert.ToString(drCourseDetails["Batch_Year"]).Trim();
                        courseId = Convert.ToString(drCourseDetails["Course_Id"]).Trim();
                        deptCode = Convert.ToString(drCourseDetails["Dept_Code"]).Trim();
                        degreeCode = Convert.ToString(drCourseDetails["Degree_Code"]).Trim();
                        courseType = Convert.ToString(drCourseDetails["type"]).Trim();
                        eduLevel = Convert.ToString(drCourseDetails["Edu_Level"]).Trim();
                        courseName = Convert.ToString(drCourseDetails["Course_Name"]).Trim();
                        departmentName = Convert.ToString(drCourseDetails["Dept_Name"]).Trim();
                        departmentAcr = Convert.ToString(drCourseDetails["dept_acronym"]).Trim();
                        degreeName = Convert.ToString(drCourseDetails["DegreeDetails"]).Trim();
                        examCode = Convert.ToString(drCourseDetails["exam_code"]).Trim();
                        examYear = Convert.ToString(drCourseDetails["Exam_year"]).Trim();
                        examMonth = Convert.ToString(drCourseDetails["Exam_Month"]).Trim();
                        monthName = Convert.ToString(drCourseDetails["Month_Name"]).Trim();
                        examMonthYear = Convert.ToString(drCourseDetails["ExamMonthYear"]).Trim();
                        currentSemester = Convert.ToString(drCourseDetails["current_semester"]).Trim();
                        redoStatus = Convert.ToString(drCourseDetails["Redo_Status"]).Trim();
                        maxDuration = Convert.ToString(drCourseDetails["Duration"]).Trim();
                        int maximumDuration = 0;
                        int.TryParse(maxDuration, out maximumDuration);
                        string currentSemesterNew = string.Empty;// currentSemester;
                        GetSem(batchYear, examYear, examMonth, maxDuration, ref currentSemesterNew);
                        DataView dvStudents = new DataView();
                        //DataTable dtStudents = new DataTable();
            //             DataSet dsExamWise = da.select_method_wo_parameter(partwise,"text");
            //DataSet dsSemWise = da.select_method_wo_parameter(semWise, "text");
            //DataSet dsOverall = da.select_method_wo_parameter(qryStud, "text");
                        if (dsOverall.Tables.Count > 0 && dsOverall.Tables[0].Rows.Count > 0 && dsSemWise.Tables.Count > 0 && dsSemWise.Tables[0].Rows.Count > 0 && dsExamWise.Tables.Count > 0 && dsExamWise.Tables[0].Rows.Count > 0)
                        {
                            //modified by Mullai(')
                            dsOverall.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and Degree_code='" + degreeCode + "' and college_code='" + collegeCode + "'"; 
                            DataTable dtOver = dsOverall.Tables[0].DefaultView.ToTable();
                            dsSemWise.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and Degree_code='" + degreeCode + "' and college_code='" + collegeCode + "'";
                            DataTable dtSem = dsSemWise.Tables[0].DefaultView.ToTable();
                            dsExamWise.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and Degree_code='" + degreeCode + "' and college_code='" + collegeCode + "'";
                            DataTable dtExam = dsExamWise.Tables[0].DefaultView.ToTable();
                            if(dtOver.Rows.Count==dtSem.Rows.Count && dtOver.Rows.Count==dtExam.Rows.Count)
                                isval = true;
                        }


                        if (dsStudents.Tables.Count > 0 && dsStudents.Tables[0].Rows.Count > 0)
                        {
                            dsStudents.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and Degree_code='" + degreeCode + "' and college_code='" + collegeCode + "' ";//and exam_month='" + examMonth + "' and Exam_year='" + examYear + "' and exam_code='" + examCode + "'
                            dvStudents = dsStudents.Tables[0].DefaultView;
                            //dtStudents = dvStudents.ToTable();
                        }
                        List<decimal> lstAppNo = dvStudents.ToTable().AsEnumerable()
                                                       .Select(r => r.Field<decimal>("app_no"))
                                                       .ToList();
                        string appNos = string.Join(",", lstAppNo.ToArray());
                        List<string> lstRollNo = dvStudents.ToTable().AsEnumerable()
                                                       .Select(r => r.Field<string>("roll_no"))
                                                       .ToList();
                        string rollNos = string.Join(",", lstRollNo.ToArray());
                        FpCalculateCGPA.Sheets[0].RowCount++;
                       

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].CellType = chkCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(appNos).Trim(); //Convert.ToString(courseType).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(rollNos).Trim(); //Convert.ToString(eduLevel).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].Locked = false;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(serialNo).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(eduLevel).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(courseName).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(collegeName).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(collegeCode).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(deptCode).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(batchYear).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(redoStatus).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(currentSemesterNew).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(degreeName).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(degreeCode).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(examCode).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(currentSemester).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(monthName).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(maximumDuration).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(examMonthYear).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(examMonth).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(examYear).Trim();
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpCalculateCGPA.Sheets[0].Cells[FpCalculateCGPA.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        if (isval)
                            FpCalculateCGPA.Sheets[0].Rows[FpCalculateCGPA.Sheets[0].RowCount - 1].BackColor = Color.LightBlue;

                    }
                    divMainContents.Visible = true;
                    FpCalculateCGPA.Sheets[0].PageSize = FpCalculateCGPA.Sheets[0].RowCount;
                    FpCalculateCGPA.Width = 980;
                    FpCalculateCGPA.Height = 500;
                    FpCalculateCGPA.SaveChanges();
                    FpCalculateCGPA.Visible = true;
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

    #region Calcuate GPA CGPA

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        try
        {
            FpCalculateCGPA.SaveChanges();
            bool result = false;
            bool isSelected = false;
            bool isCalculateSave = false;
            string userCode = string.Empty;
            if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
            }
            else
            {
                userCode = "0";
            }
            if (FpCalculateCGPA.Sheets[0].RowCount == 0 || FpCalculateCGPA.Sheets[0].RowCount <= 1)
            {
                divMainContents.Visible = false;
                lblAlertMsg.Text = "No Record(s) Were Found";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                for (int row = 1; row < FpCalculateCGPA.Sheets[0].RowCount; row++)
                {
                    int select = 0;
                    int.TryParse(Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 0].Value).Trim(), out select);
                    if (select == 1)
                    {
                        isSelected = true;
                    }
                }
            }
            if (!isSelected)
            {
                lblAlertMsg.Text = "Please Select Atleast One Record and Then Proceed!!!";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
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
                string qryRedoBatch = string.Empty;
                string qryRedoDegreeCode = string.Empty;
                bool isRedoStud = true;
                partNos = string.Empty;
                partNames = string.Empty;
                topValues = string.Empty;
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                DataSet dsStudentsMarksList = new DataSet();
                DataSet dsGradeDetails = new DataSet();
                DataSet dsClassify = new DataSet();
                Dictionary<string, int> dicStudentsFailedCount = new Dictionary<string, int>();
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
                if (cblBatch.Items.Count == 0 && ddlBatch.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
                {
                    batchYears = getCblSelectedText(cblBatch);
                    if (!string.IsNullOrEmpty(batchYears))
                    {
                        qryBatch = " and r.Batch_Year in(" + batchYears + ")";
                        qryRedoBatch = " and sr.BatchYear in(" + batchYears + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
                {
                    batchYears = string.Empty;
                    foreach (ListItem li in ddlBatch.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(batchYears))
                            {
                                batchYears = "'" + li.Text + "'";
                            }
                            else
                            {
                                batchYears += ",'" + li.Text + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(batchYears))
                    {
                        qryBatch = " and r.Batch_year in(" + batchYears + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                if (cblDegree.Items.Count > 0)
                {
                    courseIds = getCblSelectedValue(cblDegree);
                    if (!string.IsNullOrEmpty(courseIds))
                    {
                        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblDegree.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
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
                if (cblSem.Items.Count == 0 && ddlSem.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else if (cblSem.Items.Count > 0 && txtSem.Visible == true)
                {
                    semesters = getCblSelectedValue(cblSem);
                    if (!string.IsNullOrEmpty(semesters))
                    {
                        qrySemester = " and ed.current_semester in(" + semesters + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (ddlSem.Items.Count > 0 && ddlSem.Visible == true)
                {
                    semesters = string.Empty;
                    foreach (ListItem li in ddlSem.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(semesters))
                            {
                                semesters = "'" + li.Text + "'";
                            }
                            else
                            {
                                semesters += ",'" + li.Text + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(semesters))
                    {
                        qrySemester = " and ed.current_semester in(" + semesters + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
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
                if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth))
                {
                    qry = "select r.App_No,r.Roll_No,r.college_code,r.Batch_Year as Batch_Year,r.degree_code as degree_code,ed.exam_code,ed.Exam_Month,ed.Exam_year,r.Current_Semester as CurrentSem,ed.current_semester as AppliedSem,ed.current_semester as RedoSem,sm.semester as SubjectSem,ss.subType_no,ss.subject_type,ISNULL(ss.Lab,'0') as Lab,ISNULL(ss.ElectivePap,'0') as Elective,ISNULL(ss.projThe,'0') as Thesis,s.subject_no,s.subject_code,s.subject_name,ISNULL(s.Elective,'0') as SubElective,ISNULL(s.sub_lab,'0') as SubLab,ISNULL(s.Part_Type,'0') as Part_Type,ISNULL(s.subjectpriority,'0') as subjectpriority,s.credit_points,s.min_int_marks as MinICA,s.max_int_marks as MaxICA,s.min_ext_marks as MinESA,s.max_ext_marks as MaxESA,s.mintotal as MinTotal,s.maxtotal as MaxTotal,ISNULL(m.internal_mark,'0') as ICAMark,ISNULL(m.external_mark,'0') as ESAMark,ISNULL(m.total,'0') as Total,m.grade as Grade,m.cp, ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*100) else '0' end AS DECIMAL(10,0)) ,0,0) as TotalOutOf100,ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*10) else '0' end AS DECIMAL(10,1)),1,0) as TotalOutOf10,ISNULL(m.attempts,'0') as attempts,m.entry_code,m.passorfail,m.result,'0' as REDO from mark_entry m,subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed,Registration r where sc.subject_no=m.subject_no and m.roll_no=sc.roll_no and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and sm.Batch_Year=ed.batch_year and sm.degree_code=ed.degree_code and r.Roll_No=sc.roll_no and r.Roll_No=m.roll_no and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and sm.degree_code=r.degree_code and r.Batch_Year=sm.Batch_Year and m.exam_code=ed.exam_code  " + qryCollege + qryBatch + qryDegreeCode + qryExamMonth + qryExamYear;
                    if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryRedoBatch) && !string.IsNullOrEmpty(qryRedoDegreeCode) && !string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryExamMonth))
                    {
                        string studRedoQ = " select r.App_No,r.Roll_No,r.college_code,sr.BatchYear as Batch_Year,sr.DegreeCode as degree_code,ed.exam_code,ed.Exam_Month,ed.Exam_year,r.Current_Semester as CurrentSem,ed.current_semester as AppliedSem,sr.Semester as RedoSem,sm.semester as SubjectSem,ss.subType_no,ss.subject_type,ISNULL(ss.Lab,'0') as Lab,ISNULL(ss.ElectivePap,'0') as Elective,ISNULL(ss.projThe,'0') as Thesis,s.subject_no,s.subject_code,s.subject_name,ISNULL(s.Elective,'0') as SubElective,ISNULL(s.sub_lab,'0') as SubLab,ISNULL(s.Part_Type,'0') as Part_Type,ISNULL(s.subjectpriority,'0') as subjectpriority,s.credit_points,s.min_int_marks as MinICA,s.max_int_marks as MaxICA,s.min_ext_marks as MinESA,s.max_ext_marks as MaxESA,s.mintotal as MinTotal,s.maxtotal as MaxTotal,ISNULL(m.internal_mark,'0') as ICAMark,ISNULL(m.external_mark,'0') as ESAMark,ISNULL(m.total,'0') as Total,m.grade as Grade,m.cp, ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*100) else '0' end AS DECIMAL(10,0)) ,0,0) as TotalOutOf100,ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*10) else '0' end AS DECIMAL(10,1)),1,0) as TotalOutOf10,ISNULL(m.attempts,'0') as attempts,m.entry_code,m.passorfail,m.result,'1' as REDO from mark_entry m,subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed,Registration r,StudentRedoDetails sr where  sc.subject_no=m.subject_no and m.roll_no=sc.roll_no and sr.Stud_AppNo=r.App_No and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and sm.Batch_Year=ed.batch_year and sm.degree_code=ed.degree_code and r.Roll_No=sc.roll_no and r.Roll_No=m.roll_no and sr.BatchYear=ed.batch_year and ed.degree_code=sr.DegreeCode and sm.degree_code=sr.DegreeCode and sr.BatchYear=sm.Batch_Year and m.exam_code=ed.exam_code " + qryCollege + qryBatch + qryDegreeCode + qryExamMonth + qryExamYear;
                        if (isRedoStud)
                        {
                            qry += " union " + studRedoQ;
                        }
                    }
                    qry += " order by r.college_code,Batch_Year desc,degree_code,r.App_No,SubjectSem asc,subjectpriority asc,ed.Exam_year,ed.Exam_Month";
                    //dsStudentsMarksList = da.select_method_wo_parameter(qry, "text");
                }
                DataSet dsFailedCOunt = new DataSet();

                string qryGrade = "select gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Frange,gm.Trange,gm.Mark_Grade,gm.Credit_Points,gm.classify from Grade_Master gm order by gm.College_Code,gm.batch_year desc,gm.Degree_Code,gm.Frange,gm.Trange";
                dsGradeDetails = da.select_method_wo_parameter(qryGrade, "text");
                string qryClassify = "select collegecode,edu_level,frompoint,topoint,grade,classification,markgradeflag,batch_year from coe_classification order by collegecode,edu_level desc,batch_year,frompoint,topoint";
                dsClassify = da.select_method_wo_parameter(qryClassify, "text");
                for (int row = 1; row < FpCalculateCGPA.Sheets[0].RowCount; row++)
                {
                    int select = 0;
                    int.TryParse(Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 0].Value).Trim(), out select);
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
                    string appNos = string.Empty;
                    string rollNos = string.Empty;
                    int maximumDuration = 0;
                    if (select == 1)
                    {
                        DataTable dtGradeDetails = new DataTable();
                        DataTable dtClassifyDetails = new DataTable();
                        DataTable dtExamDetails = new DataTable();

                        appNos = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 0].Tag).Trim();
                        rollNos = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 0].Note).Trim();
                        eduLevel = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 1].Tag).Trim();
                        collegeCode = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 2].Tag).Trim();
                        collegeName = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 2].Text).Trim();
                        batchYear = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 3].Text).Trim();
                        redoStatus = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 3].Tag).Trim();
                        degreeCode = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 4].Tag).Trim();
                        examCode = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 4].Note).Trim();
                        currentSemester = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 5].Text).Trim();
                        monthName = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 5].Tag).Trim();
                        maxDuration = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 5].Note).Trim();

                        maximumDuration = 0;
                        int.TryParse(maxDuration, out maximumDuration);

                        examMonthYear = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 6].Text).Trim();
                        examYear = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 6].Note).Trim();
                        examMonth = Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[row, 6].Tag).Trim();

                        List<string> lstAppNo = appNos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

                        qry = "Select r.App_No,count(s.subject_no) as FailedSubjects from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r where r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and r.Roll_No=sc.roll_no and m.roll_no=r.Roll_No and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail' and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no and r.college_code='" + collegeCode + "' and r.degree_code='" + degreeCode + "' and r.batch_year='" + batchYear + "' and r.App_no in(" + appNos + ")  group by r.App_No ";
                        string qryRedoF = " Select r.App_No,count(s.subject_no) as FailedSubjects from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r,StudentRedoDetails sr where sr.DegreeCode=sm.degree_code and sm.Batch_Year=sr.BatchYear and sr.Stud_AppNo=r.App_No and r.Roll_No=sc.roll_no and m.roll_no=r.Roll_No and sm.syll_code=s.syll_code and s.syll_code=ss.syll_code and sm.syll_code=ss.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.result='fail' and m.roll_no=sc.roll_no and m.subject_no=sc.subject_no  and r.college_code='" + collegeCode + "' and sr.DegreeCode='" + degreeCode + "' and sr.BatchYear='" + batchYear + "' and r.App_no in(" + appNos + ")  group by r.App_No ";
                        if (isRedoStud)
                        {
                            qry += " union " + qryRedoF;
                        }
                        qry += "  order by r.App_No ";
                        dsFailedCOunt = da.select_method_wo_parameter(qry, "text");
                        if (dsFailedCOunt.Tables.Count > 0 && dsFailedCOunt.Tables[0].Rows.Count > 0)
                        {
                            dicStudentsFailedCount.Clear();
                            foreach (DataRow dr in dsFailedCOunt.Tables[0].Rows)
                            {
                                string appNo = Convert.ToString(dr["App_No"]).Trim();
                                string failCount = Convert.ToString(dr["FailedSubjects"]).Trim();
                                int failSubjects = 0;
                                int.TryParse(failCount, out failSubjects);
                                if (!dicStudentsFailedCount.ContainsKey(Convert.ToString(appNo).Trim()))
                                {
                                    dicStudentsFailedCount.Add(appNo, failSubjects);
                                }
                            }
                        }

                        qry = "select r.App_No,r.Roll_No,r.college_code,r.Batch_Year as Batch_Year,r.degree_code as degree_code,ed.exam_code,ed.Exam_Month,ed.Exam_year,CAST(CONVERT(nvarchar(50),Convert(varchar(10),ed.Exam_Month)+'/21/'+Convert(varchar(10),ed.Exam_year)) as Datetime) as ExamDate,r.Current_Semester as CurrentSem,ed.current_semester as AppliedSem,ed.current_semester as RedoSem,sm.semester as SubjectSem,ss.subType_no,ss.subject_type,ISNULL(ss.Lab,'0') as Lab,ISNULL(ss.ElectivePap,'0') as Elective,ISNULL(ss.projThe,'0') as Thesis,s.subject_no,s.subject_code,s.subject_name,ISNULL(s.Elective,'0') as SubElective,ISNULL(s.sub_lab,'0') as SubLab,ISNULL(s.Part_Type,'0') as Part_Type,ISNULL(s.subjectpriority,'0') as subjectpriority,s.credit_points,s.min_int_marks as MinICA,s.max_int_marks as MaxICA,s.min_ext_marks as MinESA,s.max_ext_marks as MaxESA,s.mintotal as MinTotal,s.maxtotal as MaxTotal,ISNULL(m.internal_mark,'0') as ICAMark,ISNULL(m.external_mark,'0') as ESAMark,ISNULL(m.total,'0') as Total,m.grade as Grade,m.cp, ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*100) else '0' end AS DECIMAL(10,0)) ,0,0) as TotalOutOf100,ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*10) else '0' end AS DECIMAL(10,1)),1,0) as TotalOutOf10,ISNULL(m.attempts,'0') as attempts,m.entry_code,m.passorfail,m.result,'0' as REDO from mark_entry m,subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed,Registration r where sc.subject_no=m.subject_no and m.roll_no=sc.roll_no and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and sm.Batch_Year=ed.batch_year and sm.degree_code=ed.degree_code and r.Roll_No=sc.roll_no and r.Roll_No=m.roll_no and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and sm.degree_code=r.degree_code and r.Batch_Year=sm.Batch_Year and m.exam_code=ed.exam_code and ed.Exam_Month<>'0' and ed.Exam_year<>'0' and r.college_code='" + collegeCode + "' and r.degree_code='" + degreeCode + "' and r.batch_year='" + batchYear + "' and r.App_no in(" + appNos + ")  ";
                        string studRedoQ = " select r.App_No,r.Roll_No,r.college_code,sr.BatchYear as Batch_Year,sr.DegreeCode as degree_code,ed.exam_code,ed.Exam_Month,ed.Exam_year,CAST(CONVERT(nvarchar(50),Convert(varchar(10),ed.Exam_Month)+'/21/'+Convert(varchar(10),ed.Exam_year)) as Datetime) as ExamDate,r.Current_Semester as CurrentSem,ed.current_semester as AppliedSem,sr.Semester as RedoSem,sm.semester as SubjectSem,ss.subType_no,ss.subject_type,ISNULL(ss.Lab,'0') as Lab,ISNULL(ss.ElectivePap,'0') as Elective,ISNULL(ss.projThe,'0') as Thesis,s.subject_no,s.subject_code,s.subject_name,ISNULL(s.Elective,'0') as SubElective,ISNULL(s.sub_lab,'0') as SubLab,ISNULL(s.Part_Type,'0') as Part_Type,ISNULL(s.subjectpriority,'0') as subjectpriority,s.credit_points,s.min_int_marks as MinICA,s.max_int_marks as MaxICA,s.min_ext_marks as MinESA,s.max_ext_marks as MaxESA,s.mintotal as MinTotal,s.maxtotal as MaxTotal,ISNULL(m.internal_mark,'0') as ICAMark,ISNULL(m.external_mark,'0') as ESAMark,ISNULL(m.total,'0') as Total,m.grade as Grade,m.cp, ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*100) else '0' end AS DECIMAL(10,0)) ,0,0) as TotalOutOf100,ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*10) else '0' end AS DECIMAL(10,1)),1,0) as TotalOutOf10,ISNULL(m.attempts,'0') as attempts,m.entry_code,m.passorfail,m.result,'1' as REDO from mark_entry m,subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Exam_Details ed,Registration r,StudentRedoDetails sr where  sc.subject_no=m.subject_no and m.roll_no=sc.roll_no and sr.Stud_AppNo=r.App_No and s.subject_no=sc.subject_no and s.subject_no=m.subject_no and s.syll_code=ss.syll_code and ss.syll_code=sm.syll_code and s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and sm.Batch_Year=ed.batch_year and sm.degree_code=ed.degree_code and r.Roll_No=sc.roll_no and r.Roll_No=m.roll_no and sr.BatchYear=ed.batch_year and ed.degree_code=sr.DegreeCode and sm.degree_code=sr.DegreeCode and sr.BatchYear=sm.Batch_Year and m.exam_code=ed.exam_code and ed.Exam_Month<>'0' and ed.Exam_year<>'0' and r.college_code='" + collegeCode + "' and sr.DegreeCode='" + degreeCode + "' and sr.BatchYear='" + batchYear + "' and r.App_no in(" + appNos + ") ";
                        if (isRedoStud)
                        {
                            qry += " union " + studRedoQ;
                        }
                        //qry += " order by r.college_code,Batch_Year desc,degree_code,r.App_No,SubjectSem asc,subjectpriority asc,ed.Exam_year,ed.Exam_Month";
                        qry += " order by r.college_code,Batch_Year desc,degree_code,r.App_No,ed.Exam_year,ed.Exam_Month,SubjectSem asc,subjectpriority asc";
                        DataSet dsAllStudentsMarksList = new DataSet();
                        dsAllStudentsMarksList = da.select_method_wo_parameter(qry, "text");

                        if (dsGradeDetails.Tables.Count > 0 && dsGradeDetails.Tables[0].Rows.Count > 0)
                        {
                            dsGradeDetails.Tables[0].DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "'";
                            dtGradeDetails = dsGradeDetails.Tables[0].DefaultView.ToTable();
                        }
                        if (dsClassify.Tables.Count > 0 && dsClassify.Tables[0].Rows.Count > 0)
                        {
                            dtClassifyDetails = dsClassify.Tables[0].DefaultView.ToTable();
                        }
                        if (dsAllStudentsMarksList.Tables.Count > 0 && dsAllStudentsMarksList.Tables[0].Rows.Count > 0)
                        {
                            //dsAllStudentsMarksList.Tables[0].DefaultView.RowFilter = "Exam_Month in(" + ExamMonth + ") and Exam_Year in(" + ExamYear + ")";
                            dtExamDetails = dsAllStudentsMarksList.Tables[0].DefaultView.ToTable(true, "exam_code", "college_code", "Batch_Year", "degree_code", "AppliedSem");
                            for (int i = 0; i < lstAppNo.Count; i++)
                            {
                                double wam = 0;
                                double cwam = 0;
                                double gpa = 0;
                                double cgpa = 0;
                                double totalEarnedCredits = 0;
                                double sumOfGpa = 0;
                                double sumOfWeightedMarks = 0;
                                double totalSecuredMarks = 0;
                                double totalMaxMarks = 0;
                                double avg = 0;

                                string gpaGrade = string.Empty;
                                string gpaClassify = string.Empty;
                                string cgpaGrade = string.Empty;
                                string cgpaClassify = string.Empty;

                                bool isCalculated = false;
                                DataView dv = new DataView();

                                int totalAttendSubjects = 0;
                                int totalPassedSubjects = 0;
                                int totalFailedSubjects = 0;
                                int totalCurrentSubjects = 0;
                                int totalCurrentPassedSubjects = 0;
                                int totalCurrentFailedSubjects = 0;
                                int totalArrearSubjects = 0;
                                int totalArrearPassedSubjects = 0;
                                int totalArrearFailedSubjects = 0;
                                int result1 = 0;

                                DataTable dtAttend = new DataTable();
                                DataTable dtPassed = new DataTable();
                                DataTable dtFailed = new DataTable();
                                DataTable dtCurrentAppeared = new DataTable();
                                DataTable dtCurrentPassed = new DataTable();
                                DataTable dtCurrentFailed = new DataTable();
                                DataTable dtArrearAppeared = new DataTable();
                                DataTable dtArrearPassed = new DataTable();
                                DataTable dtArrearFailed = new DataTable();
                                DataTable dtStudMarks = new DataTable();

                                int studentFailCount = 0;
                                if (dicStudentsFailedCount.ContainsKey(Convert.ToString(lstAppNo[i]).Trim()))
                                {
                                    studentFailCount = dicStudentsFailedCount[Convert.ToString(lstAppNo[i]).Trim()];
                                }
                                if (dtExamDetails.Rows.Count > 0)
                                {
                                    foreach (DataRow drExamDetails in dtExamDetails.Rows)
                                    {
                                        string examCodeNew = Convert.ToString(drExamDetails["exam_code"]).Trim();
                                        string collegeCodeNew = Convert.ToString(drExamDetails["college_code"]).Trim();
                                        string batchYearNew = Convert.ToString(drExamDetails["Batch_Year"]).Trim();
                                        string degreeCodeNew = Convert.ToString(drExamDetails["degree_code"]).Trim();
                                        string appliedSemesterNew = Convert.ToString(drExamDetails["AppliedSem"]).Trim();
                                        DataTable dtStudentsExamMarks = new DataTable();
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;
                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        dsAllStudentsMarksList.Tables[0].DefaultView.RowFilter = "exam_code='" + examCodeNew + "' and college_code='" + collegeCodeNew + "' and Batch_Year='" + batchYearNew + "' and degree_code='" + degreeCodeNew + "' and app_no='" + lstAppNo[i].ToString().Trim() + "'";
                                        dtStudentsExamMarks = dsAllStudentsMarksList.Tables[0].DefaultView.ToTable();
                                        if (dtStudentsExamMarks.Rows.Count > 0)
                                        {
                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;
                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;
                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, type: 0);

                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','0')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                                if (result1 > 0)
                                                {
                                                    isCalculateSave = true;
                                                }
                                            }
                                            //wam = 0;
                                            //cwam = 0;
                                            //gpa = 0;
                                            //cgpa = 0;
                                            //totalEarnedCredits = 0;
                                            //sumOfGpa = 0;
                                            //sumOfWeightedMarks = 0;
                                            //totalSecuredMarks = 0;
                                            //totalMaxMarks = 0;
                                            //avg = 0;

                                            //gpaGrade = string.Empty;
                                            //gpaClassify = string.Empty;
                                            //cgpaGrade = string.Empty;
                                            //cgpaClassify = string.Empty;

                                            //totalAttendSubjects = 0;
                                            //dtAttend = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'aaa'";
                                            //dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalAttendSubjects = dtAttend.Rows.Count;

                                            //totalPassedSubjects = 0;
                                            //dtPassed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result='pass'";
                                            //dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalPassedSubjects = dtPassed.Rows.Count;

                                            //totalFailedSubjects = 0;
                                            //dtFailed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                            //dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalFailedSubjects = dtFailed.Rows.Count;

                                            //totalCurrentSubjects = 0;
                                            //dtCurrentAppeared = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'aaa'";
                                            //dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            //totalCurrentPassedSubjects = 0;
                                            //dtCurrentPassed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result='pass'";
                                            //dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            //totalCurrentFailedSubjects = 0;
                                            //dtCurrentFailed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                            //dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            //totalArrearSubjects = 0;
                                            //dtArrearAppeared = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'aaa'";
                                            //dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            //totalArrearPassedSubjects = 0;
                                            //dtArrearPassed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result='pass'";
                                            //dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            //totalArrearFailedSubjects = 0;
                                            //dtArrearFailed = new DataTable();
                                            //dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                            //dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            //isCalculated = false;
                                            //isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, type: 0);
                                            //if (isCalculated)
                                            //{
                                            //    //qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','6')";
                                            //    //result1 = da.update_method_wo_parameter(qry, "text");
                                            //    if (result1 > 0)
                                            //    {
                                            //        isCalculateSave = true;
                                            //    }
                                            //}
                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;
                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;

                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='1'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'  and Part_Type='1'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='1'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='1'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='1'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='1'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='1'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='1'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='1'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, partNo: "1", type: 0);
                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','1')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                                if (result1 > 0)
                                                {
                                                    isCalculateSave = true;
                                                }
                                            }

                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;
                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;

                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='2'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'  and Part_Type='2'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='2'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='2'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='2'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='2'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='2'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='2'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='2'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, partNo: "2", type: 0);
                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','2')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                                if (result1 > 0)
                                                {
                                                    isCalculateSave = true;
                                                }
                                            }
                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;
                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;

                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='3'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'  and Part_Type='3'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='3'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='3'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='3'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='3'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='3'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='3'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='3'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, partNo: "3", type: 0);
                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','3')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                            }
                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;

                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;

                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='4'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'  and Part_Type='4'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='4'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='4'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='4'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='4'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='4'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='4'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='4'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, partNo: "4", type: 0);
                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','4')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                                if (result1 > 0)
                                                {
                                                    isCalculateSave = true;
                                                }
                                            }

                                            wam = 0;
                                            cwam = 0;
                                            gpa = 0;
                                            cgpa = 0;
                                            totalEarnedCredits = 0;
                                            sumOfGpa = 0;
                                            sumOfWeightedMarks = 0;
                                            totalSecuredMarks = 0;
                                            totalMaxMarks = 0;
                                            avg = 0;

                                            gpaGrade = string.Empty;
                                            gpaClassify = string.Empty;
                                            cgpaGrade = string.Empty;
                                            cgpaClassify = string.Empty;

                                            totalAttendSubjects = 0;
                                            dtAttend = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='5'";
                                            dtAttend = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalAttendSubjects = dtAttend.Rows.Count;

                                            totalPassedSubjects = 0;
                                            dtPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "'  and Part_Type='5'";
                                            dtPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalPassedSubjects = dtPassed.Rows.Count;

                                            totalFailedSubjects = 0;
                                            dtFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='5'";
                                            dtFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalFailedSubjects = dtFailed.Rows.Count;

                                            totalCurrentSubjects = 0;
                                            dtCurrentAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem='" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='5'";
                                            dtCurrentAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                            totalCurrentPassedSubjects = 0;
                                            dtCurrentPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='5'";
                                            dtCurrentPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                            totalCurrentFailedSubjects = 0;
                                            dtCurrentFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + appliedSemesterNew + "' and Part_Type='5'";
                                            dtCurrentFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                            totalArrearSubjects = 0;
                                            dtArrearAppeared = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "SubjectSem<>'" + appliedSemesterNew + "' and result<>'aaa' and Part_Type='5'";
                                            dtArrearAppeared = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                            totalArrearPassedSubjects = 0;
                                            dtArrearPassed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='5'";
                                            dtArrearPassed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                            totalArrearFailedSubjects = 0;
                                            dtArrearFailed = new DataTable();
                                            dtStudentsExamMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + appliedSemesterNew + "' and Part_Type='5'";
                                            dtArrearFailed = dtStudentsExamMarks.DefaultView.ToTable(true, "subject_code");
                                            totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                            isCalculated = false;
                                            isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudentsExamMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, appliedSemesterNew, partNo: "5", type: 0);
                                            if (isCalculated)
                                            {
                                                qry = " if exists (select * from CalculateExamWiseGPA_CGPA where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5') update CalculateExamWiseGPA_CGPA set TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',examWiseGpa='" + gpa + "',examWiseWam='" + wam + "',examWiseCGpa='" + cgpa + "',examWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "' where exam_code='" + examCodeNew + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5' else insert into CalculateExamWiseGPA_CGPA (app_no,exam_code,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,examWiseGpa,examWiseWam,examWiseCGpa,examWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + examCodeNew + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','5')";
                                                result1 = da.update_method_wo_parameter(qry, "text");
                                                if (result1 > 0)
                                                {
                                                    isCalculateSave = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                for (int sem = 1; sem <= maximumDuration; sem++)
                                {
                                    dtStudMarks = new DataTable();
                                    dsAllStudentsMarksList.Tables[0].DefaultView.RowFilter = "app_no='" + lstAppNo[i].ToString().Trim() + "' and SubjectSem='" + sem + "'";
                                    dtStudMarks = dsAllStudentsMarksList.Tables[0].DefaultView.ToTable(true);
                                    if (dtStudMarks.Rows.Count > 0)
                                    {
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;
                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;
                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;
                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;
                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;
                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;
                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;
                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;
                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;
                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;
                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;
                                        //totalAttendSubjects = dtStudMarks.Rows.Count;
                                        //totalPassedSubjects = 0;
                                        //totalFailedSubjects = 0;
                                        //totalCurrentSubjects = 0;
                                        //totalArrearSubjects = 0;
                                        //totalCurrentPassedSubjects = 0;
                                        //totalArrearPassedSubjects = 0;
                                        //totalCurrentFailedSubjects = 0;
                                        //totalArrearFailedSubjects = 0;
                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, Convert.ToString(sem).Trim(), type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='0' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','0')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;
                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        //isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, type: 1);
                                        if (isCalculated)
                                        {
                                            //qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='6' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','6')";
                                            //result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;

                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and Part_type='1' and result<>'aaa'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='1'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='1'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and Part_type='1' and result<>'aaa'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='1'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='1'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa' and Part_type='1'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "' and Part_type='1'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "' and Part_type='1'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, sem.ToString(), partNo: "1", type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='1' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','1')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;

                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='2'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='2'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='2'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='2'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='2'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;
                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='2'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa' and Part_type='2'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "' and Part_type='2'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "' and Part_type='2'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, sem.ToString(), partNo: "2", type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='2' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','2')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;

                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='3'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='3'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='3'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='3'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='3'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='3'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa' and Part_type='3'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "' and Part_type='3'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "' and Part_type='3'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, sem.ToString(), partNo: "3", type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='3' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','3')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;

                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='4'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='4'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='4'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='4'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='4'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='4'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa' and Part_type='4'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "' and Part_type='4'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "' and Part_type='4'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, sem.ToString(), partNo: "4", type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='4' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','4')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                        wam = 0;
                                        cwam = 0;
                                        gpa = 0;
                                        cgpa = 0;
                                        totalEarnedCredits = 0;
                                        sumOfGpa = 0;
                                        sumOfWeightedMarks = 0;
                                        totalSecuredMarks = 0;
                                        totalMaxMarks = 0;
                                        avg = 0;

                                        gpaGrade = string.Empty;
                                        gpaClassify = string.Empty;
                                        cgpaGrade = string.Empty;
                                        cgpaClassify = string.Empty;

                                        totalAttendSubjects = 0;
                                        dtAttend = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='5'";
                                        dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalAttendSubjects = dtAttend.Rows.Count;

                                        totalPassedSubjects = 0;
                                        dtPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='5'";
                                        dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalPassedSubjects = dtPassed.Rows.Count;

                                        totalFailedSubjects = 0;
                                        dtFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='5'";
                                        dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalFailedSubjects = dtFailed.Rows.Count;

                                        totalCurrentSubjects = 0;
                                        dtCurrentAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem='" + sem + "' and result<>'aaa' and Part_type='5'";
                                        dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                        totalCurrentPassedSubjects = 0;
                                        dtCurrentPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem='" + sem + "' and Part_type='5'";
                                        dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                        totalCurrentFailedSubjects = 0;
                                        dtCurrentFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem='" + sem + "' and Part_type='5'";
                                        dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                        totalArrearSubjects = 0;
                                        dtArrearAppeared = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "SubjectSem<>'" + sem + "' and result<>'aaa' and Part_type='5'";
                                        dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                        totalArrearPassedSubjects = 0;
                                        dtArrearPassed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result='pass' and SubjectSem<>'" + sem + "' and Part_type='5'";
                                        dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                        totalArrearFailedSubjects = 0;
                                        dtArrearFailed = new DataTable();
                                        dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and SubjectSem<>'" + sem + "' and Part_type='5'";
                                        dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                        totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                        isCalculated = false;
                                        isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, sem.ToString(), partNo: "5", type: 1);
                                        if (isCalculated)
                                        {
                                            qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='5' where semester='" + Convert.ToString(sem).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','5')";
                                            result1 = da.update_method_wo_parameter(qry, "text");
                                            if (result1 > 0)
                                            {
                                                isCalculateSave = true;
                                            }
                                        }
                                    }
                                }
                                dtStudMarks = new DataTable();
                                dsAllStudentsMarksList.Tables[0].DefaultView.RowFilter = "app_no='" + lstAppNo[i].ToString().Trim() + "'";
                                dtStudMarks = dsAllStudentsMarksList.Tables[0].DefaultView.ToTable(true);
                                string sem1 = "0";
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                dv = new DataView();
                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;
                                //totalAttendSubjects = dtStudMarks.Rows.Count;

                                //totalPassedSubjects = 0;
                                //totalFailedSubjects = 0;
                                //totalCurrentSubjects = 0;
                                //totalArrearSubjects = 0;
                                //totalCurrentPassedSubjects = 0;
                                //totalArrearPassedSubjects = 0;
                                //totalCurrentFailedSubjects = 0;
                                //totalArrearFailedSubjects = 0;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='0' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='0' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','0')";
                                    int results = da.update_method_wo_parameter(qry, "text");
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, type: 1);
                                if (isCalculated)
                                {
                                    //qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='6' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='6' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','6')";
                                    //result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "Part_type='1' and result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='1'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='1'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='1' and result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'  and Part_type='1'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'  and Part_type='1'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='1' and result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass'  and Part_type='1'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='1'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, partNo: "1", type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='1' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='1' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','1')";
                                    result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "Part_type='2' and result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='2'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='2'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='2' and result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='2'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='2'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='2' and result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='2'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa'  and Part_type='2'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, partNo: "2", type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='2' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='2' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','2')";
                                    result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "Part_type='3' and result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='3'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='3'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='3' and result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='3'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='3'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='3' and result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='3'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='3'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, partNo: "3", type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='3' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='3' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','3')";
                                    result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "Part_type='4' and result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='4'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='4'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='4' and result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='4'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='4'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='4' and result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='4'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='4'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, partNo: "4", type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='4' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='4' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','4')";
                                    result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                wam = 0;
                                cwam = 0;
                                gpa = 0;
                                cgpa = 0;
                                totalEarnedCredits = 0;
                                sumOfGpa = 0;
                                sumOfWeightedMarks = 0;
                                totalSecuredMarks = 0;
                                totalMaxMarks = 0;
                                avg = 0;

                                gpaGrade = string.Empty;
                                gpaClassify = string.Empty;
                                cgpaGrade = string.Empty;
                                cgpaClassify = string.Empty;

                                totalAttendSubjects = 0;
                                dtAttend = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "Part_type='5' and result<>'aaa'";
                                dtAttend = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalAttendSubjects = dtAttend.Rows.Count;

                                totalPassedSubjects = 0;
                                dtPassed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='5'";
                                dtPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalPassedSubjects = dtPassed.Rows.Count;

                                totalFailedSubjects = 0;
                                dtFailed = new DataTable();
                                dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='5'";
                                dtFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                totalFailedSubjects = dtFailed.Rows.Count;

                                totalCurrentSubjects = 0;
                                //dtCurrentAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='5' and result<>'aaa'";
                                //dtCurrentAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentSubjects = dtCurrentAppeared.Rows.Count;

                                totalCurrentPassedSubjects = 0;
                                //dtCurrentPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='5'";
                                //dtCurrentPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentPassedSubjects = dtCurrentPassed.Rows.Count;

                                totalCurrentFailedSubjects = 0;
                                //dtCurrentFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='5'";
                                //dtCurrentFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalCurrentFailedSubjects = dtCurrentFailed.Rows.Count;

                                totalArrearSubjects = 0;
                                //dtArrearAppeared = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "Part_type='5' and result<>'aaa'";
                                //dtArrearAppeared = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearSubjects = dtArrearAppeared.Rows.Count;

                                totalArrearPassedSubjects = 0;
                                //dtArrearPassed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result='pass' and Part_type='5'";
                                //dtArrearPassed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearPassedSubjects = dtArrearPassed.Rows.Count;

                                totalArrearFailedSubjects = 0;
                                //dtArrearFailed = new DataTable();
                                //dtStudMarks.DefaultView.RowFilter = "result<>'pass' and result<>'aaa' and Part_type='5'";
                                //dtArrearFailed = dtStudMarks.DefaultView.ToTable(true, "subject_code");
                                //totalArrearFailedSubjects = dtArrearFailed.Rows.Count;

                                isCalculated = false;
                                isCalculated = CalculateGPA_CGPAPartWise(dtGradeDetails, dtStudMarks, dtClassifyDetails, Convert.ToString(lstAppNo[i]).Trim(), studentFailCount, eduLevel, ref gpa, ref wam, ref cgpa, ref cwam, ref gpaGrade, ref gpaClassify, ref cgpaGrade, ref cgpaClassify, ref totalEarnedCredits, ref sumOfGpa, ref sumOfWeightedMarks, ref totalMaxMarks, ref totalSecuredMarks, ref avg, partNo: "5", type: 1);
                                if (isCalculated)
                                {
                                    qry = "if exists (select * from CalculateSemWiseGPA_CGPA where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5') update CalculateSemWiseGPA_CGPA set app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "',semester='" + Convert.ToString(sem1).Trim() + "',TotalObtainedMarks='" + totalSecuredMarks + "',TotalMarks='" + totalMaxMarks + "',Average='" + avg + "',TotalGradePoints='" + sumOfGpa + "',TotalWeightageMark='" + sumOfWeightedMarks + "',TotalEarnedCredits='" + totalEarnedCredits + "',SemWiseGpa='" + gpa + "',SemWiseWam='" + wam + "',SemWiseCGpa='" + cgpa + "',SemWiseCwam='" + cwam + "',gpaGrade='" + gpaGrade + "',gpaClassification='" + gpaClassify + "',cgpaGrade='" + cgpaGrade + "',cgpaClassification='" + cgpaClassify + "',calculatedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',calculatedTime='" + DateTime.Now.ToString("HH:mm:ss") + "',modifiedUserCode='" + userCode + "',totalAttendSubjects='" + totalAttendSubjects + "',totalPassedSubjects='" + totalPassedSubjects + "',totalFailedSubjects='" + totalFailedSubjects + "',totalCurrentSubjects='" + totalCurrentSubjects + "',totalArrearSubjects='" + totalArrearSubjects + "',totalCurrentPassedSubjects='" + totalCurrentPassedSubjects + "',totalArrearPassedSubjects='" + totalArrearPassedSubjects + "',totalCurrentFailedSubjects='" + totalCurrentFailedSubjects + "',totalArrearFailedSubjects='" + totalArrearFailedSubjects + "',type='5' where semester='" + Convert.ToString(sem1).Trim() + "' and app_no='" + Convert.ToString(lstAppNo[i]).Trim() + "' and type='5' else insert into CalculateSemWiseGPA_CGPA (app_no,semester,TotalObtainedMarks,TotalMarks,Average,TotalGradePoints,TotalWeightageMark,TotalEarnedCredits,SemWiseGpa,SemWiseWam,SemWiseCGpa,SemWiseCwam,gpaGrade,gpaClassification,cgpaGrade,cgpaClassification,calculatedDate,calculatedTime,modifiedUserCode,totalAttendSubjects,totalPassedSubjects,totalFailedSubjects,totalCurrentSubjects,totalArrearSubjects,totalCurrentPassedSubjects,totalArrearPassedSubjects,totalCurrentFailedSubjects,totalArrearFailedSubjects,type) values ('" + Convert.ToString(lstAppNo[i]).Trim() + "','" + Convert.ToString(sem1).Trim() + "','" + totalSecuredMarks + "','" + totalMaxMarks + "','" + avg + "','" + sumOfGpa + "','" + sumOfWeightedMarks + "','" + totalEarnedCredits + "','" + gpa + "','" + wam + "','" + cgpa + "','" + cwam + "','" + gpaGrade + "','" + gpaClassify + "','" + cgpaGrade + "','" + cgpaClassify + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','" + userCode + "','" + totalAttendSubjects + "','" + totalPassedSubjects + "','" + totalFailedSubjects + "','" + totalCurrentSubjects + "','" + totalArrearSubjects + "','" + totalCurrentPassedSubjects + "','" + totalArrearPassedSubjects + "','" + totalCurrentFailedSubjects + "','" + totalArrearFailedSubjects + "','5')";
                                    result1 = da.update_method_wo_parameter(qry, "text");
                                    if (result1 > 0)
                                    {
                                        isCalculateSave = true;
                                    }
                                }
                                if (result1 > 0)
                                {
                                    isCalculateSave = true;
                                }
                            }
                        }
                    }
                }
            }
            if (isCalculateSave)
            {
                lblAlertMsg.Text = "CGPA And GPA are Calculated Successfully!!!";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "CGPA And GPA are Not Calculated!!!";
                lblAlertMsg.Visible = true;
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

    #endregion

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
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
                if (FpCalculateCGPA.Visible == true)
                {
                    da.printexcelreport(FpCalculateCGPA, reportname);
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Over All GPA And CGPA Calculation Report";
            string pagename = "GPA_CGPA_CalculationProcess.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpCalculateCGPA.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpCalculateCGPA, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
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

    #region Reusable Methods

    /// <summary>
    /// This Method is used For Get the Current Semester For Arts & Science College
    /// Exam Month is 4 for even semester or 11 for odd sem
    /// </summary>
    /// <param name="batchyr"></param>
    /// <param name="exmyr"></param>
    /// <param name="month">4-APR or 11 - NOV ONLY</param>
    /// <param name="max_sem"></param>
    /// <param name="cur_sem"></param>
    public void GetSem(string batchyr, string exmyr, string month, string max_sem, ref string cur_sem)
    {
        int batchyear = 0;
        int.TryParse(batchyr, out batchyear);
        int exmyear = 0;
        int.TryParse(exmyr, out exmyear);
        int mon = 0;
        int.TryParse(month, out mon);
        int maxsem = 0;
        int.TryParse(max_sem, out maxsem);
        int year = 0;
        year = exmyear - batchyear;
        int oddoreven = year % 2;
        int cursem = 1;
        int year1 = maxsem / 2;
        // case when ((exam_year-batch_year)<=(duration/2)) then when (year == 0 && mon == 11) then '1' when ((exam_year-batch_year)%2 = 1 && Exam_Month = 4) then  (exam_year-batch_year)+(exam_year-batch_year)  when ((exam_year-batch_year)%2 = 1 && Exam_Month = 11) then  (exam_year-batch_year)+2  when (((duration/2) = (exam_year-batch_year) && Exam_Month == 11) && ((exam_year-batch_year) > (duration/2) || Exam_Month == 4 || Exam_Month == 11)) then duration+1  end
        if (year <= year1 && (mon == 4 || mon == 11))
        {
            if (year == 0 && mon == 11)
            {
                cur_sem = "1";
            }
            else if (year == 0 && mon == 4)
            {
                //cur_sem = "1";
            }
            else if (oddoreven == 1 && mon == 4)
            {
                cursem = year + year;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 1 && mon == 11)
            {
                cursem += year + 1;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 0 && mon == 4)
            {
                cursem = 0;
                cursem += year + 2;
                cur_sem = Convert.ToString(cursem);
            }
            else if (oddoreven == 0 && mon == 11)
            {
                cursem += year + 2;
                cur_sem = Convert.ToString(cursem);
            }
            if (cursem > maxsem)
            {
                cur_sem = Convert.ToString(maxsem + 1);
            }
            if ((year1 == year && mon == 11))
            {
                cur_sem = Convert.ToString(maxsem + 1);
            }
        }
        else if ((year1 == year && mon == 11) || (year > year1 || mon == 4 || mon == 11))
        {
            cur_sem = Convert.ToString(maxsem + 1);
        }
    }

    /// <summary>
    /// This Method is Used To Calculate GPA And CGPA Calculation
    /// Developed By Malang Raja
    /// </summary>
    /// <param name="dtGrades"></param>
    /// <param name="dtStudentMarks"></param>
    /// <param name="dtClassify"></param>
    /// <param name="appNo"></param>
    /// <param name="failCount"></param>
    /// <param name="gpa"></param>
    /// <param name="wam"></param>
    /// <param name="cgpa"></param>
    /// <param name="grade"></param>
    /// <param name="classify"></param>
    /// <param name="totalEarnedCredits"></param>
    /// <param name="sumOfGP"></param>
    /// <param name="sumOfWeitageMarks"></param>
    /// <param name="maxTotal"></param>
    /// <param name="totalMarks"></param>
    /// <param name="avg"></param>
    /// <param name="semester"></param>
    /// <param name="partNo">null or 0 or 1</param>
    /// <param name="type">0 or 1</param>
    private bool CalculateGPA_CGPAPartWise(DataTable dtGrades, DataTable dtStudentMarks, DataTable dtClassify, string appNo, int failCount, string eduLevel, ref double gpa, ref double wam, ref double cgpa, ref double cwam, ref string gpaGrade, ref string gpaClassify, ref string cgpaGrade, ref string cgpaClassify, ref double totalEarnedCredits, ref double sumOfGP, ref double sumOfWeitageMarks, ref double maxTotal, ref double totalMarks, ref double avg, string semester = null, string partNo = null, int type = 0)
    {
        try
        {
            bool hasCalculated = false;
            string filterBy = string.Empty;
            string filterByCgpa = string.Empty;
            double totalCredits = 0;
            double actualTotalSecuredMarks = 0;
            double actualTotalMaxMarks = 0;
            double actualaverageMarks = 0;
            double totalSecuredMarks = 0;
            double totalMaxMarks = 0;
            double averageMarks = 0;
            double gradePoint = 0;
            double totalGradePoints = 0;
            double weightageMarks = 0;
            double totalWeightageMarks = 0;
            DataTable dtMarks = new DataTable();
            DataTable dtCgpaMarks = new DataTable();
            if (partNo != null && !string.IsNullOrEmpty(partNo.Trim()))
            {
                if (string.IsNullOrEmpty(filterBy.Trim()))
                {
                    filterBy = " Part_Type='" + partNo + "'";
                    filterByCgpa = " Part_Type='" + partNo + "'";
                }
                else
                {
                    filterBy += " and Part_Type='" + partNo + "'";
                    filterByCgpa += " and Part_Type='" + partNo + "'";
                }
            }
            if (appNo != null && !string.IsNullOrEmpty(appNo.Trim()))
            {
                if (string.IsNullOrEmpty(filterBy.Trim()))
                {
                    filterBy = " app_no='" + appNo + "'";
                    filterByCgpa = " app_no='" + appNo + "'";
                }
                else
                {
                    filterBy += " and app_no='" + appNo + "'";
                    filterByCgpa += " and app_no='" + appNo + "'";
                }
            }
            if (semester != null && !string.IsNullOrEmpty(semester.Trim()))
            {
                if (string.IsNullOrEmpty(filterBy.Trim()))
                {
                    if (type == 0)
                    {
                        filterBy = " AppliedSem='" + semester + "'";
                        filterByCgpa = " AppliedSem<='" + semester + "'";
                    }
                    else
                    {
                        filterBy = " SubjectSem='" + semester + "'";
                        filterByCgpa = " SubjectSem='" + semester + "'";
                    }
                }
                else
                {
                    if (type == 0)
                    {
                        filterBy += " and AppliedSem='" + semester + "'";
                        filterByCgpa += " and AppliedSem<='" + semester + "'";
                    }
                    else
                    {
                        filterBy += " and SubjectSem='" + semester + "'";
                        filterByCgpa += " and SubjectSem='" + semester + "'";
                    }
                }
            }
            if (dtStudentMarks.Rows.Count > 0)
            {
                totalCredits = 0;
                actualTotalSecuredMarks = 0;
                actualTotalMaxMarks = 0;
                actualaverageMarks = 0;
                totalSecuredMarks = 0;
                totalMaxMarks = 0;
                averageMarks = 0;
                gradePoint = 0;
                totalGradePoints = 0;
                weightageMarks = 0;
                totalWeightageMarks = 0;
                if (!string.IsNullOrEmpty(filterBy))
                {
                    dtStudentMarks.DefaultView.RowFilter = filterBy + " " + " and result='pass'";
                    dtMarks = dtStudentMarks.DefaultView.ToTable(true);
                }
                else
                {
                    dtStudentMarks.DefaultView.RowFilter = "result='pass'";
                    dtMarks = dtStudentMarks.DefaultView.ToTable(true);
                }
                if (!string.IsNullOrEmpty(filterByCgpa))
                {
                    dtStudentMarks.DefaultView.RowFilter = filterByCgpa + " " + " and result='pass'";
                    dtCgpaMarks = dtStudentMarks.DefaultView.ToTable(true);
                }
                else
                {
                    dtStudentMarks.DefaultView.RowFilter = "result='pass'";
                    dtCgpaMarks = dtStudentMarks.DefaultView.ToTable(true);
                }
                if (dtMarks.Rows.Count > 0)
                {
                    hasCalculated = true;
                    totalCredits = 0;
                    actualTotalSecuredMarks = 0;
                    actualTotalMaxMarks = 0;
                    actualaverageMarks = 0;
                    totalSecuredMarks = 0;
                    totalMaxMarks = 0;
                    averageMarks = 0;
                    gradePoint = 0;
                    totalGradePoints = 0;
                    weightageMarks = 0;
                    totalWeightageMarks = 0;
                    string collegeCode = string.Empty;
                    string batchYear = string.Empty;
                    string degreeCode = string.Empty;
                    string subjectCode = string.Empty;
                    string subjectNo = string.Empty;
                    string examCode = string.Empty;
                    string subjectSem = string.Empty;
                    string partType = string.Empty;
                    string entryCode = string.Empty;
                    foreach (DataRow drMarks in dtMarks.Rows)
                    {
                        collegeCode = Convert.ToString(drMarks["college_code"]).Trim();
                        batchYear = Convert.ToString(drMarks["Batch_Year"]).Trim();
                        degreeCode = Convert.ToString(drMarks["degree_code"]).Trim();
                        subjectCode = Convert.ToString(drMarks["subject_code"]).Trim();
                        subjectNo = Convert.ToString(drMarks["subject_no"]).Trim();
                        examCode = Convert.ToString(drMarks["exam_code"]).Trim();
                        subjectSem = Convert.ToString(drMarks["SubjectSem"]).Trim();
                        partType = Convert.ToString(drMarks["Part_Type"]).Trim();
                        entryCode = Convert.ToString(drMarks["entry_code"]).Trim();
                        string studentRollNo = Convert.ToString(drMarks["Roll_No"]).Trim();
                        string studAppNo = Convert.ToString(drMarks["App_No"]).Trim();
                        string creditPoint = Convert.ToString(drMarks["credit_points"]).Trim();
                        string totalOUTOff100 = Convert.ToString(drMarks["TotalOutOf100"]).Trim();
                        string totalOUTOff10 = Convert.ToString(drMarks["TotalOutOf10"]).Trim();
                        string actualTotal = Convert.ToString(drMarks["Total"]).Trim();
                        string actualMaxTotal = Convert.ToString(drMarks["MaxTotal"]).Trim();
                        string gradeVal = string.Empty;
                        string classifyVal = string.Empty;
                        double creditPoints = 0;
                        double.TryParse(creditPoint, out creditPoints);
                        double totalOUTOff100s = 0;
                        double.TryParse(totalOUTOff100, out totalOUTOff100s);
                        double totalOUTOff10s = 0;
                        double.TryParse(totalOUTOff10, out totalOUTOff10s);
                        double actualTotals = 0;
                        double.TryParse(actualTotal, out actualTotals);
                        double actualMaxTotals = 0;
                        double.TryParse(actualMaxTotal, out actualMaxTotals);
                        totalCredits += creditPoints;
                        totalSecuredMarks += totalOUTOff100s;
                        totalMaxMarks += 100;
                        gradePoint = 0;
                        gradePoint = totalOUTOff10s * creditPoints;
                        totalGradePoints += gradePoint;
                        weightageMarks = 0;
                        weightageMarks = totalOUTOff100s * creditPoints;
                        totalWeightageMarks += weightageMarks;
                        actualTotalSecuredMarks += actualTotals;
                        actualTotalMaxMarks += actualMaxTotals;
                        if (dtGrades.Rows.Count > 0)
                        {
                            DataView dvGrade = new DataView();
                            double totMarks = Math.Round(totalOUTOff100s, 0, MidpointRounding.AwayFromZero);
                            dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + subjectSem + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                            dvGrade = dtGrades.DefaultView;
                            if (dvGrade.Count == 0)
                            {
                                dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                                dvGrade = dtGrades.DefaultView;
                            }
                            if (dvGrade.Count > 0)
                            {
                                dvGrade.Sort = "Frange desc";
                                gradeVal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                classifyVal = Convert.ToString(dvGrade[0]["classify"]).Trim();
                            }
                        }
                        string r = "update mark_entry set gradePoints='" + totalOUTOff10s + "' ,cp='" + creditPoints + "',gradePointsWeight='" + gradePoint + "',grade='" + gradeVal + "',weightageMarks100='" + weightageMarks + "',totalOutOff100='" + totalOUTOff100s + "' where entry_code='" + entryCode + "' and subject_no='" + subjectNo + "' and roll_no='" + studentRollNo + "'";
                        int result = da.update_method_wo_parameter(qry, "text");
                    }
                    collegeCode = Convert.ToString(dtMarks.Rows[0]["college_code"]).Trim();
                    batchYear = Convert.ToString(dtMarks.Rows[0]["Batch_Year"]).Trim();
                    degreeCode = Convert.ToString(dtMarks.Rows[0]["degree_code"]).Trim();
                    subjectCode = Convert.ToString(dtMarks.Rows[0]["subject_code"]).Trim();
                    subjectNo = Convert.ToString(dtMarks.Rows[0]["subject_no"]).Trim();
                    examCode = Convert.ToString(dtMarks.Rows[0]["exam_code"]).Trim();
                    subjectSem = Convert.ToString(dtMarks.Rows[0]["SubjectSem"]).Trim();
                    partType = Convert.ToString(dtMarks.Rows[0]["Part_Type"]).Trim();
                    entryCode = Convert.ToString(dtMarks.Rows[0]["entry_code"]).Trim();
                    sumOfGP = Math.Round(totalGradePoints, 2, MidpointRounding.AwayFromZero);
                    sumOfWeitageMarks = Math.Round(totalWeightageMarks, 2, MidpointRounding.AwayFromZero);
                    totalEarnedCredits = totalCredits;
                    maxTotal = Math.Round(totalMaxMarks, 0, MidpointRounding.AwayFromZero);
                    totalMarks = Math.Round(totalSecuredMarks, 0, MidpointRounding.AwayFromZero);
                    if (actualTotalMaxMarks > 0 && actualTotalSecuredMarks >= 0)
                    {
                        actualaverageMarks = actualTotalSecuredMarks / actualTotalMaxMarks * 100;
                        actualaverageMarks = Math.Round(actualaverageMarks, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalMaxMarks > 0 && totalSecuredMarks >= 0)
                    {
                        averageMarks = totalSecuredMarks / totalMaxMarks * 100;
                        averageMarks = Math.Round(averageMarks, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalCredits > 0 && totalGradePoints > 0)
                    {
                        gpa = totalGradePoints / totalCredits;
                        gpa = Math.Round(gpa, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalCredits > 0 && totalWeightageMarks > 0)
                    {
                        wam = totalWeightageMarks / totalCredits;
                        wam = Math.Round(wam, 2, MidpointRounding.AwayFromZero);
                    }
                    avg = Math.Round(averageMarks, 2, MidpointRounding.AwayFromZero);
                    gpaClassify = string.Empty;
                    gpaGrade = string.Empty;
                    //if (dtGrades.Rows.Count > 0)
                    //{
                    //    DataView dvGrade = new DataView();
                    //    double totMarks = Math.Round(gpa, 1, MidpointRounding.AwayFromZero);
                    //    dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + subjectSem + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                    //    dvGrade = dtGrades.DefaultView;
                    //    if (dvGrade.Count == 0)//and frompoint<='6' and topoint>='6'
                    //    {
                    //        dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                    //        dvGrade = dtGrades.DefaultView;
                    //    }
                    //    if (dvGrade.Count > 0)
                    //    {
                    //dvGrade.Sort = "Frange desc";
                    //        gpaGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                    //        gpaClassify = Convert.ToString(dvGrade[0]["classify"]).Trim();
                    //    }
                    //}
                    if (failCount != 0 && gpa > 6)//109001
                    {
                        if (dtClassify.Rows.Count > 0)
                        {
                            DataView dvClassify = new DataView();
                            double totMarks = Math.Round(gpa, 1, MidpointRounding.AwayFromZero);
                            string batchYearNew = "";
                            dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYear + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                            dvClassify = dtClassify.DefaultView;
                            dvClassify.Sort = "frompoint desc";
                            if (dvClassify.Count == 0)
                            {
                                dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYearNew + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                                dvClassify = dtClassify.DefaultView;
                                dvClassify.Sort = "frompoint desc";
                            }
                            if (dvClassify.Count > 0)
                            {
                                gpaGrade = Convert.ToString(dvClassify[0]["grade"]).Trim();
                                gpaClassify = "First Class";// Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                            else
                            {
                                gpaGrade = "A";// Convert.ToString(dvClassify[0]["grade"]).Trim();
                                gpaClassify = "First Class";// Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                        }
                    }
                    else
                    {
                        if (dtClassify.Rows.Count > 0)
                        {
                            DataView dvClassify = new DataView();
                            double totMarks = Math.Round(gpa, 1, MidpointRounding.AwayFromZero);
                            string batchYearNew = "";
                            dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYear + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                            dvClassify = dtClassify.DefaultView;
                            dvClassify.Sort = "frompoint desc";
                            if (dvClassify.Count == 0)
                            {
                                dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYearNew + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                                dvClassify = dtClassify.DefaultView;
                                dvClassify.Sort = "frompoint desc";
                            }
                            if (dvClassify.Count > 0)
                            {
                                dvClassify.Sort = "frompoint desc";
                                gpaGrade = Convert.ToString(dvClassify[0]["grade"]).Trim();
                                gpaClassify = Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                        }
                    }
                }
                if (dtCgpaMarks.Rows.Count > 0)
                {
                    hasCalculated = true;
                    totalCredits = 0;
                    actualTotalSecuredMarks = 0;
                    actualTotalMaxMarks = 0;
                    actualaverageMarks = 0;
                    totalSecuredMarks = 0;
                    totalMaxMarks = 0;
                    averageMarks = 0;
                    gradePoint = 0;
                    totalGradePoints = 0;
                    weightageMarks = 0;
                    totalWeightageMarks = 0;
                    string collegeCode = string.Empty;
                    string batchYear = string.Empty;
                    string degreeCode = string.Empty;
                    string subjectCode = string.Empty;
                    string subjectNo = string.Empty;
                    string examCode = string.Empty;
                    string subjectSem = string.Empty;
                    string partType = string.Empty;
                    string entryCode = string.Empty;
                    foreach (DataRow drMarks in dtCgpaMarks.Rows)
                    {
                        collegeCode = Convert.ToString(drMarks["college_code"]).Trim();
                        batchYear = Convert.ToString(drMarks["Batch_Year"]).Trim();
                        degreeCode = Convert.ToString(drMarks["degree_code"]).Trim();
                        subjectCode = Convert.ToString(drMarks["subject_code"]).Trim();
                        subjectNo = Convert.ToString(drMarks["subject_no"]).Trim();
                        examCode = Convert.ToString(drMarks["exam_code"]).Trim();
                        subjectSem = Convert.ToString(drMarks["SubjectSem"]).Trim();
                        partType = Convert.ToString(drMarks["Part_Type"]).Trim();
                        entryCode = Convert.ToString(drMarks["entry_code"]).Trim();
                        string studAppNo = Convert.ToString(drMarks["App_No"]).Trim();
                        string creditPoint = Convert.ToString(drMarks["credit_points"]).Trim();
                        string totalOUTOff100 = Convert.ToString(drMarks["TotalOutOf100"]).Trim();
                        string totalOUTOff10 = Convert.ToString(drMarks["TotalOutOf10"]).Trim();
                        string actualTotal = Convert.ToString(drMarks["Total"]).Trim();
                        string actualMaxTotal = Convert.ToString(drMarks["MaxTotal"]).Trim();
                        string studentRollNo = Convert.ToString(drMarks["Roll_No"]).Trim();
                        string gradeVal = string.Empty;
                        string classifyVal = string.Empty;
                        double creditPoints = 0;
                        double.TryParse(creditPoint, out creditPoints);
                        double totalOUTOff100s = 0;
                        double.TryParse(totalOUTOff100, out totalOUTOff100s);
                        double totalOUTOff10s = 0;
                        double.TryParse(totalOUTOff10, out totalOUTOff10s);
                        double actualTotals = 0;
                        double.TryParse(actualTotal, out actualTotals);
                        double actualMaxTotals = 0;
                        double.TryParse(actualMaxTotal, out actualMaxTotals);
                        totalCredits += creditPoints;
                        totalSecuredMarks += totalOUTOff100s;
                        totalMaxMarks += 100;
                        gradePoint = 0;
                        gradePoint = totalOUTOff10s * creditPoints;
                        totalGradePoints += gradePoint;
                        weightageMarks = 0;
                        weightageMarks = totalOUTOff100s * creditPoints;
                        totalWeightageMarks += weightageMarks;
                        actualTotalSecuredMarks += actualTotals;
                        actualTotalMaxMarks += actualMaxTotals;
                        if (dtGrades.Rows.Count > 0)
                        {
                            DataView dvGrade = new DataView();
                            double totMarks = Math.Round(totalOUTOff100s, 0, MidpointRounding.AwayFromZero);
                            dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + subjectSem + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                            //and Frange>='" + totMarks + "' and Trange<='" + totMarks + "'";//and frompoint<='6' and topoint>='6'
                            dvGrade = dtGrades.DefaultView;
                            if (dvGrade.Count == 0)
                            {
                                dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                                dvGrade = dtGrades.DefaultView;
                            }
                            if (dvGrade.Count > 0)
                            {
                                dvGrade.Sort = "Frange desc";
                                gradeVal = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                classifyVal = Convert.ToString(dvGrade[0]["classify"]).Trim();
                            }
                        }
                        string qry = "update mark_entry set gradePoints='" + totalOUTOff10s + "' ,cp='" + creditPoints + "',gradePointsWeight='" + gradePoint + "',grade='" + gradeVal + "',weightageMarks100='" + weightageMarks + "',totalOutOff100='" + totalOUTOff100s + "' where entry_code='" + entryCode + "' and subject_no='" + subjectNo + "' and roll_no='" + studentRollNo + "'";
                        int result = da.update_method_wo_parameter(qry, "text");
                    }
                    collegeCode = Convert.ToString(dtCgpaMarks.Rows[0]["college_code"]).Trim();
                    batchYear = Convert.ToString(dtCgpaMarks.Rows[0]["Batch_Year"]).Trim();
                    degreeCode = Convert.ToString(dtCgpaMarks.Rows[0]["degree_code"]).Trim();
                    subjectCode = Convert.ToString(dtCgpaMarks.Rows[0]["subject_code"]).Trim();
                    subjectNo = Convert.ToString(dtCgpaMarks.Rows[0]["subject_no"]).Trim();
                    examCode = Convert.ToString(dtCgpaMarks.Rows[0]["exam_code"]).Trim();
                    subjectSem = Convert.ToString(dtCgpaMarks.Rows[0]["SubjectSem"]).Trim();
                    partType = Convert.ToString(dtCgpaMarks.Rows[0]["Part_Type"]).Trim();
                    entryCode = Convert.ToString(dtCgpaMarks.Rows[0]["entry_code"]).Trim();
                    if (actualTotalMaxMarks > 0 && actualTotalSecuredMarks >= 0)
                    {
                        actualaverageMarks = actualTotalSecuredMarks / actualTotalMaxMarks * 100;
                        actualaverageMarks = Math.Round(actualaverageMarks, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalMaxMarks > 0 && totalSecuredMarks >= 0)
                    {
                        averageMarks = totalSecuredMarks / totalMaxMarks * 100;
                        averageMarks = Math.Round(averageMarks, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalCredits > 0 && totalGradePoints > 0)
                    {
                        cgpa = totalGradePoints / totalCredits;
                        cgpa = Math.Round(cgpa, 2, MidpointRounding.AwayFromZero);
                    }
                    if (totalCredits > 0 && totalWeightageMarks > 0)
                    {
                        cwam = totalWeightageMarks / totalCredits;
                        cwam = Math.Round(cwam, 2, MidpointRounding.AwayFromZero);
                    }
                    cgpaClassify = string.Empty;
                    cgpaGrade = string.Empty;
                    //if (dtGrades.Rows.Count > 0)
                    //{
                    //    DataView dvGrade = new DataView();
                    //    double totMarks = Math.Round(gpa, 1, MidpointRounding.AwayFromZero);
                    //    dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + subjectSem + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";//and frompoint<='6' and topoint>='6'
                    //    dvGrade = dtGrades.DefaultView;
                    //    if (dvGrade.Count == 0)
                    //    {
                    //        dtGrades.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and Frange<='" + totMarks + "' and Trange>='" + totMarks + "'";
                    //        dvGrade = dtGrades.DefaultView;
                    //    }
                    //    if (dvGrade.Count > 0)
                    //    {
                    //        gpaGrade.Sort = "Frange desc";
                    //        gpaGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                    //        gpaClassify = Convert.ToString(dvGrade[0]["classify"]).Trim();
                    //    }
                    //}
                    if (failCount != 0 && cgpa > 6)
                    {
                        if (dtClassify.Rows.Count > 0)
                        {
                            DataView dvClassify = new DataView();
                            double totMarks = Math.Round(cgpa, 1, MidpointRounding.AwayFromZero);
                            string batchYearNew = "";
                            dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYear + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                            dvClassify = dtClassify.DefaultView;
                            dvClassify.Sort = "frompoint desc";
                            if (dvClassify.Count == 0)
                            {
                                dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYearNew + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                                dvClassify = dtClassify.DefaultView;
                                dvClassify.Sort = "frompoint desc";
                            }
                            if (dvClassify.Count > 0)
                            {
                                dvClassify.Sort = "frompoint desc";
                                cgpaGrade = Convert.ToString(dvClassify[0]["grade"]).Trim();
                                cgpaClassify = "First Class"; //Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                            else
                            {
                                cgpaGrade = "A";// Convert.ToString(dvClassify[0]["grade"]).Trim();
                                cgpaClassify = "First Class"; //Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                        }
                    }
                    else
                    {
                        if (dtClassify.Rows.Count > 0)
                        {
                            DataView dvClassify = new DataView();
                            double totMarks = Math.Round(cgpa, 1, MidpointRounding.AwayFromZero);
                            string batchYearNew = "";
                            dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYear + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                            dvClassify = dtClassify.DefaultView;
                            dvClassify.Sort = "frompoint desc";
                            if (dvClassify.Count == 0)
                            {
                                dtClassify.DefaultView.RowFilter = "collegecode='" + collegeCode + "' and batch_year='" + batchYearNew + "' and edu_level='" + eduLevel + "' and frompoint<='" + totMarks + "' and topoint>='" + totMarks + "'";
                                dvClassify = dtClassify.DefaultView;
                                dvClassify.Sort = "frompoint desc";
                            }
                            if (dvClassify.Count > 0)
                            {
                                dvClassify.Sort = "frompoint desc";
                                cgpaGrade = Convert.ToString(dvClassify[0]["grade"]).Trim();
                                cgpaClassify = Convert.ToString(dvClassify[0]["classification"]).Trim();
                            }
                        }
                    }
                }
            }
            return hasCalculated;
        }
        catch (Exception ex)
        { return false; }
    }

    #endregion


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRegNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Reg_No  from Registration where Reg_No like '" + prefixText + "%' and DelFlag=0 and Exam_Flag <>'Debar' order by Reg_No  ";
        name = ws.Getname(query);
        return name;
    }
}