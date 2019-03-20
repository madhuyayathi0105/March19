#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wc = System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Configuration;

#endregion Namespace Declaration

public partial class MarkMod_CAMMarksBoostUp : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    string collegeCodes = string.Empty;
    string collegeNames = string.Empty;
    string courseIds = string.Empty;
    string courseNames = string.Empty;
    string batchYears = string.Empty;
    string degreeCodes = string.Empty;
    string departmentNames = string.Empty;
    string semesters = string.Empty;
    string sections = string.Empty;
    string subjectTypes = string.Empty;
    string subjectNames = string.Empty;
    string subjectNos = string.Empty;
    string subjectCodes = string.Empty;

    string testNames = string.Empty;
    string testNos = string.Empty;

    string subjectNo = string.Empty;
    string subjectCode = string.Empty;
    string testName = string.Empty;
    string testNo = string.Empty;

    string qry = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qryCollegeName = string.Empty;
    string qryCourseId = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryTestNo = string.Empty;
    string qryTestName = string.Empty;
    string qrySubjectCode = string.Empty;
    string qrySubjectNo = string.Empty;


    //added by rajasekar 13/11/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    string tagcol = "";
    string temp = "";
    ArrayList headercoltag = new ArrayList();
    ArrayList colofvisfalse = new ArrayList();
    int colspanstart = 0;
    //====================================//

    bool isSchool = false;

    int selected = 0;

    Institution institute;

    #endregion Field Declaration

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
            if (!IsPostBack)
            {
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;
                setLabelText();
                Bindcollege();
                BindRightsBaseBatch();
                BindDegree();
                BindBranch();
                BindSemester();
                BindRightsBasedSectionDetail();
                BindTest();
                BindSubject();
                Init_Spread(0);
            }
        }
        catch (ThreadAbortException tex)
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCodes = string.Empty;
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and college_code in(" + collegeCodes + ")";
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollege))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights where batch_year<>'' " + qryCollege + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = dsBatch;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.college_code in(" + collegeCodes + ")  order by r.Batch_Year desc";
                    ds.Clear();
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
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
            qryCollege = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and dg.college_code in(" + collegeCodes + ")";
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                ds = da.select_method_wo_parameter("select distinct c.course_id,c.course_name,c.Priority,case when c.Priority is null then c.Course_Id else c.Priority end OrderBy from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qryCollege + columnfield + "  order by case when c.Priority is null then c.Course_Id else c.Priority end", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataBind();
                    ddlDegree.SelectedIndex = 0;
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

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlBranch.Items.Clear();
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
            qryCollege = string.Empty;

            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and dg.college_code in(" + collegeCodes + ")";
            }
            if (ddlDegree.Items.Count > 0)
            {
                courseIds = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseIds))
                        {
                            courseIds = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseIds += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCourseId))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCourseId + qryCollege + columnfield + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
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

    public void BindSemester()
    {
        try
        {
            ds.Clear();
            ddlSem.Items.Clear();
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollege = string.Empty;
            collegeCodes = string.Empty;
            qryBatchYear = string.Empty;
            batchYears = string.Empty;
            courseIds = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and college_code in(" + collegeCodes + ")";
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and Batch_year in(" + batchYears + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatchYear))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollege + qryBatchYear + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSem.SelectedIndex = 0;
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
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSem.SelectedIndex = 0;
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

    public void BindSections()
    {
        try
        {
            ddlSec.Items.Clear();
            ht.Clear();
            ht.Add("batch_year", ddlBatch.SelectedValue.ToString());
            ht.Add("degree_code", ddlBranch.SelectedValue);
            ds.Clear();
            ds = da.select_method("bind_sec", ht, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count5 = ds.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    ddlSec.DataSource = ds;
                    ddlSec.DataTextField = "sections";
                    ddlSec.DataValueField = "sections";
                    ddlSec.DataBind();
                    ddlSec.Items.Insert(0, "All");
                    ddlSec.Enabled = true;
                }
                else
                {
                    ddlSec.Enabled = false;
                }
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBasedSectionDetail()
    {
        try
        {
            batchYears = string.Empty;
            collegeCodes = string.Empty;
            degreeCodes = string.Empty;
            string sections = string.Empty;

            qrySection = string.Empty;
            qryCollege = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and college_code in(" + collegeCodes + ")";
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and batch_year in(" + batchYears + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                qrySection = da.GetFunctionv("select distinct sections from tbl_attendance_rights where batch_year<>'' " + qryUserOrGroupCode + qryCollege + qryBatchYear).Trim();
            }
            if (!string.IsNullOrEmpty(qrySection.Trim()))
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
                qrySection = " and isnull(ltrim(rtrim(sections)),'') in(" + sections + ") ";
            }
            else
            {
                qrySection = string.Empty;
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryBatchYear))// && !string.IsNullOrEmpty(qrySection)
            {
                string sqlnew = "select distinct case when isnull(ltrim(rtrim(sections)),'')='' then 'Empty' else isnull(ltrim(rtrim(sections)),'') end sections, isnull(ltrim(rtrim(sections)),'') SecValues from registration where isnull(ltrim(rtrim(sections)),'')<>'-1' and isnull(ltrim(rtrim(sections)),'')<>' ' and delflag=0 and exam_flag<>'Debar' " + qryCollege + qryDegreeCode + qryBatchYear + qrySection + " order by SecValues";
                ds = da.select_method_wo_parameter(sqlnew, "Text");
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindTest()
    {
        try
        {
            qryCollege = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;

            collegeCodes = string.Empty;
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            ddlTest.Items.Clear();
            ds.Clear();
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and college_code in(" + collegeCodes + ")";
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and sm.batch_year in(" + batchYears + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSec.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and et.sections in(" + sections + ")";
                }
            }

            if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(semesters) && !string.IsNullOrEmpty(qrySemester))
            {
                qry = "select distinct c.Criteria_no,c.criteria from CriteriaForInternal c,syllabus_master sm,Exam_type et where c.syll_code=sm.syll_code and c.Criteria_no=et.criteria_no " + qryBatchYear + qryDegreeCode + qrySemester + qrySection + "order by c.Criteria_no,c.criteria";
                ds = da.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "Criteria_no";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindSubject()
    {
        try
        {
            qryCollege = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;
            qryTestNo = string.Empty;

            collegeCodes = string.Empty;
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            testNos = string.Empty;
            ddlSubejct.Items.Clear();
            cblSubject.Items.Clear();
            chkSubject.Checked = false;
            txtSubject.Text = "--Select--";
            txtSubject.Enabled = false;
            ddlSubejct.Enabled = false;
            ds.Clear();
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and college_code in(" + collegeCodes + ")";
            }
            if (ddlBatch.Items.Count > 0)
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
                    qryBatchYear = " and sm.batch_year in(" + batchYears + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSec.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and et.sections in(" + sections + ")";
                }
            }
            if (ddlTest.Items.Count > 0)
            {
                testNos = string.Empty;
                foreach (ListItem li in ddlTest.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(testNos))
                        {
                            testNos = "'" + li.Value + "'";
                        }
                        else
                        {
                            testNos += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(testNos))
                {
                    qryTestNo = " and c.criteria_no in(" + testNos + ")";
                }
            }
            //if (cblSubject.Items.Count > 0)
            //{
            //    subjectNos = getCblSelectedValue(cblSubject);
            //    if (!string.IsNullOrEmpty(subjectNos))
            //    {
            //        qrySubjectNo = " and ed.coll_code in(" + subjectNos + ")";
            //    }
            //}
            if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(semesters) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryTestNo))
            {
                if (Session["Staff_Code"] == null || Convert.ToString(Session["Staff_Code"]).Trim() == "")
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from CriteriaForInternal c, Exam_type et,subject s,syllabus_master sm where s.syll_code=sm.syll_code and sm.syll_code=c.syll_code and s.syll_code=c.syll_code and et.criteria_no=c.Criteria_no and et.subject_no=s.subject_no " + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryTestNo + " order by s.subject_no";
                }
                else if (Session["Staff_Code"] != null && Convert.ToString(Session["Staff_Code"]).Trim() != "")
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from CriteriaForInternal c,Exam_type et,subject s,syllabus_master sm,staff_selector ss where s.syll_code=sm.syll_code and sm.syll_code=c.syll_code and s.syll_code=c.syll_code and et.criteria_no=c.Criteria_no and et.subject_no=s.subject_no and ss.subject_no=s.subject_no and ss.subject_no=et.subject_no and ss.Sections=et.sections and ss.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryTestNo + " order by s.subject_no";
                }
                if (!string.IsNullOrEmpty(qry))
                {
                    ds = da.select_method_wo_parameter(qry, "text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ddlTest.DataSource = ds;
                //ddlTest.DataTextField = "subject_name";
                //ddlTest.DataValueField = "subject_no";
                //ddlTest.DataBind();
                //ddlTest.SelectedIndex = 0;

                cblSubject.DataSource = ds;
                cblSubject.DataTextField = "subject_name";
                cblSubject.DataValueField = "subject_no";
                cblSubject.DataBind();
                checkBoxListselectOrDeselect(cblSubject, true);
                CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
                txtSubject.Enabled = true;

                ddlSubejct.DataSource = ds;
                ddlSubejct.DataTextField = "subject_name";
                ddlSubejct.DataValueField = "subject_no";
                ddlSubejct.DataBind();
                ddlSubejct.Enabled = true;
                ddlSubejct.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public int Init_Spread(int type = 0)
    {
        try
        {
            

            

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            int colu = 0;
           

            DataSet dsSettings = new DataSet();
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method_wo_parameter(Master1, "Text");
            }
            bool isRollVisible = ColumnHeaderVisiblity(0, dsSettings);
            bool isRegVisible = ColumnHeaderVisiblity(1, dsSettings);
            bool isAdmitNoVisible = ColumnHeaderVisiblity(2, dsSettings);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3, dsSettings);
            if (type == 0)
            {
                

                dtl.Columns.Add("S.No", typeof(string));
                dtl.Rows[0][colu] = "S.No";
                colu++;
                tagcol += "0^col0$";
                colofvisfalse.Add("true");
                headercoltag.Add("0");

               

                
                dtl.Columns.Add(lblDegree.Text, typeof(string));
                dtl.Rows[0][colu] = lblDegree.Text;
                colu++;
                tagcol += "1^col1$";
                colofvisfalse.Add("false");
                headercoltag.Add("1");
               


                

                dtl.Columns.Add(lblBranch.Text, typeof(string));
                dtl.Rows[0][colu] = lblBranch.Text;
                colu++;
                tagcol += "2^col2$";
                colofvisfalse.Add("false");
                headercoltag.Add("2");

                

                dtl.Columns.Add("Roll No", typeof(string));
                dtl.Rows[0][colu] = "Roll No";
                colu++;
                tagcol += "3^col3$";
                colofvisfalse.Add(Convert.ToString(isRollVisible));
                headercoltag.Add("3");

                

                dtl.Columns.Add("Reg No", typeof(string));
                dtl.Rows[0][colu] = "Reg No";
                colu++;
                tagcol += "4^col4$";
                colofvisfalse.Add(Convert.ToString(isRegVisible));
                headercoltag.Add("4");

                

                dtl.Columns.Add("Admission No", typeof(string));
                dtl.Rows[0][colu] = "Admission No";
                colu++;
                tagcol += "5^col5$";
                colofvisfalse.Add(Convert.ToString(isAdmitNoVisible));
                headercoltag.Add("5");

                

                dtl.Columns.Add("Student Type", typeof(string));
                dtl.Rows[0][colu] = "Student Type";
                colu++;
                tagcol += "6^col6$";
                colofvisfalse.Add(Convert.ToString(isStudentTypeVisible));
                headercoltag.Add("6");

                

                dtl.Columns.Add("Student Name", typeof(string));
                dtl.Rows[0][colu] = "Student Name";
                colu++;
                tagcol += "7^col7$";
                colofvisfalse.Add(true);
                headercoltag.Add("7");

                

                dtl.Columns.Add(lblSem.Text, typeof(string));
                dtl.Rows[0][colu] = lblSem.Text;
                colu++;
                tagcol += "8^col8$";
                colofvisfalse.Add(false);
                headercoltag.Add("8");

                

                dtl.Columns.Add(lblSec.Text, typeof(string));
                dtl.Rows[0][colu] = lblSec.Text;
                colu++;
                tagcol += "9^col9$";
                colofvisfalse.Add(true);
                headercoltag.Add("9");

                

                dtl.Columns.Add("Gender", typeof(string));
                dtl.Rows[0][colu] = "Gender";
                colu++;
                tagcol += "10^col10$";
                colofvisfalse.Add(false);
                headercoltag.Add("10");

                

                dtl.Columns.Add("Mode", typeof(string));
                dtl.Rows[0][colu] = "Mode";
                colu++;
                tagcol += "11^col11$";
                colofvisfalse.Add(false);
                headercoltag.Add("11");

            }
            else
            {
               

                dtl.Columns.Add("S.No", typeof(string));
                dtl.Rows[0][colu] = "S.No";
                colu++;
                tagcol += "000^col0$";
                colofvisfalse.Add(true);
                headercoltag.Add("000");

                

                dtl.Columns.Add("Roll No", typeof(string));
                dtl.Rows[0][colu] = "Roll No";
                colu++;
                tagcol += "000^col1$";
                colofvisfalse.Add(Convert.ToString(isRollVisible));
                headercoltag.Add("000");

                

                dtl.Columns.Add("Reg No", typeof(string));
                dtl.Rows[0][colu] = "Reg No";
                colu++;
                tagcol += "000^col2$";
                colofvisfalse.Add(Convert.ToString(isRegVisible));
                headercoltag.Add("000");

                

                dtl.Columns.Add("Admission No", typeof(string));
                dtl.Rows[0][colu] = "Admission No";
                colu++;
                tagcol += "000^col3$";
                colofvisfalse.Add(Convert.ToString(isAdmitNoVisible));
                headercoltag.Add("000");

                

                dtl.Columns.Add("Student Name", typeof(string));
                dtl.Rows[0][colu] = "Student Name";
                colu++;
                tagcol += "000^col4$";
                colofvisfalse.Add(true);
                headercoltag.Add("000");

                

                dtl.Columns.Add(lblSem.Text, typeof(string));
                dtl.Rows[0][colu] = lblSem.Text;
                colu++;
                tagcol += "000^col5$";
                colofvisfalse.Add(true);
                headercoltag.Add("000");

            }
            if (isRollVisible)
            {
                return 3;
            }
            else if (isRegVisible)
            {
                return 4;
            }
            else if (isAdmitNoVisible)
            {
                return 5;
            }
            else if (isStudentTypeVisible)
            {
                return 6;
            }
            else
            {
                return 7;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return -1;
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
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSemester();
            BindRightsBasedSectionDetail();
            BindTest();
            BindSubject();

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
            BindSemester();
            BindRightsBasedSectionDetail();
            BindTest();
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBranch();
            BindSemester();
            BindRightsBasedSectionDetail();
            BindTest();
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSemester();
            BindRightsBasedSectionDetail();
            BindTest();
            BindSubject();
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
            BindRightsBasedSectionDetail();
            BindTest();
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindTest();
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSubject();
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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

    #region Button Click

    #region Save Moderation

    protected void btnSaveModeration_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSaved = false;

            if (grdover.Rows.Count > 0)
            {
                for (int row = 2; row < grdover.Rows.Count; row++)
                {
                    
                    string rollNo = Convert.ToString(grdover.Rows[row].Cells[3].Text).Trim();
                    string examCode = string.Empty;
                    string markModeration = string.Empty;
                    bool hasModeration = false;


                    string tagvaluesgridcol = Convert.ToString(grdover.Rows[row].Cells[grdover.HeaderRow.Cells.Count-1].Text).Trim();
                    
                        string[] tagvalues = Convert.ToString(tagvaluesgridcol).Trim().Split('$');


                        for (int col = 12; col < grdover.HeaderRow.Cells.Count-1; col += 2)
                    {
                        
                        markModeration = Convert.ToString(grdover.Rows[row].Cells[col + 1].Text).Trim();

                        

                        if (tagvalues[0] != "&nbsp;")
                        {
                            string[] tagvaluesexamcode = Convert.ToString(tagvalues[col]).Trim().Split('^');

                            if (tagvaluesexamcode.Length > 0)
                            {
                                examCode = tagvaluesexamcode[0].Trim();
                            }
                        }
                        else
                            examCode = "";

                        string moderation = "";

                        if (tagvalues[0] != "&nbsp;")
                        {
                            string[] tagvaluesmoderation = Convert.ToString(tagvalues[col + 1]).Trim().Split('^');

                            if (tagvaluesmoderation.Length > 0)
                            {
                                moderation = tagvaluesmoderation[0].Trim();
                            }
                        }
                        else
                            moderation = "";

                        bool.TryParse(moderation.Trim(), out hasModeration);
                        if (hasModeration)
                        {
                            int update = 0;
                            qry = "update result set moderationMark='" + markModeration + "' where roll_no='" + rollNo + "' and exam_code='" + examCode + "'";
                            update = da.update_method_wo_parameter(qry, "text");
                            if (update != 0)
                            {
                                isSaved = true;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (isSaved)
            {
                lblAlertMsg.Text = "Moderation Mark Saved Successfully";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Moderation Mark Not Saved";
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

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;

            collegeCodes = string.Empty;
            collegeNames = string.Empty;
            courseIds = string.Empty;
            courseNames = string.Empty;
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            departmentNames = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            subjectTypes = string.Empty;
            subjectNames = string.Empty;
            subjectNos = string.Empty;
            subjectCodes = string.Empty;

            testNames = string.Empty;
            testNos = string.Empty;

            subjectNo = string.Empty;
            subjectCode = string.Empty;
            testName = string.Empty;
            testNo = string.Empty;

            qry = string.Empty;
            qryUserOrGroupCode = string.Empty;
            qryCollege = string.Empty;
            qryCollegeName = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;
            qryTestNo = string.Empty;
            qryTestName = string.Empty;
            qrySubjectCode = string.Empty;
            qrySubjectNo = string.Empty;

            DataSet dsStudentDetails = new DataSet();
            DataSet dsDegreeDetails = new DataSet();

            string qryFromRange = string.Empty;
            string qryToRange = string.Empty;
            string fromRange = Convert.ToString(txtFromRange.Text).Trim();
            string toRange = Convert.ToString(txtToRange.Text).Trim();

            double fromMark = 0;
            double toMark = 0;

            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and r.college_code in(" + collegeCodes + ")";
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
                    qryBatchYear = " and sm.batch_year in(" + batchYears + ")";
                }
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
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
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSec.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.Sections,''))) in(" + sections + ")";
                }
            }
            else
            {
                sections = string.Empty;
                qrySection = string.Empty;
            }
            if (ddlTest.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                testNos = string.Empty;
                foreach (ListItem li in ddlTest.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(testNos))
                        {
                            testNos = "'" + li.Value + "'";
                        }
                        else
                        {
                            testNos += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(testNos))
                {
                    qryTestNo = " and et.criteria_no in(" + testNos + ")";
                }
            }
            if (cblSubject.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSubjects.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                subjectNos = getCblSelectedValue(cblSubject);
                if (!string.IsNullOrEmpty(subjectNos))
                {
                    qrySubjectNo = " and et.subject_no in(" + subjectNos + ")";
                }
            }

            if (!string.IsNullOrEmpty(fromRange))
            {
                bool isValidNumber = double.TryParse(fromRange.Trim(), out fromMark);
                if (isValidNumber)
                {
                    //qryFromRange = " and re.marks_obtained>='" + fromMark + "'";
                    qryFromRange = " and Marks>='" + fromMark + "'";
                }
            }
            if (!string.IsNullOrEmpty(toRange))
            {
                bool isValidNumber = double.TryParse(toRange.Trim(), out toMark);
                if (isValidNumber)
                {
                    //qryToRange = " and re.marks_obtained<='" + toMark + "'";
                    qryToRange = " and Marks<='" + toMark + "'";
                }
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryTestNo) && !string.IsNullOrEmpty(qrySubjectNo))
            {
                DataTable dtDistinctSubjects = new DataTable();
                DataTable dtDistinctStudents = new DataTable();

                string serialno = da.GetFunction("select LinkValue from inssettings where college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and linkname='Student Attendance'");
                string orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
                orderBySetting = orderBySetting.Trim();
                string orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no";
                if (serialno.Trim() == "1")
                {
                    orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.serialno";
                }
                else
                {
                    switch (orderBySetting)
                    {
                        case "0":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no";
                            break;
                        case "1":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.Reg_No";
                            break;
                        case "2":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.Stud_Name";
                            break;
                        case "0,1,2":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no,r.Reg_No,r.stud_name";
                            break;
                        case "0,1":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no,r.Reg_No";
                            break;
                        case "1,2":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.Reg_No,r.Stud_Name";
                            break;
                        case "0,2":
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no,r.Stud_Name";
                            break;
                        default:
                            orderBy = "ORDER BY r.college_code,r.Batch_Year desc,r.degree_code,r.sections,r.roll_no";
                            break;
                    }
                }

                qry = "select r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.App_No,r.serialno,r.Roll_Admit,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,case when a.sex =0 then 'Male' when a.sex=1 then 'Female' else 'Transgender' end as sex,case when r.mode=1 then 'Regular' when r.mode=2 then 'Transfer' when r.mode=3 then 'Lateral' end as Mode,s.subject_code,s.subject_name,s.subject_no,s.acronym as SubjectAcronymn,re.marks_obtained as Marks,re.exam_code,moderationMark from Registration r,applyn a,Result re,Exam_type et,syllabus_master sm,subject s where r.Roll_No=re.roll_no and r.App_No=a.app_no and et.exam_code=re.exam_code and sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and s.subject_no=et.subject_no " + qryCollege + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryTestNo + qrySubjectNo + " " + orderBy;//order by r.college_code,r.Batch_Year desc,r.degree_code,r.sections
                dsStudentDetails = da.select_method_wo_parameter(qry, "text");//+ qryFromRange + qryToRange +

                qry = "select dg.Degree_Code,dg.Duration,c.Course_Id,dt.Dept_Code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.degree_code in(" + degreeCodes + ")";
                dsDegreeDetails = da.select_method_wo_parameter(qry, "text");

                if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
                {
                    int spanColumn = Init_Spread(0);
                    
                    dsStudentDetails.Tables[0].DefaultView.RowFilter = "roll_no<>'' " + qryFromRange + qryToRange;
                    dtDistinctSubjects = dsStudentDetails.Tables[0].DefaultView.ToTable(true, "subject_code", "subject_name", "subject_no", "SubjectAcronymn");
                    dtDistinctStudents = dsStudentDetails.Tables[0].DefaultView.ToTable(true, "college_code", "Batch_Year", "degree_code", "Current_Semester", "Sections", "App_No", "serialno", "Roll_Admit", "Roll_No", "Reg_No", "Stud_Name", "Stud_Type", "sex", "Mode");
                    string minMarks = string.Empty;
                    string maxMarks = string.Empty;
                    foreach (DataRow drSubjects in dtDistinctSubjects.Rows)
                    {
                        string subjectName = Convert.ToString(drSubjects["subject_name"]).Trim();
                        string subjectNoNew = Convert.ToString(drSubjects["subject_no"]).Trim();
                        string subjectCodeNew = Convert.ToString(drSubjects["subject_code"]).Trim();
                        string subjectAcronymn = Convert.ToString(drSubjects["SubjectAcronymn"]).Trim();
                        DataTable dtMinMax = new DataTable();
                        dsStudentDetails.Tables[0].DefaultView.RowFilter = "roll_no<>'' and Marks>=0 and subject_no='" + subjectNoNew + "'";
                        dtMinMax = dsStudentDetails.Tables[0].DefaultView.ToTable();
                        double minMark = 0;
                        double maxMark = 0;
                        object minimumMarks = dtMinMax.Compute("MIN(Marks)", "roll_no<>'' and Marks>=0 and subject_no='" + subjectNoNew + "'");
                        object maximumMarks = dtMinMax.Compute("MAX(Marks)", "roll_no<>'' and Marks>=0 and subject_no='" + subjectNoNew + "'");
                        double.TryParse(Convert.ToString(minimumMarks).Trim(), out minMark);
                        double.TryParse(Convert.ToString(maximumMarks).Trim(), out maxMark);

                        

                        dtl.Columns.Add(subjectName + " (" + subjectAcronymn + " )", typeof(string));
                        dtl.Rows[0][dtl.Columns.Count - 1] = subjectName + " (" + subjectAcronymn + " )";
                        //tagcol += ""+subjectNoNew+"^col" + (dtl.Columns.Count-1) + "$";
                        
                        

                       

                        temp += " ";

                        dtl.Rows[1][dtl.Columns.Count - 1] = "Marks Obtained";
                        tagcol += ""+subjectNoNew+"^col" + (dtl.Columns.Count - 1) + "$";
                        colofvisfalse.Add("true");
                        headercoltag.Add(subjectNoNew);

                       

                        dtl.Columns.Add("Moderation Mark" + temp, typeof(string));
                        dtl.Rows[1][dtl.Columns.Count - 1] = "Moderation Mark";
                        tagcol += "" + subjectNoNew + "^col" + (dtl.Columns.Count - 1) + "$";
                        colofvisfalse.Add("true");
                        headercoltag.Add(subjectNoNew);

                        

                    }
                    dtl.Columns.Add(tagcol, typeof(string));
                    dtl.Rows[0][dtl.Columns.Count - 1] = tagcol;
                    colofvisfalse.Add("false");
                    headercoltag.Add("000");
                    int serialNo = 0;
                    foreach (DataRow drStudents in dtDistinctStudents.Rows)
                    {
                        string obtainedMarks = string.Empty;
                        string subjectName = string.Empty;
                        subjectNo = string.Empty;

                        string rollNo = Convert.ToString(drStudents["Roll_No"]).Trim();
                        string appNo = Convert.ToString(drStudents["App_No"]).Trim();
                        string regNo = Convert.ToString(drStudents["Reg_No"]).Trim();
                        string studentName = Convert.ToString(drStudents["Stud_Name"]).Trim();
                        string rollAdmit = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                        string collegeCode = Convert.ToString(drStudents["college_code"]).Trim();
                        string batchYear = Convert.ToString(drStudents["Batch_Year"]).Trim();
                        string degreeCode = Convert.ToString(drStudents["degree_code"]).Trim();
                        string currentSemester = Convert.ToString(drStudents["Current_Semester"]).Trim();
                        string section = Convert.ToString(drStudents["Sections"]).Trim();
                        string serialNos = Convert.ToString(drStudents["serialno"]).Trim();
                        string studentType = Convert.ToString(drStudents["Stud_Type"]).Trim();
                        string gender = Convert.ToString(drStudents["sex"]).Trim();
                        string modeofAdmition = Convert.ToString(drStudents["Mode"]).Trim();

                        string degreeDetails = string.Empty;
                        string courseName = string.Empty;
                        string departmentName = string.Empty;
                        string departmentAcr = string.Empty;
                        string typeName = string.Empty;
                        string eduLevel = string.Empty;
                        string duration = string.Empty;

                        DataView dvDegreeDetails = new DataView();
                        if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                        {
                            dsDegreeDetails.Tables[0].DefaultView.RowFilter = "degree_code='" + degreeCode + "'";
                            dvDegreeDetails = dsDegreeDetails.Tables[0].DefaultView;
                        }
                        if (dvDegreeDetails.Count > 0)
                        {
                            degreeDetails = Convert.ToString(dvDegreeDetails[0]["DegreeDetails"]).Trim();
                            courseName = Convert.ToString(dvDegreeDetails[0]["Course_Name"]).Trim();
                            departmentName = Convert.ToString(dvDegreeDetails[0]["Dept_Name"]).Trim();
                            departmentAcr = Convert.ToString(dvDegreeDetails[0]["dept_acronym"]).Trim();
                            typeName = Convert.ToString(dvDegreeDetails[0]["type"]).Trim();
                            eduLevel = Convert.ToString(dvDegreeDetails[0]["Edu_Level"]).Trim();
                            duration = Convert.ToString(dvDegreeDetails[0]["Duration"]).Trim();
                        }

                        
                        serialNo++;
                         

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        int rowValues = dtl.Rows.Count - 2;
                        
                        tagcol = "";


                        dtl.Rows[(rowValues - 1) + 2][0] = Convert.ToString(serialNo).Trim();
                        tagcol += "000^col0$";
                       

                        

                        dtl.Rows[(rowValues - 1) + 2][1] = Convert.ToString(degreeDetails).Trim();
                        tagcol += "" + Convert.ToString(degreeCode).Trim() + "^col1$";

                        

                        dtl.Rows[(rowValues - 1) + 2][2] = Convert.ToString(departmentName).Trim();
                        tagcol += "" + Convert.ToString(courseName).Trim() + "^col2$";

                        

                        dtl.Rows[(rowValues - 1) + 2][3] = Convert.ToString(rollNo).Trim();
                        tagcol += "" + Convert.ToString(appNo).Trim() + "^col3$";

                        

                        dtl.Rows[(rowValues - 1) + 2][4] = Convert.ToString(regNo).Trim();
                        tagcol += "000^col4$";

                        

                        dtl.Rows[(rowValues - 1) + 2][5] = Convert.ToString(rollAdmit).Trim();
                        tagcol += "000^col5$";

                        

                        dtl.Rows[(rowValues - 1) + 2][6] = Convert.ToString(studentType).Trim();
                        tagcol += "000^col6$";

                        

                        dtl.Rows[(rowValues - 1) + 2][7] = Convert.ToString(studentName).Trim();
                        tagcol += "000^col7$";

                        

                        dtl.Rows[(rowValues - 1) + 2][8] = Convert.ToString(currentSemester).Trim();
                        tagcol += "000^col8$";

                        
                        dtl.Rows[(rowValues - 1) + 2][9] = Convert.ToString(section).Trim();
                        tagcol += "000^col9$";

                        

                        dtl.Rows[(rowValues - 1) + 2][10] = Convert.ToString(gender).Trim();
                        tagcol += "000^col10$";

                       

                        dtl.Rows[(rowValues - 1) + 2][11] = Convert.ToString(modeofAdmition).Trim();
                        tagcol += "000^col11$";

                        for (int col = 0; col < dtl.Columns.Count; )
                        {
                            if (col > 11 && col < dtl.Columns.Count-1)
                            {
                                

                                subjectNo = headercoltag[col].ToString();

                                DataTable dtStudenSubjectMark = new DataTable();
                                double markObtained = 0;
                                obtainedMarks = string.Empty;
                                if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsStudentDetails.Tables[0].DefaultView.RowFilter = "degree_code='" + degreeCode + "' and roll_no='" + rollNo + "' and subject_no='" + subjectNo + "'";
                                    dtStudenSubjectMark = dsStudentDetails.Tables[0].DefaultView.ToTable();
                                }
                                if (dtStudenSubjectMark.Rows.Count > 0)
                                {
                                    markObtained = 0;
                                    obtainedMarks = Convert.ToString(dtStudenSubjectMark.Rows[0]["Marks"]).Trim();
                                    string moderationMark = Convert.ToString(dtStudenSubjectMark.Rows[0]["moderationMark"]).Trim();
                                    string examCode = Convert.ToString(dtStudenSubjectMark.Rows[0]["exam_code"]).Trim();
                                    double.TryParse(obtainedMarks, out markObtained);
                                    string dispalyMark = string.Empty;

                                    DataTable dtMinMax = new DataTable();
                                    dsStudentDetails.Tables[0].DefaultView.RowFilter = "roll_no<>'' and Marks>=0 and subject_no='" + subjectNo + "'";
                                    dtMinMax = dsStudentDetails.Tables[0].DefaultView.ToTable();
                                    double minMark = 0;
                                    double maxMark = 0;
                                    object minimumMarks = dtMinMax.Compute("MIN(Marks)", "roll_no<>'' and Marks>=0 and subject_no='" + subjectNo + "'");
                                    object maximumMarks = dtMinMax.Compute("MAX(Marks)", "roll_no<>'' and Marks>=0 and subject_no='" + subjectNo + "'");
                                    double.TryParse(Convert.ToString(minimumMarks).Trim(), out minMark);
                                    double.TryParse(Convert.ToString(maximumMarks).Trim(), out maxMark);
                                    double newMark = 0;
                                    string gg = "";
                                    if (markObtained < 0)
                                    {
                                        getMarkName(obtainedMarks, out dispalyMark);
                                        //FpCamTestMarkBoost.Sheets[0].Cells[rowValues - 1, col + 1].Text = Convert.ToString("--").Trim();
                                        //FpCamTestMarkBoost.Sheets[0].Cells[rowValues - 1, col + 1].Tag = false;
                                        if (!string.IsNullOrEmpty(moderationMark))
                                        {
                                            double.TryParse(moderationMark.Trim(), out newMark);
                                            newMark = Math.Round(newMark, 0, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(obtainedMarks) < 0)  //added by prabha 21/11/2017
                                            {
                                                newMark = (((markObtained - minMark) * 30) / (maxMark - minMark)) + 70;
                                                newMark = Math.Round(newMark, 0, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                newMark = (((markObtained - minMark) * 30) / (maxMark - minMark)) + 70;
                                                newMark = Math.Round(newMark, 0, MidpointRounding.AwayFromZero);
                                            }

                                        }
                                        

                                        dtl.Rows[(rowValues - 1) + 2][col + 1] = Convert.ToString(newMark).Trim();
                                        tagcol += "true^col" + col + 1 + "$";
                                    }
                                    else
                                    {
                                        dispalyMark = Convert.ToString(markObtained).Trim();
                                        if (!string.IsNullOrEmpty(moderationMark))
                                        {
                                            double.TryParse(moderationMark.Trim(), out newMark);
                                            newMark = Math.Round(newMark, 0, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            newMark = (((markObtained - minMark) * 30) / (maxMark - minMark)) + 70;
                                            newMark = Math.Round(newMark, 0, MidpointRounding.AwayFromZero);
                                        }
                                        

                                        dtl.Rows[(rowValues - 1) + 2][col + 1] = Convert.ToString(newMark).Trim();
                                        gg= "true^col" + col + 1 + "$";
                                    }
                                    

                                    dtl.Rows[(rowValues - 1) + 2][col] = Convert.ToString(dispalyMark).Trim();
                                    tagcol += "" + Convert.ToString(examCode).Trim() +"^col" + col + "$";
                                    if(gg!="")
                                        tagcol += gg;
                                }
                                else
                                {
                                    


                                    dtl.Rows[(rowValues - 1) + 2][col] = Convert.ToString("--").Trim();
                                    tagcol += "" + Convert.ToString("") +"^col" + col + "$";


                                    dtl.Rows[(rowValues - 1) + 2][col + 1] = Convert.ToString("--").Trim();
                                    tagcol += "false^col" + (col + 1)+ "$";
                                }

                                
                            }

                            if (dtl.Columns.Count - 1 == col)
                            {
                                dtl.Rows[(rowValues - 1) + 2][col] = tagcol;
                                col++;
                            }
                            else
                            {
                                
                             
                                if (col > 11)
                                {
                                    
                                    col += 2;
                                }
                                else
                                {
                                    col++;
                                }
                            }
                        }
                    }
                    
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    

                    dtl.Rows[dtl.Rows.Count - 5][0] = Convert.ToString("Note\t:\t\t").Trim();

                    if (spanColumn == -1)
                    {
                        spanColumn = 1;
                    }
                    colspanstart = spanColumn;
                    

                    dtl.Rows[dtl.Rows.Count - 5][spanColumn] = Convert.ToString("Moderation Mark\t=\t(((T\t-\tMin Mark)\t*\t30)\t/\t(Max Mark\t-\tMin Mark))\t+\t70").Trim();


                    

                    dtl.Rows[dtl.Rows.Count - 4][spanColumn] = Convert.ToString("Where,").Trim();

                    

                    dtl.Rows[dtl.Rows.Count - 3][spanColumn] = Convert.ToString("*\tT\t-\tActual Mark Taken by the Student,").Trim();

                    

                    dtl.Rows[dtl.Rows.Count - 2][spanColumn] = Convert.ToString("*\tMin Mark\t-\tLowest Mark in the Class in that Particular Subject,").Trim();

                    

                    dtl.Rows[dtl.Rows.Count - 1][spanColumn] = Convert.ToString("*\tMax Mark\t-\tHighest Mark in the Class in that Particular Subject").Trim();


                    grdover.DataSource = dtl;
                    grdover.DataBind();
                    grdover.HeaderRow.Visible = false;

                    int ccc = 12;
                    
                    for (int i = 0; i < grdover.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdover.HeaderRow.Cells.Count; j++)
                        {

                            if (colofvisfalse[j].ToString() == "false" || colofvisfalse[j].ToString() == "False")
                            {
                                grdover.HeaderRow.Cells[j].Visible = false;
                                grdover.Rows[i].Cells[j].Visible = false;
                            }
                            
                            if (i == 0 || i == 1)
                            {
                                grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdover.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                grdover.Rows[i].Cells[j].BorderColor = Color.Black;
                                grdover.Rows[i].Cells[j].Font.Bold = true;

                                if (i == 0)
                                {
                                    
                                    if (j < 12)
                                    {
                                        grdover.Rows[i].Cells[j].RowSpan = 2;
                                        for (int a = i; a < 1; a++)
                                            grdover.Rows[a + 1].Cells[j].Visible = false;
                                    }
                                    else if (j == ccc && j != grdover.HeaderRow.Cells.Count-1)
                                    {
                                        grdover.Rows[i].Cells[j].ColumnSpan = 2;
                                        for (int a = j + 1; a < j + 2; a++)
                                            grdover.Rows[i].Cells[a].Visible = false;

                                        ccc += 2;
                                        
                                    }



                                }
                            }
                            else
                            {

                                if (grdover.HeaderRow.Cells[j].Text == "Roll No" || grdover.HeaderRow.Cells[j].Text == "Reg No" || grdover.HeaderRow.Cells[j].Text == "Student Name")
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                else
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;


                                if (i == grdover.Rows.Count - 5 && j==0)
                                {
                                    grdover.Rows[i].Cells[j].VerticalAlign = VerticalAlign.Top;
                                    grdover.Rows[i].Cells[j].RowSpan = 5;
                                    for (int a = i; a < i+4; a++)
                                        grdover.Rows[a + 1].Cells[j].Visible = false;
                                }
                                if (i >= grdover.Rows.Count - 5 && j == colspanstart && j != 0)
                                {
                                    grdover.Rows[i].Cells[j].ColumnSpan = grdover.HeaderRow.Cells.Count - colspanstart;
                                    for (int a = j + 1; a < j + (grdover.HeaderRow.Cells.Count - colspanstart); a++)
                                        grdover.Rows[i].Cells[a].Visible = false;
                                }

                                
                            }
                        }

                    }

                    divMainContents.Visible = true;
                    
                    grdover.Visible = true;



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

    #endregion Go Click

    #region Close Popup

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

    #endregion Close Popup

    #region Print Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Printcontrol.Visible = false;
            string reportname = txtExcelFileName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (grdover.Visible == true)
                {
                    
                    da.printexcelreportgrid(grdover, reportname);
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

    #endregion Print Excel

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
            rptheadname = "CAM Moderation Report";
            string pagename = "COECoverSheetGeneration.aspx";
            string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlSem.SelectedItem).Trim();
            if (grdover.Visible == true)
            {
                string degreedetails = "";
                string ss = Session["usercode"].ToString();
                Printcontrol.loadspreaddetails(grdover, pagename, degreedetails, 0, ss);
                Printcontrol.Visible = true;
            }
            
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

    #endregion Button Click

    #region Reusable Method

    private bool getMarkValue(string marksName, out double markValue)
    {
        string mark = string.Empty;
        markValue = 0;
        bool isSuccess = false;
        try
        {
            marksName = marksName.Trim().ToUpper();
            switch (marksName)
            {
                case "AAA":
                    mark = "-1";
                    break;
                case "EL":
                    mark = "-2";
                    break;
                case "EOD":
                    mark = "-3";
                    break;
                case "ML":
                    mark = "-4";
                    break;
                case "SOD":
                    mark = "-5";
                    break;
                case "NSS":
                    mark = "-6";
                    break;
                case "NJ":
                    mark = "-7";
                    break;
                case "S":
                    mark = "-8";
                    break;
                case "L":
                    mark = "-9";
                    break;
                case "NCC":
                    mark = "-10";
                    break;
                case "HS":
                    mark = "-11";
                    break;
                case "PP":
                    mark = "-12";
                    break;
                case "SYOD":
                    mark = "-13";
                    break;
                case "COD":
                    mark = "-14";
                    break;
                case "OOD":
                    mark = "-15";
                    break;
                case "OD":
                    mark = "-16";
                    break;
                case "LA":
                    mark = "-17";
                    break;
                case "RAA":
                    mark = "-18";
                    break;
                default:
                    mark = marksName.Trim();
                    break;
            }
            isSuccess = double.TryParse(mark.Trim(), out markValue);
            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private bool getMarkName(string markValue, out string marksName)
    {
        string mark = string.Empty;
        marksName = string.Empty;
        bool isSuccess = false;
        try
        {
            switch (markValue)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "- 3":
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
                default:
                    mark = markValue.ToString().Trim();
                    break;
            }
            //isSuccess = int.TryParse(mark.Trim(), out markValue);
            marksName = mark.Trim();
            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
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
            lblBatch.Text = "Batch";
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type</param>
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
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                    dsSettings = da.select_method_wo_parameter(Master1, "Text");
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

    #endregion Reusable Method
    public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "CAM Mark Moderation";



    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
}