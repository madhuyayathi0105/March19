using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Web.Services;

public partial class HRMOD_ITOtherAllowanceDeduction : System.Web.UI.Page
{
    Boolean cellclick = false;
    Boolean cellclick1 = false;
    string usercode = string.Empty;
    static string autocol = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    FarPoint.Web.Spread.CheckBoxCellType chksel = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkselall = new FarPoint.Web.Spread.CheckBoxCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            //collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lbl_validation.Visible = false;
            if (!IsPostBack)
            {
                bindclg();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                if (ddl_popclg.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
                    autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
                }
                FpSpread1.Visible = false;
                FpSpread2.Visible = false;
                rb_allow.Checked = true;
                load_allowance();
                btn_go_Click(sender, e);
            }
            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
            }
            if (ddl_popclg.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
                autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
            }
            lbl_error.Visible = false;
        }
        catch (Exception ex) { }
    }

    public void hide()
    {
        Printcontrol.Visible = false;
        rptprint.Visible = false;
        txt_excelname.Text = "";
        addnew.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btndel.Visible = false;
        btnupdate.Visible = false;
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        rptprint.Visible = false;
        lbl_error.Visible = false;
        rb_allow.Checked = true;
        rb_deduct.Checked = false;
        btn_go_Click(sender, e);
    }

    protected void ddl_popclg_Change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        if (rb_allow.Checked == true)
        {
            string selectQuery = d2.GetFunction(" select allowances  from incentives_master where college_code ='" + autocol + "'");
            if (selectQuery != "" && selectQuery.Trim() != "0")
            {
                if (txt_name.Text.Trim() != "")
                {
                    string[] al1 = selectQuery.Split(';');
                    for (int r = 0; r < al1.Length; r++)
                    {
                        string[] al2 = al1[r].Split('\\');
                        string al3 = al2[0];
                        if (al3.Trim().ToUpper() == txt_name.Text.Trim().ToUpper())
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Allowance Already Exist!";
                            txt_name.Text = "";
                            return;
                        }
                    }
                }
            }
        }
        else if (rb_deduct.Checked == true)
        {
            string dedsel = d2.GetFunction(" select deductions from incentives_master where college_code ='" + autocol + "'");
            if (dedsel != "")
            {
                if (txt_name.Text.Trim() != "")
                {
                    string[] al1 = dedsel.Split(';');
                    for (int r = 0; r < al1.Length; r++)
                    {
                        string[] al2 = al1[r].Split('\\');
                        string al3 = al2[0];
                        if (al3.Trim().ToUpper() == txt_name.Text.Trim().ToUpper())
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Deduction Already Exist!";
                            txt_name.Text = "";
                            return;
                        }
                    }
                }
            }
        }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            ddl_popclg.Items.Clear();

            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();

                ddl_popclg.DataSource = ds;
                ddl_popclg.DataTextField = "collname";
                ddl_popclg.DataValueField = "college_code";
                ddl_popclg.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        lbl_maxAge.Visible = false;
        txt_maxAge.Visible = false;
        lbl_maxval.Visible = false;
        txt_Maxval.Visible = false;
        lbl_MinAge.Visible = false;
        txt_MinAge.Visible = false;
        lbl_minVal.Visible = false;
        txt_minval.Visible = false;
        cb_agesetting.Checked = false;
        ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
        ddl_popclg.Enabled = true;
        collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        if (rb_allow.Checked == true)
        {
            cb_splallow.Visible = true;

            lbl_newdesg.Text = "Other Income Head";
            rb_allow.Enabled = true;
            txt_name.Enabled = true;
            rb_deduct.Checked = false;
            addnew.Visible = true;
            txt_name.Text = "";
            txt_desc.Text = "";
            txt_allacr.Text = "";
            lbl_error.Text = "";
            btn_save.Text = "Save";
            cb_splallow.Checked = false;
            cbincomeTax.Checked = false;
            cbincomeTax.Visible = false;
            cb_splallow.Text = "Common Income";
        }
        if (rb_deduct.Checked == true)
        {

            lbl_newdesg.Text = "Other Deduction Head";
            rb_deduct.Enabled = true;
            rb_allow.Enabled = true;
            txt_name.Enabled = true;
            rb_allow.Checked = false;
            addnew.Visible = true;
            txt_name.Text = "";
            txt_desc.Text = "";
            txt_allacr.Text = "";
            lbl_error.Text = "";
            btn_save.Text = "Save";
            cbincomeTax.Visible = true;
            cbincomeTax.Checked = false;
            cb_splallow.Checked = false;
            cb_splallow.Text = "Common Deduction";

        }
        if (rd_salary.Checked == true)//delsi 1408
        {
            lbl_newdesg.Text = "Salary Certificate Mapping";
            rb_deduct.Enabled = true;
            rb_allow.Enabled = true;
            txt_name.Enabled = true;
            rb_allow.Checked = false;
            addnew.Visible = true;
            txt_name.Text = "";
            txt_desc.Text = "";
            txt_allacr.Text = "";
            lbl_error.Text = "";
            btn_save.Text = "Save";
            cbincomeTax.Visible = true;
            cbincomeTax.Checked = false;
            cb_splallow.Checked = false;
            cb_splallow.Text = "Common Deduction";
            cb_agesetting.Visible = false;
            cbincomeTax.Visible = false;
        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            hide();
            if (rb_allow.Checked == true)
            {
                loadspread1();
            }
            if (rb_deduct.Checked == true)
            {
                loadspread2();
            }
            if (rd_salary.Checked == true)//delsi 1408
            {
                loadspread3();
            }
            int cc = 0;
            if (cc > 0)
            {
                lbl_error.Text = "";
            }
        }
        catch { }
    }

    protected void loadspread1()
    {
        try
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Visible = true;
            chkselall.AutoPostBack = true;
            chksel.AutoPostBack = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Other Income Head";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Description";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Common Allowances";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 75;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 75;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 215;
            FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 185;
            FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;

            string selectQuery = " select IT_ID,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue from IT_OtherAllowanceDeducation where ITType='1' and CollegeCode ='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string a = "";
                string b1 = "";
                string b2 = "";
                string b3 = "";

                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chkselall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    b1 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductName"]);
                    b2 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductDiscription"]);
                    b3 = Convert.ToString(ds.Tables[0].Rows[i]["ITCommon"]);
                    a = Convert.ToString(ds.Tables[0].Rows[i]["ITCommonValue"]);
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IT_ID"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chksel;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = b1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = b2;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = a;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                rptprint.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 670;
                FpSpread1.Height = 500;
                btndel.Visible = true;
                btnupdate.Visible = true;
                lbl_error.Visible = false;
                FpSpread1.Sheets[0].FrozenRowCount = 1;
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
                hide();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "AllowanceAndDeductionMaster_Alter.aspx");
        }
    }

    protected void loadspread2()
    {
        try
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].Columns.Count = 5;

            chkselall.AutoPostBack = true;
            chksel.AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Other Deduction Head";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Descriptions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Common Deduction";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].Columns[0].Width = 75;
            FpSpread1.Sheets[0].Columns[1].Width = 75;
            FpSpread1.Sheets[0].Columns[2].Width = 215;
            FpSpread1.Sheets[0].Columns[3].Width = 185;
            FpSpread1.Sheets[0].Columns[4].Width = 100;

            string selectQuery = " select IT_ID,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue from IT_OtherAllowanceDeducation where ITType='2' and CollegeCode ='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string a = "";
                string b1 = "";
                string b2 = "";
                string b3 = "";

                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chkselall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    b1 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductName"]);
                    b2 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductDiscription"]);
                    b3 = Convert.ToString(ds.Tables[0].Rows[i]["ITCommon"]);
                    a = Convert.ToString(ds.Tables[0].Rows[i]["ITCommonValue"]);
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IT_ID"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chksel;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = b1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = b2;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = a;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                rptprint.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 670;
                FpSpread1.Height = 500;
                btndel.Visible = true;
                btnupdate.Visible = true;
                lbl_error.Visible = false;
                FpSpread1.Sheets[0].FrozenRowCount = 1;
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
                hide();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "AllowanceAndDeductionMaster_Alter.aspx");
        }
    }

    [WebMethod]
    public static string checkAlldedName(string AlldedName, string alltype)
    {
        string returnValue = "0";
        try
        {
            DAccess2 dd = new DAccess2();
            if (alltype == "0")
            {
                string selectQuery = dd.GetFunction(" select allowances  from incentives_master where college_code ='" + autocol + "'");
                if (selectQuery != "")
                {
                    if (AlldedName != "")
                    {
                        string[] al1 = selectQuery.Split(';');
                        for (int r = 0; r < al1.Length; r++)
                        {
                            string[] al2 = al1[r].Split('\\');
                            string al3 = al2[0];
                            if (al3.Trim().ToUpper() == AlldedName.Trim().ToUpper())
                            {
                                returnValue = "1";
                            }
                        }
                    }
                    else
                    {
                        returnValue = "2";
                    }
                }
            }
            else
            {
                string selectQuery = dd.GetFunction(" select deductions from incentives_master where college_code ='" + autocol + "'");
                if (selectQuery != "")
                {
                    if (AlldedName != "")
                    {
                        string[] al1 = selectQuery.Split(';');
                        for (int r = 0; r < al1.Length; r++)
                        {
                            string[] al2 = al1[r].Split('\\');
                            string al3 = al2[0];
                            if (al3.Trim().ToUpper() == AlldedName.Trim().ToUpper())
                            {
                                returnValue = "1";
                            }
                        }
                    }
                    else
                    {
                        returnValue = "2";
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [WebMethod]
    public static string checkAlldedAcr(string AlldedAcr)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string allded_acr = AlldedAcr;
            if (allded_acr.Trim() != "" && allded_acr != null)
            {
                string queryalldedacr = dd.GetFunction("select distinct AllowDedAcr,AllowDedMasterPK from HRM_AllowDedMaster where AllowDedAcr='" + allded_acr + "'");
                if (queryalldedacr.Trim() == "" || queryalldedacr == null || queryalldedacr == "0" || queryalldedacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string collcode = "";
        try
        {
            string allowid = "";
            string MaxAgeValue = string.Empty;
            string MinAgeValue = string.Empty;
            Boolean checkedage = false;

            string CollCode = string.Empty;
            string name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_name.Text.ToString());
            //string name =txt_name.Text.ToString();
            string des = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_desc.Text.ToString());
            string acr = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_allacr.Text.ToString());
            string actrow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actrow) != -1)
            {
                allowid = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 0].Tag);
            }
            string CommonDeducation = string.Empty;
            string ITType = string.Empty;
            string ITCommon = string.Empty;
            string ITIncomeTax = string.Empty;
            CollCode = Convert.ToString(ddl_popclg.SelectedValue);
            if (cb_splallow.Checked == true)
            {
                ITCommon = "1";
                //CommonDeducation = Convert.ToString(ddlotherAllowance.SelectedItem.Text);
                CommonDeducation = rs.GetSelectedItemsValue(cblOtherallowance);
            }
            else
            {
                ITCommon = "0";
            }
            if (rb_allow.Checked)
            {
                ITType = "1";
            }
            else if (rb_deduct.Checked)
            {
                ITType = "2";
            }
            else if (rd_salary.Checked)//delsi 1408
            {
                ITType = "3";
            
            }

            if (cbincomeTax.Checked)
            {
                ITIncomeTax = "1";
            }
            else
            {
                ITIncomeTax = "0";
            }
            if (cb_agesetting.Checked == true)
            {

                MaxAgeValue = Convert.ToString(txt_maxAge.Text) + "-" + Convert.ToString(txt_Maxval.Text);
                MinAgeValue = Convert.ToString(txt_MinAge.Text) + "-" + Convert.ToString(txt_minval.Text);
                checkedage = true;

            }
            string updqry = string.Empty;
            if (btn_save.Text.ToLower().Trim() == "save")
            {
                collcode = collegecode;
                if (lbl_newdesg.Text == "Other Income Head")
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',IsIncomeTax='" + ITIncomeTax + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "' where ITType='" + ITCommon + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "' else  insert into IT_OtherAllowanceDeducation (ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,CollegeCode,IsIncomeTax,IsAgeRange,MaxValue,MinValue) values ('" + ITType + "','" + name.Trim() + "','" + des.Trim() + "','" + ITCommon.Trim() + "','" + CommonDeducation + "','" + CollCode + "','" + ITIncomeTax + "','" + checkedage + "','" + MaxAgeValue + "','" + MinAgeValue + "')";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        // btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Other Deduction Head")
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',IsIncomeTax='" + ITIncomeTax + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "' where ITType='" + ITCommon + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "' else  insert into IT_OtherAllowanceDeducation (ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,CollegeCode,IsIncomeTax,IsAgeRange,MaxValue,MinValue) values ('" + ITType + "','" + name.Trim() + "','" + des.Trim() + "','" + ITCommon.Trim() + "','" + CommonDeducation + "','" + CollCode + "','" + ITIncomeTax + "','" + checkedage + "','" + MaxAgeValue + "','" + MinAgeValue + "')";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        // btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Salary Certificate Mapping")//ITType 3 for salary Certificate
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',IsIncomeTax='" + ITIncomeTax + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "' where ITType='" + ITCommon + "' and ITAllowDeductName='" + name.Trim() + "' and CollegeCode ='" + CollCode + "' else  insert into IT_OtherAllowanceDeducation (ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,CollegeCode,IsIncomeTax,IsAgeRange,MaxValue,MinValue) values ('" + ITType + "','" + name.Trim() + "','" + des.Trim() + "','" + ITCommon.Trim() + "','" + CommonDeducation + "','" + CollCode + "','" + ITIncomeTax + "','" + checkedage + "','" + MaxAgeValue + "','" + MinAgeValue + "')";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        // btn_go_Click(sender, e);
                    }
                
                }
            }


            if (btn_save.Text.ToLower().Trim() == "update")
            {
                collcode = collegecode1;
                string[] newspl = new string[2];
                if (lbl_newdesg.Text == "Other Income Head")
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',ITAllowDeductName='" + name.Trim() + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "' where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "'";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Updated Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Other Deduction Head")
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',ITAllowDeductName='" + name.Trim() + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "'  where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "'";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Updated Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Salary Certificate Mapping")//ITType 3 for salary Certificate
                {
                    updqry = "if exists (select * from IT_OtherAllowanceDeducation where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "') update IT_OtherAllowanceDeducation set ITAllowDeductDiscription='" + des + "',ITCommon='" + ITCommon + "',ITCommonValue='" + CommonDeducation + "',ITAllowDeductName='" + name.Trim() + "',IsAgeRange='" + checkedage + "',MaxValue='" + MaxAgeValue + "',MinValue='" + MinAgeValue + "'  where ITType='" + ITType + "' and IT_ID='" + allowid.Trim() + "' and CollegeCode ='" + CollCode + "'";
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Updated Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
            }
        }
        catch (Exception ex)
        {
           // d2.sendErrorMail(ex, collcode, "AllowanceAndDeductionMaster_Alter.aspx");
        }
    }

    protected void cb_pfdeduct_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string actrow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            string actcol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            string allowdedid = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
            string selquery = "select COUNT(*) as count from HRM_AllowDedMaster where IsPF = 1 and CollegeCode = '" + collegecode1 + "' and AllowDedMasterPK not in('" + allowdedid + "')";
            string getcount = d2.GetFunction(selquery);
            int pfcount = Convert.ToInt32(getcount);
            if (pfcount >= 1)
            {
                lbl_alert.Text = "The PF should be deducted only once!";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }
            else
            {
                lbl_alert.Visible = false;
                imgdiv2.Visible = false;
            }
        }
        catch { }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {

            string degreedetails = "AllowanceAndDetectionMaster";
            string pagename = "AllowanceAndDetectionMaster_Alter.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_validation.Visible = false;
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (rb_allow.Checked == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);
                }
                if (rb_deduct.Checked == true)
                {
                    d2.printexcelreport(FpSpread2, reportname);
                }
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch { }
    }

    public void rb_allow_CheckedChanged(object sender, EventArgs e)
    {

        cb_splallow.Enabled = true;

        txt_name.Text = "";
        txt_desc.Text = "";
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        lbl_error.Visible = false;
        rptprint.Visible = false;
        load_allowance();
    }

    public void rb_deduct_OncheckedChanged(object sender, EventArgs e)
    {
        txt_name.Text = "";
        txt_desc.Text = "";
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        lbl_error.Visible = false;
        rptprint.Visible = false;
        load_allowance();
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void btndel_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = true;
        lblalert.Visible = true;
        lblalert.Text = "Do you want to Delete this Record?";
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {

            lbl_maxAge.Visible = false;
            txt_maxAge.Visible = false;
            lbl_maxval.Visible = false;
            txt_Maxval.Visible = false;
            lbl_MinAge.Visible = false;
            txt_MinAge.Visible = false;
            lbl_minVal.Visible = false;
            txt_minval.Visible = false;
            cb_agesetting.Checked = false;
            int count = 0;
            load_allowance();
            ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
            collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
            autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
            ddl_popclg.Enabled = false;
            cblOtherallowance.ClearSelection();
            cbOtherallowance.Checked = false;
            txtOtherallowance.Text = "";
            if (FpSpread1.Visible == true)
            {
                if (checkedOK(FpSpread1, out count))
                {
                    if (count == 1)
                    {
                        lbl_error.Visible = false;
                        addnew.Visible = true;
                        cb_splallow.Visible = true;

                        FpSpread1.SaveChanges();
                        for (int ik = 0; ik < FpSpread1.Sheets[0].Rows.Count; ik++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[ik, 1].Value);
                            if (check == 1)
                            {
                                txt_name.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 2].Text);
                                txt_desc.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 3].Text);
                                string getval = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 4].Text);
                                if (getval.Trim() != "")
                                {
                                    cb_splallow.Checked = true;
                                    //ddlotherAllowance.SelectedIndex = ddlotherAllowance.Items.IndexOf(ddlotherAllowance.Items.FindByText(getval));
                                    if (!string.IsNullOrEmpty(getval))
                                    {                                       
                                        string[] DeductionName = getval.Split(',');
                                        foreach (var item in DeductionName)
                                        {
                                            if (!string.IsNullOrEmpty(item))
                                                cblOtherallowance.Items.FindByValue(item).Selected = true;
                                        }
                                        cbOtherallowance.Checked = false;
                                        txtOtherallowance.Text = rs.GetSelectedItemsValue(cblOtherallowance);
                                    }
                                }
                                else
                                {
                                    cb_splallow.Checked = false;
                                }

                            }
                        }

                        btn_save.Text = "Update";
                        lbl_newdesg.Text = "Other Income Head";
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Any one Allowance to Update!";
                    }
                }
                else
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select Any one Allowance to Update!";
                }

            }
        }
        catch { }
    }

    public void Cell_Click(object sender, EventArgs e)
    {
        cellclick = true;
    }

    protected void Cell_Click1(object sender, EventArgs e)
    {
        cellclick1 = true;
    }

    protected void Fpspread1_ButtonCommand(object sender, EventArgs e)
    {
        Spreadchanges(FpSpread1);
    }

    protected void Fpspread2_ButtonCommand(object sender, EventArgs e)
    {
        Spreadchanges(FpSpread2);
    }

    private void Spreadchanges(FarPoint.Web.Spread.FpSpread fpspread)
    {
        fpspread.SaveChanges();
        try
        {
            byte val = Convert.ToByte(fpspread.Sheets[0].Cells[0, 1].Value);
            if (val == 1)
            {
                for (int ik = 1; ik < fpspread.Sheets[0].Rows.Count; ik++)
                {
                    fpspread.Sheets[0].Cells[ik, 1].Value = 1;
                }
            }
            else
            {
                for (int ik = 1; ik < fpspread.Sheets[0].Rows.Count; ik++)
                {
                    fpspread.Sheets[0].Cells[ik, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    private bool checkedOK(FarPoint.Web.Spread.FpSpread fpspread, out int count)
    {
        count = 0;
        fpspread.SaveChanges();
        bool chkok = false;
        try
        {
            for (int ik = 1; ik < fpspread.Sheets[0].Rows.Count; ik++)
            {
                byte check = Convert.ToByte(fpspread.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                {
                    chkok = true;
                    count++;
                }
            }
        }
        catch { }
        return chkok;
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            bool CheckFlag = false;
            if (checkedOK(FpSpread1, out count))
            {
                for (int ik = 1; ik < FpSpread1.Sheets[0].Rows.Count; ik++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[ik, 1].Value);
                    if (check == 1)
                    {
                        string IDValue = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 0].Tag);
                        string updqry = "delete IT_OtherAllowanceDeducation where IT_ID='" + IDValue + "'";
                        int qry = d2.update_method_wo_parameter(updqry, "Text");
                        if (qry > 0)
                        {
                            CheckFlag = true;
                        }
                    }
                }
                if (CheckFlag == true)
                {
                    lbl_alert.Text = "Deleted Successfully";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    imgdiv1.Visible = false;
                    lblalert.Visible = false;
                    btn_go_Click(sender, e);
                }
                else
                {
                    lbl_alert.Text = "Not Deleted";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    imgdiv1.Visible = false;
                    lblalert.Visible = false;
                    btn_go_Click(sender, e);
                }
            }
            else
            {
                imgdiv1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select any one Record!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "AllowanceAndDeductionMaster_Alter.aspx");
        }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
    }


    void load_allowance()
    {
        //ddlotherAllowance.Items.Clear();
        cblOtherallowance.Items.Clear();
        ds.Clear();
        string Query = "Select * from incentives_master where college_code=" + ddl_popclg.SelectedValue + "";
        ds = d2.select_method_wo_parameter(Query, "Text");
        string allowanmce = "";
        string detection = "";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            allowanmce = ds.Tables[0].Rows[0]["allowances"].ToString();
            detection = ds.Tables[0].Rows[0]["deductions"].ToString();
        }
        if (rb_allow.Checked)
        {
            string[] allowanmce_arr;
            allowanmce_arr = allowanmce.Split(';');

            for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
            {
                string all2 = allowanmce_arr[i];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3.GetUpperBound(0) > 1)
                {
                    //all2 = splitallo3[2];
                    all2 = splitallo3[0];
                }
                if (all2.Trim() != "")
                {
                    // ddlotherAllowance.Items.Add(all2);
                    cblOtherallowance.Items.Add(all2);
                }
            }
        }
        if (rb_deduct.Checked)
        {
            string[] detection_arr;
            detection_arr = detection.Split(';');
            for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
            {
                string all2 = detection_arr[j];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3.GetUpperBound(0) > 0)
                {
                    //all2 = splitallo3[1];
                    all2 = splitallo3[0];
                    if (all2.Trim() != "")
                    {
                        //ddlotherAllowance.Items.Add(all2);
                        cblOtherallowance.Items.Add(all2);
                    }
                }
            }
        }
        if (rd_salary.Checked)//delsi 1408
        {
            string[] detection_arr;
            detection_arr = detection.Split(';');
            for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
            {
                string all2 = detection_arr[j];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3.GetUpperBound(0) > 0)
                {
                    //all2 = splitallo3[1];
                    all2 = splitallo3[0];
                    if (all2.Trim() != "")
                    {
                        //ddlotherAllowance.Items.Add(all2);
                        cblOtherallowance.Items.Add(all2);
                    }
                }
            }
        
        }
    }
    protected void cbOtherallowanceOnCheckedChanged(object sender, EventArgs e)
    {
        //rs.CallCheckBoxChangedEvent(cblOtherallowance, cbOtherallowance, txtOtherallowance, Name);
        if (cbOtherallowance.Checked)
            for (int i = 0; i < cblOtherallowance.Items.Count; i++)
                cblOtherallowance.Items[i].Selected = true;
        else
            for (int i = 0; i < cblOtherallowance.Items.Count; i++)
                cblOtherallowance.Items[i].Selected = false;
        txtOtherallowance.Text = rs.GetSelectedItemsValue(cblOtherallowance);
    }
    protected void cblOtherallowanceOnSelectedIndexChanged(object sender, EventArgs e)
    {
        //rs.CallCheckBoxListChangedEvent(cblOtherallowance, cbOtherallowance, txtOtherallowance, Name);
        int count = 0;
        for (int i = 0; i < cblOtherallowance.Items.Count; i++)
            if (cblOtherallowance.Items[i].Selected == true)
                count++;
        if (count == cblOtherallowance.Items.Count)
            cbOtherallowance.Checked = true;
        txtOtherallowance.Text = rs.GetSelectedItemsValue(cblOtherallowance);
    }
    public void cb_ageSettingClick(object sender, EventArgs e)
    {
        if (cb_agesetting.Checked == true)
        {
            lbl_maxAge.Visible = true;
            txt_maxAge.Visible = true;
            lbl_maxval.Visible = true;
            txt_Maxval.Visible = true;
            lbl_MinAge.Visible = true;
            txt_MinAge.Visible = true;
            lbl_minVal.Visible = true;
            txt_minval.Visible = true;
           
           

        }
        if (cb_agesetting.Checked == false)
        {
            lbl_maxAge.Visible = false;
            txt_maxAge.Visible = false;
            lbl_maxval.Visible = false;
            txt_Maxval.Visible = false;
            lbl_MinAge.Visible = false;
            txt_MinAge.Visible = false;
            lbl_minVal.Visible = false;
            txt_minval.Visible = false;
        }
    }

    public void rb_salary_OncheckedChanged(object sender, EventArgs e)
    {
        txt_name.Text = "";
        txt_desc.Text = "";
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        lbl_error.Visible = false;
        rptprint.Visible = false;
        load_allowance();
    }
    protected void loadspread3()//delsi 1408
    {
        try
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].Columns.Count = 5;

            chkselall.AutoPostBack = true;
            chksel.AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Other Deduction Head";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Descriptions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Common Deduction";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].Columns[0].Width = 75;
            FpSpread1.Sheets[0].Columns[1].Width = 75;
            FpSpread1.Sheets[0].Columns[2].Width = 215;
            FpSpread1.Sheets[0].Columns[3].Width = 185;
            FpSpread1.Sheets[0].Columns[4].Width = 100;

            string selectQuery = " select IT_ID,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue from IT_OtherAllowanceDeducation where ITType='3' and CollegeCode ='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string a = "";
                string b1 = "";
                string b2 = "";
                string b3 = "";

                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chkselall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    b1 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductName"]);
                    b2 = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductDiscription"]);
                    b3 = Convert.ToString(ds.Tables[0].Rows[i]["ITCommon"]);
                    a = Convert.ToString(ds.Tables[0].Rows[i]["ITCommonValue"]);
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IT_ID"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chksel;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = b1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = b2;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = a;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                rptprint.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 670;
                FpSpread1.Height = 500;
                btndel.Visible = true;
                btnupdate.Visible = true;
                lbl_error.Visible = false;
                FpSpread1.Sheets[0].FrozenRowCount = 1;
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
                hide();
            }


        }
        catch (Exception ex)
        { 
        
        
        }
    }
}