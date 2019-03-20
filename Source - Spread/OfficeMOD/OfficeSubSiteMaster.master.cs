using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;

public partial class OfficeSubSiteMaster : System.Web.UI.MasterPage
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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Office'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Office'  order by HeaderPriority, PagePriority asc";
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
                        tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
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


        DTRights.Rows.Add(31252, "Office", "Report", "OR301", "Letter Inward/Exit Entry Report", "LetterInwardReport.aspx", "HelpUrl.html", 1, 3);
        DTRights.Rows.Add(31255, "Office", "Report", "OR302", "Alumni Report", "AlumniReport.aspx", "HelpUrl.html", 2, 3);
        DTRights.Rows.Add(613, "Office", "Report", "OR303", "Student Photo Status", "StudentPhotoStatus.aspx", "HelpUrl.html", 3, 3);
        DTRights.Rows.Add(615, "Office", "Report", "OR304", "Bonafide Report", "bonafide1.aspx", "HelpUrl.html", 4, 3);
        DTRights.Rows.Add(616, "Office", "Report", "OR305", "Parents Meet", "parents_meet.aspx", "HelpUrl.html", 5, 3);
        DTRights.Rows.Add(617, "Office", "Report", "OR306", "Certificate Issue", "certificateissues.aspx", "HelpUrl.html", 6, 3);
        DTRights.Rows.Add(6039, "Office", "Report", "OR307", "Login Count Details", "logindetails.aspx", "HelpUrl.html", 7, 3);
        DTRights.Rows.Add(6001, "Office", "Report", "OR308", "Universal Report", "About.aspx", "HelpUrl.html", 8, 3);
        DTRights.Rows.Add(6002, "Office", "Report", "OR309", "Admin Kit Report", "AdminKitReport.aspx", "HelpUrl.html", 9, 3);
        DTRights.Rows.Add(6051, "Office", "Report", "OR310", "Staff Universal Report", "StaffUniversalReport.aspx", "HelpUrl.html", 10, 3);
        DTRights.Rows.Add(6052, "Office", "Report", "OR311", "Present and Absent Count Report", "PresentnAbsentCountDetails.aspx", "HelpUrl.html", 11, 3);

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
                int check = da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch
        {
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
