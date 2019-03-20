using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;

public partial class HRMenuIndex : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    static string grouporusercode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";

            if (grouporusercode.Contains(';'))//delsi28.04.2018
            {
                string[] group_semi = grouporusercode.Split(';');
                grouporusercode = group_semi[0].ToString();
            }
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = d2.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='HR'  order by HeaderPriority, PagePriority asc", "Text");
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                grdhrmenu.DataSource = null;
                grdhrmenu.DataBind();
            }
        }
        catch
        {
            grdhrmenu.DataSource = null;
            grdhrmenu.DataBind();
        }
    }

    private void EntryCheck()
    {
        DataTable dtRights = BuildTable();
        try
        {
            for (int row = 0; row < dtRights.Rows.Count; row++)
            {
                StringBuilder sbQuery = new StringBuilder();
                string rightsCode = Convert.ToString(dtRights.Rows[row]["RightsCode"]);
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");

                d2.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
    }

    private void BindMenuGrid(DataTable dtMenu)
    {
        grdhrmenu.DataSource = dtMenu;
        grdhrmenu.DataBind();
    }

    private DataTable BuildTable()
    {
        DataTable dtRights = new DataTable();
        dtRights.Columns.Add("RightsCode");
        dtRights.Columns.Add("Module");
        dtRights.Columns.Add("Header");
        dtRights.Columns.Add("ReportId");
        dtRights.Columns.Add("ReportName");
        dtRights.Columns.Add("PageName");
        dtRights.Columns.Add("HelpPage");
        dtRights.Columns.Add("PagePriority");
        dtRights.Columns.Add("HeaderPriority");

        //Master
        Session["StafforAdmin"] = "";
        Session["clearschedulesession"] = "clear";

        dtRights.Rows.Add("201601", "HR", "Master", "HRM001", "HR Year", "HR_Year_Alter.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("201602", "HR", "Master", "HRM002", "Code Master", "Code_Setting.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("201603", "HR", "Master", "HRM003", "Designation Master", "Designation_Master_Alter.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("201604", "HR", "Master", "HRM004", "Category Master", "CategoryMaster_Alter.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("201605", "HR", "Master", "HRM005", "Leave Master", "LeaveMaster_Alter.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("201606", "HR", "Master", "HRM006", "Manpower Master", "manpower_Alter.aspx", "HelpPage.Html", "6", "1");
        dtRights.Rows.Add("201607", "HR", "Master", "HRM007", "Allowance & Deduction Master", "AllowanceAndDetectionMaster_Alter.aspx", "HelpPage.Html", "7", "1");
        dtRights.Rows.Add("201608", "HR", "Master", "HRM008", "Slab Master", "SlabsMaster_Alter.aspx", "HelpPage.Html", "8", "1");
        dtRights.Rows.Add("201723", "HR", "Master", "HRM009", "Automatic SMS Settings", "SMS_ManagerSettings.aspx", "HelpPage.Html", "9", "1");
        dtRights.Rows.Add("201726", "HR", "Master", "HRM010", "HourWise PayProcess Settings", "HourWise_PayProcess.aspx", "HelpPage.Html", "10", "1");
        dtRights.Rows.Add("201729", "HR", "Master", "HRM011", "Staff Mandatory Bell Time Settings", "Staff_BellTime_Settings.aspx", "HelpPage.Html", "11", "1");

        dtRights.Rows.Add("201738", "HR", "Master", "HRM012", "Other Income and Deduction Head", "ITOtherAllowanceDeduction.aspx", "HelpPage.Html", "12", "1");
        dtRights.Rows.Add("201739", "HR", "Master", "HRM013", "Income and Deduction Group Mapping", "ITGroupMapping.aspx", "HelpPage.Html", "13", "1");

        //Operation
        dtRights.Rows.Add("201609", "HR", "Operation", "HROP001", "Staff Manager", "Staff_Manager.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("201610", "HR", "Operation", "HROP002", "Grade Pay Master", "GradePayMaster.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("201611", "HR", "Operation", "HROP003", "IT Calculation", "ITCalCulationSettings.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("201612", "HR", "Operation", "HROP004", "Pay Process", "Pay_Process.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("201613", "HR", "Operation", "HROP005", "Staff Attendance Entry", "Staff_Attendance1.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("201614", "HR", "Operation", "HROP006", "Staff Manual Attendance", "Staff_ManualAttnd.aspx", "HelpPage.Html", "6", "2");
        dtRights.Rows.Add("201724", "HR", "Operation", "HROP007", "Bio Device Information", "BiodeviceInformation.aspx", "HelpPage.Html", "7", "2");
        dtRights.Rows.Add("201725", "HR", "Operation", "HROP008", "Staff FingerPrint Registration", "Staff_FingerPrintReg.aspx", "HelpPage.Html", "8", "2");
        dtRights.Rows.Add("201615", "HR", "Operation", "HROP009", "Staff Certificate Issues", "", "HelpPage.Html", "9", "2");
        dtRights.Rows.Add("201616", "HR", "Operation", "HROP010", "Staff Priority", "StaffPriority.aspx", "HelpPage.Html", "10", "2");
        dtRights.Rows.Add("201727", "HR", "Operation", "HROP011", "HourWise Staff Attendance", "HourWise_StaffAttnd.aspx", "HelpPage.Html", "11", "2");
        dtRights.Rows.Add("201728", "HR", "Operation", "HROP012", "Staff Manual Grade Pay", "StaffPaySettings.aspx", "HelpPage.Html", "12", "2");
        dtRights.Rows.Add("201742", "HR", "Operation", "HROP013", "Additional Allowance And Deduction", "AdditionalAllowanceDeduction.aspx", "HelpPage.Html", "13", "2");
        // poo 04.11.17
        dtRights.Rows.Add("201744", "HR", "Operation", "HROP014", "Compensation Leave Setting", "CompensationLeaveSetting.aspx", "HelpPage.Html", "14", "2");
        dtRights.Rows.Add("201746", "HR", "Operation", "HROP015", "Salary Hold Setting", "SalaryHoldSet.aspx", "HelpPage.Html", "15", "2");
        dtRights.Rows.Add("201748", "HR", "Operation", "HROP016", "Gratuity Eligibility Setting", "graduityeligibility.aspx","HelpPage.Html", "16", "2");
        

        //Reports
        dtRights.Rows.Add("201617", "HR", "Report", "HRR001", "Staff  Strength Master", "Staff_StrengthMaster.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("201701", "HR", "Report", "HRR002", "Overall / Individual Salary Report", "Individual_SalaryReport.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("201702", "HR", "Report", "HRR003", "PF & ESI Report", "PaymentCal.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("201703", "HR", "Report", "HRR004", "Biometric Report For Staff Attendance", "BioMatric_new.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("201704", "HR", "Report", "HRR005", "Staff Attendance Strength Report", "staffattendreport.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("201705", "HR", "Report", "HRR006", "Overall Monthly Salary", "salary1.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("201706", "HR", "Report", "HRR007", "Monthly Salary Statement", "MonthlyCummulativeSalary.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("201707", "HR", "Report", "HRR008", "Biocorrection Report", "biocorrection.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("201708", "HR", "Report", "HRR009", "Staff Cumulative Leave Report", "staffleavereport2.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("201709", "HR", "Report", "HRR010", "IT Calculation Report", "Incometaxcalculation_report.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("201710", "HR", "Report", "HRR011", "Staff Report", "StaffReport.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("201711", "HR", "Report", "HRR012", "Staff Leave Report", "HrLeaveReport.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("201712", "HR", "Report", "HRR013", "Staff Gender Wise Report", "StaffStrenthReport.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("201713", "HR", "Report", "HRR014", "Staff Attendance Report", "StaffAttendanceReport.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("201714", "HR", "Report", "HRR015", "Department Wise Cummulative Salary", "DepartmentwiseCummulative Salary.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("201715", "HR", "Report", "HRR016", "Individual Staff Attendance", "StaffAttendance.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("201716", "HR", "Report", "HRR017", "Staff Experience Report", "staffexperiencereport.aspx", "HelpPage.Html", "17", "3");
        dtRights.Rows.Add("201717", "HR", "Report", "HRR018", "Staff Attendance Report", "staffattendance_report.aspx", "HelpPage.Html", "18", "3");
        dtRights.Rows.Add("201718", "HR", "Report", "HRR019", "New & Relieved Staff Details", "StaffSalarydetails.aspx", "HelpPage.Html", "19", "3");
        dtRights.Rows.Add("201719", "HR", "Report", "HRR020", "HR salary Income & PF Report", "hrsalaryincomepf.aspx", "HelpPage.Html", "20", "3");
        dtRights.Rows.Add("201720", "HR", "Report", "HRR021", "HR-Finance year Report", "HR_Finance.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("201721", "HR", "Report", "HRR022", "HR-Reconciliation", "HR_Reconciliation.aspx", "HelpPage.Html", "22", "3");
        dtRights.Rows.Add("201722", "HR", "Report", "HRR023", "Original Salary Report", "Original Salary Details.aspx", "HelpPage.Html", "23", "3");
        dtRights.Rows.Add("201730", "HR", "Report", "HRR024", "PF Acquaintance Report", "PF_Acquain_Report.aspx", "HelpPage.Html", "24", "3");
        dtRights.Rows.Add("201731", "HR", "Report", "HRR025", "CL Salary Statement Report", "CL_Salary_Stmnt.aspx", "HelpPage.Html", "25", "3");
        dtRights.Rows.Add("201732", "HR", "Report", "HRR026", "Staff Loan Details Report", "Staff_LoanDetailsReport.aspx", "HelpPage.Html", "26", "3");
        dtRights.Rows.Add("201733", "HR", "Report", "HRR027", "Newly Joined & Relieved Staff Details", "JoinedRelievedStaffDetails.aspx", "HelpPage.Html", "27", "3");
        dtRights.Rows.Add("201734", "HR", "Report", "HRR028", "Salary Comparative Statement", "HRSalComparativeReport.aspx", "HelpPage.Html", "28", "3");
        dtRights.Rows.Add("201735", "HR", "Report", "HRR029", "Salary Comparative Report", "SalaryBill.aspx", "HelpPage.Html", "29", "3");
        dtRights.Rows.Add("201736", "HR", "Report", "HRR030", "Month Wise Staff Strength Report", "StaffCategoryWiseStrengthReport.aspx", "HelpPage.Html", "30", "3");
        dtRights.Rows.Add("201737", "HR", "Report", "HRR031", "Salary Abstract Report", "AllowanceAndDeductionReport.aspx", "HelpPage.Html", "31", "3");
       
        dtRights.Rows.Add("201740", "HR", "Report", "HRR032", "Quarterly Report", "Quaterly_Report.aspx", "HelpPage.Html", "32", "3");
        dtRights.Rows.Add("201741", "HR", "Report", "HRR033", "Department Wise Attendance Report", "DepartmentWise_attendance_Report.aspx", "HelpPage.Html", "33", "3");
        dtRights.Rows.Add("201743", "HR", "Report", "HRR034", "Bank And Cash Report", "BankAndCashReport.aspx", "HelpPage.Html", "34", "3");
        dtRights.Rows.Add("201745", "HR", "Report", "HRR035", "Compensation Leave Reort", "CompensationReport.aspx", "HelpPage.Html", "35", "3");
        dtRights.Rows.Add("201747", "HR", "Report", "HRR036", "Full and Final Settlement", "fullandfinalsettlement.aspx", "HelpPage.Html", "36", "3");
        return dtRights;
    }

    protected void grdhrmenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            // e.Row.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";
        }
    }

    protected void grdhrmenu_databound(object sender, EventArgs e)
    {
        try
        {
            for (int i = grdhrmenu.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = grdhrmenu.Rows[i];
                GridViewRow previousRow = grdhrmenu.Rows[i - 1];
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
        catch { }
        loadcolor();
    }

    protected void loadcolor()
    {
        for (int ik = 0; ik < grdhrmenu.Rows.Count; ik++)
        {
            Label sno = (Label)grdhrmenu.Rows[ik].Cells[0].FindControl("lblSno");
            Label hdrname = (Label)grdhrmenu.Rows[ik].Cells[1].FindControl("lblHdrName");
            Label hdrid = (Label)grdhrmenu.Rows[ik].Cells[2].FindControl("lblReportId");
            LinkButton menu = (LinkButton)grdhrmenu.Rows[ik].Cells[3].FindControl("lbPagelink");
            LinkButton help = (LinkButton)grdhrmenu.Rows[ik].Cells[4].FindControl("lbHelplink");
            if (hdrname.Text == "Master")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.ForeColor = ColorTranslator.FromHtml("#ff00ff");
            }
            if (hdrname.Text == "Operation")
            {
                sno.ForeColor = Color.Black;
                hdrname.ForeColor = Color.Black;
                hdrid.ForeColor = Color.Black;
                menu.ForeColor = Color.Black;
                help.ForeColor = Color.Black;
            }
            if (hdrname.Text == "Report")
            {
                sno.ForeColor = Color.Green;
                hdrname.ForeColor = Color.Green;
                hdrid.ForeColor = Color.Green;
                menu.ForeColor = Color.Green;
                help.ForeColor = Color.Green;
            }
        }
    }
}