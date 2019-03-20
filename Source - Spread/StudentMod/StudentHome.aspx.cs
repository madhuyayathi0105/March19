using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;

public partial class StudentHome : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            ViewState["PreviousPage"] = Request.UrlReferrer;
        }
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
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Student' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
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
        {
            // e.Row.BackColor = ColorTranslator.FromHtml("#FF5600");
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";
        }
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
        dtRights.Rows.Add("41521", "Student", "Master", "SM01", "Student Application Manager", "StudentNewApplicationReport.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("41532", "Student", "Master", "SM02", "Student Application Manager-A", "StudentNewApplicationReportnew.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("41522", "Student", "Master", "SM03", "Admission Process", "Commom_Selection_Process.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("41565", "Student", "Master", "SM04", "Admission Process-A", "Selection_Process.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("41570", "Student", "Master", "SM04", "Admission Selection Process", "studadmissionselection.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("41523", "Student", "Master", "SM05", "Student Report Manager", "StudentStrengthStatusReport.aspx", "HelpPage.Html", "6", "1");
        dtRights.Rows.Add("41550", "Student", "Master", "SM06", "Student Scheme Admission", "SchemeAdmission.aspx", "HelpPage.Html", "7", "1");
        dtRights.Rows.Add("41579", "Student", "Master", "SM07", "Housing Master", "HousingMaster.aspx", "HelpPage.Html", "8", "1");
        dtRights.Rows.Add("41586", "Student", "Master", "SM08", "Alumni Report New", "AlumniReport.aspx", "HelpPage.Html", "9", "1");
        dtRights.Rows.Add("41593", "Student", "Master", "SM09", "Eligibility Settings", "eligiblilysetting.aspx", "HelpPage.Html", "10", "1");//added by Deepali on 5.4.18

        dtRights.Rows.Add("41601", "Student", "Master", "SM10", "Reference Entry For Others", "Referred_Entry_ForStudent.aspx", "HelpPage.Html", "11", "1");//added by saranyadevi on 3.1.19
        //Operation
        dtRights.Rows.Add("41524", "Student", "Operation", "SO01", "Enrollment Selection", "Enrollmentselection.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("41525", "Student", "Operation", "SO02", "Admission Print Format", "AdmissionPrint.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("41526", "Student", "Operation", "SO03", "Id Card print", "IdCardPrint.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("41527", "Student", "Operation", "SO04", "Certificate Master", "CertificationMaster.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("41528", "Student", "Operation", "SO05", "Certificate Issue Return", "CertificateMasterReport.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("41529", "Student", "Operation", "SO06", "Bank Reference Number Import", "BankReferenceNoImport.aspx", "HelpPage.Html", "6", "2");
        dtRights.Rows.Add("41530", "Student", "Operation", "SO07", "Application Number Generation", "applno_generation_settings.aspx", "HelpPage.Html", "7", "2");

        dtRights.Rows.Add("41531", "Student", "Operation", "SO08", "Declaration Form", "DeclarationForm.aspx", "HelpPage.Html", "8", "2");
        // dtRights.Rows.Add("41538", "Student", "Operation", "SO009", "Student Tamil Name Import", ".aspx", "HelpPage.Html", "9", "2");
        dtRights.Rows.Add("41533", "Student", "Operation", "SO09", "Student Details", "Strengthreport.aspx", "HelpPage.Html", "9", "2");
        dtRights.Rows.Add("41534", "Student", "Operation", "SO10", "Section Allocation", "SectionAllocation.aspx", "HelpPage.Html", "10", "2");
        dtRights.Rows.Add("41535", "Student", "Operation", "SO11", "Roll Number Generation", "RollNoGeneration.aspx", "HelpPage.Html", "11", "2");
        dtRights.Rows.Add("41536", "Student", "Operation", "SO12", "Register Number Mapping", "RegNoAllocation.aspx", "HelpPage.Html", "12", "2");
        //dtRights.Rows.Add("41537", "Student", "Operation", "SO13", "Student Promotion", "IndividualStudPromotion.aspx", "HelpPage.Html", "13", "2");
        dtRights.Rows.Add("41539", "Student", "Operation", "SO14", "Student Transfer", "StudentTransfer.aspx", "HelpPage.Html", "14", "2");
        dtRights.Rows.Add("41540", "Student", "Operation", "SO15", "Student Leave Request Setting", "LeaveApplySettings.aspx", "HelpPage.Html", "15", "2");
        dtRights.Rows.Add("41541", "Student", "Operation", "SO16", "Student Leave Request", "StudentLeaveRequestOff.aspx", "HelpPage.Html", "16", "2");
        dtRights.Rows.Add("41566", "Student", "Operation", "SO17", "Student Promotion", "StudentPromotion.aspx", "HelpPage.Html", "17", "2");
        dtRights.Rows.Add("41573", "Student", "Operation", "SO18", "Convocation Registration", "ConvocationRegistration.aspx", "HelpPage.Html", "18", "2");
        dtRights.Rows.Add("41574", "Student", "Operation", "SO19", "Readmission Process", "ReAdmissionProcess.aspx", "HelpPage.Html", "19", "2");
        dtRights.Rows.Add("41577", "Student", "Operation", "SO20", "Student Mentor", "CO_StudentTutor.aspx", "HelpPage.Html", "20", "2");
        dtRights.Rows.Add("41580", "Student", "Operation", "SO21", "House Manual Allotment", "HouseAllotment.aspx", "HelpPage.Html", "21", "2");
        dtRights.Rows.Add("41581", "Student", "Operation", "SO22", "House Auto Generation", "HouseAutoGeneration.aspx", "HelpPage.Html", "22", "2");
        dtRights.Rows.Add("41583", "Student", "Operation", "SO23", "Student Tamil Name", "Studenttamilnameimport.aspx", "HelpPage.Html", "23", "2");
        //modified
        //dtRights.Rows.Add("41585", "Student", "Operation", "SO25", "Prolong Absent", "TransferRefund.aspx", "HelpPage.Html", "24", "2"); //FinanceMod/

        dtRights.Rows.Add("41584", "Student", "Operation", "SO24", "Degree Priority", "DegreePriority.aspx", "HelpPage.Html", "23", "2");
        dtRights.Rows.Add("41589", "Student", "Operation", "SO25", "Discontinue/Prolong Absent", "~/FinanceMod/TransferRefundSettgins.aspx", "HelpPage.Html", "24", "2");
        dtRights.Rows.Add("41591", "Student", "Operation", "SO26", "Admission and Application Number Generation Setting", "AdmissionNoGeneration.aspx", "HelpPage.Html", "25", "2");//delsi

        dtRights.Rows.Add("41598", "Student", "Operation", "SO27", "Student FingerPrint Registration", "Student_FingerPrint_Reg.aspx", "HelpPage.Html", "26", "2");//Added By Saranyadevi 31.7.2018


        //Reports
        // dtRights.Rows.Add("41551", "Student", "Report", "SR01", "Student Scheme Admission Report", "Scheme_Admission_Report.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("41542", "Student", "Report", "SR01", "Admission Status - Enquiry Report", "Enquiry_report.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("41543", "Student", "Report", "SR02", "Admission Status - Counselling Report", "counselling_report.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("41544", "Student", "Report", "SR03", "Admission Status - Hostler and Transport Report", "Hoster_transport_report.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("41545", "Student", "Report", "SR04", "Admission Status - DistrictWise Report", "Districtwise_report.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("41546", "Student", "Report", "SR05", "Admission Status - Reference Type Report", "referencetypewise_report.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("41547", "Student", "Report", "SR06", "Admission Status - Medium of Study Report", "medium_of_study_report.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("41548", "Student", "Report", "SR07", "Admission Status - Certificate Received Status Report", "CertificateCummulativeReport.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("41549", "Student", "Report", "SR08", "Admission Status - DayWise Admission Comparison Report", "DayWise_AdmissionComparison.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("41551", "Student", "Report", "SR09", "Admission Status Report", "admissiondetails_report.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("41561", "Student", "Report", "SR10", "Staff Children Details Report", "StaffChildren_Report.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("41562", "Student", "Report", "SR11", "Student Applied and Admitted Student Details", "Student_applied_admited_details_report.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("41563", "Student", "Report", "SR12", "Attendance Certificate Report", "Attendance_certificate.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("41567", "Student", "Report", "SR13", "Admitted Student Details Report", "Studentdetreport.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("41568", "Student", "Report", "SR14", "Student Admission Count Report", "StudAdmissionReport.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("41569", "Student", "Report", "SA15", "Student TC Issue", "StudTcIssue.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("41571", "Student", "Report", "SA16", "Student Communitywise Report", "StudentAdmittedCommunitywiseReport.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("41572", "Student", "Report", "SA17", "Student Strength Report", "StatewiseStrengthReport.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("41575", "Student", "Report", "SA18", "Admission Report", "Admission_Report.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("41576", "Student", "Report", "SA19", "Admission Details Report", "newreport.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("41582", "Student", "Report", "SA20", "Student Housing Report", "Studenthousingreport.aspx", "HelpPage.Html", "17", "3");

        dtRights.Rows.Add("41585", "Student", "Report", "SA21", "Alumni Report", "Alumni1.aspx", "HelpPage.Html", "18", "3");
        dtRights.Rows.Add("41587", "Student", "Report", "SA22", "Condonation Fees Status", "CondonationFeesStatus.aspx", "HelpPage.Html", "19", "3");
        dtRights.Rows.Add("41588", "Student", "Report", "SA23", "Student Tc Remark", "TC Remark.aspx", "HelpPage.Html", "20", "3");
        dtRights.Rows.Add("41590", "Student", "Report", "SA24", "Student Updated Details", "CertificateEntry.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("41592", "Student", "Report", "SA25", "Readmission Report", "ReAdmissionReport.aspx", "HelpPage.Html", "22", "3");
        //added by kowshika
        dtRights.Rows.Add("41594", "Student", "Report", "SA26", "Biomentric Report For Students", "dayscholarstudentreport.aspx", "HelpPage.Html", "23", "3");


        dtRights.Rows.Add("41595", "Student", "Report", "SA27", "Address Slip", "addressslip.aspx", "HelpPage.Html", "24", "3");
        //delsi23/05
        dtRights.Rows.Add("41596", "Student", "Report", "SA28", "Mark Range Analysis", "MarkrangeAnalysis.aspx", "HelpPage.Html", "25", "3");//delsi 0706
        dtRights.Rows.Add("41597", "Student", "Report", "SA29", "Subject Selected Student Report", "SubjectSelectedStudentreport.aspx", "HelpPage.Html", "26", "3");//magesh 26.7.18
        dtRights.Rows.Add("41600", "Student", "Report", "SA30", "Student HomeWork Status Report", "StudentHomeWorkrReport.aspx", "HelpPage.Html", "27", "3");
        //Student Affairs
        dtRights.Rows.Add("41552", "Student", "Student Affairs", "SA01", "Overall Religion / Communitywise Student Strength Report", "overall_religion_strngth_rpt.aspx", "HelpPage.Html", "1", "4");
        dtRights.Rows.Add("41553", "Student", "Student Affairs", "SA02", "Department Grouping", "DeptmentGrouping.aspx", "HelpPage.Html", "2", "4");
        dtRights.Rows.Add("41554", "Student", "Student Affairs", "SA03", "Students Detailed Report", "stud_detailedrpt.aspx", "HelpPage.Html", "3", "4");
        dtRights.Rows.Add("41555", "Student", "Student Affairs", "SA04", "Yearwise & Departmentwise Strength Report", "Yearwise_deptwise_strngth_rpt.aspx", "HelpPage.Html", "4", "4");
        dtRights.Rows.Add("41556", "Student", "Student Affairs", "SA05", "Languagewise Strength Report", "languagewise_stngth.aspx", "HelpPage.Html", "5", "4");
        dtRights.Rows.Add("41557", "Student", "Student Affairs", "SA06", "Schoolwise / University Student Strength Report", "schoolwise_univer_stngth_rpt.aspx", "HelpPage.Html", "6", "4");
        dtRights.Rows.Add("41558", "Student", "Student Affairs", "SA07", "University Grouping", "univgrouping.aspx", "HelpPage.Html", "7", "4");
        dtRights.Rows.Add("41559", "Student", "Student Affairs", "SA08", "Student Hall Report", "stud_hall_rpt.aspx", "HelpPage.Html", "8", "4");
        dtRights.Rows.Add("41560", "Student", "Student Affairs", "SA09", "Mother Tongue Report", "MotherTonugeStateReport.aspx", "HelpPage.Html", "9", "4");
        dtRights.Rows.Add("41564", "Student", "Student Affairs", "SA10", "Transfer Certificate Report", "school_Tc.aspx", "HelpPage.Html", "10", "4");
        dtRights.Rows.Add("41578", "Student", "Student Affairs", "SA11", "Student Mentor Report", "StudentMentorReport.aspx", "HelpPage.Html", "11", "4");
        dtRights.Rows.Add("41599", "Student", "Student Affairs", "SA12", "Student Placement Report", "Placement Details.aspx", "HelpPage.Html", "12", "4");







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
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");
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
            LinkButton help = (LinkButton)gridMenu.Rows[ik].Cells[4].FindControl("lbHelplink");
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
            if (hdrname.Text == "Student Affairs")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#6600CC");
                hdrname.ForeColor = ColorTranslator.FromHtml("#6600CC");
                hdrid.ForeColor = ColorTranslator.FromHtml("#6600CC");
                menu.ForeColor = ColorTranslator.FromHtml("#6600CC");
                help.ForeColor = ColorTranslator.FromHtml("#6600CC");
            }
        }
    }

    protected void gridMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowcount = 0;
        LinkButton lnkbtn = (LinkButton)e.CommandSource;
        if (lnkbtn.Text == "Discontinue/Prolong Absent")
        {
            Session["backbutton_value"] = "student";
            foreach (GridViewRow row in gridMenu.Rows)
            {
                LinkButton link = (LinkButton)row.FindControl("lbPagelink");
                Label lbl = (Label)row.FindControl("postbackURL");
                if (link.Text == lnkbtn.Text)
                {
                    string asdasd = lbl.Text;
                    Response.Redirect(lbl.Text);
                }
            }
        }
        else
        {
            Session["backbutton_value"] = "finance";
            foreach (GridViewRow row in gridMenu.Rows)
            {
                LinkButton link = (LinkButton)row.FindControl("lbPagelink");
                Label lbl = (Label)row.FindControl("postbackURL");
                if (link.Text == lnkbtn.Text)
                {
                    string asdasd = lbl.Text;
                    Response.Redirect(lbl.Text);
                }
            }
        }
        #region hidden

        //string argname = e.CommandSource.ToString();
        //if (e.CommandSource.ToString() == "Discontinue/Prolong Absent")
        //{
        //    foreach (GridViewRow row in gridMenu.Rows)
        //    {
        //        rowcount++;
        //        LinkButton link = (LinkButton)row.FindControl("lbPagelink");
        //        Label lbl = (Label)row.FindControl("postbackURL");
        //        if (link.Text.ToLower() == "discontinue/prolong absent")
        //        {
        //            string asdasd = lbl.Text;
        //            Response.Redirect(lbl.Text);
        //            Session["backbutton_value"] = "student";
        //        }
        //    }
        //}
        //else
        //{
        //    foreach (GridViewRow row in gridMenu.Rows)
        //    {
        //        rowcount++;
        //        LinkButton link = (LinkButton)row.FindControl("lbPagelink");
        //        Label lbl = (Label)row.FindControl("postbackURL");
        //        if (link.Text.ToLower() == e.CommandSource.ToString().ToLower())
        //        {
        //            string asdasd = lbl.Text;
        //            Response.Redirect(lbl.Text);
        //            Session["backbutton_value"] = "";
        //        }
        //    }
        //} 

        #endregion
    }
}