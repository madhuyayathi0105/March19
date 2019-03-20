using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services;
using System.Text;

public partial class RequestSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    //magesh 8.3.18
     bool isnew = false;
     bool isnewstaff = false;
     bool isstafflog = false;
     bool isnews = false;
     bool isstafflogs = false;

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
        //magesh 8.3.18
        //if (Session["conformleave"] == "Yes")
        //{

        //    isnew = true;
        //}
        //if (Session["leave"] == "Yes")
        //{
        //    isstafflog = true;
        //}
        #region magesh 8.3.18
        //magesh 8.3.18
        if (Session["alters"] == "conform")
        {
            if (Session["staconform"] == "yes")
            {
                if (Session["alter_done"] == "1")
                {
                    isnews = true;
                }
            }
        }

       // if (Session["alters"] == "conform")
       // {
        if (Session["conformleave"] == "Yes")
         {
          isnew = true;
         }
        //}
        if (Session["leave"] == "Yes")
        {
            isstafflog = true;
        }
        if (Session["leave"] == "NO")
        {
            isstafflogs = true;
        }
        //if (Session["forrequest"] != null)
        //{
        //    isnewstaff = true;
        //}
#endregion
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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='request'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='request'  order by HeaderPriority, PagePriority asc";
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

        dtRights.Rows.Add("1501", "request", "Operation", "RQ001", "HierarchySetting", "HierarchySetting.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("1503", "request", "Operation", "RQ002", "Request", "Request.aspx", "HelpPage.Html", "3", "1");

        dtRights.Rows.Add("1505", "request", "Operation", "RQ003", "GatePassEntryExit", "GatePassEntryExit.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("1511", "request", "Operation", "RQ004", "Certificate Master Settings", "Certificate_setting_master.aspx", "HelpPage.Html", "7", "1");
        dtRights.Rows.Add("1512", "request", "Operation", "RQ005", "Certificate Staff Request", "CertificateRequest.aspx", "HelpPage.Html", "8", "1");
        dtRights.Rows.Add("1513", "request", "Operation", "RQ005", "CodeSettings", "CodeSettings.aspx", "HelpPage.Html", "9", "1");

        dtRights.Rows.Add("1502", "request", "Report", "RQ006", "HierarchySettingReport", "HierarchySettingReport.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("1504", "request", "Report", "RQ007", "Request Report", "Request_Report.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("1506", "request", "Report", "RQ008", "GateEntryExitReport", "GateEntryExit_Report.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("1509", "request", "Report", "RQ009", "Event Report", "EventReport.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("1510", "request", "Report", "RQ010", "GateEntryExitReport Others", "GatePassEntryExitReportOthers.aspx", "HelpPage.Html", "8", "3");
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

                int sampu = da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }

    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if (Session["Entry_Code"] != null)
        {
            string entryCode = Session["Entry_Code"].ToString();
            da.userTimeOut(entryCode);
        }
        //Session.Abandon();
        //Session.Clear();
        //Session.RemoveAll();
        //System.Web.Security.FormsAuthentication.SignOut();
        //Response.Redirect("~/Default.aspx", false);
        Session["Requestdefa"] = "1";
        if (isnew == true && isstafflog == true)
        {

            Response.Redirect("~/Default.aspx", true);
        }
        else if (isnew == false && isnews == true)
        {
            divPopupAlertContent.Visible = true;
            lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
            divPopupAlert.Visible = true;
           
            ImageButton3_Click(sender, e);
        }
        //else if (isnew == false && isnews==true)
        //{
        //    divPopupAlertContent.Visible = true;
        //    lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
        //    divPopupAlert.Visible = true;
        //}
        else if (isstafflog == false)
        {
           //Response.Redirect("~/Default.aspx", true);saranyadevi 4.4.2018
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            //Response.Redirect("~/Default.aspx", true);saranyadevi 4.4.2018
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }

    }
    #region magesh 8.3.18
    //magesh 8.3.18
    protected void ImageButton3_Click(object sender, EventArgs e)
    {

        Session["RequestHome"] = "3";
        if (isnew == true && isstafflog == true)
        {
            Response.Redirect("~/Requestmod/RequestHome.aspx", true);
        }
        else if (isnew == false  && isnews == true)
        {
            divPopupAlertContent.Visible = true;
            lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
            divPopupAlert.Visible = true;
        }
        //else if (isnew == false && isnews==true)
        //{
        //    divPopupAlertContent.Visible = true;
        //    lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
        //    divPopupAlert.Visible = true;
        //}
        else if (isstafflog == false)
        {
            Response.Redirect("~/Requestmod/RequestHome.aspx", true);
        }
        else
        {

            Response.Redirect("~/Requestmod/RequestHome.aspx", true);
        }

    }

    protected void ImageButton4_Click(object sender, EventArgs e)
    {
        Session["Requestlon"] = "2";
        if (isnew == true && isstafflog == true)
        {
            Response.Redirect("~/Default_LoginPage.aspx", true);
        }
        else if (isnew == false && isnews == true)
        {
            divPopupAlertContent.Visible = true;
            lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
            divPopupAlert.Visible = true;

          
        }
        //else if (isnew == false && isnews==true)
        //{
        //    divPopupAlertContent.Visible = true;
        //    lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
        //    divPopupAlert.Visible = true;
        //}
        else if (isstafflog == false)
        {
            Response.Redirect("~/Default_LoginPage.aspx", true);
        }
        else
        {

            Response.Redirect("~/Default_LoginPage.aspx", true);
        }
        //if (isnew == true && isstafflog == true)
        //{
        //    Response.Redirect("~/Default_LoginPage.aspx", true);
        //}
        //else if (isnew == false && isstafflog == true)
        //{
        //    divPopupAlertContent.Visible = true;
        //    lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
        //    divPopupAlert.Visible = true;
        //}
        //else if (isstafflog == false)
        //{
        //    Response.Redirect("~/Default_LoginPage.aspx", true);
        //}
        //else
        //{

        //    Response.Redirect("~/Default_LoginPage.aspx", true);
        //}

    }

    //protected void btnpopupAlertMsgCloseNEW_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //    string altertableqry = string.Empty;
    //        string altertablesched = string.Empty;
    //        if (!string.IsNullOrEmpty(Convert.ToString(Session["tbl_alter_qry"])))
    //        {
    //            altertableqry = Convert.ToString(Session["tbl_alter_qry"]);
    //            // string degree = Convert.ToString(Session["deg"]);
    //            // string seme=Convert.ToString(Session["sem"] );
    //            // string batchyear=Convert.ToString(Session["batch_year"]);
    //            // string date=Convert.ToString(Session["fromdates"]);
    //            // string sec=Convert.ToString(Session["sections"]);
    //            // string alt=Convert.ToString(Session["No_of_Alter"]);
    //            //string day=Convert.ToString(Session["getday"]);
    //            //string dys = Convert.ToString(Session["getdays"]);


    //            string subjectNumber = Convert.ToString(Session["subject_no"]);
    //            string subjecttypeNum = Convert.ToString(Session["SubjectTypeNo"]);
    //            string semester = Convert.ToString(Session["Sem"]);
    //            string batchyears = Convert.ToString(Session["Batch"]);
    //            string fromdates = Convert.ToString(Session["fromdateVal"]);
    //            string todates = Convert.ToString(Session["todateVal"]);
    //            string studsec = Convert.ToString(Session["section"]);
    //            string dayvalues = Convert.ToString(Session["dayvalue"]);
    //            string hourvalues = Convert.ToString(Session["hourVal"]);
    //            Hashtable ht = Session["has"] as Hashtable;
    //            Hashtable htvval = Session["hasvalues"] as Hashtable;
    //            Hashtable htvval1 = Session["hasvalues1"] as Hashtable;
    //            Hashtable htvval2 = Session["hasvalues2"] as Hashtable;
    //            Hashtable htvval3 = Session["hasvalues3"] as Hashtable;
    //            Hashtable htvval4 = Session["hasvalues4"] as Hashtable;
    //            Hashtable htvval5 = Session["hasvalues5"] as Hashtable;
    //            Hashtable htvval6 = Session["hasvalues6"] as Hashtable;
    //            int m = 0;
    //            int i = 0;
    //            for (int j = 0; j < ht.Count; j++)
    //            {
    //                m = j + 1;
    //                string sm = ht[m].ToString();
    //                string smt = htvval[m].ToString();
    //                string degree = htvval1[m].ToString();
    //                string seme = htvval2[m].ToString();
    //                string batchyear = htvval3[m].ToString();
    //                string date = htvval4[m].ToString();
    //                string sec = htvval6[m].ToString();
    //                string alt =htvval5[m].ToString();
    //                string[] spl1 = sm.Split(',');

    //                string[] spl2 = smt.Split(',');
    //                for (int mon = 0; mon < spl2.Count(); mon++)
    //                {
    //                    if (spl2[mon] != "''" && spl2[mon] != "")
    //                    {
    //                        string mday = spl1[mon].ToString();
    //                        string dayno = spl2[mon].ToString();

    //                        altertableqry = " update tbl_alter_schedule_Details set " + mday + "=''  where batch_year=" + batchyear + " and degree_code = " + degree + " and semester = " + seme + " and FromDate ='" + date + "'and sections='" + sec + "' and lastrec='0' and No_of_Alter='" + alt + "' and " + mday + "=" + dayno + "";

    //                        //string deleteQuery = "delete from Alternate_schedule where degree_code='" + degree + "' and semester='" + seme + "' and batch_year='" + batchyear + "' and  fromdate='" + date + "' and lastrec=0 and sections='" + sec + "' and " + mday + "=" + dayno + "";
    //                        string updateQuery = "update Alternate_schedule set " + mday + "='' where degree_code='" + degree + "' and semester='" + seme + "' and batch_year='" + batchyear + "' and  fromdate='" + date + "' and lastrec=0 and sections='" + sec + "' and " + mday + "=" + dayno + "";

    //                        int insert = da.update_method_wo_parameter(updateQuery, "Text");//delsi2603
    //                        if (subjectNumber != "" && subjecttypeNum != "" && semester != "" && batchyears != "" && fromdates != "" & todates != "")
    //                        {

    //                            string deleteQuer = "delete from subjectchooser_New where semester='" + semester + "' and subject_no='" + subjectNumber + "' and subtype_no='" + subjecttypeNum + "' and batch='" + batchyears + "' and fromdate='" + fromdates + "' and todate='" + todates + "'";
    //                            int del = da.update_method_wo_parameter(deleteQuer, "Text");
    //                            if (hourvalues != "" && dayvalues != "" && studsec != "")
    //                            {
    //                                string deleteQ = "delete from LabAlloc_New where degree_code='" + degree + "' and Semester='" + semester + "' and Batch_Year='" + batchyear + "' and Sections='" + studsec + "' and Subject_No='" + subjectNumber + "' and Day_Value='" + dayvalues + "' and  Hour_Value='" + hourvalues + "' and fdate='" + fromdates.ToString() + "' and tdate='" + todates + "'";
    //                                int delval = da.update_method_wo_parameter(deleteQ, "Text");
    //                            }
    //                        }


    //                        if (!string.IsNullOrEmpty(altertableqry))// !string.IsNullOrEmpty(altertablesched))
    //                            i = da.update_method_wo_parameter(altertableqry, "text");
    //                        if (i == 0)
    //                        {
    //                            //Btnok_Click(sender, e);
    //                        }
    //                        else
    //                        {
                              
    //                            if (insert > 0)
    //                            {

    //                                divok.Visible = true;
    //                                Lblok.Text = "Deleted Succesfully";
    //                                Session["alters"] = "";
    //                                Session["staconform"] = "";
    //                                Session["alter_done"] = "";
    //                                Session["conformleave"] = "";
    //                            }
    //                        }
    //                    }
    //                }

    //            }
    //        }
    //        //if (!string.IsNullOrEmpty(Convert.ToString(Session["tbl_alter_qry"])))
    //        //{
    //        //    altertablesched = Convert.ToString(Session["tbl_alter_qry"]);
    //        //   // string codevai = Convert.ToString(Session["code_val"]);
    //        //}
    //        //int i = 0;
    //        //if (!string.IsNullOrEmpty(altertableqry) )// !string.IsNullOrEmpty(altertablesched))
    //        //    i = da.update_method_wo_parameter(altertableqry, "text");
    //        //if (i == 0)
    //        //{
    //        //    Btnok_Click(sender, e);
    //        //}
           
    //}

    //    catch (Exception ex)
    //    {
            
    //    }
    //}

    protected void btnpopupAlertMsgCloseNEW_Click(object sender, EventArgs e)
    {
        try
        {
            string altertableqry = string.Empty;
            string altertablesched = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(Session["tbl_alter_qry"])))
            {
                altertableqry = Convert.ToString(Session["tbl_alter_qry"]);
                // string degree = Convert.ToString(Session["deg"]);
                // string seme=Convert.ToString(Session["sem"] );
                // string batchyear=Convert.ToString(Session["batch_year"]);
                // string date=Convert.ToString(Session["fromdates"]);
                // string sec=Convert.ToString(Session["sections"]);
                // string alt=Convert.ToString(Session["No_of_Alter"]);
                //string day=Convert.ToString(Session["getday"]);
                //string dys = Convert.ToString(Session["getdays"]);


                string subjectNumber = Convert.ToString(Session["subject_no"]);
                string subjecttypeNum = Convert.ToString(Session["SubjectTypeNo"]);
                string semester = Convert.ToString(Session["Sem"]);
                string batchyears = Convert.ToString(Session["Batch"]);
                string fromdates = Convert.ToString(Session["fromdateVal"]);
                string todates = Convert.ToString(Session["todateVal"]);
                string studsec = Convert.ToString(Session["section"]);
                string dayvalues = Convert.ToString(Session["dayvalue"]);
                string hourvalues = Convert.ToString(Session["hourVal"]);
                Hashtable ht = Session["has"] as Hashtable;
                Hashtable htvval = Session["hasvalues"] as Hashtable;
                Hashtable htvval1 = Session["hasvalues1"] as Hashtable;
                Hashtable htvval2 = Session["hasvalues2"] as Hashtable;
                Hashtable htvval3 = Session["hasvalues3"] as Hashtable;
                Hashtable htvval4 = Session["hasvalues4"] as Hashtable;
                Hashtable htvval5 = Session["hasvalues5"] as Hashtable;
                Hashtable htvval6 = Session["hasvalues6"] as Hashtable;
                Hashtable htvval7 = Session["hasvalues7"] as Hashtable;
                int m = 0;
                int i = 0;
                for (int j = 0; j < ht.Count; j++)
                {
                    m = j + 1;
                    string sm = ht[m].ToString();
                    string smt = htvval[m].ToString();
                    string smt1 = htvval7[m].ToString();
                    string degree = htvval1[m].ToString();
                    string seme = htvval2[m].ToString();
                    string batchyear = htvval3[m].ToString();
                    string date = htvval4[m].ToString();
                    string sec = htvval6[m].ToString();
                    string alt = htvval5[m].ToString();

                    string[] spl1 = sm.Split(',');

                    string[] spl2 = smt.Split(',');
                    string[] spl3 = smt1.Split(',');
                    for (int mon = 0; mon < spl2.Count(); mon++)
                    {
                        if (spl2[mon] != "''" && spl2[mon] != "")
                        {
                            string mday = spl1[mon].ToString();
                            string dayno = spl2[mon].ToString();
                            string daynos = "";

                            daynos = spl3[mon].ToString();
                            if (alt != "2")
                                altertableqry = " update tbl_alter_schedule_Details set " + mday + "=''  where batch_year=" + batchyear + " and degree_code = " + degree + " and semester = " + seme + " and FromDate ='" + date + "'and sections='" + sec + "' and lastrec='0' and No_of_Alter='" + alt + "' and " + mday + "=" + dayno + "";
                            else
                            {
                                altertableqry = " update tbl_alter_schedule_Details set " + mday + "=''  where batch_year=" + batchyear + " and degree_code = " + degree + " and semester = " + seme + " and FromDate ='" + date + "'and sections='" + sec + "' and lastrec='0' and No_of_Alter='" + alt + "' and " + mday + "=" + dayno + "";
                                // altertableqry =altertableqry+ " update tbl_alter_schedule_Details set " + mday + "=" + daynos + ",No_of_Alter='1'  where batch_year=" + batchyear + " and degree_code = " + degree + " and semester = " + seme + " and FromDate ='" + date + "'and sections='" + sec + "' and lastrec='0' and No_of_Alter='" + alt + "' and " + mday + "=" + dayno + "";
                            }

                            //string deleteQuery = "delete from Alternate_schedule where degree_code='" + degree + "' and semester='" + seme + "' and batch_year='" + batchyear + "' and  fromdate='" + date + "' and lastrec=0 and sections='" + sec + "' and " + mday + "=" + dayno + "";
                            string updateQuery = "";
                            if (alt != "2")
                                updateQuery = "update Alternate_schedule set " + mday + "='' where degree_code='" + degree + "' and semester='" + seme + "' and batch_year='" + batchyear + "' and  fromdate='" + date + "' and lastrec=0 and sections='" + sec + "' and " + mday + "=" + dayno + "";
                            else
                                updateQuery = "update Alternate_schedule set " + mday + "=" + daynos + " where degree_code='" + degree + "' and semester='" + seme + "' and batch_year='" + batchyear + "' and  fromdate='" + date + "' and lastrec=0 and sections='" + sec + "' and " + mday + "=" + dayno + "";

                            int insert = da.update_method_wo_parameter(updateQuery, "Text");//delsi2603
                            if (subjectNumber != "" && subjecttypeNum != "" && semester != "" && batchyears != "" && fromdates != "" & todates != "")
                            {

                                string deleteQuer = "delete from subjectchooser_New where semester='" + semester + "' and subject_no='" + subjectNumber + "' and subtype_no='" + subjecttypeNum + "' and batch='" + batchyears + "' and fromdate='" + fromdates + "' and todate='" + todates + "'";
                                int del = da.update_method_wo_parameter(deleteQuer, "Text");
                                if (hourvalues != "" && dayvalues != "" && studsec != "")
                                {
                                    string deleteQ = "delete from LabAlloc_New where degree_code='" + degree + "' and Semester='" + semester + "' and Batch_Year='" + batchyear + "' and Sections='" + studsec + "' and Subject_No='" + subjectNumber + "' and Day_Value='" + dayvalues + "' and  Hour_Value='" + hourvalues + "' and fdate='" + fromdates.ToString() + "' and tdate='" + todates + "'";
                                    int delval = da.update_method_wo_parameter(deleteQ, "Text");
                                }
                            }


                            if (!string.IsNullOrEmpty(altertableqry))// !string.IsNullOrEmpty(altertablesched))
                                i = da.update_method_wo_parameter(altertableqry, "text");
                            if (i == 0)
                            {
                                //Btnok_Click(sender, e);
                            }
                            else
                            {

                                if (insert > 0)
                                {

                                    divok.Visible = true;
                                    Lblok.Text = "Deleted Succesfully";
                                    Session["alterstaffnew"] = "0";

                                    Session["alterforrequest"] = "";

                                }
                            }
                        }
                    }

                }
            }
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["tbl_alter_qry"])))
            //{
            //    altertablesched = Convert.ToString(Session["tbl_alter_qry"]);
            //   // string codevai = Convert.ToString(Session["code_val"]);
            //}
            //int i = 0;
            //if (!string.IsNullOrEmpty(altertableqry) )// !string.IsNullOrEmpty(altertablesched))
            //    i = da.update_method_wo_parameter(altertableqry, "text");
            //if (i == 0)
            //{
            //    Btnok_Click(sender, e);
            //}

        }

        catch (Exception ex)
        {

        }
    }
    protected void btnpopupAlertMsg_Click(object sender, EventArgs e)//delsi 03/05/2018
    {
        try
        {
            lblpopupAlertMsg.Text = string.Empty;
            divPopupAlert.Visible = false;
            Session["alterstaffnew"] = "0";

            Session["alterforrequest"] = "";
            Session["alters"] = "";
            Session["staconform"] = "";
            Session["alter_done"] = "";
            Session["conformleave"] = "";
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    protected void Btnok_Click(object sender, EventArgs e)
    {
        try
        {
           // divPopAlertNEW.Visible = false;
            divok.Visible = false;
            divPopupAlertContent.Visible = false;
            lblpopupAlertMsg.Text = string.Empty;
            divPopupAlert.Visible = false;
            Session["alters"] = "";
             Session["staconform"] = "";
              Session["alter_done"] = "";
             Session["conformleave"] = "";
            if (Session["Requestdefa"] == "1" && Session["RequestHome"] == "3")
            {
                Response.Redirect("~/Default.aspx", true);
            }
            if (Session["RequestHome"] == "3")
            {
                Response.Redirect("~/Requestmod/RequestHome.aspx", true);
                Session["RequestHome"] = "";
            }
            if (Session["Requestlon"] == "2")
            {
                Response.Redirect("~/Default_LoginPage.aspx", true);
                Session["Requestlon"] = "2";
            }
           
        }
        catch (Exception ex)
        {

        }
    }
}
