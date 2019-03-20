using System;

using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using BalAccess;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.IO;
//using NAudio.Wave;
//using NAudio.CoreAudioApi;

public partial class Camvoicereport : System.Web.UI.Page
{

    InsproDirectAccess dirAcc = new InsproDirectAccess();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection ncon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //// SqlConnection condegree = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rankcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection con_gender = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection con_seat = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection con_strseat = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection Totcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    // SqlConnection lcon2 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon3 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon4 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    // SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    // SqlConnection cons = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;
    // Hashtable hat = new Hashtable();
    Hashtable htpass = new Hashtable();
    Hashtable htfail = new Hashtable();
    Hashtable htabsent = new Hashtable();
    Hashtable htpresent = new Hashtable();
    Hashtable htpassperc = new Hashtable();
    Hashtable htclsavg = new Hashtable();
    Hashtable htdate = new Hashtable();
    DAccess2 dacces2 = new DAccess2();

    Boolean yesflag = false;
    //Opt------------
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;
    DateTime Admission_date;
    //-----------------
    //opt----
    int demfcal, demtcal;
    string monthcal;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    static Boolean splhr_flag = false;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    string strorder = "";
    string SenderID = "";
    string Password = "";
    static int maxmark = 0;
    //-------

    DataSet ds_holi = new DataSet();
    DataSet ds_optim = new DataSet();
    //  string collegecode = "";
    //  string usercode = "";
    //string regularflag = "";
    string markglag = "";
    string rol_no = "";
    string courseid = "";
    string atten = "";
    string Master1 = "";
    int Atday = 0, endk = 0;
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string staff = "";
    double perofpass = 0;
    double avg = 0;
    string code = "";
    string text = "";
    static string exammothdate = "";
    Boolean Isfirst = false;
    Boolean IsFirstcol = false;
    Boolean RnkFlag;
    Boolean PresentFlag = false;
    Boolean callattfun;
    DateTime dt1, dt2;
    DateTime date_today;
    int[] hasharray;
    int student = 0;
    // Session["strvar"] = "0";
    int ic = 0;
    int i;
    static int cook = 0;
    static int colcnt = 0;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList subarray = new ArrayList();
    static ArrayList markarray = new ArrayList();
    static ArrayList resultarray = new ArrayList();
    double totpresentday;
    double perprest, perpresthrs, perabsent, perabsenthrs, perondu, peronduhrs, perleave, perleavehrs;
    double pertothrs, pertotondu, pertotleavehrs, pertotabsenthrs, onduday, cumcontotpresentday, percontotpresentday, hollyhrs, condhrs, balamonday, att_points;
    int minI, minII, perdayhrs, wk1, wk2, wk3, wk4, wk5, wk6, wk7, wk8, wk9, Ihof, IIhof, fullday, cumfullday;
    double cumperprest, cumperpresthrs, cumperabsent, cumperabsenthrs, cumperondu, cumperonduhrs, cumperleave, cumperleavehrs, checkpre, baldate, totmonth, cummcc, cumcondhrs, percondhrs = 0, cumatt_points;
    string m7, m2, m3, m4, m5, m6, m1, m8, m9;
    Double totalRows = 0;

    int hour1, hour2, hour3, hour4, hour5, hour6, hour7, hour8, hour9, condhrs1, condhrs2, condhrs3, condhrs4, condhrs5, condhrs6, condhrs7, condhrs8, condhrs9;
    int ondu1, ondu2, ondu3, ondu4, ondu5, ondu6, ondu7, ondu8, ondu9, leave1, leave2, leave3, leave4, leave5, leave6, leave7, leave8, leave9;
    string holi_month;
    string fmLength;

    //  int holi_days, abse_point, leave_point, diff_date;
    //--------------------

    double par = 0, abse = 0;
    double present, absent, hollydats, leaves, ondu;
    double presenthrs, absenthrs, hollydatshrs, leaveshrs, onduhrs;
    int perhr, abshr;
    int ond, le, fyyy, mm = 1;
    int daycount, betdays;
    int dd = 0, dat, dumm;
    double onhr, lehr;
    int fm, fyy, fd, tm, tyy, td, fcal, tcal, k;
    double wkhr, wkhd, dumwkhr, dumwkhd, dumper, per;
    int kk = 0, cumdays, printcheck;
    //    string roll_no, reg_no, roll_ad, studname;
    double dumprest, dumpresthrs, dumpresenthrs, dumleaveshrs, dumonduhrs, dumabsenthrs, dumabsent, dumondu, dumleavehrs, dumleave, attday, dumattday;
    int diff = 1, att2, lea1, lea2, on_1, on_2, hdate = 0;
    double holldays, totworkday, dumtotworkday, dumperhrs, dumtoterhrs, perhrs, totperhrs;

    int nohrs;

    double pres = 0;
    double OD = 0;
    double lev = 0;
    double ab = 0;
    double NoOfAbsent = 0;
    double NoOfPresent = 0;
    double NoOfOD = 0;
    double NoOfLe = 0;

    double tempmark, totmark, studperc;

    int appl_no;
    string abshrs_temp;

    int CountNoofPeriod = 0;
    int workinghour = 0;
    double noofWorkingHours = 0;
    double pass_perc, fail_perc;
    double NoOfPass, NoOfFail;
    double mark_avg = 0.0;
    int tot_stud;
    string subjctcode = "";
    double hours_present = 0;
    double hours_absent = 0;
    double hours_od = 0;
    double hours_total = 0;
    double hours_leave = 0;
    double hours_conduct = 0;
    double hours_pres = 0;

    string dateconcat = "";
    string date1concat = "";
    string filepath = "";

    //'----------------------------------------------------------new 
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    string frdate, todate;
    TimeSpan ts;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    // int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int m, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;


    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, tot_ml, per_tot_ml;
    int countds = 0;
    //-----------------------------------------end
    int mediumcount = 0;
    int percount = 0;
    int grdcount = 0;
    int cgpacount = 0;
    int noofdayschlr = 0;
    int noofhstl = 0;
    int totdaycount = 0;
    int tothstlcount = 0;
    int nooftamcount = 0;
    int noofengcount = 0;
    int noofgirlcount = 0;
    int noofboycount = 0;
    int tottamilcount = 0;
    int totengcount = 0;
    int totgirlcount = 0;
    int totboycount = 0;
    int totdayschpass = 0;
    int tothstlpass = 0;
    int totdayschfail = 0;
    int tothstlfail = 0;
    int totgirlspass = 0;
    int totgirlsfail = 0;
    int totboyspass = 0;
    int totboysfail = 0;

    int tottampass = 0;
    int tottamfail = 0;
    int totengpass = 0;
    int totengfail = 0;

    int resultcount = 0;
    int Dpasscount = 0;
    int Hpasscount = 0;
    int Tpasscount = 0;
    int Epasscount = 0;
    int Nooffailcount = 0;
    //int failcount = 0;
    int gendercount = 0;
    int Gpasscount = 0;
    int Bpasscount = 0;
    int quotacount = 0;

    string strseattype = "";
    int seattypecount = 0;
    string getquota = "";
    string getseat = "";
    string gettextcode = "";

    string retrvseatname = "";
    int quotafailcount = 0;
    int quotapasscount = 0;
    int Noofhrattend = 0;
    int Attendpercnt = 0;

    int classstrength = 0;
    int StudentsAppeared = 0;
    int StudentsAbsent = 0;
    int StudentsPassed = 0;
    int StudentsFailed = 0;
    int Average50 = 0;
    int Average50to65 = 0;

    int average65 = 0;
    int classaverage = 0;
    int classmaxmark = 0;
    int Passpercent1 = 0;
    int signat = 0;
    int spancount = 0;
    int count = 0;

    //saravana start
    int min_mark, per_sub_count;//15.02.12 int per_mark chngd
    double per_mark;

    int passcount, failcount, maxcount, mincount, avg_50count, avg_65count, pre_count, ab_count, pperc_count, avg_count, avgg65count;
    int perc75, perc60to74, perc50to59, perc30to49, perc20to29, perc19, maxrollnum, minrollnum, exdate;
    int concolhours;
    int avg_60count;
    int avg_80count;
    //double per;
    int pass = 0, fail = 0;
    int mmyycount;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds7 = new DataSet();
    DataSet ds8 = new DataSet();
    DataSet ds9 = new DataSet();
    DataSet ds10 = new DataSet();
    DataSet ds15 = new DataSet();
    Hashtable hat = new Hashtable();
    int count_has = 0;
    int sub_code = 0;
    double tot_marks;
    double per_marks;
    double percen;
    string pass_fail, per_tage;
    double sub_max_marks;
    int ra_nk;
    int gs_pass_count, bs_pass_count, gs_fail_count, bs_failcount, tot_gs_count, tot_bs_count;
    int gs_count, bs_count, eod_count, tot_stu, x1;
    int d_pass_count, h_pass_count, t_pass_count, e_pass_count;
    int d_fail_count, h_fail_count, t_fail_count, e_fail_count;
    string strsec = "";
    string sections = "";
    string batch = "";
    string degreecode = "";
    string subno = "";
    string semester = "";
    int quota_count;
    string exam_code = "";
    string criteria_no = "";
    int iscount = 1;
    int holi_count;
    //saravana end

    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string collegecode = "";

    //--------------------------new start 06.04.12 print
    static Boolean PrintMaster = false;

    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;

    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string form_heading_name = "";
    string batch_degree_branch = "";
    int chk_secnd_clmn = 0;
    int right_logo_clmn = 0;
    bool sendflag1 = false;
    DataTable camtable = new DataTable();
    DataTable camtable1 = new DataTable();
    DataRow drvoice;
    DataSet dsprint = new DataSet();
    static string datatablecount;
    //'-------------------end print

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       

        lblerror.Visible = false;
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }  
            if (!IsPostBack)
            {

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btn.Visible = false;

                Radiowithoutheader.Visible = false;
                RadioHeader.Visible = false;


                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                gridviewload.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btn.Visible = false;
                //'------------------------------------------------------------
                //btnPrint.Visible = true;


                gridviewload.Visible = false;


                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;

                //RadioButtonList3.SelectedValue = "4";

                //ddlBranch.Items.Insert(0, new ListItem("--Select--", "-1"));
                //ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
                //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
                //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));

                //'----------- Initial date value
                string dt1 = DateTime.Today.ToShortDateString(); 
                string[] dsplit = dt1.Split(new Char[] { '/' });
                dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtFromDate.Text = dateconcat.ToString();


                string dt2 = DateTime.Today.ToShortDateString();
                string[] dt2split = dt2.Split(new Char[] { '/' });
                date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
                txtToDate.Text = date1concat.ToString();

                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                mtrdr = mtcmd.ExecuteReader();

                Session["strvar"] = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Daywise"] = "0";
                Session["Hourwise"] = "0";
                if (mtrdr.HasRows)
                {
                    while (mtrdr.Read())
                    {
                        if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (Stud_Type='Hostler'";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                        }
                        if (mtrdr["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                        }

                        if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                        {
                            genderflag = " and (sex='0'";
                        }
                        if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or sex='1'";
                            }
                            else
                            {
                                genderflag = " and (sex='1'";
                            }

                        }
                        if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Daywise"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Hourwise"] = "1";
                        }
                    }
                }
                if (strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;
                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag;
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag + genderflag;

                if (Request.QueryString["val"] != null)
                {
                    string get_pageload_value = Request.QueryString["val"];
                    if (get_pageload_value.ToString() != null)
                    {
                        string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                        string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val

                        //'----------------------- to bind the batch_year 
                        bindbatch();
                        ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());
                        //'--------------------------------- to bind the course
                        binddegree();
                        ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                        //'----------------------------------------------------------- to bind the branch
                        if (ddlDegree.Text != "")
                        {
                            bindbranch();
                            ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                        }
                        else
                        {
                            lblnorec.Text = "Give degree rights to the staff";
                            lblnorec.Visible = true;
                            lblerror.Visible = false;
                        }
                        //batch

                        //course
                        string collegecode = Session["collegecode"].ToString();
                        string usercode = Session["usercode"].ToString();
                        singleuser = Session["single_user"].ToString();
                        group_user = Session["group_code"].ToString();
                        collegecode = Session["collegecode"].ToString();
                        usercode = Session["usercode"].ToString();

                        //bind semester
                        bindsem();
                        ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                        //bind section
                        bindsec();
                        ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                        //bing test
                        GetTest();
                        ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                        string[] spl_criteria_val = spl_load_val[1].Split('-');
                        if (spl_criteria_val.GetUpperBound(0) > 0)
                        {
                            for (int crt = 0; crt < spl_criteria_val.GetUpperBound(0) + 1; crt++)
                            {
                                //  ddlreport.Items[Convert.ToInt32(spl_criteria_val[crt])].Selected = true;
                            }
                        }
                        txtFromDate.Text = spl_load_val[2].ToString();
                        txtToDate.Text = spl_load_val[3].ToString();

                        btnGo_Click(sender, e);

                        func_header();
                    }
                }
                else
                {
                    //'----------------------- to bind the batch_year 
                    bindbatch();
                    //'--------------------------------- to bind the course
                    binddegree();
                    //'----------------------------------------------------------- to bind the branch
                    if (ddlDegree.Text != "")
                    {
                        bindbranch();
                    }
                    else
                    {
                        lblnorec.Text = "Give degree rights to the staff";
                        lblnorec.Visible = true;
                        lblerror.Visible = false;
                    }
                    //batch

                    //course
                    string collegecode = Session["collegecode"].ToString();
                    string usercode = Session["usercode"].ToString();
                    singleuser = Session["single_user"].ToString();
                    group_user = Session["group_code"].ToString();
                    collegecode = Session["collegecode"].ToString();
                    usercode = Session["usercode"].ToString();

                    //bind semester
                    bindsem();
                    //bind section
                    bindsec();
                    //bing test
                    GetTest();
                    SmsType();//Added By Saranyadevi11.1.2018

                }
            }
        }
        catch
        {
        }
    }
    public void bindsec()
    {

        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds = dacces2.select_method("bind_sec", hat, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }
    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds = dacces2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = dacces2.select_method("bind_degree", hat, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }

    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds = dacces2.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }


    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = gridviewload.FindControl("Update");
        Control cntCancelBtn = gridviewload.FindControl("Cancel");
        Control cntCopyBtn = gridviewload.FindControl("Copy");
        Control cntCutBtn = gridviewload.FindControl("Clear");
        Control cntPasteBtn = gridviewload.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = gridviewload.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    public void GetTest()
    {
        try
        {
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";


            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";


            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter1.Fill(titles);
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = titles;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                //  ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));


            }
            ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
        }
        catch
        {

        }

    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
        drnew = cmd.ExecuteReader();
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



    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;

        btn.Visible = false;


        ddlTest.Items.Clear();
        //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));
        ddlBranch.Items.Clear();

        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();

        bindbranch();

        //bind semester
        bindsem();
        //bind section

        bindsec();
        //bing test
        GetTest();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;
        btnprintmaster.Visible = false;

        bindsem();
        //bind section

        bindsec();
        //bing test
        GetTest();

        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {

            bindsem();

        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);

        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                GetTest();
            }
            else
            {
                ddlSec.Enabled = true;
                GetTest();
            }
        }
        else
        {
            ddlSec.Enabled = false;
            GetTest();
        }
    }
    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlSemYr.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        //ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            ddlSemYr.Items.Clear();
            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());

                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
    }

    //'-------------------------------------- to calculate the  no.of.hours attended
    public void getNoOfPresentHour(string rollno, DateTime fromdate, DateTime todate, string deg, int sem, string colege_code, out double noofpresent, out double noofabsent, out double noofHalfDay, out double noofWorkingHours)
    {
        int CountNoofPresent = 0;

        int CountNoofAbsent = 0;
        int CountNoofHalfDay = 0;

        string tempvalue = "-1";
        //   Dim atrs As New ADODB.Recordset
        int ObtValue = 0;
        long LngOD = 0;
        long LngML = 0;
        long LngNSS = 0;
        long LngHS = 0;
        long LngNCC = 0;
        int SPHrs = 0;
        int SAHrs = 0;
        int schrs = 0;
        int SMHrs = 0;
        string SayHrsAbsent = "";
        string SayHrsPresent = "";
        int Present = 0;
        int Absent = 0;
        int halfday = 0;
        int workingdays = 0;

        int i = 0;
        DateTime dummydate;

        string from_str;
        from_str = fromdate.ToString();
        string[] split_date = from_str.Split(' ');
        string[] from_str_splt = split_date[0].Split('/');
        int tot_month = 0;
        tot_month = Convert.ToInt16(from_str_splt[0]) + ((Convert.ToInt16(from_str_splt[2].ToString())) * 12);

        string to_str;
        to_str = todate.ToString();
        string[] split_to = to_str.Split(' ');
        string[] to_str_splt = split_to[0].Split('/');
        int tot_month_to = 0;
        tot_month_to = Convert.ToInt16(to_str_splt[0]) + ((Convert.ToInt16(to_str_splt[2].ToString())) * 12);

        SqlDataReader AttendRs;
        con3.Close();
        con3.Open();
        cmd1a = new SqlCommand("select * from attendance where roll_no='" + rollno + "' and month_year between " + tot_month + " and " + tot_month_to + " ", con3);
        AttendRs = cmd1a.ExecuteReader();
        while (AttendRs.Read())
        {
            if (AttendRs.HasRows == true)
            {
                SqlDataReader rperiod;
                con1.Close();
                con1.Open();
                cmd3a = new SqlCommand("select * from PeriodAttndSchedule where degree_code = " + deg + " and semester=" + sem + "", con1);
                rperiod = cmd3a.ExecuteReader();
                while (rperiod.Read())
                {
                    if (rperiod.HasRows == true)
                    {
                        dummydate = fromdate;
                        long my = 0;
                        while (dummydate <= (todate))
                        {
                            SayHrsAbsent = "";
                            SayHrsPresent = "";
                            CountNoofPresent = 0;
                            CountNoofPeriod = 0;
                            CountNoofAbsent = 0;
                            my = (Convert.ToInt64(dummydate.ToString("MM")) + (Convert.ToInt64(dummydate.ToString("yyyy"))) * 12);
                            string stratt = "select * from attendance where roll_no='" + rollno + "' and month_year =" + my + "";
                            cmd = new SqlCommand(stratt, con2);
                            con2.Close();
                            con2.Open();
                            SqlDataReader ratt = cmd.ExecuteReader();

                            if (ratt.Read())
                            {
                                //int  hdflag = 0;
                                //int     unmark = 0;
                                //int    wdflag = 0;
                                int hdflag1 = 0;
                                //  int  unmark1 = 0;
                                int wdflag1 = 0;

                                for (i = 1; i <= Convert.ToInt32(rperiod["No_of_hrs_per_day"].ToString()); i++)
                                {
                                    string value = ratt[("d" + dummydate.Day.ToString("") + "d" + i.ToString())].ToString();
                                    if (value != "" && value != "\0" && value != "0" && value != "7")
                                    {
                                        if (tempvalue != (value))
                                        {
                                            tempvalue = value;
                                            tempvalue = value;
                                            string strattmarksetng = "";
                                            strattmarksetng = "select * from AttMasterSetting  where LeaveCode= " + value + " and collegecode=" + colege_code + "";
                                            cona.Close();
                                            cona.Open();
                                            SqlCommand attmrksetng = new SqlCommand(strattmarksetng, cona);
                                            SqlDataReader rleave = attmrksetng.ExecuteReader();
                                            if (rleave.Read())
                                                ObtValue = Convert.ToInt32(rleave["CalcFlag"].ToString());
                                            if (ObtValue == -1)
                                            {
                                                lblnorec.Visible = true;
                                                lblerror.Visible = false;
                                                lblnorec.Text = "Please Update Attendance Master Settings ";
                                            }
                                        }
                                        {
                                            tempvalue = value;
                                            string attmrksetng1 = "select * from AttMasterSetting  where LeaveCode= " + value + " and collegecode=" + colege_code + "";
                                            cona1.Close();
                                            cona1.Open();
                                            cmd1a = new SqlCommand(attmrksetng1, cona1);
                                            SqlDataReader rleave = cmd1a.ExecuteReader();
                                            if (rleave.Read())
                                                ObtValue = Convert.ToInt32(rleave["CalcFlag"].ToString());
                                            rleave.Close();
                                            cona.Close();
                                            if (ObtValue == -1)
                                            {
                                                LabelE.Visible = true;
                                                LabelE.Text = "Please Update Attendance Master Settings";
                                                ratt.Close();
                                                // return;
                                            }
                                            else if (ObtValue == 1)
                                            {
                                                CountNoofAbsent = CountNoofAbsent + 1;
                                                if (SayHrsAbsent.Trim() == "")
                                                {
                                                    SayHrsAbsent = i.ToString();
                                                }
                                                else
                                                {
                                                    SayHrsAbsent = SayHrsAbsent + "," + i;
                                                }
                                                CountNoofPeriod = CountNoofPeriod + 1;
                                                wdflag1 = 1;
                                            }
                                            else if (ObtValue == 0)
                                            {
                                                if (SayHrsPresent.Trim() == "")
                                                {
                                                    SayHrsPresent = i.ToString();
                                                }
                                                else
                                                {
                                                    SayHrsPresent = SayHrsPresent + "," + i;
                                                }
                                                CountNoofPresent = CountNoofPresent + 1;
                                                wdflag1 = 1;
                                                CountNoofPeriod = CountNoofPeriod + 1;
                                            }
                                        }
                                    }
                                    else if (value == "7")
                                    {
                                        hdflag1 = 1;
                                    }
                                    else if (value != "0")
                                    {
                                        int deg_c = 0;
                                        int Cur_Sem = 0;
                                        con2a.Close();
                                        con2a.Open();
                                        SqlDataReader rs1;
                                        cmd4a = new SqlCommand("select degree_code,current_semester from registration where roll_no='" + rollno + " ' and cc=0 and delflag=0 and exam_flag<>'debar'", con2a);
                                        rs1 = cmd4a.ExecuteReader();
                                        while (rs1.Read())
                                        {
                                            if (rs1.HasRows == true)
                                            {
                                                deg_c = Convert.ToInt16(rs1[0].ToString());
                                                Cur_Sem = Convert.ToInt16(rs1[1].ToString());
                                            }
                                        }
                                    }
                                    else if (value == "3" || value == "5")
                                    {
                                        LngOD = LngOD + 1;
                                    }

                                    else if (value == "4")
                                    {
                                        LngML = LngML + 1;
                                    }

                                    else if (value == "6")
                                    {
                                        LngNSS = LngNSS + 1;
                                    }

                                    else if (value == "11")
                                    {
                                        LngNCC = LngNCC + 1;
                                    }

                                    else if (value == "12")
                                    {
                                        LngHS = LngHS + 1;
                                    }
                                }
                                Present = Present + CountNoofPresent;
                                Absent = Absent + CountNoofAbsent;
                                workinghour = workinghour + CountNoofPeriod;
                                if (wdflag1 == 1)
                                {
                                    workingdays = workingdays + 1;
                                }
                                else if (hdflag1 == 1)
                                {
                                    workingdays = workingdays + Convert.ToInt16(0.5);
                                }
                            }//if ratt
                            dummydate = dummydate.AddDays(1);
                        }
                    }
                }
            }//if attndrs

        }


        noofpresent = Present + SPHrs;
        noofabsent = Absent + SAHrs;
        noofHalfDay = halfday;
        noofWorkingHours = workinghour + SMHrs;
        long noofWorkingDays = workingdays;
        long ODHours = LngOD;
        long MLHours = LngML;
        long NSSHours = LngNSS;
        long NCCHours = LngNCC;
        long HSHours = LngHS;
    }



    //'=================================================================================================


    public void Total(string batch, string deg_code, string section, int criteria)
    {


        string batch_year;
        string degree_code;
        string sections;
        int criteriano = criteria;
        batch_year = batch;
        degree_code = deg_code;

        if (section == "" || section == null)
        {
            sections = "";

        }
        else
        {
            sections = "and r.sections='" + section + "'";
        }

        int count = ds5.Tables[0].Rows.Count;
        string examcode = "";
        SqlCommand cmddd = new SqlCommand();
        cmddd.CommandText = "select * from rank   where criteria_no='" + criteriano + "'";
        cmddd.Connection = rankcon;
        rankcon.Open();
        SqlDataReader dr = cmddd.ExecuteReader();
        if (dr.HasRows)
        {
            try
            {
                SqlCommand cmdd = new SqlCommand();
                cmdd.CommandText = "delete from rank  where criteria_no='" + criteriano + "'";
                cmdd.Connection = rcon;
                rcon.Open();
                cmdd.ExecuteNonQuery();
                rcon.Close();

            }
            catch
            {

                rcon.Close();
            }
        }
        dr.Close();
        rankcon.Close();
        for (int i = 0; i < count; i++)
        {
            double percent = 0;
            double total = 0;
            string rank = "";
            string RollNumber = Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]);
            Totcon2.Close();
            Totcon2.Open();
            string str = "select r.marks_obtained as marks,e.min_mark as minmark ,e.exam_code as examcode from exam_type e,subject s,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + criteriano + "' and r.roll_no='" + RollNumber.ToString() + "'";
            SqlDataAdapter da1 = new SqlDataAdapter(str, Totcon2);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            int count1;

            count1 = ds1.Tables[0].Rows.Count;
            for (int j = 0; j < count1; j++)
            {
                if ((ds1.Tables[0].Rows[j]["marks"].ToString()) == "-2")
                {
                    total = total + 0;
                }
                if (Convert.ToDouble(ds1.Tables[0].Rows[j]["marks"]) >= Convert.ToDouble(ds1.Tables[0].Rows[0]["minmark"]))
                {

                    total = total + Convert.ToDouble(ds1.Tables[0].Rows[j]["marks"]);

                }
                else if ((ds1.Tables[0].Rows[j]["marks"].ToString()) != "-2")
                {
                    total = 0;
                    goto l;

                }

            }
            Totcon3.Close();
            Totcon3.Open();
            string sqlstr;
            decimal avgstudent1 = 0;
            decimal avgstudent2 = 0;
            double avgstudent3 = 0;
            string avg = "";
            if ((total > 0) && (count1 > 0))
            {
                percent = total / count1;
                avgstudent1 = Convert.ToDecimal(percent);
                avgstudent2 = Math.Round(avgstudent1, 2);
                avgstudent3 = Convert.ToDouble(avgstudent2);
                avg = Convert.ToString(avgstudent3);
                sqlstr = "insert into Rank values('" + RollNumber + "','" + criteriano + "','" + total + "','" + avg + "','" + rank + "')";
                SqlCommand cmd = new SqlCommand(sqlstr, Totcon3);
                cmd.ExecuteNonQuery();
            }


        l:
            string stt = "";
        }

        if (examcode != " " || examcode != null)
        {
            Totcon3.Close();
            Totcon3.Open();
            string strgetroll;
            strgetroll = "select * from rank  where criteria_no='" + criteriano + "' order by total desc";
            SqlDataAdapter strda = new SqlDataAdapter(strgetroll, Totcon3);
            DataSet strds = new DataSet();
            strda.Fill(strds);
            int strcount;
            double temp = 0;
            int ranks = 0;
            string strupdate = "";
            strcount = strds.Tables[0].Rows.Count;
            for (int sti = 0; sti < strcount; sti++)
            {


                if (temp == 0)
                {
                    ranks = 1;

                    strupdate = "update rank set Rank='" + ranks + "' where Rollno='" + strds.Tables[0].Rows[sti]["Rollno"].ToString() + "' and criteria_no='" + criteriano + "'";
                    temp = Convert.ToDouble(strds.Tables[0].Rows[sti]["Total"]);
                }
                else if (temp != 0)
                {
                    if (Convert.ToDouble(strds.Tables[0].Rows[sti]["Total"]) < temp)
                    {
                        ranks = ranks + 1;
                        strupdate = "update rank set Rank='" + ranks + "' where Rollno='" + strds.Tables[0].Rows[sti]["Rollno"].ToString() + "' and criteria_no='" + criteriano + "'";
                        temp = Convert.ToDouble(strds.Tables[0].Rows[sti]["Total"]);

                    }
                    else if (Convert.ToDouble(strds.Tables[0].Rows[sti]["Total"]) == temp)
                    {

                        strupdate = "update rank set Rank='" + ranks + "' where Rollno='" + strds.Tables[0].Rows[sti]["Rollno"].ToString() + "' and criteria_no='" + criteriano + "'";
                        temp = Convert.ToDouble(strds.Tables[0].Rows[sti]["Total"]);
                        ranks = ranks + 1;
                    }

                }

                Totcon4.Close();
                Totcon4.Open();

                SqlCommand cmd1 = new SqlCommand(strupdate, Totcon4);
                cmd1.ExecuteNonQuery();


            }
        }
    }
    //'===============================================================================================
    public void findholy()
    {
        hat.Clear();
        hat.Add("date_val", date_today);
        hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
        hat.Add("sem_val", ddlSemYr.SelectedValue.ToString());
        ds_holi = dacces2.select_method("holiday_sp", hat, "sp");
    }

    public string findabsentpresent(DateTime exam_date, string roll_no, string examcode, string subno, string mark)
    {
        try
        {
            double studpresn = 0;
            double studabsen = 0;
            double studod = 0;
            double studlev = 0;
            string srtprd = "";
            string hr = "";

            long monthyear = (Convert.ToInt64(exam_date.ToString("yyyy")) * 12) + Convert.ToInt64(exam_date.ToString("MM"));
            srtprd = GetFunction("select start_period from exam_type where exam_code='" + examcode + "'");

            if ((mark != "-3") && (mark != "-2"))
            {
                if (srtprd != string.Empty)
                {

                    lcon3.Open();
                    string sqlhour;
                    string strcalflag = "";
                    sqlhour = "select d" + exam_date.Day + "d" + srtprd + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";

                    SqlCommand cmdhour = new SqlCommand(sqlhour, lcon3);
                    SqlDataReader drhour;
                    drhour = cmdhour.ExecuteReader();
                    if (drhour.HasRows == true)
                    {
                        while (drhour.Read())
                        {
                            hr = drhour[0].ToString();
                            if (hr != string.Empty)
                            {
                                strcalflag = GetFunction("select Calcflag from AttMasterSetting where LeaveCode='" + hr.ToString() + "'");
                            }
                            if ((hr == "1"))//calc present------------------
                            {
                                if ((strcalflag == "0") && (strcalflag != null) && (strcalflag != string.Empty))
                                {

                                    studpresn += 1;
                                    if (htpresent.Contains(Convert.ToInt32(subno)))
                                    {
                                        int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htpresent));
                                        val++;//absent count
                                        htpresent[Convert.ToInt32(subno)] = val;
                                    }
                                    else
                                    {

                                        htpresent.Add(Convert.ToInt32(subno), studpresn);

                                    }
                                }
                            }
                            else//-------------calc absent------------------------
                            {

                                studabsen += 1;
                                if (htabsent.Contains(Convert.ToInt32(subno)))
                                {
                                    int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htabsent));
                                    val++;//absent count
                                    htabsent[Convert.ToInt32(subno)] = val;
                                }
                                else
                                {

                                    htabsent.Add(Convert.ToInt32(subno), studabsen);

                                }
                            }

                            if ((hr == "3"))
                            {
                                studod += 1;
                            }
                            else if (hr == "10")
                            {
                                studlev += 1;
                            }

                        }
                    }
                    else
                    {
                        studabsen += 1;
                        if (htabsent.Contains(Convert.ToInt32(subno)))
                        {
                            int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htabsent));
                            val++;//absent count
                            htabsent[Convert.ToInt32(subno)] = val;
                        }
                        else
                        {

                            htabsent.Add(Convert.ToInt32(subno), studabsen);

                        }
                    }
                    drhour.Close();
                    lcon3.Close();
                }
            }
            else if ((mark != "-3") || (mark != "-2"))
            {
                if (htpresent.Contains(Convert.ToInt32(subno)))
                {
                    int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htpresent));
                    val++;//absent count
                    htpresent[Convert.ToInt32(subno)] = val;
                }
                else
                {

                    htpresent.Add(Convert.ToInt32(subno), studpresn);

                }
            }
            string cat = studpresn.ToString() + "," + studabsen.ToString() + "," + studlev.ToString();
        }

        catch
        {
        }
        return "";
    }

    //'-------------------------------------------------------------------------------------------------
    public void apercentage()
    {
        cmd.CommandText = "Select p.No_of_hrs_per_day as 'PER DAY',p.no_of_hrs_I_half_day as 'I_HALF_DAY' ,p.no_of_hrs_II_half_day as 'II_HALF_DAY',p.min_pres_I_half_day as 'MIN PREE I DAY',p.min_pres_II_half_day as 'MIN PREE II DAY' from PeriodAttndSchedule p where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString();
        cmd.Connection = ncon1;
        ncon1.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        ncon1.Close();
        int count = ds.Tables[0].Rows.Count;

        if (ds.Tables[0].Rows.Count != 0)
        {
            perdayhrs = int.Parse((ds.Tables[0].Rows[0]["PER DAY"].ToString()));
            // for (int i = 1; i <= count; i++)
            {
                hours_pres = 0;
                hours_leave = 0;
                hours_od = 0;
                hours_absent = 0;
                hours_present = 0;
                hours_total = 0;
                hours_conduct = 0;

            }
        }
    }
    //'------------------------------------------------------------------------------------------------

    //'================================================================================================
    public void optimize(DateTime exam_date, string sno, int cno)
    {
        //double ml = 0;
        //double nss = 0;
        double eod = 0;
        double Present = 0;
        double Absent = 0;
        double Onduty = 0;
        double Leave = 0;
        string minmark = "";
        //string stu = "";
        //string per = "";

        int T = 0;


        string drpIhalf = "";
        string drpminIhalf = "";
        string drp2half = "";
        string drpmin2half = "";
        string no_of_hrs = "";
        string sqlperiod = "";
        int stud_count = 0;
        int stud_pass = 0;
        int stud_fail = 0;
        int absent = 0;

        string startprd = "";
        string endprd = "";
        double hrcnt = 0;
        double studpresn = 0;
        double studabsen = 0;
        double studod = 0;
        double studlev = 0;
        string exam_codee = "";
        string drrslt = "";
        string hr = "";
        long monthyear = (Convert.ToInt64(exam_date.ToString("yyyy")) * 12) + Convert.ToInt64(exam_date.ToString("MM"));
        sqlperiod = "Select * from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + "";
        lcon1.Close();
        lcon1.Open();
        SqlCommand cmdper = new SqlCommand(sqlperiod, lcon1);
        SqlDataReader drperiod;
        drperiod = cmdper.ExecuteReader();

        if (drperiod.Read())
        {

            drpIhalf = drperiod["no_of_hrs_I_half_day"].ToString();
            drpminIhalf = drperiod["min_pres_I_half_day"].ToString();
            drp2half = drperiod["no_of_hrs_II_half_day"].ToString();
            drpmin2half = drperiod["min_pres_II_half_day"].ToString();
            no_of_hrs = drperiod["no_of_hrs_per_day"].ToString();

        }

        drperiod.Close();
        for (int optrow = 0; optrow < ds5.Tables[0].Rows.Count; optrow++)
        {
            roll_no = ds5.Tables[0].Rows[optrow]["RollNumber"].ToString();
            hat.Clear();
            hat.Add("sno", sno.ToString());
            hat.Add("cno", cno.ToString());
            hat.Add("roll_no", roll_no.ToString());

            ds_optim = dacces2.select_method("Proc_Mark_Optimize", hat, "sp");
            //string result = "";
            //result = "select * from result,exam_type where exam_type.exam_code=result.exam_code and exam_type.subject_no=" + sno + " and exam_type.criteria_no =" + cno + " and roll_no='" + roll_no + "'";
            //lcon2.Close();
            //lcon2.Open();
            //cmd = new SqlCommand(result, lcon2);
            //SqlDataReader drresult;
            //drresult = cmd.ExecuteReader();
            if (ds_optim.Tables[0].Rows.Count > 0)
            {
                //while (drresult.Read())
                //{

                //drrslt = drresult["marks_obtained"].ToString();
                //startprd = drresult["start_period"].ToString();
                //endprd = drresult["end_period"].ToString();
                //minmark = drresult["min_mark"].ToString();
                //exam_codee = drresult["exam_code"].ToString();
                drrslt = ds_optim.Tables[0].Rows[0]["marks_obtained"].ToString();
                startprd = ds_optim.Tables[0].Rows[0]["start_period"].ToString();
                endprd = ds_optim.Tables[0].Rows[0]["end_period"].ToString();
                minmark = ds_optim.Tables[0].Rows[0]["min_mark"].ToString();
                exam_codee = ds_optim.Tables[0].Rows[0]["exam_code"].ToString();
                if (Convert.ToString(drrslt) != "-3")
                {
                    if ((startprd != string.Empty) && (endprd != string.Empty))
                    {
                        T = Convert.ToInt32(startprd);
                        hrcnt++;
                        lcon3.Open();
                        string sqlhour;
                        string strcalflag = "";
                        sqlhour = "select d" + exam_date.Day + "d" + T + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";

                        SqlCommand cmdhour = new SqlCommand(sqlhour, lcon3);
                        SqlDataReader drhour;
                        drhour = cmdhour.ExecuteReader();
                        if (drhour.HasRows == true)
                        {
                            while (drhour.Read())
                            {
                                hr = drhour[0].ToString();
                                if (hr != string.Empty)
                                {
                                    strcalflag = GetFunction("select Calcflag from AttMasterSetting where LeaveCode='" + hr.ToString() + "'");
                                }
                                if ((hr == "1"))
                                {
                                    if ((strcalflag == "0") && (strcalflag != null) && (strcalflag != string.Empty))
                                    {
                                        // //   pres += 1;
                                        studpresn += 1;
                                    }
                                }
                                else
                                {

                                    studabsen += 1;
                                }

                                if ((hr == "3"))
                                {
                                    studod += 1;
                                }
                                else if (hr == "10")
                                {
                                    studlev += 1;
                                }

                            }
                        }
                        ////else
                        ////{
                        ////    studabsen++;
                        ////}
                        drhour.Close();
                        lcon3.Close();
                        //  }
                        if (Convert.ToString(drrslt) == "-3")
                            eod = eod + 1;
                    }
                    else if ((startprd == "") && (endprd == ""))//''-------loop for startprd and endprd = empty
                    {
                        if (Convert.ToString(drrslt) != "-3")
                        {
                            for (T = 1; T <= Convert.ToInt32(drpIhalf); T++)
                            {
                                lcon3.Open();
                                string sqlhour;
                                sqlhour = "select d" + exam_date.Day + "d" + T + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";
                                SqlCommand cmdhour = new SqlCommand(sqlhour, lcon3);
                                SqlDataReader drhour;
                                drhour = cmdhour.ExecuteReader();
                                if (drhour.HasRows == true)
                                {
                                    while (drhour.Read())
                                    {
                                        hr = drhour[0].ToString();
                                        if ((hr != null) && (hr == "1") && (hr != ""))
                                        {
                                            pres += 0.5;
                                        }
                                        else if ((hr != null) && (hr == "2") && (hr != ""))
                                        {
                                            ab += 0.5;
                                        }
                                        else if ((hr != null) && (hr == "3") && (hr != ""))
                                        {
                                            OD += 0.5;
                                        }
                                        else if ((hr != null) && (hr == "10") && (hr != ""))
                                        {
                                            lev += 0.5;
                                        }
                                    }
                                }
                                drhour.Close();
                                lcon3.Close();
                            }

                            if (drpIhalf == drpminIhalf)
                            {
                                double minpres;
                                minpres = Convert.ToDouble(drpminIhalf);
                                if ((pres) == (minpres / 2))
                                {
                                    Present += 0.5;
                                }
                                if (ab >= 0.5)
                                {
                                    Absent += 0.5;
                                }
                                else
                                {
                                    Absent += 0;
                                }
                                if (OD >= 0.5)
                                {
                                    Onduty += 0.5;
                                }
                                else
                                {
                                    Onduty += 0;
                                }
                                if (lev >= 0.5)
                                {
                                    Leave += 0.5;
                                }
                                else
                                {
                                    Leave += 0;
                                }
                            }
                            else
                            {
                                if ((pres) == (Convert.ToDouble(drpIhalf) / 2))
                                {
                                    Present += 0.5;
                                }
                                if (Convert.ToDouble(ab) >= 1.0)
                                {
                                    Absent += 0.5;
                                }
                                else
                                {
                                    Absent = Absent + 0;
                                }
                                if ((OD) == ((Convert.ToDouble(drpminIhalf)) / 2))
                                {
                                    Onduty += 0.5;
                                }
                                else
                                {
                                    Onduty = Onduty + 0;
                                }
                                if (lev == ((Convert.ToDouble(drpminIhalf)) / 2))
                                {
                                    Leave += 0.5;
                                }
                                else
                                {
                                    Leave = Leave + 0;
                                }
                            }
                            ab = 0;
                            pres = 0;
                            OD = 0;
                            lev = 0;
                            for (int t = T; t <= Convert.ToInt32(no_of_hrs); t++)
                            {

                                string sqlhour1 = "";
                                sqlhour1 = "select d" + exam_date.Day + "d" + t + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";

                                lcon4.Open();
                                SqlCommand cmdhour = new SqlCommand(sqlhour1, lcon4);
                                SqlDataReader drhour;
                                drhour = cmdhour.ExecuteReader();
                                if (drhour.Read())
                                {
                                    hr = drhour[0].ToString();
                                    if ((hr != "\0") && (hr == "1") && (hr != ""))
                                    {
                                        pres += 0.5;
                                    }
                                    else if ((hr != "\0") && (hr == "2") && (hr != ""))
                                    {
                                        ab += 0.5;
                                    }
                                    else if ((hr != "\0") && (hr == "3") && (hr != ""))
                                    {
                                        OD += 0.5;
                                    }
                                    else if ((hr != "\0") && (hr == "10") && (hr != ""))
                                    {
                                        lev += 0.5;
                                    }
                                }
                                lcon4.Close();
                                drhour.Close();
                            }
                            int minpres1;
                            minpres1 = Convert.ToInt32(drpmin2half);
                            if (pres == (Convert.ToDouble(minpres1) / Convert.ToDouble(2)))
                            {
                                Present += 0.5;
                            }
                            if (drp2half.ToString() == drpmin2half.ToString())
                            {
                                if (ab >= 0.5)
                                {
                                    Absent += 0.5;
                                }
                                else
                                {
                                    Absent = Absent + 0;
                                }
                                if (OD >= 0.5)
                                {
                                    Onduty += 0.5;
                                }
                                else
                                {
                                    Onduty = Onduty + 0;
                                }
                                if ((lev) >= 0.5)
                                {
                                    Leave += (0.5);
                                }
                                else
                                {
                                    Leave = Leave + 0;
                                }
                            }
                            else
                            {
                                if ((ab) >= 1)
                                {
                                    Absent += 0.5;
                                }
                                else
                                {
                                    Absent = Absent + 0;
                                }

                                if ((OD) == (Convert.ToDouble(drpmin2half)) / 2)
                                {
                                    Onduty += 0.5;
                                }
                                else
                                {
                                    Onduty = Onduty + 0;
                                }
                                if ((lev) == (Convert.ToDouble(drpmin2half)) / 2)
                                {
                                    Leave += 0.5;
                                }
                                else
                                {
                                    Leave = Leave + 0;
                                }


                            }
                        }
                        else if (Convert.ToString(drrslt) == "-3")
                            eod = eod + 1;
                    }
                }
                ////'------------------to find the  pass fail----------------------------------------
                if (drrslt != string.Empty)
                {
                    double mark = 0.0;
                    //  if (double.TryParse(rmark.GetValue(0).ToString(), out mark))
                    if (double.TryParse(drrslt.ToString(), out mark))
                    {
                        mark = Convert.ToDouble(drrslt.ToString());
                        if (mark >= 0.0)
                        {
                            if (mark >= Convert.ToDouble(minmark))
                            {
                                stud_pass = stud_pass + 1;
                                mark_avg = mark_avg + mark;
                            }
                            else
                            {
                                stud_fail = stud_fail + 1;
                                mark_avg = mark_avg + mark;
                            }
                        }
                        else if (Math.Round(mark, 2) == -1.00)
                            absent = absent + 1;
                        else
                            stud_fail = stud_fail + 1;
                    }
                }

            }
            else
            {
                studabsen++;
            }

            //pass percentage
            tot_stud = stud_pass + stud_fail;
            pass_perc = (Convert.ToDouble(stud_pass) / Convert.ToDouble(tot_stud)) * 100.0;
            pass_perc = Math.Round(pass_perc, 2);
            //fail percentage

            fail_perc = (Convert.ToDouble(stud_fail) / Convert.ToDouble(tot_stud)) * 100.0;
            fail_perc = Math.Round(fail_perc, 2);


        }

        //avg
        mark_avg = (mark_avg / Convert.ToDouble(tot_stud));
        mark_avg = Math.Round(mark_avg, 2);
        //absent
        if ((startprd != string.Empty) && (endprd != string.Empty))
        {
            NoOfAbsent = studabsen;
        }
        else
        {
            NoOfAbsent = Absent;
        }
        //present
        if ((startprd != string.Empty) && (endprd != string.Empty))
        {

            NoOfPresent = studpresn;
        }
        else
        {
            NoOfPresent = Present;
        }
        //NoOfPresent = studpresn;
        //NoOfAbsent = studabsen;
        NoOfLe = studlev;
        NoOfOD = studod;
        NoOfPass = stud_pass;
        NoOfFail = stud_fail;

    }

    //'================================================================================================



    public void persentmonthcal_old()
    {

        int demfcal, demtcal;
        string monthcal;
        int mmyycount = 0;
        frdate = txtFromDate.Text.ToString();
        todate = txtToDate.Text.ToString();
        string dt = frdate;
        string[] dsplit = dt.Split(new Char[] { '/' });

        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demfcal = int.Parse(dsplit[2].ToString());
        demfcal = demfcal * 12;
        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
        monthcal = cal_from_date.ToString();
        dt = todate;
        dsplit = dt.Split(new Char[] { '/' });
        todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demtcal = int.Parse(dsplit[2].ToString());
        demtcal = demtcal * 12;
        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);
        dumm_from_date = per_from_date;

        hat.Clear();
        hat.Add("std_rollno", ds5.Tables[0].Rows[student]["RollNumber"].ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds6 = dacces2.select_method("STUD_ATTENDANCE", hat, "sp");

        hat.Clear();
        hat.Add("degree_code", int.Parse(ddlBranch.SelectedValue.ToString()));
        hat.Add("sem", int.Parse(ddlSemYr.SelectedItem.ToString()));
        hat.Add("from_date", frdate.ToString());
        hat.Add("to_date", todate.ToString());
        hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));

        //ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
        //mmyycount  = ds2.Tables[0].Rows.Count;
        //moncount = mmyycount  - 1;

        //------------------------------------------------------------------
        int iscount = 0;
        holidaycon.Close();
        holidaycon.Open();
        string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";
        SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
        SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
        DataSet dsholiday = new DataSet();
        daholiday.Fill(dsholiday);
        if (dsholiday.Tables[0].Rows.Count > 0)
        {
            iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
        }
        hat.Add("iscount", iscount);
        //  ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
        mmyycount = ds6.Tables[0].Rows.Count;
        moncount = mmyycount - 1;

        ds7 = dacces2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
        //------------------------------------------------------------------
        if (ds7.Tables[0].Rows.Count != 0)
        {
            ts = DateTime.Parse(ds7.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
            diff_date = Convert.ToString(ts.Days);
            dif_date1 = double.Parse(diff_date.ToString());
        }
        next = 0;

        if (ds6.Tables[0].Rows.Count != 0)
        {
            int rowcount = 0;
            int ccount;
            ccount = ds7.Tables[1].Rows.Count;
            ccount = ccount - 1;
            //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
            if (ds7.Tables[0].Rows.Count > 0)
            {
                while (dumm_from_date <= (per_to_date))
                {
                    //  for (int i = 1; i <= mmyycount; i++)
                    // {
                    if (cal_from_date == int.Parse(ds6.Tables[0].Rows[next]["month_year"].ToString()))
                    {
                        if (dumm_from_date != DateTime.Parse(ds7.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()))
                        {
                            ts = DateTime.Parse(ds7.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                            diff_date = Convert.ToString(ts.Days);
                            dif_date = double.Parse(diff_date.ToString());

                            for (i = 1; i <= fnhrs; i++)
                            {
                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                value = ds6.Tables[0].Rows[next][date].ToString();

                                if (value != null && value != "0" && value != "7" && value != "")
                                {
                                    if (tempvalue != value)
                                    {
                                        tempvalue = value;
                                        for (int j = 0; j < countds; j++)
                                        {

                                            if (ds8.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds8.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }

                                    if (ObtValue == 0)
                                    {
                                        per_perhrs += 1;
                                        tot_per_hrs += 1;
                                    }
                                    else
                                    {
                                        njhr += 1;

                                    }

                                }

                            }

                            if (per_perhrs >= minpresI)
                            {
                                Present += 0.5;
                            }


                            else if (njhr >= minpresI)
                            {
                                njdate += 0.5;
                                njhr = 0;

                            }

                            per_perhrs = 0;

                            //njhr = 0;

                            int k = i;
                            for (i = k; i <= NoHrs; i++)
                            {
                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                value = ds6.Tables[0].Rows[next][date].ToString();

                                if (value != null && value != "0" && value != "7" && value != "")
                                {
                                    if (tempvalue != value)
                                    {
                                        tempvalue = value;
                                        for (int j = 0; j < countds; j++)
                                        {

                                            if (ds8.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds8.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }
                                    if (ObtValue == 0)
                                    {
                                        per_perhrs += 1;
                                        tot_per_hrs += 1;
                                    }
                                    else
                                    {

                                        njhr += 1;

                                    }

                                }

                            }
                            if (per_perhrs >= minpresII)
                            {
                                Present += 0.5;
                            }


                            else if (njhr >= minpresII)
                            {
                                njdate += 0.5;
                                njhr = 0;
                            }

                            per_perhrs = 0;




                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                cal_from_date++;
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }

                            workingdays += 1;
                            per_perhrs = 0;

                        }
                        else
                        {
                            workingdays += 1;
                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }
                            per_holidate += 1;
                            if (ccount > rowcount)
                            {
                                rowcount++;
                            }

                        }
                    }
                    else
                    {

                        if (dumm_from_date.Day == 1)
                        {

                            //DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                            //dumm_from_date = dumm_fdate;
                            //dumm_fdate = dumm_fdate.AddMonths(1);
                            //cal_from_date++;
                            //dumm_from_date = dumm_from_date.AddDays(1);
                            DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                            dumm_from_date = dumm_fdate;
                            dumm_from_date = dumm_from_date.AddMonths(1);
                            cal_from_date++;
                            if (moncount > next)
                            {
                                next++;
                                i++;
                            }

                        }

                        if (moncount > next)
                        {
                            i--;
                        }
                    }

                    // }
                }//'----end while
            }
            int diff_Date = per_from_date.Day - dumm_from_date.Day;
            student++;
        }

        per_tot_ondu = tot_ondu;
        per_njdate = njdate;
        pre_present_date = Present;
        per_per_hrs = tot_per_hrs;
        per_absent_date = Absent;
        pre_ondu_date = Onduty;
        pre_leave_date = Leave;
        per_workingdays = workingdays - per_holidate - per_njdate;
        per_dum_unmark = dum_unmark;

        Present = 0;
        tot_per_hrs = 0;
        Absent = 0;
        Onduty = 0;
        Leave = 0;
        workingdays = 0;
        per_holidate = 0;
        dum_unmark = 0;
        absent_point = 0;
        leave_point = 0;
        njdate = 0;
        tot_ondu = 0;
    }



    public void persentmonthcal(int ival)
    {

        Boolean isadm = false;
        // try
        {
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;

            notconsider_value = 0;
            conduct_hour_new = 0;

            //Opt--------

            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;


            //-----------
            dumm_from_date = per_from_date;

            string admdate = ds5.Tables[0].Rows[ival]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            //dd = ds5.Tables[0].Rows[student]["RollNumber"].ToString();
            hat.Clear();
            hat.Add("std_rollno", ds5.Tables[0].Rows[ival]["RollNumber"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds6 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds6.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (ival == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(ddlBranch.SelectedValue.ToString()));
                hat.Add("sem", int.Parse(ddlSemYr.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds7 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                holiday_table11.Clear();
                holiday_table21.Clear();
                holiday_table31.Clear();
                if (ds7.Tables[0].Rows.Count != 0)
                {
                    for (int k = 0; k < ds7.Tables[0].Rows.Count; k++)
                    {
                        if (ds7.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds7.Tables[0].Rows[0]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds7.Tables[0].Rows[0]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        string[] split_date_time1 = ds7.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }

                if (ds7.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds7.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds7.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                        if (ds7.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds7.Tables[1].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds7.Tables[1].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }

                if (ds7.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds7.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds7.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                        if (ds7.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds7.Tables[2].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds7.Tables[2].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }

                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }



            }

            //------------------------------------------------------------------
            if (ds7.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds7.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds6.Tables[0].Rows.Count != 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = ds7.Tables[1].Rows.Count;
                ccount = ccount - 1;
                //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                while (dumm_from_date <= (per_to_date))
                {
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;
                        if (splhr_flag == true)
                        {
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            if (cal_from_date == int.Parse(ds6.Tables[0].Rows[next]["month_year"].ToString()))
                            {
                                string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');


                                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                                }

                                if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    value_holi_status = GetCorrespondingKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                    split_holiday_status = value_holi_status.Split('*');

                                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "1";
                                    }
                                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                    {
                                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                        {
                                            split_holiday_status_1 = "0";
                                            split_holiday_status_2 = "1";
                                        }

                                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                        {
                                            split_holiday_status_1 = "1";
                                            split_holiday_status_2 = "0";
                                        }
                                    }
                                    else if (split_holiday_status[0].ToString() == "0")
                                    {
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                        if (dumm_from_date.Day == 1)
                                        {
                                            cal_from_date++;
                                            if (moncount > next)
                                            {
                                                next++;

                                            }
                                        }
                                        break;
                                    }

                                    if (ds7.Tables[1].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds7.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                        diff_date = Convert.ToString(ts.Days);
                                        dif_date = double.Parse(diff_date.ToString());
                                    }
                                    else
                                    {
                                        dif_date = 0;
                                    }
                                    if (dif_date == 1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                    }
                                    else if (dif_date == -1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                        if (ccount > rowcount)
                                        {
                                            rowcount += 1;
                                        }
                                    }
                                    else
                                    {
                                        leave_pointer = leav_pt;
                                        absent_pointer = absent_pt;

                                    }

                                    if (ds7.Tables[2].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds7.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                        diff_date = Convert.ToString(ts.Days);
                                        dif_date = double.Parse(diff_date.ToString());
                                        if (dif_date == 1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
                                        }

                                    }
                                    if (dif_date1 == -1)
                                    {
                                        leave_pointer = holi_leav;
                                        absent_pointer = holi_absent;
                                    }
                                    dif_date1 = 0;
                                    if (split_holiday_status_1 == "1")
                                    {

                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds6.Tables[0].Rows[next][date].ToString();

                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;
                                                    for (int j = 0; j < countds; j++)
                                                    {

                                                        if (ds8.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds8.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                            j = countds;
                                                        }
                                                    }
                                                }
                                                if (ObtValue == 1)
                                                {
                                                    per_abshrs += 1;
                                                }
                                                else if (ObtValue == 2)
                                                {
                                                    notconsider_value += 1;
                                                    njhr += 1;
                                                }
                                                else if (ObtValue == 0)
                                                {
                                                    per_perhrs += 1;
                                                    tot_per_hrs += 1;
                                                }
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
                                                }
                                                else if (value == "10")
                                                {
                                                    per_leave += 1;
                                                }
                                                else if (value == "4")
                                                {
                                                    tot_ml += 1;
                                                }

                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;

                                            }
                                            else
                                            {
                                                unmark += 1;
                                                temp_unmark++;

                                                my_un_mark++;//added 080812
                                            }
                                        }

                                        //  if (per_perhrs >= minpresI)
                                        if (per_perhrs + njhr >= minpresI)
                                        {
                                            Present += 0.5;
                                        }

                                        else if (per_leave >= 1)
                                        {
                                            leave_point += leave_pointer / 2;
                                            Leave += 0.5;
                                        }

                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                        }
                                        if (njhr >= minpresI)
                                        {
                                            njdate += 0.5;
                                            njdate_mng += 1;
                                        }
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }

                                        if (temp_unmark == fnhrs)
                                        {
                                            per_holidate_mng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark = temp_unmark;
                                        }
                                        if (fnhrs - temp_unmark >= minpresI)
                                        {
                                            workingdays += 0.5;
                                        }
                                        mng_conducted_half_days += 1;


                                    }
                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    per_abshrs = 0;
                                    //   unmark = 0;
                                    temp_unmark = 0;
                                    njhr = 0;

                                    int k = fnhrs + 1;

                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds6.Tables[0].Rows[next][date].ToString();

                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;
                                                    for (int j = 0; j < countds; j++)
                                                    {

                                                        if (ds8.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds8.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                            j = countds;
                                                        }
                                                    }
                                                }
                                                if (ObtValue == 1)
                                                {
                                                    per_abshrs += 1;
                                                }
                                                else if (ObtValue == 2)
                                                {
                                                    notconsider_value += 1;
                                                    njhr += 1;
                                                }
                                                else if (ObtValue == 0)
                                                {
                                                    per_perhrs += 1;
                                                    tot_per_hrs += 1;
                                                }
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
                                                }
                                                else if (value == "10")
                                                {
                                                    per_leave += 1;
                                                }
                                                if (value == "4")
                                                {
                                                    tot_ml += 1;
                                                }
                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;
                                            }
                                            else
                                            {
                                                unmark += 1;
                                                temp_unmark++;

                                                my_un_mark++; //added 080812
                                            }
                                        }
                                        //   if (per_perhrs >= minpresII)
                                        if (per_perhrs + njhr >= minpresII)
                                        {
                                            Present += 0.5;
                                        }

                                        else if (per_leave >= 1)
                                        {

                                            leave_point += leave_pointer / 2;
                                            Leave += 0.5;
                                        }

                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                        }
                                        if (njhr >= minpresII)
                                        {
                                            njdate_evng += 1;
                                            njdate += 0.5;
                                        }
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }



                                        if (temp_unmark == NoHrs - fnhrs)
                                        {
                                            per_holidate_evng += 1;
                                            per_holidate += 0.5;

                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark += unmark;
                                        }


                                        if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                        {
                                            workingdays += 0.5;
                                        }
                                        evng_conducted_half_days += 1;


                                    }

                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    per_abshrs = 0;
                                    unmark = 0; //hided
                                    njhr = 0;


                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;

                                        }
                                    }


                                    per_perhrs = 0;

                                }

                            }
                            else
                            {

                                //DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                                //dumm_fdate = dumm_fdate.AddMonths(1);
                                //dumm_from_date = dumm_fdate;
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {

                                    cal_from_date++;


                                    if (moncount > next)
                                    {
                                        next++; //  next++;
                                    }

                                }

                                //if (moncount > next)
                                //{
                                //    i--;
                                //}
                            }

                        }
                    }
                    if (isadm == false)
                    {
                        dumm_from_date = dumm_from_date.AddDays(1);
                        if (dumm_from_date.Day == 1)
                        {
                            cal_from_date++;
                            if (moncount > next)
                            {
                                next++;

                            }
                        }
                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }



            per_tot_ondu = tot_ondu;
            per_tot_ml = tot_ml;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            // per_workingdays = workingdays - per_holidate - per_njdate;
            per_workingdays = workingdays - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

            //  per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) -  notconsider_value - dum_unmark;
            per_dum_unmark = dum_unmark; //hided on 08.08.12

            Present = 0;
            tot_per_hrs = 0;
            Absent = 0;
            Onduty = 0;
            Leave = 0;
            workingdays = 0;
            per_holidate = 0;
            dum_unmark = 0;
            absent_point = 0;
            leave_point = 0;
            njdate = 0;
            tot_ondu = 0;
            tot_ml = 0;
        }
        //   catch
        {
        }
    }
    public string filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "ORDER BY r.Roll_No";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
            }
        }
        return strorder;
    }






    //-----------------------------------------------func to get the hash key---------------------------------
    public void SpreadBind()
    {
        try
        {
            camtable.Clear();
            colcnt = 0;
            datatablecount = "";
            subarray.Clear();
            int nothiddencount = 1;
            //  btnPrint.Visible = true;
            btnExcel.Visible = true;
            btnprintmaster.Visible = true;
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            int hasrow_count = 0;
            Radiowithoutheader.Visible = false;
            RadioHeader.Visible = false;

            gridviewload.Visible = true;

            filteration();



            string resminmrk = "";
            string subject_code = "";
            int[] maxtot = new int[100];
            string examdate = "";
            string subname = "";
            int rankcount = 0;
            int subjectfailedcount = 0;
            int serialno = 0;
            string subacron = "";
            int max = 0;


            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlBranch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSemYr.SelectedValue.ToString();
            criteria_no = ddlTest.SelectedValue.ToString();

            //'------------------------------------------------------------
            //  string sqlStr = "";


            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = sections.ToString();
            }


            //FpEntry.Sheets[0].RowCount = 0;
            //FpEntry.Sheets[0].ColumnCount = 6;
            //FpEntry.Sheets[0].ColumnHeader.RowCount = 1;//02.03.12 clmncount 7
            string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + strsec.ToString() + "' and et.sections='" + strsec.ToString() + "' " + strorder + ",s.subject_no";
            string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";
            hat.Clear();
            hat.Add("batchyear", batch.ToString());
            hat.Add("degreecode", degreecode.ToString());
            hat.Add("criteria_no", criteria_no.ToString());
            hat.Add("sections", strsec.ToString());
            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());

            ds2 = d2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
            double dum_tage_date = 0;
            double dum_tage_hrs = 0;

            hat.Clear();
            hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
            hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
            ds7 = dacces2.select_method("period_attnd_schedule", hat, "sp");
            if (ds7.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds7.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds7.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds7.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds7.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds7.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds8 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
            countds = ds8.Tables[0].Rows.Count;


            //opt-------

            //----------

            if (ds2.Tables[0].Rows.Count != 0)
            {
                filteration();
                string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "' and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";  //modified by Mullai
                string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                hat.Clear();
                hat.Add("bath_year", batch.ToString());
                hat.Add("degree_code", degreecode.ToString());
                hat.Add("sec", strsec.ToString());
                hat.Add("filterwithsectionsub", filterwithsectionsub.ToString());
                hat.Add("filterwithoutsectionsub", filterwithoutsectionsub.ToString());
                ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");

                //if (Session["Rollflag"].ToString() == "1")
                //{
                    colcnt++;
                    camtable.Columns.Add("RollNo");
             //   }
                //if (Session["Regflag"].ToString() == "1")
              //  {
                    colcnt++;
                    camtable.Columns.Add("RegNo");
               // }
                camtable.Columns.Add("Student Name");
                camtable.Columns.Add("StudentType");
                camtable.Columns.Add("ApplicationNumber");
                if (ds5.Tables[0].Rows.Count != 0)
                {
                    if (ds5.Tables[0].Rows.Count > 0)
                    {
                        int c = 0;
                        drvoice = camtable.NewRow();

                        for (int irow = 0; irow < ds5.Tables[0].Rows.Count; irow++)
                        {
                            serialno++;
                            c++;
                            drvoice = camtable.NewRow();


                          //  if (Session["Rollflag"].ToString() == "1")
                                drvoice["RollNo"] = ds5.Tables[0].Rows[irow]["RollNumber"].ToString();
                          //  if (Session["Regflag"].ToString() == "1")
                                drvoice["RegNo"] = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                            drvoice["Student Name"] = ds5.Tables[0].Rows[irow]["Student_Name"].ToString();
                            drvoice["StudentType"] = ds5.Tables[0].Rows[irow]["StudentType"].ToString();
                            drvoice["ApplicationNumber"] = ds5.Tables[0].Rows[irow]["ApplicationNumber"].ToString();
                            camtable.Rows.Add(drvoice);
                        }

                    }


                    //if (Session["Rollflag"].ToString() == "0")
                    //{
                    //    // gridviewload.Columns[2].Visible = false;
                    //    nothiddencount = 2;

                    //}
                    //if (Session["Regflag"].ToString() == "0")
                    //{
                    //    // gridviewload.Columns[3].Visible = false;
                    //    nothiddencount = 1;

                    //}
                    colcnt = colcnt + 1;
                    //if (Session["Studflag"].ToString() == "0")
                    //{
                        // gridviewload.Columns[4].Visible = false;
                       // camtable.Columns.Remove("StudentType");

                  //  }

                    camtable.Columns.Remove("ApplicationNumber");

                }
                hasrow_count = hasrow_count + 1;

                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                {

                    subno = ds2.Tables[1].Rows[i]["subject_no"].ToString();
                    subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                    //resmaxmrk = ds2.Tables[1].Rows[i]["max_mark"].ToString();
                    resminmrk = ds2.Tables[1].Rows[i]["min_mark"].ToString();
                    //resduration = ds2.Tables[1].Rows[i]["duration"].ToString();
                    exam_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                    examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                    subname = ds2.Tables[1].Rows[i]["subject_name"].ToString();
                    subacron = ds2.Tables[1].Rows[i]["acronym"].ToString();
                    subarray.Add(subacron);
                    max = Convert.ToInt32(ds2.Tables[1].Rows[i]["max_mark"]);
                    if (maxmark == 0)
                    {
                        maxmark = max;
                    }
                    else
                    {
                        maxmark = maxmark + max;
                    }
                    //x1 = FpEntry.Sheets[0].ColumnCount;
                    //FpEntry.Sheets[0].ColumnCount = Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) + 1;
                    //int incr = FpEntry.Sheets[0].ColumnCount - 1;
                    camtable1.Columns.Add(examdate + "@" + exam_code);

                    camtable.Columns.Add(subject_code);

                    camtable1.Columns.Add(subno + "@" + subname + "@" + subject_code);
                    htdate.Add(subject_code, examdate);
                    count++;
                }
            }
            //FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 2;
            //int totalcount = FpEntry.Sheets[0].ColumnCount - 2;
            camtable.Columns.Add("Total");


            camtable.Columns.Add("Percentage");





            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            hat.Add("form_name", "CAMrpt.aspx");
            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
                {
                    collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
                }
                if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
                {
                    address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
                    address = address1;
                }
                if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
                {
                    address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
                    address = address1 + "-" + address2;

                }
                if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
                {
                    district = dsprint.Tables[0].Rows[0]["address3"].ToString();
                    address = address1 + "-" + address2 + "-" + district;
                }

                if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
                {
                    Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno;
                }
                if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
                {
                    Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
                    phnfax = phnfax + "Fax  :" + " " + Faxno;
                }

                if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
                {
                    email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
                }
                if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
                {
                    email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
                }
                if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
                {
                    form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
                }
                if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
                {
                    batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
                }

            }
            //'------------------------------------load the clg information
            else if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
                SqlCommand collegecmd = new SqlCommand(college, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {

                    while (collegename.Read())
                    {
                        collnamenew1 = collegename["collname"].ToString();
                        address1 = collegename["address1"].ToString();
                        address2 = collegename["address2"].ToString();
                        district = collegename["district"].ToString();
                        address = address1 + "-" + address2 + "-" + district;
                        Phoneno = collegename["phoneno"].ToString();
                        Faxno = collegename["faxno"].ToString();
                        phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                        email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                    }
                }
                con.Close();
            }
            //'---------------------------------------------------------------------------------------------------------


            if (sections == null)
            {
                sections = "";
            }


            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            //FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            //FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            //FpEntry.Sheets[0].AllowTableCorner = true;




            int EL = 0;


            ds3 = d2.select_method_wo_parameter("Delete_Rank_Table", "sp");


            string marks_per;
            int stu_count = 0;


            per_sub_count = ds2.Tables[1].Rows.Count;

            if (ds2.Tables[0].Rows.Count != 0)
            {
                DataView dv_indstudmarks = new DataView();
                int k = 1;
                for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                {

                    tot_marks = 0;
                    per_marks = 0;
                    sub_max_marks = 0;
                    int sstat = 0;
                    for (int j = 0; j < per_sub_count; j++)
                    {

                        if (stu_count < ds2.Tables[0].Rows.Count)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds5.Tables[0].Rows[i]["RollNumber"].ToString() + "' and subject_no='" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + "'";
                            dv_indstudmarks = ds2.Tables[0].DefaultView;
                            if (dv_indstudmarks.Count > 0)
                            {
                                for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
                                {
                                    sstat++;
                                    double outof100 = 0;
                                    double testMaxMarks = double.Parse(dv_indstudmarks[cnt]["max_mark"].ToString());
                                    double marks = double.Parse(dv_indstudmarks[cnt]["mark"].ToString());
                                    marks_per = dv_indstudmarks[cnt]["mark"].ToString();
                                    string min_marksstring = dv_indstudmarks[cnt]["min_mark"].ToString();
                                    if (min_marksstring != "")
                                    {
                                        min_mark = int.Parse(min_marksstring.ToString());
                                    }
                                    else
                                    {
                                        min_mark = 0;
                                    }
                                    marks_per = marks.ToString();
                                    camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = marks_per;
                                    marks_per = dv_indstudmarks[cnt]["mark"].ToString();
                                    if (marks != 0 && testMaxMarks > 0)
                                        outof100 = Math.Round((marks / testMaxMarks) * 100, MidpointRounding.AwayFromZero);

                                    string grade = d2.GetFunction("select isnull(Mark_Grade,'-') Mark_Grade from Grade_Master where College_Code='" + Session["collegecode"].ToString().Trim() + "' and Degree_Code='" + degreecode + "' and batch_year='" + batch + "'  and  Frange<='" + outof100 + "' and Trange>='" + outof100 + "'");


                                    switch (marks_per)
                                    {
                                        case "-1":

                                            marks_per = "AAA";
                                            break;
                                        case "-2":
                                            marks_per = "EL";
                                            break;
                                        case "-3":
                                            marks_per = "EOD";
                                            break;
                                        case "-4":
                                            marks_per = "ML";
                                            break;
                                        case "-5":
                                            marks_per = "SOD";
                                            break;
                                        case "-6":
                                            marks_per = "NSS";
                                            break;
                                        case "-7":
                                            marks_per = "NJ";
                                            break;
                                        case "-8":
                                            marks_per = "S";
                                            break;
                                        case "-9":
                                            marks_per = "L";
                                            break;
                                        case "-10":
                                            marks_per = "NCC";
                                            break;
                                        case "-11":
                                            marks_per = "HS";
                                            break;
                                        case "-12":
                                            marks_per = "PP";
                                            break;
                                        case "-13":
                                            marks_per = "SYOD";
                                            break;
                                        case "-14":
                                            marks_per = "COD";
                                            break;
                                        case "-15":
                                            marks_per = "OOD";
                                            break;
                                        case "-16":
                                            marks_per = "OD";
                                            break;
                                        case "-17":
                                            marks_per = "LA";
                                            break;
                                        //Modified by subburaj 20/08/2014*******//
                                        case "-18":
                                            marks_per = "RAA";
                                            break;
                                        //*********end********//
                                    }
                                    if (marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;
                                    }
                                    if (marks >= 0 && (Convert.ToString(marks) != string.Empty))
                                    {
                                        per_mark += marks;
                                        sub_max_marks += double.Parse(dv_indstudmarks[cnt]["max_mark"].ToString());
                                    }
                                    if (marks >= min_mark || marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;

                                        if (rblsmstype.SelectedIndex == 0)

                                            camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = marks_per.ToString();
                                        else
                                            if (grade == "" || grade == "0")
                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = "-";
                                            else
                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = grade.ToString();
                                    }
                                    else
                                    {
                                        fail++;


                                        if (marks >= 0)
                                        {
                                            if (rblsmstype.SelectedIndex == 0)

                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = marks_per.ToString();
                                            else
                                                if (grade == "" || grade == "0")
                                                    camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = "-";
                                                else
                                                    camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = grade.ToString();
                                            //FpEntry.Sheets[0].Cells[k, j + 6].ForeColor = Color.Red;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Underline = true;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Name = "Book Antiqua";
                                            //FpEntry.Sheets[0].Cells[k, j + 6].HorizontalAlign = HorizontalAlign.Center;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Size = FontUnit.Medium;

                                        }
                                        else
                                        {
                                            if (rblsmstype.SelectedIndex == 0)
                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = marks_per.ToString();
                                            else
                                                if (grade == "" || grade == "0")
                                                    camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = "-";
                                                else
                                                    camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = grade.ToString();
                                            //FpEntry.Sheets[0].Cells[k, j + 6].ForeColor = Color.Red;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Underline = true;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Name = "Book Antiqua";
                                            //FpEntry.Sheets[0].Cells[k, j + 6].HorizontalAlign = HorizontalAlign.Center;
                                            //FpEntry.Sheets[0].Cells[k, j + 6].Font.Size = FontUnit.Medium;

                                            marks = 0;
                                        }




                                    }

                                    if (marks < 0 && marks_per != "EL" && marks_per != "EOD")
                                    {
                                        if (rblsmstype.SelectedIndex == 0)
                                            camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = marks_per.ToString();
                                        else
                                            if (grade == "" || grade == "0")
                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = "-";
                                            else

                                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = grade.ToString();
                                        //FpEntry.Sheets[0].Cells[k, j + 6].ForeColor = Color.Red;
                                        //FpEntry.Sheets[0].Cells[k, j + 6].Font.Underline = true;
                                        //FpEntry.Sheets[0].Cells[k, j + 6].Font.Name = "Book Antiqua";
                                        //FpEntry.Sheets[0].Cells[k, j + 6].HorizontalAlign = HorizontalAlign.Center;
                                        //FpEntry.Sheets[0].Cells[k, j + 6].Font.Size = FontUnit.Medium;

                                    }


                                    tot_marks += marks;
                                    EL = 0;
                                    stu_count++;


                                }
                            }
                            else
                            {
                                camtable.Rows[i][ds2.Tables[1].Rows[j]["subject_code"].ToString()] = "--";
                            }
                        }






                        if (EL == 0)
                        {
                            if (sstat == 0 || fail != 0)
                            {
                                pass_fail = "FAIL";
                            }
                            else
                            {
                                pass_fail = "PASS";
                            }
                        }
                        if (tot_marks > 0)
                        {
                            per_marks = ((tot_marks / sub_max_marks) * 100);
                            per_tage = String.Format("{0:0,0.00}", float.Parse(per_marks.ToString()));
                        }
                        else
                        {
                            tot_marks = 0;
                            per_marks = 0;
                            per_tage = "0";
                        }
                        if (per_tage == "NaN")
                        {
                            per_tage = "0";
                        }
                        else if (per_tage == "Infinity")
                        {
                            per_tage = "0";
                        }
                        string tr = Convert.ToString(tot_marks);

                        drvoice["Total"] = tot_marks.ToString();
                        camtable.Rows[i]["Total"] = tr;
                        drvoice["Percentage"] = per_tage.ToString();
                        camtable.Rows[i]["Percentage"] = per_tage.ToString();
                        k++;
                    }
                }

                gridviewload.DataSource = camtable;
                gridviewload.DataBind();
                gridviewload.Visible = true;
                datatablecount = camtable.Columns.Count.ToString();

                //gridviewload.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //gridviewload.Rows[0].Font.Bold = true;
                //gridviewload.Rows[0].HorizontalAlign = HorizontalAlign.Center;


            }
            else
            {
                lblnorec.Text = "Test has not been conducted for any subject";
                lblnorec.Visible = true;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                lblerror.Visible = false;
                datatablecount = "";
                colcnt = 0;
            }


        }
        catch
        {
        }

    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {

        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }

        return null;
    }

    public double findgrade(string rol_no, int semval)
    {
        int sem = semval;
        double gpacal = 0.0;
        double gpacal2 = 0.0;
        double examsys = 0.0;
        double gpa = 0.0;
        double grpoints = 0.0;
        double grcredit = 0.0;
        double cgpa1 = 0.0;

        double gpa1 = 0.0;
        double grcredit1 = 0.0;
        string sql = "select exam_system,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
        con.Close();
        con.Open();
        SqlCommand cgpa = new SqlCommand(sql, con);
        SqlDataReader cgpardr;
        cgpardr = cgpa.ExecuteReader();
        if (cgpardr.HasRows)
        {
            examsys = 0;
            while (cgpardr.Read())
            {
                if (examsys == 0.0)
                {
                    for (int i = 1; i <= sem; i++)
                    {
                        gpa = 0.0;
                        grpoints = 0.0;
                        grcredit = 0.0;
                        gpa1 = 0.0;
                        grcredit1 = 0.0;
                        int examcode = getunivcode(Convert.ToInt32(ddlBranch.SelectedValue.ToString()), i, Convert.ToInt32(ddlBatch.SelectedValue.ToString()));

                        //string sql1 = "select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " +examcode+ " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" +rol_no+ "'";

                        mycon.Close();
                        mycon.Open();
                        SqlCommand mark = new SqlCommand();
                        mark.CommandType = CommandType.StoredProcedure;
                        mark.CommandText = "Proc_Field_MarkEntry";
                        mark.Connection = mycon;
                        mark.Parameters.Add("@exam_code", SqlDbType.NVarChar).Value = examcode.ToString();
                        mark.Parameters.Add("@rol_no", SqlDbType.NVarChar).Value = rol_no.ToString();
                        SqlDataReader markdr;
                        markdr = mark.ExecuteReader();
                        if (markdr.HasRows)
                        {
                            while (markdr.Read())
                            {
                                string mgrade = markdr["grade"].ToString();
                                if (mgrade != "")
                                {
                                    // mgrade = "-";
                                    //   string sql2 = "select top 1 credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + ddlBranch.SelectedValue.ToString() + "";
                                    mycon1.Close();
                                    mycon1.Open();
                                    SqlCommand credit = new SqlCommand();
                                    credit.CommandType = CommandType.StoredProcedure;
                                    credit.CommandText = "Proc_Credit_Points";
                                    credit.Connection = mycon1;
                                    credit.Parameters.Add("@mgrade", SqlDbType.NVarChar).Value = mgrade.ToString();
                                    credit.Parameters.Add("@degcode", SqlDbType.NVarChar).Value = ddlBranch.SelectedValue.ToString();

                                    SqlDataReader creditdr;
                                    creditdr = credit.ExecuteReader();
                                    if (creditdr.HasRows)
                                    {
                                        while (creditdr.Read())
                                        {
                                            grpoints = Convert.ToDouble(creditdr["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0.0;
                                    }

                                }
                                int subno = Convert.ToInt32(markdr["subject_no"].ToString());
                                string sql3 = "select isnull(credit_points,' ') from subject where subject_no= " + subno + "";
                                setcon.Close();
                                setcon.Open();
                                SqlCommand credit1 = new SqlCommand(sql3, setcon);
                                SqlDataReader creditdr1;
                                creditdr1 = credit1.ExecuteReader();
                                if (creditdr1.HasRows)
                                {

                                    while (creditdr1.Read())
                                    {
                                        grcredit = Convert.ToDouble(creditdr1[0].ToString());
                                        grcredit1 = grcredit1 + grcredit;
                                    }
                                }
                                else
                                {
                                    grcredit = 0.0;
                                }
                                gpa = grpoints * grcredit;
                                gpa1 = gpa1 + gpa;

                            }
                        }
                        if (grcredit1 != 0.0)
                        {
                            gpacal = gpa1 / grcredit1;
                        }
                        else
                        {
                            gpacal = 0.0;
                        }

                        gpacal2 = gpacal2 + gpacal;
                    }
                    cgpa1 = gpacal2 / sem;
                }
                else
                {
                    for (int j = 1; j <= sem; j++)
                    {
                        if (j == 2)
                        {
                            break;
                        }
                        gpa = 0.0;
                        grpoints = 0.0;
                        grcredit = 0.0;
                        gpa1 = 0.0;
                        grcredit1 = 0.0;
                        int examcode = getunivcode(Convert.ToInt32(ddlBranch.SelectedValue.ToString()), j, Convert.ToInt32(ddlBatch.SelectedValue.ToString()));

                        //  string sql1 = "select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " +examcode+ " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" +rol_no+ "'";
                        mycon.Close();
                        mycon.Open();
                        SqlCommand mark = new SqlCommand();
                        mark.CommandType = CommandType.StoredProcedure;
                        mark.CommandText = "Proc_Field_MarkEntry";
                        mark.Connection = mycon;
                        mark.Parameters.Add("@exam_code", SqlDbType.NVarChar).Value = examcode.ToString();
                        mark.Parameters.Add("@rol_no", SqlDbType.NVarChar).Value = rol_no.ToString();
                        SqlDataReader markdr;
                        markdr = mark.ExecuteReader();
                        if (markdr.HasRows)
                        {

                            while (markdr.Read())
                            {
                                string mgrade = markdr["grade"].ToString();
                                if (mgrade == "")
                                {
                                    mgrade = "-";

                                    //    string sql2 = "select top 1 credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + ddlBranch.SelectedValue.ToString() + "";
                                    mycon1.Close();
                                    mycon1.Open();
                                    SqlCommand credit = new SqlCommand();
                                    credit.CommandType = CommandType.StoredProcedure;
                                    credit.CommandText = "Proc_Credit_Points";
                                    credit.Connection = mycon1;
                                    credit.Parameters.Add("@mgrade", SqlDbType.NVarChar).Value = mgrade.ToString();
                                    credit.Parameters.Add("@degcode", SqlDbType.NVarChar).Value = ddlBranch.SelectedValue.ToString();

                                    SqlDataReader creditdr;
                                    creditdr = credit.ExecuteReader();
                                    if (creditdr.HasRows)
                                    {

                                        while (creditdr.Read())
                                        {
                                            grpoints = Convert.ToDouble(creditdr["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0.0;
                                    }

                                }
                                int subno = Convert.ToInt32(markdr["subject_no"].ToString());
                                string sql3 = "select isnull(credit_points,' ') from subject where subject_no= " + subno + "";
                                setcon.Close();
                                setcon.Open();
                                SqlCommand credit1 = new SqlCommand(sql3, setcon);
                                SqlDataReader creditdr1;
                                creditdr1 = credit1.ExecuteReader();
                                if (creditdr1.HasRows)
                                {

                                    while (creditdr1.Read())
                                    {
                                        grcredit = Convert.ToDouble(creditdr1[0].ToString());
                                        grcredit1 = grcredit1 + grcredit;
                                    }
                                }
                                else
                                {
                                    grcredit = 0.0;
                                }
                                gpa = grpoints * grcredit;
                                gpa1 = gpa1 + gpa;

                            }
                            gpacal = gpa1 / grcredit1;
                            gpacal2 = gpacal2 + gpacal;

                        }
                    }

                    cgpa1 = gpacal2 / (sem - 1);

                }
            }
        }
        return cgpa1;
    }

    //'-----------------------------------func used to find the cgpa---------------------------------
    public int getunivcode(int degreecode, int sem, int batch)
    {

        int x = -1;
        string sqlcode = "Select Exam_Code from Exam_Details where Degree_Code = " + degreecode + " and Current_Semester = " + sem + " and Batch_Year = " + batch + "";
        mycon1.Close();
        mycon1.Open();
        SqlCommand code = new SqlCommand(sqlcode, mycon1);
        SqlDataReader codedr;
        codedr = code.ExecuteReader();
        if (codedr.HasRows)
        {
            while (codedr.Read())
            {

                x = Convert.ToInt32(codedr["exam_code"].ToString());
            }
        }

        return x;

    }
    public static bool IsNumeric(string s)
    {
        double Result;
        return double.TryParse(s, out Result);

    }

    public string result(string st)
    {
        con.Close();
        con.Open();
        string result = "";
        SqlDataReader drr;
        SqlCommand commmand = new SqlCommand(st, con);
        drr = commmand.ExecuteReader();


        if (drr.HasRows == true)
        {
            while (drr.Read())
            {
                if (drr[0] != null)
                {
                    result = drr[0].ToString();
                }
                else
                {
                    result = "0";
                }
            }
        }
        else if (drr.HasRows == false)
        {
            result = "";
        }

        return result;
    }
    public double roundresult(string nstr)
    {
        con.Close();
        con.Open();
        double roundresult;
        if ((nstr) != "")
        {

            double ag1;
            ag1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(nstr), 2));

            roundresult = ag1;
        }
        else
        {
            roundresult = 0;
        }
        return roundresult;
    }
    //*****
    private string Splitter(string p, string p_2)
    {
        throw new NotImplementedException();
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        //lblEduration.Visible = false;
        lblnorec.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblerror.Visible = false;

        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        ddlTest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        BindSectionDetail();
        binddate();
    }
    public void binddate()
    {
        con.Close();
        con.Open();
        string from_date = "";
        string to_date = "";
        string final_from = "";
        string final_to = "";
        SqlDataReader dr_dateset;
        cmd = new SqlCommand("select start_date,end_date from seminfo where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " ", con);
        dr_dateset = cmd.ExecuteReader();
        dr_dateset.Read();
        if (dr_dateset.HasRows == true)
        {

            //------------get from date
            from_date = dr_dateset[0].ToString();
            string[] from_split = from_date.Split(' ');
            string[] date_split_from = from_split[0].Split('/');
            final_from = date_split_from[1] + "/" + date_split_from[0] + "/" + date_split_from[2];

            string sem_start = final_from;
            txtFromDate.Text = final_from;

            //------------get to date
            to_date = dr_dateset[1].ToString();
            string[] to_split = to_date.Split(' ');
            string[] date_split_to = to_split[0].Split('/');
            final_to = date_split_to[1] + "/" + date_split_to[0] + "/" + date_split_to[2];
            txtToDate.Text = final_to;


        }
    }
    protected void gridviewload_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridviewload.PageIndex = e.NewPageIndex;
        btnGo_Click(sender, e);
    }
    protected void gridviewload_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
                "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");


        
            if (Session["Rollflag"].ToString() == "0")
                e.Row.Cells[2].Visible = false;
            else
                e.Row.Cells[2].Visible = true;
            if (Session["Regflag"].ToString() == "0")
                e.Row.Cells[3].Visible = false;
            else
                e.Row.Cells[3].Visible = true;
            if (Session["Studflag"].ToString() == "0")
                e.Row.Cells[5].Visible = false;
            else
                e.Row.Cells[5].Visible = true;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Session["Rollflag"].ToString() == "0")
                e.Row.Cells[2].Visible = false;
            else
                e.Row.Cells[2].Visible = true;
            if (Session["Regflag"].ToString() == "0")
                e.Row.Cells[3].Visible = false;
            else
                e.Row.Cells[3].Visible = true;
            if (Session["Studflag"].ToString() == "0")
                e.Row.Cells[5].Visible = false;
            else
                e.Row.Cells[5].Visible = true;
        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        buttonG0();
    }
    protected void buttonG0()
    {
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        // FpEntry.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        //FpEntry.CurrentPage = 0;
        int indexcnt = 0;

        //------------------------------------------date validation-------------------------------
        string valfromdate = "";
        string valtodate = "";
        string frmconcat = "";


        valfromdate = txtFromDate.Text.ToString();
        string[] split1 = valfromdate.Split(new char[] { '/' });
        frmconcat = split1[1].ToString() + '/' + split1[0].ToString() + '/' + split1[2].ToString();
        DateTime dtfromdate = Convert.ToDateTime(frmconcat.ToString());

        valtodate = txtToDate.Text.ToString();
        string[] split2 = valtodate.Split(new char[] { '/' });
        frmconcat = split2[1].ToString() + '/' + split2[0].ToString() + '/' + split2[2].ToString();
        DateTime dttodate = Convert.ToDateTime(frmconcat.ToString());

        TimeSpan ts = dttodate.Subtract(dtfromdate);
        int days = ts.Days;
        if (days < 0)
        {
            lblnorec.Text = "From Date Must Be Less Than To Date";
            lblnorec.Visible = true;
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            gridviewload.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btnExcel.Visible = false;
            lblerror.Visible = false;
            btnprintmaster.Visible = false;
            // FpEntry.Sheets[0].RowCount = 0;

        }
        else
        {
            lblnorec.Text = "";
            lblnorec.Visible = false;
            lblerror.Visible = false;
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = true;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
            //FpEntry.Visible = true;
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            gridviewload.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;

            if (ddlTest.Text != "")
            {
                lblnorec.Visible = false;
                lblerror.Visible = false;
                lblnorec.Text = "";
                if (ddlTest.SelectedItem.Value.ToString() == "Terminal Test")
                {
                    // MessageBox.Show("No Test conducted ");

                }
                else
                {
                    if (ddlSec.Enabled == true || ddlSec.Text != "-1" || ddlSec.Enabled == false)
                    {
                        gridviewload.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        //FpEntry.Sheets[0].ColumnHeader.RowCount = 3;

                        SpreadBind();//---------------changed 12.12-------------------
                        loadexamdate();
                        //for (int right_logo_col = 0; right_logo_col < FpEntry.Sheets[0].ColumnCount; right_logo_col++)
                        //{
                        //    if (FpEntry.Sheets[0].Columns[right_logo_col].Visible == true)
                        //    {
                        //        //MyImg mi3 = new MyImg();
                        //        //mi3.ImageUrl = "Handler/Handler2.ashx?";
                        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].HorizontalAlign = HorizontalAlign.Center; //23.02.12
                        //        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].CellType = mi3;
                        //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, right_logo_col, 9, 1);
                        //        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].Border.BorderColorBottom = Color.Black;
                        //        //FpEntry.Sheets[0].Columns[right_logo_col].Width = 150;
                        //        break;

                        //    }
                        //}
                        btn.Visible = false;
                        gridviewload.Width = 900;
                        //'-------------------------------------------------------------------------------------
                        Buttontotal.Visible = false;
                        lblrecord.Visible = false;
                        DropDownListpage.Visible = false;
                        TextBoxother.Visible = false;
                        lblpage.Visible = false;
                        TextBoxpage.Visible = false;
                        gridviewload.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                    }

                    if (Convert.ToInt32(gridviewload.Rows.Count) == 0)
                    {
                        lblnorec.Visible = true;
                        gridviewload.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        lblerror.Visible = false;
                    }
                    else
                    {
                        //Buttontotal.Visible = true;
                        //lblrecord.Visible = true;
                        //DropDownListpage.Visible = true;
                        //TextBoxother.Visible = false;
                        //lblpage.Visible = true;
                        //TextBoxpage.Visible = true;
                        gridviewload.Visible = true;
                        btnExcel.Visible = true;
                        btnprintmaster.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;

                        //    Double totalRows = 0;
                        //    totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
                        //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
                        //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                        //    DropDownListpage.Items.Clear();
                        //    if (totalRows >= 10)
                        //    {
                        //       // FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        //        {
                        //            DropDownListpage.Items.Add((k + 10).ToString());
                        //        }
                        //        DropDownListpage.Items.Add("Others");
                        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //        gridviewload.Height = 335;

                        //    }
                        //    else if (totalRows == 0)
                        //    {
                        //        DropDownListpage.Items.Add("0");
                        //        gridviewload.Height = 100;
                        //    }
                        //    else
                        //    {
                        //        FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        //        DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
                        //        FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        //    }
                        //    if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) > 10)
                        //    {
                        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //        FpEntry.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //        CalculateTotalPages();
                        //    }
                        //    // FpEntry.Sheets[0].PageSize = Convert.ToInt32(ddlto.SelectedValue.ToString()) - Convert.ToInt32(ddlfrom.SelectedValue.ToString()) + 1 + spancount + count+1;
                        //    FpEntry.Height = 200 + (20 * Convert.ToInt32(totalRows));
                    }

                    if (ddlTest.SelectedItem.Value.ToString() == "--Select--")
                    {
                        gridviewload.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        btnprintmaster.Visible = false;
                        //btnExcel.Visible = true;
                        //btnprintmaster.Visible = true;
                        //txtexcelname.Visible = true;
                        //lblrptname.Visible = true;
                    }
                }
            }
            else
            {
                gridviewload.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblnorec.Text = "Kindly Select Test";
                lblnorec.Visible = true;
                lblerror.Visible = false;
            }
        }//-----------------------------date validate------------------------------

        //FpEntry.CommandBar.Visible = false;
        //FpEntry.Sheets[0].RowHeader.Visible = false;
        if (cbsms.Checked == true || cbvoice.Checked == true)
        {
            btn.Visible = true;
        }
        else
        {
            btn.Visible = false;
        }
        //FpEntry.Sheets[0].FrozenColumnCount = 4;

    }
    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        btn.Visible = false;

        //   buttonG0();

    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTest();
        ////lblEsection.Visible = false;
        lblnorec.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        lblerror.Visible = false;
        btn.Visible = false;
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridviewload.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        btn.Visible = false;

        btnExcel.Visible = false;
        btn.Visible = false;

        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();

        binddegree();
        if (ddlDegree.Text != "")
        {
            //bindbranch();

            //bindsem();


            //bindsec();

            GetTest();
            lblnorec.Visible = false;
            lblerror.Visible = false;
        }
        else
        {
            lblnorec.Text = "Give degree rights to the staff";
            lblnorec.Visible = true;
            lblerror.Visible = false;
        }



    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            gridviewload.Visible = true;


            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            btnprintmaster.Visible = true;
            // FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
            //  FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        }
        //FpEntry.SaveChanges();
        //FpEntry.CurrentPage = 0;
    }


    void CalculateTotalPages()
    {
        //Double totalRows = 0;
        //totalRows = Convert.ToInt32(gridviewload.Rows.Count);
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
        //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //Buttontotal.Visible = false;
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //    if (TextBoxpage.Text.Trim() != "")
            //    {
            //        if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
            //        {
            //            LabelE.Visible = true;
            //            LabelE.Text = "Exceed The Page Limit";
            //            gridviewload.Visible = true;


            //            lblrptname.Visible = true;
            //            txtexcelname.Visible = true;
            //            btnExcel.Visible = true;
            //            btnprintmaster.Visible = true;
            //            TextBoxpage.Text = "";
            //        }
            //        else if (Convert.ToInt32(TextBoxpage.Text) == 0)
            //        {
            //            LabelE.Visible = true;
            //            LabelE.Text = "Search should be greater than zero";
            //            TextBoxpage.Text = "";
            //        }
            //        else
            //        {
            //            LabelE.Visible = false;
            //            FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
            //            gridviewload.Visible = true;


            //            lblrptname.Visible = true;
            //            txtexcelname.Visible = true;
            //            btnExcel.Visible = true;
            //            btnprintmaster.Visible = true;
            //        }
            //    }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {

        try
        {

            if (TextBoxother.Text != "")
            {

                //FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }

    public string Getdate(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        mycon1.Close();
        mycon1.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlCommand cmd5a = new SqlCommand(sqlstr);
        cmd5a.Connection = mycon1;
        SqlDataReader drnew;
        drnew = cmd5a.ExecuteReader();
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

    public string getattval(int att_leavetype)
    {

        switch (att_leavetype)
        {
            case 1:

                atten = "P";
                break;
            case 2:
                atten = "A";
                break;
            case 3:
                atten = "OD";
                break;
            case 4:
                atten = "ML";
                break;
            case 5:
                atten = "SOD";
                break;
            case 6:
                atten = "NSS";
                break;
            //case 7:
            //    atten = "H";
            //    break;
            case 8:
                atten = "NJ";
                break;
            case 9:
                atten = "S";
                break;
            case 10:
                atten = "L";
                break;
            case 11:
                atten = "NCC";
                break;
            case 12:
                atten = "HS";
                break;
            case 13:
                atten = "PP";
                break;
            case 14:
                atten = "SYOD";
                break;
            case 15:
                atten = "COD";
                break;
            case 16:
                atten = "OOD";
                break;
            case 17:
                atten = "LA"; //"EOD";
                break;
            //Added by subburaj 20/08/2014********//
            case 18:
                atten = "RAA";
                break;
            //***********End********************//
        }
        return atten;


    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        gridviewload.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        btn.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    // FpEntry.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    Button1.Focus();
                    //=============================================
                }
                else
                {
                    lblerror.Text = "Please enter your Report Name";
                    lblerror.Visible = true;
                    Button1.Focus();
                    lblnorec.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }

    public void func_header()
    {
    }


    public void getspecial_hr()
    {
        //  try
        {
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

            }
            if (hrdetno != "")
            {
                con_splhr_query_master.Close();
                con_splhr_query_master.Open();
                DataSet ds_splhr_query_master = new DataSet();

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";

                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                dr_splhr_query_master = cmd.ExecuteReader();

                while (dr_splhr_query_master.Read())
                {
                    if (dr_splhr_query_master.HasRows)
                    {
                        value = dr_splhr_query_master[0].ToString();

                        if (value != null && value != "0" && value != "7" && value != "")
                        {
                            if (tempvalue != value)
                            {
                                tempvalue = value;
                                for (int j = 0; j < count; j++)
                                {

                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                    {
                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                        j = count;
                                    }
                                }
                            }
                            if (ObtValue == 1)
                            {
                                per_abshrs_spl += 1;
                            }
                            else if (ObtValue == 2)
                            {
                                notconsider_value += 1;
                                njhr += 1;
                            }
                            else if (ObtValue == 0)
                            {
                                tot_per_hrs_spl += 1;
                            }
                            if (value == "3")
                            {
                                tot_ondu_spl += 1;
                            }
                            else if (value == "10")
                            {
                                per_leave += 1;
                            }
                            if (value == "4")
                            {
                                tot_ml_spl += 1;
                            }
                            tot_conduct_hr_spl++;
                        }
                        else if (value == "7")
                        {
                            per_hhday_spl += 1;
                            tot_conduct_hr_spl--;
                        }
                        else
                        {
                            unmark_spl += 1;
                            tot_conduct_hr_spl--;
                        }
                    }
                }


                per_abshrs_spl_fals = per_abshrs_spl;
                tot_per_hrs_spl_fals = tot_per_hrs_spl;
                per_leave_fals = per_leave;
                tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
                tot_ondu_spl_fals = tot_ondu_spl;
                tot_ml_spl_fals = tot_ml_spl;

            }
        }
        //  catch
        {
        }
    }


    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;

        string filt_details = "";
        string strsec = "";
        if (ddlSec.Enabled == true)
        {
            strsec = " Sec " + ddlSec.SelectedItem.Text.ToString();
        }
        filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString() + "-" + strsec;

        string date_filt = "From :" + txtFromDate.Text + "-" + "To :" + txtToDate.Text;
        string test = "Test :" + ddlTest.SelectedItem.ToString();

        string degreedetails = string.Empty;

        degreedetails = "CAM REPORT" + "@" + filt_details + "@" + date_filt + "@" + test;
        string pagename = "CAMrpt.aspx";

        //Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;

    }

    protected void btn_Click(object sender, EventArgs e)
    {
        //FpEntry.SaveChanges();
        if (cbsms.Checked == true)
        {
            sendsms();
            Button1.Focus();

        }
        if (cbvoice.Checked == true)
        {
            sendvoice();
            Button1.Focus();
        }
    }

    public void sendvoice()
    {
        //FpEntry.SaveChanges();
        try
        {
            string mark = "";

            string studname = "";
            string testname = "";
            string rollno = "";
            string total = "";


            for (int i = 0; i < gridviewload.Rows.Count; i++)
            {
                int col = 5;
                int k = 0;

                total = Convert.ToString(gridviewload.Rows[i].Cells[4].Text);
                foreach (GridViewRow row in gridviewload.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    int RowCnt = Convert.ToInt32(row.RowIndex);
                    if (cbsel.Checked == true)
                    {
                        int inc = 0;
                        for (int j = 6; j < gridviewload.Rows.Count - 3; j++)
                        {
                            string subname = "";
                            inc++;
                            studname = Convert.ToString(gridviewload.Rows[i].Cells[3].Text);
                            testname = ddlTest.SelectedItem.Text.ToString();
                            rollno = Convert.ToString(gridviewload.Rows[i].Cells[2].Text);
                            subname = subarray[k].ToString();
                            string mark1 = gridviewload.Rows[i].Cells[j].Text;
                            markarray.Add(inc);
                            if (mark == "")
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = subname + ":" + mark1 + ":" + "Fail";
                                    resultarray.Add("Fail");
                                }
                                else
                                {
                                    mark = subname + ":" + mark1 + ":" + "Pass";
                                    resultarray.Add("Pass");
                                }
                            }
                            else
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = mark + " - " + subname + ":" + mark1 + ":" + "Fail";
                                    resultarray.Add("Fail");
                                }
                                else
                                {
                                    mark = mark + " - " + subname + ":" + mark1 + ":" + "Pass";
                                    resultarray.Add("Pass");
                                }
                            }
                            k++;
                        }


                        sendvoicecall(studname, mark, testname, rollno, total);

                    }
                }
            }
        }
        catch
        {

        }
    }

    public void sendsms()
    {
        try
        {
            string SelectQ =d2.GetFunctionv("select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='2'");

            for (int i = 0; i < gridviewload.Rows.Count; i++)
            {
                string mark = "";
                string studname = "";
                string testname = "";
                string rollno = "";
                string total = "";
                int col = 5;
                int k = 0;
                int k1=0;
               
                rollno = Convert.ToString(gridviewload.Rows[i].Cells[2].Text);
                studname = Convert.ToString(gridviewload.Rows[i].Cells[4].Text);
                testname = ddlTest.SelectedItem.Text.ToString();
               
                //foreach (GridViewRow row in gridviewload.Rows)
                //{
                //CheckBox cbsel = (CheckBox)row.FindControl("selectchk");
                CheckBox cbsel = (CheckBox)gridviewload.Rows[i].FindControl("selectchk");
                //int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    for (int j = 6; j < Convert.ToInt32(datatablecount); j++)
                    {

                        string subname = "";
                        subname = subarray[k].ToString();

                        string mark1 = gridviewload.Rows[i].Cells[j].Text;
                        if (mark == "")
                        {
                            if (SelectQ.Contains("Result"))
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = subname + ":" + mark1 + ":" + "Fail";
                                }
                                else
                                {
                                    mark = subname + ":" + mark1 + ":" + "Pass";
                                }
                            }
                            else if (SelectQ.Contains("Short-Form(Pass:P,Fail:F)"))
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = subname + ":" + mark1 + ":" + "F";
                                }
                                else
                                {
                                    mark = subname + ":" + mark1 + ":" + "P";
                                }
                            }
                            else
                            {
                                mark = subname + ":" + mark1;
                            }
                        }
                        else
                        {
                            if (SelectQ.Contains("Result"))
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = mark + "  " + subname + ":" + mark1 + ":" + "Fail";
                                }
                                else
                                {
                                    mark = mark + "  " + subname + ":" + mark1 + ":" + "Pass";
                                }
                            }
                            else if (SelectQ.Contains("Short-Form(Pass:P,Fail:F)"))
                            {
                                if (gridviewload.Rows[i].Cells[j].ForeColor == Color.Red)
                                {
                                    mark = mark + "  " + subname + ":" + mark1 + ":" + "F";
                                }
                                else
                                {
                                    mark = mark + "  " + subname + ":" + mark1 + ":" + "P";
                                }
                            }
                            else
                            {
                                mark = mark + "  " + subname + ":" + mark1;
                            }
                        }
                        k++;
                        k1=j;
                    }
                    if (SelectQ.Contains("Percentage"))
                    {
                        string perce = gridviewload.Rows[i].Cells[k1 + 2].Text;
                        mark = mark + " Percentage:" + perce + "%";
                    }
                    smsdeliverd(studname, mark, testname, rollno, total);
                    //added by Mullai
                    if (sendflag1 == true)
                    {
                        divPopupAlert.Visible = true;
                        divAlertContent.Visible = true;
                        lblalertmsg.Visible = true;
                        lblalertmsg.Text = "Send Successfully";

                    }
                    else
                    {
                        divPopupAlert.Visible = true;
                        divAlertContent.Visible = true;
                        lblalertmsg.Visible = true;
                        lblalertmsg.Text = "SMS Not Sent";
                    }

                }
                //}
            }
        }
        catch
        {

        }
    }

    public void smsdeliverd(string name, string mark, string test, string rollno, string totalmark)
    {

        try
        {
            string user_id = "";

            string studname = name;
            string studmark = mark;
            string testname = test;
            string studroll = rollno;
            string total = totalmark;
            string studphone = "";
            string collegename = "";
            string collegequery = "";
            string gender = "";
            string voicelanguage = "";
            string coursename = "";
            collegequery = "select Coll_acronymn from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            ds = dacces2.select_method_wo_parameter(collegequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collegename = ds.Tables[0].Rows[0]["Coll_acronymn"].ToString();

            }

            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + ddlBranch.SelectedItem.Value + "";
            DataSet dscode = new DataSet();
            dscode = dacces2.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }

            string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dstrack;
            dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
            if (dstrack.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
            }

            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.reg_no,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + studroll + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dsMobile;
            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
            if (dsMobile.Tables[0].Rows.Count > 0)
            {
                studphone = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();

                if (dsMobile.Tables[0].Rows[0]["Gender"].ToString() == "0")
                {
                    gender = "Your Son";
                }
                if (dsMobile.Tables[0].Rows[0]["Gender"].ToString() == "1")
                {
                    gender = "Your Daughter";
                }
            }

            studphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudentMobile"]);
            string fatherphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["FatherMobile"]);
            string motherphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["MotherMobile"]);

            bool checkset = false;
            string setquery = "";
            string str = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["group_code"].ToString() + "'";
            }
            else
            {
                str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'  and USER_ID='" + Session["usercode"].ToString() + "'";
            }
            string strmsg = string.Empty;
            //string msgtext = "Dear Parent, This Message From " + collegename + ". " + gender + "  " + studname + "  " + coursename + " scored" + " " + testname + " Test " + total + " Out of " + maxmark + " On" + studmark + " Thank You!";

            DataTable dtTemplate = new DataTable();
            string template = string.Empty;
            string SelectQ = "select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='2'";//
            dtTemplate = dirAcc.selectDataTable(SelectQ);

            if (dtTemplate.Rows.Count > 0)
            {
                template = Convert.ToString(dtTemplate.Rows[0]["template"]);
            }
            if (!string.IsNullOrEmpty(template))
            {
                string[] splittemplate = template.Split('$');

                string d = splittemplate[0].ToLower();
                if (splittemplate.Length > 0)
                {
                    for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                    {
                        if (splittemplate[j].ToString() != "")
                        {
                            if (splittemplate[j].ToLower().Trim() == "your") //your
                            {
                                strmsg = strmsg + " " + gender + ",";
                            }
                            else if (splittemplate[j].ToString() == "College Name")
                            {
                                strmsg = strmsg + " " + "This Message from" + " " + Session["collegecode"].ToString() + ",";
                            }
                            else if (splittemplate[j].ToString() == "Student Name")
                            {
                                strmsg = strmsg + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                            }
                            else if (splittemplate[j].ToString() == "Register No")
                            {
                                strmsg = strmsg + " " + dsMobile.Tables[0].Rows[0]["reg_no"].ToString();
                            }
                            else if (splittemplate[j].ToString() == "Degree")
                            {
                                strmsg = strmsg + " ";
                            }
                            else if (splittemplate[j].ToString() == "CAM Mark")
                            {
                                strmsg = strmsg + " " + "scored" + " " + testname +" "+ total + studmark + "";
                            }
                            else if (splittemplate[j].ToString() == "Short-Form(Pass:P,Fail:F)" || splittemplate[j].ToString() == "Result" || splittemplate[j].ToString() == "Percentage")
                            {
                                
                            }
                            else
                            {
                                if (strmsg == "")
                                {
                                    strmsg = splittemplate[j].ToString();
                                }
                                else
                                {
                                    strmsg = strmsg + " " + splittemplate[j].ToString();
                                }
                            }

                        }

                    }
                }
            }
            else
            {
                lblnorec.Visible = true;

                //lblnorec.Text = "Kindly Create SMS Template";
            }

            DateTime dt = Convert.ToDateTime(DateTime.Now.ToString());

            //modified by srinath 1/8/2014
            //GetUserapi(user_id);
            string getval = dacces2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {

                SenderID = spret[0].ToString();
                Password = spret[0].ToString();
                Session["api"] = user_id;
                Session["senderid"] = SenderID;
            }

            DataSet data = new DataSet();
            bool sendflag = false;
            data = d2.select_method_wo_parameter(str, "txt");
            if (data.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
            {
                for (int jj1 = 0; jj1 < data.Tables[0].Rows.Count; jj1++)
                {
                    if (data.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && data.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                    {
                        if (fatherphone != "" && fatherphone != null && fatherphone != "0")
                        {
                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, fatherphone, strmsg, "0");
                            if (nofosmssend == 1)
                            {
                                sendflag1 = true;
                            }

                        }
                    }

                    if (data.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && data.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                    {
                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                        {
                            if (motherphone != "" && motherphone != null && motherphone != "0")
                            {
                                int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, motherphone, strmsg, "0");
                                if (nofosmssend == 1)
                                {
                                    sendflag1 = true;
                                }
                            }

                        }
                    }
                    if (data.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && data.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                    {
                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                        {
                            if (studphone != "" && studphone != null)
                            {
                                int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, studphone, strmsg, "0");
                                if (nofosmssend == 1)
                                {
                                    sendflag1 = true;
                                }
                            }
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }


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
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    public void smsreport(string uril, string isstaff, DateTime dt, string phone, string msg)
    {
        try
        {
            string mobile = phone;
            string message = msg;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = "";
            string[] split_mobileno = mobile.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
                sms = dacces2.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {
        }

    }

    public void sendvoicecall(string name, string markdetails, string test, string rollno, string totalmark)
    {
        try
        {
            string Gender = "";


            string roll = rollno;
            string studphone = "";
            string coursename = "";
            string collegename = "";
            // string studname = name;
            string mark = markdetails;
            string collacronym = "";
            string voicelanguage = "";
            string total = totalmark;

            string orginalname = "";

            string student_name = name;
            if (student_name.Contains(".") == true)
            {
                string[] splitname = student_name.Split('.');

                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                {
                    string lengthname = splitname[i].ToString();
                    if (lengthname.Trim().Length > 2)
                    {
                        orginalname = splitname[i].ToString();
                    }



                }
            }

            else
            {

                string[] split2ndname = name.Split(' ');
                if (split2ndname.Length > 0)
                {
                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                    {
                        string firstname = split2ndname[k].ToString();
                        if (firstname.Trim().Length > 2)
                        {
                            if (orginalname == "")
                            {
                                orginalname = firstname.ToString();
                            }
                            else
                            {
                                orginalname = orginalname + " " + firstname.ToString();
                            }
                        }
                    }
                }
            }



            string collegequery = "";
            collegequery = "select Coll_acronymn,collname from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            ds = dacces2.select_method_wo_parameter(collegequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collegename = ds.Tables[0].Rows[0]["collname"].ToString();
                collacronym = ds.Tables[0].Rows[0]["Coll_acronymn"].ToString();

            }


            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + ddlBranch.SelectedItem.Value + "";
            DataSet dscode = new DataSet();
            dscode = dacces2.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }



            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dsMobile;
            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
            string fatherphone = "";
            string motherphone = "";
            if (dsMobile.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                {
                    Gender = "MALE";
                }
                else
                {
                    Gender = "FEMALE";

                }

                studphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudentMobile"]);
                fatherphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["FatherMobile"]);
                motherphone = Convert.ToString(dsMobile.Tables[0].Rows[0]["MotherMobile"]);

                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery = "";
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                    DataSet datalang = new DataSet();
                    datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }

                // voicelanguage = "English";

                //}

                //DateTime dt = Convert.ToDateTime(date);

                //string stud_name = dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                //string fulname = stud_name.ToUpper();
                //string[] split = fulname.Split('.');





                //string[] files = new string[11 + subarray.Count * 3 + 3];

                //if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\DEARPARENTS.wav") == true)
                //{
                //    files[0] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\DEARPARENTS.wav";

                //    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\GOODMORNING.wav") == true)
                //    {
                //        files[1] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\GOODMORNING.wav";
                //        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\THISCALLFROM.wav") == true)
                //        {
                //            files[2] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\THISCALLFROM.wav";
                //            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\INSTITUTENAME.wav") == true)
                //            {
                //                files[3] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\INSTITUTENAME.wav";
                //                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + Gender + ".wav") == true)
                //                {
                //                    files[4] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + Gender + ".wav";
                //                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + split[0].ToString() + ".wav") == true)
                //                    {
                //                        files[5] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + split[0].ToString() + ".wav";

                //                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\4THSTANDARD.wav") == true)
                //                        {
                //                            files[6] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\4THSTANDARD.wav";
                //                            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\INTERNALMARK.wav") == true)
                //                            {
                //                                files[7] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\INTERNALMARK.wav";
                //                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\TOTALMARKSOBTAINED.wav") == true)
                //                                {
                //                                    files[8] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\TOTALMARKSOBTAINED.wav";
                //                                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\OUTOF.wav") == true)
                //                                    {
                //                                        files[9] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\OUTOF.wav";
                //                                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\TOTALOUTOFMARK.wav") == true)
                //                                        {
                //                                            files[10] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\TOTALOUTOFMARK.wav";
                //                                            if (subarray.Count > 0)
                //                                            {

                //                                                int a = 10;
                //                                                for (int i = 0; i < subarray.Count; i++)
                //                                                {
                //                                                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\"+subarray[i].ToString()+".wav") == true)
                //                                                    {
                //                                                        a++;
                //                                                        files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + subarray[i].ToString().ToUpper() + ".wav";
                //                                                        //string newmark = markarray[i].ToString();
                //                                                        //string[] x = new string[newmark.Length];
                //                                                        //for (int k = 0; k < newmark.Length; k++)
                //                                                        //{
                //                                                        //    x[k] = newmark.Substring(i, 1);

                //                                                        //}
                //                                                        //for (int count = 0; count < x.Length; count++)
                //                                                        //{
                //                                                        //    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\"+x[count].ToString()+".wav") == true)
                //                                                        //    {
                //                                                        //        a++;
                //                                                        //        files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + x[count].ToString() + ".wav";
                //                                                        //    }
                //                                                        //}



                //                                                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\"+markarray[i].ToString ()+".wav") == true)
                //                                                        {
                //                                                         a++;
                //                                                            files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\"+markarray[i].ToString ()+".wav";
                //                                                            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\"+resultarray[i].ToString().ToUpper()+".wav") == true)
                //                                                            {
                //                                                                a++;
                //                                                                files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\" + resultarray[i].ToString().ToUpper() + ".wav";
                //                                                            }
                //                                                        }
                //                                                    }
                //                                                }
                //                                                a++;
                //                                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\CONDUCTEDON.wav") == true)
                //                                                {
                //                                                    files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\CONDUCTEDON.wav";

                //                                                }
                //                                                a++;
                //                                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\DATE.wav") == true)
                //                                                {
                //                                                    files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\DATE.wav";

                //                                                }
                //                                                a++;
                //                                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\THANKYOU.wav") == true)
                //                                                {
                //                                                    files[a] = "C:\\Documents and Settings\\Admin\\Desktop\\ivr\\MARKS VOICE\\THANKYOU.wav";

                //                                                }

                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }

                //    }
                //}



                //string jairam = split[0].ToString();

                //Concatenate(Server.MapPath("~/UploadFiles/temp.wav"), files);
                //filepath = Server.MapPath("~/UploadFiles/temp.wav");
                //insertmethod(filepath,split[0].ToString ());


                //FileInfo fileinfo = new FileInfo(Server.MapPath("~/UploadFiles/" + split[0].ToString() + ".wav"));


                //string filename = fileinfo.Name;
                // string gender = "female";
                //Modified By Srinath
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                // string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                bool checkset = false;
                //string setquery = "";
                //setquery = "select TextName ,Taxtval  from Attendance_Settings where TextName ='CAM' and College_Code=" + Session["collegecode"] + "";
                //DataSet setrights = new DataSet();
                //setrights = d2.select_method_wo_parameter(setquery, "Text");
                //if (setrights.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < setrights.Tables[0].Rows.Count; i++)
                //    {
                //        if (setrights.Tables[0].Rows[i]["TextName"].ToString() == "CAM" && setrights.Tables[0].Rows[i]["Taxtval"].ToString() == "1")
                //        {
                //            checkset = true;
                //        }
                //    }
                //}



                //if (checkset == true)
                //{





                biz.lbinfotech.www.marks h = new biz.lbinfotech.www.marks();
                bool sendflag = false;

                string count = Convert.ToString(subarray.Count);

                //string lengthname = splitname[i].ToString();
                //if (lengthname.Trim().Length > 2)
                //{
                //    orginalname = splitname[i].ToString();
                //}
                if (cbstudent.Checked == true)
                {
                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                    {
                        if (studphone != "" && studphone != null)
                        {
                            string value = h.Getmarks("" + studphone + "", "CAM", "INTERNAL TEST", "" + collegename + "", "" + orginalname + "", "" + Gender + "", "" + ddlBatch.SelectedItem.Value + "", "" + coursename + "", "" + roll + "", "" + ddlTest.SelectedItem.Text + "", "" + mark + "", "" + maxmark + "", "" + total + "", "" + voicelanguage.ToUpper().ToString() + "", "" + count + "", "" + exammothdate + "");
                            sendflag = true;
                        }
                    }
                }
                if (cbfather.Checked == true)
                {
                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                    {
                        if (fatherphone != "" && fatherphone != null)
                        {
                            string value = h.Getmarks("" + fatherphone + "", "CAM", "INTERNAL TEST", "" + collegename + "", "" + orginalname + "", "" + Gender + "", "" + ddlBatch.SelectedItem.Value + "", "" + coursename + "", "" + roll + "", "" + ddlTest.SelectedItem.Text + "", "" + mark + "", "" + maxmark + "", "" + total + "", "" + voicelanguage.ToUpper().ToString() + "", "" + count + "", "" + exammothdate + "");
                            sendflag = true;
                        }
                    }
                }
                if (cbmother.Checked == true)
                {
                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                    {
                        if (motherphone != "" && motherphone != null)
                        {
                            string value = h.Getmarks("" + motherphone + "", "CAM", "INTERNAL TEST", "" + collegename + "", "" + orginalname + "", "" + Gender + "", "" + ddlBatch.SelectedItem.Value + "", "" + coursename + "", "" + roll + "", "" + ddlTest.SelectedItem.Text + "", "" + mark + "", "" + maxmark + "", "" + total + "", "" + voicelanguage.ToUpper().ToString() + "", "" + count + "", "" + exammothdate + "");
                            sendflag = true;
                        }
                    }

                }

                if (sendflag == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Voice Call Sent successfully')", true);
                }


            }
        }
        catch
        {

        }
    }

    //public void Concatenate(string outputFile, IEnumerable<string> sourceFiles)
    //{
    //    byte[] buffer = new byte[1024];
    //    WaveFileWriter waveFileWriter = null;



    //    try
    //    {
    //        foreach (string sourceFile in sourceFiles)
    //        {
    //            using (WaveFileReader reader = new WaveFileReader(sourceFile))
    //            {
    //                if (waveFileWriter == null)
    //                {
    //                    // first time in create new Writer
    //                    waveFileWriter = new WaveFileWriter(outputFile, reader.WaveFormat);
    //                }


    //                int read;
    //                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
    //                {
    //                    waveFileWriter.WriteData(buffer, 0, read);

    //                }
    //            }
    //        }
    //    }
    //    finally
    //    {
    //        if (waveFileWriter != null)
    //        {
    //            waveFileWriter.Dispose();
    //        }
    //    }

    //}

    //public void insertmethod(string filepath,string filename)
    //{
    //    bool upload = false;
    //    String filePath = filepath;

    //    //WaveFormat target = new WaveFormat(8000, 8, 1);
    //    //WaveStream stream = new WaveFileReader(filePath);
    //    //WaveFormatConversionStream str = new WaveFormatConversionStream(target, stream);
    //    //WaveFileWriter.CreateWaveFile(Server.MapPath("~/UploadFiles/" + filename + ".wav"), str);


    //    // FileUpload1.SaveAs(filePath);
    //    FileInfo fileInf = new FileInfo(Server.MapPath("~/UploadFiles/" + filename + ".wav"));
    //    //  string uri = "ftp://" + "203.109.109.29" + "/" + fileInf.Name; //ftp://192.168.1.99/New Stories (Highway Blues).wma
    //    FtpWebRequest reqFTP;

    //    // Create FtpWebRequest object from the Uri provided
    //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + "203.109.109.29" + "/" + fileInf.Name));

    //    // Provide the WebPermission Credintials
    //    reqFTP.Credentials = new NetworkCredential("LBITIND", "vodafone");

    //    // By default KeepAlive is true, where the control connection is not closed
    //    // after a command is executed.
    //    reqFTP.KeepAlive = false;

    //    // Specify the command to be executed.
    //    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

    //    // Specify the data transfer type.
    //    reqFTP.UseBinary = true;

    //    // Notify the server about the size of the uploaded file
    //    reqFTP.ContentLength = fileInf.Length; //size

    //    // The buffer size is set to 2kb
    //    int buffLength = 2048;
    //    byte[] buff = new byte[buffLength];
    //    int contentLen;

    //    // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
    //    FileStream fs = fileInf.OpenRead();

    //    try
    //    {
    //        // Stream to which the file to be upload is written
    //        Stream strm = reqFTP.GetRequestStream();

    //        // Read from the file stream 2kb at a time
    //        contentLen = fs.Read(buff, 0, buffLength); //ftp://192.168.1.99/New%20Stories%20(Highway%20Blues).wma

    //        // Till Stream content ends
    //        while (contentLen != 0)
    //        {
    //            // Write Content from the file stream to the FTP Upload Stream
    //            strm.Write(buff, 0, contentLen);
    //            contentLen = fs.Read(buff, 0, buffLength);
    //            upload = true;
    //            // lblerrorvoice.Visible = false;
    //        }

    //        // Close the file stream and the Request Stream
    //        strm.Close();
    //        fs.Close();


    //    }
    //    catch
    //    {

    //    }

    //}

    public void loadexamdate()
    {
        try
        {
            string examdate = "";
            examdate = "select exam_date  from Exam_type where criteria_no='" + ddlTest.SelectedItem.Value + "'and batch_year='" + ddlBatch.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(examdate, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime exam = Convert.ToDateTime(ds.Tables[0].Rows[0]["exam_date"]);
                exammothdate = exam.ToString("yyyy-MM-dd");
            }
        }
        catch
        {

        }
    }

    protected void btnpopupalert_Click(object sender, EventArgs e)
    {
        divPopupAlert.Visible = false;
        divAlertContent.Visible = false;
    }

    //Added By Saranyadevi 11.10.2018

    #region SmsType

    public void SmsType()
    {
        try
        {
            rblsmstype.Items.Add("Mark");
            rblsmstype.Items.Add("Grade");

            rblsmstype.Items.FindByText("Mark").Selected = true;

        }
        catch (Exception ex) { }

    }

    protected void rblsmstype_Selected(object sender, EventArgs e)
    {
        try
        {
            gridviewload.Visible = false;
            btn.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btn.Visible = false;
        }

        catch (Exception ex) { }
    }
    #endregion
}
