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
using System.Web.Services;
using System.Drawing;
using AjaxControlToolkit;
public partial class Pay_Process : System.Web.UI.Page
{
    static string clgcode1 = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string dtfromdate = string.Empty;
    string dt1todate = string.Empty;
    string m = string.Empty;
    DataSet ds = new DataSet();
    DataSet ss = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet dsYr = new DataSet();
    bool genchk = false;
    bool check = false;
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    DateTime joindate = new DateTime();
    DataSet dnew = new DataSet();
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();
    int days = 0;
    string hoscode = "";
    string clgcode = "";
    string hoscode1 = "";
    string messcode = "";
    string monthvalue = "";
    bool isbate = false;
    string lblgetscode = "";
    int missedcount = 0;
    int gencount = 0;
    int deductionval = 0;//delsi
    Hashtable rebetedays_hash = new Hashtable();
    Hashtable rebeteamt_hash = new Hashtable();
    Hashtable grantday_hash = new Hashtable();
    Hashtable grantamt_hash = new Hashtable();
    Hashtable guestrebetedays_hash = new Hashtable();
    Hashtable guestgrant_hash = new Hashtable();
    Hashtable hat = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //clgcode1 = Session["collegecode"].ToString();
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (clgcode1 == "")
            {
                if (ddlcollege.Items.Count > 0)
                    clgcode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            if (!IsPostBack)
            {
                int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                //bindyear();
                binddept();
                designation();
                binddesig();
                ViewState["isdivident"] = null;
                rdb_indivual1.Checked = true;
                Fpspread1.Visible = false;
                fpspreadshow.Visible = false;
                Session["year"] = null;
                Session["reb_amt"] = null;
                Session["reb_days"] = null;
                Session["grantamt"] = null;
                Session["guestrebateamt"] = null;
                Session["guestrebateday"] = null;
                Session["guestgrantamout"] = null;
                Session["guestrebateday"] = null;
                Session["reb_days"] = null;
                bindsearchstapp();
                txt_StaffCode.Visible = false;
                txt_staffname.Visible = false;
                btn_go_Click(sender, e);
            }
            lblshowerr.Visible = false;
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                clgcode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        //bindyear();
        Fpspread1.Visible = false;
        lbl_error.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        try
        {
            Div2.Visible = false;
            Fpspread1.Visible = false;
            lbl_error.Visible = false;
            DataSet HrpayMonDS = d2.select_method_wo_parameter("select PayMonthNum,PayYear,PayMonth from hrpaymonths where college_code='" + collegecode1 + "' and SelStatus='1'", "text");
            if (HrpayMonDS.Tables != null)
            {
                if (HrpayMonDS.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = false;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 5;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;
                    Fpspread1.Columns[0].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[1].Width = 100;
                    Fpspread1.Columns[1].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Month";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 100;
                    Fpspread1.Columns[2].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Generate";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[3].Width = 200;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Show Results";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[4].Width = 200;
                    FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                    FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                    FarPoint.Web.Spread.ButtonCellType btnType2 = new FarPoint.Web.Spread.ButtonCellType();
                    btnType.CssClass = "btn textbox";
                    btnType1.CssClass = "btn textbox";
                    btnType2.CssClass = "btn textbox";
                    int row = 1;
                    foreach (DataRow dr in HrpayMonDS.Tables[0].Rows)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dr["PayYear"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dr["PayMonthNum"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["PayYear"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = returnMonYear(Convert.ToString(dr["PayMonthNum"])); //Convert.ToString(dr["PayMonth"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["PayMonthNum"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        string selq = "select * from monthlypay where PayMonth='" + Convert.ToString(dr["PayMonthNum"]) + "' and PayYear='" + Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text) + "' and college_code='" + collegecode1 + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType1;
                                btnType1.Text = "Regenerate";
                                btnType1.ForeColor = Color.Red;
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType;
                                btnType.Text = "Generate";
                            }
                        }
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType2;
                        btnType2.Text = "Show Results";
                    }
                    Fpspread1.Visible = true;
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Columns[1].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Columns[2].VerticalAlign = VerticalAlign.Middle;
                }
            }
            /*
            if (ddl_fromyr.SelectedItem.Text.Trim() != "Select" && ddl_toyr.SelectedItem.Text.Trim() != "Select")
            {
                int ddlfromdate = Convert.ToInt32(ddl_fromyr.SelectedItem.Text);
                int ddltodate = Convert.ToInt32(ddl_toyr.SelectedItem.Text);
                if (ddlfromdate <= ddltodate)
                {
                    string fromyr = "";
                    fromyr = ddl_fromyr.SelectedItem.Value.ToString();
                    string toyr = "";
                    toyr = ddl_toyr.SelectedItem.Value.ToString();
                    int yr1 = Convert.ToInt32(fromyr);
                    int yr2 = Convert.ToInt32(toyr);
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = false;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 5;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;
                    Fpspread1.Columns[0].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[1].Width = 100;
                    Fpspread1.Columns[1].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Month";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 100;
                    Fpspread1.Columns[2].Locked = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Generate";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[3].Width = 200;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Show Results";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[4].Width = 200;
                    FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                    FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                    FarPoint.Web.Spread.ButtonCellType btnType2 = new FarPoint.Web.Spread.ButtonCellType();
                    int row = 1;
                    while (yr1 <= yr2)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(yr1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(i);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(yr1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = returnMonYear(Convert.ToString(i));
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(i);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            string selq = "select * from monthlypay where PayMonth='" + Convert.ToString(i) + "' and PayYear='" + Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text) + "' and college_code='" + collegecode1 + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType1;
                                    btnType1.Text = "Regenerate";
                                    btnType1.ForeColor = Color.Red;
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType;
                                    btnType.Text = "Generate";
                                }
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType2;
                            btnType2.Text = "Show Results";
                        }
                        yr1 = yr1 + 1;
                    }
                    Fpspread1.Visible = true;
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Columns[1].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Columns[2].VerticalAlign = VerticalAlign.Middle;
                }
                else
                {
                    Div2.Visible = false;
                    Fpspread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please select to year greater then from year";
                }
            }
            else
            {
                Div2.Visible = false;
                Fpspread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please select from year and to year";
            }
            */
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx"); }
    }
    protected void imagebtnpopclose_addnew_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void btn_addnew_exit_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
        clear();
        btn_go_Click(sender, e);
    }
    public string returnMonYear(string numeral)
    {
        string monthyear = String.Empty;
        switch (numeral)
        {
            case "1":
                monthyear = "Jan";
                break;
            case "2":
                monthyear = "Feb";
                break;
            case "3":
                monthyear = "Mar";
                break;
            case "4":
                monthyear = "Apr";
                break;
            case "5":
                monthyear = "May";
                break;
            case "6":
                monthyear = "Jun";
                break;
            case "7":
                monthyear = "Jul";
                break;
            case "8":
                monthyear = "Aug";
                break;
            case "9":
                monthyear = "Sep";
                break;
            case "10":
                monthyear = "Oct";
                break;
            case "11":
                monthyear = "Nov";
                break;
            case "12":
                monthyear = "Dec";
                break;
        }
        return monthyear;
    }
    protected void rdb_common1_CheckedChange(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
    }
    protected void rdb_rdb_indivual1_CheckedChange(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
    }
    public void loadsettings(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            DataSet dschk = new DataSet();
            string year = "";
            string month1 = "";
            string mon = "";
            string[] splamnt = new string[2];
            year = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
            month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
            mon = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
            lbl_showmonyear.Text = "Pay Process for " + mon + "," + year;
            string selqchk = "select * from monthlypay where college_code='" + collegecode1 + "' and PayMonth='" + month1 + "' and PayYear='" + year + "'";
            dschk.Clear();
            dschk = d2.select_method_wo_parameter(selqchk, "Text");
            if (dschk.Tables.Count > 0)
            {
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    Div2.Visible = true;
                    string selq = "select * from HR_PaySettings where College_Code='" + collegecode1 + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsAttnLOP"]).ToUpper() == "TRUE")
                                cb_lopfrom_atn.Checked = true;
                            else
                                cb_lopfrom_atn.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["LOPBasic"]).ToUpper() == "TRUE")
                                cb_Lopfrom_basic.Checked = true;
                            else
                                cb_Lopfrom_basic.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["LOPPB"]).ToUpper() == "TRUE")
                                cb_Lopfrom_payband.Checked = true;
                            else
                                cb_Lopfrom_payband.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["LOPGP"]).ToUpper() == "TRUE")
                                cb_lopgradpay.Checked = true;
                            else
                                cb_lopgradpay.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxType"]) == "1")
                                rdb_month.Checked = true;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxType"]) == "2")
                            {
                                rdb_Days.Checked = false;
                                txt_totaldays.Enabled = true;
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]).Trim() != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]).Trim() != "0")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]).Trim().Contains('.'))
                                {
                                    splamnt = Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]).Trim().Split('.');
                                    txt_totaldays.Text = Convert.ToString(splamnt[0]);
                                }
                                else
                                    txt_totaldays.Text = Convert.ToString(ds.Tables[0].Rows[0]["SalCalMaxDays"]);
                            }
                            else
                                txt_totaldays.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsAbsCal"]).ToUpper() == "TRUE")
                            {
                                cb_absentcalculation.Checked = true;
                                txt_absent.Enabled = true;
                            }
                            else
                                cb_absentcalculation.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]) != "0")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]).Trim().Contains('.'))
                                {
                                    splamnt = Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]).Trim().Split('.');
                                    txt_absent.Text = Convert.ToString(splamnt[0]);
                                }
                                else
                                    txt_absent.Text = Convert.ToString(ds.Tables[0].Rows[0]["AbsCalPer"]);
                            }
                            else
                                txt_absent.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsMaxPer"]).ToUpper() == "TRUE")
                            {
                                cb_max_PER.Checked = true;
                                txt_after.Enabled = true;
                            }
                            else
                                cb_max_PER.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]) != "0")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]).Trim().Contains('.'))
                                {
                                    splamnt = Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]).Trim().Split('.');
                                    txt_after.Text = Convert.ToString(splamnt[0]);
                                }
                                else
                                    txt_after.Text = Convert.ToString(ds.Tables[0].Rows[0]["MaxPerVal"]);
                            }
                            else
                                txt_after.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsPFLopDays"]).ToUpper() == "TRUE")
                            {
                                cb_LOP_to_PF.Checked = true;
                                txt_days.Enabled = true;
                            }
                            else
                                cb_LOP_to_PF.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]) != "0")
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]).Trim().Contains('.'))
                                {
                                    splamnt = Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]).Trim().Split('.');
                                    txt_days.Text = Convert.ToString(splamnt[0]);
                                }
                                else
                                    txt_days.Text = Convert.ToString(ds.Tables[0].Rows[0]["PFLopDays"]);
                            }
                            else
                                txt_days.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["FPFPer"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["FPFPer"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["FPFPer"]) != "0")
                            {
                                txt_fpf.Enabled = true;
                                txt_fpf.Text = Convert.ToString(ds.Tables[0].Rows[0]["FPFPer"]);
                            }
                            else
                                txt_fpf.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["FPFAge"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["FPFAge"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["FPFAge"]) != "0")
                            {
                                txt_age_val.Enabled = true;
                                txt_age_val.Text = Convert.ToString(ds.Tables[0].Rows[0]["FPFAge"]);
                            }
                            else
                                txt_age_val.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["FPFMaxAmt"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["FPFMaxAmt"]) != "0.00" && Convert.ToString(ds.Tables[0].Rows[0]["FPFMaxAmt"]) != "0")
                            {
                                txt_max_amount.Enabled = true;
                                txt_max_amount.Text = Convert.ToString(ds.Tables[0].Rows[0]["FPFMaxAmt"]);
                            }
                            else
                                txt_max_amount.Text = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsMulMaxPer"]).ToUpper() == "TRUE")
                            {
                                cb_formulate_days.Checked = true;
                                cb_formulate_days.Enabled = true;
                                if (Convert.ToString(ds.Tables[0].Rows[0]["PerLOPDet"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["PerLOPDet"]).Trim() != "0")
                                {
                                    DataTable dtget = new DataTable();
                                    dtget.Columns.Add("From");
                                    dtget.Columns.Add("To");
                                    dtget.Columns.Add("LOPDays");
                                    DataRow dr;
                                    string[] spl = Convert.ToString(ds.Tables[0].Rows[0]["PerLOPDet"]).Trim().Split('\\');
                                    if (spl.Length > 0)
                                    {
                                        for (int ik = 0; ik < spl.Length; ik++)
                                        {
                                            string[] newspl = spl[ik].Split(';');
                                            if (newspl.Length >= 3)
                                            {
                                                dr = dtget.NewRow();
                                                dr[0] = Convert.ToString(newspl[0]);
                                                dr[1] = Convert.ToString(newspl[1]);
                                                dr[2] = Convert.ToString(newspl[2]);
                                                dtget.Rows.Add(dr);
                                            }
                                        }
                                    }
                                    grid_multiple_days.DataSource = dtget;
                                    grid_multiple_days.DataBind();
                                    grid_multiple_days.Visible = true;
                                }
                            }
                            else
                            {
                                cb_formulate_days.Checked = false;
                                grid_multiple_days.Visible = false;
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[0]["AttCurForNA"]).ToUpper() == "TRUE")
                                cb_NA_RL.Checked = true;
                            else
                                cb_NA_RL.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsAutoDeduct"]).ToUpper() == "TRUE")
                                cb_auto_deduct.Checked = true;
                            else
                                cb_auto_deduct.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["AttCurForUPL"]).ToUpper() == "TRUE")
                                cb_unpaid_leave.Checked = true;
                            else
                                cb_unpaid_leave.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["LOPGross"]).ToUpper() == "TRUE")
                                cb_lop_fromgross.Checked = true;
                            else
                                cb_lop_fromgross.Checked = false;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsHourWise"]).ToUpper() == "TRUE")
                            {
                                ChkHourWise.Checked = true;
                                lblStfCat.Visible = true;
                                UpdatePanel1.Visible = true;
                                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Category_code"])))
                                {
                                    string[] splCat = Convert.ToString(ds.Tables[0].Rows[0]["Category_code"]).Split(',');
                                    if (splCat.Length > 0)
                                    {
                                        for (int sp = 0; sp < splCat.Length; sp++)
                                        {
                                            for (int sel = 0; sel < cblHrStfCat.Items.Count; sel++)
                                            {
                                                if (splCat[sp].Trim() == cblHrStfCat.Items[sp].Value.Trim())
                                                    cblHrStfCat.Items[sel].Selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ChkHourWise.Checked = false;
                                lblStfCat.Visible = false;
                                UpdatePanel1.Visible = false;
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[0]["Is_CLCalc"]).ToUpper() == "TRUE")
                            {
                                chk_CLCalc.Checked = true;
                                chk_CLCalc_Change(sender, e);
                            }
                            else
                            {
                                chk_CLCalc.Checked = false;
                                chk_CLCalc_Change(sender, e);
                            }
                            DataTable dtCol = new DataTable();
                            dtCol.Columns.Add("");
                            dtCol.Columns.Add("LateDays");
                            dtCol.Columns.Add("LOPDays");
                            DataRow drLate;
                            string LeaveSet = string.Empty;
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Per_Days"])))
                                txtPer.Text = Convert.ToString(ds.Tables[0].Rows[0]["Per_Days"]);
                            if (Convert.ToString(ds.Tables[0].Rows[0]["Per_Leave"]).Trim() != "Select")
                                ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["Per_Leave"])));
                            else
                                ddlLeaveType.SelectedIndex = 0;
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Abs_Days_Calc"])))
                                txtAbs_Days.Text = Convert.ToString(ds.Tables[0].Rows[0]["Abs_Days_Calc"]);
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Inc_AllLOP"])) && Convert.ToString(ds.Tables[0].Rows[0]["Inc_AllLOP"]).Trim().ToUpper() == "TRUE")
                                chkIncAllLop.Checked = true;
                            else
                                chkIncAllLop.Checked = false;
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Late_Set"])))
                            {
                                string[] LeaveDet = Convert.ToString(ds.Tables[0].Rows[0]["Late_Set"]).Split('\\');
                                if (LeaveDet.Length > 0)
                                {
                                    for (int ik = 0; ik < LeaveDet.Length; ik++)
                                    {
                                        if (!String.IsNullOrEmpty(LeaveDet[ik]))
                                        {
                                            string[] SplVal = Convert.ToString(LeaveDet[ik]).Split(';');
                                            if (SplVal.Length > 1)
                                            {
                                                drLate = dtCol.NewRow();
                                                drLate[1] = Convert.ToString(SplVal[0]);
                                                drLate[2] = Convert.ToString(SplVal[1]);
                                                dtCol.Rows.Add(drLate);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dtCol.Rows.Add("", "", "");
                            }
                            grdLateDays.DataSource = dtCol;
                            grdLateDays.DataBind();
                        }
                    }
                }
                else
                {
                    Div2.Visible = true;
                }
            }
        }
        catch { }
    }
    public void btnType_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)//del
    {
        try
        {
            txtStaffName.Text = "";
            txtStaffcode.Text = "";
            bindsearchstapp();
            divprocessclear();
            string year = "";
            string month1 = "";
            string mon = "";
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (activerow.Trim() != "" && activecol.Trim() != "" && activecol.Trim() == "3")
            {
                loadsettings(sender, e);
                tblcommon.Visible = true;
                chkdept.Checked = false;
                cbldeptcom.Items.Clear();
                txtdeptcom.Enabled = false;
                txtdeptcom.Text = "--Select--";
                chkstftype.Checked = false;
                cblstftypecom.Items.Clear();
                txtstftypecom.Enabled = false;
                txtstftypecom.Text = "--Select--";
                chkstfcat.Checked = false;
                cblscatcom.Items.Clear();
                txtscatcom.Enabled = false;
                txtscatcom.Text = "--Select--";
                cbl_desig.Items.Clear();
                txt_desig.Enabled = false;
                txt_desig.Text = "--Select--";
                //cbl_staffname.Items.Clear();
                txt_staffname.Enabled = false;
                // txt_staffname.Text = "--Select--";
                //CblStaffCode.Items.Clear();
                txt_StaffCode.Enabled = false;
                // txt_StaffCode.Text = "--Select--";
            }
            else if (activerow.Trim() != "" && activecol.Trim() != "" && activecol.Trim() == "4")
            {
                year = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                mon = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
                lblshow.Text = "Pay Process for " + mon + "," + year;
                string selq = "select * from monthlypay where PayMonth='" + month1 + "' and PayYear='" + year + "' and latestrec='1' and college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    divshow.Visible = true;
                    binddept();
                    designation();
                    bindstftype();
                    staffcategory();
                    //   BindStaffIndividual(0);
                    //  BindStaffIndividual(1);
                    lblshowerr.Visible = false;
                    fpspreadshow.Visible = false;
                    btnindgen.Visible = false;
                    btninddel.Visible = false;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Generate the Pay Process for this Month!";
                }
            }
        }
        catch { }
    }
    protected void fpspreadshow_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        fpspreadshow.SaveChanges();
        byte check = Convert.ToByte(fpspreadshow.Sheets[0].Cells[0, 1].Value);
        if (check == 1)
        {
            for (int ik = 1; ik < fpspreadshow.Sheets[0].RowCount; ik++)
            {
                fpspreadshow.Sheets[0].Cells[ik, 1].Value = 1;
            }
        }
        else
        {
            for (int ik = 1; ik < fpspreadshow.Sheets[0].RowCount; ik++)
            {
                fpspreadshow.Sheets[0].Cells[ik, 1].Value = 0;
            }
        }
    }
    public bool checkedok()
    {
        bool OK = false;
        try
        {
            fpspreadshow.SaveChanges();
            for (int ik = 1; ik < fpspreadshow.Sheets[0].RowCount; ik++)
            {
                byte check = Convert.ToByte(fpspreadshow.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                    OK = true;
            }
        }
        catch { }
        return OK;
    }
    public void divprocessclear()
    {
        txt_absent.Text = "";
        txt_absent.Enabled = false;
        txt_totaldays.Text = "";
        txt_totaldays.Enabled = false;
        txt_days.Text = "";
        txt_days.Enabled = false;
        txt_fpf.Text = "";
        txt_fpf.Enabled = false;
        txt_age_val.Text = "";
        txt_age_val.Enabled = false;
        txt_max_amount.Text = "";
        txt_max_amount.Enabled = false;
        txt_after.Text = "";
        txt_after.Enabled = false;
        cb_lopfrom_atn.Checked = false;
        cb_lop_fromgross.Checked = false;
        cb_Lopfrom_basic.Checked = false;
        cb_Lopfrom_payband.Checked = false;
        cb_lopgradpay.Checked = false;
        cb_absentcalculation.Checked = false;
        cb_NA_RL.Checked = false;
        cb_LOP_to_PF.Checked = false;
        chk_fpf.Checked = false;
        cb_auto_deduct.Checked = false;
        cb_unpaid_leave.Checked = false;
        cb_max_PER.Checked = false;
        cbincitcalc.Checked = false;
        cb_formulate_days.Checked = false;
        cb_formulate_days.Enabled = false;
        grid_multiple_days.Visible = false;
    }
    protected void chk_fpf_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_fpf.Checked)
        {
            txt_fpf.Enabled = true;
            txt_age_val.Enabled = true;
            txt_max_amount.Enabled = true;
        }
        if (chk_fpf.Checked == false)
        {
            txt_fpf.Enabled = false;
            txt_age_val.Enabled = false;
            txt_max_amount.Enabled = false;
            txt_fpf.Text = "";
            txt_age_val.Text = "";
            txt_max_amount.Text = "";
        }
    }
    protected void chkdept_change(object sender, EventArgs e)
    {
        if (chkdept.Checked == true)
        {
            binddept();
            txtdeptcom.Enabled = true;
        }
        else
        {
            cbldeptcom.Items.Clear();
            txtdeptcom.Text = "--Select--";
            txtdeptcom.Enabled = false;
        }
        designation();
    }
    protected void chkstftype_change(object sender, EventArgs e)
    {
        if (chkstftype.Checked == true)
        {
            bindstftype();
            txtstftypecom.Enabled = true;
        }
        else
        {
            cblstftypecom.Items.Clear();
            txtstftypecom.Text = "--Select--";
            txtstftypecom.Enabled = false;
        }
    }
    protected void chkstfcat_change(object sender, EventArgs e)
    {
        if (chkstfcat.Checked == true)
        {
            staffcategory();
            txtscatcom.Enabled = true;
        }
        else
        {
            cblscatcom.Items.Clear();
            txtscatcom.Text = "--Select--";
            txtscatcom.Enabled = false;
        }
    }
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
    }
    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "Staff Type");
    }
    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "Staff Type");
    }
    protected void cb_scat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_scat, cbl_scat, txt_scat, "Staff Category");
    }
    protected void cbl_scat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_scat, cbl_scat, txt_scat, "Staff Category");
    }
    protected void cbdeptcom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbdeptcom, cbldeptcom, txtdeptcom, "Department");
        if (cbDesig.Checked == true)
        {
            designation();
        }
    }
    protected void cbldeptcom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbdeptcom, cbldeptcom, txtdeptcom, "Department");
        int countval = 0;

        string depart_code = GetSelectedItemsValueAsString(cbldeptcom, out countval);
        ViewState["department"] = depart_code;//delsi1004
        if (cbDesig.Checked == true)
        {
            designation();
        }
    }
    protected void cbstftypecom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbstftypecom, cblstftypecom, txtstftypecom, "Staff Type");
    }
    protected void cblstftypecom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbstftypecom, cblstftypecom, txtstftypecom, "Staff Type");
    }
    protected void cbscatcom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbscatcom, cblscatcom, txtscatcom, "Staff Category");
    }
    protected void cblscatcom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbscatcom, cblscatcom, txtscatcom, "Staff Category");
    }
    public string monthdays(string month1, string year)
    {
        string pay_end = "";
        try
        {
            switch (month1)
            {
                case "1":
                    pay_end = "31";
                    break;
                case "2":
                    int yyear = Convert.ToInt32(year);
                    if ((yyear % 4) == 0)
                        pay_end = "29";
                    else
                        pay_end = "28";
                    break;
                case "3":
                    pay_end = "31";
                    break;
                case "4":
                    pay_end = "30";
                    break;
                case "5":
                    pay_end = "31";
                    break;
                case "6":
                    pay_end = "30";
                    break;
                case "7":
                    pay_end = "31";
                    break;
                case "8":
                    pay_end = "31";
                    break;
                case "9":
                    pay_end = "30";
                    break;
                case "10":
                    pay_end = "31";
                    break;
                case "11":
                    pay_end = "30";
                    break;
                case "12":
                    pay_end = "31";
                    break;
            }
        }
        catch { }
        return pay_end;
    }
    public DateTime getmonfrst(string strdate)
    {
        string[] date = new string[2];
        DateTime dt = new DateTime();
        try
        {
            date = strdate.Split('/');
            dt = Convert.ToDateTime(date[1] + "/" + date[0] + "/" + date[2]);
        }
        catch { }
        return dt;
    }
    public DateTime getdayfrst(string strdate)
    {
        string[] date = new string[2];
        DateTime dt = new DateTime();
        try
        {
            date = strdate.Split('/');
            dt = Convert.ToDateTime(date[1] + "/" + date[0] + "/" + date[2]);
        }
        catch { }
        return dt;
    }
    public Dictionary<string, string> getdatas(string slabfor, string amnt, string catcode)//delsi2201
    {
        string amntformat = "";
        string slabvalue = "";
        DataSet dsnewt = new DataSet();
        string collcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        Dictionary<string, string> newdic = new Dictionary<string, string>();
        newdic.Clear();
        try
        {
            string selq = "select slabvalue,slabtype from pfslabs where SlabFor='" + slabfor + "' and '" + amnt + "' between salfrom and salto and category_code='" + catcode + "' and college_code='" + collcode + "'";
            dsnewt.Clear();
            dsnewt = d2.select_method_wo_parameter(selq, "Text");
            if (dsnewt.Tables.Count > 0 && dsnewt.Tables[0].Rows.Count > 0)
            {
                amntformat = Convert.ToString(dsnewt.Tables[0].Rows[0]["slabtype"]);
                if (amntformat == "Percent")
                    slabvalue = Convert.ToString(dsnewt.Tables[0].Rows[0]["slabvalue"]);
                else if (amntformat == "Amount")
                    slabvalue = Convert.ToString(dsnewt.Tables[0].Rows[0]["slabvalue"]);
                newdic.Add(slabfor, amntformat + "-" + slabvalue);
            }
        }
        catch { }
        return newdic;
    }
    public Double lopamntcalc(string type, Double amount, Double pers)
    {
        Double newamnt = 0;
        try
        {
            if (type == "Percent")
                newamnt = (pers / 100) * amount;
            else if (type == "Amount")
                newamnt = pers;
        }
        catch { }
        return newamnt;
    }
    public string getamntformat(Dictionary<string, string> newdic, string slabfor)
    {
        string amntformat = "";
        string formatfrmdic = "";
        string[] split = new string[2];
        try
        {
            formatfrmdic = Convert.ToString(newdic[slabfor]);
            if (formatfrmdic.Trim() != "")
            {
                split = formatfrmdic.Split('-');
                if (split.Length > 0)
                    amntformat = Convert.ToString(split[0]);
            }
        }
        catch { }
        return amntformat;
    }
    public string getamnt(Dictionary<string, string> newdic, string slabfor)
    {
        string amnt = "";
        string formatfrmdic = "";
        string[] split = new string[2];
        try
        {
            formatfrmdic = Convert.ToString(newdic[slabfor]);
            if (formatfrmdic.Trim() != "")
            {
                split = formatfrmdic.Split('-');
                if (split.Length > 1)
                    amnt = Convert.ToString(split[1]);
            }
        }
        catch { }
        return amnt;
    }
    protected void imgshow_Click(object sender, EventArgs e)
    {
        divshow.Visible = false;
    }
    protected void btnshow_click(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        try
        {
            Double netamnt = 0;
            Double grandtotal_grosssalary = 0;
            Double grandtotal_totaldeduction = 0;
            Double grandtotal_netsalary = 0;
            Double netded = 0;
            Double netsal = 0;
            fpspreadshow.Visible = false;
            fpspreadshow.Sheets[0].ColumnCount = 0;
            fpspreadshow.CommandBar.Visible = false;
            fpspreadshow.RowHeader.Visible = false;
            fpspreadshow.Sheets[0].AutoPostBack = false;
            fpspreadshow.Sheets[0].ColumnHeader.RowCount = 1;
            fpspreadshow.Sheets[0].RowCount = 0;
            fpspreadshow.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            fpspreadshow.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            chkcell.AutoPostBack = false;
            fpspreadshow.Sheets[0].FrozenRowCount = 1;
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string year1 = "";
            string month1 = "";
            //string endyear = "";
            string dept = "";
            string stftype = "";
            string desig = "";
            string staffcategory = "";
            int depcount = 0;
            int desigcount = 0;
            int stfcount = 0;
            int catcount = 0;
            int staffnameCnt = 0;
            int staffcodeCnt = 0;
            dept = GetSelectedItemsValueAsString(cbl_dept, out depcount);
            desig = GetSelectedItemsValueAsString(CblDesignation, out desigcount);
            stftype = GetSelectedItemsText(cbl_stype, out stfcount);
            staffcategory = GetSelectedItemsValueAsString(cbl_scat, out catcount);
            //string staffName = GetSelectedItemsValueAsString(cblStaff_Name, out staffnameCnt);
            string staffName = Convert.ToString(txtStaffName.Text);
            // string staffCode = GetSelectedItemsValueAsString(cblStaff_Code, out staffcodeCnt);
            string staffCode = Convert.ToString(txtStaffcode.Text);
            if (activerow.Trim() != "" && activecol.Trim() != "")
            {
                year1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                //endyear = Convert.ToString(ddl_toyr.SelectedItem.Text);
                string selq = "select m.staff_code,s.staff_name,m.netadd,m.netded,m.netsal from monthlypay m,staffmaster s,staff_appl_master sa,stafftrans st,hrdept_master h,desig_master d,staffcategorizer sc where m.college_code=s.college_code and m.college_code=h.college_code and m.college_code=d.collegeCode and m.college_code=sc.college_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=sc.college_code and m.staff_code=s.staff_code and m.staff_code=st.staff_code and s.staff_code=st.staff_code and s.appl_no=sa.appl_no and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.category_code=sc.category_code and m.category_code=sc.category_code and m.category_code=st.category_code and st.latestrec='1' and ((s.resign='0' and s.settled='0') and (Discontinue='0' or Discontinue is null)) and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and m.college_code='" + collegecode1 + "'";
                if (dept.Trim() != "")
                    selq = selq + " and st.dept_code in('" + dept + "')";
                if (desig.Trim() != "")
                    selq = selq + " and st.desig_code in('" + desig + "')";
                if (stftype.Trim() != "")
                    selq = selq + " and st.stftype in('" + stftype + "')";
                if (staffcategory.Trim() != "")
                    selq = selq + " and st.category_code in('" + staffcategory + "')";
                if (!string.IsNullOrEmpty(staffCode))
                    selq = selq + " and s.staff_code in('" + staffCode + "')";//staffName
                if (!string.IsNullOrEmpty(staffName))
                    selq = selq + " and s.staff_name in('" + staffName + "')";//staffCode
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[0].Width = 50;
                    fpspreadshow.Columns[0].Locked = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[1].Width = 75;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[2].Width = 125;
                    fpspreadshow.Columns[2].Locked = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[3].Width = 225;
                    fpspreadshow.Columns[3].Locked = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Gross Salary";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[4].Width = 100;
                    fpspreadshow.Columns[4].Locked = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Deduction";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[5].Width = 100;
                    fpspreadshow.Columns[5].Locked = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Net Salary";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Columns[6].Width = 100;
                    fpspreadshow.Columns[6].Locked = true;
                    fpspreadshow.Sheets[0].RowCount++;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].Value = 0;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                    {
                        netamnt = 0;
                        netded = 0;
                        netsal = 0;
                        fpspreadshow.Sheets[0].RowCount++;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].Value = 0;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_code"]);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_name"]);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[ik]["netadd"]), out netamnt);
                        netamnt = Math.Round(netamnt, 0, MidpointRounding.AwayFromZero);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(netamnt);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[ik]["netded"]), out netded);
                        netded = Math.Round(netded, 0, MidpointRounding.AwayFromZero);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(netded);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[ik]["netsal"]), out netsal);
                        netsal = Math.Round(netsal, 0, MidpointRounding.AwayFromZero);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(netsal);
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        grandtotal_grosssalary = grandtotal_grosssalary + netamnt;
                        grandtotal_totaldeduction = grandtotal_totaldeduction + netded;
                        grandtotal_netsalary = grandtotal_netsalary + netsal;
                    }
                    fpspreadshow.Sheets[0].RowCount++;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Text = "Grand Total";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(grandtotal_grosssalary);
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(grandtotal_totaldeduction);
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(grandtotal_netsalary);
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    fpspreadshow.Sheets[0].Cells[fpspreadshow.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadshow.Sheets[0].PageSize = fpspreadshow.Sheets[0].RowCount;
                    fpspreadshow.Visible = true;
                    btnindgen.Visible = true;
                    btninddel.Visible = true;
                    lblshowerr.Visible = false;
                }
                else
                {
                    fpspreadshow.Visible = false;
                    btnindgen.Visible = false;
                    btninddel.Visible = false;
                    lblshowerr.Visible = true;
                    lblshowerr.Text = "No Records Found!";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void btnindgen_click(object sender, EventArgs e)
    {
        try
        {
            if (checkedok())
            {
                lblshowerr.Visible = false;
                Div2.Visible = true;
                loadsettings(sender, e);
                tblcommon.Visible = false;
                divshow.Visible = false;
            }
            else
            {
                lblshowerr.Visible = true;
                lblshowerr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }
    protected void btninddel_click(object sender, EventArgs e)
    {
        try
        {
            if (checkedok())
            {
                lblshowerr.Visible = false;
                Fpspread1.SaveChanges();
                string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                string year1 = "";
                string month1 = "";
                int delcount = 0;
                if (activerow.Trim() != "" && activecol.Trim() != "")
                {
                    year1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                    month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                }
                fpspreadshow.SaveChanges();
                for (int ik = 1; ik < fpspreadshow.Sheets[0].RowCount; ik++)
                {
                    byte check = Convert.ToByte(fpspreadshow.Sheets[0].Cells[ik, 1].Value);
                    if (check == 1)
                    {
                        string scode = Convert.ToString(fpspreadshow.Sheets[0].Cells[ik, 2].Text);
                        string delq = "delete from monthlypay where staff_code='" + scode + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "'";
                        int upcount = d2.update_method_wo_parameter(delq, "Text");
                        if (upcount > 0)
                            delcount++;
                    }
                }
                if (delcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully!";
                }
            }
            else
            {
                lblshowerr.Visible = true;
                lblshowerr.Text = "Please Select Any Staff!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void btnindexit_click(object sender, EventArgs e)
    {
        divshow.Visible = false;
    }
    protected void ChkHourWise_Change(object sender, EventArgs e)
    {
        if (ChkHourWise.Checked)
        {
            bindStfCat();
            loadsettings(sender, e);
            ChkHourWise.Checked = true;
            lblStfCat.Visible = true;
            UpdatePanel1.Visible = true;
        }
        else
        {
            lblStfCat.Visible = false;
            UpdatePanel1.Visible = false;
        }
    }
    protected void cbHrStfCat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbHrStfCat, cblHrStfCat, txtHrStfCat, "Staff Category");
    }
    protected void cblHrStfCat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbHrStfCat, cblHrStfCat, txtHrStfCat, "Staff Category");
    }
    private void bindStfCat()
    {
        try
        {
            DataSet dsCat = new DataSet();
            cblHrStfCat.Items.Clear();
            txtHrStfCat.Text = "--Select--";
            cbHrStfCat.Checked = false;
            string SelQ = "select category_code,category_name from staffcategorizer where college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            dsCat.Clear();
            dsCat = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsCat.Tables.Count > 0 && dsCat.Tables[0].Rows.Count > 0)
            {
                cblHrStfCat.DataSource = dsCat;
                cblHrStfCat.DataTextField = "category_name";
                cblHrStfCat.DataValueField = "category_code";
                cblHrStfCat.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void ChkdesigWise_Change(object sender, EventArgs e) /* poomalar 23.10.17*/
    {
        if (chk_desigwise.Checked == true)
            chk_staffwise.Enabled = false;
        else
            chk_staffwise.Enabled = true;
    }
    protected void ChkstaffWise_Change(object sender, EventArgs e) /* poomalar 23.10.17*/
    {
        if (chk_staffwise.Checked == true)
            chk_desigwise.Enabled = false;
        else
            chk_desigwise.Enabled = true;
    }
    protected void chk_CLCalc_Change(object sender, EventArgs e)
    {
        if (chk_CLCalc.Checked)
        {
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string Selyear1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            string Selmonth1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            if (Selmonth1.Length == 1)
                Selmonth1 = "0" + Selmonth1;
            bindFrmMonth(Selmonth1, Selyear1);
            year("", Selmonth1, Selyear1);
            year1("", Selmonth1, Selyear1);
            tdFrmYr.Visible = true;
            tdToYr.Visible = true;
        }
        else
        {
            tdFrmYr.Visible = false;
            tdToYr.Visible = false;
        }
    }
    protected void ddlFromMonth_Change(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string Selyear1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            string Selmonth1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            if (Selmonth1.Length == 1)
                Selmonth1 = "0" + Selmonth1;
            string college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlToMonth.Items.Clear();
            string str = "select PayMonth,PayMonthNum,From_Date from HrPayMonths where College_Code='" + college_code + "' and SelStatus='1' and (From_Date<='" + Selmonth1 + "/01/" + Selyear1 + "' or To_Date<='" + Selmonth1 + "/01/" + Selyear1 + "')";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string mon = ds.Tables[0].Rows[i]["PayMonth"].ToString();
                    if (ddlFromMonth.SelectedItem.Text.ToString() == GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[i]["PayMonthNum"])))
                    {
                        string date = Convert.ToString(ddlFromMonth.SelectedItem.Value);
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlToMonth.Items.Insert(count, new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[j]["PayMonthNum"])), ds.Tables[0].Rows[j]["PayMonthNum"].ToString()));
                            count++;
                        }
                        year(date, Selmonth1, Selyear1);
                    }
                }
                ddlToMonth.Items.Insert(0, "---Select---");
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlfromyear_selectchange(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string Selyear1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            string Selmonth1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            if (Selmonth1.Length == 1)
                Selmonth1 = "0" + Selmonth1;
            string college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlToYear.Items.Clear();
            string str = "select distinct year(To_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' and (From_Date<='" + Selmonth1 + "/01/" + Selyear1 + "' or To_Date<='" + Selmonth1 + "/01/" + Selyear1 + "') order by year asc";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var mon = ds.Tables[0].Rows[i]["year"].ToString();
                    if (ddlFromYear.SelectedItem.Text.ToString() == mon)
                    {
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlToYear.Items.Add(ds.Tables[0].Rows[j]["year"].ToString());
                        }
                        ddlToYear.Items.Insert(0, "Select");
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlToMonth_Change(object sender, EventArgs e)
    {
        Fpspread1.SaveChanges();
        string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
        string Selyear1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
        string Selmonth1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
        if (Selmonth1.Length == 1)
            Selmonth1 = "0" + Selmonth1;
        year1(ddlToMonth.SelectedItem.Value, Selmonth1, Selyear1);
    }
    public void year(string date, string SelMon, string SelYear)
    {
        try
        {
            ddlFromYear.Items.Clear();
            string college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            dsYr.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' and (From_Date<='" + SelMon + "/01/" + SelYear + "' or To_Date<='" + SelMon + "/01/" + SelYear + "') order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " and SelStatus='1' and (From_Date<='" + SelMon + "/01/" + SelYear + "' or To_Date<='" + SelMon + "/01/" + SelYear + "') order by year asc";
            }
            dsYr = da.select_method_wo_parameter(year, "text");
            if (dsYr.Tables[0].Rows.Count > 0)
            {
                ddlFromYear.DataSource = dsYr;
                ddlFromYear.DataTextField = "year";
                ddlFromYear.DataValueField = "year";
                ddlFromYear.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    public void year1(string date, string SelMon, string SelYear)
    {
        try
        {
            ddlToYear.Items.Clear();
            string college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            dsYr.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and SelStatus='1' and (From_Date<='" + SelMon + "/01/" + SelYear + "' or To_Date<='" + SelMon + "/01/" + SelYear + "') order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " and SelStatus='1' and (From_Date<='" + SelMon + "/01/" + SelYear + "' or To_Date<='" + SelMon + "/01/" + SelYear + "') order by year asc";
            }
            dsYr = da.select_method_wo_parameter(year, "text");
            if (dsYr.Tables[0].Rows.Count > 0)
            {
                ddlToYear.DataSource = dsYr;
                ddlToYear.DataTextField = "year";
                ddlToYear.DataValueField = "year";
                ddlToYear.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    private string GetMonTxt(int Month)
    {
        string myMon = string.Empty;
        try
        {
            switch (Month)
            {
                case 1:
                    myMon = "Jan";
                    break;
                case 2:
                    myMon = "Feb";
                    break;
                case 3:
                    myMon = "Mar";
                    break;
                case 4:
                    myMon = "Apr";
                    break;
                case 5:
                    myMon = "May";
                    break;
                case 6:
                    myMon = "June";
                    break;
                case 7:
                    myMon = "July";
                    break;
                case 8:
                    myMon = "Aug";
                    break;
                case 9:
                    myMon = "Sep";
                    break;
                case 10:
                    myMon = "Oct";
                    break;
                case 11:
                    myMon = "Nov";
                    break;
                case 12:
                    myMon = "Dec";
                    break;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
        return myMon;
    }
    private void bindFrmMonth(string Selmonth1, string Selyear1)
    {
        try
        {
            string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlFromMonth.Items.Clear();
            ddlToMonth.Items.Clear();
            string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + collegecode1 + "' and SelStatus='1' and (From_Date<='" + Selmonth1 + "/01/" + Selyear1 + "' or To_Date<='" + Selmonth1 + "/01/" + Selyear1 + "')";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int myk = 0; myk < ds.Tables[0].Rows.Count; myk++)
                {
                    ddlFromMonth.Items.Insert(myk, new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[myk]["PayMonthNum"])), Convert.ToString(ds.Tables[0].Rows[myk]["PayMonthNum"])));
                }
                ddlFromMonth.Items.Insert(0, "---Select---");
                ddlToMonth.Items.Insert(0, "---Select---");
            }
            else
            {
                ddlFromMonth.Items.Insert(0, "---Select---");
                ddlToMonth.Items.Insert(0, "---Select---");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void lbLeave_Click(object sender, EventArgs e)
    {
        divLeaveSet.Visible = true;
        txtPer.Text = "";
        txtAbs_Days.Text = "";
        chkIncAllLop.Checked = false;
        bindLeave();
        bindLeaveGrid();
    }
    protected void btnGrdExt_Click(object sender, EventArgs e)
    {
        divLeaveSet.Visible = false;
    }
    protected void imgLeaveclose_Click(object sender, EventArgs e)
    {
        divLeaveSet.Visible = false;
    }
    protected void btnAddRows_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCol = new DataTable();
            dtCol.Columns.Add("");
            dtCol.Columns.Add("LateDays");
            dtCol.Columns.Add("LOPDays");
            foreach (GridViewRow gRow in grdLateDays.Rows)
            {
                TextBox txt_Latedays = (TextBox)gRow.FindControl("txt_Latedays");
                TextBox txt_LOPdays = (TextBox)gRow.FindControl("txt_LOPdays");
                DataRow dr = dtCol.NewRow();
                dr["LateDays"] = txt_Latedays.Text;
                dr["LOPDays"] = txt_LOPdays.Text;
                dtCol.Rows.Add(dr);
            }
            DataRow drNew = dtCol.NewRow();
            dtCol.Rows.Add(drNew);
            grdLateDays.DataSource = dtCol;
            grdLateDays.DataBind();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void btnRemoveRows_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCol = new DataTable();
            dtCol.Columns.Add("");
            dtCol.Columns.Add("LateDays");
            dtCol.Columns.Add("LOPDays");
            int dtValCount = 0;
            foreach (GridViewRow gRow in grdLateDays.Rows)
            {
                TextBox txt_Latedays = (TextBox)gRow.FindControl("txt_Latedays");
                TextBox txt_LOPdays = (TextBox)gRow.FindControl("txt_LOPdays");
                if (!String.IsNullOrEmpty(txt_Latedays.Text) || !String.IsNullOrEmpty(txt_LOPdays.Text))
                    dtValCount += 1;
                DataRow dr = dtCol.NewRow();
                dr["LateDays"] = txt_Latedays.Text;
                dr["LOPDays"] = txt_LOPdays.Text;
                dtCol.Rows.Add(dr);
            }
            if (dtValCount <= grdLateDays.Rows.Count - 1 && grdLateDays.Rows.Count > 1)
                dtCol.Rows.Remove(dtCol.Rows[grdLateDays.Rows.Count - 1]);
            grdLateDays.DataSource = dtCol;
            grdLateDays.DataBind();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void bindLeaveGrid()
    {
        try
        {
            DataTable dtCol = new DataTable();
            dtCol.Columns.Add("");
            dtCol.Columns.Add("LateDays");
            dtCol.Columns.Add("LOPDays");
            DataRow dr;
            string LeaveSet = string.Empty;
            string GetData = "select Per_Days,Per_Leave,Late_Set,Abs_Days_Calc,Inc_AllLOP from HR_PaySettings where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(GetData, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Per_Days"])))
                    txtPer.Text = Convert.ToString(ds.Tables[0].Rows[0]["Per_Days"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["Per_Leave"]).Trim() != "Select")
                    ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["Per_Leave"])));
                else
                    ddlLeaveType.SelectedIndex = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Abs_Days_Calc"])))
                    txtAbs_Days.Text = Convert.ToString(ds.Tables[0].Rows[0]["Abs_Days_Calc"]);
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Inc_AllLOP"])) && Convert.ToString(ds.Tables[0].Rows[0]["Inc_AllLOP"]).Trim().ToUpper() == "TRUE")
                    chkIncAllLop.Checked = true;
                else
                    chkIncAllLop.Checked = false;
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Late_Set"])))
                {
                    string[] LeaveDet = Convert.ToString(ds.Tables[0].Rows[0]["Late_Set"]).Split('\\');
                    if (LeaveDet.Length > 0)
                    {
                        for (int ik = 0; ik < LeaveDet.Length; ik++)
                        {
                            if (!String.IsNullOrEmpty(LeaveDet[ik]))
                            {
                                string[] SplVal = Convert.ToString(LeaveDet[ik]).Split(';');
                                if (SplVal.Length > 1)
                                {
                                    dr = dtCol.NewRow();
                                    dr[1] = Convert.ToString(SplVal[0]);
                                    dr[2] = Convert.ToString(SplVal[1]);
                                    dtCol.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                else
                {
                    dtCol.Rows.Add("", "", "");
                }
            }
            else
            {
                dtCol.Rows.Add("", "", "");
            }
            grdLateDays.DataSource = dtCol;
            grdLateDays.DataBind();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void btnGrdsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckDays() == true)
            {
                string PerDays = "0";
                if (!String.IsNullOrEmpty(Convert.ToString(txtPer.Text)))
                    PerDays = Convert.ToString(txtPer.Text);
                string PerLevType = Convert.ToString(ddlLeaveType.SelectedItem.Text);
                string AbsDays = "0";
                if (!String.IsNullOrEmpty(Convert.ToString(txtAbs_Days.Text)))
                    AbsDays = Convert.ToString(txtAbs_Days.Text);
                string IncAllLop = "0";
                if (chkIncAllLop.Checked == true)
                    IncAllLop = "1";
                string LateSet = string.Empty;
                string InsQ = "";
                foreach (GridViewRow gRow in grdLateDays.Rows)
                {
                    TextBox txt_Latedays = (TextBox)gRow.FindControl("txt_Latedays");
                    TextBox txt_LOPdays = (TextBox)gRow.FindControl("txt_LOPdays");
                    if (!String.IsNullOrEmpty(txt_Latedays.Text) && !String.IsNullOrEmpty(txt_LOPdays.Text))
                    {
                        if (LateSet.Trim() == "")
                            LateSet = Convert.ToString(txt_Latedays.Text) + ";" + Convert.ToString(txt_LOPdays.Text);
                        else
                            LateSet = LateSet + "\\" + Convert.ToString(txt_Latedays.Text) + ";" + Convert.ToString(txt_LOPdays.Text);
                    }
                }
                if (PerDays == "0" && PerLevType.Trim() == "Select" && IncAllLop == "0" && AbsDays == "0" && String.IsNullOrEmpty(LateSet))
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Fill any Values!";
                }
                else
                {
                    InsQ = "if exists(select * from HR_PaySettings where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') update HR_PaySettings set Per_Days='" + PerDays + "',Per_Leave='" + PerLevType + "',Late_Set='" + LateSet + "',Abs_Days_Calc='" + AbsDays + "',Inc_AllLOP='" + IncAllLop + "' where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' else insert into HR_PaySettings (Per_Days,Per_Leave,Late_Set,Abs_Days_Calc,College_Code,Inc_AllLOP) values ('" + PerDays + "','" + PerLevType + "','" + LateSet + "','" + AbsDays + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "','" + IncAllLop + "')";
                    int UpdC = d2.update_method_wo_parameter(InsQ, "Text");
                    if (UpdC > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Settings Saved Successfully!";
                    }
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Late Days Should be Greater than or Equal to Lop Days!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    private bool CheckDays()
    {
        bool DaysCheck = true;
        try
        {
            double LateDays = 0;
            double LopDays = 0;
            foreach (GridViewRow gRow in grdLateDays.Rows)
            {
                TextBox txt_Latedays = (TextBox)gRow.FindControl("txt_Latedays");
                TextBox txt_LOPdays = (TextBox)gRow.FindControl("txt_LOPdays");
                if (!String.IsNullOrEmpty(txt_Latedays.Text) && !String.IsNullOrEmpty(txt_LOPdays.Text))
                {
                    double.TryParse(Convert.ToString(txt_Latedays.Text), out LateDays);
                    double.TryParse(Convert.ToString(txt_LOPdays.Text), out LopDays);
                    if (LateDays <= LopDays)
                    {
                        DaysCheck = false;
                        return DaysCheck;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
        return DaysCheck;
    }
    private void bindLeave()
    {
        try
        {
            ddlLeaveType.Items.Clear();
            string GetLeaveQ = "select shortname from leave_category where (status='comp' or status='1') and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(GetLeaveQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLeaveType.DataSource = ds;
                ddlLeaveType.DataValueField = "shortname";
                ddlLeaveType.DataValueField = "shortname";
                ddlLeaveType.DataBind();
                ddlLeaveType.Items.Insert(0, "Select");
            }
            else
            {
                ddlLeaveType.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    public void payprocess()
    {
        lblgetscode = "";
        string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        try
        {
            bool lopfrombasic = false;
            #region check condition Alert
            string StfCategory = string.Empty;
            if (cb_lopfrom_atn.Checked == false && (cb_lop_fromgross.Checked == false && cb_Lopfrom_basic.Checked == false && cb_Lopfrom_payband.Checked == false && cb_lopgradpay.Checked == false) && cb_absentcalculation.Checked == false && cb_NA_RL.Checked == false && cb_LOP_to_PF.Checked == false && chk_fpf.Checked == false && cb_auto_deduct.Checked == false && cb_unpaid_leave.Checked == false && cb_max_PER.Checked == false && cbincitcalc.Checked == false && cbHrStfCat.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please select any one Item and Generate!";
                return;
            }
            if (rdb_Days.Checked == true && (txt_totaldays.Text.Trim() == "" || txt_totaldays.Text.Trim() == "0" || txt_totaldays.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Days!";
                return;
            }
            if (cb_absentcalculation.Checked == true && (txt_absent.Text.Trim() == "" || txt_absent.Text.Trim() == "0" || txt_absent.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Absent Days!";
                return;
            }

            if (rdb_month.Checked == false && rdb_Days.Checked == false)//delsi18/05
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select MonthWise Or Days For Calculation!";
                return;

            }

            if (cb_absentcalculation.Checked == false && cb_NA_RL.Checked == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please select Absent Calculation!";
                return;
            }
            if (cb_LOP_to_PF.Checked == true && (txt_days.Text.Trim() == "" || txt_days.Text.Trim() == "0" || txt_days.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the LOP & PF Max Days!";
                return;
            }
            if (chk_fpf.Checked == true && (txt_fpf.Text.Trim() == "" || txt_fpf.Text.Trim() == "0" || txt_fpf.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the FPF Percentage!";
                return;
            }
            if (chk_fpf.Checked == true && (txt_max_amount.Text.Trim() == "" || txt_max_amount.Text.Trim() == "0" || txt_max_amount.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Maximum Amount!";
                return;
            }
            if (cb_max_PER.Checked == true && (txt_after.Text.Trim() == "" || txt_after.Text.Trim() == "0" || txt_after.Text.Trim() == "0.00"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Max Permission Days!";
                return;
            }
            int StfCatCount = 0;
            StfCategory = GetSelectedItemsValueAsString(cblHrStfCat, out StfCatCount);
            if (ChkHourWise.Checked == true && (String.IsNullOrEmpty(StfCategory)))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Staff Category!";
                return;
            }
            if (chk_CLCalc.Checked == true && (ddlFromMonth.SelectedItem.Text == "---Select---" || ddlToMonth.SelectedItem.Text == "---Select---" || ddlToYear.SelectedItem.Text == "Select"))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select CL Calculation Period!";
                return;
            }
            #endregion
            #region Variables
            Boolean check_join = false;
            string strstaffcode = "";
            string year = "";
            string year1 = "";
            string month = "";
            string month1 = "";
            string endyear = "";
            int depcount = 0;
            int desigcount = 0;
            int catcount = 0;
            int stfcount = 0;
            string dept = "";
            string desig = "";
            string newdept = "";
            string staftype = "";
            string staffcategory = "";
            string islopfrmattn = "";
            string islopfrmgross = "";
            string isptgrosslop = "";
            string islopfrmbas = "";
            string islopfrmpayband = "";
            string islopfrmgp = "";
            string isabscal = "";
            string isnarl = "";
            string isloppf = "";
            double loppfdays = 0;
            string isfpfcheck = "";
            string isautoded = "";
            string isunpaidlev = "";
            string ismaxperla = "";
            string isformuldays = "";
            string isitcalc = "";
            string isHourWise = "";
            string isCLCalc = "";
            string isIncAllLop = "";
            string monthwise = "";
            string daywise = "";
            string strautoded = "";
            string fromdate = "";
            string todate = "";
            string Pay_Start = "";
            string pay_end = "";
            int joinworkdays = 0; // poo 31.10.17
            DateTime newdt = new DateTime();
            DateTime newdt1 = new DateTime();
            DateTime frmdt = new DateTime();
            DateTime todt = new DateTime();
            string staf_cd = "";
            string appl_ID = "";
            string dept_code = "";
            string desig_code = "";
            double bas_salary = 0;
            double grade_pay = 0;
            double cb_payband = 0;
            double agp = 0;
            string auto_GP = "";
            string dateofbirth = "";
            string deptname = "";
            string designame = "";
            string catcode = "";
            string stftype = "";
            string ismanuallop = "";
            string isdailywages = "";
            string ismpf = "";
            string mpfper = "";
            string isconsolid = "";
            string slabtype = "";
            string TransferBankFK = string.Empty;
            double slabval = 0;
            string[] spl = new string[2];
            int bornyear = 0;
            int bornmon = 0;
            int bornday = 0;
            DateTime borndate = new DateTime();
            DateTime currdate = DateTime.Now;
            string joindt = "";
            string relievedt = "";
            int joinday = 0;
            int joinmon = 0;
            int joinyear = 0;
            int relieveday = 0;
            int relievemon = 0;
            int relieveyear = 0;
            string[] spljoin = new string[2];
            string[] splrelieve = new string[2];
            int curryear = currdate.Year;
            int currmon = currdate.Month;
            int currday = currdate.Day;
            int totdays = 0;
            double absval = 0;
            double fpfper = 0;
            double age = 0;
            int ageval = 0;
            double maxamnt = 0;
            int perla = 0;
            double workingdays = 0;
            int savecount = 0;
            int insitcount = 0;
            double count;
            double cabscount;
            double permissioncount;
            double notappcount;
            double nacount;
            int mon_day;
            Double workdays;
            double prelop;
            double curlop;
            int newmon;
            double rlcount;
            double pcount;
            double cpcount;
            double hcount;
            double lacount;
            double odcount;
            double empcount;
            double clecount;
            double unpaidlev;
            double lopcount;
            double actpercount;
            double presdays;
            double absdays;
            double lopdays;
            string leavedet;
            double actbasic;
            double actpayband;
            double actgradepay;
            double basicwlop;
            double paybandwlop;
            double gradepaywlop;
            double lopperday;
            double lopbasamnt;
            double lopamnt;
            double AllDedLopAmnt = 0;
            double loppaybandamnt;
            double lopgrossamnt;
            double lopgpamnt;
            double oned_bassal;
            double oned_paybandsal;
            double oned_gpsal;
            double mpfamnt = 0;
            bool isentry;
            bool IsHourEntry = false;
            double CasualLeave = 0;
            double CasualGetLeave = 0;
            double CasualRemainLeave = 0;
            double CLCalcAmnt = 0;
            string[] lp = new string[2];
            string dat = "";
            string mont = "";
            string yr = "";
            string mon_year = "";
            string allowence;
            string deduction;
            double PerDays = 0;
            double AbsDays = 0;
            double FromDays = 0;
            double toDays = 0;
            double GetDays = 0;
            string PerLeaveType = string.Empty;
            string GetLateSet = string.Empty;
            string ParttimeStaff = string.Empty;
            if (rdb_month.Checked == true)
                monthwise = "1";
            if (rdb_Days.Checked == true)
                daywise = "1";
            if (cb_lopfrom_atn.Checked == true)
                islopfrmattn = "1";
            if (cb_Lopfrom_basic.Checked == true)
                islopfrmbas = "1";
            if (cb_Lopfrom_payband.Checked == true)
                islopfrmpayband = "1";
            if (cb_lopgradpay.Checked == true)
                islopfrmgp = "1";
            if (cb_lop_fromgross.Checked == true)
                islopfrmgross = "1";
            if (cb_ptgrosslop.Checked == true) //poomalar 25.10.17
                isptgrosslop = "1";
            if (cb_absentcalculation.Checked == true)
                isabscal = "1";
            if (cb_NA_RL.Checked == true)
                isnarl = "1";
            if (cb_LOP_to_PF.Checked == true)
                isloppf = "1";
            if (chk_fpf.Checked == true)
                isfpfcheck = "1";
            #endregion
            if (cb_auto_deduct.Checked == true)
            {
                isautoded = "1";
                string selq = d2.GetFunction("select deductions from incentives_master where college_code='" + collegecode1 + "'");
                if (selq.Trim() != "" && selq.Trim() != "0")
                {
                    string[] dedspl = selq.Split(';');
                    if (dedspl.Length > 0)
                    {
                        for (int ik = 0; ik < dedspl.Length; ik++)
                        {
                            string[] splval = dedspl[ik].Split('\\');
                            if (splval.Length >= 4)
                            {
                                if (splval[3].Trim() != "" && splval[3].Trim() != "0" && (splval[3].Trim() == "1"))
                                {
                                    if (strautoded.Trim() == "")
                                        strautoded = splval[0];
                                    else
                                        strautoded = strautoded + "," + splval[0];
                                }
                            }
                        }
                    }
                }
            }
            if (cb_unpaid_leave.Checked == true)
                isunpaidlev = "1";
            if (cb_max_PER.Checked == true)
                ismaxperla = "1";
            if (cb_formulate_days.Checked == true)
                isformuldays = "1";
            if (cbincitcalc.Checked == true)
                isitcalc = "1";
            if (ChkHourWise.Checked == true)
                isHourWise = "1";
            if (chk_CLCalc.Checked == true)
                isCLCalc = "1";
            Int32.TryParse(Convert.ToString(txt_totaldays.Text), out totdays);
            Double.TryParse(Convert.ToString(txt_absent.Text), out absval);
            Double.TryParse(Convert.ToString(txt_days.Text), out loppfdays);
            Double.TryParse(Convert.ToString(txt_fpf.Text), out fpfper);
            Double.TryParse(Convert.ToString(txt_age_val.Text), out age);
            Double.TryParse(Convert.ToString(txt_max_amount.Text), out maxamnt);
            Int32.TryParse(Convert.ToString(txt_after.Text), out perla);
            dept = GetSelectedItemsValueAsString(cbldeptcom, out depcount);
            desig = GetSelectedItemsValueAsString(cbl_desig, out desigcount);//delsi
            staftype = GetSelectedItemsText(cblstftypecom, out stfcount);
            staffcategory = GetSelectedItemsValueAsString(cblscatcom, out catcount);
            newdept = GetSelectedItemsText(cbldeptcom, out depcount);
            // int staffcount = 0;
            string Staffcode = string.Empty;
            Staffcode = Convert.ToString(txt_StaffCode.Text);
            string StfName = string.Empty;
            StfName = Convert.ToString(txt_staffname.Text);
            //if (cbStaffName.Checked)
            //    Staffcode = GetSelectedItemsValueAsString(cbl_staffname, out staffcount);
            //if (Cb_StaffCode.Checked)
            //    Staffcode = GetSelectedItemsValueAsString(CblStaffCode, out staffcount);
            Div2.Visible = true;
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            DataSet dsgetdates = new DataSet();
            DataSet dsgrad = new DataSet();
            DataSet dsgetslab = new DataSet();
            DataView dv = new DataView();
            ArrayList category = new ArrayList();
            bool delflag = false;
            #region leave settings
            string GetLeaveSetQ = "select Per_Days,Late_Set,Abs_Days_Calc,Per_Leave,Inc_AllLOP from HR_PaySettings where College_Code='" + collegecode1 + "'";//leave settings
            ds.Clear();
            ds = d2.select_method_wo_parameter(GetLeaveSetQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["Per_Days"]), out PerDays);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["Abs_Days_Calc"]), out AbsDays);
                GetLateSet = Convert.ToString(ds.Tables[0].Rows[0]["Late_Set"]);
                PerLeaveType = Convert.ToString(ds.Tables[0].Rows[0]["Per_Leave"]);
                isIncAllLop = Convert.ToString(ds.Tables[0].Rows[0]["Inc_AllLOP"]);
            }
            #endregion
            if (activerow.Trim() != "" && activecol.Trim() != "")
            {
                #region HR Year and Pay month Checking
                year1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                //endyear = Convert.ToString(ddl_toyr.SelectedItem.Text);
                ds.Clear();
                month = " select hryear_start,hryear_end,Pay_Start,Pay_End from hryears where ((year(hryear_start)='" + year1 + "') or (year(hryear_end)='" + year1 + "')) and collcode='" + collegecode1 + "' order by hryear_end desc";
                month = month + " select shortname from leave_category where (status ='comp' or status ='1') and college_code ='" + collegecode1 + "'";
                if (checkedok() && activecol.Trim() == "4")
                {
                    fpspreadshow.SaveChanges();
                    for (int ik = 1; ik < fpspreadshow.Sheets[0].RowCount; ik++)
                    {
                        byte check = Convert.ToByte(fpspreadshow.Sheets[0].Cells[ik, 1].Value);
                        if (check == 1)
                        {
                            if (strstaffcode.Trim() == "")
                                strstaffcode = Convert.ToString(fpspreadshow.Sheets[0].Cells[ik, 2].Text);
                            else
                                strstaffcode = strstaffcode + "','" + Convert.ToString(fpspreadshow.Sheets[0].Cells[ik, 2].Text);
                        }
                    }
                    month = month + " select s.staff_code,s.staff_name,st.stftype,st.allowances,st.deductions,ISNULL(st.bsalary,'0') as bsalary,ISNULL(st.grade_pay,'0') as grade_pay,ISNULL(pay_band,'0') as pay_band,IsAutoGP, st.isconsolid,app.date_of_birth as birthdate,dept.Dept_Name,desig.desig_name,st.category_code,st.AGP,st.IsManualLOP,st.IsDailyWages,st.IsMPFAmt,st.MPFAmount,st.MPFPer,st.IsConsolid,app.appl_id,dept.dept_code,desig.desig_code,st.stfnature,st.CollegeTransferBankFK from staffmaster s,stafftrans st,staff_appl_master app,desig_master desig,hrdept_master dept where desig.desig_code=st.desig_code and dept.Dept_Code=st.dept_code and s.staff_code =st.staff_code and app.appl_no=s.appl_no and s.college_code=desig.collegeCode and s.college_code=dept.college_code and desig.collegeCode=dept.college_code and st.latestrec ='1' and ((s.resign =0 and s.settled =0) and (Discontinue=0 or Discontinue is null)) and s.college_code ='" + collegecode1 + "' and s.staff_code in('" + strstaffcode + "') and app.interviewstatus='Appointed'";
                }
                else if (activecol.Trim() == "3")//delsi
                {
                    month = month + " select s.staff_code,s.staff_name,st.stftype,st.allowances,st.deductions,ISNULL(st.bsalary,'0') as bsalary,ISNULL(st.grade_pay,'0') as grade_pay,ISNULL(pay_band,'0') as pay_band,IsAutoGP, st.isconsolid,app.date_of_birth as birthdate,dept.Dept_Name,desig.desig_name,st.category_code,st.AGP,st.IsManualLOP,st.IsDailyWages,st.IsMPFAmt,st.MPFAmount,st.MPFPer,st.IsConsolid,app.appl_id,dept.dept_code,desig.desig_code,st.stfnature,st.CollegeTransferBankFK from staffmaster s,stafftrans st,staff_appl_master app,desig_master desig,hrdept_master dept where desig.desig_code=st.desig_code and dept.Dept_Code=st.dept_code and s.staff_code =st.staff_code and app.appl_no=s.appl_no and s.college_code=desig.collegeCode and s.college_code=dept.college_code and desig.collegeCode=dept.college_code and st.latestrec ='1' and ((s.resign =0 and s.settled =0) and (Discontinue=0 or Discontinue is null)) and s.college_code ='" + collegecode1 + "' and app.interviewstatus='Appointed'";
                    if (dept.Trim() != "")
                        month = month + " and st.dept_code in('" + dept + "')";

                    if (desig.Trim() != "")
                        month = month + " and st.desig_code in('" + desig + "')";
                    if (staftype.Trim() != "")
                        month = month + " and st.stftype in('" + staftype + "')";
                    if (staffcategory.Trim() != "")
                        month = month + " and st.category_code in('" + staffcategory + "')";
                    if (!string.IsNullOrEmpty(Staffcode))
                        month = month + " and st.staff_code in('" + Staffcode + "')";
                    if (!string.IsNullOrEmpty(StfName))
                        month = month + " and s.staff_name in('" + StfName + "')";
                    if (ViewState["staff_code"] != "" && ViewState["staff_code"] != null)//delsi1104
                    {
                        string stfcode = Convert.ToString(ViewState["staff_code"]);
                        if (!string.IsNullOrEmpty(stfcode))
                            month = month + " and st.staff_code in('" + stfcode + "')";
                    }

                }
                ds = d2.select_method_wo_parameter(month, "Text");
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int cat = 0; cat < ds.Tables[1].Rows.Count; cat++)
                    {
                        string shotname = "";
                        shotname = ds.Tables[1].Rows[cat]["shortname"].ToString();
                        if (!category.Contains(shotname))
                            category.Add(shotname);
                    }
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string getdates = "select Convert(varchar(10),From_Date,103) as From_Date,Convert(varchar(10),To_Date,103) as To_Date from HrPayMonths where PayMonthNum='" + month1 + "' and PayYear='" + year1 + "' and College_Code='" + collegecode1 + "' and SelStatus='1'";//barath 08.01.17
                    dsgetdates.Clear();
                    dsgetdates = d2.select_method_wo_parameter(getdates, "Text");
                    if (dsgetdates.Tables.Count > 0 && dsgetdates.Tables[0].Rows.Count > 0)
                    {
                        fromdate = Convert.ToString(dsgetdates.Tables[0].Rows[0]["From_Date"]);
                        frmdt = getdayfrst(fromdate);
                        todate = Convert.ToString(dsgetdates.Tables[0].Rows[0]["To_Date"]);
                        todt = getdayfrst(todate);
                        newdt = frmdt;
                        newdt1 = todt;
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Select the Corresponding HR Pay Months!";
                        return;
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select the HR Year!";
                    return;
                }
                #endregion
                DataSet dsHourDet = new DataSet();
                string GetHrSelQ = " select WorkingHour,Staff_Code,appl_id,workingdate from Hour_Staff_Attnd where WorkingDate between '" + frmdt.ToString("MM/dd/yyyy") + "' and '" + todt.ToString("MM/dd/yyyy") + "' order by Staff_Code,WorkingDate";
                GetHrSelQ = GetHrSelQ + " select Amnt_Per_Hrs,dept_code,desig_code,Tot_Hrs,PayType,StaffCode from HourWise_PaySettings where college_code='" + collegecode1 + "' ";
                if (dept.Trim() != "")
                    GetHrSelQ += " and dept_code in('" + dept + "')";
                dsHourDet.Clear();
                dsHourDet = d2.select_method_wo_parameter(GetHrSelQ, "Text");
                DataView dvMy1 = new DataView();
                DataView dvMy2 = new DataView();
                DataView dvMy3 = new DataView();
                Dictionary<string, string> dictCat = new Dictionary<string, string>();
                dictCat.Clear();
                for (int icat = 0; icat < cblHrStfCat.Items.Count; icat++)
                {
                    if (cblHrStfCat.Items[icat].Selected == true)
                    {
                        if (!dictCat.ContainsKey(Convert.ToString(cblHrStfCat.Items[icat].Value)))
                            dictCat.Add(Convert.ToString(cblHrStfCat.Items[icat].Value), Convert.ToString(cblHrStfCat.Items[icat].Text));
                    }
                }
                DataSet dsAvail = new DataSet();
                DataSet dsCLgetVal = new DataSet();
                DataSet dsGetPay = new DataSet();
                DataSet dsgetCL = new DataSet();
                if (isCLCalc == "1")
                {
                    string SelGetCL = "select staff_code,leavetype,category_code from individual_Leave_type where college_code='" + collegecode1 + "' and (leavetype like 'Casual Leave%' or leavetype like '%Casual Leave%' or leavetype like '%Casual Leave')";
                    dsgetCL.Clear();
                    dsgetCL = d2.select_method_wo_parameter(SelGetCL, "Text");
                    string str = "select PayMonth,PayMonthNum,PayYear,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date from HrPayMonths where College_Code='" + collegecode1 + "' and SelStatus='1' and From_Date>='" + Convert.ToString(ddlFromMonth.SelectedItem.Value) + "/01/" + Convert.ToString(ddlFromYear.SelectedItem.Text) + "' and To_Date<='" + Convert.ToString(ddlToMonth.SelectedItem.Value) + "/" + monthdays(Convert.ToString(ddlToMonth.SelectedItem.Value), Convert.ToString(ddlToYear.SelectedItem.Text)) + "/" + Convert.ToString(ddlToYear.SelectedItem.Text) + "'";
                    dsGetPay.Clear();
                    dsGetPay = d2.select_method_wo_parameter(str, "Text");
                }
                DataSet dsGetHol = new DataSet();
                DataView dvHol = new DataView();
                Dictionary<string, string> dictGetHol = new Dictionary<string, string>();
                dictGetHol.Clear();
                //string SelHolQ = " select halforfull,morning,evening,category_code,StfType,DAY(holiday_date) as HolDay from holidayStaff where college_code='" + collegecode1 + "' and holiday_date between '" + frmdt.ToString("MM/dd/yyyy") + "' and '" + todt.ToString("MM/dd/yyyy") + "'";
                string SelHolQ = " select halforfull,morning,evening,category_code,dept_code,StfType,DAY(holiday_date) as HolDay from holidayStaff where college_code='" + collegecode1 + "' and holiday_date between '" + frmdt.ToString("MM/dd/yyyy") + "' and '" + todt.ToString("MM/dd/yyyy") + "'";
                dsGetHol.Clear();
                dsGetHol = d2.select_method_wo_parameter(SelHolQ, "Text");
                if (dsGetHol.Tables.Count > 0 && dsGetHol.Tables[0].Rows.Count > 0)
                {
                    for (int hol = 0; hol < dsGetHol.Tables[0].Rows.Count; hol++)
                    {
                        //if (!dictGetHol.ContainsKey(Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"])))
                        //    dictGetHol.Add(Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"]), Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["halforfull"]).Trim().ToUpper() + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["morning"]).Trim().ToUpper() + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["evening"]).Trim().ToUpper());

                        if (!dictGetHol.ContainsKey(Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["dept_code"])))
                            dictGetHol.Add(Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["dept_code"]), Convert.ToString(dsGetHol.Tables[0].Rows[hol]["HolDay"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["category_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["StfType"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["dept_code"]) + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["halforfull"]).Trim().ToUpper() + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["morning"]).Trim().ToUpper() + "-" + Convert.ToString(dsGetHol.Tables[0].Rows[hol]["evening"]).Trim().ToUpper());


                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int scd = 0; scd < ds.Tables[2].Rows.Count; scd++)
                    {
                        check_join = false;//Added by saranya on 31/08/2018
                        CasualLeave = 0;
                        CasualGetLeave = 0;
                        CasualRemainLeave = 0;
                        CLCalcAmnt = 0;
                        string SelCLDt = string.Empty;
                        DateTime dtFrmCL = new DateTime();
                        DateTime dtToCL = new DateTime();
                        ParttimeStaff = Convert.ToString(ds.Tables[2].Rows[scd]["stfnature"]);
                        if (ParttimeStaff.Trim().ToUpper() == "PART")//delsi0906
                        {
                            ParttimeStaff = "1";
                        }

                        staf_cd = ds.Tables[2].Rows[scd]["staff_code"].ToString();
                        appl_ID = Convert.ToString(ds.Tables[2].Rows[scd]["appl_id"]);
                        dept_code = Convert.ToString(ds.Tables[2].Rows[scd]["dept_code"]);
                        desig_code = Convert.ToString(ds.Tables[2].Rows[scd]["desig_code"]);
                        Double.TryParse(Convert.ToString(ds.Tables[2].Rows[scd]["bsalary"]), out bas_salary);
                        Double.TryParse(Convert.ToString(ds.Tables[2].Rows[scd]["grade_pay"]), out grade_pay);
                        Double.TryParse(Convert.ToString(ds.Tables[2].Rows[scd]["pay_band"]), out cb_payband);
                        Double.TryParse(Convert.ToString(ds.Tables[2].Rows[scd]["AGP"]), out agp);
                        auto_GP = Convert.ToString(ds.Tables[2].Rows[scd]["IsAutoGP"]);
                        dateofbirth = Convert.ToString(ds.Tables[2].Rows[scd]["birthdate"]);
                        deptname = Convert.ToString(ds.Tables[2].Rows[scd]["Dept_Name"]);
                        designame = Convert.ToString(ds.Tables[2].Rows[scd]["desig_name"]);
                        catcode = Convert.ToString(ds.Tables[2].Rows[scd]["category_code"]);
                        stftype = Convert.ToString(ds.Tables[2].Rows[scd]["stftype"]);
                        ismanuallop = Convert.ToString(ds.Tables[2].Rows[scd]["IsManualLOP"]);
                        isdailywages = Convert.ToString(ds.Tables[2].Rows[scd]["IsDailyWages"]);
                        ismpf = Convert.ToString(ds.Tables[2].Rows[scd]["IsMPFAmt"]);
                        mpfper = Convert.ToString(ds.Tables[2].Rows[scd]["MPFPer"]);
                        isconsolid = Convert.ToString(ds.Tables[2].Rows[scd]["IsConsolid"]);
                        Double.TryParse(Convert.ToString(ds.Tables[2].Rows[scd]["MPFAmount"]), out mpfamnt);
                        TransferBankFK = Convert.ToString(ds.Tables[2].Rows[scd]["CollegeTransferBankFK"]);
                        frmdt = newdt;
                        todt = newdt1;
                        if (isCLCalc == "1" && dsgetCL.Tables.Count > 0 && dsgetCL.Tables[0].Rows.Count > 0)
                        {
                            dsgetCL.Tables[0].DefaultView.RowFilter = " staff_code='" + staf_cd + "' and category_code='" + catcode + "'";
                            dvMy3 = dsgetCL.Tables[0].DefaultView;
                            if (dvMy3.Count > 0)
                            {
                                string GetCL = Convert.ToString(dvMy3[0]["leavetype"]);
                                if (!String.IsNullOrEmpty(GetCL))
                                {
                                    string[] splLev = GetCL.Split('\\');
                                    if (splLev.Length > 0)
                                    {
                                        for (int cl = 0; cl < splLev.Length; cl++)
                                        {
                                            if (splLev[cl].Contains("Casual Leave") || splLev[cl].Contains("CASUAL LEAVE"))
                                            {
                                                string[] splMyLev = splLev[cl].Split(';');
                                                if (splMyLev.Length > 1)
                                                {
                                                    double.TryParse(Convert.ToString(splMyLev[1]), out CasualLeave);
                                                    break;
                                                }
                                            }
                                        }
                                        if (CasualLeave > 0 && dsGetPay.Tables.Count > 0 && dsGetPay.Tables[0].Rows.Count > 0)
                                        {
                                            for (int cs = 0; cs < dsGetPay.Tables[0].Rows.Count; cs++)
                                            {
                                                if (Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayMonthNum"]) + "/" + Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayYear"]) != month1 + "/" + year1)
                                                {
                                                    dsAvail.Clear();
                                                    dsAvail = d2.select_method_wo_parameter("select * from staff_attnd where staff_code='" + staf_cd + "' and mon_year='" + Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayMonthNum"]) + "/" + Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayYear"]) + "'", "Text");
                                                    if (!String.IsNullOrEmpty(Convert.ToString(dsGetPay.Tables[0].Rows[cs]["From_Date"])) && !String.IsNullOrEmpty(Convert.ToString(dsGetPay.Tables[0].Rows[cs]["To_Date"])) && dsAvail.Tables.Count > 0 && dsAvail.Tables[0].Rows.Count > 0)
                                                    {
                                                        dtFrmCL = Convert.ToDateTime(Convert.ToString(dsGetPay.Tables[0].Rows[cs]["From_Date"]));
                                                        dtToCL = Convert.ToDateTime(Convert.ToString(dsGetPay.Tables[0].Rows[cs]["To_Date"]));
                                                        while (dtFrmCL <= dtToCL)
                                                        {
                                                            SelCLDt = string.Empty;
                                                            SelCLDt = d2.GetFunction("select [" + dtFrmCL.Day + "] from staff_attnd where staff_code='" + staf_cd + "' and mon_year='" + Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayMonthNum"]) + "/" + Convert.ToString(dsGetPay.Tables[0].Rows[cs]["PayYear"]) + "'");
                                                            if (!String.IsNullOrEmpty(SelCLDt) && SelCLDt.Trim() != "0")
                                                            {
                                                                string[] splDay = SelCLDt.Split('-');
                                                                if (splDay.Length > 1)
                                                                {
                                                                    if (splDay[0] == "CL")
                                                                        CasualGetLeave += 1;
                                                                    if (splDay[1] == "CL")
                                                                        CasualGetLeave += 1;
                                                                }
                                                            }
                                                            dtFrmCL = dtFrmCL.AddDays(1);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (isfpfcheck.Trim() == "1")
                        {
                            if (dateofbirth.Trim() != "")
                            {
                                spl = dateofbirth.Split(' ')[0].Split('/');
                                Int32.TryParse(spl[1], out bornday);
                                Int32.TryParse(spl[0], out bornmon);
                                Int32.TryParse(spl[2], out bornyear);
                                borndate = Convert.ToDateTime(spl[0] + "/" + spl[1] + "/" + spl[2]);
                            }
                            if (borndate != null)
                                ageval = curryear - bornyear;
                        }
                        ds5.Clear();
                        string current_Join_relive = "select Convert(varchar(10),relieve_date,103) as relieve_date,Convert(varchar(10),join_date,103) as join_date from staffmaster where ((resign = 1 and settled = 1) or Discontinue=1) and (relieve_date between ('" + frmdt + "') and ('" + todt + "') or join_date between ('" + frmdt + "') and ('" + todt + "')) and staff_code = '" + staf_cd + "' and college_code='" + collegecode1 + "'";
                        ds5 = d2.select_method_wo_parameter(current_Join_relive, "Text");
                        if (monthwise == "1")
                        {
                            TimeSpan ts = todt - frmdt;
                            workingdays = ts.Days + 1;
                        }
                        else if (daywise == "1")
                            workingdays = totdays;
                        //poo
                        // region added by poo 31.10.17
                        # region calculate joindate for staff
                        string joinquery = "select Convert(varchar(10),join_date,103) as join_date from staffmaster where college_code='" + collegecode1 + "' and staff_code='" + staf_cd + "'";
                        string joindatevalue = d2.GetFunction(joinquery).Trim();
                        if (joindatevalue != "" && !string.IsNullOrEmpty(joindatevalue))
                        {
                            string[] splitdet = joindatevalue.Split('/');
                            string jnday = splitdet[0].ToString();
                            string joinmonth = splitdet[1].ToString();
                            string joinyr = splitdet[2].ToString();
                            joindate = new DateTime(Convert.ToInt32(joinyr), Convert.ToInt32(joinmonth), Convert.ToInt32(jnday)); // poo
                            DataSet joinds = new DataSet();
                            if (frmdt.Month == joindate.Month && frmdt.Year == joindate.Year)
                            {
                                string joinhrmonth = d2.GetFunction("select Convert(varchar(10),join_date,103) as join_date from staffmaster where college_code='" + collegecode1 + "' and staff_code='" + staf_cd + "' and join_date between ('" + frmdt + "') and ('" + todt + "')");//delsi 25/07

                                if (joinhrmonth != "0" && joinhrmonth != "")
                                {
                                    joinworkdays = todt.Day - joindate.Day;
                                    joinworkdays += 1;
                                    frmdt = joindate;
                                    check_join = true;//delsi05/05/2018
                                }
                            }
                            else if (todt.Month == joindate.Month && todt.Year == joindate.Year)
                            {
                                string joinhrmonth = d2.GetFunction("select Convert(varchar(10),join_date,103) as join_date from staffmaster where college_code='" + collegecode1 + "' and staff_code='" + staf_cd + "' and join_date between ('" + frmdt + "') and ('" + todt + "')");//delsi 25/07

                                if (joinhrmonth != "0" && joinhrmonth != "")
                                {

                                    joinworkdays = todt.Day - joindate.Day;
                                    joinworkdays += 1;
                                    frmdt = joindate;
                                    check_join = true;//delsi05/05/2018
                                }
                            }
                        }
                        # endregion
                        if (auto_GP.Trim().ToUpper() == "TRUE" && isconsolid.Trim().ToUpper() == "FALSE")
                        {
                            string getgradeamnt = "select slabtype,slabvalue from pfslabs where SlabFor='Grade Pay' and '" + Convert.ToString(bas_salary) + "' between salfrom and salto and college_code='" + collegecode1 + "'";
                            dsgrad.Clear();
                            dsgrad = d2.select_method_wo_parameter(getgradeamnt, "Text");
                            if (dsgrad.Tables.Count > 0 && dsgrad.Tables[0].Rows.Count > 0)
                            {
                                slabtype = Convert.ToString(dsgrad.Tables[0].Rows[0]["slabtype"]);
                                Double.TryParse(Convert.ToString(dsgrad.Tables[0].Rows[0]["slabvalue"]), out slabval);
                                if (slabtype.Trim() == "Percent")
                                    grade_pay = (slabval / 100) * bas_salary;
                                else if (slabtype.Trim() == "Amount")
                                    grade_pay = slabval;
                            }
                            int updq = d2.update_method_wo_parameter("update stafftrans set grade_pay='" + grade_pay + "' where staff_code='" + staf_cd + "' and latestrec='1'", "Text");
                        }
                        count = 0;
                        cabscount = 0;
                        double doubleabsentcount = 0;
                        permissioncount = 0;
                        notappcount = 0;
                        nacount = 0;
                        prelop = 0;
                        curlop = 0;
                        newmon = 0;
                        rlcount = 0;
                        pcount = 0;
                        cpcount = 0;
                        hcount = 0;
                        lacount = 0;
                        odcount = 0;
                        empcount = 0;
                        clecount = 0;
                        unpaidlev = 0;
                        lopcount = 0;
                        actpercount = 0;
                        mon_day = 0;
                        workdays = 0;
                        presdays = 0;
                        absdays = 0;
                        lopdays = 0;
                        leavedet = "";
                        actbasic = 0;
                        actpayband = 0;
                        actgradepay = 0;
                        basicwlop = 0;
                        paybandwlop = 0;
                        gradepaywlop = 0;
                        lopbasamnt = 0;
                        lopamnt = 0;
                        AllDedLopAmnt = 0;
                        lopperday = 0;
                        loppaybandamnt = 0;
                        lopgpamnt = 0;
                        lopgrossamnt = 0;
                        oned_bassal = 0;
                        oned_paybandsal = 0;
                        oned_gpsal = 0;
                        Double DABasic;
                        allowence = "";
                        deduction = "";
                        Double pers;
                        Double basicallow;
                        Double persallow;
                        Double lop_amou;
                        Double slabvalue;
                        Double slablop;
                        Double slabamnt;
                        Double DAAmt = 0;
                        Double totallow = 0;
                        Double totallowwolop = 0;
                        Double Gross_salary = 0;
                        double grosswlop = 0;
                        Double fpfamnt = 0;
                        string allow = "";
                        Double pfamnt = 0;
                        Double esiamnt = 0;
                        isentry = false;
                        DataSet dsgetdays = new DataSet();
                        Double currwrkdays = 0;
                        Double currpresdays = 0;
                        Double currlopdays = 0;
                        Double prevwrkdays = 0;
                        Double prevlopdays = 0;
                        Double prevpresdays = 0;
                        IsHourEntry = false;
                        Double HourWiseAmnt = 0;
                        Double WrkingHrs = 0;
                        double TotalLopAmount = 0;
                        GetDays = 0; double absentdouble = 0;
                        double GetAmntPerHrs = 0;

                        double FirstLopDays = 0;
                        double SecondLopDays = 0;
                        string preMon = string.Empty;
                        string CurMon = string.Empty;
                        bool differentMonthBool = false;

                        if (isHourWise == "1" && dictCat.ContainsKey(Convert.ToString(catcode)) && ParttimeStaff == "1")
                        {
                            #region Hourwise staff
                            if (dsHourDet.Tables.Count > 0 && dsHourDet.Tables[1].Rows.Count > 0)//&& dsHourDet.Tables[0].Rows.Count > 0
                            {
                                IsHourEntry = true;
                                //dsHourDet.Tables[0].DefaultView.RowFilter = " Staff_Code='" + staf_cd + "' and Appl_ID='" + appl_ID + "'";
                                //dvMy1 = dsHourDet.Tables[0].DefaultView;
                                if (chk_desigwise.Checked == true) /* poomalar 23.10.17*/
                                {
                                    dsHourDet.Tables[1].DefaultView.RowFilter = " dept_Code='" + dept_code + "' and desig_code='" + desig_code + "' and isnull(PayType,0)=0";
                                }
                                if (chk_staffwise.Checked == true) /* poomalar 23.10.17*/
                                {
                                    dsHourDet.Tables[1].DefaultView.RowFilter = " StaffCode='" + staf_cd + "' and isnull(PayType,0)=1";
                                }
                                dvMy2 = dsHourDet.Tables[1].DefaultView;
                                if (dvMy2.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(dvMy2[0]["Amnt_Per_Hrs"]), out GetAmntPerHrs);
                                    int.TryParse(Convert.ToString(dvMy2[0]["Tot_Hrs"]), out mon_day); /* poomalar 16.10.17*/
                                    WrkingHrs = mon_day; /* poomalar 16.10.17*/
                                    HourWiseAmnt += (WrkingHrs * GetAmntPerHrs);
                                }
                                //if ( GetAmntPerHrs > 0)//dvMy1.Count > 0 &&/* poomalar 16.10.17*/
                                //{
                                //    for (int ho = 0; ho < dvMy2.Count; ho++)
                                //    {
                                //        WrkingHrs = 0; 
                                //        Double.TryParse(Convert.ToString(dvMy2[ho]["Tot_Hrs"]), out WrkingHrs); 
                                //        HourWiseAmnt += (WrkingHrs * GetAmntPerHrs);
                                //        //if (WrkingHrs != 0)
                                //        //    mon_day += 1;
                                //    }
                                //}
                                workdays = mon_day;
                                lopdays = 0;
                                absdays = 0;
                                presdays = mon_day;
                                leavedet = Convert.ToString(workdays) + ";" + Convert.ToString(presdays) + ";" + Convert.ToString(absdays) + ";0;0;0;" + Convert.ToString(lopdays) + ";0;0;0;0;0;0\\";
                            }
                            #endregion
                        }
                        else
                        {
                            #region Full time Staff
                            if (ismanuallop.ToUpper() == "TRUE")   //Added By Jeyaprakash on Sep 12th
                            {
                                #region Manual Lop
                                string selq = "select * from StaffLOP_Details where college_code='" + collegecode1 + "' and staff_code='" + staf_cd + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "'";
                                dsgetdays.Clear();
                                dsgetdays = d2.select_method_wo_parameter(selq, "Text");
                                if (dsgetdays.Tables.Count > 0 && dsgetdays.Tables[0].Rows.Count > 0)
                                {
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["First_LOP_Days"]), out prevlopdays);
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["Second_LOP_Days"]), out currlopdays);
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["First_PresDays"]), out prevpresdays);
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["Second_PresDays"]), out currpresdays);
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["First_WorkDays"]), out prevwrkdays);
                                    Double.TryParse(Convert.ToString(dsgetdays.Tables[0].Rows[0]["Second_WorkDays"]), out currwrkdays);
                                    workdays = prevwrkdays + currwrkdays;
                                    presdays = prevpresdays + currpresdays;
                                    lopdays = prevlopdays + currlopdays;
                                    absdays = 0;

                                    joinworkdays = Convert.ToInt32(presdays);//22.01.18 nec
                                    prelop = prevlopdays;
                                    curlop = currlopdays;
                                }
                                else
                                {
                                    workdays = workingdays;
                                    presdays = workingdays;
                                    lopdays = 0;
                                    absdays = 0;
                                }
                                if (isloppf == "1")
                                {
                                    Double loppf = loppfdays;
                                    if (loppf != 0)
                                    {
                                        if (workdays > loppf)
                                        {
                                            if (absdays > loppf)
                                                absdays = absdays - loppf;
                                        }
                                    }
                                }
                                //if (AbsDays > 0)
                                //    absdays = absdays * AbsDays;
                                mon_day = Convert.ToInt32(workdays);
                                #endregion
                            }
                            else
                            {
                                if (frmdt.Month == todt.Month)
                                {
                                    #region Same Month
                                    while (frmdt <= todt)
                                    {
                                        mon_day++;
                                        newmon = 1;
                                        string dts = Convert.ToString(frmdt);
                                        string[] sp = dts.Split(' ');
                                        string firstdate = sp[0].ToString();
                                        string[] split = firstdate.Split('/');
                                        dat = split[1].ToString().TrimStart('0');
                                        mont = split[0].ToString().TrimStart('0');
                                        yr = split[2].ToString();
                                        mon_year = mont + "/" + yr;
                                        string lop_morn = "";
                                        string lop_even = "";
                                        string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
                                        if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
                                        {
                                            lp = atten.Split('-');
                                            if (lp.Length > 0)
                                            {
                                                lop_morn = lp[0].ToString();
                                                lop_even = lp[1].ToString();
                                            }
                                            if (lop_morn == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "A")
                                            {
                                                count++;
                                                cabscount++;

                                            }
                                            else if (lop_morn == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "NA")
                                                nacount++;
                                            else if (lop_morn == "RL")
                                                rlcount++;
                                            else if (lop_morn == "LOP")
                                                lopcount++;
                                            else if (lop_morn == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_morn))
                                                    lopcount++;
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                            if (lop_even == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "A")
                                            {
                                                count++;
                                                cabscount++;
                                            }
                                            else if (lop_even == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "NA")
                                                nacount++;
                                            else if (lop_even == "RL")
                                                rlcount++;
                                            else if (lop_even == "LOP")
                                                lopcount++;
                                            else if (lop_even == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_even))
                                                    lopcount++;
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!dictGetHol.ContainsKey(dat + "-" + catcode + "-" + stftype + "-" + dept_code))
                                                notappcount = notappcount + (newmon * 2);
                                        }
                                        frmdt = frmdt.AddDays(1);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Different Month
                                    differentMonthBool = true;
                                    int mon = frmdt.Month;
                                    int convyear = frmdt.Year;
                                    preMon = monthdays(Convert.ToString(mon), Convert.ToString(convyear));
                                    DateTime dtendday = new DateTime();
                                    dtendday = Convert.ToDateTime(Convert.ToString(mon) + "/" + preMon + "/" + Convert.ToString(convyear));
                                    #region FirstMonth
                                    while (frmdt <= dtendday)
                                    {
                                        newmon = 1;
                                        mon_day++;
                                        string dts = Convert.ToString(frmdt);
                                        string[] sp = dts.Split(' ');
                                        string firstdate = sp[0].ToString();
                                        string[] split = firstdate.Split('/');
                                        dat = split[1].ToString().TrimStart('0');
                                        string lop_morn = "";
                                        string lop_even = "";
                                        mont = split[0].ToString().TrimStart('0');
                                        yr = split[2].ToString();
                                        mon_year = mont + "/" + yr;
                                        string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
                                        if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
                                        {
                                            lp = atten.Split('-');
                                            if (lp.Length > 0)
                                            {
                                                lop_morn = lp[0].ToString();
                                                lop_even = lp[1].ToString();
                                            }
                                            if (lop_morn == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "A")
                                            {
                                                count++;
                                                cabscount++;
                                                prelop++;//delsi1205
                                            }
                                            else if (lop_morn == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "NA")
                                                nacount++;
                                            else if (lop_morn == "RL")
                                                rlcount++;
                                            else if (lop_morn == "LOP")
                                            {
                                                prelop++;
                                                lopcount++;
                                            }
                                            else if (lop_morn == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_morn))
                                                {
                                                    if (lop_morn.Trim() != "LOP" && lop_morn.Trim() != "LLP")
                                                        unpaidlev = unpaidlev + 1;
                                                    count++;
                                                    prelop++;
                                                    lopcount++;
                                                }
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                            if (lop_even == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "A")
                                            {
                                                count++;
                                                cabscount++;
                                                prelop++;//delsi1205
                                            }
                                            else if (lop_even == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "NA")
                                                nacount++;
                                            else if (lop_even == "RL")
                                                rlcount++;
                                            else if (lop_even == "LOP")
                                            {
                                                prelop++;
                                                lopcount++;
                                            }
                                            else if (lop_even == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_even))
                                                {
                                                    if (lop_even.Trim() != "LOP" && lop_even.Trim() != "LLP")
                                                        unpaidlev = unpaidlev + 1;
                                                    count++;
                                                    prelop++;
                                                    lopcount++;
                                                }
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!dictGetHol.ContainsKey(dat + "-" + catcode + "-" + stftype + "-" + dept_code))
                                                notappcount = notappcount + (newmon * 2);
                                        }
                                        frmdt = frmdt.AddDays(1);
                                    }
                                    FirstLopDays = lopcount;
                                    #endregion
                                    //LOP Amount

                                    #region SecondMonth
                                    dtendday = dtendday.AddDays(1);
                                    while (dtendday <= todt)
                                    {
                                        mon_day++;
                                        newmon = 1;
                                        string dts = Convert.ToString(dtendday);
                                        string[] sp = dts.Split(' ');
                                        string firstdate = sp[0].ToString();
                                        string[] split = firstdate.Split('/');
                                        dat = split[1].ToString().TrimStart('0');
                                        string lop_morn = "";
                                        string lop_even = "";
                                        mont = split[0].ToString().TrimStart('0');
                                        yr = split[2].ToString();
                                        mon_year = mont + "/" + yr;
                                        CurMon = monthdays(Convert.ToString(mont), Convert.ToString(yr));
                                        string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
                                        if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
                                        {
                                            lp = atten.Split('-');
                                            if (lp.Length > 0)
                                            {
                                                lop_morn = lp[0].ToString();
                                                lop_even = lp[1].ToString();
                                            }
                                            if (lop_morn == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "A")
                                            {
                                                count++;
                                                cabscount++;
                                                curlop++;//delsi1205
                                            }
                                            else if (lop_morn == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_morn == "NA")
                                                nacount++;
                                            else if (lop_morn == "RL")
                                                rlcount++;
                                            else if (lop_morn == "LOP")
                                            {
                                                curlop++;
                                                lopcount++;
                                            }
                                            else if (lop_morn == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_morn))
                                                {
                                                    count++;
                                                    curlop++;
                                                    lopcount++;
                                                }
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                            if (lop_even == "P")
                                            {
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "CL")
                                            {
                                                CasualGetLeave += 1;
                                                pcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "A")
                                            {
                                                count++;
                                                cabscount++;
                                                curlop++;//delsi1205
                                            }
                                            else if (lop_even == "PER")
                                            {
                                                permissioncount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "LA")
                                            {
                                                lacount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "OD")
                                            {
                                                odcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "H")
                                            {
                                                hcount++;
                                                cpcount++;
                                            }
                                            else if (lop_even == "NA")
                                                nacount++;
                                            else if (lop_even == "RL")
                                                rlcount++;
                                            else if (lop_even == "LOP")
                                            {
                                                curlop++;
                                                lopcount++;
                                            }
                                            else if (lop_even == " ")
                                            {
                                                empcount++;
                                                notappcount++;
                                            }
                                            else
                                            {
                                                if (category.Contains(lop_even))
                                                {
                                                    count++;
                                                    curlop++;
                                                    lopcount++;
                                                }
                                                else
                                                {
                                                    cpcount++;
                                                    clecount++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!dictGetHol.ContainsKey(dat + "-" + catcode + "-" + stftype + "-" + dept_code))
                                                notappcount = notappcount + (newmon * 2);
                                        }
                                        dtendday = dtendday.AddDays(1);
                                    }
                                    SecondLopDays = lopcount - FirstLopDays;
                                    if (isnarl == "1" && ds5.Tables[0].Rows.Count > 0)
                                    {
                                        if (nacount > 0)
                                        {
                                            isentry = true;
                                            workingdays = workingdays - ((nacount / 2) + (rlcount / 2));
                                            cpcount = (cpcount / 2) + workingdays;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                }
                                double myPerCount = permissioncount;
                                double myLateCount = lacount;
                                permissioncount = permissioncount / 2;
                                lacount = lacount / 2;
                                cpcount = cpcount / 2;
                                pcount = pcount / 2;
                                nacount = nacount / 2;
                                prelop = prelop / 2;
                                curlop = curlop / 2;
                                rlcount = rlcount / 2;
                                hcount = hcount / 2;
                                odcount = odcount / 2;
                                empcount = empcount / 2;
                                clecount = clecount / 2;
                                unpaidlev = unpaidlev / 2;
                                lopcount = lopcount / 2;
                                cabscount = cabscount / 2;
                                count = count / 2;
                                notappcount = notappcount / 2;
                                CasualGetLeave = CasualGetLeave / 2;
                                if (cabscount > 0)//barath
                                    absentdouble = cabscount;
                                if (PerDays > 0 && myPerCount > PerDays && !String.IsNullOrEmpty(PerLeaveType.Trim()) && PerLeaveType.Trim() != "Select")
                                    GetDays = myPerCount - PerDays;
                                if (GetDays > 0)
                                {
                                    GetDays /= 2;
                                    switch (PerLeaveType)
                                    {
                                        case "P":
                                            pcount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "CL":
                                            CasualGetLeave += GetDays;
                                            pcount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "A":
                                            cabscount += GetDays;
                                            cpcount -= GetDays;
                                            break;
                                        case "PER":
                                            permissioncount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "LA":
                                            lacount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "OD":
                                            odcount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "H":
                                            hcount += GetDays;
                                            cpcount += GetDays;
                                            cabscount -= GetDays;
                                            break;
                                        case "NA":
                                            nacount += GetDays;
                                            break;
                                        case "RL":
                                            rlcount += GetDays;
                                            break;
                                        case "LOP":
                                            lopcount += GetDays;
                                            cpcount -= GetDays;
                                            break;
                                        default:
                                            if (category.Contains(PerLeaveType))
                                            {
                                                lopcount += GetDays;
                                                cpcount -= GetDays;
                                            }
                                            break;
                                    }
                                }
                                if (CasualLeave >= CasualGetLeave)
                                    CasualRemainLeave = CasualLeave - CasualGetLeave;
                                TimeSpan tsnew = newdt1 - newdt;
                                int dayscount = tsnew.Days;
                                if (dayscount == 31 && curlop > 15)
                                {
                                    curlop = curlop - 1;
                                    cabscount = cabscount - 1;
                                }
                                if (ismaxperla == "1" && (myPerCount + myLateCount) > perla && isformuldays != "1")
                                {
                                    cabscount = cabscount + (((myPerCount + myLateCount) - perla) / 2);
                                    cpcount = cpcount - (((myPerCount + myLateCount) - perla) / 2);
                                }
                                else if (ismaxperla == "1" && isformuldays == "1")    //Modified on July 28th
                                {
                                    actpercount = (myPerCount + myLateCount) * 2;
                                    foreach (GridViewRow gv in grid_multiple_days.Rows)
                                    {
                                        TextBox txtfrm = (TextBox)gv.FindControl("txt_from");
                                        TextBox txtto = (TextBox)gv.FindControl("txt_To");
                                        TextBox grdlopdays = (TextBox)gv.FindControl("txt_LOP_days");
                                        if (txtfrm.Text.Trim() != "" && txtto.Text.Trim() != "" && grdlopdays.Text.Trim() != "")
                                        {
                                            if (actpercount != 0)
                                            {
                                                if (Convert.ToDouble(txtfrm.Text.Trim()) <= actpercount || Convert.ToDouble(txtto.Text.Trim()) <= actpercount)
                                                {
                                                    //if (actpercount > Convert.ToDouble(grdlopdays.Text.Trim()))
                                                    //{
                                                    cabscount = cabscount + Convert.ToDouble(grdlopdays.Text.Trim());
                                                    cpcount = cpcount - Convert.ToDouble(grdlopdays.Text.Trim());
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isloppf == "1")
                                {
                                    Double loppf = loppfdays;
                                    if (loppf != 0)
                                    {
                                        if (mon_day > loppf)
                                        {
                                            if (cabscount > loppf)
                                                cabscount = cabscount - loppf;
                                        }
                                    }
                                }
                                if (GetLateSet.Trim() != "0" && !String.IsNullOrEmpty(GetLateSet) && myLateCount > 0)
                                {
                                    string[] LeaveSet = GetLateSet.Split('\\');
                                    if (LeaveSet.Length > 0)
                                    {
                                        for (int le = LeaveSet.Length - 1; le > 0; le--)
                                        {
                                            if (!String.IsNullOrEmpty(LeaveSet[le]))
                                            {
                                                string[] MyLevSet = Convert.ToString(LeaveSet[le]).Split(';');
                                                if (MyLevSet.Length > 1)
                                                {
                                                    double.TryParse(Convert.ToString(MyLevSet[0]), out FromDays);
                                                    double.TryParse(Convert.ToString(MyLevSet[1]), out toDays);
                                                    if (FromDays <= myLateCount)
                                                    {
                                                        cabscount = cabscount + toDays;
                                                        cpcount = cpcount - toDays;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (workingdays >= (nacount + rlcount) && isentry == false)
                                    workdays = workingdays - (nacount + rlcount);
                                else
                                    workdays = workingdays;
                                presdays = cpcount;
                                absdays = cabscount;
                                lopdays = lopcount;
                            }
                            leavedet = Convert.ToString(workdays) + ";" + Convert.ToString(presdays) + ";" + Convert.ToString(absdays) + ";0;0;0;" + Convert.ToString(lopdays) + ";0;0;0;0;0;0\\";
                            #endregion
                        }

                        if (mon_day != 0 || (ParttimeStaff == "1"))
                        {
                            if (notappcount != 0)
                            {
                                missedcount++;
                                if (lblgetscode.Trim() == "")
                                    lblgetscode = Convert.ToString(staf_cd);
                                else
                                    lblgetscode = lblgetscode + "<br />" + Convert.ToString(staf_cd);
                            }
                            else if ((presdays == 0 && lopdays == 0) && ParttimeStaff != "1") /* poomalar 12.10.17*/
                            {
                                missedcount++;
                                if (lblgetscode.Trim() == "")
                                    lblgetscode = Convert.ToString(staf_cd);
                                else
                                    lblgetscode = lblgetscode + "<br />" + Convert.ToString(staf_cd);
                            }
                            else
                            {
                                if (IsHourEntry == true)
                                {
                                    actbasic = HourWiseAmnt;
                                    bas_salary = HourWiseAmnt;//barath 07/08/17
                                }
                                else
                                    actbasic = bas_salary;
                                actpayband = cb_payband;
                                actgradepay = grade_pay;
                                //if (AbsDays > 0)//change to top bb
                                //    absdays = cabscount * AbsDays;
                                //else
                                //    absdays = cabscount;
                                if (absentdouble > 0)
                                {
                                    if (AbsDays != 0)
                                    {
                                        if (cabscount != absentdouble)
                                            cabscount -= absentdouble;
                                        absdays = cabscount * AbsDays;
                                    }
                                    else
                                    {
                                        absdays = cabscount;//delsi3103
                                    }

                                }
                                lopdays += absdays;
                                oned_bassal = (bas_salary / workdays);
                                if (ParttimeStaff != "1") // poo 22.12.17
                                {
                                    if (todt.Month == joindate.Month && todt.Year == joindate.Year) //poo 31.10.17
                                        bas_salary = oned_bassal * joinworkdays; //poo 31.10.17
                                }
                                oned_bassal = (Convert.ToString(oned_bassal).ToUpper() == "NAN" ? 0 : oned_bassal);
                                oned_paybandsal = (cb_payband / workdays);
                                oned_gpsal = (grade_pay / workdays);
                                if (isCLCalc == "1" && CasualRemainLeave > 0)
                                    CLCalcAmnt = oned_bassal * CasualRemainLeave;
                                //LOP AMOUNT BARATH 12.01.18

                                double fmon = prelop;
                                double smon = curlop;
                                double FmonDays = 0;
                                double SmonDays = 0;
                                double.TryParse(preMon, out FmonDays);
                                double.TryParse(CurMon, out SmonDays);
                                double preLopAmt = 0;
                                double CurLopAmt = 0;
                                double preLopAmtpayband = 0;
                                double preLopAmtgrosspay = 0;
                                double CurLopAmtpayband = 0;
                                double CurLopAmtgrosspay = 0;
                                if (!differentMonthBool)
                                {
                                    lopbasamnt = lopdays * oned_bassal;//17.01.18
                                    loppaybandamnt = lopdays * oned_paybandsal;//17.01.18
                                    lopgpamnt = lopdays * oned_gpsal;
                                }
                                else
                                {
                                    if (prelop != 0)//delsi2040
                                    {
                                        preLopAmt = (bas_salary / FmonDays) * prelop;
                                        preLopAmtgrosspay = (grade_pay / FmonDays) * prelop;
                                        preLopAmtpayband = (cb_payband / FmonDays) * prelop;
                                    }
                                    if (curlop != 0)
                                    {
                                        CurLopAmt = (bas_salary / SmonDays) * curlop;
                                        CurLopAmtgrosspay = (grade_pay / SmonDays) * curlop;
                                        CurLopAmtpayband = (cb_payband / SmonDays) * curlop;
                                    }
                                    lopbasamnt = preLopAmt + CurLopAmt;
                                    loppaybandamnt = preLopAmtpayband + CurLopAmtpayband;
                                    lopgpamnt = preLopAmtgrosspay + CurLopAmtgrosspay;
                                }
                                lopbasamnt = (Convert.ToString(lopbasamnt) == "NaN") ? 0 : lopbasamnt;
                                loppaybandamnt = (Convert.ToString(loppaybandamnt) == "NaN") ? 0 : loppaybandamnt;
                                lopgpamnt = (Convert.ToString(lopgpamnt) == "NaN") ? 0 : lopgpamnt;

                                double preAllowAmt = 0;
                                double CurAllowAmt = 0;
                                double AllowAmt = 0;

                                //lopcount
                                //lopbasamnt = lopdays * oned_bassal;//17.01.18
                                //loppaybandamnt = lopdays * oned_paybandsal;//17.01.18
                                //lopgpamnt = lopdays * oned_gpsal;
                                if (islopfrmbas == "1")
                                {
                                    if (isabscal == "1" && absval > 0)
                                    {
                                        if (lopbasamnt <= bas_salary)
                                        {
                                            basicwlop = bas_salary - ((bas_salary / workdays) * (absdays * absval));
                                            lopamnt = lopamnt + ((bas_salary / workdays) * (absdays * absval));
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            basicwlop = actbasic - lopbasamnt;//22.01.18 barath change nec basicwlop = 0;
                                    }
                                    else
                                    {
                                        if (bas_salary >= lopbasamnt && ismanuallop.ToUpper() == "FALSE")//delsi2805
                                        {
                                            basicwlop = bas_salary - lopbasamnt;
                                            lopamnt = lopamnt + lopbasamnt;
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            basicwlop = actbasic - lopbasamnt;//22.01.18 barath change nec basicwlop = 0;
                                    }
                                }
                                else
                                    basicwlop = bas_salary;
                                TotalLopAmount += (bas_salary - basicwlop); // Added by jairam Lop Amount Calculation
                                //TotalLopAmount +=(actbasic - lopbasamnt);//22.01.18 barath change nec basicwlop = 0;

                                basicwlop = Math.Round(basicwlop, 0, MidpointRounding.AwayFromZero);
                                lopbasamnt = Math.Round(lopbasamnt, 0, MidpointRounding.AwayFromZero);
                                lopamnt = Math.Round(lopamnt, 0, MidpointRounding.AwayFromZero);
                                lopperday = Math.Round(lopperday, 0, MidpointRounding.AwayFromZero);
                                if (islopfrmpayband == "1")
                                {
                                    if (isabscal == "1" && absval > 0)
                                    {
                                        if (((cb_payband / workdays) * (absdays * absval)) <= cb_payband)
                                        {
                                            paybandwlop = cb_payband - ((cb_payband / workdays) * (absdays * absval));
                                            lopamnt = lopamnt + ((cb_payband / workdays) * (absdays * absval));
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            paybandwlop = 0;
                                    }
                                    else
                                    {
                                        if (cb_payband >= loppaybandamnt)
                                        {
                                            paybandwlop = cb_payband - loppaybandamnt;
                                            lopamnt = lopamnt + loppaybandamnt;
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            paybandwlop = 0;
                                    }
                                }
                                else
                                    paybandwlop = cb_payband;
                                paybandwlop = Math.Round(paybandwlop, 0, MidpointRounding.AwayFromZero);
                                loppaybandamnt = Math.Round(loppaybandamnt, 0, MidpointRounding.AwayFromZero);
                                lopamnt = Math.Round(lopamnt, 0, MidpointRounding.AwayFromZero);
                                lopperday = Math.Round(lopperday, 0, MidpointRounding.AwayFromZero);
                                if (islopfrmgp == "1")
                                {
                                    if (isabscal == "1" && absval > 0)
                                    {
                                        if (((grade_pay / workdays) * (absdays * absval)) <= grade_pay)
                                        {
                                            gradepaywlop = grade_pay - ((grade_pay / workdays) * (absdays * absval));
                                            lopamnt = lopamnt + ((grade_pay / workdays) * (absdays * absval));
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            gradepaywlop = 0;
                                    }
                                    else
                                    {
                                        if (lopgpamnt <= grade_pay)
                                        {
                                            gradepaywlop = grade_pay - lopgpamnt;
                                            lopamnt = lopamnt + lopgpamnt;
                                            if (lopdays != 0)
                                                lopperday = lopamnt / lopdays;
                                        }
                                        else
                                            gradepaywlop = 0;
                                    }
                                }
                                else
                                    gradepaywlop = grade_pay;
                                gradepaywlop = Math.Round(gradepaywlop, 0, MidpointRounding.AwayFromZero);
                                lopgpamnt = Math.Round(lopgpamnt, 0, MidpointRounding.AwayFromZero);
                                lopamnt = Math.Round(lopamnt, 0, MidpointRounding.AwayFromZero);
                                lopperday = Math.Round(lopperday, 0, MidpointRounding.AwayFromZero);
                                AllDedLopAmnt = lopamnt;
                                Hashtable hsgetall = new Hashtable();
                                hsgetall.Clear();
                                Double netsalary = 0;
                                Double actnetsal = 0;
                                Double deductwlop = 0;
                                DABasic = 0;
                                DAAmt = 0;
                                Double dawithlop = 0; // Allowance Condition
                                string format = "";
                                string ded_format = "";
                                Double totded = 0;
                                Double pfsal = 0;
                                Double esisal = 0;
                                Double pfSetAmnt = 0;
                                Double OrgPFAmnt = 0;
                                #region Allowances
                                allowence = ds.Tables[2].Rows[scd]["allowances"].ToString();
                                string[] slash_split = allowence.Split('\\');
                                for (int i = 0; i < slash_split.Length; i++)
                                {
                                    pers = 0;
                                    basicallow = 0;
                                    persallow = 0;
                                    lop_amou = 0;
                                    allow = "";
                                    slabvalue = 0;
                                    slablop = 0;
                                    slabamnt = 0;
                                    bool thirteen = false;
                                    string groupspl = slash_split[i].ToString();
                                    if (groupspl != "")
                                    {
                                        string[] semicol = groupspl.Split(';');
                                        if (semicol.Length >= 0)
                                        {
                                            if (semicol.Length >= 8)
                                            {
                                                #region Allowance Percent
                                                if (semicol[1] == "Percent")
                                                {
                                                    if (semicol[2].Trim() != "")
                                                    {
                                                        allow = Convert.ToString(semicol[0]);
                                                        allow = allow == "Da" ? "DA" : allow;//barath
                                                        Double.TryParse(semicol[2], out pers);
                                                        if (semicol[6] == "1")
                                                        {
                                                            if (semicol[4] == "1")
                                                                persallow = (pers / 100) * basicwlop;
                                                            else
                                                                persallow = (pers / 100) * bas_salary;
                                                            basicallow = (pers / 100) * bas_salary;
                                                        }
                                                        if (semicol.Length >= 9)
                                                        {
                                                            if (semicol[8] == "1")
                                                            {
                                                                if (semicol[4] == "1")
                                                                    persallow = (pers / 100) * (basicwlop + gradepaywlop);//grade_pay 27.12.17 barath
                                                                else
                                                                    persallow = pers / 100 * (bas_salary + grade_pay);//grade_pay
                                                                basicallow = (pers / 100) * (bas_salary + grade_pay);
                                                            }
                                                        }
                                                        if (semicol.Length >= 11)
                                                        {
                                                            if (semicol[10] == "1")
                                                            {
                                                                if (semicol[4] == "1")
                                                                    persallow = (pers / 100) * (basicwlop + gradepaywlop);//grade_pay 27.12.17 barath
                                                                else
                                                                    persallow = (pers / 100) * (bas_salary + grade_pay);
                                                                basicallow = (pers / 100) * (bas_salary + grade_pay);
                                                            }
                                                        }
                                                        if (semicol[4] == "1")
                                                        {
                                                            if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                lopamnt = AllDedLopAmnt + lopamnt;
                                                        }
                                                        DABasic = bas_salary + persallow;
                                                        if (semicol.Length >= 11)//barath 01.02.18
                                                        {
                                                            if (semicol[11] == ">=1" || semicol[11] == "<=5" )
                                                            {
                                                                persallow = Math.Ceiling(persallow);
                                                                basicallow = Math.Ceiling(basicallow);
                                                            }
                                                            else
                                                            {
                                                                persallow = Math.Round(persallow, 2, MidpointRounding.AwayFromZero);
                                                                basicallow = Math.Round(basicallow, 2, MidpointRounding.AwayFromZero);
                                                               
                                                            }
                                                        }
                                                        if (semicol.Length >= 13)//delsi05/05/2018
                                                        {
                                                            string[] splallamnt = semicol[13].Split('+');
                                                           
                                                            if (splallamnt.Length > 0)
                                                            {

                                                                if (splallamnt[0].Trim() != "")
                                                                {
                                                                    for (int newro = 0; newro < splallamnt.Length; newro++)
                                                                    {

                                                                        if (splallamnt[newro].Trim() != "")
                                                                        {
                                                                            thirteen = true;
                                                                         
                                                                            if (splallamnt[newro] == "Basic" || splallamnt[newro] == "Grade Pay")
                                                                            {

                                                                                if (splallamnt[newro] == "Basic")
                                                                                {
                                                                                    persallow = bas_salary;
                                                                                    basicallow = bas_salary;

                                                                                }
                                                                                else if (splallamnt[newro] == "Grade Pay")
                                                                                {
                                                                                    persallow = grade_pay;
                                                                                    basicallow = bas_salary;
                                                                                }

                                                                            }
                                                                            else
                                                                            {

                                                                                if (hsgetall.ContainsKey(Convert.ToString(splallamnt[newro]) == "Da" ? "DA" : splallamnt[newro]))
                                                                                {
                                                                                    Double allval = 0;
                                                                                    Double.TryParse(Convert.ToString(hsgetall[Convert.ToString(splallamnt[newro]) == "Da" ? "DA" : splallamnt[newro]]), out allval);

                                                                                     persallow =persallow+allval;
                                                                                     basicallow = basicallow + allval;
                                                                                   
                                                                                }
                                                                            
                                                                            }

                                                                        }

                                                                    }
                                                                    if (thirteen == true)
                                                                    {
                                                                        persallow = (pers / 100) * persallow;
                                                                        basicallow = (pers / 100) * basicallow;
                                                                    
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                    TotalLopAmount += (basicallow - persallow); // Added by jairam Lop Amount Calculation

                                                    persallow = Math.Round(persallow, 0, MidpointRounding.AwayFromZero);
                                                    basicallow = Math.Round(basicallow, 0, MidpointRounding.AwayFromZero);
                                                    if (allow.ToUpper() == "DA")
                                                    {
                                                        DAAmt = basicallow;
                                                        dawithlop = persallow;
                                                    }
                                                    if (format == "")
                                                    {
                                                        format = allow + ";" + "Percent" + ";" + Convert.ToString(pers) + "-" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
                                                        if (!hsgetall.ContainsKey(allow))
                                                            hsgetall.Add(allow, persallow);
                                                    }
                                                    else if (format != "")
                                                    {
                                                        format = format + allow + ";" + "Percent" + ";" + Convert.ToString(pers) + "-" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
                                                        if (!hsgetall.ContainsKey(allow))
                                                            hsgetall.Add(allow, persallow);
                                                    }
                                                }
                                                #endregion
                                                #region Allowance Amount
                                                else if (semicol[1] == "Amount")
                                                {
                                                    if (semicol[0] != "Slab")
                                                    {
                                                        allow = "";
                                                        allow = Convert.ToString(semicol[0]);
                                                        allow = (allow == "Da") ? "DA" : allow;//barath
                                                        Double AllLopAmnt = 0;
                                                        if (semicol[2].Trim() != "")
                                                        {
                                                            if (semicol[4].Trim() == "1")
                                                            {
                                                                if (differentMonthBool)//19.01.18
                                                                {
                                                                    preAllowAmt = (Convert.ToDouble(semicol[2]) / FmonDays) * prelop;
                                                                    CurAllowAmt = (Convert.ToDouble(semicol[2]) / SmonDays) * curlop;
                                                                    AllLopAmnt = preAllowAmt + CurAllowAmt;
                                                                }
                                                                else
                                                                    AllLopAmnt = (Convert.ToDouble(semicol[2]) / workdays) * lopdays;

                                                                if (Convert.ToDouble(semicol[2]) > AllLopAmnt)
                                                                {
                                                                    persallow = Convert.ToDouble(semicol[2]) - AllLopAmnt;
                                                                    basicallow = Convert.ToDouble(semicol[2]);
                                                                    if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                        lopamnt = AllDedLopAmnt + AllLopAmnt;
                                                                }
                                                                else
                                                                {
                                                                    basicallow = 0;
                                                                    persallow = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                persallow = Convert.ToDouble(semicol[2]);
                                                                basicallow = persallow;
                                                            }
                                                        }
                                                        if (semicol.Length >= 11)//barath 01.02.18
                                                        {
                                                            if (semicol[11] == ">=1" || semicol[11] == "<=5")
                                                            {
                                                                persallow = Math.Ceiling(persallow);
                                                                basicallow = Math.Ceiling(basicallow);
                                                            }
                                                            else
                                                            {
                                                                persallow = Math.Round(persallow, 2, MidpointRounding.AwayFromZero);
                                                                basicallow = Math.Round(basicallow, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        persallow = Math.Round(persallow, 0, MidpointRounding.AwayFromZero);
                                                        basicallow = Math.Round(basicallow, 0, MidpointRounding.AwayFromZero);
                                                        TotalLopAmount += (basicallow - persallow); // Added by jairam Lop Amount Calculation
                                                        if (allow.ToUpper() == "DA")
                                                        {
                                                            DAAmt = basicallow;
                                                            dawithlop = persallow;
                                                        }
                                                        string amnt = allow + ";" + "Amount" + ";" + Convert.ToString(persallow) + "-" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
                                                        if (format == "")
                                                        {
                                                            format = amnt;
                                                            if (!hsgetall.ContainsKey(allow))
                                                                hsgetall.Add(allow, persallow);
                                                        }
                                                        else
                                                        {
                                                            format = format + amnt;
                                                            if (!hsgetall.ContainsKey(allow))
                                                                hsgetall.Add(allow, persallow);
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Allowance Slab
                                                else if (semicol[1] == "Slab")
                                                {
                                                    allow = Convert.ToString(semicol[0]);
                                                    allow = allow == "Da" ? "DA" : allow;
                                                    Dictionary<string, string> mydic = new Dictionary<string, string>();
                                                    string amntformat = "";
                                                    if (allow.Trim() != "")
                                                    {
                                                        if (semicol[6] == "1")
                                                        {
                                                            if (semicol[4] == "1")
                                                            {
                                                                mydic = getdatas(allow, Convert.ToString(basicwlop), catcode);
                                                                amntformat = getamntformat(mydic, allow);
                                                                Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                persallow = lopamntcalc(amntformat, basicwlop, slabvalue);
                                                            }
                                                            else
                                                            {
                                                                mydic = getdatas(allow, Convert.ToString(bas_salary), catcode);
                                                                amntformat = getamntformat(mydic, allow);
                                                                Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                persallow = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                            }
                                                            basicallow = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                        }
                                                        if (semicol.Length >= 9)
                                                        {
                                                            if (semicol[8] == "1")
                                                            {
                                                                if (semicol[4] == "1")
                                                                {
                                                                    mydic = getdatas(allow, Convert.ToString(basicwlop + grade_pay), catcode);
                                                                    amntformat = getamntformat(mydic, allow);
                                                                    Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                    persallow = lopamntcalc(amntformat, (basicwlop + grade_pay), slabvalue);
                                                                }
                                                                else
                                                                {
                                                                    mydic = getdatas(allow, Convert.ToString(bas_salary + grade_pay), catcode);
                                                                    amntformat = getamntformat(mydic, allow);
                                                                    Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                    persallow = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                                }
                                                                basicallow = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                            }
                                                        }
                                                        if (semicol.Length >= 11)
                                                        {
                                                            if (semicol[10] == "1")
                                                            {
                                                                if (semicol[4] == "1")
                                                                {
                                                                    mydic = getdatas(allow, Convert.ToString(basicwlop + grade_pay), catcode);
                                                                    amntformat = getamntformat(mydic, allow);
                                                                    Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                    persallow = lopamntcalc(amntformat, (basicwlop + grade_pay), slabvalue);
                                                                }
                                                                else
                                                                {
                                                                    mydic = getdatas(allow, Convert.ToString(bas_salary + grade_pay), catcode);
                                                                    amntformat = getamntformat(mydic, allow);
                                                                    Double.TryParse(getamnt(mydic, allow), out slabvalue);
                                                                    persallow = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                                }
                                                                basicallow = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                            }
                                                        }
                                                        if (semicol[4] == "1")
                                                        {
                                                            if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                lopamnt = AllDedLopAmnt + lopamnt;
                                                        }
                                                        if (semicol.Length >= 11)//barath 01.02.18
                                                        {
                                                            if (semicol[11] == ">=1" || semicol[11] == "<=5")
                                                            {
                                                                persallow = Math.Ceiling(persallow);
                                                                basicallow = Math.Ceiling(basicallow);
                                                            }
                                                            else
                                                            {
                                                                persallow = Math.Round(persallow, 2, MidpointRounding.AwayFromZero);
                                                                basicallow = Math.Round(basicallow, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        DABasic = bas_salary + persallow;
                                                    }
                                                    persallow = Math.Round(persallow, 0, MidpointRounding.AwayFromZero);
                                                    basicallow = Math.Round(basicallow, 0, MidpointRounding.AwayFromZero);
                                                    TotalLopAmount += (basicallow - persallow); // Added by jairam Lop Amount Calculation
                                                    if (allow.ToUpper() == "DA")
                                                    {
                                                        DAAmt = basicallow;
                                                        dawithlop = persallow;
                                                    }
                                                    if (format == "")
                                                    {
                                                        format = allow + ";" + "Slab" + ";" + Convert.ToString(persallow) + "-" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
                                                        if (!hsgetall.ContainsKey(allow))
                                                            hsgetall.Add(allow, persallow);
                                                    }
                                                    else if (format != "")
                                                    {
                                                        format = format + allow + ";" + "Slab" + ";" + Convert.ToString(persallow) + "-" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
                                                        if (!hsgetall.ContainsKey(allow))
                                                            hsgetall.Add(allow, persallow);
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        totallow = totallow + persallow;
                                        totallowwolop = totallowwolop + basicallow;
                                    }
                                }
                                //Double totallowlop = 0;
                                //if (totallow > lopamnt)
                                //    totallowlop = totallow - lopamnt;
                                //else
                                //    totallowlop = totallow;
                                //Commented By Jeyaprakash on Apr 26th,2017
                                //Double dawithlop = 0;
                                //if (DAAmt > lopamnt)
                                //    dawithlop = DAAmt - lopamnt;
                                //else
                                //    dawithlop = DAAmt;
                                #endregion
                                if (isconsolid.Trim().ToUpper() == "TRUE")
                                {
                                    Gross_salary = Math.Round((bas_salary + totallowwolop), 0, MidpointRounding.AwayFromZero);
                                    if (differentMonthBool)//19.01.18
                                    {
                                        preAllowAmt = (Gross_salary / FmonDays) * prelop;
                                        CurAllowAmt = (Gross_salary / SmonDays) * curlop;
                                        lopgrossamnt = preAllowAmt + CurAllowAmt;
                                    }
                                    else
                                        lopgrossamnt = (Gross_salary / workdays) * lopdays;
                                    //lopgrossamnt = (Gross_salary / workdays) * lopdays;
                                    if (islopfrmgross == "1")
                                    {
                                        if (isabscal == "1" && absval > 0)
                                        {
                                            if (((Gross_salary / workdays) * (absdays * absval)) <= Gross_salary)
                                            {
                                                grosswlop = Gross_salary - ((Gross_salary / workdays) * (absdays * absval));
                                                lopamnt = lopamnt + ((Gross_salary / workdays) * (absdays * absval));
                                                if (lopdays != 0)
                                                    lopperday = lopamnt / lopdays;
                                            }
                                            else
                                                grosswlop = 0;
                                        }
                                        else
                                        {
                                            if (Gross_salary >= lopgrossamnt)
                                            {
                                                grosswlop = Gross_salary - lopgrossamnt;
                                                lopamnt = lopgrossamnt;
                                                lopperday = lopamnt / lopdays;
                                            }
                                            else
                                                grosswlop = 0;
                                        }
                                    }
                                    else
                                    {
                                        //lopamnt = ((bas_salary + totallowwolop) / workdays) * lopdays;
                                        lopamnt = TotalLopAmount;
                                        grosswlop = basicwlop + totallow;
                                    }

                                }
                                else
                                {
                                    Gross_salary = Math.Round((bas_salary + grade_pay + totallowwolop), 0, MidpointRounding.AwayFromZero);
                                    //lopgrossamnt = (Gross_salary / workdays) * lopdays;
                                    if (differentMonthBool)//19.01.18   delsi2004
                                    {
                                        if (daywise == "1")
                                        {
                                            preAllowAmt = (Gross_salary / workdays) * prelop;
                                            CurAllowAmt = (Gross_salary / workdays) * curlop;
                                            lopgrossamnt = preAllowAmt + CurAllowAmt;
                                        }
                                        else
                                        {
                                            preAllowAmt = (Gross_salary / FmonDays) * prelop;
                                            CurAllowAmt = (Gross_salary / SmonDays) * curlop;
                                            lopgrossamnt = preAllowAmt + CurAllowAmt;
                                        }
                                    }
                                    else
                                    {
                                        if (check_join == true)//delsi05/05/
                                        {
                                           
                                            lopgrossamnt = (Gross_salary / joinworkdays) * lopdays;
                                        }
                                        else
                                        {
                                            lopgrossamnt = (Gross_salary / workdays) * lopdays;
                                        }
                                    }
                                    if (islopfrmgross == "1")
                                    {
                                        //delsi3103
                                        if (isabscal == "1" && absval > 0)
                                        {
                                            if (absdays == 0 && lopdays != 0)
                                            {
                                                if (((Gross_salary / workdays) * (lopdays)) <= Gross_salary)
                                                {
                                                    grosswlop = Gross_salary - ((Gross_salary / workdays) * (lopdays));
                                                    lopamnt = lopamnt + ((Gross_salary / workdays) * (lopdays));
                                                    if (lopdays != 0)
                                                        lopperday = lopamnt / lopdays;
                                                    if (check_join == true)//delsi 0908
                                                    {
                                                        if (totallow != 0)
                                                        {
                                                            Gross_salary = Math.Round((actbasic + grade_pay + totallowwolop), 0, MidpointRounding.AwayFromZero);//delsi 0809
                                                            lopamnt = (Gross_salary / workdays) * lopdays;
                                                           
                                                            Gross_salary = (Gross_salary / workdays) * joinworkdays;
                                                            grosswlop = Gross_salary - lopamnt;
                                                            
                                                            if (lopdays != 0)
                                                                lopperday = lopamnt / lopdays;
                                                        }
                                                    
                                                    }
                                                }
                                                else
                                                    grosswlop = 0;
                                            }
                                            else if (absdays != 0 && lopdays != 0)//delsi3103
                                            {
                                                if (((Gross_salary / workdays) * (lopdays * absval)) <= Gross_salary)
                                                {
                                                    double totalabsday = (absdays * absval) + lopcount;
                                                    grosswlop = Gross_salary - ((Gross_salary / workdays) * totalabsday);
                                                    lopamnt = lopamnt + ((Gross_salary / workdays) * totalabsday);
                                                    if (lopdays != 0)
                                                        lopperday = lopamnt / lopdays;
                                                }
                                                else
                                                    grosswlop = 0;

                                            }
                                            //delsi3103
                                            else
                                            {
                                                if (((Gross_salary / workdays) * (absdays * absval)) <= Gross_salary)
                                                {
                                                    grosswlop = Gross_salary - ((Gross_salary / workdays) * (absdays * absval));
                                                    lopamnt = lopamnt + ((Gross_salary / workdays) * (absdays * absval));
                                                    if (lopdays != 0)
                                                        lopperday = lopamnt / lopdays;
                                                }
                                                else
                                                    grosswlop = 0;
                                            }
                                        }
                                        else
                                        {
                                            if (Gross_salary >= lopgrossamnt)
                                            {
                                                grosswlop = Gross_salary - lopgrossamnt;//delsiref27
                                                lopamnt = lopgrossamnt;
                                                lopperday = lopamnt / lopdays;
                                                lopfrombasic = true;//delsi2702
                                            }
                                            else
                                                grosswlop = 0;
                                        }
                                    }
                                    else
                                    {
                                        //lopamnt = ((bas_salary + grade_pay + totallowwolop) / workdays) * lopdays;
                                        lopamnt = TotalLopAmount;
                                        grosswlop = basicwlop + gradepaywlop + totallow;
                                    }
                                }
                                double rlamount = 0;
                                if (rlcount != 0)//delsi 2604
                                {
                                    if (oned_bassal != 0 && workdays != 0)
                                    {
                                        double totworkdayss = workdays + rlcount;
                                        double onedaysal = ((bas_salary + grade_pay + totallow) / totworkdayss); // added gradepay and totallow
                                        rlamount = onedaysal * rlcount;
                                        grosswlop = grosswlop - rlamount;

                                    }

                                }
                                string permonthLop = string.Empty;
                                int loppermonthcount = 0;
                                if (cb_permonth.Checked == true)//delsi0606
                                {

                                    permonthLop = GetSelectedItemsText(cbl_loppermonth, out loppermonthcount);
                                    foreach(string leaveTypes in category)
                                    {
                                        if (permonthLop.Trim().ToUpper() == leaveTypes.Trim().ToUpper())
                                        {
                                            if (workdays == lopdays)
                                            {
                                                grosswlop = 0;
                                                lopamnt = 0;
                                                Gross_salary = 0;
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                }

                                double nattcount = 0;

                                grosswlop = Math.Round(grosswlop, 0, MidpointRounding.AwayFromZero);
                                lopamnt = Math.Round(lopamnt, 0, MidpointRounding.AwayFromZero);
                                lopperday = Math.Round(lopperday, 0, MidpointRounding.AwayFromZero);
                                AllDedLopAmnt = lopamnt;
                                if (cb_lopfrom_atn.Checked == true && cb_lop_fromgross.Checked == false && cb_Lopfrom_basic.Checked == false)//delsi12/05
                                {
                                    if (absdays != 0)

                                    if (Gross_salary >= lopgrossamnt)
                                    {
                                        grosswlop = Gross_salary - lopgrossamnt;
                                        lopamnt = lopgrossamnt;
                                        lopperday = lopamnt / lopdays;
                                       
                                    }
                                    else
                                        grosswlop = 0;

                                }
                                if (check_join == true)//delsi1106
                                {
                                    if(ismanuallop.ToUpper() != "TRUE")//delsi 2407
                                    basicwlop = actbasic;
                                }

                                //Double dawithlop = 0;
                                //if (DAAmt > lopamnt)
                                //    dawithlop = DAAmt - lopamnt;
                                //else
                                //    dawithlop = DAAmt;
                                if (grosswlop != 0)//Gross_salary grosswlop != 0) /*only if condition added by poomalar 12.10.17*/
                                {
                                    #region Deduction
                                    deduction = ds.Tables[2].Rows[scd]["deductions"].ToString();
                                    string[] slash1_split = deduction.Split('\\');
                                    string[] splautoval = strautoded.Split(',');

                                    for (int i = 0; i < slash1_split.Length; i++)
                                    {
                                        Double frmtot = 0;
                                        Double deduct1 = 0;
                                        Double pers_ded1 = 0;
                                        string netval = "";
                                        string dedtion = "";
                                        bool entryfifteen = false;
                                        string ded_spl = slash1_split[i].ToString();
                                        if (ded_spl != "" && ded_spl.Contains(';'))
                                        {
                                            string[] semicol1 = ded_spl.Split(';');
                                            string[] newsemicol1 = semicol1;
                                            if (semicol1.Length >= 15)
                                            {
                                                dedtion = Convert.ToString(semicol1[0]);
                                                if (!splautoval.Contains(dedtion))
                                                {
                                                    #region Deduction Percentage
                                                    if (semicol1[1] == "Percent")
                                                    {
                                                        if (semicol1[2].Trim() != "")
                                                        {
                                                            if (semicol1.Length >= 20)
                                                                netval = Convert.ToString(semicol1[19]);
                                                            pers_ded1 = Convert.ToDouble(semicol1[2]);
                                                            if (semicol1[3] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = grosswlop;
                                                                    else
                                                                        frmtot = Gross_salary;
                                                                    // deduct1 = frmtot;
                                                                    deduct1 = Gross_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        OrgPFAmnt = Gross_salary;
                                                                }
                                                                else
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = (pers_ded1 / 100) * grosswlop;
                                                                    else
                                                                        frmtot = (pers_ded1 / 100) * Gross_salary;
                                                                    //deduct1 = frmtot;
                                                                    deduct1 = (pers_ded1 / 100) * Gross_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        OrgPFAmnt = (pers_ded1 / 100) * Gross_salary;
                                                                }
                                                                if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    pfSetAmnt = Gross_salary;
                                                            }
                                                            if (semicol1[4] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + daval;
                                                                        else
                                                                            frmtot = bas_salary + daval;
                                                                        // deduct1 = frmtot;
                                                                        deduct1 = bas_salary + daval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + daval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop;
                                                                        else
                                                                            frmtot = bas_salary;
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = bas_salary;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * (basicwlop + daval);
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * (bas_salary + daval);
                                                                        // deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * (bas_salary + daval);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + daval;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (bas_salary + daval);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * (basicwlop);
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * (bas_salary);
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * (bas_salary);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (bas_salary);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[7] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + grade_pay + daval;
                                                                        else
                                                                            frmtot = bas_salary + grade_pay + daval;
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = bas_salary + grade_pay + daval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + grade_pay + daval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + grade_pay;
                                                                        else
                                                                            frmtot = bas_salary + grade_pay;
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = bas_salary + grade_pay;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + grade_pay;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * (basicwlop + gradepaywlop + daval);//grade_pay 27.12.17 barath
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * (bas_salary + grade_pay + daval);
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * (bas_salary + grade_pay + daval);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + grade_pay + daval;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (bas_salary + grade_pay + daval);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * (basicwlop + grade_pay);
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * (bas_salary + grade_pay);
                                                                        // deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * (bas_salary + grade_pay);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + grade_pay;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (bas_salary + grade_pay);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[8] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = basicwlop;
                                                                    else
                                                                        frmtot = bas_salary;
                                                                    // deduct1 = frmtot;
                                                                    deduct1 = bas_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = deduct1;
                                                                        OrgPFAmnt = bas_salary;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = (pers_ded1 / 100) * basicwlop;
                                                                    else
                                                                        frmtot = (pers_ded1 / 100) * bas_salary;
                                                                    //deduct1 = frmtot;
                                                                    deduct1 = (pers_ded1 / 100) * bas_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = bas_salary;
                                                                        OrgPFAmnt = (pers_ded1 / 100) * bas_salary;
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[9] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DP"))
                                                                    {
                                                                        Double dpval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DP"]), out dpval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + dpval;
                                                                        else
                                                                            frmtot = bas_salary + dpval;
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = bas_salary + dpval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + dpval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop;
                                                                        else
                                                                            frmtot = bas_salary;
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = bas_salary;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DP"))
                                                                    {
                                                                        Double dpval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DP"]), out dpval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * (basicwlop + dpval);
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * (bas_salary + dpval);
                                                                        //deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * (bas_salary + dpval);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + dpval;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (bas_salary + dpval);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = (pers_ded1 / 100) * basicwlop;
                                                                        else
                                                                            frmtot = (pers_ded1 / 100) * bas_salary;
                                                                        // deduct1 = frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * bas_salary;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * bas_salary;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[10] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("Petty"))
                                                                    {
                                                                        Double pettyval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["Petty"]), out pettyval);
                                                                        frmtot = pettyval;
                                                                        deduct1 = frmtot;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = pettyval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = 0;
                                                                        deduct1 = 0;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = deduct1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("Petty"))
                                                                    {
                                                                        Double pettyval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["Petty"]), out pettyval);
                                                                        frmtot = (pers_ded1 / 100) * (pettyval);
                                                                        deduct1 = frmtot;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = pettyval;
                                                                            OrgPFAmnt = (pers_ded1 / 100) * (pettyval);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = 0;
                                                                        deduct1 = 0;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = deduct1;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1.Length >= 16)
                                                            {
                                                                if (semicol1[15] == "1")
                                                                {
                                                                    if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                    {
                                                                        if (hsgetall.ContainsKey("Arrear"))
                                                                        {
                                                                            Double arrval = 0;
                                                                            Double.TryParse(Convert.ToString(hsgetall["Arrear"]), out arrval);
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = basicwlop + arrval;
                                                                            else
                                                                                frmtot = bas_salary + arrval;
                                                                            // deduct1 = frmtot;
                                                                            deduct1 = bas_salary + arrval;
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = deduct1;
                                                                                OrgPFAmnt = bas_salary + arrval;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = basicwlop;
                                                                            else
                                                                                frmtot = bas_salary;
                                                                            // deduct1 = frmtot;
                                                                            deduct1 = bas_salary;
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = deduct1;
                                                                                OrgPFAmnt = bas_salary;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (hsgetall.ContainsKey("Arrear"))
                                                                        {
                                                                            Double arrval = 0;
                                                                            Double.TryParse(Convert.ToString(hsgetall["Arrear"]), out arrval);
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = (pers_ded1 / 100) * (basicwlop + arrval);
                                                                            else
                                                                                frmtot = (pers_ded1 / 100) * (bas_salary + arrval);
                                                                            // deduct1 = frmtot;
                                                                            deduct1 = (pers_ded1 / 100) * (bas_salary + arrval);
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = bas_salary + arrval;
                                                                                OrgPFAmnt = (pers_ded1 / 100) * (bas_salary + arrval);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = (pers_ded1 / 100) * basicwlop;
                                                                            else
                                                                                frmtot = (pers_ded1 / 100) * bas_salary;
                                                                            //deduct1 = frmtot;
                                                                            deduct1 = (pers_ded1 / 100) * bas_salary;
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = bas_salary;
                                                                                OrgPFAmnt = (pers_ded1 / 100) * bas_salary;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1.Length >= 18)
                                                            {
                                                                double myDed = 0;
                                                                string[] splallamnt = semicol1[17].Split('+');
                                                                if (splallamnt.Length > 0)
                                                                {
                                                                    if (splallamnt[0].Trim() != "")//barath19.01.18
                                                                    {
                                                                        frmtot = 0; //barath19.01.18
                                                                        deduct1 = 0;
                                                                        for (int newro = 0; newro < splallamnt.Length; newro++)
                                                                        {
                                                                            if (splallamnt[newro].Trim() != "")
                                                                            {
                                                                                entryfifteen = true;
                                                                                if (splallamnt[newro] == "Basic" || splallamnt[newro] == "Grade Pay")
                                                                                {
                                                                                    if (splallamnt[newro] == "Basic")
                                                                                    {
                                                                                        if (semicol1[6] == "1")
                                                                                        {
                                                                                            frmtot = frmtot + basicwlop;
                                                                                            deduct1 = deduct1 + bas_salary;
                                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                            {
                                                                                                myDed = myDed + bas_salary;
                                                                                                OrgPFAmnt = myDed;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            frmtot = frmtot + bas_salary;
                                                                                            deduct1 = deduct1 + bas_salary;
                                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                                OrgPFAmnt = deduct1;
                                                                                        }
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                            pfSetAmnt = deduct1;
                                                                                    }
                                                                                    else if (splallamnt[newro] == "Grade Pay")
                                                                                    {
                                                                                        frmtot = frmtot + gradepaywlop;
                                                                                        deduct1 = deduct1 + grade_pay;
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                        {
                                                                                            pfSetAmnt = deduct1;
                                                                                            OrgPFAmnt = deduct1;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (hsgetall.ContainsKey(Convert.ToString(splallamnt[newro]) == "Da" ? "DA" : splallamnt[newro]))
                                                                                    {
                                                                                        Double allval = 0;
                                                                                        Double.TryParse(Convert.ToString(hsgetall[Convert.ToString(splallamnt[newro]) == "Da" ? "DA" : splallamnt[newro]]), out allval);
                                                                                        frmtot = frmtot + allval;
                                                                                        deduct1 = deduct1 + allval;
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                        {
                                                                                            pfSetAmnt = deduct1;
                                                                                            OrgPFAmnt = deduct1;//barath 19.01.18
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (entryfifteen == true)
                                                                        {
                                                                            if (semicol1[12].Trim() != "1" || netval.Trim() != "1")
                                                                            {
                                                                                frmtot = (pers_ded1 / 100) * frmtot;
                                                                                deduct1 = (pers_ded1 / 100) * deduct1;
                                                                                if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                                    OrgPFAmnt = (pers_ded1 / 100) * OrgPFAmnt;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[6] == "1")
                                                            {
                                                                if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                {
                                                                    if (pfSetAmnt > lopamnt)
                                                                        pfSetAmnt = pfSetAmnt - lopamnt;
                                                                    else
                                                                        pfSetAmnt = 0;
                                                                }
                                                                if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                    lopamnt = AllDedLopAmnt + lopamnt;
                                                            }
                                                            if (semicol1[12] == "1")
                                                            {
                                                                if (semicol1[13].Trim() != "")
                                                                {
                                                                    Double newval = 0;
                                                                    Double.TryParse(Convert.ToString(semicol1[13]), out newval);
                                                                    if (frmtot < newval)
                                                                    {
                                                                        frmtot = (pers_ded1 / 100) * frmtot;
                                                                        deduct1 = (pers_ded1 / 100) * deduct1;
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = (pers_ded1 / 100) * newval;
                                                                        deduct1 = (pers_ded1 / 100) * newval;//barath deduct1;
                                                                    }
                                                                    if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                        OrgPFAmnt = (pers_ded1 / 100) * OrgPFAmnt;
                                                                }
                                                            }
                                                            if (netval.Trim() == "1")
                                                            {
                                                                frmtot = (pers_ded1 / 100) * frmtot;
                                                                deduct1 = (pers_ded1 / 100) * deduct1;
                                                                if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                    OrgPFAmnt = (pers_ded1 / 100) * OrgPFAmnt;
                                                            }
                                                        }
                                                        if (semicol1.Length >= 11)//barath 01.02.18
                                                        {
                                                            if (semicol1[11] == ">=1" || semicol1[11] == "<=5")
                                                            {
                                                                frmtot = Math.Ceiling(frmtot);
                                                                deduct1 = Math.Ceiling(deduct1);
                                                            }
                                                            else
                                                            {
                                                                frmtot = Math.Round(frmtot, 2, MidpointRounding.AwayFromZero);
                                                                deduct1 = Math.Round(deduct1, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        frmtot = Math.Round(frmtot, 0, MidpointRounding.AwayFromZero);
                                                        pers_ded1 = Math.Round(pers_ded1, 0, MidpointRounding.AwayFromZero);
                                                        deduct1 = Math.Round(deduct1, 0, MidpointRounding.AwayFromZero);
                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                        {
                                                            pfamnt = OrgPFAmnt;
                                                            pfsal = frmtot;
                                                        }
                                                        if (ded_format.Trim() == "")
                                                            ded_format = Convert.ToString(dedtion) + ";" + "Percent" + ";" + Convert.ToString(pers_ded1) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                        else
                                                            ded_format = ded_format + Convert.ToString(dedtion) + ";" + "Percent" + ";" + Convert.ToString(pers_ded1) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                    }
                                                    #endregion
                                                    #region Deduction Amount
                                                    else if (semicol1[1] == "Amount")
                                                    {
                                                        Double DedLopAmnt = 0;
                                                        if (Convert.ToString(semicol1[2]).Trim() != "")
                                                        {
                                                            DedLopAmnt = (Convert.ToDouble(semicol1[2]) / workdays) * lopdays;
                                                            if (semicol1[6] == "1")
                                                            {
                                                                if (Convert.ToDouble(semicol1[2]) > DedLopAmnt)
                                                                {
                                                                    frmtot = Convert.ToDouble(semicol1[2]) - DedLopAmnt;
                                                                    deduct1 = Convert.ToDouble(semicol1[2]);
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = frmtot;
                                                                        OrgPFAmnt = deduct1;
                                                                    }
                                                                    if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                        lopamnt = AllDedLopAmnt + DedLopAmnt;
                                                                }
                                                                else
                                                                {
                                                                    frmtot = 0;
                                                                    deduct1 = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //frmtot = Convert.ToDouble(semicol1[2]);
                                                                //deduct1 = frmtot;
                                                                if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                {
                                                                    frmtot = Convert.ToDouble(semicol1[2]);
                                                                    deduct1 = frmtot;
                                                                    pfSetAmnt = frmtot;
                                                                    OrgPFAmnt = frmtot;
                                                                }
                                                                //barath 14.11.17
                                                                else if (dedtion.Trim().ToUpper() == "P.TAX" || dedtion.Trim().ToUpper() == "P TAX" || dedtion.Trim().ToUpper() == "PROFESSIONAL TAX" || dedtion.Trim().ToUpper() == "PROFTAX")
                                                                {
                                                                    if (semicol1.Length > 20 && isptgrosslop == "1")
                                                                    {
                                                                        if (semicol1[20].Trim() == "1" && isptgrosslop == "1" && (dedtion.Trim().ToUpper() == "P.TAX" || dedtion.Trim().ToUpper() == "P TAX" || dedtion.Trim().ToUpper() == "PROFESSIONAL TAX" || dedtion.Trim().ToUpper() == "PROFTAX"))
                                                                        {
                                                                            string query = "select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + ddlcollege.SelectedItem.Value + "' and user_code ='" + usercode + "'";
                                                                            string sql = d2.GetFunction(query).Trim();
                                                                            string ptstmonth = string.Empty;
                                                                            string ptendmonth = string.Empty;
                                                                            string ptstyear = string.Empty;
                                                                            string ptendyear = string.Empty;
                                                                            if (sql != " ")
                                                                            {
                                                                                string[] det = sql.Split(';');
                                                                                string monthyear = det[0];
                                                                                string endmonthyear = det[1];
                                                                                ptstmonth = monthyear.Split('-')[0];
                                                                                ptendmonth = endmonthyear.Split('-')[0];
                                                                                ptstyear = monthyear.Split('-')[1];
                                                                                ptendyear = endmonthyear.Split('-')[1];
                                                                            }
                                                                            if (month1 == ptstmonth && year1 == ptstyear || month1 == ptendmonth && year1 == ptendyear)
                                                                            {
                                                                                if (semicol1.Length > 20)
                                                                                {
                                                                                    if (semicol1[20].Trim() == "1" && isptgrosslop == "1" && (dedtion.Trim().ToUpper() == "P.TAX" || dedtion.Trim().ToUpper() == "P TAX" || dedtion.Trim().ToUpper() == "PROFESSIONAL TAX" || dedtion.Trim().ToUpper() == "PROFTAX"))
                                                                                    {
                                                                                        frmtot = Convert.ToDouble(semicol1[2]);
                                                                                        deduct1 = frmtot;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                frmtot = 0;
                                                                                deduct1 = 0;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            frmtot = Convert.ToDouble(semicol1[2]);
                                                                            deduct1 = frmtot;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = Convert.ToDouble(semicol1[2]);
                                                                        deduct1 = frmtot;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    frmtot = Convert.ToDouble(semicol1[2]);
                                                                    deduct1 = frmtot;
                                                                }
                                                            }
                                                        }
                                                        frmtot = Math.Round(frmtot, 0, MidpointRounding.AwayFromZero);
                                                        pers_ded1 = Math.Round(pers_ded1, 0, MidpointRounding.AwayFromZero);
                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                        {
                                                            pfamnt = OrgPFAmnt;
                                                            pfsal = frmtot;
                                                        }
                                                        if (ded_format == "")
                                                            ded_format = dedtion + ";" + "Amount" + ";" + Convert.ToString(frmtot) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                        else
                                                            ded_format = ded_format + dedtion + ";" + "Amount" + ";" + Convert.ToString(frmtot) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                    }
                                                    #endregion
                                                    #region Deduction Slap
                                                    else if (semicol1[1] == "Slab")
                                                    {
                                                        allow = Convert.ToString(semicol1[0]);
                                                        Dictionary<string, string> mydic = new Dictionary<string, string>();
                                                        string amntformat = "";
                                                        if (allow.Trim() != "")
                                                        {
                                                            if (semicol1.Length >= 20)
                                                                netval = Convert.ToString(semicol1[19]);
                                                            if (semicol1[3] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = grosswlop;
                                                                    else
                                                                        frmtot = Gross_salary;
                                                                    deduct1 = Gross_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = deduct1;
                                                                        OrgPFAmnt = Gross_salary;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(grosswlop), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, grosswlop, slabvalue);
                                                                    }
                                                                    else
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(Gross_salary), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, Gross_salary, slabvalue);
                                                                    }
                                                                    deduct1 = lopamntcalc(amntformat, Gross_salary, slabvalue);
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = Gross_salary;
                                                                        mydic = getdatas(dedtion, Convert.ToString(Gross_salary), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        OrgPFAmnt = lopamntcalc(amntformat, Gross_salary, slabvalue);
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[4] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + daval;
                                                                        else
                                                                            frmtot = bas_salary + daval;
                                                                        deduct1 = bas_salary + daval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + daval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop;
                                                                        else
                                                                            frmtot = bas_salary;
                                                                        deduct1 = bas_salary;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop + daval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (basicwlop + daval), slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + daval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (bas_salary + daval), slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, (bas_salary + daval), slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + daval;
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + daval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, (bas_salary + daval), slabvalue);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, basicwlop, slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        }
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            pfSetAmnt = bas_salary;
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[7] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + gradepaywlop + daval;//grade_pay 27.12.17 barath
                                                                        else
                                                                            frmtot = bas_salary + grade_pay + daval;
                                                                        deduct1 = bas_salary + grade_pay + daval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + grade_pay + daval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + grade_pay;
                                                                        else
                                                                            frmtot = bas_salary + grade_pay;
                                                                        deduct1 = bas_salary + grade_pay;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + grade_pay;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DA"))
                                                                    {
                                                                        Double daval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DA"]), out daval);
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop + gradepaywlop + daval), catcode);//grade_pay 27.12.17 barath
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (basicwlop + grade_pay + daval), slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + grade_pay + daval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (bas_salary + grade_pay + daval), slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, (bas_salary + grade_pay + daval), slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + grade_pay + daval;
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + grade_pay + daval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, (bas_salary + grade_pay + daval), slabvalue);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop + grade_pay), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (basicwlop + grade_pay), slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + grade_pay), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + grade_pay;
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + grade_pay), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, (bas_salary + grade_pay), slabvalue);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[8] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                        frmtot = basicwlop;
                                                                    else
                                                                        frmtot = bas_salary;
                                                                    deduct1 = bas_salary;
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = deduct1;
                                                                        OrgPFAmnt = bas_salary;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (semicol1[6] == "1")
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(basicwlop), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, basicwlop, slabvalue);
                                                                    }
                                                                    else
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                    }
                                                                    deduct1 = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                    {
                                                                        pfSetAmnt = bas_salary;
                                                                        mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        OrgPFAmnt = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[9] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("DP"))
                                                                    {
                                                                        Double dpval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DP"]), out dpval);
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop + dpval;
                                                                        else
                                                                            frmtot = bas_salary + dpval;
                                                                        deduct1 = bas_salary + dpval;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary + dpval;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                            frmtot = basicwlop;
                                                                        else
                                                                            frmtot = bas_salary;
                                                                        deduct1 = bas_salary;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = bas_salary;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("DP"))
                                                                    {
                                                                        Double dpval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["DP"]), out dpval);
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop + dpval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (basicwlop + dpval), slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + dpval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, (bas_salary + dpval), slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, (bas_salary + dpval), slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary + dpval;
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary + dpval), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, (bas_salary + dpval), slabvalue);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (semicol1[6] == "1")
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(basicwlop), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, basicwlop, slabvalue);
                                                                        }
                                                                        else
                                                                        {
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            frmtot = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        }
                                                                        deduct1 = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = bas_salary;
                                                                            mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                            OrgPFAmnt = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[10] == "1")
                                                            {
                                                                if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                {
                                                                    if (hsgetall.ContainsKey("Petty"))
                                                                    {
                                                                        Double pettyval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["Petty"]), out pettyval);
                                                                        frmtot = pettyval;
                                                                        deduct1 = frmtot;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = frmtot;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = 0;
                                                                        deduct1 = 0;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = deduct1;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hsgetall.ContainsKey("Petty"))
                                                                    {
                                                                        Double pettyval = 0;
                                                                        Double.TryParse(Convert.ToString(hsgetall["Petty"]), out pettyval);
                                                                        mydic = getdatas(dedtion, Convert.ToString(pettyval), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, pettyval, slabvalue);
                                                                        deduct1 = frmtot;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = pettyval;
                                                                            OrgPFAmnt = deduct1;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        frmtot = 0;
                                                                        deduct1 = 0;
                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                        {
                                                                            pfSetAmnt = deduct1;
                                                                            OrgPFAmnt = deduct1;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1.Length >= 16)
                                                            {
                                                                if (semicol1[15] == "1")
                                                                {
                                                                    if (semicol1[12].Trim() == "1" || netval.Trim() == "1")
                                                                    {
                                                                        if (hsgetall.ContainsKey("Arrear"))
                                                                        {
                                                                            Double arrval = 0;
                                                                            Double.TryParse(Convert.ToString(hsgetall["Arrear"]), out arrval);
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = basicwlop + arrval;
                                                                            else
                                                                                frmtot = bas_salary + arrval;
                                                                            deduct1 = bas_salary + arrval;
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = deduct1;
                                                                                OrgPFAmnt = bas_salary + arrval;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (semicol1[6] == "1")
                                                                                frmtot = basicwlop;
                                                                            else
                                                                                frmtot = bas_salary;
                                                                            deduct1 = bas_salary;
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = deduct1;
                                                                                OrgPFAmnt = bas_salary;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (hsgetall.ContainsKey("Arrear"))
                                                                        {
                                                                            Double arrval = 0;
                                                                            Double.TryParse(Convert.ToString(hsgetall["Arrear"]), out arrval);
                                                                            if (semicol1[6] == "1")
                                                                            {
                                                                                mydic = getdatas(dedtion, Convert.ToString(basicwlop + arrval), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                frmtot = lopamntcalc(amntformat, (basicwlop + arrval), slabvalue);
                                                                            }
                                                                            else
                                                                            {
                                                                                mydic = getdatas(dedtion, Convert.ToString(bas_salary + arrval), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                frmtot = lopamntcalc(amntformat, (bas_salary + arrval), slabvalue);
                                                                            }
                                                                            deduct1 = lopamntcalc(amntformat, (bas_salary + arrval), slabvalue);
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = bas_salary + arrval;
                                                                                mydic = getdatas(dedtion, Convert.ToString(bas_salary + arrval), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                OrgPFAmnt = lopamntcalc(amntformat, (bas_salary + arrval), slabvalue);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (semicol1[6] == "1")
                                                                            {
                                                                                mydic = getdatas(dedtion, Convert.ToString(basicwlop), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                frmtot = lopamntcalc(amntformat, basicwlop, slabvalue);
                                                                            }
                                                                            else
                                                                            {
                                                                                mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                frmtot = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                            }
                                                                            deduct1 = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                            {
                                                                                pfSetAmnt = bas_salary;
                                                                                mydic = getdatas(dedtion, Convert.ToString(bas_salary), catcode);
                                                                                amntformat = getamntformat(mydic, dedtion);
                                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                OrgPFAmnt = lopamntcalc(amntformat, bas_salary, slabvalue);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1.Length >= 18)
                                                            {
                                                                double myDedAmnt = 0;
                                                                string[] splallamnt = semicol1[17].Split('+');
                                                                string checkGrossMinusLop = string.Empty;
                                                                if (semicol1.Length > 21)
                                                                {

                                                                    checkGrossMinusLop = Convert.ToString(semicol1[21]);
                                                                }
                                                                if (splallamnt.Length > 0)
                                                                {
                                                                    if (splallamnt[0].Trim() != "")//barath19.01.18
                                                                    {
                                                                        frmtot = 0; //barath19.01.18
                                                                        deduct1 = 0;//
                                                                        for (int newro = 0; newro < splallamnt.Length; newro++)
                                                                        {
                                                                            if (splallamnt[newro].Trim() != "")
                                                                            {
                                                                                entryfifteen = true;
                                                                                if (splallamnt[newro] == "Basic" || splallamnt[newro] == "Grade Pay")
                                                                                {
                                                                                    if (splallamnt[newro] == "Basic")
                                                                                    {
                                                                                        if (semicol1[6] == "1")
                                                                                        {
                                                                                            frmtot = frmtot + basicwlop;
                                                                                            deduct1 = deduct1 + bas_salary;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            frmtot = frmtot + bas_salary;
                                                                                            deduct1 = deduct1 + bas_salary;
                                                                                        }
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                        {
                                                                                            pfSetAmnt = deduct1;
                                                                                            myDedAmnt = myDedAmnt + bas_salary;
                                                                                            OrgPFAmnt = myDedAmnt;
                                                                                        }
                                                                                    }
                                                                                    else if (splallamnt[newro] == "Grade Pay")
                                                                                    {
                                                                                        frmtot = frmtot + gradepaywlop;
                                                                                        deduct1 = deduct1 + grade_pay;
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                        {
                                                                                            pfSetAmnt = deduct1;
                                                                                            OrgPFAmnt = deduct1;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (hsgetall.ContainsKey(splallamnt[newro]))
                                                                                    {
                                                                                        Double allval = 0;
                                                                                        Double.TryParse(Convert.ToString(hsgetall[splallamnt[newro]]), out allval);
                                                                                        frmtot = frmtot + allval;
                                                                                        deduct1 = deduct1 + allval;
                                                                                        if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                                        {
                                                                                            pfSetAmnt = deduct1;
                                                                                            OrgPFAmnt = deduct1;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (entryfifteen == true)
                                                                        {
                                                                            if (semicol1[12].Trim() != "1" || netval.Trim() != "1")
                                                                            {

                                                                                if (checkGrossMinusLop == "1")//delsi1604
                                                                                {
                                                                                    string coll_code = Convert.ToString(ddlcollege.SelectedItem.Value);

                                                                                    double startRange = 0;
                                                                                    string selq = "select slabvalue,slabtype from pfslabs where SlabFor='" + dedtion + "' and '" + frmtot + "' between salfrom and salto and category_code='" + catcode + "' and college_code='" + coll_code + "'";
                                                                                    DataSet dsnewdt = new DataSet();
                                                                                    dsnewdt.Clear();
                                                                                    dsnewdt = d2.select_method_wo_parameter(selq, "Text");
                                                                                    if (dsnewdt.Tables.Count > 0 && dsnewdt.Tables[0].Rows.Count > 0)
                                                                                    {
                                                                                        amntformat = Convert.ToString(dsnewdt.Tables[0].Rows[0]["slabtype"]);


                                                                                    }
                                                                                    if (amntformat == "Amount")
                                                                                    {
                                                                                       // frmtot = frmtot - lopamnt;
                                                                                        double callop = lopamnt;//delsi 1809
                                                                                        double lopfromtot = 0;
                                                                                        if (lopamnt != 0)
                                                                                        {
                                                                                            lopfromtot = (frmtot / workdays);//delsi 1809
                                                                                            lopfromtot = lopfromtot * lopdays;//delsi 10012018

                                                                                        }
                                                                                        
                                                                                        frmtot = frmtot - lopfromtot;
                                                                                        frmtot = Math.Round(frmtot, 0, MidpointRounding.AwayFromZero);

                                                                                        string fromrangeVal = d2.GetFunction("select salfrom from pfslabs where SlabFor='" + dedtion + "' and category_code='" + catcode + "' and college_code='" + coll_code + "'");


                                                                                        string[] splitval = fromrangeVal.Split('.');
                                                                                        if (splitval.Length > 0)
                                                                                            Double.TryParse(Convert.ToString(splitval[0]), out startRange);

                                                                                        if (frmtot < startRange)
                                                                                        {
                                                                                            mydic = getdatasval(dedtion, Convert.ToString(frmtot), catcode);
                                                                                            amntformat = getamntformat(mydic, dedtion);

                                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                            frmtot = lopamntcalc(amntformat, frmtot, slabvalue);

                                                                                            deduct1 = frmtot;

                                                                                        }
                                                                                        else
                                                                                        {

                                                                                            mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                            frmtot = lopamntcalc(amntformat, frmtot, slabvalue);

                                                                                            deduct1 = frmtot;

                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        double callop = lopamnt;//delsi 1809
                                                                                        double lopfromtot = 0;
                                                                                        if (lopamnt != 0)
                                                                                        {
                                                                                            lopfromtot = (frmtot / workdays);//delsi 1809
                                                                                            lopfromtot = lopfromtot * lopdays;//delsi 10012018
                                                                                        
                                                                                        }
                                                                                        
                                                                                        frmtot = frmtot - lopfromtot;
                                                                                        frmtot = Math.Round(frmtot, 0, MidpointRounding.AwayFromZero);
                                                                                      //  frmtot = frmtot - lopamnt;
                                                                                        mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                        frmtot = lopamntcalc(amntformat, frmtot, slabvalue);

                                                                                        deduct1 = frmtot;

                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                                    amntformat = getamntformat(mydic, dedtion);
                                                                                    Double.TryParse(getamnt(mydic, dedtion), out slabvalue);

                                                                                    if (cb_pfCalculation.Checked == true)//delsi18/05
                                                                                    {
                                                                                        if (amntformat.ToUpper() == "PERCENT" && lopamnt != 0)
                                                                                        {
                                                                                            double basictalop = (deduct1 / workdays);
                                                                                            double overallamount = deduct1 - (basictalop * lopdays);

                                                                                            frmtot = overallamount;
                                                                                            deduct1 = overallamount;

                                                                                        }

                                                                                        if (amntformat.ToUpper() == "AMOUNT" && lopamnt != 0)
                                                                                        {
                                                                                            frmtot = frmtot - lopamnt;
                                                                                            mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                                            amntformat = getamntformat(mydic, dedtion);
                                                                                            Double.TryParse(getamnt(mydic, dedtion), out slabvalue);

                                                                                            if (amntformat.ToUpper() == "PERCENT" && lopamnt != 0)
                                                                                            {
                                                                                                double basictalop = (deduct1 / workdays);
                                                                                                double overallamount = deduct1 - (basictalop * lopdays);

                                                                                                frmtot = overallamount;
                                                                                                deduct1 = overallamount;

                                                                                            }

                                                                                        
                                                                                        }

                                                                                    }
                                                                                    frmtot = lopamntcalc(amntformat, frmtot, slabvalue);

                                                                                    deduct1 = frmtot;

                                                                                }


                                                                                if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                                {
                                                                                    mydic = getdatas(dedtion, Convert.ToString(OrgPFAmnt), catcode);
                                                                                    amntformat = getamntformat(mydic, dedtion);
                                                                                    Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                                    OrgPFAmnt = lopamntcalc(amntformat, OrgPFAmnt, slabvalue);
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1[6] == "1")
                                                            {
                                                                if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                                {
                                                                    if (pfSetAmnt > lopamnt)
                                                                        pfSetAmnt = pfSetAmnt - lopamnt;
                                                                    else
                                                                        pfSetAmnt = 0;
                                                                }
                                                                if (isIncAllLop.Trim().ToUpper() == "TRUE")
                                                                    lopamnt = AllDedLopAmnt + lopamnt;
                                                            }
                                                            if (semicol1[12] == "1")
                                                            {
                                                                if (semicol1[13].Trim() != "")
                                                                {
                                                                    Double newval = 0;
                                                                    Double.TryParse(Convert.ToString(semicol1[13]), out newval);
                                                                    if (frmtot < newval)
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, frmtot, slabvalue);
                                                                        mydic = getdatas(dedtion, Convert.ToString(deduct1), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        deduct1 = lopamntcalc(amntformat, deduct1, slabvalue);
                                                                    }
                                                                    else
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(newval), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        frmtot = lopamntcalc(amntformat, newval, slabvalue);
                                                                        mydic = getdatas(dedtion, Convert.ToString(deduct1), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        deduct1 = lopamntcalc(amntformat, deduct1, slabvalue);
                                                                    }
                                                                    if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                    {
                                                                        mydic = getdatas(dedtion, Convert.ToString(OrgPFAmnt), catcode);
                                                                        amntformat = getamntformat(mydic, dedtion);
                                                                        Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                        OrgPFAmnt = lopamntcalc(amntformat, OrgPFAmnt, slabvalue);
                                                                    }
                                                                }
                                                            }
                                                            if (semicol1.Length > 20 && isptgrosslop == "1") //poomalar 25.10.17
                                                            {
                                                                string query = "select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + ddlcollege.SelectedItem.Value + "' and user_code ='" + usercode + "'";
                                                                string sql = d2.GetFunction(query).Trim();
                                                                string ptstmonth = "";
                                                                string ptendmonth = "";
                                                                string ptstyear = "";
                                                                string ptendyear = "";
                                                                string pt = "";
                                                                string slab = "";
                                                                if (sql != " ")
                                                                {
                                                                    string[] det = sql.Split(';');
                                                                    string monthyear = det[0];
                                                                    string endmonthyear = det[1];
                                                                    ptstmonth = monthyear.Split('-')[0];
                                                                    ptendmonth = endmonthyear.Split('-')[0];
                                                                    ptstyear = monthyear.Split('-')[1];
                                                                    ptendyear = endmonthyear.Split('-')[1];
                                                                }
                                                                if (month1 == ptstmonth && year1 == ptstyear || month1 == ptendmonth && year1 == ptendyear)
                                                                {
                                                                    if (semicol1.Length > 20)
                                                                    {
                                                                        if (semicol1[20].Trim() == "1" && isptgrosslop == "1" && (dedtion.Trim().ToUpper() == "P.TAX" || dedtion.Trim().ToUpper() == "P TAX" || dedtion.Trim().ToUpper() == "PROFESSIONAL TAX" || dedtion.Trim().ToUpper() == "PROFTAX"))
                                                                        {
                                                                            DateTime FCalYearDT = new DateTime(Convert.ToInt32(ptstyear), Convert.ToInt32(ptstmonth), 28);
                                                                            DateTime TCalYearDT = new DateTime(Convert.ToInt32(ptendyear), Convert.ToInt32(ptendmonth), 28);
                                                                            //Diffmonth = (TCalYearDT.Month - FCalYearDT.Month) + 12 * (TCalYearDT.Year - FCalYearDT.Year);
                                                                            DateTime diffmonth = new DateTime();
                                                                            string payyearpt = "";
                                                                            string paymonthpt = "";
                                                                            if (month1 == ptstmonth)
                                                                            {
                                                                                diffmonth = FCalYearDT.AddMonths(-6);
                                                                                payyearpt = ptstyear;
                                                                                paymonthpt = ptstmonth;
                                                                            }
                                                                            if (month1 == ptendmonth)
                                                                            {
                                                                                diffmonth = TCalYearDT.AddMonths(-6);
                                                                                payyearpt = ptendyear;
                                                                                paymonthpt = ptendmonth;
                                                                            }
                                                                            //barath 11.11.17
                                                                            string querypt = "select SUM(netadd)-SUM(lop) pt,SUM(netadd) salary,SUM(lop) lop from monthlypay m where CAST(CONVERT(varchar(20),m.PayMonth)+'/01/'+CONVERT(varchar(20),m.PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + diffmonth.Month + "')+'/01/'+CONVERT(varchar(20),'" + diffmonth.Year + "') as Datetime) and CAST(CONVERT(varchar(20),'" + paymonthpt + "')+'/01/'+CONVERT(varchar(20),'" + payyearpt + "') as Datetime) and m.staff_code='" + staf_cd + "' ";
                                                                            //((PayMonth >= '" + diffmonth.Month + "' and PayYear = '" + diffmonth.Year + "') or (PayMonth <='" + paymonthpt + "' and PayYear = '" + payyearpt + "' )) and staff_code='" + staf_cd + "' ";
                                                                            DataSet dspt = new DataSet();
                                                                            dspt = d2.select_method_wo_parameter(querypt, "Text");
                                                                            pt = dspt.Tables[0].Rows[0]["pt"].ToString();
                                                                            string qryslab = "select  slabvalue from pfslabs where SlabFor='" + dedtion + "' and category_code='" + catcode + "' and college_code='" + collegecode1 + "' and salfrom<='" + pt + "' and salto>='" + pt + "'";
                                                                            DataSet dsslab = new DataSet();
                                                                            dsslab = d2.select_method_wo_parameter(qryslab, "Text");
                                                                            slab = dsslab.Tables[0].Rows[0]["slabvalue"].ToString();
                                                                            deduct1 = Convert.ToDouble(slab);
                                                                            frmtot = Convert.ToDouble(slab);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (netval.Trim() == "1")
                                                            {
                                                                mydic = getdatas(dedtion, Convert.ToString(frmtot), catcode);
                                                                amntformat = getamntformat(mydic, dedtion);
                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                frmtot = lopamntcalc(amntformat, frmtot, slabvalue);
                                                                mydic = getdatas(dedtion, Convert.ToString(deduct1), catcode);
                                                                amntformat = getamntformat(mydic, dedtion);
                                                                Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                deduct1 = lopamntcalc(amntformat, deduct1, slabvalue);
                                                                if (dedtion.Trim() == "PF" || dedtion.Trim() == "PROVIDENT FUND")
                                                                {
                                                                    mydic = getdatas(dedtion, Convert.ToString(OrgPFAmnt), catcode);
                                                                    amntformat = getamntformat(mydic, dedtion);
                                                                    Double.TryParse(getamnt(mydic, dedtion), out slabvalue);
                                                                    OrgPFAmnt = lopamntcalc(amntformat, OrgPFAmnt, slabvalue);
                                                                }
                                                            }
                                                            if (semicol1[11] == ">=1")//barath
                                                            {
                                                                deduct1 = Math.Ceiling(deduct1);
                                                                frmtot = Math.Ceiling(frmtot);
                                                                pers_ded1 = Math.Ceiling(pers_ded1);
                                                            }
                                                            else
                                                            {
                                                                deduct1 = Math.Round(deduct1, 0, MidpointRounding.AwayFromZero);
                                                                frmtot = Math.Round(frmtot, 0, MidpointRounding.AwayFromZero);
                                                                pers_ded1 = Math.Round(pers_ded1, 0, MidpointRounding.AwayFromZero);
                                                            }
                                                            if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                            {
                                                                pfamnt = OrgPFAmnt;
                                                                pfsal = frmtot;
                                                            }
                                                            if (ded_format.Trim() == "")
                                                                ded_format = Convert.ToString(dedtion) + ";" + "Slab" + ";" + Convert.ToString(frmtot) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                            else
                                                                ded_format = ded_format + Convert.ToString(dedtion) + ";" + "Slab" + ";" + Convert.ToString(frmtot) + "-" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
                                                        }
                                                    }
                                                    #endregion
                                                    #region PF Calculation
                                                    if (dedtion.Trim().ToUpper() == "PF" || dedtion.Trim().ToUpper() == "PROVIDENT FUND")
                                                    {
                                                        DataSet dspf = new DataSet();
                                                        if (isfpfcheck == "1" && maxamnt != 0) //Cal Basic Salary
                                                        {
                                                            if (maxamnt <= pfSetAmnt) // Pf grade pay setting total amount---- PF contripution
                                                            {
                                                                if (age != 0 && ageval <= Convert.ToInt32(age))
                                                                {
                                                                    fpfamnt = maxamnt * (fpfper / 100);
                                                                    fpfamnt = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (age != 0 && ageval <= Convert.ToInt32(age))
                                                                {
                                                                    fpfamnt = pfSetAmnt * (fpfper / 100);
                                                                    fpfamnt = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                        }
                                                        string get_slab = "select slabtype,slabvalue from pfslabs where SlabFor = 'PF' and '" + bas_salary + "' between salfrom and salto and college_code='" + collegecode1 + "'";
                                                        dspf.Clear();
                                                        dspf = d2.select_method_wo_parameter(get_slab, "Text");
                                                        if (dspf.Tables.Count > 0 && dspf.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (Convert.ToString(dspf.Tables[0].Rows[0]["slabvalue"]).Trim() != "")
                                                            {
                                                                if (Convert.ToString(dspf.Tables[0].Rows[0]["slabtype"]) == "Percent")
                                                                    pfsal = bas_salary * (Convert.ToDouble(dspf.Tables[0].Rows[0]["slabvalue"]) / 100);
                                                                if (Convert.ToString(dspf.Tables[0].Rows[0]["slabtype"]) == "Amount")
                                                                    pfsal = Convert.ToDouble(dspf.Tables[0].Rows[0]["slabvalue"]);
                                                            }
                                                        }
                                                        int uppf = d2.update_method_wo_parameter("update staffmaster set Is_PF='1' where staff_code='" + staf_cd + "' and college_code='" + collegecode1 + "'", "Text");
                                                    }
                                                    #endregion
                                                    #region ESI Calculation
                                                    if (dedtion.Trim().ToUpper() == "ESI" || dedtion.Trim().ToUpper() == "EMPLOYEE STATE INSURANCE")
                                                    {
                                                        if (semicol1[1] == "Percent")
                                                        {
                                                            if (Convert.ToString(semicol1[2]).Trim() != "")
                                                            {
                                                                pers_ded1 = Convert.ToDouble(semicol1[2]);
                                                                esiamnt = (pers_ded1 / 100) * bas_salary;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToString(semicol1[2]).Trim() != "")
                                                                esiamnt = Convert.ToDouble(semicol1[2]);
                                                        }
                                                        DataSet dsesi = new DataSet();
                                                        string get_slab = "select slabtype,slabvalue from pfslabs where SlabFor = 'ESI' and '" + bas_salary + "' between salfrom and salto and college_code='" + collegecode1 + "'";
                                                        dsesi.Clear();
                                                        dsesi = d2.select_method_wo_parameter(get_slab, "Text");
                                                        if (dsesi.Tables.Count > 0 && dsesi.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (Convert.ToString(dsesi.Tables[0].Rows[0]["slabvalue"]).Trim() != "")
                                                            {
                                                                if (Convert.ToString(dsesi.Tables[0].Rows[0]["slabtype"]) == "Percent")
                                                                    esisal = bas_salary * (Convert.ToDouble(dsesi.Tables[0].Rows[0]["slabvalue"]) / 100);
                                                                if (Convert.ToString(dsesi.Tables[0].Rows[0]["slabtype"]) == "Amount")
                                                                    esisal = Convert.ToDouble(dsesi.Tables[0].Rows[0]["slabvalue"]);
                                                            }
                                                        }
                                                        int upesi = d2.update_method_wo_parameter("update staffmaster set IsESIStaff='1' where staff_code='" + staf_cd + "' and college_code='" + collegecode1 + "'", "Text");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            totded = totded + frmtot;
                                            deductionval = deductionval + Convert.ToInt32(frmtot);
                                        }
                                    }
                                    totded = totded + lopamnt;   //Added by Jeyaprakash on Apr 25th,2017
                                    if (ismpf.Trim().ToUpper() == "TRUE" && mpfper.Trim() != "" && mpfamnt != 0)
                                    {
                                        Double myMpf = 0;
                                        Double.TryParse(mpfper, out myMpf);
                                        pfsal = ((mpfamnt * myMpf) / 100) - fpfamnt;   //Mpf Cal = PF_Salary
                                    }
                                    else
                                    {
                                        if (pfsal > fpfamnt)
                                            pfsal = pfsal - fpfamnt;
                                        else
                                            pfsal = 0;
                                    }
                                    //if (totded > lopamnt)
                                    //    deductwlop = totded - lopamnt;
                                    //else
                                    //    deductwlop = totded;

                                    //if (totded > lopamnt)//delsi0421
                                    //    deductwlop = totded - lopamnt;
                                    //else
                                    //    deductwlop = totded;

                                    if (lopamnt != 0)//delsi0421
                                    {
                                        deductwlop = totded - lopamnt;

                                    }
                                    else
                                    {
                                        deductwlop = totded;

                                    }

                                    // netsalary = Gross_salary - totded; //totallowwolop+totallow  //Changed by JP on Apr 25th,2017 (grosswlop)
                                    //netsalary = grosswlop - deductwlop;//barath 27.12.17 grosswlop//barath 22.01.18 commented nec
                                    //actnetsal = Gross_salary - totded;//barath 22.01.18 commented nec

                                    actnetsal = Gross_salary - deductwlop;
                                    if (grosswlop == 0)//22.01.18
                                    {
                                        netsalary = Gross_salary - deductwlop;
                                    }
                                    else
                                    {
                                        if (lopfrombasic == true)// ;delsi2702
                                        {
                                            netsalary = grosswlop;

                                            if (cb_lop_fromgross.Checked == true)//delsi2303
                                            {
                                                netsalary = grosswlop - deductwlop;

                                            }
                                        }
                                        else
                                        {
                                            if (deductionval == 0)//delsi2703
                                            {
                                                netsalary = grosswlop;

                                            }
                                            else
                                            {
                                                if (check_join == false)
                                                {
                                                    netsalary = grosswlop - deductwlop;//delsi2703
                                                }
                                                else if (check_join == true && ismanuallop.ToUpper() != "TRUE")//delsi 0908
                                                {
                                                    if (Gross_salary > totded)
                                                    {
                                                        netsalary = Gross_salary - totded;
                                                    }
                                                    else
                                                    {
                                                        netsalary = 0;
                                                    }
                                                }
                                                else if (check_join == true && ismanuallop.ToUpper() == "TRUE")
                                                {
                                                    if (grosswlop > deductwlop)
                                                    {
                                                        netsalary = grosswlop - deductwlop;
                                                    }
                                                    else
                                                    { 
                                                        netsalary = 0;
                                                    
                                                    }
                                                
                                                }
                                            }

                                        }
                                    }
                                    if (isautoded == "1")
                                    {
                                        string get_slab = "";
                                        string slab = "";
                                        Double slabsal = 0;
                                        if (strautoded.Trim() != "")
                                        {
                                            string[] splauto = strautoded.Split(',');
                                            if (splauto.Length > 0)
                                            {
                                                for (int ik = 0; ik < splauto.Length; ik++)
                                                {
                                                    get_slab = "select ESI_EmpSlabType,ESI_EmpSlabValue from pfslabs where SlabFor = '" + Convert.ToString(splauto[ik]) + "' and '" + netsalary + "' between salfrom and salto and college_code='" + collegecode1 + "'";
                                                    ds1 = d2.select_method_wo_parameter(get_slab, "Text");
                                                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) != "")
                                                        {
                                                            if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabType"]) == "Percent")
                                                            {
                                                                slabsal = netsalary * (Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) / 100);
                                                                netsalary = netsalary - slabsal;
                                                                actnetsal = actnetsal - slabsal;
                                                            }
                                                            if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabType"]) == "Amount")
                                                            {
                                                                slabsal = Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]);
                                                                netsalary = netsalary - slabsal;
                                                                actnetsal = actnetsal - slabsal;
                                                            }
                                                        }
                                                        totded = totded + slabsal;
                                                        slab = Convert.ToString(splauto[ik]) + ";" + "Amount" + ";" + Convert.ToString(slabsal) + "-" + Convert.ToString(slabsal) + ";" + Convert.ToString(slabsal) + "\\";
                                                        if (ded_format == "")
                                                            ded_format = slab;
                                                        else
                                                            ded_format = ded_format + slab;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #region loan deduction
                                    string getsumemi = "";
                                    Double loanamt = 0;
                                    Double totemi = 0;
                                   // Double currbal = 0;
                                    int currbal = 0;
                                    Double dedamt = 0;
                                    Double totdedamt = 0;
                                    Double emiamt = 0;
                                    Double intamnt = 0;
                                    Double intper = 0;
                                    string lon_cod = "";
                                    ds2.Clear();
                                    string loan = "select IsDed,DedName,DedAmt,EMIAmt,LoanCode,LoanAmount,LoanType,DedFromMonth,DedFromYear,IntAmt,InterestPer,IsInterest,PolicyAmt from StaffLoanDet where Staff_Code = '" + staf_cd + "' and IsActive = 1 and IsClose = 0 and IsDed = 1 and cast(convert(varchar, dedfrommonth)+'/'+'1/'+CONVERT(varchar,dedfromyear) as DATE) <='" + month1 + "/1/" + year1 + "'";
                                    //DedFromYear >='" + year1 + "' and DedFromMonth >='" + month1 + "'";   // 
                                    ds2 = d2.select_method_wo_parameter(loan, "Text");
                                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                    {
                                        string updq = "UPDATE StaffLoanDet SET IsClose = '0' WHERE Staff_Code ='" + staf_cd + "' AND LoanCode IN (SELECT LoanCode FROM StaffLoanPayDet WHERE STaff_Code ='" + staf_cd + "' AND PayMonth ='" + month1 + "' AND PayYear ='" + year1 + "')";
                                        updq = updq + " DELETE FROM StaffLoanPayDet WHERE Staff_Code ='" + staf_cd + "' AND PayMonth ='" + month1 + "' AND PayYear ='" + year1 + "'";
                                        int newupcount = d2.update_method_wo_parameter(updq, "Text");
                                        for (int ln = 0; ln < ds2.Tables[0].Rows.Count; ln++)
                                        {
                                            string loantype = Convert.ToString(ds2.Tables[0].Rows[ln]["LoanType"]);
                                            lon_cod = Convert.ToString(ds2.Tables[0].Rows[ln]["LoanCode"]);
                                            string ded_emi = Convert.ToString(ds2.Tables[0].Rows[ln]["DedAmt"]);
                                            string dedname = Convert.ToString(ds2.Tables[0].Rows[ln]["DedName"]);
                                            Double.TryParse(Convert.ToString(ds2.Tables[0].Rows[ln]["InterestPer"]), out intper);
                                            getsumemi = d2.GetFunction("SELECT ISNULL(SUM(EMIAmt),0) From StaffLoanPayDet WHERE Staff_Code='" + staf_cd + "' and LoanCode='" + lon_cod + "'");
                                            Double.TryParse(getsumemi, out totemi);
                                            if (loantype.Trim() == "0")
                                            {
                                                Double.TryParse(Convert.ToString(ds2.Tables[0].Rows[ln]["LoanAmount"]), out loanamt);
                                                Double.TryParse(Convert.ToString(ds2.Tables[0].Rows[ln]["EMIAmt"]), out dedamt);
                                                //totemi = (totemi == 0) ? dedamt : totemi;//bb 23.09.17
                                                currbal = Convert.ToInt32(loanamt) - Convert.ToInt32(totemi);//delsi 0708
                                                if (currbal >= 0)//delsi 0708
                                                {
                                                    if (dedname.Trim() != "")
                                                    {
                                                        if (currbal >= dedamt)
                                                        {
                                                            emiamt = dedamt;
                                                            totdedamt = totdedamt + dedamt;
                                                        }
                                                        else
                                                        {
                                                            emiamt = currbal;
                                                            totdedamt = totdedamt + currbal;
                                                        }
                                                        intamnt = currbal * (intper / 100);
                                                        if (Convert.ToString(ds2.Tables[0].Rows[ln]["IsInterest"]).Trim().ToUpper() == "TRUE")
                                                            totdedamt = totdedamt + (currbal * (intper / 100));
                                                        if (currbal <= 0 || loanamt == dedamt) //bb 23.09.17
                                                        {
                                                            if (lon_cod.Trim() != "")
                                                            {
                                                                string updsql = "UPDATE StaffLoanDet SET IsClose = '1',CloseDate='" + todt.ToString("MM/dd/yyyy") + "' WHERE Staff_Code ='" + staf_cd + "' AND LoanCode ='" + lon_cod + "'";
                                                                int upcount = d2.update_method_wo_parameter(updsql, "Text");
                                                            }
                                                        }
                                                        if (emiamt > 0)
                                                        {
                                                            if (ded_format.Contains(dedname))
                                                            {
                                                                string[] splded = ded_format.Split('\\');
                                                                if (splded.Length > 0)
                                                                {
                                                                    ded_format = "";
                                                                    for (int ik = 0; ik < splded.Length; ik++)
                                                                    {
                                                                        string[] spldd = splded[ik].Split(';');
                                                                        if (spldd.Length >= 4)
                                                                        {
                                                                            Double newded = 0;
                                                                            if (spldd[2].Trim() != "")
                                                                            {
                                                                                string[] splnew = spldd[2].Split('-');
                                                                                if (splnew.Length == 2)
                                                                                {
                                                                                    if (dedname == spldd[0])
                                                                                    {
                                                                                        if (spldd[1].Trim().ToUpper() == "PERCENT")
                                                                                        {
                                                                                            Double.TryParse(Convert.ToString(splnew[1]), out newded);
                                                                                            newded = newded + emiamt;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Double.TryParse(Convert.ToString(splnew[0]), out newded);
                                                                                            newded = newded + emiamt;
                                                                                            if (splnew[1] == "0")//barath 28.11.17 mcc windows
                                                                                            {
                                                                                                splnew[1] = Convert.ToString(newded);
                                                                                                spldd[3] = Convert.ToString(newded);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (spldd[1].Trim().ToUpper() == "PERCENT")
                                                                                            newded = Convert.ToDouble(splnew[0]);
                                                                                        else
                                                                                            newded = Convert.ToDouble(splnew[1]);//barath 28.08.17 change splnew[0] to splnew[1] and 
                                                                                    }
                                                                                    if (ded_format.Trim() == "")
                                                                                        ded_format = Convert.ToString(spldd[0]) + ";" + Convert.ToString(spldd[1]) + ";" + Convert.ToString(newded) + "-" + splnew[1] + ";" + Convert.ToString(spldd[3]) + ";" + "\\";
                                                                                    else
                                                                                        ded_format = ded_format + Convert.ToString(spldd[0]) + ";" + Convert.ToString(spldd[1]) + ";" + Convert.ToString(newded) + "-" + splnew[1] + ";" + Convert.ToString(spldd[3]) + ";" + "\\";
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ded_format.Trim() == "")
                                                                    ded_format = dedname + ";" + "Amount" + ";" + Convert.ToString(emiamt) + "-" + Convert.ToString(emiamt) + ";" + Convert.ToString(emiamt) + ";" + "\\";
                                                                else
                                                                    ded_format = ded_format + dedname + ";" + "Amount" + ";" + Convert.ToString(emiamt) + "-" + Convert.ToString(emiamt) + ";" + Convert.ToString(emiamt) + ";" + "\\";
                                                            }
                                                            netsalary = netsalary - emiamt;
                                                            actnetsal = actnetsal - emiamt;
                                                            totded = totded + emiamt;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Double.TryParse(Convert.ToString(ds2.Tables[0].Rows[ln]["PolicyAmt"]), out loanamt);
                                                Double.TryParse(Convert.ToString(ds2.Tables[0].Rows[ln]["DedAmt"]), out dedamt);
                                                currbal =Convert.ToInt32(loanamt) - Convert.ToInt32(totemi);//delsi 0708
                                                if (currbal >= 0)//delsi 0708
                                                {
                                                    if (dedname.Trim() != "")
                                                    {
                                                        if (currbal >= dedamt)
                                                        {
                                                            emiamt = dedamt;
                                                            totdedamt = totdedamt + dedamt;
                                                        }
                                                        else
                                                        {
                                                            emiamt = currbal;
                                                            totdedamt = totdedamt + currbal;
                                                        }
                                                        intamnt = currbal * (intper / 100);
                                                        if (Convert.ToString(ds2.Tables[0].Rows[ln]["IsInterest"]).Trim().ToUpper() == "TRUE")
                                                            totdedamt = totdedamt + (currbal * (intper / 100));
                                                        if ((currbal - totdedamt) <= 0)
                                                        {
                                                            if (lon_cod.Trim() != "")
                                                            {
                                                                string updsql = "UPDATE StaffLoanDet SET IsClose = '1',CloseDate='" + todt.ToString("MM/dd/yyyy") + "' WHERE Staff_Code ='" + staf_cd + "' AND LoanCode ='" + lon_cod + "'";
                                                                int upcount = d2.update_method_wo_parameter(updsql, "Text");
                                                            }
                                                        }
                                                        if (dedamt > 0)
                                                        {
                                                            if (ded_format.Contains(dedname))
                                                            {
                                                                string[] splded = ded_format.Split('\\');
                                                                if (splded.Length > 0)
                                                                {
                                                                    ded_format = "";
                                                                    for (int ik = 0; ik < splded.Length; ik++)
                                                                    {
                                                                        string[] spldd = splded[ik].Split(';');
                                                                        if (spldd.Length >= 4)
                                                                        {
                                                                            Double newded = 0;
                                                                            if (spldd[2].Trim() != "")
                                                                            {
                                                                                string[] splnew = spldd[2].Split('-');
                                                                                if (splnew.Length == 2)
                                                                                {
                                                                                    if (dedname == spldd[0])
                                                                                    {
                                                                                        if (spldd[1].Trim().ToUpper() == "PERCENT")
                                                                                        {
                                                                                            Double.TryParse(Convert.ToString(splnew[1]), out newded);
                                                                                            newded = newded + emiamt;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Double.TryParse(Convert.ToString(splnew[0]), out newded);
                                                                                            newded = newded + emiamt;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (spldd[1].Trim().ToUpper() == "PERCENT")
                                                                                            newded = Convert.ToDouble(splnew[1]);
                                                                                        else
                                                                                            newded = Convert.ToDouble(splnew[0]);
                                                                                    }
                                                                                    if (ded_format.Trim() == "")
                                                                                        ded_format = Convert.ToString(spldd[0]) + ";" + Convert.ToString(spldd[1]) + ";" + Convert.ToString(newded) + "-" + splnew[0] + ";" + Convert.ToString(spldd[3]) + ";" + "\\";
                                                                                    else
                                                                                        ded_format = ded_format + Convert.ToString(spldd[0]) + ";" + Convert.ToString(spldd[1]) + ";" + Convert.ToString(newded) + "-" + splnew[0] + ";" + Convert.ToString(spldd[3]) + ";" + "\\";
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ded_format.Trim() == "")
                                                                    ded_format = dedname + ";" + "Amount" + ";" + Convert.ToString(dedamt) + "-" + Convert.ToString(dedamt) + ";" + dedamt + ";" + "\\";
                                                                else
                                                                    ded_format = ded_format + dedname + ";" + "Amount" + ";" + Convert.ToString(dedamt) + "-" + Convert.ToString(dedamt) + ";" + dedamt + ";" + "\\";
                                                            }
                                                            netsalary = netsalary - dedamt;
                                                            actnetsal = actnetsal - dedamt;
                                                            totded = totded + dedamt;
                                                        }
                                                    }
                                                }
                                            }
                                            if (lon_cod.Trim() != "")
                                            {
                                                string chkisclose = d2.GetFunction("select IsDed,DedName,DedAmt,EMIAmt,LoanCode,LoanAmount,LoanType,DedFromMonth,DedFromYear,IntAmt,InterestPer,IsInterest,PolicyAmt from StaffLoanDet where Staff_Code = '" + staf_cd + "' and IsActive = 1 and IsClose = 0 and IsDed = 1 and cast(convert(varchar, dedfrommonth)+'/'+'1/'+CONVERT(varchar,dedfromyear) as DATE) <='" + month1 + "/1/" + year1 + "' and LoanCode='" + lon_cod + "'");

                                                if (chkisclose=="True")//delsi 0708
                                                {

                                                    string insq = "Insert into StaffLoanPayDet (Staff_Code,EMIAmt,IntAmt,PayMonth, PayYear,LoanCode) values ('" + staf_cd + "','" + emiamt + "','" + intamnt + "','" + month1 + "','" + year1 + "','" + lon_cod + "')";
                                                    int newcount = d2.update_method_wo_parameter(insq, "Text");
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #endregion
                                }
                                else /* poomalar 12.10.17*/
                                {
                                    totded = 0;
                                    pfsal = 0;
                                    esisal = 0;
                                    pfSetAmnt = 0;
                                    if (lopamnt != 0)
                                    {
                                        totded = lopamnt;
                                    }
                                    //  lopamnt = 0; delsi 30/04/2018
                                }
                                if (cbincitcalc.Checked == true)
                                {
                                    string selitset = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IT Calculation Settings' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
                                    Double taxamnt = 0;
                                    if (selitset.Trim() != "0")
                                    {
                                        string gettaxamnt = d2.GetFunction("select TaxAmount from StaffTaxDetails where Staff_Code='" + staf_cd + "' and Asst_Year='" + selitset + "'");
                                        Double.TryParse(gettaxamnt, out taxamnt);
                                        netsalary = netsalary - taxamnt;
                                        actnetsal = actnetsal - taxamnt;
                                    }
                                }
                                if (lopperday.ToString().ToUpper() == "NAN")
                                    lopperday = 0;
                                string insertquery = "";
                                if (bas_salary > 0 && netsalary >= 0 || (netsalary >= 0 && ParttimeStaff == "1"))
                                {
                                    if (checkedok() && activecol.Trim() == "4")
                                    {
                                        int delcount = d2.update_method_wo_parameter("delete from monthlypay where PayMonth='" + month1 + "' and PayYear='" + year1 + "' and staff_code ='" + staf_cd + "' and college_Code='" + collegecode1 + "'", "Text");
                                    }
                                    else if (activecol.Trim() == "3")
                                    {
                                        if (delflag == false)
                                        {
                                            if (ddlsearchappstf.SelectedIndex == 0)// 27.12.17 barath
                                            {
                                                string delq = "  delete monthlypay from staffmaster s,stafftrans t where s.staff_code=t.staff_code and monthlypay.staff_code=s.staff_code and t.staff_code =monthlypay.staff_code and s.college_code=monthlypay.college_code and monthlypay.college_code ='" + collegecode1 + "' and t.latestrec=1  and monthlypay.PayMonth='" + month1 + "' and monthlypay.PayYear='" + year1 + "'";
                                                if (dept.Trim() != "")
                                                    delq = delq + " and t.dept_code in('" + dept + "')";
                                                if (desig.Trim() != "")
                                                    delq = delq + " and desig_Code in('" + desig + "')";
                                                if (staftype.Trim() != "")
                                                    delq = delq + " and t.stftype in('" + staftype + "')";
                                                if (staffcategory.Trim() != "")
                                                    delq = delq + " and t.category_code in('" + staffcategory + "')";
                                                int newdelcount = d2.update_method_wo_parameter(delq, "Text");
                                                delflag = true;
                                            }
                                            else
                                            {
                                                string delq = "  delete monthlypay from staffmaster s,stafftrans t where s.staff_code=t.staff_code and monthlypay.staff_code=s.staff_code and t.staff_code =monthlypay.staff_code and s.college_code=monthlypay.college_code and monthlypay.college_code ='" + collegecode1 + "' and t.latestrec=1  and monthlypay.PayMonth='" + month1 + "' and monthlypay.PayYear='" + year1 + "'";
                                                if (dept.Trim() != "")
                                                    delq = delq + " and t.dept_code in('" + dept + "')";
                                                if (desig.Trim() != "")
                                                    delq = delq + " and desig_Code in('" + desig + "')";
                                                if (staftype.Trim() != "")
                                                    delq = delq + " and t.stftype in('" + staftype + "')";
                                                if (staffcategory.Trim() != "")
                                                    delq = delq + " and t.category_code in('" + staffcategory + "')";
                                                if (!string.IsNullOrEmpty(Staffcode))
                                                    delq += " and t.staff_code in('" + Staffcode + "')";
                                                if (!string.IsNullOrEmpty(StfName))
                                                    delq += " and s.staff_name in('" + StfName + "')";
                                                int newdelcount = d2.update_method_wo_parameter(delq, "Text");
                                                delflag = true;
                                            }
                                        }
                                    }
                                    // ,Actual_Basic='" + Convert.ToDouble(bas_salary) + "' change 23.01.18 barath nec actbasic

                                    insertquery = " if exists(select * from monthlypay where staff_code='" + staf_cd + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and latestrec='1' and college_code='" + collegecode1 + "') update monthlypay set dept_name='" + deptname + "',desig_name='" + designame + "',fdate='" + newdt.ToString("MM/dd/yyyy") + "',stftype='" + stftype + "',tdate='" + newdt1.ToString("MM/dd/yyyy") + "',adate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',bsalary='" + basicwlop + "',pf='" + pfamnt + "',lop='" + lopperday + "',leavedetail='" + leavedet + "',allowances='" + format + "',deductions='" + ded_format + "',addd='" + totallow + "',netadd='" + grosswlop + "',deddd='" + totded + "',netded='" + totded + "',netsal='" + netsalary + "',paybybank='1',category_code='" + catcode + "',basic_alone='" + actbasic + "',pay_band='" + Convert.ToDouble(cb_payband) + "',grade_pay='" + Convert.ToDouble(grade_pay) + "',Cur_Lop='" + curlop + "',Pre_Lop='" + prelop + "',Actual_Basic='" + Convert.ToDouble(actbasic) + "',DAAmt='" + DAAmt + "',Tot_LOP='" + lopamnt + "',ESI_Salary='" + esisal + "',PF_Salary='" + pfsal + "',G_Pay='" + gradepaywlop + "',NetAddAct='" + Gross_salary + "',ESI='" + esiamnt + "',DAWithLOP='" + dawithlop + "',FPF='" + fpfamnt + "',PFContribution='" + pfSetAmnt + "',CL_Calc_Amnt='" + CLCalcAmnt + "',CL_RemainLeave='" + CasualRemainLeave + "',PerDay_Salary='" + oned_bassal + "',TransferBankFK='" + TransferBankFK + "' where staff_code='" + staf_cd + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and latestrec='1' and college_code='" + collegecode1 + "' else Insert into monthlypay (staff_code,dept_name,desig_name,fdate,tdate,adate,bsalary,pf,lop,leavedetail,allowances,deductions,addd,netadd,deddd,netded,netsal,paybybank,college_code,category_code,basic_alone,pay_band,grade_pay,Cur_Lop,Pre_Lop,Actual_Basic,DAAmt,Tot_LOP,ESI_Salary,PF_Salary,G_Pay,NetAddAct,ESI,DAWithLOP,FPF,PayMonth,PayYear,latestrec,stftype,BPArr,BPArrWithLOP,GPArr,GPArrWithLOP,DAArr,DAArrWithLOP,SAAmt,SAAmtWithLOP,SAArr,SAArrWithLOP,HBALoanIntAmt,MediClaimIns,EduLoanIntAmt,LICAmt,HBALoanAmt,PPFAmt,TutionFees,PFContribution,CL_Calc_Amnt,CL_RemainLeave,PerDay_Salary,TransferBankFK) Values ('" + staf_cd + "','" + deptname + "','" + designame + "','" + newdt.ToString("MM/dd/yyyy") + "','" + newdt1.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + basicwlop + "','" + pfamnt + "','" + lopperday + "','" + leavedet + "','" + format + "','" + ded_format + "','" + totallow + "','" + grosswlop + "','" + totded + "','" + totded + "','" + netsalary + "','1','" + collegecode1 + "','" + catcode + "','" + actbasic + "','" + Convert.ToDouble(cb_payband) + "','" + Convert.ToDouble(grade_pay) + "','" + curlop + "','" + prelop + "','" + Convert.ToDouble(actbasic) + "','" + DAAmt + "','" + lopamnt
 + "','" + esisal + "','" + pfsal + "','" + gradepaywlop + "','" + Gross_salary + "','" + esiamnt + "','" + dawithlop + "','" + fpfamnt + "','" + month1 + "','" + year1 + "','1','" + stftype + "','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','" + pfSetAmnt + "','" + CLCalcAmnt + "','" + CasualRemainLeave + "','" + oned_bassal + "','" + TransferBankFK + "')";
                                    int inscount = d2.update_method_wo_parameter(insertquery, "Text");
                                    if (inscount > 0)
                                    {
                                        savecount++;
                                        gencount++;
                                    }
                                }
                            }
                        }
                    }
                    if (savecount > 0)
                        genchk = true;
                    if (savecount == 0)
                    {
                        genchk = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Update the Attendance Details for Staff!";
                        return;
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Staff Found!";
                }
            }
        }
        catch (Exception ex)
        {

            //d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int LOP_From_Attendance = 0;
        int LOP_From_Basic = 0;
        int OP_from_Payband = 0;
        int LOP_from_Gradepay = 0;
        int tot_month_wisedays = 0;
        string maxdays = txt_totaldays.Text.ToString();
        if (maxdays == "")
            maxdays = "0";
        int Absent_Calculation = 0;
        string absentcal = txt_absent.Text.ToString();
        if (absentcal == "")
            absentcal = "0";
        int NA_RL_take_attendance_from_month = 0;
        int LOP_PF = 0;
        string LOP_PF_days = txt_days.Text.ToString();
        if (LOP_PF_days == "")
            LOP_PF_days = "0";
        string FPF_Calculation = txt_fpf.Text.ToString();
        if (FPF_Calculation == "")
            FPF_Calculation = "0";
        string Age_Less_than = txt_age_val.Text.ToString();
        if (Age_Less_than == "")
            Age_Less_than = "0";
        string Maximum_Amount = txt_max_amount.Text.ToString();
        if (Maximum_Amount == "")
            Maximum_Amount = "0";
        int Auto_Deduct = 0;
        int Unpaid_Leave_attendence = 0;
        int Maximum_PER_LA = 0;
        string MaxPerVal_after = txt_after.Text.ToString();
        if (MaxPerVal_after == "")
            MaxPerVal_after = "0";
        int multiple_days = 0;
        int LOP_From_Gross = 0;
        if (LOP_From_Attendance == 1)
            cb_lopfrom_atn.Checked = true;
        else
            cb_lopfrom_atn.Checked = false;
        if (LOP_From_Gross == 1)
            cb_lop_fromgross.Checked = true;
        else
            cb_lop_fromgross.Checked = false;
        if (LOP_From_Basic == 1)
            cb_Lopfrom_basic.Checked = true;
        else
            cb_Lopfrom_basic.Checked = false;
        if (OP_from_Payband == 1)
            cb_Lopfrom_payband.Checked = true;
        else
            cb_Lopfrom_payband.Checked = false;
        if (LOP_from_Gradepay == 1)
            cb_lopgradpay.Checked = true;
        else
            cb_lopgradpay.Checked = false;
        if (tot_month_wisedays == 1)
            rdb_month.Checked = true;
        else
            rdb_month.Checked = false;
        if (tot_month_wisedays == 2)
            rdb_Days.Checked = true;
        else
            rdb_Days.Checked = false;
        if (Absent_Calculation == 1)
            cb_absentcalculation.Checked = true;
        else
            cb_absentcalculation.Checked = false;
        if (NA_RL_take_attendance_from_month == 1)
            cb_NA_RL.Checked = true;
        else
            cb_NA_RL.Checked = false;
        if (LOP_PF == 1)
            cb_LOP_to_PF.Checked = true;
        else
            cb_LOP_to_PF.Checked = false;
        if (Auto_Deduct == 1)
            cb_auto_deduct.Checked = true;
        else
            cb_auto_deduct.Checked = false;
        if (Unpaid_Leave_attendence == 1)
            cb_unpaid_leave.Checked = true;
        else
            cb_unpaid_leave.Checked = false;
        if (Maximum_PER_LA == 1)
            cb_max_PER.Checked = true;
        else
            cb_max_PER.Checked = false;
        if (multiple_days == 1)
            cb_formulate_days.Checked = true;
        else
            cb_formulate_days.Checked = false;
    }
    public void clear()
    {
        cb_lopfrom_atn.Checked = false;
        cb_lop_fromgross.Checked = false;
        cb_Lopfrom_basic.Checked = false;
        cb_Lopfrom_payband.Checked = false;
        cb_lopgradpay.Checked = false;
        rdb_month.Checked = true;
        rdb_Days.Checked = false;
        cb_absentcalculation.Checked = false;
        cb_LOP_to_PF.Checked = false;
        cb_auto_deduct.Checked = false;
        cb_NA_RL.Checked = false;
        cb_unpaid_leave.Checked = false;
        cb_max_PER.Checked = false;
        cb_formulate_days.Checked = false;
        txt_absent.Text = "";
        txt_totaldays.Text = "";
        txt_days.Text = "";
        txt_fpf.Text = "";
        txt_age_val.Text = "";
        txt_max_amount.Text = "";
        txt_after.Text = "";
    }
    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        string collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        try
        {
            payprocess();
            if (genchk == true)
            {
                int LOP_From_Attendance = 0;
                int LOP_From_Basic = 0;
                int OP_from_Payband = 0;
                int LOP_from_Gradepay = 0;
                int tot_month_wisedays = 0;
                string maxdays = txt_totaldays.Text.ToString();
                string perlopdet = "";
                if (maxdays == "")
                    maxdays = "0";
                int Absent_Calculation = 0;
                string absentcal = txt_absent.Text.ToString();
                if (absentcal == "")
                    absentcal = "0";
                int NA_RL_take_attendance_from_month = 0;
                int LOP_PF = 0;
                string LOP_PF_days = txt_days.Text.ToString();
                if (LOP_PF_days == "")
                    LOP_PF_days = "0";
                string FPF_Calculation = txt_fpf.Text.ToString();
                if (FPF_Calculation == "")
                    FPF_Calculation = "0";
                string Age_Less_than = txt_age_val.Text.ToString();
                if (Age_Less_than == "")
                    Age_Less_than = "0";
                string Maximum_Amount = txt_max_amount.Text.ToString();
                if (Maximum_Amount == "")
                    Maximum_Amount = "0";
                int Auto_Deduct = 0;
                int Unpaid_Leave_attendence = 0;
                int Maximum_PER_LA = 0;
                string MaxPerVal_after = txt_after.Text.ToString();
                if (MaxPerVal_after == "")
                    MaxPerVal_after = "0";
                int multiple_days = 0;
                int LOP_From_Gross = 0;
                if (cb_lopfrom_atn.Checked == true)
                    LOP_From_Attendance = 1;
                if (cb_lop_fromgross.Checked == true)
                    LOP_From_Gross = 1;
                if (cb_Lopfrom_basic.Checked == true)
                    LOP_From_Basic = 1;
                if (cb_Lopfrom_payband.Checked == true)
                    OP_from_Payband = 1;
                if (cb_lopgradpay.Checked == true)
                    LOP_from_Gradepay = 1;
                if (rdb_month.Checked == true)
                    tot_month_wisedays = 1;
                if (rdb_Days.Checked == true)
                    tot_month_wisedays = 2;
                if (cb_absentcalculation.Checked == true)
                    Absent_Calculation = 1;
                if (cb_NA_RL.Checked == true)
                    NA_RL_take_attendance_from_month = 1;
                if (cb_LOP_to_PF.Checked == true)
                    LOP_PF = 1;
                if (cb_auto_deduct.Checked == true)
                    Auto_Deduct = 1;
                if (cb_unpaid_leave.Checked == true)
                    Unpaid_Leave_attendence = 1;
                if (cb_max_PER.Checked == true)
                    Maximum_PER_LA = 1;
                if (cb_formulate_days.Checked == true)
                {
                    multiple_days = 1;
                    foreach (GridViewRow gv in grid_multiple_days.Rows)
                    {
                        TextBox txtfrm = (TextBox)gv.FindControl("txt_from");
                        TextBox txtto = (TextBox)gv.FindControl("txt_To");
                        TextBox grdlopdays = (TextBox)gv.FindControl("txt_LOP_days");
                        if (txtfrm.Text.Trim() != "" && txtto.Text.Trim() != "" && grdlopdays.Text.Trim() != "")
                        {
                            if (perlopdet.Trim() == "")
                                perlopdet = Convert.ToString(txtfrm.Text.Trim()) + ";" + Convert.ToString(txtto.Text.Trim()) + ";" + Convert.ToString(grdlopdays.Text.Trim()) + ";";
                            else
                                perlopdet = perlopdet + "\\" + Convert.ToString(txtfrm.Text.Trim()) + ";" + Convert.ToString(txtto.Text.Trim()) + ";" + Convert.ToString(grdlopdays.Text.Trim()) + ";";
                        }
                    }
                }
                int HrWise = 0;
                string Stfcategory = GetSelectedItemsValue(cblHrStfCat);
                if (ChkHourWise.Checked && !String.IsNullOrEmpty(Stfcategory))
                    HrWise = 1;
                int ClCalc = 0;
                string ClMonYrFrm = string.Empty;
                string ClMonYrTo = string.Empty;
                if (chk_CLCalc.Checked == true)
                {
                    ClCalc = 1;
                    ClMonYrFrm = Convert.ToString(ddlFromMonth.SelectedItem.Value) + "/" + Convert.ToString(ddlFromYear.SelectedItem.Text);
                    ClMonYrTo = Convert.ToString(ddlToMonth.SelectedItem.Value) + "/" + Convert.ToString(ddlToYear.SelectedItem.Text);
                }
                string insert = "if exists (select * from HR_PaySettings where College_Code='" + collegecode1 + "') update  HR_PaySettings set IsAttnLOP ='" + LOP_From_Attendance + "', LOPGross='" + LOP_From_Gross + "',LOPBasic='" + LOP_From_Basic + "',LOPPB='" + OP_from_Payband + "',LOPGP='" + LOP_from_Gradepay + "',SalCalMaxType='" + tot_month_wisedays + "',SalCalMaxDays='" + maxdays + "',IsAbsCal='" + Absent_Calculation + "',AbsCalPer='" + absentcal + "',AttCurForNA='" + NA_RL_take_attendance_from_month + "',IsPFLopDays='" + LOP_PF + "',PFLopDays='" + LOP_PF_days + "',FPFPer='" + FPF_Calculation + "',FPFAge='" + Age_Less_than + "',FPFMaxAmt='" + Maximum_Amount + "',IsAutoDeduct='" + Auto_Deduct + "',AttCurForUPL='" + Unpaid_Leave_attendence + "', IsMaxPer='" + Maximum_PER_LA + "',MaxPerVal='" + MaxPerVal_after + "',IsMulMaxPer='" + multiple_days + "',PerLOPDet='" + perlopdet + "',IsHourWise='" + HrWise + "',Category_code='" + Stfcategory + "',Is_CLCalc='" + ClCalc + "',CLCalc_FrmMonYr='" + ClMonYrFrm + "',CLCalc_ToMonYr='" + ClMonYrTo + "' where College_Code='" + collegecode1 + "'  else INSERT INTO HR_PaySettings (IsAttnLOP, LOPGross,LOPBasic,LOPPB,LOPGP,SalCalMaxType, SalCalMaxDays,IsAbsCal,AbsCalPer,AttCurForNA,IsPFLopDays,PFLopDays,FPFPer,FPFAge,FPFMaxAmt,IsAutoDeduct,AttCurForUPL, IsMaxPer,MaxPerVal,IsMulMaxPer,PerLOPDet,College_Code,IsHourWise,Category_code,Is_CLCalc,CLCalc_FrmMonYr,CLCalc_ToMonYr) values ('" + LOP_From_Attendance + "','" + LOP_From_Gross + "','" + LOP_From_Basic + "','" + OP_from_Payband + "','" + LOP_from_Gradepay + "','" + tot_month_wisedays + "','" + maxdays + "','" + Absent_Calculation + "','" + absentcal + "','" + NA_RL_take_attendance_from_month + "','" + LOP_PF + "','" + LOP_PF_days + "','" + FPF_Calculation + "','" + Age_Less_than + "','" + Maximum_Amount + "','" + Auto_Deduct + "' ,'" + Unpaid_Leave_attendence + "','" + Maximum_PER_LA + "','" + MaxPerVal_after + "','" + multiple_days + "','" + perlopdet + "','" + collegecode1 + "','" + HrWise + "','" + Stfcategory + "','" + ClCalc + "','" + ClMonYrFrm + "','" + ClMonYrTo + "')";
                int insertvalue2 = d2.update_method_wo_parameter(insert, "Text");
                alertpopwindow.Visible = true;
                if (lblgetscode.Trim() == "")
                    lblalerterr.Text = "Generated Successfully! - (StaffCount : " + gencount + ")";
                else
                    lblalerterr.Text = "Generated Successfully! - (StaffCount : " + gencount + ")" + "<br />" + " Enter the Attendance Details for the Following Staff - (StaffCount : " + missedcount + ")" + "<br />" + lblgetscode;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx"); }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void rdb_Days_CheckedChanged(object sender, EventArgs e)
    {
        txt_totaldays.Enabled = true;
    }
    protected void rdb_month_CheckedChanged(object sender, EventArgs e)
    {
        txt_totaldays.Enabled = false;
        txt_totaldays.Text = "";
    }
    public void chked()
    {
        cb_lop_fromgross.Checked = false;
        cb_Lopfrom_basic.Enabled = true;
        cb_Lopfrom_payband.Enabled = true;
        cb_lopgradpay.Enabled = true;
        cb_ptgrosslop.Enabled = true; //poomalar 25.10.17
    }
    protected void cb_lop_fromgross_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_lop_fromgross.Checked == true)
        {
            cb_Lopfrom_basic.Checked = false;
            cb_Lopfrom_payband.Checked = false;
            cb_lopgradpay.Checked = false;
            cb_ptgrosslop.Checked = false;//poomalar 25.10.17
            cb_Lopfrom_basic.Enabled = false;
            cb_Lopfrom_payband.Enabled = false;
            cb_lopgradpay.Enabled = false;
            cb_ptgrosslop.Enabled = true; //poomalar 25.10.17
        }
        else
        {
            chked();
        }
    }
    protected void cb_Lopfrom_basic_CheckedChanged(object sender, EventArgs e)
    {
        chked();
        //cb_Lopfrom_basic.Checked = true;
    }
    protected void cb_Lopfrom_payband_CheckedChanged(object sender, EventArgs e)
    {
        chked();
        //cb_Lopfrom_payband.Checked = true;
    }
    protected void cb_lopgradpay_CheckedChanged(object sender, EventArgs e)
    {
        chked();
        //cb_lopgradpay.Checked = true;
    }
    //poomalar 25.10.17
    protected void cb_ptgrosslop_CheckedChanged(object sender, EventArgs e) /*poomalar 24.10.17*/
    {
        chked();
    }
    protected void cb_LOP_to_PF_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_LOP_to_PF.Checked == true)
        {
            txt_days.Enabled = true;
            txt_days.Text = "";
        }
        else
        {
            txt_days.Enabled = false;
            txt_days.Text = "";
        }
    }
    protected void cb_max_PER_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_max_PER.Checked == true)
        {
            txt_after.Enabled = true;
            cb_formulate_days.Enabled = true;
            txt_after.Text = "";
        }
        else
        {
            txt_after.Enabled = false;
            cb_formulate_days.Enabled = false;
            txt_after.Text = "";
            cb_formulate_days.Checked = false;
            grid_multiple_days.Visible = false;
        }
    }
    protected void cb_formulate_days_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_formulate_days.Checked == true)
        {
            grid_multiple_days.Visible = true;
            bindGrid();
        }
        else
        {
            grid_multiple_days.Visible = false;
        }
    }
    protected void txt_from_OnTextChanged(object sender, EventArgs e)
    {
    }
    protected void txt_To_OnTextChanged(object sender, EventArgs e)
    {
    }
    protected void txt_LOP_days_OnTextChanged(object sender, EventArgs e)
    {
    }
    public void bindGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("From");
        dt.Columns.Add("To");
        dt.Columns.Add("LOPDays");
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            DataRow dr;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            grid_multiple_days.DataSource = dt;
            grid_multiple_days.DataBind();
        }
    }
    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }
    //public void bindyear()
    //{
    //    try
    //    {
    //        string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
    //        ddl_fromyr.Items.Clear();
    //        ddl_toyr.Items.Clear();
    //        ds.Clear();
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = d2.select_method_wo_parameter("select distinct YEAR(To_Date) as PayYear from HrPayMonths where College_Code='" + collegecode + "' order by PayYear", "Text");
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_fromyr.DataSource = ds;
    //            ddl_fromyr.DataTextField = "PayYear";
    //            ddl_fromyr.DataValueField = "PayYear";
    //            ddl_fromyr.DataBind();
    //            ddl_fromyr.Items.Insert(0, "Select");
    //            ddl_toyr.DataSource = ds;
    //            ddl_toyr.DataTextField = "PayYear";
    //            ddl_toyr.DataValueField = "PayYear";
    //            ddl_toyr.DataBind();
    //            ddl_toyr.Items.Insert(0, "Select");
    //        }
    //        else
    //        {
    //            ddl_fromyr.Items.Insert(0, "Select");
    //            ddl_toyr.Items.Insert(0, "Select");
    //        }
    //    }
    //    catch { }
    //}
    protected void binddept()//delsi
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            cbldeptcom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct Dept_Code,Dept_Name from hrdept_master where college_code='" + collcode + "' order by Dept_Name ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
                cbl_dept.DataBind();
                cbldeptcom.DataSource = ds;
                cbldeptcom.DataTextField = "Dept_Name";
                cbldeptcom.DataValueField = "Dept_Code";
                cbldeptcom.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }
                if (cbldeptcom.Items.Count > 0)
                {
                    for (int i = 0; i < cbldeptcom.Items.Count; i++)
                    {
                        cbldeptcom.Items[i].Selected = true;
                    }
                    txtdeptcom.Text = "Department (" + cbldeptcom.Items.Count + ")";
                    cbdeptcom.Checked = true;
                }
                designation();
            }
            else
            {
                txt_dept.Text = "--Select--";
                txtdeptcom.Text = "--Select--";
                cb_dept.Checked = false;
                cbdeptcom.Checked = false;
            }
        }
        catch { }
    }
    protected void bindstftype()
    {
        try
        {
            ds.Clear();
            cbl_stype.Items.Clear();
            cblstftypecom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stype.DataSource = ds;
                cbl_stype.DataTextField = "stftype";
                cbl_stype.DataBind();
                cblstftypecom.DataSource = ds;
                cblstftypecom.DataTextField = "stftype";
                cblstftypecom.DataBind();
                if (cbl_stype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stype.Items.Count; i++)
                    {
                        cbl_stype.Items[i].Selected = true;
                    }
                    txt_stype.Text = "Staff Type (" + cbl_stype.Items.Count + ")";
                    cb_stype.Checked = true;
                }
                if (cblstftypecom.Items.Count > 0)
                {
                    for (int i = 0; i < cblstftypecom.Items.Count; i++)
                    {
                        cblstftypecom.Items[i].Selected = true;
                    }
                    txtstftypecom.Text = "Staff Type (" + cblstftypecom.Items.Count + ")";
                    cbstftypecom.Checked = true;
                }
            }
            else
            {
                txt_stype.Text = "--Select--";
                txtstftypecom.Text = "--Select--";
                cb_stype.Checked = false;
                cbstftypecom.Checked = false;
            }
        }
        catch { }
    }
    protected void staffcategory()
    {
        try
        {
            ds.Clear();
            cbl_scat.Items.Clear();
            cblscatcom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct category_name,category_code from staffcategorizer where college_code= '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_scat.DataSource = ds;
                cbl_scat.DataTextField = "category_name";
                cbl_scat.DataValueField = "category_code";
                cbl_scat.DataBind();
                cblscatcom.DataSource = ds;
                cblscatcom.DataTextField = "category_name";
                cblscatcom.DataValueField = "category_code";
                cblscatcom.DataBind();
                if (cbl_scat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_scat.Items.Count; i++)
                    {
                        cbl_scat.Items[i].Selected = true;
                    }
                    txt_scat.Text = "Staff Category (" + cbl_scat.Items.Count + ")";
                    cb_scat.Checked = true;
                }
                if (cblscatcom.Items.Count > 0)
                {
                    for (int i = 0; i < cblscatcom.Items.Count; i++)
                    {
                        cblscatcom.Items[i].Selected = true;
                    }
                    txtscatcom.Text = "Staff Category (" + cblscatcom.Items.Count + ")";
                    cbscatcom.Checked = true;
                }
            }
            else
            {
                txt_scat.Text = "--Select--";
                txtscatcom.Text = "--Select--";
                cb_scat.Checked = false;
                cbscatcom.Checked = false;
            }
        }
        catch { }
    }
    protected void grid_multiple_days_DataBound(object sender, EventArgs e)
    {
        // Div2.Visible = true;
    }
    protected void cb_absentcalculation_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_absentcalculation.Checked == true)
            txt_absent.Enabled = true;
        else
            txt_absent.Enabled = false;
    }
    public int tot_month_wisedays { get; set; }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    else
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void designation()
    {
        try
        {
            if (chkdept.Checked == true)
            {
                cbl_desig.Items.Clear();
                txt_desig.Text = "--Select--";
                cb_desig.Checked = false;
                Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
                dicgetcode.Clear();
                Dictionary<string, string> dicdescode = new Dictionary<string, string>();
                dicdescode.Clear();
                string collcode = Convert.ToString(ddlcollege.SelectedValue);
                if (cbldeptcom.Items.Count > 0)
                {
                    for (int ik = 0; ik < cbldeptcom.Items.Count; ik++)
                    {
                        if (cbldeptcom.Items[ik].Selected == true)
                        {
                            if (!dicgetcode.ContainsKey(Convert.ToString(cbldeptcom.Items[ik].Value)))
                            {
                                string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbldeptcom.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbldeptcom.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbldeptcom.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbldeptcom.Items[ik].Value) + "'))";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selq, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                    {
                                        if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                                        {
                                            cbl_desig.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                                            dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                                        }
                                    }
                                }
                                dicgetcode.Add(Convert.ToString(cbl_dept.Items[ik].Value), Convert.ToString(cbl_dept.Items[ik].Text));
                            }
                        }
                    }
                }
                if (cbl_desig.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_desig.Items.Count; i++)
                    {
                        cbl_desig.Items[i].Selected = true;
                    }
                    txt_desig.Text = "Designation (" + cbl_desig.Items.Count + ")";
                    cb_desig.Checked = true;
                }
            }
            if (chkdept.Checked == false)
            {
                ds.Clear();
                cbl_desig.Items.Clear();
                CblDesignation.Items.Clear();
                string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' order by desig_name";
                ds = d2.select_method_wo_parameter(statequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_desig.DataSource = ds;
                    cbl_desig.DataTextField = "desig_name";
                    cbl_desig.DataValueField = "desig_code";
                    cbl_desig.DataBind();
                    CblDesignation.DataSource = ds;
                    CblDesignation.DataTextField = "desig_name";
                    CblDesignation.DataValueField = "desig_code";
                    CblDesignation.DataBind();
                    cbl_desig.Visible = true;
                    if (cbl_desig.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_desig.Items.Count; i++)
                        {
                            cbl_desig.Items[i].Selected = true;
                        }
                        txt_desig.Text = "Designation(" + cbl_desig.Items.Count + ")";
                        cb_desig.Checked = true;
                    }
                    if (CblDesignation.Items.Count > 0)
                    {
                        for (int i = 0; i < CblDesignation.Items.Count; i++)
                        {
                            CblDesignation.Items[i].Selected = true;
                        }
                        txtDesignation.Text = "Designation(" + CblDesignation.Items.Count + ")";
                        CbDesignation.Checked = true;
                    }
                }
                else
                {
                    txtDesignation.Text = "--Select--";
                    CbDesignation.Checked = false;
                    txt_desig.Text = "--Select--";
                    cb_desig.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    //ds.Clear();
    //cbl_desig.Items.Clear();
    //CblDesignation.Items.Clear();
    //string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' order by desig_name";
    //ds = d2.select_method_wo_parameter(statequery, "Text");
    //if (ds.Tables[0].Rows.Count > 0)
    //{
    //    cbl_desig.DataSource = ds;
    //    cbl_desig.DataTextField = "desig_name";
    //    cbl_desig.DataValueField = "desig_code";
    //    cbl_desig.DataBind();
    //    CblDesignation.DataSource = ds;
    //    CblDesignation.DataTextField = "desig_name";
    //    CblDesignation.DataValueField = "desig_code";
    //    CblDesignation.DataBind();
    //    cbl_desig.Visible = true;
    //    if (cbl_desig.Items.Count > 0)
    //    {
    //        for (int i = 0; i < cbl_desig.Items.Count; i++)
    //        {
    //            cbl_desig.Items[i].Selected = true;
    //        }
    //        txt_desig.Text = "Designation(" + cbl_desig.Items.Count + ")";
    //        cb_desig.Checked = true;
    //    }
    //    if (CblDesignation.Items.Count > 0)
    //    {
    //        for (int i = 0; i < CblDesignation.Items.Count; i++)
    //        {
    //            CblDesignation.Items[i].Selected = true;
    //        }
    //        txtDesignation.Text = "Designation(" + CblDesignation.Items.Count + ")";
    //        CbDesignation.Checked = true;
    //    }
    //}
    //else
    //{
    //    txtDesignation.Text = "--Select--";
    //    CbDesignation.Checked = false;
    //    txt_desig.Text = "--Select--";
    //    cb_desig.Checked = false;
    //}
    protected void binddesig()
    {
        try
        {
            CblDesignation.Items.Clear();
            txtDesignation.Text = "--Select--";
            CbDesignation.Checked = false;
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (cbl_dept.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_dept.Items.Count; ik++)
                {
                    if (cbl_dept.Items[ik].Selected == true)
                    {
                        if (!dicgetcode.ContainsKey(Convert.ToString(cbl_dept.Items[ik].Value)))
                        {
                            string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                {
                                    if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                                    {
                                        CblDesignation.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                                        dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                                    }
                                }
                            }
                            dicgetcode.Add(Convert.ToString(cbl_dept.Items[ik].Value), Convert.ToString(cbl_dept.Items[ik].Text));
                        }
                    }
                }
            }
            if (CblDesignation.Items.Count > 0)
            {
                for (int i = 0; i < CblDesignation.Items.Count; i++)
                {
                    CblDesignation.Items[i].Selected = true;
                }
                txtDesignation.Text = "Designation (" + CblDesignation.Items.Count + ")";
                CbDesignation.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbDesigChange(object sender, EventArgs e)
    {
        if (cbDesig.Checked == true)
        {
            txt_desig.Enabled = true;
            //binddept();
            designation();
        }
        else
        {
            cbl_desig.Items.Clear();
            txt_desig.Text = "--Select--";
            txt_desig.Enabled = false;
        }
    }
    //protected void cbStaffNameOnchange(object sender, EventArgs e)
    //{
    //    if (cbStaffName.Checked == true)
    //    {
    //        //  BindStaff(0);
    //        txt_staffname.Enabled = true;
    //    }
    //    if (cbStaffName.Checked == false)
    //    {
    //        //cbl_staffname.Items.Clear();
    //        // txt_staffname.Text = "--Select--";
    //        txt_staffname.Enabled = false;
    //        txt_staffname.Text = "";
    //    }
    //}
    //protected void Cb_StaffCodeOnchange(object sender, EventArgs e)
    //{
    //    if (Cb_StaffCode.Checked == true)
    //    {
    //        // BindStaff(1);
    //        txt_StaffCode.Enabled = true;
    //    }
    //    if (Cb_StaffCode.Checked == false)
    //    {
    //        //CblStaffCode.Items.Clear();
    //        // txt_StaffCode.Text = "--Select--";
    //        txt_StaffCode.Enabled = false;
    //        txt_StaffCode.Text = "";
    //    }
    //}
    //public void cb_staffname_CheckedChanged(object sender, EventArgs e)
    //{
    //    chkchange(cb_staffname, cbl_staffname, txt_staffname, "Staff Name");
    //}
    //public void cbl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    chklstchange(cb_staffname, cbl_staffname, txt_staffname, "Staff Name");
    //}
    //protected void BindStaff(byte Type)
    //{
    //    ds.Clear();
    //    CheckBoxList cbl = new CheckBoxList();
    //    TextBox txt = new TextBox();
    //    CheckBox cb = new CheckBox();
    //    string OrderBy = string.Empty;
    //    string Name = string.Empty;
    //    if (Type == 0)
    //    {
    //        cbl = cbl_staffname;
    //        txt = txt_staffname;
    //        cb = cb_staffname;
    //        OrderBy = " staff_name";
    //        Name = "Staff Name";
    //    }
    //    else
    //    {
    //        cbl = CblStaffCode;
    //        txt = txt_StaffCode;
    //        cb = CbStaffCode;
    //        OrderBy = " LEN(sm.staff_code) ,sm.staff_code";
    //        Name = "Staff Code";
    //    }
    //    cbl.Items.Clear();
    //    string statequery = " select sm.staff_code,sm.staff_name from staffmaster sm,stafftrans t where sm.staff_code=t.staff_code and t.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code in('" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') order by " + OrderBy + "";
    //    ds = d2.select_method_wo_parameter(statequery, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        cbl.DataSource = ds;
    //        if (Type == 0)
    //        {
    //            cbl.DataTextField = "staff_name";
    //            cbl.DataValueField = "staff_code";
    //        }
    //        else
    //        {
    //            cbl.DataTextField = "staff_code";
    //            cbl.DataValueField = "staff_code";
    //        }
    //        cbl.DataBind();
    //        cbl.Visible = true;
    //        if (cbl.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl.Items.Count; i++)
    //            {
    //                cbl.Items[i].Selected = true;
    //            }
    //            txt.Text = Name + "(" + cbl.Items.Count + ")";
    //            cb.Checked = true;
    //        }
    //    }
    //    else
    //    {
    //        txt.Text = "--Select--";
    //        cb.Checked = false;
    //    }
    ////}
    //protected void CbStaffCodeCheckedChange(object sender, EventArgs e)
    //{
    //    chkchange(CbStaffCode, CblStaffCode, txt_StaffCode, "Staff Code");
    //}
    //protected void CblStaffCodeSelectedIndexChange(object sender, EventArgs e)
    //{
    //    chklstchange(CbStaffCode, CblStaffCode, txt_StaffCode, "Staff Code");
    //}
    protected void CbDesignCheckedChange(object sender, EventArgs e)
    {
        chkchange(CbDesignation, CblDesignation, txtDesignation, "Designation");
    }
    protected void CblDesignationSelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(CbDesignation, CblDesignation, txtDesignation, "Designation");
    }
    //protected void cbStaff_NameCheckedChanged(object sender, EventArgs e)
    //{
    //    chkchange(cbStaff_Name, cblStaff_Name, txtStaffName, "Staff Name");
    //}
    //protected void cblstaffname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    chklstchange(cbStaff_Name, cblStaff_Name, txtStaffName, "Staff Name");
    //}
    //protected void cbStaff_CodeCheckedChange(object sender, EventArgs e)
    //{
    //    chkchange(cbStaff_Code, cblStaff_Code, txtStaffcode, "Staff Code");
    //}
    //protected void cblStaff_CodeSelectedIndexChange(object sender, EventArgs e)
    //{
    //    chklstchange(cbStaff_Code, cblStaff_Code, txtStaffcode, "Staff Code");
    //}
    //protected void BindStaffIndividual(byte Type)
    //{
    //    ds.Clear();
    //    CheckBoxList cbl = new CheckBoxList();
    //    TextBox txt = new TextBox();
    //    CheckBox cb = new CheckBox();
    //    string OrderBy = string.Empty;
    //    string Name = string.Empty;
    //    if (Type == 0)
    //    {
    //        cbl = cblStaff_Name;
    //        txt = txtStaffName;
    //        cb = cbStaff_Name;
    //        OrderBy = " staff_name";
    //        Name = "Staff Name";
    //    }
    //    else
    //    {
    //        cbl = cblStaff_Code;
    //        txt = txtStaffcode;
    //        cb = cbStaff_Code;
    //        OrderBy = " LEN(sm.staff_code) ,sm.staff_code";
    //        Name = "Staff Code";
    //    }
    //    cbl.Items.Clear();
    //    string statequery = " select sm.staff_code,sm.staff_name from staffmaster sm,stafftrans t where sm.staff_code=t.staff_code and t.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code in('" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') order by " + OrderBy + "";
    //    ds = d2.select_method_wo_parameter(statequery, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        cbl.DataSource = ds;
    //        if (Type == 0)
    //        {
    //            cbl.DataTextField = "staff_name";
    //            cbl.DataValueField = "staff_code";
    //        }
    //        else
    //        {
    //            cbl.DataTextField = "staff_code";
    //            cbl.DataValueField = "staff_name";
    //        }
    //        cbl.DataBind();
    //        cbl.Visible = true;
    //        if (cbl.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl.Items.Count; i++)
    //            {
    //                cbl.Items[i].Selected = true;
    //            }
    //            txt.Text = Name + "(" + cbl.Items.Count + ")";
    //            cb.Checked = true;
    //        }
    //    }
    //    else
    //    {
    //        txt.Text = "--Select--";
    //        cb.Checked = false;
    //    }
    //}
    protected void txt_staffname_change(object sender, EventArgs e)
    {
        //txtappstfapplcode.Text = "";
        //txtappstfcode.Text = "";
    }
    protected void txt_staffcode_change(object sender, EventArgs e)
    {
        //txtappstfapplcode.Text = "";
        //txtappstfcode.Text = "";
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        //string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_name  from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_name like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_code from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_code like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }
    protected void bindsearchstapp()
    {
        ddlsearchappstf.Items.Clear();
        ddlsearchappstf.Items.Add(new ListItem("Select", "0"));
        ddlsearchappstf.Items.Add(new ListItem("Staff Name", "1"));
        ddlsearchappstf.Items.Add(new ListItem("Staff Code", "2"));
        ddlsearchappstf.DataBind();
        ddl_staffNameCode.Items.Clear();
        ddl_staffNameCode.Items.Add(new ListItem("Select", "0"));
        ddl_staffNameCode.Items.Add(new ListItem("Staff Name", "1"));
        ddl_staffNameCode.Items.Add(new ListItem("Staff Code", "2"));
        ddl_staffNameCode.DataBind();
        if (ddl_staffNameCode.SelectedItem.Text == "Select")
        {
            txtStaffcode.Text = "";
            txtStaffName.Text = "";
        }
    }
    protected void ddlsearchappstf_change(object sender, EventArgs e)//delsi
    {
        if (ddlsearchappstf.SelectedItem.Text == "Staff Name")
        {
            txt_staffname.Visible = true;
            txt_staffname.Enabled = true;
            txt_StaffCode.Visible = false;
        }
        if (ddlsearchappstf.SelectedItem.Text == "Staff Code")
        {
            txt_StaffCode.Enabled = true;
            txt_StaffCode.Visible = true;
            txt_staffname.Visible = false;
        }
        txt_StaffCode.Text = "";
        txt_staffname.Text = "";
        //txtappstfname.Text = "";
    }
    protected void ddl_staffNameCode_change(object sender, EventArgs e)//delsi
    {
        if (ddl_staffNameCode.SelectedItem.Text == "Staff Name")
        {
            txtStaffName.Visible = true;
            txtStaffName.Enabled = true;
            txtStaffcode.Visible = false;
        }
        if (ddl_staffNameCode.SelectedItem.Text == "Staff Code")
        {
            txtStaffcode.Enabled = true;
            txtStaffcode.Visible = true;
            txtStaffName.Visible = false;
        }
        txtStaffcode.Text = "";
        txtStaffName.Text = "";
        //txtappstfname.Text = "";
    }
    protected void view_click(object sender, EventArgs e)//delsi1104
    {
        try
        {
            int divheight = 0;
            FarPoint.Web.Spread.StyleInfo darknewstyle = new FarPoint.Web.Spread.StyleInfo();
            darknewstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            darknewstyle.ForeColor = System.Drawing.Color.Black;
            darknewstyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpreadstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darknewstyle;
            FpSpreadstaff.Sheets[0].ColumnCount = 7;
            FpSpreadstaff.Sheets[0].RowCount = 1;
            FpSpreadstaff.Sheets[0].RowHeader.Visible = false;
            FpSpreadstaff.CommandBar.Visible = false;
            FpSpreadstaff.Sheets[0].AutoPostBack = false;


            string dept_code = Convert.ToString(ViewState["department"]);
            DataSet staffdet_ds = new DataSet();
            if (dept_code != "")
            {
                string query = "select s.staff_code,s.staff_name,st.stftype,st.allowances,st.deductions,ISNULL(st.bsalary,'0') as bsalary,ISNULL(st.grade_pay,'0') as grade_pay,ISNULL(pay_band,'0') as pay_band,IsAutoGP, st.isconsolid,app.date_of_birth as birthdate,dept.Dept_Name,desig.desig_name,st.category_code,st.AGP,st.IsManualLOP,st.IsDailyWages,st.IsMPFAmt,st.MPFAmount,st.MPFPer,st.IsConsolid,app.appl_id,dept.dept_code,desig.desig_code,st.stfnature,st.CollegeTransferBankFK from staffmaster s,stafftrans st,staff_appl_master app,desig_master desig,hrdept_master dept where desig.desig_code=st.desig_code and dept.Dept_Code=st.dept_code and s.staff_code =st.staff_code and app.appl_no=s.appl_no and s.college_code=desig.collegeCode and s.college_code=dept.college_code and desig.collegeCode=dept.college_code and st.latestrec ='1' and ((s.resign =0 and s.settled =0) and (Discontinue=0 or Discontinue is null)) and s.college_code ='" + collegecode1 + "' and app.interviewstatus='Appointed' and st.dept_code in('" + dept_code + "')";
                staffdet_ds = d2.select_method_wo_parameter(query, "text");
                if (staffdet_ds.Tables.Count > 0)
                {
                    if (staffdet_ds.Tables[0].Rows.Count > 0)
                    {

                        FarPoint.Web.Spread.StyleInfo style5 = new FarPoint.Web.Spread.StyleInfo();
                        style5.Font.Size = 13;
                        style5.Font.Name = "Book Antiqua";
                        style5.Font.Bold = true;
                        style5.HorizontalAlign = HorizontalAlign.Center;
                        style5.ForeColor = System.Drawing.Color.Black;
                        style5.BackColor = System.Drawing.Color.AliceBlue;
                        FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();

                        cball.AutoPostBack = true;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[0].Width = 50;
                        FpSpreadstaff.Columns[0].Locked = true;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";

                        FpSpreadstaff.Sheets[0].Cells[0, 1].CellType = cball;
                        FpSpreadstaff.Sheets[0].Cells[0, 1].Value = 1;


                        FpSpreadstaff.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[1].Width = 50;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[2].Width = 100;
                        FpSpreadstaff.Columns[2].Locked = true;


                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[3].Width = 150;
                        FpSpreadstaff.Columns[3].Locked = true;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Type";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[4].Width = 150;
                        FpSpreadstaff.Columns[4].Locked = true;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[5].Width = 200;
                        FpSpreadstaff.Columns[5].Locked = true;

                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Designation";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpreadstaff.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadstaff.Columns[6].Width = 150;
                        FpSpreadstaff.Columns[6].Locked = true;


                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        for (int row = 0; row < staffdet_ds.Tables[0].Rows.Count; row++)
                        {

                            FpSpreadstaff.Sheets[0].RowCount++;
                            divheight += 7;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                            check.AutoPostBack = false;

                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 1].CellType = check;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 1].Value = 1;

                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["staff_code"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].CellType = txt;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["staff_code"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["staff_name"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["stftype"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["Dept_Name"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(staffdet_ds.Tables[0].Rows[row]["desig_name"]);
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Small;
                            FpSpreadstaff.Sheets[0].Cells[FpSpreadstaff.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                        }
                        divview.Visible = true;
                        lblerr1.Visible = false;
                        Div3.Visible = true;
                        btnok1.Visible = true;
                        FpSpreadstaff.Visible = true;
                        FpSpreadstaff.Sheets[0].PageSize = FpSpreadstaff.Sheets[0].RowCount;
                        FpSpreadstaff.Width = 800;
                        FpSpreadstaff.Height = 400;
                        FpSpreadstaff.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                        FpSpreadstaff.Sheets[0].FrozenRowCount = 1;
                        staffdetail.Visible = true;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");

        }
    }

    protected void FpSpreadstaff_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpreadstaff.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpreadstaff.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpreadstaff.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpreadstaff.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpreadstaff.Sheets[0].RowCount; i++)
                        {
                            FpSpreadstaff.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpreadstaff.Sheets[0].RowCount; i++)
                        {
                            FpSpreadstaff.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }

    }

    protected void btnok1_Click(object sender, EventArgs e)
    {
        try
        {
            int cellchkcount = 0;
            string actrow = FpSpreadstaff.ActiveSheetView.ActiveRow.ToString();
            string actcol = FpSpreadstaff.ActiveSheetView.ActiveColumn.ToString();

            FpSpreadstaff.SaveChanges();
            string Staff_code = string.Empty;
            string bindstaff_code = string.Empty;
            for (int no = 0; no < FpSpreadstaff.Sheets[0].RowCount; no++)
            {
                if (no == 0)
                    continue;
                int checkno = Convert.ToInt32(FpSpreadstaff.Sheets[0].Cells[no, 1].Value);
                if (checkno == 1)
                {
                    cellchkcount++;

                    Staff_code = Convert.ToString(FpSpreadstaff.Sheets[0].Cells[no, 2].Tag);
                    if (Staff_code != "")
                    {
                        if (bindstaff_code == "")
                        {
                            bindstaff_code = Staff_code;
                        }
                        else
                        {
                            bindstaff_code = bindstaff_code + "','" + Staff_code;
                        }
                    }


                }

            }

            if (cellchkcount == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select any one staff!";
                FpSpreadstaff.Visible = true;

            }
            if (cellchkcount != 0)
            {

                if (bindstaff_code != null)
                {
                    ViewState["staff_code"] = Convert.ToString(bindstaff_code);

                    lnkview.Text = "View Details";
                    FpSpreadstaff.Visible = false;
                    divview.Visible = false;
                    Div3.Visible = false;
                    lblerr1.Visible = false;
                    //alertpopwindow.Visible = true;
                    //lblalerterr.Visible = true;
                    //lblalerterr.Text = "Staff's Selected!";

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
        }
    }

    protected void btnexitstud_Click(object sender, EventArgs e)
    {
        lnkview.Text = "View Details";
        FpSpreadstaff.Visible = false;
        divview.Visible = false;
        Div3.Visible = false;
        lblerr1.Visible = false;

    }
    public void imagebtnorder_Click(object sender, EventArgs e)
    {
        divview.Visible = false;
    }


    public Dictionary<string, string> getdatasval(string slabfor, string amnt, string catcode)//delsi1604
    {
        string amntformat = "";
        string slabvalue = "";
        DataSet dsnewt = new DataSet();
        string collcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        Dictionary<string, string> newdic = new Dictionary<string, string>();
        newdic.Clear();
        try
        {
            string selq = "select slabvalue,slabtype from pfslabs where SlabFor='" + slabfor + "' and category_code='" + catcode + "' and college_code='" + collcode + "'";
            dsnewt.Clear();
            dsnewt = d2.select_method_wo_parameter(selq, "Text");
            if (dsnewt.Tables.Count > 0 && dsnewt.Tables[0].Rows.Count > 0)
            {
                double amount = 0;
                Double.TryParse(Convert.ToString(amnt), out amount);
                amntformat = Convert.ToString(dsnewt.Tables[0].Rows[0]["slabtype"]);
                if (amntformat == "Amount")
                    slabvalue = Convert.ToString(Math.Round((amount * 12) / 100));
                newdic.Add(slabfor, amntformat + "-" + slabvalue);
            }
        }
        catch { }
        return newdic;
    }


    protected void cb_pfCalculation_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_pfCalculation.Checked == true)
        {
           
        }
        else
        {

            
        }
    }

    protected void cb_permonth_CheckedChanged(object sender, EventArgs e)//delsi0606
    {
        if (cb_permonth.Checked == true)
        {
            staffloppermonth();//staffcategory()
            txtloppermonth.Enabled = true;
        }
        else
        {
            cbl_loppermonth.Items.Clear();
            txtloppermonth.Text = "--Select--";
            txtloppermonth.Enabled = false;

        }

    }


    protected void staffloppermonth()
    {
        try
        {
            ds.Clear();
            
            cbl_loppermonth.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            
            string item = "select shortname from leave_category where (status='comp' or status='1') and college_code= '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_loppermonth.DataSource = ds;
                cbl_loppermonth.DataTextField = "shortname";
                cbl_loppermonth.DataValueField = "shortname";
                cbl_loppermonth.DataBind();

                if (cbl_loppermonth.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_loppermonth.Items.Count; i++)
                    {
                        cbl_loppermonth.Items[i].Selected = true;
                    }
                    txtloppermonth.Text = "Per Month LOP(" + cbl_loppermonth.Items.Count + ")";
                    cb_loppermonth.Checked = true;
                }
            }
            else
            {
                txtloppermonth.Text = "--Select--";

                cb_loppermonth.Checked = false;
               
            }
        }
        catch { }
    }


  

    protected void cb_loppermonth_CheckedChange(object sender, EventArgs e)
    {
        chkchanges(cb_loppermonth, cbl_loppermonth, txtloppermonth, "Per Month LOP");
    }


    protected void chkchanges(CheckBox cb_loppermonth, CheckBoxList cbl_loppermonth, TextBox txtloppermonth, string label)
    {
        try
        {
            if (cb_loppermonth.Checked == true)
            {
                for (int i = 0; i < cbl_loppermonth.Items.Count; i++)
                {
                    cbl_loppermonth.Items[i].Selected = true;
                }
                txtloppermonth.Text = label + "(" + Convert.ToString(cbl_loppermonth.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_loppermonth.Items.Count; i++)
                {
                    cbl_loppermonth.Items[i].Selected = false;
                }
                txtloppermonth.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void cb_loppermonth_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchanges(cb_loppermonth, cbl_loppermonth, txtloppermonth, "Per Month LOP");
    }

    protected void chklstchanges(CheckBox cb_loppermonth, CheckBoxList cbl_loppermonth, TextBox txtloppermonth, string label)
    {
        try
        {
            txtloppermonth.Text = "--Select--";
            cb_loppermonth.Checked = false;
            int count = 0;
            for (int i = 0; i < cbl_loppermonth.Items.Count; i++)
            {
                if (cbl_loppermonth.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtloppermonth.Text = label + "(" + count + ")";
                if (count == cbl_loppermonth.Items.Count)
                {
                    cb_loppermonth.Checked = true;
                }
            }
        }
        catch { }
    }

}
#region Unwanted Function
//public void payprocess()
//{
//    string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
//    try
//    {
//        lblgetscode = "";
//        string year = "";
//        string year1 = "";
//        string month = "";
//        string month1 = "";
//        string endyear = "";
//        int inscount = 0;
//        int savecount = 0;
//        int insitcount = 0;
//        Div2.Visible = true;
//        Fpspread1.SaveChanges();
//        string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
//        string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
//        DataView dv = new DataView();
//        ArrayList category = new ArrayList();
//        if (activerow.Trim() != "" && activecol.Trim() != "")
//        {
//            year = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
//            year1 = year;
//            month1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
//            endyear = Convert.ToString(ddl_toyr.SelectedItem.Text);
//            ds.Clear();
//            month = " select hryear_start,hryear_end,Pay_Start,Pay_End from hryears where year(hryear_start)='" + year + "' and collcode='" + collegecode1 + "' order by hryear_end desc";
//            month = month + " select shortname from leave_category where status ='comp' and college_code ='" + collegecode1 + "'";
//            month = month + " select top 10 s.staff_code,s.staff_name,st.stftype,st.allowances,st.deductions,ISNULL(st.bsalary,'0') as bsalary,ISNULL(st.grade_pay,'0') as grade_pay,ISNULL(pay_band,'0') as pay_band,IsAutoGP, st.isconsolid,app.date_of_birth as birthdate,dept.Dept_Name,desig.desig_name,st.category_code,st.AGP from staffmaster s,stafftrans st,staff_appl_master app,desig_master desig,hrdept_master dept where desig.desig_code=st.desig_code and dept.Dept_Code=st.dept_code and s.staff_code =st.staff_code and app.appl_no=s.appl_no and s.college_code=desig.collegeCode and s.college_code=dept.college_code and desig.collegeCode=dept.college_code and st.latestrec ='1' and ((s.resign =0 and s.settled =0) or Discontinue=0 or Discontinue is null) and s.college_code ='" + collegecode1 + "'";
//            ds = d2.select_method_wo_parameter(month, "Text");
//            string mont = "";
//            string dat = "";
//            string yr = "";
//            string mon_year = "";
//            string Pay_Start = "";
//            string pay_end = "";
//            DateTime dt = new DateTime();
//            DateTime dt1 = new DateTime();
//            DateTime newdt = new DateTime();
//            DateTime newdt1 = new DateTime();
//            DataSet dsgetdates = new DataSet();
//            string fromdate = "";
//            string todate = "";
//            string[] splitgetdates = new string[2];
//            if (ds.Tables[1].Rows.Count > 0)
//            {
//                for (int cat = 0; cat < ds.Tables[1].Rows.Count; cat++)
//                {
//                    string shotname = "";
//                    shotname = ds.Tables[1].Rows[cat]["shortname"].ToString();
//                    if (!category.Contains(shotname))
//                    {
//                        category.Add(shotname);
//                    }
//                }
//            }
//            if (cb_lopfrom_atn.Checked == false && (cb_lop_fromgross.Checked == false || cb_Lopfrom_basic.Checked == false || cb_Lopfrom_payband.Checked == false || cb_lopgradpay.Checked == false) && cb_absentcalculation.Checked == false && cb_NA_RL.Checked == false && cb_LOP_to_PF.Checked == false && chk_fpf.Checked == false && cb_auto_deduct.Checked == false && cb_unpaid_leave.Checked == false && cb_max_PER.Checked == false && cbincitcalc.Checked == false)
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please select any one Item and Generate!";
//                return;
//            }
//            if (rdb_Days.Checked == true && (txt_totaldays.Text.Trim() == "" || txt_totaldays.Text.Trim() == "0" || txt_totaldays.Text.Trim() == "0.00"))
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please Enter the Days!";
//                return;
//            }
//            if (cb_absentcalculation.Checked == true && (txt_absent.Text.Trim() == "" || txt_absent.Text.Trim() == "0" || txt_absent.Text.Trim() == "0.00"))
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please Enter the Absent Days!";
//                return;
//            }
//            if (cb_absentcalculation.Checked == false && cb_NA_RL.Checked == true)
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please select Absent Calculation!";
//                return;
//            }
//            if (cb_LOP_to_PF.Checked == true && (txt_days.Text.Trim() == "" || txt_days.Text.Trim() == "0" || txt_days.Text.Trim() == "0.00"))
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please Enter the LOP & PF Max Days!";
//                return;
//            }
//            if (chk_fpf.Checked == true && (txt_fpf.Text.Trim() == "" || txt_fpf.Text.Trim() == "0" || txt_fpf.Text.Trim() == "0.00"))
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please Enter the FPF Percentage!";
//                return;
//            }
//            if (cb_max_PER.Checked == true && (txt_after.Text.Trim() == "" || txt_after.Text.Trim() == "0" || txt_after.Text.Trim() == "0.00"))
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "Please Enter the Max Permission Days!";
//                return;
//            }
//            string strautoded = "";
//            if (cb_auto_deduct.Checked == true)
//            {
//                string selq = d2.GetFunction("select deductions from incentives_master and college_code='" + collegecode1 + "'");
//                if (selq.Trim() != "" && selq.Trim() != "0")
//                {
//                    string[] dedspl = selq.Split(';');
//                    if (dedspl.Length > 0)
//                    {
//                        for (int ik = 0; ik < dedspl.Length; ik++)
//                        {
//                            string[] splval = dedspl[ik].Split('\\');
//                            if (splval.Length == 3)
//                            {
//                                if (splval[1].Trim() != "" && splval[1].Trim() != "0" && (splval[1].Trim() == "2" || splval[1].Trim() == "3"))
//                                {
//                                    if (strautoded.Trim() == "")
//                                    {
//                                        strautoded = splval[0];
//                                    }
//                                    else
//                                    {
//                                        strautoded = strautoded + "," + splval[0];
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            if (ds.Tables[2].Rows.Count > 0)
//            {
//                for (int scd = 0; scd < ds.Tables[2].Rows.Count; scd++)
//                {
//                    string staf_cd = "";
//                    string bas_salary = "";
//                    string[] spl = new string[2];
//                    int bornyear = 0;
//                    int bornmon = 0;
//                    int bornday = 0;
//                    DateTime borndate = new DateTime();
//                    DateTime currdate = DateTime.Now;
//                    string joindt = "";
//                    string relievedt = "";
//                    int joinday = 0;
//                    int joinmon = 0;
//                    int joinyear = 0;
//                    int relieveday = 0;
//                    int relievemon = 0;
//                    int relieveyear = 0;
//                    string[] spljoin = new string[2];
//                    string[] splrelieve = new string[2];
//                    int curryear = currdate.Year;
//                    int currmon = currdate.Month;
//                    int currday = currdate.Day;
//                    int age = 0;
//                    staf_cd = ds.Tables[2].Rows[scd]["staff_code"].ToString();
//                    bas_salary = ds.Tables[2].Rows[scd]["bsalary"].ToString();
//                    string grade_pay = ds.Tables[2].Rows[scd]["grade_pay"].ToString();
//                    string cb_payband = ds.Tables[2].Rows[scd]["pay_band"].ToString();
//                    string auto_GP = ds.Tables[2].Rows[scd]["IsAutoGP"].ToString();
//                    string dateofbirth = ds.Tables[2].Rows[scd]["birthdate"].ToString();
//                    string deptname = ds.Tables[2].Rows[scd]["Dept_Name"].ToString();
//                    string designame = ds.Tables[2].Rows[scd]["desig_name"].ToString();
//                    string catcode = ds.Tables[2].Rows[scd]["category_code"].ToString();
//                    string stftype = ds.Tables[2].Rows[scd]["stftype"].ToString();
//                    string agp = ds.Tables[2].Rows[scd]["AGP"].ToString();
//                    if (dateofbirth.Trim() != "")
//                    {
//                        spl = dateofbirth.Split(' ')[0].Split('/');
//                        Int32.TryParse(spl[1], out bornday);
//                        Int32.TryParse(spl[0], out bornmon);
//                        Int32.TryParse(spl[2], out bornyear);
//                        borndate = Convert.ToDateTime(spl[0] + "/" + spl[1] + "/" + spl[2]);
//                    }
//                    if (borndate != null)
//                    {
//                        age = curryear - bornyear;
//                    }
//                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                    {
//                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//                        {
//                            int st_yer = 0;
//                            int st_month = 0;
//                            int eyear = 0;
//                            st_month = Convert.ToInt32(month1) - 1;
//                            st_yer = Convert.ToInt32(year);
//                            eyear = Convert.ToInt32(endyear);
//                            if (month1 == "1")
//                            {
//                                st_yer = Convert.ToInt32(year) - 1;
//                                st_month = 12;
//                            }
//                            Pay_Start = ds.Tables[0].Rows[i]["Pay_Start"].ToString();
//                            string endday = monthdays(month1, year);
//                            pay_end = endday;
//                            string getdates = "select From_Date,To_Date from HrPayMonths where PayMonthNum='" + month1 + "' and PayYear='" + year + "' and College_Code='" + collegecode1 + "'";
//                            dsgetdates.Clear();
//                            dsgetdates = d2.select_method_wo_parameter(getdates, "Text");
//                            if (dsgetdates.Tables.Count > 0)
//                            {
//                                if (dsgetdates.Tables[0].Rows.Count > 0)
//                                {
//                                    fromdate = Convert.ToString(dsgetdates.Tables[0].Rows[0]["From_Date"]);
//                                    splitgetdates = fromdate.Split('/');
//                                    dt = Convert.ToDateTime(splitgetdates[0] + "/" + splitgetdates[1] + "/" + splitgetdates[2]);
//                                    todate = Convert.ToString(dsgetdates.Tables[0].Rows[0]["To_Date"]);
//                                    splitgetdates = todate.Split('/');
//                                    dt1 = Convert.ToDateTime(splitgetdates[0] + "/" + splitgetdates[1] + "/" + splitgetdates[2]);
//                                    newdt = dt;
//                                    newdt1 = dt1;
//                                }
//                                else
//                                {
//                                    alertpopwindow.Visible = true;
//                                    lblalerterr.Text = "Please Select the Corresponding HR Pay Months!";
//                                    return;
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        alertpopwindow.Visible = true;
//                        lblalerterr.Text = "Please Select the HR Year!";
//                        return;
//                    }
//                    ds5.Clear();
//                    string current_Join_relive = "select Convert(varchar(10),relieve_date,103) as relieve_date,Convert(varchar(10),join_date,103) as join_date from staffmaster where resign = 1 and settled = 1 and relieve_date between ('" + dt + "') and ('" + dt1 + "') and staff_code = '" + staf_cd + "' and college_code='" + collegecode1 + "'";
//                    ds5 = d2.select_method_wo_parameter(current_Join_relive, "Text");
//                    if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
//                    {
//                        pay_end = monthdays(month1, year);
//                        Pay_Start = "1";
//                        joindt = Convert.ToString(ds5.Tables[0].Rows[0]["join_date"]);
//                        spljoin = joindt.Split('/');
//                        if (spljoin.Length > 0)
//                        {
//                            joinday = Convert.ToInt32(spljoin[0]);
//                            joinmon = Convert.ToInt32(spljoin[1]);
//                            joinyear = Convert.ToInt32(spljoin[2]);
//                        }
//                        relievedt = Convert.ToString(ds5.Tables[0].Rows[0]["relieve_date"]);
//                        splrelieve = relievedt.Split('/');
//                        if (splrelieve.Length > 0)
//                        {
//                            relieveday = Convert.ToInt32(splrelieve[0]);
//                            relievemon = Convert.ToInt32(splrelieve[1]);
//                            relieveyear = Convert.ToInt32(splrelieve[2]);
//                        }
//                        if (joinmon == relievemon && joinyear == relieveyear && joinday < relieveday)
//                        {
//                            fromdate = Convert.ToString(joinmon) + "/" + Convert.ToString(joinday) + "/" + Convert.ToString(joinyear);
//                            todate = Convert.ToString(relievemon) + "/" + Convert.ToString(relieveday) + "/" + Convert.ToString(relieveyear);
//                        }
//                        else
//                        {
//                            fromdate = Convert.ToString(Convert.ToString(relievemon) + "/" + Convert.ToString(Pay_Start) + "/" + Convert.ToString(relieveyear));
//                            todate = Convert.ToString(Convert.ToString(relievemon) + "/" + Convert.ToString(relieveday) + "/" + Convert.ToString(relieveyear));
//                        }
//                        dt = Convert.ToDateTime(fromdate);
//                        dt1 = Convert.ToDateTime(todate);
//                    }
//                    int count = 0;
//                    int cabscount = 0;
//                    int permissioncount = 0;
//                    int notappcount = 0;
//                    int mon_day = 0;
//                    int workdays = 0;
//                    int prelop = 0;
//                    int curlop = 0;
//                    int newmon = 0;
//                    int rlcount = 0;
//                    int pcount = 0;
//                    int cpcount = 0;
//                    int hcount = 0;
//                    int lacount = 0;
//                    int odcount = 0;
//                    int empcount = 0;
//                    int clecount = 0;
//                    string[] lp = new string[2];
//                    if (dt.Day > 1 && dt.Day < 31)
//                    {
//                        int mon = dt.Month;
//                        int convyear = dt.Year;
//                        string endday = monthdays(Convert.ToString(mon), Convert.ToString(convyear));
//                        DateTime dtendday = new DateTime();
//                        dtendday = Convert.ToDateTime(Convert.ToString(mon) + "/" + endday + "/" + Convert.ToString(convyear));
//                        while (dt <= dtendday)
//                        {
//                            newmon = 1;
//                            mon_day++;
//                            string dts = Convert.ToString(dt);
//                            string[] sp = dts.Split(' ');
//                            string firstdate = sp[0].ToString();
//                            string[] split = firstdate.Split('/');
//                            dat = split[1].ToString();
//                            dat = dat.TrimStart('0');
//                            string lop_morn = "";
//                            string lop_even = "";
//                            mont = split[0].ToString();
//                            mont = mont.TrimStart('0');
//                            yr = split[2].ToString();
//                            mon_year = mont + "/" + yr;
//                            string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
//                            if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
//                            {
//                                lp = atten.Split('-');
//                                if (lp.Length > 0)
//                                {
//                                    lop_morn = lp[0].ToString();
//                                    lop_even = lp[1].ToString();
//                                }
//                                if (category.Contains(lop_morn))
//                                {
//                                    count++;
//                                    prelop++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_morn == "A")
//                                {
//                                    count++;
//                                    prelop++;
//                                    cabscount++;
//                                }
//                                else if (lop_morn == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_morn == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_morn == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "")
//                                {
//                                    empcount++;
//                                }
//                                if (category.Contains(lop_even))
//                                {
//                                    count++;
//                                    prelop++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_even == "A")
//                                {
//                                    count++;
//                                    prelop++;
//                                    cabscount++;
//                                }
//                                else if (lop_even == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_even == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_even == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "")
//                                {
//                                    empcount++;
//                                }
//                            }
//                            else
//                            {
//                                notappcount = notappcount + (newmon * 2);
//                            }
//                            dt = dt.AddDays(1);
//                        }
//                        dtendday = dtendday.AddDays(1);
//                        while (dtendday <= dt1)
//                        {
//                            mon_day++;
//                            newmon = 1;
//                            string dts = Convert.ToString(dtendday);
//                            string[] sp = dts.Split(' ');
//                            string firstdate = sp[0].ToString();
//                            string[] split = firstdate.Split('/');
//                            dat = split[1].ToString();
//                            dat = dat.TrimStart('0');
//                            string lop_morn = "";
//                            string lop_even = "";
//                            mont = split[0].ToString();
//                            mont = mont.TrimStart('0');
//                            yr = split[2].ToString();
//                            mon_year = mont + "/" + yr;
//                            string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
//                            if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
//                            {
//                                lp = atten.Split('-');
//                                if (lp.Length > 0)
//                                {
//                                    lop_morn = lp[0].ToString();
//                                    lop_even = lp[1].ToString();
//                                }
//                                if (category.Contains(lop_morn))
//                                {
//                                    count++;
//                                    curlop++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_morn == "A")
//                                {
//                                    count++;
//                                    curlop++;
//                                    cabscount++;
//                                }
//                                else if (lop_morn == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_morn == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_morn == "")
//                                {
//                                    empcount++;
//                                }
//                                if (category.Contains(lop_even))
//                                {
//                                    count++;
//                                    curlop++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_even == "A")
//                                {
//                                    count++;
//                                    curlop++;
//                                    cabscount++;
//                                }
//                                else if (lop_even == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_even == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_even == "")
//                                {
//                                    empcount++;
//                                }
//                            }
//                            else
//                            {
//                                notappcount = notappcount + (newmon * 2);
//                            }
//                            dtendday = dtendday.AddDays(1);
//                        }
//                    }
//                    else
//                    {
//                        while (dt <= dt1)
//                        {
//                            mon_day++;
//                            newmon = 1;
//                            string dts = Convert.ToString(dt);
//                            string[] sp = dts.Split(' ');
//                            string firstdate = sp[0].ToString();
//                            string[] split = firstdate.Split('/');
//                            dat = split[1].ToString();
//                            dat = dat.TrimStart('0');
//                            mont = split[0].ToString();
//                            mont = mont.TrimStart('0');
//                            yr = split[2].ToString();
//                            mon_year = mont + "/" + yr;
//                            string lop_morn = "";
//                            string lop_even = "";
//                            string atten = d2.GetFunction("select [" + dat + "] from staff_attnd where staff_code ='" + staf_cd + "' and mon_year ='" + mon_year + "'");
//                            if (atten.Trim() != "0" && atten.Trim() != "" && atten.Trim() != null)
//                            {
//                                lp = atten.Split('-');
//                                if (lp.Length > 0)
//                                {
//                                    lop_morn = lp[0].ToString();
//                                    lop_even = lp[1].ToString();
//                                }
//                                if (category.Contains(lop_morn))
//                                {
//                                    count++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_morn == "A")
//                                {
//                                    count++;
//                                    cabscount++;
//                                }
//                                else if (lop_morn == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_morn == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_morn == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_morn == "")
//                                {
//                                    empcount++;
//                                }
//                                if (category.Contains(lop_even))
//                                {
//                                    count++;
//                                    cabscount++;
//                                }
//                                else
//                                {
//                                    cpcount++;
//                                    clecount++;
//                                }
//                                if (lop_even == "A")
//                                {
//                                    count++;
//                                    cabscount++;
//                                }
//                                else if (lop_even == "LA")
//                                {
//                                    lacount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "H")
//                                {
//                                    hcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "OD")
//                                {
//                                    odcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "P")
//                                {
//                                    pcount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "NA")
//                                {
//                                    notappcount++;
//                                }
//                                else if (lop_even == "PER")
//                                {
//                                    permissioncount++;
//                                    cpcount++;
//                                }
//                                else if (lop_even == "RL")
//                                {
//                                    rlcount++;
//                                }
//                                else if (lop_even == "")
//                                {
//                                    empcount++;
//                                }
//                            }
//                            else
//                            {
//                                notappcount = notappcount + (newmon * 2);
//                            }
//                            dt = dt.AddDays(1);
//                        }
//                    }
//                    if (mon_day != 0)
//                    {
//                        if (notappcount / 2 == mon_day)
//                        {
//                            if (lblgetscode.Trim() == "")
//                            {
//                                lblgetscode = Convert.ToString(staf_cd);
//                            }
//                            else
//                            {
//                                lblgetscode = lblgetscode + "<br />" + Convert.ToString(staf_cd);
//                            }
//                        }
//                        else
//                        {
//                            if (rdb_Days.Checked == true)
//                            {
//                                if (txt_totaldays.Text != "")
//                                {
//                                    mon_day = Convert.ToInt32(txt_totaldays.Text.ToString());
//                                }
//                            }
//                            cpcount = cpcount / 2;
//                            cabscount = cabscount / 2;
//                            count = count / 2;
//                            permissioncount = permissioncount / 2;
//                            lacount = lacount / 2;
//                            odcount = odcount / 2;
//                            hcount = hcount / 2;
//                            empcount = empcount / 2;
//                            notappcount = notappcount / 2;
//                            rlcount = rlcount / 2;
//                            if (cb_NA_RL.Checked == true)
//                            {
//                                workdays = mon_day - (notappcount + rlcount);
//                            }
//                            Double lose_days = 0;
//                            lose_days = count;
//                            Double absdays = 0;
//                            absdays = cabscount;
//                            Double percount = 0;
//                            percount = permissioncount;
//                            Double pre_lop = 0;
//                            pre_lop = prelop / 2;
//                            Double cur_lop = 0;
//                            cur_lop = curlop / 2;
//                            if (cb_max_PER.Checked == true)
//                            {
//                                if (cb_formulate_days.Checked == false)
//                                {
//                                    if (txt_after.Text.Trim() != "")
//                                    {
//                                        if (percount != 0)
//                                        {
//                                            if (percount > Convert.ToDouble(txt_after.Text.Trim()))
//                                            {
//                                                percount = percount - Convert.ToDouble(txt_after.Text.Trim());
//                                            }
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    foreach (GridViewRow gv in grid_multiple_days.Rows)
//                                    {
//                                        TextBox txtfrm = (TextBox)gv.FindControl("txt_from");
//                                        TextBox txtto = (TextBox)gv.FindControl("txt_To");
//                                        TextBox lopdays = (TextBox)gv.FindControl("txt_LOP_days");
//                                        if (txtfrm.Text.Trim() != "" && txtto.Text.Trim() != "" && lopdays.Text.Trim() != "")
//                                        {
//                                            if (percount != 0)
//                                            {
//                                                if (Convert.ToDouble(txtfrm.Text.Trim()) <= percount && Convert.ToDouble(txtto.Text.Trim()) >= percount)
//                                                {
//                                                    if (percount > Convert.ToDouble(lopdays.Text.Trim()))
//                                                    {
//                                                        percount = percount - Convert.ToDouble(lopdays.Text.Trim());
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            if (cb_absentcalculation.Checked == true)
//                            {
//                                if (txt_absent.Text != "")
//                                {
//                                    lose_days = Convert.ToDouble(txt_absent.Text) * 2;
//                                }
//                            }
//                            if (cb_LOP_to_PF.Checked == true)
//                            {
//                                Double loppf = Convert.ToDouble(txt_days.Text);
//                                if (loppf != 0)
//                                {
//                                    if (mon_day > loppf)
//                                    {
//                                        if (lose_days > loppf)
//                                        {
//                                            lose_days = lose_days - loppf;
//                                        }
//                                    }
//                                }
//                            }
//                            if (percount != 0)
//                            {
//                                lose_days = lose_days + percount;
//                            }
//                            Double presdays = mon_day - lose_days;
//                            Double oned_salary = Convert.ToDouble(bas_salary) / mon_day;
//                            Double pres_month_totalsalry = 0;
//                            Double lopdaysin = lose_days + pre_lop + cur_lop;
//                            Double lopamnt = lose_days * oned_salary;
//                            Double basicwolop = Convert.ToDouble(bas_salary);
//                            Double gradepaywlop = 0;
//                            pres_month_totalsalry = Convert.ToDouble(presdays * oned_salary);
//                            string leavedetail = Convert.ToString(mon_day) + ";" + Convert.ToString(presdays) + ";" + Convert.ToString(absdays) + ";0;0;0;" + Convert.ToString(lopdaysin) + ";0;0;0;0;0;0;\"";
//                            if (grade_pay.Trim() != "0" && Convert.ToDouble(grade_pay) > lopamnt)
//                            {
//                                gradepaywlop = Convert.ToDouble(grade_pay) - lopamnt;
//                            }
//                            if (cb_Lopfrom_basic.Checked == true)
//                            {
//                                if (Convert.ToDouble(bas_salary) > lopamnt)
//                                {
//                                    bas_salary = Convert.ToString(Convert.ToDouble(bas_salary) - lopamnt);
//                                }
//                            }
//                            if (cb_Lopfrom_payband.Checked == true)
//                            {
//                                if (Convert.ToDouble(cb_payband) > lopamnt)
//                                {
//                                    cb_payband = Convert.ToString(Convert.ToDouble(cb_payband) - lopamnt);
//                                }
//                            }
//                            if (cb_lopgradpay.Checked == true)
//                            {
//                                if (Convert.ToDouble(grade_pay) > lopamnt)
//                                {
//                                    grade_pay = Convert.ToString(Convert.ToDouble(grade_pay) - lopamnt);
//                                }
//                            }
//                            //select allowances
//                            Double Gross_salary = 0;
//                            Double netsalary = 0;
//                            Double Persent = 0;
//                            Double DA = 0;
//                            Double daamnt = 0;
//                            Double DABasic = 0;
//                            Double esisal = 0;
//                            Double slabsal = 0;
//                            Double pfsal = 0;
//                            Double detslap = 0;
//                            string index = "";
//                            string allowence = "";
//                            Double totallow = 0;
//                            Double lop_amou = 0;
//                            Double persallow = 0;
//                            allowence = ds.Tables[2].Rows[scd]["allowances"].ToString();
//                            string[] slash_split = allowence.Split('\\');
//                            string format = "";
//                            for (int i = 0; i < slash_split.Length; i++)
//                            {
//                                string groupspl = slash_split[i].ToString();
//                                if (groupspl != "")
//                                {
//                                    string[] semicol = groupspl.Split(';');
//                                    if (semicol.Length >= 0)
//                                    {
//                                        if (semicol.Length >= 8)
//                                        {
//                                            if (semicol[1] == "Percent")
//                                            {
//                                                Double pers = 0;
//                                                Double basicallow = 0;
//                                                persallow = 0;
//                                                lop_amou = 0;
//                                                string allow = "";
//                                                Double gradeamnt = 0;
//                                                Double agpamnt = 0;
//                                                Double allbasamnt = 0;
//                                                bool enter = false;
//                                                if (semicol[2].Trim() != "")
//                                                {
//                                                    Double.TryParse(semicol[2], out pers);
//                                                    Double.TryParse(bas_salary, out allbasamnt);
//                                                    Double.TryParse(grade_pay, out gradeamnt);
//                                                    Double.TryParse(agp, out agpamnt);
//                                                    if (semicol[3] == "1")
//                                                    {
//                                                        enter = true;
//                                                        persallow = (pers / 100) * lopamnt;
//                                                        basicallow = (pers / 100) * lopamnt;
//                                                    }
//                                                    if (semicol[4] == "1")
//                                                    {
//                                                        if (enter == true)
//                                                        {
//                                                            persallow = persallow + ((pers / 100) * pres_month_totalsalry);
//                                                            basicallow = basicallow + ((pers / 100) * allbasamnt);
//                                                        }
//                                                        else
//                                                        {
//                                                            persallow = (pers / 100) * pres_month_totalsalry;
//                                                            basicallow = (pers / 100) * allbasamnt;
//                                                        }
//                                                        allow = Convert.ToString(semicol[0]);
//                                                        Persent = Persent + persallow;
//                                                        index = Convert.ToString(semicol[0]);
//                                                        DA = pres_month_totalsalry + persallow;
//                                                        if (semicol[0] == "DA")
//                                                        {
//                                                            daamnt = DA;
//                                                        }
//                                                        DABasic = allbasamnt + basicallow;
//                                                    }
//                                                    if (semicol[5] == "1")
//                                                    {
//                                                        if (enter == true)
//                                                        {
//                                                            persallow = persallow + ((pers / 100) * pres_month_totalsalry);
//                                                            basicallow = basicallow + ((pers / 100) * (allbasamnt + gradeamnt));
//                                                        }
//                                                        else
//                                                        {
//                                                            persallow = (pers / 100) * pres_month_totalsalry;
//                                                            basicallow = (pers / 100) * (allbasamnt + gradeamnt);
//                                                        }
//                                                    }
//                                                    if (semicol[7] == "1")
//                                                    {
//                                                        if (enter == true)
//                                                        {
//                                                            persallow = persallow + ((pers / 100) * pres_month_totalsalry);
//                                                            basicallow = basicallow + ((pers / 100) * (allbasamnt + agpamnt));
//                                                        }
//                                                        else
//                                                        {
//                                                            persallow = (pers / 100) * pres_month_totalsalry;
//                                                            basicallow = (pers / 100) * (allbasamnt + agpamnt);
//                                                        }
//                                                    }
//                                                }
//                                                if (format == "")
//                                                {
//                                                    format = allow + ";" + "Percent" + ";" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
//                                                }
//                                                else if (format != "")
//                                                {
//                                                    format = format + allow + ";" + "Percent" + ";" + Convert.ToString(persallow) + ";" + Convert.ToString(basicallow) + ";" + "\\";
//                                                }
//                                            }
//                                            else if (semicol[1] == "Amount")
//                                            {
//                                                if (semicol[0] != "Slab")
//                                                {
//                                                    string allow = "";
//                                                    allow = Convert.ToString(semicol[0]);
//                                                    Double bas_amount = 0;
//                                                    if (semicol[2].Trim() != "")
//                                                    {
//                                                        if (semicol[3].Trim() == "1")
//                                                        {
//                                                            Persent = Persent + Convert.ToDouble(semicol[2]);
//                                                            bas_amount = Convert.ToDouble(semicol[2]);
//                                                            lop_amou = Convert.ToDouble(semicol[2]) - lopamnt;
//                                                        }
//                                                        else
//                                                        {
//                                                            Persent = Persent + Convert.ToDouble(semicol[2]);
//                                                            bas_amount = Convert.ToDouble(semicol[2]);
//                                                            lop_amou = Convert.ToDouble(semicol[2]);
//                                                        }
//                                                    }
//                                                    if (semicol[0] == "DA")
//                                                    {
//                                                        daamnt = lop_amou;
//                                                    }
//                                                    string amnt = allow + ";" + "Amount" + ";" + Convert.ToString(lop_amou) + ";" + Convert.ToString(bas_amount) + ";" + "\\";
//                                                    if (format == "")
//                                                    {
//                                                        format = amnt;
//                                                    }
//                                                    else
//                                                    {
//                                                        format = format + amnt;
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    totallow = totallow + (lop_amou + persallow);
//                                }
//                            }
//                            string isconsolidate = ds.Tables[2].Rows[scd]["isconsolid"].ToString();
//                            if (isconsolidate.ToUpper() == "TRUE")
//                            {
//                                Gross_salary = pres_month_totalsalry + Persent;
//                            }
//                            else
//                            {
//                                Gross_salary = pres_month_totalsalry + Convert.ToDouble(cb_payband) + Persent;
//                            }
//                            Double grosswlop = 0;
//                            if (cb_lop_fromgross.Checked == true)
//                            {
//                                if (Gross_salary > lopamnt)
//                                {
//                                    Gross_salary = Gross_salary - lopamnt;
//                                    grosswlop = Gross_salary;
//                                }
//                            }
//                            else
//                            {
//                                grosswlop = Gross_salary;
//                            }
//                            Double dawithlop = 0;
//                            if (daamnt > lopamnt)
//                            {
//                                dawithlop = daamnt - lopamnt;
//                            }
//                            //////deductions
//                            string ded_format = "";
//                            string deduction = "";
//                            Double deductpersent = 0;
//                            Double gradamnt = 0;
//                            double basval = 0;
//                            Double basic_deductpersent = 0;
//                            Double fpfamnt = 0;
//                            Double totded = 0;
//                            double maxamnt = 0;
//                            double basamnt = 0;
//                            double fpfpercent = 0;
//                            double fpfround = 0;
//                            double pfamnt = 0;
//                            double esiamnt = 0;
//                            bool entrded = false;
//                            double.TryParse(txt_max_amount.Text, out maxamnt);
//                            double.TryParse(bas_salary, out basamnt);
//                            double.TryParse(txt_fpf.Text, out fpfpercent);
//                            deduction = ds.Tables[2].Rows[scd]["deductions"].ToString();
//                            string[] slash1_split = deduction.Split('\\');
//                            string[] splautoval = strautoded.Split(',');
//                            for (int i = 0; i < slash1_split.Length; i++)
//                            {
//                                basic_deductpersent = 0;
//                                Double frmtot = 0;
//                                Double deduct1 = 0;
//                                Double pers_ded1 = 0;
//                                string dedtion = "";
//                                Double persval = 0;
//                                string ded_spl = slash1_split[i].ToString();
//                                if (ded_spl != "" && ded_spl.Contains(';'))
//                                {
//                                    string[] semicol1 = ded_spl.Split(';');
//                                    string[] newsemicol1 = semicol1;
//                                    if (semicol1.Length >= 0)
//                                    {
//                                        dedtion = Convert.ToString(semicol1[0]);
//                                        if (!splautoval.Contains(dedtion))
//                                        {
//                                            if (semicol1[1] == "Percent")
//                                            {
//                                                if (semicol1.Length >= 11)
//                                                {
//                                                    if (semicol1[2].Trim() != "")
//                                                    {
//                                                        pers_ded1 = Convert.ToDouble(semicol1[2]);
//                                                        Double.TryParse(bas_salary, out basval);
//                                                        Double.TryParse(grade_pay, out gradamnt);
//                                                        if (semicol1[5] == "1")
//                                                        {
//                                                            entrded = true;
//                                                            frmtot = (pers_ded1 / 100) * lopamnt;
//                                                            deduct1 = (pers_ded1 / 100) * lopamnt;
//                                                        }
//                                                        if (semicol1[3] == "1")
//                                                        {
//                                                            if (entrded == true)
//                                                            {
//                                                                frmtot = frmtot + ((pers_ded1 / 100) * pres_month_totalsalry);
//                                                                deduct1 = deduct1 + ((pers_ded1 / 100) * Gross_salary);
//                                                                basic_deductpersent = basval - deduct1;
//                                                            }
//                                                            else
//                                                            {
//                                                                frmtot = (pers_ded1 / 100) * pres_month_totalsalry;
//                                                                deduct1 = (pers_ded1 / 100) * Gross_salary;
//                                                                basic_deductpersent = basval - deduct1;
//                                                                deductpersent = basic_deductpersent;
//                                                            }
//                                                        }
//                                                        if (semicol1[4] == "1")
//                                                        {
//                                                            for (int k = 0; k < newsemicol1.Length; k++)
//                                                            {
//                                                                if (newsemicol1[0] == "DA")
//                                                                {
//                                                                    if (newsemicol1[1] == "Percent")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + (persval / 100) * (pres_month_totalsalry);
//                                                                                deduct1 = deduct1 + ((persval / 100) * (basval));
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = (persval / 100) * (pres_month_totalsalry);
//                                                                                deduct1 = ((persval / 100) * (basval));
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                    else if (newsemicol1[1] == "Amount")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + persval;
//                                                                                deduct1 = deduct1 + persval + basval;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = persval;
//                                                                                deduct1 = persval + basval;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        if (semicol1[6] == "1")
//                                                        {
//                                                            for (int k = 0; k < newsemicol1.Length; k++)
//                                                            {
//                                                                if (newsemicol1[0] == "DA")
//                                                                {
//                                                                    if (newsemicol1[1] == "Percent")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = deduct1 + ((persval / 100) * (gradamnt));
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = (persval / 100) * (gradamnt);
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                    if (newsemicol1[1] == "Amount")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + persval;
//                                                                                deduct1 = deduct1 + persval + gradamnt;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = persval;
//                                                                                deduct1 = persval + gradamnt;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        if (semicol1[7] == "1")
//                                                        {
//                                                            if (entrded == true)
//                                                            {
//                                                                frmtot = frmtot + (pers_ded1 / 100) * pres_month_totalsalry;
//                                                                deduct1 = deduct1 + (pers_ded1 / 100) * basval;
//                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                            }
//                                                            else
//                                                            {
//                                                                frmtot = (pers_ded1 / 100) * pres_month_totalsalry;
//                                                                deduct1 = (pers_ded1 / 100) * (basval);
//                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                            }
//                                                        }
//                                                        if (semicol1[8] == "1")
//                                                        {
//                                                            for (int k = 0; k < newsemicol1.Length; k++)
//                                                            {
//                                                                if (newsemicol1[0] == "DP")
//                                                                {
//                                                                    if (newsemicol1[1] == "Percent")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = deduct1 + ((persval / 100) * (basval));
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = (pers_ded1 / 100) * pres_month_totalsalry;
//                                                                                deduct1 = (pers_ded1 / 100) * (basval);
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                    else if (newsemicol1[1] == "Amount")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + persval;
//                                                                                deduct1 = deduct1 + persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = persval;
//                                                                                deduct1 = persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        if (semicol1[9] == "1")
//                                                        {
//                                                            for (int k = 0; k < newsemicol1.Length; k++)
//                                                            {
//                                                                if (newsemicol1[0] == "Petty")
//                                                                {
//                                                                    if (newsemicol1[1] == "Percent")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = deduct1 + ((persval / 100) * basval);
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = ((persval / 100) * basval);
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                    else if (newsemicol1[1] == "Amount")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + persval;
//                                                                                deduct1 = deduct1 + persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = persval;
//                                                                                deduct1 = persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        if (semicol1[10] == "1")
//                                                        {
//                                                            for (int k = 0; k < newsemicol1.Length; k++)
//                                                            {
//                                                                if (newsemicol1[0] == "Arrear")
//                                                                {
//                                                                    if (newsemicol1[1] == "Percent")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = deduct1 + ((persval / 100) * (basval));
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = (persval / 100) * pres_month_totalsalry;
//                                                                                deduct1 = (persval / 100) * (basval);
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                    if (newsemicol1[1] == "Amount")
//                                                                    {
//                                                                        if (newsemicol1[2].Trim() != "")
//                                                                        {
//                                                                            Double.TryParse(newsemicol1[2], out persval);
//                                                                            if (entrded == true)
//                                                                            {
//                                                                                frmtot = frmtot + persval;
//                                                                                deduct1 = deduct1 + persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                            else
//                                                                            {
//                                                                                frmtot = persval;
//                                                                                deduct1 = persval + basval;
//                                                                                deductpersent = pres_month_totalsalry - frmtot;
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        //if (semicol1[4] == "1")
//                                                        //{
//                                                        //    deductpersent = Convert.ToDouble(bas_salary) + DA;
//                                                        //}
//                                                        //if (semicol1[11] == "1")
//                                                        //{
//                                                        //    Double zero = Math.Round(deductpersent, 0, MidpointRounding.AwayFromZero);
//                                                        //}
//                                                        if (semicol1.Length >= 15)
//                                                        {
//                                                            string[] splallamnt = semicol1[15].Split('+');
//                                                            if (splallamnt.Length > 0)
//                                                            {
//                                                                for (int newro = 0; newro < splallamnt.Length; newro++)
//                                                                {
//                                                                    if (splallamnt[newro] == "Basic" || splallamnt[newro] == "Grade Pay")
//                                                                    {
//                                                                        for (int myro = 0; myro < newsemicol1.Length; myro++)
//                                                                        {
//                                                                            if (newsemicol1[0] == splallamnt[newro])
//                                                                            {
//                                                                                if (newsemicol1[1] == "Percent")
//                                                                                {
//                                                                                    if (newsemicol1[2].Trim() != "")
//                                                                                    {
//                                                                                        double.TryParse(newsemicol1[2], out persval);
//                                                                                        if (entrded == true)
//                                                                                        {
//                                                                                            frmtot = frmtot + (persval / 100) * pres_month_totalsalry;
//                                                                                            if (splallamnt[newro] == "Basic")
//                                                                                            {
//                                                                                                deduct1 = deduct1 + ((persval / 100) * (basval));
//                                                                                            }
//                                                                                            if (splallamnt[newro] == "Grade Pay")
//                                                                                            {
//                                                                                                deduct1 = deduct1 + ((persval / 100) * (gradamnt));
//                                                                                            }
//                                                                                            deductpersent = pres_month_totalsalry - frmtot;
//                                                                                        }
//                                                                                        else
//                                                                                        {
//                                                                                            frmtot = (persval / 100) * pres_month_totalsalry;
//                                                                                            deduct1 = 0;
//                                                                                            if (splallamnt[newro] == "Basic")
//                                                                                            {
//                                                                                                deduct1 = (persval / 100) * (basval);
//                                                                                            }
//                                                                                            if (splallamnt[newro] == "Grade Pay")
//                                                                                            {
//                                                                                                deduct1 = (persval / 100) * (gradamnt);
//                                                                                            }
//                                                                                            deductpersent = pres_month_totalsalry - frmtot;
//                                                                                        }
//                                                                                    }
//                                                                                }
//                                                                                if (newsemicol1[1] == "Amount")
//                                                                                {
//                                                                                    if (newsemicol1[2].Trim() != "")
//                                                                                    {
//                                                                                        double.TryParse(newsemicol1[2], out persval);
//                                                                                        if (entrded == true)
//                                                                                        {
//                                                                                            frmtot = frmtot + persval;
//                                                                                            if (splallamnt[newro] == "Basic")
//                                                                                            {
//                                                                                                deduct1 = deduct1 + persval + basval;
//                                                                                            }
//                                                                                            if (splallamnt[newro] == "Grade Pay")
//                                                                                            {
//                                                                                                deduct1 = deduct1 + persval + gradamnt;
//                                                                                            }
//                                                                                            deductpersent = pres_month_totalsalry - frmtot;
//                                                                                        }
//                                                                                        else
//                                                                                        {
//                                                                                            frmtot = persval;
//                                                                                            deduct1 = 0;
//                                                                                            if (splallamnt[newro] == "Basic")
//                                                                                            {
//                                                                                                deduct1 = persval + basval;
//                                                                                            }
//                                                                                            if (splallamnt[newro] == "Grade Pay")
//                                                                                            {
//                                                                                                deduct1 = persval + gradamnt;
//                                                                                            }
//                                                                                            deductpersent = pres_month_totalsalry - frmtot;
//                                                                                        }
//                                                                                    }
//                                                                                }
//                                                                                if (ded_format.Trim() == "")
//                                                                                {
//                                                                                    ded_format = Convert.ToString(newsemicol1[0]) + ";" + "Amount" + ";" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
//                                                                                }
//                                                                                else
//                                                                                {
//                                                                                    ded_format = ded_format + Convert.ToString(newsemicol1[0]) + ";" + "Amount" + ";" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
//                                                                                }
//                                                                            }
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                            else if (semicol1[1] == "Amount")
//                                            {
//                                                if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                {
//                                                    deductpersent = pres_month_totalsalry - Convert.ToDouble(semicol1[2]);
//                                                    basic_deductpersent = basval - Convert.ToDouble(semicol1[2]);
//                                                    frmtot = Convert.ToDouble(semicol1[2]);
//                                                    deduct1 = basic_deductpersent;
//                                                }
//                                            }
//                                            else if (semicol1.Length >= 11)
//                                            {
//                                                if (semicol1[11] == "Amount")
//                                                {
//                                                    if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                    {
//                                                        deductpersent = pres_month_totalsalry - Convert.ToDouble(semicol1[2]);
//                                                        basic_deductpersent = basval - Convert.ToDouble(semicol1[2]);
//                                                        frmtot = Convert.ToDouble(semicol1[2]);
//                                                        deduct1 = basic_deductpersent;
//                                                    }
//                                                }
//                                            }
//                                            if (semicol1[7] == "1")
//                                            {
//                                                deductpersent = basval + DA + Convert.ToDouble(grade_pay);
//                                            }
//                                            if (dedtion.Trim() == "PF")
//                                            {
//                                                if (semicol1[1] == "Percent")
//                                                {
//                                                    if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                    {
//                                                        pers_ded1 = Convert.ToDouble(semicol1[2]);
//                                                        pfamnt = (pers_ded1 / 100) * pres_month_totalsalry;
//                                                        basic_deductpersent = (pers_ded1 / 100) * basval;
//                                                        frmtot = Convert.ToDouble(pfamnt);
//                                                        deduct1 = basic_deductpersent;
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                    {
//                                                        pfamnt = Convert.ToDouble(semicol1[2]);
//                                                        basic_deductpersent = basval - Convert.ToDouble(semicol1[2]);
//                                                        frmtot = Convert.ToDouble(pfamnt);
//                                                        deduct1 = basic_deductpersent;
//                                                    }
//                                                }
//                                                if (deductpersent != 0)
//                                                {
//                                                    if (chk_fpf.Checked == true)
//                                                    {
//                                                        if (txt_max_amount.Text.Trim() != "")
//                                                        {
//                                                            if (maxamnt >= basamnt)
//                                                            {
//                                                                if (txt_age_val.Text.Trim() != "")
//                                                                {
//                                                                    if (age <= Convert.ToInt32(txt_age_val.Text.Trim()))
//                                                                    {
//                                                                        fpfamnt = deductpersent * (fpfpercent / 100);
//                                                                        fpfround = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
//                                                                    }
//                                                                }
//                                                                else
//                                                                {
//                                                                    if (txt_max_amount.Text.Trim() != "")
//                                                                    {
//                                                                        if (maxamnt >= basamnt)
//                                                                        {
//                                                                            fpfamnt = deductpersent * (fpfpercent / 100);
//                                                                            fpfround = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
//                                                                        }
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        fpfamnt = deductpersent * (fpfpercent / 100);
//                                                                        fpfround = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        else
//                                                        {
//                                                            if (txt_age_val.Text.Trim() != "")
//                                                            {
//                                                                if (age <= Convert.ToInt32(txt_age_val.Text.Trim()))
//                                                                {
//                                                                    fpfamnt = deductpersent * (fpfpercent / 100);
//                                                                    fpfround = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
//                                                                }
//                                                            }
//                                                            else
//                                                            {
//                                                                fpfamnt = deductpersent * (fpfpercent / 100);
//                                                                fpfround = Math.Round(fpfamnt, 0, MidpointRounding.AwayFromZero);
//                                                            }
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                            if (dedtion.Trim() == "ESI")
//                                            {
//                                                if (semicol1[1] == "Percent")
//                                                {
//                                                    if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                    {
//                                                        pers_ded1 = Convert.ToDouble(semicol1[2]);
//                                                        esiamnt = (pers_ded1 / 100) * pres_month_totalsalry;
//                                                        basic_deductpersent = (pers_ded1 / 100) * basval;
//                                                        frmtot = esiamnt;
//                                                        deduct1 = basic_deductpersent;
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    if (Convert.ToString(semicol1[2]).Trim() != "")
//                                                    {
//                                                        esiamnt = Convert.ToDouble(semicol1[2]);
//                                                        basic_deductpersent = Convert.ToDouble(bas_salary) - Convert.ToDouble(semicol1[2]);
//                                                        frmtot = Convert.ToDouble(esiamnt);
//                                                        deduct1 = basic_deductpersent;
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    totded = totded + frmtot;
//                                }
//                                if (ded_format == "")
//                                {
//                                    ded_format = dedtion + ";" + "Amount" + ";" + Convert.ToString(frmtot) + ";" + Convert.ToString(deduct1) + ";" + "\\";
//                                }
//                                else
//                                {
//                                    ded_format = ded_format + dedtion + ";" + "Amount" + ";" + Convert.ToString(frmtot) + ";" + Convert.ToString(frmtot) + ";" + "\\";
//                                }
//                            }
//                            //loan deduct 
//                            Double deduct_loan = 0;
//                            string lon_cod = "";
//                            staf_cd = ds.Tables[2].Rows[scd]["staff_code"].ToString();
//                            ds2.Clear();
//                            string loan = "select IsDed,DedName,DedAmt,EMIAmt,LoanCode, DedFromMonth,DedFromYear,IntAmt from StaffLoanDet where Staff_Code = '" + staf_cd + "' and IsActive = 1 and IsClose = 0 and DedFromMonth <='" + month1 + "' and DedFromYear <='" + year + "'";
//                            ds2 = d2.select_method_wo_parameter(loan, "Text");
//                            if (ds2.Tables.Count > 0)
//                            {
//                                if (ds2.Tables[0].Rows.Count > 0)
//                                {
//                                    for (int ln = 0; ln < ds2.Tables[0].Rows.Count; ln++)
//                                    {
//                                        string ded_emi = ds2.Tables[0].Rows[ln]["EMIAmt"].ToString();
//                                        string dedname = ds2.Tables[0].Rows[ln]["DedName"].ToString();
//                                        deduct_loan = Convert.ToDouble(ded_emi);
//                                        deductpersent = deductpersent + deduct_loan;
//                                        ded_format = ded_format + dedname + ";" + "Amount" + ";" + ded_emi + ";" + ded_emi + ";" + "\\";
//                                        if (ded_emi != "" && ded_emi != "0")
//                                        {
//                                            lon_cod = ds2.Tables[0].Rows[ln]["LoanCode"].ToString();
//                                            //string lon_dedEntry = "Insert into StaffLoanPayDet (Staff_Code,EMIAmt,PayMonth, PayYear,LoanCode) values('" + staf_cd + "','" + deduct_loan + "','" + month1 + "','" + year + "','" + lon_cod + "') ";
//                                            //lon_dedEntry = lon_dedEntry + " UPDATE StaffLoanDet set IsClose = 1 where Staff_Code = '" + staf_cd + "' and LoanCode == '" + lon_cod + "' ";
//                                            //ds3.Clear();
//                                            //int insert = d2.update_method_wo_parameter(lon_dedEntry, "Text");
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    //string lon_dedEntry = "UPDATE StaffLoanDet set IsClose = 1 where Staff_Code = '" + staf_cd + "' and LoanCode = 11 ";
//                                    //ds3.Clear();
//                                    //int insert = d2.update_method_wo_parameter(lon_dedEntry, "Text");
//                                }
//                            }
//                            if (cb_auto_deduct.Checked == true)
//                            {
//                                string get_slab = "";
//                                //string slab = "";
//                                if (strautoded.Trim() != "")
//                                {
//                                    string[] splauto = strautoded.Split(',');
//                                    if (splauto.Length > 0)
//                                    {
//                                        for (int ik = 0; ik < splauto.Length; ik++)
//                                        {
//                                            get_slab = "select ESI_EmpSlabType,ESI_EmpSlabValue from pfslabs where SlabFor = '" + Convert.ToString(splauto[ik]) + "' and '" + Gross_salary + "' between salfrom and salto and college_code='" + collegecode1 + "'";
//                                            ds1 = d2.select_method_wo_parameter(get_slab, "Text");
//                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
//                                            {
//                                                if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) != "")
//                                                {
//                                                    if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabType"]) == "Percent")
//                                                    {
//                                                        if (Convert.ToString(splauto[ik]) == "ESI")
//                                                        {
//                                                            esisal = Gross_salary * (Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) / 100);
//                                                            slabsal = esisal;
//                                                        }
//                                                        else if (Convert.ToString(splauto[ik]) == "PF")
//                                                        {
//                                                            pfsal = Gross_salary * (Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) / 100);
//                                                            slabsal = pfsal;
//                                                        }
//                                                        else
//                                                        {
//                                                            slabsal = Gross_salary * (Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]) / 100);
//                                                        }
//                                                    }
//                                                    if (Convert.ToString(ds1.Tables[0].Rows[0]["ESI_EmpSlabType"]) == "Amount")
//                                                    {
//                                                        if (Convert.ToString(splauto[ik]) == "ESI")
//                                                        {
//                                                            esisal = Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]);
//                                                            slabsal = esisal;
//                                                        }
//                                                        else if (Convert.ToString(splauto[ik]) == "PF")
//                                                        {
//                                                            pfsal = Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]);
//                                                            slabsal = pfsal;
//                                                        }
//                                                        else
//                                                        {
//                                                            slabsal = Convert.ToDouble(ds1.Tables[0].Rows[0]["ESI_EmpSlabValue"]);
//                                                        }
//                                                    }
//                                                }
//                                                totded = totded + slabsal;
//                                                //slab = Convert.ToString(splauto[ik]) + ";" + "Amount" + ";" + Convert.ToString(slabsal) + ";" + "\\";
//                                                //if (format == "")
//                                                //{
//                                                //    format = slab;
//                                                //}
//                                                //else
//                                                //{
//                                                //    format = format + slab;
//                                                //}
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            netsalary = Gross_salary - totded;
//                            //string insertquery = "if exists(select * from monthlypay where staff_code='" + staf_cd + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and latestrec='1' and college_code='" + collegecode1 + "') update monthlypay set dept_name='" + deptname + "',desig_name='" + designame + "',fdate='" + newdt.ToString("MM/dd/yyyy") + "',stftype='" + stftype + "',tdate='" + newdt1.ToString("MM/dd/yyyy") + "',adate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',bsalary='" + pres_month_totalsalry + "',pf='" + pfamnt + "',lop='" + lopamnt + "',leavedetail='" + leavedetail + "',allowances='" + format + "',deductions='" + ded_format + "',addd='" + totallow + "',netadd='" + grosswlop + "',deddd='" + totded + "',netded='" + totded + "',netsal='" + netsalary + "',paybybank='1',category_code='" + catcode + "',basic_alone='" + basicwolop + "',pay_band='" + Convert.ToDouble(cb_payband) + "',grade_pay='" + Convert.ToDouble(grade_pay) + "',Cur_Lop='" + cur_lop + "',Pre_Lop='" + pre_lop + "',Actual_Basic='" + Convert.ToDouble(bas_salary) + "',DAAmt='" + daamnt + "',Tot_LOP='" + lopamnt + "',ESI_Salary='" + esisal + "',PF_Salary='" + pfsal + "',G_Pay='" + gradepaywlop + "',NetAddAct='" + Gross_salary + "',ESI='" + esiamnt + "',DAWithLOP='" + dawithlop + "',FPF='" + fpfamnt + "',PayMonth='" + month1 + "',PayYear='" + year1 + "' where staff_code='" + staf_cd + "' and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and latestrec='1' and college_code='" + collegecode1 + "' else Insert into monthlypay (staff_code,dept_name,desig_name,fdate,tdate,adate,bsalary,pf,lop,leavedetail,allowances,deductions,addd,netadd,deddd,netded,netsal,paybybank,college_code,category_code,basic_alone,pay_band,grade_pay,Cur_Lop,Pre_Lop,Actual_Basic,DAAmt,Tot_LOP,ESI_Salary,PF_Salary,G_Pay,NetAddAct,ESI,DAWithLOP,FPF,PayMonth,PayYear,latestrec,stftype) Values ('" + staf_cd + "','" + deptname + "','" + designame + "','" + newdt.ToString("MM/dd/yyyy") + "','" + newdt1.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + pres_month_totalsalry + "','" + pfamnt + "','" + lopamnt + "','" + leavedetail + "','" + format + "','" + ded_format + "','" + totallow + "','" + grosswlop + "','" + totded + "','" + totded + "','" + netsalary + "','1','" + collegecode1 + "','" + catcode + "','" + basicwolop + "','" + Convert.ToDouble(cb_payband) + "','" + Convert.ToDouble(grade_pay) + "','" + cur_lop + "','" + pre_lop + "','" + Convert.ToDouble(bas_salary) + "','" + daamnt + "','" + lopamnt + "','" + esisal + "','" + pfsal + "','" + gradepaywlop + "','" + Gross_salary + "','" + esiamnt + "','" + dawithlop + "','" + fpfamnt + "','" + month1 + "','" + year1 + "','1','" + stftype + "')";
//                            //inscount = d2.update_method_wo_parameter(insertquery, "Text");
//                            savecount++;
//                            if (cbincitcalc.Checked == true)
//                            {
//                                DataSet dsgetdate = new DataSet();
//                                DataSet dsnew = new DataSet();
//                                DataSet dsall = new DataSet();
//                                DataSet dsded = new DataSet();
//                                DataSet dsitded = new DataSet();
//                                string[] split = new string[5];
//                                string[] amntspl = new string[5];
//                                string[] amntcon = new string[2];
//                                string frmmonyear = "";
//                                string tomonyear = "";
//                                string frmmon = "";
//                                string frmyear = "";
//                                string tomon = "";
//                                string toyear = "";
//                                string paystdate = "";
//                                string payenddt = "";
//                                string basicsalary = "";
//                                string netamnt = "";
//                                double basic = 0;
//                                double netsal = 0;
//                                string amntfrm = "";
//                                string amntto = "";
//                                string amountorper = "";
//                                string mode = "";
//                                double frmamnt = 0;
//                                double toamnt = 0;
//                                double amnt = 0;
//                                double taxamnt = 0;
//                                double splnetsal = 0;
//                                string selitset = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IT Calculation Settings' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
//                                if (selitset.Trim() != "0")
//                                {
//                                    frmmonyear = selitset.Split('-')[0];
//                                    tomonyear = selitset.Split('-')[1];
//                                    frmmon = frmmonyear.Split(',')[0];
//                                    frmyear = frmmonyear.Split(',')[1];
//                                    tomon = tomonyear.Split(',')[0];
//                                    toyear = tomonyear.Split(',')[1];
//                                    DateTime dtstrday = new DateTime();
//                                    DateTime dtendday = new DateTime();
//                                    if (Convert.ToInt32(frmmon) < Convert.ToInt32(tomon) && Convert.ToInt32(frmyear) == Convert.ToInt32(toyear))
//                                    {
//                                        for (int stmon = Convert.ToInt32(frmmon); stmon <= Convert.ToInt32(tomon); stmon++)
//                                        {
//                                            string getdate = "select Convert(varchar(10),From_Date,103) as frmdate,Convert(varchar(10),To_Date,103) as todate from HrPayMonths where PayMonthNum='" + stmon + "' and PayYear='" + frmyear + "' and College_Code='" + collegecode1 + "'";
//                                            dsgetdate.Clear();
//                                            dsgetdate = d2.select_method_wo_parameter(getdate, "Text");
//                                            if (dsgetdate.Tables.Count > 0)
//                                            {
//                                                if (dsgetdate.Tables[0].Rows.Count > 0)
//                                                {
//                                                    paystdate = Convert.ToString(dsgetdate.Tables[0].Rows[0]["frmdate"]);
//                                                    split = paystdate.Split('/');
//                                                    dtstrday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    payenddt = Convert.ToString(dsgetdate.Tables[0].Rows[0]["todate"]);
//                                                    split = payenddt.Split('/');
//                                                    dtendday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    string getsal = "select basic_alone,netsal from monthlypay where staff_code='" + staf_cd + "' and fdate='" + dtstrday + "' and tdate='" + dtendday + "' and college_code='" + collegecode1 + "'";
//                                                    dsnew.Clear();
//                                                    dsnew = d2.select_method_wo_parameter(getsal, "Text");
//                                                    if (dsnew.Tables.Count > 0)
//                                                    {
//                                                        if (dsnew.Tables[0].Rows.Count > 0)
//                                                        {
//                                                            basicsalary = Convert.ToString(dsnew.Tables[0].Rows[0]["basic_alone"]);
//                                                            netamnt = Convert.ToString(dsnew.Tables[0].Rows[0]["netsal"]);
//                                                            netsal = netsal + Convert.ToDouble(netsalary);
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    else if ((Convert.ToInt32(frmmon) < Convert.ToInt32(tomon) || Convert.ToInt32(frmmon) > Convert.ToInt32(tomon)) && Convert.ToInt32(frmyear) < Convert.ToInt32(toyear))
//                                    {
//                                        for (int stmon = Convert.ToInt32(frmmon); stmon <= 12; stmon++)
//                                        {
//                                            string getdate = "select Convert(varchar(10),From_Date,103) as frmdate,Convert(varchar(10),To_Date,103) as todate from HrPayMonths where PayMonthNum='" + stmon + "' and PayYear='" + frmyear + "' and College_Code='" + collegecode1 + "'";
//                                            dsgetdate.Clear();
//                                            dsgetdate = d2.select_method_wo_parameter(getdate, "Text");
//                                            if (dsgetdate.Tables.Count > 0)
//                                            {
//                                                if (dsgetdate.Tables[0].Rows.Count > 0)
//                                                {
//                                                    paystdate = Convert.ToString(dsgetdate.Tables[0].Rows[0]["frmdate"]);
//                                                    split = paystdate.Split('/');
//                                                    dtstrday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    payenddt = Convert.ToString(dsgetdate.Tables[0].Rows[0]["todate"]);
//                                                    split = payenddt.Split('/');
//                                                    dtendday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    string getsal = "select basic_alone,netsal from monthlypay where staff_code='" + staf_cd + "' and fdate='" + dtstrday + "' and tdate='" + dtendday + "' and college_code='" + collegecode1 + "'";
//                                                    dsnew.Clear();
//                                                    dsnew = d2.select_method_wo_parameter(getsal, "Text");
//                                                    if (dsnew.Tables.Count > 0)
//                                                    {
//                                                        if (dsnew.Tables[0].Rows.Count > 0)
//                                                        {
//                                                            basicsalary = Convert.ToString(dsnew.Tables[0].Rows[0]["basic_alone"]);
//                                                            netamnt = Convert.ToString(dsnew.Tables[0].Rows[0]["netsal"]);
//                                                            netsal = netsal + Convert.ToDouble(netsalary);
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                        for (int newst = 1; newst <= Convert.ToInt32(tomon); newst++)
//                                        {
//                                            string getdate = "select Convert(varchar(10),From_Date,103) as frmdate,Convert(varchar(10),To_Date,103) as todate from HrPayMonths where PayMonthNum='" + newst + "' and PayYear='" + toyear + "' and College_Code='" + collegecode1 + "'";
//                                            dsgetdate.Clear();
//                                            dsgetdate = d2.select_method_wo_parameter(getdate, "Text");
//                                            if (dsgetdate.Tables.Count > 0)
//                                            {
//                                                if (dsgetdate.Tables[0].Rows.Count > 0)
//                                                {
//                                                    paystdate = Convert.ToString(dsgetdate.Tables[0].Rows[0]["frmdate"]);
//                                                    split = paystdate.Split('/');
//                                                    dtstrday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    payenddt = Convert.ToString(dsgetdate.Tables[0].Rows[0]["todate"]);
//                                                    split = payenddt.Split('/');
//                                                    dtendday = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
//                                                    string getsal = "select basic_alone,netsal from monthlypay where staff_code='" + staf_cd + "' and fdate='" + dtstrday + "' and tdate='" + dtendday + "' and college_code='" + collegecode1 + "'";
//                                                    dsnew.Clear();
//                                                    dsnew = d2.select_method_wo_parameter(getsal, "Text");
//                                                    if (dsnew.Tables.Count > 0)
//                                                    {
//                                                        if (dsnew.Tables[0].Rows.Count > 0)
//                                                        {
//                                                            basicsalary = Convert.ToString(dsnew.Tables[0].Rows[0]["basic_alone"]);
//                                                            netamnt = Convert.ToString(dsnew.Tables[0].Rows[0]["netsal"]);
//                                                            netsal = netsal + Convert.ToDouble(netsalary);
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    string selall = "select AllowDedAmount from ITAddAllowDedDetails where Staff_Code='" + staf_cd + "' and IsAllow='1'";
//                                    dsall.Clear();
//                                    dsall = d2.select_method_wo_parameter(selall, "Text");
//                                    if (dsall.Tables.Count > 0)
//                                    {
//                                        if (dsall.Tables[0].Rows.Count > 0)
//                                        {
//                                            for (int ro = 0; ro < dsall.Tables[0].Rows.Count; ro++)
//                                            {
//                                                netsal = netsal + Convert.ToDouble(dsall.Tables[0].Rows[ro]["AllowDedAmount"]);
//                                            }
//                                        }
//                                    }
//                                    string selded = "select AllowDedAmount from ITAddAllowDedDetails where Staff_Code='" + staf_cd + "' and IsAllow='0'";
//                                    dsded.Clear();
//                                    dsded = d2.select_method_wo_parameter(selded, "Text");
//                                    if (dsded.Tables.Count > 0)
//                                    {
//                                        if (dsded.Tables[0].Rows.Count > 0)
//                                        {
//                                            for (int ro = 0; ro < dsded.Tables[0].Rows.Count; ro++)
//                                            {
//                                                netsal = netsal - Convert.ToDouble(dsded.Tables[0].Rows[ro]["AllowDedAmount"]);
//                                            }
//                                        }
//                                    }
//                                    string selitded = "select * from HR_ITCalculationSettings where collegeCode='" + collegecode1 + "' order by FromRange";
//                                    dsitded.Clear();
//                                    dsitded = d2.select_method_wo_parameter(selitded, "Text");
//                                    if (dsitded.Tables.Count > 0)
//                                    {
//                                        if (dsitded.Tables[0].Rows.Count > 0)
//                                        {
//                                            splnetsal = netsal;
//                                            bool entryflag = false;
//                                            for (int ik = 0; ik < dsitded.Tables[0].Rows.Count; ik++)
//                                            {
//                                                amntfrm = Convert.ToString(dsitded.Tables[0].Rows[ik]["FromRange"]);
//                                                amntto = Convert.ToString(dsitded.Tables[0].Rows[ik]["ToRange"]);
//                                                mode = Convert.ToString(dsitded.Tables[0].Rows[ik]["Mode"]);
//                                                amountorper = Convert.ToString(dsitded.Tables[0].Rows[ik]["Amount"]);
//                                                Double.TryParse(amntfrm, out frmamnt);
//                                                Double.TryParse(amntto, out toamnt);
//                                                Double.TryParse(amountorper, out amnt);
//                                                if (frmamnt < splnetsal)
//                                                {
//                                                    if (mode == "False")
//                                                    {
//                                                        taxamnt = taxamnt + amnt;
//                                                        if (entryflag == false)
//                                                        {
//                                                            splnetsal = splnetsal - toamnt;
//                                                            entryflag = true;
//                                                        }
//                                                        else
//                                                        {
//                                                            splnetsal = splnetsal - frmamnt;
//                                                        }
//                                                    }
//                                                    else
//                                                    {
//                                                        if (entryflag == false)
//                                                        {
//                                                            taxamnt = taxamnt + ((toamnt * amnt) / 100);
//                                                            splnetsal = splnetsal - toamnt;
//                                                            entryflag = true;
//                                                        }
//                                                        else
//                                                        {
//                                                            taxamnt = taxamnt + ((frmamnt * amnt) / 100);
//                                                            splnetsal = splnetsal - frmamnt;
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    //string insquery = " if exists(select * from StaffTaxDetails where Staff_Code='" + staf_cd + "' and Asst_Year='" + selitset + "') Update StaffTaxDetails set TaxAmount='" + taxamnt + "' where Staff_Code='" + staf_cd + "' and Asst_Year='" + selitset + "' else Insert into StaffTaxDetails (Staff_Code,Asst_Year,TaxAmount) Values ('" + staf_cd + "','" + selitset + "','" + taxamnt + "')";
//                                    //int insupcount = d2.update_method_wo_parameter(insquery, "Text");
//                                    insitcount++;
//                                }
//                            }
//                        }
//                    }
//                }
//                if (savecount > 0 || insitcount > 0)
//                {
//                    genchk = true;
//                }
//                if (savecount == 0)
//                {
//                    genchk = false;
//                    alertpopwindow.Visible = true;
//                    lblalerterr.Text = "Please Update the Attendance Details for Staff!";
//                    return;
//                }
//            }
//            else
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Text = "No Staff Found!";
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        d2.sendErrorMail(ex, collegecode1, "Pay_Process.aspx");
//    }
//}
#endregion