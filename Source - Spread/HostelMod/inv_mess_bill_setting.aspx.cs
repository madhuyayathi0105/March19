using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class inv_mess_bill_setting : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string dtfromdate = string.Empty;
    string dt1todate = string.Empty;
    string m = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds7 = new DataSet();
    DAccess2 d2 = new DAccess2();
    bool check = false;
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    ReuasableMethods rs = new ReuasableMethods();
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
    bool hostel_bool = false;
    bool guest_bool = false;
    //int days = 0;
    Hashtable grantday_hash = new Hashtable();
    Hashtable staffrebateamount_hash = new Hashtable();
    Hashtable staffrebateday_hash = new Hashtable();
    Hashtable guestgrant_hash = new Hashtable();
    Hashtable Rebateamount_hash = new Hashtable();
    Hashtable guestRebateamount_hash = new Hashtable();
    Hashtable totcount = new Hashtable();
    int VegTotalNoofStudentstrength = 0;
    int NonvegTotalNoofStudentstrength = 0;
    int TotalNoofStudentstrength = 0;
    Hashtable expanses_hash = new Hashtable();
    string groupUsercode = string.Empty;
    #region magesh 23.3.18
    Hashtable typhsstaf = new Hashtable();
    Hashtable typamo = new Hashtable();
    Hashtable typhsgue = new Hashtable();
    Hashtable typhsstu = new Hashtable();
    Hashtable typdsstaf = new Hashtable();
    Hashtable typdsstu = new Hashtable();
    Hashtable hatndndiv = new Hashtable();
    Hashtable hos = new Hashtable();
    Hashtable htrebam = new Hashtable();
    Hashtable htrebday = new Hashtable();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (string.IsNullOrEmpty(group_user))
        {
            groupUsercode = " and group_code='" + group_user + "'";
        }
        else
        {
            groupUsercode = " and usercode='" + usercode + "'";
        }
        rdb_indivual1.Checked = true;
        if (!IsPostBack)
        {
            int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            for (int l = 0; l < 15; l++)
            {
                ddl_fromyr.Items.Add(Convert.ToString(year));
                ddl_toyr.Items.Add(Convert.ToString(year));
                year--;
            }
            ddl_fromyr.Items.Insert(0, "Select");
            ddl_toyr.Items.Insert(0, "Select");
            //magesh 12.3.18
            int divyear = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            for (int l = 0; l < 15; l++)
            {
                ddlyear.Items.Add(Convert.ToString(divyear));
                divyear--;
            }
            ddlyear.Items.Insert(0, "Select");
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            BindStudentType();
            txt_messfess.Text = "";
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            ViewState["isdivident"] = null;
            //rdb_common1.Checked = true;
            rdb_indivual1.Checked = true;
            bindhostelname();
            bindcollege();
            rdb_common.Checked = true;
            rdo_day.Checked = true;
            btn_go_Click(sender, e);
            lb_datesetting_Click(sender, e);
            //magesh 12.3.18
            lb_billsetting_Click(sender, e);
            rdb_fix.Visible = true;
            poperrjs.Visible = false;
            Divdivding.Visible = false;
            common();
            Session["year"] = null;
            Session["reb_amt"] = null;
            Session["reb_days"] = null;
            Session["grantamt"] = null;
            Session["guestrebateamt"] = null;
            Session["guestrebateday"] = null;
            Session["guestgrantamout"] = null;
            Session["guestrebateday"] = null;
            Session["reb_days"] = null;
            ViewState["Regenerate"] = null;
            Session["noofstudent"] = null;
            Session["nonveg"] = null;
            Session["incgroup"] = null;
            Session["finalvalue"] = null;
            bindaddgroup();
            bindhostel();
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose_dateset_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    //magesh 12.3.18
    protected void Imagbutn_dateset_Click(object sender, EventArgs e)
    {
        Divdivding.Visible = false;
    }
    protected void btn_exit_dateset1_Click(object sender, EventArgs e)
    {
        Divdivding.Visible = false;
    }//magesh 12.3.18
    protected void btn_exit_dateset_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            #region Dividend
            if (rdb_div.Checked == true)
            {
                if (rdb_common1.Checked == true)
                {
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
                            Fpspread1.Sheets[0].ColumnCount = 4;
                            Fpspread1.Width = 470;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
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
                            FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                            FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                            btnType.CssClass = "textbox btn4";
                            btnType1.CssClass = "textbox btn4";
                            int row = 1;
                            while (yr1 <= yr2)
                            {
                                for (int i = 1; i <= 12; i++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(i);
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
                                    //string backcolor = "select * from  MessBill_Master where BillMonth='" + i + "' and Bill_Year='" + yr1 + "'";
                                    string backcolor = "select*from HT_MessBillMaster where messmonth='" + i + "' and messyear='" + yr1 + "'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(backcolor, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        btnType1.Text = "Regenerate";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Red;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Red;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType1;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = btnType1.Text;
                                        btnType1.ForeColor = Color.Red;
                                        ViewState["Regenerate"] = "Regenerate";
                                    }
                                    else
                                    {
                                        btnType.Text = "Generate";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = btnType.Text;
                                        ViewState["Regenerate"] = "Generate";
                                    }
                                }
                                yr1 = yr1 + 1;
                            }
                            //Fpspread1.SaveChanges();
                            Fpspread1.Visible = true;
                            div1.Visible = true;
                            lbl_error.Visible = false;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        }
                        else
                        {
                            Div2.Visible = false;
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select To year Greater then From year";
                        }
                    }
                    else
                    {
                        Div2.Visible = false;
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select From year and To year";
                    }
                }
                else
                {
                    Div2.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lbl_error.Visible = false;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                            Fpspread1.Width = 618;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
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
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Mess Name";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[3].Width = 150;
                            Fpspread1.Columns[3].Locked = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Generate";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[4].Width = 200;
                            FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                            FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                            int row = 1;
                            btnType.CssClass = "textbox btn4";
                            btnType1.CssClass = "textbox btn4";
                            while (yr1 <= yr2)
                            {
                                for (int i = 1; i <= 12; i++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    //dnew = d2.Bindmess_inv(collegecode1);
                                    dnew = d2.Bindmess_basedonrights(usercode, collegecode1);
                                    if (dnew.Tables[0].Rows.Count > 0)
                                    {
                                        for (int rcount = 0; rcount < dnew.Tables[0].Rows.Count; rcount++)
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(yr1);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(i);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(yr1);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = returnMonYear(Convert.ToString(i));
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(i);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dnew.Tables[0].Rows[rcount]["MessName"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dnew.Tables[0].Rows[rcount]["MessMasterPK"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            string Hoscode = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag);
                                            //hoscode = Gethostelcodenew(Hoscode);
                                            string backcolor = "select*from HT_MessBillMaster where messmonth='" + i + "' and messyear='" + yr1 + "' and MessMasterFK='" + Hoscode + "'";
                                            ds1.Clear();
                                            ds1 = d2.select_method_wo_parameter(backcolor, "Text");
                                            if (ds1.Tables[0].Rows.Count > 0)
                                            {
                                                btnType1.Text = "Regenerate";
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType1;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = btnType1.Text;
                                                btnType1.ForeColor = Color.Red;
                                                ViewState["Regenerate"] = "Regenerate";
                                            }
                                            else
                                            {
                                                btnType.Text = "Generate";
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = btnType.Text;
                                                ViewState["Regenerate"] = "Generate";
                                            }
                                            Fpspread1.Sheets[0].RowCount++;
                                        }
                                    }
                                    else
                                    {
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Add Mess Details";
                                    }
                                }
                                yr1 = yr1 + 1;
                            }
                            //Fpspread1.SaveChanges();
                            Fpspread1.Visible = true;
                            div1.Visible = true;
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
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select to year greater then from year";
                        }
                    }
                    else
                    {
                        Div2.Visible = false;
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please select from year and to year";
                    }
                }
            }
            #endregion
            #region Non-Dividend
            if (rdb_nondiv.Checked == true)
            {
                #region comom
                if (rdb_common1.Checked == true)
                {
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
                            Fpspread1.Sheets[0].ColumnCount = 4;
                            Fpspread1.Width = 470;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
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
                            FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                            FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                            btnType.CssClass = "textbox btn4";
                            btnType1.CssClass = "textbox btn4";
                            int row = 1;
                            while (yr1 <= yr2)
                            {
                                for (int i = 1; i <= 12; i++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row++);
                                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(i);
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
                                    //string backcolor = "select * from  MessBill_Master where BillMonth='" + i + "' and Bill_Year='" + yr1 + "'";
                                    string backcolor = "select*from HT_MessBillMaster where messmonth='" + i + "' and messyear='" + yr1 + "'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(backcolor, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        btnType1.Text = "Regenerate";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Red;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Red;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType1;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = btnType1.Text;
                                        btnType1.ForeColor = Color.Red;
                                        ViewState["Regenerate"] = "Regenerate";
                                    }
                                    else
                                    {
                                        btnType.Text = "Generate";
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnType;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = btnType.Text;
                                        ViewState["Regenerate"] = "Generate";
                                    }
                                }
                                yr1 = yr1 + 1;
                            }
                            //Fpspread1.SaveChanges();
                            Fpspread1.Visible = true;
                            div1.Visible = true;
                            lbl_error.Visible = false;
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        }
                        else
                        {
                            Div2.Visible = false;
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select To year Greater then From year";
                        }
                    }
                    else
                    {
                        Div2.Visible = false;
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select From year and To year";
                    }
                }
                #endregion

                else
                {
                    Div2.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lbl_error.Visible = false;
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
                            Fpspread1.Width = 618;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
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
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostel Name";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[3].Width = 150;
                            Fpspread1.Columns[3].Locked = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Generate";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[4].Width = 200;
                            FarPoint.Web.Spread.ButtonCellType btnType = new FarPoint.Web.Spread.ButtonCellType();
                            FarPoint.Web.Spread.ButtonCellType btnType1 = new FarPoint.Web.Spread.ButtonCellType();
                            int row = 1;
                            btnType.CssClass = "textbox btn4";
                            btnType1.CssClass = "textbox btn4";
                            while (yr1 <= yr2)
                            {
                                for (int i = 1; i <= 12; i++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    //dnew = d2.Bindmess_inv(collegecode1);
                                    // dnew = d2.Bindmess_basedonrights(usercode, collegecode1);
                                    string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
                                    dnew = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
                                    if (dnew.Tables[0].Rows.Count > 0)
                                    {
                                        for (int rcount = 0; rcount < dnew.Tables[0].Rows.Count; rcount++)
                                        {
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
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dnew.Tables[0].Rows[rcount]["HostelName"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dnew.Tables[0].Rows[rcount]["HostelMasterPK"]);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            string Hoscode = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag);
                                            //hoscode = Gethostelcodenew(Hoscode);
                                            string backcolor = "select*from HT_MessBillMaster where messmonth='" + i + "' and messyear='" + yr1 + "' and Hostel_code='" + Hoscode + "'";
                                            ds1.Clear();
                                            ds1 = d2.select_method_wo_parameter(backcolor, "Text");
                                            if (ds1.Tables[0].Rows.Count > 0)
                                            {
                                                btnType1.Text = "Regenerate";
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Red;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType1;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = btnType1.Text;
                                                btnType1.ForeColor = Color.Red;
                                                ViewState["Regenerate"] = "Regenerate";
                                            }
                                            else
                                            {
                                                btnType.Text = "Generate";
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = btnType;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = btnType.Text;
                                                ViewState["Regenerate"] = "Generate";
                                            }
                                            Fpspread1.Sheets[0].RowCount++;
                                        }
                                    }
                                    else
                                    {
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Add Mess Details";
                                    }
                                }
                                yr1 = yr1 + 1;
                            }
                            //Fpspread1.SaveChanges();
                            Fpspread1.Visible = true;
                            div1.Visible = true;
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
                            div1.Visible = false;
                            Fpspread1.Visible = false;
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please select to year greater then from year";
                        }
                    }
                    else
                    {
                        Div2.Visible = false;
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please select from year and to year";
                    }
                }
            }
            #endregion
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose_addnew_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_hostelname.Text = "--Select--";
        if (cb_hostelname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Mess Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
        }
        common();
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname.Checked = false;
            int commcount = 0;
            txt_hostelname.Text = "--Select--";
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";
            }
            common();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            //ds = d2.Bindmess_inv(collegecode1);
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "MessName";
                cbl_hostelname.DataValueField = "MessMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Mess Name(" + cbl_hostelname.Items.Count + ")";
                    cb_hostelname.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            cbl_clgname.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clgname.DataSource = ds;
                cbl_clgname.DataTextField = "collname";
                cbl_clgname.DataValueField = "college_code";
                cbl_clgname.DataBind();
                if (cbl_clgname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_clgname.Items.Count; i++)
                    {
                        cbl_clgname.Items[i].Selected = true;
                    }
                    txt_clgname.Text = "College Name(" + cbl_clgname.Items.Count + ")";
                    cb_clgname.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_clgname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_clgname.Text = "--Select--";
        if (cb_clgname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_clgname.Items.Count; i++)
            {
                cbl_clgname.Items[i].Selected = true;
            }
            txt_clgname.Text = "College Name(" + (cbl_clgname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_clgname.Items.Count; i++)
            {
                cbl_clgname.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_clgname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_clgname.Checked = false;
            int commcount = 0;
            txt_clgname.Text = "--Select--";
            for (i = 0; i < cbl_clgname.Items.Count; i++)
            {
                if (cbl_clgname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_clgname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_clgname.Items.Count)
                {
                    cb_clgname.Checked = true;
                }
                txt_clgname.Text = "College Name(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_addnew_exit_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
        checkboxfalse();
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
        div1.Visible = false;
        Fpspread1.Visible = false;
    }
    protected void rdb_rdb_indivual1_CheckedChange(object sender, EventArgs e)
    {
        div1.Visible = false;
        Fpspread1.Visible = false;
    }
    public void btnType_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string year = "";
            string month = "";
            string todate = "";
            string txtdate = "";
            string txtmonth = "";
            string txtyear = "";
            string txttodate = "";
            string txttomonth = "";
            string txttoyear = "";
            int mon2 = 0;
            int dbmonth2 = 0;
            int finalmonth = 0;
            int finalyear = 0;
            int txtfyear2 = 0;
            string dbtodate = "";
            string dbfromdate = "";
            Fpspread1.SaveChanges();
            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            if (actrow.Trim() != "" && actcol.Trim() != "")
            {
                year = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                month = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string buttontext = "";
                if (rdb_common1.Checked == true)
                {
                    buttontext = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                }
                else
                {
                    buttontext = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Tag);
                }
                if (buttontext == "Regenerate")
                {
                    ViewState["Regenerate"] = "Regenerate";
                }
                else
                {
                    ViewState["Regenerate"] = "Generate";
                }
                lbl_year.Text = year;
                lbl_mon.Text = month;
                monthvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                Session["monthvalue"] = Convert.ToInt32(monthvalue);
                Session["year"] = year;
                string fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                char[] delimiterChars = { '/' };
                string[] lnkvalue = fromdate.Split(delimiterChars);
                string[] todate1 = todate.Split(delimiterChars);
                txtdate = Convert.ToString(lnkvalue[0]);
                txtmonth = Convert.ToString(lnkvalue[1]);
                txtyear = Convert.ToString(lnkvalue[2]);
                txtfyear2 = Convert.ToInt32(txtyear);
                txttodate = Convert.ToString(todate1[0]);
                txttomonth = Convert.ToString(todate1[1]);
                txttoyear = Convert.ToString(todate1[2]);
                int month1 = Convert.ToInt32(txttomonth);
                int mon1 = Convert.ToInt32(txtmonth);
                mon2 = month1 - mon1;
                //from month
                if (rdb_frommonth.Checked == true)
                {
                    dbmonth2 = Convert.ToInt32(monthvalue) + mon2;
                    finalmonth = dbmonth2;
                }
                else if (rdb_tomonth.Checked == true)// to Month
                {
                    dbmonth2 = Convert.ToInt32(monthvalue) - mon2;
                    finalmonth = dbmonth2;
                }
                int year1 = Convert.ToInt32(year);
                int txtyear1 = Convert.ToInt32(txttoyear);
                int finalyear1 = 0;
                if (year1 > txtyear1)
                {
                    finalyear1 = txtfyear2 - txtyear1;
                }
                else
                {
                    finalyear1 = txtyear1 - txtfyear2;
                }
                if (dbmonth2 > 12)
                {
                    finalmonth = 1;
                    finalyear = year1 + 1;
                }
                else
                {
                    finalyear = year1 + finalyear1;
                }
                if (finalmonth != 0 && finalyear != 0)
                {
                    // from month
                    string d = "";
                    if (rdb_frommonth.Checked == true)
                    {
                        d = Convert.ToString("/");
                        dbfromdate = monthvalue + d + txtdate + d + year;
                        dbtodate = finalmonth + d + txttodate + d + finalyear;
                    }
                    else if (rdb_tomonth.Checked == true)  //to month
                    {
                        d = Convert.ToString("/");
                        dbfromdate = finalmonth + d + txtdate + d + year;
                        dbtodate = monthvalue + d + txttodate + d + finalyear;
                    }
                    Session["dtfromdate"] = Convert.ToDateTime(dbfromdate);
                    Session["dt1todate"] = Convert.ToDateTime(dbtodate);
                    Div2.Visible = true;
                    rdb_commonmess.Checked = true;
                    rdb_indmess.Checked = false;
                    rdb_indmess_CheckedChange(sender, e);
                    cb_addinco_CheckedChange(sender, e);
                    rdb_commonmess_CheckedChange(sender, e);
                    cb_addex_CheckedChange(sender, e);
                }
                else
                {
                    lblalerterr.Visible = true;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Set Date Setting";
                }
                string hosteln = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                string collegen = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code");
                lbl_clg.Text = collegen;
                lbl_hosname.Text = hosteln;
                if (rdb_indivual1.Checked == true)
                {
                    hoscode1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                    Session["Messidcode"] = Convert.ToString(hoscode1);
                    hoscode = d2.Gethostelcode_inv(hoscode1);

                    //magesh 24.3.18
                    if (rdb_nondiv.Checked == true)
                    {
                        ViewState["fixnondivded"] = "nondiv";
                        string messhoscode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                        Session["hos_codes"] = Convert.ToString(messhoscode).Trim();
                        cb_addinco.Enabled = false;
                        cb_addex.Enabled = false;
                        cb_adj_exe.Enabled = false;
                        clgcode = d2.GetFunction("select College_Code from HostelMessSettings where Hostel_Code='" + hoscode1 + "'");

                        //magesh 4.7.18

                        clgcode = Convert.ToString(Session["collegecode"]);


                    }//magesh 24.3.18
                    else
                    {
                        ViewState["fixnondivded"] = "";
                        clgcode = d2.GetFunction("select CollegeCode from HM_MessMaster where MessMasterpk='" + hoscode1 + "'");
                        clgcode = Convert.ToString(Session["collegecode"]);

                    }
                    Session["hoscode"] = Convert.ToString(hoscode);
                    Session["clgcode"] = Convert.ToString(clgcode);
                    Session["messcode"] = Convert.ToString(hoscode1);
                    ds2.Clear();
                    #region magesh 26.3.18
                    //string q = "select distinct MessBillType,IncludeRebate from HM_HostelMaster where HostelMasterPK in('" + hoscode + "')";
                    //ds2 = d2.select_method_wo_parameter(q, "Text");

                    //if (ds2.Tables[0].Rows.Count > 0)
                   // {
                    //    string fixtype = Convert.ToString(ds2.Tables[0].Rows[0]["MessBillType"].ToString());
                    //    string isrebate = Convert.ToString(ds2.Tables[0].Rows[0]["IncludeRebate"].ToString());
                    #endregion
                        collegen = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code");
                        hosteln = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                        string messcode1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                        lbl_clg.Text = collegen;
                        lbl_hosname.Text = hosteln;
                        lbl_clgn.Visible = true;
                        lbl_hosn.Visible = true;
                        //if (fixtype.Trim() != "" && fixtype.Trim() == "0" && fixtype.Trim() != null)//magesh 26.3.18
                    if (rdb_fix.Checked == true)
                        {
                            cb_stuadd.Visible = true;
                            cb_addinco.Visible = true;
                            cb_addex.Visible = true;
                            cb_adj_exe.Visible = true;
                            cb_instaff.Visible = true;
                            cb_dayssch.Visible = true;
                            cb_hosteler.Visible = true;
                            lbl_year.Visible = false;
                            lbl_mon.Visible = false;
                            lbl_messfee.Visible = false;
                            txt_messfess.Visible = false;
                            rdo_day.Visible = false;
                            rdo_month.Visible = false;

                            string q4 = "select * from HostelMessSettings where Hostel_Code='" + hoscode1 + "' and college_code='" + clgcode + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(q4, "Text");
                            int cbinstaff = 0;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ViewState["isdivident"] = "Divid";
                                cb_instaff.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeStaff"]);
                                cb_dayssch.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeDaysscholour"]);
                                cb_hosteler.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeHosteler"]);
                                bool value = Convert.ToBoolean(ds.Tables[0].Rows[0]["RebateType"]);
                                if (value == true)
                                {
                                    rdb_common.Checked = false;
                                    rdb_indivula.Checked = true;
                                }
                                else
                                {
                                    rdb_common.Checked = true;
                                    rdb_indivula.Checked = false;
                                }
                                cb_stuadd.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeStudAdd"]);
                                #region magesh 26.3.18
                                //cb_addex.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeAddExp"]);

                                //cb_addinco.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeAddInc"]);
                                //cb_adj_exe.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["includeAdjustexe"]);
                                if (ViewState["fixnondivded"] == "nondiv")
                                {
                                    cb_addex.Checked = false;

                                    cb_addinco.Checked = false;
                                    cb_adj_exe.Checked = false;
                                }
                                else
                                {
                                    cb_addinco.Enabled = true;
                                    cb_addex.Enabled = true;
                                    cb_adj_exe.Enabled = true;
                                    cb_addex.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeAddExp"]);

                                    cb_addinco.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IncludeAddInc"]);
                                    cb_adj_exe.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["includeAdjustexe"]);
                                }
#endregion
                                string guest = Convert.ToString(ds.Tables[0].Rows[0]["includeguest"]);
                                if (guest.Trim() != "")
                                {
                                    cb_guest.Checked = true;
                                }
                                else
                                {
                                    cb_guest.Checked = false;
                                }
                                //hosteln = d2.GetFunction("select Hostel_Name from Hostel_Details where Hostel_code='" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "'");
                                // string hostelc = d2.Gethostelcode(hoscode);//14.10.15
                                //string hosteln = d2.GetFunction("select distinct messname from MessMaster m,MessDetail md where m.MessID =md.MessID and m.MessID in ('" + hoscode + "')");
                                string clgcod = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                                lbl_clg.Text = collegen;
                                lbl_hosname.Text = hosteln;
                                lbl_clgn.Visible = true;
                                lbl_hosn.Visible = true;
                                lbl_clg.Visible = true;
                                lbl_hosname.Visible = true;
                                lbl_clgname.Visible = false;
                                txt_clgname.Visible = false;
                                Panel1.Visible = false;
                                lbl_hostelname.Visible = false;
                                txt_hostelname.Visible = false;
                                Panel6.Visible = false;
                            }
              
                            else
                            {
                                //hoscode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                                //clgcode = d2.GetFunction("select College_Code from MessMaster where MessID='" + hoscode1 + "'");
                                //Session["hoscode"] = Convert.ToString(hoscode);
                                //Session["clgcode"] = Convert.ToString(clgcode);
                                //collegen = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code");
                                //hosteln = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                                //string messcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                                lbl_clg.Text = collegen;
                                lbl_hosname.Text = hosteln;
                                lbl_clgn.Visible = true;
                                lbl_hosn.Visible = true;
                                lbl_clg.Visible = true;
                                lbl_hosname.Visible = true;
                                lbl_clgname.Visible = false;
                                txt_clgname.Visible = false;
                                Panel1.Visible = false;
                                lbl_hostelname.Visible = false;
                                txt_hostelname.Visible = false;
                                Panel6.Visible = false;
                                checkboxfalse();
                                Div2.Visible = true;
                                rdb_commonmess.Checked = true;
                                rdb_indmess.Checked = false;
                                rdb_indmess_CheckedChange(sender, e);
                                cb_addinco_CheckedChange(sender, e);
                                rdb_commonmess_CheckedChange(sender, e);
                                cb_addex_CheckedChange(sender, e);
                                ViewState["isdivident"] = "Divid";
                            }
                        }
                       // else if (fixtype.Trim() != "" && fixtype.Trim() == "1" && fixtype.Trim() != null)
                    else
                        {
                            cb_stuadd.Visible = false;
                            cb_addinco.Visible = false;
                            cb_addex.Visible = false;
                            cb_adj_exe.Visible = false;
                            cb_instaff.Visible = false;
                            cb_dayssch.Visible = false;
                            cb_hosteler.Visible = false;
                            cb_guest.Visible = false;
                            lbl_year.Visible = true;
                            lbl_mon.Visible = true;
                            lbl_messfee.Visible = true;
                            txt_messfess.Visible = true;
                            rdo_day.Visible = true;
                            rdo_month.Visible = true;
                            collegen = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code");
                            hosteln = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                            string messcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                            lbl_clg.Text = collegen;
                            lbl_hosname.Text = hosteln;
                            lbl_clgn.Visible = true;
                            lbl_hosn.Visible = true;
                            ViewState["isdivident"] = "Non";
                            monthvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                            string q5 = "select  RebateType,MessAmt,Messmonth,MessYear,MessType from HostelMessSettings where Hostel_Code='" + hoscode1 + "' and college_code='" + clgcode + "' and Messmonth='" + monthvalue + "' and MessYear='" + year + "' ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(q5, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                bool value = Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
                                bool value1 = Convert.ToBoolean(ds.Tables[0].Rows[0][4]);
                                if (value == true)
                                {
                                    rdb_common.Checked = false;
                                    rdb_indivula.Checked = true;
                                }
                                else
                                {
                                    rdb_common.Checked = true;
                                    rdb_indivula.Checked = false;
                                }
                                if (value1 == true)
                                {
                                    rdo_day.Checked = false;
                                    rdo_month.Checked = true;
                                }
                                else
                                {
                                    rdo_day.Checked = true;
                                    rdo_month.Checked = false;
                                }
                                string messamt = Convert.ToString(ds.Tables[0].Rows[0][1]);
                                txt_messfess.Text = messamt;
                                //cb_instaff.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][2]);
                            }
                        #region magesh 26.3.18
                            //if (isrebate.Trim() == "True")//isrebate.Trim() != "" && isrebate.Trim() == "1" && isrebate.Trim() != null
                            //{
                            //    lbl_rebate.Visible = true;
                            //    rdb_common.Visible = true;
                            //    rdb_indivula.Visible = true;
                            //}
                            //else
                            //{
                            //    lbl_rebate.Visible = false;
                            //    rdb_common.Visible = false;
                            //    rdb_indivula.Visible = false;
                            //    cb_stuadd.Visible = false;
                            //    cb_addinco.Visible = false;
                            //    cb_addex.Visible = false;
                            //    cb_adj_exe.Visible = false;
                            //    cb_instaff.Visible = false;
                            //    cb_dayssch.Visible = false;
                            //    cb_hosteler.Visible = false;
                            //}
                        #endregion
                        }
                    //}magesh 26.3.18
                }
                else
                {
                    lbl_clgname.Visible = true;
                    txt_clgname.Visible = true;
                    Panel1.Visible = true;
                    lbl_hostelname.Visible = true;
                    txt_hostelname.Visible = true;
                    Panel6.Visible = true;
                    lbl_clgname.Visible = true;
                    lbl_clg.Visible = false;
                    lbl_hosname.Visible = false;
                    lbl_clgn.Visible = false;
                    lbl_hosn.Visible = false;
                    common();
                }
            }
        }
        catch
        { }
    }
    protected void common()
    {
        string collegename = "";
        for (int i = 0; i < cbl_clgname.Items.Count; i++)
        {
            if (cbl_clgname.Items[i].Selected == true)
            {
                if (collegename == "")
                {
                    collegename = "" + cbl_clgname.Items[i].Value.ToString() + "";
                }
                else
                {
                    collegename = collegename + "" + "," + "" + "" + cbl_clgname.Items[i].Value.ToString() + "";
                }
            }
        }
        string hostelcode1 = "";
        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                if (hostelcode1 == "")
                {
                    hostelcode1 = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                }
                else
                {
                    hostelcode1 = hostelcode1 + "" + "," + "" + "" + cbl_hostelname.Items[i].Value.ToString() + "";
                }
            }
        }
        //string clgcode = d2.GetFunction("select college_code  from Hostel_Details where Hostel_code in('" + hostelcode1 + "')");
        if (Convert.ToString(ViewState["isdivident"]) == "Divid")
        {
            string q4 = "select * from HostelMessSettings where Hostel_Code='" + hostelcode1 + "' and college_code='" + collegename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q4, "Text");
            int cbinstaff = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                cb_instaff.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][2]);
                cb_dayssch.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][3]);
                cb_hosteler.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][4]);
                bool value = Convert.ToBoolean(ds.Tables[0].Rows[0][5]);
                if (value == true)
                {
                    rdb_common.Checked = false;
                    rdb_indivula.Checked = true;
                }
                else
                {
                    rdb_common.Checked = true;
                    rdb_indivula.Checked = false;
                }
                cb_stuadd.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][6]);
                cb_addex.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][7]);
                cb_addinco.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][8]);
            }
            else
            {
            }
        }
        else if (Convert.ToString(ViewState["isdivident"]) == "Non")
        {
            string q4 = "select * from HostelMessSettings where Hostel_Code='" + hostelcode1 + "' and college_code='" + collegename + "' and MessType='1'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q4, "Text");
        }
    }
    protected void checkboxfalse()
    {
        cb_instaff.Checked = false;
        cb_dayssch.Checked = false;
        cb_hosteler.Checked = false;
        rdb_common.Checked = false;
        cb_stuadd.Checked = false;
        cb_addex.Checked = false;
        cb_adj_exe.Checked = false;
        cb_addinco.Checked = false;
    }
    protected void btn_save_dateset_Click(object sender, EventArgs e)
    {
        string fromdate = Convert.ToString(txt_fromdate.Text);
        string todate = Convert.ToString(txt_todate.Text);
        string mi = Convert.ToString("-");
        string linkvalue = fromdate + mi + todate;
        string hostelmessdate = "Hostel Mess Date";
        string linkvalue1 = "";
        if (rdb_frommonth.Checked == true)
        {
            linkvalue1 = "0";
        }
        else if (rdb_tomonth.Checked == true)
        {
            linkvalue1 = "1";
        }
        string q1 = "if exists (select * from InsSettings where LinkName ='" + hostelmessdate + "' and college_code ='" + collegecode1 + "')update InsSettings set LinkValue ='" + linkvalue + "'  where LinkName ='" + hostelmessdate + "' and college_code ='" + collegecode1 + "' else INSERT INTO InsSettings(LinkName,LinkValue,college_code) VALUES('" + hostelmessdate + "','" + linkvalue + "','" + collegecode1 + "')";
        int ins1 = d2.update_method_wo_parameter(q1, "Text");
        string q2 = "if exists (select * from InsSettings where LinkName ='Hostel Mess Calculate Date' and college_code ='" + collegecode1 + "')update InsSettings set LinkValue ='" + linkvalue1 + "'  where LinkName ='Hostel Mess Calculate Date' and college_code ='" + collegecode1 + "' else INSERT INTO InsSettings(LinkName,LinkValue,college_code) VALUES('Hostel Mess Calculate Date','" + linkvalue1 + "','" + collegecode1 + "')";
        int ins2 = d2.update_method_wo_parameter(q2, "Text");
        if (ins1 != 0 && ins2 != 0)
        {
            poperrjs.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Saved Successfully";
            lblalerterr.Visible = true;
        }
    }
    protected void lb_datesetting_Click(object sender, EventArgs e)
    {
        try
        {
            string hostelmessdate = "Hostel Mess Date";
            poperrjs.Visible = true;
            string q2 = "select LinkValue  from InsSettings where LinkName='" + hostelmessdate + "'";
            ds = d2.select_method_wo_parameter(q2, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string lnkvalue1 = Convert.ToString(ds.Tables[0].Rows[0][0].ToString());
                char[] delimiterChars = { '-' };
                string[] lnkvalue = lnkvalue1.Split(delimiterChars);
                txt_fromdate.Text = Convert.ToString(lnkvalue[0]);
                txt_todate.Text = Convert.ToString(lnkvalue[1]);
            }
            string q3 = "select LinkValue  from InsSettings where LinkName='Hostel Mess Calculate Date'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(q3, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string check = Convert.ToString(ds1.Tables[0].Rows[0]["LinkValue"]);
                if (check.Trim() == "0")
                {
                    rdb_frommonth.Checked = true;
                    rdb_tomonth.Checked = false;
                }
                else if (check.Trim() == "1")
                {
                    rdb_tomonth.Checked = true;
                    rdb_frommonth.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_generate_Click(object sender, EventArgs e)
    {
        try
        {
            dt = Convert.ToDateTime(Session["dtfromdate"]);
            dt1 = Convert.ToDateTime(Session["dt1todate"]);
            string year2 = dt.ToString("yyyy");
            int messy = Convert.ToInt32(year2);
            string mont = dt1.ToString("MM");
          
            int mont2 = Convert.ToInt32(mont);
            TimeSpan ts = dt1 - dt;
            days = ts.Days;
            days++;
            string holidayquery = " select COUNT(*)as holiday_count from HT_Holidays where HolidayType='1' and HolidayForDayscholar ='1' and HolidayForHostler ='1' and  HolidayForStaff ='1' and HolidayDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and messcode='" + Convert.ToString(Session["messcode"]) + "'";
            ds4.Clear();
            ds4 = d2.select_method_wo_parameter(holidayquery, "Text");
            if (ds4.Tables[0].Rows.Count > 0)
                days = days - Convert.ToInt32(ds4.Tables[0].Rows[0]["holiday_count"]);
            Session["days"] = Convert.ToInt32(days);
            string includestaff = "";
            string daysscholour = "";
            string hosteler = "";
            string additionalincome = "";
            string includestudentadditional = "";
            string additionalexpances = "";
            string common = "";
            double rebateamt1 = 0;
            double rebate_days1 = 0;
            double fixedfinalval = 0;
            double additionalamt1 = 0;
            string rebate_days = "";
            string rebate_Amount = "";
            double finalcalvalue = Convert.ToDouble(Session["finalcalvalue"]);
            double feeamount1 = 0;
            string fee_amount = "";
            double fee_amt = 0;
            ArrayList countdays = new ArrayList();
            bool insertflag = false;
            bool insertflag1 = false;
            bool feeallotfeestatus = false;
            bool feeallotfeestatus1 = false;
            double messfee = 0;
            string guest = "";
            string adjexe = "";
            string appcode = "";
            
            string group = string.Empty;
            DataSet dshealthfees = new DataSet();
            DataSet dsgymfees = new DataSet();
            DataSet dsbreakfees = new DataSet();
            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                if (cbl_groupname.Items[i].Selected == true)
                {
                    if (group == "")
                        group = "" + cbl_groupname.Items[i].Value.ToString() + "";
                    else
                        group = group + "'" + "," + "" + "'" + cbl_groupname.Items[i].Value.ToString() + "";
                }
            }
            string groupex = string.Empty;
            for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
            {
                if (cbl_groupnameex.Items[i].Selected == true)
                {
                    if (groupex == "")
                        groupex = "" + cbl_groupnameex.Items[i].Value.ToString() + "";
                    else
                        groupex = groupex + "'" + "," + "" + "'" + cbl_groupnameex.Items[i].Value.ToString() + "";
                }
            }
            #region Divident
            if (Convert.ToString(ViewState["isdivident"]) == "Divid")
            {
                if (cb_instaff.Checked)
                    includestaff = "1";
                else
                    includestaff = "0";
                if (cb_dayssch.Checked)
                    daysscholour = "1";
                else
                    daysscholour = "0";
                if (cb_hosteler.Checked)
                    hosteler = "1";
                else
                    hosteler = "0";
                //rebate
                if (rdb_common.Checked)
                    common = "0";
                else
                    common = "1";
                //additional
                if (cb_stuadd.Checked)
                    includestudentadditional = "1";
                else
                    includestudentadditional = "0";
                if (cb_addinco.Checked)
                    additionalincome = "1";
                else
                    additionalincome = "0";
                if (cb_addex.Checked)
                    additionalexpances = "1";
                else
                    additionalexpances = "0";
                if (cb_guest.Checked)
                    guest = "1";
                else
                    guest = "0";
                if (cb_adj_exe.Checked)
                    adjexe = "1";
                else
                    adjexe = "0";
                string mess_type = "";
                if (rdb_commonmess.Checked)
                    mess_type = "0";
                else
                    mess_type = "1";
                if (rdb_indivual1.Checked == true)
                {
                    hoscode = Convert.ToString(Session["hoscode"]);
                    clgcode = Convert.ToString(Session["clgcode"]);
                    messcode = Convert.ToString(Session["messcode"]);
                    int fincyr = 0;
                    string finyrP = d2.getCurrentFinanceYear(Session["usercode"].ToString(), Convert.ToString(clgcode));
                    int.TryParse(Convert.ToString(finyrP), out fincyr);
                    #region Hostel messsettings
                    string q1 = "if exists(select * from HostelMessSettings where Hostel_Code= '" + messcode + "' and college_code='" + clgcode + "') update HostelMessSettings set  IncludeStaff='" + includestaff + "' ,IncludeDaysscholour='" + daysscholour + "' , IncludeHosteler='" + hosteler + "' , RebateType='" + common + "' , IncludeStudAdd='" + includestudentadditional + "' , IncludeAddExp='" + additionalexpances + "' , IncludeAddInc='" + additionalincome + "',includeGuest='" + guest + "',includeAdjustexe='" + adjexe + "',messgen_type='" + mess_type + "' where Hostel_Code ='" + messcode + "'  else insert into HostelMessSettings (College_Code,Hostel_Code,IncludeStaff,IncludeDaysscholour, IncludeHosteler,RebateType,IncludeStudAdd,IncludeAddExp,IncludeAddInc,includeGuest,includeAdjustexe,messgen_type) values('" + clgcode + "','" + messcode + "','" + includestaff + "','" + daysscholour + "','" + hosteler + "','" + common + "','" + includestudentadditional + "','" + additionalexpances + "','" + additionalincome + "','" + guest + "','" + adjexe + "','" + mess_type + "')";
                    int ins = d2.update_method_wo_parameter(q1, "Text");
                    #endregion
                    double TotalperdayAmt = 0;
                    double TotalCount = 0;
                    double NonvegPerdayAmt = 0;
                    double NonvegCount = 0;
                    double VegCount = 0;
                    double VegExpansestotal = 0;
                    double NonvegExpanceTotal = 0;
                    double CommonExpances = 0;
                    if (rdb_commonmess.Checked == true)
                    {
                        if (Convert.ToString(ViewState["fixnondivded"]) == "nondiv")
                        {
                            ViewState["fixdivded"] = Convert.ToString(Session["hos_codes"]).Trim();
                            nondiv(ref  TotalperdayAmt, ref  TotalCount, ref  NonvegPerdayAmt, ref  NonvegCount, ref  VegCount, ref  VegExpansestotal, ref  NonvegExpanceTotal, ref  CommonExpances);
                            

                        }
                        else
                        {
                            calculation(ref TotalperdayAmt, ref TotalCount, ref  NonvegPerdayAmt, ref  NonvegCount, ref VegCount, ref VegExpansestotal, ref NonvegExpanceTotal, ref CommonExpances);
                            if (TotalCount == 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please Update Student Details";
                                lblalerterr.Visible = true;
                                return;
                            }
                        }
                        
                    }
                    else if (rdb_indmess.Checked == true)
                    {
                        indiviual_calculation();
                    }
                    if (Convert.ToString(ViewState["fixnondivded"]) == "nondiv")
                    {

                        string sturoll = "";
                        if (grantday_hash.Count > 0)
                        {
                            #region Hosteler Generation
                            foreach (DictionaryEntry parameters in grantday_hash)
                            {
                                sturoll = Convert.ToString(parameters.Key);
                                string grantdays1 = Convert.ToString(parameters.Value);
                                string Rebateamounts = Convert.ToString(Rebateamount_hash[sturoll]);
                                string qu2 = "select h.HostelMasterFK,r.college_code,r.degree_code,h.StudMessType from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and r.app_no='" + sturoll + "' and MemType='1' and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";
                                qu2 = qu2 + " select APP_No,HostelMasterFK from HT_HostelRegistration where MemType=3 and IsVacated=0 and HostelMasterFK in('" +Convert.ToString(Session["hos_codes"])+ "') and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";
                                qu2 = qu2 + " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                                ds6.Clear();
                                ds6 = d2.select_method_wo_parameter(qu2, "Text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {
                                    string hostelcode3 = Convert.ToString(ds6.Tables[0].Rows[0]["HostelMasterFK"]);
                                    string collegecode3 = Convert.ToString(ds6.Tables[0].Rows[0]["college_code"]);
                                    string hostelerdegreecode = Convert.ToString(ds6.Tables[0].Rows[0]["degree_code"]);
                                    string StudentMessType = Convert.ToString(ds6.Tables[0].Rows[0]["StudMessType"]);
                                    
                                    double days1 = days - Convert.ToInt32(grantdays1);
                                    double studentstrentgh = 0;
                                    string expgroup = "";
                                    string expgroupamt = "";
                                    double studMessTypeAmt = 0;
                                    double mandays = 0;
                                    string header_id = string.Empty;
                                    string ledgPK = string.Empty;
                                    string exincludemessbill = string.Empty;
                                    string header_idgym = string.Empty;
                                    string ledgPKgym = string.Empty;
                                    string exincludemessbillgym = string.Empty;
                                    string header_idbreak = string.Empty;
                                    string ledgPKbreak = string.Empty;
                                    string exincludemessbillbreak = string.Empty;
                                    int mess;
                                    double ExpancesTotal1 = 0.0;

                                  
                                    int.TryParse(StudentMessType,out mess);
                                    studentstrentgh = Convert.ToDouble(typhsstu[mess]);
                                            double value = Convert.ToDouble(typamo[mess]);
                                            studMessTypeAmt = value;
                                            fixedfinalval = value * days1;
                                            mandays = Convert.ToDouble(totcount[mess]);
                                            string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + collegecode1 + "'";
                                            dshealthfees.Clear();
                                            dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
                                            if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
                                            {
                                                header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                                                ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                                                exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
                                            }
                                            double healthamount = 0.0;
                                            double gym = 0.0;
                                            double breakage = 0.0;
                                            if (sturoll != "")
                                            {

                                                if (exincludemessbill == "1")
                                                {
                                                    string healthamo = d2.GetFunction("select SUM(HealthAdditionalAmt) from HT_HealthCheckup where App_No='" + sturoll + "' and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                                    double.TryParse(healthamo, out healthamount);
                                                   
                                                }
                                            }
                                           
                                            string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode1 + "'";
                                            dsgymfees.Clear();
                                            dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                                            if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                                            {
                                                header_idgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                                                ledgPKgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                                                exincludemessbillgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                                            }
                                            
                                            if (sturoll != "")
                                            {

                                                if (exincludemessbillgym == "1")
                                                {
                                                    string discontinue = "select * from Gym_Discontinue where App_No='" + sturoll + "'";
                                            DataSet gymdis = new DataSet();
                                            gymdis = d2.select_method_wo_parameter(discontinue, "Text");
                                            if (gymdis.Tables[0].Rows.Count == 0)
                                            {
                                                string gymamo = d2.GetFunction("select SUM(cost) from Hm_GymFeeAllot where App_No='" + sturoll + "'");
                                                double.TryParse(gymamo, out gym);
                                            }

                                                }
                                            }
                                           
                                            string breakfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Breakage' and collegecode='" + collegecode1 + "'";
                                            dsbreakfees.Clear();
                                            dsbreakfees = d2.select_method_wo_parameter(breakfeeset, "Text");
                                            if (dsbreakfees.Tables.Count > 0 && dsbreakfees.Tables[0].Rows.Count > 0)
                                            {
                                                header_idbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["header"]);
                                                ledgPKbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["ledger"]);
                                                exincludemessbillbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["Text_value"]);
                                            }

                                            if (sturoll != "")
                                            {

                                                if (exincludemessbillbreak == "1")
                                                {
                                                    string breakamo = d2.GetFunction("select SUM(PayAmount) from IT_BreakageDetails where MemCode='" + sturoll + "' and Breakage_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                                    double.TryParse(breakamo, out breakage);

                                                }
                                            }
                                           
                                            if (rdb_commonmess.Checked == true)
                                            {
                                                fixedfinalval = (fixedfinalval -( Convert.ToDouble(Rebateamounts)* days1));
                                            }
                                            #region Additional Check
                                            double gradays = 0;
                                            double.TryParse(grantdays1, out gradays);
                                            if (days1 != 0)
                                        rebateamt1 = ((fixedfinalval / days1) - Convert.ToDouble(Rebateamounts)) * Convert.ToInt32(grantdays1);
                                    else
                                    rebateamt1 = 0;
                                            if (cb_stuadd.Checked == true)
                                            {
                                                additionalamt1 = 0;
                                                string add_amount = d2.GetFunction("select SUM(AdditionalAmt)as Add_Amount from HT_StudAdditionalDet where App_No = '" + sturoll + "' and TransDate BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and MemType=1");
                                                if (add_amount.Trim() != "" && add_amount.Trim() != "0")
                                                {
                                                    additionalamt1 = Convert.ToDouble(add_amount);
                                                }
                                            }
                                            additionalamt1 += healthamount + breakage + gym;
                                            #endregion
                                            string insmessbillmaster = " if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "',GroupCode='" + expgroup + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "'  else insert HT_MessBillMaster (MessMonth,MessYear, Hostel_code,GroupCode,MessMasterFK) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + Convert.ToString(Session["hos_codes"]) + "','" + expgroup + "','" + Convert.ToString(Session["hos_codes"]) + "')";
                                            int insert = d2.update_method_wo_parameter(insmessbillmaster, "Text");
                                            string messbill_masterpk = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "'");
                                            string insmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + sturoll + "' and MessBillMasterFK='" + messbill_masterpk + "' and MemType='1')update ht_messbilldetail set messamount='" + fixedfinalval + "',MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + expgroupamt + "',ExpanceGroupCode='" + expgroup + "',ExpanceGroupAmtTotal='" + ExpancesTotal1 + "',RebateDays='" + days1 + "' where app_no='" + sturoll + "' and messbillmasterfk='" + messbill_masterpk + "'  and MemType='1' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt, RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode, ExpanceGroupAmtTotal, RebateDays) values('1','" + sturoll + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk + "','" + expgroupamt + "','" + expgroup + "','" + ExpancesTotal1 + "','" + days1 + "')";
                                            int insert1 = d2.update_method_wo_parameter(insmessbilldetails, "Text");
                                            //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                            //rebateamt1 = Math.Ceiling(rebateamt1);
                                            //fixedfinalval = Math.Ceiling(rebateamt1);
                                            string dividingdetails = "if exists(select Hostel_Code from HMessbill_StudDetails where Hostel_Code='" + Convert.ToString(Session["hos_codes"]) + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "'  and MemType='1')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days1 + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + expgroup + "',mandays='" + mandays + "',StudStrength='" + studentstrentgh + "' , MessType='" + StudentMessType + "',MemType='1' where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "'  and MemType='1' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest, Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + messcode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days1 + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + expgroup + "','" + mandays + "','" + studentstrentgh + "','" + StudentMessType + "','1')";
                                            string regenfeeamount = d2.GetFunction("select messamount+messadditonalamt as regenamt from HT_MessBillDetail where app_no='" + sturoll + "' and MessBillMasterFK='" + messbill_masterpk + "' and MemType='1'");

                                            int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                            if (insert1 != 0 && ins != 0)
                                            {
                                                insertflag = true;
                                            }

                                            int FinanceAffected = 0;
                                            if (ds6.Tables[2].Rows.Count > 0)
                                                int.TryParse(Convert.ToString(ds6.Tables[2].Rows[0]["value"]), out FinanceAffected);
                                            if (FinanceAffected == 1)
                                            {
                                                #region Feecatagory
                                                string getsemester = d2.GetFunction("select Current_Semester from Registration where App_No ='" + sturoll + "'");
                                                string textcode = "";
                                                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode3 + "'";
                                                settingquery = settingquery + "   select Degree_code,FeeCategory,t.TextVal  from Fee_degree_match f,textvaltable t where f.FeeCategory=t.TextCode and f.College_code=t.college_code and f.college_code ='" + Convert.ToString(collegecode3) + "'";
                                                ds4.Clear();
                                                ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                                if (ds4.Tables[0].Rows.Count > 0)
                                                {
                                                    string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                                    if (linkvalue == "0" || linkvalue == "1")
                                                    {
                                                        if (linkvalue == "0")
                                                        {
                                                            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                            ds4.Clear();
                                                            ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                            if (ds4.Tables[0].Rows.Count > 0)
                                                            {
                                                                textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                                Session["fee_category"] = Convert.ToString(textcode);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            #region yearwise
                                                            if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                            {
                                                                getsemester = "1 Year";
                                                            }
                                                            else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                            {
                                                                getsemester = "2 Year";
                                                            }
                                                            else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                            {
                                                                getsemester = "3 Year";
                                                            }
                                                            else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                            {
                                                                getsemester = "4 Year";
                                                            }
                                                            else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                            {
                                                                getsemester = "5 Year";
                                                            }
                                                            else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                            {
                                                                getsemester = "6 Year";
                                                            }
                                                            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                            ds4.Clear();
                                                            ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                            if (ds4.Tables[0].Rows.Count > 0)
                                                            {
                                                                textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                                Session["fee_category"] = Convert.ToString(textcode);
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    if (linkvalue == "2")
                                                    {
                                                        ds4.Tables[1].DefaultView.RowFilter = "degree_code='" + hostelerdegreecode + "'";
                                                        DataView dvsem1 = ds4.Tables[1].DefaultView;
                                                        if (dvsem1.Count > 0)
                                                        {
                                                            textcode = Convert.ToString(dvsem1[0]["FeeCategory"]);
                                                            Session["fee_category"] = Convert.ToString(textcode);
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Finance Affected query
                                                int insexcessdet = 0;
                                                int insadjustinclude = 0;
                                                int insExcessLedger = 0;
                                                int insfeeallot = 0;
                                                appcode = sturoll;
                                                feeamount1 = fixedfinalval;
                                                fee_amt = feeamount1 + additionalamt1;
                                                fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                                string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + hostelcode3 + "'";// and CollegeCode='" + collegecode1 + "'";
                                                ds7.Clear();
                                                ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                                string messheader = "";
                                                string messledger = "";
                                                if (ds7.Tables[0].Rows.Count > 0)
                                                {
                                                    messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                                    messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                                }
                                                string errormsg = d2.GetFunction(" select h.headerpk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode=l.collegecode and h.headerpk='" + messheader + "' and l.ledgerpk ='" + messledger + "'");
                                                if (errormsg == "0" || errormsg.Trim() == "")
                                                {
                                                    lblalerterr.Visible = true;
                                                    alertpopwindow.Visible = true;
                                                    Div2.Visible = false;
                                                    lblalerterr.Text = "Please Update Header, Ledger, Financial Year Setting";
                                                    return;
                                                }
                                                //string transcode = generateReceiptNo();
                                                //if (transcode.Trim() != "")
                                                //{
                                                if (cb_adj_exe.Checked == true)
                                                {
                                                    #region adject in execise
                                                    //string adjustinclude = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK,IsExcessAdj,ExcessAdjAmt) values('" + dtaccessdate + "','" + dtaccesstime + "','" + transcode + "','1','" + appcode + "','" + messheader + "','" + messledger + "','" + textcode + "','0','" + fee_amount + "','1','1','" + fincyr + "','1','" + fee_amount + "')";
                                                    //insadjustinclude = d2.update_method_wo_parameter(adjustinclude, "Text");
                                                    //if (insadjustinclude != 0)
                                                    //{
                                                    if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                                    {
                                                        string regenexcess = "if exists (select * from ft_excessdet where app_no = '" + appcode + "' and FeeCategory='" + textcode + "')update ft_excessdet set AdjAmt=AdjAmt - '" + regenfeeamount + "',BalanceAmt=BalanceAmt+'" + regenfeeamount + "'  where App_No = '" + appcode + "' and MemType = 1 and FeeCategory='" + textcode + "' ";
                                                        int regen = d2.update_method_wo_parameter(regenexcess, "Text");
                                                        string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + appcode + "' and FeeCategory='" + textcode + "'");
                                                        string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt - '" + regenfeeamount + "',BalanceAmt =BalanceAmt + '" + regenfeeamount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "'";
                                                        int regen1 = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                                    }
                                                    string excessdet = "if exists ( select * from ft_excessdet where app_no = '" + appcode + "' and FeeCategory='" + textcode + "') update FT_ExcessDet set AdjAmt = AdjAmt + '" + fee_amount + "',BalanceAmt = (BalanceAmt - isnull('" + fee_amount + "',0))  where App_No = '" + appcode + "' and MemType = 1 and FeeCategory='" + textcode + "' else  insert into ft_excessdet (ExcessTransDate,TransTime,DailyTransCode,App_No ,MemType,ExcessType,ExcessAmt, AdjAmt,BalanceAmt,FinYearFK,FeeCategory) values ('" + dtaccessdate + "','" + dtaccesstime + "','','" + appcode + "','1','1','0','" + fee_amount + "','0','" + fincyr + "','" + textcode + "')";
                                                    insexcessdet = d2.update_method_wo_parameter(excessdet, "Text");
                                                    //}
                                                    if (insexcessdet != 0)
                                                    {
                                                        string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + appcode + "' and FeeCategory='" + textcode + "'");
                                                        string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt + '" + fee_amount + "',BalanceAmt =BalanceAmt - '" + fee_amount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,AdjAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values ('" + messheader + "','" + messledger + "','0','" + fee_amount + "','0','" + excessdepk + "','" + textcode + "','" + fincyr + "')";
                                                        insExcessLedger = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                 
                                                    string FeeAmountMonthly = "";
                                                    string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='2'");
                                                    if (previousmonthfee.Trim() == "0")
                                                    {
                                                        FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                                        {
                                                           // FeeAmountMonthly = previousmonthfee;
                                                            string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                            Hashtable hs = new Hashtable();

                                                            double feeval1 = 0;
                                                            FeeAmountMonthly = "";
                                                            foreach (string feeamt in feeamtvalue1)
                                                            {
                                                                string[] val = feeamt.Split(':');
                                                                if (val.Length > 0)
                                                                {
                                                                    if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                                    {
                                                                        if (FeeAmountMonthly != "")
                                                                        {
                                                                            FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                            // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                                        }
                                                                        else
                                                                        {
                                                                            FeeAmountMonthly = Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FeeAmountMonthly == "")
                                                                        {


                                                                            FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                                        }
                                                                        else
                                                                        {
                                                                            FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                                        }
                                                    }
                                                    string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                                    double feeval = 0;
                                                    foreach (string feeamt in feeamtvalue)
                                                    {
                                                        string[] val = feeamt.Split(':');
                                                        if (val.Length > 0)
                                                        {
                                                            if (feeval == 0)
                                                            {
                                                                double.TryParse(val[2].ToString(), out feeval);
                                                            }
                                                            else
                                                            {
                                                                double feeadd = 0;
                                                                double.TryParse(val[2].ToString(), out feeadd);
                                                                feeval += feeadd;
                                                            }
                                                        }
                                                    }
                                                    string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + appcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                                    if (paidmt.Trim() == "")
                                                    {
                                                        paidmt = "0";
                                                    }
                                                    //fee_amount = Convert.ToString(feeval + Convert.ToDouble(fee_amount));
                                                    if (Convert.ToDouble(paidmt) <= Convert.ToDouble(feeval))
                                                    {
                                                        string fee_allot_query = "if exists (select app_no from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "')) update FT_FeeAllot set FeeAmount=" + feeval + ", TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "') else insert into FT_FeeAllot (AllotDate,LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType, PayMode,FeeAmount, FeeAmountMonthly)values ('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + appcode + "','" + feeval + "','" + feeval + "','1','2','" + feeval + "','" + FeeAmountMonthly + "')";
                                                        insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                                        if (insfeeallot != 0)
                                                        {
                                                            double feeallotpk = 0;
                                                            int fnyear = 0;
                                                            double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + appcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                            int.TryParse(d2.GetFunction("select finyearfk from FT_FeeAllot where App_No=" + appcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out fnyear);
                                                            foreach (string feeamt in feeamtvalue)
                                                            {
                                                                string[] val = feeamt.Split(':');
                                                                if (val.Length > 0)
                                                                {
                                                                    if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                                    {
                                                                        string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), AllotYear=" + Convert.ToString(Session["year"]) + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + Convert.ToString(Session["monthvalue"]) + ", " + Convert.ToString(Session["year"]) + ", " + val[2] + ", " + val[2] + ", " + fnyear + ")";
                                                                        d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)
                                                {
                                                    feeallotfeestatus = true;
                                                }
                                                //}
                                                #endregion
                                            }
                                            else
                                            {
                                                feeallotfeestatus = true;
                                            }
                                }
                            }

                            #endregion

                        }
                        if (cbHostlerStaff.Checked)
                        {
                            #region Hostler Staff
                            foreach (DictionaryEntry parameter in staffrebateday_hash)
                            {
                               string roll = Convert.ToString(parameter.Key);
                                string grantday1 = Convert.ToString(parameter.Value);
                                string Rebateamount = Convert.ToString(staffrebateamount_hash[roll]);
                                string Q = "select distinct ht.APP_No, ht.HostelMasterFK,hm.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration ht,HM_HostelMaster hm where ht.HostelMasterFK=hm.HostelMasterPK and ht.MemType='2' and isnull(ht.isdiscontinued,0)=0 and isnull(ht.issuspend,0)=0 and isnull(isvacated,0)=0 and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and HostelMasterFK in('" + Convert.ToString(Session["hos_codes"]) + "') and ht.APP_No='" + roll + "'";
                            Q += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                            DataSet HostlerStaffDs = new DataSet();
                            HostlerStaffDs = d2.select_method_wo_parameter(Q, "text");
                            if (HostlerStaffDs.Tables[0].Rows.Count > 0)
                            {
                                #region Staff generation
                                foreach (DataRow dr in HostlerStaffDs.Tables[0].Rows)
                                {
                                    string StaffMessCode = Convert.ToString(dr["HostelMasterFK"]);
                                    //string guestclgcode = Convert.ToString(HostlerStaffDs.Tables[0].Rows[0]["CollegeCode"]);
                                    string StaffMessType = Convert.ToString(dr["StudMessType"]);
                                    string guestexpgrp = "";
                                    string guestexpamt = "";
                                    string ApplID = Convert.ToString(dr["APP_No"]);
                                    string header_id = string.Empty;
                                    string ledgPK = string.Empty;
                                    string exincludemessbill = string.Empty;
                                    string header_idgym = string.Empty;
                                    string ledgPKgym = string.Empty;
                                    string exincludemessbillgym = string.Empty;
                                    string header_idbreak = string.Empty;
                                    string ledgPKbreak = string.Empty;
                                    string exincludemessbillbreak = string.Empty;
                                    double studMessTypeAmt = 0;
                                    double studentstrentgh = 0;
                                    double mandays = 0;
                                    int mess;
                                    double ExpancesTotal1 = 0.0;
                                    int.TryParse(StaffMessType, out mess);
                                    studentstrentgh = Convert.ToDouble(typhsstaf[mess]);
                                    double value = Convert.ToDouble(typamo[mess]);
                                    studMessTypeAmt = value;
                                    fixedfinalval = value * days;
                                    mandays = Convert.ToDouble(totcount[mess]);
                                    string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + collegecode1 + "'";
                                    dshealthfees.Clear();
                                    dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
                                    if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                                        ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
                                    }
                                    double healthamount = 0.0;
                                    double gym = 0.0;
                                    double breakage = 0.0;
                                    if (ApplID != "")
                                    {

                                        if (exincludemessbill == "1")
                                        {
                                            string healthamo = d2.GetFunction("select SUM(HealthAdditionalAmt) from HT_HealthCheckup where App_No='" + ApplID + "' and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(healthamo, out healthamount);

                                        }
                                    }

                                    string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode1 + "'";
                                    dsgymfees.Clear();
                                    dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                                    if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_idgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                                        ledgPKgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbillgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                                    }

                                    if (ApplID != "")
                                    {

                                        if (exincludemessbillgym == "1")
                                        {
                                             string discontinue = "select * from Gym_Discontinue where App_No='" + ApplID + "'";
                                            DataSet gymdis = new DataSet();
                                            gymdis = d2.select_method_wo_parameter(discontinue, "Text");
                                            if (gymdis.Tables[0].Rows.Count == 0)
                                            {
                                                string gymamo = d2.GetFunction("select SUM(cost) from Hm_GymFeeAllot where App_No='" + ApplID + "' and GymJoinDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                                double.TryParse(gymamo, out gym);
                                            }
                                        }
                                    }

                                    string breakfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Breakage' and collegecode='" + collegecode1 + "'";
                                    dsbreakfees.Clear();
                                    dsbreakfees = d2.select_method_wo_parameter(breakfeeset, "Text");
                                    if (dsbreakfees.Tables.Count > 0 && dsbreakfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_idbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["header"]);
                                        ledgPKbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbillbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["Text_value"]);
                                    }

                                    if (ApplID != "")
                                    {

                                        if (exincludemessbillbreak == "1")
                                        {
                                            string breakamo = d2.GetFunction("select SUM(PayAmount) from IT_BreakageDetails where MemCode='" + sturoll + "' and Breakage_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(breakamo, out breakage);

                                        }
                                    }

                                    if (rdb_commonmess.Checked == true)
                                        fixedfinalval = studMessTypeAmt;
                                    additionalamt1 += healthamount + breakage + gym;
                                    double ExpancesTotal = 0;
                                    string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and  Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "'  else insert HT_MessBillMaster (MessMonth,MessYear, Hostel_code,GroupCode,MessMasterFK) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + Convert.ToString(Session["hos_codes"]) + "','" + guestexpgrp + "','" + Convert.ToString(Session["hos_codes"]) + "')";
                                    int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                    string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "'");
                                    string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + ApplID + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='2')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "' where app_no='" + ApplID + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='2' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal) values('2','" + ApplID + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "')";
                                    int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                    //mandays,StudStrength
                                    //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    //fixedfinalval = Math.Ceiling(rebateamt1);
                                    string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StaffMessType + "' and MemType='2')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + studentstrentgh + "' , MessType='" + StaffMessType + "' , MemType='2'  where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='2' and MessType='" + StaffMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + StaffMessCode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + studentstrentgh + "','" + StaffMessType + "','2')";//Bill_Type='',total=''
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (messbilldetails != 0 && messbillmaster != 0)
                                    {
                                        insertflag1 = true;
                                    }
                                    #region Fincance Affected
                                    int FinanceAffected = 0;
                                    if (HostlerStaffDs.Tables[1].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(HostlerStaffDs.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        string getsemester = "1";
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                        string textcode = string.Empty;
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                            }
                                        }
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + StaffMessCode + "'";//and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }
                                        string FeeAmountMonthly = "";
                                        string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + ApplID + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='1'");
                                        if (previousmonthfee.Trim() == "0")
                                        {
                                            FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                        }
                                        else
                                        {
                                            if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                            {
                                              //  FeeAmountMonthly = previousmonthfee;
                                                string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                Hashtable hs = new Hashtable();

                                                double feeval1 = 0;
                                                FeeAmountMonthly = "";
                                                foreach (string feeamt in feeamtvalue1)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                        {
                                                            if (FeeAmountMonthly != "")
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (FeeAmountMonthly == "")
                                                            {


                                                                FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                            }
                                        }
                                        string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                        double feeval = 0;
                                        foreach (string feeamt in feeamtvalue)
                                        {
                                            string[] val = feeamt.Split(':');
                                            if (val.Length > 0)
                                            {
                                                if (feeval == 0)
                                                {
                                                    double.TryParse(val[2].ToString(), out feeval);
                                                }
                                                else
                                                {
                                                    double feeadd = 0;
                                                    double.TryParse(val[2].ToString(), out feeadd);
                                                    feeval += feeadd;
                                                }
                                            }
                                        }
                                        string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + appcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                        if (paidmt.Trim() == "")
                                        {
                                            paidmt = "0";
                                        }
                                        if (textcode.Trim() != "")
                                        {

                                            string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and App_No in('" + ApplID + "') and memtype='2') update FT_FeeAllot set FeeAmount='" + feeval + "', TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "',App_No='" + ApplID + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + ApplID + "') and memtype='2' else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount,FeeAmountMonthly)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + ApplID + "','" + fee_amount + "','" + fee_amount + "','2','1','" + fee_amount + "','" + FeeAmountMonthly + "')";
                                            insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                            if (insfeeallot != 0)
                                            {
                                                double feeallotpk = 0;
                                                double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + ApplID + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                foreach (string feeamt in feeamtvalue)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + val[0].ToString() + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), FinYearFK=" + fincyr + ",AllotYear=" + val[1].ToString() + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + val[0].ToString() + ", " + val[1].ToString() + ", " + val[2] + ", " + val[2] + ", " + fincyr + ")";
                                                        d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                    }
                                                }
                                            }

                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                        {
                                            feeallotfeestatus1 = true;
                                        }
                                    }
                                    else { feeallotfeestatus1 = true; }
                                    #endregion


                                }
                                #endregion
                            }


                            }
                            #endregion
                        }
                        string guestcode = string.Empty;
                        if (guestgrant_hash.Count > 0)
                        {
                            #region guest generation
                            foreach (DictionaryEntry parameter in guestgrant_hash)
                            {
                                guestcode = Convert.ToString(parameter.Key);
                                string grantday1 = Convert.ToString(parameter.Value);
                                string guestrebateamt = Convert.ToString(guestRebateamount_hash[guestcode]);
                                string q3 = "select distinct gr.APP_No, gr.HostelMasterFK,hd.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and MemType='3' and APP_No in('" + guestcode + "')";
                                q3 += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                                ds6.Clear();
                                ds6 = d2.select_method_wo_parameter(q3, "Text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {
                                    string guesthostelcode = Convert.ToString(ds6.Tables[0].Rows[0]["HostelMasterFK"]);
                                    string guestclgcode = Convert.ToString(ds6.Tables[0].Rows[0]["CollegeCode"]);
                                    string StudentMessType = Convert.ToString(ds6.Tables[0].Rows[0]["StudMessType"]);
                                    double days1 = days - Convert.ToInt32(grantday1);
                                    double studMessTypeAmt = 0;
                                    string guestexpgrp = "";
                                    string guestexpamt = "";
                                    double mandays = 0;
                                    double noofstudentcount = 0;
                                    double studentstrentgh = 0;
                                    string header_id = string.Empty;
                                    string ledgPK = string.Empty;
                                    string exincludemessbill = string.Empty;
                                    string header_idgym = string.Empty;
                                    string ledgPKgym = string.Empty;
                                    string exincludemessbillgym = string.Empty;
                                    string header_idbreak = string.Empty;
                                    string ledgPKbreak = string.Empty;
                                    string exincludemessbillbreak = string.Empty;
                                    int mess;
                                    double ExpancesTotal1 = 0.0;


                                    int.TryParse(StudentMessType, out mess);
                                    studentstrentgh = Convert.ToDouble(typhsgue[mess]);
                                    double value = Convert.ToDouble(typamo[mess]);
                                    studMessTypeAmt = value;
                                    fixedfinalval = value * days;
                                    
                                    mandays = Convert.ToDouble(totcount[mess]);
                                    string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + collegecode1 + "'";
                                    dshealthfees.Clear();
                                    dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
                                    if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                                        ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
                                    }
                                    double healthamount = 0.0;
                                    double gym = 0.0;
                                    double breakage = 0.0;
                                    if (guestcode != "")
                                    {

                                        if (exincludemessbill == "1")
                                        {
                                            string healthamo = d2.GetFunction("select SUM(HealthAdditionalAmt) from HT_HealthCheckup where App_No='" + guestcode + "' and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(healthamo, out healthamount);

                                        }
                                    }

                                    //string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode1 + "'";
                                    //dsgymfees.Clear();
                                    //dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                                    //if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                                    //{
                                    //    header_idgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                                    //    ledgPKgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                                    //    exincludemessbillgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                                    //}

                                    //if (sturoll != "")
                                    //{

                                    //    if (exincludemessbillgym == "1")
                                    //    {
                                    //        string gymamo = d2.GetFunction("select SUM(cost) from Hm_GymFeeAllot where App_No='" + sturoll + "' and GymJoinDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                    //        double.TryParse(gymamo, out gym);

                                    //    }
                                    //}

                                    string breakfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Breakage' and collegecode='" + collegecode1 + "'";
                                    dsbreakfees.Clear();
                                    dsbreakfees = d2.select_method_wo_parameter(breakfeeset, "Text");
                                    if (dsbreakfees.Tables.Count > 0 && dsbreakfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_idbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["header"]);
                                        ledgPKbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbillbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["Text_value"]);
                                    }

                                    if (guestcode != "")
                                    {

                                        if (exincludemessbillbreak == "1")
                                        {
                                            string breakamo = d2.GetFunction("select SUM(PayAmount) from IT_BreakageDetails where MemCode='" + guestcode + "' and Breakage_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(breakamo, out breakage);

                                        }
                                    }

                                    if (rdb_commonmess.Checked == true)
                                    {
                                        fixedfinalval = fixedfinalval - (Convert.ToDouble(guestrebateamt) * days1);
                                    }
                                    else if (rdb_indmess.Checked == true)
                                    {
                                        fixedfinalval = fixedfinalval - (Convert.ToDouble(guestrebateamt) * days1);
                                    }
                                    double gradays = 0;
                                    double.TryParse(grantday1, out gradays);
                                    if (gradays != 0)
                                     
                                    rebateamt1 =((fixedfinalval / days1) - Convert.ToDouble(guestrebateamt)) * Convert.ToInt32(grantday1);
                                    rebateamt1 = 0;
                                    #region Additional Check
                                   
                                    if (cb_stuadd.Checked == true)
                                    {
                                        additionalamt1 = 0;
                                        string add_amount = d2.GetFunction("select SUM(AdditionalAmt)as Add_Amount from HT_StudAdditionalDet where App_No = '" + sturoll + "' and TransDate BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and MemType=1");
                                        if (add_amount.Trim() != "" && add_amount.Trim() != "0")
                                        {
                                            additionalamt1 = Convert.ToDouble(add_amount);
                                        }
                                    }
                                    #endregion
                                    additionalamt1 += healthamount + breakage + gym;
                                    double ExpancesTotal = 0;
                                    string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',Hostel_code='" + messcode + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "' and MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "' else insert HT_MessBillMaster (MessMonth,MessYear, Hostel_code,GroupCode,MessMasterFK) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + Convert.ToString(Session["hos_codes"]) + "','" + guestexpgrp + "','" + Convert.ToString(Session["hos_codes"]) + "')";
                                    int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                    string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + 
                                        Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "'");
                                    string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + guestcode + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='3')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "',RebateDays='" + days1 + "' where app_no='" + guestcode + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='3' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal,RebateDays) values('3','" + guestcode + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "','" + days1 + "')";
                                    int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                    //mandays,StudStrength
                                    //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    //fixedfinalval = Math.Ceiling(rebateamt1);
                                    string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "' and MemType='3')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days1 + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + noofstudentcount + "' , MessType='" + StudentMessType + "' , MemType='3'  where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='3' and MessType='" + StudentMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + messcode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days1 + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + noofstudentcount + "','" + StudentMessType + "','3')";//Bill_Type='',total=''
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (messbilldetails != 0 && messbillmaster != 0)
                                    {
                                        insertflag1 = true;
                                    }
                                    #region Fincance Affected
                                    int FinanceAffected = 0;
                                    if (ds6.Tables[1].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(ds6.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        string getsemester = d2.GetFunction("select Current_Semester  from Registration where app_no ='" + guestcode + "'");
                                        if (getsemester.Trim() == "" || getsemester.Trim() == null || getsemester.Trim() == "0")
                                        {
                                            getsemester = "1";
                                        }
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                        string textcode = "";
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                        // settingquery = settingquery + "   select Degree_code,FeeCategory,t.TextVal  from Fee_degree_match f,textvaltable t where f.FeeCategory=t.TextCode and f.College_code=t.college_code and f.college_code ='" + Convert.ToString(guestclgcode) + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                            }

                                        }
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + guesthostelcode + "'";//and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }


                                        string FeeAmountMonthly = "";
                                        string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + guestcode + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='1'");
                                        if (previousmonthfee.Trim() == "0")
                                        {
                                            FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                        }
                                        else
                                        {
                                            if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                            {
                                               // FeeAmountMonthly = previousmonthfee;
                                                string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                Hashtable hs = new Hashtable();

                                                double feeval1 = 0;
                                                FeeAmountMonthly = "";
                                                foreach (string feeamt in feeamtvalue1)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                        {
                                                            if (FeeAmountMonthly != "")
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (FeeAmountMonthly == "")
                                                            {


                                                                FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                            }
                                        }
                                        string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                        double feeval = 0;
                                        foreach (string feeamt in feeamtvalue)
                                        {
                                            string[] val = feeamt.Split(':');
                                            if (val.Length > 0)
                                            {
                                                if (feeval == 0)
                                                {
                                                    double.TryParse(val[2].ToString(), out feeval);
                                                }
                                                else
                                                {
                                                    double feeadd = 0;
                                                    double.TryParse(val[2].ToString(), out feeadd);
                                                    feeval += feeadd;
                                                }
                                            }
                                        }
                                        string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + guestcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                        if (paidmt.Trim() == "")
                                        {
                                            paidmt = "0";
                                        }
                                        string transcode = generateReceiptNo();
                                        if (textcode.Trim() != "")
                                        {

                                            string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + guestcode + "')) update FT_FeeAllot set FeeAmount='" + feeval + "', TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "',App_No='" + guestcode + "' where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and App_No in('" + guestcode + "') else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount,FeeAmountMonthly)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + guestcode + "','" + fee_amount + "','" + fee_amount + "','3','1','" + fee_amount + "','" + FeeAmountMonthly + "')";
                                            insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                            if (insfeeallot != 0)
                                            {
                                                double feeallotpk = 0;
                                                double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + guestcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                foreach (string feeamt in feeamtvalue)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + val[0].ToString() + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), FinYearFK=" + fincyr + ",AllotYear=" + val[1].ToString() + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + val[0].ToString() + ", " + val[1].ToString() + ", " + val[2] + ", " + val[2] + ", " + fincyr + ")";
                                                        d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                    }
                                                }
                                            }

                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                        {
                                            feeallotfeestatus1 = true;
                                        }
                                    }
                                    else { feeallotfeestatus1 = true; }
                                    #endregion
                                }

                            }
                                  
                            #endregion
                        }
                        if (cb_instaff.Checked)
                        {
                            #region include Staff
                            string Q = "select distinct ht.staff_code, ht.Hostel_Code,hm.CollegeCode,isnull(StudMessType,0)StudMessType from DayScholourStaffAdd ht,HM_HostelMaster hm where ht.Hostel_Code=hm.HostelMasterPK and ht.typ='2' and Date<='" + dt1.ToString("MM/dd/yyyy") + "' and Hostel_Code in('" + Convert.ToString(Session["hos_codes"]) + "')";
                            Q += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                            DataSet HostlerStaffDs = new DataSet();
                            HostlerStaffDs = d2.select_method_wo_parameter(Q, "text");
                            if (HostlerStaffDs.Tables[0].Rows.Count > 0)
                            {
                                #region Staff generation
                                foreach (DataRow dr in HostlerStaffDs.Tables[0].Rows)
                                {
                                    string StaffMessCode = Convert.ToString(dr["Hostel_Code"]);
                                    //string guestclgcode = Convert.ToString(HostlerStaffDs.Tables[0].Rows[0]["CollegeCode"]);
                                    string StaffMessType = Convert.ToString(dr["StudMessType"]);
                                    string guestexpgrp = "";
                                    string guestexpamt = "";
                                    string Appl = Convert.ToString(dr["staff_code"]);
                                    string ApplID = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + Appl + "' and sam.appl_no = sm.appl_no");
                                    string header_id = string.Empty;
                                    string ledgPK = string.Empty;
                                    string exincludemessbill = string.Empty;
                                    string header_idgym = string.Empty;
                                    string ledgPKgym = string.Empty;
                                    string exincludemessbillgym = string.Empty;
                                    string header_idbreak = string.Empty;
                                    string ledgPKbreak = string.Empty;
                                    string exincludemessbillbreak = string.Empty;
                                    double studMessTypeAmt = 0;
                                    double studentstrentgh = 0;
                                    double mandays = 0;
                                    int mess;
                                    double ExpancesTotal1 = 0.0;
                                    int.TryParse(StaffMessType, out mess);
                                    studentstrentgh = Convert.ToDouble(typhsstaf[mess]);
                                    double value = Convert.ToDouble(typamo[mess]);
                                    studMessTypeAmt = value;
                                    mandays = Convert.ToDouble(totcount[mess]);
                                    string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + collegecode1 + "'";
                                    dshealthfees.Clear();
                                    dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
                                    if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                                        ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
                                    }
                                    double healthamount = 0.0;
                                    double gym = 0.0;
                                    double breakage = 0.0;
                                    if (ApplID != "")
                                    {

                                        if (exincludemessbill == "1")
                                        {
                                            string healthamo = d2.GetFunction("select SUM(HealthAdditionalAmt) from HT_HealthCheckup where App_No='" + ApplID + "' and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(healthamo, out healthamount);

                                        }
                                    }

                                    //string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode1 + "'";
                                    //dsgymfees.Clear();
                                    //dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                                    //if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                                    //{
                                    //    header_idgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                                    //    ledgPKgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                                    //    exincludemessbillgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                                    //}

                                    //if (ApplID != "")
                                    //{

                                    //    if (exincludemessbillgym == "1")
                                    //    {
                                    //        string discontinue = "select * from Gym_Discontinue where App_No='" + ApplID + "'";
                                    //        DataSet gymdis = new DataSet();
                                    //        gymdis = d2.select_method_wo_parameter(discontinue, "Text");
                                    //        if (gymdis.Tables[0].Rows.Count == 0)
                                    //        {
                                    //            string gymamo = d2.GetFunction("select SUM(cost) from Hm_GymFeeAllot where App_No='" + ApplID + "' and GymJoinDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                    //            double.TryParse(gymamo, out gym);
                                    //        }

                                    //    }
                                    //}

                                    string breakfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Breakage' and collegecode='" + collegecode1 + "'";
                                    dsbreakfees.Clear();
                                    dsbreakfees = d2.select_method_wo_parameter(breakfeeset, "Text");
                                    if (dsbreakfees.Tables.Count > 0 && dsbreakfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_idbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["header"]);
                                        ledgPKbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbillbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["Text_value"]);
                                    }

                                    if (ApplID != "")
                                    {

                                        if (exincludemessbillbreak == "1")
                                        {
                                            string breakamo = d2.GetFunction("select SUM(PayAmount) from IT_BreakageDetails where MemCode='" + sturoll + "' and Breakage_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(breakamo, out breakage);

                                        }
                                    }

                                    if (rdb_commonmess.Checked == true)
                                        fixedfinalval = studMessTypeAmt;
                                    additionalamt1 += healthamount + breakage + gym;
                                    double ExpancesTotal = 0;
                                    string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + StaffMessCode + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "' else insert HT_MessBillMaster (MessMonth,MessYear, Hostel_code,GroupCode) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + Convert.ToString(Session["hos_codes"]) + "','" + guestexpgrp + "')";
                                    int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                    string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "'");
                                    fixedfinalval = Math.Ceiling(fixedfinalval);
                                    additionalamt1 = Math.Ceiling(additionalamt1);
                                    rebateamt1 = Math.Ceiling(rebateamt1);
                                    string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + ApplID + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='2')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "' where app_no='" + ApplID + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='2' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal) values('2','" + ApplID + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "')";
                                    int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                    //mandays,StudStrength
                                    studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    rebateamt1 = Math.Ceiling(rebateamt1);
                                    fixedfinalval = Math.Ceiling(rebateamt1);
                                    string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StaffMessType + "' and MemType='2')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + studentstrentgh + "' , MessType='" + StaffMessType + "' , MemType='2'  where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='2' and MessType='" + StaffMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + StaffMessCode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + studentstrentgh + "','" + StaffMessType + "','2')";//Bill_Type='',total=''
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (messbilldetails != 0 && messbillmaster != 0)
                                    {
                                        insertflag1 = true;
                                    }
                                    #region Fincance Affected
                                    int FinanceAffected = 0;
                                    if (HostlerStaffDs.Tables[1].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(HostlerStaffDs.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        string getsemester = "1";
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                        string textcode = string.Empty;
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                            }
                                        }
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + StaffMessCode + "'";//and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }
                                        if (textcode.Trim() != "")
                                        {
                                            string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2') update FT_FeeAllot set FeeAmount=FeeAmount+" + fee_amount + ", TotalAmount=TotalAmount+'" + fee_amount + "',BalAmount=BalAmount+'" + fee_amount + "',LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FinYearFK='" + fincyr + "',App_No='" + ApplID + "' where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2' else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + ApplID + "','" + fee_amount + "','" + fee_amount + "','2','1','" + fee_amount + "')";
                                            insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                        {
                                            feeallotfeestatus1 = true;
                                        }
                                    }
                                    else { feeallotfeestatus1 = true; }
                                    #endregion


                                }
                                #endregion
                            }

                            #endregion
                        }
                        if (cb_dayssch.Checked)
                        {
                            #region days scholour
                            string Q = "select distinct ht.Roll_No, ht.Hostel_Code,hm.CollegeCode,isnull(StudMessType,0)StudMessType from DayScholourStaffAdd ht,HM_HostelMaster hm where ht.Hostel_Code=hm.HostelMasterPK and ht.typ='1' and Date<='" + dt1.ToString("MM/dd/yyyy") + "' and Hostel_Code in('" + Convert.ToString(Session["hos_codes"]) + "')";
                            Q += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                            DataSet HostlerStaffDs = new DataSet();
                            HostlerStaffDs = d2.select_method_wo_parameter(Q, "text");
                            if (HostlerStaffDs.Tables[0].Rows.Count > 0)
                            {
                                #region Staff generation
                                foreach (DataRow dr in HostlerStaffDs.Tables[0].Rows)
                                {
                                    string StaffMessCode = Convert.ToString(dr["Hostel_Code"]);
                                    //string guestclgcode = Convert.ToString(HostlerStaffDs.Tables[0].Rows[0]["CollegeCode"]);
                                    string StaffMessType = Convert.ToString(dr["StudMessType"]);
                                    string guestexpgrp = "";
                                    string guestexpamt = "";
                                    string Appl = Convert.ToString(dr["Roll_No"]);
                                  string ApplID=d2.GetFunction("select * from Registration where Roll_No='" + Appl + "'");

                                    string header_id = string.Empty;
                                    string ledgPK = string.Empty;
                                    string exincludemessbill = string.Empty;
                                    string header_idgym = string.Empty;
                                    string ledgPKgym = string.Empty;
                                    string exincludemessbillgym = string.Empty;
                                    string header_idbreak = string.Empty;
                                    string ledgPKbreak = string.Empty;
                                    string exincludemessbillbreak = string.Empty;
                                    double studMessTypeAmt = 0;
                                    double studentstrentgh = 0;
                                    double mandays = 0;
                                    int mess;
                                    double ExpancesTotal1 = 0.0;
                                    int.TryParse(StaffMessType, out mess);
                                    studentstrentgh = Convert.ToDouble(typhsstaf[mess]);
                                    double value = Convert.ToDouble(typamo[mess]);
                                    studMessTypeAmt = value;
                                    mandays = Convert.ToDouble(totcount[mess]);
                                    string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + collegecode1 + "'";
                                    dshealthfees.Clear();
                                    dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
                                    if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                                        ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
                                    }
                                    double healthamount = 0.0;
                                    double gym = 0.0;
                                    double breakage = 0.0;
                                    if (ApplID != "")
                                    {

                                        if (exincludemessbill == "1")
                                        {
                                            string healthamo = d2.GetFunction("select SUM(HealthAdditionalAmt) from HT_HealthCheckup where App_No='" + ApplID + "' and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(healthamo, out healthamount);

                                        }
                                    }

                                    //string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode1 + "'";
                                    //dsgymfees.Clear();
                                    //dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                                    //if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                                    //{
                                    //    header_idgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                                    //    ledgPKgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                                    //    exincludemessbillgym = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                                    //}

                                    //if (ApplID != "")
                                    //{

                                    //    if (exincludemessbillgym == "1")
                                    //    {
                                    //        string gymamo = d2.GetFunction("select SUM(cost) from Hm_GymFeeAllot where App_No='" + ApplID + "' and GymJoinDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                    //        double.TryParse(gymamo, out gym);

                                    //    }
                                    //}

                                    string breakfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Breakage' and collegecode='" + collegecode1 + "'";
                                    dsbreakfees.Clear();
                                    dsbreakfees = d2.select_method_wo_parameter(breakfeeset, "Text");
                                    if (dsbreakfees.Tables.Count > 0 && dsbreakfees.Tables[0].Rows.Count > 0)
                                    {
                                        header_idbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["header"]);
                                        ledgPKbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["ledger"]);
                                        exincludemessbillbreak = Convert.ToString(dsbreakfees.Tables[0].Rows[0]["Text_value"]);
                                    }

                                    if (ApplID != "")
                                    {

                                        if (exincludemessbillbreak == "1")
                                        {
                                            string breakamo = d2.GetFunction("select SUM(PayAmount) from IT_BreakageDetails where MemCode='" + sturoll + "' and Breakage_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'");
                                            double.TryParse(breakamo, out breakage);

                                        }
                                    }

                                    if (rdb_commonmess.Checked == true)
                                        fixedfinalval = studMessTypeAmt;
                                    additionalamt1 += healthamount + breakage + gym;
                                    double ExpancesTotal = 0;
                                    string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + Convert.ToString(Session["hos_codes"]) + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + StaffMessCode + "' else insert HT_MessBillMaster (MessMonth,MessYear, Hostel_code,GroupCode) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + Convert.ToString(Session["hos_codes"]) + "','" + guestexpgrp + "')";
                                    int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                    string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and Hostel_code='" + Convert.ToString(Session["hos_codes"]) + "'");
                                    //magesh 2.4.18
                                    fixedfinalval = Math.Ceiling(fixedfinalval);
                                    additionalamt1 = Math.Ceiling(additionalamt1);
                                    rebateamt1 = Math.Ceiling(rebateamt1);
                                    string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + ApplID + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='1')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "' where app_no='" + ApplID + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='1' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal) values('1','" + ApplID + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "')";
                                    int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                    //mandays,StudStrength
                                    studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    rebateamt1 = Math.Ceiling(rebateamt1);
                                    fixedfinalval = Math.Ceiling(rebateamt1);
                                    string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StaffMessType + "' and MemType='1')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + studentstrentgh + "' , MessType='" + StaffMessType + "' , MemType='1'  where Hostel_Code='" + StaffMessCode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='1' and MessType='" + StaffMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + StaffMessCode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + studentstrentgh + "','" + StaffMessType + "','2')";//Bill_Type='',total=''
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (messbilldetails != 0 && messbillmaster != 0)
                                    {
                                        insertflag1 = true;
                                    }
                                    #region Fincance Affected
                                    int FinanceAffected = 0;
                                    if (HostlerStaffDs.Tables[1].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(HostlerStaffDs.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        string getsemester = "1";
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                        string textcode = string.Empty;
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                            }
                                        }
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + StaffMessCode + "'";//and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }
                                        if (textcode.Trim() != "")
                                        {
                                            string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2') update FT_FeeAllot set FeeAmount=FeeAmount+" + fee_amount + ", TotalAmount=TotalAmount+'" + fee_amount + "',BalAmount=BalAmount+'" + fee_amount + "',LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FinYearFK='" + fincyr + "',App_No='" + ApplID + "' where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2' else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + ApplID + "','" + fee_amount + "','" + fee_amount + "','2','1','" + fee_amount + "')";
                                            insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                        {
                                            feeallotfeestatus1 = true;
                                        }
                                    }
                                    else { feeallotfeestatus1 = true; }
                                    #endregion


                                }
                                #endregion
                            }

                            #endregion
                        }

                        if (cb_guest.Checked == true && cb_hosteler.Checked == true)
                        {
                            if (hostel_bool == true && guest_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Hostel Student";
                            }
                        }
                        else if (cb_guest.Checked == true)
                        {
                            if (guest_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Guest Details";
                            }
                        }
                        else if (cb_hosteler.Checked == true)
                        {
                            if (hostel_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Hostel Student Details";
                            }
                        }
                        if (insertflag == true && feeallotfeestatus == true || insertflag1 == true || feeallotfeestatus1 == true)
                        {
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            Div2.Visible = false;
                            lblalerterr.Text = "Saved Successfully";
                            btn_go_Click(sender, e);
                          
                        }
                      
                    }

                        
                    
                    else
                    {
                        string roll = "";
                        if (grantday_hash.Count > 0)
                        {
                            #region Hosteler Generation
                            foreach (DictionaryEntry parameter in grantday_hash)
                            {
                                roll = Convert.ToString(parameter.Key);
                                string grantday1 = Convert.ToString(parameter.Value);
                                string Rebateamount = Convert.ToString(Rebateamount_hash[roll]);
                                string q2 = "select h.HostelMasterFK,r.college_code,r.degree_code,h.StudMessType from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and r.app_no='" + roll + "' and MemType='1' and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and Messcode in ('" + messcode + "') ";//barath 21.10.17 adm date added";
                                q2 = q2 + " select APP_No,HostelMasterFK from HT_HostelRegistration where MemType=3 and IsVacated=0 and HostelMasterFK in('" + hoscode + "') and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";
                                q2 = q2 + " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                                ds6.Clear();
                                ds6 = d2.select_method_wo_parameter(q2, "Text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {
                                    string hostelcode3 = Convert.ToString(ds6.Tables[0].Rows[0]["HostelMasterFK"]);
                                    string collegecode3 = Convert.ToString(ds6.Tables[0].Rows[0]["college_code"]);
                                    string hostelerdegreecode = Convert.ToString(ds6.Tables[0].Rows[0]["degree_code"]);
                                    string StudentMessType = Convert.ToString(ds6.Tables[0].Rows[0]["StudMessType"]);
                                    double days1 = days - Convert.ToInt32(grantday1);
                                    double noofstudentcount = 0;
                                    string expgroup = "";
                                    string expgroupamt = "";
                                    double studMessTypeAmt = 0;
                                    double mandays = 0;
                                    studMessTypeAmt = StudentMessType == "1" ? NonvegPerdayAmt + TotalperdayAmt : TotalperdayAmt;//+ TotalperdayAmt ;delsi
                                    //mandays = (StudentMessType == "1") ? NonvegCount + TotalCount : TotalCount;
                                    noofstudentcount = StudentMessType == "1" ? NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;//VegTotalNoofStudentstrength;delsi
                                    //mandays = TotalCount; delsi
                                    mandays = StudentMessType == "1" ? NonvegCount : TotalCount;
                                   
                                    //noofstudentcount = TotalNoofStudentstrength;
                                    if (rdb_commonmess.Checked == true)
                                    {
                                        fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount));//(Convert.ToDouble(Session["finalcalvalue"])
                                        fixedfinalval = fixedfinalval * days1;
                                       // fixedfinalval =fixedfinalval-0.2;
                                        #region Expances Calculate
                                        if (expanses_hash.Count > 0)
                                        {
                                            foreach (DictionaryEntry expancesgroup in expanses_hash)
                                            {
                                                string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                double examt = Convert.ToDouble(expancesgroup.Value);
                                                if (expgroup == "")
                                                    expgroup = "" + exgroupcode + "";
                                                else
                                                    expgroup = expgroup + "," + "" + exgroupcode + "";
                                                if (expgroupamt == "")
                                                    expgroupamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                else
                                                    expgroupamt += "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (rdb_indmess.Checked == true)
                                    {
                                        fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * days1;
                                        if (expanses_hash.Count > 0)
                                        {
                                            #region Expances Calculate
                                            foreach (DictionaryEntry expancesgroup in expanses_hash)
                                            {
                                                string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                double examt = Convert.ToDouble(expancesgroup.Value);
                                                double noofstudent = Convert.ToDouble(Session["noofstudent"]);
                                                double noofpersons = examt / noofstudentcount;
                                                fixedfinalval = fixedfinalval + noofpersons;
                                                examt = Convert.ToDouble(examt / noofstudentcount);
                                                //double noofpersons = examt / Convert.ToDouble(Session["finalvalue"]);
                                                //double expensecalvalue = noofpersons * days1;
                                                //fixedfinalval = (fixedfinalval + expensecalvalue);
                                                //examt = Convert.ToDouble(examt / Convert.ToDouble(Session["finalvalue"]));
                                                if (expgroup == "")
                                                {
                                                    expgroup = "" + exgroupcode + "";
                                                }
                                                else
                                                {
                                                    expgroup = expgroup + "," + "" + exgroupcode + "";
                                                }
                                                if (expgroupamt == "")
                                                {
                                                    expgroupamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                }
                                                else
                                                {
                                                    expgroupamt = expgroupamt + "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            //fixedfinalval = Convert.ToDouble(Session["finalcalvalue"]) * days1;
                                            fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * days1;
                                        }
                                    }
                                    #region Additional Check
                                    rebateamt1 = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * Convert.ToInt32(grantday1);
                                    if (cb_stuadd.Checked == true)
                                    {
                                        additionalamt1 = 0;
                                        string add_amount = d2.GetFunction("select SUM(AdditionalAmt)as Add_Amount from HT_StudAdditionalDet where App_No = '" + roll + "' and TransDate BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and MemType=1");
                                        if (add_amount.Trim() != "" && add_amount.Trim() != "0")
                                        {
                                            additionalamt1 = Convert.ToDouble(add_amount);
                                        }
                                    }
                                    #endregion
                                    #region Mess Bill Insert Query
                                    double ExpancesTotal = StudentMessType == "1" ? NonvegExpanceTotal : VegExpansestotal;
                                    if (ExpancesTotal != 0)
                                    {
                                        double VegAmt = 0;
                                        if (StudentMessType == "1")
                                        {
                                            if (VegExpansestotal != 0)
                                                VegAmt = VegExpansestotal / TotalNoofStudentstrength;//NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;
                                            VegAmt = Math.Round(VegAmt, 2, MidpointRounding.AwayFromZero);
                                        }
                                        ExpancesTotal = Math.Round((ExpancesTotal / noofstudentcount), 2, MidpointRounding.AwayFromZero);
                                        ExpancesTotal += VegAmt;
                                    }
                                    else
                                        ExpancesTotal = 0;
                                    string insmessbillmaster = " if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + messcode + "',GroupCode='" + expgroup + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "'  else insert HT_MessBillMaster (MessMonth,MessYear, MessMasterFK,GroupCode) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + messcode + "','" + expgroup + "')";
                                    int insert = d2.update_method_wo_parameter(insmessbillmaster, "Text");
                                    string messbill_masterpk = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "'");
                                    //fixedfinalval = Math.Ceiling(fixedfinalval);
                                    //additionalamt1 = Math.Ceiling(additionalamt1);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    string insmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + roll + "' and MessBillMasterFK='" + messbill_masterpk + "' and MemType='1')update ht_messbilldetail set messamount='" + fixedfinalval + "',MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + expgroupamt + "',ExpanceGroupCode='" + expgroup + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "',RebateDays='" + days1 + "' where app_no='" + roll + "' and messbillmasterfk='" + messbill_masterpk + "'  and MemType='1' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt, RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode, ExpanceGroupAmtTotal, RebateDays) values('1','" + roll + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk + "','" + expgroupamt + "','" + expgroup + "','" + ExpancesTotal + "','" + days1 + "')";
                                    int insert1 = d2.update_method_wo_parameter(insmessbilldetails, "Text");
                                    //magesh 2.4.18
                                    //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    //fixedfinalval = Math.Ceiling(fixedfinalval);
                                    string dividingdetails = "if exists(select Hostel_Code from HMessbill_StudDetails where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "'  and MemType='1')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days1 + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + expgroup + "',mandays='" + mandays + "',StudStrength='" + noofstudentcount + "' , MessType='" + StudentMessType + "',MemType='1' where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "'  and MemType='1' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest, Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + messcode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days1 + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + expgroup + "','" + mandays + "','" + noofstudentcount + "','" + StudentMessType + "','1')";
                                    string regenfeeamount = d2.GetFunction("select messamount+messadditonalamt as regenamt from HT_MessBillDetail where app_no='" + roll + "' and MessBillMasterFK='" + messbill_masterpk + "' and MemType='1'");
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (insert1 != 0 && ins != 0)
                                    {
                                        insertflag = true;
                                    }
                                    #endregion
                                    int FinanceAffected = 0;
                                    if (ds6.Tables[2].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(ds6.Tables[2].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        #region Feecatagory
                                        string getsemester = d2.GetFunction("select Current_Semester from Registration where App_No ='" + roll + "'");
                                        string textcode = "";
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode3 + "'";
                                        settingquery = settingquery + "   select Degree_code,FeeCategory,t.TextVal  from Fee_degree_match f,textvaltable t where f.FeeCategory=t.TextCode and f.College_code=t.college_code and f.college_code ='" + Convert.ToString(collegecode3) + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue == "0" || linkvalue == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    #region yearwise
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                    #endregion
                                                }
                                            }
                                            if (linkvalue == "2")
                                            {
                                                ds4.Tables[1].DefaultView.RowFilter = "degree_code='" + hostelerdegreecode + "'";
                                                DataView dvsem1 = ds4.Tables[1].DefaultView;
                                                if (dvsem1.Count > 0)
                                                {
                                                    textcode = Convert.ToString(dvsem1[0]["FeeCategory"]);
                                                    Session["fee_category"] = Convert.ToString(textcode);
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Finance Affected query
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        appcode = roll;
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 0));
                                    
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + hostelcode3 + "'";// and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }
                                        string errormsg = d2.GetFunction(" select h.headerpk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode=l.collegecode and h.headerpk='" + messheader + "' and l.ledgerpk ='" + messledger + "'");
                                        if (errormsg == "0" || errormsg.Trim() == "")
                                        {
                                            lblalerterr.Visible = true;
                                            alertpopwindow.Visible = true;
                                            Div2.Visible = false;
                                            lblalerterr.Text = "Please Update Header, Ledger, Financial Year Setting";
                                            return;
                                        }
                                        //string transcode = generateReceiptNo();
                                        //if (transcode.Trim() != "")
                                        //{
                                        if (cb_adj_exe.Checked == true)
                                        {
                                            #region adject in execise
                                            //string adjustinclude = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK,IsExcessAdj,ExcessAdjAmt) values('" + dtaccessdate + "','" + dtaccesstime + "','" + transcode + "','1','" + appcode + "','" + messheader + "','" + messledger + "','" + textcode + "','0','" + fee_amount + "','1','1','" + fincyr + "','1','" + fee_amount + "')";
                                            //insadjustinclude = d2.update_method_wo_parameter(adjustinclude, "Text");
                                            //if (insadjustinclude != 0)
                                            //{
                                            if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                            {
                                                string regenexcess = "if exists (select * from ft_excessdet where app_no = '" + appcode + "' and FeeCategory='" + textcode + "')update ft_excessdet set AdjAmt=AdjAmt - '" + regenfeeamount + "',BalanceAmt=BalanceAmt+'" + regenfeeamount + "'  where App_No = '" + appcode + "' and MemType = 1 and FeeCategory='" + textcode + "' ";
                                                int regen = d2.update_method_wo_parameter(regenexcess, "Text");
                                                string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + appcode + "' and FeeCategory='" + textcode + "'");
                                                string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt - '" + regenfeeamount + "',BalanceAmt =BalanceAmt + '" + regenfeeamount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "'";
                                                int regen1 = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                            }
                                            string excessdet = "if exists ( select * from ft_excessdet where app_no = '" + appcode + "' and FeeCategory='" + textcode + "') update FT_ExcessDet set AdjAmt = AdjAmt + '" + fee_amount + "',BalanceAmt = (BalanceAmt - isnull('" + fee_amount + "',0))  where App_No = '" + appcode + "' and MemType = 1 and FeeCategory='" + textcode + "' else  insert into ft_excessdet (ExcessTransDate,TransTime,DailyTransCode,App_No ,MemType,ExcessType,ExcessAmt, AdjAmt,BalanceAmt,FinYearFK,FeeCategory) values ('" + dtaccessdate + "','" + dtaccesstime + "','','" + appcode + "','1','1','0','" + fee_amount + "','0','" + fincyr + "','" + textcode + "')";
                                            insexcessdet = d2.update_method_wo_parameter(excessdet, "Text");
                                            //}
                                            if (insexcessdet != 0)
                                            {
                                                string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + appcode + "' and FeeCategory='" + textcode + "'");
                                                string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt + '" + fee_amount + "',BalanceAmt =BalanceAmt - '" + fee_amount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' and FinYearFK='" + fincyr + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,AdjAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values ('" + messheader + "','" + messledger + "','0','" + fee_amount + "','0','" + excessdepk + "','" + textcode + "','" + fincyr + "')";
                                                insExcessLedger = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            //30.12.16
                                            string FeeAmountMonthly = "";
                                            string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='2'");
                                            if (previousmonthfee.Trim() == "0")
                                            {
                                                FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                            }
                                            else
                                            {
                                                if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                                {
                                                    //magesh 4.4.18
                                                  //FeeAmountMonthly = previousmonthfee;
                                                    //================================
                                                    string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                    Hashtable hs = new Hashtable();
                                                    
                                            double feeval1 = 0;
                                            FeeAmountMonthly = ""; 
                                            foreach (string feeamt in feeamtvalue1)
                                            {
                                                string[] val = feeamt.Split(':');
                                                if (val.Length > 0)
                                                {
                                                    if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                    {
                                                        if (FeeAmountMonthly != "")
                                                        {
                                                            FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                           // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                        }
                                                        else
                                                        {
                                                            FeeAmountMonthly =Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (FeeAmountMonthly == "")
                                                        {


                                                            FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                        }
                                                        else 
                                                        {
                                                            FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                        }

                                                    }
                                                }
                                            }
                                                    //================================
                                                }
                                                else
                                                {
                                                    FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                                }
                                            }
                                            string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                            double feeval = 0;
                                            foreach (string feeamt in feeamtvalue)
                                            {
                                                string[] val = feeamt.Split(':');
                                                if (val.Length > 0)
                                                {
                                                  //  int month = val[0].ToString();
                                                    if (feeval == 0)
                                                    {
                                                        double.TryParse(val[2].ToString(), out feeval);
                                                    }
                                                    else
                                                    {
                                                        double feeadd = 0;
                                                        double.TryParse(val[2].ToString(), out feeadd);
                                                        feeval += feeadd;
                                                    }
                                                }
                                            }
                                            string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + appcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                            if (paidmt.Trim() == "")
                                            {
                                                paidmt = "0";
                                            }
                                            //fee_amount = Convert.ToString(feeval + Convert.ToDouble(fee_amount));
                                            if (Convert.ToDouble(paidmt) <= Convert.ToDouble(feeval))
                                            {
                                                //string fee_allot_query = "if exists (select app_no from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + appcode + "')) update FT_FeeAllot set FeeAmount=" + feeval + ", TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FinYearFK='" + fincyr + "',FeeAmountMonthly='" + FeeAmountMonthly + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + appcode + "') else insert into FT_FeeAllot (AllotDate,LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType, PayMode,FeeAmount, FeeAmountMonthly)values ('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + appcode + "','" + feeval + "','" + feeval + "','1','2','" + feeval + "','" + FeeAmountMonthly + "')";
                                                string fee_allot_query = "if exists (select app_no from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "')) update FT_FeeAllot set FeeAmount=" + feeval + ", TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + appcode + "') else insert into FT_FeeAllot (AllotDate,LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType, PayMode,FeeAmount, FeeAmountMonthly)values ('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + appcode + "','" + feeval + "','" + feeval + "','1','2','" + feeval + "','" + FeeAmountMonthly + "')";
                                                insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                                if (insfeeallot != 0)
                                                {
                                                    double feeallotpk = 0;
                                                    int fnyear = 0;
                                                    double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + appcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                    int.TryParse(d2.GetFunction("select finyearfk from FT_FeeAllot where App_No=" + appcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out fnyear);
                                                    foreach (string feeamt in feeamtvalue)
                                                    {
                                                        string[] val = feeamt.Split(':');
                                                        if (val.Length > 0)
                                                        {
                                                             if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                             {
                                                            //string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + val[0].ToString() + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), FinYearFK=" + fincyr + ",AllotYear=" + val[1].ToString() + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + val[0].ToString() + ", " + val[1].ToString() + ", " + val[2] + ", " + val[2] + ", " + fincyr + ")";
                                                                 string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), AllotYear=" + Convert.ToString(Session["year"]) + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + Convert.ToString(Session["monthvalue"]) + ", " + Convert.ToString(Session["year"]) + ", " + val[2] + ", " + val[2] + ", " + fnyear + ")";
                                                            d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                        }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)
                                        {
                                            feeallotfeestatus = true;
                                        }
                                        //}
                                        #endregion
                                    }
                                    else
                                    {
                                        feeallotfeestatus = true;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (cbHostlerStaff.Checked)
                        {
                            #region Hostler Staff
                            foreach (DictionaryEntry parameter in staffrebateday_hash)
                            {
                                roll = Convert.ToString(parameter.Key);
                                string grantday1 = Convert.ToString(parameter.Value);
                                string Rebateamount = Convert.ToString(staffrebateamount_hash[roll]);
                                //    magesh 14.7.18
                                string Q = "select distinct ht.APP_No, ht.HostelMasterFK,hm.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration ht,HM_HostelMaster hm where ht.HostelMasterFK=hm.HostelMasterPK and ht.MemType='2' and isnull(ht.isdiscontinued,0)=0 and isnull(ht.issuspend,0)=0 and isnull(isvacated,0)=0 and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'  and ht.Messcode in ('" + messcode + "') and ht.APP_No='" + roll + "'";//and HostelMasterFK in('" + hoscode + "') magesh 6.7.18
                                // string Q = "select distinct ht.APP_No, ht.HostelMasterFK,hm.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration ht,HM_HostelMaster hm where ht.HostelMasterFK=hm.HostelMasterPK and ht.MemType='2' and isnull(ht.isdiscontinued,0)=0 and isnull(ht.issuspend,0)=0 and isnull(isvacated,0)=0 and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'  and ht.Messcode in ('" + messcode + "') ";//and HostelMasterFK in('" + hoscode + "') magesh 6.7.18  magesh 14.7.18
                                //string Q = "select distinct ht.APP_No, ht.HostelMasterFK,hm.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration ht,HM_HostelMaster hm where ht.HostelMasterFK=hm.HostelMasterPK and ht.MemType='2' and isnull(ht.isdiscontinued,0)=0 and isnull(ht.issuspend,0)=0 and isnull(isvacated,0)=0 and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and HostelMasterFK in('2','3')" messcode;
                                Q += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                                DataSet HostlerStaffDs = new DataSet();
                                HostlerStaffDs = d2.select_method_wo_parameter(Q, "text");
                                if (HostlerStaffDs.Tables[0].Rows.Count > 0)
                                {
                                    #region Staff generation
                                    foreach (DataRow dr in HostlerStaffDs.Tables[0].Rows)
                                    {
                                        string StaffMessCode = Convert.ToString(dr["HostelMasterFK"]);
                                        //string guestclgcode = Convert.ToString(HostlerStaffDs.Tables[0].Rows[0]["CollegeCode"]);
                                        string StaffMessType = Convert.ToString(dr["StudMessType"]);
                                        string ApplID = Convert.ToString(dr["APP_No"]);

                                        double noofstudentcount = 0;
                                        noofstudentcount = StaffMessType == "1" ? NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;
                                        double studMessTypeAmt = StaffMessType == "1" ? NonvegPerdayAmt + TotalperdayAmt : TotalperdayAmt;
                                        double mandays = TotalCount;
                                        double days1 = days - Convert.ToInt32(grantday1);

                                        string expgroup = "";
                                        string expgroupamt = "";

                                        # region rebate for staff magesh 14.7.18
                                        // if (rdb_commonmess.Checked == true)
                                        //  fixedfinalval = studMessTypeAmt * days;


                                        if (rdb_commonmess.Checked == true)
                                        {
                                            fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount));//(Convert.ToDouble(Session["finalcalvalue"])
                                            fixedfinalval = fixedfinalval * days1;
                                            // fixedfinalval =fixedfinalval-0.2;
                                            #region Expances Calculate
                                            if (expanses_hash.Count > 0)
                                            {
                                                foreach (DictionaryEntry expancesgroup in expanses_hash)
                                                {
                                                    string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                    double examt = Convert.ToDouble(expancesgroup.Value);
                                                    if (expgroup == "")
                                                        expgroup = "" + exgroupcode + "";
                                                    else
                                                        expgroup = expgroup + "," + "" + exgroupcode + "";
                                                    if (expgroupamt == "")
                                                        expgroupamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                    else
                                                        expgroupamt += "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (rdb_indmess.Checked == true)
                                        {
                                            fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * days1;
                                            if (expanses_hash.Count > 0)
                                            {
                                                #region Expances Calculate
                                                foreach (DictionaryEntry expancesgroup in expanses_hash)
                                                {
                                                    string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                    double examt = Convert.ToDouble(expancesgroup.Value);
                                                    double noofstudent = Convert.ToDouble(Session["noofstudent"]);
                                                    double noofpersons = examt / noofstudentcount;
                                                    fixedfinalval = fixedfinalval + noofpersons;
                                                    examt = Convert.ToDouble(examt / noofstudentcount);
                                                    //double noofpersons = examt / Convert.ToDouble(Session["finalvalue"]);
                                                    //double expensecalvalue = noofpersons * days1;
                                                    //fixedfinalval = (fixedfinalval + expensecalvalue);
                                                    //examt = Convert.ToDouble(examt / Convert.ToDouble(Session["finalvalue"]));
                                                    if (expgroup == "")
                                                    {
                                                        expgroup = "" + exgroupcode + "";
                                                    }
                                                    else
                                                    {
                                                        expgroup = expgroup + "," + "" + exgroupcode + "";
                                                    }
                                                    if (expgroupamt == "")
                                                    {
                                                        expgroupamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                    }
                                                    else
                                                    {
                                                        expgroupamt = expgroupamt + "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                //fixedfinalval = Convert.ToDouble(Session["finalcalvalue"]) * days1;
                                                fixedfinalval = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * days1;
                                            }
                                        }

                                        rebateamt1 = (studMessTypeAmt - Convert.ToDouble(Rebateamount)) * Convert.ToInt32(grantday1);
                                        # endregion

                                        string guestexpgrp = string.Empty;
                                        string guestexpamt = string.Empty;
                                        #region Expances Calculate
                                        if (expanses_hash.Count > 0)
                                        {
                                            foreach (DictionaryEntry expancesgroup in expanses_hash)
                                            {
                                                string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                double examt = Convert.ToDouble(expancesgroup.Value);
                                                if (guestexpgrp == "")
                                                    guestexpgrp = "" + exgroupcode + "";
                                                else
                                                    guestexpgrp = guestexpgrp + "," + "" + exgroupcode + "";
                                                if (guestexpamt == "")
                                                    guestexpamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                else
                                                    guestexpamt += "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                            }
                                        }
                                        double ExpancesTotal = StaffMessType == "1" ? NonvegExpanceTotal : VegExpansestotal;
                                        if (ExpancesTotal != 0)
                                        {
                                            double VegAmt = 0;
                                            if (StaffMessType == "1")
                                            {
                                                if (VegExpansestotal != 0)
                                                    VegAmt = VegExpansestotal / TotalNoofStudentstrength;//NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;
                                                VegAmt = Math.Round(VegAmt, 2, MidpointRounding.AwayFromZero);
                                            }
                                            ExpancesTotal = Math.Round((ExpancesTotal / noofstudentcount), 2, MidpointRounding.AwayFromZero);
                                            ExpancesTotal += VegAmt;
                                        }
                                        else
                                            ExpancesTotal = 0;
                                        #endregion
                                        additionalamt1 = 0;
                                        string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + messcode + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "' else insert HT_MessBillMaster (MessMonth,MessYear, MessMasterFK,GroupCode) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + messcode + "','" + guestexpgrp + "')";
                                        int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                        string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "'");
                                        //magesh 18.5.18
                                        //fixedfinalval = Math.Ceiling(fixedfinalval);
                                        //additionalamt1 = Math.Ceiling(additionalamt1);
                                        //rebateamt1 = Math.Ceiling(rebateamt1);
                                        string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + ApplID + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='2')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "', RebateDays='" + days1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "' where app_no='" + ApplID + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='2' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal,RebateDays) values('2','" + ApplID + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "','" + days1 + "')";
                                        int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                        //mandays,StudStrength
                                        //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                        //rebateamt1 = Math.Ceiling(rebateamt1);
                                        //fixedfinalval = Math.Ceiling(fixedfinalval);
                                        string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StaffMessType + "' and MemType='2')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days1 + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + noofstudentcount + "' , MessType='" + StaffMessType + "' , MemType='2'  where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='2' and MessType='" + StaffMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + messcode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days1 + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + noofstudentcount + "','" + StaffMessType + "','2')";//Bill_Type='',total=''
                                        int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                        if (messbilldetails != 0 && messbillmaster != 0)
                                        {
                                            insertflag1 = true;
                                        }
                                        #region Fincance Affected
                                        int FinanceAffected = 0;
                                        if (HostlerStaffDs.Tables[1].Rows.Count > 0)
                                            int.TryParse(Convert.ToString(HostlerStaffDs.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                        if (FinanceAffected == 1)
                                        {
                                            string getsemester = "1";
                                            feeamount1 = fixedfinalval;
                                            fee_amt = feeamount1 + additionalamt1;
                                            //magesh 18.5.18
                                            // fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                            fee_amount = Convert.ToString(Math.Round(fee_amt, 0));
                                            string textcode = string.Empty;
                                            string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                            ds4.Clear();
                                            ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                            if (ds4.Tables[0].Rows.Count > 0)
                                            {
                                                string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                                if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                                {
                                                    if (linkvalue == "0")
                                                    {
                                                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                        ds4.Clear();
                                                        ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                        if (ds4.Tables[0].Rows.Count > 0)
                                                        {
                                                            textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                            Session["fee_category"] = Convert.ToString(textcode);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                        {
                                                            getsemester = "1 Year";
                                                        }
                                                        else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                        {
                                                            getsemester = "2 Year";
                                                        }
                                                        else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                        {
                                                            getsemester = "3 Year";
                                                        }
                                                        else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                        {
                                                            getsemester = "4 Year";
                                                        }
                                                        else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                        {
                                                            getsemester = "5 Year";
                                                        }
                                                        else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                        {
                                                            getsemester = "6 Year";
                                                        }
                                                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                        ds4.Clear();
                                                        ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                        if (ds4.Tables[0].Rows.Count > 0)
                                                        {
                                                            textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                            Session["fee_category"] = Convert.ToString(textcode);
                                                        }
                                                    }
                                                }
                                            }
                                            int insexcessdet = 0;
                                            int insadjustinclude = 0;
                                            int insExcessLedger = 0;
                                            int insfeeallot = 0;
                                            string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + StaffMessCode + "'";//and CollegeCode='" + collegecode1 + "'";
                                            ds7.Clear();
                                            ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                            string messheader = "";
                                            string messledger = "";
                                            if (ds7.Tables[0].Rows.Count > 0)
                                            {
                                                messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                                messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                            }
                                            string FeeAmountMonthly = "";
                                            string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + ApplID + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='1'");
                                            if (previousmonthfee.Trim() == "0")
                                            {
                                                FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                            }
                                            else
                                            {
                                                if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                                {
                                                   // FeeAmountMonthly = previousmonthfee;
                                                    string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                    Hashtable hs = new Hashtable();

                                                    double feeval1 = 0;
                                                    FeeAmountMonthly = "";
                                                    foreach (string feeamt in feeamtvalue1)
                                                    {
                                                        string[] val = feeamt.Split(':');
                                                        if (val.Length > 0)
                                                        {
                                                            if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                            {
                                                                if (FeeAmountMonthly != "")
                                                                {
                                                                    FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                    // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                                }
                                                                else
                                                                {
                                                                    FeeAmountMonthly = Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (FeeAmountMonthly == "")
                                                                {


                                                                    FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                                }
                                                                else
                                                                {
                                                                    FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                                }
                                            }
                                            string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                            double feeval = 0;
                                            foreach (string feeamt in feeamtvalue)
                                            {
                                                string[] val = feeamt.Split(':');
                                                if (val.Length > 0)
                                                {
                                                    if (feeval == 0)
                                                    {
                                                        double.TryParse(val[2].ToString(), out feeval);
                                                    }
                                                    else
                                                    {
                                                        double feeadd = 0;
                                                        double.TryParse(val[2].ToString(), out feeadd);
                                                        feeval += feeadd;
                                                    }
                                                }
                                            }
                                            string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + appcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                            if (paidmt.Trim() == "")
                                            {
                                                paidmt = "0";
                                            }
                                            if (textcode.Trim() != "")
                                            {
                                                //string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2') update FT_FeeAllot set FeeAmount=FeeAmount+" + fee_amount + ", TotalAmount=TotalAmount+'" + fee_amount + "',BalAmount=BalAmount+'" + fee_amount + "',LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FinYearFK='" + fincyr + "',App_No='" + ApplID + "' where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + ApplID + "') and memtype='2' else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + ApplID + "','" + fee_amount + "','" + fee_amount + "','2','1','" + fee_amount + "')";

                                              
                                                string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and App_No in('" + ApplID + "') and memtype='2') update FT_FeeAllot set FeeAmount='" + feeval + "', TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "',App_No='" + ApplID + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + ApplID + "') and memtype='2' else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount,FeeAmountMonthly)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + ApplID + "','" + fee_amount + "','" + fee_amount + "','2','1','" + fee_amount + "','" + FeeAmountMonthly + "')";
                                                insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                                if (insfeeallot != 0)
                                                {
                                                    double feeallotpk = 0;
                                                    double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + ApplID + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                    foreach (string feeamt in feeamtvalue)
                                                    {
                                                        string[] val = feeamt.Split(':');
                                                        if (val.Length > 0)
                                                        {
                                                            string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + val[0].ToString() + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), FinYearFK=" + fincyr + ",AllotYear=" + val[1].ToString() + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + val[0].ToString() + ", " + val[1].ToString() + ", " + val[2] + ", " + val[2] + ", " + fincyr + ")";
                                                            d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                            if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                            {
                                                feeallotfeestatus1 = true;
                                            }
                                        }
                                        else { feeallotfeestatus1 = true; }
                                        #endregion
                                    }
                                    #endregion
                                }
                            #endregion
                            }
                        }
                        string guestcode = string.Empty;
                        if (guestgrant_hash.Count > 0)
                        {
                            #region guest generation
                            foreach (DictionaryEntry parameter in guestgrant_hash)
                            {
                                guestcode = Convert.ToString(parameter.Key);
                                string grantday1 = Convert.ToString(parameter.Value);
                                string guestrebateamt = Convert.ToString(guestRebateamount_hash[guestcode]);
                                string q3 = "select distinct gr.APP_No, gr.HostelMasterFK,hd.CollegeCode,isnull(StudMessType,0)StudMessType from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and MemType='3' and APP_No in('" + guestcode + "') and gr.Messcode in ('" + messcode + "') ";
                                q3 += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                                ds6.Clear();
                                ds6 = d2.select_method_wo_parameter(q3, "Text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {
                                    string guesthostelcode = Convert.ToString(ds6.Tables[0].Rows[0]["HostelMasterFK"]);
                                    string guestclgcode = Convert.ToString(ds6.Tables[0].Rows[0]["CollegeCode"]);
                                    string StudentMessType = Convert.ToString(ds6.Tables[0].Rows[0]["StudMessType"]);
                                    double days1 = days - Convert.ToInt32(grantday1);
                                    string guestexpgrp = "";
                                    string guestexpamt = "";
                                    double noofstudentcount = 0;
                                    //double mandays = (StudentMessType == "1") ? NonvegCount + TotalCount : TotalCount;
                                    noofstudentcount = StudentMessType == "1" ? NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;
                                    double studMessTypeAmt = StudentMessType == "1" ? NonvegPerdayAmt + TotalperdayAmt : TotalperdayAmt;
                                    double mandays = TotalCount;
                                    //noofstudentcount = TotalNoofStudentstrength;
                                    if (rdb_commonmess.Checked == true)
                                    {
                                        fixedfinalval = (studMessTypeAmt - Convert.ToDouble(guestrebateamt)) * days1;
                                        #region Expances Calculate
                                        if (expanses_hash.Count > 0)
                                        {
                                            foreach (DictionaryEntry expancesgroup in expanses_hash)
                                            {
                                                string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                double examt = Convert.ToDouble(expancesgroup.Value);
                                                if (guestexpgrp == "")
                                                    guestexpgrp = "" + exgroupcode + "";
                                                else
                                                    guestexpgrp = guestexpgrp + "," + "" + exgroupcode + "";
                                                if (guestexpamt == "")
                                                    guestexpamt = "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                                else
                                                    guestexpamt += "," + "" + Convert.ToString(Math.Round(examt, 2)) + "";
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (rdb_indmess.Checked == true)
                                    {
                                        fixedfinalval = (studMessTypeAmt - Convert.ToDouble(guestrebateamt)) * days1;
                                        if (expanses_hash.Count > 0)
                                        {
                                            foreach (DictionaryEntry expancesgroup in expanses_hash)
                                            {
                                                string exgroupcode = Convert.ToString(expancesgroup.Key);
                                                double examt = Convert.ToDouble(expancesgroup.Value);
                                                double noofstudent = Convert.ToDouble(Session["noofstudent"]);
                                                double noofpersons = examt / noofstudentcount;
                                                fixedfinalval = fixedfinalval + noofpersons;
                                                //double noofpersons = examt / Convert.ToDouble(Session["finalvalue"]);
                                                //double guestvalue = noofpersons * days1;
                                                //fixedfinalval = (fixedfinalval + guestvalue);
                                                examt = Convert.ToDouble(examt / noofstudentcount);
                                                examt = Convert.ToDouble(Math.Round(examt, 2));
                                                if (guestexpgrp == "")
                                                    guestexpgrp = "" + exgroupcode + "";
                                                else
                                                    guestexpgrp = guestexpgrp + "," + "" + exgroupcode + "";
                                                if (guestexpamt == "")
                                                    guestexpamt = "" + examt + "";
                                                else
                                                    guestexpamt = guestexpamt + "," + "" + examt + "";
                                            }
                                        }
                                    }
                                    rebateamt1 = (studMessTypeAmt - Convert.ToDouble(guestrebateamt)) * Convert.ToInt32(grantday1);
                                    if (cb_stuadd.Checked == true)
                                    {
                                        string add_amount = d2.GetFunction("select SUM(AdditionalAmt)as Add_Amount from HT_StudAdditionalDet where App_No = '" + guestcode + "' and TransDate BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and MemType=3");
                                        if (add_amount.Trim() != "" && add_amount.Trim() != "0")
                                        {
                                            additionalamt1 = Convert.ToDouble(add_amount);
                                        }
                                        else
                                        {
                                            additionalamt1 = 0;
                                        }
                                    }
                                    double ExpancesTotal = StudentMessType == "1" ? NonvegExpanceTotal : VegExpansestotal;
                                    if (ExpancesTotal != 0)
                                    {
                                        double VegAmt = 0;
                                        if (StudentMessType == "1")
                                        {
                                            if (VegExpansestotal != 0)
                                                VegAmt = VegExpansestotal / TotalNoofStudentstrength;//NonvegTotalNoofStudentstrength : TotalNoofStudentstrength;
                                            VegAmt = Math.Round(VegAmt, 2, MidpointRounding.AwayFromZero);
                                        }
                                        ExpancesTotal = Math.Round((ExpancesTotal / noofstudentcount), 2, MidpointRounding.AwayFromZero);
                                        ExpancesTotal += VegAmt;
                                    }
                                    else
                                        ExpancesTotal = 0;
                                    string insertmessbillmaster = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + guesthostelcode + "')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + messcode + "',GroupCode='" + guestexpgrp + "' where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "' else insert HT_MessBillMaster (MessMonth,MessYear, MessMasterFK,GroupCode) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + messcode + "','" + guestexpgrp + "')";
                                    int messbillmaster = d2.update_method_wo_parameter(insertmessbillmaster, "Text");
                                    string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "'");
                                    //magesh 18.5.18
                                    //fixedfinalval = Math.Ceiling(fixedfinalval);
                                    //additionalamt1 = Math.Ceiling(additionalamt1);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    string insertmessbilldetails = "if exists(select*from HT_MessBillDetail where app_no='" + guestcode + "' and MessBillMasterFK='" + messbill_masterpk1 + "' and MemType='3')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "',GroupAmount='" + guestexpamt + "',ExpanceGroupCode='" + guestexpgrp + "',ExpanceGroupAmtTotal='" + ExpancesTotal + "',RebateDays='" + days1 + "' where app_no='" + guestcode + "' and messbillmasterfk='" + messbill_masterpk1 + "'  and MemType='3' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK,GroupAmount,ExpanceGroupCode,ExpanceGroupAmtTotal,RebateDays) values('3','" + guestcode + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "','" + guestexpamt + "','" + guestexpgrp + "','" + ExpancesTotal + "','" + days1 + "')";
                                    int messbilldetails = d2.update_method_wo_parameter(insertmessbilldetails, "Text");
                                    //mandays,StudStrength
                                    //studMessTypeAmt = Math.Ceiling(studMessTypeAmt);
                                    //rebateamt1 = Math.Ceiling(rebateamt1);
                                    //fixedfinalval = Math.Ceiling(fixedfinalval);
                                    string dividingdetails = "if exists(select*from HMessbill_StudDetails where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MessType='" + StudentMessType + "' and MemType='3')update HMessbill_StudDetails set No_Of_Days='" + days + "', rebate_days='" + days1 + "', Per_Day_Amount='" + studMessTypeAmt + "', rebate_amount='" + rebateamt1 + "' , mess_amount='" + fixedfinalval + "',incgroupcode='" + Convert.ToString(Session["incgroup"]) + "',expgroupcode='" + guestexpgrp + "',mandays='" + mandays + "',StudStrength='" + noofstudentcount + "' , MessType='" + StudentMessType + "' , MemType='3'  where Hostel_Code='" + messcode + "' and MessBill_Month='" + Convert.ToString(Session["monthvalue"]) + "' and MessBill_Year='" + Convert.ToString(Session["year"]) + "' and MemType='3' and MessType='" + StudentMessType + "' else insert into HMessbill_StudDetails (Hostel_Code,MessBill_Month,MessBill_Year,No_Of_Days, rebate_days,Per_Day_Amount,rebate_amount,mess_amount, Hreg_code,Extras,guest,Total,inmatetype,incgroupcode,expgroupcode,mandays,StudStrength,MessType,MemType) values ('" + messcode + "','" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + days + "','" + days1 + "','" + studMessTypeAmt + "','" + rebateamt1 + "','" + fixedfinalval + "','0','0','0','0','0','" + Convert.ToString(Session["incgroup"]) + "','" + guestexpgrp + "','" + mandays + "','" + noofstudentcount + "','" + StudentMessType + "','3')";//Bill_Type='',total=''
                                    int insert2 = d2.update_method_wo_parameter(dividingdetails, "Text");
                                    if (messbilldetails != 0 && messbillmaster != 0)
                                    {
                                        insertflag1 = true;
                                    }
                                    #region Fincance Affected
                                    int FinanceAffected = 0;
                                    if (ds6.Tables[1].Rows.Count > 0)
                                        int.TryParse(Convert.ToString(ds6.Tables[1].Rows[0]["value"]), out FinanceAffected);
                                    if (FinanceAffected == 1)
                                    {
                                        string getsemester = d2.GetFunction("select Current_Semester  from Registration where app_no ='" + guestcode + "'");
                                        if (getsemester.Trim() == "" || getsemester.Trim() == null || getsemester.Trim() == "0")
                                        {
                                            getsemester = "1";
                                        }
                                        feeamount1 = fixedfinalval;
                                        fee_amt = feeamount1 + additionalamt1;
                                        //magesh 18 .5.18
                                       // fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                        fee_amount = Convert.ToString(Math.Round(fee_amt, 0));
                                        string textcode = "";
                                        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "'";// and college_code ='" + guestclgcode + "'";
                                        // settingquery = settingquery + "   select Degree_code,FeeCategory,t.TextVal  from Fee_degree_match f,textvaltable t where f.FeeCategory=t.TextCode and f.College_code=t.college_code and f.college_code ='" + Convert.ToString(guestclgcode) + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(settingquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                            if (linkvalue.Trim() == "0" || linkvalue.Trim() == "1")
                                            {
                                                if (linkvalue == "0")
                                                {
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                                else
                                                {
                                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                                    {
                                                        getsemester = "1 Year";
                                                    }
                                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                                    {
                                                        getsemester = "2 Year";
                                                    }
                                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                                    {
                                                        getsemester = "3 Year";
                                                    }
                                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                                    {
                                                        getsemester = "4 Year";
                                                    }
                                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                                    {
                                                        getsemester = "5 Year";
                                                    }
                                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                                    {
                                                        getsemester = "6 Year";
                                                    }
                                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";//Year
                                                    ds4.Clear();
                                                    ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                                    if (ds4.Tables[0].Rows.Count > 0)
                                                    {
                                                        textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                                                        Session["fee_category"] = Convert.ToString(textcode);
                                                    }
                                                }
                                            }
                                            //if (linkvalue.Trim() == "2")
                                            //{
                                            //    //ds.Tables[4].DefaultView.RowFilter = "degree_code='" + degreecode + "'";
                                            //    //DataView dvsem1 = ds.Tables[4].DefaultView;
                                            //    //if (dvsem1.Count > 0)
                                            //    //{
                                            //    //}
                                            //}
                                        }
                                        int insexcessdet = 0;
                                        int insadjustinclude = 0;
                                        int insExcessLedger = 0;
                                        int insfeeallot = 0;
                                        string headledger = "select MessBillHeaderFK,NessBukkLedgerFK from HM_HostelMaster where HostelMasterPK='" + guesthostelcode + "'";//and CollegeCode='" + collegecode1 + "'";
                                        ds7.Clear();
                                        ds7 = d2.select_method_wo_parameter(headledger, "Text");
                                        string messheader = "";
                                        string messledger = "";
                                        if (ds7.Tables[0].Rows.Count > 0)
                                        {
                                            messheader = Convert.ToString(ds7.Tables[0].Rows[0]["MessBillHeaderFK"]);
                                            messledger = Convert.ToString(ds7.Tables[0].Rows[0]["NessBukkLedgerFK"]);
                                        }


                                        string FeeAmountMonthly = "";
                                        string previousmonthfee = d2.GetFunction(" select FeeAmountMonthly  from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + guestcode + "') and FeeAmountMonthly is not null and FeeAmountMonthly <>'' and paymode='1'");
                                        if (previousmonthfee.Trim() == "0")
                                        {
                                            FeeAmountMonthly = "" + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                        }
                                        else
                                        {
                                            if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                            {
                                                //FeeAmountMonthly = previousmonthfee;
                                                string[] feeamtvalue1 = previousmonthfee.Split(',');
                                                Hashtable hs = new Hashtable();

                                                double feeval1 = 0;
                                                FeeAmountMonthly = "";
                                                foreach (string feeamt in feeamtvalue1)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        if (Convert.ToString(val[0]) == Convert.ToString(Session["monthvalue"]) && Convert.ToString(val[1]) == Convert.ToString(Session["year"]))
                                                        {
                                                            if (FeeAmountMonthly != "")
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                                // hs.Add(Convert.ToString(Session["monthvalue"]), Convert.ToString(Session["year"]));
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString((Session["year"])) + ":" + (fee_amount);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (FeeAmountMonthly == "")
                                                            {


                                                                FeeAmountMonthly = Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }
                                                            else
                                                            {
                                                                FeeAmountMonthly = FeeAmountMonthly + "," + Convert.ToString(val[0]) + ":" + Convert.ToString(val[1]) + ":" + Convert.ToString(val[2]);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                FeeAmountMonthly = previousmonthfee + "," + Convert.ToString(Session["monthvalue"]) + ":" + Convert.ToString(Session["year"]) + ":" + fee_amount + "";
                                            }
                                        }
                                        string[] feeamtvalue = FeeAmountMonthly.Split(',');
                                        double feeval = 0;
                                        foreach (string feeamt in feeamtvalue)
                                        {
                                            string[] val = feeamt.Split(':');
                                            if (val.Length > 0)
                                            {
                                                if (feeval == 0)
                                                {
                                                    double.TryParse(val[2].ToString(), out feeval);
                                                }
                                                else
                                                {
                                                    double feeadd = 0;
                                                    double.TryParse(val[2].ToString(), out feeadd);
                                                    feeval += feeadd;
                                                }
                                            }
                                        }
                                        string paidmt = d2.GetFunction("select PaidAmount from FT_FeeAllot where App_No='" + guestcode + "' and HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and FeeCategory='" + textcode + "'");
                                        if (paidmt.Trim() == "")
                                        {
                                            paidmt = "0";
                                        }
                                        string transcode = generateReceiptNo();
                                        if (textcode.Trim() != "")
                                        {
                                            //if (cb_adj_exe.Checked == true)
                                            //{
                                            //    string adjustinclude = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK,IsExcessAdj,ExcessAdjAmt) values('" + dtaccessdate + "','" + dtaccesstime + "','" + transcode + "','1','" + guestcode + "','" + messheader + "','" + messledger + "','" + textcode + "','0','" + fee_amount + "','1','1','" + fincyr + "','1','" + fee_amount + "')";
                                            //    insadjustinclude = d2.update_method_wo_parameter(adjustinclude, "Text");
                                            //    insexcessdet = 0;
                                            //    if (insadjustinclude != 0)
                                            //    {
                                            //        string excessdet = "if exists ( select * from ft_excessdet where app_no = '" + guestcode + "') update FT_ExcessDet set AdjAmt = AdjAmt + '" + fee_amount + "',BalanceAmt = (BalanceAmt - isnull('" + fee_amount + "',0)) where App_No = '" + guestcode + "' and MemType = 1 else  insert into ft_excessdet (ExcessTransDate,TransTime,DailyTransCode,App_No ,MemType,ExcessType,ExcessAmt, AdjAmt,BalanceAmt,FinYearFK) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + transcode + "','" + guestcode + "','1','1','" + fee_amount + "','0','" + fee_amount + "','" + fincyr + "')";
                                            //        insexcessdet = d2.update_method_wo_parameter(excessdet, "Text");
                                            //    }
                                            //    if (insexcessdet != 0)
                                            //    {
                                            //        string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + guestcode + "'");
                                            //        string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt + '" + fee_amount + "',BalanceAmt =BalanceAmt - '" + fee_amount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,AdjAmt,BalanceAmt) values ( '" + messheader + "','" + messledger + "','" + fee_amount + "','0','" + fee_amount + "')";
                                            //        insExcessLedger = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                            //    }
                                            //}
                                            //else
                                            //{

                                           

                                            string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "')  and App_No in('" + guestcode + "')) update FT_FeeAllot set FeeAmount='" + feeval + "', TotalAmount='" + feeval + "',BalAmount='" + feeval + "'-isnull(Paidamount,0),LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FeeAmountMonthly='" + FeeAmountMonthly + "',App_No='" + guestcode + "' where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and App_No in('" + guestcode + "') else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType,PayMode,FeeAmount,FeeAmountMonthly)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + guestcode + "','" + fee_amount + "','" + fee_amount + "','3','1','" + fee_amount + "','" + FeeAmountMonthly + "')";
                                            insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                            if (insfeeallot != 0)
                                            {
                                                double feeallotpk = 0;
                                                double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + guestcode + " and LedgerFK='" + messledger + "' and HeaderFK='" + messheader + "' and FeeCategory=" + textcode + "").Trim(), out feeallotpk);
                                                foreach (string feeamt in feeamtvalue)
                                                {
                                                    string[] val = feeamt.Split(':');
                                                    if (val.Length > 0)
                                                    {
                                                        string feeallotmonthly = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + val[0].ToString() + " ) update ft_feeallotmonthly set  AllotAmount= " + val[2] + ", BalAmount=(" + val[2] + "-isnull(PaidAMount,0)), FinYearFK=" + fincyr + ",AllotYear=" + val[1].ToString() + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + Convert.ToString(Session["monthvalue"]) + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + val[0].ToString() + ", " + val[1].ToString() + ", " + val[2] + ", " + val[2] + ", " + fincyr + ")";
                                                        d2.update_method_wo_parameter(feeallotmonthly, "Text");
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                        if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)//&& insadjustinclude != 0
                                        {
                                            feeallotfeestatus1 = true;
                                        }
                                    }
                                    else { feeallotfeestatus1 = true; }
                                    #endregion
                                }
                                //else
                                //{
                                //    lblalerterr.Visible = true;
                                //    alertpopwindow.Visible = true;
                                //    lblalerterr.Text = "Please Update Guest Details";
                                //}
                            }
                            #endregion
                        }
                        if (cb_guest.Checked == true && cb_hosteler.Checked == true)
                        {
                            if (hostel_bool == true && guest_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Hostel Student";
                            }
                        }
                        else if (cb_guest.Checked == true)
                        {
                            if (guest_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Guest Details";
                            }
                        }
                        else if (cb_hosteler.Checked == true)
                        {
                            if (hostel_bool == true)
                            {
                                lblalerterr.Visible = true;
                                alertpopwindow.Visible = true;
                                Div2.Visible = false;
                                lblalerterr.Text = "Please Update Hostel Student Details";
                            }
                        }
                        if (insertflag == true && feeallotfeestatus == true || insertflag1 == true || feeallotfeestatus1 == true)
                        {
                            lblalerterr.Visible = true;
                            alertpopwindow.Visible = true;
                            Div2.Visible = false;
                            lblalerterr.Text = "Saved Successfully";
                            btn_go_Click(sender, e);
                        }
                    }
                }
            }
 #endregion


            #region Non divident
            if (Convert.ToString(ViewState["isdivident"]) == "Non")
            {
                string Financialyear = d2.GetFunction("select LinkValue  from InsSettings where LinkName = 'Current Financial Year'");
                int fincyr = Convert.ToInt32(Financialyear);
                int insexcessdet = 0;
                int insadjustinclude = 0;
                int insExcessLedger = 0;
                int insfeeallot = 0;
                hoscode = Convert.ToString(Session["hoscode"]);
                string rabat = " select HostelAdmFeeAmount as Mess_FixedFeeAmt, IncludeRebate as IncludeRebate from HM_HostelMaster where HostelMasterPK  in('" + hoscode + "')";
                ds2 = d2.select_method_wo_parameter(rabat, "Text");
                string fixfee1 = "";
                string isrebate1 = "";
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    fixfee1 = Convert.ToString(ds2.Tables[0].Rows[0]["Mess_FixedFeeAmt"]);
                    isrebate1 = Convert.ToString(ds2.Tables[0].Rows[0]["IncludeRebate"]);
                }
                if (isrebate1.Trim() == "True")//isrebate1.Trim() != "" && isrebate1.Trim() == "null" &&
                {
                    isbate = true;
                }
                else
                {
                    isbate = false;
                }
                if (isbate == false || isbate == true)
                {
                    hoscode = Convert.ToString(Session["hoscode"]);
                    clgcode = Convert.ToString(Session["clgcode"]);
                    messcode = Convert.ToString(Session["messcode"]);
                    double messfee1 = Convert.ToDouble(txt_messfess.Text);
                    string messtype = "";
                    if (rdo_day.Checked == true)
                    {
                        messfee = days * messfee1;
                        messtype = "0";
                    }
                    else if (rdo_month.Checked == true)
                    {
                        messfee = messfee1;
                        messtype = "1";
                    }
                    string rebatetype = "";
                    if (isrebate1.Trim() != "" && isrebate1.Trim() == "null" && isrebate1.Trim() == "1")
                    {
                        rebatetype = isrebate1;
                    }
                    else
                    {
                        rebatetype = "0";
                    }
                    #region Hostel Mess Settings
                    m = Convert.ToString(Session["monthvalue"]);
                    string q1 = "if exists(select * from HostelMessSettings where Hostel_Code= '" + messcode + "' and college_code='" + clgcode + "' and MessType='" + messtype + "' and messmonth='" + m + "') update HostelMessSettings set  RebateType='" + rebatetype + "',MessType='" + messtype + "',messAmt='" + messfee1 + "',messmonth='" + m + "',messyear='" + messy + "' where Hostel_Code ='" + messcode + "'  else   insert into HostelMessSettings (College_Code,Hostel_Code,MessType,messAmt,messmonth,messyear, RebateType) values ('" + clgcode + "','" + messcode + "','" + messtype + "','" + messfee1 + "','" + m + "','" + messy + "','" + rebatetype + "')";
                    int ins = d2.update_method_wo_parameter(q1, "Text");
                    #endregion
                    string roll1 = "";
                    string q2 = "select r.app_no,h.HostelMasterFK,r.college_code from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and h.HostelMasterFK in('" + hoscode + "') and MemType='1'";
                    q2 += " select value from Master_Settings where settings='Mess Bill Include in Finance' " + groupUsercode + "";
                    ds6.Clear();
                    ds6 = d2.select_method_wo_parameter(q2, "Text");
                    //hosteler
                    if (ds6.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds6.Tables[0].Rows.Count; j++)
                        {
                            int days1 = 0;
                            roll1 = Convert.ToString(ds6.Tables[0].Rows[j]["app_no"]);
                            string hostelcode3 = Convert.ToString(ds6.Tables[0].Rows[j]["HostelMasterFK"]);
                            string collegecode3 = Convert.ToString(ds6.Tables[0].Rows[j]["college_code"]);
                            #region Rebate Check
                            if (isbate == true)
                            {
                                if (rdb_indivula.Checked == true)
                                {
                                    string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode3 + "') and RebateType='1' and RebateMonth between '" + dt.ToString("MM") + "' and '" + dt1.ToString("MM") + "'";
                                    ds1.Clear();
                                    ds1 = d2.select_method_wo_parameter(q7, "Text");
                                    if (ds1.Tables[0].Rows.Count > 0)
                                    {
                                        rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                        rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                        if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                        {
                                            rebate_days1 = Convert.ToDouble(rebate_days);
                                            rebateamt1 = rebate_days1 * messfee1;
                                            messfee = messfee - rebateamt1;
                                        }
                                        if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                        {
                                            rebateamt1 = Convert.ToDouble(rebate_Amount);
                                            // fixedfinalval = fixedfinalval - rebateamt1;
                                        }
                                        // additionalamt = d2.GetFunction("select SUM(Add_Amount)as Add_Amount from StudentAdditional_Details where Roll_No = '" + roll + "' and Entry_Date BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "'");
                                    }
                                }
                                else if (rdb_common.Checked == true)
                                {
                                    int count = 0;
                                    DateTime dt2 = new DateTime();
                                    dt2 = dt;
                                    while (dt2 <= dt1)
                                    {
                                        int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                        int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                        string attend = "[d" + fdate + "]";
                                        string q8 = " SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + roll1 + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";//fromdate month and todate month
                                        ds2.Clear();
                                        ds2 = d2.select_method_wo_parameter(q8, "Text");
                                        if (ds2.Tables[0].Rows.Count > 0)
                                        {
                                            count += 1;
                                        }
                                        else
                                        {
                                            if (count != 0)
                                            {
                                                // countdays.Add(count);
                                                string cnt = Convert.ToString(countdays);//[r]
                                                string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode3 + "')and RebateType='1' and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";
                                                ds3 = d2.select_method_wo_parameter(q9, "Text");
                                                if (ds3.Tables[0].Rows.Count > 0)
                                                {
                                                    string grant_day = Convert.ToString(ds3.Tables[0].Rows[0][0]);
                                                    string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0][1]);
                                                    if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                                    {
                                                        rebate_days1 = Convert.ToDouble(grant_day);
                                                        rebateamt1 = rebate_days1 * messfee1;
                                                        // fixedfinalval = fixedfinalval - rebateamt1;
                                                    }
                                                    if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                                    {
                                                        rebateamt1 = Convert.ToDouble(grant_amt);
                                                        //fixedfinalval = fixedfinalval - rebateamt1;
                                                    }
                                                }
                                            }
                                            count = 0;
                                        }
                                        dt2 = dt2.AddDays(1);
                                    }
                                }
                            }
                            days1 = days - Convert.ToInt32(rebate_days1);
                            #endregion
                            #region Insert Mess Bill
                            fixedfinalval = messfee;
                            string regenfeeamount = d2.GetFunction("select messamount+messadditonalamt as regenamt from HT_MessBillDetail where app_no='" + roll1 + "'");
                            string insmessbillmaster1 = "if exists(select*from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='')update HT_MessBillMaster set MessMonth='" + Convert.ToString(Session["monthvalue"]) + "',MessYear='" + Convert.ToString(Session["year"]) + "',MessMasterFK='" + messcode + "' where MessMasterFK='" + messcode + "'  else insert HT_MessBillMaster (MessMonth,MessYear, MessMasterFK) values('" + Convert.ToString(Session["monthvalue"]) + "','" + Convert.ToString(Session["year"]) + "','" + messcode + "')";
                            int insert11 = d2.update_method_wo_parameter(insmessbillmaster1, "Text");
                            string messbill_masterpk1 = d2.GetFunction(" select MessBillMasterPK from HT_MessBillMaster where MessMonth='" + Convert.ToString(Session["monthvalue"]) + "' and MessYear='" + Convert.ToString(Session["year"]) + "' and MessMasterFK='" + messcode + "'");
                            string insmessbilldetails1 = "if exists(select*from HT_MessBillDetail where app_no='" + roll1 + "' and MessBillMasterFK='" + messbill_masterpk1 + "')update ht_messbilldetail set messamount='" + fixedfinalval + "', MessAdditonalAmt='" + additionalamt1 + "',rebateamount='" + rebateamt1 + "' where app_no='" + roll1 + "' and messbillmasterfk='" + messbill_masterpk1 + "' else insert into ht_messbilldetail (MemType,App_No,MessAmount,MessAdditonalAmt,RebateAmount,MessBillMasterFK) values('3','" + roll1 + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + messbill_masterpk1 + "')";
                            int insert111 = d2.update_method_wo_parameter(insmessbilldetails1, "Text");
                            if (insert11 != 0)//&& ins1 != 0
                            {
                                insertflag = true;
                            }
                            #endregion
                            int FinanceAffected = 0;
                            if (ds6.Tables[1].Rows.Count > 0)
                                int.TryParse(Convert.ToString(ds6.Tables[1].Rows[0]["value"]), out FinanceAffected);
                            if (FinanceAffected == 1)
                            {
                                #region Feecatagory
                                string getsemester1 = d2.GetFunction("select Current_Semester from Registration where app_no ='" + roll1 + "'");
                                string textcode = "";
                                string settingquery1 = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode3 + "'";
                                ds4.Clear();
                                ds4 = d2.select_method_wo_parameter(settingquery1, "Text");
                                if (ds4.Tables[0].Rows.Count > 0)
                                {
                                    string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                                    if (linkvalue == "0")
                                    {
                                        string semesterquery = " select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='Fee category' and MasterValue='" + getsemester1.Trim() + " Semester'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            textcode = Convert.ToString(ds4.Tables[0].Rows[0]["MasterCode"]);
                                            Session["fee_category"] = Convert.ToString(textcode);
                                        }
                                    }
                                    else
                                    {
                                        if (getsemester1.Trim() == "1" || getsemester1.Trim() == "2")
                                        {
                                            getsemester1 = "1 Year";
                                        }
                                        else if (getsemester1.Trim() == "3" || getsemester1.Trim() == "4")
                                        {
                                            getsemester1 = "2 Year";
                                        }
                                        else if (getsemester1.Trim() == "5" || getsemester1.Trim() == "6")
                                        {
                                            getsemester1 = "3 Year";
                                        }
                                        else if (getsemester1.Trim() == "7" || getsemester1.Trim() == "8")
                                        {
                                            getsemester1 = "4 Year";
                                        }
                                        else if (getsemester1.Trim() == "9" || getsemester1.Trim() == "10")
                                        {
                                            getsemester1 = "5 Year";
                                        }
                                        else if (getsemester1.Trim() == "11" || getsemester1.Trim() == "12")
                                        {
                                            getsemester1 = "6 Year";
                                        }
                                        string semesterquery = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='Fee category' and MasterValue='" + getsemester1.Trim() + "'";
                                        ds4.Clear();
                                        ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                                        if (ds4.Tables[0].Rows.Count > 0)
                                        {
                                            textcode = Convert.ToString(ds4.Tables[0].Rows[0]["MasterCode"]);
                                            Session["fee_category"] = Convert.ToString(textcode);
                                        }
                                    }
                                }
                                #endregion
                                #region Finance Affected Query
                                feeamount1 = fixedfinalval - rebateamt1;
                                fee_amt = feeamount1 + additionalamt1;
                                fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                                string messledger = d2.GetFunction("select Mess_FeeCode from HM_HostelMaster where Hostel_code = '" + hostelcode3 + "'");
                                string messheader = d2.GetFunction("select distinct h.HeaderPK from FM_LedgerMaster l,FM_HeaderMaster h where l.LedgerPK='" + messledger + "' and l.HeaderfK=h.HeaderPK");
                                //string transcode = generateReceiptNo();
                                if (cb_adj_exe.Checked == true)
                                {
                                    if (Convert.ToString(ViewState["Regenerate"]) == "Regenerate")
                                    {
                                        string regenexcess = "if exists (select * from ft_excessdet where app_no = '" + roll1 + "') update ft_excessdet set AdjAmt=AdjAmt-'" + regenfeeamount + "',BalanceAmt=BalanceAmt+'" + regenfeeamount + "' where App_No = '" + roll1 + "' and MemType = 1 ";
                                        int regen = d2.update_method_wo_parameter(regenexcess, "Text");
                                        string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + roll1 + "'");
                                        string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt - '" + regenfeeamount + "',BalanceAmt =BalanceAmt + '" + regenfeeamount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "'";
                                        int regen1 = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                    }
                                    string excessdet = "if exists ( select * from ft_excessdet where app_no = '" + roll1 + "') update FT_ExcessDet set AdjAmt = AdjAmt + '" + fee_amount + "',BalanceAmt = (BalanceAmt - isnull('" + fee_amount + "',0))  where App_No = '" + roll1 + "' and MemType = 1 else  insert into ft_excessdet (ExcessTransDate,TransTime,DailyTransCode,App_No ,MemType,ExcessType,ExcessAmt, AdjAmt,BalanceAmt,FinYearFK) values ('" + dtaccessdate + "','" + dtaccesstime + "','','" + roll1 + "','1','1','0','" + fee_amount + "','0','" + fincyr + "')";
                                    insexcessdet = d2.update_method_wo_parameter(excessdet, "Text");
                                    if (insexcessdet != 0)
                                    {
                                        string excessdepk = d2.GetFunction("select ExcessDetPK  from  ft_excessdet where App_No ='" + roll1 + "'");
                                        string ExcessLedger = "if exists ( select * from FT_ExcessLedgerDet where HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "') update FT_ExcessLedgerDet set AdjAmt =AdjAmt + '" + fee_amount + "',BalanceAmt =BalanceAmt - '" + fee_amount + "'  where  HeaderFK='" + messheader + "' and LedgerFK='" + messledger + "' and ExcessDetFK='" + excessdepk + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,AdjAmt,BalanceAmt,ExcessDetFK) values ('" + messheader + "','" + messledger + "','0','" + fee_amount + "','0','" + excessdepk + "')";
                                        insExcessLedger = d2.update_method_wo_parameter(ExcessLedger, "Text");
                                    }
                                }
                                else
                                {
                                    string fee_allot_query = "if exists (select * from FT_FeeAllot where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + roll1 + "')) update FT_FeeAllot set TotalAmount=TotalAmount+'" + fee_amount + "',BalAmount=BalAmount+'" + fee_amount + "',LedgerFK='" + messledger + "', HeaderFK='" + messheader + "',FeeCategory='" + textcode + "',FinYearFK='" + fincyr + "',App_No='" + roll1 + "'  where LedgerFK in('" + messledger + "') and HeaderFK in('" + messheader + "') and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No in('" + roll1 + "') else insert into FT_FeeAllot (LedgerFK,HeaderFK,FeeCategory,FinYearFK,App_No,TotalAmount,BalAmount,MemType)values ('" + messledger + "','" + messheader + "','" + textcode + "','" + fincyr + "','" + roll1 + "','" + fee_amount + "','" + fee_amount + "','1')";
                                    insfeeallot = d2.update_method_wo_parameter(fee_allot_query, "Text");
                                }
                                #endregion
                                if (insExcessLedger != 0 && insexcessdet != 0 || insfeeallot != 0)
                                {
                                    feeallotfeestatus = true;
                                }
                            }
                            else
                            {
                                feeallotfeestatus = true;
                            }
                        }
                    }
                    else
                    {
                        //lblalerterr.Visible = true;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Update Hostel Student Details";
                    }
                    #region guest old
                    //Guest 
                    //if (ds6.Tables[1].Rows.Count > 0)
                    //{
                    //    for (int j = 0; j < ds6.Tables[1].Rows.Count; j++)
                    //    {
                    //        string guestcode = Convert.ToString(ds6.Tables[1].Rows[j]["Guestcode"]);
                    //        string guesthostelcode = Convert.ToString(ds6.Tables[1].Rows[j]["Hostel_Code"]);
                    //        string guestclgcode = Convert.ToString(ds6.Tables[1].Rows[j]["college_code"]);
                    //        fixedfinalval = messfee;
                    //        if (isrebate1.Trim() != "" && isrebate1.Trim() == "null" && isrebate1.Trim() == "1")
                    //        {
                    //            isbate = true;
                    //        }
                    //        if (isbate == true)
                    //        {
                    //            if (rdb_indivula.Checked == true)
                    //            {
                    //                string q7 = "SELECT SUM(Rebate_Days)as rebate_days,SUM(Rebate_Amount)as rebate_Amount FROM StudentRebate_Details  WHERE Roll_No = '" + guestcode + "' and Hostel_Code in ('" + guesthostelcode + "') and College_Code in ('" + guestclgcode + "')  AND From_Date >='" + dt.ToString("MM/dd/yyyy") + "' AND To_Date <='" + dt1.ToString("MM/dd/yyyy") + "'";
                    //                ds1.Clear();
                    //                ds1 = d2.select_method_wo_parameter(q7, "Text");
                    //                if (ds1.Tables[0].Rows.Count > 0)
                    //                {
                    //                    rebate_days = "";
                    //                    rebate_Amount = "";
                    //                    rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                    //                    rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                    //                    if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                    //                    {
                    //                        rebate_days1 = Convert.ToDouble(rebate_days);
                    //                        rebateamt1 = rebate_days1 * finalcalvalue;
                    //                    }
                    //                    if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                    //                    {
                    //                        rebateamt1 = Convert.ToDouble(rebate_Amount);
                    //                    }
                    //                }
                    //            }
                    //            else if (rdb_common.Checked == true)
                    //            {
                    //                int count = 0;
                    //                DateTime dt2 = new DateTime();
                    //                dt2 = dt;
                    //                while (dt2 <= dt1)
                    //                {
                    //                    int fdate = Convert.ToInt32(dt2.ToString("dd"));
                    //                    int tdate = Convert.ToInt32(dt1.ToString("dd"));
                    //                    string attend = "[d" + fdate + " ]";
                    //                    string q8 = "SELECT roll_no," + attend + " FROM HAttendance WHERE Roll_No = '" + guestcode + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";//fromdate month and todate month
                    //                    ds2.Clear();
                    //                    ds2 = d2.select_method_wo_parameter(q8, "Text");
                    //                    if (ds2.Tables[0].Rows.Count > 0)
                    //                    {
                    //                        count += 1;
                    //                    }
                    //                    else
                    //                    {
                    //                        if (count != 0)
                    //                        {
                    //                            // countdays.Add(count);
                    //                            string cnt = Convert.ToString(countdays);//[r]
                    //                            string q9 = " select Grant_Day,Grant_Amount  from Rebate_Master where Hostel_Code in ('" + guesthostelcode + "') AND Actual_Day = '" + count + "'";
                    //                            ds3 = d2.select_method_wo_parameter(q9, "Text");
                    //                            if (ds3.Tables[0].Rows.Count > 0)
                    //                            {
                    //                                rebateamt1 = 0;
                    //                                string grant_day = Convert.ToString(ds3.Tables[0].Rows[0][0]);
                    //                                string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0][1]);
                    //                                if (grant_day.Trim() != "" && grant_day.Trim() != null)
                    //                                {
                    //                                    rebate_days1 = Convert.ToDouble(grant_day);
                    //                                    rebateamt1 = rebate_days1 * finalcalvalue;
                    //                                }
                    //                                if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                    //                                {
                    //                                    rebateamt1 = Convert.ToDouble(grant_amt);
                    //                                }
                    //                            }
                    //                        }
                    //                        count = 0;
                    //                    }
                    //                    dt2 = dt2.AddDays(1);
                    //                }
                    //            }
                    //        }
                    //        string insmessbillmaster1 = "if exists(select * from MessBill_Master where BillMonth='" + dt.ToString("MM") + "' and Bill_Year='" + dt.ToString("yyyy") + "') update MessBill_Master set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',From_Date='" + dt.ToString("MM/dd/yyyy") + "',To_Date='" + dt1.ToString("MM/dd/yyyy") + "' where BillMonth='" + dt.ToString("MM") + "' and  Bill_Year='" + dt.ToString("yyyy") + "' and Hostel_Code='" + guesthostelcode + "' and College_Code='" + clgcode + "' else insert into MessBill_Master (Access_Date,Access_Time,From_Date,To_Date,BillMonth,Bill_Year,Hostel_Code,College_Code) values('" + dtaccessdate + "','" + dtaccesstime + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + dt.ToString("MM") + "','" + dt.ToString("yyyy") + "','" + guesthostelcode + "','" + clgcode + "')";
                    //        int insert11 = d2.update_method_wo_parameter(insmessbillmaster1, "Text");
                    //        string Messbill_masterid1 = d2.GetFunction("select messbillmasterid from MessBill_Master where Hostel_Code='" + guesthostelcode + "' and BillMonth='" + dt.ToString("MM") + "' and bill_year='" + dt.ToString("yyyy") + "' and College_Code='" + guestclgcode + "'");
                    //        string insmessbilldetails1 = "if exists(select*from MessBill_Detail where Roll_No='" + guestcode + "' and Hostel_Code='" + guesthostelcode + "' and College_Code='" + guestclgcode + "' and MessBill_MasterCode='" + Messbill_masterid1 + "') update MessBill_Detail set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "', Roll_No='" + guestcode + "',Fixed_Amount='" + fixedfinalval + "',Additional_Amount='" + additionalamt1 + "',Rebate_Amount='" + rebateamt1 + "',MessBill_MasterCode='" + Messbill_masterid1 + "',Is_Staff='0' where Hostel_Code='" + guesthostelcode + "' and College_Code='" + guestclgcode + "' and Roll_No='" + guestcode + "' else insert into MessBill_Detail(Access_Date,Access_Time,Roll_No,Fixed_Amount,Additional_Amount,Rebate_Amount,MessBill_MasterCode,Hostel_Code,College_Code,Is_Staff)values('" + dtaccessdate + "','" + dtaccesstime + "','" + guestcode + "','" + fixedfinalval + "','" + additionalamt1 + "','" + rebateamt1 + "','" + Messbill_masterid1 + "','" + guesthostelcode + "','" + guestclgcode + "','0')";
                    //        int insert111 = d2.update_method_wo_parameter(insmessbilldetails1, "Text");
                    //        if (insert11 != 0)
                    //        {
                    //            insertflag1 = true;
                    //        }
                    //        string getsemester1 = d2.GetFunction("select Current_Semester  from Registration where Roll_No ='" + guestcode + "'");
                    //        if (getsemester1.Trim() == "" || getsemester1.Trim() == null || getsemester1.Trim() == "0")
                    //        {
                    //            getsemester1 = "1";
                    //        }
                    //        feeamount1 = fixedfinalval - rebateamt1;
                    //        fee_amt = feeamount1 + additionalamt1;
                    //        fee_amount = Convert.ToString(Math.Round(fee_amt, 2));
                    //        string textcode = "";
                    //        string settingquery1 = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + guestclgcode + "'";
                    //        ds4.Clear();
                    //        ds4 = d2.select_method_wo_parameter(settingquery1, "Text");
                    //        if (ds4.Tables[0].Rows.Count > 0)
                    //        {
                    //            string linkvalue = Convert.ToString(ds4.Tables[0].Rows[0]["LinkValue"]);
                    //            if (linkvalue == "0")
                    //            {
                    //                string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester1.Trim() + " Semester' and textval not like '-1%'";//Semester
                    //                ds4.Clear();
                    //                ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                    //                if (ds4.Tables[0].Rows.Count > 0)
                    //                {
                    //                    textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                    //                    Session["fee_category"] = Convert.ToString(textcode);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (getsemester1.Trim() == "1" || getsemester1.Trim() == "2")
                    //                {
                    //                    getsemester1 = "1 Year";
                    //                }
                    //                else if (getsemester1.Trim() == "3" || getsemester1.Trim() == "4")
                    //                {
                    //                    getsemester1 = "2 Year";
                    //                }
                    //                else if (getsemester1.Trim() == "5" || getsemester1.Trim() == "6")
                    //                {
                    //                    getsemester1 = "3 Year";
                    //                }
                    //                else if (getsemester1.Trim() == "7" || getsemester1.Trim() == "8")
                    //                {
                    //                    getsemester1 = "4 Year";
                    //                }
                    //                else if (getsemester1.Trim() == "9" || getsemester1.Trim() == "10")
                    //                {
                    //                    getsemester1 = "5 Year";
                    //                }
                    //                else if (getsemester1.Trim() == "11" || getsemester1.Trim() == "12")
                    //                {
                    //                    getsemester1 = "6 Year";
                    //                }
                    //                string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester1.Trim() + "' and textval not like '-1%'";// Year
                    //                ds4.Clear();
                    //                ds4 = d2.select_method_wo_parameter(semesterquery, "Text");
                    //                if (ds4.Tables[0].Rows.Count > 0)
                    //                {
                    //                    textcode = Convert.ToString(ds4.Tables[0].Rows[0]["TextCode"]);
                    //                    Session["fee_category"] = Convert.ToString(textcode);
                    //                }
                    //            }
                    //        }
                    //        string q12 = " SELECT Mess_FeeCode,f.header_id  FROM Hostel_Details h,fee_info f where h.Mess_FeeCode =f.fee_code and h.Hostel_code ='" + guesthostelcode + "'";
                    //        ds5.Clear();
                    //        ds5 = d2.select_method_wo_parameter(q12, "Text");
                    //        if (ds5.Tables[0].Rows.Count > 0)
                    //        {
                    //            messfee_code = Convert.ToString(ds5.Tables[0].Rows[0]["Mess_FeeCode"]);
                    //            header_code = Convert.ToString(ds5.Tables[0].Rows[0]["header_id"]);
                    //            if (messfee_code.Trim() != "")
                    //            {
                    //                string fee_allot_query = "if exists(select * from  Fee_Allot where fee_code='" + messfee_code + "' and fee_category ='" + textcode + "' and roll_admit ='" + guestcode + "') update fee_allot set allotdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',flag_status='false',fee_amount='" + fee_amount + "',duedate='',permittedby='0',fine='0',deduct='0',total='" + fee_amount + "',intvallot='N',AdmisFees='0',DueExtDate1='',fine1='0',DueExtDate2='',fine2='0',DueExtDate3='',fine3='0',DueExtDate4='',fine4='0',Allot_Flg='1', batch='0',refound_amt='0',semyearflg='0',seatcate='0',modeofpay='Regular',app_formno='' where fee_code='" + messfee_code + "' and fee_category ='" + textcode + "' and roll_admit ='" + guestcode + "' else INSERT INTO Fee_Allot(roll_admit,fee_code,allotdate,flag_status,fee_amount,fee_category,duedate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg, batch,refound_amt,semyearflg,seatcate,modeofpay,app_formno) VALUES('" + guestcode + "','" + messfee_code + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','false','" + fee_amount + "','" + textcode + "','',0,0,0,'" + fee_amount + "','N',0,'',0,'',0,'',0,'',0,1,'0',0,0,'0','Regular','')";
                    //                int fee_allotins = d2.update_method_wo_parameter(fee_allot_query, "Text");
                    //                string fee_status = "if exists(select*from fee_status where roll_admit='" + guestcode + "' and header_id='" + header_code + "' and fee_category='" + textcode + "') update fee_status set amount=(amount+'" + fee_amount + "'),balance=(balance+'" + fee_amount + "') where roll_admit='" + guestcode + "' and header_id='" + header_code + "' and fee_category='" + textcode + "' else INSERT INTO fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno) VALUES('" + guestcode + "','" + fee_amount + "',0,'" + fee_amount + "','false','" + textcode + "', '" + header_code + "','0','')";
                    //                int fee_status_ins = d2.update_method_wo_parameter(fee_status, "Text");
                    //                if (fee_allotins != 0 && fee_status_ins != 0)
                    //                {
                    //                    feeallotfeestatus1 = true;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    lblalerterr.Visible = true;
                    //    alertpopwindow.Visible = true;
                    //    lblalerterr.Text = "Please Update Guest Details";
                    //}
                    #endregion
                    if (insertflag == true && feeallotfeestatus == true || insertflag1 == true || feeallotfeestatus1 == true)
                    {
                        lblalerterr.Visible = true;
                        alertpopwindow.Visible = true;
                        Div2.Visible = false;
                        lblalerterr.Text = "Saved Successfully";
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            lblalerterr.Visible = true;
            alertpopwindow.Visible = true;
            lblalerterr.Text = ex.ToString();
        }
    }
    protected void calculation(ref double TotalperdayAmt, ref double TotalCount, ref double NonvegPerdayAmt, ref double NonvegCount, ref double VegCount, ref double VegExpansestotal, ref double NonvegExpanceTotal, ref double CommonExpances)
    {
        if (dt <= dt1)
        {
            string hostelcode = "";
            string HostelMasterFk = string.Empty;
            string messnemeMasterFk = string.Empty;
            if (rdb_common1.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (hostelcode == "")
                            hostelcode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                        else
                            hostelcode = hostelcode + "," + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
                HostelMasterFk = hostelcode;
            }
            else
            {
                hostelcode = Convert.ToString(Session["hoscode"]);
                HostelMasterFk = Convert.ToString(Session["hoscode"]).Replace("'", "");
            }
            //string q2 = "SELECT (SUM(Consumption_Qty) * RPU)as consume_total,itemheader_name,RPU FROM DailyConsumption_Detail D,DailyConsumption_Master M,item_master I WHERE D.Item_Code = I.item_code AND D.DailyConsumptionMaster_Code = M.DailyConsumptionMaster_Code AND Consumption_Date BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "'  and M.Hostel_Code in ('" + Convert.ToString(Session["Messidcode"]) + "') GROUP BY itemheader_name,RPU";
            //string q2 = "select (SUM(ConsumptionQty)*rpu)as consume_total from HT_DailyConsumptionMaster m,HT_DailyConsumptionDetail d,IM_ItemMaster i where d.DailyConsumptionMasterFK=m.DailyConsumptionMasterPK and d.ItemFK=i.ItemPK and m.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and m.MessMasterFK in('" + Convert.ToString(Session["Messidcode"]) + "') and ForMess <>'2' group by ItemHeaderName,RPU";
            string q2 = " select SUM(ConsumptionQty*rpu)as consume_total,d.menutype from HT_DailyConsumptionMaster m,HT_DailyConsumptionDetail d,IM_ItemMaster i where d.DailyConsumptionMasterFK=m.DailyConsumptionMasterPK and d.ItemFK=i.ItemPK and m.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and m.MessMasterFK in('" + Convert.ToString(Session["Messidcode"]) + "') and ForMess <>'2' and d.menutype is not  null group by d.menutype";
            //q2 = q2 + " select SUM(expamount)as expanses_total from HT_HostelExpenses where  HostelFK in('" + hostelcode + "') and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ExpGroup";
            //q2 = q2 + " select SUM(expamount)as expanses_total,ExpensesType from HT_HostelExpenses where   HostelFK in('" + hostelcode + "') and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ExpensesType ";
            q2 = q2 + " select SUM(ex.ExpAmount)as Exp_Amount, MasterCode,ExpensesType,Messname  from HT_HostelExpenses ex,CO_MasterValues co where ex.ExpGroup=co.MasterCode and MasterCriteria='hostelexpgrp' and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  and  Messname in('" + Convert.ToString(Session["Messidcode"]) + "') group by MasterCode,ExpensesType,Messname order by ExpensesType ";
            q2 = q2 + " select SUM(IncomeAmount)as income_total from HT_HostelIncome where HostelMasterFK in('" + hostelcode + "') and IncomeDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            q2 = q2 + " select COUNT(*)as no_of_student from HT_HostelRegistration where MemType=1 and ISNULL(IsSuspend,'')=0 and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and StudMessType='0'  and Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "')  and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";//barath 21.10.17 adm date added//and HostelMasterFK in('" + hostelcode + "')
            q2 = q2 + " select COUNT(*)as no_of_student1 from HT_HostelRegistration where MemType=1 and ISNULL(IsSuspend,'')=0 and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and StudMessType='1'  and Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "')   and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";//barath 21.10.17 adm date added // magesh and HostelMasterFK in('" + hostelcode + "')
            q2 = q2 + " select COUNT(distinct staff_code) as staff_total from DayScholourStaffAdd where Hostel_Code in ('" + Convert.ToString(Session["Messidcode"]) + "')  and Typ =2";
            q2 = q2 + " select COUNT(distinct Roll_No) as dayscholour_total from DayScholourStaffAdd where Hostel_Code in ('" + Convert.ToString(Session["Messidcode"]) + "') and Typ =1";
            q2 = q2 + " select COUNT(*)as register_guest from HT_HostelRegistration where MemType=3 and IsVacated=0  and Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "') and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0";//and HostelMasterFK in('" + hostelcode + "')
            q2 = q2 + " select COUNT(*)as registerStaff from HT_HostelRegistration where MemType=2 and IsVacated=0 and  Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "') and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0";//and HostelMasterFK in('" + hostelcode + "') 

            //barath 21.10.17 adm date added
            //ds = d2.select_method_wo_parameter(q2, "Text");



            Hashtable hat = new Hashtable();
            hat.Add("@MessmasterFK", Convert.ToString(Session["Messidcode"]));
            hat.Add("@HostelMasterfk", HostelMasterFk);

            
            hat.Add("@ConsumptionFromDate", dt.ToString("MM/dd/yyyy"));
            hat.Add("@ConsumptionToDate", dt1.ToString("MM/dd/yyyy"));
            hat.Add("@Admdate", dt1.ToString("MM/dd/yyyy"));
            ds = d2.select_method("MessbillCalcultion", hat, "sp");//02.02.18

            double consumtiontotal = 0;
            double VegConsumtiontotal = 0;
            double NonVegConsumtiontotal = 0;
            //double VegExpansestotal = 0;
            //double NonvegExpanceTotal = 0;
            double incometotal = 0;
            double totalcalvalue = 0;
            double noofstudent = 0;
            double noofstudent1 = 0;
            double Vegstudent = 0;
            double NonVegstudent = 0;
            double guestVeg = 0;
            double guestNonVeg = 0;
            double totalstudent = 0;
            double totalguest = 0;
            double finalcalvalue = 0;
            double stafftotal = 0;
            double daysholour = 0;
            double calstaff = 0;
            double caldayscholour = 0;
            double finalvalue = 0;
            double guesttotal = 0;
            double guestnontotal = 0;
            double calguest = 0;
            double vegCalvalue = 0;
            double NonvegCalvalue = 0;
            int HostlerVEGStaffTotal = 0;
            int HostlerNONVEGStaffTotal = 0;
            double HostlerStaffTotalCal = 0;
            double HostlerStaffTotal = 0;
            double HostlerStaffTotalCheckval = 0;
            double HostlerStaffvegTotalCheckval = 0;
            double HostlerStaffnonvegTotalCheckval = 0;
            if (ds.Tables[0].Rows.Count > 0)
                VegConsumtiontotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(consume_total)", "menutype='0'"));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.RowFilter = "menutype='1'";
                DataView dvfilter1 = new DataView();
                dvfilter1 = ds.Tables[0].DefaultView;
                if (dvfilter1.Count!=0)
                NonVegConsumtiontotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(consume_total)", "menutype='1'"));
                consumtiontotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(consume_total)", ""));
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[1].Rows[0][0]).Trim() != "" && Convert.ToString(ds.Tables[1].Rows[0][0]).Trim() != null)
                {
                    //condition added by prabha on feb 28 2018
                    DataView dvfilter = new DataView();
                    ds.Tables[1].DefaultView.RowFilter = "ExpensesType='1'";
                    dvfilter = ds.Tables[1].DefaultView;
                    if (dvfilter.Count > 0)
                        VegExpansestotal = Convert.ToDouble(ds.Tables[1].Compute("Sum(Exp_Amount)", "ExpensesType='1'"));
                    ds.Tables[1].DefaultView.RowFilter = "ExpensesType='2'";
                    dvfilter = ds.Tables[1].DefaultView;
                    if (dvfilter.Count > 0)
                        NonvegExpanceTotal = Convert.ToDouble(ds.Tables[1].Compute("Sum(Exp_Amount)", "ExpensesType='2'"));
                    ds.Tables[1].DefaultView.RowFilter = "ExpensesType='0'";
                    dvfilter = ds.Tables[1].DefaultView;
                    if (dvfilter.Count > 0)
                        CommonExpances = Convert.ToDouble(ds.Tables[1].Compute("Sum(Exp_Amount)", "ExpensesType='0'"));
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        string groupcode = Convert.ToString(ds.Tables[1].Rows[i]["MasterCode"]);
                        string groupexpanses = Convert.ToString(ds.Tables[1].Rows[i]["Exp_Amount"]);
                        if (expanses_hash.ContainsKey(groupcode))
                        {
                            double value = 0;
                            double.TryParse(Convert.ToString(expanses_hash[groupcode]), out value);
                            expanses_hash.Remove(groupcode);
                            double total = 0;
                            double groupExpances = 0;
                            double.TryParse(groupexpanses, out groupExpances);
                            total = value + groupExpances;
                            expanses_hash.Add(groupcode, total);
                        }
                        else
                        {
                            expanses_hash.Add(groupcode, groupexpanses);
                        }
                    }
                    //expansestotal = Convert.ToDouble(ds.Tables[1].Compute("Sum(expanses_total)", ""));
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[2].Rows[0][0]).Trim() != "" && Convert.ToString(ds.Tables[2].Rows[0][0]).Trim() != null)
                    incometotal = Convert.ToDouble(ds.Tables[2].Compute("Sum(income_total)", ""));
            }
            if (ds.Tables[3].Rows.Count > 0)
                noofstudent = Convert.ToDouble(ds.Tables[3].Compute("Sum(no_of_student)", ""));
            if (ds.Tables[4].Rows.Count > 0)
                noofstudent1 = Convert.ToDouble(ds.Tables[4].Compute("Sum(no_of_student1)", ""));
            if (ds.Tables[5].Rows.Count > 0)
                stafftotal = Convert.ToDouble(ds.Tables[5].Compute("Sum(staff_total)", ""));
            if (ds.Tables[6].Rows.Count > 0)
                daysholour = Convert.ToDouble(ds.Tables[6].Compute("Sum(dayscholour_total)", ""));
            if (ds.Tables[7].Rows.Count > 0)
                guesttotal = Convert.ToDouble(ds.Tables[7].Compute("Sum(register_guest)", ""));
            //magesh 2.4.18
            if (ds.Tables[8].Rows.Count > 0)
                guestnontotal = Convert.ToDouble(ds.Tables[8].Compute("Sum(register_guest)", ""));
            if (ds.Tables[9].Rows.Count > 0)
                HostlerStaffTotal = Convert.ToDouble(ds.Tables[9].Compute("Sum(registerStaff)", ""));


            //check isrebete
            hoscode = Convert.ToString(Session["hoscode"]);
            string rabat = "select IncludeRebate  from HM_HostelMaster where HostelMasterPK in('" + hoscode + "')";
            ds2 = d2.select_method_wo_parameter(rabat, "Text");
            string isrebate1 = "";
            if (ds2.Tables[0].Rows.Count > 0)
                isrebate1 = Convert.ToString(ds2.Tables[0].Rows[0]["IncludeRebate"]);
            if (isrebate1.Trim() == "True")//isrebate1.Trim() != "" && isrebate1.Trim() == "null" &&
                isbate = true;
            else
                isbate = false;
            //calculation

            int reb_days = 0;
            int vegRebDays = 0;
            int NonvegRebDays = 0;
            int reb_amount = 0;
            int vegRebAmt = 0;
            int NonvegRebAmt = 0;
            totalcalvalue = consumtiontotal;
            if (cb_addex.Checked)
            {
                totalcalvalue = consumtiontotal + VegExpansestotal;
                vegCalvalue = CommonExpances + VegConsumtiontotal + VegExpansestotal;//barath common expance added nec 14.02.18
                NonvegCalvalue = NonVegConsumtiontotal + NonvegExpanceTotal; //barath 12.02.17 + VegExpansestotal + VegConsumtiontotal+ CommonExpances 
            }
            if (cb_addinco.Checked)
            {
                totalcalvalue -= incometotal;
                vegCalvalue -= incometotal;// VegExpansestotal; 22.12.17 barath
                //NonvegCalvalue -= VegExpansestotal + NonvegExpanceTotal;
            }

            if (cb_hosteler.Checked)
            {
                #region Hosteler
                Vegstudent = noofstudent * days;
                NonVegstudent = noofstudent1 * days;
                totalstudent = Vegstudent + NonVegstudent;
                clgcode = Convert.ToString(Session["clgcode"]);
                string q6 = "select r.App_No,h.HostelMasterFK,r.college_code,h.studmesstype from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'0')=0 and ISNULL(IsDiscontinued,'0')=0 and ISNULL(IsVacated,'0')=0 and memtype='1'  and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "' and ISNULL(IsSuspend,'')=0 and  h.Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "')";//barath 21.10.17 adm date added and ISNULL(IsSuspend,'')=0
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(q6, "Text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                    {
                        string roll1 = Convert.ToString(ds3.Tables[0].Rows[j][0]);
                        string hostelcode3 = Convert.ToString(ds3.Tables[0].Rows[j][1]);
                        string collegecode3 = Convert.ToString(ds3.Tables[0].Rows[j][2]);
                        string rebate_days = "";
                        string rebate_Amount = "";
                        string grantday1 = "";
                        double rebateamt = 0;
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + roll1 + "'"; //RebateType='1' and //22.12.17 barath
                            //string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail rb,ht_hostelregistration h  where rb.app_no=h.app_no and h.studmesstype='" + Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]) +"' and RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + roll1 + "'"; 
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    // reb_days = reb_days + Convert.ToInt32(rebate_days);
                                    grantday1 = Convert.ToString(rebate_days);
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt = Convert.ToDouble(rebate_Amount);
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            #region Commom
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            string grant_day = "";
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + roll1 + "' AND AttnMonth  = '" + dt2.ToString("MM") + "' AND AttnYear = '" + dt2.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        DataSet newdataset = new DataSet();
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode + "')  and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1'
                                        newdataset = d2.select_method_wo_parameter(q9, "Text");
                                        if (newdataset.Tables[0].Rows.Count > 0)
                                        {
                                            grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                            string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                            if (count != 0)
                            {
                                DataSet newdataset = new DataSet();
                                string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode + "') and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1' 
                                newdataset = d2.select_method_wo_parameter(q9, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                    string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                    if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                    {
                                        grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                    }
                                    if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                    {
                                        rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                    }
                                }
                            }
                            #endregion
                        }
                        if (grantday1.Trim() == "")
                        {
                            grantday1 = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(grantday1);
                        reb_amount = reb_amount + Convert.ToInt32(rebateamt);
                        if (Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]) == "0")//22.12.17 barath
                        {
                            vegRebDays += Convert.ToInt32(grantday1);
                            vegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        else
                        {
                            NonvegRebDays += Convert.ToInt32(grantday1);
                            NonvegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        if (!grantday_hash.ContainsKey(roll1))
                            grantday_hash.Add(roll1, grantday1);
                        if (!Rebateamount_hash.ContainsKey(roll1))
                            Rebateamount_hash.Add(roll1, rebateamt);
                    }

                  
                }
                else
                {
                    hostel_bool = true;
                }
                #endregion
            }
            if (cb_instaff.Checked)
            {
                calstaff = stafftotal * days;
                // totalstaff = calstaff / days;
            }
            if (cb_dayssch.Checked)
            {
                caldayscholour = daysholour * days;
                //totaldayscholour = caldayscholour / days;
            }
            if (cbHostlerStaff.Checked)
            {
                HostlerStaffTotalCheckval = HostlerStaffTotal;
                HostlerStaffTotalCal = HostlerStaffTotal * days;

                //magesh 2.4.18
                if (ds.Tables[9].Rows.Count > 0)
                {
                    int.TryParse(Convert.ToString(ds.Tables[9].Compute("Sum(registerStaff)", "StudMessType='0'")), out HostlerVEGStaffTotal);
                    int.TryParse(Convert.ToString(ds.Tables[9].Compute("Sum(registerStaff)", "StudMessType='1'")), out HostlerNONVEGStaffTotal);
                    HostlerStaffnonvegTotalCheckval = HostlerNONVEGStaffTotal * days;
                    HostlerStaffvegTotalCheckval = HostlerVEGStaffTotal * days;

                }
                #region Staff

                string q6 = "select staff_code,ht.APP_No,ht.HostelMasterFK,hd.CollegeCode,ht.studmesstype from staffmaster s,staff_appl_master a,HT_HostelRegistration ht,HM_HostelMaster hd where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and ht.MemType='2' and a.appl_id=ht.app_no and HT.Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "') and ht.HostelMasterFK=hd.HostelMasterPK and isnull(ht.isdiscontinued,0)=0 and isnull(ht.issuspend,0)=0 and isnull(isvacated,0)=0 ";
               
                ds.Clear();
                ds = d2.select_method_wo_parameter(q6, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string guestcode = Convert.ToString(ds.Tables[0].Rows[j]["APP_No"]);
                        string guesthostelcode = Convert.ToString(ds.Tables[0].Rows[j]["HostelMasterFK"]);
                        string guestclgcode = Convert.ToString(ds.Tables[0].Rows[j]["CollegeCode"]);
                        string rebateday = "";
                        double rebateamt = 0;
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where  RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + guestcode + "'";// RebateType='1' and
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string rebate_days = "";
                                string rebate_Amount = "";
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    // reb_days = reb_days + Convert.ToInt32(rebate_days);
                                    rebateday = Convert.ToString(rebate_days);
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt = Convert.ToDouble(rebate_Amount);
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + guestcode + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + guesthostelcode + "')  and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1'
                                        ds3 = d2.select_method_wo_parameter(q9, "Text");
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            string grant_day = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_days"]);
                                            string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_Amount"]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                rebateday = rebateday + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                        }
                        if (rebateday.Trim() == "")
                        {
                            rebateday = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(rebateday);
                        reb_amount = reb_amount + Convert.ToInt32(rebateamt);
                        if (Convert.ToString(ds.Tables[0].Rows[j]["studmesstype"]) == "0")//22.12.17 barath
                        {
                            vegRebDays += Convert.ToInt32(rebateday);
                            vegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        else
                        {
                            NonvegRebDays += Convert.ToInt32(rebateday);
                            NonvegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        if (!staffrebateday_hash.ContainsKey(guestcode))
                        {
                            staffrebateday_hash.Add(guestcode, rebateday);
                        }
                        if (!Rebateamount_hash.ContainsKey(guestcode))
                        {
                            staffrebateamount_hash.Add(guestcode, rebateamt);
                        }
                    }
                }
                else
                {
                    guest_bool = true;
                }
                #endregion
            }
            if (cb_guest.Checked)
            {
                #region Guest
                calguest = guesttotal * days;
                guestNonVeg = guestnontotal * days;
                totalguest = calguest + guestNonVeg;
                string q6 = " select distinct gr.APP_No, gr.HostelMasterFK,hd.CollegeCode,gr.studmesstype from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and  gr.Messcode in ('" + Convert.ToString(Session["Messidcode"]) + "') and isnull(gr.isdiscontinued,0)=0 and isnull(gr.issuspend,0)=0 and isnull(isvacated,0)=0 ";//HostelMasterFK in('" + hostelcode + "')
                ds.Clear();
                ds = d2.select_method_wo_parameter(q6, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string guestcode = Convert.ToString(ds.Tables[0].Rows[j]["APP_No"]);
                        string guesthostelcode = Convert.ToString(ds.Tables[0].Rows[j]["HostelMasterFK"]);
                        string guestclgcode = Convert.ToString(ds.Tables[0].Rows[j]["CollegeCode"]);
                        string rebateday = "";
                        double rebateamt = 0;
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where  RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + guestcode + "'";// RebateType='1' and
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string rebate_days = "";
                                string rebate_Amount = "";
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    // reb_days = reb_days + Convert.ToInt32(rebate_days);
                                    rebateday = Convert.ToString(rebate_days);
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt = Convert.ToDouble(rebate_Amount);
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + guestcode + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + guesthostelcode + "')  and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1'
                                        ds3 = d2.select_method_wo_parameter(q9, "Text");
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            string grant_day = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_days"]);
                                            string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_Amount"]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                rebateday = rebateday + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                        }
                        if (rebateday.Trim() == "")
                        {
                            rebateday = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(rebateday);
                        reb_amount = reb_amount + Convert.ToInt32(rebateamt);
                        if (Convert.ToString(ds.Tables[0].Rows[j]["studmesstype"]) == "0")//22.12.17 barath
                        {
                            vegRebDays += Convert.ToInt32(rebateday);
                            vegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        else
                        {
                            NonvegRebDays += Convert.ToInt32(rebateday);
                            NonvegRebAmt += Convert.ToInt32(rebateamt);
                        }
                        if (!guestgrant_hash.ContainsKey(guestcode))
                        {
                            guestgrant_hash.Add(guestcode, rebateday);
                        }
                        if (!Rebateamount_hash.ContainsKey(guestcode))
                        {
                            guestRebateamount_hash.Add(guestcode, rebateamt);
                        }
                    }
                }
                else
                {
                    guest_bool = true;
                }
                #endregion
            }
            //finalvalue = calstaff + caldayscholour + totalstudent + calguest;//staff+daysscholour+hosteler               
            //finalvalue = finalvalue - reb_days;
            //finalcalvalue = totalcalvalue / finalvalue;
            //Session["finalcalvalue"] = Convert.ToDouble(finalcalvalue);
            //Session["finalvalue"] = finalvalue;
            //Total Mandays
            //magesh 2.4.18
           // TotalCount = calstaff + caldayscholour + totalstudent + calguest + HostlerStaffTotalCal;//Vegstudent
            TotalCount = calstaff + caldayscholour + totalstudent + totalguest + HostlerStaffTotalCal;//Vegstudent
            TotalCount = TotalCount - reb_days;
            TotalperdayAmt = vegCalvalue / TotalCount;
            //veg mandays
            //magesh
          //  VegCount = calstaff + caldayscholour + Vegstudent + calguest + HostlerVEGStaffTotal;//Vegstudent
            VegCount = calstaff + caldayscholour + Vegstudent + calguest + HostlerStaffvegTotalCheckval;//Vegstudent

            
            VegCount -= vegRebDays;// reb_days;

            //Non veg
            //magesh 2.4.18
           // NonvegCount = calstaff + caldayscholour + NonVegstudent + calguest + HostlerNONVEGStaffTotal;
            NonvegCount = calstaff + caldayscholour + NonVegstudent + guestNonVeg + HostlerStaffnonvegTotalCheckval;
            NonvegCount = NonvegCount - NonvegRebDays;// reb_days;NonvegRebDays
            NonvegPerdayAmt = NonvegCalvalue / NonvegCount;
            int daysscholorcount = 0;
            int guesttotalcount = 0;
            int staffTotalCount = 0;
            if (cb_dayssch.Checked == true)
                daysscholorcount = Convert.ToInt32(daysholour);
            if (cb_guest.Checked == true)
                guesttotalcount = Convert.ToInt32(guesttotal);
            if (cb_instaff.Checked == true)
                int.TryParse(Convert.ToString(stafftotal), out staffTotalCount);
            
            TotalNoofStudentstrength = Convert.ToInt32(noofstudent) + Convert.ToInt32(noofstudent1) + staffTotalCount + guesttotalcount + Convert.ToInt32(guestnontotal) + daysscholorcount + Convert.ToInt32(HostlerStaffTotalCheckval);
           
            //magesh 2.4.18
            //NonvegTotalNoofStudentstrength = Convert.ToInt32(noofstudent1) + Convert.ToInt32(staffTotalCount) + guesttotalcount + daysscholorcount + HostlerNONVEGStaffTotal;
            NonvegTotalNoofStudentstrength = Convert.ToInt32(noofstudent1) + Convert.ToInt32(staffTotalCount) +Convert.ToInt32(guestnontotal)+ daysscholorcount + HostlerNONVEGStaffTotal;
            VegTotalNoofStudentstrength = Convert.ToInt32(noofstudent) + Convert.ToInt32(staffTotalCount) + guesttotalcount + daysscholorcount + HostlerVEGStaffTotal;//delsi 
        }
    }
    //26.12.15
    protected void indiviual_calculation()
    {
        if (dt <= dt1)
        {
            string hostelcode = "";
            if (rdb_common1.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (hostelcode == "")
                        {
                            hostelcode = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            hostelcode = hostelcode + "'" + "," + "" + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            else
            {
                hostelcode = Convert.ToString(Session["hoscode"]);
            }
            string grpincome = "";
            if (txt_groupname.Text.Trim() != "--Select--")
            {
                for (int i = 0; i < cbl_groupname.Items.Count; i++)
                {
                    if (cbl_groupname.Items[i].Selected == true)
                    {
                        if (grpincome == "")
                        {
                            grpincome = "" + cbl_groupname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            grpincome = grpincome + "'" + "," + "" + "'" + cbl_groupname.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            else
            {
                grpincome = "0";
            }
            string grpexpance = "";
            if (txt_groupnameex.Text.Trim() != "--Select--")
            {
                for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
                {
                    if (cbl_groupnameex.Items[i].Selected == true)
                    {
                        if (grpexpance == "")
                        {
                            grpexpance = "" + cbl_groupnameex.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            grpexpance = grpexpance + "'" + "," + "" + "'" + cbl_groupnameex.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            else
            {
                grpexpance = "0";
            }
            //string q2 = "select (SUM(ConsumptionQty)*rpu)as consume_total from HT_DailyConsumptionMaster m,HT_DailyConsumptionDetail d,IM_ItemMaster i where d.DailyConsumptionMasterFK=m.DailyConsumptionMasterPK and d.ItemFK=i.ItemPK and m.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and m.MessMasterFK in('" + Convert.ToString(Session["Messidcode"]) + "') and ForMess <>'2'  group by ItemHeaderName,RPU";
            string q2 = "select ExpDesc,SUM(ExpAmount)as expanses_total from HT_HostelExpenses where HostelFK in('" + hostelcode + "') and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ExpensesType='2' group by ExpGroup,ExpDesc ";
            q2 = q2 + " select ExpGroup,SUM(ExpAmount)as expanses_total from HT_HostelExpenses where HostelFK in('" + hostelcode + "') and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ExpensesType='1' group by ExpGroup";
            q2 = q2 + " select IncomeGroup,SUM(IncomeAmount)as income_total from HT_HostelIncome where IncomeDate BETWEEN '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and HostelMasterFK in('" + hostelcode + "') and IncomeGroup in('" + grpincome + "')group by IncomeGroup";
            // q2 = q2 + " select SUM(expamount)as expanses_total from HT_HostelExpenses where  HostelFK in('" + hostelcode + "') and ExpensesDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ExpGroup";
            //q2 = q2 + " select SUM(IncomeAmount)as income_total from HT_HostelIncome where HostelMasterFK in('" + hostelcode + "') and IncomeDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            q2 = q2 + " select COUNT(*)as no_of_student from HT_HostelRegistration where MemType=1 and ISNULL(IsSuspend,'')=0 and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and StudMessType='0' and HostelMasterFK in('" + hostelcode + "')";
            q2 = q2 + " select COUNT(*)as no_of_student1 from HT_HostelRegistration where MemType=1 and ISNULL(IsSuspend,'')=0 and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and StudMessType='1' and HostelMasterFK in('" + hostelcode + "')";
            q2 = q2 + " select COUNT(distinct staff_code) as staff_total from DayScholourStaffAdd where Hostel_Code in ('" + hostelcode + "') and Typ =2";
            q2 = q2 + " select COUNT(distinct Roll_No) as dayscholour_total from DayScholourStaffAdd where Hostel_Code in ('" + hostelcode + "') and Typ =1";
            q2 = q2 + " select COUNT(*)as register_guest from HT_HostelRegistration where MemType=3 and isnull(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0 and HostelMasterFK in('" + hostelcode + "')";
            ds = d2.select_method_wo_parameter(q2, "Text");
            double consumtiontotal = 0;
            double expansestotal = 0;
            double incometotal = 0;
            double totalcalvalue = 0;
            double noofstudent = 0;
            double noofstudent1 = 0;
            double calstudent = 0;
            double calstudent1 = 0;
            double totalstudent = 0;
            double finalcalvalue = 0;
            double stafftotal = 0;
            double daysholour = 0;
            double calstaff = 0;
            double caldayscholour = 0;
            double finalvalue = 0;
            double guesttotal = 0;
            double calguest = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                consumtiontotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(expanses_total)", ""));
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    string groupcode = Convert.ToString(ds.Tables[1].Rows[i]["ExpGroup"]);
                    string groupexpanses = Convert.ToString(ds.Tables[1].Rows[i]["expanses_total"]);
                    expanses_hash.Add(groupcode, groupexpanses);
                }
                expansestotal = Convert.ToDouble(ds.Tables[1].Compute("Sum(expanses_total)", ""));
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                incometotal = Convert.ToDouble(ds.Tables[2].Compute("Sum(income_total)", ""));
                string incgroup = "";
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    string incgroupcode = Convert.ToString(ds.Tables[2].Rows[i]["IncomeGroup"]);
                    if (incgroup == "")
                    {
                        incgroup = "" + incgroupcode + "";
                    }
                    else
                    {
                        incgroup = incgroup + "," + "" + incgroupcode + "";
                    }
                }
                Session["incgroup"] = incgroup;
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                noofstudent = Convert.ToDouble(ds.Tables[3].Compute("Sum(no_of_student)", ""));
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                noofstudent1 = Convert.ToDouble(ds.Tables[4].Compute("Sum(no_of_student1)", ""));
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                stafftotal = Convert.ToDouble(ds.Tables[5].Compute("Sum(staff_total)", ""));
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                daysholour = Convert.ToDouble(ds.Tables[6].Compute("Sum(dayscholour_total)", ""));
            }
            if (ds.Tables[7].Rows.Count > 0)
            {
                guesttotal = Convert.ToDouble(ds.Tables[7].Compute("Sum(register_guest)", ""));
            }
            //check isrebete
            hoscode = Convert.ToString(Session["hoscode"]);
            string rabat = "select IncludeRebate  from HM_HostelMaster where HostelMasterPK in('" + hoscode + "')";
            ds2 = d2.select_method_wo_parameter(rabat, "Text");
            string isrebate1 = "";
            if (ds2.Tables[0].Rows.Count > 0)
            {
                isrebate1 = Convert.ToString(ds2.Tables[0].Rows[0]["IncludeRebate"]);
            }
            if (isrebate1.Trim() == "True")//isrebate1.Trim() != "" && isrebate1.Trim() == "null" &&
            {
                isbate = true;
            }
            else
            {
                isbate = false;
            }
            //calculation
            int guestrebateday = 0;
            int reb_days = 0;
            totalcalvalue = consumtiontotal;
            if (cb_addex.Checked == true)
            {
                totalcalvalue = consumtiontotal;// +expansestotal;
            }
            if (cb_addinco.Checked == true)
            {
                totalcalvalue = totalcalvalue - incometotal;
            }
            if (cb_hosteler.Checked == true)
            {
                calstudent = noofstudent * days;
                calstudent1 = noofstudent1 * days;
                Session["nonveg"] = calstudent1;
                totalstudent = calstudent + calstudent1;
                clgcode = Convert.ToString(Session["clgcode"]);
                TotalNoofStudentstrength = Convert.ToInt32(noofstudent + noofstudent1);
                string q6 = " select r.app_no,h.HostelMasterFK,r.college_code from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'')=0 and ISNULL(IsDiscontinued,'')=0 and ISNULL(IsVacated,'')=0 and h.HostelMasterFK in('" + hostelcode + "')";
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(q6, "Text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                    {
                        double reb_amt = 0;
                        string roll1 = Convert.ToString(ds3.Tables[0].Rows[j][0]);
                        string hostelcode3 = Convert.ToString(ds3.Tables[0].Rows[j][1]);
                        string collegecode3 = Convert.ToString(ds3.Tables[0].Rows[j][2]);
                        string rebate_days = "";
                        string rebate_Amount = "";
                        string grantday1 = "";
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where  RebateType='1' and RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + roll1 + "'";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    grantday1 = Convert.ToString(rebate_days);
                                    Session["reb_days"] = reb_days;
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    reb_amt = reb_amt + Convert.ToDouble(rebate_Amount);
                                    Session["reb_amt"] = reb_amt;
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            string grant_day = "";
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + roll1 + "' AND AttnMonth  = '" + dt2.ToString("MM") + "' AND AttnYear = '" + dt2.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        DataSet newdataset = new DataSet();
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode + "')and RebateType='1' and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";
                                        newdataset = d2.select_method_wo_parameter(q9, "Text");
                                        if (newdataset.Tables[0].Rows.Count > 0)
                                        {
                                            double grantamt = 0;
                                            grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                            string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                grantamt = Convert.ToDouble(grant_amt);
                                                grantamt = grantamt + Convert.ToDouble(grant_amt);
                                                Session["grantamt"] = grantamt;
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                            if (count != 0)
                            {
                                DataSet newdataset = new DataSet();
                                string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + hostelcode + "')and RebateType='1' and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";
                                newdataset = d2.select_method_wo_parameter(q9, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    double grantamt = 0;
                                    grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                    string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                    if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                    {
                                        grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                    }
                                    if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                    {
                                        grantamt = Convert.ToDouble(grant_amt);
                                        grantamt = grantamt + Convert.ToDouble(grant_amt);
                                        Session["grantamt"] = grantamt;
                                    }
                                }
                            }
                        }
                        if (grantday1.Trim() != "")
                        {
                        }
                        else
                        {
                            grantday1 = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(grantday1);
                        Session["reb_days"] = reb_days;
                        if (!grantday_hash.ContainsKey(roll1))
                        {
                            grantday_hash.Add(roll1, grantday1);
                        }
                    }
                }
                else
                {
                    hostel_bool = true;
                }
            }
            if (cb_instaff.Checked == true)
            {
                TotalNoofStudentstrength = Convert.ToInt32(TotalNoofStudentstrength + stafftotal);
                calstaff = stafftotal * days;
                // totalstaff = calstaff / days;
            }
            if (cb_dayssch.Checked == true)
            {
                caldayscholour = daysholour * days;
                TotalNoofStudentstrength = Convert.ToInt32(TotalNoofStudentstrength + daysholour);
                //totaldayscholour = caldayscholour / days;
            }
            if (cb_guest.Checked == true)
            {
                calguest = guesttotal * days;
                TotalNoofStudentstrength = Convert.ToInt32(TotalNoofStudentstrength + guesttotal);
                double rebate_days1 = 0;
                string q6 = " select distinct  gr.HostelMasterFK,gr.APP_No,hd.CollegeCode from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and HostelMasterFK in('" + hostelcode + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q6, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string guestcode = Convert.ToString(ds.Tables[0].Rows[j]["APP_No"]);
                        string guesthostelcode = Convert.ToString(ds.Tables[0].Rows[j]["HostelMasterFK"]);
                        string guestclgcode = Convert.ToString(ds.Tables[0].Rows[j]["CollegeCode"]);
                        rebate_days1 = 0;
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where  RebateType='1' and RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + guestcode + "'";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string rebate_days = "";
                                string rebate_Amount = "";
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                double rebateamt1 = 0;
                                double guestrebateamt = 0;
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    rebate_days1 = rebate_days1 + Convert.ToInt32(rebate_days);
                                    Session["guestrebateday"] = guestrebateday;
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt1 = Convert.ToDouble(rebate_Amount);
                                    guestrebateamt = guestrebateamt + rebateamt1;
                                    Session["guestrebateamt"] = guestrebateamt;
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            double rebateamt1 = 0;
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + guestcode + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + guesthostelcode + "')and RebateType='1' and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";
                                        ds3 = d2.select_method_wo_parameter(q9, "Text");
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            int guestgrantday = 0;
                                            string grant_day = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_days"]);
                                            string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_Amount"]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                guestgrantday += 1;
                                                rebate_days1 = rebate_days1 + Convert.ToDouble(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                double guestgrantamout = 0;
                                                rebateamt1 = Convert.ToDouble(grant_amt);
                                                guestgrantamout = guestgrantamout + rebateamt1;
                                                Session["guestgrantamout"] = guestgrantamout;
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                        }
                        if (Convert.ToString(rebate_days1).Trim() != "")
                        {
                        }
                        else
                        {
                            rebate_days1 = 0;
                        }
                        guestrebateday = guestrebateday + Convert.ToInt32(rebate_days1);
                        Session["guestrebateday"] = guestrebateday;
                        if (!guestgrant_hash.ContainsKey(guestcode))
                        {
                            guestgrant_hash.Add(guestcode, rebate_days1);
                        }
                    }
                }
                else
                {
                    guest_bool = true;
                }
            }
            int finalrebateday = guestrebateday + reb_days;
            finalvalue = calstaff + caldayscholour + totalstudent + calguest;//finalvalue no of student //staff+daysscholour+hosteler            Session["noofstudent"] = finalvalue;
            finalvalue = finalvalue - finalrebateday;
            Session["finalvalue"] = finalvalue;
            finalcalvalue = totalcalvalue / (finalvalue);
            Session["finalcalvalue"] = Convert.ToDouble(finalcalvalue);
        }
    }
    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string accountid = d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            string secondreciptqurey = "select receipt,finyear_start  from account_info where acct_id ='" + accountid + "' and (Header_id is null or Header_id ='') order by finyear_start desc";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur + 1;
                }
                string acronymquery = d2.GetFunction("select rept_acr from Finacode_settings where  college_code =" + collegecode1 + " and user_code =" + usercode + " and (Header_id='' or Header_id is null) order by modifydate desc");
                recacr = acronymquery;
                recno = recacr + Convert.ToString(receno);
            }
            return recno;
        }
        catch { return recno; }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    public string Gethostelcodenew(string messname)
    {
        string build = "";
        try
        {
            string itemname = "select distinct HostelMasterPK from HM_HostelMaster where MessMasterFK in ('" + messname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string build1 = ds.Tables[0].Rows[i]["HostelMasterPK"].ToString();
                    if (build1.Trim() != "")
                    {
                        if (build == "")
                        {
                            build = build1;
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return build;
    }
    protected void cb_groupname_CheckedChange(object sender, EventArgs e)
    {
        if (cb_groupname.Checked == true)
        {
            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                cbl_groupname.Items[i].Selected = true;
            }
            txt_groupname.Text = "Group Name(" + (cbl_groupname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_groupname.Items.Count; i++)
            {
                cbl_groupname.Items[i].Selected = false;
            }
            txt_groupname.Text = "--Select--";
        }
        //   cb_description_CheckedChange(sender, e);
        cbl_groupname_SelectedIndexChange(sender, e);
        //  binddescription();
    }
    protected void cbl_groupname_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_groupname.Text = "--Select--";
        cb_groupname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_groupname.Items.Count; i++)
        {
            if (cbl_groupname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_groupname.Text = "Group Name(" + commcount.ToString() + ")";
            if (commcount == cbl_groupname.Items.Count)
            {
                cb_groupname.Checked = true;
            }
        }
        //  cb_description_CheckedChange(sender, e);
        //  cbl_description_SelectedIndexChange(sender, e);
        // binddescription();
    }
    protected void bindaddgroup()
    {
        try
        {
            //ddl_group.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue from HT_HostelIncome hi,CO_MasterValues co where hi.IncomeGroup=co.MasterCode and MasterCriteria='HostelIncomeGRP' and collegecode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_group.DataSource = ds;
                //ddl_group.DataTextField = "TextVal";
                //ddl_group.DataValueField = "TextCode";
                //ddl_group.DataBind();
                //ddl_group.Items.Insert(0, new ListItem("Select", "0"));
                cbl_groupname.DataSource = ds;
                cbl_groupname.DataTextField = "MasterValue";
                cbl_groupname.DataValueField = "MasterCode";
                cbl_groupname.DataBind();
                if (cbl_groupname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_groupname.Items.Count; i++)
                    {
                        cbl_groupname.Items[i].Selected = true;
                    }
                    txt_groupname.Text = "Group Name(" + cbl_groupname.Items.Count + ")";
                }
                //  binddescription();
                //if (cbl_description.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_description.Items.Count; i++)
                //    {
                //        cbl_description.Items[i].Selected = true;
                //    }
                //    txt_description.Text = "Description(" + cbl_description.Items.Count + ")";
                //}
            }
            else
            {
                // ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void rdb_indmess_CheckedChange(object sender, EventArgs e)
    {
        if (rdb_indmess.Checked == true)
        {
            cb_addinco.Checked = false;
            cb_addex.Checked = false;
        }
    }
    protected void cb_addinco_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_addinco.Checked == true && rdb_commonmess.Checked == false)
            {
                lbl_groupname.Visible = true;
                updatepanel_groupname.Visible = true;
                bindaddgroup();
            }
            else
            {
                lbl_groupname.Visible = false;
                updatepanel_groupname.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void rdb_commonmess_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_commonmess.Checked == true)
            {
                lbl_groupname.Visible = false;
                updatepanel_groupname.Visible = false;
                cb_addinco.Checked = false;
                cb_addex.Checked = false;
                label_groupnameex.Visible = false;
                updatepanel2.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_groupnameex_CheckedChange(object sender, EventArgs e)
    {
        if (cb_groupnameex.Checked == true)
        {
            for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
            {
                cbl_groupnameex.Items[i].Selected = true;
            }
            txt_groupnameex.Text = "Group Name(" + (cbl_groupnameex.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
            {
                cbl_groupnameex.Items[i].Selected = false;
            }
            txt_groupnameex.Text = "--Select--";
        }
    }
    protected void cbl_groupnameex_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_groupnameex.Text = "--Select--";
        cb_groupnameex.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
        {
            if (cbl_groupnameex.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_groupnameex.Text = "Group Name(" + commcount.ToString() + ")";
            if (commcount == cbl_groupnameex.Items.Count)
            {
                cb_groupnameex.Checked = true;
            }
        }
    }
    protected void bindaddgroupex()
    {
        try
        {
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from HT_HostelExpenses ex,CO_MasterValues co where ex.ExpGroup=co.MasterCode and MasterCriteria='hostelexpgrp' and collegecode='" + collegecode1 + "' and ExpensesType='1' ";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_groupnameex.DataSource = ds;
                cbl_groupnameex.DataTextField = "MasterValue";
                cbl_groupnameex.DataValueField = "MasterCode";
                cbl_groupnameex.DataBind();
                if (cbl_groupnameex.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_groupnameex.Items.Count; i++)
                    {
                        cbl_groupnameex.Items[i].Selected = true;
                    }
                    txt_groupnameex.Text = "Group Name(" + cbl_groupnameex.Items.Count + ")";
                }
            }
            else
            {
                //ddl_group.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void cb_addex_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_addex.Checked == true && rdb_commonmess.Checked == false)
            {
                label_groupnameex.Visible = true;
                updatepanel2.Visible = true;
                bindaddgroupex();
            }
            else
            {
                label_groupnameex.Visible = false;
                updatepanel2.Visible = false;
            }
        }
        catch
        {
        }
    }
    #region mess bill amount magesh 12.3.18
    protected void lb_billsetting_Click(object sender, EventArgs e)
    {
        try
        {
            string sumpart = string.Empty;
            Divdivding.Visible = true;
            lbl_error1.Visible = false;
            Fpspread2.Visible = true;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 4;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.FpSpread fpSpread1 = new FarPoint.Web.Spread.FpSpread();
            FarPoint.Web.Spread.SheetView shv = new FarPoint.Web.Spread.SheetView();
            Fpspread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            Fpspread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[0].Width = 50;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[1].Width = 80;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Month";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[2].Width = 90;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostel Name";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[3].Width = 150;

            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int fpcol = 0; fpcol < ds.Tables[0].Rows.Count; fpcol++)
                {
                    Fpspread2.Sheets[0].ColumnCount++;
                    int col = Fpspread2.Sheets[0].ColumnCount - 1;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].Text = Convert.ToString(ds.Tables[0].Rows[fpcol]["StudentTypeName"]).Trim();
                    int tag;
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[fpcol]["StudentType"]).Trim(), out tag);
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].Tag = tag - 1;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[col].Width = 110;
                }
            }

            int divyear = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            int divyears = divyear - 1;
            string query = "select * from Ft_Messbill_Calculation where year in('" + divyear + "','" + divyears + "')order by hostel_code, month,year";
            DataSet dm = new DataSet();
            Hashtable hs = new Hashtable();
            hs.Clear();
            dm = d2.select_method_wo_parameter(query, "TEXT");

            if (dm.Tables.Count > 0 && dm.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dm.Tables[0].Rows.Count; row++)
                {
                    string year = Convert.ToString(dm.Tables[0].Rows[row]["year"]).Trim();
                    string mon = Convert.ToString(dm.Tables[0].Rows[row]["Month"]).Trim();
                    string month = returnMonYear(mon);
                    string hos_code = Convert.ToString(dm.Tables[0].Rows[row]["Hostel_Code"]).Trim();
                    string hostelname = " select HostelName from HM_HostelMaster where HostelMasterPK ='" + Convert.ToString(dm.Tables[0].Rows[row]["Hostel_Code"]).Trim() + "' order by hostelname";
                    DataSet hos = new DataSet();
                    hos = d2.select_method_wo_parameter(hostelname, "text");
                    string hos_name = string.Empty;
                    if (hos.Tables.Count > 0 && hos.Tables[0].Rows.Count > 0)
                    {
                        hos_name = Convert.ToString(hos.Tables[0].Rows[0]["HostelName"]).Trim();
                    }
                    if (!hs.ContainsKey(year) || !hs.ContainsKey(month) || !hs.ContainsKey(hos_code))
                    {
                        hs.Clear();
                        hs.Add(year, Convert.ToString(dm.Tables[0].Rows[row]["year"]).Trim());
                        hs.Add(month, Convert.ToString(dm.Tables[0].Rows[row]["Month"]).Trim());
                        hs.Add(hos_code, hos_name);
                        //hs.Add(Convert.ToString(dm.Tables[0].Rows[row]["Month"]).Trim(), Convert.ToString(dm.Tables[0].Rows[row]["Month"]).Trim());
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Fpspread2.Sheets[0].RowCount).Trim();
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = year.ToString();
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = month;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = hos_name.ToString();
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                        //string stutype =d2.GetFunction( "select StudentTypeName from HostelStudentType where StudentType='" + Convert.ToString(dm.Tables[0].Rows[row]["type"]).Trim() + "'and  CollegeCode='" + collegecode1 + "'");
                        dm.Tables[0].DefaultView.RowFilter = "Hostel_Code='" + Convert.ToString(dm.Tables[0].Rows[row]["Hostel_Code"]).Trim() + "' and Month='" + mon + "' and year='" + year + "'";
                        DataView clgfilter = dm.Tables[0].DefaultView;

                        if (clgfilter.Count > 0)
                        {
                            for (int i = 4; i < Fpspread2.Sheets[0].ColumnCount; i++)
                            {
                                for (int col = 0; col < clgfilter.Count; col++)
                                {
                                    if (Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, i].Tag) == Convert.ToString(clgfilter[col]["type"]).Trim())
                                    {
                                        double money = 0.00;
                                        double.TryParse(Convert.ToString(clgfilter[col]["Amount"]).Trim(), out money);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, i].Text = money.ToString();
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            Fpspread2.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    //magesh 12.3.18
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";//
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }


    protected void bindhostel()
    {
        try
        {
            cbl_hostelname1.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname1.DataSource = ds;
                cbl_hostelname1.DataTextField = "HostelName";
                cbl_hostelname1.DataValueField = "HostelMasterPK";
                cbl_hostelname1.DataBind();
            }
            else
            {

                txt_hostelname1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    public void cb_hostelname1_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_hostelname1.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname1.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                    {
                        if (cb_hostelname1.Checked == true)
                        {
                            cbl_hostelname1.Items[i].Selected = true;
                            txt_hostelname1.Text = "Hostel(" + (cbl_hostelname1.Items.Count) + ")";
                            build1 = cbl_hostelname1.Items[i].Value.ToString();
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
            }
            else
            {
                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    cbl_hostelname1.Items[i].Selected = false;
                    txt_hostelname1.Text = "--Select--";

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    public void cbl_hostelname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname1.Checked = false;
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            txt_hostelname1.Text = "--Select--";
            for (i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname1.Checked = false;
                    build = cbl_hostelname1.Items[i].Value.ToString();
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
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname1.Items.Count)
                {
                    cb_hostelname1.Checked = true;
                }
                txt_hostelname1.Text = "Hostel Name(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dssq = new DataSet();
            lbl_error1.Visible = false;
            string valhostel = string.Empty;
            double amount = 0.0;
            //double stuam =Convert.ToDouble(Txtamount).ToString();
            double.TryParse(Convert.ToString(Txtamount.Text).Trim(), out amount);
            valhostel = rs.GetSelectedItemsValueAsString(cbl_hostelname1);
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            string studmesstype = Convert.ToString(messtype - 1);
            if (valhostel != "")
            {
                if (Convert.ToString(ddlmonth.SelectedItem).Trim() != "Select" && ddlyear.SelectedValue != "" && ddlyear.SelectedValue != "Select")
                {
                    for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                    {
                        if (cbl_hostelname1.Items[i].Selected == true)
                        {
                            string sql = "if exists(select * from Ft_Messbill_Calculation where Hostel_Code ='" + cbl_hostelname1.Items[i].Value + "' and Month='" + ddlmonth.SelectedValue + "'and year='" + ddlyear.SelectedValue + "' and   type='" + studmesstype + "') update Ft_Messbill_Calculation set Amount='" + amount + "'where Hostel_Code ='" + cbl_hostelname1.Items[i].Value + "' and Month='" + ddlmonth.SelectedValue + "'and year='" + ddlyear.SelectedValue + "' and   type='" + studmesstype + "' else INSERT INTO Ft_Messbill_Calculation(Hostel_Code,Month,year,type,Amount) VALUES('" + cbl_hostelname1.Items[i].Value + "','" + ddlmonth.SelectedValue + "','" + ddlyear.SelectedValue + "','" + studmesstype + "','" + amount + "')  ";
                            int ins1 = d2.update_method_wo_parameter(sql, "Text");
                        }
                    }
                    lb_billsetting_Click(sender, e);
                }
                else
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Select All Field";
                }
            }
            else
            {
                lbl_error1.Visible = true;
                lbl_error1.Text = "Please Select The Hostel Name";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    #endregion

    #region nondivided magesh 21.3.18
    protected void nondiv(ref double TotalperdayAmtnondiv, ref double TotalCountnondiv, ref double NonvegPerdayAmtnondiv, ref double NonvegCountnondiv, ref double VegCountnondiv, ref double VegExpansestotalnondiv, ref double NonvegExpanceTotalnondiv, ref double CommonExpancesnondiv)
    {
        try
        {
            #region variable
            //magesh 21.3.18
            double consumtiontotal = 0;
            double VegConsumtiontotal = 0;
            double NonVegConsumtiontotal = 0;
            //double VegExpansestotal = 0;
            //double NonvegExpanceTotal = 0;
            double incometotal = 0;
            double totalcalvalue = 0;
            double noofstudent = 0;
            double noofstudent1 = 0;
            double Vegstudent = 0;
            double NonVegstudent = 0;
            double totalstudent = 0;
            double finalcalvalue = 0;
            double stafftotal = 0;
            double daysholour = 0;
            double calstaff = 0;
            double caldayscholour = 0;
            double finalvalue = 0;
            double guesttotal = 0;
            double calguest = 0;
            double vegCalvalue = 0;
            double NonvegCalvalue = 0;
            int HostlerVEGStaffTotal = 0;
            int HostlerNONVEGStaffTotal = 0;
            double HostlerStaffTotalCal = 0;
            double HostlerStaffTotal = 0;
            double HostlerStaffTotalCheckval = 0;
            double VegExpansestotal = 0;
            double NonvegExpanceTotal = 0;
            double CommonExpances = 0;
            int reb_days = 0;
            int vegRebDays = 0;
            int messrebeat = 0;
            int NonvegRebDays = 0;
            int reb_amount = 0;
            int vegRebAmt = 0;
            int messRebAmt = 0;
            int NonvegRebAmt = 0;

            #endregion
            // hat.Add("@hostel", Convert.ToString(Session["hoscode"]));
            hatndndiv.Add("@MessmasterFK", Convert.ToString(Session["Messidcode"]));
            hatndndiv.Add("@HostelMasterfk", Convert.ToString(ViewState["fixdivded"]));
            hatndndiv.Add("@ConsumptionFromDate", dt.ToString("MM/dd/yyyy"));
            hatndndiv.Add("@ConsumptionToDate", dt1.ToString("MM/dd/yyyy"));
            hatndndiv.Add("@Admdate", dt1.ToString("MM/dd/yyyy"));
            hatndndiv.Add("@month", Convert.ToString(Session["monthvalue"]));
            hatndndiv.Add("@year", Convert.ToString(Session["year"]));
            DataSet dsnonmess = new DataSet();
            dsnonmess = d2.select_method("MessbillnonfixedCalcultion", hatndndiv, "sp");


            string collhostel = Convert.ToString(ViewState["fixdivded"]);
            string[] spl = collhostel.Split(',');
            for (int i = 0; i < spl.Count(); i++)
            {

                string stu_messtype = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";//where CollegeCode='" + collegecode1 + "'
                DataView dvfilter = new DataView();
                DataView dvhsstu = new DataView();
                DataView dvhsstaff = new DataView();
                DataView dvhsgue = new DataView();
                DataSet stumess = new DataSet();
                DataView dvdsstu = new DataView();
                DataView dvdsstaff = new DataView();
                stumess = d2.select_method_wo_parameter(stu_messtype, "text");
                for (int mes = 0; mes < stumess.Tables[0].Rows.Count; mes++)
                {
                    int mess_ty;
                    int.TryParse(Convert.ToString(stumess.Tables[0].Rows[mes]["StudentType"]).Trim(), out mess_ty);
                    mess_ty -= 1;

                    // consumtiontotal = dsnonmess.Tables[0].Rows.Count;
                    if (dsnonmess.Tables[0].Rows.Count > 0)
                    {
                        consumtiontotal = Convert.ToDouble(dsnonmess.Tables[0].Compute("Sum(amount)", ""));

                        dsnonmess.Tables[0].DefaultView.RowFilter = "Hostel_Code='" + spl[i].ToString() + "' and type='" + mess_ty + "'";

                        dvfilter = dsnonmess.Tables[0].DefaultView;
                        int messtyp = mess_ty;
                        if (dvfilter.Count > 0)
                        {
                            double amou = Convert.ToDouble(dvfilter[0]["amount"]);
                            //consumtiontotal = Convert.ToDouble(dvfilter.Table.Compute("Sum(consume_total)");
                            string monthyerar=string.Empty;
                            double monyear=0.0;
                            selectmonth(Convert.ToString(Session["monthvalue"]), ref monthyerar, Convert.ToString(Session["year"]));
                            double.TryParse(monthyerar, out monyear);
                            amou = amou / monyear;
                            typamo.Add(mess_ty, amou);
                        }
                    }
                    if (dsnonmess.Tables[3].Rows.Count > 0)
                    {
                        dsnonmess.Tables[3].DefaultView.RowFilter = "HostelMasterFK='" + spl[i].ToString() + "' and StudMessType='" + mess_ty + "'";
                        totalstudent = Convert.ToDouble(dsnonmess.Tables[3].Rows.Count);
                        dvhsstu = dsnonmess.Tables[3].DefaultView;
                        if (dvhsstu.Count > 0)
                        {
                            string hsstu = Convert.ToString(dvhsstu.Count).Trim();
                            typhsstu.Add(mess_ty, hsstu);
                        }
                    }
                    if (dsnonmess.Tables[4].Rows.Count > 0)
                    {
                        dsnonmess.Tables[4].DefaultView.RowFilter = "HostelMasterFK='" + spl[i].ToString() + "' and StudMessType='" + mess_ty + "'";
                        HostlerStaffTotal = Convert.ToDouble(dsnonmess.Tables[4].Rows.Count);
                        dvhsstaff = dsnonmess.Tables[4].DefaultView;
                        if (dvhsstaff.Count > 0)
                        {
                            string hsstaf = Convert.ToString(dvhsstaff.Count).Trim();
                            typhsstaf.Add(mess_ty, hsstaf);
                        }
                    }
                    if (dsnonmess.Tables[5].Rows.Count > 0)
                    {
                        dsnonmess.Tables[5].DefaultView.RowFilter = "HostelMasterFK='" + spl[i].ToString() + "' and StudMessType='" + mess_ty + "'";
                        guesttotal = Convert.ToDouble(dsnonmess.Tables[5].Rows.Count);
                        dvhsgue = dsnonmess.Tables[5].DefaultView;
                        if (dvhsgue.Count > 0)
                        {
                            string hsgues = Convert.ToString(dvhsgue.Count).Trim();
                            typhsgue.Add(mess_ty, hsgues);
                        }
                    }
                    if (dsnonmess.Tables[6].Rows.Count > 0)
                    {
                        dsnonmess.Tables[6].DefaultView.RowFilter = "Hostel_Code='" + spl[i].ToString() + "' and StudMessType='" + mess_ty + "'";
                        dvdsstaff = dsnonmess.Tables[6].DefaultView;
                        stafftotal = Convert.ToDouble(dsnonmess.Tables[6].Rows.Count);
                        if (dvdsstaff.Count > 0)
                        {
                            string dsstaf = Convert.ToString(dvdsstaff.Count).Trim();
                            typdsstaf.Add(mess_ty, dsstaf);
                        }
                    }
                    if (dsnonmess.Tables[7].Rows.Count > 0)
                    {
                        dsnonmess.Tables[7].DefaultView.RowFilter = "Hostel_Code='" + spl[i].ToString() + "' and StudMessType='" + mess_ty + "'";
                        daysholour = Convert.ToDouble(dsnonmess.Tables[7].Rows.Count);
                        dvdsstu = dsnonmess.Tables[7].DefaultView;
                        if (dvdsstu.Count > 0)
                        {
                            string dsstu = Convert.ToString(dvdsstu.Count).Trim();
                            typdsstu.Add(mess_ty, dsstu);
                        }
                    }
                }

                string rabat = "select IncludeRebate  from HM_HostelMaster where HostelMasterPK in('" + spl[i].ToString() + "')";
                ds2 = d2.select_method_wo_parameter(rabat, "Text");
                string isrebate1 = "";
                if (ds2.Tables[0].Rows.Count > 0)
                    isrebate1 = Convert.ToString(ds2.Tables[0].Rows[0]["IncludeRebate"]);
                if (isrebate1.Trim() == "True")//isrebate1.Trim() != "" && isrebate1.Trim() == "null" &&
                    isbate = true;
                else
                    isbate = false;

                totalcalvalue = consumtiontotal;
            }
            if (cb_hosteler.Checked)
            {
                #region Hosteler
                Vegstudent = noofstudent * days;
                NonVegstudent = noofstudent1 * days;
                totalstudent = Vegstudent + NonVegstudent;
                clgcode = Convert.ToString(Session["clgcode"]);
                string q6 = "select r.App_No,h.HostelMasterFK,r.college_code,h.studmesstype from HT_HostelRegistration h,Registration r where h.APP_No=r.App_No and ISNULL(IsVacated,'0')=0 and ISNULL(IsDiscontinued,'0')=0 and ISNULL(IsVacated,'0')=0 and memtype='1' and h.HostelMasterFK in('" + collhostel + "') and HostelAdmDate<='" + dt1.ToString("MM/dd/yyyy") + "'";//barath 21.10.17 adm date added
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(q6, "Text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                    {
                        string roll1 = Convert.ToString(ds3.Tables[0].Rows[j][0]);
                        string hostelcode3 = Convert.ToString(ds3.Tables[0].Rows[j][1]);
                        string collegecode3 = Convert.ToString(ds3.Tables[0].Rows[j][2]);
                        string rebate_days = "";
                        string rebate_Amount = "";
                        string grantday1 = "";
                        double rebateamt = 0;
                        if (rdb_indivula.Checked == true)
                        {
                         string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + roll1 + "'"; //RebateType='1' and //22.12.17 barath
                            //string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail rb,ht_hostelregistration h  where rb.app_no=h.app_no and h.studmesstype='" + Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]) +"' and RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + roll1 + "'"; 
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                   
                                    grantday1 = Convert.ToString(rebate_days);
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt = Convert.ToDouble(rebate_Amount);
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            #region Commom
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            string grant_day = "";
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + roll1 + "' AND AttnMonth  = '" + dt2.ToString("MM") + "' AND AttnYear = '" + dt2.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        DataSet newdataset = new DataSet();
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + collhostel + "')  and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1'
                                        newdataset = d2.select_method_wo_parameter(q9, "Text");
                                        if (newdataset.Tables[0].Rows.Count > 0)
                                        {
                                            grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                            string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                            if (count != 0)
                            {
                                DataSet newdataset = new DataSet();
                                string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + collhostel + "') and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1' 
                                newdataset = d2.select_method_wo_parameter(q9, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    grant_day = Convert.ToString(newdataset.Tables[0].Rows[0][0]);
                                    string grant_amt = Convert.ToString(newdataset.Tables[0].Rows[0][1]);
                                    if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                    {
                                        grantday1 = grantday1 + Convert.ToInt32(grant_day);
                                    }
                                    if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                    {
                                        rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                    }
                                }
                            }
                            #endregion
                        }
                        if (grantday1.Trim() == "")
                        {
                            grantday1 = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(grantday1);
                        reb_amount = reb_amount + Convert.ToInt32(rebateamt);
                        string stummess_type = Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]).Trim();
                        //messrebeat += Convert.ToInt32(grantday1);
                        //messRebAmt += Convert.ToInt32(rebateamt);
                        //htrebam.Add(stummess_type, messRebAmt);
                        //htrebday.Add(stummess_type, messrebeat);

                            
                        
                        //if (Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]) == "0")//22.12.17 barath
                        //{
                        //    vegRebDays += Convert.ToInt32(grantday1);
                        //    vegRebAmt += Convert.ToInt32(rebateamt);
                        //}
                        //else if (Convert.ToString(ds3.Tables[0].Rows[j]["studmesstype"]) == "1")
                        //{
                        //    NonvegRebDays += Convert.ToInt32(grantday1);
                        //    NonvegRebAmt += Convert.ToInt32(rebateamt);
                        //}
                        //else
                        //{
                        //    NonvegRebDays += Convert.ToInt32(grantday1);
                        //    NonvegRebAmt += Convert.ToInt32(rebateamt);
                        //}
                        if (!grantday_hash.ContainsKey(roll1))
                            grantday_hash.Add(roll1, grantday1);
                        if (!Rebateamount_hash.ContainsKey(roll1))
                            Rebateamount_hash.Add(roll1, rebateamt);
                   
                    }
                }
                else
                {
                    hostel_bool = true;
                }
                #endregion
            }
            if (cb_instaff.Checked)
            {
                calstaff = stafftotal * days;
                
            }
            if (cb_dayssch.Checked)
            {
                caldayscholour = daysholour * days;
              
            }
            if (cbHostlerStaff.Checked)
            {
                HostlerStaffTotalCheckval = HostlerStaffTotal;
                HostlerStaffTotalCal = HostlerStaffTotal * days;
               
            }
            if (cb_guest.Checked)
            {
                #region Guest
                calguest = guesttotal * days;
                string q6 = " select distinct gr.APP_No, gr.HostelMasterFK,hd.CollegeCode from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and HostelMasterFK in('" + collhostel + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q6, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        string guestcode = Convert.ToString(ds.Tables[0].Rows[j]["APP_No"]);
                        string guesthostelcode = Convert.ToString(ds.Tables[0].Rows[j]["HostelMasterFK"]);
                        string guestclgcode = Convert.ToString(ds.Tables[0].Rows[j]["CollegeCode"]);
                        string rebateday = "";
                        double rebateamt = 0;
                        if (rdb_indivula.Checked == true)
                        {
                            string q7 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HT_HostelRebateDetail  where  RebateFromDate>='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate<= '" + dt1.ToString("MM/dd/yyyy") + "' and App_No='" + guestcode + "'";// RebateType='1' and
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(q7, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string rebate_days = "";
                                string rebate_Amount = "";
                                rebate_days = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                                rebate_Amount = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                                if (rebate_days.Trim() != "" && rebate_days.Trim() != null)
                                {
                                    // reb_days = reb_days + Convert.ToInt32(rebate_days);
                                    rebateday = Convert.ToString(rebate_days);
                                }
                                if (rebate_Amount.Trim() != "" && rebate_Amount.Trim() != null)
                                {
                                    rebateamt = Convert.ToDouble(rebate_Amount);
                                }
                            }
                        }
                        else if (rdb_common.Checked == true)
                        {
                            int count = 0;
                            DateTime dt2 = new DateTime();
                            dt2 = dt;
                            while (dt2 <= dt1)
                            {
                                int fdate = Convert.ToInt32(dt2.ToString("dd"));
                                int tdate = Convert.ToInt32(dt1.ToString("dd"));
                                string attend = "[d" + fdate + "]";
                                string q8 = "SELECT App_no," + attend + " FROM HT_Attendance WHERE app_no = '" + guestcode + "' AND AttnMonth  = '" + dt.ToString("MM") + "' AND AttnYear = '" + dt.ToString("yyyy") + "' and " + attend + "=2";
                                ds2.Clear();
                                ds2 = d2.select_method_wo_parameter(q8, "Text");
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    count += 1;
                                }
                                else
                                {
                                    if (count != 0)
                                    {
                                        string q9 = " SELECT SUM(RebateDays)as rebate_days,SUM(RebateAmount)as rebate_Amount FROM HM_RebateMaster where HostelFK in('" + guesthostelcode + "')  and RebateMonth = '" + dt2.ToString("MM") + "' and RebateActDays='" + count + "'";//and RebateType='1'
                                        ds3 = d2.select_method_wo_parameter(q9, "Text");
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            string grant_day = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_days"]);
                                            string grant_amt = Convert.ToString(ds3.Tables[0].Rows[0]["rebate_Amount"]);
                                            if (grant_day.Trim() != "" && grant_day.Trim() != null)
                                            {
                                                rebateday = rebateday + Convert.ToInt32(grant_day);
                                            }
                                            if (grant_amt.Trim() != "" && grant_amt.Trim() != null)
                                            {
                                                rebateamt = rebateamt + Convert.ToDouble(grant_amt);
                                            }
                                        }
                                    }
                                    count = 0;
                                }
                                dt2 = dt2.AddDays(1);
                            }
                        }
                        if (rebateday.Trim() == "")
                        {
                            rebateday = "0";
                        }
                        reb_days = reb_days + Convert.ToInt32(rebateday);
                        reb_amount = reb_amount + Convert.ToInt32(rebateamt);
                        if (!guestgrant_hash.ContainsKey(guestcode))
                        {
                            guestgrant_hash.Add(guestcode, rebateday);
                        }
                        if (!Rebateamount_hash.ContainsKey(guestcode))
                        {
                            guestRebateamount_hash.Add(guestcode, rebateamt);
                        }
                    }
                }
                else
                {
                    guest_bool = true;
                }
                #endregion
            }
            string stu_messtypes = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";//where CollegeCode='" + collegecode1 + "'
            DataSet dsmes = new DataSet();
            int mestyp = 0;
            dsmes = d2.select_method_wo_parameter(stu_messtypes, "Text");
            if (dsmes.Tables.Count > 0 && dsmes.Tables[0].Rows.Count > 0)
            {
                for (int mes = 0; mes < dsmes.Tables[0].Rows.Count; mes++)
                {
                    string mem = Convert.ToString(dsmes.Tables[0].Rows[mes]["StudentType"]).Trim();
                   
                     int.TryParse(mem,out mestyp);
                     mestyp -= 1;
                     TotalCountnondiv = Convert.ToDouble(typhsstu[mestyp]) + Convert.ToDouble(typhsstaf[mestyp]) + Convert.ToDouble(typhsgue[mestyp]) + Convert.ToDouble(typdsstaf[mestyp]) + Convert.ToDouble(typdsstu[mestyp]);
                     totcount.Add(mestyp, TotalCountnondiv);
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "inv_mess_bill_setting");
        }
    }
    #endregion
    protected void rdb_fix_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            rdb_div.Visible = true;
            rdb_nondiv.Visible = true;


        }
        catch
        {
        }
    }

    public string selectmonth(string month, ref string monthyear, string year)
    {
       
         switch (month)
        {
            case "1":
                monthyear = "31";
                break;
            case "2":
                int ms = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                monthyear = Convert.ToString(ms);
                break;
            case "3":
                monthyear = "31";
                break;
            case "4":
                monthyear = "30";
                break;
            case "5":
                monthyear = "31";
                break;
            case "6":
                monthyear = "30";
                break;
            case "7":
                monthyear = "31";
                break;
            case "8":
                monthyear = "31";
                break;
            case "9":
                monthyear = "30";
                break;
            case "10":
                monthyear = "31";
                break;
            case "11":
                monthyear = "30";
                break;
            case "12":
                monthyear = "31";
                break;
        }
        return monthyear;
    }
    
}