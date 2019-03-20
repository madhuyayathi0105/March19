using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Configuration;

public partial class StudentBankStatemnetImport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                loadMode();
                loadMemtype();
                loadHeader();
                loadLedger();
                loadBank();
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    //memtype
    protected void loadMemtype()
    {
        try
        {
            ddlmemtype.Items.Clear();
            ddlmemtype.Items.Add(new System.Web.UI.WebControls.ListItem("Student", "0"));
            ddlmemtype.Items.Add(new System.Web.UI.WebControls.ListItem("Staff", "1"));
            ddlmemtype.Items.Add(new System.Web.UI.WebControls.ListItem("Vendor", "2"));
            ddlmemtype.Items.Add(new System.Web.UI.WebControls.ListItem("Others", "2"));

        }
        catch { }
    }

    protected void loadBank()
    {
        try
        {
            ddlbank.Items.Clear();
            string selquery = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbank.DataSource = ds;
                ddlbank.DataTextField = "BankName";
                ddlbank.DataValueField = "BankPK";
                ddlbank.DataBind();
            }
        }
        catch { }
    }

    //mode
    protected void loadMode()
    {
        try
        {
            cblmode.Items.Clear();
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Refund", "0"));
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Excess", "1"));
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Scholarship", "2"));
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = true;
            }
            cbmode.Checked = true;
            txtmode.Text = "Mode" + "(" + cblmode.Items.Count + ")";

        }
        catch { }
    }
    protected void cbmode_changed(object sender, EventArgs e)
    {
        if (cbmode.Checked == true)
        {
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = true;
            }
            txtmode.Text = "Mode (" + cblmode.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = false;
            }
            txtmode.Text = "---Select---";
        }
    }
    protected void cblmode_selected(object sender, EventArgs e)
    {
        txtmode.Text = "---Select---";
        cbmode.Checked = false;
        int count = 0;
        for (int i = 0; i < cblmode.Items.Count; i++)
        {
            if (cblmode.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtmode.Text = "Mode (" + count + ")";
            if (count == cblmode.Items.Count)
            {
                cbmode.Checked = true;
            }
        }
    }

    public void loadHeader()
    {
        try
        {
            ddlheader.Items.Clear();
            // string selqry = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheader.DataSource = ds;
                ddlheader.DataTextField = "HeaderName";
                ddlheader.DataValueField = "HeaderPK";
                ddlheader.DataBind();
                loadLedger();
            }
        }
        catch
        {
        }
    }
    public void loadLedger()
    {
        try
        {
            ddlledger.Items.Clear();
            string hed = "";
            if (ddlheader.Items.Count > 0)
                hed = Convert.ToString(ddlheader.SelectedItem.Value);

            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "  and L.CollegeCode = " + collegecode + "  and L.HeaderFK in('" + hed + "') and LedgerMode=1  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlledger.DataSource = ds;
                ddlledger.DataTextField = "LedgerName";
                ddlledger.DataValueField = "LedgerPK";
                ddlledger.DataBind();
            }

        }
        catch
        {
        }
    }

    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadLedger();
        }
        catch { }
    }

    protected void btnimport_Click(object sender, EventArgs e)
    {
        try
        {
            importDetails();
        }
        catch { }
    }

    protected void importDetails()
    {
        try
        {
            DataSet dsl = new DataSet();
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    if (FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx"))
                    {
                        string path = Server.MapPath("~/UploadFiles/" + System.IO.Path.GetFileName(FileUpload1.FileName));
                        FileUpload1.SaveAs(path);
                        dsl.Clear();
                        dsl = Excelconvertdataset(path);
                        if (dsl.Tables.Count > 0 && dsl.Tables[0].Rows.Count > 0)
                        {
                            DatasetValues(dsl);
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Excel Should Be Correct Format";
                        }
                    }
                }
            }
        }
        catch { }
    }


    protected void DatasetValues(DataSet dsl)
    {
        try
        {
            bool save = false;
            string memtype = string.Empty;
            string fnlYr = string.Empty;
            string bankFK = string.Empty;
            string header = string.Empty;
            string ledger = string.Empty;
            string TransCode = string.Empty;
            if (ddlmemtype.Items.Count > 0)
                memtype = Convert.ToString(ddlmemtype.SelectedItem.Value);
            if (ddlheader.Items.Count > 0)
                header = Convert.ToString(ddlheader.SelectedItem.Value);
            if (ddlledger.Items.Count > 0)
                ledger = Convert.ToString(ddlledger.SelectedItem.Value);
            if (ddlbank.Items.Count > 0)
                bankFK = Convert.ToString(ddlbank.SelectedItem.Value);

            fnlYr = d2.GetFunction("select LinkValue  from InsSettings where LinkName = 'Current Financial Year' and  college_code=" + collegecode + "");
            if (fnlYr != "0")
            {
                for (int row = 0; row < dsl.Tables[0].Rows.Count; row++)
                {
                    string studno = Convert.ToString(dsl.Tables[0].Rows[row][1]);
                    string appNo = getAppno(studno);
                    double Amount = 0;
                    double.TryParse(Convert.ToString(dsl.Tables[0].Rows[row][2]), out Amount);
                    string transDate = getDate(Convert.ToString(dsl.Tables[0].Rows[row][3]));
                    string bankRefNo = Convert.ToString(dsl.Tables[0].Rows[row][4]);
                    string bankRefDate = getDate(Convert.ToString(dsl.Tables[0].Rows[row][5]));
                    if (validateDetails(appNo, Amount, transDate, memtype, header, ledger, bankRefNo, bankRefDate, bankFK))
                    {
                        for (int mem = 0; mem < cblmode.Items.Count; mem++)
                        {
                            string Mtype = string.Empty;
                            if (cblmode.Items[mem].Selected)
                            {
                                double oldAmount = 0;
                                int selVal = Convert.ToInt32(cblmode.Items[mem].Value);
                                if (selVal == 0)
                                {
                                    double.TryParse(Convert.ToString(d2.GetFunction(" select SUM(ISNULL(RefundAmount,'0'))- SUM(ISNULL(RefundAdjAmount,'0')) as Amount from FT_FeeAllot where app_no='" + appNo + "' and finyearfk='" + fnlYr + "' ")), out oldAmount);
                                    Mtype = "1";
                                    if (oldAmount != 0 && oldAmount >= Amount)
                                    {
                                        TransCode = generateReceiptNo();
                                        if (!string.IsNullOrEmpty(TransCode))
                                            save = refundDetails(appNo, Amount, transDate, TransCode, memtype, header, ledger, bankRefNo, bankRefDate, fnlYr, bankFK, Mtype);
                                    }
                                }
                                if (selVal == 1)
                                {
                                    double.TryParse(Convert.ToString(d2.GetFunction(" select sum(excessamt)-sum(adjamt) as Amount from ft_excessdet where app_no='" + appNo + "' and finyearfk='" + fnlYr + "' ")), out oldAmount);
                                    Mtype = "2";
                                    if (oldAmount != 0 && oldAmount >= Amount)
                                    {
                                        TransCode = generateReceiptNo();
                                        if (!string.IsNullOrEmpty(TransCode))
                                            save = refundDetails(appNo, Amount, transDate, TransCode, memtype, header, ledger, bankRefNo, bankRefDate, fnlYr, bankFK, Mtype);
                                    }

                                }
                                if (selVal == 2)
                                {
                                    double.TryParse(Convert.ToString(d2.GetFunction(" select SUM(ISNULL(FromGovtAmt,'0'))as Amount from FT_FeeAllot where app_no='" + appNo + "' and ISNULL(FromGovtAmt,'0')>0 ")), out oldAmount);
                                    Mtype = "3";
                                    if (oldAmount != 0 && oldAmount >= Amount)
                                    {
                                        TransCode = generateReceiptNo();
                                        if (!string.IsNullOrEmpty(TransCode))
                                            save = refundDetails(appNo, Amount, transDate, TransCode, memtype, header, ledger, bankRefNo, bankRefDate, fnlYr, bankFK, Mtype);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please Enter Valid Details";
                    }
                }
                if (save)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Import Successfully";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Financial Year";
            }
        }
        catch { }
    }

    protected string getDate(string date)
    {
        try
        {
            if (date.Contains('/'))
            {
                string dtval = date.Split(' ')[0];
                string[] dt = dtval.Split('/');
                if (dt.Length > 0)
                    date = Convert.ToString(dt[1] + "/" + dt[0] + "/" + dt[2]);
            }
            else
                date = string.Empty;
        }
        catch { }
        return date;
    }

    protected bool refundDetails(string appNo, double Amount, string transDate, string TransCode, string memtype, string header, string ledger, string bankRefNo, string bankRefDate, string fnlYr, string bankFK, string Mtype)
    {
        bool value = false;
        try
        {
            if (Mtype == "2")
                excessUpdate(appNo, Amount);

            string updQ = string.Empty;
            if (Mtype == "1")
            {
                updQ = " update ft_feeallot set RefundAdjAmount=isnull(RefundAdjAmount,'0')+'" + Amount + "' where app_no='" + appNo + "' and memtype='" + memtype + "'";
                int upds = d2.update_method_wo_parameter(updQ, "Text");
            }

            string insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,ddbankcode,DDDate,TransType,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + transDate + "','" + DateTime.Now.ToShortTimeString() + "','" + TransCode + "','" + memtype + "','" + appNo + "','" + header + "','" + ledger + "','" + 0 + "','" + Amount + " ','0','5','" + bankRefNo + "','" + bankFK + "','" + bankRefDate + "','" + 2 + "','" + usercode + "','" + fnlYr + "','0','0','','0','','','','0','','','0','','1','','','0','','','0','','','0','','','','0')";
            int save = d2.update_method_wo_parameter(insqry, "Text");

            //bank transaction
            insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK) values ('" + transDate + "','" + DateTime.Now.ToShortTimeString() + "','" + bankFK + "','5','" + TransCode + "','1','1','0','" + Amount + "','0','" + fnlYr + "')";
            int savebk = d2.update_method_wo_parameter(insqry, "Text");
            if (save > 0)
            {
                string uprec = "update FM_FinCodeSettings set VouchStNo=" + ViewState["receno"] + "+1 where IsHeader=0 and FinYearFK='" + fnlYr + "' and collegecode ='" + collegecode + "' and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                int uprecno = d2.update_method_wo_parameter(uprec, "Text");
                value = true;
            }

        }
        catch { }
        return value;
    }


    private bool validateDetails(string appNo, double Amount, string transDate, string memtype, string header, string ledger, string bankRefNo, string bankRefDate, string bankFK)
    {
        bool save = false;
        try
        {
            if (appNo != "0" && Amount != 0 && !string.IsNullOrEmpty(transDate) && !string.IsNullOrEmpty(memtype) && !string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(ledger) && !string.IsNullOrEmpty(bankRefNo) && !string.IsNullOrEmpty(bankRefDate) && !string.IsNullOrEmpty(bankFK))
            {
                save = true;
            }
        }
        catch { }
        return save;
    }

    protected void excessUpdate(string appNo, double Amount)
    {
        try
        {
            DataSet dsex = new DataSet();
            string updQ = "update FT_ExcessDet set AdjAmt=ISNULL(AdjAmt,'0') +'" + Amount + "',BalanceAmt=isnull(BalanceAmt,'0')-'" + Amount + "' where App_No='" + appNo + "' and ExcessType=1";
            int upadj = d2.update_method_wo_parameter(updQ, "Text");
            string exPK = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + appNo + "' and excessType=1");
            string select = "select (ISNULL(ExcessAmt,'0')-ISNULL(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + exPK + "'";
            dsex.Clear();
            dsex = d2.select_method_wo_parameter(select, "Text");
            if (dsex.Tables[0].Rows.Count > 0)
            {
                for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                {
                    string headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                    string ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                    double oldexldg = 0;
                    double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out oldexldg);
                    string updateexcess = string.Empty;
                    int updatex = 0;
                    if (oldexldg != 0)
                    {
                        if (oldexldg >= Amount)
                        {
                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + Amount + "',,BalanceAmt=isnull(BalanceAmt,'0')-'" + Amount + "' where ExcessDetFK ='" + exPK + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                            Amount = 0;
                        }
                        if (oldexldg <= Amount)
                        {
                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + oldexldg + "',BalanceAmt=isnull(BalanceAmt,'0')-'" + oldexldg + "' where ExcessDetFK ='" + exPK + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                            Amount = Amount - oldexldg;
                        }
                    }

                }
            }
        }
        catch { }
    }


    protected string getAppno(string rollno)
    {
        string appno = string.Empty;
        try
        {
            appno = d2.GetFunction(" select app_no from registration where roll_no='" + rollno + "' and college_code='" + collegecode + "'");
            if (appno == "0")
                appno = d2.GetFunction(" select app_no from registration where Reg_no='" + rollno + "' and college_code='" + collegecode + "'");
            if (appno == "0")
                appno = d2.GetFunction(" select app_no from registration where roll_admit='" + rollno + "' and college_code='" + collegecode + "'");
        }
        catch { }
        return appno;
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = "";

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();
            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();

            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
            }
        }
        catch
        {
        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }

    //help

    protected void lnkhelp_Click(object sender, EventArgs e)
    {
        downloadhelp_excel("StudenBankStatementImport");
    }
    protected void downloadhelp_excel(string filename)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = filename;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    string szPath = appPath + "/UploadFiles/";
                    string szFile = print + ".xls";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            //Browsefile_div.Visible = false;
            //lbl_alerterror.Visible = true;
            //lbl_alerterror.Text = ex.Message;
            //alertmessage.Visible = true;
        }
    }

    //voucher no
    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string fincyr = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode + "");
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            //string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
            string secondreciptqurey = "SELECT VouchStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                //string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                string acronymquery = d2.GetFunction("SELECT VouchAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                //int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)"));

                int size = Convert.ToInt32(d2.GetFunction("SELECT  VouchSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                ViewState["receno"] = Convert.ToString(recenoString);
                //lstrcpt.Text = Convert.ToString(receno);
            }

            return recno;
        }
        catch { return recno; }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

}