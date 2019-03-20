using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class COEInternalMarksUpdate : System.Web.UI.Page
{
    #region Fields Declaration

    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();

    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string collegeCode = string.Empty;

    string qry = string.Empty;
    string collegeName = string.Empty;
    string courseName = string.Empty;
    string departmentName = string.Empty;
    string degreeCode = string.Empty;
    string batchYear = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string examMonth = string.Empty;
    string examYear = string.Empty;
    string examCode = string.Empty;

    string studentName = string.Empty;
    string appNo = string.Empty;
    string rollNo = string.Empty;
    string regNo = string.Empty;
    string admissionNo = string.Empty;
    string qrySearch = string.Empty;
    string text = "Text";
    string storedProcedure = "sp";

    string subjectNo = string.Empty;
    string subjectCode = string.Empty;
    string subjectName = string.Empty;
    string internalMark = string.Empty;
    string externalMark = string.Empty;
    string total = string.Empty;
    string result = string.Empty;
    string minInternal = string.Empty;
    string maxInternal = string.Empty;
    string minExternal = string.Empty;
    string maxExternal = string.Empty;
    string minTotal = string.Empty;
    string maxTotal = string.Empty;

    double internalMarks = 0;
    double externalMarks = 0;
    double totalMarks = 0;
    double minimumInternal = 0;
    double minimumExternal = 0;
    double maximumInternal = 0;
    double maximumExternal = 0;
    double minimumTotal = 0;
    double maximumTotal = 0;

    int monthValue = 0;
    int attempts = 0;

    bool isSchool = false;
    bool passOrFail = false;

    Hashtable hat = new Hashtable();

    #endregion Fields Declaration

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
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            collegeCode = Convert.ToString(Session["collegecode"]).Trim();

            string grouporusercode1 = "";
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]).Trim();
                if (schoolvalue.Trim() == "0")
                {
                    isSchool = true;
                }
            }

            if (!IsPostBack)
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                divMainGrid.Visible = false;
                divPrint.Visible = false;
                txtSearch.Text = string.Empty;
                if (ddlSearchBy.Items.Count > 0)
                {
                    ddlSearchBy.SelectedIndex = 0;
                    lblSearch.Text = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
                }
                BindExamYear();
                BindExamMonth();

                ViewState["Rollflag"] = "0";
                ViewState["Regflag"] = "0";
                ViewState["Studflag"] = "0";
                ViewState["AdmissionNo"] = "0";

                string grouporusercode = "";

                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                string user_code = Convert.ToString(Session["usercode"]).Trim();

                hat.Clear();
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method_wo_parameter(Master, text);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        ViewState["Rollflag"] = "1";

                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        ViewState["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        ViewState["Studflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        ViewState["AdmissionNo"] = "1";
                    }
                }
            }
        }
        catch (ThreadAbortException et)
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Page Load

    #region Bind Header

    public void BindExamYear()
    {
        ds.Clear();
        string group_user = string.Empty;
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " and group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
        }
        Boolean setflag = false;
        ddlExamYear.Items.Clear();
        string getexamvalue = d2.GetFunction("select value from master_settings where settings='Exam year and month Valuation' " + grouporusercode + "");
        if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
        {
            string[] spe = getexamvalue.Split(',');
            if (spe.GetUpperBound(0) == 1)
            {
                if (spe[0].Trim() != "0")
                {
                    ddlExamYear.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(spe[0]), Convert.ToString(spe[0])));
                    setflag = true;
                }
            }
        }
        if (setflag == false)
        {
            ds = d2.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlExamYear.DataSource = ds;
                ddlExamYear.DataTextField = "Exam_year";
                ddlExamYear.DataValueField = "Exam_year";
                ddlExamYear.DataBind();
            }
        }
        ddlExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void BindExamMonth()
    {
        try
        {
            ddlExamMonth.Items.Clear();
            string group_user = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " and group_code='" + group_user + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            Boolean setflag = false;
            string monthval = string.Empty;
            string getexamvalue = d2.GetFunction("select value from master_settings where settings='Exam year and month Valuation' " + grouporusercode + "");
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    if (spe[1].Trim() != "0")
                    {
                        string val = spe[1].ToString();
                        monthval = " and Exam_month='" + val + "'";
                    }
                }
            }
            ds.Clear();
            string year1 = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            ds = d2.select_method_wo_parameter(strsql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlExamMonth.DataSource = ds;
                ddlExamMonth.DataTextField = "monthName";
                ddlExamMonth.DataValueField = "Exam_month";
                ddlExamMonth.DataBind();
                ddlExamMonth.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void Init_Spread(Farpoint.FpSpread FpAttendanceReport)
    {
        try
        {
            #region FpSpread Style

            FpAttendanceReport.Visible = false;
            FpAttendanceReport.Sheets[0].ColumnCount = 0;
            FpAttendanceReport.Sheets[0].RowCount = 0;
            FpAttendanceReport.Sheets[0].SheetCorner.ColumnCount = 0;
            FpAttendanceReport.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpAttendanceReport.Visible = false;
            FpAttendanceReport.CommandBar.Visible = false;
            FpAttendanceReport.RowHeader.Visible = false;
            FpAttendanceReport.Sheets[0].AutoPostBack = false;
            FpAttendanceReport.Sheets[0].RowCount = 0;
            FpAttendanceReport.Sheets[0].ColumnCount = 10;
            FpAttendanceReport.Sheets[0].FrozenRowCount = 0;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            Farpoint.TextCellType txtCellType = new Farpoint.TextCellType();

            FpAttendanceReport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpAttendanceReport.Sheets[0].DefaultStyle = sheetstyle;
            FpAttendanceReport.Sheets[0].ColumnHeader.RowCount = 2;

            FpAttendanceReport.Sheets[0].Columns[0].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[1].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[2].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[3].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[4].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[5].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[6].Locked = false;
            FpAttendanceReport.Sheets[0].Columns[7].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[8].Locked = true;
            FpAttendanceReport.Sheets[0].Columns[9].Locked = true;

            FpAttendanceReport.Sheets[0].Columns[0].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[1].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[2].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[3].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[4].Visible = false;
            FpAttendanceReport.Sheets[0].Columns[5].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[6].Visible = true;
            FpAttendanceReport.Sheets[0].Columns[7].Visible = false;
            FpAttendanceReport.Sheets[0].Columns[8].Visible = false;
            FpAttendanceReport.Sheets[0].Columns[9].Visible = false;

            FpAttendanceReport.Sheets[0].Columns[0].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[1].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[2].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[3].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[4].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[5].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[6].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[7].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[8].Resizable = false;
            FpAttendanceReport.Sheets[0].Columns[9].Resizable = false;

            FpAttendanceReport.Sheets[0].Columns[0].Width = 35;
            FpAttendanceReport.Sheets[0].Columns[1].Width = 100;
            FpAttendanceReport.Sheets[0].Columns[2].Width = 100;
            FpAttendanceReport.Sheets[0].Columns[3].Width = 100;
            FpAttendanceReport.Sheets[0].Columns[4].Width = 90;
            FpAttendanceReport.Sheets[0].Columns[5].Width = 250;
            FpAttendanceReport.Sheets[0].Columns[6].Width = 85;
            FpAttendanceReport.Sheets[0].Columns[7].Width = 80;
            FpAttendanceReport.Sheets[0].Columns[8].Width = 80;
            FpAttendanceReport.Sheets[0].Columns[9].Width = 80;

            FpAttendanceReport.Sheets[0].Columns[1].CellType = txtCellType;
            FpAttendanceReport.Sheets[0].Columns[2].CellType = txtCellType;
            FpAttendanceReport.Sheets[0].Columns[3].CellType = txtCellType;
            FpAttendanceReport.Sheets[0].Columns[4].CellType = txtCellType;

            FpAttendanceReport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            FpAttendanceReport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpAttendanceReport.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;

            FpAttendanceReport.Sheets[0].AutoPostBack = false;
            //FpAttendanceReport.Sheets[0].AutoPostBack = true;
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject No";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Name";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Internal Marks";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "External Marks";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
            FpAttendanceReport.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Result";

            //if (ViewState["AdmissionNo"] != null && Convert.ToString(ViewState["AdmissionNo"]).Trim() == "1")
            //{
            //    FpAttendanceReport.Sheets[0].Columns[1].Visible = true;
            //}
            //else
            //{
            //    FpAttendanceReport.Sheets[0].Columns[1].Visible = false;
            //}
            //if (ViewState["Rollflag"] != null && Convert.ToString(ViewState["Rollflag"]).Trim() == "1")
            //{
            //    FpAttendanceReport.Sheets[0].Columns[2].Visible = true;
            //}
            //else
            //{
            //    FpAttendanceReport.Sheets[0].Columns[2].Visible = false;
            //}

            //if (ViewState["Regflag"] != null && Convert.ToString(ViewState["Regflag"]).Trim() == "1")
            //{
            //    FpAttendanceReport.Sheets[0].Columns[3].Visible = true;
            //}
            //else
            //{
            //    FpAttendanceReport.Sheets[0].Columns[3].Visible = false;
            //}
            //if (ViewState["Studflag"] != null && Convert.ToString(ViewState["Studflag"]).Trim() == "1")
            //{
            //    FpAttendanceReport.Sheets[0].Columns[4].Visible = true;
            //}
            //else
            //{
            //    FpAttendanceReport.Sheets[0].Columns[4].Visible = false;
            //}

            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            FpAttendanceReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Bind Header

    #region DropDownList Events

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divPrint.Visible = false;
            if (ddlSearchBy.Items.Count > 0)
            {
                lblSearch.Text = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divPrint.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divPrint.Visible = false;
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion DropDownList Events

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divPrint.Visible = false;
            string searchBy = string.Empty;
            string search = string.Empty;
            search = Convert.ToString(txtSearch.Text).Trim();
            qrySearch = string.Empty;

            DataSet dsStudentDetails = new DataSet();
            DataSet dsNewInternal = new DataSet();

            int RoundOff = 0;
            string getmarkround = d2.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'").Trim();
            int.TryParse(getmarkround, out RoundOff);


            if (ddlSearchBy.Items.Count > 0)
            {
                searchBy = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(search))
                {
                    if (ddlSearchBy.SelectedIndex == 0)
                    {
                        qrySearch = " and reg_no='" + search + "'";
                        regNo = search;
                    }
                    else if (ddlSearchBy.SelectedIndex == 1)
                    {
                        qrySearch = " and roll_no='" + search + "'";
                        rollNo = search;
                    }
                    else if (ddlSearchBy.SelectedIndex == 2)
                    {
                        qrySearch = " and Roll_Admit='" + search + "'";
                        admissionNo = search;
                    }
                }
            }

            if (string.IsNullOrEmpty(search))
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Enter " + searchBy;
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(qrySearch.Trim()))
                {
                    //select r.college_code,cl.collname,r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Current_Semester,r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit,isnull(ltrim(rtrim(r.Sections)),'') Sections from Registration r,collinfo cl,Course c,Department dt,Degree dg where cl.college_code=r.college_code and c.college_code=cl.college_code and c.college_code=r.college_code and c.college_code=dt.college_code and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=cl.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code
                    qry = "select r.college_code,cl.collname,r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Current_Semester,r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,isnull(ltrim(rtrim(r.Sections)),'') Sections from Registration r,collinfo cl,Course c,Department dt,Degree dg where cl.college_code=r.college_code and c.college_code=cl.college_code and c.college_code=r.college_code and c.college_code=dt.college_code and c.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=cl.college_code and dg.college_code=r.college_code and r.college_code=dt.college_code and r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qrySearch;
                    dsStudentDetails = d2.select_method_wo_parameter(qry, text);
                }
            }
            if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
            {
                collegeCode = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["college_code"]).Trim();
                collegeName = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["collname"]).Trim();
                batchYear = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Batch_Year"]).Trim();
                degreeCode = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["degree_code"]).Trim();
                courseName = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Course_Name"]).Trim();
                departmentName = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Dept_Name"]).Trim();
                semester = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Current_Semester"]).Trim();
                appNo = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["App_No"]).Trim();
                rollNo = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Roll_No"]).Trim();
                regNo = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Reg_No"]).Trim();
                admissionNo = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Roll_Admit"]).Trim();
                section = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Sections"]).Trim();
                studentName = Convert.ToString(dsStudentDetails.Tables[0].Rows[0]["Stud_Name"]).Trim();

                if (ddlExamYear.Items.Count > 0)
                {
                    examYear = Convert.ToString(ddlExamYear.SelectedItem.Text).Trim();
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No Exam Year Were Found!!! Give Exam Year rights to Staff";
                    return;
                }
                if (ddlExamMonth.Items.Count > 0)
                {
                    examMonth = Convert.ToString(ddlExamMonth.SelectedItem.Value).Trim();
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No Exam Month Were Found!!! Give Exam Month rights to Staff";
                    return;
                }

                if (!string.IsNullOrEmpty(examYear.Trim()) && !string.IsNullOrEmpty(examMonth.Trim()))
                {
                    int monthval = (Convert.ToInt32(examYear) * 12) + Convert.ToInt32(examMonth);
                    attempts = 0;
                    dsNewInternal = d2.select_method_wo_parameter("select sc.roll_no,s.subject_no,s.subject_code,isnull(s.subjectpriority,'0') as subjectpriority,total,actual_total,ca.Exam_Year,ca.Exam_Month from camarks ca,subject s,subjectChooser sc,Exam_Details ed,Registration r where r.Roll_No=sc.roll_no  and ca.roll_no=r.Roll_No and ca.roll_no=sc.roll_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.subject_no=s.subject_no and s.subject_no=ca.subject_no and ca.subject_no=sc.subject_no  and ed.Exam_Month='" + Convert.ToString(examMonth).Trim() + "' and ed.Exam_year='" + Convert.ToString(examYear).Trim() + "' and ed.batch_year='" + batchYear + "' and ed.degree_code='" + degreeCode + "' and r.roll_no='" + rollNo + "' and isnull(r.Reg_No,'') <>'' order by r.Reg_No,subjectpriority", "Text");
                    qry = "select ed.exam_code,m.roll_no,isnull(s.subjectpriority,'0') as subjectpriority,s.credit_points,m.subject_no,s.subject_code,s.subject_name,s.min_int_marks,s.max_int_marks,m.internal_mark,s.min_ext_marks,s.max_ext_marks,m.external_mark,s.mintotal,s.maxtotal,m.total,m.result,m.passorfail,m.MYData,(ed.Exam_year*12+ed.Exam_Month) as YearMonthValue from Exam_Details ed,mark_entry m,subject s where s.subject_no=m.subject_no and ed.exam_code=m.exam_code and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and ed.batch_year='" + batchYear + "' and ed.degree_code='" + degreeCode + "' and m.roll_no='" + rollNo + "' order by subjectpriority";
                    DataSet dsStudentMarks = new DataSet();
                    DataSet dsPreviousInternalMarks = new DataSet();
                    dsStudentMarks = d2.select_method_wo_parameter(qry, text);
                    qry = "select m.roll_no,r.Reg_No,s.subject_no,s.subject_code,s.credit_points,s.subject_name,isnull(s.subjectpriority,'0') as subjectpriority,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + examYear + "'*12+'" + examMonth + "')  and ed.batch_year='" + batchYear + "' and ed.degree_code='" + degreeCode + "' and m.roll_no='" + rollNo + "' and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc,subjectpriority";
                    dsPreviousInternalMarks = d2.select_method_wo_parameter(qry, text);

                    if (dsStudentMarks.Tables.Count > 0 && dsStudentMarks.Tables[0].Rows.Count > 0)
                    {
                        Init_Spread(FpSpreadInternalMarks);
                        int sno = 0;
                        foreach (DataRow drMarks in dsStudentMarks.Tables[0].Rows)
                        {
                            sno++;
                            subjectNo = string.Empty;
                            string rollNo1 = string.Empty;
                            string regNo1 = string.Empty;
                            string creditPoints = string.Empty;
                            subjectNo = string.Empty;
                            subjectCode = string.Empty;
                            subjectName = string.Empty;

                            internalMark = string.Empty;
                            internalMarks = 0;
                            externalMark = string.Empty;
                            externalMarks = 0;
                            total = string.Empty;
                            totalMarks = 0;
                            result = string.Empty;
                            passOrFail = false;

                            minInternal = string.Empty;
                            minExternal = string.Empty;
                            minTotal = string.Empty;

                            minimumInternal = 0;
                            minimumExternal = 0;
                            minimumTotal = 0;

                            maxInternal = string.Empty;
                            maxExternal = string.Empty;
                            maxTotal = string.Empty;

                            maximumInternal = 0;
                            maximumExternal = 0;
                            maximumTotal = 0;

                            examCode = Convert.ToString(drMarks["exam_code"]).Trim();
                            subjectNo = Convert.ToString(drMarks["subject_no"]).Trim();
                            rollNo1 = Convert.ToString(drMarks["roll_no"]).Trim();
                            regNo1 = regNo.Trim();
                            subjectCode = Convert.ToString(drMarks["subject_code"]).Trim();
                            subjectName = Convert.ToString(drMarks["subject_name"]).Trim();

                            minInternal = Convert.ToString(drMarks["min_int_marks"]).Trim();
                            maxInternal = Convert.ToString(drMarks["max_int_marks"]).Trim();
                            internalMark = Convert.ToString(drMarks["internal_mark"]).Trim();

                            maxExternal = Convert.ToString(drMarks["max_ext_marks"]).Trim();
                            minExternal = Convert.ToString(drMarks["min_ext_marks"]).Trim();
                            externalMark = Convert.ToString(drMarks["external_mark"]).Trim();


                            minTotal = Convert.ToString(drMarks["mintotal"]).Trim();
                            maxTotal = Convert.ToString(drMarks["maxtotal"]).Trim();
                            total = Convert.ToString(drMarks["total"]).Trim();

                            result = Convert.ToString(drMarks["result"]).Trim();
                            string passfail = Convert.ToString(drMarks["passorfail"]).Trim();
                            creditPoints = Convert.ToString(drMarks["credit_points"]).Trim();

                            internalMarks = 0;
                            externalMarks = 0;
                            totalMarks = 0;
                            passOrFail = false;
                            minimumInternal = 0;
                            minimumExternal = 0;
                            minimumTotal = 0;
                            maximumInternal = 0;
                            maximumExternal = 0;
                            maximumTotal = 0;
                            double creditPoints1 = 0;

                            double.TryParse(creditPoints, out creditPoints1);
                            double.TryParse(minInternal, out minimumInternal);
                            double.TryParse(maxInternal, out maximumInternal);
                            double.TryParse(internalMark, out internalMarks);

                            double.TryParse(minExternal, out minimumExternal);
                            double.TryParse(maxExternal, out maximumExternal);
                            double.TryParse(externalMark, out externalMarks);

                            double.TryParse(minTotal, out minimumTotal);
                            double.TryParse(maxTotal, out maximumTotal);
                            double.TryParse(total, out totalMarks);
                            bool.TryParse(passfail, out passOrFail);

                            FpSpreadInternalMarks.Sheets[0].RowCount++;
                            FarPoint.Web.Spread.RegExpCellType regExpressionInt = new FarPoint.Web.Spread.RegExpCellType();
                            FarPoint.Web.Spread.RegExpCellType regExpressionExt = new FarPoint.Web.Spread.RegExpCellType();

                            string regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$";
                            string regexpree = "AB|ab|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                            string newExapressionRaja = string.Empty;
                            string roundValuesRaja = string.Empty;
                            if (RoundOff == 0)
                            {
                                roundValuesRaja = "1,2";
                            }
                            for (int round = 1; round <= RoundOff; round++)
                            {
                                if (string.IsNullOrEmpty(roundValuesRaja))
                                {
                                    roundValuesRaja = Convert.ToString(round).Trim();
                                }
                                else
                                {
                                    roundValuesRaja += "," + Convert.ToString(round).Trim();
                                }
                            }
                            for (int i = 0; i <= maximumInternal; i++)
                            {
                                newExapressionRaja += "|" + "^(" + i + ")?$";
                                regexpree = regexpree + "|" + "" + i + "";
                                if (i != maximumInternal)
                                {
                                    newExapressionRaja += @"|" + "^(" + i + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                    for (int d = 0; d < 100; d++)
                                    {
                                        regexpree = regexpree + "|" + "" + i + "." + d;
                                    }
                                }
                                else
                                {
                                    newExapressionRaja += @"|" + "^(" + i + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                }
                            }
                            regExpressionInt.ValidationExpression = regularNewRaja + newExapressionRaja;
                            regExpressionInt.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + maximumInternal + ")";
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].CellType = regExpressionInt;

                            regularNewRaja = @"^(AB)?$|^(Ab)?$|^(aB)?$|^(ab)?$|^(a)?$|^(A)?$|^(M)?$|^(m)?$|^(lt)?$|^(LT)?$|^(Lt)?$|^(lT)?$|^(nr)?$|^(Nr)?$|^(nR)?$|^(NR)?$|^(NE)?$|^(nE)?$|^(Ne)?$|^(ne)?$|^(RA)?$|^(rA)?$|^(Ra)?$|^(ra)?$";
                            regexpree = "AB|ab||NR|nr|NE|ne|ra||RA|a|A|M|m|LT|lt|00|01|02|03|04|05|06|07|08|09|";
                            newExapressionRaja = string.Empty;
                            for (int i = 0; i <= Convert.ToInt32(maximumExternal); i++)
                            {
                                regexpree = regexpree + "|" + "" + i + "";
                                if (i != Convert.ToInt32(maximumExternal))
                                {
                                    newExapressionRaja += @"|" + "^(" + i + ")(\\.[0-9]{" + roundValuesRaja + "})?$";
                                    for (int d = 0; d < 100; d++)
                                    {
                                        regexpree = regexpree + "|" + "" + i + "." + d;
                                    }
                                }
                                else
                                {
                                    newExapressionRaja += @"|" + "^(" + i + ")(\\.[0]{" + roundValuesRaja + "})?$";
                                }
                            }
                            regExpressionExt.ValidationExpression = regularNewRaja + newExapressionRaja;
                            regExpressionExt.ErrorMessage = "Please Enter the Mark Less Than or Equal to (" + maximumExternal + ")";
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].CellType = regExpressionExt;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(examCode).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 1].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 2].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(subjectCode).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 3].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(subjectNo).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 4].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(subjectName).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(creditPoints).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(internalMark).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(minInternal).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(maxInternal).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].Locked = false;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(externalMark).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(minExternal).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].Note = Convert.ToString(maxExternal).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(total).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(minTotal).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].Note = Convert.ToString(maxTotal).Trim();
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].Locked = true;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(result).Trim();
                            FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(passfail).Trim();

                            for (int col = 0; col < FpSpreadInternalMarks.Sheets[0].ColumnCount; col++)
                            {
                                FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].Locked = true;
                                FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].VerticalAlign = VerticalAlign.Middle;
                                if (col == 6)
                                {
                                    FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].Locked = false;
                                }
                                if (col == 5)
                                {

                                    FpSpreadInternalMarks.Sheets[0].Cells[FpSpreadInternalMarks.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                }
                            }
                        }

                        FpSpreadInternalMarks.SaveChanges();
                        FpSpreadInternalMarks.Sheets[0].PageSize = FpSpreadInternalMarks.Sheets[0].RowCount;
                        FpSpreadInternalMarks.Height = 400;
                        FpSpreadInternalMarks.Width = 700;
                        FpSpreadInternalMarks.Visible = true;
                        divMainGrid.Visible = true;
                        divPrint.Visible = true;
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "No Record(s) Were Found.Please Check Mark Entry!!!";
                        return;
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    return;
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Student Were Found";
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Go Click

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
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Close

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpSpreadInternalMarks.Visible == true)
                {
                    d2.printexcelreport(FpSpreadInternalMarks, reportname);
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
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string rptheadname = "Internal Marks Updation";
            string pagename = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString();
            if (FpSpreadInternalMarks.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpreadInternalMarks, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #region Save Click

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSuccess = false;
            FpSpreadInternalMarks.SaveChanges();
            if (FpSpreadInternalMarks.Sheets[0].RowCount > 0)
            {
                int RoundOff = 0;

                //qry = "select m.roll_no,r.Reg_No,s.subject_no,s.subject_code,s.credit_points,s.subject_name,isnull(s.subjectpriority,'0') as subjectpriority,m.internal_mark,m.exam_code,ED.Exam_year,ED.Exam_Month,(ed.Exam_year*12+ed.Exam_Month) EXAMYEARMONTHVAL from mark_entry m,subject s,Registration r,Exam_Details ed where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and ed.exam_code=m.exam_code and (ed.Exam_year*12+ed.Exam_Month)<=('" + examYear + "'*12+'" + examMonth + "')  and ed.batch_year='" + batchYear + "' and ed.degree_code='" + degreeCode + "' and m.roll_no='" + rollNo + "' and m.internal_mark is not null order by m.roll_no,EXAMYEARMONTHVAL DESC,ed.Exam_year,ED.Exam_Month desc,subjectpriority";
                //DataSet dsPreviousInternalMarks = d2.select_method_wo_parameter(qry, text);

                string getmarkround = d2.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'").Trim();
                int.TryParse(getmarkround, out RoundOff);
                int my = Convert.ToInt32(ddlExamMonth.SelectedIndex.ToString()) + Convert.ToInt32(ddlExamYear.SelectedValue.ToString()) * 12;
                for (int row = 0; row < FpSpreadInternalMarks.Sheets[0].RowCount; row++)
                {
                    subjectNo = string.Empty;
                    string rollNo1 = string.Empty;
                    string regNo1 = string.Empty;
                    string creditPoints = string.Empty;
                    subjectNo = string.Empty;
                    subjectCode = string.Empty;
                    subjectName = string.Empty;

                    internalMark = string.Empty;
                    internalMarks = 0;
                    externalMark = string.Empty;
                    externalMarks = 0;
                    total = string.Empty;
                    totalMarks = 0;
                    result = string.Empty;
                    passOrFail = false;

                    minInternal = string.Empty;
                    minExternal = string.Empty;
                    minTotal = string.Empty;

                    minimumInternal = 0;
                    minimumExternal = 0;
                    minimumTotal = 0;

                    maxInternal = string.Empty;
                    maxExternal = string.Empty;
                    maxTotal = string.Empty;

                    maximumInternal = 0;
                    maximumExternal = 0;
                    maximumTotal = 0;

                    examCode = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 0].Tag).Trim();
                    subjectNo = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 4].Text).Trim();
                    rollNo1 = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 1].Text).Trim();
                    regNo1 = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 2].Text).Trim();
                    subjectCode = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 3].Text).Trim();
                    subjectName = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 5].Text).Trim();
                    creditPoints = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 5].Tag).Trim();

                    minInternal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 6].Tag).Trim();
                    maxInternal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 6].Note).Trim();
                    internalMark = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 6].Text).Trim();

                    minExternal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 7].Tag).Trim();
                    maxExternal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 7].Note).Trim();
                    externalMark = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 7].Text).Trim();

                    minTotal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 8].Tag).Trim();
                    maxTotal = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 8].Note).Trim();
                    total = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 8].Text).Trim();

                    result = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 9].Text).Trim();
                    string passfail = Convert.ToString(FpSpreadInternalMarks.Sheets[0].Cells[row, 9].Tag).Trim();

                    double creditPoints1 = 0;

                    double.TryParse(creditPoints, out creditPoints1);
                    internalMarks = 0;
                    externalMarks = 0;
                    totalMarks = 0;
                    passOrFail = false;
                    minimumInternal = 0;
                    minimumExternal = 0;
                    minimumTotal = 0;
                    maximumInternal = 0;
                    maximumExternal = 0;
                    maximumTotal = 0;

                    //if (dsPreviousInternalMarks.Tables.Count > 0 && dsPreviousInternalMarks.Tables[0].Rows.Count > 0)
                    //{
                    //    dsPreviousInternalMarks.Tables[0].DefaultView.RowFilter = "";
                    //    DataView dvPrev = dsPreviousInternalMarks.Tables[0].DefaultView;
                    //    attempts = dvPrev.Count + 1;
                    //}

                    double.TryParse(minInternal, out minimumInternal);
                    double.TryParse(maxInternal, out maximumInternal);
                    double.TryParse(internalMark, out internalMarks);

                    double.TryParse(minExternal, out minimumExternal);
                    double.TryParse(maxExternal, out maximumExternal);
                    double.TryParse(externalMark, out externalMarks);

                    double.TryParse(minTotal, out minimumTotal);
                    double.TryParse(maxTotal, out maximumTotal);
                    double.TryParse(total, out totalMarks);
                    bool.TryParse(passfail, out passOrFail);

                    if (string.IsNullOrEmpty(internalMark.Trim()))
                    {
                        passfail = "0";
                        passOrFail = false;
                        result = "Null";
                        internalMark = "Null";
                    }
                    else
                    {
                        if (internalMark.Trim().ToLower().Contains('a') || internalMark == "-1")// == "ab")
                        {
                            internalMark = "-1";
                        }
                        else
                        {
                            double.TryParse(internalMark, out internalMarks);
                        }
                    }
                    if (string.IsNullOrEmpty(externalMark.Trim()))
                    {
                        externalMark = "Null";
                        externalMarks = 0;
                        result = "Null";
                        total = "Null";
                    }
                    else
                    {
                        if (externalMark.Trim().ToLower().Contains('a') || externalMark.Trim().ToLower() == "-1")
                        {
                            externalMark = "-1";
                            result = "AAA";
                            externalMarks = -1;
                        }
                        else if (externalMark.Trim().ToLower().Contains("ne") || externalMark.Trim().ToLower() == "-2")
                        {
                            externalMark = "-2";
                            externalMarks = -2;
                        }
                        else if (externalMark.Trim().ToLower().Contains("nr") || externalMark.Trim().ToLower() == "-3")
                        {
                            externalMark = "-3";
                            externalMarks = -3;
                        }
                        else if (externalMark.Trim().ToLower().Contains("m"))
                        {
                            externalMark = "0";
                            externalMarks = 0;
                            result = "WHD";
                        }
                        else if (externalMark.Trim().ToLower().Contains("lt") || externalMark.Trim() == "-4")
                        {
                            externalMark = "-4";
                            externalMarks = -4;
                        }
                        else
                        {
                            externalMark = externalMark;
                        }
                        if (internalMarks > 0)
                        {
                            if (externalMarks > 0)
                            {
                                totalMarks = Convert.ToDouble(externalMarks) + Convert.ToDouble(internalMarks);
                            }
                            else
                            {
                                totalMarks = Convert.ToDouble(internalMarks);
                            }
                        }
                        else if (externalMarks > 0)
                        {
                            totalMarks = Convert.ToDouble(externalMarks);
                        }
                        total = totalMarks.ToString();
                        result = "Fail";
                        if (minimumInternal <= internalMarks && minimumExternal <= externalMarks && minimumTotal <= totalMarks)
                        {
                            result = "Pass";
                        }
                    }
                    string insupdquery = "if not exists(select * from mark_entry where exam_code='" + examCode + "' and roll_no='" + rollNo1 + "' and subject_no='" + subjectNo + "')";
                    insupdquery += " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,MYData,rej_stat,cp)";
                    //,evaluation1,evaluation2,evaluation3
                    insupdquery = insupdquery + " values('" + rollNo1 + "','" + subjectNo + "','" + examCode + "'," + internalMark + "," + externalMark + "," + total + ",'" + result + "','" + passfail + "','" + my + "','0','" + creditPoints + "')";
                    //," + evauation1 + "," + evauation2 + "," + evauation3 + "
                    insupdquery += " else";
                    insupdquery += " update mark_entry set internal_mark=" + internalMark + ",external_mark=" + externalMark + ",total=" + total + ",result='" + result + "',passorfail='" + passfail + "'";
                    //,evaluation1=" + evauation1 + ",evaluation2=" + evauation2 + ",evaluation3=" + evauation3 + "";
                    insupdquery += " where exam_code='" + examCode + "' and roll_no='" + rollNo1 + "' and subject_no='" + subjectNo + "'";
                    int insupdval = d2.insert_method(insupdquery, hat, "Text");

                    if (insupdval > 0)
                    {
                        isSuccess = true;
                    }
                }
                btnGo_Click(sender, e);
                if (isSuccess)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Saved Successfully";
                    return;
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Not Saved";
                    return;
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Record(s) Were Found";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Save Click

}