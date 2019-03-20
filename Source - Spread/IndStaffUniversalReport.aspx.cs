using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
public partial class IndStaffUniversalReport : System.Web.UI.Page
{
    System.Web.UI.WebControls.TreeNode mm9 = new System.Web.UI.WebControls.TreeNode();
    System.Web.UI.WebControls.TreeNode mm1 = new System.Web.UI.WebControls.TreeNode();
    #region vaiable declaration
    //vetri.`
    InsproDirectAccess dir = new InsproDirectAccess();
    bool isSchoolOrCollege = false;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string nameprint = string.Empty;
    string departmentprint = string.Empty;
    string batchprint = string.Empty;
    string semprint = string.Empty;
    string admdatev = "", strtdate = "", examdate = string.Empty;
    string studname = string.Empty;
    string latmode = string.Empty;
    string regn = string.Empty;
    int subjectctot = 0, criteriatot = 0, tottet;
    string criteriain;
    string sqlmarkcmd;
    string rnkv = string.Empty;
    string rankov3 = string.Empty;
    string marks_per, marks_perfinal;
    int failv = 0;
    string sqlStr = string.Empty;
    int ddlcount = 0;
    string hcrollno = string.Empty;
    string batchyearv = string.Empty;
    string semesterv = string.Empty;
    string degreecodev = string.Empty;
    string sectionv = string.Empty;
    double strtot = 0;
    double strgradetempfrm = 0;
    double strgradetempto = 0;
    string syll_code = string.Empty;
    string examcodevalg = string.Empty;
    int gtempejval = 0;
    string strgradetempgrade = string.Empty;
    string strtotgrac = string.Empty;
    static string gatepass_staffdept = string.Empty;
    string staffcodesession = string.Empty;
    string collegcode1 = string.Empty;
    DataSet dggradetot = new DataSet();
    DataSet dssem = new DataSet();
    DataSet dsmethodgoper = new DataSet();
    DataSet dsmethodgosubj = new DataSet();
    DataSet dsmethodgocriteria = new DataSet();
    DataSet dsmethodgomark = new DataSet();
    DataSet dsuni = new DataSet();
    DataSet ds_sub = new DataSet();
    Hashtable htv = new Hashtable();
    Hashtable htv3 = new Hashtable();
    Hashtable hat5 = new Hashtable();
    static Hashtable htb = new Hashtable();
    static Hashtable htcriteria = new Hashtable();
    static Hashtable htsubjcide = new Hashtable();
    static ArrayList ItemList_gate = new ArrayList();
    static ArrayList Itemindex_gate = new ArrayList();
    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    DateTime Admission_date;
    int tot_ml_spl = 0;
    double cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    int rows_count;
    Boolean splhr_flag = false;
    int tot_ml_spl_fals = 0;
    //vetri.^
    SqlDataAdapter daques = new SqlDataAdapter();
    DataSet dsques = new DataSet();
    static int quecnt = 0;
    int rowquestio = 0;
    static Hashtable hstap = new Hashtable();
    static Hashtable hsanswer = new Hashtable();
    Hashtable hashforpattern = new Hashtable();
    static int savflag = 0;
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql3 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection mysql2 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection csql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection ncon1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_rset = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con2_subj = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_stud_conduct = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_roll = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_all = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_tree = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    FarPoint.Web.Spread.HyperLinkCellType hypertext = new FarPoint.Web.Spread.HyperLinkCellType();
    SqlCommand cmd = new SqlCommand();
    string appno = string.Empty;
    string Reg_no = string.Empty;
    static string path1 = string.Empty;
    Boolean cellclick = false;
    Boolean cellclick4 = false;
    static Boolean semperc = false;
    string semdates = string.Empty;
    string semesterval = string.Empty;
    ArrayList subcode = new ArrayList();
    ArrayList sname = new ArrayList();
    ArrayList staffname = new ArrayList();
    //----------------
    double dum_tage_date = 0;
    double hollyhrs;
    int mm = 1;
    int i, minI, minII, perdayhrs, Ihof, IIhof, fullday;
    double checkpre, totmonth;
    string m7, m2, m3, m4, m5, m6, m1, m8, m9;
    int hour1, hour2, hour3, hour4, hour5, hour6, hour7, hour8, hour9;
    int NoHrs = 0, fnhrs = 0, anhrs = 0, minpresI = 0, minpresII = 0;
    int demfcal, demtcal, cal_from_date = 0, cal_to_date = 0;
    string monthcal;
    DateTime per_from_date = new DateTime();
    DateTime per_to_date = new DateTime();
    DateTime dumm_from_date = new DateTime();
    DataSet ds4 = new DataSet();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int leave_pointer = 0, absent_pointer = 0;
    string date = "", value = "", tempvalue = string.Empty;
    int ObtValue = 0, per_abshrs = 0, njhr = 0, per_perhrs = 0, tot_per_hrs = 0, per_ondu = 0, tot_ondu = 0;
    int per_leave = 0, per_hhday = 0, unmark = 0;
    double Present = 0, leave_point = 0, Leave = 0;
    double Absent = 0, absent_point = 0, Onduty = 0;
    double per_holidate = 0, njdate = 0, workingdays = 0;
    int dum_unmark = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int per_tot_ondu = 0, per_per_hrs = 0;
    double per_njdate = 0, pre_present_date = 0, per_absent_date = 0, pre_ondu_date = 0;
    double pre_leave_date = 0, per_workingdays = 0;
    string dum_tage_hrs;
    double per_tage_date = 0, per_con_hrs = 0, per_tage_hrs = 0, per_dum_unmark = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    //--------------------
    string adegree, abatch, asem, asec, aroll;
    string Att_mark = string.Empty;
    string byr = string.Empty;
    double hollydats;
    int daycount;
    string dd = string.Empty;
    int dat;
    int fm, fyy, fd, tm, tyy, td, fcal, tcal, k;
    double per;
    string roll_no;
    string csem;
    double hours_present = 0;
    double hours_absent = 0;
    double hours_od = 0;
    double hours_total = 0;
    double hours_leave = 0;
    double hours_conduct = 0;
    double hours_pres = 0;
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    //******raja
    string[] split1 = new string[3];
    string b_year = "", deg_code = "", semes = string.Empty;
    string sub = string.Empty;
    //******raja
    int njdate_mng = 0, njdate_evng = 0, mmyycount = 0, moncount = 0;
    int per_holidate_mng = 0, per_holidate_evng = 0, per_workingdays1 = 0;
    //===================20/7/12 PRABHA
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0;
    double per_leave_true = 0;
    Boolean cellclick3 = false;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, notconsider_value = 0, conduct_hour_new = 0, absent_hours = 0;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    int next = 0, count = 0;
    string value_holi_status = "", split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string[] split_holiday_status;
    double dif_date = 0;
    TimeSpan ts;
    string diff_date = string.Empty;
    double dif_date1 = 0;
    Boolean questionflag = false;
    Boolean getquestionflag = false;
    Boolean questionbankflag = false;
    Boolean Cellclick = false;
    //added by annyutha//
    Chart dataf = new Chart();
    AjaxControlToolkit.AccordionPane pn;
    DataSet datauniv = new DataSet();
    DataSet datagrade = new DataSet();
    DataSet datachart = new DataSet();
    DataSet datacre_no = new DataSet();
    DataSet datacam = new DataSet();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    string f = string.Empty;
    string cur_start_date_date = string.Empty;
    string cur_end_date_date = string.Empty;
    string cur_start_date = "", cur_end_date = string.Empty;
    int tot_abs_hrs = 0;
    double attday = 0;
    double atthour = 0;
    DataSet roll_data = new DataSet();
    DateTime from_date, to_date;
    DateTime t_date;
    DateTime f_date;
    decimal avgstudent1 = 0;
    decimal avgstudent2 = 0;
    double avgstudent3 = 0;
    string[] s_code;
    string day_find;
    string[] split_date_time1;
    string[] dummy_split;
    int rollcount = 0, attroll = 0;
    int tval = 0;
    DateTime dummy_from_date, dummy_to_date;
    int f_month_year = 0, t_month_year = 0, rollmonthcount = 0;
    int tempfdate = 0, temptdate = 0, date_day = 0, date_mnth = 0, date_yr = 0, tot_mnth = 0, rollcolumncount = 0;
    string usercode = string.Empty, collegecode = string.Empty, singleuser = string.Empty, group_user = string.Empty;
    string fdate = "", tdate = "", d = "", d1 = "", totoal_c_hrs = string.Empty;
    int total_conducted_hrs = 0, total_attended_hrs = 0, whole_total_conducted_hrs = 0;
    int row = 0, table = 0;
    DateTime s_date;
    string sume = "", sem_start = "", sem_end = string.Empty;
    int sdate = 0, enddate = 0, month = 0, day = 0, year = 0;
    string strsec1;
    string h = "", da1 = "", davalue = string.Empty;
    Hashtable hasspl_tot = new Hashtable();
    Hashtable hasspl_pres = new Hashtable();
    string key_value = "", attnd_val = string.Empty;
    Panel pnchart1;
    Label lblerror;
    Label lblerror1;
    Label lblerror2;
    Label lblerror3;
    Label lblerror4;
    Label lblerror5;
    Boolean chartbol = false;
    string collegecode1 = string.Empty;
    static string hrr = string.Empty;
    DataSet dsStudDetails = new DataSet();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        Page.MaintainScrollPositionOnPostBack = false;
        string get_value = string.Empty;
        string GetType = string.Empty;
        tbfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
        tbto.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Request.QueryString["app"] != "" && Request.QueryString["app"] != null)
        {
            get_value = (Request.QueryString["app"].ToString());
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
        if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
        {
            GetType = (Request.QueryString["Type"].ToString());
        }
        if (GetType == "Student")
        {
            lnkback.Visible = false;
            lnkHome.Visible = false;
        }

        get_value = Decrypt(get_value);
        string[] ffff = get_value.Split(new char[] { '$' });
        for (int y = 0; y <= split1.GetUpperBound(0); y++)
        {
            if (y <= ffff.GetUpperBound(0))
            {
                split1[y] = ffff[y];
            }
        }
        appno = split1[0];

        string appl_id = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + appno + "'");
        string staff_code = d2.GetFunction("select staff_code from staffmaster sm,staff_appl_master s where sm.appl_no=s.appl_no and sm.appl_no='" + appno + "'");
        string selstfimg = d2.GetFunction("select photo from StaffPhoto where (staff_code='" + staff_code + "' or appl_id='" + appl_id + "')");
        if (selstfimg.Trim() != "0" && selstfimg.Trim() != "")
        {
            if (!String.IsNullOrEmpty(staff_code))
                Imagestudent.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staff_code;
            else
                Imagestudent.ImageUrl = "~/Handler/staffphoto.ashx?appl_id=" + appl_id;
        }
        else
        {
            Imagestudent.ImageUrl = "";
        }


        Reg_no = GetFunction("select appl_no  from staff_appl_master where appl_no ='" + appno + "'");
        if (appno != "" && appno != null)
        {
            dsStudDetails.Dispose();
            dsStudDetails.Reset();
            dsStudDetails.Clear();
            //dsStudDetails = da.select_method_wo_parameter("select staff_appl_master.appl_no,appl_name,staff_appl_master.desig_code,staff_appl_master.dept_code,father_name,staff_appl_master.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,staff_type,CONVERT(varchar(20),dateofapply,103) as dateofapply,staff_appl_master.desig_name,staff_appl_master.dept_name,yofexp,bldgrp,subjects,CONVERT(varchar(20),interviewdate,103) as interviewdate,interviewstatus,com_mobileno,qualification,email,appl_id,staffmaster.staff_code,staffmaster.resign,staffmaster.settled,staffmaster.Discontinue from staff_appl_master inner join Desig_Master on Desig_Master.desig_code = staff_appl_master.desig_code and desig_master.collegecode = staff_appl_master.college_code inner join hrdept_master on hrdept_master.dept_code = staff_appl_master.dept_code and hrdept_master.college_code = staff_appl_master.college_code left join StaffMaster on StaffMaster.appl_no = staff_appl_master.appl_no and StaffMaster.college_code = staff_appl_master.college_code where staff_appl_master.appl_no='" + appno + "'", "text");

            dsStudDetails = da.select_method_wo_parameter("select s.appl_no,appl_name,st.staff_code,dm.desig_name,sc.category_name, hrd.dept_name,isnull(comm_address,'') as comm_address ,isnull(per_address,'') as per_address,isnull(martial_status,'') as martial_status,isnull(father_name,'') as father_name,s.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,st.stftype,CONVERT(varchar(20),dateofapply,103) as dateofapply,CONVERT(varchar(20),join_date,103) as join_date,CONVERT(varchar(20),retr_date,103) as retr_date,dm.desig_name,hrd.dept_name,yofexp,bldgrp,religion,Caste,Community,Nationality,CONVERT(varchar(20),interviewdate,103) as interviewdate,interviewstatus,com_mobileno,qualification,email,appl_id,sm.staff_code,resign,settled,Discontinue from staff_appl_master s,staffmaster sm,stafftrans st,Desig_Master dm,hrdept_master hrd,staffcategorizer sc where s.college_code=sm.college_code and sm.college_code=dm.collegeCode and sm.college_code=hrd.college_code and sm.college_code=sc.college_code and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.category_code=sc.category_code and st.dept_code=hrd.dept_code and s.appl_no=sm.appl_no and sm.appl_no='" + appno + "' and latestrec='1'", "text");

        }
        if (dsStudDetails.Tables.Count > 0)
        {
            if (dsStudDetails.Tables[0].Rows.Count > 0)
            {
                lblstaffname.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["appl_name"]);
                nameprint = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["appl_name"]);
                lbldept.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["dept_name"]);
                departmentprint = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["dept_name"]);
                int lenth = lbldept.Text.Length;

                // Session["degree_code"] = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["desig_code"]);
                lbldesig.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["desig_name"]);
                semprint = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["desig_name"]);
                //batchprint = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["batch_year"]);
                // lblcol.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["College_Name"]);
                // Session["college_code"] = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["college_code"]);
                collegecode1 = Convert.ToString(Session["collegecode"]);
                lblstaffcodeprint.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["staff_code"]);
                lblstafftypeprint.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["stftype"]);

                if (collegecode1 != "")
                {

                    string coll_name = d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'");
                    lblcol.Text = Convert.ToString(coll_name);
                }
            }

        }


    }



    public byte[] ScrambleKey
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                // Use existing key if non provided
                key = ScrambleKey;
            }
            Session["ScrambleKey"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleKey"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateKey();
                key = rc2.Key;
                Session["ScrambleKey"] = key;
            }
            return key;
        }
    }

    // Initialization vector management for scrambling support
    public byte[] ScrambleIV
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                key = ScrambleIV;
            }
            Session["ScrambleIV"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleIV"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateIV();
                key = rc2.IV;
                Session["ScrambleIV"] = key;
            }
            return key;
        }
    }


    public string Decrypt(string scrambledMessage)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

        string b64mod = HttpUtility.UrlDecode(scrambledMessage);

        string b64 = b64mod.Replace('@', '+');
        byte[] encrypted = Convert.FromBase64String(b64);
        ICryptoTransform decryptor = rc2CSP.CreateDecryptor(ScrambleKey, ScrambleIV);
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[encrypted.Length - 4];
        byte[] length = new byte[4];
        csDecrypt.Read(length, 0, 4);
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
        int len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);
        return textConverter.GetString(fromEncrypt).Substring(0, len);
    }


    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr = string.Empty;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataReader drnew;
        SqlCommand cd = new SqlCommand(sqlstr);
        cd.Connection = getsql;
        drnew = cd.ExecuteReader();
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
    protected void lblogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void lnkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default_LoginPage.aspx");

    }
    protected void lnkback_Click(object sender, EventArgs e)
    {

        //if (Convert.ToString(Session["studentmentor"]) == "studentmentor")
        //{
        //    Session["studentmentor"] = null;
        //    Response.Redirect("~/StudentMod/StudentMentorReport.aspx");
        //}
        //else
        //{
        //    if (split1[1] == null)
        //    {
        //        Response.Redirect("~/OfficeMOD/About.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("stud_login_rollno.aspx?");
        //    }
        //}

        Response.Redirect("~/OfficeMOD/StaffUniversalReport.aspx");
    }
    protected void Buttonbiodata_Click(object sender, EventArgs e)
    {
        try
        {
            FarPoint.Web.Spread.TextCellType objtext = new FarPoint.Web.Spread.TextCellType();
            initpersonal();
            Fpspersonal.Visible = true;
            string query = string.Empty;
            //query = "select s.appl_no,appl_name,st.staff_code,dm.desig_name,hrd.dept_name,isnull(comm_address,'') as comm_address ,isnull(per_address,'') as per_address,isnull(martial_status,'') as martial_status,isnull(father_name,'') as father_name,s.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,st.stftype,CONVERT(varchar(20),dateofapply,103) as dateofapply,CONVERT(varchar(20),join_date,103) as join_date,CONVERT(varchar(20),retr_date,103) as retr_date,dm.desig_name,hrd.dept_name,yofexp,bldgrp,religion,Caste,Community,Nationality,CONVERT(varchar(20),interviewdate,103) as interviewdate,interviewstatus,com_mobileno,qualification,email,appl_id,sm.staff_code,resign,settled,Discontinue from staff_appl_master s,staffmaster sm,stafftrans st,Desig_Master dm,hrdept_master hrd where s.college_code=sm.college_code and sm.college_code=dm.collegeCode and sm.college_code=hrd.college_code and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hrd.dept_code and s.appl_no=sm.appl_no and sm.appl_no='" + appno + "' and latestrec='1'";

            query = "select s.appl_no,appl_name,st.staff_code,dm.desig_name,hrd.dept_name,isnull(comm_address,'') as comm_address ,isnull(per_address,'') as per_address,isnull(martial_status,'') as martial_status,isnull(father_name,'') as father_name,s.mid_name,sex,CONVERT(varchar(20),date_of_birth,103) as date_of_birth,st.stftype,CONVERT(varchar(20),dateofapply,103) as dateofapply,CONVERT(varchar(20),join_date,103) as join_date,CONVERT(varchar(20),retr_date,103) as retr_date,dm.desig_name,hrd.dept_name,yofexp,(select isnull(Textval,'') from textvaltable where TextCriteria='bgrou' and LTRIM(RTRIM(isnull(textvaltable.TextCode,'')))=ltrim(rtrim(isnull(s.bldgrp,''))) )as bldgrp, (select isnull(Textval,'') from textvaltable where TextCriteria='relig' and LTRIM(RTRIM(isnull(textvaltable.TextCode,'')))=ltrim(rtrim(isnull(s.religion,'')))) as religion,(select Textval from textvaltable where TextCriteria='caste' and ltrim(rtrim(isnull(textvaltable.TextCode,'')))=ltrim(rtrim(isnull(s.Caste,'')))) as Caste ,(select isnull(Textval,'') from textvaltable where TextCriteria='comm' and ltrim(rtrim(isnull(textvaltable.TextCode,'')))=ltrim(rtrim(isnull(s.Community,'')))) as Community,(select isnull(Textval,'') from textvaltable where TextCriteria='natio' and ltrim(rtrim(isnull(textvaltable.TextCode,'')))=ltrim(rtrim(isnull(s.Nationality,''))))as Nationality,CONVERT(varchar(20),interviewdate,103) as interviewdate,interviewstatus,com_mobileno,qualification,email,appl_id,sm.staff_code,resign,settled,Discontinue from staff_appl_master s,staffmaster sm,stafftrans st,Desig_Master dm,hrdept_master hrd where s.college_code=sm.college_code and sm.college_code=dm.collegeCode and sm.college_code=hrd.college_code and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hrd.dept_code and s.appl_no=sm.appl_no and sm.appl_no='" + appno + "' and latestrec='1'";

            Fpspersonal.Visible = true;
            Fpspersonal.Sheets[0].ColumnCount = 7;
            Fpspersonal.ColumnHeader.Visible = false;
            Fpspersonal.RowHeader.Visible = false;
            Fpspersonal.CommandBar.Visible = false;
            Fpspersonal.Sheets[0].DefaultRowHeight = 20;
            Fpspersonal.TitleInfo.Visible = true;
            Fpspersonal.TitleInfo.Text = "Staff Bio Data";
            Fpspersonal.TitleInfo.Font.Size = FontUnit.Large;
            Fpspersonal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fpspersonal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].GridLines = GridLines.None;
            Fpspersonal.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Columns[0].Locked = true;
            Fpspersonal.Columns[1].Locked = true;
            Fpspersonal.Columns[2].Locked = true;
            Fpspersonal.Columns[3].Locked = true;
            Fpspersonal.Columns[4].Locked = true;
            Fpspersonal.Columns[5].Locked = true;

            Fpspersonal.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;

            Fpspersonal.Sheets[0].Columns[0].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[1].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[2].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[3].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[4].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[5].CellType = new FarPoint.Web.Spread.TextCellType();
            Fpspersonal.Sheets[0].Columns[6].CellType = new FarPoint.Web.Spread.TextCellType();

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].SpanModel.Add(Fpspersonal.Sheets[0].RowCount - 1, 0, 1, Fpspersonal.Sheets[0].ColumnCount);
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Personal Information";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
            Fpspersonal.Columns[0].Font.Bold = true;
            Fpspersonal.Columns[0].Width = 200;
            Fpspersonal.Columns[1].Width = 10;
            Fpspersonal.Columns[2].Width = 270;
            Fpspersonal.Columns[3].Width = 180;
            Fpspersonal.Columns[3].Font.Bold = true;
            Fpspersonal.Columns[4].Width = 10;
            Fpspersonal.Columns[5].Width = 300;

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Staff Name";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Department";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";
            Fpspersonal.Sheets[0].Rows[Fpspersonal.Sheets[0].RowCount - 1].VerticalAlign = VerticalAlign.Top;


            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Application No";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Designation";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Top;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Staff Code";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Staff Type";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Join Date";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Retire Date";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Sex";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Religion";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";


            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Date of Birth";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Caste";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Year Of Experience";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Community";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Blood Group";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = "Nationality";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";


            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].SpanModel.Add(Fpspersonal.Sheets[0].RowCount - 1, 0, 1, Fpspersonal.Sheets[0].ColumnCount);
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Family Information";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;


            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = " Father's Name";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = " Marital Status";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";



            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].SpanModel.Add(Fpspersonal.Sheets[0].RowCount - 1, 0, 1, Fpspersonal.Sheets[0].ColumnCount);
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Address Information";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
            Fpspersonal.Rows[Fpspersonal.Sheets[0].RowCount - 1].VerticalAlign = VerticalAlign.Top;

            Fpspersonal.Sheets[0].RowCount++;
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 0].Text = "Communication Address";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 3].Text = " Permanant Address";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 1].Text = ":";
            Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 4].Text = ":";

            con.Open();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataReader rbio = cmd.ExecuteReader();
            if (rbio.Read())
            {
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(rbio["comm_address"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(rbio["per_address"]);

                string maritalstatus = Convert.ToString(rbio["martial_status"]);
                if (maritalstatus.Trim() == "0")
                {
                    maritalstatus = "Single";
                
                }
                else if (maritalstatus.Trim() == "1")
                {
                    maritalstatus = "Married";
                }
                else if (maritalstatus.Trim() == "2")
                {
                    maritalstatus = "Widowed";
                
                }
                else if (maritalstatus.Trim() == "3")
                {
                    maritalstatus = "Divorced";
                }
                else if (maritalstatus.Trim() == "4")
                {
                    maritalstatus = "Separated";
                }

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 3, 2].Text = Convert.ToString(rbio["father_name"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 3, 5].Text = Convert.ToString(maritalstatus);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 5, 2].Text = Convert.ToString(rbio["bldgrp"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 5, 5].Text = Convert.ToString(rbio["Nationality"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 6, 2].Text = Convert.ToString(rbio["yofexp"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 6, 5].Text = Convert.ToString(rbio["Community"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 7, 2].Text = Convert.ToString(rbio["date_of_birth"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 7, 5].Text = Convert.ToString(rbio["Caste"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 8, 2].Text = Convert.ToString(rbio["sex"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 8, 5].Text = Convert.ToString(rbio["religion"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 9, 2].Text = Convert.ToString(rbio["join_date"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 9, 5].Text = Convert.ToString(rbio["retr_date"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 10, 2].Text = Convert.ToString(rbio["staff_code"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 10, 5].Text = Convert.ToString(rbio["stftype"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 11, 2].Text = Convert.ToString(rbio["appl_no"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 11, 5].Text = Convert.ToString(rbio["desig_name"]);

                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 12, 2].Text = Convert.ToString(rbio["appl_name"]);
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 12, 5].Text = Convert.ToString(rbio["dept_name"]);




                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 1, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 2, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 2, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 3, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 3, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 4, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 4, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 5, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 5, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 6, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 6, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 7, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 7, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 8, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 8, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 9, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 9, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 10, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 10, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 11, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 11, 5].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 12, 2].CellType = txt;
                Fpspersonal.Sheets[0].Cells[Fpspersonal.Sheets[0].RowCount - 12, 5].CellType = txt;
                con.Close();



                Fpspersonal.Width = 970;
                Fpspersonal.Height = 600;
            }


        }
        catch (Exception ex)
        {

        }
    }
    protected void Buttoncontact_Click(object sender, EventArgs e)
    {
        try
        {
            initpersonal();
            Fpspersonal.Visible = true;
            string query = string.Empty;
            query = "select isnull(comm_address,'')+ ' ' +isnull(com_pincode,'')+' '+ISNULL(ccity,'')+ISNULL(cstate,'') as comm_address,isnull(per_address,'')+' '+ISNULL(per_pincode,'')+' '+ISNULL(pcity,'')+' '+ISNULL(pstate,'') as per_address,com_mobileno,per_mobileno,email from staff_appl_master s,staffmaster sm where sm.appl_no=s.appl_no and sm.appl_no='" + appno + "'";
            Fpspersonal.Sheets[0].ColumnCount = 5;
            Fpspersonal.Sheets[0].RowCount = 25;
            Fpspersonal.ColumnHeader.Visible = false;
            Fpspersonal.RowHeader.Visible = false;
            Fpspersonal.CommandBar.Visible = false;
            Fpspersonal.Sheets[0].DefaultColumnWidth = 50;
            Fpspersonal.Sheets[0].DefaultRowHeight = 20;
            Fpspersonal.TitleInfo.Visible = true;
            Fpspersonal.TitleInfo.Text = "Staff Contacts Details";
            Fpspersonal.TitleInfo.Font.Size = FontUnit.Large;
            Fpspersonal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fpspersonal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            Fpspersonal.Sheets[0].GridLines = GridLines.None;
            Fpspersonal.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fpspersonal.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fpspersonal.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fpspersonal.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
            Fpspersonal.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
            Fpspersonal.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;

            Fpspersonal.Sheets[0].Cells[0, 1].Text = "Communication Address";
            Fpspersonal.Sheets[0].Cells[6, 1].Text = "permanent Address";
            Fpspersonal.Sheets[0].Cells[12, 1].Text = "Communication Mobile Number";
            Fpspersonal.Sheets[0].Cells[14, 1].Text = "permanent Mobile Number";
            Fpspersonal.Sheets[0].Cells[16, 1].Text = "Email Id";

            Fpspersonal.Sheets[0].Cells[0, 2].Text = ":";
            Fpspersonal.Sheets[0].Cells[6, 2].Text = ":";
            Fpspersonal.Sheets[0].Cells[12, 2].Text = ":";
            Fpspersonal.Sheets[0].Cells[14, 2].Text = ":";
            Fpspersonal.Sheets[0].Cells[16, 2].Text = ":";
            Fpspersonal.Columns[0].Font.Bold = true;
            Fpspersonal.Columns[1].Font.Bold = true;
            Fpspersonal.Columns[0].Width = 5;
            Fpspersonal.Columns[1].Width = 200;
            Fpspersonal.Columns[2].Width = 20;
            Fpspersonal.Columns[3].Width = 300;
            Fpspersonal.Columns[4].Width = 20;

            int i = 0;
            string contact_adres = string.Empty;
            string perm_adres = string.Empty;
            cmd.CommandText = query;
            cmd.Connection = con;
            con.Open();
            SqlDataReader rad = cmd.ExecuteReader();
            if (rad.Read())
            {
                Fpspersonal.Sheets[0].SpanModel.Add(0, 3, 6, 1);
                Fpspersonal.Sheets[0].SpanModel.Add(6, 3, 6, 1);
                Fpspersonal.Sheets[0].Cells[0, 3].Text = Convert.ToString(rad["comm_address"]);
                Fpspersonal.Sheets[0].Cells[6, 3].Text = Convert.ToString(rad["per_address"]);
                Fpspersonal.Sheets[0].Cells[12, 3].Text = Convert.ToString(rad["com_mobileno"]);
                Fpspersonal.Sheets[0].Cells[14, 3].Text = Convert.ToString(rad["per_mobileno"]);
                Fpspersonal.Sheets[0].Cells[16, 3].Text = Convert.ToString(rad["email"]);
                Fpspersonal.Columns[3].VerticalAlign = VerticalAlign.Top;
                Fpspersonal.Width = 550;
                Fpspersonal.Height = 550;

            }
            else
            {
                Fpspersonal.Sheets[0].ColumnCount = 0;
                Fpspersonal.Sheets[0].RowCount = 0;
                Fpspersonal.Sheets[0].ColumnCount = 4;
                Fpspersonal.Sheets[0].RowCount = 1;
                Fpspersonal.Sheets[0].SpanModel.Add(0, 0, 1, 4);
                Fpspersonal.Height = 50;
                Fpspersonal.Width = 600;
                Fpspersonal.ColumnHeader.Visible = false;
                Fpspersonal.Sheets[0].Cells[0, 0].Text = "No information Available";
                Fpspersonal.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspersonal.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspersonal.Sheets[0].Cells[0, 0].ForeColor = Color.Blue;
                Fpspersonal.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspersonal.Width = 550;
                Fpspersonal.Height = 40;

            }
            rad.Close();
            con.Close();
            Fpspersonal.Sheets[0].PageSize = 26;
            Fpspersonal.SaveChanges();



        }
        catch (Exception ex)
        {

        }
    }

    protected void Buttonbiometricatt_Click(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = false;
            lbldatefrom.Visible = true;
            txtfromdate.Visible = true;
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lbltodate.Visible = true;
            txttodate.Visible = true;
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btn_go.Visible = false;
            btn_bio_go.Visible = true;
            Fpsattendence.Visible = false;
            lblError.Visible = false;
            lblError.Text = "";


        }
        catch (Exception ex)
        {

        }
    }

    protected void Buttongeneralatt_Click(object sender, EventArgs e)
    {
        try
        {
            lbldatefrom.Visible = true;
            txtfromdate.Visible = true;

            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbltodate.Visible = true;
            txttodate.Visible = true;
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btn_go.Visible = true;
            btn_bio_go.Visible = false;
            Fpsattendence.Visible = false;
            lblError.Visible = false;
            lblError.Text = "";
        }
        catch (Exception ex)
        {

        }
    }
    protected void Buttonperfomance_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Dispose();
            labeldatevalid.Visible = false;
            labeldatevalid.Text = "";

            ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int p = d2.insert_method("IF not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbl_staff_topper' AND COLUMN_NAME = 'user_code') alter table tbl_staff_topper add user_code nvarchar(25)", hat, "text");
            }
            else
            {
                int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int,user_code nvarchar(25))", hat, "text");
            }
            int strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
            DateTime dtf = new DateTime();
            DateTime dtt = new DateTime();
            if (tbfrom.Text != "" && tbto.Text != "")
            {
                string fadte = tbfrom.Text.ToString();
                string[] spf = fadte.Split('/');
                dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                string tdate = tbto.Text.ToString();
                string[] spt = tdate.Split('/');
                dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
                if (dtt < dtf)
                {
                    labeldatevalid.Visible = true;
                    labeldatevalid.Text = "From Date Must Be Less Than Or Equal To Date";
                    return;
                }
            }
            else
            {
                labeldatevalid.Visible = true;
                labeldatevalid.Text = "Please Enter From Date and To Date";
                return;

            }
            Boolean setflag = false;
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            Fpspermomance.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspermomance.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpspermomance.Sheets[0].DefaultStyle.Font.Bold = true;
            Fpspermomance.Sheets[0].SheetCorner.RowCount = 1;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            Fpspermomance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            Fpspermomance.Sheets[0].AllowTableCorner = true;
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspermomance.Sheets[0].RowHeader.Visible = false;
            Fpspermomance.CommandBar.Visible = false;
            Fpspermomance.Sheets[0].RowCount = 0;
            Fpspermomance.Sheets[0].ColumnCount = 0;
            Fpspermomance.Sheets[0].SheetCorner.ColumnCount = 0;
            Fpspermomance.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspermomance.Sheets[0].ColumnCount = 12;
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //  Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
            //  Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation";
            //Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            // Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Code";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Details";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Exam";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total No.of Students";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Appear";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Passed";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Absent";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Fail";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Pass %";
            Fpspermomance.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Over All Pass %";

            Fpspermomance.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
            Fpspermomance.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;
            //  Fpspermomance.Sheets[0].Columns[12].VerticalAlign = VerticalAlign.Middle;
            //  Fpspermomance.Sheets[0].Columns[13].VerticalAlign = VerticalAlign.Middle;
            //  Fpspermomance.Sheets[0].Columns[14].VerticalAlign = VerticalAlign.Middle;
            //  Fpspermomance.Sheets[0].Columns[15].VerticalAlign = VerticalAlign.Middle;


            Fpspermomance.Sheets[0].Columns[0].Width = 50;
            //   Fpspermomance.Sheets[0].Columns[1].Width = 100;
            //   Fpspermomance.Sheets[0].Columns[2].Width = 100;
            //   Fpspermomance.Sheets[0].Columns[3].Width = 100;
            //   Fpspermomance.Sheets[0].Columns[4].Width = 100;
            Fpspermomance.Sheets[0].Columns[1].Width = 300;
            Fpspermomance.Sheets[0].Columns[2].Width = 100;
            Fpspermomance.Sheets[0].Columns[3].Width = 100;
            Fpspermomance.Sheets[0].Columns[4].Width = 100;
            Fpspermomance.Sheets[0].Columns[5].Width = 50;
            Fpspermomance.Sheets[0].Columns[6].Width = 50;
            Fpspermomance.Sheets[0].Columns[7].Width = 50;
            Fpspermomance.Sheets[0].Columns[8].Width = 50;
            Fpspermomance.Sheets[0].Columns[9].Width = 50;
            Fpspermomance.Sheets[0].Columns[10].Width = 50;
            Fpspermomance.Sheets[0].Columns[11].Width = 50;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.DeepSkyBlue;
            Fpspermomance.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            Fpspermomance.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspermomance.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspermomance.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspermomance.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            string staff_code = d2.GetFunction("select staff_code from staffmaster where appl_no='" + appno + "'");
            string strgetexam = "select c.criteria,c.Criteria_no,e.exam_code,e.batch_year,e.sections,e.subject_no,c.syll_code,e.min_mark from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no ";
            strgetexam = strgetexam + " select distinct ed.batch_year,ed.degree_code,ed.current_semester,ed.Exam_Month,ed.Exam_year,ed.exam_code,m.subject_no from Exam_Details ed,mark_entry m where ed.exam_code=m.exam_code";
            DataSet dsexam = d2.select_method_wo_parameter(strgetexam, "Text");

            strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
            string strqureystaff = "select distinct sy.Batch_Year,sy.degree_code,sy.semester,st.Sections,sy.syll_code,st.staff_code,s.subject_name,s.subject_no from seminfo si,syllabus_master sy,sub_sem ss,subject s,staff_selector st where si.batch_year=sy.Batch_Year and si.degree_code=sy.degree_code and si.semester=sy.semester and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code  and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and st.subject_no=s.subject_no and st.batch_year=si.batch_year  and st.batch_year=sy.Batch_Year and ss.promote_count=1 and si.start_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "' and st.staff_code in('" + staff_code + "')";

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strqureystaff, "Text");

            for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
            {
                string staffname = string.Empty;
                string staff = ds.Tables[0].Rows[s]["staff_code"].ToString();
                string subject = ds.Tables[0].Rows[s]["subject_name"].ToString();
                string degreecode = ds.Tables[0].Rows[s]["degree_code"].ToString();
                string subjectno = ds.Tables[0].Rows[s]["subject_no"].ToString();
                string batch = ds.Tables[0].Rows[s]["batch_year"].ToString();
                string syllcode = ds.Tables[0].Rows[s]["syll_code"].ToString();
                string sections = ds.Tables[0].Rows[s]["sections"].ToString();
                string semester = ds.Tables[0].Rows[s]["semester"].ToString();
                string departmentvalue = string.Empty;
                string sp_section = string.Empty;
                if (sections.ToString().Trim() != "-1" && sections.ToString().Trim() != "" && sections != null)
                {
                    sp_section = sections;
                    sections = "and r.sections='" + sections + "'";
                }
                else
                {
                    sections = string.Empty;
                }

                if (ddlexam.SelectedItem.ToString() != "External")
                {
                    DataView dvint = new DataView();
                    if (dsexam.Tables.Count > 0 && dsexam.Tables[0].Rows.Count > 0)
                    {
                        dsexam.Tables[0].DefaultView.RowFilter = "syll_code='" + syllcode + "' and subject_no='" + subjectno + "' and batch_year='" + batch + "' and sections='" + sp_section + "'";
                        dvint = dsexam.Tables[0].DefaultView;
                    }
                    for (int ine = 0; ine < dvint.Count; ine++)
                    {
                        string examname = dvint[ine]["criteria"].ToString();
                        string examcode = dvint[ine]["exam_code"].ToString();
                        string minmarks = dvint[ine]["min_mark"].ToString();
                        string totalstudent = string.Empty;
                        string staffvaluequery = "select distinct count(s.roll_no) as total from subjectchooser s,registration r where r.roll_no=s.roll_no and r.cc=0 and r.exam_flag<>'debar' and r.delflag=0 and subject_no='" + subjectno + "' and r.batch_year=" + batch + " and s.semester=" + semester + " and r.degree_code=" + degreecode + " " + sections + "";
                        hat.Clear();
                        DataSet dsstaff = d2.select_method(staffvaluequery, hat, "Text");
                        if (dsstaff.Tables[0].Rows.Count > 0)
                        {
                            totalstudent = dsstaff.Tables[0].Rows[0]["total"].ToString();
                        }
                        hat.Clear();
                        hat.Add("exam_code", examcode);
                        hat.Add("min_marks", minmarks);
                        hat.Add("section", sp_section);
                        DataSet dsexamdetails = d2.select_method("Proc_All_Subject_Details", hat, "sp");
                        if (dsexamdetails.Tables.Count > 0 && dsexamdetails.Tables[0].Rows.Count > 0)
                        {
                            string appear = dsexamdetails.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                            string passcount = dsexamdetails.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                            string failcount = dsexamdetails.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                            string absent = dsexamdetails.Tables[9].Rows[0]["Absent_count"].ToString();
                            int totalcount = Convert.ToInt32(appear) + Convert.ToInt32(passcount) + Convert.ToInt32(failcount);
                            if (totalcount != 0)
                            {
                                string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail,ext_appear,isExternal,user_code) values ";
                                insertallexam = "" + insertallexam + " ('" + staff + "','" + staffname + "','" + sp_section + "','" + subjectno + "','" + examname + "'," + totalstudent + "," + appear + "," + passcount + "," + failcount + ",'" + absent + "',0,'" + usercode + "')";
                                int value = d2.insert_method(insertallexam, hat, "Text");
                            }
                        }
                    }
                }
                //internal External  
                if (ddlexam.SelectedItem.ToString() != "Internal")
                {
                    string strgetexamquery = "select ed.Exam_Month,ed.Exam_year,ed.exam_code from Exam_Details ed,mark_entry m where m.exam_code=ed.exam_code and ed.batch_year='" + batch + "' and ed.degree_code='" + degreecode + "' and ed.current_semester='" + semester + "' and m.subject_no='" + subjectno + "'";
                    DataSet dsexamquery = d2.select_method_wo_parameter(strgetexamquery, "Text");
                    DataView dvext = new DataView();
                    if (dsexam.Tables.Count > 1 && dsexam.Tables[1].Rows.Count > 0)
                    {
                        dsexam.Tables[1].DefaultView.RowFilter = "batch_year='" + batch + "' and degree_code='" + degreecode + "' and current_semester='" + semester + "' and subject_no='" + subjectno + "'";
                        dvext = dsexam.Tables[1].DefaultView;
                    }
                    if (dvext.Count > 0)
                    {
                        string examcode = dvext[0]["exam_code"].ToString();
                        string exammonth = dvext[0]["Exam_month"].ToString();
                        string examyear = dvext[0]["Exam_Year"].ToString();
                        if (exammonth == "1")
                            exammonth = "Jan";
                        else if (exammonth == "2")
                            exammonth = "Feb";
                        else if (exammonth == "3")
                            exammonth = "Mar";
                        else if (exammonth == "4")
                            exammonth = "Apr";
                        else if (exammonth == "5")
                            exammonth = "May";
                        else if (exammonth == "6")
                            exammonth = "Jun";
                        else if (exammonth == "7")
                            exammonth = "Jul";
                        else if (exammonth == "8")
                            exammonth = "Aug";
                        else if (exammonth == "9")
                            exammonth = "Sep";
                        else if (exammonth == "10")
                            exammonth = "Oct";
                        else if (exammonth == "11")
                            exammonth = "Nov";
                        else if (exammonth == "12")
                            exammonth = "Dec";
                        string examname = examyear + " / " + exammonth;
                        hat.Clear();
                        hat.Add("Exam_code", examcode);
                        hat.Add("Subject_no", subjectno);
                        DataSet dsexterdetail = d2.select_method("Sp_External_Student_Details", hat, "sp");
                        if (dsexterdetail.Tables.Count > 0 && dsexterdetail.Tables[0].Rows.Count > 0)
                        {
                            string Total = dsexterdetail.Tables[5].Rows[0]["Total"].ToString();
                            string Pass = dsexterdetail.Tables[2].Rows[0]["Pass_Count"].ToString();
                            string Fail = dsexterdetail.Tables[3].Rows[0]["Fail_Count_With_AB"].ToString();
                            string Appear = dsexterdetail.Tables[0].Rows[0]["Present_count"].ToString();
                            int totalcount = Convert.ToInt32(Total) + Convert.ToInt32(Pass) + Convert.ToInt32(Fail) + Convert.ToInt32(Appear);
                            if (totalcount != 0)
                            {
                                string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail,isExternal,user_code) values ";
                                insertallexam = "" + insertallexam + " ('" + staff + "','" + staffname + "','" + departmentvalue + "','" + subjectno + "','" + examname + "'," + Total + "," + Appear + "," + Pass + "," + Fail + ",1,'" + usercode + "')";
                                int value = d2.insert_method(insertallexam, hat, "Text");
                            }
                        }
                    }
                }
            }

            Hashtable hatstaff = new Hashtable();
            string type = string.Empty;
            if (ddlexam.SelectedItem.ToString() == "Internal")
            {
                type = " and isExternal='0'";
            }
            else if (ddlexam.SelectedItem.ToString() == "External")
            {
                type = " and isExternal='1'";
            }
            ds.Dispose();
            ds.Reset();

            string strstaffperformancequery = "select distinct sm.staff_code,sm.staff_name,h.dept_name,d.desig_name,s.subject_code,s.subject_name,s.subject_no,sy.Batch_Year,c.Course_Name,dep.Dept_Name as department,sy.degree_code,sy.semester,ss.Sections,ts.in_total,ts.in_appear,ts.in_pass,ts.in_fail,ts.ext_appear,ts.internal_exam_type from tbl_staff_topper ts,staffmaster sm,stafftrans st,hrdept_master h,desig_master d,subject s,syllabus_master sy,staff_selector ss,Degree de,Course c,Department dep where st.staff_code=sm.staff_code and sm.staff_code=ts.staff_code and st.staff_code=st.staff_code and sm.staff_code=ss.staff_code and st.staff_code=ss.staff_code and ss.staff_code=ts.staff_code and s.subject_no=ss.subject_no and sm.college_code=h.college_code and sm.college_code=d.collegeCode and de.Degree_Code=sy.degree_code and de.Dept_Code=dep.Dept_Code and c.Course_Id=de.Course_Id and st.dept_code=h.dept_code and st.desig_code=d.desig_code and ts.subject=s.subject_no and s.syll_code=sy.syll_code and ss.Sections=ts.degree and st.latestrec='1' and ts.user_code='" + usercode + "' " + type + " order by sm.staff_code,sy.Batch_Year,c.Course_Name,department";
            ds = d2.select_method_wo_parameter(strstaffperformancequery, "text");
            string getpervcstaff = "select distinct round(sum(round(isnull(in_pass,0)/isnull(in_appear,0)*100,2))/(count(staff_code)*100)*100,2) as passpercentage,count(staff_code),staff_code,subject,degree from tbl_staff_topper where in_appear is not null " + type + " and isnull(in_pass,'0')<>'0' and isnull(in_appear,0)<>'0' and user_code='" + usercode + "' group by staff_code,subject,degree order by staff_code,degree,subject";
            DataSet dsstafffper = d2.select_method_wo_parameter(getpervcstaff, "text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpspermomance.Visible = true;
                Fpspermomance.Sheets[0].AutoPostBack = true;

                int sno = 0;
                if (dsstafffper.Tables.Count > 0 && dsstafffper.Tables[0].Rows.Count > 0)
                {
                    for (int st = 0; st < dsstafffper.Tables[0].Rows.Count; st++)
                    {
                        string subjectno = dsstafffper.Tables[0].Rows[st]["subject"].ToString();
                        string staffcode = dsstafffper.Tables[0].Rows[st]["staff_code"].ToString();
                        string overperc = dsstafffper.Tables[0].Rows[st]["passpercentage"].ToString();
                        string secval = dsstafffper.Tables[0].Rows[st]["degree"].ToString();
                        if (secval.Trim() != "" && secval.Trim() != "0" && secval.Trim() != "-1")
                        {
                            secval = " and sections='" + secval + "'";
                        }

                        DataView dvstaffco = new DataView();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "' and subject_no='" + subjectno + "' " + secval + "";
                            dvstaffco = ds.Tables[0].DefaultView;
                        }
                        Double paper = 0;
                        if (dvstaffco.Count > 0)
                        {
                            setflag = true;
                            for (int spe = 0; spe < dvstaffco.Count; spe++)
                            {
                                sno++;
                                string degeredeatisl = dvstaffco[spe]["Batch_Year"].ToString() + " - " + dvstaffco[spe]["Course_Name"].ToString() + " - " + dvstaffco[spe]["department"].ToString() + " - " + dvstaffco[spe]["semester"].ToString();
                                if (dvstaffco[spe]["Sections"].ToString().Trim() != "-1" && dvstaffco[spe]["Sections"].ToString().Trim() != "")
                                {
                                    degeredeatisl = degeredeatisl + " - " + dvstaffco[spe]["Sections"].ToString();
                                }
                                Double total = 0;
                                Double apperar = 0;
                                Double pass = 0;
                                Double fail = 0;
                                Double absent = 0;
                                Double appear = 0;
                                string exam = dvstaffco[spe]["internal_exam_type"].ToString();
                                Fpspermomance.Sheets[0].RowCount++;

                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dvstaffco[spe]["dept_name"].ToString();
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dvstaffco[spe]["desig_name"].ToString();
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dvstaffco[spe]["staff_name"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = staffcode;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 1].Text = degeredeatisl;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 2].Text = dvstaffco[spe]["subject_name"].ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 3].Text = dvstaffco[spe]["subject_code"].ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 4].Text = exam;
                                if (dvstaffco[spe]["ext_appear"].ToString().Trim() != "")
                                {
                                    absent = Convert.ToDouble(dvstaffco[spe]["ext_appear"].ToString());
                                }
                                if (dvstaffco[spe]["in_appear"].ToString().Trim() != "")
                                {
                                    appear = Convert.ToDouble(dvstaffco[spe]["in_appear"].ToString());
                                }
                                if (dvstaffco[spe]["in_total"].ToString().Trim() != "")
                                {
                                    total = Convert.ToDouble(dvstaffco[spe]["in_total"].ToString());
                                }
                                if (dvstaffco[spe]["in_appear"].ToString().Trim() != "")
                                {
                                    apperar = Convert.ToDouble(dvstaffco[spe]["in_appear"].ToString());
                                }
                                if (dvstaffco[spe]["in_pass"].ToString().Trim() != "")
                                {
                                    pass = Convert.ToDouble(dvstaffco[spe]["in_pass"].ToString());
                                }
                                if (dvstaffco[spe]["in_fail"].ToString().Trim() != "")
                                {
                                    fail = Convert.ToDouble(dvstaffco[spe]["in_fail"].ToString());
                                }
                                Double passper = pass / apperar * 100;
                                if (passper > 100)
                                {
                                    passper = 100;
                                }
                                passper = Math.Round(passper, 2, MidpointRounding.AwayFromZero);
                                paper = paper + passper;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 5].Text = total.ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 6].Text = appear.ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 7].Text = pass.ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 8].Text = absent.ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 9].Text = fail.ToString();
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 10].Text = passper.ToString();

                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;


                            }

                            paper = paper / (Convert.ToDouble(dvstaffco.Count) * 100) * 100;
                            paper = Math.Round(paper, 2, MidpointRounding.AwayFromZero);
                            Fpspermomance.Sheets[0].SpanModel.Add(Fpspermomance.Sheets[0].RowCount - Convert.ToInt32(dvstaffco.Count), 11, Convert.ToInt32(dvstaffco.Count), 1);

                            Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - dvstaffco.Count, 11].Text = paper.ToString();
                            Fpspermomance.Sheets[0].Cells[Fpspermomance.Sheets[0].RowCount - dvstaffco.Count, 11].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            if (setflag == false)
            {
                Fpspermomance.Visible = false;
                labeldatevalid.Visible = true;
                labeldatevalid.Text = "No Records Found";
            }

            Fpspermomance.Sheets[0].PageSize = Fpspermomance.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {

        }
    }
    protected void Fpspersonal_SelectedIndexChanged(Object sender, EventArgs e)
    {
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        //lbl.Add(lbl_semOrTerm);


        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    public void initpersonal()
    {
        Fpspersonal.Sheets[0].ColumnCount = 0;
        Fpspersonal.Sheets[0].RowCount = 0;
        Fpspersonal.TitleInfo.Visible = true;
        Fpspersonal.ColumnHeader.Visible = false;
        Fpspersonal.RowHeader.Visible = false;
        Fpspersonal.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.LightSlateGray;
        Fpspersonal.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fpspersonal.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpspersonal.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpspersonal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        Fpspersonal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        Fpspersonal.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Left;
        Fpspersonal.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        Fpspersonal.Sheets[0].Columns.Default.Locked = true;
        Fpspersonal.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
        Fpspersonal.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
        Fpspersonal.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
        Fpspersonal.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
        //Fpspersonal.Sheets[0].DefaultColumnWidth = 80;
        //Fpspersonal.Sheets[0].DefaultRowHeight = 20;
        Fpspersonal.Sheets[0].PageSize = 55;
        Fpspersonal.ColumnHeader.Height = 60;
        Fpspersonal.TitleInfo.Height = 20;
        Fpspersonal.TitleInfo.Font.Name = "Book Antiqua";
        Fpspersonal.TitleInfo.Font.Size = FontUnit.Large;
        Fpspersonal.TitleInfo.Font.Bold = true;
        Fpspersonal.TitleInfo.BackColor = Color.DeepSkyBlue;
        Fpspersonal.Visible = false;
        Fpspersonal.CommandBar.Visible = false;
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {


    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ButtonGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                lblError.Visible = false;
                lblError.Text = "";

                string[] spf = txtfromdate.Text.ToString().Split('/');
                string[] spt = txttodate.Text.ToString().Split('/');
                DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
                DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
                DateTime dtnow = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                if (dtf > dtt)
                {
                    lblError.Visible = true;
                    lblError.Text = "From Date Should Be Less Than To Date";
                    return;
                }
                else
                {
                    lblError.Visible = false;
                    lblError.Text = "";

                }


                initattendence();
                Fpsattendence.Sheets[0].ColumnHeader.Visible = true;
                Fpsattendence.Visible = true;
                Fpsattendence.TitleInfo.Text = "STAFF ATTENDENCE DETAILS";
                ArrayList arrayceck = new ArrayList();
                string[] dtfrom;
                string[] dttodate;
                dtfrom = txtfromdate.Text.Split('/');
                dttodate = txttodate.Text.Split('/');
                DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;
                DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]).Date;
                TimeSpan t = strenddate.Subtract(strstartdate);
                long days = t.Days;
                Fpsattendence.Sheets[0].ColumnHeader.RowCount = 2;
                while (strstartdate <= strenddate)
                {
                    string date = strstartdate.ToString("dd/MM/yyyy");
                    arrayceck.Add(strstartdate.ToString("d/MM/yyyy"));
                    Fpsattendence.Sheets[0].ColumnHeader.Columns.Count = Fpsattendence.Sheets[0].ColumnHeader.Columns.Count + 2;
                    Fpsattendence.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpsattendence.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                    Fpsattendence.Sheets[0].ColumnHeader.Cells[0, Fpsattendence.Sheets[0].ColumnHeader.Columns.Count - 2].Text = Convert.ToString(date);
                    Fpsattendence.Sheets[0].ColumnHeader.Cells[1, Fpsattendence.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "M";
                    Fpsattendence.Sheets[0].ColumnHeader.Cells[1, Fpsattendence.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "E";

                    strstartdate = strstartdate.AddDays(1);

                    // Fpsattendence.SaveChanges();
                }
                string monyear = "";
                string resondate = "";
                string staff_code = d2.GetFunction("select staff_code from staffmaster where appl_no='" + appno + "'");
                Fpsattendence.Sheets[0].RowCount++;
                int count = 0;

                for (int k = 0; k < Fpsattendence.Sheets[0].ColumnCount; k += 2)//delsiref16
                {
                    string date = arrayceck[count].ToString();
                    string[] split_d = date.Split(new Char[] { '/' });
                    string strdate = split_d[0].ToString();
                    string Atmonth = split_d[1].ToString();
                    string Atyear = split_d[2].ToString();
                    string atmonth1 = (Atmonth.TrimStart('0'));
                    string strdate1 = (strdate.TrimStart('0'));
                    monyear = atmonth1 + "/" + Atyear;
                    resondate = atmonth1 + "/" + strdate1 + "/" + Atyear;

                    string countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(countquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string attndsplit = Convert.ToString(ds.Tables[0].Rows[0][0]);
                        if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                        {
                            string[] splitarray = attndsplit.Split('-');
                            if (splitarray.GetUpperBound(0) == 1)
                            {
                                string setval = splitarray[0].ToString().Trim().ToLower();
                                string setval1 = splitarray[1].ToString().Trim().ToLower();

                                Fpsattendence.Sheets[0].Cells[0, k].Text = Convert.ToString(splitarray[0]);
                                Fpsattendence.Sheets[0].Cells[0, k].HorizontalAlign = HorizontalAlign.Center;
                                Fpsattendence.Sheets[0].Cells[0, k + 1].Text = Convert.ToString(splitarray[1]);
                                Fpsattendence.Sheets[0].Cells[0, k + 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }
                    }
                    count++;

                }

                Fpsattendence.Sheets[0].PageSize = Fpsattendence.Sheets[0].RowCount;
                Fpsattendence.SaveChanges();
                Fpsattendence.Height = 100;
                Fpsattendence.Width = 700;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please Enter From Date And To Date";

            }

        }
        catch (Exception ex)
        {

        }
    }

    public void initattendence()
    {
        Fpsattendence.Sheets[0].AutoPostBack = false;
        Fpsattendence.Sheets[0].ColumnCount = 0;
        Fpsattendence.Sheets[0].RowCount = 0;
        Fpsattendence.TitleInfo.Visible = true;
        Fpsattendence.TitleInfo.Font.Name = "Book Antiqua";
        Fpsattendence.TitleInfo.Font.Size = FontUnit.Large;
        Fpsattendence.TitleInfo.Font.Bold = true;
        Fpsattendence.TitleInfo.BackColor = Color.DeepSkyBlue;
        Fpsattendence.ColumnHeader.Visible = false;
        Fpsattendence.RowHeader.Visible = false;
        Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.LightSlateGray;
        Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        Fpsattendence.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        //  Fpsattendence.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        Fpsattendence.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Left;
        Fpsattendence.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        Fpsattendence.Sheets[0].Columns.Default.Locked = true;
        Fpsattendence.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
        Fpsattendence.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
        Fpsattendence.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
        Fpsattendence.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
        Fpsattendence.CommandBar.Visible = false;
        Fpsattendence.Visible = false;
        Fpsattendence.Sheets[0].GridLines = GridLines.Both;
        //Fpsattendence.SaveChanges();
    }
    protected void ButtonBioGo_Click(object sender, EventArgs e)
    {
        try
        {

            Fpsattendence.Visible = true;
            Fpsattendence.Sheets[0].AutoPostBack = false;
            Fpsattendence.Sheets[0].RowCount = 0;
            Fpsattendence.Sheets[0].ColumnCount = 0;
            Fpsattendence.Sheets[0].ColumnCount = 6;
            Fpsattendence.Sheets[0].ColumnHeader.RowCount = 1;
            Fpsattendence.RowHeader.Visible = false;
            Fpsattendence.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpsattendence.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpsattendence.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].AllowTableCorner = true;
            Fpsattendence.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpsattendence.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Time In";
            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Time Out";
            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Morning";
            Fpsattendence.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Evening";

            Fpsattendence.Sheets[0].Columns[0].Width = 50;
            Fpsattendence.Sheets[0].Columns[1].Width = 80;
            Fpsattendence.Sheets[0].Columns[2].Width = 70;
            Fpsattendence.Sheets[0].Columns[3].Width = 70;
            Fpsattendence.Sheets[0].Columns[4].Width = 70;
            Fpsattendence.Sheets[0].Columns[5].Width = 70;

            Fpsattendence.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            Fpsattendence.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            string[] dtfrom;
            string[] dttodate;
            dtfrom = txtfromdate.Text.Split('/');
            dttodate = txttodate.Text.Split('/');
            DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]);
            DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]);

            string staff_code = d2.GetFunction("select staff_code from staffmaster where appl_no='" + appno + "'");
            while (strstartdate <= strenddate)
            {
                ds = da.select_method("select * from bio_attendance where roll_no='" + staff_code + "' and access_date='" + strstartdate + "'", ht, "Text");
                Fpsattendence.Sheets[0].RowCount++;
                Fpsattendence.Sheets[0].RowCount = Fpsattendence.Sheets[0].RowCount++;
                Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 0].Text = Fpsattendence.Sheets[0].RowCount.ToString();

                Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 1].Text = String.Format("{0:dd-MM-yyyy}", strstartdate);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string intime = ds.Tables[0].Rows[0]["Time_in"].ToString();
                    string outtime = ds.Tables[0].Rows[0]["Time_Out"].ToString();

                    if (intime != "")
                    {
                        DateTime in_time = Convert.ToDateTime(ds.Tables[0].Rows[0]["Time_in"]);
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 2].Text = in_time.ToString("hh:mm tt");

                    }
                    else
                    {
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 2].Text = "-";
                    }
                    if (outtime != "")
                    {
                        DateTime out_time = Convert.ToDateTime(ds.Tables[0].Rows[0]["Time_Out"]);
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 3].Text = out_time.ToString("hh:mm tt");
                    }
                    else
                    {
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 3].Text = "-";
                    }
                }
                else
                {
                    Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 2].Text = "-";
                    Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 3].Text = "-";
                }
                string sd = strstartdate.ToString();
                string[] sda = sd.Split(' ');
                string dt1 = sda[0].ToString();
                string[] dta1 = dt1.Split('/');
                string dat = dta1[1].ToString();
                string monthyear = dta1[0].ToString().TrimStart('0') + "/" + dta1[2].ToString();

                ds1 = da.select_method("select [" + dat + "] as dat from staff_attnd where mon_year='" + monthyear + "' and staff_code='" + staff_code + "'", ht, "Text");

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string atn = ds1.Tables[0].Rows[0]["dat"].ToString();
                    if (atn != "")
                    {
                        string[] atns = atn.Split('-');
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].Text = atns[0].ToString();
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].Text = atns[1].ToString();
                        if (atns[0].ToString().ToUpper().Trim() == "P")
                        {
                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].BackColor = Color.Green;
                        
                        }
                        else if (atns[0].ToString().ToUpper().Trim() == "PER")
                        {
                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].BackColor = Color.Chocolate;
                        }
                        else if (atns[0].ToString().ToUpper().Trim() == "LA")
                        {

                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].BackColor = Color.DarkRed;
                        }
                       
                        if (atns[1].ToString().ToUpper() == "P")
                        {
                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].BackColor = Color.Green;
                        
                        }
                        else if (atns[1].ToString().ToUpper().Trim() == "PER")
                        {
                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].BackColor = Color.Chocolate;
                        }
                        else if (atns[1].ToString().ToUpper().Trim() == "LA")
                        {

                            Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].BackColor = Color.DarkRed;
                        }
                        

                    }

                    else
                    {
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].Text = "-";
                        Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].Text = "-";
                    }
                }
                else
                {
                    Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 4].Text = "-";
                    Fpsattendence.Sheets[0].Cells[Fpsattendence.Sheets[0].RowCount - 1, 5].Text = "-";
                }

                strstartdate = strstartdate.AddDays(1);
            }
            Fpsattendence.Sheets[0].PageSize = Fpsattendence.Sheets[0].Rows.Count;
            Fpsattendence.Width = 430;

            Fpsattendence.Height = 300;
        }
        catch (Exception ex)
        {

        }
    }
}