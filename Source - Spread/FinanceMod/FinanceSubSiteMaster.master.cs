using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Collections;

public partial class FinanceSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    ArrayList arlist = new ArrayList();
    string sql = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        string group_code = Convert.ToString(Session["group_code"]);
        if (group_code.Contains(";"))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            grouporusercode = " group_code=" + group_code + "";
        else
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        //string strPreviousPage = "";
        //if (Request.UrlReferrer != null)
        //{
        //    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //}
        //if (strPreviousPage == "")
        //{
        //    Session["IsLogin"] = "0";
        //    Response.Redirect("~/Default.aspx");
        //}

        string collegecode = Session["Collegecode"].ToString();

        string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "' order by college_code");

        if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
        {
            string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "' order by college_code").Trim();
            collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
        }
        lblcolname.Text = collegeName;

        //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
        string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
        string colornew = "";
        if (color.Trim() == "0")
        {
            colornew = "#06d995";
        }
        else
        {
            colornew = color;
            //prewcolor.Attributes.Add("style", "background-color:" + colornew + ";");
        }
        if (!IsPostBack)
        {
            MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = "";
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);


                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = "";
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);



                    string staffname = "";
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);

                    string deptname = "";
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);

                }
            }
            else
            {


                string staffname = "";
                sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                staffname = da.GetFunction(sql);
                lbslstaffname.Text = Convert.ToString(staffname);

            }
        }
        try
        {
            loadFinanceDetails();
            bool save = false;
            if (arlist.Count > 0)
            {
                for (int row = 0; row < arlist.Count; row++)
                {
                    string values = Convert.ToString(arlist[row]);
                    if (!string.IsNullOrEmpty(values))
                    {
                        string[] splval = values.Split('-');
                        if (splval.Length > 0)
                        {
                            string InsQ = "if exists(select * from Security_Rights_Details where rights_Code='" + splval[2] + "' and ModuleName='" + splval[0] + "' and HeaderName='" + splval[1] + "') update Security_Rights_Details set ReportId='" + splval[3] + "',ReportName='" + splval[4] + "',PageName='" + splval[5] + "',HelpUrl='" + splval[6] + "',PagePriority='" + splval[7] + "',HeaderPriority='" + splval[8] + "' where rights_Code='" + splval[2] + "' and ModuleName='" + splval[0] + "' and HeaderName='" + splval[1] + "' else insert into Security_Rights_Details(ModuleName,HeaderName,Rights_Code,ReportId,ReportName,PageName,HelpUrl,PagePriority,HeaderPriority) values('" + splval[0] + "','" + splval[1] + "','" + splval[2] + "','" + splval[3] + "','" + splval[4] + "','" + splval[5] + "','" + splval[6] + "','" + splval[7] + "','" + splval[8] + "')";
                            int upd = da.update_method_wo_parameter(InsQ, "Text");
                            save = true;
                        }
                    }
                }
            }

            if (save)
            {
                DataSet dsRights = new DataSet();
                DataTable dtOutput = new DataTable();
                DataView dvnew = new DataView();
                string SelQ = string.Empty;
                SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Finance'";
                SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Finance' order by HeaderPriority, PagePriority asc";
                dsRights = da.select_method_wo_parameter(SelQ, "Text");
                if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0 && dsRights.Tables[1].Rows.Count > 0)
                {
                    dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Master'";
                    dvnew = dsRights.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        MasterList.Visible = true;
                        for (int tab1 = 0; tab1 < dvnew.Count; tab1++)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            tabs1.Controls.Add(li);
                            tabs1.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("target", "_blank");
                            anchor.Attributes.Add("href", Convert.ToString(dvnew[tab1]["PageName"]));
                            anchor.InnerText = Convert.ToString(dvnew[tab1]["ReportName"]);
                            li.Controls.Add(anchor);
                        }
                    }
                    else
                        MasterList.Visible = false;
                    dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Operation'";
                    dvnew = dsRights.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        OperationList.Visible = true;
                        for (int tab2 = 0; tab2 < dvnew.Count; tab2++)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            tabs2.Controls.Add(li);
                            if (dvnew.Count <= 10)
                                tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;height:auto;");
                            else if (dvnew.Count > 10)
                                tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px; height:450px;");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("target", "_blank");
                            anchor.Attributes.Add("href", Convert.ToString(dvnew[tab2]["PageName"]));
                            anchor.InnerText = Convert.ToString(dvnew[tab2]["ReportName"]);
                            li.Controls.Add(anchor);
                        }
                    }
                    else
                        OperationList.Visible = false;
                    dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Reports'";
                    dvnew = dsRights.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        ReportList.Visible = true;
                        for (int tab3 = 0; tab3 < dvnew.Count; tab3++)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            tabs3.Controls.Add(li);
                            if (dvnew.Count <= 10)
                                tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;height:auto;");
                            else if (dvnew.Count > 10)
                                tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px; height:450px;");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("target", "_blank");
                            anchor.Attributes.Add("href", Convert.ToString(dvnew[tab3]["PageName"]));
                            anchor.InnerText = Convert.ToString(dvnew[tab3]["ReportName"]);
                            li.Controls.Add(anchor);
                        }
                    }
                    else
                        ReportList.Visible = false;
                    dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Charts'";
                    dvnew = dsRights.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        ChartList.Visible = true;
                        for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            tabs4.Controls.Add(li);
                            tabs4.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                            HtmlGenericControl anchor = new HtmlGenericControl("a");
                            anchor.Attributes.Add("target", "_blank");
                            anchor.Attributes.Add("href", Convert.ToString(dvnew[tab4]["PageName"]));
                            anchor.InnerText = Convert.ToString(dvnew[tab4]["ReportName"]);
                            li.Controls.Add(anchor);
                        }
                    }
                    else
                        ChartList.Visible = false;
                }
            }
        }
        catch { }
        LiteralControl ltr = new LiteralControl();
        ltr.Text = "<style type=\"text/css\" rel=\"stylesheet\">" +
                    @"#showmenupages .has-sub ul li:hover a
                                                {
color:lightyellow;
                                                    background-color:" + colornew + @";

                                                }
#showmenupages .has-sub ul li a
        {
border-bottom: 1px dotted " + colornew + @";
}
ul li
{
  border-bottom: 1px dotted " + colornew + @";
            border-right: 1px dotted " + colornew + @";
}
ul li:hover
        {
color:lightyellow;
 background-color:" + colornew + @";
}
a:hover
        {
color:lightyellow;
}
                                                </style>
                                                ";
        this.Page.Header.Controls.Add(ltr);
    }

    protected void loadFinanceDetails()
    {
        try
        {
            //master
            arlist.Add("Finance-Master-102-FNM001-Financial Year-FinanceYear.aspx-~/FinanceHelpPages/FinancialHelp.htm-1-1");
            arlist.Add("Finance-Master-103-FNM002-Group Master-GroupMaster.aspx-~/FinanceHelpPages/GroupMasterHelp.htm-2-1");
            arlist.Add("Finance-Master-104-FNM003-Header Master-Account_Header.aspx-~/FinanceHelpPages/HeaderHelp.htm-3-1");
            arlist.Add("Finance-Master-105-FNM004-Ledger Master-ledger_master.aspx-~/FinanceHelpPages/LedgerHelp.htm-4-1");
            arlist.Add("Finance-Master-106-FNM005-Code Setting-BankCodeMaster.aspx-~/FinanceHelpPages/CodeSettingHelp.htm-5-1");
            arlist.Add("Finance-Master-107-FNM006-Bank Master-BankMaster.aspx-~/FinanceHelpPages/BankMasterHelp.htm-6-1");
            arlist.Add("Finance-Master-108-FNM007-Receipt / Challan Print Setting-CRSettings.aspx-~/FinanceHelpPages/CRSettingHelp.htm-7-1");
            arlist.Add("Finance-Master-232017108-FNM008-Part Payment Student Settings-PartPayStudsettings.aspx-~/FinanceHelpPages/CRSettingHelp.htm-8-1");

            //operation
            arlist.Add("Finance-Operation-110-FNOP001-Journal-journal.aspx-~/FinanceHelpPages/journalHelp.htm-1-2");
            arlist.Add("Finance-Operation-9110-FNOP001-Journal-journalGrid.aspx-~/FinanceHelpPages/journalGridHelp.htm-2-2");
            arlist.Add("Finance-Operation-111-FNOP002-Receipt / Challan-ChallanReceipt.aspx-~/FinanceHelpPages/ChallanReceiptHelp.htm-3-2");
            arlist.Add("Finance-Operation-169-FNOP002-Receipt / Challan-NewChallanReceipt.aspx-~/FinanceHelpPages/ChallanReceiptHelp.htm-3-2");
            arlist.Add("Finance-Operation-112-FNOP003-Challan Confirm-ChallanConfirm.aspx-~/FinanceHelpPages/ChallanConfirmHelp.htm-4-2");
            arlist.Add("Finance-Operation-174-FNOP004-New Receipt Miscellaneous-NewChallanReceiptOther.aspx-~/FinanceHelpPages/NewChallanReceiptOtherHelp.htm-5-2");
            arlist.Add("Finance-Operation-113-FNOP005-Receipt Cancel and Duplicate-CancelReceiptDuplicate.aspx-~/FinanceHelpPages/CancelReceiptDuplicateHelp.htm-6-2");
            arlist.Add("Finance-Operation-114-FNOP006-Bank Reconciliation-BankReconciliation.aspx-~/FinanceHelpPages/BankReconciliationHelp.htm-7-2");
            arlist.Add("Finance-Operation-115-FNOP007-Transfer / Refund-TransferRefundSettgins.aspx-~/FinanceHelpPages/TransferRefundHelp.htm-8-2");
            arlist.Add("Finance-Operation-116-FNOP008-Payment – Cash / Bank-DirectPayment.aspx-~/FinanceHelpPages/DirectPaymentHelp.htm-9-2");
            arlist.Add("Finance-Operation-117-FNOP009-Contra-Contra.aspx-~/FinanceHelpPages/ContraHelp.htm-10-2");
            arlist.Add("Finance-Operation-118-FNOP010-Student Fee Due Extention-Student_FeeExtn.aspx-~/FinanceHelpPages/Student_FeeExtnHelp.htm-11-2");
            arlist.Add("Finance-Operation-119-FNOP011-Bank Statment Import-Bank_Stmnt_Import.aspx-~/FinanceHelpPages/Bank_Stmnt_ImportHelp.htm-12-2");
            arlist.Add("Finance-Operation-145-FNOP012-Month Wise Import-MonthwiseImport.aspx-~/FinanceHelpPages/MonthwiseImportHelp.htm-13-2");
            arlist.Add("Finance-Operation-120-FNOP013-Receipt-ReceiptJpr.aspx-~/FinanceHelpPages/ReceiptHelp.htm-14-2");
            arlist.Add("Finance-Operation-146-FNOP014-Challan No Update-ChallanNoUpdate.aspx-~/FinanceHelpPages/ChallanNoUpdateHelp.htm-15-2");
            arlist.Add("Finance-Operation-151-FNOP015-Excess Receipt-ExcessReceipt.aspx-~/FinanceHelpPages/ExcessReceiptHelp.htm-16-2");
            arlist.Add("Finance-Operation-162-FNOP016-Student Bank Statemnet Import-StudentBankStatemnetImport.aspx-~/Student Bank Statemnet ImportHelp.htm-17-2");
            arlist.Add("Finance-Operation-182-FNOP017-Student Transfer Without Fees-StudTransWithoutFess.aspx-~/StudTransWithoutFessHelp.htm-18-2");
            arlist.Add("Finance-Operation-183-FNOP018-Student Transfer Report-StudTransReport.aspx-~/StudTransReportHelp.htm-19-2");
            arlist.Add("Finance-Operation-192-FNOP019-Payment Reconciliation-PaymentReconciliation.aspx-~/FinanceHelpPages/PaymentReconciliationHelp.htm-20-2");
            arlist.Add("Finance-Operation-196-FNOP020-Staff / Vendor / Other FeeEdit-StaffVendorOtherFeeEdit.aspx-~/FinanceHelpPages/StaffVendorOtherFeeEdit.htm-21-2");
            arlist.Add("Finance-Operation-198-FNOP021-Actual FinanceYear Settings-actualfinyearsetting.aspx-~/FinanceHelpPages/actualFinyearsetting.htm-22-2");
            arlist.Add("Finance-Operation-199-FNOP022-Fine Cancel Setting-Finesetting.aspx-~/FinanceHelpPages/Finesetting.htm-23-2");//Added by saranya on 5April2018


            //report
            arlist.Add("Finance-Reports-122-FNR001-Student / Staff Fee Allotment , Paid, Concession and Balance-StudentfeeAllotReportalter.aspx-~/FinanceHelpPages/StudentfeeAllotReportalterHelp.htm-1-3");
            arlist.Add("Finance-Reports-123-FNR002-Credit and Debit Report-CreditdebitReport.aspx-~/FinanceHelpPages/CreditdebitReportHelp.htm-2-3");
            arlist.Add("Finance-Reports-124-FNR003-Bank Wise Collection Report-BankWise_Deposit.aspx-~/FinanceHelpPages/BankWise_DepositHelp.htm-3-3");
            arlist.Add("Finance-Reports-125-FNR004-Daily Payment Report-DailyPayment_Report.aspx-~/FinanceHelpPages/DailyPayment_ReportHelp.htm-4-3");
            arlist.Add("Finance-Reports-126-FNR005-Daily Fees Collection Report-DFCR_Report.aspx-~/FinanceHelpPages/Overall_student_Fee_StatusHelp.htm-5-3");
            arlist.Add("Finance-Reports-127-FNR006-Contra WithDraw Report-ContraWithdrawReport.aspx-~/FinanceHelpPages/ContraWithdrawReportHelp.htm-6-3");
            arlist.Add("Finance-Reports-128-FNR007-General Fee Structure Report-Fee_Structure.aspx-~/FinanceHelpPages/GeneralFeeStructureReportHelp.htm-7-3");
            arlist.Add("Finance-Reports-129-FNR008-Term Fee Report-Term_Fee_Report.aspx-~/FinanceHelpPages/Term_Fee_ReportHelp.htm-8-3");
            arlist.Add("Finance-Reports-130-FNR009-Finance MIS Report-Finance_MIS_Report.aspx-~/FinanceHelpPages/Finance_MIS_ReportHelp.htm-9-3");
            arlist.Add("Finance-Reports-131-FNR010-Individual Student Fee Status Report-Individual_StudentFeeStatus.aspx-~/FinanceHelpPages/Individual_StudentFeeStatusHelp.htm-10-3");

            arlist.Add("Finance-Reports-132-FNR011-Concession Report-Concession Report.aspx-~/FinanceHelpPages/Concession ReportHelp.htm-11-3");
            arlist.Add("Finance-Reports-133-FNR012-Finance Concession Report-FinanceConcessionReport.aspx-~/FinanceHelpPages/FinanceConcessionReportHelp.htm-12-3");
            arlist.Add("Finance-Reports-134-FNR013-Finance Yearwise Header Report-FinanceYearWiseHeaderReport.aspx-~/FinanceHelpPages/FinanceYearWiseHeaderReportHelp.htm-13-3");
            arlist.Add("Finance-Reports-135-FNR014-Fees Structure Report-FeesStructureReport.aspx-~/FinanceHelpPages/FeesStructureReportHelp.htm-14-3");
            arlist.Add("Finance-Reports-147-FNR015-Finance Reconsilation Report-Finance Reconsilation.aspx-~/FinanceHelpPages/Finance ReconsilationHelp.htm-15-3");
            arlist.Add("Finance-Reports-148-FNR016-Refund Report-Refund Report.aspx-~/FinanceHelpPages/Refund ReportHelp.htm-16-3");
            arlist.Add("Finance-Reports-149-FNR017-Consolidate Count Report-ConsolidateCountReport.aspx-~/FinanceHelpPages/ConsolidateCountReportHelp.htm-17-3");
            arlist.Add("Finance-Reports-150-FNR018-Transport Allot Report-TransportAllotReport.aspx-~/FinanceHelpPages/TransportAllotReportHelp.htm-18-3");

            arlist.Add("Finance-Reports-136-FNR019-Scholarship Report-ScholarshipReport.aspx-~/FinanceHelpPages/ScholarshipReportHelp.htm-19-3");
            arlist.Add("Finance-Reports-152-FNR020-ExportXML Import Report-ExportXMLForTally.aspx-~/FinanceHelpPages/ExportXMLForTallyHelp.htm-20-3");
            arlist.Add("Finance-Reports-153-FNR021-Finance MIS Budget Report-Finance_MIS_Budget_Report.aspx-~/FinanceHelpPages/Finance_MIS_Budget_ReportHelp.htm-21-3");
            arlist.Add("Finance-Reports-154-FNR022-Sms Send To Student-SmsSendtoStudent.aspx-~/FinanceHelpPages/SmsSendtoStudentHelp.htm-22-3");
            arlist.Add("Finance-Reports-155-FNR023-Due Fine Amount Allot For Student-DueFineAmountAllotForStudent.aspx-~/FinanceHelpPages/DueFineAmountAllotForStudentHelp.htm-23-3");
            arlist.Add("Finance-Reports-156-FNR024-Transport And Hostel Allotment Report-TransportAndHostelAllotmentReport.aspx-~/FinanceHelpPages/TransportAndHostelAllotmentReportHelp.htm-24-3");

            arlist.Add("Finance-Reports-157-FNR025-Finance Collection Report-FinanceCollectionReport.aspx-~/FinanceHelpPages/Finance_MIS_Budget_ReportHelp.htm-25-3");
            arlist.Add("Finance-Reports-158-FNR026-Cancel Receipt Report-CancelReceiptReport.aspx-~/FinanceHelpPages/SmsSendtoStudentHelp.htm-26-3");
            arlist.Add("Finance-Reports-159-FNR027-Student Remove Hostel and Transport-StudentTransferHostelTransport.aspx-~/FinanceHelpPages/DueFineAmountAllotForStudentHelp.htm-27-3");
            arlist.Add("Finance-Reports-160-FNR028-Post Metric Scholarship-PostMetricScholarship.aspx-~/FinanceHelpPages/TransportAndHostelAllotmentReportHelp.htm-28-3");
            arlist.Add("Finance-Reports-161-FNR029-Receipt Amount Adjust-ReceiptAmountAdjust.aspx-~/FinanceHelpPages/ReceiptAmountAdjustHelp.htm-28-3");
            arlist.Add("Finance-Reports-163-FNR030-Financial Statement_Daily Collection Statement-FinanceDailyCollectionStatementReport.aspx-~/FinanceHelpPages/FinanceDailyCollectionStatementReportHelp.htm-29-3");
            arlist.Add("Finance-Reports-164-FNR031-Financial Statement_Daily Collection Detailed Statement-DailyCollectionDetailedStatementReport.aspx-~/FinanceHelpPages/DailyCollectionDetailedStatementReportHelp.htm-30-3");
            arlist.Add("Finance-Reports-165-FNR032-Financial Statement_Daily Collection Department Cumulative Statement Report-DailyCollectionDeptCumulativeReport.aspx-~/FinanceHelpPages/DailyCollectionDeptCumulativeReportHelp.htm-31-3");

            arlist.Add("Finance-Reports-166-FNR033-Financial Statement_Paymode Collection Report-FinancePaymodeCollectionReport.aspx-~/FinanceHelpPages/FinancePaymodeCollectionReportHelp.htm-32-3");
            arlist.Add("Finance-Reports-167-FNR034-Financial Statement_Departmentwise Collection Report-DepatmentwiseCollectionReport.aspx-~/FinanceHelpPages/DepatmentwiseCollectionReporttHelp.htm-33-3");

            arlist.Add("Finance-Reports-168-FNR035-Financial Daily Fess Collection Report-DailyFeesCollectionReport.aspx-~/FinanceHelpPages/DailyFeesCollectionReporttHelp.htm-34-3");
            arlist.Add("Finance-Reports-170-FNR036-Student Scheme Admission Report-Scheme_Admission_Report.aspx-~/FinanceHelpPages/Scheme_Admission_ReporttHelp.htm-35-3");
            arlist.Add("Finance-Reports-171-FNR037-Feecategory Settings-Feecatagorysettings.aspx-~/FinanceHelpPages/FeecatagorysettingsHelp.htm-36-3");
            arlist.Add("Finance-Reports-172-FNR038-Termwise Financial Daily Fees Collection Report-DailyFeesCollectionReportTerm.aspx-~/FinanceHelpPages/DailyFeesCollectionReportTermHelp.htm-37-3");
            arlist.Add("Finance-Reports-173-FNR039-Student Log Detail Report-StudentLogDetailReport.aspx-~/FinanceHelpPages/StudentLogDetailReportHelp.htm-38-3");

            arlist.Add("Finance-Reports-175-FNR040-Finance BillNowise Paid Report-FinancestudPaidDet.aspx-~/FinanceHelpPages/FinancestudPaidDetHelp.htm-39-3");
            arlist.Add("Finance-Reports-176-FNR041-Finance Headerwise Paid Report-MulInstHdCollection.aspx-~/FinanceHelpPages/MulInstHdCollectionHelp.htm-40-3");
            arlist.Add("Finance-Reports-177-FNR042-Finance Header Institutionwise Paid Report-FinanceInstwiseRpt.aspx-~/FinanceHelpPages/FinanceInstwiseRptHelp.htm-41-3");
            arlist.Add("Finance-Reports-178-FNR043-Finance Institutionwise Paid Report-FinanceBalDet.aspx-~/FinanceHelpPages/FinanceBalDetReportHelp.htm-42-3");
            arlist.Add("Finance-Reports-179-FNR044-Denomination Report-DenominationReport.aspx-~/FinanceHelpPages/DenominationReportReportHelp.htm-43-3");
            arlist.Add("Finance-Reports-180-FNR045-Daily Fees Collection School Report-DFCR_ReportSchool.aspx-~/FinanceHelpPages/DFCR_ReportSchoolHelp.htm-44-3");
            arlist.Add("Finance-Reports-181-FNR046-Consolidate Count Report School-ConsolidateCountReportSchool.aspx-~/FinanceHelpPages/ConsolidateCountReportSchoolHelp.htm-45-3");
            arlist.Add("Finance-Reports-184-FNR047-Student Paid Count Report-DeptWiseStudPaidCountReport.aspx-~/FinanceHelpPages/DeptWiseStudPaidCountReportHelp.htm-46-3");
            arlist.Add("Finance-Reports-185-FNR048-Finance Universal Report-FinanceUniversalReport.aspx-~/FinanceHelpPages/FinanceUniversalReportHelp.htm-47-3");
            arlist.Add("Finance-Reports-186-FNR049-Finance Student Abstract Report-FinStudAbstractReport.aspx-~/FinanceHelpPages/FinStudAbstractReportHelp.htm-48-3");
            arlist.Add("Finance-Reports-187-FNR050-Finance Academic Year Settings-AcademicYearSettings.aspx-~/FinanceHelpPages/AcademicYearSettingsHelp.htm-49-3");
            arlist.Add("Finance-Reports-188-FNR051-Finance Student Payment Voucher-StudentPayment.aspx-~/FinanceHelpPages/AcademicYearSettingsHelp.htm-50-3");
            arlist.Add("Finance-Reports-189-FNR052-Finance Universal Report Multiple-FinanceUniversalReportMultiple.aspx-~/FinanceHelpPages/FinanceUniversalReportMultipleHelp.htm-51-3");
            arlist.Add("Finance-Reports-190-FNR053-Receipt / Payment Cumulative Report-ReceiptPaymentCumulative.aspx-~/FinanceHelpPages/ReceiptPaymentCumulativeHelp.htm-52-3");
            arlist.Add("Finance-Reports-191-FNR054-Trail Balance Sheet Report-FinanceTrialBalance.aspx-~/FinanceHelpPages/FinanceTrialBalanceHelp.htm-53-3");


            arlist.Add("Finance-Reports-193-FNR055-Consolidated Demand Report-ConsolidatedDemandReport.aspx-~/FinanceHelpPages/ConsolidatedDemandReportHelp.htm-54-3");//Added by saranya on 7/2/2018
            arlist.Add("Finance-Reports-194-FNR056-Finance YearWise Allotment Report-FinanceYearWiseCollectionReport.aspx-~/FinanceHelpPages/FinanceYearWiseCollectionReportHelp.htm-55-3");//added by abrna on 16.02.2018

            arlist.Add("Finance-Reports-195-FNR057-Individual Header Detailed Report-IndividualHeaderDetailReport.aspx-~/FinanceHelpPages/IndividualHeaderDetailReportHelp.htm-56-3");//Added by saranya on 23/2/2018
            arlist.Add("Finance-Reports-197-FNR058-Student Wise Fee Concession Report-FeeConcessionByManagement.aspx-~/FinanceHelpPages/FeeConcessionByManagementHelp.htm-57-3");//Added by saranya on 26/3/2018
            arlist.Add("Finance-Reports-202-FNR059-Variation Statement Report-VariationStatementReport.aspx-~/FinanceHelpPages/VariationStatementReportHelp.htm-58-3");//Added by saranya on 30/3/2018
            arlist.Add("Finance-Reports-203-FNR060-Monthly Fees Report-MonthlyFeesReport.aspx-~/FinanceHelpPages/MonthlyFeesReportHelp.htm-59-3");//Delsi 28/06/2018
            arlist.Add("Finance-Reports-204-FNR061-IOB Payment Missing Updation-onlinefeestransactionupdate.aspx-~/FinanceHelpPages/onlinefeestransactionupdateHelp.htm-60-3");
            arlist.Add("Finance-Reports-205-FNR062-Extension Report-ExtensionReport.aspx-~/FinanceHelpPages/ExtensionReportHelp.htm-61-3");//abarna
            //charts
            arlist.Add("Finance-Charts-138-FNC001-Student / Staff Fee Allotment , Paid, Concession and Balance Chart-StudentFeeAllotReportChart.aspx-~/FinanceHelpPages/StudentFeeAllotReportChartHelp.htm-1-4");
            arlist.Add("Finance-Charts-139-FNC002-Credit and Debit Report Chart-CreditdebitReportChart.aspx-~/FinanceHelpPages/CreditdebitReportChartHelp.htm-2-4");
            arlist.Add("Finance-Charts-140-FNC003-BankWise Deposit Chart-BankWise_DepositChart.aspx-~/FinanceHelpPages/BankWise_DepositChartHelp.htm-3-4");
            arlist.Add("Finance-Charts-141-FNC004-Daily Payment Report Chart-DailyPayment_ReportChart.aspx-~/FinanceHelpPages/DailyPayment_ReportChartHelp.htm-4-4");

            //others
            arlist.Add("Finance-Others-143-FNO001-Budget Master-BudgetAllocation.aspx-~/FinanceHelpPages/BudgetAllocationHelp.htm-1-5");
            arlist.Add("Finance-Others-144-FNO002-Concession Master-ConsessionMaster.aspx-~/FinanceHelpPages/ConsessionMasterHelp.htm-2-5");
        }
        catch { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if (Session["Entry_Code"] != null)
        {
            string entryCode = Session["Entry_Code"].ToString();
            da.userTimeOut(entryCode);
        }
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/default.aspx", false);

    }


    protected void ImageButton3_Onclick(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["backbutton_value"]).ToLower() != "student" || string.IsNullOrEmpty(Convert.ToString(Session["backbutton_value"])))
        {
            ImageButton3.PostBackUrl = "~/Financemod/FinanceIndex.aspx";
            Response.Redirect(ImageButton3.PostBackUrl);
        }
        else if (Convert.ToString(Session["backbutton_value"]).ToLower() != "finance" || string.IsNullOrEmpty(Convert.ToString(Session["backbutton_value"])))
        {
            Session["backbutton_value"] = "finance";
            ImageButton3.PostBackUrl = "~/studentmod/StudentHome.aspx";
            Response.Redirect(ImageButton3.PostBackUrl);
        }
    }
}

