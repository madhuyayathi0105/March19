using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class BankWise_DepositChart : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string maincol = string.Empty;
    bool spreadclick = false;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    int check = 0;
    int sel = 0;
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

                if (bankcode.Trim() != "" && paidcode.Trim() != "")
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 4;

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

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amount";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = System.Drawing.Color.Black;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    string selq = "";
                    double total = 0;
                    if (rb_Datewise.Checked == true)
                    {
                        selq = " SELECT BankName,BankPK,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' and IsCleared = 1 group by BankName,BankPK ";
                        selq = selq + " SELECT BankName,BankPK,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' and IsCleared = 1 group by BankName,PayMode,BankPK order by PayMode";
                        selq = selq + " SELECT distinct case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + newdt.ToString("MM/dd/yyyy") + "' and IsCleared = 1";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            check = 1;
                            BankWiseChart();
                            lblerrgo.Visible = false;

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
                    if (rd_monthwise.Checked == true)
                    {
                        selq = "SELECT BankName,BankPK,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + tomondate.ToString("MM/dd/yyyy") + "' and IsCleared = 1 group by BankName,BankPK";
                        selq = selq + " SELECT BankName,BankPK,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + tomondate.ToString("MM/dd/yyyy") + "' and IsCleared = 1 group by BankName,PayMode,BankPK order by PayMode";
                        selq = selq + " SELECT distinct case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and TransDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + tomondate.ToString("MM/dd/yyyy") + "' and IsCleared = 1 ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            check = 2;
                            BankWiseChart();
                            lblerrgo.Visible = false;

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
                    if (rb_yearwise.Checked == true)
                    {


                        selq = "SELECT BankName,BankPK,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and year(TransDate) >='" + frmyear + "' and year(TransDate) <='" + toyear + "' and IsCleared = 1 group by BankName,BankPK";
                        selq = selq + " SELECT BankName,BankPK,case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode,SUM(Debit) as Debit FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and year(TransDate) >='" + frmyear + "' and year(TransDate) <='" + toyear + "' and IsCleared = 1 group by BankName,BankPK,PayMode order by PayMode";
                        selq = selq + " SELECT distinct case when PayMode=1 then 'Cash' when PayMode=2 then 'Cheque' when PayMode=3 then 'DD' when PayMode=4 then 'Challan' when PayMode=5 then 'Online' end as PayMode FROM FT_FinBankTransaction T,FM_FinBankMaster B where t.BankFK = b.BankPK and b.BankPK in ('" + bankcode + "') and B.CollegeCode ='" + maincol + "' and PayMode in ('" + paidcode + "') and year(TransDate) >='" + frmyear + "' and year(TransDate) <='" + toyear + "' and IsCleared = 1";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            check = 3;
                            BankWiseChart();
                            lblerrgo.Visible = false;
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

    #region Bankwise Chart

    private void BankWiseChart()
    {
        try
        {
            int val = 0;
            int row = 0;
            int col = 0;
            string cash = "";
            string cheque = "";
            string dd = "";
            string fromdate = "";
            string todates = "";
            string bankname = "";
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            List<string> list = new List<string>();
            List<string> collist = new List<string>();
            DataView dv = new DataView();
            DataTable dtchart = new DataTable();
            DataColumn dtcol1 = new DataColumn();

            //chart.ChartAreas[0].AxisX.Title = "HeaderWise Common";
            //  chart.ChartAreas[0].AxisY.Title = "Amount";
            chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
            chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
            chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
            chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
            chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            dtchart.Columns.Clear();
            chart.Series.Clear();


            fromdate = Convert.ToString(txt_frmdate.Text);
            todates = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                dt = Convert.ToDateTime(fromdate);
            }
            string[] tdate = todates.Split('/');
            if (tdate.Length == 3)
            {
                todates = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                dt1 = Convert.ToDateTime(todates);
            }
            string frmday = "01";
            string frmmonval = Convert.ToString(Convert.ToInt32(ddlfrmmonwise.SelectedItem.Value) + 1);
            string frmmonyear = Convert.ToString(ddlfrmmonyear.SelectedItem.Value);
            DateTime fromondate = Convert.ToDateTime(frmmonval + "/" + frmday + "/" + frmmonyear);

            string defday = "01";
            string tomonval = Convert.ToString(Convert.ToInt32(ddltomonwise.SelectedItem.Value) + 1);
            string tomonyear = Convert.ToString(ddltomonyear.SelectedItem.Value);
            DateTime dtfrmmon = Convert.ToDateTime(tomonval + "/" + defday + "/" + tomonyear);
            DateTime day = dtfrmmon.AddMonths(1).AddDays(-1);
            string today = Convert.ToString(day.Day);
            DateTime tomondate = Convert.ToDateTime(tomonval + "/" + today + "/" + tomonyear);


            int frmyear = Convert.ToInt32(ddlfrmyear.SelectedItem.Value);
            int toyear = Convert.ToInt32(ddltoyear.SelectedItem.Value);

            int count = 0;
            dtchart.Columns.Add(dtcol1);
            for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                count++;
                DataColumn dtcol = new DataColumn();
                bankname = Convert.ToString(ds.Tables[0].Rows[sel]["BankName"]);
                ListItem colli = new ListItem(Convert.ToString(bankname), Convert.ToString(count));
                collist.Add(Convert.ToString(colli));
                dtchart.Columns.Add(bankname);
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                for (row = 0; row < ds.Tables[2].Rows.Count; row++)
                {
                    string payname = Convert.ToString(ds.Tables[2].Rows[row]["Paymode"]);
                    DataRow dtrow = dtchart.NewRow();
                    dtrow[0] = Convert.ToString(payname);
                    dtchart.Rows.Add(dtrow);
                    ListItem li = new ListItem(Convert.ToString(payname), Convert.ToString(row));
                    list.Add(Convert.ToString(li));
                    chart.Series.Add(payname);

                }
            }

            if (dtchart.Columns.Count > 0)
            {
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    string bankfk = Convert.ToString(ds.Tables[0].Rows[sel]["BankPK"]);
                    bankname = Convert.ToString(ds.Tables[0].Rows[sel]["BankName"]);
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "BankPK='" + Convert.ToString(bankfk) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0 && dv != null)
                        {
                            for (row = 0; row < dv.Count; row++)
                            {
                                string paymode = Convert.ToString(dv[row]["Paymode"]);

                                if (paymode == "Cash")
                                {
                                    cash = Convert.ToString(dv[row]["Debit"]);
                                    if (list.Contains("Cash"))
                                    {
                                        int rowval = Convert.ToInt32(list.IndexOf("Cash"));
                                        int colval = Convert.ToInt32(collist.IndexOf(bankname));
                                        dtchart.Rows[rowval][colval + 1] = Convert.ToString(cash);
                                    }
                                }
                                if (paymode == "Cheque")
                                {
                                    cheque = Convert.ToString(dv[row]["Debit"]);
                                    if (list.Contains("Cheque"))
                                    {
                                        int rowval = Convert.ToInt32(list.IndexOf("Cheque"));
                                        int colval = Convert.ToInt32(collist.IndexOf(bankname));
                                        dtchart.Rows[rowval][colval + 1] = Convert.ToString(cheque);
                                    }
                                }
                                if (paymode == "DD")
                                {
                                    dd = Convert.ToString(dv[row]["Debit"]);
                                    if (list.Contains("DD"))
                                    {
                                        int rowval = Convert.ToInt32(list.IndexOf("DD"));
                                        int colval = Convert.ToInt32(collist.IndexOf(bankname));
                                        dtchart.Rows[rowval][colval + 1] = Convert.ToString(dd);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (dtchart.Rows.Count > 0)
            {
                for (col = 1; col < dtchart.Columns.Count; col++)
                {
                    for (row = 0; row < dtchart.Rows.Count; row++)
                    {
                        chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                        chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                        chart.Series[row].IsValueShownAsLabel = true;
                        chart.Series[row].IsXValueIndexed = true;

                        chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                    }
                }
                chart.Visible = true;
                chart.Height = 450;
                chart.Width = 1000;
            }
        }
        catch { }
    }

    #endregion

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

    // last modified 05-10-2016 sudhagar
}