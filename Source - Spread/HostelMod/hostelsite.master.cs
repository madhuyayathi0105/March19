using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;

public partial class hostelsite : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    ArrayList rights = new ArrayList();

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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Hostel'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Hostel'  order by HeaderPriority, PagePriority asc";
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
                        tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
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
        //master
        rights.Add("Hostel,Master,110001,HM001,Mess Master,HM_MessMaster.aspx,~/helpcontentpages/helpmessmaster.htm,1,1");
        rights.Add("Hostel,Master,110002,HM002,Building Master,Building_Master.aspx,~/helpcontentpages/helpbuildingmaster.htm,1,2");
        rights.Add("Hostel,Master,110003,HM003,Hostel Master,HM_HostelMaster.aspx,~/helpcontentpages/helphostelmaster.htm,1,3");
        rights.Add("Hostel,Master,110004,HM004,Session Master,HM_SessionMaster.aspx,~/helpcontentpages/helpsessionmaster.htm,1,4");
        rights.Add("Hostel,Master,110005,HM005,Menu Master,HM_MenuMaster.aspx,~/helpcontentpages/helpmenumaster.htm,1,5");
        rights.Add("Hostel,Master,110006,HM006,Menu Item Master,HM_MenuItemMaster.aspx,~/helpcontentpages/helpmenuitemmaster.htm,1,6");
        rights.Add("Hostel,Master,999026,HM007,Gym Master,GymMaster.aspx,~/helpcontentpages/helpmenuitemmaster.htm,1,7");//saranyadevi 13.3.2018

        //operation
        rights.Add("Hostel,Operation,110047,HMO001,Hostel Admission Process,Hosteladmissionprocess.aspx,~/helpcontentpages/.htm,2,6");
        rights.Add("Hostel,Operation,110007,HMO002,Hostel Registration,HT_HostelRegistration.aspx,~/helpcontentpages/helphostelregistration.htm,2,7");
        rights.Add("Hostel,Operation,110008,HMO003,Hostel Staff / Guest Registration,HT_StaffRegistration.aspx,~/helpcontentpages/helpstaffregentry.htm,2,8");
        rights.Add("Hostel,Operation,110009,HMO004,Student Mentor,CO_StudentTutor.aspx,~/helpcontentpages/helpStudentmentor.htm,2,9");
        rights.Add("Hostel,Operation,110010,HMO005,DayScholar Student / Staff Registration,Inv_Dayscholar_stud_staff.aspx,~/helpcontentpages/dayscholarstudstaff.htm,2,10");
        rights.Add("Hostel,Operation,110011,HMO006,Menu Schedule,HT_MenuSchedule.aspx,~/helpcontentpages/helpmenuschedule.htm,2,11");
        rights.Add("Hostel,Operation,110012,HMO007,Hostel  Master Attendance,HM_HostelMasterAttendance.aspx,~/helpcontentpages/helphostelmasterattendance.htm,2,12");
        rights.Add("Hostel,Operation,110013,HMO008,Mess Attendance,MessAttendance.aspx,~/helpcontentpages/messattendance.htm,2,13");
        rights.Add("Hostel,Operation,110014,HMO009,Hostel Settings,Inv_Hostel_setting.aspx,~/helpcontentpages/hostelsettings.htm,2,14");
        rights.Add("Hostel,Operation,110015,HMO010,Student Strength Status,inv_studstrenght.aspx,~/helpcontentpages/studentstrengthstatus.htm,2,15");
        rights.Add("Hostel,Operation,110016,HMO011,Daily Consumption Status,inv_daily_consumption.aspx,~/helpcontentpages/helpDailyconsumption.htm,2,16");
        rights.Add("Hostel,Operation,110017,HMO012,Daily Consumption-Item Return Entry,HT_Return_item.aspx,~/helpcontentpages/dailyconsumptionreturn.htm,2,17");
        rights.Add("Hostel,Operation,110018,HMO013,Hostel Income,HT_Income.aspx,~/helpcontentpages/hostelincome.htm,2,18");
        rights.Add("Hostel,Operation,110019,HMO014,Hostel Expenses,HM_Expanses.aspx,~/helpcontentpages/hostelexpances.htm,2,19");
        rights.Add("Hostel,Operation,110020,HMO015,Rebate,HM_RebateMaster.aspx,~/helpcontentpages/rebate.htm,2,20");
        rights.Add("Hostel,Operation,110021,HMO016,Student Rebate Details,HM_StudentRebateDetailNew.aspx,~/helpcontentpages/studentrebatedetails.htm,2,21");
        rights.Add("Hostel,Operation,110022,HMO017,Student Additional Collection,HM_StudentAdditionalpop.aspx,~/helpcontentpages/studentaddtionalcollection.htm,2,22");
        rights.Add("Hostel,Operation,110023,HMO018,Menu Cost Master,HM_MenuCostMaster.aspx,~/helpcontentpages/menucostmaster.htm,2,23");
        rights.Add("Hostel,Operation,110024,HMO019,Individual Student Item Master,individual_student_item_master.aspx,~/helpcontentpages/.htm,2,24");
        rights.Add("Hostel,Operation,110025,HMO020,Individual Item Cost Master,indivual_item_cost_master.aspx,~/helpcontentpages/.htm,2,25");
        rights.Add("Hostel,Operation,110026,HMO021,Individual Student Item Request,indivual_student_item_request.aspx,~/helpcontentpages/.htm,2,26");
        rights.Add("Hostel,Operation,110027,HMO022,Individual Student Item Request Approval,indivual_student_item_request_approval.aspx,~/helpcontentpages/.htm,2,27");
        rights.Add("Hostel,Operation,110028,HMO023,Student Token Details,Inv_student_token_details.aspx,~/helpcontentpages/TokenEntry.htm,2,28");
        rights.Add("Hostel,Operation,110029,HMO024,Room Availability,RoomAvailability.aspx,~/helpcontentpages/roomavailability.htm,2,29");
        rights.Add("Hostel,Operation,110030,HMO025,Mess Bill Calculation,inv_mess_bill_setting.aspx,~/helpcontentpages/messbillsettings.htm,2,30");
        rights.Add("Hostel,Operation,110031,HMO026,Room Rent Master,room_rent_master.aspx,~/helpcontentpages/roomrentmaster.htm,2,31");
        rights.Add("Hostel,Operation,110045,HMO027,Purchase Menu Master,smartcardmenu.aspx,~/helpcontentpages/menuitempurchase.htm,2,32");
        rights.Add("Hostel,Operation,110042,HMO028,Guest Attendance,HM_GuestAttendance.aspx,~/helpcontentpages/guestattendance.htm,2,33");

        //saranyadevi 13.3.2018
        rights.Add("Hostel,Operation,999027,HMO029,Gym Cost Master,GymCostMaster.aspx,~/helpcontentpages/guestattendance.htm,2,34");
        rights.Add("Hostel,Operation,999028,HMO030,Gym Allotment,GymAllotment.aspx,~/helpcontentpages/guestattendance.htm,2,35");
        rights.Add("Hostel,Operation,999029,HMO031,Health Checkup Master,Health.aspx,~/helpcontentpages/guestattendance.htm,2,36");
        rights.Add("Hostel,Operation,999030,HMO032,Student Guest Attendance,Student_gustAttendance.aspx,~/helpcontentpages/guestattendance.htm,2,37");//magesh 20.3.18
        rights.Add("Hostel,Operation,999031,HMO033,Hostel Attendance Manual,Hostel_Attendance_Manual.aspx,~/helpcontentpages/guestattendance.htm,2,38");//magesh 19.4.18
        rights.Add("Hostel,Operation,999032,HMO034,Hostel Id Generation,Hostelidgeneration.aspx,~/helpcontentpages/Hostelidgeneration.htm,2,39");//magesh 23.4.18

        //report
        rights.Add("Hostel,Report,110032,HMR001,Mess Bill Report,HM_MonthlyMessBillReport.aspx,~/helpcontentpages/messbillmonthlyreport.htm,3,34");
        //rights.Add("Hostel,Report,110033,HMR002,Item History Report,HM_Hostelsupplier_report.aspx,,3,34");
        rights.Add("Hostel,Report,110034,HMR002,Supplier History Report,HM_Hostelsupplier_report.aspx,~/helpcontentpages/supplierhistory.htm,3,35");
        rights.Add("Hostel,Report,110035,HMR003,Stock Status Report,HM_Stock_Status_Report.aspx,~/helpcontentpages/Stockstatusreport.htm,3,36");
        rights.Add("Hostel,Report,110036,HMR004,Room Stock Details,~/inventoryMod/inv_StockDetails.aspx,~/helpcontentpages/Roomstockdetails.htm,3,37");
        rights.Add("Hostel,Report,110038,HMR006,Hostel Expenses & Strength Report,inv_hostelexpanses_and_strengthreport.aspx,~/helpcontentpages/hostelexpanses_and_strengthreport.htm,3,38");
        rights.Add("Hostel,Report,110039,HMR007,Hostel Absentees Attendance Report,HM_Hostelattendance_report.aspx,~/helpcontentpages/hostelabsentr.htm,3,39");
        rights.Add("Hostel,Report,110040,HMR008,Mess Monthly Consumption Report,HM_mess_monthly_consumption_report.aspx,~/helpcontentpages/monthlymessbillconsumptionreport.htm,3,40");
        rights.Add("Hostel,Report,110041,HMR009,Purchase Order Print Settings,Investorsposetting.aspx,~/helpcontentpages/purchaseorderprintsettings.htm,3,41");

        rights.Add("Hostel,Report,110043,HMR011,Mess Bill Cost Sheet,inv_messbill_cost_sheet.aspx,~/helpcontentpages/messbillcostsheet.htm,3,43");
        rights.Add("Hostel,Report,110044,HMR012,Bio Metric Attendance,Biohostel_new.aspx,~/helpcontentpages/biohostel.htm,3,44");
        rights.Add("Hostel,Report,110046,HMR013,Menu Purchase Report,smartcardmenu_report.aspx,~/helpcontentpages/purchasemenuitem.htm,3,45");
        rights.Add("Hostel,Report,110048,HMR014,Cumulative Mess Attendance Report,MessAttendance_report1.aspx,~/helpcontentpages/purchasemenuitem.htm,3,46");
        rights.Add("Hostel,Report,110049,HMR015,Hostel Attendance Manual Report,Hostel_Attendance_Manual_Report.aspx,~/helpcontentpages/purchasemenuitem.htm,3,47");//magesh 19.4.18
        rights.Add("Hostel,Report,110050,HMR016,Student Search,StudentSearch.aspx,~/helpcontentpages/purchasemenuitem.htm,3,48");//magesh 10.5.18
        //chart
        rights.Add("Hostel,Chart,110037,HMC001,Hostel Performance Report / Chart,inv_Hostel_performance_report_and_chart.aspx,~/helpcontentpages/hostelperformancechart.htm,4,46");
        for (int i = 0; i < rights.Count; i++)
        {
            string[] index = Convert.ToString(rights[i]).Split(',');
            string q1 = " if exists(select ReportName from  Security_Rights_Details where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' ) update Security_Rights_Details set HeaderName='" + index[1].ToString() + "',ReportId='" + index[3].ToString() + "',ReportName='" + index[4].ToString() + "',PageName='" + index[5].ToString() + "',HelpURL='" + index[6].ToString() + "',HeaderPriority='" + index[7].ToString() + "',PagePriority='" + index[8].ToString() + "' where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' else insert into Security_Rights_Details (ModuleName,HeaderName,Rights_Code,ReportId,ReportName,PageName,HelpURL,HeaderPriority,PagePriority) values ('" + index[0].ToString() + "','" + index[1].ToString() + "','" + index[2].ToString() + "','" + index[3].ToString() + "','" + index[4].ToString() + "','" + index[5].ToString() + "','" + index[6].ToString() + "','" + index[7].ToString() + "','" + index[8].ToString() + "')";
            int insert = da.update_method_wo_parameter(q1, "text");
        }
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
