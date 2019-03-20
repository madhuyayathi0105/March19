using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
public partial class hostelindex : System.Web.UI.Page
{
    Boolean Cellclick;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ArrayList rights = new ArrayList();
    ArrayList indexcontain = new ArrayList();

    string usercode = "";
    string groupcode = "";
    string collegecode = string.Empty;
    string streamcode = string.Empty;
    string sessstream = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            string finance = d2.GetFunction(" select * from IPatchStatus where UpdatedDate <= ClientUpdatedDate and ModuleName ='Hostel' ");
            if (finance == "0")
            {
                lblalerterr.Text = "Please Update Patch File";
                alertpopwindow.Visible = true;
                return;
            }
            bindhostelindex();
        }
    }
    protected void bindhostelindex()
    {
        try
        {
            ////master
            //rights.Add("Hostel,Master,110001,HM001,Mess Master,HM_MessMaster.aspx,~/helpcontentpages/helpmessmaster.htm,1,1");
            //rights.Add("Hostel,Master,110002,HM002,Building Master,Building_Master.aspx,~/helpcontentpages/helpbuildingmaster.htm,1,2");
            //rights.Add("Hostel,Master,110003,HM003,Hostel Master,HM_HostelMaster.aspx,~/helpcontentpages/helphostelmaster.htm,1,3");
            //rights.Add("Hostel,Master,110004,HM004,Session Master,HM_SessionMaster.aspx,~/helpcontentpages/helpsessionmaster.htm,1,4");
            //rights.Add("Hostel,Master,110005,HM005,Menu Master,HM_MenuMaster.aspx,~/helpcontentpages/helpmenumaster.htm,1,5");
            //rights.Add("Hostel,Master,110006,HM006,Menu Item Master,HM_MenuItemMaster.aspx,~/helpcontentpages/helpmenuitemmaster.htm,1,6");

            ////operation
            //rights.Add("Hostel,Operation,110047,HMO001,Hostel Admission Process,Hosteladmissionprocess.aspx,~/helpcontentpages/.htm,2,6");
            //rights.Add("Hostel,Operation,110007,HMO002,Hostel Registration,HT_HostelRegistration.aspx,~/helpcontentpages/helphostelregistration.htm,2,7");
            //rights.Add("Hostel,Operation,110008,HMO003,Hostel Staff / Guest Registration,HT_StaffRegistration.aspx,~/helpcontentpages/helpstaffregentry.htm,2,8");
            //rights.Add("Hostel,Operation,110009,HMO004,Student Mentor,CO_StudentTutor.aspx,~/helpcontentpages/helpStudentmentor.htm,2,9");
            //rights.Add("Hostel,Operation,110010,HMO005,DayScholar Student / Staff Registration,Inv_Dayscholar_stud_staff.aspx,~/helpcontentpages/dayscholarstudstaff.htm,2,10");
            //rights.Add("Hostel,Operation,110011,HMO006,Menu Schedule,HT_MenuSchedule.aspx,~/helpcontentpages/helpmenuschedule.htm,2,11");
            //rights.Add("Hostel,Operation,110012,HMO007,Hostel  Master Attendance,HM_HostelMasterAttendance.aspx,~/helpcontentpages/helphostelmasterattendance.htm,2,12");
            //rights.Add("Hostel,Operation,110013,HMO008,Mess Attendance,MessAttendance.aspx,~/helpcontentpages/messattendance.htm,2,13");
            //rights.Add("Hostel,Operation,110014,HMO009,Hostel Settings,Inv_Hostel_setting.aspx,~/helpcontentpages/hostelsettings.htm,2,14");
            //rights.Add("Hostel,Operation,110015,HMO010,Student Strength Status,inv_studstrenght.aspx,~/helpcontentpages/studentstrengthstatus.htm,2,15");
            //rights.Add("Hostel,Operation,110016,HMO011,Daily Consumption Status,inv_daily_consumption.aspx,~/helpcontentpages/helpDailyconsumption.htm,2,16");
            //rights.Add("Hostel,Operation,110017,HMO012,Daily Consumption-Item Return Entry,HT_Return_item.aspx,~/helpcontentpages/dailyconsumptionreturn.htm,2,17");
            //rights.Add("Hostel,Operation,110018,HMO013,Hostel Income,HT_Income.aspx,~/helpcontentpages/hostelincome.htm,2,18");
            //rights.Add("Hostel,Operation,110019,HMO014,Hostel Expenses,HM_Expanses.aspx,~/helpcontentpages/hostelexpances.htm,2,19");
            //rights.Add("Hostel,Operation,110020,HMO015,Rebate,HM_RebateMaster.aspx,~/helpcontentpages/rebate.htm,2,20");
            //rights.Add("Hostel,Operation,110021,HMO016,Student Rebate Details,HM_StudentRebateDetailNew.aspx,~/helpcontentpages/studentrebatedetails.htm,2,21");
            //rights.Add("Hostel,Operation,110022,HMO017,Student Additional Collection,HM_StudentAdditionalpop.aspx,~/helpcontentpages/studentaddtionalcollection.htm,2,22");
            //rights.Add("Hostel,Operation,110023,HMO018,Menu Cost Master,HM_MenuCostMaster.aspx,~/helpcontentpages/menucostmaster.htm,2,23");
            //rights.Add("Hostel,Operation,110024,HMO019,Individual Student Item Master,individual_student_item_master.aspx,~/helpcontentpages/.htm,2,24");
            //rights.Add("Hostel,Operation,110025,HMO020,Individual Item Cost Master,indivual_item_cost_master.aspx,~/helpcontentpages/.htm,2,25");
            //rights.Add("Hostel,Operation,110026,HMO021,Individual Student Item Request,indivual_student_item_request.aspx,~/helpcontentpages/.htm,2,26");
            //rights.Add("Hostel,Operation,110027,HMO022,Individual Student Item Request Approval,indivual_student_item_request_approval.aspx,~/helpcontentpages/.htm,2,27");
            //rights.Add("Hostel,Operation,110028,HMO023,Student Token Details,Inv_student_token_details.aspx,~/helpcontentpages/TokenEntry.htm,2,28");
            //rights.Add("Hostel,Operation,110029,HMO024,Room Availability,RoomAvailability.aspx,~/helpcontentpages/roomavailability.htm,2,29");
            //rights.Add("Hostel,Operation,110030,HMO025,Mess Bill Calculation,inv_mess_bill_setting.aspx,~/helpcontentpages/messbillsettings.htm,2,30");
            //rights.Add("Hostel,Operation,110031,HMO026,Room Rent Master,room_rent_master.aspx,~/helpcontentpages/roomrentmaster.htm,2,31");
            //rights.Add("Hostel,Operation,110045,HMO027,Purchase Menu Master,smartcardmenu.aspx,~/helpcontentpages/menuitempurchase.htm,2,32");
            //rights.Add("Hostel,Operation,110042,HMO028,Guest Attendance,HM_GuestAttendance.aspx,~/helpcontentpages/guestattendance.htm,2,33");

            ////report
            //rights.Add("Hostel,Report,110032,HMR001,Mess Bill Report,HM_MonthlyMessBillReport.aspx,~/helpcontentpages/messbillmonthlyreport.htm,3,34");
            ////rights.Add("Hostel,Report,110033,HMR002,Item History Report,HM_Hostelsupplier_report.aspx,,3,34");
            //rights.Add("Hostel,Report,110034,HMR002,Supplier History Report,HM_Hostelsupplier_report.aspx,~/helpcontentpages/supplierhistory.htm,3,35");
            //rights.Add("Hostel,Report,110035,HMR003,Stock Status Report,HM_Stock_Status_Report.aspx,~/helpcontentpages/Stockstatusreport.htm,3,36");
            //rights.Add("Hostel,Report,110036,HMR004,Room Stock Details,inv_StockDetails.aspx,~/helpcontentpages/Roomstockdetails.htm,3,37");
            //rights.Add("Hostel,Report,110038,HMR006,Hostel Expenses & Strength Report,inv_hostelexpanses_and_strengthreport.aspx,~/helpcontentpages/hostelexpanses_and_strengthreport.htm,3,38");
            //rights.Add("Hostel,Report,110039,HMR007,Hostel Absentees Attendance Report,HM_Hostelattendance_report.aspx,~/helpcontentpages/hostelabsentr.htm,3,39");
            //rights.Add("Hostel,Report,110040,HMR008,Mess Monthly Consumption Report,HM_mess_monthly_consumption_report.aspx,~/helpcontentpages/monthlymessbillconsumptionreport.htm,3,40");
            //rights.Add("Hostel,Report,110041,HMR009,Purchase Order Print Settings,Investorsposetting.aspx,~/helpcontentpages/purchaseorderprintsettings.htm,3,41");

            //rights.Add("Hostel,Report,110043,HMR011,Mess Bill Cost Sheet,inv_messbill_cost_sheet.aspx,~/helpcontentpages/messbillcostsheet.htm,3,43");
            //rights.Add("Hostel,Report,110044,HMR012,Bio Metric Attendance,Biohostel_new.aspx,~/helpcontentpages/biohostel.htm,3,44");
            //rights.Add("Hostel,Report,110046,HMR013,Menu Purchase Report,smartcardmenu_report.aspx,~/helpcontentpages/purchasemenuitem.htm,3,45");
            //rights.Add("Hostel,Report,110048,HMR014,Cumulative Mess Attendance Report,MessAttendance_report1.aspx,~/helpcontentpages/purchasemenuitem.htm,3,46");
            ////chart
            //rights.Add("Hostel,Chart,110037,HMC001,Hostel Performance Report / Chart,inv_Hostel_performance_report_and_chart.aspx,~/helpcontentpages/hostelperformancechart.htm,4,46");
            //for (int i = 0; i < rights.Count; i++)
            //{
            //    string[] index = Convert.ToString(rights[i]).Split(',');
            //    string q1 = " if exists(select ReportName from  Security_Rights_Details where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' ) update Security_Rights_Details set HeaderName='" + index[1].ToString() + "',ReportId='" + index[3].ToString() + "',ReportName='" + index[4].ToString() + "',PageName='" + index[5].ToString() + "',HelpURL='" + index[6].ToString() + "',HeaderPriority='" + index[7].ToString() + "',PagePriority='" + index[8].ToString() + "' where ModuleName='" + index[0].ToString() + "' and Rights_Code='" + index[2].ToString() + "' else insert into Security_Rights_Details (ModuleName,HeaderName,Rights_Code,ReportId,ReportName,PageName,HelpURL,HeaderPriority,PagePriority) values ('" + index[0].ToString() + "','" + index[1].ToString() + "','" + index[2].ToString() + "','" + index[3].ToString() + "','" + index[4].ToString() + "','" + index[5].ToString() + "','" + index[6].ToString() + "','" + index[7].ToString() + "','" + index[8].ToString() + "')";
            //    int insert = d2.update_method_wo_parameter(q1, "text");
            //}
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
            }
            string q2 = " select  s.ModuleName,s.HeaderName,s.Rights_Code,s.ReportId,s.ReportName,s.PageName,s.HelpURL,s.PagePriority,s.HeaderPriority  from Security_Rights_Details s,security_user_right r where s.Rights_Code=r.rights_code " + grouporusercode + " and s.ModuleName='Hostel' order by headerpriority ,pagepriority";
            //and college_code=" + Session["collegecode"] + " 
            ds = d2.select_method_wo_parameter(q2, "text");
            DataTable dtt = new DataTable();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtt.Columns.Add("ModuleName");
                dtt.Columns.Add("HeaderName");
                dtt.Columns.Add("ReportId");
                dtt.Columns.Add("ReportName");
                dtt.Columns.Add("PageName");
                dtt.Columns.Add("HelpURL");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtt.NewRow();
                    dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["ModuleName"]);
                    dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                    dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["ReportId"]);
                    dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["ReportName"]);
                    dr[4] = Convert.ToString(ds.Tables[0].Rows[i]["PageName"]);
                    dr[5] = Convert.ToString(ds.Tables[0].Rows[i]["HelpURL"]);
                    dtt.Rows.Add(dr);
                }
                if (dtt.Rows.Count > 0)
                {
                    importgrid.DataSource = dtt;
                    importgrid.DataBind();
                    importgrid.Visible = true;
                }
                for (int ik = 0; ik < importgrid.Rows.Count; ik++)
                {
                    Label sno = (Label)importgrid.Rows[ik].Cells[0].FindControl("lbl_sno");
                    Label headername = (Label)importgrid.Rows[ik].Cells[1].FindControl("lblModul_name");
                    Label reportrid = (Label)importgrid.Rows[ik].Cells[2].FindControl("lbl_rid");
                    LinkButton menu = (LinkButton)importgrid.Rows[ik].Cells[3].FindControl("lbl_menu");
                    Label help = (Label)importgrid.Rows[ik].Cells[4].FindControl("lbl_help");
                    if (headername.Text == "Master")
                    {
                        sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        headername.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        reportrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                        help.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    }
                    if (headername.Text == "Operation")
                    {
                        sno.ForeColor = Color.Black;
                        headername.ForeColor = Color.Black;
                        reportrid.ForeColor = Color.Black;
                        menu.ForeColor = Color.Black;
                        help.ForeColor = Color.Black;
                    }
                    if (headername.Text == "Report")
                    {
                        sno.ForeColor = Color.Green;
                        headername.ForeColor = Color.Green;
                        reportrid.ForeColor = Color.Green;
                        menu.ForeColor = Color.Green;
                        help.ForeColor = Color.Green;
                    }
                }
            }
        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("i_patch_master.aspx");
    }
    protected void importgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

    }
    protected void importgrid_span(object sender, EventArgs e)
    {
        try
        {
            for (int i = importgrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = importgrid.Rows[i];
                GridViewRow previousRow = importgrid.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lblModul_name");
                    Label lnlname1 = (Label)previousRow.FindControl("lblModul_name");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
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
}