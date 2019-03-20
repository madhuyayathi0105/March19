using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class HR_Year_Alter : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dg = new DataSet();
    DAccess2 d2 = new DAccess2();
    string CollegeCode = string.Empty;
    static int countnew = 0;

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
            CollegeCode = Convert.ToString(Session["collegecode"]);
            if (!IsPostBack)
            {
                rb_leave.Checked = true;
                rb_Payprocess.Checked = false;
                bindcollege();
                rptprint.Visible = false;
                btn_go_Click(sender, e);
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        lblalerterr.Visible = false;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
        rb_leave.Checked = true;
        rb_Payprocess.Checked = false;
        btn_go_Click(sender, e);
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
        rb_leave.Checked = true;
        rb_Payprocess.Checked = false;
        btn_go_Click(sender, e);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
        txtexcelname.Text = "";
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "HR Year Report";
            string pagename = "HR_Year_Alter.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void txtdatestart_Change(object sender, EventArgs e)
    {
        try
        {
            string yearstart = txtdatestart.Text;
            string yearend = txtdateend.Text;
            DateTime dt = getspldt(yearstart);

            int strtmonth = dt.Month;
            int year = dt.Year;
            int Eyear = year + 1;
            DateTime currdt = DateTime.Now;
            DateTime newdt = DateTime.Now;
            int curryear = currdt.Year;
            string collcode = Convert.ToString(ddlcoll.SelectedItem.Value);

            if (((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)))
            {
                newdt = dt.AddDays(365);
                if (newdt.Month <= 2)
                {
                    newdt = dt.AddDays(364);
                }
            }
            else
            {
                if ((Eyear % 4 == 0 && Eyear % 100 != 0) || (Eyear % 400 == 0))
                {
                    newdt = dt.AddDays(365);
                    if (newdt.Month <= 2)
                    {
                        newdt = dt.AddDays(364);
                    }
                }
                else
                {
                    newdt = dt.AddDays(364);
                }
            }

            txtdateend.Text = newdt.ToString("dd/MM/yyyy");
            int endmonth = newdt.Month;
            int endyear = newdt.Year;
            TimeSpan ts = newdt - dt;

            string slno = "";
            FpSpread1.SaveChanges();
            for (int ro = 0; ro < FpSpread1.Sheets[0].RowCount; ro++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[ro, 3].Value);
                if (check == 1)
                {
                    slno = Convert.ToString(FpSpread1.Sheets[0].Cells[ro, 1].Tag);
                }
            }
            string selquery = "";
            int count = 0;
            if (btnsave.Visible == true)
            {
                selquery = "select hryear_start,hryear_end from hryears where collcode='" + collcode + "'";
            }
            else
            {
                selquery = " select hryear_start,hryear_end from hryears where collcode='" + collcode + "' and slno in('" + slno + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTime getdt = Convert.ToDateTime(ds.Tables[0].Rows[i]["hryear_start"]);
                    DateTime getenddt = Convert.ToDateTime(ds.Tables[0].Rows[i]["hryear_end"]);
                    int getyear = getdt.Year;
                    int getendyear = getenddt.Year;
                    int getmonth = getdt.Month;
                    int getendmonth = getenddt.Month;
                    if ((endyear == getendyear && (strtmonth >= getmonth || endmonth <= getendmonth)))
                    {
                        count++;
                    }
                }
            }
            //if (count > 0)
            //{
            //    lbldateerr.Visible = true;
            //    lbldateerr.Text = "HR Year Already Exist!";
            //    btnsave.Enabled = false;
            //    btnupdate.Enabled = false;
            //    btndelete.Enabled = false;
            //}
            //else
            //{
            //    lbldateerr.Visible = false;
            //    btnsave.Enabled = true;
            //    btnupdate.Enabled = true;
            //    btndelete.Enabled = true;
            //}
        }
        catch { }
    }

    protected void txtdateend_Change(object sender, EventArgs e)
    {
        try
        {
            string yearstart = txtdatestart.Text;
            string yearend = txtdateend.Text;
            DateTime dt = getspldt(yearstart);
            DateTime newdt = dt.AddMonths(12).AddDays(-1);
            txtdateend.Text = newdt.ToString("dd/MM/yyyy");

            int startyear = dt.Year;
            if (newdt < dt)
            {
                lbldateerr.Visible = true;
                lbldateerr.Text = "End Date Less Than Start Date";
            }
            else
            {
                lbldateerr.Visible = false;
            }
        }
        catch { }
    }

    public bool checkedOK()
    {
        bool Ok = false;
        FpSpread1.SaveChanges();
        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 3].Value);
            if (check == 1)
            {
                Ok = true;
                FpSpread1.Sheets[0].Cells[i, 0].BackColor = ColorTranslator.FromHtml("#00CC00");
                FpSpread1.Sheets[0].Cells[i, 1].BackColor = ColorTranslator.FromHtml("#00CC00");
                FpSpread1.Sheets[0].Cells[i, 2].BackColor = ColorTranslator.FromHtml("#00CC00");
            }
            else
            {
                FpSpread1.Sheets[0].Cells[i, 0].BackColor = ColorTranslator.FromHtml("White");
                FpSpread1.Sheets[0].Cells[i, 1].BackColor = ColorTranslator.FromHtml("White");
                FpSpread1.Sheets[0].Cells[i, 2].BackColor = ColorTranslator.FromHtml("White");
            }
        }
        return Ok;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            bool check = false;
            string[] splval = new string[3];

            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    int val = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 3].Value);
                    if (val == 1)
                    {
                        string sno_Yr = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                        string getdate = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Text);
                        splval = getdate.Split('-');
                        string frmdate = Convert.ToString(splval[0]);
                        string todate = Convert.ToString(splval[1]);
                        DateTime dt = getspldt(frmdate);
                        DateTime newdt = getspldt(todate);
                        string actid = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                        string actcole = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                        string updq = "Update HrPayMonths set SelStatus='0' where College_Code='" + actcole + "'";
                        int upcount = d2.update_method_wo_parameter(updq, "Text");
                        string insertquery = "update HrPayMonths set SelStatus='1' where College_Code='" + actcole + "' and From_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' and To_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' and hryearkey='" + sno_Yr + "'";
                        int inscount = d2.update_method_wo_parameter(insertquery, "Text");
                        if (inscount > 0)
                        {
                            check = true;
                        }
                        if (checkedOK())
                        {
                            FpSpread1.Sheets[0].Cells[row, 0].BackColor = ColorTranslator.FromHtml("#00CC00");
                            FpSpread1.Sheets[0].Cells[row, 1].BackColor = ColorTranslator.FromHtml("#00CC00");
                            FpSpread1.Sheets[0].Cells[row, 3].BackColor = ColorTranslator.FromHtml("#00CC00");
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[row, 0].BackColor = ColorTranslator.FromHtml("White");
                            FpSpread1.Sheets[0].Cells[row, 1].BackColor = ColorTranslator.FromHtml("White");
                            FpSpread1.Sheets[0].Cells[row, 3].BackColor = ColorTranslator.FromHtml("White");
                        }
                    }
                }

                if (check == true)
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "HR Year Selected Successfully";
                    alertpopwindow.Visible = true;
                    btn_go_Click(sender, e);
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please Select Any One Year";
                }
            }
        }
        catch { }
    }

    protected void btnmod_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            bool modcheck = false;
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    int val = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 3].Value);
                    if (val == 1)
                    {
                        bindcollegepop();
                        popper1.Visible = true;
                        btnsave.Visible = false;
                        btnupdate.Visible = true;
                        btndelete.Visible = true;
                        lbldateerr.Visible = false;
                        btnupdate.Enabled = true;
                        btndelete.Enabled = true;

                        string curractid = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                        string colcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);

                        string selectload = "select (Convert(Varchar(10),hryear_start,103)) as hryear_start,(Convert(varchar(10),hryear_end,103)) as hryear_end,Pay_Month from hryears where slno='" + curractid + "' and collcode='" + colcode + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectload, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            modcheck = true;
                            if (ddlcoll.Items.Count > 0)
                                ddlcoll.SelectedValue = colcode;
                            ddlcoll.Enabled = false;
                            txtdatestart.Text = ds.Tables[0].Rows[0]["hryear_start"].ToString();
                            txtdateend.Text = ds.Tables[0].Rows[0]["hryear_end"].ToString();
                            Session["frmdate"] = txtdatestart.Text;
                            Session["todate"] = txtdateend.Text;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["Pay_Month"]) == "True")
                            {
                                rb_monthfrm.Checked = true;
                                rb_monthto.Checked = false;
                            }
                            else
                            {
                                rb_monthfrm.Checked = false;
                                rb_monthto.Checked = true;
                            }
                        }
                    }
                }
                if (modcheck == true)
                {

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please Select Any One Year";
                }
            }
        }
        catch { }
    }

    protected void rb_leave_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btn_go_Click(sender, e);
        }
        catch { }
    }

    protected void rb_Payprocess_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btn_go_Click(sender, e);
        }
        catch { }
    }

    protected void rb_monthfrm_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void rb_monthto_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void rb_radleave_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void rb_radpaypro_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void ddlcol_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlcol.SelectedItem.Value);
        try
        {
            string query = "";
            string[] spldt = new string[2];
            string slno = string.Empty;

            query = "select (CONVERT(varchar(10), hryear_start,103)+' - '+CONVERT(varchar(10), hryear_end,103))as hrdate,(Convert(varchar(10),Pay_Start)+'-'+Convert(varchar(10),Pay_End)) as hrday,slno,collcode from hryears where collcode='" + collcode + "'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Divspread.Visible = true;
                lblerr.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 4;

                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Leave Process";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cb1.AutoPostBack = true;
                FpSpread1.Columns[3].CellType = cb1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Columns[1].Width = 400;
                FpSpread1.Columns[2].Width = 200;
                FpSpread1.Columns[3].Width = 134;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    spldt = Convert.ToString(ds.Tables[0].Rows[i]["hrdate"]).Split('-');
                    slno = Convert.ToString(ds.Tables[0].Rows[i]["slno"]);
                    DateTime getnewdt = getspldt(spldt[0]);
                    DateTime getnewdt1 = getspldt(spldt[1]);

                    string hrgetid = d2.GetFunction("select SelStatus from HrPayMonths where  College_Code='" + collcode + "' and From_Date between '" + getnewdt.ToString("MM/dd/yyyy") + "' and '" + getnewdt1.ToString("MM/dd/yyyy") + "' and To_Date between '" + getnewdt.ToString("MM/dd/yyyy") + "' and '" + getnewdt1.ToString("MM/dd/yyyy") + "'and hryearkey='" + slno + "'");
                   

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = slno;
                    FpSpread1.Columns[0].Locked = true;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["hrdate"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["slno"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Column.Width = 300;
                    FpSpread1.Columns[1].Locked = true;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["hrday"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["collcode"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                    FpSpread1.Columns[2].Locked = true;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Column.Width = 100;

                    if (hrgetid.Trim() == "True")
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Value = 1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#00CC00");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#00CC00");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#00CC00");
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Value = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("White");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("White");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("White");
                    }
                }
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Visible = true;
                rptprint.Visible = true;
                btndiv.Visible = true;
            }
            else
            {
                rptprint.Visible = false;
                btndiv.Visible = false;
                Divspread.Visible = false;
                FpSpread1.Visible = false;
                lblerr.Visible = true;
                lblerr.Text = "No Records Found";
            }

            if (rb_leave.Checked == true)
            {
                FpSpread1.Width = 610;
                FpSpread1.Height = 300;
            }
            else if (rb_Payprocess.Checked == true)
            {
                FpSpread1.Width = 610;
                FpSpread1.Height = 300;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "HR_Year_Alter.aspx");
        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        popper1.Visible = true;
        bindcollegepop();
        btnsave.Visible = true;
        btnupdate.Visible = false;
        btndelete.Visible = false;
        rb_monthfrm.Checked = true;
        rb_monthto.Checked = false;
        rb_radleave.Checked = true;
        rb_radpaypro.Checked = false;
        ddlcoll.SelectedIndex = ddlcoll.Items.IndexOf(ddlcoll.Items.FindByValue(ddlcol.SelectedItem.Value));
        txtdatestart.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdateend.Text = DateTime.Now.ToString("dd/MM/yyyy");
        lbldateerr.Visible = false;
        btnsave.Enabled = true;
        ddlcoll.Enabled = true;
    }

    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string activerow = "";
            string activecol = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            countnew = 0;

            if (activecol == "3")
            {
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), Convert.ToInt32(activecol)].Value) == 1)
                    {
                        countnew++;
                        if (countnew == 1)
                        {
                            FpSpread1.Sheets[0].Cells[i, 0].BackColor = ColorTranslator.FromHtml("#00CC00");
                            FpSpread1.Sheets[0].Cells[i, 1].BackColor = ColorTranslator.FromHtml("#00CC00");
                            FpSpread1.Sheets[0].Cells[i, 3].BackColor = ColorTranslator.FromHtml("#00CC00");
                        }
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[i, 0].BackColor = ColorTranslator.FromHtml("White");
                        FpSpread1.Sheets[0].Cells[i, 1].BackColor = ColorTranslator.FromHtml("White");
                        FpSpread1.Sheets[0].Cells[i, 3].BackColor = ColorTranslator.FromHtml("White");
                    }
                }
            }
            if (countnew > 1)
            {
                countnew--;
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Value = 0;
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].BackColor = Color.White;
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].BackColor = Color.White;
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].BackColor = Color.White;
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Only One HR Year Is Allowed";
            }
            FpSpread1.SaveChanges();
        }
        catch { }
    }

    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlcoll.SelectedItem.Value);
        try
        {
            string monchk = "";
            string yrstype = "";
            int count = 0;
            if (rb_monthfrm.Checked == true)
            {
                monchk = "1";
            }
            if (rb_monthto.Checked == true)
            {
                monchk = "0";
            }

            if (rb_radleave.Checked == true)
            {
                yrstype = "1";
            }
            if (rb_radpaypro.Checked == true)
            {
                yrstype = "2";
            }
            string yearstart = txtdatestart.Text;
            string yearend = txtdateend.Text;

            DateTime dt = getspldt(yearstart);
            int year = dt.Year;
            int stday = dt.Day;
            DateTime currdt = DateTime.Now;
            int curryear = currdt.Year;
            DateTime newdt = getspldt(yearend);
            int enday = newdt.Day;
            string strtst = "";
            string enddt = "";

            DateTime dtoddmon = DateTime.Now;
            DateTime dtfeb = DateTime.Now;
            DateTime dtevenmon = DateTime.Now;
            DateTime enddtnew = DateTime.Now;
            DateTime strtstnew = DateTime.Now;

            Hashtable ht1 = new Hashtable();
            Hashtable ht2 = new Hashtable();

            int FD = dt.Day;
            int FM = dt.Month;
            int FY = dt.Year;

            int TD = newdt.Day;
            int TM = newdt.Month;
            int TY = newdt.Year;

            int oddmon = 0;
            int feb = 0;
            int evenmon = 0;

            int oddday = 0;
            int febday = 0;
            int evenday = 0;
            int countday = 0;

            string selquery = "";
            if (rb_radleave.Checked == true)
            {
                selquery = "select * from hryears where hryear_start='" + dt.ToString("MM/dd/yyyy") + "' and collcode='" + collcode + "' ";
            }
            if (rb_radpaypro.Checked == true)
            {
                selquery = "select * from hryears where hryear_start='" + dt.ToString("MM/dd/yyyy") + "' and collcode='" + collcode + "' ";
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    lbldateerr.Visible = true;
            //    lbldateerr.Text = "HR year already exists!";
            //}
            if (dt == newdt)
            {
                lbldateerr.Visible = true;
                lbldateerr.Text = "Year must be exactly One Year!";
            }
            else
            {
                string insquery = "";
                int inscount = 0;
                string selq = "select top 1 slno from hryears order by slno desc";
                string yearsid = d2.GetFunction(selq);
                Int32 slno = Convert.ToInt32(yearsid) + 1;

                insquery = "Insert into hryears (hryear_start,hryear_end,Pay_Start,Pay_End,Pay_Month,collcode,slno) values ('" + dt.ToString("MM/dd/yyyy") + "','" + newdt.ToString("MM/dd/yyyy") + "','" + stday + "','" + enday + "','" + monchk + "','" + collcode + "','" + slno + "')";

                inscount = d2.update_method_wo_parameter(insquery, "Text");

                System.Globalization.DateTimeFormatInfo mfi = new
                System.Globalization.DateTimeFormatInfo();
                string strMonthName = "";
                int daysInJuly = 0;
                int newday = 0;

                //Checking Leap Year
                //Condition 1
                if ((FM > TM && FY != TY) || (TM > FM && FY != TY))
                {
                    for (int i = FM; i <= 12; i++)
                    {
                        newday = i;
                        if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                //insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "')";
                                //inscount = d2.update_method_wo_parameter(insquery, "Text");
                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                        else
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }

                    for (int j = 1; j <= TM; j++)
                    {
                        newday = j;
                        if ((TY % 4 == 0 && TY % 100 != 0) || (TY % 400 == 0))
                        {
                            if (j == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    TY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (j == 4 || j == 6 || j == 9 || j == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    TY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    TY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                        else
                        {
                            if (j == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    TY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (j == 4 || j == 6 || j == 9 || j == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    TY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    TY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }
                }

                //Condition 2

                if ((FM > TM && FY == TY) || (TM > FM && FY == TY))
                {
                    for (int i = FM; i <= 12; i++)
                    {
                        newday = i;
                        if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                        else
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','0','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }
                }

                //Condition 3

                if (TM == FM)
                {
                    for (int i = FM; i <= 12; i++)
                    {
                        newday = i;
                        if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                        else
                        {
                            if (i == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    FY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (i == 4 || i == 6 || i == 9 || i == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    FY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                strtst = newday + "/" + FD + "/" + FY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    FY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + FY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }

                    for (int j = 1; j < TM; j++)
                    {
                        newday = j;
                        if ((TY % 4 == 0 && TY % 100 != 0) || (TY % 400 == 0))
                        {
                            if (j == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    TY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (j == 4 || j == 6 || j == 9 || j == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    TY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    TY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                        else
                        {
                            if (j == 2)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                    newday = dtfeb.Month;
                                    TY = dtfeb.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','" + slno + "')";//delsi changed j to new day delsi 1307
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else if (j == 4 || j == 6 || j == 9 || j == 11)
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtst = newday + "/" + FD + "/" + TY;
                                strtstnew = Convert.ToDateTime(strtst);
                                dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                    newday = dtevenmon.Month;
                                    TY = dtevenmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','"+slno+"')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                strMonthName = mfi.GetMonthName(newday).ToString();
                                strtst = newday + "/" + FD + "/" + TY;
                                daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                strtstnew = Convert.ToDateTime(strtst);
                                dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                if (monchk == "0")
                                {
                                    strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                    daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                    newday = dtoddmon.Month;
                                    TY = dtoddmon.Year;
                                }

                                insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + collcode + "','" + TY + "','" + monchk + "','" + slno + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }
                }

                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                    lbldateerr.Visible = false;
                    clear();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "HR_Year_Alter.aspx");
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string monchk = "";
            string yrstype = "";
            string yearsid = "";
            string colid = "";
            int count = 0;
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    string sno = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                    int val = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 3].Value);
                    if (val == 1)
                    {
                        if (rb_monthfrm.Checked == true)
                        {
                            monchk = "1";
                        }
                        if (rb_monthto.Checked == true)
                        {
                            monchk = "0";
                        }

                        if (rb_radleave.Checked == true)
                        {
                            yrstype = "1";
                        }
                        if (rb_radpaypro.Checked == true)
                        {
                            yrstype = "2";
                        }
                        string yearstart = txtdatestart.Text;
                        string yearend = txtdateend.Text;

                        DateTime dt = getspldt(yearstart);
                        int year = dt.Year;
                        int stday = dt.Day;
                        DateTime currdt = DateTime.Now;
                        int curryear = currdt.Year;
                        DateTime newdt = getspldt(yearend);
                        int enday = newdt.Day;
                        string strtst = "";
                        string enddt = "";

                        string getoldfrmdt = Convert.ToString(Session["frmdate"]);
                        string getoldtodt = Convert.ToString(Session["todate"]);
                        DateTime oldfrmdt = getspldt(getoldfrmdt);
                        DateTime oldtodt = getspldt(getoldtodt);

                        DateTime dtoddmon = DateTime.Now;
                        DateTime dtfeb = DateTime.Now;
                        DateTime dtevenmon = DateTime.Now;
                        DateTime enddtnew = DateTime.Now;
                        DateTime strtstnew = DateTime.Now;

                        Hashtable ht1 = new Hashtable();
                        Hashtable ht2 = new Hashtable();

                        int FD = dt.Day;
                        int FM = dt.Month;
                        int FY = dt.Year;

                        int TD = newdt.Day;
                        int TM = newdt.Month;
                        int TY = newdt.Year;

                        int oddmon = 0;
                        int feb = 0;
                        int evenmon = 0;

                        int oddday = 0;
                        int febday = 0;
                        int evenday = 0;
                        int countday = 0;

                        string yyyy = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Text);
                        string[] a = yyyy.Split('-');
                        string aa = a[0];
                        string bb = a[1];
                        DateTime cc = getspldt(aa);
                        DateTime dd = getspldt(bb);
                        yearsid = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);
                        colid = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);

                        string selquery = "";
                        if (dt == newdt)
                        {
                            lbldateerr.Visible = true;
                            lbldateerr.Text = "Year must be exactly One Year!";
                        }
                        else
                        {
                            string updatequery = "";
                            int upscount = 0;
                            string insquery = "";
                            int inscount = 0;

                            string delq = " delete from HrPayMonths where From_Date between '" + oldfrmdt.ToString("MM/dd/yyyy") + "' and '" + oldtodt.ToString("MM/dd/yyyy") + "' and To_Date between '" + oldfrmdt.ToString("MM/dd/yyyy") + "' and '" + oldtodt.ToString("MM/dd/yyyy") + "' and College_Code='" + colid + "' and hryearkey='" + sno + "'";

                            int delcount = d2.update_method_wo_parameter(delq, "Text");

                            updatequery = "Update hryears set hryear_start='" + dt.ToString("MM/dd/yyyy") + "',hryear_end='" + newdt.ToString("MM/dd/yyyy") + "',Pay_Start='" + stday + "',Pay_End='" + enday + "',Pay_Month='" + monchk + "'  where hryear_start='" + cc.ToString("MM/dd/yyyy") + "' and hryear_end='" + dd.ToString("MM/dd/yyyy") + "' and collcode='" + colid + "' and slno='" + yearsid + "'";
                            upscount = d2.update_method_wo_parameter(updatequery, "Text");

                            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                            string strMonthName = "";
                            int daysInJuly = 0;
                            int newday = 0;

                            //Checking Leap Year
                            //Condition 1
                            if ((FM > TM && FY != TY) || (TM > FM && FY != TY))
                            {
                                for (int i = FM; i <= 12; i++)
                                {
                                    newday = i;
                                    if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                }

                                for (int j = 1; j <= TM; j++)
                                {
                                    newday = j;
                                    if ((TY % 4 == 0 && TY % 100 != 0) || (TY % 400 == 0))
                                    {
                                        if (j == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                TY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (j == 4 || j == 6 || j == 9 || j == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                TY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                TY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                    else
                                    {
                                        if (j == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                TY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (j == 4 || j == 6 || j == 9 || j == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                TY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                TY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                }
                            }

                            //Condition 2

                            if ((FM > TM && FY == TY) || (TM > FM && FY == TY))
                            {
                                for (int i = FM; i <= 12; i++)
                                {
                                    newday = i;
                                    if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,SelStatus,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','0','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                }
                            }

                            //Condition 3

                            if (TM == FM)
                            {
                                for (int i = FM; i <= 12; i++)
                                {
                                    newday = i;
                                    if ((FY % 4 == 0 && FY % 100 != 0) || (FY % 400 == 0))
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                    else
                                    {
                                        if (i == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                FY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (i == 4 || i == 6 || i == 9 || i == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                FY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(FY, newday);
                                            strtst = newday + "/" + FD + "/" + FY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(FY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                FY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + FY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                }

                                for (int j = 1; j < TM; j++)
                                {
                                    newday = j;
                                    if ((TY % 4 == 0 && TY % 100 != 0) || (TY % 400 == 0))
                                    {
                                        if (j == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtfeb = strtstnew.AddDays(29).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                TY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (j == 4 || j == 6 || j == 9 || j == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                TY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','" + sno + "')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                TY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                    else
                                    {
                                        if (j == 2)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            dtfeb = strtstnew.AddDays(28).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtfeb.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtfeb.Month);
                                                newday = dtfeb.Month;
                                                TY = dtfeb.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtfeb.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else if (j == 4 || j == 6 || j == 9 || j == 11)
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtst = newday + "/" + FD + "/" + TY;
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtevenmon = strtstnew.AddDays(30).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtevenmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtevenmon.Month);
                                                newday = dtevenmon.Month;
                                                TY = dtevenmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtevenmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                        else
                                        {
                                            strMonthName = mfi.GetMonthName(newday).ToString();
                                            strtst = newday + "/" + FD + "/" + TY;
                                            daysInJuly = System.DateTime.DaysInMonth(TY, newday);
                                            strtstnew = Convert.ToDateTime(strtst);
                                            dtoddmon = strtstnew.AddDays(31).AddDays(-1);
                                            if (monchk == "0")
                                            {
                                                strMonthName = mfi.GetMonthName(dtoddmon.Month).ToString();
                                                daysInJuly = System.DateTime.DaysInMonth(TY, dtoddmon.Month);
                                                newday = dtoddmon.Month;
                                                TY = dtoddmon.Year;
                                            }

                                            insquery = "insert into HrPayMonths (PayMonthNum,PayMonth,From_Date,To_Date,PayDays,College_Code,PayYear,MonthType,hryearkey) values('" + newday + "','" + strMonthName + "','" + strtstnew.ToString("MM/dd/yyyy") + "','" + dtoddmon.ToString("MM/dd/yyyy") + "','" + daysInJuly + "','" + colid + "','" + TY + "','" + monchk + "','"+sno+"')";
                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                        }
                                    }
                                }
                            }

                            if (upscount > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Updated Successfully";
                                lbldateerr.Visible = false;
                                clear();
                                Session["frmdate"] = "";
                                Session["todate"] = "";
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    private DateTime getspldt(string getdate)
    {
        DateTime dt = new DateTime();
        string[] newspl = new string[2];
        try
        {
            newspl = getdate.Split('/');
            dt = Convert.ToDateTime(newspl[1] + "/" + newspl[0] + "/" + newspl[2]);
        }
        catch { }
        return dt;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = true;
        lblconfirm.Visible = true;
        lblconfirm.Text = "Do you want to delete this record?";
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string delquery = "";
            string[] spldt = new string[2];

            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                {
                    int val = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 3].Value);
                    if (val == 1)
                    {
                        string genaccid = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Tag);
                        string collcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 2].Tag);
                        string date = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(row), 1].Text).Trim();
                        string frmdt = date.Split('-')[0];
                        spldt = frmdt.Split('/');
                        DateTime dt = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
                        string todt = date.Split('-')[1];
                        spldt = todt.Split('/');
                        DateTime dt1 = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
                        ddlcol.SelectedValue = collcode;

                        delquery = "DELETE from hryears WHERE slno = '" + genaccid + "' AND collcode = '" + collcode + "'";

                        delquery = delquery + " delete from HrPayMonths where From_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and To_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and College_Code='" + collcode + "' and hryearkey='" + genaccid + "'";
                        int count = d2.update_method_wo_parameter(delquery, "Text");
                        if (count > 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            imgDiv1.Visible = false;
                            lblconfirm.Visible = false;
                            lblalerterr.Text = "Deleted Successfully";
                            bindcollegepop();
                            btn_go_Click(sender, e);
                            popper1.Visible = false;
                            rptprint.Visible = true;

                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = false;
        lblconfirm.Visible = false;
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddlcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcol.DataSource = ds;
                ddlcol.DataTextField = "collname";
                ddlcol.DataValueField = "college_code";
                ddlcol.DataBind();
            }
        }
        catch { }
    }

    protected void bindcollegepop()
    {
        try
        {
            ds.Clear();
            ddlcoll.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoll.DataSource = ds;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();
            }
        }
        catch { }
    }

    public void clear()
    {
        rb_radleave.Checked = true;
        rb_radpaypro.Checked = false;
        ddlcoll.SelectedIndex = 0;
        txtdatestart.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdateend.Text = DateTime.Now.ToString("dd/MM/yyyy");
        rb_monthfrm.Checked = true;
        rb_monthto.Checked = false;
    }
}