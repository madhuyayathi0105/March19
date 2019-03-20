using System;//////////////.........modified on 1.2.12, 18/2/12, 21/2/12(mor tn 2 days col issu), 8/6/12(change bindstaff query)
//----------------------------------15/6/12(hide curr_sem , add enddate condition), 10/7/12(add one more back button)
//---------------10/7/12(visible search option,add one more btngo for staffalter redirect page,)
//--------------12/7/12(changes for alter), 26/7/12(deptwise filter ->deptcode fun), 27/7/12(include class staff , all staff filter)
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Drawing;
using System.Net;
using System.IO;
using System.Net.Mail;
using InsproDataAccess;
public partial class NewAttendance : System.Web.UI.Page
{
    Hashtable hatsubact11 = new Hashtable();
    Hashtable hatsubcon11 = new Hashtable();
    Hashtable hatmisub11 = new Hashtable();
    Hashtable hatSubType = new Hashtable();
    Hashtable hatdicSub = new Hashtable();
    int countsub = 0;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_mobno = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_holi = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysq3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_query = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection new_sql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproDirectAccess dir = new InsproDirectAccess();
    // SqlConnection con1a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SqlCommand cmda;
    SqlCommand cmd1a;
    SqlCommand cmd1;
    SqlCommand cmd_get;
    SqlCommand cmd_query;
    //SqlCommand cmd2;
    //SqlCommand cmd3;
    //SqlCommand cmd5;
    //SqlCommand cmd6;
    // SqlCommand cmda;
    //  SqlCommand cmd1a;
    DAccess2 dac = new DAccess2();
    Hashtable has = new Hashtable();
    DataSet ds_staff = new DataSet();
    DataSet ds_staff_attnd = new DataSet();
    Hashtable has_days_end = new Hashtable();
    Hashtable has_days_first = new Hashtable();
    DataSet ds_holi = new DataSet();
    Hashtable has_holiday = new Hashtable();
    int temp_holi_count = 0;
    int holiday_count = 0;
    Hashtable hatfeestaff = new Hashtable();
    Boolean checkflag = false;
    Boolean rowflag = false;
    Boolean norec_flag = false;
    int preiod_diff = 0, column_count = 0, temp_inc_hour = 0;
    string date = string.Empty;
    int period = 0, hour_difference = 0;
    Boolean loadflag = false;
    string Att_strqueryst = string.Empty;
    string strdesig = "", strdept = "", strstaff = "", strsubno = string.Empty;
    string staff_category_code = string.Empty;
    int diff_hour = 0;
    string attnd_value = string.Empty;
    string date1;
    string date2;
    string datefrom;
    int temp_date = 0;
    string dateto;
    string attnd_val = string.Empty;
    string sqlstr = string.Empty;
    static int noofhrs = 0;
    static int tot_hrs = 0;
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    int days = 0;
    int day_count = 0;
    int col_count = 0;
    int count_staff = 0;
    int from_month = 0, to_month = 0;
    string mon_year_val = string.Empty;
    string staff_code = string.Empty;
    int count_present_staff = 0;
    string staff_attnd_query = string.Empty;
    int temp_month = 0;
    int fd = 0, fyy = 0, fm = 0, td = 0, tyy = 0, tm = 0;
    int totpresentday = 0, daycount = 0;
    int start_date = 0, end_date = 0;
    static int first_half_hr = 0;
    static int sec_half_hr = 0;
    int start_hour = 0, end_hour = 0;
    Boolean first_date_flag = false;
    DateTime dummy_dt = new DateTime();
    string text_temp = string.Empty;
    string text_temp_1 = string.Empty;
    int temp = 0;
    string ddd = string.Empty;
    string mng_attnd_val = "", evng_attnd_val = string.Empty;
    Boolean check_hour = false;
    string SqlBatchYear = "", SqlPrefinal1 = "", SqlPrefinal2 = "", SqlPrefinal3 = "", SqlPrefinal4 = "", SqlFinal = string.Empty;
    string SqlBatchYear1 = "", SqlPrefinal11 = "", SqlPrefinal22 = "", SqlPrefinal33 = "", SqlPrefinal44 = "", SqlFinal1 = string.Empty;
    string sql_s = "", asql = "", Strsql = "", strday = "", sql1 = string.Empty;
    int increment_col_count = 0;
    int increment_day_count = 0;
    string section = "", staff_type_load_sect = string.Empty;
    static string staff_type_load = string.Empty;
    //Start======declared by Manikandan=====
    int SchOrder = 0, nodays = 0;
    int intNHrs = 0;
    string start_dayorder = string.Empty;
    string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
    string startdate = string.Empty;
    string splitenddate = string.Empty;
    string string_day = string.Empty;
    string todate = string.Empty;
    string degree_code = string.Empty;
    string curr_sem = string.Empty;
    string bat_year = string.Empty;
    string subject_name = string.Empty;
    string strmobileno = string.Empty;
    string strfmobile = string.Empty;
    string strmmobile = string.Empty;
    string strstaffmobile = string.Empty;
    string strmsg = string.Empty;
    string mobilenos = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    string send_mail = string.Empty;
    string send_pw = string.Empty;
    string to_mail = string.Empty;
    string strstuname = string.Empty;
    bool flagstudent;
    Boolean flag_true = false;
    Boolean workload_click;// = false;
    Hashtable hat = new Hashtable();
    Hashtable hat_att = new Hashtable();
    int colspan = 0;
    int col_color_new = 0;
    int verify = 0;
    SqlCommand cmd_new;
    SqlCommand cmd_new1;
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet1();
    string staffcode_selected = string.Empty;
    //===End================================
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        load_spread.Sheets[0].SheetName = " ";
        lblnorec.Visible = false;
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            if (Session["date"] != "" && Session["date"] != "\0" && Session["date"] != null)
            {
                date = Session["date"].ToString();
                period = Convert.ToInt16(Session["period"].ToString());
                if (date != "" && date != null)
                {
                    attndddl.Items[0].Enabled = false;
                    attndddl.Items[1].Enabled = false;
                    attndddl.Items[2].Enabled = false;
                    attndddl.Items[3].Enabled = false;
                    attndddl.Items[4].Enabled = false;
                    attndddl.Items[5].Enabled = false;
                    attndddl.Items[6].Enabled = false;
                    attndddl.Items[0].Selected = false;
                    attndddl.Items[1].Selected = false;
                    attndddl.Items[2].Selected = false;
                    attndddl.Items[3].Selected = false;
                    attndddl.Items[4].Selected = false;
                    attndddl.Items[5].Selected = false;
                    attndddl.Items[6].Selected = false;
                    btngo_session.Visible = true;
                    // LinkButton3.Visible = false;
                    //LinkButton2.Visible = false;
                    // backbtnstaffalter.Visible = true;
                    invisiblediv.Visible = false;
                    //Panel5.Visible = false;
                    okbtn.Visible = false;
                    load_spread.Sheets[0].AutoPostBack = false;
                    chk_sms.Visible = false;
                    chk_mail.Visible = false;
                    txt_message.Visible = false;
                    btnsms.Visible = false;
                    //load_spread.Sheets[0].FrozenColumnCount = 3;
                    errlbl.Visible = false;
                    pageset();
                    binddesig();
                    binddept();
                    bindstaff();
                    //--------------
                    //Start Srinath--------------
                    string[] splittest = date.Split('-');//added by srinath 8/2/2013
                    string date15 = string.Empty;
                    if (splittest.GetUpperBound(0) == 2)
                    {
                        date15 = splittest[0] + '/' + splittest[1] + '/' + splittest[2];
                    }
                    if (date15 != "")
                    {
                        date = date15;
                    }
                    //End-----------------
                    string[] split_year = date.Split('/');
                    txtToDate.Text = date;
                    Session["curr_year"] = split_year[2].ToString();
                    txtFromDate.Text = date;
                    //---------------------------
                    staff_type_load = Request.QueryString["ID"];
                    //string[] staff_type_load_sect_splt = staff_type_load_sect.Split(';');
                    //staff_type_load = staff_type_load_sect_splt[0].ToString();
                    //if (staff_type_load_sect_splt.GetUpperBound(0) == 1)
                    //{
                    //    section ="and staff_selector.sections='"+ staff_type_load_sect_splt[1].ToString()+"'";
                    //}
                    //else
                    //{
                    //    section =string.Empty;
                    //}
                    toperiod();
                    optradio.SelectedValue = "subj";
                    work_load();
                    Session.Remove("date");
                    // Session.Remove("suubname_deg_sem");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                txtFromDate.Attributes.Add("Readonly", "Readonly");
                txtToDate.Attributes.Add("Readonly", "Readonly");
                DataSet dss = new DataSet();
                Hashtable hat = new Hashtable();
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
                }
                hat.Add("column_field", grouporusercode);
                dss = dac.select_method("bind_college", hat, "sp");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    //ddlcollege.DataSource = cmd.ExecuteReader();
                    //ddlcollege.DataTextField = "acr";
                    ddlcollege.DataSource = dss;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                }
                load_spread.CommandBar.Visible = false;
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                txt_sem.Enabled = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                attndddl.Items[0].Enabled = false;
                attndddl.Items[1].Enabled = false;
                attndddl.Items[2].Enabled = false;
                attndddl.Items[3].Enabled = false;
                attndddl.Items[4].Enabled = false;
                attndddl.Items[5].Enabled = false;
                attndddl.Items[6].Enabled = false;
                attndddl.Items[0].Selected = false;
                attndddl.Items[1].Selected = false;
                attndddl.Items[2].Selected = false;
                attndddl.Items[3].Selected = false;
                attndddl.Items[4].Selected = false;
                attndddl.Items[5].Selected = false;
                attndddl.Items[6].Selected = false;
                chk_sms.Visible = false;
                chk_mail.Visible = false;
                txt_message.Visible = false;
                btnsms.Visible = false;
                if (Session["date"] != "" && Session["date"] != "\0" && Session["date"] != null)
                {
                }
                else
                {
                    //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
                    //{
                    //    LinkButton3.Visible = false;
                    //    LinkButton2.Visible = true;
                    //    backbtnstaffalter.Visible = false;
                    //}
                    //else
                    //{
                    //    LinkButton3.Visible = true;
                    //    LinkButton2.Visible = false;
                    //    backbtnstaffalter.Visible = false;
                    //}
                    staff_type_load = "0";
                }
                //--------binding
                binddesig();
                binddept();
                toperiod();
                bindsubject();
                bindstaff();
                pageset();
                //---------------------------bind date value
                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                DateTime stDate;
                stDate = System.DateTime.Today.AddDays(-6);
                Session["curr_year"] = dsplit[2].ToString();
                string from_date = stDate.Date.ToShortDateString();
                string[] dsplit_from = from_date.Split(new Char[] { '/' });
                txtFromDate.Text = dsplit_from[1].ToString() + "/" + dsplit_from[0].ToString() + "/" + dsplit_from[2].ToString();
                btngo_session.Visible = false;
                errlbl.Visible = false;
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                Panel3.Visible = false;
                pageddltxt.Visible = false;
                fmlbl.Visible = false;
                tolbl.Visible = false;
                difflbl.Visible = false;
                diffperlbl.Visible = false;
                colorpnl.Visible = false;
                load_tree.Visible = false;
                optradio.SelectedValue = "color";
                load_spread.Sheets[0].AutoPostBack = false;
                //Added by srinath 3/04/2014
                lblbatch.Enabled = false;
                lbldegree.Enabled = false;
                lblbranch.Enabled = false;
                ddlbatch.Enabled = false;
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void pageset()
    {
        okbtn.Visible = false;
        pageddltxt.Visible = false;
        //-------------fond setting
        load_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        load_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        load_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        load_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
        load_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        load_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
        load_spread.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
        load_spread.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
        //load_spread.CommandBar.Visible = true;
        // load_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
        //---------------sheet cornet setting
        load_spread.Sheets[0].SheetCorner.RowCount = 2;
        load_spread.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
        load_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 13;
        style.Font.Bold = true;
        load_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        load_spread.Sheets[0].AllowTableCorner = true;
        load_spread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //---------------page number
        load_spread.Sheets[0].PageSize = 13;
        load_spread.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        load_spread.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        load_spread.Pager.Align = HorizontalAlign.Right;
        load_spread.Pager.Font.Bold = true;
        load_spread.Pager.Font.Name = "Book Antiqua";
        load_spread.Pager.ForeColor = Color.DarkGreen;
        load_spread.Pager.BackColor = Color.Beige;
        load_spread.Pager.BackColor = Color.AliceBlue;
        load_spread.Pager.PageCount = 5;
        //---------------------------
    }

    private void binddesig()
    {
        desigddl.Items.Clear();
        string strquery = "select  distinct desig_name,desig_code from desig_master where staffcategory='Teaching' and collegecode='" + ddlcollege.SelectedValue.ToString() + "'";
        DataSet ds1 = d2.select_method_wo_parameter(strquery, "Text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            desigddl.DataSource = ds1;
            desigddl.DataValueField = "desig_code";
            desigddl.DataTextField = "desig_name";
            desigddl.DataBind();
            desigddl.Items.Insert(0, "All");
        }
    }

    private void binddept()
    {
        deptddl.Items.Clear();
        //cmd = new SqlCommand(" select distinct degree.degree_code,department.dept_name,department.dept_acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code='" + Session["collegecode"].ToString() + "'  and deptprivilages.Degree_code=degree.Degree_code", con);
        string strquery = "select distinct Dept_Code,Dept_Name from department where isacademic=1 and college_code='" + ddlcollege.SelectedValue.ToString() + "'";
        DataSet ds1 = d2.select_method_wo_parameter(strquery, "Text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            deptddl.DataSource = ds1;
            deptddl.DataValueField = "Dept_Code";
            deptddl.DataTextField = "dept_name";
            deptddl.DataBind();
            deptddl.Items.Insert(0, "All");
        }
    }

    public void bindsubject()
    {
        subjddl.Items.Clear();
        //---------------bind subject
        string strsyll_yr = string.Empty;
        string deg_code = string.Empty;
        string bat_yr = string.Empty;
        string curr_sem = string.Empty;
        string subj_name = string.Empty;
        subjddl.Items.Insert(0, "All");
        //ddlcollege.SelectedValue.ToString()
        //cmd = new SqlCommand("select distinct batch_year,degree_code,current_semester from registration where cc=0 and exam_flag<>'debar' and delflag=0 order by batch_year,degree_code,current_semester", con);
        string strquery = "select distinct batch_year,degree_code,current_semester from registration where cc=0 and exam_flag<>'debar' and delflag=0 and college_code='" + ddlcollege.SelectedValue.ToString() + "' order by batch_year,degree_code,current_semester";//modified by Manikandan from above commented line
        DataSet dssubbatch = d2.select_method_wo_parameter(strquery, "Text");
        for (int i = 0; i < dssubbatch.Tables[0].Rows.Count; i++)
        {
            //----------get syllabus year
            deg_code = dssubbatch.Tables[0].Rows[i]["degree_code"].ToString();
            bat_yr = dssubbatch.Tables[0].Rows[i]["batch_year"].ToString();
            curr_sem = dssubbatch.Tables[0].Rows[i]["current_semester"].ToString();
            strsyll_yr = d2.GetFunction("select distinct syll_code from syllabus_master where degree_code='" + deg_code + "' and batch_year='" + bat_yr + "' and semester='" + curr_sem + "'");
            //-------------get subject
            if (strsyll_yr.Trim() != "" && strsyll_yr.Trim() != "0" && strsyll_yr != null)
            {
                string strsubjecty = "select distinct subject_no,subtype_no, subject_code,subject_name from subject where syll_code=" + strsyll_yr + "";
                DataSet dssub = d2.select_method_wo_parameter(strsubjecty, "Text");
                for (int s = 0; s < dssub.Tables[0].Rows.Count; s++)
                {
                    subj_name = dssub.Tables[0].Rows[s]["subject_name"].ToString() + "-" + dssub.Tables[0].Rows[s]["subject_code"].ToString();
                    System.Web.UI.WebControls.ListItem acclist = new System.Web.UI.WebControls.ListItem();
                    acclist.Value = (dssub.Tables[0].Rows[s]["subject_no"].ToString());
                    acclist.Text = (subj_name.ToString());
                    subjddl.Items.Add(acclist);
                }
            }
        }
    }

    public void bindstaff()
    {
        stafftxt.Items.Clear();
        //  cmd = new SqlCommand("select distinct staff_name,dept_name,desig_name,m.staff_code from staffmaster m,stafftrans t,hrdept_master h,desig_master d,staff_selector st where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and latestrec = 1 and st.staff_code=m.staff_code order by staff_name", con);
        string strquery = " select distinct staff_name,staff_code from staffmaster where resign<>1 and settled<>1 and college_Code='" + ddlcollege.SelectedValue.ToString() + "' order by staff_name";
        DataSet ds1 = d2.select_method_wo_parameter(strquery, "Text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            stafftxt.DataSource = ds1.Tables[0];
            stafftxt.DataValueField = "staff_code";
            stafftxt.DataTextField = "staff_name";
            stafftxt.DataBind();
            stafftxt.Items.Insert(0, "All");
        }
    }

    private void toperiod()
    {
        toperddl.Items.Clear();
        string sqlstr = string.Empty;
        int noofhrs = 0;
        int item = 0;
        ds_staff.Clear();
        sqlstr = "select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half'  from PeriodAttndSchedule";
        ds_staff = d2.select_method_wo_parameter(sqlstr, "Text");
        if (ds_staff.Tables[0].Rows.Count > 0)
        {
            noofhrs = Convert.ToInt16(ds_staff.Tables[0].Rows[0]["Total Hours"].ToString());
            for (item = 1; item <= noofhrs; item++)
            {
                toperddl.Items.Insert(item - 1, item.ToString());
                frmperddl.Items.Insert(item - 1, item.ToString());
            }
            toperddl.Items.Insert(0, "All");
            frmperddl.Items.Insert(0, "All");
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        try
        {
            string strfdate = txtFromDate.Text.ToString();
            string strtdate = txtToDate.Text.ToString();
            string[] spf = strfdate.Split('/');
            string[] spt = strtdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                DateTime stDate = System.DateTime.Now;
                txtToDate.Text = stDate.ToString("dd/MM/yyyy");
                stDate = System.DateTime.Today.AddDays(-6);
                txtFromDate.Text = stDate.ToString("dd/MM/yyyy");
                errlbl.Visible = true;
                errlbl.Text = "From Date Must Be Lesser Than or Equal to Todate";
            }
        }
        catch
        {
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        try
        {
            string strfdate = txtFromDate.Text.ToString();
            string strtdate = txtToDate.Text.ToString();
            string[] spf = strfdate.Split('/');
            string[] spt = strtdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                DateTime stDate = System.DateTime.Now;
                txtToDate.Text = stDate.ToString("dd/MM/yyyy");
                stDate = System.DateTime.Today.AddDays(-6);
                txtFromDate.Text = stDate.ToString("dd/MM/yyyy");
                errlbl.Visible = true;
                errlbl.Text = "From Date Must Be Lesser Than or Equal to Todate";
            }
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    btnclick_function();
        //}
        //catch (Exception ex)
        //{
        //    errlbl.Text = ex.ToString();
        //    errlbl.Visible = true;
        //}
        //Added by srinath 3/04/2014
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        if (ddlreporttype.SelectedIndex == 3)
        {
            loadclassrepor();
        }
        else if (ddlreporttype.SelectedIndex == 4)  //modified by Mullai
        {
            loadclasshourreport();
        }
        else
        {
            btnclick_function();
        }
    }

    public void btnclick_function()
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        load_spread.CurrentPage = 0;
        if (ddlreporttype.SelectedIndex == 0)
        {
            if ((frmperddl.Text != "All" && toperddl.Text != "All"))
            {
                if ((Convert.ToInt16(frmperddl.SelectedValue) < Convert.ToInt16(toperddl.SelectedValue) || Convert.ToInt16(frmperddl.SelectedValue) == Convert.ToInt16(toperddl.SelectedValue)))
                {
                    diffperlbl.Visible = false;
                    work_load();
                }
            }
            else if ((frmperddl.Text == "All" && toperddl.Text == "All"))
            {
                diffperlbl.Visible = false;
                work_load();
            }
            else
            {
                diffperlbl.Visible = true;
            }
        }
        else if (ddlreporttype.SelectedIndex == 1)
        {
            staff_schedule_report();
        }
        else if (ddlreporttype.SelectedIndex == 5)
        {
            loadstaffworkloadwithexp();
        }
        else
        {
            individual_workload();
        }
    }

    public void work_load()
    {
        load_spread.Sheets[0].RowHeader.Visible = false;
        load_spread.Sheets[0].ColumnCount = 0;
        load_spread.Sheets[0].RowCount = 0;
        staff_type_load = "0";
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        date2 = txtToDate.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '/' });
        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        hat_att.Clear();
        for (int add_hat = 0; add_hat < attndddl.Items.Count; add_hat++)
        {
            if (attndddl.Items[add_hat].Selected == true)
            {
                hat_att.Add(attndddl.Items[add_hat].Text, attndddl.Items[add_hat].Value);
            }
        }
        string ddf = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        string ddt = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
        {
            if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                days = -1;
                dt1 = DateTime.Now.AddDays(-6);
                dt2 = DateTime.Now;
                try
                {
                    dt1 = Convert.ToDateTime(ddf);
                    dt2 = Convert.ToDateTime(ddt);
                    TimeSpan t = dt2.Subtract(dt1);
                    days = t.Days;
                }
                catch
                {
                    try
                    {
                        dt1 = Convert.ToDateTime(date1);
                        dt2 = Convert.ToDateTime(date2);
                        TimeSpan t = dt2.Subtract(dt1);
                        days = t.Days;
                    }
                    catch
                    {
                        difflbl.Text = ddf + ddt;
                    }
                }
                if (days < 0)
                {
                    difflbl.Visible = true;
                    load_spread.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    return;
                }
                date1 = txtFromDate.Text;
                string[] split_from = date1.Split(new Char[] { '/' });
                if (split_from.GetUpperBound(0) == 2)
                {
                    if (Convert.ToInt16(split_from[0].ToString()) <= 31 && Convert.ToInt16(split_from[1].ToString()) <= 12 && Convert.ToInt16(split_from[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                        date2 = txtToDate.Text;
                        string[] split_to = date2.Split(new Char[] { '/' });
                        if (split_to.GetUpperBound(0) == 2)
                        {
                            if (Convert.ToInt16(split_to[0].ToString()) <= 31 && Convert.ToInt16(split_to[1].ToString()) <= 12 && Convert.ToInt16(split_to[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                            {
                                if (days >= 0)
                                {
                                    difflbl.Visible = false;
                                    load_spread.Visible = true;
                                    btnprintmaster.Visible = true;
                                    lblrptname.Visible = true;
                                    txtexcelname.Visible = true;
                                    btnxl.Visible = true;
                                    string[] differdays = new string[days];
                                    // con.Close();
                                    // con.Open();
                                    //  ds_staff.Clear();
                                    //  sqlstr = "select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half',max(no_of_hrs_II_half_day) as 'Second Half' from PeriodAttndSchedule where no_of_hrs_I_half_day<>'' and no_of_hrs_II_half_day<>''";
                                    //  SqlDataAdapter da_hrs = new SqlDataAdapter(sqlstr, con);
                                    //  da_hrs.Fill(ds_staff);
                                    sqlstr = "select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half',max(no_of_hrs_II_half_day) as 'Second Half' from PeriodAttndSchedule where no_of_hrs_I_half_day<>'' and no_of_hrs_II_half_day<>''";
                                    ds_staff = dac.select_method_wo_parameter(sqlstr, "Text");//===================Modified by Venkat ============================
                                    if (ds_staff.Tables.Count > 0 && ds_staff.Tables[0].Rows.Count > 0)
                                    {
                                        tot_hrs = Convert.ToInt16(ds_staff.Tables[0].Rows[0]["Total Hours"].ToString());
                                        first_half_hr = Convert.ToInt16(ds_staff.Tables[0].Rows[0]["First Half"].ToString());
                                        sec_half_hr = Convert.ToInt16(ds_staff.Tables[0].Rows[0]["Second Half"].ToString());
                                        //if (toperddl.SelectedItem.ToString() != "All")
                                        //{
                                        //    colspan = Convert.ToInt32(toperddl.SelectedItem.ToString());
                                        //}
                                        //else
                                        //{
                                        //    colspan = Convert.ToInt32(ds_staff.Tables[0].Rows[0]["Total Hours"].ToString());
                                        //}
                                    }
                                    if (tot_hrs != 0)
                                    {
                                        FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                                        chkbox.AutoPostBack = true;
                                        load_spread.Sheets[0].ColumnHeader.RowCount = 2;
                                        load_spread.Sheets[0].RowCount = 0;
                                        load_spread.Sheets[0].ColumnCount = 5;
                                        load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                        load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                                        load_spread.Sheets[0].Columns[1].CellType = chkbox;
                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                        load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                        load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                        load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Name";
                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                        load_spread.Sheets[0].FrozenColumnCount = 5;
                                        load_spread.Sheets[0].Columns[0].Locked = true;
                                        load_spread.Sheets[0].Columns[2].Locked = true;
                                        load_spread.Sheets[0].Columns[3].Locked = true;
                                        load_spread.Sheets[0].Columns[4].Locked = true;
                                        load_spread.Sheets[0].Columns[0].Width = 30;
                                        load_spread.Sheets[0].Columns[1].Width = 35;
                                        load_spread.Sheets[0].Columns[2].Width = 140;
                                        load_spread.Sheets[0].Columns[3].Width = 120;
                                        load_spread.Sheets[0].Columns[4].Width = 200;
                                        load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                        load_spread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                                        load_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                        load_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                        //load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        //load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        //tot_hrs = tot_hrs + 2;//Added by Manikandan on 15/10/2013
                                        //----------------designation ALL
                                        if (desigddl.Items.Count > 0)
                                        {
                                            if (desigddl.SelectedValue == "All" || desigddl.SelectedValue == "")
                                            {
                                                strdesig = string.Empty;
                                            }
                                            else
                                            {
                                                strdesig = " and desig_name='" + desigddl.SelectedItem.ToString() + "'";
                                            }
                                        }
                                        //--------------department ALL
                                        if (deptddl.Items.Count > 0)
                                        {
                                            if (deptddl.SelectedValue == "All" || deptddl.SelectedValue == "")
                                            {
                                                strdept = string.Empty;
                                            }
                                            else
                                            {
                                                strdept = " and h.dept_code='" + deptddl.SelectedValue.ToString() + "'";
                                                //strdept = " and h.dept_code='" + GetFunction("select dept_code from  degree where degree_code='" + deptddl.SelectedValue.ToString() + "'") + "'";
                                            }
                                        }
                                        //--------------staff name
                                        string only_staff_code = string.Empty;
                                        if (date == "" || date == null)
                                        {
                                            if (stafftxt.Items.Count > 0)
                                            {
                                                if (stafftxt.SelectedValue.ToString() == "All" || stafftxt.SelectedValue.ToString() == "")
                                                {
                                                    strstaff = string.Empty;
                                                    only_staff_code = string.Empty;
                                                }
                                                else
                                                {
                                                    strstaff = " and m.staff_code='" + stafftxt.SelectedValue.ToString() + "'";
                                                    only_staff_code = " and ss.staff_code<>'" + stafftxt.SelectedValue.ToString() + "'";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            strstaff = " and m.staff_code<>'" + Session["Staff_Code"].ToString() + "'";
                                            only_staff_code = " and ss.staff_code<>'" + Session["Staff_Code"].ToString() + "'";
                                        }
                                        //--------------subject
                                        if (subjddl.Items.Count > 0)
                                        {
                                            if (subjddl.SelectedValue.ToString() == "All" || subjddl.SelectedValue.ToString() == "")
                                            {
                                                strsubno = string.Empty;
                                            }
                                            else
                                            {
                                                strsubno = "subject_no='" + subjddl.SelectedValue.ToString() + "'";
                                            }
                                        }
                                        //--------------checkbox
                                        int start_prd = 0;
                                        if (date != "" && date != null)
                                        {
                                            start_prd = Convert.ToInt16(period.ToString());
                                            noofhrs = Convert.ToInt16(period.ToString());
                                            frmperddl.SelectedIndex = start_prd;
                                            toperddl.SelectedIndex = start_prd;
                                        }
                                        else
                                        {
                                            if (frmperddl.Items.Count > 0 && toperddl.Items.Count > 0)
                                            {
                                                if (frmperddl.SelectedValue.ToString() == "All" && toperddl.SelectedValue.ToString() == "All")
                                                {
                                                    noofhrs = tot_hrs;
                                                    start_prd = 1;
                                                }
                                                else
                                                {
                                                    start_prd = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                    noofhrs = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                }
                                            }
                                        }
                                        preiod_diff = 0;
                                        if (date == "" || date == null)
                                        {
                                            if (frmperddl.Items.Count > 0 && toperddl.Items.Count > 0)
                                            {
                                                if ((frmperddl.SelectedItem.ToString()) != "All" && toperddl.SelectedItem.ToString() != "All")
                                                {
                                                    for (int diff = Convert.ToInt16(frmperddl.SelectedItem.ToString()); diff <= Convert.ToInt16(toperddl.SelectedItem.ToString()); diff++)
                                                    {
                                                        preiod_diff++;//-------------tot perid selected
                                                    }
                                                }
                                                else
                                                {
                                                    preiod_diff = tot_hrs;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            preiod_diff = 1;
                                        }
                                        load_spread.Sheets[0].ColumnCount = load_spread.Sheets[0].ColumnCount + (days * preiod_diff) + preiod_diff;
                                        fd = Convert.ToInt16(dt1.ToString("dd"));
                                        fyy = Convert.ToInt16(dt1.ToString("yyyy"));
                                        fm = Convert.ToInt16(dt1.ToString("MM"));
                                        td = Convert.ToInt16(dt2.ToString("dd"));
                                        tyy = Convert.ToInt16(dt2.ToString("yyyy"));
                                        tm = Convert.ToInt16(dt2.ToString("MM"));
                                        DateTime col_date = dt1;
                                        for (day_count = 0; day_count <= days; day_count++)
                                        {
                                            //column_count = 4 + (preiod_diff * day_count);
                                            column_count = 6 + (preiod_diff * day_count);//modified by Manikandan on 15/10/2013
                                            //start=======added by Manikandan======
                                            if (frmperddl.Items.Count > 0 && toperddl.Items.Count > 0)
                                            {
                                                if (frmperddl.SelectedItem.ToString() != "All" && toperddl.SelectedItem.ToString() != "All")
                                                {
                                                    if (Convert.ToInt32(frmperddl.SelectedItem.ToString()) <= Convert.ToInt32(toperddl.SelectedItem.ToString()))
                                                    {
                                                        colspan = (Convert.ToInt32(toperddl.SelectedItem.ToString()) - Convert.ToInt32(frmperddl.SelectedItem.ToString())) + 1;
                                                    }
                                                }
                                            }
                                            //====================End==============
                                            load_spread.Sheets[0].ColumnHeader.Cells[0, (column_count - 1)].Text = col_date.ToString("dd") + "/" + col_date.ToString("MM").ToString() + "/" + col_date.ToString("yyyy").ToString();
                                            if (frmperddl.Items.Count > 0 && toperddl.Items.Count > 0)
                                            {
                                                if (frmperddl.SelectedItem.ToString() != "All" && toperddl.SelectedItem.ToString() != "All")
                                                {
                                                    if (Convert.ToInt32(frmperddl.SelectedItem.ToString()) <= Convert.ToInt32(toperddl.SelectedItem.ToString()))
                                                    {
                                                        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, (column_count - 1), 1, colspan);//this line modified by Manikandan from above commented line on 08/10/2013
                                                    }
                                                }
                                                else
                                                {
                                                    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, (column_count - 1), 1, tot_hrs);
                                                }
                                            }
                                            for (col_count = start_prd; col_count <= noofhrs; col_count++)
                                            {
                                                if (optradio.Items.Count > 0)
                                                {
                                                    if (optradio.SelectedItem.ToString() != "Color")
                                                    {
                                                        load_spread.Sheets[0].ColumnHeader.Cells[1, column_count - 1].Text = "Period " + Convert.ToString(col_count);
                                                        load_spread.Sheets[0].Columns[column_count - 1].Locked = true;
                                                    }
                                                    else
                                                    {
                                                        load_spread.Sheets[0].Columns[column_count - 1].Width = 30;
                                                        load_spread.Sheets[0].Columns[column_count - 1].Locked = true;
                                                        load_spread.Sheets[0].ColumnHeader.Cells[1, column_count - 1].Text = Convert.ToString(col_count);
                                                        load_spread.Sheets[0].Columns[column_count - 1].Locked = true;
                                                        load_spread.Width = (column_count * 30) + 200;
                                                    }
                                                }
                                                column_count = column_count + 1;
                                            }
                                            col_date = col_date.AddDays(1);
                                        }
                                        //   if (subjddl.SelectedValue.ToString() == "All" || subjddl.SelectedValue.ToString() == "")
                                        ds_staff.Clear();
                                        ds_staff.Tables[0].Columns.Clear();
                                        if (staff_type_load == "0")
                                        {
                                            workload_click = true;
                                            pnl_filter.Visible = true;
                                            has.Add("@coll_code", ddlcollege.SelectedValue.ToString());
                                            has.Add("@subjno ", " ");
                                            has.Add("@strdesig  ", strdesig);
                                            has.Add("@strdept", strdept);
                                            has.Add("@strstaff ", @strstaff);
                                            ds_staff = dac.select_method("workload_getstafflist", has, "sp");
                                        }
                                        else
                                        {
                                            pnl_filter.Visible = false;
                                            string suubname_deg_sem = string.Empty;
                                            suubname_deg_sem = Session["suubname_deg_sem"].ToString();
                                            //=========tree
                                            string[] sp1 = suubname_deg_sem.Split('-');
                                            string sections = "", sem = "", byear = "", subj_count_in_onehr = "", subj_no = "", degree = string.Empty;
                                            //-----------------0n 11/7/12
                                            if (sp1.GetUpperBound(0) == 7)
                                            {
                                                degree = sp1[0];
                                                sections = sp1[3];
                                                sem = sp1[1];
                                                byear = sp1[4];
                                                subj_no = sp1[2];
                                                subj_count_in_onehr = sp1[6];
                                            }
                                            else
                                            {
                                                degree = sp1[0];
                                                sections = string.Empty;
                                                //-----------------------------------
                                                // byear = sp1[3]; modified by srinath
                                                byear = sp1[4];
                                                //------------------------------------
                                                sem = sp1[1];
                                                subj_no = sp1[2];
                                                subj_count_in_onehr = sp1[5];
                                            }
                                            if (sections != "")
                                            {
                                                section = "and ss.sections='" + sections + "'";
                                            }
                                            else
                                            {
                                                section = string.Empty;
                                            }
                                            //===================Modified by Venkat ============================
                                            //string query_staff_load = "select distinct staff_name,subject_code,desig_name,dept_name ,dept_acronym,h.dept_code,t.category_code,subject_name,ss.staff_code from subject s,sub_sem,staff_selector ss,staffmaster m,syllabus_master,stafftrans t,hrdept_master h,desig_master d where sub_sem.subtype_no=s.subtype_no and  s.syll_code=(select syll_code from syllabus_master where degree_code=" + degree + " and semester=" + sem + "  and batch_year =" + byear + ") and ss.subject_no=s.subject_no and ss.staff_code=m.staff_code and syllabus_master.syllabus_year=" + byear + "  " + section + " and promote_count=1 and m.resign<>1 and m.settled<>1 and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.staff_code = ss.staff_code and ss.staff_code = t.staff_code and latestrec = 1 and t.stftype='TEACHING' " + only_staff_code + "";
                                            //string query_staff_load = "select distinct staff_name,desig_name,dept_name ,dept_acronym,h.dept_code,t.category_code,ss.staff_code from subject s,sub_sem,staff_selector ss,staffmaster m,syllabus_master,stafftrans t,hrdept_master h,desig_master d where sub_sem.subtype_no=s.subtype_no and  s.syll_code=(select syll_code from syllabus_master where degree_code=" + degree + " and semester=" + sem + "  and batch_year =" + byear + ") and ss.subject_no=s.subject_no and ss.staff_code=m.staff_code and syllabus_master.syllabus_year=" + byear + "  " + section + " and promote_count=1 and m.resign<>1 and m.settled<>1 and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.staff_code = ss.staff_code and ss.staff_code = t.staff_code and latestrec = 1 and t.stftype='TEACHING' " + only_staff_code + "";
                                            //cmd = new SqlCommand(query_staff_load, con);
                                            //con.Close();
                                            //con.Open();
                                            //SqlDataAdapter da = new SqlDataAdapter(cmd);
                                            //da.Fill(ds_staff);
                                            string query_staff_load = "select distinct staff_name,desig_name,dept_name ,dept_acronym,h.dept_code,t.category_code,ss.staff_code from subject s,sub_sem,staff_selector ss,staffmaster m,syllabus_master,stafftrans t,hrdept_master h,desig_master d where sub_sem.subtype_no=s.subtype_no and  s.syll_code=(select syll_code from syllabus_master where degree_code=" + degree + " and semester=" + sem + "  and batch_year =" + byear + ") and ss.subject_no=s.subject_no and ss.staff_code=m.staff_code and syllabus_master.syllabus_year=" + byear + "  " + section + " and promote_count=1 and m.resign<>1 and m.settled<>1 and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.staff_code = ss.staff_code and ss.staff_code = t.staff_code and latestrec = 1 and t.stftype='TEACHING' " + only_staff_code + "";
                                            ds_staff = dac.select_method_wo_parameter(query_staff_load, "Text");
                                            //=======================================
                                        }
                                        if (ds_staff.Tables.Count > 0 && ds_staff.Tables[0].Rows.Count > 0)
                                            count_staff = ds_staff.Tables[0].Rows.Count;
                                        if (count_staff > 0)
                                        {
                                            find_staff_attendance(); //===============================function=======================================================                                        
                                        }
                                        else
                                        {
                                            errlbl.Visible = true;
                                            errlbl.Text = "No Staff(s) Available";
                                        }
                                    }
                                    if (load_spread.Sheets[0].RowCount == 0)
                                    {
                                        load_spread.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        colorpnl.Visible = false;
                                        errlbl.Visible = true;
                                        errlbl.Text = "No Record(s) Found";
                                    }
                                    else
                                    {
                                        load_spread.Visible = true;
                                        btnprintmaster.Visible = true;
                                        lblrptname.Visible = true;
                                        txtexcelname.Visible = true;
                                        btnxl.Visible = true;
                                        colorpnl.Visible = true;
                                        if (optradio.SelectedIndex != 0)
                                        {
                                            freehr.Visible = false;
                                            Label2.Visible = false;
                                            Bc.Visible = false;
                                            Label10.Visible = false;
                                            noattnd.Visible = true;
                                            Label1.Visible = true;
                                            p.Visible = true;
                                            Label9.Visible = true;
                                            a.Visible = true;
                                            Label3.Visible = true;
                                            per.Visible = true;
                                            Label4.Visible = true;
                                            la.Visible = true;
                                            Label5.Visible = true;
                                            od.Visible = true;
                                            Label6.Visible = true;
                                            na.Visible = true;
                                            Label8.Visible = true;
                                        }
                                        else
                                        {
                                            freehr.Visible = true;
                                            Label2.Visible = true;
                                            Bc.Visible = true;
                                            Label10.Visible = true;
                                            noattnd.Visible = false;
                                            Label1.Visible = false;
                                            p.Visible = false;
                                            Label9.Visible = false;
                                            a.Visible = false;
                                            Label3.Visible = false;
                                            per.Visible = false;
                                            Label4.Visible = false;
                                            la.Visible = false;
                                            Label5.Visible = false;
                                            od.Visible = false;
                                            Label6.Visible = false;
                                            na.Visible = false;
                                            Label8.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (load_spread.Sheets[0].RowCount > 0)
        {
            load_spread.Sheets[0].Columns[0].BackColor = Color.White;
            load_spread.Sheets[0].Columns[1].BackColor = Color.White;
            load_spread.Sheets[0].Columns[2].BackColor = Color.White;
            load_spread.Sheets[0].Columns[3].BackColor = Color.White;
            load_spread.Sheets[0].Columns[4].BackColor = Color.White;
            chk_sms.Visible = true;
            chk_mail.Visible = true;
            txt_message.Visible = true;
            btnsms.Visible = true;
            if (optradio.Items.Count > 0)
            {
                if (optradio.SelectedItem.ToString() == "Color")
                {
                    if (load_spread.Sheets[0].RowCount > 10)
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 40;
                    }
                    else
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 80;
                    }
                    //load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount * 60;
                }
                else if (optradio.SelectedItem.ToString() == "Subject")
                {
                    if (load_spread.Sheets[0].RowCount > 10)
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 60;
                    }
                    else
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 100;
                    }
                }
                else
                {
                    if (load_spread.Sheets[0].RowCount > 10)
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 60;
                    }
                    else
                    {
                        load_spread.Height = load_spread.Sheets[0].RowCount * 100;
                    }
                    //load_spread.Height = load_spread.Sheets[0].RowCount * 100;
                }
            }
            else
            {
                if (load_spread.Sheets[0].RowCount > 10)
                {
                    load_spread.Height = load_spread.Sheets[0].RowCount * 60;
                }
                else
                {
                    load_spread.Height = load_spread.Sheets[0].RowCount * 100;
                }
                //load_spread.Height = load_spread.Sheets[0].RowCount * 100;
            }
            load_spread.Width = 960;
            load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
            load_spread.Visible = true;
        }
        else
        {
            chk_sms.Visible = false;
            chk_mail.Visible = false;
            txt_message.Visible = false;
            btnsms.Visible = false;
            load_spread.Visible = false;
            errlbl.Text = "No Records are found";
            errlbl.Visible = true;
        }
    }

    public void find_staff_attendance()
    {
        int sno = 0;
        load_spread.Sheets[0].RowCount++;
        load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].BackColor = Color.AliceBlue;
        errlbl.Visible = false;
        SqlDataReader dr_periodattndsched;
        for (int temp_staff = 0; temp_staff < count_staff; temp_staff++)
        {
            increment_col_count = 0;
            increment_day_count = 0;
            dummy_dt = dt1;
            staff_code = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
            from_month = (Convert.ToInt16(dt1.ToString("MM"))) + ((Convert.ToInt16(dt1.ToString("yyyy"))) * 12);
            to_month = (Convert.ToInt16(dt2.ToString("MM"))) + ((Convert.ToInt16(dt2.ToString("yyyy"))) * 12);
            for (temp_month = from_month; temp_month <= to_month; temp_month++)
            {
                totpresentday = 0;
                mon_year_val = (temp_month % 12) + "/" + (temp_month / 12);
                //=================================Modified by Venkat==================================
                //staff_attnd_query = "select * from staff_attnd where mon_year='" + mon_year_val + "' and staff_code='" + staff_code + "' ";// and (" + day.ToString() + " like 'P-%' or " + day.ToString() + " like 'PER-%' or " + day.ToString() + " like 'LA-%' or " + day.ToString() + " like 'OD-%' or " + day.ToString() + " like 'OOD-%' and " + day.ToString() + " like '%-P' or " + day.ToString() + " like '%-PER' or " + day.ToString() + " like '%-LA' or " + day.ToString() + " like '%-OD' or " + day.ToString() + " like '%-OOD')";
                //ds_staff_attnd.Clear();//added by Manikandan on 09/10/2013
                //SqlDataAdapter da = new SqlDataAdapter(staff_attnd_query, con);
                //da.Fill(ds_staff_attnd);
                staff_attnd_query = "select * from staff_attnd where mon_year='" + mon_year_val + "' and staff_code='" + staff_code + "' ";
                ds_staff_attnd = dac.select_method_wo_parameter(staff_attnd_query, "Text");
                //===============================================
                count_present_staff = ds_staff_attnd.Tables[0].Rows.Count;
                //if (count_present_staff > 0)
                {
                    staff_category_code = ds_staff.Tables[0].Rows[temp_staff]["category_code"].ToString();
                    sno++;
                    load_spread.Sheets[0].RowCount++;
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Text = sno.ToString();
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 2].Text = ds_staff.Tables[0].Rows[temp_staff]["dept_name"].ToString();
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Text = ds_staff.Tables[0].Rows[temp_staff]["desig_name"].ToString();
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 4].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Note = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                    if (!hatfeestaff.Contains(staff_code))
                    {
                        hatfeestaff.Add(staff_code, staff_code);
                    }
                    //  if(first_date_flag==false)
                    {
                        first_date_flag = true;
                        if (temp_month == to_month)
                        {
                            cal_date(temp_month);
                            if (fd == td)
                            {
                                totpresentday += 1;
                            }
                            else if (td == daycount)
                            {
                                totpresentday += daycount;
                            }
                            else
                            {
                                totpresentday += td - (fd - 1);
                            }
                        }
                        if (temp_month != to_month)
                        {
                            cal_date(temp_month);
                            totpresentday += daycount;
                        }
                        if (temp_month == from_month)
                        {
                            start_date = Convert.ToInt16(dt1.ToString("dd"));
                        }
                        else
                        {
                            start_date = 1;
                        }
                        if (temp_month == to_month)
                        {
                            end_date = Convert.ToInt16(dt2.ToString("dd"));
                        }
                        else
                        {
                            end_date = totpresentday;
                        }
                        //has_days_first.Add(temp_staff, start_date);
                        // has_days_end.Add(temp_staff, end_date);
                    }
                    //------------------------get holiday
                    has_holiday.Clear();
                    has_holiday.Add("@fromdate", dt1.ToShortDateString());
                    has_holiday.Add("@todate", dt2.ToShortDateString());
                    has_holiday.Add("@category_code", staff_category_code);
                    ds_holi = dac.select_method("get_staff_holiday", has_holiday, "sp");
                    holiday_count = ds_holi.Tables[0].Rows.Count;
                    has_holiday.Clear();
                    for (temp_holi_count = 0; temp_holi_count < holiday_count; temp_holi_count++)
                    {
                        has_holiday.Add(ds_holi.Tables[0].Rows[temp_holi_count]["holiday_date"], ds_holi.Tables[0].Rows[temp_holi_count]["holiday_desc"]);
                    }
                    //------------------------------------
                    for (temp_date = start_date; temp_date <= end_date; temp_date++)
                    {
                        increment_day_count++;
                        if (temp_date <= end_date)
                        {
                            if (has_holiday.ContainsKey(dummy_dt))
                            {
                                load_spread.Sheets[0].SpanModel.Add((load_spread.Sheets[0].RowCount - 1), ((increment_day_count * preiod_diff) - preiod_diff) + 3, 1, preiod_diff);
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), (((increment_day_count * preiod_diff) - preiod_diff) + 3)].Text = "Holiday For Staff";
                                increment_col_count = increment_col_count + preiod_diff - 1;
                            }
                            else
                            {
                                sql1 = string.Empty; Strsql = string.Empty; asql = string.Empty;
                                sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                                asql = "select Alternate_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=Alternate_schedule.degree_code and semester=Alternate_schedule.semester), ";
                                ddd = (Convert.ToInt16(dummy_dt.ToString("dd"))).ToString();
                                for (int day_lp = 0; day_lp < 7; day_lp++)
                                {
                                    strday = Days[day_lp].ToString();
                                    for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                    {
                                        //Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                        Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                        if (sql1 == "")
                                        {
                                            sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
                                        }
                                        else
                                        {
                                            sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
                                        }
                                    }
                                }
                                hour_difference = 1;
                                if (ds_staff_attnd.Tables[0].Rows.Count > 0)
                                {
                                    attnd_value = ds_staff_attnd.Tables[0].Rows[0][ddd].ToString();
                                }
                                else
                                {
                                    attnd_value = "-";
                                }
                                string[] split_attnd_value = attnd_value.Split('-');
                                if (split_attnd_value.GetUpperBound(0) == 1)
                                {
                                    mng_attnd_val = split_attnd_value[0].ToString();
                                    evng_attnd_val = split_attnd_value[1].ToString();
                                    execute_query();//===============================function=======================================================
                                    //====================this code added by Manikandan for findday function on 03/10/2013=====================
                                    //=========================Modified by Venkat========================
                                    //new_sql.Close();
                                    //new_sql.Open();
                                    //string strsqlfinal = SqlFinal;
                                    //DataSet ds_sqlfinal = new DataSet();
                                    //ds_sqlfinal = dac.select_method(strsqlfinal, hat, "Text");
                                    //SqlDataReader read_period = cmd_query.ExecuteReader();
                                    string strsqlfinal = SqlFinal;
                                    DataSet ds_sqlfinal = new DataSet();
                                    ds_sqlfinal = dac.select_method_wo_parameter(strsqlfinal, "Text");
                                    //======================================================
                                    if (ds_sqlfinal.Tables[0].Rows.Count > 0)
                                        //for (int i = 0; i < ds_sqlfinal.Tables[0].Rows.Count; i++)
                                        for (int i = 0; i < 1; i++)
                                        {
                                            string strcmd = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString() + " and semester = " + ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString() + "";
                                            DataSet ds_cmd = new DataSet();
                                            // ds_cmd = dac.select_method(strcmd, hat, "Text");
                                            ds_cmd = dac.select_method_wo_parameter(strcmd, "Text");//====================Modified By Venkat====================
                                            if (ds_cmd.Tables[0].Rows.Count > 0)
                                            {
                                                if ((ds_cmd.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString()) != "")
                                                {
                                                    intNHrs = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                                                    SchOrder = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["schorder"]);
                                                    nodays = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["nodays"]);
                                                }
                                            }
                                            //dr_periodattndsched.Close();
                                            //SqlDataReader dr1;
                                            string str_seminfo = "select * from seminfo where degree_code=" + ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString() + " and semester=" + ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString() + " and batch_year=" + ds_sqlfinal.Tables[0].Rows[i]["batch_year"].ToString() + " ";
                                            //dr1 = cmd.ExecuteReader();
                                            //dr1.Read();
                                            DataSet ds_seminfo = new DataSet();
                                            // ds_seminfo = dac.select_method(str_seminfo, hat, "Text");
                                            ds_seminfo = dac.select_method_wo_parameter(str_seminfo, "Text");//====================Modified By Venkat====================
                                            if (ds_seminfo.Tables[0].Rows.Count > 0)
                                            {
                                                if ((ds_seminfo.Tables[0].Rows[0]["start_date"].ToString()) != "" && (ds_seminfo.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
                                                {
                                                    string[] tmpdate = ds_seminfo.Tables[0].Rows[0]["start_date"].ToString().Split(new char[] { ' ' });
                                                    string[] enddate = ds_seminfo.Tables[0].Rows[0]["end_date"].ToString().Split(new char[] { ' ' });
                                                    startdate = tmpdate[0].ToString();
                                                    splitenddate = enddate[0].ToString();
                                                    if (Convert.ToString(ds_seminfo.Tables[0].Rows[0]["starting_dayorder"]) != "")
                                                    {
                                                        start_dayorder = ds_seminfo.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                                    }
                                                    else
                                                    {
                                                        start_dayorder = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    errlbl.ForeColor = Color.Red;
                                                    errlbl.Text = "Update semester Information";
                                                    errlbl.Visible = true;
                                                    return;
                                                }
                                            }
                                            curr_sem = ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString();
                                            degree_code = ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString();
                                            if (intNHrs > 0)
                                            {
                                                if (SchOrder != 0)
                                                {
                                                    strday = dummy_dt.ToString("ddd");
                                                }
                                                else
                                                {
                                                    string get_date = Convert.ToString(dummy_dt);
                                                    string[] split_find_date = get_date.Split(new char[] { ' ' });
                                                    string[] split_date_only = split_find_date[0].Split(new char[] { '/' });
                                                    string findday_date = split_date_only[1] + "/" + split_date_only[0] + "/" + split_date_only[2];
                                                    //todate = SpdInfo.Sheets[0].ColumnHeader.Cells[0, 0].Text;
                                                    strday = findday(findday_date.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
                                                }
                                            }
                                            sql1 = string.Empty; Strsql = string.Empty; asql = string.Empty;
                                            sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
                                            asql = "select Alternate_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=Alternate_schedule.degree_code and semester=Alternate_schedule.semester), ";
                                            for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                            {
                                                //Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                if (sql1 == "")
                                                {
                                                    sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
                                                }
                                                else
                                                {
                                                    sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
                                                }
                                            }
                                            execute_query();
                                            //}
                                            //==================end==================================================
                                            if (hat_att.ContainsKey("Present"))
                                            {
                                                //if (mng_attnd_val == "P" || mng_attnd_val == "PER" || mng_attnd_val == "LA" || mng_attnd_val == "OD" || mng_attnd_val == "OOD")
                                                if (hat_att.ContainsValue("P") || hat_att.ContainsValue("PER") || hat_att.ContainsValue("LA") || hat_att.ContainsValue("OD") || hat_att.ContainsValue("OOD"))
                                                {
                                                    if (date == "" || date == null)
                                                    {
                                                        if (frmperddl.SelectedItem.ToString() == "All")
                                                        {
                                                            attnd_val = mng_attnd_val;
                                                            rowflag = true;
                                                            start_hour = 1;
                                                            end_hour = first_half_hr;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            //=====added by Manikandan
                                                            start_hour = first_half_hr + 1;
                                                            end_hour = tot_hrs;
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            //=====end=========
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) <= first_half_hr)
                                                            {
                                                                start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                                if (Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                                {
                                                                    end_hour = first_half_hr;
                                                                }
                                                                else
                                                                {
                                                                    end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                                }
                                                                rowflag = true;
                                                                attnd_val = mng_attnd_val;
                                                                diff_hour = end_hour - start_hour;
                                                                load_spread_period();//===============================function=======================================================
                                                                //=========added by Manikandan==19/10/2013===
                                                                start_hour = first_half_hr + 1;
                                                                end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                                attnd_val = evng_attnd_val;
                                                                diff_hour = end_hour - start_hour;
                                                                load_spread_period();//===============================function=======================================================
                                                                //==========end========
                                                            }
                                                            else
                                                            {
                                                                start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                                if (Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                                {
                                                                    //end_hour = first_half_hr;
                                                                    end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                                }
                                                                else
                                                                {
                                                                    end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                                }
                                                                rowflag = true;
                                                                attnd_val = evng_attnd_val;
                                                                diff_hour = end_hour - start_hour;
                                                                load_spread_period();//===============================function=======================================================                                            
                                                            }
                                                        }
                                                    }//
                                                    else
                                                    {
                                                        if (Convert.ToInt16(frmperddl.Text) <= first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.Text);
                                                            if (Convert.ToInt16(toperddl.Text) > first_half_hr)
                                                            {
                                                                end_hour = first_half_hr;
                                                            }
                                                            else
                                                            {
                                                                end_hour = Convert.ToInt16(toperddl.Text);
                                                            }
                                                            rowflag = true;
                                                            attnd_val = mng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================                                            
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                }
                                                //==============start=================Hided by Manikandan==17/10/2013==================
                                                ////if (evng_attnd_val == "P" || evng_attnd_val == "PER" || evng_attnd_val == "LA" || evng_attnd_val == "OD" || evng_attnd_val == "OOD")
                                                //if (hat_att.ContainsValue("P") || hat_att.ContainsValue("PER") || hat_att.ContainsValue("LA") || hat_att.ContainsValue("OD") || hat_att.ContainsValue("OOD"))
                                                //{
                                                //    if (date == "" || date == null)
                                                //    {
                                                //        if (toperddl.SelectedItem.ToString() == "All")
                                                //        {
                                                //            attnd_val = mng_attnd_val;
                                                //            rowflag = true;
                                                //            start_hour = first_half_hr + 1;
                                                //            end_hour = tot_hrs;
                                                //            diff_hour = end_hour - start_hour;
                                                //            load_spread_period();//===============================function=======================================================
                                                //        }
                                                //        else
                                                //        {
                                                //            if (Convert.ToInt16(toperddl.SelectedItem.ToString()) <= tot_hrs && Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                //            {
                                                //                if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) <= first_half_hr)
                                                //                {
                                                //                    start_hour = first_half_hr + 1;
                                                //                }
                                                //                else
                                                //                {
                                                //                    start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                //                }
                                                //                end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                //                rowflag = true;
                                                //                attnd_val = mng_attnd_val;
                                                //                diff_hour = end_hour - start_hour;
                                                //                load_spread_period();//===============================function=======================================================                                            
                                                //            }
                                                //            else
                                                //            {
                                                //            }
                                                //        }
                                                //    }//
                                                //    else
                                                //    {
                                                //        if (Convert.ToInt16(toperddl.Text) <= tot_hrs && Convert.ToInt16(toperddl.Text) > first_half_hr)
                                                //        {
                                                //            if (Convert.ToInt16(frmperddl.Text) <= first_half_hr)
                                                //            {
                                                //                start_hour = first_half_hr + 1;
                                                //            }
                                                //            else
                                                //            {
                                                //                start_hour = Convert.ToInt16(frmperddl.Text);
                                                //            }
                                                //            end_hour = Convert.ToInt16(toperddl.Text);
                                                //            rowflag = true;
                                                //            attnd_val = mng_attnd_val;
                                                //            diff_hour = end_hour - start_hour;
                                                //            load_spread_period();//===============================function=======================================================                                            
                                                //        }
                                                //        else
                                                //        {
                                                //        }
                                                //    }
                                                //}
                                                //==============End=================Hided by Manikandan==17/10/2013==================
                                                if (rowflag == false)
                                                {
                                                    //load_spread.Sheets[0].RowCount = load_spread.Sheets[0].RowCount - 1;//Hided by Manikandan 07/10/2013
                                                }
                                            }////////////////////present
                                            else if (hat_att.ContainsKey("Absent"))
                                            {
                                                //if (mng_attnd_val == "A" || mng_attnd_val == "RL" || mng_attnd_val == "NA" || mng_attnd_val == "CL")
                                                if (hat_att.ContainsValue("A") || hat_att.ContainsValue("RL") || hat_att.ContainsValue("NA") || hat_att.ContainsValue("CL"))
                                                {
                                                    if (date == "" || date == null)
                                                    {
                                                        if (toperddl.SelectedItem.ToString() == "All")
                                                        {
                                                            start_hour = 1;
                                                            end_hour = first_half_hr;
                                                            rowflag = true;
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            //=====added by Manikandan
                                                            start_hour = first_half_hr + 1;
                                                            end_hour = tot_hrs;
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            //=====end=========
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) <= first_half_hr)
                                                            {
                                                                start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                                if (Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                                {
                                                                    end_hour = first_half_hr;
                                                                }
                                                                else
                                                                {
                                                                    end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                                }
                                                                rowflag = true;
                                                                attnd_val = evng_attnd_val;
                                                                diff_hour = end_hour - start_hour;
                                                                load_spread_period();//===============================function=======================================================                                            
                                                            }
                                                            else
                                                            {
                                                            }
                                                        }
                                                    }//
                                                    else
                                                    {
                                                        if (Convert.ToInt16(frmperddl.Text) <= first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.Text);
                                                            if (Convert.ToInt16(toperddl.Text) > first_half_hr)
                                                            {
                                                                end_hour = first_half_hr;
                                                            }
                                                            else
                                                            {
                                                                end_hour = Convert.ToInt16(toperddl.Text);
                                                            }
                                                            rowflag = true;
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================                                            
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                }
                                                //==============Start=================Hided by Manikandan==17/10/2013==================
                                                ////if (evng_attnd_val == "A" || evng_attnd_val == "RL" || evng_attnd_val == "NA" || evng_attnd_val == "CL")
                                                //if (hat_att.ContainsValue("A") || hat_att.ContainsValue("RL") || hat_att.ContainsValue("NA") || hat_att.ContainsValue("CL"))
                                                //{
                                                //    if (date == "" || date == null)
                                                //    {
                                                //        if (toperddl.SelectedItem.ToString() == "All")
                                                //        {
                                                //            attnd_val = evng_attnd_val;
                                                //            rowflag = true;
                                                //            start_hour = first_half_hr + 1;
                                                //            end_hour = tot_hrs;
                                                //            diff_hour = end_hour - start_hour;
                                                //            load_spread_period();//===============================function=======================================================
                                                //        }
                                                //        else
                                                //        {
                                                //            if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) <= first_half_hr && Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                //            {
                                                //                start_hour = first_half_hr + 1;
                                                //                end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                //                rowflag = true;
                                                //                attnd_val = evng_attnd_val;
                                                //                diff_hour = end_hour - start_hour;
                                                //                load_spread_period();//===============================function=======================================================                                            
                                                //            }
                                                //            else if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) > first_half_hr)
                                                //            {
                                                //                start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                //                end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                //                rowflag = true;
                                                //                attnd_val = evng_attnd_val;
                                                //                diff_hour = end_hour - start_hour;
                                                //                load_spread_period();//===============================function=======================================================                                            
                                                //            }
                                                //            else
                                                //            {
                                                //            }
                                                //        }
                                                //    }//
                                                //    else
                                                //    {
                                                //        {
                                                //            if (Convert.ToInt16(frmperddl.Text) <= first_half_hr && Convert.ToInt16(toperddl.Text) > first_half_hr)
                                                //            {
                                                //                start_hour = first_half_hr + 1;
                                                //                end_hour = Convert.ToInt16(toperddl.Text);
                                                //                rowflag = true;
                                                //                attnd_val = evng_attnd_val;
                                                //                diff_hour = end_hour - start_hour;
                                                //                load_spread_period();//===============================function=======================================================                                            
                                                //            }
                                                //            else if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) > first_half_hr)
                                                //            {
                                                //                start_hour = Convert.ToInt16(frmperddl.Text);
                                                //                end_hour = Convert.ToInt16(toperddl.Text);
                                                //                rowflag = true;
                                                //                attnd_val = evng_attnd_val;
                                                //                diff_hour = end_hour - start_hour;
                                                //                load_spread_period();//===============================function=======================================================                                            
                                                //            }
                                                //            else
                                                //            {
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                                //==============End=================Hided by Manikandan==17/10/2013==================
                                                if (rowflag == false)
                                                {
                                                    load_spread.Sheets[0].RowCount = load_spread.Sheets[0].RowCount - 1;
                                                }
                                            }
                                            else
                                            {
                                                if (date == "" || date == null)
                                                {
                                                    if (toperddl.SelectedItem.ToString() == "All" || frmperddl.SelectedItem.ToString() == "All")
                                                    {
                                                        start_hour = 1;
                                                        end_hour = first_half_hr;
                                                        attnd_val = mng_attnd_val;
                                                        diff_hour = end_hour - start_hour;
                                                        load_spread_period();//===============================function=======================================================
                                                        start_hour = first_half_hr + 1;
                                                        end_hour = tot_hrs;
                                                        attnd_val = evng_attnd_val;
                                                        diff_hour = end_hour - start_hour;
                                                        load_spread_period();//===============================function=======================================================
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToInt16(toperddl.SelectedItem.ToString()) <= first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                            end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                            attnd_val = mng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                        if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) > first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                            end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                        if (Convert.ToInt16(frmperddl.SelectedItem.ToString()) <= first_half_hr && Convert.ToInt16(toperddl.SelectedItem.ToString()) > first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.SelectedItem.ToString());
                                                            end_hour = first_half_hr;
                                                            attnd_val = mng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            start_hour = first_half_hr + 1;
                                                            end_hour = Convert.ToInt16(toperddl.SelectedItem.ToString());
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                    }
                                                }//
                                                else
                                                {
                                                    {
                                                        if (Convert.ToInt16(toperddl.Text) <= first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.Text);
                                                            end_hour = Convert.ToInt16(toperddl.Text);
                                                            attnd_val = mng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                        if (Convert.ToInt16(frmperddl.Text) > first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.Text);
                                                            end_hour = Convert.ToInt16(toperddl.Text);
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                        if (Convert.ToInt16(frmperddl.Text) <= first_half_hr && Convert.ToInt16(toperddl.Text) > first_half_hr)
                                                        {
                                                            start_hour = Convert.ToInt16(frmperddl.Text);
                                                            end_hour = first_half_hr;
                                                            attnd_val = mng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                            start_hour = first_half_hr + 1;
                                                            end_hour = Convert.ToInt16(toperddl.Text);
                                                            attnd_val = evng_attnd_val;
                                                            diff_hour = end_hour - start_hour;
                                                            load_spread_period();//===============================function=======================================================
                                                        }
                                                    }
                                                }
                                            }
                                        }//===============================while loop end===================
                                    rowflag = false;
                                }
                            }
                        }
                        dummy_dt = dummy_dt.AddDays(1);
                    }
                }
            }
            if (chkfreestaff.Checked == true)
            {
                if (hatfeestaff.Contains(staff_code))
                {
                    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].Visible = true;
                }
                else
                {
                    sno--;
                    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].Visible = false;
                }
            }
        }
    }

    public void execute_query()
    {
        checkflag = false;
        norec_flag = false;
        sql1 = "(" + sql1 + ")";
        sql_s = sql_s + Strsql + "";
        asql = asql + Strsql + "";
        string day_from = dummy_dt.ToString("yyyy-MM-dd");
        SqlBatchYear = "(select distinct(registration.batch_year) from registration,semester_schedule where registration.degree_code=semester_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = semester_schedule.semester)";
        SqlPrefinal1 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
        SqlPrefinal2 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
        SqlPrefinal3 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
        SqlPrefinal4 = sql_s + " semester,sections,batch_year from semester_schedule where lastrec=1 and batch_year in " + SqlBatchYear + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
        SqlFinal = "(" + SqlPrefinal1 + ") union all (" + SqlPrefinal4 + ") union all (" + SqlPrefinal2 + ") union all (" + SqlPrefinal3 + ")";
        SqlBatchYear1 = "(select distinct(registration.batch_year) from registration,Alternate_schedule where registration.degree_code=Alternate_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = Alternate_schedule.semester)";
        SqlPrefinal11 = asql + " semester,sections,batch_year from Alternate_schedule where batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
        SqlPrefinal22 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate ='" + day_from + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
        SqlPrefinal33 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate ='" + day_from + "' and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
        SqlPrefinal44 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate ='" + day_from + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
        SqlFinal1 = "(" + SqlPrefinal11 + ") union all (" + SqlPrefinal44 + ") union all (" + SqlPrefinal22 + ") union all (" + SqlPrefinal33 + ")";
    }

    public void load_spread_period()
    {
        //=************************************************************************
        cmd_query = new SqlCommand(SqlFinal, mysql);
        mysql.Close();
        mysql.Open();
        SqlDataReader read_period = cmd_query.ExecuteReader();
        if (read_period.HasRows)
            while (read_period.Read())
            {
                temp_inc_hour = hour_difference;
                norec_flag = true;
                for (temp = start_hour; temp <= end_hour; temp++)
                {
                    increment_col_count = ((increment_day_count * preiod_diff) - preiod_diff) + 4 + temp_inc_hour;
                    {
                        if (read_period[temp + 1].ToString() != "" && read_period[temp + 1].ToString() != null && read_period[temp + 1].ToString() != "\0")
                        {
                            string sp_rd = read_period[temp + 1].ToString();
                            string[] sp_rd_semi = sp_rd.Split(';');
                            for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                            {
                                string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                if (sp2.GetUpperBound(0) >= 1)
                                {
                                    for (int staffcnt = 1; staffcnt <= sp2.GetUpperBound(0) - 1; staffcnt++)
                                    {
                                        if (sp2[staffcnt] == (string)staff_code) //if (sp2[1] == (string)staff_code)
                                        {
                                            string sect = read_period["sections"].ToString();
                                            cmd = new SqlCommand("select end_date,semester,start_date from seminfo where degree_code=" + read_period["degree_code"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " and semester='" + read_period["semester"].ToString() + "'", mysql1);//=========15/6/12 PRABHA
                                            mysql1.Close();
                                            mysql1.Open();
                                            SqlDataReader read_sem = cmd.ExecuteReader();
                                            if (read_sem.Read())
                                            {
                                                //  if (read_sem[0].ToString() == read_period["semester"].ToString())
                                                if (dummy_dt <= (Convert.ToDateTime(read_sem[0])))//=========15/6/12 PRABHA
                                                {
                                                    check_hour = true;
                                                    text_temp = GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[2];
                                                    text_temp_1 = GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "");
                                                    string Schedule_string = string.Empty;
                                                    if (read_period["sections"].ToString() == "-1")
                                                    {
                                                        Schedule_string = read_period["degree_code"].ToString() + "-" + read_period["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_period["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                                    }
                                                    else
                                                    {
                                                        Schedule_string = read_period["degree_code"].ToString() + "-" + read_period["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_period["sections"].ToString() + "-" + read_period["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                                    }
                                                    //--------------check holiday
                                                    cmd.CommandText = "select holiday_desc from holidaystudents where holiday_date='" + dummy_dt + "'and degree_code=" + read_period["degree_code"].ToString() + " and semester=" + read_period["semester"].ToString();
                                                    cmd.Connection = mysql2;
                                                    mysql2.Close();
                                                    mysql2.Open();
                                                    SqlDataReader dr_holday1 = cmd.ExecuteReader();
                                                    if (dr_holday1.Read())
                                                    {
                                                        if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                                        {
                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp;
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                            {
                                                                //gowthaman 25july2013 load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                            }
                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = "Selected day is Holiday For Students- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                                        }
                                                        else
                                                        {
                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                            {
                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp)
                                                                {
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp;
                                                                }
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                            {
                                                                //gowthaman 25july2013   if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "")
                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                                {
                                                                    //gowthaman 25july2013  load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                }
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                            }
                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + "Selected day is Holiday- Reason-" + dr_holday1.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                                        }
                                                    }
                                                    //=============Start====================this part added by Manikandan for single subject==============================
                                                    else if (subjddl.Items.Count > 0 && subjddl.SelectedItem.ToString() != "All" && subjddl.SelectedItem.ToString() != string.Empty)
                                                    {
                                                        //if (subjddl.SelectedValue.ToString() == sp2[0])
                                                        if (subjddl.SelectedItem.ToString() == text_temp_1)
                                                        {
                                                            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                                            {
                                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                                {
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                                }
                                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                {
                                                                    //gowthaman 25july2013 load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                }
                                                                if (optradio.SelectedItem.ToString() == "Color")
                                                                {
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                }
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = Schedule_string.ToString() + "-sem";
                                                            }
                                                            else
                                                            {
                                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                                {
                                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp)
                                                                    {
                                                                        //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp;
                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                                    }
                                                                }
                                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                {
                                                                    //gowthaman 25july2013  if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "")
                                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                                    {
                                                                        //gowthaman 25july2013  load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                        //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                    }
                                                                }
                                                                if (optradio.SelectedItem.ToString() == "Color")
                                                                {
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                }
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + Schedule_string.ToString() + "-sem";
                                                            }
                                                        }
                                                        //----------------set color
                                                    }
                                                    else
                                                    {
                                                        if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                                        {
                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                            {
                                                                //gowthaman 25july2013 load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                            }
                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = Schedule_string.ToString() + "-sem";
                                                        }
                                                        else
                                                        {
                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                            {
                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp)
                                                                {
                                                                    //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp;
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                                }
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                            {
                                                                //gowthaman 25july2013  if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "")
                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                                {
                                                                    //gowthaman 25july2013  load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                                    //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                }
                                                            }
                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                            {
                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                            }
                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + Schedule_string.ToString() + "-sem";
                                                        }
                                                        //----------------set color
                                                    }
                                                    //=========================
                                                }
                                            }
                                            read_sem.Close();
                                            mysql1.Close();
                                        }
                                    }
                                }
                                //==========if hour avilable means check alter for that hr
                                string query_alter = string.Empty;
                                if (check_hour == true)
                                {
                                    if (read_period["sections"].ToString() == "-1")
                                    {
                                        query_alter = "select " + strday.Trim() + temp + ",degree_code,semester,batch_year,sections  from Alternate_schedule where fromdate='" + dummy_dt.ToString("yyyy-MM-dd") + "' and degree_code=" + read_period["degree_code"].ToString() + " and semester=" + read_period["semester"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " ";
                                    }
                                    else
                                    {
                                        query_alter = "select " + strday.Trim() + temp + ",degree_code,semester,batch_year,sections  from Alternate_schedule where fromdate='" + dummy_dt.ToString("yyyy-MM-dd") + "' and degree_code=" + read_period["degree_code"].ToString() + " and semester=" + read_period["semester"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " and sections='" + read_period["sections"].ToString() + "'";
                                    }
                                    cmd.CommandText = query_alter;
                                    ssql.Close();
                                    cmd.Connection = ssql;
                                    ssql.Open();
                                    SqlDataReader read_alter = cmd.ExecuteReader();
                                    if (read_alter.HasRows)
                                    {
                                        // for ( temp = 1; temp <= noofhrs; temp++)
                                        while (read_alter.Read())
                                        {
                                            sp_rd = read_alter.GetValue(0).ToString();
                                            string[] sp_rd_split = sp_rd.Split(';');
                                            for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                            {
                                                sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                sp2 = sp_rd.Split(new Char[] { '-' });
                                                if (sp2.GetUpperBound(0) >= 1)
                                                {
                                                    if (sp2[1] == (string)staff_code)
                                                    {
                                                        string sect = read_period["sections"].ToString();
                                                        //if (sect != "-1" && sect != null && sect.Trim() != "")
                                                        //    cmd =new SqlCommand ( "select current_semester from registration where degree_code=" + read_period["degree_code"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " and sections='" + read_period["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'",mysql1);
                                                        //else
                                                        //{
                                                        //    sect =string.Empty;
                                                        //    cmd =new SqlCommand ( "select current_semester from registration where degree_code=" + read_period["degree_code"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'",mysql1);
                                                        //}
                                                        cmd = new SqlCommand("select end_date,semester from seminfo where degree_code=" + read_period["degree_code"].ToString() + " and batch_year=" + read_period["batch_year"].ToString() + " and semester='" + read_period["semester"].ToString() + "'", mysql1);//=========15/6/12 PRABHA
                                                        mysql1.Close();
                                                        mysql1.Open();
                                                        SqlDataReader read_sem = cmd.ExecuteReader();
                                                        if (read_sem.Read())
                                                        {
                                                            // if (read_sem[0].ToString() == read_period["semester"].ToString())
                                                            if (dummy_dt <= (Convert.ToDateTime(read_sem[0])))//=========15/6/12 PRABHA
                                                            {
                                                                text_temp = GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[2];
                                                                string Schedule_string = string.Empty;
                                                                if (read_period["sections"].ToString() == "-1")
                                                                {
                                                                    Schedule_string = read_period["degree_code"].ToString() + "-" + read_period["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_period["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                                                }
                                                                else
                                                                {
                                                                    Schedule_string = read_period["degree_code"].ToString() + "-" + read_period["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_period["sections"].ToString() + "-" + read_period["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                                                }
                                                                //--------------check holiday
                                                                cmd.CommandText = "select holiday_desc from holidaystudents where holiday_date='" + dummy_dt + "'and degree_code=" + read_period["degree_code"].ToString() + " and semester=" + read_period["semester"].ToString();
                                                                cmd.Connection = mysql2;
                                                                mysql2.Close();
                                                                mysql2.Open();
                                                                SqlDataReader dr_holday2 = cmd.ExecuteReader();
                                                                if (dr_holday2.Read())
                                                                {
                                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                                                    {
                                                                        if (optradio.SelectedItem.ToString() == "Subject")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp;
                                                                        }
                                                                        if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                        }
                                                                        if (optradio.SelectedItem.ToString() == "Color")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                        }
                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                                                    }
                                                                    else
                                                                    {
                                                                        {
                                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                                            {
                                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp)
                                                                                {
                                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp;
                                                                                }
                                                                            }
                                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                            {
                                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                                                {
                                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                                }
                                                                            }
                                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                                            {
                                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                            }
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday2.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //===============================
                                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                                                    {
                                                                        if (optradio.SelectedItem.ToString() == "Subject")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                                        }
                                                                        if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                        }
                                                                        if (optradio.SelectedItem.ToString() == "Color")
                                                                        {
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                        }
                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = Schedule_string.ToString() + "-alter";
                                                                    }
                                                                    else
                                                                    {
                                                                        {
                                                                            if (optradio.SelectedItem.ToString() == "Subject")
                                                                            {
                                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp)
                                                                                {
                                                                                    //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp;
                                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                                                }
                                                                            }
                                                                            if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                                            {
                                                                                if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                                                {
                                                                                    //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_period["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_period["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                                                }
                                                                            }
                                                                            if (optradio.SelectedItem.ToString() == "Color")
                                                                            {
                                                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                                            }
                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + Schedule_string.ToString() + "-alter";
                                                                        }
                                                                    }
                                                                }
                                                                //=========================
                                                            }
                                                        }
                                                        read_sem.Close();
                                                        mysql1.Close();
                                                    }
                                                    else
                                                    {
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = " ";
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = " ";
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightGray;
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Note = "Staff Free";
                                                        //col_color_new = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    check_hour = false;
                                }
                                //==========================================================
                            }
                        }
                    }
                    set_color();
                    temp_inc_hour++;
                }
                //  checkflag = true;
            }
        checkflag = false;
        cmd_query = new SqlCommand(SqlFinal1, con_query);
        con_query.Close();
        con_query.Open();
        SqlDataReader read_alterperiod = cmd_query.ExecuteReader();
        while (read_alterperiod.Read())
        {
            temp_inc_hour = hour_difference;
            norec_flag = true;
            for (temp = start_hour; temp <= end_hour; temp++)
            {
                increment_col_count = ((increment_day_count * preiod_diff) - preiod_diff) + 4 + temp_inc_hour;
                if (read_alterperiod[temp + 1].ToString() != "" && read_alterperiod[temp + 1].ToString() != null && read_alterperiod[temp + 1].ToString() != "\0")
                {
                    string sp_rd = read_alterperiod[temp + 1].ToString();
                    string[] sp_rd_semi = sp_rd.Split(';');
                    for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                    {
                        string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                        if (sp2.GetUpperBound(0) >= 1)
                        {
                            if (sp2[1] == (string)staff_code)
                            {
                                string sect = read_alterperiod["sections"].ToString();
                                //if (sect != "-1" && sect != null && sect.Trim() != "")
                                //    cmd=new SqlCommand ( "select current_semester from registration where degree_code=" + read_alterperiod["degree_code"].ToString() + " and batch_year=" + read_alterperiod["batch_year"].ToString() + " and sections='" + read_alterperiod["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'",mysql1);
                                //else
                                //{
                                //    sect =string.Empty;
                                //    cmd =new SqlCommand ( "select current_semester from registration where degree_code=" + read_alterperiod["degree_code"].ToString() + " and batch_year=" + read_alterperiod["batch_year"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'",mysql1);
                                //}
                                cmd_new1 = new SqlCommand("select end_date,semester from seminfo where degree_code=" + read_alterperiod["degree_code"].ToString() + " and batch_year=" + read_alterperiod["batch_year"].ToString() + " and semester='" + read_alterperiod["semester"].ToString() + "'", mysql1);//=========15/6/12 PRABHA
                                mysql1.Close();
                                mysql1.Open();
                                SqlDataReader read_sem = cmd_new1.ExecuteReader();
                                if (read_sem.Read())
                                {
                                    // if (read_sem[0].ToString() == read_alterperiod["semester"].ToString())
                                    if (dummy_dt <= (Convert.ToDateTime(read_sem[0])))//=========15/6/12 PRABHA
                                    {
                                        text_temp = GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[2];
                                        string Schedule_string = string.Empty;
                                        if (read_alterperiod["sections"].ToString() == "-1")
                                        {
                                            Schedule_string = read_alterperiod["degree_code"].ToString() + "-" + read_alterperiod["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_alterperiod["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                        }
                                        else
                                        {
                                            Schedule_string = read_alterperiod["degree_code"].ToString() + "-" + read_alterperiod["semester"].ToString() + "-" + sp2[0].ToString() + "-" + read_alterperiod["sections"].ToString() + "-" + read_alterperiod["batch_year"].ToString() + "-" + sp2[2].ToString() + "-" + sp_rd_semi.GetUpperBound(0);
                                        }
                                        //--------------check holiday
                                        string test = "select holiday_desc from holidaystudents where holiday_date='" + dummy_dt + "'and degree_code=" + read_alterperiod["degree_code"].ToString() + " and semester=" + read_alterperiod["semester"].ToString() + "";
                                        //  cmd_new.CommandText = "select holiday_desc from holidaystudents where holiday_date='" + dummy_dt + "'and degree_code=" + read_alterperiod["degree_code"].ToString() + " and semester=" + read_alterperiod["semester"].ToString();
                                        //cmd_new.Connection = mysql2;
                                        mysql2.Close();
                                        mysql2.Open();
                                        cmd_new = new SqlCommand(test, mysql2);
                                        SqlDataReader dr_holday3 = cmd_new.ExecuteReader();
                                        if (dr_holday3.Read())
                                        {
                                            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                            {
                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp;
                                                }
                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                }
                                                if (optradio.SelectedItem.ToString() == "Color")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                }
                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                            }
                                            else
                                            {
                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                {
                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp)
                                                    {
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp;
                                                    }
                                                }
                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                {
                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                    {
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                    }
                                                }
                                                if (optradio.SelectedItem.ToString() == "Color")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                }
                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + "Selected day is Holiday For Students- Reason-" + dr_holday3.GetValue(0).ToString() + "-" + Schedule_string.ToString() + "-sem";
                                            }
                                        }
                                        else
                                        {
                                            //===============================
                                            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text.Trim() == "")
                                            {
                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp;
                                                }
                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                {
                                                    //load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["current_semester"].ToString() + " " + sect + "";
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";// this line modified by Manikandan from above above commented line on 08/10/2013
                                                }
                                                if (optradio.SelectedItem.ToString() == "Color")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                }
                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = Schedule_string.ToString() + "-sem";
                                            }
                                            else
                                            {
                                                if (optradio.SelectedItem.ToString() == "Subject")
                                                {
                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp)
                                                    {
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp;
                                                    }
                                                }
                                                if (optradio.SelectedItem.ToString() == "Subject and Class")
                                                {
                                                    if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "")
                                                    {
                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text + " * " + text_temp + "  " + read_alterperiod["batch_year"].ToString() + "-" + GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-" + GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + read_alterperiod["degree_code"].ToString() + "") + "-Sem" + read_sem["semester"].ToString() + " " + sect + "";
                                                    }
                                                }
                                                if (optradio.SelectedItem.ToString() == "Color")
                                                {
                                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text = "a";
                                                }
                                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag = load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Tag + " * " + Schedule_string.ToString() + "-sem";
                                            }
                                            //----------------set color
                                        }
                                    }
                                } read_sem.Close();
                                mysql1.Close();
                            }
                            else//this condition added by Manikandan 08/10/2013
                            {
                                //col_color_new = 1;
                            }
                        }
                    }
                }
                set_color();
                temp_inc_hour++;
            }
        }
        read_period.Close();
        mysql.Close();
        if (norec_flag == false)
        {
            if (frmperddl.Items.Count > 0 && frmperddl.SelectedItem.ToString() != "All")
            {
                if (start_hour == Convert.ToInt16(frmperddl.SelectedItem.ToString()))
                {
                    for (int t = ((increment_day_count * preiod_diff) - preiod_diff) + 3; t <= ((increment_day_count * preiod_diff) - preiod_diff + diff_hour) + 3; t++)
                    {
                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), t].BackColor = Color.LightGray;
                    }
                }
                else
                {
                    for (int t = ((increment_day_count * preiod_diff) - diff_hour) + 2; t <= (increment_day_count * preiod_diff) + 2; t++)
                    {
                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), t].BackColor = Color.LightGray;
                    }
                }
            }
            else
            {
                for (int t = ((increment_day_count * preiod_diff) - preiod_diff) + 3 + (start_hour - 1); t <= ((increment_day_count * preiod_diff) - preiod_diff + diff_hour) + 3 + (start_hour - 1); t++)
                {
                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), t].BackColor = Color.LightGray;
                }
            }
        }
        hour_difference = temp_inc_hour;
        //=************************************************************************
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        errmsg.Visible = false;
        pagesearch_txt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            pageddltxt.Visible = true;
            pageddltxt.Focus();
        }
        else
        {
            pageddltxt.Visible = false;
            load_spread.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    //----------record per page "other" value per page
    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        load_spread.CurrentPage = 0;
        int row_cnt = 0;
        row_cnt = load_spread.Sheets[0].RowCount;
        pagesearch_txt.Text = string.Empty;
        try
        {
            if (load_spread.Sheets[0].RowCount > Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
            {
                if (pageddltxt.Text != "")
                {
                    errmsg.Visible = false;
                    load_spread.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    CalculateTotalPages();
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter valid Record count";
            }
        }
        catch
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter valid Record count";
        }
        //if (Convert.ToInt16(pageddltxt.Text) < row_cnt)
        //{
        //    errmsg.Visible = false;
        //}
        //else
        //{
        //    errmsg.Visible = true;
        //    errmsg.Text = "Please Enter valid Record count";
        //}
    }

    //-------------------page search text
    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        if (pagesearch_txt.Text.Trim() != "")
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                load_spread.CurrentPage = 0;
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                pagesearch_txt.Text = string.Empty;
                load_spread.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                load_spread.CurrentPage = 0;
                errmsg.Visible = true;
                errmsg.Text = "Page search should be more than 0";
                pagesearch_txt.Text = string.Empty;
                load_spread.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
            }
            else
            {
                errmsg.Visible = false;
                load_spread.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                load_spread.Visible = true;
                btnprintmaster.Visible = true;
            }
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(load_spread.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / load_spread.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    //protected void periodchk_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (periodchk.Checked == true)
    //    {
    //        frmperddl.Items.Clear();
    //        toperddl.Items.Clear();
    //        toperiod();
    //        fmperiod();
    //        frmperddl.Visible = true;
    //        frmperlbl.Visible = true;
    //        toperddl.Visible = true;
    //        toperlbl.Visible = true;
    //    }
    //    else
    //    {
    //        frmperddl.Items.Clear();
    //        toperddl.Items.Clear();
    //        toperiod();
    //        fmperiod();
    //        frmperddl.Visible = false;
    //        frmperlbl.Visible = false;
    //        toperddl.Visible = false;
    //        toperlbl.Visible = false;
    //        diffperlbl.Visible = false;
    //    }
    //}

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        con5.Close();
        con5.Open();
        SqlDataReader drnew;
        SqlCommand cmd;
        cmd_get = new SqlCommand(sqlstr, con5);
        cmd_get.Connection = con5;
        drnew = cmd_get.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected void desigddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        Panel3.Visible = false;
        load_tree.Visible = false;
        okbtn.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        //bindstaff();
        deptddl_SelectedIndexChanged(sender, e);
        //bindstaff_desigddl();
        // bindsubject();
    }

    protected void deptddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            load_spread.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            errlbl.Visible = false;
            colorpnl.Visible = false;
            fmlbl.Visible = false;
            tolbl.Visible = false;
            Panel3.Visible = false;
            load_tree.Visible = false;
            okbtn.Visible = false;
            chk_sms.Visible = false;
            chk_mail.Visible = false;
            txt_message.Visible = false;
            btnsms.Visible = false;
            string staffdesig = string.Empty;//declared by Manikandan on 07/10/2013
            string get_degreecode = string.Empty;//declared by Manikandan on 11/10/2013
            stafftxt.Items.Clear();
            string staff_name = "", dept_code = "", s_dept_code = "", r_dept_code = string.Empty;
            if (deptddl.Text != "" && deptddl.Text != "All")
            {
                get_degreecode = d2.GetFunction("select degree_code from degree where dept_code='" + deptddl.SelectedValue.ToString() + "'");
                if (deptddl.SelectedItem.ToString() != "All" && deptddl.SelectedItem.ToString() != "")
                {
                    //dept_code = " and de.degree_code=" + deptddl.SelectedValue.ToString();
                    //s_dept_code = " and syllabus_master.degree_code=" + deptddl.SelectedValue.ToString();
                    //r_dept_code = " and  registration.degree_code=" + deptddl.SelectedValue.ToString();
                    dept_code = " and h.dept_code=" + deptddl.SelectedValue.ToString();
                    if (get_degreecode != string.Empty)
                    {
                        s_dept_code = " and syllabus_master.degree_code=" + get_degreecode;
                        r_dept_code = " and  r.degree_code=" + get_degreecode;
                    }
                }
                else
                {
                    dept_code = string.Empty;
                }
            }
            else
            {
                dept_code = string.Empty;
            }
            if (desigddl.Items.Count > 0)
            {
                if (desigddl.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    staffdesig = " and d.desig_name='" + desigddl.SelectedItem.ToString() + "'";
                }
                else
                {
                    staffdesig = string.Empty;
                }
            }
            //Modified By Srinath 25/6/2015 Adding distinct
            //  string strstaff = "select distinct staff_name,m.staff_code from staffmaster m, degree de,stafftrans t,hrdept_master h,desig_master d where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and  t.desig_code = d.desig_code and latestrec = 1  and t.dept_code = h.dept_code " + staffdesig + " and staffcategory='Teaching' " + dept_code + " and de.dept_code=h.dept_code  order by staff_name";
            string strstaff = "select distinct staff_name,m.staff_code from staffmaster m, stafftrans t,hrdept_master h,desig_master d where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and  t.desig_code = d.desig_code and latestrec = 1 and t.dept_code = h.dept_code and m.college_code=d.collegeCode and m.college_code=d.collegeCode " + dept_code + " " + staffdesig + " and staffcategory='Teaching' order by staff_name";
            DataSet ds_staffname = d2.select_method_wo_parameter(strstaff, "Text");
            if (ds_staffname.Tables.Count > 0 && ds_staffname.Tables[0].Rows.Count > 0)
            {
                stafftxt.DataSource = ds_staffname.Tables[0];
                stafftxt.DataValueField = "staff_code";
                stafftxt.DataTextField = "staff_name";
                stafftxt.DataBind();
                stafftxt.Items.Insert(0, "All");
            }
            //*********************bindsubjectt
            subjddl.Items.Clear();
            //Modified By Srinath 7/3/2013 Adding distinct
            //cmd = new SqlCommand("select distinct subject_name,subject_code from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master,exam_type where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and  subject.syll_code=syllabus_master.syll_code " + s_dept_code + "  and syllabus_master.semester=registration.current_semester and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no " + r_dept_code + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no", con);
            string strsubjec = "select distinct s.subject_name,s.subject_code from stafftrans st,staff_selector ss,subject s,sub_sem sm, syllabus_master sy,Registration r,hrdept_master hd where st.staff_code=ss.staff_code and st.dept_code=hd.dept_code and ss.subject_no=s.subject_no and s.subType_no=sm.subType_no and s.syll_code=sy.syll_code and sm.syll_code=sy.syll_code and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester " + r_dept_code + "";
            DataSet ds = d2.select_method_wo_parameter(strsubjec, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                subjddl.DataSource = ds.Tables[0];
                subjddl.DataValueField = "subject_code";
                subjddl.DataTextField = "subject_name";
                subjddl.DataBind();
                subjddl.Items.Insert(0, "All");
            }
        }
        catch (TimeoutException ex)
        {
            errlbl.Text = "Timeout please try again by clicking Go button";
        }
    }

    public void load_spread_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            errlbl.Visible = false;
            ////Start=================Added by Manikandan 15/10/2013===============
            ////if (workload_click == true)
            ////{
            //    if (load_spread.Sheets[0].ActiveColumn == 1 && load_spread.Sheets[0].ActiveRow == 0)
            //    {
            //        if (Convert.ToInt32(load_spread.Sheets[0].Cells[0, 1].Value) == 1)
            //        {
            //            for (int i = 1; i < load_spread.Sheets[0].RowCount; i++)
            //            {
            //                load_spread.Sheets[0].Cells[i, 1].Value = 1;
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 1; i < load_spread.Sheets[0].RowCount; i++)
            //            {
            //                load_spread.Sheets[0].Cells[i, 1].Value = 0;
            //            }
            //        }
            //    }
            ////}
            //====================================End============================
            if (loadflag == true)
            {
                //else
                //{
                load_tree.Visible = true;
                //okbtn.Visible = true;//temp hided by Manikandan
                int ar = 0;
                int ac = 0;
                string suubname_deg_sem = string.Empty;
                string text_val = string.Empty;
                string subjname = string.Empty;
                string batch_year = string.Empty;
                string staff_code = string.Empty;
                ar = load_spread.Sheets[0].ActiveRow;
                ac = load_spread.Sheets[0].ActiveColumn;
                staff_code = load_spread.Sheets[0].Cells[ar, 0].Note;
                if (ac == 3)
                {
                    if (load_spread.Sheets[0].Cells[ar, ac].Text == "")
                    {
                        suubname_deg_sem = Session["suubname_deg_sem"].ToString();
                        //=========tree
                        Session.Remove("suubname_deg_sem");
                        string[] sp1 = suubname_deg_sem.Split('-');
                        string sections = "", sem = "", byear = "", subj_count_in_onehr = "", subj_no = string.Empty;
                        //-----------------0n 11/7/12
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            sem = sp1[1];
                            byear = sp1[4];
                            subj_no = sp1[2];
                            subj_count_in_onehr = sp1[6];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            sem = sp1[1];
                            subj_no = sp1[2];
                            subj_count_in_onehr = sp1[5];
                        }
                        string[] splitsubj = suubname_deg_sem.Split(new Char[] { '-' });
                        subjname = splitsubj[0].ToString();
                        //----batch year
                        batch_year = "select distinct batch_year from syllabus_master sy ,subject s where sy.syll_code=s.syll_code and semester=" + sem + " and s.subject_no=" + subj_no + "";
                        DataSet dsbyear = d2.select_method_wo_parameter(batch_year, "Text");
                        if (dsbyear.Tables[0].Rows.Count > 0)
                        {
                            string Syllabus_year = string.Empty;
                            Syllabus_year = GetSyllabusYear(sp1[0].ToString(), sem, byear);
                            if (Syllabus_year != "-1")
                            {
                                //--------------get subject type and subjects
                                cona.Close();
                                cona.Open();
                                SqlDataReader subTypeRs;
                                cmda = new SqlCommand("select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + sp1[0].ToString() + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + byear + ") order by subject.subtype_no", cona);
                                subTypeRs = cmda.ExecuteReader();
                                TreeNode node;
                                int rec_count = 0;
                                while (subTypeRs.Read())
                                {
                                    if ((subTypeRs["subject_type"].ToString()) != "0")
                                    {
                                        SqlDataReader subTypeRs1;
                                        con1a.Close();
                                        con1a.Open();
                                        cmd1a = new SqlCommand("select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + sp1[0].ToString() + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + byear + ") and subject.subtype_no=" + subTypeRs["subtype_no"] + " order by subject.subtype_no,subject.subject_no", con1a);
                                        subTypeRs1 = cmd1a.ExecuteReader();
                                        node = new TreeNode(subTypeRs["subject_type"].ToString(), rec_count.ToString());
                                        while (subTypeRs1.Read())//-------------set to tree
                                        {
                                            if (subTypeRs1["subject_name"].ToString() != "0" && subTypeRs1["subject_name"].ToString() != subjname)
                                            {
                                                //node.ChildNodes.Add(new TreeNode(subTypeRs1["subject_name"].ToString(),subj_no  + "-" + splitsubj[1] + "-" + subTypeRs1["subject_no"].ToString() + "-" + splitsubj[3] + "-" + staff_code + "-" + subTypeRs1["subject_name"]));
                                                node.ChildNodes.Add(new TreeNode(subTypeRs1["subject_name"].ToString(), subTypeRs1["subject_no"].ToString() + "-" + staff_code + "-" + subTypeRs1["subject_name"]));
                                                rec_count = rec_count + 1;
                                            }
                                        }
                                        load_tree.Nodes.Add(node);
                                    }
                                }
                                load_tree.Nodes[0].Selected = true;
                                cona.Close();
                                con1a.Close();
                            }
                            load_tree.Visible = true;
                            okbtn.Visible = true;
                            //============================================
                            //  text_val = load_spread.Sheets[0].ColumnHeader.Cells[1, ac].Text;
                            //  Session["teaxtvalue"] = text_val;
                        }
                    }
                    else
                    {
                        //errlbl.Visible = true;                            
                        errlbl.Text = "Select Free Period";
                        errlbl.Visible = false;
                    }
                }
                else
                {
                    //errlbl.Visible = true;
                    errlbl.Text = "Select Period Column";
                    okbtn.Visible = false;
                }
                loadflag = false;
                //}
            }
        }
        catch
        {
        }
    }

    protected void load_spread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        loadflag = true;
    }

    private string GetSyllabusYear(string degree_code, string sem, string batch)
    {
        string syl_year = string.Empty;
        con2a.Close();
        con2a.Open();
        SqlCommand cmd2a;
        SqlDataReader get_syl_year;
        cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + degree_code.ToString() + " and semester =" + sem.ToString() + " and batch_year=" + batch.ToString() + " ", con2a);
        get_syl_year = cmd2a.ExecuteReader();
        get_syl_year.Read();
        if (get_syl_year.HasRows == true)
        {
            if (get_syl_year[0].ToString() == "\0")
            {
                syl_year = "-1";
            }
            else
            {
                syl_year = get_syl_year[0].ToString();
            }
        }
        else
        {
            syl_year = "-1";
        }
        return syl_year;
        con2a.Close();
    }

    protected void okbtn_Click(object sender, EventArgs e)
    {
        try
        {
            errlbl.Visible = false;
            string text_val = string.Empty;
            string strsec = string.Empty;
            string text = string.Empty;
            string node_val = string.Empty;
            load_tree.Nodes[0].Selected = true;
            int parent_count = load_tree.Nodes.Count;//----------count parent node value
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < load_tree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (load_tree.Nodes[i].ChildNodes[node_count].Checked == true)//-------check checked condition
                    {
                        node_val = load_tree.Nodes[i].ChildNodes[node_count].Value;
                        string[] split_val = node_val.Split(new Char[] { '-' });
                        text_val = split_val[0] + "-" + split_val[1] + "-S";
                        if (text == "")
                        {
                            text = text_val;
                        }
                        else
                        {
                            text = text + ";" + text_val;
                        }
                    }
                }
            }
            Session["teaxtvalue"] = text.ToString();
            if (text == "")
            {
                errlbl.Visible = true;
                errlbl.Text = "Select any subject from tree view";
                okbtn.Visible = true;
            }
            else
            {
                Session.Remove("date");
                Session.Remove("period");
                Response.Redirect("individualstaffalter.aspx");
            }
        }
        catch
        {
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        //Control cntUpdateBtn = classreport.FindControl("Update");
        //Control cntCancelBtn = classreport.FindControl("Cancel");
        //Control cntCopyBtn = classreport.FindControl("Copy");
        //Control cntCutBtn = classreport.FindControl("Clear");
        //Control cntPasteBtn = classreport.FindControl("Paste");
        Control cntPageNextBtn = load_spread.FindControl("Next");
        Control cntPagePreviousBtn = load_spread.FindControl("Prev");
        // Control cntPagePrintBtn = classreport.FindControl("Print");
        if ((cntPageNextBtn != null))
        {
            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);
            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    public void cal_date(double cumd)
    {
        int fyy = (12 / temp_month);
        int calm1 = fyy * 12 + 1;
        int calm2 = fyy * 12 + 2;
        int calm3 = fyy * 12 + 3;
        int calm4 = fyy * 12 + 4;
        int calm5 = fyy * 12 + 5;
        int calm6 = fyy * 12 + 6;
        int calm7 = fyy * 12 + 7;
        int calm8 = fyy * 12 + 8;
        int calm9 = fyy * 12 + 9;
        int calm10 = fyy * 12 + 10;
        int calm11 = fyy * 12 + 11;
        int calm12 = fyy * 12 + 12;
        if (calm1 == cumd || calm3 == cumd || calm5 == cumd || calm7 == cumd || calm8 == cumd || calm10 == cumd || calm12 == cumd)
        {
            daycount = 31;
        }
        if (calm4 == cumd || calm6 == cumd || calm9 == cumd || calm11 == cumd)
        {
            daycount = 30;
        }
        if (calm2 == cumd)
        {
            int lyear = 2000;
            int ly;
            if (lyear <= fyy)
            {
                ly = lyear - fyy;
            }
            else
            {
                ly = fyy - lyear;
            }
            if (ly == 4)
            {
                daycount = 29;
            }
            else
            {
                daycount = 28;
            }
        }
    }

    public void set_color()
    {
        if (!hatfeestaff.Contains(staff_code))
        {
            hatfeestaff.Add(staff_code, staff_code);
        }
        // if( load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text
        if (optradio.SelectedItem.ToString() == "Color")
        {
            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "" && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Note == "")//&& col_color_new!=1)
            {
                if (hat_att.ContainsValue("Busy"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.DarkMagenta;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.DarkMagenta;
                    col_color_new = 0;
                    if (hatfeestaff.Contains(staff_code))
                    {
                        hatfeestaff.Remove(staff_code);
                    }
                }
                else
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.White;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.White;
                    col_color_new = 0;
                }
            }
            else
            {
                if (attnd_val == "PER" && hat_att.ContainsValue("Per") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.Wheat;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Blue;
                    col_color_new = 1;
                }
                if (attnd_val == "LA" && hat_att.ContainsValue("LA") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightBlue;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Blue;
                    col_color_new = 1;
                }
                if (attnd_val == "OD" && hat_att.ContainsValue("OD") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.DarkGoldenrod;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.White;
                    col_color_new = 1;
                }
                if (attnd_val == "RL" && hat_att.ContainsValue("RL") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightPink;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Green;
                    col_color_new = 1;
                }
                if (attnd_val == "NA" && hat_att.ContainsValue("NA") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.MediumOrchid;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Orange;
                    col_color_new = 1;
                }
                if (hat_att.ContainsValue("Free") && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == string.Empty)
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightGray;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.LightGray;
                    col_color_new = 0;
                }
            }
        }
        else
        {
            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text == "" && load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Note != "")
            {
                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightGray;
                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.LightGray;
            }
            else
            {
                if (attnd_val == "P" && hat_att.ContainsValue("P"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.Olive;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Black;
                }
                else if (attnd_val == "A" && hat_att.ContainsValue("A"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.Red;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.White;
                }
                else if (attnd_val == "PER" && hat_att.ContainsValue("Per"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.Wheat;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Blue;
                }
                else if (attnd_val == "LA" && hat_att.ContainsValue("LA"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightBlue;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Blue;
                }
                else if (attnd_val == "OD" && hat_att.ContainsValue("OD"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.DarkGoldenrod;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.White;
                }
                else if (attnd_val == "RL" && hat_att.ContainsValue("RL"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightPink;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Green;
                }
                else if (attnd_val == "NA" && hat_att.ContainsValue("NA"))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.MediumOrchid;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Orange;
                }
                else if (hat_att.ContainsValue("NO") && (string.IsNullOrEmpty(attnd_val) || attnd_val == " "))
                {
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].BackColor = Color.LightSeaGreen;
                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].ForeColor = Color.Maroon;
                }
            }
            if (load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, increment_col_count].Text != "")
            {
                if (hatfeestaff.Contains(staff_code))
                {
                    hatfeestaff.Remove(staff_code);
                }
            }
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }

    protected void stafftxt_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        Panel3.Visible = false;
        load_tree.Visible = false;
        okbtn.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        string staff_code = string.Empty;
        if (stafftxt.SelectedItem.ToString() != "All")
        {
            if (stafftxt.Text != "")
            {
                if (stafftxt.SelectedItem.ToString() == "All")
                {
                    staff_code = string.Empty;
                }
                else
                {
                    staff_code = " and staff_code='" + stafftxt.SelectedValue.ToString() + "'";
                }
            }
            else
            {
                staff_code = string.Empty;
            }
            //con.Close();//================Modified by Venkat========================
            //con.Open();
            ////cmd = new SqlCommand("select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year " + staff_code + " and r.cc=0 and delflag=0 and exam_flag<>'Debar' order by st.batch_year,sy.degree_code,semester,st.sections ", con);
            ////cmd = new SqlCommand("select distinct s.subject_name+'-'+s.subject_code as subjname_code,s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year " + staff_code + " and r.cc=0 and delflag=0 and exam_flag<>'Debar' order by st.batch_year,sy.degree_code,semester ", con);
            //cmd = new SqlCommand("select distinct s.subject_code,s.subject_name from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year " + staff_code + " and r.cc=0 and delflag=0 and exam_flag<>'Debar'", con);
            //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            //DataSet ds1 = new DataSet();
            //da1.Fill(ds1);
            string str = "select distinct s.subject_code,s.subject_name from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year " + staff_code + " and r.cc=0 and delflag=0 and exam_flag<>'Debar'";
            ds1 = dac.select_method_wo_parameter(str, "Text");
            subjddl.DataSource = ds1.Tables[0];
            //subjddl.DataValueField = "subject_no";
            subjddl.DataTextField = "subject_name";
            subjddl.DataBind();
            subjddl.Items.Insert(0, "All");
        }
        //added by Manikandan 15/10/2013
        else
        {
            string dept_code = "", s_dept_code = "", r_dept_code = string.Empty;
            string staffdesig = string.Empty;//declared by Manikandan on 07/10/2013
            string get_degreecode = string.Empty;//declared by Manikandan on 11/10/2013
            if (deptddl.Text != "" && deptddl.Text != "All")
            {
                get_degreecode = GetFunction("select degree_code from degree where dept_code='" + deptddl.SelectedValue.ToString() + "'");
                if (deptddl.SelectedItem.ToString() != "All" && deptddl.SelectedItem.ToString() != "")
                {
                    //dept_code = " and de.degree_code=" + deptddl.SelectedValue.ToString();
                    //s_dept_code = " and syllabus_master.degree_code=" + deptddl.SelectedValue.ToString();
                    //r_dept_code = " and  registration.degree_code=" + deptddl.SelectedValue.ToString();
                    dept_code = " and de.dept_code=" + deptddl.SelectedValue.ToString();
                    if (get_degreecode != string.Empty)
                    {
                        s_dept_code = " and syllabus_master.degree_code=" + get_degreecode;
                        r_dept_code = " and  r.degree_code=" + get_degreecode;
                    }
                }
                else
                {
                    dept_code = string.Empty;
                }
            }
            else
            {
                dept_code = string.Empty;
            }
            if (desigddl.SelectedItem.ToString() != "All")
            {
                staffdesig = " and d.desig_name='" + desigddl.SelectedItem.ToString() + "'";
            }
            else
            {
                staffdesig = string.Empty;
            }
            subjddl.Items.Clear();
            con.Close();
            con.Open();
            //Modified By Srinath 7/3/2013 Adding distinct
            string strsubjec = "select distinct s.subject_name,s.subject_code from staff_selector st,subject s,sub_sem ss,syllabus_master sy,registration r,subjectchooser sc,usermaster u where st.staff_code=u.staff_code and st.subject_no=sc.subject_no and st.subject_no=s.subject_no and st.batch_year=r.Batch_Year and LTRIM(rtrim(isnull(st.Sections,'')))=LTRIM(rtrim(isnull(r.Sections,''))) and st.subject_no=sc.subject_no and st.batch_year=sy.Batch_Year and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sy.syll_code and s.subject_no=sc.subject_no and ss.syll_code=sy.syll_code and ss.subType_no=sc.subtype_no and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and sy.semester=sc.semester and r.Roll_No=sc.roll_no and r.Current_Semester=sc.semester and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + r_dept_code + "";
            //=======================Modified by Venkat==============================
            //cmd = new SqlCommand("select distinct subject_name,subject_code from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master,exam_type where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and  subject.syll_code=syllabus_master.syll_code " + s_dept_code + "  and syllabus_master.semester=registration.current_semester and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no " + r_dept_code + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no", con);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.SelectCommand.CommandTimeout = 120;
            DataSet ds = dac.select_method_wo_parameter(strsubjec, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                subjddl.DataSource = ds.Tables[0];
                subjddl.DataValueField = "subject_code";
                subjddl.DataTextField = "subject_name";
                subjddl.DataBind();
                subjddl.Items.Insert(0, "All");
            }
        }
        //===================End=======================
    }

    protected void subjddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
    }

    protected void frmperddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        diffperlbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
    }

    protected void toperddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        diffperlbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
    }

    protected void optradio_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errlbl.Visible = false;
        colorpnl.Visible = false;
        fmlbl.Visible = false;
        tolbl.Visible = false;
        chk_sms.Visible = false;
        chk_mail.Visible = false;
        txt_message.Visible = false;
        btnsms.Visible = false;
        if (optradio.SelectedIndex == 0)
        {
            attndddl.Items[0].Enabled = false;
            attndddl.Items[1].Enabled = false;
            attndddl.Items[2].Enabled = false;
            attndddl.Items[3].Enabled = false;
            attndddl.Items[4].Enabled = false;
            attndddl.Items[5].Enabled = false;
            attndddl.Items[6].Enabled = false;
            attndddl.Items[0].Selected = false;
            attndddl.Items[1].Selected = false;
            attndddl.Items[2].Selected = false;
            attndddl.Items[3].Selected = false;
            attndddl.Items[4].Selected = false;
            attndddl.Items[5].Selected = false;
            attndddl.Items[6].Selected = false;
            attndddl.Items[7].Enabled = true;
            attndddl.Items[8].Enabled = true;
            attndddl.Items[7].Selected = true;
            attndddl.Items[8].Selected = true;
        }
        else
        {
            attndddl.Items[0].Enabled = true;
            attndddl.Items[1].Enabled = true;
            attndddl.Items[2].Enabled = true;
            attndddl.Items[3].Enabled = true;
            attndddl.Items[4].Enabled = true;
            attndddl.Items[5].Enabled = true;
            attndddl.Items[6].Enabled = true;
            attndddl.Items[0].Selected = true;
            attndddl.Items[1].Selected = true;
            attndddl.Items[2].Selected = true;
            attndddl.Items[3].Selected = true;
            attndddl.Items[4].Selected = true;
            attndddl.Items[5].Selected = true;
            attndddl.Items[6].Selected = true;
            attndddl.Items[7].Enabled = false;
            attndddl.Items[8].Enabled = false;
            attndddl.Items[7].Selected = false;
            attndddl.Items[8].Selected = false;
        }
    }

    protected void attndddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //load_spread.Visible = false;
        //btnprintmaster.Visible = false;
        //errlbl.Visible = false;
        //colorpnl.Visible = false;
        //fmlbl.Visible = false;
        //tolbl.Visible = false;
        int count = 0;
        for (int cnt_value = 0; cnt_value < attndddl.Items.Count; cnt_value++)
        {
            if (attndddl.Items[cnt_value].Selected == true)
            {
                count++;
                txt_attndddl.Text = "Type(" + count.ToString() + ")";
            }
        }
        if (count == 0)
        {
            txt_attndddl.Text = "---Select---";
        }
    }

    protected void chk_attndddl_ChekedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (optradio.SelectedIndex != 0)
        {
            if (chk_attndddl.Checked == true)
            {
                for (int cnt_value = 0; cnt_value < attndddl.Items.Count - 2; cnt_value++)
                {
                    count++;
                    attndddl.Items[cnt_value].Selected = true;
                    txt_attndddl.Text = "Type(" + count.ToString() + ")";
                }
            }
            else
            {
                for (int cnt_value = 0; cnt_value < attndddl.Items.Count - 2; cnt_value++)
                {
                    count++;
                    attndddl.Items[cnt_value].Selected = false;
                    txt_attndddl.Text = "---Select---";
                }
            }
        }
        else
        {
            if (chk_attndddl.Checked == true)
            {
                for (int cnt_value = 7; cnt_value < attndddl.Items.Count; cnt_value++)
                {
                    count++;
                    attndddl.Items[cnt_value].Selected = true;
                    txt_attndddl.Text = "Type(" + count.ToString() + ")";
                }
            }
            else
            {
                for (int cnt_value = 7; cnt_value < attndddl.Items.Count; cnt_value++)
                {
                    count++;
                    attndddl.Items[cnt_value].Selected = false;
                    txt_attndddl.Text = "---Select---";
                }
            }
        }
    }

    protected void btngo_session_Click(object sender, EventArgs e)
    {
        try
        {
            btnclick_function();
        }
        catch
        {
        }
    }

    protected void load_tree_SelectedNodeChanged(object sender, EventArgs e)
    {
        load_tree.Nodes[0].Selected = true;
    }

    protected void load_tree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        load_tree.Nodes[0].Selected = true;
    }

    protected void load_tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
    {
        load_tree.Nodes[0].Selected = true;
    }

    public string findday(string curday, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = curday.Split(new Char[] { '/' });
            string cur_date = sp_date[1].ToString() + "-" + sp_date[0].ToString() + "-" + sp_date[2].ToString();
            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(cur_date);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + degree_code + " and semester=" + curr_sem + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";";
            string holday = GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            //-----------------------------------------------------------     
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;
        string deg_details = string.Empty;
        string date_pdf = string.Empty;
        string header = string.Empty;
        if (stafftxt.SelectedItem.ToString() != "All")
        {
            deg_details = " @Staff Name: " + stafftxt.SelectedItem.ToString();
        }
        string degreedetails = string.Empty;
        degreedetails = "Staff Workload Report" + deg_details + "@Date From: " + txtFromDate.Text.ToString() + " To: " + txtToDate.Text.ToString();
        string pagename = "workload.aspx";
        Printcontrol.loadspreaddetails(load_spread, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    public void staff_schedule_report()
    {
        string staffcode_1 = string.Empty;
        string subjectcode_1 = string.Empty;
        string code = string.Empty;
        string subjectname_1 = string.Empty;
        string semester_1 = string.Empty;
        string spread_sem = string.Empty;
        string batchyear_1 = string.Empty;
        string section_1 = string.Empty;
        string staff_name_1 = string.Empty;
        string degree_code_1 = string.Empty;
        string deptname = string.Empty;
        string staff_dept = string.Empty;
        int sno = 0;
        List<string> overload = new List<string>();
        load_spread.Sheets[0].ColumnHeader.RowCount = 0;
        load_spread.Sheets[0].RowCount = 0;
        load_spread.Sheets[0].ColumnCount = 0;
        load_spread.Sheets[0].ColumnHeader.RowCount = 2;
        load_spread.Sheets[0].ColumnCount = 6;
        load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
        load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
        load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
        load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Handling Subjects";
        load_spread.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Batch Year/ Branch / Sem / Sec";
        load_spread.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Subjects";
        load_spread.Sheets[0].Columns[0].Width = 50;
        load_spread.Sheets[0].Columns[2].Width = 150;
        load_spread.Sheets[0].Columns[4].Width = 300;
        load_spread.Sheets[0].Columns[5].Width = 250;
        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
        load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        load_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
        load_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
        load_spread.Sheets[0].RowHeader.Visible = false;
        //FarPoint.Web.Spread.SheetSkin myskin = new FarPoint.Web.Spread.SheetSkin("MySkin", Color.BlanchedAlmond, Color.Bisque, Color.Navy, 2, Color.Blue, GridLines.Both,Color.Blue, Color.Black, Color.AntiqueWhite, Color.Brown, Color.Bisque, Color.Bisque, true, true, true, true, false);
        //myskin.Apply(load_spread.Sheets[0]);
        //FarPoint.Web.Spread.DefaultSkins.Professional3.Apply(load_spread.Sheets[0]);
        string staffname = string.Empty;
        //con.Close();//==================Modified by Venkat=======================
        //con.Open();
        //SqlCommand cmd_tot_hrs = new SqlCommand("select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half'  from PeriodAttndSchedule", con);
        //string totalhrs = Convert.ToString(cmd_tot_hrs.ExecuteScalar());
        string cmd_tot_hrs = "select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half'  from PeriodAttndSchedule";
        ds = dac.select_method_wo_parameter(cmd_tot_hrs, "Text");
        string totalhrs = string.Empty;// ds.Tables[0].Rows[0]["Total Hours"].ToString();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            totalhrs = ds.Tables[0].Rows[0]["Total Hours"].ToString();
        }
        if (string.IsNullOrEmpty(totalhrs))
        {
            totalhrs = "0";
        }
        for (int getsem = 0; getsem < ddlsem.Items.Count; getsem++)
        {
            if (ddlsem.Items[getsem].Selected == true)
            {
                if (semester_1 == string.Empty)
                {
                    semester_1 = ddlsem.Items[getsem].Text.ToString();
                }
                else
                {
                    semester_1 = semester_1 + "','" + ddlsem.Items[getsem].Text.ToString();
                }
            }
        }
        if (stafftxt.Items.Count > 0)
        {
            if (stafftxt.SelectedItem.ToString() != "All")
            {
                staffname = stafftxt.SelectedValue.ToString();
                if (desigddl.Items.Count > 0)
                {
                    if (desigddl.SelectedValue == "All" || desigddl.SelectedValue == "")
                    {
                        strdesig = string.Empty;
                    }
                    else
                    {
                        strdesig = " and desig_name='" + desigddl.SelectedItem.ToString() + "'";
                    }
                }
                //--------------department ALL
                if (deptddl.Items.Count > 0)
                {
                    if (deptddl.SelectedValue == "All" || deptddl.SelectedValue == "")
                    {
                        strdept = string.Empty;
                    }
                    else
                    {
                        strdept = " and h.dept_code='" + deptddl.SelectedValue.ToString() + "'";
                        //strdept = " and h.dept_code='" + GetFunction("select dept_code from  degree where degree_code='" + deptddl.SelectedValue.ToString() + "'") + "'";
                    }
                }
                //--------------staff name
                string only_staff_code = string.Empty;
                strstaff = " and m.staff_code='" + stafftxt.SelectedValue.ToString() + "'";
                only_staff_code = " and ss.staff_code<>'" + stafftxt.SelectedValue.ToString() + "'";
                pnl_filter.Visible = true;
                has.Clear();
                has.Add("@coll_code", ddlcollege.SelectedValue.ToString());
                has.Add("@subjno ", " ");
                has.Add("@strdesig  ", strdesig);
                has.Add("@strdept", strdept);
                has.Add("@strstaff ", @strstaff);
                ds_staff = dac.select_method("workload_getstafflist", has, "sp");
                if (ds_staff.Tables.Count > 0 && ds_staff.Tables[0].Rows.Count > 0)
                {
                    for (int temp_staff = 0; temp_staff < ds_staff.Tables[0].Rows.Count; temp_staff++)
                    {
                        staff_code = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                        staff_dept = ds_staff.Tables[0].Rows[temp_staff]["dept_name"].ToString();
                        staff_name_1 = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                        sno++;
                        //SqlFinal = "select distinct s.subject_name,r.degree_code,r.batch_year,r.current_semester,r.sections from staff_selector sl,syllabus_master sy,subject s,registration r where sl.subject_no=s.subject_no and sy.syll_code=s.syll_code and r.batch_year=sy.batch_year and r.degree_code=sy.degree_code and r.current_semester=sy.semester and sl.batch_year=r.batch_year and staff_code ='" + staff_code + "' and r.current_semester in('" + semester_1 + "') and cc=0 and delflag=0 and exam_flag<>'debar' order by r.degree_code,r.batch_year";
                        SqlFinal = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and LTRIM(rtrim(isnull(st.sections,'')))=LTRIM(rtrim(isnull(r.sections,''))) and staff_code='" + staff_code + "' and semester in('" + semester_1 + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar'  order by st.batch_year,sy.degree_code,semester,st.sections";
                        DataSet ds_staff_schedule = new DataSet();
                        // ds_staff_schedule = dac.select_method(SqlFinal, hat, "Text");============Modified By Venkat=====================
                        ds_staff_schedule = dac.select_method_wo_parameter(SqlFinal, "Text");
                        if (ds_staff_schedule.Tables.Count > 0 && ds_staff_schedule.Tables[0].Rows.Count > 0)
                        {
                            for (int row_cnt = 0; row_cnt < ds_staff_schedule.Tables[0].Rows.Count; row_cnt++)
                            {
                                batchyear_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["batch_year"].ToString();
                                semester_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["semester"].ToString();
                                degree_code_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["degree_code"].ToString();
                                subjectname_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_name"].ToString();
                                //staffcode_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["staff_code"].ToString();
                                if (ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != string.Empty && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != "-1" && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != null)
                                {
                                    section_1 = "-Sec:" + ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString();
                                }
                                else
                                {
                                    section_1 = string.Empty;
                                }
                                //subjectname_1 = GetFunction("select subject_name from subject where subject_no='" + subjectcode_1 + "'");
                                //staff_name_1 = GetFunction("select staff_name from staffmaster where staff_code='" + staffcode_1 + "'");
                                deptname = GetFunction("select Dept_Name from department where Dept_Code=(select dept_code from degree where degree_code='" + degree_code_1 + "')");
                                load_spread.Sheets[0].RowCount++;
                                //if (load_spread.Sheets[0].RowCount % 2 == 0)
                                //{
                                //    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].BackColor = Color.Azure;
                                //}
                                //else
                                //{
                                //    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].BackColor = Color.Beige;
                                //}
                                //sno++;
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Text = sno.ToString();
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 1].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 2].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                                //load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Note = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Text = staff_dept.ToString();
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 4].Text = batchyear_1 + " " + deptname + "-Sem:" + semester_1 + section_1;
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 5].Text = subjectname_1;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int allstaff = 1; allstaff < stafftxt.Items.Count; allstaff++)
                {
                    staffname = stafftxt.Items[allstaff].Value;
                    if (desigddl.Items.Count > 0)
                    {
                        if (desigddl.SelectedValue == "All" || desigddl.SelectedValue == "")
                        {
                            strdesig = string.Empty;
                        }
                        else
                        {
                            strdesig = " and desig_name='" + desigddl.SelectedItem.ToString() + "'";
                        }
                    }
                    //--------------department ALL
                    if (deptddl.Items.Count > 0)
                    {
                        if (deptddl.SelectedValue == "All" || deptddl.SelectedValue == "")
                        {
                            strdept = string.Empty;
                        }
                        else
                        {
                            strdept = " and h.dept_code='" + deptddl.SelectedValue.ToString() + "'";
                            //strdept = " and h.dept_code='" + GetFunction("select dept_code from  degree where degree_code='" + deptddl.SelectedValue.ToString() + "'") + "'";
                        }
                    }
                    //--------------staff name
                    string only_staff_code = string.Empty;
                    if (staffname != string.Empty)
                    {
                        strstaff = " and m.staff_code='" + staffname + "'";
                        only_staff_code = " and ss.staff_code<>'" + staffname + "'";
                        //strstaff = " and m.staff_code<>'" + staffname + "'";
                        //only_staff_code = " and ss.staff_code<>'" + staffname + "'";
                    }
                    pnl_filter.Visible = true;
                    has.Clear();
                    has.Add("@coll_code", ddlcollege.SelectedValue.ToString());
                    has.Add("@subjno ", " ");
                    has.Add("@strdesig  ", strdesig);
                    has.Add("@strdept", strdept);
                    has.Add("@strstaff ", @strstaff);
                    ds_staff = dac.select_method("workload_getstafflist", has, "sp");
                    if (ds_staff.Tables.Count > 0 && ds_staff.Tables[0].Rows.Count > 0)
                    {
                        for (int temp_staff = 0; temp_staff < ds_staff.Tables[0].Rows.Count; temp_staff++)
                        {
                            staff_code = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                            staff_dept = ds_staff.Tables[0].Rows[temp_staff]["dept_name"].ToString();
                            staff_name_1 = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                            //sno++;
                            //SqlFinal = "select distinct s.subject_name,r.degree_code,r.batch_year,r.current_semester,r.sections from staff_selector sl,syllabus_master sy,subject s,registration r where sl.subject_no=s.subject_no and sy.syll_code=s.syll_code and r.batch_year=sy.batch_year and r.degree_code=sy.degree_code and r.current_semester=sy.semester and sl.batch_year=r.batch_year and staff_code ='" + staff_code + "' and r.current_semester in('" + semester_1 + "') and cc=0 and delflag=0 and exam_flag<>'debar' order by r.degree_code,r.batch_year";
                            SqlFinal = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and staff_code='" + staff_code + "' and semester in('" + semester_1 + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar'  order by st.batch_year,sy.degree_code,semester,st.sections";//and st.sections=r.sections
                            DataSet ds_staff_schedule = new DataSet();
                            // ds_staff_schedule = dac.select_method(SqlFinal, hat, "Text");============Modified by Venkat================
                            ds_staff_schedule = dac.select_method_wo_parameter(SqlFinal, "Text");
                            if (ds_staff_schedule.Tables.Count > 0 && ds_staff_schedule.Tables[0].Rows.Count > 0)
                            {
                                sno++;
                                for (int row_cnt = 0; row_cnt < ds_staff_schedule.Tables[0].Rows.Count; row_cnt++)
                                {
                                    batchyear_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["batch_year"].ToString();
                                    spread_sem = ds_staff_schedule.Tables[0].Rows[row_cnt]["semester"].ToString();
                                    degree_code_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["degree_code"].ToString();
                                    subjectname_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_name"].ToString();
                                    //staffcode_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["staff_code"].ToString();
                                    if (ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != string.Empty && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != "-1" && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != null)
                                    {
                                        section_1 = "-Sec:" + ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString();
                                    }
                                    else
                                    {
                                        section_1 = string.Empty;
                                    }
                                    //subjectname_1 = GetFunction("select subject_name from subject where subject_no='" + subjectcode_1 + "'");
                                    //staff_name_1 = GetFunction("select staff_name from staffmaster where staff_code='" + staffcode_1 + "'");
                                    deptname = GetFunction("select Dept_Name from department where Dept_Code=(select dept_code from degree where degree_code='" + degree_code_1 + "')");
                                    load_spread.Sheets[0].RowCount++;
                                    //if (load_spread.Sheets[0].RowCount % 2 == 0)
                                    //{
                                    //    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].BackColor = Color.Azure;
                                    //}
                                    //else
                                    //{
                                    //    load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].BackColor = Color.Beige;
                                    //}
                                    //sno++;
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Text = sno.ToString();
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 1].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 2].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                                    //load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Note = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Text = staff_dept.ToString();
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 4].Text = batchyear_1 + " " + deptname + "-Sem:" + spread_sem + section_1;
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 5].Text = subjectname_1;
                                }
                            }
                        }
                    }
                }
            }
        }
        if (load_spread.Sheets[0].RowCount > 0)
        {
            chk_sms.Visible = false;
            chk_mail.Visible = false;
            txt_message.Visible = false;
            btnsms.Visible = false;
            load_spread.Height = 500;
            //if (load_spread.Sheets[0].RowCount > 10)
            //{
            //    load_spread.Height = load_spread.Sheets[0].RowCount * 20;
            //}
            //else
            //{
            //    load_spread.Height = load_spread.Sheets[0].RowCount * 60;
            //}
            load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
            load_spread.Width = 946;
            load_spread.Visible = true;
            btnprintmaster.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
        }
        else
        {
            chk_sms.Visible = false;
            chk_mail.Visible = false;
            txt_message.Visible = false;
            btnsms.Visible = false;
            load_spread.Visible = false;
            errlbl.Text = "No Records are found";
            errlbl.Visible = true;
        }
    }

    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        ddlbatch.Enabled = false;
        ddldegree.Enabled = false;
        ddlbranch.Enabled = false;
        chkfreestaff.Checked = false;
        chkfreestaff.Visible = false;
        if (ddlreporttype.SelectedIndex == 0)
        {
            frmperddl.Enabled = true;
            toperddl.Enabled = true;
            optradio.Enabled = true;
            txt_attndddl.Enabled = true;
            subjddl.Enabled = true;
            txt_sem.Enabled = false;
            chkfreestaff.Visible = true;
        }
        else if (ddlreporttype.SelectedIndex == 1)
        {
            frmperddl.Enabled = false;
            toperddl.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            optradio.Enabled = false;
            txt_attndddl.Enabled = false;
            subjddl.Enabled = false;
            txt_sem.Enabled = true;
        }
        else if (ddlreporttype.SelectedIndex == 2)
        {
            frmperddl.Enabled = false;
            toperddl.Enabled = false;
            optradio.Enabled = false;
            txt_attndddl.Enabled = false;
            subjddl.Enabled = true;
            for (int i = 0; i < ddlsem.Items.Count; i++)
            {
                ddlsem.Items[i].Selected = true;
            }
            txt_sem.Enabled = false;
        }
        else if (ddlreporttype.SelectedIndex == 3)
        {
            ddlbatch.Enabled = true;
            ddldegree.Enabled = true;
            ddlbranch.Enabled = true;
            BindBatch();
            BindDegree();
            bindbranch();
            deptddl.Enabled = false;
            desigddl.Enabled = false;
            stafftxt.Enabled = false;
            subjddl.Enabled = false;
        }
        else if (ddlreporttype.SelectedIndex == 5)
        {
            //BindBatch();
            //BindDegree();
            //bindbranch();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else if (ddlreporttype.SelectedIndex == 4)
        {
            BindBatch();
            BindDegree();
            bindbranch();
            deptddl.Enabled = true;
            desigddl.Enabled = true;
            stafftxt.Enabled = true;
            subjddl.Enabled = true;
            frmperddl.Enabled = false;
            toperddl.Enabled = false;
            optradio.Enabled = false;
            txt_attndddl.Enabled = false;
            subjddl.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else
        {
            BindBatch();
            BindDegree();
            bindbranch();
            deptddl.Enabled = true;
            desigddl.Enabled = true;
            stafftxt.Enabled = true;
            subjddl.Enabled = true;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            frmperddl.Enabled = false;
            toperddl.Enabled = false;
            optradio.Enabled = false;
            txt_attndddl.Enabled = false;
            subjddl.Enabled = false;
        }
    }

    protected void load_spread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(load_spread.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    load_spread.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

    //Modified By Srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SCHCLG")
    //        {
    //            SenderID = "SCHCLG";
    //            Password = "SCHCLG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "SRECTD")
    //        {
    //            SenderID = "SRECTD";
    //            Password = "SRECTD";
    //        }
    //        else if (user_id == "EICTPC")
    //        {
    //            SenderID = "EICTPC";
    //            Password = "EICTPC";
    //        }
    //        else if (user_id == "SHACLG")
    //        {
    //            SenderID = "SHACLG";
    //            Password = "SHACLG";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "TECAAA")
    //        {
    //            SenderID = "TECAAA";
    //            Password = "TECAAA";
    //        }
    //        else if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVISTE")
    //        {
    //            SenderID = "SVISTE";
    //            Password = "SVISTE";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //         else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //         else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }    
    //       else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }    
    //      else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        } 
    //     else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        } 
    //   else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }
    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            SenderID = "AUDIIT";
    //            Password = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            SenderID = "SAENGG";
    //            Password = "SAENGG";
    //        }
    //        else if (user_id == "STANE")
    //        {
    //            SenderID = "STANES";
    //            Password = "STANES";
    //        }
    //        else if (user_id == "MBCBSE")
    //        {
    //            SenderID = "MBCBSE";
    //            Password = "MBCBSE";
    //        }
    //        else if (user_id == "HIETPT")
    //        {
    //            SenderID = "HIETPT";
    //            Password = "HIETPT";
    //        }
    //        else if (user_id == "SVPITM")
    //        {
    //            SenderID = "SVPITM";
    //            Password = "SVPITM";
    //        }
    //        else if (user_id == "AUDCET")
    //        {
    //            SenderID = "AUDCET";
    //            Password = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            SenderID = "AUDWOM";
    //            Password = "AUDWOM";
    //        }
    //        else if (user_id == "AUDIPG")
    //        {
    //            SenderID = "AUDIPG";
    //            Password = "AUDIPG";
    //        }
    //        else if (user_id == "MCCDAY")
    //        {
    //            SenderID = "MCCDAY";
    //            Password = "MCCDAY";
    //        }
    //        else if (user_id == "MCCSFS")
    //        {
    //            SenderID = "MCCSFS";
    //            Password = "MCCSFS";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public void smsreport(string uril, string isstaff)
    {
        string date = DateTime.Now.ToString("MM/dd/yyyy");
        WebRequest request = WebRequest.Create(uril);
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        StreamReader sr = new StreamReader(data);
        string strvel = sr.ReadToEnd();
        string groupmsgid = string.Empty;
        groupmsgid = strvel;
        int sms = 0;
        string smsreportinsert = string.Empty;
        string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
        for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
        {
            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + strmsg + "','" + ddlcollege.SelectedValue.ToString() + "','" + isstaff + "','" + System.DateTime.Now + "','" + Session["UserCode"].ToString() + "')";
            sms = d2.insert_method(smsreportinsert, hat, "Text");
        }
        if (sms == 1)
        {
            errmsg.Text = "Detail's added Succefully";
            errmsg.Visible = true;
            flagstudent = true;
        }
        else
        {
            errmsg.Text = "Detail's added failed";
            errmsg.Visible = true;
        }
        if (verify == 1)
        {
            if (chk_sms.Checked == true && chk_mail.Checked == false)
            {
                if (flagstudent == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Message has been sent for Selected Staff')", true);
                }
            }
            if (chk_mail.Checked == true && chk_sms.Checked == false)
            {
                if (flagstudent == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Mail has been sent for Selected Staff ')", true);
                }
            }
            if (chk_mail.Checked == true && chk_sms.Checked == true)
            {
                if (flagstudent == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Message/Mail has been sent for Selected Staff')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Atleast One Staff')", true);
        }
    }

    protected void btnsms_Click(object sender, EventArgs e)
    {
        if (chk_sms.Checked == false && chk_mail.Checked == false)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any One Option SMS or MAIL ')", true);
            return;
        }
        try
        {
            if (chk_sms.Checked == true)
            {
                string strsmsuserid = string.Empty;
                string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
                ds1.Dispose();
                ds1.Reset();
                // ds1 = d2.select_method(strsenderquery, hat, "Text");==============Modified by Venkat======================
                ds1 = d2.select_method_wo_parameter(strsenderquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                //modified by srinath 1/8/2014
                //GetUserapi(user_id);
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                strmsg = txt_message.Text;
                con_mobno.Close();
                con_mobno.Open();
                for (int i = 1; i < load_spread.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(load_spread.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {
                        SqlCommand cmd_mobno = new SqlCommand("select per_mobileno from staff_appl_master where appl_no=(select appl_no from staffmaster where staff_code='" + load_spread.Sheets[0].Cells[i, 0].Note + "')", con_mobno);
                        strmobileno = Convert.ToString(cmd_mobno.ExecuteScalar());
                        //strmobileno = Convert.ToString(load_spread.Sheets[0].Cells[i, 4].Note);
                        if (strmobileno != "Nil" && strmobileno != "")
                        {
                            if (mobilenos == "")
                            {
                                mobilenos = strmobileno;
                            }
                            else
                            {
                                mobilenos = mobilenos + "," + strmobileno;
                            }
                        }
                        verify = 1;
                    }
                }
                //Modified By Srinath 8/2/2014
                //string strpath1 = "http://alerts.sinfini.com/api/web2sms.php?workingkey=" + strsenderid + " &sender=" + struserapi + "&to=" + mobilenos + "  &message=" + strmsg;
                //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                ////string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobilenos + "&message=" + strmsg + "&sender=" + SenderID;
                //// System.Diagnostics.Process.Start(strpath1);
                //string isstf = "1";
                //smsreport(strpath1, isstf);
                int noofsms = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), Session["usercode"].ToString(), mobilenos, strmsg, "1");
            }
            if (chk_mail.Checked == true)
            {
                strmsg = txt_message.Text;
                string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                ds1.Dispose();
                ds1.Reset();
                // ds1 = d2.select_method(strquery, hat, "Text");=========Modified by Venkat=====================
                ds1 = d2.select_method_wo_parameter(strquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                }
                con_mobno.Close();
                con_mobno.Open();
                for (int i = 1; i < load_spread.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(load_spread.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {
                        SqlCommand cmd_mobno = new SqlCommand("select email from staff_appl_master where appl_no=(select appl_no from staffmaster where staff_code='" + load_spread.Sheets[0].Cells[i, 0].Note + "')", con_mobno);
                        to_mail = Convert.ToString(cmd_mobno.ExecuteScalar());
                        strstuname = Convert.ToString(load_spread.Sheets[0].Cells[i, 4].Text);
                        //to_mail = Convert.ToString(load_spread.Sheets[0].Cells[i, 5].Note);
                        // to_mail = "karthikeyanmurugesan08@gmail.com";
                        SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                        MailMessage mailmsg = new MailMessage();
                        MailAddress mfrom = new MailAddress(send_mail);
                        mailmsg.From = mfrom;
                        mailmsg.To.Add(to_mail);
                        mailmsg.Subject = "Report";
                        mailmsg.IsBodyHtml = true;
                        mailmsg.Body = "Hi  ";
                        mailmsg.Body = mailmsg.Body + strstuname;
                        mailmsg.Body = mailmsg.Body + strmsg;
                        mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                        Mail.EnableSsl = true;
                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                        Mail.UseDefaultCredentials = false;
                        Mail.Credentials = credentials;
                        Mail.Send(mailmsg);
                        verify = 1;
                        flagstudent = true;
                    }
                }
                if (verify == 1)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Mail has been sent successfully for selected staff')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select atleast one staff')", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void chk_sem_ChekedChanged(object sender, EventArgs e)
    {
        //ph.Controls.Add(psem);
        //psem.CssClass = "newstyle";
        int count = 0;
        if (chk_sem.Checked == true)
        {
            for (int chklst_sem = 0; chklst_sem < ddlsem.Items.Count; chklst_sem++)
            {
                count++;
                ddlsem.Items[chklst_sem].Selected = true;
                txt_sem.Text = "Sem(" + count.ToString() + ")";
            }
        }
        else
        {
            for (int chklst_sem = 0; chklst_sem < ddlsem.Items.Count; chklst_sem++)
            {
                ddlsem.Items[chklst_sem].Selected = false;
            }
            txt_sem.Text = "--select--";
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int chklst_sem = 0; chklst_sem < ddlsem.Items.Count; chklst_sem++)
        {
            if (ddlsem.Items[chklst_sem].Selected == true)
            {
                count++;
                txt_sem.Text = "Sem(" + count.ToString() + ")";
            }
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddesig();
        binddept();
        bindsubject();
        bindstaff();
    }

    public void individual_workload()
    {
        try
        {
            load_spread.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            Hashtable alltotalvalues = new Hashtable(); // added sridhar  for total issue
            string staffcode_1 = string.Empty;
            string subjectcode_1 = string.Empty;
            string code = string.Empty;
            string subjectname_1 = string.Empty;
            string semester_1 = string.Empty;
            string semester_2 = string.Empty;
            string batchyear_1 = string.Empty;
            string section_1 = string.Empty;
            string desig_name = string.Empty;
            string staff_name_1 = string.Empty;
            string degree_code_1 = string.Empty;
            string deptname = string.Empty;
            string staff_dept = string.Empty;
            string staff_cat_code = string.Empty;
            string subcode = string.Empty;
            string stfcode = string.Empty;
            string theory_prac = string.Empty;
            int sno = 0;
            List<string> day_name = new List<string>();
            List<string> overload = new List<string>();
            DataView dv_alternate = new DataView();
            Boolean recflag = false;
            load_spread.Sheets[0].ColumnHeader.RowCount = 0;
            load_spread.Sheets[0].RowCount = 0;
            load_spread.Sheets[0].ColumnCount = 0;
            load_spread.Sheets[0].ColumnHeader.RowCount = 2;
            load_spread.Sheets[0].ColumnCount = 10;
            load_spread.Sheets[0].FrozenColumnCount = 5;
            load_spread.Sheets[0].RowHeader.Visible = false;
            load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year/ Branch / Sem / Sec";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Handling Subjects";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Papers";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Theory";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Practical";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total";
            load_spread.Sheets[0].Columns[0].Locked = true;
            load_spread.Sheets[0].Columns[1].Locked = true;
            load_spread.Sheets[0].Columns[2].Locked = true;
            load_spread.Sheets[0].Columns[3].Locked = true;
            load_spread.Sheets[0].Columns[4].Locked = true;
            load_spread.Sheets[0].Columns[5].Locked = true;
            load_spread.Sheets[0].Columns[6].Locked = true;
            load_spread.Sheets[0].Columns[7].Locked = true;
            load_spread.Sheets[0].Columns[8].Locked = true;
            load_spread.Sheets[0].Columns[9].Locked = true;
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, 6, 1, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
            load_spread.Sheets[0].Columns[0].Width = 30;
            load_spread.Sheets[0].Columns[1].Width = 70;
            load_spread.Sheets[0].Columns[2].Width = 150;
            load_spread.Sheets[0].Columns[3].Width = 100;
            load_spread.Sheets[0].Columns[4].Width = 150;
            load_spread.Sheets[0].Columns[5].Width = 200;
            load_spread.Sheets[0].Columns[6].Width = 200;
            load_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].RowHeader.Visible = false;
            string[] split_fromdate = txtFromDate.Text.Split(new char[] { '/' });
            string[] split_todate = txtToDate.Text.Split(new char[] { '/' });
            DateTime dt_fromdate = Convert.ToDateTime(split_fromdate[1] + "/" + split_fromdate[0] + "/" + split_fromdate[2]);
            DateTime dt_todate = Convert.ToDateTime(split_todate[1] + "/" + split_todate[0] + "/" + split_todate[2]);
            dt2 = dt_todate;
            dt1 = dt_fromdate;
            TimeSpan t = dt2.Subtract(dt1);
            days = t.Days;
            if (dt_fromdate > dt_todate)
            {
                chk_sms.Visible = false;
                chk_mail.Visible = false;
                txt_message.Visible = false;
                btnsms.Visible = false;
                load_spread.Visible = false;
                errlbl.Text = "From  Date Must Be Lesser Than or Equal to To Date";
                errlbl.Visible = true;
                return;
            }
            DataSet dsgetvalue = new DataSet();
            string sql1 = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();
            int noofhrs = 0;
            string vari = string.Empty;
            Hashtable ht_sch = new Hashtable();
            DataSet ds_attndmaster = new DataSet();
            string degree_var = string.Empty;
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = dac.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            Hashtable ht_sdate = new Hashtable();
            ht_sdate.Clear();
            if (ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            string degreename = string.Empty;
            Hashtable hatdegreename = new Hashtable();
            for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
            {
                if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                {
                    hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                }
            }
            string staffname = string.Empty;
            string semesters = string.Empty;
            for (int getsem = 0; getsem < ddlsem.Items.Count; getsem++)
            {
                if (ddlsem.Items[getsem].Selected == true)
                {
                    if (semesters == string.Empty)
                    {
                        semesters = ddlsem.Items[getsem].Text.ToString();
                    }
                    else
                    {
                        semesters = semesters + "','" + ddlsem.Items[getsem].Text.ToString();
                    }
                }
            }
            if (stafftxt.Items.Count > 0)
            {
                int loopcount = 1;
                int startcnt = 0;
                if (stafftxt.SelectedItem.ToString() != "All")
                {
                    loopcount = 1;
                    startcnt = 0;
                }
                else
                {
                    loopcount = stafftxt.Items.Count;
                    startcnt = 1;
                }
                Hashtable combinestaff = new Hashtable();
                for (int selstaff = startcnt; selstaff < loopcount; selstaff++)
                {
                    combinestaff.Clear();
                    if (stafftxt.SelectedItem.ToString() != "All")
                    {
                        staffcode_selected = stafftxt.SelectedValue.ToString();
                    }
                    else
                    {
                        staffcode_selected = stafftxt.Items[selstaff].Value.ToString();
                    }
                    staffname = staffcode_selected.ToString();
                    if (desigddl.SelectedValue == "All" || desigddl.SelectedValue == "")
                    {
                        strdesig = string.Empty;
                    }
                    else
                    {
                        strdesig = " and desig_name='" + desigddl.SelectedItem.ToString() + "'";
                    }
                    if (deptddl.SelectedValue == "All" || deptddl.SelectedValue == "")
                    {
                        strdept = string.Empty;
                    }
                    else
                    {
                        strdept = " and h.dept_code='" + deptddl.SelectedValue.ToString() + "'";
                    }
                    string only_staff_code = string.Empty;
                    strstaff = " and m.staff_code='" + staffcode_selected.ToString() + "'";
                    only_staff_code = " and ss.staff_code<>'" + staffcode_selected.ToString() + "'";
                    pnl_filter.Visible = true;
                    has.Clear();
                    has.Add("@coll_code", ddlcollege.SelectedValue.ToString());
                    has.Add("@subjno ", " ");
                    has.Add("@strdesig  ", strdesig);
                    has.Add("@strdept", strdept);
                    has.Add("@strstaff ", @strstaff);
                    ds_staff = dac.select_method("workload_getstafflist", has, "sp");
                    for (int temp_staff = 0; temp_staff < ds_staff.Tables[0].Rows.Count; temp_staff++)
                    {
                        staff_code = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                        staff_dept = ds_staff.Tables[0].Rows[temp_staff]["dept_name"].ToString();
                        desig_name = ds_staff.Tables[0].Rows[temp_staff]["desig_name"].ToString();
                        staff_name_1 = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                        staff_cat_code = ds_staff.Tables[0].Rows[temp_staff]["category_code"].ToString();
                        SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                        SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                        SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                        SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and LTRIM(rtrim(isnull(r.sections,'')))=LTRIM(rtrim(isnull(ss.sections,''))) and ss.batch_year=r.Batch_Year";
                        SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                        SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                        SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "'";
                        DataView dvalternaet = new DataView();
                        DataView dvsemster = new DataView();
                        DataView dvholiday = new DataView();
                        DataView dvdaily = new DataView();
                        DataView dvsubject = new DataView();
                        DataView dvsublab = new DataView();
                        Hashtable hatstaffhours = new Hashtable();
                        string getalldetails = "select * from Alternate_Schedule where FromDate between '" + dt_fromdate.ToString("MM/dd/yyyy") + "' and '" + dt_todate.ToString("MM/dd/yyyy") + "' ; ";
                        getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
                        getalldetails = getalldetails + "Select * from holidaystudents where holiday_date between '" + dt_fromdate.ToString("MM/dd/yyyy") + "' and '" + dt_todate.ToString("MM/dd/yyyy") + "' ; ";
                        getalldetails = getalldetails + "select * from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code and ds.sch_date between '" + dt_fromdate.ToString("MM/dd/yyyy") + "' and '" + dt_todate.ToString("MM/dd/yyyy") + "'  ; ";
                        getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s,staff_selector ss where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and ss.subject_no=s.subject_no and ss.batch_year=sy.Batch_Year and ss.staff_code='" + staff_code + "' order by sy.Batch_Year,sy.degree_code,sy.semester ;";
                        getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
                        getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,No_of_hrs_per_day,degree_code,semester from periodattndschedule";
                        DataSet dsall = dac.select_method_wo_parameter(getalldetails, "Text");
                        Hashtable hatcombinedclass = new Hashtable();
                        Boolean combainedflag = false;
                        string strstaffselector = " and s.staffcode='" + staff_code + "'";
                        string cur_camprevar = string.Empty;
                        string tmp_camprevar = string.Empty;
                        string strsction = string.Empty;
                        Hashtable hatholiday = new Hashtable();
                        DataSet dsperiod = dac.select_method(SqlFinal, hat, "Text");
                        int stafftotalhours = 0;
                        int stafftotalrowspan = 0;
                        int noofhourscombine = 0;
                        Boolean srnoflag = false;
                        if (dsperiod.Tables[0].Rows.Count > 0)
                        {
                            for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                            {
                                cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                string getdate = string.Empty;
                                string getsection = string.Empty;
                                if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                {
                                    Boolean staffhour = false;
                                    strsction = string.Empty;
                                    if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                    {
                                        strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                        getsection = " Sec - " + dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                    }
                                    hatstaffhours.Clear();
                                    dsall.Tables[4].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                    DataView dtcurlab = dsall.Tables[4].DefaultView;
                                    Hashtable hatcurlab = new Hashtable();
                                    for (int cula = 0; cula < dtcurlab.Count; cula++)
                                    {
                                        string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                        string labhour = dtcurlab[cula]["lab"].ToString();
                                        if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                        {
                                            if (!hatcurlab.Contains(lasubno))
                                            {
                                                hatcurlab.Add(lasubno, lasubno);
                                            }
                                        }
                                    }
                                    string strsubstucount = " select count(distinct r.Roll_No) as stucount,r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date from registration r,subjectchooser s where  r.roll_no=s.roll_no and  r.current_semester=s.semester";
                                    strsubstucount = strsubstucount + " and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and  degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and current_semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'  and cc=0 and delflag=0 and exam_flag<>'debar' " + strsction + " " + strstaffselector + "  group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_no,r.adm_date";
                                    DataSet dssubstucount = dac.select_method_wo_parameter(strsubstucount, "Text");
                                    DataView dvsubstucount = new DataView();
                                    hatholiday.Clear();
                                    dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                    DataView duholiday = dsall.Tables[2].DefaultView;
                                    for (int i = 0; i < duholiday.Count; i++)
                                    {
                                        if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                        {
                                            hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                        }
                                    }
                                    int frshlf = 0, schlf = 0;
                                    dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                    DataView dvperiod = dsall.Tables[6].DefaultView;
                                    if (dvperiod.Count > 0)
                                    {
                                        string morhr = dvperiod[0]["mor"].ToString();
                                        string evehr = dvperiod[0]["mor"].ToString();
                                        string nofh = dvperiod[0]["No_of_hrs_per_day"].ToString();
                                        if (morhr != null && morhr.Trim() != "")
                                        {
                                            frshlf = Convert.ToInt32(morhr);
                                        }
                                        if (evehr != null && evehr.Trim() != "")
                                        {
                                            schlf = Convert.ToInt32(evehr);
                                        }
                                        if (evehr != null && evehr.Trim() != "")
                                        {
                                            noofhrs = Convert.ToInt32(nofh);
                                        }
                                    }
                                    string getcurrent_sem = string.Empty;
                                    dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                    DataView dvcurrsem = dsall.Tables[5].DefaultView;
                                    if (dvcurrsem.Count > 0)
                                    {
                                        getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                    }
                                    if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                    {
                                        string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                        string altersetion = string.Empty;
                                        if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                        {
                                            altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                        }
                                        string tmp_datevalue = string.Empty;
                                        string noofdays = string.Empty;
                                        string start_datesem = string.Empty;
                                        for (int row_inc = 0; row_inc <= days; row_inc++) //Date Loop
                                        {
                                            if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                            {
                                                degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                            }
                                            DateTime cur_day = new DateTime();
                                            cur_day = dt2.AddDays(-row_inc);
                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                            {
                                                tmp_datevalue = Convert.ToString(cur_day);
                                                degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                string SchOrder = string.Empty;
                                                string day_from = cur_day.ToString("yyyy-MM-dd");
                                                DateTime schfromdate = cur_day;
                                                dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                                dvsemster = dsall.Tables[1].DefaultView;
                                                if (dvsemster.Count > 0)
                                                {
                                                    getdate = dvsemster[0]["FromDate"].ToString();
                                                }
                                                else
                                                {
                                                    getdate = string.Empty;
                                                }
                                                if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                {
                                                    DateTime getsche = Convert.ToDateTime(getdate);
                                                    if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                    {
                                                        if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                        {
                                                            string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                            string[] sp_rd_semi = contvar.Split(',');
                                                            if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                            {
                                                                SchOrder = sp_rd_semi[0].ToString();
                                                                noofdays = sp_rd_semi[1].ToString();
                                                            }
                                                        }
                                                        degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                        if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                        {
                                                            string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                            string[] sp_rd_semi = contvar.Split(',');
                                                            if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                            {
                                                                start_datesem = sp_rd_semi[0].ToString();
                                                                start_dayorder = sp_rd_semi[1].ToString();
                                                            }
                                                        }
                                                        if (noofdays.ToString().Trim() != "")
                                                        {
                                                            if (SchOrder == "1")
                                                            {
                                                                strday = cur_day.ToString("ddd");
                                                            }
                                                            else
                                                            {
                                                                string[] sps = dt2.ToString().Split('/');
                                                                string curdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                                                                strday = dac.findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                            }
                                                            if (strday.ToString().Trim() != "")
                                                            {
                                                                string reasonsun = string.Empty;
                                                                if (hatholiday.Contains(cur_day.ToString()))
                                                                {
                                                                    reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                }
                                                                if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                                {
                                                                    string str_day = strday;
                                                                    string Atmonth = cur_day.Month.ToString();
                                                                    string Atyear = cur_day.Year.ToString();
                                                                    long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                                    string day_aten = cur_day.Day.ToString();
                                                                    string strsectionvar = string.Empty;
                                                                    string labsection = string.Empty;
                                                                    if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                    {
                                                                        strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                        labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                    }
                                                                    sql1 = " and (" + sql1 + ")";
                                                                    dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                                    dvalternaet = dsall.Tables[0].DefaultView;
                                                                    int temp = 0;
                                                                    Boolean moringleav = false;
                                                                    Boolean evenleave = false;
                                                                    dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                    dvholiday = dsall.Tables[2].DefaultView;
                                                                    if (dvholiday.Count > 0)
                                                                    {
                                                                        if (!hatholiday.Contains(cur_day.ToString()))
                                                                        {
                                                                            hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                                        }
                                                                        if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                        {
                                                                            moringleav = true;
                                                                        }
                                                                        if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                        {
                                                                            evenleave = true;
                                                                        }
                                                                        if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                        {
                                                                            evenleave = true;
                                                                            moringleav = true;
                                                                        }
                                                                    }
                                                                    int finhours = noofhrs;
                                                                    if (moringleav == true && evenleave == false)
                                                                    {
                                                                        temp = frshlf + 1;
                                                                    }
                                                                    if (moringleav == false && evenleave == true)
                                                                    {
                                                                        finhours = frshlf;
                                                                    }
                                                                    if (moringleav == true && evenleave == true)
                                                                    {
                                                                        finhours = 0;
                                                                    }
                                                                    for (temp = 1; temp <= finhours; temp++)
                                                                    {
                                                                        string sp_rd = string.Empty;
                                                                        string getcolumnfield = Convert.ToString(strday + temp);
                                                                        if (dvsemster.Count > 0)
                                                                        {
                                                                            if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                            {
                                                                                sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                string[] sp_rd_semi = sp_rd.Split(';');
                                                                                for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        int upperbound = sp2.GetUpperBound(0);
                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff] == staff_code)
                                                                                            {
                                                                                                recflag = true;
                                                                                                if (!hatcombinedclass.Contains(cur_day.ToString() + '&' + temp))
                                                                                                {
                                                                                                    hatcombinedclass.Add(cur_day.ToString() + '&' + temp, 1);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    combainedflag = true;
                                                                                                    int hrval = Convert.ToInt32(hatcombinedclass[cur_day.ToString() + '&' + temp]);
                                                                                                    hrval++;
                                                                                                    hatcombinedclass[cur_day.ToString() + '&' + temp] = hrval;
                                                                                                    noofhourscombine++;
                                                                                                }
                                                                                                staffhour = true;
                                                                                                if (hatstaffhours.Contains(sp2[0]))
                                                                                                {
                                                                                                    int ho = Convert.ToInt32(hatstaffhours[sp2[0]]);
                                                                                                    ho++;
                                                                                                    hatstaffhours[sp2[0]] = ho;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    hatstaffhours.Add(sp2[0], 1);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (staffhour == true)
                                    {
                                        load_spread.Visible = true;
                                        foreach (DictionaryEntry entry in hatstaffhours)
                                        {
                                            dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + entry.Key.ToString() + "";
                                            dvsubject = dsall.Tables[4].DefaultView;
                                            if (dvsubject.Count > 0)
                                            {
                                                subjectname_1 = dvsubject[0]["subject_name"].ToString();
                                            }
                                            stafftotalrowspan++;
                                            if (srnoflag == false)
                                            {
                                                sno++;
                                                srnoflag = true;
                                                load_spread.Sheets[0].RowCount++;
                                                load_spread.Sheets[0].Rows[load_spread.Sheets[0].RowCount - 1].Visible = false;
                                            }
                                            load_spread.Sheets[0].RowCount++;
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Text = sno.ToString();
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 1].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 2].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Text = ds_staff.Tables[0].Rows[temp_staff]["desig_name"].ToString();
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 4].Text = staff_dept.ToString();
                                            if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                            {
                                                degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                            }
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 5].Text = dsperiod.Tables[0].Rows[pre]["batch_year"] + " " + degreename + "-Sem:" + dsperiod.Tables[0].Rows[pre]["semester"] + getsection;
                                            load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 6].Text = subjectname_1;
                                            stafftotalhours = stafftotalhours + Convert.ToInt32(entry.Value.ToString());
                                            if (!hatcurlab.Contains(entry.Key.ToString()))
                                            {
                                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 7].Text = entry.Value.ToString();
                                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 8].Text = "0";
                                            }
                                            else
                                            {
                                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 7].Text = "0";
                                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 8].Text = entry.Value.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (stafftotalrowspan > 0)
                        {
                            //load_spread.Sheets[0].SpanModel.Add(load_spread.Sheets[0].RowCount - stafftotalrowspan, 9, stafftotalrowspan, 1);
                            //load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - stafftotalrowspan), 9].Text = stafftotalhours.ToString();
                            // if (combainedflag == true && cbcombine.Checked == true)
                            // {
                            //     load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - stafftotalrowspan), 9].BackColor = Color.Green;
                            // }
                            if (combainedflag == true && cbcombine.Checked == true)
                            {
                                if (stafftotalhours > noofhourscombine)
                                {
                                    stafftotalhours = stafftotalhours - noofhourscombine;
                                }
                            }
                            for (int st = 1; st <= stafftotalrowspan; st++)
                            {
                                if (combainedflag == true && cbcombine.Checked == true)
                                {
                                    load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - st), 9].BackColor = Color.Green;
                                }
                                load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - st), 9].Text = stafftotalhours.ToString();
                            }
                        }
                    }
                }
            }
            if (recflag == true)
            {
                chk_sms.Visible = false;
                chk_mail.Visible = false;
                txt_message.Visible = false;
                btnsms.Visible = false;
                load_spread.Height = 500;
                load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
                load_spread.Width = 946;
                load_spread.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
            }
            else
            {
                chk_sms.Visible = false;
                chk_mail.Visible = false;
                txt_message.Visible = false;
                btnsms.Visible = false;
                load_spread.Visible = false;
                errlbl.Text = "No Records are found";
                errlbl.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errlbl.Text = ex.ToString();
            errlbl.Visible = true;
        }
    }

    //public void individual_workload()
    //{
    //    Hashtable alltotalvalues = new Hashtable(); // added sridhar  for total issue
    //    int chekrowscountforsubjects = 0;
    //    string staffcode_1 = string.Empty;
    //    string subjectcode_1 = string.Empty;
    //    string code = string.Empty;
    //    string subjectname_1 = string.Empty;
    //    string semester_1 = string.Empty;
    //    string semester_2 = string.Empty;
    //    string batchyear_1 = string.Empty;
    //    string section_1 = string.Empty;
    //    string desig_name = string.Empty;
    //    string staff_name_1 = string.Empty;
    //    string degree_code_1 = string.Empty;
    //    string deptname = string.Empty;
    //    string staff_dept = string.Empty;
    //    string staff_cat_code = string.Empty;
    //    string subcode = string.Empty;
    //    string stfcode = string.Empty;
    //    string theory_prac = string.Empty;
    //    int row_exists = 0;
    //    int sno = 0;
    //    int theorycount = 0;
    //    int practicalcount = 0;
    //    long totalhr = 0;
    //    int rowspan1 = 0;
    //    int rowspan2 = 0;
    //    int rowspan3 = 0;
    //    int startrow = 0;
    //    int endrow = 0;
    //    List<string> day_name = new List<string>();
    //    List<string> overload = new List<string>();
    //    DataView dv_alternate = new DataView();
    //    load_spread.Sheets[0].ColumnHeader.RowCount = 0;
    //    load_spread.Sheets[0].RowCount = 0;
    //    load_spread.Sheets[0].ColumnCount = 0;
    //    load_spread.Sheets[0].ColumnHeader.RowCount = 2;
    //    load_spread.Sheets[0].ColumnCount = 10;
    //    load_spread.Sheets[0].FrozenColumnCount = 5;
    //    load_spread.Sheets[0].RowHeader.Visible = false;
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year/ Branch / Sem / Sec";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Handling Subjects";
    //    //load_spread.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Batch Year/ Branch / Sem / Sec";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Papers";
    //    load_spread.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Theory";
    //    load_spread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Practical";
    //    load_spread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total";
    //    load_spread.Sheets[0].Columns[0].Locked = true;
    //    load_spread.Sheets[0].Columns[1].Locked = true;
    //    load_spread.Sheets[0].Columns[2].Locked = true;
    //    load_spread.Sheets[0].Columns[3].Locked = true;
    //    load_spread.Sheets[0].Columns[4].Locked = true;
    //    load_spread.Sheets[0].Columns[5].Locked = true;
    //    load_spread.Sheets[0].Columns[6].Locked = true;
    //    load_spread.Sheets[0].Columns[7].Locked = true;
    //    load_spread.Sheets[0].Columns[8].Locked = true;
    //    load_spread.Sheets[0].Columns[9].Locked = true;
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, 6, 1, 1);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);
    //    load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
    //    load_spread.Sheets[0].Columns[0].Width = 30;
    //    load_spread.Sheets[0].Columns[1].Width = 70;
    //    load_spread.Sheets[0].Columns[2].Width = 150;
    //    load_spread.Sheets[0].Columns[3].Width = 100;
    //    load_spread.Sheets[0].Columns[4].Width = 150;
    //    load_spread.Sheets[0].Columns[5].Width = 200;
    //    load_spread.Sheets[0].Columns[6].Width = 200;
    //    load_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
    //    load_spread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
    //    load_spread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
    //    load_spread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
    //    //load_spread.Width = 950;
    //    //load_spread.Height = 200;      
    //    load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    load_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
    //    load_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
    //    load_spread.Sheets[0].RowHeader.Visible = false;
    //    string[] split_fromdate = txtFromDate.Text.Split(new char[] { '/' });
    //    string[] split_todate = txtToDate.Text.Split(new char[] { '/' });
    //    DateTime dt_fromdate = Convert.ToDateTime(split_fromdate[1] + "/" + split_fromdate[0] + "/" + split_fromdate[2]);
    //    DateTime dt_todate = Convert.ToDateTime(split_todate[1] + "/" + split_todate[0] + "/" + split_todate[2]);
    //    if (dt_fromdate > dt_todate)
    //    {
    //        chk_sms.Visible = false;
    //        chk_mail.Visible = false;
    //        txt_message.Visible = false;
    //        btnsms.Visible = false;
    //        load_spread.Visible = false;
    //        errlbl.Text = "From  Date Must Be Lesser Than or Equal to To Date";
    //        errlbl.Visible = true;
    //        return;
    //    }
    //    string staffname = string.Empty;
    //    //con.Close();=====================Modified by Venkat========================
    //    //con.Open();
    //    //SqlCommand cmd_tot_hrs = new SqlCommand("select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half'  from PeriodAttndSchedule", con);
    //    //string totalhrs = Convert.ToString(cmd_tot_hrs.ExecuteScalar());
    //    string str = "select max(No_of_hrs_per_day) as 'Total Hours',max(no_of_hrs_I_half_day) as 'First Half'  from PeriodAttndSchedule";
    //    ds = dac.select_method_wo_parameter(str, "Text");
    //    string totalhrs = ds.Tables[0].Rows[0]["Total Hours"].ToString();
    //    if (string.IsNullOrEmpty(totalhrs))
    //    {
    //        totalhrs = "0";
    //    }
    //    string semesters =string.Empty;
    //    for (int getsem = 0; getsem < ddlsem.Items.Count; getsem++)
    //    {
    //        if (ddlsem.Items[getsem].Selected == true)
    //        {
    //            if (semesters == string.Empty)
    //            {
    //                semesters = ddlsem.Items[getsem].Text.ToString();
    //            }
    //            else
    //            {
    //                semesters = semesters + "','" + ddlsem.Items[getsem].Text.ToString();
    //            }
    //        }
    //    }
    //    if (stafftxt.Items.Count > 0)
    //    {
    //        int loopcount = 1;
    //        int startcnt = 0;
    //        if (stafftxt.SelectedItem.ToString() != "All")
    //        {
    //            loopcount = 1;
    //            startcnt = 0;
    //        }
    //        else
    //        {
    //            loopcount = stafftxt.Items.Count;
    //            startcnt = 1;
    //        }
    //        Hashtable combinestaff = new Hashtable(); // Added by jairam 18-11-2014 
    //        int combinehour = 0; // Added by jairam 18-11-2014 
    //        for (int selstaff = startcnt; selstaff < loopcount; selstaff++)
    //        {
    //            combinestaff.Clear(); // Added by jairam 18-11-2014 
    //            if (stafftxt.SelectedItem.ToString() != "All")
    //            {
    //                staffcode_selected = stafftxt.SelectedValue.ToString();
    //            }
    //            else
    //            {
    //                staffcode_selected = stafftxt.Items[selstaff].Value.ToString();
    //            }
    //            staffname = staffcode_selected.ToString(); // stafftxt.SelectedValue.ToString();
    //            if (staffname=="POLY131")
    //            {
    //            }
    //            if (desigddl.SelectedValue == "All" || desigddl.SelectedValue == "")
    //            {
    //                strdesig =string.Empty;
    //            }
    //            else
    //            {
    //                strdesig = " and desig_name='" + desigddl.SelectedItem.ToString() + "'";
    //            }
    //            //--------------department ALL
    //            if (deptddl.SelectedValue == "All" || deptddl.SelectedValue == "")
    //            {
    //                strdept =string.Empty;
    //            }
    //            else
    //            {
    //                strdept = " and h.dept_code='" + deptddl.SelectedValue.ToString() + "'";
    //                //strdept = " and h.dept_code='" + GetFunction("select dept_code from  degree where degree_code='" + deptddl.SelectedValue.ToString() + "'") + "'";
    //            }
    //            //--------------staff name
    //            string only_staff_code =string.Empty;
    //            strstaff = " and m.staff_code='" + staffcode_selected.ToString() + "'";
    //            only_staff_code = " and ss.staff_code<>'" + staffcode_selected.ToString() + "'";
    //            pnl_filter.Visible = true;
    //            has.Clear();
    //            has.Add("@coll_code", ddlcollege.SelectedValue.ToString());
    //            has.Add("@subjno ", " ");
    //            has.Add("@strdesig  ", strdesig);
    //            has.Add("@strdept", strdept);
    //            has.Add("@strstaff ", @strstaff);
    //            ds_staff = dac.select_method("workload_getstafflist", has, "sp");
    //            string snostaffcode="";
    //            for (int temp_staff = 0; temp_staff < ds_staff.Tables[0].Rows.Count; temp_staff++)
    //            {
    //                theorycount = 0;
    //                practicalcount = 0;
    //                staff_code = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
    //                staff_dept = ds_staff.Tables[0].Rows[temp_staff]["dept_name"].ToString();
    //                desig_name = ds_staff.Tables[0].Rows[temp_staff]["desig_name"].ToString();
    //                staff_name_1 = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
    //                staff_cat_code = ds_staff.Tables[0].Rows[temp_staff]["category_code"].ToString();
    //               // sno++;
    //                //SqlFinal = "select distinct s.subject_name,r.degree_code,r.batch_year,r.current_semester,r.sections from staff_selector sl,syllabus_master sy,subject s,registration r where sl.subject_no=s.subject_no and sy.syll_code=s.syll_code and r.batch_year=sy.batch_year and r.degree_code=sy.degree_code and r.current_semester=sy.semester and sl.batch_year=r.batch_year and staff_code ='" + staff_code + "' and r.current_semester in('" + semester_1 + "') and cc=0 and delflag=0 and exam_flag<>'debar' order by r.degree_code,r.batch_year";
    //                //Modified by Srinath 29/03/2014
    //                // SqlFinal = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code  from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and staff_code='" + staff_code + "' and semester in('" + semesters + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and st.sections=r.sections  order by st.batch_year,sy.degree_code,semester,st.sections";
    //                //SqlFinal = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code,s.subject_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and staff_code='" + staff_code + "' and semester in('" + semesters + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and st.sections=r.sections  order by st.batch_year,sy.degree_code,semester,st.sections";
    //                SqlFinal = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,si.semester,st.sections,sy.degree_code,s.subject_code from subject s,syllabus_master sy,staff_selector st,registration r,seminfo si where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and staff_code='" + staff_code + "' and sy.semester in('" + semesters + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and st.sections=r.sections and si.batch_year=r.Batch_Year and r.degree_code=si.degree_code and r.Current_Semester=si.semester and si.batch_year=st.batch_year and si.batch_year=sy.Batch_Year and r.degree_code=sy.degree_code and si.semester=sy.semester and (si.start_date between '" + dt_fromdate.ToString("MM/dd/yyyy") + "' and '" + dt_todate.ToString("MM/dd/yyyy") + "'  or si.end_date between '" + dt_fromdate.ToString("MM/dd/yyyy") + "' and '" + dt_todate.ToString("MM/dd/yyyy") + "'  or '" + dt_todate.ToString("MM/dd/yyyy") + "' between si.start_date and si.end_date or '" + dt_fromdate.ToString("MM/dd/yyyy") + "' between si.start_date and si.end_date)   order by st.batch_year,sy.degree_code,si.semester,st.sections";
    //                DataSet ds_staff_schedule = new DataSet();
    //                // ds_staff_schedule = dac.select_method(SqlFinal, hat, "Text");============Modified by Venkat===================
    //                ds_staff_schedule = dac.select_method_wo_parameter(SqlFinal, "Text");
    //                //Added by Srinath 29/03/2014
    //                string subject = subjddl.SelectedItem.ToString();
    //                string subjectco = subjddl.SelectedValue.ToString();
    //                chekrowscountforsubjects = ds_staff_schedule.Tables[0].Rows.Count;
    //                for (int row_cnt = 0; row_cnt < ds_staff_schedule.Tables[0].Rows.Count; row_cnt++)
    //                {  //Added by Srinath 29/03/2014
    //                    Boolean subflag = false;
    //                    if (subject == "All")
    //                    {
    //                        subflag = true;
    //                    }
    //                    else
    //                    {
    //                        string actcode = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_code"].ToString();
    //                        string actsub = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_name"].ToString();
    //                        if (subjectco == actcode || subjectco == actsub)
    //                        {
    //                            subflag = true;
    //                        }
    //                        else
    //                        {
    //                            subflag = false;
    //                        }
    //                    }
    //                    if (subflag == true)
    //                    {
    //                        batchyear_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["batch_year"].ToString();
    //                        semester_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["semester"].ToString();
    //                        degree_code_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["degree_code"].ToString();
    //                        subjectname_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_name"].ToString();
    //                        string subjectnumber = ds_staff_schedule.Tables[0].Rows[row_cnt]["subject_no"].ToString();
    //                        //staffcode_1 = ds_staff_schedule.Tables[0].Rows[row_cnt]["staff_code"].ToString();
    //                        string sectvar =string.Empty;
    //                        if (ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != string.Empty && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != "-1" && ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() != null)
    //                        {
    //                            section_1 = "-Sec:" + ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString();
    //                            sectvar = " and s.sections='" + ds_staff_schedule.Tables[0].Rows[row_cnt]["sections"].ToString() + "'";
    //                        }
    //                        else
    //                        {
    //                            section_1 = string.Empty;
    //                            sectvar =string.Empty;
    //                        }
    //                        //subjectname_1 = GetFunction("select subject_name from subject where subject_no='" + subjectcode_1 + "'");
    //                        //staff_name_1 = GetFunction("select staff_name from staffmaster where staff_code='" + staffcode_1 + "'");
    //                        deptname = GetFunction("select Dept_Name from department where Dept_Code=(select dept_code from degree where degree_code='" + degree_code_1 + "')");
    //                        load_spread.Sheets[0].RowCount++;
    //                        //sno++;
    //                        if (snostaffcode!=ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString())
    //                        {
    //                            sno++;
    //                            snostaffcode = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
    //                        }
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 0].Text = sno.ToString();
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 1].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 2].Text = ds_staff.Tables[0].Rows[temp_staff]["staff_name"].ToString();
    //                        //load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Note = ds_staff.Tables[0].Rows[temp_staff]["staff_code"].ToString();
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 3].Text = ds_staff.Tables[0].Rows[temp_staff]["desig_name"].ToString();
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 4].Text = staff_dept.ToString(); //
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 5].Text = batchyear_1 + " " + deptname + "-Sem:" + semester_1 + section_1; //
    //                        load_spread.Sheets[0].Cells[(load_spread.Sheets[0].RowCount - 1), 6].Text = subjectname_1;
    //                        row_exists = 1;
    //                        if ((selstaff == 0) || (stafftxt.SelectedItem.ToString() == "All" && selstaff == 1))
    //                        {
    //                            startrow = 0;
    //                        }
    //                        if (row_cnt == 0)
    //                        {
    //                            rowspan1 = load_spread.Sheets[0].RowCount - 1;
    //                        }
    //                        has_holiday.Clear();
    //                        has_holiday.Add("@fromdate", dt_fromdate.ToShortDateString());
    //                        has_holiday.Add("@todate", dt_todate.ToShortDateString());
    //                        has_holiday.Add("@category_code", staff_cat_code);
    //                        ds_holi = dac.select_method("get_staff_holiday", has_holiday, "sp");
    //                        holiday_count = ds_holi.Tables[0].Rows.Count;
    //                        has_holiday.Clear();
    //                        for (temp_holi_count = 0; temp_holi_count < holiday_count; temp_holi_count++)
    //                        {
    //                            has_holiday.Add(ds_holi.Tables[0].Rows[temp_holi_count]["holiday_date"], ds_holi.Tables[0].Rows[temp_holi_count]["holiday_desc"]);
    //                        }
    //                        sql1 =string.Empty; Strsql =string.Empty; asql =string.Empty;
    //                        string sqlsub =string.Empty;
    //                        sql_s = "select semester_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=semester_schedule.degree_code and semester=semester_schedule.semester), ";
    //                        asql = "select Alternate_schedule.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=Alternate_schedule.degree_code and semester=Alternate_schedule.semester), ";
    //                        for (int day_lp = 0; day_lp < 7; day_lp++)
    //                        {
    //                            strday = Days[day_lp].ToString();
    //                            for (int i_loop = 1; i_loop <= Convert.ToUInt32(totalhrs); i_loop++)
    //                            {
    //                                //Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
    //                                Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
    //                                if (sql1 == "")
    //                                {
    //                                    sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
    //                                }
    //                                else
    //                                {
    //                                    sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + (string)staff_code + "%'";
    //                                }
    //                                //aruna=================
    //                                if (sqlsub == "")
    //                                {
    //                                    sqlsub = sqlsub + strday + Convert.ToString(i_loop) + " like '%" + (string)subjectnumber + "%'";
    //                                }
    //                                else
    //                                {
    //                                    sqlsub = sqlsub + " or " + strday + Convert.ToString(i_loop) + " like '%" + (string)subjectnumber + "%'";
    //                                }
    //                                //=====================
    //                            }
    //                        }
    //                        sql1 = "(" + sql1 + ")";
    //                        sqlsub = " and (" + sqlsub + ")";  //aruna
    //                        sql1 = sql1 + sqlsub; //aruna
    //                        sql_s = sql_s + Strsql + "";
    //                        asql = asql + Strsql + "";
    //                        SqlBatchYear1 = "(select distinct(registration.batch_year) from registration,Alternate_schedule where registration.degree_code=Alternate_schedule.degree_code and registration.cc=0 and delflag=0 and registration.exam_flag<>'DEBAR' AND registration.current_Semester = Alternate_schedule.semester)";
    //                        SqlPrefinal11 = asql + " semester,sections,batch_year from Alternate_schedule where batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester=1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
    //                        SqlPrefinal22 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate between '" + dt_fromdate + "' and '" + dt_todate + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Semester' and First_Year_Nonsemester=0)";
    //                        SqlPrefinal33 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate between '" + dt_fromdate + "' and '" + dt_todate + "' and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and degree_code in (select degree_code from degree where Exam_System='Non Semester') ";
    //                        SqlPrefinal44 = asql + " semester,sections,batch_year from Alternate_schedule where  FromDate between '" + dt_fromdate + "' and '" + dt_todate + "'  and batch_year in " + SqlBatchYear1 + " and " + sql1 + " and semester<>1 and semester<>-1 and degree_code in (select degree_code from degree where Exam_System= 'Semester' and First_Year_Nonsemester<>0)";
    //                        SqlFinal1 = "(" + SqlPrefinal11 + ") union all (" + SqlPrefinal44 + ") union all (" + SqlPrefinal22 + ") union all (" + SqlPrefinal33 + ")";
    //                        //========================================================
    //                        SqlFinal =string.Empty;
    //                        SqlFinal = " select distinct  r.degree_code,(select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=s.degree_code and semester=s.semester),";
    //                        SqlFinal = SqlFinal + Strsql;
    //                        SqlFinal = SqlFinal + " s.semester,r.sections,r.batch_year ";
    //                        SqlFinal = SqlFinal + " from semester_schedule s,registration r where s.semester=r.current_semester and s.batch_year=r.batch_year and s.degree_code=r.degree_code and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and s.sections=r.sections and ";
    //                        SqlFinal = SqlFinal + "(" + sql1 + ")";
    //                        //SqlFinal = SqlFinal + " and FromDate in (select top 1 FromDate from semester_schedule where degree_code =r.degree_code  and semester = s.semester  and batch_year = r.batch_year and FromDate <='" + datefrom + "'  order by FromDate Desc)";
    //                        SqlFinal = SqlFinal + " and s.degree_code=" + degree_code_1 + " and s.batch_year=" + batchyear_1 + " and s.semester=" + semester_1 + " " + sectvar + "";
    //                        SqlFinal = SqlFinal + " order by r.degree_code,r.batch_year,s.semester,r.sections";
    //                        //========================================================
    //                        DataSet ds_sqlfinal = new DataSet();//dataset for semester_schedule
    //                        //ds_sqlfinal = dac.select_method(SqlFinal, hat, "Text");==================Modified by Venkat=============
    //                        ds_sqlfinal = dac.select_method_wo_parameter(SqlFinal, "Text");
    //                        string tempdegree =string.Empty;
    //                        //added by srinath 25/1/2014 ========Start=====
    //                        Dictionary<string, string> availabledegree = new Dictionary<string, string>();
    //                        for (int chkst = 0; chkst < ds_sqlfinal.Tables[0].Rows.Count; chkst++)
    //                        {
    //                            string degree = ds_sqlfinal.Tables[0].Rows[chkst]["degree_code"].ToString();
    //                            string batch = ds_sqlfinal.Tables[0].Rows[chkst]["Batch_year"].ToString();
    //                            string sem = ds_sqlfinal.Tables[0].Rows[chkst]["semester"].ToString();
    //                            string sce = ds_sqlfinal.Tables[0].Rows[chkst]["Sections"].ToString();
    //                            if (sce != "0" & sce.Trim() != "" && sce != "-1" && sce != null)
    //                                sce = "and sections='" + sce + "'";
    //                            else
    //                                sce =string.Empty;
    //                            int count = int.Parse(d2.GetFunction("Select Count(*) from registration where degree_code='" + degree + "' and batch_year='" + batch + "' and current_semester='" + sem + "' " + sce + " and cc=0 and delflag=0 and exam_flag<>'deber'"));
    //                            if (count > 0)
    //                            {
    //                            }
    //                            else
    //                            {
    //                                availabledegree.Add(chkst.ToString(), chkst.ToString());
    //                            }
    //                        }
    //                        //===========End
    //                        DataSet ds_sqlfinal1 = new DataSet();//dataset for alternate_schedule
    //                        ds_sqlfinal1 = dac.select_method(SqlFinal1, hat, "Text");
    //                        //day_name.Clear();
    //                        if (ds_sqlfinal.Tables[0].Rows.Count > 0)
    //                        {
    //                            for (DateTime loop_date = dt_fromdate; loop_date <= dt_todate; loop_date = loop_date.AddDays(1))
    //                            {
    //                                if (has_holiday.ContainsKey(loop_date))
    //                                {
    //                                }
    //                                else
    //                                {
    //                                    for (int i = 0; i < 1; i++)
    //                                    {
    //                                        string strcmd = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString() + " and semester = " + ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString() + "";
    //                                        DataSet ds_cmd = new DataSet();
    //                                        // ds_cmd = dac.select_method(strcmd, hat, "Text");============Modified by Venkat=============
    //                                        ds_cmd = dac.select_method_wo_parameter(strcmd, "Text");
    //                                        if (ds_cmd.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if ((ds_cmd.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString()) != "")
    //                                            {
    //                                                intNHrs = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["No_of_hrs_per_day"]);
    //                                                SchOrder = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["schorder"]);
    //                                                nodays = Convert.ToInt16(ds_cmd.Tables[0].Rows[0]["nodays"]);
    //                                            }
    //                                        }
    //                                        string str_seminfo = "select * from seminfo where degree_code=" + ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString() + " and semester=" + ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString() + " and batch_year=" + ds_sqlfinal.Tables[0].Rows[i]["batch_year"].ToString() + " ";
    //                                        DataSet ds_seminfo = new DataSet();
    //                                        //ds_seminfo = dac.select_method(str_seminfo, hat, "Text");============Modified by Venkat===============
    //                                        ds_seminfo = dac.select_method_wo_parameter(str_seminfo, "Text");
    //                                        if (ds_seminfo.Tables[0].Rows.Count > 0)
    //                                        {
    //                                            if ((ds_seminfo.Tables[0].Rows[0]["start_date"].ToString()) != "" && (ds_seminfo.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
    //                                            {
    //                                                string[] tmpdate = ds_seminfo.Tables[0].Rows[0]["start_date"].ToString().Split(new char[] { ' ' });
    //                                                string[] enddate = ds_seminfo.Tables[0].Rows[0]["end_date"].ToString().Split(new char[] { ' ' });
    //                                                startdate = tmpdate[0].ToString();
    //                                                splitenddate = enddate[0].ToString();
    //                                                if (Convert.ToString(ds_seminfo.Tables[0].Rows[0]["starting_dayorder"]) != "")
    //                                                {
    //                                                    start_dayorder = ds_seminfo.Tables[0].Rows[0]["starting_dayorder"].ToString();
    //                                                }
    //                                                else
    //                                                {
    //                                                    start_dayorder = "1";
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                errlbl.ForeColor = Color.Red;
    //                                                errlbl.Text = "Update semester Information";
    //                                                errlbl.Visible = true;
    //                                                return;
    //                                            }
    //                                        }
    //                                        curr_sem = ds_sqlfinal.Tables[0].Rows[i]["semester"].ToString();
    //                                        degree_code = ds_sqlfinal.Tables[0].Rows[i]["degree_code"].ToString();
    //                                        if (intNHrs > 0)
    //                                        {
    //                                            if (SchOrder != 0)
    //                                            {
    //                                                strday = loop_date.ToString("ddd");
    //                                                strday = strday.ToLower();
    //                                                for (int set_hr = 1; set_hr <= Convert.ToInt16(totalhrs); set_hr++)
    //                                                {
    //                                                    day_name.Add(strday + set_hr);
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                string get_date = Convert.ToString(loop_date);
    //                                                string[] split_find_date = get_date.Split(new char[] { ' ' });
    //                                                string[] split_date_only = split_find_date[0].Split(new char[] { '/' });
    //                                                string findday_date = split_date_only[1] + "/" + split_date_only[0] + "/" + split_date_only[2];
    //                                                strday = findday(findday_date.ToString(), startdate.ToString(), nodays.ToString(), start_dayorder.ToString());
    //                                                strday = strday.ToLower();
    //                                                for (int set_hr = 1; set_hr <= Convert.ToInt16(totalhrs); set_hr++)
    //                                                {
    //                                                    day_name.Add(strday + set_hr);
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            int read_col = 0;
    //                            con.Close();
    //                            con.Open();
    //                            for (read_col = 2; read_col < ds_sqlfinal.Tables[0].Columns.Count - 3; read_col++)
    //                            {
    //                                if (day_name.Contains(ds_sqlfinal.Tables[0].Columns[read_col].ColumnName))
    //                                {
    //                                    string tempdegreedetails =string.Empty;
    //                                    for (int read_row = 0; read_row < ds_sqlfinal.Tables[0].Rows.Count; read_row++)
    //                                    {
    //                                        if (!availabledegree.ContainsKey(read_row.ToString()))//added by srinath 25/1/2014
    //                                        {
    //                                            string get_value = ds_sqlfinal.Tables[0].Rows[read_row][read_col].ToString();
    //                                            string get_value_alternate = string.Empty;
    //                                            string get_sections = ds_sqlfinal.Tables[0].Rows[read_row]["sections"].ToString();
    //                                            if (!string.IsNullOrEmpty(get_sections))
    //                                            {
    //                                                get_sections = "  and sections='" + get_sections + "'";
    //                                            }
    //                                            ds_sqlfinal1.Tables[0].DefaultView.RowFilter = "batch_year='" + ds_sqlfinal.Tables[0].Rows[read_row]["batch_year"].ToString() + "' and degree_code='" + ds_sqlfinal.Tables[0].Rows[read_row]["degree_code"].ToString() + "' and semester='" + ds_sqlfinal.Tables[0].Rows[read_row]["semester"].ToString() + "'" + get_sections;
    //                                            dv_alternate = ds_sqlfinal1.Tables[0].DefaultView;
    //                                            DataTable dt_alternate = dv_alternate.ToTable();
    //                                            string degreedtails = ds_sqlfinal.Tables[0].Rows[read_row]["batch_year"].ToString() + '-' + ds_sqlfinal.Tables[0].Rows[read_row]["degree_code"].ToString() + '-' + ds_sqlfinal.Tables[0].Rows[read_row]["semester"].ToString() + '-' + get_sections;
    //                                            if (tempdegreedetails != degreedtails)
    //                                            {
    //                                                tempdegreedetails = degreedtails;
    //                                                for (int j = 2; j < dt_alternate.Columns.Count - 3; j++)
    //                                                {
    //                                                    if (dt_alternate.Columns[j].ColumnName.ToString() == ds_sqlfinal.Tables[0].Columns[read_col].ColumnName.ToString())
    //                                                    {
    //                                                        for (int i = 0; i < dt_alternate.Rows.Count; i++)
    //                                                        {
    //                                                            if (get_value_alternate == string.Empty)
    //                                                            {
    //                                                                get_value_alternate = dt_alternate.Rows[i][j].ToString();
    //                                                            }
    //                                                            else
    //                                                            {
    //                                                                get_value_alternate = get_value_alternate + ";" + dt_alternate.Rows[i][j].ToString();
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                    break;
    //                                                }
    //                                                string[] split_getvalue = get_value.Split(new char[] { ';' });
    //                                                for (int get_UP = 0; get_UP <= split_getvalue.GetUpperBound(0); get_UP++)
    //                                                {
    //                                                    string[] getstaff_code = split_getvalue[get_UP].Split(new char[] { '-' });
    //                                                    subcode =string.Empty;
    //                                                    stfcode =string.Empty;
    //                                                    if (getstaff_code.GetUpperBound(0) >= 1)
    //                                                    {
    //                                                        subcode = getstaff_code[0];
    //                                                        stfcode = getstaff_code[1];
    //                                                    }
    //                                                    if (subcode.Trim().ToString() == subjectnumber.Trim().ToString() && stfcode.Trim().ToString() == staff_code.Trim().ToString())
    //                                                    {
    //                                                        SqlCommand cmd_lab_thry = new SqlCommand("select ss.lab from sub_sem ss,subject s where s.subtype_no=ss.subtype_no and s.subject_no='" + subcode + "'", con);
    //                                                        theory_prac = Convert.ToString(cmd_lab_thry.ExecuteScalar());
    //                                                        if (stfcode == staff_code)
    //                                                        {
    //                                                            if (theory_prac.Trim().ToLower() == "false" || theory_prac=="0")
    //                                                            {
    //                                                                if (!combinestaff.ContainsKey(read_col)) // Added by jairam 18-11-2014 
    //                                                                {
    //                                                                    combinestaff.Add(read_col, stfcode);
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    combinehour++;
    //                                                                }
    //                                                                theorycount++;
    //                                                            }
    //                                                            else if (theory_prac.Trim().ToLower() == "true" || theory_prac == "1")
    //                                                            {
    //                                                                practicalcount++;
    //                                                                if (!combinestaff.ContainsKey(read_col)) // Added by jairam 18-11-2014 
    //                                                                {
    //                                                                    combinestaff.Add(read_col, stfcode);
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    combinehour++;
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                                if (get_value_alternate != string.Empty)
    //                                                {
    //                                                    string[] split_alternate = get_value_alternate.Split(new char[] { ';' });
    //                                                    for (int get_upper = 0; get_upper <= split_alternate.GetUpperBound(0); get_upper++)
    //                                                    {
    //                                                        string[] single_val = split_alternate[get_upper].Split(new char[] { '-' });
    //                                                        subcode =string.Empty;
    //                                                        stfcode =string.Empty;
    //                                                        if (single_val.GetUpperBound(0) >= 1)
    //                                                        {
    //                                                            subcode = single_val[0];
    //                                                            stfcode = single_val[1];
    //                                                        }
    //                                                        if (subcode.Trim().ToString() == subjectnumber.Trim().ToString() && stfcode.Trim().ToString() == staff_code.Trim().ToString())
    //                                                        {
    //                                                            SqlCommand cmd_lab_thry = new SqlCommand("select ss.lab from sub_sem ss,subject s where s.subtype_no=ss.subtype_no and s.subject_no='" + subcode + "'", con);
    //                                                            theory_prac = Convert.ToString(cmd_lab_thry.ExecuteScalar());
    //                                                            if (theory_prac.Trim().ToLower() == "false" || theory_prac == "0")
    //                                                            {
    //                                                                if (!combinestaff.ContainsKey(read_col)) // Added by jairam 18-11-2014
    //                                                                {
    //                                                                    combinestaff.Add(read_col, stfcode);
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    combinehour++;
    //                                                                }
    //                                                                theorycount++;
    //                                                            }
    //                                                            else if (theory_prac.Trim().ToLower() == "true" || theory_prac == "1")
    //                                                            {
    //                                                                practicalcount++;
    //                                                                if (!combinestaff.ContainsKey(read_col)) // Added by jairam 18-11-2014
    //                                                                {
    //                                                                    combinestaff.Add(read_col, stfcode);
    //                                                                }
    //                                                                else
    //                                                                {
    //                                                                    combinehour++;
    //                                                                }
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    read_col += (Convert.ToInt32(totalhrs) - 1);  // jairam 
    //                                }
    //                            }
    //                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 7].Text = theorycount.ToString();
    //                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 8].Text = practicalcount.ToString();
    //                            totalhr = totalhr + theorycount + practicalcount;
    //                        }
    //                        theorycount = 0;
    //                        practicalcount = 0;
    //                    }//Added by srinath 9/03/2014
    //                }
    //            }
    //            if (load_spread.Sheets[0].RowCount > 0) 
    //            {
    //                if (load_spread.Sheets[0].RowCount != startrow)
    //                {
    //                    endrow = load_spread.Sheets[0].RowCount - startrow;
    //                }
    //                if (stafftxt.SelectedItem.ToString() != "All")
    //                {
    //                    if (cbcombine.Checked == true) // Added by jairam 18-11-2014 
    //                    {
    //                        if (combinehour == 0)
    //                        {
    //                            int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                            load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                            load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                        }
    //                        else
    //                        {
    //                            if (ddlreporttype.SelectedIndex == 2)
    //                            {
    //                                int value = Convert.ToInt32(totalhr) - Convert.ToInt32(combinehour);
    //                                int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                                load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                                load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(value);
    //                                load_spread.Sheets[0].Cells[0, 9].BackColor = Color.Green;
    //                            }
    //                            else
    //                            {
    //                                int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                                load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                                load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                        load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                        load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                    }
    //                }
    //                else
    //                {
    //                    if (cbcombine.Checked == true) // Added by jairam 18-11-2014 
    //                    {
    //                        if (combinehour == 0)
    //                        {
    //                            if (startrow == 0)
    //                            {
    //                                int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                                load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                                load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                            }
    //                            else
    //                            {
    //                                load_spread.Sheets[0].SpanModel.Add(startrow, 9, endrow, 1);
    //                                if (load_spread.Sheets[0].RowCount != startrow)
    //                                {
    //                                    load_spread.Sheets[0].Cells[startrow, 9].Text = Convert.ToString(totalhr);
    //                                    load_spread.Sheets[0].Cells[startrow, 9].Note = Convert.ToString(totalhr);
    //                                }
    //                                else
    //                                {
    //                                    //if (chekrowscountforsubjects == 0)
    //                                    //{
    //                                    //}
    //                                    //else
    //                                    //{
    //                                    //    load_spread.Sheets[0].Cells[startrow - 1, 9].Text = Convert.ToString(totalhr);
    //                                    //}
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (startrow == 0)
    //                            {
    //                                if (ddlreporttype.SelectedIndex == 2)
    //                                {
    //                                    int value = Convert.ToInt32(totalhr) - Convert.ToInt32(combinehour);
    //                                    int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                                    load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                                    load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(value);
    //                                    load_spread.Sheets[0].Cells[0, 9].BackColor = Color.Green;
    //                                }
    //                                else
    //                                {
    //                                    int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                                    load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                                    load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (ddlreporttype.SelectedIndex == 2)
    //                                {
    //                                    load_spread.Sheets[0].SpanModel.Add(startrow, 9, endrow, 1);
    //                                    if (load_spread.Sheets[0].RowCount != startrow)
    //                                    {
    //                                        int value = Convert.ToInt32(totalhr) - Convert.ToInt32(combinehour);
    //                                        load_spread.Sheets[0].Cells[startrow, 9].Text = Convert.ToString(value);
    //                                        load_spread.Sheets[0].Cells[startrow, 9].BackColor = Color.Green;
    //                                    }
    //                                    else
    //                                    {
    //                                        int value = Convert.ToInt32(totalhr) - Convert.ToInt32(combinehour);
    //                                        load_spread.Sheets[0].Cells[startrow - 1, 9].Text = Convert.ToString(value);
    //                                        load_spread.Sheets[0].Cells[startrow - 1, 9].BackColor = Color.Green;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    load_spread.Sheets[0].SpanModel.Add(startrow, 9, endrow, 1);
    //                                    if (load_spread.Sheets[0].RowCount != startrow)
    //                                    {
    //                                        load_spread.Sheets[0].Cells[startrow, 9].Text = Convert.ToString(totalhr);
    //                                    }
    //                                    else
    //                                    {
    //                                        load_spread.Sheets[0].Cells[startrow - 1, 9].Text = Convert.ToString(totalhr);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (startrow == 0)
    //                        {
    //                            int rowcnt = Convert.ToInt16(load_spread.Sheets[0].RowCount);
    //                            load_spread.Sheets[0].SpanModel.Add(0, 9, rowcnt, 1);
    //                            load_spread.Sheets[0].Cells[0, 9].Text = Convert.ToString(totalhr);
    //                        }
    //                        else
    //                        {
    //                            load_spread.Sheets[0].SpanModel.Add(startrow, 9, endrow, 1);
    //                            if (load_spread.Sheets[0].RowCount != startrow)
    //                            {
    //                                load_spread.Sheets[0].Cells[startrow, 9].Text = Convert.ToString(totalhr);
    //                                alltotalvalues.Add(startrow, totalhr);
    //                            }
    //                            else
    //                            {
    //                                load_spread.Sheets[0].Cells[startrow - 1, 9].Text = Convert.ToString(totalhr);
    //                            }
    //                        }
    //                    }
    //                    startrow = load_spread.Sheets[0].RowCount;
    //                }
    //                totalhr = 0;
    //                combinehour = 0;
    //            }
    //            totalhr = 0;
    //            combinehour = 0;
    //        }
    //    }
    //    if (ddlreporttype.SelectedIndex==2 && cbcombine.Checked==false && deptddl.SelectedItem.Text == "All" && desigddl.SelectedItem.Text == "All" && stafftxt.SelectedItem.Text == "All" && subjddl.SelectedItem.Text == "All")
    //    {
    //        if (alltotalvalues.Count>0)
    //        {
    //            foreach (DictionaryEntry entry in alltotalvalues)
    //            {
    //                load_spread.Sheets[0].Cells[Convert.ToInt32(entry.Key), 9].Text = Convert.ToString(entry.Value);
    //            }
    //        }
    //    }
    //    if (load_spread.Sheets[0].RowCount > 0)
    //    {
    //        chk_sms.Visible = false;
    //        chk_mail.Visible = false;
    //        txt_message.Visible = false;
    //        btnsms.Visible = false;
    //        load_spread.Height = 500;
    //        load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
    //        load_spread.Width = 946;
    //        load_spread.Visible = true;
    //        btnprintmaster.Visible = true;
    //        lblrptname.Visible = true;
    //        txtexcelname.Visible = true;
    //        btnxl.Visible = true;
    //    }
    //    else
    //    {
    //        chk_sms.Visible = false;
    //        chk_mail.Visible = false;
    //        txt_message.Visible = false;
    //        btnsms.Visible = false;
    //        load_spread.Visible = false;
    //        errlbl.Text = "No Records are found";
    //        errlbl.Visible = true;
    //    }
    //}

    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    //Added by srinath 3/04/2014
    DataSet ds = new DataSet();
    public void BindBatch()
    {
        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "Batch_year";
            ddlbatch.DataValueField = "Batch_year";
            ddlbatch.DataBind();
            ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = ddlcollege.SelectedValue.ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree();
        bindbranch();
        load_spread.Sheets[0].RowHeader.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        load_spread.Sheets[0].RowHeader.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_spread.Sheets[0].RowHeader.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
    }

    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";";
            string holday = dac.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            string leave = dac.GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
            if (leave != null && leave != "0")
            {
                dif_days = dif_days + 1;
            }
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            //-----------------------------------------------------------     
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------
            if (order.ToString() == "0")
            {
                order = Convert.ToInt32(no_days);
            }
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(load_spread, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void loadclassrepor()
    {
        try
        {
            Boolean resulflag = false;
            load_spread.Sheets[0].RowHeader.Visible = false;
            load_spread.Sheets[0].ColumnCount = 0;
            load_spread.Sheets[0].ColumnCount = 6;
            load_spread.Sheets[0].RowCount = 0;
            load_spread.Sheets[0].ColumnHeader.RowCount = 1;
            load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Actual Periods";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Conducted Periods";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Missing Periods";
            load_spread.Sheets[0].Columns[0].Width = 50;
            load_spread.Sheets[0].Columns[1].Width = 295;
            load_spread.Sheets[0].Columns[2].Width = 295;
            load_spread.Sheets[0].Columns[3].Width = 100;
            load_spread.Sheets[0].Columns[4].Width = 100;
            load_spread.Sheets[0].Columns[5].Width = 100;
            load_spread.Sheets[0].Columns[0].Locked = true;
            load_spread.Sheets[0].Columns[1].Locked = true;
            load_spread.Sheets[0].Columns[2].Locked = true;
            load_spread.Sheets[0].Columns[3].Locked = true;
            load_spread.Sheets[0].Columns[4].Locked = true;
            load_spread.Sheets[0].Columns[5].Locked = true;
            string degeree = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            date2 = txtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '/' });
            dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            string bin_semester = string.Empty;
            DateTime dtfrom = Convert.ToDateTime(datefrom);
            DateTime dtto = Convert.ToDateTime(dateto);
            if (dtfrom <= dtto)
            {
                string getsection_sem = "select distinct current_semester from registration where batch_year in(" + batch + ") and degree_code in(" + degeree + ")  and delflag=0 and exam_flag<>'Debar'";
                // DataSet dssec_sem = d2.select_method(getsection_sem, hat, "Text");
                DataSet dssec_sem = d2.select_method_wo_parameter(getsection_sem, "Text");//=================Modified by Venkat ======================
                if (dssec_sem.Tables[0].Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dssec_sem.Tables[0].Rows.Count; cnt++)
                    {
                        if (bin_semester == "")
                        {
                            bin_semester = dssec_sem.Tables[0].Rows[cnt]["current_semester"].ToString();
                        }
                        else
                        {
                            bin_semester = bin_semester + ',' + dssec_sem.Tables[0].Rows[cnt]["current_semester"].ToString();
                        }
                    }
                }
                if (bin_semester != "")
                {
                    string bindvals21 = "select distinct s.degree_code,s.semester,s.batch_year ,s.start_date,s.end_date,no_of_hrs_II_half_day as sch,no_of_hrs_I_half_day as fih,No_of_hrs_per_day as nohrs  from seminfo s,Registration r,PeriodAttndSchedule p  where  r.Batch_Year=s.batch_year and r.Current_Semester=s.semester and r.degree_code=s.degree_code   and r.CC=0 and r.DelFlag=0 and s.semester in(" + bin_semester + ") and s.batch_year in (" + batch + ")   and s.degree_code in (" + degeree + ") and p.degree_code=s.degree_code and p.semester=s.semester   order by s.batch_year,s.semester, s.degree_code";
                    // DataSet dsbindvalues = d2.select_method(bindvals21, hat, "Text");
                    DataSet dsbindvalues = d2.select_method_wo_parameter(bindvals21, "Text");//=================Modified by Venkat ======================
                    if (dsbindvalues.Tables[0].Rows.Count > 0)
                    {
                        string subjectquery = string.Empty;
                        for (int cnt = 0; cnt < dsbindvalues.Tables[0].Rows.Count; cnt++)
                        {
                            string strsection = string.Empty;
                            DateTime s_date = Convert.ToDateTime(dsbindvalues.Tables[0].Rows[cnt]["start_date"]);
                            DateTime e_date = Convert.ToDateTime(dsbindvalues.Tables[0].Rows[cnt]["end_date"]);
                            string dcode = dsbindvalues.Tables[0].Rows[cnt]["degree_code"].ToString();
                            string sem = dsbindvalues.Tables[0].Rows[cnt]["semester"].ToString();
                            string batchyear = dsbindvalues.Tables[0].Rows[cnt]["batch_year"].ToString();
                            string nhr = dsbindvalues.Tables[0].Rows[cnt]["nohrs"].ToString();
                            string fis = dsbindvalues.Tables[0].Rows[cnt]["fih"].ToString();
                            string sehs = dsbindvalues.Tables[0].Rows[cnt]["sch"].ToString();
                            int noofhrs = 0, fsh = 0, shfs = 0;
                            if (nhr.Trim() != "" && nhr != null)
                            {
                                noofhrs = Convert.ToInt32(nhr);
                            }
                            if (fis.Trim() != "" && fis != null)
                            {
                                fsh = Convert.ToInt32(fis);
                            }
                            if (sehs.Trim() != "" && sehs != null)
                            {
                                shfs = Convert.ToInt32(sehs);
                            }
                            hat.Clear();
                            string deaptname = d2.GetFunction("select c.Course_Name+'-'+de.Dept_Name from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code=" + dcode + "");
                            DataSet dssection = d2.BindSectionDetail(batchyear, dcode);
                            if (dssection.Tables[0].Rows.Count > 0)
                            {
                                for (int sec = 0; sec < dssection.Tables[0].Rows.Count; sec++)
                                {
                                    if (strsection == "")
                                    {
                                        strsection = dssection.Tables[0].Rows[sec]["sections"].ToString();
                                    }
                                    else
                                    {
                                        strsection = strsection + '\\' + dssection.Tables[0].Rows[sec]["sections"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                strsection = string.Empty;
                            }
                            int sno = 0;
                            string[] sectionspilt = strsection.Split('\\');
                            for (int scet = 0; scet <= sectionspilt.GetUpperBound(0); scet++)
                            {
                                string chksectionvalue = sectionspilt[scet].ToString();
                                Boolean head = false;
                                string sectionvalue = string.Empty;
                                load_spread.Sheets[0].RowCount++;
                                if (chksectionvalue == "")
                                {
                                    sectionvalue = string.Empty;
                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = batchyear + '-' + deaptname + '-' + sem;
                                }
                                else
                                {
                                    load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = batchyear + '-' + deaptname + '-' + sem + '-' + chksectionvalue;
                                    sectionvalue = " and Sections='" + chksectionvalue.ToString() + "'";
                                }
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                load_spread.Sheets[0].SpanModel.Add(load_spread.Sheets[0].RowCount - 1, 0, 1, 5);
                                hat.Clear();
                                Hashtable hatsubact = new Hashtable();
                                Hashtable hatsubcon = new Hashtable();
                                Hashtable hatmisub = new Hashtable();
                                string staffquery = "select distinct S.subject_no,subject_code,subject_name,st.staff_code,ss.staff_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st,staffmaster ss where S.subject_no=SC.Subject_no  and st.staff_code=ss.staff_code and  s.syll_code=SM.syll_code  and st.subject_no=s.subject_no   and S.subtype_no = Sem.subtype_no  and SM.degree_code=" + dcode + " and  SM.batch_year=" + batchyear + " and SM.semester=" + sem + " " + sectionvalue + " order by subject_code";
                                //  DataSet dsstaff = d2.select_method(staffquery, hat, "Text");
                                DataSet dsstaff = d2.select_method_wo_parameter(staffquery, "Text");//===================Modified by Venkat===================
                                string tempsubcode = string.Empty;
                                string staffname = string.Empty;
                                int startrow = load_spread.Sheets[0].RowCount - 1;
                                for (int i = 1; i < dsstaff.Tables[0].Rows.Count; i++)
                                {
                                    string subjetname = dsstaff.Tables[0].Rows[i]["subject_name"].ToString();
                                    string subjectcode = dsstaff.Tables[0].Rows[i]["subject_no"].ToString();
                                    if (tempsubcode != subjectcode)
                                    {
                                        staffname = dsstaff.Tables[0].Rows[i]["staff_name"].ToString();
                                        tempsubcode = subjectcode;
                                        load_spread.Sheets[0].RowCount++;
                                        sno++;
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 1].Text = subjetname.ToString();
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 1].Tag = subjectcode.ToString();
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 2].Text = staffname.ToString();

                                        if (!hatsubact.Contains(subjectcode))
                                        {
                                            hatsubact.Add(subjectcode, 0);
                                            hatsubcon.Add(subjectcode, 0);
                                            hatmisub.Add(subjectcode, 0);
                                        }
                                    }
                                    else
                                    {
                                        staffname = staffname + " , " + dsstaff.Tables[0].Rows[i]["staff_name"].ToString();
                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 2].Text = staffname.ToString();
                                    }
                                    load_spread.Visible = true;
                                }



                                for (DateTime caldate = dtfrom; caldate <= dtto; caldate = caldate.AddDays(1))
                                {
                                    if (caldate >= s_date && caldate <= e_date)
                                    {
                                        string schorder = "", noofdays = "", start_datesem = "", end_datesem = string.Empty;
                                        string dayorderquery = "select convert(nvarchar(15),s.start_date,101) as start_date,s.end_date,s.starting_dayorder,p.nodays,p.schorder from periodattndschedule p,seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + dcode + " and s.semester=" + sem + " and batch_year=" + batchyear + "";
                                        //  DataSet dsdayorder = d2.select_method(dayorderquery, hat, "Text");
                                        DataSet dsdayorder = d2.select_method_wo_parameter(dayorderquery, "Text");//==================Modified By Venkat===========================
                                        if (dsdayorder.Tables[0].Rows.Count > 0)
                                        {
                                            schorder = dsdayorder.Tables[0].Rows[0]["SchOrder"].ToString();
                                            noofdays = dsdayorder.Tables[0].Rows[0]["nodays"].ToString();
                                            start_datesem = dsdayorder.Tables[0].Rows[0]["start_date"].ToString();
                                            end_datesem = dsdayorder.Tables[0].Rows[0]["end_date"].ToString();
                                            start_dayorder = dsdayorder.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                        }
                                        string dayget = string.Empty;
                                        if (schorder == "1")
                                        {
                                            dayget = Convert.ToString(caldate.ToString("ddd"));
                                        }
                                        else
                                        {
                                            string[] startdatspilt = start_datesem.Split(' ');
                                            start_datesem = startdatspilt[0].ToString();
                                            dayget = findday(caldate.ToString("MM/dd/yyyy"), start_datesem.ToString(), noofdays.ToString(), start_dayorder.ToString());
                                        }
                                        string classhour = string.Empty;
                                        string holidayqery = "select halforfull,morning,evening from holidayStudents where degree_code=" + dcode + " and semester=" + sem + " and holiday_date='" + caldate.ToString() + "'";
                                        DataSet dsholiady = d2.select_method_wo_parameter(holidayqery, "Text");
                                        int stcol = 1, nocol = 0;
                                        if (dsholiady.Tables[0].Rows.Count > 0)
                                        {
                                            string halforfull = dsholiady.Tables[0].Rows[0]["halforfull"].ToString();
                                            string morin = dsholiady.Tables[0].Rows[0]["morning"].ToString();
                                            string even = dsholiady.Tables[0].Rows[0]["evening"].ToString();
                                            if (halforfull.Trim().ToString().ToLower() == "false" || halforfull.Trim().ToString().ToLower() == "0")
                                            {
                                                halforfull = "0";
                                                stcol = noofhrs + 2;
                                                nocol = 1;
                                            }
                                            else
                                            {
                                                halforfull = "1";
                                            }
                                            if (morin.Trim().ToString().ToLower() == "true" || morin.Trim().ToString().ToLower() == "1")
                                            {
                                                morin = "1";
                                            }
                                            else
                                            {
                                                morin = "0";
                                            }
                                            if (even.Trim().ToString().ToLower() == "true" || even.Trim().ToString().ToLower() == "1")
                                            {
                                                even = "1";
                                            }
                                            else
                                            {
                                                even = "0";
                                            }
                                            if (halforfull == "1" && morin == "1")
                                            {
                                                nocol = noofhrs;
                                                stcol = fsh + 1;
                                            }
                                            if (halforfull == "1" && morin == "1")
                                            {
                                                nocol = fsh;
                                                stcol = 1;
                                            }
                                        }
                                        else
                                        {
                                            stcol = 1;
                                            nocol = noofhrs;
                                        }
                                        for (int i = stcol; i <= nocol; i++)
                                        {
                                            if (classhour == "")
                                            {
                                                classhour = classhour + dayget + i;
                                            }
                                            else
                                            {
                                                classhour = classhour + ',' + dayget + i;
                                            }
                                        }
                                        if (classhour.Trim() != "")
                                        {
                                            string altershedulequery = "select " + classhour + " from Alternate_schedule where batch_year=" + batchyear + " and degree_code=" + dcode + " and semester=" + sem + " " + sectionvalue + " and fromdate= '" + caldate.ToString() + "'";
                                            // DataSet dsaltershudel = d2.select_method(altershedulequery, hat, "Text");
                                            DataSet dsaltershudel = d2.select_method_wo_parameter(altershedulequery, "Text");//=============================Modified By Venkat===================
                                            string shedulequery = "select top 1 " + classhour + ",batch_year,degree_code,semester,sections,ttname from semester_schedule where batch_year=" + batchyear + " and degree_code=" + dcode + " and semester=" + sem + " " + sectionvalue + " and fromdate <= '" + caldate.ToString() + "'  order by fromdate desc";
                                            ds.Dispose();
                                            ds.Reset();
                                            // ds = d2.select_method(shedulequery, hat, "Text");
                                            ds = d2.select_method_wo_parameter(shedulequery, "Text");//=============================Modified By Venkat===================
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    string[] classhourspilt = classhour.Split(new char[] { ',' });
                                                    for (int colu = 0; colu <= classhourspilt.GetUpperBound(0); colu++)
                                                    {
                                                        string ttname = string.Empty;
                                                        string columnvalue = classhourspilt[colu].ToString();
                                                        string classhour1 = string.Empty;
                                                        Boolean alternatelab = false;
                                                        if (dsaltershudel.Tables[0].Rows.Count > 0 && dsaltershudel.Tables[0].Rows[0]["" + columnvalue + ""].ToString() != null && dsaltershudel.Tables[0].Rows[0]["" + columnvalue + ""].ToString().Trim() != "")
                                                        {
                                                            classhour1 = dsaltershudel.Tables[0].Rows[0]["" + columnvalue + ""].ToString();
                                                            alternatelab = true;
                                                        }
                                                        else
                                                        {
                                                            alternatelab = false;
                                                            classhour1 = ds.Tables[0].Rows[i]["" + columnvalue + ""].ToString();
                                                            ttname = ds.Tables[0].Rows[i]["ttname"].ToString();
                                                            if (ttname.Trim() != "" && ttname != null)
                                                            {
                                                                ttname = " and Timetablename='" + ttname + "'";
                                                            }
                                                        }
                                                        if (classhour1.ToString().Trim() != "")
                                                        {
                                                            string[] splitcode = classhour1.Split(';');
                                                            for (int k = 0; k <= splitcode.GetUpperBound(0); k++)
                                                            {
                                                                string getsphr = splitcode[k].ToString();
                                                                string[] spgetsphr = getsphr.Split('-');
                                                                if (spgetsphr.GetUpperBound(0) > 1)
                                                                {
                                                                    string subject = spgetsphr[0].ToString();





                                                                    if (hatsubact.Contains(subject))
                                                                    {
                                                                        int gethour = Convert.ToInt32(hatsubact[subject]);
                                                                        gethour++;
                                                                        hatsubact[subject] = gethour;
                                                                        resulflag = true;
                                                                        string hr = columnvalue[3].ToString();
                                                                        string Att_strqueryst1 = "0";
                                                                        string[] spiltdate = caldate.ToString("d/MM/yyyy").Split('/');
                                                                        long strdate = (Convert.ToInt32(spiltdate[1]) + Convert.ToInt32(spiltdate[2]) * 12);
                                                                        string Att_dcolumn1 = "d" + spiltdate[0] + "d" + hr;
                                                                        string check_lab = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject + "'");//Modified by srinath 7/1/2013
                                                                        if (check_lab == "0" || check_lab.Trim().ToLower() == "false")//Modified by srinath 7/1/2013
                                                                        {
                                                                            if (chksectionvalue == "-1" || chksectionvalue == "" || chksectionvalue == null)
                                                                            {
                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + subject + "' and degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' ");
                                                                                if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + subject + "' and degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                        gethour++;
                                                                                        hatsubcon[subject] = gethour;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                    gethour++;
                                                                                    hatmisub[subject] = gethour;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + subject + "' and degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "'");
                                                                                if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + subject + "' and degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                        gethour++;
                                                                                        hatsubcon[subject] = gethour;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                    gethour++;
                                                                                    hatmisub[subject] = gethour;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (chksectionvalue == "-1" || chksectionvalue == "" || chksectionvalue == null)
                                                                            {
                                                                                if (alternatelab == true)
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                    {
                                                                                        Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='') and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                        if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                            gethour++;
                                                                                            hatmisub[subject] = gethour;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                            gethour++;
                                                                                            hatsubcon[subject] = gethour;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' " + ttname + " and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                    {
                                                                                        Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='') and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' " + ttname + " and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                        if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                            gethour++;
                                                                                            hatsubcon[subject] = gethour;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                            gethour++;
                                                                                            hatmisub[subject] = gethour;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (alternatelab == true)
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                    {
                                                                                        Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')=LTRIM(rtrim(isnull(registration.sections,'') and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester  and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')    ");
                                                                                        if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                            gethour++;
                                                                                            hatsubcon[subject] = gethour;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                            gethour++;
                                                                                            hatmisub[subject] = gethour;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' " + ttname + " and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                    if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                    {
                                                                                        Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dcode + "' and current_semester=" + sem + " and batch_year=" + batchyear + " and sections='" + chksectionvalue + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + subject + "' and batch_year=registration.batch_year and day_value='" + dayget + "' " + ttname + " and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester  and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')  ");
                                                                                        if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatsubcon[subject]);
                                                                                            gethour++;
                                                                                            hatsubcon[subject] = gethour;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                            gethour++;
                                                                                            hatmisub[subject] = gethour;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        gethour = Convert.ToInt32(hatmisub[subject]);
                                                                                        gethour++;
                                                                                        hatmisub[subject] = gethour;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }






                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Boolean hourflag = false;
                                int conhrs = 0, acthrs = 0, mishrs = 0;
                                for (int i = startrow + 1; i < load_spread.Sheets[0].RowCount; i++)
                                {
                                    subject_name = load_spread.Sheets[0].Cells[i, 1].Tag.ToString();
                                    if (hatsubact.Contains(subject_name))
                                    {
                                        string gethours = hatsubact[subject_name].ToString();
                                        load_spread.Sheets[0].Cells[i, 3].Text = gethours;
                                        load_spread.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Center;
                                        conhrs = conhrs + Convert.ToInt32(gethours);
                                    }
                                    else
                                    {
                                        load_spread.Sheets[0].Cells[i, 3].Text = "0";
                                    }
                                    if (hatsubcon.Contains(subject_name))
                                    {
                                        string gethours = hatsubcon[subject_name].ToString();
                                        load_spread.Sheets[0].Cells[i, 4].Text = gethours;
                                        load_spread.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                                        acthrs = acthrs + Convert.ToInt32(gethours);
                                    }
                                    else
                                    {
                                        load_spread.Sheets[0].Cells[i, 4].Text = "0";
                                    }
                                    if (hatmisub.Contains(subject_name))
                                    {
                                        string gethours = hatmisub[subject_name].ToString();
                                        load_spread.Sheets[0].Cells[i, 5].Text = gethours;
                                        load_spread.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;
                                        mishrs = mishrs + Convert.ToInt32(gethours);
                                    }
                                    else
                                    {
                                        load_spread.Sheets[0].Cells[i, 5].Text = "0";
                                    }
                                }
                                load_spread.Sheets[0].RowCount++;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = "Total";
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                load_spread.Sheets[0].SpanModel.Add(load_spread.Sheets[0].RowCount - 1, 0, 1, 3);
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 3].Text = conhrs.ToString();
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Text = acthrs.ToString();
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 5].Text = mishrs.ToString();
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
            load_spread.Height = 600;
            load_spread.Width = 950;
            load_spread.SaveChanges();
            if (resulflag == true)
            {
                load_spread.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
            }
            else
            {
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    public void loadstaffworkloadwithexp()
    {
        try
        {
            DataSet qualds = new DataSet();
            string Qualification = string.Empty;
            Boolean visibleflag = false;
            load_spread.Sheets[0].AutoPostBack = true;
            load_spread.Sheets[0].ColumnHeader.RowCount = 0;
            load_spread.Sheets[0].RowCount = 0;
            load_spread.Sheets[0].ColumnCount = 0;
            load_spread.Sheets[0].ColumnHeader.RowCount = 2;
            load_spread.Sheets[0].ColumnCount = 16;
            load_spread.Sheets[0].RowHeader.Visible = false;
            load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Qualification & Specialization";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Desigantion And Teaching Experience";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Preferred";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Allotted By HOD";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Semester";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Branch & Section";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Subject Code";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Subject Name";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 10].Text = "No Of Students";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "No Of Hours";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Theory";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Lab";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 13].Text = "Tutorial";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 14].Text = "TOTAL";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Sign Of Faculty";
            load_spread.Sheets[0].Columns[0].Width = 50;
            load_spread.Sheets[0].Columns[1].Width = 150;
            load_spread.Sheets[0].Columns[2].Width = 200;
            load_spread.Sheets[0].Columns[3].Width = 175;
            load_spread.Sheets[0].Columns[4].Width = 175;
            load_spread.Sheets[0].Columns[5].Width = 450;
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 5);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 4);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 15, 2, 1);
            load_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(10, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(11, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(12, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(13, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(14, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].Columns[0].Locked = true;
            load_spread.Sheets[0].Columns[1].Locked = true;
            load_spread.Sheets[0].Columns[2].Locked = true;
            load_spread.Sheets[0].Columns[3].Locked = true;
            load_spread.Sheets[0].Columns[4].Locked = true;
            load_spread.Sheets[0].Columns[5].Locked = true;
            load_spread.Sheets[0].Columns[6].Locked = true;
            load_spread.Sheets[0].Columns[7].Locked = true;
            load_spread.Sheets[0].Columns[8].Locked = true;
            load_spread.Sheets[0].Columns[9].Locked = true;
            load_spread.Sheets[0].Columns[10].Locked = true;
            load_spread.Sheets[0].Columns[11].Locked = true;
            load_spread.Sheets[0].Columns[12].Locked = true;
            load_spread.Sheets[0].Columns[13].Locked = true;
            load_spread.Sheets[0].Columns[14].Locked = true;
            load_spread.Sheets[0].Columns[15].Locked = true;
            load_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
            string Strsql = string.Empty;
            DataSet dsgetvalue = new DataSet();
            string SqlFinal = string.Empty;
            string sql1 = string.Empty;
            string tmp_camprevar = string.Empty;
            string cur_camprevar = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();
            DataSet ds_attndmaster = new DataSet();
            Hashtable ht_sch = new Hashtable();
            Hashtable ht_sdate = new Hashtable();
            Hashtable ht_bell = new Hashtable();
            Hashtable ht_period = new Hashtable();
            string degreename = string.Empty;
            Hashtable hatdegreename = new Hashtable();
            string degree_var = string.Empty;
            int noofhrs = 0;
            string vari = string.Empty;
            ht_sch.Clear();
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = dac.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_sdate.Clear();
            if (ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_bell.Clear();
            if (ds_attndmaster.Tables[2].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);
                    if (!ht_bell.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                        ht_bell.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_period.Clear();
            if (ds_attndmaster.Tables[3].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);
                    if (!ht_period.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                        ht_period.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
            {
                if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                {
                    hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                }
            }
            string getalldetails = "select * from Alternate_Schedule ; ";
            getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
            getalldetails = getalldetails + "Select * from holidaystudents ; ";
            getalldetails = getalldetails + "select * from seminfo ; ";
            getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab,sm.promote_count from syllabus_master sy,sub_sem sm,subject s,Registration r where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and r.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and  sy.semester=r.Current_Semester and r.college_code='" + Session["collegecode"].ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester ;";
            getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
            getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,degree_code,semester,No_of_hrs_per_day as tot from periodattndschedule";
            DataSet dsall = dac.select_method_wo_parameter(getalldetails, "Text");
            if (dsall.Tables[0].Rows.Count > 0)
            {
            }
            else
            {
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Records Found";
            }
            string deptcode = string.Empty;
            string designcode = string.Empty;
            string staffcode = string.Empty;
            if (deptddl.SelectedItem.ToString() != "All" && deptddl.SelectedItem.ToString() != "")
            {
                deptcode = " and st.dept_code='" + deptddl.SelectedValue.ToString() + "'";
            }
            if (desigddl.SelectedItem.ToString() != "All" && desigddl.SelectedItem.ToString() != "")
            {
                designcode = " and st.desig_code='" + desigddl.SelectedValue.ToString() + "'";
            }
            if (stafftxt.Items.Count > 0)
            {
                if (stafftxt.SelectedItem.ToString() != "All" && stafftxt.SelectedItem.ToString() != "")
                {
                    staffcode = " and s.staff_code='" + stafftxt.SelectedValue.ToString() + "'";
                }
            }
            string tempdegree = string.Empty;
            string Designame = string.Empty;
            string Experience = string.Empty;
            string SubjectCode = string.Empty;
            string SubjectName = string.Empty;
            string noofstudents = string.Empty;
            int tut = 0;
            int srno = 0;
            string staffquery = "select s.staff_code,s.staff_name,st.dept_code,st.desig_code,h.dept_name,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=st.staff_code and s.resign=0 and s.settled=0 and st.latestrec=1 and st.dept_code=h.dept_code and st.desig_code=d.desig_code " + deptcode + " " + designcode + " " + staffcode + " and s.college_code=d.collegeCode and s.college_code=h.college_code order by st.dept_code,st.desig_code,s.staff_name";
            DataSet dsstaffvalues = dac.select_method_wo_parameter(staffquery, "Text");
            if (dsstaffvalues.Tables[0].Rows.Count > 0)
            {
                for (int st = 0; st < dsstaffvalues.Tables[0].Rows.Count; st++)
                {
                    staff_code = dsstaffvalues.Tables[0].Rows[st]["staff_code"].ToString();
                    Designame = dsstaffvalues.Tables[0].Rows[st]["desig_name"].ToString();
                    if (staff_code.Trim() != "")
                    {
                        string qualquery = "select sm.staff_code,sa.qualification,Convert(nvarchar(20), sm.join_date,103 ) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sa.college_code=sm.college_code and sm.staff_code='" + staff_code + "'";
                        qualquery = qualquery + " select ss.staff_code,s.subject_name,s.subject_code,s.subject_no from staff_selector ss,subject s,sub_sem sm,syllabus_master sy,staffmaster st where sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no and s.subject_no=ss.subject_no and ss.staff_code=st.staff_code and st.staff_code='" + staff_code + "' order by ss.staff_code,s.subject_name,s.subject_code ";
                        qualds = d2.select_method_wo_parameter(qualquery, "Text");
                        if (qualds.Tables[0].Rows.Count > 0)
                        {
                            string qual = qualds.Tables[0].Rows[0]["qualification"].ToString();
                            if (qual.Trim() != "")
                            {
                                string[] qn = qual.Split('\\');
                                if (qn.Length > 0)
                                {
                                    for (int i = 0; i < qn.GetUpperBound(0); i++)
                                    {
                                        string[] qf = qn[i].Split(';');
                                        if (qf.Length > 0)
                                        {
                                            Qualification = qf[2].ToString();
                                        }
                                    }
                                }
                            }
                            string jdate = qualds.Tables[0].Rows[0]["join_date"].ToString();
                            if (jdate.Trim() != "")
                            {
                                string[] jdt = jdate.Split('/');
                                DateTime dt = Convert.ToDateTime(jdt[1] + "/" + jdt[0] + "/" + jdt[2]);
                                DateTime dt1 = DateTime.Now;
                                int yr = dt1.Year - dt.Year;
                                int mnt = dt1.Month - dt.Month;
                                if (mnt > 0)
                                {
                                }
                                else
                                {
                                    yr = yr - 1;
                                    mnt = dt.Month;
                                }
                                Experience = Convert.ToString(yr) + "." + Convert.ToString(mnt) + "Yrs";
                            }
                            if (qualds.Tables[1].Rows.Count > 0)
                            {
                                SubjectCode = qualds.Tables[1].Rows[0]["subject_code"].ToString();
                                SubjectName = qualds.Tables[1].Rows[0]["subject_name"].ToString();
                            }
                        }
                    }
                    string stafcode = dsstaffvalues.Tables[0].Rows[st]["staff_code"].ToString();
                    SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date,si.start_date from staff_selector ss,Registration r,";
                    SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                    SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                    SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and  LTRIM(rtrim(isnull(r.sections,'')))=LTRIM(rtrim(isnull(ss.sections,''))) and ss.batch_year=r.Batch_Year";
                    SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                    SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                    SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "'";
                    DataSet dsperiod = dac.select_method(SqlFinal, hat, "Text");
                    string strsction = string.Empty;
                    Hashtable hatholiday = new Hashtable();
                    DataView dvalternaet = new DataView();
                    DataView dvsemster = new DataView();
                    DataView dvholiday = new DataView();
                    DataView dvdaily = new DataView();
                    DataView dvsubject = new DataView();
                    DataView dvsublab = new DataView();
                    string start_datesem = string.Empty;
                    string noofdays = string.Empty;
                    string Day_Order = string.Empty;
                    string tmp_datevalue = string.Empty;
                    string lasubno = string.Empty;
                    if (dsperiod.Tables[0].Rows.Count > 0)
                    {
                        srno++;
                        int totalhours = 0;
                        int totaltheohours = 0;
                        int totallabhours = 0;
                        int spanrow = 0;
                        int finalstartrow = load_spread.Sheets[0].RowCount;
                        for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                        {
                            Hashtable hatsubject = new Hashtable();
                            int startrow = load_spread.Sheets[0].RowCount;
                            cur_camprevar = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                            string getdate = string.Empty;
                            if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                            {
                                if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                {
                                    degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                }
                                strsction = string.Empty;
                                if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                {
                                    strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                }
                                dsall.Tables[4].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                DataView dtcurlab = dsall.Tables[4].DefaultView;
                                Hashtable hatcurlab = new Hashtable();
                                for (int cula = 0; cula < dtcurlab.Count; cula++)
                                {
                                    lasubno = dtcurlab[cula]["subject_no"].ToString();
                                    string labhour = dtcurlab[cula]["lab"].ToString();
                                    if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                    {
                                        if (!hatcurlab.Contains(lasubno))
                                        {
                                            hatcurlab.Add(lasubno, lasubno);
                                        }
                                    }
                                }
                                hatholiday.Clear();
                                dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                DataView duholiday = dsall.Tables[2].DefaultView;
                                for (int i = 0; i < duholiday.Count; i++)
                                {
                                    if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                    {
                                        hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                    }
                                }
                                int frshlf = 0, schlf = 0;
                                dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                DataView dvperiod = dsall.Tables[6].DefaultView;
                                if (dvperiod.Count > 0)
                                {
                                    string morhr = dvperiod[0]["mor"].ToString();
                                    string evehr = dvperiod[0]["mor"].ToString();
                                    if (morhr != null && morhr.Trim() != "")
                                    {
                                        frshlf = Convert.ToInt32(morhr);
                                        noofhrs = Convert.ToInt32(dvperiod[0]["tot"].ToString());
                                    }
                                    if (evehr != null && evehr.Trim() != "")
                                    {
                                        schlf = Convert.ToInt32(evehr);
                                    }
                                }
                                string getcurrent_sem = string.Empty;
                                dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                DataView dvcurrsem = dsall.Tables[5].DefaultView;
                                if (dvcurrsem.Count > 0)
                                {
                                    getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                }
                                if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                {
                                    string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                    string altersetion = string.Empty;
                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                    {
                                        altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                    }
                                    dsall.Tables[3].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                    DataView dvsemdate = dsall.Tables[3].DefaultView;
                                    if (dvsemdate.Count > 0)
                                    {
                                        //DateTime dtendate = Convert.ToDateTime(dvsemdate[0]["end_date"].ToString());
                                        //DateTime dtstartdate = Convert.ToDateTime(dvsemdate[0]["start_date"].ToString());
                                        string fromdate = txtFromDate.Text;
                                        string todate = txtToDate.Text;
                                        string[] split = fromdate.Split('/');
                                        string[] tdt = todate.Split('/');
                                        DateTime dtendate = Convert.ToDateTime(tdt[1] + "/" + tdt[0] + "/" + tdt[2]);
                                        DateTime dtstartdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                        for (DateTime dtn = dtstartdate; dtn <= dtendate; dtn = dtn.AddDays(1))
                                        {
                                            DateTime cur_day = dtn;
                                            tmp_datevalue = Convert.ToString(cur_day);
                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                            string SchOrder = string.Empty;
                                            string day_from = cur_day.ToString("yyyy-MM-dd");
                                            DateTime schfromdate = cur_day;
                                            dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                            dvsemster = dsall.Tables[1].DefaultView;
                                            if (dvsemster.Count > 0)
                                            {
                                                getdate = dvsemster[0]["FromDate"].ToString();
                                            }
                                            else
                                            {
                                                getdate = string.Empty;
                                            }
                                            if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                            {
                                                DateTime getsche = Convert.ToDateTime(getdate);
                                                if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                {
                                                    if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                    {
                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                        string[] sp_rd_semi = contvar.Split(',');
                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                        {
                                                            SchOrder = sp_rd_semi[0].ToString();
                                                            noofdays = sp_rd_semi[1].ToString();
                                                        }
                                                    }
                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                    if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                    {
                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                        string[] sp_rd_semi = contvar.Split(',');
                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                        {
                                                            start_datesem = sp_rd_semi[0].ToString();
                                                            start_dayorder = sp_rd_semi[1].ToString();
                                                        }
                                                    }
                                                    Day_Order = string.Empty;
                                                    if (SchOrder == "1")
                                                    {
                                                        strday = cur_day.ToString("ddd");
                                                        Day_Order = "0-" + Convert.ToString(strday);
                                                    }
                                                    else
                                                    {
                                                        strday = findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                    }
                                                    string reasonsun = string.Empty;
                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                    {
                                                        reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                        if (reasonsun.Trim().ToLower() == "sunday")
                                                        {
                                                        }
                                                    }
                                                    if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                    {
                                                        string str_day = strday;
                                                        string Atmonth = cur_day.Month.ToString();
                                                        string Atyear = cur_day.Year.ToString();
                                                        long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                        sql1 = string.Empty;
                                                        Strsql = string.Empty;
                                                        for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                        {
                                                            Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                            if (sql1 == "")
                                                            {
                                                                sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                            }
                                                            else
                                                            {
                                                                sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                            }
                                                        }
                                                        string day_aten = cur_day.Day.ToString();
                                                        Boolean check_hour = false;
                                                        string strsectionvar = string.Empty;
                                                        string labsection = string.Empty;
                                                        if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                        {
                                                            strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                            labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                        }
                                                        sql1 = " and (" + sql1 + ")";
                                                        dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                        dvalternaet = dsall.Tables[0].DefaultView;
                                                        text_temp = string.Empty;
                                                        Boolean moringleav = false;
                                                        Boolean evenleave = false;
                                                        dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                        dvholiday = dsall.Tables[2].DefaultView;
                                                        if (dvholiday.Count > 0)
                                                        {
                                                            if (!hatholiday.Contains(cur_day.ToString()))
                                                            {
                                                                hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                            }
                                                            if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                            {
                                                                moringleav = true;
                                                            }
                                                            if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                            {
                                                                evenleave = true;
                                                            }
                                                            if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                            {
                                                                evenleave = true;
                                                                moringleav = true;
                                                            }
                                                        }
                                                        for (temp = 1; temp <= noofhrs; temp++)
                                                        {
                                                            string sp_rd = string.Empty;
                                                            Boolean altfalg = false;
                                                            string getcolumnfield = Convert.ToString(strday + temp);
                                                            if (dvsemster.Count > 0)
                                                            {
                                                                if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                {
                                                                    string timetable = string.Empty;
                                                                    string name = dvsemster[0]["ttname"].ToString();
                                                                    if (name != null && name.Trim() != "")
                                                                    {
                                                                        timetable = name;
                                                                    }
                                                                    sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                    DataSet snds = new DataSet();
                                                                    string[] sp_rd_semi = sp_rd.Split(';');
                                                                    string sno = " select count(r.roll_no),Batch_Year,degree_code,semester,Sections,subject_no from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and r.degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and r.Current_Semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and subject_no='" + lasubno + "' group by Batch_Year,degree_code,semester,Sections,subject_no";
                                                                    snds = d2.select_method_wo_parameter(sno, "Text");
                                                                    if (snds.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        noofstudents = snds.Tables[0].Rows[0]["Column1"].ToString();
                                                                    }
                                                                    for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                    {
                                                                        string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                        if (sp2.GetUpperBound(0) >= 1)
                                                                        {
                                                                            int upperbound = sp2.GetUpperBound(0);
                                                                            for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                            {
                                                                                if (sp2[multi_staff] == stafcode)
                                                                                {
                                                                                    if (!hatsubject.Contains(sp2[0].ToString()))
                                                                                    {
                                                                                        visibleflag = true;
                                                                                        spanrow++;
                                                                                        if (tempdegree != dsstaffvalues.Tables[0].Rows[st]["dept_name"].ToString())
                                                                                        {
                                                                                            load_spread.Visible = true;
                                                                                            btnprintmaster.Visible = true;
                                                                                            lblrptname.Visible = true;
                                                                                            txtexcelname.Visible = true;
                                                                                            btnxl.Visible = true;
                                                                                            tempdegree = dsstaffvalues.Tables[0].Rows[st]["dept_name"].ToString();
                                                                                            load_spread.Sheets[0].RowCount++;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = tempdegree;
                                                                                            load_spread.Sheets[0].SpanModel.Add(load_spread.Sheets[0].RowCount - 1, 0, 1, load_spread.Sheets[0].ColumnCount);
                                                                                        }
                                                                                        load_spread.Sheets[0].RowCount++;
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 1].Text = staff_code;
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 2].Text = dsstaffvalues.Tables[0].Rows[st]["staff_name"].ToString();
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 3].Text = Qualification;//dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + '-' + degreename + '-' + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + '-' + dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                        if (dvsubject.Count > 0)
                                                                                        {
                                                                                            string subnam = dvsubject[0]["subject_name"].ToString();
                                                                                            string prcount = dvsubject[0]["promote_count"].ToString();
                                                                                            if (prcount.ToUpper().Trim() == "FALSE")
                                                                                            {
                                                                                                tut++;
                                                                                            }
                                                                                            if (!hatcurlab.Contains(sp2[0].ToString()))
                                                                                            {
                                                                                                subnam = subnam + " - Theory";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                subnam = subnam + " - Lab";
                                                                                            }
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 5].Text = subnam;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Text = Designame + " " + Experience;//subnam;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Tag = sp2[0].ToString();
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 6].Text = dsperiod.Tables[0].Rows[pre]["semester"].ToString();
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 7].Text = degreename + ' ' + dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 8].Text = SubjectCode;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 9].Text = SubjectName;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 10].Text = noofstudents;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 15].Text = string.Empty;
                                                                                        }
                                                                                        hatsubject.Add(sp2[0].ToString(), 0);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int hrva = Convert.ToInt32(hatsubject[sp2[0].ToString()]);
                                                                                        hrva++;
                                                                                        hatsubject[sp2[0].ToString()] = hrva;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (startrow < load_spread.Sheets[0].RowCount)
                                {
                                    int tothr = 0;
                                    for (int l = startrow; l < load_spread.Sheets[0].RowCount; l++)
                                    {
                                        if (load_spread.Sheets[0].Cells[l, 4].Tag != null)
                                        {
                                            string subn = load_spread.Sheets[0].Cells[l, 4].Tag.ToString();
                                            if (hatsubject.Contains(subn))
                                            {
                                                int getno = Convert.ToInt32(hatsubject[subn]);
                                                tothr = tothr + getno;
                                                if (!hatcurlab.Contains(subn))
                                                {
                                                    totaltheohours = totaltheohours + getno;
                                                }
                                                else
                                                {
                                                    totallabhours = totallabhours + getno;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            startrow = l + 1;
                                            finalstartrow = l + 1;
                                        }
                                    }
                                    totalhours = totalhours + tothr;
                                    totalhours = totalhours + tut;
                                }
                            }
                        }
                        if (finalstartrow < load_spread.Sheets[0].RowCount)
                        {
                            load_spread.Sheets[0].Cells[finalstartrow, 11].Text = totaltheohours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 11, spanrow, 1);
                            load_spread.Sheets[0].Cells[finalstartrow, 12].Text = totallabhours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 12, spanrow, 1);
                            load_spread.Sheets[0].Cells[finalstartrow, 13].Text = Convert.ToString(tut);
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 13, spanrow, 1);
                            load_spread.Sheets[0].Cells[finalstartrow, 14].Text = totalhours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 14, spanrow, 1);
                        }
                        load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
                        load_spread.Height = 900;
                        load_spread.Width = 1015;
                    }
                }
            }
            if (visibleflag == false)
            {
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Records Found";
            }
            int hei = 250;
            for (int i = 0; i < load_spread.Sheets[0].RowCount; i++)
            {
                hei = hei + load_spread.Sheets[0].Rows[i].Height;
            }
            if (hei < 900)
            {
                load_spread.Height = hei;
            }
        }
        catch (Exception ex)
        {
            errlbl.Text = ex.ToString();
            errlbl.Visible = true;
        }
    }

    public void loadclasshourreport()
    {
        try
        {
            Boolean visibleflag = false;
            load_spread.Sheets[0].ColumnHeader.RowCount = 0;
            load_spread.Sheets[0].RowCount = 0;
            load_spread.Sheets[0].ColumnCount = 0;
            load_spread.Sheets[0].ColumnHeader.RowCount = 2;
            load_spread.Sheets[0].ColumnCount = 11;
            load_spread.Sheets[0].FrozenColumnCount = 5;
            //load_spread.Pager.PageCount = 9;
            //load_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            //load_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            load_spread.Sheets[0].RowHeader.Visible = false;
            load_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Batch Year/ Branch / Sem / Sec";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Handling Subjects";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Wise Periods";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Class Wise Periods";

            load_spread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Conducted Periods";

            load_spread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Papers";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Theory Periods";
            load_spread.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Practical Periods";
            load_spread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Total Periods";
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);
            load_spread.Sheets[0].Columns[0].Locked = true;
            load_spread.Sheets[0].Columns[1].Locked = true;
            load_spread.Sheets[0].Columns[2].Locked = true;
            load_spread.Sheets[0].Columns[3].Locked = true;
            load_spread.Sheets[0].Columns[4].Locked = true;
            load_spread.Sheets[0].Columns[5].Locked = true;
            load_spread.Sheets[0].Columns[6].Locked = true;
            load_spread.Sheets[0].Columns[7].Locked = true;
            load_spread.Sheets[0].Columns[8].Locked = true;
            load_spread.Sheets[0].Columns[9].Locked = true;
            load_spread.Sheets[0].Columns[0].Width = 30;
            load_spread.Sheets[0].Columns[1].Width = 70;
            load_spread.Sheets[0].Columns[2].Width = 150;
            load_spread.Sheets[0].Columns[3].Width = 200;
            load_spread.Sheets[0].Columns[4].Width = 200;
            load_spread.Sheets[0].Columns[5].Width = 70;
            load_spread.Sheets[0].Columns[6].Width = 70;
            load_spread.Sheets[0].Columns[7].Width = 70;
            load_spread.Sheets[0].Columns[8].Width = 70;
            load_spread.Sheets[0].Columns[9].Width = 70;

            load_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            load_spread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            load_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            load_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            load_spread.Sheets[0].RowHeader.Visible = false;
            //string sql_s =string.Empty;
            string Strsql = string.Empty;
            //string SqlBatchYear =string.Empty;
            //string SqlPrefinal1 =string.Empty;
            //string SqlPrefinal2 =string.Empty;
            //string SqlPrefinal3 =string.Empty;
            //string SqlPrefinal4 =string.Empty;
            DataSet dsgetvalue = new DataSet();
            //string getquery =string.Empty;
            string SqlFinal = string.Empty;
            //string SqlFinal1 =string.Empty;
            string sql1 = string.Empty;
            string tmp_varstr = string.Empty;
            string tmp_camprevar = string.Empty;
            string cur_camprevar = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            //string check_lab =string.Empty;
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();
            //string sectionsvalue =string.Empty;
            //string sectionvar =string.Empty;
            DataSet ds_attndmaster = new DataSet();
            Hashtable ht_sch = new Hashtable();
            Hashtable ht_sdate = new Hashtable();
            Hashtable ht_bell = new Hashtable();
            Hashtable ht_period = new Hashtable();
            string degreename = string.Empty;
            Hashtable hatdegreename = new Hashtable();
            string degree_var = string.Empty;
            //string date1;
            //string date2;
            // string datefrom;
            // string dateto;
            // string sqlstr =string.Empty;
            int noofhrs = 0;
            string vari = string.Empty;
            ht_sch.Clear();
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = dac.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_sdate.Clear();
            if (ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_bell.Clear();
            if (ds_attndmaster.Tables[2].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);
                    if (!ht_bell.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                        ht_bell.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_period.Clear();
            if (ds_attndmaster.Tables[3].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);
                    if (!ht_period.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                        ht_period.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
            {
                if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                {
                    hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                }
            }
            string getalldetails = "select * from Alternate_Schedule ; ";
            getalldetails = getalldetails + "select * from Semester_Schedule order by FromDate desc; ";
            getalldetails = getalldetails + "Select * from holidaystudents ; ";
            getalldetails = getalldetails + "select * from seminfo ; ";
            getalldetails = getalldetails + " select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester,sm.Lab from syllabus_master sy,sub_sem sm,subject s,Registration r where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and r.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and  sy.semester=r.Current_Semester and r.college_code='" + Session["collegecode"].ToString() + "' order by sy.Batch_Year,sy.degree_code,sy.semester ;";
            getalldetails = getalldetails + " select distinct Current_Semester,Batch_Year,degree_code from Registration where cc=0 and delflag=0 and exam_flag<>'debar'; ";
            getalldetails = getalldetails + " select no_of_hrs_I_half_day as mor,no_of_hrs_I_half_day as eve,degree_code,semester,No_of_hrs_per_day as tot from periodattndschedule";
            DataSet dsall = dac.select_method_wo_parameter(getalldetails, "Text");
            string deptcode = string.Empty;
            string designcode = string.Empty;
            string staffcode = string.Empty;
            if (deptddl.SelectedItem.ToString() != "All" && deptddl.SelectedItem.ToString() != "")
            {
                deptcode = " and st.dept_code='" + deptddl.SelectedValue.ToString() + "'";
            }
            if (desigddl.SelectedItem.ToString() != "All" && desigddl.SelectedItem.ToString() != "")
            {
                designcode = " and st.desig_code='" + desigddl.SelectedValue.ToString() + "'";
            }
            if (stafftxt.Items.Count > 0)
            {
                if (stafftxt.SelectedItem.ToString() != "All" && stafftxt.SelectedItem.ToString() != "")
                {
                    staffcode = " and s.staff_code='" + stafftxt.SelectedValue.ToString() + "'";
                }
            }
            string tempdegree = string.Empty;
            int srno = 0;
            string staffquery = "select s.staff_code,s.staff_name,st.dept_code,st.desig_code,h.dept_name,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=st.staff_code and s.resign=0 and s.settled=0 and st.latestrec=1 and st.dept_code=h.dept_code and st.desig_code=d.desig_code " + deptcode + " " + designcode + " " + staffcode + " and s.college_code=d.collegeCode and s.college_code=h.college_code order by st.dept_code,st.desig_code,s.staff_name";
            DataSet dsstaffvalues = dac.select_method_wo_parameter(staffquery, "Text");
            if (dsstaffvalues.Tables[0].Rows.Count > 0)
            {
                for (int st = 0; st < dsstaffvalues.Tables[0].Rows.Count; st++)
                {
                    staff_code = dsstaffvalues.Tables[0].Rows[st]["staff_code"].ToString();
                    string stafcode = dsstaffvalues.Tables[0].Rows[st]["staff_code"].ToString();
                    SqlFinal = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date,si.start_date from staff_selector ss,Registration r,";
                    SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                    SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                    SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and  LTRIM(rtrim(isnull(r.sections,'')))=LTRIM(rtrim(isnull(ss.sections,''))) and ss.batch_year=r.Batch_Year";
                    SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                    SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                    SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "'";
                    DataSet dsperiod = dac.select_method(SqlFinal, hat, "Text");
                    string strsction = string.Empty;
                    Hashtable hatholiday = new Hashtable();
                    DataView dvalternaet = new DataView();
                    DataView dvsemster = new DataView();
                    DataView dvholiday = new DataView();
                    DataView dvdaily = new DataView();
                    DataView dvsubject = new DataView();
                    DataView dvsublab = new DataView();
                    string start_datesem = string.Empty;
                    string noofdays = string.Empty;
                    string Day_Order = string.Empty;
                    string tmp_datevalue = string.Empty;
                    if (dsperiod.Tables[0].Rows.Count > 0)
                    {
                        srno++;
                        int totalhours = 0;
                        int totaltheohours = 0;
                        int totallabhours = 0;
                        int spanrow = 0;
                        int finalstartrow = load_spread.Sheets[0].RowCount;
                        for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                        {
                            Hashtable hatsubject = new Hashtable();
                            int startrow = load_spread.Sheets[0].RowCount;
                            cur_camprevar = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                            string getdate = string.Empty;
                            if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                            {
                                if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                {
                                    degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                }
                                strsction = string.Empty;
                                if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                {
                                    strsction = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                }
                                dsall.Tables[4].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                DataView dtcurlab = dsall.Tables[4].DefaultView;
                                Hashtable hatcurlab = new Hashtable();
                                for (int cula = 0; cula < dtcurlab.Count; cula++)
                                {
                                    string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                    string labhour = dtcurlab[cula]["lab"].ToString();
                                    if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                    {
                                        if (!hatcurlab.Contains(lasubno))
                                        {
                                            hatcurlab.Add(lasubno, lasubno);
                                        }
                                    }
                                }
                                hatholiday.Clear();
                                dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                DataView duholiday = dsall.Tables[2].DefaultView;
                                for (int i = 0; i < duholiday.Count; i++)
                                {
                                    if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                    {
                                        hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                    }
                                }
                                int frshlf = 0, schlf = 0;
                                dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                DataView dvperiod = dsall.Tables[6].DefaultView;
                                if (dvperiod.Count > 0)
                                {
                                    string morhr = dvperiod[0]["mor"].ToString();
                                    string evehr = dvperiod[0]["mor"].ToString();
                                    if (morhr != null && morhr.Trim() != "")
                                    {
                                        frshlf = Convert.ToInt32(morhr);
                                        noofhrs = Convert.ToInt32(dvperiod[0]["tot"].ToString());
                                    }
                                    if (evehr != null && evehr.Trim() != "")
                                    {
                                        schlf = Convert.ToInt32(evehr);
                                    }
                                }
                                string getcurrent_sem = string.Empty;
                                dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "'";
                                DataView dvcurrsem = dsall.Tables[5].DefaultView;
                                if (dvcurrsem.Count > 0)
                                {
                                    getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                }
                                if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                {
                                    string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                    string altersetion = string.Empty;
                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                    {
                                        altersetion = "and Sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                    }
                                    dsall.Tables[3].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                    DataView dvsemdate = dsall.Tables[3].DefaultView;
                                    if (dvsemdate.Count > 0)
                                    {
                                        //DateTime dtendate = Convert.ToDateTime(dvsemdate[0]["end_date"].ToString());
                                        //DateTime dtstartdate = Convert.ToDateTime(dvsemdate[0]["start_date"].ToString());

                                        string fromdate = txtFromDate.Text;
                                        string todate = txtToDate.Text;
                                        string[] split = fromdate.Split('/');
                                        string[] tdt = todate.Split('/');
                                        DateTime dtendate = Convert.ToDateTime(tdt[1] + "/" + tdt[0] + "/" + tdt[2]);
                                        DateTime dtstartdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                                        //Rajkumar======================
                                        hatsubcon11.Clear();
                                        hatmisub11.Clear();
                                        hatsubact11.Clear();
                                        for (DateTime dtn = dtstartdate; dtn <= dtendate; dtn = dtn.AddDays(1))
                                        {
                                            DateTime cur_day = dtn;
                                            tmp_datevalue = Convert.ToString(cur_day);
                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                            string SchOrder = string.Empty;
                                            string day_from = cur_day.ToString("yyyy-MM-dd");
                                            DateTime schfromdate = cur_day;
                                            dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                            dvsemster = dsall.Tables[1].DefaultView;
                                            if (dvsemster.Count > 0)
                                            {
                                                getdate = dvsemster[0]["FromDate"].ToString();
                                            }
                                            else
                                            {
                                                getdate = string.Empty;
                                            }
                                            if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                            {
                                                DateTime getsche = Convert.ToDateTime(getdate);
                                                if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                {
                                                    if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                    {
                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                        string[] sp_rd_semi = contvar.Split(',');
                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                        {
                                                            SchOrder = sp_rd_semi[0].ToString();
                                                            noofdays = sp_rd_semi[1].ToString();
                                                        }
                                                    }
                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                    if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                    {
                                                        string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                        string[] sp_rd_semi = contvar.Split(',');
                                                        if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                        {
                                                            start_datesem = sp_rd_semi[0].ToString();
                                                            start_dayorder = sp_rd_semi[1].ToString();
                                                        }
                                                    }
                                                    Day_Order = string.Empty;
                                                    if (SchOrder == "1")
                                                    {
                                                        strday = cur_day.ToString("ddd"); //Week Dayorder
                                                        Day_Order = "0-" + Convert.ToString(strday);
                                                    }
                                                    else
                                                    {
                                                        strday = findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                    }
                                                    string reasonsun = string.Empty;
                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                    {
                                                        reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                        if (reasonsun.Trim().ToLower() == "sunday")
                                                        {
                                                        }
                                                    }
                                                    if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                    {
                                                        string str_day = strday;
                                                        string Atmonth = cur_day.Month.ToString();
                                                        string Atyear = cur_day.Year.ToString();
                                                        long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                        sql1 = string.Empty;
                                                        Strsql = string.Empty;
                                                        for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                        {
                                                            Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                            if (sql1 == "")
                                                            {
                                                                sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                            }
                                                            else
                                                            {
                                                                sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                            }
                                                        }
                                                        string day_aten = cur_day.Day.ToString();
                                                        Boolean check_hour = false;
                                                        string strsectionvar = string.Empty;
                                                        string labsection = string.Empty;
                                                        if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                        {
                                                            strsectionvar = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                            labsection = " and sections='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                        }
                                                        sql1 = " and (" + sql1 + ")";
                                                        dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                        dvalternaet = dsall.Tables[0].DefaultView;
                                                        text_temp = string.Empty;
                                                        Boolean moringleav = false;
                                                        Boolean evenleave = false;
                                                        dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                        dvholiday = dsall.Tables[2].DefaultView;
                                                        if (dvholiday.Count > 0)
                                                        {
                                                            if (!hatholiday.Contains(cur_day.ToString()))
                                                            {
                                                                hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                            }
                                                            if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                            {
                                                                moringleav = true;
                                                            }
                                                            if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                            {
                                                                evenleave = true;
                                                            }
                                                            if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                            {
                                                                evenleave = true;
                                                                moringleav = true;
                                                            }
                                                        }
                                                        for (temp = 1; temp <= noofhrs; temp++)
                                                        {
                                                            string sp_rd = string.Empty;
                                                            Boolean altfalg = false;
                                                            string getcolumnfield = Convert.ToString(strday + temp);
                                                            if (dvsemster.Count > 0)
                                                            {
                                                                if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                {
                                                                    string timetable = string.Empty;
                                                                    string name = dvsemster[0]["ttname"].ToString();
                                                                    if (name != null && name.Trim() != "")
                                                                    {
                                                                        timetable = name;
                                                                    }
                                                                    sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                    string[] sp_rd_semi = sp_rd.Split(';');
                                                                    for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                    {
                                                                        string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                        if (sp2.GetUpperBound(0) >= 1)
                                                                        {
                                                                            int upperbound = sp2.GetUpperBound(0);
                                                                            for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                            {
                                                                                if (sp2[multi_staff] == stafcode)
                                                                                {
                                                                                    if (!hatsubject.Contains(sp2[0].ToString()))
                                                                                    {
                                                                                        if (!hatdicSub.Contains(sp2[0].ToString()))
                                                                                        {
                                                                                            hatdicSub.Add(sp2[0].ToString(), 1);
                                                                                        }
                                                                                        visibleflag = true;
                                                                                        spanrow++;
                                                                                        if (tempdegree != dsstaffvalues.Tables[0].Rows[st]["dept_name"].ToString())
                                                                                        {
                                                                                            load_spread.Visible = true;
                                                                                            btnprintmaster.Visible = true;
                                                                                            lblrptname.Visible = true;
                                                                                            txtexcelname.Visible = true;
                                                                                            btnxl.Visible = true;
                                                                                            tempdegree = dsstaffvalues.Tables[0].Rows[st]["dept_name"].ToString();
                                                                                            load_spread.Sheets[0].RowCount++;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = tempdegree;
                                                                                            load_spread.Sheets[0].SpanModel.Add(load_spread.Sheets[0].RowCount - 1, 0, 1, load_spread.Sheets[0].ColumnCount);
                                                                                        }
                                                                                        load_spread.Sheets[0].RowCount++;
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 2].Text = dsstaffvalues.Tables[0].Rows[st]["staff_name"].ToString();
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 1].Text = staff_code;
                                                                                        load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 3].Text = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + '-' + degreename + '-' + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + '-' + dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                        if (dvsubject.Count > 0)
                                                                                        {
                                                                                            string subnam = dvsubject[0]["subject_name"].ToString();
                                                                                            if (!hatcurlab.Contains(sp2[0].ToString()))
                                                                                            {
                                                                                                subnam = subnam + " - Theory";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                subnam = subnam + " - Lab";
                                                                                            }
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Text = subnam;
                                                                                            load_spread.Sheets[0].Cells[load_spread.Sheets[0].RowCount - 1, 4].Tag = sp2[0].ToString();
                                                                                        }
                                                                                        hatsubject.Add(sp2[0].ToString(), 0);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int hrva = Convert.ToInt32(hatsubject[sp2[0].ToString()]);
                                                                                        hrva++;
                                                                                        hatsubject[sp2[0].ToString()] = hrva;
                                                                                    }


                                                                                    //Rajkumar for conduct hour on 12-10-2018

                                                                                    //str_day
                                                                                    int gethour = Convert.ToInt32(hatsubact11[sp2[0].ToString()]);
                                                                                    gethour++;
                                                                                    hatsubact11[sp2[0].ToString()] = gethour;
                                                                                    //string ttname = "  and ttname='"+name+"'";
                                                                                    string ttname = string.Empty;
                                                                                    string hr = temp.ToString();
                                                                                    string Att_strqueryst1 = "0";
                                                                                    string[] spiltdate = cur_day.ToString("d/MM/yyyy").Split('/');
                                                                                    long strdate11 = (Convert.ToInt32(spiltdate[1]) + Convert.ToInt32(spiltdate[2]) * 12);
                                                                                    string Att_dcolumn1 = "d" + spiltdate[0] + "d" + hr;
                                                                                    string check_lab = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + sp2[0].ToString() + "'");//Modified by srinath 7/1/2013
                                                                                    string subjectType = d2.GetFunction("select subject_type from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + sp2[0].ToString() + "'");
                                                                                    if (!hatSubType.ContainsKey(subjectType))
                                                                                    {
                                                                                        hatSubType.Add(subjectType, 0);
                                                                                    }
                                                                                    if (check_lab == "0" || check_lab.Trim().ToLower() == "false")//Modified by srinath 7/1/2013
                                                                                    {
                                                                                        if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                        {
                                                                                            Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + sp2[0].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' ");
                                                                                            if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                            {
                                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + sp2[0].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')");
                                                                                                if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                    countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                    countsub++;
                                                                                                    hatSubType[subjectType] = countsub;
                                                                                                }
                                                                                                else
                                                                                                {

                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {

                                                                                                gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                gethour++;
                                                                                                hatmisub11[sp2[0].ToString()] = gethour;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + sp2[0].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "'");
                                                                                            if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                            {
                                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and registration.current_semester=sc.semester and sc.subject_no='" + sp2[0].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')");
                                                                                                if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                    countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                    countsub++;
                                                                                                    hatSubType[subjectType] = countsub;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                gethour++;
                                                                                                hatmisub11[sp2[0].ToString()] = gethour;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                        {
                                                                                            if (false)//alternatelab == true
                                                                                            {
                                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                                {
                                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='') and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                        countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                        countsub++;
                                                                                                        hatSubType[subjectType] = countsub;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' " + ttname + " and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                                {
                                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "  and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='') and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' " + ttname + " and hour_value='" + hr + "' and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                        countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                        countsub++;
                                                                                                        hatSubType[subjectType] = countsub;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (false)//alternatelab == true
                                                                                            {
                                                                                                Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                if (Convert.ToInt32(Att_strqueryst1) > 0) // check record available or not for particular month
                                                                                                {
                                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser_new sc where degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester  and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')    ");
                                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                        countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                        countsub++;
                                                                                                        hatSubType[subjectType] = countsub;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" +dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' " + ttname + " and hour_value='" + hr + "'  and sections=registration.sections and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester   ");
                                                                                                if (true) // check record available or not for particular month Convert.ToInt32(Att_strqueryst1) > 0
                                                                                                {
                                                                                                    Att_strqueryst1 = d2.GetFunctionv("select count(*) from registration,attendance,subjectchooser sc where degree_code='" +
dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and current_semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and sections='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and sc.roll_no=registration.roll_no  and sc.batch in(select distinct stu_batch from laballoc where subject_no='" + sp2[0].ToString() + "' and batch_year=registration.batch_year and day_value='" + str_day + "' " + ttname + " and hour_value='" + hr + "'  and LTRIM(rtrim(isnull(sections,'')))=LTRIM(rtrim(isnull(registration.sections,''))) and  semester=sc.semester and  subject_no=sc.subject_no) and registration.current_semester=sc.semester  and (" + Att_dcolumn1 + " is null or " + Att_dcolumn1 + "=0 or " + Att_dcolumn1 + "='')  ");
                                                                                                    if (Convert.ToInt32(Att_strqueryst1) == 0) // check record available or not for particular month
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatsubcon11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatsubcon11[sp2[0].ToString()] = gethour;
                                                                                                        countsub = Convert.ToInt32(hatSubType[subjectType]);
                                                                                                        countsub++;
                                                                                                        hatSubType[subjectType] = countsub;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                        gethour++;
                                                                                                        hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    gethour = Convert.ToInt32(hatmisub11[sp2[0].ToString()]);
                                                                                                    gethour++;
                                                                                                    hatmisub11[sp2[0].ToString()] = gethour;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    //



                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (startrow < load_spread.Sheets[0].RowCount)
                                {
                                    int tothr = 0;
                                    for (int l = startrow; l < load_spread.Sheets[0].RowCount; l++)
                                    {
                                        if (load_spread.Sheets[0].Cells[l, 4].Tag != null)
                                        {
                                            string subn = load_spread.Sheets[0].Cells[l, 4].Tag.ToString();
                                            if (hatsubject.Contains(subn))
                                            {
                                                int getno = Convert.ToInt32(hatsubject[subn]);
                                                tothr = tothr + getno;
                                                load_spread.Sheets[0].Cells[l, 5].Text = getno.ToString();
                                                if (!hatcurlab.Contains(subn))
                                                {
                                                    totaltheohours = totaltheohours + getno;
                                                }
                                                else
                                                {
                                                    totallabhours = totallabhours + getno;
                                                }
                                            }

                                            if (hatsubcon11.Contains(subn))
                                            {
                                                int getno = Convert.ToInt32(hatsubcon11[subn]);
                                                load_spread.Sheets[0].Cells[l, 7].Text = getno.ToString();
                                            }
                                        }
                                        else
                                        {
                                            startrow = l + 1;
                                            finalstartrow = l + 1;
                                        }
                                    }
                                    load_spread.Sheets[0].Cells[startrow, 6].Text = tothr.ToString();
                                    load_spread.Sheets[0].SpanModel.Add(startrow, 6, load_spread.Sheets[0].RowCount - startrow, 1);
                                    totalhours = totalhours + tothr;
                                }
                            }
                        }
                        load_spread.Sheets[0].PageSize = load_spread.Sheets[0].RowCount;
                        load_spread.Height = 900;
                        load_spread.Width = 1015;
                        if (finalstartrow < load_spread.Sheets[0].RowCount)
                        {
                            load_spread.Sheets[0].Cells[finalstartrow, 8].Text = totaltheohours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 8, spanrow, 1);
                            load_spread.Sheets[0].Cells[finalstartrow, 9].Text = totallabhours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 9, spanrow, 1);
                            load_spread.Sheets[0].Cells[finalstartrow, 10].Text = totalhours.ToString();
                            load_spread.Sheets[0].SpanModel.Add(finalstartrow, 10, spanrow, 1);
                            load_spread.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
                            load_spread.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                            //load_spread.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;
                            //load_spread.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                            load_spread.Sheets[0].Columns[8].Visible = false;
                            load_spread.Sheets[0].Columns[9].Visible = false;
                        }
                    }
                }
                string subNo = string.Empty;
                string Qrysubtype = string.Empty;
                DataTable dtSubType = new DataTable();
                foreach (object key in hatdicSub.Keys)
                {
                    if (string.IsNullOrEmpty(subNo))
                        subNo = Convert.ToString(key);
                    else
                        subNo = subNo + "," + Convert.ToString(key);
                }
                if (!string.IsNullOrEmpty(subNo))
                {
                    Qrysubtype = "select distinct  ss.subject_type from subject s,sub_sem ss where s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no in(" + subNo + ")";
                    dtSubType = dir.selectDataTable(Qrysubtype);
                    if (dtSubType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSubType.Rows)
                        {
                            load_spread.Sheets[0].ColumnCount++;
                            string sub = Convert.ToString(dr["subject_type"]);
                            string subNoCount = Convert.ToString(hatSubType[sub]);
                            load_spread.Sheets[0].ColumnHeader.Cells[0, load_spread.Sheets[0].ColumnCount - 1].Text = sub;
                            load_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, load_spread.Sheets[0].ColumnCount - 1, 2, 1);
                            load_spread.Sheets[0].Cells[1, load_spread.Sheets[0].ColumnCount - 1].Text = subNoCount.ToString();
                            load_spread.Sheets[0].SpanModel.Add(1, load_spread.Sheets[0].ColumnCount - 1, load_spread.Sheets[0].RowCount, 1);
                            load_spread.Sheets[0].Columns[load_spread.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            load_spread.Sheets[0].Columns[load_spread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            if (visibleflag == false)
            {
                load_spread.Visible = false;
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Records Found";
            }
            int hei = 250;

            load_spread.SaveChanges();
            for (int i = 0; i < load_spread.Sheets[0].RowCount; i++)
            {
                hei = hei + load_spread.Sheets[0].Rows[i].Height;
            }
            if (hei < 900)
            {
                load_spread.Height = hei;
            }
        }
        catch
        {
        }
    }

    public void conductHour()
    {
        try
        {



        }
        catch
        {
        }
    }

}
