using System; //modified 06.04.12 ,modified on 08.06.12 (logo size)
//========removed textvaltable from select_allcam report details proc.and changed to displaying seattype for student on 01.08.12
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using BalAccess;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net;

public partial class CAMrpt : System.Web.UI.Page
{
    #region Variable Declaration

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection ncon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection condegree = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rankcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_gender = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_seat = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_strseat = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon2 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon3 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection lcon4 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection cons = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
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
    DataTable chartdtgrid = new DataTable();
    DataTable chartdt1 = new DataTable();
    Hashtable charthash1 = new Hashtable();
    Hashtable charthash2 = new Hashtable();
    string chartmin_mark = string.Empty;
    string chartmin_mark_optional = string.Empty;
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
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    static Boolean splhr_flag = false;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    string strorder = string.Empty;
    Institution institute;
    //-------
    DataSet ds_holi = new DataSet();
    DataSet ds_optim = new DataSet();
    //  string collegecode = string.Empty;
    //  string usercode = string.Empty;
    //string regularflag = string.Empty;
    string markglag = string.Empty;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master1 = string.Empty;
    int Atday = 0, endk = 0;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string staff = string.Empty;
    double perofpass = 0;
    double avg = 0;
    string code = string.Empty;
    string text = string.Empty;

    #region RAY
    DataTable dtable1 = new DataTable();
    DataTable dtable2 = new DataTable();
    DataTable fnaltabl = new DataTable();
    DataRow dtrow1 = null;
    DataRow dtrow2 = null;
    DataRow dtrow3 = null;
    static int minmarks = 0;
    static int rowcount = 0;
    #endregion

    Boolean Isfirst = false;
    Boolean IsFirstcol = false;
    Boolean RnkFlag;
    Boolean PresentFlag = false;
    Boolean callattfun;

    DateTime dt1, dt2;
    DateTime date_today;

    int[] hasharray;
    int student = 0;
    int ic = 0;
    int i;
    int minI, minII, perdayhrs, wk1, wk2, wk3, wk4, wk5, wk6, wk7, wk8, wk9, Ihof, IIhof, fullday, cumfullday;
    int hour1, hour2, hour3, hour4, hour5, hour6, hour7, hour8, hour9, condhrs1, condhrs2, condhrs3, condhrs4, condhrs5, condhrs6, condhrs7, condhrs8, condhrs9;
    int ondu1, ondu2, ondu3, ondu4, ondu5, ondu6, ondu7, ondu8, ondu9, leave1, leave2, leave3, leave4, leave5, leave6, leave7, leave8, leave9;
    static int cook = 0;

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    double totpresentday;
    double perprest, perpresthrs, perabsent, perabsenthrs, perondu, peronduhrs, perleave, perleavehrs;
    double pertothrs, pertotondu, pertotleavehrs, pertotabsenthrs, onduday, cumcontotpresentday, percontotpresentday, hollyhrs, condhrs, balamonday, att_points;
    double cumperprest, cumperpresthrs, cumperabsent, cumperabsenthrs, cumperondu, cumperonduhrs, cumperleave, cumperleavehrs, checkpre, baldate, totmonth, cummcc, cumcondhrs, percondhrs = 0, cumatt_points;

    string m7, m2, m3, m4, m5, m6, m1, m8, m9;
    double totalRows = 0;

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
    string subjctcode = string.Empty;
    double hours_present = 0;
    double hours_absent = 0;
    double hours_od = 0;
    double hours_total = 0;
    double hours_leave = 0;
    double hours_conduct = 0;
    double hours_pres = 0;
    string dateconcat = string.Empty;
    string date1concat = string.Empty;
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
    string strseattype = string.Empty;
    int seattypecount = 0;
    string getquota = string.Empty;
    string getseat = string.Empty;
    string gettextcode = string.Empty;
    string retrvseatname = string.Empty;
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
    int passcount, failcount, maxcount, mincount, avg_50count, avg_65count, pre_count, ab_count, pperc_count, avg_count, avgg65count, opasscoun, ofailcount, opperc_count;
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
    int gs_pass_count, bs_pass_count, gs_fail_count, bs_failcount, tot_gs_count, tot_bs_count, no_of_all_clear;
    int gs_count, bs_count, eod_count, tot_stu, x1;
    int d_pass_count, h_pass_count, t_pass_count, e_pass_count;
    int d_fail_count, h_fail_count, t_fail_count, e_fail_count;
    string strsec = string.Empty;
    string sections = string.Empty;
    string batch = string.Empty;
    string degreecode = string.Empty;
    string subno = string.Empty;
    string semester = string.Empty;
    int quota_count;
    string exam_code = string.Empty;
    string criteria_no = string.Empty;
    int iscount = 1;
    int holi_count;
    //saravana end
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string collegecode = string.Empty;
    //--------------------------new start 06.04.12 print
    static Boolean PrintMaster = false;
    int final_print_col_cnt = 0;
    string footer_text = string.Empty;
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    string collnamenew1 = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address = string.Empty;
    string Phoneno = string.Empty;
    string Faxno = string.Empty;
    string phnfax = string.Empty;
    int subjectcount = 0;
    string district = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string form_heading_name = string.Empty;
    string batch_degree_branch = string.Empty;
    int chk_secnd_clmn = 0;
    int right_logo_clmn = 0;
    DataSet dsprint = new DataSet();
    int colrowcount = 0;
    //'-------------------end print 
    #endregion

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
            lblnorec.Visible = false;
            collegecode = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                setLabelText();
                // Added By Sridharan 12 March 2015
                //{
                chk_pass.Checked = true;
                chk_fail.Checked = true;
                chk_abst.Checked = true;
                chkIncludeAbsent.Checked = false;
                //}
                RadioBtnlist_sub.Items[0].Selected = true;
                GridViewchart.Visible = false;
                Chart1.Visible = false;
                btnExcelchart.Visible = false;
                btnPrintchart.Visible = false;
                GridViewselectedfield.Visible = false;

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btnprintmaster.Visible = false;
                Radiowithoutheader.Visible = false;
                RadioHeader.Visible = false;

                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;

                gview.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                //'------------------------------------------------------------
                //btnPrint.Visible = true;
                //FpEntry.Visible = false;
                //FpEntry.Sheets[0].PageSize = 10;
                gview.Visible = false;

                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                // RadioButtonList3.SelectedValue = "4";  // Hided By Sridharan 12 March 2015
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
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }


                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                mtrdr = mtcmd.ExecuteReader();
                Session["strvar"] = string.Empty;
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
                            lblnorec.Text = "Give " + lblDegree.Text + " rights to the staff";
                            lblnorec.Visible = true;
                        }
                        //batch
                        //course
                        collegecode = Session["collegecode"].ToString();
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
                                ddlreport.Items[Convert.ToInt32(spl_criteria_val[crt])].Selected = true;
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
                        lblnorec.Text = "Give " + lblDegree.Text + " rights to the staff";
                        lblnorec.Visible = true;
                    }
                    //batch
                    //course
                    collegecode = Session["collegecode"].ToString();
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
        ds.Clear();
        if (cbincludepassout.Checked == true)
        {
            string qry = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar' order by batch_year; select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar'";
           
            ds = d2.select_method_wo_parameter(qry, "text");
        }
        else
        {
            ds = dacces2.select_method_wo_parameter("bind_batch", "sp");
        }
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
        ////Control cntUpdateBtn = FpEntry.FindControl("Update");
        ////Control cntCancelBtn = FpEntry.FindControl("Cancel");
        ////Control cntCopyBtn = FpEntry.FindControl("Copy");
        ////Control cntCutBtn = FpEntry.FindControl("Clear");
        ////Control cntPasteBtn = FpEntry.FindControl("Paste");
        //////Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //////Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        ////Control cntPagePrintBtn = FpEntry.FindControl("Print");

        Control cntUpdateBtn = gview.FindControl("Update");
        Control cntCancelBtn = gview.FindControl("Cancel");
        Control cntCopyBtn = gview.FindControl("Copy");
        Control cntCutBtn = gview.FindControl("Clear");
        Control cntPasteBtn = gview.FindControl("Paste");
        Control cntPagePrintBtn = gview.FindControl("Print");

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
            ddlTest.Items.Clear();
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester ='" + ddlSemYr.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "'";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = string.Empty;
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester='" + ddlSemYr.SelectedValue.ToString() + "' and syllabus_year='" + SyllabusYr.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' order by criteria";
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter1.Fill(titles);
            if (titles.Tables.Count > 0 && titles.Tables[0].Rows.Count > 0)
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
        GridViewchart.Visible = false;
        Chart1.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        GridViewselectedfield.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
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
        GridViewchart.Visible = false;
        Chart1.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        GridViewselectedfield.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
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
    protected void cbincludepassout_OnCheckedChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
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
        string SayHrsAbsent = string.Empty;
        string SayHrsPresent = string.Empty;
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
                            SayHrsAbsent = string.Empty;
                            SayHrsPresent = string.Empty;
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
                                            string strattmarksetng = string.Empty;
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
            sections = string.Empty;
        }
        else
        {
            sections = "and r.sections='" + section + "'";
        }
        int count = ds5.Tables[0].Rows.Count;
        string examcode = string.Empty;
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
            string rank = string.Empty;
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
            string avg = string.Empty;
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
            string stt = string.Empty;
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
            string strupdate = string.Empty;
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
            string srtprd = string.Empty;
            string hr = string.Empty;
            long monthyear = (Convert.ToInt64(exam_date.ToString("yyyy")) * 12) + Convert.ToInt64(exam_date.ToString("MM"));
            srtprd = GetFunction("select start_period from exam_type where exam_code='" + examcode + "'");
            if ((mark != "-3") && (mark != "-2"))
            {
                if (srtprd != string.Empty)
                {
                    lcon3.Open();
                    string sqlhour;
                    string strcalflag = string.Empty;
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
        string minmark = string.Empty;
        //string stu = string.Empty;
        //string per = string.Empty;
        int T = 0;
        string drpIhalf = string.Empty;
        string drpminIhalf = string.Empty;
        string drp2half = string.Empty;
        string drpmin2half = string.Empty;
        string no_of_hrs = string.Empty;
        string sqlperiod = string.Empty;
        int stud_count = 0;
        int stud_pass = 0;
        int stud_fail = 0;
        int absent = 0;
        string startprd = string.Empty;
        string endprd = string.Empty;
        double hrcnt = 0;
        double studpresn = 0;
        double studabsen = 0;
        double studod = 0;
        double studlev = 0;
        string exam_codee = string.Empty;
        string drrslt = string.Empty;
        string hr = string.Empty;
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
            //string result = string.Empty;
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
                        string strcalflag = string.Empty;
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
                                string sqlhour1 = string.Empty;
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
                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }
                if (ds7.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds7.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds7.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }
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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }
                if (ds7.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds7.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds7.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }
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
                        if (!holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
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
            int passedcount_chart = 0;
            int passedcount_chart_opt = 0;
            int sub_avg_chart = 0;
            int staff_name_chart = 0;

            Hashtable studabs = new Hashtable();
            Dictionary<int, int> dicsubfacount = new Dictionary<int, int>();
            int nothiddencount = 0;
            //  btnPrint.Visible = true;
            //btnExcel.Visible = true;
            //btnprintmaster.Visible = true;
            //txtexcelname.Visible = true;
            //lblrptname.Visible = true;
            int hasrow_count = 0;
            Radiowithoutheader.Visible = false;
            RadioHeader.Visible = false;

            gview.Visible = true;
            filteration();
            int optinalminpass = 0;
            string getminpass = txtoptiminpassmark.Text.ToString();
            if (getminpass.Trim() != "" && getminpass != null)
            {
                optinalminpass = Convert.ToInt32(getminpass);
            }
            string resminmrk = string.Empty;
            string subject_code = string.Empty;
            int[] maxtot = new int[100];
            string examdate = string.Empty;
            string subname = string.Empty;
            int rankcount = 0;
            int subjectfailedcount = 0;
            int serialno = 0;
            int noofabcout = 0;
            int stusubabsent = 0;
            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlBranch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSemYr.SelectedValue.ToString();
            criteria_no = ddlTest.SelectedValue.ToString();
            //'------------------------------------------------------------
            //  string sqlStr = string.Empty;
            if (sections.ToString().ToLower().Trim() == "all" || sections.ToString().Trim() == string.Empty || sections.ToString().Trim() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = sections.ToString().Trim();
            }
            string ccflag = string.Empty;
            if (cbincludepassout.Checked == true)
            {
                ccflag = string.Empty;
            }
            else
            {
                ccflag = " and cc=0";
            }

            string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no " + ccflag + " and exam_flag <> 'DEBAR' and delflag=0 and  r.sections='" + strsec.ToString() + "' " + strorder + ",s.subject_no";
            string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no " + ccflag + " and exam_flag <> 'DEBAR' and  delflag=0  " + strorder + ",s.subject_no";
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
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count != 0)
            {
                filteration();
                string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 " + ccflag + " and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "'   and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 " + ccflag + " and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                //and r.degree_code=a.degree_code
                //and r.degree_code=a.degree_code
                hat.Clear();
                hat.Add("bath_year", batch.ToString());
                hat.Add("degree_code", degreecode.ToString());
                hat.Add("sec", strsec.ToString());
                hat.Add("filterwithsectionsub", filterwithsectionsub.ToString());
                hat.Add("filterwithoutsectionsub", filterwithoutsectionsub.ToString());

                dtable1.Columns.Add("Sl.No");
                dtable1.Columns.Add("Roll No");
                dtable1.Columns.Add("Reg No");
                dtable1.Columns.Add("Student Name");
                dtable1.Columns.Add("Student Type");
                dtable1.Columns.Add("Application Number");

                dtrow1 = dtable1.NewRow();
                dtrow1["Sl.No"] = "Sl.No";
                dtrow1["Roll No"] = "Roll No";
                dtrow1["Reg No"] = "Reg No";
                dtrow1["Student Name"] = "Student Name";
                dtrow1["Student Type"] = "Student Type";
                dtrow1["Application Number"] = "Application Number";
                dtable1.Rows.Add(dtrow1);

                ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");
                if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count != 0)
                {
                    if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
                    {
                        int c = 0;
                        for (int irow = 0; irow < ds5.Tables[0].Rows.Count; irow++)
                        {
                            dtrow1 = dtable1.NewRow();
                            serialno++;
                            c++;

                            dtrow1["Sl.No"] = c.ToString();
                            dtrow1["Roll No"] = ds5.Tables[0].Rows[irow]["RollNumber"].ToString();
                            dtrow1["Reg No"] = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                            dtrow1["Student Name"] = ds5.Tables[0].Rows[irow]["Student_Name"].ToString();
                            dtrow1["Student Type"] = ds5.Tables[0].Rows[irow]["StudentType"].ToString();
                            dtrow1["Application Number"] = ds5.Tables[0].Rows[irow]["ApplicationNumber"].ToString();

                            dtable1.Rows.Add(dtrow1);
                        }
                        rowcount = c;
                    }

                    chartdtgrid.Columns.Add("SUBJECT", typeof(string));//sridharan
                }
                dtable2.Columns.Add("Temp");
                dtrow2 = dtable2.NewRow();
                dtable2.Rows.Add(dtrow2);

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
                    string acronym = ds2.Tables[1].Rows[i]["acronym"].ToString();

                    if (RadioBtnlist_sub.SelectedIndex == 0)
                    {
                        dtable2.Columns.Add(subject_code);

                        dtable2.Rows[0][subject_code] = subject_code;
                    }
                    else
                    {
                        dtable2.Columns.Add(subname);

                        dtable2.Rows[0][subname] = subname;
                    }

                    charthash1.Add(subject_code.Trim(), "");// added by sridharan 06 mar 2015
                    chartdt1.Columns.Add(subject_code.Trim(), typeof(string));
                    chartdtgrid.Columns.Add(subject_code, typeof(string));
                    charthash2.Add(subject_code.Trim(), "");// added by sridharan 06 mar 2015

                    htdate.Add(subject_code, examdate);
                    count++;
                    dicsubfacount.Add(count, 0);
                }
                dtable2.Columns.Remove("Temp");
            }
            DataRow g1 = chartdtgrid.NewRow();// sridharan
            DataRow g2 = chartdtgrid.NewRow();
            DataRow chartdr10 = chartdt1.NewRow();// sridharan
            DataRow chartdr11 = chartdt1.NewRow();
            DataRow g3 = chartdtgrid.NewRow();
            DataRow g4 = chartdtgrid.NewRow();
            DataRow g5 = chartdtgrid.NewRow();
            DataRow g6 = chartdtgrid.NewRow();
            int totalcount = dtable2.Columns.Count - 2;//
            int percentcount = dtable2.Columns.Count - 1;//

            dtable2.Columns.Add("Total");
            dtable2.Columns.Add("Percentage");
            dtable2.Columns.Add("Result");

            dtable2.Rows[0]["Total"] = "Total";
            dtable2.Rows[0]["Percentage"] = "Percentage";
            dtable2.Rows[0]["Result"] = "Result";

            if (ddlreport.Items[36].Selected == true)
            {
                spancount++;
                dtable2.Columns.Add("No of Subjects Failed");

                dtable2.Rows[0]["No of Subjects Failed"] = "No of Subjects Failed";

            }
            if (ddlreport.Items[43].Selected == true)
            {
                spancount++;
                dtable2.Columns.Add("No of Subjects Absent");

                dtable2.Rows[0]["No of Subjects Absent"] = "No of Subjects Absent";

            }
            if (ddlreport.Items[0].Selected == true)
            {
                spancount++;
                dtable2.Columns.Add("Rank");

                dtable2.Rows[0]["Rank"] = "Rank";

            }
            if (ddlreport.Items[1].Selected == true)
            {
                spancount++;
                dtable2.Columns.Add("Medium");

                dtable2.Rows[0]["Medium"] = "Medium";
            }
            if (ddlreport.Items[2].Selected == true)
            {

                dtable2.Columns.Add("12th/Dip Grp");

                dtable2.Columns.Add("12th/Dip %");

                dtable2.Rows[0]["12th/Dip Grp"] = "12th/Dip Grp";
                dtable2.Rows[0]["12th/Dip %"] = "12th/Dip %";

                spancount += 2;

                gview.Width = 900;
            }
            if (ddlreport.Items[3].Selected == true)
            {

                dtable2.Columns.Add("CGPA");

                dtable2.Rows[0]["CGPA"] = "CGPA";
                spancount++;

                gview.Width = 900;
            }
            //'-----------------------------------------------------------------------------------------------------
            //'--------------------------------------------cam new modify--------------------
           
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[24].Selected == true)
            {

                dtable2.Columns.Add("NFPS");

                dtable2.Rows[0]["NFPS"] = "NFPS";
                spancount++;
            }
            //'------------------------------------------------------------------------
            if (ddlreport.Items[16].Selected == true)
            {
                dtable2.Columns.Add("DPass");

                dtable2.Rows[0]["DPass"] = "DPass";
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[17].Selected == true)
            {

                dtable2.Columns.Add("HPass");

                dtable2.Rows[0]["HPass"] = "HPass";
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[18].Selected == true)
            {
                dtable2.Columns.Add("TPass");

                dtable2.Rows[0]["TPass"] = "TPass";
                spancount++;
            }
            if (ddlreport.Items[19].Selected == true)
            {
                dtable2.Columns.Add("EPass");

                dtable2.Rows[0]["EPass"] = "EPass";
                spancount++;
            }
            if (ddlreport.Items[20].Selected == true)
            {
                dtable2.Columns.Add("G/B");

                dtable2.Rows[0]["G/B"] = "G/B";
                spancount++;
            }
            if (ddlreport.Items[21].Selected == true)
            {
                dtable2.Columns.Add("GPass");

                dtable2.Rows[0]["GPass"] = "GPass";
                spancount++;
            }
            if (ddlreport.Items[22].Selected == true)
            {
                dtable2.Columns.Add("BPass");

                dtable2.Rows[0]["BPass"] = "BPass";
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[23].Selected == true)
            {

                dtable2.Columns.Add("Quota");

                dtable2.Rows[0]["Quota"] = "Quota";
                quotacount = dtable2.Columns.Count;

                spancount++;
                //'-----------------------------------------------------------------------
                //   strseattype = "select distinct seattype from applyn,registration r where r.Batch_Year='" + batch + "' and r.degree_code='" + degreecode + "' and seattype<>0 "; //old 
                strseattype = "select distinct seattype,textval from applyn ,textvaltable where  Batch_Year=" + batch + " and  degree_code=" + degreecode + " and seattype<>0 and seattype=textcode"; //new on 23.06.12
                con_strseat.Close();
                con_strseat.Open();
                SqlCommand cmdstrseatype = new SqlCommand(strseattype, con_strseat);
                SqlDataReader drstrseattype;
                drstrseattype = cmdstrseatype.ExecuteReader();
                while (drstrseattype.Read())
                {
                    if (drstrseattype.HasRows == true)
                    {
                        seattypecount += 1;
                        //-----------------------------------getting the textcode value(seattype)
                        gettextcode = drstrseattype["seattype"].ToString();
                        //'-------------------------------------             -incerment the column                        
                        spancount++;
                        //'-------------------------------------------- getting the textval(columnheading)
                        retrvseatname = drstrseattype["textval"].ToString();
                        //'-------------------------------------------- set the textcode as note
                        dtable2.Columns.Add(retrvseatname);
                        dtable2.Columns.Add(gettextcode);

                        dtable2.Rows[0][retrvseatname] = retrvseatname;
                        dtable2.Rows[0][gettextcode] = gettextcode;
                    }
                }
            }
            //'--------------------------------------- -----------------------------------
            //concolhours
            if (ddlreport.Items[35].Selected == true)
            {

                dtable2.Columns.Add("Conducted Hours");

                dtable2.Rows[0]["Conducted Hours"] = "Conducted Hours";
                spancount++;
                //  FpEntry.Width = 900;
            }
            if (ddlreport.Items[25].Selected == true)
            {

                dtable2.Columns.Add("No of hrs Attended");

                dtable2.Rows[0]["No of hrs Attended"] = "No of hrs Attended";
                spancount++;
                //  FpEntry.Width = 900;
            }
            if (ddlreport.Items[26].Selected == true)
            {

                dtable2.Columns.Add("Attendance %");

                dtable2.Rows[0]["Attendance %"] = "Attendance %";
                spancount++;
                // FpEntry.Width = 900;
            }
            //'=================================================================================================
            //'------------------------------------load the clg information
           
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
                sections = string.Empty;
            }

            //'-================================================================================================
            // student marks start
            int EL = 0;
            int res = 0;
            ds3 = d2.select_method_wo_parameter("Delete_Rank_Table", "sp");
            int d_count, h_count, t_count, e_count, b_count, g_count;
            string marks_per;
            int stu_count = 0;
            int sub_strength = 0;
            // tot_stu = ds5.Tables[0].Rows.Count;
            per_sub_count = ds2.Tables[1].Rows.Count;
            if (ds2.Tables[0].Rows.Count != 0)   //15.02.12
            {
                DataView dv_indstudmarks = new DataView();
                for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                {
                    dtrow2 = dtable2.NewRow();
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
                                   double marks = 0;
                                   // double marks = double.Parse(dv_indstudmarks[cnt]["mark"].ToString());
                                    marks_per = Convert.ToString( dv_indstudmarks[cnt]["mark"]);
                                    double.TryParse(marks_per, out marks);
                                    string min_marksstring = dv_indstudmarks[cnt]["min_mark"].ToString();
                                    if (optinalminpass > 0)//Added by srinath 26 Sep 2016
                                    {
                                        min_mark = optinalminpass;
                                        minmarks = optinalminpass;
                                    }
                                    else
                                    {
                                        if (min_marksstring != "")
                                        {
                                            min_mark = int.Parse(min_marksstring.ToString());
                                            minmarks = int.Parse(min_marksstring.ToString());
                                        }
                                        else
                                        {
                                            min_mark = 0;
                                            minmarks = 0;
                                        }
                                    }
                                    marks_per = marks.ToString();
                                    marks_per = dv_indstudmarks[cnt]["mark"].ToString();
                                    switch (marks_per)
                                    {
                                        case "-1":
                                            if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                            {
                                                if (Convert.ToString(studabs[ds5.Tables[0].Rows[i]["RollNumber"]]) == "1")
                                                    studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "1";
                                            }
                                            else
                                            {
                                                studabs.Add(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), "1");
                                            }
                                            stusubabsent++;
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
                                        //Added by Subburaj 21.08.2014*************//
                                        case "-18":
                                            marks_per = "RAA";
                                            break;
                                        //****************End*************************//
                                    }
                                    if (marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;
                                    }
                                    if (marks >= 0 && (Convert.ToString(marks) != string.Empty))
                                    {
                                        per_mark += marks;
                                        sub_max_marks += double.Parse(dv_indstudmarks[cnt]["max_mark"].ToString());
                                        if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                        {
                                            studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "0";
                                        }
                                        else
                                        {
                                            studabs.Add(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), "0");
                                        }
                                    }
                                    if (marks >= min_mark || marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;
                                        if (chk_pass.Checked == true)  // Changed By Sridharan 12 March 2015
                                        {


                                            dtrow2[j] = marks_per.ToString();
                                        }
                                    }
                                    else
                                    {
                                        fail++;
                                        if (chk_fail.Checked == true) // Changed By Sridharan 12 March 2015
                                        {
                                            if (marks >= 0)
                                            {


                                                dtrow2[j] = marks_per.ToString();
                                            }
                                            else
                                            {

                                                marks = 0;

                                                dtrow2[j] = marks_per.ToString();
                                            }
                                        }
                                    }
                                    if (chk_abst.Checked == true) // Changed By Sridharan 12 March 2015
                                    {
                                        if (marks < 0 && marks_per != "EL" && marks_per != "EOD")
                                        {

                                            dtrow2[j] = marks_per.ToString();
                                        }
                                    }
                                    tot_marks += marks;
                                    EL = 0;
                                    stu_count++;
                                }
                            }
                        }
                    }
                    //dtable2.Rows.Add(dtrow2);//
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
                    if (pass_fail == "PASS")
                    {
                        no_of_all_clear++;
                        hat.Clear();
                        hat.Add("RollNumber", ds5.Tables[0].Rows[i]["RollNumber"].ToString());
                        hat.Add("criteria_no", criteria_no.ToString());
                        hat.Add("Total", tot_marks.ToString());
                        hat.Add("avg", per_tage.ToString());
                        hat.Add("rank", "");
                        int o = d2.insert_method("INSERT_RANK", hat, "sp");
                        if (ddlreport.Items[21].Selected == true)
                        {
                            if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "1")
                            {
                                gs_pass_count++;
                                tot_gs_count++;
                                gs_count = 1;

                                dtrow2["GPass"] = gs_count.ToString();
                            }
                        }
                        if (ddlreport.Items[22].Selected == true)
                        {
                            if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "0")
                            {
                                bs_pass_count++;
                                tot_bs_count++;
                                bs_count = 1;
                               
                                dtrow2["BPass"] = bs_count.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "1")
                        {
                            if (ddlreport.Items[21].Selected == true)
                            {
                                gs_fail_count++;
                                tot_gs_count++;
                                gs_count = 0;
                            }
                        }
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "0")
                        {
                            if (ddlreport.Items[22].Selected == true)
                            {
                                bs_failcount++;
                                tot_bs_count++;
                                bs_count = 0;
                            }
                        }
                    }
                    if (ddlreport.Items[36].Selected == true)
                    {
                        string re = string.Empty;
                        if (fail == 0)
                        {
                            re = "-";
                        }
                        else
                        {
                            re = fail.ToString();
                        }
                        //FpEntry.Sheets[0].Cells[i, subjectfailedcount].Text = re.ToString();

                        dtrow2["No of Subjects Failed"] = re.ToString();
                    }
                    if (ddlreport.Items[43].Selected == true)
                    {
                        string re = string.Empty;
                        if (stusubabsent == 0)
                        {
                            re = "-";
                        }
                        else
                        {
                            re = stusubabsent.ToString();
                        }
                        //FpEntry.Sheets[0].Cells[i, noofabcout].Text = re.ToString();

                        dtrow2["No of Subjects Absent"] = re.ToString();
                    }
                    stusubabsent = 0;
                    if (dicsubfacount.ContainsKey(fail))
                    {
                        int subfail = dicsubfacount[fail];
                        subfail++;
                        dicsubfacount[fail] = subfail;
                    }
                    
                    dtrow2["Total"] = tot_marks.ToString();
                    dtrow2["Percentage"] = per_tage.ToString();

                    if (pass_fail != null)
                    {
                       
                        dtrow2["Result"] = pass_fail.ToString();
                    }
                    if (ddlreport.Items[24].Selected == true)
                    {
                        
                        dtrow2["NFPS"] = fail.ToString();
                    }
                  
                    //'-----------------------------find the medium for the student------------------------------------
                    string medium1 = string.Empty;
                    if (ddlreport.Items[1].Selected == true)
                    {
                      
                        medium1 = GetFunction("select distinct medium from stud_prev_details where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + " and medium is not NULL");
                        if ((medium1 == "") || (medium1 == null))
                        {
                          
                            dtrow2["Medium"] = "-";
                        }
                        else 
                        {
                            
                            dtrow2["Medium"] = medium1.ToString();
                           
                        }
                        
                    }

                    if (ddlreport.Items[0].Selected == true)
                    {
                        DataView dvrank = new DataView();
                        ////ra_nk = 1;
                        double temp_rank = 0;
                        int zx = 1;
                        ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                        if (ds3.Tables[0].Rows.Count != 0)
                        {
                            //if (i == 0)
                            //{
                            for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                            {
                                if (temp_rank == 0)
                                {
                                    ra_nk = 1;
                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", criteria_no.ToString());
                                    hat.Add("Total", Convert.ToString(tot_marks));
                                    hat.Add("avg", Convert.ToString(per_tage));
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                    zx++;
                                }
                                else if (temp_rank != 0)
                                {
                                    if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                    {
                                        //   ra_nk += 1;
                                        ra_nk = zx;
                                        hat.Clear();
                                        hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                        hat.Add("criteria_no", criteria_no.ToString());
                                        hat.Add("Total", Convert.ToString(tot_marks));
                                        hat.Add("avg", Convert.ToString(per_tage));
                                        hat.Add("rank", ra_nk.ToString());
                                        int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                        temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                        zx++;
                                    }
                                    else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                    {
                                        hat.Clear();
                                        hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                        hat.Add("criteria_no", criteria_no.ToString());
                                        hat.Add("Total", Convert.ToString(tot_marks));
                                        hat.Add("avg", Convert.ToString(per_tage));
                                        hat.Add("rank", ra_nk.ToString());
                                        int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                        temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                    }
                                }
                            }
                            //}
                            ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                            int rank_row_count = 0;

                            string roll = ds5.Tables[0].Rows[i]["RollNumber"].ToString();
                            ds3.Tables[1].DefaultView.RowFilter = "rollno='" + roll + "'";
                            dvrank = ds3.Tables[1].DefaultView;
                            if (dvrank.Count > 0)
                            {
                                dtrow2["Rank"] = dvrank[0]["Rank"].ToString();
                            }
                            else
                            {
                                dtrow2["Rank"] = "-";
                            }
                        }
                    }

                    if (ddlreport.Items[23].Selected == true)//modified on 01.08.12
                    {
                        string textval = string.Empty;
                        if (ds5.Tables[0].Rows[i]["SeatType"].ToString() != "" && ds5.Tables[0].Rows[i]["SeatType"].ToString() != " ")
                        {
                            textval = GetFunction("Select TextVal from textvaltable where textcode=" + ds5.Tables[0].Rows[i]["seattype"].ToString() + "");
                        }
                        else
                        {
                            textval = "-";
                        }


                        dtrow2["Quota"] = textval.ToString();
                        quota_count = quotacount;
                    }

                    //''-----------------------find the 12th percent and 12th group------------------------------------------------------
                    if (ddlreport.Items[2].Selected == true)
                    {
                        string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code=" + ddlBranch.SelectedValue.ToString() + ""; //
                        condegree.Close();
                        condegree.Open();
                        SqlCommand cmddegree = new SqlCommand(s, condegree);
                        SqlDataReader degreereader;
                        degreereader = cmddegree.ExecuteReader();
                        if (degreereader.HasRows)
                        {
                            while (degreereader.Read())
                            {
                                courseid = degreereader[0].ToString();
                                string schoolgrd = GetFunction("select edu_level from course where course_id= " + courseid + "");
                                if (schoolgrd != string.Empty)
                                {
                                    if (schoolgrd == "UG")
                                    {
                                        string scholmrk = GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");
                                        //FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Right;
                                        if (scholmrk != string.Empty)
                                        {

                                            dtrow2["12th/Dip %"] = scholmrk.ToString();
                                        }
                                        else
                                        {

                                            dtrow2["12th/Dip %"] = "-";
                                            
                                        }
                                        string scholmrk1 = GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                        if (scholmrk1 != string.Empty)
                                        {
                                            string sam = scholmrk1.ToString();

                                            dtrow2["12th/Dip Grp"] = scholmrk1.ToString();
                                            
                                        }
                                        else
                                        {
                                            dtrow2["12th/Dip Grp"] = "-";
                                            
                                        }
                                    }
                                    else if (schoolgrd == "PG")
                                    {
                                        string scholmrk2 = GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                        //FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                        if (scholmrk2 != string.Empty)
                                        {

                                            dtrow2["12th/Dip Grp"] = scholmrk2.ToString();
                                        }
                                        else
                                        {

                                            dtrow2["12th/Dip Grp"] = "-";
                                        }
                                        string scholmrk3 = GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                        //FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        if (scholmrk3 != string.Empty)
                                        {
                                            con.Close();

                                            dtrow2["12th/Dip %"] = scholmrk3.ToString();
                                        }
                                        else
                                        {
                                            dtrow2["12th/Dip %"] = "-";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ddlreport.Items[3].Selected == true)
                    {
                        int sem = Convert.ToInt32(ddlSemYr.SelectedValue.ToString());
                        double degcgpa = Math.Round(findgrade(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), sem), 2);
                        degcgpa = Math.Round(degcgpa, 2);

                        dtrow2["CGPA"] = degcgpa.ToString();
                    }
                    if (ddlreport.Items[20].Selected == true)
                    {
                        int g = int.Parse(ds5.Tables[0].Rows[i]["Gen"].ToString());
                        string gender;
                        if (g == 1)
                        {
                            gender = "G";
                        }
                        else
                        {
                            gender = "B";
                        }

                        dtrow2["G/B"] = gender.ToString();
                    }
                    if (ddlreport.Items[17].Selected == true)
                    {
                        if (fail == 0)
                        {
                            if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Hostler")
                            {
                                h_pass_count++;
                                bs_count = 1;

                                dtrow2["HPass"] = bs_count.ToString();
                            }
                        }
                        else
                        {
                            if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Hostler")
                            {
                                h_fail_count++;
                            }
                        }
                    }
                    if (ddlreport.Items[16].Selected == true)
                    {
                        if (fail == 0)
                        {
                            if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Day Scholar")
                            {
                                d_pass_count++;
                                bs_count = 1;

                                dtrow2["DPass"] = bs_count.ToString();
                            }
                        }
                        else
                        {
                            if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Day Scholar")
                            {
                                d_fail_count++;
                            }
                        }
                    }
                    if (ddlreport.Items[18].Selected == true)
                    {
                        //  if (ds5.Tables[0].Rows[i]["medium"].ToString() == "Tamil")
                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                        {
                            t_pass_count++;
                            bs_count = 1;

                            dtrow2["TPass"] = bs_count.ToString();
                        }
                        else
                        {
                            // if (ds5.Tables[0].Rows[i]["medium"].ToString() == "Tamil")
                            if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                            {
                                t_fail_count++;
                            }
                        }
                    }
                    if (ddlreport.Items[19].Selected == true)
                    {
                        // if (ds5.Tables[0].Rows[i]["medium"].ToString() == "English")
                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                        {
                            e_pass_count++;
                            bs_count = 1;

                            dtrow2["EPass"] = bs_count.ToString();
                        }
                        else
                            //if (ds5.Tables[0].Rows[i]["medium"].ToString() == "English")
                            if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                            {
                                e_fail_count++;
                            }
                    }
                    if (ddlreport.Items[23].Selected == true)
                    {
                        dtrow2[quota_count] = getquota;
                        for (int colloop = quota_count; colloop <= dtable2.Columns.Count - 1; colloop++)
                        {
                            gettextcode = dtable2.Columns[colloop].ToString();
                            if (ds5.Tables[0].Rows[i]["seattype"].ToString() == gettextcode.ToString())
                            {
                                if (fail != 0)
                                //if ((strseattype == gettextcode) && (Convert.ToString(FpEntry.Sheets[0].GetText(res, resultcount)) == "Fail"))
                                {
                                    dtrow2[colloop - 1] = "0";
                                    //quotafailcount += 1;
                                }
                                else
                                //if ((strseattype == gettextcode) && (Convert.ToString(FpEntry.Sheets[0].GetText(res, resultcount)) == "Pass"))
                                {
                                    dtrow2[colloop - 1] = "1";
                                    // quotapasscount += 1;
                                }
                            }
                            else
                            {
                                dtrow2[colloop] = "-";
                            }
                        }
                    }
                    res++;
                    tot_marks = 0;
                    per_mark = 0;
                    pass = 0;
                    fail = 0;
                    sub_max_marks = 0;
                    eod_count = 0;
                    EL = 0;
                    pass_fail = string.Empty;
                    bs_count = 0;
                    per_tage = "0";
                    //--------------------attend function-------------------------
                    if (ddlreport.Items[25].Selected == true || ddlreport.Items[26].Selected == true)
                    {
                        if (i == 0)
                        {
                            frdate = txtFromDate.Text.ToString();
                            todate = txtToDate.Text.ToString();
                            string dt = frdate;
                            string[] dsplit = dt.Split(new Char[] { '/' });
                            frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                            demfcal = int.Parse(dsplit[2].ToString());
                            demfcal = demfcal * 12;
                            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                            cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
                            monthcal = cal_from_date.ToString();
                            dt = todate;
                            dsplit = dt.Split(new Char[] { '/' });
                            todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                            demtcal = int.Parse(dsplit[2].ToString());
                            demtcal = demtcal * 12;
                            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                            cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());
                            per_from_gendate = Convert.ToDateTime(frdate);
                            per_to_gendate = Convert.ToDateTime(todate);
                            ht_sphr.Clear();
                            string hrdetno = string.Empty;
                            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + per_from_gendate.ToString() + "' and '" + per_to_gendate.ToString() + "'";
                            ds_sphr = d2.select_method(getsphr, hat, "Text");
                            if (ds_sphr.Tables[0].Rows.Count > 0)
                            {
                                for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                                    {
                                        hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                                        hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                                        ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                                    }
                                    else
                                    {
                                        ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                                    }
                                }
                            }
                        }
                        hat.Clear();
                        hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
                        hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
                        ds4 = dacces2.select_method("period_attnd_schedule", hat, "sp");
                        if (ds4.Tables[0].Rows.Count != 0)
                        {
                            NoHrs = int.Parse(ds4.Tables[0].Rows[0]["PER DAY"].ToString());
                            fnhrs = int.Parse(ds4.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                            anhrs = int.Parse(ds4.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                            minpresI = int.Parse(ds4.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                            minpresII = int.Parse(ds4.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                        }
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds15 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        countds = ds15.Tables[0].Rows.Count;
                        persentmonthcal(i);
                        //'----------------------------------------new start----------------
                        per_con_hrs = per_workingdays1;
                        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);
                        if (per_tage_hrs > 100)
                        {
                            per_tage_hrs = 100;
                        }
                        per_tage_date = ((pre_present_date / per_workingdays) * 100);
                        if (per_tage_date > 100)
                        {
                            per_tage_date = 100;
                        }
                        dum_tage_date = Math.Round(per_tage_date, 2);
                        dum_tage_hrs = Math.Round(per_tage_hrs, 2);
                        if (ddlreport.Items[25].Selected == true || ddlreport.Items[26].Selected == true)
                        {
                            if (Session["Hourwise"] == "1")
                            {
                                if (ddlreport.Items[25].Selected == true)
                                {
                                    dtrow2["No of hrs Attended"] = per_per_hrs.ToString();
                                }
                                if (ddlreport.Items[26].Selected == true)
                                {
                                    dtrow2["Attendance %"] = dum_tage_hrs.ToString();
                                }
                                if (ddlreport.Items[35].Selected == true)
                                {
                                    dtrow2["Conducted Hours"] = per_con_hrs.ToString();
                                }
                            }
                            else
                            {
                                if (ddlreport.Items[25].Selected == true)
                                {
                                    dtrow2["No of hrs Attended"] = pre_present_date.ToString();
                                }
                                if (ddlreport.Items[26].Selected == true)
                                {
                                    dtrow2["Attendance %"] = dum_tage_date.ToString();
                                }
                                if (ddlreport.Items[35].Selected == true)
                                {
                                    dtrow2["Conducted Hours"] = per_con_hrs.ToString();
                                }
                            }
                        }
                    }
                    dtable2.Rows.Add(dtrow2);
                }
                for (int i = 0; i < dtable2.Columns.Count; i++)
                {
                    if (dtable2.Columns[i].ToString() == "MANAGEMENT" || dtable2.Columns[i].ToString() == "COUNSELLING" || dtable2.Columns[i].ToString() == "Lateral Counselling" || dtable2.Columns[i].ToString() == "GOVERNMENT")
                    {
                        dtable2.Columns.RemoveAt(i + 1);
                    }
                }
                //'---------------------------------------------------------------
                fnaltabl = MergeTablesByIndex(dtable1, dtable2);

                if (ddlreport.Items[4].Selected == true)
                {
                    x1 = per_sub_count - 1;
                    nothiddencount = 0;//Modified by srinath 23/5/2014

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "CLASS STRENGTH";
                    dtrow3[6] = ds5.Tables[0].Rows.Count.ToString();
                    dtrow3["Total"] = "Total";
                    dtrow3["Percentage"] = "PASS";
                    dtrow3["Result"] = "FAIL";
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[16].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "DAYS SCHOLAR TOTAL:";
                    dtrow3["Total"] = (d_fail_count + d_pass_count).ToString();
                    dtrow3["Percentage"] = d_pass_count.ToString();
                    dtrow3["Result"] = d_fail_count.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[17].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "HOSTLER TOTAL:";
                    dtrow3["Total"] = (h_fail_count + h_pass_count).ToString();
                    dtrow3["Percentage"] = h_pass_count.ToString();
                    dtrow3["Result"] = h_fail_count.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[18].Selected == true)
                {

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "TAMIL:";
                    dtrow3["Total"] = (t_pass_count + t_fail_count).ToString();
                    dtrow3["Percentage"] = t_pass_count.ToString();
                    dtrow3["Result"] = t_fail_count.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[19].Selected == true)
                {

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "ENGLISH:";
                    dtrow3["Total"] = (e_pass_count + e_fail_count).ToString();
                    dtrow3["Percentage"] = e_pass_count.ToString();
                    dtrow3["Result"] = e_fail_count.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[21].Selected == true)
                {

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "GIRLS:";
                    dtrow3["Total"] = tot_gs_count.ToString();
                    dtrow3["Percentage"] = gs_pass_count.ToString();
                    dtrow3["Result"] = gs_fail_count.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[22].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "BOYS:";
                    dtrow3["Total"] = tot_bs_count.ToString();
                    dtrow3["Percentage"] = bs_pass_count.ToString();
                    dtrow3["Result"] = bs_failcount.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[15].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    int staff_index = 0;
                    dtrow3[nothiddencount] = "STAFF SIGNATURE";
                    for (int staff_col = 6; staff_col < 6 + ds2.Tables[1].Rows.Count; staff_col++)
                    {
                        string temp = string.Empty;
                        string staff = string.Empty;
                        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and exam_type.sections='" + sections.ToString() + "'";
                        }
                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no "+ccflag+" and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[staff_index]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        //FpEntry.Sheets[0].SetText(signat, staff_col, staff.ToString());
                        staff_index++;
                        dtrow3[staff_col] = staff.ToString();
                    }
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (chart.Checked == true)
                {
                    int staff_index = 0;
                    for (int staff_col = 6; staff_col < 6 + ds2.Tables[1].Rows.Count; staff_col++)
                    {
                        string temp = string.Empty;
                        string staff = string.Empty;
                        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and exam_type.sections='" + sections.ToString() + "'";
                        }
                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no "+ccflag+" and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[staff_index]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        // FpEntry.Sheets[0].SetText(signat, staff_col, staff.ToString());
                        staff_index++;
                    }
                }
                //seattypecount
                if (ddlreport.Items[0].Selected == true)
                {
                    DataView dvrank = new DataView();
                    ////ra_nk = 1;
                    double temp_rank = 0;
                    int zx = 1;
                    ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                    if (ds3.Tables[0].Rows.Count != 0)
                    {
                        for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                        {
                            if (temp_rank == 0)
                            {
                                ra_nk = 1;
                                hat.Clear();
                                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                hat.Add("criteria_no", criteria_no.ToString());
                                hat.Add("Total", Convert.ToString(tot_marks));
                                hat.Add("avg", Convert.ToString(per_tage));
                                hat.Add("rank", ra_nk.ToString());
                                int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                zx++;
                            }
                            else if (temp_rank != 0)
                            {
                                if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                {
                                    //   ra_nk += 1;
                                    ra_nk = zx;
                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", criteria_no.ToString());
                                    hat.Add("Total", Convert.ToString(tot_marks));
                                    hat.Add("avg", Convert.ToString(per_tage));
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                    zx++;
                                }
                                else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                {
                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", criteria_no.ToString());
                                    hat.Add("Total", Convert.ToString(tot_marks));
                                    hat.Add("avg", Convert.ToString(per_tage));
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                }
                            }
                        }
                        ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                        int rank_row_count = 0;
                        for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                        {
                            string roll = ds5.Tables[0].Rows[i]["RollNumber"].ToString();
                            ds3.Tables[1].DefaultView.RowFilter = "rollno='" + roll + "'";
                            dvrank = ds3.Tables[1].DefaultView;
                            if (dvrank.Count > 0)
                            {
                                fnaltabl.Rows[i+1]["Rank"] = dvrank[0]["Rank"].ToString();
                               // dtrow2["Rank"] = dvrank[0]["Rank"].ToString();
                            }
                            else
                            {
                                fnaltabl.Rows[i+1]["Rank"] = "-";
                               // dtrow2["Rank"] = "-";
                            }
                            if (dvrank.Count > 0)
                            {
                                //FpEntry.Sheets[0].Cells[i, rankcount].Text = dvrank[0]["Rank"].ToString();
                                //FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                //FpEntry.Sheets[0].Cells[i, rankcount].Text = "-";
                                //FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //if (rank_row_count < ds3.Tables[1].Rows.Count)
                            //{
                            //    if (ds3.Tables[1].Rows[rank_row_count]["Rollno"].ToString() == ds5.Tables[0].Rows[i]["RollNumber"].ToString())
                            //    {
                            //        FpEntry.Sheets[0].Cells[i, rankcount].Text = ds3.Tables[1].Rows[rank_row_count]["Rank"].ToString();
                            //        rank_row_count++;
                            //    }
                            //}
                        }
                        
                    }
                }

                #region RAY_BIND

                string sec1 = string.Empty;
                if (ddlSec.Enabled == true) // added by sridhar aug 2014
                {
                    sec1 = ddlSec.SelectedItem.Text.ToString();
                }
                else
                {
                    sec1 = string.Empty;
                }
                if (sec1.ToString().Trim() == "-1" || sec1.ToString().Trim() == "" || sec1.ToString().Trim() == null || sec1.ToString().Trim() == "All")
                {
                    sec1 = string.Empty; // added by sridhar aug 2014
                }
                else
                {
                    sec1 = ddlSec.SelectedItem.Text; // added by sridhar aug 2014
                }

                int rows_count = 6;
                if (ddlreport.Items[34].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "EXAM DATE:";

                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        //---end--//                        
                        dtrow3[rows_count] = date.ToString();
                        rows_count++;
                    }

                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[7].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "PASS COUNT:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        ds4 = d2.select_method("PassCount", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["PASS_COUNT"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[8].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "FAIL COUNT:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        ds4 = d2.select_method("FailCount", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();//table[2] for failcount with absent
                        rows_count++;
                    }

                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[13].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "MAX MARK:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("[MAXMARK]", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["MAX_MARK"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "MIN MARK:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("MINMARK", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["MIN_MARK"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[33].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "MAX ROLL NUMBER";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("MAX_ROLL_NO", hat, "sp");
                        //---end--//
                        string name = GetFunction("select stud_name from registration where roll_no='" + ds4.Tables[0].Rows[0]["roll_no"].ToString() + "'");
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["roll_no"].ToString() + '-' + name;
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "MIN ROLL NUMBER";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("MIN_ROLL_NO", hat, "sp");
                        //---end--//
                        string namemin = GetFunction("select stud_name from registration where roll_no='" + ds4.Tables[0].Rows[0]["roll_no"].ToString() + "'");
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["roll_no"].ToString() + '-' + namemin;
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[9].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVG < 50 MARK:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_LESS_50", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG<50"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[10].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVG 50 To 65:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_50_60", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG_50to65"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[11].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVG > 60:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_GREATE_60", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG>=60"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[37].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVG > 65:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_LESS_65", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG>65"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[38].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVG > 80:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_80", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG>=80"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[5].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "PRESENT COUNT:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("[PresentCount]", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["PRESENT_COUNT"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[6].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "ABSENT COUNT:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AbsentCount", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["ABSENT_COUNT"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[14].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "PASS PERCENTAGE:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("PassPercentage", hat, "sp");
                        //---end--//
                        if (ds4.Tables.Count != 0)
                        {
                            double absentCount = 0;
                            if (chkIncludeAbsent.Checked)
                            {
                                string absent = Convert.ToString(ds4.Tables[2].Rows[0]["ABSENT_COUNT"]).Trim();
                                double.TryParse(absent.Trim(), out absentCount);
                            }
                            double final_pperc = 0;
                            //calculate pass perc by present
                            final_pperc = (Convert.ToDouble(ds4.Tables[0].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(ds4.Tables[1].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                            final_pperc = Math.Round(final_pperc, 2);
                            dtrow3[rows_count] = final_pperc.ToString();
                            rows_count++;
                        }
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (optinalminpass > 0)
                {
                    if (ddlreport.Items[7].Selected == true)
                    {
                        dtrow3 = fnaltabl.NewRow();
                        dtrow3[nothiddencount] = "NO OF STUDENTS PASSED FOR " + optinalminpass + "%:";
                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                        {
                            //---end--//
                            if (ds4.Tables.Count != 0)
                            {
                                hat.Clear();
                                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                                hat.Add("min_marks", optinalminpass);
                                hat.Add("section", sec1);
                                DataSet dsopt = d2.select_method("PassCount", hat, "sp");

                                dtrow3[rows_count] = dsopt.Tables[0].Rows[0]["PASS_COUNT"].ToString();
                            }
                            rows_count++;
                        }
                        fnaltabl.Rows.Add(dtrow3);
                        rows_count = 6;
                    }
                    if (ddlreport.Items[8].Selected == true)
                    {
                        dtrow3 = fnaltabl.NewRow();
                        dtrow3[nothiddencount] = "NO OF STUDENTS FAILED FOR " + optinalminpass + "%:";
                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                        {
                            string date = string.Empty;
                            string sgcode = string.Empty;
                            hat.Clear();
                            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("section", sec1); //----modified by annyutha----//
                            date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                            sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                            ds4 = d2.select_method("FailCount", hat, "sp");
                            //---end--//
                            if (ds4.Tables.Count != 0)
                            {
                                hat.Clear();
                                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                                hat.Add("min_marks", optinalminpass);
                                hat.Add("section", sec1);
                                DataSet dsopt = d2.select_method("FailCount", hat, "sp");

                                dtrow3[rows_count] = dsopt.Tables[0].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();//table[2] for failcount with absent                                
                            }
                            rows_count++;
                        }
                        fnaltabl.Rows.Add(dtrow3);
                        rows_count = 6;
                    }
                    if (ddlreport.Items[14].Selected == true)
                    {
                        dtrow3 = fnaltabl.NewRow();
                        dtrow3[nothiddencount] = "PASS PERCENTAGE (FOR " + optinalminpass + "):";
                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                        {
                            string date = string.Empty;
                            string sgcode = string.Empty;
                            hat.Clear();
                            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("section", sec1); //----modified by annyutha----//
                            date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                            sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                            ds4 = d2.select_method("PassPercentage", hat, "sp");
                            //---end--//
                            if (ds4.Tables.Count != 0)
                            {
                                hat.Clear();
                                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                                hat.Add("min_marks", optinalminpass);
                                hat.Add("section", sec1);
                                DataSet dsopt = d2.select_method("PassPercentage", hat, "sp");

                                double final_pperc = 0;
                                double absentCount = 0;
                                if (chkIncludeAbsent.Checked)
                                {
                                    string absent = Convert.ToString(ds4.Tables[2].Rows[0]["ABSENT_COUNT"]).Trim();
                                    double.TryParse(absent.Trim(), out absentCount);
                                }
                                final_pperc = (Convert.ToDouble(dsopt.Tables[0].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(dsopt.Tables[1].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                                final_pperc = Math.Round(final_pperc, 2);
                                dtrow3[rows_count] = final_pperc.ToString();
                            }
                            rows_count++;
                        }
                        fnaltabl.Rows.Add(dtrow3);
                        rows_count = 6;
                    }

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "NO OF STUDENTS PASSED " + ds2.Tables[1].Rows[0]["min_mark"].ToString() + "%:";
                    for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[j]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[j]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[j]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[j]["subject_code"].ToString();
                        ds4 = d2.select_method("PassCount", hat, "sp");
                        //---end--//
                        if (ds4.Tables.Count != 0)
                        {
                            dtrow3[rows_count] = ds4.Tables[0].Rows[0]["PASS_COUNT"].ToString();
                        }
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "NO OF STUDENTS FAILED " + ds2.Tables[1].Rows[0]["min_mark"].ToString() + "%:";
                    for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[j]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[j]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[j]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[j]["subject_code"].ToString();
                        ds4 = d2.select_method("FailCount", hat, "sp");
                        //---end--//
                        if (ds4.Tables.Count != 0)
                        {
                            dtrow3[rows_count] = ds4.Tables[0].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();//table[2] for failcount with absent
                        }
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "PASS PERCENTAGE (FOR " + ds2.Tables[1].Rows[0]["min_mark"].ToString() + ")";
                    for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[j]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[j]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[j]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[j]["subject_code"].ToString();
                        ds4 = d2.select_method("PassPercentage", hat, "sp");
                        //---end--//
                        if (ds4.Tables.Count != 0)
                        {
                            hat.Clear();
                            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("min_marks", optinalminpass);
                            hat.Add("section", sec1);
                            DataSet dsopt = d2.select_method("PassPercentage", hat, "sp");

                            double final_pperc = 0;
                            double absentCount = 0;
                            if (chkIncludeAbsent.Checked)
                            {
                                string absent = Convert.ToString(ds4.Tables[2].Rows[0]["ABSENT_COUNT"]).Trim();
                                double.TryParse(absent.Trim(), out absentCount);
                            }
                            final_pperc = (Convert.ToDouble(dsopt.Tables[0].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(dsopt.Tables[1].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                            final_pperc = Math.Round(final_pperc, 2);
                            dtrow3[rows_count] = final_pperc.ToString();
                        }
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[12].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "CLASS AVERAGE:";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("CLASSAVERAGE", hat, "sp");
                        //---end--//
                        double final_avg_value = (Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(ds4.Tables[1].Rows[0]["PRESENT_COUNT"]));
                        final_avg_value = Math.Round(final_avg_value, 2);
                        dtrow3[rows_count] = final_avg_value.ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[27].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE >= 75";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_75", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG>=75"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[28].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE >= 60 and <=74";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_60_70", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG60to74"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[29].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE >= 50 and <=59";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_50_59", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG50to59"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[30].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE >= 30 and <=49";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_30_49", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG30to49"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[31].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE >= 20 and <=29";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_20_29", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG20to29"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                if (ddlreport.Items[32].Selected == true)
                {
                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "AVERAGE <=19";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec1); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("AVG_LESS_19", hat, "sp");
                        //---end--//
                        dtrow3[rows_count] = ds4.Tables[0].Rows[0]["AVG<=19"].ToString();
                        rows_count++;
                    }
                    fnaltabl.Rows.Add(dtrow3);
                    rows_count = 6;
                }
                DataSet dsgrade1 = new DataSet();
                int graderow1 = 0;
                int grsem1 = 0;
                if (ddlreport.Items[41].Selected == true)
                {
                    string strgradesem = d2.GetFunction("select distinct Semester from grade_master where batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + semester + "'");
                    if (strgradesem.Trim() != "" && strgradesem != null)
                    {
                        grsem1 = Convert.ToInt32(strgradesem);
                        dsgrade1 = d2.select_method_wo_parameter("select  Mark_Grade,trange from grade_master where batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + grsem1 + "' and Credit_Points>0 order by Trange desc", "text");
                        if (dsgrade1.Tables[0].Rows.Count > 0)
                        {
                            //graderow1 = FpEntry.Sheets[0].RowCount;
                            graderow1 = dtable1.Rows.Count;
                            if (graderow1 > 0)
                            {
                                for (int dg = 0; dg < dsgrade1.Tables[0].Rows.Count; dg++)
                                {
                                    dtrow3 = fnaltabl.NewRow();
                                    dtrow3[nothiddencount] = "NO OF STUDENTS SECURED " + dsgrade1.Tables[0].Rows[dg]["Mark_Grade"].ToString() + " GRADE";
                                    for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                                    {
                                        string gerdaeste = "select g.Mark_Grade,g.Trange,(select  count(r.roll_no)  from Result r,Exam_type e where r.exam_code=e.exam_code and r.exam_code='" + ds2.Tables[1].Rows[j]["exam_code"].ToString() + "' and (r.marks_obtained/e.max_mark *100) between g.Frange and g.Trange) as studcoun from grade_master g where  batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + grsem1 + "' and g.Credit_Points>0 order by g.Trange desc";
                                        dsgrade1.Reset();
                                        dsgrade1.Dispose();
                                        dsgrade1 = d2.select_method_wo_parameter(gerdaeste, "Text");
                                        //for (int dg1 = 0; dg1 < dsgrade1.Tables[0].Rows.Count; dg1++)
                                        //{
                                        //FpEntry.Sheets[0].Cells[graderow + dg1, rows_count].Text = dsgrade.Tables[0].Rows[dg1]["studcoun"].ToString();
                                        dtrow3[rows_count] = dsgrade1.Tables[0].Rows[dg]["studcoun"].ToString();
                                        //}
                                        rows_count++;
                                    }
                                    fnaltabl.Rows.Add(dtrow3);
                                    rows_count = 6;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Command
                ////rows_count = 6;//int
                ////if (ddlreport.Items[34].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    exdate = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(exdate, nothiddencount, "EXAM DATE:");
                ////    FpEntry.Sheets[0].SpanModel.Add(exdate, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[7].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    passcount = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(passcount, nothiddencount, "PASS COUNT:");
                ////    FpEntry.Sheets[0].SpanModel.Add(passcount, 0, 1, 5);
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[8].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    failcount = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(failcount, nothiddencount, "FAIL COUNT:");
                ////    FpEntry.Sheets[0].SpanModel.Add(failcount, 0, 1, 5);
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (optinalminpass > 0)
                ////{
                ////    if (ddlreport.Items[7].Selected == true)
                ////    {
                ////        FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////        opasscoun = FpEntry.Sheets[0].RowCount - 1;
                ////        FpEntry.Sheets[0].SetText(opasscoun, nothiddencount, "NO OF STUDENTS PASSED FOR " + optinalminpass + "%:");
                ////        FpEntry.Sheets[0].SpanModel.Add(opasscoun, 0, 1, 5);
                ////        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    }
                ////    if (ddlreport.Items[8].Selected == true)
                ////    {
                ////        FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////        ofailcount = FpEntry.Sheets[0].RowCount - 1;
                ////        FpEntry.Sheets[0].SetText(ofailcount, nothiddencount, "NO OF STUDENTS FAILED FOR " + optinalminpass + "%:");
                ////        FpEntry.Sheets[0].SpanModel.Add(ofailcount, 0, 1, 5);
                ////        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    }
                ////}
                ////if (ddlreport.Items[13].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    maxcount = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(maxcount, nothiddencount, "MAX MARK:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    mincount = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(mincount, nothiddencount, "MIN MARK:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[33].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    maxrollnum = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(maxrollnum, nothiddencount, " MAX ROLL NUMBER");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    minrollnum = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(minrollnum, nothiddencount, "MIN ROLL NUMBER");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[9].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avg_50count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avg_50count, nothiddencount, "AVG < 50 MARK:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[10].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avg_65count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avg_65count, nothiddencount, "AVG 50 To 65:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[11].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avgg65count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avgg65count, nothiddencount, "AVG > 60:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[37].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avg_60count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avg_60count, nothiddencount, "AVG > 65:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[38].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avg_80count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avg_80count, nothiddencount, "AVG > 80:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[5].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    pre_count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(pre_count, nothiddencount, "PRESENT COUNT:");
                ////    FpEntry.Sheets[0].SpanModel.Add(pre_count, 0, 1, 5);
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[6].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    ab_count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(ab_count, nothiddencount, "ABSENT COUNT:");
                ////    FpEntry.Sheets[0].SpanModel.Add(ab_count, 0, 1, 5);
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[14].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    pperc_count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(pperc_count, nothiddencount, "PASS PERCENTAGE:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (optinalminpass > 0)
                ////{
                ////    if (ddlreport.Items[14].Selected == true)
                ////    {
                ////        FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////        opperc_count = FpEntry.Sheets[0].RowCount - 1;
                ////        FpEntry.Sheets[0].SetText(opperc_count, nothiddencount, "PASS PERCENTAGE (FOR " + optinalminpass + "):");
                ////        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    }
                ////}
                ////if (ddlreport.Items[12].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    avg_count = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(avg_count, nothiddencount, "CLASS AVERAGE:");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[27].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc75 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc75, nothiddencount, "AVERAGE >= 75");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[28].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc60to74 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc60to74, nothiddencount, "AVERAGE >= 60 and <=74");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[29].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc50to59 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc50to59, nothiddencount, "AVERAGE >= 50 and <=59");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[30].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc30to49 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc30to49, nothiddencount, "AVERAGE >= 30 and <=49");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[31].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc20to29 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc20to29, nothiddencount, "AVERAGE >= 20 and <=29");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                ////if (ddlreport.Items[32].Selected == true)
                ////{
                ////    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                ////    perc19 = FpEntry.Sheets[0].RowCount - 1;
                ////    FpEntry.Sheets[0].SetText(perc19, nothiddencount, "AVERAGE <=19");
                ////    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                ////    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                ////}
                #endregion
                DataSet dsgrade = new DataSet();
                int graderow = 0;
                int grsem = 0;
                if (ddlreport.Items[41].Selected == true)
                {
                    string strgradesem = d2.GetFunction("select distinct Semester from grade_master where batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + semester + "'");
                    if (strgradesem.Trim() != "" && strgradesem != null)
                    {
                        grsem = Convert.ToInt32(strgradesem);
                        dsgrade = d2.select_method_wo_parameter("select  Mark_Grade,trange from grade_master where batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + grsem + "' and Credit_Points>0 order by Trange desc", "text");
                        if (dsgrade.Tables[0].Rows.Count > 0)
                        {
                            //graderow = FpEntry.Sheets[0].RowCount;
                            for (int dg = 0; dg < dsgrade.Tables[0].Rows.Count; dg++)
                            {
                                //FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                //FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, nothiddencount, "NO OF STUDENTS SECURED " + dsgrade.Tables[0].Rows[dg]["Mark_Grade"].ToString() + " GRADE");
                                //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                if (ddlreport.Items[42].Selected == true)
                {
                    for (int sub = 1; sub <= count; sub++)
                    {
                        dtrow3 = fnaltabl.NewRow();

                        dtrow3[nothiddencount] = "" + sub + " SUBJECT FAILURE :";
                        int stico = 0;
                        if (dicsubfacount.ContainsKey(sub))
                        {
                            stico = dicsubfacount[sub];
                        }
                        dtrow3[rows_count] = stico.ToString();
                        fnaltabl.Rows.Add(dtrow3);
                    }
                }
                if (ddlreport.Items[39].Selected == true)//Modified by srinath 23/5/2014
                {

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "NO OF ALL CLEARED:";
                    dtrow3[6] = no_of_all_clear.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                if (ddlreport.Items[40].Selected == true)
                {
                    int present_count = 0;
                    foreach (DictionaryEntry Parameters in studabs)
                    {
                        if (Convert.ToString(Parameters.Value) == "0")
                        {
                            present_count = present_count + 1;
                        }
                    }
                    double per_noof_allclear = 0;
                    per_noof_allclear = (Convert.ToDouble(no_of_all_clear) / Convert.ToDouble(present_count)) * 100;
                    per_noof_allclear = Math.Round(per_noof_allclear, 2);

                    dtrow3 = fnaltabl.NewRow();
                    dtrow3[nothiddencount] = "% OF ALL CLEARED:";
                    dtrow3[6] = per_noof_allclear.ToString();
                    fnaltabl.Rows.Add(dtrow3);
                }
                string sec = string.Empty;
                if (ddlSec.Enabled == true) // added by sridhar aug 2014
                {
                    sec = ddlSec.SelectedItem.Text.ToString();
                }
                else
                {
                    sec = string.Empty;
                }
                if (sec.ToString().Trim() == "-1" || sec.ToString().Trim() == "" || sec.ToString().Trim() == null || sec.ToString().Trim() == "All")
                {
                    sec = string.Empty; // added by sridhar aug 2014
                }
                else
                {
                    sec = ddlSec.SelectedItem.Text; // added by sridhar aug 2014
                }
                if (ds2.Tables[1].Rows.Count != 0)
                {
                    Chart1.Series.Clear();
                    chartmin_mark = ds2.Tables[0].Rows[1]["min_mark"].ToString();
                    chartmin_mark_optional = txtoptiminpassmark.Text.ToString();
                    Chart1.Series.Add("Pass % (" + chartmin_mark + ")");
                    Chart1.Series[0].BorderWidth = 2;
                    Chart1.Series.Add("Optional Pass % (" + txtoptiminpassmark.Text.Trim() + ")");
                    Chart1.Series[1].BorderWidth = 2;
                    Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                    Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string date = string.Empty;
                        string sgcode = string.Empty;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", sec); //----modified by annyutha----//
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        DataSet dsopt = d2.select_method("PassPercentage", hat, "sp");
                        //---end--//
                        if (ds4.Tables.Count != 0)
                        {
                            double absentCount = 0;
                            if (chkIncludeAbsent.Checked)
                            {
                                string absent = Convert.ToString(dsopt.Tables[2].Rows[0]["ABSENT_COUNT"]).Trim();
                                double.TryParse(absent.Trim(), out absentCount);
                            }
                            if (charthash2.ContainsKey(ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim())) // sridharan 07mar2015
                            {
                                passedcount_chart_opt++;
                                string sub_hash = ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                double final_pperc = 0;

                                //g1[passedcount_chart_opt] = ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                g3[passedcount_chart_opt] = dsopt.Tables[0].Rows[0]["PASS_COUNT"];
                                final_pperc = (Convert.ToDouble(dsopt.Tables[0].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(dsopt.Tables[1].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                                final_pperc = Math.Round(final_pperc, 2);
                                g6[passedcount_chart_opt] = final_pperc;
                                Chart1.Series[1].Points.AddXY(sub_hash, final_pperc);
                                chartdr11[passedcount_chart_opt] = final_pperc;
                                charthash2.Remove(sub_hash);
                                charthash2.Add(sub_hash, final_pperc);
                            }
                            //}
                            //}
                            if (ddlreport.Items[41].Selected == true)
                            {
                                if (graderow > 0)
                                {
                                    string gerdaeste = "select g.Mark_Grade,g.Trange,(select  count(r.roll_no)  from Result r,Exam_type e where r.exam_code=e.exam_code and r.exam_code='" + ds2.Tables[1].Rows[i]["exam_code"].ToString() + "' and (r.marks_obtained/e.max_mark *100) between g.Frange and g.Trange) as studcoun from grade_master g where  batch_year=" + batch + " and Degree_Code=" + degreecode + " and Semester='" + grsem + "' and g.Credit_Points>0 order by g.Trange desc";
                                    dsgrade.Reset();
                                    dsgrade.Dispose();
                                    dsgrade = d2.select_method_wo_parameter(gerdaeste, "Text");
                                }
                            }
                            if (charthash1.ContainsKey(ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim())) // sridharan 07mar2015
                            {
                                passedcount_chart++;
                                string sub_hash = ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                double final_pperc = 0;
                                //g1[passedcount_chart] = ds2.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                g2[passedcount_chart] = Convert.ToString(dsopt.Tables[0].Rows[0]["PASS_COUNT"]).Trim();
                                //calculate pass perc by present
                                final_pperc = (Convert.ToDouble(dsopt.Tables[0].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(Convert.ToString(dsopt.Tables[1].Rows[0]["PRESENT_COUNT"]).Trim()) + absentCount)) * 100;
                                final_pperc = Math.Round(final_pperc, 2);
                                g5[passedcount_chart_opt] = final_pperc;
                                Chart1.Series[0].Points.AddXY(sub_hash, final_pperc);
                                chartdr10[passedcount_chart_opt] = final_pperc;
                                charthash1.Remove(sub_hash);
                                charthash1.Add(sub_hash, final_pperc);
                            }
                            if (chart.Checked == true)
                            {
                                ds4 = d2.select_method("CLASSAVERAGE", hat, "sp");
                                sub_avg_chart++;
                                double final_avg_value = (Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(ds4.Tables[1].Rows[0]["PRESENT_COUNT"]));
                                final_avg_value = Math.Round(final_avg_value, 2);

                                g4[sub_avg_chart] = final_avg_value.ToString();//sridharan

                            }
                            rows_count++;
                        }
                    }
                }
                g1[0] = "STAFF INCHARGE";
                g2[0] = "NO OF STUDENTS PASSED(FOR " + chartmin_mark + " )";
                g3[0] = "NO OF STUDENTS PASSED(FOR " + chartmin_mark_optional + " )";
                g4[0] = "SUBJECT AVERAGE";
                g5[0] = "PASS PERCENTAGE (FOR " + chartmin_mark + " )";
                g6[0] = "PASS PERCENTAGE (FOR " + chartmin_mark_optional + " )";
                chartmin_mark = "Pass % (" + chartmin_mark + ")";
                chartmin_mark_optional = "Optional Pass % (" + chartmin_mark_optional + ")";
                chartdr10[0] = chartmin_mark;
                chartdr11[0] = chartmin_mark_optional;
                //'----------------new load subj name , code-----------
                //-----------------------------Start------------------------------By M.SakthiPriya 09-12-2014                
                int no = 0;

                dtrow3 = fnaltabl.NewRow();
                dtrow3[6] = "S.No";
                dtrow3[7] = "Subject Code";
                dtrow3[8] = "Subject Name";
                dtrow3[9] = "Staff Name";
                fnaltabl.Rows.Add(dtrow3);
                //-----------------------------End------------------------------By M.SakthiPriya 09-12-2014
                //int row_span_start = FpEntry.Sheets[0].RowCount - 1;
                int incrrowcnt = 1;
                int subrow = 0;
                if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and ss.sections='" + sections.ToString() + "'";
                }
                string strsubstaff = "select sm.staff_name,subject_code from subject s,staff_selector ss,staffmaster sm,syllabus_master sy,sub_sem sb  where s.subject_no=ss.subject_no and sm.staff_code=ss.staff_code and sy.syll_code=sb.syll_code and sb.subtype_no=s.subtype_no and sy.batch_year=" + ddlBatch.SelectedValue.ToString() + " and sy.degree_code=" + ddlBranch.SelectedValue.ToString() + " and sy.semester=" + ddlSemYr.SelectedItem.ToString() + " " + strsec + "";
                DataSet dssubstaff = d2.select_method_wo_parameter(strsubstaff, "Text");
                if (ds2.Tables[1].Rows.Count > 0)
                {
                    int totrowcnt = fnaltabl.Rows.Count + ds2.Tables[1].Rows.Count;
                    for (subrow = fnaltabl.Rows.Count; subrow < totrowcnt; subrow++) //changed 21.02.12
                    {
                        if (incrrowcnt <= ds2.Tables[1].Rows.Count)
                        {
                            //FpEntry.Sheets[0].RowCount += 1;
                            //-----------------------------Start------------------------------By M.SakthiPriya 09-12-2014
                            no++;

                            dtrow3 = fnaltabl.NewRow();
                            dtrow3[6] = Convert.ToString(no);
                            dtrow3[7] = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_code"].ToString();
                            dtrow3[8] = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_name"].ToString();
                            //-----------------------------End------------------------------By M.SakthiPriya 09-12-2014
                            string temp = string.Empty;
                            string staff = string.Empty;
                            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                            {
                                strsec = string.Empty;
                            }
                            else
                            {
                                strsec = " and exam_type.sections='" + sections.ToString() + "'";
                            }
                            dssubstaff.Tables[0].DefaultView.RowFilter = " subject_code='" + ds2.Tables[1].Rows[incrrowcnt - 1]["subject_code"].ToString() + "'";
                            DataView dvstaff = dssubstaff.Tables[0].DefaultView;
                            for (int st = 0; st < dvstaff.Count; st++)
                            {
                                if (staff == "")
                                {
                                    staff = dvstaff[st]["staff_name"].ToString();
                                }
                                else
                                {
                                    staff = staff + " , " + dvstaff[st]["staff_name"].ToString();
                                }
                            }
                            //temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[incrrowcnt - 1]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
                            //if (temp != "")
                            //{
                            //    staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                            //}
                            //FpEntry.Sheets[0].SetText(subrow, 9, staff);// Modify By M.SakthiPriya 09-12-2014
                            staff_name_chart++;
                            g1[staff_name_chart] = staff;
                            incrrowcnt++;
                            dtrow3[9] = staff;
                            fnaltabl.Rows.Add(dtrow3);
                        }
                    }
                }
                //-------------spaning the unwanted cell 030412
                colrowcount = 1;
                //if (Session["Regflag"].ToString() == "0")
                //{
                //    colrowcount++;
                //    fnaltabl.Columns.RemoveAt(2);
                //}
               // Session["rowcount"] = dtable2.Rows.Count;
                //if (Session["Rollflag"].ToString() == "0")
                //{
                //    colrowcount++;
                //    fnaltabl.Columns.RemoveAt(1);
                //}
                //if (Session["Studflag"].ToString() == "0")
                //{
                //    colrowcount++;
                //    fnaltabl.Columns.RemoveAt(4);
                //}
                Session["colrowcount"] = colrowcount;
               // fnaltabl.Columns.RemoveAt(4);

                gview.DataSource = fnaltabl;
                gview.DataBind();
               
                minmarks = 0;
                rowcount = 0;
                if (gview.Rows.Count > 1)
                {
                    gview.Visible = true;
                }
                RowHead(gview, 1);

                for (int roww = 0; roww < gview.Rows.Count; roww++)
                {
                    for (int coll = 0; coll < gview.Rows[roww].Cells.Count; coll++)
                    {
                        if (gview.HeaderRow.Cells[coll].Text == "Management" || gview.HeaderRow.Cells[coll].Text == "Counselling")
                        {
                            gview.Rows[roww].Cells[coll + 1].Visible = false;
                        }
                    }
                }

                for (int row = fnaltabl.Rows.Count - 1; row > dtable1.Rows.Count - 1; row--)
                {
                    gview.Rows[row].Cells[0].ColumnSpan = 4;
                    for (int cell = 1; cell < 4; cell++)
                    {
                        gview.Rows[row].Cells[cell].Visible = false;
                    }
                }
                for (int align = 0; align < gview.Rows.Count; align++)
                {
                    for (int cell = 0; cell < gview.HeaderRow.Cells.Count; cell++)
                    {
                        if (gview.HeaderRow.Cells[cell].Text != "Student Name" && gview.HeaderRow.Cells[cell].Text != "Quota")
                        {
                            gview.Rows[align].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                            gview.Rows[align].Cells[cell].VerticalAlign = VerticalAlign.Middle;
                        }
                        if (gview.Rows[align].Cells[cell].Text == "S.No" && gview.Rows[align].Cells[cell].Text == "Subject Code" && gview.Rows[align].Cells[cell].Text != "Subject Name" && gview.Rows[align].Cells[cell].Text == "Staff Name")
                        {
                            gview.Rows[align].Cells[cell].Font.Bold = true;
                        }
                    }
                }

            }//condn end for ds2 row count
            else
            {
                lblnorec.Text = "Test has not been conducted for any subject";
                lblnorec.Visible = true;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
            }
            if (chart.Checked == true)
            {
                if (txtoptiminpassmark.Text.Trim() != null && txtoptiminpassmark.Text.Trim() != "")
                {
                    chartdtgrid.Rows.Add(g1);
                    chartdtgrid.Rows.Add(g2);
                    chartdtgrid.Rows.Add(g3);
                    chartdtgrid.Rows.Add(g4);
                    chartdtgrid.Rows.Add(g5);
                    chartdtgrid.Rows.Add(g6);
                    chartdt1.Rows.Add(chartdr10);
                    chartdt1.Rows.Add(chartdr11);
                    Chart1.Series[0].IsValueShownAsLabel = true;
                    Chart1.Series[1].IsValueShownAsLabel = true;
                    GridViewchart.DataSource = chartdt1;
                    GridViewchart.DataBind();
                    GridViewselectedfield.DataSource = chartdtgrid;
                    GridViewselectedfield.DataBind();
                    GridViewchart.Visible = true;
                    Chart1.Visible = true;
                    GridViewselectedfield.Visible = true;
                    btnExcelchart.Visible = true;
                    btnPrintchart.Visible = true;
                    for (int ij = 0; ij < GridViewselectedfield.Rows.Count; ij++)
                    {
                        GridViewselectedfield.Rows[ij].HorizontalAlign = HorizontalAlign.Center;
                        GridViewselectedfield.Rows[ij].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    GridViewchart.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    GridViewchart.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    GridViewchart.Rows[0].Font.Bold = true;
                    GridViewchart.Rows[1].Font.Bold = true;
                    GridViewchart.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    GridViewchart.Rows[1].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    GridViewselectedfield.Rows[0].Cells[0].Font.Bold = true;
                    GridViewselectedfield.Rows[5].Font.Bold = true;
                    GridViewselectedfield.Rows[5].Cells[0].Font.Bold = false;
                    GridViewselectedfield.Rows[4].Font.Bold = true;
                    GridViewselectedfield.Rows[4].Cells[0].Font.Bold = false;
                    GridViewchart.Rows[0].Cells[0].Font.Bold = false;
                    GridViewchart.Rows[1].Cells[0].Font.Bold = false;
                    //GridViewselectedfield.Columns[0].HeaderStyle.Width = 120;
                }
            }
            Session.Add("data", fnaltabl);
        }
        catch
        {
        }
    }

    protected void gviewOnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                if (Session["Regflag"].ToString() == "0")
                {
                    e.Row.Cells[2].Visible = false;

                    // fnaltabl.Columns.RemoveAt(2);
                }
                else
                {
                    e.Row.Cells[2].Visible = true;
                }

                if (Session["Rollflag"].ToString() == "0")
                {
                    e.Row.Cells[1].Visible = false;
                    //fnaltabl.Columns.RemoveAt(1);
                }
                else
                {
                    e.Row.Cells[1].Visible = true;
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    e.Row.Cells[4].Visible = false;
                    // fnaltabl.Columns.RemoveAt(4);
                }
                else
                {
                    e.Row.Cells[4].Visible = true;
                }
                e.Row.Cells[5].Visible = false;
                if (e.Row.RowIndex != 0)
                {
                   // int cell1 = Convert.ToInt32(Session["colrowcount"])+1;
                    if (e.Row.RowIndex <= rowcount)
                    {
                        for (int cell = 1; cell < e.Row.Cells.Count; cell++)
                        {
                            if (gview.HeaderRow.Cells[cell].Text != "Sl.No" && gview.HeaderRow.Cells[cell].Text != "Roll No" && gview.HeaderRow.Cells[cell].Text != "Reg No" && gview.HeaderRow.Cells[cell].Text != "Student Name" && gview.HeaderRow.Cells[cell].Text != "Total" && gview.HeaderRow.Cells[cell].Text != "Percentage" && gview.HeaderRow.Cells[cell].Text != "Result" && gview.HeaderRow.Cells[cell].Text != "No of Subjects Failed" && gview.HeaderRow.Cells[cell].Text != "No of Subjects Failed" && gview.HeaderRow.Cells[cell].Text != "No of Subjects Absent" && gview.HeaderRow.Cells[cell].Text != "Rank" && gview.HeaderRow.Cells[cell].Text != "Medium" && gview.HeaderRow.Cells[cell].Text != "12th/Dip Grp" && gview.HeaderRow.Cells[cell].Text != "12th/Dip %" && gview.HeaderRow.Cells[cell].Text != "CGPA" && gview.HeaderRow.Cells[cell].Text != "NFPS" && gview.HeaderRow.Cells[cell].Text != "DPass" && gview.HeaderRow.Cells[cell].Text != "HPass" && gview.HeaderRow.Cells[cell].Text != "TPass" && gview.HeaderRow.Cells[cell].Text != "EPass" && gview.HeaderRow.Cells[cell].Text != "G/B" && gview.HeaderRow.Cells[cell].Text != "GPass" && gview.HeaderRow.Cells[cell].Text != "BPass" && gview.HeaderRow.Cells[cell].Text.ToLower() != "quota" && gview.HeaderRow.Cells[cell].Text.ToUpper() != "MANAGEMENT" && gview.HeaderRow.Cells[cell].Text.ToUpper() != "COUNSELLING" && gview.HeaderRow.Cells[cell].Text != "Lateral Counselling" && gview.HeaderRow.Cells[cell].Text.ToUpper() != "GOVERNMENT" && gview.HeaderRow.Cells[cell].Text != "Conducted Hours" && gview.HeaderRow.Cells[cell].Text != "Noofhrs.Attended" && gview.HeaderRow.Cells[cell].Text != "Attendance %")
                            {
                                string val = Convert.ToString(e.Row.Cells[cell].Text);
                                if (!string.IsNullOrEmpty(val) && val.Trim() != "&nbsp;")
                                {
                                    if (e.Row.Cells[cell].Text.ToUpper().Trim() == "AAA")
                                    {
                                        e.Row.Cells[cell].Font.Underline = true;
                                        e.Row.Cells[cell].ForeColor = System.Drawing.Color.Red;
                                        e.Row.Cells[cell].BorderColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        string val4 = e.Row.Cells[cell].Text;
                                        if (Convert.ToString(val4).All(char.IsNumber))
                                        {
                                            if (Convert.ToDouble(e.Row.Cells[cell].Text) < minmarks)
                                            {
                                                e.Row.Cells[cell].Font.Underline = true;
                                                e.Row.Cells[cell].ForeColor = System.Drawing.Color.Red;
                                                e.Row.Cells[cell].BorderColor = System.Drawing.Color.Black;
                                            }
                                        }
                                    }
                                }
                                //else if (e.Row.Cells[cell].Text.ToUpper().Trim() == "AAA")
                                //{
                                //    e.Row.Cells[cell].Font.Underline = true;
                                //    e.Row.Cells[cell].ForeColor = System.Drawing.Color.Red;
                                //    e.Row.Cells[cell].BorderColor = System.Drawing.Color.Black;
                                //}
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
    {
        if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

        DataTable t3 = t1.Clone();  // first add columns from table1
        foreach (DataColumn col in t2.Columns)
        {
            string newColumnName = col.ColumnName;
            int colNum = 1;
            while (t3.Columns.Contains(newColumnName))
            {
                newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
            }
            t3.Columns.Add(newColumnName, col.DataType);
        }
        var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
            (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
        foreach (object[] rowFields in mergedRows)
            t3.Rows.Add(rowFields);

        return t3;
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
        string result = string.Empty;
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
            result = string.Empty;
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

    private string Splitter(string p, string p_2)
    {
        throw new NotImplementedException();
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewchart.Visible = false;
        Chart1.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        GridViewselectedfield.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        //lblEduration.Visible = false;
        lblnorec.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
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
        string from_date = string.Empty;
        string to_date = string.Empty;
        string final_from = string.Empty;
        string final_to = string.Empty;
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        lblcharterr.Text = string.Empty;
        GridViewchart.Visible = false;
        Chart1.Visible = false;
        GridViewselectedfield.Visible = false;
        if (ddlTest.SelectedIndex != 0)
        {
            if (chart.Checked == true)
            {
                if (txtoptiminpassmark.Text.Trim() != null && txtoptiminpassmark.Text.Trim() != "")
                {
                }
                else
                {
                    lblcharterr.Text = "Please Enter Optional Min Pass Mark";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('Please Enter Optional Min Pass Mark');", true);
                    return;
                }
            }
        }
        else
        {
            lblcharterr.Text = "Please Select The Test";
            return;
        }
        chartdt1.Columns.Add("Pass Percentage", typeof(string));
        buttonG0();
    }

    protected void buttonG0()
    {
        try
        {
            //FpEntry.Visible = false;
            gview.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            //FpEntry.Sheets[0].RowCount = 0;
            TextBoxother.Visible = false;
            TextBoxother.Text = string.Empty;
            TextBoxpage.Text = string.Empty;
            //FpEntry.CurrentPage = 0;
            int indexcnt = 0;
            //------------------------------------------date validation-------------------------------
            string valfromdate = string.Empty;
            string valtodate = string.Empty;
            string frmconcat = string.Empty;
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
                gview.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;

            }
            else
            {
                lblnorec.Text = string.Empty;
                lblnorec.Visible = false;
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
                //FpEntry.Visible = false;
                gview.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                if (ddlTest.Text != "")
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = string.Empty;
                    if (ddlTest.SelectedItem.Value.ToString() == "Terminal Test")
                    {
                        // MessageBox.Show("No Test conducted ");
                    }
                    else
                    {
                        if (ddlSec.Enabled == true || ddlSec.Text != "-1" || ddlSec.Enabled == false)
                        {
                            gview.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;

                            string camoptionalmark = txtoptiminpassmark.Text.ToString();
                            if (camoptionalmark.Trim() != "" && camoptionalmark != null)
                            {
                                if (Convert.ToInt32(camoptionalmark) == 0)
                                {
                                    lblnorec.Text = "Please Enter The Optional Min Pass Mark Greater Than 0.";
                                    lblnorec.Visible = true;
                                    txtoptiminpassmark.Text = "100";
                                    return;
                                }
                                if (100 < Convert.ToInt32(camoptionalmark))
                                {
                                    lblnorec.Text = "Please Enter The Optional Min Pass Mark Lesser Than or Equal to 100.";
                                    lblnorec.Visible = true;
                                    txtoptiminpassmark.Text = "100";
                                    return;
                                }
                            }
                            SpreadBind();//---------------changed 12.12-------------------
                            //for (int right_logo_col = 0; right_logo_col < FpEntry.Sheets[0].ColumnCount; right_logo_col++)
                            //{
                            //    if (FpEntry.Sheets[0].Columns[right_logo_col].Visible == true)
                            //    {
                            //        //MyImg mi3 = new MyImg();
                            //        //mi3.ImageUrl = "Handler2.ashx?";
                            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].HorizontalAlign = HorizontalAlign.Center; //23.02.12
                            //        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].CellType = mi3;
                            //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, right_logo_col, 9, 1);
                            //        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_col].Border.BorderColorBottom = Color.Black;
                            //        //FpEntry.Sheets[0].Columns[right_logo_col].Width = 150;
                            //        break;
                            //    }
                            //}                            
                            gview.Width = 500;
                            //'-------------------------------------------------------------------------------------
                            Buttontotal.Visible = false;
                            lblrecord.Visible = false;
                            DropDownListpage.Visible = false;
                            TextBoxother.Visible = false;
                            lblpage.Visible = false;
                            TextBoxpage.Visible = false;

                            gview.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                        }

                        if (Convert.ToInt32(gview.Rows.Count) == 0)
                        {
                            lblnorec.Visible = true;
                            //FpEntry.Visible = false;
                            gview.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                        }
                        else
                        {
                            //Buttontotal.Visible = true;
                            //lblrecord.Visible = true;
                            //DropDownListpage.Visible = true;
                            //TextBoxother.Visible = false;
                            //lblpage.Visible = true;
                            //TextBoxpage.Visible = true;
                            //FpEntry.Visible = true;
                            gview.Visible = true;
                            btnExcel.Visible = true;
                            btnprintmaster.Visible = true;
                            txtexcelname.Visible = true;
                            lblrptname.Visible = true;
                            double totalRows = 0;
                            //totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
                            //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
                            totalRows = Convert.ToInt32(gview.Rows.Count);
                            Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                            DropDownListpage.Items.Clear();
                            if (totalRows >= 10)
                            {
                                //FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                                gview.PageSize = Convert.ToInt32(totalRows);
                                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                                {
                                    DropDownListpage.Items.Add((k + 10).ToString());
                                }
                                DropDownListpage.Items.Add("Others");
                                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                                //FpEntry.Height = 335;
                                gview.Height = 335;
                            }
                            else if (totalRows == 0)
                            {
                                DropDownListpage.Items.Add("0");
                                //FpEntry.Height = 100;
                                gview.Height = 100;
                            }
                            else
                            {

                                gview.PageSize = Convert.ToInt32(totalRows);
                                DropDownListpage.Items.Add(gview.PageSize.ToString());
                                gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                            }
                            if (Convert.ToInt32(gview.Rows.Count) > 10)
                            {
                                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;

                                gview.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                                CalculateTotalPages();
                            }
                            gview.Height = 200 + (20 * Convert.ToInt32(totalRows));
                        }
                        if (ddlTest.SelectedItem.Value.ToString() == "--Select--")
                        {
                            //FpEntry.Visible = false;
                            gview.Visible = false;
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
                    //FpEntry.Visible = false;
                    gview.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnprintmaster.Visible = false;
                    lblnorec.Text = "Kindly Select Test";
                    lblnorec.Visible = true;
                }
            }//-----------------------------date validate------------------------------
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Chart1.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        GridViewselectedfield.Visible = false;
        GridViewchart.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        //   buttonG0();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTest();
        ////lblEsection.Visible = false;
        lblnorec.Visible = false;
        btnExcel.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewchart.Visible = false;
        Chart1.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        GridViewselectedfield.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
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
        }
        else
        {
            lblnorec.Text = "Give degree rights to the staff";
            lblnorec.Visible = true;
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            //FpEntry.Visible = true;
            gview.Visible = false;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            btnprintmaster.Visible = true;
            //FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            gview.PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
            //  FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        }
        //FpEntry.SaveChanges();
        //FpEntry.CurrentPage = 0;
    }

    void CalculateTotalPages()
    {
        double totalRows = 0;
        //totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
        totalRows = Convert.ToInt32(gview.Rows.Count);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = false;
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    //FpEntry.Visible = true;
                    gview.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmaster.Visible = true;
                    TextBoxpage.Text = string.Empty;
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = string.Empty;
                }
                else
                {
                    LabelE.Visible = false;
                    //FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    //FpEntry.Visible = true;
                    gview.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmaster.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = string.Empty;
        }
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxother.Text != "")
            {
                //FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                gview.PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = string.Empty;
        }
    }

    protected void FpEntry_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
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
            //**********Added By S21.08.2014****************//
            case 18:
                atten = "RAA";
                break;
            //**************End***********************//
        }
        return atten;
    }

    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart1.Visible = false;
        GridViewselectedfield.Visible = false;
        GridViewchart.Visible = false;
        ddlTest.SelectedIndex = -1;
        TextBoxother.Visible = false;
        TextBoxother.Text = string.Empty;
        TextBoxpage.Text = string.Empty;
        //FpEntry.Visible = false;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
    }

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }

    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FpEntry.Visible = false;
        gview.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        //GetTest();
        int selectcout = 0;
        for (int i = 0; i < ddlreport.Items.Count; i++)
        {
            if (ddlreport.Items[i].Selected == true)
            {
                selectcout = selectcout + 1;
            }
        }
        TextBox1.Text = "Criteria(" + (selectcout) + ")";
        ddlTest.SelectedIndex = -1;
    }

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (SelectAll.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlreport.Items)
            {
                li.Selected = true;
                TextBox1.Text = "Criteria(" + (ddlreport.Items.Count) + ")";
                //FpEntry.Visible = false;
                gview.Visible = false;
                btnExcel.Visible = false;
                btnExcelchart.Visible = false;
                btnPrintchart.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlreport.Items)
            {
                li.Selected = false;
                TextBox1.Text = "--Select--";
                //FpEntry.Visible = false;
                gview.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                btnExcelchart.Visible = false;
                btnPrintchart.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        ddlTest.SelectedIndex = -1;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        //FpEntry.Visible = false;
        gview.Visible = false;
        btnExcelchart.Visible = false;
        btnPrintchart.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            DataTable Tissues = Session["data"] as DataTable;
            if (reportname.ToString().Trim() != "")
            {
                if (Tissues.Columns.Count > 0)
                {
                    ExportTable(Tissues, "CamEntry");
                }
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string selected_criteria = string.Empty;
        string select_frm_date = txtFromDate.Text;
        string select_to_date = txtToDate.Text;
        if (ddlreport.Items.Count > 0)
        {
            for (int criteria = 0; criteria < ddlreport.Items.Count; criteria++)
            {
                if (ddlreport.Items[criteria].Selected == true)
                {
                    if (selected_criteria == "")
                    {
                        selected_criteria = ddlreport.Items[criteria].Value;
                    }
                    else
                    {
                        selected_criteria = selected_criteria + "-" + ddlreport.Items[criteria].Value;
                    }
                }
            }
        }
        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex + "$" + selected_criteria.ToString() + "$" + select_frm_date + "$" + select_to_date;
        //   PrintMaster = true;
        SpreadBind();
        string clmnheadrname = string.Empty;
        //int total_clmn_count = FpEntry.Sheets[0].ColumnCount;
        int total_clmn_count = gview.HeaderRow.Cells.Count;
        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (clmnheadrname == "")
            {
                //clmnheadrname = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            }
            else
            {
                //clmnheadrname = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            }
        }
        string dis_hdng_batch = lblYear.Text + "- " + ddlBatch.SelectedItem.ToString() + ((lblYear.Text.Trim().ToLower() != "year") ? "COURSE" : "STANDARD") + "  " + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        //added by anandan 
        int totsec = ddlSec.Items.Count;
        string dis_hdng_sec = string.Empty;
        if (totsec > 0)
        {
            dis_hdng_sec = lblDuration.Text.Trim() + " " + "- " + ddlSemYr.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        }
        else
        {
            dis_hdng_sec = lblDuration.Text.Trim() + " " + "- " + ddlSemYr.SelectedItem.ToString();
        }
        string dis_date = "From Date " + "- " + txtFromDate.Text.ToString() + " " + "To Date " + "- " + txtToDate.Text.ToString();
        //Response.Redirect("Print_Master_Setting.aspx?ID=" + clmnheadrname.ToString() + ":" + "CAMrpt.aspx" + ":" + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + ddlSec.SelectedItem.ToString()+":"+"CAM REPORT");
        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "CAMrpt.aspx" + ":" + dis_hdng_batch + "@" + dis_hdng_sec + "@" + dis_date + ":" + "CAM REPORT");
    }

    public void func_header()
    {
    }

    public void getspecial_hr()
    {
        //  try
        {
            string hrdetno = string.Empty;
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
        string filt_details = string.Empty;
        string strsec = string.Empty;
        if (ddlSec.Enabled == true)
        {
            strsec = " Sec " + ddlSec.SelectedItem.Text.ToString();
        }
        filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + lblDuration.Text.Trim() + " " + ddlSemYr.SelectedItem.ToString() + "-" + strsec;
        string date_filt = "From :" + txtFromDate.Text + "-" + "To :" + txtToDate.Text;
        string test = "Test :" + ddlTest.SelectedItem.ToString();
        string degreedetails = string.Empty;
        degreedetails = ((lblYear.Text.Trim().ToLower() != "year") ? "CAM" : "TEST") + " REPORT" + "@" + filt_details + "@" + date_filt + "@" + test;
        string pagename = "CAMrpt.aspx";
        string ss = Session["usercode"].ToString();
        NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnExcelchart_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition",
                "attachment;filename=OverAllTop.xls");
            Response.ContentType = "applicatio/excel";
            StringWriter sw = new StringWriter(); ;
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            GridViewselectedfield.RenderControl(htm);
            GridViewchart.RenderControl(htm);
            Response.Write(sw.ToString());
            Response.End();
            Response.Clear();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPrintchart_Click(object sender, EventArgs e)
    {
        try
        {
            string degree_code = ddlBranch.SelectedValue.ToString();
            string batch_year = ddlBatch.SelectedValue.ToString();
            string current_sem = ddlSemYr.SelectedValue.ToString();
            string branch = ddlBranch.SelectedItem.ToString();
            if (ddlSec.SelectedItem.Text != "ALL")
            {
                sections = "&nbsp;-&nbsp;" + ddlSec.SelectedValue.ToString().ToUpper();
            }
            else
            {
                sections = string.Empty;
            }
            string degreedetails = string.Empty;
            degreedetails = ddlDegree.SelectedItem.ToString().ToUpper() + "&nbsp;-&nbsp;" + branch.ToUpper() + "&nbsp;" + sections + "&nbsp;(" + lblYear.Text.Trim().ToUpper() + "&nbsp;" + batch_year.ToString() + ")&nbsp;" + lblDuration.Text + "&nbsp;-&nbsp;" + current_sem.ToString();
            btnGo_Click(sender, e);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Passpercentageanalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();
            string collegename = string.Empty;
            DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + collegecode + "' ", "Text");
            if (dscol.Tables[0].Rows.Count > 0)
            {
                lb.Text = dscol.Tables[0].Rows[0]["collname"].ToString() + "<br> ";
                lb.Style.Add("height", "100px");
                lb.Style.Add("text-decoration", "none");
                lb.Style.Add("font-family", "Book Antiqua;");
                lb.Style.Add("font-size", "18px");
                lb.Style.Add("text-align", "center");
                lb.RenderControl(hw);
                string address = string.Empty;
                if (dscol.Tables[0].Rows[0]["address1"].ToString().Trim() != "")
                {
                    address = dscol.Tables[0].Rows[0]["address1"].ToString();
                }
                if (dscol.Tables[0].Rows[0]["address2"].ToString().Trim() != "")
                {
                    if (address == "")
                    {
                        address = dscol.Tables[0].Rows[0]["address2"].ToString();
                    }
                    else
                    {
                        address = address + ", " + dscol.Tables[0].Rows[0]["address2"].ToString();
                    }
                }
                if (dscol.Tables[0].Rows[0]["address3"].ToString().Trim() != "")
                {
                    if (address == "")
                    {
                        address = dscol.Tables[0].Rows[0]["address3"].ToString();
                    }
                    else
                    {
                        address = address + ", " + dscol.Tables[0].Rows[0]["address3"].ToString();
                    }
                }
                if (address.Trim() != "")
                {
                    lb.Text = address + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "12px");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(hw);
                }
                //address = string.Empty;
                //if (dscol.Tables[0].Rows[0]["category"].ToString().Trim() != "")
                //{
                //    address = dscol.Tables[0].Rows[0]["category"].ToString();
                //}
                //if (dscol.Tables[0].Rows[0]["university"].ToString().Trim() != "")
                //{
                //    if (address == "")
                //    {
                //        address = dscol.Tables[0].Rows[0]["university"].ToString();
                //    }
                //    else
                //    {
                //        address = address + " by " + dscol.Tables[0].Rows[0]["university"].ToString();
                //    }
                //}
                //if (address.Trim() != "")
                //{
                //    lb.Text = address + "<br> ";
                //    lb.Style.Add("height", "100px");
                //    lb.Style.Add("text-decoration", "none");
                //    lb.Style.Add("font-family", "Book Antiqua;");
                //    lb.Style.Add("font-size", "12px");
                //    lb.Style.Add("text-align", "center");
                //    lb.RenderControl(hw);
                //}
            }
            Label lb2 = new Label();
            lb2.Text = degreedetails;
            lb2.Style.Add("height", "100px");
            lb2.Style.Add("text-decoration", "none");
            lb2.Style.Add("font-family", "Book Antiqua;");
            lb2.Style.Add("font-size", "10px");
            lb2.Style.Add("text-align", "left");
            lb2.RenderControl(hw);
            Label lb3 = new Label();
            lb3.Text = "<br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw);
            Label lb4 = new Label();
            lb4.Text = ddlTest.SelectedItem.Text.ToString().ToUpper() + " PASS PERCENTAGE<br><br><br>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "left");
            lb4.RenderControl(hw);
            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
            if (GridViewselectedfield.Rows.Count > 0)
            {
                GridViewselectedfield.AllowPaging = false;
                GridViewselectedfield.HeaderRow.Style.Add("width", "15%");
                GridViewselectedfield.HeaderRow.Style.Add("font-size", "8px");
                GridViewselectedfield.HeaderRow.Style.Add("text-align", "center");
                GridViewselectedfield.Style.Add("font-family", "Book Antiqua;");
                GridViewselectedfield.Style.Add("font-size", "6px");
                GridViewselectedfield.RenderControl(hw);
                GridViewselectedfield.DataBind();
            }
            lb4 = new Label();
            if (Chart1.Visible == true)
            {
                lb4.Text = "<br>STAFF PERFORMANCE ANALYSIS CHART<br><br><br>";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "8px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "center");
                lb4.RenderControl(hw);
            }
            if (GridViewchart.Rows.Count > 0)
            {
                GridViewchart.AllowPaging = false;
                GridViewchart.HeaderRow.Style.Add("width", "15%");
                GridViewchart.HeaderRow.Style.Add("font-size", "8px");
                GridViewchart.HeaderRow.Style.Add("text-align", "center");
                GridViewchart.Style.Add("font-family", "Book Antiqua;");
                GridViewchart.Style.Add("font-size", "6px");
                GridViewchart.DataBind();
                GridViewchart.Enabled = true;
                GridViewchart.RenderControl(hw1);
                GridViewchart.DataBind();
            }
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            //{
            //    string getpath = HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg").ToString();
            //    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(getpath);
            //    jpg.ScaleToFit(60f, 40f);
            //    jpg.Alignment = Element.ALIGN_LEFT;
            //    jpg.IndentationLeft = 9f;
            //    jpg.SpacingAfter = 9f;
            //    pdfDoc.Add(jpg);
            //}
            StringReader sr = new StringReader(sw.ToString());
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);
            if (Chart1.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Chart1.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }
            lb3.Text = "<br><b><br><br><br><br><br><br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw1);
            sr = new StringReader(sw1.ToString());
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);
            PdfPTable pdftbl0 = new PdfPTable(4);
            pdftbl0.TotalWidth = 500f;
            float[] width = new float[] { 200f, 100f, 200f, 200f };
            pdftbl0.SetWidths(width);
            PdfPCell cell = new PdfPCell(new Phrase("CLASS INCHARGE"));
            cell.Border = 0;
            cell.HorizontalAlignment = 0;
            pdftbl0.AddCell(cell);
            cell = new PdfPCell(new Phrase("HOD"));
            cell.Border = 0;
            cell.HorizontalAlignment = 1;
            pdftbl0.AddCell(cell);
            cell = new PdfPCell(new Phrase("VICE PRINCIPAL"));
            cell.Border = 0;
            cell.HorizontalAlignment = 1;
            pdftbl0.AddCell(cell);
            cell = new PdfPCell(new Phrase("PRINCIPAL"));
            cell.Border = 0;
            cell.HorizontalAlignment = 2;
            pdftbl0.AddCell(cell);
            pdfDoc.Add(pdftbl0);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }

    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(lblDegree);
            fields.Add(2);
            lbl.Add(lblBranch);
            fields.Add(3);
            lbl.Add(lblDuration);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblYear.Text = "Year";
            }
            else
            {
                lblYear.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {

        }
    }

    private void ExportTable(DataTable dtt, string filename)
    {
        Response.ClearContent();
        Response.Buffer = true;
        string headername = Convert.ToString(filename + DateTime.Now.ToString("dd/MM/yyyy-hh:ss") + ".xls");
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", headername));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtt;
        string str = string.Empty;



        foreach (DataRow dr in dt.Rows)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string value = Convert.ToString(dr[j]);
                if (value.EndsWith("\r\n"))
                {
                    string[] values = value.Split('\r');
                    string val = values[0];
                    Response.Write(str + val);
                    str = "\t";
                }
                else
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
            }
            str = "\r\n";
        }
        System.Web.HttpContext.Current.Response.Flush();
        Response.End();
    }
}
