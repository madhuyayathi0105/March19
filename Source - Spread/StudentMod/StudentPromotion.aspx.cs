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
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;

#endregion Namespace Declaration

public partial class StudentMod_StudentPromotion : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

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

    string qry = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qryCollegeName = string.Empty;
    string qryCourseId = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;

    bool isSchool = false;

    int selected = 0;

    Institution institute;

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

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            if (!IsPostBack)
            {
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblFromErr.Text = string.Empty;
                lblFromErr.Visible = false;
                //divMainContentFrom.Visible = false;

                lblpromotionconfirm.Text = "";

                lblToErr.Text = string.Empty;
                lblToErr.Visible = false;
                //divMainContentToPromote.Visible = false;

                setLabelText();
                Bindcollege();
                BindRightsBaseBatch();
                BindDegree();
                BindBranch();
                BindSemester();
                BindRightsBasedSectionDetail();
                Init_Spread(FpFromPromote, 0);

                BindcollegeTo();
                BindRightsBaseBatchTo();
                BindDegreeTo();
                BindBranchTo();
                BindSemesterTo();
                BindRightsBasedSectionDetailTo();
                Init_Spread(FpToPromote, 1);
                rdb_Transfer_OnCheckedChanged(sender, e);

                cbl_degree_SelectedIndexChanged(sender, e);
                cbl_branch_SelectedIndexChanged(sender, e);
            }
        }
        catch (ThreadAbortException tex)
        {
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            ds.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", ht, "sp");
            ddlFromCollege.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlFromCollege.DataSource = ds;
                ddlFromCollege.DataTextField = "collname";
                ddlFromCollege.DataValueField = "college_code";
                ddlFromCollege.DataBind();
                ddlFromCollege.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            if (ddlFromCollege.Items.Count > 0 && ddlFromCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
                ddlFromBatch.DataSource = dsBatch;
                ddlFromBatch.DataTextField = "Batch_year";
                ddlFromBatch.DataValueField = "Batch_year";
                ddlFromBatch.DataBind();
                ddlFromBatch.SelectedIndex = 0;
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
                    ddlFromBatch.DataSource = ds;
                    ddlFromBatch.DataTextField = "Batch_Year";
                    ddlFromBatch.DataValueField = "Batch_Year";
                    ddlFromBatch.DataBind();
                    ddlFromBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            cbl_degree.Items.Clear();
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
            if (ddlFromCollege.Items.Count > 0 && ddlFromCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
            if (ddlFromBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlFromBatch.Items)
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
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    cbl_degree.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            cbl_branch.Items.Clear();
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

            if (ddlFromCollege.Items.Count > 0 && ddlFromCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
            if (cbl_degree.Items.Count > 0)
            {
                courseIds = string.Empty;
                foreach (ListItem li in cbl_degree.Items)
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
            if (ddlFromBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlFromBatch.Items)
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
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    cbl_branch.SelectedIndex = 0;
                    checkBoxListselectOrDeselect(cbl_branch, true);
                    CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lblFromBranch.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSemester()
    {
        try
        {
            ds.Clear();
            ddlFromSem.Items.Clear();
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
            if (ddlFromCollege.Items.Count > 0 && ddlFromCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
            if (ddlFromBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlFromBatch.Items)
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
            if (cbl_branch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in cbl_branch.Items)
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
                        ddlFromSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlFromSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlFromSem.SelectedIndex = 0;
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
                            ddlFromSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlFromSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlFromSem.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSections()
    {
        try
        {
            ddlFromSec.Items.Clear();
            ht.Clear();
            ht.Add("batch_year", ddlFromBatch.SelectedValue.ToString());
            ht.Add("degree_code", cbl_degree.SelectedValue);  //to be modified
            ds.Clear();
            ds = da.select_method("bind_sec", ht, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count5 = ds.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    ddlFromSec.DataSource = ds;
                    ddlFromSec.DataTextField = "sections";
                    ddlFromSec.DataValueField = "sections";
                    ddlFromSec.DataBind();
                    ddlFromSec.Items.Insert(0, "All");
                    ddlFromSec.Enabled = true;
                }
                else
                {
                    ddlFromSec.Enabled = false;
                }
            }
            else
            {
                ddlFromSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            ddlFromSec.Items.Clear();
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
            if (ddlFromCollege.Items.Count > 0 && ddlFromCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
            if (ddlFromBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlFromBatch.Items)
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
            if (cbl_branch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in cbl_branch.Items)
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
                ddlFromSec.DataSource = ds;
                ddlFromSec.DataTextField = "sections";
                ddlFromSec.DataValueField = "SecValues";
                ddlFromSec.DataBind();
                ddlFromSec.Enabled = true;

            }
            else
            {
                ddlFromSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindcollegeTo()
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
            ds.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", ht, "sp");
            ddlToCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlToCollege.DataSource = ds;
                ddlToCollege.DataTextField = "collname";
                ddlToCollege.DataValueField = "college_code";
                ddlToCollege.DataBind();
                ddlToCollege.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBaseBatchTo()
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
            if (ddlToCollege.Items.Count > 0 && ddlToCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlToCollege.Items)
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
                ddlToBatch.DataSource = dsBatch;
                ddlToBatch.DataTextField = "Batch_year";
                ddlToBatch.DataValueField = "Batch_year";
                ddlToBatch.DataBind();
                ddlToBatch.SelectedIndex = 0;
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
                    ddlToBatch.DataSource = ds;
                    ddlToBatch.DataTextField = "Batch_Year";
                    ddlToBatch.DataValueField = "Batch_Year";
                    ddlToBatch.DataBind();
                    ddlToBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegreeTo()
    {
        try
        {
            lblToErr.Text = string.Empty;
            lblToErr.Visible = false;
            ddlToDegree.Items.Clear();
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
            if (ddlToCollege.Items.Count > 0 && ddlToCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlToCollege.Items)
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
            if (ddlToBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlToBatch.Items)
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
                    ddlToDegree.DataSource = ds;
                    ddlToDegree.DataTextField = "course_name";
                    ddlToDegree.DataValueField = "course_id";
                    ddlToDegree.DataBind();
                    ddlToDegree.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranchTo()
    {
        try
        {
            lblToErr.Text = string.Empty;
            lblToErr.Visible = false;
            ddlToBranch.Items.Clear();
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
            if (ddlToCollege.Items.Count > 0 && ddlToCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlToCollege.Items)
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
            if (ddlToDegree.Items.Count > 0)
            {
                courseIds = string.Empty;
                foreach (ListItem li in ddlToDegree.Items)
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
            if (ddlToBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlToBatch.Items)
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
                    ddlToBranch.DataSource = ds;
                    ddlToBranch.DataTextField = "dept_name";
                    ddlToBranch.DataValueField = "degree_code";
                    ddlToBranch.DataBind();
                    ddlToBranch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSemesterTo()
    {
        try
        {
            ds.Clear();
            ddlToSem.Items.Clear();
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
            if (ddlToCollege.Items.Count > 0 && ddlToCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlToCollege.Items)
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
            if (ddlToBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlToBatch.Items)
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
            if (ddlToBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlToBranch.Items)
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
                        ddlToSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlToSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlToSem.SelectedIndex = 0;
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
                            ddlToSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlToSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlToSem.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSectionsTo()
    {
        try
        {
            ddlToSec.Items.Clear();
            ht.Clear();
            ht.Add("batch_year", ddlToBatch.SelectedValue.ToString());
            ht.Add("degree_code", ddlToBranch.SelectedValue);
            ds.Clear();
            ds = da.select_method("bind_sec", ht, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count5 = ds.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    ddlToSec.DataSource = ds;
                    ddlToSec.DataTextField = "sections";
                    ddlToSec.DataValueField = "sections";
                    ddlToSec.DataBind();
                    ddlToSec.Items.Insert(0, "All");
                    ddlToSec.Enabled = true;
                }
                else
                {
                    ddlToSec.Enabled = false;
                }
            }
            else
            {
                ddlToSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBasedSectionDetailTo()
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
            ddlToSec.Items.Clear();
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
            if (ddlToCollege.Items.Count > 0 && ddlToCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlToCollege.Items)
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
            if (ddlToBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlToBatch.Items)
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
            if (ddlToBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlToBranch.Items)
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
                ddlToSec.DataSource = ds;
                ddlToSec.DataTextField = "sections";
                ddlToSec.DataValueField = "SecValues";
                ddlToSec.DataBind();
                ddlToSec.Enabled = true;

            }
            else
            {
                ddlToSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblToErr.Text = Convert.ToString(ex);
            lblToErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlToCollege.Items.Count > 0) ? Convert.ToString(ddlToCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public int Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {

            #region FpSpread Style

            //FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;

            FpSpread1.Sheets[0].Visible = true;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            Farpoint.StyleInfo darkstyle = new Farpoint.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = Color.Black;
            Farpoint.StyleInfo sheetstyle = new Farpoint.StyleInfo();
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
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.ShowHeaderSelection = false;
            FpSpread1.Sheets[0].FrozenRowCount = 1;

            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.Always;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.Always;
            Farpoint.CheckBoxCellType chkSelectAll = new Farpoint.CheckBoxCellType();
            chkSelectAll.AutoPostBack = true;

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
                FpSpread1.Sheets[0].ColumnCount = 13;
                FpSpread1.Sheets[0].Columns[0].Width = 48;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Tag = 0;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblFromDegree.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Tag = 1;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lblFromBranch.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Tag = 2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isRollVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Tag = 3;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isRegVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Tag = 4;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 5;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 120;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 6;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].Columns[7].Width = 200;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 7;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = lblFromSem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 8;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                FpSpread1.Sheets[0].Columns[9].Width = 150;  //modified 
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Section";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Tag = 9;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

                FpSpread1.Sheets[0].Columns[10].Width = 100;
                FpSpread1.Sheets[0].Columns[10].Locked = true;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = 10;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                FpSpread1.Sheets[0].Columns[11].Width = 100;
                FpSpread1.Sheets[0].Columns[11].Locked = true;
                FpSpread1.Sheets[0].Columns[11].Resizable = false;
                FpSpread1.Sheets[0].Columns[11].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Mode";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = 11;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);

                FpSpread1.Sheets[0].Columns[12].Width = 50;
                FpSpread1.Sheets[0].Columns[12].Locked = false;
                FpSpread1.Sheets[0].Columns[12].Resizable = false;
                FpSpread1.Sheets[0].Columns[12].Visible = true;
                FpSpread1.Sheets[0].Columns[12].CellType = chkSelectAll;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = 12;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
                FpSpread1.Height = 600;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 13;
                FpSpread1.Sheets[0].Columns[0].Width = 48;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Tag = 0;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblFromDegree.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Tag = 1;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lblFromBranch.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Tag = 2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isRollVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Tag = 3;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isRegVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Tag = 4;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 5;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 120;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 6;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].Columns[7].Width = 200;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 7;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = lblFromSem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 8;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                FpSpread1.Sheets[0].Columns[9].Width = 150;
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Section";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Tag = 9;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

                FpSpread1.Sheets[0].Columns[10].Width = 100;
                FpSpread1.Sheets[0].Columns[10].Locked = true;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = 10;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                FpSpread1.Sheets[0].Columns[11].Width = 100;
                FpSpread1.Sheets[0].Columns[11].Locked = true;
                FpSpread1.Sheets[0].Columns[11].Resizable = false;
                FpSpread1.Sheets[0].Columns[11].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Mode";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = 11;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);

                FpSpread1.Sheets[0].Columns[12].Width = 50;
                FpSpread1.Sheets[0].Columns[12].Locked = false;
                FpSpread1.Sheets[0].Columns[12].Resizable = false;
                FpSpread1.Sheets[0].Columns[12].Visible = false;
                FpSpread1.Sheets[0].Columns[12].CellType = new Farpoint.CheckBoxCellType();
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = 12;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
                FpSpread1.Height = 600;
            }
            FpSpread1.Sheets[0].RowCount = 1;
            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].CellType = chkSelectAll;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            if (type == 0)
            {
                FpSpread1.Sheets[0].Rows[0].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Rows[0].Visible = false;
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
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return -1;
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void ddlFromCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;

            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;

            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;


            //divMainContentFrom.Visible = false;
            collegeCode = Convert.ToString(ddlFromCollege.SelectedValue).Trim();
            //ddlToCollege.SelectedValue = collegeCode;
            if (ddlToCollege.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToCollege.Items.Count];
                ddlToCollege.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromCollege.SelectedItem.Text, collegeCode)))
                {
                    ddlToCollege.SelectedValue = collegeCode;
                }
            }
            ddlToCollege_SelectedIndexChanged(sender, e);
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSemester();
            BindRightsBasedSectionDetail();
            Init_Spread(FpFromPromote, 0);

            BindRightsBaseBatchTo();
            BindDegreeTo();
            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFromBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            collegeCode = Convert.ToString(ddlFromCollege.SelectedValue).Trim();
            if (ddlToCollege.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToCollege.Items.Count];
                ddlToCollege.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromCollege.SelectedItem.Text, collegeCode)))
                {
                    ddlToCollege.SelectedValue = collegeCode;
                }
            }
            ddlToCollege_SelectedIndexChanged(sender, e);
            batchYears = Convert.ToString(ddlFromBatch.SelectedValue).Trim();
            if (ddlToBatch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBatch.Items.Count];
                ddlToBatch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromBatch.SelectedItem.Text, batchYears)))
                {
                    ddlToBatch.SelectedValue = batchYears;
                }
            }

            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;

            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;

            ddlToBatch_SelectedIndexChanged(sender, e);
            BindDegree();
            BindBranch();
            BindSemester();
            BindRightsBasedSectionDetail();
            Init_Spread(FpFromPromote, 0);

            BindDegreeTo();
            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFromDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;

            collegeCode = Convert.ToString(ddlFromCollege.SelectedValue).Trim();
            if (ddlToCollege.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToCollege.Items.Count];
                ddlToCollege.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromCollege.SelectedItem.Text, collegeCode)))
                {
                    ddlToCollege.SelectedValue = collegeCode;
                }
            }
            ddlToCollege_SelectedIndexChanged(sender, e);
            batchYears = Convert.ToString(ddlFromBatch.SelectedValue).Trim();
            if (ddlToBatch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBatch.Items.Count];
                ddlToBatch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromBatch.SelectedItem.Text, batchYears)))
                {
                    ddlToBatch.SelectedValue = batchYears;
                }
            }
            ddlToBatch_SelectedIndexChanged(sender, e);
            degreeCodes = Convert.ToString(cbl_degree.SelectedValue).Trim();  //to be modifed
            if (ddlToDegree.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToDegree.Items.Count];
                ddlToDegree.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(cbl_degree.SelectedItem.Text, degreeCodes)))
                {
                    ddlToDegree.SelectedValue = degreeCodes;
                }
            }
            ddlToDegree_SelectedIndexChanged(sender, e);
            BindBranch();
            BindSemester();
            BindRightsBasedSectionDetail();
            Init_Spread(FpFromPromote, 0);

            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFromBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            collegeCode = Convert.ToString(ddlFromCollege.SelectedValue).Trim();
            if (ddlToCollege.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToCollege.Items.Count];
                ddlToCollege.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromCollege.SelectedItem.Text, collegeCode)))
                {
                    ddlToCollege.SelectedValue = collegeCode;
                }
            }
            ddlToCollege_SelectedIndexChanged(sender, e);
            batchYears = Convert.ToString(ddlFromBatch.SelectedValue).Trim();
            if (ddlToBatch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBatch.Items.Count];
                ddlToBatch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromBatch.SelectedItem.Text, batchYears)))
                {
                    ddlToBatch.SelectedValue = batchYears;
                }
            }
            ddlToBatch_SelectedIndexChanged(sender, e);
            courseIds = Convert.ToString(cbl_degree.SelectedValue).Trim();  //to be modified   ddlFromBranch_SelectedIndexChanged
            if (ddlToDegree.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToDegree.Items.Count];
                ddlToDegree.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(cbl_degree.SelectedItem.Text, courseIds)))
                {
                    ddlToDegree.SelectedValue = courseIds;
                }
            }
            ddlToDegree_SelectedIndexChanged(sender, e);
            degreeCodes = Convert.ToString(cbl_branch.SelectedValue).Trim();  //to be modified   ddlFromBranch_SelectedIndexChanged
            if (ddlToBranch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBranch.Items.Count];
                ddlToBranch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(cbl_branch.SelectedItem.Text, degreeCodes)))
                {
                    ddlToBranch.SelectedValue = degreeCodes;
                }
            }
            ddlToBranch_SelectedIndexChanged(sender, e);
            BindSemester();
            BindRightsBasedSectionDetail();
            Init_Spread(FpFromPromote, 0);

            BindSemesterTo();
            BindRightsBasedSectionDetailTo();
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFromSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            collegeCode = Convert.ToString(ddlFromCollege.SelectedValue).Trim();
            if (ddlToCollege.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToCollege.Items.Count];
                ddlToCollege.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromCollege.SelectedItem.Text, collegeCode)))
                {
                    ddlToCollege.SelectedValue = collegeCode;
                }
            }
            ddlToCollege_SelectedIndexChanged(sender, e);
            batchYears = Convert.ToString(ddlFromBatch.SelectedValue).Trim();
            if (ddlToBatch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBatch.Items.Count];
                ddlToBatch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromBatch.SelectedItem.Text, batchYears)))
                {
                    ddlToBatch.SelectedValue = batchYears;
                }
            }
            ddlToBatch_SelectedIndexChanged(sender, e);
            courseIds = Convert.ToString(cbl_degree.SelectedValue).Trim();   //to be modified  ddlFromSem_SelectedIndexChanged
            if (ddlToDegree.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToDegree.Items.Count];
                ddlToDegree.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(cbl_degree.SelectedItem.Text, courseIds)))
                {
                    ddlToDegree.SelectedValue = courseIds;
                }
            }
            ddlToDegree_SelectedIndexChanged(sender, e);
            degreeCodes = Convert.ToString(cbl_branch.SelectedValue).Trim();   //to be modified  ddlFromSem_SelectedIndexChanged
            if (ddlToBranch.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToBranch.Items.Count];
                ddlToBranch.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(cbl_branch.SelectedItem.Text, degreeCodes)))
                {
                    ddlToBranch.SelectedValue = degreeCodes;
                }
            }
            ddlToBranch_SelectedIndexChanged(sender, e);
            semesters = Convert.ToString(ddlFromSem.SelectedValue).Trim();
            if (ddlToSem.Items.Count > 0)
            {
                ListItem[] listItem = new ListItem[ddlToSem.Items.Count];
                ddlToSem.Items.CopyTo(listItem, 0);
                if (listItem.Contains(new ListItem(ddlFromSem.SelectedItem.Text, semesters)))
                {
                    ddlToSem.SelectedValue = semesters;
                }
            }
            chkpassed.Checked = false;
            chkpassed.Enabled = false;
            if (ddlFromSem.Items.Count > 0)
            {
                if (ddlFromSem.SelectedIndex == (ddlFromSem.Items.Count - 1))
                    chkpassed.Enabled = true;
            }
            ddlToSem_SelectedIndexChanged(sender, e);
            BindRightsBasedSectionDetail();
            BindRightsBasedSectionDetailTo();
            Init_Spread(FpFromPromote, 0);
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFromSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            Init_Spread(FpFromPromote, 0);
            Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            BindRightsBaseBatchTo();
            BindDegreeTo();
            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            BindDegreeTo();
            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            BindBranchTo();
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            BindSemesterTo();
            BindRightsBasedSectionDetailTo();

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;
            BindRightsBasedSectionDetailTo();

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlToSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            //divMainContentFrom.Visible = false;

            //Init_Spread(FpToPromote, 1);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpFromPromote_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpFromPromote.SaveChanges();
            int r = FpFromPromote.Sheets[0].ActiveRow;
            int j = FpFromPromote.Sheets[0].ActiveColumn;
            if (r == 0 && j == FpFromPromote.Sheets[0].ColumnCount - 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpFromPromote.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpFromPromote.Sheets[0].RowCount; row++)
                {
                    if (val == 1)
                        FpFromPromote.Sheets[0].Cells[row, j].Value = 1;
                    else
                        FpFromPromote.Sheets[0].Cells[row, j].Value = 0;
                }
            }
        }
        catch
        {
        }
    }

    #endregion Index Changed Events

    #region Button Click

    #region Close Popup

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region Click Promote From

    protected void btnPromoteFrom_Click(object sender, EventArgs e)
    {
        try
        {
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            FpFromPromote.SaveChanges();
            FpToPromote.SaveChanges();

            DataTable dtFromPromote = new DataTable();
            DataRow drFromPromote;
            dtFromPromote.Columns.Add("college_code");
            dtFromPromote.Columns.Add("Batch_Year");
            dtFromPromote.Columns.Add("degree_code");
            dtFromPromote.Columns.Add("Current_Semester");
            dtFromPromote.Columns.Add("Sections");
            dtFromPromote.Columns.Add("App_No");
            dtFromPromote.Columns.Add("serialno");
            dtFromPromote.Columns.Add("Roll_Admit");
            dtFromPromote.Columns.Add("Roll_No");
            dtFromPromote.Columns.Add("Reg_No");
            dtFromPromote.Columns.Add("Stud_Name");
            dtFromPromote.Columns.Add("Stud_Type");
            dtFromPromote.Columns.Add("sex");
            dtFromPromote.Columns.Add("Mode");
            dtFromPromote.Columns.Add("DegreeDetails");
            dtFromPromote.Columns.Add("Course_Name");
            dtFromPromote.Columns.Add("Dept_Name");
            dtFromPromote.Columns.Add("dept_acronym");
            dtFromPromote.Columns.Add("type");
            dtFromPromote.Columns.Add("Edu_Level");
            dtFromPromote.Columns.Add("Duration");
            dtFromPromote.Columns.Add("InsType");
            dtFromPromote.Columns.Add("IsPromoted");

            bool isRemove = false;
            if (FpToPromote.Sheets[0].RowCount > 1)
            {
                for (int row = 1; row < FpToPromote.Sheets[0].RowCount; row++)
                {
                    int selected = 0;
                    int.TryParse(Convert.ToString(FpToPromote.Sheets[0].Cells[row, FpToPromote.Sheets[0].ColumnCount - 1].Value).Trim(), out selected);

                    string rollNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Text).Trim();
                    string appNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Tag).Trim();
                    string regNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Text).Trim();
                    string studentName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 7].Text).Trim();
                    string rollAdmit = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 5].Text).Trim();
                    string collegeCode = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 0].Note).Trim();
                    string batchYear = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Note).Trim();
                    string degreeCode = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 1].Tag).Trim();
                    string currentSemester = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 8].Text).Trim();
                    string section = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 9].Text).Trim();
                    string serialNos = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 0].Tag).Trim();
                    string studentType = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 6].Text).Trim();
                    string gender = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 10].Text).Trim();
                    string modeofAdmition = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 11].Text).Trim();
                    string InsType = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Note).Trim();
                    string degreeDetails = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 1].Text).Trim();
                    string courseName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Tag).Trim();
                    string departmentName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Text).Trim();
                    string departmentAcr = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 11].Text).Trim();
                    string typeName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Tag).Trim();
                    string eduLevel = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Note).Trim();
                    string duration = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 8].Tag).Trim();
                    if (!string.IsNullOrEmpty(appNo))
                    {
                        drFromPromote = dtFromPromote.NewRow();
                        drFromPromote["college_code"] = Convert.ToString(collegeCode).Trim();
                        drFromPromote["Batch_Year"] = Convert.ToString(batchYear).Trim();
                        drFromPromote["degree_code"] = Convert.ToString(degreeCode).Trim();
                        drFromPromote["Current_Semester"] = Convert.ToString(currentSemester).Trim();
                        drFromPromote["Sections"] = Convert.ToString(section).Trim();
                        drFromPromote["App_No"] = Convert.ToString(appNo).Trim();
                        drFromPromote["serialno"] = Convert.ToString(serialNos).Trim();
                        drFromPromote["Roll_Admit"] = Convert.ToString(rollAdmit).Trim();
                        drFromPromote["Roll_No"] = Convert.ToString(rollNo).Trim();
                        drFromPromote["Reg_No"] = Convert.ToString(regNo).Trim();
                        drFromPromote["Stud_Name"] = Convert.ToString(studentName).Trim();
                        drFromPromote["Stud_Type"] = Convert.ToString(studentType).Trim();
                        drFromPromote["sex"] = Convert.ToString(gender).Trim();
                        drFromPromote["Mode"] = Convert.ToString(modeofAdmition).Trim();
                        drFromPromote["DegreeDetails"] = Convert.ToString(degreeDetails).Trim();
                        drFromPromote["Course_Name"] = Convert.ToString(courseName).Trim();
                        drFromPromote["Dept_Name"] = Convert.ToString(departmentName).Trim();
                        drFromPromote["dept_acronym"] = Convert.ToString(departmentAcr).Trim();
                        drFromPromote["type"] = Convert.ToString(typeName).Trim();
                        drFromPromote["Edu_Level"] = Convert.ToString(eduLevel).Trim();
                        drFromPromote["Duration"] = Convert.ToString(duration).Trim();
                        drFromPromote["InsType"] = Convert.ToString(InsType).Trim();
                        drFromPromote["IsPromoted"] = Convert.ToString("1").Trim();
                        dtFromPromote.Rows.Add(drFromPromote);
                    }
                }
            }

            if (FpFromPromote.Sheets[0].RowCount > 1)
            {
                int serialNo = 0;
                Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                Farpoint.CheckBoxCellType chkSelectAll = new Farpoint.CheckBoxCellType();
                chkSelectAll.AutoPostBack = true;
                Farpoint.CheckBoxCellType chkOneByOne = new Farpoint.CheckBoxCellType();
                for (int row = 1; row < FpFromPromote.Sheets[0].RowCount; row++)
                {
                    int selected = 0;
                    int.TryParse(Convert.ToString(FpFromPromote.Sheets[0].Cells[row, FpFromPromote.Sheets[0].ColumnCount - 1].Value).Trim(), out selected);
                    string rollNo = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 3].Text).Trim();
                    string appNo = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 3].Tag).Trim();
                    string regNo = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 4].Text).Trim();
                    string studentName = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 7].Text).Trim();
                    string rollAdmit = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 5].Text).Trim();
                    string collegeCode = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 0].Note).Trim();
                    string batchYear = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 3].Note).Trim();
                    string degreeCode = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 1].Tag).Trim();
                    string currentSemester = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 8].Text).Trim();
                    string section = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 9].Tag).Trim();
                    string serialNos = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 0].Tag).Trim();
                    string studentType = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 6].Text).Trim();
                    string gender = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 10].Text).Trim();
                    string modeofAdmition = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 11].Text).Trim();
                    string InsType = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 4].Note).Trim();
                    string degreeDetails = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 1].Text).Trim();
                    string courseName = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 2].Tag).Trim();
                    string departmentName = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 2].Text).Trim();
                    string departmentAcr = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 11].Text).Trim();
                    string typeName = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 4].Tag).Trim();
                    string eduLevel = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 2].Note).Trim();
                    string duration = Convert.ToString(FpFromPromote.Sheets[0].Cells[row, 8].Tag).Trim();
                    if (!string.IsNullOrEmpty(appNo))
                    {
                        drFromPromote = dtFromPromote.NewRow();
                        drFromPromote["college_code"] = Convert.ToString(collegeCode).Trim();
                        drFromPromote["Batch_Year"] = Convert.ToString(batchYear).Trim();
                        drFromPromote["degree_code"] = Convert.ToString(degreeCode).Trim();
                        drFromPromote["Current_Semester"] = Convert.ToString(currentSemester).Trim();
                        drFromPromote["Sections"] = Convert.ToString(section).Trim();
                        drFromPromote["App_No"] = Convert.ToString(appNo).Trim();
                        drFromPromote["serialno"] = Convert.ToString(serialNos).Trim();
                        drFromPromote["Roll_Admit"] = Convert.ToString(rollAdmit).Trim();
                        drFromPromote["Roll_No"] = Convert.ToString(rollNo).Trim();
                        drFromPromote["Reg_No"] = Convert.ToString(regNo).Trim();
                        drFromPromote["Stud_Name"] = Convert.ToString(studentName).Trim();
                        drFromPromote["Stud_Type"] = Convert.ToString(studentType).Trim();
                        drFromPromote["sex"] = Convert.ToString(gender).Trim();
                        drFromPromote["Mode"] = Convert.ToString(modeofAdmition).Trim();
                        drFromPromote["DegreeDetails"] = Convert.ToString(degreeDetails).Trim();
                        drFromPromote["Course_Name"] = Convert.ToString(courseName).Trim();
                        drFromPromote["Dept_Name"] = Convert.ToString(departmentName).Trim();
                        drFromPromote["dept_acronym"] = Convert.ToString(departmentAcr).Trim();
                        drFromPromote["type"] = Convert.ToString(typeName).Trim();
                        drFromPromote["Edu_Level"] = Convert.ToString(eduLevel).Trim();
                        drFromPromote["Duration"] = Convert.ToString(duration).Trim();
                        drFromPromote["InsType"] = Convert.ToString(InsType).Trim();
                        if (selected == 1)
                        {
                            drFromPromote["IsPromoted"] = Convert.ToString("1").Trim();
                            isRemove = true;
                        }
                        else
                        {
                            drFromPromote["IsPromoted"] = Convert.ToString("0").Trim();
                        }
                        dtFromPromote.Rows.Add(drFromPromote);
                    }
                }
                if (isRemove)
                {
                    FpFromPromote.Sheets[0].RowCount = 0;
                    FpToPromote.Sheets[0].RowCount = 0;
                    int serialNoTo = 0;
                    int serialNoFrom = 0;
                    DataTable dtTempPromote = new DataTable();
                    DataTable dtTempNotPromote = new DataTable();
                    if (dtFromPromote.Rows.Count > 0)
                    {
                        dtFromPromote.DefaultView.RowFilter = "IsPromoted='0'";
                        dtTempNotPromote = dtFromPromote.DefaultView.ToTable();
                    }
                    if (dtFromPromote.Rows.Count > 0)
                    {
                        dtFromPromote.DefaultView.RowFilter = "IsPromoted='1'";
                        dtTempPromote = dtFromPromote.DefaultView.ToTable();
                    }
                    if (dtTempPromote.Rows.Count > 0)
                    {
                        int count = 1;
                        FpToPromote.Sheets[0].RowCount = 0;
                        Init_Spread(FpToPromote, 1);
                        foreach (DataRow drStudents in dtTempPromote.Rows)
                        {
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
                            string InsType = Convert.ToString(drStudents["InsType"]).Trim();
                            string degreeDetails = Convert.ToString(drStudents["DegreeDetails"]).Trim();
                            string courseName = Convert.ToString(drStudents["Course_Name"]).Trim();
                            string departmentName = Convert.ToString(drStudents["Dept_Name"]).Trim();
                            string departmentAcr = Convert.ToString(drStudents["dept_acronym"]).Trim();
                            string typeName = Convert.ToString(drStudents["type"]).Trim();
                            string eduLevel = Convert.ToString(drStudents["Edu_Level"]).Trim();
                            string duration = Convert.ToString(drStudents["Duration"]).Trim();
                            string IsPromoted = Convert.ToString(drStudents["IsPromoted"]).Trim();
                            if (IsPromoted == "1")
                            {
                                count++;
                                txtCell = new Farpoint.TextCellType();
                                chkOneByOne = new Farpoint.CheckBoxCellType();
                                FpToPromote.Sheets[0].RowCount++;
                                int rowValues = FpToPromote.Sheets[0].RowCount;
                                serialNoTo++;
                                if (FpToPromote.Sheets[0].RowCount > 0)
                                {
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 0].Text = Convert.ToString(rowValues - 1).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 0].Tag = Convert.ToString(serialNos).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 0].Note = Convert.ToString(collegeCode).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 1].Text = Convert.ToString(degreeDetails).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 1].Tag = Convert.ToString(degreeCode).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 1].Note = Convert.ToString(departmentAcr).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 2].Text = Convert.ToString(departmentName).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 2].Tag = Convert.ToString(courseName).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 2].Note = Convert.ToString(eduLevel).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 3].Text = Convert.ToString(rollNo).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 3].Tag = Convert.ToString(appNo).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 3].Note = Convert.ToString(batchYear).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 4].Text = Convert.ToString(regNo).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 4].Tag = Convert.ToString(typeName).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 4].Note = Convert.ToString(InsType).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 5].Text = Convert.ToString(rollAdmit).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 6].Text = Convert.ToString(studentType).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 7].Text = Convert.ToString(studentName).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 8].Text = Convert.ToString(currentSemester).Trim();
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 8].Tag = Convert.ToString(duration).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 9].Text = Convert.ToString(section).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 10].Text = Convert.ToString(gender).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 11].Text = Convert.ToString(modeofAdmition).Trim();

                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 12].Value = 0;
                                    FpToPromote.Sheets[0].Cells[rowValues - 1, 12].CellType = chkOneByOne;
                                    for (int col = 0; col < FpToPromote.Sheets[0].ColumnCount; col++)
                                    {
                                        FpToPromote.Sheets[0].Cells[rowValues - 1, col].Font.Name = "Book Antiqua";

                                        FpToPromote.Sheets[0].Cells[rowValues - 1, col].VerticalAlign = VerticalAlign.Middle;
                                        if (col != 7)
                                        {
                                            FpToPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpToPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (col != 12)
                                        {
                                            FpToPromote.Sheets[0].Cells[rowValues - 1, col].Locked = true;
                                            FpToPromote.Sheets[0].Cells[rowValues - 1, col].CellType = txtCell;
                                        }
                                    }
                                }
                            }
                        }
                        FpToPromote.Sheets[0].PageSize = FpToPromote.Sheets[0].RowCount;
                        FpToPromote.SaveChanges();
                        FpToPromote.Height = 600;
                        FpToPromote.Visible = true;

                    }

                    if (dtTempNotPromote.Rows.Count > 0)
                    {
                        Init_Spread(FpFromPromote, 0);
                        foreach (DataRow drStudents in dtTempNotPromote.Rows)
                        {
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
                            string InsType = Convert.ToString(drStudents["InsType"]).Trim();
                            string degreeDetails = Convert.ToString(drStudents["DegreeDetails"]).Trim();
                            string courseName = Convert.ToString(drStudents["Course_Name"]).Trim();
                            string departmentName = Convert.ToString(drStudents["Dept_Name"]).Trim();
                            string departmentAcr = Convert.ToString(drStudents["dept_acronym"]).Trim();
                            string typeName = Convert.ToString(drStudents["type"]).Trim();
                            string eduLevel = Convert.ToString(drStudents["Edu_Level"]).Trim();
                            string duration = Convert.ToString(drStudents["Duration"]).Trim();
                            string IsPromoted = Convert.ToString(drStudents["IsPromoted"]).Trim();

                            txtCell = new Farpoint.TextCellType();
                            chkOneByOne = new Farpoint.CheckBoxCellType();
                            serialNoFrom++;
                            int rowValues = ++FpFromPromote.Sheets[0].RowCount;
                            if (FpFromPromote.Sheets[0].RowCount > 0)
                            {
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Text = Convert.ToString(serialNoFrom).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Tag = Convert.ToString(serialNos).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Note = Convert.ToString(collegeCode).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Text = Convert.ToString(degreeDetails).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Tag = Convert.ToString(degreeCode).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Note = Convert.ToString(departmentAcr).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Text = Convert.ToString(departmentName).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Tag = Convert.ToString(courseName).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Note = Convert.ToString(eduLevel).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Text = Convert.ToString(rollNo).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Tag = Convert.ToString(appNo).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Note = Convert.ToString(batchYear).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Text = Convert.ToString(regNo).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Tag = Convert.ToString(typeName).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Note = Convert.ToString(InsType).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 5].Text = Convert.ToString(rollAdmit).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 6].Text = Convert.ToString(studentType).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 7].Text = Convert.ToString(studentName).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 8].Text = Convert.ToString(currentSemester).Trim();
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 8].Tag = Convert.ToString(duration).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 9].Text = Convert.ToString(section).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 10].Text = Convert.ToString(gender).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 11].Text = Convert.ToString(modeofAdmition).Trim();

                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 12].Value = 0;
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, 12].CellType = chkOneByOne;

                                for (int col = 0; col < FpFromPromote.Sheets[0].ColumnCount; col++)
                                {
                                    FpFromPromote.Sheets[0].Cells[rowValues - 1, col].Font.Name = "Book Antiqua";

                                    FpFromPromote.Sheets[0].Cells[rowValues - 1, col].VerticalAlign = VerticalAlign.Middle;
                                    if (col != 7)
                                    {
                                        FpFromPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpFromPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (col != 12)
                                    {
                                        FpFromPromote.Sheets[0].Cells[rowValues - 1, col].Locked = true;
                                        FpFromPromote.Sheets[0].Cells[rowValues - 1, col].CellType = txtCell;
                                    }
                                }
                            }
                        }
                        FpFromPromote.SaveChanges();
                        FpFromPromote.Sheets[0].PageSize = FpFromPromote.Sheets[0].RowCount;
                        FpFromPromote.Height = 600;
                        FpFromPromote.Visible = true;

                    }
                    //added by prabha jan 24 2018
                    #region Promotion or transfer

                    if (rdb_Promotion.Checked)
                    {
                        FpToPromote.Sheets[0].Visible = false;
                        btnSavePromotion_Click(sender, e);

                        Init_Spread(FpFromPromote, 0);
                        Init_Spread(FpToPromote, 0);
                        btnGo_Click(sender, e);
                        if (lblpromotionconfirm.Text.ToUpper() == "PROMOTED")
                        {
                            lblAlertMsg.Visible = true;
                            lblAlertMsg.Text = "Students are promoted successfully";
                            divPopAlert.Visible = true;
                        }
                        else
                        {
                            lblAlertMsg.Visible = true;
                            lblAlertMsg.Text = "Students are not Promoted";
                            divPopAlert.Visible = true;
                        }
                    }
                    else if (rdb_Transfer.Checked)
                        FpToPromote.Sheets[0].Visible = true;

                    #endregion

                    //Page_Load(sender, e); // modified by prabha on feb 06 2018
                }
                else
                {
                    lblAlertMsg.Text = "Please Select Atleast One Student To Promote";
                    divPopAlert.Visible = true;
                    return;
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
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Click Promote From

    //protected void getCourse()
    //{
    //    string lastvalue = string.Empty;
    //    for (int Row = 0; Row < ddlFromBatch.Items.Count; Row++)
    //    {
    //        lastvalue = ddlFromBatch.Items[Row].Value;
    //    }
    //    string Selsem = Convert.ToString(ddlFromBatch.SelectedValue);
    //    ddlToSem.Enabled = true;
    //    chkpassed.Checked = false;
    //    chkpassed.Enabled = false;
    //    if (Selsem == lastvalue)
    //    {
    //        ddlToSem.Enabled = false;
    //        chkpassed.Enabled = true;
    //    }
    //}

    #region Click Save Promotion

    protected void btnSavePromotion_Click(object sender, EventArgs e)
    {
        try
        {
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            FpFromPromote.SaveChanges();

            collegeCodes = string.Empty;
            collegeNames = string.Empty;
            courseIds = string.Empty;
            courseNames = string.Empty;
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            departmentNames = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;

            qry = string.Empty;
            qryUserOrGroupCode = string.Empty;
            qryCollege = string.Empty;
            qryCollegeName = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;
            bool isSaved = false;
            selected = 0;
            if (ddlToCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCodes = string.Empty;
                selected = 0;
                foreach (ListItem li in ddlToCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
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
                if (selected != 1)
                {
                    lblAlertMsg.Text = "Please Select Only One " + lblToCollege.Text.Trim();
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlToBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYears = string.Empty;
                selected = 0;
                foreach (ListItem li in ddlToBatch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
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
                    qryBatchYear = " and r.batch_year in(" + batchYears + ")";
                }
                if (selected != 1)
                {
                    lblAlertMsg.Text = "Please Select Only One " + lblToBatch.Text.Trim();
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlToDegree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {

            }
            if (ddlToBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCodes = string.Empty;
                selected = 0;
                foreach (ListItem li in ddlToBranch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
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
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
                if (selected != 1)
                {
                    lblAlertMsg.Text = "Please Select Only One " + lblToBranch.Text.Trim();
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlToSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semesters = string.Empty;
                selected = 0;
                foreach (ListItem li in ddlToSem.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
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
                    qrySemester = " and r.current_semester in(" + semesters + ")";
                }
                if (selected != 1)
                {
                    lblAlertMsg.Text = "Please Select Only One " + lblToSem.Text.Trim();
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlToSec.Items.Count > 0)
            {
                sections = string.Empty;
                selected = 0;
                foreach (ListItem li in ddlToSec.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
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
                //if (selected != 1)
                //{
                //    lblAlertMsg.Text = "Please Select Only One " + lblToSec.Text.Trim();
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            else
            {
                sections = string.Empty;
                qrySection = string.Empty;
            }

            if (FpToPromote.Sheets[0].RowCount > 1)
            {
                int serialNo = 0;
                bool isSelected = true;

                qry = "select dg.Degree_Code,dg.Duration,c.Course_Id,dt.Dept_Code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.degree_code in(" + degreeCodes + ")";
                DataSet dsDegreeDetails = da.select_method_wo_parameter(qry, "text");
                for (int row = 1; row < FpToPromote.Sheets[0].RowCount; row++)
                {
                    selected = 0;
                    int.TryParse(Convert.ToString(FpToPromote.Sheets[0].Cells[row, FpToPromote.Sheets[0].ColumnCount - 1].Value).Trim(), out selected);
                    if (selected == 1)
                    {
                        isSelected = true;
                    }
                }
                if (!isSelected)
                {
                    lblAlertMsg.Text = "Please Select Any One Record And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    for (int row = 1; row < FpToPromote.Sheets[0].RowCount; row++)
                    {
                        string rollNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Text).Trim();
                        string appNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Tag).Trim();
                        string regNo = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Text).Trim();
                        string studentName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 7].Text).Trim();
                        string rollAdmit = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 5].Text).Trim();
                        string collegeCode = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 0].Note).Trim();
                        string batchYear = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 3].Note).Trim();
                        string degreeCode = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 1].Tag).Trim();
                        string currentSemester = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 8].Text).Trim();
                        string section = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 9].Text).Trim();
                        string serialNos = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 0].Tag).Trim();
                        string studentType = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 6].Text).Trim();
                        string gender = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 10].Text).Trim();
                        string modeofAdmition = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 11].Text).Trim();
                        string InsType = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Note).Trim();
                        string degreeDetails = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 1].Text).Trim();
                        string courseName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Tag).Trim();
                        string departmentName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Text).Trim();
                        string departmentAcr = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 11].Text).Trim();
                        string typeName = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 4].Tag).Trim();
                        string eduLevel = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 2].Note).Trim();
                        string duration = Convert.ToString(FpToPromote.Sheets[0].Cells[row, 8].Tag).Trim();
                        string admitDate = dirAcc.selectScalarString("select ISNULL(Adm_Date,GETDATE()) from Registration where App_No='" + appNo + "'");
                        string qrySectionToPromote = string.Empty;
                        string qrySectionToPromoteIns = string.Empty;
                        string qrySectionToPromoteUpd = string.Empty;
                        string qrySectionToPromoteCol = string.Empty;
                        if (!string.IsNullOrEmpty(section.Trim().Replace("'", "")))
                        {
                            qrySectionToPromote = " and LTRIM(RTRIM(ISNULL(r.sections,'')))='" + section.Trim().Replace("'", "") + "'";
                            qrySectionToPromoteIns = "'" + section.Trim().Replace("'", "") + "'";
                            qrySectionToPromoteUpd = ",sections='" + section.Trim().Replace("'", "") + "'";
                            qrySectionToPromoteCol = ",sections";
                        }

                        selected = 0;
                        int updateQ = 0;
                        string deptCode = string.Empty;
                        DataView dvDegreeDetails = new DataView();
                        if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                        {
                            dsDegreeDetails.Tables[0].DefaultView.RowFilter = "degree_code='" + degreeCodes.Trim().Replace("'", "") + "'";
                            dvDegreeDetails = dsDegreeDetails.Tables[0].DefaultView;
                        }
                        if (dvDegreeDetails.Count > 0)
                        {
                            deptCode = Convert.ToString(dvDegreeDetails[0]["Dept_Code"]).Trim();
                        }
                        int.TryParse(Convert.ToString(FpToPromote.Sheets[0].Cells[row, FpToPromote.Sheets[0].ColumnCount - 1].Value).Trim(), out selected);
                        if (selected == 1)
                        {

                        }

                        if (!string.IsNullOrEmpty(appNo))
                        {
                            //if (InsType != "0")
                            //{
                            //    qry = "if not exists (select Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id from RegistrationNew  r where r.Batch_Year='" + batchYear.Trim().Replace("'", "") + "' and r.degree_code='" + degreeCode.Trim().Replace("'", "") + "' and r.Current_Semester='" + currentSemester.Trim().Replace("'", "") + "' and r.App_No='" + appNo + "' and r.college_code='" + collegeCode.Trim().Replace("'", "") + "') begin insert into RegistrationNew (Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id) select top 1 Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id from Registration r where r.App_No='" + appNo + "' update Registration set Batch_Year='" + batchYears.Trim().Replace("'", "") + "',college_code='" + collegeCodes.Trim().Replace("'", "") + "' ,degree_code='" + degreeCodes.Trim().Replace("'", "") + "' ,Branch_code='" + deptCode.Trim().Replace("'", "") + "',Current_Semester='" + semesters.Trim().Replace("'", "") + "',mode='1' where App_No='" + appNo + "'  end else update Registration set Batch_Year='" + batchYears.Trim().Replace("'", "") + "',college_code='" + collegeCodes.Trim().Replace("'", "") + "' ,degree_code='" + degreeCodes.Trim().Replace("'", "") + "' ,Branch_code='" + deptCode.Trim().Replace("'", "") + "',Current_Semester='" + semesters.Trim().Replace("'", "") + "',mode='1' where App_No='" + appNo + "'";
                            //    updateQ = da.update_method_wo_parameter(qry, "text");
                            //}
                            //else
                            //{
                            //    //qry = "if not exists (select Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id from RegistrationNew  r where r.Batch_Year='" + batchYear.Trim().Replace("'", "") + "' and r.degree_code='" + degreeCode.Trim().Replace("'", "") + "' and r.Current_Semester='" + currentSemester.Trim().Replace("'", "") + "' and r.App_No='" + appNo + "' and r.college_code='" + collegeCode.Trim().Replace("'", "") + "') begin insert into RegistrationNew (Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id) select top 1 Roll_No,App_No,Current_Year,Current_Semester,Stud_Type,Roll_Admit,RollNo_Flag,CC,DelFlag,Access_Date,Access_Time,Adm_Date,Stud_Name,Reg_No,Batch_Year,Exam_Flag,degree_code,category_code,tcno,tcdate,pass,referby,LockInfo,mode,Advisor,debar_reason,cardno,Sections,serialno,lib_id,JMonth,JYear,Attempts,Rejoin_Status,Regulation,Exam_Elg,Add_Batch,Fingerprint1,Fingerprint2,Fingerprint3,Branch_code,group_code,pinnumber,Mark_serialno,apply_degree_code,cons_date,acr,batch_acr,roll_acr,Status,subdiv,Bus_RouteID,Boarding,Consortium,PIN_No,PIN_Status,college_code,VehID,access_pinno,stud_log_pw,stud_father_pw,stud_mother_pw,Seat_No,DeviceID,Univcode,OldCollegeCode,OldDegree_Code,Trans_PayType,finger_id,Traveller_Date,smart_serial_no,TransferDate,TMRCode,AdmitedDegree,AdmitedYear,DateOfLeaving,AcademicYear,TCSerialNo,isalumni,is_other_due,is_fee_due,IsCanceledStage,Post_Matric_Scholarship,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount,isRedo,SchemeDate,hostel_admission_status,Is_Stud_Staff,staff_appl_id from Registration r where r.App_No='" + appNo + "' update Registration set Batch_Year='" + batchYears.Trim().Replace("'", "") + "',college_code='" + collegeCodes.Trim().Replace("'", "") + "' ,degree_code='" + degreeCodes.Trim().Replace("'", "") + "' ,Branch_code='" + deptCode.Trim().Replace("'", "") + "',Current_Semester='" + semesters.Trim().Replace("'", "") + "' where App_No='" + appNo + "' end else update Registration set Batch_Year='" + batchYears.Trim().Replace("'", "") + "',college_code='" + collegeCodes.Trim().Replace("'", "") + "' ,degree_code='" + degreeCodes.Trim().Replace("'", "") + "' ,Branch_code='" + deptCode.Trim().Replace("'", "") + "',Current_Semester='" + semesters.Trim().Replace("'", "") + "' where App_No='" + appNo + "'";
                            //    qry = "update Registration set Batch_Year='" + batchYears.Trim().Replace("'", "") + "',college_code='" + collegeCodes.Trim().Replace("'", "") + "' ,degree_code='" + degreeCodes.Trim().Replace("'", "") + "' ,Branch_code='" + deptCode.Trim().Replace("'", "") + "',Current_Semester='" + semesters.Trim().Replace("'", "") + "' where App_No='" + appNo + "'";
                            //    updateQ = da.update_method_wo_parameter(qry, "text");
                            //}
                            //@rollNo nvarchar(max), @regNo nvarchar(max), @OldCollegeCode varchar(100), @admitDate datetime, @OldBatchYear varchar(100), @OldDegreeCode varchar(100), @OldSemester varchar(100), @NewCollegeCode varchar(100), @NewBatchYear varchar(100), @NewDegreeCode varchar(100), @NewSemester varchar(100), @studentAppNo int, @OldSection varchar(300)='', @NewSection varchar(300)='', @schoolOrCollege tinyint, @redoType tinyint
                            if (rdb_Transfer.Checked)
                            {
                                Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
                                dicSQLParameter.Clear();
                                dicSQLParameter.Add("@OldCollegeCode", collegeCode.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@OldBatchYear", batchYear.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@OldDegreeCode", degreeCode.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@OldSemester", currentSemester.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@NewCollegeCode", collegeCodes.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@NewBatchYear", batchYears.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@NewDegreeCode", degreeCodes.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@NewSemester", semesters.Trim().Replace("'", ""));
                                dicSQLParameter.Add("@studentAppNo", appNo);
                                dicSQLParameter.Add("@rollNo", rollNo);
                                dicSQLParameter.Add("@regNo", regNo);
                                dicSQLParameter.Add("@OldSection", section.Trim().Replace("'", "").Trim());
                                dicSQLParameter.Add("@NewSection", "");
                                dicSQLParameter.Add("@admitDate", admitDate);
                                dicSQLParameter.Add("@schoolOrCollege", InsType);
                                dicSQLParameter.Add("@redoType", "2");
                                dicSQLParameter.Add("@isPassedOut", ((chkpassed.Checked) ? "1" : "0"));
                                updateQ = storeAcc.updateData("uspStudentPromote", dicSQLParameter);
                            }
                            else if (rdb_Promotion.Checked)//Rajkumar
                            {
                                int semesterto = 0;
                                Int32.TryParse(currentSemester, out semesterto);
                                string maxDuration = da.GetFunction("select distinct NDurations from ndegree where Degree_code='" + degreeCode + " and batch_year='" + batchYear + "");
                                int maxDurations = 0;
                                if (maxDuration.Trim() == "" || maxDuration == null || maxDuration == "0")
                                {
                                    maxDuration = da.GetFunction("select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreeCode + "'");
                                }
                                int.TryParse(maxDuration, out maxDurations);

                                if (semesterto != 0)
                                {
                                    if (semesterto != ddlFromSem.Items.Count && string.IsNullOrEmpty(txtDessem.Text) && txtDessem.Text=="0")
                                        semesterto += 1;
                                    else if (!string.IsNullOrEmpty(txtDessem.Text) && Convert.ToInt32(maxDuration) >= Convert.ToInt32(txtDessem.Text) && txtDessem.Text!="0")
                                        semesterto =Convert.ToInt32(txtDessem.Text);

                                    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
                                    dicSQLParameter.Clear();
                                    dicSQLParameter.Add("@OldCollegeCode", collegeCode.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@OldBatchYear", batchYear.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@OldDegreeCode", degreeCode.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@OldSemester", currentSemester.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@NewCollegeCode", collegeCode.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@NewBatchYear", batchYear.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@NewDegreeCode", degreeCode.Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@NewSemester", Convert.ToString(semesterto).Trim().Replace("'", ""));
                                    dicSQLParameter.Add("@studentAppNo", appNo);
                                    dicSQLParameter.Add("@rollNo", rollNo);
                                    dicSQLParameter.Add("@regNo", regNo);
                                    dicSQLParameter.Add("@OldSection", section.Trim().Replace("'", "").Trim());
                                    dicSQLParameter.Add("@NewSection", "");
                                    dicSQLParameter.Add("@admitDate", admitDate);
                                    dicSQLParameter.Add("@schoolOrCollege", InsType);
                                    dicSQLParameter.Add("@redoType", "2");
                                    dicSQLParameter.Add("@isPassedOut", ((chkpassed.Checked) ? "1" : "0"));
                                    updateQ = storeAcc.updateData("uspStudentPromote", dicSQLParameter);
                                }
                            }

                        }
                        if (updateQ != 0)
                        {
                            isSaved = true;
                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (isSaved)
            {
                if (rdb_Promotion.Checked)
                {
                    lblAlertMsg.Text = "Students Are Promoted Successfully";
                    lblpromotionconfirm.Text = "promoted";
                }
                else if (rdb_Transfer.Checked)
                {
                    lblAlertMsg.Text = "Students Are Transferred Successfully";
                }
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                if (rdb_Promotion.Checked)
                    lblAlertMsg.Text = "Students Are Not Promoted";
                else if (rdb_Transfer.Checked)
                    lblAlertMsg.Text = "Students Are Not Transferred";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Click Save Promotion

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblFromErr.Text = string.Empty;
            lblFromErr.Visible = false;

            collegeCodes = string.Empty;
            collegeNames = string.Empty;
            courseIds = string.Empty;
            courseNames = string.Empty;
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            departmentNames = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;

            qry = string.Empty;
            qryUserOrGroupCode = string.Empty;
            qryCollege = string.Empty;
            qryCollegeName = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;

            Init_Spread(FpFromPromote, 0);
            if (btnPromoteFrom.Text == "Transfer")
                Init_Spread(FpToPromote, 1);
            DataSet dsStudentDetails = new DataSet();
            if (ddlFromCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblFromCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlFromCollege.Items)
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
            if (ddlFromBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblFromBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlFromBatch.Items)
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
                    qryBatchYear = " and r.batch_year in(" + batchYears + ")";
                }
            }
            if (cbl_degree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblFromDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (cbl_branch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblToBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in cbl_branch.Items)
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
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlFromSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblFromSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlFromSem.Items)
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
                    qrySemester = " and r.current_semester in(" + semesters + ")";
                }
            }
            if (ddlFromSec.Items.Count > 0)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlFromSec.Items)
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
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
            {
                DataTable dtDistinctSubjects = new DataTable();
                DataTable dtDistinctStudents = new DataTable();

                string serialno = da.GetFunction("select LinkValue from inssettings where college_code='" + Convert.ToString(ddlFromCollege.SelectedValue).Trim() + "' and linkname='Student Attendance'");
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

                qry = "select r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.App_No,r.serialno,r.Roll_Admit,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,case when a.sex =0 then 'Male' when a.sex=1 then 'Female' else 'Transgender' end as sex,case when r.mode=1 then 'Regular' when r.mode=2 then 'Transfer' when r.mode=3 then 'Lateral' end as Mode,ISNULL(c.InstType,'0') as InsType from Registration r,applyn a,collinfo c where c.college_code=r.college_code and r.App_No=a.app_no  and r.CC='0' and DelFlag='0' and Exam_Flag<>'debar' " + qryCollege + qryBatchYear + qryDegreeCode + qrySemester + " " + orderBy;//order by r.college_code,r.Batch_Year desc,r.degree_code,r.sections
                dsStudentDetails = da.select_method_wo_parameter(qry, "text");//+ qryFromRange + qryToRange +

                qry = "select dg.Degree_Code,dg.Duration,c.Course_Id,dt.Dept_Code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails from Course c,Department dt,Degree dg where c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.degree_code in(" + degreeCodes + ")";
                DataSet dsDegreeDetails = da.select_method_wo_parameter(qry, "text");

                if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
                {
                    int spanColumn = Init_Spread(FpFromPromote, 0);
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    Farpoint.CheckBoxCellType chkSelectAll = new Farpoint.CheckBoxCellType();
                    chkSelectAll.AutoPostBack = true;
                    Farpoint.CheckBoxCellType chkOneByOne = new Farpoint.CheckBoxCellType();
                    int serialNo = 0;
                    foreach (DataRow drStudents in dsStudentDetails.Tables[0].Rows)
                    {
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
                        string InsType = Convert.ToString(drStudents["InsType"]).Trim();

                        string sectionStringqry = dirAcc.selectScalarString("select c.Course_Name+'-'+dept.dept_acronym as Degree from Department dept,Course c,Degree deg where deg.Degree_Code='" + degreeCode + "' and deg.Course_Id=c.Course_Id and deg.college_code=c.college_code and dept.Dept_Code=deg.Dept_Code and dept.college_code=deg.college_code");

                        string New_SectionVal = batchYear + "-" + sectionStringqry + "-" + section;

                        string degreeDetails = string.Empty;
                        string courseName = string.Empty;
                        string departmentName = string.Empty;
                        string departmentAcr = string.Empty;
                        string typeName = string.Empty;
                        string eduLevel = string.Empty;
                        string duration = string.Empty;
                        chkOneByOne = new Farpoint.CheckBoxCellType();
                        //chkOneByOne.Text = "0";
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

                        txtCell = new Farpoint.TextCellType();
                        serialNo++;
                        int rowValues = ++FpFromPromote.Sheets[0].RowCount;
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Text = Convert.ToString(serialNo).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Tag = Convert.ToString(serialNos).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 0].Note = Convert.ToString(collegeCode).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Text = Convert.ToString(degreeDetails).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Tag = Convert.ToString(degreeCode).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 1].Note = Convert.ToString(departmentAcr).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Text = Convert.ToString(departmentName).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Tag = Convert.ToString(courseName).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 2].Note = Convert.ToString(eduLevel).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Text = Convert.ToString(rollNo).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Tag = Convert.ToString(appNo).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 3].Note = Convert.ToString(batchYear).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Text = Convert.ToString(regNo).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Tag = Convert.ToString(typeName).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 4].Note = Convert.ToString(InsType).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 5].Text = Convert.ToString(rollAdmit).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 6].Text = Convert.ToString(studentType).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 7].Text = Convert.ToString(studentName).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 8].Text = Convert.ToString(currentSemester).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 8].Tag = Convert.ToString(duration).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 9].Text = Convert.ToString(New_SectionVal).Trim();
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 9].Tag = Convert.ToString(section).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 10].Text = Convert.ToString(gender).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 11].Text = Convert.ToString(modeofAdmition).Trim();

                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 12].Value = 0;
                        FpFromPromote.Sheets[0].Cells[rowValues - 1, 12].CellType = chkOneByOne;

                        for (int col = 0; col < FpFromPromote.Sheets[0].ColumnCount; col++)
                        {
                            FpFromPromote.Sheets[0].Cells[rowValues - 1, col].Font.Name = "Book Antiqua";

                            FpFromPromote.Sheets[0].Cells[rowValues - 1, col].VerticalAlign = VerticalAlign.Middle;
                            if (col != 7)
                            {
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, col].HorizontalAlign = HorizontalAlign.Left;
                            }
                            if (col != 12)
                            {
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, col].Locked = true;
                                FpFromPromote.Sheets[0].Cells[rowValues - 1, col].CellType = txtCell;
                            }
                        }
                    }
                    FpFromPromote.Sheets[0].PageSize = FpFromPromote.Sheets[0].RowCount;
                    //FpFromPromote.Width= 600;
                    FpFromPromote.Height = 600;
                    FpFromPromote.SaveChanges();
                    FpFromPromote.Visible = true;
                }
                else
                {
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Go Click

    #endregion Button Click

    #region Reusable Method

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
            lbl.Add(lblFromCollege);
            lbl.Add(lblToCollege);
            lbl.Add(lblFromDegree);
            lbl.Add(lblToDegree);
            lbl.Add(lblFromBranch);
            lbl.Add(lblToBranch);
            lbl.Add(lblFromSem);
            lbl.Add(lblToSem);
            fields.Add(0);
            fields.Add(0);
            fields.Add(2);
            fields.Add(2);
            fields.Add(3);
            fields.Add(3);
            fields.Add(4);
            fields.Add(4);
            lblFromBatch.Text = "Batch";
            lblToBatch.Text = "Batch";
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblFromBatch.Text = "Year";
                lblToBatch.Text = "Year";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblFromErr.Text = Convert.ToString(ex);
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            lblFromErr.Text = Convert.ToString(ex).Trim();
            lblFromErr.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlFromCollege.Items.Count > 0) ? Convert.ToString(ddlFromCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    #endregion Reusable Method

    //modified on 13/12/2017
    #region added by prabha for multiselection on Degree and branch

    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                
                
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            BindBranch();
            BindSemester();
            //cb_branch_checkedchange(sender, e);
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
          
            if (seatcount == cbl_degree.Items.Count)
            {
                //txt_degree.Text = lbl_degreeT.Text + "(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            BindBranch();
            BindSemester();
            //cb_branch_checkedchange(sender, e);
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch (" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            BindSemester();
        }
        catch
        {
        }
    }

    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = "Branch (" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else
            {
                txt_branch.Text = "Branch (" + commcount.ToString() + ")";
            }
            BindSemester();
        }
        catch
        {
        }
    }

    #endregion

    //modified on 24 jan 2018
    #region added by prabha for checkbox on promotion and transfer

    protected void rdb_Promotion_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdb_Promotion.Checked)
        {
            btnPromoteFrom.Text = "Promote";
            ddlToCollege.Enabled = false;
            btnSavePromotion.Enabled = false;
            ddlToBatch.Enabled = false;
            ddlToSec.Enabled = false;
            ddlToSem.Enabled = false;
            ddlToDegree.Enabled = false;
            ddlToBranch.Enabled = false;
        }
    }

    protected void rdb_Transfer_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdb_Transfer.Checked)
        {
            btnPromoteFrom.Text = "Transfer";
            ddlToCollege.Enabled = true;
            btnSavePromotion.Enabled = true;
            ddlToBatch.Enabled = true;
            ddlToSec.Enabled = true;
            ddlToSem.Enabled = true;
            ddlToBranch.Enabled = true;
            ddlToDegree.Enabled = true;
            btnSavePromotion.Text = "Save Transfer";
        }
    }

    #endregion
//===================Added by madhumathi for from to range=============//
    protected void Btn_range_Click(object sender, EventArgs e)
    {
        if (txt_frange.Text == "" || txt_trange.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
            return;
        }

        if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
            return;
        }

        for (int i = 0; i < FpFromPromote.Sheets[0].RowCount; i++)
        {
            string sl_no = FpFromPromote.Sheets[0].Cells[i, 0].Text.ToString();

            if (sl_no != "")
            {
                if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                {
                    FpFromPromote.Sheets[0].Cells[i, 12].Value = "1";
                    FpFromPromote.Sheets[0].Cells[i, 12].Locked = false; 

                }
                else
                {
                    FpFromPromote.Sheets[0].Cells[i, 12].Value = "0";
                }
            }
        }

        txt_frange.Text = "";
        txt_trange.Text = "";
    }


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
//=====================================================//
}