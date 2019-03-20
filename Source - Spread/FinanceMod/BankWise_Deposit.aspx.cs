using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class BankWise_Deposit : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string maincol = string.Empty;
    bool spreadclick = false;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    //  string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    bool usBasedRights = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Session["usercode"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //   college_code = Session["collegecode"].ToString();
            // clgcode = collegecode1;
            // college_code = Session["collegecode"].ToString(); 
            if (!IsPostBack)
            {
                setLabelText();
                bindclg();
                if (ddl_col.Items.Count > 0)
                {
                    maincol = Convert.ToString(ddl_col.SelectedItem.Value);
                }
                loadpaid();
                bindfrmmonyear();
                bindtomonyear();
                bindfrmyear();
                bindtoyear();
                bindbank();
                txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                rportprint.Visible = false;
                rb_Datewise_OnCheckedChanged(sender, e);
                UserbasedRights();
            }
            if (ddl_col.Items.Count > 0)
            {
                maincol = Convert.ToString(ddl_col.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Bank Wise Deposit";
            string pagename = "BankWise_Deposit.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddl_col_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbank();
            bindfrmmonyear();
            bindtomonyear();
            bindfrmyear();
            bindtoyear();
        }
        catch
        {

        }
    }

    protected void cbbankname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string bankname = "";
            if (cbbankname.Checked == true)
            {
                for (int i = 0; i < cblbankname.Items.Count; i++)
                {
                    cblbankname.Items[i].Selected = true;
                    bankname = Convert.ToString(cblbankname.Items[i].Text);
                }
                if (cblbankname.Items.Count == 1)
                {
                    txt_bank.Text = "" + bankname + "";
                }
                else
                {
                    txt_bank.Text = "Bank Name(" + cblbankname.Items.Count + ")";
                }

            }
            else
            {
                for (int j = 0; j < cblbankname.Items.Count; j++)
                {
                    cblbankname.Items[j].Selected = false;
                }
                txt_bank.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    protected void cblbankname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string bankname = "";
            txt_bank.Text = "--Select--";
            cbbankname.Checked = false;
            int count = 0;
            for (int i = 0; i < cblbankname.Items.Count; i++)
            {
                if (cblbankname.Items[i].Selected == true)
                {
                    count = count + 1;
                    bankname = Convert.ToString(cblbankname.Items[i].Text);
                }
            }
            if (count > 0)
            {

                if (count == cblbankname.Items.Count)
                {
                    cbbankname.Checked = true;
                }
                if (count == 1)
                {
                    txt_bank.Text = "" + bankname + "";
                }
                else
                {
                    txt_bank.Text = "Bank Name(" + count.ToString() + ")";
                }
            }
        }
        catch
        {

        }
    }

    protected void cbtypedep_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string headername = "";
            if (cbtypedep.Checked == true)
            {
                for (int i = 0; i < cbltypedep.Items.Count; i++)
                {
                    cbltypedep.Items[i].Selected = true;
                    headername = Convert.ToString(cbltypedep.Items[i].Text);
                }
                if (cbltypedep.Items.Count == 1)
                {
                    txt_typedep.Text = "" + headername + "";
                }
                else
                {
                    txt_typedep.Text = "Paid(" + cbltypedep.Items.Count + ")";
                }

            }
            else
            {
                for (int j = 0; j < cbltypedep.Items.Count; j++)
                {
                    cbltypedep.Items[j].Selected = false;
                }
                txt_typedep.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    protected void cbltypedep_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_typedep.Text = "--Select--";
            cbtypedep.Checked = false;
            int count = 0;
            string headername = "";
            for (int i = 0; i < cbltypedep.Items.Count; i++)
            {
                if (cbltypedep.Items[i].Selected == true)
                {
                    count = count + 1;
                    headername = Convert.ToString(cbltypedep.Items[i].Text);
                }
            }
            if (count > 0)
            {

                if (count == cbltypedep.Items.Count)
                {
                    cbtypedep.Checked = true;
                }
                if (count == 1)
                {
                    txt_typedep.Text = "" + headername + "";
                }
                else
                {
                    txt_typedep.Text = "Paid(" + count.ToString() + ")";
                }
            }
        }
        catch
        {

        }
    }

    protected void Cellcont_Click(object sender, EventArgs e)
    {
        try
        {
            spreadclick = true;
        }
        catch
        {

        }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }

    protected void rb_Datewise_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            maintable.Width = "900px";
            lblfrmdate.Text = "From Date";
            txt_frmdate.Visible = true;
            cal_frmdate.Enabled = true;
            ddlfrmyear.Visible = false;
            ddlfrmmonwise.Visible = false;
            ddlfrmmonyear.Visible = false;
            lbltodate.Text = "To Date";
            txt_todate.Visible = true;
            ddltoyear.Visible = false;
            CalendarExtender1.Enabled = true;
            ddltomonwise.Visible = false;
            ddltomonyear.Visible = false;
        }
        catch
        {

        }
    }

    protected void rd_monthwise_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            maintable.Width = "1000px";
            lblfrmdate.Text = "From Month & Year";
            ddlfrmmonwise.Visible = true;
            ddlfrmmonyear.Visible = true;
            txt_frmdate.Visible = false;
            ddlfrmyear.Visible = false;
            cal_frmdate.Enabled = false;
            lbltodate.Text = "To Month & Year";
            ddltomonwise.Visible = true;
            ddltomonyear.Visible = true;
            txt_todate.Visible = false;
            ddltoyear.Visible = false;
            CalendarExtender1.Enabled = false;
        }
        catch
        {

        }
    }

    protected void rb_yearwise_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            maintable.Width = "900px";
            lblfrmdate.Text = "From Year";
            txt_frmdate.Visible = false;
            ddlfrmyear.Visible = true;
            cal_frmdate.Enabled = false;
            ddlfrmmonwise.Visible = false;
            ddlfrmmonyear.Visible = false;
            lbltodate.Text = "To Year";
            txt_todate.Visible = false;
            ddltoyear.Visible = true;
            CalendarExtender1.Enabled = false;
            ddltomonwise.Visible = false;
            ddltomonyear.Visible = false;
        }
        catch
        {

        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            if (collegecode.Trim() != null)
            {
                #region getvalue

                string bankcode = "";
                string paidcode = "";

                string frmdate = Convert.ToString(txt_frmdate.Text);
                string[] split = new string[2];
                split = frmdate.Split('/');
                DateTime dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string todate = Convert.ToString(txt_todate.Text);
                split = todate.Split('/');
                DateTime newdt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                string frmday = "01";
                string frmmonval = Convert.ToString(Convert.ToInt32(ddlfrmmonwise.SelectedItem.Value) + 1);
                string frmmonyear = Convert.ToString(ddlfrmmonyear.SelectedItem.Value);
                DateTime fromdate = Convert.ToDateTime(frmmonval + "/" + frmday + "/" + frmmonyear);

                string defday = "01";
                string tomonval = Convert.ToString(Convert.ToInt32(ddltomonwise.SelectedItem.Value) + 1);
                string tomonyear = Convert.ToString(ddltomonyear.SelectedItem.Value);
                DateTime dtfrmmon = Convert.ToDateTime(tomonval + "/" + defday + "/" + tomonyear);
                DateTime day = dtfrmmon.AddMonths(1).AddDays(-1);
                string today = Convert.ToString(day.Day);
                DateTime tomondate = Convert.ToDateTime(tomonval + "/" + today + "/" + tomonyear);

                int frmyear = Convert.ToInt32(ddlfrmyear.SelectedItem.Value);
                int toyear = Convert.ToInt32(ddltoyear.SelectedItem.Value);

                for (int i = 0; i < cblbankname.Items.Count; i++)
                {
                    if (cblbankname.Items[i].Selected == true)
                    {
                        if (bankcode.Trim() == "")
                        {
                            bankcode = "" + cblbankname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            bankcode = bankcode + "'" + "," + "'" + cblbankname.Items[i].Value.ToString() + "";
                        }
                    }
                }

                for (int j = 0; j < cbltypedep.Items.Count; j++)
                {
                    if (cbltypedep.Items[j].Selected == true)
                    {
                        if (paidcode.Trim() == "")
                        {
                            paidcode = "" + cbltypedep.Items[j].Value.ToString() + "";
                        }
                        else
                        {
                            paidcode = paidcode + "'" + "," + "'" + cbltypedep.Items[j].Value.ToString() + "";
                        }
                    }
                }
                if (bankcode.Trim() == "")
                {
                    lblerrgo.Visible = true;
                    rportprint.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lblerrgo.Text = "Please Select Bank Name!";
                }

                if (paidcode.Trim() == "")
                {
                    lblerrgo.Visible = true;
                    rportprint.Visible = false;
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lblerrgo.Text = "Please Select Paid Status!";
                }

                #endregion

                if (bankcode.Trim() != "" && paidcode.Trim() != "")
                {
                    #region design

                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 5;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    if (rb_Datewise.Checked == true)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                    }
                    if (rd_monthwise.Checked == true)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
                    }
                    if (rb_yearwise.Checked == true)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                    }
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Type of Deposit";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Credit";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Debit";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    #endregion
                    UserbasedRights();
                    string userCode = "";
                    if (usBasedRights == true)
                        userCode = " and t.EntryUserCode in('" + usercode + "')";
                    string selq = "";
                    double dbtot = 0;
                    double crtot = 0;
                    double debit = 0;
                    double credit = 0;
                    double total = 0;
                    if (rb_Datewise.Checked == true)
                    {
                        #region date

                        selq = "SELECT Convert(varchar(10),TransDate,103) as TransDate,BankName,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit,SUM(Credit) as Credit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' " + userCode + " and IsCleared = 1 group by TransDate,BankName,PayMode order by PayMode";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["TransDate"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["BankName"]);
                                    // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["PayMode"]);
                                    // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Debit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Debit"]), out debit);
                                    dbtot += debit;

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Credit"]), out credit);
                                    crtot += credit;
                                }
                                Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(dbtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(crtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                                double grandtot = 0;
                                grandtot = dbtot - crtot;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(grandtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 4);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;

                                rportprint.Visible = true;
                                Fpspread1.Visible = true;
                                div1.Visible = true;
                                lblerrgo.Visible = false;
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            }
                            else
                            {
                                rportprint.Visible = false;
                                Fpspread1.Visible = false;
                                div1.Visible = false;
                                lblerrgo.Visible = true;
                                lblerrgo.Text = "No Records Found!";
                            }
                        }
                        #endregion
                    }
                    if (rd_monthwise.Checked == true)
                    {
                        #region month

                        selq = "SELECT DATENAME(month, transdate) as Month,BankName,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit,SUM(Credit) as Credit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + tomondate.ToString("MM/dd/yyyy") + "' " + userCode + " and IsCleared = 1 group by DATENAME(month, transdate),BankName,PayMode order by PayMode";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Month"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["BankName"]);
                                    // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["PayMode"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Debit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Debit"]), out debit);
                                    dbtot += debit;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Credit"]), out credit);
                                    crtot += credit;
                                }
                                Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(dbtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(crtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;

                                // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                                double grandtot = 0;
                                grandtot = dbtot - crtot;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(grandtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 4);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;

                                rportprint.Visible = true;
                                Fpspread1.Visible = true;
                                div1.Visible = true;
                                lblerrgo.Visible = false;
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            }
                            else
                            {
                                rportprint.Visible = false;
                                Fpspread1.Visible = false;
                                div1.Visible = false;
                                lblerrgo.Visible = true;
                                lblerrgo.Text = "No Records Found!";
                            }
                        }
                        #endregion
                    }
                    if (rb_yearwise.Checked == true)
                    {
                        #region year

                        selq = "SELECT year(TransDate) as Year,BankName,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit,SUM(Credit) as Credit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and year(TransDate) >='" + frmyear + "' and year(TransDate) <='" + toyear + "' " + userCode + " and IsCleared = 1 group by year(TransDate),BankName,PayMode order by PayMode";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Year"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["BankName"]);
                                    // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["PayMode"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Debit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Debit"]), out debit);
                                    dbtot += debit;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Border.BorderColor = System.Drawing.Color.Black;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Credit"]), out credit);
                                    crtot += credit;

                                }
                                Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                Fpspread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(dbtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(crtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;

                                //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                                double grandtot = 0;
                                grandtot = dbtot - crtot;
                                Fpspread1.Sheets[0].Rows.Count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(grandtot);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 4);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Black;
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;

                                rportprint.Visible = true;
                                Fpspread1.Visible = true;
                                div1.Visible = true;
                                lblerrgo.Visible = false;
                                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            }
                            else
                            {
                                rportprint.Visible = false;
                                Fpspread1.Visible = false;
                                div1.Visible = false;
                                lblerrgo.Visible = true;
                                lblerrgo.Text = "No Records Found!";
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        catch
        {

        }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_col.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_col.DataSource = ds;
                ddl_col.DataTextField = "collname";
                ddl_col.DataValueField = "college_code";
                ddl_col.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindbank()
    {
        try
        {
            ds.Clear();
            cblbankname.Items.Clear();
            string selquery = "select BankPK,BankName from FM_FinBankMaster where CollegeCode='" + maincol + "'";
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblbankname.DataSource = ds;
                    cblbankname.DataTextField = "BankName";
                    cblbankname.DataValueField = "BankPK";
                    cblbankname.DataBind();

                    if (cblbankname.Items.Count > 0)
                    {
                        for (int i = 0; i < cblbankname.Items.Count; i++)
                        {
                            cblbankname.Items[i].Selected = true;
                        }
                        txt_bank.Text = "Bank Name(" + cblbankname.Items.Count + ")";
                        cbbankname.Checked = true;
                    }
                }
                else
                {
                    txt_bank.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    public void loadpaid()
    {
        try
        {
            cbltypedep.Items.Clear();
            //cbltypedep.Items.Add(new ListItem("Cash", "1"));
            //cbltypedep.Items.Add(new ListItem("Cheque", "2"));
            //cbltypedep.Items.Add(new ListItem("DD", "3"));
            //cbltypedep.Items.Add(new ListItem("Challan", "4"));
            //cbltypedep.Items.Add(new ListItem("Online", "5"));
            d2.BindPaymodeToCheckboxList(cbltypedep, usercode, maincol);
            if (cbltypedep.Items.Count > 0)
            {
                for (int i = 0; i < cbltypedep.Items.Count; i++)
                {
                    cbltypedep.Items[i].Selected = true;
                }
                txt_typedep.Text = "Paid(" + cbltypedep.Items.Count + ")";
                cbtypedep.Checked = true;
            }
        }
        catch
        {

        }
    }

    public void bindfrmmonyear()
    {
        try
        {
            ddlfrmmonyear.Items.Clear();
            string curyr = DateTime.Now.ToString("yyyy");
            if (curyr != "")
            {
                for (int i = Convert.ToInt32(curyr); i >= 2010; i--)
                {
                    ddlfrmmonyear.Items.Add(Convert.ToString(i));
                }
            }
        }
        catch
        {
        }
    }

    public void bindtomonyear()
    {
        try
        {
            ddltomonyear.Items.Clear();
            string curyr = DateTime.Now.ToString("yyyy");
            if (curyr != "")
            {
                for (int i = Convert.ToInt32(curyr); i >= 2010; i--)
                {
                    ddltomonyear.Items.Add(Convert.ToString(i));
                }
            }
        }
        catch
        {
        }
    }

    public void bindfrmyear()
    {
        try
        {
            ddlfrmyear.Items.Clear();
            string curyr = DateTime.Now.ToString("yyyy");
            if (curyr != "")
            {
                for (int i = Convert.ToInt32(curyr); i >= 2010; i--)
                {
                    ddlfrmyear.Items.Add(Convert.ToString(i));
                }
            }
        }
        catch
        {
        }
    }

    public void bindtoyear()
    {
        try
        {
            ddltoyear.Items.Clear();
            string curyr = DateTime.Now.ToString("yyyy");
            if (curyr != "")
            {
                for (int i = Convert.ToInt32(curyr); i >= 2010; i--)
                {
                    ddltoyear.Items.Add(Convert.ToString(i));
                }
            }

        }
        catch
        {
        }
    }

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lbl_clg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar
}