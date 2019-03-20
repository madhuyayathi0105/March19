using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for GnanamaniChallanReceipt
/// </summary>
public class GnanamaniChallanReceipt : ReuasableMethods
{
    private string appno = string.Empty;
    private string rollno = string.Empty;
    private string collegecode = string.Empty;
    private string usercode = string.Empty;
    private byte rcptType = 0;
    private string studname = string.Empty;
    private string recptDt = string.Empty;
    private string rcptTime = string.Empty;
    private string recptNo = string.Empty;
    private string cursem = string.Empty;
    private string degString = string.Empty;
    private string deptString = string.Empty;

    DAccess2 d2 = new DAccess2();

	public GnanamaniChallanReceipt()
	{
	}
    public GnanamaniChallanReceipt(string Appno, string Rollno, string Collegecode, string Usercode, byte GpHdrType, string Studname,string RecptDt,string RcptTime,string RecptNo,string Cursem,string DegString,string DeptString)
    {
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
    }
    /// <summary>
    /// Returns Html For Duplicate Receipt
    /// </summary>
    /// <param name="createPDFOK">Pass variable to check the Pdf creation</param>
    /// <returns></returns>
    public StringBuilder returnHtmlString(out bool createPDFOK)
    {
        createPDFOK = false;
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            #region Receipt Header
           
            sbHtml.Append("<div style='padding-left:5px;height: 480px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 473px; padding-top:100px; ' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:470px; height:90px;padding-bottom:15px;' class='classBold10' cellpadding='2'><tr><td colspan='2' style='width:400px;'><span style='padding-left:50px;'>" + recptNo + "</span></td><td  style='width:60px;text-align:right;'><span style='padding-left:50px;'>" + recptDt + "</span></td></tr><td colspan='3'  style='width:460px;' ><span style='padding-left:50px;'>" + studname.ToUpper() + "</span></td></tr> <tr><td  style='width:160px;'><span style='padding-left:50px;text-align:center;'>" + degString + "</span></td><td  style='width:160px;'><span style='padding-left:50px;'>" + romanLetter(returnYearforSem(cursem)) + "</span></td><td  style='width:140px;text-align:right;'><span style='padding-left:50px;'>" + rollno + "</span></td></tr></table>");
            #endregion

            #region Receipt Body

            sbHtml.Append("<div style='width:470px; height:189px; padding-top:3px; '><table  class='classBold10'>");
            int rows = 0;

            int sno = 0;
            int indx = 0;
            double totalamt = 0;
            double balanamt = 0;
            double curpaid = 0;
            double deductionamt = 0;
            // double paidamount = 0;

            string selHeadersQ = string.Empty;
            DataSet dsHeaders = new DataSet();

            if (rcptType == 1 || rcptType == 2)
            {
                string StudStream = string.Empty;

                DataSet dsStr = new DataSet();
                dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + appno + "", "Text");
                if (dsStr.Tables.Count > 0)
                {
                    if (dsStr.Tables[0].Rows.Count > 0)
                    {
                        StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                    }
                }

                selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + appno + "'  ";
                if (StudStream != "")
                {
                    selHeadersQ += " and f.stream='" + StudStream + "' ";
                }
                selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
            }
            else if (rcptType == 3)
            {
                selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + appno + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
            }
            else if (rcptType == 4)
            {
                selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + appno + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
            }

            if (selHeadersQ != string.Empty)
            {
                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                dsHeaders.Clear();
                dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                if (dsHeaders.Tables.Count > 0)
                {
                    if (dsHeaders.Tables[0].Rows.Count > 0)
                    {
                        rows += dsHeaders.Tables[0].Rows.Count;

                        for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                        {
                            string disphdr = string.Empty;
                            double allotamt0 = 0;
                            double deductAmt0 = 0;
                            double totalAmt0 = 0;
                            double paidAmt0 = 0;
                            double balAmt0 = 0;
                            double creditAmt0 = 0;

                            creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                            totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                            //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                            //paidAmt0 = totalAmt0 - balAmt0;
                            deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                            disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                            string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                            string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                            string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appno + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)<>1  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                            #region Monthwise
                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                            string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                            int monWisemon = 0;
                            int monWiseYea = 0;
                            string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                            string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                            int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                            int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                            if (monWisemon > 0 && monWiseYea > 0)
                            {
                                string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                DataSet dsMonwise = new DataSet();
                                dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                {
                                    totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                    disphdr += "-" + returnMonthName(monWisemon) + "-" + monWiseYea;
                                }
                            }
                            #endregion

                            balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                            feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode + "");
                            sno++;

                            totalamt += Convert.ToDouble(totalAmt0);
                            balanamt += Convert.ToDouble(balAmt0);
                            curpaid += Convert.ToDouble(creditAmt0);

                            deductionamt += Convert.ToDouble(deductAmt0);

                            indx++;
                            createPDFOK = true;
                            sbHtml.Append("<tr><td style='width:20px;text-align:right;'>" + sno + "</td><td style='width:380px;'>" + disphdr + "</td><td style='width:60px;text-align:right;'>" + returnIntegerPart(creditAmt0) +"."+ returnDecimalPart(creditAmt0) + "</td></tr>");
                        }

                        sbHtml.Append("</table></div>");

                        double totalamount = curpaid;

                        sbHtml.Append("<div style='width:470px; height:87px; padding-top:3px; '><table  class='classBold10'><tr><td style='width:20px;text-align:right;'></td><td style='width:380px;'></td><td style='width:60px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td></tr><tr><td Colspan='3'><span style='padding-left:50px;'> "+DecimalToWords((decimal)totalamount)+" Only</span></td></tr></table></div>");
                    }
                }
            }

            #endregion

            sbHtml.Append("</td></tr></table></div>");
        }
        catch { sbHtml.Clear(); }
        return sbHtml;
    }

    /// <summary>
    /// Returns Html For Original Receipt - Single
    /// </summary>
    /// <returns></returns>
    public StringBuilder returnHtmlStringReceipt(GridView grid_Details, CheckBox cb_exfees, CheckBox cb_govt, CheckBox cb_CautionDep)
    {
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            #region Receipt Header
            sbHtml.Append("<div style='padding-left:5px;height: 480px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 473px; padding-top:100px; ' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:470px; height:90px;padding-bottom:15px;' class='classBold10' cellpadding='2'><tr><td colspan='2' style='width:400px;'><span style='padding-left:50px;'>" + recptNo + "</span></td><td  style='width:60px;text-align:right;'><span style='padding-left:50px;'>" + recptDt + "</span></td></tr><td colspan='3'  style='width:460px;' ><span style='padding-left:50px;'>" + studname.ToUpper() + "</span></td></tr> <tr><td  style='width:160px;'><span style='padding-left:50px;text-align:center;'>" + degString + "</span></td><td  style='width:160px;'><span style='padding-left:50px;'>" + romanLetter(returnYearforSem(cursem)) + "</span></td><td  style='width:140px;text-align:right;'><span style='padding-left:50px;'>" + rollno + "</span></td></tr></table>");
            #endregion

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

            sbHtml.Append("<div style='width:470px; height:189px; padding-top:3px; '><table  class='classBold10'>");

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

                    sbHtml.Append("<tr><td style='width:20px;text-align:right;'>" + sno + "</td><td style='width:380px;'>" + lblFeeCategory.Text + "</td><td style='width:60px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td></tr>");
                }
            }

            #endregion

            sbHtml.Append("</table></div>");
            double totalamount = curpaid;
            sbHtml.Append("<div style='width:470px; height:87px; padding-top:3px; '><table  class='classBold10'><tr><td style='width:20px;text-align:right;'></td><td style='width:380px;'></td><td style='width:60px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td></tr><tr><td Colspan='3'><span style='padding-left:50px;'> " + DecimalToWords((decimal)totalamount) + " Only</span></td></tr></table></div>");

            sbHtml.Append("</td></tr></table></div>");
            #endregion
        }
        catch { sbHtml.Clear(); }
        return sbHtml;
    }

    /// <summary>
    /// Returns Html For Original Receipt - Multiple
    /// </summary>
    /// <returns></returns>
    public StringBuilder returnHtmlStringMulReceipt(out bool createPDFOK, out bool InsertUpdateOK, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, RadioButton rdo_receipt, RadioButton rdo_sngle, Label lbltype, byte PayMode, byte memtype, string checkDDno, string dtchkdd, string newbankcode, string branch, string finYearid)
    {
        createPDFOK = false;
        InsertUpdateOK = false;
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            #region Receipt Header

            sbHtml.Append("<div style='padding-left:5px;height: 480px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 473px; padding-top:100px; ' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:470px; height:90px;padding-bottom:15px;' class='classBold10' cellpadding='2'><tr><td colspan='2' style='width:400px;'><span style='padding-left:50px;'>" + recptNo + "</span></td><td  style='width:60px;text-align:right;'><span style='padding-left:50px;'>" + recptDt + "</span></td></tr><td colspan='3'  style='width:460px;' ><span style='padding-left:50px;'>" + studname.ToUpper() + "</span></td></tr> <tr><td  style='width:160px;'><span style='padding-left:50px;text-align:center;'>" + degString + "</span></td><td  style='width:160px;'><span style='padding-left:50px;'>" + romanLetter(returnYearforSem(cursem)) + "</span></td><td  style='width:140px;text-align:right;'><span style='padding-left:50px;'>" + rollno + "</span></td></tr></table>");
            recptDt = recptDt.Split('/')[1] + "/" + recptDt.Split('/')[0] + "/" + recptDt.Split('/')[2];
            #endregion

            #region Receipt Body
            sbHtml.Append("<div style='width:470px; height:189px; padding-top:3px; '><table  class='classBold10'>");
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

                    sbHtml.Append("<tr><td style='width:20px;text-align:right;'>" + sno + "</td><td style='width:380px;'>" + disphdr + "</td><td style='width:60px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td></tr>");
                }

            }
            #endregion

            sbHtml.Append("</table></div>");
            double totalamount = curpaid;

            sbHtml.Append("<div style='width:470px; height:87px; padding-top:3px; '><table  class='classBold10'><tr><td style='width:20px;text-align:right;'></td><td style='width:380px;'></td><td style='width:60px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td></tr><tr><td Colspan='3'><span style='padding-left:50px;'> " + DecimalToWords((decimal)totalamount) + " Only</span></td></tr></table></div>");
            sbHtml.Append("</td></tr></table></div>");
            #endregion
        }
        catch { sbHtml.Clear(); }
        return sbHtml;
    }
}