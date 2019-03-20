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
        lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
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
            arlist.Add("Finance-Operation-112-FNOP003-Challan Confirm-ChallanConfirm.aspx-~/FinanceHelpPages/ChallanConfirmHelp.htm-4-2");
            arlist.Add("Finance-Operation-113-FNOP004-Receipt Cancel and Duplicate-CancelReceiptDuplicate.aspx-~/FinanceHelpPages/CancelReceiptDuplicateHelp.htm-5-2");
            arlist.Add("Finance-Operation-114-FNOP005-Bank Reconciliation-BankReconciliation.aspx-~/FinanceHelpPages/BankReconciliationHelp.htm-6-2");
            arlist.Add("Finance-Operation-115-FNOP006-Transfer / Refund-TransferRefund.aspx-~/FinanceHelpPages/TransferRefundHelp.htm-7-2");
            arlist.Add("Finance-Operation-116-FNOP007-Payment – Cash / Bank-DirectPayment.aspx-~/FinanceHelpPages/DirectPaymentHelp.htm-8-2");
            arlist.Add("Finance-Operation-117-FNOP008-Contra-Contra.aspx-~/FinanceHelpPages/ContraHelp.htm-9-2");
            arlist.Add("Finance-Operation-118-FNOP09-Student Fee Due Extention-Student_FeeExtn.aspx-~/FinanceHelpPages/Student_FeeExtnHelp.htm-10-2");
            arlist.Add("Finance-Operation-119-FNOP010-Bank Statment Import-Bank_Stmnt_Import.aspx-~/FinanceHelpPages/Bank_Stmnt_ImportHelp.htm-11-2");
            arlist.Add("Finance-Operation-145-FNOP011-Month Wise Import-MonthwiseImport.aspx-~/FinanceHelpPages/MonthwiseImportHelp.htm-12-2");
            arlist.Add("Finance-Operation-120-FNOP012-Receipt-ReceiptJpr.aspx-~/FinanceHelpPages/ReceiptHelp.htm-13-2");
            arlist.Add("Finance-Operation-146-FNOP013-Challan No Update-ChallanNoUpdate.aspx-~/FinanceHelpPages/ChallanNoUpdateHelp.htm-14-2");
            arlist.Add("Finance-Operation-151-FNOP014-Excess Receipt-ExcessReceipt.aspx-~/FinanceHelpPages/ExcessReceiptHelp.htm-15-2");
            arlist.Add("Finance-Operation-162-FNOP015-Student Bank Statemnet Import-StudentBankStatemnetImport.aspx-~/Student Bank Statemnet ImportHelp.htm-16-2");


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
            arlist.Add("Finance-Reports-152-FNR020-ExportXMLForTally Report-ExportXMLForTally.aspx-~/FinanceHelpPages/ExportXMLForTallyHelp.htm-20-3");
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
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/default.aspx", false);

    }

}

