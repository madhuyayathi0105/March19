using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;
public partial class Schedule : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;
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
        {
            grouporusercode = " group_code=" + group_code + "";
            //magesh 13/2/18
            Session["forreqstaff"] = grouporusercode;
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            //EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " )and ModuleName='Schedule' order by HeaderPriority, PagePriority asc", "Text");
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                schedulegrid.DataSource = null;
                schedulegrid.DataBind();
            }
        }
        catch
        {
            schedulegrid.DataSource = null;
            schedulegrid.DataBind();
        }
    }
    private void BindMenuGrid(DataTable dtMenu)
    {
        schedulegrid.DataSource = dtMenu;
        schedulegrid.DataBind();
    }

    private DataTable BuildTable()
    {
        string saveCOl = DA.GetFunction("select LinkValue from inssettings where College_code ='" + Session["collegecode"].ToString() + "' and LinkName ='Individual Staff Login Attendance New'").Trim();
        string linkpath = string.Empty;
        string TimeTablepath = string.Empty;
        string BatchAllocationPath = string.Empty;
        if (saveCOl == "1")
        {
            linkpath = "NewStaffAttendance.aspx";
            TimeTablepath = "TT_AlterSchedule.aspx";

        }
        else
        {
            linkpath = "newstaf.aspx";
            TimeTablepath = "Alternatesched.aspx";

        }

        DataTable DTRights = new DataTable();
        DTRights.Columns.Add("RightsCode");
        DTRights.Columns.Add("Module");
        DTRights.Columns.Add("Header");
        DTRights.Columns.Add("ReportId");
        DTRights.Columns.Add("ReportName");
        DTRights.Columns.Add("PageName");
        DTRights.Columns.Add("HelpPage");
        DTRights.Columns.Add("PagePriority");
        DTRights.Columns.Add("HeaderPriority");
        DTRights.Rows.Add("3002", "Schedule", "Reports", "S302", "Individual Class Report", "indclassreport.aspx", "HelpUrl.html", 1, 3);
        DTRights.Rows.Add("3003", "Schedule", "Reports", "S303", "Individual Staff Time Table", linkpath, "HelpUrl.html", 2, 3);
        DTRights.Rows.Add("3004", "Schedule", "Reports", "S304", "Staff Workload Report", "workload.aspx", "HelpUrl.html", 3, 3);
        DTRights.Rows.Add("3005", "Schedule", "Reports", "S305", "Time Table Changer Report", "timetablechangerreport.aspx", "HelpUrl.html", 4, 3);
        DTRights.Rows.Add("3006", "Schedule", "Reports", "S306", "Staff Subject Details Report", "StaffSubjectDetailsReport.aspx", "HelpUrl.html", 5, 3);
        DTRights.Rows.Add("3001", "Schedule", "Master", "S301", "Entry Check", TimeTablepath, "HelpUrl.html", 1, 1);
        DTRights.Rows.Add("3012", "Schedule", "Master", "S312", "Staff Time Table", "StaffTimeTable.aspx", "HelpUrl.html", 2, 1);//Deepali 11.5.18
        DTRights.Rows.Add("3013", "Schedule", "Master", "S313", "New Staff Time Table", "SimpeNewStaffTimeTable.aspx", "HelpUrl.html", 2, 1);//Rajkumar 9-7-2018
        DTRights.Rows.Add("3007", "Schedule", "Reports", "S307", "Class Timetable", "Class_Time_Table.aspx", "HelpUrl.html", 6, 3);
        DTRights.Rows.Add("3008", "Schedule", "Reports", "S308", "Staff Timetable", "Staff_Time_Table.aspx", "HelpUrl.html", 7, 3);
        DTRights.Rows.Add("3009", "Schedule", "Reports", "S309", "Room Timetable", "Room_Time_Table.aspx", "HelpUrl.html", 8, 3);
        DTRights.Rows.Add("3010", "Schedule", "Reports", "S310", "Staff Workload", "TT_StaffWorkload.aspx", "HelpUrl.html", 9, 3);
        DTRights.Rows.Add("3011", "Schedule", "Reports", "S311", "Alternate Schedule Change", "NewAlternateSchedule.aspx", "HelpUrl.html", 10, 3);//Deepali 27.3.18
        DTRights.Rows.Add("3012", "Schedule", "Reports", "S312", "Detailed Semester TimeTable Report", "DegreeWiseTimeTableReportaspx.aspx", "HelpUrl.html", 10, 3);
        return DTRights;
    }
    protected void loadcolor()
    {
        try
        {

            for (int ik = 0; ik < schedulegrid.Rows.Count; ik++)
            {
                Label sno = (Label)schedulegrid.Rows[ik].Cells[0].FindControl("lblsno");
                Label hname = (Label)schedulegrid.Rows[ik].Cells[1].FindControl("lblheadername");
                Label hid = (Label)schedulegrid.Rows[ik].Cells[2].FindControl("lblreportid");
                LinkButton menu = (LinkButton)schedulegrid.Rows[ik].Cells[3].FindControl("lbreportname");
                HtmlAnchor help = (HtmlAnchor)schedulegrid.Rows[ik].Cells[4].FindControl("lbHelp");
                if (hname.Text == "Master")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    help.Style.Add("color", "#ff00ff");
                }
                if (hname.Text == "Operation")
                {
                    sno.ForeColor = Color.Black;
                    hname.ForeColor = Color.Black;
                    hid.ForeColor = Color.Black;
                    menu.ForeColor = Color.Black;
                    help.Style.Add("color", "Black");

                }
                if (hname.Text == "Reports")
                {
                    sno.ForeColor = Color.Green;
                    hname.ForeColor = Color.Green;
                    hid.ForeColor = Color.Green;
                    menu.ForeColor = Color.Green;
                    help.Style.Add("color", "Green");
                }
                if (hname.Text == "Charts")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    hname.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    hid.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    menu.ForeColor = ColorTranslator.FromHtml("#3869fa");
                    help.Style.Add("color", "#3869fa");
                }
                if (hname.Text == "Others")
                {
                    sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    hid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                    help.Style.Add("color", "#ff00ff");
                }
            }
        }
        catch { }
    }
    protected void schedulegrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }
    protected void schedulegrid_OnDataBound(object sender, EventArgs e)
    {

        try
        {
            for (int i = schedulegrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = schedulegrid.Rows[i];
                GridViewRow previousRow = schedulegrid.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lblheadername");
                    Label lnlname1 = (Label)previousRow.FindControl("lblheadername");
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

}


