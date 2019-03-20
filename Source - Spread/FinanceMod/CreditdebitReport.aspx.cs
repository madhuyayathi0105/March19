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

public partial class CreditdebitReport : System.Web.UI.Page
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
    ReuasableMethods reuse = new ReuasableMethods();
    Boolean cellclick = false;
    Boolean cellclick1 = false;
    Boolean cellclick2 = false;
    Boolean cellclick3 = false;
    int commcount;
    int i;
    int cout;
    int row;
    bool usBasedRights = false;
    DataTable dtCreditDebitReport = new DataTable();
    DataRow drowInst;
    ArrayList arrColHdrNames = new ArrayList();
    Dictionary<int, string> dicRowColor = new Dictionary<int, string>();
    Dictionary<int, string> dicColumnVisible = new Dictionary<int, string>();
    Hashtable hsPaymodeHeader = new Hashtable();

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
            //collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
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
                UserbasedRights();
                loadpaid();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
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
            // datevalidate(txt_fromdate, txt_todate);
            paymentDateCheck();
        }
        catch (Exception ex)
        {
        }
    }

    protected void paymentDateCheck()
    {
        try
        {
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            DateTime fdt = new DateTime();
            string[] split = firstdate.Split('/');
            string frdt = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);
            dt = Convert.ToDateTime(frdt);
            split = seconddate.Split('/');
            string todt = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);
            dt1 = Convert.ToDateTime(todt);
            string findt = "";
            bool dtcheck = false;
            string fincyr = d2.getCurrentFinanceYear(usercode, collegecode1);
            //if (fincyr != "0")
            //{
            //    findt = d2.GetFunction("select CONVERT(varchar(10),finyearstart,103) as findt from FM_FinYearMaster where FinYearPK='" + fincyr + "'");
            //    // +'-'+CONVERT(varchar(10),FinYearEnd,103)
            //    string[] spfindt = findt.Split('/');
            //    findt = spfindt[1] + "/" + spfindt[0] + "/" + spfindt[2];
            //    fdt = Convert.ToDateTime(findt);
            //    while (dt >= fdt)
            //    {
            //        dtcheck = true;
            //        break;
            //    }
            //    if (dtcheck == false)
            //    {
            //        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        imgdiv2.Visible = true;
            //        lbl_alert1.Text = "From date Sholud Be Greater Than Current Financial Year Start Date";
            //    }
            //}
        }
        catch { }
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
            //   string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "  and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            //AND  Ledgermode='0'
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
            // string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
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
        GrdDateWise.Visible = false;
        GrdHeaderWise.Visible = false;
        GrdMonthCellClick.Visible = false;
        GrdLedger.Visible = false;
    }

    #region Date Wise

    public void dtdate2()
    {
        fairpoin();
        UserbasedRights();
        GrdMonth.Visible = false;
        GrdLedger.Visible = false;
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

        string userCode = "";
        if (usBasedRights == true)
            userCode = " and EntryUserCode in('" + usercode + "')";
        string firstdate = Convert.ToString(txt_fromdate.Text);
        string seconddate = Convert.ToString(txt_todate.Text);
        DateTime dtfrom = new DateTime();
        DateTime dt = new DateTime();
        DateTime dt1 = new DateTime();
        DateTime fdt = new DateTime();
        string[] split = firstdate.Split('/');
        string frdt = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);
        dt = Convert.ToDateTime(frdt);
        dtfrom = Convert.ToDateTime(frdt);
        split = seconddate.Split('/');
        string todt = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);
        dt1 = Convert.ToDateTime(todt);

        arrColHdrNames.Add("S.No");
        dtCreditDebitReport.Columns.Add("S.No");
        arrColHdrNames.Add("Date");
        dtCreditDebitReport.Columns.Add("Date");
        arrColHdrNames.Add("Opening Balance");
        dtCreditDebitReport.Columns.Add("Opening Balance");
        arrColHdrNames.Add("Credit");
        dtCreditDebitReport.Columns.Add("Credit");
        arrColHdrNames.Add("Debit");
        dtCreditDebitReport.Columns.Add("Debit");
        arrColHdrNames.Add("Closing Balance");
        dtCreditDebitReport.Columns.Add("Closing Balance");
        DataRow drHdr1 = dtCreditDebitReport.NewRow();
        for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
        {
            drHdr1[grCol] = arrColHdrNames[grCol];
        }
        dtCreditDebitReport.Rows.Add(drHdr1);
        //ds.Clear();
        string totamt = "";
        string selqry = "";
        string debitvalue = "";
        string creditvalue = "";
        string opningbal = "";
        double closingbal = 0;
        string findt = "";
        bool dtcheck = false;
        string fincyr = d2.getCurrentFinanceYear(usercode, collegecode1);

        //Commented by Saranya on 9April2018
        //if (fincyr != "0")
        //{
        //    findt = d2.GetFunction("select CONVERT(varchar(10),finyearstart,103) as findt from FM_FinYearMaster where FinYearPK='" + fincyr + "'");
        //    // +'-'+CONVERT(varchar(10),FinYearEnd,103)
        //    if (findt != "0")
        //    {
        //        string[] spfindt = findt.Split('/');
        //        findt = spfindt[1] + "/" + spfindt[0] + "/" + spfindt[2];
        //        fdt = Convert.ToDateTime(findt);
        //        while (dt >= fdt)
        //        {
        //            dtcheck = true;
        //            if (dt > fdt)
        //            {
        //                dt = dt.AddDays(-1);
        //            }
        //            break;
        //        }
        //    }
        //}
        //if (dtcheck == true)
        //{
        if (rb_date.Checked == true)
        {

            //selqry = " SELECT SUM(Debit)as Debit,SUM(credit)as Credit,(SUM(Debit)-SUM(credit)) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and TransDate Between '" + dtfrom.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " GROUP BY TransDate order by cast(transdate as datetime)";

            //selqry = selqry + " SELECT SUM(Debit)-SUM(credit) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate between '" + fdt.ToString("MM/dd/yyyy") + "' and '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " ";
            //ds = d2.select_method_wo_parameter(selqry, "Text");

            //totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate >= '" + dt + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");
            //totamt = Convert.ToString(totamt);

            selqry = " SELECT SUM(Debit)as Debit,SUM(credit)as Credit,(SUM(Debit)-SUM(credit)) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction f,registration r WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and TransDate Between '" + frdt + "' AND '" + todt + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code='" + collegecode1 + "' and  r.app_no=F.app_no and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 GROUP BY TransDate order by cast(transdate as datetime)";

            selqry = selqry + " SELECT SUM(Debit)-SUM(credit) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate between '" + frdt + "' and '" + todt + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " ";
            ds = d2.select_method_wo_parameter(selqry, "Text");

            totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate >= '" + frdt + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");
            totamt = Convert.ToString(totamt);
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    opningbal = "";
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (dt == fdt)
                            opningbal = "0";
                        else
                            opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                    }
                    else
                    {
                        opningbal = totamt;
                    }
                    if (rb_date.Checked == true)
                    {
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        if (opningbal == "")
                        {
                            opningbal = "0";
                        }
                        drowInst[2] = Convert.ToString(opningbal);
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
                        drowInst[3] = Convert.ToString(debitvalue);
                        drowInst[4] = Convert.ToString(creditvalue);
                        closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                        drowInst[5] = Convert.ToString(closingbal);
                        // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        opningbal = Convert.ToString(closingbal);
                    }
                }
                else
                {
                    if (rb_date.Checked == true)
                    {
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        if (opningbal == "")
                        {
                            opningbal = "0";
                        }
                        drowInst[2] = Convert.ToString(opningbal);
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
                        drowInst[3] = Convert.ToString(debitvalue);
                        drowInst[4] = Convert.ToString(creditvalue);
                        closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                        // FpSpread1.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                        drowInst[5] = Convert.ToString(closingbal);
                        // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        opningbal = Convert.ToString(closingbal);
                    }
                }
                dtCreditDebitReport.Rows.Add(drowInst);
            }
            grdCreditReport.DataSource = dtCreditDebitReport;
            grdCreditReport.DataBind();
            grdCreditReport.Visible = true;

            grdCreditReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdCreditReport.Rows[0].Font.Bold = true;
            grdCreditReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            rbchange();
            rptprint1.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert1.Text = "No Records Found";
            grdCreditReport.Visible = false;
            div1.Visible = false;
            rptprint1.Visible = false;
        }
        //}
        //else
        //{
        //    grdCreditReport.Visible = false;
        //    div1.Visible = false;
        //    rptprint1.Visible = false;
        //    imgdiv2.Visible = true;
        //    lbl_alert1.Text = "From date Sholud Be Greater Than Current Financial Year Start Date";
        //}

    }

    protected void grdCreditReport_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdCreditReport_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            divCellClickDate.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            fairpoin();
            UserbasedRights();
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
            GrdMonth3CellClick.Visible = false;

            arrColHdrNames.Add("S.No");
            dtCreditDebitReport.Columns.Add("S.No");
            arrColHdrNames.Add("Date");
            dtCreditDebitReport.Columns.Add("Date");
            arrColHdrNames.Add("Recipt NO");
            dtCreditDebitReport.Columns.Add("Recipt NO");
            arrColHdrNames.Add("Particulars");
            dtCreditDebitReport.Columns.Add("Particulars");
            arrColHdrNames.Add("Credit");
            dtCreditDebitReport.Columns.Add("Credit");
            arrColHdrNames.Add("Debit");
            dtCreditDebitReport.Columns.Add("Debit");
            DataRow drHdr1 = dtCreditDebitReport.NewRow();
            for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtCreditDebitReport.Rows.Add(drHdr1);

            string creditop = grdCreditReport.Rows[rowIndex].Cells[2].Text;
            string debitop = grdCreditReport.Rows[rowIndex].Cells[5].Text;

            string userCode = "";
            if (usBasedRights == true)
                userCode = " and EntryUserCode in('" + usercode + "')";

            string dte = "";
            string dtm = "";
            string date = grdCreditReport.Rows[rowIndex].Cells[1].Text;
            DateTime dt3 = new DateTime();
            string[] split1 = date.Split('/');

            dt3 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,ISNULL(Debit,'0')as Debit,ISNULL(credit,'0') as credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and d.LedgerFK =l.LedgerPK and LedgerPK in('" + Ledgercode + "')and HeaderPK in('" + itemheadercode + "') and TransDate between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "  order by isnull(l.priority,1000), l.ledgerName asc ";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    drowInst = dtCreditDebitReport.NewRow();
                    drowInst[5] = creditop;
                    drowInst[0] = "Opening Balance  ";
                    dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Balance");
                    dtCreditDebitReport.Rows.Add(drowInst);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        drowInst[2] = ds.Tables[0].Rows[i]["TransCode"].ToString();
                        drowInst[3] = ds.Tables[0].Rows[i]["particulars"].ToString();
                        drowInst[4] = ds.Tables[0].Rows[i]["Debit"].ToString();
                        drowInst[5] = ds.Tables[0].Rows[i]["credit"].ToString();
                        dtCreditDebitReport.Rows.Add(drowInst);
                    }
                    double credit = 0;
                    double debit = 0;
                    for (int i = 2; i < dtCreditDebitReport.Rows.Count; i++)
                    {
                        if (credit == 0 && debit == 0)
                        {
                            credit = Convert.ToDouble(dtCreditDebitReport.Rows[i]["Credit"].ToString());
                            debit = Convert.ToDouble(dtCreditDebitReport.Rows[i]["Debit"].ToString());
                        }
                        else
                        {
                            credit = credit + Convert.ToDouble(dtCreditDebitReport.Rows[i]["Credit"].ToString());
                            debit = debit + Convert.ToDouble(dtCreditDebitReport.Rows[i]["Debit"].ToString());
                        }
                    }
                    drowInst = dtCreditDebitReport.NewRow();
                    drowInst[0] = "Total";
                    drowInst[4] = Convert.ToString(credit);
                    drowInst[5] = Convert.ToString(debit);
                    dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Total");
                    dtCreditDebitReport.Rows.Add(drowInst);

                    drowInst = dtCreditDebitReport.NewRow();
                    drowInst[0] = "Closing Balance";
                    drowInst[5] = Convert.ToString(debitop);
                    dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Balance");
                    dtCreditDebitReport.Rows.Add(drowInst);
                }

                divCellClickDate.Visible = true;
                GrdDateWise.DataSource = dtCreditDebitReport;
                GrdDateWise.DataBind();
                GrdDateWise.Visible = true;

                GrdDateWise.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GrdDateWise.Rows[0].Font.Bold = true;
                GrdDateWise.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Balance")
                    {
                        GrdDateWise.Rows[g].BackColor = ColorTranslator.FromHtml("#4870BE");
                        GrdDateWise.Rows[g].Cells[0].ColumnSpan = 4;
                        GrdDateWise.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        GrdDateWise.Rows[g].Cells[0].Font.Bold = true;
                        for (int a = 1; a < 4; a++)
                        {
                            GrdDateWise.Rows[g].Cells[a].Visible = false;
                        }
                    }
                    if (DicValue == "Total")
                    {
                        GrdDateWise.Rows[g].Cells[0].ColumnSpan = 4;
                        GrdDateWise.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        GrdDateWise.Rows[g].Cells[0].Font.Bold = true;
                        for (int a = 1; a < 4; a++)
                        {
                            GrdDateWise.Rows[g].Cells[a].Visible = false;
                        }
                    }
                }
            }
            else
            {
                GrdDateWise.Visible = false;
            }
            MergeRows(GrdDateWise);
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdCreditReport_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
    }

    protected void GrdDateWise_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
    }

    public static void MergeRows(GridView GrdDateWise)
    {
        string Date = GrdDateWise.HeaderRow.Cells[1].Text;

        for (int rowIndex = GrdDateWise.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = GrdDateWise.Rows[rowIndex];
            GridViewRow previousRow = GrdDateWise.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (GrdDateWise.HeaderRow.Cells[i].Text.ToLower() == Date.ToLower())
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }

    #endregion

    #region Month Wise

    public void dtmonth2()
    {
        try
        {
            UserbasedRights();
            grdCreditReport.Visible = false;
            GrdDateWise.Visible = false;
            GrdHeaderWise.Visible = false;
            GrdLedger.Visible = false;
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
            DateTime fdt = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            // rptprint.Visible  = true;

            arrColHdrNames.Add("S.No");
            dtCreditDebitReport.Columns.Add("S.No");
            arrColHdrNames.Add("Date");
            dtCreditDebitReport.Columns.Add("Date");
            arrColHdrNames.Add("Opening Balance");
            dtCreditDebitReport.Columns.Add("Opening Balance");
            arrColHdrNames.Add("Credit");
            dtCreditDebitReport.Columns.Add("Credit");
            arrColHdrNames.Add("Debit");
            dtCreditDebitReport.Columns.Add("Debit");
            arrColHdrNames.Add("Closing Balance");
            dtCreditDebitReport.Columns.Add("Closing Balance");
            DataRow drHdr1 = dtCreditDebitReport.NewRow();
            for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtCreditDebitReport.Rows.Add(drHdr1);

            string selqry = "";
            string totamt = "";
            string debitvalue = "";
            string creditvalue = "";
            string opningbal = "";
            double closingbal = 0;
            bool dtcheck = false;
            string findt = "";
            string fincyr = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (fincyr != "0")
            {
                findt = d2.GetFunction("select CONVERT(varchar(10),finyearstart,103) as findt from FM_FinYearMaster where FinYearPK='" + fincyr + "'");
                // +'-'+CONVERT(varchar(10),FinYearEnd,103)
                string[] spfindt = findt.Split('/');
                findt = spfindt[1] + "/" + spfindt[0] + "/" + spfindt[2];
                fdt = Convert.ToDateTime(findt);
                while (dt >= fdt)
                {
                    dtcheck = true;
                    break;
                }

            }
            UserbasedRights();
            string userCode = "";
            if (usBasedRights == true)
                userCode = " and EntryUserCode in('" + usercode + "')";
            //if (dtcheck == true)
            //{
            if (rb_month.Checked == true)
            {
                //selqry = "SELECT CONVERT(varchar(10), Month(TransDate))+'-'+ CONVERT(varchar(10), YEAR(TransDate)) MonthYear,SUM(Debit)as Debit,SUM(credit)as Credit,((sum(Debit)-SUM(credit))+ SUM(Debit)-SUM(credit)) ClosingBal FROM FT_FinDailyTransaction WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and  TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " GROUP BY YEAR(TransDate),Month(TransDate)";
                //selqry = selqry + "SELECT SUM(Debit)-SUM(credit) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate <= '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "";
                //ds = d2.select_method_wo_parameter(selqry, "Text");
                //totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate < '" + dt.ToString("MM/dd/yyyy") + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");

                //totamt = Convert.ToString(totamt);

                selqry = "SELECT CONVERT(varchar(10), Month(TransDate))+'-'+ CONVERT(varchar(10), YEAR(TransDate)) MonthYear,SUM(Debit)as Debit,SUM(credit)as Credit,((sum(Debit)-SUM(credit))+ SUM(Debit)-SUM(credit)) ClosingBal FROM FT_FinDailyTransaction f,registration r WHERE  LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and  TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and f.app_no=r.app_no and r.college_code ='" + collegecode1 + "'  and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and  ISNULL(IsCollected,0)='1' GROUP BY YEAR(TransDate),Month(TransDate) order by YEAR(TransDate),Month(TransDate)";// " + userCode + "
                selqry = selqry + " SELECT SUM(Debit)-SUM(credit) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate <= '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate < '" + dt.ToString("MM/dd/yyyy") + "' and LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "')");

                totamt = Convert.ToString(totamt);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        opningbal = "";
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (dt == fdt)
                                opningbal = "0";
                            else
                                opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
                        }
                        else
                        {
                            opningbal = totamt;
                        }
                        if (rb_month.Checked == true)
                        {
                            drowInst = dtCreditDebitReport.NewRow();
                            drowInst[0] = Convert.ToString(i + 1);
                            drowInst[1] = ds.Tables[0].Rows[i]["MonthYear"].ToString();
                            if (opningbal == "")
                            {
                                opningbal = "0";
                            }
                            drowInst[2] = Convert.ToString(opningbal);
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
                            drowInst[3] = Convert.ToString(debitvalue);
                            drowInst[4] = Convert.ToString(creditvalue);
                            closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                            drowInst[5] = Convert.ToString(closingbal);
                            // FpSpread3.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            //opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                            opningbal = Convert.ToString(closingbal);

                        }
                    }
                    else
                    {
                        if (rb_month.Checked == true)
                        {
                            drowInst = dtCreditDebitReport.NewRow();
                            drowInst[0] = Convert.ToString(i + 1);
                            drowInst[1] = ds.Tables[0].Rows[i]["MonthYear"].ToString();
                            if (opningbal == "")
                            {
                                opningbal = "0";
                            }
                            drowInst[2] = Convert.ToString(opningbal);
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
                            drowInst[3] = Convert.ToString(debitvalue);
                            drowInst[4] = Convert.ToString(creditvalue);
                            closingbal = Convert.ToDouble(opningbal) + Convert.ToDouble(debitvalue) - Convert.ToDouble(creditvalue);
                            //FpSpread3.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
                            drowInst[5] = Convert.ToString(closingbal);
                            //  opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                            opningbal = Convert.ToString(closingbal);
                        }
                    }
                    dtCreditDebitReport.Rows.Add(drowInst);
                }

                monthdiv.Visible = true;
                rptprint1.Visible = true;
                GrdMonth.DataSource = dtCreditDebitReport;
                GrdMonth.DataBind();
                GrdMonth.Visible = true;

                GrdMonth.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GrdMonth.Rows[0].Font.Bold = true;
                GrdMonth.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                monthdiv.Visible = false;
                GrdMonth.Visible = false;
                // rptprint.Visible = false;
            }
            rbchange();
            //}
            //else
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alert1.Text = "From date Sholud Be Greater Than Current Financial Year Start Date";
            //    monthdiv.Visible = false;
            //    GrdMonth.Visible = false;
            //}
        }
        catch
        {
        }
    }

    protected void GrdMonth_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldMonth.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        fairpoin();
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.HiddenFieldMonth.Value);

        arrColHdrNames.Add("S.No");
        dtCreditDebitReport.Columns.Add("S.No");
        arrColHdrNames.Add("Date");
        dtCreditDebitReport.Columns.Add("Date");
        arrColHdrNames.Add("Opening Balance");
        dtCreditDebitReport.Columns.Add("Opening Balance");
        arrColHdrNames.Add("Credit");
        dtCreditDebitReport.Columns.Add("Credit");
        arrColHdrNames.Add("Debit");
        dtCreditDebitReport.Columns.Add("Debit");
        arrColHdrNames.Add("Closing Balance");
        dtCreditDebitReport.Columns.Add("Closing Balance");
        DataRow drHdr1 = dtCreditDebitReport.NewRow();
        for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
        {
            drHdr1[grCol] = arrColHdrNames[grCol];
        }
        dtCreditDebitReport.Rows.Add(drHdr1);

        string creditop = GrdMonth.Rows[rowIndex].Cells[2].Text;
        string debitop = GrdMonth.Rows[rowIndex].Cells[5].Text;

        string date = GrdMonth.Rows[rowIndex].Cells[1].Text;
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
        UserbasedRights();
        string userCode = "";
        if (usBasedRights == true)
            userCode = " and EntryUserCode in('" + usercode + "')";

        string selqry = " SELECT SUM(Debit)as Debit,SUM(credit)as Credit,(SUM(Debit)-SUM(credit)) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE  TransDate Between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt4.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " GROUP BY TransDate order by cast(transdate as datetime)";
        selqry = selqry + " SELECT SUM(Debit)-SUM(credit) OpeningBal FROM FT_FinDailyTransaction D WHERE TransDate < '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "  ";
        ds = d2.select_method_wo_parameter(selqry, "Text");

        string totamt = d2.GetFunction(" select SUM(TotalAmount) from FT_FeeAllot WHERE PayStartDate < '" + dt3.ToString("MM/dd/yyyy") + "'");
        totamt = Convert.ToString(totamt);

        if (ds.Tables[0].Rows.Count > 0)
        {
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
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        if (opningbal == "")
                        {
                            opningbal = "0";
                        }
                        drowInst[2] = Convert.ToString(opningbal);
                        drowInst[3] = ds.Tables[0].Rows[i]["Debit"].ToString();
                        drowInst[4] = ds.Tables[0].Rows[i]["Credit"].ToString();
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
                        drowInst[5] = Convert.ToString(closingbal);
                        // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        opningbal = Convert.ToString(closingbal);
                    }
                }
                else
                {
                    if (rb_month.Checked == true)
                    {
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        drowInst[2] = Convert.ToString(opningbal);
                        drowInst[3] = ds.Tables[0].Rows[i]["Debit"].ToString();
                        drowInst[4] = ds.Tables[0].Rows[i]["credit"].ToString();
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
                        drowInst[5] = Convert.ToString(closingbal);
                        // opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
                        opningbal = Convert.ToString(closingbal);
                    }
                }
                dtCreditDebitReport.Rows.Add(drowInst);
            }
            Div2.Visible = true;
            GrdMonthCellClick.DataSource = dtCreditDebitReport;
            GrdMonthCellClick.DataBind();
            GrdMonthCellClick.Visible = true;

            GrdMonthCellClick.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GrdMonthCellClick.Rows[0].Font.Bold = true;
            GrdMonthCellClick.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            rptprint1.Visible = true;
        }
        else
        {
            Div2.Visible = true;
            lbl_alert1.Text = "No Records Found";
            //grdCreditReport.Visible = false;
        }

    }

    protected void GrdMonth_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != 0)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void GrdMonthCellClick_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != 0)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void GrdMonthCellClick_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldMonth2.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdMonthCellClick_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.HiddenFieldMonth2.Value);

            DateTime dt = new DateTime();

            arrColHdrNames.Add("S.No");
            dtCreditDebitReport.Columns.Add("S.No");
            arrColHdrNames.Add("Date");
            dtCreditDebitReport.Columns.Add("Date");
            arrColHdrNames.Add("Recipt NO");
            dtCreditDebitReport.Columns.Add("Recipt NO");
            arrColHdrNames.Add("Particulars");
            dtCreditDebitReport.Columns.Add("Particulars");
            arrColHdrNames.Add("Credit");
            dtCreditDebitReport.Columns.Add("Credit");
            arrColHdrNames.Add("Debit");
            dtCreditDebitReport.Columns.Add("Debit");
            DataRow drHdr1 = dtCreditDebitReport.NewRow();
            for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtCreditDebitReport.Rows.Add(drHdr1);

            string creditop = GrdMonthCellClick.Rows[rowIndex].Cells[2].Text;
            string debitop = GrdMonthCellClick.Rows[rowIndex].Cells[5].Text;

            UserbasedRights();
            string userCode = "";
            if (usBasedRights == true)
                userCode = " and EntryUserCode in('" + usercode + "')";

            string dte = "";
            string dtm = "";
            string date = GrdMonthCellClick.Rows[rowIndex].Cells[1].Text;
            DateTime dt3 = new DateTime();
            string[] split1 = date.Split('/');
            dt3 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,isnull(Debit,'0') as Debit,isnull(credit,'0') as credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and d.LedgerFK =l.LedgerPK and  TransDate between '" + dt3.ToString("MM/dd/yyyy") + "' AND '" + dt3.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        drowInst = dtCreditDebitReport.NewRow();
                        drowInst[0] = Convert.ToString(i + 1);
                        drowInst[1] = ds.Tables[0].Rows[i]["TransDate"].ToString();
                        drowInst[2] = ds.Tables[0].Rows[i]["TransCode"].ToString();
                        drowInst[3] = ds.Tables[0].Rows[i]["particulars"].ToString();
                        drowInst[4] = ds.Tables[0].Rows[i]["Debit"].ToString();
                        drowInst[5] = ds.Tables[0].Rows[i]["credit"].ToString();
                        dtCreditDebitReport.Rows.Add(drowInst);
                    }
                    double credit = 0;
                    double debit = 0;
                    for (int i = 1; i < dtCreditDebitReport.Rows.Count; i++)
                    {
                        if (credit == 0 && debit == 0)
                        {
                            credit = Convert.ToDouble(dtCreditDebitReport.Rows[i]["Credit"].ToString());
                            debit = Convert.ToDouble(dtCreditDebitReport.Rows[i]["Debit"].ToString());
                        }
                        else
                        {
                            credit = credit + Convert.ToDouble(dtCreditDebitReport.Rows[i]["Credit"].ToString());
                            debit = debit + Convert.ToDouble(dtCreditDebitReport.Rows[i]["Debit"].ToString());
                        }
                    }
                    drowInst = dtCreditDebitReport.NewRow();
                    drowInst[0] = "Total";
                    drowInst[4] = Convert.ToString(credit);
                    drowInst[5] = Convert.ToString(debit);
                    dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Total");
                    dtCreditDebitReport.Rows.Add(drowInst);

                    drowInst = dtCreditDebitReport.NewRow();
                    drowInst[0] = "Closing Balance";
                    drowInst[5] = Convert.ToString(debitop);
                    dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Balance");
                    dtCreditDebitReport.Rows.Add(drowInst);
                }
            }
            divMonth3.Visible = true;
            GrdMonth3CellClick.DataSource = dtCreditDebitReport;
            GrdMonth3CellClick.DataBind();
            GrdMonth3CellClick.Visible = true;

            GrdMonth3CellClick.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GrdMonth3CellClick.Rows[0].Font.Bold = true;
            GrdMonth3CellClick.Rows[0].HorizontalAlign = HorizontalAlign.Center;

            foreach (KeyValuePair<int, string> dr in dicRowColor)
            {
                int g = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "Balance")
                {
                    GrdMonth3CellClick.Rows[g].BackColor = ColorTranslator.FromHtml("#4870BE");
                    GrdMonth3CellClick.Rows[g].Cells[0].ColumnSpan = 4;
                    GrdMonth3CellClick.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    GrdMonth3CellClick.Rows[g].Cells[0].Font.Bold = true;
                    for (int a = 1; a < 4; a++)
                    {
                        GrdMonth3CellClick.Rows[g].Cells[a].Visible = false;
                    }
                }
                if (DicValue == "Total")
                {
                    GrdMonth3CellClick.Rows[g].Cells[0].ColumnSpan = 4;
                    GrdMonth3CellClick.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    GrdMonth3CellClick.Rows[g].Cells[0].Font.Bold = true;
                    for (int a = 1; a < 4; a++)
                    {
                        GrdMonth3CellClick.Rows[g].Cells[a].Visible = false;
                    }
                }
            }
            MergeRows(GrdMonth3CellClick);
        }
        catch { }
    }

    protected void GrdMonth3CellClick_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != 0)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    #endregion

    public void FpSpreadLedger_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick2 = true;
        }
        catch
        { }

    }

    public void FpSpreadLedger_Selectedindexchange(object sender, EventArgs e)
    {
        if (cellclick2 == true)
        {
            if (rb_header.Checked == true)
            {
                dtLedgerWise();
                // popwindow1.Visible = true;
                GrdLedger.Visible = true;
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
            Div3.Visible = false;
            Div4.Visible = false;
            dtdate2();
        }
        if (rb_month.Checked == true)
        {
            monthdiv.Visible = true;
            div1.Visible = false;
            Div3.Visible = false;
            Div4.Visible = false;
            dtmonth2();
        }
        if (rb_header.Checked == true)//Added by saranya on 23/12/2017
        {
            monthdiv.Visible = false;
            div1.Visible = false;
            Div3.Visible = true;
            Div4.Visible = false;
            dtHeaderWise();
        }
        if (rb_ledger.Checked == true)//Added by saranya on 10/01/2017
        {
            monthdiv.Visible = false;
            div1.Visible = false;
            Div3.Visible = false;
            Div4.Visible = true;
            dtLedgerWise();
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

    protected void rb_date_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        Div2.Visible = false;
        Div3.Visible = false;
        Div4.Visible = false;
        GrdMonth3CellClick.Visible = false;
        GrdLedger.Visible = false;
        monthdiv.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }

    protected void rb_month_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        div1.Visible = false;
        Div3.Visible = false;
        Div4.Visible = false;
        grdCreditReport.Visible = false;
        GrdDateWise.Visible = false;
        GrdLedger.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }

    #region Added by saranya on23/12/2017
    protected void rb_header_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        div1.Visible = false;
        Div2.Visible = false;
        Div4.Visible = false;
        grdCreditReport.Visible = false;
        GrdDateWise.Visible = false;
        GrdLedger.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }
    //Added on 10/01/2018
    protected void rb_ledger_CheckedChanged(object sender, EventArgs e)
    {
        rbchange();
        div1.Visible = false;
        Div2.Visible = false;
        Div3.Visible = false;
        grdCreditReport.Visible = false;
        GrdDateWise.Visible = false;
        rptprint1.Visible = false;
        lbl_norec1.Text = "";
        txtexcelname1.Text = "";
    }
    #endregion

    //rb_header_CheckedChanged
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
            //  string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
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
            // string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
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
        //fairpoin();
        //string itemheadercode = "";
        //for (int i = 0; i < cbl_header1.Items.Count; i++)
        //{
        //    if (cbl_header1.Items[i].Selected == true)
        //    {
        //        if (itemheadercode == "")
        //        {
        //            itemheadercode = "" + cbl_header1.Items[i].Value.ToString() + "";
        //        }
        //        else
        //        {
        //            itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header1.Items[i].Value.ToString() + "";
        //        }
        //    }
        //}

        //string Ledgercode = "";
        //for (int i = 0; i < cbl_header1.Items.Count; i++)
        //{
        //    if (cbl_header1.Items[i].Selected == true)
        //    {
        //        if (Ledgercode == "")
        //        {
        //            Ledgercode = "" + cbl_header1.Items[i].Value.ToString() + "";
        //        }
        //        else
        //        {
        //            Ledgercode = Ledgercode + "'" + "," + "'" + cbl_header1.Items[i].Value.ToString() + "";
        //        }
        //    }
        //}

        //string firstdate = Convert.ToString(txt_fromdate1.Text);
        //string seconddate = Convert.ToString(txt_todate1.Text);
        //DateTime dt = new DateTime();
        //DateTime dt1 = new DateTime();
        //string[] split = firstdate.Split('/');
        //dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        //split = seconddate.Split('/');
        //dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);



        //FpSpread4.Sheets[0].RowCount = 0;
        //FpSpread4.Sheets[0].ColumnCount = 0;
        //FpSpread4.CommandBar.Visible = false;
        //FpSpread4.Sheets[0].AutoPostBack = true;
        //FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
        //FpSpread4.Sheets[0].RowHeader.Visible = false;
        //FpSpread4.Sheets[0].ColumnCount = 7;
        //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //darkstyle.ForeColor = Color.White;
        //FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //GrdMonthCellClick.Visible = true;
        //// Dateview.Visible = true;
        //maindiv.Visible = true;

        //string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
        //string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
        //string creditop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
        //string debitop = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text;



        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Opening Balance";
        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Debit";
        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        //FpSpread4.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Closing Balance";


        //for (int i = 0; i < FpSpread4.Sheets[0].Columns.Count; i++)
        //{
        //    FpSpread4.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
        //    FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
        //    FpSpread4.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
        //    FpSpread4.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

        //}

        //FpSpread4.Sheets[0].ColumnHeader.Columns[0].Width = 51;
        //FpSpread4.Sheets[0].ColumnHeader.Columns[1].Width = 140;
        //FpSpread4.Sheets[0].ColumnHeader.Columns[3].Width = 150;
        //FpSpread4.Sheets[0].ColumnHeader.Columns[4].Width = 175;
        //FpSpread4.Sheets[0].ColumnHeader.Columns[5].Width = 148;
        //FpSpread4.Sheets[0].ColumnHeader.Columns[6].Width = 119;
        //UserbasedRights();
        //string userCode = "";
        //if (usBasedRights == true)
        //    userCode = " and EntryUserCode in('" + usercode + "')";


        //string selqry = " SELECT SUM(Debit)as Debit,SUM(credit)as Credit,SUM(Debit)-SUM(credit) ClosingBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction WHERE LedgerFK in('" + Ledgercode + "') and HeaderFK in('" + itemheadercode + "') and TransDate Between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " GROUP BY TransDate";
        //selqry = selqry + " SELECT SUM(Debit)-SUM(credit) OpeningBal,CONVERT(varchar(10), TransDate,103) as TransDate FROM FT_FinDailyTransaction D WHERE TransDate < '" + dt.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + " GROUP BY TransDate ";
        //ds = d2.select_method_wo_parameter(selqry, "Text");




        //if (ds.Tables.Count > 0)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        string opningbal = "";
        //        FpSpread4.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {

        //            if (i == 0)
        //            {
        //                opningbal = "";
        //                if (ds.Tables[1].Rows.Count > 0)
        //                {
        //                    opningbal = Convert.ToString(ds.Tables[1].Rows[0]["OpeningBal"]);
        //                }
        //                else
        //                {
        //                    opningbal = "0";
        //                }
        //                if (rb_month.Checked == true)
        //                {
        //                    FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
        //                    FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 2].Text = Convert.ToString(opningbal);
        //                    FpSpread4.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
        //                    opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
        //                }

        //            }
        //            else
        //            {
        //                if (rb_month.Checked == true)
        //                {

        //                    FpSpread4.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
        //                    FpSpread4.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 3].Text = Convert.ToString(opningbal);
        //                    FpSpread4.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
        //                    FpSpread4.Sheets[0].Cells[i, 6].Text = ds.Tables[0].Rows[i]["ClosingBal"].ToString();
        //                    opningbal = Convert.ToString(ds.Tables[0].Rows[i]["ClosingBal"]);
        //                }


        //            }
        //        }
        //        for (int i = 0; i < FpSpread4.Sheets[0].Columns.Count; i++)
        //        {
        //            FpSpread4.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread4.Sheets[0].Columns[6].Visible = false;
        //        FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].Rows.Count;
        //        GrdMonthCellClick.Visible = true;
        //        FpSpread4.SaveChanges();

        //    }
        //    else
        //    {
        //        imgdiv2.Visible = true;
        //        lbl_alert1.Text = "No Records Found";
        //        GrdMonthCellClick.Visible = false;

        //    }
        //}

        //else
        //{

        //    lbl_alert1.Text = "No Records Found";
        //    GrdMonthCellClick.Visible = false;

        //}


        ////rbchange();

        //;



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
            //  string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";

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
            //  string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
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
        //GrdMonth3CellClick.Visible = false;
        //GrdDateWise.Visible = false;
        //// GrdMonth3CellClick.Visible = true;
        //string itemheadercode = "";
        //for (int i = 0; i < cbl_header2.Items.Count; i++)
        //{
        //    if (cbl_header2.Items[i].Selected == true)
        //    {
        //        if (itemheadercode == "")
        //        {
        //            itemheadercode = "" + cbl_header2.Items[i].Value.ToString() + "";
        //        }
        //        else
        //        {
        //            itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header2.Items[i].Value.ToString() + "";
        //        }
        //    }
        //}

        //string Ledgercode = "";
        //for (int i = 0; i < cbl_header2.Items.Count; i++)
        //{
        //    if (cbl_header2.Items[i].Selected == true)
        //    {
        //        if (Ledgercode == "")
        //        {
        //            Ledgercode = "" + cbl_header2.Items[i].Value.ToString() + "";
        //        }
        //        else
        //        {
        //            Ledgercode = Ledgercode + "'" + "," + "'" + cbl_header2.Items[i].Value.ToString() + "";
        //        }
        //    }
        //}

        //string firstdate = Convert.ToString(txt_fromdate2.Text);
        //string seconddate = Convert.ToString(txt_todate2.Text);
        //DateTime dt = new DateTime();
        //DateTime dt1 = new DateTime();
        //string[] split = firstdate.Split('/');
        //dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        //split = seconddate.Split('/');
        //dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

        //FpSpread5.Sheets[0].RowCount = 0;

        //FpSpread5.Sheets[0].ColumnCount = 0;
        //FpSpread5.CommandBar.Visible = false;
        //FpSpread5.Sheets[0].AutoPostBack = true;
        //FpSpread5.Sheets[0].ColumnHeader.RowCount = 1;
        //FpSpread5.Sheets[0].RowHeader.Visible = false;
        //FpSpread5.Sheets[0].ColumnCount = 7;
        //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //darkstyle.ForeColor = Color.White;
        //FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        //GrdMonth3CellClick.Visible = true;
        ////  popwindow1.Visible = true;
        //maindiv2.Visible = true;
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Recipt NO";
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Particulars";
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Credit";
        //FpSpread5.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Debit";
        //FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        //for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
        //{
        //    FpSpread5.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
        //    FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
        //    FpSpread5.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
        //    FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
        //}
        //FpSpread5.Sheets[0].ColumnHeader.Columns[0].Width = 50;
        //FpSpread5.Sheets[0].ColumnHeader.Columns[1].Width = 128;
        //FpSpread5.Sheets[0].ColumnHeader.Columns[2].Width = 128;
        //FpSpread5.Sheets[0].ColumnHeader.Columns[3].Width = 220;
        //FpSpread5.Sheets[0].ColumnHeader.Columns[4].Width = 145;
        //FpSpread5.Sheets[0].ColumnHeader.Columns[5].Width = 145;
        //UserbasedRights();
        //string userCode = "";
        //if (usBasedRights == true)
        //    userCode = " and EntryUserCode in('" + usercode + "')";


        //string selqry = " SELECT convert(varchar(10),TransDate,103) as TransDate,TransCode,HeaderName+'-'+LedgerName as particulars ,Debit,credit FROM FT_FinDailyTransaction D,FM_HeaderMaster M,FM_LedgerMaster L WHERE D.Headerfk = m.HeaderPK and m.HeaderPK =  l.headerfk and LedgerPK in('" + Ledgercode + "')and HeaderPK in('" + itemheadercode + "')  and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + userCode + "  order by isnull(l.priority,1000), l.ledgerName asc ";

        //ds = d2.select_method_wo_parameter(selqry, "Text");
        //if (ds.Tables.Count > 0)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        FpSpread5.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            FpSpread5.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
        //            FpSpread5.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();
        //            FpSpread5.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["TransCode"].ToString();
        //            FpSpread5.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["particulars"].ToString();
        //            FpSpread5.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
        //            FpSpread5.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["credit"].ToString();
        //            // lbl_alert1.Visible = false;
        //        }
        //        int credit = 0;
        //        int debit = 0;
        //        for (int i = 0; i < FpSpread5.Sheets[0].Rows.Count; i++)
        //        {
        //            if (credit == 0 && debit == 0)
        //            {
        //                credit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
        //                debit = Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
        //            }
        //            else
        //            {
        //                credit = credit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
        //                debit = debit + Convert.ToInt32(FpSpread5.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        lbl_alert1.Text = "No Records Found";
        //        GrdMonth3CellClick.Visible = false;
        //    }
        //    for (int i = 0; i < FpSpread5.Sheets[0].Columns.Count; i++)
        //    {
        //        FpSpread5.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
        //    }
        //    FpSpread5.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        //    FpSpread5.Sheets[0].Columns[6].Visible = false;
        //    FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].Rows.Count;
        //    FpSpread5.ShowHeaderSelection = false;
        //    GrdMonth3CellClick.Visible = true;
        //    FpSpread5.SaveChanges();
        //}
        //else
        //{
        //    lbl_alert1.Text = "No Records Found";
        //    GrdMonth3CellClick.Visible = false;
        //}
        ////rbchange();
    }

    //protected void btnExcel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string reportname = txtexcelname.Text;
    //        if (reportname.ToString().Trim() != "")
    //        {
    //            if (GrdMonth3CellClick.Visible == true)
    //            {
    //                d2.printexcelreport(FpSpread5, reportname);
    //            }
    //            else if (GrdMonthCellClick.Visible == true)
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
    //        if (GrdDateWise.Visible == true)
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

    #region Print And Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (rb_date.Checked == true)
            {
                if (reportname.Trim() != "")
                {
                    if (grdCreditReport.Visible == true)
                    {
                        d2.printexcelreportgrid(grdCreditReport, reportname);
                    }
                    if (GrdDateWise.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdDateWise, reportname);
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
                    if (GrdMonth.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdMonth, reportname);
                    }
                    if (GrdMonthCellClick.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdMonthCellClick, reportname);
                    }
                    if (GrdMonth3CellClick.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdMonth3CellClick, reportname);
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
            else if (rb_header.Checked == true)//added by abarna 3.05.2018
            {
                if (reportname.Trim() != "")
                {
                    if (GrdHeaderWise.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdHeaderWise, reportname);
                    }
                    lbl_norec1.Visible = false;
                }
                else
                {
                    lbl_norec1.Text = "Please Enter Your Headerwise Report Name";
                    lbl_norec1.Visible = true;
                    txtexcelname1.Focus();
                }
            }
            else if (rb_ledger.Checked == true)//added by abarna 3.05.2018
            {
                if (reportname.Trim() != "")
                {
                    if (GrdLedger.Visible == true)
                    {
                        d2.printexcelreportgrid(GrdLedger, reportname);
                    }
                    lbl_norec1.Visible = false;
                }
                else
                {
                    lbl_norec1.Text = "Please Enter Your LedgerWise Report Name";
                    lbl_norec1.Visible = true;
                    txtexcelname1.Focus();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "CriditdebitReport";
            string pagename = "CreditdebitReport.aspx";
            string ss = Session["usercode"].ToString();
            if (rb_date.Checked == true)
            {
                if (grdCreditReport.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(grdCreditReport, pagename, dptname, 0, ss);
                }
                if (GrdDateWise.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdDateWise, pagename, dptname, 0, ss);
                }
                Printcontrol1.Visible = true;
                lbl_norec1.Visible = false;
            }
            else if (rb_month.Checked == true)
            {
                if (GrdMonth.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdMonth, pagename, dptname, 0, ss);
                }
                if (GrdMonthCellClick.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdMonthCellClick, pagename, dptname, 0, ss);
                }
                if (GrdMonth3CellClick.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdMonth3CellClick, pagename, dptname, 0, ss);
                }
                Printcontrol1.Visible = true;
                lbl_norec1.Visible = false;
            }
            else if (rb_header.Checked == true)//added by abarna 3.05.2018
            {
                if (GrdHeaderWise.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdHeaderWise, pagename, dptname, 0, ss);
                }
                Printcontrol1.Visible = true;
                lbl_norec1.Visible = false;
            }
            else if (rb_ledger.Checked == true)//added by abarna 3.05.2018
            {
                if (GrdLedger.Visible == true)
                {
                    Printcontrol1.loadspreaddetails(GrdLedger, pagename, dptname, 0, ss);
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

            //Printcontrol1.loadspreaddetails(FpSpread4, pagename, dptname);
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
                d2.printexcelreportgrid(GrdMonth3CellClick, reportname);
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

    #endregion

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

    public void loadcollege()
    {
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        ddl_collegename.Items.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_collegename.DataSource = ds;
            ddl_collegename.DataTextField = "collname";
            ddl_collegename.DataValueField = "college_code";
            ddl_collegename.DataBind();
        }
        //reuse.bindCollegeToDropDown(usercode, ddl_collegename);

    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
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

        lbl.Add(lblclg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar

    #region Header and Ledger Wise added by saranya on 23/12/2017

    public void dtHeaderWise()
    {
        try
        {
            Dictionary<string, string> dicmonthval = new Dictionary<string, string>();
            UserbasedRights();
            grdCreditReport.Visible = false;
            GrdDateWise.Visible = false;
            GrdMonth.Visible = false;
            GrdMonthCellClick.Visible = false;
            GrdMonth3CellClick.Visible = false;
            GrdLedger.Visible = false;
            Hashtable HdWiseTotal = new Hashtable();
            Hashtable GrandTotal = new Hashtable();
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
            //string userCode = "";
            //if (usBasedRights == true)
            string payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            string userCode = " and EntryUserCode in('" + usercode + "')";
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            string selqry = "select  CONVERT(varchar(10), TransDate,103) as TransDate,headerfk,sum(ISNULL (debit,'0')) as debit,f.paymode,DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year from ft_findailytransaction f,registration r where TransDate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + itemheadercode + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.App_No =f.App_No and r.college_code ='" + collegecode1 + "' and f.paymode in('" + payMode + "')  and Debit<>0  GROUP BY headerfk ,transdate,paymode order by cast(transdate as datetime )";//" + userCode + "//" + userCode + "             //and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0
            selqry = selqry + " select  distinct DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year from ft_findailytransaction f,registration r where TransDate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + itemheadercode + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.App_No =f.App_No and r.college_code ='" + collegecode1 + "' and f.paymode in('" + payMode + "')  and Debit<>0  group by  DATEPART(month,TransDate),DATEPART(year,TransDate) order by DATEPART(year,TransDate), DATEPART(month,TransDate)";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            DataTable dtDate = new DataTable();
            dtDate = ds.Tables[0];
            DataTable dtDateVal = new DataTable();
            dtDateVal = dtDate.DefaultView.ToTable(true, "TransDate");

            #region Grid columnheader bind

            double TotalDebit = 0;
            double total = 0;
            string value = string.Empty;
            double debitAmt = 0;
            double FinalTot = 0;
            double grandtotal = 0;
            bool isPaymodeVal = false;
            string PaymodeHeader = string.Empty;
            int b;
            string hid = string.Empty;
            int ColCountVisible = 1;

            arrColHdrNames.Add("S.No");
            dtCreditDebitReport.Columns.Add("S.No");
            arrColHdrNames.Add("Date");
            dtCreditDebitReport.Columns.Add("Date");
            for (b = 0; b < cbl_header.Items.Count; b++)
            {
                if (cbl_header.Items[b].Selected == true)
                {
                    string headtest = cbl_header.Items[b].Text.ToString();
                    hid = cbl_header.Items[b].Value.ToString();
                    ds.Tables[0].DefaultView.RowFilter = "HeaderFK='" + hid + "'";
                    arrColHdrNames.Add(headtest);
                    dtCreditDebitReport.Columns.Add(headtest);
                    ColCountVisible++;
                    arrColHdrNames.Add(hid);
                    dtCreditDebitReport.Columns.Add(hid);
                    ColCountVisible++;
                    dicColumnVisible.Add(ColCountVisible, "Visible");
                }
            }
            arrColHdrNames.Add("Total");
            dtCreditDebitReport.Columns.Add("Total");

            DataRow drHdr1 = dtCreditDebitReport.NewRow();
            for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtCreditDebitReport.Rows.Add(drHdr1);

            #endregion

            #region value

            int Sno = 1;
            string MonthVal = string.Empty;
            string Month = string.Empty;
            string hid1 = string.Empty;
            string hidTot = string.Empty;
            string Year = string.Empty;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                {
                    if (chkl_paid.Items[mode].Selected)
                    {
                        string payMode1 = Convert.ToString(chkl_paid.Items[mode].Value);
                        string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                        for (int monthloop = 0; monthloop < ds.Tables[1].Rows.Count; monthloop++)
                        {
                            Month = Convert.ToString(ds.Tables[1].Rows[monthloop]["month"]);
                            Year = Convert.ToString(ds.Tables[1].Rows[monthloop]["year"]);
                            if (dtDateVal.Rows.Count > 0)
                            {
                                for (int row = 0; row < dtDateVal.Rows.Count; row++)
                                {
                                    string transdate = Convert.ToString(dtDateVal.Rows[row]["TransDate"]);
                                    ds.Tables[0].DefaultView.RowFilter = "TransDate='" + transdate + "' and paymode='" + payMode1 + "' and month='" + Month + "' and year='" + Year + "'";
                                    DataTable dthead = ds.Tables[0].DefaultView.ToTable();
                                    if (dthead.Rows.Count > 0)
                                    {
                                        if (hsPaymodeHeader.Contains(payMode1))
                                        {
                                            hsPaymodeHeader.Remove(payMode1);
                                            hsPaymodeHeader.Add(payMode1, payModeText);
                                        }
                                        else
                                        {
                                            hsPaymodeHeader.Add(payMode1, payModeText);
                                            drowInst = dtCreditDebitReport.NewRow();
                                            drowInst[0] = Convert.ToString(payModeText);
                                            dtCreditDebitReport.Rows.Add(drowInst);
                                            dicRowColor.Add(dtCreditDebitReport.Rows.Count - 1, "Paymode");
                                        }
                                        isPaymodeVal = true;
                                        drowInst = dtCreditDebitReport.NewRow();
                                        drowInst[0] = Convert.ToString(Sno);
                                        Sno++;
                                        drowInst[1] = Convert.ToString(dtDateVal.Rows[row]["TransDate"]);
                                        string[] Date = transdate.Split('/');
                                        if (Date.Length == 3)
                                            transdate = Date[1].ToString() + "/" + Date[0].ToString() + "/" + Date[2].ToString();
                                        Month = Date[1];
                                        TotalDebit = 0;
                                        if (dthead.Rows.Count > 0)
                                        {
                                            for (int col = 0; col < dthead.Rows.Count; col++)
                                            {
                                                hid1 = Convert.ToString(dthead.Rows[col]["headerfk"]);
                                                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                                                {
                                                    string header = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                                                    if (hid1 == header)
                                                        drowInst[i - 1] = Convert.ToString(dthead.Rows[col]["Debit"].ToString());
                                                }
                                                debitAmt = Convert.ToInt32(dthead.Rows[col]["Debit"]);
                                                TotalDebit = TotalDebit + debitAmt;
                                                double.TryParse(Convert.ToString(dthead.Rows[col]["Debit"]), out debitAmt);
                                                total += debitAmt;
                                                if (HdWiseTotal.Contains(hid1))
                                                {
                                                    value = "";
                                                    value = HdWiseTotal[hid1].ToString();
                                                    HdWiseTotal.Remove(hid1);
                                                    total = 0;
                                                    total = Convert.ToInt32(value) + Convert.ToInt32(debitAmt);
                                                    HdWiseTotal.Add(hid1, total);
                                                }
                                                else
                                                {
                                                    HdWiseTotal.Add(hid1, Convert.ToInt32(debitAmt));
                                                }

                                                #region for grand total

                                                if (GrandTotal.Contains(hid1))
                                                {
                                                    value = "";
                                                    value = GrandTotal[hid1].ToString();
                                                    GrandTotal.Remove(hid1);
                                                    total = 0;
                                                    total = Convert.ToInt32(value) + Convert.ToInt32(debitAmt);
                                                    GrandTotal.Add(hid1, total);
                                                }
                                                else
                                                {
                                                    GrandTotal.Add(hid1, Convert.ToInt32(debitAmt));
                                                }

                                                #endregion

                                                drowInst[dtCreditDebitReport.Columns.Count - 1] = TotalDebit;
                                                FinalTot = FinalTot + debitAmt;
                                            }
                                            dtCreditDebitReport.Rows.Add(drowInst);
                                        }
                                    }
                                }
                            }
                            if (isPaymodeVal == true)
                            {
                                drowInst = dtCreditDebitReport.NewRow();
                                drowInst[0] = "Total";
                                dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Total");
                                int TotCol = 2;
                                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                                {
                                    string header = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                                    if (HdWiseTotal.Contains(header))
                                        drowInst[i - 1] = Convert.ToString(HdWiseTotal[header]);
                                    TotCol++;
                                }
                                drowInst[TotCol - 1] = Convert.ToString(FinalTot);
                                dtCreditDebitReport.Rows.Add(drowInst);
                                HdWiseTotal.Clear();
                                FinalTot = 0;
                                isPaymodeVal = false;
                            }
                        }
                    }
                }

                #region printing the grandtotal

                drowInst = dtCreditDebitReport.NewRow();
                drowInst[0] = "Grand Total";
                dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Grand Total");
                int GrandTotCol = 2;
                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                {
                    string header = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                    if (GrandTotal.Contains(header))
                        drowInst[i - 1] = Convert.ToString(GrandTotal[header]);
                    if (GrandTotal.Contains(header))
                    {
                        value = "";
                        value = GrandTotal[header].ToString();
                        GrandTotal.Remove(header);
                        total = 0;
                        total = Convert.ToInt32(value);
                        grandtotal += total;
                    }
                    GrandTotCol++;
                }
                drowInst[GrandTotCol - 1] = Convert.ToString(grandtotal);
                dtCreditDebitReport.Rows.Add(drowInst);

                #endregion

                GrdHeaderWise.DataSource = dtCreditDebitReport;
                GrdHeaderWise.DataBind();
                GrdHeaderWise.Visible = true;

                GrdHeaderWise.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GrdHeaderWise.Rows[0].Font.Bold = true;
                GrdHeaderWise.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Paymode")
                    {
                        GrdHeaderWise.Rows[g].Cells[0].ColumnSpan = dtCreditDebitReport.Columns.Count;
                        for (int a = 1; a < dtCreditDebitReport.Columns.Count; a++)
                            GrdHeaderWise.Rows[g].Cells[a].Visible = false;
                        GrdHeaderWise.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdHeaderWise.Rows[g].Cells[0].Font.Bold = true;
                        GrdHeaderWise.Rows[g].BackColor = ColorTranslator.FromHtml("#F08080");
                    }
                    if (DicValue == "Total")
                    {
                        GrdHeaderWise.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            GrdHeaderWise.Rows[g].Cells[a].Visible = false;
                        GrdHeaderWise.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdHeaderWise.Rows[g].Font.Bold = true;
                        GrdHeaderWise.Rows[g].BackColor = ColorTranslator.FromHtml("skyblue");
                    }
                    if (DicValue == "Grand Total")
                    {
                        GrdHeaderWise.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            GrdHeaderWise.Rows[g].Cells[a].Visible = false;
                        GrdHeaderWise.Rows[g].Font.Bold = true;
                        GrdHeaderWise.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdHeaderWise.Rows[g].BackColor = Color.Green;
                    }
                }
                rptprint1.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                GrdHeaderWise.Visible = false;
                //div1.Visible = false;
                rptprint1.Visible = false;
            }
            #endregion
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "CreditdebitReport.aspx");
        }
    }

    protected void GrdHeaderWise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        int totalcol = 0;
        foreach (KeyValuePair<int, string> dr in dicColumnVisible)
        {
            int g = dr.Key;
            string DicValue = dr.Value;
            if (DicValue == "Visible")
            {
                e.Row.Cells[g].Visible = false;
                if (e.Row.RowIndex != 0)
                {
                    e.Row.Cells[g - 1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            totalcol = g;
        }
        e.Row.Cells[totalcol + 1].HorizontalAlign = HorizontalAlign.Right;
    }

    public void dtLedgerWise()
    {
        try
        {
            bool isPaymodeVal = false;
            Dictionary<string, string> dicmonthval = new Dictionary<string, string>();
            UserbasedRights();
            grdCreditReport.Visible = false;
            GrdDateWise.Visible = false;
            GrdMonth.Visible = false;
            GrdMonthCellClick.Visible = false;
            GrdMonth3CellClick.Visible = false;
            GrdHeaderWise.Visible = false;
            Hashtable LedgerWiseTotal = new Hashtable();
            Hashtable GrandTotal = new Hashtable();
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
            //string userCode = "";
            //if (usBasedRights == true)
            string payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            string userCode = " and EntryUserCode in('" + usercode + "')";
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            rptprint1.Visible = true;

            string selqry = "select  CONVERT(varchar(10), TransDate,103) as TransDate,LedgerFK,sum(ISNULL (debit,'0')) as debit,f.paymode,DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year from ft_findailytransaction f,registration r  where TransDate between '" + fromdate + "' and '" + todate + "' and LedgerFK in('" + Ledgercode + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and  r.App_No =f.App_No and r.college_code ='" + collegecode1 + "' and f.paymode in('" + payMode + "')  and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0  and Debit<>0  GROUP BY LedgerFK ,transdate,paymode order by cast(transdate as datetime)";//" + userCode + "

            selqry = selqry + " select  distinct DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year from ft_findailytransaction f,registration r where TransDate between '" + fromdate + "' and '" + todate + "' and LedgerFK in('" + Ledgercode + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.App_No =f.App_No and r.college_code ='" + collegecode1 + "' and f.paymode in('" + payMode + "')  and Debit<>0  group by  DATEPART(month,TransDate),DATEPART(year,TransDate) order by DATEPART(year,TransDate), DATEPART(month,TransDate)";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            DataTable dtDate = new DataTable();
            dtDate = ds.Tables[0];
            DataTable dtDateVal = new DataTable();
            dtDateVal = dtDate.DefaultView.ToTable(true, "TransDate");

            double TotalDebit = 0;
            double total = 0;
            string value = string.Empty;
            double debitAmt = 0;
            double FinalTot = 0;
            double grandtotal = 0;
            int b;
            string Ledgerid = string.Empty;

            #region Grid columnheader bind

            int ColCountVisible = 1;
            arrColHdrNames.Add("S.No");
            dtCreditDebitReport.Columns.Add("S.No");
            arrColHdrNames.Add("Date");
            dtCreditDebitReport.Columns.Add("Date");
            for (b = 0; b < cbl_ledger.Items.Count; b++)
            {
                if (cbl_ledger.Items[b].Selected == true)
                {
                    string headtest = cbl_ledger.Items[b].Text.ToString();
                    Ledgerid = cbl_ledger.Items[b].Value.ToString();
                    ds.Tables[0].DefaultView.RowFilter = "LedgerFK='" + Ledgerid + "'";
                    arrColHdrNames.Add(headtest);
                    dtCreditDebitReport.Columns.Add(headtest);
                    ColCountVisible++;
                    arrColHdrNames.Add(Ledgerid);
                    dtCreditDebitReport.Columns.Add(Ledgerid);
                    ColCountVisible++;
                    dicColumnVisible.Add(ColCountVisible, "Visible");
                }
            }
            arrColHdrNames.Add("Total");
            dtCreditDebitReport.Columns.Add("Total");

            DataRow drHdr1 = dtCreditDebitReport.NewRow();
            for (int grCol = 0; grCol < dtCreditDebitReport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtCreditDebitReport.Rows.Add(drHdr1);

            #endregion

            #region Value

            int Sno = 1;
            string MonthVal = string.Empty;
            string Month = string.Empty;
            string Year = string.Empty;
            string Ledger_id = string.Empty;
            string hidTot = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                {
                    if (chkl_paid.Items[mode].Selected)
                    {
                        string payMode1 = Convert.ToString(chkl_paid.Items[mode].Value);
                        string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                        for (int monthloop = 0; monthloop < ds.Tables[1].Rows.Count; monthloop++)
                        {
                            Month = Convert.ToString(ds.Tables[1].Rows[monthloop]["month"]);
                            Year = Convert.ToString(ds.Tables[1].Rows[monthloop]["year"]);
                            //FpSpreadLedger.Sheets[0].RowCount++;
                            //FpSpreadLedger.Sheets[0].Cells[FpSpreadLedger.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(payModeText);//abarna
                            //FpSpreadLedger.Sheets[0].SpanModel.Add(FpSpreadLedger.Sheets[0].RowCount - 1, 0, 1, cbl_ledger.Items.Count + 3);
                            //FpSpreadLedger.Sheets[0].Cells[FpSpreadLedger.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F08080");
                            // FpSpread6.Sheets[0].Cells[FpSpread6.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                            if (dtDateVal.Rows.Count > 0)
                            {
                                for (int row = 0; row < dtDateVal.Rows.Count; row++)
                                {
                                    bool isCal = false;
                                    string transdate = Convert.ToString(dtDateVal.Rows[row]["TransDate"]);
                                    ds.Tables[0].DefaultView.RowFilter = "TransDate='" + transdate + "' and paymode='" + payMode1 + "' and month='" + Month + "' and year='" + Year + "'";
                                    DataTable dthead = ds.Tables[0].DefaultView.ToTable();
                                    if (dthead.Rows.Count > 0)
                                    {
                                        if (hsPaymodeHeader.Contains(payMode1))
                                        {
                                            hsPaymodeHeader.Remove(payMode1);
                                            hsPaymodeHeader.Add(payMode1, payModeText);
                                        }
                                        else
                                        {
                                            hsPaymodeHeader.Add(payMode1, payModeText);
                                            drowInst = dtCreditDebitReport.NewRow();
                                            drowInst[0] = Convert.ToString(payModeText);
                                            dtCreditDebitReport.Rows.Add(drowInst);
                                            dicRowColor.Add(dtCreditDebitReport.Rows.Count - 1, "Paymode");
                                        }
                                        isPaymodeVal = true;
                                        drowInst = dtCreditDebitReport.NewRow();
                                        drowInst[0] = Convert.ToString(Sno);
                                        Sno++;
                                        drowInst[1] = Convert.ToString(dtDateVal.Rows[row]["TransDate"]);
                                        string[] Date = transdate.Split('/');
                                        if (Date.Length == 3)
                                            transdate = Date[1].ToString() + "/" + Date[0].ToString() + "/" + Date[2].ToString();
                                        Month = Date[1];
                                        TotalDebit = 0;

                                        if (dthead.Rows.Count > 0)
                                        {
                                            for (int col = 0; col < dthead.Rows.Count; col++)
                                            {
                                                Ledger_id = Convert.ToString(dthead.Rows[col]["LedgerFK"]);
                                                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                                                {
                                                    string header = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                                                    if (Ledger_id == header)
                                                        drowInst[i - 1] = Convert.ToString(dthead.Rows[col]["Debit"].ToString());
                                                }
                                                debitAmt = Convert.ToInt32(dthead.Rows[col]["Debit"]);
                                                TotalDebit = TotalDebit + debitAmt;
                                                double.TryParse(Convert.ToString(dthead.Rows[col]["Debit"]), out debitAmt);
                                                //totalvalue += debitAmt;
                                                total += debitAmt;
                                                if (LedgerWiseTotal.Contains(Ledger_id))
                                                {
                                                    value = "";
                                                    value = LedgerWiseTotal[Ledger_id].ToString();
                                                    LedgerWiseTotal.Remove(Ledger_id);
                                                    total = 0;
                                                    total = Convert.ToInt32(value) + Convert.ToInt32(debitAmt);
                                                    LedgerWiseTotal.Add(Ledger_id, total);
                                                }
                                                else
                                                {
                                                    LedgerWiseTotal.Add(Ledger_id, Convert.ToInt32(debitAmt));
                                                }

                                                #region for grand total

                                                if (GrandTotal.Contains(Ledger_id))
                                                {
                                                    value = "";
                                                    value = GrandTotal[Ledger_id].ToString();
                                                    GrandTotal.Remove(Ledger_id);
                                                    total = 0;
                                                    total = Convert.ToInt32(value) + Convert.ToInt32(debitAmt);
                                                    GrandTotal.Add(Ledger_id, total);
                                                }
                                                else
                                                {
                                                    GrandTotal.Add(Ledger_id, Convert.ToInt32(debitAmt));
                                                }

                                                #endregion

                                                drowInst[dtCreditDebitReport.Columns.Count - 1] = TotalDebit;
                                                FinalTot = FinalTot + debitAmt;
                                            }
                                            dtCreditDebitReport.Rows.Add(drowInst);
                                        }
                                    }
                                }
                            }
                            if (isPaymodeVal == true)
                            {
                                drowInst = dtCreditDebitReport.NewRow();
                                drowInst[0] = "Total";
                                dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Total");
                                int TotCol = 2;
                                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                                {
                                    string LedgerName = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                                    if (LedgerWiseTotal.Contains(LedgerName))
                                        drowInst[i - 1] = Convert.ToString(LedgerWiseTotal[LedgerName]);
                                    TotCol++;
                                }
                                drowInst[TotCol - 1] = Convert.ToString(FinalTot);
                                dtCreditDebitReport.Rows.Add(drowInst);
                                LedgerWiseTotal.Clear();
                                FinalTot = 0;
                                isPaymodeVal = false;
                            }
                        }
                    }
                }

                #region printing the grandtotal

                drowInst = dtCreditDebitReport.NewRow();
                drowInst[0] = "Grand Total";
                dicRowColor.Add(dtCreditDebitReport.Rows.Count, "Grand Total");
                int GrandTotCol = 2;
                for (int i = 2; i < dtCreditDebitReport.Columns.Count; i++)
                {
                    string LedgerName = Convert.ToString(dtCreditDebitReport.Rows[0][i].ToString());
                    if (GrandTotal.Contains(LedgerName))
                        drowInst[i - 1] = Convert.ToString(GrandTotal[LedgerName]);
                    if (GrandTotal.Contains(LedgerName))
                    {
                        value = "";
                        value = GrandTotal[LedgerName].ToString();
                        GrandTotal.Remove(LedgerName);
                        total = 0;
                        total = Convert.ToInt32(value);
                        grandtotal += total;
                    }
                    GrandTotCol++;
                }
                drowInst[GrandTotCol - 1] = Convert.ToString(grandtotal);
                dtCreditDebitReport.Rows.Add(drowInst);

                #endregion

                GrdLedger.DataSource = dtCreditDebitReport;
                GrdLedger.DataBind();
                GrdLedger.Visible = true;

                GrdLedger.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GrdLedger.Rows[0].Font.Bold = true;
                GrdLedger.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Paymode")
                    {
                        GrdLedger.Rows[g].Cells[0].ColumnSpan = dtCreditDebitReport.Columns.Count;
                        for (int a = 1; a < dtCreditDebitReport.Columns.Count; a++)
                            GrdLedger.Rows[g].Cells[a].Visible = false;
                        GrdLedger.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdLedger.Rows[g].Cells[0].Font.Bold = true;
                        GrdLedger.Rows[g].BackColor = ColorTranslator.FromHtml("#F08080");
                    }
                    if (DicValue == "Total")
                    {
                        GrdLedger.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            GrdLedger.Rows[g].Cells[a].Visible = false;
                        GrdLedger.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdLedger.Rows[g].Font.Bold = true;
                        GrdLedger.Rows[g].BackColor = ColorTranslator.FromHtml("skyblue");
                    }
                    if (DicValue == "Grand Total")
                    {
                        GrdLedger.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            GrdLedger.Rows[g].Cells[a].Visible = false;
                        GrdLedger.Rows[g].Font.Bold = true;
                        GrdLedger.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GrdLedger.Rows[g].BackColor = Color.Green;
                    }
                }
                rptprint1.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                GrdLedger.Visible = false;
                //div1.Visible = false;
                rptprint1.Visible = false;
            }
            #endregion
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CreditdebitReport.aspx");
        }
    }

    protected void GrdLedger_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        int totalcol = 0;
        foreach (KeyValuePair<int, string> dr in dicColumnVisible)
        {
            int g = dr.Key;
            string DicValue = dr.Value;
            if (DicValue == "Visible")
            {
                e.Row.Cells[g].Visible = false;
                if (e.Row.RowIndex != 0)
                {
                    e.Row.Cells[g - 1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            totalcol = g;
        }
        e.Row.Cells[totalcol + 1].HorizontalAlign = HorizontalAlign.Right;
    }

    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            collegecode = ddl_collegename.SelectedItem.Value;
            chkl_paid.Items.Clear();
            txt_paid.Text = "--Select--";
            chk_paid.Checked = false;
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
                chk_paid.Checked = true;
            }
        }
        catch
        {

        }

    }
    public void chk_paid_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    #endregion

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    # endregion
}