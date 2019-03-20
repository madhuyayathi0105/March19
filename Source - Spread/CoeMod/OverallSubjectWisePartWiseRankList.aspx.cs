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

public partial class CoeMod_SubjectsPartWiseRankList : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    string batchYears = string.Empty;
    string collegeCodes = string.Empty;
    string collegeNames = string.Empty;
    string courseIds = string.Empty;
    string courseNames = string.Empty;
    string degreeCodes = string.Empty;
    string departmentNames = string.Empty;
    string eduLevels = string.Empty;
    string courseTypes = string.Empty;
    string streamNames = string.Empty;
    string semesters = string.Empty;
    string subjectTypes = string.Empty;
    string subjectNames = string.Empty;
    string subjectNos = string.Empty;
    string subjectCodes = string.Empty;
    string partNos = string.Empty;
    string partNames = string.Empty;
    string qry = string.Empty;
    string qryStream = string.Empty;
    string qryCollege = string.Empty;
    string qryCourseId = string.Empty;
    string qryEduLevel = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryDepartment = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qryPartTypeNos = string.Empty;
    string topValues = string.Empty;

    string qrySubjectNos = string.Empty;
    string qrySubjectNames = string.Empty;
    string qrySubjectCodes = string.Empty;


    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    bool isSchool = false;
    int selected = 0;
    int top = 1;

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

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
                Bindcollege();
                BindStream();
                BindEduLevel();
                BindBatch();
                BindDegree();
                BindBranch();
                ddlReportType_SelectedIndexChanged(sender, e);
                BindSem();
                BindParts();
                BindSubject();

                ddlRankBy.Visible = true;
                ddlRankBy.SelectedIndex = 0;
                divGroupBy.Visible = false;
                ddlSubReportType.Visible = false;
                lblSubReportType.Visible = false;
                ddlSubReportType.SelectedIndex = 0;
                chkDepartmentWise.Checked = false;
                chkDepartmentWise_CheckedChanged(sender, e);
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                txtOrder.Visible = false;
                chkColumnOrderAll.Checked = false;
                string value = "";
                int index;
                value = string.Empty;

                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    if (cblColumnOrder.Items[i].Selected == false)
                    {
                        ItemList.Remove(cblColumnOrder.Items[i].Text.ToString());
                        Itemindex.Remove(Convert.ToString(i));
                    }
                    else
                    {
                        if (!Itemindex.Contains(i))
                        {
                            ItemList.Add(cblColumnOrder.Items[i].Text.ToString());
                            Itemindex.Add(i);
                        }
                    }

                }

                txtOrder.Visible = true;
                txtOrder.Text = "";
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
            //if (cblBatch.Items.Count > 0)
            //{
            //    batchYears = getCblSelectedText(cblBatch);
            //    if (!string.IsNullOrEmpty(batchYears))
            //    {
            //        qryBatch = " and r.Batch_year in(" + batchYears + ")";
            //    }
            //}
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
            //if (cblBatch.Items.Count > 0)
            //{
            //    batchYears = getCblSelectedText(cblBatch);
            //    if (!string.IsNullOrEmpty(batchYears))
            //    {
            //        qryBatch = " and r.Batch_year in(" + batchYears + ")";
            //    }
            //}
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
            //if (cblBatch.Items.Count > 0)
            //{
            //    batchYears = getCblSelectedText(cblBatch);
            //    if (!string.IsNullOrEmpty(batchYears))
            //    {
            //        qryBatch = " and Batch_year in(" + batchYears + ")";
            //    }
            //}
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
                            //ddlSem.Items.Insert(i - 1, );
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

    public void BindParts()
    {
        try
        {
            ddlPartType.Items.Clear();
            lblPartType.Visible = false;
            ddlPartType.Enabled = false;
            ddlPartType.Visible = false;
            ds.Clear();
            selected = 0;

            qryBatch = string.Empty;
            batchYears = string.Empty;

            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            qrySemester = string.Empty;
            partNos = string.Empty;
            qryPartTypeNos = string.Empty;

            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
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
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (cblSem.Items.Count > 0 && txtSem.Visible == true)
            {
                semesters = getCblSelectedValue(cblSem);
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
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
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }

            if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                // --and sm.Batch_Year in('2015') and sm.degree_code in('45') and sm.semester in('1','2','3','4','5','6') and s.Part_Type in('1') 
                qry = "select distinct ISNULL(Part_Type,'0') Part_Type,'Part - ' +Replicate('M', Part_Type/1000)+ REPLACE(REPLACE(REPLACE(Replicate('C', Part_Type%1000/100),Replicate('C', 9), 'CM'),Replicate('C', 5), 'D'),Replicate('C', 4), 'CD')+ REPLACE(REPLACE(REPLACE(Replicate('X', Part_Type%100 / 10),Replicate('X', 9),'XC'),Replicate('X', 5), 'L'),Replicate('X', 4), 'XL')+ REPLACE(REPLACE(REPLACE(Replicate('I', Part_Type%10),Replicate('I', 9),'IX'),Replicate('I', 5), 'V'),Replicate('I', 4),'IV') as Part from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no " + qryPartTypeNos + qryBatch + qryDegreeCode + qrySemester + " and ISNULL(Part_Type,'0')<>'0'  order by  Part_Type";
                //qry = "select distinct s.subject_code,s.subject_name from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no " + qryPartTypeNos + qryBatch + qryDegreeCode + qrySemester + " order by s.subject_code,s.subject_name";union select '0' as Part_Type,'All Part' as Part
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlPartType.DataSource = ds;
                    ddlPartType.DataTextField = "Part";
                    ddlPartType.DataValueField = "Part_Type";
                    ddlPartType.DataBind();
                    ddlPartType.Items.Insert(0, new ListItem("All Part", "0"));
                    ddlPartType.SelectedIndex = 0;
                    lblPartType.Visible = true;
                    ddlPartType.Enabled = true;
                    ddlPartType.Visible = true;
                }
                else
                {
                    ddlPartType.Items.Insert(0, new ListItem("All Part", "0"));
                    ddlPartType.Enabled = false;
                    lblPartType.Visible = false;
                    ddlPartType.Visible = false;
                }
            }
            else
            {
                ddlPartType.Items.Insert(0, new ListItem("All Part", "0"));
                ddlPartType.Enabled = false;
                lblPartType.Visible = false;
                ddlPartType.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSubject()
    {
        try
        {
            cblSubject.Items.Clear();
            chkSubject.Checked = false;
            txtSubject.Text = "--Select--";
            ds.Clear();
            selected = 0;

            qryBatch = string.Empty;
            batchYears = string.Empty;

            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            qrySemester = string.Empty;
            partNos = string.Empty;
            qryPartTypeNos = string.Empty;

            //if (cblBatch.Items.Count > 0)
            //{
            //    batchYears = getCblSelectedText(cblBatch);
            //    if (!string.IsNullOrEmpty(batchYears))
            //    {
            //        qryBatch = " and sm.Batch_year in(" + batchYears + ")";
            //    }
            //}
            if (cblBatch.Items.Count > 0 && txtBatch.Visible == true)
            {
                batchYears = getCblSelectedText(cblBatch);
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
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
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            //if (cblSem.Items.Count > 0)
            //{
            //    semesters = getCblSelectedValue(cblSem);
            //    if (!string.IsNullOrEmpty(semesters))
            //    {
            //        qrySemester = " and sm.semester in(" + semesters + ")";
            //    }
            //}
            if (cblSem.Items.Count > 0 && txtSem.Visible == true)
            {
                semesters = getCblSelectedValue(cblSem);
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
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
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            if (ddlPartType.Items.Count > 0)
            {
                foreach (wc.ListItem li in ddlPartType.Items)
                {
                    if (li.Selected)
                    {
                        if (li.Value.Trim() != "0")
                        {
                            if (string.IsNullOrEmpty(partNos))
                            {
                                partNos = "'" + li.Value + "'";
                            }
                            else
                            {
                                partNos += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(partNos))
                {
                    qryPartTypeNos = " and s.Part_Type in(" + partNos + ")";
                }
            }

            if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                // --and sm.Batch_Year in('2015') and sm.degree_code in('45') and sm.semester in('1','2','3','4','5','6') and s.Part_Type in('1') 
                qry = "select distinct s.subject_code,s.subject_name from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no " + qryPartTypeNos + qryBatch + qryDegreeCode + qrySemester + " order by s.subject_code,s.subject_name";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblSubject.DataSource = ds;
                    cblSubject.DataTextField = "subject_name";
                    cblSubject.DataValueField = "subject_code";
                    cblSubject.DataBind();
                    checkBoxListselectOrDeselect(cblSubject, true);
                    CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
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
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 20;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 330;
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 80;
                FpSpread1.Sheets[0].Columns[5].Width = 150;
                FpSpread1.Sheets[0].Columns[6].Width = 255;
                FpSpread1.Sheets[0].Columns[7].Width = 270;
                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[9].Width = 100;
                FpSpread1.Sheets[0].Columns[10].Width = 100;
                FpSpread1.Sheets[0].Columns[11].Width = 100;
                FpSpread1.Sheets[0].Columns[12].Width = 230;
                FpSpread1.Sheets[0].Columns[13].Width = 70;
                FpSpread1.Sheets[0].Columns[14].Width = 70;
                FpSpread1.Sheets[0].Columns[15].Width = 70;
                FpSpread1.Sheets[0].Columns[16].Width = 70;
                FpSpread1.Sheets[0].Columns[17].Width = 70;
                FpSpread1.Sheets[0].Columns[18].Width = 70;
                FpSpread1.Sheets[0].Columns[19].Width = 150;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[10].Locked = true;
                FpSpread1.Sheets[0].Columns[11].Locked = true;
                FpSpread1.Sheets[0].Columns[12].Locked = true;
                FpSpread1.Sheets[0].Columns[13].Locked = true;
                FpSpread1.Sheets[0].Columns[14].Locked = true;
                FpSpread1.Sheets[0].Columns[15].Locked = true;
                FpSpread1.Sheets[0].Columns[16].Locked = true;
                FpSpread1.Sheets[0].Columns[17].Locked = true;
                FpSpread1.Sheets[0].Columns[18].Locked = true;
                FpSpread1.Sheets[0].Columns[19].Locked = true;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[11].Resizable = false;
                FpSpread1.Sheets[0].Columns[12].Resizable = false;
                FpSpread1.Sheets[0].Columns[13].Resizable = false;
                FpSpread1.Sheets[0].Columns[14].Resizable = false;
                FpSpread1.Sheets[0].Columns[15].Resizable = false;
                FpSpread1.Sheets[0].Columns[16].Resizable = false;
                FpSpread1.Sheets[0].Columns[17].Resizable = false;
                FpSpread1.Sheets[0].Columns[18].Resizable = false;
                FpSpread1.Sheets[0].Columns[19].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].Columns[10].Visible = true;
                FpSpread1.Sheets[0].Columns[11].Visible = true;
                FpSpread1.Sheets[0].Columns[12].Visible = true;
                FpSpread1.Sheets[0].Columns[13].Visible = true;
                FpSpread1.Sheets[0].Columns[14].Visible = true;
                FpSpread1.Sheets[0].Columns[15].Visible = true;
                FpSpread1.Sheets[0].Columns[16].Visible = true;
                FpSpread1.Sheets[0].Columns[17].Visible = true;
                FpSpread1.Sheets[0].Columns[18].Visible = true;
                FpSpread1.Sheets[0].Columns[19].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stream";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "EduLevel";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Course";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree Details";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Average";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "CGPA";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "CWAM";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Grade";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Rank";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Signature Of the HOD With Date";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 15, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 16, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 17, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 18, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 19, 2, 1);

                if (cblColumnOrder.Items.Count > 0)
                {
                    int index = 0;
                    if (ddlRankBy.SelectedIndex == 0)
                    {

                        cblColumnOrder.Items[15].Selected = true;
                        cblColumnOrder.Items[15].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[15].Enabled = true;
                    }
                    if (ddlRankBy.SelectedIndex == 1)
                    {

                        cblColumnOrder.Items[16].Selected = true;
                        cblColumnOrder.Items[16].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[16].Enabled = true;
                    }
                    if (ddlRankBy.SelectedIndex == 2)
                    {

                        cblColumnOrder.Items[14].Selected = true;
                        cblColumnOrder.Items[14].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[14].Enabled = true;
                    }
                    if (ddlRankBy.SelectedIndex == 3)
                    {

                        cblColumnOrder.Items[13].Selected = true;
                        cblColumnOrder.Items[13].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[13].Enabled = true;
                    }
                    cblColumnOrder.Items[18].Selected = true;
                    cblColumnOrder.Items[18].Enabled = false;
                    foreach (ListItem li in cblColumnOrder.Items)
                    {
                        string selVal = li.Value;
                        FpSpread1.Sheets[0].Columns[index].Visible = false;
                        if (li.Selected)
                        {
                            FpSpread1.Sheets[0].Columns[index].Visible = true;
                            switch (selVal)
                            {
                                case "0":
                                    break;
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                                case "6":
                                    break;
                                case "7":
                                    break;
                                case "8":
                                    break;
                                case "9":
                                    break;
                                case "10":
                                    break;
                                case "11":
                                    break;
                                case "12":
                                    break;
                                case "13":
                                    if (ddlRankBy.SelectedIndex == 3)
                                    {
                                        li.Selected = true;
                                        li.Enabled = false;
                                    }
                                    else
                                    {
                                        li.Enabled = true;
                                    }
                                    break;
                                case "14":
                                    if (ddlRankBy.SelectedIndex == 2)
                                    {
                                        li.Selected = true;
                                        li.Enabled = false;
                                    }
                                    else
                                    {
                                        li.Enabled = true;
                                    }
                                    break;
                                case "15":
                                    if (ddlRankBy.SelectedIndex == 0)
                                    {
                                        li.Selected = true;
                                        li.Enabled = false;
                                    }
                                    else
                                    {
                                        li.Enabled = true;
                                    }
                                    break;
                                case "16":
                                    if (ddlRankBy.SelectedIndex == 1)
                                    {
                                        li.Selected = true;
                                        li.Enabled = false;
                                    }
                                    else
                                    {
                                        li.Enabled = true;
                                    }
                                    break;
                                case "17":
                                    break;
                                case "18":
                                    li.Selected = true;
                                    li.Enabled = false;
                                    break;
                                case "19":
                                    break;
                                default:
                                    break;
                            }
                        }
                        index++;
                    }
                }
                FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(5, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(6, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(7, Farpoint.Model.MergePolicy.Always);
            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 19;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 330;
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 80;
                FpSpread1.Sheets[0].Columns[5].Width = 150;
                FpSpread1.Sheets[0].Columns[6].Width = 255;
                FpSpread1.Sheets[0].Columns[7].Width = 270;
                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[9].Width = 100;
                FpSpread1.Sheets[0].Columns[10].Width = 100;
                FpSpread1.Sheets[0].Columns[11].Width = 100;
                FpSpread1.Sheets[0].Columns[12].Width = 230;
                FpSpread1.Sheets[0].Columns[13].Width = 70;
                FpSpread1.Sheets[0].Columns[14].Width = 70;
                FpSpread1.Sheets[0].Columns[15].Width = 70;
                FpSpread1.Sheets[0].Columns[16].Width = 70;
                //FpSpread1.Sheets[0].Columns[17].Width = 70;
                //FpSpread1.Sheets[0].Columns[18].Width = 70;
                //FpSpread1.Sheets[0].Columns[19].Width = 150;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[10].Locked = true;
                FpSpread1.Sheets[0].Columns[11].Locked = true;
                FpSpread1.Sheets[0].Columns[12].Locked = true;
                FpSpread1.Sheets[0].Columns[13].Locked = true;
                FpSpread1.Sheets[0].Columns[14].Locked = true;
                FpSpread1.Sheets[0].Columns[15].Locked = true;
                FpSpread1.Sheets[0].Columns[16].Locked = true;
                //FpSpread1.Sheets[0].Columns[17].Locked = true;
                //FpSpread1.Sheets[0].Columns[18].Locked = true;
                //FpSpread1.Sheets[0].Columns[19].Locked = true;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[11].Resizable = false;
                FpSpread1.Sheets[0].Columns[12].Resizable = false;
                FpSpread1.Sheets[0].Columns[13].Resizable = false;
                FpSpread1.Sheets[0].Columns[14].Resizable = false;
                FpSpread1.Sheets[0].Columns[15].Resizable = false;
                FpSpread1.Sheets[0].Columns[16].Resizable = false;
                //FpSpread1.Sheets[0].Columns[17].Resizable = false;
                //FpSpread1.Sheets[0].Columns[18].Resizable = false;
                //FpSpread1.Sheets[0].Columns[19].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].Columns[10].Visible = true;
                FpSpread1.Sheets[0].Columns[11].Visible = true;
                FpSpread1.Sheets[0].Columns[12].Visible = true;
                FpSpread1.Sheets[0].Columns[13].Visible = true;
                FpSpread1.Sheets[0].Columns[14].Visible = true;
                FpSpread1.Sheets[0].Columns[15].Visible = true;
                FpSpread1.Sheets[0].Columns[16].Visible = true;
                //FpSpread1.Sheets[0].Columns[17].Visible = true;
                //FpSpread1.Sheets[0].Columns[18].Visible = true;
                //FpSpread1.Sheets[0].Columns[19].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stream";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "EduLevel";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Course";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree Details";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Average";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "CGPA";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "CWAM";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Grade";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Rank";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Signature Of the HOD With Date";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 15, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 16, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 17, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 18, 2, 1);
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 19, 2, 1);

                if (cblColumnOrder.Items.Count > 0)
                {
                    int index = 0;
                    if (ddlRankBySubject.SelectedIndex == 0)
                    {

                        cblColumnOrder.Items[15].Selected = true;
                        cblColumnOrder.Items[15].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[15].Enabled = true;
                    }
                    if (ddlRankBySubject.SelectedIndex == 1)
                    {

                        cblColumnOrder.Items[16].Selected = true;
                        cblColumnOrder.Items[16].Enabled = false;
                    }
                    else
                    {
                        cblColumnOrder.Items[16].Enabled = true;
                    }
                    cblColumnOrder.Items[18].Selected = true;
                    cblColumnOrder.Items[18].Enabled = false;
                    foreach (ListItem li in cblColumnOrder.Items)
                    {
                        string selVal = li.Value;
                        if (index <= 12)
                        {
                            FpSpread1.Sheets[0].Columns[index].Visible = false;
                            if (li.Selected)
                            {
                                FpSpread1.Sheets[0].Columns[index].Visible = true;
                                switch (selVal)
                                {
                                    case "0":
                                        break;
                                    case "1":
                                        break;
                                    case "2":
                                        break;
                                    case "3":
                                        break;
                                    case "4":
                                        break;
                                    case "5":
                                        break;
                                    case "6":
                                        break;
                                    case "7":
                                        break;
                                    case "8":
                                        break;
                                    case "9":
                                        break;
                                    case "10":
                                        break;
                                    case "11":
                                        break;
                                    case "12":
                                        break;
                                    case "13":
                                        if (ddlRankBySubject.SelectedIndex == 0)
                                        {
                                            li.Selected = true;
                                            li.Enabled = false;
                                        }
                                        else
                                        {
                                            li.Enabled = true;
                                        }
                                        break;
                                    case "14":
                                        if (ddlRankBy.SelectedIndex == 1)
                                        {
                                            li.Selected = true;
                                            li.Enabled = false;
                                        }
                                        else
                                        {
                                            li.Enabled = true;
                                        }
                                        break;
                                    case "18":
                                        li.Selected = true;
                                        li.Enabled = false;
                                        break;
                                    case "19":
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (li.Selected)
                            {
                                switch (selVal)
                                {
                                    case "13":
                                        FpSpread1.Sheets[0].Columns[15].Visible = true;
                                        if (ddlRankBySubject.SelectedIndex == 0)
                                        {
                                            li.Selected = true;
                                            li.Enabled = false;
                                        }
                                        else
                                        {
                                            li.Enabled = true;
                                        }
                                        break;
                                    case "14":
                                        FpSpread1.Sheets[0].Columns[16].Visible = true;
                                        if (ddlRankBy.SelectedIndex == 1)
                                        {
                                            li.Selected = true;
                                            li.Enabled = false;
                                        }
                                        else
                                        {
                                            li.Enabled = true;
                                        }
                                        break;
                                    case "18":
                                        FpSpread1.Sheets[0].Columns[17].Visible = true;
                                        li.Selected = true;
                                        li.Enabled = false;
                                        break;
                                    case "19":
                                        FpSpread1.Sheets[0].Columns[18].Visible = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        index++;
                    }
                }
                FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(5, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(6, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(7, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(13, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(14, Farpoint.Model.MergePolicy.Always);
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
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
            BindSem();
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
            BindSubject();
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
            BindParts();
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
            BindParts();
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlPartType_SelectedIndexChanged(object sender, EventArgs e)
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
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
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
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubject.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            lblSem.Visible = false;
            ddlSem.Visible = false;
            txtSem.Visible = false;
            pnlSem.Visible = false;

            lblSubject.Visible = false;
            txtSubject.Visible = false;
            pnlSubject.Visible = false;
            divGroupBy.Visible = false;
            divAll.Visible = false;
            divSubjectWise.Visible = false;

            ddlRankBy.Visible = false;
            ddlRankBySubject.Visible = false;

            ddlSubReportType.Visible = false;
            ddlSubReportType.SelectedIndex = 0;
            lblSubReportType.Visible = false;
            if (ddlReportType.Items.Count > 0)
            {
                int index = ddlReportType.SelectedIndex;
                string selectedValue = Convert.ToString(ddlReportType.SelectedItem.Value).Trim();
                switch (index)
                {
                    case 0:
                        lblSem.Visible = true;
                        ddlSem.Visible = true;
                        ddlRankBy.Visible = true;
                        ddlRankBySubject.Visible = false;
                        ddlRankBy.SelectedIndex = 0;
                        if (chkDepartmentWise.Checked)
                        {
                            divGroupBy.Visible = true;
                            divAll.Visible = true;
                            divSubjectWise.Visible = false;
                        }
                        break;
                    case 1:
                        lblSem.Visible = true;
                        txtSem.Visible = true;
                        pnlSem.Visible = true;
                        lblSubject.Visible = true;
                        txtSubject.Visible = true;
                        pnlSubject.Visible = true;
                        ddlRankBy.Visible = false;
                        ddlRankBySubject.Visible = true;
                        ddlRankBy.SelectedIndex = 0;
                        ddlSubReportType.Visible = true;
                        lblSubReportType.Visible = true;
                        ddlSubReportType.SelectedIndex = 0;
                        if (chkDepartmentWise.Checked)
                        {
                            divGroupBy.Visible = true;
                            divAll.Visible = false;
                            divSubjectWise.Visible = true;
                        }
                        break;
                    case 2:
                        ddlRankBy.Visible = true;
                        ddlRankBySubject.Visible = false;
                        ddlRankBy.SelectedIndex = 0;
                        if (chkDepartmentWise.Checked)
                        {
                            divGroupBy.Visible = true;
                            divAll.Visible = true;
                            divSubjectWise.Visible = false;
                        }
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
            }
            BindSem();
            BindParts();
            BindSubject();
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
            divGroupBy.Visible = false;
            if (chkDepartmentWise.Checked)
            {
                divMainContents.Visible = false;
                divGroupBy.Visible = false;
                divSubjectWise.Visible = false;
                divGroupBy.Visible = true;
                if (ddlReportType.Items.Count > 0)
                {
                    int index = ddlReportType.SelectedIndex;
                    string selectedValue = Convert.ToString(ddlReportType.SelectedItem.Value).Trim();
                    switch (index)
                    {
                        case 0:
                            lblSem.Visible = true;
                            ddlSem.Visible = true;
                            if (chkDepartmentWise.Checked)
                            {
                                divGroupBy.Visible = true;
                                divAll.Visible = true;
                                divSubjectWise.Visible = false;
                            }
                            break;
                        case 1:
                            lblSem.Visible = true;
                            txtSem.Visible = true;
                            pnlSem.Visible = true;
                            lblSubject.Visible = true;
                            txtSubject.Visible = true;
                            pnlSubject.Visible = true;
                            if (chkDepartmentWise.Checked)
                            {
                                divGroupBy.Visible = true;
                                divAll.Visible = false;
                                divSubjectWise.Visible = true;
                            }
                            break;
                        case 2:
                            if (chkDepartmentWise.Checked)
                            {
                                divGroupBy.Visible = true;
                                divAll.Visible = true;
                                divSubjectWise.Visible = false;
                            }
                            break;
                        case 3:
                            break;
                        default:
                            break;
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

    protected void chkGroupBy_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkGroupBy, cblGroupBy, txtGroupBy, lblGroupBy.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblGroupBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkGroupBy, cblGroupBy, txtGroupBy, lblGroupBy.Text, "--Select--");

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSubGroupBy_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSubGroupBy, cblSubGroupBy, txtSubGroupBy, lblGroupBy.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSubGroupBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSubGroupBy, cblSubGroupBy, txtSubGroupBy, lblGroupBy.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Index Changed Events

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

            string qryRedoBatch = string.Empty;
            string qryRedoDegreeCode = string.Empty;
            bool isRedoStud = true;

            partNos = string.Empty;
            partNames = string.Empty;
            topValues = string.Empty;

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
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblEduLevel.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}
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
                    qrySemester = " and sm.semester in(" + semesters + ")";
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
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlPartType.Items.Count > 0)
            {
                partNos = Convert.ToString(ddlPartType.SelectedValue).Trim();
                if (partNos.Trim() != "0")
                {
                    if (!string.IsNullOrEmpty(partNos))
                    {
                        qryPartTypeNos = " and s.Part_Type in(" + partNos + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblPartType.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblPartType.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (cblSubject.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSubject.Text.Trim() + "  Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                subjectCodes = getCblSelectedValue(cblSubject);
                subjectNames = getCblSelectedText(cblSubject);
                int selCount = getCblSelectedCount(cblSubject);
                if (!string.IsNullOrEmpty(subjectCodes))
                {
                    if (selCount != cblSubject.Items.Count)
                    {
                        qrySubjectCodes = " and s.subject_code in (" + subjectCodes + ")";
                    }
                }
                else
                {
                }
                if (!string.IsNullOrEmpty(subjectNames))
                {
                    if (selCount != cblSubject.Items.Count)
                    {
                        qrySubjectNames = " and s.subject_name in (" + subjectNames + ")";
                    }
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubject.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }


            DataSet dsRankDetails = new DataSet();
            DataTable dtRankDetails = new DataTable();
            string qryCalculateSemester = string.Empty;
            string qryCalculatePart = string.Empty;
            string qryPartitionBy = string.Empty;
            string qryRankBy = string.Empty;
            string qryOrderBy = string.Empty;
            string qryRankFilter = string.Empty;
            string qryFailCountSub = string.Empty;
            bool isSubjectWise = false;
            if (!string.IsNullOrEmpty(txtTop.Text.Trim()) && txtTop.Text.Trim() != "0")
            {
                topValues = Convert.ToString(txtTop.Text).Trim();
                top = 0;
                int.TryParse(topValues.Trim(), out top);
                if (top <= 0)
                {
                    lblAlertMsg.Text = lblTop.Text.Trim() + " Must Greater Than Zero's And Must Be Numeric";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    qryRankFilter = " rank<='" + top + "'";
                }
            }
            switch (partNos)
            {
                case "0":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                case "1":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                case "2":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                case "3":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                case "4":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                case "5":
                    qryCalculatePart = " and cg.type='" + Convert.ToString(partNos).Trim() + "'";
                    break;
                default:
                    qryCalculatePart = " and cg.type='" + Convert.ToString("0").Trim() + "'";
                    break;
            }
            if (ddlReportType.Items.Count > 0)
            {
                int index = 0;
                index = ddlReportType.SelectedIndex;
                string selectedValue = Convert.ToString(ddlReportType.SelectedItem.Value).Trim();
                switch (index)
                {
                    case 0:
                        if (ddlSem.Visible == true)
                        {
                            if (ddlSem.Items.Count > 0)
                            {
                                qryCalculateSemester = " and cg.Semester='" + Convert.ToString(ddlSem.SelectedItem.Value).Trim() + "'";
                            }
                        }
                        break;
                    case 1:
                        isSubjectWise = true;
                        qryFailCountSub = qrySubjectCodes;
                        break;
                    case 2:
                        qryCalculateSemester = " and cg.Semester='0'";
                        break;
                    default:
                        break;
                }
            }
            CheckBoxList cblGrpNew = new CheckBoxList();
            bool isAvg = false;
            if (isSubjectWise)
            {
                if (ddlRankBySubject.Items.Count > 0)
                {
                    int index = 0;
                    qryRankBy = string.Empty;
                    index = ddlRankBySubject.SelectedIndex;
                    string selectedValue = Convert.ToString(ddlRankBySubject.SelectedItem.Value).Trim();
                    switch (index)
                    {
                        case 0:
                            isAvg = false;
                            qryRankBy = "0";
                            break;
                        case 1:
                            isAvg = true;
                            qryRankBy = "1";
                            break;
                    }
                }
                cblGrpNew = cblSubGroupBy;
            }
            else
            {
                if (ddlRankBy.Items.Count > 0)
                {
                    int index = 0;
                    qryRankBy = string.Empty;
                    index = ddlRankBy.SelectedIndex;
                    string selectedValue = Convert.ToString(ddlRankBy.SelectedItem.Value).Trim();
                    switch (index)
                    {
                        case 0:
                            qryRankBy = "cg.SemWiseCGpa";
                            break;
                        case 1:
                            qryRankBy = "cg.SemWiseCwam";
                            break;
                        case 2:
                            qryRankBy = "cg.Average";
                            break;
                        case 3:
                            qryRankBy = "cg.TotalObtainedMarks";
                            break;
                        default:
                            qryRankBy = "cg.SemWiseCGpa";
                            break;
                    }
                }
                cblGrpNew = cblGroupBy;
            }

            qryOrderBy = "order by rank";
            string qryRedoPartition = string.Empty;
            if (chkDepartmentWise.Checked)
            {
                //qryPartitionBy = " partition by dt.Dept_Name ";
                //qryOrderBy = " order by dt.Dept_Name,rank";
                string partitions = string.Empty;
                string partitionRedo = string.Empty;
                string orderBy = string.Empty;
                if (cblGroupBy.Items.Count > 0)
                {
                    foreach (ListItem liG in cblGrpNew.Items)
                    {
                        string value = liG.Value.Trim();
                        if (liG.Selected)
                        {
                            switch (value)
                            {
                                case "0":
                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " clg.college_code,";
                                        orderBy = " clg.college_code,";
                                        partitionRedo = " clg.college_code,";
                                    }
                                    else
                                    {
                                        partitions += " clg.college_code,";
                                        orderBy += " clg.college_code,";
                                        partitionRedo += " clg.college_code,";
                                    }

                                    break;
                                case "1":
                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " c.type,";
                                        orderBy = " type,";
                                        partitionRedo = " c.type,";
                                    }
                                    else
                                    {
                                        partitions += " c.type,";
                                        orderBy += " type,";
                                        partitionRedo += " c.type,";
                                    }

                                    break;
                                case "2":
                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " c.Edu_Level,";
                                        orderBy = " c.Edu_Level,";
                                        partitionRedo = " c.Edu_Level,";
                                    }
                                    else
                                    {
                                        partitions += " c.Edu_Level,";
                                        orderBy += " c.Edu_Level,";
                                        partitionRedo += " c.Edu_Level,";
                                    }

                                    break;
                                case "3":

                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " r.Batch_Year,";
                                        partitionRedo = " sr.BatchYear,";
                                        orderBy = " Batch_Year,";
                                    }
                                    else
                                    {
                                        partitions += " r.Batch_Year,";
                                        partitionRedo += " sr.BatchYear,";
                                        orderBy += " Batch_Year,";
                                    }

                                    break;
                                case "4":
                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " dt.Dept_Name,";
                                        partitionRedo = " dt.Dept_Name,";
                                        orderBy = " dt.Dept_Name,";
                                    }
                                    else
                                    {
                                        partitions += " dt.Dept_Name,";
                                        partitionRedo += " dt.Dept_Name,";
                                        orderBy += " dt.Dept_Name,";
                                    }
                                    break;
                                case "5":
                                    if (string.IsNullOrEmpty(partitions))
                                    {
                                        partitions = " dg.degree_code,";
                                        partitionRedo = " dg.degree_code,";
                                        orderBy = " degree_code,";
                                    }
                                    else
                                    {
                                        partitions += " dg.degree_code,";
                                        partitionRedo += " dg.degree_code,";
                                        orderBy += " degree_code,";
                                    }

                                    break;
                                case "6":
                                    if (isSubjectWise)
                                    {
                                        if (ddlSubReportType.Items.Count > 0)
                                        {
                                            int index = ddlSubReportType.SelectedIndex;
                                            switch (index)
                                            {
                                                case 0:
                                                    if (string.IsNullOrEmpty(partitions))
                                                    {
                                                        partitions = " s.subject_name,";
                                                        partitionRedo = " s.subject_name,";
                                                        orderBy = " s.subject_name,";
                                                    }
                                                    else
                                                    {
                                                        partitions += " s.subject_name,";
                                                        partitionRedo += "  s.subject_name,";
                                                        orderBy += "  s.subject_name,";
                                                    }
                                                    break;
                                                case 1:
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case "7":
                                    if (isSubjectWise)
                                    {

                                        if (ddlSubReportType.Items.Count > 0)
                                        {
                                            int index = ddlSubReportType.SelectedIndex;
                                            switch (index)
                                            {
                                                case 0:
                                                    if (string.IsNullOrEmpty(partitions))
                                                    {
                                                        partitions = " s.subject_code,";
                                                        partitionRedo = " s.subject_code,";
                                                        orderBy = " s.subject_code,";
                                                    }
                                                    else
                                                    {
                                                        partitions += "  s.subject_code,";
                                                        partitionRedo += "  s.subject_code,";
                                                        orderBy += "  s.subject_code,";
                                                    }
                                                    break;
                                                case 1:
                                                    break;
                                            }
                                        }

                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(partitions.Trim().Trim(',')))
                {
                    qryPartitionBy = " partition by " + partitions.Trim(',');
                }
                if (!string.IsNullOrEmpty(partitionRedo.Trim().Trim(',')))
                {
                    qryRedoPartition = " partition by " + partitionRedo.Trim(',');
                }
                qryOrderBy = " order by " + orderBy + "rank";
            }

            if (!string.IsNullOrEmpty(qryRankBy))
            {
                string failedAbsentStudents = string.Empty;
                //qry = "select m.roll_no from mark_entry m,Subject s,syllabus_master sm,subjectChooser sc,sub_sem ss where m.roll_no=sc.roll_no and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and s.subject_no=m.subject_no and s.syll_code=sm.syll_code and ss.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and  m.result='fail' " + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub + " group by m.roll_no union select m.roll_no from mark_entry m,Subject s,syllabus_master sm,subjectChooser sc,sub_sem ss where m.roll_no=sc.roll_no and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and s.subject_no=m.subject_no and s.syll_code=sm.syll_code and ss.syll_code=s.syll_code and sm.syll_code=ss.syll_code and s.subType_no=ss.subType_no and  m.result='aaa' " + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub + " group by m.roll_no having Count(*)>1";
                qry = "select m.roll_no from mark_entry m,Subject s,syllabus_master sm where s.subject_no=m.subject_no and s.syll_code=sm.syll_code and  m.result='fail' " + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub + " group by m.roll_no union select m.roll_no from mark_entry m,Subject s,syllabus_master sm where s.subject_no=m.subject_no and s.syll_code=sm.syll_code and  m.result='aaa'  " + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub + " group by m.roll_no having Count(*)>1";
                DataSet dsFailedAndAbsentDetails = new DataSet();
                dsFailedAndAbsentDetails = da.select_method_wo_parameter(qry, "text");
                if (dsFailedAndAbsentDetails.Tables.Count > 0 && dsFailedAndAbsentDetails.Tables[0].Rows.Count > 0)
                {
                    List<string> list = dsFailedAndAbsentDetails.Tables[0].AsEnumerable()
                                                       .Select(r => Convert.ToString(r.Field<string>("roll_no")))
                                                       .ToList();
                    list.Distinct();
                    string failedRollNos = string.Join("','", list.ToArray());
                    if (!string.IsNullOrEmpty(failedRollNos))
                    {
                        failedAbsentStudents = " and r.Roll_No not in(" + qry + ")";
                        //failedAbsentStudents = " and r.Roll_No not in('" + failedRollNos + "')";
                    }
                }

                if (!isSubjectWise)
                {
                    qry = "select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,r.Batch_Year,dg.degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,cg.TotalObtainedMarks,cg.TotalMarks,cg.Average,cg.TotalEarnedCredits,cg.TotalGradePoints,cg.TotalWeightageMark,cg.SemWiseWam,cg.SemWiseGpa,cg.SemWiseCwam,cg.SemWiseCGpa,cg.cgpaGrade,cg.cgpaClassification,dense_rank() over(" + qryPartitionBy + " order by (" + qryRankBy + ") desc) as rank,r.Current_Semester from CalculateSemWiseGPA_CGPA cg,Registration r,Course c,Degree dg,Department dt,collinfo clg where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=r.degree_code and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code and r.App_No=cg.app_no " + failedAbsentStudents + qryCalculateSemester + qryCalculatePart + qryCollege + qryBatch + qryDegreeCode;

                    //qry = "select r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,cg.TotalObtainedMarks,cg.TotalMarks,cg.Average,cg.TotalEarnedCredits,cg.TotalGradePoints,cg.TotalWeightageMark,cg.SemWiseWam,cg.SemWiseGpa,cg.SemWiseCwam,cg.SemWiseCGpa,cg.cgpaGrade,cg.cgpaClassification,dense_rank() over(" + qryPartitionBy + " order by (" + qryRankBy + ") desc) as rank from CalculateSemWiseGPA_CGPA cg,Registration r where r.App_No=cg.app_no " + failedAbsentStudents + qryCalculateSemester + qryCalculatePart + qryCollege + qryBatch + qryDegreeCode;

                    string qryStud = "select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,r.Batch_Year,c.Course_Id,dt.Dept_Code,r.degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Current_Semester from Registration r,Course c,Degree dg,Department dt,collinfo clg where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=r.degree_code and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code " + qryCollege + qryBatch + qryDegreeCode;
                    if (isRedoStud)
                    {
                        qry += " union select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,sr.BatchYear as Batch_Year,dg.degree_code as degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,cg.TotalObtainedMarks,cg.TotalMarks,cg.Average,cg.TotalEarnedCredits,cg.TotalGradePoints,cg.TotalWeightageMark,cg.SemWiseWam,cg.SemWiseGpa,cg.SemWiseCwam,cg.SemWiseCGpa,cg.cgpaGrade,cg.cgpaClassification,dense_rank() over(" + qryRedoPartition + " order by (" + qryRankBy + ") desc) as rank,r.Current_Semester from CalculateSemWiseGPA_CGPA cg,Registration r,Course c,Degree dg,Department dt,collinfo clg,StudentRedoDetails sr where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and sr.Stud_AppNo=r.App_No and  dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=sr.DegreeCode and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code and r.App_No=cg.app_no " + failedAbsentStudents + qryCalculateSemester + qryCalculatePart + qryCollege + qryRedoBatch + qryRedoDegreeCode;
                        //qry += " union select r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,cg.TotalObtainedMarks,cg.TotalMarks,cg.Average,cg.TotalEarnedCredits,cg.TotalGradePoints,cg.TotalWeightageMark,cg.SemWiseWam,cg.SemWiseGpa,cg.SemWiseCwam,cg.SemWiseCGpa,cg.cgpaGrade,cg.cgpaClassification,dense_rank() over(" + qryPartitionBy + " order by (" + qryRankBy + ") desc) as rank from CalculateSemWiseGPA_CGPA cg,Registration r,StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and r.App_No=cg.app_no " + failedAbsentStudents + qryCalculateSemester + qryCalculatePart + qryCollege + qryRedoBatch + qryRedoDegreeCode;
                        qryStud += "union select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,sr.BatchYear as Batch_Year,c.Course_Id,dt.Dept_Code,sr.DegreeCode as degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Current_Semester from Registration r,Course c,Degree dg,Department dt,collinfo clg,StudentRedoDetails sr where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and sr.Stud_AppNo=r.App_No and  dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=sr.DegreeCode and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code " + qryCollege + qryRedoBatch + qryRedoDegreeCode;
                    }
                    qry += qryOrderBy;
                    dsRankDetails = da.select_method_wo_parameter(qry, "text");
                    DataSet dsStudentList = new DataSet();
                    dsStudentList = da.select_method_wo_parameter(qryStud, "text");
                    if (dsRankDetails.Tables.Count > 0 && dsRankDetails.Tables[0].Rows.Count > 0)
                    {
                        dtRankDetails = new DataTable();
                        dsRankDetails.Tables[0].DefaultView.RowFilter = qryRankFilter;
                        dtRankDetails = dsRankDetails.Tables[0].DefaultView.ToTable(true);
                        if (dtRankDetails.Rows.Count > 0)
                        {
                            Init_Spread(FpStudentsRankList, 0);
                            FpStudentsRankList.Sheets[0].RowCount = 0;
                            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();

                            string collegeCode = string.Empty;
                            string collegeName = string.Empty;
                            string batchYear = string.Empty;
                            string streamName = string.Empty;
                            string eduLevel = string.Empty;
                            string courseName = string.Empty;
                            string courseId = string.Empty;
                            string departmentName = string.Empty;
                            string deptCode = string.Empty;
                            string departmentAcronymn = string.Empty;

                            string degreeDetails = string.Empty;
                            string semester = string.Empty;
                            string regNo = string.Empty;
                            string rollNo = string.Empty;
                            string admitNo = string.Empty;
                            string studentName = string.Empty;
                            string totalSecuredMarks = string.Empty;
                            string totalMarks = string.Empty;
                            string averageMarks = string.Empty;
                            string cgpa = string.Empty;
                            string cwam = string.Empty;
                            string grade = string.Empty;
                            string rank = string.Empty;
                            int serialNo = 0;
                            foreach (DataRow drRankList in dtRankDetails.Rows)
                            {
                                string appNo = Convert.ToString(drRankList["App_No"]).Trim();
                                totalSecuredMarks = Convert.ToString(drRankList["TotalObtainedMarks"]).Trim();
                                totalMarks = Convert.ToString(drRankList["TotalMarks"]).Trim();
                                averageMarks = Convert.ToString(drRankList["Average"]).Trim();
                                string creditEarned = Convert.ToString(drRankList["TotalEarnedCredits"]).Trim();
                                string totalGradePoint = Convert.ToString(drRankList["TotalGradePoints"]).Trim();
                                string totalWeightageMarks = Convert.ToString(drRankList["TotalWeightageMark"]).Trim();
                                string semWiseWAM = Convert.ToString(drRankList["SemWiseWam"]).Trim();
                                string semWiseGPA = Convert.ToString(drRankList["SemWiseGpa"]).Trim();
                                cgpa = Convert.ToString(drRankList["SemWiseCGpa"]).Trim();
                                cwam = Convert.ToString(drRankList["SemWiseCwam"]).Trim();
                                grade = Convert.ToString(drRankList["cgpaGrade"]).Trim();
                                string classification = Convert.ToString(drRankList["cgpaClassification"]).Trim();
                                rank = Convert.ToString(drRankList["rank"]).Trim();

                                DataView dvStudents = new DataView();
                                if (dsStudentList.Tables.Count > 0 && dsStudentList.Tables[0].Rows.Count > 0)
                                {
                                    dsStudentList.Tables[0].DefaultView.RowFilter = "App_No='" + appNo + "'";
                                    dvStudents = dsStudentList.Tables[0].DefaultView;
                                }

                                if (dvStudents.Count > 0)
                                {
                                    collegeCode = Convert.ToString(dvStudents[0]["college_code"]).Trim();
                                    collegeName = Convert.ToString(dvStudents[0]["collname"]).Trim();
                                    batchYear = Convert.ToString(dvStudents[0]["Batch_Year"]).Trim();
                                    streamName = Convert.ToString(dvStudents[0]["type"]).Trim();
                                    eduLevel = Convert.ToString(dvStudents[0]["Edu_Level"]).Trim();
                                    courseName = Convert.ToString(dvStudents[0]["Course_Name"]).Trim();
                                    //courseId = Convert.ToString(dvStudents[0]["Course_Id"]).Trim();
                                    departmentName = Convert.ToString(dvStudents[0]["Dept_Name"]).Trim();
                                    //deptCode = Convert.ToString(dvStudents[0]["Dept_Code"]).Trim();
                                    departmentAcronymn = Convert.ToString(dvStudents[0]["dept_acronym"]).Trim();
                                    degreeDetails = Convert.ToString(dvStudents[0]["DegreeDetails"]).Trim();
                                    semester = Convert.ToString(dvStudents[0]["Current_Semester"]).Trim();
                                    regNo = Convert.ToString(dvStudents[0]["Reg_No"]).Trim();
                                    rollNo = Convert.ToString(dvStudents[0]["Roll_No"]).Trim();
                                    admitNo = Convert.ToString(dvStudents[0]["Roll_Admit"]).Trim();
                                    studentName = Convert.ToString(dvStudents[0]["Stud_Name"]).Trim();
                                    string degreeCode = Convert.ToString(dvStudents[0]["degree_code"]).Trim();

                                    FpStudentsRankList.Sheets[0].RowCount++;
                                    serialNo++;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(eduLevel).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(courseName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(collegeName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(collegeCode).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(deptCode).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(batchYear).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(streamName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(eduLevel).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(eduLevel).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(eduLevel).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(courseName).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(courseId).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseId).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(departmentName).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(deptCode).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(departmentAcronymn).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(degreeDetails).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(degreeCode).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Note = Convert.ToString(degreeCode).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(semester).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(semester).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Note = Convert.ToString(semester).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(regNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(regNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Note = Convert.ToString(regNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(rollNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(appNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Note = Convert.ToString(rollNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(admitNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Tag = Convert.ToString(admitNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Note = Convert.ToString(admitNo).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(studentName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Tag = Convert.ToString(studentName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Note = Convert.ToString(studentName).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(totalSecuredMarks).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Tag = Convert.ToString(totalMarks).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(averageMarks).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Tag = Convert.ToString(totalGradePoint).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Note = Convert.ToString(totalWeightageMarks).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(cgpa).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Tag = Convert.ToString(semWiseGPA).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Note = Convert.ToString(semWiseWAM).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(cwam).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Tag = Convert.ToString(semWiseWAM).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(grade).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Tag = Convert.ToString(classification).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Note = Convert.ToString(classification).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Text = Convert.ToString(rank).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Tag = Convert.ToString(collegeCode).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].Text = Convert.ToString("").Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].HorizontalAlign = HorizontalAlign.Left;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 19].VerticalAlign = VerticalAlign.Middle;
                                }
                            }
                            divMainContents.Visible = true;
                            FpStudentsRankList.Sheets[0].PageSize = FpStudentsRankList.Sheets[0].RowCount;
                            FpStudentsRankList.Width = 980;
                            FpStudentsRankList.SaveChanges();
                            FpStudentsRankList.Visible = true;
                        }
                        else
                        {
                            lblAlertMsg.Text = "No Record(s) Were Found!!!";
                            divPopAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record(s) Were Found!!! Please Check The GPA And CGPA Calculation";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    string qryRank = string.Empty;
                    string outOff100 = string.Empty;
                    outOff100 = " ROUND(CAST(case when (ISNULL(s.maxtotal,'0')<>'0' and ISNULL(m.total,'0')>=0) then ((ISNULL(m.total,'0') /s.maxtotal)*100) else '0' end AS DECIMAL(10,1)) ,1,0)";
                    string total = string.Empty;
                    isAvg = false;
                    bool isGroupByAll = false;
                    string groupBYAll = string.Empty;
                    string subjectInd = string.Empty;
                    bool isShowSubjectName = false;
                    if (ddlRankBySubject.Items.Count > 0)
                    {
                        int index = ddlRankBySubject.SelectedIndex;
                        switch (index)
                        {
                            case 0:
                                subjectInd = "s.subject_code,s.subject_name";
                                total = outOff100;
                                isShowSubjectName = true;
                                break;
                            case 1:
                                isShowSubjectName = false;
                                isAvg = true;
                                break;
                        }
                    }
                    if (ddlSubReportType.Items.Count > 0)
                    {
                        int index = ddlSubReportType.SelectedIndex;
                        switch (index)
                        {
                            case 0:
                                subjectInd = "s.subject_code,s.subject_name";
                                total = outOff100;
                                isShowSubjectName = true;
                                break;
                            case 1:
                                isGroupByAll = true;
                                subjectInd = string.Empty;
                                isShowSubjectName = false;
                                string count = " count(s.subject_code) ";
                                total = " ROUND(CAST( SUM(" + outOff100 + ") " + ((isAvg) ? "/" + count : "") + " AS DECIMAL(10,2)) ,2,0)";
                                break;
                        }
                    }
                    if (isGroupByAll)
                    {
                        groupBYAll = " group by clg.college_code,collname,sm.Batch_Year,dg.degree_code,c.type,Edu_Level,Course_Name,Dept_Name,dept_acronym,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Current_Semester ";
                    }
                    qry = "select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,sm.Batch_Year,dg.Degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Current_Semester," + ((string.IsNullOrEmpty(subjectInd)) ? "" : subjectInd + ",") + total + " as Total,dense_rank() over(" + qryPartitionBy + " order by (" + total + ") desc) as rank from Registration r,Course c,Degree dg,Department dt,collinfo clg,subject s,mark_entry m,syllabus_master sm where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=r.degree_code and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code and s.subject_no=m.subject_no and s.syll_code=sm.syll_code and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and m.roll_no=r.Roll_No and s.maxtotal>0 and isnull(total,'0')>0 and m.result='pass' " + failedAbsentStudents + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub + groupBYAll;

                    string qryRedo = " union select clg.college_code,clg.collname+'('+ltrim(rtrim(isnull(c.type,'')))+')' as collname,sm.Batch_Year as Batch_Year,dg.Degree_code as degree_code,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Current_Semester," + ((string.IsNullOrEmpty(subjectInd)) ? "" : subjectInd + ",") + total + " as Total,dense_rank() over(" + qryRedoPartition + " order by (" + total + ") desc) as rank from Registration r,Course c,Degree dg,Department dt,collinfo clg,StudentRedoDetails sr,subject s,mark_entry m,syllabus_master sm where clg.college_code=c.college_code and clg.college_code=r.college_code and clg.college_code=dg.college_code and clg.college_code=dt.college_code and sr.Stud_AppNo=r.App_No and  dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=sr.DegreeCode and c.Course_Id=dg.Course_Id and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=r.college_code and r.college_code=dt.college_code and s.subject_no=m.subject_no and s.syll_code=sm.syll_code and m.roll_no=r.Roll_No and sm.Batch_Year=sr.BatchYear and sr.DegreeCode=sr.DegreeCode  and s.maxtotal>0 and isnull(total,'0')>0 and m.result='pass'" + failedAbsentStudents + ((!string.IsNullOrEmpty(batchYears)) ? " and sm.batch_year in(" + batchYears + ")" : "") + ((!string.IsNullOrEmpty(degreeCodes)) ? " and sm.degree_code in(" + degreeCodes + ")" : "") + qrySemester + qryPartTypeNos + qryFailCountSub;
                    if (isRedoStud)
                    {
                        qry += qryRedo + groupBYAll + qryOrderBy;
                    }

                    dsRankDetails = da.select_method_wo_parameter(qry, "text");
                    if (dsRankDetails.Tables.Count > 0 && dsRankDetails.Tables[0].Rows.Count > 0)
                    {
                        dtRankDetails = new DataTable();
                        dsRankDetails.Tables[0].DefaultView.RowFilter = qryRankFilter;
                        dtRankDetails = dsRankDetails.Tables[0].DefaultView.ToTable(true);
                        if (dtRankDetails.Rows.Count > 0)
                        {
                            Init_Spread(FpStudentsRankList, 1);
                            FpStudentsRankList.Sheets[0].RowCount = 0;
                            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();

                            string collegeCode = string.Empty;
                            string collegeName = string.Empty;
                            string batchYear = string.Empty;
                            string streamName = string.Empty;
                            string eduLevel = string.Empty;
                            string courseName = string.Empty;
                            string courseId = string.Empty;
                            string departmentName = string.Empty;
                            string deptCode = string.Empty;
                            string departmentAcronymn = string.Empty;

                            string degreeDetails = string.Empty;
                            string semester = string.Empty;
                            string regNo = string.Empty;
                            string rollNo = string.Empty;
                            string admitNo = string.Empty;
                            string studentName = string.Empty;
                            string totalSecuredMarks = string.Empty;
                            string totalMarks = string.Empty;
                            string averageMarks = string.Empty;
                            string cgpa = string.Empty;
                            string cwam = string.Empty;
                            string grade = string.Empty;
                            string rank = string.Empty;
                            int serialNo = 0;
                            ArrayList arrSubject = new ArrayList();
                            foreach (DataRow drRankList in dtRankDetails.Rows)
                            {
                                string appNo = Convert.ToString(drRankList["App_No"]).Trim();
                                totalSecuredMarks = Convert.ToString(drRankList["Total"]).Trim();
                                //totalMarks = Convert.ToString(drRankList["TotalMarks"]).Trim();
                                averageMarks = Convert.ToString(drRankList["Total"]).Trim();
                                //string creditEarned = Convert.ToString(drRankList["TotalEarnedCredits"]).Trim();
                                //string totalGradePoint = Convert.ToString(drRankList["TotalGradePoints"]).Trim();
                                //string totalWeightageMarks = Convert.ToString(drRankList["TotalWeightageMark"]).Trim();
                                //string semWiseWAM = Convert.ToString(drRankList["SemWiseWam"]).Trim();
                                //string semWiseGPA = Convert.ToString(drRankList["SemWiseGpa"]).Trim();
                                //cgpa = Convert.ToString(drRankList["SemWiseCGpa"]).Trim();
                                //cwam = Convert.ToString(drRankList["SemWiseCwam"]).Trim();
                                //grade = Convert.ToString(drRankList["cgpaGrade"]).Trim();
                                //string classification = Convert.ToString(drRankList["cgpaClassification"]).Trim();
                                rank = Convert.ToString(drRankList["rank"]).Trim();


                                collegeCode = Convert.ToString(drRankList["college_code"]).Trim();
                                collegeName = Convert.ToString(drRankList["collname"]).Trim();
                                batchYear = Convert.ToString(drRankList["Batch_Year"]).Trim();
                                streamName = Convert.ToString(drRankList["type"]).Trim();
                                eduLevel = Convert.ToString(drRankList["Edu_Level"]).Trim();
                                courseName = Convert.ToString(drRankList["Course_Name"]).Trim();
                                //courseId = Convert.ToString(drRankList["Course_Id"]).Trim();
                                departmentName = Convert.ToString(drRankList["Dept_Name"]).Trim();
                                //deptCode = Convert.ToString(drRankList["Dept_Code"]).Trim();
                                departmentAcronymn = Convert.ToString(drRankList["dept_acronym"]).Trim();
                                degreeDetails = Convert.ToString(drRankList["DegreeDetails"]).Trim();
                                semester = Convert.ToString(drRankList["Current_Semester"]).Trim();
                                regNo = Convert.ToString(drRankList["Reg_No"]).Trim();
                                rollNo = Convert.ToString(drRankList["Roll_No"]).Trim();
                                admitNo = Convert.ToString(drRankList["Roll_Admit"]).Trim();
                                studentName = Convert.ToString(drRankList["Stud_Name"]).Trim();
                                string degreeCode = Convert.ToString(drRankList["degree_code"]).Trim();

                                FpStudentsRankList.Sheets[0].RowCount++;
                                serialNo++;
                                string subjectNameOrCode = string.Empty;
                                if (isShowSubjectName)
                                {
                                    subjectNameOrCode = Convert.ToString(drRankList["subject_name"]).Trim() + "[ " + Convert.ToString(drRankList["subject_code"]).Trim() + " ]";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(drRankList["subject_name"]).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Tag = Convert.ToString(drRankList["subject_name"]).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;

                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(drRankList["subject_code"]).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Tag = Convert.ToString(totalGradePoint).Trim();
                                    //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Note = Convert.ToString(totalWeightageMarks).Trim();
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].CellType = txtCell;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Locked = true;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                                    FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;
                                    FpStudentsRankList.Sheets[0].Columns[13].Visible = true;
                                    FpStudentsRankList.Sheets[0].Columns[14].Visible = true;
                                }
                                else
                                {
                                    FpStudentsRankList.Sheets[0].Columns[13].Visible = false;
                                    FpStudentsRankList.Sheets[0].Columns[14].Visible = false;
                                }
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(eduLevel).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(courseName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(collegeName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(collegeCode).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(deptCode).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(batchYear).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(streamName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(eduLevel).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(eduLevel).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(eduLevel).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(courseName).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(courseId).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseId).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(departmentName).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(deptCode).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(departmentAcronymn).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(degreeDetails).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(degreeCode).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Note = Convert.ToString(degreeCode).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(semester).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(semester).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Note = Convert.ToString(semester).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(regNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(regNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Note = Convert.ToString(regNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(rollNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(appNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Note = Convert.ToString(rollNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(admitNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Tag = Convert.ToString(admitNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Note = Convert.ToString(admitNo).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(studentName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Tag = Convert.ToString(studentName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Note = Convert.ToString(studentName).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;

                                if (qryRankBy.Trim() == "0")
                                {
                                    FpStudentsRankList.Sheets[0].Columns[15].Visible = true;
                                    FpStudentsRankList.Sheets[0].Columns[16].Visible = false;
                                }
                                else
                                {
                                    FpStudentsRankList.Sheets[0].Columns[15].Visible = false;
                                    FpStudentsRankList.Sheets[0].Columns[16].Visible = true;
                                }
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(totalSecuredMarks).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Tag = Convert.ToString(totalMarks).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(averageMarks).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Tag = Convert.ToString(totalGradePoint).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 14].Note = Convert.ToString(totalWeightageMarks).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].VerticalAlign = VerticalAlign.Middle;

                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(cgpa).Trim();
                                ////FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Tag = Convert.ToString(semWiseGPA).Trim();
                                ////FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 15].Note = Convert.ToString(semWiseWAM).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].CellType = txtCell;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Locked = true;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].VerticalAlign = VerticalAlign.Middle;

                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Text = Convert.ToString(cwam).Trim();
                                ////FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 16].Tag = Convert.ToString(semWiseWAM).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].CellType = txtCell;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Font.Name = "Book Antiqua";
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Locked = true;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].HorizontalAlign = HorizontalAlign.Center;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].VerticalAlign = VerticalAlign.Middle;

                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(grade).Trim();
                                ////FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Tag = Convert.ToString(classification).Trim();
                                ////FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Note = Convert.ToString(classification).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].CellType = txtCell;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Locked = true;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(rank).Trim();
                                //FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Tag = Convert.ToString(collegeCode).Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].HorizontalAlign = HorizontalAlign.Center;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 17].VerticalAlign = VerticalAlign.Middle;

                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Text = Convert.ToString("").Trim();
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].CellType = txtCell;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Font.Name = "Book Antiqua";
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].Locked = true;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].HorizontalAlign = HorizontalAlign.Left;
                                FpStudentsRankList.Sheets[0].Cells[FpStudentsRankList.Sheets[0].RowCount - 1, 18].VerticalAlign = VerticalAlign.Middle;

                            }
                            divMainContents.Visible = true;
                            FpStudentsRankList.Sheets[0].PageSize = FpStudentsRankList.Sheets[0].RowCount;
                            FpStudentsRankList.Width = 980;
                            FpStudentsRankList.SaveChanges();
                            FpStudentsRankList.Visible = true;
                        }
                        else
                        {
                            lblAlertMsg.Text = "No Record(s) Were Found!!!";
                            divPopAlert.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record(s) Were Found!!! Please Check The Mark Entry";
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
                if (FpStudentsRankList.Visible == true)
                {
                    da.printexcelreport(FpStudentsRankList, reportname);
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
            rptheadname = "Over All Rank List Report";
            string pagename = "SubjectsPartWiseRankList.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpStudentsRankList.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpStudentsRankList, pagename, rptheadname);
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

    private int getCblSelectedCount(CheckBoxList cblSelected)
    {
        try
        {
            int sel = 0;
            foreach (ListItem li in cblSelected.Items)
            {
                if (li.Selected)
                {
                    sel++;
                }
            }
            return sel;
        }
        catch
        {
            return 0;
        }
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

    #region Column Order

    #region Added By Malang Raja on Oct 20 2016

    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkColumnOrderAll.Checked == true)
            {
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    string si = Convert.ToString(i).Trim();
                    cblColumnOrder.Items[i].Selected = true;
                    lbtnRemoveAll.Visible = true;
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Add(si);
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
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    cblColumnOrder.Items[i].Selected = false;
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
            string value = "";
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
            txtOrder.Text = "";
            string colname12 = "";
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

    #endregion

}