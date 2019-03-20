using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Text;//added By Srinath 11/2/2013

public partial class StudentTestReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string strsec1 = string.Empty;
    string strsecmark = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string sqlpercmd, sqlmarkcmd;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string marks_per, marks_perfinal;
    string tnpsubno = "";
    string rollno;
    string strorder = "";
    string strregorder = "";
    double minmark, mark;
    int subjectctot = 0, criteriatot = 0, tottet;
    string examcode;
    double passperc;
    double presentperc;
    static int subjectcnt = 0;
    public string criteriano, subjno;
    string strsem = string.Empty;
    static int sectioncnt = 0;
    int count4 = 0;
    string order = "";
    string orderreg = "";
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataSet dsmethodgoper = new DataSet();
    DataSet dsmethodgosyl = new DataSet();
    DataSet dsmethodgosubj = new DataSet();
    DataSet dsmethodgocriteria = new DataSet();
    DataSet dsmethodgomark = new DataSet();
    DataSet dstotd = new DataSet();
    DataSet dsfact = new DataSet();
    DataSet tempdssubj = new DataSet();
    DataSet dssettings = new DataSet();

    static string grouporusercode = "";
    static string groupor_usercode = "";
    string strdayflag;
    string regularflag = "";
    string genderflag = "";
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);


    //Added By Srinath 11/2/2013 For Presentmonthcall()
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    int mmyycount;
    int moncount;
    string dd = "";
    int i, rows_count;
    string frdate, todate;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int unmark;
    TimeSpan ts;
    string tempvalue = "-1";
    int ObtValue = -1;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string diff_date;
    double dif_date = 0;
    double dif_date1 = 0;
    static Boolean splhr_flag = false;
    int demfcal, demtcal;
    string monthcal;

    static Hashtable ht_sphr = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    DataSet ds4 = new DataSet();
    DataSet ds3 = new DataSet();

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date;
    double cum_tot_point, per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs;
    double njhr, njdate, per_njdate, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int check;
    int colcnt = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    Dictionary<string, string> dicsubject = new Dictionary<string, string>();
    DataTable data = new DataTable();


    protected void Page_Load(object sender, EventArgs e)
    {
        string staff_code = "";
        staff_code = (string)Session["staff_code"];
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
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            groupor_usercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            groupor_usercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }

        if (!IsPostBack)
        {
            //--------Spread Design Format-----------

            txtfromdate.Attributes.Add("Readonly", "Readonly");
            txttodate.Attributes.Add("Readonly", "Readonly");


            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            norecordlbl.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            Printcontrol.Visible = false;



            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count >= 1)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Give degree rights for staff ";
                ddlbatch.Items.Clear();
            }
            //added by Srinath 11/2/20103 ==Start
            txtfromdate.Text = DateTime.Today.ToString("d/MM/yyyy");//added by Srinath 11/2/20103
            txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
            //End

            //-------------------------------Master settings

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            if (Session["usercode"] != "")
            {

                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";

                dssettings = d2.select_method(Master1, hat, "");
                strdayflag = "";


                if (dssettings != null && dssettings.Tables[0].Rows.Count > 0)
                {


                    foreach (DataRow mtrdr in dssettings.Tables[0].Rows)
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

                        if (mtrdr["settings"].ToString() == "Regular" && mtrdr["value"].ToString() == "1")
                        {
                            regularflag = "and ((registration.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral" && mtrdr["value"].ToString() == "1")
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
                        if (mtrdr["settings"].ToString() == "Transfer" && mtrdr["value"].ToString() == "1")
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
                                genderflag = genderflag + " or applyn.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (applyn.sex='1'";
                            }

                        }

                        if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["daywise"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["hourwise"] = "1";
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
            }//        

        }

    }

    //------Load Function for the Batch Details-----

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Degree Details-----

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
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
                ddldegree.DataSource = ds2;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Branch Details-----

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
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
                ddlbranch.DataSource = ds2;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Section Details-----

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds2;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();

                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsection.Enabled = false;
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    Bindtest(strbatch, strbranch, strsem, strsec1);
                }
                else
                {
                    ddlsection.Enabled = true;
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    //    Bindtest(strbatch, strbranch, strsem, strsec1);
                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    //------Load Function for the Semester Details-----

    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
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
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
            BindSubjecttest(strbatch, strbranch, strsem, strsec);
            //  Bindtest(strbatch, strbranch, strsem, strsec1);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }



    //------Load Function for the Subject Details-----


    public void BindSubjecttest(string strbatch, string strbranch, string strsem, string strsec)
    {
        try
        {
            count4 = 0;
            txtsubject.Text = "---Select---";
            chksubject.Checked = false;
            chklstsubject.Items.Clear();
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec = "";
                strsec1 = "";
            }
            else
            {
                strsec = " and registration.sections='" + ddlsection.Text.ToString() + "'";
                strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
            }
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            strsem = ddlsemester.SelectedValue.ToString();

            dsmethodgosubj.Dispose();
            dsmethodgosubj.Reset();


            if (Session["Staff_Code"].ToString() == "")
            {
                dsmethodgosubj = d2.BindSubjecttest(strbatch, strbranch, strsem, strsec);
            }
            else if (Session["Staff_Code"].ToString() != "")
            {

                dsmethodgosubj = d2.BindparticularstaffSubject(strbatch, strbranch, strsem, strsec, Session["Staff_Code"].ToString());

            }
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {

                chklstsubject.DataSource = dsmethodgosubj;
                chklstsubject.DataTextField = "subject_name";
                chklstsubject.DataValueField = "subject_no";
                chklstsubject.DataBind();
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < chklstsubject.Items.Count; i++)
                {
                    chklstsubject.Items[i].Selected = true;
                    if (chklstsubject.Items[i].Selected == true)
                    {
                        count4 += 1;
                    }
                    if (count4 > 0)
                    {
                        txtsubject.Text = "Subject(" + count4 + ")";
                        if (chklstsubject.Items.Count == count4)
                        {
                            chksubject.Checked = true;
                        }
                    }
                }
                Bindtest(strbatch, strbranch, strsem, strsec1);
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Visible = true;

        }
    }

    //load function for test details


    public void Bindtest(string strbatch, string strbranch, string strsem, string strsec1)
    {
        try
        {
            cbltest.Items.Clear();
            txttest.Text = "---Select---";
            chktest.Checked = false;
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {

                strsec1 = "";
            }
            else
            {

                strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
            }
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            strsem = ddlsemester.SelectedValue.ToString();

            dsmethodgocriteria.Dispose();
            dsmethodgocriteria.Reset();
            dsmethodgocriteria = d2.Bindtest(strbatch, strbranch, strsem, strsec1);
            if (dsmethodgocriteria.Tables[0].Rows.Count > 0)
            {
                cbltest.DataSource = dsmethodgocriteria;
                cbltest.DataTextField = "criteria";
                cbltest.DataValueField = "criteria_no";
                cbltest.DataBind();
                count4 = 0;
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cbltest.Items[i].Selected = true;
                    if (cbltest.Items[i].Selected == true)
                    {
                        count4 += 1;
                    }
                    if (count4 > 0)
                    {
                        txttest.Text = "Test (" + count4 + ")";
                        if (cbltest.Items.Count == count4)
                        {
                            chktest.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Visible = true;

        }
    }
    
    //------Load Function for the DropdownBox Details------

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            norecordlbl.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            Printcontrol.Visible = false;

            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count >= 1)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            norecordlbl.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;


            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        norecordlbl.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;

        //if (!Page.IsPostBack == false)
        //{
        //    ddlsemester.Items.Clear();
        //}
        //try
        //{
        //    if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
        //    {
        BindSem(strbranch, strbatchyear, collegecode);
        BindSectionDetail(strbatch, strbranch);
        //}
        //}
        //catch (Exception ex)
        //{
        //    string s = ex.ToString();
        //    Response.Write(s);
        //}
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;
            if (!Page.IsPostBack == false)
            {
                ddlsection.Items.Clear();
            }
            DataSet testsubj = new DataSet();
            BindSectionDetail(strbatch, strbranch);
            BindSubjecttest(strbatch, strbranch, strsem, strsec);

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        norecordlbl.Visible = false;
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        BindSubjecttest(strbatch, strbranch, strsem, strsec);
    }





    // method for button go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            if (ddldegree.Text == "")
            {
                return;
            }
            string[] splitfromcheck = txtfromdate.Text.Split(new Char[] { '/' });
            string[] splittocheck = txttodate.Text.Split(new char[] { '/' });
            string fdate = splitfromcheck[1] + '/' + splitfromcheck[0] + '/' + splitfromcheck[2];
            string tdate = splittocheck[1] + '/' + splittocheck[0] + '/' + splittocheck[2];
            DateTime fromdatechech = Convert.ToDateTime(fdate);
            DateTime todatecheck = Convert.ToDateTime(tdate);
            if (fromdatechech > todatecheck)
            {
                Showgrid.Visible = false;
                btnxl.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                btnPrint.Visible = false;
                errmsg.Text = "Please Enter To Date Grater Than From Date";
                errmsg.Visible = true;
            }
            else
            {
                if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
                {
                    strsec = "";
                    strsec1 = "";
                    strsecmark = "";
                }
                else
                {
                    strsec = " and registration.sections='" + ddlsection.Text.ToString() + "'";
                    strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
                    strsecmark = "and re.sections='" + ddlsection.Text.ToString() + "'";
                }
                //Added By Srinath 15/6/2013

                string orderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                if (orderby == "0")
                {
                    order = "Len(roll_no),registration.roll_no";
                    orderreg = "re.roll_no";
                }
                else if (orderby == "1")
                {
                    order = "registration.reg_no";
                    orderreg = "re.reg_no";
                }
                else if (orderby == "2")
                {
                    order = "registration.stud_name";
                    orderreg = "re.stud_name";
                }
                else if (orderby == "0,1")
                {
                    order = "Len(roll_no),registration.roll_no,registration.reg_no";
                    orderreg = "re.roll_no,re.reg_no";
                }
                else if (orderby == "0,2")
                {
                    order = "Len(roll_no),registration.roll_no,registration.stud_name";
                    orderreg = "re.roll_no,re.stud_name";
                }
                else if (orderby == "1,2")
                {
                    order = "registration.reg_no,registration.stud_name";
                    orderreg = "re.reg_no,re.stud_name";
                }
                else
                {
                    order = "Len(roll_no),registration.roll_no,registration.reg_no,registration.stud_name";
                    orderreg = "re.roll_no,re.reg_no,re.stud_name";
                }

                //End
                sqlpercmd = "select ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,roll_no,reg_no,registration.stud_name, CASE registration.stud_type When 'Day Scholar' then 'DS' When 'Hostler' then 'H' End as stud_type  from registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code='" + ddlbranch.SelectedValue + "'   " + strsec + "  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and registration.batch_year='" + ddlbatch.SelectedValue + "' " + strdayflag + " " + genderflag + " " + regularflag + " order by " + order + " ";
                methodgo(sqlpercmd);

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
        getcon.Close();
        getcon.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getcon);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getcon;
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

    private void methodgo(string sqlfirstcmd)
    {
        DataRow drow;
        Hashtable htpass = new Hashtable();
        Hashtable htfail = new Hashtable();
        dicsubject.Clear();
        Dictionary<int, string> dicsubcritno = new Dictionary<int, string>();
        Dictionary<int, string> dicsubstaffcolspn = new Dictionary<int, string>();
        Dictionary<int, string> dicmarkclr = new Dictionary<int, string>();
        int ini_column = 0;
        int no_column = 0;
        btnprintmaster.Visible = true;
        btnPrint.Visible = true;
        Printcontrol.Visible = false;
        Session["hourwise"] = "";
        ArrayList arrColHdrNames1 = new ArrayList();
        ArrayList arrColHdrNames = new ArrayList();
        Dictionary<int, string> dicrollno = new Dictionary<int, string>();
        try
        {
            int subcount = 0;
            int testcout = 0;
            hat.Clear();
            dsmethodgoper.Dispose();
            dsmethodgoper.Reset();
            int conversionmark = 0;
            colcnt = 0;
            string strgetconm = txtConvert_Value.Text.ToString();
            if (strgetconm.Trim() != "" && strgetconm != null)
            {
                conversionmark = Convert.ToInt32(strgetconm);
            }

            dsmethodgoper = d2.select_method(sqlfirstcmd, hat, "Text");
            if (dsmethodgoper != null && dsmethodgoper.Tables[0] != null && dsmethodgoper.Tables[0].Rows.Count > 0)
            {
                errmsg.Visible = false;//added By Srinath 11/2/2013
                Showgrid.Visible = true;
                arrColHdrNames1.Add("S.No");
                arrColHdrNames.Add("S.No");
                data.Columns.Add("col0");
                if (Session["Rollflag"] == "1")
                {
                    arrColHdrNames.Add("RollNo");
                    arrColHdrNames1.Add("RollNo");
                    colcnt++;
                    data.Columns.Add("col" + colcnt);
                }

                if (Session["Regflag"] == "1")
                {
                    arrColHdrNames.Add("Reg No");
                    arrColHdrNames1.Add("Reg No");
                    colcnt++;
                    data.Columns.Add("col" + colcnt);
                }
                colcnt++;
                arrColHdrNames.Add("Student Name");
                arrColHdrNames1.Add("Student Name");
                data.Columns.Add("col" + colcnt);
                if (Session["Studflag"] == "1")
                {
                    arrColHdrNames.Add("Student Type");
                    arrColHdrNames1.Add("Student Type");
                    colcnt++;
                    data.Columns.Add("col" + colcnt);
                }
                colcnt = colcnt + 1;




                Boolean tesflag = false;
                int colindex = colcnt - 1;
                int cocnt = colcnt - 1;
                int colspancnt = colcnt - 1;
                for (int subj = 0; subj < chklstsubject.Items.Count; subj++)
                {
                    if (chklstsubject.Items[subj].Selected == true)
                    {
                        subjectctot = subjectctot + 1;
                        subcount++;
                        no_column = 0;
                        string subcode = d2.GetFunction("select Subject_code from subject where subject_no='" + chklstsubject.Items[subj].Value.ToString() + "'");
                        for (int test = 0; test < cbltest.Items.Count; test++)
                        {
                            if (cbltest.Items[test].Selected == true)
                            {
                                if (tesflag == false)
                                {
                                    testcout++;
                                }
                                criteriatot = criteriatot + 1;
                                cocnt++;
                                colindex++;
                                arrColHdrNames1.Add(cbltest.Items[test].Text.ToString());
                                arrColHdrNames.Add(Convert.ToString(subcode + "- " + chklstsubject.Items[subj].Text.ToString()));
                                data.Columns.Add("col" + colindex);

                                dicsubcritno.Add(cocnt, Convert.ToString(cbltest.Items[test].Value) + ',' + Convert.ToString(chklstsubject.Items[subj].Value));

                                no_column = no_column + 1;
                                if (conversionmark > 0)
                                {
                                    colindex++;
                                    data.Columns.Add("col" + colindex);

                                    arrColHdrNames.Add(Convert.ToString(subcode + "- " + chklstsubject.Items[subj].Text.ToString()));
                                    arrColHdrNames1.Add(Convert.ToString(cbltest.Items[test].Text.ToString() + "(" + conversionmark + ")"));
                                    //System.Text.StringBuilder conhr1 = new System.Text.StringBuilder(Convert.ToString(cbltest.Items[test].Text.ToString() + "(" + conversionmark + ")"));

                                    //AddTableColumn(data, conhr1);

                                    cocnt++;

                                    no_column = no_column + 1;
                                }
                            }
                        }
                        if (no_column != 0)
                        {
                            //Saran

                            dicsubject.Add(cocnt.ToString(), Convert.ToString(subcode + "- " + chklstsubject.Items[subj].Text.ToString()) + '$' + no_column.ToString());
                            dicsubstaffcolspn.Add(colspancnt + 1, no_column.ToString());
                            colspancnt = colspancnt + no_column;
                        }
                        tesflag = true;
                    }

                    tottet = ((subjectctot * no_column));
                    if (conversionmark > 0)
                    {
                        tottet = tottet / 2;
                    }
                }

                arrColHdrNames.Add("No.of Test Conducted");
                arrColHdrNames1.Add("No.of Test Conducted");
                colindex++;
                data.Columns.Add("col" + colindex);
                arrColHdrNames.Add("No.of Test Passed");
                arrColHdrNames1.Add("No.of Test Passed");
                colindex++;
                data.Columns.Add("col" + colindex);
                arrColHdrNames.Add("No.of Test Failed");
                arrColHdrNames1.Add("No.of Test Failed");
                colindex++;
                data.Columns.Add("col" + colindex);
                arrColHdrNames.Add("Attendance %");
                arrColHdrNames1.Add("Attendance %");
                colindex++;
                data.Columns.Add("col" + colindex);

                DataRow drHdr0 = data.NewRow();
                DataRow drHdr1 = data.NewRow();
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                {
                    drHdr0[grCol] = arrColHdrNames[grCol];
                    drHdr1[grCol] = arrColHdrNames1[grCol];
                }
                data.Rows.Add(drHdr0);
                data.Rows.Add(drHdr1);

                int cn = 0;
                for (int bindval = 0; bindval < dsmethodgoper.Tables[0].Rows.Count; bindval++)
                {
                    cn++;
                    drow = data.NewRow();
                    data.Rows.Add(drow);

                    int clcnt = 0;

                    data.Rows[data.Rows.Count - 1][clcnt] = cn.ToString();
                    dicrollno.Add(data.Rows.Count - 1, dsmethodgoper.Tables[0].Rows[bindval]["Roll_No"].ToString());
                    if (Session["Rollflag"] == "1")
                    {
                        clcnt++;
                        data.Rows[data.Rows.Count - 1][clcnt] = dsmethodgoper.Tables[0].Rows[bindval]["Roll_No"].ToString();
                    }


                    if (Session["Regflag"] == "1")
                    {
                        clcnt++;
                        data.Rows[data.Rows.Count - 1][clcnt] = dsmethodgoper.Tables[0].Rows[bindval]["Reg_No"].ToString();

                    }
                    clcnt++;
                    data.Rows[data.Rows.Count - 1][clcnt] = dsmethodgoper.Tables[0].Rows[bindval]["stud_name"].ToString();

                    if (Session["Studflag"] == "1")
                    {
                        clcnt++;
                        data.Rows[data.Rows.Count - 1][clcnt] = dsmethodgoper.Tables[0].Rows[bindval]["stud_type"].ToString();

                    }
                }


                Dictionary<int, string> dicmaxminmark = new Dictionary<int, string>();
                Boolean columns = false;
                for (int n = colcnt; n < data.Columns.Count - 4; n++)
                {
                    int x = 0;
                    int ve = 0;
                    if (dicsubcritno.ContainsKey(n))
                    {
                        string value = dicsubcritno[n];
                        string[] spiltnum = value.Split(',');
                        criteriano = Convert.ToString(spiltnum[0]);
                        subjno = Convert.ToString(spiltnum[1]);
                        // sqlmarkcmd = "select distinct r.marks_obtained,isnull(e.min_mark,0) as min_mark,r.roll_no,e.exam_code,re.roll_no,Len(r.roll_no),e.max_mark from result r,exam_type e,registration re where r.roll_no=re.roll_no and e.exam_code=r.exam_code  and e.subject_no='" + subjno + "' and e.criteria_no='" + criteriano + "'  " + strsecmark + " and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  and re.sections=e.sections order by Len(r.roll_no),r.roll_no";
                        sqlmarkcmd = "select distinct r.marks_obtained,isnull(e.min_mark,0) as min_mark,r.roll_no,re.reg_no,re.stud_name,e.exam_code,re.roll_no,Len(r.roll_no),e.max_mark,Convert(nvarchar(15),e.exam_date,103) as edate from result r,exam_type e,registration re where r.roll_no=re.roll_no and e.exam_code=r.exam_code  and e.subject_no='" + subjno + "' and e.criteria_no='" + criteriano + "'  " + strsecmark + " and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by " + orderreg + " ";//modifoed By Srinath 18/2/2013
                        dsmethodgomark = d2.select_method(sqlmarkcmd, hat, "Text");
                        ve = dsmethodgomark.Tables[0].Rows.Count;

                        for (int v = 2; v < data.Rows.Count; v++)
                        {
                            rollno = Convert.ToString(dicrollno[v]);
                            if (v < data.Rows.Count)
                            {
                                if (dsmethodgomark != null && dsmethodgomark.Tables[0] != null && dsmethodgomark.Tables[0].Rows.Count > 0)
                                {
                                    if (x <= ve - 1)
                                    {
                                        // string rov = dsmethodgomark.Tables[0].Rows[x]["Roll_no"].ToString();

                                        dsmethodgomark.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                                        DataView dvroll = dsmethodgomark.Tables[0].DefaultView;
                                        //if (rollno == rov)
                                        if (dvroll.Count > 0)
                                        {
                                            if (subcount == 1 && testcout == 1 && conversionmark == 0)
                                            {
                                                if (dicsubcritno.ContainsKey(n))
                                                {
                                                    dicsubject.Remove(n.ToString());
                                                    dicsubject.Add(n.ToString(), Convert.ToString(dvroll[0]["edate"]) + '$' + "1");

                                                }
                                            }
                                            else if (subcount > 0 && testcout == 1 && conversionmark > 0)
                                            {
                                                if (!columns)
                                                {

                                                    data.Rows[1][n] = "Max (" + dvroll[0]["max_mark"].ToString() + ")";
                                                    data.Rows[1][n + 1] = "Max (" + conversionmark + ")";


                                                    columns = true;
                                                }

                                            }

                                            examcode = Convert.ToString(dvroll[0]["exam_code"]);
                                            marks_per = Convert.ToString(dvroll[0]["marks_obtained"]);
                                            if (marks_per.ToString().Trim() == "")
                                                marks_per = "0";
                                            mark = Convert.ToDouble(marks_per);
                                            minmark = Convert.ToDouble(dvroll[0]["min_mark"]);
                                            if (dicsubcritno.ContainsKey(n))
                                            {
                                                dicmaxminmark.Remove(n);
                                                dicmaxminmark.Add(n, Convert.ToString(examcode) + ',' + Convert.ToString(minmark));
                                            }

                                            if (mark >= minmark)
                                            {
                                                if (htpass.Contains(Convert.ToString(rollno)))
                                                {
                                                    int passcount = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rollno), htpass));
                                                    passcount++;
                                                    htpass[Convert.ToString(rollno)] = passcount;
                                                }
                                                else
                                                {
                                                    htpass.Add(Convert.ToString(rollno), 1);
                                                }
                                            }
                                            else
                                            {

                                                if (htfail.Contains(Convert.ToString(rollno)))
                                                {
                                                    int failcount = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rollno), htfail));
                                                    failcount++;
                                                    htfail[Convert.ToString(rollno)] = failcount;
                                                }
                                                else
                                                {
                                                    htfail.Add(Convert.ToString(rollno), 1);
                                                }
                                            }

                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_perfinal = "A";
                                                    break;
                                                case "-2":
                                                    marks_perfinal = "EL";
                                                    break;
                                                case "-3":
                                                    marks_perfinal = "EOD";
                                                    break;
                                                case "-4":
                                                    marks_perfinal = "ML";
                                                    break;
                                                case "-5":
                                                    marks_perfinal = "SOD";
                                                    break;
                                                case "-6":
                                                    marks_perfinal = "NSS";
                                                    break;
                                                case "-7":
                                                    marks_perfinal = "NJ";
                                                    break;
                                                case "-8":
                                                    marks_perfinal = "S";
                                                    break;
                                                case "-9":
                                                    marks_perfinal = "L";
                                                    break;
                                                case "-10":
                                                    marks_perfinal = "NCC";
                                                    break;
                                                case "-11":
                                                    marks_perfinal = "HS";
                                                    break;
                                                case "-12":
                                                    marks_perfinal = "PP";
                                                    break;
                                                case "-13":
                                                    marks_perfinal = "SYOD";
                                                    break;
                                                case "-14":
                                                    marks_perfinal = "COD";
                                                    break;
                                                case "-15":
                                                    marks_perfinal = "OOD";
                                                    break;
                                                case "-16":
                                                    marks_perfinal = "OD";
                                                    break;
                                                //Added By Subburaj 21.08.2014//
                                                case "-18":
                                                    marks_perfinal = "RAA";
                                                    break;
                                                //*****End**************//
                                                default:
                                                    marks_perfinal = marks_per;
                                                    break;
                                            }


                                            data.Rows[v][n] = Convert.ToString(marks_perfinal);


                                            if (conversionmark > 0)
                                            {
                                                int num = 0;
                                                string colname = data.Columns[n + 1].ColumnName;
                                                if (int.TryParse(marks_perfinal, out num))
                                                {
                                                    int getmaxmark = Convert.ToInt32(dvroll[0]["max_mark"]);
                                                    Double getmarkconve = ((Convert.ToDouble(marks_perfinal) / Convert.ToDouble(getmaxmark)) * Convert.ToDouble(conversionmark));
                                                    getmarkconve = Math.Round(getmarkconve, 0, MidpointRounding.AwayFromZero);
                                                    data.Rows[v][colname] = Convert.ToString(getmarkconve);



                                                }
                                                else
                                                {
                                                    data.Rows[v][colname] = Convert.ToString(marks_perfinal);


                                                }

                                            }
                                            if (x == 0)
                                            {
                                                if (!columns)
                                                {
                                                    //Saran
                                                    string strcrimax = "";
                                                    strcrimax = data.Rows[1][n].ToString();
                                                    strcrimax = strcrimax + "(" + dvroll[0]["max_mark"].ToString() + ")";
                                                    data.Rows[1][n] = strcrimax;

                                                }

                                            }
                                            x = x + 1;
                                        }
                                    }

                                }
                            }

                        }
                        if (conversionmark > 0)
                        {
                            n = n + 1;

                        }

                    }
                }

                for (int vxt = 2; vxt < data.Rows.Count; vxt++)
                {
                    rollno = Convert.ToString(dicrollno[vxt]);
                    string rollnov = Convert.ToString(dsmethodgoper.Tables[0].Rows[vxt - 2]["roll_no"]);
                    int passcountprint = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rollnov), htpass));
                    int failcountprint = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(rollnov), htfail));

                    data.Rows[vxt][data.Columns.Count - 4] = Convert.ToString(tottet);
                    data.Rows[vxt][data.Columns.Count - 3] = Convert.ToString(passcountprint);
                    data.Rows[vxt][data.Columns.Count - 2] = Convert.ToString(failcountprint);

                    if (failcountprint > 2)
                    {
                        dicmarkclr.Add(vxt, rollno);
                    }

                }

                //added By Srinath 11/2/2013 ==Start 



                //Persent Month Call Function
                string sections = "";
                string strsec = "";
                string sec = "";
                sections = ddlsection.SelectedValue.ToString();
                if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and sections='" + sections.ToString() + "'";
                    sec = sections;
                }
                filteration();
                string filterwithsection = "exam_flag<>'debar' and delflag=0 and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'  and sections='" + sec.ToString() + "'" + strorder + "";
                string filterwithoutsection = "exam_flag<>'debar' and delflag=0 and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + strorder + "";
                hat.Clear();
                hat.Add("bath", int.Parse(ddlbatch.SelectedItem.ToString()));
                hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                hat.Add("sec", sec.ToString());
                hat.Add("filterwithsection", filterwithsection.ToString());
                hat.Add("filterwithoutsection", filterwithoutsection.ToString());
                ds4 = d2.select_method("ALL_STUDENT_DETAILS", hat, "sp");

                hat.Clear();
                hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                hat.Add("sem_ester", int.Parse(ddlsemester.SelectedValue.ToString()));
                ds = d2.select_method("period_attnd_schedule", hat, "sp");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                    fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                    anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                    minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                    minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                }
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                count = ds1.Tables[0].Rows.Count;
                int stu_count = ds4.Tables[0].Rows.Count;

                int rowcnt = 1;
                for (rows_count = 0; rows_count < stu_count; rows_count++)
                {
                    rowcnt++;
                    string roll_no = Convert.ToString(ds4.Tables[0].Rows[rows_count][0]);
                    per_abshrs_spl = 0;
                    tot_per_hrs_spl = 0;
                    tot_ondu_spl = 0;
                    tot_ml_spl = 0;
                    per_hhday_spl = 0;
                    unmark_spl = 0;
                    tot_conduct_hr_spl = 0;
                    check = 1;
                    per_workingdays1 = 0;
                    cum_per_workingdays1 = 0;
                    if (rows_count == 0)
                    {
                        frdate = txtfromdate.Text;
                        todate = txttodate.Text;
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
                        DataSet ds_sphr = new DataSet();
                        ht_sphr.Clear();
                        string hrdetno = "";
                        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and date between '" + per_from_gendate.ToString() + "' and '" + per_to_gendate.ToString() + "'";
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
                        //Added By Srinath 25/2/2013 ======Start
                        splhr_flag = false;
                        String splhrquery = "select rights from  special_hr_rights where " + grouporusercode + "";
                        DataSet dssplhrchech = d2.select_method(splhrquery, hat, "Text");
                        if (dssplhrchech.Tables[0].Rows.Count > 0)
                        {
                            string spl_hr_rights = dssplhrchech.Tables[0].Rows[0]["rights"].ToString();
                            if (spl_hr_rights == "True" || spl_hr_rights == "true")
                            {
                                splhr_flag = true;

                            }
                        }
                        string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                        DataSet dsattendacereport = d2.select_method(Master1, hat, "Text");
                        if (dsattendacereport.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsattendacereport.Tables[0].Rows.Count; i++)
                            {
                                string attencheck = dsattendacereport.Tables[0].Rows[i]["settings"].ToString();
                                if (attencheck.ToString().Trim() == "Day Wise" && dsattendacereport.Tables[0].Rows[i]["Value"].ToString().Trim() == "1")
                                {
                                    Session["Daywise"] = "1";
                                    i = dsattendacereport.Tables[0].Rows.Count;
                                }
                                if (attencheck.ToString().Trim() == "Hour Wise" && dsattendacereport.Tables[0].Rows[i]["Value"].ToString().Trim() == "1")
                                {
                                    Session["Hourwise"] = "1";
                                    i = dsattendacereport.Tables[0].Rows.Count;
                                }

                            }
                        }
                        //=============End
                    }
                    persentmonthcal();
                    //Added By Srinath 25/2/2013 ======Start
                    if (Session["hourwise"].ToString() == "1")
                    {
                        per_tage_date = ((per_per_hrs + tot_per_hrs_spl_fals) / (per_workingdays1 + tot_conduct_hr_spl_fals) * 100);
                        per_tage_date = Math.Round(per_tage_date, 2);
                    }
                    else
                    {
                        per_tage_date = ((pre_present_date / per_workingdays) * 100);
                        per_tage_date = Math.Round(per_tage_date, 2);
                    }
                    //=========End
                    string attpercent = per_tage_date.ToString();

                    if (attpercent == "NaN")
                    {
                        attpercent = "0";
                    }
                    else if (attpercent == "Infinity")
                    {
                        attpercent = "0";
                    }
                    data.Rows[rowcnt][data.Columns.Count - 1] = attpercent;


                }
                //=====End


                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "Total No Of Students";
                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "No Of Students Appeared";

                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "No Of Students Absent";
                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "No Of Students Passed";
                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "No Of Students Failed";
                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "Pass Percentage";

                drow = data.NewRow();
                data.Rows.Add(drow);
                data.Rows[data.Rows.Count - 1][0] = "Name of the Faculty/Department Acronym";



                //for (int nxt = 5; nxt < FpSpread1.Sheets[0].ColumnCount - 3; nxt++)//modified By Srinath 11/2/2013
                for (int nxt = 4; nxt < data.Columns.Count - 4; nxt++)
                {

                    if (dicmaxminmark.ContainsKey(nxt))
                    {
                        string examcomark = dicmaxminmark[nxt];
                        string[] splitmrk = examcomark.Split(',');
                        string examc = Convert.ToString(splitmrk[0]);
                        string minmrk = Convert.ToString(splitmrk[1]);

                        if (examc != null && examc != "" && minmrk != null && minmrk != "")
                        {
                            //modified by srinath 2/9/2014
                            string secsss = sec;
                            //string secsss = ddlsection.SelectedItem.Text.ToString();
                            dstotd = d2.retriveoveralldetailsp(examc, Convert.ToInt32(minmrk), secsss);
                            //present
                            if (dstotd != null && dstotd.Tables[8] != null)
                            {
                                data.Rows[data.Rows.Count - 7][nxt] = Convert.ToString(dsmethodgoper.Tables[0].Rows.Count);
                                data.Rows[data.Rows.Count - 6][nxt] = Convert.ToString(dstotd.Tables[8].Rows[0]["PRESENT_COUNT"]);


                                presentperc = Convert.ToDouble(dstotd.Tables[8].Rows[0]["PRESENT_COUNT"]);
                            }
                            //absent
                            if (dstotd != null && dstotd.Tables[9] != null)
                            {
                                data.Rows[data.Rows.Count - 5][nxt] = Convert.ToString(dstotd.Tables[9].Rows[0]["ABSENT_COUNT"]);

                            }
                            //passcount
                            if (dstotd != null && dstotd.Tables[1] != null)
                            {
                                data.Rows[data.Rows.Count - 4][nxt] = Convert.ToString(dstotd.Tables[1].Rows[0]["PASS_COUNT"]);


                                passperc = Convert.ToDouble(dstotd.Tables[1].Rows[0]["PASS_COUNT"]);

                            }
                            //failcount
                            if (dstotd != null && dstotd.Tables[2] != null)
                            {
                                data.Rows[data.Rows.Count - 3][nxt] = Convert.ToString(dstotd.Tables[2].Rows[0]["FAIL_COUNT"]);

                            }

                            //percentage
                            if (dstotd != null && dstotd.Tables[3] != null)
                            {
                                double tempperce = ((passperc / presentperc) * 100);
                                data.Rows[data.Rows.Count - 2][nxt] = Convert.ToString(Decimal.Parse(tempperce.ToString("0.00")));

                            }

                        }
                        string subno = dicsubcritno[nxt];
                        string[] spilt1 = subno.Split(',');
                        string subjnofact = Convert.ToString(spilt1[1]);
                        if (subjnofact != null)
                        {
                            if (tnpsubno != subjnofact)
                            {
                                string sqlcmdfact = "select distinct e.staff_code,s.staff_name,d.dept_acronym  from staff_selector e,staffmaster s,stafftrans sts,department d where  e.staff_code=s.staff_code and sts.staff_code=s.staff_code and d.dept_code=sts.dept_code  and subject_no='" + subjnofact + "' " + strsec1 + "";
                                dsfact = d2.select_method(sqlcmdfact, hat, "Text");
                                if (dsfact != null && dsfact.Tables[0] != null)
                                {
                                    string staffnme = "";
                                    string deptnme = "";
                                    for (int k = 0; k < dsfact.Tables[0].Rows.Count; k++)
                                    {
                                        if (staffnme == "")
                                        {
                                            staffnme = Convert.ToString(dsfact.Tables[0].Rows[k]["staff_name"]);
                                            deptnme = Convert.ToString(dsfact.Tables[0].Rows[k]["dept_acronym"]);
                                        }
                                        else
                                        {
                                            staffnme = staffnme + ", " + Convert.ToString(dsfact.Tables[0].Rows[k]["staff_name"]);
                                            deptnme = deptnme + ", " + Convert.ToString(dsfact.Tables[0].Rows[k]["dept_acronym"]);
                                        }
                                    }
                                    data.Rows[data.Rows.Count - 1][nxt] = Convert.ToString(staffnme + " / " + deptnme);

                                    tnpsubno = subjnofact.ToString();
                                }
                            }

                        }
                        if (conversionmark > 0)
                        {

                            nxt = nxt + 1;
                        }
                    }
                }
                //Hidden By srinath 10/6/2013
                //int xyz = 0;
                //for (int v = 0; v < FpSpread1.Sheets[0].RowCount - 7; v++)
                //{
                //    xyz = xyz + 1;
                //    FpSpread1.Sheets[0].Cells[v, 0].Text = xyz.ToString();
                //}


                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, coltciunt);

                ////Modified By SRinath 11/2/2013 ========Start
                //if (ddlsection.ToString() == "All" || ddlsection.ToString() == string.Empty || ddlsection.ToString() == "-1")
                //{
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Batch  :" + ddlbatch.SelectedItem.Text + " Degree  :" + ddldegree.SelectedItem.Text + " Branch  :" + ddlbranch.SelectedItem.Text + " Semester  :" + ddlsemester.SelectedItem.Text + " Section  :" + ddlsection.SelectedItem.Text;
                //}
                //else
                //{
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Batch  :" + ddlbatch.SelectedItem.Text + " Degree  :" + ddldegree.SelectedItem.Text + " Branch  :" + ddlbranch.SelectedItem.Text + " Semester  :" + ddlsemester.SelectedItem.Text;
                //}
                //===End
                if (data.Columns.Count > 0 && data.Rows.Count > 1)
                {

                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    btnxl.Visible = true;
                    txtexcelname.Visible = true;
                    lblrptname.Visible = true;
                    lblnorec.Visible = true;



                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {


                        for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                        {

                            if (Showgrid.Rows[0].Cells[j].Text == "Reg No" || Showgrid.Rows[0].Cells[j].Text == "Roll No" || Showgrid.Rows[0].Cells[j].Text == "Student Name" || Showgrid.Rows[0].Cells[j].Text == "Student Type")
                            {

                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                            }

                            else
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                if (Showgrid.HeaderRow.Cells[j].Text == "No.of Test Failed")
                                {
                                    if (Showgrid.Rows[i].Cells[j].Text != "&nbsp;")
                                    {
                                        int rr = Convert.ToInt32(Showgrid.Rows[i].Cells[j].Text);
                                        if (rr > 2)
                                        {
                                            Showgrid.Rows[i].Cells[j].BackColor = Color.LightCoral;
                                        }
                                    }
                                }

                            }
                        }
                    }

                    for (int g = colcnt; g < data.Columns.Count - 4; g++)
                    {
                        if (dicmaxminmark.ContainsKey(g))
                        {
                            string examcomark = dicmaxminmark[g];
                            string[] splitmrk = examcomark.Split(',');
                            string minmrk = Convert.ToString(splitmrk[1]);
                            for (int j = 2; j < Showgrid.Rows.Count - 7; j++)
                            {
                                string mark = data.Rows[j][g].ToString();
                                double stuMark = 0;
                                double.TryParse(mark, out stuMark);

                                if (mark != "")
                                {
                                    if (stuMark!=0)
                                    {
                                        if (Convert.ToDouble(stuMark) < Convert.ToDouble(minmark))
                                        {
                                            Showgrid.Rows[j].Cells[g].ForeColor = Color.Red;
                                            Showgrid.Rows[j].Cells[g].BorderColor = Color.Black;
                                            if (conversionmark > 0)
                                            {
                                                Showgrid.Rows[j].Cells[g + 1].ForeColor = Color.Red;
                                                Showgrid.Rows[j].Cells[g + 1].BorderColor = Color.Black;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Showgrid.Rows[j].Cells[g].ForeColor = Color.Red;
                                        Showgrid.Rows[j].Cells[g].BorderColor = Color.Black;
                                        if (conversionmark > 0)
                                        {
                                            Showgrid.Rows[j].Cells[g + 1].ForeColor = Color.Red;
                                            Showgrid.Rows[j].Cells[g + 1].BorderColor = Color.Black;
                                        }
                                    }
                                }
                            }

                        }

                    }

                    int d = Convert.ToInt32(data.Rows.Count - 7);
                    for (int g = d; g < data.Rows.Count; g++)
                    {
                        Showgrid.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[g].Cells[0].ColumnSpan = colcnt;
                        for (int a = 1; a < colcnt; a++)
                            Showgrid.Rows[g].Cells[a].Visible = false;
                        if (conversionmark > 0)
                        {
                            for (int cl = colcnt; cl < data.Columns.Count - 4; cl = cl + 2)
                            {
                                Showgrid.Rows[g].Cells[cl].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[g].Cells[cl].ColumnSpan = 2;

                                Showgrid.Rows[g].Cells[cl + 1].Visible = false;
                            }
                        }
                    }
                    int col = 0;

                    foreach (KeyValuePair<int, string> dr in dicsubstaffcolspn)
                    {
                        int colno = dr.Key;
                        string columspn = dr.Value;

                        if (col != 0)
                        {
                            col = col + Convert.ToInt32(columspn);
                        }
                        else
                        {
                            col = Convert.ToInt32(colno) + Convert.ToInt32(columspn);
                        }
                        Showgrid.Rows[data.Rows.Count - 1].Cells[Convert.ToInt32(colno)].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[data.Rows.Count - 1].Cells[Convert.ToInt32(colno)].ColumnSpan = Convert.ToInt32(columspn);
                        for (int a = Convert.ToInt32(colno + 1); a < col; a++)
                            Showgrid.Rows[data.Rows.Count - 1].Cells[a].Visible = false;


                    }
                    for (int g = 2; g < data.Rows.Count; g++)
                    {
                        if (dicmarkclr.ContainsKey(g))
                            Showgrid.Rows[g].Cells[data.Columns.Count - 2].BackColor = Color.LightCoral;
                        else
                            Showgrid.Rows[g].Cells[data.Columns.Count - 2].BackColor = Color.White;
                    }


                    int rct = Showgrid.Rows.Count - 2;
                    //Rowspan
                    GridViewRow row = Showgrid.Rows[0];
                    GridViewRow previousRow = Showgrid.Rows[1];
                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[1].Font.Bold = true;
                    Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {

                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                   previousRow.Cells[i].RowSpan + 1;
                            previousRow.Cells[i].Visible = false;
                        }
                    }

                    //ColumnSpan
                    for (int rowIndex = Showgrid.Rows.Count - rct - 1; rowIndex >= 0; rowIndex--)
                    {
                        for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;

                                }
                            }
                        }

                    }

                }
            }
            else
            {
                norecordlbl.Text = "No Record Found";
                norecordlbl.Visible = true;
            }
        }
        catch (Exception e)
        {
            errmsg.Visible = true;
            errmsg.Text = e.ToString();
        }

    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

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


    //----------Subject Dropdown Extender-----------------

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = true;
                txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = false;
                txtsubject.Text = "---Select---";
            }
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsubject.Text = "---Select---";
        chksubject.Checked = false;
        int subjectcount = 0;

        for (int i = 0; i < chklstsubject.Items.Count; i++)
        {
            if (chklstsubject.Items[i].Selected == true)
            {
                subjectcount = subjectcount + 1;
            }
        }

        if (subjectcount > 0)
        {
            txtsubject.Text = "Subject(" + subjectcount.ToString() + ")";
            if (subjectcount == chklstsubject.Items.Count)
            {
                chksubject.Checked = true;
            }
        }
        subjectcnt = subjectcount;
        //BindTest(strbatch, strbranch);

    }

    public void subjectimg_Click(object sender, ImageClickEventArgs e)
    {
        subjectcnt = subjectcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsubject.Items[r].Selected = false;

        txtsubject.Text = "Subject(" + sectioncnt.ToString() + ")";
        if (txtsubject.Text == "Subject(0)")
        {
            txtsubject.Text = "---Select---";

        }

    }

    public Label subjectlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton subjectimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }



    //----------Test Dropdown Extender-----------------

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        if (chktest.Checked == true)
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = true;
            }
            txttest.Text = "Test (" + (cbltest.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = false;
            }
            txttest.Text = "---Select---";
        }
    }

    protected void cbltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        psubject.Focus();
        txttest.Text = "---Select---";
        chktest.Checked = false;
        int subjectcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cbltest.Items.Count; i++)
        {
            if (cbltest.Items[i].Selected == true)
            {
                value = cbltest.Items[i].Text;
                code = cbltest.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
            }

        }
        if (subjectcount > 0)
        {
            txttest.Text = "Test (" + subjectcount.ToString() + ")";
            if (subjectcount == cbltest.Items.Count)
            {
                chktest.Checked = true;
            }
        }
        subjectcnt = subjectcount;
        //BindTest(strbatch, strbranch);

    }

    public void testimg_Click(object sender, ImageClickEventArgs e)
    {
        subjectcnt = subjectcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbltest.Items[r].Selected = false;

        txttest.Text = "Test(" + sectioncnt.ToString() + ")";
        if (txttest.Text == "Test(0)")
        {
            txttest.Text = "---Select---";

        }

    }

    public Label testlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton testimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }



    //------Method for the Excel Coversion -----
    protected void btnxl_Click(object sender, EventArgs e)
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
                    //print = strexcelname;
                    //FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

                    d2.printexcelreportgrid(Showgrid, strexcelname);
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                    txtexcelname.Focus();// added by sridhar 03 sep 2014
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
        //string appPath = HttpContext.Current.Server.MapPath("~");
        //string print = "";
        //if (appPath != "")
        //{
        //    int i = 1;
        //    appPath = appPath.Replace("\\", "/");
        //e:
        //    try
        //    {
        //        print = "Student Over All CAM Report" + i;
        //        FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet

        //    }
        //    catch
        //    {
        //        i++;
        //        goto e;

        //    }
        //}
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    //added By Srinath 11/2/2013
    public void persentmonthcal()
    {

        Boolean isadm = false;
        try
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
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;

            //-----------
            dumm_from_date = per_from_date;

            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(ddlbranch.SelectedValue.ToString()));
                hat.Add("sem", int.Parse(ddlsemester.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;

                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedItem.ToString() + "";

                DataSet dsholiday = d2.select_method(sqlstr_holiday, hat, "Text");
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
            }

            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables[0].Rows.Count != 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;
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
                            if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
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

                                    if (split_holiday_status[0].ToString() == "3")
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "1";
                                    }
                                    else if (split_holiday_status[0].ToString() == "1")
                                    {
                                        if (split_holiday_status[1].ToString() == "1")
                                        {
                                            split_holiday_status_1 = "0";
                                            split_holiday_status_2 = "1";
                                        }

                                        if (split_holiday_status[2].ToString() == "1")
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
                                            value = ds2.Tables[0].Rows[next][date].ToString();

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
                                                my_un_mark++;
                                            }
                                        }

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
                                    temp_unmark = 0;
                                    njhr = 0;

                                    int k = fnhrs + 1;

                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds2.Tables[0].Rows[next][date].ToString();

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

                                                my_un_mark++;
                                            }
                                        }

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

            if (check == 1)
            {

                per_tot_ondu = tot_ondu;
                per_tot_ml = tot_ml;
                per_njdate = njdate;
                pre_present_date = Present - njdate;
                per_per_hrs = tot_per_hrs;
                per_absent_date = Absent;
                pre_ondu_date = Onduty;
                pre_leave_date = Leave;
                per_workingdays = workingdays - per_njdate;
                per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark;
                per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value;
                per_dum_unmark = dum_unmark;
            }
            if (check == 2)
            {
                cum_tot_ondu = tot_ondu;
                cum_tot_ml = tot_ml;
                cum_njdate = njdate;
                cum_present_date = Present - njdate;
                cum_per_perhrs = tot_per_hrs;
                cum_absent_date = Absent;
                cum_ondu_date = Onduty;
                cum_leave_date = Leave;
                cum_workingdays = workingdays - cum_njdate;
                cum_per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark;
                cum_per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value;
                cum_dum_unmark = dum_unmark;
                cum_tot_point = absent_point + leave_point;
            }
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
        catch
        {
        }
    }
    //added By Srinath 11/2/2013
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

                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";

                ds_splhr_query_master = d2.select_method(splhr_query_master, hat, "Text");

                if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
                {
                    for (int dsspahl = 0; dsspahl < ds_splhr_query_master.Tables[0].Rows.Count; dsspahl++)
                    {

                        value = ds_splhr_query_master.Tables[0].Rows[dsspahl]["attendance"].ToString();

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
                if (check == 1)
                {

                    per_abshrs_spl_fals = per_abshrs_spl;
                    tot_per_hrs_spl_fals = tot_per_hrs_spl;
                    per_leave_fals = per_leave;
                    tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
                    tot_ondu_spl_fals = tot_ondu_spl;
                    tot_ml_spl_fals = tot_ml_spl;
                }
                else if (check == 2)
                {
                    per_abshrs_spl_true = per_abshrs_spl;
                    tot_per_hrs_spl_true = tot_per_hrs_spl;
                    per_leave_true = per_leave;
                    tot_conduct_hr_spl_true = tot_conduct_hr_spl;
                    tot_ondu_spl_true = tot_ondu_spl;
                    tot_ml_spl_true = tot_ml_spl;
                }

            }
        }
        //  catch
        {
        }
    }
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        string[] splitfromcheck = txtfromdate.Text.Split(new Char[] { '/' });
        string[] splittocheck = txttodate.Text.Split(new char[] { '/' });
        string fdate = splitfromcheck[1] + '/' + splitfromcheck[0] + '/' + splitfromcheck[2];
        string tdate = splittocheck[1] + '/' + splittocheck[0] + '/' + splittocheck[2];
        DateTime fromdatechech = Convert.ToDateTime(fdate);
        DateTime todatecheck = Convert.ToDateTime(tdate);
        if (fromdatechech > todatecheck)
        {
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }

    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        string[] splitfromcheck = txtfromdate.Text.Split(new Char[] { '/' });
        string[] splittocheck = txttodate.Text.Split(new char[] { '/' });
        string fdate = splitfromcheck[1] + '/' + splitfromcheck[0] + '/' + splitfromcheck[2];
        string tdate = splittocheck[1] + '/' + splittocheck[0] + '/' + splittocheck[2];
        DateTime fromdatechech = Convert.ToDateTime(fdate);
        DateTime todatecheck = Convert.ToDateTime(tdate);
        if (fromdatechech > todatecheck)
        {
            errmsg.Text = "Please Enter To Date Grater Than From Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    protected void txtConvert_Value_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        errmsg.Visible = false;
        btnPrint.Visible = false;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        // Session["column_header_row_count"] = Convert.ToString(FpSpread1.ColumnHeader.RowCount);
        string sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        string subjecname = "", testname = "";
        string subcode = "";
        int stv = 0;
        int testcou = 0;
        for (int s = 0; s < chklstsubject.Items.Count; s++)
        {
            if (chklstsubject.Items[s].Selected == true)
            {
                stv++;
                subjecname = chklstsubject.Items[s].Text;
                subcode = d2.GetFunction("select Subject_code from subject where subject_no='" + chklstsubject.Items[s].Value.ToString() + "'");
            }
        }

        for (int s = 0; s < cbltest.Items.Count; s++)
        {
            if (cbltest.Items[s].Selected == true)
            {
                testcou++;
                testname = cbltest.Items[s].Text;
            }
        }
        string strgetconm = txtConvert_Value.Text.ToString();
        string degreedetails = "Student Over All Cam report" + '@' + "Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlsemester.SelectedItem.ToString() + '-' + sections + '@' + "Date :" + txtfromdate.Text.ToString() + " To " + txttodate.Text.ToString();
        if (stv == 1 && testcou == 1)
        {
            degreedetails = "Over All " + testname + " Report" + '@' + "Branch: " + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + "]                                                                                                               Sem : " + ddlsemester.SelectedItem.ToString() + "@Subject Code / Name : " + subcode + " / " + subjecname;
        }
        else if (testcou == 1 && strgetconm.Trim() != "" && strgetconm != null)
        {
            string section = "";
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
            }
            else
            {
                section = "@SECTION : " + ddlsection.SelectedItem.ToString() + "";
            }

            string stergetsem = ddlsemester.SelectedItem.ToString();
            if (stergetsem == "1" || stergetsem == "2")
            {
                stergetsem = "I";
            }
            else if (stergetsem == "3" || stergetsem == "4")
            {
                stergetsem = "II";
            }
            else if (stergetsem == "5" || stergetsem == "6")
            {
                stergetsem = "III";
            }
            else if (stergetsem == "7" || stergetsem == "8")
            {
                stergetsem = "IV";
            }
            else if (stergetsem == "9" || stergetsem == "10")
            {
                stergetsem = "V";
            }
            degreedetails = "DEPARTMENT OF " + ddlbranch.SelectedItem.ToString() + "$CONSOLIDATED MARK SHEET - " + testname + "@" + "YEAR/SEMESTER : " + stergetsem + " / " + ddlsemester.SelectedItem.ToString() + section;
        }
        string ss = Session["usercode"].ToString();
        string pagename = "StudentTestReport.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }
    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Student's Overall CAM Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

}