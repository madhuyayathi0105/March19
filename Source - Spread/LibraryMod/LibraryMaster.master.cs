using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;

public partial class LibraryMod_LibraryMaster : System.Web.UI.MasterPage
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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Library'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Library'  order by HeaderPriority, PagePriority asc";
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
                //dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Student Affairs'";
                //dvnew = dsRights.Tables[1].DefaultView;
                //if (dvnew.Count > 0)
                //{
                //    ChartList.Visible = true;
                //    for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                //    {
                //        HtmlGenericControl li = new HtmlGenericControl("li");
                //        tabs4.Controls.Add(li);
                //        tabs4.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
                //        HtmlGenericControl anchor = new HtmlGenericControl("a");
                //        anchor.Attributes.Add("target", "_blank");
                //        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab4]["PageName"]));
                //        anchor.InnerText = Convert.ToString(dvnew[tab4]["ReportName"]);
                //        li.Controls.Add(anchor);
                //    }
                //}
                //else
                //    ChartList.Visible = false;
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
        dtRights.Rows.Add("999014", "Library", "Report", "LR11", "Book Statistic", "BookStatistic.aspx", "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("999058", "Library", "Report", "LR12", "Non Book Material Report", "nonbookmaterialreport.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("999057", "Library", "Report", "LR13", "Library Books And Journal Details", "Cumulative_Books_Journal.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("999034", "Library", "Report", "LR14", "Cummulative issued Report", "cumm_issued_report.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("999020", "Library", "Report", "LR15", "Utilization Report", "UtilizationReport.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("999059", "Library", "Report", "LR16", "Journal Report", "JournalLetterReport.aspx", "HelpPage.Html", "16", "3");
        //added by kowshika
        dtRights.Rows.Add("999072", "Library", "Report", "LR17", "Individual Student Book Circulation Report", "individualbookcirculation.aspx", "HelpPage.Html", "17", "3");
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
                da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
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
            Response.Redirect("~/LibraryMod/LibraryHome.aspx");
        }
        else
        {
            Session["hosteladmissionprocessrequest"] = null;
            Response.Redirect("~/hostelmod/Hosteladmissionprocess.aspx");
        }
    }  


}
