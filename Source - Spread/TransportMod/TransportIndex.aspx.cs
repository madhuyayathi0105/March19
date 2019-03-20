using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text;

public partial class TransportIndex : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
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
            //EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Transport' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                GdTrans.DataSource = null;
                GdTrans.DataBind();
            }
        }
        catch
        {
            GdTrans.DataSource = null;
            GdTrans.DataBind();
        }
    }
    private void BindMenuGrid(DataTable dtMenu)
    {
        GdTrans.DataSource = dtMenu;
        GdTrans.DataBind();
    }
    protected void GdTrans_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }
    protected void GdTrans_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = GdTrans.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = GdTrans.Rows[i];
                GridViewRow previousRow = GdTrans.Rows[i - 1];
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
    //private DataTable BuildTable()
    //{
    //    DataTable dtRights = new DataTable();
    //    dtRights.Columns.Add("RightsCode");
    //    dtRights.Columns.Add("Module");
    //    dtRights.Columns.Add("Header");
    //    dtRights.Columns.Add("ReportId");
    //    dtRights.Columns.Add("ReportName");
    //    dtRights.Columns.Add("PageName");
    //    dtRights.Columns.Add("HelpPage");
    //    dtRights.Columns.Add("PagePriority");
    //    dtRights.Columns.Add("HeaderPriority");


    //    //master

    //    dtRights.Rows.Add("704", "Transport", "Master", "TPM01", "Vehicle Master", "Transport_New.aspx", "HelpPage.Html", "1", "1");
    //    dtRights.Rows.Add("800", "Transport", "Master", "TPM02", "Stage Master", "StageMaster.aspx", "HelpPage.Html", "2", "1");
    //    dtRights.Rows.Add("705", "Transport", "Master", "TPM03", "Route Master", "RouteInformation.aspx", "HelpPage.Html", "3", "1");
    //    dtRights.Rows.Add("707", "Transport", "Master", "TPM04", "Cost Master", "Cost_Master.aspx", "HelpPage.Html", "4", "1");

    //    //operation
    //    dtRights.Rows.Add("706", "Transport", "Operation", "TPO01", "Driver Allotment", "Driver_Allotment.aspx", "HelpPage.Html", "1", "2");
    //    dtRights.Rows.Add("708", "Transport", "Operation", "TPO02", "Traveler Allotment", "Traveller_NewPage.aspx", "HelpPage.Html", "2", "2");
    //    dtRights.Rows.Add("709", "Transport", "Operation", "TPO03", "Vehicle Usage", "Vehicle_Usage.aspx", "HelpPage.Html", "3", "2");
    //    dtRights.Rows.Add("710", "Transport", "Operation", "TPO04", "Vehicle Monitor", "Transport_Master.aspx", "HelpPage.Html", "4", "2");
    //    dtRights.Rows.Add("7120", "Transport", "Operation", "TPO05", "Route Transfer", "Routetransfer.aspx", "HelpPage.Html", "5", "2");

    //    //Reports
    //    dtRights.Rows.Add("7121", "Transport", "Report", "TPR01", "Vehicle Vacancy Report", "Vechiclevacancyreport.aspx", "HelpPage.Html", "1", "3");
    //    dtRights.Rows.Add("7122", "Transport", "Report", "TPR02", "Drivers Information Report", "driversinformation.aspx", "HelpPage.Html", "2", "3");
    //    dtRights.Rows.Add("7123", "Transport", "Report", "TPR03", "Travellers PhotoList Report", "travellers.aspx", "HelpPage.Html", "3", "3");
    //    dtRights.Rows.Add("7124", "Transport", "Report", "TPR04", "Transport - Bus Route Details", "TransportReport.aspx", "HelpPage.Html", "4", "3");
    //    dtRights.Rows.Add("7125", "Transport", "Report", "TPR05", "Students Transport/Hostel Details", "Student Transport Report.aspx", "HelpPage.Html", "5", "3");
    //    dtRights.Rows.Add("7126", "Transport", "Report", "TPR06", "Fuel Consumption Report", "Fuel_Consumption.aspx", "HelpPage.Html", "6", "3");
    //    dtRights.Rows.Add("7127", "Transport", "Report", "TPR07", "Detailed Vehicles Report", "Vehicle_Details.aspx", "HelpPage.Html", "7", "3");
    //    dtRights.Rows.Add("7128", "Transport", "Report", "TPR08", "Transport-Detailed Report", "Transport_strength_Report.aspx", "HelpPage.Html", "8", "3");
    //    dtRights.Rows.Add("7129", "Transport", "Report", "TPR09", "Vehicle Diesel Expenses Cumulative Report", "Vehicle_expenses.aspx", "HelpPage.Html", "9", "3");
    //    dtRights.Rows.Add("7130", "Transport", "Report", "TPR10", "Vehicle Expenses Abstract", "transport_expenses.aspx", "HelpPage.Html", "10", "3");
    //    dtRights.Rows.Add("7131", "Transport", "Report", "TPR11", "Transport Fee status Report", "Transport_fees.aspx", "HelpPage.Html", "11", "3");
    //    dtRights.Rows.Add("7132", "Transport", "Report", "TPR12", "GPS Tracking Status", "Gps_tracking_status.aspx", "HelpPage.Html", "12", "3");
    //    dtRights.Rows.Add("7133", "Transport", "Report", "TPR13", "GPS Map", "map.aspx", "HelpPage.Html", "13", "3");
    //    dtRights.Rows.Add("7134", "Transport", "Report", "TPR14", "Route & Time Wise Report", "Route_Timewisereport.aspx", "HelpPage.Html", "14", "3");
    //    dtRights.Rows.Add("7135", "Transport", "Report", "TPR15", "Trip Sheet", "Vehicleusage_rpt.aspx", "HelpPage.Html", "15", "3");


    //    return dtRights;
    //}
    //private void EntryCheck()
    //{
    //    DataTable dtRights = BuildTable();
    //    try
    //    {
    //        for (int row = 0; row < dtRights.Rows.Count; row++)
    //        {
    //            StringBuilder sbQuery = new StringBuilder();
    //            string rightsCode = Convert.ToString(dtRights.Rows[row]["RightsCode"]);
    //            sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");

    //            DA.update_method_wo_parameter(sbQuery.ToString(), "Text");
    //        }
    //    }
    //    catch { }

    //}
    protected void loadcolor()
    {
        for (int ik = 0; ik < GdTrans.Rows.Count; ik++)
        {
            Label sno = (Label)GdTrans.Rows[ik].Cells[0].FindControl("lblSno");
            Label hdrname = (Label)GdTrans.Rows[ik].Cells[1].FindControl("lblHdrName");
            Label hdrid = (Label)GdTrans.Rows[ik].Cells[2].FindControl("lblReportId");
            LinkButton menu = (LinkButton)GdTrans.Rows[ik].Cells[3].FindControl("lbPagelink");
            LinkButton help = (LinkButton)GdTrans.Rows[ik].Cells[4].FindControl("lbHelplink");
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
        }
    }
}