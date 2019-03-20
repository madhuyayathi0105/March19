using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text;

public partial class LibraryMod_LibraryHome : System.Web.UI.Page
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
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Library' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
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
        dtRights.Rows.Add("999037", "Library", "Master", "LM01", "Code Setting", "CodeSettings.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("999035", "Library", "Master", "LM02", "Library Master", "Library_Master.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("999036", "Library", "Master", "LM03", "Rack Master", "LibraryRackAllocation.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("999017", "Library", "Master", "LM04", "Periodicals", "periodicalmaster.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("999016", "Library", "Master", "LM05", "Inward Entries", "Inward_Entry.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("999025", "Library", "Master", "LM06", "Back Volume", "backvolume.aspx", "HelpPage.Html", "6", "1");
        dtRights.Rows.Add("999066", "Library", "Master", "LM07", "Non Book Materials", "nonbookmaterial.aspx", "HelpPage.Html", "7", "1");
        dtRights.Rows.Add("999069", "Library", "Master", "LM08", "Project Book", "projectbook.aspx", "HelpPage.Html", "8", "1");
        dtRights.Rows.Add("999067", "Library", "Master", "LM09", "Proceedings Master", "ProceedingsMaster.aspx", "HelpPage.Html", "9", "1");
        dtRights.Rows.Add("999068", "Library", "Master", "LM10", "Standard Master", "StandardMaster.aspx", "HelpPage.Html", "10", "1");
        dtRights.Rows.Add("999054", "Library", "Master", "LM11", "Library Norms", "librarynorm.aspx", "HelpPage.Html", "11", "1");
        dtRights.Rows.Add("999062", "Library", "Master", "LM12", "Rack Status Monitor", "Rack_Status_Monitor.aspx", "HelpPage.Html", "12", "1");
        dtRights.Rows.Add("999018", "Library", "Master", "LM13", "Library Card Master", "Library_Card_Master.aspx", "HelpPage.Html", "13", "1");
        dtRights.Rows.Add("999019", "Library", "Master", "LM14", "Individual Card", "individualstudent.aspx", "HelpPage.Html", "14", "1");
        dtRights.Rows.Add("999063", "Library", "Master", "LM15", "Budget Master", "budgetmaster.aspx", "HelpPage.Html", "15", "1");

        //Operation

        dtRights.Rows.Add("999043", "Library", "Operation", "LO01", "User In/Out Entry Screen", "LibraryAddScreen.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("999039", "Library", "Operation", "LO02", "Library ID Card Generation", "LibraryIdcardgeneration.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("999041", "Library", "Operation", "LO03", "Non-Member Entry", "NonMemberEntry.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("999045", "Library", "Operation", "LO04", "Book Allocation", "BooKAllocation.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("999038", "Library", "Operation", "LO05", "Periodical Entry", "PeriodicalEntry.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("999044", "Library", "Operation", "LO06", "Book Reservation", "Book_Reservation.aspx", "HelpPage.Html", "6", "2");
        dtRights.Rows.Add("999022", "Library", "Operation", "LO07", "New Book Request", "NewBookRequest.aspx", "HelpPage.Html", "7", "2");
        dtRights.Rows.Add("999047", "Library", "Operation", "LO08", "Bind Books", "BindingBooks.aspx", "HelpPage.Html", "8", "2");
        dtRights.Rows.Add("999023", "Library", "Operation", "LO09", "Bind Book CheckList", "BindingCheckList.aspx", "HelpPage.Html", "9", "2");
        dtRights.Rows.Add("999061", "Library", "Operation", "LO10", "Book Lock/Unlock", "Book_Lock_Unlock.aspx", "HelpPage.Html", "10", "2");
        dtRights.Rows.Add("999056", "Library", "Operation", "LO11", "Card Lock/Unlock", "Card_Lock_Unlock.aspx", "HelpPage.Html", "11", "2");
        dtRights.Rows.Add("999055", "Library", "Operation", "LO12", "Library Entry Correction", "library Entry Correction.aspx", "HelpPage.Html", "12", "2");
        dtRights.Rows.Add("999051", "Library", "Operation", "LO13", "No Dues Entry", "No Dues.aspx", "HelpPage.Html", "13", "2");
        dtRights.Rows.Add("999060", "Library", "Operation", "LO14", "Book Photo", "BookPhoto.aspx", "HelpPage.Html", "14", "2");
        dtRights.Rows.Add("999048", "Library", "Operation", "LO15", "News Paper Entry", "News_Paper_Entry.aspx", "HelpPage.Html", "15", "2");
        dtRights.Rows.Add("999050", "Library", "Operation", "LO16", "Book Availability", "Book_Availability.aspx", "HelpPage.Html", "16", "2");
        dtRights.Rows.Add("999015", "Library", "Operation", "LO17", "Book Issue and Return", "bookissue.aspx", "HelpPage.Html", "17", "2");
        dtRights.Rows.Add("999065", "Library", "Operation", "LO18", "Subscribe", "Subscription_Subscribe.aspx", "HelpPage.Html", "18", "2");
        dtRights.Rows.Add("999901", "Library", "Operation", "LO19", "Barcode Label Generation", "BarcodeGeneration.aspx", "HelpPage.Html", "19", "2");
        dtRights.Rows.Add("999070", "Library", "Operation", "LO20", "Library Notification", "Library_Notification.aspx", "HelpPage.Html", "20", "2");
        dtRights.Rows.Add("999071", "Library", "Operation", "LO21", "Rack Allocation With CallNo", "Rack_Allocation_with_Call_No.aspx", "HelpPage.Html", "21", "2");
        dtRights.Rows.Add("999902", "Library", "Operation", "LO22", "Stock Analyser", "StockAnalyser.aspx", "HelpPage.Html", "22", "2");

        //Reports

        dtRights.Rows.Add("999042", "Library", "Report", "LR01", "User In/Out Entry Report", "UserInOutEntry.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("999049", "Library", "Report", "LR02", "Book Allocation Report", "Book Allocation Report.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("999064", "Library", "Report", "LR03", "Book Issue Return-Transaction Report", "BookIssueReturnTransactionReport.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("999021", "Library", "Report", "LR04", "Transaction Report", "TransactionReport.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("999040", "Library", "Report", "LR05", "Book Circulation Report", "book_circulation_report.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("999046", "Library", "Report", "LR06", "Invoice Report", "InvoiceReport.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("999053", "Library", "Report", "LR07", "Library Information", "LibraryInformation.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("999052", "Library", "Report", "LR08", "Transfer Report", "TransferReports.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("999013", "Library", "Report", "LR09", "Title,Author And publisherwise Report", "Title_Author_publisherwisereport.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("999033", "Library", "Report", "LR10", "Card List And Card Holders", "Card_list_and_holder.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("999024", "Library", "Report", "LR11", "Fine Report", "FineDetailsReport.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("999014", "Library", "Report", "LR12", "Book Statistic", "BookStatistic.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("999058", "Library", "Report", "LR13", "Non Book Material Report", "nonbookmaterialreport.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("999057", "Library", "Report", "LR14", "Library Books And Journal Details", "Cumulative_Books_Journal.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("999034", "Library", "Report", "LR15", "Cummulative issued Report", "cumm_issued_report.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("999020", "Library", "Report", "LR16", "Utilization Report", "UtilizationReport.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("999059", "Library", "Report", "LR17", "Journal Report", "JournalLetterReport.aspx", "HelpPage.Html", "17", "3");
        //added by kowshika
        dtRights.Rows.Add("999072", "Library", "Report", "LR18", "Individual Student Book Circulation Repor", "individualbookcirculation.aspx", "HelpPage.Html", "18", "3");
        dtRights.Rows.Add("999903", "Library", "Report", "LR18", "Stock Analyser Report", "StockAnalyserReport.aspx", "HelpPage.Html", "18", "3");
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

}