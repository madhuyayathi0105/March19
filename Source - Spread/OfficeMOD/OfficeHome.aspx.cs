using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Drawing;
using System.Text;

public partial class Office : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
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
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            // dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Office' order by HeaderPriority, PagePriority asc", "Text");

            dsRights = DA.select_method_wo_parameter(" select ModuleName,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL,HeaderPriority, PagePriority  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where  " + grouporusercode + " ) and Modulename='Office' order by HeaderPriority, PagePriority asc", "text");
            //union select 'Office' ModuleName,'Operation' HeaderName ,'0' Rights_Code ,'OO305' ReportId ,'Smartcard Mapping' ReportName ,'Smartcard_Mapping.aspx' PageName ,'HelpUrl.html' HelpURL ,'2' HeaderPriority, '5' PagePriority
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                officegrid.DataSource = null;
                officegrid.DataBind();
            }
        }
        catch
        {
            officegrid.DataSource = null;
            officegrid.DataBind();
        }
    }

    private void BindMenuGrid(DataTable dtMenu)
    {
        officegrid.DataSource = dtMenu;
        officegrid.DataBind();
    }

    private DataTable BuildTable()
    {
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

        DTRights.Rows.Add(72800, "Office", "Master", "M301", "Building Master", "Building_Master.aspx", "HelpUrl.html", 1, 1);
        DTRights.Rows.Add(72801, "Office", "Master", "M302", "Header Column Settings - ModuleWise", "HeaderColumnSettings.aspx", "HelpUrl.html", 2, 1);
        DTRights.Rows.Add(72805, "Office", "Master", "M303", "Smartcard Mapping", "Smartcard_Mapping.aspx", "HelpUrl.html", 3, 1);
        //added by kowshi 10.05.2018
        DTRights.Rows.Add(72808, "Office", "Master", "M304", "Code Setting", "CodeSetting.aspx", "HelpUrl.html", 4, 1);
        DTRights.Rows.Add(31251, "Office", "Operation", "OO301", "Letter Inward/Exit Entry", "LetterDocumentInward.aspx", "HelpUrl.html", 1, 2);
        DTRights.Rows.Add(725, "Office", "Operation", "OO302", "Room Allocation", "Room_Allocation.aspx", "HelpUrl.html", 2, 2);
        DTRights.Rows.Add(726, "Office", "Operation", "OO303", "Staff Room Allocation", "Subject_Room_Allocation.aspx", "HelpUrl.html", 3, 2);
        DTRights.Rows.Add(72799, "Office", "Operation", "OO304", "Admin User Settings", "AdminUserSetting.aspx", "HelpUrl.html", 4, 2);
        DTRights.Rows.Add(72802, "Office", "Operation", "OO305", "Holiday Entry", "HolidayEntry.aspx", "HelpUrl.html", 5, 2);
        DTRights.Rows.Add(72803, "Office", "Operation", "OO306", "Degree Priority Settings", "DegreePriority.aspx", "HelpUrl.html", 6, 2);
        DTRights.Rows.Add(72804, "Office", "Operation", "OO307", "Department Vission,Mission Settings", "DepartmentVissionMission.aspx", "HelpUrl.html", 7, 2);
        DTRights.Rows.Add(72806, "Office", "Operation", "OO308", "Student Tamil Name Import", "Studenttamilnameimport.aspx", "HelpUrl.html", 8, 2);
        // DTRights.Rows.Add(1, "Office", "Operation", "OO305", "Smartcard Mapping", "Smartcard_Mapping.aspx", "HelpUrl.html", 5, 2);
        DTRights.Rows.Add(31252, "Office", "Report", "OR301", "Letter Inward/Exit Entry Report", "LetterInwardReport.aspx", "HelpUrl.html", 1, 3);
        DTRights.Rows.Add(31255, "Office", "Report", "OR302", "Alumni Report", "AlumniReport.aspx", "HelpUrl.html", 2, 3);
        DTRights.Rows.Add(613, "Office", "Report", "OR303", "Student Photo Status", "StudentPhotoStatus.aspx", "HelpUrl.html", 3, 3);
        DTRights.Rows.Add(615, "Office", "Report", "OR304", "Bonafide Report", "bonafide1.aspx", "HelpUrl.html", 4, 3);
        DTRights.Rows.Add(616, "Office", "Report", "OR305", "Parents Meet", "parents_meet.aspx", "HelpUrl.html", 5, 3);
        DTRights.Rows.Add(617, "Office", "Report", "OR306", "Certificate Issue", "certificateissues.aspx", "HelpUrl.html", 6, 3);
        DTRights.Rows.Add(6039, "Office", "Report", "OR307", "Login Count Details", "logindetails.aspx", "HelpUrl.html", 7, 3);
        DTRights.Rows.Add(6001, "Office", "Report", "OR308", "Universal Report", "About.aspx", "HelpUrl.html", 8, 3);
        DTRights.Rows.Add(6002, "Office", "Report", "OR309", "Admin Kit Report", "AdminKitReport.aspx", "HelpUrl.html", 9, 3);
        DTRights.Rows.Add(6051, "Office", "Report", "OR310", "Staff Universal Report", "StaffUniversalReport.aspx", "HelpUrl.html", 10, 3);//delsi28/05
        DTRights.Rows.Add(6052, "Office", "Report", "OR311", "Present and Absent Count Report", "PresentnAbsentCountDetails.aspx", "HelpUrl.html", 11, 3);//

        DTRights.Rows.Add(6053, "Office", "Placement", "Pl001", "Company Master", "Company Master.aspx", "HelpUrl.html", 1, 4);
        DTRights.Rows.Add(6054, "Office", "Placement", "Pl002", "Placement Details", "Placement Details.aspx", "HelpUrl.html", 2, 4);
        DTRights.Rows.Add(6055, "Office", "Placement", "Pl003", "Student Attendance", "Student Attendance.aspx", "HelpUrl.html", 3, 4);
        DTRights.Rows.Add(6056, "Office", "Placement", "Pl004", "Interview Selection", "Interview Selection.aspx", "HelpUrl.html", 4, 4);
        DTRights.Rows.Add(6057, "Office", "Placement", "Pl005", "Placement Report", "Placement Report.aspx", "HelpUrl.html", 5, 4);
        DTRights.Rows.Add(6058, "Office", "Placement", "Pl006", "Schedule Report", "Schedule Report.aspx", "HelpUrl.html", 6, 4);
        return DTRights;

    }

    private void EntryCheck()
    {
        DataTable DTRights = BuildTable();
        try
        {
            for (int row = 0; row < DTRights.Rows.Count; row++)
            {
                StringBuilder sbQuery = new StringBuilder();
                string rightsCode = Convert.ToString(DTRights.Rows[row]["RightsCode"]);
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(DTRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(DTRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(DTRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(DTRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(DTRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(DTRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(DTRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(DTRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(DTRights.Rows[row]["Module"]) + "','" + Convert.ToString(DTRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(DTRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(DTRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(DTRights.Rows[row]["PageName"]) + "','" + Convert.ToString(DTRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(DTRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(DTRights.Rows[row]["HeaderPriority"]) + "')");
                int check = DA.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch
        {
        }
    }

    protected void loadcolor()
    {
        for (int ik = 0; ik < officegrid.Rows.Count; ik++)
        {
            Label sno = (Label)officegrid.Rows[ik].Cells[0].FindControl("lblsno");
            Label hname = (Label)officegrid.Rows[ik].Cells[1].FindControl("lblheadername");
            Label hid = (Label)officegrid.Rows[ik].Cells[2].FindControl("lblreportid");
            LinkButton menu = (LinkButton)officegrid.Rows[ik].Cells[3].FindControl("lbreportname");
            LinkButton help = (LinkButton)officegrid.Rows[ik].Cells[4].FindControl("lbHelp");
            if (hname.Text == "Master")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.ForeColor = ColorTranslator.FromHtml("#ff00ff");
            }
            if (hname.Text == "Operation")
            {
                sno.ForeColor = Color.Black;
                hname.ForeColor = Color.Black;
                hid.ForeColor = Color.Black;
                menu.ForeColor = Color.Black;
                help.ForeColor = Color.Black;
            }
            if (hname.Text == "Report")
            {
                sno.ForeColor = Color.Green;
                hname.ForeColor = Color.Green;
                hid.ForeColor = Color.Green;
                menu.ForeColor = Color.Green;
                help.ForeColor = Color.Green;
            }
            if (hname.Text == "Placement")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#985f36");
                hname.ForeColor = ColorTranslator.FromHtml("#985f36");
                hid.ForeColor = ColorTranslator.FromHtml("#985f36");
                menu.ForeColor = ColorTranslator.FromHtml("#985f36");
                help.ForeColor = ColorTranslator.FromHtml("#985f36");
            }

        }
    }
    protected void officegrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            // e.Row.BackColor = ColorTranslator.FromHtml("#FF5600");
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";
        }
    }
    protected void officegrid_OnDataBound(object sender, EventArgs e)
    {

        try
        {
            for (int i = officegrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = officegrid.Rows[i];
                GridViewRow previousRow = officegrid.Rows[i - 1];
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