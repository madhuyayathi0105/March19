using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using System.Configuration;

public partial class CreditdebitReportChart : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    Boolean cellclick = false;
    Boolean cellclick1 = false;
    Boolean cellclick2 = false;
    Boolean cellclick3 = false;
    int commcount;
    int i;
    int cout;
    int row;

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate2.Attributes.Add("readonly", "readonly");
                txt_fromdate2.Attributes.Add("readonly", "readonly");
                headerbind();
                ledgerbind();
                //bindmonth();
                //bindyear();
                header1bind();
                ledger1bind();
                header2bind();
                ledger2bind();
            }
        }
        catch (Exception ex) { }

    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch
        {
        }
    }
    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            txt_header.Text = "--Select--";
            if (cb_header.Checked == true)
            {

                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                    header = Convert.ToString(cbl_header.Items[i].Text);
                }
                if (cbl_header.Items.Count == 1)
                {
                    txt_header.Text = "" + header + "";
                }
                else
                {
                    txt_header.Text = "Header(" + (cbl_header.Items.Count) + ")";
                }

            }
            else
            {
                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = false;
                }
                txt_header.Text = "--Select--";
            }
            ledgerbind();

        }
        catch { }
    }
    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header.Checked = false;
            commcount = 0;
            txt_header.Text = "--Select--";
            string header = "";
            for (i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    header = Convert.ToString(cbl_header.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header.Items.Count)
                {
                    cb_header.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_header.Text = "" + header + "";
                }
                else
                {
                    txt_header.Text = "Header(" + commcount.ToString() + ")";
                }

            }
            ledgerbind();

        }
        catch { }
    }
    protected void cb_ledger_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            txt_ledger.Text = "--Select--";
            if (cb_ledger.Checked == true)
            {

                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    cbl_ledger.Items[i].Selected = true;
                    ledger = Convert.ToString(cbl_ledger.Items[i].Text);
                }
                if (cbl_ledger.Items.Count == 1)
                {
                    txt_ledger.Text = "" + ledger + "";
                }
                else
                {
                    txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
                }

            }
            else
            {
                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    cbl_ledger.Items[i].Selected = false;
                }
            }

        }
        catch { }
    }
    protected void cbl_ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_ledger.Checked = false;
            commcount = 0;
            txt_ledger.Text = "--Select--";
            string ledger = "";
            for (i = 0; i < cbl_ledger.Items.Count; i++)
            {
                if (cbl_ledger.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    ledger = Convert.ToString(cbl_ledger.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledger.Items.Count)
                {
                    cb_ledger.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_ledger.Text = "" + ledger + "";
                }
                else
                {
                    txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
                }

            }

        }
        catch { }

    }


    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
    }
    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt1.Text);
                string seconddate = Convert.ToString(txt2.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    //imgdiv2.Visible = true;
                    //lbl_alert1.Text = "Pleace Select  FromDate Before or equal than ToDate  ";
                    txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void ledgerbind()
    {
        try
        {
            string HeaderPK = "";

            for (i = 0; i < cbl_header.Items.Count; i++)
            {

                if (cbl_header.Items[i].Selected == true)
                {

                    if (HeaderPK == "")
                    {
                        HeaderPK = cbl_header.Items[i].Value.ToString();
                    }
                    else
                    {
                        HeaderPK += "','" + cbl_header.Items[i].Value.ToString();

                    }
                }
            }

            ds.Clear();
            cbl_ledger.Items.Clear();
            //  string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger.DataSource = ds;
                cbl_ledger.DataTextField = "LedgerName";
                cbl_ledger.DataValueField = "LedgerPK";
                cbl_ledger.DataBind();

                if (cbl_ledger.Items.Count > 0)
                {

                    for (int i = 0; i < cbl_ledger.Items.Count; i++)
                    {
                        cbl_ledger.Items[i].Selected = true;
                    }
                    txt_ledger.Text = "Ledger(" + cbl_ledger.Items.Count + ")";
                    cb_ledger.Checked = true;
                }
            }


        }

        catch
        {
        }
    }
    public void headerbind()
    {
        try
        {


            ds.Clear();
            cbl_header.Items.Clear();
            //  string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {


                cbl_header.DataSource = ds;
                cbl_header.DataTextField = "HeaderName";
                cbl_header.DataValueField = "HeaderPK";
                cbl_header.DataBind();

                cb_header.Checked = true;

                if (cbl_header.Items.Count > 0)
                {
                    for (i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;

                    }
                    txt_header.Text = "Header(" + cbl_header.Items.Count + ")";
                    cb_header.Checked = true;
                    // ledgerbind();

                }
            }

        }
        catch { }
    }

    public void fairpoin()
    {
        FpSpread2.Visible = false;

        FpSpread4.Visible = false;


    }
    public void monthview()
    {
        try
        {
            fairpoin();
            FpSpread4.Sheets[0].RowCount = 0;
            FpSpread4.Sheets[0].ColumnCount = 0;
            FpSpread4.CommandBar.Visible = false;
            FpSpread4.Sheets[0].AutoPostBack = true;
            FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread4.Sheets[0].RowHeader.Visible = false;
            FpSpread4.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread4.Visible = true;

            string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            string creditop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            string debitop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;

            FarPoint.Web.Spread.TextCellType opnbal = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType clsbal = new FarPoint.Web.Spread.TextCellType();
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            // FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Opening Balance";
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Opening Balance";
            FpSpread4.Sheets[0].Columns[3].CellType = opnbal;
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
            FpSpread4.Sheets[0].Columns[4].CellType = debit;
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
            FpSpread4.Sheets[0].Columns[5].CellType = credit;
            FpSpread4.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Closing Balance";
            FpSpread4.Sheets[0].Columns[6].CellType = clsbal;


            for (int i = 0; i < FpSpread4.Sheets[0].Columns.Count; i++)
            {
                FpSpread4.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
            }
            FpSpread4.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
            FpSpread4.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread4.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread4.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread4.Sheets[0].ColumnHeader.Columns[0].Width = 51;
            FpSpread4.Sheets[0].ColumnHeader.Columns[1].Width = 140;
            FpSpread4.Sheets[0].ColumnHeader.Columns[3].Width = 150;
            FpSpread4.Sheets[0].ColumnHeader.Columns[4].Width = 175;
            FpSpread4.Sheets[0].ColumnHeader.Columns[5].Width = 148;
            FpSpread4.Sheets[0].ColumnHeader.Columns[6].Width = 119;

            if (cellclick1 == true)
            {
                string date = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;


                DateTime dt3 = new DateTime();
                DateTime dt4 = new DateTime();


                string[] split1 = date.Split('-');

                dt3 = Convert.ToDateTime(split1[0] + "/" + 1 + "/" + split1[1]);
                string month = split1[0];
                int year = Convert.ToInt32(split1[1]);


                string dat = "";
                switch (month)
                {
                    case "1":
                        dat = "31";
                        break;
                    case "2":
                        year = Convert.ToInt32(year);
                        if ((year / 4) == 0)
                        {
                            dat = "28";
                        }
                        else
                        {
                            dat = "29";
                        }

                        break;
                    case "3":
                        dat = "31";
                        break;
                    case "4":
                        dat = "30";
                        break;
                    case "5":
                        dat = "31";
                        break;
                    case "6":
                        dat = "30";
                        break;
                    case "7":
                        dat = "31";
                        break;
                    case "8":
                        dat = "31";
                        break;
                    case "9":
                        dat = "30";
                        break;
                    case "10":
                        dat = "31";
                        break;
                    case "11":
                        dat = "30";
                        break;
                    case "12":
                        dat = "31";
                        break;


                }
                string debitvalue = "";
                string creditvalue = "";
                string opningbal = "";
                double closingbal = 0;

                dt4 = Convert.ToDateTime(split1[0] + "/" + dat + "/" + split1[1]);

                string selqry = " SELECT SUM(Debit)as Debit,ISNULL( sum(Credit),0)as Credit,(SUM(Debit)-ISNULL( sum(Credit),0)) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE  TransDate Between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt4.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' GROUP BY TransDate";
                selqry = selqry + " SELECT SUM(Debit)-ISNULL( sum(Credit),0) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate < '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  ";
                ds = d2.select_method_wo_parameter(selqry, "Text");

                string totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate < '" + dt3.ToString("MM/dd/yyyy") + "'");
                totamt = Convert.ToString(totamt);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread4.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            opningbal = "";
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                            }
                            else
                            {
                                opningbal = totamt;
                            }
                            if (rb_month.Checked == true)
                            {
                                FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                                if (opningbal == "")
                                {
                                    opningbal = "0";
                                }
                                FpSpread4.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
                                FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                                FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
                                debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
                                creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
                                if (debitvalue == "")
                                {
                                    debitvalue = "0";
                                }
                                if (creditvalue == "")
                                {
                                    creditvalue = "0";
                                }
                                if (opningbal == "")
                                {
                                    opningbal = "0";
                                }
                                closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                                // FpSpread4.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                                FpSpread4.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
                                // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                                opningbal = Convert.ToString(closingbal);
                            }

                        }
                        else
                        {
                            if (rb_month.Checked == true)
                            {

                                FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                                FpSpread4.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
                                FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                                FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
                                debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
                                creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
                                if (debitvalue == "")
                                {
                                    debitvalue = "0";
                                }
                                if (creditvalue == "")
                                {
                                    creditvalue = "0";
                                }
                                if (opningbal == "")
                                {
                                    opningbal = "0";
                                }
                                closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                                // FpSpread4.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                                FpSpread4.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
                                // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                                opningbal = Convert.ToString(closingbal);
                            }

                        }
                    }
                    FpSpread4.Sheets[0].Columns[2].Visible = false;
                    FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].Rows.Count;
                    Div2.Visible = true;
                    rptprint1.Visible = true;
                    FpSpread4.Visible = true;
                    FpSpread4.SaveChanges();

                }
                else
                {
                    Div2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    //FpSpread1.Visible = false;
                }
            }
        }
        catch
        {
        }
    }

    public void dtmonth2()
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread2.Visible = false;
            div1.Visible = false;

            string itemheadercode = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }

            string Ledgercode = "";
            for (int i = 0; i < cbl_ledger.Items.Count; i++)
            {
                if (cbl_ledger.Items[i].Selected == true)
                {
                    if (Ledgercode == "")
                    {
                        Ledgercode = "" + cbl_ledger.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Ledgercode = Ledgercode + "'" + "," + "'" + cbl_ledger.Items[i].Value.ToString() + "";
                    }
                }
            }

            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            // rptprint.Visible  = true;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = true;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.TextCellType opnbal = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType clsbal = new FarPoint.Web.Spread.TextCellType();
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;


            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;


            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Opening Balance";
            FpSpread3.Sheets[0].Columns[3].CellType = opnbal;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
            FpSpread3.Sheets[0].Columns[4].CellType = debit;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
            FpSpread3.Sheets[0].Columns[5].CellType = credit;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Closing Balance";
            FpSpread3.Sheets[0].Columns[6].CellType = clsbal;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

            FpSpread3.Sheets[0].ColumnHeader.Columns[0].Width = 51;
            FpSpread3.Sheets[0].ColumnHeader.Columns[1].Width = 155;
            FpSpread3.Sheets[0].ColumnHeader.Columns[3].Width = 150;
            FpSpread3.Sheets[0].ColumnHeader.Columns[4].Width = 175;
            FpSpread3.Sheets[0].ColumnHeader.Columns[5].Width = 148;
            FpSpread3.Sheets[0].ColumnHeader.Columns[6].Width = 119;


            string selqry = "";
            string totamt = "";
            string debitvalue = "";
            string creditvalue = "";
            string opningbal = "";
            double closingbal = 0;
            if (rb_month.Checked == true)
            {
                selqry = "SELECT CONVERT(varchar(10), Month(TransDate))+'-'+ CONVERT(varchar(10), YEAR(TransDate)) MonthYear,SUM(Debit)as Debit,ISNULL( sum(Credit),0)as Credit,((sum(Debit)-ISNULL( sum(Credit),0))+ SUM(Debit)-ISNULL( sum(Credit),0)) ClosingBal FROM FT_FinDailyTransaction WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and  TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' GROUP BY YEAR(TransDate),Month(TransDate)";
                selqry = selqry + " SELECT SUM(Debit)-ISNULL( sum(Credit),0) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate <= '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate < '" + dt.ToString("MM/dd/yyyy") + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");

                totamt = Convert.ToString(totamt);
                monthWiseChart();
            }

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    FpSpread3.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            opningbal = "";
            //            if (ds.Tables[1].Rows.Count > 0)
            //            {
            //                opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
            //            }
            //            else
            //            {
            //                opningbal = totamt;
            //            }
            //            if (rb_month.Checked == true)
            //            {
            //                FpSpread3.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
            //                FpSpread3.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["MonthYear"].ToString();
            //                if (opningbal == "")
            //                {
            //                    opningbal = "0";
            //                }
            //                FpSpread3.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
            //                debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
            //                creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
            //                if (debitvalue == "")
            //                {
            //                    debitvalue = "0";
            //                }
            //                if (creditvalue == "")
            //                {
            //                    creditvalue = "0";
            //                }
            //                FpSpread3.Sheets[0].Cells[i, 4].Text = Convert.ToString(debitvalue);
            //                FpSpread3.Sheets[0].Cells[i, 5].Text = Convert.ToString(creditvalue);
            //                closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
            //                FpSpread3.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
            //                // FpSpread3.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
            //                //opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
            //                opningbal = Convert.ToString(closingbal);

            //            }
            //        }
            //        else
            //        {
            //            if (rb_month.Checked == true)
            //            {
            //                FpSpread3.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
            //                FpSpread3.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["MonthYear"].ToString();
            //                if (opningbal == "")
            //                {
            //                    opningbal = "0";
            //                }
            //                FpSpread3.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
            //                debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
            //                creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
            //                if (debitvalue == "")
            //                {
            //                    debitvalue = "0";
            //                }
            //                if (creditvalue == "")
            //                {
            //                    creditvalue = "0";
            //                }
            //                FpSpread3.Sheets[0].Cells[i, 4].Text = Convert.ToString(debitvalue);
            //                FpSpread3.Sheets[0].Cells[i, 5].Text = Convert.ToString(creditvalue);
            //                closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
            //                //FpSpread3.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
            //                FpSpread3.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
            //                //  opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
            //                opningbal = Convert.ToString(closingbal);


            //            }

            //        }
            //    }
            //    for (int i = 0; i < FpSpread3.Sheets[0].Columns.Count; i++)
            //    {
            //        FpSpread3.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
            //        FpSpread3.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
            //    }
            //    FpSpread3.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
            //    FpSpread3.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            //    FpSpread3.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            //    FpSpread3.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            //    //  monthdiv.Visible = true;
            //    // rptprint1.Visible = true;
            //    /// FpSpread3.Visible = true;
            //    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].Rows.Count;
            //    FpSpread3.Columns[2].Visible = false;
            //    FpSpread3.SaveChanges();
            //    monthdiv.Visible = false;

            //}
            //else
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alert1.Text = "No Records Found";
            //    monthdiv.Visible = false;
            //    FpSpread3.Visible = false;
            //    // rptprint.Visible = false;
            //}
            //rbchange();
        }
        catch
        {
        }
    }
    public void dtdate2()
    {
        fairpoin();


        FpSpread3.Visible = false;
        string itemheadercode = "";
        for (int i = 0; i < cbl_header.Items.Count; i++)
        {
            if (cbl_header.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                }
            }
        }

        string Ledgercode = "";
        for (int i = 0; i < cbl_ledger.Items.Count; i++)
        {
            if (cbl_ledger.Items[i].Selected == true)
            {
                if (Ledgercode == "")
                {
                    Ledgercode = "" + cbl_ledger.Items[i].Value.ToString() + "";
                }
                else
                {
                    Ledgercode = Ledgercode + "'" + "," + "'" + cbl_ledger.Items[i].Value.ToString() + "";
                }
            }
        }


        string firstdate = Convert.ToString(txt_fromdate.Text);
        string seconddate = Convert.ToString(txt_todate.Text);
        DateTime dt = new DateTime();
        DateTime dt1 = new DateTime();
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        split = seconddate.Split('/');
        dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].ColumnCount = 7;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //  FpSpread1.Visible = true;
        //  rptprint1.Visible = true;
        div1.Visible = false;

        FarPoint.Web.Spread.TextCellType opnbal = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.TextCellType clsbal = new FarPoint.Web.Spread.TextCellType();
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;


        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;


        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Opening Balance";
        FpSpread1.Sheets[0].Columns[3].CellType = opnbal;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        FpSpread1.Sheets[0].Columns[4].CellType = debit;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
        FpSpread1.Sheets[0].Columns[5].CellType = credit;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Closing Balance";
        FpSpread1.Sheets[0].Columns[6].CellType = clsbal;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 51;
        FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 155;
        FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 175;
        FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 148;
        FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 119;

        //ds.Clear();
        string totamt = "";
        string selqry = "";
        string debitvalue = "";
        string creditvalue = "";
        string opningbal = "";
        double closingbal = 0;
        if (rb_date.Checked == true)
        {
            selqry = " SELECT SUM(Debit)as Debit,ISNULL( sum(Credit),0)as Credit,(SUM(Debit)-ISNULL( sum(Credit),0)) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' GROUP BY TransDate";
            selqry = selqry + " SELECT SUM(Debit)-ISNULL( sum(Credit),0) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate < '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate >= '" + dt + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");
            totamt = Convert.ToString(totamt);
        }

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (i == 0)
                    {
                        opningbal = "";
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                        }
                        else
                        {
                            opningbal = totamt;
                        }
                        if (rb_date.Checked == true)
                        {
                            FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                            if (opningbal == "")
                            {
                                opningbal = "0";
                            }
                            FpSpread1.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
                            debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
                            creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
                            if (debitvalue == "")
                            {
                                debitvalue = "0";
                            }
                            if (creditvalue == "")
                            {
                                creditvalue = "0";
                            }
                            FpSpread1.Sheets[0].Cells[i, 4].Text = Convert.ToString(debitvalue);
                            FpSpread1.Sheets[0].Cells[i, 5].Text = Convert.ToString(creditvalue);
                            closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                            //  FpSpread1.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            FpSpread1.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
                            // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                            opningbal = Convert.ToString(closingbal);
                        }

                    }
                    else
                    {
                        if (rb_date.Checked == true)
                        {

                            FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                            if (opningbal == "")
                            {
                                opningbal = "0";
                            }
                            FpSpread1.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
                            debitvalue = ds.Tables[0].Rows[i]["Debit"].ToString();
                            creditvalue = ds.Tables[0].Rows[i]["Credit"].ToString();
                            if (debitvalue == "")
                            {
                                debitvalue = "0";
                            }
                            if (creditvalue == "")
                            {
                                creditvalue = "0";
                            }
                            FpSpread1.Sheets[0].Cells[i, 4].Text = Convert.ToString(debitvalue);
                            FpSpread1.Sheets[0].Cells[i, 5].Text = Convert.ToString(creditvalue);
                            closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                            // FpSpread1.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            FpSpread1.Sheets[0].Cells[i, 6].Text = Convert.ToString(closingbal);
                            // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                            opningbal = Convert.ToString(closingbal);
                        }


                    }
                }
                for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                {
                    FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                }
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
                div1.Visible = false;
                rptprint1.Visible = false;
            }
        }
        else
        {

            //lbl_alert1.Text = "No Records Found";
            //FpSpread1.Visible = false;

        }

        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
        FpSpread1.Columns[2].Visible = false;

        rbchange();


    }
    public void dateview()
    {
        fairpoin();


        string itemheadercode = "";
        for (int i = 0; i < cbl_header.Items.Count; i++)
        {
            if (cbl_header.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                }
            }
        }

        string Ledgercode = "";
        for (int i = 0; i < cbl_ledger.Items.Count; i++)
        {
            if (cbl_ledger.Items[i].Selected == true)
            {
                if (Ledgercode == "")
                {
                    Ledgercode = "" + cbl_ledger.Items[i].Value.ToString() + "";
                }
                else
                {
                    Ledgercode = Ledgercode + "'" + "," + "'" + cbl_ledger.Items[i].Value.ToString() + "";
                }
            }
        }
        FpSpread5.Visible = false;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.CommandBar.Visible = false;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread2.Sheets[0].RowHeader.Visible = false;
        FpSpread2.Sheets[0].ColumnCount = 7;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread2.Visible = true;
        // Dateview.Visible = true;
        //  rptprint.Visible = true;
        // maindiv2.Visible = true;
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        string creditop = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
        string debitop = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;

        FarPoint.Web.Spread.TextCellType txtdebit = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.TextCellType txtcredit = new FarPoint.Web.Spread.TextCellType();

        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Recipt NO";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Particulars";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        FpSpread2.Sheets[0].Columns[4].CellType = txtcredit;
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
        FpSpread2.Sheets[0].Columns[5].CellType = txtdebit;
        FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);




        for (int i = 0; i < FpSpread2.Sheets[0].Columns.Count; i++)
        {
            FpSpread2.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

        }
        FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
        FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;

        FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 128;
        FpSpread2.Sheets[0].ColumnHeader.Columns[2].Width = 128;
        FpSpread2.Sheets[0].ColumnHeader.Columns[3].Width = 249;
        FpSpread2.Sheets[0].ColumnHeader.Columns[4].Width = 150;
        FpSpread2.Sheets[0].ColumnHeader.Columns[5].Width = 152;


        //ds.Clear();
        if (cellclick == true)
        {
            string dte = "";
            string dtm = "";
            string date = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            DateTime dt3 = new DateTime();
            string[] split1 = date.Split('/');

            dt3 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,Debit,credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and d.LedgerFK =l.LedgerPK and LedgerPK in('" + Ledgercode + "')and HeaderPK in('" + itemheadercode + "') and TransDate between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  order by isnull(l.priority,1000), l.sledgerName asc ";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[0, 5].Text = creditop;
                    FpSpread2.Sheets[0].Cells[0, 0].Text = "Opening Balance  ";
                    FpSpread2.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                    FpSpread2.Sheets[0].SpanModel.Add(0, 5, 1, 1);
                    FpSpread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread2.Sheets[0].Rows[0].BackColor = ColorTranslator.FromHtml("#4870BE");
                    FpSpread2.Sheets[0].Rows[0].ForeColor = Color.White;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].Rows.Count++;
                        FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i);

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = ds.Tables[0].Rows[i]["TransCode"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 3].Text = ds.Tables[0].Rows[i]["particulars"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
                    }
                    double credit = 0;
                    double debit = 0;
                    for (int i = 1; i < FpSpread2.Sheets[0].Rows.Count; i++)
                    {
                        if (credit == 0 && debit == 0)
                        {
                            credit = Convert.ToDouble(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                            debit = Convert.ToDouble(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);

                        }
                        else
                        {
                            credit = credit + Convert.ToDouble(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                            debit = debit + Convert.ToDouble(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);

                        }
                    }
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Rows.Count = FpSpread2.Sheets[0].RowCount + 1;
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 2), 4].Text = Convert.ToString(credit);
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 2), 5].Text = Convert.ToString(debit);
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 2), 0].Text = "Total";
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1), 0].Text = "Closing Balance  ";
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1), 5].Text = debitop;
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].Rows.Count - 1, 0, 1, 4);
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].Rows.Count - 1, 5, 1, 1);
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].Rows.Count - 2, 0, 1, 4);

                    FpSpread2.Sheets[0].Rows[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1)].BackColor = ColorTranslator.FromHtml("#4870BE");
                    FpSpread2.Sheets[0].Rows[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1)].ForeColor = Color.White;
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1), 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 2), 0].HorizontalAlign = HorizontalAlign.Right;
                }
                for (int i = 0; i < FpSpread2.Sheets[0].Columns.Count; i++)
                {
                    FpSpread2.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                }
                FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;
                FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
                FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
                FpSpread2.Sheets[0].Columns[6].Visible = false;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].Rows.Count;
                FpSpread2.ShowHeaderSelection = false;
                FpSpread2.SaveChanges();
            }
            else
            { }
        }
    }
    public void monthdate()
    {
        try
        {
            DateTime dt = new DateTime();
            FpSpread5.Sheets[0].RowCount = 0;
            FpSpread5.Sheets[0].ColumnCount = 0;
            FpSpread5.CommandBar.Visible = false;
            FpSpread5.Sheets[0].AutoPostBack = false;
            FpSpread5.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread5.Sheets[0].RowHeader.Visible = false;
            FpSpread5.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread5.Visible = true;
            string activerow = FpSpread4.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread4.ActiveSheetView.ActiveColumn.ToString();
            string creditop = FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            string debitop = FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Recipt NO";
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Particulars";
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
            FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
            FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
            {
                FpSpread5.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
            }
            FpSpread5.Sheets[0].ColumnHeader.Columns[0].Width = 50;
            FpSpread5.Sheets[0].ColumnHeader.Columns[1].Width = 128;
            FpSpread5.Sheets[0].ColumnHeader.Columns[2].Width = 128;
            FpSpread5.Sheets[0].ColumnHeader.Columns[3].Width = 265;
            FpSpread5.Sheets[0].ColumnHeader.Columns[4].Width = 145;
            FpSpread5.Sheets[0].ColumnHeader.Columns[5].Width = 145;
            if (cellclick2 == true)
            {
                string dte = "";
                string dtm = "";
                string date = FpSpread4.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                DateTime dt3 = new DateTime();
                string[] split1 = date.Split('/');
                dt3 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,Debit,credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and d.LedgerFK =l.LedgerPK and  TransDate between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  order by isnull(l.priority,1000), l.ledgerName asc ";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //// FpSpread5.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                        // FpSpread5.Sheets[0].Cells[0, 5].Text = creditop;
                        // FpSpread5.Sheets[0].Cells[0, 0].Text = "Opening Balance";
                        FpSpread5.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                        FpSpread5.Sheets[0].SpanModel.Add(0, 5, 1, 1);
                        // FpSpread5.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Right;
                        // FpSpread5.Sheets[0].Rows[0].BackColor = ColorTranslator.FromHtml("#4870BE");
                        // FpSpread5.Sheets[0].Rows[0].ForeColor = Color.White;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread5.Sheets[0].Rows.Count++;
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread5.Sheets[0].Rows.Count);
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 2].Text = ds.Tables[0].Rows[i]["TransCode"].ToString();
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 3].Text = ds.Tables[0].Rows[i]["particulars"].ToString();
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                            FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].Rows.Count - 1, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
                        }
                        int credit = 0;
                        int debit = 0;
                        for (int i = 0; i < FpSpread5.Sheets[0].Rows.Count; i++)
                        {
                            if (credit == 0 && debit == 0)
                            {
                                credit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                                debit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);

                            }
                            else
                            {
                                credit = credit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                                debit = debit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
                            }
                        }
                        FpSpread5.Sheets[0].RowCount++;
                        FpSpread5.Sheets[0].Rows.Count = FpSpread5.Sheets[0].RowCount + 1;
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 2), 4].Text = Convert.ToString(credit);
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 2), 5].Text = Convert.ToString(debit);
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 2), 0].Text = "Total";
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 1), 0].Text = "Closing Balance";
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 1), 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 2), 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread5.Sheets[0].Cells[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 1), 5].Text = debitop;

                        FpSpread5.Sheets[0].SpanModel.Add(FpSpread5.Sheets[0].Rows.Count - 1, 5, 1, 2);
                        FpSpread5.Sheets[0].SpanModel.Add(FpSpread5.Sheets[0].Rows.Count - 1, 0, 1, 4);
                        FpSpread5.Sheets[0].SpanModel.Add(FpSpread5.Sheets[0].Rows.Count - 2, 0, 1, 4);
                        FpSpread5.Sheets[0].Rows[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 1)].BackColor = ColorTranslator.FromHtml("#4870BE");
                        FpSpread5.Sheets[0].Rows[Convert.ToInt32(FpSpread5.Sheets[0].Rows.Count - 1)].ForeColor = Color.White;

                    }
                    else
                    { }
                }
                else
                { }

            }
            for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
            {
                FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

            }
            FpSpread5.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread5.Sheets[0].Columns[6].Visible = false;
            FpSpread5.ShowHeaderSelection = false;
            FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].Rows.Count;
            FpSpread5.SaveChanges();
        }
        catch { }

    }
    public void FpSpread4_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick2 = true;
        }
        catch
        { }

    }
    public void FpSpread4_Selectedindexchange(object sender, EventArgs e)
    {
        if (cellclick2 == true)
        {
            if (rb_month.Checked == true)
            {
                monthdate();
                // popwindow1.Visible = true;
                FpSpread5.Visible = true;
            }

        }

    }

    public void FpSpread5_OnCellClick(object sender, EventArgs e)
    {

    }
    public void FpSpread5_Selectedindexchange(object sender, EventArgs e)
    {

    }


    protected void btnsearch_Click(object sender, EventArgs e)
    {

        if (rb_date.Checked == true)
        {

            div1.Visible = true;
            monthdiv.Visible = false;
            Div2.Visible = false;
            dtdate2();
            dateWiseChart();
        }
        else
        {
            monthdiv.Visible = true;
            div1.Visible = false;

            dtmonth2();

        }
    }

    public void rbchange()
    {

        if (rb_date.Checked == true)
        {
            //FpSpread1.Sheets[0].Columns[1].Visible = true;
            //FpSpread1.Sheets[0].Columns[2].Visible = false;
        }

        if (rb_month.Checked == true)
        {

        }


    }
    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick = true;

        }
        catch
        {

        }

    }
    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        if (cellclick == true)
        {
            if (rb_date.Checked == true)
            {
                dateview();
                //popwindow1.Visible = true;
                FpSpread2.Visible = true;
            }

        }
    }
    protected void FpSpread2_OnCellClick(object sender, EventArgs e)
    {
        try
        {

            cellclick = true;

        }
        catch
        {

        }

    }
    protected void FpSpread2_Selectedindexchange(object sender, EventArgs e)
    {

    }
    protected void FpSpread3_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick1 = true;

        }
        catch
        {

        }

    }

    protected void FpSpread3_Selectedindexchange(object sender, EventArgs e)
    {
        if (cellclick1 == true)
        {
            if (rb_month.Checked == true)
            {
                monthview();
                //popwindow.Visible = true;
                FpSpread4.Visible = true;
                FpSpread5.Visible = false;
            }
        }
    }

    protected void rb_date_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        Div2.Visible = false;
        FpSpread5.Visible = false;
        monthdiv.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }
    protected void rb_month_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        div1.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }
    //public void bindmonth()
    //{
    //    try
    //    {


    //        cb_month.Checked = false;
    //        txt_month.Text = "Select All";



    //        if (cbl_month.Items.Count > 0)
    //        {
    //            for (i = 0; i < cbl_month.Items.Count; i++)
    //            {
    //                cbl_month.Items[i].Selected = true;
    //            }
    //            txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
    //            cb_month.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cb_month_CheckedChanged(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        cout = 0;
    //        txt_month.Text = "--Select--";
    //        if (cb_month.Checked == true)
    //        {
    //            cout++;
    //            for (i = 0; i < cbl_month.Items.Count; i++)
    //            {
    //                cbl_month.Items[i].Selected = true;
    //            }
    //            txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
    //        }
    //        else
    //        {
    //            for (i = 0; i < cbl_month.Items.Count; i++)
    //            {
    //                cbl_month.Items[i].Selected = false;
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cbl_month_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        i = 0;
    //        cb_month.Checked = false;
    //        commcount = 0;
    //        txt_month.Text = "---Select---";
    //        for (i = 0; i < cbl_month.Items.Count; i++)
    //        {
    //            if (cbl_month.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_month.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_month.Items.Count)
    //            {
    //                cb_month.Checked = true;
    //            }
    //            txt_month.Text = "Month(" + commcount.ToString() + ")";

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }


    //}
    //public void bindyear()
    //{
    //    try
    //    {


    //        cb_year.Checked = false;
    //        txt_year.Text = "Select All";



    //        if (cbl_year.Items.Count > 0)
    //        {
    //            for (i = 0; i < cbl_year.Items.Count; i++)
    //            {
    //                cbl_year.Items[i].Selected = true;
    //            }
    //            txt_year.Text = "Year(" + cbl_year.Items.Count + ")";
    //            cb_year.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cb_year_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        cout = 0;
    //        txt_year.Text = "--Select--";
    //        if (cb_year.Checked == true)
    //        {
    //            cout++;
    //            for (i = 0; i < cbl_year.Items.Count; i++)
    //            {
    //                cbl_year.Items[i].Selected = true;
    //            }
    //            txt_year.Text = "Year(" + cbl_year.Items.Count + ")";
    //        }
    //        else
    //        {
    //            for (i = 0; i < cbl_year.Items.Count; i++)
    //            {
    //                cbl_year.Items[i].Selected = false;
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //}
    //protected void cbl_year_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        i = 0;
    //        cb_year.Checked = false;
    //        commcount = 0;
    //        txt_year.Text = "---Select---";
    //        for (i = 0; i < cbl_year.Items.Count; i++)
    //        {
    //            if (cbl_year.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_year.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_year.Items.Count)
    //            {
    //                cb_year.Checked = true;
    //            }
    //            txt_year.Text = "Year(" + commcount.ToString() + ")";


    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;


    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        //  popwindow.Visible = false;
    }
    public void header1bind()
    {
        try
        {


            ds.Clear();
            cbl_header1.Items.Clear();
            // string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";

            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {


                cbl_header1.DataSource = ds;
                cbl_header1.DataTextField = "HeaderName";
                cbl_header1.DataValueField = "HeaderPK";
                cbl_header1.DataBind();

                cb_header1.Checked = true;

                if (cbl_header1.Items.Count > 0)
                {
                    for (i = 0; i < cbl_header1.Items.Count; i++)
                    {
                        cbl_header1.Items[i].Selected = true;

                    }
                    txt_header1.Text = "Header(" + cbl_header1.Items.Count + ")";
                    cb_header1.Checked = true;
                    // ledgerbind();

                }
            }

        }
        catch { }
    }
    public void ledger1bind()
    {
        try
        {
            string HeaderPK = "";

            for (i = 0; i < cbl_header1.Items.Count; i++)
            {

                if (cbl_header1.Items[i].Selected == true)
                {

                    if (HeaderPK == "")
                    {
                        HeaderPK = cbl_header1.Items[i].Value.ToString();
                    }
                    else
                    {
                        HeaderPK += "','" + cbl_header1.Items[i].Value.ToString();

                    }
                }
            }

            ds.Clear();
            cbl_ledger1.Items.Clear();
            //  string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger1.DataSource = ds;
                cbl_ledger1.DataTextField = "LedgerName";
                cbl_ledger1.DataValueField = "LedgerPK";
                cbl_ledger1.DataBind();

                if (cbl_ledger1.Items.Count > 0)
                {

                    for (int i = 0; i < cbl_ledger1.Items.Count; i++)
                    {
                        cbl_ledger1.Items[i].Selected = true;
                    }
                    txt_ledger1.Text = "Ledger(" + cbl_ledger1.Items.Count + ")";
                    cb_ledger1.Checked = true;
                }
            }


        }

        catch
        {
        }
    }
    protected void cb_header1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_header1.Text = "--Select--";
            if (cb_header1.Checked == true)
            {

                for (i = 0; i < cbl_header1.Items.Count; i++)
                {
                    cbl_header1.Items[i].Selected = true;
                }
                txt_header1.Text = "Header(" + (cbl_header1.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_header1.Items.Count; i++)
                {
                    cbl_header1.Items[i].Selected = false;
                }
                txt_header1.Text = "--Select--";
            }
            ledger1bind();

        }
        catch { }

    }
    protected void cbl_header1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header1.Checked = false;
            commcount = 0;
            txt_header1.Text = "--Select--";
            for (i = 0; i < cbl_header1.Items.Count; i++)
            {
                if (cbl_header1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header1.Items.Count)
                {
                    cb_header1.Checked = true;
                }
                txt_header1.Text = "Header(" + commcount.ToString() + ")";
            }
            ledger1bind();

        }
        catch { }
    }


    protected void cb_ledger1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_ledger1.Text = "--Select--";
            if (cb_ledger1.Checked == true)
            {

                for (i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = true;
                }
                txt_ledger1.Text = "Ledger(" + (cbl_ledger1.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = false;
                }
            }

        }
        catch { }

    }
    protected void cbl_ledger1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_ledger1.Checked = false;
            commcount = 0;
            txt_ledger1.Text = "--Select--";
            for (i = 0; i < cbl_ledger1.Items.Count; i++)
            {
                if (cbl_ledger1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledger1.Items.Count)
                {
                    cb_ledger1.Checked = true;
                }
                txt_ledger1.Text = "Ledger(" + commcount.ToString() + ")";
            }

        }
        catch { }


    }
    protected void btnsearch2_Click(object sender, EventArgs e)
    {


        fairpoin();
        string itemheadercode = "";
        for (int i = 0; i < cbl_header1.Items.Count; i++)
        {
            if (cbl_header1.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_header1.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header1.Items[i].Value.ToString() + "";
                }
            }
        }

        string Ledgercode = "";
        for (int i = 0; i < cbl_header1.Items.Count; i++)
        {
            if (cbl_header1.Items[i].Selected == true)
            {
                if (Ledgercode == "")
                {
                    Ledgercode = "" + cbl_header1.Items[i].Value.ToString() + "";
                }
                else
                {
                    Ledgercode = Ledgercode + "'" + "," + "'" + cbl_header1.Items[i].Value.ToString() + "";
                }
            }
        }

        string firstdate = Convert.ToString(txt_fromdate1.Text);
        string seconddate = Convert.ToString(txt_todate1.Text);
        DateTime dt = new DateTime();
        DateTime dt1 = new DateTime();
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        split = seconddate.Split('/');
        dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);



        FpSpread4.Sheets[0].RowCount = 0;
        FpSpread4.Sheets[0].ColumnCount = 0;
        FpSpread4.CommandBar.Visible = false;
        FpSpread4.Sheets[0].AutoPostBack = true;
        FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread4.Sheets[0].RowHeader.Visible = false;
        FpSpread4.Sheets[0].ColumnCount = 7;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread4.Visible = true;
        // Dateview.Visible = true;
        maindiv.Visible = true;

        string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
        string creditop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
        string debitop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;



        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Opening Balance";
        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Debit";
        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Closing Balance";


        for (int i = 0; i < FpSpread4.Sheets[0].Columns.Count; i++)
        {
            FpSpread4.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
            FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
            FpSpread4.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

        }

        FpSpread4.Sheets[0].ColumnHeader.Columns[0].Width = 51;
        FpSpread4.Sheets[0].ColumnHeader.Columns[1].Width = 140;
        FpSpread4.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        FpSpread4.Sheets[0].ColumnHeader.Columns[4].Width = 175;
        FpSpread4.Sheets[0].ColumnHeader.Columns[5].Width = 148;
        FpSpread4.Sheets[0].ColumnHeader.Columns[6].Width = 119;




        string selqry = " SELECT SUM(Debit)as Debit,ISNULL( sum(Credit),0)as Credit,SUM(Debit)-ISNULL( sum(Credit),0) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' GROUP BY TransDate";
        selqry = selqry + " SELECT SUM(Debit)-ISNULL( sum(Credit),0) OpeningBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction D WHERE TransDate < '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' GROUP BY TransDate ";
        ds = d2.select_method_wo_parameter(selqry, "Text");




        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string opningbal = "";
                FpSpread4.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (i == 0)
                    {
                        opningbal = "";
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                        }
                        else
                        {
                            opningbal = "0";
                        }
                        if (rb_month.Checked == true)
                        {
                            FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 2].Text = Convert.ToString(opningbal);
                            FpSpread4.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        }

                    }
                    else
                    {
                        if (rb_month.Checked == true)
                        {

                            FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
                            FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
                            FpSpread4.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        }


                    }
                }
                for (int i = 0; i < FpSpread4.Sheets[0].Columns.Count; i++)
                {
                    FpSpread4.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                }
                FpSpread4.Sheets[0].Columns[6].Visible = false;
                FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].Rows.Count;
                FpSpread4.Visible = true;
                FpSpread4.SaveChanges();

            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread4.Visible = false;

            }
        }

        else
        {

            lbl_alert1.Text = "No Records Found";
            FpSpread4.Visible = false;

        }


        //rbchange();

        ;



    }
    protected void txt_fromdate1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate1, txt_todate1);
        }
        catch (Exception ex)
        {
        }

    }
    protected void txt_todate1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate1, txt_todate1);
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        //popwindow1.Visible = false;
    }

    public void header2bind()
    {
        try
        {


            ds.Clear();
            cbl_header2.Items.Clear();
            //   string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {


                cbl_header2.DataSource = ds;
                cbl_header2.DataTextField = "HeaderName";
                cbl_header2.DataValueField = "HeaderPK";
                cbl_header2.DataBind();

                cb_header2.Checked = true;

                if (cbl_header2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_header2.Items.Count; i++)
                    {
                        cbl_header2.Items[i].Selected = true;

                    }
                    txt_header2.Text = "Header(" + cbl_header2.Items.Count + ")";
                    cb_header2.Checked = true;

                }
            }

        }
        catch { }
    }
    public void ledger2bind()
    {
        try
        {
            string HeaderPK = "";

            for (i = 0; i < cbl_header2.Items.Count; i++)
            {

                if (cbl_header2.Items[i].Selected == true)
                {

                    if (HeaderPK == "")
                    {
                        HeaderPK = cbl_header2.Items[i].Value.ToString();
                    }
                    else
                    {
                        HeaderPK += "','" + cbl_header2.Items[i].Value.ToString();

                    }
                }
            }

            ds.Clear();
            cbl_ledger2.Items.Clear();
            //string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger2.DataSource = ds;
                cbl_ledger2.DataTextField = "LedgerName";
                cbl_ledger2.DataValueField = "LedgerPK";
                cbl_ledger2.DataBind();

                if (cbl_ledger2.Items.Count > 0)
                {

                    for (int i = 0; i < cbl_ledger2.Items.Count; i++)
                    {
                        cbl_ledger2.Items[i].Selected = true;
                    }
                    txt_ledger2.Text = "Ledger(" + cbl_ledger2.Items.Count + ")";
                    cb_ledger2.Checked = true;
                }
            }


        }

        catch
        {
        }
    }
    protected void cb_header2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_header2.Text = "--Select--";
            if (cb_header2.Checked == true)
            {

                for (i = 0; i < cbl_header2.Items.Count; i++)
                {
                    cbl_header2.Items[i].Selected = true;
                }
                txt_header2.Text = "Header(" + (cbl_header2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_header2.Items.Count; i++)
                {
                    cbl_header2.Items[i].Selected = false;
                }
                txt_header2.Text = "--Select--";
            }
            ledger2bind();

        }
        catch { }

    }
    protected void cbl_header2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header2.Checked = false;
            commcount = 0;
            txt_header2.Text = "--Select--";
            for (i = 0; i < cbl_header2.Items.Count; i++)
            {
                if (cbl_header2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header2.Items.Count)
                {
                    cb_header2.Checked = true;
                }
                txt_header2.Text = "Header(" + commcount.ToString() + ")";
            }
            ledger2bind();

        }
        catch { }
    }
    protected void cb_ledger2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_ledger2.Text = "--Select--";
            if (cb_ledger2.Checked == true)
            {

                for (i = 0; i < cbl_ledger2.Items.Count; i++)
                {
                    cbl_ledger2.Items[i].Selected = true;
                }
                txt_ledger2.Text = "Ledger(" + (cbl_ledger2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_ledger2.Items.Count; i++)
                {
                    cbl_ledger2.Items[i].Selected = false;
                }
            }

        }
        catch { }

    }
    protected void cbl_ledger2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_ledger2.Checked = false;
            commcount = 0;
            txt_ledger2.Text = "--Select--";
            for (i = 0; i < cbl_ledger2.Items.Count; i++)
            {
                if (cbl_ledger2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledger2.Items.Count)
                {
                    cb_ledger2.Checked = true;
                }
                txt_ledger2.Text = "Ledger(" + commcount.ToString() + ")";
            }

        }
        catch { }


    }

    protected void txt_fromdate2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate2, txt_todate2);
        }
        catch (Exception ex)
        {
        }

    }
    protected void txt_todate2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate2, txt_todate2);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnsearch3_Click(object sender, EventArgs e)
    {
        FpSpread5.Visible = false;
        FpSpread2.Visible = false;
        // FpSpread5.Visible = true;
        string itemheadercode = "";
        for (int i = 0; i < cbl_header2.Items.Count; i++)
        {
            if (cbl_header2.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_header2.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header2.Items[i].Value.ToString() + "";
                }
            }
        }

        string Ledgercode = "";
        for (int i = 0; i < cbl_header2.Items.Count; i++)
        {
            if (cbl_header2.Items[i].Selected == true)
            {
                if (Ledgercode == "")
                {
                    Ledgercode = "" + cbl_header2.Items[i].Value.ToString() + "";
                }
                else
                {
                    Ledgercode = Ledgercode + "'" + "," + "'" + cbl_header2.Items[i].Value.ToString() + "";
                }
            }
        }

        string firstdate = Convert.ToString(txt_fromdate2.Text);
        string seconddate = Convert.ToString(txt_todate2.Text);
        DateTime dt = new DateTime();
        DateTime dt1 = new DateTime();
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        split = seconddate.Split('/');
        dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

        FpSpread5.Sheets[0].RowCount = 0;

        FpSpread5.Sheets[0].ColumnCount = 0;
        FpSpread5.CommandBar.Visible = false;
        FpSpread5.Sheets[0].AutoPostBack = true;
        FpSpread5.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread5.Sheets[0].RowHeader.Visible = false;
        FpSpread5.Sheets[0].ColumnCount = 7;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread5.Visible = true;
        //  popwindow1.Visible = true;
        maindiv2.Visible = true;



        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Recipt NO";
        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Particulars";
        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
        FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);





        for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
        {
            FpSpread5.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
            FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
            FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

        }

        FpSpread5.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        FpSpread5.Sheets[0].ColumnHeader.Columns[1].Width = 128;
        FpSpread5.Sheets[0].ColumnHeader.Columns[2].Width = 128;
        FpSpread5.Sheets[0].ColumnHeader.Columns[3].Width = 220;
        FpSpread5.Sheets[0].ColumnHeader.Columns[4].Width = 145;
        FpSpread5.Sheets[0].ColumnHeader.Columns[5].Width = 145;



        string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,Debit,credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and LedgerPK in('" + Ledgercode + "')and HeaderPK in('" + itemheadercode + "')  and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  order by isnull(l.priority,1000), l.ledgerName asc ";

        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread5.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    FpSpread5.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);

                    FpSpread5.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
                    FpSpread5.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["TransCode"].ToString();
                    FpSpread5.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["particulars"].ToString();
                    FpSpread5.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                    FpSpread5.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();


                    // lbl_alert1.Visible = false;

                }



                int credit = 0;
                int debit = 0;


                for (int i = 0; i < FpSpread5.Sheets[0].Rows.Count; i++)
                {
                    if (credit == 0 && debit == 0)
                    {
                        credit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                        debit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);

                    }
                    else
                    {
                        credit = credit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                        debit = debit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);

                    }

                }


            }

            else
            {

                lbl_alert1.Text = "No Records Found";
                FpSpread5.Visible = false;

            }
            for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
            {
                FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
            }
            FpSpread5.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread5.Sheets[0].Columns[6].Visible = false;
            FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].Rows.Count;
            FpSpread5.ShowHeaderSelection = false;
            FpSpread5.Visible = true;
            FpSpread5.SaveChanges();
        }
        else
        {

            lbl_alert1.Text = "No Records Found";
            FpSpread5.Visible = false;

        }


        //rbchange();



    }


    //protected void btnExcel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string reportname = txtexcelname.Text;
    //        if (reportname.ToString().Trim() != "")
    //        {
    //            if (FpSpread5.Visible == true)
    //            {
    //                d2.printexcelreport(FpSpread5, reportname);
    //            }
    //            else if (FpSpread4.Visible == true)
    //            {
    //                d2.printexcelreport(FpSpread4, reportname);

    //            }
    //            else 
    //            {
    //                d2.printexcelreport(FpSpread3, reportname);              

    //            }
    //            lbl_norec.Visible = false;
    //        }
    //        else
    //        {
    //            lbl_norec.Text = "Please Enter Your Report Name";
    //            lbl_norec.Visible = true;
    //            txtexcelname.Focus();
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    //public void btnprintmaster_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string dptname = "CriditdebitReport";
    //        string pagename = "CreditdebitReport.aspx";
    //        if (FpSpread2.Visible == true)
    //        {
    //            Printcontrol.loadspreaddetails(FpSpread2, pagename, dptname);

    //        }
    //        else
    //         {
    //             Printcontrol.loadspreaddetails(FpSpread5, pagename, dptname);

    //        }
    //       Printcontrol.Visible = true;
    //       lbl_norec.Visible = false;

    //    }
    //    catch
    //    {
    //    }
    //}
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (rb_date.Checked == true)
            {
                if (reportname.Trim() != "")
                {
                    if (FpSpread1.Visible == true)
                    {
                        d2.printexcelreport(FpSpread1, reportname);
                    }
                    if (FpSpread2.Visible == true)
                    {
                        d2.printexcelreport(FpSpread2, reportname);
                    }
                    lbl_norec1.Visible = false;
                }
                else
                {

                    lbl_norec1.Text = "Please Enter Your DateWise Report Name";
                    lbl_norec1.Visible = true;
                    txtexcelname1.Focus();

                }
            }
            else if (rb_month.Checked == true)
            {
                if (reportname.Trim() != "")
                {
                    if (FpSpread3.Visible == true)
                    {
                        d2.printexcelreport(FpSpread3, reportname);
                    }
                    if (FpSpread4.Visible == true)
                    {
                        d2.printexcelreport(FpSpread4, reportname);
                    }
                    if (FpSpread5.Visible == true)
                    {
                        d2.printexcelreport(FpSpread5, reportname);
                    }
                    lbl_norec1.Visible = false;
                }
                else
                {
                    lbl_norec1.Text = "Please Enter Your MonthWise Report Name";
                    lbl_norec1.Visible = true;
                    txtexcelname1.Focus();
                }
            }

        }
        catch
        {

        }
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "CriditdebitReport";
            string pagename = "CreditdebitReport.aspx";
            if (rb_date.Checked == true)
            {
                if (FpSpread1.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
                }
                if (FpSpread2.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(FpSpread2, pagename, dptname);
                }
                Printcontrol1.Visible = true;
                lbl_norec1.Visible = false;
            }
            else if (rb_month.Checked == true)
            {
                if (FpSpread3.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(FpSpread3, pagename, dptname);
                }
                if (FpSpread4.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(FpSpread4, pagename, dptname);
                }
                if (FpSpread5.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(FpSpread5, pagename, dptname);
                }
                Printcontrol1.Visible = true;
                lbl_norec1.Visible = false;
            }
        }
        catch
        {
        }
    }



    protected void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "CriditdebitReport";
            string pagename = "CreditdebitReport.aspx";

            Printcontrol1.loadspreaddetails(FpSpread4, pagename, dptname);
            Printcontrol2.Visible = true;
            lbl_norec2.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreport(FpSpread4, reportname);


                lbl_norec2.Visible = false;
            }
            else
            {
                lbl_norec2.Text = "Please Enter Your Report Name";
                lbl_norec2.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch
        {

        }
    }
    public void Txt_vendore_TextChanged(object sender, EventArgs e)
    { }

    public void Txt_student_TextChanged(object sender, EventArgs e)
    { }
    public void Txt_Staff_TextChanged(object sender, EventArgs e)
    { }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaff(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";

        name = ws.Getname(query);

        return name;
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname3(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT VendorCompName FROM CO_VendorMaster WHERE VendorType IN (1,2) and VendorCompName like '" + prefixText + "%' ";

        name = ws.Getname(query);

        return name;
    }

    #region Chart report

    private void dateWiseChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int sel = 0;
                int col = 0;
                int row = 0;
                int colvalue = 0;
                int width = 0;
                string openbal = "";
                string closgbal = "";
                string credit = "";
                string debit = "";
                string fromdate = "";
                string todates = "";
                string totamt = "";
                string headerid = "";
                string ledgerid = "";
                bool value = false;
                DataTable dtchart = new DataTable();
                DataColumn dtcol = new DataColumn();
                DataColumn dtcol1 = new DataColumn();
                DataRow dtrow = dtchart.NewRow();
                DataView dvdate = new DataView();
                DataView dvopen = new DataView();
                List<string> list = new List<string>();
                List<string> listrow = new List<string>();
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                headerid = getCheckboxvalue(cbl_header);
                ledgerid = getCheckboxvalue(cbl_ledger);
                fromdate = Convert.ToString(txt_fromdate.Text);
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
                dtchart.Columns.Clear();
                chart.ChartAreas[0].AxisX.Title = "DateWise";
                chart.ChartAreas[0].AxisY.Title = "Amount";
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

                dtchart.Columns.Add(dtcol1);
                int count = 0;
                while (dt <= dt1)
                {

                    dtchart.Columns.Add(dt.ToString("dd/MM/yyyy"));
                    ListItem newli = new ListItem(Convert.ToString(count), Convert.ToString(dt));
                    list.Add(Convert.ToString(newli));
                    dt = dt.AddDays(1);
                }
                if (dtchart.Columns.Count > 0)
                {
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Opening Balance";
                    chart.Series.Add("Opening Balance");
                    dtchart.Rows.Add(dtrow);
                    ListItem li1 = new ListItem(Convert.ToString("Opening Balance"));
                    listrow.Add(Convert.ToString(li1));

                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Credit";
                    chart.Series.Add("Credit");
                    dtchart.Rows.Add(dtrow);
                    ListItem li2 = new ListItem(Convert.ToString("Credit"));
                    listrow.Add(Convert.ToString(li2));

                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Debit";
                    chart.Series.Add("Debit");
                    dtchart.Rows.Add(dtrow);
                    ListItem li3 = new ListItem(Convert.ToString("Debit"));
                    listrow.Add(Convert.ToString(li3));

                    dtrow = dtchart.NewRow();
                    chart.Series.Add("Closing Balance");
                    dtrow[0] = "Closing Balance";
                    dtchart.Rows.Add(dtrow);
                    ListItem li4 = new ListItem(Convert.ToString("Closing Balance"));
                    listrow.Add(Convert.ToString(li4));
                }
                totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate >= '" + dt + "' and LedgerFK in('" + ledgerid + "') and HeaderFK in('" + headerid + "')");
                // totamt = Convert.ToString(totamt);
                if (dtchart.Rows.Count > 0)
                {
                    bool openBalance = true;
                    for (sel = 1; sel < dtchart.Columns.Count; sel++)
                    {

                        string coldate = dtchart.Columns[sel].ColumnName;
                        //  DateTime dtm = new DateTime();
                        //dtm = Convert.ToDateTime(coldate);
                        // string name = dtm.ToString("MMM"); ;
                        // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(coldate));
                        // DateTime.Now.M
                        ds.Tables[0].DefaultView.RowFilter = "TransDate='" + Convert.ToString(coldate) + "'";
                        dvdate = ds.Tables[0].DefaultView;
                        if (dvdate.Count > 0 && dvdate.Count != null)
                        {
                            width += 400;
                            if (openBalance)
                            {
                                openBalance = false;
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    openbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                                    dtchart.Rows[colvalue][sel] = Convert.ToString(openbal);
                                }
                                else
                                {
                                    openbal = Convert.ToString(totamt);
                                    dtchart.Rows[0][colvalue] = Convert.ToString(openbal);
                                }
                                if (openbal == "")
                                    openbal = "0";
                                credit = Convert.ToString(dvdate[0]["Credit"]);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                debit = Convert.ToString(dvdate[0]["Debit"]);
                                if (credit == "")
                                    credit = "0";
                                if (debit == "")
                                    debit = "0";
                                colvalue = Convert.ToInt32(listrow.IndexOf("Credit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(credit);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Debit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(debit);
                                closgbal = Convert.ToString(Convert.ToDouble(openbal) + Convert.ToDouble(debit) - Convert.ToDouble(credit));
                                colvalue = Convert.ToInt32(listrow.IndexOf("Closing Balance"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(closgbal);
                                openbal = closgbal;
                            }
                            else
                            {
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                openbal = Convert.ToString(openbal);
                                dtchart.Rows[colvalue][sel] = Convert.ToString(openbal);
                                credit = Convert.ToString(dvdate[0]["Credit"]);
                                debit = Convert.ToString(dvdate[0]["Debit"]);
                                //closgbal = Convert.ToString(dvdate[0]["ClosingBal"]);
                                if (credit == "")
                                    credit = "0";
                                if (debit == "")
                                    debit = "0";
                                colvalue = Convert.ToInt32(listrow.IndexOf("Credit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(credit);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Debit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(debit);
                                closgbal = Convert.ToString(Convert.ToDouble(openbal) + Convert.ToDouble(debit) - Convert.ToDouble(credit));
                                colvalue = Convert.ToInt32(listrow.IndexOf("Closing Balance"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(closgbal);
                            }
                            value = true;
                        }
                    }
                }
                if (value == true)
                {
                    for (row = 0; row < dtchart.Rows.Count; row++)
                    {
                        for (col = 1; col < dtchart.Columns.Count; col++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                            chart.Series[row].ChartType = SeriesChartType.Column;
                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;
                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Width = Convert.ToInt32(width);
                    chart.Height = Convert.ToInt32(350);
                }

            }
            else
            {
            }
        }
        catch { }
    }

    private void monthWiseChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int sel = 0;
                int col = 0;
                int row = 0;
                int colvalue = 0;
                int width = 0;
                string openbal = "";
                string closgbal = "";
                string credit = "";
                string debit = "";
                string fromdate = "";
                string todates = "";
                string totamt = "";
                string headerid = "";
                string ledgerid = "";
                bool value = false;
                DataTable dtchart = new DataTable();
                DataColumn dtcol = new DataColumn();
                DataColumn dtcol1 = new DataColumn();
                DataRow dtrow = dtchart.NewRow();
                DataView dvdate = new DataView();
                DataView dvopen = new DataView();
                List<string> list = new List<string>();
                List<string> listrow = new List<string>();
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                headerid = getCheckboxvalue(cbl_header);
                ledgerid = getCheckboxvalue(cbl_ledger);
                fromdate = Convert.ToString(txt_fromdate.Text);
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
                dtchart.Columns.Clear();
                chart.ChartAreas[0].AxisX.Title = "MonthWise";
                chart.ChartAreas[0].AxisY.Title = "Amount";
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

                dtchart.Columns.Add(dtcol1);
                int count = 0;
                while (dt < dt1)
                {
                    dtchart.Columns.Add(dt.ToString("MM-yyyy").TrimStart('0'));
                    ListItem newli = new ListItem(Convert.ToString(count), Convert.ToString(dt));
                    list.Add(Convert.ToString(newli));
                    dt = dt.AddMonths(1);

                }
                if (dtchart.Columns.Count > 0)
                {
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Opening Balance";
                    chart.Series.Add("Opening Balance");
                    dtchart.Rows.Add(dtrow);
                    ListItem li1 = new ListItem(Convert.ToString("Opening Balance"));
                    listrow.Add(Convert.ToString(li1));

                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Credit";
                    chart.Series.Add("Credit");
                    dtchart.Rows.Add(dtrow);
                    ListItem li2 = new ListItem(Convert.ToString("Credit"));
                    listrow.Add(Convert.ToString(li2));

                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Debit";
                    chart.Series.Add("Debit");
                    dtchart.Rows.Add(dtrow);
                    ListItem li3 = new ListItem(Convert.ToString("Debit"));
                    listrow.Add(Convert.ToString(li3));

                    dtrow = dtchart.NewRow();
                    chart.Series.Add("Closing Balance");
                    dtrow[0] = "Closing Balance";
                    dtchart.Rows.Add(dtrow);
                    ListItem li4 = new ListItem(Convert.ToString("Closing Balance"));
                    listrow.Add(Convert.ToString(li4));
                }
                totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate >= '" + dt + "' and LedgerFK in('" + ledgerid + "') and HeaderFK in('" + headerid + "')");
                if (dtchart.Rows.Count > 0)
                {
                    bool openBalance = true;
                    for (sel = 1; sel < dtchart.Columns.Count; sel++)
                    {

                        string coldate = dtchart.Columns[sel].ColumnName;
                        //string[] split = coldate.Split('/');
                        //if (split.Length > 0)
                        //{
                        //    coldate = split[1].ToString() + "-" + split[2].ToString();
                        //}
                        ds.Tables[0].DefaultView.RowFilter = "MonthYear='" + Convert.ToString(coldate.TrimStart('0')) + "'";
                        dvdate = ds.Tables[0].DefaultView;
                        if (dvdate.Count > 0 && dvdate.Count != null)
                        {
                            width += 400;
                            if (openBalance)
                            {
                                openBalance = false;
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    openbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                                    dtchart.Rows[colvalue][sel] = Convert.ToString(openbal);
                                }
                                else
                                {
                                    openbal = Convert.ToString(totamt);
                                    dtchart.Rows[0][colvalue] = Convert.ToString(openbal);
                                }
                                if (openbal == "")
                                    openbal = "0";
                                credit = Convert.ToString(dvdate[0]["Credit"]);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                debit = Convert.ToString(dvdate[0]["Debit"]);
                                if (credit == "")
                                    credit = "0";
                                if (debit == "")
                                    debit = "0";
                                colvalue = Convert.ToInt32(listrow.IndexOf("Credit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(credit);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Debit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(debit);
                                closgbal = Convert.ToString(Convert.ToDouble(openbal) + Convert.ToDouble(debit) - Convert.ToDouble(credit));
                                colvalue = Convert.ToInt32(listrow.IndexOf("Closing Balance"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(closgbal);
                                openbal = closgbal;
                            }
                            else
                            {
                                colvalue = Convert.ToInt32(listrow.IndexOf("Opening Balance"));
                                openbal = Convert.ToString(openbal);
                                dtchart.Rows[colvalue][sel] = Convert.ToString(openbal);
                                credit = Convert.ToString(dvdate[0]["Credit"]);
                                debit = Convert.ToString(dvdate[0]["Debit"]);
                                if (credit == "")
                                    credit = "0";
                                if (debit == "")
                                    debit = "0";
                                colvalue = Convert.ToInt32(listrow.IndexOf("Credit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(credit);
                                colvalue = Convert.ToInt32(listrow.IndexOf("Debit"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(debit);
                                closgbal = Convert.ToString(Convert.ToDouble(openbal) + Convert.ToDouble(debit) - Convert.ToDouble(credit));
                                colvalue = Convert.ToInt32(listrow.IndexOf("Closing Balance"));
                                dtchart.Rows[colvalue][sel] = Convert.ToString(closgbal);
                            }
                            value = true;
                        }
                    }
                }
                if (value == true)
                {
                    for (row = 0; row < dtchart.Rows.Count; row++)
                    {
                        for (col = 1; col < dtchart.Columns.Count; col++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                            chart.Series[row].ChartType = SeriesChartType.Column;
                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;
                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Width = Convert.ToInt32(width);
                    chart.Height = Convert.ToInt32(350);
                }

            }
            else
            {
            }
        }
        catch { }
    }
    private string getCheckboxvalue(CheckBoxList cbl)
    {
        System.Text.StringBuilder cblSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    if (cblSelected.Length == 0)
                    {
                        cblSelected.Append(cbl.Items[sel].Value);
                    }
                    else
                    {
                        cblSelected.Append("','" + cbl.Items[sel].Value);
                    }
                }
            }
        }
        catch { cbl.Items.Clear(); }
        return cblSelected.ToString();
    }

    //private string getMonth()
    //{
    //}
    #endregion
}