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

public partial class FinanceTrialBalance : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    //string collname = "";
    string openbal = "";
    //string address1 = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            string PageLogOut = "";
            string sess = Convert.ToString(Session["IsLogin"]);
            PageLogOut = Convert.ToString(Session["PageLogout"]);
            if (sess == "")
            {
            }
            else
            {
                if (!Request.FilePath.Contains("FinM"))
                {
                    string strPreviousPage = "";
                    if (Request.UrlReferrer != null)
                    {
                        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                    }
                    if (strPreviousPage == "")
                    {
                        string redrURI = ConfigurationManager.AppSettings["Finance"].Trim();
                        Response.Redirect(redrURI, false);
                        return;
                    }
                }
            }
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();

            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                bindcollege();
                if (ddl_collegename.Items.Count > 0)
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                headerbind();
                ledgerbind();
                getHeadername();
                loadfinanceyear();
            }
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            //print.Attributes.Add("Style", "display:block;");
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        DAccess2 d2 = new DAccess2();
        try
        {
            ds.Clear();

            ds = d2.BindCollegebaseonrights(Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                headerbind();
                ledgerbind();
            }
        }
        catch
        {
        }
    }
    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        headerbind();
        ledgerbind();
    }

    #region headerandledger
    public void headerbind()
    {
        try
        {
            txt_studhed.Text = "Header";
            chk_studhed.Checked = false;
            chkl_studhed.Items.Clear();

            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "   ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
            }

        }
        catch (Exception ex) { }
    }
    public void ledgerbind()
    {
        try
        {
            txt_studled.Text = "Ledger";
            chk_studledg.Checked = false;
            string itemheadercode = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "" + "," + "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                }
            }

            chkl_studled.Items.Clear();

            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";

            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and  P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " and L.HeaderFK in (" + itemheadercode + ")";

            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studledg.Checked = true;
            }
        }
        catch (Exception ex) { }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            ledgerbind();
        }
        catch (Exception ex)
        { }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            ledgerbind();
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

    #region Financial Year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode1 + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                {
                    txtfyear.Text = "" + fnalyr + "";
                }
                else
                {
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                }
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
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
        string dateEnding = string.Empty;
        ds.Reset();
        ds = loaddata(ref dateEnding);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            // lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            loadSpreadValues(ds, dateEnding);
        }
        else
        {
            txtexcelname.Text = string.Empty;
            Fpspread1.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            print.Attributes.Add("Style", "display:block;");
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                //lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                // lblvalidation1.Visible = true;
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
            Printcontrolhed.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
            // lblvalidation1.Visible = false;
        }
        catch
        {

        }
    }
    public DataSet loaddata(ref string dateEnding)
    {
        DataSet data = new DataSet();
        try
        {
            string selectQuery = "";
            string headervalue = "";
            string ledgervalue = "";
            //UserbasedRights();
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            string finYear = Convert.ToString(getCblSelectedValue(chklsfyear));
            string fromdate = "";
            string todate = "";
            DateTime date = getBeforeDate();
            dateEnding = date.ToString("dd/MM/yyyy");
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            switch (rblType.SelectedIndex)
            {
                case 0:
                    selectQuery = " select headerfk,finyearfk,sum(credit)[debit],sum(debit)[credit] from ft_findailytransaction where transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "')  group by  headerfk,finyearfk";//and finyearfk in('" + finYear + "')
                    selectQuery += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_fincashcontradet where isnull(isbank,'0')='1' group by bankfk";
                    selectQuery += " union all  select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_finbanktransaction  where transdate between '" + fromdate + "' and '" + todate + "' group by bankfk ";
                    selectQuery += " select (accno+'-'+accholdername) as bankname,bankpk from fm_finbankmaster where collegecode='" + collegecode1 + "'";

                    selectQuery += " select headerfk,finyearfk,sum(credit)[debit],sum(debit)[credit] from ft_findailytransaction where transdate<='" + date.ToString("MM/dd/yyyy") + "'  and headerfk in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "')  group by  headerfk,finyearfk";//and finyearfk in('" + finYear + "')
                    selectQuery += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_fincashcontradet where isnull(isbank,'0')='1' and transdate<='" + date.ToString("MM/dd/yyyy") + "'  group by bankfk";
                    selectQuery += " union all  select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_finbanktransaction  where transdate between '" + fromdate + "' and '" + todate + "' group by bankfk ";
                    selectQuery += " select (accno+'-'+accholdername) as bankname,bankpk from fm_finbankmaster where collegecode='" + collegecode1 + "'";
                    selectQuery += " select headerpk,openbal from fm_headermaster where headerpk in('" + headervalue + "')";
                    break;
                case 1:
                    selectQuery = " select ledgerfk,finyearfk,sum(credit)[debit],sum(debit)[credit] from ft_findailytransaction where transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "')  group by  ledgerfk,finyearfk";//and finyearfk in('" + finYear + "')
                    selectQuery += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_fincashcontradet where isnull(isbank,'0')='1' group by bankfk";
                    selectQuery += " union all  select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_finbanktransaction  where transdate between '" + fromdate + "' and '" + todate + "' group by bankfk ";
                    selectQuery += " select (accno+'-'+accholdername) as bankname,bankpk from fm_finbankmaster where collegecode='" + collegecode1 + "'";

                     selectQuery += " select ledgerfk,finyearfk,sum(credit)[debit],sum(debit)[credit] from ft_findailytransaction where transdate<='" + date.ToString("MM/dd/yyyy") + "' and headerfk in('" + headervalue + "') and ledgerfk in('" + ledgervalue + "')  group by  ledgerfk,finyearfk";//and finyearfk in('" + finYear + "')
                     selectQuery += " select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_fincashcontradet where isnull(isbank,'0')='1' and transdate<='" + date.ToString("MM/dd/yyyy") + "' group by bankfk";
                     selectQuery += " union all  select sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,bankfk from ft_finbanktransaction  where transdate between '" + fromdate + "' and '" + todate + "' group by bankfk ";
                    selectQuery += " select (accno+'-'+accholdername) as bankname,bankpk from fm_finbankmaster where collegecode='" + collegecode1 + "'";
                    selectQuery += " select headerpk,openbal from fm_headermaster where headerpk in('" + headervalue + "')";

                    break;
            }

            data.Clear();
            data = d2.select_method_wo_parameter(selectQuery, "Text");

        }
        catch
        {
        }
        return data;
    }

    protected ArrayList getColumn()
    {
        ArrayList arCol = new ArrayList();
        try
        {
            arCol.Add("Sno");
            arCol.Add("Year Ending(₹)");
            arCol.Add("Particulars");
            arCol.Add("Debit(₹)");
            arCol.Add("Credit(₹)");
        }
        catch { }
        return arCol;
    }

    protected Dictionary<string, string> getFinancialYear()
    {
        Dictionary<string, string> fnlYear = new Dictionary<string, string>();
        for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
        {
            if (!chklsfyear.Items[fnlYr].Selected)
                continue;
            if (!fnlYear.ContainsKey(chklsfyear.Items[fnlYr].Value))
            {
                fnlYear.Add(chklsfyear.Items[fnlYr].Value, chklsfyear.Items[fnlYr].Text);
            }
        }
        return fnlYear;
    }

    private void loadSpreadValues(DataSet dsVal, string dateEnding)
    {
        try
        {
            #region Design
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.Sheets[0].ColumnCount = 0;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            ArrayList arColumn = getColumn();
            foreach (string columN in arColumn)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                int colCnt = Fpspread1.Sheets[0].ColumnCount - 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = columN;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[colCnt].Width = 40;
                switch (columN)
                {
                    case "Year Ending(₹)":
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = columN + "-" + dateEnding;
                        Fpspread1.Sheets[0].Columns[colCnt].Width = 150;
                        Fpspread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Right;
                        break;
                    case "Particulars":
                        Fpspread1.Sheets[0].Columns[colCnt].Width = 200;
                        Fpspread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        break;
                    case "Debit(₹)":
                        Fpspread1.Sheets[0].Columns[colCnt].Width = 80;
                        Fpspread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Right;
                        break;
                    case "Credit(₹)":
                        Fpspread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread1.Sheets[0].Columns[colCnt].Width = 80;
                        break;
                }
            }

            #endregion

            #region Value
            Hashtable htSubTot = new Hashtable();
            int rowNum = 0;
            Dictionary<string, string> fnalYear = getFinancialYear();
            Dictionary<string, string> hdName = getHeadername();
            Dictionary<string, string> feesName = getFeeName();
            int rowCnt = 0;
            //foreach (KeyValuePair<string, string> financialYear in fnalYear)
            //{
            //    int rowCnt = 0;
            //    Fpspread1.Sheets[0].RowCount++;
            //    rowCnt = Fpspread1.Sheets[0].RowCount - 1;
            //    Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = Convert.ToString(financialYear.Value);
            //    Fpspread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, Fpspread1.Sheets[0].ColumnCount - 1);
            //    Fpspread1.Sheets[0].Rows[rowCnt].BackColor = Color.Green;
            bool boolCheck = false;
            if (rblType.SelectedIndex == 0)
            {
                #region header
                foreach (KeyValuePair<string, string> headName in feesName)
                {

                    //for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                    //{
                    //    if (!chkl_studhed.Items[hd].Selected)
                    //        continue;
                    Fpspread1.Sheets[0].RowCount++;
                    rowCnt = Fpspread1.Sheets[0].RowCount - 1;
                    Fpspread1.Sheets[0].Cells[rowCnt, 0].Text = Convert.ToString(++rowNum);
                    double ob = 0;
                    double openBalhead = 0;
                    if (dsVal.Tables[3].Rows.Count > 0 || dsVal .Tables[6].Rows.Count>0)
                    {
                        dsVal.Tables[3].DefaultView.RowFilter = " headerfk='" + headName.Key + "' ";//and finyearfk='" + financialYear.Key + "'
                        dsVal.Tables[6].DefaultView.RowFilter = "headerpk='" + headName.Key + "' ";
                        DataTable dtOpen = dsVal.Tables[3].DefaultView.ToTable();
                        DataTable dtopenhead = dsVal.Tables[6].DefaultView.ToTable();
                        if (dtOpen.Rows.Count > 0 || dtopenhead.Rows.Count >0)
                            double.TryParse(Convert.ToString(dtOpen.Compute("sum(credit)", "")), out ob);
                        double.TryParse(Convert.ToString(dtopenhead.Compute("sum(openbal)", "")), out openBalhead);
                    }
                    double openBal = 0;
                    openBal = ob + openBalhead;
                    Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = Convert.ToString(openBal);
                    if (openBal == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = "-";
                    if (!htSubTot.ContainsKey("OpenBal"))
                        htSubTot.Add("OpenBal", openBal);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["OpenBal"]), out amount);
                        amount += openBal;
                        htSubTot.Remove("OpenBal");
                        htSubTot.Add("OpenBal", Convert.ToString(amount));
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 2].Text = Convert.ToString(headName.Value);
                    double debitAmt = 0;
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        dsVal.Tables[0].DefaultView.RowFilter = " headerfk='" + headName.Key + "' ";//and finyearfk='" + financialYear.Key + "'
                        DataTable dtDebit = dsVal.Tables[0].DefaultView.ToTable();
                        if (dtDebit.Rows.Count > 0)
                            double.TryParse(Convert.ToString(dtDebit.Compute("sum(debit)", "")), out debitAmt);
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = Convert.ToString(debitAmt);
                    if (debitAmt == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = "-";

                    if (!htSubTot.ContainsKey("Debit"))
                        htSubTot.Add("Debit", debitAmt);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                        amount += debitAmt;
                        htSubTot.Remove("Debit");
                        htSubTot.Add("Debit", Convert.ToString(amount));
                    }

                    double creditAmt = 0;
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        dsVal.Tables[0].DefaultView.RowFilter = " headerfk='" + headName.Key + "' ";//and finyearfk='" + financialYear.Key + "'
                        DataTable dtCredit = dsVal.Tables[0].DefaultView.ToTable();
                        if (dtCredit.Rows.Count > 0)
                            double.TryParse(Convert.ToString(dtCredit.Compute("sum(credit)", "")), out creditAmt);
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = Convert.ToString(creditAmt);
                    if (creditAmt == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = "-";

                    if (!htSubTot.ContainsKey("Credit"))
                        htSubTot.Add("Credit", creditAmt);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                        amount += creditAmt;
                        htSubTot.Remove("Credit");
                        htSubTot.Add("Credit", Convert.ToString(amount));
                    }
                    boolCheck = true;
                }
                #endregion
            }
            else
            {
                #region ledger
                string lastName = string.Empty;
                foreach (KeyValuePair<string, string> ldName in feesName)
                {

                    //  }
                    //for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                    //{
                    //    if (!chkl_studled.Items[hd].Selected)
                    //        continue;
                    if (hdName.ContainsKey(ldName.Key))
                    {
                        string NewName = Convert.ToString(hdName[ldName.Key]);
                        if (lastName != NewName)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            rowCnt = Fpspread1.Sheets[0].RowCount - 1;
                            lastName = NewName;
                            Fpspread1.Sheets[0].Cells[rowCnt, 0].Text = lastName;
                            Fpspread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                            Fpspread1.Sheets[0].Rows[rowCnt].BackColor = Color.YellowGreen;
                        }
                    }
                    Fpspread1.Sheets[0].RowCount++;
                    rowCnt = Fpspread1.Sheets[0].RowCount - 1;
                    Fpspread1.Sheets[0].Cells[rowCnt, 0].Text = Convert.ToString(++rowNum);
                    double ob = 0;
                    double openBalhead = 0;
                    if (dsVal.Tables[3].Rows.Count > 0)
                    {
                        dsVal.Tables[3].DefaultView.RowFilter = " ledgerfk='" + ldName.Key + "' ";
                        //and finyearfk='" + financialYear.Key + "'

                      //  dsVal.Tables[6].DefaultView.RowFilter = "headerpk='" + headName.Key + "' ";
                        DataTable dtOpen = dsVal.Tables[3].DefaultView.ToTable();
                        DataTable dtopenhead = dsVal.Tables[6].DefaultView.ToTable();
                        if (dtOpen.Rows.Count > 0 || dtopenhead.Rows.Count > 0)
                            double.TryParse(Convert.ToString(dtOpen.Compute("sum(credit)", "")), out ob);
                        double.TryParse(Convert.ToString(dtopenhead.Compute("sum(openbal)", "")), out openBalhead);
                    }
                    double openBal = 0;
                    openBal = ob + openBalhead;
                    Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = Convert.ToString(openBal);
                    if (openBal == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = "-";
                    if (!htSubTot.ContainsKey("OpenBal"))
                        htSubTot.Add("OpenBal", openBal);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["OpenBal"]), out amount);
                        amount += openBal;
                        htSubTot.Remove("OpenBal");
                        htSubTot.Add("OpenBal", Convert.ToString(amount));
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 2].Text = Convert.ToString(ldName.Value);
                    double debitAmt = 0;
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        dsVal.Tables[0].DefaultView.RowFilter = " ledgerfk='" + ldName.Key + "' ";//and finyearfk='" + financialYear.Key + "'
                        DataTable dtDebit = dsVal.Tables[0].DefaultView.ToTable();
                        if (dtDebit.Rows.Count > 0)
                            double.TryParse(Convert.ToString(dtDebit.Compute("sum(debit)", "")), out debitAmt);
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = Convert.ToString(debitAmt);
                    if (debitAmt == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = "-";
                    if (!htSubTot.ContainsKey("Debit"))
                        htSubTot.Add("Debit", debitAmt);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                        amount += debitAmt;
                        htSubTot.Remove("Debit");
                        htSubTot.Add("Debit", Convert.ToString(amount));
                    }

                    double creditAmt = 0;
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        dsVal.Tables[0].DefaultView.RowFilter = " ledgerfk='" + ldName.Key + "' ";//and finyearfk='" + financialYear.Key + "'
                        DataTable dtCredit = dsVal.Tables[0].DefaultView.ToTable();
                        if (dtCredit.Rows.Count > 0)
                            double.TryParse(Convert.ToString(dtCredit.Compute("sum(credit)", "")), out creditAmt);
                    }
                    Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = Convert.ToString(creditAmt);
                    if (creditAmt == 0)
                        Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = "-";

                    if (!htSubTot.ContainsKey("Credit"))
                        htSubTot.Add("Credit", creditAmt);
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                        amount += creditAmt;
                        htSubTot.Remove("Credit");
                        htSubTot.Add("Credit", Convert.ToString(amount));
                    }
                    boolCheck = true;
                }
                #endregion
            }
            if (boolCheck)
            {
                bool boolBank = false;
                for (int bnK = 0; bnK < ds.Tables[2].Rows.Count; bnK++)
                {                   
                    ds.Tables[1].DefaultView.RowFilter = "bankfk='" + Convert.ToString(ds.Tables[2].Rows[bnK]["bankpk"]) + "'";
                    DataTable dtBank = ds.Tables[1].DefaultView.ToTable();
                    if (dtBank.Rows.Count > 0)
                    {
                        if (!boolBank)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            rowCnt = Fpspread1.Sheets[0].RowCount - 1;
                            Fpspread1.Sheets[0].Cells[rowCnt, 0].Text = "Bank Balances";
                            Fpspread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                            Fpspread1.Sheets[0].Rows[rowCnt].BackColor = Color.Gray;
                            boolBank = true;
                        }
                        Fpspread1.Sheets[0].RowCount++;
                        rowCnt = Fpspread1.Sheets[0].RowCount - 1;
                        Fpspread1.Sheets[0].Cells[rowCnt, 0].Text = Convert.ToString(++rowNum);
                       // double openBal = 0;
                        double ob = 0;
                        double openBalhead = 0;
                        if (dsVal.Tables[4].Rows.Count > 0)
                        {
                            dsVal.Tables[4].DefaultView.RowFilter = " bankfk='" + Convert.ToString(ds.Tables[2].Rows[bnK]["bankpk"]) + "' ";//and finyearfk='" + financialYear.Key + "'
                            DataTable dtOpen = dsVal.Tables[3].DefaultView.ToTable();
                            DataTable dtopenhead = dsVal.Tables[6].DefaultView.ToTable();
                            if (dtOpen.Rows.Count > 0 || dtopenhead.Rows.Count > 0)
                                double.TryParse(Convert.ToString(dtOpen.Compute("sum(credit)", "")), out ob);
                            double.TryParse(Convert.ToString(dtopenhead.Compute("sum(openbal)", "")), out openBalhead);
                        }
                        double openBal = 0;
                        openBal = ob + openBalhead;
                        Fpspread1.Sheets[0].Cells[rowCnt, 1].Text = Convert.ToString(openBal);
                        if (!htSubTot.ContainsKey("OpenBal"))
                            htSubTot.Add("OpenBal", openBal);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htSubTot["OpenBal"]), out amount);
                            amount += openBal;
                            htSubTot.Remove("OpenBal");
                            htSubTot.Add("OpenBal", Convert.ToString(amount));
                        }
                        Fpspread1.Sheets[0].Cells[rowCnt, 2].Text = Convert.ToString(ds.Tables[2].Rows[bnK]["bankname"]);
                        double debitAmt = 0;
                        double.TryParse(Convert.ToString(dtBank.Rows[0]["credit"]), out debitAmt);
                        Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = Convert.ToString(debitAmt);
                        if (debitAmt == 0)
                            Fpspread1.Sheets[0].Cells[rowCnt, 3].Text = "-";

                        if (!htSubTot.ContainsKey("Debit"))
                            htSubTot.Add("Debit", debitAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                            amount += debitAmt;
                            htSubTot.Remove("Debit");
                            htSubTot.Add("Debit", Convert.ToString(amount));
                        }

                        double creditAmt = 0;
                        double.TryParse(Convert.ToString(dtBank.Rows[0]["debit"]), out creditAmt);
                        Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = Convert.ToString(creditAmt);
                        if (creditAmt == 0)
                            Fpspread1.Sheets[0].Cells[rowCnt, 4].Text = "-";

                        if (!htSubTot.ContainsKey("Credit"))
                            htSubTot.Add("Credit", creditAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                            amount += creditAmt;
                            htSubTot.Remove("Credit");
                            htSubTot.Add("Credit", Convert.ToString(amount));
                        }
                    }
                }
            }


            // Fpspread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            if (boolCheck)
            {
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 2].Text = "Deficit";
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                // Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;

                double debit = 0;
                double credit = 0;
                double.TryParse(Convert.ToString(htSubTot["Debit"]), out debit);
                double.TryParse(Convert.ToString(htSubTot["Credit"]), out credit);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = "-";
                if (debit > credit)
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(debit - credit);

                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                double grandvalue = 0;
                double.TryParse(Convert.ToString(htSubTot["OpenBal"]), out grandvalue);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(grandvalue);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(debit);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(credit);
            }

            //for (int j = 2; j < FpSpread1.Sheets[0].ColumnCount; j++)
            //{
            //    double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            //}

            // }
            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Visible = true;
            print.Attributes.Add("Style", "display:block;");
            // print.Visible = true;
            Fpspread1.SaveChanges();


            #endregion
        }
        catch { }
    }
    protected void rblType_Selected(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        print.Attributes.Add("Style", "display:none;");
        // lblvalidation1.Text = string.Empty;
        txtexcelname.Text = string.Empty;
        // print.Visible = false;
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
            string headerfk = Convert.ToString(getCblSelectedValue(chkl_studhed));
            selQFK = "select ledgerpk as pk,headername as name from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and HeaderPK in('" + headerfk + "') order by headerpk";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch
        {
            hthdName.Clear();
        }

        return hthdName;
    }
    protected Dictionary<string, string> getFeeName()
    {
        Dictionary<string, string> hthdName = new Dictionary<string, string>();
        try
        {

            string selQFK = string.Empty;
            string headerfk = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ledgerfk = Convert.ToString(getCblSelectedValue(chkl_studled));
            if (rblType.SelectedIndex == 0)
                selQFK = "select headerpk as pk,headername as name from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and HeaderPK in('" + headerfk + "') and ledgerpk in('" + ledgerfk + "') order by headerpk";
            else
                selQFK = "select ledgerpk as pk,ledgername as name from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and HeaderPK in('" + headerfk + "') and ledgerpk in('" + ledgerfk + "') order by headerpk";

            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch
        {
            hthdName.Clear();
        }

        return hthdName;
    }

    protected DateTime getBeforeDate()
    {
        DateTime dtDate = new DateTime();
        try
        {
            string finYearid = d2.getCurrentFinanceYear(usercode, ddl_collegename.SelectedValue);
            string selQDt = d2.GetFunction("select finyearstart from fm_finyearmaster where collegecode='" + ddl_collegename.SelectedValue + "' and finyearpk='" + finYearid + "'");
            if (selQDt != "0")
            {
                DateTime dtFirst = Convert.ToDateTime(selQDt);
                dtDate = dtFirst.AddDays(-1);                
            }
        }
        catch { }
        return dtDate;
    }
    protected string getDate()
    {
        string tempDate = string.Empty;
        try
        {
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            DateTime dtFrom = Convert.ToDateTime(fromdate);
            DateTime dtTo = Convert.ToDateTime(todate);
            string selQFK = "select finyearstart,finyearend from fm_finyearmaster where collegecode='" + ddl_collegename.SelectedValue + "'";
            //  string selQFK = "select convert(varchar(10),finyearstart,103) as finyearstart,convert(varchar(10),finyearend,103) as finyearend from fm_finyearmaster where collegecode='" + ddl_collegename.SelectedValue + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    DateTime dtFirst = Convert.ToDateTime(dsval.Tables[0].Rows[row]["finyearstart"]);
                    DateTime dtSecond = Convert.ToDateTime(dsval.Tables[0].Rows[row]["finyearend"]);
                    if (dtFirst >= dtFrom && dtSecond >= dtTo)
                    {
                        tempDate = Convert.ToString(dtFirst.AddDays(-1));
                        break;
                    }

                }
            }
            //  string finYearid = d2.getCurrentFinanceYear(usercode, ddl_collegename.SelectedValue);
            //string getCurFnlYr = d2.GetFunction("select convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103) from fm_finyearmaster where finyearpk='" + finYearid + "'");
            //if (!string.IsNullOrEmpty(getCurFnlYr) && getCurFnlYr != "0")
            //{

            //}
        }
        catch { }
        return tempDate;
    }
}