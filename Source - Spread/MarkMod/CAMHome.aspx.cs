using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class CAMHome : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
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
        {
            grouporusercode = " group_code=" + group_code.Trim() + "";
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and Rights_Code not in('2002','2003' ,'2024' ,'2005','2019' ,'2022','2023','2028','2029','2038','13001','13002','13003','13009','13005','13006','13007','13008','13010','13011','13012','13013','13014','13015','13016','13017','13018','13019','13020','13021','13022','13023','13024','13025','13026','13027','13028','13029') and ModuleName='CAM' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                gridMenu.DataSource = null;
                gridMenu.DataBind();
            }
        }
        catch
        {
            gridMenu.DataSource = null;
            gridMenu.DataBind();
        }
    }
    private void BindMenuGrid(DataTable dtMenu)
    {
        gridMenu.DataSource = dtMenu;
        gridMenu.DataBind();
    }
    protected void gridMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }
    protected void gridMenu_OnDataBound(object sender, EventArgs e)
    {

        try
        {
            for (int i = gridMenu.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gridMenu.Rows[i];
                GridViewRow previousRow = gridMenu.Rows[i - 1];
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
        loadcolor();
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
        dtRights.Rows.Add("2037", "CAM", "Master", "CM01", "Criteria For Internal", "CriteriaForInternal.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("2030", "CAM", "Master", "CM02", "CAM Calculation Lock Entry", "CAM_Calculation_Lock.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("2060", "CAM", "Master", "CM03", "Internal Exam Questions Setting", "ExamQuesSettings.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("2061", "CAM", "Master", "CM04", "COThreshold Settings", "COThresholdSettings.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("2064", "CAM", "Master", "CM05", "Weightage Settings", "weightagesettings.aspx", "HelpPage.Html", "5", "1");
        //Operation
        dtRights.Rows.Add("2001", "CAM", "Operation", "CO01", "CAM Entry", "Internal.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("716", "CAM", "Operation", "CO02", "CAM Internal Mark Calculation", "Cam Internal Mark Calculation.aspx", "HelpPage.Html", "2", "2");
      //  dtRights.Rows.Add("2024", "CAM", "Operation", "CO03", "CAM Planed Mark Entry", "cam_planed_mark.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("2036", "CAM", "Operation", "CO03", "C.I.A Mark Entry", "Internalnew.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("2053", "CAM", "Operation", "CO04", "Internal Seating Arrangement", "InternalSeatingArrangement.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("2054", "CAM", "Operation", "CO05", "Invigilation", "Invigilation.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("2055", "CAM", "Operation", "CO06", "Invigilation Schedule", "InvigilationAlter.aspx", "HelpPage.Html", "6", "2");
        //----added by Deepali on 30.4.18
        dtRights.Rows.Add("2057", "CAM", "Operation", "CO07", "Internal Mark Entry", "InternalMarkEntryNew.aspx", "HelpPage.Html", "7", "2");
        dtRights.Rows.Add("2058", "CAM", "Operation", "CO08", "Laboratory Internal Mark Entry", "LabInternalMarkEntry.aspx", "HelpPage.Html", "8", "2");
        dtRights.Rows.Add("2062", "CAM", "Operation", "CO09", "CoBasedInternal Calculation", "CoBasedInternalCalculation.aspx", "HelpPage.Html", "9", "2");
     
        //---------------end 30.4.18

        //Reports
      //  dtRights.Rows.Add("2002", "CAM", "Reports", "CR01", "Faculty Performance", "FacultyPerformance.aspx", "HelpPage.Html", "1", "3");
      //  dtRights.Rows.Add("2003", "CAM", "Reports", "CR02", "Monthly and Model Examination Fine Report", "CAMfine.aspx", "HelpPage.Html", "2", "3");
       // dtRights.Rows.Add("2005", "CAM", "Reports", "CR03", "Mark Analysis for Monthly/Model Examinations Report", "MarkEntry.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("2006", "CAM", "Reports", "CR01", "CAM Subject Range Analysis Report", "CAMRange.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("2007", "CAM", "Reports", "CR02", "CAM Report", "CAMrpt.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("2008", "CAM", "Reports", "CR03", "CAT Report", "CAT.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("2009", "CAM", "Reports", "CR04", "Overall Best Performance", "overall.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("2010", "CAM", "Reports", "CR05", "CAM Result Analysis", "Resultanalysis.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("2011", "CAM", "Reports", "CR06", "Letter Format Report", "CAMLetter.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("2012", "CAM", "Reports", "CR07", "Internal Assessment Marks", "internalassessment.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("2013", "CAM", "Reports", "CR08", "Continuous Assessment Report", "CAM_Report.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("2014", "CAM", "Reports", "CR09", "Branchwise Result Analysis", "Result_Analysis_Rpt.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("2015", "CAM", "Reports", "CR10", "Student Overall CAM Report", "StudentTestReport.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("2016", "CAM", "Reports", "CR11", "Branchwise Subject Analysis", "Internal_Report.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("2017", "CAM", "Reports", "CR12", "CAM-Subjectwise Performance", "Cam_Performance_Report.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("2018", "CAM", "Reports", "CR13", "Individual Student Performance", "IndividualStudentCamReport.aspx", "HelpPage.Html", "13", "3");
       // dtRights.Rows.Add("2019", "CAM", "Reports", "CR17", "Target Report", "Target_report.aspx", "HelpPage.Html", "17", "3");
        dtRights.Rows.Add("2020", "CAM", "Reports", "CR14", "Overall College Best Performance", "Overallcamperformance.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("2021", "CAM", "Reports", "CR15", "CAM Voice Call Send", "Camvoicereport.aspx", "HelpPage.Html", "15", "3");
       // dtRights.Rows.Add("2022", "CAM", "Reports", "CR20", "CAM Report Format II", "CAMrptFormat2.aspx", "HelpPage.Html", "20", "3");
        //dtRights.Rows.Add("2023", "CAM", "Reports", "CR16", "Fail Report", "failreport.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("2025", "CAM", "Reports", "CR16", "Department Wise Performance Report", "Department_performance.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("2026", "CAM", "Reports", "CR17", "Consolidated Mark Sheet Report", "Consolidated_report.aspx", "HelpPage.Html", "17", "3");
        dtRights.Rows.Add("2027", "CAM", "Reports", "CR18", "Over All Cam Report", "OverAllCamReport.aspx", "HelpPage.Html", "18", "3");
       // dtRights.Rows.Add("2028", "CAM", "Reports", "CR25", "Cumulative Mark Report", "CummulativeMarkReport.aspx", "HelpPage.Html", "25", "3");
        //dtRights.Rows.Add("2029", "CAM", "Reports", "CR26", "Consolidate Mark Report", "ConsolidateReport.aspx", "HelpPage.Html", "26", "3");
        dtRights.Rows.Add("2031", "CAM", "Reports", "CR19", "Internal Performance Analysis Report", "performance_analysis.aspx", "HelpPage.Html", "19", "3");
        dtRights.Rows.Add("2032", "CAM", "Reports", "CR20", "Subject Analysis Report", "Subject_Analysis.aspx", "HelpPage.Html", "20", "3");
        dtRights.Rows.Add("2033", "CAM", "Reports", "CR21", "Faculty Wise Performance", "Facultywiseperformance.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("2034", "CAM", "Reports", "CR22", "Internal Result Analysis", "CamResultAnalysisi.aspx", "HelpPage.Html", "22", "3");
        dtRights.Rows.Add("2035", "CAM", "Reports", "CR23", "Department Wise Internal Exam Result Analysis", "DepartmentWiseInternaltestAnalysis.aspx", "HelpPage.Html", "23", "3");
        //dtRights.Rows.Add("2038", "CAM", "Reports", "CR32", "Yearwise Result Analysis", "YearwiseResultAnalysis.aspx", "HelpPage.Html", "32", "3");
        dtRights.Rows.Add("2039", "CAM", "Reports", "CR24", "Department Wise Result Analysis", "DegreewiseResultAnalysis.aspx", "HelpPage.Html", "24", "3");
        dtRights.Rows.Add("2041", "CAM", "Reports", "CR25", "Subjectwise Multiple Test Report", "subjectwisemultitest.aspx", "HelpPage.Html", "25", "3");
        dtRights.Rows.Add("2043", "CAM", "Reports", "CR26", "Consolidated Subject Wise Report", "Commonsubjectwise.aspx", "HelpPage.Html", "26", "3");
        dtRights.Rows.Add("2044", "CAM", "Reports", "CR27", "Overall College Faculty Wise Result Analysis Report", "facultywiseresultanalysis.aspx", "HelpPage.Html", "27", "3");
        dtRights.Rows.Add("2045", "CAM", "Reports", "CR28", "Subject Wise Test Result Analysis Report", "Subjectwise_Test_Analysis.aspx", "HelpPage.Html", "28", "3");
        dtRights.Rows.Add("2047", "CAM", "Reports", "CR29", "CAM Moderation Mark", "CAMMarksModeration.aspx", "HelpPage.Html", "29", "3");
        dtRights.Rows.Add("2048", "CAM", "Reports", "CR30", "Student Previous Mark Report ", "StudentsMarkPrevousHistory.aspx", "HelpPage.Html", "30", "3");
        dtRights.Rows.Add("2049", "CAM", "Reports", "CR31", "Individual Student Academic Performance", "IndividualStudentTestWisePerformance.aspx", "HelpPage.Html", "31", "3");
        dtRights.Rows.Add("2050", "CAM", "Reports", "CR32", "Subject Wise Test Mark Report", "SubjectWiseTestMark.aspx", "HelpPage.Html", "32", "3");
        dtRights.Rows.Add("2051", "CAM", "Reports", "CR33", "Consolidated Statement of Marks Report", "ConsolidatedStatementofMarks.aspx", "HelpPage.Html", "33", "3");
        dtRights.Rows.Add("2052", "CAM", "Reports", "CR34", "Class Wise Test Mark Statistical Analysis", "ClassWiseTestMarkStatisticalAnalysis.aspx", "HelpPage.Html", "34", "3");
        dtRights.Rows.Add("2056", "CAM", "Reports", "CR35", "Invigilation Report ", "InvigilationReport.aspx", "HelpPage.Html", "35", "3");
        dtRights.Rows.Add("2059", "CAM", "Reports", "CR36", "Internal Mark Report ", "InternalMarkReport.aspx", "HelpPage.Html", "36", "3");//Deepali 30.4.18

        //Saranyadevi29.10.2018
        dtRights.Rows.Add("999008", "CAM", "Reports", "CR37", "Course Outcomes Based Report 1 ", "Student_Performance_Report.aspx", "HelpPage.Html", "37", "3");

        dtRights.Rows.Add("999009", "CAM", "Reports", "CR38", "Course Outcomes Based Report 2 ", "Assessment_Course_Report.aspx", "HelpPage.Html", "38", "3");
        dtRights.Rows.Add("2063", "CAM", "Reports", "CR39", "Course Outcome Attainment Report", "courseoutcomeattainment.aspx", "HelpPage.Html", "39", "3");
        dtRights.Rows.Add("2065", "CAM", "Reports", "CR40", "Studentwise PO Analysis", "pocalculation.aspx", "HelpPage.Html", "40", "3");
        dtRights.Rows.Add("2066", "CAM", "Reports", "CR41", "Overall PO Chart Analysis and Report", "POSemesterwiseAnalysis.aspx", "HelpPage.Html", "41", "3");
        //SCHOOL
        //dtRights.Rows.Add("13001", "CAM", "Report Card Master", "RCM001", "Master Settings", "reportcard_mastersettings.aspx", "HelpPage.Html", "1", "4");
        //dtRights.Rows.Add("13002", "CAM", "Report Card Master", "RCM002", "Activity Settings", "reportcard_activitysettings.aspx", "HelpPage.Html", "2", "4");
        //dtRights.Rows.Add("13003", "CAM", "Report Card Operation", "RCO003", "Activity Entry", "reportcard_activityentry.aspx", "HelpPage.Html", "3", "5");
        //dtRights.Rows.Add("13009", "CAM", "Report Card Operation", "RCO004", "Remark Entry", "remarksentry.aspx", "HelpPage.Html", "4", "5");
        //dtRights.Rows.Add("13005", "CAM", "Report Card Reports", "RCR005", "Subjectwise", "subjectwise_report.aspx", "HelpPage.Html", "5", "6");
        //dtRights.Rows.Add("13006", "CAM", "Report Card Reports", "RCR006", "Cummulative Mark And Grade", "cummulativemark_and_grade.aspx", "HelpPage.Html", "6", "6");
        //dtRights.Rows.Add("13007", "CAM", "Report Card Reports", "RCR007", "Consolidated Mark And Grade Report", "consolidatemarkandgrade_report.aspx", "HelpPage.Html", "7", "6");
        //dtRights.Rows.Add("13008", "CAM", "Report Card Reports", "RCR008", "Class Register Mark And Grade Report", "classmarkregister.aspx", "HelpPage.Html", "8", "6");
        ////dtRights.Rows.Add("13010", "CAM", "Report", "SH009", "Report Card And Grade Sheet", "School_Report.aspx", "HelpPage.Html", "9", "3");

        //dtRights.Rows.Add("13011", "CAM", "Report Card Reports", "RCR009", "CBSE PREKG", "ReportCard_For_KG.aspx", "HelpPage.Html", "9", "6");
        //dtRights.Rows.Add("13012", "CAM", "Report Card Reports", "RCR010", "CBSE I-II", "ReportCard_I_To_II.aspx", "HelpPage.Html", "10", "6");
        //dtRights.Rows.Add("13013", "CAM", "Report Card Reports", "RCR011", "CBSE III-V", "ReportCard_III_To_V.aspx", "HelpPage.Html", "11", "6");
        //dtRights.Rows.Add("13014", "CAM", "Report Card Reports", "RCR012", "CBSE IX- X", "ReportCard_CBSE.aspx", "HelpPage.Html", "12", "6");
        //dtRights.Rows.Add("13015", "CAM", "Report Card Reports", "RCR013", "Matric Report Card  VI- VIII", "ReportCardVIToVIII.aspx", "HelpPage.Html", "13", "6");
        //dtRights.Rows.Add("13016", "CAM", "Report Card Reports", "RCR014", "Matric Report Card IX & X", "ReportCardMatric_IX_And_X .aspx", "HelpPage.Html", "14", "6");
        //dtRights.Rows.Add("13017", "CAM", "Report Card Reports", "RCR015", "Matric Report Card XI - XII", "ReportCard_XI_ToXII.aspx", "HelpPage.Html", "15", "6");
        //dtRights.Rows.Add("13018", "CAM", "Report Card Reports", "RCR016", "Anglo Indian", "ReportCardAngloIndian.aspx", "HelpPage.Html", "16", "6");
        //dtRights.Rows.Add("13019", "CAM", "Report Card Reports", "RCR017", "Anglo Indian Report Card Xth,XIth & XIIth", "ReportcardAngeloIndian_Xth_To_XIIth.aspx", "HelpPage.Html", "17", "6");
        //dtRights.Rows.Add("13020", "CAM", "Report Card Reports", "RCR018", "ICSE Reportcard I - V", "ReportCardICSE_I_To_V.aspx", "HelpPage.Html", "18", "6");
        //dtRights.Rows.Add("13021", "CAM", "Report Card Reports", "RCR019", "ICSE Reportcard VI - VIII", "ReportCardICSE_VIth_To_VIIIth.aspx", "HelpPage.Html", "19", "6");
        //dtRights.Rows.Add("13022", "CAM", "Report Card Reports", "RCR020", "ICSE Reportcard IX - X", "ReportCardICSE_IX_To_X.aspx", "HelpPage.Html", "20", "6");
        //dtRights.Rows.Add("13023", "CAM", "Report Card Reports", "RCR021", "ICSE Reportcard XI - XII", "ReportCardICSE_XI_To_XII.aspx", "HelpPage.Html", "21", "6");
        //dtRights.Rows.Add("13024", "CAM", "Report Card Reports", "RCR022", "Nursery PREKG", "ReportCard_Nursery_LKG_UKG .aspx", "HelpPage.Html", "22", "6");
        //dtRights.Rows.Add("13025", "CAM", "Report Card Reports", "RCR023", "Performance Report Card ICSE I To III", "PerformanceReportCardICSE I To III.aspx", "HelpPage.Html", "23", "6");
        //dtRights.Rows.Add("13026", "CAM", "Report Card Reports", "RCR024", "Performance Report Card ICSE IV - VIII", "PerformanceReportCardICSE IV - VIII.aspx", "HelpPage.Html", "24", "6");
        //dtRights.Rows.Add("13027", "CAM", "Report Card Reports", "RCR025", "Performance Report Card ICSE IX - X", "PerformanceReportCardICSE IX - X.aspx", "HelpPage.Html", "25", "6");
        //dtRights.Rows.Add("13028", "CAM", "Report Card Reports", "RCR026", "Performance Report Card ICSE XI", "PerformanceReportCardICSE XI.aspx", "HelpPage.Html", "26", "6");
        //dtRights.Rows.Add("13029", "CAM", "Report Card Reports", "RCR027", "Performance Report Card ICSE XII", "PerformanceReportCardICSE XII.aspx", "HelpPage.Html", "27", "6");

        return dtRights;
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
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "' AND ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' AND ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");

                DA.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }

    }
    protected void loadcolor()
    {
        for (int ik = 0; ik < gridMenu.Rows.Count; ik++)
        {
            Label sno = (Label)gridMenu.Rows[ik].Cells[0].FindControl("lblSno");
            Label hdrname = (Label)gridMenu.Rows[ik].Cells[1].FindControl("lblHdrName");
            Label hdrid = (Label)gridMenu.Rows[ik].Cells[2].FindControl("lblReportId");
            LinkButton menu = (LinkButton)gridMenu.Rows[ik].Cells[3].FindControl("lbPagelink");
            HtmlAnchor help = (HtmlAnchor)gridMenu.Rows[ik].Cells[4].FindControl("lbHelplink");

            if (hdrname.Text == "Master" || hdrname.Text == "Report Card Master")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.Style.Add("color", "#ff00ff");
            }
            if (hdrname.Text == "Operation" || hdrname.Text == "Report Card Operation")
            {
                sno.ForeColor = Color.Black;
                hdrname.ForeColor = Color.Black;
                hdrid.ForeColor = Color.Black;
                menu.ForeColor = Color.Black;
                help.Style.Add("color", "Black");

            }
            if (hdrname.Text == "Reports" || hdrname.Text == "Report Card Reports")
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
}