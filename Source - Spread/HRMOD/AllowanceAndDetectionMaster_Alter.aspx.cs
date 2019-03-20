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

public partial class AllowanceAndDetectionMaster_Alter : System.Web.UI.Page
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
            //****************************************************//
            //if (Session["collegecode"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
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
                cb_pfdeduct.Enabled = false;
                cb_autodeduct.Enabled = false;
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
                            lbl_alert.Text = "Allowance/Addition Already Exist!";
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
        ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
        ddl_popclg.Enabled = true;
        collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
        autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
        if (rb_allow.Checked == true)
        {
            cb_splallow.Visible = true;
            cb_pfdeduct.Visible = false;
            cb_autodeduct.Visible = false;
            lbl_newdesg.Text = "Allowance/Addition";
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
            cb_ItCalcAllow.Visible = true;
            cb_ItCalcDeduc.Visible = false;
            
        }
        if (rb_deduct.Checked == true)
        {
            cb_splallow.Visible = false;
            cb_pfdeduct.Visible = true;
            cb_autodeduct.Visible = true;
            lbl_newdesg.Text = "Deduction";
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
            cb_pfdeduct.Checked = false;
            cb_autodeduct.Checked = false;
            cb_ItCalcAllow.Visible = false;
            cb_ItCalcDeduc.Visible = true;
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

            FpSpread1.Sheets[0].FrozenRowCount = 1;

            string selectQuery = d2.GetFunction(" select allowances  from incentives_master where college_code ='" + collegecode1 + "'");
            if (selectQuery != "" && selectQuery != "0")
            {
                string[] split1 = selectQuery.Split(';');
                FpSpread1.Sheets[0].Rows.Count = 0;

                string a = "";
                string b1 = "";
                string b2 = "";
                string b3 = "";
                string isspl = "";
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chkselall;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";

                for (int i = 0; i < split1.Length; i++)
                {
                    a = split1[i];
                    if (a != "" && a != null)
                    {
                        string[] split2 = a.Split('\\');
                        if (split2.Length >= 3)
                        {
                            b1 = split2[0];
                            b2 = split2[1];
                            b3 = split2[2];
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chksel;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = b1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = b3;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";
                        if (b2 == "1")
                        {
                            isspl = "Yes";
                        }
                        else
                        {
                            isspl = "No";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = isspl;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

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

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Allowances/Additions";
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

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Special Allowance/Addition";
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


                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                rptprint.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 655;
                FpSpread1.Height = 500;
                btndel.Visible = true;
                btnupdate.Visible = true;
                lbl_error.Visible = false;
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
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            FpSpread2.Sheets[0].Columns.Count = 6;

            chkselall.AutoPostBack = true;
            chksel.AutoPostBack = false;

            FpSpread2.Sheets[0].FrozenRowCount = 1;

            string selectQuery = d2.GetFunction(" select deductions  from incentives_master where college_code ='" + collegecode1 + "'");
            if (selectQuery != "" && selectQuery != "0")
            {
                string[] split1 = selectQuery.Split(';');
                FpSpread2.Sheets[0].Rows.Count = 0;

                string a = "";
                string b1 = "";
                string b2 = "";
                string b3 = "";
                string b4 = "";
                FpSpread2.Sheets[0].Rows.Count++;
                FpSpread2.Sheets[0].Cells[0, 1].CellType = chkselall;
                FpSpread2.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";

                for (int i = 0; i < split1.Length; i++)
                {
                    a = split1[i];

                    if (a != "" && a != null)
                    {
                        string[] split2 = a.Split('\\');
                        if (split2.Length >= 2)
                        {
                            b1 = split2[0];
                            b2 = split2[1];
                            if (split2.Length > 2)
                            {
                                b3 = split2[2];
                            }
                            if (split2.Length > 3)
                            {
                                b4 = split2[3];
                            }
                        }
                        FpSpread2.Sheets[0].Rows.Count++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].CellType = chksel;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = b1;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 3].Text = b2;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 4].Font.Name = "Book Antiqua";
                        if (b3 == "PF")
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 4].Text = "PF";
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 4].Text = "";
                        }
                        if (b4 == "1")
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 5].Text = "Net Amount";
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 5].Text = "";
                        }
                    }
                }

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].Columns[0].Locked = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Deductions";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[2].Locked = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Descriptions";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[3].Locked = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Deduction From PF";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[4].Locked = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Auto Deduction";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[5].Locked = true;

                FpSpread2.Sheets[0].Columns[0].Width = 75;
                FpSpread2.Sheets[0].Columns[1].Width = 75;
                FpSpread2.Sheets[0].Columns[2].Width = 215;
                FpSpread2.Sheets[0].Columns[3].Width = 185;
                FpSpread2.Sheets[0].Columns[4].Width = 100;
                FpSpread2.Sheets[0].Columns[5].Width = 100;

                FpSpread2.SaveChanges();
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Width = 755;
                FpSpread2.Height = 500;
                FpSpread2.Visible = true;
                rptprint.Visible = true;
                btndel.Visible = true;
                btnupdate.Visible = true;
                lbl_error.Visible = false;
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
            int save = 0;
            string allowid = "";
            string dedid = "";
            string name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_name.Text.ToString());
            string des = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_desc.Text.ToString());
            string acr = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_allacr.Text.ToString());

            string actrow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actrow) != -1)
            {
                allowid = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
            }

            string actdedrow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(actdedrow) != -1)
            {
                dedid = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(actdedrow), 3].Tag);
            }

            int spall = 0;
            string dedpf = "";
            int itcal=0;
            int dedauto = 0;
            if (cb_splallow.Checked == true)
            {
                spall = 1;
            }
            else
            {
                spall = 0;
            }
            if (rb_allow.Checked == true)
            {
                itcal = 1; 
            }
            if (rb_deduct.Checked == true)
            {
                itcal = 2;
            }

            if (cb_pfdeduct.Checked == true)
            {
                dedpf = "PF";
            }
            if (cb_autodeduct.Checked == true)
            {
                dedauto = 1;
            }
            if (btn_save.Text.ToLower().Trim() == "save")
            {
                collcode = collegecode;
                if (lbl_newdesg.Text == "Allowance/Addition")
                {
                    string oldsplall = d2.GetFunction("select SplAllowCount from incentives_master where college_code='" + collegecode + "'");
                    string oldallown = d2.GetFunction("select allowances from incentives_master where college_code='" + collegecode + "'");
                    if (oldallown.Trim() == "" || oldallown.Trim() == "0")
                    {
                        oldallown = name + "\\" + spall + "\\" + des;
                    }
                    else
                    {
                        oldallown = oldallown + ";" + name + "\\" + spall + "\\" + des;
                    }
                    string itcalcallow = oldallown + itcal;
                    int splcount = 0;
                    if (Int32.TryParse(oldsplall, out splcount))
                    {
                        splcount = Convert.ToInt32(oldsplall);
                    }
                    if (cb_splallow.Checked == true)
                    {
                        splcount++;
                    }
                    string updqry = string.Empty;
                      updqry  = " if exists(select * from incentives_master where college_code='" + collegecode + "') update incentives_master set allowances='" + oldallown + "', SplAllowCount='" + splcount + "' where college_code='" + collegecode + "' else insert into incentives_master (allowances,college_code,SplAllowCount) values ('" + oldallown + "','" + collegecode + "','" + splcount + "')";
                      if (cb_ItCalcAllow.Checked == true)
                      {
                          updqry += " if exists(select * from ITcalculationAllowanceDeduction where Name='" + name + "' and collegeCode='" + collegecode + "') update ITcalculationAllowanceDeduction set Type='" + itcal + "',Description='" + des + "' where Name='" + name + "' and collegeCode='" + collegecode + "' else insert into ITcalculationAllowanceDeduction (Name,Description,Type,collegeCode) values ('" + name + "','" + des + "','" + itcal + "','" + collegecode + "')";

                      }
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Deduction")
                {
                    string oldall = d2.GetFunction("select deductions from incentives_master where college_code='" + collegecode + "'");
                    if (oldall.Trim() == "" || oldall.Trim() == "0")
                    {
                        oldall = name + "\\" + des + "\\" + dedpf + "\\" + dedauto;
                    }
                    else
                    {
                        oldall = oldall + ";" + name + "\\" + des + "\\" + dedpf + "\\" + dedauto;
                    }

                    string insqry = " if exists (select * from incentives_master where college_code='" + collegecode + "') update incentives_master set deductions='" + oldall + "' where  college_code='" + collegecode + "' else insert into incentives_master (deductions,college_code) values ('" + oldall + "','" + collegecode + "')";

                    if (cb_ItCalcDeduc.Checked == true)
                    {
                        insqry += "if exists(select * from ITcalculationAllowanceDeduction where Name='" + name + "' and collegeCode='" + collegecode + "') update ITcalculationAllowanceDeduction set Type='" + itcal + "',Description='" + des + "' where Name='" + name + "' and collegeCode='" + collegecode + "' else insert into ITcalculationAllowanceDeduction (Name,Description,Type,collegeCode) values ('" + name + "','" + des + "','" + itcal + "','" + collegecode + "')";
                    }
                    int qry = d2.update_method_wo_parameter(insqry, "Text");

                    int inscount = d2.update_method_wo_parameter(insqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Saved  Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
            }
            if (btn_save.Text.ToLower().Trim() == "update")
            {
                collcode = collegecode1;
                string[] newspl = new string[2];
                if (lbl_newdesg.Text == "Allowance/Addition")
                {
                    string newname = "";
                    string newdes = "";
                    string newacr = "";
                    string newall = "";

                    string oldallown = d2.GetFunction("select allowances from incentives_master where college_code='" + collegecode1 + "'");
                    if (oldallown.Trim() != "" && oldallown.Trim() != "0")
                    {
                        string[] splval = oldallown.Split(';');
                        if (splval.Length > 0)
                        {
                            for (int ik = 0; ik < splval.Length; ik++)
                            {
                                newspl = splval[ik].Split('\\');
                                if (newspl.Length > 1)
                                {
                                    newname = Convert.ToString(newspl[0]);
                                    newdes = Convert.ToString(newspl[2]);
                                    newacr = Convert.ToString(newspl[1]);
                                    if (newname.Trim() == name.Trim())
                                    {
                                        if (newall.Trim() == "")
                                        {
                                            newall = name + "\\" + spall + "\\" + des;
                                        }
                                        else
                                        {
                                            newall = newall + ";" + name + "\\" + spall + "\\" + des;
                                        }
                                    }
                                    else
                                    {
                                        if (newall.Trim() == "")
                                        {
                                            newall = newname + "\\" + newacr + "\\" + newdes;
                                        }
                                        else
                                        {
                                            newall = newall + ";" + newname + "\\" + newacr + "\\" + newdes;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    string updqry = " update incentives_master set allowances='" + newall + "' where college_code='" + collegecode1 + "'";
                    if (cb_ItCalcAllow.Checked == true)
                    {
                        updqry += "if exists(select * from ITcalculationAllowanceDeduction where Name='" + name + "' and collegeCode='" + collegecode + "') update ITcalculationAllowanceDeduction set Type='" + itcal + "',Description='" + des + "' where Name='" + name + "' and collegeCode='" + collegecode + "' else insert into ITcalculationAllowanceDeduction (Name,Description,Type,collegeCode) values ('" + name + "','" + des + "','" + itcal + "','" + collegecode + "')";
                    }
                    int inscount = d2.update_method_wo_parameter(updqry, "Text");
                    if (inscount > 0)
                    {
                        lbl_alert.Text = "Updated Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                    }
                }
                else if (lbl_newdesg.Text == "Deduction")
                {
                    string newname = "";
                    string newdes = "";
                    string newacr = "";
                    string newded = "";
                    string newdedauto = "";

                    string oldall = d2.GetFunction("select deductions from incentives_master where college_code='" + collegecode1 + "'");
                    if (oldall.Trim() != "" && oldall.Trim() != "0")
                    {
                        string[] splval = oldall.Split(';');
                        if (splval.Length > 0)
                        {
                            for (int ik = 0; ik < splval.Length; ik++)
                            {
                                newspl = splval[ik].Split('\\');
                                if (newspl.Length > 1)
                                {
                                    newname = Convert.ToString(newspl[0]);
                                    newacr = Convert.ToString(newspl[1]);
                                    if (newspl.Length >= 3)
                                    {
                                        newdes = Convert.ToString(newspl[2]);
                                    }
                                    if (newspl.Length >= 4)
                                    {
                                        newdedauto = Convert.ToString(newspl[3]);
                                    }
                                    if (newname.Trim() == name.Trim())
                                    {
                                        if (newded.Trim() == "")
                                        {
                                            newded = name + "\\" + des + "\\" + dedpf + "\\" + dedauto;
                                        }
                                        else
                                        {
                                            newded = newded + ";" + name + "\\" + des + "\\" + dedpf + "\\" + dedauto;
                                        }
                                    }
                                    else
                                    {
                                        if (newded.Trim() == "")
                                        {
                                            newded = newname + "\\" + newacr + "\\" + newdes + "\\" + newdedauto;
                                        }
                                        else
                                        {
                                            newded = newded + ";" + newname + "\\" + newacr + "\\" + newdes + "\\" + newdedauto;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    string insqry = " update incentives_master set deductions='" + newded + "' where college_code='" + collegecode1 + "'";
                    if (cb_ItCalcDeduc.Checked == true)
                    {
                        insqry += "if exists(select * from ITcalculationAllowanceDeduction where Name='" + name + "' and collegeCode='" + collegecode + "') update ITcalculationAllowanceDeduction set Type='" + itcal + "',Description='" + des + "' where Name='" + name + "' and collegeCode='" + collegecode + "' else insert into ITcalculationAllowanceDeduction (Name,Description,Type,collegeCode) values ('" + name + "','" + des + "','" + itcal + "','" + collegecode + "')";
                    }
                    int qry = d2.update_method_wo_parameter(insqry, "Text");
                    int inscount = d2.update_method_wo_parameter(insqry, "Text");
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
            d2.sendErrorMail(ex, collcode, "AllowanceAndDeductionMaster_Alter.aspx");
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
                cb_pfdeduct.Checked = false;
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
        cb_pfdeduct.Enabled = false;
        cb_autodeduct.Enabled = false;
        cb_splallow.Enabled = true;
        cb_pfdeduct.Checked = false;
        cb_autodeduct.Checked = false;
        txt_name.Text = "";
        txt_desc.Text = "";
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        lbl_error.Visible = false;
        rptprint.Visible = false;
    }

    public void rb_deduct_OncheckedChanged(object sender, EventArgs e)
    {
        cb_splallow.Enabled = false;
        cb_pfdeduct.Enabled = true;
        cb_autodeduct.Enabled = true;
        cb_splallow.Checked = false;
        txt_name.Text = "";
        txt_desc.Text = "";
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        btnupdate.Visible = false;
        btndel.Visible = false;
        lbl_error.Visible = false;
        rptprint.Visible = false;
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
            int count = 0;
            ddl_popclg.SelectedIndex = ddl_popclg.Items.IndexOf(ddl_popclg.Items.FindByValue(ddl_college.SelectedItem.Value));
            collegecode = Convert.ToString(ddl_popclg.SelectedItem.Value);
            autocol = Convert.ToString(ddl_popclg.SelectedItem.Value);
            ddl_popclg.Enabled = false;
            if (FpSpread1.Visible == true)
            {
                if (rb_allow.Checked == true)
                {
                    if (checkedOK(FpSpread1, out count))
                    {
                        if (count == 1)
                        {
                            lbl_error.Visible = false;
                            addnew.Visible = true;
                            cb_splallow.Visible = true;
                            cb_ItCalcAllow.Visible = true;
                            cb_ItCalcDeduc.Visible = false;
                            cb_pfdeduct.Visible = false;
                            cb_autodeduct.Visible = false;
                            FpSpread1.SaveChanges();
                            for (int ik = 0; ik < FpSpread1.Sheets[0].Rows.Count; ik++)
                            {
                                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[ik, 1].Value);
                                if (check == 1)
                                {
                                    txt_name.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 2].Text);
                                    txt_desc.Text = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 3].Text);
                                    string getval = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 4].Text);
                                    if (getval.Trim() == "Yes")
                                    {
                                        cb_splallow.Checked = true;
                                    }
                                    else
                                    {
                                        cb_splallow.Checked = false;
                                    }
                                }
                            }
                            txt_name.Enabled = false;
                            btn_save.Text = "Update";
                            lbl_newdesg.Text = "Allowance/Addition";
                        }
                        else
                        {
                            lbl_error.Visible = true;
                            lbl_error.Text = "Please Select Any one Allowance/Addition to Update!";
                        }
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Any one Allowance/Addition to Update!";
                    }
                }
                else
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Click Go button & then Proceed!";
                }
            }
            if (FpSpread2.Visible == true)
            {
                if (rb_deduct.Checked == true)
                {
                    if (checkedOK(FpSpread2, out count))
                    {
                        if (count == 1)
                        {
                            addnew.Visible = true;
                            cb_pfdeduct.Visible = true;
                            cb_autodeduct.Visible = true;
                            cb_ItCalcDeduc.Visible = true;
                            cb_splallow.Visible = false;
                            cb_ItCalcAllow.Visible = false;
                            FpSpread2.SaveChanges();
                            for (int ik = 0; ik < FpSpread2.Sheets[0].Rows.Count; ik++)
                            {
                                byte check = Convert.ToByte(FpSpread2.Sheets[0].Cells[ik, 1].Value);
                                if (check == 1)
                                {
                                    txt_name.Text = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 2].Text);
                                    txt_desc.Text = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 3].Text);
                                    string getval = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 4].Text);
                                    string getauto = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 5].Text);
                                    if (getval.Trim() == "PF")
                                    {
                                        cb_pfdeduct.Checked = true;
                                    }
                                    else
                                    {
                                        cb_pfdeduct.Checked = false;
                                    }
                                    if (getauto.Trim() == "Net Amount")
                                    {
                                        cb_autodeduct.Checked = true;
                                    }
                                    else
                                    {
                                        cb_autodeduct.Checked = false;
                                    }
                                }
                            }
                            txt_name.Enabled = false;
                            btn_save.Text = "Update";
                            lbl_newdesg.Text = "Deduction";
                            lbl_error.Visible = false;
                        }
                        else
                        {
                            lbl_error.Visible = true;
                            lbl_error.Text = "Please Select Any one Deduction to Update!";
                        }
                    }
                    else
                    {
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select Any one Deduction to Update!";
                    }
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
            Dictionary<string, string> dicall = new Dictionary<string, string>();
            Dictionary<string, string> dicded = new Dictionary<string, string>();
            dicall.Clear();
            dicded.Clear();
            if (rb_allow.Checked == true)
            {
                if (checkedOK(FpSpread1, out count))
                {
                    string getall = "";

                    FpSpread1.SaveChanges();
                    for (int ik = 1; ik < FpSpread1.Sheets[0].Rows.Count; ik++)
                    {
                        byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[ik, 1].Value);
                        if (check == 1)
                        {
                            string newb1 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 2].Text);
                            string newb3 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 3].Text);
                            string newb2 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 4].Text);
                            if (!dicall.ContainsKey(newb1))
                            {
                                dicall.Add(newb1, newb3);
                            }
                        }
                    }

                    FpSpread1.SaveChanges();
                    for (int ik = 1; ik < FpSpread1.Sheets[0].Rows.Count; ik++)
                    {
                        string newb1 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 2].Text);
                        string newb3 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 3].Text);
                        string newb2 = Convert.ToString(FpSpread1.Sheets[0].Cells[ik, 4].Text);
                        if (dicall.ContainsKey(newb1))
                        {

                        }
                        else
                        {
                            if (getall.Trim() == "")
                            {
                                getall = Convert.ToString(newb1) + "\\" + Convert.ToString(newb2) + "\\" + Convert.ToString(newb3);
                            }
                            else
                            {
                                getall = getall + ";" + Convert.ToString(newb1) + "\\" + Convert.ToString(newb2) + "\\" + Convert.ToString(newb3);
                            }
                        }
                    }

                    string updqry = "";
                    updqry = "update incentives_master set allowances='" + getall + "' where college_code='" + collegecode + "'";
                    int qry = d2.update_method_wo_parameter(updqry, "Text");
                    if (qry > 0)
                    {
                        lbl_alert.Text = "Deleted Successfully";
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
                    lbl_alert.Text = "Please Select any one Allowance/Addition!";
                }
            }
            if (rb_deduct.Checked == true)
            {
                if (checkedOK(FpSpread2, out count))
                {
                    string getded = "";
                    string newval = "";
                    string newautoded = "";
                    FpSpread2.SaveChanges();
                    for (int ik = 1; ik < FpSpread2.Sheets[0].Rows.Count; ik++)
                    {
                        byte check = Convert.ToByte(FpSpread2.Sheets[0].Cells[ik, 1].Value);
                        if (check == 1)
                        {
                            string newb1 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 2].Text);
                            string newb2 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 3].Text);
                            string newb3 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 4].Text);
                            if (!dicded.ContainsKey(newb1))
                            {
                                dicded.Add(newb1, newb2);
                            }
                        }
                    }

                    FpSpread2.SaveChanges();
                    for (int ik = 1; ik < FpSpread2.Sheets[0].Rows.Count; ik++)
                    {
                        string newb1 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 2].Text);
                        string newb2 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 3].Text);
                        string newb3 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 4].Text);
                        string newb4 = Convert.ToString(FpSpread2.Sheets[0].Cells[ik, 5].Text);

                        if (newb3 == "PF")
                        {
                            newval = "PF";
                        }
                        else
                        {
                            newval = "";
                        }

                        if (newb4 == "Net Amount")
                        {
                            newautoded = "1";
                        }
                        else
                        {
                            newautoded = "";
                        }

                        if (dicded.ContainsKey(newb1))
                        {

                        }
                        else
                        {
                            if (getded.Trim() == "")
                            {
                                getded = Convert.ToString(newb1) + "\\" + Convert.ToString(newb2) + "\\" + Convert.ToString(newval) + "\\" + Convert.ToString(newautoded);
                            }
                            else
                            {
                                getded = getded + ";" + Convert.ToString(newb1) + "\\" + Convert.ToString(newb2) + "\\" + Convert.ToString(newval) + "\\" + Convert.ToString(newautoded);
                            }
                        }
                    }
                    string updqry = "";
                    updqry = "update incentives_master set deductions='" + getded + "' where college_code='" + collegecode + "'";
                    int qry = d2.update_method_wo_parameter(updqry, "Text");
                    if (qry > 0)
                    {
                        lbl_alert.Text = "Deleted  Successfully";
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
                    lbl_alert.Text = "Please Select any one Deduction!";
                }
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
}
