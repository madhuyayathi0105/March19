using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Collections;
public partial class ScheduleSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    bool alterisnewstaff = false;
    bool alterisnews = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage =string.Empty;
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
        if (Session["alterstaffnew"] == "1")
        {
            alterisnewstaff = true;
        }
        if (Session["alterforrequest"] != null)
        {
            alterisnews = true;
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
        string colornew = string.Empty;
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
            Session["StafforAdmin"] = string.Empty;
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                Session["StafforAdmin"] = "Staff";
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = string.Empty;
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);
                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = string.Empty;
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);
                    string staffname = string.Empty;
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);
                    string deptname = string.Empty;
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);
                }
            }
            else
            {
                Session["StafforAdmin"] = "Admin";
                string staffname = string.Empty;
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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Schedule'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Schedule'  order by HeaderPriority, PagePriority asc";
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
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Reports'";
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
        ltr.Text = "<style type=\"text/css\" rel=\"stylesheet\">" + @"#showmenupages .has-sub ul li:hover a { color:lightyellow; background-color:" + colornew + @"; } #showmenupages .has-sub ul li a { border-bottom: 1px dotted " + colornew + @"; } ul li {  border-bottom: 1px dotted " + colornew + @"; border-right: 1px dotted " + colornew + @";}ul li:hover {color:lightyellow; background-color:" + colornew + @";}a:hover {color:lightyellow;} </style>";
        this.Page.Header.Controls.Add(ltr);
    }

    private DataTable BuildTable()
    {
        string saveCOl = da.GetFunction("select LinkValue from inssettings where College_code ='" + Session["collegecode"].ToString() + "' and LinkName ='Individual Staff Login Attendance New'").Trim();
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
        catch { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if (Session["Entry_Code"] != null)
        {
            string entryCode = Session["Entry_Code"].ToString();
            da.userTimeOut(entryCode);
        }
        Session["Requestdefa"] = "1";
       

        if (alterisnewstaff != true && alterisnews != true)
        {

            Response.Redirect("~/Default.aspx", true);
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        else if (alterisnewstaff == true && alterisnews == true)
        {
            divPopupAlertContent.Visible = true;
            lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
            divPopupAlert.Visible = true;

        }
        else
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }

       
    }
    protected void ImageButton4_Click(object sender, EventArgs e)
    {
        Session["Requestlon"] = "2";
        if (alterisnewstaff != true && alterisnews != true)
        {
            Response.Redirect("~/Default_LoginPage.aspx", true);
        }
        else if (alterisnewstaff == true && alterisnews == true)
        {
            divPopupAlertContent.Visible = true;
            lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
            divPopupAlert.Visible = true;


        }
        else
        {
            Response.Redirect("~/Default_LoginPage.aspx", true);
        }
        //else if (isnew == false && isnews==true)
        //{
        //    divPopupAlertContent.Visible = true;
        //    lblpopupAlertMsg.Text = "Alteration of hours has been Saved.Do You Want to delete the Altered hour(s)?";
        //    divPopupAlert.Visible = true;
        //}
        
    

    }
    protected void ImageButton3_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["RequestPageReDirect"] != null)
            {
                //1@01/12/2017$05/12/2017
                string[] RequestValue = Convert.ToString(Session["RequestPageReDirect"]).Split('@');
                if (Convert.ToString(RequestValue[0]) == "1") //04/12/2017
                {
                    Session["RequestPageReDirect"] = null;
                    Session["back"] = "1";// 12/1/2018
                    Response.Redirect("~/RequestMod/Request.aspx");

                }
            }
            if (Session["Batch_ReDir"] == null)
            {
                Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
            }
            else if (Session["Batch_ReDir"] == "FromNewAlternateSchedule")//Deepali 14.5.18
            {
                Session["Batch_ReDir"] = null;
                Response.Redirect("~/ScheduleMOD/NewAlternateSchedule.aspx");
            }
            else
            {
                Session["Batch_ReDir"] = null;
                Response.Redirect("~/ScheduleMOD/Alternatesched.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Btnok_Click(object sender, EventArgs e)
    {
        try
        {
            // divPopAlertNEW.Visible = false;
            divok.Visible = false;
            divPopupAlertContent.Visible = false;
            lblpopupAlertMsg.Text = string.Empty;
            divPopupAlert.Visible = false;
            Session["alterstaffnew"] = "0";
          
            Session["alterforrequest"] = "";
            if (Session["Requestdefa"] == "1")
            {
                Response.Redirect("~/Default.aspx", true);
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
    protected void btnpopupAlertMsg_Click(object sender, EventArgs e)//delsi 03/05/2018
    {
        try
        {
            lblpopupAlertMsg.Text = string.Empty;
            divPopupAlert.Visible = false;
            Session["alterstaffnew"] = "0";

            Session["alterforrequest"] = "";

          
        }
        catch (Exception ex)
        {

        }
    }
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
                    string smt1=htvval7[m].ToString();
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
                            string daynos="";
                            
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
                            if (alt!="2")
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
}
