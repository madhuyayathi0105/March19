using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class FinanceIndex : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ArrayList arlist = new ArrayList();

    string usercode = "";
    string groupcode = "";
    string collegecode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        collegecode = Convert.ToString(Session["collegecode"]);
        usercode = Convert.ToString(Session["usercode"]);
        groupcode = Convert.ToString(Session["group_code"]);
        if (!IsPostBack)
        {
            loadGrid();
        }
    }

    protected void loadGrid()
    {
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
                            int upd = d2.update_method_wo_parameter(InsQ, "Text");
                            save = true;
                        }
                    }
                }
            }

            //if (save)
            //{
            string grouporusercode = "";
            string group_code = Convert.ToString(Session["group_code"]);
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                grouporusercode = " and group_code=" + group_code.Trim() + "";

            else
                grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";

            string SelQ = " select ModuleName ,HeaderName ,srd.Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL from Security_Rights_Details srd,security_user_right sur where sur.rights_code=srd.Rights_Code  " + grouporusercode + "  and srd.ModuleName='Finance' order by HeaderPriority, PagePriority asc";

            //string SelQ = " select ModuleName ,HeaderName ,srd.Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL from Security_Rights_Details srd,security_user_right sur where sur.rights_code=srd.Rights_Code and user_code='30'  and college_code='13' order by HeaderPriority, PagePriority asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(ds.Tables[0]);
            }
            else
            {
                GdFin.DataSource = null;
                GdFin.DataBind();
            }
            //}


        }
        catch { }
    }

    private void BindMenuGrid(DataTable dtMenu)
    {
        GdFin.DataSource = dtMenu;
        GdFin.DataBind();
        loadcolor();
    }

    protected void loadcolor()
    {
        try
        {
            for (int ik = 0; ik < GdFin.Rows.Count; ik++)
            {
                Label sno = (Label)GdFin.Rows[ik].Cells[0].FindControl("lblSno");
                Label hdrname = (Label)GdFin.Rows[ik].Cells[1].FindControl("lblHdrName");
                Label hdrid = (Label)GdFin.Rows[ik].Cells[2].FindControl("lblReportId");
                LinkButton menu = (LinkButton)GdFin.Rows[ik].Cells[3].FindControl("lbPagelink");
                HtmlAnchor help = (HtmlAnchor)GdFin.Rows[ik].Cells[4].FindControl("lbHelplink");
                // HyperLink help = (HyperLink)GdFin.Rows[ik].Cells[3].FindControl("lbHelplink");
                // Label help = (Label)GdFin.Rows[ik].Cells[4].FindControl("lbHelplink");
                if (hdrname.Text == "Master")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    help.Style.Add("color", "#ff00ff");
                }
                if (hdrname.Text == "Operation")
                {
                    sno.ForeColor = Color.Black;
                    hdrname.ForeColor = Color.Black;
                    hdrid.ForeColor = Color.Black;
                    menu.ForeColor = Color.Black;
                    help.Style.Add("color", "Black");

                }
                if (hdrname.Text == "Reports")
                {
                    sno.ForeColor = Color.Green;
                    hdrname.ForeColor = Color.Green;
                    hdrid.ForeColor = Color.Green;
                    menu.ForeColor = Color.Green;
                    help.Style.Add("color", "Green");
                }
                if (hdrname.Text == "Charts")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    hdrname.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    hdrid.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    menu.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    help.Style.Add("color", "#3869fa");
                }
                if (hdrname.Text == "Others")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    help.Style.Add("color", "#ff00ff");
                }
            }
        }
        catch { }
    }

    protected void GdFin_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        ////Add CSS class on normal row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //          e.Row.RowState == DataControlRowState.Normal)
        //    e.Row.CssClass = "normal";

        ////Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        //          e.Row.RowState == DataControlRowState.Alternate)
        //    e.Row.CssClass = "alternate";       
    }

    protected void GdFin_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = GdFin.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = GdFin.Rows[i];
                GridViewRow previousRow = GdFin.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lblHdrName");
                    Label lnlname1 = (Label)previousRow.FindControl("lblHdrName");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan = 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
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
            arlist.Add("Finance-Operation-118-FNOP010-Student Fee Due Concession/Extention-Student_FeeExtn.aspx-~/FinanceHelpPages/Student_FeeExtnHelp.htm-11-2");
            arlist.Add("Finance-Operation-119-FNOP011-Bank Statment Import-Bank_Stmnt_Import.aspx-~/FinanceHelpPages/Bank_Stmnt_ImportHelp.htm-12-2");
            arlist.Add("Finance-Operation-145-FNOP012-Month Wise Import-MonthwiseImport.aspx-~/FinanceHelpPages/MonthwiseImportHelp.htm-13-2");
            arlist.Add("Finance-Operation-120-FNOP013-Receipt-ReceiptJpr.aspx-~/FinanceHelpPages/ReceiptHelp.htm-14-2");
            arlist.Add("Finance-Operation-146-FNOP014-Challan No Update-ChallanNoUpdate.aspx-~/FinanceHelpPages/ChallanNoUpdateHelp.htm-15-2");
            arlist.Add("Finance-Operation-151-FNOP015-Excess Receipt-ExcessReceipt.aspx-~/FinanceHelpPages/ExcessReceiptHelp.htm-16-2");
            arlist.Add("Finance-Operation-162-FNOP016-Student Bank Statemnet Import-StudentBankStatemnetImport.aspx-~/Student Bank Statemnet ImportHelp.htm-17-2");
            arlist.Add("Finance-Operation-182-FNOP017-Student Transfer Without Fees-StudTransWithoutFess.aspx-~/StudTransWithoutFessHelp.htm-18-2");
            arlist.Add("Finance-Operation-183-FNOP018-Student Transfer Report-StudTransReport.aspx-~/StudTransReportHelp.htm-19-2");
            arlist.Add("Finance-Operation-192-FNOP019-Payment Reconciliation-PaymentReconciliation.aspx-~/FinanceHelpPages/PaymentReconciliationHelp.htm-20-2");
            arlist.Add("Finance-Operation-196-FNOP020-Staff / Vendor / Other FeeEdit-StaffVendorOtherFeeEdit.aspx-~/FinanceHelpPages/StaffVendorOtherFeeEditHelp.htm-21-2");
            arlist.Add("Finance-Operation-198-FNOP021-Actual FinanceYear Settings-actualFinyearsetting.aspx-~/FinanceHelpPages/actualFinyearsettingHelp.htm-22-2");
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
            arlist.Add("Finance-Reports-193-FNR055-Concession Wise Student Count-ConsolidatedDemandReport.aspx-~/FinanceHelpPages/ConsolidatedDemandReportHelp.htm-54-3");//Added by saranya on 7/2/2018
            arlist.Add("Finance-Reports-194-FNR056-Finance YearWise Allotment Report-FinanceYearWiseCollectionReport.aspx-~/FinanceHelpPages/FinanceYearWiseCollectionReportHelp.htm-55-3");//added by abrna on 16.02.2018
            arlist.Add("Finance-Reports-195-FNR057-Individual Header Detailed Report-IndividualHeaderDetailReport.aspx-~/FinanceHelpPages/IndividualHeaderDetailReportHelp.htm-56-3");//Added by saranya on 23/2/2018
            arlist.Add("Finance-Reports-197-FNR058-Student Wise Fee Concession Report-FeeConcessionByManagement.aspx-~/FinanceHelpPages/FeeConcessionByManagementHelp.htm-57-3");   //Added by saranya on 26/3/2018
            arlist.Add("Finance-Reports-202-FNR059-Variation Statement Report-VariationStatementReport.aspx-~/FinanceHelpPages/VariationStatementReportHelp.htm-58-3");//Added by saranya on 30/3/2018
            arlist.Add("Finance-Reports-203-FNR060-Monthly Fees Report-MonthlyFeesReport.aspx-~/FinanceHelpPages/MonthlyFeesReportHelp.htm-59-3");//Delsi 28/06/2018
            arlist.Add("Finance-Reports-204-FNR061-IOB Payment Missing Updation-onlinefeestransactionupdate.aspx-~/FinanceHelpPages/onlinefeestransactionupdateHelp.htm-60-3");
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
}