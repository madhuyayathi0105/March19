using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;


public partial class ChartMaster : System.Web.UI.Page
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
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " )and ModuleName='Charts' order by HeaderPriority, PagePriority asc", "Text");
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {

                chartgrid.DataSource = null;
                chartgrid.DataBind();
            }
        }
        catch
        {

            chartgrid.DataSource = null;
            chartgrid.DataBind();
        }
    }

    private void BindMenuGrid(DataTable dtMenu)
    {
        chartgrid.DataSource = dtMenu;
        chartgrid.DataBind();
        loadcolor();
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
        DTRights.Rows.Add(1201, "Charts", "Charts", "C1201", "Staff Cut-Off Chart", "staffcutoffchart.aspx", "HelpUrl.html", 1, 4);
        DTRights.Rows.Add(1202, "Charts", "Charts", "C1202", "Student Attendance Chart", "attendancechart.aspx", "HelpUrl.html", 2, 4);
        DTRights.Rows.Add(1203, "Charts", "Charts", "C1203", "Degree & Departmentwise Chart", "deg_dept_chart.aspx", "HelpUrl.html", 3, 4);
        DTRights.Rows.Add(1204, "Charts", "Charts", "C1204", "Testwise & Subjectwise Chart", "test_subject_chart.aspx", "HelpUrl.html", 4, 4);
        DTRights.Rows.Add(1205, "Charts", "Charts", "C1205", "Arrear Chart", "arrearresult.aspx", "HelpUrl.html", 5, 4);
        DTRights.Rows.Add(1206, "Charts", "Charts", "C1206", "Overall Result with Internal and External Marks Comparison", "OverallResultwithinternalexternal.aspx", "HelpUrl.html", 6, 4);
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
        try
        {
            for (int ik = 0; ik < chartgrid.Rows.Count; ik++)
            {
                Label sno = (Label)chartgrid.Rows[ik].Cells[0].FindControl("lblsno");
                Label hname = (Label)chartgrid.Rows[ik].Cells[1].FindControl("lblheadername");
                Label hid = (Label)chartgrid.Rows[ik].Cells[2].FindControl("lblreportid");
                LinkButton menu = (LinkButton)chartgrid.Rows[ik].Cells[3].FindControl("lbreportname");
                HtmlAnchor help = (HtmlAnchor)chartgrid.Rows[ik].Cells[4].FindControl("lbHelp");
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





    protected void chartgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.BackColor = ColorTranslator.FromHtml("#FF5600");
        //}
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }
    protected void chartgrid_OnDataBound(object sender, EventArgs e)
    {

        try
        {
            for (int i = chartgrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = chartgrid.Rows[i];
                GridViewRow previousRow = chartgrid.Rows[i - 1];
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
        // loadcolor();
    }
}