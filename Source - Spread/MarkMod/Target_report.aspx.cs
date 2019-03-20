//========================modified for kongu changes on 28.06.12 by mythili,modified on 30.06.12(ht_fail_subject!containskey) by mythili
//modified on 04.07.12 by mythili (ht_fail)subject)for each loop
//=====modified on 12.07.12 by mythili(default date sem start date end date)
//=======printmaster sec error and hdr state and pincode and regno column only visible
//=======modified on 17.07.12 by mythili same format like university_mark report
//========added printmaster setting condition based on mastersetting in pageload on 21.07.12 by mythili
//=======modified on 23.07.12 (check the condition for subno b4 set the mark)
//======modified on 24.07.12 (changes in dipslaying standard deviation calc foreach loop),25.07.12 calc the %pass outof total no of student
//===modified width for each columns on 30.07.12 by m ythili
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BalAccess;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Reflection;


public partial class Target_report : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {


            //''------------clg left logo
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(90);
            img.Height = Unit.Percentage(90);
            return img;

            //'-------------clg right logo
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl;
            img2.Width = Unit.Percentage(90);
            img2.Height = Unit.Percentage(90);
            return img2;

        }
    }
    public class RotateTextCellType : FarPoint.Web.Spread.TextCellType
    {
        public override Control PaintCell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, Object value, Boolean upperLevel)
        {

            parent.Style.Add("writing-mode", "tb-rl");
            parent.Width = System.Web.UI.WebControls.Unit.Pixel(20);
            return base.PaintCell(id, parent, style, margin, value, upperLevel);
        }

    }

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

    Hashtable htdeviation = new Hashtable();
    Hashtable htfail = new Hashtable();
    Hashtable htabsent = new Hashtable();
    Hashtable htpresent = new Hashtable();
    Hashtable htpassperc = new Hashtable();
    Hashtable htclsavg = new Hashtable();
    Hashtable ht_fail_subject = new Hashtable();
    DAccess2 dacces2 = new DAccess2();

    DataSet ds_holi = new DataSet();
    DataSet ds_optim = new DataSet();
    Boolean Yesflag = false;
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
    string stud_roll = "";
    Boolean RnkFlag;
    Boolean PresentFlag = false;
    Boolean callattfun;
    DateTime dt1, dt2;
    DateTime date_today;
    int[] hasharray;
    int student = 0;
    string strorder = "";
    string strregorder = "";

    int ic = 0;
    int i;
    static int cook = 0;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Boolean cellroll = false;
    Boolean flag_true = false;
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
    string group_code = "", columnfield = "";

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
    int next = 0;
    int minpresII = 0;
    string value, date;
    int m, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
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
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    int countds = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
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
    int min_mark, per_sub_count;
    double per_mark;

    int passcount, failcount, maxcount, mincount, avg_50count, avg_65count, pre_count, ab_count;
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
    DataSet ds_2 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable attmaster = new Hashtable();
    DataSet attnew = new DataSet();

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

    //'-----------------new mythili //20.04.12
    DataSet dsprint = new DataSet();
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string MultiISO = "";
    //====================added on 16.07.12
    string address3 = "";
    string affliated = "";
    string category = "";
    //=========================
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    string pincode = "";
    string state = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string form_heading_name = "";
    string batch_degree_branch = "";

    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    static Boolean splhr_flag = false;

    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0;
    double per_leave_fals = 0;
    double per_leave_true = 0;
    int irow1 = 0;
    long stud_count = 0;
    int demfcal, demtcal;
    string monthcal;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    static int prevs_endrow = 0;
    //'------------------------------
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

    public void sem_start_end_date()
    {
        string semdate = "select convert(varchar(10),start_date,103) as startdate,convert(varchar(10),end_date,103) as enddate from seminfo where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + "";
        SqlDataAdapter da_semdate = new SqlDataAdapter(semdate, con);
        con.Close();
        con.Open();
        DataSet ds_semdate = new DataSet();
        da_semdate.Fill(ds_semdate);
        if (ds_semdate.Tables[0].Rows.Count > 0)
        {
            txtFromDate.Text = ds_semdate.Tables[0].Rows[0]["startdate"].ToString();
            txtToDate.Text = ds_semdate.Tables[0].Rows[0]["enddate"].ToString();
        }
        else
        {
            string dt1 = DateTime.Today.ToShortDateString();
            string[] dsplit = dt1.Split(new Char[] { '/' });
            dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dateconcat.ToString();

            string dt2 = DateTime.Today.ToShortDateString();
            string[] dt2split = dt2.Split(new Char[] { '/' });
            date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
            txtToDate.Text = date1concat.ToString();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        lblnorec.Visible = false;
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

                btnselecttargetstudent.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                lblnorecc.Visible = false;

                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;

                prevs_endrow = 0;
                Session["QueryString"] = "";
                group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    columnfield = " and group_code='" + group_code + "'";
                }
                else
                {
                    columnfield = " and user_code='" + Session["usercode"] + "'";
                }
                hat.Clear();
                hat.Add("column_field", columnfield.ToString());
                dsprint = dacces2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                else
                {
                    lblnorec.Text = "Give college rights to the staff";
                    lblnorec.Visible = true;
                    RadioHeader.Visible = false;
                    Radiowithoutheader.Visible = false;
                    lblpages.Visible = false;
                    ddlpage.Visible = false;
                    FpEntry.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    return;
                }


                Pageload(sender, e);
                //-----------------------------------
                splhr_flag = false;
                con.Close();
                cmd.CommandText = "select rights from  special_hr_rights where usercode=" + Session["usercode"].ToString() + "";
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = "";
                        Hashtable od_has = new Hashtable();

                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;

                        }
                    }
                }
                //-------------------------------------

            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (rbselectionlist.SelectedValue == "1")
            {
                btnGo.Visible = true;
                btnselecttargetstudent.Visible = false;
            }
            else if (rbselectionlist.SelectedValue == "2")
            {
                btnGo.Visible = false;
                btnselecttargetstudent.Visible = true;
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
        collegecode = Session["InternalCollegeCode"].ToString();
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
        collegecode = Session["InternalCollegeCode"].ToString();
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


            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);

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
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;

        ddlTest.Items.Clear();
        ddlBranch.Items.Clear();

        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();

        bindbranch();


        bindsem();
        bindsec();
        sem_start_end_date();
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
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;

        bindsem();
        bindsec();
        GetTest();

        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {

            bindsem();
            sem_start_end_date();
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


        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["InternalCollegeCode"] + "", con);
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
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["InternalCollegeCode"] + "", con);
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

        con.Close();
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();

        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

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

                                int hdflag1 = 0;

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
                            }
                            dummydate = dummydate.AddDays(1);
                        }
                    }
                }
            }

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
    //=================================================================================================
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
    //===============================================================================================
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
                            if ((hr == "1"))
                            {
                                if ((strcalflag == "0") && (strcalflag != null) && (strcalflag != string.Empty))
                                {

                                    studpresn += 1;
                                    if (htpresent.Contains(Convert.ToInt32(subno)))
                                    {
                                        int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htpresent));
                                        val++;
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

    //-------------------------------------------------------------------------------------------------
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

    //================================================================================================
    public void optimize(DateTime exam_date, string sno, int cno)
    {

        double eod = 0;
        double Present = 0;
        double Absent = 0;
        double Onduty = 0;
        double Leave = 0;
        string minmark = "";
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

            if (ds_optim.Tables[0].Rows.Count > 0)
            {

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

                        drhour.Close();
                        lcon3.Close();

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


            tot_stud = stud_pass + stud_fail;
            pass_perc = (Convert.ToDouble(stud_pass) / Convert.ToDouble(tot_stud)) * 100.0;
            pass_perc = Math.Round(pass_perc, 2);


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
        hat.Add("coll_code", int.Parse(Session["InternalCollegeCode"].ToString()));


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

            if (ds7.Tables[1].Rows.Count > 0)
            {
                while (dumm_from_date <= (per_to_date))
                {

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

    public void persentmonthcal()
    {
        try
        {
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

            // try
            {
                int my_un_mark = 0;
                int njdate_mng = 0, njdate_evng = 0;
                int per_holidate_mng = 0, per_holidate_evng = 0;

                mng_conducted_half_days = 0;
                evng_conducted_half_days = 0;

                notconsider_value = 0;
                conduct_hour_new = 0;


                cal_from_date = cal_from_date_tmp;
                cal_to_date = cal_to_date_tmp;
                dumm_from_date = per_from_date;



                stud_roll = ds5.Tables[0].Rows[irow1]["RollNumber"].ToString();
                hat.Clear();
                hat.Add("std_rollno", ds5.Tables[0].Rows[irow1]["RollNumber"].ToString());
                hat.Add("from_month", cal_from_date);
                hat.Add("to_month", cal_to_date);
                ds_2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
                mmyycount = ds_2.Tables[0].Rows.Count;
                moncount = mmyycount - 1;
                //Modified By Srinath 26/2/2013
                // if (rows_count == 0)
                if (irow1 == 0)
                {
                    hat.Clear();
                    hat.Add("degree_code", int.Parse(ddlBranch.SelectedValue.ToString()));
                    hat.Add("sem", int.Parse(ddlSemYr.SelectedItem.ToString()));
                    hat.Add("from_date", frdate.ToString());
                    hat.Add("to_date", todate.ToString());
                    hat.Add("coll_code", int.Parse(Session["InternalCollegeCode"].ToString()));


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

                    ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                    Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                    Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                    Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                    holiday_table11.Clear();
                    holiday_table21.Clear();
                    holiday_table31.Clear();
                    if (ds3.Tables[0].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                        {
                            if (ds3.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                            {
                                halforfull = "0";
                            }
                            else
                            {
                                halforfull = "1";
                            }
                            if (ds3.Tables[0].Rows[0]["morning"].ToString() == "False")
                            {
                                mng = "0";
                            }
                            else
                            {
                                mng = "1";
                            }
                            if (ds3.Tables[0].Rows[0]["evening"].ToString() == "False")
                            {
                                evng = "0";
                            }
                            else
                            {
                                evng = "1";
                            }

                            holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                            string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                            // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                        }
                    }

                    if (ds3.Tables[1].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                        {
                            string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                            if (ds3.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                            {
                                halforfull = "0";
                            }
                            else
                            {
                                halforfull = "1";
                            }
                            if (ds3.Tables[1].Rows[k]["morning"].ToString() == "False")
                            {
                                mng = "0";
                            }
                            else
                            {
                                mng = "1";
                            }
                            if (ds3.Tables[1].Rows[k]["evening"].ToString() == "False")
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

                    if (ds3.Tables[2].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                        {
                            string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                            if (ds3.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                            {
                                halforfull = "0";
                            }
                            else
                            {
                                halforfull = "1";
                            }
                            if (ds3.Tables[2].Rows[k]["morning"].ToString() == "False")
                            {
                                mng = "0";
                            }
                            else
                            {
                                mng = "1";
                            }
                            if (ds3.Tables[2].Rows[k]["evening"].ToString() == "False")
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



                    //}//Hidden By Srianth 26/2/2013
                    //===================================
                    //------------------------------------------------------------------
                    if (ds3.Tables[0].Rows.Count != 0)
                    {
                        ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                        diff_date = Convert.ToString(ts.Days);
                        dif_date1 = double.Parse(diff_date.ToString());
                    }
                }//Added By Srinath 26/2/2013
                next = 0;

                if (ds_2.Tables[0].Rows.Count != 0)
                {
                    int rowcount = 0;
                    int ccount;
                    ccount = ds3.Tables[1].Rows.Count;
                    ccount = ccount - 1;
                    //if ( == ds_2.Tables [0].Rows [mmyycount].["Month_year"])
                    while (dumm_from_date <= (per_to_date))
                    {
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
                            if (cal_from_date == int.Parse(ds_2.Tables[0].Rows[next]["month_year"].ToString()))
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

                                    if (ds3.Tables[1].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
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

                                    if (ds3.Tables[2].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
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
                                            value = ds_2.Tables[0].Rows[next][date].ToString();

                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;
                                                    if (attmaster.Contains(value.ToString()))
                                                    {
                                                        ObtValue = int.Parse(GetCorrespondingKey(value.ToString(), attmaster).ToString());
                                                    }
                                                    else
                                                    {
                                                        ObtValue = 0;
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
                                            value = ds_2.Tables[0].Rows[next][date].ToString();

                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;

                                                    if (attmaster.Contains(value.ToString()))
                                                    {
                                                        ObtValue = int.Parse(GetCorrespondingKey(value.ToString(), attmaster).ToString());
                                                    }
                                                    else
                                                    {
                                                        ObtValue = 0;
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

                                                    per_leave += 1;
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

                                DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                                dumm_fdate = dumm_fdate.AddMonths(1);
                                dumm_from_date = dumm_fdate;

                                if (dumm_from_date.Day == 1)
                                {

                                    cal_from_date++;


                                    if (moncount > next)
                                    {
                                        //  next++;
                                    }

                                }

                                if (moncount > next)
                                {
                                    i--;
                                }
                            }

                        }
                    }
                    int diff_Date = per_from_date.Day - dumm_from_date.Day;
                }



                per_tot_ondu = tot_ondu;
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


                //Present = 0;
                //tot_per_hrs = 0;
                //Absent = 0;
                //Onduty = 0;
                //Leave = 0;
                //workingdays = 0;
                //per_holidate = 0;
                //dum_unmark = 0;
                //absent_point = 0;
                //leave_point = 0;
                //njdate = 0;
                //tot_ondu = 0;
            }
        }
        catch
        {
        }
    }
    public void SpreadBind()
    {
        try
        {
            string gcount = "0";
            DataSet dstargetstudent = new DataSet();
            string y = "";
            string totalregstudent = "";
            long avgcnt = 0;
            long register_count = 0;
            lblnorec.Visible = false;
            int hasrow_count = 0;
            int mark_new = 0;
            Radiowithoutheader.Visible = false;
            RadioHeader.Visible = false;//---------20.04.12
            FpEntry.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            string staff_name = "";
            int ug = 0, pg = 0;
            int nn = 0;
            string resminmrk = "";
            string a2 = "";
            int b2 = 0;
            int totalsubcount = 0;
            int spancnt = 0;
            string subject_code = "";

            int[] maxtot = new int[100];
            string examdate = "";
            string subname = "";
            int rankcount = 0;
            int serialno = 0;
            string ff = ddlDegree.SelectedItem.Text;
            string fd = ddlTest.SelectedItem.Text;
            int rwcnt = 0;
            Boolean isabsent = false;
            string str_sec = "";
            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlBranch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSemYr.SelectedValue.ToString();
            criteria_no = ddlTest.SelectedValue.ToString();

            DataSet dsarrearrollno = new DataSet();
            ArrayList alarrrollnum = new ArrayList();
            if (rbselectionlist.SelectedValue == "1")
            {
                string previousarrearquery = "select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "'";
                dsarrearrollno = dacces2.select_method_wo_parameter(previousarrearquery, "n");
                if (dsarrearrollno.Tables[0].Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dsarrearrollno.Tables[0].Rows.Count; cnt++)
                    {
                        alarrrollnum.Add(dsarrearrollno.Tables[0].Rows[cnt]["roll_no"].ToString());
                    }
                }
            }
            else if (rbselectionlist.SelectedValue == "2")
            {
                int isval = 0;
                for (int i = 1; i < sprdselectrollno.Sheets[0].Rows.Count; i++)
                {
                    isval = Convert.ToInt16(sprdselectrollno.Sheets[0].Cells[i, 4].Value);
                    if (isval == 1)
                    {
                        alarrrollnum.Add(sprdselectrollno.Sheets[0].Cells[i, 1].Text);
                    }
                }
            }

            gcount = Convert.ToString(alarrrollnum.Count);


            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = "";
                str_sec = "";
            }
            else
            {
                strsec = sections.ToString();
                str_sec = " and sections='" + sections.ToString() + "'";
            }


            FpEntry.Sheets[0].RowCount = 0;
            FpEntry.Sheets[0].ColumnCount = 0;
            FpEntry.Sheets[0].ColumnHeader.RowCount = 7;
            FpEntry.Sheets[0].RowHeader.Visible = false;

            hat.Clear();
            hat.Add("colege_code", Session["InternalCollegeCode"].ToString());
            ds15 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
            countds = ds15.Tables[0].Rows.Count;
            count = ds15.Tables[0].Rows.Count;
            attmaster.Clear();
            if (count > 0)
            {
                for (int lcnt = 0; lcnt < ds15.Tables[0].Rows.Count; lcnt++)
                {
                    attmaster.Add(ds15.Tables[0].Rows[lcnt]["LeaveCode"].ToString(), ds15.Tables[0].Rows[lcnt]["CalcFlag"].ToString());
                }
            }
            filteration();
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
            totalsubcount = ds2.Tables[1].Rows.Count;
            if (0 < ds2.Tables[1].Rows.Count)
            {

                int uo = 0;
                FpEntry.Sheets[0].ColumnCount = ds2.Tables[1].Rows.Count + 10;
                int yb = FpEntry.Sheets[0].ColumnCount;
                nn = FpEntry.Sheets[0].ColumnCount % 2;
                if (nn == 0)
                {
                    uo = yb / 2;
                }
                else
                {
                    int ns = yb - 1;
                    uo = ns / 2;

                }

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
                //Hidden By Srinath 26/2/2013 
                //hat.Clear();                
                //hat.Add("colege_code", Session["InternalCollegeCode"].ToString());
                //ds8 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                //countds = ds8.Tables[0].Rows.Count;

                //******************
                FpEntry.Sheets[0].RowCount++;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);
                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "TARGET REPORT";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Height = 40;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Visible = false;
                //@@@@@@@@@@@@ academic year by mythili on 28.06.12
                // int academic_year = System.DateTime.Now.Year;

                int academic_year;
                string acefromyear = "";
                string acetoyear = "";
                int academic_toyear;

                string academicfromtoyear = GetFunction("select value from master_settings where settings='Academic year'");
                if (academicfromtoyear != "")
                {
                    string[] fromtoyear = academicfromtoyear.Split(',');
                    acefromyear = fromtoyear[0].ToString();
                    acetoyear = fromtoyear[1].ToString();

                }

                int yr = 0;
                int tot_sem = 0;//int.Parse(ddlBatch.SelectedValue.ToString()) + 1;//-23/6/12 PRABHA
                yr = 0;
                cmd = new SqlCommand("select ndurations from ndegree where batch_year=" + ddlBatch.SelectedValue + "  and degree_code=" + ddlBranch.SelectedValue + "", con);

                SqlDataReader no_on_sem_dr;
                con.Close();
                con.Open();
                no_on_sem_dr = cmd.ExecuteReader();
                if (no_on_sem_dr.HasRows)
                {
                    while (no_on_sem_dr.Read())
                    {
                        tot_sem = Convert.ToInt32(no_on_sem_dr[0].ToString());
                        yr = Convert.ToInt32(ddlBatch.SelectedValue.ToString()) + (tot_sem / 2);
                    }
                }
                else
                {
                    cmd = new SqlCommand("select duration from degree where degree_code=" + ddlBranch.SelectedValue + "", con);
                    con.Close();
                    con.Open();
                    no_on_sem_dr = cmd.ExecuteReader();
                    if (no_on_sem_dr.HasRows)
                    {
                        while (no_on_sem_dr.Read())
                        {
                            tot_sem = Convert.ToInt32(no_on_sem_dr[0].ToString());
                            yr = Convert.ToInt32(ddlBatch.SelectedValue.ToString()) + (tot_sem / 2);
                        }
                    }
                }
                //-----------------------------------------------------------

                if ((ddlpage.Text.ToString() == "") || (ddlpage.Text.ToString() == "1") || (ddlpage.Text.ToString() == "0"))
                {
                    //@@@@@@@@@@@@@
                    string dept_acronym = GetFunction("select dept_acronym from department where dept_name='" + ddlBranch.SelectedItem.Text + "'");
                    FpEntry.Sheets[0].RowCount++;
                    //Modified By Srinath 7/3/2013
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 5);
                    // FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 10);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "Degree :   " + ff + " " + ddlBranch.SelectedItem.ToString(); ///modified on 16.07.12 +dept_acronym;//modified on 28.06.12  PROGRAMME :  +ff
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;//Added By Srinath 7/3/2013
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 5);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ff+" "+ddlBranch.SelectedItem.ToString();///"DEGREE : " + ff;  // " TEST NAME : " + fd; mofdified
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium; //Hidden By Srinath 7/3/2013

                    //Modified By Srinath ========STart
                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 6, 1, 4);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Academic Year : ";/// +academic_year + "-" + (academic_year + 1);  //"START DATE : " + txtFromDate.Text; modified
                    ////   FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 5, 1, totalsubcount + 1);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = "Academic Year :  " + (acefromyear) + "-" + (acetoyear);/// +academic_year + "-" + (academic_year + 1);  //"START DATE : " + txtFromDate.Text; modified
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    //==========End

                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 10, 1, 2);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].Text = (academic_year - 1) + "-" + (academic_year);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].Font.Bold = true;

                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 5 + totalsubcount, 1, 2);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1,  totalsubcount + 5].Text = (academic_year - 1) + "-" + (academic_year);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1,  totalsubcount + 5].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1,  totalsubcount + 5].Font.Bold = true;


                    if (totalsubcount > 6)
                    {
                        spancnt = 4 + (totalsubcount - 6);
                    }
                    else
                    {
                        spancnt = 4;
                    }

                    //Added by Srinath 7/3/2013 ===Start
                    if (FpEntry.Sheets[0].ColumnCount > 12)
                    {
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 12, 1, spancnt);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 12].Text = "Batch Passing Out Year : " + yr;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 12].Font.Bold = true;
                    }
                    else
                    {
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7, 1, spancnt);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Text = "Batch Passing Out Year : " + yr;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Font.Bold = true;
                    }
                    //===========End


                    FpEntry.Sheets[0].RowCount++;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, 2);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "Semester No : " + findroman(semester); //"BATCH YEAR  :  " + batch; modified on 28.06.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;//Added By Srinath 7/3/2013
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 1);//30.07.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = "Year:" + getyear(semester);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Font.Bold = true;


                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, 1);
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, 3);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = "Section : " + sections;// +sections;  // "SEMESTER NO  :  " + semester; modified on 28.06.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Bold = true;//Added By Srinath 7/3/2013
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    // FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 5, 1, 1);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = sections;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = "";

                    //Modified By Srinath 7/3/2013
                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 6, 1, 4);


                    //@@@@@@@@@@@@@ find the min exam date month and year
                    string min_examdate = "";
                    string final_date = "";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        string[] spl_examdate = examdate.Split('/');
                        string aftr_spl = spl_examdate[1].ToString() + "/" + spl_examdate[0].ToString() + "/" + spl_examdate[2].ToString();
                        if (min_examdate == "")
                        {
                            min_examdate = aftr_spl;

                        }
                        else
                        {
                            if (Convert.ToDateTime(min_examdate) < Convert.ToDateTime(aftr_spl))
                            {
                                final_date = min_examdate;
                            }
                            else
                            {
                                final_date = aftr_spl;
                            }
                        }

                    }
                    int date_mm = 0;
                    int date_yr = 0;

                    if (final_date != "")
                    {
                        date_mm = Convert.ToDateTime(final_date).Month;
                        date_yr = Convert.ToDateTime(final_date).Year;
                    }
                    string strExam_month = "";
                    if (date_mm.ToString() != "")
                    {
                        if (date_mm.ToString() == "1")
                            strExam_month = "Jan ";
                        else if (date_mm.ToString() == "2")
                            strExam_month = "Feb ";
                        else if (date_mm.ToString() == "3")
                            strExam_month = "Mar ";
                        else if (date_mm.ToString() == "4")
                            strExam_month = "Apr ";
                        else if (date_mm.ToString() == "5")
                            strExam_month = "Mar ";
                        else if (date_mm.ToString() == "6")
                            strExam_month = "Jun ";
                        else if (date_mm.ToString() == "7")
                            strExam_month = "Jul ";
                        else if (date_mm.ToString() == "8")
                            strExam_month = "Aug ";
                        else if (date_mm.ToString() == "9")
                            strExam_month = "Sep ";
                        else if (date_mm.ToString() == "10")
                            strExam_month = "Oct ";
                        else if (date_mm.ToString() == "11")
                            strExam_month = "Nov ";
                        else if (date_mm.ToString() == "12")
                            strExam_month = "Dec ";
                    }
                    if (strExam_month != "")
                    {
                        //Modified By Srinath 7/3/2013
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 10].Text = strExam_month.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 6].Text = strExam_month.ToString() + '-' + date_yr.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 6].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 6].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 6].Text = "";
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, totalsubcount + 6, 1, 2);
                    }
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 6, 1, totalsubcount + 2);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Month & Year Of Exam";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Month & Year Of Exam: " + strExam_month.ToString() + '-' + date_yr.ToString();
                    if (date_yr.ToString() != "")
                    {
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 11].Text = date_yr.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Text = date_yr.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, totalsubcount + 7].Text = "";
                    }

                    if (totalsubcount > 6)
                    {
                        spancnt = 2 + (totalsubcount - 6);
                    }
                    else
                    {
                        spancnt = 2;
                    }

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2, 1, spancnt);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Text = " Test Name : " + fd; //"SECTION : " + sections; modified on 28.06.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;


                    // }

                    string h = "";

                    FpEntry.Sheets[0].RowCount++;
                    int sl_no = 1;
                    if (nn == 0)
                    {

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "S.No";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Subject Code";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, uo - 1);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = "Subject Name";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                        int stcolcnt = 0;
                        if (totalsubcount > 6)
                        {
                            spancnt = 7 + (totalsubcount - 6);
                            stcolcnt = 11;
                        }
                        else
                        {
                            spancnt = 7;
                            stcolcnt = 10;
                        }


                        stcolcnt = uo + 2;
                        spancnt = (FpEntry.Sheets[0].ColumnCount - (uo + 2));
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, uo + 2, 1, (FpEntry.Sheets[0].ColumnCount - (uo + 2)));
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Text = "Faculty Name";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Font.Size = FontUnit.Medium;
                        //======================================================
                        if (0 < ds2.Tables[1].Rows.Count)
                        {
                            for (int po = 0; po < ds2.Tables[1].Rows.Count; po++)
                            {
                                FpEntry.Sheets[0].RowCount++;

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds2.Tables[1].Rows[po]["subject_code"].ToString();/// +"-" + ds2.Tables[1].Rows[po]["subject_name"].ToString();
                                //added on 18.07.12                                                                                                                  
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, uo - 1);
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = ds2.Tables[1].Rows[po]["subject_name"].ToString();
                                //================

                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, stcolcnt, 1, spancnt);
                                string ioo = ds2.Tables[1].Rows[po]["staff_code"].ToString();
                                staff_name = GetFunction("select staff_name from staffmaster where staff_code = '" + ioo + "'");
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Text = Convert.ToString(staff_name);


                                sl_no++;
                            }
                        }
                    }
                    else
                    {
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "S.No";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Subject Code";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, uo - 1);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = "Subject Name";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                        int stcolcnt = uo + 2;
                        spancnt = (FpEntry.Sheets[0].ColumnCount - (uo + 2));

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, stcolcnt, 1, spancnt);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Text = "Faculty Name";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Font.Size = FontUnit.Medium;
                        if (0 < ds2.Tables[1].Rows.Count)
                        {
                            for (int po = 0; po < ds2.Tables[1].Rows.Count; po++)
                            {
                                FpEntry.Sheets[0].RowCount++;

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds2.Tables[1].Rows[po]["subject_code"].ToString();/// +"-" + ds2.Tables[1].Rows[po]["subject_name"].ToString();
                                //added on 18.07.12                                                                                                                  
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 3, 1, uo - 1);
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = ds2.Tables[1].Rows[po]["subject_name"].ToString();
                                //================
                                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, stcolcnt, 1, spancnt);
                                string ioo = ds2.Tables[1].Rows[po]["staff_code"].ToString();
                                staff_name = GetFunction("select staff_name from staffmaster where staff_code = '" + ioo + "'");
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, stcolcnt].Text = staff_name;

                                sl_no++;
                            }
                        }
                    }
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                }

                FpEntry.Sheets[0].RowCount += 3;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 0, 1, FpEntry.Sheets[0].ColumnCount);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Text = "ASSESSMENT MARK STATEMENT";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 0, 1, FpEntry.Sheets[0].ColumnCount);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.Black;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Border.BorderColorLeft = Color.Black;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;

                FpEntry.Sheets[0].RowCount += 3;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 5, 1, ds2.Tables[1].Rows.Count);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5].Text = "Marks Obtained(Max:100 Marks)";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 5, 1, ds2.Tables[1].Rows.Count);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5].Text = "Subject";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Center;


                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 0, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Text = "S.No";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].VerticalAlign = VerticalAlign.Middle;

                //@@@@@@@@@@@@@@@@@ added on 17.07.12 @@@@@@@@@@@@@@@
                if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1")
                {
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 3, 1);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "Reg. No";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;


                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 2, 3, 1);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 2].Text = "Roll No";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 2].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 2].VerticalAlign = VerticalAlign.Middle;

                    FpEntry.Sheets[0].Columns[1].Width = 90;
                    FpEntry.Sheets[0].Columns[2].Width = 90;
                }
                else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
                {
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 3, 2);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "Reg. No";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Columns[1].Width = 80;
                    FpEntry.Sheets[0].Columns[2].Width = 10;
                }
                else if (Session["Rollflag"].ToString() == "1")
                {
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 3, 2);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "Roll No";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Columns[1].Width = 80;
                    FpEntry.Sheets[0].Columns[2].Width = 10;
                }
                else if (Session["Regflag"].ToString() == "1")
                {
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 3, 2);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "Reg. No";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Columns[1].Width = 80;
                    FpEntry.Sheets[0].Columns[2].Width = 10;
                }




                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 3, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 3].Text = "Name of the Student"; //"Student Name"; modified
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 3].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 3].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Columns[3].Width = 150;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 4, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 4].Text = "Student Type";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 4].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 4].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;


                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "Total Marks";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 5].Width = 60;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4].Text = "Average %";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 4].Width = 60;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3].Text = ddlTest.SelectedItem.Text + " Status";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 3].Width = 80;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2].Text = "No of Subjects Absent In This Assessment"; // "No of Subjects Absent in This Assesment"; modified
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 2].Width = 70;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1, 3, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = "No of Subjects Failed In This Assessment";//"No of Subjects Fail in This Assesment";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 70;

                int re = 5;

                for (int y1 = 0; y1 < ds2.Tables[1].Rows.Count; y1++)
                {
                    //@@@@@@@@@ changed subcode clmn heading as that serial no@@@@@@@@@@
                    // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].Text = ds2.Tables[1].Rows[y1]["subject_code"].ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].Text = (y1 + 1).ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].Note = ds2.Tables[1].Rows[y1]["subject_no"].ToString(); //set the note on 23.07.12
                    FpEntry.Sheets[0].Columns[re].Width = 20;
                    //@@@@@@@
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, re].VerticalAlign = VerticalAlign.Middle;
                    re++;
                    Session["subjcode_note_row"] = FpEntry.Sheets[0].RowCount - 1;
                }

                rwcnt = FpEntry.Sheets[0].RowCount;
                int sl_no1 = 1;

                if (ds2.Tables[0].Rows.Count != 0)
                {
                    filteration();

                    string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "' and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                    string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + degreecode.ToString() + "' and r.batch_year='" + batch.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0 and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";

                    hat.Clear();
                    hat.Add("bath_year", batch.ToString());
                    hat.Add("degree_code", degreecode.ToString());
                    hat.Add("sec", strsec.ToString());
                    hat.Add("filterwithsectionsub", filterwithsectionsub.ToString());
                    hat.Add("filterwithoutsectionsub", filterwithoutsectionsub.ToString());
                    ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");

                    totalregstudent = Convert.ToString(ds5.Tables[0].Rows.Count);

                    if (ds5.Tables[0].Rows.Count != 0)
                    {
                        if (ds5.Tables[0].Rows.Count > 0)
                        {
                            for (int irow = 0; irow < ds5.Tables[0].Rows.Count; irow++)
                            {
                                serialno++;
                                FpEntry.Sheets[0].RowCount++;
                                FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].CellType = tt;
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sl_no1.ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1")
                                {
                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = ds5.Tables[0].Rows[irow]["RollNumber"].ToString();
                                }
                                else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                                }
                                else if (Session["Rollflag"].ToString() == "1")
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds5.Tables[0].Rows[irow]["RollNumber"].ToString();
                                }
                                else if (Session["Regflag"].ToString() == "1")
                                {
                                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 2);
                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = ds5.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                                }
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = ds5.Tables[0].Rows[irow]["Student_Name"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = ds5.Tables[0].Rows[irow]["StudentType"].ToString();

                                FpEntry.Sheets[0].ColumnHeader.Columns[1].Width = 80;
                                sl_no1++;
                            }
                        }

                        Session["rowcount"] = FpEntry.Sheets[0].RowCount;

                        if (Session["Studflag"].ToString() == "0")
                        {
                            FpEntry.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                        }


                    }
                    hasrow_count = hasrow_count + 1;
                    int u = 5;
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {

                        subno = ds2.Tables[1].Rows[i]["subject_no"].ToString();
                        subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        resminmrk = ds2.Tables[1].Rows[i]["min_mark"].ToString();
                        exam_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                        examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                        subname = ds2.Tables[1].Rows[i]["subject_name"].ToString();
                        mark_new = Convert.ToInt32(ds2.Tables[0].Rows[i]["mark"]);


                    }
                    //++++++++++
                    double tot = 0;
                    double ag = 0, avg = 0;

                    int t = 0;
                    int yh = rwcnt;
                    int fail_cnt = 0;
                    int fail_sub_cnt = 0;
                    int absent_cnt = 0;
                    string fg = "";
                    int subrow = 0;
                    string rolnosubno = "";
                    string get_subcode_note = "";
                    string tmproll = "";
                    Boolean register = false;
                    stud_count = Convert.ToInt16(ds5.Tables[0].Rows.Count);
                    int cnt = ds2.Tables[0].Rows.Count;
                    //modified by gowtham
                    //------------ start ----------------

                    for (irow1 = 0; irow1 < ds5.Tables[0].Rows.Count; irow1++)
                    {
                        avgcnt = 0;
                        isabsent = false;
                        if (yh < ds5.Tables[0].Rows.Count + rwcnt)
                        {
                            DataView dv_indstudmarks = new DataView();
                            for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                            {
                                y = "";
                                if ((tmproll != "") && (tmproll.ToString() != ds5.Tables[0].Rows[irow1]["RollNumber"].ToString())) // ds2.Tables[0].Rows[i1]["roll"].ToString()
                                {
                                    if (register == true)
                                    {
                                        register_count = register_count + 1;
                                        register = false;
                                    }
                                    tmproll = ds5.Tables[0].Rows[irow1]["RollNumber"].ToString();
                                }

                                if (tmproll == "")
                                    tmproll = ds5.Tables[0].Rows[irow1]["RollNumber"].ToString();

                                ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds5.Tables[0].Rows[irow1]["RollNumber"].ToString() + "' and subject_no='" + ds2.Tables[1].Rows[i1]["subject_no"].ToString() + "' and exam_code='" + ds2.Tables[1].Rows[i1]["exam_code"].ToString() + "'";
                                dv_indstudmarks = ds2.Tables[0].DefaultView;
                                if (dv_indstudmarks.Count > 0)
                                {
                                    for (int studcnt = 0; studcnt < dv_indstudmarks.Count; studcnt++)
                                    {

                                        if (dv_indstudmarks[studcnt]["mark"].ToString().Trim() == "" || dv_indstudmarks[studcnt]["mark"].ToString().Trim() == "\0")
                                        {
                                            y = "";
                                        }
                                        else
                                        {
                                            y = dv_indstudmarks[studcnt]["mark"].ToString();
                                        }
                                        if (y != "")
                                        {


                                            if (50 <= Convert.ToDouble(y))
                                            {
                                                ug = ug + 1;
                                            }



                                            if (Convert.ToDouble(y) == -1)
                                            {

                                                y = "AB";
                                            }
                                            else if (Convert.ToDouble(y) == -2)
                                            {
                                                y = "EL";
                                            }
                                            else if (Convert.ToDouble(y) == -3)
                                            {
                                                y = "EOD";
                                            }
                                            else if (Convert.ToDouble(y) == -4)
                                            {
                                                y = "ML";
                                            }
                                            else if (Convert.ToDouble(y) == -5)
                                            {
                                                y = "SOD";
                                            }
                                            else if (Convert.ToDouble(y) == -6)
                                            {
                                                y = "NSS";
                                            }
                                            else if (Convert.ToDouble(y) == -7)
                                            {
                                                y = "NJ";
                                            }
                                            else if (Convert.ToDouble(y) == -8)
                                            {
                                                y = "S";
                                            }
                                            else if (Convert.ToDouble(y) == -9)
                                            {
                                                y = "L";
                                            }
                                            else if (Convert.ToDouble(y) == -10)
                                            {
                                                y = "NCC";
                                            }
                                            else if (Convert.ToDouble(y) == -11)
                                            {
                                                y = "HS";
                                            }
                                            else if (Convert.ToDouble(y) == -12)
                                            {
                                                y = "PP";
                                            }
                                            else if (Convert.ToDouble(y) == -13)
                                            {
                                                y = "SYOD";
                                            }
                                            else if (Convert.ToDouble(y) == -14)
                                            {
                                                y = "COD";
                                            }

                                            else if (Convert.ToDouble(y) == -15)
                                            {
                                                y = "OOD";
                                            }

                                            else if (Convert.ToDouble(y) == -16)
                                            {
                                                y = "OD";
                                            }

                                            else if (Convert.ToDouble(y) == -17)
                                            {
                                                y = "LA";
                                            }
                                            //******Subburaj 21.08.2014**********//
                                            else if (Convert.ToDouble(y) == -18)
                                            {
                                                y = "RAA";
                                            }
                                            //********End***********************//
                                            if ((y == "AB") && (register == false))
                                            {
                                                register = false;
                                            }
                                            else
                                            {
                                                register = true;
                                            }
                                            get_subcode_note = FpEntry.Sheets[0].Cells[Convert.ToInt16(Session["subjcode_note_row"]), i1 + 5].Note;

                                            if (get_subcode_note == dv_indstudmarks[studcnt]["subject_no"].ToString()) //added on 23.07.12
                                            {

                                                FpEntry.Sheets[0].Cells[yh, i1 + 5].Text = y.ToString();

                                                FpEntry.Sheets[0].Cells[yh, i1 + 5].HorizontalAlign = HorizontalAlign.Center;
                                                avgcnt = avgcnt + 1;
                                                if (y == "AB")
                                                {
                                                    FpEntry.Sheets[0].Cells[yh, i1 + 5].ForeColor = Color.Red;
                                                    FpEntry.Sheets[0].Cells[yh, i1 + 5].Font.Underline = true;
                                                    isabsent = true;
                                                    absent_cnt = absent_cnt + 1;//Added by Srinath 7/2/2013
                                                }
                                                if (y != "AB" && y != "AAA" && y != "EL" && y != "EOD" && y != "ML" && y != "SOD" && y != "NSS" && y != "NJ" && y != "S" && y != "L" && y != "NCC" && y != "HS" && y != "PP" && y != "SYOD" && y != "COD" && y != "OOD" && y != "OD" && y != "" && y != " " && y != "LA" && y != "" && y != " " && y != "RAA")//added on 23.07.12//Added By Subburaj 21.08.2014
                                                {
                                                    if (dv_indstudmarks[studcnt]["min_mark"].ToString().Trim() != "" && dv_indstudmarks[studcnt]["min_mark"] != null)
                                                    {
                                                        tot = tot + Convert.ToDouble(y);

                                                        if (Convert.ToDouble(y) < Convert.ToDouble(dv_indstudmarks[studcnt]["min_mark"]))
                                                        {

                                                            FpEntry.Sheets[0].Cells[yh, i1 + 5].ForeColor = Color.Red;
                                                            FpEntry.Sheets[0].Cells[yh, i1 + 5].Font.Underline = true;
                                                            fail_cnt = fail_cnt + 1;
                                                        }
                                                    }

                                                }
                                                t++;
                                            }

                                        }
                                        else
                                        {

                                            FpEntry.Sheets[0].Cells[yh, i1 + 5].Text = "-";
                                            FpEntry.Sheets[0].Cells[yh, i1 + 5].ForeColor = Color.Red;
                                        }
                                    }



                                }

                            }
                            if (ug == ds2.Tables[1].Rows.Count)
                            {
                                pg = pg + 1;
                            }
                            if (a2 != "")
                            {
                                b2 = b2 + 1;
                            }
                            if (fail_cnt == 0)//condn added on 17.07.12
                            {
                                //avg = tot / ds2.Tables[1].Rows.Count;

                                avg = tot / avgcnt;
                                ag = Math.Round(avg, 2);
                                if (tot == 0)
                                {
                                    FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].Text = "-";
                                    FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].Text = "-";
                                }
                                else
                                {
                                    if (isabsent == true)
                                    {
                                        FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].Text = "-";
                                        FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].Text = "-";
                                    }
                                    else
                                    {
                                        FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].Text = tot.ToString();
                                        FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].Text = ag.ToString();
                                    }
                                }

                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].Text = "-";
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].Text = "-";
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 2].Text = absent_cnt.ToString();
                            FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 1].Text = fail_cnt.ToString();
                            FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                            fail_cnt = 0;
                            absent_cnt = 0;

                            ug = 0;
                            a2 = "";
                            string dum_tage_date = "";
                            string dum_tage_hrs = "";


                            per_abshrs_spl = 0;
                            tot_per_hrs_spl = 0;
                            tot_ondu_spl = 0;
                            per_hhday_spl = 0;
                            unmark_spl = 0;
                            tot_conduct_hr_spl = 0;
                            per_workingdays1 = 0;
                            cum_per_workingdays1 = 0;


                            if (FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 5].Text == "-")
                            {

                                if (alarrrollnum.Contains(tmproll) == true)
                                {
                                    FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].BackColor = Color.Red;
                                }
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].Text = "FAIL";
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                if (alarrrollnum.Contains(tmproll) == true)
                                {
                                    FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].BackColor = Color.Red;
                                }
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].Text = "PASS";
                                FpEntry.Sheets[0].Cells[yh, FpEntry.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                            }




                            tot = 0;
                            yh++;


                        }
                    }

                    if (register == true)
                    {
                        register_count = register_count + 1;
                    }
                    t = 0;
                }



                //-------------end -------------------



                FpEntry.Sheets[0].RowCount++;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "SUBJECTWISE PERFORMANCE"; //"SUNJECTWISE PERFOMANCE"; modified
                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Height = 40;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                FpEntry.Sheets[0].RowCount += 8;  // FpEntry.Sheets[0].RowCount += 8; old modified on 28.06.12


                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, 0].Text = "1";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 8, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, 1].Text = "Total no.of students appeared for the subject"; //"Total No.of Students for the Subject"; modified

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 7, 0].Text = "2";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 7, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 7, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 7, 1].Text = "No.of students Passed";

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 6, 0].Text = "3";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 6, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 6, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 6, 1].Text = "No.of students Failed";

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Text = "4";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 1].Text = "No.of students Absent";




                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].Text = "5";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 4, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Text = "% Pass in each subject ";
                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 4].Visible = false;



                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Text = "6";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "Average mark ";


                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Text = "7";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 1].Text = "Standard deviation";

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "8";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Coefficient of variation (%)";

                int a = 5;
                double avg1;
                string exam_code_value = "";

                //START COPY
                int hidecolcont = 0;


                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                {
                    hat.Clear();
                    string secss = "";
                    if (ddlSec.Enabled == false) // added by sridhar aug 2014
                    {
                        secss = "";
                    }
                    else
                    {
                        secss = ddlSec.SelectedItem.Text.ToString();
                    }


                    if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                    {
                        secss = ""; // added by sridhar aug 2014
                    }
                    else
                    {
                        secss = ddlSec.SelectedItem.Text; // added by sridhar aug 2014
                    }
                    hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                    hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                    hat.Add("section", secss);
                    ds4 = d2.select_method("Proc_All_Subject_Details", hat, "sp");
                    hidecolcont++;
                    if (exam_code_value == "")
                    {
                        exam_code_value = Convert.ToString(ds2.Tables[1].Rows[i]["exam_code"]);
                    }
                    else
                    {
                        exam_code_value = exam_code_value + "," + Convert.ToString(ds2.Tables[1].Rows[i]["exam_code"]);
                    }

                    if (ds4.Tables.Count != 0)
                    {

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 6, a].Text = ds4.Tables[2].Rows[0]["FAIL_COUNT"].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 6, a].HorizontalAlign = HorizontalAlign.Center;

                        float yy = float.Parse(ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString());

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 7, a].Text = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 7, a].HorizontalAlign = HorizontalAlign.Center;


                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, a].Text = ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, a].HorizontalAlign = HorizontalAlign.Center;
                        float yy1 = float.Parse(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString());


                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].Text = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].HorizontalAlign = HorizontalAlign.Center;
                        float yy2 = float.Parse(ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString());

                        float totfi = yy1 + yy2;
                        //avg1 = (yy * 100) / yy1;
                        avg1 = (yy * 100) / totfi;
                        double avg2 = Math.Round(avg1, 2);
                        if (Convert.ToString(avg2) == "NaN")
                        {
                            avg2 = 0;
                        }

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].Text = avg2.ToString();//pass% based on no of stud appeared for the exam
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].HorizontalAlign = HorizontalAlign.Center;



                        int degreecount1;
                        int yq;

                        int ui = 5;
                        double s = 0;
                        int uop = 5;


                        //avg mark-------------------------------------------------------------------------
                        degreecount1 = sl_no1 - 1;
                        yq = rwcnt;
                        s = 0;
                        for (int fintot = 0; fintot < degreecount1; fintot++)
                        {
                            string a1 = FpEntry.Sheets[0].Cells[yq, a].Text;
                            if (a1 != "AB" && a1 != "AAA" && a1 != "EL" && a1 != "EOD" && a1 != "ML" && a1 != "SOD" && a1 != "NSS" && a1 != "NJ" && a1 != "S" && a1 != "L" && a1 != "NCC" && a1 != "HS" && a1 != "PP" && a1 != "SYOD" && a1 != "COD" && a1 != "OOD" && a1 != "OD" && a1 != "" && a1 != " " && a1 != "LA" && a1 != "" && a1 != " " && a1 != "RAA")//Added By subburaj21.08.2014
                            {
                                s = s + Convert.ToDouble(a1);
                            }
                            yq++;
                        }

                        double hu = s / (Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString()));
                        double dds = Math.Round(hu, 2);
                        if (Convert.ToString(dds) == "NaN")
                        {
                            dds = 0;
                        }
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].Text = Convert.ToString(dds);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].HorizontalAlign = HorizontalAlign.Center;
                        //---------------------------------------------------------------------------------   


                        // standard deviation for a subject--------------------------------------------------------------------------------    
                        double add_allmark_1subj = 0;
                        int degreecount2 = sl_no1 - 1;
                        int yq1 = rwcnt;
                        double deviation = 0;
                        for (int fintot = 0; fintot < degreecount1; fintot++)
                        {
                            string a1 = FpEntry.Sheets[0].Cells[yq1, a].Text;
                            if (a1 != "AB" && a1 != "AAA" && a1 != "EL" && a1 != "EOD" && a1 != "ML" && a1 != "SOD" && a1 != "NSS" && a1 != "NJ" && a1 != "S" && a1 != "L" && a1 != "NCC" && a1 != "HS" && a1 != "PP" && a1 != "SYOD" && a1 != "COD" && a1 != "OOD" && a1 != "OD" && a1 != "-" && a1 != "" && a1 != " " && a1 != "LA" && a1 != "" && a1 != " " && a1 != "RAA") //Modified By Subburaj 21.08.2014
                            {
                                add_allmark_1subj = add_allmark_1subj + ((Convert.ToDouble(a1) - dds) * (Convert.ToDouble(a1) - dds));

                            }
                            yq1++;
                        }

                        deviation = Math.Round(Math.Sqrt(((Convert.ToDouble(add_allmark_1subj)) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString()))), 2);///sqrt( summation(x-x`)2/(n-1))
                        if (Convert.ToString(deviation) == "NaN")
                        {
                            deviation = 0;
                        }

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].Text = deviation.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].HorizontalAlign = HorizontalAlign.Center;

                        //---co efficient of variation -----
                        if (deviation == 0)
                        {
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, a].Text = "0";
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, a].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, a].Text = Math.Round(((deviation / Convert.ToDouble(dds)) * 100), 2).ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, a].HorizontalAlign = HorizontalAlign.Center;

                        }
                        //-----------------------------------

                        //---------------------------------------------------------------------------------------------------                     
                        double passperc_outof_totalstud = 0;

                        passperc_outof_totalstud = ((yy * 100) / (Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString()) + Convert.ToDouble(ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString())));
                        passperc_outof_totalstud = Math.Round(passperc_outof_totalstud, 2);
                        if (Convert.ToString(passperc_outof_totalstud) == "NaN")
                        {
                            passperc_outof_totalstud = 0;
                        }

                        a++;
                        avg1 = 0;
                    }
                    rows_count++;
                }


                //added by aruna 23oct2012=======================================
                if (exam_code_value != "")
                {
                    exam_code_value = "in(" + exam_code_value + ")";
                }
                int allpascnt = 0;
                string ssd = "";
                string str_section = "";
                int test_minmark = 0;
                int fail_stud_atleast_one = 0;
                int stud_appear = 0;
                if (strsec != "")
                    str_section = " and sections='" + strsec + "'";


                ssd = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + "";
                allpascnt = int.Parse(GetFunction(ssd));
                ssd = "select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " ";
                stud_appear = int.Parse(GetFunction(ssd));
                test_minmark = Convert.ToInt32(GetFunction("select min_mark from criteriaforinternal where criteria_no=" + ddlTest.SelectedValue.ToString() + ""));
                ssd = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_Code " + exam_code_value.ToString() + " and rt.roll_no=r.roll_no and r.degree_code=" + ddlBranch.SelectedValue.ToString() + " and r.batch_year=" + ddlBatch.SelectedItem.ToString() + "  " + str_section + " and (rt.marks_obtained<" + test_minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0  ";



                fail_stud_atleast_one = int.Parse(GetFunction(ssd));
                double allpassperc = 0;
                allpassperc = (Convert.ToDouble((allpascnt - fail_stud_atleast_one) / Convert.ToDouble(stud_appear))) * 100;
                allpassperc = Math.Round(allpassperc, 2);
                //===============================================================

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5, 3, 4);
                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No Of Students Passed All Subject";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS"; //"No Of Students Passed All Subject";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;



                //DataSet dsregist = d2.select_method("select count( distinct roll_no) as registr from result where marks_obtained<>'-1' and exam_code in (select distinct exam_code from exam_type where criteria_no=" + criteria_no + " " + str_sec + ")", hat, "Text");
                // DataSet dsregist = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + "", hat, "Text");
                DataSet dsregist = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from registration rt where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + "", hat, "Text");
                string registrer = "";
                if (dsregist.Tables[0].Rows.Count > 0)
                {
                    registrer = dsregist.Tables[0].Rows[0]["registr"].ToString();
                }
                //====================================================================

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1, 3, 1);
                //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); //pg.ToString();
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].Text = registrer.ToString(); //pg.ToString();
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 8, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;



                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5, 3, 4);
                //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS "; //"No of Students Apperaed"; modified on 28.06.12
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS "; //"No of Students Apperaed"; modified on 28.06.12
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;
                //added by srinath 7/2/2013==========================================


                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1, 3, 1);

                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = registrer.ToString(); // register_count.ToString();
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); // register_count.ToString();
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                double perc = 0, mm = 0;
                perc = (Convert.ToDouble(pg) * 100 / Convert.ToDouble(register_count));//ds5.Tables[0].Rows.Count;
                mm = Math.Round(perc, 0);

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5].Text = "ALL PASS IN  % ";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;


                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Text = allpassperc.ToString(); // +"%"; 
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;




                if (rbselectionlist.SelectedValue == "1")
                {

                    FpEntry.Sheets[0].RowCount += 1;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "RESULT OF TARGETED STUDENTS";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);
                    FpEntry.Sheets[0].RowCount += 5;



                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Text = "1";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 1].Text = "No.of students Passed";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].Text = "2";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 4, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Text = "No.of students Failed";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Text = "3";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "No.of students Absent";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Text = "4";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 1].Text = "Subject Wise Pass %";

                    //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "5";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 4);
                    //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Targeted Student Pass %";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "";
                    a = 5;
                    int colc = 0;
                    int spancolum = FpEntry.Sheets[0].RowCount - 5;
                    if (gcount != "0")
                    {
                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                        {
                            colc++;
                            hat.Clear();
                            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("batchyear", ddlBatch.SelectedItem.Text);
                            hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
                            hat.Add("cnt", txtnofarreares.Text);
                            hat.Add("option", "");
                            hat.Add("Rollno", "");
                            hat.Add("@passcount", pass);
                            hat.Add("@failcount", fail);
                            hat.Add("@presentcount", present);
                            hat.Add("@absentcount", absent);
                            dstargetstudent = d2.select_method("proctarget_details", hat, "sp");


                            if (dstargetstudent.Tables.Count != 0)
                            {

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].Text = dstargetstudent.Tables[1].Rows[0]["FAIL_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].HorizontalAlign = HorizontalAlign.Center;

                                float yy = float.Parse(dstargetstudent.Tables[0].Rows[0]["PASS_COUNT"].ToString());

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].Text = dstargetstudent.Tables[0].Rows[0]["PASS_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].HorizontalAlign = HorizontalAlign.Center;


                                float yy1 = float.Parse(dstargetstudent.Tables[2].Rows[0]["PRESENT_COUNT"].ToString());


                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].Text = dstargetstudent.Tables[3].Rows[0]["ABSENT_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].HorizontalAlign = HorizontalAlign.Center;
                                float yy2 = float.Parse(dstargetstudent.Tables[2].Rows[0]["PRESENT_COUNT"].ToString());

                                float t = yy1 + yy2;
                                avg1 = (yy * 100) / t;
                                // avg1 = (yy * 100) / yy1;
                                double avg2 = Math.Round(avg1, 2);
                                if (Convert.ToString(avg2) == "NaN")
                                {
                                    avg2 = 0;
                                }

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].Text = avg2.ToString();//pass% based on no of stud appeared for the exam
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].HorizontalAlign = HorizontalAlign.Center;
                                a++;

                            }
                        }

                        ssd = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " and r.roll_no not in(select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "') ";
                        allpascnt = int.Parse(GetFunction(ssd));
                        ssd = "select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " and r.roll_no not in(select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "')  ";
                        stud_appear = int.Parse(GetFunction(ssd));
                        test_minmark = Convert.ToInt32(GetFunction("select min_mark from criteriaforinternal where criteria_no=" + ddlTest.SelectedValue.ToString() + ""));
                        ssd = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_Code " + exam_code_value.ToString() + " and rt.roll_no=r.roll_no and r.degree_code=" + ddlBranch.SelectedValue.ToString() + " and r.batch_year=" + ddlBatch.SelectedItem.ToString() + "  " + str_section + " and (rt.marks_obtained<" + test_minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 and r.roll_no not in(select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "')  ";

                        fail_stud_atleast_one = int.Parse(GetFunction(ssd));
                        double allpassperc1 = 0;
                        allpassperc1 = (Convert.ToDouble((allpascnt - fail_stud_atleast_one) / Convert.ToDouble(stud_appear))) * 100;
                        allpassperc1 = Math.Round(allpassperc1, 2);

                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = allpassperc1.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = "";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, colc + 5);


                        //========================

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No Of Students Passed All Subject";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS"; //"No Of Students Passed All Subject";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;

                        //DataSet dsregist = d2.select_method("select count( distinct roll_no) as registr from result where marks_obtained<>'-1' and exam_code in (select distinct exam_code from exam_type where criteria_no=" + criteria_no + " " + str_sec + ")", hat, "Text");
                        DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from registration rt where batch_year='" + ddlBatch.SelectedItem.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + " and rt.roll_no not in(select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "') ", hat, "Text");
                        string registrerr = "";
                        if (dsregistr.Tables[0].Rows.Count > 0)
                        {
                            registrerr = dsregistr.Tables[0].Rows[0]["registr"].ToString();
                        }
                        //====================================================================


                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); //pg.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); //pg.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;



                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS "; //"No of Students Apperaed"; modified on 28.06.12
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = " NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No of Students Apperaed"; modified on 28.06.12
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;
                        //added by srinath 7/2/2013==========================================


                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);

                        //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); // register_count.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); // register_count.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                        double percs = 0, mms = 0;
                        percs = (Convert.ToDouble(pg) * 100 / Convert.ToDouble(register_count));//ds5.Tables[0].Rows.Count;
                        mms = Math.Round(perc, 0);

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5, 1, 4);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Text = "ALL PASS IN  % ";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;


                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = allpassperc1.ToString(); // +"%"; 
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                        //========================


                    }

                    else
                    {

                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                        {
                            colc++;
                            hat.Clear();
                            ds4.Clear();
                            string secss = "";
                            if (ddlSec.Enabled == false) // added by sridhar aug 2014
                            {
                                secss = "";
                            }
                            else
                            {
                                secss = ddlSec.SelectedItem.Text.ToString();
                            }

                            if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                            {
                                secss = ""; // added by sridhar aug 2014
                            }
                            else
                            {
                                secss = ddlSec.SelectedItem.Text; // added by sridhar aug 2014
                            }

                            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("section", secss);
                            ds4 = d2.select_method("Proc_All_Subject_Details", hat, "sp");
                            if (ds4.Tables.Count != 0)
                            {

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].Text = ds4.Tables[2].Rows[0]["FAIL_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].HorizontalAlign = HorizontalAlign.Center;

                                float yy = float.Parse(ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString());

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].Text = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].HorizontalAlign = HorizontalAlign.Center;


                                float yy1 = float.Parse(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString());

                                avg1 = (yy * 100) / yy1;
                                double avg2 = Math.Round(avg1, 2);
                                if (Convert.ToString(avg2) == "NaN")
                                {
                                    avg2 = 0;
                                }
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].Text = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].HorizontalAlign = HorizontalAlign.Center;

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].Text = avg2.ToString();//pass% based on no of stud appeared for the exam
                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].HorizontalAlign = HorizontalAlign.Center;
                                a++;

                            }
                        }

                        ssd = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + "";
                        allpascnt = int.Parse(GetFunction(ssd));
                        ssd = "select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " ";
                        stud_appear = int.Parse(GetFunction(ssd));
                        test_minmark = Convert.ToInt32(GetFunction("select min_mark from criteriaforinternal where criteria_no=" + ddlTest.SelectedValue.ToString() + ""));
                        ssd = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_Code " + exam_code_value.ToString() + " and rt.roll_no=r.roll_no and r.degree_code=" + ddlBranch.SelectedValue.ToString() + " and r.batch_year=" + ddlBatch.SelectedItem.ToString() + "  " + str_section + " and (rt.marks_obtained<" + test_minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0  ";

                        fail_stud_atleast_one = int.Parse(GetFunction(ssd));
                        double allpassperc1 = 0;
                        allpassperc1 = (Convert.ToDouble((allpascnt - fail_stud_atleast_one) / Convert.ToDouble(stud_appear))) * 100;
                        allpassperc1 = Math.Round(allpassperc, 2);

                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = allpassperc1.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 5, 1, colc);



                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                        //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No Of Students Passed All Subject";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS"; //"No Of Students Passed All Subject";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;

                        //DataSet dsregist = d2.select_method("select count( distinct roll_no) as registr from result where marks_obtained<>'-1' and exam_code in (select distinct exam_code from exam_type where criteria_no=" + criteria_no + " " + str_sec + ")", hat, "Text");
                        //DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + "", hat, "Text");
                        DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from registration rt where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + "", hat, "Text");
                        string registrerr = "";
                        if (dsregistr.Tables[0].Rows.Count > 0)
                        {
                            registrerr = dsregistr.Tables[0].Rows[0]["registr"].ToString();
                        }
                        //====================================================================

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);
                        //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); //pg.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); //pg.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;



                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS "; //"No of Students Apperaed"; modified on 28.06.12
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = " NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No of Students Apperaed"; modified on 28.06.12
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;
                        //added by srinath 7/2/2013==========================================


                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);

                        // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); // register_count.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); // register_count.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                        double percs = 0, mms = 0;
                        percs = (Convert.ToDouble(pg) * 100 / Convert.ToDouble(register_count));//ds5.Tables[0].Rows.Count;
                        mms = Math.Round(perc, 0);

                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5, 1, 4);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Text = "ALL PASS IN  % ";
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;


                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = allpassperc1.ToString(); // +"%"; 
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                        //========================







                    }
                }

                else if (rbselectionlist.SelectedValue == "2")
                {
                    string rollnum = "";
                    for (int cn = 0; cn < alarrrollnum.Count; cn++)
                    {
                        if (rollnum == "")
                        {
                            rollnum = "'" + alarrrollnum[cn].ToString() + "'";
                        }
                        else
                        {
                            rollnum = rollnum + ',' + "'" + alarrrollnum[cn].ToString() + "'";
                        }
                    }

                    FpEntry.Sheets[0].RowCount += 1;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "RESULT OF TARGETED STUDENTS";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);
                    FpEntry.Sheets[0].RowCount += 5;



                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Text = "1";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 1].Text = "No.of students Passed";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].Text = "2";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 4, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Text = "No.of students Failed";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Text = "3";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "No.of students Absent";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Text = "4";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 1, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 1].Text = "Subject Wise Pass %";

                    // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "5";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = "";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 4);
                    // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Targeted Student Pass %";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "";
                    a = 5;
                    int colc = 0;
                    int spancolum = FpEntry.Sheets[0].RowCount - 5;

                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        colc++;
                        hat.Clear();
                        hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                        hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                        hat.Add("batchyear", ddlBatch.SelectedItem.Text);
                        hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
                        hat.Add("cnt", txtnofarreares.Text);
                        hat.Add("option", "manual");
                        hat.Add("Rollno", rollnum);
                        string pass = "r.exam_code='" + ds2.Tables[1].Rows[i]["exam_code"].ToString() + "' and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained>='" + ds2.Tables[1].Rows[i]["min_mark"].ToString() + "' or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'and r.roll_no not in(" + rollnum + ") ";
                        string fail = "r.exam_code='" + ds2.Tables[1].Rows[i]["exam_code"].ToString() + "' and ex.exam_code=r.exam_code and r.roll_no=su.roll_no and ex.subject_no=su.subject_no and (r.marks_obtained<'" + ds2.Tables[1].Rows[i]["min_mark"].ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1')and r.roll_no not in(" + rollnum + ") ";
                        string present = "r.exam_code='" + ds2.Tables[1].Rows[i]["exam_code"].ToString() + "' and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and  marks_obtained<>'-1'  and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and r.roll_no not in(" + rollnum + ")";
                        string absent = "r.exam_code='" + ds2.Tables[1].Rows[i]["exam_code"].ToString() + "' and r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3'and r.marks_obtained<>'-7' )and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and r.roll_no not in(" + rollnum + ")";
                        hat.Add("@passcount", pass);
                        hat.Add("@failcount", fail);
                        hat.Add("@presentcount", present);
                        hat.Add("@absentcount", absent);
                        dstargetstudent = d2.select_method("proctarget_details", hat, "sp");


                        if (dstargetstudent.Tables.Count != 0)
                        {

                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].Text = dstargetstudent.Tables[1].Rows[0]["FAIL_COUNT"].ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, a].HorizontalAlign = HorizontalAlign.Center;

                            float yy = float.Parse(dstargetstudent.Tables[0].Rows[0]["PASS_COUNT"].ToString());

                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].Text = dstargetstudent.Tables[0].Rows[0]["PASS_COUNT"].ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, a].HorizontalAlign = HorizontalAlign.Center;


                            float yy1 = float.Parse(dstargetstudent.Tables[2].Rows[0]["PRESENT_COUNT"].ToString());

                            avg1 = (yy * 100) / yy1;
                            double avg2 = Math.Round(avg1, 2);
                            if (Convert.ToString(avg2) == "NaN")
                            {
                                avg2 = 0;
                            }
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].Text = dstargetstudent.Tables[3].Rows[0]["ABSENT_COUNT"].ToString();
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, a].HorizontalAlign = HorizontalAlign.Center;

                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].Text = avg2.ToString();//pass% based on no of stud appeared for the exam
                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, a].HorizontalAlign = HorizontalAlign.Center;
                            a++;

                        }
                    }


                    ssd = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " and r.roll_no not in(" + rollnum + ") ";
                    allpascnt = int.Parse(GetFunction(ssd));
                    ssd = "select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_section + " and r.roll_no not in(" + rollnum + ")  ";
                    stud_appear = int.Parse(GetFunction(ssd));
                    test_minmark = Convert.ToInt32(GetFunction("select min_mark from criteriaforinternal where criteria_no=" + ddlTest.SelectedValue.ToString() + ""));
                    ssd = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_Code " + exam_code_value.ToString() + " and rt.roll_no=r.roll_no and r.degree_code=" + ddlBranch.SelectedValue.ToString() + " and r.batch_year=" + ddlBatch.SelectedItem.ToString() + "  " + str_section + " and (rt.marks_obtained<" + test_minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 and r.roll_no not in(" + rollnum + ")  ";

                    fail_stud_atleast_one = int.Parse(GetFunction(ssd));
                    double allpassperc1 = 0;
                    allpassperc1 = (Convert.ToDouble((allpascnt - fail_stud_atleast_one) / Convert.ToDouble(stud_appear))) * 100;
                    allpassperc1 = Math.Round(allpassperc1, 2);

                    //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = allpassperc1.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = "";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 5, 1, colc);
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, colc + 2);


                    //========================

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS"; //"No Of Students Passed All Subject";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS"; //"No Of Students Passed All Subject";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;

                    //DataSet dsregist = d2.select_method("select count( distinct roll_no) as registr from result where marks_obtained<>'-1' and exam_code in (select distinct exam_code from exam_type where criteria_no=" + criteria_no + " " + str_sec + ")", hat, "Text");
                    //
                    //  DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from result r,registration rt where r.exam_code " + exam_code_value.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + " and r.roll_no not in(" + rollnum + ") ", hat, "Text");
                    //DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from registration rt where batch_year='" + ddlBatch.SelectedItem.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + " and rt.roll_no not in(select roll_no from (select distinct count(distinct mark_entry.subject_no) as cnt,mark_entry.roll_no from  Mark_Entry ,Subject,sub_sem,exam_details ex,registration r where mark_entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and mark_entry.Exam_Code = ex.exam_code and ex.degree_code=r.degree_code and ex.batch_year=r.batch_year and r.batch_year='" + ddlBatch.SelectedItem.Text + "' and r.degree_code='" + ddlBranch.SelectedValue + "'  and mark_entry.Attempts =1 and mark_entry.result = 'Fail'   group by mark_entry.roll_no) a where cnt='" + txtnofarreares.Text + "') ", hat, "Text");
                    DataSet dsregistr = d2.select_method("select isnull(count(distinct rt.roll_no),0) as 'registr' from registration rt where batch_year='" + ddlBatch.SelectedItem.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + str_sec + " and rt.roll_no not in(" + rollnum + ") ", hat, "Text");

                    string registrerr = "";
                    if (dsregistr.Tables[0].Rows.Count > 0)
                    {
                        registrerr = dsregistr.Tables[0].Rows[0]["registr"].ToString();
                    }
                    //====================================================================
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);
                    //  FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); //pg.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); //pg.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;



                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "TOTAL NO.OF STUDENTS "; //"No of Students Apperaed"; modified on 28.06.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "NO.OF STUDENTS PASSED IN ALL SUBJECTS "; //"No of Students Apperaed"; modified on 28.06.12
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;
                    //added by srinath 7/2/2013==========================================


                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);

                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = registrerr.ToString(); // register_count.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = (allpascnt - fail_stud_atleast_one).ToString(); // register_count.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                    double percs = 0, mms = 0;
                    percs = (Convert.ToDouble(pg) * 100 / Convert.ToDouble(register_count));//ds5.Tables[0].Rows.Count;
                    mms = Math.Round(perc, 0);

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5, 1, 4);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Text = "ALL PASS IN  % ";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Left;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = 14;


                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = allpassperc1.ToString(); // +"%"; 
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = 14;

                    //========================



                }



                //FpEntry.Sheets[0].SpanModel.Add(spancolum, colc + 1, 5, FpEntry.Sheets[0].ColumnCount);


                FpEntry.Sheets[0].RowCount += 5;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 5, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
                if (ddlSec.Text != string.Empty)
                {
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Text = "Section " + ddlSec.SelectedItem.Text;
                }
                else
                {
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Text = "Section -";
                }

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 5, 0].Font.Size = FontUnit.Medium;


                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 4, 1, 1, FpEntry.Sheets[0].ColumnCount - 6);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, 1].Text = "FAILURE ANALYSIS (Excluding AB)";
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 1, 1, 3);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 1].Text = "No.of subjects failed :";
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 1, 1, 3);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 1].Text = "No.of students";
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 1, 1, 3);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = "Percentage %";


                //Added By aruna on 10Sep2012----Fail Count--------------------------------------------------------------------------------------
                string failstr = "";
                string getfailcnt = "";

                DataSet failds = new DataSet();

                for (int subcnt = 1; subcnt <= ds2.Tables[1].Rows.Count; subcnt++)
                {

                    failds.Clear();
                    failds.Reset();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5 + subcnt - 1].Text = subcnt.ToString();
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5 + subcnt - 1].Text = "-";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5 + subcnt - 1].Text = "-";

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5 + subcnt - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5 + subcnt - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 5 + subcnt - 1].Font.Size = FontUnit.Medium;

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5 + subcnt - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5 + subcnt - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5 + subcnt - 1].Font.Size = FontUnit.Medium;

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5 + subcnt - 1].Font.Bold = true;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5 + subcnt - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5 + subcnt - 1].Font.Size = FontUnit.Medium;

                    failstr = "select roll_no from result r,exam_type e where  batch_year=" + ddlBatch.SelectedValue.ToString() + " and criteria_no=" + ddlTest.SelectedValue.ToString() + "  " + str_sec + "  and r.exam_code=e.exam_code and marks_obtained < min_mark and (marks_obtained<>'-1' and marks_obtained<>'-2' and marks_obtained<>'-3')  group  by roll_no having count(roll_no)=" + subcnt + " ";
                    con_splhr_query_master.Close();
                    con_splhr_query_master.Open();
                    SqlCommand failcmd = new SqlCommand(failstr, con_splhr_query_master);
                    SqlDataAdapter filda = new SqlDataAdapter(failcmd);
                    filda.Fill(failds);
                    getfailcnt = failds.Tables[0].Rows.Count.ToString();
                    if ((getfailcnt.ToString() != "") && (getfailcnt.ToString() != "0"))
                    {
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 5 + subcnt - 1].Text = getfailcnt.ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5 + subcnt - 1].Text = Math.Round(((Convert.ToDouble(getfailcnt.ToString()) / Convert.ToDouble(ds5.Tables[0].Rows.Count)) * 100), 2).ToString();// +"%";

                    }
                }
                //---------------------------------------------------------------------------------------------------------------------

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 4, FpEntry.Sheets[0].ColumnCount - 5, 1, 5);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, FpEntry.Sheets[0].ColumnCount - 5].Text = "ABSENTEES IN EXAM";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, FpEntry.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 4, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5, 2, 4);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Text = "Students Absent for test in atleast one subject";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);


                //============find the student absent in atlest one course=========================
                string all_exm_code = "";
                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                {
                    if (all_exm_code == "")
                    {
                        all_exm_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                    }
                    else
                    {
                        all_exm_code = all_exm_code + "," + ds2.Tables[1].Rows[i]["exam_code"].ToString();
                    }

                }
                all_exm_code = "in (" + all_exm_code + ")";

                string find_ab_count = GetFunction("select count( distinct roll_no) from result where exam_code " + all_exm_code + " and marks_obtained=-1");
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Text = find_ab_count;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5, 1, 4);
                // FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Text = "In %";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Text = "Overall absent for test in atleast one subject in %";

                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;

                double ab_percent = (Convert.ToDouble(find_ab_count) / Convert.ToDouble(ds5.Tables[0].Rows.Count)) * 100;
                ab_percent = Math.Round(ab_percent, 2);
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = ab_percent.ToString();// +"%";
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                int ffd = FpEntry.Sheets[0].ColumnCount;
                hat.Clear();
                hat.Add("college_code", Session["InternalCollegeCode"].ToString());
                hat.Add("form_name", "Target_Report.aspx");
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

                    }
                    if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
                    {
                        address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();


                    }
                    if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
                    {
                        address3 = dsprint.Tables[0].Rows[0]["address3"].ToString();

                    }
                    if (dsprint.Tables[0].Rows[0]["district"].ToString() != "")
                    {
                        district = dsprint.Tables[0].Rows[0]["district"].ToString();

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
                    //-------newly added on 13.07.12
                    if (dsprint.Tables[0].Rows[0]["state"].ToString() != "")
                    {
                        state = dsprint.Tables[0].Rows[0]["state"].ToString();
                    }
                    if (dsprint.Tables[0].Rows[0]["pincode"].ToString() != "")
                    {
                        if (pincode == "0")
                        {
                            pincode = " ";
                        }
                        else
                        {
                            pincode = dsprint.Tables[0].Rows[0]["pincode"].ToString();
                        }
                        // state = state + "-" + pincode;
                    }

                    //---------------newly added
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
                    else
                    {
                        form_heading_name = " "; //for batch degree branch
                    }
                    if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
                    {
                        batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();//affliated

                    }
                    if (dsprint.Tables[0].Rows[0]["affliated"].ToString() != "")
                    {
                        affliated = dsprint.Tables[0].Rows[0]["affliated"].ToString();//affliated

                    }

                }

                else if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
                {
                    string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'-') as phoneno,category,affliatedby,isnull(faxno,'-') as faxno,district,email,website,pincode,state from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
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
                            address3 = collegename["address3"].ToString();
                            district = collegename["district"].ToString();
                            address = address1 + "-" + address2 + "-" + district;
                            Phoneno = collegename["phoneno"].ToString();
                            Faxno = collegename["faxno"].ToString();
                            phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                            email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                            pincode = collegename["pincode"].ToString();
                            state = collegename["state"].ToString();
                            category = collegename["category"].ToString();
                            affliated = collegename["affliatedby"].ToString();
                        }
                        affliated = category + "," + affliated;
                        batch_degree_branch = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + sections.ToString();
                    }
                    con.Close();
                }


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



                //------------------------spaning the 3 rows



                MyImg mi3 = new MyImg();
                mi3.ImageUrl = "Handler/Handler2.ashx?";
                MyImg mi4 = new MyImg();
                mi4.ImageUrl = "Handler/Handler5.ashx?";


                FpEntry.Sheets[0].ColumnHeader.Cells[5, resultcount].Border.BorderColorBottom = Color.LightGray;
                //if (spancount == 0)
                //{
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.LightGray;
                FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColor = Color.LightGray;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColor = Color.LightGray;

                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;

                FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;

                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.LightGray;
                FpEntry.Sheets[0].SheetCorner.Cells[1, 0].Border.BorderColorRight = Color.LightGray;
                FpEntry.Sheets[0].SheetCorner.Cells[5, 0].Border.BorderColorRight = Color.LightGray;
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


                FarPoint.Web.Spread.EmptyCellType et = new EmptyCellType();
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 2);

                //============================== set the left logo as per given condition 
                //if (dsprint.Tables[0].Rows.Count > 0)
                //{
                //    if (dsprint.Tables[0].Rows[0]["leftlogo"].ToString() == "1")
                //    {
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi3;
                //    }
                //}
                //====================
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.Black;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;

                func_header();
                //FpEntry.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                FpEntry.Sheets[0].ColumnHeader.Columns[3].Width = 250;

                FpEntry.Width = 1300;
                FpEntry.Sheets[0].ColumnHeader.Columns[0].Width = 40; //width from 150 to 50
                FpEntry.Sheets[0].ColumnHeader.Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 150;

                FpEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;

                FpEntry.Sheets[0].ColumnHeader.Cells[2, resultcount].Border.BorderColorBottom = Color.LightGray;
                FpEntry.Sheets[0].ColumnHeader.Rows[2].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Rows[3].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Rows[4].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Rows[5].Border.BorderColorTop = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColor = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorLeft = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorLeft = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;

                FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.White;


                //+========================== modified 0n 13.07.12
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 0].Text = " ";
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = " ";
                //FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = " ";
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, FpEntry.Sheets[0].ColumnCount - 1].Text = " ";
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
                // FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = "";
                //============================== set the right logo as per given condition 
                //if (dsprint.Tables[0].Rows.Count > 0)
                //{
                //    if (dsprint.Tables[0].Rows[0]["rightlogo"].ToString() == "1")
                //    {
                FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].CellType = mi4;
                //    }
                //}

                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 6, 1);//gowtham

                //============================================

                FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;

                FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;

                FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[1, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[2, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[3, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[4, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[5, 0].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].SheetCorner.Cells[6, 0].Border.BorderColorRight = Color.White;
                //Added By Srinath 31/5/2013
                FpEntry.Sheets[0].ColumnHeader.Cells[6, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorRight = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;

                if (FpEntry.Sheets[0].RowCount > 0) // Added by sridharan april 30 2015
                {
                    int colccount = FpEntry.Sheets[0].Columns.Count;
                    colccount = colccount / 3;
                    FpEntry.Sheets[0].RowCount++;
                    FpEntry.Sheets[0].RowCount++;
                    FpEntry.Sheets[0].RowCount++;


                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 2, 1, colccount);

                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = "Class Advisor";



                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, colccount * 2, 1, colccount);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, colccount * 2].Text = "HOD";

                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, colccount * 3, 1, colccount);
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, colccount * 3].Text = "Principal";
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.White;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 2].Border.BorderColor = Color.White;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 3].Border.BorderColor = Color.White;

                    FpEntry.SaveChanges();

                }

                lblnorec.Visible = false;
            }
            else
            {
                //FpEntry.Visible = false;
                //lblnorec.Visible = true;
            }
        }
        catch
        {
        }
    }

    //-----------------------------------------------func to get the hash key---------------------------------
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
        lblnorecc.Visible = false;
        lblnorec.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;

        ddlTest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        BindSectionDetail();
        sem_start_end_date();
    }
    public void filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "ORDER BY r.Roll_No";
            strregorder = "ORDER BY registration.Roll_No";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strregorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strregorder = "ORDER BY registration.Stud_Name";
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
        }

    }


    protected void btnGo_Click(object sender, EventArgs e)
    {


        ddlpage.Items.Clear();
        ddlpage.Text = "";
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
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;

        if ((ddlDegree.Items.Count > 0) && (ddlDegree.Items.Count > 0))
        {
            buttonG0();
            func_hide_clmnhdr_row();
            func_multi_iso();
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = "";

        }


    }
    protected void buttonG0()
    {
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        FpEntry.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        FpEntry.CurrentPage = 0;
        int indexcnt = 0;


        string valfromdate = "";
        string valtodate = "";
        string frmconcat = "";

        if (rbselectionlist.SelectedValue == "1")
        {
            if (txtnofarreares.Text == "")
            {
                lblnorec.Text = "Kindly Enter number of Arreares";
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
                lblnorecc.Visible = false;
                FpEntry.Sheets[0].RowCount = 0;
                return;

            }
        }

        int days = ts.Days;

        if (Convert.ToString(Session["QueryString"]) != "")
        {
            if (days < 0)
            {
                days = 0;
            }
        }
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
            lblnorecc.Visible = false;
            FpEntry.Sheets[0].RowCount = 0;

        }




        else
        {
            //lblnorec.Text = "";
            lblnorec.Visible = false;
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
            lblnorecc.Visible = false;


            if (ddlTest.SelectedIndex != 0)
            {
                //--------------------------------------------------------------------------------
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

                per_from_date = Convert.ToDateTime(frdate);
                per_to_date = Convert.ToDateTime(todate);

                ht_sphr.Clear();
                string hrdetno = "";
                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + per_from_date.ToString() + "' and '" + per_to_date.ToString() + "'";
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
                //--------------------------------------------------------------------------
                if (ddlTest.Items.Count <= 0) //added by sridhar 05 sep 2014
                {
                    lblnorec.Text = "Please Select Any One Test";
                    lblnorec.Visible = true;
                    return;
                }
                if (ddlTest.SelectedItem.Value.ToString() == "Terminal Test")
                {


                }
                else
                {
                    if (ddlSec.Enabled == true || ddlSec.Text != "-1" || ddlSec.Enabled == false)
                    {
                        FpEntry.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        lblnorecc.Visible = false;
                        FpEntry.Sheets[0].ColumnHeader.RowCount = 3;
                        SpreadBind();
                        if (FpEntry.Sheets[0].RowCount > 0)
                        {
                            function_footer();

                            function_radioheader();
                            FpEntry.SaveChanges();
                        }
                        // view_header_setting();


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
                        lblnorecc.Visible = false;


                    }

                    if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) == 0)
                    {
                        lblnorec.Visible = true;
                        FpEntry.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        lblnorecc.Visible = false;
                        RadioHeader.Visible = false;
                        Radiowithoutheader.Visible = false;
                        lblpages.Visible = false;
                        ddlpage.Visible = false;
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

                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
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
                            // CalculateTotalPages();
                        }

                    }

                    if (ddlTest.SelectedItem.Value.ToString() == "--Select--")
                    {
                        FpEntry.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        lblnorecc.Visible = false;
                    }
                }
            }
        }
    }
    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
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
            lblnorecc.Visible = false;
            RadioHeader.Visible = false;
            Radiowithoutheader.Visible = false;
            lblpages.Visible = false;
            ddlpage.Visible = false;

            buttonG0();
        }
        catch
        {
            lblnorec.Visible = true;
            FpEntry.Visible = true;

            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
        }

    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTest();

        lblnorec.Visible = false;
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
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;

        con.Open();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();

        //binddegree();
        //bindbranch();       
        //bindsem();        
        //bindsec();

        sem_start_end_date();
        GetTest();

        lblnorec.Visible = false;
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
            FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            //CalculateTotalPages();

        }
        FpEntry.SaveChanges();
        FpEntry.CurrentPage = 0;
    }


    //void CalculateTotalPages()
    //{
    //    Double totalRows = 0;
    //    totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
    //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //    Buttontotal.Visible = true;
    //}
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
                //CalculateTotalPages();
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
                atten = "LA"; //EOD
                break;
            //**********Modified By Subburaj 21.08.2014**********//
            case 18:
                atten = "RAA";
                break;
            //****************End***************************//
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
        lblnorecc.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;

        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;


    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }


    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
    }
    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        int totrowcount = FpEntry.Sheets[0].RowCount;
        int pages = totrowcount / 14;
        int intialrow = 1;
        int remainrows = totrowcount % 14;
        if (FpEntry.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 14;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
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
                //CalculateTotalPages();
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;

        }
    }
    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        int totrowcount = FpEntry.Sheets[0].RowCount;
        int pages = totrowcount / 14;
        int intialrow = 1;
        int remainrows = totrowcount % 14;
        if (FpEntry.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 14;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
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

                // CalculateTotalPages();
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;

        }
    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {

        //--------------------------------------------------------------------------------
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

        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);

        ht_sphr.Clear();
        string hrdetno = "";
        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + per_from_date.ToString() + "' and '" + per_to_date.ToString() + "'";
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
        //--------------------------------------------------------------------------

        batch = ddlBatch.SelectedValue.ToString();
        degreecode = ddlBranch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();
        semester = ddlSemYr.SelectedValue.ToString();
        criteria_no = ddlTest.SelectedValue.ToString();

        SpreadBind();
        function_footer();
        func_multi_iso();

        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "CAM_Report.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");

        string subcount = "0";
        int start = 0;
        int end = 0;
        int rollcol = 0;
        int inirow = 0;
        int visible_start = 0;
        start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + (4 + Convert.ToInt16(subcount) + 3);

        if ((ddlpage.Text.ToString() == "") || (ddlpage.Text.ToString() == "1") || (ddlpage.Text.ToString() == "0"))
        {
            subcount = ds2.Tables[1].Rows.Count.ToString();
            start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + (4 + Convert.ToInt16(subcount) + 3);
            end = (start + 18) - 1;
            rollcol = 3;
            inirow = 4;
            visible_start = start - 1;
        }
        else
        {
            start = (Convert.ToInt32(ddlpage.SelectedValue.ToString()) - 25) + (4 + Convert.ToInt16(subcount) + 3); //start = (Convert.ToInt32(ddlpage.SelectedValue.ToString())-30) + (4 + Convert.ToInt16(subcount) + 3);
            end = (start + 40) - 1;
            rollcol = 0;
            inirow = 0;
            visible_start = start - 1;
        }


        for (int i = (inirow + Convert.ToInt16(subcount) + rollcol); i < FpEntry.Sheets[0].RowCount; i++)
        {
            FpEntry.Sheets[0].Rows[i].Visible = false;
        }


        if (end >= FpEntry.Sheets[0].RowCount - 16)
        {
            end = FpEntry.Sheets[0].RowCount - 16;
        }

        int rowstart = (FpEntry.Sheets[0].RowCount - 16) - Convert.ToInt32(start);
        int rowend = (FpEntry.Sheets[0].RowCount - 16) - Convert.ToInt32(end);

        for (int i = visible_start; i < end; i++)
        {
            FpEntry.Sheets[0].Rows[i].Visible = true;
        }

        Boolean isflag = false;
        if (Convert.ToInt16(ddlpage.SelectedIndex) == (ddlpage.Items.Count - 1))
        {
            for (int last17 = (FpEntry.Sheets[0].RowCount - 20); last17 < FpEntry.Sheets[0].RowCount; last17++)
            {
                FpEntry.Sheets[0].Rows[last17].Visible = true;
                if (FpEntry.Sheets[0].Cells[last17, 0].Text.ToString() == "55") //not repeat 55'th stud record in last page
                {
                    FpEntry.Sheets[0].Rows[last17].Visible = false;
                }

                if (stud_count < 55) //not repeat less 55 stud record in last page
                {
                    if ((FpEntry.Sheets[0].Cells[last17, 0].Text.ToString() == "COURSEWISE PERFORMANCE") && (isflag == false))
                    {
                        isflag = true;
                    }
                    else if (isflag == false)
                    {
                        FpEntry.Sheets[0].Rows[last17].Visible = false;
                    }
                }
            }


            if (dsprint.Tables[0].Rows.Count > 0)
            {
                if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "1")
                {
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 3].Visible = true;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 2].Visible = true;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Visible = true;
                }
            }
        }
        else
        {
            for (int last17 = (FpEntry.Sheets[0].RowCount - 16); last17 < FpEntry.Sheets[0].RowCount; last17++)
            {
                FpEntry.Sheets[0].Rows[last17].Visible = false;
            }
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0")
                {
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 3].Visible = true;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 2].Visible = true;
                    FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Visible = true;
                }
            }
        }


        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
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

                //CalculateTotalPages();
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;

        }

        if ((ddlpage.Text.ToString() != "") && (ddlpage.Text.ToString() != "1"))
        {
            for (int i = 0; i <= FpEntry.Sheets[0].ColumnHeader.RowCount - 1; i++)
            {
                FpEntry.Sheets[0].ColumnHeader.Rows[i].Visible = false;
            }

            for (int colcount = 0; colcount <= (FpEntry.Sheets[0].ColumnCount - 1); colcount++)
            {
                FpEntry.Sheets[0].Cells[0, colcount].Border.BorderColorTop = Color.Black;
                FpEntry.Sheets[0].Cells[0, colcount].Border.BorderColorBottom = Color.Black;
            }


        }
        prevs_endrow = end;
    }

    //'-------------func for printmaster
    protected void btnPrintMaster_Click(object sender, EventArgs e)
    {
        string selected_criteria = "";
        string select_frm_date = txtFromDate.Text;
        string select_to_date = txtToDate.Text;
        string select_affliate = "";

        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex + "$" + selected_criteria.ToString() + "$" + select_frm_date + "$" + select_to_date + "$" + ddlcollege.SelectedIndex.ToString();
        //   PrintMaster = true;
        buttonG0();
        //lblpages.Visible = true;
        //ddlpage.Visible = true;

        string clmnheadrname = "";

        string dis_hdng_batch = "";
        if (ddlSec.Text != "")
        {
            dis_hdng_batch = ddlBatch.SelectedItem.ToString() + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + ddlSec.SelectedItem.ToString();
        }
        else
        {
            dis_hdng_batch = ddlBatch.SelectedItem.ToString() + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString();
        }
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    select_affliate = batch_degree_branch;
        //}
        //else
        //{
        select_affliate = category + ", Affliated to" + affliated;
        //}

        //string dis_hdng_sec = "Semester " + "- " + ddlSemYr.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        //string dis_date = "From Date " + "- " + txtFromDate.Text.ToString() + " " + "To Date " + "- " + txtToDate.Text.ToString();
        Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "CAM_Report.aspx" + ":" + "Continuous Assessment Report" + ":" + dis_hdng_batch);

    }
    public void func_header()
    {
        collnamenew1 = "";
        address = "";
        address1 = "";
        address2 = "";
        district = "";
        Phoneno = "";
        phnfax = "";
        Faxno = "";
        email = "";
        form_heading_name = "";
        batch_degree_branch = "";
        pincode = "";
        state = "";
        affliated = "";
        int temp_count_temp = 0;
        string[] header_align_index;
        string[] header_align;
        string new_header = "", new_header_index = "", header_alignment = "";


        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "CAM_Report.aspx");
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

            }
            if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
            {
                address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();

            }
            if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
            {
                address3 = dsprint.Tables[0].Rows[0]["address3"].ToString();

            }
            if (dsprint.Tables[0].Rows[0]["district"].ToString() != "")
            {
                district = dsprint.Tables[0].Rows[0]["district"].ToString();

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
            //-------newly added on 13.07.12
            if (dsprint.Tables[0].Rows[0]["state"].ToString() != "")
            {
                state = dsprint.Tables[0].Rows[0]["state"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["pincode"].ToString() != "")
            {
                pincode = dsprint.Tables[0].Rows[0]["pincode"].ToString();
                if (pincode == "0")
                {
                    pincode = " ";
                }
                else
                {
                    pincode = dsprint.Tables[0].Rows[0]["pincode"].ToString();
                }
                //  state=state+"-"+pincode;
            }

            //---------------newly added
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
            else
            {
                form_heading_name = " ";
            }
            if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
            {
                batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
                //split_batch_deg = batch_degree_branch.Split('@');
            }
            else
            {
                batch_degree_branch = "";
            }
            if (dsprint.Tables[0].Rows[0]["affliated"].ToString() != "") //added on 19.07.12
            {
                affliated = dsprint.Tables[0].Rows[0]["affliated"].ToString();

            }
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "") //added on 19.07.12
            {
                new_header = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "") //added on 19.07.12
            {
                new_header_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["MultiISOCode"].ToString() != "") //added on 19.07.12
            {
                MultiISO = dsprint.Tables[0].Rows[0]["MultiISOCode"].ToString();
            }

        }
        else if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
        {
            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'-') as phoneno,category,affliatedby,isnull(faxno,'-') as faxno,district,email,website,pincode,state from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
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
                    address3 = collegename["address3"].ToString();
                    district = collegename["district"].ToString();
                    address = address1 + "-" + address2 + "-" + district;
                    Phoneno = collegename["phoneno"].ToString();
                    Faxno = collegename["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                    email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                    pincode = collegename["pincode"].ToString();
                    if (pincode != "")
                    {
                        if (pincode == "0")
                        {
                            pincode = " ";
                        }
                        else
                        {
                            pincode = collegename["pincode"].ToString();
                        }

                    }
                    address3 = collegename["address3"].ToString() + "-" + pincode;
                    state = collegename["state"].ToString();
                    category = collegename["category"].ToString();
                    affliated = collegename["affliatedby"].ToString();
                }
                //  affliated = category + "," + affliated;
                affliated = affliated;
                batch_degree_branch = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + sections.ToString();
            }
            con.Close();
        }
        //FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1.ToString();
        //FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = affliated;//affliate
        //FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Text = address3 + "-" + district + "," + pincode;//phnfax;
        //FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Text = state;
        //FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].Text = phnfax;
        //FpEntry.Sheets[0].ColumnHeader.Cells[5, 1].Text = email;
        //FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = batch_degree_branch;

        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["MultiISOCode"].ToString() != "")
            {
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, FpEntry.Sheets[0].ColumnCount - 1);

            }
            else
            {
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, FpEntry.Sheets[0].ColumnCount - 1);
            }
        }
        else
        {
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, FpEntry.Sheets[0].ColumnCount - 3);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, FpEntry.Sheets[0].ColumnCount - 1);
        }





        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorTop = Color.White;


        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorTop = Color.White;


        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1.ToString();
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Text = affliated;//category + ", Affliated to" + affliated;//address.ToString();


        if (address3 != string.Empty && district != string.Empty && pincode != string.Empty)
        {
            //gowthman 02Aug2013 FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = address3 + "-" + district + "," + pincode;//phnfax;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = address3 + ", " + state;//phnfax;
        }
        else if (address1 != string.Empty && address3 != string.Empty && pincode != string.Empty)
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = address1 + "," + address3 + "-" + pincode + ", " + state;//phnfax;
        }
        else if (district != string.Empty && pincode != string.Empty)
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = district + "," + pincode + ", " + state;//phnfax;
        }
        else if (address3 != string.Empty && pincode != string.Empty)
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = address3;
        }
        else if (pincode != string.Empty)
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = pincode + ", " + state;
        }

        FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Text = state;
        FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Text = phnfax;
        FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Text = email;
        FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Text = batch_degree_branch;
        FpEntry.Sheets[0].ColumnHeader.Cells[6, 3].Text = "";


        FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].HorizontalAlign = HorizontalAlign.Center;

        //@@@@@@@@@@@ added on 17.07.12
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Large;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].Font.Size = FontUnit.Medium;

        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[3, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[4, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[5, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[6, 2].Font.Bold = true;
        //@@@@@@@@@@@@@@@@@@@@@@@@
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
        //================ add new header===================

        if (new_header.Trim() != "")
        {

            if (new_header.Trim() != null && new_header.Trim() != "")
            {
                header_align = new_header.ToString().Split(',');
                header_align_index = new_header_index.ToString().Split(',');
                FpEntry.Sheets[0].ColumnHeader.Rows.Count += header_align_index.GetUpperBound(0) + 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
                FpEntry.Sheets[0].ColumnHeader.Cells[6, 1].Text = "";
                for (int row_head_count = 7; row_head_count < (7 + header_align.GetUpperBound(0) + 1); row_head_count++)
                {
                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Text = header_align[temp_count_temp].ToString();
                    //if (final_print_col_cnt > 3)
                    {
                        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, 0, 1, (FpEntry.Sheets[0].ColumnCount));
                    }
                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Border.BorderColorTop = Color.White;
                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Border.BorderColorBottom = Color.White;


                    if (temp_count_temp <= header_align_index.GetUpperBound(0))
                    {
                        if (header_align_index[temp_count_temp].ToString() != string.Empty)
                        {
                            header_alignment = header_align_index[temp_count_temp].ToString();
                            if (header_alignment == "2")
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (header_alignment == "1")
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }

                    temp_count_temp++;
                }
            }
        }
        ////==================================================
        //if (MultiISO != "")
        //{
        //    string[] spl_iso = MultiISO.Split(',');
        //    if (spl_iso.GetUpperBound(0) > 6)
        //    {
        //    }
        //    else
        //    {
        //        for (int iso = 0; iso < spl_iso.GetUpperBound(0) + 1; iso++)
        //        {

        //            FpEntry.Sheets[0].ColumnHeader.Cells[iso, FpEntry.Sheets[0].ColumnCount - 2].Text = spl_iso[iso].ToString();
        //        }
        //    }
        // }
        ////================================================
    }

    //'-------------------------------------------------------
    public void function_radioheader()
    {
        ddlpage.Items.Clear();
        int totrowcount = 0;
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != string.Empty)
            {
                for (int find_tot_rowcnt = 8; find_tot_rowcnt < (FpEntry.Sheets[0].RowCount - 19); find_tot_rowcnt++)
                {
                    totrowcount++;
                }
            }
            else
            {
                for (int find_tot_rowcnt = 8; find_tot_rowcnt < (FpEntry.Sheets[0].RowCount - 16); find_tot_rowcnt++)
                {
                    totrowcount++;
                }
            }
        }
        else
        {

            for (int find_tot_rowcnt = 8; find_tot_rowcnt < (FpEntry.Sheets[0].RowCount - 16); find_tot_rowcnt++)
            {
                totrowcount++;
            }
        }
        int pages = totrowcount / 40;
        int intialrow = 1;
        int remainrows = totrowcount % 40;

        if (stud_count <= 72)  //Aruna on 13sep2012
            pages = pages + 1;


        if (FpEntry.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 40;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
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
                // CalculateTotalPages();
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;

        }
    }
    //'----------------------func for footer
    public void function_footer()
    {
        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "CAM_Report.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");

        int col_count = 0;
        ////  int no_of_footer =Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
        //int footer_flag = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString());
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            ////----------------start for setting the footer
            //if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
            //{

            //    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
            //    FpEntry.Sheets[0].RowCount += 3;
            //    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            //    string[] footer_text_split = footer_text.Split(',');

            //    int count_span = FpEntry.Sheets[0].ColumnCount / footer_count;

            //    if (footer_text_split.GetUpperBound(0) > 0)
            //    {
            //        for (footer_balanc_col = 0; footer_balanc_col < footer_text_split.GetUpperBound(0) + 1; footer_balanc_col++)
            //        {
            //            if (footer_balanc_col == 0)
            //            {
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Text = footer_text_split[footer_balanc_col].ToString();
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Font.Size = FontUnit.Medium;
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Font.Bold = true;
            //                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, footer_balanc_col, 1, footer_balanc_col);
            //            }
            //            else
            //            {
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Text = footer_text_split[footer_balanc_col].ToString();
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Size = FontUnit.Medium;
            //                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Bold = true;
            //                FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span, 1, FpEntry.Sheets[0].ColumnCount);
            //            }

            //        }
            //    }
            //    else
            //    {
            //        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount-1].Text = footer_text;
            //        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount-1].Font.Size = FontUnit.Medium;
            //        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount-1].Font.Bold = true;
            //        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount].Border.BorderColorLeft = Color.White;

            //    }
            //    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 0, 1, FpEntry.Sheets[0].ColumnCount);
            // //   FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 0, 1, FpEntry.Sheets[0].ColumnCount);
            //    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount);
            //    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Border.BorderColor = Color.White;
            //    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.White;
            //    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;
            //}

            //2.Footer setting


            if (footer_text.Trim() != "")
            {
                if (footer_text != null && footer_text != "")
                {

                    string[] footer_text_split = footer_text.Split(',');

                    footer_count = Convert.ToInt16((footer_text_split.GetUpperBound(0) + 1).ToString());
                    FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 3;

                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 3), 0].ColumnSpan = FpEntry.Sheets[0].ColumnCount;
                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), 0].ColumnSpan = FpEntry.Sheets[0].ColumnCount;

                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;




                    footer_text = "";




                    if (FpEntry.Sheets[0].ColumnCount < footer_count)
                    {
                        for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
                        {
                            if (footer_text == "")
                            {
                                footer_text = footer_text_split[concod_footer].ToString();
                            }
                            else
                            {
                                footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
                            }
                        }

                        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                        {
                            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                            {
                                FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, FpEntry.Sheets[0].ColumnCount);
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                                break;
                            }
                        }

                    }

                    else if (FpEntry.Sheets[0].ColumnCount == footer_count)
                    {
                        temp_count = 0;
                        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                        {
                            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                            {
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                                temp_count++;
                                if (temp_count == footer_count)
                                {
                                    break;
                                }
                            }
                        }

                    }

                    else
                    {

                        temp_count = 0;
                        split_col_for_footer = FpEntry.Sheets[0].ColumnCount / footer_count;
                        footer_balanc_col = FpEntry.Sheets[0].ColumnCount % footer_count;

                        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                        {
                            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                            {
                                if (temp_count == 0)
                                {
                                    FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);//+ footer_balanc_col);
                                }
                                else
                                {

                                    FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

                                }
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;

                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col_count].HorizontalAlign = HorizontalAlign.Center;

                                if (col_count - 1 >= 0)
                                {
                                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
                                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
                                }
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                                FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
                                if (col_count + 1 < FpEntry.Sheets[0].ColumnCount)
                                {
                                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
                                    FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
                                }


                                temp_count++;
                                if (temp_count == 0)
                                {
                                    col_count = col_count + split_col_for_footer + footer_balanc_col;
                                }
                                else
                                {
                                    col_count = col_count + split_col_for_footer;
                                }
                                if (temp_count == footer_count)
                                {
                                    break;
                                }
                            }
                        }
                    }



                }
            }
        }
        //2 end.Footer setting
    }
    public string findroman(string sem)
    {
        string sem3 = "";
        if (sem == "1")
            sem3 = "I";
        else if (sem == "2")
            sem3 = "II";
        else if (sem == "3")
            sem3 = "III";
        else if (sem == "4")
            sem3 = "IV";
        else if (sem == "5")
            sem3 = "V";
        else if (sem == "6")
            sem3 = "VI";
        else if (sem == "7")
            sem3 = "VII";
        else if (sem == "8")
            sem3 = "VIII";
        else if (sem == "9")
            sem3 = "IX";
        else if (sem == "10")
            sem3 = "X";
        return sem3;
    }

    public string getyear(string sem)
    {
        string sem3 = "";
        if (sem == "1" || sem == "2")
        {
            sem3 = "I";
        }
        else if (sem == "3" || sem == "4")
        {
            sem3 = "II";
        }
        else if (sem == "5" || sem == "6")
        {
            sem3 = "III";
        }
        else if (sem == "7" || sem == "8")
        {
            sem3 = "IV";
        }
        else if (sem == "9" || sem == "10")
        {
            sem3 = "V";
        }
        return sem3;
    }
    public void func_hide_clmnhdr_row()
    {
        //if (collnamenew1.ToString() == "")
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[0].Visible = false;
        //}
        //if (affliated.ToString() == "")
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[1].Visible = false;
        //}
        //if (address3 == string.Empty && district == string.Empty && pincode == string.Empty)
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[2].Visible = false;
        //}
        //if (state == string.Empty)
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[3].Visible = false;
        //}
        //if (phnfax == "")
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[4].Visible = false;
        //}
        //if (email == "")
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //}
        //if (batch_degree_branch == "")
        //{
        //    FpEntry.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //}


        //for (int i = 0; i < FpEntry.Sheets[0].ColumnHeader.RowCount; i++)
        //{
        //    if (FpEntry.Sheets[0].ColumnHeader.Cells[i, 1].Text == " " || FpEntry.Sheets[0].ColumnHeader.Cells[i, 1].Text == "" )
        //    {
        //        FpEntry.Sheets[0].ColumnHeader.Rows[i].Visible = false;
        //    }
        //}
    }

    public void func_multi_iso()
    {
        try
        {
            hat.Clear();
            hat.Add("college_code", Session["InternalCollegeCode"].ToString());
            hat.Add("form_name", "CAM_Report.aspx");
            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                MultiISO = dsprint.Tables[0].Rows[0]["MultiISOCode"].ToString();
            }
            //==================================================
            if (MultiISO != "")
            {
                string[] spl_iso = MultiISO.Split(',');
                int c = 0;
                int isocount = 0;
                int rowcount = 0;
                isocount = spl_iso.GetUpperBound(0) + 1;
                if (spl_iso.GetUpperBound(0) > 0)
                {
                    for (int iso = 0; iso < FpEntry.Sheets[0].ColumnHeader.RowCount; iso++)
                    {
                        if (FpEntry.Sheets[0].ColumnHeader.Rows[iso].Visible == true)
                        {
                            if (c <= spl_iso.GetUpperBound(0))
                            {
                                rowcount++;
                                FpEntry.Sheets[0].ColumnHeader.Cells[iso, FpEntry.Sheets[0].ColumnCount - 2].Text = spl_iso[c].ToString();
                                FpEntry.Sheets[0].ColumnHeader.Cells[iso, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorRight = Color.White;
                                FpEntry.Sheets[0].ColumnHeader.Cells[iso, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;
                                FpEntry.Sheets[0].ColumnHeader.Cells[iso, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorTop = Color.White;
                                FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorTop = Color.Black;
                                c++;
                            }
                        }
                    }
                }
                int remain_rowcount = isocount - rowcount;
                if (remain_rowcount != 0)
                {
                    //  FpEntry.Sheets[0].ColumnHeader.RowCount += remain_rowcount;
                    for (int iso1 = c; iso1 < isocount; iso1++)
                    {
                        FpEntry.Sheets[0].ColumnHeader.RowCount++;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Text = spl_iso[c].ToString();
                        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount - 3);

                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = " ";
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Text = " ";
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 3].Text = " ";

                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].Border.BorderColorRight = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].Border.BorderColorBottom = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, 0].Border.BorderColorTop = Color.White;

                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorRight = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 2].Border.BorderColorTop = Color.White;

                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 3].Border.BorderColorRight = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 3].Border.BorderColorBottom = Color.White;
                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 3].Border.BorderColorTop = Color.White;

                        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;

                        c++;
                    }
                }
            }
        }
        catch
        {
        }
        //================================================
    }
    protected void rbselectionlist_selectedindexchanged(object sender, EventArgs e)
    {
        if (rbselectionlist.SelectedValue == "1")
        {
            txtnofarreares.Text = "";
            txtnofarreares.Enabled = true;
        }
        else if (rbselectionlist.SelectedValue == "2")
        {
            txtnofarreares.Text = "";
            txtnofarreares.Enabled = false;
        }


    }
    public void view_header_setting()
    {
        try
        {
            int row_cnt = 0;
            DataSet dsprint = new DataSet();
            string view_footer = "", view_header = "", view_footer_text = "";
            hat.Clear();
            hat.Add("college_code", Session["InternalCollegeCode"].ToString());
            hat.Add("form_name", "CAM_Report.aspx");
            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");

            if (dsprint.Tables[0].Rows.Count > 0)
            {

                //ddlpage.Visible = true;
                //lblpages.Visible = true;

                view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
                view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
                view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
                if (view_header == "0" || view_header == "1")
                {
                    // lblError.Visible = false;

                    //for (row_cnt = 0; row_cnt < FpEntry.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    //{

                    //    if (FpEntry.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1")
                    //    {
                    //        FpEntry.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                    //    }
                    //}

                    for (row_cnt = 0; row_cnt < FpEntry.Sheets[0].RowCount; row_cnt++)
                    {

                        if (FpEntry.Sheets[0].Cells[row_cnt, 0].Text == "CONSOLIDATED GRADE SHEET")
                        {
                            break;
                        }
                    }
                    row_cnt += 4;

                    int i = 0;
                    ddlpage.Items.Clear();
                    int totrowcount = FpEntry.Sheets[0].RowCount;
                    int pages = (totrowcount - row_cnt - 16) / 25;
                    int intialrow = 1;
                    int remainrows = (totrowcount - row_cnt - 16) % 25;
                    if (FpEntry.Sheets[0].RowCount > 0)
                    {
                        int i5 = 0;
                        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                        for (i = 1; i <= pages; i++)
                        {
                            i5 = i;

                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                            intialrow = intialrow + 25;
                        }
                        if (remainrows > 0)
                        {
                            i = i5 + 1;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));

                        }
                        {
                            intialrow = FpEntry.Sheets[0].RowCount - 16;
                            i = i5 + 2;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        }
                    }
                    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                    {
                        for (i = 0; i < FpEntry.Sheets[0].RowCount; i++)
                        {
                            FpEntry.Sheets[0].Rows[i].Visible = true;
                        }
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
                            FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                            // CalculateTotalPages();
                        }


                        // pnlrecordcount.Visible = true;


                    }
                    //else
                    //{
                    ////    lblError.Visible = false;
                    //   // pnlrecordcount.Visible = false;
                    //}/
                }
                else if (view_header == "2")
                {

                    for (row_cnt = 0; row_cnt < FpEntry.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {
                        if (FpEntry.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1")
                        {
                            FpEntry.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                        }
                    }

                    //   lblError.Visible = false;
                    int i = 0;
                    ddlpage.Items.Clear();
                    int totrowcount = FpEntry.Sheets[0].RowCount;
                    int pages = totrowcount / 25;
                    int intialrow = 1;
                    int remainrows = totrowcount % 25;
                    if (FpEntry.Sheets[0].RowCount > 0)
                    {
                        int i5 = 0;
                        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                        for (i = 1; i <= pages; i++)
                        {
                            i5 = i;

                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                            intialrow = intialrow + 25;
                        }
                        if (remainrows > 0)
                        {
                            i = i5 + 1;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        }
                    }
                    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                    {
                        for (i = 0; i < FpEntry.Sheets[0].RowCount; i++)
                        {
                            FpEntry.Sheets[0].Rows[i].Visible = true;
                        }
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
                            //  FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                            //CalculateTotalPages();
                        }
                        //  pnlrecordcount.Visible = true;
                    }
                    // else
                    //{
                    //    pnlrecordcount.Visible = false;
                    //}
                }
                else
                {

                }
                //lblpages.Visible = true;
                //ddlpage.Visible = true;
            }
            else
            {
                lblpages.Visible = false;
                ddlpage.Visible = false;
            }
        }
        catch
        {
        }
    }
    //public void getspecial_hr()
    //{
    //    //  try
    //    {
    //        con_splhr_query_master.Close();
    //        con_splhr_query_master.Open();
    //        DataSet ds_splhr_query_master = new DataSet();
    //        //  no_stud_flag = false;
    //        string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlBatch.SelectedValue.ToString() + " and current_semester=" + ddlSemYr.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + stud_roll + "'  order by r.roll_no asc";
    //        SqlDataReader dr_splhr_query_master;
    //        cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
    //        dr_splhr_query_master = cmd.ExecuteReader();

    //        while (dr_splhr_query_master.Read())
    //        {
    //            if (dr_splhr_query_master.HasRows)
    //            {
    //                value = dr_splhr_query_master[0].ToString();

    //                if (value != null && value != "0" && value != "7" && value != "")
    //                {
    //                    if (tempvalue != value)
    //                    {
    //                        tempvalue = value;
    //                        for (int j = 0; j < count; j++)
    //                        {

    //                            if (ds8.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
    //                            {
    //                                ObtValue = int.Parse(ds8.Tables[0].Rows[j]["CalcFlag"].ToString());
    //                                j = count;
    //                            }
    //                        }
    //                    }
    //                    if (ObtValue == 1)
    //                    {
    //                        per_abshrs_spl += 1;
    //                    }
    //                    else if (ObtValue == 2)
    //                    {
    //                        notconsider_value += 1;
    //                        njhr += 1;
    //                    }
    //                    else if (ObtValue == 0)
    //                    {
    //                        tot_per_hrs_spl += 1;
    //                    }
    //                    if (value == "3")
    //                    {
    //                        tot_ondu_spl += 1;
    //                    }
    //                    else if (value == "10")
    //                    {
    //                        per_leave += 1;
    //                    }
    //                    tot_conduct_hr_spl++;
    //                }
    //                else if (value == "7")
    //                {
    //                    per_hhday_spl += 1;
    //                    tot_conduct_hr_spl--;
    //                }
    //                else
    //                {
    //                    unmark_spl += 1;
    //                    tot_conduct_hr_spl--;
    //                }
    //            }
    //        }


    //            per_abshrs_spl_fals = per_abshrs_spl;
    //            tot_per_hrs_spl_fals = tot_per_hrs_spl;
    //            per_leave_fals = per_leave;
    //            tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
    //            tot_ondu_spl_fals = tot_ondu_spl;


    //    }
    //    //  catch
    //    {
    //    }
    //}
    public void getspecial_hr()
    {
        try
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

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + stud_roll + "'  and hrdet_no in(" + hrdetno + ")";
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
                                if (attmaster.Contains(value.ToString()))
                                {
                                    ObtValue = int.Parse(GetCorrespondingKey(value.ToString(), attmaster).ToString());
                                }
                                else
                                {
                                    ObtValue = 0;
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


            }
        }
        catch
        {
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Pageload(sender, e);
    }
    public void Pageload(object sender, EventArgs e)
    {
        Session["QueryString"] = "";
        Radiowithoutheader.Visible = false;
        RadioHeader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        FpEntry.Sheets[0].SheetName = " ";
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;

        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
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

        FpEntry.Sheets[0].SheetCorner.RowCount = 7;
        FpEntry.Sheets[0].SheetCorner.Cells[6, 0].Text = "S.No";
        FpEntry.Sheets[0].SheetCorner.Cells[6, 0].BackColor = Color.AliceBlue;


        //FpEntry.Sheets[0].Columns[1].Width = 100;
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
        FpEntry.Sheets[0].FrozenColumnCount = 4;
        FpEntry.Sheets[0].Columns[0].Width = 70;
        //  FpEntry.Sheets[0].Columns[1].Width = 70;
        //FpEntry.Sheets[0].Columns[2].Width = 100;

        FpEntry.Pager.PageCount = 5;
        FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpEntry.Sheets[0].AutoPostBack = true;
        RadioButtonList3.SelectedValue = "4";


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
                //=========== hide the printmaster setting button based on print master setting mythili on 21.07.12
                if (mtrdr["settings"].ToString() == "print_master_setting" && mtrdr["value"].ToString() == "1")
                {
                    btnPrintMaster.Visible = false; // true;
                }
                else
                {
                    btnPrintMaster.Visible = false;
                }
                //===================
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

        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();
        usercode = Session["usercode"].ToString();

        if (Request.QueryString["val"] == null)
        {

            string dt1 = DateTime.Today.ToShortDateString();
            string[] dsplit = dt1.Split(new Char[] { '/' });
            dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dateconcat.ToString();


            string dt2 = DateTime.Today.ToShortDateString();
            string[] dt2split = dt2.Split(new Char[] { '/' });
            date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
            txtToDate.Text = date1concat.ToString();

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
            bindsem();
            bindsec();
            GetTest();
        }
        else if (Request.QueryString["val"] != null)
        {


            Session["QueryString"] = Request.QueryString["val"].ToString();
            string get_pageload_value = Request.QueryString["val"];
            if (get_pageload_value.ToString() != null)
            {
                string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val

                bindbatch();
                ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());

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

                bindsem();
                ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                bindsec();
                ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                GetTest();
                ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                txtFromDate.Text = spl_load_val[2].ToString();
                txtToDate.Text = spl_load_val[3].ToString();

                ddlcollege.SelectedIndex = Convert.ToInt16(spl_load_val[4].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();



                btnGo_Click(sender, e);
                func_header();
                //    function_footer();
            }
        }
        if (!IsPostBack)
        {
            //Added By subburaj 06/09/2014
            txtFromDate.Attributes.Add("Readonly", "Readonly");
            txtToDate.Attributes.Add("Readonly", "Readonly");
        }



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

                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls";

                    FpEntry.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    protected void ddlbatch_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        BindDegreepop(singleuser, group_user, collegecode, usercode);
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSempop(strbranch, strbatchyear, collegecode);
        BindSectionDetailpop(strbatch, strbranch);

        sprdrollbind();
    }
    public void BindBranchpop(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegreeadd.SelectedValue.ToString();
            ddlbrachadd.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbrachadd.DataSource = ds2;
                ddlbrachadd.DataTextField = "dept_name";
                ddlbrachadd.DataValueField = "degree_code";
                ddlbrachadd.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void BindSectionDetailpop(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatchadd.SelectedValue.ToString();
            strbranch = ddlbrachadd.SelectedValue.ToString();

            ddlsecadd.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsecadd.DataSource = ds2;
                ddlsecadd.DataTextField = "sections";
                ddlsecadd.DataBind();
                ddlsecadd.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsecadd.Enabled = false;
                    ddlsecadd.Items.Add("All");
                }
                else
                {
                    ddlsecadd.Enabled = true;

                }
            }
            else
            {
                ddlsecadd.Enabled = false;
                ddlsecadd.Items.Add("All");
            }
        }
        catch (Exception ex)
        {

        }
    }

    // ----load sem pop---
    public void BindSempop(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = ddlbatchadd.Text.ToString();
            strbranch = ddlbrachadd.SelectedValue.ToString();

            ddlsemadd.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemadd.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemadd.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void BindDegreepop(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegreeadd.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddldegreeadd.DataSource = ds2;
                ddldegreeadd.DataTextField = "course_name";
                ddldegreeadd.DataValueField = "course_id";
                ddldegreeadd.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void sprdrollbind()
    {

        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdselectrollno.Sheets[0].ColumnCount = 6;
        sprdselectrollno.SaveChanges();
        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        sprdselectrollno.Sheets[0].Columns[4].CellType = chkcell;

        sprdselectrollno.Sheets[0].RowCount = sprdselectrollno.Sheets[0].RowCount + 1;
        sprdselectrollno.Sheets[0].SpanModel.Add(sprdselectrollno.Sheets[0].RowCount - 1, 0, 1, 4);

        sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].CellType = chkcell;
        sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        sprdselectrollno.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

        sprdselectrollno.Sheets[0].AutoPostBack = true;

        string strsec = "";
        if (ddlsecadd.SelectedValue.ToString() != "All")
        {
            strsec = "and sections='" + ddlsecadd.SelectedValue.ToString() + "'";

        }
        else
        {
            strsec = "";
        }
        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();

        sprdselectrollno.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Label = "Reg No";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[3].Label = "Name";
        sprdselectrollno.Sheets[0].ColumnHeader.Columns[4].Label = "Select";

        sprdselectrollno.Sheets[0].Columns[0].Width = 60;
        sprdselectrollno.Sheets[0].Columns[1].Width = 140;
        sprdselectrollno.Sheets[0].Columns[2].Width = 140;
        sprdselectrollno.Sheets[0].Columns[3].Width = 260;
        sprdselectrollno.Sheets[0].Columns[4].Width = 60;
        sprdselectrollno.Sheets[0].Columns[5].Width = 1;
        if (Session["Rollflag"].ToString() == "0")
        {
            sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible = false;
        }
        else
        {
            sprdselectrollno.Sheets[0].ColumnHeader.Columns[1].Visible = true;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible = false;
        }
        else
        {
            sprdselectrollno.Sheets[0].ColumnHeader.Columns[2].Visible = true;
        }
        if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "1")
        {
            sprdselectrollno.Sheets[0].Columns[2].Width = 220;
            sprdselectrollno.Sheets[0].Columns[3].Width = 320;
        }
        else if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "0")
        {
            sprdselectrollno.Sheets[0].Columns[1].Width = 220;
            sprdselectrollno.Sheets[0].Columns[3].Width = 320;
        }
        else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "0")
        {
            sprdselectrollno.Sheets[0].Columns[3].Width = 540;
        }


        //added By Srinath 15/8/2013
        Boolean serialflag = false;
        string strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
        string strserial = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (strserial != "" && strserial != "0" && strserial != null)
        {
            serialflag = true;
            strorder = "ORDER BY registration.serialno";
        }
        else
        {
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No,registration.stud_name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY len(registration.roll_no),registration.roll_no,registration.Stud_Name";
            }
        }

        //string getrollnoquery = "select roll_no,reg_no as reg_no,stud_name,current_semester from registration where batch_year=" + ddlbatchadd.SelectedValue.ToString() + " and current_semester=" + ddlsemadd.SelectedValue.ToString() + " " + strsec + " and   degree_code='" + ddlbrachadd.SelectedValue.ToString() + "' order by roll_no";
        string getrollnoquery = "select roll_no,reg_no as reg_no,stud_name,current_semester from registration where cc=0 and delflag=0 and exam_flag<>'Debar' and batch_year=" + ddlbatchadd.SelectedValue.ToString() + " and current_semester=" + ddlsemadd.SelectedValue.ToString() + " " + strsec + " and   degree_code='" + ddlbrachadd.SelectedValue.ToString() + "' " + strorder + "";
        SqlDataAdapter dagetrollnoquery = new SqlDataAdapter(getrollnoquery, con1);
        DataSet dsgetrollnoquery = new DataSet();
        con1.Close();
        con1.Open();
        dagetrollnoquery.Fill(dsgetrollnoquery);
        // sprdselectrollno.Sheets[0].RowCount = 0;
        if (dsgetrollnoquery.Tables[0].Rows.Count > 0)
        {
            lblnorec.Visible = false;
            int sno = 0;
            for (int rollnocount = 0; rollnocount < dsgetrollnoquery.Tables[0].Rows.Count; rollnocount++)
            {
                sno++;
                string rollno = dsgetrollnoquery.Tables[0].Rows[rollnocount]["roll_no"].ToString();
                string regno = dsgetrollnoquery.Tables[0].Rows[rollnocount]["reg_no"].ToString();

                sprdselectrollno.Sheets[0].RowCount++;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].Text = dsgetrollnoquery.Tables[0].Rows[rollnocount]["roll_no"].ToString();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Rows[(sprdselectrollno.Sheets[0].RowCount - 1)].CellType = textcel_type;
                if (regno == "")
                {
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].Text = "-";
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].Text = regno;
                    sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                }

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].Text = dsgetrollnoquery.Tables[0].Rows[rollnocount]["stud_name"].ToString();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Rows[sprdselectrollno.Sheets[0].RowCount - 1].Font.Bold = false;

                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].CellType = chkcell1;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                sprdselectrollno.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].Text = dsgetrollnoquery.Tables[0].Rows[rollnocount]["current_semester"].ToString();
                sprdselectrollno.Sheets[0].Cells[sprdselectrollno.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                chkcell.AutoPostBack = true;
                // chkcell1.AutoPostBack = true;
            }

        }
        else
        {
            //lblnorec.Visible = true;
            //lblnorec.Text = "No Records Found";
        }

        sprdselectrollno.Sheets[0].Columns[4].CellType = chkcell;
        int rowcount = sprdselectrollno.Sheets[0].RowCount;
        sprdselectrollno.Height = 300;
        sprdselectrollno.Sheets[0].PageSize = 25 + (rowcount * 20);
        sprdselectrollno.SaveChanges();
        con1.Close();
        sprdselectrollno.SaveChanges();
        sprdselectrollno.Sheets[0].AutoPostBack = false;
    }
    protected void sprdselectrollno_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellroll = true;
    }
    protected void sprdselectrollno_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //if (cellroll == true)
        //{
        //    cellroll = false;
        //    int flag = 0;

        //    for (int res = 1; res <= Convert.ToInt32(sprdselectrollno.Sheets[0].RowCount) - 1; res++)
        //    {
        //        int isval = 0;
        //        string s = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 4].Text;
        //        //sankar added may'20...............................
        //        string roll_no = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 1].Text;
        //        string staff_name = sprdselectrollno.Sheets[0].Cells[Convert.ToInt32(res), 3].Text;
        //        isval = Convert.ToInt32(sprdselectrollno.Sheets[0].Cells[res, 4].Value);
        //        if (isval == 0 || s == "")
        //        {

        //        }
        //        else
        //        {
        //            txtstdrollno.Enabled = false;

        //            txtstdrollno.Text = "";
        //            flag = 1;
        //        }
        //    }

        //    if (flag == 1)
        //    {
        //        txtstdrollno.Enabled = true;

        //        txtstdrollno.Text = "";
        //    }
        //    else
        //    {
        //        txtstdrollno.Enabled = false;

        //        txtstdrollno.Text = "";
        //    }

        //}
    }

    protected void sprdselectrollno_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(sprdselectrollno.Sheets[0].RowCount); j++)
                {
                    string actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "System.Object")
                        sprdselectrollno.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                }
                flag_true = true;
            }


            int flag = 0;


        }
        catch (Exception ex)
        {
            // errmsg.Text = ex.ToString();
        }
    }
    protected void ddldegreeadd_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }
    protected void ddlbrachadd_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }
    protected void ddlsem_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        BindSectionDetailpop(strbatch, strbranch);
        sprdrollbind();
    }
    protected void ddlsec_SelectedIndexXhanged(object sender, EventArgs e)
    {
        sprdselectrollno.Sheets[0].RowCount = 0;
        sprdrollbind();
    }
    public void BindBatchadd()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatchadd.DataSource = ds2;
                ddlbatchadd.DataTextField = "Batch_year";
                ddlbatchadd.DataValueField = "Batch_year";
                ddlbatchadd.DataBind();
                ddlbatchadd.SelectedIndex = ddlbatchadd.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            // errmsg.Text = ex.ToString();
        }
    }
    protected void btnselecttargetstudent_Click(object sender, EventArgs e)
    {
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        sprdselectrollno.Visible = true;
        panelrollnopop.Visible = true;

        sprdselectrollno.Sheets[0].RowCount = 0;
        BindBatchadd();
        BindDegreepop(singleuser, group_user, collegecode, usercode);
        BindBranchpop(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetailpop(strbatch, strbranch);
        BindSempop(strbranch, strbatchyear, collegecode);
        sprdrollbind();
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        sprdselectrollno.Visible = false;
        panelrollnopop.Visible = false;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int check = 0;
        sprdselectrollno.SaveChanges();
        int isval = 0;
        for (int i = 1; i < sprdselectrollno.Sheets[0].Rows.Count; i++)
        {
            isval = Convert.ToInt16(sprdselectrollno.Sheets[0].Cells[i, 4].Value);
            if (isval == 1)
            {
                check++;
            }
        }
        if (check == 0)
        {
            lblnoselecterr.Text = "Kindly select atleast one student";
            lblnoselecterr.Visible = true;
            panelrollnopop.Visible = true;
        }
        else
        {
            lblnoselecterr.Text = "";
            lblnoselecterr.Visible = false;
            panelrollnopop.Visible = false;
            buttonG0();
        }

    }
}

