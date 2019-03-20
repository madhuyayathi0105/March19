using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class HRSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
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
        string collegecode = Session["Collegecode"].ToString();
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


            string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

            if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
            {
                string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
                collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
            }
            lblcolname.Text = collegeName;

            //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

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
            EntryCheck();
            DataSet dsRights = new DataSet();
            DataTable dtOutput = new DataTable();
            DataView dvnew = new DataView();
            string SelQ = string.Empty;
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='HR'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='HR'  order by HeaderPriority, PagePriority asc";
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
                        tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab2]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab2]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    OperationList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Report'";
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
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Chart'";
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

                da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
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
        dtRights.Rows.Add("201729", "HR", "Master", "HRM011", "Individual Staff Bell Time Settings", "Staff_BellTime_Settings.aspx", "HelpPage.Html", "11", "1");

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
        dtRights.Rows.Add("201742", "HR", "Operation", "HROP013", "Additional Allowance And Deduction", "AdditionalAllowanceDeduction.aspx", "HelpPage.Html", "13", "2"); //poo 04.11.17
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
}
