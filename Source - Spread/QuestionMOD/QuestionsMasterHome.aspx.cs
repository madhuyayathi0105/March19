using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;

public partial class QuestionsMasterHome : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    static string grouporusercode = string.Empty;
    DataSet ds = new DataSet();
    string qry = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
            }
            else
            {
                grouporusercode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!IsPostBack)
            {
                //InsertQuestionSettings();
                ds = new DataSet();
                qry = "select srd.ModuleName ,srd.HeaderName ,srd.Rights_Code ,srd.ReportId ,srd.ReportName ,srd.PageName ,srd.HelpURL  from Security_Rights_Details srd,security_user_right sur where sur.rights_code=srd.Rights_Code " + grouporusercode + "  and ModuleName='Question' order by HeaderPriority, PagePriority asc";
                //ds = d2.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Question' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 

                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    BindQuestionGrid(ds.Tables[0]);
                }
                else
                {
                    gvQuestionMenu.EmptyDataText = "Please Check Settings";
                    gvQuestionMenu.DataSource = null;
                    gvQuestionMenu.DataBind();
                    gvQuestionMenu.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            gvQuestionMenu.EmptyDataText = "Please Check Settings";
            gvQuestionMenu.DataSource = null;
            gvQuestionMenu.DataBind();
            gvQuestionMenu.Visible = true;
        }
    }

    private void BindQuestionGrid(DataTable dtMenu)
    {
        if (dtMenu.Rows.Count > 0)
        {
            gvQuestionMenu.EmptyDataText = "Please Check Settings";
            gvQuestionMenu.DataSource = dtMenu;
            gvQuestionMenu.DataBind();
            gvQuestionMenu.Visible = true;
        }
        else
        {
            gvQuestionMenu.EmptyDataText = "Please Check Settings";
            gvQuestionMenu.DataSource = null;
            gvQuestionMenu.DataBind();
            gvQuestionMenu.Visible = true;
        }
    }

    //private DataTable QuestionsMaster()
    //{
    //    DataTable dtQuestionMaster = new DataTable();
    //    try
    //    {           
    //        dtQuestionMaster.Columns.Add("RightsCode");
    //        dtQuestionMaster.Columns.Add("Module");
    //        dtQuestionMaster.Columns.Add("Header");
    //        dtQuestionMaster.Columns.Add("ReportId");
    //        dtQuestionMaster.Columns.Add("ReportName");
    //        dtQuestionMaster.Columns.Add("PageName");
    //        dtQuestionMaster.Columns.Add("HelpPage");
    //        dtQuestionMaster.Columns.Add("PagePriority");
    //        dtQuestionMaster.Columns.Add("HeaderPriority");


    //        dtQuestionMaster.Rows.Add("801", "Question", "Master", "QM001", "Internal And External Test Creation", "Test_name.aspx", "HelpPage.Html", "1", "1");
    //        dtQuestionMaster.Rows.Add("802", "Question", "Master", "QM002", "Question Addition", "Question_addition.aspx", "HelpPage.Html", "2", "1");
    //        dtQuestionMaster.Rows.Add("803", "Question", "Master", "QM003", "Consolidated Question Paper Master", "Consolidate_question_paper_master.aspx", "HelpPage.Html", "3", "1");

    //        dtQuestionMaster.Rows.Add("804", "Question", "Operation", "QO001", "Question Paper Genarator", "Question_paper_generator.aspx", "HelpPage.Html", "1", "2");
    //        dtQuestionMaster.Rows.Add("805", "Question", "Operation", "QO002", "Question Paper Type Setting", "Question_paper_type_setting.aspx", "HelpPage.Html", "1", "2");
    //        dtQuestionMaster.Rows.Add("806", "Question", "Operation", "QO003", "Question Paper Mark Entry", "Individual_mark_entry.aspx", "HelpPage.Html", "1", "2");
    //        dtQuestionMaster.Rows.Add("807", "Question", "Operation", "QO004", "CAM Test Mapping Settings", "CAM_Calculation_Settings_New.aspx", "HelpPage.Html", "1", "2");
    //        dtQuestionMaster.Rows.Add("808", "Question", "Operation", "QO005", "CAM Test Comparision Settings", "Cam_Comparision_Settings_For_PerformanceMoniter.aspx", "HelpPage.Html", "1", "2");

    //        dtQuestionMaster.Rows.Add("809", "Question", "Report", "QR001", "Chapter And Question Wise Result Analysis Report", "ChapterAndQuestion_Wise_Result_Analysis_Reports.aspx", "HelpPage.Html", "1", "3");
    //        dtQuestionMaster.Rows.Add("810", "Question", "Report", "QR002", "Individual Student''s Chapter And Question Wise DMG Analysis Report", "Individual_Students_Chapter_Question_Wise_DMG_Analysis.aspx", "HelpPage.Html", "2", "3");
    //        dtQuestionMaster.Rows.Add("811", "Question", "Report", "QR003", "Overall Subject Wise Test Analysis Report", "Subjectwise_Test_Analysis.aspx", "HelpPage.Html", "3", "3");
    //        dtQuestionMaster.Rows.Add("812", "Question", "Report", "QR004", "Individual Student''s Test Wise Performance Analysis Report", "Individual_Students_Performance_Results_Analysis_Report.aspx", "HelpPage.Html", "4", "3");

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    return dtQuestionMaster;
    //}

    //private void InsertQuestionSettings()
    //{
    //    try
    //    {
    //        DataTable dtRights = QuestionsMaster();
    //        for (int row = 0; row < dtRights.Rows.Count; row++)
    //        {
    //            StringBuilder sbQuery = new StringBuilder();
    //            string rightsCode = Convert.ToString(dtRights.Rows[row]["RightsCode"]);
    //            sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");

    //            d2.update_method_wo_parameter(sbQuery.ToString(), "Text");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //protected void gvQuestionMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        e.Row.BackColor = ColorTranslator.FromHtml("#42adf4");
    //        e.Row.ForeColor = ColorTranslator.FromHtml("#ffffff");
    //        e.Row.BorderColor = ColorTranslator.FromHtml("#000000");
    //    }
    //}

    protected void gvQuestionMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";


    }

    protected void gvQuestionMenu_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gvQuestionMenu.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gvQuestionMenu.Rows[i];
                GridViewRow previousRow = gvQuestionMenu.Rows[i - 1];
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
        SetColor();
    }

    protected void SetColor()
    {
        for (int ik = 0; ik < gvQuestionMenu.Rows.Count; ik++)
        {
            //if (ik % 2 == 0)
            //{
            //    gvQuestionMenu.Rows[ik].BackColor = ColorTranslator.FromHtml("#8890a8");
            //}
            //else
            //{
            //    gvQuestionMenu.Rows[ik].BackColor = ColorTranslator.FromHtml("#4f437a");
            //}
            Label sno = (Label)gvQuestionMenu.Rows[ik].Cells[0].FindControl("lblSno");
            Label hdrname = (Label)gvQuestionMenu.Rows[ik].Cells[1].FindControl("lblHdrName");
            Label hdrid = (Label)gvQuestionMenu.Rows[ik].Cells[2].FindControl("lblReportId");
            LinkButton menu = (LinkButton)gvQuestionMenu.Rows[ik].Cells[3].FindControl("lbPagelink");
            LinkButton help = (LinkButton)gvQuestionMenu.Rows[ik].Cells[4].FindControl("lbHelplink");
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