using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UpdateFeeallotanddailytrans : System.Web.UI.Page
{

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    static int isHeaderwise = 0;
    string usercode = "30";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_update_click(object sender, EventArgs e)
    {
        string qry = string.Empty;


        qry = "select r.stud_name,r.roll_no,r.Reg_No,r.App_No,isnull(f.Debit,'0') as debit ,convert(varchar(100), f.app_no)as appno,convert(varchar(100), f.feecategory)as feecategory,convert(varchar(100),f.ledgerfk)as ledgerfk,convert(varchar(100),f.headerfk)as headerfk,TransDate,paymode,finyearfk,memtype from ft_findailytransaction f,registration r where f.app_no=r.app_no and convert(varchar(100), f.app_no)+'-'+convert(varchar(100), f.feecategory)+'-'+ convert(varchar(100),f.ledgerfk)+'-'+convert(varchar(100),f.headerfk) not in(select  convert(varchar(100), f.app_no)+'-'+convert(varchar(100), f.feecategory)+'-'+ convert(varchar(100),f.ledgerfk)+'-'+convert(varchar(100),f.headerfk) from ft_findailytransaction t inner join ft_feeallot f on f.app_no=t.app_no and f.ledgerfk=t.ledgerfk and f.headerfk=t.headerfk and f.feecategory=t.feecategory )";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qry, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                string header = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                string appno = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                string ledger = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]);
                string sem = Convert.ToString(ds.Tables[0].Rows[i]["feecategory"]);
                string amt = Convert.ToString(ds.Tables[0].Rows[i]["debit"]);
                string transdate = Convert.ToString(ds.Tables[0].Rows[i]["transdate"]);
                string paymode = Convert.ToString(ds.Tables[0].Rows[i]["paymode"]);
                string finyear = Convert.ToString(ds.Tables[0].Rows[i]["finyearfk"]);
                string memtype = Convert.ToString(ds.Tables[0].Rows[i]["memtype"]);

                string insertqry = "insert into ft_feeallot(allotdate,app_no,headerfk,ledgerfk,paymode,feeamount,totalamount,feecategory,paidamount,balamount,finyearfk,memtype)values('" + transdate + "','" + appno + "','" + header + "','" + ledger + "','" + paymode + "','" + amt + "','" + amt + "','" + sem + "','" + amt + "',0,'" + finyear + "','" + memtype + "')";
                int Up = d2.update_method_wo_parameter(insertqry, "Text");
                if (Up > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                }


            }


        }

    }
    protected void update_click(object sender, EventArgs e)
    {
        string q = string.Empty;
        q = "select * from FT_FeeAllot where PaidAmount>TotalAmount";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string header = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                string appno = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                string ledger = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]);
                string sem = Convert.ToString(ds.Tables[0].Rows[i]["feecategory"]);
                string feeamount = Convert.ToString(ds.Tables[0].Rows[i]["feeamount"]);
                string totalamount = Convert.ToString(ds.Tables[0].Rows[i]["totalamount"]);
                string paidamount = Convert.ToString(ds.Tables[0].Rows[i]["paidamount"]);
                string balamount = Convert.ToString(ds.Tables[0].Rows[i]["balamount"]);
                string updateqry = "update ft_feeallot set feeamount='" + paidamount + "',totalamount='" + paidamount + "',balamount=0 where app_no='" + appno + "' and headerfk='" + header + "' and ledgerfk='" + ledger + "'";
                int Up = d2.update_method_wo_parameter(updateqry, "Text");
                if (Up > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                }
            }
        }
    }
    protected void update1_click(object sender, EventArgs e)
    {
        string query = string.Empty;
        bool receipt = false;
        DataSet dsvalue = new DataSet();
        string lastRecptNo = string.Empty;
        query = "select * from ft_Feeallot where allotdate='07/04/2018' and app_no in(62354,55505)";
        dsvalue.Clear();
        dsvalue = d2.select_method_wo_parameter(query, "Text");
        if (dsvalue.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsvalue.Tables[0].Rows.Count; i++)
            {
               
                lastRecptNo = lbllastrcpt.Text;
                string header = Convert.ToString(dsvalue.Tables[0].Rows[i]["HeaderFK"]);
                string appno = Convert.ToString(dsvalue.Tables[0].Rows[i]["app_no"]);
                string ledger = Convert.ToString(dsvalue.Tables[0].Rows[i]["LedgerFK"]);
                string sem = Convert.ToString(dsvalue.Tables[0].Rows[i]["feecategory"]);
                string paidamount = Convert.ToString(dsvalue.Tables[0].Rows[i]["paidamount"]);
                string paymode = "5";
                string transdate="2018-07-09";
                string iscollected = "1";
                string collecteddate = DateTime.Now.ToString("MM/dd/yyyy");
               
                string collegecode = "13";
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                string transcode = generateReceiptNo(finYearid, collegecode);
                string insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate) VALUES('" + transdate + "','" + DateTime.Now.ToLongTimeString() + "','" + transcode + "', 1, " + appno + ", " + ledger + ", " + header + ", " + sem + ", 0, " + (paidamount) + ", " + paymode + ", 1, '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'3','" + iscollected + "','" + collecteddate + "','" + iscollected + "','" + collecteddate + "')";
                d2.update_method_wo_parameter(insquery, "text");
                receipt = true;
               
            }
        }
    }

    public string generateReceiptNo(string finYearid, string collegecode)
    {
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo(finYearid, collegecode);
        }
        else
        {
            return getHeaderwiseReceiptNo(finYearid, collegecode);
        }
    }
    private string getCommonReceiptNo(string finYearid, string collegecode)
    {
        string recno = string.Empty;
        // lblaccid.Text = "";
        // lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            //  string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + ")";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + ")");
                recacr = acronymquery;


                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + ")"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                lbllastrcpt.Text = Convert.ToString(receno);
            }
            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private string getHeaderwiseReceiptNo(string finYearid, string collegecode)
    {
        string recno = string.Empty;
        // lblaccid.Text = "";
        //  lstrcpt.Text = "";
        string accountid = "";
        // lblaccid.Text = accountid;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            //  string isheaderFk = GetSelectedItemsValue(cbl_grpheader);
            string isheaderFk = string.Empty;
            //   string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode + " and FinyearFK=" + finYearid + "", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1)//&& rbl_headerselect.SelectedIndex == 1
            {
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]) + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode + " ";
                DataSet dsrecYr = new DataSet();
                dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
                if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
                {
                    recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptStNo"]);
                    if (recnoprev != "")
                    {
                        int recno_cur = Convert.ToInt32(recnoprev);
                        receno = recno_cur;
                    }
                    recacr = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptAcr"]);

                    int size = Convert.ToInt32(dsrecYr.Tables[0].Rows[0]["Rcptsize"]);

                    string recenoString = receno.ToString();

                    if (size != recenoString.Length && size > recenoString.Length)
                    {
                        while (size != recenoString.Length)
                        {
                            recenoString = "0" + recenoString;
                        }
                    }
                    recno = recacr + recenoString;

                    //lstrcpt.Text = Convert.ToString(receno);
                }
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
}