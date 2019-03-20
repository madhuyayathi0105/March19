using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Web.UI;
using System.Configuration;

public partial class OptionalSubjectsPage : System.Web.UI.Page
{

    #region Variable And Object Declaration

    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;

    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string degreeCode = string.Empty;
    string courseId = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string subjectNo = string.Empty;
    string subjectTypeNo = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qryCourseId = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qrySubjectTypeNo = string.Empty;
    string qrySubjectNo = string.Empty;

    DAccess2 d2 = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataSet ds = new DataSet();

    string sptype = "Text";
    string batchyear = string.Empty;
    string degreecode = string.Empty;
    string sec = string.Empty;
    string sub_no = string.Empty;

    #endregion Variable And Object Declaration

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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if (!IsPostBack)
            {
                divGradeSetting.Visible = false;
                divMainContent.Visible = false;
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lblerrmsg.Visible = false;
                BindCollege();
                bindBatch();
                bindDegree();
                bindBranch();
                bindSemester();
                bindSection();
                BindSubjectType();
                BindSubject();
                GridHeader();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void GridHeader(byte type = 0, DataTable dtGradeSettings = null)
    {
        FpStudent.Sheets[0].AutoPostBack = false;
        FpStudent.CommandBar.Visible = false;
        FpStudent.Sheets[0].SheetCorner.ColumnCount = 0;
        FpStudent.Sheets[0].ColumnCount = 0;
        FpStudent.Sheets[0].RowCount = 0;
        FpStudent.Sheets[0].ColumnCount = (type == 0) ? 9 : (dtGradeSettings.Rows.Count > 0) ? 5 : 9;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
        FpStudent.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
        if (type == 0)
        {
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Good";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Excellent";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Outstanding";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Average";

            FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 1;
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 2;
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 3;
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 4;

            FpStudent.Sheets[0].Columns[5].Width = 60;
            FpStudent.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpStudent.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

            FpStudent.Sheets[0].Columns[6].Width = 100;
            FpStudent.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpStudent.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

            FpStudent.Sheets[0].Columns[7].Width = 150;
            FpStudent.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpStudent.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;

            FpStudent.Sheets[0].Columns[8].Width = 100;
            FpStudent.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpStudent.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;

        }
        else if (dtGradeSettings.Rows.Count > 0)
        {
            foreach (DataRow drGrade in dtGradeSettings.Rows)
            {
                string MarkType = Convert.ToString(drGrade["MarkType"]).Trim();
                string grade = Convert.ToString(drGrade["grade"]).Trim();
                string description = Convert.ToString(drGrade["description"]).Trim();
                string IsShow = Convert.ToString(drGrade["IsShow"]).Trim();

                FpStudent.Sheets[0].ColumnCount++;
                FpStudent.Sheets[0].ColumnHeader.Cells[0, FpStudent.Sheets[0].ColumnCount - 1].Text = grade;
                FpStudent.Sheets[0].ColumnHeader.Cells[0, FpStudent.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpStudent.Sheets[0].ColumnHeader.Cells[0, FpStudent.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpStudent.Sheets[0].ColumnHeader.Cells[0, FpStudent.Sheets[0].ColumnCount - 1].Tag = MarkType;
                FpStudent.Sheets[0].Columns[FpStudent.Sheets[0].ColumnCount - 1].Width = 100;
                FpStudent.Sheets[0].Columns[FpStudent.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpStudent.Sheets[0].Columns[FpStudent.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            }
        }
        //else
        //{
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Good";
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Excellent";
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Outstanding";
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Average";

        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 1;
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 2;
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 3;
        //    FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 4;

        //    FpStudent.Sheets[0].Columns[5].Width = 60;
        //    FpStudent.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        //    FpStudent.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

        //    FpStudent.Sheets[0].Columns[6].Width = 100;
        //    FpStudent.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        //    FpStudent.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

        //    FpStudent.Sheets[0].Columns[7].Width = 150;
        //    FpStudent.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
        //    FpStudent.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;

        //    FpStudent.Sheets[0].Columns[8].Width = 100;
        //    FpStudent.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
        //    FpStudent.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;

        //}


        //FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].CellType = chkall;
        //FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;

        //FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkall;

        //FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 7].CellType = chkall;

        //FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 8].CellType = chkall;
        FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
        chkall.AutoPostBack = true;
        FpStudent.Sheets[0].RowCount = 1;
        for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
        {
            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].CellType = chkall;
        }
        FpStudent.Sheets[0].Columns[0].Width = 37;
        FpStudent.Sheets[0].Columns[1].Width = 100;
        FpStudent.Sheets[0].Columns[2].Width = 100;
        FpStudent.Sheets[0].Columns[3].Width = 280;
        FpStudent.Sheets[0].Columns[4].Width = 200;

        FpStudent.Sheets[0].Columns[0].Locked = true;
        FpStudent.Sheets[0].Columns[1].Locked = true;
        FpStudent.Sheets[0].Columns[2].Locked = true;
        FpStudent.Sheets[0].Columns[3].Locked = true;

        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
        style2.Font.Size = 13;
        style2.Font.Name = "Book Antiqua";
        style2.Font.Bold = true;
        style2.HorizontalAlign = HorizontalAlign.Center;
        style2.ForeColor = System.Drawing.Color.Black;
        // style2.BackColor = System.Drawing.Color.Teal;
        style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

        FpStudent.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

        FpStudent.Sheets[0].SheetName = "Settings";
        FpStudent.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        FpStudent.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
        FpStudent.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        FpStudent.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpStudent.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpStudent.Sheets[0].DefaultStyle.Font.Bold = false;
    }

    #endregion Page Load

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindBatch();
        bindDegree();
        bindBranch();
        bindSemester();
        bindSection();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindDegree();
        bindBranch();
        bindSemester();
        bindSection();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindBranch();
        bindSemester();
        bindSection();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindSemester();
        bindSection();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindSection();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
    }

    protected void ddlSubjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false;
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
        BindSubject();
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg.Visible = false; 
        divMainContent.Visible = false;
        FpStudent.Visible = false;
        btnSave.Visible = false;
    }

    #endregion DropDown Events

    #region Bind Header

    public void BindCollege()
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
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
            d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void bindBatch()
    {
        try
        {
            ds.Clear();
            ddlBatch.Items.Clear();
            collegecode = (ddlCollege.Items.Count > 0) ? ddlCollege.SelectedValue : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13");
            ds = dirAcc.selectDataSet("select distinct batch_year from registration where batch_year<>'-1' and batch_year<>'' and college_code='" + collegecode + "' order by batch_year desc");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                collegecode = Convert.ToString(Session["collegecode"]).Trim();
            }
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            //collegecode = Convert.ToString(Session["collegecode"]); ;
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                collegecode = Convert.ToString(Session["collegecode"]).Trim();
            }
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.SelectedIndex = 0;
            }
        }
        catch
        {

        }
    }

    public void bindSemester()
    {
        try
        {
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                collegecode = Convert.ToString(Session["collegecode"]).Trim();
            }
            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Convert.ToString(collegecode) + "";
            //DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Convert.ToString(collegecode) + "";

                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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
        catch
        {

        }
    }

    public void bindSection()
    {
        try
        {
            ddlSec.Enabled = false;
            ddlSec.Items.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                collegecode = Convert.ToString(Session["collegecode"]).Trim();
            }
            hat.Clear();
            ds.Clear();
            ds = d2.BindSectionDetail(ddlBatch.SelectedValue, ddlBranch.SelectedValue);
            ds = dirAcc.selectDataSet("select distinct LTRIM(RTRIM(ISNULL(sections,''))) as sections from registration where batch_year in(" + Convert.ToString(ddlBatch.SelectedValue).Trim() + ") and degree_code in(" + Convert.ToString(ddlBranch.SelectedValue).Trim() + ") and college_code='" + collegecode + "' and LTRIM(RTRIM(ISNULL(sections,'')))<>'-1' and LTRIM(RTRIM(ISNULL(sections,'')))<>'' and LTRIM(RTRIM(ISNULL(sections,''))) is not null and delflag=0 and exam_flag<>'Debar'");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
                ddlSec.Items.Insert(0, "All");
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch
        {

        }
    }

    public void BindSubjectType()
    {
        try
        {
            ddlSubjectType.Items.Clear();
            ddlSubjectType.Enabled = false;
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                qry = "select distinct ss.subject_type,ss.subType_no,ISNULL(ss.lab,'0') as Lab,ss.priority from sub_sem ss,syllabus_master sm where ss.syll_code=sm.syll_code and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' order by Lab,ss.priority";
                ds = dirAcc.selectDataSet(qry);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSubjectType.DataSource = ds;
                ddlSubjectType.DataTextField = "subject_type";
                ddlSubjectType.DataValueField = "subType_no";
                ddlSubjectType.DataBind();
                ddlSubjectType.Enabled = true;
            }
        }
        catch
        {
        }
    }

    public void BindSubject()
    {
        try
        {
            ds.Clear();
            ddlSubject.Items.Clear();
            ddlSubject.Enabled = false;
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
            }
            subjectTypeNo = string.Empty;
            if (ddlSubjectType.Items.Count > 0)
            {
                subjectTypeNo = Convert.ToString(ddlSubjectType.SelectedValue).Trim();
            }
            //ds = d2.select_method_wo_parameter("select distinct subject_no,Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from Subject where Part_Type='5'", "Text");
            //select distinct s.subject_no,s.subject_code,s.subject_name,Convert(Varchar(max),s.subject_code+' - '+s.subject_name) as Subject_Name from Subject s,sub_sem ss,syllabus_master sm where ss.syll_code=sm.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='2015' and sm.degree_code='45' and sm.semester='1' and ss.subType_no='' order by s.subject_no,s.subject_code,s.subject_name
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                qry = "select distinct s.subject_no,s.subject_code,s.subject_name,Convert(Varchar(max),s.subject_code+' - '+s.subject_name) as Subject_Name from Subject s,sub_sem ss,syllabus_master sm where ss.syll_code=sm.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and ss.subType_no='" + subjectTypeNo + "' order by s.subject_no,s.subject_code,s.subject_name";
                ds = dirAcc.selectDataSet(qry);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSubject.DataSource = ds;
                ddlSubject.DataTextField = "Subject_Name";
                ddlSubject.DataValueField = "subject_no";
                ddlSubject.DataBind();
                ddlSubject.SelectedIndex = ddlSubject.Items.Count - 1;
                ddlSubject.Enabled = true;
                ddlSubject.Items.Insert(0," ");
                ddlSubject.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion Bind Header

    #region Button Event

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Visible = false;
            divMainContent.Visible = false;
            btnDelete.Visible = false;
            int max_sem1 = 0;
            string max_sem = string.Empty;
            int cc = 0;

            FpStudent.Visible = false;
            btnSave.Visible = false;
            FpStudent.SaveChanges();
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "").Trim();

            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY batch_year,degree_code,serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Stud_Name";
                }
            }
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSem.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (section.ToLower().Trim() == "all" || string.IsNullOrEmpty(section.ToLower().Trim()) || section.ToLower().Trim() == "-1" || section.ToLower().Trim() == "0")
                {
                    section = string.Empty;
                }
            }
            subjectTypeNo = string.Empty;
            if (ddlSubjectType.Items.Count > 0)
            {
                subjectTypeNo = Convert.ToString(ddlSubjectType.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjectType.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            //if (ddlBatch.Items.Count > 0)
            //{
            //    batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            //}
            //if (ddlBranch.Items.Count > 0)
            //{
            //    degreecode = Convert.ToString(ddlBranch.SelectedValue);
            //}
            //if (ddlSem.Items.Count > 0)
            //{
            //    semester = Convert.ToString(ddlSem.SelectedItem);
            //}
            //if (ddlSec.Enabled)
            //{
            //    if (ddlSec.Items.Count > 0)
            //    {
            //        sec = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
            //        if (sec.ToLower().Trim() == "all" || string.IsNullOrEmpty(sec.ToLower().Trim()) || sec.ToLower().Trim() == "-1" || sec.ToLower().Trim() == "0")
            //        {
            //            sec = string.Empty;
            //        }
            //    }
            //}
            //else
            //{
            //    sec = string.Empty;
            //}
            //int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration where Batch_Year='" + batchyear + "' and degree_code in('" + degreecode + "')  order by sem"));//and cc=0 and DelFlag=0 and Exam_Flag<>'debar'
            //semester = Convert.ToString(stusemester);
            if (ddlSubject.Items.Count == 0)
            {
                lblAlertMsg.Text = "No Subject(s) Found ";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                sub_no = Convert.ToString(ddlSubject.SelectedValue);
            }
            DataTable dtGradeSettings = new DataTable();
            dtGradeSettings = dirAcc.selectDataTable("select MarkType,grade,description,IsShow from SpecialCourseGradeDetail order by MarkType");
            if (dtGradeSettings.Rows.Count == 0)
            {
                lblAlertMsg.Text = "Please Give Grade Settings And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchYear + "'  and Degree_code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'");
                if (max_sem == "" || max_sem == null)
                {
                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'");
                }
                int.TryParse(max_sem, out max_sem1);
                if (cbpassedout.Checked)
                {
                    //semester = Convert.ToString((max_sem1 + 1));
                    cc = 1;
                }
                ds.Clear();
                if (!string.IsNullOrEmpty(section) && section.Trim() != "0")
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreeCode + "' and Batch_Year='" + batchYear + "'  and Sections='" + section + "' and college_code='" + Convert.ToString(collegeCode).Trim() + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select s.subject_code,s.subject_name,Convert(Varchar(max),subject_code+' - '+subject_name) as SubjectName,s.subject_no,spl.*,sg.MarkType as MT,sg.grade,sg.description,sg.IsShow from subject s,sub_sem ss,syllabus_master sm,SpecialCourseSubject spl left join SpecialCourseGradeDetail sg on sg.MarkType=spl.MarkType where s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.subject_no=spl.Subject_No and CurrentSem='" + semester + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "'";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                else
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreeCode + "' and Batch_Year='" + batchYear + "'  and college_code='" + Convert.ToString(collegeCode).Trim() + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select s.subject_code,s.subject_name,Convert(Varchar(max),subject_code+' - '+subject_name) as SubjectName,s.subject_no,spl.*,sg.MarkType as MT,sg.grade,sg.description,sg.IsShow from subject s,sub_sem ss,syllabus_master sm,SpecialCourseSubject spl left join SpecialCourseGradeDetail sg on sg.MarkType=spl.MarkType where s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.subject_no=spl.Subject_No and CurrentSem='" + semester + "' --and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "'  ";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkeach = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.ComboBoxCellType comboeach = new FarPoint.Web.Spread.ComboBoxCellType();
                    comboeach.AutoPostBack = true;
                    string qrysub = "select distinct s.subject_no,s.subject_code,s.subject_name,Convert(Varchar(max),s.subject_code+' - '+s.subject_name) as Subject_Name from Subject s,sub_sem ss,syllabus_master sm where ss.syll_code=sm.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.syll_code=sm.syll_code and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and ss.subType_no='" + subjectTypeNo + "' order by s.subject_no,s.subject_code,s.subject_name";
                    DataSet dssub = new DataSet();
                    dssub = d2.select_method_wo_parameter(qrysub, "Text");
                    if (dssub.Tables.Count > 0 && dssub.Tables[0].Rows.Count > 0)
                    {
                        comboeach.DataSource = dssub;
                        comboeach.DataTextField = "Subject_Name";
                        comboeach.DataValueField = "subject_no";
                        //comboeach.UseValue = true;
                    }
                    else
                    {
                        FpStudent.Visible = false;
                        btnSave.Visible = false;
                        lblAlertMsg.Text = "There is No Subjects were Found.Please Allocate Subject";
                        divPopAlert.Visible = true;
                        return;
                    }

                    chkall.AutoPostBack = true;
                    chkeach.AutoPostBack = true;
                    comboeach.AutoPostBack = true;
                    //FpStudent.Sheets[0].AutoPostBack = true;
                    if (dtGradeSettings.Rows.Count == 0)
                    {
                        GridHeader(0);
                    }
                    else
                    {
                        GridHeader(1, dtGradeSettings);
                    }
                    for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                    {
                        string appNo = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]).Trim();
                        string regNo = Convert.ToString(ds.Tables[0].Rows[stu]["Reg_No"]).Trim();
                        string rollNo = Convert.ToString(ds.Tables[0].Rows[stu]["Roll_No"]).Trim();
                        string studentName = Convert.ToString(ds.Tables[0].Rows[stu]["Stud_Name"]).Trim();

                        DataView dv = new DataView();

                        FpStudent.Sheets[0].RowCount++;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stu + 1);
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Text = regNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Tag = appNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Text = rollNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Text = studentName;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].CellType = comboeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Locked = false;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                        {
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].CellType = chkeach;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].Locked = false;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                        }

                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + appNo + "'";
                            dv = ds.Tables[1].DefaultView;
                        }
                        if (dv.Count > 0)
                        {
                            string studentMarkValue = Convert.ToString(dv[0]["MarkType"]).Trim();
                            string subjectNos = Convert.ToString(dv[0]["subject_no"]).Trim();
                            string subjectName = Convert.ToString(dv[0]["SubjectName"]).Trim();
                            string grade = Convert.ToString(dv[0]["grade"]).Trim();
                            string description = Convert.ToString(dv[0]["description"]).Trim();
                            if (!string.IsNullOrEmpty(studentMarkValue))
                            {
                                for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                                {
                                    string markTypeVal = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                                    if (markTypeVal == studentMarkValue)
                                    {
                                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].Value = 1;
                                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                        break;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(subjectName) && !string.IsNullOrEmpty(subjectNos))
                            {
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = subjectName;
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = subjectNos;
                            }
                            else
                            {
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = string.Empty;
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = string.Empty;
                            }
                        }
                        //else
                        //{
                        //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;
                        //}
                    }
                    divMainContent.Visible = true;
                    FpStudent.Visible = true;
                    btnSave.Visible = true;
                    btnDelete.Visible = false;
                    FpStudent.Sheets[0].PageSize = FpStudent.Sheets[0].RowCount;
                    FpStudent.Height = (FpStudent.Sheets[0].RowCount * 23) + 24;
                    FpStudent.SaveChanges();
                }
                else
                {
                    divMainContent.Visible = false;
                    FpStudent.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    lblAlertMsg.Text = "No Records Found ";
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Visible = false;
            divConfirmBox.Visible = true;
            lblConfirmMsg.Text = "Do You Want To Delete SubjectChooser?";
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void FpStudent_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            int activeRow = FpStudent.Sheets[0].ActiveRow;
            int activeColumn = FpStudent.Sheets[0].ActiveColumn;
            if (activeRow == 0 && activeColumn == 6 && FpStudent.Sheets[0].ColumnHeader.Cells[0, activeColumn].Text.Trim().ToLower() == "select")
            {
                for (int i = 0; i < FpStudent.Sheets[0].RowCount; i++)
                {
                    if (Convert.ToInt32(FpStudent.Sheets[0].Cells[activeRow, activeColumn].Value) == 1)
                    {
                        FpStudent.Sheets[0].Cells[i, 6].Value = 1;
                    }
                    else
                    {
                        FpStudent.Sheets[0].Cells[i, 6].Value = 0;
                    }
                }

                //if (Convert.ToInt32(FpStudent.Sheets[0].Cells[0, 6].Value) == 1)
                //{
                //    for (int i = 0; i < FpStudent.Sheets[0].RowCount; i++)
                //    {
                //        FpStudent.Sheets[0].Cells[i, 6].Value = 1;
                //    }

                //}
                //else if (Convert.ToInt32(FpStudent.Sheets[0].Cells[0, 6].Value) == 0)
                //{
                //    for (int i = 0; i < FpStudent.Sheets[0].RowCount; i++)
                //    {
                //        FpStudent.Sheets[0].Cells[i, 6].Value = 0;
                //    }

                //}
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void FpStudent_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
           // FpStudent.SaveChanges();
            int r = FpStudent.Sheets[0].ActiveRow;
            int j = FpStudent.Sheets[0].ActiveColumn;
            int k = Convert.ToInt32(j);

            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);
            if (b != 4)
            {
                if (r >= 0 && FpStudent.Sheets[0].ColumnHeader.Cells[0, j].Text.Trim().ToLower() != "select")
                {
                    if (Convert.ToInt32(r) == 0)
                    {
                        if (r.ToString().Trim() != "" && j.ToString().Trim() != "")
                        {
                            if (FpStudent.Sheets[0].RowCount > 0)
                            {
                                int checkval = Convert.ToInt32(FpStudent.Sheets[0].Cells[a, b].Value);
                                if (checkval == 0)
                                {
                                    string headervalue = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, b].Tag);
                                    for (int i = 1; i < FpStudent.Sheets[0].RowCount; i++)
                                    {
                                        for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                                        {
                                            if (col != b)
                                            {
                                                FpStudent.Sheets[0].Cells[i, col].Value = 0;
                                                FpStudent.Sheets[0].Cells[0, col].Value = 0;
                                            }
                                            else
                                            {
                                                FpStudent.Sheets[0].Cells[i, col].Value = 1;
                                                FpStudent.Sheets[0].Cells[0, col].Value = 1;
                                            }
                                        }
                                        //int checkvalue = Convert.ToInt32(FpStudent.Sheets[0].Cells[i, b].Value);
                                        //int checkvalue1 = Convert.ToInt32(FpStudent.Sheets[0].Cells[i, b].Value);
                                        //if (headervalue.Trim() == "1")
                                        //{
                                        //    FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        //    FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b + 3].Value = 0;

                                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                                        //}
                                        //if (headervalue.Trim() == "2")
                                        //{
                                        //    FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        //    FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;

                                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                        //}
                                        //if (headervalue.Trim() == "3")
                                        //{
                                        //    FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        //    FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;

                                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        //}
                                        //if (headervalue.Trim() == "4")
                                        //{
                                        //    FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        //    FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[i, b - 3].Value = 0;

                                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        //    FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                                        //}

                                    }
                                }
                                else if (checkval == 1)
                                {
                                    for (int i = 1; i < FpStudent.Sheets[0].RowCount; i++)
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string headervalue = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(j)].Tag);

                        for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                        {
                            if (col != j)
                            {
                                FpStudent.Sheets[0].Cells[a, col].Value = 0;
                            }
                        }
                        //if (headervalue.Trim() == "1")
                        //{
                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                        //}
                        //if (headervalue.Trim() == "2")
                        //{
                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        //}
                        //if (headervalue.Trim() == "3")
                        //{
                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        //}
                        //if (headervalue.Trim() == "4")
                        //{
                        //    FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        //    FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        //}
                    }
                }
            }
        }
        catch
        {

        }
    }
   
    protected void btnview_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = string.Empty;
            int cc = 0;
            FpStudent.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            FpStudent.SaveChanges();
            columnBind();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkeach = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            chkeach.AutoPostBack = false;
            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY batch_year,degree_code,serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Stud_Name";
                }
            }

            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSem.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (section.ToLower().Trim() == "all" || string.IsNullOrEmpty(section.ToLower().Trim()) || section.ToLower().Trim() == "-1" || section.ToLower().Trim() == "0")
                {
                    section = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchYear + "'  and Degree_code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'").Trim();
                if (string.IsNullOrEmpty(max_sem) || max_sem.Trim() == "0")
                {
                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'").Trim();
                }
                int.TryParse(max_sem, out max_sem1);
                if (cbpassedout.Checked)
                {
                    //semester = Convert.ToString((max_sem1 + 1));
                    cc = 1;
                }
                ds.Clear();
                if (!string.IsNullOrEmpty(section) && section.Trim() == "0")
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreeCode + "' and Batch_Year='" + batchYear + "'  and Sections='" + section + "' and college_code='" + Convert.ToString(collegeCode) + "' and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ;  select s.subject_code,s.subject_name,Convert(Varchar(max),subject_code+' - '+subject_name) as SubjectName,s.subject_no,spl.*,sg.MarkType as MT,sg.grade,sg.description,sg.IsShow from subject s,sub_sem ss,syllabus_master sm,SpecialCourseSubject spl left join SpecialCourseGradeDetail sg on sg.MarkType=spl.MarkType where s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.subject_no=spl.Subject_No and CurrentSem='" + semester + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' ";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //Subject_No='" + sub_no + "' and and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                else
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreeCode + "' and Batch_Year='" + batchYear + "'  and college_code='" + Convert.ToString(collegeCode) + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select s.subject_code,s.subject_name,Convert(Varchar(max),subject_code+' - '+subject_name) as SubjectName,s.subject_no,spl.*,sg.MarkType as MT,sg.grade,sg.description,sg.IsShow from subject s,sub_sem ss,syllabus_master sm,SpecialCourseSubject spl left join SpecialCourseGradeDetail sg on sg.MarkType=spl.MarkType where s.syll_code=sm.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.subject_no=spl.Subject_No and CurrentSem='" + semester + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' ";
                    //Subject_No='" + sub_no + "' and and Current_Semester='" + semester + "' and CC='" + cc + "'
                    ds = d2.select_method_wo_parameter(qry, sptype);
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    FpStudent.Sheets[0].RowCount = 0;
                    FpStudent.Sheets[0].RowCount++;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkall;
                    for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                    {
                        //string app_no = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]);
                        string appNo = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]).Trim();
                        string regNo = Convert.ToString(ds.Tables[0].Rows[stu]["Reg_No"]).Trim();
                        string rollNo = Convert.ToString(ds.Tables[0].Rows[stu]["Roll_No"]).Trim();
                        string studentName = Convert.ToString(ds.Tables[0].Rows[stu]["Stud_Name"]).Trim();
                        DataView dv = new DataView();
                        FpStudent.Sheets[0].RowCount++;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stu + 1).Trim();
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Text = regNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Tag = appNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Text = rollNo;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Text = studentName;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].Locked = false;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + appNo + "'";
                            dv = ds.Tables[1].DefaultView;
                        }
                        if (dv.Count > 0)
                        {
                            string studentMarkValue = Convert.ToString(dv[0]["MarkType"]).Trim();
                            string subjectNos = Convert.ToString(dv[0]["subject_no"]).Trim();
                            string subjectName = Convert.ToString(dv[0]["SubjectName"]).Trim();
                            string grade = Convert.ToString(dv[0]["grade"]).Trim();
                            string description = Convert.ToString(dv[0]["description"]).Trim();

                            if (!string.IsNullOrEmpty(studentMarkValue))
                            {
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = grade;
                            }
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = subjectName;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Tag = subjectNos;
                            //if (Convert.ToString(dv[0]["MarkType"]) == "1")
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Good";
                            //}
                            //else if (Convert.ToString(dv[0]["MarkType"]) == "2")
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Excellent";
                            //}
                            //else if (Convert.ToString(dv[0]["MarkType"]) == "3")
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Outstanding";
                            //}
                            //else if (Convert.ToString(dv[0]["MarkType"]) == "4")
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Average";
                            //}
                            //string sub_no1 = Convert.ToString(dv[0]["Subject_No"]);
                            //string sub_name = d2.GetFunctionv("select Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from subject where subject_no='" + sub_no1 + "' and Part_Type='5'");
                            //if (sub_name != "" && sub_no1 != "" && sub_name != null && sub_no1 != null)
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = sub_name;
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Tag = sub_no1;
                            //}
                            //else
                            //{
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = string.Empty;
                            //    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = string.Empty;
                            //}
                        }
                        else
                        {
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = string.Empty;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = string.Empty;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Tag = string.Empty;
                        }
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    }
                    divMainContent.Visible = true;
                    FpStudent.Visible = true;
                    // btnSave.Visible = true;
                    btnSave.Visible = false;
                    btnDelete.Visible = true;
                    FpStudent.Sheets[0].PageSize = FpStudent.Sheets[0].RowCount;
                    FpStudent.Height = (FpStudent.Sheets[0].RowCount * 23) + 24;
                    FpStudent.SaveChanges();
                }
                else
                {
                    divMainContent.Visible = false;
                    FpStudent.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    lblAlertMsg.Text = "No Records Found ";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }

    public void columnBind()
    {
        try
        {
            FpStudent.Sheets[0].AutoPostBack = false;
            FpStudent.CommandBar.Visible = false;
            FpStudent.Sheets[0].SheetCorner.ColumnCount = 0;
            FpStudent.Sheets[0].ColumnCount = 0;
            FpStudent.Sheets[0].RowCount = 0;
            FpStudent.Sheets[0].ColumnCount = 7;
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpStudent.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mark Type";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
            FpStudent.Sheets[0].Columns[0].Width = 37;
            FpStudent.Sheets[0].Columns[1].Width = 100;
            FpStudent.Sheets[0].Columns[2].Width = 100;
            FpStudent.Sheets[0].Columns[3].Width = 310;
            FpStudent.Sheets[0].Columns[4].Width = 200;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            // style2.BackColor = System.Drawing.Color.Teal;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

            FpStudent.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpStudent.Sheets[0].SheetName = "Settings";
            FpStudent.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpStudent.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpStudent.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpStudent.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpStudent.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpStudent.Sheets[0].DefaultStyle.Font.Bold = false;

        }
        catch
        {

        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = string.Empty;
            string app_no = string.Empty;
            string mark_type = string.Empty;
            bool result = false;
            bool isfinal = false;
            string subject_no = string.Empty;
            qry = string.Empty;
            int cc = 0;
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            string curr_sem = Convert.ToString(ddlSem.SelectedItem);
            max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchyear + "'  and Degree_code='" + degreecode + "'").Trim();
            if (string.IsNullOrEmpty(max_sem) || max_sem == "0")
            {
                max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreecode + "'");
            }
            int.TryParse(max_sem, out max_sem1);
            if (cbpassedout.Checked)
            {
                semester = Convert.ToString((max_sem1 + 1));
                cc = 1;
                isfinal = true;
                curr_sem = semester;
            }
            FpStudent.SaveChanges();
            if (FpStudent.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpStudent.Sheets[0].RowCount; row++)
                {
                    int val = Convert.ToInt32(FpStudent.Sheets[0].Cells[row, 6].Value);
                    if (val == 1)
                    {
                        app_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 1].Tag).Trim();
                        subject_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 4].Tag).Trim();
                        if (!string.IsNullOrEmpty(app_no) && !string.IsNullOrEmpty(subject_no) && subject_no != "0")
                        {
                            qry = "if exists (select * from SpecialCourseSubject where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "' and subject_no='" + subject_no + "') delete SpecialCourseSubject where subject_no='" + subject_no + "' and App_no='" + app_no + "' and CurrentSem='" + curr_sem + "'";
                            int res = d2.update_method_wo_parameter(qry, "Text");
                            if (res == 1)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                divMainContent.Visible = false;
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lblAlertMsg.Text = "No Records Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (result == true)
            {
                btnview_Click(sender, e);
                lblAlertMsg.Text = "Deleted Successfully ";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Deleted";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {

        }
    }

    #region Confirmation Yes/No Click

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Visible = false;
            divConfirmBox.Visible = false;
            Button btnSender = (Button)sender;
            bool isDeleteSubjectChooser = false;
            if (btnSender.Text.Trim().ToLower() == "yes")
            {
                isDeleteSubjectChooser = true;
            }
            else if (btnSender.Text.Trim().ToLower() == "no")
            {
                isDeleteSubjectChooser = false;
            }

            int max_sem1 = 0;
            string max_sem = string.Empty;
            int cc = 0;
            FpStudent.SaveChanges();
            string subject_no = Convert.ToString(ddlSubject.SelectedValue);
            string findfinalSem = Convert.ToString(ddlSem.Items.Count);
            string curr_sem = Convert.ToString(ddlSem.SelectedItem);
            bool isfinal = false;
            string app_no = string.Empty;
            string mark_type = string.Empty;
            bool result = false;
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count > 0)
            {
                curr_sem = semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSem.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (section.ToLower().Trim() == "all" || string.IsNullOrEmpty(section.ToLower().Trim()) || section.ToLower().Trim() == "-1" || section.ToLower().Trim() == "0")
                {
                    section = string.Empty;
                }
            }
            subjectTypeNo = string.Empty;
            if (ddlSubjectType.Items.Count > 0)
            {
                subjectTypeNo = Convert.ToString(ddlSubjectType.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjectType.Text + " Found ";
                divPopAlert.Visible = true;
                return;
            }

            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchYear + "'  and Degree_code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'");
            if (max_sem == "" || max_sem == null)
            {
                max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreeCode + "' and college_code='" + Convert.ToString(collegeCode) + "'");
            }
            max_sem = ddlSem.SelectedItem.Text;
            int.TryParse(max_sem, out max_sem1);
            if (cbpassedout.Checked)
            {
                semester = Convert.ToString((max_sem1 + 1));
                cc = 1;
                isfinal = true;
                curr_sem = semester;
            }
            else
            {
                isfinal = false;
            }
            qry = string.Empty;
            FpStudent.SaveChanges();
            string rollNos = string.Empty;
            if (FpStudent.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpStudent.Sheets[0].RowCount; row++)
                {
                    string rollNo = Convert.ToString(FpStudent.Sheets[0].Cells[row, 2].Text).Trim();
                    app_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 1].Tag);
                    for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                    {
                        int val = 0;//Convert.ToInt32(FpStudent.Sheets[0].Cells[row, col].Value);
                        int.TryParse(Convert.ToString(FpStudent.Sheets[0].Cells[row, col].Value).Trim(), out val);
                        if (val == 1)
                        {
                            mark_type = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                            break;
                        }
                    }
                    subject_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 4].Value).Trim();
                    //subject_no = Convert.ToString(ddlSubject.SelectedValue);
                    if (!string.IsNullOrEmpty(app_no) && !string.IsNullOrEmpty(subject_no) && !string.IsNullOrEmpty(curr_sem) && !string.IsNullOrEmpty(mark_type) && app_no != "0" && subject_no != "0" && mark_type != "0" && curr_sem != "0")
                    {
                        //Subject_No='" + subject_no + "' and Subject_No='" + subject_no + "' and
                        //qry = "if exists (select * from SpecialCourseSubject where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "')update SpecialCourseSubject set MarkType='" + mark_type + "',subject_no='" + subject_no + "' where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "'  else  insert into SpecialCourseSubject (Subject_No,App_no,MarkType,IsFinalsem,CurrentSem) values ('" + subject_no + "','" + app_no + "','" + mark_type + "','" + isfinal + "','" + curr_sem + "')";
                        //int res = d2.update_method_wo_parameter(qry, "Text");
                        dicQueryParameter.Clear();
                        dicQueryParameter.Add("rollNo", rollNo);
                        dicQueryParameter.Add("appNo", app_no);
                        dicQueryParameter.Add("subjectNo", subject_no);
                        dicQueryParameter.Add("markType", mark_type);
                        dicQueryParameter.Add("isFinalSem", (isfinal) ? "1" : "0");
                        dicQueryParameter.Add("currentSem", curr_sem);
                        dicQueryParameter.Add("isDeleteSubjectChooser", (isDeleteSubjectChooser) ? "1" : "0");
                        int res = storeAcc.updateData("uspInsertSpecialSubject", dicQueryParameter);
                        if (res != 0)
                        {
                            result = true;
                        }
                    }
                    else if (!string.IsNullOrEmpty(app_no) && !string.IsNullOrEmpty(subject_no) && !string.IsNullOrEmpty(curr_sem) && string.IsNullOrEmpty(mark_type))
                    {
                        lblerrmsg.Visible = true;
                        if(string.IsNullOrEmpty(rollNos))
                            rollNos = rollNo;
                        else
                            rollNos = rollNos+","+rollNo;
                        lblerrmsg.Text = rollNos + "  Are Not Update";
                    }
                }
            }
            else
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lblAlertMsg.Text = "No Records Found ";
                divPopAlert.Visible = true;
                return;
            }
            if (result == true)
            {
                lblAlertMsg.Text = "Saved Successfully ";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Saved";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    #region Grade Setting

    protected void btnSettings_Click(object sender, EventArgs e)
    {
        try
        {
            divGradeSetting.Visible = true;
            divMainContent.Visible = false;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            // style2.BackColor = System.Drawing.Color.Teal;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

            FpGradeSetting.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpGradeSetting.Sheets[0].SheetName = "Settings";
            FpGradeSetting.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpGradeSetting.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpGradeSetting.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpGradeSetting.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpGradeSetting.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpGradeSetting.Sheets[0].DefaultStyle.Font.Bold = false;

            FpGradeSetting.Sheets[0].AutoPostBack = false;
            FpGradeSetting.CommandBar.Visible = false;
            FpGradeSetting.Sheets[0].SheetCorner.ColumnCount = 0;
            FpGradeSetting.Sheets[0].ColumnCount = 0;
            FpGradeSetting.Sheets[0].RowCount = 0;
            FpGradeSetting.Sheets[0].ColumnCount = 3;
            FpGradeSetting.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpGradeSetting.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Grade";
            FpGradeSetting.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
            FpGradeSetting.Sheets[0].Columns[0].Width = 50;
            FpGradeSetting.Sheets[0].Columns[0].Resizable = false;
            FpGradeSetting.Sheets[0].Columns[1].Width = 125;
            FpGradeSetting.Sheets[0].Columns[1].Resizable = false;
            FpGradeSetting.Sheets[0].Columns[2].Width = 210;
            FpGradeSetting.Sheets[0].Columns[2].Resizable = false;

            DataTable dtGradeSettings = new DataTable();
            dtGradeSettings = dirAcc.selectDataTable("select MarkType,grade,description,IsShow from SpecialCourseGradeDetail order by MarkType asc");
            if (dtGradeSettings.Rows.Count > 0)
            {
                foreach (DataRow drGrade in dtGradeSettings.Rows)
                {
                    string MarkType = Convert.ToString(drGrade["MarkType"]).Trim();
                    string grade = Convert.ToString(drGrade["grade"]).Trim();
                    string description = Convert.ToString(drGrade["description"]).Trim();
                    string IsShow = Convert.ToString(drGrade["IsShow"]).Trim();

                    FpGradeSetting.Sheets[0].RowCount++;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpGradeSetting.Sheets[0].RowCount).Trim();
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].Locked = true;

                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(grade).Trim();
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(MarkType).Trim();
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].Locked = false;

                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(description).Trim();
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(IsShow).Trim();
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].Locked = false;
                }
                FpGradeSetting.Sheets[0].PageSize = FpGradeSetting.Sheets[0].RowCount;
                FpGradeSetting.SaveChanges();
            }
        }
        catch
        {
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //int noOfGrade = 0;
            //string totalNoOfGrades = txtTotalGrade.Text.Trim();
            //int.TryParse(totalNoOfGrades, out noOfGrade);
            //if (string.IsNullOrEmpty(txtTotalGrade.Text.Trim()))
            //{
            //    lblAlertMsg.Text = "Please Enter Total No. of Grades";
            //    divPopAlert.Visible = true;
            //    return;
            //}
            FpGradeSetting.SaveChanges();

            FpGradeSetting.Sheets[0].RowCount++;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpGradeSetting.Sheets[0].RowCount).Trim();
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 0].Locked = true;

            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].Text = Convert.ToString("").Trim();
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 1].Locked = false;


            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].Text = Convert.ToString("").Trim();
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            FpGradeSetting.Sheets[0].Cells[FpGradeSetting.Sheets[0].RowCount - 1, 2].Locked = false;

            FpGradeSetting.Sheets[0].PageSize = FpGradeSetting.Sheets[0].RowCount;
            FpGradeSetting.SaveChanges();

        }
        catch
        {
        }
    }

    protected void btnSaveSetting_Click(object sender, EventArgs e)
    {
        try
        {
            //divGradeSetting.Visible = false;
            FpGradeSetting.SaveChanges();
            bool isSaved = false;
            //            if exists(select MarkType from SpecialCourseGradeDetail where grade='') update SpecialCourseGradeDetail set description='' where grade='' else insert into SpecialCourseGradeDetail (grade,description,IsShow) values ('','','1')

            //if exists(select MarkType from SpecialCourseGradeDetail where MarkType='') update SpecialCourseGradeDetail set description='' where MarkType='' else insert into SpecialCourseGradeDetail (grade,description,IsShow) values ('','','1')
            if (FpGradeSetting.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpGradeSetting.Sheets[0].RowCount; row++)
                {
                    string grade = Convert.ToString(FpGradeSetting.Sheets[0].Cells[row, 1].Text).Trim();
                    string markType = Convert.ToString(FpGradeSetting.Sheets[0].Cells[row, 1].Tag).Trim();
                    string description = Convert.ToString(FpGradeSetting.Sheets[0].Cells[row, 2].Text).Trim();
                    string isShow = Convert.ToString(FpGradeSetting.Sheets[0].Cells[row, 2].Tag).Trim();
                    int save = 0;
                    if (!string.IsNullOrEmpty(grade) && !string.IsNullOrEmpty(description))
                    {
                        if (!string.IsNullOrEmpty(markType))
                        {
                            qry = "if exists(select MarkType from SpecialCourseGradeDetail where MarkType='" + markType + "') update SpecialCourseGradeDetail set description='" + description + "',grade='" + grade + "' where MarkType='" + markType + "' else insert into SpecialCourseGradeDetail (grade,description,IsShow) values ('" + grade + "','" + description + "','1')";
                            save = dirAcc.updateData(qry);
                        }
                        else
                        {
                            qry = "if exists(select MarkType from SpecialCourseGradeDetail where grade='" + grade + "') update SpecialCourseGradeDetail set description='" + description + "',grade='" + grade + "' where grade='" + grade + "' else insert into SpecialCourseGradeDetail (grade,description,IsShow) values ('" + grade + "','" + description + "','1')";
                            save = dirAcc.updateData(qry);
                        }
                        if (save != 0)
                            isSaved = true;
                    }
                    else
                    {
                        lblAlertMsg.Text = "Grade and Description Can't Be Empty";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "No Records Found ";
                divPopAlert.Visible = true;
                return;
            }
            btnSettings_Click(sender, e);
            lblAlertMsg.Text = (isSaved) ? "Saved Successfully" : "Not Saved";
            divPopAlert.Visible = true;
            return;
        }
        catch
        {
        }
    }

    protected void btnCloseSetting_Click(object sender, EventArgs e)
    {
        try
        {
            divGradeSetting.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    #endregion

}