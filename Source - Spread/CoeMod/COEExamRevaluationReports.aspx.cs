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


public partial class CoeMod_COEExamRevaluationReports : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    string collegeCode = string.Empty;
    string collegeName = string.Empty;

    string batchYear = string.Empty;
    string batchYearValue = string.Empty;

    string courseId = string.Empty;
    string courseName = string.Empty;

    string departmentName = string.Empty;
    string degreeCode = string.Empty;

    string semester = string.Empty;
    string semesterValue = string.Empty;

    string sectionName = string.Empty;
    string section = string.Empty;

    string examYear = string.Empty;
    string examMonth = string.Empty;
    string examYearName = string.Empty;
    string examMonthName = string.Empty;
    string examMonthYear = string.Empty;

    string reportDate = string.Empty;
    string failShowsResult = string.Empty;
    string collegeHeaderName = string.Empty;
    string officeController = string.Empty;
    string revaluationReportName = string.Empty;
    string degreeDetails = string.Empty;

    string orderBy = string.Empty;
    string collegeCodes = string.Empty;
    string userCode = string.Empty;
    string groupUserCode = string.Empty;
    string singleOrGroupUser = string.Empty;

    string qry = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    string qryCourseId = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryExamYear = string.Empty;
    string qryExamMonth = string.Empty;

    string revaluationReportFor = string.Empty;

    bool isSchool = false;
    bool isSingleUser = true;
    bool isShowFailResult = false;
    bool isShowBySectionWise = false;
    bool isShowNoteDescription = false;
    byte OfficeOrDeptCopy = 0;
    int selected = 0;
    int find_subjrow_count = 0;
    public enum ReValuation
    {
        reTake = 6, reTotal = 2
    }

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
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Attributes.Add("readonly", "readonly");

                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;
                txtExcelName.Text = string.Empty;

                if (ddlOrderby.Items.Count > 0)
                {
                    ddlOrderby.SelectedIndex = 0;
                }
                if (rblOfficeDeptCopy.Items.Count > 0)
                {
                    rblOfficeDeptCopy.Items[0].Selected = true;
                    rblOfficeDeptCopy.Items[1].Selected = false;
                    rblOfficeDeptCopy.Items[2].Selected = false;
                }

                chkShowFailResults.Checked = false;
                divFailValue.Visible = false;
                txtFailValue.Text = string.Empty;

                txtCollegeHeader.Text = string.Empty;
                chkColumnOrderAll.Checked = false;
                txtOrder.Text = string.Empty;
                txtOrder.Visible = false;
                ItemList.Clear();
                Itemindex.Clear();

                string value = string.Empty;
                int index = 0;
                int item = 0;
                value = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    liOrder.Selected = true;
                    liOrder.Enabled = true;
                    item++;
                    if (liOrder.Selected == false)
                    {
                        ItemList.Remove(liOrder.Text);
                        Itemindex.Remove(Convert.ToString(liOrder.Value));
                    }
                    else
                    {
                        if (!Itemindex.Contains(liOrder.Value))
                        {
                            ItemList.Add(liOrder.Text);
                            Itemindex.Add(liOrder.Value);
                        }
                    }
                }
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    if (txtOrder.Text == "")
                    {
                        txtOrder.Text = ItemList[i].ToString();
                    }
                    else
                    {
                        txtOrder.Text = txtOrder.Text + "," + ItemList[i].ToString();
                    }
                }
                if (ItemList.Count == cblColumnOrder.Items.Count)
                {
                    chkColumnOrderAll.Checked = true;
                }
                if (ItemList.Count > 0)
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = true;
                }
                else
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = false;
                }
                Bindcollege();
                //BindBatch();
                BindRightsBaseBatch();
                BindDegree();
                BindBranch();
                BindSem();
                //BindSections();
                BindRightsBasedSectionDetail();
                BindExamYear();
                BindExamMonth();
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
            ddlCollege.Items.Clear();
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds.Clear();
            collegeCodes = string.Empty;

            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
            //--and LTRIM(RTRIM(ISNULL(c.type,''))) in('aided') and r.college_code in(14) and c.Edu_Level in('pg')
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.college_code in(" + collegeCodes + ")  order by r.Batch_Year desc";
                ds = da.select_method_wo_parameter(qry, "Text");
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
        catch
        {
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
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                ds = da.select_method_wo_parameter("select distinct c.course_id,c.course_name,c.Priority,case when c.Priority is null then c.Course_Id else c.Priority end OrderBy from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCollege + columnfield + qryBatch + "  order by case when c.Priority is null then c.Course_Id else c.Priority end", "text");
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCodes = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCodes))
            //    {
            //        qryCollege = " and dg.college_code in(" + collegeCodes + ")";
            //    }
            //}
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
                courseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCourseId))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCourseId + qryCollege + columnfield + qryBatch + "order by dg.Degree_Code", "text");
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSem()
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
            qryBatch = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCodes = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCodes))
            //    {
            //        qryCollege = " and college_code in(" + collegeCodes + ")";
            //    }
            //}
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                //degreeCode = getCblSelectedValue(ddlBranch);
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
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
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSections()
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

    public void BindRightsBasedSectionDetail()
    {
        batchYear = string.Empty;
        collegeCodes = string.Empty;
        degreeCode = string.Empty;
        string sections = string.Empty;

        qrySection = string.Empty;
        qryCollege = string.Empty;
        qryBatch = string.Empty;
        qryDegreeCode = string.Empty;
        ds.Clear();
        ddlSec.Items.Clear();
        txtSec.Enabled = false;
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
        //if (cblCollege.Items.Count > 0)
        //{
        //    collegeCodes = getCblSelectedValue(cblCollege);
        //    if (!string.IsNullOrEmpty(collegeCodes))
        //    {
        //        qryCollege = " and college_code in(" + collegeCodes + ")";
        //    }
        //}
        if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
        {
            collegeCodes = getCblSelectedValue(cblCollege);
        }
        else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
            batchYear = string.Empty;
            foreach (ListItem li in ddlBatch.Items)
            {
                if (li.Selected)
                {
                    if (string.IsNullOrEmpty(batchYear))
                    {
                        batchYear = "'" + li.Text + "'";
                    }
                    else
                    {
                        batchYear += ",'" + li.Text + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatch = " and batch_year in(" + batchYear + ")";
            }
        }
        if (ddlBranch.Items.Count > 0)
        {
            degreeCode = string.Empty;
            foreach (ListItem li in ddlBranch.Items)
            {
                if (li.Selected)
                {
                    if (string.IsNullOrEmpty(degreeCode))
                    {
                        degreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        degreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and degree_code in(" + degreeCode + ")";
            }
        }
        if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryBatch))
        {
            qrySection = da.GetFunctionv("select distinct sections from tbl_attendance_rights where batch_year<>'' " + qryUserOrGroupCode + qryCollege + qryBatch).Trim();
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
        if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryBatch))// && !string.IsNullOrEmpty(qrySection)
        {
            string sqlnew = "select distinct case when isnull(ltrim(rtrim(sections)),'')='' then 'Empty' else isnull(ltrim(rtrim(sections)),'') end sections, isnull(ltrim(rtrim(sections)),'') SecValues from registration where isnull(ltrim(rtrim(sections)),'')<>'-1' and isnull(ltrim(rtrim(sections)),'')<>' ' and delflag=0 and exam_flag<>'Debar' " + qryCollege + qryDegreeCode + qryBatch + qrySection + " order by SecValues";
            ds = da.select_method_wo_parameter(sqlnew, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "SecValues";
            ddlSec.DataBind();
            ddlSec.Enabled = true;

            cblSec.DataSource = ds;
            cblSec.DataTextField = "sections";
            cblSec.DataValueField = "SecValues";
            cblSec.DataBind();

            for (int h = 0; h < cblSec.Items.Count; h++)
            {
                cblSec.Items[h].Selected = true;
            }
            txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
            chkSec.Checked = true;
            txtSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
            txtSec.Enabled = false;
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
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCode))
            //    {
            //        collegeCode = " and dg.college_code in (" + collegeCode + ")";
            //    }
            //}
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
                collegeCodes = " and dg.college_code in (" + collegeCodes + ")";
            }
            if (ddlDegree.Items.Count > 0)
            {
                courseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlSem.Items.Count > 0 && ddlSem.Visible == true)
            {
                semester = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + li.Text + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and ed.current_semester in(" + semester + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + collegeCodes + qryDegreeCode + qryBatch + " order by ed.Exam_year desc";
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
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCode))
            //    {
            //        collegeCode = " and dg.college_code in (" + collegeCode + ")";
            //    }
            //}
            if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
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
                collegeCodes = " and dg.college_code in (" + collegeCodes + ")";
            }
            if (ddlDegree.Items.Count > 0)
            {
                courseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + li.Text + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and ed.current_semester in(" + semester + ")";
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
                    ExamYear = " and ed.Exam_year in (" + ExamYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + collegeCodes + qryBatch + qryDegreeCode + ExamYear + " order by Exam_Month desc";
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
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            Dictionary<string, byte> dicColumnVisiblity = new Dictionary<string, byte>();
            columnVisibility(ref dicColumnVisiblity);
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 6;

                byte value = 0;
                FpSpread1.Sheets[0].Columns[0].Width = 38;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "0", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "1", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "2", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[3].Width = 250;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "3", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[4].Width = 250;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "4", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = getColumnOrderVisiblity(dicColumnVisiblity, "5", out value);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(5, Farpoint.Model.MergePolicy.Always);

                //FpSpread1.Sheets[0].Columns[6].Width = 110;
                //FpSpread1.Sheets[0].Columns[6].Locked = true;
                //FpSpread1.Sheets[0].Columns[6].Resizable = false;
                //FpSpread1.Sheets[0].Columns[6].Visible = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Exam Month & Year";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
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
            //BindBatch();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            //BindSections();
            BindRightsBasedSectionDetail();
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
            //BindBatch();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            //BindSections();
            BindRightsBasedSectionDetail();
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
            BindSem();
            //BindSections();
            BindRightsBasedSectionDetail();
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
            //BindSections();
            BindRightsBasedSectionDetail();
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
            BindSem();
            //BindSections();
            BindRightsBasedSectionDetail();
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

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSem();
            //BindSections();
            BindRightsBasedSectionDetail();
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
            //BindSections();
            BindRightsBasedSectionDetail();
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

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
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

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        divPopAlert.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        divMainContents.Visible = false;
        CallCheckboxChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
        BindExamYear();
        BindExamMonth();
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        divPopAlert.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        divMainContents.Visible = false;
        CallCheckboxListChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
        BindExamYear();
        BindExamMonth();
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

    protected void chkShowFailResults_CheckedChanged(object sender, EventArgs e)
    {
        divFailValue.Visible = false;
        txtFailValue.Text = string.Empty;
        if (chkShowFailResults.Checked)
        {
            divFailValue.Visible = true;
            txtFailValue.Text = "F";
        }
    }

    #endregion Index Changed Events

    #region Column Order

    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkColumnOrderAll.Checked == true)
            {
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    liOrder.Selected = true;
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    lbtnRemoveAll.Visible = true;
                    ItemList.Add(Convert.ToString(liOrder.Text).Trim());
                    Itemindex.Add(liValue);
                    //switch (liValue)
                    //{
                    //    case "5":
                    //        if (!chkgender.Checked)
                    //        {
                    //            liOrder.Enabled = false;
                    //            liOrder.Selected = false;
                    //            ItemList.Remove(liOrder.Text);
                    //            Itemindex.Remove(liOrder.Value);
                    //        }
                    //        break;
                    //    case "10":
                    //    case "13":
                    //        if (!chkgrade.Checked)
                    //        {
                    //            liOrder.Enabled = false;
                    //            liOrder.Selected = false;
                    //            ItemList.Remove(liOrder.Text);
                    //            Itemindex.Remove(liOrder.Value);
                    //        }
                    //        break;
                    //    case "14":
                    //        if (!chk_subjectwisegrade.Checked)
                    //        {
                    //            liOrder.Enabled = false;
                    //            liOrder.Selected = false;
                    //            if (Itemindex.Contains(liValue))
                    //            {
                    //                ItemList.Remove(liOrder.Text);
                    //                Itemindex.Remove(liOrder.Value);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            liOrder.Enabled = true;
                    //            liOrder.Selected = true;
                    //            if (!Itemindex.Contains(liValue))
                    //            {
                    //                ItemList.Add(liOrder.Text);
                    //                Itemindex.Add(liOrder.Value);
                    //            }
                    //        }
                    //        break;
                    //    case "6":
                    //        if (chkshowsub_name.Checked)
                    //            liOrder.Text = "Subject Name";
                    //        else
                    //            liOrder.Text = "Subject Code";
                    //        break;
                    //    default:
                    //        liOrder.Selected = true;
                    //        liOrder.Enabled = true;
                    //        break;
                    //}
                }
                lbtnRemoveAll.Visible = true;
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                int j = 0;
                string colname12 = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                }
                txtOrder.Text = colname12;
            }
            else
            {
                ItemList.Clear();
                Itemindex.Clear();
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    liOrder.Selected = false;
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    //switch (liValue)
                    //{
                    //    case "5":
                    //        if (!chkgender.Checked)
                    //        {
                    //            liOrder.Enabled = false;
                    //            liOrder.Selected = false;
                    //        }
                    //        break;
                    //    case "10":
                    //    case "13":
                    //    case "14":
                    //        if (!chkgrade.Checked)
                    //        {
                    //            liOrder.Enabled = false;
                    //            liOrder.Selected = false;
                    //        }
                    //        break;
                    //    case "6":
                    //        if (chkshowsub_name.Checked)
                    //            liOrder.Text = "Subject Name";
                    //        else
                    //            liOrder.Text = "Subject Code";
                    //        break;
                    //}
                }
                lbtnRemoveAll.Visible = false;
                txtOrder.Text = string.Empty;
                txtOrder.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lbtnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            cblColumnOrder.ClearSelection();
            chkColumnOrderAll.Checked = false;
            lbtnRemoveAll.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            txtOrder.Text = string.Empty;
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblColumnOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkColumnOrderAll.Checked = false;
            string value = string.Empty;
            int index;
            //cblColumnOrder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index).Trim();
            if (cblColumnOrder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i).Trim();
                    ItemList.Remove(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Remove(sindex);
                }
            }
            lbtnRemoveAll.Visible = true;
            txtOrder.Visible = false;
            txtOrder.Text = string.Empty;
            string colname12 = string.Empty;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
            }
            txtOrder.Text = colname12;
            if (ItemList.Count == 14)
            {
                chkColumnOrderAll.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

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

    #region Button Events

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
            collegeName = string.Empty;

            batchYear = string.Empty;
            batchYearValue = string.Empty;

            courseId = string.Empty;
            courseName = string.Empty;

            degreeCode = string.Empty;
            departmentName = string.Empty;

            semester = string.Empty;
            semesterValue = string.Empty;

            section = string.Empty;
            sectionName = string.Empty;

            examYear = string.Empty;
            examYearName = string.Empty;
            examMonthName = string.Empty;
            examMonth = string.Empty;
            examMonthYear = string.Empty;

            failShowsResult = string.Empty;
            Dictionary<string, byte> dicColumnVisiblity = new Dictionary<string, byte>();
            columnVisibility(ref dicColumnVisiblity);
            selected = 0;
            revaluationReportFor = string.Empty;
            collegeHeaderName = string.Empty;
            officeController = string.Empty;
            //officeController = Convert.ToString("OFFICE OF THE CONTROLLER OF EXAMINATIONS").Trim().ToUpper();
            revaluationReportName = string.Empty;
            revaluationReportName = Convert.ToString("DEGREE EXAMINATIONS\t\t-\t\tREVALUATION RESULT").Trim().ToUpper();
            degreeDetails = string.Empty;


            isShowBySectionWise = false;
            isShowFailResult = false;
            isShowNoteDescription = false;
            OfficeOrDeptCopy = 0;
            selected = 0;

            bool isDepartmentCopy = false;
            bool isRegularResult = false;

            string selectedValue = string.Empty;
            string selectedText = string.Empty;
            int selectedIndex = 0;

            DataSet dsRevalAppliedPapers = new DataSet();
            DataSet dsRevalMarkList = new DataSet();

            DataTable dtRevalAppliedStudents = new DataTable();

            if (chkShowFailResults.Checked)
            {
                isShowFailResult = true;
                if (!string.IsNullOrEmpty(txtFailValue.Text.Trim()))
                {
                    failShowsResult = txtFailValue.Text.Trim();
                }
            }
            else
            {
                failShowsResult = string.Empty;
                isShowFailResult = false;
            }
            if (chkShowNoteDescription.Checked)
            {
                isShowNoteDescription = true;
            }
            else
            {
                isShowNoteDescription = false;
            }
            if (chkShowsSectionWise.Checked)
            {
                isShowBySectionWise = true;
            }
            else
            {
                isShowBySectionWise = false;
            }
            if (rblOfficeDeptCopy.Items.Count > 0)
            {
                selectedValue = Convert.ToString(rblOfficeDeptCopy.SelectedValue).Trim();
                selectedText = Convert.ToString(rblOfficeDeptCopy.SelectedItem.Text).Trim();
                selectedIndex = rblOfficeDeptCopy.SelectedIndex;
                switch (selectedIndex)
                {
                    case 0:
                        OfficeOrDeptCopy = 0;
                        isDepartmentCopy = false;
                        revaluationReportFor = string.Empty;
                        break;
                    case 1:
                        OfficeOrDeptCopy = 1;
                        isDepartmentCopy = false;
                        revaluationReportFor = "(OFFICE COPY)";
                        break;
                    case 2:
                        OfficeOrDeptCopy = 2;
                        isDepartmentCopy = true;
                        revaluationReportFor = "(DEPT. COPY)";
                        break;
                    default:
                        OfficeOrDeptCopy = 0;
                        isDepartmentCopy = false;
                        revaluationReportFor = string.Empty;
                        break;
                }
            }
            selected = 0;
            Boolean flag_stud_u = false;
            Boolean flag_subj_rowcnt = false;
            if (cblCollege.Items.Count == 0 && ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else if (cblCollege.Items.Count > 0 && txtCollege.Visible == true)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                collegeName = string.Empty;
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
                if (collegeCodes.Split(',').Length == 1)
                {
                    collegeName = getCblSelectedText(cblCollege);
                }

            }
            else if (ddlCollege.Items.Count > 0 && ddlCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                collegeName = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblCollege.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (collegeCodes.Split(',').Length == 1 && selected == 1)
                {
                    collegeName = Convert.ToString(ddlCollege.SelectedItem.Text).Trim();
                }
            }
            if (!string.IsNullOrEmpty(txtCollegeHeader.Text.Trim()))
            {
                collegeHeaderName = Convert.ToString(txtCollegeHeader.Text.Trim()).Trim();
            }
            else
            {
                collegeHeaderName = string.Empty;
                if (!string.IsNullOrEmpty(collegeName))
                {
                    collegeHeaderName = Convert.ToString(collegeName).Trim();
                }
            }
            selected = 0;
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                batchYearValue = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and r.Batch_year in(" + batchYear + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (batchYear.Split(',').Length == 1 && selected == 1)
                {
                    batchYearValue = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            selected = 0;
            if (ddlDegree.Items.Count > 0)
            {
                courseId = string.Empty;
                courseName = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (courseId.Split(',').Length == 1 && selected == 1)
                {
                    courseName = Convert.ToString(ddlDegree.SelectedItem.Text).Trim();
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            selected = 0;
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                departmentName = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (degreeCode.Split(',').Length == 1 && selected == 1)
                {
                    departmentName = Convert.ToString(ddlBranch.SelectedItem.Text).Trim();
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            selected = 0;
            if (ddlSem.Items.Count > 0)
            {
                semester = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + li.Text + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and sm.semester in(" + semester + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (semester.Split(',').Length == 1 || selected == 1)
                {
                    semesterValue = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            selected = 0;
            if (cblSec.Items.Count == 0 && ddlSec.Items.Count == 0)
            {
                //lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                //divPopAlert.Visible = true;
                //return;
            }
            else if (cblSec.Items.Count > 0 && txtSec.Visible == true)
            {
                section = getCblSelectedValue(cblSec);
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.Sections,''))) in(" + section + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblSec.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
                if (section.Split(',').Length == 1)
                {
                    sectionName = section;
                }
            }
            else if (ddlSec.Items.Count > 0 && ddlSec.Visible == true)
            {
                section = string.Empty;
                foreach (ListItem li in ddlSec.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(li.Value.Trim().ToLower()) && li.Value.Trim().ToLower() != "all" && li.Value.Trim().ToLower() != "0" && li.Value.Trim().ToLower() != "-1")
                        {
                            selected++;
                            if (string.IsNullOrEmpty(section))
                            {
                                section = "'" + li.Value + "'";
                            }
                            else
                            {
                                section += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.Sections,''))) in(" + section + ")";
                }
                else
                {
                    //lblAlertMsg.Text = "Please Select " + lblSec.Text.Trim() + " And Then Proceed";
                    //divPopAlert.Visible = true;
                    //return;
                }
                if (section.Split(',').Length == 1 && selected == 1)
                {
                    sectionName = Convert.ToString(ddlSec.SelectedItem.Value).Trim();
                }
            }
            selected = 0;
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
                        selected++;
                        if (!string.IsNullOrEmpty(examYear))
                        {
                            examYear += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            examYear = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and ed.Exam_Year in(" + examYear + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamYear.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (examYear.Split(',').Length == 1 && selected == 1)
                {
                    examYearName = Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                }
            }
            selected = 0;
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
                        selected++;
                        if (!string.IsNullOrEmpty(examMonth))
                        {
                            examMonth += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            examMonth = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in(" + examMonth + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamMonth.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
                if (examMonth.Split(',').Length == 1 && selected == 1)
                {
                    examMonthName = Convert.ToString(ddlExamMonth.SelectedItem.Text).Trim();
                }
            }
            orderBy = string.Empty;
            if (ddlOrderby.Items.Count > 0)
            {
                selectedIndex = ddlOrderby.SelectedIndex;
                selectedText = Convert.ToString(ddlOrderby.SelectedItem.Text).Trim();
                selectedValue = Convert.ToString(ddlOrderby.SelectedItem.Value).Trim();
                switch (selectedIndex)
                {
                    case 0:
                        orderBy = "asc";
                        break;
                    case 1:
                        orderBy = "desc";
                        break;
                    default:
                        orderBy = "asc";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(examMonth) && !string.IsNullOrEmpty(examYear) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryExamMonth) && !string.IsNullOrEmpty(qryExamYear))
            {
                string qry = "select clg.collname,c.Edu_Level,c.Course_Name,dt.Dept_Name,ltrim(rtrim(ISNULL(c.type,''))) as Type,'Class :'+c.Course_Name+' '+dt.Dept_Name as Degree_Details from collinfo clg,Course c,Degree dg,Department dt where c.college_code=clg.college_code and clg.college_code=dg.college_code and  clg.college_code=dt.college_code and dt.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'";
                DataSet dsDegreeDetails = new DataSet();
                dsDegreeDetails = da.select_method_wo_parameter(qry, "text");


                spnCollegeHeader.InnerHtml = collegeHeaderName;
                officeController = Convert.ToString("OFFICE OF THE CONTROLLER OF EXAMINATIONS").Trim().ToUpper();
                spnOfficeController.InnerHtml = officeController;
                examMonthYear = examMonthName + " " + examYearName;
                //spnExamYearMonth.InnerHtml = examMonthYear;
                degreeDetails = Convert.ToString("CLASS\t\t\t\t:\t\t\t\t").Trim().ToUpper() + "\t\t\t\t" + batchYearValue + "\t\t\t\t" + courseName + "\t\t\t\t" + departmentName + "\t\t\t\t" + ((string.IsNullOrEmpty(sectionName)) ? "" : "\t\t\t\t'" + sectionName + "'" + "\t\t\t\tSection" + " ( " + dsDegreeDetails.Tables[0].Rows[0]["Type"]+" ) ");
                //degreeDetails+=
                spnExamYearMonth.InnerHtml ="REVALUATION RESULT"+"\t\t\t\t\t\t" + examMonthYear;
                spnDegreeDetails.InnerHtml = degreeDetails;
                spnSemester.InnerHtml = "Semester\t\t:\t\t" + ddlSem.SelectedItem.Text;
                int typeValu = (int)(ReValuation.reTotal);
                qry = "select r.college_code,r.Batch_Year,r.degree_code,r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,r.Stud_Type,case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transend' else 'Other' end as Gender,a.dob,sm.semester,s.subject_no,s.subject_code,s.subject_name,s.subjectpriority from Registration r,applyn a,Exam_Details ed,exam_appl_details ead,exam_application ea,subject s,syllabus_master sm where a.App_No=r.App_No and r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and ed.Batch_Year=r.Batch_Year and ed.degree_code=r.degree_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and  r.roll_no=ea.roll_no  and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no  and ead.subject_no=s.subject_no and sm.syll_code=s.syll_code  and ed.exam_code=ea.exam_code and ea.Exam_type='" + typeValu + "' " + qryCollege + qryBatch + qryDegreeCode + qrySection + qryExamMonth + qryExamYear + " order by r.college_code,r.Batch_Year desc,r.degree_code,Sections,r.Reg_No,sm.semester " + orderBy + ",s.subjectpriority";//and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "'
                dsRevalAppliedPapers = da.select_method_wo_parameter(qry, "text");
                dtRevalAppliedStudents.Clear();
                qry = "select r.college_code,r.Batch_Year,r.degree_code,r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,LTRIM(RTRIM(ISNULL(r.Sections,''))) Sections,r.Stud_Type,sm.semester,s.subject_no,s.subject_code,s.subject_name,s.subjectpriority,s.credit_points,s.min_int_marks,s.max_int_marks,m.internal_mark,m.actual_internal_mark,s.min_ext_marks,s.max_ext_marks,m.external_mark,m.actual_external_mark,s.mintotal,s.maxtotal,m.total,m.actual_total,m.grade,m.Actual_Grade,m.attempts,m.evaluation1,m.evaluation2,m.evaluation3,m.exam_code,m.Act_Reval_Mark,m.result,m.passorfail,LTRIM(RTRIM(ISNULL(m.revaluation_1,''))) as revaluation_1,LTRIM(RTRIM(ISNULL(m.revaluation_2,''))) as revaluation_2,LTRIM(RTRIM(ISNULL(m.revaluation_3,''))) as revaluation_3 from Registration r,Exam_Details ed,exam_appl_details ead,exam_application ea,mark_entry m,subject s,syllabus_master sm where r.degree_code=sm.degree_code and sm.Batch_Year=r.Batch_Year and ed.Batch_Year=r.Batch_Year and ed.degree_code=r.degree_code and ed.batch_year=sm.Batch_Year and sm.degree_code=ed.degree_code and m.roll_no=r.roll_no and m.roll_no=ea.roll_no and r.roll_no=ea.roll_no and ea.roll_no=m.roll_no and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and m.subject_no=s.subject_no and ead.subject_no=s.subject_no and sm.syll_code=s.syll_code and ea.exam_code=m.exam_code and m.exam_code=ed.exam_code and ed.exam_code=ea.exam_code and ea.Exam_type='" + typeValu + "' " + qryCollege + qryBatch + qryDegreeCode + qrySection + qryExamMonth + qryExamYear + " order by r.college_code,r.Batch_Year desc,r.degree_code,Sections,r.Reg_No,sm.semester " + orderBy + ",s.subjectpriority";
                //from Registration r,applyn a,Exam_Details ed,exam_appl_details ead,exam_application ea,mark_entry m,subject s,syllabus_master sm where a.app_no=r.App_No and r.Roll_No=ea.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and r.Roll_No=m.roll_no and m.roll_no=ea.roll_no and s.subject_no=m.subject_no and m.subject_no=ead.subject_no and ead.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year=ed.batch_year and sm.degree_code=ed.degree_code and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code 

                dsRevalMarkList = da.select_method_wo_parameter(qry, "text");
                if (rblformat.SelectedIndex == 0)
                {
                    if (dsRevalAppliedPapers.Tables.Count > 0 && dsRevalAppliedPapers.Tables[0].Rows.Count > 0)
                    {
                        dtRevalAppliedStudents = dsRevalAppliedPapers.Tables[0].DefaultView.ToTable(true, "college_code", "Batch_Year", "degree_code", "App_No", "Roll_No", "Reg_No", "Roll_Admit", "Stud_Name", "Sections", "Stud_Type", "Gender", "dob");
                        if (dsRevalMarkList.Tables.Count > 0 && dsRevalMarkList.Tables[0].Rows.Count > 0)
                        {
                            if (dtRevalAppliedStudents.Rows.Count > 0)
                            {
                                //Init_Spread(FpRevaluation, type: 0);
                                function_load_header();
                                FpRevaluation.Visible = true;
                                //FpRevaluationHeader.Visible = true;
                                FpRevaluation.Sheets[0].RowCount = 0;
                                FpRevaluation.Sheets[0].RowHeader.Visible = false;
                                FpRevaluation.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                                FpRevaluation.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Large;
                                FpRevaluation.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                int serialNo = 0;
                                int subjectColumn = FpRevaluation.Sheets[0].ColumnCount;
                                int maxColumn = subjectColumn;
                                foreach (DataRow drStudents in dtRevalAppliedStudents.Rows)
                                {
                                    DataTable dtStudentMark = new DataTable();
                                    string studentName = string.Empty;
                                    string rollNo = string.Empty;
                                    string regNo = string.Empty;
                                    string appNo = string.Empty;
                                    string rollAdmit = string.Empty;
                                    string gender = string.Empty;
                                    string studentType = string.Empty;
                                    string sectionNew = string.Empty;
                                    string batchYearNew = string.Empty;
                                    string dob = string.Empty;
                                    string collegeCode = string.Empty;
                                    string degreeCodeNew = string.Empty;

                                    studentName = Convert.ToString(drStudents["Stud_Name"]).Trim();
                                    rollNo = Convert.ToString(drStudents["Roll_No"]).Trim();
                                    regNo = Convert.ToString(drStudents["Reg_No"]).Trim();
                                    appNo = Convert.ToString(drStudents["App_No"]).Trim();
                                    rollAdmit = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                                    gender = Convert.ToString(drStudents["Gender"]).Trim();
                                    studentType = Convert.ToString(drStudents["Stud_Type"]).Trim();
                                    sectionNew = Convert.ToString(drStudents["Sections"]).Trim();
                                    batchYearNew = Convert.ToString(drStudents["Batch_Year"]).Trim();
                                    dob = Convert.ToString(drStudents["dob"]).Trim();
                                    collegeCode = Convert.ToString(drStudents["college_code"]).Trim();
                                    degreeCodeNew = Convert.ToString(drStudents["degree_code"]).Trim();

                                    dsRevalMarkList.Tables[0].DefaultView.RowFilter = "App_No='" + appNo + "'";
                                    dtStudentMark = dsRevalMarkList.Tables[0].DefaultView.ToTable();
                                    int starColumn = subjectColumn;
                                    int startColumn = 0;
                                    if (dtStudentMark.Rows.Count > 0)
                                    {
                                        //if (serialNo == 0)
                                        //{
                                        //    starColumn = FpRevaluation.Sheets[0].ColumnCount += 6;
                                        //    startColumn = starColumn - 6;
                                        //    maxColumn = starColumn;
                                        //}
                                        //else
                                        //{
                                        //    startColumn = starColumn;
                                        //}


                                        serialNo++;
                                        //FpRevaluation.Sheets[0].RowCount += 2;
                                        Farpoint.TextCellType txtCell = new Farpoint.TextCellType();

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(serialNo).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].Tag = Convert.ToString(serialNo).Trim(); //Convert.ToString(courseType).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].Note = Convert.ToString(serialNo).Trim(); //Convert.ToString(eduLevel).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].Locked = false;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 0, 2, 1);

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].Text = Convert.ToString(rollNo).Trim();
                                        ////FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(eduLevel).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].Note = Convert.ToString(courseName).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].Locked = true;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 1].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 1, 2, 1);

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].Text = Convert.ToString(regNo).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].Tag = Convert.ToString(collegeCode).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].Note = Convert.ToString(degreeCode).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].Locked = true;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Left;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 2].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 2, 2, 1);

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].Text = Convert.ToString(studentName).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].Tag = Convert.ToString(batchYear).Trim();
                                        ////FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(currentSemesterNew).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].Locked = true;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Center;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 3].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 3, 2, 1);

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Text = Convert.ToString(studentType).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Tag = Convert.ToString(degreeCode).Trim();
                                        ////FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Note = Convert.ToString(examCode).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Locked = true;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Center;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 4, 2, 1);

                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].Text = Convert.ToString(gender).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].Tag = Convert.ToString(degreeCode).Trim();
                                        ////FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 4].Note = Convert.ToString(examCode).Trim();
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].CellType = txtCell;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].Font.Name = "Book Antiqua";
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].Locked = true;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Center;
                                        //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, 5].VerticalAlign = VerticalAlign.Middle;
                                        //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, 5, 2, 1);

                                        int studRowCount = FpRevaluation.Sheets[0].RowCount++;

                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Text = "  " + Convert.ToString(studentName).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].Text = gender;
                                        int sub_increment = 1;
                                        int subIncrementCount = 11;
                                        int spanRowCount = 1;
                                        int subject = 0;
                                        DataTable dtSubjectList = new DataTable();
                                        dtSubjectList = dtStudentMark.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name");
                                        //foreach (DataRow drSubject in dtSubjectList.Rows)
                                        //{
                                        Boolean row = false;
                                        for (int subcode = 0; subcode < dtSubjectList.Rows.Count; subcode++)
                                        {
                                            string subjectNo = string.Empty;
                                            string subjectCode = string.Empty;
                                            string subjectName = string.Empty;
                                            subjectNo = Convert.ToString(dtSubjectList.Rows[subcode]["subject_no"]).Trim();
                                            subjectCode = Convert.ToString(dtSubjectList.Rows[subcode]["subject_code"]).Trim();
                                            subjectName = Convert.ToString(dtSubjectList.Rows[subcode]["subject_name"]).Trim();

                                            DataView dvSubMarks = new DataView();
                                            dtStudentMark.DefaultView.RowFilter = "subject_no='" + subjectNo + "'";
                                            dvSubMarks = dtStudentMark.DefaultView;
                                            dvSubMarks.Sort = "total desc,external_mark desc,internal_mark desc";
                                            if (dvSubMarks.Count > 0)
                                            {
                                                DataRowView drMarks = dvSubMarks[0];
                                                string internalMark = string.Empty;
                                                string minInternalMark = string.Empty;
                                                string MaxInternalMark = string.Empty;
                                                string actualInternalMark = string.Empty;

                                                string externalMark = string.Empty;
                                                string minExternalMark = string.Empty;
                                                string MaxExternalMark = string.Empty;
                                                string actualExternalMark = string.Empty;

                                                string totalMark = string.Empty;
                                                string minTotalMark = string.Empty;
                                                string MaxTotalMark = string.Empty;
                                                string actualTotalMark = string.Empty;

                                                string grade = string.Empty;
                                                string actualGrade = string.Empty;

                                                string evaluation1 = string.Empty;
                                                string evaluation2 = string.Empty;
                                                string evaluation3 = string.Empty;

                                                string passOrFail = string.Empty;
                                                string result = string.Empty;
                                                string actualResult = string.Empty;

                                                string attempt = string.Empty;
                                                string subjectSemester = string.Empty;


                                                string creditPoint = string.Empty;

                                                internalMark = Convert.ToString(drMarks["internal_mark"]).Trim();
                                                minInternalMark = Convert.ToString(drMarks["min_int_marks"]).Trim();
                                                MaxInternalMark = Convert.ToString(drMarks["max_int_marks"]).Trim();
                                                actualInternalMark = Convert.ToString(drMarks["actual_internal_mark"]).Trim();

                                                externalMark = Convert.ToString(drMarks["external_mark"]).Trim();
                                                minExternalMark = Convert.ToString(drMarks["min_ext_marks"]).Trim();
                                                MaxExternalMark = Convert.ToString(drMarks["max_ext_marks"]).Trim();
                                                actualExternalMark = Convert.ToString(drMarks["actual_external_mark"]).Trim();

                                                totalMark = Convert.ToString(drMarks["total"]).Trim();
                                                minTotalMark = Convert.ToString(drMarks["mintotal"]).Trim();
                                                MaxTotalMark = Convert.ToString(drMarks["maxtotal"]).Trim();
                                                actualTotalMark = Convert.ToString(drMarks["actual_total"]).Trim();

                                                grade = Convert.ToString(drMarks["grade"]).Trim();
                                                actualGrade = Convert.ToString(drMarks["Actual_Grade"]).Trim();

                                                evaluation1 = Convert.ToString(drMarks["evaluation1"]).Trim();
                                                evaluation2 = Convert.ToString(drMarks["evaluation2"]).Trim();
                                                evaluation3 = Convert.ToString(drMarks["evaluation3"]).Trim();

                                                passOrFail = Convert.ToString(drMarks["passorfail"]).Trim();
                                                result = Convert.ToString(drMarks["result"]).Trim();
                                                actualResult = Convert.ToString(drMarks["result"]).Trim();

                                                string examCode = Convert.ToString(drMarks["exam_code"]).Trim();
                                                string actualRevalMArk = Convert.ToString(drMarks["Act_Reval_Mark"]).Trim();

                                                attempt = Convert.ToString(drMarks["attempts"]).Trim();
                                                subjectSemester = Convert.ToString(drMarks["semester"]).Trim();
                                                subjectNo = Convert.ToString(drMarks["subject_no"]).Trim();
                                                subjectCode = Convert.ToString(drMarks["subject_code"]).Trim();
                                                subjectName = Convert.ToString(drMarks["subject_name"]).Trim();
                                                creditPoint = Convert.ToString(drMarks["credit_points"]).Trim();

                                                double internalMarksValue = 0;
                                                double externalMarksValue = 0;
                                                double totalMarksValue = 0;
                                                double.TryParse(internalMark, out internalMarksValue);
                                                double.TryParse(externalMark, out externalMarksValue);
                                                double.TryParse(totalMark, out totalMarksValue);

                                                bool externalOnly = false;
                                                bool internalOnly = false;

                                                //string totalValues = totalMark;
                                                string totalValues = (string.IsNullOrEmpty(totalMark) ? "--" : ((totalMarksValue == -1) ? "AA" : (totalMarksValue == -2) ? "NE" : (totalMarksValue == -3) ? "NR" : (totalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(totalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string internalValues = (string.IsNullOrEmpty(internalMark) ? "--" : ((internalMarksValue == -1) ? "AA" : (internalMarksValue == -2) ? "NE" : (internalMarksValue == -3) ? "NR" : (internalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(internalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string externalValues = (string.IsNullOrEmpty(externalMark) ? "--" : ((externalMarksValue == -1) ? "AA" : (externalMarksValue == -2) ? "NE" : (externalMarksValue == -3) ? "NR" : (externalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(externalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                //string showInternalMark = internalMark;
                                                //string showExternalMark = externalMark;
                                                string showInternalMark = internalValues;
                                                string showExternalMark = externalValues;
                                                string showTotalMark = totalValues;
                                                string showingResult = string.Empty;

                                                //if (internalMarksValue == -1)
                                                //{
                                                //    showInternalMark = "AA";
                                                //}
                                                //else if (internalMarksValue == -2)
                                                //{
                                                //    showInternalMark = "NE";
                                                //}
                                                //else if (internalMarksValue == -3)
                                                //{
                                                //    showInternalMark = "NR";
                                                //}
                                                //else if (internalMarksValue == -4)
                                                //{
                                                //    showInternalMark = "L";
                                                //}
                                                //else
                                                //{
                                                //    showInternalMark = internalMark;
                                                //}

                                                //if (externalMarksValue == -1)
                                                //{
                                                //    showExternalMark = "AA";
                                                //}
                                                //else if (externalMarksValue == -2)
                                                //{
                                                //    showExternalMark = "NE";
                                                //}
                                                //else if (externalMarksValue == -3)
                                                //{
                                                //    showExternalMark = "NR";
                                                //}
                                                //else if (externalMarksValue == -4)
                                                //{
                                                //    showExternalMark = "L";
                                                //}
                                                //else
                                                //{
                                                //    showExternalMark = externalMark;
                                                //}

                                                if (string.IsNullOrEmpty(result.Trim().ToLower()))
                                                {
                                                    showingResult = "--";
                                                }
                                                else if (result.Trim().ToLower().Contains("pass"))
                                                {
                                                    showingResult = "P";
                                                }
                                                else if (result.Trim().ToLower().Contains("f"))
                                                {
                                                    if (isShowFailResult)
                                                    {
                                                        showingResult = failShowsResult;
                                                    }
                                                }
                                                else if (result.ToLower().Trim().Contains("aaa") || result.ToLower().Trim().Contains("aa") || result.ToLower().Trim().Contains("ab"))
                                                {
                                                    showingResult = "AA";
                                                }
                                                else
                                                {
                                                    showingResult = result.Trim();
                                                }

                                                if (string.IsNullOrEmpty(MaxInternalMark) || MaxInternalMark.Trim() == "0")
                                                {
                                                    externalOnly = true;
                                                    showInternalMark = "--";
                                                    if (externalMarksValue < 0)
                                                    {
                                                        totalValues = "--";
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(MaxExternalMark) || MaxExternalMark.Trim() == "0")
                                                {
                                                    internalOnly = true;
                                                    showExternalMark = "--";
                                                    if (internalMarksValue < 0)
                                                    {
                                                        totalValues = "--";
                                                    }
                                                }
                                                if (internalOnly)
                                                {
                                                    if (internalMarksValue == -1)
                                                    {
                                                        showingResult = "RA";
                                                    }
                                                }
                                                else if (externalOnly)
                                                {
                                                    if (externalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                    else if (externalMarksValue == -1 || totalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                }
                                                else
                                                {
                                                    if (internalMarksValue == -1 && (externalMarksValue >= 0 && totalMarksValue >= 0))
                                                    {
                                                        showingResult = "RA";
                                                    }
                                                    else if (externalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                    else if (externalMarksValue == -1 || totalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                }
                                                if (subcode % 3 == 0 & subcode != 0)
                                                {
                                                    FpRevaluation.Sheets[0].RowCount++;
                                                }
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Text = "  " + Convert.ToString(studentName).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].Text = gender;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 6 + sub_increment].CellType = new TextCellType();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 7 + sub_increment].CellType = new TextCellType();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 8 + sub_increment].CellType = new TextCellType();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5 + sub_increment].Text = Convert.ToString(subjectCode).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 6 + sub_increment].Text = Convert.ToString(showInternalMark).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 7 + sub_increment].Text = Convert.ToString(showExternalMark).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 8 + sub_increment].Text = Convert.ToString(showTotalMark).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 9 + sub_increment].Text = Convert.ToString(grade).Trim();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 10 + sub_increment].Text = Convert.ToString(showingResult).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 11 + sub_increment].Text = Convert.ToString(examMonthYear).Trim();

                                                if ((11 + sub_increment) == (FpRevaluation.Sheets[0].ColumnCount - 1))
                                                {
                                                    if (flag_subj_rowcnt == false)
                                                    {
                                                        find_subjrow_count++;
                                                    }
                                                    sub_increment = 1;
                                                }
                                                else
                                                {
                                                    sub_increment += 7;
                                                }

                                                //if (startColumn >= maxColumn)
                                                //{
                                                //    starColumn = FpRevaluation.Sheets[0].ColumnCount += 6;
                                                //    startColumn = starColumn - 6;
                                                //    maxColumn = starColumn;
                                                //}
                                                //int spancoll = startColumn;
                                                //int visibleCount = 6;
                                                //byte valueS = 0;
                                                //bool[] isVisible = new bool[6];

                                                //int totalCol = 7;

                                                //for (int col = 0; col < 6; col++)
                                                //{
                                                //    isVisible[col] = getColumnOrderVisiblity(dicColumnVisiblity, Convert.ToString(totalCol).Trim(), out valueS);
                                                //    totalCol++;
                                                //}
                                                //int columnVal = 0;

                                                //foreach (bool column in isVisible)
                                                //{
                                                //    if (column)
                                                //    {
                                                //        spancoll = startColumn + columnVal;
                                                //        visibleCount = (6 - columnVal);
                                                //        //visibleCount++;
                                                //        break;
                                                //    }
                                                //    else if (column)
                                                //    {
                                                //        //visibleCount++;
                                                //    }
                                                //    columnVal++;
                                                //}
                                                ////if (true)
                                                ////{

                                                ////    FpRevaluation.Sheets[0].Columns[startColumn].Width = 60;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn].Locked = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn].Visible = isVisible[0];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn].Text = "CIA";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn, 2, 1);


                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 1].Width = 60;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 1].Locked = true;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 1].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 1].Visible = isVisible[1];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn + 1].Text = "ESA";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn + 1, 2, 1);

                                                ////    //isVisible = false;
                                                ////    //isVisible = getColumnOrderVisiblity(dicColumnVisiblity, "9", out valueS);
                                                ////    //if (isVisible)
                                                ////    //{
                                                ////    //    spancoll = startColumn + 2;
                                                ////    //}
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 2].Width = 60;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 2].Locked = true;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 2].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 2].Visible = isVisible[2];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn + 2].Text = "TOTAL";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn + 2, 2, 1);


                                                ////    //isVisible = false;
                                                ////    //isVisible = getColumnOrderVisiblity(dicColumnVisiblity, "10", out valueS);
                                                ////    //if (isVisible)
                                                ////    //{
                                                ////    //    spancoll = startColumn + 3;
                                                ////    //}

                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 3].Width = 60;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 3].Locked = true;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 3].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 3].Visible = isVisible[3];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn + 3].Text = "Grade";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn + 3, 2, 1);


                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 4].Width = 85;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 4].Locked = true;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 4].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 4].Visible = isVisible[4];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn + 4].Text = "RESULT";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn + 4, 2, 1);

                                                ////    //isVisible = false;
                                                ////    //isVisible = getColumnOrderVisiblity(dicColumnVisiblity, "11", out valueS);
                                                ////    //if (isVisible)
                                                ////    //{
                                                ////    //    spancoll = startColumn + 4;
                                                ////    //}


                                                ////    //isVisible = false;
                                                ////    //isVisible = getColumnOrderVisiblity(dicColumnVisiblity, "12", out valueS);
                                                ////    //if (isVisible)
                                                ////    //{
                                                ////    //    spancoll = startColumn + 5;
                                                ////    //}
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 5].Width = 120;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 5].Locked = true;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 5].Resizable = false;
                                                ////    FpRevaluation.Sheets[0].Columns[startColumn + 5].Visible = isVisible[5];
                                                ////    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, startColumn + 5].Text = "Y";
                                                ////    FpRevaluation.Sheets[0].ColumnHeaderSpanModel.Add(0, startColumn + 5, 2, 1);

                                                ////}


                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].CellType = txtCell;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].Text = Convert.ToString(subjectCode).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].Tag = Convert.ToString(subjectNo).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].Note = Convert.ToString(subjectName).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].Font.Name = "Book Antiqua";
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].Locked = false;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].HorizontalAlign = HorizontalAlign.Left;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, spancoll].VerticalAlign = VerticalAlign.Middle;
                                                //FpRevaluation.Sheets[0].AddSpanCell(FpRevaluation.Sheets[0].RowCount - 2, spancoll, 1, visibleCount);

                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].CellType = txtCell;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].Text = Convert.ToString(showInternalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].Tag = Convert.ToString(actualInternalMark).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].Note = Convert.ToString(MaxInternalMark).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].Font.Name = "Book Antiqua";
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].Locked = false;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Center;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn].VerticalAlign = VerticalAlign.Middle;

                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].CellType = txtCell;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].Text = Convert.ToString(showExternalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].Tag = Convert.ToString(actualExternalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].Note = Convert.ToString(MaxExternalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].Font.Name = "Book Antiqua";
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].Locked = false;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].HorizontalAlign = HorizontalAlign.Center;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 1].VerticalAlign = VerticalAlign.Middle;

                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].Text = Convert.ToString(showTotalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1,startColumn+ 2].Tag = Convert.ToString(eduLevel).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].Note = Convert.ToString(actualTotalMark).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].CellType = txtCell;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].Font.Name = "Book Antiqua";
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].Locked = true;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].HorizontalAlign = HorizontalAlign.Center;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 2].VerticalAlign = VerticalAlign.Middle;

                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].Text = Convert.ToString(showingResult).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].Tag = Convert.ToString(passOrFail).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].Note = Convert.ToString(actualResult).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].CellType = txtCell;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].Font.Name = "Book Antiqua";
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].Locked = true;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].HorizontalAlign = HorizontalAlign.Center;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 4].VerticalAlign = VerticalAlign.Middle;

                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].Text = Convert.ToString(grade).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].Tag = Convert.ToString(actualGrade).Trim();
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn+3].Note = Convert.ToString(currentSemesterNew).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].CellType = txtCell;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].Font.Name = "Book Antiqua";
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].Locked = true;
                                                //FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].HorizontalAlign = HorizontalAlign.Center;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 3].VerticalAlign = VerticalAlign.Middle;

                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].Text = Convert.ToString(examMonthYear).Trim();
                                                //  FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].Tag = Convert.ToString(examYear).Trim();
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].Note = Convert.ToString(examMonth).Trim();
                                                //  FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].CellType = txtCell;//
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].Font.Name = "Book Antiqua";
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].Locked = true;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].HorizontalAlign = HorizontalAlign.Center;
                                                // FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, startColumn + 5].VerticalAlign = VerticalAlign.Middle;

                                                //startColumn += 6;
                                                //subject++;
                                                row = true;
                                            }
                                        }
                                        if (row == true)
                                        {


                                            double spanCount = Math.Ceiling(Convert.ToDouble(dtSubjectList.Rows.Count) / 3);
                                            spanRowCount = Convert.ToInt32(spanCount);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 0, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 1, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 2, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 3, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 4, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 5, spanRowCount, 1);



                                            flag_subj_rowcnt = true;

                                            FpRevaluation.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;


                                            FpRevaluation.Sheets[0].Rows[FpRevaluation.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Rows[FpRevaluation.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                                        }
                                    }
                                }
                            }
                            divMainContents.Visible = true;
                            FpRevaluation.Sheets[0].PageSize = FpRevaluation.Sheets[0].RowCount;
                            //FpRevaluation.Width = 1000;
                            //FpRevaluation.Height = 500;
                            FpRevaluation.SaveChanges();
                            FpRevaluation.Visible = true;

                            func_footer();
                        }
                        else
                        {
                            lblAlertMsg.Text = "Revaluation Marks is Not Found";
                            divPopAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Students Were Applied For Revaluation";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    if (dsRevalAppliedPapers.Tables.Count > 0 && dsRevalAppliedPapers.Tables[0].Rows.Count > 0)
                    {
                        dtRevalAppliedStudents = dsRevalAppliedPapers.Tables[0].DefaultView.ToTable(true, "college_code", "Batch_Year", "degree_code", "App_No", "Roll_No", "Reg_No", "Roll_Admit", "Stud_Name", "Sections", "Stud_Type", "Gender", "dob");
                        if (dsRevalMarkList.Tables.Count > 0 && dsRevalMarkList.Tables[0].Rows.Count > 0)
                        {
                            if (dtRevalAppliedStudents.Rows.Count > 0)
                            {
                              
                                function_load_header();
                                FpRevaluation.Visible = true;
                             
                                FpRevaluation.Sheets[0].RowCount = 0;
                                FpRevaluation.Sheets[0].RowHeader.Visible = false;
                                FpRevaluation.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                                FpRevaluation.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Large;
                                FpRevaluation.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpRevaluation.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                int serialNo = 0;
                                int subjectColumn = FpRevaluation.Sheets[0].ColumnCount;
                                int maxColumn = subjectColumn;
                                foreach (DataRow drStudents in dtRevalAppliedStudents.Rows)
                                {
                                    DataTable dtStudentMark = new DataTable();
                                    string studentName = string.Empty;
                                    string rollNo = string.Empty;
                                    string regNo = string.Empty;
                                    string appNo = string.Empty;
                                    string rollAdmit = string.Empty;
                                    string gender = string.Empty;
                                    string studentType = string.Empty;
                                    string sectionNew = string.Empty;
                                    string batchYearNew = string.Empty;
                                    string dob = string.Empty;
                                    string collegeCode = string.Empty;
                                    string degreeCodeNew = string.Empty;

                                    studentName = Convert.ToString(drStudents["Stud_Name"]).Trim();
                                    rollNo = Convert.ToString(drStudents["Roll_No"]).Trim();
                                    regNo = Convert.ToString(drStudents["Reg_No"]).Trim();
                                    appNo = Convert.ToString(drStudents["App_No"]).Trim();
                                    rollAdmit = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                                    gender = Convert.ToString(drStudents["Gender"]).Trim();
                                    studentType = Convert.ToString(drStudents["Stud_Type"]).Trim();
                                    sectionNew = Convert.ToString(drStudents["Sections"]).Trim();
                                    batchYearNew = Convert.ToString(drStudents["Batch_Year"]).Trim();
                                    dob = Convert.ToString(drStudents["dob"]).Trim();
                                    collegeCode = Convert.ToString(drStudents["college_code"]).Trim();
                                    degreeCodeNew = Convert.ToString(drStudents["degree_code"]).Trim();

                                    dsRevalMarkList.Tables[0].DefaultView.RowFilter = "App_No='" + appNo + "'";
                                    dtStudentMark = dsRevalMarkList.Tables[0].DefaultView.ToTable();
                                    int starColumn = subjectColumn;
                                    int startColumn = 0;
                                    if (dtStudentMark.Rows.Count > 0)
                                    {
                                       


                                        serialNo++;
                                      
                                        Farpoint.TextCellType txtCell = new Farpoint.TextCellType();

                                       


                                        int studRowCount = FpRevaluation.Sheets[0].RowCount++;

                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Text = "  " + Convert.ToString(studentName).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].Text = gender;
                                        int sub_increment = 1;
                                        int subIncrementCount = 11;
                                        int spanRowCount = 1;
                                        int subject = 0;
                                        DataTable dtSubjectList = new DataTable();
                                        dtSubjectList = dtStudentMark.DefaultView.ToTable(true, "subject_no", "subject_code", "subject_name");
                                      
                                        Boolean row = false;
                                        for (int subcode = 0; subcode < dtSubjectList.Rows.Count; subcode++)
                                        {
                                            string subjectNo = string.Empty;
                                            string subjectCode = string.Empty;
                                            string subjectName = string.Empty;
                                            subjectNo = Convert.ToString(dtSubjectList.Rows[subcode]["subject_no"]).Trim();
                                            subjectCode = Convert.ToString(dtSubjectList.Rows[subcode]["subject_code"]).Trim();
                                            subjectName = Convert.ToString(dtSubjectList.Rows[subcode]["subject_name"]).Trim();

                                            DataView dvSubMarks = new DataView();
                                            dtStudentMark.DefaultView.RowFilter = "subject_no='" + subjectNo + "'";
                                            dvSubMarks = dtStudentMark.DefaultView;
                                            dvSubMarks.Sort = "total desc,external_mark desc,internal_mark desc";
                                            if (dvSubMarks.Count > 0)
                                            {
                                                DataRowView drMarks = dvSubMarks[0];
                                                string internalMark = string.Empty;
                                                string minInternalMark = string.Empty;
                                                string MaxInternalMark = string.Empty;
                                                string actualInternalMark = string.Empty;

                                                string externalMark = string.Empty;
                                                string minExternalMark = string.Empty;
                                                string MaxExternalMark = string.Empty;
                                                string actualExternalMark = string.Empty;
                                                string revaluation1 = string.Empty;
                                                string revaluation2 = string.Empty;
                                                string revaluation3 = string.Empty;

                                                string totalMark = string.Empty;
                                                string minTotalMark = string.Empty;
                                                string MaxTotalMark = string.Empty;
                                                string actualTotalMark = string.Empty;

                                                string grade = string.Empty;
                                                string actualGrade = string.Empty;

                                                string evaluation1 = string.Empty;
                                                string evaluation2 = string.Empty;
                                                string evaluation3 = string.Empty;

                                                string passOrFail = string.Empty;
                                                string result = string.Empty;
                                                string actualResult = string.Empty;

                                                string attempt = string.Empty;
                                                string subjectSemester = string.Empty;


                                                string creditPoint = string.Empty;

                                                internalMark = Convert.ToString(drMarks["internal_mark"]).Trim();
                                                minInternalMark = Convert.ToString(drMarks["min_int_marks"]).Trim();
                                                MaxInternalMark = Convert.ToString(drMarks["max_int_marks"]).Trim();
                                                actualInternalMark = Convert.ToString(drMarks["actual_internal_mark"]).Trim();

                                                externalMark = Convert.ToString(drMarks["external_mark"]).Trim();
                                                minExternalMark = Convert.ToString(drMarks["min_ext_marks"]).Trim();
                                                MaxExternalMark = Convert.ToString(drMarks["max_ext_marks"]).Trim();
                                                actualExternalMark = Convert.ToString(drMarks["actual_external_mark"]).Trim();

                                                revaluation1 = Convert.ToString(drMarks["revaluation_1"]).Trim();
                                                revaluation2 = Convert.ToString(drMarks["revaluation_2"]).Trim();
                                                revaluation3 = Convert.ToString(drMarks["revaluation_3"]).Trim();

                                                totalMark = Convert.ToString(drMarks["total"]).Trim();
                                                minTotalMark = Convert.ToString(drMarks["mintotal"]).Trim();
                                                MaxTotalMark = Convert.ToString(drMarks["maxtotal"]).Trim();
                                                actualTotalMark = Convert.ToString(drMarks["actual_total"]).Trim();

                                                grade = Convert.ToString(drMarks["grade"]).Trim();
                                                actualGrade = Convert.ToString(drMarks["Actual_Grade"]).Trim();

                                                evaluation1 = Convert.ToString(drMarks["evaluation1"]).Trim();
                                                evaluation2 = Convert.ToString(drMarks["evaluation2"]).Trim();
                                                evaluation3 = Convert.ToString(drMarks["evaluation3"]).Trim();

                                                passOrFail = Convert.ToString(drMarks["passorfail"]).Trim();
                                                result = Convert.ToString(drMarks["result"]).Trim();
                                                actualResult = Convert.ToString(drMarks["result"]).Trim();

                                                string examCode = Convert.ToString(drMarks["exam_code"]).Trim();
                                                string actualRevalMArk = Convert.ToString(drMarks["Act_Reval_Mark"]).Trim();

                                                attempt = Convert.ToString(drMarks["attempts"]).Trim();
                                                subjectSemester = Convert.ToString(drMarks["semester"]).Trim();
                                                subjectNo = Convert.ToString(drMarks["subject_no"]).Trim();
                                                subjectCode = Convert.ToString(drMarks["subject_code"]).Trim();
                                                subjectName = Convert.ToString(drMarks["subject_name"]).Trim();
                                                creditPoint = Convert.ToString(drMarks["credit_points"]).Trim();

                                                double internalMarksValue = 0;
                                                double externalMarksValue = 0;
                                                double totalMarksValue = 0;
                                                double actual_externalmark=0;
                                                double reval1 = 0;
                                                double reval2 = 0;
                                                double reval3 = 0;

                                                double.TryParse(internalMark, out internalMarksValue);
                                                double.TryParse(externalMark, out externalMarksValue);
                                                double.TryParse(actualExternalMark, out actual_externalmark);
                                                double.TryParse(revaluation1, out reval1);
                                                double.TryParse(revaluation2, out reval2);
                                                double.TryParse(revaluation3, out reval3);
                                                double.TryParse(totalMark, out totalMarksValue);

                                                bool externalOnly = false;
                                                bool internalOnly = false;

                                              
                                                string totalValues = (string.IsNullOrEmpty(totalMark) ? "--" : ((totalMarksValue == -1) ? "AA" : (totalMarksValue == -2) ? "NE" : (totalMarksValue == -3) ? "NR" : (totalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(totalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string internalValues = (string.IsNullOrEmpty(internalMark) ? "--" : ((internalMarksValue == -1) ? "AA" : (internalMarksValue == -2) ? "NE" : (internalMarksValue == -3) ? "NR" : (internalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(internalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string externalValues = (string.IsNullOrEmpty(externalMark) ? "--" : ((externalMarksValue == -1) ? "AA" : (externalMarksValue == -2) ? "NE" : (externalMarksValue == -3) ? "NR" : (externalMarksValue == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(externalMarksValue).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string act_extmark = (string.IsNullOrEmpty(actualExternalMark) ? "--" : ((actual_externalmark == -1) ? "AA" : (actual_externalmark == -2) ? "NE" : (actual_externalmark == -3) ? "NR" : (actual_externalmark == -4) ? "LT" : Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToString(actual_externalmark).Trim()), 1, MidpointRounding.AwayFromZero)).PadLeft(2, '0')));
                                                string revaluation_1 = (string.IsNullOrEmpty(revaluation1) ? "NA" : ((reval1 == 0) ? "NA" : Convert.ToString(reval1)));
                                                string revaluation_2 = (string.IsNullOrEmpty(revaluation2) ? "NA" : ((reval2 == 0) ? "NA" : Convert.ToString(reval2)));
                                                string revaluation_3 = (string.IsNullOrEmpty(revaluation3) ? "NA" : ((reval3 == 0) ? "NA" : Convert.ToString(reval3)));
                                                string showInternalMark = internalValues;
                                                string showExternalMark = externalValues;
                                                string showTotalMark = totalValues;
                                                string showingResult = string.Empty;

                                              
                                                if (string.IsNullOrEmpty(result.Trim().ToLower()))
                                                {
                                                    showingResult = "--";
                                                }
                                                else if (result.Trim().ToLower().Contains("pass"))
                                                {
                                                    showingResult = "P";
                                                }
                                                else if (result.Trim().ToLower().Contains("f"))
                                                {
                                                    if (isShowFailResult)
                                                    {
                                                        showingResult = failShowsResult;
                                                    }
                                                }
                                                else if (result.ToLower().Trim().Contains("aaa") || result.ToLower().Trim().Contains("aa") || result.ToLower().Trim().Contains("ab"))
                                                {
                                                    showingResult = "AA";
                                                }
                                                else
                                                {
                                                    showingResult = result.Trim();
                                                }

                                                if (string.IsNullOrEmpty(MaxInternalMark) || MaxInternalMark.Trim() == "0")
                                                {
                                                    externalOnly = true;
                                                    showInternalMark = "--";
                                                    if (externalMarksValue < 0)
                                                    {
                                                        totalValues = "--";
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(MaxExternalMark) || MaxExternalMark.Trim() == "0")
                                                {
                                                    internalOnly = true;
                                                    showExternalMark = "--";
                                                    if (internalMarksValue < 0)
                                                    {
                                                        totalValues = "--";
                                                    }
                                                }
                                                if (internalOnly)
                                                {
                                                    if (internalMarksValue == -1)
                                                    {
                                                        showingResult = "RA";
                                                    }
                                                }
                                                else if (externalOnly)
                                                {
                                                    if (externalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                    else if (externalMarksValue == -1 || totalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                }
                                                else
                                                {
                                                    if (internalMarksValue == -1 && (externalMarksValue >= 0 && totalMarksValue >= 0))
                                                    {
                                                        showingResult = "RA";
                                                    }
                                                    else if (externalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                    else if (externalMarksValue == -1 || totalMarksValue == -1)
                                                    {
                                                        showingResult = "A";
                                                    }
                                                }
                                                if (subcode % 3 == 0 & subcode != 0)
                                                {
                                                    FpRevaluation.Sheets[0].RowCount++;
                                                }
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(rollNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].Text = "  " + Convert.ToString(studentName).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].Text = gender;
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                                             
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 5 + sub_increment].Text = Convert.ToString(subjectCode).Trim();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 6 + sub_increment].Text = Convert.ToString(showInternalMark).Trim();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 7 + sub_increment].Text = Convert.ToString(act_extmark).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 8 + sub_increment].Text = Convert.ToString(revaluation_1).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 9 + sub_increment].Text = Convert.ToString(revaluation_2).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 10 + sub_increment].Text = Convert.ToString(revaluation_3).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 11 + sub_increment].Text = Convert.ToString(showExternalMark).Trim();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 12 + sub_increment].Text = Convert.ToString(showTotalMark).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 13 + sub_increment].Text = Convert.ToString(grade).Trim();

                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 14 + sub_increment].Text = Convert.ToString(showingResult).Trim();
                                                FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 1, 15 + sub_increment].Text = Convert.ToString(examMonthYear).Trim();

                                                if ((15 + sub_increment) == (FpRevaluation.Sheets[0].ColumnCount - 1))
                                                {
                                                    if (flag_subj_rowcnt == false)
                                                    {
                                                        find_subjrow_count++;
                                                    }
                                                    sub_increment = 1;
                                                }
                                                else
                                                {
                                                    sub_increment += 11;
                                                }

                                               
                                                row = true;
                                            }
                                        }
                                        if (row == true)
                                        {


                                            double spanCount = Math.Ceiling(Convert.ToDouble(dtSubjectList.Rows.Count) / 3);
                                            spanRowCount = Convert.ToInt32(spanCount);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 0, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 1, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 2, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 3, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 4, spanRowCount, 1);
                                            FpRevaluation.Sheets[0].AddSpanCell(studRowCount, 5, spanRowCount, 1);



                                            flag_subj_rowcnt = true;

                                            FpRevaluation.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;


                                            FpRevaluation.Sheets[0].Rows[FpRevaluation.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                            FpRevaluation.Sheets[0].Rows[FpRevaluation.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                                        }
                                    }
                                }
                            }
                            divMainContents.Visible = true;
                            FpRevaluation.Sheets[0].PageSize = FpRevaluation.Sheets[0].RowCount;
                           
                            FpRevaluation.SaveChanges();
                            FpRevaluation.Visible = true;

                            func_footer();
                        }
                        else
                        {
                            lblAlertMsg.Text = "Revaluation Marks is Not Found";
                            divPopAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Students Were Applied For Revaluation";
                        divPopAlert.Visible = true;
                        return;
                    }

                }
            }
            else
            {

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

    #region Generate Excel

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
                if (FpRevaluation.Visible == true)
                {
                    da.printexcelreport(FpRevaluation, reportname);
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
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Over All GPA And CGPA Calculation Report";
            string pagename = "GPA_CGPA_CalculationProcess.aspx";
            string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlSem.SelectedItem).Trim();

            string exam_month = string.Empty;

            exam_month = ddlExamMonth.SelectedValue.ToString();
            string strExam_month = string.Empty;
            exam_month = exam_month.Trim();
            switch (exam_month)
            {
                case "1":
                    strExam_month = "January";
                    break;
                case "2":
                    strExam_month = "February";
                    break;
                case "3":
                    strExam_month = "March";
                    break;
                case "4":
                    strExam_month = "April";
                    break;
                case "5":
                    strExam_month = "May";
                    break;
                case "6":
                    strExam_month = "June";
                    break;
                case "7":
                    strExam_month = "July";
                    break;
                case "8":
                    strExam_month = "Augest";
                    break;
                case "9":
                    strExam_month = "September";
                    break;
                case "10":
                    strExam_month = "October";
                    break;
                case "11":
                    strExam_month = "November";
                    break;
                case "12":
                    strExam_month = "December";
                    break;
            }


            string qry = "select clg.collname,c.Edu_Level,c.Course_Name,dt.Dept_Name,ltrim(rtrim(ISNULL(c.type,''))) as Type,'Class :'+c.Course_Name+' '+dt.Dept_Name as Degree_Details from collinfo clg,Course c,Degree dg,Department dt where c.college_code=clg.college_code and clg.college_code=dg.college_code and  clg.college_code=dt.college_code and dt.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'";
            DataSet dsDegreeDetails = new DataSet();
            dsDegreeDetails = da.select_method_wo_parameter(qry, "text");
            string className = string.Empty;
            string sectionDetails = string.Empty;
            string collegeHeader = txtCollegeHeader.Text.Trim();
            if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
            {
                className = Convert.ToString(dsDegreeDetails.Tables[0].Rows[0]["Degree_Details"]).Trim();
            }
            else
            {
                className = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " " + ddlBranch.SelectedItem.ToString();
            }
            if (ddlSec.Enabled)
            {
                if (ddlSec.Items.Count > 0)
                {
                    if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                    {
                        sectionDetails = " '" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "' SECTION";
                    }
                }
            }
            string degreedetails = ((string.IsNullOrEmpty(collegeHeader)) ? "" : collegeHeader + "$") + "Office of the Controller of Examinations$" + "Revaluation Result" + "-" + strExam_month + " " + ddlExamYear.SelectedItem.ToString() + "$" + className + " " + sectionDetails +" ( "+ dsDegreeDetails.Tables[0].Rows[0]["Type"]+" ) "+"@"+"Semester : "+ddlSem.SelectedItem.Text ;




            if (FpRevaluation.Visible == true)
            {
                printCommonPdf.loadspreaddetails(FpRevaluation, pagename, degreedetails);
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

    #endregion Print PDF

    private void columnVisibility(ref Dictionary<string, byte> dicColumnVisiblity)
    {
        try
        {
            dicColumnVisiblity = new Dictionary<string, byte>();
            foreach (ListItem li in cblColumnOrder.Items)
            {
                string value = Convert.ToString(li.Value).Trim();
                string text = Convert.ToString(li.Text).Trim();
                if (!dicColumnVisiblity.ContainsKey(value))
                {
                    dicColumnVisiblity.Add(value, 0);
                }
            }
            if (cblColumnOrder.Items.Count > 0)
            {
                foreach (ListItem li in cblColumnOrder.Items)
                {
                    string value = Convert.ToString(li.Value).Trim();
                    string text = Convert.ToString(li.Text).Trim();
                    if (li.Selected)
                    {
                        switch (value)
                        {
                            default:
                                if (dicColumnVisiblity.ContainsKey(value))
                                {
                                    dicColumnVisiblity[value] = 1;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (value)
                        {
                            default:
                                if (dicColumnVisiblity.ContainsKey(value))
                                {
                                    dicColumnVisiblity[value] = 0;
                                }
                                break;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    private bool getColumnOrderVisiblity(Dictionary<string, byte> dicColumnVisiblity, string key, out byte value)
    {
        bool isVisible = false;
        value = 0;
        byte visiblityValue = 0;
        try
        {
            if (dicColumnVisiblity.Count > 0)
            {
                if (dicColumnVisiblity.ContainsKey(key))
                {
                    value = dicColumnVisiblity[key];
                }
                else
                {
                    isVisible = false;
                    value = 0;
                }
            }
            switch (value)
            {
                case 0:
                    isVisible = false;
                    break;
                case 1:
                    isVisible = true;
                    break;
                default:
                    isVisible = false;
                    break;
            }
            return isVisible;
        }
        catch
        {
            return isVisible;
        }
    }

    #endregion Button Events


    protected void rblformat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblformat.SelectedIndex == 1)
            {
                pnlHeaderFilter.Visible = true;
                lblFilter.Visible = true;
                pnlColumnOrder.Visible = true;
            }
            else
            {
                pnlHeaderFilter.Visible = false;
                lblFilter.Visible = false;
                pnlColumnOrder.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void function_load_header()
    {
        try
        {
            //MyImg mi = new MyImg();
            //mi.ImageUrl = "~/college/Left_Logo.jpeg";
            //mi.ImageUrl = "Handler2.ashx?";
            //MyImg mi2 = new MyImg();
            //mi2.ImageUrl = "~/images/10BIT001.jpeg";
            //mi2.ImageUrl = "Handler5.ashx?";
            if (rblformat.SelectedIndex == 0)
            {
                FpRevaluation.Visible = true;
                FpRevaluation.Sheets[0].RowCount = 0;
                FpRevaluation.Sheets[0].ColumnCount = 0;
                FpRevaluation.Sheets[0].ColumnCount = 27;
                FpRevaluation.Sheets[0].ColumnHeader.RowCount = 1;
                FpRevaluation.CommandBar.Visible = false;
                FpRevaluation.Sheets[0].RowHeader.Visible = false;
                FpRevaluation.Sheets[0].AutoPostBack = true;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                style.HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpRevaluation.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpRevaluation.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpRevaluation.Sheets[0].Columns[0].Width = 60;
                FpRevaluation.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
                FpRevaluation.Sheets[0].Columns[1].Width = 120;
                FpRevaluation.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Regn.No";
                FpRevaluation.Sheets[0].Columns[2].Width = 120;
                FpRevaluation.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpRevaluation.Sheets[0].Columns[3].Width = 235;
                FpRevaluation.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpRevaluation.Sheets[0].Columns[4].Width = 150;
                FpRevaluation.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpRevaluation.Sheets[0].Columns[5].Width = 100;
                FpRevaluation.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subcode";
                FpRevaluation.Sheets[0].Columns[6].Width = 135;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 7].Text = "CIA";
                FpRevaluation.Sheets[0].Columns[7].Width = 80;
                FpRevaluation.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 8].Text = "ESA";
                FpRevaluation.Sheets[0].Columns[8].Width = 80;
                FpRevaluation.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 9].Text = "TOTAL";
                FpRevaluation.Sheets[0].Columns[9].Width = 95;
                FpRevaluation.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Grade";
                FpRevaluation.Sheets[0].Columns[10].Width = 65;
                FpRevaluation.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Result";
                FpRevaluation.Sheets[0].Columns[11].Width = 70;
                FpRevaluation.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Y";
                FpRevaluation.Sheets[0].Columns[12].Width = 65;
                FpRevaluation.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;

                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Subcode";
                FpRevaluation.Sheets[0].Columns[13].Width = 135;

                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 14].Text = "CIA";
                FpRevaluation.Sheets[0].Columns[14].Width = 80;
                FpRevaluation.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 15].Text = "ESA";
                FpRevaluation.Sheets[0].Columns[15].Width = 80;
                FpRevaluation.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 16].Text = "TOTAL";
                FpRevaluation.Sheets[0].Columns[16].Width = 95;
                FpRevaluation.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Grade";
                FpRevaluation.Sheets[0].Columns[17].Width = 65;
                FpRevaluation.Sheets[0].Columns[17].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Result";
                FpRevaluation.Sheets[0].Columns[18].Width = 70;
                FpRevaluation.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Y";
                FpRevaluation.Sheets[0].Columns[19].Width = 65;
                FpRevaluation.Sheets[0].Columns[19].HorizontalAlign = HorizontalAlign.Center;

                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Subcode";
                FpRevaluation.Sheets[0].Columns[20].Width = 135;

                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 21].Text = "CIA";
                FpRevaluation.Sheets[0].Columns[21].Width = 80;
                FpRevaluation.Sheets[0].Columns[21].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 22].Text = "ESA";
                FpRevaluation.Sheets[0].Columns[22].Width = 80;
                FpRevaluation.Sheets[0].Columns[22].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 23].Text = "TOTAL";
                FpRevaluation.Sheets[0].Columns[23].Width = 95;
                FpRevaluation.Sheets[0].Columns[23].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 24].Text = "Grade";
                FpRevaluation.Sheets[0].Columns[24].Width = 65;
                FpRevaluation.Sheets[0].Columns[24].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Result";
                FpRevaluation.Sheets[0].Columns[25].Width = 70;
                FpRevaluation.Sheets[0].Columns[25].HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 26].Text = "Y";
                FpRevaluation.Sheets[0].Columns[26].Width = 65;
            }
            else
            {
                FpRevaluation.Visible = true;
                FpRevaluation.Sheets[0].RowCount = 0;
                FpRevaluation.Sheets[0].ColumnCount = 0;
                FpRevaluation.Sheets[0].ColumnCount = 39;
                FpRevaluation.Sheets[0].ColumnHeader.RowCount = 1;
                FpRevaluation.CommandBar.Visible = false;
                FpRevaluation.Sheets[0].RowHeader.Visible = false;
                FpRevaluation.Sheets[0].AutoPostBack = true;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                style.HorizontalAlign = HorizontalAlign.Center;
                FpRevaluation.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpRevaluation.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpRevaluation.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                if (cblColumnOrder.Items[0].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpRevaluation.Sheets[0].Columns[0].Width = 60;
                    FpRevaluation.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[0].Visible = false;
                }
                if (cblColumnOrder.Items[1].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
                    FpRevaluation.Sheets[0].Columns[1].Width = 120;
                    FpRevaluation.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[1].Visible = false;
                }
                if (cblColumnOrder.Items[2].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Regn.No";
                    FpRevaluation.Sheets[0].Columns[2].Width = 120;
                    FpRevaluation.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[2].Visible = false;
                }
                if (cblColumnOrder.Items[3].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpRevaluation.Sheets[0].Columns[3].Width = 235;
                    FpRevaluation.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[3].Visible = false;
                }
                if (cblColumnOrder.Items[4].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                    FpRevaluation.Sheets[0].Columns[4].Width = 150;
                    FpRevaluation.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[4].Visible = false;
                }
                if (cblColumnOrder.Items[5].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                    FpRevaluation.Sheets[0].Columns[5].Width = 100;
                    FpRevaluation.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[5].Visible = false;
                }
                if (cblColumnOrder.Items[6].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subcode";
                    FpRevaluation.Sheets[0].Columns[6].Width = 135;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Subcode";
                    FpRevaluation.Sheets[0].Columns[17].Width = 135;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 28].Text = "Subcode";
                    FpRevaluation.Sheets[0].Columns[28].Width = 135;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[6].Visible = false;
                    FpRevaluation.Sheets[0].Columns[17].Visible = false;
                    FpRevaluation.Sheets[0].Columns[28].Visible = false;
                }
                if (cblColumnOrder.Items[7].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 7].Text = "CIA";
                    FpRevaluation.Sheets[0].Columns[7].Width = 80;
                    FpRevaluation.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 18].Text = "CIA";
                    FpRevaluation.Sheets[0].Columns[18].Width = 80;
                    FpRevaluation.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 29].Text = "CIA";
                    FpRevaluation.Sheets[0].Columns[29].Width = 80;
                    FpRevaluation.Sheets[0].Columns[29].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[7].Visible = false;
                    FpRevaluation.Sheets[0].Columns[18].Visible = false;
                    FpRevaluation.Sheets[0].Columns[29].Visible = false;
                }
                if (cblColumnOrder.Items[8].Selected == true)
                {

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Actual ESA";
                    FpRevaluation.Sheets[0].Columns[8].Width = 80;
                    FpRevaluation.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Actual ESA";
                    FpRevaluation.Sheets[0].Columns[19].Width = 80;
                    FpRevaluation.Sheets[0].Columns[19].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 30].Text = "Actual ESA";
                    FpRevaluation.Sheets[0].Columns[30].Width = 80;
                    FpRevaluation.Sheets[0].Columns[30].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[8].Visible = false;
                    FpRevaluation.Sheets[0].Columns[19].Visible = false;
                    FpRevaluation.Sheets[0].Columns[30].Visible = false;
                }
                if (cblColumnOrder.Items[9].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Revaluation_1";
                    FpRevaluation.Sheets[0].Columns[9].Width = 95;
                    FpRevaluation.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Revaluation_1";
                    FpRevaluation.Sheets[0].Columns[20].Width = 95;
                    FpRevaluation.Sheets[0].Columns[20].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 31].Text = "Revaluation_1";
                    FpRevaluation.Sheets[0].Columns[31].Width = 95;
                    FpRevaluation.Sheets[0].Columns[31].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[9].Visible = false;
                    FpRevaluation.Sheets[0].Columns[20].Visible = false;
                    FpRevaluation.Sheets[0].Columns[31].Visible = false;
                }
                if (cblColumnOrder.Items[10].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Revaluation_2";
                    FpRevaluation.Sheets[0].Columns[10].Width = 95;
                    FpRevaluation.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 21].Text = "Revaluation_2";
                    FpRevaluation.Sheets[0].Columns[21].Width = 95;
                    FpRevaluation.Sheets[0].Columns[21].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 32].Text = "Revaluation_2";
                    FpRevaluation.Sheets[0].Columns[32].Width = 95;
                    FpRevaluation.Sheets[0].Columns[32].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[10].Visible = false;
                    FpRevaluation.Sheets[0].Columns[21].Visible = false;
                    FpRevaluation.Sheets[0].Columns[32].Visible = false;

                }
                if (cblColumnOrder.Items[11].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Revaluation_3";
                    FpRevaluation.Sheets[0].Columns[11].Width = 95;
                    FpRevaluation.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 22].Text = "Revaluation_3";
                    FpRevaluation.Sheets[0].Columns[22].Width = 95;
                    FpRevaluation.Sheets[0].Columns[22].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 33].Text = "Revaluation_3";
                    FpRevaluation.Sheets[0].Columns[33].Width = 95;
                    FpRevaluation.Sheets[0].Columns[33].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[11].Visible = false;
                    FpRevaluation.Sheets[0].Columns[22].Visible = false;
                    FpRevaluation.Sheets[0].Columns[33].Visible = false;
                }
                if (cblColumnOrder.Items[12].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Final ESA";
                    FpRevaluation.Sheets[0].Columns[12].Width = 95;
                    FpRevaluation.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 23].Text = "Final ESA";
                    FpRevaluation.Sheets[0].Columns[23].Width = 95;
                    FpRevaluation.Sheets[0].Columns[23].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 34].Text = "Final ESA";
                    FpRevaluation.Sheets[0].Columns[34].Width = 95;
                    FpRevaluation.Sheets[0].Columns[34].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[12].Visible = false;
                    FpRevaluation.Sheets[0].Columns[23].Visible = false;
                    FpRevaluation.Sheets[0].Columns[34].Visible = false;
                }
                if (cblColumnOrder.Items[13].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 13].Text = "TOTAL";
                    FpRevaluation.Sheets[0].Columns[13].Width = 95;
                    FpRevaluation.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 24].Text = "TOTAL";
                    FpRevaluation.Sheets[0].Columns[24].Width = 95;
                    FpRevaluation.Sheets[0].Columns[24].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 35].Text = "TOTAL";
                    FpRevaluation.Sheets[0].Columns[35].Width = 95;
                    FpRevaluation.Sheets[0].Columns[35].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[13].Visible = false;
                    FpRevaluation.Sheets[0].Columns[24].Visible = false;
                    FpRevaluation.Sheets[0].Columns[35].Visible = false;
                }
                if (cblColumnOrder.Items[14].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Grade";
                    FpRevaluation.Sheets[0].Columns[14].Width = 65;
                    FpRevaluation.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Grade";
                    FpRevaluation.Sheets[0].Columns[25].Width = 65;
                    FpRevaluation.Sheets[0].Columns[25].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 36].Text = "Grade";
                    FpRevaluation.Sheets[0].Columns[36].Width = 65;
                    FpRevaluation.Sheets[0].Columns[36].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[14].Visible = false;
                    FpRevaluation.Sheets[0].Columns[25].Visible = false;
                    FpRevaluation.Sheets[0].Columns[36].Visible = false;
                }
                if (cblColumnOrder.Items[15].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Result";
                    FpRevaluation.Sheets[0].Columns[15].Width = 70;
                    FpRevaluation.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 26].Text = "Result";
                    FpRevaluation.Sheets[0].Columns[26].Width = 70;
                    FpRevaluation.Sheets[0].Columns[26].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 37].Text = "Result";
                    FpRevaluation.Sheets[0].Columns[37].Width = 70;
                    FpRevaluation.Sheets[0].Columns[37].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[15].Visible = false;
                    FpRevaluation.Sheets[0].Columns[26].Visible = false;
                    FpRevaluation.Sheets[0].Columns[37].Visible = false;
                }
                if (cblColumnOrder.Items[16].Selected == true)
                {
                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Y";
                    FpRevaluation.Sheets[0].Columns[16].Width = 65;
                    FpRevaluation.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 27].Text = "Y";
                    FpRevaluation.Sheets[0].Columns[27].Width = 65;
                    FpRevaluation.Sheets[0].Columns[27].HorizontalAlign = HorizontalAlign.Center;

                    FpRevaluation.Sheets[0].ColumnHeader.Cells[0, 38].Text = "Y";
                    FpRevaluation.Sheets[0].Columns[38].Width = 65;
                    FpRevaluation.Sheets[0].Columns[38].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpRevaluation.Sheets[0].Columns[16].Visible = false;
                    FpRevaluation.Sheets[0].Columns[27].Visible = false;
                    FpRevaluation.Sheets[0].Columns[38].Visible = false;
                }
                

              
               
               
            
            }


        }
        catch (Exception ex)
        {

        }
    }

    public void func_footer()
    {
        try
        {
            if (FpRevaluation.Sheets[0].RowCount > 0)
            {
                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    //string address2 = daccess.GetFunction("select address2 from collinfo where college_code=" + Session["collegecode"] + "");
                    //string coename = daccess.GetFunction("select coe from collinfo where college_code=" + Session["collegecode"] + "");
                    int startColumn = 0;
                    if (cblColumnOrder.Items.Count > 0)
                    {
                        foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                        {
                            string liValue = Convert.ToString(liOrder.Value).Trim();
                            if (liOrder.Selected)
                            {
                                break;
                            }
                            startColumn++;
                        }
                    }

                    if (chkShowNoteDescription.Checked)
                    {
                        FpRevaluation.Sheets[0].RowCount += 2;
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].Text = " P - PASS , " + ((string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? "F - FAIL" : (txtFailValue.Text) + " - " + ((txtFailValue.Text.Trim().ToLower() == "f" || (txtFailValue.Text).Trim().ToLower().Contains("f")) ? "FAIL" : ((txtFailValue.Text).Trim().ToLower() == "ra" || (txtFailValue.Text).Trim().ToLower().Contains("ra")) ? " RE APPEAR" : "FAIL")) + " , A - ABSENT , W - WITHHELD [L - LACK OF ATTENDANCE , M - MALPRACTICE , F - FEES NOT PAID , D - DUES]";
                        divFooterResult.InnerHtml = " P - PASS , " + ((string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? "F - FAIL" : (txtFailValue.Text) + " - " + ((txtFailValue.Text.Trim().ToLower() == "f" || (txtFailValue.Text).Trim().ToLower().Contains("f")) ? "FAIL" : ((txtFailValue.Text).Trim().ToLower() == "ra" || (txtFailValue.Text).Trim().ToLower().Contains("ra")) ? " RE APPEAR" : "FAIL")) + " , A - ABSENT , W - WITHHELD [L - LACK OF ATTENDANCE , M - MALPRACTICE , F - FEES NOT PAID , D - DUES]";
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].Font.Bold = true;
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].Border.BorderColor = Color.Wheat;
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].VerticalAlign = VerticalAlign.Middle;
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].HorizontalAlign = HorizontalAlign.Center;
                        FpRevaluation.Sheets[0].SpanModel.Add(FpRevaluation.Sheets[0].RowCount - 2, startColumn, 2, FpRevaluation.Sheets[0].ColumnCount - startColumn);
                        FpRevaluation.Sheets[0].Cells[FpRevaluation.Sheets[0].RowCount - 2, startColumn].VerticalAlign = VerticalAlign.Bottom;
                    }
                    FpRevaluation.Sheets[0].PageSize = FpRevaluation.Sheets[0].RowCount;
                    FpRevaluation.Height = (FpRevaluation.Sheets[0].RowCount * 20) + 200;
                    FpRevaluation.Sheets[0].SheetName = " ";
                }
            }
        }
        catch
        {
        }
    }


}