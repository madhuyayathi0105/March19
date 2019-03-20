using System; //modified 06.04.12 ,modified on 08.06.12 (logo size)
//========removed textvaltable from select_allcam report details proc.and changed to displaying seattype for student on 01.08.12
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using BalAccess;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

public partial class CAMrptFormat2 : System.Web.UI.Page
{
   

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
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

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

    string grouporusercode = "";
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
    string coll = "0";
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
    int gs_pass_count, bs_pass_count, gs_fail_count, bs_failcount, tot_gs_count, tot_bs_count, no_of_all_clear;
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

    DataSet dsprint = new DataSet();

    Hashtable has_load_rollno = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
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
        

        lblnorec.Visible = false;
        lblexcelerr.Visible = false;
        btnprintmaster.Visible = false;
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
                TextBox1.Text = "---Select---";
                FpEntry.Sheets[0].RowHeader.Visible = false;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                TextBox1.Attributes.Add("readonly", "readonly");
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                btnprintmaster.Visible = false;

                Radiowithoutheader.Visible = false;
                RadioHeader.Visible = false;

                FpEntry.Sheets[0].SheetName = " ";
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                FpEntry.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;

                //'------------------------------------------------------------
                //btnPrint.Visible = true;


                FpEntry.Visible = false;
                FpEntry.Sheets[0].PageSize = 10;

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpEntry.Sheets[0].AllowTableCorner = true;
                FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
                svsort = FpEntry.ActiveSheetView;
                svsort.AllowSort = true;
                FpEntry.CommandBar.Visible = true;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                FpEntry.Sheets[0].SheetCorner.RowCount = 10;
                FpEntry.Sheets[0].SheetCorner.Cells[9, 0].Text = "S.No";
                FpEntry.Sheets[0].SheetCorner.Cells[9, 0].BackColor = Color.AliceBlue;

                //FpEntry.Sheets[0].SheetCorner.Rows[0].Visible = false;// on 23.02.12
                //FpEntry.Sheets[0].RowHeader.Visible=false;
                //FpEntry.Sheets[0].SheetCorner.Columns[0].Width = 120;

                FpEntry.Sheets[0].Columns[1].Width = 180;
                FpEntry.Sheets[0].Columns[0].Width = 180;
                FpEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 5, 1);

                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpEntry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpEntry.Pager.Align = HorizontalAlign.Right;
                FpEntry.Pager.Font.Bold = true;
                FpEntry.Pager.Font.Name = "Book Antiqua";
                FpEntry.Pager.ForeColor = Color.DarkGreen;
                FpEntry.Pager.BackColor = Color.Beige;
                FpEntry.Pager.BackColor = Color.AliceBlue;

                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                //FpEntry.Sheets[0].FrozenColumnCount = 5;
                //FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                FpEntry.Sheets[0].Columns[0].Width = 70;
                FpEntry.Sheets[0].Columns[1].Width = 70;
                FpEntry.Sheets[0].Columns[2].Width = 200;
                // FpEntry.Sheets[0].PageSize = 10;
                FpEntry.Pager.PageCount = 5;
                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpEntry.Sheets[0].AutoPostBack = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;

                RadioButtonList3.SelectedValue = "4";

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
                                ddlreport.Items[Convert.ToInt32(spl_criteria_val[crt])].Selected = true;
                            }
                        }
                        txtFromDate.Text = spl_load_val[2].ToString();
                        txtToDate.Text = spl_load_val[3].ToString();
                        // binddate();
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
                    // binddate();

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

        Control cntUpdateBtn = FpEntry.FindControl("Update");
        Control cntCancelBtn = FpEntry.FindControl("Cancel");
        Control cntCopyBtn = FpEntry.FindControl("Copy");
        Control cntCutBtn = FpEntry.FindControl("Clear");
        Control cntPasteBtn = FpEntry.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = FpEntry.FindControl("Print");

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
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
            ds4 = dacces2.select_method_wo_parameter(Sqlstr, "text");
            if (ds4.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds4;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                //  ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            }

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
        FpEntry.Visible = false;
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
        binddate();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
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
        binddate();
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
        try
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
                            if (!holiday_table21.ContainsKey((dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0])))
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
        }
        catch
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

    public void SpreadBind()
    {
        try
        {
            Hashtable studabs = new Hashtable();
            Hashtable hatsubper = new Hashtable();
            Hashtable hatquota = new Hashtable();
            int startrow = 0, startcolumn = 0;
            DataSet dsquota = new DataSet();
            int nothiddencount = 0;
            int hasrow_count = 0;
            Radiowithoutheader.Visible = false;
            RadioHeader.Visible = false;

            FpEntry.Visible = true;

            filteration();
            int setco = 0;
            int nofosub = 0;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            string resminmrk = "";
            string subject_code = "";
            int[] maxtot = new int[100];
            string examdate = "";
            string subname = "";
            int rankcount = 0;
            int subjectfailedcount = 0;
            int serialno = 0;

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
            //================Added by srinath 18/7/2014=====Start================
            string cutofquery = "select sd.app_no,pm.max_marks as maxma,pm.acual_marks subma,r.roll_no,textval as sub from Stud_prev_details sd,perv_marks_history pm,Registration r,textvaltable t where sd.course_entno=pm.course_entno and r.App_No=sd.app_no and pm.psubjectno=t.TextCode and r.batch_year='" + batch + "' and r.degree_code='" + degreecode + "' and r.current_semester='" + semester + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and (textval  like '%phy%' or textval  like '%che%' or textval  like '%ma%' )";
            DataSet dscutoff = d2.select_method_wo_parameter(cutofquery, "Text");
            DataView dvcutof = new DataView();
            //=========End=======================

            FpEntry.Sheets[0].RowCount = 0;
            FpEntry.Sheets[0].ColumnCount = 0;
            FpEntry.Sheets[0].ColumnCount = 6;
            FpEntry.Sheets[0].ColumnHeader.RowCount = 6;//
            string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + strsec.ToString() + "' " + strorder + ",s.subject_no";
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
                //Cmd Saranyadevi 10.8.2018
                //string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "' and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                //string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0 and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                //Added By Saranyadevi 10.8.2018
                string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "'   and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                hat.Clear();
                hat.Add("bath_year", batch.ToString());
                hat.Add("degree_code", degreecode.ToString());
                hat.Add("sec", strsec.ToString());
                hat.Add("filterwithsectionsub", filterwithsectionsub.ToString());
                hat.Add("filterwithoutsectionsub", filterwithoutsectionsub.ToString());
                ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");

                if (ds5.Tables[0].Rows.Count != 0)
                {
                    if (ds5.Tables[0].Rows.Count > 0)
                    {
                        int c = 0;
                        for (int irow = 0; irow < ds5.Tables[0].Rows.Count; irow++)
                        {
                            serialno++;
                            c++;
                            FpEntry.Sheets[0].RowCount++;
                            FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                            FpEntry.Sheets[0].Cells[irow, 2].CellType = tt;
                            FpEntry.Sheets[0].Cells[irow, 1].CellType = tt;
                            FpEntry.Sheets[0].Cells[irow, 0].Text = c.ToString();
                            FpEntry.Sheets[0].Cells[irow, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[irow, 1].Text = ds5.Tables[0].Rows[irow]["RollNumber"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 1].HorizontalAlign = HorizontalAlign.Center;

                            FpEntry.Sheets[0].Cells[irow, 2].Text = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 2].HorizontalAlign = HorizontalAlign.Center;

                            FpEntry.Sheets[0].Cells[irow, 3].Text = ds5.Tables[0].Rows[irow]["Student_Name"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 4].Text = ds5.Tables[0].Rows[irow]["StudentType"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 5].Text = ds5.Tables[0].Rows[irow]["ApplicationNumber"].ToString();
                        }
                    }

                    Session["rowcount"] = FpEntry.Sheets[0].RowCount;
                    if (Session["Rollflag"].ToString() == "0")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                        //  nothiddencount = 2;
                    }
                    if (Session["Regflag"].ToString() == "0")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                        //  nothiddencount = 1;
                    }
                    if (Session["Studflag"].ToString() == "0")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                    }

                    FpEntry.Sheets[0].ColumnHeader.Rows[0].Visible = true;
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "Sl.No";
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].BackColor = Color.AliceBlue;

                    FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Roll No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Reg No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Student Name";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Student Type";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    //Added By Srinath 7/6/2013
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Application Number";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Columns[5].Visible = false;
                    //FpEntry.Sheets[0].ColumnHeader.Columns[1].Width = 120;
                    FpEntry.Sheets[0].ColumnHeader.Columns[0].Width = 120;
                    FpEntry.Sheets[0].Columns[4].Width = 150;
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
                    string acronym = ds2.Tables[1].Rows[i]["acronym"].ToString();
                    x1 = FpEntry.Sheets[0].ColumnCount;
                    FpEntry.Sheets[0].ColumnCount = Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) + 1;
                    int incr = FpEntry.Sheets[0].ColumnCount - 1;

                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Tag = examdate + "@" + exam_code;

                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, incr].Text = subject_code + "-" + acronym;//Modify By M.SakthiPriya 09-12-2014
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Note = subno + "@" + subname + "@" + subject_code;
                    //FpEntry.Sheets[0].SetColumnVisible(4, false);
                    htdate.Add(subject_code, examdate);
                    count++;
                    if (ddlreport.Items[42].Selected == true)
                    {
                        FpEntry.Sheets[0].ColumnCount++;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = "ATT";
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Note = subno;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            startcolumn = FpEntry.Sheets[0].ColumnCount;
            FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 2;
            int totalcount = FpEntry.Sheets[0].ColumnCount - 2;
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, totalcount].Text = "Total";
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, totalcount].HorizontalAlign = HorizontalAlign.Center;

            int percentcount = FpEntry.Sheets[0].ColumnCount - 1;
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, percentcount].Text = "Percentage";
            FpEntry.Sheets[0].Columns[percentcount].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, percentcount].HorizontalAlign = HorizontalAlign.Center;

            //=======================Subject Wise Attendance Precntage====================
            if (ddlreport.Items[42].Selected == true)
            {
                load_attendance();
            }
            // sqlStr = "";

            //'--------------------------result column----------------


            if (ddlreport.Items[24].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Nooffailcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Nooffailcount].Text = "NFPS";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }

            FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
            resultcount = FpEntry.Sheets[0].ColumnCount - 1;
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, resultcount].Text = "Result";
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, resultcount].HorizontalAlign = HorizontalAlign.Center;

            if (ddlreport.Items[36].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;

                subjectfailedcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, subjectfailedcount].Text = "Subjects Failed";
                FpEntry.Sheets[0].Columns[subjectfailedcount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }

            if (ddlreport.Items[0].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                rankcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, rankcount].Text = "Rank";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;

            }
            if (ddlreport.Items[1].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                mediumcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, mediumcount].Text = "Medium";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;

            }
            if (ddlreport.Items[2].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 2;
                percount = FpEntry.Sheets[0].ColumnCount - 2;
                grdcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, percount].Text = "12th/Dip Grp";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, grdcount].Text = "12th/Dip %";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                spancount += 2;
                FpEntry.Width = 900;
            }
            if (ddlreport.Items[3].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                cgpacount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, cgpacount].Text = "CGPA";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
                FpEntry.Width = 900;
            }
            //'-----------------------------------------------------------------------------------------------------
            //'--------------------------------------------cam new modify--------------------

            //string subjctname = "";
            //string subjcode = "";
            //string[] split_sub;
            //double totmaxmark = 0;
            //'-----------------------------------------------------------------------


            //'------------------------------------------------------------------------
            if (ddlreport.Items[16].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Dpasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Dpasscount].Text = "DPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[17].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Hpasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Hpasscount].Text = "HPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[18].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Tpasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Tpasscount].Text = "TPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[19].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Epasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Epasscount].Text = "EPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[20].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                gendercount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, gendercount].Text = "G/B";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[21].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Gpasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Gpasscount].Text = "GPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            if (ddlreport.Items[22].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Bpasscount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Bpasscount].Text = "BPass";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            //'-----------------------------------------------------------------------
            int cutoff = 0;
            if (ddlreport.Items[41].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                cutoff = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, cutoff].Text = "Cut Off Marks";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, cutoff].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
            }
            strseattype = "select distinct seattype,textval from applyn ,textvaltable where  Batch_Year=" + batch + " and  degree_code=" + degreecode + " and seattype<>0 and seattype=textcode"; //new on 23.06.12
            dsquota = d2.select_method_wo_parameter(strseattype, "Text");
            if (dsquota.Tables[0].Rows.Count > 0)
            {
                for (int q = 0; q < dsquota.Tables[0].Rows.Count; q++)
                {
                    seattypecount += 1;
                    gettextcode = dsquota.Tables[0].Rows[q]["seattype"].ToString();
                    string toquota = gettextcode + "t";
                    if (!hatquota.Contains(toquota))
                    {
                        hatquota.Add(toquota, 0);
                        hatquota.Add(gettextcode + "p", 0);
                    }
                }
            }


            FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
            quotacount = FpEntry.Sheets[0].ColumnCount - 1;
            spancount++;
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, quotacount].Text = "Quota";
            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
            if (ddlreport.Items[23].Selected == true)
            {
                FpEntry.Sheets[0].Columns[quotacount].Visible = true;
            }
            else
            {
                FpEntry.Sheets[0].Columns[quotacount].Visible = false;
            }
            for (int q = 0; q < dsquota.Tables[0].Rows.Count; q++)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                spancount++;
                retrvseatname = dsquota.Tables[0].Rows[q]["textval"].ToString();
                gettextcode = dsquota.Tables[0].Rows[q]["seattype"].ToString();
                if (ddlreport.Items[23].Selected == true)
                {
                    FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Visible = true;
                }
                else
                {
                    FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Visible = false;
                }
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = retrvseatname;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                //'-------------------------------------------- set the textcode as note 
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Note = gettextcode;

            }
            // }
            //'--------------------------------------- -----------------------------------

            //concolhours
            if (ddlreport.Items[35].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                concolhours = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, concolhours].Text = "Conducted Hours";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, concolhours].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
                //  FpEntry.Width = 900;
            }

            if (ddlreport.Items[25].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Noofhrattend = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Noofhrattend].Text = "Noofhrs.Attended";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Noofhrattend].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
                //  FpEntry.Width = 900;
            }
            if (ddlreport.Items[26].Selected == true)
            {

                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                Attendpercnt = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Attendpercnt].Text = "Attendance %";
                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Attendpercnt].HorizontalAlign = HorizontalAlign.Center;
                spancount++;
                // FpEntry.Width = 900;
            }

            //'=================================================================================================
            //'------------------------------------load the clg information

            string collnamenew1 = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string address = "";
            string Phoneno = "";
            string Faxno = "";
            string phnfax = "";
            int subjectcount = 0;
            string district = "";
            string email = "";
            string website = "";

            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(pincode,'') as pincode,isnull(address3,'') as address3,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
                DataSet dscollege = d2.select_method_wo_parameter(college, "Text");
                if (dscollege.Tables[0].Rows.Count > 0)
                {
                    collnamenew1 = dscollege.Tables[0].Rows[0]["collname"].ToString();
                    address1 = dscollege.Tables[0].Rows[0]["address1"].ToString();
                    address2 = dscollege.Tables[0].Rows[0]["address2"].ToString();
                    address3 = dscollege.Tables[0].Rows[0]["address3"].ToString();
                    district = dscollege.Tables[0].Rows[0]["district"].ToString();
                    address = address3 + "-" + dscollege.Tables[0].Rows[0]["pincode"].ToString();
                    Phoneno = dscollege.Tables[0].Rows[0]["phoneno"].ToString();
                    Faxno = dscollege.Tables[0].Rows[0]["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                    email = "E-Mail:" + dscollege.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site:" + dscollege.Tables[0].Rows[0]["website"].ToString();
                }
            }
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = collnamenew1;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Large;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Text = address;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Large;
            int semval = Convert.ToInt32(ddlSemYr.SelectedItem.ToString());
            string semtext = "";
            string yearval = "";
            if (semval == 1 || semval == 2)
            {
                yearval = "I";
                if (semval == 1)
                {
                    semtext = "First";
                }
                if (semval == 2)
                {
                    semtext = "Second";
                }
            }
            else if (semval == 3 || semval == 4)
            {
                yearval = "II";
                if (semval == 3)
                {
                    semtext = "Third";
                }
                if (semval == 4)
                {
                    semtext = "Fourth";
                }
            }
            else if (semval == 5 || semval == 6)
            {
                yearval = "III";
                if (semval == 5)
                {
                    semtext = "Fifth";
                }
                if (semval == 6)
                {
                    semtext = "Sixth";
                }
            }
            else if (semval == 7 || semval == 8)
            {
                yearval = "IV";
                if (semval == 7)
                {
                    semtext = "Seventh";
                }
                if (semval == 8)
                {
                    semtext = "Eighth";
                }
            }
            else if (semval == 9 || semval == 10)
            {
                yearval = "V";
                if (semval == 9)
                {
                    semtext = "Ninth";
                }
                if (semval == 10)
                {
                    semtext = "Tenth";
                }
            }
            string getsem = NumToText(semval);
            string bracn = d2.GetFunction("select de.dept_acronym from Degree d ,Department de where d.Dept_Code=de.Dept_Code and d.Degree_Code='" + ddlBranch.SelectedValue.ToString() + "'");
            int academic_year = System.DateTime.Now.Year;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Student Performance Analysis " + academic_year + "-" + (academic_year + 1) + " [" + semtext + " Semester]";
            FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Text = ddlTest.SelectedItem.Text.ToString();
            FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Font.Size = FontUnit.Large;
            FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Text = yearval + " Year " + bracn;
            FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Font.Size = FontUnit.Large;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Large;

            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].HorizontalAlign = HorizontalAlign.Center;

            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, FpEntry.Sheets[0].ColumnCount);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, FpEntry.Sheets[0].ColumnCount);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, FpEntry.Sheets[0].ColumnCount);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, FpEntry.Sheets[0].ColumnCount);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 0, 1, FpEntry.Sheets[0].ColumnCount);

            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorBottom = Color.White;
            FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorBottom = Color.White;

            if (sections == null)
            {
                sections = "";
            }


            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpEntry.Sheets[0].AllowTableCorner = true;

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
                                    marks_per = dv_indstudmarks[cnt]["mark"].ToString();

                                    switch (marks_per)
                                    {
                                        case "-1":

                                            marks_per = "AAA";
                                            //Aruna 01nov2012================================
                                            if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                            {
                                                if (Convert.ToString(studabs[ds5.Tables[0].Rows[i]["RollNumber"]]) == "1")
                                                    studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "1";
                                            }
                                            else
                                            {
                                                studabs.Add(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), "1");
                                            }
                                            //===============================================
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
                                    }
                                    if (marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;
                                    }
                                    if (marks >= 0 && (Convert.ToString(marks) != string.Empty))
                                    {
                                        per_mark += marks;
                                        sub_max_marks += double.Parse(dv_indstudmarks[cnt]["max_mark"].ToString());
                                        //Aruna 01nov2012================================
                                        if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                        {
                                            studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "0";
                                        }
                                        else
                                        {
                                            studabs.Add(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), "0");
                                        }
                                        //=============================================== 
                                    }

                                    int subcol = j + 6;
                                    if (ddlreport.Items[42].Selected == true)
                                    {
                                        if (j > 0)
                                        {
                                            int js = j * 2;
                                            subcol = js + 6;
                                        }
                                    }
                                    if (marks >= min_mark || marks_per == "EL" || marks_per == "EOD")
                                    {
                                        pass++;
                                        if ((RadioButtonList3.SelectedItem.ToString() == "Pass") || RadioButtonList3.SelectedItem.ToString() == "All")
                                        {

                                            FpEntry.Sheets[0].Cells[i, subcol].Text = marks_per.ToString();
                                            FpEntry.Sheets[0].Cells[i, subcol].Font.Name = "Book Antiqua";
                                            FpEntry.Sheets[0].Cells[i, subcol].HorizontalAlign = HorizontalAlign.Center;
                                            //   FpEntry.Sheets[0].Cells[i, j + 5].Font.Size = FontUnit.Medium;
                                        }

                                    }
                                    else
                                    {
                                        fail++;
                                        if ((RadioButtonList3.SelectedItem.ToString() == "Fail") || RadioButtonList3.SelectedItem.ToString() == "All")
                                        {

                                            if (marks >= 0)
                                            {

                                                FpEntry.Sheets[0].Cells[i, subcol].Text = marks_per.ToString();
                                                FpEntry.Sheets[0].Cells[i, subcol].ForeColor = Color.Red;
                                                FpEntry.Sheets[0].Cells[i, subcol].Font.Underline = true;
                                                FpEntry.Sheets[0].Cells[i, subcol].Font.Name = "Book Antiqua";
                                                FpEntry.Sheets[0].Cells[i, subcol].HorizontalAlign = HorizontalAlign.Center;
                                                // FpEntry.Sheets[0].Cells[i, j + 5].Font.Size = FontUnit.Medium;
                                            }
                                            else
                                            {
                                                FpEntry.Sheets[0].Cells[i, subcol].Text = marks_per.ToString();
                                                FpEntry.Sheets[0].Cells[i, subcol].ForeColor = Color.Red;
                                                FpEntry.Sheets[0].Cells[i, subcol].Font.Underline = true;
                                                FpEntry.Sheets[0].Cells[i, subcol].Font.Name = "Book Antiqua";
                                                FpEntry.Sheets[0].Cells[i, subcol].HorizontalAlign = HorizontalAlign.Center;
                                                //  FpEntry.Sheets[0].Cells[i, j + 5].Font.Size = FontUnit.Medium;
                                                marks = 0;
                                            }

                                        }


                                    }
                                    if ((RadioButtonList3.SelectedItem.ToString() == "Absent") || RadioButtonList3.SelectedItem.ToString() == "All")
                                    {
                                        if (marks < 0 && marks_per != "EL" && marks_per != "EOD")
                                        {
                                            FpEntry.Sheets[0].Cells[i, subcol].Text = marks_per.ToString();
                                            FpEntry.Sheets[0].Cells[i, subcol].ForeColor = Color.Red;
                                            FpEntry.Sheets[0].Cells[i, subcol].Font.Underline = true;
                                            FpEntry.Sheets[0].Cells[i, subcol].Font.Name = "Book Antiqua";
                                            FpEntry.Sheets[0].Cells[i, subcol].HorizontalAlign = HorizontalAlign.Center;
                                            // FpEntry.Sheets[0].Cells[i, j + 5].Font.Size = FontUnit.Medium;
                                        }
                                    }
                                    tot_marks += marks;
                                    EL = 0;
                                    stu_count++;


                                }
                            }
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
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "1")
                        {
                            gs_pass_count++;
                            tot_gs_count++;
                            gs_count = 1;
                            if (ddlreport.Items[21].Selected == true)
                            {


                                FpEntry.Sheets[0].Cells[i, Gpasscount].Text = gs_count.ToString();
                                FpEntry.Sheets[0].Cells[i, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "0")
                        {
                            bs_pass_count++;
                            tot_bs_count++;
                            bs_count = 1;
                            if (ddlreport.Items[22].Selected == true)
                            {

                                FpEntry.Sheets[0].Cells[i, Bpasscount].Text = bs_count.ToString();
                                FpEntry.Sheets[0].Cells[i, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "1")
                        {
                            gs_fail_count++;
                            tot_gs_count++;
                            gs_count = 0;
                            if (ddlreport.Items[21].Selected == true)
                            {

                            }
                        }
                        if (ds5.Tables[0].Rows[i]["Gen"].ToString() == "0")
                        {
                            bs_failcount++;
                            tot_bs_count++;
                            bs_count = 0;
                            if (ddlreport.Items[22].Selected == true)
                            {

                            }
                        }
                    }

                    if (ddlreport.Items[36].Selected == true)
                    {
                        string re = "";
                        if (fail == 0)
                        {
                            re = "-";
                        }
                        else
                        {
                            re = fail.ToString();
                        }
                        FpEntry.Sheets[0].Cells[i, subjectfailedcount].Text = re.ToString();
                    }


                    FpEntry.Sheets[0].Cells[i, totalcount].Text = tot_marks.ToString();
                    FpEntry.Sheets[0].Cells[i, totalcount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[i, percentcount].Text = per_tage.ToString();
                    FpEntry.Sheets[0].Cells[i, percentcount].HorizontalAlign = HorizontalAlign.Center;
                    if (pass_fail != null)
                    {
                        FpEntry.Sheets[0].Cells[i, resultcount].Text = pass_fail.ToString();
                        FpEntry.Sheets[0].Cells[i, resultcount].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (ddlreport.Items[24].Selected == true)
                    {
                        FpEntry.Sheets[0].Cells[i, Nooffailcount].Text = fail.ToString();
                        FpEntry.Sheets[0].Cells[i, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                    }
                    string medium1 = "";
                    FpEntry.Sheets[0].Cells[i, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                    medium1 = GetFunction("select distinct medium from stud_prev_details where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + " and medium is not NULL");
                    if (ddlreport.Items[1].Selected == true)
                    {
                        if ((medium1 == "") || (medium1 == null))
                        {
                            FpEntry.Sheets[0].Cells[i, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[i, mediumcount].Text = "-";
                        }
                        else
                        {
                            FpEntry.Sheets[0].Cells[i, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[i, mediumcount].Text = medium1.ToString();
                        }
                    }

                    string textval = "";
                    if (ds5.Tables[0].Rows[i]["SeatType"].ToString() != "" && ds5.Tables[0].Rows[i]["SeatType"].ToString() != " ")
                    {
                        textval = GetFunction("Select TextVal from textvaltable where textcode=" + ds5.Tables[0].Rows[i]["seattype"].ToString() + "");
                    }
                    else
                    {
                        textval = "-";
                    }
                    if (ddlreport.Items[23].Selected == true)//modified on 01.08.12
                    {
                        FpEntry.Sheets[0].Cells[i, quotacount].Text = textval.ToString();
                        FpEntry.Sheets[0].Cells[i, quotacount].HorizontalAlign = HorizontalAlign.Center;
                        quota_count = quotacount;

                    }
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

                                        FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Right;

                                        if (scholmrk != string.Empty)
                                        {
                                            FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].Cells[i, grdcount].Text = scholmrk.ToString();

                                        }
                                        else
                                        {
                                            FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].Cells[i, grdcount].Text = "-";
                                        }

                                        string scholmrk1 = GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                        FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        if (scholmrk1 != string.Empty)
                                        {
                                            string sam = scholmrk1.ToString();
                                            FpEntry.Sheets[0].Cells[i, percount].Text = scholmrk1.ToString();
                                            FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {

                                            FpEntry.Sheets[0].Cells[i, percount].Text = "-";
                                            FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        }



                                    }
                                    else if (schoolgrd == "PG")
                                    {


                                        string scholmrk2 = GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                        FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                        if (scholmrk2 != string.Empty)
                                        {

                                            FpEntry.Sheets[0].Cells[i, grdcount].Text = scholmrk2.ToString();
                                            FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpEntry.Sheets[0].Cells[i, grdcount].Text = "-";
                                            FpEntry.Sheets[0].Cells[i, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        string scholmrk3 = GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                        FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        if (scholmrk3 != string.Empty)
                                        {

                                            FpEntry.Sheets[0].Cells[i, percount].Text = scholmrk3.ToString();

                                            con.Close();
                                        }
                                        else
                                        {

                                            FpEntry.Sheets[0].Cells[i, percount].Text = "-";
                                            FpEntry.Sheets[0].Cells[i, percount].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (ddlreport.Items[41].Selected == true)
                    {
                        dscutoff.Tables[0].DefaultView.RowFilter = " app_no='" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "' and sub like 'Ma%'";
                        dvcutof = dscutoff.Tables[0].DefaultView;
                        string cutvalue = "-";
                        Double mathcut = 0;
                        if (dvcutof.Count > 0)
                        {
                            Double maxmark = Convert.ToDouble(dvcutof[0]["maxma"].ToString());
                            Double submark = Convert.ToDouble(dvcutof[0]["subma"].ToString());
                            if (maxmark > 0 && submark > 0)
                            {
                                Double totalmar = submark / maxmark * 100;
                                totalmar = Math.Round(totalmar, 2, MidpointRounding.AwayFromZero);
                                mathcut = totalmar;
                            }
                        }
                        dscutoff.Tables[0].DefaultView.RowFilter = " app_no='" + ds5.Tables[0].Rows[i]["ApplicationNumber"].ToString() + "' and (sub  like '%phy%' or sub  like '%che%')";
                        dvcutof = dscutoff.Tables[0].DefaultView;
                        Double othercut = 0;
                        if (dvcutof.Count > 0)
                        {
                            double getma = 0;
                            Double getmax = 0;
                            for (int cus = 0; cus < dvcutof.Count; cus++)
                            {
                                Double maxmark = Convert.ToDouble(dvcutof[cus]["maxma"].ToString());
                                Double submark = Convert.ToDouble(dvcutof[cus]["subma"].ToString());
                                if (maxmark > 0 && submark > 0)
                                {
                                    getma = getma + submark;
                                    getmax = getmax + maxmark;
                                }
                            }
                            othercut = getma / getmax * 100;
                            othercut = Math.Round(othercut, 2, MidpointRounding.AwayFromZero);
                        }
                        mathcut = mathcut + othercut;
                        if (mathcut > 0)
                        {
                            cutvalue = mathcut.ToString();
                        }
                        FpEntry.Sheets[0].Cells[i, cutoff].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[i, cutoff].Text = cutvalue;
                    }


                    if (ddlreport.Items[3].Selected == true)
                    {
                        int sem = Convert.ToInt32(ddlSemYr.SelectedValue.ToString());
                        double degcgpa = Math.Round(findgrade(ds5.Tables[0].Rows[i]["RollNumber"].ToString(), sem), 2);
                        degcgpa = Math.Round(degcgpa, 2);
                        FpEntry.Sheets[0].Cells[i, cgpacount].Text = degcgpa.ToString();
                        FpEntry.Sheets[0].Cells[i, cgpacount].HorizontalAlign = HorizontalAlign.Center;
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
                        FpEntry.Sheets[0].Cells[i, gendercount].Text = gender.ToString();
                        FpEntry.Sheets[0].Cells[i, gendercount].HorizontalAlign = HorizontalAlign.Center;
                    }


                    if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Hostler" || ds5.Tables[0].Rows[i]["StudentType"].ToString().Trim().ToLower() == "hostler")
                    {

                        if (ddlreport.Items[17].Selected == true)
                        {
                            bs_count = 1;
                            FpEntry.Sheets[0].Cells[i, Hpasscount].Text = bs_count.ToString();
                            FpEntry.Sheets[0].Cells[i, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (pass_fail.Trim().ToLower() == "pass")
                        {
                            h_pass_count++;
                        }
                        else
                        {
                            h_fail_count++;
                        }
                    }

                    if (ds5.Tables[0].Rows[i]["StudentType"].ToString() == "Day Scholar" || ds5.Tables[0].Rows[i]["StudentType"].ToString().ToLower().Trim() == "day scholar")
                    {
                        if (ddlreport.Items[16].Selected == true)
                        {
                            bs_count = 1;
                            FpEntry.Sheets[0].Cells[i, Dpasscount].Text = bs_count.ToString();
                            FpEntry.Sheets[0].Cells[i, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (pass_fail.Trim().ToLower() == "pass")
                        {
                            d_pass_count++;
                        }
                        else
                        {
                            d_fail_count++;
                        }
                    }
                    if ((medium1.Trim().ToLower() == "tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                    {

                        if (ddlreport.Items[18].Selected == true)
                        {
                            bs_count = 1;
                            FpEntry.Sheets[0].Cells[i, Tpasscount].Text = bs_count.ToString();
                            FpEntry.Sheets[0].Cells[i, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (pass_fail.Trim().ToLower() == "pass")
                        {
                            t_pass_count++;
                        }
                        else
                        {
                            t_fail_count++;
                        }

                    }
                    if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1.Trim().ToLower() == "english"))
                    {

                        if (ddlreport.Items[19].Selected == true)
                        {
                            bs_count = 1;
                            FpEntry.Sheets[0].Cells[i, Epasscount].Text = bs_count.ToString();
                            FpEntry.Sheets[0].Cells[i, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (pass_fail.Trim().ToLower() == "pass")
                        {
                            e_pass_count++;
                        }
                        else
                        {
                            e_fail_count++;
                        }
                    }
                    //else
                    //    //if (ds5.Tables[0].Rows[i]["medium"].ToString() == "English")
                    //    if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                    //    {
                    //        e_fail_count++;
                    //    }
                    //}

                    if (ddlreport.Items[23].Selected == true)
                    {
                        quota_count = quota_count + 1;
                        //  getquota =  ds5.Tables[0].Rows[i]["textval"].ToString();
                        FpEntry.Sheets[0].Cells[res, quota_count].Note = strseattype.ToString();
                        FpEntry.Sheets[0].SetText(res, quota_count, getquota);
                        FpEntry.Sheets[0].Cells[res, quota_count].HorizontalAlign = HorizontalAlign.Center;
                    }
                    for (int colloop = quota_count; colloop <= FpEntry.Sheets[0].ColumnCount - 1; colloop++)
                    {
                        gettextcode = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, colloop].Note;

                        if (ds5.Tables[0].Rows[i]["seattype"].ToString() == gettextcode.ToString())
                        {
                            if (hatquota.Contains(gettextcode + "t"))
                            {
                                int qpasco = Convert.ToInt32(hatquota[gettextcode + "t"]);
                                qpasco = qpasco + 1;
                                hatquota[gettextcode + "t"] = qpasco;
                            }
                            if (fail != 0)
                            {
                                if (ddlreport.Items[23].Selected == true)
                                {
                                    FpEntry.Sheets[0].SetText(res, colloop, "0");
                                    FpEntry.Sheets[0].Cells[res, colloop].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            else
                            {
                                if (hatquota.Contains(gettextcode + "p"))
                                {
                                    int qpasco = Convert.ToInt32(hatquota[gettextcode + "p"]);
                                    qpasco = qpasco + 1;
                                    hatquota[gettextcode + "p"] = qpasco;
                                }
                                if (ddlreport.Items[23].Selected == true)
                                {
                                    FpEntry.Sheets[0].SetText(res, colloop, "1");
                                    FpEntry.Sheets[0].Cells[res, colloop].HorizontalAlign = HorizontalAlign.Center;
                                }
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
                    pass_fail = "";
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
                            string hrdetno = "";
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





                        dum_tage_date = Math.Round(per_tage_date, 2);
                        dum_tage_hrs = Math.Round(per_tage_hrs, 2);

                        if (ddlreport.Items[25].Selected == true || ddlreport.Items[26].Selected == true)
                        {
                            if (Session["Hourwise"] == "1")
                            {
                                if (ddlreport.Items[25].Selected == true)
                                {
                                    FpEntry.Sheets[0].Cells[i, Noofhrattend].Text = per_per_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[i, Noofhrattend].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (ddlreport.Items[26].Selected == true)
                                {
                                    FpEntry.Sheets[0].Cells[i, Attendpercnt].Text = dum_tage_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[i, Attendpercnt].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (ddlreport.Items[35].Selected == true)
                                {

                                    FpEntry.Sheets[0].Cells[i, concolhours].Text = per_con_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[i, concolhours].HorizontalAlign = HorizontalAlign.Center;
                                }


                            }
                            else
                            {
                                if (ddlreport.Items[25].Selected == true)
                                {
                                    FpEntry.Sheets[0].Cells[i, Noofhrattend].Text = pre_present_date.ToString();
                                    FpEntry.Sheets[0].Cells[i, Noofhrattend].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (ddlreport.Items[26].Selected == true)
                                {
                                    FpEntry.Sheets[0].Cells[i, Attendpercnt].Text = dum_tage_date.ToString();
                                    FpEntry.Sheets[0].Cells[i, Attendpercnt].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (ddlreport.Items[35].Selected == true)
                                {

                                    FpEntry.Sheets[0].Cells[i, concolhours].Text = per_con_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[i, concolhours].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }

                        }
                    }
                    if (ddlreport.Items[41].Selected == true)
                    {

                    }

                }




                //'---------------------------------------------------------------
                if (ddlreport.Items[4].Selected == true)
                {
                    x1 = per_sub_count - 1;
                    nothiddencount = 0;//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    classstrength = FpEntry.Sheets[0].RowCount - 1;
                    startrow = classstrength;
                    FpEntry.Sheets[0].SetText(classstrength, nothiddencount, "Class Strength");
                    FpEntry.Sheets[0].Cells[classstrength, nothiddencount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(classstrength, 0, 1, 5);
                    FpEntry.Sheets[0].Cells[classstrength, 6].Text = ds5.Tables[0].Rows.Count.ToString();//03.04.12
                    FpEntry.Sheets[0].Cells[classstrength, 6].HorizontalAlign = HorizontalAlign.Center;
                    if (startcolumn > 7)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(classstrength, 7, 1, startcolumn - 7);
                        FpEntry.Sheets[0].Cells[classstrength, 7].Border.BorderColorRight = Color.White;
                    }
                    //Hidden by srinath 23/5/2014
                    //FpEntry.Sheets[0].SetText(classstrength, totalcount, "Total");
                    //FpEntry.Sheets[0].Cells[classstrength, totalcount].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].SetText(classstrength, percentcount, "Pass");
                    //FpEntry.Sheets[0].Cells[classstrength, percentcount].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].SetText(classstrength, resultcount, "Fail");
                    ////Modified by srinath 23/5/2014
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].Cells[classstrength, resultcount].HorizontalAlign = HorizontalAlign.Center;


                }
                nofosub = 0;
                if (ddlreport.Items[15].Selected == true)
                {
                    int staff_index = 0;
                    FpEntry.Sheets[0].RowCount += 1;
                    signat = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = classstrength;
                    }
                    FpEntry.Sheets[0].SetText(signat, nothiddencount, "Staff Signature");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    for (int staff_col = 6; staff_col < 6 + ds2.Tables[1].Rows.Count; staff_col++)
                    {
                        string temp = "";
                        string staff = "";

                        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                        {
                            strsec = "";
                        }
                        else
                        {
                            strsec = " and exam_type.sections='" + sections.ToString() + "'";
                        }

                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[staff_index]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");

                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        setco = staff_col;
                        if (ddlreport.Items[42].Selected == true)
                        {
                            if (nofosub > 0)
                            {
                                setco = 6 + (nofosub * 2);
                            }
                        }
                        nofosub++;
                        FpEntry.Sheets[0].SetText(signat, setco, staff.ToString());
                        if (ddlreport.Items[42].Selected == true)
                        {
                            FpEntry.Sheets[0].SpanModel.Add(signat, setco, 1, 2);
                        }
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
                        ////double top_no = double.Parse(ds3.Tables[0].Rows[0]["Total"].ToString());
                        ////for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                        ////{

                        ////    if (top_no > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                        ////    {
                        ////        ra_nk += 1;
                        ////    }
                        ////    else
                        ////    {
                        ////        ra_nk = ra_nk;
                        ////    }
                        ////    top_no = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                        ////    hat.Clear();
                        ////    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                        ////    hat.Add("criteria_no", criteria_no.ToString());
                        ////    hat.Add("Total", tot_marks.ToString());
                        ////    hat.Add("avg", per_tage.ToString());
                        ////    hat.Add("rank", ra_nk.ToString());
                        ////    int o = d2.insert_method("INSERT_RANK", hat, "sp");
                        ////}
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
                            zx++;
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
                                FpEntry.Sheets[0].Cells[i, rankcount].Text = dvrank[0]["Rank"].ToString();
                                FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpEntry.Sheets[0].Cells[i, rankcount].Text = "-";
                                FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
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

                int rows_count = 6;

                if (ddlreport.Items[34].Selected == true)
                {

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    exdate = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = exdate;
                    }
                    FpEntry.Sheets[0].SetText(exdate, nothiddencount, "Exam Date:");
                    FpEntry.Sheets[0].SpanModel.Add(exdate, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[7].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    passcount = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = passcount;
                    }
                    FpEntry.Sheets[0].SetText(passcount, nothiddencount, "No.of.Students Passed");
                    FpEntry.Sheets[0].SpanModel.Add(passcount, 0, 1, 5);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[8].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    failcount = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = failcount;
                    }
                    FpEntry.Sheets[0].SetText(failcount, nothiddencount, "No.of.Students RA");
                    FpEntry.Sheets[0].SpanModel.Add(failcount, 0, 1, 5);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[13].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    maxcount = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = maxcount;
                    }
                    FpEntry.Sheets[0].SetText(maxcount, nothiddencount, "MAX MARK:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    mincount = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(mincount, nothiddencount, "MIN MARK:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[33].Selected == true)
                {

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    maxrollnum = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = maxrollnum;
                    }
                    FpEntry.Sheets[0].SetText(maxrollnum, nothiddencount, " Max Roll Number");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    minrollnum = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(minrollnum, nothiddencount, "Min Roll Number");

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                }
                if (ddlreport.Items[9].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avg_50count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avg_50count;
                    }
                    FpEntry.Sheets[0].SetText(avg_50count, nothiddencount, "AVG < 50 MARK:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[10].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avg_65count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avg_65count;
                    }
                    FpEntry.Sheets[0].SetText(avg_65count, nothiddencount, "AVG 50 To 65:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }


                if (ddlreport.Items[11].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avgg65count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avgg65count;
                    }
                    FpEntry.Sheets[0].SetText(avgg65count, nothiddencount, "AVG > 65:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[37].Selected == true)
                {

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avg_60count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avg_60count;
                    }
                    FpEntry.Sheets[0].SetText(avg_60count, nothiddencount, "AVG > 60:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[38].Selected == true)
                {

                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avg_80count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avg_80count;
                    }
                    FpEntry.Sheets[0].SetText(avg_80count, nothiddencount, "AVG > 80:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[5].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    pre_count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = pre_count;
                    }
                    FpEntry.Sheets[0].SetText(pre_count, nothiddencount, "No.of.Students Present");
                    FpEntry.Sheets[0].SpanModel.Add(pre_count, 0, 1, 5);
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[6].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    ab_count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = ab_count;
                    }
                    FpEntry.Sheets[0].SetText(ab_count, nothiddencount, "No.of.Students Absent");
                    FpEntry.Sheets[0].SpanModel.Add(ab_count, 0, 1, 5);
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[14].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    pperc_count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = pperc_count;
                    }
                    FpEntry.Sheets[0].SetText(pperc_count, nothiddencount, "PASS PERCENTAGE:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[12].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    avg_count = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = avg_count;
                    }
                    FpEntry.Sheets[0].SetText(avg_count, nothiddencount, "CLASS AVERAGE:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[27].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    perc75 = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(perc75, nothiddencount, "AVERAGE >= 75");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[28].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;

                    perc60to74 = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = perc60to74;
                    }
                    FpEntry.Sheets[0].SetText(perc60to74, nothiddencount, "AVERAGE >= 60 and <=74");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[29].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;

                    perc50to59 = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = perc50to59;
                    }
                    FpEntry.Sheets[0].SetText(perc50to59, nothiddencount, "AVERAGE >= 50 and <=59");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[30].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;

                    perc30to49 = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = perc30to49;
                    }
                    FpEntry.Sheets[0].SetText(perc30to49, nothiddencount, "AVERAGE >= 30 and <=49");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[31].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    perc20to29 = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = perc20to29;
                    }
                    FpEntry.Sheets[0].SetText(perc20to29, nothiddencount, "AVERAGE >= 20 and <=29");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (ddlreport.Items[32].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    perc19 = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = perc19;
                    }
                    FpEntry.Sheets[0].SetText(perc19, nothiddencount, "AVERAGE <=19");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }


                if (ds2.Tables[1].Rows.Count != 0)
                {
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        string txst = FpEntry.Sheets[0].Cells[0, 6].Text;
                        string secss = "";
                        if (ddlSec.Enabled == false)
                        {
                            secss = "";
                        }
                        else
                        {
                            secss = ddlSec.SelectedItem.Text.ToString();
                        }
                        if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                        {
                            secss = "";  // added by sridhar aug 2014
                        }
                        else
                        {
                            secss = ddlSec.SelectedItem.Text.ToString(); // added by sridhar aug 2014
                        }
                        string date = "";
                        string sgcode = "";
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("section", secss);
                        date = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        sgcode = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        ds4 = d2.select_method("Proc_All_Subject_Details", hat, "sp");

                        if (ds4.Tables.Count != 0)
                        {

                            // FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            // sub_strength = FpEntry.Sheets[0].RowCount - 1;


                            //20,21

                            if (ddlreport.Items[37].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[avg_60count, rows_count].Text = ds4.Tables[20].Rows[0]["AVG>=60"].ToString();
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avg_60count, rows_count, 1, 2);
                                }
                                FpEntry.Sheets[0].Cells[avg_60count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ddlreport.Items[38].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[avg_80count, rows_count].Text = ds4.Tables[21].Rows[0]["AVG>=80"].ToString();
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avg_80count, rows_count, 1, 2);
                                }
                                FpEntry.Sheets[0].Cells[avg_80count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ddlreport.Items[34].Selected == true)
                            {
                                string[] sp = date.Split(' ');
                                if (sp.GetUpperBound(0) > 0)
                                {
                                    date = sp[0].ToString();
                                }
                                FpEntry.Sheets[0].Cells[exdate, rows_count].CellType = txt;
                                FpEntry.Sheets[0].Cells[exdate, rows_count].Text = date.ToString();
                                FpEntry.Sheets[0].SpanModel.Add(exdate, rows_count, 1, 2);
                                FpEntry.Sheets[0].Cells[exdate, rows_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ddlreport.Items[7].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[passcount, rows_count].Text = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(passcount, rows_count, 1, 2);
                                }
                                FpEntry.Sheets[0].Cells[passcount, rows_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ddlreport.Items[8].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[failcount, rows_count].Text = ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();//table[2] for failcount with absent
                                FpEntry.Sheets[0].Cells[failcount, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(failcount, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[13].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[maxcount, rows_count].Text = ds4.Tables[3].Rows[0]["MAX_MARK"].ToString();
                                FpEntry.Sheets[0].Cells[maxcount, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(maxcount, rows_count, 1, 2);
                                }
                                FpEntry.Sheets[0].Cells[mincount, rows_count].Text = ds4.Tables[4].Rows[0]["MIN_MARK"].ToString();
                                FpEntry.Sheets[0].Cells[mincount, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(mincount, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[33].Selected == true)
                            {
                                string name = GetFunction("select stud_name from registration where roll_no='" + ds4.Tables[18].Rows[0]["roll_no"].ToString() + "'");
                                FpEntry.Sheets[0].Cells[maxrollnum, rows_count].Text = ds4.Tables[18].Rows[0]["roll_no"].ToString() + '-' + name;
                                FpEntry.Sheets[0].Cells[maxrollnum, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(maxrollnum, rows_count, 1, 2);
                                }
                                string namemin = GetFunction("select stud_name from registration where roll_no='" + ds4.Tables[19].Rows[0]["roll_no"].ToString() + "'");
                                FpEntry.Sheets[0].Cells[minrollnum, rows_count].Text = ds4.Tables[19].Rows[0]["roll_no"].ToString() + '-' + namemin;
                                FpEntry.Sheets[0].Cells[minrollnum, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(minrollnum, rows_count, 1, 2);
                                }
                            }


                            if (ddlreport.Items[9].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[avg_50count, rows_count].Text = ds4.Tables[5].Rows[0]["AVG<50"].ToString();
                                FpEntry.Sheets[0].Cells[avg_50count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avg_50count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[10].Selected == true)
                            {

                                FpEntry.Sheets[0].Cells[avg_65count, rows_count].Text = ds4.Tables[6].Rows[0]["AVG_50to65"].ToString();
                                FpEntry.Sheets[0].Cells[avg_65count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avg_65count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[11].Selected == true)
                            {

                                FpEntry.Sheets[0].Cells[avgg65count, rows_count].Text = ds4.Tables[7].Rows[0]["AVG>65"].ToString();
                                FpEntry.Sheets[0].Cells[avgg65count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avgg65count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[5].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[pre_count, rows_count].Text = ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[pre_count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(pre_count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[6].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[ab_count, rows_count].Text = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[ab_count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(ab_count, rows_count, 1, 2);
                                }
                            }
                            double final_pperc = 0;
                            final_pperc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"])) * 100;
                            final_pperc = Math.Round(final_pperc, 2);
                            // FpEntry.Sheets[0].Cells[pperc_count, rows_count].Text = final_pperc.ToString();
                            if (!hatsubper.Contains(i))
                            {
                                hatsubper.Add(i, final_pperc);
                            }

                            if (ddlreport.Items[14].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[pperc_count, rows_count].Text = final_pperc.ToString();
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[pperc_count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(pperc_count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[12].Selected == true)
                            {
                                double final_avg_value = (Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"]));
                                final_avg_value = Math.Round(final_avg_value, 2);
                                FpEntry.Sheets[0].Cells[avg_count, rows_count].Text = final_avg_value.ToString();
                                FpEntry.Sheets[0].Cells[avg_count, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(avg_count, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[27].Selected == true)
                            {

                                FpEntry.Sheets[0].Cells[perc75, rows_count].Text = ds4.Tables[12].Rows[0]["AVG>=75"].ToString();
                                FpEntry.Sheets[0].Cells[perc75, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc75, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[28].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[perc60to74, rows_count].Text = ds4.Tables[13].Rows[0]["AVG60to74"].ToString();
                                FpEntry.Sheets[0].Cells[perc60to74, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc60to74, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[29].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[perc50to59, rows_count].Text = ds4.Tables[14].Rows[0]["AVG50to59"].ToString();
                                FpEntry.Sheets[0].Cells[perc50to59, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc50to59, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[30].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[perc30to49, rows_count].Text = ds4.Tables[15].Rows[0]["AVG30to49"].ToString();
                                FpEntry.Sheets[0].Cells[perc30to49, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc30to49, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[31].Selected == true)
                            {

                                FpEntry.Sheets[0].Cells[perc20to29, rows_count].Text = ds4.Tables[16].Rows[0]["AVG20to29"].ToString();
                                FpEntry.Sheets[0].Cells[perc20to29, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc20to29, rows_count, 1, 2);
                                }
                            }
                            if (ddlreport.Items[32].Selected == true)
                            {
                                FpEntry.Sheets[0].Cells[perc19, rows_count].Text = ds4.Tables[17].Rows[0]["AVG<=19"].ToString();
                                FpEntry.Sheets[0].Cells[perc19, rows_count].HorizontalAlign = HorizontalAlign.Center;
                                if (ddlreport.Items[42].Selected == true)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(perc19, rows_count, 1, 2);
                                }
                            }

                            if (ddlreport.Items[42].Selected == true)
                            {
                                rows_count = rows_count + 2;
                            }
                            else
                            {
                                rows_count++;
                            }
                        }
                    }
                }

                if (ddlreport.Items[39].Selected == true)//Modified by srinath 23/5/2014
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    int allclear_cnt = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = allclear_cnt;
                    }
                    FpEntry.Sheets[0].SetText(allclear_cnt, nothiddencount, "No of all Cleared:");
                    FpEntry.Sheets[0].Cells[allclear_cnt, 6].Text = no_of_all_clear.ToString();
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);
                    FpEntry.Sheets[0].Cells[allclear_cnt, nothiddencount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[allclear_cnt, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[allclear_cnt, 6].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[allclear_cnt, 6].Font.Size = FontUnit.Medium;
                    if (ddlreport.Items[40].Selected == false)
                    {
                        if (startcolumn > 9)
                        {
                            FpEntry.Sheets[0].SpanModel.Add(allclear_cnt, 7, 1, 3);
                            FpEntry.Sheets[0].Cells[allclear_cnt, 7].Border.BorderColorRight = Color.White;
                        }
                        else
                        {
                            if (startcolumn > 7)
                            {
                                FpEntry.Sheets[0].SpanModel.Add(allclear_cnt, 7, 1, startcolumn - 7);
                                FpEntry.Sheets[0].Cells[allclear_cnt, 7].Border.BorderColorRight = Color.White;
                            }
                        }
                        if (startcolumn > 10)
                        {
                            FpEntry.Sheets[0].SpanModel.Add(allclear_cnt, 10, 1, startcolumn - 10);
                            FpEntry.Sheets[0].Cells[allclear_cnt, 10].Border.BorderColorRight = Color.White;
                            FpEntry.Sheets[0].Cells[allclear_cnt, 10].Border.BorderColorBottom = Color.White;
                        }
                    }
                    else
                    {
                        if (startcolumn > 7)
                        {
                            FpEntry.Sheets[0].SpanModel.Add(allclear_cnt, 7, 1, startcolumn - 7);
                            FpEntry.Sheets[0].Cells[allclear_cnt, 7].Border.BorderColorRight = Color.White;
                            FpEntry.Sheets[0].Cells[allclear_cnt, 7].Border.BorderColorBottom = Color.White;
                        }
                    }
                }

                if (ddlreport.Items[40].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    int allclear_perc_cnt = FpEntry.Sheets[0].RowCount - 1;
                    if (startrow == 0)//Added by srinath 3/7/2014
                    {
                        startrow = allclear_perc_cnt;
                    }
                    FpEntry.Sheets[0].SetText(allclear_perc_cnt, nothiddencount, "% of all Cleared:");
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, nothiddencount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, nothiddencount].Border.BorderColorLeft = Color.Black;
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
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, 6].Text = per_noof_allclear.ToString();
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, 6].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[allclear_perc_cnt, 6].Font.Size = FontUnit.Medium;
                    if (startcolumn > 9)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(allclear_perc_cnt, 7, 1, 3);
                        FpEntry.Sheets[0].Cells[allclear_perc_cnt, 7].Border.BorderColorRight = Color.White;
                    }
                    else
                    {
                        if (startcolumn > 7)
                        {
                            FpEntry.Sheets[0].SpanModel.Add(allclear_perc_cnt, 7, 1, startcolumn - 7);
                            FpEntry.Sheets[0].Cells[allclear_perc_cnt, 7].Border.BorderColorRight = Color.White;
                        }
                    }
                    if (startcolumn > 10)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(allclear_perc_cnt, 10, 1, startcolumn - 10);
                        FpEntry.Sheets[0].Cells[allclear_perc_cnt, 10].Border.BorderColorRight = Color.White;
                        FpEntry.Sheets[0].Cells[allclear_perc_cnt, 10].Border.BorderColorBottom = Color.White;
                    }
                }
                //'----------------new load subj name , code-----------
                if (ds5.Tables[0].Rows.Count == FpEntry.Sheets[0].RowCount)
                {
                    startcolumn = 10;
                }
                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;

                int row_span_start = FpEntry.Sheets[0].RowCount - 1;
                int spanrow = row_span_start - ds5.Tables[0].Rows.Count;
                if (spanrow == 0)
                {
                    spanrow = 1;
                }

                FpEntry.Sheets[0].SpanModel.Add(row_span_start, 0, FpEntry.Sheets[0].RowCount, 5);
                if (FpEntry.Sheets[0].ColumnCount > 10)//Added by srinath 3/7/2014
                {
                    if (startcolumn > 9)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(ds5.Tables[0].Rows.Count, startcolumn, spanrow, FpEntry.Sheets[0].ColumnCount - 5);
                        FpEntry.Sheets[0].Cells[ds5.Tables[0].Rows.Count, startcolumn].Border.BorderColorBottom = Color.White;
                    }
                    else
                    {
                        FpEntry.Sheets[0].SpanModel.Add(ds5.Tables[0].Rows.Count, startcolumn, spanrow, 10 - startcolumn);
                        FpEntry.Sheets[0].Cells[ds5.Tables[0].Rows.Count, startcolumn].Border.BorderColorRight = Color.White;
                        FpEntry.Sheets[0].SpanModel.Add(ds5.Tables[0].Rows.Count, 10, spanrow, FpEntry.Sheets[0].ColumnCount - 5);
                        FpEntry.Sheets[0].Cells[ds5.Tables[0].Rows.Count, 10].Border.BorderColorBottom = Color.White;
                    }
                    if (ddlreport.Items[41].Selected == true)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(row_span_start, 10, FpEntry.Sheets[0].RowCount, FpEntry.Sheets[0].ColumnCount - 9);
                    }
                    FpEntry.Sheets[0].SpanModel.Add(row_span_start, 10, FpEntry.Sheets[0].RowCount, FpEntry.Sheets[0].ColumnCount - 10);
                }

                int cn = 0;
                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "S.No";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Subject Code";//Modify By M.SakthiPriya 09-12-2014
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = "Subject Name";//Modify By M.SakthiPriya 09-12-2014
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Text = "Staff Name";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Text = "Percentage";//Added by srinath 3/7/2014

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].Font.Bold = true;

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                int incrrowcnt = 1;
                int subrow = 0;
                if (ds2.Tables[1].Rows.Count > 0)
                {
                    int totrowcnt = FpEntry.Sheets[0].RowCount + ds2.Tables[1].Rows.Count;
                    for (subrow = FpEntry.Sheets[0].RowCount; subrow < totrowcnt; subrow++) //changed 21.02.12
                    {
                        if (incrrowcnt <= ds2.Tables[1].Rows.Count)
                        {
                            cn++;
                            FpEntry.Sheets[0].RowCount += 1;
                            FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                            FpEntry.Sheets[0].Columns[6].CellType = txt1;
                            //FpEntry.Sheets[0].Cells[subrow, 6].Text = Convert.ToInt32(cn).ToString();
                            //FpEntry.Sheets[0].Cells[subrow, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[subrow, 6].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_code"].ToString();//Modify By M.SakthiPriya 09-12-2014
                            FpEntry.Sheets[0].Cells[subrow, 7].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_name"].ToString();//Modify By M.SakthiPriya 09-12-2014
                            FpEntry.Sheets[0].Cells[subrow, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpEntry.Sheets[0].Cells[subrow, 7].HorizontalAlign = HorizontalAlign.Left;
                            FpEntry.Sheets[0].Cells[subrow, 8].HorizontalAlign = HorizontalAlign.Left;
                            if (hatsubper.Contains(incrrowcnt - 1))//Added by srinath 3/7/2014
                            {
                                string val = hatsubper[incrrowcnt - 1].ToString();
                                FpEntry.Sheets[0].Cells[subrow, 9].Text = val;
                                FpEntry.Sheets[0].Cells[subrow, 9].HorizontalAlign = HorizontalAlign.Center;
                            }

                            string temp = "";
                            string staff = "";

                            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                            {
                                strsec = "";
                            }
                            else
                            {
                                strsec = " and exam_type.sections='" + sections.ToString() + "'";
                            }

                            temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[incrrowcnt - 1]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");

                            if (temp != "")
                            {
                                staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                            }
                            FpEntry.Sheets[0].SetText(subrow, 8, staff);

                            incrrowcnt++;
                        }

                    }

                }


                //if (ddlreport.Items[16].Selected == true || ddlreport.Items[17].Selected == true || ddlreport.Items[18].Selected == true || ddlreport.Items[19].Selected == true || ddlreport.Items[21].Selected == true || ddlreport.Items[22].Selected == true || ddlreport.Items[23].Selected == true)
                //{
                if (ddlreport.Items[43].Selected == true)
                {
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Details";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = "Total";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Text = "Pass";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Text = "Fail";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //if (ddlreport.Items[16].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    d_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(d_count, 6, " Days Scholar Total");

                    FpEntry.Sheets[0].Cells[d_count, 7].Text = (d_fail_count + d_pass_count).ToString();
                    FpEntry.Sheets[0].Cells[d_count, 8].Text = d_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[d_count, 9].Text = d_fail_count.ToString();

                    FpEntry.Sheets[0].Cells[d_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[d_count, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[d_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(d_count, 1, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //if (ddlreport.Items[17].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    h_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(h_count, 6, "Hostler Total");
                    FpEntry.Sheets[0].Cells[h_count, 7].Text = (h_fail_count + h_pass_count).ToString();
                    FpEntry.Sheets[0].Cells[h_count, 8].Text = h_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[h_count, 9].Text = h_fail_count.ToString();

                    FpEntry.Sheets[0].Cells[h_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[h_count, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[h_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(h_count, 1, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}

                    //if (ddlreport.Items[18].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    t_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(t_count, 6, "Tamil");
                    FpEntry.Sheets[0].Cells[t_count, 7].Text = (t_pass_count + t_fail_count).ToString();
                    FpEntry.Sheets[0].Cells[t_count, 8].Text = t_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[t_count, 9].Text = t_fail_count.ToString();

                    FpEntry.Sheets[0].Cells[t_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[t_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[t_count, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(t_count, 1, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}

                    //if (ddlreport.Items[19].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    e_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(e_count, 6, "English");
                    FpEntry.Sheets[0].Cells[e_count, 7].Text = (e_pass_count + e_fail_count).ToString();
                    FpEntry.Sheets[0].Cells[e_count, 8].Text = e_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[e_count, 9].Text = e_fail_count.ToString();

                    FpEntry.Sheets[0].Cells[e_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[e_count, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[e_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(e_count, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}

                    //if (ddlreport.Items[21].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    g_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(g_count, 6, "Girls");
                    FpEntry.Sheets[0].Cells[g_count, 7].Text = tot_gs_count.ToString();
                    FpEntry.Sheets[0].Cells[g_count, 8].Text = gs_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[g_count, 9].Text = gs_fail_count.ToString();

                    FpEntry.Sheets[0].Cells[g_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[g_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[g_count, 8].HorizontalAlign = HorizontalAlign.Center;

                    FpEntry.Sheets[0].SpanModel.Add(g_count, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //if (ddlreport.Items[22].Selected == true)
                    //{
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                    b_count = FpEntry.Sheets[0].RowCount - 1;
                    FpEntry.Sheets[0].SetText(b_count, 6, "Boys");
                    FpEntry.Sheets[0].Cells[b_count, 7].Text = tot_bs_count.ToString();
                    FpEntry.Sheets[0].Cells[b_count, 8].Text = bs_pass_count.ToString();
                    FpEntry.Sheets[0].Cells[b_count, 9].Text = bs_failcount.ToString();

                    FpEntry.Sheets[0].Cells[b_count, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[b_count, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[b_count, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(b_count, 0, 1, 5);//Modified by srinath 23/5/2014
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //if (ddlreport.Items[23].Selected == true)
                    //{
                    if (dsquota.Tables[0].Rows.Count > 0)
                    {
                        for (int q = 0; q < dsquota.Tables[0].Rows.Count; q++)
                        {
                            gettextcode = dsquota.Tables[0].Rows[q]["seattype"].ToString();
                            retrvseatname = dsquota.Tables[0].Rows[q]["textval"].ToString();
                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 6, retrvseatname);
                            int qtotco = 0, qtotpac = 0, qtotfcou = 0;
                            if (hatquota.Contains(gettextcode + "t"))
                            {
                                qtotco = Convert.ToInt32(hatquota[gettextcode + "t"]);
                            }
                            if (hatquota.Contains(gettextcode + "p"))
                            {
                                qtotpac = Convert.ToInt32(hatquota[gettextcode + "p"]);
                            }
                            qtotfcou = qtotco - qtotpac;
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Text = qtotfcou.ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = qtotco.ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Text = qtotpac.ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                for (int c = 0; c < FpEntry.Sheets[0].ColumnCount; c++)
                {
                    FpEntry.Sheets[0].Columns[c].VerticalAlign = VerticalAlign.Middle;
                }

                FpEntry.SaveChanges();


            }
            else
            {
                lblnorec.Text = "Test has not been conducted for any subject";
                lblnorec.Visible = true;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;

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
        FpEntry.Visible = false;
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
        //  binddate();
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
        else
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        buttonG0();
    }
    protected void buttonG0()
    {
        txtexcelname.Text = "";
        Printcontrol.Visible = false;
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        FpEntry.CurrentPage = 0;
        int indexcnt = 0;


        if (ddlTest.Items.Count <= 0) //added by sridhar 05 sep 2014
        {
            lblerroe.Text = "Please Select Any One Test";
            lblerroe.Visible = true;
            return;
        }

        //added by sridhar 03 sep 2014 --------------* start  *-------------------------
        DateTime dtnow = DateTime.Now;
        lblerroe.Visible = false;
        string datefad, dtfromad;
        string datefromad;
        string yr4, m4, d4;
        datefad = txtFromDate.Text.ToString();

        string[] split4 = datefad.Split(new Char[] { '/' });
        if (split4.Length == 3)
        {
            datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
            yr4 = split4[2].ToString();
            m4 = split4[1].ToString();
            d4 = split4[0].ToString();
            dtfromad = m4 + "/" + d4 + "/" + yr4;
            //DateTime dt1 = Convert.ToDateTime(dtfromad);
            //if (dt1 > dtnow)
            //{
            //    lblerroe.Visible = false;
            //    lblerroe.Text = "Please Enter Valid From date";
            //    lblerroe.Visible = true;
            //    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyy");
            //    return;

            //}
        }
        datefad = txtToDate.Text.ToString();

        split4 = datefad.Split(new Char[] { '/' });

        if (split4.Length == 3)
        {
            datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
            yr4 = split4[2].ToString();
            m4 = split4[1].ToString();
            d4 = split4[0].ToString();
            dtfromad = m4 + "/" + d4 + "/" + yr4;
            //DateTime dt1 = Convert.ToDateTime(dtfromad);
            //if (dt1 > dtnow)
            //{
            //    lblerroe.Visible = false;
            //    lblerroe.Text = "Please Enter Valid To date";
            //    lblerroe.Visible = true;
            //    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyy");
            //    return;

            //}
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            string date2ad = "";
            date2ad = txtToDate.Text.ToString();
            lblerroe.Visible = false;

            datefad = txtFromDate.Text.ToString();
            split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;


                string adatetoad;
                string ayr5, am5, ad5;

                string[] asplit5 = date2ad.Split(new Char[] { '/' });
                if (asplit5.Length == 3)
                {
                    adatetoad = asplit5[0].ToString() + "/" + asplit5[1].ToString() + "/" + asplit5[2].ToString();
                    ayr5 = asplit5[2].ToString();
                    am5 = asplit5[1].ToString();
                    ad5 = asplit5[0].ToString();
                    adatetoad = am5 + "/" + ad5 + "/" + ayr5;
                    DateTime dt1 = Convert.ToDateTime(dtfromad);
                    DateTime dt2 = Convert.ToDateTime(adatetoad);

                    TimeSpan ts2 = dt2 - dt1;

                    int days2 = ts2.Days;
                    if (days2 < 0)
                    {
                        lblerroe.Text = "From Date Can't Be Greater Than To Date";
                        lblerroe.Visible = true;
                        return;
                    }
                }

            }
        }
        if (ddlTest.Items.Count >= 0)
        {
            //******added by jayaram 5th sep 14////
            if (ddlTest.Text != "")
            {

                if (ddlTest.SelectedItem.Text == "--Select--" || ddlTest.SelectedItem.Text == "-1" || ddlTest.SelectedItem.Text == null || ddlTest.SelectedItem.Text == "")
                {
                    lblerroe.Text = "Please Select Any one Test";
                    lblerroe.Visible = true;
                    //lblnorec.Text = "";
                    lblnorec.Visible = false;
                    return;
                }
            }
            //*end*//
        }

        //added by sridhar 03 sep 2014 --------------* End  *-------------------------








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
            FpEntry.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            FpEntry.Sheets[0].RowCount = 0;

        }
        else
        {
            lblnorec.Text = "";
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
            FpEntry.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            if (ddlTest.Items.Count == 0)
            {
                FpEntry.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblnorec.Text = "No Test Conducted";
                lblnorec.Visible = true;
                return;
            }
            if (ddlTest.Text != "")
            {
                if (ddlTest.SelectedIndex != 0 && ddlTest.SelectedItem.Text != "")
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = "";
                    if (ddlTest.SelectedItem.Value.ToString() == "Terminal Test")
                    {
                        // MessageBox.Show("No Test conducted ");

                    }
                    else
                    {
                        if (ddlSec.Enabled == true || ddlSec.Text != "-1" || ddlSec.Enabled == false)
                        {
                            FpEntry.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            FpEntry.Sheets[0].ColumnHeader.RowCount = 3;

                            SpreadBind();//---------------changed 12.12-------------------

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

                            FpEntry.Width = 500;
                            //'-------------------------------------------------------------------------------------
                            Buttontotal.Visible = false;
                            lblrecord.Visible = false;
                            DropDownListpage.Visible = false;
                            TextBoxother.Visible = false;
                            lblpage.Visible = false;
                            TextBoxpage.Visible = false;
                            FpEntry.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                        }

                        if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) == 0)
                        {
                            lblnorec.Visible = true;
                            FpEntry.Visible = false;
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
                            FpEntry.Visible = true;
                            btnExcel.Visible = true;
                            btnprintmaster.Visible = true;
                            txtexcelname.Visible = true;
                            lblrptname.Visible = true;

                            Double totalRows = 0;
                            totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
                            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
                            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                            DropDownListpage.Items.Clear();
                            if (totalRows >= 10)
                            {
                                FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                                {
                                    DropDownListpage.Items.Add((k + 10).ToString());
                                }
                                DropDownListpage.Items.Add("Others");
                                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                                FpEntry.Height = 335;

                            }
                            else if (totalRows == 0)
                            {
                                DropDownListpage.Items.Add("0");
                                FpEntry.Height = 100;
                            }
                            else
                            {
                                FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                                DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
                                FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                            }
                            if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) > 10)
                            {
                                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                                FpEntry.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                                CalculateTotalPages();
                            }
                            // FpEntry.Sheets[0].PageSize = Convert.ToInt32(ddlto.SelectedValue.ToString()) - Convert.ToInt32(ddlfrom.SelectedValue.ToString()) + 1 + spancount + count+1;
                            FpEntry.Height = 200 + (20 * Convert.ToInt32(totalRows));
                        }

                        if (ddlTest.SelectedItem.Value.ToString() == "--Select--")
                        {
                            FpEntry.Visible = false;
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
            }
            else
            {
                FpEntry.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblnorec.Text = "Please Select Test and then Proceed";
                lblnorec.Visible = true;
            }
        }//-----------------------------date validate------------------------------

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
        FpEntry.Visible = false;
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
        btnprintmaster.Visible = false;
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
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
            bindbranch();

            bindsem();


            bindsec();

            GetTest();
            binddate();
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
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            FpEntry.Visible = true;


            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            btnprintmaster.Visible = true;
            FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
            //  FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        }
        FpEntry.SaveChanges();
        FpEntry.CurrentPage = 0;
    }


    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
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
                    FpEntry.Visible = true;


                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmaster.Visible = true;
                    TextBoxpage.Text = "";
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpEntry.Visible = true;


                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmaster.Visible = true;
                }
            }
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

                FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
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

        }
        return atten;


    }
    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTest.SelectedIndex = -1;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        FpEntry.Visible = false;
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
        FpEntry.Visible = false;

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
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
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
        SelectAll.Checked = false;
        if (selectcout == ddlreport.Items.Count)
        {
            SelectAll.Checked = true;
        }
        if (selectcout > 0)
        {
            TextBox1.Text = "Criteria(" + (selectcout) + ")";
        }
        else
        {
            TextBox1.Text = "---Select---";
        }
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
                FpEntry.Visible = false;
                btnExcel.Visible = false;
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
                FpEntry.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
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

        btnExcel.Visible = false;
        btnprintmaster.Visible = false;
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        FpEntry.Visible = false;
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
            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                dacces2.printexcelreport(FpEntry, strexcelname);
            }
            else
            {
                lblexcelerr.Text = "Please Enter Your Report Name";
                lblexcelerr.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string selected_criteria = "";
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


        string clmnheadrname = "";
        int total_clmn_count = FpEntry.Sheets[0].ColumnCount;
        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (clmnheadrname == "")
            {
                clmnheadrname = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            }
            else
            {
                clmnheadrname = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            }
        }

        string dis_hdng_batch = "Batch Year " + "- " + ddlBatch.SelectedItem.ToString() + " Course " + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        //added by anandan 
        int totsec = ddlSec.Items.Count;
        string dis_hdng_sec = "";
        if (totsec > 0)
        {

            dis_hdng_sec = "Semester " + "- " + ddlSemYr.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        }
        else
        {
            dis_hdng_sec = "Semester " + "- " + ddlSemYr.SelectedItem.ToString();
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

        Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    protected string NumToText(int totamt)
    {

        int inputNo = Convert.ToInt32(totamt.ToString());

        if (inputNo == 0)
            return "Zero";

        int[] numbers = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder strResult = new System.Text.StringBuilder();

        if (inputNo < 0)
        {
            strResult.Append("Minus ");
            inputNo = -inputNo;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        numbers[0] = inputNo % 1000; // units
        numbers[1] = inputNo / 1000;
        numbers[2] = inputNo / 100000;
        numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
        numbers[3] = inputNo / 10000000; // crores
        numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

        for (int i = 3; i > 0; i--)
        {
            if (numbers[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (numbers[i] == 0) continue;
            u = numbers[i] % 10; // ones
            t = numbers[i] / 10;
            h = numbers[i] / 100; // hundreds
            t = t - 10 * h; // tens
            if (h > 0) strResult.Append(words0[h] + "Hundred ");
            if (u > 0 || t > 0)
            {
                if (h > 0 || i == 0) strResult.Append(" ");
                if (t == 0)
                    strResult.Append(words0[u]);
                else if (t == 1)
                    strResult.Append(words1[u]);
                else
                    strResult.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) strResult.Append(words3[i - 1]);
        }

        string substring = "";
        substring = strResult.ToString();
        return substring.ToString().TrimEnd();

    }

    public void load_attendance()
    {
        string sections = "";
        Hashtable hatonduty = new Hashtable();
        DataSet dsonduty = new DataSet();
        Hashtable hatodtot = new Hashtable();

        DataSet dsmark = new DataSet();
        DataView dvmark = new DataView();

        Hashtable has_hs = new Hashtable();
        Hashtable has_attnd_masterset_notconsider = new Hashtable();
        Hashtable hatabsentvalues = new Hashtable();
        Hashtable has = new Hashtable();
        DateTime temp_date, dt2;
        Hashtable hat_holy = new Hashtable();
        Hashtable temp_has_subj_code = new Hashtable();
        int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
        Boolean holiflag = false;
        try
        {
            DataSet ds_attndmaster = new DataSet();
            int count_master = 0;
            string splhrsec = "";
            string rstrsec = "";
            if (ddlSec.SelectedValue.ToString() == "" || ddlSec.SelectedValue.ToString() == "-1")
            {
                strsec = "";
                rstrsec = "";
                splhrsec = "";
            }
            else
            {
                strsec = " and sections='" + ddlSec.SelectedItem.ToString() + "'";
                rstrsec = " and r.sections='" + ddlSec.SelectedItem.ToString() + "'";
                splhrsec = "and sections='" + ddlSec.SelectedItem.ToString() + "'";
            }

            string date1 = txtFromDate.Text;
            string[] split = date1.Split(new Char[] { '/' });
            string datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            dt1 = Convert.ToDateTime(datefrom.ToString());

            string date2 = txtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '/' });
            string dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            dt2 = Convert.ToDateTime(dateto.ToString());

            DataSet ds_student = d2.select_method_wo_parameter(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',r.Adm_Date,p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(r.roll_no),convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year=" + ddlBatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlBatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlBranch.SelectedValue.ToString() + " and s.degree_code= " + ddlBranch.SelectedValue.ToString() + " and  s.semester=" + ddlSemYr.SelectedValue.ToString() + " and p.semester=" + ddlSemYr.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL)  " + strsec + " ", "Text");
            int stud_count = ds_student.Tables[0].Rows.Count;
            int no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
            int mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
            int evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
            string order = ds_student.Tables[0].Rows[0]["order"].ToString();
            string sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();


            has.Clear();
            has.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = d2.select_method("ATT_MASTER_SETTING", has, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);
            if (count_master > 0)
            {
                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                {
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        if (!has_attnd_masterset.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            has_attnd_masterset.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")//==31/5/12 pRABHA
                    {
                        if (!has_attnd_masterset_notconsider.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            has_attnd_masterset_notconsider.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {

                        if (!hatabsentvalues.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            hatabsentvalues.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                }
            }

            string[] fromdatespit = txtFromDate.Text.Split('/');
            string[] todatespit = txtToDate.Text.Split('/');
            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);


            string dummy_date = "", month_year = "", strDay = "", full_hour = "", single_hour = "", temp_hr_field = "", date_temp_field = "";

            ht_sphr.Clear();
            string hrdetno = "";
            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " " + splhrsec + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
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
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string spl_hr_rights = d2.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
            if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
            {
                splhr_flag = true;
            }

            int present_count = 0;


            temp_date = dt1;
            string subject_no = ds2.Tables[1].Rows[i]["subject_no"].ToString();
            string stralldetaisquery = "select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and subject_no='" + subject_no + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and subject_no='" + ddlBranch.SelectedValue.ToString() + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select * from Semester_Schedule where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester='" + ddlSemYr.SelectedItem.ToString() + "'  " + strsec + " order by FromDate desc";
            stralldetaisquery = stralldetaisquery + " ;select * from Alternate_Schedule where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester='" + ddlSemYr.SelectedItem.ToString() + "'  " + strsec + "  order by FromDate desc";
            DataSet dsalldetails = d2.select_method_wo_parameter(stralldetaisquery, "Text");



            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlSemYr.SelectedItem.ToString() + "' and s.batch_year='" + ddlBatch.Text.ToString() + "'  and s.degree_code='" + ddlBranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + ddlSemYr.SelectedItem.ToString() + "' and batch_year='" + ddlBatch.Text.ToString() + "'  and degree_code='" + ddlBranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select subject_type,LAB From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            string semstartdate = "";
            string noofdays = "";
            string startday = "";
            if (dssem.Tables[0].Rows.Count > 0)
            {
                semstartdate = dssem.Tables[0].Rows[0]["start_date"].ToString();
                noofdays = dssem.Tables[0].Rows[0]["nodays"].ToString();
                startday = dssem.Tables[0].Rows[0]["starting_dayorder"].ToString();
            }

            Hashtable hatdc = new Hashtable();
            try
            {
                for (int dc = 0; dc < dssem.Tables[1].Rows.Count; dc++)
                {
                    DateTime dtdcf = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["from_date"].ToString());
                    DateTime dtdct = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["to_date"].ToString());
                    for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                    {
                        if (!hatdc.Contains(dtc))
                        {
                            hatdc.Add(dtc, dtc);
                        }
                    }
                }
            }
            catch
            {
            }
            string subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
            while (temp_date <= dt2)
            {
                Boolean check_alter = false;
                if (!hatdc.Contains(temp_date))
                {
                    if (splhr_flag == true)
                    {
                        if (ht_sphr.Contains(Convert.ToString(temp_date)))
                        {
                            getspecial_hr();
                        }
                    }

                    if (!hat_holy.ContainsKey(temp_date))
                    {
                        if (!hat_holy.ContainsKey(temp_date))
                        {
                            hat_holy.Add(temp_date, "3*0*0");
                        }
                    }

                    value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
                    split_holiday_status = value_holi_status.Split('*');

                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = no_of_hrs;
                    }
                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                    {

                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                        {
                            split_holiday_status_1 = mng_hrs + 1;
                            split_holiday_status_2 = no_of_hrs;
                        }

                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                        {
                            split_holiday_status_1 = 1;
                            split_holiday_status_2 = mng_hrs;
                        }
                    }
                    else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                    {
                        split_holiday_status_1 = 0;
                        split_holiday_status_2 = 0;
                    }


                    if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                    {
                    }
                    else
                    {
                        holiflag = true;
                        dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + "";
                        DataView dvaltersech = dsalldetails.Tables[7].DefaultView;
                        dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + "";
                        DataView dvsemsech = dsalldetails.Tables[6].DefaultView;

                        if (dvsemsech.Count > 0)
                        {
                            if (no_of_hrs > 0)
                            {
                                dummy_date = temp_date.ToString();
                                string[] dummy_date_split = dummy_date.Split(' ');
                                string[] final_date_string = dummy_date_split[0].Split('/');
                                dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                if (order != "0")
                                {
                                    strDay = temp_date.ToString("ddd");
                                }
                                else
                                {
                                    string[] sp = dummy_date.Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    strDay = d2.findday(curdate, ddlBranch.SelectedValue.ToString(), ddlSemYr.SelectedItem.ToString(), ddlBatch.Text.ToString(), semstartdate, noofdays, startday);
                                }


                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    check_alter = false;
                                    present_count = 0;
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    if (dvaltersech.Count > 0)
                                    {
                                        for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                        {
                                            full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                            if (full_hour.Trim() != "")
                                            {
                                                temp_has_subj_code.Clear();
                                                string[] split_full_hour = full_hour.Split(';');
                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                {
                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                    string[] split_single_hour = single_hour.Split('-');
                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                    {
                                                        string subjectno = split_single_hour[0].ToString();
                                                        check_alter = true;
                                                        Hashtable has_stud_list = new Hashtable();
                                                        subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                        if (subj_type != "1" && subj_type.Trim().ToLower() != "true")
                                                        {
                                                            dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                            DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                            for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                            {
                                                                string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                if (dvattva.Count > 0)
                                                                {
                                                                    string attval = dvattva[0][date_temp_field].ToString();
                                                                    if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                    {
                                                                        if (has_attnd_masterset.ContainsKey(attval))
                                                                        {
                                                                            if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                            {
                                                                                present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                present_count++;
                                                                                has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                            }
                                                                            else
                                                                            {
                                                                                has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                            }
                                                                        }
                                                                        if (has_total_attnd_hour.Contains(rollno + '-' + subjectno))
                                                                        {
                                                                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno + '-' + subjectno]);
                                                                            present_count++;
                                                                            has_total_attnd_hour[rollno + '-' + subjectno] = present_count;
                                                                        }
                                                                        else
                                                                        {
                                                                            has_total_attnd_hour.Add(rollno + '-' + subjectno, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            dsalldetails.Tables[3].DefaultView.RowFilter = "hour_value=" + temp_hr + "  and day_value='" + strDay + "' and subject_no='" + subject_no + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                            DataView dvlabbatch = dsalldetails.Tables[3].DefaultView;
                                                            for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                            {
                                                                string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                if (batch != null && batch.Trim() != "")
                                                                {
                                                                    dsalldetails.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                    DataView dvlabhr = dsalldetails.Tables[1].DefaultView;
                                                                    for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                    {
                                                                        string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                        DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                        if (dvattva.Count > 0)
                                                                        {
                                                                            string attval = dvattva[0][date_temp_field].ToString();
                                                                            if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                            {
                                                                                if (has_attnd_masterset.ContainsKey(attval))
                                                                                {
                                                                                    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                                    {
                                                                                        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                        present_count++;
                                                                                        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                    }
                                                                                }
                                                                                if (has_total_attnd_hour.Contains(rollno + '-' + subjectno))
                                                                                {
                                                                                    present_count = Convert.ToInt16(has_total_attnd_hour[rollno + '-' + subjectno]);
                                                                                    present_count++;
                                                                                    has_total_attnd_hour[rollno + '-' + subjectno] = present_count;
                                                                                }
                                                                                else
                                                                                {
                                                                                    has_total_attnd_hour.Add(rollno + '-' + subjectno, 1);
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
                                    present_count = 0;
                                    if (check_alter == false)
                                    {
                                        full_hour = dvsemsech[0][temp_hr_field].ToString();
                                        if (full_hour.Trim() != "")
                                        {
                                            temp_has_subj_code.Clear();
                                            string[] split_full_hour_sem = full_hour.Split(';');
                                            for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                            {
                                                single_hour = split_full_hour_sem[semi_colon].ToString();
                                                string[] split_single_hour = single_hour.Split('-');

                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                {
                                                    string subjectno = split_single_hour[0].ToString();
                                                    Hashtable has_stud_list = new Hashtable();
                                                    subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                    if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                    {
                                                        dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                        DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                        for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                        {
                                                            string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                            dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                            DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                            if (dvattva.Count > 0)
                                                            {
                                                                string attval = dvattva[0][date_temp_field].ToString();
                                                                if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                {
                                                                    if (has_attnd_masterset.ContainsKey(attval))
                                                                    {
                                                                        if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                        {
                                                                            present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                            present_count++;
                                                                            has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                        }
                                                                        else
                                                                        {
                                                                            has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                        }
                                                                    }
                                                                    if (has_total_attnd_hour.Contains(rollno + '-' + subjectno))
                                                                    {
                                                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno + '-' + subjectno]);
                                                                        present_count++;
                                                                        has_total_attnd_hour[rollno + '-' + subjectno] = present_count;
                                                                    }
                                                                    else
                                                                    {
                                                                        has_total_attnd_hour.Add(rollno + '-' + subjectno, 1);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dsalldetails.Tables[2].DefaultView.RowFilter = "hour_value=" + temp_hr + " and subject_no='" + subject_no + "'  and day_value='" + strDay + "' and timetablename='" + dvsemsech[0]["ttname"].ToString() + "'";
                                                        DataView dvlabbatch = dsalldetails.Tables[2].DefaultView;
                                                        for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                        {
                                                            string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                            if (batch != null && batch.Trim() != "")
                                                            {
                                                                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                {
                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                    dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                    DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                    if (dvattva.Count > 0)
                                                                    {
                                                                        string attval = dvattva[0][date_temp_field].ToString();
                                                                        if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                        {
                                                                            if (!has_attnd_masterset.ContainsKey(attval))
                                                                            {
                                                                                if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                                                                {
                                                                                    present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                    present_count++;
                                                                                    has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                }
                                                                                else
                                                                                {
                                                                                    has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                                                                }
                                                                            }
                                                                            if (has_total_attnd_hour.Contains(rollno + '-' + subjectno))
                                                                            {
                                                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno + '-' + subjectno]);
                                                                                present_count++;
                                                                                has_total_attnd_hour[rollno + '-' + subjectno] = present_count;
                                                                            }
                                                                            else
                                                                            {
                                                                                has_total_attnd_hour.Add(rollno + '-' + subjectno, 1);
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
                                check_alter = false;
                            }
                        }
                    }
                }

                temp_date = temp_date.AddDays(1);
            }

            for (int r = 0; r < FpEntry.Sheets[0].RowCount; r++)
            {
                for (int c = 7; c < FpEntry.Sheets[0].ColumnCount - 2; c = c + 2)
                {
                    string roll = FpEntry.Sheets[0].Cells[r, 1].Text.ToString().Trim().ToLower();
                    string subno = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, c].Note.ToString();
                    double attnd_hr = 0, tot_hr = 0;
                    if (has_load_rollno.Contains(roll + '-' + subno))
                    {
                        attnd_hr = Convert.ToDouble(has_load_rollno[roll + '-' + subno]);
                    }
                    if (has_total_attnd_hour.Contains(roll + '-' + subno))
                    {
                        tot_hr = Convert.ToDouble(has_total_attnd_hour[roll + '-' + subno]);
                    }

                    FpEntry.Sheets[0].Cells[r, c].Text = attnd_hr.ToString();
                    FpEntry.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch
        {

        }
    }
    public void getspecial_hr(DateTime temp_date)
    {

        try
        {
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {
                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select spa.roll_no,spa.attendance,spd.subject_no from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.hrdet_no in(" + hrdetno + ") order by spa.hrdet_no";
                DataSet dsspatatt = d2.select_method_wo_parameter(splhr_query_master, "text");
                for (int sp = 0; sp < dsspatatt.Tables[0].Rows.Count; sp++)
                {
                    string rollno = dsspatatt.Tables[0].Rows[sp]["roll_no"].ToString().Trim().ToLower();
                    string sub = dsspatatt.Tables[0].Rows[sp]["subject_no"].ToString().Trim().ToLower();
                    string attval = dsspatatt.Tables[0].Rows[sp]["attendance"].ToString().Trim().ToLower();
                    int present_count = 0;
                    if (attval != "8" && attval != "12" && attval.Trim() != "")
                    {
                        if (!has_attnd_masterset.ContainsKey(attval))
                        {
                            if (has_load_rollno.Contains(rollno + '-' + sub))
                            {
                                present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + sub]);
                                present_count++;
                                has_load_rollno[rollno + '-' + sub] = present_count;
                            }
                            else
                            {
                                has_load_rollno.Add(rollno + '-' + sub, 1);
                            }
                        }
                        if (has_total_attnd_hour.Contains(rollno + '-' + sub))
                        {
                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno + '-' + sub]);
                            present_count++;
                            has_total_attnd_hour[rollno + '-' + sub] = present_count;
                        }
                        else
                        {
                            has_total_attnd_hour.Add(rollno + '-' + sub, 1);
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
}

