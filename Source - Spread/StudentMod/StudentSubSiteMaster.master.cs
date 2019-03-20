using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class StudentSubSiteMaster : System.Web.UI.MasterPage
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

        string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

        if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
        {
            string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
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
            EntryCheck();
            DataSet dsRights = new DataSet();
            DataTable dtOutput = new DataTable();
            DataView dvnew = new DataView();
            string SelQ = string.Empty;
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Student'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Student'  order by HeaderPriority, PagePriority asc";
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
                        tabs1.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
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
                            tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;height:auto;");
                        else if (dvnew.Count > 10)
                            tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px; height:450px;");
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
                        tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab3]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab3]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ReportList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Student Affairs'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ChartList.Visible = true;
                    for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs4.Controls.Add(li);
                        tabs4.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
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
border-radius:5px;
}
ul li a:hover{
  color:lightyellow;
}
                                                </style>
                                                ";
        this.Page.Header.Controls.Add(ltr);
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
        dtRights.Rows.Add("41523", "Student", "Master", "SM04", "Student Report Manager", "StudentStrengthStatusReport.aspx", "HelpPage.Html", "6", "1");
        dtRights.Rows.Add("41550", "Student", "Master", "SM05", "Student Scheme Admission", "SchemeAdmission.aspx", "HelpPage.Html", "7", "1");
        dtRights.Rows.Add("41579", "Student", "Master", "SM06", "Housing Master", "HousingMaster.aspx", "HelpPage.Html", "8", "1");
        dtRights.Rows.Add("41586", "Student", "Master", "SM08", "Alumni Report NEw", "AlumniReport.aspx", "HelpPage.Html", "9", "1");
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
        dtRights.Rows.Add("41580", "Student", "Operation", "SO21", "House Manual Allotment", "HouseAllotment.aspx", "HelpPage.Html", "21", "2");
        dtRights.Rows.Add("41581", "Student", "Operation", "SO22", "House Auto Generation", "HouseAutoGeneration.aspx", "HelpPage.Html", "22", "2");
        dtRights.Rows.Add("41583", "Student", "Operation", "SO23", "Student Tamil Name", "Studenttamilnameimport.aspx", "HelpPage.Html", "23", "2");
        dtRights.Rows.Add("41584", "Student", "Operation", "SO24", "Degree Priority", "DegreePriority.aspx", "HelpPage.Html", "23", "2");
        //modified
        dtRights.Rows.Add("41589", "Student", "Operation", "SO25", "Discontinue/Prolong Absent", "~/FinanceMod/TransferRefundSettgins.aspx", "HelpPage.Html", "24", "2");
        dtRights.Rows.Add("41591", "Student", "Operation", "SO26", "Admission and Application Number Generation Setting", "AdmissionNoGeneration.aspx", "HelpPage.Html", "25", "2");

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
        //delsi24/05
        dtRights.Rows.Add("41596", "Student", "Report", "SA28", "Mark Range Analysis", "MarkrangeAnalysis.aspx", "HelpPage.Html", "25", "3");
        dtRights.Rows.Add("41597", "Student", "Report", "SA29", "Subject Selected Student Report", "SubjectSelectedStudentreport.aspx", "HelpPage.Html", "26", "3");//magesh 26.7.18
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
                da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
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
        Response.Redirect("~/Default.aspx", false);
    }

    protected void ImageButton3_Click(object sender, EventArgs e)
    {
        if (Session["hosteladmissionprocessrequest"] == null)
        {
            Response.Redirect("~/studentmod/StudentHome.aspx");
        }
        else
        {
            Session["hosteladmissionprocessrequest"] = null;
            Response.Redirect("~/hostelmod/Hosteladmissionprocess.aspx");
        }
    }

}
