using System;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Configuration;
//developed by abarna
public partial class ReceiptPaymentCumulative : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    string headervalue = "";
    string ledgervalue = "";
    //string collname = "";
    string openbal = "";
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    //string address1 = "";    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
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

            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                bindCollege();
                collegecode1 = Convert.ToString(getCblSelectedValue(cblclg));
                //if (ddl_collegename.Items.Count > 0)
                //    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                // headerbind();
                //ledgerbind();
                bindheader();
                bindledger();
                getHeadername();
            }
            collegecode1 = Convert.ToString(getCblSelectedValue(cblclg));
            //if (ddl_collegename.Items.Count > 0)
            //    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }

    //public void bindcollege()
    //{
    //    DAccess2 d2 = new DAccess2();
    //    try
    //    {
    //        ds.Clear();

    //        ds = d2.BindCollegebaseonrights(Session["usercode"].ToString());
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_collegename.DataSource = ds;
    //            ddl_collegename.DataTextField = "collname";
    //            ddl_collegename.DataValueField = "college_code";
    //            ddl_collegename.DataBind();
    //            headerbind();
    //            ledgerbind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //headerbind();
        //ledgerbind();
        bindheader();
        bindledger();
    }

    #region headerandledger
    //public void loadheaderandledger()
    //{
    //    try
    //    {
    //        string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        chkl_studhed.Items.Clear();
    //        string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  order by len(isnull(hd_priority,10000)),hd_priority asc";

    //        ds = d2.select_method_wo_parameter(query, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            chkl_studhed.DataSource = ds;
    //            chkl_studhed.DataTextField = "HeaderName";
    //            chkl_studhed.DataValueField = "HeaderPK";
    //            chkl_studhed.DataBind();
    //            for (int i = 0; i < chkl_studhed.Items.Count; i++)
    //            {
    //                chkl_studhed.Items[i].Selected = true;
    //            }
    //            txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
    //            chk_studhed.Checked = true;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    //public void ledgerload()
    //{
    //    try
    //    {
    //        string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        chkl_studled.Items.Clear();
    //        string hed = "";
    //        for (int i = 0; i < chkl_studhed.Items.Count; i++)
    //        {
    //            if (chkl_studhed.Items[i].Selected == true)
    //            {
    //                if (hed == "")
    //                {
    //                    hed = chkl_studhed.Items[i].Value.ToString();
    //                }
    //                else
    //                {
    //                    hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
    //                }
    //            }
    //        }


    //        string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by len(isnull(l.priority,1000)) , l.priority asc";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(query1, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            chkl_studled.DataSource = ds;
    //            chkl_studled.DataTextField = "LedgerName";
    //            chkl_studled.DataValueField = "LedgerPK";
    //            chkl_studled.DataBind();
    //            for (int i = 0; i < chkl_studled.Items.Count; i++)
    //            {
    //                chkl_studled.Items[i].Selected = true;
    //            }
    //            txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
    //            chk_studledg.Checked = true;

    //        }
    //        else
    //        {
    //            for (int i = 0; i < chkl_studled.Items.Count; i++)
    //            {
    //                chkl_studled.Items[i].Selected = false;
    //            }
    //            txt_studled.Text = "--Select--";
    //            chk_studledg.Checked = false;
    //        }

    //    }
    //    catch
    //    {
    //    }
    //}
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            bindledger();
        }
        catch (Exception ex)
        { }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            bindledger();
        }
        catch (Exception ex)
        {

        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studledg, chkl_studled, txt_studled, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studledg, chkl_studled, txt_studled, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion
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
    protected void btn_search_click(object sender, EventArgs e)
    {
        loadspread(loaddata());

        if (dtl.Rows.Count > 1)
        {
            gridview1.DataSource = dtl;
            gridview1.DataBind();

            for (int i = 0; i < gridview1.Rows.Count; i++)
            {
                for (int j = 0; j < gridview1.HeaderRow.Cells.Count; j++)
                {
                    if (j == 0)
                    {
                        gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        gridview1.Rows[i].Cells[j].Width = 40;
                       
                    }
                    else if (j == 1)
                    {
                        gridview1.Rows[i].Cells[j].Width = 300;
                    }
                    else if (j == 2)
                    {
                        gridview1.Rows[i].Cells[j].Width = 150;
                    }
                    else if (j == 3)
                    {
                        gridview1.Rows[i].Cells[j].Width = 300;
                    }
                    else if (j == 4)
                    {
                        gridview1.Rows[i].Cells[j].Width = 150;
                    }
                    if (i == 0)
                    {
                        gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        gridview1.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        gridview1.Rows[i].Cells[j].BorderColor = Color.Black;
                        gridview1.Rows[i].Cells[j].Font.Bold = true;
                        gridview1.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                        gridview1.Rows[i].Cells[j].Font.Size = FontUnit.Medium;


             
                    }
                    else
                    {
                        string value = gridview1.Rows[i].Cells[j].Text.ToString();
                        string[] splitval = value.Split('^');
                        if (splitval.Length > 1)
                        {
                            gridview1.Rows[i].Cells[j].Text = splitval[0];
                            if (splitval[1] == "Green")
                                gridview1.Rows[i].Cells[j].BackColor = Color.Green;
                            else if(splitval[1] == "YellowGreen")
                                gridview1.Rows[i].Cells[j].BackColor = Color.YellowGreen;

                        }
                        int colspan = 1;
                        
                        if (j == 0)
                        {
                            while (gridview1.Rows[i].Cells[j].Text != "&nbsp;" && gridview1.Rows[i].Cells[j + colspan].Text == "&nbsp;")
                            {
                                colspan++;
                                if (gridview1.HeaderRow.Cells.Count == j + colspan)
                                    break;

                            }
                        }
                        
                        if (colspan != 1)
                        {
                            gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            
                            gridview1.Rows[i].Cells[j].ColumnSpan = colspan;
                            for (int a = j + 1; a < j + colspan; a++)
                                gridview1.Rows[i].Cells[a].Visible = false;
                        }

                      
                    

                    }
                   
                    

                }

            }
            gridview1.Visible = true;


        }

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                
                d2.printexcelreportgrid(gridview1, reportname);
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
            
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {

        }
    }
    public DataSet loaddata()
    {
        DataSet data = new DataSet();
        try
        {

            //UserbasedRights();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string headervaluetext = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ledgervaluetext = Convert.ToString(getCblSelectedValue(chkl_studled));
            //collegecode1 = Convert.ToString(getCblSelectedValue(cblclg));
            //headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            //ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            headervalue = getHeaderFK(headervaluetext, collegecode);
            ledgervalue = getLedgerFK(ledgervaluetext, collegecode);
            string fromdate = "";
            string todate = "";

            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                {
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }
            selectQuery = "select HeaderFK,ledgerfk,sum(debit)[Debit],convert(varchar(10),transdate,103)[transdate] from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate<'" + fromdate + "' and HeaderFK in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "') group by HeaderFK,ledgerfk,paymode,transdate having sum(debit)>0";
            selectQuery += "select HeaderFK,ledgerfk,sum(debit)[Debit],convert(varchar(10),transdate,103)[transdate] from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and HeaderFK in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "') group by HeaderFK,ledgerfk,paymode,transdate having sum(debit)>0";
            selectQuery += "select HeaderFK,ledgerfk,sum(credit)[Credit],convert(varchar(10),transdate,103)[transdate] from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and HeaderFK in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "') group by HeaderFK,ledgerfk,paymode,transdate having sum(credit)>0";
            selectQuery += "select distinct f.HeaderFK,headername,f.ledgerfk,ledgername from ft_findailytransaction f,fm_headermaster h,fm_ledgermaster l where f.headerfk=h.headerpk and f.headerfk=l.headerfk and f.ledgerfk=l.ledgerpk and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and f.HeaderFK in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')";
            selectQuery += " select headerpk,openbal from fm_headermaster where headerpk in('" + headervalue + "')";
            data.Clear();

            data = d2.select_method_wo_parameter(selectQuery, "Text");

        }
        catch
        {
        }
        return data;
    }
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lbl_collegename.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lbl_collegename.Text, "--Select--");

    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lbl_collegename.Text, "--Select--");

    }
    private void loadspread(DataSet ds)
    {
        try
        {
            gridview1.Visible = true;
            rptprint.Visible = true;
            #region design

            

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            for (int col = 0; col < 5; col++)
            {
                dtl.Columns.Add("", typeof(string));
                
            }

            

            dtl.Rows[0][0] = "S.No";

            

            dtl.Rows[0][1] = "Receipts";

            

            dtl.Rows[0][2] = "Amounts";

           

            dtl.Rows[0][3] = "Payments";

            

            dtl.Rows[0][4] = "Amounts";

            string format = "Receipts and Payment for the Date of-" + Convert.ToString(txt_fromdate.Text) + "-" + Convert.ToString(txt_todate.Text);

            #endregion

            DataRow drow;
            int rowcount = 0;
            DataTable dtnew = new DataTable();
            getHeadername();
            Dictionary<string, string> hthdName = getHeadername();

            //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            //{
            //    Fpspread1.Sheets[0].RowCount++;
            //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = 
            //    //ds.Tables[1].DefaultView.RowFilter = "subType_no ='" + subno + "'";
            //    dtnew = ds.Tables[1].DefaultView.ToTable();
            //}


            Hashtable htRowCount = new Hashtable();
            int totRcptCount = 0;
            int RcptCountStart = 0;
            double grandtotal = 0;//abarna
            double grandtotalp = 0;
            foreach (KeyValuePair<string, string> hdVal in hthdName)
            {
                

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                

                dtl.Rows[dtl.Rows.Count - 1][0] = Convert.ToString(hdVal.Value) + "^Green";

                

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                


                dtl.Rows[dtl.Rows.Count - 1][0] = format;

                double openbal = 0;
                double openbalancehead = 0;
                double openbalance = 0;
                htRowCount = new Hashtable();
                totRcptCount = 0;
                RcptCountStart = 0;
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {

                    int sno = 0;
                    double total = 0;
                    string headerfk = "select headerpk from fm_headermaster where headername in('" + hdVal.Value + "') and collegecode in('" + collegecode + "')";
                    //string[] str = headerfk.Split(',');
                    DataSet s = d2.select_method_wo_parameter(headerfk, "Text");
                    string str = "";
                    for (int k = 0; k < s.Tables[0].Rows.Count; k++)
                    {
                        
                        if (str == "")
                            str = Convert.ToString(s.Tables[0].Rows[k]["headerpk"]);
                        else
                            str = str + "','" + Convert.ToString(s.Tables[0].Rows[k]["headerpk"]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = "headerfk in('" + str  + "')";
                    ds.Tables[4].DefaultView.RowFilter = "headerpk in('" + str + "')";
                    DataTable dtOpenBal = ds.Tables[0].DefaultView.ToTable();
                    DataTable dtopenhead = ds.Tables[4].DefaultView.ToTable();
                    object sum = dtOpenBal.Compute("Sum(debit)", "");
                    object sumhead = dtopenhead.Compute("sum(openbal)", "");
                    // object sum = ds.Tables[0].Compute("Sum(debit)", "");
                    //sno++;
                    double.TryParse(Convert.ToString(sum), out openbal);
                    double.TryParse(Convert.ToString(sumhead), out openbalancehead);
                    openbalance = openbal + openbalancehead;
                    

                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    

                    dtl.Rows[dtl.Rows.Count - 1][1] = "OPENING BALANCE";
                    dtl.Rows[dtl.Rows.Count - 1][2] = d2 .numberformat(openbalance.ToString());
                    total = openbalance;
                    //initial count set
                    
                    

                    if (RcptCountStart == 0)
                        int.TryParse(Convert.ToString((dtl.Rows.Count - 1)), out RcptCountStart);
                    
                    totRcptCount++;
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        DataTable dtDistinctLedger = new DataTable();
                        ds.Tables[1].DefaultView.RowFilter = "headerfk in('" + str + "')";

                        DataTable dtTablOne = ds.Tables[1].DefaultView.ToTable();

                        // dtDistinctLedger = ds.Tables[1].DefaultView.ToTable(true, "ledgerfk");
                        dtDistinctLedger = dtTablOne.DefaultView.ToTable(true, "ledgerfk");
                        double debit = 0;
                        foreach (DataRow drrow in dtDistinctLedger.Rows)
                        {
                            string ledgerId = Convert.ToString(drrow["ledgerfk"]).Trim();

                            //ds.Tables[1].DefaultView.RowFilter = "ledgerfk='" + ledgerId + "'";

                            double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(debit)", "ledgerfk='" + ledgerId + "'")), out debit);
                            DataView ledger = new DataView();
                            if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                            {
                                ds.Tables[3].DefaultView.RowFilter = "ledgerfk='" + ledgerId + "'";
                                ledger = ds.Tables[3].DefaultView;
                            }
                            if (ledger.Count > 0)
                            {
                                sno++;
                                
                                dtrow = dtl.NewRow();
                                dtl.Rows.Add(dtrow); 

                                

                                dtl.Rows[dtl.Rows.Count - 1][0] = Convert.ToString(sno);

                                

                                dtl.Rows[dtl.Rows.Count - 1][1] = Convert.ToString(ledger[0]["LedgerName"]);

                               

                                dtl.Rows[dtl.Rows.Count - 1][2] = d2.numberformat(Convert.ToString(debit));

                                totRcptCount++;
                            }
                            total += debit;
                        }
                        grandtotal += total;
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow); 

                        

                        dtl.Rows[dtl.Rows.Count - 1][1] = "Total^YellowGreen"; 

                        
                       
                        dtl.Rows[dtl.Rows.Count - 1][2] = d2.numberformat(Convert.ToString(total)) + "^YellowGreen";
                        //foreach (DataRow drrow1 in ds.Tables[2].Rows)
                        //{
                        //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ledger[0]["LedgerName"]);
                        //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(drrow1["credit"]);
                        //}
                    }


                    double creditAmt = 0;
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        #region Voucher
                        sno = 0;
                        double closingbalance = 0;
                        double credittotal = 0;
                        int startingRow = RcptCountStart;
                        int maxRowCount = totRcptCount;

                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                        {
                            DataTable dtDistinctLedgerCredit = new DataTable();
                            ds.Tables[2].DefaultView.RowFilter = "headerfk in('" + str + "')";
                            DataTable dtTablTwo = ds.Tables[2].DefaultView.ToTable();
                            dtDistinctLedgerCredit = dtTablTwo.DefaultView.ToTable(true, "ledgerfk");
                            //  dtDistinctLedgerCredit = ds.Tables[2].DefaultView.ToTable(true, "ledgerfk");
                            foreach (DataRow drrow in dtDistinctLedgerCredit.Rows)
                            {
                                string ledgerId = Convert.ToString(drrow["ledgerfk"]).Trim();
                                double credit = 0;
                                double.TryParse(Convert.ToString(ds.Tables[2].Compute("Sum(credit)", "ledgerfk='" + ledgerId + "'")), out credit);
                                DataView ledger = new DataView();
                                if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                                {
                                    ds.Tables[3].DefaultView.RowFilter = "ledgerfk='" + ledgerId + "'";

                                    ledger = ds.Tables[3].DefaultView;
                                }
                                if (ledger.Count > 0)
                                {
                                    sno++;
                                    if (maxRowCount == 0)
                                    {
                                        

                                        dtrow = dtl.NewRow();
                                        dtl.Rows.Add(dtrow); 

                                        
                                        int.TryParse(Convert.ToString((dtl.Rows.Count - 1)), out startingRow);
                                    }
                                    else
                                        maxRowCount--;
                                    //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                   

                                    dtl.Rows[startingRow][3] = Convert.ToString(ledger[0]["LedgerName"]);

                                    

                                    dtl.Rows[startingRow][4] = d2.numberformat(Convert.ToString(credit));

                                    startingRow++;
                                    creditAmt += credit;
                                }
                            }
                            if (total != 0)
                            {
                                if (maxRowCount == 0)
                                {
                                    

                                    dtrow = dtl.NewRow();
                                    dtl.Rows.Add(dtrow); 

                                    
                                    int.TryParse(Convert.ToString((dtl.Rows.Count - 1)), out startingRow);
                                }
                                

                                dtl.Rows[startingRow][3] = "Closing Balance^YellowGreen";
                                // Fpspread1.Sheets[0].Cells[startingRow, 3].Tag = "total-debitamount";
                                
                                closingbalance = total - creditAmt;
                                

                                dtl.Rows[startingRow][4] = d2.numberformat(Convert.ToString(closingbalance));


                            }
                            

                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow); 
                            //startingRow++;

                            credittotal = creditAmt + closingbalance;
                            // Fpspread1.Sheets[0].RowCount++;
                            //
                           
                            dtl.Rows[dtl.Rows.Count-1][3] = "Total^YellowGreen";

                            
                            dtl.Rows[dtl.Rows.Count - 1][4] = d2.numberformat(Convert.ToString(credittotal)) + "^YellowGreen";

                            
                            //Fpspread1.Sheets[0].Cells[startingRow, 3].Text = "Total";
                            //Fpspread1.Sheets[0].Cells[startingRow, 4].Text = Convert.ToString(credittotal);
                            //Fpspread1.Sheets[0].Cells[startingRow, 3].BackColor = Color.Yellow;
                            //Fpspread1.Sheets[0].Cells[startingRow, 4].BackColor = Color.Yellow;
                            grandtotalp += credittotal;
                        }

                        #endregion
                    }

                }

            }
            

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow); 

            

            dtl.Rows[dtl.Rows.Count - 1][1] = "Grand Total^YellowGreen";

            

            dtl.Rows[dtl.Rows.Count - 1][2] = d2.numberformat(Convert.ToString(grandtotal)) + "^YellowGreen";

            

            dtl.Rows[dtl.Rows.Count - 1][3] = "Total" + "^YellowGreen";

            

            dtl.Rows[dtl.Rows.Count - 1][4] = d2.numberformat(Convert.ToString(grandtotalp)) + "^YellowGreen";

            
        }
        catch { }
    }
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

    //public void headerbind()
    //{
    //    try
    //    {
    //        txt_studhed.Text = "Header";
    //        chk_studhed.Checked = false;
    //        chkl_studhed.Items.Clear();

    //        string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in( '" + collegecode1 + " ')  ";
    //        ds = d2.select_method_wo_parameter(query, "Text");

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            chkl_studhed.DataSource = ds;
    //            chkl_studhed.DataTextField = "HeaderName";
    //            chkl_studhed.DataValueField = "HeaderPK";
    //            chkl_studhed.DataBind();
    //            for (int i = 0; i < chkl_studhed.Items.Count; i++)
    //            {
    //                chkl_studhed.Items[i].Selected = true;
    //            }
    //            txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
    //            chk_studhed.Checked = true;
    //        }

    //    }
    //    catch (Exception ex) { }
    //}
    //public void ledgerbind()
    //{
    //    try
    //    {
    //        txt_studled.Text = "Ledger";
    //        chk_studledg.Checked = false;
    //        string itemheadercode = "";
    //        for (int i = 0; i < chkl_studhed.Items.Count; i++)
    //        {
    //            if (chkl_studhed.Items[i].Selected == true)
    //            {
    //                if (itemheadercode == "")
    //                {
    //                    itemheadercode = "" + chkl_studhed.Items[i].Value.ToString() + "";
    //                }
    //                else
    //                {
    //                    itemheadercode = itemheadercode + "" + "," + "" + chkl_studhed.Items[i].Value.ToString() + "";
    //                }
    //            }
    //        }

    //        chkl_studled.Items.Clear();

    //        //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";

    //        string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and  P. UserCode = " + usercode + " AND L.CollegeCode in( '" + collegecode1 + "') and L.HeaderFK in (" + itemheadercode + ")";

    //        ds = d2.select_method_wo_parameter(query, "Text");

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            chkl_studled.DataSource = ds;
    //            chkl_studled.DataTextField = "LedgerName";
    //            chkl_studled.DataValueField = "LedgerPK";
    //            chkl_studled.DataBind();
    //            for (int i = 0; i < chkl_studled.Items.Count; i++)
    //            {
    //                chkl_studled.Items[i].Selected = true;
    //            }
    //            txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
    //            chk_studledg.Checked = true;
    //        }
    //    }
    //    catch (Exception ex) { }
    //}
    public void bindheader()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderName";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = Label1.Text + "(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
                bindledger();
            }
        }
        catch
        {
        }
    }
    #region Ledger
    public void bindledger()
    {
        try
        {
            string headercode;

            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            headercode = Convert.ToString(getCblSelectedValue(chkl_studhed));
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studledg.Checked = false;
            if (Convert.ToString(collegecode) != "" && Convert.ToString(headercode) != "")
            {
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P where l.HeaderFK =h.HeaderPK   and L.LedgerPK = P.LedgerFK and l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ds;
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataValueField = "ledgername";
                    chkl_studled.DataBind();
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        chkl_studled.Items[i].Selected = true;
                    }
                    txt_studled.Text = Label2.Text + "(" + chkl_studled.Items.Count + ")";
                    chk_studledg.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    # endregion
    //public void getheader()
    //{
    //    string collname = "";
    //    string header = "";
    //    string format = "";

    //    header = "select headername from fm_headermaster where HeaderPK='" + headervalue + "'";
    //    collname = "select collname,address1,address2  from collinfo where college_code='" + collegecode1 + "'";
    //    format = "Receipt and Payments for the";

    //}

    protected Dictionary<string, string> getHeadername()
    {
        Dictionary<string, string> hthdName = new Dictionary<string, string>();
        try
        {

            string selQFK = string.Empty;

            selQFK = "select headername as name from fm_headermaster where HeaderPK in('" + headervalue + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["name"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["name"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch
        {
            hthdName.Clear();
        }

        return hthdName;
    }
    protected Dictionary<string, string> getHeadername1()
    {
        Dictionary<string, string> hthdName1 = new Dictionary<string, string>();
        try
        {

            string selQFK = string.Empty;

            selQFK = "select headername as name from fm_headermaster where HeaderPK in('" + headervalue + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName1.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["name"])))
                        hthdName1.Add(Convert.ToString(dsval.Tables[0].Rows[row]["name"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch
        {
            hthdName1.Clear();
        }

        return hthdName1;
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
    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    protected string getHeaderFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct headerpk from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["headerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }
    protected string getLedgerFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct ledgerpk from fm_ledgermaster where collegecode in('" + collegecode + "') and ledgername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["ledgerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
}