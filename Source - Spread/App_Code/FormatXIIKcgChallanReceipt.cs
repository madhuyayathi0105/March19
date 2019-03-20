using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for FormatXIIKcgChallanReceipt
/// </summary>
public class FormatXIIKcgChallanReceipt : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
	public FormatXIIKcgChallanReceipt()
	{

	}
    #region KCG Multiple Receipt
    public StringBuilder returnHtmlStringMulReceiptKCG(out bool createPDFOK, out bool InsertUpdateOK, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, RadioButton rdo_receipt, RadioButton rdo_sngle, Label lbltype, byte PayMode, byte memtype, string checkDDno, string dtchkdd, string newbankcode, string branch, string finYearid, string Appno, string Rollno, string Collegecode, string Usercode, byte GpHdrType, string Studname, string RecptDt, string RcptTime, string RecptNo, string Cursem, string DegString, string DeptString, string Narration)
    {
        string appno = string.Empty;
        string rollno = string.Empty;
        string collegecode = string.Empty;
        string usercode = string.Empty;
        byte rcptType = 0;
        string studname = string.Empty;
        string recptDt = string.Empty;
        string rcptTime = string.Empty;
        string recptNo = string.Empty;
        string cursem = string.Empty;
        string degString = string.Empty;
        string deptString = string.Empty;
        string narration = string.Empty;

        appno = Appno;
        rollno = Rollno;
        collegecode = Collegecode;
        usercode = Usercode;
        rcptType = GpHdrType;
        studname = Studname;
        recptDt = RecptDt;
        recptNo = RecptNo;
        cursem = Cursem;
        degString = DegString;
        deptString = DeptString;
        rcptTime = RcptTime;
        narration = Narration;


        createPDFOK = false;
        InsertUpdateOK = false;
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            #region Receipt Header

            sbHtml.Append("<div style='padding-left:5px;height:500px;'><table cellpadding='0' cellspacing='0' text-align:center; width: 700px;' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:700px; height:20px; padding-left:5px;padding-top:60px; ' class='classReg12'><tr><td colspan='3' style='text-align:right;'>" + recptNo + "</td></tr></table><table style='width:700px; height:160px; padding-left:5px;padding-top:40px; ' class='classReg12'><tr><td colspan='3'><br><center>" + studname.ToUpper() + "</center><br/></td></tr><tr><td style='width:300px; text-align:right; '>" + "" + rollno + "</td><td style='width:300px;text-align:right;'>" + "" + romanLetter(returnYearforSem(cursem)) + " Year" + "</td><td style='width:300px; text-align:right;'>" + "" + recptDt + "</td></tr><tr><td style='width:200px; text-align:center;'><br>" + degString + "</td><td style='width:500px;margin-left:10px; text-align:center;' colspan='2'><br>" + deptString + "</td></tr></table>");

            recptDt = recptDt.Split('/')[1] + "/" + recptDt.Split('/')[0] + "/" + recptDt.Split('/')[2];
            #endregion

            #region Receipt Body
            sbHtml.Append("<div style='width:700px; height:218px; padding-top:30px;padding-left:-50px; '><table  class='classReg12'>");
            string semyear = "";
            if (ddl_semrcpt.Items.Count > 0)
            {
                semyear = Convert.ToString(ddl_semrcpt.SelectedItem.Value);
            }
            int rows = 0;
            string selectQuery = "";
            List<string> lstgrpHeaderValu = new List<string>();
            List<string> lstgrpHeaderName = new List<string>();

            lstgrpHeaderValu = GetSelectedItemsValueList(cbl_grpheader);
            lstgrpHeaderName = GetSelectedItemsTextList(cbl_grpheader);
            #region To Count Rows
            for (int j = 0; j < lstgrpHeaderValu.Count; j++)
            {
                string BalNOT0 = string.Empty;
                #region Load Ledgers
                string headercode = "";

                headercode = Convert.ToString(lstgrpHeaderValu[j]);

                selectQuery = " SELECT isnull(sum(BalAmount),0) as BalAmount FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and l.LedgerMode=0  and T.TextCode in('" + semyear + "') AND A.App_No = " + appno + "";
                if (rbl_headerselect.SelectedIndex == 0)
                {
                    //Group Header
                    selectQuery = " SELECT isnull(sum(BalAmount),0) as BalAmount FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + headercode + "') and T.TextCode in('" + semyear + "') ";
                    if (rdo_receipt.Checked || rdo_sngle.Checked)
                    {
                        selectQuery += " AND A.App_No = " + appno + " ";
                    }

                    if (lbltype.Text != "")
                    {
                        selectQuery += "  and Stream ='" + lbltype.Text.Trim() + "' ";
                    }

                }
                else if (rbl_headerselect.SelectedIndex == 1)
                {
                    //Header
                    selectQuery += "  and A.HeaderFK in (" + headercode + ") ";
                }
                else
                {
                    //Ledger
                    selectQuery += "  and A.LedgerFK  in (" + headercode + ")  ";
                }


                if (selectQuery.Trim() != "")
                {
                    BalNOT0 = d2.GetFunction(selectQuery);
                    double balChk = 0;
                    double.TryParse(BalNOT0, out balChk);
                    if (balChk > 0)
                    {
                        rows++;
                    }
                }
                #endregion

            }

            #endregion

            int sno = 0;
            int indx = 0;
            double totalamt = 0;
            double balanamt = 0;
            double curpaid = 0;
            double paidamount = 0;
            double deductionamt = 0;

            #region Insert Process New

            //For Every Selected Headers

            for (int j = 0; j < lstgrpHeaderValu.Count; j++)
            {
                string disphdr = string.Empty;
                double allotamt0 = 0;
                double deductAmt0 = 0;
                double totalAmt0 = 0;
                double paidAmt0 = 0;
                double balAmt0 = 0;
                double creditAmt0 = 0;
                double alreadyPaid = 0;

                #region Load Ledgers

                string headercode = "";
                disphdr = Convert.ToString(lstgrpHeaderName[j]);
                headercode = Convert.ToString(lstgrpHeaderValu[j]);

                selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,priority,LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0) as   DeductAmount,isnull(TotalAmount,0) as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount, isnull(BalAmount,0) as BalAmount,TextVal,TextCode FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and l.LedgerMode=0  and T.TextCode in('" + semyear + "') AND A.App_No = " + appno + "  and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";

                if (rbl_headerselect.SelectedIndex == 0)
                {
                    //Group Header
                    selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,priority,LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)   as DeductAmount ,isnull(TotalAmount,0)   as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount,TextVal,TextCode,ChlGroupHeader FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + headercode + "') and T.TextCode in('" + semyear + "')  and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";
                    if (rdo_receipt.Checked || rdo_sngle.Checked)
                    {
                        selectQuery += " AND A.App_No = " + appno + " ";
                    }

                    if (lbltype.Text != "")
                    {
                        selectQuery += "  and Stream ='" + lbltype.Text.Trim() + "' ";
                    }

                }
                else if (rbl_headerselect.SelectedIndex == 1)
                {
                    //Header
                    selectQuery += "  and A.HeaderFK in (" + headercode + ") ";
                }
                else
                {
                    //Ledger
                    selectQuery += "  and A.LedgerFK  in (" + headercode + ")  ";
                }

                selectQuery += "  order by case when priority is null then 1 else 0 end, priority ";

                #endregion

                DataSet dsLedgers = new DataSet();
                dsLedgers = d2.select_method_wo_parameter(selectQuery, "Text");
                if (dsLedgers.Tables.Count > 0 && dsLedgers.Tables[0].Rows.Count > 0)
                {
                    for (int lgri = 0; lgri < dsLedgers.Tables[0].Rows.Count; lgri++)
                    {
                        string feecat1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["TextCode"]);
                        string headerfk1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["HeaderFK"]);
                        string ledgerfk1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["LedgerFK"]);
                        double feeamt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["FeeAmount"]);
                        double deductAmt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["DeductAmount"]);
                        double totalamt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["TotalAmount"]);
                        double paidAmt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["PaidAmount"]);
                        double balAmt1 = totalamt1 - paidAmt1;
                        double creditAmt1 = balAmt1;
                        alreadyPaid += paidAmt1;
                        creditAmt0 += creditAmt1;
                        totalAmt0 += totalamt1;
                        balAmt0 += balAmt1 - creditAmt1;
                        paidAmt0 += creditAmt1;
                        deductAmt0 += deductAmt1;

                        #region Ledger Insert Update

                        if (creditAmt1 > 0)
                        {
                            string iscollected = "0";
                            string collecteddate = "";
                            if (PayMode == 1)
                            {
                                iscollected = "1";
                                collecteddate = (recptDt).ToString();
                            }
                            string insertDebit = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate) VALUES('" + recptDt + "','" + DateTime.Now.ToLongTimeString() + "','" + recptNo + "', " + memtype + ", " + appno + ", " + ledgerfk1 + ", " + headerfk1 + ", " + feecat1 + ", 0, " + creditAmt1 + ", " + PayMode + ", '" + checkDDno + "', '" + dtchkdd + "', '" + newbankcode + "','" + branch + "', 1, '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'" + rcptType + "','" + iscollected + "','" + collecteddate + "')";

                            d2.update_method_wo_parameter(insertDebit, "Text");

                            //Update process

                            string selectquery = " select  isnull(TotalAmount,0) as TotalAmount,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount  from FT_FeeAllot where App_No =" + appno + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "'";

                            DataSet dsPrevAMount = new DataSet();
                            dsPrevAMount = d2.select_method_wo_parameter(selectquery, "Text");
                            if (dsPrevAMount.Tables.Count > 0)
                            {
                                if (dsPrevAMount.Tables[0].Rows.Count > 0)
                                {
                                    double total = 0;
                                    double paidamt = 0;
                                    double balamt = 0;

                                    total = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["TotalAmount"]);

                                    if (total > 0)
                                    {
                                        paidamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["PaidAmount"]);
                                        balamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["BalAmount"]);

                                        balamt = (total - paidamt);

                                        string updatequery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +" + creditAmt1 + " ,BalAmount =" + (balamt - creditAmt1) + "  where App_No =" + appno + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "'";
                                        d2.update_method_wo_parameter(updatequery, "Text");

                                        InsertUpdateOK = true;
                                    }

                                }
                            }
                        }

                        #endregion

                    }
                }

                if (creditAmt0 > 0)
                {
                    sno++;

                    totalamt += Convert.ToDouble(totalAmt0);
                    balanamt += Convert.ToDouble(balAmt0);
                    curpaid += Convert.ToDouble(paidAmt0);
                    deductionamt += Convert.ToDouble(deductAmt0);

                    indx++;
                    createPDFOK = true;

                    sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>" + disphdr + "</td><td style='width:150px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(creditAmt0) + "</td></tr>");
                }

            }
            #endregion

            sbHtml.Append("</table></div>");
            double totalamount = curpaid;

            sbHtml.Append("<div><table  class='classReg12'><tr><td style='width:30px;text-align:right;'></td><td style='width:700px;text-indent:20;'>" + narration + "</td><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:470px; font-size:12px;'>(" + DecimalToWords((decimal)totalamount) + " Rupees Only.)</td></table><br/><br/><table  class='classReg12'></tr><tr><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:270px;'>&nbsp;</td><td style='width:210px;text-align:right;'>" + returnIntegerPart(totalamount) + "</td><td style='width:60px;text-align:right;'>" + returnDecimalPart(totalamount) + "</td></tr></table></div>");

            sbHtml.Append("</td></tr></table></div>");
            #endregion
        }
        catch { sbHtml.Clear(); }
        return sbHtml;
    }
    #endregion
    #region Original Receipt for kcg added by sudhagar 11-07-2016
    public StringBuilder kcgprint(string Appno, string Rollno, string Collegecode, string Usercode, byte GpHdrType, string Studname, string RecptDt, string RcptTime, string RecptNo, string Cursem, string DegString, string DeptString, string Narration,string remarks, GridView grid_Details, CheckBox cb_exfees, CheckBox cb_govt, CheckBox cb_CautionDep, string collegecode, string usercode, TextBox txt_examt)
    {
        string appno = "";
        string rollno = "";
        byte rcptType;
        string studname = "";
        string recptDt = "";
        string recptNo = "";
        string cursem = "";
        string degString = "";
        string deptString = "";
        string rcptTime = "";
        string narration = "";
        string nar = "";
        appno = Appno;
        rollno = Rollno;
        collegecode = Collegecode;
        usercode = Usercode;
        rcptType = GpHdrType;
        studname = Studname;
        recptDt = RecptDt;
        recptNo = RecptNo;
        cursem = Cursem;
        degString = DegString;
        deptString = DeptString;
        rcptTime = RcptTime;
        narration = Narration;
        nar = remarks;
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            //            #region Receipt Header
            //            //763.5
            sbHtml.Append("<div style='padding-left:5px;height:500px;'><table cellpadding='0' cellspacing='0' text-align:center; width: 700px;' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:700px; height:20px; padding-left:5px;padding-top:60px; ' class='classReg12'><tr><td colspan='3' style='text-align:right;'>" + recptNo + "</td></tr></table><table style='width:700px; height:160px; padding-left:5px;padding-top:40px; ' class='classReg12'><tr><td colspan='3'><br><center>" + studname.ToUpper() + "</center><br/></td></tr><tr><td style='width:300px; text-align:right; '>" + "" + rollno + "</td><td style='width:300px;text-align:right;'>" + "" + romanLetter(returnYearforSem(cursem)) + " Year" + "</td><td style='width:300px; text-align:right;'>" + "" + recptDt + "</td></tr><tr><td style='width:200px; text-align:center;'><br>" + degString + "</td><td style='width:500px;margin-left:10px; text-align:center;' colspan='2'><br>" + deptString + "</td></tr></table>");


            #region Receipt Body

            int rows = 0;
            foreach (GridViewRow row in grid_Details.Rows)
            {
                CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                if (!chkOkPay.Checked)
                    continue;
                TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                double creditamt = 0;

                if (txtTobePaidamt.Text != "")
                {
                    creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                    TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                    double exgridamt = 0;
                    if (cb_exfees.Checked)
                    {
                        double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                    }
                    creditamt += exgridamt;
                    TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                    double gvtamt = 0;
                    if (cb_govt.Checked)
                    {
                        double.TryParse(txtScholAmt.Text, out gvtamt);
                    }
                    creditamt += gvtamt;
                    TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                    double curCautamt = 0;
                    if (cb_CautionDep.Checked)
                    {
                        double.TryParse(txtCautAmt.Text, out curCautamt);
                    }
                    creditamt += curCautamt;
                    if (creditamt > 0)
                    {
                        rows++;
                    }
                }
            }

            sbHtml.Append("<div style='width:700px; height:218px; padding-top:30px;padding-left:-50px; '><table  class='classReg12'>");

            #region feedata
            int sno = 0;
            int indx = 0;
            double totalamt = 0;
            double balanamt = 0;
            double curpaid = 0;
            double deductionamt = 0;
            foreach (GridViewRow row in grid_Details.Rows)
            {
                CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                if (!chkOkPay.Checked)
                    continue;

                TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                TextBox txtdeductamt = (TextBox)row.FindControl("txt_deduct_amt");

                Label lblFeeCategory = (Label)row.FindControl("lbl_feetype");
                Label lblsem = (Label)row.FindControl("lbl_textval");

                double creditamt = 0;

                if (txtTobePaidamt.Text != "")
                {
                    creditamt = Convert.ToDouble
(txtTobePaidamt.Text);
                    TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                    double exgridamt = 0;
                    if (cb_exfees.Checked)
                    {
                        double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                    }
                    creditamt += exgridamt;
                    TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                    double gvtamt = 0;
                    if (cb_govt.Checked)
                    {
                        double.TryParse(txtScholAmt.Text, out gvtamt);
                    }
                    creditamt += gvtamt;
                    TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                    double curCautamt = 0;
                    if (cb_CautionDep.Checked)
                    {
                        double.TryParse(txtCautAmt.Text, out curCautamt);
                    }
                    creditamt += curCautamt;
                }

                if (creditamt > 0)
                {
                    sno++;

                    totalamt += Convert.ToDouble(txtTotalamt.Text);
                    balanamt += Convert.ToDouble(txtBalamt.Text);
                    curpaid += creditamt;
                    //balanamt += Convert.ToDouble(txtTotalamt.Text) + Convert.ToDouble(txtTobePaidamt.Text) - creditamt;
                    deductionamt += Convert.ToDouble(txtdeductamt.Text);
                    indx++;
                    if (lblsem.Text == "0")
                    {
                        sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>" + lblFeeCategory.Text + "</td><td style='width:150px;text-align:right;'>" + returnIntegerPart(creditamt) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(creditamt) + "</td></tr>");//+ "-" + "(" + lblsem.Text + ")"
                    }
                    else
                    {
                        sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>" + lblFeeCategory.Text + "-" + "(" + lblsem.Text + ")" + "</td><td style='width:150px;text-align:right;'>" + returnIntegerPart(creditamt) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(creditamt) + "</td></tr>");
                    }
                }
            }

            if (sno != 0)
            {
                double exAmtD = 0; double.TryParse(txt_examt.Text, out exAmtD);
                if (exAmtD > 0)
                {
                    ++sno;
                    sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>Excess Amount </td><td style='width:150px;text-align:right;'>" + returnIntegerPart(exAmtD) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(exAmtD) + "</td></tr>");
                    curpaid += exAmtD;
                }
            }
            #endregion
            sbHtml.Append("</table></div>");

            double totalamount = curpaid;
            sbHtml.Append("<div><table  class='classReg12'><tr><td style='width:30px;text-align:right;'></td><td style='width:700px;text-indent:20;'>" + narration + "</td><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:700px;text-indent:10;'>" + nar + "</td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:470px; font-size:12px;'>(" + DecimalToWords((decimal)totalamount) + " Rupees Only.)</td></table><br/><br/><table  class='classReg12'></tr><tr><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:270px;'>&nbsp;</td><td style='width:210px;text-align:right;'>" + returnIntegerPart(totalamount) + "</td><td style='width:60px;text-align:right;'>" + returnDecimalPart(totalamount) + "</td></tr></table></div>");

            sbHtml.Append("</td></tr></table></div>");
            #endregion
        }
        catch { sbHtml.Clear(); }
        return sbHtml;


    }
    #endregion
}