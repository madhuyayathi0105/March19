using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;

using Gios.Pdf;
using System.Text;

public partial class Resultanalysis : System.Web.UI.Page
{


    #region variable Declaration

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection condegree = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rankcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    //string regularflag =string.Empty;
    string markglag = string.Empty;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string staff = string.Empty;
    string staff1 = string.Empty;
    double perofpass = 0;
    double avg = 0;
    Boolean IsFirstcol = false;
    Boolean Isfirst = false;
    string strorder = string.Empty;
    string strregorder = string.Empty;

    string dateconcat = string.Empty;
    string date1concat = string.Empty;

    int student = 0;
    int totds1count = 0;
    Hashtable hat = new Hashtable();
    Hashtable htattperc = new Hashtable();
    Hashtable ht_fail_subject = new Hashtable();
    DAccess2 daccess2 = new DAccess2();
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
    int moncount;
    double dif_date = 0;
    int dum_diff_date, unmark;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    string frdate, todate;
    TimeSpan ts;
    int eligiblepercent = 0;
    double halfday = 0;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    //  double cum_tot_point, per_holidate, cum_per_holidate;
    int per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double per_njhr, cum_njhr, cum_njdate;
    int countds = 0;
    //end
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds_load = new DataSet();
    //'------------new var mythili on 27.04.12 for attendance
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date, start_column = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    static string view_footer = string.Empty; string view_header = string.Empty; string view_footer_text = string.Empty;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    string halforfull = string.Empty; string mng = string.Empty; string evng = string.Empty; string holiday_sched_details = string.Empty;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    double tot_ondu, per_tot_ondu;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = string.Empty; string split_holiday_status_2 = string.Empty;
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    int mmyycount = 0;
    double per_holidate;
    //'---------------------------start print master
    DataSet dsprint = new DataSet();
    int final_print_col_cnt = 0;
    string footer_text = string.Empty;
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    string subjeccode = string.Empty;
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
    //Added By Srinath 25/2/2013
    string tempdegreesem = string.Empty;
    string chkdegreesem = string.Empty;
    Boolean datechk = false;
    string tempfrdate = string.Empty;
    string temptodate = string.Empty;
    int temcallfrommonth = 0;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    static Boolean splhr_flag = false;
    static string grouporusercode = string.Empty;
    double spl_tot_condut = 0;
    string branch = string.Empty;
    string degree = string.Empty;
    string sem = string.Empty;
    string sec = string.Empty;
    string test = string.Empty;
    string batch = string.Empty;
    string sem3 = string.Empty;
    string bat = string.Empty;
    string academic = string.Empty;
    string Strength = string.Empty;
    string temp7 = string.Empty;
    int sno = 0;
    string subject_code = string.Empty;
    string subject_name = string.Empty;
    string staff_name = string.Empty;
    int total_pass_fail = 0;
    int total_pass_fail1 = 0;
    string pren_count = string.Empty;
    string pass_count_new = string.Empty;
    string fail_count = string.Empty;
    string absent_count = string.Empty;
    string pertecount = string.Empty;
    string dum_tage_date = string.Empty;
    string dum_tage_hrs = string.Empty;
    int pass_count = 0;
    string sqlStr1 = string.Empty;
    int fail_sub_cnt = 0;
    string rolnosubno = string.Empty;
    string no_of_studentpass = string.Empty;
    string Percentage_of_Students = string.Empty;
    int total_pass_count = 0;
    double pass_percentage;
    //failur count..............
    string FailureSubj_Count = string.Empty;
    string spl_htval_Count = string.Empty;
    //%...............
    string serialno1 = string.Empty;
    string keyvalue = string.Empty;
    string stud_name_final = string.Empty;
    string attenPer = string.Empty;
    string NoofStudents = string.Empty;
    string Serial_New = string.Empty;
    string Roll_Stude = string.Empty;
    string stud_Name_Cnt = string.Empty;
    string Attendance = string.Empty;
    int valPer = 0;

    //added by rajasekar 13/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    ArrayList tblstartrowvalue = new ArrayList();

    StringBuilder html = new StringBuilder();



    //============================//

    #endregion
    //'---------------------------end print mnaster

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

    // Session["strvar"] = "0";
    protected void Page_Load(object sender, EventArgs e)
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
        try
        {
            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                //Added By Srinath 25/2/2013 =======Start
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                //==End
                norecordlbl.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnprint_Pdf.Visible = false;
                //Radiowithoutheader.Visible = false;
                //RadioHeader.Visible = false;
                ddlpage.Visible = false;
                lblpages.Visible = false;
                // Radiowithoutheader.Checked = true;

                //'----------- Initial date value
                string dt1 = DateTime.Today.ToShortDateString();
                string[] dsplit = dt1.Split(new Char[] { '/' });
                dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtFromDate.Text = dateconcat.ToString();
                string dt2 = DateTime.Today.ToShortDateString();
                string[] dt2split = dt2.Split(new Char[] { '/' });
                date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
                txtToDate.Text = date1concat.ToString();
                rdattnd_daywise.Checked = true;

                divgrid.Visible = false;


                Master = "select * from Master_Settings where " + grouporusercode + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master, setcon);
                mtrdr = mtcmd.ExecuteReader();
                Session["strvar"] = string.Empty;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
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
                            strdayflag = " and (registration.Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler'";
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
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                if (Request.QueryString["val"] != null)
                {
                    string get_pageload_value = Request.QueryString["val"];
                    if (get_pageload_value.ToString() != null)
                    {
                        string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                        string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val
                        bindbatch();
                        ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());
                        binddegree();
                        ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                        if (ddlDegree.Text != "")
                        {
                            bindbranch();
                            ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                            //bind semester
                            bindsem();
                            ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                            //bind section
                            bindsec();
                            ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                            //bing test
                            GetTest();
                            ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                            lblnorec.Visible = false;
                            txtFromDate.Text = spl_load_val[1].ToString();
                            txtToDate.Text = spl_load_val[2].ToString();
                            btnGo_Click(sender, e);
                            //   ddlpage_SelectedIndexChanged(sender, e);
                            func_radio_header();
                            //  func_header();
                            function_footer();
                        }
                        else
                        {
                            lblnorec.Text = "Give degree rights to the staff";
                            lblnorec.Visible = true;
                        }
                    }
                }
                else
                {
                    bindbatch();
                    binddegree();
                    if (ddlDegree.Text != "")
                    {
                        bindbranch();
                        //bind semester
                        bindsem();
                        //bind section
                        bindsec();
                        //bing test
                        GetTest();
                        lblnorec.Visible = false;
                    }
                    else
                    {
                        lblnorec.Text = "Give degree rights to the staff";
                        lblnorec.Visible = true;
                    }
                }
            }
        }
        catch
        {
        }
    }



    public void bindbatch()
    {
        ////batch
        ddlBatch.Items.Clear();
        string sqlstring = string.Empty;
        int max_bat = 0;
        con.Close();
        con.Open();


        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlBatch.DataSource = ds1;
        ddlBatch.DataValueField = "batch_year";
        ddlBatch.DataBind();
        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' ";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlBatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();
        //int count = 0;
        //ddlBatch.Items.Clear();
        //ds_load.Clear();
        //ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
        //if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        //{
        //    count = ds_load.Tables[0].Rows.Count;
        //    if (count > 0)
        //    {
        //        ddlBatch.DataSource = ds_load;
        //        ddlBatch.DataTextField = "batch_year";
        //        ddlBatch.DataValueField = "batch_year";
        //        ddlBatch.DataBind();
        //    }
        //}
        //if (ds_load.Tables.Count > 1 && ds_load.Tables[1].Rows.Count > 0)
        //{
        //    int count1 = ds_load.Tables[1].Rows.Count;
        //    if (count > 0)
        //    {
        //        int max_bat = 0;
        //        max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
        //        ddlBatch.SelectedValue = max_bat.ToString();
        //        con.Close();
        //    }
        //}
    }

    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        ds_load.Clear();
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
        ds_load = daccess2.select_method("bind_branch", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds_load;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
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
        ds_load.Clear();
        ds_load = daccess2.select_method("bind_degree", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count1 = ds_load.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddlDegree.DataSource = ds_load;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
    }

    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load.Clear();
        ds_load = daccess2.select_method("bind_sec", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count5 = ds_load.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds_load;
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
        else
        {
            ddlSec.Enabled = false;
        }
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
            Sqlstr = string.Empty;
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
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
                ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            }
        }
        catch
        {
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

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
        }
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
        try
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
            cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
            //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
            con.Close();
        }
        catch
        {
        }
    }

    public void Get_Semester()
    {
        bool first_year;
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
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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

    public void SpreadBind()
    {
        //try
        //{
        //        int hasrow_count = 0;
        //        int rk = 0;

        //        int y = 0;
        //        int flag = 0;
        //        string SyllabusYr;
        //        string SyllabusQry;
        //        string sqlTest = string.Empty;
        //        string strsec = string.Empty;
        //        string sections = string.Empty;
        //        string batch = string.Empty;
        //        string degreecode = string.Empty;
        //        string subno = string.Empty;
        //        string semester = string.Empty;
        //        string display = string.Empty;
        //        string exam_code = string.Empty;
        //        string criteria_no = string.Empty;
        //        int rowcnt = 0;
        //        string rollno = string.Empty;
        //        string resmaxmrk = string.Empty;
        //        string resminmrk = string.Empty;
        //        string resduration = string.Empty;
        //        string subject_code = string.Empty;
        //        int totall = 0;
        //        string strsex = string.Empty;
        //        string strregular = string.Empty;
        //        int[] maxtot = new int[100];
        //        //string [] a= new string [20];
        //        int minimark = 0;
        //        string examdate = string.Empty;
        //        string examdate1 = string.Empty;
        //        string staffcode = string.Empty;
        //        string sqlpass = string.Empty;
        //        string sqlFail = string.Empty;
        //        string sqlAbsent = string.Empty;
        //        string chkmark = string.Empty;
        //        Boolean isstudflag = false;
        //        Boolean rankflag = false;
        //        string gmonth = string.Empty;
        //        string gdate = string.Empty;
        //        string gyear = string.Empty;
        //        string monthyear = string.Empty;
        //        string exampresent = string.Empty;
        //        string Date = string.Empty;
        //        string leavetype = string.Empty;
        //        int present = 0;
        //        int markcount = 0;
        //        double[] percentarray = new double[200];
        //        string[] rollnoarray = new string[200];
        //        int[] rankcountflag = new int[200];
        //        int count = 0;
        //        batch = ddlBatch.SelectedValue.ToString();
        //        degreecode = ddlBranch.SelectedValue.ToString();
        //        sections = ddlSec.SelectedValue.ToString();
        //        semester = ddlSemYr.SelectedValue.ToString();
        //        criteria_no = ddlTest.SelectedValue.ToString();
        //        string sqlStr = string.Empty;
        //        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        //        {
        //            strsec = string.Empty;
        //        }
        //        else
        //        {
        //            strsec = " and sections='" + sections.ToString() + "'";
        //        }

        //        string sqlTest1 = "select distinct s.subject_no,s.subject_code,staff_code,duration,exam_date,max_mark,min_mark,r.exam_code from exam_type e,subject s,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no=" + criteria_no + " " + strsec + "";
        //        mycon.Close();
        //        mycon.Open();
        //        SqlCommand cmd1 = new SqlCommand(sqlTest1, mycon);
        //        SqlDataReader readermark;
        //        readermark = cmd1.ExecuteReader();
        //        string includePastout = string.Empty;


        //        if (!chkincludepastout.Checked)
        //        {

        //            includePastout = "and CC=0";
        //        }
        //        if (readermark.HasRows)
        //        {
        //            sqlStr = "select distinct registration.Roll_No as RollNumber, registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " order by  roll_no ";
        //            con.Close();
        //            con.Open();
        //            if (sqlStr != "")
        //            {
        //                SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr, con);
        //                adaSyll1.Fill(ds5, "ds");

        //                if (ds5.Tables.Count > 0 && ds5.Tables[0].Rows.Count > 0)
        //                {
        //                    totds1count = ds5.Tables[0].Rows.Count;
        //                }
        //                if (Session["Rollflag"].ToString() == "0")
        //                {

        //                }
        //                if (Session["Regflag"].ToString() == "0")
        //                {

        //                }
        //                if (Session["Studflag"].ToString() == "0")
        //                {

        //                }

        //            }
        //            hasrow_count = hasrow_count + 1;
        //            while (readermark.Read())
        //            {
        //                subno = readermark["subject_no"].ToString();
        //                subject_code = readermark["subject_code"].ToString();
        //                resmaxmrk = readermark["max_mark"].ToString();
        //                resminmrk = readermark["min_mark"].ToString();
        //                resduration = readermark["duration"].ToString();
        //                exam_code = readermark["exam_code"].ToString();
        //                examdate = readermark["exam_date"].ToString();


        //                // FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Tag = subject_code;

        //                //FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Tag = exam_code;

        //                count++;
        //            }
        //        }
        //        //=============================


        //        sqlStr = string.Empty;
        //        int mediumcount = 0;
        //        int percount = 0;
        //        int grdcount = 0;
        //        int cgpacount = 0;
        //        int classstrength = 0;
        //        int StudentsAppeared = 0;
        //        int StudentsAbsent = 0;
        //        int StudentsPassed = 0;
        //        int StudentsFailed = 0;
        //        int Average50 = 0;
        //        int Average50to65 = 0;
        //        //string temp="";
        //        int allflag = 0;
        //        int average65 = 0;
        //        int classaverage = 0;
        //        int classmaxmark = 0;
        //        int Passpercent1 = 0;
        //        int signat = 0;
        //        string dum_tage_date = string.Empty;
        //        string dum_tage_hrs = string.Empty;
        //        string strsec1 = string.Empty;
        //        string rol_no = string.Empty;
        //        if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == "" || sections.ToString().Trim().ToLower() == "-1")
        //        {
        //            strsec = string.Empty;
        //            strsec1 = string.Empty;
        //        }
        //        else
        //        {
        //            strsec = " and exam_type.sections='" + sections.ToString() + "'";
        //            strsec1 = " and Sections='" + sections.ToString() + "'";
        //        }
        //        if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) != 0)
        //        {
        //            for (int res = 0; res <= Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 7; res++)
        //            {
        //                double total = 0;
        //                markcount = 0;
        //                rankcountflag[res] = 0;
        //                for (int col = 5; col < 5 + count; col++)
        //                {
        //                    markcount = markcount + 1;
        //                    FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
        //                    examdate1 = FpEntry.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
        //                    string subjctcode = string.Empty;
        //                    string[] splitdate_ecode = examdate1.Split(new Char[] { '@' });
        //                    examdate = splitdate_ecode[0].ToString();
        //                    exam_code = splitdate_ecode[1].ToString();
        //                    string s = strsec1;
        //                    //subjctcode= Convert.ToInt32(  FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Text) ;
        //                    rol_no = FpEntry.Sheets[0].GetText(res, 0).ToString();
        //                    int appl_no = Convert.ToInt32(FpEntry.Sheets[0].GetText(res, 4).ToString());
        //                    subjctcode = FpEntry.Sheets[0].ColumnHeader.Cells[0, col].Note;
        //                    // staffcode = FpEntry.Sheets[0].ColumnHeader.Cells[0, col].Note;
        //                    if (Isfirst == false)
        //                    {
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                StudentsAppeared = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(StudentsAppeared, 2, "Students Appeared");
        //                            }
        //                            string temp3 = ("select text_value from cam_mark_opt where degree_code=" + degreecode + " and semester=" +
        // semester + " and batch_year=" + batch + strsec1 + " and subject_no=" + subjctcode + " and criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_code=" + exam_code + " and text_criteria='no_of_present'");
        //                            string Appeared = GetFunction(temp3);
        //                            FpEntry.Sheets[0].Cells[StudentsAppeared, col].Text = Appeared;
        //                        }
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                StudentsAbsent = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(StudentsAbsent, 2, "Students Absent");
        //                            }
        //                            string temp2 = ("select sum (text_value) from cam_mark_opt where degree_code=" + degreecode + " and semester=" +
        // semester + " and batch_year=" + batch + strsec1 + " and subject_no=" + subjctcode + " and criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_code=" + exam_code + " and (text_criteria='no_of_absent' or text_criteria='no_of_leave')");
        //                            string Absent = GetFunction(temp2);
        //                            FpEntry.Sheets[0].Cells[StudentsAbsent, col].Text = Absent;
        //                        }
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                StudentsPassed = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 2, "Students Passed");
        //                            }
        //                            string temp1 = ("select text_value from cam_mark_opt where degree_code=" + degreecode + " and semester=" +
        // semester + " and batch_year=" + batch + strsec1 + " and subject_no=" + subjctcode + " and criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_code=" + exam_code + " and text_criteria='no_of_pass'");
        //                            string Passed = GetFunction(temp1);
        //                            FpEntry.Sheets[0].Cells[StudentsPassed, col].Text = Passed;
        //                        }
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                StudentsFailed = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 2, "Students Failed");
        //                            }
        //                            string temp = ("select text_value from cam_mark_opt where degree_code=" + degreecode + " and semester=" +
        //semester + " and batch_year=" + batch + strsec1 + " and subject_no=" + subjctcode + " and criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_code=" + exam_code + " and text_criteria='no_of_fail'");
        //                            string failed = GetFunction(temp);
        //                            FpEntry.Sheets[0].Cells[StudentsFailed, col].Text = failed;
        //                        }
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                Passpercent1 = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 2, "Percentage of pass");
        //                            }
        //                            string temp4 = ("select text_value from cam_mark_opt where degree_code=" + degreecode + " and semester=" +
        // semester + " and batch_year=" + batch + strsec1 + " and subject_no=" + subjctcode + " and criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_code=" + exam_code + " and text_criteria='pass_percentage'");
        //                            string Percent = GetFunction(temp4);
        //                            FpEntry.Sheets[0].Cells[Passpercent1, col].Text = Percent;
        //                        }
        //                        {
        //                            if (IsFirstcol == false)
        //                            {
        //                                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
        //                                signat = FpEntry.Sheets[0].RowCount - 1;
        //                                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 2, "Staff signature");
        //                            }
        //                            string signature = string.Empty;
        //                            //string includePastout = string.Empty;


        //                            //if (!chkincludepastout.Checked)
        //                            //{

        //                            //    includePastout = "and CC=0";
        //                            //}
        //                            signature = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no " + includePastout + " and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + subjctcode + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
        //                            if (signature != "")
        //                            {
        //                                staff = GetFunction("select nameacr from staff_appl_master where appl_no in(select distinct  appl_no from staffmaster where staff_code = '" + signature + "')");
        //                            }
        //                            if (staff == "" && (signature) != "")
        //                            {
        //                                staff = GetFunction("select staff_name from staffmaster where staff_code = '" + signature + "'");
        //                            }
        //                            FpEntry.Sheets[0].Cells[signat, col].Text = staff;
        //                        }
        //                    }
        //                    IsFirstcol = true;
        //                    string sqlmarks = "Select marks_obtained  from result,exam_type,criteriaforinternal,registration where registration.app_no= " + appl_no + " and exam_type.subjecT_no = '" + subjctcode + "' and  registration.roll_no=result.roll_no " + includePastout + " and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = " + ddlBatch.SelectedValue.ToString() + "  and exam_type.criteria_no =" + ddlTest.SelectedValue.ToString() + "" + strsec + "";
        //                    mycon.Close();
        //                    mycon.Open();
        //                    SqlCommand cmdd = new SqlCommand(sqlmarks, mycon);
        //                    SqlDataReader reader1;
        //                    reader1 = cmdd.ExecuteReader();
        //                    if (reader1.HasRows)
        //                    {
        //                        string celltag = string.Empty;
        //                        while (reader1.Read())
        //                        {
        //                            string v = reader1["marks_obtained"].ToString();
        //                            if ((v != "") || (v != null))
        //                            {
        //                                if (Convert.ToString(v) == "-1")
        //                                {
        //                                    rankcountflag[res] = rankcountflag[res] + 1;
        //                                    FpEntry.Sheets[0].Cells[res, col].Tag = "1";
        //                                    if (examdate != "")
        //                                    {
        //                                        string[] splitdate = Convert.ToDateTime(examdate).ToString("M/d/yyyy").Split(new Char[] { '/' });
        //                                        gmonth = splitdate[0].ToString();
        //                                        gdate = splitdate[1].ToString();
        //                                        gyear = splitdate[2].ToString();
        //                                        string[] split5 = gyear.Split(new Char[] { ' ' });
        //                                        string gbal = split5[0].ToString();
        //                                        monthyear = ((Convert.ToInt32(gmonth)) + (Convert.ToInt32(gbal) * 12) + "");
        //                                        exampresent = "select d" + gdate + "d1 from attendance where roll_no='" + rol_no + "' and month_year='" + monthyear + "'";
        //                                        Date = Getdate(exampresent);
        //                                        if ((Date != ""))
        //                                        {
        //                                            present = Convert.ToInt32(Date);
        //                                            leavetype = getattval(present);
        //                                            FpEntry.Sheets[0].Cells[res, col].Text = leavetype;
        //                                            FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
        //                                            FpEntry.Sheets[0].Cells[res, col].Tag = "1";
        //                                            celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                        }
        //                                        else
        //                                        {
        //                                            FpEntry.Sheets[0].Cells[res, col].Tag = "1";
        //                                            FpEntry.Sheets[0].Cells[res, col].Text = "AAA";
        //                                            FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
        //                                            celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                        }
        //                                    }
        //                                }
        //                                else if (v == "0")
        //                                {
        //                                    FpEntry.Sheets[0].Cells[res, col].Tag = "1";
        //                                    FpEntry.Sheets[0].Cells[res, col].Text = "0";
        //                                    FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
        //                                    rankcountflag[res] = rankcountflag[res] + 1;
        //                                    celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                }
        //                                else if (Convert.ToString(v) == "-3")
        //                                {
        //                                    FpEntry.Sheets[0].Cells[res, col].Text = "EOD";
        //                                    rankcountflag[res] = rankcountflag[res] + 1;
        //                                    celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                }
        //                                else if (Convert.ToString(v) == "-2")
        //                                {
        //                                    FpEntry.Sheets[0].Cells[res, col].Text = "EL";
        //                                    rankcountflag[res] = rankcountflag[res] + 1;
        //                                    celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                }
        //                                else if ((Convert.ToDouble(v) >= 0) && ((Convert.ToDouble(v)) < (Convert.ToDouble(resminmrk))))
        //                                {
        //                                    FpEntry.Sheets[0].Cells[res, col].Tag = "1";
        //                                    FpEntry.Sheets[0].Cells[res, col].Text = v;
        //                                    FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
        //                                    FpEntry.Sheets[0].Cells[res, col].Font.Underline = true;
        //                                    FpEntry.Sheets[0].Cells[res, col].Font.Name = "Book Antiqua";
        //                                    FpEntry.Sheets[0].Cells[res, col].Font.Size = FontUnit.Medium;
        //                                    celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                }
        //                                else
        //                                {
        //                                    FpEntry.Sheets[0].Cells[res, col].Tag = "0";
        //                                    FpEntry.Sheets[0].Cells[res, col].Text = Convert.ToString(v);
        //                                    FpEntry.Sheets[0].Cells[res, col].Font.Name = "Book Antiqua";
        //                                    FpEntry.Sheets[0].Cells[res, col].Font.Size = FontUnit.Medium;
        //                                    celltag = FpEntry.Sheets[0].Cells[res, col].Tag + "";
        //                                }
        //                            }
        //                            //putting total
        //                            string marktot = FpEntry.Sheets[0].Cells[res, col].Text;
        //                            if ((marktot != "EL") && (marktot != "A") && (marktot != "-") && (marktot != "OD") && (marktot != "AAA") && (marktot != "EOD") && (marktot != "\0") && (marktot != "") && (marktot != "P") && (marktot != "S") && (marktot != "SL") && (marktot != "MOD") && (marktot != "NSS") && (marktot != "L") && (marktot != "H") && (marktot != "NJ"))
        //                            {
        //                                total = total + Convert.ToDouble(FpEntry.Sheets[0].Cells[res, col].Text);
        //                                //}
        //                            }
        //                            else
        //                            {
        //                                total = total + 0;
        //                            }
        //                            FpEntry.Sheets[0].Cells[res, (5 + count)].HorizontalAlign = HorizontalAlign.Center;
        //                            FpEntry.Sheets[0].Cells[res, (5 + count)].Text = total.ToString();
        //                            //putting percentage
        //                            decimal percent = 0;
        //                            percent = Convert.ToDecimal((Convert.ToDouble(total) / count));
        //                            double percent3 = 0;
        //                            decimal percent2 = Math.Round(percent, 2);
        //                            percent3 = Convert.ToDouble(percent2);
        //                            FpEntry.Sheets[0].Cells[res, (5 + count + 1)].Text = percent3.ToString();
        //                            FpEntry.Sheets[0].Cells[res, (5 + count + 1)].HorizontalAlign = HorizontalAlign.Center;
        //                        }
        //                    }
        //                }
        //                Isfirst = true;
        //                if (FpEntry.Sheets[0].ColumnHeader.Cells[0, (5 + count + 2)].Text == "Rank")
        //                {
        //                    if ((FpEntry.Sheets[0].Cells[res, 0].Text != "") && (FpEntry.Sheets[0].Cells[res, 0].Text != null))
        //                    {
        //                        string studentrank = "select rank from rank where rollno='" + rol_no + "' and criteria_no=" + ddlTest.SelectedValue.ToString() + "";
        //                        string ranknow = GetFunction(studentrank);
        //                        FpEntry.Sheets[0].Cells[res, rankcount].Text = ranknow;
        //                        FpEntry.Sheets[0].Cells[res, rankcount].HorizontalAlign = HorizontalAlign.Center;
        //                    }
        //                }
        //                //  '--------------------------attendance percentage---------------------------------------
        //                hat.Clear();
        //                hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
        //                hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
        //                ds = d2.select_method("period_attnd_schedule", hat, "sp");
        //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                {
        //                    NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
        //                    fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
        //                    anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
        //                    minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
        //                    minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
        //                    eligiblepercent = int.Parse(ds.Tables[0].Rows[0]["Eligible_Percent"].ToString());
        //                }
        //                hat.Clear();
        //                hat.Add("colege_code", Session["collegecode"].ToString());
        //                ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
        //                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
        //                {
        //                    countds = ds1.Tables[0].Rows.Count;
        //                }
        //                //'----------------------------------------new start---------------------------------------------
        //                persentmonthcal();
        //                per_tage_date = ((pre_present_date / per_workingdays) * 100);
        //                if (per_tage_date > 100)
        //                {
        //                    per_tage_date = 100;
        //                }
        //                per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
        //                per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);
        //                if (per_tage_hrs > 100)
        //                {
        //                    per_tage_hrs = 100;
        //                }
        //                dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
        //                dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
        //                if (dum_tage_hrs == "NaN")
        //                {
        //                    dum_tage_hrs = "0";
        //                }
        //                else if (dum_tage_hrs == "Infinity")
        //                {
        //                    dum_tage_hrs = "0";
        //                }
        //                if (dum_tage_date == "NaN")
        //                {
        //                    dum_tage_date = "0";
        //                }
        //                else if (dum_tage_date == "Infinity")
        //                {
        //                    dum_tage_date = "0";
        //                }
        //                //'------------------------------------------------new end------------
        //                //'----------------------adding the percentage below 80 % to hash table-----------------------------
        //                if (htattperc.Contains(Convert.ToString(ds5.Tables[0].Rows[student]["RollNumber"]).Trim().ToLower()))
        //                {
        //                    int value1 = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rol_no).Trim().ToLower(), htattperc));
        //                    value1++;//fail count
        //                    htattperc[Convert.ToString(rol_no).Trim().ToLower()] = value1;
        //                }
        //                else
        //                {
        //                    if (Convert.ToDouble(dum_tage_date) < Convert.ToDouble(eligiblepercent))
        //                    {
        //                        htattperc.Add(Convert.ToString(ds5.Tables[0].Rows[student]["RollNumber"]).Trim().ToLower(), dum_tage_date.ToString());
        //                    }
        //                }
        //                student++;
        //                //'--------------------------------------------------------------------------------
        //            }
        //        }
        //        int failcount1 = 0;
        //        string numberoffailsub = string.Empty;
        //        for (int totrow = 0; totrow <= Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 7; totrow++)
        //        {
        //            for (int totcol = 5; totcol <= Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) - 1; totcol++)
        //            {
        //                if (FpEntry.Sheets[0].Cells[totrow, totcol].Tag == "1")
        //                {
        //                    if (FpEntry.Sheets[0].Cells[totrow, totcol].Text != "-")
        //                    {
        //                        string failsub = FpEntry.Sheets[0].ColumnHeader.Cells[0, totcol].Note;
        //                        string name = "select subject_name from subject where Subject_no=" + failsub + "";
        //                        mycon.Close();
        //                        mycon.Open();
        //                        SqlCommand namecmdd = new SqlCommand(name, mycon);
        //                        SqlDataReader namereader;
        //                        namereader = namecmdd.ExecuteReader();
        //                        string Subjectname = string.Empty;
        //                        if (namereader.HasRows)
        //                        {
        //                            while (namereader.Read())
        //                            {
        //                                Subjectname = namereader["subject_name"].ToString();
        //                            }
        //                        }
        //                        if (numberoffailsub == "")
        //                        {
        //                            numberoffailsub = Subjectname;
        //                        }
        //                        else
        //                        {
        //                            numberoffailsub = numberoffailsub + "," + Subjectname;
        //                        }
        //                        failcount1++;
        //                    }
        //                }
        //            }
        //            FpEntry.Sheets[0].Cells[totrow, 0].Tag = failcount1++;
        //            FpEntry.Sheets[0].Cells[totrow, 0].Note = numberoffailsub;
        //            failcount1 = 0;
        //            numberoffailsub = string.Empty;
        //            string gettag = FpEntry.Sheets[0].Cells[totrow, 0].Tag + "";
        //        }
        //}
        //catch
        //{
        //    Buttontotal.Visible = false;
        //    lblrecord.Visible = false;
        //    DropDownListpage.Visible = false;
        //    TextBoxother.Visible = false;
        //    lblpage.Visible = false;
        //    TextBoxpage.Visible = false;
        //    FpEntry.Visible = false;
        //    FpSpread3.Visible = false;
        //    lblnorec.Visible = true;
        //}
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

        //FpSpread3.Visible = false;
        Button2.Visible = false;
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
        ddlBranch.Items.Clear();
        ddlTest.Items.Clear();
        string course_id = ddlDegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
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

        Button2.Visible = false;
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
        ddlTest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{
            // Get_Semester();
            bindsem();
            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
        //bind section
        bindsec();
        //bing test
        GetTest();
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

    //'-----------------------------------------------------------------------------------------------------
    //public void persentmonthcal()
    //{
    //    int demfcal, demtcal;
    //    string monthcal;
    //    int mmyycount = 0;
    //    frdate = txtFromDate.Text.ToString();
    //    todate = txtToDate.Text.ToString();
    //    string dt = frdate;
    //    string[] dsplit = dt.Split(new Char[] { '/' });
    //    if (txtFromDate.Text.ToString() != "")
    //    {
    //        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
    //        demfcal = int.Parse(dsplit[2].ToString());
    //        demfcal = demfcal * 12;
    //        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
    //        monthcal = cal_from_date.ToString();
    //        per_from_date = Convert.ToDateTime(frdate);
    //        dumm_from_date = per_from_date;
    //    }
    //    dt = todate;
    //    dsplit = dt.Split(new Char[] { '/' });
    //    if (txtToDate.Text.ToString() != "")
    //    {
    //        todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
    //        demtcal = int.Parse(dsplit[2].ToString());
    //        demtcal = demtcal * 12;
    //        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
    //        per_to_date = Convert.ToDateTime(todate);
    //    }
    //    hat.Clear();
    //    hat.Add("std_rollno", ds7.Tables[0].Rows[student]["roll"].ToString());
    //    hat.Add("from_month", cal_from_date);
    //    hat.Add("to_month", cal_to_date);
    //    ds2 = daccess2.select_method("STUD_ATTENDANCE", hat, "sp");
    //    hat.Clear();
    //    hat.Add("degree_code", int.Parse(ddlBranch.SelectedValue.ToString()));
    //    hat.Add("sem", int.Parse(ddlSemYr.SelectedItem.ToString()));
    //    hat.Add("from_date", frdate.ToString());
    //    hat.Add("to_date", todate.ToString());
    //    hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
    //    //ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
    //    //mmyycount  = ds2.Tables[0].Rows.Count;
    //    //moncount = mmyycount  - 1;
    //    //------------------------------------------------------------------
    //    int iscount = 0;
    //    holidaycon.Close();
    //    holidaycon.Open();
    //    string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";
    //    SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
    //    SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
    //    DataSet dsholiday = new DataSet();
    //    daholiday.Fill(dsholiday);
    //   if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
    //    {
    //        iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
    //    }
    //    hat.Add("iscount", iscount);
    //    //  ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
    //    mmyycount = ds2.Tables[0].Rows.Count;
    //    moncount = mmyycount - 1;
    //    ds3 = daccess2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
    //    //------------------------------------------------------------------
    //    if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
    //    {
    //        ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
    //        diff_date = Convert.ToString(ts.Days);
    //        dif_date1 = double.Parse(diff_date.ToString());
    //    }
    //    next = 0;
    //    int rowcount = 0;
    //    int ccount;
    //    ccount = ds3.Tables[1].Rows.Count;
    //    ccount = ccount - 1;
    //    if ((ds2.Tables[0].Rows.Count != 0)&&(ds3.Tables[1].Rows.Count!=0))
    //    {
    //        //ccount = ccount - 1;
    //        //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
    //        while (dumm_from_date <= (per_to_date))
    //        {
    //            //for (int i = 1; i <= mmyycount; i++)
    //            //{
    //                if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
    //                {
    //                    if (dumm_from_date != DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()))
    //                    {
    //                    //    ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
    //                    //    diff_date = Convert.ToString(ts.Days);
    //                    //    dif_date = double.Parse(diff_date.ToString());
    //                        for (i = 1; i <= fnhrs; i++)
    //                        {
    //                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
    //                            value = ds2.Tables[0].Rows[next][date].ToString();
    //                            if (value != null && value != "0" && value != "7" && value != "")
    //                            {
    //                                if (tempvalue != value)
    //                                {
    //                                    tempvalue = value;
    //                                    for (int j = 0; j < countds; j++)
    //                                    {
    //                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
    //                                        {
    //                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
    //                                            j = countds;
    //                                        }
    //                                    }
    //                                }
    //                                if (ObtValue == 0)
    //                                {
    //                                    per_perhrs += 1;
    //                                    tot_per_hrs += 1;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                njhr += 1;
    //                            }
    //                        }
    //                        if (per_perhrs >= minpresI)
    //                        {
    //                            Present += 0.5;
    //                        }
    //                        else if (njhr == fnhrs)
    //                        {
    //                            njdate += 0.5;
    //                        }
    //                        per_perhrs = 0;
    //                    //    njhr = 0;
    //                        int k = i;
    //                        for (i = k; i <= NoHrs; i++)
    //                        {
    //                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
    //                            value = ds2.Tables[0].Rows[next][date].ToString();
    //                            if (value != null && value != "0" && value != "7" && value != "")
    //                            {
    //                                if (tempvalue != value)
    //                                {
    //                                    tempvalue = value;
    //                                    for (int j = 0; j < countds; j++)
    //                                    {
    //                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
    //                                        {
    //                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
    //                                            j = countds;
    //                                        }
    //                                    }
    //                                }
    //                                if (ObtValue == 0)
    //                                {
    //                                    per_perhrs += 1;
    //                                    tot_per_hrs += 1;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                njhr += 1;
    //                            }
    //                        }
    //                        if (per_perhrs >= minpresII)
    //                        {
    //                            Present += 0.5;
    //                        }
    //                        else if (njhr == NoHrs)
    //                        {
    //                            njdate += 0.5;
    //                        }
    //                        per_perhrs = 0;
    //                        njhr = 0;
    //                        dumm_from_date = dumm_from_date.AddDays(1);
    //                        if (dumm_from_date.Day == 1)
    //                        {
    //                            cal_from_date++;
    //                            if (moncount > next)
    //                            {
    //                                next++;
    //                            }
    //                        }
    //                        workingdays += 1;
    //                        per_perhrs = 0;
    //                    }
    //                    else
    //                    {
    //                        workingdays += 1;
    //                        dumm_from_date = dumm_from_date.AddDays(1);
    //                        if (dumm_from_date.Day == 1)
    //                        {
    //                            if (moncount > next)
    //                            {
    //                                next++;
    //                            }
    //                        }
    //                        per_holidate += 1;
    //                        if (ccount > rowcount)
    //                        {
    //                            rowcount++;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (dumm_from_date.Day == 1)
    //                    {
    //                        DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
    //                        dumm_from_date = dumm_fdate;
    //                        dumm_fdate = dumm_fdate.AddMonths(1);
    //                        cal_from_date++;
    //                        if (moncount > next)
    //                        {
    //                            next++;
    //                            i++;
    //                        }
    //                    }
    //                    if (moncount > next)
    //                    {
    //                        i--;
    //                    }
    //                }
    //          //  }
    //        }//'----end while
    //        int diff_Date = per_from_date.Day - dumm_from_date.Day;
    //    }
    //    per_tot_ondu = tot_ondu;
    //    per_njdate = njdate;
    //    pre_present_date = Present;
    //    per_per_hrs = tot_per_hrs;
    //    per_absent_date = Absent;
    //    pre_ondu_date = Onduty;
    //    pre_leave_date = Leave;
    //    per_workingdays = workingdays - per_holidate - per_njdate;
    //    per_dum_unmark = dum_unmark;
    //    Present = 0;
    //    tot_per_hrs = 0;
    //    Absent = 0;
    //    Onduty = 0;
    //    Leave = 0;
    //    workingdays = 0;
    //    per_holidate = 0;
    //    dum_unmark = 0;
    //    absent_point = 0;
    //    leave_point = 0;
    //    njdate = 0;
    //    tot_ondu = 0;
    //}

    public void persentmonthcal()
    {
        //Hidden By Srinath 25/2/2013
        //DataSet dsperiod = new DataSet();
        //hat.Clear();
        //hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
        //hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
        //dsperiod = daccess2.select_method("period_attnd_schedule", hat, "sp");
        //if (dsperiod.Tables[0].Rows.Count != 0)
        //{
        //    NoHrs = int.Parse(dsperiod.Tables[0].Rows[0]["PER DAY"].ToString());
        //    fnhrs = int.Parse(dsperiod.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
        //    anhrs = int.Parse(dsperiod.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
        //    minpresI = int.Parse(dsperiod.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
        //    minpresII = int.Parse(dsperiod.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
        //}
        //int countcount = 0;
        //hat.Clear();
        //hat.Add("colege_code", Session["collegecode"].ToString());
        //ds1 = daccess2.select_method("ATT_MASTER_SETTING", hat, "sp");
        //countcount = ds1.Tables[0].Rows.Count;
        //@@@@@
        frdate = txtFromDate.Text.ToString();
        todate = txtToDate.Text.ToString();
        int my_un_mark = 0;//Added By Srinath 16/3/2013
        int njdate_mng = 0, njdate_evng = 0;
        int per_holidate_mng = 0, per_holidate_evng = 0;
        mng_conducted_half_days = 0;
        evng_conducted_half_days = 0;
        notconsider_value = 0;
        int demfcal, demtcal;
        string monthcal;
        conduct_hour_new = 0;
        if (datechk != true)
        {
            datechk = true;
            string dt = frdate;
            string[] dsplit = dt.Split(new Char[] { '/' });
            if ((dsplit[0].ToString() != string.Empty) && (dsplit[1].ToString() != string.Empty) && (dsplit[2].ToString() != string.Empty))
            {
                frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            }
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
            tempfrdate = frdate;
            temcallfrommonth = cal_from_date;
            temptodate = todate;
        }
        frdate = tempfrdate;
        todate = temptodate;
        cal_from_date = temcallfrommonth;
        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);
        dumm_from_date = per_from_date;
        DataSet dsat = new DataSet();
        hat.Clear();
        hat.Add("std_rollno", ds7.Tables[0].Rows[student]["roll"].ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        dsat = daccess2.select_method("STUD_ATTENDANCE", hat, "sp");
        //Added By Srinath 25/2/2013 =====Start
        if (dsat.Tables.Count > 0 && dsat.Tables[0].Rows.Count > 0)
        {
            mmyycount = dsat.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
        }
        //=======End
        //Added By Srinath 25/2/2013 =======Start
        chkdegreesem = ddlBranch.SelectedValue.ToString() + '/' + ddlSemYr.SelectedItem.ToString();
        if (chkdegreesem != tempdegreesem)
        {
            tempdegreesem = chkdegreesem;
            //Specail Hour Status Check Details ============Start
            ht_sphr.Clear();
            string hrdetno = string.Empty;
            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + per_from_date.ToString() + "' and '" + per_to_date.ToString() + "'";
            ds_sphr = d2.select_method(getsphr, hat, "Text");
            if (ds_sphr.Tables.Count > 0 && ds_sphr.Tables[0].Rows.Count > 0)
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
            // added by sridhar 27 aug 2014----------------------------start
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            // added by sridhar 27 aug 2014----------------------end
            String splhrquery = "select rights from  special_hr_rights where " + grouporusercode + "";
            splhr_flag = false;
            DataSet dssplhrstartus = d2.select_method(splhrquery, hat, "Text");
            if (dssplhrstartus.Tables.Count > 0 && dssplhrstartus.Tables[0].Rows.Count > 0)
            {
                string spl_hr_rights = dssplhrstartus.Tables[0].Rows[0]["rights"].ToString();
                if (spl_hr_rights == "True" || spl_hr_rights == "true")
                {
                    splhr_flag = true;
                }
            }
            //============ End -----------------------------
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
            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            //hidden By Srinath 25/2/2013
            //mmyycount = ds2.Tables[0].Rows.Count;
            //moncount = mmyycount - 1;
            ds3 = daccess2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
            Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
            Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();
            holiday_table11.Clear();
            holiday_table21.Clear();
            holiday_table31.Clear();
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
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
                    if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                    }
                    // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                }
            }
            if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count > 0)
            {
                for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                {
                    string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                    string[] dummy_split = split_date_time1[0].Split('/');
                    if (!holiday_table21.Contains(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                    {
                        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                    }
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
                    if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                    }
                    if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }
            }
            if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count > 0)
            {
                for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                {
                    string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                    string[] dummy_split = split_date_time1[0].Split('/');
                    if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                    {
                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                    }
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
                    if (!holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                    {
                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }
            }
            //------------------------------------------------------------------
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
        }//=============End 
        next = 0;
        if (dsat.Tables.Count > 0 && dsat.Tables[0].Rows.Count > 0)
        {
            int rowcount = 0;
            int ccount = 0;
            if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count > 0)
            {
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;
            }
            //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
            while (dumm_from_date <= (per_to_date))
            {
                //Added By Srinath 25/2/2013 ==Start
                if (splhr_flag == true)
                {
                    if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                    {
                        getspecial_hr();
                    }
                }
                //=============End
                for (int i = 1; i <= mmyycount; i++)
                {
                    if (cal_from_date == int.Parse(dsat.Tables[0].Rows[next]["month_year"].ToString()))
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
                            if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count > 0)
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
                            if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
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
                                    value = dsat.Tables[0].Rows[next][date].ToString();
                                    if (value != null && value != "0" && value != "7" && value != "")
                                    {
                                        if (tempvalue != value)
                                        {
                                            tempvalue = value;
                                            //Modified By bSrinath
                                            //  for (int j = 0; j < countcount; j++)
                                            for (int j = 0; j < countds; j++)
                                            {
                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                {
                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = countds;
                                                    //j=countcount //Modified By Srinath
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
                                    }
                                    else if (value == "7")
                                    {
                                        per_hhday += 1;
                                    }
                                    else
                                    {
                                        unmark += 1;
                                        my_un_mark++;//Added By Srinath 16/3/2013
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
                                else if (njhr >= minpresI)
                                {
                                    njdate += 0.5;
                                    njdate_mng += 1;
                                }
                                if (per_ondu >= 1)
                                {
                                    Onduty += 0.5;
                                }
                                if (unmark == fnhrs)
                                {
                                    per_holidate_mng += 1;
                                    per_holidate += 0.5;
                                    unmark = 0;
                                }
                                workingdays += 0.5;
                                mng_conducted_half_days += 1;
                            }
                            per_perhrs = 0;
                            per_ondu = 0;
                            per_leave = 0;
                            per_abshrs = 0;
                            // unmark = 0;
                            njhr = 0;
                            int temp_unmark = 0;
                            int k = fnhrs + 1;
                            if (split_holiday_status_2 == "1")
                            {
                                for (i = k; i <= NoHrs; i++)
                                {
                                    date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                    value = dsat.Tables[0].Rows[next][date].ToString();
                                    if (value != null && value != "0" && value != "7" && value != "")
                                    {
                                        if (tempvalue != value)
                                        {
                                            tempvalue = value;
                                            //Modified By bSrinath
                                            //  for (int j = 0; j < countcount; j++)
                                            for (int j = 0; j < countds; j++)
                                            {
                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                {
                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = countds;
                                                    // j = countcount;//Modified By Srinath 25/2/20103
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
                                            per_leave += 1;
                                    }
                                    else if (value == "7")
                                    {
                                        per_hhday += 1;
                                    }
                                    else
                                    {
                                        unmark += 1;
                                        my_un_mark++; //added By Srinath 16/3/2013
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
                                else if (njhr >= minpresII)
                                {
                                    njdate_evng += 1;
                                    njdate += 0.5;
                                }
                                if (per_ondu >= 1)
                                {
                                    Onduty += 0.5;
                                }
                                if (unmark == NoHrs - fnhrs)
                                {
                                    per_holidate_evng += 1;
                                    per_holidate += 0.5;
                                    unmark = 0;
                                }
                                else
                                {
                                    dum_unmark += unmark;
                                }
                                workingdays += 0.5;
                                evng_conducted_half_days += 1;
                            }
                            per_perhrs = 0;
                            per_ondu = 0;
                            per_leave = 0;
                            per_abshrs = 0;
                            unmark = 0;
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
        pre_present_date = Present;
        per_per_hrs = tot_per_hrs;
        per_absent_date = Absent;
        pre_ondu_date = Onduty;
        pre_leave_date = Leave;
        per_workingdays = workingdays - per_holidate - per_njdate;
        per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value;// ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs))); //tot wrkng days
        per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //Added By Srinath 16/3/2013 
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

    //'-------------------------------------------------------------------------------------------------
    //Added By Srinath 25/2/2013

    public void getspecial_hr()
    {
        string hrdetno = string.Empty;
        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
        {
            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));
        }
        if (hrdetno != "")
        {
            DataSet ds_splhr_query_master = new DataSet();
            string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + ds7.Tables[0].Rows[student]["roll"].ToString() + "'  and hrdet_no in(" + hrdetno + ")";
            ds_splhr_query_master = d2.select_method(splhr_query_master, hat, "Text");
            if (ds_splhr_query_master.Tables.Count > 0 && ds_splhr_query_master.Tables[0].Rows.Count > 0)
            {
                for (int splhr = 0; splhr < ds_splhr_query_master.Tables[0].Rows.Count; splhr++)
                {
                    value = ds_splhr_query_master.Tables[0].Rows[0]["attendance"].ToString();
                    if (value != null && value != "0" && value != "7" && value != "")
                    {
                        if (tempvalue != value)
                        {
                            tempvalue = value;
                            for (int j = 0; j < countds; j++)
                            {
                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                {
                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                    j = countds;
                                }
                            }
                        }
                        spl_tot_condut += 1;
                        if (ObtValue == 0)
                        {
                            per_perhrs += 1;
                            tot_per_hrs += 1;
                        }
                    }
                    else
                    {
                        njhr += 1;
                    }
                }
                if (per_perhrs >= minpresII)
                {
                    Present += 0.5;
                }
                else if (njhr == NoHrs)
                {
                    njdate += 0.5;
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
            }
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;

        //FpSpread3.Visible = false;
        Button2.Visible = false;
        //lblEduration.Visible = false;
        lblnorec.Visible = false;
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
        ddlTest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        bindsec();
        GetTest();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        lblerroe.Visible = false;
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
            DateTime dt1 = Convert.ToDateTime(dtfromad);
            if (dt1 > dtnow)
            {
                lblerroe.Visible = false;
                lblerroe.Text = "Please Enter Valid From date";
                lblerroe.Visible = true;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyy");
                return;
            }
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
            DateTime dt1 = Convert.ToDateTime(dtfromad);
            if (dt1 > dtnow)
            {
                lblerroe.Visible = false;
                lblerroe.Text = "Please Enter Valid To date";
                lblerroe.Visible = true;
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyy");
                return;
            }
        }
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            string date2ad = string.Empty;
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
                    TimeSpan ts = dt2 - dt1;
                    int days = ts.Days;
                    if (days < 0)
                    {
                        lblerroe.Text = "From Date Can't Be Greater Than To Date";
                        lblerroe.Visible = true;
                        return;
                    }
                }
            }
        }
        //added by sridhar 03 sep 2014 --------------* End  *-------------------------
        if (ddlTest.Items.Count > 0) //added by sridhar 3 sep 2014 ================start
        {
            if (ddlTest.SelectedItem.Text == "" || ddlTest.SelectedItem.Text == null || ddlTest.SelectedItem.Text == "-1" || ddlTest.SelectedItem.Text == "--Select--")
            {
                lblerroe.Text = "Please Select Any Test";
                lblerroe.Visible = true;
                return;
            }
            else
            {
                lblerroe.Visible = false;
                buttonGo();
            }
        }
        else
        {
            lblerroe.Text = "No Test Found";
            lblerroe.Visible = true;
            return;
        }//added by sridhar 3 sep 2014 ================end
    }

    protected void buttonGo()
    {
        if (ddlBranch.Items.Count == 0)
        {
            return;
        }
        // FpEntry.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = string.Empty;
        TextBoxpage.Text = string.Empty;
        //   FpEntry.CurrentPage = 0;
        if (ddlTest.SelectedIndex != 0)
        {
            lblnorec.Visible = false;
            if ((ddlSec.Enabled == true && ddlSec.Text != "-1") || ddlSec.Enabled == false)
            {
                //sankar edit
                result_analysis_print();
            }
            //if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) == 0)
            //{
            //    Button2.Visible = false;
            //    lblnorec.Visible = true;
            //    //FpSpread3.Visible = false;
            //}
            //else
            //{
            //    Buttontotal.Visible = false;
            //    lblrecord.Visible = false;
            //    DropDownListpage.Visible = false;
            //    TextBoxother.Visible = false;
            //    lblpage.Visible = false;
            //    TextBoxpage.Visible = false;
            //    FpEntry.Visible = false;
            //    //FpSpread3.Visible = true;
            //    Button2.Enabled = true;
            //    //Button2.Visible = true;
            //    Double totalRows = 0;
            //    totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
            //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
            //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            //    DropDownListpage.Items.Clear();
            //    if (totalRows >= 10)
            //    {
            //        FpEntry.Sheets[0].PageSize = 10;
            //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            //        {
            //            DropDownListpage.Items.Add((k + 10).ToString());
            //        }
            //        DropDownListpage.Items.Add("Others");
            //        FpEntry.Height = 335;
            //    }
            //    else if (totalRows == 0)
            //    {
            //        DropDownListpage.Items.Add("0");
            //        FpEntry.Height = 100;
            //    }
            //    else
            //    {
            //        FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //        DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
            //        FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //    }
            //}
        }
    }

    protected void btnprint_Pdf_Click(object sender, EventArgs e)
    {
        bindpdf();
    }

    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
        //lblEtest.Visible = false;
        //   buttonGo();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTest();
        //lblEsection.Visible = false;
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
        Button2.Visible = false;
        lblnorec.Visible = false;
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        //binddegree();
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
            divgrid.Visible = true;
            //FpSpread2.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
        //FpSpread2.CurrentPage = 0;
    }

    void CalculateTotalPages()
    {
        //Double totalRows = 0;
        //totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
        //Buttontotal.Text = "Records : " + totalRows + "  Pages : " + Session["totalPages"];
        //Buttontotal.Visible = true;
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
            case 7:
                atten = "H";
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
        }
        return atten;
    }

    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FpSpread1.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = string.Empty;
        TextBoxpage.Text = string.Empty;
        //FpSpread1.CurrentPage = 0;
        try
        {
            string ranksample = "drop table rankcount";
            SqlCommand rankcmdsample = new SqlCommand(ranksample, con);
            con.Open();
            rankcmdsample.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
            con.Close();
        }
        if ((ddlTest.SelectedIndex != 0) && (ddlTest.Text != ""))
        {
            SpreadBind();
            //result_analysis();
            result_analysis_print();
        }
        else if ((ddlTest.SelectedIndex == 0) || (ddlTest.Text != ""))
        {
            //lblEtest.Visible = true;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
    }

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
    }

    public void bindpdf()
    {


        Font Fontbold = new Font("Times New Roman", 20, FontStyle.Bold);
        Font Fontsmall = new Font("Times New Roman", 18, FontStyle.Regular);
        Font Fontbold1 = new Font("Times New Roman", 14, FontStyle.Bold);
        Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
        //Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
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
        }
        //.........................................................sankar add
        btnExcel.Visible = true;
        btnprint_Pdf.Visible = true;
        norecordlbl.Visible = true;
        txtexcelname.Visible = true;
        //Radiowithoutheader.Visible = true;
        //RadioHeader.Visible = true;
        //ddlpage.Visible = true;
        //lblpages.Visible = true;

        string[] split_batch_deg = new string[10];
        //'-----------------------------------------------------


        branch = ddlBranch.SelectedItem.Text;
        degree = ddlDegree.SelectedItem.Text;
        sem = ddlSemYr.SelectedValue;
        sec = ddlSec.SelectedValue;
        test = ddlTest.SelectedItem.Text;
        //-----------
        DateTime currentdate = System.DateTime.Now;
        string fromdate = currentdate.ToString("yyyy");
        string sem1 = string.Empty;
        string semester1 = "select duration from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
        SqlCommand semcmd = new SqlCommand(semester1, con);
        SqlDataReader semreader;
        con.Close();
        con.Open();
        semreader = semcmd.ExecuteReader();
        if (semreader.HasRows)
        {
            while (semreader.Read())
            {
                sem1 = semreader["duration"].ToString();
            }
        }
        string strsec1 = string.Empty;
        if (sec.ToString() == "All" || sec.ToString() == "" || sec.ToString() == "-1")
        {
            strsec1 = string.Empty;
        }
        else
        {
            strsec1 = " and sections='" + sec.ToString() + "'";
        }
        int todate1 = Convert.ToInt32(ddlBatch.SelectedItem.Text) + Convert.ToInt32(sem1) / 2;
        batch = "Batch :" + Convert.ToInt32(ddlBatch.SelectedItem.Text) + "-" + todate1;
        if (sem == "1")
        {
            sem3 = "II";
            bat = "Odd";
        }
        else if (sem == "2")
        {
            sem3 = "I";
            bat = "Even";
        }
        else if (sem == "3")
        {
            sem3 = "III";
            bat = "Odd";
        }
        else if (sem == "4")
        {
            sem3 = "IV";
            bat = "Even";
        }
        else if (sem == "5")
        {
            sem3 = "V";
            bat = "Odd";
        }
        else if (sem == "6")
        {
            sem3 = "VI";
            bat = "Even";
        }
        else if (sem == "7")
        {
            sem3 = "VII";
            bat = "Odd";
        }
        else if (sem == "8")
        {
            sem3 = "VIII";
            bat = "Even";
        }
        else if (sem == "9")
        {
            sem3 = "IX";
            bat = "Odd";
        }
        else if (sem == "10")
        {
            sem3 = "X";
            bat = "Even";
        }
        if ((sec == "") || (sec == "-1") || (sec == "All"))
        {
            sec = string.Empty;
        }
        if (bat == "Odd")
        {
            academic = "" + fromdate + "-" + (Convert.ToInt32(fromdate) + 1) + "" + (bat) + "";
        }
        else
        {
            academic = "" + (Convert.ToInt32(fromdate) - 1) + "-" + fromdate + "(" + (bat) + ")";
        }
        int pascount = 0;
        string includePastout = string.Empty;


        if (!chkincludepastout.Checked)
        {

            includePastout = "and CC=0";
        }
        string perpass3 = string.Empty;
        temp7 = "select count(*) from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + "and batch_year=" + ddlBatch.SelectedValue.ToString() + " and delflag=0 and exam_flag <> 'DEBAR' " + includePastout + "" + strsec1 + "and current_semester>=" + ddlSemYr.SelectedValue.ToString() + "";
        Strength = GetFunction(temp7);
        //----------------------------------sankar add may20
        filteration();
        string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + ddlSec.SelectedValue.ToString() + "' " + strorder + ",s.subject_no";
        string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";
        hat.Clear();
        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
        hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
        hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
        hat.Add("sections", ddlSec.SelectedValue.ToString());
        hat.Add("filterwithsection", filterwithsection.ToString());
        hat.Add("filterwithoutsection", filterwithoutsection.ToString());
        ds2 = daccess2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
        string sections = string.Empty;
        string strsec = string.Empty;
        int total_pass_fail = 0;
        sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = string.Empty;
        }
        else
        {
            strsec = " and exam_type.sections='" + sections.ToString() + "'";
        }
        string secs = string.Empty;
        if (ddlSec.Text.ToString().Trim() == "-1" || ddlSec.Text.ToString().Trim() == "" || ddlSec.Text.ToString().Trim() == null || ddlSec.Text.ToString().Trim().ToLower() == "all")
        {
            secs = string.Empty;
        }
        else
        {
            secs = ddlSec.SelectedItem.Value;
        }
        //string staff_name =string.Empty;
        //int total_pass_fail = 0;
        //string pren_count =string.Empty;
        //string pass_count =string.Empty;
        //string fail_count =string.Empty;
        //string absent_count =string.Empty;
        int countY = 0;
        bool isStudentStaffSelector = CheckStudentStaffSelector(Convert.ToString(ddlBatch.SelectedValue).Trim());
        if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
            {
                hat.Clear();
                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                hat.Add("section", secs);
                ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");

                if (!isStudentStaffSelector)
                {
                    if (subject_code == "")
                    {
                        subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        subject_name = ds2.Tables[1].Rows[i]["subject_name"].ToString();
                        string temp = string.Empty;
                        if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                        {
                            temp = ds2.Tables[1].Rows[i]["staff_code"].ToString();

                            if (temp != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                            {
                                staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                            }


                            staff_name = staff;
                        }
                        total_pass_fail = Convert.ToInt32(ds4.Tables[8].Rows[0]["PRESENT_COUNT"]);
                        double cal_avg = Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                        cal_avg = Math.Round(cal_avg, 2);
                        double pass_perc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                        pass_perc = Math.Round(pass_perc, 2);
                        pren_count = ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                        pass_count_new = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                        fail_count = ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                        absent_count = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                        pertecount = pass_perc.ToString();
                    }
                    else
                    {
                        subject_code = subject_code + '\n' + ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        subject_name = subject_name + '\n' + ds2.Tables[1].Rows[i]["subject_name"].ToString();
                        string temp1 = string.Empty;
                        if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                        {
                            temp1 = ds2.Tables[1].Rows[i]["staff_code"].ToString();
                            if (temp1 != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                            {
                                staff1 = GetFunction("select staff_name from staffmaster where staff_code = '" + temp1 + "'");
                            }
                            staff_name = staff_name + '\n' + staff1;
                        }
                        total_pass_fail1 = Convert.ToInt32(ds4.Tables[8].Rows[0]["PRESENT_COUNT"]);

                        double cal_avg1 = 0;
                        string SumVal = Convert.ToString(ds4.Tables[0].Rows[0]["SUM"]);
                        if (!string.IsNullOrEmpty(SumVal))
                            cal_avg1 = Convert.ToDouble(SumVal) / Convert.ToDouble(total_pass_fail);
                        cal_avg1 = Math.Round(cal_avg1, 2);

                        double pass_perc1 = 0;
                        string passVal = Convert.ToString(ds4.Tables[1].Rows[0]["PASS_COUNT"]);
                        if (!string.IsNullOrEmpty(passVal))
                            pass_perc1 = (Convert.ToDouble(passVal) / Convert.ToDouble(total_pass_fail1)) * 100;
                        pass_perc1 = Math.Round(pass_perc1, 2);
                        pren_count = pren_count + '\n' + ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                        pass_count_new = pass_count_new + '\n' + ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                        fail_count = fail_count + '\n' + ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                        absent_count = absent_count + '\n' + ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                        pertecount = pertecount + '\n' + pass_perc1.ToString();
                    }
                    countY++;
                }
                else
                {
                    DataSet dsSubjectStaff = new DataSet();
                    DataSet dsPerformance = new DataSet();
                    hat.Clear();
                    hat.Add("examCode", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                    hat.Add("minMarks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                    dsPerformance = daccess2.select_method("usp_CAM_StudentsPerformance", hat, "sp");

                    dsSubjectStaff = daccess2.select_method_wo_parameter("select distinct sfm.staff_name,sfm.staff_code from staff_selector ss, staffmaster sfm,syllabus_master sm,subject s where sm.Batch_Year=ss.batch_year and ss.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and  sm.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and ss.staff_code =sfm.staff_code and s.subject_no='" + Convert.ToString(ds2.Tables[1].Rows[i]["subject_no"]).Trim() + "'", "text");
                    if (dsSubjectStaff.Tables.Count > 0 && dsSubjectStaff.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drSubject in dsSubjectStaff.Tables[0].Rows)
                        {
                            string staffCode = Convert.ToString(drSubject["staff_code"]).Trim();
                            string staffName = Convert.ToString(drSubject["staff_name"]).Trim();
                            hat.Clear();
                            dsPerformance.Clear();
                            dsPerformance.Reset();
                            hat.Add("examCode", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("minMarks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("staffCode", staffCode);
                            hat.Add("isStudentStaffSelector", '1');
                            dsPerformance = daccess2.select_method("usp_CAM_StudentsPerformance", hat, "sp");
                            countY++;
                            if (subject_code == "")
                            {
                                subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                                subject_name = ds2.Tables[1].Rows[i]["subject_name"].ToString();
                                string temp = string.Empty;
                                if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                                {
                                    temp = ds2.Tables[1].Rows[i]["staff_code"].ToString();
                                    #region magesh 24.7.18
                                    //if (temp != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                                    //{
                                    //    staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                                    //}
                                    staff = staffName;
                                    #endregion magesh 24.7.18
                                    staff_name = staff;
                                }
                                total_pass_fail = Convert.ToInt32(dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"]);
                                double cal_avg = 0;  //modified on 15/12/2017
                                string SumVal = Convert.ToString(dsPerformance.Tables[0].Rows[0]["SUM"]);
                                if (!string.IsNullOrEmpty(SumVal))
                                    cal_avg = Convert.ToDouble(SumVal) / Convert.ToDouble(total_pass_fail);
                                cal_avg = Math.Round(cal_avg, 2);
                                double pass_perc = 0;
                                string Passval = Convert.ToString(dsPerformance.Tables[1].Rows[0]["PASS_COUNT"]);
                                if (!string.IsNullOrEmpty(Passval))
                                    pass_perc = (Convert.ToDouble(Passval) / Convert.ToDouble(total_pass_fail)) * 100;
                                pass_perc = Math.Round(pass_perc, 2);
                                pren_count = dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"].ToString();
                                pass_count_new = dsPerformance.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                fail_count = dsPerformance.Tables[4].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                absent_count = dsPerformance.Tables[3].Rows[0]["ABSENT_COUNT"].ToString();
                                pertecount = pass_perc.ToString();
                            }
                            else
                            {
                                subject_code = subject_code + '\n' + ds2.Tables[1].Rows[i]["subject_code"].ToString();
                                subject_name = subject_name + '\n' + ds2.Tables[1].Rows[i]["subject_name"].ToString();
                                string temp1 = string.Empty;
                                if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                                {
                                    temp1 = Convert.ToString(drSubject["staff_code"]).Trim();   //modified by Mullai
                                    if (temp1 != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                                    {
                                        staff1 = GetFunction("select staff_name from staffmaster where staff_code = '" + temp1 + "'");
                                    }
                                    staff_name = staff_name + '\n' + staff1;
                                }
                                total_pass_fail1 = Convert.ToInt32(dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"]);
                                double cal_avg1 = 0;
                                string SumVal = Convert.ToString(dsPerformance.Tables[0].Rows[0]["SUM"]);
                                if (!String.IsNullOrEmpty(SumVal))
                                    cal_avg1 = Convert.ToDouble(SumVal) / Convert.ToDouble(total_pass_fail);
                                cal_avg1 = Math.Round(cal_avg1, 2);
                                double pass_perc1 = 0;
                                string PassSum = Convert.ToString(dsPerformance.Tables[1].Rows[0]["PASS_COUNT"]);
                                if (!string.IsNullOrEmpty(PassSum))
                                    pass_perc1 = (Convert.ToDouble(PassSum) / Convert.ToDouble(total_pass_fail1)) * 100;
                                pass_perc1 = Math.Round(pass_perc1, 2);
                                pren_count = pren_count + '\n' + dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"].ToString();
                                pass_count_new = pass_count_new + '\n' + dsPerformance.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                fail_count = fail_count + '\n' + dsPerformance.Tables[4].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                absent_count = absent_count + '\n' + dsPerformance.Tables[3].Rows[0]["ABSENT_COUNT"].ToString();
                                pertecount = pertecount + '\n' + pass_perc1.ToString();
                            }
                        }
                    }
                }
            }
        }
        if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == "" || sections.ToString().Trim().ToLower() == "-1")
        {
            strsec = string.Empty;
        }
        else
        {
            strsec = " and registration.sections='" + sections.ToString().Trim() + "'";
        }
        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        {
            lblnorec.Visible = false;
            //sqlStr1 = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " order by  len(registration.Roll_No),roll_no "; //modified in 12.04.12
            sqlStr1 = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " " + strregorder + " ";
            con.Close();
            con.Open();
            //added by gowtham
            //---------------------- start -----------------
            if (sqlStr1 != "")
            {
                SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr1, con);
                adaSyll1.Fill(ds7);
                int subrow = 0;
                if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds7.Tables[0].Rows.Count; row++)
                    {
                        fail_sub_cnt = 0;
                        rolnosubno = string.Empty;
                        bool appeared = false;
                        DataView dv_indstudmarks = new DataView();
                        if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                            {
                                if (subrow < ds2.Tables[0].Rows.Count)
                                {
                                    ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds7.Tables[0].Rows[row]["roll"].ToString() + "' and subject_no='" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + "'";
                                    dv_indstudmarks = ds2.Tables[0].DefaultView;
                                    if (dv_indstudmarks.Count > 0)
                                    {
                                        appeared = true;
                                        for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
                                        {
                                            if (dv_indstudmarks[cnt]["mark"].ToString() != "-7" && dv_indstudmarks[cnt]["mark"].ToString() != "-2" && dv_indstudmarks[cnt]["mark"].ToString() != "-3" && (Convert.ToDouble(dv_indstudmarks[cnt]["mark"].ToString()) < Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                            {
                                                fail_sub_cnt++;
                                                if (rolnosubno == string.Empty)
                                                {
                                                    rolnosubno = ds7.Tables[0].Rows[row]["roll"].ToString() + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
                                                }
                                                else
                                                {
                                                    rolnosubno = rolnosubno + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
                                                }
                                            }
                                            subrow++;
                                        }
                                    }
                                }
                            }
                            subjectcount = ds2.Tables[1].Rows.Count;
                            for (int htr = 1; htr <= ds2.Tables[1].Rows.Count; htr++)
                            {
                                if (ht_fail_subject.Contains(Convert.ToString(htr)))
                                {
                                    string val = Convert.ToString(GetCorrespondingKey(htr, ht_fail_subject));
                                    if (fail_sub_cnt == htr)
                                    {
                                        string[] spl_val = val.Split('@');
                                        int value = Convert.ToInt32(spl_val[0].ToString());
                                        value++;
                                        string add_stud = spl_val[1].ToString() + ";" + rolnosubno;
                                        ht_fail_subject[Convert.ToString(htr)] = value + "@" + add_stud;
                                    }
                                }
                                else
                                {
                                    if (fail_sub_cnt == htr)
                                    {
                                        string concat = Convert.ToString(1) + "@" + rolnosubno;
                                        ht_fail_subject.Add(Convert.ToString(htr), concat);
                                    }
                                }
                            }

                            //'--------------to calculat the no.of stud passed in all subj--------------
                            if (fail_sub_cnt == 0 && appeared)
                            {
                                pass_count++;
                            }
                        }
                    }
                    //}
                }
            }
            //------------------------end -----------------
            //
            //if (sqlStr1 != "")
            //{
            //    SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr1, con);
            //    adaSyll1.Fill(ds7);
            //    int subrow = 0;
            //    if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
            //    {
            //        for (int row = 0; row < ds7.Tables[0].Rows.Count; row++)
            //        {
            //            fail_sub_cnt = 0;
            //            rolnosubno =string.Empty;
            //            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
            //            {
            //                if (subrow < ds2.Tables[0].Rows.Count)
            //                {
            //                    if (ds7.Tables[0].Rows[row]["roll"].ToString() == ds2.Tables[0].Rows[subrow]["roll"].ToString())
            //                    {
            //                        if (ds2.Tables[0].Rows[subrow]["mark"].ToString() != "-2" && ds2.Tables[0].Rows[subrow]["mark"].ToString() != "-3" && (Convert.ToDouble(ds2.Tables[0].Rows[subrow]["mark"].ToString()) < Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
            //                        {
            //                            fail_sub_cnt++;
            //                            if (rolnosubno == string.Empty)
            //                            {
            //                                rolnosubno = ds7.Tables[0].Rows[row]["roll"].ToString() + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
            //                            }
            //                            else
            //                            {
            //                                rolnosubno = rolnosubno + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
            //                            }
            //                        }
            //                    }
            //                    subrow++;
            //                }
            //            }
            //            subjectcount = ds2.Tables[1].Rows.Count;
            //            for (int htr = 1; htr <= ds2.Tables[1].Rows.Count; htr++)
            //            {
            //                if (ht_fail_subject.Contains(Convert.ToString(htr)))
            //                {
            //                    string val = Convert.ToString(GetCorrespondingKey(htr, ht_fail_subject));
            //                    if (fail_sub_cnt == htr)
            //                    {
            //                        string[] spl_val = val.Split('-');
            //                        int value = Convert.ToInt32(spl_val[0].ToString());
            //                        value++;
            //                        string add_stud = spl_val[1].ToString() + ";" + rolnosubno;
            //                        ht_fail_subject[Convert.ToString(htr)] = value + "-" + add_stud;
            //                    }
            //                }
            //                else
            //                {
            //                    if (fail_sub_cnt == htr)
            //                    {
            //                        string concat = Convert.ToString(1) + "-" + rolnosubno;
            //                        ht_fail_subject.Add(Convert.ToString(htr), concat);
            //                    }
            //                }
            //            }
            //            //'--------------to calculat the no.of stud passed in all subj--------------
            //            if (fail_sub_cnt == 0)
            //            {
            //                pass_count++;
            //            }
            //        }
            //    }
            //}
            //No of Students Passed in all Subject.....................................
            //no_of_studentpass = pass_count.ToString();
            string sec1 = string.Empty;
            if (ddlSec.Text.ToString().Trim() == "-1" || ddlSec.Text.ToString().Trim() == "" || ddlSec.Text.ToString().Trim() == null || ddlSec.Text.ToString().Trim().ToLower() == "all")
            {
                sec1 = string.Empty;
            }
            else
            {
                sec1 = ddlSec.SelectedItem.Text;
            }
            if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
            {
                for (int passtot = 0; passtot < ds2.Tables[1].Rows.Count; passtot++)
                {
                    hat.Clear();
                    hat.Add("exam_code", ds2.Tables[1].Rows[passtot]["exam_code"].ToString());
                    hat.Add("min_marks", ds2.Tables[1].Rows[passtot]["min_mark"].ToString());
                    hat.Add("section", sec1);
                    ds8 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");
                    //total_pass_count---------------------------------------
                    total_pass_count = total_pass_count + Convert.ToInt32(ds8.Tables[1].Rows[0]["PASS_COUNT"].ToString());
                }
            }
            pass_percentage = (Convert.ToDouble(pass_count) / Convert.ToDouble(Strength)) * 100;
            pass_percentage = Math.Round(pass_percentage, 2);
            //Percentage of Students Passed in all Subject.....................................................
            Percentage_of_Students = pass_percentage.ToString();
            //Failure count calculate................................................ 
            foreach (DictionaryEntry parameter in ht_fail_subject)
            {
                lblnorec.Visible = false;
                string htkey = Convert.ToString(parameter.Key);
                string htvalu = Convert.ToString(parameter.Value);
                string[] spl_htval = htvalu.Split('@');
                if (FailureSubj_Count == "")
                {
                    FailureSubj_Count = "Failure in" + " " + (htkey) + " " + "subjects";
                    spl_htval_Count = spl_htval[0].ToString() + "";
                }
                else
                {
                    FailureSubj_Count = FailureSubj_Count + '\n' + "Failure in" + " " + (htkey) + " " + "subjects";
                    spl_htval_Count = spl_htval_Count + '\n' + spl_htval[0].ToString() + "";
                }
            }
            //  '--------------------------attendance percentage--------------------------------------
            hat.Clear();
            hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
            hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                eligiblepercent = int.Parse(ds.Tables[0].Rows[0]["Eligible_Percent"].ToString());
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                countds = ds1.Tables[0].Rows.Count;
            }
            if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
            {
                for (int att = 0; att < ds7.Tables[0].Rows.Count; att++)
                {
                    persentmonthcal();
                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }
                    //modified By Srinath 23/2/2013 
                    //per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
                    per_con_hrs = per_workingdays1 + spl_tot_condut;
                    per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);
                    if (per_tage_hrs > 100)
                    {
                        per_tage_hrs = 100;
                    }
                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                    dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
                    if (dum_tage_hrs == "NaN")
                    {
                        dum_tage_hrs = "0";
                    }
                    else if (dum_tage_hrs == "Infinity")
                    {
                        dum_tage_hrs = "0";
                    }
                    if (dum_tage_date == "NaN")
                    {
                        dum_tage_date = "0";
                    }
                    else if (dum_tage_date == "Infinity")
                    {
                        dum_tage_date = "0";
                    }
                    //'------------------------------------------------new end------------
                    //'----------------------adding the percentage below 80 % to hash table-----------------------------
                    if (htattperc.Contains(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower()))
                    {
                        int value1 = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rol_no).Trim().ToLower(), htattperc));
                        value1++;//fail count
                        htattperc[Convert.ToString(rol_no).Trim().ToLower()] = value1;
                    }
                    else
                    {
                        if (rdattnd_daywise.Checked == true)
                        {
                            if (Convert.ToDouble(dum_tage_date) < Convert.ToDouble(eligiblepercent))
                            {
                                htattperc.Add(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower(), dum_tage_date.ToString());
                            }
                        }
                        else if (rdattnd_hourwise.Checked == true)
                        {
                            if (Convert.ToDouble(dum_tage_hrs) < Convert.ToDouble(eligiblepercent))
                            {
                                htattperc.Add(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower(), dum_tage_hrs.ToString());
                            }
                        }
                    }
                    student++;
                }
            }
            int serialno = 0;
            //added by gowtham
            //----------------
            if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
            {
                for (int order = 0; order < ds7.Tables[0].Rows.Count; order++)
                {
                    string roll = ds7.Tables[0].Rows[order]["roll"].ToString();
                    if (htattperc.ContainsKey(roll))
                    {
                        serialno++;
                        string key1 = roll.ToString();
                        string value1 = (GetCorrespondingKey(key1, htattperc).ToString());
                        string studname = GetFunction("select stud_name from registration where roll_no='" + key1.ToString() + "'");
                        string eli_mark = string.Empty;
                        eli_mark = eligiblepercent.ToString();
                        if (serialno == 1)
                        {
                            serialno1 = serialno.ToString();
                            keyvalue = key1.ToString();
                            stud_name_final = studname.ToString();
                            attenPer = value1.ToString();
                        }
                        else
                        {
                            serialno1 = serialno1 + '\n' + serialno.ToString();
                            keyvalue = keyvalue + '\n' + key1.ToString();
                            stud_name_final = stud_name_final + '\n' + studname.ToString();
                            attenPer = attenPer + '\n' + value1.ToString();
                        }
                    }
                }
                NoofStudents = ds7.Tables[0].Rows.Count.ToString();
            }
            //-----------------------
            //foreach (DictionaryEntry parameter in htattperc)
            //{
            //    serialno++;
            //    string key1 = parameter.Key.ToString();
            //    string value1 = parameter.Value.ToString();
            //    string studname = GetFunction("select stud_name from registration where roll_no='" + key1.ToString() + "'");
            //    string eli_mark =string.Empty;
            //    //Attendance Below %............................
            //    eli_mark = eligiblepercent.ToString();
            //    if (serialno == 1)
            //    {
            //        serialno1 = serialno.ToString();
            //        keyvalue = key1.ToString();
            //        stud_name_final = studname.ToString();
            //        attenPer = value1.ToString();
            //    }
            //    else
            //    {
            //        serialno1 = serialno1 + '\n' + serialno.ToString();
            //        keyvalue = keyvalue + '\n' + key1.ToString();
            //        stud_name_final = stud_name_final + '\n' + studname.ToString();
            //        attenPer = attenPer + '\n' + value1.ToString();
            //    }
            //}

        }
        else
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Test has not been conducted for any subject";
            divgrid.Visible = false;
            //RadioHeader.Visible = false;
            //Radiowithoutheader.Visible = false;
            ddlpage.Visible = false;
            lblpages.Visible = false;
            btnExcel.Visible = false;
            norecordlbl.Visible = false;
            txtexcelname.Visible = false;
        }
        generateletterformat1(mydocument, Fontsmall, Fontbold, Fontbold1, ds2.Tables[1], Response, countY);
    }

    public void generateletterformat1(Gios.Pdf.PdfDocument mydocument, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt, HttpResponse response, int countY)
    {
        try
        {
            contentDiv.InnerHtml = "";
            string subj_name_new = string.Empty;
            string staff_val = string.Empty;
            string pre_count_new = string.Empty;
            string pass_count_val = string.Empty;
            string fail_count_val = string.Empty;
            string absent_count_val = string.Empty;
            string preg_count_val = string.Empty;
            int subno = 0;
            int pagecount = countY / 10;
            int repage = countY % 10;
            int nopages = pagecount;
            if (repage > 0)
            {
                nopages++;
            }
            //int subno1 = 0;
            int cntperc = htattperc.Count;
            int cnt1_pagescount = 0;
            int last_page_count = 0;
            int last_per_page = 0;
            int cnt_fail = 0;
            int pagecount_fb2 = cntperc / 38;
            int repage_fb2 = cntperc % 38;
            if (repage_fb2 > 0)
            {
                pagecount_fb2++;
            }
            if (cntperc > 0)
            {
                cnt_fail = ht_fail_subject.Count;
                //sankar edit May'30//////////////////////
                //cnt1_pagescount = cnt_fail + 3;
                if (cnt_fail > 0)
                {
                    cnt1_pagescount = cnt_fail + 2 + pagecount_fb2;
                }
                else
                {
                    cnt1_pagescount = cnt_fail + 1 + pagecount_fb2;
                }
                last_page_count = cnt1_pagescount - 1;
                last_per_page = cnt1_pagescount - 2;
            }
            else
            {
                cnt_fail = ht_fail_subject.Count;
                cnt1_pagescount = cnt_fail + 2;
                last_page_count = cnt1_pagescount;
                last_per_page = cnt1_pagescount - 2;
            }
            int totalsubject = 0;
            if (nopages > 0)
            {
                PdfTablePage newpdftabpage;
                for (int row = 0; row < cnt1_pagescount; row++)
                {
                    lblnorec.Visible = false;
                    subno++;
                    if (subno == 2)
                        continue;

                    //table create for pdf...............................
                    //start

                    //int cnt = subno * sno;
                    //int cnt1 = subno * 20;
                    html.Append("<div style='height: 1000px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'>");

                    html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'>");

                    html.Append("<tr><td style='width: 50px;'></td><td style='text-align: right;' > <img src=~/college/Left_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + collnamenew1 + "</span> <br><span style='font-size: 14px;font-weight:bold;'>" + address + " <br> " + phnfax + "  <br>" + email + "<br> </span></td><td style='text-align: right;' > <img src=~/college/Right_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td></tr><tr><td style='width: 50px;'></td><td></td><td ></td><td style='text-align: right;' ></td></tr> ");

                    html.Append(" </table>");
                    if (subno == 1)
                    {

                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");

                        html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: left;'><span style='font-weight:bold;'><br> " + degree + "Branch: [" + branch + "] </span> <span ><br><br> Semester: " + sem + "  -" + sec + " Sec <br><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " + test + "-" + "Result Analysis<br><br>Subject Wise Percentage &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Class Strength" + ":" + " " + Strength + "<br><br> </span></td></tr> ");

                        html.Append(" </table>");



                        int cnt = countY;
                        int sheetNo = 1;
                        int rows = (cnt % 13 != 0) ? cnt : 13;




                        html.Append("<br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                        html.Append("<tr>");



                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';> Subject Code </td>");


                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Subject Name</td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';> Staff Name </td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Present</td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Passed</td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Failed</td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Absents</td>");

                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Pass%</td>");
                        html.Append(" </tr>");
                        int val = 0;
                        double posY = 300;
                        double posYNew = 0;
                        for (int i = 0; i < cnt; i++)
                        {
                            if (i % 13 == 0 && i != 0)
                            {


                                html.Append(" </table>");
                                html.Append("</div>");
                                html.Append("<div style='height: 1000px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'>");

                                html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'>");

                                html.Append("<tr><td style='width: 50px;'></td><td style='text-align: right;' > <img src=~/college/Left_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + collnamenew1 + "</span> <br><span style='font-size: 14px;font-weight:bold;'>" + address + " <br> " + phnfax + "  <br>" + email + "<br> </span></td><td style='text-align: right;' > <img src=~/college/Right_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td></tr><tr><td style='width: 50px;'></td><td></td><td ></td><td style='text-align: right;' ></td></tr> ");

                                html.Append(" </table>");
                                html.Append("<br><br><br><br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                                html.Append("<tr>");



                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';> Subject Code </td>");


                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Subject Name</td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';> Staff Name </td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Present</td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Passed</td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Failed</td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Absents</td>");

                                html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Pass%</td>");
                                html.Append(" </tr>");
                                //table create for pdf...............................
                                //start


                                rows = ((cnt - (sheetNo * 13)) % 13 != 0) ? (cnt - (sheetNo * 13)) : 13;

                                val = 0;
                                sheetNo++;
                            }

                            html.Append("<tr>");

                            val++;

                            string[] splitsubjecode = subject_code.Split(new Char[] { '\n' });
                            subjeccode = splitsubjecode[i];

                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: left;'>");
                            html.Append("" + subjeccode + "");
                            html.Append("</td>");

                            totalsubject = splitsubjecode.GetUpperBound(0);//Added by srinath 2/8/2014

                            string[] subjec_name = subject_name.Split(new Char[] { '\n' });
                            subj_name_new = subjec_name[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: left;'>");
                            html.Append("" + subj_name_new + "");
                            html.Append("</td>");



                            string[] staf_name = staff_name.Split(new Char[] { '\n' });
                            staff_val = staf_name[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: left;'>");
                            html.Append("" + staff_val + "");
                            html.Append("</td>");


                            string[] present_count = pren_count.Split(new Char[] { '\n' });
                            pre_count_new = present_count[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: center;'>");
                            html.Append("" + pre_count_new + "");
                            html.Append("</td>");


                            string[] passnew_count = pass_count_new.Split(new Char[] { '\n' });
                            pass_count_val = passnew_count[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: center;'>");
                            html.Append("" + pass_count_val + "");
                            html.Append("</td>");



                            string[] failstu = fail_count.Split(new Char[] { '\n' });
                            fail_count_val = failstu[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: center;'>");
                            html.Append("" + fail_count_val + "");
                            html.Append("</td>");



                            string[] absent = absent_count.Split(new Char[] { '\n' });
                            absent_count_val = absent[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: center;'>");
                            html.Append("" + absent_count_val + "");
                            html.Append("</td>");



                            string[] pescount = pertecount.Split(new Char[] { '\n' });
                            preg_count_val = pescount[i];


                            html.Append("<td class='style1' style='border: thin solid #000000;");
                            html.Append("text-align: center;'>");
                            html.Append("" + preg_count_val + "");
                            html.Append("</td>");
                            html.Append(" </tr>");

                            if (cnt < 13)
                                posY = 260;
                            else
                                posY = 190;
                        }

                        html.Append(" </table>");
                        //  Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 300, 700, 1000));



                        //end......................................
                        //bind first table...........
                        //PdfTextArea pt123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydocument, 25, 920, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_________________________________________________________________________________________");
                        //mypdfpage.Add(pt123);
                        int cnt1 = ht_fail_subject.Count;
                        if (posYNew + (cnt1 * 25) >= 920)
                        {

                            html.Append("</div>");
                            html.Append("<div style='height: 1000px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'>");

                            html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'>");

                            html.Append("<tr><td style='width: 50px;'></td><td style='text-align: right;' > <img src=~/college/Left_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + collnamenew1 + "</span> <br><span style='font-size: 14px;font-weight:bold;'>" + address + " <br> " + phnfax + "  <br>" + email + "<br> </span></td><td style='text-align: right;' > <img src=~/college/Right_Logo.jpeg alt='' style='height: 100px; width: 120px;' /></td></tr><tr><td style='width: 50px;'></td><td></td><td ></td><td style='text-align: right;' ></td></tr> ");

                            html.Append(" </table>");
                            //table create for pdf...............................
                            //start


                            posYNew = 180;
                        }
                        if (cnt_fail > 0)
                        {



                            html.Append("<br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                            html.Append("<tr>");


                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Failure_Subject</td>");

                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Failure_Count</td>");


                            html.Append(" </tr>");
                            int val1 = 0;
                            if (cnt1 < 20)
                            {
                                //nopages++;
                                for (int i = 0; i < cnt1; i++)
                                {
                                    html.Append("<tr>");
                                    val1++;

                                    string failurecount_new = string.Empty;
                                    string subjectfailcount = string.Empty;

                                    string[] failurestud = FailureSubj_Count.Split(new Char[] { '\n' });
                                    failurecount_new = failurestud[i];


                                    html.Append("<td class='style1' style='border: thin solid #000000;");
                                    html.Append("text-align: left;'>");
                                    html.Append("" + failurecount_new + "");
                                    html.Append("</td>");


                                    string[] subjcount = spl_htval_Count.Split(new Char[] { '\n' });
                                    subjectfailcount = subjcount[i];


                                    html.Append("<td class='style1' style='border: thin solid #000000;");
                                    html.Append("text-align: center;'>");
                                    html.Append("" + subjectfailcount + "");
                                    html.Append("</td>");
                                    html.Append(" </tr>");



                                }
                                html.Append(" </table>");
                            }


                        }




                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");

                        html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: left;'> <span ><br><br><br><br><br><br> No of Students Passed in all Subject =" + pass_count + " <br><br>Percentage of Students Passed in all Subject =" + Percentage_of_Students + "<br><br><br><br><br>Class Advisor&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp HOD<br><br><br> </span></td></tr> ");

                        html.Append(" </table>");


                    }
                    if (cnt_fail > 0)
                    {
                        if (subno == 2)
                        {

                        }
                        //'-----------loop for displaying the stud name-------------
                        if (subno > 2)
                        {
                            if (subno <= last_page_count)
                            {
                                int check = 2;
                                foreach (DictionaryEntry param in ht_fail_subject)//'-----------loop for displaying the stud name-------------
                                {
                                    lblnorec.Visible = false;
                                    lblnorec.Text = string.Empty;
                                    //sankar changes
                                    check++;
                                    if (subno == check)
                                    {
                                        string htkey1 = Convert.ToString(param.Key);
                                        string htval = Convert.ToString(param.Value);
                                        string serialno = string.Empty;
                                        string stud_RollNo = string.Empty;
                                        string stud_Name = string.Empty;
                                        string stud_subj = string.Empty;
                                        string indu_Failure = string.Empty;
                                        indu_Failure = "Failure in" + " " + htkey1 + " " + "subjects";
                                        string[] spl_htval = htval.Split('@');
                                        string[] spl_count = spl_htval[1].Split(';');
                                        int cnt2 = spl_count.Length;
                                        int totalcnt = cnt2 + (spl_count.Length);
                                        //int cntlen_count = cnt2 / 10;
                                        //int pages = cnt2 % 10;
                                        //int subno1 = 0;
                                        //int nopages1 = cntlen_count;
                                        //if (pages > 0)
                                        //{
                                        //    nopages1++;
                                        //}
                                        int serial = 0;
                                        string tempsub = string.Empty;
                                        //if (totalcnt == 10)
                                        //{


                                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");

                                        html.Append("<tr><td style='font-size: 14px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'> <span ><br><br> " + indu_Failure + " <br> </span></td></tr> ");

                                        html.Append(" </table>");

                                        html.Append("<br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                                        html.Append("<tr>");


                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>S.No</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Roll No</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Name</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Subjects</td>");


                                        html.Append(" </tr>");



                                        int val2 = 0;
                                        for (int printrw = 0; printrw < Convert.ToInt32(spl_htval[0].ToString()); printrw++)
                                        {
                                            serial++;
                                            tempsub = string.Empty;
                                            string[] spl_stud = spl_count[printrw].Split(',');
                                            if (stud_RollNo == "")
                                            {
                                                serialno = serial + "";
                                                stud_RollNo = spl_stud[0].ToString();
                                                string studname = GetFunction("select stud_name from registration where Roll_No='" + spl_stud[0].ToString() + "'");
                                                stud_Name = studname;
                                                for (int subcnt = 1; subcnt <= spl_stud.GetUpperBound(0); subcnt++)
                                                {
                                                    if (tempsub == "")
                                                    {
                                                        tempsub = spl_stud[subcnt].ToString();
                                                    }
                                                    else
                                                    {
                                                        tempsub = tempsub + "," + spl_stud[subcnt].ToString();
                                                    }
                                                }
                                                if (tempsub != "")
                                                {
                                                    tempsub = "in(" + tempsub + ")";
                                                }
                                                string subname = "select subject_name from subject where subject_no " + tempsub + "";
                                                SqlDataAdapter da_subnam = new SqlDataAdapter(subname, con);
                                                con.Close();
                                                con.Open();
                                                DataSet ds9 = new DataSet();
                                                da_subnam.Fill(ds9);
                                                string displayname = string.Empty;
                                                if (ds9.Tables.Count > 0 && ds9.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int subnamerw = 0; subnamerw < ds9.Tables[0].Rows.Count; subnamerw++)
                                                    {
                                                        if (displayname == "")
                                                        {
                                                            displayname = ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            displayname = displayname + "," + ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                                                        }
                                                    }
                                                }
                                                stud_subj = displayname.ToString();
                                            }
                                            else
                                            {
                                                serialno = serialno + '\n' + serial + "";
                                                stud_RollNo = stud_RollNo + '\n' + spl_stud[0].ToString();
                                                string studname = GetFunction("select stud_name from registration where Roll_No='" + spl_stud[0].ToString() + "'");
                                                stud_Name = stud_Name + '\n' + studname;
                                                for (int subcnt = 1; subcnt <= spl_stud.GetUpperBound(0); subcnt++)
                                                {
                                                    if (tempsub == "")
                                                    {
                                                        tempsub = spl_stud[subcnt].ToString();
                                                    }
                                                    else
                                                    {
                                                        tempsub = tempsub + "," + spl_stud[subcnt].ToString();
                                                    }
                                                }
                                                if (tempsub != "")
                                                {
                                                    tempsub = "in(" + tempsub + ")";
                                                }
                                                string subname = "select subject_name from subject where subject_no " + tempsub + "";
                                                SqlDataAdapter da_subnam = new SqlDataAdapter(subname, con);
                                                con.Close();
                                                con.Open();
                                                DataSet ds9 = new DataSet();
                                                da_subnam.Fill(ds9);
                                                string displayname = string.Empty;
                                                if (ds9.Tables.Count > 0 && ds9.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int subnamerw = 0; subnamerw < ds9.Tables[0].Rows.Count; subnamerw++)
                                                    {
                                                        if (displayname == "")
                                                        {
                                                            displayname = ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            displayname = displayname + "," + ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                                                        }
                                                    }
                                                }
                                                stud_subj = stud_subj + '\n' + displayname.ToString();
                                            }
                                        }
                                        string serialval = string.Empty;
                                        string roll_no_stud = string.Empty;
                                        string stud_name_val = string.Empty;
                                        string displ_valu = string.Empty;
                                        int val3 = 0;
                                        //int cnt3 = spl_count.Length;
                                        for (i = 0; i < cnt2; i++)
                                        {
                                            html.Append("<tr>");
                                            val3++;

                                            string[] seri = serialno.Split(new Char[] { '\n' });
                                            serialval = seri[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + serialval + "");
                                            html.Append("</td>");

                                            string[] stud_roll = stud_RollNo.Split(new Char[] { '\n' });
                                            roll_no_stud = stud_roll[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: left;'>");
                                            html.Append("" + roll_no_stud + "");
                                            html.Append("</td>");

                                            string[] stud_val = stud_Name.Split(new Char[] { '\n' });
                                            stud_name_val = stud_val[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: left;'>");
                                            html.Append("" + stud_name_val + "");
                                            html.Append("</td>");

                                            string[] dis_stud = stud_subj.Split(new Char[] { '\n' });
                                            displ_valu = dis_stud[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: left;'>");
                                            html.Append("" + displ_valu + "");
                                            html.Append("</td>");
                                            html.Append(" </tr>");
                                        }
                                        html.Append(" </table>");

                                        //}
                                    }
                                }
                            }
                        }
                    }
                    if (cntperc > 0)
                    {
                        //if (last_page_count <= subno)
                        if (totalsubject + 2 < row)//modified by srinath 2/9/2014
                        {




                            html.Append("<br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                            html.Append("<tr>");


                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>S.No</td>");

                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>RollNo</td>");

                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Name</td>");

                            html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Attendance%</td>");


                            html.Append(" </tr>");
                            //if (subno == last_page_count)
                            if (totalsubject + 3 == row)//modified by srinath 2/9/2014
                            {
                                if (cntperc < 38)
                                {
                                    for (int i = 0; i < cntperc; i++)
                                    {
                                        html.Append("<tr>");
                                        valPer++;

                                        string[] serill = serialno1.Split(new Char[] { '\n' });
                                        Serial_New = serill[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Serial_New + "");
                                        html.Append("</td>");

                                        string[] roll = keyvalue.Split(new Char[] { '\n' });
                                        Roll_Stude = roll[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Roll_Stude + "");
                                        html.Append("</td>");

                                        string[] name_stu = stud_name_final.Split(new Char[] { '\n' });
                                        stud_Name_Cnt = name_stu[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: left;'>");
                                        html.Append("" + stud_Name_Cnt + "");
                                        html.Append("</td>");

                                        string[] att = attenPer.Split(new Char[] { '\n' });
                                        Attendance = att[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Attendance + "");
                                        html.Append("</td>");
                                        html.Append(" </tr>");
                                    }
                                    html.Append(" </table>");


                                    html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");

                                    html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'> <span ><br><br> No.of.Students: " + ds7.Tables[0].Rows.Count.ToString() + " <br> </span></td></tr> ");

                                    html.Append(" </table>");
                                }
                                else
                                {
                                    html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");
                                    html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'> <span ><br><br> Attendance Below " + eligiblepercent + "% <br> </span></td></tr> ");

                                    html.Append(" </table>");




                                    html.Append("<br><br><br><br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                                    html.Append("<tr>");


                                    html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>S.No</td>");

                                    html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>RollNo</td>");

                                    html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Name</td>");

                                    html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Attendance%</td>");


                                    html.Append(" </tr>");
                                    for (int i = 0; i < 38; i++)
                                    {
                                        html.Append("<tr>");
                                        valPer++;

                                        string[] serill = serialno1.Split(new Char[] { '\n' });
                                        Serial_New = serill[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Serial_New + "");
                                        html.Append("</td>");

                                        string[] roll = keyvalue.Split(new Char[] { '\n' });
                                        Roll_Stude = roll[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Roll_Stude + "");
                                        html.Append("</td>");

                                        string[] name_stu = stud_name_final.Split(new Char[] { '\n' });
                                        stud_Name_Cnt = name_stu[i];

                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: left;'>");
                                        html.Append("" + stud_Name_Cnt + "");
                                        html.Append("</td>");

                                        string[] att = attenPer.Split(new Char[] { '\n' });
                                        Attendance = att[i];


                                        html.Append("<td class='style1' style='border: thin solid #000000;");
                                        html.Append("text-align: center;'>");
                                        html.Append("" + Attendance + "");
                                        html.Append("</td>");
                                        html.Append(" </tr>");
                                    }
                                    html.Append(" </table>");
                                }
                            }
                            int valPer1 = 2;
                            if (subno > last_page_count)
                            {
                                if (cntperc > 38)//added by srinath 14/8/2014
                                {
                                    valPer = (valPer1 - 1) * 38;
                                    int ro = 0;
                                    int remaindsubs = cntperc - 38;
                                    if (remaindsubs < 38)
                                    {
                                        //Gios.Pdf.PdfTable tableper1 = mydocument.NewTable(Fontsmall, remaindsubs + 1, 4, 1);
                                        //tableper1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);



                                        html.Append("<br><br><br><br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                                        html.Append("<tr>");


                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>S.No</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>RollNo</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Name</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Attendance%</td>");


                                        html.Append(" </tr>");

                                        for (int fg = 0; fg < remaindsubs; fg++)
                                        {
                                            html.Append("<tr>");
                                            ro++;

                                            string[] serill = serialno1.Split(new Char[] { '\n' });
                                            Serial_New = serill[valPer];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Serial_New + "");
                                            html.Append("</td>");

                                            string[] roll = keyvalue.Split(new Char[] { '\n' });
                                            Roll_Stude = roll[valPer];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Roll_Stude + "");
                                            html.Append("</td>");

                                            string[] name_stu = stud_name_final.Split(new Char[] { '\n' });
                                            stud_Name_Cnt = name_stu[valPer];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: left;'>");
                                            html.Append("" + stud_Name_Cnt + "");
                                            html.Append("</td>");

                                            string[] att = attenPer.Split(new Char[] { '\n' });
                                            Attendance = att[valPer];

                                            valPer++;
                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Attendance + "");
                                            html.Append("</td>");
                                            html.Append(" </tr>");
                                        }
                                        html.Append(" </table>");



                                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");
                                        html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'> <span ><br><br><br> No.of.Students: " + ds7.Tables[0].Rows.Count.ToString() + "<br> </span></td></tr> ");

                                        html.Append(" </table>");
                                    }
                                    else
                                    {

                                        html.Append("<br><br><br><br><br><table style='width: 95%; margin-left: 80px; margin-top: 0px; margin-bottom: 2px; font-size: 12px;'cellpadding='5' cellspacing='0'>");
                                        html.Append("<tr>");


                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>S.No</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>RollNo</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Name</td>");

                                        html.Append("<td  style='border: thin solid #000000;' align='center'; font-weight:'bold';>Attendance%</td>");


                                        html.Append(" </tr>");
                                        for (int fg = 0; fg < 38; fg++)
                                        {
                                            html.Append("<tr>");
                                            ro++;

                                            string[] serill = serialno1.Split(new Char[] { '\n' });
                                            Serial_New = serill[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Serial_New + "");
                                            html.Append("</td>");

                                            string[] roll = keyvalue.Split(new Char[] { '\n' });
                                            Roll_Stude = roll[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Roll_Stude + "");
                                            html.Append("</td>");

                                            string[] name_stu = stud_name_final.Split(new Char[] { '\n' });
                                            stud_Name_Cnt = name_stu[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: left;'>");
                                            html.Append("" + stud_Name_Cnt + "");
                                            html.Append("</td>");

                                            string[] att = attenPer.Split(new Char[] { '\n' });
                                            Attendance = att[i];

                                            html.Append("<td class='style1' style='border: thin solid #000000;");
                                            html.Append("text-align: center;'>");
                                            html.Append("" + Attendance + "");
                                            html.Append("</td>");
                                            html.Append(" </tr>");
                                        }
                                        html.Append(" </table>");


                                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 80px;' border='0'>");
                                        html.Append("<tr><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'> <span ><br><br><br> No.of.Students: " + ds7.Tables[0].Rows.Count.ToString() + "<br></span></td></tr> ");

                                        html.Append(" </table>");
                                    }
                                }//added by srinath 2/8/2014
                            }

                        }
                    }


                    contentDiv.InnerHtml = html.ToString();
                    contentDiv.Visible = true;

                    ScriptManager.RegisterStartupScript(this, GetType(), "btn_print", "PrintDiv();", true);
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        lblnorec.Visible = false;
                        lblnorec.Text = string.Empty;

                    }


                    html.Append("</div>");
                }
            }
        }
        catch
        {
        }
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

    protected void result_analysis_print()
    {
        //try
        //{
        btnExcel.Visible = true;
        btnprint_Pdf.Visible = true;
        norecordlbl.Visible = true;
        txtexcelname.Visible = true;
        //Radiowithoutheader.Visible = true;
        //RadioHeader.Visible = true;
        //ddlpage.Visible = true;
        //lblpages.Visible = true;

        string[] split_batch_deg = new string[10];
        //'-----------------------------------------------------

        string branch = ddlBranch.SelectedItem.Text;
        string degree = ddlDegree.SelectedItem.Text;
        string sem = ddlSemYr.SelectedValue;
        string sec = ddlSec.SelectedValue;
        string test = ddlTest.SelectedItem.Text;
        //-----------
        DateTime currentdate = System.DateTime.Now;
        string fromdate = currentdate.ToString("yyyy");
        string sem1 = string.Empty;
        string semester1 = "select duration from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
        SqlCommand semcmd = new SqlCommand(semester1, con);
        SqlDataReader semreader;
        con.Close();
        con.Open();
        semreader = semcmd.ExecuteReader();
        if (semreader.HasRows)
        {
            while (semreader.Read())
            {
                sem1 = semreader["duration"].ToString();
            }
        }
        string strsec1 = string.Empty;
        if (sec.ToString().Trim().ToLower() == "all" || sec.ToString().Trim().ToLower() == "" || sec.ToString().Trim().ToLower() == "-1")
        {
            strsec1 = string.Empty;
        }
        else
        {
            strsec1 = " and sections='" + sec.ToString() + "'";
        }
        int todate1 = Convert.ToInt32(ddlBatch.SelectedItem.Text) + Convert.ToInt32(sem1) / 2;
        string batch = "Batch :" + Convert.ToInt32(ddlBatch.SelectedItem.Text) + "-" + todate1;
        string sem3 = string.Empty;
        string bat = string.Empty;
        string academic = string.Empty;
        if (sem == "1")
        {
            sem3 = "II";
            bat = "Odd";
        }
        else if (sem == "2")
        {
            sem3 = "I";
            bat = "Even";
        }
        else if (sem == "3")
        {
            sem3 = "III";
            bat = "Odd";
        }
        else if (sem == "4")
        {
            sem3 = "IV";
            bat = "Even";
        }
        else if (sem == "5")
        {
            sem3 = "V";
            bat = "Odd";
        }
        else if (sem == "6")
        {
            sem3 = "VI";
            bat = "Even";
        }
        else if (sem == "7")
        {
            sem3 = "VII";
            bat = "Odd";
        }
        else if (sem == "8")
        {
            sem3 = "VIII";
            bat = "Even";
        }
        else if (sem == "9")
        {
            sem3 = "IX";
            bat = "Odd";
        }
        else if (sem == "10")
        {
            sem3 = "X";
            bat = "Even";
        }
        if ((sec == "") || (sec == "-1") || (sec == "All"))
        {
            sec = string.Empty;
        }
        if (bat == "Odd")
        {
            academic = "" + fromdate + "-" + (Convert.ToInt32(fromdate) + 1) + "" + (bat) + "";
        }
        else
        {
            academic = "" + (Convert.ToInt32(fromdate) - 1) + "-" + fromdate + "(" + (bat) + ")";
        }
        int pascount = 0;
        string includePastout = string.Empty;


        if (!chkincludepastout.Checked)
            includePastout = "and CC='0'";
        string perpass3 = string.Empty;
        string temp7 = "select count(*) from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + "and batch_year=" + ddlBatch.SelectedValue.ToString() + " and delflag=0 and exam_flag <> 'DEBAR' " + includePastout + "" + strsec1 + "and current_semester>=" + ddlSemYr.SelectedValue.ToString() + "";
        string Strength = GetFunction(temp7);

        divgrid.Visible = true;

        //  FpSpread2.Sheets[0].AutoPostBack = false;


        for (int headcol = 0; headcol < 8; headcol++)
            dtl.Columns.Add("", typeof(string));

        for (int rowcount1 = 0; rowcount1 < 11; rowcount1++)
        {
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
        }
        //FpSpread2.Sheets[0].PageSize = 150;

        //for logo




        //'------------------------for settings the header based on print master table
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "Resultanalysis.aspx");
        dsprint = daccess2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
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
                //batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
                //split_batch_deg = batch_degree_branch.Split('@');
                //if ((split_batch_deg[1].ToString() != string.Empty) && (split_batch_deg[1].ToString() != null))
                //{
                //    FpSpread2.Sheets[0].Cells[4, 1].Text = split_batch_deg[1].ToString();//course and degree
                //}
                //if ((split_batch_deg[0].ToString() != string.Empty) && (split_batch_deg[0].ToString() != null))
                //{
                //    FpSpread2.Sheets[0].Cells[5, 5].Text = split_batch_deg[0].ToString();//batch year
                //    FpSpread2.Sheets[0].Cells[5, 5].Border.BorderColorRight = Color.Black;
                //}
                //if ((split_batch_deg[2].ToString() != string.Empty) && (split_batch_deg[2].ToString() != null))
                //{
                //    FpSpread2.Sheets[0].Cells[6, 0].Text = split_batch_deg[2].ToString();//semester and sections
                //}
                //if ((split_batch_deg[4].ToString() != string.Empty) && (split_batch_deg[4].ToString() != null))
                //{
                //    FpSpread2.Sheets[0].Cells[8, 0].Text = split_batch_deg[4].ToString();//test name and form name
                //    FpSpread2.Sheets[0].Cells[8, 0].HorizontalAlign = HorizontalAlign.Center;
                //}
                //if ((split_batch_deg[3].ToString() != string.Empty) && (split_batch_deg[3].ToString() != null))
                //{
                //    FpSpread2.Sheets[0].Cells[7, 0].Text = split_batch_deg[3].ToString();//date
                //    FpSpread2.Sheets[0].Cells[7, 0].Border.BorderColorRight = Color.Black;
                //}


                dtl.Rows[4][0] = degree + "[" + branch + "]";

                dtl.Rows[5][5] = batch;

                dtl.Rows[6][0] = "Semester: " + sem + "  -" + sec + " Sec";

                dtl.Rows[8][0] = test;
            }


            dtl.Rows[0][0] = collnamenew1;

            dtl.Rows[1][0] = address;

            dtl.Rows[2][0] = phnfax;

            dtl.Rows[3][0] = email;

            dtl.Rows[5][0] = string.Empty;

            dtl.Rows[6][5] = "Academic Year :" + academic;

            dtl.Rows[7][0] = form_heading_name;
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


            dtl.Rows[0][0] = collnamenew1;

            dtl.Rows[1][0] = address;

            dtl.Rows[2][0] = phnfax;

            dtl.Rows[3][0] = email;

            dtl.Rows[4][0] = degree + "[" + branch + "]";

            dtl.Rows[5][0] = string.Empty;

            dtl.Rows[5][5] = batch;

            dtl.Rows[6][0] = "Semester: " + sem + "  -" + sec + " Sec";

            dtl.Rows[6][5] = "Academic Year :" + academic;

            dtl.Rows[7][0] = test + "-" + "Result Analysis";

        }

        dtl.Rows[9][0] = "Subject Wise Percentage";

        dtl.Rows[9][5] = "Class Strength" + ":" + " " + Strength;



        //'-----------------------------------------------new mythili start----------header set-----------------


        tblstartrowvalue.Add(dtl.Rows.Count);

        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);

        dtl.Rows[dtl.Rows.Count - 1][0] = "Subject Code";

        dtl.Rows[dtl.Rows.Count - 1][1] = "Subject Name";

        dtl.Rows[dtl.Rows.Count - 1][2] = "Staff Name";

        dtl.Rows[dtl.Rows.Count - 1][3] = "Present";

        dtl.Rows[dtl.Rows.Count - 1][4] = "Absents";

        dtl.Rows[dtl.Rows.Count - 1][5] = "Passed";

        dtl.Rows[dtl.Rows.Count - 1][6] = "Failed";

        dtl.Rows[dtl.Rows.Count - 1][7] = "Pass%";


        //'--------------------------------------------------------------------------------------------------
        filteration();
        string section2 = string.Empty;
        string strsection2 = string.Empty;
        if (ddlSec.Enabled == true && ddlSec.Items.Count > 0)
        {
            section2 = ddlSec.SelectedValue.ToString();
            strsection2 = " and r.sections='" + ddlSec.SelectedValue.ToString() + "'";
        }


        string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.sections=Et.sections and r.roll_no=rt.roll_no " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  "+ strsection2  + strorder + ",s.subject_no";
        string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code  and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";
        hat.Clear();
        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
        hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
        hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
        hat.Add("sections", section2);
        hat.Add("filterwithsection", filterwithsection.ToString());
        hat.Add("filterwithoutsection", filterwithoutsection.ToString());
        ds2 = daccess2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
        string sections = string.Empty;
        string strsec = string.Empty;
        int total_pass_fail = 0;

        string qrySections = string.Empty;
        if (ddlSec.Items.Count > 0)
        {
            sections = ddlSec.SelectedValue.ToString();
            if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == "" || sections.ToString().Trim().ToLower() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and exam_type.sections='" + sections.ToString().Trim() + "'";
                qrySections = " and ss.Sections='" + Convert.ToString(sections).Trim() + "' ";
            }
        }
        string sec2 = string.Empty;
        if (ddlSec.Items.Count > 0)
        {
            if (ddlSec.Text.ToString().ToLower() == null || ddlSec.Text.ToString().Trim().ToLower() == "-1" || ddlSec.Text.ToString().Trim().ToLower() == "" || ddlSec.Text.ToString().Trim().ToLower() == "all")
            {
                sec2 = string.Empty;
            }
            else
            {
                sec2 = ddlSec.SelectedItem.Text.Trim();
            }
        }
        if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
            {
                bool isStudentStaffSelector = CheckStudentStaffSelector(Convert.ToString(ddlBatch.SelectedValue).Trim());
                hat.Clear();
                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                hat.Add("section", sec2);
                hat.Add("isPassOut", (chkincludepastout.Checked) ? "1" : "0");
                ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");
                FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                if (!isStudentStaffSelector)
                {

                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    dtl.Rows[dtl.Rows.Count - 1][0] = ds2.Tables[1].Rows[i]["subject_code"].ToString();

                    dtl.Rows[dtl.Rows.Count - 1][1] = ds2.Tables[1].Rows[i]["subject_name"].ToString();




                    string temp = string.Empty;
                    if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                    {
                        temp = ds2.Tables[1].Rows[i]["staff_code"].ToString();
                        staff = string.Empty;
                        //if (temp != "")
                        //{
                        //    staff = GetFunction("select nameacr from staff_appl_master where appl_no in(select distinct  appl_no from staffmaster where staff_code = '" + temp + "')");
                        //}

                        if (temp != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                            //staff = GetFunction("SELECT STUFF((select distinct ','+sfm.staff_name from staff_selector ss, staffmaster sfm,syllabus_master sm,subject s where sm.Batch_Year=ss.batch_year and ss.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and  sm.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and ss.staff_code =sfm.staff_code and s.subject_no='" + Convert.ToString(ds2.Tables[1].Rows[i]["subject_no"]).Trim() + "' FOR XML PATH('')),1,1,'')");

                        }

                        dtl.Rows[dtl.Rows.Count - 1][2] = staff;


                    }
                    //comment and added by anandan
                    //total_pass_fail = Convert.ToInt32(ds4.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds4.Tables[2].Rows[0]["FAIL_COUNT"]);

                    total_pass_fail = Convert.ToInt32(ds4.Tables[8].Rows[0]["PRESENT_COUNT"]);
                    double cal_avg = 0;
                    //modified by raghul on dec 12 2017

                    string valSum = Convert.ToString(ds4.Tables[0].Rows[0]["SUM"]);
                    if (!string.IsNullOrEmpty(valSum) && valSum != "")
                        cal_avg = Convert.ToDouble(valSum) / Convert.ToDouble(total_pass_fail);

                    cal_avg = Math.Round(cal_avg, 2);

                    double absent_count = 0;
                    if (chkIncludeAbsent.Checked)   //modified by prabha on feb 21 2018
                        double.TryParse(Convert.ToString(ds4.Tables[9].Rows[0]["ABSENT_COUNT"]), out absent_count);

                    double pass_perc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(total_pass_fail) + absent_count)) * 100;

                    pass_perc = Math.Round(pass_perc, 2);
                    dtl.Rows[dtl.Rows.Count - 1][3] = ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();

                    dtl.Rows[dtl.Rows.Count - 1][4] = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();

                    dtl.Rows[dtl.Rows.Count - 1][5] = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();

                    dtl.Rows[dtl.Rows.Count - 1][6] = ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();

                    dtl.Rows[dtl.Rows.Count - 1][7] = pass_perc.ToString();



                }
                else
                {
                    DataSet dsSubjectStaff = new DataSet();
                    DataSet dsPerformance = new DataSet();
                    hat.Clear();
                    hat.Add("examCode", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                    hat.Add("minMarks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                    dsPerformance = daccess2.select_method("usp_CAM_StudentsPerformance", hat, "sp");

                    string qrystss = "select distinct sfm.staff_name,sfm.staff_code from staff_selector ss, staffmaster sfm,syllabus_master sm,subject s where sm.Batch_Year=ss.batch_year and ss.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and  sm.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and ss.staff_code =sfm.staff_code and s.subject_no='" + Convert.ToString(ds2.Tables[1].Rows[i]["subject_no"]).Trim() + "'";

                    dsSubjectStaff = daccess2.select_method_wo_parameter("select distinct sfm.staff_name,sfm.staff_code from staff_selector ss, staffmaster sfm,syllabus_master sm,subject s where sm.Batch_Year=ss.batch_year and ss.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and  sm.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and ss.staff_code =sfm.staff_code and s.subject_no='" + Convert.ToString(ds2.Tables[1].Rows[i]["subject_no"]).Trim() + "'" + qrySections + "", "text");
                    if (dsSubjectStaff.Tables.Count > 0 && dsSubjectStaff.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drSubject in dsSubjectStaff.Tables[0].Rows)
                        {
                            string staffCode = Convert.ToString(drSubject["staff_code"]).Trim();
                            string staffName = Convert.ToString(drSubject["staff_name"]).Trim();
                            hat.Clear();
                            dsPerformance.Clear();
                            dsPerformance.Reset();
                            hat.Add("examCode", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                            hat.Add("minMarks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                            hat.Add("staffCode", staffCode);
                            hat.Add("isStudentStaffSelector", '1');
                            dsPerformance = daccess2.select_method("usp_CAM_StudentsPerformance", hat, "sp");


                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);

                            dtl.Rows[dtl.Rows.Count - 1][0] = ds2.Tables[1].Rows[i]["subject_code"].ToString();

                            dtl.Rows[dtl.Rows.Count - 1][1] = ds2.Tables[1].Rows[i]["subject_name"].ToString();





                            txt1 = new FarPoint.Web.Spread.TextCellType();


                            string temp = string.Empty;
                            if ((ds2.Tables[1].Rows[i]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                            {
                                temp = ds2.Tables[1].Rows[i]["staff_code"].ToString();
                                //staff = string.Empty;
                                //if (temp != "")//   if (staff == "" && (temp) != "") changed 21.02.12
                                //{
                                //    //staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                                //    staff = GetFunction("SELECT STUFF((select distinct ','+sfm.staff_name from staff_selector ss, staffmaster sfm,syllabus_master sm,subject s where sm.Batch_Year=ss.batch_year and ss.subject_no=s.subject_no and sm.syll_code=s.syll_code and sm.Batch_Year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and  sm.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and ss.staff_code =sfm.staff_code and s.subject_no='" + Convert.ToString(ds2.Tables[1].Rows[i]["subject_no"]).Trim() + "' FOR XML PATH('')),1,1,'')");

                                //}

                                dtl.Rows[dtl.Rows.Count - 1][2] = staffName;



                            }
                            //comment and added by anandan
                            //total_pass_fail = Convert.ToInt32(ds4.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds4.Tables[2].Rows[0]["FAIL_COUNT"]);
                            total_pass_fail = Convert.ToInt32(dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"]);
                            double pass_perc = 0;
                            if (total_pass_fail > 0)
                            {
                                double cal_avg = 0;
                                string SumVal = Convert.ToString(dsPerformance.Tables[0].Rows[0]["SUM"]);
                                if (!string.IsNullOrEmpty(SumVal))
                                    cal_avg = Convert.ToDouble(SumVal) / Convert.ToDouble(total_pass_fail);
                                cal_avg = Math.Round(cal_avg, 2);
                                pass_perc = (Convert.ToDouble(dsPerformance.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                                pass_perc = Math.Round(pass_perc, 2);
                            }

                            dtl.Rows[dtl.Rows.Count - 1][3] = dsPerformance.Tables[2].Rows[0]["PRESENT_COUNT"].ToString();

                            dtl.Rows[dtl.Rows.Count - 1][4] = dsPerformance.Tables[3].Rows[0]["ABSENT_COUNT"].ToString();

                            dtl.Rows[dtl.Rows.Count - 1][5] = dsPerformance.Tables[1].Rows[0]["PASS_COUNT"].ToString();

                            dtl.Rows[dtl.Rows.Count - 1][6] = dsPerformance.Tables[4].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();

                            dtl.Rows[dtl.Rows.Count - 1][7] = pass_perc.ToString();




                        }
                    }
                }
            }
        }

        //'-----------------------------------------------new end------------------------------------
        string dum_tage_date = string.Empty;
        string dum_tage_hrs = string.Empty;
        //'----------------------------------------new start-----------------------------------------------------
        int pass_count = 0;
        string sqlStr1 = string.Empty;
        int fail_sub_cnt = 0;
        string rolnosubno = string.Empty;
        Dictionary<string, bool> dicFailStudents = new Dictionary<string, bool>();
        string qrySec = string.Empty;
        string qrySec1 = string.Empty;
        if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == "" || sections.ToString().Trim().ToLower() == "-1")
        {
            strsec = string.Empty;
            qrySec = string.Empty;
            qrySec1 = string.Empty;
        }
        else
        {
            strsec = " and registration.sections='" + sections.ToString() + "'";
            qrySec = " and sections='" + sections.ToString() + "'";
            qrySec1 = " and e.sections='" + sections.ToString() + "'"; ;
        }
        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        {
            sqlStr1 = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 " + includePastout + " and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " " + strregorder + " "; //modified in 12.04.12
            con.Close();
            con.Open();
            //newly added
            //--
            if (sqlStr1 != "")
            {
                SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr1, con);
                adaSyll1.Fill(ds7);
                int subrow = 0;
                if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds7.Tables[0].Rows.Count; row++) //loop starting for student
                    {
                        fail_sub_cnt = 0;
                        rolnosubno = string.Empty;
                        DataView dv_indstudmarks = new DataView();
                        bool appeared = false;
                        if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)  //loop for iteration on subject
                            {
                                if (subrow < ds2.Tables[0].Rows.Count)
                                {
                                    ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds7.Tables[0].Rows[row]["roll"].ToString() + "' and subject_no='" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + "'";
                                    dv_indstudmarks = ds2.Tables[0].DefaultView;
                                    if (ds7.Tables[0].Rows[row]["roll"].ToString() == "15JEIT112")
                                    {
                                    }
                                    if (dv_indstudmarks.Count > 0)
                                    {
                                        appeared = true;
                                        for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
                                        {

                                            //if (Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-7" && Convert.ToString( dv_indstudmarks[cnt]["mark"]) != "-1" && Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-2" && Convert.ToString( dv_indstudmarks[cnt]["mark"]) != "-3" && (Convert.ToDouble(dv_indstudmarks[cnt]["mark"].ToString()) < Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                            double mark2 = 0;
                                            double minmk = 0;
                                            double.TryParse(Convert.ToString(dv_indstudmarks[cnt]["mark"]), out mark2);
                                            double.TryParse(Convert.ToString(ds2.Tables[1].Rows[j]["min_mark"]), out minmk);
                                            if (Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-7" && Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-1" && Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-2" && Convert.ToString(dv_indstudmarks[cnt]["mark"]) != "-3" && (mark2) < (minmk))
                                            {
                                                fail_sub_cnt++;
                                                if (rolnosubno == string.Empty)
                                                {
                                                    rolnosubno = ds7.Tables[0].Rows[row]["roll"].ToString() + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
                                                }
                                                else
                                                {
                                                    rolnosubno = rolnosubno + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
                                                }
                                            }
                                            subrow++;
                                        }
                                    }
                                }
                            }
                            subjectcount = ds2.Tables[1].Rows.Count;
                            for (int htr = 1; htr <= ds2.Tables[1].Rows.Count; htr++)
                            {
                                if (ht_fail_subject.Contains(Convert.ToString(htr)))
                                {
                                    string val = Convert.ToString(GetCorrespondingKey(htr, ht_fail_subject));
                                    if (fail_sub_cnt == htr)
                                    {
                                        string[] spl_val = val.Split('@');
                                        int value = Convert.ToInt32(spl_val[0].ToString());
                                        value++;
                                        string add_stud = spl_val[1].ToString() + ";" + rolnosubno;
                                        ht_fail_subject[Convert.ToString(htr)] = value + "@" + add_stud;
                                    }
                                }
                                else
                                {
                                    if (fail_sub_cnt == htr)
                                    {
                                        string concat = Convert.ToString(1) + "@" + rolnosubno;
                                        ht_fail_subject.Add(Convert.ToString(htr), concat);
                                    }
                                }
                            }

                            //'--------------to calculat the no.of stud passed in all subj--------------
                            if (fail_sub_cnt == 0 && appeared)
                            {
                                pass_count++;
                            }
                            if (fail_sub_cnt > 0 && appeared)
                            {
                                if (!dicFailStudents.ContainsKey(Convert.ToString(ds7.Tables[0].Rows[row]["roll"]).Trim().ToLower()))
                                {
                                    dicFailStudents.Add(Convert.ToString(ds7.Tables[0].Rows[row]["roll"]).Trim().ToLower(), true);
                                }
                            }
                        }
                    }
                    //}
                }
            }
            #region Commented part

            ////---
            //if (sqlStr1 != "")
            //{
            //    SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr1, con);
            //    adaSyll1.Fill(ds7);
            //    int subrow = 0;
            //    if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
            //    {
            //        for (int row = 0; row < ds7.Tables[0].Rows.Count; row++)
            //        {
            //            fail_sub_cnt = 0;
            //            rolnosubno =string.Empty;
            //            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
            //            {
            //                if (subrow < ds2.Tables[0].Rows.Count)
            //                {
            //                    if (ds7.Tables[0].Rows[row]["roll"].ToString() == ds2.Tables[0].Rows[subrow]["roll"].ToString())
            //                    {
            //                        if (ds2.Tables[0].Rows[subrow]["mark"].ToString() != "-2" && ds2.Tables[0].Rows[subrow]["mark"].ToString() != "-3" && (Convert.ToDouble(ds2.Tables[0].Rows[subrow]["mark"].ToString()) < Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
            //                        {
            //                            fail_sub_cnt++;
            //                            if (rolnosubno == string.Empty)
            //                            {
            //                                rolnosubno = ds7.Tables[0].Rows[row]["roll"].ToString() + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
            //                            }
            //                            else
            //                            {
            //                                rolnosubno = rolnosubno + "," + ds2.Tables[1].Rows[j]["subject_no"].ToString();
            //                            }
            //                        }
            //                    }
            //                    subrow++;
            //                }
            //            }
            //            //added by gowtham
            //            //------------
            //            DataView dv_indstudmarks = new DataView();
            //            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
            //            {
            //                if (subrow < ds2.Tables[0].Rows.Count)
            //                {
            //                    ds2.Tables[0].DefaultView.RowFilter = "roll='" + ds7.Tables[0].Rows[row]["roll"].ToString() + "'";
            //                    dv_indstudmarks = ds2.Tables[0].DefaultView;
            //                    if (dv_indstudmarks.Count > 0)
            //                    {
            //                        for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
            //                        {
            //                            if (dv_indstudmarks[cnt]["mark"].ToString() != "-2" && dv_indstudmarks[cnt]["mark"].ToString() != "-3" && (Convert.ToDouble(dv_indstudmarks[cnt]["mark"].ToString()) < Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
            //                            {
            //                                fail_sub_cnt++;
            //                                if (rolnosubno == string.Empty)
            //                                {
            //                                    rolnosubno = ds7.Tables[0].Rows[row]["roll"].ToString() + "," + dv_indstudmarks[cnt]["subject_no"].ToString();
            //                                }
            //                                else
            //                                {
            //                                    rolnosubno = rolnosubno + "," + dv_indstudmarks[cnt]["subject_no"].ToString();
            //                                }
            //                            }
            //                            subrow++;
            //                        }
            //                        subjectcount = ds2.Tables[1].Rows.Count;
            //                        for (int htr = 1; htr <= ds2.Tables[1].Rows.Count; htr++)
            //                        {
            //                            if (ht_fail_subject.Contains(Convert.ToString(htr)))
            //                            {
            //                                string val = Convert.ToString(GetCorrespondingKey(htr, ht_fail_subject));
            //                                if (fail_sub_cnt == htr)
            //                                {
            //                                    string[] spl_val = val.Split('-');
            //                                    int value = Convert.ToInt32(spl_val[0].ToString());
            //                                    value++;
            //                                    string add_stud = spl_val[1].ToString() + ";" + rolnosubno;
            //                                    ht_fail_subject[Convert.ToString(htr)] = value + "-" + add_stud;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                if (fail_sub_cnt == htr)
            //                                {
            //                                    string concat = Convert.ToString(1) + "-" + rolnosubno;
            //                                    ht_fail_subject.Add(Convert.ToString(htr), concat);
            //                                }
            //                            }
            //                        }
            //                        //'--------------to calculat the no.of stud passed in all subj--------------
            //                        if (fail_sub_cnt == 0)
            //                        {
            //                            pass_count++;
            //                        }
            //                    }
            //                }
            //            }
            // --------------------------
            //        subjectcount = ds2.Tables[1].Rows.Count;
            //        for (int htr = 1; htr <= ds2.Tables[1].Rows.Count; htr++)
            //        {
            //            if (ht_fail_subject.Contains(Convert.ToString(htr)))
            //            {
            //                string val = Convert.ToString(GetCorrespondingKey(htr, ht_fail_subject));
            //                if (fail_sub_cnt == htr)
            //                {
            //                    string[] spl_val = val.Split('-');
            //                    int value = Convert.ToInt32(spl_val[0].ToString());
            //                    value++;
            //                    string add_stud = spl_val[1].ToString() + ";" + rolnosubno;
            //                    ht_fail_subject[Convert.ToString(htr)] = value + "-" + add_stud;
            //                }
            //            }
            //            else
            //            {
            //                if (fail_sub_cnt == htr)
            //                {
            //                    string concat = Convert.ToString(1) + "-" + rolnosubno;
            //                    ht_fail_subject.Add(Convert.ToString(htr), concat);
            //                }
            //            }
            //        }
            //        //'--------------to calculat the no.of stud passed in all subj--------------
            //        if (fail_sub_cnt == 0)
            //        {
            //            pass_count++;
            //        }
            //    }
            //}
            //} 

            #endregion

            string sec_examcode = "select distinct e.exam_code as exam_code from exam_type e,subject s where e.subject_no=s.subject_no and e.criteria_no=" + ddlTest.SelectedValue.ToString() + "  " + qrySec + "  ";
            DataSet dsExamCode = d2.select_method_wo_parameter(sec_examcode, "text");
            string qryExamCode = string.Empty;
            if (dsExamCode.Tables.Count > 0 && dsExamCode.Tables[0].Rows.Count > 0)
            {
                for (int scexm = 0; scexm < dsExamCode.Tables[0].Rows.Count; scexm++)
                {
                    if (qryExamCode == "")
                    {
                        qryExamCode = "'" + Convert.ToString(dsExamCode.Tables[0].Rows[scexm]["exam_code"]).Trim() + "'";
                    }
                    else
                    {
                        qryExamCode += ",'" + Convert.ToString(dsExamCode.Tables[0].Rows[scexm]["exam_code"]).Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(qryExamCode.Trim()))
            {
                qryExamCode = " in (" + qryExamCode + ") ";
            }
            double test_minmark = 0;
            string minmrk = d2.GetFunction("select min_mark from criteriaforinternal where criteria_no=" + ddlTest.SelectedValue.ToString() + "");
            double.TryParse(minmrk, out test_minmark);

            int totalPassed = 0;
            string totalpassedQry = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt,exam_type e where r.exam_code " + qryExamCode.ToString() + "  and (marks_obtained>=e.min_mark or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-7')  and r.roll_no=rt.roll_no and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + qrySec1 + " and e.exam_code=r.exam_code  "; //or marks_obtained='-1'
            string totalPassedStudents = daccess2.GetFunction(totalpassedQry);
            int.TryParse(totalPassedStudents.Trim(), out totalPassed);

            int totalAppeared = 0;
            string appearedCount = daccess2.GetFunction("select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + qryExamCode.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-7')  and r.roll_no=rt.roll_no and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + qrySec + " ");
            int.TryParse(appearedCount.Trim(), out totalAppeared);



            string qrysql = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r,exam_type e where rt.exam_Code " + qryExamCode.ToString() + " and rt.roll_no=r.roll_no and r.degree_code=" + ddlBranch.SelectedValue.ToString() + " and r.batch_year=" + ddlBatch.SelectedItem.ToString() + "  " + qrySec1 + " and (rt.marks_obtained < e.min_mark and rt.marks_obtained<>'-3' and rt.marks_obtained<>-1  and rt.marks_obtained<>'-7' and rt.marks_obtained<>'-2' and rt.marks_obtained<>'-18') and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0  and e.exam_code=rt.exam_code   ";
            ds = d2.select_method_wo_parameter(qrysql, "Text");

            int fail_in_allsubj = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                fail_in_allsubj = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            int totalStudents = 0;
            int totalFailed = 0;
            int passedCount = 0;
            if (totalPassed == 0)
                passedCount = 0;
            else
                passedCount = Convert.ToInt32(appearedCount) - fail_in_allsubj;
                
                //passedCount = totalPassed - fail_in_allsubj;
                //passedCount = Convert.ToInt32(appearedCount) - fail_in_allsubj;

            double passPercentage = 0;

            if (totalAppeared > 0)
            {
                passPercentage = Math.Round((Convert.ToDouble(passedCount) / Convert.ToDouble(totalAppeared)) * 100, 2, MidpointRounding.AwayFromZero);
            }


            string secf = string.Empty;
            if (ddlSec.Text.ToString().Trim() == "-1" || ddlSec.Text.ToString().Trim() == "" || ddlSec.Text.ToString().Trim() == null || ddlSec.Text.ToString().Trim().ToLower() == "all")
            {
                secf = string.Empty;
            }
            else
            {
                secf = ddlSec.SelectedItem.Text;
            }
            int total_pass_count = 0;
            if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
            {
                for (int passtot = 0; passtot < ds2.Tables[1].Rows.Count; passtot++)
                {
                    hat.Clear();
                    hat.Add("exam_code", ds2.Tables[1].Rows[passtot]["exam_code"].ToString());
                    hat.Add("min_marks", ds2.Tables[1].Rows[passtot]["min_mark"].ToString());
                    hat.Add("section", secf);
                    ds8 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");
                    total_pass_count = total_pass_count + Convert.ToInt32(ds8.Tables[1].Rows[0]["PASS_COUNT"].ToString());
                }
            }
            //   double pass_percentage = (Convert.ToDouble(pass_count) / Convert.ToDouble(total_pass_count)) * 100;



            double pass_percentage = (Convert.ToDouble(pass_count) / Convert.ToDouble(Strength)) * 100;
            pass_percentage = Math.Round(pass_percentage, 2);

            if (ds2.Tables.Count > 1 && ds2.Tables[1].Rows.Count > 0)
            {
                for (int sb = 1; sb <= ds2.Tables[1].Rows.Count; sb++)
                {
                }
            }



            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);



            tblstartrowvalue.Add(dtl.Rows.Count);
            foreach (DictionaryEntry parameter in ht_fail_subject)
            {
                string htkey = Convert.ToString(parameter.Key);
                string htvalu = Convert.ToString(parameter.Value);
                string[] spl_htval = htvalu.Split('@');


                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtl.Rows[dtl.Rows.Count - 1][0] = "Failure in" + " " + (htkey) + " " + "subjects";

                dtl.Rows[dtl.Rows.Count - 1][2] = spl_htval[0].ToString() + "";


                //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.Black;

            }


            for (int ii = 0; ii < 10; ii++)
            {
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

            }



            dtl.Rows[dtl.Rows.Count - 3][0] = "Class Advisor";

            dtl.Rows[dtl.Rows.Count - 3][7] = " HOD  ";





            dtl.Rows[dtl.Rows.Count - 8][0] = "No of Students Passed in all Subject =" + passedCount;

            dtl.Rows[dtl.Rows.Count - 7][0] = "Percentage of Students Passed in all Subject =" + passPercentage.ToString();




            for (int ii = 0; ii < 5; ii++)
            {
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

            }
            tblstartrowvalue.Add(dtl.Rows.Count);
            foreach (DictionaryEntry param in ht_fail_subject)//'-----------loop for displaying the stud name-------------
            {
                string htkey1 = Convert.ToString(param.Key);
                string htval = Convert.ToString(param.Value);


                dtl.Rows[dtl.Rows.Count - 1][0] = "Failure in" + " " + htkey1 + " " + "subjects";


                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);



                dtl.Rows[dtl.Rows.Count - 1][0] = "S.No";

                dtl.Rows[dtl.Rows.Count - 1][1] = "Roll No";

                dtl.Rows[dtl.Rows.Count - 1][2] = "Name";

                dtl.Rows[dtl.Rows.Count - 1][3] = "Subjects";

                string[] spl_htval = htval.Split('@');
                string[] spl_count = spl_htval[1].Split(';');
                int serial = 0;
                string tempsub = string.Empty;
                for (int printrw = 0; printrw < Convert.ToInt32(spl_htval[0].ToString()); printrw++)
                {
                    serial++;
                    tempsub = string.Empty;
                    string[] spl_stud = spl_count[printrw].Split(',');


                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);



                    dtl.Rows[dtl.Rows.Count - 1][0] = serial + "";

                    dtl.Rows[dtl.Rows.Count - 1][1] = spl_stud[0].ToString();




                    string studname = GetFunction("select stud_name from registration where Roll_No='" + spl_stud[0].ToString() + "'");
                    dtl.Rows[dtl.Rows.Count - 1][2] = studname;


                    for (int subcnt = 1; subcnt <= spl_stud.GetUpperBound(0); subcnt++)
                    {
                        if (tempsub == "")
                        {
                            tempsub = spl_stud[subcnt].ToString();
                        }
                        else
                        {
                            tempsub = tempsub + "," + spl_stud[subcnt].ToString();
                        }
                    }
                    if (tempsub != "")
                    {
                        tempsub = "in(" + tempsub + ")";
                    }
                    string subname = "select subject_name from subject where subject_no " + tempsub + "";
                    SqlDataAdapter da_subnam = new SqlDataAdapter(subname, con);
                    con.Close();
                    con.Open();
                    DataSet ds9 = new DataSet();
                    da_subnam.Fill(ds9);
                    string displayname = string.Empty;
                    if (ds9.Tables.Count > 0 && ds9.Tables[0].Rows.Count > 0)
                    {
                        for (int subnamerw = 0; subnamerw < ds9.Tables[0].Rows.Count; subnamerw++)
                        {
                            if (displayname == "")
                            {
                                displayname = ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                            }
                            else
                            {
                                displayname = displayname + "," + ds9.Tables[0].Rows[subnamerw]["subject_name"].ToString();
                            }
                        }
                    }

                    dtl.Rows[dtl.Rows.Count - 1][3] = displayname.ToString();


                }

                for (int ii = 0; ii < 5; ii++)
                {
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                }
                tblstartrowvalue.Add(dtl.Rows.Count);
            }
            //  '--------------------------attendance percentage---------------------------------------
            hat.Clear();
            hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
            hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                eligiblepercent = int.Parse(ds.Tables[0].Rows[0]["Eligible_Percent"].ToString());
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                countds = ds1.Tables[0].Rows.Count;
            }
            //'----------------------------------------new start---------------------------------------------
            //int student = 0;
            if (ds7.Tables.Count > 0 && ds7.Tables[0].Rows.Count > 0)
            {
                for (int att = 0; att < ds7.Tables[0].Rows.Count; att++)
                {
                    persentmonthcal();
                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }
                    //modified By Srinath 23/2/2013 
                    //per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
                    per_con_hrs = per_workingdays1 + spl_tot_condut;
                    per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);
                    if (per_tage_hrs > 100)
                    {
                        per_tage_hrs = 100;
                    }
                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                    dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
                    if (dum_tage_hrs == "NaN")
                    {
                        dum_tage_hrs = "0";
                    }
                    else if (dum_tage_hrs == "Infinity")
                    {
                        dum_tage_hrs = "0";
                    }
                    if (dum_tage_date == "NaN")
                    {
                        dum_tage_date = "0";
                    }
                    else if (dum_tage_date == "Infinity")
                    {
                        dum_tage_date = "0";
                    }
                    //'------------------------------------------------new end------------
                    //'----------------------adding the percentage below 80 % to hash table-----------------------------
                    if (htattperc.Contains(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower()))
                    {
                        int value1 = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rol_no).Trim().ToLower(), htattperc));
                        value1++;//fail count
                        htattperc[Convert.ToString(rol_no).Trim().ToLower()] = value1;
                    }
                    else
                    {
                        if (rdattnd_daywise.Checked == true)
                        {
                            if (Convert.ToDouble(dum_tage_date) < Convert.ToDouble(eligiblepercent))
                            {
                                htattperc.Add(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower(), dum_tage_date.ToString());
                            }
                        }
                        else if (rdattnd_hourwise.Checked == true)
                        {
                            if (Convert.ToDouble(dum_tage_hrs) < Convert.ToDouble(eligiblepercent))
                            {
                                htattperc.Add(Convert.ToString(ds7.Tables[0].Rows[student]["roll"]).Trim().ToLower(), dum_tage_hrs.ToString());
                            }
                        }
                    }
                    student++;
                }
                //'-----------------------------------------new end---------------------------------------------------------

                //'----------------------------------------display attendance % from the hashtable-------------------------
                int serialno = 0;
                //foreach (DictionaryEntry parameter in htattperc)
                //{
                for (int order = 0; order < ds7.Tables[0].Rows.Count; order++)
                {
                    string roll = ds7.Tables[0].Rows[order]["roll"].ToString().Trim();
                    if (htattperc.ContainsKey(roll.Trim().ToLower()))
                    {
                        serialno++;
                        //string key1 = parameter.Key.ToString();
                        //string value1 = parameter.Value.ToString();
                        //string amount = (GetCorrespondingKey(d3.Tables[0].Rows[0]["header_id"].ToString(), fee_has).ToString());
                        string key1 = roll.ToString();
                        string value1 = (GetCorrespondingKey(key1.Trim().ToLower(), htattperc).ToString());
                        string studname = GetFunction("select stud_name from registration where roll_no='" + key1.ToString() + "'");
                        if (serialno == 1)
                        {
                            //  FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                            //   FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Border.BorderColorRight = Color.Black;


                            dtl.Rows[dtl.Rows.Count - 1][0] = "Attendance Below " + eligiblepercent + "%";

                            tblstartrowvalue.Add(dtl.Rows.Count);

                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);


                            dtl.Rows[dtl.Rows.Count - 1][0] = "S.No";

                            dtl.Rows[dtl.Rows.Count - 1][1] = "RollNo";

                            dtl.Rows[dtl.Rows.Count - 1][2] = "Name";

                            dtl.Rows[dtl.Rows.Count - 1][3] = "Attendance%";



                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);

                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Border.BorderColorRight = Color.Black;
                        }
                        //'------------------display the stud att percent-----------------------------


                        dtl.Rows[dtl.Rows.Count - 1][0] = serialno.ToString();

                        dtl.Rows[dtl.Rows.Count - 1][1] = key1.ToString();

                        dtl.Rows[dtl.Rows.Count - 1][2] = studname.ToString();

                        dtl.Rows[dtl.Rows.Count - 1][3] = value1.ToString();



                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                    }
                }

                // }

                for (int ii = 0; ii < 6; ii++)
                {
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                }
                tblstartrowvalue.Add(dtl.Rows.Count);


                dtl.Rows[dtl.Rows.Count - 6][0] = "No.of.Students:";

                dtl.Rows[dtl.Rows.Count - 6][1] = ds7.Tables[0].Rows.Count.ToString();


            }
            int rowcount1 = dtl.Rows.Count;





            if (dtl.Rows.Count > 0)
            {
                Showgrid.DataSource = dtl;
                Showgrid.DataBind();
                divgrid.Visible = true;
                Showgrid.HeaderRow.Visible = false;

                for (int i = 0; i < Showgrid.Rows.Count; i++)
                {
                    Showgrid.Rows[i].Cells[0].Width = 100;
                    Showgrid.Rows[i].Cells[1].Width = 135;
                    Showgrid.Rows[i].Cells[2].Width = 175;
                    Showgrid.Rows[i].Cells[3].Width = 60;
                    Showgrid.Rows[i].Cells[4].Width = 80;
                    Showgrid.Rows[i].Cells[5].Width = 80;
                    Showgrid.Rows[i].Cells[6].Width = 80;
                    Showgrid.Rows[i].Cells[7].Width = 100;
                    for (int j = 0; j < dtl.Columns.Count; j++)
                    {
                        if (i <= Convert.ToInt32(tblstartrowvalue[0]))
                        {
                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[i].Cells[j].Font.Bold = true;

                            if ((j == 0 || j == 5) && i < Convert.ToInt32(tblstartrowvalue[0]))
                            {
                                if (i == Convert.ToInt32(tblstartrowvalue[0]) - 1)
                                    Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                else
                                    Showgrid.Rows[i].Cells[j].BorderColor = Color.White;



                                if (i != 5 && i != 6 && i != 9 && j == 0)
                                {

                                    Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.HeaderRow.Cells.Count;
                                    for (int a = 1; a < Showgrid.HeaderRow.Cells.Count; a++)
                                        Showgrid.Rows[i].Cells[j + a].Visible = false;
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = 5;
                                        for (int a = 1; a < 5; a++)
                                            Showgrid.Rows[i].Cells[j + a].Visible = false;
                                    }
                                    else
                                    {
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = 3;
                                        for (int a = 1; a < 3; a++)
                                            Showgrid.Rows[i].Cells[j + a].Visible = false;
                                    }
                                }
                            }

                        }
                        else if (i > Convert.ToInt32(tblstartrowvalue[0]) && i < Convert.ToInt32(tblstartrowvalue[1]))
                        {
                            if (j != 0 && j != 1 && j != 2)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (i == Convert.ToInt32(tblstartrowvalue[1]) - 2 || i == Convert.ToInt32(tblstartrowvalue[1]) - 1)
                            {
                                if (j == 0)
                                {

                                    if (i == Convert.ToInt32(tblstartrowvalue[1]) - 2)
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                    Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.HeaderRow.Cells.Count;
                                    for (int a = 1; a < Showgrid.HeaderRow.Cells.Count; a++)
                                        Showgrid.Rows[i].Cells[j + a].Visible = false;
                                }
                            }
                        }
                        else if (i >= Convert.ToInt32(tblstartrowvalue[1]) && i <= Convert.ToInt32(tblstartrowvalue[2]))
                        {
                            if (i <= Convert.ToInt32(tblstartrowvalue[2]) - 16)
                            {
                                if (j == 0)
                                {
                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 2;
                                    for (int a = 1; a < 2; a++)
                                        Showgrid.Rows[i].Cells[j + a].Visible = false;
                                }
                                else if (j == 2)
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else if (j == 3)
                                {
                                    Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 5;
                                    for (int a = 1; a < 5; a++)
                                        Showgrid.Rows[i].Cells[j + a].Visible = false;

                                }
                            }
                            else if (i > Convert.ToInt32(tblstartrowvalue[2]) - 16)
                            {
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                if (i != Convert.ToInt32(tblstartrowvalue[2]) - 8)
                                {
                                    if (i != Convert.ToInt32(tblstartrowvalue[2]))
                                    {
                                        if (j == 0)
                                        {
                                            Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                            if (i != Convert.ToInt32(tblstartrowvalue[2]) - 1)
                                                Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                            Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.HeaderRow.Cells.Count;
                                            for (int a = 1; a < Showgrid.HeaderRow.Cells.Count; a++)
                                                Showgrid.Rows[i].Cells[j + a].Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        if (j == 3)
                                        {
                                            Showgrid.Rows[i].Cells[j].ColumnSpan = 5;
                                            for (int a = 1; a < 5; a++)
                                                Showgrid.Rows[i].Cells[j + a].Visible = false;
                                        }
                                    }
                                }
                                else
                                {


                                    if (j == Showgrid.HeaderRow.Cells.Count - 1)
                                    {

                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                    }
                                    else if (j == 0)
                                    {
                                        Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.HeaderRow.Cells.Count - 1;
                                        for (int a = 1; a < Showgrid.HeaderRow.Cells.Count - 1; a++)
                                            Showgrid.Rows[i].Cells[j + a].Visible = false;
                                    }

                                }
                            }



                        }
                        else
                        {
                            for (int ii = 3; ii < tblstartrowvalue.Count; ii++)
                            {
                                if (ii != tblstartrowvalue.Count - 1)
                                {
                                    if (i <= Convert.ToInt32(tblstartrowvalue[ii]))
                                    {
                                        if (i < Convert.ToInt32(tblstartrowvalue[ii]) - 5)
                                        {
                                            if (j == 0 || j == 1)
                                            {
                                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                                break;
                                            }
                                            else if (j == 2)
                                            {
                                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                                break;
                                            }
                                            else if (j == 3)
                                            {
                                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                                Showgrid.Rows[i].Cells[j].ColumnSpan = 5;
                                                for (int a = 1; a < 5; a++)
                                                    Showgrid.Rows[i].Cells[j + a].Visible = false;

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (i == Convert.ToInt32(tblstartrowvalue[ii]))
                                            {
                                                if (j == 0 || j == 1 || j == 2)
                                                {
                                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                                    Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                                    break;
                                                }
                                                else if (j == 3)
                                                {
                                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                                    Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 5;
                                                    for (int a = 1; a < 5; a++)
                                                        Showgrid.Rows[i].Cells[j + a].Visible = false;

                                                    break;
                                                }

                                            }
                                            else
                                            {
                                                if (j == 0)
                                                {
                                                    Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                                    if (i != Convert.ToInt32(tblstartrowvalue[ii]) - 1)
                                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                                    Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.HeaderRow.Cells.Count;
                                                    for (int a = 1; a < Showgrid.HeaderRow.Cells.Count; a++)
                                                        Showgrid.Rows[i].Cells[j + a].Visible = false;

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    if (i > Convert.ToInt32(tblstartrowvalue[ii - 1]))
                                    {
                                        if (j == 0)
                                            Showgrid.Rows[i - 2].Cells[j].BorderColor = Color.White;
                                        Showgrid.Rows[i - 1].Cells[j].BorderColor = Color.White;
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.White;
                                    }
                                }
                            }
                        }

                    }
                }


            }

            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "1")//for first page
                {
                    ddlpage.Items.Clear();
                    int totrowcount = dtl.Rows.Count;
                    int pages = totrowcount / 45;
                    int intialrow = 1;
                    int remainrows = totrowcount % 45;
                    if (dtl.Rows.Count > 0)
                    {
                        int i5 = 0;
                        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                        for (int i = 1; i <= pages; i++)
                        {
                            i5 = i;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                            intialrow = intialrow + 45;
                        }
                        if (remainrows > 0)
                        {
                            i = i5 + 1;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        }
                    }
                }
                else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "0")//for all page
                {
                    ddlpage.Items.Clear();
                    int totrowcount = dtl.Rows.Count;
                    int pages = totrowcount / 35;
                    int intialrow = 1;
                    int remainrows = totrowcount % 35;
                    if (dtl.Rows.Count > 0)
                    {
                        int i5 = 0;
                        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                        for (int i = 1; i <= pages; i++)
                        {
                            i5 = i;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                            intialrow = intialrow + 35;
                        }
                        if (remainrows > 0)
                        {
                            i = i5 + 1;
                            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        }
                    }
                }
                else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "2")
                {
                    //for (int h = 0; h < FpSpread2.Sheets[0].ColumnHeader.RowCount; h++)
                    //{
                    //    FpSpread2.Sheets[0].ColumnHeader.Rows[h].Visible = false;
                    //}
                }
            }//end codn for dsprint rowcount
        } // cond end for ds2 row count
        else
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Test has not been conducted for any subject";
            divgrid.Visible = false;
            //RadioHeader.Visible = false;
            //Radiowithoutheader.Visible = false;
            ddlpage.Visible = false;
            lblpages.Visible = false;
            btnExcel.Visible = false;
            btnprint_Pdf.Visible = false;
            norecordlbl.Visible = false;
            txtexcelname.Visible = false;
        }
        //'---------------------------------------------------------end---
        //}
        //catch
        //{
        //}
    }

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        //if (SelectAll.Checked == true)
        //{
        //    foreach (ListItem li in ddlreport.Items)
        //    {
        //        li.Selected = true;
        //        TextBox1.Text = "criteria(" + (ddlreport.Items.Count) + ")";
        //    }
        //}
        //else
        //{
        //    foreach (ListItem li in ddlreport.Items)
        //    {
        //        li.Selected = false;
        //        TextBox1.Text = "--Select--";
        //    }
        //}
    }

    protected void ddlto_SelectedIndexChanged(object sender, EventArgs e)
    {
        //for (int i = 0; i < FpSpread2.Sheets[0].RowCount;i++)
        //{
        //    FpSpread2.Sheets[0].Rows[ i].Visible = false;
        //}
        //string start = ddlfrom.SelectedValue.ToString();
        //string end = ddlto.SelectedValue.ToString();
        //int rowstart = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(start);
        //int rowend = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(end);
        //for (int i = (Convert.ToInt32(start)-1); i < Convert.ToInt32(end); i++)
        //{
        //    int regularrowstart = FpSpread2.Sheets[0].RowCount;
        //    regularrowstart = regularrowstart - i;
        //    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - regularrowstart].Visible = true;
        //}
    }

    protected void ddlfrom_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                norecordlbl.Visible = false;

                d2.printexcelreportgrid(Showgrid, reportname);
                txtexcelname.Text = string.Empty;
            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hat.Clear();
        //hat.Add("college_code", Session["collegecode"].ToString());
        //hat.Add("form_name", "Resultanalysis.aspx");
        //dsprint = daccess2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        //if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "1")//for first page
        //{
        //    FpSpread2.SheetCorner.RowCount = 0;
        //    //if (Radiowithoutheader.Checked == true)
        //    //{
        //    FpSpread2.ColumnHeader.Visible = false;
        //    for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread2.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 44;
        //    int rowstart = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(end);
        //    int flag2 = 0;
        //    int flag = 0;
        //    int flagattend = 0;
        //    //if ((ddlpage.SelectedValue.ToString() == string.Empty) && (ddlpage.SelectedValue.ToString() == "0"))
        //    //{
        //    // }
        //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedIndex.ToString() == "0"))
        //    {
        //        for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        //        {
        //            FpSpread2.Sheets[0].Rows[i].Visible = true;
        //        }
        //        Double totalRows = 0;
        //        totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
        //        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
        //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //        DropDownListpage.Items.Clear();
        //        if (totalRows >= 10)
        //        {
        //            FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //            {
        //                DropDownListpage.Items.Add((k + 10).ToString());
        //            }
        //            DropDownListpage.Items.Add("Others");
        //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //            FpSpread2.Height = 335;
        //        }
        //        else if (totalRows == 0)
        //        {
        //            DropDownListpage.Items.Add("0");
        //            FpSpread2.Height = 100;
        //        }
        //        else
        //        {
        //            FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //            DropDownListpage.Items.Add(FpSpread2.Sheets[0].PageSize.ToString());
        //            FpSpread2.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //        }
        //        if (Convert.ToInt32(FpSpread2.Sheets[0].RowCount) > 10)
        //        {
        //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //            FpSpread2.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //            //  FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //            CalculateTotalPages();
        //        }
        //        Buttontotal.Visible = true;
        //        lblrecord.Visible = true;
        //        DropDownListpage.Visible = true;
        //        TextBoxother.Visible = false;
        //        lblpage.Visible = true;
        //        TextBoxpage.Visible = true;
        //    }
        //    else
        //    {
        //        for (int i = (Convert.ToInt32(start) - 1); i < Convert.ToInt32(end); i++)
        //        {
        //            int regularrowstart = FpSpread2.Sheets[0].RowCount;
        //            if (i < regularrowstart)
        //            {
        //                regularrowstart = regularrowstart - i;
        //                if (flag2 == 0)
        //                {
        //                    flag2 = 1;
        //                    string gettext = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 0].Text;
        //                    int newgettext = 0;
        //                    if (int.TryParse(gettext, out newgettext))
        //                    {
        //                        flag = 1;
        //                        newgettext = Convert.ToInt32(gettext);
        //                        string gettext1 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 3].Text;
        //                        int newgettext1 = 0;
        //                        if (int.TryParse(gettext1, out newgettext1))
        //                        {
        //                            flagattend = 1;
        //                            newgettext1 = Convert.ToInt32(gettext1);
        //                        }
        //                    }
        //                    if (flag == 1)
        //                    {
        //                        if (flagattend != 1)
        //                        {
        //                            //FpSpread2.Sheets[0].
        //                            FpSpread2.ColumnHeader.Visible = true;
        //                            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        //                            style.Font.Size = 10;
        //                            style.Font.Bold = true;
        //                            FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //                            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //                            FpSpread2.SheetCorner.RowCount = 1;
        //                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 5);
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subjects";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        //                        }
        //                        else if (flagattend == 1)
        //                        {
        //                            FpSpread2.ColumnHeader.Visible = true;
        //                            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        //                            style.Font.Size = 10;
        //                            style.Font.Bold = true;
        //                            FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //                            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //                            FpSpread2.SheetCorner.RowCount = 1;
        //                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 2);
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Attendance%";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorTop = Color.White;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColorTop = Color.White;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColorTop = Color.White;
        //                            FpSpread2.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = " ";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = " ";
        //                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = " ";
        //                        }
        //                    }
        //                }
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - regularrowstart].Visible = true;
        //            }
        //            else
        //            {
        //                FpSpread2.Sheets[0].RowCount++;
        //            }
        //            //'----------------------------for footer display start
        //            if (dsprint.Tables[0].Rows[0]["footer_name"].ToString() != string.Empty)
        //            {
        //                if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0") //all pages footer
        //                {
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //                }
        //                else //last page footer
        //                {
        //                    if (ddlpage.SelectedIndex == (ddlpage.Items.Count - 1))
        //                    {
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //                    }
        //                    else
        //                    {
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = false;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = false;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
        //                    }
        //                }
        //            }
        //            //'-------------------------end for footer display
        //        }
        //    }
        //}//end condn for last page
        //else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "0")//for all page
        //{
        //    FpSpread2.SheetCorner.RowCount = 0;
        //    //radiowith header
        //    FpSpread2.ColumnHeader.Visible = false;
        //    for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread2.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + 10;
        //    int end = start + 34;
        //    int rowstart = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(end);
        //    //=================column header
        //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        //    {
        //        string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno from collinfo where college_code=" + Session["collegecode"] + "";
        //        SqlCommand collegecmd = new SqlCommand(college, con);
        //        SqlDataReader collegename;
        //        con.Close();
        //        con.Open();
        //        collegename = collegecmd.ExecuteReader();
        //        if (collegename.HasRows)
        //        {
        //            while (collegename.Read())
        //            {
        //                collnamenew1 = collegename["collname"].ToString();
        //                address1 = collegename["address1"].ToString();
        //                address2 = collegename["address2"].ToString();
        //                address = address1 + "-" + address2;
        //                Phoneno = collegename["phoneno"].ToString();
        //                Faxno = collegename["faxno"].ToString();
        //                phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
        //            }
        //        }
        //        con.Close();
        //    }
        //    MyImg collogo = new MyImg();
        //    collogo.ImageUrl = "Handler/Handler2.ashx?";
        //    MyImg collogoright = new MyImg();
        //    collogoright.ImageUrl = "../images/10BIT001.jpeg";
        //    collogoright.ImageUrl = "Handler/Handler5.ashx?";
        //    string branch = ddlBranch.SelectedItem.Text;
        //    string degree = ddlDegree.SelectedItem.Text;
        //    string sem = ddlSemYr.SelectedValue;
        //    string sec = ddlSec.SelectedValue;
        //    string test = ddlTest.SelectedItem.Text;
        //    //-----------
        //    DateTime currentdate = System.DateTime.Now;
        //    string fromdate = currentdate.ToString("yyyy");
        //    string sem1 = string.Empty;
        //    string semester1 = "select duration from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
        //    SqlCommand semcmd = new SqlCommand(semester1, con);
        //    SqlDataReader semreader;
        //    con.Close();
        //    con.Open();
        //    semreader = semcmd.ExecuteReader();
        //    if (semreader.HasRows)
        //    {
        //        while (semreader.Read())
        //        {
        //            sem1 = semreader["duration"].ToString();
        //        }
        //    }
        //    string strsec1 = string.Empty;
        //    if (sec.ToString() == "All" || sec.ToString() == "" || sec.ToString() == "-1")
        //    {
        //        strsec1 = string.Empty;
        //    }
        //    else
        //    {
        //        strsec1 = " and sections='" + sec.ToString() + "'";
        //    }
        //    int todate1 = Convert.ToInt32(fromdate) + Convert.ToInt32(sem1) / 2;
        //    string batch = "Batch :" + fromdate + "-" + todate1;
        //    string sem3 = string.Empty;
        //    string bat = string.Empty;
        //    string academic = string.Empty;
        //    if (sem == "1")
        //    {
        //        sem3 = "II";
        //        bat = "Odd";
        //    }
        //    else if (sem == "2")
        //    {
        //        sem3 = "I";
        //        bat = "Even";
        //    }
        //    else if (sem == "3")
        //    {
        //        sem3 = "III";
        //        bat = "Odd";
        //    }
        //    else if (sem == "4")
        //    {
        //        sem3 = "IV";
        //        bat = "Even";
        //    }
        //    else if (sem == "5")
        //    {
        //        sem3 = "V";
        //        bat = "Odd";
        //    }
        //    else if (sem == "6")
        //    {
        //        sem3 = "VI";
        //        bat = "Even";
        //    }
        //    else if (sem == "7")
        //    {
        //        sem3 = "VII";
        //        bat = "Odd";
        //    }
        //    else if (sem == "8")
        //    {
        //        sem3 = "VIII";
        //        bat = "Even";
        //    }
        //    else if (sem == "9")
        //    {
        //        sem3 = "IX";
        //        bat = "Odd";
        //    }
        //    else if (sem == "10")
        //    {
        //        sem3 = "X";
        //        bat = "Even";
        //    }
        //    if ((sec == "") || (sec == "-1") || (sec == "All"))
        //    {
        //        sec = string.Empty;
        //    }
        //    if (bat == "odd")
        //    {
        //        academic = "" + fromdate + "-" + (Convert.ToInt32(fromdate) + 1) + "" + (bat) + "";
        //    }
        //    else
        //    {
        //        academic = "" + (Convert.ToInt32(fromdate) - 1) + "-" + fromdate + "(" + (bat) + ")";
        //    }
        //    //int pascountotal = 0;
        //    //int pascount = 0;
        //    //string perpass3 =string.Empty;
        //    string includePastout = string.Empty;


        //    if (!chkincludepastout.Checked)
        //    {

        //        includePastout = "and CC=0";
        //    }
        //    ////-------------
        //    string temp7 = "select count(*) from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + "and batch_year=" + ddlBatch.SelectedValue.ToString() + " and delflag=0 and exam_flag <> 'DEBAR' and " + includePastout + "" + strsec1 + "and current_semester>=" + ddlSemYr.SelectedValue.ToString() + "";
        //    string Strength = GetFunction(temp7);
        //    FpEntry.Visible = false;
        //    //FpSpread3.Visible = true;
        //    FpSpread2.Visible = true;
        //    FpSpread2.Sheets[0].ColumnHeader.Visible = true;
        //    FpSpread2.Sheets[0].RowHeader.Visible = false;
        //    FpSpread2.SheetCorner.RowCount = 11;
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 6);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 6);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 6);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 6);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 6);
        //    //for logo
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 5, 1);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(5, 0, 1, 2);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(5, 5, 1, 3);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(6, 0, 1, 2);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(6, 5, 1, 3);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(7, 0, 1, 8);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(8, 0, 1, 8);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(9, 0, 1, 2);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(9, 5, 1, 3);
        //    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(10, 0, 1, 8);
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].CellType = collogo;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].CellType = collogoright;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColorTop = Color.Black;
        //    for (int i = 0; i < 7; i++)
        //    {
        //        FpSpread2.Sheets[0].ColumnHeader.Columns[i].Locked = true;
        //    }
        //    for (int i = 0; i < 9; i++)
        //    {
        //        FpSpread2.Sheets[0].ColumnHeader.Rows[i].Border.BorderColor = Color.White;
        //        FpSpread2.Sheets[0].ColumnHeader.Rows[i].Font.Bold = true;
        //        FpSpread2.Sheets[0].ColumnHeader.Rows[i].Font.Size = FontUnit.Medium;
        //    }
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[7, 0].HorizontalAlign = HorizontalAlign.Center;
        //    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        //    style.Font.Size = 10;
        //    style.Font.Bold = true;
        //    FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //    FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorTop = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Text = address;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[2, 1].Text = phnfax;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[3, 1].Text = degree + "[" + branch + "]";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[4, 1].Text = email;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[5, 0].Text = string.Empty; // "Class :" + sem3 + "";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[5, 5].Text = batch;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Semester: " + sem + "  -" + sec + " Sec";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[6, 5].Text = "Academic Year :" + academic;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[7, 0].Text = test + "-" + "Result Analysis";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[9, 0].Text = "Subject Wise Percentage";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[9, 5].Text = "Class Strength" + ":" + " " + Strength;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[9, 2].Text = " ";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[9, 3].Text = " ";
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[9, 4].Text = " ";
        //    //string[] split_batch_deg = new string[10];
        //    ////'------------------------for settings the header based on print master table
        //    //hat.Clear();
        //    //hat.Add("college_code", Session["collegecode"].ToString());
        //    //hat.Add("form_name", "Resultanalysis.aspx");
        //    //dsprint = daccess2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        //    //if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
        //    //{
        //    //    if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
        //    //    {
        //    //        collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
        //    //    {
        //    //        address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
        //    //        address = address1;
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
        //    //    {
        //    //        address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
        //    //        address = address1 + "-" + address2;
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
        //    //    {
        //    //        district = dsprint.Tables[0].Rows[0]["address3"].ToString();
        //    //        address = address1 + "-" + address2 + "-" + district;
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
        //    //    {
        //    //        Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
        //    //        phnfax = "Phone :" + " " + Phoneno;
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
        //    //    {
        //    //        Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
        //    //        phnfax = phnfax + "Fax  :" + " " + Faxno;
        //    //    }
        //    //    if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
        //    //    {
        //    //        email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
        //    //    {
        //    //        email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
        //    //    {
        //    //        form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
        //    //    }
        //    //    if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
        //    //    {
        //    //        batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
        //    //        split_batch_deg = batch_degree_branch.Split('@');
        //    //        if ((split_batch_deg[1].ToString() != string.Empty) && (split_batch_deg[1].ToString() != null))
        //    //        {
        //    //            FpSpread2.Sheets[0].Cells[4, 1].Text = split_batch_deg[1].ToString();
        //    //        }
        //    //        if ((split_batch_deg[0].ToString() != string.Empty) && (split_batch_deg[0].ToString() != null))
        //    //        {
        //    //            FpSpread2.Sheets[0].Cells[5, 5].Text = split_batch_deg[0].ToString();
        //    //            FpSpread2.Sheets[0].Cells[5, 5].Border.BorderColorRight = Color.Black;
        //    //        }
        //    //        if ((split_batch_deg[2].ToString() != string.Empty) && (split_batch_deg[2].ToString() != null))
        //    //        {
        //    //            FpSpread2.Sheets[0].Cells[6, 0].Text = split_batch_deg[2].ToString();
        //    //        }
        //    //        if ((split_batch_deg[4].ToString() != string.Empty) && (split_batch_deg[4].ToString() != null))
        //    //        {
        //    //            FpSpread2.Sheets[0].Cells[8, 0].Text = split_batch_deg[4].ToString();
        //    //            FpSpread2.Sheets[0].Cells[8, 0].HorizontalAlign = HorizontalAlign.Center;
        //    //        }
        //    //        if ((split_batch_deg[3].ToString() != string.Empty) && (split_batch_deg[3].ToString() != null))
        //    //        {
        //    //            FpSpread2.Sheets[0].Cells[7, 0].Text = split_batch_deg[3].ToString();
        //    //            FpSpread2.Sheets[0].Cells[7, 0].Border.BorderColorRight = Color.Black;
        //    //        }
        //    //    }
        //    //    else//if the batch in print master didnt selected this else ll work
        //    //    {
        //    //        FpSpread2.Sheets[0].Cells[4, 1].Text = degree + "[" + branch + "]";
        //    //        FpSpread2.Sheets[0].Cells[5, 5].Text = batch;
        //    //        FpSpread2.Sheets[0].Cells[5, 5].HorizontalAlign = HorizontalAlign.Center;
        //    //        FpSpread2.Sheets[0].Cells[5, 5].Border.BorderColorRight = Color.Black;
        //    //        FpSpread2.Sheets[0].Cells[6, 0].Text = "Semester: " + sem + "  -" + sec + " Sec";
        //    //        FpSpread2.Sheets[0].Cells[7, 0].Text = test + "-" + "Result Analysis";
        //    //        FpSpread2.Sheets[0].Cells[7, 0].Border.BorderColorRight = Color.Black;
        //    //    }
        //    //    FpSpread2.Sheets[0].Cells[0, 1].Text = collnamenew1;
        //    //    FpSpread2.Sheets[0].Cells[1, 1].Text = address;
        //    //    FpSpread2.Sheets[0].Cells[2, 1].Text = phnfax;
        //    //    FpSpread2.Sheets[0].Cells[3, 1].Text = email;
        //    //    FpSpread2.Sheets[0].Cells[5, 0].Text = "Class :" + sem3 + "";
        //    //    FpSpread2.Sheets[0].Cells[5, 0].HorizontalAlign = HorizontalAlign.Center;
        //    //    FpSpread2.Sheets[0].Cells[6, 5].Text = "Academic Year :" + academic;
        //    //    FpSpread2.Sheets[0].Cells[6, 5].Border.BorderColorRight = Color.Black;
        //    //}
        //    ////'------------------------------------load the clg information
        //    //else if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        //    //{
        //    //    string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
        //    //    SqlCommand collegecmd = new SqlCommand(college, con);
        //    //    SqlDataReader collegename;
        //    //    con.Close();
        //    //    con.Open();
        //    //    collegename = collegecmd.ExecuteReader();
        //    //    if (collegename.HasRows)
        //    //    {
        //    //        while (collegename.Read())
        //    //        {
        //    //            collnamenew1 = collegename["collname"].ToString();
        //    //            address1 = collegename["address1"].ToString();
        //    //            address2 = collegename["address2"].ToString();
        //    //            district = collegename["district"].ToString();
        //    //            address = address1 + "-" + address2 + "-" + district;
        //    //            Phoneno = collegename["phoneno"].ToString();
        //    //            Faxno = collegename["faxno"].ToString();
        //    //            phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
        //    //            email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
        //    //        }
        //    //    }
        //    //    con.Close();
        //    //    FpSpread2.Sheets[0].Cells[0, 1].Text = collnamenew1;
        //    //    FpSpread2.Sheets[0].Cells[1, 1].Text = address;
        //    //    FpSpread2.Sheets[0].Cells[2, 1].Text = phnfax;
        //    //    FpSpread2.Sheets[0].Cells[3, 1].Text = email;
        //    //    FpSpread2.Sheets[0].Cells[4, 1].Text = degree + "[" + branch + "]";
        //    //    FpSpread2.Sheets[0].Cells[5, 0].Text = "Class :" + sem3 + "";
        //    //    FpSpread2.Sheets[0].Cells[5, 5].Text = batch;
        //    //    FpSpread2.Sheets[0].Cells[5, 5].Border.BorderColorRight = Color.Black;
        //    //    FpSpread2.Sheets[0].Cells[6, 0].Text = "Semester: " + sem + "  -" + sec + " Sec";
        //    //    FpSpread2.Sheets[0].Cells[6, 5].Text = "Academic Year :" + academic;
        //    //    FpSpread2.Sheets[0].Cells[6, 5].Border.BorderColorRight = Color.Black;
        //    //    FpSpread2.Sheets[0].Cells[7, 0].Text = test + "-" + "Result Analysis";
        //    //    FpSpread2.Sheets[0].Cells[7, 0].Border.BorderColorRight = Color.Black;
        //    //}
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 0].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 1].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 2].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 3].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 4].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 5].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 6].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Cells[10, 7].Border.BorderColorBottom = Color.Black;
        //    FpSpread2.Sheets[0].ColumnHeader.Rows[9].Font.Bold = false;
        //    int flag2 = 0;
        //    int flag = 0;
        //    int flagattend = 0;
        //    for (int i = (Convert.ToInt32(start) - 1); i < Convert.ToInt32(end); i++)
        //    {
        //        int regularrowstart = FpSpread2.Sheets[0].RowCount;
        //        if (i < regularrowstart)
        //        {
        //            regularrowstart = regularrowstart - i;
        //            if (flag2 == 0)
        //            {
        //                flag2 = 1;
        //                string gettext = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 0].Text;
        //                int newgettext = 0;
        //                if (int.TryParse(gettext, out newgettext))
        //                {
        //                    flag = 1;
        //                    newgettext = Convert.ToInt32(gettext);
        //                    string gettext1 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 3].Text;
        //                    int newgettext1 = 0;
        //                    if (int.TryParse(gettext1, out newgettext1))
        //                    {
        //                        flagattend = 1;
        //                        newgettext1 = Convert.ToInt32(gettext1);
        //                    }
        //                }
        //                if (flag == 1)
        //                {
        //                    if (flagattend != 1)
        //                    {
        //                        //FpSpread2.Sheets[0].
        //                        FpSpread2.ColumnHeader.Visible = true;
        //                        FpSpread2.SheetCorner.RowCount++;
        //                        Session["sheetcorner"] = FpSpread2.Sheets[0].SheetCorner.RowCount;
        //                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread2.SheetCorner.RowCount - 1, 3, 1, 5);
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Size = FontUnit.Medium;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Text = "S.No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Text = "Roll No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Text = "Name";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Text = "Subjects";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Bold = true;
        //                    }
        //                    else if (flagattend == 1)
        //                    {
        //                        FpSpread2.ColumnHeader.Visible = true;
        //                        FpSpread2.SheetCorner.RowCount++;
        //                        Session["sheetcorner"] = FpSpread2.Sheets[0].SheetCorner.RowCount;
        //                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread2.SheetCorner.RowCount - 1, 3, 1, 2);
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Size = FontUnit.Medium;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Text = "S.No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Text = "Roll No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Text = "Name";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Text = "Attendance%";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 5].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 6].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 7].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Bold = true;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 5].Text = " ";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 6].Text = " ";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 7].Text = " ";
        //                    }
        //                }
        //                else
        //                {
        //                    FpSpread2.SheetCorner.RowCount--;
        //                }
        //            }
        //            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - regularrowstart].Visible = true;
        //        }
        //        else
        //        {
        //            // FpSpread2.Sheets[0].RowCount++;
        //        }
        //        //'----------------------------for footer display start
        //        if (dsprint.Tables[0].Rows[0]["footer_name"].ToString() != string.Empty)
        //        {
        //            if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0") //all pages footer
        //            {
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //            }
        //            else //last page footer
        //            {
        //                if (ddlpage.SelectedIndex == (ddlpage.Items.Count - 1))
        //                {
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //                }
        //                else
        //                {
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = false;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = false;
        //                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
        //                }
        //            }
        //        }
        //        //'-------------------------end for footer display
        //    }
        //}//end for header in all pages
        //else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "2")
        //{
        //    for (int h = 0; h < FpSpread2.Sheets[0].ColumnHeader.RowCount; h++)
        //    {
        //        FpSpread2.Sheets[0].ColumnHeader.Rows[h].Visible = false;
        //    }
        //    for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread2.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + 10;
        //    int end = start + 34;
        //    int rowstart = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = FpSpread2.Sheets[0].RowCount - Convert.ToInt32(end);
        //    int flag2 = 0;
        //    int flag = 0;
        //    int flagattend = 0;
        //    for (int i = (Convert.ToInt32(start) - 1); i < Convert.ToInt32(end); i++)
        //    {
        //        int regularrowstart = FpSpread2.Sheets[0].RowCount;
        //        if (i < regularrowstart)
        //        {
        //            regularrowstart = regularrowstart - i;
        //            if (flag2 == 0)
        //            {
        //                flag2 = 1;
        //                string gettext = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 0].Text;
        //                int newgettext = 0;
        //                if (int.TryParse(gettext, out newgettext))
        //                {
        //                    flag = 1;
        //                    newgettext = Convert.ToInt32(gettext);
        //                    string gettext1 = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - regularrowstart, 3].Text;
        //                    int newgettext1 = 0;
        //                    if (int.TryParse(gettext1, out newgettext1))
        //                    {
        //                        flagattend = 1;
        //                        newgettext1 = Convert.ToInt32(gettext1);
        //                    }
        //                }
        //                if (flag == 1)
        //                {
        //                    if (flagattend != 1)
        //                    {
        //                        //FpSpread2.Sheets[0].
        //                        FpSpread2.ColumnHeader.Visible = true;
        //                        FpSpread2.SheetCorner.RowCount++;
        //                        Session["sheetcorner"] = FpSpread2.Sheets[0].SheetCorner.RowCount;
        //                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread2.SheetCorner.RowCount - 1, 3, 1, 5);
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Size = FontUnit.Medium;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Text = "S.No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Text = "Roll No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Text = "Name";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Text = "Subjects";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Bold = true;
        //                    }
        //                    else if (flagattend == 1)
        //                    {
        //                        FpSpread2.ColumnHeader.Visible = true;
        //                        FpSpread2.SheetCorner.RowCount++;
        //                        Session["sheetcorner"] = FpSpread2.Sheets[0].SheetCorner.RowCount;
        //                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread2.SheetCorner.RowCount - 1, 3, 1, 2);
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Size = FontUnit.Medium;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Text = "S.No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 0].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Text = "Roll No";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 1].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Text = "Name";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 2].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Text = "Attendance%";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 3].Border.BorderColor = Color.Black;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 5].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 6].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 7].Border.BorderColorTop = Color.White;
        //                        FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.SheetCorner.RowCount - 1].Font.Bold = true;
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 5].Text = " ";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 6].Text = " ";
        //                        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.SheetCorner.RowCount - 1, 7].Text = " ";
        //                    }
        //                }
        //                else
        //                {
        //                    if (FpSpread2.SheetCorner.RowCount != 0)
        //                    {
        //                        FpSpread2.SheetCorner.RowCount--;
        //                    }
        //                }
        //            }
        //            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - regularrowstart].Visible = true;
        //        }
        //        else
        //        {
        //            //    FpSpread2.Sheets[0].RowCount++;
        //        }
        //        //'----------------------------for footer display start
        //        if (dsprint.Tables[0].Rows[0]["footer_name"].ToString() != string.Empty)
        //        {
        //            if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0") //all pages footer
        //            {
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //            }
        //            else //last page footer
        //            {
        //                if (ddlpage.SelectedIndex == (ddlpage.Items.Count - 1))
        //                {
        //                    if (FpSpread2.Sheets[0].RowCount != 0)
        //                    {
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = true;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = true;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if (FpSpread2.Sheets[0].RowCount != 0)
        //                    {
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Visible = false;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = false;
        //                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
        //                    }
        //                }
        //            }
        //        }
        //        //'-------------------------end for footer display
        //    }
        //}
    }

    public void func_radio_header()
    {
        //ddlpage.Items.Clear();
        //int totrowcount = FpSpread2.Sheets[0].RowCount;
        //int pages = totrowcount / 35;
        //int intialrow = 1;
        //int remainrows = totrowcount % 35;
        //if (FpSpread2.Sheets[0].RowCount > 0)
        //{
        //    int i5 = 0;
        //    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //    for (int i = 1; i <= pages; i++)
        //    {
        //        i5 = i;
        //        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //        intialrow = intialrow + 35;
        //    }
        //    if (remainrows > 0)
        //    {
        //        i = i5 + 1;
        //        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //    }
        //}
    }

    protected void rdattnd_daywise_CheckedChanged(object sender, EventArgs e)
    {
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        ddlTest.SelectedIndex = -1;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
    }

    protected void rdattnd_hourwise_CheckedChanged(object sender, EventArgs e)
    {
        divgrid.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        //RadioHeader.Visible = false;
        //Radiowithoutheader.Visible = false;
        btnExcel.Visible = false;
        btnprint_Pdf.Visible = false;
        ddlTest.SelectedIndex = -1;
        norecordlbl.Visible = false;
        txtexcelname.Visible = false;
    }

    //protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    //{
    //    func_radio_header();
    //}
    //protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    ddlpage.Items.Clear();
    //    int totrowcount = FpSpread2.Sheets[0].RowCount;
    //    int pages = totrowcount / 45;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 45;
    //    if (FpSpread2.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (int i = 1; i <= pages; i++)
    //        {
    //            i5 = i;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 45;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //}

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        GetTest();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        GetTest();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //    string select_frm_date = txtFromDate.Text;
        //    string select_to_date = txtToDate.Text;
        //    Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex + "$" + select_frm_date + "$" + select_to_date;
        //    result_analysis_print();
        //    //lblpages.Visible = true;
        //    //ddlpage.Visible = true;
        //    string clmnheadrname = string.Empty;
        //    string dis_hdng_batch = string.Empty;
        //    string dis_hdng_deg = string.Empty;
        //    string dis_hdng_sec = string.Empty;
        //    string dis_test = string.Empty;
        //    if (ddlBatch.Text != string.Empty)
        //    {
        //        dis_hdng_batch = "Batch Year " + "- " + ddlBatch.SelectedItem.ToString();
        //    }
        //    if ((ddlDegree.Text != string.Empty) && (ddlBranch.Text != string.Empty))
        //    {
        //        dis_hdng_deg = ddlDegree.SelectedItem.ToString() + "[" + ddlBranch.SelectedItem.ToString() + "]";
        //    }
        //    if ((ddlSemYr.Text != string.Empty) && (ddlSec.Text != string.Empty))
        //    {
        //        dis_hdng_sec = "Semester " + "- " + ddlSemYr.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        //    }
        //    string dis_date = "From Date " + "- " + txtFromDate.Text.ToString() + " " + "To Date " + "- " + txtToDate.Text.ToString();
        //    if (ddlTest.Text != string.Empty)
        //    {
        //        dis_test = ddlTest.SelectedItem.ToString() + "Result Analysis";
        //    }
        //    Response.Redirect("Print_Master_Setting.aspx?ID=" + clmnheadrname + ":" + "Resultanalysis.aspx" + ":" + dis_hdng_batch + "@" + dis_hdng_deg + "@" + dis_hdng_sec + "@" + dis_date + "@" + dis_test + ":" + "Result Analysis Report");
        //}

        //public void func_header()
        //{
        //    if ((dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != " ") && (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != ""))
        //    {
        //        //  FpSpread2.Sheets[0].ColumnHeader.Rows[9].Visible = false;
        //        string hdr_nam = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        //        string[] spl_nwhdrname = hdr_nam.Split(',');
        //        int strwindexcnt = 1;
        //        if (spl_nwhdrname.GetUpperBound(0) > 0)
        //        {
        //            int shtcnrrwcnt = spl_nwhdrname.GetUpperBound(0) + 2;
        //            FpSpread2.Sheets[0].SheetCorner.RowCount += shtcnrrwcnt;
        //            for (int strw = Convert.ToInt32(Session["sheetcorner"]); strw < FpSpread2.Sheets[0].SheetCorner.RowCount - 1; strw++)
        //            {
        //                if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
        //                {
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Left;
        //                }
        //                else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
        //                {
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Center;
        //                }
        //                else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
        //                {
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        //                    FpSpread2.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Right;
        //                }
        //                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(strw, 0, 1, FpSpread2.Sheets[0].ColumnCount - 1);
        //                strwindexcnt++;
        //            }
        //        }
        //        else
        //        {
        //            FpSpread2.Sheets[0].SheetCorner.RowCount += 2;
        //            FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        //            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread2.Sheets[0].ColumnHeader.RowCount - 2, 0, 1, FpSpread2.Sheets[0].ColumnCount - 1);
        //            if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
        //            {
        //                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
        //            }
        //            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
        //            {
        //                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
        //            }
        //            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
        //            {
        //                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Right;
        //            }
        //        }
        //        //FpSpread2.Sheets[0].ColumnHeader.Rows[FpSpread2.Sheets[0].ColumnHeader.RowCount - 3].Visible = false;
        //    }
        //    //if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
        //    //{
        //    //    if (dsprint.Tables[0].Rows[0]["column_fields"].ToString() == string.Empty)
        //    //    {
        //    //        lblnorec.Visible = true;
        //    //        lblnorec.Text = "Select Atleast One Column From The TreeView";
        //    //        FpEntry.Visible = false;
        //    //        Buttontotal.Visible = false;
        //    //        lblrecord.Visible = false;
        //    //        DropDownListpage.Visible = false;
        //    //        TextBoxother.Visible = false;
        //    //        lblpage.Visible = false;
        //    //        TextBoxpage.Visible = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        lblnorec.Visible = false;
        //    //        lblnorec.Text =string.Empty;
        //    //        FpEntry.Visible = true;
        //    //        Buttontotal.Visible = true;
        //    //        lblrecord.Visible = true;
        //    //        DropDownListpage.Visible = true;
        //    //        TextBoxother.Visible = true;
        //    //        lblpage.Visible = true;
        //    //        TextBoxpage.Visible = true;
        //    //    }
        //    //}
    }

    //'----------------------func for footer
    public void function_footer()
    {
        //----------------start for setting the footer
        //if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
        //{
        //    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
        //    //   FpSpread2.Sheets[0].RowCount += 3;
        //    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //    string[] footer_text_split = footer_text.Split(',');
        //    int count_span = FpSpread2.Sheets[0].ColumnCount / footer_count;
        //    if (footer_text_split.GetUpperBound(0) > 0)
        //    {
        //        for (footer_balanc_col = 0; footer_balanc_col < footer_text_split.GetUpperBound(0) + 1; footer_balanc_col++)
        //        {
        //            if (footer_balanc_col == 0)
        //            {
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col].Text = footer_text_split[footer_balanc_col].ToString();
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col].Font.Size = FontUnit.Medium;
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col].Font.Bold = true;
        //            }
        //            else
        //            {
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Text = footer_text_split[footer_balanc_col].ToString();
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Size = FontUnit.Medium;
        //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Bold = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 5].Text = footer_text;
        //        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 5].Font.Size = FontUnit.Medium;
        //        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 5].Font.Bold = true;
        //    }
        //}
    }

    private bool CheckStudentStaffSelector(string batchYear)
    {
        bool isStudentStaffSelector = false;
        try
        {
            string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString().Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear.ToString()) >= batchyearsetting)
                    {
                        isStudentStaffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    isStudentStaffSelector = true;
                }
            }
            //if (isStudentStaffSelector)
            //{
            //    qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            //}
        }
        catch
        {
        }
        return isStudentStaffSelector;
    }

    protected void includepastout_CheckedChanged(object sender, EventArgs e)
    {

    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
}