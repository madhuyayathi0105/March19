using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class SubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page Load Modified By Aruna ON 04june2018  For Mcc Slowness=======================================================
        
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


        if (!IsPostBack)
        {

            try
            {
                string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

                if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
                {
                    string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
                    collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
                }
                lblcolname.Text = collegeName;


                string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
                string colornew = string.Empty;
                if (color.Trim() == "0")
                {
                    colornew = "#06d995";
                }
                else
                {
                    colornew = color;

                }


                MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                    imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                    string stfdescode = string.Empty;
                    sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                    stfdescode = da.GetFunction(sql);
                    if (stfdescode != "" && stfdescode != null)
                    {
                        string stfdesigname = string.Empty;
                        sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                        stfdesigname = da.GetFunction(sql);
                        string staffname = string.Empty;
                        sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                        staffname = da.GetFunction(sql);
                        string deptname = string.Empty;
                        sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                        deptname = da.GetFunction(sql);
                        lbslstaffname.Text = Convert.ToString(staffname);
                        lbldesignation.Text = Convert.ToString(stfdesigname);
                        lbldept.Text = Convert.ToString(deptname);
                    }
                }
                else
                {
                    string staffname = string.Empty;
                    sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                    staffname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                }
                LiteralControl ltr = new LiteralControl();
                ltr.Text = "<style type=\"text/css\" rel=\"stylesheet\">" + @"#showmenupages .has-sub ul li:hover a { color:lightyellow; background-color:" + colornew + @"; }#showmenupages .has-sub ul li a {border-bottom: 1px dotted " + colornew + @";} ul li {border-bottom: 1px dotted " + colornew + @";border-right: 1px dotted " + colornew + @";}ul li:hover { color:lightyellow; background-color:" + colornew + @"; } a:hover { color:lightyellow; }</style>";
                this.Page.Header.Controls.Add(ltr);


                // EntryCheck(); Hide By Aruna ON 04june2018  For Mcc Slowness
                DataSet dsRights = new DataSet();
                DataTable dtOutput = new DataTable();
                DataView dvnew = new DataView();
                string SelQ = string.Empty;
                SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='COE'";
                SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='COE' order by HeaderPriority, PagePriority asc";
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
            catch { }
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
                int sampu = da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch
        {
        }
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
        //master
        dtRights.Rows.Add("8001", "COE", "Master", "COEM01", "Mark Import/Entry", "mark_import.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("80001", "COE", "Master", "COEM02", "Examination Coordinators", "examstaffmaster.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("80002", "COE", "Master", "COEM03", "Exam Fee Master Settings", "ExamFeeMaster.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("80009", "COE", "Master", "COEM04", "Examination Hall Priority Master Setting", "ExamHallMaster.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("80015", "COE", "Master", "COEM05", "Invigilator Setting", "ExaminvigilatorReport.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("80072", "COE", "Master", "COEM06", "Exam Valuation Settings", "ExamValuationsettings.aspx", "HelpPage.Html", "6", "1");
        dtRights.Rows.Add("80125", "COE", "Master", "COEM07", "Question Paper Setter Master", "QuestionPaperSelector.aspx", "HelpPage.Html", "7", "1");
        //Operation
        dtRights.Rows.Add("80003", "COE", "Operation", "COEO01", "Examination Application", "exam application.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("80004", "COE", "Operation", "COEO02", "Exam Fee Paid Status", "Exam fee status.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("80005", "COE", "Operation", "COEO03", "Equal Paper Matching", "Equalpaperselection.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("80006", "COE", "Operation", "COEO04", "Examination Time Table", "ExamTimeTable.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("80010", "COE", "Operation", "COEO05", "Hall Definition", "examhalldefination.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("80011", "COE", "Operation", "COEO06", "Examination Seating Arrangement", "seatingarrange.aspx", "HelpPage.Html", "6", "2");
        dtRights.Rows.Add("80012", "COE", "Operation", "COEO07", "Examination Hall Ticket", "hallticket.aspx", "HelpPage.Html", "7", "2");
        dtRights.Rows.Add("80013", "COE", "Operation", "COEO08", "Hall Supervision", "hallsupervisor.aspx", "HelpPage.Html", "8", "2");
        dtRights.Rows.Add("80014", "COE", "Operation", "COEO09", "Question Paper Packing", "questionpackage.aspx", "HelpPage.Html", "9", "2");

        dtRights.Rows.Add("80117", "COE", "Operation", "COEO010", "Question Paper Packing With Seat Number", "COE_QuestionPaperPacking.aspx", "HelpPage.Html", "10", "2");

        dtRights.Rows.Add("80016", "COE", "Operation", "COEO11", "Attendance Sheet", "ExamattendanceReport.aspx", "HelpPage.Html", "11", "2");
        dtRights.Rows.Add("80017", "COE", "Operation", "COEO12", "Exam Attendance Entry", "ExamAttendance.aspx", "HelpPage.Html", "12", "2");
        dtRights.Rows.Add("80018", "COE", "Operation", "COEO13", "Batch Allocation For Laboratory", "BatchAllocationForPractical.aspx", "HelpPage.Html", "13", "2");
        dtRights.Rows.Add("80019", "COE", "Operation", "COEO14", "Dummy Number Generation", "dummynumberbarcode.aspx", "HelpPage.Html", "14", "2");
        dtRights.Rows.Add("80020", "COE", "Operation", "COEO15", "Dummy Number Matching", "Dummywithregno.aspx", "HelpPage.Html", "15", "2");
        dtRights.Rows.Add("80021", "COE", "Operation", "COEO16", "Evalvator Allotment", "ExamvalidatorselectionMaster.aspx", "HelpPage.Html", "16", "2");
        dtRights.Rows.Add("80022", "COE", "Operation", "COEO17", "Mark Entry", "Examvalidatormark.aspx", "HelpPage.Html", "17", "2");
        dtRights.Rows.Add("80069", "COE", "Operation", "COEO18", "Hall Ticket", "HallTicketFormateaspx.aspx", "HelpPage.Html", "18", "2");
        dtRights.Rows.Add("80107", "COE", "Operation", "COEO19", "Student Redo/Repeat Semester Registartion", "StudentsRedoBatchUpdation.aspx", "HelpPage.Html", "19", "2");
        dtRights.Rows.Add("80108", "COE", "Operation", "COEO20", "Over All GPA And CGPA Calculation", "GPA_CGPA_CalculationProcess.aspx", "HelpPage.Html", "20", "2");
        dtRights.Rows.Add("80112", "COE", "Operation", "COEO21", "Exam Time Table Generation - A", "COEExamTimeTableGeneration.aspx", "HelpPage.Html", "21", "2");

        dtRights.Rows.Add("80119", "COE", "Operation", "COEO22", "Course Wise Max and Min Credits Entry", "CourseWiseMaxandMinCriditPointEntry.aspx", "HelpPage.Html", "21", "2");
        //added by kowshika 05.04.2018
        dtRights.Rows.Add("80129", "COE", "Operation", "COEO23", "Exam Laboratory Mark Entry", "ExamDoubleValuationMarkEntry.aspx", "HelpPage.Html", "99", "2");
        dtRights.Rows.Add("80130", "COE", "Operation", "COEO24", "Moderation Apply", "ModurationApply.aspx", "HelpPage.Html", "100", "2");
        dtRights.Rows.Add("80131", "COE", "Operation", "COEO25", "SubjectWise Exam Attendance Sheet", "SubjectWiseExamAttnReport.aspx", "HelpPage.Html", "101", "2");
        dtRights.Rows.Add("80133", "COE", "Operation", "COEO26", "Moderation Mark Settings", "ModerationMarkSettings.aspx", "HelpPage.Html", "102", "2");
        dtRights.Rows.Add("80137", "COE", "Operation", "COEO27", "Exam Sub-Subject Settings", "COESubtypePartSettings.aspx", "HelpPage.Html", "103", "2");
        dtRights.Rows.Add("80136", "COE", "Operation", "COEO28", "Exam Batch Allocation", "ExamTheoryBatchAllocation.aspx", "HelpPage.Html", "104", "2");
        dtRights.Rows.Add("80138", "COE", "Operation", "COEO29", "Fee Setting for Exam Invigilation", "FeeSettingForInvigilation.aspx", "HelpPage.Html", "105", "2");
        dtRights.Rows.Add("80139", "COE", "Operation", "COEO30", "CO Based Mark Entry", "CoExternalMarkEntry.aspx", "HelpPage.Html", "106", "2");
        //Report
        dtRights.Rows.Add("80007", "COE", "Report", "COER01", "Exam Time Table Report", "ExamTimeTableReport.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("80008", "COE", "Report", "COER02", "Nominal Roll", "Nominal_Roll.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("80023", "COE", "Report", "COER03", "Mark Sheet", "ExamMarkSheetForTheoryAndPractical.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("80024", "COE", "Report", "COER04", "Provisional Result Report - Before Moderation", "provisionalresult.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("80025", "COE", "Report", "COER05", "Moderation-Internal", "Exam_moderation_forInternal.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("800251", "COE", "Report", "COER06", "Moderation-External", "Exam_Moderation.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("80026", "COE", "Report", "COER07", "Provisional Result Report - After Moderation", "aftermoderation_external.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("80027", "COE", "Report", "COER08", "Consolidated Mark Sheet", "marksheetnewreport.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("80028", "COE", "Report", "COER09", "Tabulated Mark Statement", "provisionalresult.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("80029", "COE", "Report", "COER10", "Provisional Result", "ExternalReport.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("80030", "COE", "Report", "COER11", "Common Subjectwise Result Analysis", "SubjectWiseResultAnalysis2.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("80031", "COE", "Report", "COER12", "Branchwise(Subjectwise) Result Analysis", "BranchWiseResultAnalysis.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("80032", "COE", "Report", "COER13", "Degreewise Result Analysis", "Result_Analysis_Report.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("80033", "COE", "Report", "COER14", "Range Analysis", "Range_Analysis.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("80034", "COE", "Report", "COER15", "Rank/Topper List", "student_rank_topperlist.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("80035", "COE", "Report", "COER16", "Over All Arrear List", "ExamArrearList.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("80036", "COE", "Report", "COER17", "Student Wise Arrear List", "Studentwise_Arrear_List.aspx", "HelpPage.Html", "17", "3");
        dtRights.Rows.Add("80037", "COE", "Report", "COER18", "Exam Mark Convertion", "Exam_Markconvertion.aspx", "HelpPage.Html", "18", "3");
        dtRights.Rows.Add("80038", "COE", "Report", "COER19", "Consolidated Grade Sheet", "University_mark.aspx", "HelpPage.Html", "19", "3");
        dtRights.Rows.Add("80039", "COE", "Report", "COER20", "Branchwise Result Analysis", "Result_Analysis_Rpt.aspx", "HelpPage.Html", "20", "3");
        dtRights.Rows.Add("80040", "COE", "Report", "COER21", "TMR Report", "TMR_Report2.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("80041", "COE", "Report", "COER22", "Consolidated GPA and CGPA Details", "IndividualStudentGPA.aspx", "HelpPage.Html", "22", "3");
        dtRights.Rows.Add("80042", "COE", "Report", "COER23", "Details of Candidates eligible for the award of Degree", "awardofdegree.aspx", "HelpPage.Html", "23", "3");
        dtRights.Rows.Add("80043", "COE", "Report", "COER24", "Semester Grade Sheet and Result Analysis", "newuniversityresultanalysis.aspx", "HelpPage.Html", "24", "3");
        dtRights.Rows.Add("80044", "COE", "Report", "COER25", "Comparision Of Results (Before And After Revaluation)", "Beforeandafterrevaluation.aspx", "HelpPage.Html", "25", "3");
        dtRights.Rows.Add("80045", "COE", "Report", "COER26", "Subjectwise Students List", "SubjectWiseStudent.aspx", "HelpPage.Html", "26", "3");
        dtRights.Rows.Add("80046", "COE", "Report", "COER27", "Barcode Generation", "Reg_no_wise_Barcode.aspx", "HelpPage.Html", "27", "3");
        dtRights.Rows.Add("80047", "COE", "Report", "COER28", "Student Acedemic Report", "Student_Academic_record.aspx", "HelpPage.Html", "28", "3");
        dtRights.Rows.Add("80048", "COE", "Report", "COER29", "Departmentwise Result Analysis", "DepartmentwiseResultAnalysis.aspx", "HelpPage.Html", "29", "3");
        dtRights.Rows.Add("80049", "COE", "Report", "COER30", "Moderation Analysis Report", "moderation Report.aspx", "HelpPage.Html", "30", "3");
        dtRights.Rows.Add("80050", "COE", "Report", "COER31", "Passing Board Report", "Passing_Board_Report.aspx", "HelpPage.Html", "31", "3");
        dtRights.Rows.Add("80051", "COE", "Report", "COER32", "Foil Card", "Foil_Sheet_for_Internal_External.aspx", "HelpPage.Html", "32", "3");
        dtRights.Rows.Add("80052", "COE", "Report", "COER33", "Exam Revaluation Application", "reval.aspx", "HelpPage.Html", "33", "3");
        dtRights.Rows.Add("80053", "COE", "Report", "COER34", "University Result Analysis Report", "UnivresultAnalysis.aspx", "HelpPage.Html", "34", "3");
        dtRights.Rows.Add("80054", "COE", "Report", "COER35", "Overall Percentage Analysis Report", "OverallPassPercentageAnalysis.aspx", "HelpPage.Html", "35", "3");
        dtRights.Rows.Add("80055", "COE", "Report", "COER36", "Departmentwise Arrear Statement", "Arrear_Report.aspx", "HelpPage.Html", "36", "3");
        dtRights.Rows.Add("80056", "COE", "Report", "COER37", "Third Valuation Eligible Students Report", "Third Valuation Eligible Students Report.aspx", "HelpPage.Html", "37", "3");
        dtRights.Rows.Add("80057", "COE", "Report", "COER38", "Subjectwise Arrear Status Report", "StudentwiseArrearStatus.aspx", "HelpPage.Html", "38", "3");
        dtRights.Rows.Add("80058", "COE", "Report", "COER39", "Consolidate Mark Sheet", "ConsolidateMarksheet.aspx", "HelpPage.Html", "39", "3");
        dtRights.Rows.Add("80059", "COE", "Report", "COER40", "Common Subjectwise Result Analysis", "Common_Subjectwise_Result.aspx", "HelpPage.Html", "40", "3");
        dtRights.Rows.Add("80060", "COE", "Report", "COER41", "Subjectwise Analysis Report", "Subwise_Analy_rep.aspx", "HelpPage.Html", "41", "3");
        dtRights.Rows.Add("80061", "COE", "Report", "COER42", "Performance Comparison Report", "pcreport.aspx", "HelpPage.Html", "42", "3");
        dtRights.Rows.Add("80062", "COE", "Report", "COER43", "Student Cout Wise Arrear List", "DepartmentwiseArrearReport.aspx", "HelpPage.Html", "43", "3");
        dtRights.Rows.Add("80063", "COE", "Report", "COER44", "Dummy Number Report", "DummyNumReport.aspx", "HelpPage.Html", "44", "3");
        dtRights.Rows.Add("80064", "COE", "Report", "COER45", "Over All Topper Count", "overallcollege_topper.aspx", "HelpPage.Html", "45", "3");
        dtRights.Rows.Add("80065", "COE", "Report", "COER46", "Provisional Result Publication Statement (Before Moderation)", "Resultstatementbeforemoderation.aspx", "HelpPage.Html", "46", "3");
        dtRights.Rows.Add("80066", "COE", "Report", "COER47", "Result Analysis Statement (After Revaluation)", "RevaluationAnalysis.aspx", "HelpPage.Html", "47", "3");
        dtRights.Rows.Add("80067", "COE", "Report", "COER48", "Cam & Coe Performance Analysis Report", "pareport.aspx", "HelpPage.Html", "48", "3");
        dtRights.Rows.Add("80068", "COE", "Report", "COER49", "Exam Time Table Alternate", "ExamTimeTableAlter.aspx", "HelpPage.Html", "49", "3");
        dtRights.Rows.Add("80070", "COE", "Report", "COER50", "Hall Wise Student Strength", "hallwisestudentcount.aspx", "HelpPage.Html", "50", "3");
        dtRights.Rows.Add("80071", "COE", "Report", "COER51", "Manual Mark Sheet", "Manualmarksheet.aspx", "HelpPage.Html", "51", "3");
        dtRights.Rows.Add("80073", "COE", "Report", "COER52", "Exam ICA Application", "ExamICAOnlyApplication.aspx", "HelpPage.Html", "52", "3");
        dtRights.Rows.Add("80074", "COE", "Report", "COER53", "Subject Print Priority", "Subjectprioritysettings.aspx", "HelpPage.Html", "53", "3");
        dtRights.Rows.Add("80075", "COE", "Report", "COER54", "Grade Master", "Grademastersettings.aspx", "HelpPage.Html", "54", "3");
        dtRights.Rows.Add("80076", "COE", "Report", "COER55", "MARK SHEET / Consolidated MARK SHEET", "statementofmarks.aspx", "HelpPage.Html", "55", "3");
        dtRights.Rows.Add("80079", "COE", "Report", "COER56", "Exam Valuation & Question Paper Setter", "ExamValuationSettings.aspx", "HelpPage.Html", "56", "3");
        dtRights.Rows.Add("80080", "COE", "Report", "COER57", "Exam Revaluation Request", "Revaluation_Request.aspx", "HelpPage.Html", "57", "3");
        dtRights.Rows.Add("80081", "COE", "Report", "COER58", "Exam Revaluation Mark Entry", "Revaluation_MarkEntry.aspx", "HelpPage.Html", "58", "3");
        dtRights.Rows.Add("80082", "COE", "Report", "COER59", "Publish Result Settings", "Publishresult.aspx", "HelpPage.Html", "59", "3");
        dtRights.Rows.Add("80083", "COE", "Report", "COER60", "Subjectwise Toppers", "SubjectWiseExternalRanklist.aspx", "HelpPage.Html", "60", "3");
        dtRights.Rows.Add("80084", "COE", "Report", "COER61", "Semesterwise Result Analysis", "Result_Analysis_new.aspx", "HelpPage.Html", "61", "3");
        dtRights.Rows.Add("80085", "COE", "Report", "COER62", "Degreewise Result Analysis", "DegreewiseResultAnalysis.aspx", "HelpPage.Html", "62", "3");
        dtRights.Rows.Add("80086", "COE", "Report", "COER63", "Yearwise Result Analysis", "YearwiseResultAnalysis.aspx", "HelpPage.Html", "63", "3");
        dtRights.Rows.Add("80087", "COE", "Report", "COER64", "University Result Analysis", "UniversityresultAnalysis.aspx", "HelpPage.Html", "64", "3");
        dtRights.Rows.Add("80088", "COE", "Report", "COER65", "Tab Report", "Tabdesign.aspx", "HelpPage.Html", "65", "3");
        dtRights.Rows.Add("80089", "COE", "Report", "COER66", "Chart Arrear List", "arrearresult.aspx", "HelpPage.Html", "66", "3");
        dtRights.Rows.Add("80090", "COE", "Report", "COER67", "Cumulative Report", "CumulativeReport.aspx", "HelpPage.Html", "67", "3");
        dtRights.Rows.Add("80091", "COE", "Report", "COER68", "Supplementary Exam Result Analysis", "SupplementaryTerm.aspx", "HelpPage.Html", "68", "3");
        dtRights.Rows.Add("80092", "COE", "Report", "COER69", "Subject Allotment With Mark Entry", "COE_Batchyearreport.aspx", "HelpPage.Html", "69", "3");
        dtRights.Rows.Add("80093", "COE", "Report", "COER70", "Optional Subject Creation", "OptionalSubjectsPage.aspx", "HelpPage.Html", "70", "3");
        //struck
        dtRights.Rows.Add("80094", "COE", "Report", "COER71", "Subjects Part Allocation", "SubjectPartsAllocation.aspx", "HelpPage.Html", "71", "3");
        dtRights.Rows.Add("80095", "COE", "Report", "COER72", "Student Hall Ticket Report", "Student_HT_Report.aspx", "HelpPage.Html", "72", "3");
        dtRights.Rows.Add("80096", "COE", "Report", "COER73", "Equal Subject For Choice Based Grade System", "EqualSubjectChoiceBasedGradeSystem.aspx", "HelpPage.Html", "73", "3");
        dtRights.Rows.Add("80097", "COE", "Report", "COER74", "Choice Based Grade System", "Choice Based Grade System.aspx", "HelpPage.Html", "74", "3");
        dtRights.Rows.Add("80098", "COE", "Report", "COER75", "Tab Design Report", "TabDesignNew.aspx", "HelpPage.Html", "75", "3");
        dtRights.Rows.Add("80099", "COE", "Report", "COER76", "Mark Entry For Batch Wise", "ExamvalidatormarkBatchWise.aspx", "HelpPage.Html", "76", "3");
        dtRights.Rows.Add("80100", "COE", "Report", "COER77", "Tabulated Mark Report", "TabulatedMarkResults.aspx", "HelpPage.Html", "77", "3");
        dtRights.Rows.Add("80101", "COE", "Report", "COER78", "COE Exam Mark Entry II", "MarkEntryNew.aspx", "HelpPage.Html", "78", "3");
        dtRights.Rows.Add("80102", "COE", "Report", "COER79", "COE Exam Mark Entry III", "ExamValidatorMarksNew.aspx", "HelpPage.Html", "79", "3");
        dtRights.Rows.Add("80103", "COE", "Report", "COER80", "Only ICA Mark Entry", "OnlyICAMarkEntry.aspx", "HelpPage.Html", "80", "3");
        dtRights.Rows.Add("80104", "COE", "Report", "COER81", "Condonation Fee Status Report", "CondonationFeeStatus.aspx", "HelpPage.Html", "81", "3");
        dtRights.Rows.Add("80105", "COE", "Report", "COER82", "Condonation Report", "CondonationReports.aspx", "HelpPage.Html", "82", "3");
        dtRights.Rows.Add("80106", "COE", "Report", "COER83", "Internal Mark Updation", "COEInternalMarksUpdate.aspx", "HelpPage.Html", "83", "3");
        dtRights.Rows.Add("80109", "COE", "Report", "COER84", "Overall Consolidate/Subject/Part Wise Rank List", "OverallSubjectWisePartWiseRankList.aspx", "HelpPage.Html", "84", "3");
        dtRights.Rows.Add("80110", "COE", "Report", "COER85", "Semester Exam Pass Percentage Analysis Report", "SemesterExamPassPercentageReport.aspx", "HelpPage.Html", "85", "3");
        dtRights.Rows.Add("80111", "COE", "Report", "COER86", "Exam Revaluation Report", "COEExamRevaluationReports.aspx", "HelpPage.Html", "86", "3");
        dtRights.Rows.Add("80113", "COE", "Report", "COER87", "Cover Sheet Generation Report", "COECoverSheetGeneration.aspx", "HelpPage.Html", "87", "3");
        dtRights.Rows.Add("80114", "COE", "Report", "COER88", "Question Paper Packing Report", "COEQPaperPacking.aspx", "HelpPage.Html", "88", "3");
        dtRights.Rows.Add("80115", "COE", "Report", "COER89", "Date Wise Student Strength & Summary Report", "COEDateWiseStudentStrengthReport.aspx", "HelpPage.Html", "89", "3");
        dtRights.Rows.Add("80116", "COE", "Report", "COER90", "Foil Sheet Generation Report", "COEFoilSheetGenerationReport.aspx", "HelpPage.Html", "90", "3");
        dtRights.Rows.Add("80118", "COE", "Report", "COER91", "Subject Wise Exam Eligibility", "COESubjectWiseExamEligibility.aspx", "HelpPage.Html", "91", "3");
        dtRights.Rows.Add("80120", "COE", "Report", "COER92", "Additional/Exempted Subject Entry", "AdditionalSubjectEntry.aspx", "HelpPage.Html", "92", "3");
        dtRights.Rows.Add("80121", "COE", "Report", "COER93", "University Pass Percentage Report", "UniversityPassPercentageReport.aspx", "HelpPage.Html", "93", "3");
        dtRights.Rows.Add("80122", "COE", "Report", "COER94", "Studentwise Consolidate Report", "StudentwiseConsolidateReport.aspx", "HelpPage.Html", "94", "3");
        dtRights.Rows.Add("80123", "COE", "Report", "COER95", "Studentwise Pass Percentage Report", "StudentwiseReport.aspx", "HelpPage.Html", "95", "3");
        dtRights.Rows.Add("80124", "COE", "Report", "COER96", "Revaluation and Xerox Application Report", "RevaluationXeroxApplicationReport.aspx", "HelpPage.Html", "96", "3");
        dtRights.Rows.Add("80126", "COE", "Report", "COER97", "Question Paper Setter Report", "AllotmentReport1.aspx", "HelpPage.Html", "97", "3");
        //magesh 23/1/18
        dtRights.Rows.Add("80127", "COE", "Report", "COER98", "Despatch Of Answer Packets Report", "DespatchOfAnswerPackets.aspx", "HelpPage.Html", "98", "3");
        dtRights.Rows.Add("80128", "COE", "Report", "COER99", "Individual Student Wise Marks/Grade Report", "IndividualStudentResult.aspx", "HelpPage.Html", "99", "3");
        dtRights.Rows.Add("80132", "COE", "Report", "COER100", "Supplymentary Exam Eligible Students Report", "Supplymentary Exam Eligible Students Report.aspx", "HelpPage.Html", "100", "3");
        dtRights.Rows.Add("80134", "COE", "Report", "COER101", "Course/Programme wise Classification Report", "Course_Programme_wise_Classification_Report.aspx", "HelpPage.Html", "101", "3");
        dtRights.Rows.Add("80136", "COE", "Report", "COER102", "External Internal Staff Neft Details", "External Internal Staff Neft Details.aspx", "HelpPage.Html", "102", "3");
        dtRights.Rows.Add("80140", "COE", "Report", "COER103", "Staff Performance Comparison Report", "StaffPerformanceComparisonReport.aspx", "HelpPage.Html", "103", "3");

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
        Response.Redirect("~/Default.aspx", false);
    }

}